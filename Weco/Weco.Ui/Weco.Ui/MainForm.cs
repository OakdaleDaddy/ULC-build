
namespace Weco.Ui
{
   using System;
   using System.Collections.Generic;
   using System.ComponentModel;
   using System.Data;
   using System.Diagnostics;
   using System.Drawing;
   using System.Linq;
   using System.Text;
   using System.Threading.Tasks;
   using System.Windows.Forms;

   using Weco.Utilities;

   public partial class MainForm : Form
   {
      #region Definitions

      private delegate void ProcessHandler();

      private enum JoystickApplications
      {
         none,
         laserRobot,
         targetRobot,
         topCamera,
      }

      private enum SessionStates
      {
         idle,
         start,
         firstMeasure,
         move,
         measure,
      }

#if false
      private enum VideoSelectModes
      {
         none,
         light,
         laserCamera,
         targetCamera,
      }
#endif

      #endregion

      #region Fields

      private FileTraceListener fileTraceListener;
      private UdpTraceListener traceListener;

      private bool processStopNeeded;
      private bool processExitNeeded;
      private bool processStopped;

      private bool indicatorFlasher;
      private bool messageFlasher;
      private int messageFlashCount;

      private JoystickApplications joystickApplication;

      private bool laserMovementFastSelected;

      private bool targetMovementFastSelected;

      private SessionStates sessionState;
      private bool measurementRequested;
      private bool measurementStarted;
      private bool measurementRecorded;
      private double measurement;

      private bool laserStepperFaultProcessed;
      private int laserLeftStepperChange;
      private int laserRightStepperChange;

      private bool topCameraStepperFaultProcessed;
      private int topCameraStepperChange;
      private int topCameraRequestedChange;

      //private VideoSelectModes videoSelectMode;
      private bool laserLightEnabled;
      private bool targetLightEnabled;
      private Controls.CameraSelectButton selectedLaserCameraButton;
      private Controls.CameraSelectButton selectedTargetCameraButton;
      private Controls.CameraSelectButton[] cameraButtons;

      private PopupDimmerForm dimmerForm;

      #endregion

      #region Properties

      private ProcessHandler Process { set; get; }

      #endregion

      #region Helper Functions

      #region General

      private string GetValuePrecisionFormatString(int precision)
      {
         string result = "{0:0";

         if (precision > 0)
         {
            result += ".";

            for (int i = 0; i < precision; i++)
            {
               result += "0";
            }
         }

         result += "} {1}";

         return (result);
      }

      private string GetValueText(ValueParameter parameter)
      {
         string formatString = this.GetValuePrecisionFormatString(parameter.Precision);
         string result = string.Format(formatString, parameter.OperationalValue, parameter.Unit);
         return (result);
      }

      private string GetValueText(double operationalValue, ValueParameter parameter)
      {
         string formatString = this.GetValuePrecisionFormatString(parameter.Precision);
         string result = string.Format(formatString, operationalValue, parameter.Unit);
         return (result);
      }

      private string GetValueText(double value, int precision, string unit)
      {
         string formatString = this.GetValuePrecisionFormatString(precision);
         string result = string.Format(formatString, value, unit);
         return (result);
      }

      private string GetValueText(int value, string unit = "")
      {
         string result = value.ToString();

         if ("" != unit)
         {
            result += (" " + unit);
         }

         return (result);
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

      private DialogResult LaunchNumberEdit(Controls.ValueButton button, ValueParameter valueParameter, string title = null)
      {
         NumberEntryForm numberEntryForm = new NumberEntryForm();
         this.SetDialogLocation(button, numberEntryForm);

         numberEntryForm.Title = (null != title) ? title : button.Text;
         numberEntryForm.Unit = valueParameter.Unit;
         numberEntryForm.PostDecimalDigitCount = valueParameter.Precision;
         numberEntryForm.PresentValue = valueParameter.OperationalValue;
         numberEntryForm.DefaultValue = valueParameter.DefaultValue;
         numberEntryForm.MinimumValue = valueParameter.MinimumValue;
         numberEntryForm.MaximumValue = valueParameter.MaximumValue;

         this.DimBackground();
         DialogResult result = numberEntryForm.ShowDialog();
         this.LightBackground();

         if (result == System.Windows.Forms.DialogResult.OK)
         {
            valueParameter.OperationalValue = numberEntryForm.EnteredValue;
            button.ValueText = this.GetValueText(valueParameter);
         }

         return (result);
      }

      private void RestartSystem()
      {
         this.processStopNeeded = true;
      }

      private void SetTraceListenerDestination()
      {
         this.traceListener.SetDestination(ParameterAccessor.Instance.Trace.Address, ParameterAccessor.Instance.Trace.Port);
         Tracer.WriteHigh(TraceGroup.UI, null, "endpoint set");
      }

      #endregion

      #region Joystick 

      private void UpdateJoystickApplicationButtons()
      {
         if (JoystickApplications.none == this.joystickApplication)
         {
            this.LaserRobotMovementJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.LaserRobotMovementJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);

            this.TargetRobotMovementJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.TargetRobotMovementJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);

            this.TopCameraJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.TopCameraJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);
         }
         else if (JoystickApplications.laserRobot == this.joystickApplication)
         {
            this.LaserRobotMovementJoystickEnableButton.Text = "MANUAL DRIVE";
            this.LaserRobotMovementJoystickEnableButton.BackColor = Color.Lime;

            this.TargetRobotMovementJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.TargetRobotMovementJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);

            this.TopCameraJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.TopCameraJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);
         }
         else if (JoystickApplications.targetRobot == this.joystickApplication)
         {
            this.LaserRobotMovementJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.LaserRobotMovementJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);
            
            this.TargetRobotMovementJoystickEnableButton.Text = "MANUAL DRIVE";
            this.TargetRobotMovementJoystickEnableButton.BackColor = Color.Lime;

            this.TopCameraJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.TopCameraJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);
         }
         else if (JoystickApplications.topCamera == this.joystickApplication)
         {
            this.LaserRobotMovementJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.LaserRobotMovementJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);

            this.TargetRobotMovementJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.TargetRobotMovementJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);

            this.TopCameraJoystickEnableButton.Text = "MANUAL DRIVE";
            this.TopCameraJoystickEnableButton.BackColor = Color.Lime;
         }
      }

      #endregion

      #region Laser Robot 

      private void LaserUpdateMovementControls()
      {
         MovementModes laserMovementMode = DeviceCommunication.Instance.GetLaserMovementMode();
 
         if (MovementModes.off == laserMovementMode)
         {
            this.LaserRobotWheelOffButton.BackColor = Color.Lime;
            this.LaserRobotWheelMoveButton.BackColor = Color.FromArgb(171, 171, 171);
         }
         else if (MovementModes.move == laserMovementMode)
         {
            this.LaserRobotWheelMoveButton.BackColor = Color.Lime;
            this.LaserRobotWheelOffButton.BackColor = Color.FromArgb(171, 171, 171);

            this.LaserRobotWheelOffButton.HoldTimeoutEnable = false;
         }
         else if (MovementModes.locked == laserMovementMode)
         {
            this.LaserRobotWheelOffButton.BackColor = Color.FromArgb(171, 171, 171);
            this.LaserRobotWheelMoveButton.BackColor = Color.FromArgb(171, 171, 171);

            this.LaserRobotWheelOffButton.HoldTimeoutEnable = true;
         }
      }
      
      #endregion

      #region Target Robot 

      private void TargetUpdateMovementControls()
      {
         MovementModes targetMovementMode = DeviceCommunication.Instance.GetTargetMovementMode();

         if (MovementModes.off == targetMovementMode)
         {
            this.TargetWheelOffButton.BackColor = Color.Lime;
            this.TargetWheelMoveButton.BackColor = Color.FromArgb(171, 171, 171);
         }
         else if (MovementModes.move == targetMovementMode)
         {
            this.TargetWheelMoveButton.BackColor = Color.Lime;
            this.TargetWheelOffButton.BackColor = Color.FromArgb(171, 171, 171);

            this.TargetWheelOffButton.HoldTimeoutEnable = false;
         }
         else if (MovementModes.locked == targetMovementMode)
         {
            this.TargetWheelOffButton.BackColor = Color.FromArgb(171, 171, 171);
            this.TargetWheelMoveButton.BackColor = Color.FromArgb(171, 171, 171);

            this.TargetWheelOffButton.HoldTimeoutEnable = true;
         }
      }

      #endregion

      #region Measurement

      private void UpdateSessionControls()
      {
         string laserRobotFrontWheelFault = LaserCommunicationBus.Instance.GetFaultStatus(LaserCommunicationBus.BusComponentId.LaserBoardFrontWheel);
         string laserRobotRearWheelFault = LaserCommunicationBus.Instance.GetFaultStatus(LaserCommunicationBus.BusComponentId.LaserBoardRearWheel);
         bool laserRobotFaulted = ((null != laserRobotFrontWheelFault) || (null != laserRobotRearWheelFault)) ? true : false;
         bool laserRobotUnlocked = ((DeviceCommunication.Instance.GetLaserMovementLock() == false) && (false == laserRobotFaulted)) ? true : false;
         bool laserMoved = DeviceCommunication.Instance.GetLaserMoved();
         bool laserRelayChanged = NumatoUsbRelay.Instance.GetChanged(0) || NumatoUsbRelay.Instance.GetChanged(1);

         string targetRobotFrontWheelFault = TargetCommunicationBus.Instance.GetFaultStatus(TargetCommunicationBus.BusComponentId.TargetBoardFrontWheel);
         string targetRobotRearWheelFault = TargetCommunicationBus.Instance.GetFaultStatus(TargetCommunicationBus.BusComponentId.TargetBoardRearWheel);
         bool targetRobotFaulted = ((null != targetRobotFrontWheelFault) || (null != targetRobotRearWheelFault)) ? true : false;
         bool targetRobotUnlocked = ((DeviceCommunication.Instance.GetTargetMovementLock() == false) && (false == targetRobotFaulted)) ? true : false;
         bool targetMoved = DeviceCommunication.Instance.GetTargetMoved();
         bool targetRelayChanged = NumatoUsbRelay.Instance.GetChanged(2) || NumatoUsbRelay.Instance.GetChanged(3);

         if (SessionStates.idle == this.sessionState)
         {
            this.LaserRobotLockButton.Enabled = true;
            this.LaserRobotLeftButton.Enabled = laserRobotUnlocked;
            this.LaserRobotRightButton.Enabled = laserRobotUnlocked;
            this.LaserRobotJogReverseButton.Enabled = laserRobotUnlocked;
            this.LaserRobotJogForwardButton.Enabled = laserRobotUnlocked;
            this.LaserRobotMoveReverseButton.Enabled = laserRobotUnlocked;
            this.LaserRobotMoveForwardButton.Enabled = laserRobotUnlocked;
            this.LaserRobotWheelOffButton.Enabled = laserRobotUnlocked;
            this.LaserRobotWheelMoveButton.Enabled = laserRobotUnlocked;

            this.TargetRobotLockButton.Enabled = true;
            this.TargetRobotLeftButton.Enabled = targetRobotUnlocked;
            this.TargetRobotRightButton.Enabled = targetRobotUnlocked;
            this.TargetRobotJogReverseButton.Enabled = targetRobotUnlocked;
            this.TargetRobotJogForwardButton.Enabled = targetRobotUnlocked;
            this.TargetRobotMoveReverseButton.Enabled = targetRobotUnlocked;
            this.TargetRobotMoveForwardButton.Enabled = targetRobotUnlocked;
            this.TargetWheelOffButton.Enabled = targetRobotUnlocked;
            this.TargetWheelMoveButton.Enabled = targetRobotUnlocked;
         }
         else if ((SessionStates.start == this.sessionState) ||
                  (SessionStates.firstMeasure == this.sessionState) ||
                  (SessionStates.measure == this.sessionState))
         {
            this.LaserRobotLockButton.Enabled = false;
            this.LaserRobotLeftButton.Enabled = false;
            this.LaserRobotRightButton.Enabled = false;
            this.LaserRobotJogReverseButton.Enabled = false;
            this.LaserRobotJogForwardButton.Enabled = false;
            this.LaserRobotMoveReverseButton.Enabled = false;
            this.LaserRobotMoveForwardButton.Enabled = false;
            this.LaserRobotWheelOffButton.Enabled = false;
            this.LaserRobotWheelMoveButton.Enabled = false;

            this.TargetRobotLockButton.Enabled = false;
            this.TargetRobotLeftButton.Enabled = false;
            this.TargetRobotRightButton.Enabled = false;
            this.TargetRobotJogReverseButton.Enabled = false;
            this.TargetRobotJogForwardButton.Enabled = false;
            this.TargetRobotMoveReverseButton.Enabled = false;
            this.TargetRobotMoveForwardButton.Enabled = false;
            this.TargetWheelOffButton.Enabled = false;
            this.TargetWheelMoveButton.Enabled = false;
         }
         else if (SessionStates.move == this.sessionState)
         {
            this.LaserRobotLockButton.Enabled = !targetRobotUnlocked && !targetMoved && !targetRelayChanged; 
            this.LaserRobotLeftButton.Enabled = laserRobotUnlocked;
            this.LaserRobotRightButton.Enabled = laserRobotUnlocked;
            this.LaserRobotJogReverseButton.Enabled = laserRobotUnlocked;
            this.LaserRobotJogForwardButton.Enabled = laserRobotUnlocked;
            this.LaserRobotMoveReverseButton.Enabled = laserRobotUnlocked;
            this.LaserRobotMoveForwardButton.Enabled = laserRobotUnlocked;
            this.LaserRobotWheelOffButton.Enabled = laserRobotUnlocked;
            this.LaserRobotWheelMoveButton.Enabled = laserRobotUnlocked;

            this.TargetRobotLockButton.Enabled = !laserRobotUnlocked && !laserMoved && !laserRelayChanged;
            this.TargetRobotLeftButton.Enabled = targetRobotUnlocked;
            this.TargetRobotRightButton.Enabled = targetRobotUnlocked;
            this.TargetRobotJogReverseButton.Enabled = targetRobotUnlocked;
            this.TargetRobotJogForwardButton.Enabled = targetRobotUnlocked;
            this.TargetRobotMoveReverseButton.Enabled = targetRobotUnlocked;
            this.TargetRobotMoveForwardButton.Enabled = targetRobotUnlocked;
            this.TargetWheelOffButton.Enabled = targetRobotUnlocked;
            this.TargetWheelMoveButton.Enabled = targetRobotUnlocked;
         }
      }

      private void RecordMeasurement(double measurement, SessionMeasurementData.Types measurementType)
      {
         if (SessionStates.firstMeasure != this.sessionState)
         {
            double laserFront = DeviceCommunication.Instance.GetLaserWheelPositionValue(WheelLocations.front);
            double laserRear = DeviceCommunication.Instance.GetLaserWheelPositionValue(WheelLocations.rear);
            double targetFront = DeviceCommunication.Instance.GetTargetWheelPositionValue(WheelLocations.front);
            double targetRear = DeviceCommunication.Instance.GetTargetWheelPositionValue(WheelLocations.rear);
            SessionRecord.Instance.RecordMovement(laserFront, laserRear, targetFront, targetRear);
         }

         SessionRecord.Instance.RecordMeasurement(measurement, measurementType);
         
         this.measurementRecorded = true;
         this.RecordBetweenButton.Enabled = false;
         this.RecordServiceButton.Enabled = false;

         DeviceCommunication.Instance.ResetLaserMoved();
         DeviceCommunication.Instance.ResetTargetMoved();
         NumatoUsbRelay.Instance.ResetChanged();

         this.sessionState = SessionStates.move;
         this.UpdateSessionControls();
      }

      #endregion

      #region Video

#if false
      private void UpdateCameraHoldEnable()
      {
         for (int i = 0; i < this.cameraButtons.Length; i++)
         {
            bool holdEnabled = (VideoSelectModes.light == this.videoSelectMode) ? true : false;
            bool selectEnabled = true;

            if (VideoSelectModes.none == this.videoSelectMode)
            {
               selectEnabled = false;
            }
            else if ((VideoSelectModes.light == this.videoSelectMode) &&
                     (false == this.cameraButtons[i].CenterEnabled))
            {
               selectEnabled = false;
            }
            else if (VideoSelectModes.laserCamera == this.videoSelectMode)
            {
               if ((Ui.Controls.CameraLocations.laserFront != this.cameraButtons[i].Camera) &&
                   (Ui.Controls.CameraLocations.laserRear != this.cameraButtons[i].Camera))
               {
                  selectEnabled = false;
               }
            }
            else if (VideoSelectModes.targetCamera == this.videoSelectMode)
            {
               if ((Ui.Controls.CameraLocations.targetFront != this.cameraButtons[i].Camera) &&
                   (Ui.Controls.CameraLocations.targetRear!= this.cameraButtons[i].Camera) &&
                   (Ui.Controls.CameraLocations.targetTop != this.cameraButtons[i].Camera))
               {
                  selectEnabled = false;
               }
            }

            this.cameraButtons[i].Enabled = selectEnabled;
            this.cameraButtons[i].HoldTimeoutEnable = holdEnabled;
            this.cameraButtons[i].Invalidate();
         }
      }

      private void UpdateVideoSelectorColor()
      {
         if (VideoSelectModes.light == this.videoSelectMode)
         {
            this.LightSelectButton.BackColor = Color.Lime;
            this.LaserMonitorSelectButton.BackColor = Color.FromArgb(171, 171, 171);
            this.TargetMonitorSelectButton.BackColor = Color.FromArgb(171, 171, 171);
         }
         else if (VideoSelectModes.laserCamera == this.videoSelectMode)
         {
            this.LightSelectButton.BackColor = Color.FromArgb(171, 171, 171);
            this.LaserMonitorSelectButton.BackColor = Color.Lime;
            this.TargetMonitorSelectButton.BackColor = Color.FromArgb(171, 171, 171);
         }
         else if (VideoSelectModes.targetCamera == this.videoSelectMode)
         {
            this.LightSelectButton.BackColor = Color.FromArgb(171, 171, 171);
            this.LaserMonitorSelectButton.BackColor = Color.FromArgb(171, 171, 171);
            this.TargetMonitorSelectButton.BackColor = Color.Lime;
         }
         else
         {
            this.LightSelectButton.BackColor = Color.FromArgb(171, 171, 171);
            this.LaserMonitorSelectButton.BackColor = Color.FromArgb(171, 171, 171);
            this.TargetMonitorSelectButton.BackColor = Color.FromArgb(171, 171, 171);
         }
      }
#endif

      private void AssignLaserCamera(Controls.CameraSelectButton selected)
      {
         if (selected != this.selectedLaserCameraButton)
         {
            CameraSelectParameters cameraSelectParameters;

            if (null != this.selectedLaserCameraButton)
            {
               this.selectedLaserCameraButton.LeftVisible = false;
               cameraSelectParameters = ParameterAccessor.Instance.GetCameraSelectParameters(this.selectedLaserCameraButton.Camera);
               cameraSelectParameters.LightIntensity = DeviceCommunication.Instance.GetCameraLightLevel(this.selectedLaserCameraButton.Camera);
               //cameraSelectParameters.LightChannelMask = DeviceCommunication.Instance.GetCameraLightChannelMask(this.selectedLaserCameraButton.Camera);
            }

            cameraSelectParameters = ParameterAccessor.Instance.GetCameraSelectParameters(selected.Camera);
            int channelMask = (false != this.laserLightEnabled) ? cameraSelectParameters.LightChannelMask : 0;

            Tracer.WriteHigh(TraceGroup.UI, "", "laser camera {0}", selected.Camera.ToString());
            DeviceCommunication.Instance.SetLaserCamera(selected.Camera);
            DeviceCommunication.Instance.SetCameraLightLevel(selected.Camera, cameraSelectParameters.LightIntensity);
            DeviceCommunication.Instance.SetCameraLightChannelMask(selected.Camera, channelMask);
            VideoStampOsd.Instance.SetCameraIdText(1, selected.Text);

            selected.LeftVisible = true;
            this.LaserFrontCameraSelectButton.CenterVisible = DeviceCommunication.Instance.GetCameraLightEnable(Ui.Controls.CameraLocations.laserFront);
            this.LaserFrontCameraSelectButton.CenterLevel = cameraSelectParameters.LightIntensity;
            this.LaserRearCameraSelectButton.CenterVisible = DeviceCommunication.Instance.GetCameraLightEnable(Ui.Controls.CameraLocations.laserRear);
            this.LaserRearCameraSelectButton.CenterLevel = cameraSelectParameters.LightIntensity;
            this.selectedLaserCameraButton = selected;
            ParameterAccessor.Instance.LaserSelectedCamera = selected.Camera;
         }
      }

      private void AssignTargetCamera(Controls.CameraSelectButton selected)
      {
         if (selected != this.selectedTargetCameraButton)
         {
            CameraSelectParameters cameraSelectParameters;

            if (null != this.selectedTargetCameraButton)
            {
               this.selectedTargetCameraButton.RightVisible = false; 
               cameraSelectParameters = ParameterAccessor.Instance.GetCameraSelectParameters(this.selectedTargetCameraButton.Camera);
               cameraSelectParameters.LightIntensity = DeviceCommunication.Instance.GetCameraLightLevel(this.selectedTargetCameraButton.Camera);
               //cameraSelectParameters.LightChannelMask = DeviceCommunication.Instance.GetCameraLightChannelMask(this.selectedTargetCameraButton.Camera);
            }

            cameraSelectParameters = ParameterAccessor.Instance.GetCameraSelectParameters(selected.Camera);
            int channelMask = (false != this.targetLightEnabled) ? cameraSelectParameters.LightChannelMask : 0;

            Tracer.WriteHigh(TraceGroup.UI, "", "target camera {0}", selected.Camera.ToString());
            DeviceCommunication.Instance.SetTargetCamera(selected.Camera);
            DeviceCommunication.Instance.SetCameraLightLevel(selected.Camera, cameraSelectParameters.LightIntensity);
            DeviceCommunication.Instance.SetCameraLightChannelMask(selected.Camera, channelMask);
            VideoStampOsd.Instance.SetCameraIdText(2, selected.Text);

            selected.RightVisible = true;
            this.TargetFrontCameraSelectButton.CenterVisible = DeviceCommunication.Instance.GetCameraLightEnable(Ui.Controls.CameraLocations.targetFront);
            this.TargetFrontCameraSelectButton.CenterLevel = cameraSelectParameters.LightIntensity;
            this.TargetRearCameraSelectButton.CenterVisible = DeviceCommunication.Instance.GetCameraLightEnable(Ui.Controls.CameraLocations.targetRear);
            this.TargetRearCameraSelectButton.CenterLevel = cameraSelectParameters.LightIntensity;
            this.TargetTopCameraSelectButton.CenterVisible = DeviceCommunication.Instance.GetCameraLightEnable(Ui.Controls.CameraLocations.targetTop);
            this.TargetTopCameraSelectButton.CenterLevel = cameraSelectParameters.LightIntensity;
            this.selectedTargetCameraButton = selected;
            ParameterAccessor.Instance.TargetSelectedCamera = selected.Camera;
         }
      }

      #endregion

      #endregion

      #region Process Functions

      private void ProcessStart()
      {
         // zero-initialize fields

         this.processStopped = false;

         this.indicatorFlasher = false;
         this.messageFlasher = false;
         this.messageFlashCount = 0;

         this.joystickApplication = JoystickApplications.none;
         this.UpdateJoystickApplicationButtons();

         this.laserMovementFastSelected = true;

         this.targetMovementFastSelected = true;

         this.sessionState = SessionStates.idle;
         this.measurementRequested = false;
         this.measurementStarted = false;
         this.measurementRecorded = false;

         this.laserStepperFaultProcessed = false;
         this.laserLeftStepperChange = 0;
         this.laserRightStepperChange = 0;

         this.topCameraStepperFaultProcessed = false;
         this.topCameraStepperChange = 0;
         this.topCameraRequestedChange = 0;

         //this.videoSelectMode = VideoSelectModes.none;
         this.laserLightEnabled = true;
         this.targetLightEnabled = true;
         this.selectedLaserCameraButton = null;
         this.selectedTargetCameraButton = null;


         // set next state

         this.UpdateTimer.Interval = 1;
         this.Process = this.ProcessShow;
      }
      
      private void ProcessShow()
      {
         // set version

         string versionString = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
         this.VersionLabel.Text = versionString;


         // clear display

         DateTime dateTime = DateTime.Now.ToLocalTime();
         this.DateTimeTextPanel.ValueText = string.Format("{0:D2}-{1:D2}-{2:D4}   {3:D2}:{4:D2}:{5:D2}", dateTime.Month, dateTime.Day, dateTime.Year, dateTime.Hour, dateTime.Minute, dateTime.Second);

         this.LaserRobotAlternateMotionMotorPanel.Visible = false;

         this.LaserRobotRollDisplay.Roll = 0;
         this.LaserRobotRollDisplay.Pitch = 0;
         this.LaserRobotRollDisplay.Yaw = 0;
         this.LaserRobotRollDisplay.LeftPushIndicatorColor = Color.Gray;
         this.LaserRobotRollDisplay.RightPushIndicatorColor = Color.Gray;
         this.LaserRobotLeftButton.BackColor = Color.FromArgb(171, 171, 171);
         this.LaserRobotRightButton.BackColor = Color.FromArgb(171, 171, 171);
         this.LaserRobotFrontWheelCurrentPanel.ValueText = "";
         this.LaserRobotFrontWheelTemperaturePanel.ValueText = "";
         this.LaserRobotTotalPositionPanel.ValueText = "";
         this.LaserRobotRearWheelCurrentPanel.ValueText = "";
         this.LaserRobotRearWheelTemperaturePanel.ValueText = "";
         this.LaserRobotTripPositionPanel.ValueText = "";
         this.LaserRobotMotorLinkVoltagePanel.ValueText = "";
         this.LaserRobotJogDistanceButton.ValueText = "";
         this.LaserRobotMoveSpeedButton.ValueText = "";
         this.LaserWheelDirectionalValuePanel.ValueText = "";
         this.LaserWheelDirectionalValuePanel.Direction = Ui.Controls.DirectionalValuePanel.Directions.Idle;
         this.LaserRobotWheelMoveButton.ValueText = "";
         this.LaserRobotWheelMoveButton.LeftArrowVisible = false;
         this.LaserRobotWheelMoveButton.RightArrowVisible = false;
         this.LaserRobotWheelSpeedToggleButton.OptionASelected = this.laserMovementFastSelected;
         this.LaserRobotLockButton.Enabled = false;
         this.LaserRobotLockButton.BackColor = Color.FromArgb(171, 171, 171);
         this.LaserRobotLockButton.DisabledForeColor = Color.Gray;
         this.LaserRobotLockButton.Text = "LOCK";


         this.TargetRobotAlternateMotionMotorPanel.Visible = false;

         this.TargetRobotRollDisplay.Roll = 0;
         this.TargetRobotRollDisplay.Pitch = 0;
         this.TargetRobotRollDisplay.Yaw = 0;
         this.TargetRobotRollDisplay.LeftPushIndicatorColor = Color.Gray;
         this.TargetRobotRollDisplay.RightPushIndicatorColor = Color.Gray;
         this.TargetRobotLeftButton.BackColor = Color.FromArgb(171, 171, 171);
         this.TargetRobotRightButton.BackColor = Color.FromArgb(171, 171, 171);
         this.TargetRobotFrontWheelCurrentPanel.ValueText = "";
         this.TargetRobotFrontWheelTemperaturePanel.ValueText = "";
         this.TargetRobotTotalPositionPanel.ValueText = "";
         this.TargetRobotRearWheelCurrentPanel.ValueText = "";
         this.TargetRobotRearWheelTemperaturePanel.ValueText = "";
         this.TargetRobotTripPositionPanel.ValueText = "";
         this.TargetRobotMotorLinkVoltagePanel.ValueText = "";
         this.TargetRobotJogDistanceButton.ValueText = "";
         this.TargetRobotMoveSpeedButton.ValueText = "";
         this.TargetWheelDirectionalValuePanel.ValueText = "";
         this.TargetWheelDirectionalValuePanel.Direction = Ui.Controls.DirectionalValuePanel.Directions.Idle;
         this.TargetWheelMoveButton.ValueText = "";
         this.TargetWheelMoveButton.LeftArrowVisible = false;
         this.TargetWheelMoveButton.RightArrowVisible = false;
         this.TargetWheelSpeedToggleButton.OptionASelected = this.targetMovementFastSelected;
         this.TargetRobotLockButton.Enabled = false;
         this.TargetRobotLockButton.BackColor = Color.FromArgb(171, 171, 171);
         this.TargetRobotLockButton.DisabledForeColor = Color.Gray;
         this.TargetRobotLockButton.Text = "LOCK";

         this.TopCameraRollDisplay.Roll = 0;


         this.LaserPitchTickTargetPanel.ValueText = "";
         this.LaserLeftStepperTickPanel.ValueText = "";
         this.LaserYawTickTargetPanel.ValueText = "";
         this.LaserRightStepperTickPanel.ValueText = "";

         this.LaserRightStepperHomeSwitchIndicator.IndicatorColor = Color.FromArgb(50, 0, 0);
         this.LaserLeftStepperHomeSwitchIndicator.IndicatorColor = Color.FromArgb(50, 0, 0);

         this.TopCameraRollPanel.ValueText = "";
         this.TopCameraStepperPositionPanel.ValueText = "";
         this.TopCameraHomeIndicator.IndicatorColor = Color.FromArgb(50, 0, 0);


         this.LaserTitleLabel.Text = "MEASURE";
         this.MeasurementValuePanel.ValueText = "";
         this.MeasureButton.Text = "MEASURE";

         this.SensorIndicator.MissColor = Color.FromKnownColor(KnownColor.ControlDark);
         this.SensorIndicator.BackColor = Color.FromKnownColor(KnownColor.ControlDark);
         this.SensorIndicator.CoordinateValue = 0;

         this.LaserUpButton.Enabled = true;
         this.LaserDownButton.Enabled = true;
         this.LaserLeftButton.Enabled = true;
         this.LaserRightButton.Enabled = true;
         this.LaserCenterButton.Enabled = true;

         this.TopCameraCounterClockwiseButton.Enabled = true;
         this.TopCameraCenterButton.Enabled = true;
         this.TopCameraClockwiseButton.Enabled = true;

         this.MeasureButton.Enabled = false;
         this.RecordBetweenButton.Enabled = false;
         this.RecordServiceButton.Enabled = false;
         this.SessionActivityButton.Enabled = false;
         this.SessionActivityButton.Text = "START";

         SessionRecord.Instance.Reset();
         this.SessionDataControl.Reset();


         this.StopAllPanel.BackColor = Color.FromArgb(64, 64, 64);
         this.StopAllPanel.Refresh();
         this.StopAllButton.Refresh();

         this.Width = 1920;
         this.Height = 1080;

         // set next state

         this.UpdateTimer.Interval = 1;

         this.LaserStatusTextBox.Width = (this.TargetStatusTextBox.Left + this.TargetStatusTextBox.Width - this.LaserStatusTextBox.Left);
         this.TargetStatusTextBox.Visible = false;

         this.Process = this.ProcessStarting;
      }

      private void ProcessStarting()
      {
         // read parameters

         ParameterAccessor.Instance.Read(Application.ExecutablePath);
         this.traceListener.SetDestination(ParameterAccessor.Instance.Trace.Address, ParameterAccessor.Instance.Trace.Port);
         Tracer.WriteHigh(TraceGroup.UI, null, "starting");


         // process parameters

         SessionRecord.Instance.StoragePath = ParameterAccessor.Instance.SessionDataPath;

         if ((0 == ParameterAccessor.Instance.LaserBus.ConsumerHeartbeatRate) ||
             (0 == ParameterAccessor.Instance.LaserBus.ProducerHeartbeatRate) ||
             (0 == ParameterAccessor.Instance.TargetBus.ConsumerHeartbeatRate) ||
             (0 == ParameterAccessor.Instance.TargetBus.ProducerHeartbeatRate))
         {
            this.HeartbeatsDisabledLabel.Visible = true;
         }

         this.LaserUpdateMovementControls();
         this.TargetUpdateMovementControls();

         this.LaserStepperPitchIndicator.Position = 0;
         this.LaserStepperPitchIndicator.MaximumPosition = (int)(ParameterAccessor.Instance.LaserStepperPivotAngle * 1000);
         this.LaserStepperPitchIndicator.MinimumPosition = (int)(ParameterAccessor.Instance.LaserStepperPivotAngle * -1000);

         this.LaserStepperYawIndicator.Position = 0;
         this.LaserStepperYawIndicator.MaximumPosition = (int)(ParameterAccessor.Instance.LaserStepperPivotAngle * 1000);
         this.LaserStepperYawIndicator.MinimumPosition = (int)(ParameterAccessor.Instance.LaserStepperPivotAngle * -1000);

         this.TopCameraRollDisplay.PositionCcwLimit = (float)ParameterAccessor.Instance.TargetTopCameraCcwLimit;
         this.TopCameraRollDisplay.PositionCwLimit = (float)ParameterAccessor.Instance.TargetTopCameraCwLimit;

         this.LaserRobotJogDistanceButton.ValueText = this.GetValueText(ParameterAccessor.Instance.LaserWheelManualWheelDistance);
         this.LaserRobotMoveSpeedButton.ValueText = this.GetValueText(ParameterAccessor.Instance.LaserWheelManualWheelSpeed);

         this.TargetRobotJogDistanceButton.ValueText = this.GetValueText(ParameterAccessor.Instance.TargetWheelManualWheelDistance);
         this.TargetRobotMoveSpeedButton.ValueText = this.GetValueText(ParameterAccessor.Instance.TargetWheelManualWheelSpeed);

         for (int i = 0; i < this.cameraButtons.Length; i++)
         {
            this.cameraButtons[i].LeftVisible = false;
            this.cameraButtons[i].CenterVisible = false;
            this.cameraButtons[i].RightVisible = false;
         }

//         this.UpdateCameraHoldEnable();
//         this.UpdateVideoSelectorColor();
         this.LaserRobotLightEnableButton.BackColor = (false != this.laserLightEnabled) ? Color.Lime : Color.FromArgb(171, 171, 171);
         this.TargetRobotLightEnableButton.BackColor = (false != this.targetLightEnabled) ? Color.Lime : Color.FromArgb(171, 171, 171);


         DeviceCommunication.Instance.Start();


         // set next state

         this.LaserStatusTextBox.Text = "starting";
         this.LaserStatusTextBox.BackColor = Color.Yellow;

         this.UpdateTimer.Interval = 100;
         this.Process = this.ProcessWaitComm;
      }

      private void ProcessWaitComm()
      {
         DateTime dateTime = DateTime.Now.ToLocalTime();
         this.DateTimeTextPanel.ValueText = string.Format("{0:D2}-{1:D2}-{2:D4}   {3:D2}:{4:D2}:{5:D2}", dateTime.Month, dateTime.Day, dateTime.Year, dateTime.Hour, dateTime.Minute, dateTime.Second);

         if (false != this.processExitNeeded)
         {
            this.Process = this.ProcessStopping;
         }
         else if (false != DeviceCommunication.Instance.Ready)
         {
            // initialize with ready hardware

            Controls.CameraLocations laserCameraLocation = ParameterAccessor.Instance.LaserSelectedCamera;
            Controls.CameraLocations targetCameraLocation = ParameterAccessor.Instance.TargetSelectedCamera;

            for (int i = 0; i < this.cameraButtons.Length; i++)
            {
               if (laserCameraLocation == this.cameraButtons[i].Camera)
               {
                  this.AssignLaserCamera(this.cameraButtons[i]);
               }

               if (targetCameraLocation == this.cameraButtons[i].Camera)
               {
                  this.AssignTargetCamera(this.cameraButtons[i]);
               }
            }

            this.UpdateSessionControls();
            this.LaserUpdateMovementControls();
            this.TargetUpdateMovementControls();

            this.LaserStatusTextBox.Width = (this.TargetStatusTextBox.Left - 8 - this.LaserStatusTextBox.Left);
            this.TargetStatusTextBox.Visible = true;

            
            // set next state

            Tracer.WriteHigh(TraceGroup.UI, null, "started");
            this.Process = this.ProcessExecution;
         }
      }

      private void ProcessExecution()
      {
         #region General 

         this.indicatorFlasher = !this.indicatorFlasher;

         this.messageFlashCount++;

         if (messageFlashCount >= 3)
         {
            this.messageFlashCount = 0;
            this.messageFlasher = !this.messageFlasher;
         }

         DeviceCommunication.Instance.Service();

         DateTime dateTime = DateTime.Now.ToLocalTime();
         this.DateTimeTextPanel.ValueText = string.Format("{0:D2}-{1:D2}-{2:D4}   {3:D2}:{4:D2}:{5:D2}", dateTime.Month, dateTime.Day, dateTime.Year, dateTime.Hour, dateTime.Minute, dateTime.Second);

         #endregion

         #region Laser Robot Bus Status

         string laserStatus = null;
         bool laserWarning = false;

         if (null != Joystick.Instance.FaultReason)
         {
            laserStatus = "joystick missing";
         }
         else if (null != NumatoUsbRelay.Instance.FaultReason)
         {
            laserStatus = "relay offline";
         }
         else
         {
            laserStatus = DeviceCommunication.Instance.GetMainFaultStatus();

            if (null == laserStatus)
            {
               laserStatus = DeviceCommunication.Instance.GetMainWarningStatus();
               laserWarning = true;
            }
         }

         if (null == laserStatus)
         {
            this.LaserStatusTextBox.Text = "ready";
            this.LaserStatusTextBox.BackColor = Color.Lime;
         }
         else
         {
            this.LaserStatusTextBox.Text = laserStatus;
            this.LaserStatusTextBox.BackColor = (false == laserWarning) ? Color.Red : Color.Yellow;
         }

         #endregion

         #region Target Robot Bus Status

         string targetStatus = null;
         bool targetWarning = false;

         targetStatus = DeviceCommunication.Instance.GetTargetFaultStatus();

         if (null == targetStatus)
         {
            targetStatus = DeviceCommunication.Instance.GetTargetWarningStatus();
            targetWarning = true;
         }

         if (null == targetStatus)
         {
            this.TargetStatusTextBox.Text = "ready";
            this.TargetStatusTextBox.BackColor = Color.Lime;
         }
         else
         {
            this.TargetStatusTextBox.Text = targetStatus;
            this.TargetStatusTextBox.BackColor = (false == targetWarning) ? Color.Red : Color.Yellow;
         }

         #endregion

         #region Joystick Status

         int joystickXChange = 0;
         int joystickYChange = 0;
         int joystickThrottleChange = 0;
         int joystickYRange = 1;

         Joystick.Instance.Update();

         if (false != Joystick.Instance.Valid)
         {
            int joystickXAxis = (int)(((Joystick.Instance.XAxis) - 32767) * -1);
            int joystickYAxis = (int)(((Joystick.Instance.YAxis) - 32767) * -1);
            int joystickThrottle = (int)(((Joystick.Instance.Throttle) - 32767) * -1);

            int joystickDeadband = ParameterAccessor.Instance.JoystickDeadband;

            if (joystickXAxis > joystickDeadband)
            {
               joystickXAxis -= joystickDeadband;
            }
            else if (joystickXAxis < -joystickDeadband)
            {
               joystickXAxis += joystickDeadband;
            }
            else
            {
               joystickXAxis = 0;
            }

            if (joystickYAxis > joystickDeadband)
            {
               joystickYAxis -= joystickDeadband;
               joystickYRange = (32767 - joystickDeadband);
            }
            else if (joystickYAxis < -joystickDeadband)
            {
               joystickYAxis += joystickDeadband;
               joystickYRange = (32768 - joystickDeadband);
            }
            else
            {
               joystickYAxis = 0;
            }

            if (joystickThrottle > joystickDeadband)
            {
               joystickThrottle -= joystickDeadband;
            }
            else if (joystickThrottle < -joystickDeadband)
            {
               joystickThrottle += joystickDeadband;
            }
            else
            {
               joystickThrottle = 0;
            }

            if (JoystickApplications.none != this.joystickApplication)
            {
               joystickXChange = joystickXAxis;
               joystickYChange = joystickYAxis;
               joystickThrottleChange = joystickThrottle;

               if (this.joystickApplication == JoystickApplications.laserRobot)
               {
                  if (0 != joystickYAxis)
                  {
                     this.LaserRobotWheelSpeedToggleButton.Enabled = false;
                  }
                  else
                  {
                     this.LaserRobotWheelSpeedToggleButton.Enabled = true;
                  }
               }

               if (this.joystickApplication == JoystickApplications.targetRobot)
               {
                  if (0 != joystickYAxis)
                  {
                     this.TargetWheelSpeedToggleButton.Enabled = false;
                  }
                  else
                  {
                     this.TargetWheelSpeedToggleButton.Enabled = true;
                  }
               }
            }

            if (this.joystickApplication != JoystickApplications.laserRobot)
            {
               if ((0 != joystickXAxis) ||
                   (0 != joystickYAxis))
               {
                  this.LaserRobotMovementJoystickEnableButton.Enabled = false;
               }
               else
               {
                  this.LaserRobotMovementJoystickEnableButton.Enabled = true;
               }
            }

            if (this.joystickApplication != JoystickApplications.targetRobot)
            {
               if ((0 != joystickXAxis) ||
                   (0 != joystickYAxis))
               {
                  this.TargetRobotMovementJoystickEnableButton.Enabled = false;
               }
               else
               {
                  this.TargetRobotMovementJoystickEnableButton.Enabled = true;
               }
            }

            if (this.joystickApplication != JoystickApplications.topCamera)
            {
               if ((0 != joystickXAxis) ||
                   (0 != joystickYAxis))
               {
                  this.TopCameraJoystickEnableButton.Enabled = false;
               }
               else
               {
                  this.TopCameraJoystickEnableButton.Enabled = true;
               }
            }
         }

         #endregion

         #region Laser Robot

         this.LaserRobotRollDisplay.Roll = (float)DeviceCommunication.Instance.GetLaserMainRoll();
         this.LaserRobotRollDisplay.Pitch = (float)DeviceCommunication.Instance.GetLaserMainPitch();
         this.LaserRobotRollDisplay.Yaw = (float)DeviceCommunication.Instance.GetLaserMainYaw();

         string usbRelayFault = NumatoUsbRelay.Instance.FaultReason;
         bool laserLeftPush = NumatoUsbRelay.Instance.GetRelayState(0);
         bool laserRightPush = NumatoUsbRelay.Instance.GetRelayState(1);
         bool laserRobotLocked = DeviceCommunication.Instance.GetLaserMovementLock();
         this.LaserRobotRollDisplay.LeftPushIndicatorColor = (null != usbRelayFault) ? Color.Red : (false != laserLeftPush) ? Color.Lime : Color.Gray;
         this.LaserRobotRollDisplay.RightPushIndicatorColor = (null != usbRelayFault) ? Color.Red : (false != laserRightPush) ? Color.Lime : Color.Gray;


         if (null != usbRelayFault)
         {
            this.LaserRobotLeftButton.BackColor = Color.Red;
            this.LaserRobotLeftButton.DisabledForeColor = Color.Red;

            this.LaserRobotRightButton.BackColor = Color.Red;
            this.LaserRobotRightButton.DisabledForeColor = Color.Red;
         }
         else
         {
            if (false != laserLeftPush)
            {
               this.LaserRobotLeftButton.BackColor = Color.Lime;
               this.LaserRobotLeftButton.DisabledForeColor = Color.Lime;
            }
            else
            {
               this.LaserRobotLeftButton.BackColor = Color.FromArgb(171, 171, 171);
               this.LaserRobotLeftButton.DisabledForeColor = Color.Gray;
            }

            if (false != laserRightPush)
            {
               this.LaserRobotRightButton.BackColor = Color.Lime;
               this.LaserRobotRightButton.DisabledForeColor = Color.Lime;
            }
            else
            {
               this.LaserRobotRightButton.BackColor = Color.FromArgb(171, 171, 171);
               this.LaserRobotRightButton.DisabledForeColor = Color.Gray;
            }
         }


         if (false != laserRobotLocked)
         {
            this.LaserRobotLockButton.BackColor = Color.Lime;
            this.LaserRobotLockButton.DisabledForeColor = Color.Lime;
            this.LaserRobotLockButton.Text = (false != this.LaserRobotLockButton.Enabled) ? "UNLOCK" : "LOCKED";
         }
         else
         {
            this.LaserRobotLockButton.BackColor = Color.FromArgb(171, 171, 171);
            this.LaserRobotLockButton.DisabledForeColor = Color.Gray;
            this.LaserRobotLockButton.Text = "LOCK";
         }


         double laserWheelCurrent = 0;
         double laserWheelTemperature = 0;
         double laserWheelPosition = 0;

         laserWheelCurrent = DeviceCommunication.Instance.GetLaserWheelCurrentValue(WheelLocations.front);
         this.LaserRobotFrontWheelCurrentPanel.ValueText = this.GetValueText(laserWheelCurrent, 2, "A"); ;

         laserWheelTemperature = DeviceCommunication.Instance.GetLaserWheelTemperatureValue(WheelLocations.front);
         this.LaserRobotFrontWheelTemperaturePanel.ValueText = this.GetValueText(laserWheelTemperature, 0, "°C");

         laserWheelCurrent = DeviceCommunication.Instance.GetLaserWheelCurrentValue(WheelLocations.rear);
         this.LaserRobotRearWheelCurrentPanel.ValueText = this.GetValueText(laserWheelCurrent, 2, "A"); ;

         laserWheelTemperature = DeviceCommunication.Instance.GetLaserWheelTemperatureValue(WheelLocations.rear);
         this.LaserRobotRearWheelTemperaturePanel.ValueText = this.GetValueText(laserWheelTemperature, 0, "°C");

         laserWheelPosition = DeviceCommunication.Instance.GetLaserWheelTotalPositionValue();
         this.LaserRobotTotalPositionPanel.ValueText = this.GetValueText(laserWheelPosition, ParameterAccessor.Instance.LaserWheelManualWheelDistance);
         
         laserWheelPosition = DeviceCommunication.Instance.GetLaserWheelTripPositionValue();
         this.LaserRobotTripPositionPanel.ValueText = this.GetValueText(laserWheelPosition, ParameterAccessor.Instance.LaserWheelManualWheelDistance);

         double laserLinkVoltage = DeviceCommunication.Instance.GetLaserLinkVoltage();
         this.LaserRobotMotorLinkVoltagePanel.ValueText = this.GetValueText(laserLinkVoltage, 1, "V");


         this.LaserUpdateMovementControls();


         double laserMovementRequestValue = 0;
         ValueParameter laserMovementParameter = null;
         DeviceCommunication.Instance.GetLaserMovementRequestValues(ref laserMovementParameter, ref laserMovementRequestValue);

         double laserMovementValue = DeviceCommunication.Instance.GetLaserMovementValue();
         double laserMovementStatusDisplayValue = Math.Abs(laserMovementValue);

         if (laserMovementStatusDisplayValue < laserMovementParameter.StepValue)
         {
            laserMovementValue = 0;
            laserMovementStatusDisplayValue = 0;
         }

         if (laserMovementValue > 0)
         {
            this.LaserWheelDirectionalValuePanel.Direction = Ui.Controls.DirectionalValuePanel.Directions.Forward;
         }
         else if (laserMovementValue < 0)
         {
            this.LaserWheelDirectionalValuePanel.Direction = Ui.Controls.DirectionalValuePanel.Directions.Reverse;
         }
         else
         {
            this.LaserWheelDirectionalValuePanel.Direction = Ui.Controls.DirectionalValuePanel.Directions.Idle;
         }

         this.LaserWheelDirectionalValuePanel.ValueText = this.GetValueText(laserMovementStatusDisplayValue, laserMovementParameter);


         bool laserMovementReverse = false;
         bool laserMovementForward = false;
         bool laserMovementSet = false;

         if (JoystickApplications.laserRobot == this.joystickApplication)
         {
            if (false == this.LaserRobotAlternateMotionMotorPanel.Visible)
            {
               this.LaserRobotAlternateMotionMotorPanel.Top = this.GetAbsoluteTop(this.LaserRobotJogReverseButton);
               this.LaserRobotAlternateMotionMotorPanel.Left = this.LaserRobotWheelPanel.Left;

               this.LaserRobotAlternateMotionMotorPanel.Visible = true;
            }

            if (joystickYChange < 0)
            {
               laserMovementReverse = true;
               laserMovementForward = false;
            }
            else if (joystickYChange > 0)
            {
               laserMovementReverse = false;
               laserMovementForward = true;
            }

            double laserMovementScale = (false != this.laserMovementFastSelected) ? 1.0 : ParameterAccessor.Instance.LaserWheelLowSpeedScale.OperationalValue / 100;
            double laserMovementRequestPercent = laserMovementScale * joystickYChange / joystickYRange;

            bool laserMovementTriggered = (false != Joystick.Instance.Button1Pressed);
            DeviceCommunication.Instance.SetLaserMovementVelocityRequest(laserMovementRequestPercent, laserMovementTriggered);

            bool laserMovementActivated = DeviceCommunication.Instance.GetLaserMovementActivated();

            if (false != this.LaserRobotWheelMoveButton.Enabled)
            {
               double laserMovementDisplayValue = Math.Abs(laserMovementRequestValue);
               this.LaserRobotWheelMoveButton.LeftArrowVisible = laserMovementReverse;
               this.LaserRobotWheelMoveButton.RightArrowVisible = laserMovementForward;
               this.LaserRobotWheelMoveButton.ValueForeColor = (false != laserMovementActivated) ? Color.White : Color.FromKnownColor(KnownColor.ControlDarkDark);
               this.LaserRobotWheelMoveButton.ValueText = this.GetValueText(laserMovementDisplayValue, laserMovementParameter);
               laserMovementSet = true;
            }
         }
         else
         {
            if (false != this.LaserRobotAlternateMotionMotorPanel.Visible)
            {
               this.LaserRobotAlternateMotionMotorPanel.Visible = false;
            }
         }


         if (false == laserMovementSet)
         {
            bool laserMovementManual = DeviceCommunication.Instance.GetLaserMovementManualMode();

            if (false == laserMovementManual)
            {
               DeviceCommunication.Instance.SetLaserMovementVelocityRequest(0, false);
            }

            this.LaserRobotWheelMoveButton.LeftArrowVisible = false;
            this.LaserRobotWheelMoveButton.RightArrowVisible = false;
            this.LaserRobotWheelMoveButton.ValueForeColor = Color.FromKnownColor(KnownColor.ControlDarkDark);
            this.LaserRobotWheelMoveButton.ValueText = this.GetValueText(0, laserMovementParameter);
         }

         #endregion

         #region Target Robot

         double targetRobotMainRoll = DeviceCommunication.Instance.GetTargetMainRoll();
         this.TargetRobotRollDisplay.Roll = (float)targetRobotMainRoll;
         this.TargetRobotRollDisplay.Pitch = (float)DeviceCommunication.Instance.GetTargetMainPitch();
         this.TargetRobotRollDisplay.Yaw = (float)DeviceCommunication.Instance.GetTargetMainYaw();

         usbRelayFault = NumatoUsbRelay.Instance.FaultReason;
         bool targetLeftPush = NumatoUsbRelay.Instance.GetRelayState(2);
         bool targetRightPush = NumatoUsbRelay.Instance.GetRelayState(3);
         bool targetRobotLocked = DeviceCommunication.Instance.GetTargetMovementLock();
         this.TargetRobotRollDisplay.LeftPushIndicatorColor = (null != usbRelayFault) ? Color.Red : (false != targetLeftPush) ? Color.Lime : Color.Gray;
         this.TargetRobotRollDisplay.RightPushIndicatorColor = (null != usbRelayFault) ? Color.Red : (false != targetRightPush) ? Color.Lime : Color.Gray;


         if (null != usbRelayFault)
         {
            this.TargetRobotLeftButton.BackColor = Color.Red;
            this.TargetRobotLeftButton.DisabledForeColor = Color.Red;

            this.TargetRobotRightButton.BackColor = Color.Red;
            this.TargetRobotRightButton.DisabledForeColor = Color.Red;
         }
         else
         {
            if (false != targetLeftPush)
            {
               this.TargetRobotLeftButton.BackColor = Color.Lime;
               this.TargetRobotLeftButton.DisabledForeColor = Color.Lime;
            }
            else
            {
               this.TargetRobotLeftButton.BackColor = Color.FromArgb(171, 171, 171);
               this.TargetRobotLeftButton.DisabledForeColor = Color.Gray;
            }

            if (false != targetRightPush)
            {
               this.TargetRobotRightButton.BackColor = Color.Lime;
               this.TargetRobotRightButton.DisabledForeColor = Color.Lime;
            }
            else
            {
               this.TargetRobotRightButton.BackColor = Color.FromArgb(171, 171, 171);
               this.TargetRobotRightButton.DisabledForeColor = Color.Gray;
            }
         }


         if (false != targetRobotLocked)
         {
            this.TargetRobotLockButton.BackColor = Color.Lime;
            this.TargetRobotLockButton.DisabledForeColor = Color.Lime;
            this.TargetRobotLockButton.Text = (false != this.TargetRobotLockButton.Enabled) ? "UNLOCK" : "LOCKED";
         }
         else
         {
            this.TargetRobotLockButton.BackColor = Color.FromArgb(171, 171, 171);
            this.TargetRobotLockButton.DisabledForeColor = Color.Gray;
            this.TargetRobotLockButton.Text = "LOCK";
         }

         double topCameraRoll = DeviceCommunication.Instance.GetTargetTopCameraRoll();
         this.TopCameraRollDisplay.Roll = (float)topCameraRoll;
         this.TopCameraRollDisplay.PositionRoll = (float)targetRobotMainRoll;
         this.TopCameraRollPanel.ValueText = this.GetValueText(topCameraRoll, 1, "°");
         
         int topCameraStepperPosition = DeviceCommunication.Instance.GetTargetStepperActualPosition();
         this.TopCameraStepperPositionPanel.ValueText = this.GetValueText(topCameraStepperPosition);

         bool topCameraStepperHomeActive = DeviceCommunication.Instance.GetTopCameraStepperHomeSwitchActive();
         this.TopCameraHomeIndicator.IndicatorColor = (false != topCameraStepperHomeActive) ? Color.Red : Color.FromArgb(50, 0, 0);


         double targetWheelCurrent = 0;
         double targetWheelTemperature = 0;
         double targetWheelPosition = 0;

         targetWheelCurrent = DeviceCommunication.Instance.GetTargetWheelCurrentValue(WheelLocations.front);
         this.TargetRobotFrontWheelCurrentPanel.ValueText = this.GetValueText(targetWheelCurrent, 2, "A"); ;

         targetWheelTemperature = DeviceCommunication.Instance.GetTargetWheelTemperatureValue(WheelLocations.front);
         this.TargetRobotFrontWheelTemperaturePanel.ValueText = this.GetValueText(targetWheelTemperature, 0, "°C");

         targetWheelCurrent = DeviceCommunication.Instance.GetTargetWheelCurrentValue(WheelLocations.rear);
         this.TargetRobotRearWheelCurrentPanel.ValueText = this.GetValueText(targetWheelCurrent, 2, "A"); ;

         targetWheelTemperature = DeviceCommunication.Instance.GetTargetWheelTemperatureValue(WheelLocations.rear);
         this.TargetRobotRearWheelTemperaturePanel.ValueText = this.GetValueText(targetWheelTemperature, 0, "°C");

         targetWheelPosition = DeviceCommunication.Instance.GetTargetWheelTotalPositionValue();
         this.TargetRobotTotalPositionPanel.ValueText = this.GetValueText(targetWheelPosition, ParameterAccessor.Instance.LaserWheelManualWheelDistance);

         targetWheelPosition = DeviceCommunication.Instance.GetTargetWheelTripPositionValue();
         this.TargetRobotTripPositionPanel.ValueText = this.GetValueText(targetWheelPosition, ParameterAccessor.Instance.LaserWheelManualWheelDistance);

         double targetLinkVoltage = DeviceCommunication.Instance.GetTargetLinkVoltage();
         this.TargetRobotMotorLinkVoltagePanel.ValueText = this.GetValueText(targetLinkVoltage, 1, "V");


         this.TargetUpdateMovementControls();


         double targetMovementRequestValue = 0;
         ValueParameter targetMovementParameter = null;
         DeviceCommunication.Instance.GetTargetMovementRequestValues(ref targetMovementParameter, ref targetMovementRequestValue);

         double targetMovementValue = DeviceCommunication.Instance.GetTargetMovementValue();
         double targetMovementStatusDisplayValue = Math.Abs(targetMovementValue);

         if (targetMovementStatusDisplayValue < targetMovementParameter.StepValue)
         {
            targetMovementValue = 0;
            targetMovementStatusDisplayValue = 0;
         }

         if (targetMovementValue > 0)
         {
            this.TargetWheelDirectionalValuePanel.Direction = Ui.Controls.DirectionalValuePanel.Directions.Forward;
         }
         else if (targetMovementValue < 0)
         {
            this.TargetWheelDirectionalValuePanel.Direction = Ui.Controls.DirectionalValuePanel.Directions.Reverse;
         }
         else
         {
            this.TargetWheelDirectionalValuePanel.Direction = Ui.Controls.DirectionalValuePanel.Directions.Idle;
         }

         this.TargetWheelDirectionalValuePanel.ValueText = this.GetValueText(targetMovementStatusDisplayValue, targetMovementParameter);
         

         bool targetMovementReverse = false;
         bool targetMovementForward = false;
         bool targetMovementSet = false;

         if (JoystickApplications.targetRobot == this.joystickApplication)
         {
            if (false == this.TargetRobotAlternateMotionMotorPanel.Visible)
            {
               this.TargetRobotAlternateMotionMotorPanel.Top = this.GetAbsoluteTop(this.TargetRobotJogReverseButton);
               this.TargetRobotAlternateMotionMotorPanel.Left = this.TargetRobotWheelPanel.Left;

               this.TargetRobotAlternateMotionMotorPanel.Visible = true;
            }

            if (joystickYChange < 0)
            {
               targetMovementReverse = true;
               targetMovementForward = false;
            }
            else if (joystickYChange > 0)
            {
               targetMovementReverse = false;
               targetMovementForward = true;
            }

            double targetMovementScale = (false != this.targetMovementFastSelected) ? 1.0 : ParameterAccessor.Instance.TargetWheelLowSpeedScale.OperationalValue / 100;
            double targetMovementRequestPercent = targetMovementScale * joystickYChange / joystickYRange;

            bool targetMovementTriggered = (false != Joystick.Instance.Button1Pressed);
            DeviceCommunication.Instance.SetTargetMovementVelocityRequest(targetMovementRequestPercent, targetMovementTriggered);

            bool targetMovementActivated = DeviceCommunication.Instance.GetTargetMovementActivated();

            if (false != this.TargetWheelMoveButton.Enabled)
            {
               double targetMovementDisplayValue = Math.Abs(targetMovementRequestValue);
               this.TargetWheelMoveButton.LeftArrowVisible = targetMovementReverse;
               this.TargetWheelMoveButton.RightArrowVisible = targetMovementForward;
               this.TargetWheelMoveButton.ValueForeColor = (false != targetMovementActivated) ? Color.White : Color.FromKnownColor(KnownColor.ControlDarkDark);
               this.TargetWheelMoveButton.ValueText = this.GetValueText(targetMovementDisplayValue, targetMovementParameter);
               targetMovementSet = true;
            }
         }
         else
         {
            if (false != this.TargetRobotAlternateMotionMotorPanel.Visible)
            {
               this.TargetRobotAlternateMotionMotorPanel.Visible = false;
            }
         }

         if (false == targetMovementSet)
         {
            bool targetMovementManual = DeviceCommunication.Instance.GetTargetMovementManualMode();

            if (false == targetMovementManual)
            {
               DeviceCommunication.Instance.SetTargetMovementVelocityRequest(0, false);
            }

            this.TargetWheelMoveButton.LeftArrowVisible = false;
            this.TargetWheelMoveButton.RightArrowVisible = false;
            this.TargetWheelMoveButton.ValueForeColor = Color.FromKnownColor(KnownColor.ControlDarkDark);
            this.TargetWheelMoveButton.ValueText = this.GetValueText(0, targetMovementParameter);
         }

         #endregion

         #region Laser Measurement

         this.UpdateSessionControls();

         string laserMeasureFault = LaserCommunicationBus.Instance.GetFaultStatus(LaserCommunicationBus.BusComponentId.LaserBoard);
         string targetMeasureFault = TargetCommunicationBus.Instance.GetFaultStatus(TargetCommunicationBus.BusComponentId.TargetBoard);

         if ((null == laserMeasureFault) && (null == targetMeasureFault))
         {
            bool robotsLocked = ((false != laserRobotLocked) && (false != targetRobotLocked)) ? true : false;
            bool measurementEnabled = ((SessionStates.idle == this.sessionState) || (false != robotsLocked)) ? true : false;
            this.MeasureButton.Enabled = measurementEnabled;
            this.SessionActivityButton.Enabled = robotsLocked;
            
            bool laserAimActive = DeviceCommunication.Instance.GetLaserAim();
            this.LaserAimButton.BackColor = (false != laserAimActive) ? Color.Lime : Color.FromArgb(171, 171, 171);

            bool measurementReady = DeviceCommunication.Instance.GetLaserMeasurementReady();
            bool measurementActive = DeviceCommunication.Instance.GetLaserMeasurementActivity();


            string measurementStatusText = "";

            if (false != this.measurementRequested)
            {
               if (false != measurementReady)
               {
                  Tracer.WriteHigh(TraceGroup.UI, "", "measure ready");

                  double deviceMeasurement = DeviceCommunication.Instance.GetAverageLaserMeasurement();
                  this.MeasurementValuePanel.BackColor = Color.Black;
                  this.MeasurementValuePanel.ValueText = this.GetValueText(deviceMeasurement, ParameterAccessor.Instance.LaserMeasurementConstant);
                  measurementStatusText = "MEASUREMENT READY";
                  this.measurement = deviceMeasurement;

                  if (SessionStates.idle != this.sessionState)
                  {
                     this.RecordBetweenButton.Enabled = true;
                     this.RecordServiceButton.Enabled = true;
                  }

                  this.MeasureButton.Text = "MEASURE";
                  this.measurementRequested = false;
                  this.measurementStarted = true;
               }
               else
               {
                  this.MeasurementValuePanel.BackColor = (false != this.messageFlasher) ? Color.DodgerBlue : Color.SteelBlue;

                  if (false != measurementActive)
                  {
                     int laserMeasureRemainingCount = DeviceCommunication.Instance.GetLaserSampleRemainingCount();
                     string laserMeasureStatusText = string.Format("MEASURING ({0})", laserMeasureRemainingCount);
                     measurementStatusText = laserMeasureStatusText;
                  }

                  this.MeasureButton.Text = "CANCEL";
               }
            }
            else if ((false != measurementReady) && (false == this.measurementRecorded) && (false != this.measurementStarted))
            {
               measurementStatusText = "MEASUREMENT READY";
               this.MeasureButton.Text = "MEASURE";
               this.MeasurementValuePanel.BackColor = Color.Black;
            }
            else
            {
               measurementStatusText = "MEASURE READY";
               this.MeasureButton.Text = "MEASURE";
               this.MeasurementValuePanel.BackColor = Color.Black;
            }

            string sessionStatusText = "";

            if (SessionStates.idle != this.sessionState)
            {
               if (false != this.measurementRequested)
               {
                  sessionStatusText = string.Format("SESSION ACTIVE - {0}", measurementStatusText);
               }
               else if (false != this.measurementRecorded)
               {
                  if (SessionStates.move == this.sessionState)
                  {
                     sessionStatusText = string.Format("SESSION ACTIVE - MOVE");
                  }
               }
               else
               {
                  sessionStatusText = string.Format("SESSION ACTIVE - {0}", measurementStatusText);
               }
            }
            else
            {
               sessionStatusText = measurementStatusText;
            }

            this.LaserTitleLabel.Text = sessionStatusText;
         }
         else
         {
            this.MeasureButton.Enabled = false;
            this.SessionActivityButton.Enabled = false;
            
            this.LaserTitleLabel.Text = "MEASURE - FAULTED";
         }


         string laserScannerFault = TargetCommunicationBus.Instance.GetFaultStatus(TargetCommunicationBus.BusComponentId.TargetBoard);

         if (null == laserScannerFault)
         {
            this.SensorIndicator.MissColor = Color.FromArgb(140, 0, 0);
            this.SensorIndicator.BackColor = Color.FromKnownColor(KnownColor.ControlDark);
            this.SensorIndicator.CoordinateValue = DeviceCommunication.Instance.GetTargetScannerCoordinates();
         }
         else
         {
            this.SensorIndicator.MissColor = Color.Red;
            this.SensorIndicator.BackColor = Color.Red;
            this.SensorIndicator.CoordinateValue = 0;
         }


         this.SessionDataControl.UpdateRecordData();

         #endregion

         #region Laser Aiming

         string laserLeftStepperFault = LaserCommunicationBus.Instance.GetFaultStatus(LaserCommunicationBus.BusComponentId.LaserBoardLeftStepper);
         string laserRightStepperFault = LaserCommunicationBus.Instance.GetFaultStatus(LaserCommunicationBus.BusComponentId.LaserBoardRightStepper);
         bool laserStepperFaulted = ((null != laserLeftStepperFault) || (null != laserRightStepperFault)) ? true : false;

         if (false != laserStepperFaulted)
         {
            if (false == this.laserStepperFaultProcessed)
            {
               this.LaserUpButton.Enabled = false;
               this.LaserDownButton.Enabled = false;
               this.LaserLeftButton.Enabled = false;
               this.LaserRightButton.Enabled = false;
               this.LaserCenterButton.Enabled = false;

               this.laserLeftStepperChange = 0;
               this.laserRightStepperChange = 0;

               this.laserStepperFaultProcessed = true;
            }
         }
         else
         {
            if (false != this.laserStepperFaultProcessed)
            {
               this.LaserUpButton.Enabled = true;
               this.LaserDownButton.Enabled = true;
               this.LaserLeftButton.Enabled = true;
               this.LaserRightButton.Enabled = true;
               this.LaserCenterButton.Enabled = true;

               this.laserStepperFaultProcessed = false;
            }
         }

         int laserLeftStepperTargetValue = DeviceCommunication.Instance.GetLaserLeftStepperTargetPosition();
         int laserRightStepperTargetValue = DeviceCommunication.Instance.GetLaserRightStepperTargetPosition();

         int fullRangeTicks = (int)(ParameterAccessor.Instance.LaserStepperPivotAngle * 2 * 10); // full range in 10 seconds
         int leftStepperAdjust = (ParameterAccessor.Instance.LaserLeftStepper.MaximumPosition - ParameterAccessor.Instance.LaserLeftStepper.MinimumPosition) / fullRangeTicks;
         int rightStepperAdjust = (ParameterAccessor.Instance.LaserRightStepper.MaximumPosition - ParameterAccessor.Instance.LaserRightStepper.MinimumPosition) / fullRangeTicks;

         if (this.laserLeftStepperChange > 0)
         {
            laserLeftStepperTargetValue += leftStepperAdjust;

            if (laserLeftStepperTargetValue > ParameterAccessor.Instance.LaserLeftStepper.MaximumPosition)
            {
               laserLeftStepperTargetValue = ParameterAccessor.Instance.LaserLeftStepper.MaximumPosition;
            }
         }
         else if (this.laserLeftStepperChange < 0)
         {
            laserLeftStepperTargetValue -= leftStepperAdjust;

            if (laserLeftStepperTargetValue < ParameterAccessor.Instance.LaserLeftStepper.MinimumPosition)
            {
               laserLeftStepperTargetValue = ParameterAccessor.Instance.LaserLeftStepper.MinimumPosition;
            }
         }

         if (this.laserRightStepperChange > 0)
         {
            laserRightStepperTargetValue += rightStepperAdjust;

            if (laserRightStepperTargetValue > ParameterAccessor.Instance.LaserRightStepper.MaximumPosition)
            {
               laserRightStepperTargetValue = ParameterAccessor.Instance.LaserRightStepper.MaximumPosition;
            }
         }
         else if (this.laserRightStepperChange < 0)
         {
            laserRightStepperTargetValue -= rightStepperAdjust;

            if (laserRightStepperTargetValue < ParameterAccessor.Instance.LaserRightStepper.MinimumPosition)
            {
               laserRightStepperTargetValue = ParameterAccessor.Instance.LaserRightStepper.MinimumPosition;
            }
         }

         DeviceCommunication.Instance.SetLaserLeftStepperTargetPosition(laserLeftStepperTargetValue);

         int laserleftStepperPosition = DeviceCommunication.Instance.GetLaserLeftStepperPosition();
         this.LaserLeftStepperTickPanel.ValueText = this.GetValueText(laserleftStepperPosition);

         bool laserLeftStepperHomeActive = DeviceCommunication.Instance.GetLaserLeftStepperHomeSwitchActive();
         this.LaserLeftStepperHomeSwitchIndicator.IndicatorColor = (false != laserLeftStepperHomeActive) ? Color.Red : Color.FromArgb(50, 0, 0);


         DeviceCommunication.Instance.SetLaserRightStepperTargetPosition(laserRightStepperTargetValue);

         int laserRightStepperPosition = DeviceCommunication.Instance.GetLaserRightStepperPosition();
         this.LaserRightStepperTickPanel.ValueText = this.GetValueText(laserRightStepperPosition);

         bool laserStepperXHomeActive = DeviceCommunication.Instance.GetLaserRightStepperHomeSwitchActive();
         this.LaserRightStepperHomeSwitchIndicator.IndicatorColor = (false != laserStepperXHomeActive) ? Color.Red : Color.FromArgb(50, 0, 0);


         double stepperMaximum = ((ParameterAccessor.Instance.LaserLeftStepper.MaximumPosition - ParameterAccessor.Instance.LaserLeftStepper.MinimumPosition) +
                                  (ParameterAccessor.Instance.LaserRightStepper.MaximumPosition - ParameterAccessor.Instance.LaserRightStepper.MinimumPosition)) / 2;
         double calculatedPitch = ((laserLeftStepperTargetValue + laserRightStepperTargetValue - stepperMaximum) / stepperMaximum) * ParameterAccessor.Instance.LaserStepperPivotAngle;
         double calculatedYaw = ((laserLeftStepperTargetValue - laserRightStepperTargetValue) / stepperMaximum) * ParameterAccessor.Instance.LaserStepperPivotAngle;

         this.LaserPitchTickTargetPanel.ValueText = this.GetValueText(calculatedPitch, 1, "°");
         this.LaserStepperPitchIndicator.Position = (int)(calculatedPitch * 1000);

         this.LaserYawTickTargetPanel.ValueText = this.GetValueText(calculatedYaw, 1, "°");
         this.LaserStepperYawIndicator.Position = (int)(calculatedYaw * -1000); ;

         #endregion

         #region Top Camera

         string topCamerStepperFault = TargetCommunicationBus.Instance.GetFaultStatus(TargetCommunicationBus.BusComponentId.TargetBoardCameraStepper);
         bool topCameraStepperFaulted = (null != topCamerStepperFault) ? true : false;
         int topCameraStepperRequest = DeviceCommunication.Instance.GetTargetStepperActualPosition();
         int topCameraChange = 0;

         if (false != topCameraStepperFaulted)
         {
            if (false == this.topCameraStepperFaultProcessed)
            {
               this.TopCameraCounterClockwiseButton.Enabled = false;
               this.TopCameraCenterButton.Enabled = false;
               this.TopCameraClockwiseButton.Enabled = false;

               this.topCameraStepperChange = 0;

               this.topCameraStepperFaultProcessed = true;
            }
         }
         else
         {
            if (false != this.topCameraStepperFaultProcessed)
            {
               this.topCameraStepperFaultProcessed = false;
            }

            if (JoystickApplications.topCamera == this.joystickApplication)
            {
               this.TopCameraCounterClockwiseButton.Enabled = false;
               this.TopCameraCenterButton.Enabled = false;
               this.TopCameraClockwiseButton.Enabled = false;

               topCameraChange = joystickYChange;
            }
            else
            {
               this.TopCameraCounterClockwiseButton.Enabled = true;
               this.TopCameraCenterButton.Enabled = true;
               this.TopCameraClockwiseButton.Enabled = true;

               topCameraChange = this.topCameraStepperChange;
            }
         }

         if (topCameraChange > 0)
         {
            topCameraStepperRequest = ParameterAccessor.Instance.TargetStepper.MaximumPosition;
         }
         else if (topCameraChange < 0)
         {
            topCameraStepperRequest = ParameterAccessor.Instance.TargetStepper.MinimumPosition;
         }

         if (topCameraChange != this.topCameraRequestedChange)
         {
            if (0 != topCameraChange)
            {
               DeviceCommunication.Instance.SetTargetStepperPosition(topCameraStepperRequest);
            }
            else
            {
               DeviceCommunication.Instance.StopTargetStepper();
            }

            this.topCameraRequestedChange = topCameraChange;
         }

         #endregion


         // set next state

         if (false != this.processStopNeeded)
         {
            this.processStopNeeded = false;
            this.Process = this.ProcessStopping;
         }
      }

      private void ProcessStopping()
      {
         this.TargetStatusTextBox.Visible = false;
         this.LaserStatusTextBox.Width = (this.TargetStatusTextBox.Left + this.TargetStatusTextBox.Width - this.LaserStatusTextBox.Left);
         this.LaserStatusTextBox.Text = "stopping";
         this.LaserStatusTextBox.BackColor = Color.Yellow;
         Tracer.WriteHigh(TraceGroup.UI, null, "stopping");


         // set next state

         DeviceCommunication.Instance.Stop();

         this.Process = this.ProcessWaitCommStop;
      }

      private void ProcessWaitCommStop()
      {
         DateTime dateTime = DateTime.Now.ToLocalTime();
         this.DateTimeTextPanel.ValueText = string.Format("{0:D2}-{1:D2}-{2:D4}   {3:D2}:{4:D2}:{5:D2}", dateTime.Month, dateTime.Day, dateTime.Year, dateTime.Hour, dateTime.Minute, dateTime.Second);

         if (false == DeviceCommunication.Instance.Running)
         {
            ParameterAccessor.Instance.Write(Application.ExecutablePath);

            Tracer.WriteHigh(TraceGroup.UI, null, "stopped");

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

      #region Laser Robot Movement Events

      private void LaserRobotLeftButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         bool state = NumatoUsbRelay.Instance.GetRelayState(0);
         NumatoUsbRelay.Instance.SetRelay(0, !state);
      }

      private void LaserRobotRightButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         bool state = NumatoUsbRelay.Instance.GetRelayState(1);
         NumatoUsbRelay.Instance.SetRelay(1, !state);
      }

      private void LaserRobotWheelOffButton_Click(object sender, EventArgs e)
      {
         if (false == this.LaserRobotWheelOffButton.HoldTimeoutEnable)
         {
            DeviceCommunication.Instance.SetLaserMovementMode(MovementModes.off);
         }
      }

      private void LaserRobotWheelOffButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         DeviceCommunication.Instance.SetLaserMovementMode(MovementModes.off);
      }

      private void LaserRobotWheelMoveButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         DeviceCommunication.Instance.SetLaserMovementMode(MovementModes.move);
      }

      private void LaserRobotWheelSpeedToggleButton_Click(object sender, EventArgs e)
      {
         bool selection = !this.LaserRobotWheelSpeedToggleButton.OptionASelected;
         this.laserMovementFastSelected = selection;
         this.LaserRobotWheelSpeedToggleButton.OptionASelected = selection;
      }

      private void LaserRobotJogReverseButton_MouseDown(object sender, MouseEventArgs e)
      {
         DeviceCommunication.Instance.SetLaserMovementPositionRequest(-ParameterAccessor.Instance.LaserWheelManualWheelDistance.OperationalValue);
      }

      private void LaserRobotJogDistanceButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         DialogResult result = this.LaunchNumberEdit(this.LaserRobotJogDistanceButton, ParameterAccessor.Instance.LaserWheelManualWheelDistance, "JOG DISTANCE");
      }

      private void LaserRobotJogForwardButton_MouseDown(object sender, MouseEventArgs e)
      {
         DeviceCommunication.Instance.SetLaserMovementPositionRequest(ParameterAccessor.Instance.LaserWheelManualWheelDistance.OperationalValue);
      }

      private void LaserRobotMoveReverseButton_MouseDown(object sender, MouseEventArgs e)
      {
         double neededSpeed = ParameterAccessor.Instance.LaserWheelManualWheelSpeed.OperationalValue;
         double neededPercent = neededSpeed / ParameterAccessor.Instance.LaserWheelMaximumSpeed.OperationalValue;

         DeviceCommunication.Instance.SetLaserMovementManualMode(true);
         DeviceCommunication.Instance.SetLaserMovementVelocityRequest(-neededPercent, true);
      }

      private void LaserRobotMoveReverseButton_MouseUp(object sender, MouseEventArgs e)
      {
         DeviceCommunication.Instance.SetLaserMovementVelocityRequest(0, false);
         DeviceCommunication.Instance.SetLaserMovementManualMode(false);
      }

      private void LaserRobotMoveSpeedButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         ParameterAccessor.Instance.LaserWheelManualWheelSpeed.MaximumValue = ParameterAccessor.Instance.LaserWheelMaximumSpeed.OperationalValue;
         DialogResult result = this.LaunchNumberEdit(this.LaserRobotMoveSpeedButton, ParameterAccessor.Instance.LaserWheelManualWheelSpeed, "MOVE SPEED");
      }

      private void LaserRobotMoveForwardButton_MouseDown(object sender, MouseEventArgs e)
      {
         double neededSpeed = ParameterAccessor.Instance.LaserWheelManualWheelSpeed.OperationalValue;
         double neededPercent = neededSpeed / ParameterAccessor.Instance.LaserWheelMaximumSpeed.OperationalValue;

         DeviceCommunication.Instance.SetLaserMovementManualMode(true);
         DeviceCommunication.Instance.SetLaserMovementVelocityRequest(neededPercent, true);
      }

      private void LaserRobotMoveForwardButton_MouseUp(object sender, MouseEventArgs e)
      {
         DeviceCommunication.Instance.SetLaserMovementVelocityRequest(0, false);
         DeviceCommunication.Instance.SetLaserMovementManualMode(false);
      }

      private void LaserRobotMotorSetupButton_Click(object sender, EventArgs e)
      {
         Button button = (Button)sender;

         LaserRobotMovementSetupForm laserRobotMovementSetupForm = new LaserRobotMovementSetupForm();

         this.SetDialogLocation(button, laserRobotMovementSetupForm);
         this.DimBackground();
         laserRobotMovementSetupForm.ShowDialog();
         this.LightBackground();
      }

      private void LaserRobotMovementJoystickEnableButton_Click(object sender, EventArgs e)
      {
         if (JoystickApplications.laserRobot != this.joystickApplication)
         {
            this.joystickApplication = JoystickApplications.laserRobot;
         }
         else
         {
            this.joystickApplication = JoystickApplications.none;
         }

         this.UpdateJoystickApplicationButtons();
      }
      
      private void LaserRobotLockButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         bool locked = DeviceCommunication.Instance.GetLaserMovementLock();
         DeviceCommunication.Instance.SetLaserMovementLock(!locked);
         this.UpdateSessionControls();
      }

      #endregion

      #region Target Robot Movement Events

      private void TargetRobotLeftButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         bool state = NumatoUsbRelay.Instance.GetRelayState(2);
         NumatoUsbRelay.Instance.SetRelay(2, !state);
      }

      private void TargetRobotRightButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         bool state = NumatoUsbRelay.Instance.GetRelayState(3);
         NumatoUsbRelay.Instance.SetRelay(3, !state);      
      }

      private void TargetWheelOffButton_Click(object sender, EventArgs e)
      {
         if (false == this.TargetWheelOffButton.HoldTimeoutEnable)
         {
            DeviceCommunication.Instance.SetTargetMovementMode(MovementModes.off);
         }
      }

      private void TargetWheelOffButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         DeviceCommunication.Instance.SetTargetMovementMode(MovementModes.off);
      }

      private void TargetWheelMoveButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         DeviceCommunication.Instance.SetTargetMovementMode(MovementModes.move);
      }

      private void TargetWheelSpeedToggleButton_Click(object sender, EventArgs e)
      {
         bool selection = !this.TargetWheelSpeedToggleButton.OptionASelected;
         this.targetMovementFastSelected = selection;
         this.TargetWheelSpeedToggleButton.OptionASelected = selection;
      }

      private void TargetRobotJogReverseButton_MouseDown(object sender, MouseEventArgs e)
      {
         DeviceCommunication.Instance.SetTargetMovementPositionRequest(-ParameterAccessor.Instance.TargetWheelManualWheelDistance.OperationalValue);
      }

      private void TargetRobotJogDistanceButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         DialogResult result = this.LaunchNumberEdit(this.TargetRobotJogDistanceButton, ParameterAccessor.Instance.TargetWheelManualWheelDistance, "JOG DISTANCE");
      }

      private void TargetRobotJogForwardButton_MouseDown(object sender, MouseEventArgs e)
      {
         DeviceCommunication.Instance.SetTargetMovementPositionRequest(ParameterAccessor.Instance.TargetWheelManualWheelDistance.OperationalValue);
      }

      private void TargetRobotMoveReverseButton_MouseDown(object sender, MouseEventArgs e)
      {
         double neededSpeed = ParameterAccessor.Instance.TargetWheelManualWheelSpeed.OperationalValue;
         double neededPercent = neededSpeed / ParameterAccessor.Instance.TargetWheelMaximumSpeed.OperationalValue;

         DeviceCommunication.Instance.SetTargetMovementManualMode(true);
         DeviceCommunication.Instance.SetTargetMovementVelocityRequest(-neededPercent, true);
      }

      private void TargetRobotMoveReverseButton_MouseUp(object sender, MouseEventArgs e)
      {
         DeviceCommunication.Instance.SetTargetMovementVelocityRequest(0, false);
         DeviceCommunication.Instance.SetTargetMovementManualMode(false);
      }

      private void TargetRobotMoveSpeedButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         ParameterAccessor.Instance.TargetWheelManualWheelSpeed.MaximumValue = ParameterAccessor.Instance.TargetWheelMaximumSpeed.OperationalValue;
         DialogResult result = this.LaunchNumberEdit(this.TargetRobotMoveSpeedButton, ParameterAccessor.Instance.TargetWheelManualWheelSpeed, "MOVE SPEED");
      }

      private void TargetRobotMoveForwardButton_MouseDown(object sender, MouseEventArgs e)
      {
         double neededSpeed = ParameterAccessor.Instance.TargetWheelManualWheelSpeed.OperationalValue;
         double neededPercent = neededSpeed / ParameterAccessor.Instance.TargetWheelMaximumSpeed.OperationalValue;

         DeviceCommunication.Instance.SetTargetMovementManualMode(true);
         DeviceCommunication.Instance.SetTargetMovementVelocityRequest(neededPercent, true);
      }

      private void TargetRobotMoveForwardButton_MouseUp(object sender, MouseEventArgs e)
      {
         DeviceCommunication.Instance.SetTargetMovementVelocityRequest(0, false);
         DeviceCommunication.Instance.SetTargetMovementManualMode(false);
      }

      private void TargetRobotMotorSetupButton_Click(object sender, EventArgs e)
      {
         Button button = (Button)sender;
         
         TargetRobotMovementSetupForm targetRobotMovementSetupForm = new TargetRobotMovementSetupForm();

         this.SetDialogLocation(button, targetRobotMovementSetupForm);
         this.DimBackground();
         targetRobotMovementSetupForm.ShowDialog();
         this.LightBackground();
      }

      private void TargetRobotMovementJoystickEnableButton_Click(object sender, EventArgs e)
      {
         if (JoystickApplications.targetRobot != this.joystickApplication)
         {
            this.joystickApplication = JoystickApplications.targetRobot;
         }
         else
         {
            this.joystickApplication = JoystickApplications.none;
         }

         this.UpdateJoystickApplicationButtons();
      }

      private void TargetRobotLockButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         bool locked = DeviceCommunication.Instance.GetTargetMovementLock();
         DeviceCommunication.Instance.SetTargetMovementLock(!locked);
         this.UpdateSessionControls();
      }

      #endregion

      #region Laser Actions Events

      private void LaserUpButton_MouseDown(object sender, MouseEventArgs e)
      {
         this.laserLeftStepperChange = 1;
         this.laserRightStepperChange = 1;
      }

      private void LaserUpButton_MouseUp(object sender, MouseEventArgs e)
      {
         this.laserLeftStepperChange = 0;
         this.laserRightStepperChange = 0;
      }

      private void LaserDownButton_MouseDown(object sender, MouseEventArgs e)
      {
         this.laserLeftStepperChange = -1;
         this.laserRightStepperChange = -1;
      }

      private void LaserDownButton_MouseUp(object sender, MouseEventArgs e)
      {
         this.laserLeftStepperChange = 0;
         this.laserRightStepperChange = 0;
      }

      private void LaserLeftButton_MouseDown(object sender, MouseEventArgs e)
      {
         this.laserLeftStepperChange = -1;
         this.laserRightStepperChange = 1;
      }

      private void LaserLeftButton_MouseUp(object sender, MouseEventArgs e)
      {
         this.laserLeftStepperChange = 0;
         this.laserRightStepperChange = 0;
      }

      private void LaserRightButton_MouseDown(object sender, MouseEventArgs e)
      {
         this.laserLeftStepperChange = 1;
         this.laserRightStepperChange = -1;
      }

      private void LaserRightButton_MouseUp(object sender, MouseEventArgs e)
      {
         this.laserLeftStepperChange = 0;
         this.laserRightStepperChange = 0;
      }

      private void LaserCenterButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         DeviceCommunication.Instance.SetLaserCenter();
      }

      private void MeasureSetupButton_Click(object sender, EventArgs e)
      {
         Button button = (Button)sender;

         LaserSetupForm laserSetupForm = new LaserSetupForm();

         this.SetDialogLocation(button, laserSetupForm);
         this.DimBackground();
         laserSetupForm.ShowDialog();
         this.LightBackground();
      }

      private void LaserAimButton_Click(object sender, EventArgs e)
      {
         bool request = !DeviceCommunication.Instance.GetLaserAim();
         DeviceCommunication.Instance.SetLaserAim(request);
      }

      private void MeasureButton_Click(object sender, EventArgs e)
      {
         bool laserMeasureActivity = DeviceCommunication.Instance.GetLaserMeasurementActivity();

         if (false == laserMeasureActivity)
         {
            DeviceCommunication.Instance.StartLaserMeasurement();
            this.measurementRequested = true;
            this.MeasurementValuePanel.ValueText = "";
            this.measurementRecorded = false;

            this.RecordBetweenButton.Enabled = false;
            this.RecordServiceButton.Enabled = false;

            Tracer.WriteHigh(TraceGroup.UI, "", "measure start");

            if (SessionStates.idle != this.sessionState)
            {
               this.sessionState = (SessionStates.start == this.sessionState) ? SessionStates.firstMeasure : SessionStates.measure;
            }

            this.UpdateSessionControls();
         }
         else
         {
            DeviceCommunication.Instance.CancelLaserMeasurement();
            this.measurementRequested = false;
         }
      }

      private void RecordBetweenButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         this.RecordMeasurement(this.measurement, SessionMeasurementData.Types.between);
      }

      private void RecordServiceButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)      
      {
         this.RecordMeasurement(this.measurement, SessionMeasurementData.Types.service);
      }

      private void SessionActivityButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         if (SessionStates.idle == this.sessionState)
         {
            this.SessionDataControl.Reset();

            double laserFront = DeviceCommunication.Instance.GetLaserWheelPositionValue(WheelLocations.front);
            double laserRear = DeviceCommunication.Instance.GetLaserWheelPositionValue(WheelLocations.rear);
            double targetFront = DeviceCommunication.Instance.GetTargetWheelPositionValue(WheelLocations.front);
            double targetRear = DeviceCommunication.Instance.GetTargetWheelPositionValue(WheelLocations.rear);
            double latitude = ParameterAccessor.Instance.Latitude;
            double longitude = ParameterAccessor.Instance.Longitude;
            SessionRecord.Instance.Start(laserFront, laserRear, targetFront, targetRear, latitude, longitude);

            this.sessionState = SessionStates.start;
            this.UpdateSessionControls();

            this.MeasurementValuePanel.ValueText = "";
            this.measurementStarted = false;
            this.measurementRecorded = false;

            this.SessionActivityButton.Text = "COMPLETE";
         }
         else
         {
            MessageForm messageForm = new MessageForm();
            messageForm.Title = "SESSION COMPLETION";
            messageForm.Message = "COMPLETE SESSION?";
            messageForm.Buttons = MessageBoxButtons.OKCancel;

            this.SetDialogLocation(this.SessionActivityButton, messageForm);

            this.DimBackground();
            DialogResult result = messageForm.ShowDialog();
            this.LightBackground();

            if (DialogResult.OK == result)
            {
               double laserFront = DeviceCommunication.Instance.GetLaserWheelPositionValue(WheelLocations.front);
               double laserRear = DeviceCommunication.Instance.GetLaserWheelPositionValue(WheelLocations.rear);
               double targetFront = DeviceCommunication.Instance.GetTargetWheelPositionValue(WheelLocations.front);
               double targetRear = DeviceCommunication.Instance.GetTargetWheelPositionValue(WheelLocations.rear);
               SessionRecord.Instance.Complete(laserFront, laserRear, targetFront, targetRear);

               this.sessionState = SessionStates.idle;
               this.UpdateSessionControls();

               this.SessionActivityButton.Text = "START";
            }
         }

         e.Handled = true;
      }

      #endregion

      #region Target Top Camera Events

      private void TopCameraCounterClockwiseButton_MouseDown(object sender, MouseEventArgs e)
      {
         this.topCameraStepperChange = 1;
      }

      private void TopCameraCounterClockwiseButton_MouseUp(object sender, MouseEventArgs e)
      {
         this.topCameraStepperChange = 0;
      }

      private void TopCameraClockwiseButton_MouseDown(object sender, MouseEventArgs e)
      {
         this.topCameraStepperChange = -1;
      }

      private void TopCameraClockwiseButton_MouseUp(object sender, MouseEventArgs e)
      {
         this.topCameraStepperChange = 0;
      }

      private void TopCameraCenterButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         DeviceCommunication.Instance.SetTargetCenter();
      }

      private void TopCameraJoystickEnableButton_Click(object sender, EventArgs e)
      {
         if (JoystickApplications.topCamera != this.joystickApplication)
         {
            this.joystickApplication = JoystickApplications.topCamera;
         }
         else
         {
            this.joystickApplication = JoystickApplications.none;
         }

         this.UpdateJoystickApplicationButtons();
      }

      #endregion

      #region Video Events

      private void LightSelectButton_Click(object sender, EventArgs e)
      {
#if false
         if (VideoSelectModes.light == this.videoSelectMode)
         {
            this.videoSelectMode = VideoSelectModes.none;
         }
         else
         {
            this.videoSelectMode = VideoSelectModes.light;
         }

         this.UpdateCameraHoldEnable();
         this.UpdateVideoSelectorColor();
#endif
      }

      private void LaserMonitorSelectButton_Click(object sender, EventArgs e)      
      {
#if false
         if (VideoSelectModes.laserCamera == this.videoSelectMode)
         {
            this.videoSelectMode = VideoSelectModes.none;
         }
         else
         {
            this.videoSelectMode = VideoSelectModes.laserCamera;
         }

         this.UpdateCameraHoldEnable();
         this.UpdateVideoSelectorColor();
#endif
      }

      private void TargetMonitorSelectButton_Click(object sender, EventArgs e)
      {
#if false
         if (VideoSelectModes.targetCamera == this.videoSelectMode)
         {
            this.videoSelectMode = VideoSelectModes.none;
         }
         else
         {
            this.videoSelectMode = VideoSelectModes.targetCamera;
         }

         this.UpdateCameraHoldEnable();
         this.UpdateVideoSelectorColor();
#endif
      }

      private void LaserRobotLightEnableButton_Click(object sender, EventArgs e)
      {
         this.laserLightEnabled = !this.laserLightEnabled;
         this.LaserRobotLightEnableButton.BackColor = (false != this.laserLightEnabled) ? Color.Lime : Color.FromArgb(171, 171, 171);

         CameraSelectParameters cameraSelectParameters = ParameterAccessor.Instance.GetCameraSelectParameters(this.selectedLaserCameraButton.Camera);
         int channelMask = (false != this.laserLightEnabled) ? cameraSelectParameters.LightChannelMask : 0;
         DeviceCommunication.Instance.SetCameraLightChannelMask(this.selectedLaserCameraButton.Camera, channelMask);

         this.LaserFrontCameraSelectButton.CenterVisible = DeviceCommunication.Instance.GetCameraLightEnable(Ui.Controls.CameraLocations.laserFront);
         this.LaserRearCameraSelectButton.CenterVisible = DeviceCommunication.Instance.GetCameraLightEnable(Ui.Controls.CameraLocations.laserRear);
      }

      private void TargetRobotLightEnableButton_Click(object sender, EventArgs e)
      {
         this.targetLightEnabled = !this.targetLightEnabled;
         this.TargetRobotLightEnableButton.BackColor = (false != this.targetLightEnabled) ? Color.Lime : Color.FromArgb(171, 171, 171);

         CameraSelectParameters cameraSelectParameters = ParameterAccessor.Instance.GetCameraSelectParameters(this.selectedTargetCameraButton.Camera);
         int channelMask = (false != this.targetLightEnabled) ? cameraSelectParameters.LightChannelMask : 0;
         DeviceCommunication.Instance.SetCameraLightChannelMask(this.selectedTargetCameraButton.Camera, channelMask);

         this.TargetFrontCameraSelectButton.CenterVisible = DeviceCommunication.Instance.GetCameraLightEnable(Ui.Controls.CameraLocations.targetFront);
         this.TargetRearCameraSelectButton.CenterVisible = DeviceCommunication.Instance.GetCameraLightEnable(Ui.Controls.CameraLocations.targetRear);
         this.TargetTopCameraSelectButton.CenterVisible = DeviceCommunication.Instance.GetCameraLightEnable(Ui.Controls.CameraLocations.targetTop);
      }

      private void CameraButton_MouseClick(object sender, MouseEventArgs e)
      {
         Controls.CameraSelectButton button = (Controls.CameraSelectButton)sender;

         if ((Ui.Controls.CameraLocations.laserFront == button.Camera) ||
             (Ui.Controls.CameraLocations.laserRear == button.Camera))
         {
            this.AssignLaserCamera(button);
         }
         else if ((Ui.Controls.CameraLocations.targetFront == button.Camera) ||
                  (Ui.Controls.CameraLocations.targetRear == button.Camera) ||
                  (Ui.Controls.CameraLocations.targetTop == button.Camera))
         {
            this.AssignTargetCamera(button);
         }

#if false
         if (VideoSelectModes.light == this.videoSelectMode)
         {
            button.CenterVisible = !button.CenterVisible;

            if (false != button.CenterVisible)
            {
               DeviceCommunication.Instance.SetCameraLightEnable(button.Camera, true);
            }
            else
            {
               DeviceCommunication.Instance.SetCameraLightEnable(button.Camera, false);
            }
         }
         else if (VideoSelectModes.laserCamera == this.videoSelectMode)
         {
            this.AssignLaserCamera(button);
         }
         else if (VideoSelectModes.targetCamera == this.videoSelectMode)
         {
            this.AssignTargetCamera(button);
         }
#endif
      }

      private void CameraButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         Controls.CameraSelectButton button = (Controls.CameraSelectButton)sender;

//         if (VideoSelectModes.light == this.videoSelectMode)
         if (false != button.CenterVisible)
         {
            int lightLevel = DeviceCommunication.Instance.GetCameraLightLevel(button.Camera);
            ValueParameter value = new ValueParameter("Light", "", 0, 0, 100, 1, 15, lightLevel);

            if (null != value)
            {
#if false
               if (false == button.CenterVisible)
               {
                  button.CenterVisible = true;
                  DeviceCommunication.Instance.SetCameraLightEnable(button.Camera, true);
               }
#endif

               LightIntensitySelectForm intensityForm = new LightIntensitySelectForm();
               this.SetDialogLocation(button, intensityForm);
               intensityForm.LocationText = button.Text;
               intensityForm.IntensityValue = value;
               intensityForm.Camera = button.Camera;
               this.DimBackground();
               intensityForm.ShowDialog();
               this.LightBackground();

               int lightIntensity = (int)intensityForm.IntensityValue.OperationalValue;

               if ((Ui.Controls.CameraLocations.laserFront == button.Camera) ||
                   (Ui.Controls.CameraLocations.laserRear == button.Camera))
               {
                  if (Ui.Controls.CameraLocations.laserFront == button.Camera)
                  {
                     ParameterAccessor.Instance.LaserFrontCamera.LightIntensity = lightIntensity;
                  }
                  else
                  {
                     ParameterAccessor.Instance.LaserRearCamera.LightIntensity = lightIntensity;
                  }

                  this.LaserFrontCameraSelectButton.CenterLevel = lightIntensity;
                  this.LaserRearCameraSelectButton.CenterLevel = lightIntensity;
               }
               else if ((Ui.Controls.CameraLocations.targetFront == button.Camera) ||
                        (Ui.Controls.CameraLocations.targetRear == button.Camera) ||
                        (Ui.Controls.CameraLocations.targetTop == button.Camera))
               {
                  if (Ui.Controls.CameraLocations.targetFront == button.Camera)
                  {
                     ParameterAccessor.Instance.TargetFrontCamera.LightIntensity = lightIntensity;
                  }
                  else if (Ui.Controls.CameraLocations.targetRear == button.Camera)
                  {
                     ParameterAccessor.Instance.TargetRearCamera.LightIntensity = lightIntensity;
                  }
                  else
                  {
                     ParameterAccessor.Instance.TargetTopCamera.LightIntensity = lightIntensity;
                  }

                  this.TargetFrontCameraSelectButton.CenterLevel = lightIntensity;
                  this.TargetRearCameraSelectButton.CenterLevel = lightIntensity;
                  this.TargetTopCameraSelectButton.CenterLevel = lightIntensity;
               }
            }
         }

         e.Handled = true;
      }

      #endregion

      #region System User Actions

      private void StopAllButton_Click(object sender, EventArgs e)
      {
         this.StopAllPanel.BackColor = Color.Red;
         DeviceCommunication.Instance.StopAll();
      }

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


      private void SystemResetButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         this.RestartSystem();
      }

      private void ExitButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         this.processExitNeeded = true;
         this.processStopNeeded = true;
      }

      #endregion

      #region Form Events Process

      private void MainForm_Shown(object sender, EventArgs e)
      {
         this.Process = this.ProcessStart;
         this.UpdateTimer.Interval = 1;
         this.UpdateTimer.Enabled = true;
      }

      private void UpdateTimer_Tick(object sender, EventArgs e)
      {
         this.TickProcess();
      }

      private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
      {
         if (false == this.processStopped)
         {
            this.processExitNeeded = true;
            this.processStopNeeded = true;
            e.Cancel = true;
         }
      }

      private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
      {
      }

      #endregion

      #region Constructor

      public MainForm()
      {
         this.InitializeComponent();

         this.fileTraceListener = new FileTraceListener();
         this.fileTraceListener.LogFilePath = @"c:\logs\e4";
         this.fileTraceListener.MaximumLines = 10000;
         this.fileTraceListener.Prefix = "E4_";
         Trace.Listeners.Add(this.fileTraceListener);

         this.traceListener = new UdpTraceListener("127.0.0.1", 10000);
         Trace.Listeners.Add(this.traceListener);

         Tracer.MaskString = "FFFFFFFF";

         this.cameraButtons = new Controls.CameraSelectButton[5];
         this.cameraButtons[0] = this.LaserFrontCameraSelectButton;
         this.cameraButtons[1] = this.LaserRearCameraSelectButton;
         this.cameraButtons[2] = this.TargetFrontCameraSelectButton;
         this.cameraButtons[3] = this.TargetRearCameraSelectButton;
         this.cameraButtons[4] = this.TargetTopCameraSelectButton;

         this.dimmerForm = new PopupDimmerForm();
         this.dimmerForm.Opacity = 0.65;
         this.dimmerForm.ShowInTaskbar = false;
      }

      #endregion

   }
}
