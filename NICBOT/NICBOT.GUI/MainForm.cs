using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using NICBOT.Controls;
using NICBOT.Utilities;

namespace NICBOT.GUI
{
   public partial class MainForm : Form
   {
      #region Definitions

      private enum CameraSelectModes
      {
         none,
         launchCamera,
         robotCameraA,
         light,
         robotCameraB,
      }

      #endregion

      #region Fields

      private delegate void ProcessHandler();

      private ProcessHandler Process { set; get; }
      private bool processStopNeeded;
      private bool processExitNeeded;
      private bool processStopped;

      private FileTraceListener fileTraceListener;
      private UdpTraceListener traceListener;

      private bool indicatorFlasher;

      private bool feederFastSelected;
      private double feederPreviousRequest;
      private FeederModes feederNonManualMode;

      private bool movementFastSelected;
      private MovementModes movementNonManualMode;
      private MovementForwardModes movementNonManualForwardMode;

      private ToolLocations toolLocation;

      private DrillParameters selectedDrill;
      private bool drillManualVisible;
      private bool drillManualActivated;

      private bool pumpManualVisible;
      private bool pumpManualActivated;

      private bool sensorThicknessPending;
      private bool newThicknessReading;
      private bool sensorStressPending;
      private bool newStressReading;
      private Directions sensorDirection;
      private double thicknessReading;
      private double stressReading;

      private CameraSelectModes cameraSelectMode;
      private CameraSelectButton selectedLaunchCameraButton;
      private CameraSelectButton selectedRobotCameraAButton;
      private CameraSelectButton selectedRobotCameraBButton;

      private CameraSelectButton[] cameraButtons;

      private PopupDimmerForm dimmerForm;

      #endregion
      
      #region Helper Functions

      #region General Helper Functions 

      private string GetValueText(double operationalValue, ValueParameter parameter)
      {
         string doubleFormat = "N" + parameter.Precision.ToString();
         string result = operationalValue.ToString(doubleFormat) + " " + parameter.Unit;
         return (result);
      }

      private string GetValueText(ValueParameter parameter)
      {
         string doubleFormat = "N" + parameter.Precision.ToString();
         string result = parameter.OperationalValue.ToString(doubleFormat) + " " + parameter.Unit;
         return (result);
      }

      private string GetValueText(double value, string doubleFormat, string unit)
      {
         string result = value.ToString(doubleFormat) + " " + unit;
         return (result);
      }

      private string GetValueText(int value, string unit)
      {
         string result = value.ToString() + " " + unit;
         return (result);
      }

      private void SetCautionPanel(double reading, CautionParameter cautionParameter, Control panel)
      {
         Color backColor = Color.Black;
         Color foreColor = Color.White;

         if ((reading > cautionParameter.DangerHighLimit) ||
             (reading < cautionParameter.DangerLowLimit))
         {
            backColor = Color.Red;
         }
         else if ((reading > cautionParameter.WarningHighLimit) ||
                  (reading < cautionParameter.WarningLowLimit))
         {
            backColor = Color.Yellow;
            foreColor = Color.Black;
         }

         panel.BackColor = backColor;
         panel.ForeColor = foreColor;
      }

      private int GetAbsoluteLeft(Control control)
      {
         int result = 0;

         while (null != control)
         {
            result += control.Left;
            control = control.Parent;
         }

         return (result);
      }

      private int GetAbsoluteTop(Control control)
      {
         int result = 0;

         while (null != control)
         {
            result += control.Top;
            control = control.Parent;
         }

         return (result);
      }

      private void SetDialogLocation(Control control, Form form)
      {
         int offsetX = (form.Width - control.Width) / 2;
         int offsetY = (form.Height - control.Height) / 2;
         int formLeft = this.GetAbsoluteLeft(control) - offsetX;
         int formTop = this.GetAbsoluteTop(control) - offsetY;
         int formLeftMaximum = (this.Left + this.Width) - form.Width - 1;
         int formTopMaximum = (this.Top + this.Height) - form.Height - 1;

         if (formLeft < this.Left)
         {
            formLeft = this.Left;
         }
         else if (formLeft > formLeftMaximum)
         {
            formLeft = formLeftMaximum;
         }

         if (formTop < this.Top)
         {
            formTop = this.Top;
         }
         else if (formTop > formTopMaximum)
         {
            formTop = formTopMaximum;
         }

         form.Left = formLeft;
         form.Top = formTop;
      }

      private void DimBackground()
      {
         this.dimmerForm.Top = 0;
         this.dimmerForm.Left = 0;
         this.dimmerForm.Height = this.Height;
         this.dimmerForm.Width = this.Width;
         this.dimmerForm.Show();
      }

      private void LightBackground()
      {
         this.dimmerForm.Hide();
      }
      
      private DialogResult LaunchNumberEdit(Control control, string title, ValueParameter valueParameter)
      {
         NumberEntryForm numberEntryForm = new NumberEntryForm();
         this.SetDialogLocation(control, numberEntryForm);
         
         numberEntryForm.Title = title;
         numberEntryForm.Unit = valueParameter.Unit;
         numberEntryForm.PostDecimalDigitCount = valueParameter.Precision;
         numberEntryForm.PresentValue = valueParameter.OperationalValue;
         numberEntryForm.DefaultValue = valueParameter.DefaultValue;
         numberEntryForm.MinimumValue = valueParameter.MinimumValue;
         numberEntryForm.MaximumValue = valueParameter.MaximumValue;

         this.DimBackground();
         DialogResult result = numberEntryForm.ShowDialog();
         this.LightBackground();

         if (System.Windows.Forms.DialogResult.OK == result)
         {
            valueParameter.OperationalValue = numberEntryForm.EnteredValue;
         }

         return (result);
      }

      private DialogResult LaunchNumberEdit(ref double value, Control control, string title, int postDecimalDigitCount, string unit, double defaultValue, double mimimumValue, double maximumValue)
      {
         NumberEntryForm numberEntryForm = new NumberEntryForm();
         this.SetDialogLocation(control, numberEntryForm);

         numberEntryForm.Title = title;
         numberEntryForm.PostDecimalDigitCount = postDecimalDigitCount;
         numberEntryForm.Unit = unit;
         numberEntryForm.PresentValue = value;
         numberEntryForm.DefaultValue = defaultValue;
         numberEntryForm.MinimumValue = mimimumValue;
         numberEntryForm.MaximumValue = maximumValue;

         this.DimBackground();
         DialogResult result = numberEntryForm.ShowDialog();
         this.LightBackground();
         value = numberEntryForm.EnteredValue;

         return (result);
      }

      private void RestartSystem()
      {
         this.processStopNeeded = true;
      }

      private void SetTraceListenerDestination()
      {
         this.traceListener.SetDestination(ParameterAccessor.Instance.Trace.Address, ParameterAccessor.Instance.Trace.Port);
         Tracer.WriteHigh(TraceGroup.GUI, null, "endpoint set");
      }

      #endregion

      #region Reel Helper Functions

      private void UpdateReelControls()
      {
         ReelModes reelMode = NicBotComm.Instance.GetReelMode();

         switch (reelMode)
         {
            default:
            case ReelModes.off:
            {
               this.ReelOffButton.ForeColor = Color.FromArgb(240, 240, 240);
               this.ReelReverseButton.ForeColor = Color.Black;
               this.ReelLockButton.ForeColor = Color.Black;
               break;
            }
            case ReelModes.reverse:
            {
               this.ReelReverseButton.ForeColor = Color.FromArgb(240, 240, 240);
               this.ReelOffButton.ForeColor = Color.Black;
               this.ReelLockButton.ForeColor = Color.Black;
               break;
            }
            case ReelModes.locked:
            {
               this.ReelLockButton.ForeColor = Color.FromArgb(240, 240, 240);
               this.ReelOffButton.ForeColor = Color.Black;
               this.ReelReverseButton.ForeColor = Color.Black;
               break;
            }
            case ReelModes.manual:
            {
               this.ReelLockButton.ForeColor = Color.Black;
               this.ReelOffButton.ForeColor = Color.Black;
               this.ReelReverseButton.ForeColor = Color.Black;
               break;
            }
         }

         if (ReelModes.reverse != reelMode)
         {
            this.ReelShowManualButton.Enabled = true;
         }
         else
         {
            this.ReelShowManualButton.Enabled = false;
         }
      }

      private void IncrementReelManualValue()
      {
         if (MovementForwardControls.current == ParameterAccessor.Instance.ReelMotionMode)
         {
            double limit = ParameterAccessor.Instance.ReelManualCurrent.MaximumValue - ParameterAccessor.Instance.ReelManualCurrent.StepValue;

            if (ParameterAccessor.Instance.ReelManualCurrent.OperationalValue <= limit)
            {
               ParameterAccessor.Instance.ReelManualCurrent.OperationalValue += ParameterAccessor.Instance.ReelManualCurrent.StepValue;

               this.ReelManualValueTextPanel.ValueText = this.GetValueText(ParameterAccessor.Instance.ReelManualCurrent);

               double directionalModifier = (false != this.ReelManualDirectionToggleButton.OptionASelected) ? 1 : -1;
               double manualCurrent = ParameterAccessor.Instance.ReelManualCurrent.OperationalValue * directionalModifier;
               NicBotComm.Instance.SetReelManualCurrent(manualCurrent);
            }
         }
         else
         {
            double limit = ParameterAccessor.Instance.ReelManualSpeed.MaximumValue - ParameterAccessor.Instance.ReelManualSpeed.StepValue;

            if (ParameterAccessor.Instance.ReelManualSpeed.OperationalValue <= limit)
            {
               ParameterAccessor.Instance.ReelManualSpeed.OperationalValue += ParameterAccessor.Instance.ReelManualSpeed.StepValue;

               this.ReelManualValueTextPanel.ValueText = this.GetValueText(ParameterAccessor.Instance.ReelManualSpeed);

               double directionalModifier = (false != this.ReelManualDirectionToggleButton.OptionASelected) ? 1 : -1;
               double manualSpeed = ParameterAccessor.Instance.ReelManualSpeed.OperationalValue * directionalModifier;
               NicBotComm.Instance.SetReelManualSpeed(manualSpeed);
            }
         }
      }

      private void DecrementReelManualValue()
      {
         if (MovementForwardControls.current == ParameterAccessor.Instance.ReelMotionMode)
         {
            double limit = ParameterAccessor.Instance.ReelManualCurrent.MinimumValue + ParameterAccessor.Instance.ReelManualCurrent.StepValue;

            if (ParameterAccessor.Instance.ReelManualCurrent.OperationalValue >= limit)
            {
               ParameterAccessor.Instance.ReelManualCurrent.OperationalValue -= ParameterAccessor.Instance.ReelManualCurrent.StepValue;

               this.ReelManualValueTextPanel.ValueText = this.GetValueText(ParameterAccessor.Instance.ReelManualCurrent);

               double directionalModifier = (false != this.ReelManualDirectionToggleButton.OptionASelected) ? 1 : -1;
               double manualCurrent = ParameterAccessor.Instance.ReelManualCurrent.OperationalValue * directionalModifier;
               NicBotComm.Instance.SetReelManualCurrent(manualCurrent);
            }
         }
         else
         {
            double limit = ParameterAccessor.Instance.ReelManualSpeed.MinimumValue + ParameterAccessor.Instance.ReelManualSpeed.StepValue;

            if (ParameterAccessor.Instance.ReelManualSpeed.OperationalValue >= limit)
            {
               ParameterAccessor.Instance.ReelManualSpeed.OperationalValue -= ParameterAccessor.Instance.ReelManualSpeed.StepValue;

               this.ReelManualValueTextPanel.ValueText = this.GetValueText(ParameterAccessor.Instance.ReelManualSpeed);

               double directionalModifier = (false != this.ReelManualDirectionToggleButton.OptionASelected) ? 1 : -1;
               double manualSpeed = ParameterAccessor.Instance.ReelManualSpeed.OperationalValue * directionalModifier;
               NicBotComm.Instance.SetReelManualSpeed(manualSpeed);
            }
         }
      }

      #endregion

      #region Feeder Helper Functions

      private bool GetAutomaticFeederTracking()
      {
         bool result = true;

         MovementWheelModes movementWheelMode = NicBotComm.Instance.GetMovementWheelMode();
         MovementForwardModes movementForwardMode = NicBotComm.Instance.GetMovementForwardMode();
         MovementForwardControls movementForwardControl = NicBotComm.Instance.GetMovementForwardControl();
         
         bool manualControl = this.FeederManualPanel.Visible;
         bool modeMovementEnabled = (MovementWheelModes.neither != movementWheelMode) && (MovementWheelModes.both != movementWheelMode) && (MovementForwardModes.circumferential != movementForwardMode);

         result = result && (false == this.FeederManualPanel.Visible);
         result = result && (false != modeMovementEnabled);
         result = result && (MovementForwardControls.velocity == movementForwardControl);
         result = result && (false != ParameterAccessor.Instance.FeederAutomaticTracking);

         return (result);
      }

      private void UpdateFeederControls()
      {
         MovementWheelModes movementWheelMode = NicBotComm.Instance.GetMovementWheelMode();
         MovementForwardModes movementForwardMode = NicBotComm.Instance.GetMovementForwardMode();
         MovementForwardControls movementForwardControl = NicBotComm.Instance.GetMovementForwardControl();
         FeederModes feederMode = NicBotComm.Instance.GetFeederMode();

         bool feederAutomaticTracking = this.GetAutomaticFeederTracking();
         bool manualControl = this.FeederManualPanel.Visible;
         bool modeMovementEnabled = (MovementWheelModes.neither != movementWheelMode) && (MovementWheelModes.both != movementWheelMode) && (MovementForwardModes.circumferential != movementForwardMode);

         if ((false == manualControl) && (false == modeMovementEnabled))
         {
            this.FeederTitleLabel.Text = "TETHER FEEDER - NO MOTION";
            this.FeederMoveButton.Enabled = false;

            if (FeederModes.move == feederMode)
            {
               NicBotComm.Instance.SetFeederMode(FeederModes.off);
               feederMode = NicBotComm.Instance.GetFeederMode();
            }
         }
         else if ((false == manualControl) && (false != feederAutomaticTracking))
         {
            this.FeederTitleLabel.Text = "TETHER FEEDER - TRACKING";
            this.FeederMoveButton.Enabled = true;
         }
         else
         {
            this.FeederTitleLabel.Text = "TETHER FEEDER";
            this.FeederMoveButton.Enabled = true;
         }

         switch (feederMode)
         {
            default:

            case FeederModes.off:
            {
               this.FeederOffButton.ForeColor = Color.FromArgb(240, 240, 240);
               this.FeederMoveButton.ForeColor = Color.Black;
               this.FeederLockButton.ForeColor = Color.Black;

               break;
            }
            case FeederModes.move:
            {
               this.FeederMoveButton.ForeColor = Color.FromArgb(240, 240, 240);
               this.FeederOffButton.ForeColor = Color.Black;
               this.FeederLockButton.ForeColor = Color.Black;

               break;
            }
            case FeederModes.locked:
            {
               this.FeederLockButton.ForeColor = Color.FromArgb(240, 240, 240);
               this.FeederOffButton.ForeColor = Color.Black;
               this.FeederMoveButton.ForeColor = Color.Black;


               break;
            }
         }

         if (false != this.FeederManualPanel.Visible)
         {
            this.FeederManulDisplayButton.Enabled = true;
         }
         else
         {
            if (feederMode != FeederModes.move)
            {
               this.FeederManulDisplayButton.Enabled = true;
            }
            else
            {
               this.FeederManulDisplayButton.Enabled = false;
            }
         }

         if ((false != this.FeederManualPanel.Visible) || (false != feederAutomaticTracking))
         {
            this.FeederSpeedToggleButton.Visible = false;
         }
         else
         {
            this.FeederSpeedToggleButton.Visible = true;
         }
      }

      #endregion

      #region Robot Helper Functions

      private void UpdatePipePositionDisplay()
      {
         int robotPitch = (int)NicBotComm.Instance.GetRobotPitch();
         int robotRoll = (int)NicBotComm.Instance.GetRobotRoll();
         this.RobotCrossSectionView.RobotRoll = robotRoll - 90;
         this.RobotCrossSectionView.RobotPitch = robotPitch;

         int robotAngle = 360 - ((robotRoll - 90) % 360);
         int hour = robotAngle / 30;
         int minute = (robotAngle % 30) * 2;

         if (0 == hour)
         {
            hour = 12;
         }
         else if (hour > 12)
         {
            hour -= 12;
         }

         string pipePositionText = string.Format("{0:D2}:{1:D2}", hour, minute);
         this.DrillPipePositionLabel.Text = pipePositionText;
         this.SealantPipePositionLabel.Text = pipePositionText;
         this.SensorPipePositionTextPanel.ValueText = pipePositionText;
         VideoStampOsd.Instance.SetPipePositionText(pipePositionText);
      }

      private bool GetBodyChangeAllowed()
      {
         bool result = true;

         if ((NicBotComm.Instance.GetDrillAutoState() != DrillAutoStates.off) ||
             (false != this.drillManualActivated) ||
             (NicBotComm.Instance.GetPumpAutoState(this.toolLocation) != PumpAutoStates.off) ||
             (false != this.pumpManualActivated))
         {
            result = false;
         }

         return(result);
      }
      
      private void UpdateBodyControls()
      {
         BodyPositions bodyPosition = NicBotComm.Instance.GetBodyPosition();

         switch (bodyPosition)
         {
            case BodyPositions.closed:
            {
               this.BodyClosedButton.ForeColor = Color.FromArgb(240, 240, 240);
               this.BodyOpenButton.ForeColor = Color.Black;
               this.BodyDrillButton.ForeColor = Color.Black;
               this.BodyRearReleaseButton.ForeColor = Color.Black;
               this.BodyFrontReleaseButton.ForeColor = Color.Black;
               this.CustomSetupButton.ForeColor = Color.Black;
               break;
            }
            case BodyPositions.opened:
            {
               this.BodyOpenButton.ForeColor = Color.FromArgb(240, 240, 240);
               this.BodyClosedButton.ForeColor = Color.Black;
               this.BodyDrillButton.ForeColor = Color.Black;
               this.BodyRearReleaseButton.ForeColor = Color.Black;
               this.BodyFrontReleaseButton.ForeColor = Color.Black;
               this.CustomSetupButton.ForeColor = Color.Black;
               break;
            }
            case BodyPositions.drill:
            {
               this.BodyDrillButton.ForeColor = Color.FromArgb(240, 240, 240);
               this.BodyClosedButton.ForeColor = Color.Black;
               this.BodyOpenButton.ForeColor = Color.Black;
               this.BodyRearReleaseButton.ForeColor = Color.Black;
               this.BodyFrontReleaseButton.ForeColor = Color.Black;
               this.CustomSetupButton.ForeColor = Color.Black;
               break;
            }
            case BodyPositions.rearLoose:
            {
               this.BodyRearReleaseButton.ForeColor = Color.FromArgb(240, 240, 240);
               this.BodyClosedButton.ForeColor = Color.Black;
               this.BodyOpenButton.ForeColor = Color.Black;
               this.BodyDrillButton.ForeColor = Color.Black;
               this.BodyFrontReleaseButton.ForeColor = Color.Black;
               this.CustomSetupButton.ForeColor = Color.Black;
               break;
            }
            case BodyPositions.frontLoose:
            {
               this.BodyFrontReleaseButton.ForeColor = Color.FromArgb(240, 240, 240);
               this.BodyClosedButton.ForeColor = Color.Black;
               this.BodyOpenButton.ForeColor = Color.Black;
               this.BodyDrillButton.ForeColor = Color.Black;
               this.BodyRearReleaseButton.ForeColor = Color.Black;
               this.CustomSetupButton.ForeColor = Color.Black;
               break;
            }
            default:
            case BodyPositions.manual:
            {
               this.BodyClosedButton.ForeColor = Color.Black;
               this.BodyOpenButton.ForeColor = Color.Black;
               this.BodyDrillButton.ForeColor = Color.Black;
               this.BodyRearReleaseButton.ForeColor = Color.Black;
               this.BodyFrontReleaseButton.ForeColor = Color.Black;
               this.CustomSetupButton.ForeColor = Color.FromArgb(240, 240, 240);
               break;
            }
         }

         bool bodyChangeAllowed = this.GetBodyChangeAllowed();
         Color enabledIndicator = (false != bodyChangeAllowed) ? Color.FromArgb(171, 171, 171) : Color.FromArgb(151, 151, 151);

         this.BodyClosedButton.BackColor = enabledIndicator;
         this.BodyOpenButton.BackColor = enabledIndicator;
         this.BodyDrillButton.BackColor = enabledIndicator;
         this.BodyRearReleaseButton.BackColor = enabledIndicator;
         this.BodyFrontReleaseButton.BackColor = enabledIndicator;
         this.CustomSetupButton.BackColor = enabledIndicator;

         this.BotSideView.Position = bodyPosition;
      }

      private void UpdateMovementControls()
      {
         MovementWheelModes movementWheelMode = NicBotComm.Instance.GetMovementWheelMode();
         MovementForwardModes movementForwardModes = NicBotComm.Instance.GetMovementForwardMode();
         MovementModes movementMode = NicBotComm.Instance.GetMovementMode();

         if ((MovementWheelModes.neither == movementWheelMode) || (MovementWheelModes.both == movementWheelMode))
         {
            this.MovementOffButton.Enabled = false;
            this.MovementMoveButton.Enabled = false;
            this.MovementLockButton.Enabled = false;

            this.MovementAxialToggleButton.OptionASelected = (MovementWheelModes.both == movementWheelMode);
            this.MovementAxialToggleButton.OptionBSelected = (MovementWheelModes.both == movementWheelMode);
            this.MovementLaunchModeToggleButton.OptionASelected = false;
            this.MovementCornerModeToggleButton.OptionASelected = false;

            this.MotorTitleLabel.Text = "WHEEL MOTORS - NO SELECTION";
            this.MotorTitleLabel.Font = new Font(this.MotorTitleLabel.Font.Name, 12.75f, this.MotorTitleLabel.Font.Style);

            this.MovementSpeedToggleButton.Visible = true;
            this.MovementManulPanel.Visible = false;
            this.MovementManaulDisplayButton.Enabled = false;
            this.MovementManaulDisplayButton.Text = "SHOW MANUAL";
         }
         else
         {
            this.MovementOffButton.Enabled = true;
            this.MovementMoveButton.Enabled = true;
            this.MovementLockButton.Enabled = true;

            if (MovementForwardModes.circumferential == movementForwardModes)
            {
               this.MovementAxialToggleButton.OptionASelected = false;
               this.MovementAxialToggleButton.OptionBSelected = true;
               this.MovementLaunchModeToggleButton.OptionASelected = false;
               this.MovementCornerModeToggleButton.OptionASelected = false;

               this.MotorTitleLabel.Text = "WHEEL MOTORS - CIRCUMFERENTIAL MOTION";
               this.MotorTitleLabel.Font = new Font(this.MotorTitleLabel.Font.Name, 11.25f, this.MotorTitleLabel.Font.Style);

               this.MotorManualJogReverseButton.Text = "JOG          CCW";
               this.MotorManualJogForwardButton.Text = "JOG          CW";
               this.MotorManualMoveReverseButton.Text = "MANUAL MOVE     CCW";
               this.MotorManualMoveForwardButton.Text = "MANUAL MOVE        CW";

               this.RobotCrossSectionView.Axial = false;
            }
            else
            {
               this.MovementAxialToggleButton.OptionASelected = true;
               this.MovementAxialToggleButton.OptionBSelected = false;

               this.MotorTitleLabel.Text = "WHEEL MOTORS - AXIAL MOTION";
               this.MotorTitleLabel.Font = new Font(this.MotorTitleLabel.Font.Name, 12.75f, this.MotorTitleLabel.Font.Style);

               this.MotorManualJogReverseButton.Text = "JOG REVERSE";
               this.MotorManualJogForwardButton.Text = "JOG FORWARD";
               this.MotorManualMoveReverseButton.Text = "MANUAL MOVE REVERSE";
               this.MotorManualMoveForwardButton.Text = "MANUAL MOVE FORWARD";

               if (MovementForwardModes.launchAxial == movementForwardModes)
               {
                  this.MovementLaunchModeToggleButton.OptionASelected = true;
                  this.MovementCornerModeToggleButton.OptionASelected = false;
               }
               else if (MovementForwardModes.cornerAxial == movementForwardModes)
               {
                  this.MovementLaunchModeToggleButton.OptionASelected = false;
                  this.MovementCornerModeToggleButton.OptionASelected = true;
               }
               else if (MovementForwardModes.normalAxial == movementForwardModes)
               {
                  this.MovementLaunchModeToggleButton.OptionASelected = false;
                  this.MovementCornerModeToggleButton.OptionASelected = false;
               }

               this.RobotCrossSectionView.Axial = true;
            }
         }

         FeederModes neededTrackingFeederMode = FeederModes.off;

         if (MovementModes.off == movementMode)
         {
            this.MovementOffButton.ForeColor = Color.FromArgb(240, 240, 240);
            this.MovementMoveButton.ForeColor = Color.Black;
            this.MovementLockButton.ForeColor = Color.Black;

            this.MovementAxialToggleButton.Enabled = true;
            this.MovementLaunchModeToggleButton.Enabled = true;
            this.MovementCornerModeToggleButton.Enabled = true;

            this.MovementManaulDisplayButton.Enabled = this.MovementOffButton.Enabled;

            neededTrackingFeederMode = FeederModes.off;
         }
         else if (MovementModes.move == movementMode)
         {
            this.MovementMoveButton.ForeColor = Color.FromArgb(240, 240, 240);
            this.MovementOffButton.ForeColor = Color.Black;
            this.MovementLockButton.ForeColor = Color.Black;

            this.MovementAxialToggleButton.Enabled = false;
            this.MovementLaunchModeToggleButton.Enabled = false;
            this.MovementCornerModeToggleButton.Enabled = false;

            this.MovementManaulDisplayButton.Enabled = this.MovementManulPanel.Visible; // need to enable button when manual is selected
         
            neededTrackingFeederMode = FeederModes.move;
         }
         else if (MovementModes.locked == movementMode)
         {
            this.MovementLockButton.ForeColor = Color.FromArgb(240, 240, 240);
            this.MovementOffButton.ForeColor = Color.Black;
            this.MovementMoveButton.ForeColor = Color.Black;

            this.MovementAxialToggleButton.Enabled = true;
            this.MovementLaunchModeToggleButton.Enabled = true;
            this.MovementCornerModeToggleButton.Enabled = true;

            this.MovementManaulDisplayButton.Enabled = this.MovementOffButton.Enabled;
         
            neededTrackingFeederMode = FeederModes.locked;
         }

         if ((this.GetAutomaticFeederTracking() != false) && (NicBotComm.Instance.GetFeederMode() != neededTrackingFeederMode))
         {
            NicBotComm.Instance.SetFeederMode(neededTrackingFeederMode);
         }
         
         this.UpdateFeederControls();
      }

      #endregion

      #region Drill Helper Functions

      private void SetDrillSelection(ToolLocations location)
      {
         if (ToolLocations.front == location)
         {
            this.selectedDrill = ParameterAccessor.Instance.FrontDrill;
         }
         else
         {
            this.selectedDrill = ParameterAccessor.Instance.RearDrill;
         }

         this.DrillSelectionLabel.Text = (ToolLocations.front == location) ? "FRONT DRILL SELECTED" : "REAR DRILL SELECTED";

         if (false != this.selectedDrill.PeckMode)
         {
            this.DrillModeLabel.Text = "PECK";
            this.DrillModeLabel.Font = new Font(this.DrillModeLabel.Font.Name, 12f, this.DrillModeLabel.Font.Style);
         }
         else
         {
            this.DrillModeLabel.Text = "CONTINUOUS";
            this.DrillModeLabel.Font = new Font(this.DrillModeLabel.Font.Name, 8.25f, this.DrillModeLabel.Font.Style);
         }

         this.DrillLaserLightButton.OptionASelected = NicBotComm.Instance.GetLaserSight(location);

         //this.DrillExtendedSetPointValuePanel.ValueText = this.GetValueText(this.selectedDrill.ExtendedDistance);
         this.DrillRotaionSetPointSpeedValuePanel.ValueText = this.GetValueText(this.selectedDrill.RotationSpeed);
      }

      private void UpdateDrillControls()
      {
         DrillAutoStates drillAutoState = NicBotComm.Instance.GetDrillAutoState();

         if (DrillAutoStates.off == drillAutoState)
         {
            bool autoPumpStartEnabled = (NicBotComm.Instance.GetBodyPosition() == BodyPositions.drill);

            this.DrillAutoStartButton.Enabled = autoPumpStartEnabled;
            this.DrillAutoPauseResumeButton.Enabled = false;
            this.DrillAutoStopButton.Enabled = false;
         }
         else if (DrillAutoStates.running == drillAutoState)
         {
            this.DrillAutoStartButton.Enabled = false;
            this.DrillAutoPauseResumeButton.Enabled = true;
            this.DrillAutoStopButton.Enabled = true;

            this.DrillAutoPauseResumeButton.Text = "PAUSE";
         }
         else if (DrillAutoStates.paused == drillAutoState)
         {
            this.DrillAutoStartButton.Enabled = false;
            this.DrillAutoPauseResumeButton.Enabled = true;
            this.DrillAutoStopButton.Enabled = true;

            this.DrillAutoPauseResumeButton.Text = "RESUME";
         }

         this.DrillManualToggleButton.OptionASelected = this.drillManualActivated;
      }

      private void IncrementDrillIndex()
      {
         double extendedDistance = this.selectedDrill.ExtendedDistance.OperationalValue;

         if (extendedDistance < this.selectedDrill.ExtendedDistance.MaximumValue)
         {
            extendedDistance += this.selectedDrill.ExtendedDistance.StepValue;
         }

         NicBotComm.Instance.SetDrillIndexSetPoint(this.toolLocation, extendedDistance);
         this.selectedDrill.ExtendedDistance.OperationalValue = extendedDistance;
         //this.DrillExtendedSetPointValuePanel.ValueText = this.GetValueText(this.selectedDrill.ExtendedDistance);
      }

      private void DecrementDrillIndex()
      {
         double extendedDistance = this.selectedDrill.ExtendedDistance.OperationalValue;

         if (extendedDistance > this.selectedDrill.ExtendedDistance.MinimumValue)
         {
            extendedDistance -= this.selectedDrill.ExtendedDistance.StepValue;
         }

         NicBotComm.Instance.SetDrillIndexSetPoint(this.toolLocation, extendedDistance);
         this.selectedDrill.ExtendedDistance.OperationalValue = extendedDistance;
         //this.DrillExtendedSetPointValuePanel.ValueText = this.GetValueText(this.selectedDrill.ExtendedDistance);
      }

      private void IncrementDrillSpeed()
      {
         double drillSpeed = this.selectedDrill.RotationSpeed.OperationalValue;

         if (drillSpeed < this.selectedDrill.RotationSpeed.MaximumValue)
         {
            drillSpeed += this.selectedDrill.RotationSpeed.StepValue;
         }

         if (false != this.drillManualActivated)
         {
            NicBotComm.Instance.SetDrillRotationSpeed(this.toolLocation, drillSpeed);
         }

         this.selectedDrill.RotationSpeed.OperationalValue = drillSpeed;
         this.DrillRotaionSetPointSpeedValuePanel.ValueText = this.GetValueText(this.selectedDrill.RotationSpeed);
      }

      private void DecrementDrillSpeed()
      {
         double drillSpeed = this.selectedDrill.RotationSpeed.OperationalValue;

         if (drillSpeed > this.selectedDrill.RotationSpeed.MinimumValue)
         {
            drillSpeed -= this.selectedDrill.RotationSpeed.StepValue;
         }

         if (false != this.drillManualActivated)
         {
            NicBotComm.Instance.SetDrillRotationSpeed(this.toolLocation, drillSpeed);
         }

         this.selectedDrill.RotationSpeed.OperationalValue = drillSpeed;
         this.DrillRotaionSetPointSpeedValuePanel.ValueText = this.GetValueText(this.selectedDrill.RotationSpeed);
      }

      #endregion

      #region Sealant Helper Functions

      private void UpdateSealantControls()
      {
         this.NozzleSelectionLabel.Text = (ToolLocations.front == this.toolLocation) ? "FRONT NOZZLE SELECTED" : "REAR NOZZLE SELECTED";
         ValueParameter relievePressure = (ToolLocations.front == this.toolLocation) ? ParameterAccessor.Instance.FrontPump.RelievedPressure : ParameterAccessor.Instance.RearPump.RelievedPressure;
         this.SealantRelievePressureButton.ValueText = this.GetValueText(relievePressure);

         bool nozzleExtended = NicBotComm.Instance.GetNozzleExtended(this.toolLocation);
         bool drillRetracted = (NicBotComm.Instance.GetDrillIndexPosition(this.toolLocation) < 2) ? true : false;

         PumpAutoStates pumpAutoState = NicBotComm.Instance.GetPumpAutoState(this.toolLocation);
         PumpModes pumpMode = NicBotComm.Instance.GetPumpMode(this.toolLocation);

         if ((PumpAutoStates.off != pumpAutoState) || (false != nozzleExtended))
         {
            this.SealDrillModeButton.Enabled = false;
         }
         else
         {
            this.SealDrillModeButton.Enabled = true;
         }

         this.SealantLaserLightButton.OptionASelected = NicBotComm.Instance.GetLaserSight(this.toolLocation);

         if (PumpAutoStates.off == pumpAutoState)
         {
            bool autoPumpStartEnabled = ((NicBotComm.Instance.GetBodyPosition() == BodyPositions.drill) && (false == this.pumpManualActivated));

            this.SealantAutoStartButton.Enabled = autoPumpStartEnabled;
            this.SealantAutoPauseResumeButton.Enabled = false;
            this.SealantAutoStopButton.Enabled = false;
         }
         else if (PumpAutoStates.running == pumpAutoState)
         {
            this.SealantAutoStartButton.Enabled = false;
            this.SealantAutoPauseResumeButton.Enabled = true;
            this.SealantAutoStopButton.Enabled = true;

            this.SealantAutoPauseResumeButton.Text = "PAUSE";
         }
         else if (PumpAutoStates.paused == pumpAutoState)
         {
            this.SealantAutoStartButton.Enabled = false;
            this.SealantAutoPauseResumeButton.Enabled = true;
            this.SealantAutoStopButton.Enabled = true;

            this.SealantAutoPauseResumeButton.Text = "RESUME";
         }

         if (PumpAutoStates.running == pumpAutoState)
         {
            this.SealantNozzleToggleButton.Enabled = false;
            this.SealantRelievePressureButton.Enabled = false;
            this.SealantManualPumpToggleButton.Enabled = false;

            this.pumpManualActivated = false;
         }
         else
         {
            this.SealantNozzleToggleButton.Enabled = drillRetracted;
            this.SealantRelievePressureButton.Enabled = true;
            this.SealantManualPumpToggleButton.Enabled = true;
         }

         this.SealantManualPumpToggleButton.OptionASelected = this.pumpManualActivated;

         if (false != this.pumpManualActivated)
         {
            this.SealantManulDisplayButton.Enabled = false;
            this.SealantManualModeToggleButton.Enabled = false;
            this.SealantDirectionToggleButton.Enabled = false;

            if (false != this.SealantManualModeToggleButton.OptionASelected)
            {
               this.SealantPressureSetPointValuePanel.Enabled = true;
               this.SealantPressureSetPointValuePanel.HoldTimeoutEnable = true;

               this.SealantPressureIncreaseButton.Enabled = true;
               this.SealantPressureDecreaseButton.Enabled = true;
               this.SealantSpeedIncreaseButton.Enabled = false;
               this.SealantSpeedDecreaseButton.Enabled = false;
            }
            else 
            {
               this.SealantSpeedSetPointValuePanel.Enabled = true;
               this.SealantSpeedSetPointValuePanel.HoldTimeoutEnable = true;

               this.SealantPressureIncreaseButton.Enabled = false;
               this.SealantPressureDecreaseButton.Enabled = false;
               this.SealantSpeedIncreaseButton.Enabled = true;
               this.SealantSpeedDecreaseButton.Enabled = true;
            }
         }
         else
         {
            this.SealantManulDisplayButton.Enabled = true;
            this.SealantPressureSetPointValuePanel.Enabled = false;
            this.SealantPressureSetPointValuePanel.HoldTimeoutEnable = false;

            this.SealantSpeedSetPointValuePanel.Enabled = false;
            this.SealantSpeedSetPointValuePanel.HoldTimeoutEnable = false;

            this.SealantManualModeToggleButton.Enabled = true;
            this.SealantDirectionToggleButton.Enabled = true;

            this.SealantPressureIncreaseButton.Enabled = false;
            this.SealantPressureDecreaseButton.Enabled = false;
            this.SealantSpeedIncreaseButton.Enabled = false;
            this.SealantSpeedDecreaseButton.Enabled = false;
         }

         bool pressureAuroFill = (ToolLocations.front == this.toolLocation) ? ParameterAccessor.Instance.FrontPump.PressureAutoFill : ParameterAccessor.Instance.RearPump.PressureAutoFill;
         
         if (false != pressureAuroFill)
         {
            this.SealantModeLabel.Text = "PRESSURE";
            this.SealantModeLabel.Font = new Font(this.SealantModeLabel.Font.Name, 11.25f, this.SealantModeLabel.Font.Style);
         }
         else
         {
            this.SealantModeLabel.Text = "VOLUME";
            this.SealantModeLabel.Font = new Font(this.SealantModeLabel.Font.Name, 12f, this.SealantModeLabel.Font.Style);
         }
      }

      private void IncreaseSealantPressure()
      {
         double maximumValue = (ToolLocations.front == this.toolLocation) ? ParameterAccessor.Instance.FrontPump.MaximumPressure.OperationalValue : ParameterAccessor.Instance.RearPump.MaximumPressure.OperationalValue;
         double stepValue = (ToolLocations.front == this.toolLocation) ? ParameterAccessor.Instance.FrontPump.MaximumPressure.StepValue : ParameterAccessor.Instance.RearPump.MaximumPressure.StepValue;
         double setPoint = NicBotComm.Instance.GetPumpSetPoint(this.toolLocation);
         double adjustLimit = maximumValue - stepValue;

         if (setPoint <= adjustLimit)
         {
            setPoint += stepValue;
            NicBotComm.Instance.SetPumpPressure(this.toolLocation, setPoint);
         }
      }

      private void DecreaseSealantPressure()
      {
         double stepValue = (ToolLocations.front == this.toolLocation) ? ParameterAccessor.Instance.FrontPump.MaximumPressure.StepValue : ParameterAccessor.Instance.RearPump.MaximumPressure.StepValue;
         double setPoint = NicBotComm.Instance.GetPumpSetPoint(this.toolLocation);
         double adjustLimit = 0 + stepValue;

         if (setPoint >= adjustLimit)
         {
            setPoint -= stepValue;
            NicBotComm.Instance.SetPumpPressure(this.toolLocation, setPoint);
         }
      }
      
      private void IncreaseSealantPumpSpeed()
      {
         double maximumValue = (ToolLocations.front == this.toolLocation) ? ParameterAccessor.Instance.FrontPump.MaximumSpeed.OperationalValue : ParameterAccessor.Instance.RearPump.MaximumSpeed.OperationalValue;
         double stepValue = (ToolLocations.front == this.toolLocation) ? ParameterAccessor.Instance.FrontPump.MaximumSpeed.StepValue : ParameterAccessor.Instance.RearPump.MaximumSpeed.StepValue;
         double setPoint = NicBotComm.Instance.GetPumpSetPoint(this.toolLocation);
         double adjustLimit = maximumValue - stepValue;

         if (setPoint <= adjustLimit)
         {
            setPoint += stepValue;
            NicBotComm.Instance.SetPumpSpeed(this.toolLocation, setPoint);
         }
      }

      private void DecreaseSealantPumpSpeed()
      {
         double stepValue = (ToolLocations.front == this.toolLocation) ? ParameterAccessor.Instance.FrontPump.MaximumPressure.StepValue : ParameterAccessor.Instance.RearPump.MaximumPressure.StepValue;
         double setPoint = NicBotComm.Instance.GetPumpSetPoint(this.toolLocation);
         double adjustLimit = 0 + stepValue;

         if (setPoint >= adjustLimit)
         {
            setPoint -= stepValue;
            NicBotComm.Instance.SetPumpSpeed(this.toolLocation, setPoint);
         }
      }

      #endregion

      #region Video Helper Functions

      private void ClearCameraSelects(CameraSelectButton button)
      {
         button.LeftVisible = false;
         button.CenterVisible = false;
         button.RightVisible = false;
      }

      private void UpdateCameraHoldEnable()
      {
         for (int i = 0; i < this.cameraButtons.Length; i++)
         {
            bool holdEnabled = (CameraSelectModes.light == this.cameraSelectMode) ? true : false;
            this.cameraButtons[i].HoldTimeoutEnable = holdEnabled;
         }
      }

      private void UpdateCameraSelectorColor()
      {
         if (CameraSelectModes.launchCamera == this.cameraSelectMode)
         {
            this.LaunchCameraSelectButton.ForeColor = Color.FromArgb(240, 240, 240);
            this.RobotCameraASelectButton.ForeColor = Color.Black;
            this.LightSelectButton.ForeColor = Color.Black;
            this.RobotCameraBSelectButton.ForeColor = Color.Black;
         }
         else if (CameraSelectModes.robotCameraA == this.cameraSelectMode)
         {
            this.LaunchCameraSelectButton.ForeColor = Color.Black;
            this.RobotCameraASelectButton.ForeColor = Color.FromArgb(240, 240, 240);
            this.LightSelectButton.ForeColor = Color.Black;
            this.RobotCameraBSelectButton.ForeColor = Color.Black;
         }
         else if (CameraSelectModes.light == this.cameraSelectMode)
         {
            this.LaunchCameraSelectButton.ForeColor = Color.Black;
            this.RobotCameraASelectButton.ForeColor = Color.Black;
            this.LightSelectButton.ForeColor = Color.FromArgb(240, 240, 240);
            this.RobotCameraBSelectButton.ForeColor = Color.Black;
         }
         else if (CameraSelectModes.robotCameraB == this.cameraSelectMode)
         {
            this.LaunchCameraSelectButton.ForeColor = Color.Black;
            this.RobotCameraASelectButton.ForeColor = Color.Black;
            this.LightSelectButton.ForeColor = Color.Black;
            this.RobotCameraBSelectButton.ForeColor = Color.FromArgb(240, 240, 240);
         }
         else
         {
            this.LaunchCameraSelectButton.ForeColor = Color.Black;
            this.RobotCameraASelectButton.ForeColor = Color.Black;
            this.LightSelectButton.ForeColor = Color.Black;
            this.RobotCameraBSelectButton.ForeColor = Color.Black;
         }
      }

      private void AssignLaunchCamera(CameraSelectButton selected)
      {
         CameraLocations previousCamera = NicBotComm.Instance.GetLaunchCamera();

         if (selected != this.selectedLaunchCameraButton)
         {
            if (null != this.selectedLaunchCameraButton)
            {
               this.selectedLaunchCameraButton.LeftVisible = false;

               if ((false == this.selectedLaunchCameraButton.RightVisible) && (false == this.selectedLaunchCameraButton.CenterVisibleIndependent))
               {
                  NicBotComm.Instance.SetCameraLightLevel(previousCamera, 0);
                  this.selectedLaunchCameraButton.CenterVisible = false;
               }
            }

            NicBotComm.Instance.SetLaunchCamera(selected.Camera);
            NicBotComm.Instance.SetCameraLightLevel(selected.Camera, selected.CenterLevel);
            VideoStampOsd.Instance.SetCameraIdText(3, selected.Text);

            selected.LeftVisible = true;
            selected.CenterVisible = true;
            this.selectedLaunchCameraButton = selected;
         }
      }

      private void AssignRobotCameraA(CameraSelectButton selected)
      {
         CameraLocations previousCamera = NicBotComm.Instance.GetRobotCameraA();

         if (selected != this.selectedRobotCameraAButton)
         {
            if (null != this.selectedRobotCameraAButton)
            {
               this.selectedRobotCameraAButton.LeftVisible = false;

               if ((false == this.selectedRobotCameraAButton.RightVisible) && (false == this.selectedRobotCameraAButton.CenterVisibleIndependent))
               {
                  NicBotComm.Instance.SetCameraLightLevel(previousCamera, 0);
                  this.selectedRobotCameraAButton.CenterVisible = false;
               }
            }

            Tracer.WriteHigh(TraceGroup.GUI, "", "robot camera A {0}", selected.Camera.ToString()); 
            NicBotComm.Instance.SetRobotCameraA(selected.Camera);
            NicBotComm.Instance.SetCameraLightLevel(selected.Camera, selected.CenterLevel);
            VideoStampOsd.Instance.SetCameraIdText(1, selected.Text);

            selected.LeftVisible = true;
            selected.CenterVisible = true;
            this.selectedRobotCameraAButton = selected;
         }
      }

      private void AssignRobotCameraB(CameraSelectButton selected)
      {
         CameraLocations previousCamera = NicBotComm.Instance.GetRobotCameraB();

         if (selected != this.selectedRobotCameraBButton)
         {
            if (null != this.selectedRobotCameraBButton)
            {
               this.selectedRobotCameraBButton.RightVisible = false;

               if ((false == this.selectedRobotCameraBButton.LeftVisible) && (false == this.selectedRobotCameraBButton.CenterVisibleIndependent))
               {
                  NicBotComm.Instance.SetCameraLightLevel(previousCamera, 0);
                  this.selectedRobotCameraBButton.CenterVisible = false;
               }
            }

            Tracer.WriteHigh(TraceGroup.GUI, "", "robot camera B {0}", selected.Camera.ToString());
            NicBotComm.Instance.SetRobotCameraB(selected.Camera);
            NicBotComm.Instance.SetCameraLightLevel(selected.Camera, selected.CenterLevel);
            VideoStampOsd.Instance.SetCameraIdText(2, selected.Text);

            selected.RightVisible = true;
            selected.CenterVisible = true;

            this.selectedRobotCameraBButton = selected;
         }
      }

      #endregion

      #endregion

      #region Delegate Functions

      private bool LocationDataProvider(ref double latitude, ref double longitude, ref DateTime dateTime, ref Directions direction, ref double displacement, ref double radialLocation)
      {
         bool result = false;

         latitude = NicBotComm.Instance.GetGpsLatitude();
         if (double.IsNaN(latitude) != false)
         {
            latitude = ParameterAccessor.Instance.Latitude;
         }

         longitude = NicBotComm.Instance.GetGpsLongitude();
         if (double.IsNaN(longitude) != false)
         {
            longitude = ParameterAccessor.Instance.Longitude;
         }

         dateTime = NicBotComm.Instance.GetGpsTime();
         if (dateTime.Year < 2000)
         {
            dateTime = DateTime.UtcNow;
         }
         
         direction = this.sensorDirection;
         displacement = NicBotComm.Instance.GetReelTotalDistance() * 100;
         radialLocation = NicBotComm.Instance.GetRobotRoll();

         if ((double.IsNaN(latitude) == false) &&
             (double.IsNaN(longitude) == false) &&
             (dateTime.Year > 2000) &&
             (Directions.unknown != direction))
         {
            result = true; 
         }

         return (result);
      }

      private void ReceiveThicknessReading(double thicknessReading)
      {
         if (double.IsNaN(thicknessReading) == false)
         {
            this.thicknessReading = thicknessReading;
            this.newThicknessReading = true;
         }
      }

      private void ReceiveStressReading(double stessReading)
      {
         if (double.IsNaN(stessReading) == false)
         {
            this.stressReading = stessReading;
            this.newStressReading = true;
         }
      }

      #endregion

      #region Process Functions

      private void ProcessStart()
      {
         this.processStopped = false;
         this.UpdateTimer.Interval = 1;
         this.Process = this.ProcessShow;
      }

      private void ProcessShow()
      {
         this.NitrogenPressure1TextPanel.ValueText = "";
         this.NitrogenPressure2TextPanel.ValueText = "";

         this.ReelTotalTextPanel.ValueText = "";
         this.ReelTripTextPanel.ValueText = "";
         this.ReelActualValuePanel.ValueText = "";

         this.FeederActualValuePanel.ValueText = "";
         this.FeederMoveButton.ValueText = "";
         this.FeederCurrentIndicatorTextBox.BackColor = Color.Black;
         this.TopFrontFeederCurrentTextPanel.ValueText = "";
         this.TopRearFeederCurrentTextPanel.ValueText = "";
         this.BottomFrontFeederCurrentTextPanel.ValueText = "";
         this.BottomRearFeederCurrentTextPanel.ValueText = "";

         this.MotorStatusDirectionalValuePanel.ValueText = "";
         this.MovementMoveButton.ValueText = "";
         this.TopFrontMovementMotorCurrentTextPanel.ValueText = "";
         this.MovementCurrentIndciatorTextBox.BackColor = Color.Black;
         this.MovementTemperatureIndicatorTextBox.BackColor = Color.Black;
         this.TopRearMovementMotorCurrentTextPanel.ValueText = "";
         this.BottomFrontMovementMotorCurrentTextPanel.ValueText = "";
         this.BottomRearMovementMotorCurrentTextPanel.ValueText = "";
         this.TopFrontMovementMotorTemperatureTextPanel.ValueText = "";
         this.TopRearMovementMotorTemperatureTextPanel.ValueText = "";
         this.BottomFrontMovementMotorTemperatureTextPanel.ValueText = "";
         this.BottomRearMovementMotorTemperatureTextPanel.ValueText = "";

         this.DrillExtendedActualValuePanel.ValueText = "";
         this.DrillExtendedSetPointValuePanel.ValueText = "";
         this.DrillRotationActualSpeedValuePanel.ValueText = "";
         this.DrillRotaionSetPointSpeedValuePanel.ValueText = "";

         this.SealantReserviorTextPanel.ValueText = "";
         this.SealantFlowRateTextPanel.ValueText = "";
         this.SealantActualPressureValuePanel.ValueText = "";
         this.SealantActualVolumeValuePanel.ValueText = "";
         this.SealantActualSpeedValuePanel.ValueText = "";

         this.SensorPipePositionTextPanel.ValueText = "";
         this.SensorLatitudeTextPanel.ValueText = "";
         this.SensorLongitudeTextPanel.ValueText = "";
         this.SensorGpsDateTextPanel.ValueText = "";
         this.SensorGpsTimeTextPanel.ValueText = "";
         this.SensorDisplacementTextPanel.ValueText = "";

         this.UpdateTimer.Interval = 1;
         this.Process = this.ProcessStarting;
      }

      private void ProcessStarting()
      {
         string versionString = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
         this.VersionLabel.Text = versionString;

         ParameterAccessor.Instance.Read(Application.ExecutablePath);
         this.traceListener.SetDestination(ParameterAccessor.Instance.Trace.Address, ParameterAccessor.Instance.Trace.Port);
         Tracer.WriteHigh(TraceGroup.GUI, null, "starting");

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            this.TitleLabel.Text = "  CIRRIS XR";

            this.FrontSealantReserviorPanel.Visible = true;
            this.RearSealantReserviorPanel.Visible = true;

            this.DrillMainPanel.Visible = true;

            this.InspectionPanel.Top = this.DrillMainPanel.Top;
            this.InspectionPanel.Left = this.DrillMainPanel.Left;
            this.InspectionPanel.Visible = false;

            ParameterAccessor.Instance.FrontDrill.ExtendedDistance.OperationalValue = 0;
            ParameterAccessor.Instance.FrontDrill.RotationSpeed.OperationalValue = 0;
            ParameterAccessor.Instance.RearDrill.ExtendedDistance.OperationalValue = 0;
            ParameterAccessor.Instance.RearDrill.RotationSpeed.OperationalValue = 0;

            this.RobotCamera6Button.Visible = true;
            this.RobotCamera11Button.Visible = true;

            this.RobotCamera2Button.Text = "RFF DRILL";
            this.RobotCamera6Button.Text = "FFF DRILL";
            this.RobotCamera10Button.Text = "LOWER FORWARD";
            this.RobotCamera9Button.Text = "RRF DRILL";
            this.RobotCamera11Button.Text = "FRF DRILL";
            this.RobotCamera12Button.Text = "LOWER BACK";

            this.RobotCamera2Button.Camera = CameraLocations.robotRffDrill;
            this.RobotCamera6Button.Camera = CameraLocations.robotFffDrill;
            this.RobotCamera10Button.Camera = CameraLocations.robotLowerForward;
            this.RobotCamera9Button.Camera = CameraLocations.robotRrfDrill;
            this.RobotCamera11Button.Camera = CameraLocations.robotFrfDrill;
            this.RobotCamera12Button.Camera = CameraLocations.robotLowerBack;
         }

         if (RobotApplications.inspect == ParameterAccessor.Instance.RobotApplication)
         {
            this.TitleLabel.Text = "  CIRRIS XI";

            this.FrontSealantReserviorPanel.Visible = false;
            this.RearSealantReserviorPanel.Visible = false;

            this.DrillMainPanel.Visible = false;
            this.DrillManualPanel.Visible = false;
            this.SealantMainPanel.Visible = false;
            this.SealantManualPanel.Visible = false;

            this.BodyDrillButton.Text = "";
            this.BodyDrillButton.Enabled = false;

            this.InspectionPanel.Top = this.DrillMainPanel.Top;
            this.InspectionPanel.Left = this.DrillMainPanel.Left;
            this.InspectionPanel.Visible = true;

            this.RobotCamera6Button.Visible = false;
            this.RobotCamera11Button.Visible = false;

            this.RobotCamera2Button.Text = "LOWER BACK";
            this.RobotCamera10Button.Text = "LOWER FORWARD";
            this.RobotCamera9Button.Text = "SENSOR ARM";
            this.RobotCamera12Button.Text = "SENSOR BAY";

            this.RobotCamera2Button.Camera = CameraLocations.robotLowerBack;
            this.RobotCamera10Button.Camera = CameraLocations.robotLowerForward;
            this.RobotCamera9Button.Camera = CameraLocations.robotSensorArm;
            this.RobotCamera12Button.Camera = CameraLocations.robotSensorBay;
         }

         this.indicatorFlasher = false;

         this.feederFastSelected = true;
         this.feederPreviousRequest = 0;
         this.feederNonManualMode = FeederModes.off;
         this.FeederSpeedToggleButton.OptionASelected = this.feederFastSelected;
         this.FeederManualPanel.Visible = false;
         this.FeederManulDisplayButton.Text = "SHOW MANUAL";
         this.FeederSpeedToggleButton.Visible = true;
         this.FeederSpeedValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.FeederManualSpeed);
         this.UpdateFeederControls();

         this.ReelCalibrateToButton.ValueText = this.GetValueText(ParameterAccessor.Instance.ReelCalibrationDistance);
         this.ReelManualCalibrateToButton.ValueText = this.GetValueText(ParameterAccessor.Instance.ReelCalibrationDistance);
         this.ReelCalibrateToButton.Enabled = false;
         this.ReelShowManualButton.Enabled = true;
         this.ReelManualDirectionToggleButton.OptionASelected = true;
         this.ReelManualDirectionToggleButton.Enabled = true;
         this.ReelSetupButton.Enabled = true;
         this.ReelManualSetupButton.Enabled = true;
         this.ReelManualHideButton.Enabled = true;
         this.UpdateReelControls();

         this.ReelManualPanel.Visible = false;
         
         if (MovementForwardControls.current == ParameterAccessor.Instance.ReelMotionMode)
         {
            this.ReelManualValueTextPanel.ValueText = this.GetValueText(ParameterAccessor.Instance.ReelManualCurrent);
            this.ReelManualDirectionToggleButton.Text = "TORQUE DIRECTION";
            this.ReelValuePromptLabel.Text = "SET CURRENT";
         }
         else
         {
            this.ReelManualValueTextPanel.ValueText = this.GetValueText(ParameterAccessor.Instance.ReelManualSpeed);
            this.ReelManualDirectionToggleButton.Text = "SPEED DIRECTION";
            this.ReelValuePromptLabel.Text = "SET SPEED";
         }

         this.movementFastSelected = true;
         this.movementNonManualMode = MovementModes.off;
         this.movementNonManualForwardMode = MovementForwardModes.normalAxial;
         this.MotorManualJogDistanceValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.MovementMotorManualJogDistance);
         this.MotorManualMoveSpeedValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.MovementMotorManualMoveSpeed);
         this.MovementSpeedToggleButton.OptionASelected = this.movementFastSelected;
         this.MovementManulPanel.Visible = false;
         this.MovementManaulDisplayButton.Text = "SHOW MANUAL";
         this.MovementSpeedToggleButton.Visible = true;

         #region Drill/Sealant Controls

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            this.toolLocation = (false != ParameterAccessor.Instance.FrontToolSelected) ? ToolLocations.front : ToolLocations.rear;
            this.SetDrillSelection(this.toolLocation);
            this.DrillManulDisplayButton.Text = "SHOW MANUAL";
            this.DrillMainPanel.Visible = true;
            this.DrillManualPanel.Visible = false;
            this.drillManualVisible = false;
            this.drillManualActivated = false;

            NicBotComm.Instance.SetPumpSpeed(this.toolLocation, 0);
            this.SealantManulDisplayButton.Text = "SHOW MANUAL";
            this.SealantMainPanel.Visible = false;
            this.SealantManualPanel.Visible = false;
            this.pumpManualVisible = false;
            this.pumpManualActivated = false;
            this.SealantManualModeToggleButton.OptionASelected = false;

            this.DrillMainPanel.Focus();
         }

         #endregion

         #region Sensor Controls

         if (RobotApplications.inspect == ParameterAccessor.Instance.RobotApplication)
         {
            this.sensorThicknessPending = false;
            this.newThicknessReading = false;
            this.sensorStressPending = false;
            this.newStressReading = false;
            this.sensorDirection = Directions.north;

            double latitude = ParameterAccessor.Instance.Latitude;
            if (double.IsNaN(latitude) == false)
            {
               this.SensorLatitudeTextPanel.ValueText = latitude.ToString("N4");
            }
            else
            {
               this.SensorLatitudeTextPanel.ValueText = "---";
            }

            double longitude = ParameterAccessor.Instance.Longitude;
            if (double.IsNaN(longitude) == false)
            {
               this.SensorLongitudeTextPanel.ValueText = longitude.ToString("N4");
            }
            else
            {
               this.SensorLongitudeTextPanel.ValueText = "---";
            }

            this.SensorDirectionTextPanel.ValueText = this.sensorDirection.ToString().ToUpper();
            this.SensorDirectionTextPanel.BackColor = Color.FromArgb(51, 51, 51);
            this.SensorDirectionTextPanel.Enabled = true;

            this.SensorDisplacementTextPanel.ValueText = "---";
            this.SensorGpsDateTextPanel.ValueText = "---";
            this.SensorGpsTimeTextPanel.ValueText = "---";

            this.SensorThicknessAcquireButton.Enabled = true;
            this.SensorThicknessReadingTextPanel.ValueText = "";

            this.SensorStressAcquireButton.Enabled = true;
            this.SensorStressReadingTextPanel.ValueText = "";

            if (0 != ParameterAccessor.Instance.LocationServer.Port)
            {
               LocationServer.Instance.Start(ParameterAccessor.Instance.LocationServer.Address, ParameterAccessor.Instance.LocationServer.Port);
            }
         }

         #endregion

         #region Camera Controls 

         this.RobotCamera1Button.CenterLevel = (int)ParameterAccessor.Instance.GetLightLevel(this.RobotCamera1Button.Camera);
         this.RobotCamera2Button.CenterLevel = (int)ParameterAccessor.Instance.GetLightLevel(this.RobotCamera2Button.Camera);
         this.RobotCamera3Button.CenterLevel = (int)ParameterAccessor.Instance.GetLightLevel(this.RobotCamera3Button.Camera);
         this.RobotCamera4Button.CenterLevel = (int)ParameterAccessor.Instance.GetLightLevel(this.RobotCamera4Button.Camera);
         this.RobotCamera5Button.CenterLevel = (int)ParameterAccessor.Instance.GetLightLevel(this.RobotCamera5Button.Camera);
         this.RobotCamera6Button.CenterLevel = (int)ParameterAccessor.Instance.GetLightLevel(this.RobotCamera6Button.Camera);
         this.RobotCamera7Button.CenterLevel = (int)ParameterAccessor.Instance.GetLightLevel(this.RobotCamera7Button.Camera);
         this.RobotCamera8Button.CenterLevel = (int)ParameterAccessor.Instance.GetLightLevel(this.RobotCamera8Button.Camera);
         this.RobotCamera9Button.CenterLevel = (int)ParameterAccessor.Instance.GetLightLevel(this.RobotCamera9Button.Camera);
         this.RobotCamera10Button.CenterLevel = (int)ParameterAccessor.Instance.GetLightLevel(this.RobotCamera10Button.Camera);
         this.RobotCamera11Button.CenterLevel = (int)ParameterAccessor.Instance.GetLightLevel(this.RobotCamera11Button.Camera);
         this.RobotCamera12Button.CenterLevel = (int)ParameterAccessor.Instance.GetLightLevel(this.RobotCamera12Button.Camera);
         this.LaunchCamera1Button.CenterLevel = (int)ParameterAccessor.Instance.GetLightLevel(this.LaunchCamera1Button.Camera);
         this.LaunchCamera2Button.CenterLevel = (int)ParameterAccessor.Instance.GetLightLevel(this.LaunchCamera2Button.Camera);
         this.LaunchCamera3Button.CenterLevel = (int)ParameterAccessor.Instance.GetLightLevel(this.LaunchCamera3Button.Camera);
         this.LaunchCamera4Button.CenterLevel = (int)ParameterAccessor.Instance.GetLightLevel(this.LaunchCamera4Button.Camera);

         this.ClearCameraSelects(this.RobotCamera1Button);
         this.ClearCameraSelects(this.RobotCamera2Button);
         this.ClearCameraSelects(this.RobotCamera3Button);
         this.ClearCameraSelects(this.RobotCamera4Button);
         this.ClearCameraSelects(this.RobotCamera5Button);
         this.ClearCameraSelects(this.RobotCamera6Button);
         this.ClearCameraSelects(this.RobotCamera7Button);
         this.ClearCameraSelects(this.RobotCamera8Button);
         this.ClearCameraSelects(this.RobotCamera9Button);
         this.ClearCameraSelects(this.RobotCamera10Button);
         this.ClearCameraSelects(this.RobotCamera11Button);
         this.ClearCameraSelects(this.RobotCamera12Button);
         this.ClearCameraSelects(this.LaunchCamera1Button);
         this.ClearCameraSelects(this.LaunchCamera2Button);
         this.ClearCameraSelects(this.LaunchCamera3Button);
         this.ClearCameraSelects(this.LaunchCamera4Button);

         this.cameraSelectMode = CameraSelectModes.none;
         this.UpdateCameraHoldEnable();
         this.UpdateCameraSelectorColor();

         this.selectedLaunchCameraButton = null;
         this.selectedRobotCameraAButton = null;
         this.selectedRobotCameraBButton = null;

         this.UpdateCameraHoldEnable();

         #endregion

         NicBotComm.Instance.Start();

         ParameterAccessor.Instance.FrontDrill.ExtendedDistance.OperationalValue = ParameterAccessor.Instance.FrontDrill.ExtendedDistance.MinimumValue; // todo remove
         ParameterAccessor.Instance.FrontDrill.RotationSpeed.OperationalValue = ParameterAccessor.Instance.FrontDrill.RotationSpeed.MinimumValue; // todo remove
         ParameterAccessor.Instance.RearDrill.ExtendedDistance.OperationalValue = ParameterAccessor.Instance.RearDrill.ExtendedDistance.MinimumValue; // todo remove
         ParameterAccessor.Instance.RearDrill.RotationSpeed.OperationalValue = ParameterAccessor.Instance.RearDrill.RotationSpeed.MinimumValue; // todo remove

         this.SystemStatusTextBox.Text = "starting";
         this.SystemStatusTextBox.BackColor = Color.Yellow;

         this.UpdateTimer.Interval = 100;
         this.Process = this.ProcessWaitComm;
      }

      private void ProcessWaitComm()
      {
         if (false != NicBotComm.Instance.Running)
         {
            this.AssignLaunchCamera(this.LaunchCamera1Button);
            this.AssignRobotCameraA(this.RobotCamera8Button);
            this.AssignRobotCameraB(this.RobotCamera1Button);

            this.UpdateBodyControls();
            this.UpdateMovementControls();
            this.UpdateSealantControls();

            Tracer.WriteHigh(TraceGroup.GUI, null, "started");
            this.Process = this.ProcessExecution;
         }
      }

      private void ProcessExecution()
      {
         this.indicatorFlasher = !this.indicatorFlasher;

         TruckCommBus.Instance.Service();
         RobotCommBus.Instance.Service();

         #region System Status

         string systemStatus = null;

         if (null != Joystick.Instance.FaultReason)
         {
            systemStatus = "joystick missing";
         }
         else
         {
            systemStatus = NicBotComm.Instance.GetStatus();
         }

         if (null == systemStatus)
         {
            this.SystemStatusTextBox.Text = "ready";
            this.SystemStatusTextBox.BackColor = Color.LimeGreen;
         }
         else
         {
            this.SystemStatusTextBox.Text = systemStatus;
            this.SystemStatusTextBox.BackColor = Color.Red;
         }

         #endregion

         #region Status (Main Air, Total Currents, Sealant Reserved)

         double nitrogenPressure1Reading = NicBotComm.Instance.GetNitrogenPressureReading1();

         if (double.IsNaN(nitrogenPressure1Reading) == false)
         {
            this.NitrogenPressure1TextPanel.ValueText = this.GetValueText(nitrogenPressure1Reading, ParameterAccessor.Instance.NitrogenPressureConversionUnit);
            this.SetCautionPanel(nitrogenPressure1Reading, ParameterAccessor.Instance.NitrogenPressureCaution, this.NitrogenPressure1TextPanel);
         }
         else
         {
            this.NitrogenPressure1TextPanel.ValueText = "---";
         }

         double nitrogenPressure2Reading = NicBotComm.Instance.GetNitrogenPressureReading2();

         if (double.IsNaN(nitrogenPressure2Reading) == false)
         {
            this.NitrogenPressure2TextPanel.ValueText = this.GetValueText(nitrogenPressure2Reading, ParameterAccessor.Instance.NitrogenPressureConversionUnit);
            this.SetCautionPanel(nitrogenPressure2Reading, ParameterAccessor.Instance.NitrogenPressureCaution, this.NitrogenPressure2TextPanel);
         }
         else
         {
            this.NitrogenPressure2TextPanel.ValueText = "---";
         }

         double robotTotalCurrentReading = NicBotComm.Instance.GetRobotTotalCurrentReading();

         if (double.IsNaN(robotTotalCurrentReading) == false)
         {
            this.RobotTotalCurrentTextPanel.ValueText = this.GetValueText(robotTotalCurrentReading, ParameterAccessor.Instance.RobotTotalCurrentConversionUnit);
         }
         else
         {
            this.RobotTotalCurrentTextPanel.ValueText = "---";
         }

         double launchTotalCurrentReading = NicBotComm.Instance.GetLaunchTotalCurrentReading();

         if (double.IsNaN(launchTotalCurrentReading) == false)
         {
            this.LaunchTotalCurrentTextPanel.ValueText = this.GetValueText(launchTotalCurrentReading, ParameterAccessor.Instance.LaunchTotalCurrentConversionUnit);
         }
         else
         {
            this.LaunchTotalCurrentTextPanel.ValueText = "---";
         }

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            double frontReserviourWeightReading = NicBotComm.Instance.GetReserviorWeightReading(ToolLocations.front);

            if (double.IsNaN(frontReserviourWeightReading) == false)
            {
               frontReserviourWeightReading *= 1000;
               this.FrontSealantReserviorWeightTextPanel.ValueText = this.GetValueText(frontReserviourWeightReading, "N0", "g");

               double frontReserviourVolumeReading = frontReserviourWeightReading / ParameterAccessor.Instance.FrontPump.SealantWeight.OperationalValue;
               this.FrontSealantReserviorVolumeTextPanel.ValueText = this.GetValueText(frontReserviourVolumeReading, "N0", "mL");
            }
            else
            {
               this.FrontSealantReserviorWeightTextPanel.ValueText = "---";
               this.FrontSealantReserviorVolumeTextPanel.ValueText = "---";
            }

            double rearReserviourWeightReading = NicBotComm.Instance.GetReserviorWeightReading(ToolLocations.rear);

            if (double.IsNaN(rearReserviourWeightReading) == false)
            {
               rearReserviourWeightReading *= 1000;
               this.RearSealantReserviorWeightTextPanel.ValueText = this.GetValueText(rearReserviourWeightReading, "N0", "g");

               double rearReserviourVolumeReading = rearReserviourWeightReading / ParameterAccessor.Instance.RearPump.SealantWeight.OperationalValue;
               this.RearSealantReserviorVolumeTextPanel.ValueText = this.GetValueText(rearReserviourVolumeReading, "N0", "mL");
            }
            else
            {
               this.RearSealantReserviorWeightTextPanel.ValueText = "---";
               this.RearSealantReserviorVolumeTextPanel.ValueText = "---";
            }
         }

         #endregion

         #region Reel

         string reelActualValueText = "";
         DirectionalValuePanel.Directions reelActualDirection = DirectionalValuePanel.Directions.Idle;

         bool displayReelCurrent = NicBotComm.Instance.ReelInCurrentMode();

         if (false != displayReelCurrent)
         {
            double reelCurrent = NicBotComm.Instance.GetReelCurrent();

            if (0 == reelCurrent)
            {
               reelActualDirection = DirectionalValuePanel.Directions.Idle;
            }
            else if (reelCurrent > 0)
            {
               reelActualDirection = DirectionalValuePanel.Directions.Forward;
            }
            else if (reelCurrent < 0)
            {
               reelActualDirection = DirectionalValuePanel.Directions.Reverse;
            }

            double reelDisplayCurrent = Math.Abs(reelCurrent);
            reelActualValueText = this.GetValueText(reelDisplayCurrent, ParameterAccessor.Instance.ReelReverseCurrent);
         }
         else
         {
            double reelSpeed = NicBotComm.Instance.GetReelSpeed();

            if (0 == reelSpeed)
            {
               reelActualDirection = DirectionalValuePanel.Directions.Idle;
            }
            else if (reelSpeed > 0)
            {
               reelActualDirection = DirectionalValuePanel.Directions.Forward;
            }
            else if (reelSpeed < 0)
            {
               reelActualDirection = DirectionalValuePanel.Directions.Reverse;
            }

            double reelDisplaySpeed = Math.Abs(reelSpeed);
            reelActualValueText = this.GetValueText(reelDisplaySpeed, ParameterAccessor.Instance.ReelReverseSpeed);
         }

         this.ReelActualValuePanel.Direction = reelActualDirection;
         this.ReelActualValuePanel.ValueText = reelActualValueText;

         double reelTotalDistance = NicBotComm.Instance.GetReelTotalDistance();
         string reelTotalText = this.GetValueText(reelTotalDistance, ParameterAccessor.Instance.ReelDistance);
         this.ReelTotalTextPanel.ValueText = reelTotalText;
         VideoStampOsd.Instance.SetPipeDisplacementText(reelTotalText);

         double reelTripDistance = NicBotComm.Instance.GetReelTripDistance();
         this.ReelTripTextPanel.ValueText = this.GetValueText(reelTripDistance, ParameterAccessor.Instance.ReelDistance);

         #endregion

         #region Feeder

         double feederVelocity = NicBotComm.Instance.GetFeederVelocity();

         if (feederVelocity > 0)
         {
            this.FeederActualValuePanel.Direction = DirectionalValuePanel.Directions.Forward;
         }
         else if (feederVelocity < 0)
         {
            this.FeederActualValuePanel.Direction = DirectionalValuePanel.Directions.Reverse;
         }
         else
         {
            this.FeederActualValuePanel.Direction = DirectionalValuePanel.Directions.Idle;
         }

         double feederSpeedValue = Math.Abs(feederVelocity);
         this.FeederActualValuePanel.ValueText = this.GetValueText(feederSpeedValue, ParameterAccessor.Instance.FeederMaxSpeed);

         bool feederClampRequest = NicBotComm.Instance.GetFeederClampSetPoint();
         bool feederClampActual = NicBotComm.Instance.GetFeederClamp();
         string feederClampStatus = "";

         if (feederClampRequest == feederClampActual)
         {
            if (false == feederClampRequest)
            {
               feederClampStatus = "CLAMP GRABBING";
            }
            else
            {
               feederClampStatus = "CLAMP RELEASING";
            }
         }
         else if (false != feederClampActual)
         {
            feederClampStatus = "CLAMP HELD";
         }
         else
         {
            feederClampStatus = "CLAMP RELEASED";
         }

         this.FeederClampSetupButton.Text = feederClampStatus;

         double maximumFeederCurrent = 0.0;
         double feederCurrent = 0;

         feederCurrent = NicBotComm.Instance.GetTopFrontFeederCurrent();
         maximumFeederCurrent = (feederCurrent > maximumFeederCurrent) ? feederCurrent : maximumFeederCurrent;
         this.TopFrontFeederCurrentTextPanel.ValueText = this.GetValueText(feederCurrent, "N2", "A");

         feederCurrent = NicBotComm.Instance.GetTopRearFeederCurrent();
         maximumFeederCurrent = (feederCurrent > maximumFeederCurrent) ? feederCurrent : maximumFeederCurrent;
         this.TopRearFeederCurrentTextPanel.ValueText = this.GetValueText(feederCurrent, "N2", "A");

         feederCurrent = NicBotComm.Instance.GetBottomFrontFeederCurrent();
         maximumFeederCurrent = (feederCurrent > maximumFeederCurrent) ? feederCurrent : maximumFeederCurrent;
         this.BottomFrontFeederCurrentTextPanel.ValueText = this.GetValueText(feederCurrent, "N2", "A");

         feederCurrent = NicBotComm.Instance.GetBottomRearFeederCurrent();
         maximumFeederCurrent = (feederCurrent > maximumFeederCurrent) ? feederCurrent : maximumFeederCurrent;
         this.BottomRearFeederCurrentTextPanel.ValueText = this.GetValueText(feederCurrent, "N2", "A");

         this.SetCautionPanel(maximumFeederCurrent, ParameterAccessor.Instance.FeederCurrentCaution, this.FeederCurrentIndicatorTextBox);

         #endregion

         #region Guide

         Color moveFlashColor = (false != indicatorFlasher) ? Color.FromArgb(190, 255, 50) : Color.FromArgb(32, 32, 32);

         if (NicBotComm.Instance.GuideAtExtendLimit(GuideLocations.left) != false)
         {
            this.GuideExtendLeftButton.LeftColor = Color.FromArgb(190, 255, 50);
         }
         else if (NicBotComm.Instance.GetGuideDirection(GuideLocations.left) == GuideDirections.extend)
         {
            this.GuideExtendLeftButton.LeftColor = moveFlashColor;
         }
         else
         {
            this.GuideExtendLeftButton.LeftColor = Color.FromArgb(32, 32, 32);
         }

         if (NicBotComm.Instance.GuideAtRetractLimit(GuideLocations.left) != false)
         {
            this.GuideRetractLeftButton.LeftColor = Color.FromArgb(190, 255, 50);
         }
         else if (NicBotComm.Instance.GetGuideDirection(GuideLocations.left) == GuideDirections.retract)
         {
            this.GuideRetractLeftButton.LeftColor = moveFlashColor;
         }
         else
         {
            this.GuideRetractLeftButton.LeftColor = Color.FromArgb(32, 32, 32);
         }

         if (NicBotComm.Instance.GuideAtExtendLimit(GuideLocations.right) != false)
         {
            this.GuideExtendRightButton.RightColor = Color.FromArgb(190, 255, 50);
         }
         else if (NicBotComm.Instance.GetGuideDirection(GuideLocations.right) == GuideDirections.extend)
         {
            this.GuideExtendRightButton.RightColor = moveFlashColor;
         }
         else
         {
            this.GuideExtendRightButton.RightColor = Color.FromArgb(32, 32, 32);
         }

         if (NicBotComm.Instance.GuideAtRetractLimit(GuideLocations.right) != false)
         {
            this.GuideRetractRightButton.RightColor = Color.FromArgb(190, 255, 50);
         }
         else if (NicBotComm.Instance.GetGuideDirection(GuideLocations.right) == GuideDirections.retract)
         {
            this.GuideRetractRightButton.RightColor = moveFlashColor;
         }
         else
         {
            this.GuideRetractRightButton.RightColor = Color.FromArgb(32, 32, 32);
         }

         #endregion

         #region Joystick and Movement

         Joystick.Instance.Update();

         if (false != Joystick.Instance.Valid)
         {
            #region Y Axis Movement

            int yAxis = (int)(((Joystick.Instance.YAxis) - 32768) * -1);
            bool movementReverse = false;
            bool movementForward = false;
            int joystickDeadBand = ParameterAccessor.Instance.JoystickDeadband;
            int joystickRange;

            int yAxisPrevious = yAxis;

            if (yAxis > joystickDeadBand)
            {
               this.MovementSpeedToggleButton.Enabled = false;
               yAxis -= joystickDeadBand;
               joystickRange = (32768 - joystickDeadBand);
            }
            else if (yAxis < -joystickDeadBand)
            {
               this.MovementSpeedToggleButton.Enabled = false;
               yAxis += joystickDeadBand;
               joystickRange = (32767 - joystickDeadBand);
            }
            else
            {
               this.MovementSpeedToggleButton.Enabled = true;
               yAxis = 0;
               joystickRange = 1;
            }

            if (yAxis < 0)
            {
               movementReverse = true;
               movementForward = false;
            }
            else if (yAxis > 0)
            {
               movementReverse = false;
               movementForward = true;
            }

            // Tracer.WriteHigh(TraceGroup.GUI, "", "joystick {0} {1}", yAxisPrevious, yAxis); // todo add joystick group
            double movementScale = (false != this.movementFastSelected) ? 1.0 : ParameterAccessor.Instance.MovementMotorLowSpeedScale.OperationalValue / 100;
            double movementRequest = movementScale * yAxis / joystickRange;

            bool movementTriggered = Joystick.Instance.Button1Pressed;
            NicBotComm.Instance.SetMovementRequest(movementRequest, movementTriggered);

            bool movementActivated = NicBotComm.Instance.GetMovementActivated();
            double movementRequestValue = 0;
            ValueParameter movementParameter = null;
            NicBotComm.Instance.GetMovementRequestValues(ref movementParameter, ref movementRequestValue);

            if (false != this.MovementMoveButton.Enabled)
            {
               double movementDisplayValue = Math.Abs(movementRequestValue);
               this.MovementMoveButton.LeftArrowVisible = movementReverse;
               this.MovementMoveButton.RightArrowVisible = movementForward;
               this.MovementMoveButton.ValueForeColor = (false != movementActivated) ? Color.White : Color.FromKnownColor(KnownColor.ControlDarkDark);
               this.MovementMoveButton.ValueText = this.GetValueText(movementDisplayValue, movementParameter);
            }
            else
            {
               this.MovementMoveButton.LeftArrowVisible = false;
               this.MovementMoveButton.RightArrowVisible = false;
               this.MovementMoveButton.ValueForeColor = Color.FromKnownColor(KnownColor.ControlDarkDark);
               this.MovementMoveButton.ValueText = this.GetValueText(0, movementParameter);
            }

            double movementValue = NicBotComm.Instance.GetMovementValue();
            DirectionalValuePanel.Directions direction = DirectionalValuePanel.Directions.Idle;

            if (movementValue > 0)
            {
               direction = DirectionalValuePanel.Directions.Forward;
            }
            else if (movementValue < 0)
            {
               direction = DirectionalValuePanel.Directions.Reverse;
            }

            this.MotorStatusDirectionalValuePanel.Direction = direction;
            double movementStatusDisplayValue = Math.Abs(movementValue);
            this.MotorStatusDirectionalValuePanel.ValueText = this.GetValueText(movementStatusDisplayValue, movementParameter);

            #endregion

            #region Trottle Axis Feeder

            int joystickThrottle = (int)(65535 - Joystick.Instance.Throttle);

            double feederRequest = 0;
            bool feederAutomaticTracking = this.GetAutomaticFeederTracking();


            if (false != feederAutomaticTracking)
            {
               feederRequest = movementRequestValue * ParameterAccessor.Instance.FeederTrackingCalibration.OperationalValue / 100;
            }
            else
            {
               double feederMaxSpeed = (false != this.feederFastSelected) ? ParameterAccessor.Instance.FeederMaxSpeed.OperationalValue : (ParameterAccessor.Instance.FeederMaxSpeed.OperationalValue * ParameterAccessor.Instance.FeederLowSpeedScale.OperationalValue / 100);
               feederRequest = feederMaxSpeed * joystickThrottle / 65535;

               if (false != movementReverse)
               {
                  feederRequest *= -1;
               }
            }


            bool directionSelected = (false != movementReverse) || (false != movementForward);
            bool feederMoveEnabled = false;

            if ((NicBotComm.Instance.GetFeederMode() == FeederModes.move) && (false == this.FeederManualPanel.Visible) && (false != movementActivated))
            {
               feederMoveEnabled = true;
            }

            if (false != this.FeederMoveButton.Enabled)
            {
               double feederDisplayValue = Math.Abs(feederRequest);
               this.FeederMoveButton.LeftArrowVisible = movementReverse;
               this.FeederMoveButton.RightArrowVisible = movementForward;
               this.FeederMoveButton.ValueForeColor = (false != feederMoveEnabled) ? Color.White : Color.FromKnownColor(KnownColor.ControlDarkDark);
               this.FeederMoveButton.ValueText = this.GetValueText(feederDisplayValue, ParameterAccessor.Instance.FeederMaxSpeed);
            }
            else
            {
               this.FeederMoveButton.LeftArrowVisible = false;
               this.FeederMoveButton.RightArrowVisible = false;
               this.FeederMoveButton.ValueForeColor = Color.FromKnownColor(KnownColor.ControlDarkDark);
               this.FeederMoveButton.ValueText = this.GetValueText(0, ParameterAccessor.Instance.FeederMaxSpeed);
            }

            if (false != feederMoveEnabled)
            {
               if (feederRequest != this.feederPreviousRequest)
               {
                  this.feederPreviousRequest = feederRequest;
                  NicBotComm.Instance.SetFeederVelocity(feederRequest);
               }
            }
            else
            {
               if (0 != this.feederPreviousRequest)
               {
                  this.feederPreviousRequest = 0;
                  NicBotComm.Instance.SetFeederVelocity(0);
               }
            }

            #endregion
         }

         double maximumMovementCurrent = 0;
         double movementCurrent = 0;

         movementCurrent = NicBotComm.Instance.GetTopFrontMovementCurrent();
         maximumFeederCurrent = (movementCurrent > maximumMovementCurrent) ? movementCurrent : maximumMovementCurrent;
         this.TopFrontMovementMotorCurrentTextPanel.ValueText = this.GetValueText(movementCurrent, "N2", "A");

         movementCurrent = NicBotComm.Instance.GetTopRearMovementCurrent();
         maximumMovementCurrent = (movementCurrent > maximumMovementCurrent) ? movementCurrent : maximumMovementCurrent;
         this.TopRearMovementMotorCurrentTextPanel.ValueText = this.GetValueText(movementCurrent, "N2", "A");

         movementCurrent = NicBotComm.Instance.GetBottomFrontMovementCurrent();
         maximumMovementCurrent = (movementCurrent > maximumMovementCurrent) ? movementCurrent : maximumMovementCurrent;
         this.BottomFrontMovementMotorCurrentTextPanel.ValueText = this.GetValueText(movementCurrent, "N2", "A");

         movementCurrent = NicBotComm.Instance.GetBottomRearMovementCurrent();
         maximumMovementCurrent = (movementCurrent > maximumMovementCurrent) ? movementCurrent : maximumMovementCurrent;
         this.BottomRearMovementMotorCurrentTextPanel.ValueText = this.GetValueText(movementCurrent, "N2", "A");

         this.SetCautionPanel(maximumMovementCurrent, ParameterAccessor.Instance.MovementCurrentCaution, this.MovementCurrentIndciatorTextBox);


         double maximumMovementTemperature = 0;
         double movementTemperature = 0;

         movementTemperature = NicBotComm.Instance.GetTopFrontMovementTemperature();
         maximumMovementTemperature = (movementTemperature > maximumMovementTemperature) ? movementTemperature : maximumMovementTemperature;
         this.TopFrontMovementMotorTemperatureTextPanel.ValueText = this.GetValueText(movementTemperature, "N1", "°C");

         movementTemperature = NicBotComm.Instance.GetTopRearMovementTemperature();
         maximumMovementTemperature = (movementTemperature > maximumMovementTemperature) ? movementTemperature : maximumMovementTemperature;
         this.TopRearMovementMotorTemperatureTextPanel.ValueText = this.GetValueText(movementTemperature, "N1", "°C");

         movementTemperature = NicBotComm.Instance.GetBottomFrontMovementTemperature();
         maximumMovementTemperature = (movementTemperature > maximumMovementTemperature) ? movementTemperature : maximumMovementTemperature;
         this.BottomFrontMovementMotorTemperatureTextPanel.ValueText = this.GetValueText(movementTemperature, "N1", "°C");

         movementTemperature = NicBotComm.Instance.GetBottomRearMovementTemperature();
         maximumMovementTemperature = (movementTemperature > maximumMovementTemperature) ? movementTemperature : maximumMovementTemperature;
         this.BottomRearMovementMotorTemperatureTextPanel.ValueText = this.GetValueText(movementTemperature, "N1", "°C");

         this.SetCautionPanel(maximumMovementTemperature, ParameterAccessor.Instance.MovementTemperatureCaution, this.MovementTemperatureIndicatorTextBox);

         #endregion

         #region Index Position

         #endregion

         #region Robot Status

         this.UpdateBodyControls();
         this.UpdatePipePositionDisplay();

         #endregion

         #region Drill Status

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            double drillIndexSetPoint = NicBotComm.Instance.GetDrillIndexSetPoint(this.toolLocation) * -1;
            double actualDrillIndexPosition = NicBotComm.Instance.GetDrillIndexPosition(this.toolLocation) * -1;
            bool atRetractionLimit = (0 == actualDrillIndexPosition) ? true : false;
            double drillOriginOffset = NicBotComm.Instance.GetDrillOriginOffset(this.toolLocation);

            if (double.IsNaN(drillOriginOffset) == false)
            {
               drillIndexSetPoint += drillOriginOffset;
               actualDrillIndexPosition += drillOriginOffset;
               this.OriginSetLightTextBox.BackColor = Color.Black;
               this.DrillMoveToOriginButton.Enabled = true;
            }
            else
            {
               this.OriginSetLightTextBox.BackColor = Color.Red;
               this.DrillMoveToOriginButton.Enabled = false;
            }
            
            this.DrillExtendedActualValuePanel.ValueText = this.GetValueText(actualDrillIndexPosition, ParameterAccessor.Instance.FrontDrill.ExtendedDistance);
            this.DrillExtendedSetPointValuePanel.ValueText = this.GetValueText(drillIndexSetPoint, ParameterAccessor.Instance.FrontDrill.ExtendedDistance);
            this.DrillRotationActualSpeedValuePanel.ValueText = this.GetValueText(NicBotComm.Instance.GetDrillRotationSpeed(this.toolLocation), ParameterAccessor.Instance.FrontDrill.RotationSpeed);

            if (false != atRetractionLimit)
            {
               this.RetractionLimitLightTextBox.BackColor = Color.Yellow;
               this.DrillSealModeButton.Enabled = true;
            }
            else
            {
               this.RetractionLimitLightTextBox.BackColor = Color.Black;
               this.DrillSealModeButton.Enabled = false;
            }

            this.DrillErrorLightTextBox.BackColor = (NicBotComm.Instance.GetDrillError(this.toolLocation) != false) ? Color.Red : Color.Black;

            this.UpdateDrillControls();
         }

         #endregion

         #region Sealant and Nozzle Status

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            bool nozzleExtended = NicBotComm.Instance.GetNozzleExtended(this.toolLocation);
            bool pumpSetPointActive = NicBotComm.Instance.GetPumpActivity(this.toolLocation);
            PumpAutoStates pumpAutoState = NicBotComm.Instance.GetPumpAutoState(this.toolLocation);

            bool nozzleError = (false != pumpSetPointActive) && (PumpAutoStates.running != pumpAutoState) && (false == nozzleExtended);

            this.SealantNozzlePositionTextPanel.BackColor = (false == nozzleError) ? Color.FromArgb(51, 51, 51) : Color.Red;
            this.SealantNozzlePositionTextPanel.ValueText = (false != nozzleExtended) ? "EXTENDED" : "RETRACTED";
            this.SealantNozzleToggleButton.OptionASelected = nozzleExtended;

            PumpParameters pumpParameters = (ToolLocations.front == this.toolLocation) ? ParameterAccessor.Instance.FrontPump : ParameterAccessor.Instance.RearPump;

            double sealantReserviorReading = NicBotComm.Instance.GetReserviorWeightReading(this.toolLocation);

            if (double.IsNaN(sealantReserviorReading) == false)
            {
               sealantReserviorReading *= 1000;
               this.SealantReserviorTextPanel.ValueText = this.GetValueText(sealantReserviorReading, "N0", "g");
            }
            else
            {
               this.SealantReserviorTextPanel.ValueText = "---";
            }

            double pumpVolumePerMinute = NicBotComm.Instance.GetPumpVolumePerSecond(this.toolLocation) * 60;
            this.SealantFlowRateTextPanel.ValueText = this.GetValueText((int)pumpVolumePerMinute, "mL/M");

            double pumpPressureReading = NicBotComm.Instance.GetPumpPressureReading(this.toolLocation);
            double pumpVolumeMeasure = NicBotComm.Instance.GetPumpVolumeMeasure(this.toolLocation);
            double pumpSpeedReading = NicBotComm.Instance.GetPumpSpeedReading(this.toolLocation);

            this.SealantActualPressureValuePanel.ValueText = this.GetValueText(pumpPressureReading, pumpParameters.MaximumPressure);
            this.SealantActualVolumeValuePanel.ValueText = this.GetValueText(pumpVolumeMeasure, pumpParameters.MaximumVolume);
            this.SealantActualSpeedValuePanel.ValueText = this.GetValueText(pumpSpeedReading, pumpParameters.MaximumSpeed);

            this.SealantActualVolumeValuePanel.ValueText = this.GetValueText(pumpVolumeMeasure, pumpParameters.MaximumVolume);

            PumpModes pumpMode = NicBotComm.Instance.GetPumpMode(this.toolLocation);
            double pumpSetPoint = NicBotComm.Instance.GetPumpSetPoint(this.toolLocation);

            if (PumpModes.pressure == pumpMode)
            {
               this.SealantPressureSetPointValuePanel.ForeColor = (false != pumpSetPointActive) ? Color.White : Color.Gray;
               this.SealantPressureSetPointValuePanel.ValueText = this.GetValueText(pumpSetPoint, pumpParameters.MaximumPressure);

               this.SealantVolumeSetPointValuePanel.ForeColor = Color.Black;
               this.SealantSpeedSetPointValuePanel.ForeColor = Color.Black;
            }
            else if (PumpModes.volume == pumpMode)
            {
               this.SealantVolumeSetPointValuePanel.ForeColor = (false != pumpSetPointActive) ? Color.White : Color.Gray;
               this.SealantVolumeSetPointValuePanel.ValueText = this.GetValueText(pumpSetPoint, pumpParameters.MaximumVolume);

               this.SealantPressureSetPointValuePanel.ForeColor = Color.Black;
               this.SealantSpeedSetPointValuePanel.ForeColor = Color.Black;
            }
            else if (PumpModes.speed == pumpMode)
            {
               PumpDirections pumpDirection = NicBotComm.Instance.GetPumpDirection(this.toolLocation);
               double pumpDisplaySetPoint = (PumpDirections.forward == pumpDirection) ? pumpSetPoint : -pumpSetPoint;

               this.SealantSpeedSetPointValuePanel.ForeColor = (false != pumpSetPointActive) ? Color.White : Color.Gray;
               this.SealantSpeedSetPointValuePanel.ValueText = this.GetValueText(pumpDisplaySetPoint, pumpParameters.MaximumSpeed);

               this.SealantPressureSetPointValuePanel.ForeColor = Color.Black;
               this.SealantVolumeSetPointValuePanel.ForeColor = Color.Black;
            }

            this.UpdateSealantControls();
         }

         #endregion

         #region Sensor Status

         if (RobotApplications.inspect == ParameterAccessor.Instance.RobotApplication)
         {
            double latitude = NicBotComm.Instance.GetGpsLatitude();
            if (double.IsNaN(latitude) == false)
            {
               ParameterAccessor.Instance.Latitude = latitude;
               this.SensorLatitudeTextPanel.ValueText = latitude.ToString("N4");
            }

            double longitude = NicBotComm.Instance.GetGpsLongitude();
            if (double.IsNaN(latitude) == false)
            {
               ParameterAccessor.Instance.Longitude = longitude;
               this.SensorLongitudeTextPanel.ValueText = longitude.ToString("N4");
            }

            DateTime dateTime = NicBotComm.Instance.GetGpsTime();
            if (dateTime.Year < 2000)
            {
               dateTime = DateTime.UtcNow;
            }

            this.SensorGpsDateTextPanel.ValueText = string.Format("{0:D2}-{1:D2}-{2:D4}", dateTime.Month, dateTime.Day, dateTime.Year);
            this.SensorGpsTimeTextPanel.ValueText = string.Format("{0:D2}:{1:D2}:{2:D2}", dateTime.Hour, dateTime.Minute, dateTime.Second);

            bool readingEnabled = true;

            if ((double.IsNaN(ParameterAccessor.Instance.Latitude) != false) ||
                (double.IsNaN(ParameterAccessor.Instance.Longitude) != false) ||
                (this.sensorDirection == Directions.unknown))
            {
               readingEnabled = false;
            }

            bool thicknessReadingEnabled = (NicBotComm.Instance.GetThicknessReadingEnabled() && readingEnabled);
            bool stressReadingEnabled = (NicBotComm.Instance.GetStressReadingEnabled() && readingEnabled);

            double sensorDisplacement = NicBotComm.Instance.GetReelTotalDistance() * 100;
            this.SensorDisplacementTextPanel.ValueText = this.GetValueText(sensorDisplacement, "N0", "cm");

            if (false != this.sensorThicknessPending)
            {
               if (NicBotComm.Instance.GetThicknessReadingPending() == false)
               {
                  this.sensorThicknessPending = false;
                  this.SensorThicknessAcquireButton.Enabled = thicknessReadingEnabled;

                  double thicknessReading = NicBotComm.Instance.GetThicknessReading();

                  if (double.IsNaN(thicknessReading) == false)
                  {
                     double reading = thicknessReading * ParameterAccessor.Instance.ThicknessConversionUnit.OperationalValue;
                     this.SensorThicknessReadingTextPanel.ValueText = this.GetValueText(reading, ParameterAccessor.Instance.ThicknessConversionUnit);
                  }
                  else
                  {
                     this.SensorThicknessReadingTextPanel.ValueText = "---";
                  }
               }
            }
            else
            {
               this.SensorThicknessAcquireButton.Enabled = thicknessReadingEnabled;
            }

            if (false != this.newThicknessReading)
            {
               this.newThicknessReading = false;
               double reading = this.thicknessReading * ParameterAccessor.Instance.ThicknessConversionUnit.OperationalValue;
               this.SensorThicknessReadingTextPanel.ValueText = this.GetValueText(reading, ParameterAccessor.Instance.ThicknessConversionUnit);
            }

            if (false != this.sensorStressPending)
            {
               if (NicBotComm.Instance.GetStressReadingPending() == false)
               {
                  this.sensorStressPending = false;
                  this.SensorStressAcquireButton.Enabled = stressReadingEnabled;

                  double stressReading = NicBotComm.Instance.GetStressReading();

                  if (double.IsNaN(stressReading) == false)
                  {
                     double reading = stressReading * ParameterAccessor.Instance.StressConversionUnit.OperationalValue;
                     this.SensorStressReadingTextPanel.ValueText = this.GetValueText(reading, ParameterAccessor.Instance.StressConversionUnit);
                  }
                  else
                  {
                     this.SensorStressReadingTextPanel.ValueText = "---";
                  }
               }
            }
            else
            {
               this.SensorStressAcquireButton.Enabled = stressReadingEnabled;
            }

            if (false != this.newStressReading)
            {
               this.newStressReading = false;
               double reading = this.stressReading * ParameterAccessor.Instance.StressConversionUnit.OperationalValue;
               this.SensorStressReadingTextPanel.ValueText = this.GetValueText(reading, ParameterAccessor.Instance.StressConversionUnit);
            }

            if (false == this.SensorDirectionTextPanel.Enabled)
            {
               if ((false == this.sensorThicknessPending) &&
                   (false == this.sensorStressPending))
               {
                  this.SensorDirectionTextPanel.Enabled = true;
               }
            }
         }

         #endregion

         if (false != this.processStopNeeded)
         {
            this.processStopNeeded = false;
            this.Process = this.ProcessStopping;
         }
      }

      private void ProcessStopping()
      {

         this.SystemStatusTextBox.Text = "stopping";
         this.SystemStatusTextBox.BackColor = Color.Yellow;
         Tracer.WriteHigh(TraceGroup.GUI, null, "stopping");

         NicBotComm.Instance.Stop();

         this.Process = this.ProcessWaitCommStop;
      }

      private void ProcessWaitCommStop()
      {
         if (false == NicBotComm.Instance.Running)
         {
            LocationServer.Instance.Stop();

            ParameterAccessor.Instance.FrontToolSelected = (ToolLocations.front == this.toolLocation) ? true : false;
            ParameterAccessor.Instance.Write(Application.ExecutablePath);

            Tracer.WriteHigh(TraceGroup.GUI, null, "stopped");

            this.Process = this.ProcessCommStopped;
         }
      }

      private void ProcessCommStopped()
      {
         this.processStopped = true;

         if (false != this.processExitNeeded)
         {
            this.Process = this.ProcessExit;
         }
         else
         {
            this.Process = this.ProcessStart;
         }
      }

      private void ProcessExit()
      {
         this.UpdateTimer.Enabled = false;
         this.Close();
      }

      private void TickProcess()
      {
         this.Process();
      }

      #endregion

      #region Air Event Process

      #endregion

      #region Reel Event Process

      private void ReelTotalTextPanel_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         double reelTotalDistance = NicBotComm.Instance.GetReelTotalDistance();
         DialogResult result = this.LaunchNumberEdit(ref reelTotalDistance, this.ReelTotalTextPanel, "TOTAL DISTANCE", 0, "m", reelTotalDistance, -1000, 1000);

         if (result == System.Windows.Forms.DialogResult.OK)
         {
            NicBotComm.Instance.SetReelTotalDistance(reelTotalDistance);
         }
      }

      private void ReelTripTextPanel_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         double reelTripDistance = NicBotComm.Instance.GetReelTripDistance();
         DialogResult result = this.LaunchNumberEdit(ref reelTripDistance, this.ReelTotalTextPanel, "TRIP DISTANCE", 0, "m", reelTripDistance, -1000, 1000);

         if (result == System.Windows.Forms.DialogResult.OK)
         {
            NicBotComm.Instance.SetReelTripDistance(reelTripDistance);
         }
      }

      private void ReelOffButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         NicBotComm.Instance.SetReelMode(ReelModes.off);
         this.ReelSetupButton.Enabled = true;
         this.UpdateReelControls();

         e.Handled = true;
      }

      private void ReelReverseButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         NicBotComm.Instance.SetReelMode(ReelModes.reverse);
         this.ReelSetupButton.Enabled = false;
         this.UpdateReelControls();

         e.Handled = true;
      }

      private void ReelLockButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         NicBotComm.Instance.SetReelMode(ReelModes.locked);
         this.ReelSetupButton.Enabled = false;
         this.UpdateReelControls();

         e.Handled = true;
      }

      private void ReelResetTotalButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         NicBotComm.Instance.ResetReelTotalDistance();
         NicBotComm.Instance.StartReelCalibration();
         this.ReelCalibrateToButton.Enabled = true;

         e.Handled = true;
      }

      private void ReelCalibrateToButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         NicBotComm.Instance.CalibrateReel(ParameterAccessor.Instance.ReelCalibrationDistance.OperationalValue);
         this.ReelCalibrateToButton.Enabled = false;
         e.Handled = true;
      }

      private void ReelResetTripButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         NicBotComm.Instance.ResetReelTripDistance();
         e.Handled = true;
      }

      private void ReelSetupButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         ReelSetupForm reelSetupForm = new ReelSetupForm();
         this.SetDialogLocation(this.ReelSetupButton, reelSetupForm);

         this.DimBackground();
         reelSetupForm.ShowDialog();
         this.LightBackground();

         this.ReelCalibrateToButton.ValueText = this.GetValueText(ParameterAccessor.Instance.ReelCalibrationDistance);
         this.ReelManualCalibrateToButton.ValueText = this.GetValueText(ParameterAccessor.Instance.ReelCalibrationDistance);
         this.ReelManualValueTextPanel.ValueText = (MovementForwardControls.current == ParameterAccessor.Instance.ReelMotionMode) ? this.GetValueText(ParameterAccessor.Instance.ReelManualCurrent) : this.GetValueText(ParameterAccessor.Instance.ReelManualSpeed);
         this.ReelManualDirectionToggleButton.Text = (MovementForwardControls.current == ParameterAccessor.Instance.ReelMotionMode) ? "TORQUE DIRECTION" : "SPEED DIRECTION";
         this.ReelValuePromptLabel.Text = (MovementForwardControls.current == ParameterAccessor.Instance.ReelMotionMode) ? "SET CURRENT" : "SET SPEED";

         e.Handled = true;
      }

      private void ReelShowManualButton_Click(object sender, EventArgs e)
      {
         this.ReelManualOnOffToggleButton.OptionASelected = false;

         this.ReelManualPanel.Top = this.GetAbsoluteTop(this.ReelOffButton);
         this.ReelManualPanel.Left = this.ReelMainPanel.Left;
         this.ReelManualPanel.Visible = true;
      }

      private void ReelManualOnOffToggleButton_MouseClick(object sender, MouseEventArgs e)
      {
         bool requested = !this.ReelManualOnOffToggleButton.OptionASelected;

         if (false == requested)
         {
            NicBotComm.Instance.SetReelManualMode(false);
            this.ReelManualHideButton.Enabled = true;
            this.ReelManualDirectionToggleButton.Enabled = true;
            this.ReelManualSetupButton.Enabled = true;

            this.ReelManualOnOffToggleButton.OptionASelected = requested;
         }
      }

      private void ReelManualOnOffToggleButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         bool requested = !this.ReelManualOnOffToggleButton.OptionASelected;

         if (false != requested)
         {
            double directionalModifier = (false != this.ReelManualDirectionToggleButton.OptionASelected) ? 1 : -1;
            double manualCurrent = ParameterAccessor.Instance.ReelManualCurrent.OperationalValue * directionalModifier;
            NicBotComm.Instance.SetReelManualCurrent(manualCurrent);

            NicBotComm.Instance.SetReelManualMode(true);
            this.ReelManualHideButton.Enabled = false;
            this.ReelManualDirectionToggleButton.Enabled = false;
            this.ReelManualSetupButton.Enabled = false;

            this.ReelManualOnOffToggleButton.OptionASelected = requested;
         }
         else
         {
            NicBotComm.Instance.SetReelManualMode(false);
            this.ReelManualHideButton.Enabled = true;
            this.ReelManualDirectionToggleButton.Enabled = true;
            this.ReelManualSetupButton.Enabled = true;

            this.ReelManualOnOffToggleButton.OptionASelected = requested;
         }
      }

      private void ReelManualValueTextPanel_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         if (MovementForwardControls.current == ParameterAccessor.Instance.ReelMotionMode)
         {
            DialogResult result = this.LaunchNumberEdit(this.ReelManualValueTextPanel, "REEL CURRENT", ParameterAccessor.Instance.ReelManualCurrent);

            if (result == System.Windows.Forms.DialogResult.OK)
            {
               this.ReelManualValueTextPanel.ValueText = this.GetValueText(ParameterAccessor.Instance.ReelManualCurrent);

               double directionalModifier = (false != this.ReelManualDirectionToggleButton.OptionASelected) ? 1 : -1;
               double manualCurrent = ParameterAccessor.Instance.ReelManualCurrent.OperationalValue * directionalModifier;
               NicBotComm.Instance.SetReelManualCurrent(manualCurrent);
            }
         }
         else
         {
            DialogResult result = this.LaunchNumberEdit(this.ReelManualValueTextPanel, "REEL SPEED", ParameterAccessor.Instance.ReelManualSpeed);

            if (result == System.Windows.Forms.DialogResult.OK)
            {
               this.ReelManualValueTextPanel.ValueText = this.GetValueText(ParameterAccessor.Instance.ReelManualSpeed);

               double directionalModifier = (false != this.ReelManualDirectionToggleButton.OptionASelected) ? 1 : -1;
               double manualSpeed = ParameterAccessor.Instance.ReelManualSpeed.OperationalValue * directionalModifier;
               NicBotComm.Instance.SetReelManualSpeed(manualSpeed);
            }
         }
      }

      private void ReelManualValueUpButton_Click(object sender, EventArgs e)
      {
         this.IncrementReelManualValue();
      }

      private void ReelManualValueUpButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         this.IncrementReelManualValue();
      }

      private void ReelManualValueDownButton_Click(object sender, EventArgs e)
      {
         this.DecrementReelManualValue();
      }

      private void ReelManualValueDownButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         this.DecrementReelManualValue();
      }

      private void ReelManualDirectionToggleButton_Click(object sender, EventArgs e)
      {
         this.ReelManualDirectionToggleButton.OptionASelected = !this.ReelManualDirectionToggleButton.OptionASelected;

         double directionalModifier = (false != this.ReelManualDirectionToggleButton.OptionASelected) ? 1 : -1;
         double manualCurrent = ParameterAccessor.Instance.ReelManualCurrent.OperationalValue * directionalModifier;
         NicBotComm.Instance.SetReelManualCurrent(manualCurrent);
      }

      private void ReelManualResetTotalButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         this.ReelResetTotalButton_HoldTimeout(sender, e);
      }

      private void ReelManualCalibrateToButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         this.ReelCalibrateToButton_HoldTimeout(sender, e);
      }

      private void ReelManualResetTripButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         this.ReelResetTripButton_HoldTimeout(sender, e);
      }

      private void ReelManualSetupButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         this.ReelSetupButton_HoldTimeout(sender, e);
      }

      private void ReelManualHideButton_Click(object sender, EventArgs e)
      {
         this.UpdateReelControls();
         this.ReelManualPanel.Visible = false;
      }

      #endregion

      #region Feeder Event Process

      private void FeederOffButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         if (this.GetAutomaticFeederTracking() == false)
         {
            NicBotComm.Instance.SetFeederMode(FeederModes.off);
            this.UpdateFeederControls();
         }
      }

      private void FeederMoveButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)      
      {
         if (this.GetAutomaticFeederTracking() == false)
         {
            NicBotComm.Instance.SetFeederMode(FeederModes.move);
            this.UpdateFeederControls();
         }
      }

      private void FeederLockButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         if (this.GetAutomaticFeederTracking() == false)
         {
            NicBotComm.Instance.SetFeederMode(FeederModes.locked);
            this.UpdateFeederControls();
         }
      }

      private void FeederSetupButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         FeederSetupForm feederSetupForm = new FeederSetupForm();
         this.SetDialogLocation(this.FeederSetupButton, feederSetupForm);

         this.DimBackground();
         feederSetupForm.ShowDialog();
         this.LightBackground();
         this.UpdateMovementControls();
         NicBotComm.Instance.EvaluateFeederParameters(); // needed?

         e.Handled = true;
      }

      private void FeederSpeedToggleButton_Click(object sender, EventArgs e)
      {
         bool selection = !this.FeederSpeedToggleButton.OptionASelected;
         this.feederFastSelected = selection;
         this.FeederSpeedToggleButton.OptionASelected = selection;
      }

      private void FeederManulDisplayButton_Click(object sender, EventArgs e)
      {
         if (false == this.FeederManualPanel.Visible)
         {
            this.feederNonManualMode = NicBotComm.Instance.GetFeederMode();

            this.FeederManualPanel.Left = this.FeederMainPanel.Left;
            this.FeederManualPanel.Top = this.GetAbsoluteTop(this.FeederOffButton);
            this.FeederManualPanel.Visible = true;

            this.FeederManulDisplayButton.Text = "HIDE MANUAL";
         }
         else
         {
            this.FeederManualPanel.Visible = false;
            this.FeederManulDisplayButton.Text = "SHOW MANUAL";

            if (this.GetAutomaticFeederTracking() != false)
            {
               MovementModes movementMode = NicBotComm.Instance.GetMovementMode();

               if (MovementModes.off == movementMode)
               {
                  NicBotComm.Instance.SetFeederMode(FeederModes.off);
               }
               else if (MovementModes.move == movementMode)
               {
                  NicBotComm.Instance.SetFeederMode(FeederModes.move);
               }
               else if (MovementModes.locked == movementMode)
               {
                  NicBotComm.Instance.SetFeederMode(FeederModes.locked);
               }
            }
         }

         this.UpdateFeederControls();
      }

      private void FeederManualReverseButton_MouseDown(object sender, MouseEventArgs e)
      {
         NicBotComm.Instance.SetFeederVelocity(-ParameterAccessor.Instance.FeederManualSpeed.OperationalValue);
         NicBotComm.Instance.SetFeederMode(FeederModes.move);
      }

      private void FeederManualReverseButton_MouseUp(object sender, MouseEventArgs e)
      {
         NicBotComm.Instance.SetFeederVelocity(0);
         NicBotComm.Instance.SetFeederMode(this.feederNonManualMode);
      }

      private void FeederSpeedValueButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         DialogResult result = this.LaunchNumberEdit(this.FeederSpeedValueButton, "FEEDER SPEED", ParameterAccessor.Instance.FeederManualSpeed);

         if (result == System.Windows.Forms.DialogResult.OK)
         {
            this.FeederSpeedValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.FeederManualSpeed);
         }
      }

      private void FeederManualForwardButton_MouseDown(object sender, MouseEventArgs e)
      {
         NicBotComm.Instance.SetFeederVelocity(ParameterAccessor.Instance.FeederManualSpeed.OperationalValue);
         NicBotComm.Instance.SetFeederMode(FeederModes.move);
      }

      private void FeederManualForwardButton_MouseUp(object sender, MouseEventArgs e)
      {
         NicBotComm.Instance.SetFeederVelocity(0);
         NicBotComm.Instance.SetFeederMode(this.feederNonManualMode);
      }

      private void FeederClampSetupButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         FeederClampSetupForm feederGrabberSetupForm = new FeederClampSetupForm();
         this.SetDialogLocation(this.FeederClampSetupButton, feederGrabberSetupForm);

         this.DimBackground();
         feederGrabberSetupForm.ShowDialog();
         this.LightBackground();

         e.Handled = true;
      }

      #endregion

      #region Guide Event Process

      private void GuideRetractLeftButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         if (false == ParameterAccessor.Instance.GuideMomentaryButtonAction)
         {
            if (NicBotComm.Instance.GetGuideDirection(GuideLocations.left) == GuideDirections.retract)
            {
               NicBotComm.Instance.SetGuideDirection(GuideLocations.left, GuideDirections.off);
            }
            else
            {
               NicBotComm.Instance.SetGuideDirection(GuideLocations.left, GuideDirections.retract);
            }
         }
         else
         {
            NicBotComm.Instance.SetGuideDirection(GuideLocations.left, GuideDirections.retract);
         }

         e.Handled = false;
      }

      private void GuideRetractLeftButton_Click(object sender, EventArgs e)
      {
         if (false != ParameterAccessor.Instance.GuideMomentaryButtonAction)
         {
            NicBotComm.Instance.SetGuideDirection(GuideLocations.left, GuideDirections.off);
         }
      }

      private void GuideExtendLeftButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         if (false == ParameterAccessor.Instance.GuideMomentaryButtonAction)
         {
            if (NicBotComm.Instance.GetGuideDirection(GuideLocations.left) == GuideDirections.extend)
            {
               NicBotComm.Instance.SetGuideDirection(GuideLocations.left, GuideDirections.off);
            }
            else
            {
               NicBotComm.Instance.SetGuideDirection(GuideLocations.left, GuideDirections.extend);
            }
         }
         else
         {
            NicBotComm.Instance.SetGuideDirection(GuideLocations.left, GuideDirections.extend);
         }

         e.Handled = false;
      }

      private void GuideExtendLeftButton_Click(object sender, EventArgs e)
      {
         if (false != ParameterAccessor.Instance.GuideMomentaryButtonAction)
         {
            NicBotComm.Instance.SetGuideDirection(GuideLocations.left, GuideDirections.off);
         }
      }

      private void GuideRetractRightButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         if (false == ParameterAccessor.Instance.GuideMomentaryButtonAction)
         {
            if (NicBotComm.Instance.GetGuideDirection(GuideLocations.right) == GuideDirections.retract)
            {
               NicBotComm.Instance.SetGuideDirection(GuideLocations.right, GuideDirections.off);
            }
            else
            {
               NicBotComm.Instance.SetGuideDirection(GuideLocations.right, GuideDirections.retract);
            }
         }
         else
         {
            NicBotComm.Instance.SetGuideDirection(GuideLocations.right, GuideDirections.retract);
         }

         e.Handled = false;
      }

      private void GuideRetractRightButton_Click(object sender, EventArgs e)
      {
         if (false != ParameterAccessor.Instance.GuideMomentaryButtonAction)
         {
            NicBotComm.Instance.SetGuideDirection(GuideLocations.right, GuideDirections.off);
         }
      }

      private void GuideExtendRightButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         if (false == ParameterAccessor.Instance.GuideMomentaryButtonAction)
         {
            if (NicBotComm.Instance.GetGuideDirection(GuideLocations.right) == GuideDirections.extend)
            {
               NicBotComm.Instance.SetGuideDirection(GuideLocations.right, GuideDirections.off);
            }
            else
            {
               NicBotComm.Instance.SetGuideDirection(GuideLocations.right, GuideDirections.extend);
            }
         }
         else
         {
            NicBotComm.Instance.SetGuideDirection(GuideLocations.right, GuideDirections.extend);
         }

         e.Handled = false;
      }

      private void GuideExtendRightButton_Click(object sender, EventArgs e)
      {
         if (false != ParameterAccessor.Instance.GuideMomentaryButtonAction)
         {
            NicBotComm.Instance.SetGuideDirection(GuideLocations.right, GuideDirections.off);
         }
      }

      private void GuideSetupButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         GuideSetupForm tetherGuildSetupForm = new GuideSetupForm();
         this.SetDialogLocation(this.GuideSetupButton, tetherGuildSetupForm);

         tetherGuildSetupForm.ExtensionSpeed = ParameterAccessor.Instance.GuideExtensionSpeed;
         tetherGuildSetupForm.RetractionSpeed = ParameterAccessor.Instance.GuideRetractionSpeed;
         tetherGuildSetupForm.MomentaryButtonAction = ParameterAccessor.Instance.GuideMomentaryButtonAction;

         this.DimBackground();
         tetherGuildSetupForm.ShowDialog();
         this.LightBackground();

         ParameterAccessor.Instance.GuideExtensionSpeed = tetherGuildSetupForm.ExtensionSpeed;
         ParameterAccessor.Instance.GuideRetractionSpeed = tetherGuildSetupForm.RetractionSpeed;
         ParameterAccessor.Instance.GuideMomentaryButtonAction = tetherGuildSetupForm.MomentaryButtonAction;

         e.Handled = true;
      }

      #endregion

      #region Body Event Process

      private void BodyClosedButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         if (this.GetBodyChangeAllowed() != false)
         {
            NicBotComm.Instance.SetBodyPosition(BodyPositions.closed);
            this.UpdateBodyControls();
         }

         e.Handled = true;
      }

      private void BodyOpenButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         if (this.GetBodyChangeAllowed() != false)
         {
            NicBotComm.Instance.SetBodyPosition(BodyPositions.opened);
            this.UpdateBodyControls();
         }

         e.Handled = true;
      }

      private void BodyDrillButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         if (this.GetBodyChangeAllowed() != false)
         {
            NicBotComm.Instance.SetBodyPosition(BodyPositions.drill);
            this.UpdateBodyControls();
         }

         e.Handled = true;
      }

      private void BodyRearReleaseButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         if (this.GetBodyChangeAllowed() != false)
         {
            NicBotComm.Instance.SetBodyPosition(BodyPositions.rearLoose);
            this.UpdateBodyControls();
         }

         e.Handled = true;
      }

      private void BodyFrontReleaseButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         if (this.GetBodyChangeAllowed() != false)
         {
            NicBotComm.Instance.SetBodyPosition(BodyPositions.frontLoose);
            this.UpdateBodyControls();
         }

         e.Handled = true;
      }

      private void CustomSetupButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         if (this.GetBodyChangeAllowed() != false)
         {
            if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
            {
               RepairBodySetupForm repairBodySetupForm = new RepairBodySetupForm();
               this.SetDialogLocation(this.CustomSetupButton, repairBodySetupForm);

               this.DimBackground();
               repairBodySetupForm.ShowDialog();
               this.LightBackground();
            }
            else if (RobotApplications.inspect == ParameterAccessor.Instance.RobotApplication)
            {
               InspectBodySetupForm inspectBodySetupForm = new InspectBodySetupForm();
               this.SetDialogLocation(this.CustomSetupButton, inspectBodySetupForm);

               this.DimBackground();
               inspectBodySetupForm.ShowDialog();
               this.LightBackground();
            }

            this.UpdateBodyControls();
            this.UpdateMovementControls();
            this.UpdateSealantControls();
         }

         e.Handled = true;
      }

      #endregion

      #region Movement Main Event Process

      private void MovementOffButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         NicBotComm.Instance.SetMovementMode(MovementModes.off);
         this.UpdateMovementControls();
      }

      private void MovementMoveButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         NicBotComm.Instance.SetMovementMode(MovementModes.move);
         this.UpdateMovementControls();
      }

      private void MovementLockButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         NicBotComm.Instance.SetMovementMode(MovementModes.locked);
         this.UpdateMovementControls();
      }

      private void MovementAxialToggleButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         bool selectionA = this.MovementAxialToggleButton.OptionASelected;
         bool selectionB = this.MovementAxialToggleButton.OptionBSelected;
         bool selection = !selectionA;

         if (selectionA == selectionB)
         {
            selection = true;
         }

         MovementForwardModes mode = (false != selection) ? MovementForwardModes.normalAxial : MovementForwardModes.circumferential;
         NicBotComm.Instance.SetMovementForwardMode(mode);

         this.UpdateMovementControls();
      }

      private void MovementLaunchModeToggleButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         bool selection = !this.MovementLaunchModeToggleButton.OptionASelected;
         MovementForwardModes mode = (false != selection) ? MovementForwardModes.launchAxial : MovementForwardModes.normalAxial;
         NicBotComm.Instance.SetMovementForwardMode(mode);

         this.UpdateMovementControls();
      }

      private void MovementCornerModeToggleButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         bool selection = !this.MovementCornerModeToggleButton.OptionASelected;
         MovementForwardModes mode = (false != selection) ? MovementForwardModes.cornerAxial : MovementForwardModes.normalAxial;
         NicBotComm.Instance.SetMovementForwardMode(mode);

         this.UpdateMovementControls();
      }

      private void MovementSetupButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         MovementSetupForm motorSetupForm = new MovementSetupForm();
         this.SetDialogLocation(this.MovementSetupButton, motorSetupForm);

         this.DimBackground();
         motorSetupForm.ShowDialog();
         this.LightBackground();

         this.UpdateMovementControls();
         e.Handled = true;
      }

      private void MovementSpeedToggleButton_Click(object sender, EventArgs e)
      {
         bool selection = !this.MovementSpeedToggleButton.OptionASelected;
         this.movementFastSelected = selection;
         this.MovementSpeedToggleButton.OptionASelected = selection;
      }

      private void MovementManaulDisplayButton_Click(object sender, EventArgs e)
      {
         this.movementNonManualMode = NicBotComm.Instance.GetMovementMode();
         this.movementNonManualForwardMode = NicBotComm.Instance.GetMovementForwardMode();

         if (MovementForwardModes.normalAxial == this.movementNonManualForwardMode)
         {
            NicBotComm.Instance.SetMovementForwardMode(MovementForwardModes.normalAxial);
         }
         else
         {
            NicBotComm.Instance.SetMovementForwardMode(MovementForwardModes.circumferential);
         }

         NicBotComm.Instance.SetMovementMode(MovementModes.move);
            
         this.MovementManulPanel.Left = this.MovementMainPanel.Left;
         this.MovementManulPanel.Top = this.GetAbsoluteTop(this.MovementOffButton);
         this.MovementManulPanel.Visible = true;

         this.UpdateMovementControls();
      }

      #endregion

      #region Movement Manual Event Process

      private void MotorManualJogReverseButton_Click(object sender, EventArgs e)
      {
         //NicBotComm.Instance.Move(-ParameterAccessor.Instance.MovementMotorManualJogDistance.OperationalValue);
      }

      private void MotorManualJogDistanceValueButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         DialogResult result = this.LaunchNumberEdit(this.MotorManualJogDistanceValueButton, "JOG DISTANCE", ParameterAccessor.Instance.MovementMotorManualJogDistance);

         if (result == System.Windows.Forms.DialogResult.OK)
         {
            this.MotorManualJogDistanceValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.MovementMotorManualJogDistance);
         }
      }

      private void MotorManualJogForwardButton_Click(object sender, EventArgs e)
      {
         //NicBotComm.Instance.Move(ParameterAccessor.Instance.MovementMotorManualJogDistance.OperationalValue);
      }

      private void MotorManualMoveReverseButton_MouseDown(object sender, MouseEventArgs e)
      {
         NicBotComm.Instance.SetMovementManualVelocity(-ParameterAccessor.Instance.MovementMotorManualMoveSpeed.OperationalValue);
      }

      private void MotorManualMoveReverseButton_MouseUp(object sender, MouseEventArgs e)
      {
         NicBotComm.Instance.SetMovementManualVelocity(0);
      }

      private void MotorManualMoveSpeedValueButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         DialogResult result = this.LaunchNumberEdit(this.MotorManualMoveSpeedValueButton, "MOVE SPEED", ParameterAccessor.Instance.MovementMotorManualMoveSpeed);

         if (result == System.Windows.Forms.DialogResult.OK)
         {
            this.MotorManualMoveSpeedValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.MovementMotorManualMoveSpeed);
         }
      }

      private void MotorManualMoveForwardButton_MouseDown(object sender, MouseEventArgs e)
      {
         NicBotComm.Instance.SetMovementManualVelocity(ParameterAccessor.Instance.MovementMotorManualMoveSpeed.OperationalValue);
      }

      private void MotorManualMoveForwardButton_MouseUp(object sender, MouseEventArgs e)
      {
         NicBotComm.Instance.SetMovementManualVelocity(0);
      }

      private void MovementManualSetupButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         this.MovementSetupButton_HoldTimeout(sender, e);
      }

      private void MovementHideManualButton_Click(object sender, EventArgs e)
      {
         NicBotComm.Instance.SetMovementMode(this.movementNonManualMode);
         NicBotComm.Instance.SetMovementForwardMode(this.movementNonManualForwardMode);

         this.MovementSpeedToggleButton.Visible = true;
         this.MovementManulPanel.Visible = false;

         this.UpdateMovementControls();
      }

      #endregion

      #region Drill Event Process

      private void DrillSealModeButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         this.UpdateSealantControls();

         this.SealantMainPanel.Left = this.DrillMainPanel.Left;
         this.SealantMainPanel.Top = this.DrillMainPanel.Top;
         this.SealantMainPanel.Visible = true;

         this.SealantManualPanel.Left = this.SealantMainPanel.Left;
         this.SealantManualPanel.Top = this.SealantMainPanel.Top + this.SealantMainPanel.Height + 8;
         this.SealantManualPanel.Visible = this.pumpManualVisible;

         this.DrillMainPanel.Visible = false;
         this.DrillManualPanel.Visible = false;


         e.Handled = true;
      }

      private void DrillExtendedValuePanel_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         DialogResult result = this.LaunchNumberEdit(this.DrillExtendedSetPointValuePanel, "INDEX POSITION", this.selectedDrill.ExtendedDistance);

         if (result == System.Windows.Forms.DialogResult.OK)
         {
            NicBotComm.Instance.SetDrillIndexSetPoint(this.toolLocation, this.selectedDrill.ExtendedDistance.OperationalValue);
            //this.DrillExtendedSetPointValuePanel.ValueText = this.GetValueText(this.selectedDrill.ExtendedDistance);
         }

         e.Handled = true;
      }

      private void DrillRotaionSpeedValuePanel_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         DialogResult result = this.LaunchNumberEdit(this.DrillRotaionSetPointSpeedValuePanel, "DRILL SPEED", this.selectedDrill.RotationSpeed);

         if (System.Windows.Forms.DialogResult.OK == result)
         {
            if (false != this.drillManualActivated)
            {
               NicBotComm.Instance.SetDrillRotationSpeed(this.toolLocation, this.selectedDrill.RotationSpeed.OperationalValue);
            }

            this.DrillRotaionSetPointSpeedValuePanel.ValueText = this.GetValueText(this.selectedDrill.RotationSpeed);
         }

         e.Handled = true;
      }

      private void DrillLaserLightButton_Click(object sender, EventArgs e)
      {  
         bool request =!this.DrillLaserLightButton.OptionASelected;
         NicBotComm.Instance.SetLaserSight(this.toolLocation, request);
         this.DrillLaserLightButton.OptionASelected = request;
      }

      private void DrillSetupButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         DrillSetupForm drillSetupForm = new DrillSetupForm();
         this.SetDialogLocation(this.DrillSetupButton, drillSetupForm);

         drillSetupForm.FrontDrillParameters = ParameterAccessor.Instance.FrontDrill;         
         drillSetupForm.RearDrillParameters = ParameterAccessor.Instance.RearDrill;
         drillSetupForm.DrillLocation = this.toolLocation;

         this.DimBackground();
         drillSetupForm.ShowDialog();
         this.LightBackground();

         this.toolLocation = drillSetupForm.DrillLocation;
         this.SetDrillSelection(this.toolLocation);
         NicBotComm.Instance.ConfigureDrillServo(this.toolLocation);

         e.Handled = true;
      }

      private void DrillManulDisplayButton_Click(object sender, EventArgs e)
      {
         if (false == this.DrillManualPanel.Visible)
         {
            this.DrillManualPanel.Visible = true;
            this.DrillManulDisplayButton.Text = "HIDE MANUAL";
         }
         else
         {
            this.DrillManualPanel.Visible = false;
            this.DrillManulDisplayButton.Text = "SHOW MANUAL";
         }

         this.drillManualVisible = this.DrillManualPanel.Visible;
      }

      private void DrillFindOriginButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         NicBotComm.Instance.FindDrillOrigin(this.toolLocation);
      }

      private void DrillSetOriginButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         NicBotComm.Instance.SetDrillOriginOffset(this.toolLocation);
         double originOffset = NicBotComm.Instance.GetDrillOriginOffset(this.toolLocation);
         NicBotComm.Instance.SetDrillIndexSetPoint(this.toolLocation, originOffset);
      }

      private void DrillManualToggleButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         bool selection = !this.DrillManualToggleButton.OptionASelected;
         this.drillManualActivated = selection;

         if (false != this.drillManualActivated)
         {
            NicBotComm.Instance.SetDrillRotationSpeed(this.toolLocation, this.selectedDrill.RotationSpeed.OperationalValue);
         }
         else
         {
            NicBotComm.Instance.SetDrillRotationSpeed(this.toolLocation, 0);
         }

         e.Handled = true;
      }

      private void DrillAutoStartButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         NicBotComm.Instance.StartAutoDrill(this.toolLocation);
         this.UpdateDrillControls();
      }

      private void DrillAutoPauseResumeButton_Click(object sender, EventArgs e)
      {
         DrillAutoStates drillAutoState = NicBotComm.Instance.GetDrillAutoState();

         if (DrillAutoStates.running == drillAutoState)
         {
            NicBotComm.Instance.PauseAutoDrill();
         }
         else
         {
            NicBotComm.Instance.ResumeAutoDrill();
         }

         this.UpdateDrillControls();
      }

      private void DrillAutoStopButton_Click(object sender, EventArgs e)
      {
         NicBotComm.Instance.StopAutoDrill();
         this.UpdateDrillControls();
      }

      private void DrillRetractToLimitButton_Click(object sender, EventArgs e)
      {
         NicBotComm.Instance.RetractDrillToLimit(this.toolLocation); 
         this.selectedDrill.ExtendedDistance.OperationalValue = 0;
      }

      private void DrillMoveToOriginButton_Click(object sender, EventArgs e)
      {
         double originOffset = NicBotComm.Instance.GetDrillOriginOffset(this.toolLocation);

         if (double.IsNaN(originOffset) == false)
         {
            NicBotComm.Instance.SetDrillIndexSetPoint(this.toolLocation, originOffset);
            this.selectedDrill.ExtendedDistance.OperationalValue = originOffset;
         }
      }

      private void DrillDirectionToggleButton_Click(object sender, EventArgs e)
      {
         this.DrillDirectionToggleButton.OptionASelected = !this.DrillDirectionToggleButton.OptionASelected;
      }

      private void DrillStopButton_Click(object sender, EventArgs e)
      {
         NicBotComm.Instance.StopDrill(this.toolLocation);
      }

      private void DrillIndexUpButton_Click(object sender, EventArgs e)
      {
         this.DecrementDrillIndex();
      }

      private void DrillIndexUpButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         this.DecrementDrillIndex();
         e.Handled = true;
      }

      private void DrillIndexDownButton_Click(object sender, EventArgs e)
      {
         this.IncrementDrillIndex();
      }

      private void DrillIndexDownButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         this.IncrementDrillIndex();
         e.Handled = true;
      }

      private void DrillSpeedIncreaseButton_Click(object sender, EventArgs e)
      {
         this.IncrementDrillSpeed();
      }

      private void DrillSpeedIncreaseButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         this.IncrementDrillSpeed();
         e.Handled = true;
      }

      private void DrillSpeedDecreaseButton_Click(object sender, EventArgs e)
      {
         this.DecrementDrillSpeed();
      }

      private void DrillSpeedDecreaseButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         this.DecrementDrillSpeed();
         e.Handled = true;
      }

      #endregion

      #region Sealant and Pump Event Process

      private void SealDrillModeButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         this.SetDrillSelection(this.toolLocation);

         this.DrillMainPanel.Left = this.SealantMainPanel.Left;
         this.DrillMainPanel.Top = this.SealantMainPanel.Top;
         this.DrillMainPanel.Visible = true;

         this.DrillManualPanel.Left = this.DrillMainPanel.Left;
         this.DrillManualPanel.Top = this.DrillMainPanel.Top + this.DrillMainPanel.Height + 8;
         this.DrillManualPanel.Visible = this.drillManualVisible;

         this.SealantMainPanel.Visible = false;
         this.SealantManualPanel.Visible = false;

         e.Handled = true;
      }

      private void SealantPressureSetPointValuePanel_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         PumpParameters pumpParameters = (ToolLocations.front == this.toolLocation) ? ParameterAccessor.Instance.FrontPump : ParameterAccessor.Instance.RearPump;
         ValueParameter pumpPressure = new ValueParameter(pumpParameters.MaximumPressure);
         pumpPressure.OperationalValue = NicBotComm.Instance.GetPumpSetPoint(this.toolLocation);

         DialogResult result = this.LaunchNumberEdit(this.SealantPressureSetPointValuePanel, "PUMP PRESSURE", pumpPressure);

         if (result == System.Windows.Forms.DialogResult.OK)
         {
            NicBotComm.Instance.SetPumpPressure(this.toolLocation, pumpPressure.OperationalValue);
            this.UpdateSealantControls();
         }

         e.Handled = true;
      }

      private void SealantSpeedSetPointValuePanel_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         PumpParameters pumpParameters = (ToolLocations.front == this.toolLocation) ? ParameterAccessor.Instance.FrontPump : ParameterAccessor.Instance.RearPump;
         ValueParameter pumpSpeed = new ValueParameter(pumpParameters.MaximumSpeed);
         pumpSpeed.OperationalValue = NicBotComm.Instance.GetPumpSetPoint(this.toolLocation);
         pumpSpeed.MaximumValue = pumpParameters.MaximumSpeed.OperationalValue;

         DialogResult result = this.LaunchNumberEdit(this.SealantPressureSetPointValuePanel, "PUMP SPEED", pumpSpeed);

         if (result == System.Windows.Forms.DialogResult.OK)
         {
            NicBotComm.Instance.SetPumpSpeed(this.toolLocation, pumpSpeed.OperationalValue);
            this.UpdateSealantControls();
         }

         e.Handled = true;
      }

      private void SealantLaserLightButton_Click(object sender, EventArgs e)
      {
         bool request = !this.SealantLaserLightButton.OptionASelected;
         NicBotComm.Instance.SetLaserSight(this.toolLocation, request);
         this.SealantLaserLightButton.OptionASelected = request;
      }

      private void SealantSetupButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         SealantSetupForm sealantSetupForm = new SealantSetupForm();
         this.SetDialogLocation(this.SealantSetupButton, sealantSetupForm);

         sealantSetupForm.Pump = (ToolLocations.front == this.toolLocation) ? ParameterAccessor.Instance.FrontPump : ParameterAccessor.Instance.RearPump;
         sealantSetupForm.ToolLocation = this.toolLocation;

         this.DimBackground();
         sealantSetupForm.ShowDialog();
         this.LightBackground();

         this.toolLocation = sealantSetupForm.ToolLocation;
         this.UpdateSealantControls();

         e.Handled = true;
      }

      private void SealantManulDisplayButton_Click(object sender, EventArgs e)
      {
         if (false == this.SealantManualPanel.Visible)
         {
            this.SealantManualPanel.Visible = true;
            this.SealantManulDisplayButton.Text = "HIDE MANUAL";
         }
         else
         {
            this.SealantManualPanel.Visible = false;
            this.SealantManulDisplayButton.Text = "SHOW MANUAL";
         }

         this.pumpManualVisible = this.SealantManualPanel.Visible;
      }

      private void SealantAutoStartButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         NicBotComm.Instance.StartAutoPump(this.toolLocation);
         this.UpdateSealantControls();
      }

      private void SealantAutoPauseResumeButton_Click(object sender, EventArgs e)
      {
         PumpAutoStates pumpAutoState = NicBotComm.Instance.GetPumpAutoState(this.toolLocation);

         if (PumpAutoStates.running == pumpAutoState)
         {
            NicBotComm.Instance.PauseAutoPump(this.toolLocation);
         }
         else
         {
            NicBotComm.Instance.ResumeAutoPump(this.toolLocation);
         }

         this.UpdateSealantControls();
      }

      private void SealantAutoStopButton_Click(object sender, EventArgs e)
      {
         NicBotComm.Instance.StopAutoPump(this.toolLocation);
         this.UpdateSealantControls();
      }

      private void SealantNozzleToggleButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         bool extended = NicBotComm.Instance.GetNozzleExtended(this.toolLocation);

         if (false != extended)
         {
            NicBotComm.Instance.SetNozzleExtend(this.toolLocation, false);
         }
         else
         {
            NicBotComm.Instance.SetNozzleExtend(this.toolLocation, true);
         }

         this.UpdateSealantControls();
      }

      private void SealantRelievePressureButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         NicBotComm.Instance.RelievePumpPressure(this.toolLocation);
      }

      private void SealantManualPumpToggleButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         bool selection = !this.SealantManualPumpToggleButton.OptionASelected;
         this.pumpManualActivated = selection;
         this.UpdateSealantControls();

         if (false != this.pumpManualActivated)
         {
            if (false != this.SealantManualModeToggleButton.OptionASelected)
            {
               double pumpPressure = NicBotComm.Instance.GetPumpPressureReading(this.toolLocation);
               NicBotComm.Instance.SetPumpPressure(this.toolLocation, pumpPressure);
            }
            else
            {
               PumpDirections pumpDirection = (false != this.SealantDirectionToggleButton.OptionASelected) ? PumpDirections.forward : PumpDirections.reverse;
               NicBotComm.Instance.SetPumpDirection(this.toolLocation, pumpDirection);

               NicBotComm.Instance.SetPumpSpeed(this.toolLocation, 0);
            }

            NicBotComm.Instance.StartPump(this.toolLocation);
         }
         else
         {
            NicBotComm.Instance.StopPump(this.toolLocation);
        }
      }

      private void SealantPressureIncreaseButton_Click(object sender, EventArgs e)
      {
         this.IncreaseSealantPressure();
      }

      private void SealantPressureIncreaseButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         this.IncreaseSealantPressure();
         e.Handled = true;
      }

      private void SealantPressureDecreaseButton_Click(object sender, EventArgs e)
      {
         this.DecreaseSealantPressure();
      }

      private void SealantPressureDecreaseButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         this.DecreaseSealantPressure();
         e.Handled = true;
      }

      private void SealantManualModeToggleButton_Click(object sender, EventArgs e)
      {
         bool selected = !this.SealantManualModeToggleButton.OptionASelected;
         this.SealantManualModeToggleButton.OptionASelected = selected;
      }
      
      private void SealantDirectionToggleButton_Click(object sender, EventArgs e)
      {
         bool selected = !this.SealantDirectionToggleButton.OptionASelected;
         this.SealantDirectionToggleButton.OptionASelected = selected;
      }

      private void SealantSpeedIncreaseButton_Click(object sender, EventArgs e)
      {
         this.IncreaseSealantPumpSpeed();
      }

      private void SealantSpeedIncreaseButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         this.IncreaseSealantPumpSpeed();
         e.Handled = true;
      }

      private void SealantSpeedDecreaseButton_Click(object sender, EventArgs e)
      {
         this.DecreaseSealantPumpSpeed();
      }

      private void SealantSpeedDecreaseButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         this.DecreaseSealantPumpSpeed();
         e.Handled = true;
      }

      #endregion
      
      #region Sensor Event Process

      private void SensorLatitudeTextPanel_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         double latitude = ParameterAccessor.Instance.Latitude;
         DialogResult result = this.LaunchNumberEdit(ref latitude, this.SensorLatitudeTextPanel, "LATITUDE", 4, "°", 40.812838, -90, 90);

         if (result == System.Windows.Forms.DialogResult.OK)
         {
            ParameterAccessor.Instance.Latitude = latitude;
            this.SensorLatitudeTextPanel.ValueText = latitude.ToString("N4");
         }
      }

      private void SensorLongitudeTextPanel_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         double longitude = ParameterAccessor.Instance.Longitude;
         DialogResult result = this.LaunchNumberEdit(ref longitude, this.SensorLongitudeTextPanel, "LONGITUDE", 4, "°", -73.254690, -180, 180);

         if (result == System.Windows.Forms.DialogResult.OK)
         {
            ParameterAccessor.Instance.Longitude = longitude;
            this.SensorLongitudeTextPanel.ValueText = longitude.ToString("N4");
         }
      }

      private void SensorDirectionTextPanel_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         DirectionEntryForm directionEntryForm = new DirectionEntryForm();
         this.SetDialogLocation(this.SensorDirectionTextPanel, directionEntryForm);

         directionEntryForm.Direction = this.sensorDirection;

         this.DimBackground();
         DialogResult result = directionEntryForm.ShowDialog();
         this.LightBackground();

         if (System.Windows.Forms.DialogResult.OK == result)
         {
            this.sensorDirection = directionEntryForm.Direction;
            this.SensorDirectionTextPanel.ValueText = this.sensorDirection.ToString().ToUpper();
            this.SensorDirectionTextPanel.BackColor = Color.FromArgb(51, 51, 51);
         }
      }

      private void SensorThicknessAcquireButton_Click(object sender, EventArgs e)
      {
         double latitude = 0;
         double longitude = 0;
         DateTime dateTime = default(DateTime);
         Directions direction = Directions.unknown;
         double displacement = 0;
         double radialLocation = 0;

         bool validData = this.LocationDataProvider(ref latitude, ref longitude, ref dateTime, ref direction, ref displacement, ref radialLocation);

         if (false != validData)
         {
            NicBotComm.Instance.TriggerThicknessReading(latitude, longitude, dateTime, direction, displacement, radialLocation);

            this.SensorThicknessAcquireButton.Enabled = false;
            this.SensorDirectionTextPanel.Enabled = false;
            this.SensorThicknessReadingTextPanel.ValueText = "";
            this.sensorThicknessPending = true;
         }
      }

      private void SensorStressAcquireButton_Click(object sender, EventArgs e)
      {
         double latitude = 0;
         double longitude = 0;
         DateTime dateTime = default(DateTime);
         Directions direction = Directions.unknown;
         double displacement = 0;
         double radialLocation = 0;

         bool validData = this.LocationDataProvider(ref latitude, ref longitude, ref dateTime, ref direction, ref displacement, ref radialLocation);

         if (false != validData)
         {
            NicBotComm.Instance.TriggerStressReading(latitude, longitude, dateTime, direction, displacement, radialLocation);

            this.SensorStressAcquireButton.Enabled = false;
            this.SensorDirectionTextPanel.Enabled = false;
            this.SensorStressReadingTextPanel.ValueText = "";
            this.sensorStressPending = true;
         }
      }

      #endregion

      #region Video Event Process
      
      private void LaunchCameraSelectButton_Click(object sender, EventArgs e)
      {
         if (CameraSelectModes.launchCamera == this.cameraSelectMode)
         {
            this.cameraSelectMode = CameraSelectModes.none;
         }
         else
         {
            this.cameraSelectMode = CameraSelectModes.launchCamera;
         }

         this.UpdateCameraHoldEnable();
         this.UpdateCameraSelectorColor();
      }

      private void RobotCameraASelectButton_Click(object sender, EventArgs e)
      {
         if (CameraSelectModes.robotCameraA == this.cameraSelectMode)
         {
            this.cameraSelectMode = CameraSelectModes.none;
         }
         else
         {
            this.cameraSelectMode = CameraSelectModes.robotCameraA;
         }

         this.UpdateCameraHoldEnable();
         this.UpdateCameraSelectorColor();
      }

      private void LightSelectButton_Click(object sender, EventArgs e)
      {
         if (CameraSelectModes.light == this.cameraSelectMode)
         {
            this.cameraSelectMode = CameraSelectModes.none;
         }
         else
         {
            this.cameraSelectMode = CameraSelectModes.light;
         }

         this.UpdateCameraHoldEnable();
         this.UpdateCameraSelectorColor();
      }
            
      private void RobotCameraBSelectButton_Click(object sender, EventArgs e)
      {
         if (CameraSelectModes.robotCameraB == this.cameraSelectMode)
         {
            this.cameraSelectMode = CameraSelectModes.none;
         }
         else
         {
            this.cameraSelectMode = CameraSelectModes.robotCameraB;
         }

         this.UpdateCameraHoldEnable();
         this.UpdateCameraSelectorColor();
      }

      private void LaunchCameraButton_MouseClick(object sender, MouseEventArgs e)
      {
         CameraSelectButton button = (CameraSelectButton)sender;

         if (CameraSelectModes.launchCamera == this.cameraSelectMode)
         {
            this.AssignLaunchCamera(button);
         }
         else if (CameraSelectModes.light == this.cameraSelectMode)
         {
            button.CenterVisible = !button.CenterVisible;

            if (false != button.CenterVisible)
            {
               NicBotComm.Instance.SetCameraLightLevel(button.Camera, button.CenterLevel);
            }
            else
            {
               NicBotComm.Instance.SetCameraLightLevel(button.Camera, 0);
            }
         }
      }

      private void RobotCameraButton_MouseClick(object sender, MouseEventArgs e)
      {
         CameraSelectButton button = (CameraSelectButton)sender;

         if (CameraSelectModes.robotCameraA == this.cameraSelectMode)
         {
            this.AssignRobotCameraA(button);
         }
         else if (CameraSelectModes.light == this.cameraSelectMode)
         {
            button.CenterVisible = !button.CenterVisible;

            if (false != button.CenterVisible)
            {
               NicBotComm.Instance.SetCameraLightLevel(button.Camera, button.CenterLevel);
            }
            else
            {
               NicBotComm.Instance.SetCameraLightLevel(button.Camera, 0);
            }
         }
         else if (CameraSelectModes.robotCameraB == this.cameraSelectMode)
         {
            this.AssignRobotCameraB(button);
         }
      }

      private void CameraButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         CameraSelectButton button = (CameraSelectButton)sender;

         if (CameraSelectModes.light == this.cameraSelectMode)
         {
            ValueParameter value = ParameterAccessor.Instance.GetLightValue(button.Camera);

            if (null != value)
            {
               if (false == button.CenterVisible)
               {
                  button.CenterVisible = true;
                  NicBotComm.Instance.SetCameraLightLevel(button.Camera, button.CenterLevel);
               }

               LightIntensitySelectForm intensityForm = new LightIntensitySelectForm();
               this.SetDialogLocation(button, intensityForm);
               intensityForm.LocationText = button.Text;
               intensityForm.IntensityValue = value;
               intensityForm.Camera = button.Camera;
               this.DimBackground();
               intensityForm.ShowDialog();
               this.LightBackground();

               button.CenterLevel = (int)intensityForm.IntensityValue.OperationalValue;
            }
         }

         e.Handled = true;
      }

      #endregion

      #region System User Actions

      private void WriteOsdButton_Click(object sender, EventArgs e)
      {
         Button button = (Button)sender;

         OsdForm osdForm = new OsdForm();

         this.SetDialogLocation(button, osdForm);
         this.DimBackground();
         osdForm.ShowDialog();
         this.LightBackground();
      }

      private void SystemStatusButton_Click(object sender, EventArgs e)
      {
         Button button = (Button)sender;

         SystemStatusForm systemStatusForm = new SystemStatusForm();
         systemStatusForm.SystemResetHandler = new SystemStatusForm.SystemResetDelegate(this.RestartSystem);
         systemStatusForm.TraceListenerDestination = new SystemStatusForm.TraceListenerDestinationDelegate(this.SetTraceListenerDestination);

         this.SetDialogLocation(button, systemStatusForm);
         this.DimBackground();
         systemStatusForm.ShowDialog();
         this.LightBackground();
      }

      private void SystemResetButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         this.RestartSystem();
      }

      private void ExitButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         this.processExitNeeded = true;
         this.processStopNeeded = true;
         //this.StopProcess();
         //this.Close();
      }

      #endregion

      #region Form Events Process 

      private void MainForm_Shown(object sender, EventArgs e)
      {
         this.Process = this.ProcessStart;
         this.UpdateTimer.Interval = 1;
         this.UpdateTimer.Enabled = true;

#if false
         this.PopupTransparentPanel.Top = this.Top;
         this.PopupTransparentPanel.Left = this.Left;
         this.PopupTransparentPanel.BringToFront();
         this.PopupTransparentPanel.Height = this.Height;
         this.PopupTransparentPanel.Width = this.Width-30;
         this.PopupTransparentPanel.Visible = false;
#endif
      }
            
      private void UpdateTimer_Tick(object sender, EventArgs e)
      {
         this.TickProcess();
      }

      private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
      {
         if (false == this.processStopped)
         {
            //this.StopProcess();
            this.processExitNeeded = true;
            this.processStopNeeded = true;
            e.Cancel = true;
         }
      }

      private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
      {
         //this.StopProcess();
      }

      #endregion

      #region Constructor 

      public MainForm()
      {
         this.InitializeComponent();

         this.fileTraceListener = new FileTraceListener();
         this.fileTraceListener.LogFilePath = @"c:\logs\nicbot";
         this.fileTraceListener.MaximumLines = 10000;
         this.fileTraceListener.Prefix = "NICBOT_";
         Trace.Listeners.Add(this.fileTraceListener);

         this.traceListener = new UdpTraceListener("127.0.0.1", 10000);
         Trace.Listeners.Add(this.traceListener);

         Tracer.MaskString = "FFFFFFFF";

         this.cameraButtons = new CameraSelectButton[16];
         this.cameraButtons[0] = this.RobotCamera1Button;
         this.cameraButtons[1] = this.RobotCamera2Button;
         this.cameraButtons[2] = this.RobotCamera3Button;
         this.cameraButtons[3] = this.RobotCamera4Button;
         this.cameraButtons[4] = this.RobotCamera5Button;
         this.cameraButtons[5] = this.RobotCamera6Button;
         this.cameraButtons[6] = this.RobotCamera7Button;
         this.cameraButtons[7] = this.RobotCamera8Button;
         this.cameraButtons[8] = this.RobotCamera9Button;
         this.cameraButtons[9] = this.RobotCamera10Button;
         this.cameraButtons[10] = this.RobotCamera11Button;
         this.cameraButtons[11] = this.RobotCamera12Button;
         this.cameraButtons[12] = this.LaunchCamera1Button;
         this.cameraButtons[13] = this.LaunchCamera2Button;
         this.cameraButtons[14] = this.LaunchCamera3Button;
         this.cameraButtons[15] = this.LaunchCamera4Button;

         this.dimmerForm = new PopupDimmerForm();
         this.dimmerForm.Opacity = 0.65;
         this.dimmerForm.ShowInTaskbar = false;
         
         LocationServer.Instance.OnLocationRequest = new LocationServer.LocationHandler(this.LocationDataProvider);
         LocationServer.Instance.OnThicknessReading = new LocationServer.ThicknessReadingHandler(this.ReceiveThicknessReading);
         LocationServer.Instance.OnStressReading = new LocationServer.StressReadingHandler(this.ReceiveStressReading);
      }

      #endregion      

   }
}
