namespace E4.Ui
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

   using E4.Utilities;

   public partial class MainForm : Form
   {
      #region Definitions

      private delegate void ProcessHandler();

      private enum JoystickApplications
      {
         none,
         laserRobot,
         targetRobot,
         laserAim,
      }

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

      private bool laserMeasurementRequested;
      private bool laserMeasurementRecorded;

      private int laserXButtonChange;
      private int laserXRequestedChange;
      private int laserYButtonChange;
      private int laserYRequestedChange;
      
      private int targetButtonChange;
      private int targetRequestedChange;

      private PopupDimmerForm dimmerForm;

      #endregion

      #region Properties

      private ProcessHandler Process { set; get; }

      #endregion

      #region Helper Functions

      #region General

      private string GetValueText(double operationalValue, ValueParameter parameter)
      {
         string doubleFormat = "N" + parameter.Precision.ToString();
         string result = operationalValue.ToString(doubleFormat) + " " + parameter.Unit;
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

      private string GetValueText(double value, string doubleFormat, string unit)
      {
         string result = value.ToString(doubleFormat) + " " + unit;
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

            this.LaserJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.LaserJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);

            this.TargetRobotMovementJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.TargetRobotMovementJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);
         }
         else if (JoystickApplications.laserRobot == this.joystickApplication)
         {
            this.LaserRobotMovementJoystickEnableButton.Text = "RELEASE JOYSTICK";
            this.LaserRobotMovementJoystickEnableButton.BackColor = Color.Lime;

            this.LaserJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.LaserJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);

            this.TargetRobotMovementJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.TargetRobotMovementJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);
         }
         else if (JoystickApplications.targetRobot == this.joystickApplication)
         {
            this.LaserRobotMovementJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.LaserRobotMovementJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);
            
            this.LaserJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.LaserJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);

            this.TargetRobotMovementJoystickEnableButton.Text = "RELEASE JOYSTICK";
            this.TargetRobotMovementJoystickEnableButton.BackColor = Color.Lime;
         }
         else if (JoystickApplications.laserAim == this.joystickApplication)
         {
            this.LaserRobotMovementJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.LaserRobotMovementJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);

            this.LaserJoystickEnableButton.Text = "RELEASE JOYSTICK";
            this.LaserJoystickEnableButton.BackColor = Color.Lime;

            this.TargetRobotMovementJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.TargetRobotMovementJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);
         }
      }

      #endregion

      #region Laser Robot 

      private void LaserUpdateMovementControls()
      {
         MovementModes laserMovementMode = DeviceCommunication.Instance.GetLaserMovementMode();
 
         if (MovementModes.off == laserMovementMode)
         {
            this.LaserWheelOffButton.BackColor = Color.Lime;
            this.LaserWheelMoveButton.BackColor = Color.FromArgb(171, 171, 171);
         }
         else if (MovementModes.move == laserMovementMode)
         {
            this.LaserWheelMoveButton.BackColor = Color.Lime;
            this.LaserWheelOffButton.BackColor = Color.FromArgb(171, 171, 171);

            this.LaserWheelOffButton.HoldTimeoutEnable = false;
         }
         else if (MovementModes.locked == laserMovementMode)
         {
            this.LaserWheelOffButton.BackColor = Color.FromArgb(171, 171, 171);
            this.LaserWheelMoveButton.BackColor = Color.FromArgb(171, 171, 171);

            this.LaserWheelOffButton.HoldTimeoutEnable = true;
         }
      }

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

         this.laserMovementFastSelected = true;

         this.targetMovementFastSelected = true;

         this.laserMeasurementRequested = false;
         this.laserMeasurementRecorded = false;

         this.laserXButtonChange = 0;
         this.laserXRequestedChange = 0;
         this.laserYButtonChange = 0;
         this.laserYRequestedChange = 0;

         this.targetButtonChange = 0;
         this.targetRequestedChange = 0;


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

         this.LaserWheelDirectionalValuePanel.ValueText = "";
         this.LaserWheelDirectionalValuePanel.Direction = Ui.Controls.DirectionalValuePanel.Directions.Idle;
         this.LaserWheelMoveButton.ValueText = "";
         this.LaserWheelMoveButton.LeftArrowVisible = false;
         this.LaserWheelMoveButton.RightArrowVisible = false;
         this.LaserWheelSpeedToggleButton.OptionASelected = this.laserMovementFastSelected;

         this.TargetWheelDirectionalValuePanel.ValueText = "";
         this.TargetWheelMoveButton.ValueText = "";
         this.TargetWheelMoveButton.LeftArrowVisible = false;
         this.TargetWheelMoveButton.RightArrowVisible = false;
         this.TargetWheelDirectionalValuePanel.Direction = Ui.Controls.DirectionalValuePanel.Directions.Idle;
         this.TargetWheelSpeedToggleButton.OptionASelected = this.targetMovementFastSelected;

         this.LaserPitchTickPanel.ValueText = this.GetValueText(DeviceCommunication.Instance.GetLaserStepperYActualPosition());
         this.LaserYawTickPanel.ValueText = this.GetValueText(DeviceCommunication.Instance.GetLaserStepperXActualPosition());

         this.SensorPitchPanel.ValueText = "";
         this.SensorPitchTickPanel.ValueText = this.GetValueText(DeviceCommunication.Instance.GetTargetStepperActualPosition());

         this.LaserTitleLabel.Text = "LASER MEASURE";
         this.LaserMeasurementValuePanel.ValueText = "";
         this.LaserMeasureButton.Text = "MEASURE";

         this.SensorIndicator.MissColor = Color.FromKnownColor(KnownColor.ControlDark);
         this.SensorIndicator.BackColor = Color.FromKnownColor(KnownColor.ControlDark);
         this.SensorIndicator.CoordinateValue = 0;

         this.StopAllPanel.BackColor = Color.FromArgb(64, 64, 64);
         this.StopAllPanel.Refresh();
         this.StopAllButton.Refresh();


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

         if ((0 == ParameterAccessor.Instance.LaserBus.ConsumerHeartbeatRate) ||
             (0 == ParameterAccessor.Instance.LaserBus.ProducerHeartbeatRate) ||
             (0 == ParameterAccessor.Instance.TargetBus.ConsumerHeartbeatRate) ||
             (0 == ParameterAccessor.Instance.TargetBus.ProducerHeartbeatRate))
         {
            this.HeartbeatsDisabledLabel.Visible = true;
         }

         this.LaserUpdateMovementControls();
         this.TargetUpdateMovementControls();

         this.LaserRangeJoystickXRequestIndicator.Position = 0;
         this.LaserRangeJoystickXRequestIndicator.MaximumPosition = 32767 - ParameterAccessor.Instance.JoystickDeadband;
         this.LaserRangeJoystickXRequestIndicator.MinimumPosition = -32767 + ParameterAccessor.Instance.JoystickDeadband;

         this.LaserRangeJoystickYRequestIndicator.Position = 0;
         this.LaserRangeJoystickYRequestIndicator.MaximumPosition = 32767 - ParameterAccessor.Instance.JoystickDeadband;
         this.LaserRangeJoystickYRequestIndicator.MinimumPosition = -32767 + ParameterAccessor.Instance.JoystickDeadband;

         this.LaserScannerJoystickYRequestIndicator.Position = 0;
         this.LaserScannerJoystickYRequestIndicator.MaximumPosition = 32767 - ParameterAccessor.Instance.JoystickDeadband;
         this.LaserScannerJoystickYRequestIndicator.MinimumPosition = -32767 + ParameterAccessor.Instance.JoystickDeadband;

         DeviceCommunication.Instance.Start();


         // set next state

         this.LaserStatusTextBox.Text = "starting";
         this.LaserStatusTextBox.BackColor = Color.Yellow;

         this.UpdateTimer.Interval = 100;
         this.Process = this.ProcessWaitComm;
      }

      private void ProcessWaitComm()
      {
         if (false != this.processExitNeeded)
         {
            this.Process = this.ProcessStopping;
         }
         else if (false != DeviceCommunication.Instance.Ready)
         {
            // initialize based on hardware

            this.LaserUpdateMovementControls();
            this.TargetUpdateMovementControls();

            Tracer.WriteHigh(TraceGroup.UI, null, "started");
            this.Process = this.ProcessExecution;

            this.LaserStatusTextBox.Width = (this.TargetStatusTextBox.Left - 8 - this.LaserStatusTextBox.Left);
            this.TargetStatusTextBox.Visible = true;
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

         #endregion

         #region Laser Status

         string laserStatus = null;
         bool laserWarning = false;

         if (null != Joystick.Instance.FaultReason)
         {
            laserStatus = "joystick missing";
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
            this.LaserStatusTextBox.BackColor = Color.LimeGreen;
         }
         else
         {
            this.LaserStatusTextBox.Text = laserStatus;
            this.LaserStatusTextBox.BackColor = (false == laserWarning) ? Color.Red : Color.Yellow;
         }

         #endregion

         #region Target Status

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
            this.TargetStatusTextBox.BackColor = Color.LimeGreen;
         }
         else
         {
            this.TargetStatusTextBox.Text = targetStatus;
            this.TargetStatusTextBox.BackColor = (false == targetWarning) ? Color.Red : Color.Yellow;
         }

         #endregion

         #region Joystick and Movement

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

            this.LaserRangeJoystickXRequestIndicator.Position = joystickXAxis;
            this.LaserRangeJoystickYRequestIndicator.Position = joystickYAxis;
            this.LaserScannerJoystickYRequestIndicator.Position = joystickThrottle;

            if (JoystickApplications.none != this.joystickApplication)
            {
               joystickXChange = joystickXAxis;
               joystickYChange = joystickYAxis;
               joystickThrottleChange = joystickThrottle;

               if (this.joystickApplication == JoystickApplications.laserRobot)
               {
                  if (0 != joystickYAxis)
                  {
                     this.LaserWheelSpeedToggleButton.Enabled = false;
                  }
                  else
                  {
                     this.LaserWheelSpeedToggleButton.Enabled = true;
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

            if (this.joystickApplication != JoystickApplications.laserAim)
            {
               if ((0 != joystickXAxis) ||
                   (0 != joystickYAxis) ||
                   (0 != joystickThrottle))
               {            
                  this.LaserJoystickEnableButton.Enabled = false;
               }
               else
               {
                  this.LaserJoystickEnableButton.Enabled = true;
               }
            }
         }

         #endregion

         #region Laser Robot

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
            DeviceCommunication.Instance.SetLaserMovementRequest(laserMovementRequestPercent, laserMovementTriggered);

            bool laserMovementActivated = DeviceCommunication.Instance.GetLaserMovementActivated();

            if (false != this.TargetWheelMoveButton.Enabled)
            {
               double laserMovementDisplayValue = Math.Abs(laserMovementRequestValue);
               this.LaserWheelMoveButton.LeftArrowVisible = laserMovementReverse;
               this.LaserWheelMoveButton.RightArrowVisible = laserMovementForward;
               this.LaserWheelMoveButton.ValueForeColor = (false != laserMovementActivated) ? Color.White : Color.FromKnownColor(KnownColor.ControlDarkDark);
               this.LaserWheelMoveButton.ValueText = this.GetValueText(laserMovementDisplayValue, laserMovementParameter);
               laserMovementSet = true;
            }
         }

         if (false == laserMovementSet)
         {
            DeviceCommunication.Instance.SetLaserMovementRequest(0, false);
            this.LaserWheelMoveButton.LeftArrowVisible = false;
            this.LaserWheelMoveButton.RightArrowVisible = false;
            this.LaserWheelMoveButton.ValueForeColor = Color.FromKnownColor(KnownColor.ControlDarkDark);
            this.LaserWheelMoveButton.ValueText = this.GetValueText(0, laserMovementParameter);
         }

         #endregion

         #region Target Robot

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
            DeviceCommunication.Instance.SetTargetMovementRequest(targetMovementRequestPercent, targetMovementTriggered);

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

         if (false == targetMovementSet)
         {
            this.TargetWheelMoveButton.LeftArrowVisible = false;
            this.TargetWheelMoveButton.RightArrowVisible = false;
            this.TargetWheelMoveButton.ValueForeColor = Color.FromKnownColor(KnownColor.ControlDarkDark);
            this.TargetWheelMoveButton.ValueText = this.GetValueText(0, targetMovementParameter);
         }

         #endregion

         #region Laser Measurement

         string laserMeasureFault = LaserCommunicationBus.Instance.GetFaultStatus(LaserCommunicationBus.BusComponentId.LaserBoard);
         string targetMeasureFault = TargetCommunicationBus.Instance.GetFaultStatus(TargetCommunicationBus.BusComponentId.TargetBoard);

         this.LaserPitchTickPanel.ValueText = this.GetValueText(DeviceCommunication.Instance.GetLaserStepperYActualPosition());
         this.LaserYawTickPanel.ValueText = this.GetValueText(DeviceCommunication.Instance.GetLaserStepperXActualPosition());

         double targetPitchValue = DeviceCommunication.Instance.GetTargetPitch();
         this.SensorPitchPanel.ValueText = this.GetValueText(targetPitchValue, "N1", "°");
         this.SensorPitchTickPanel.ValueText = this.GetValueText(DeviceCommunication.Instance.GetTargetStepperActualPosition());

         if ((null == laserMeasureFault) && (null == targetMeasureFault))
         {
            this.LaserMeasureButton.BackColor = Color.FromArgb(171, 171, 171);
            
            bool laserAimActive = DeviceCommunication.Instance.GetLaserAim();
            this.LaserAimButton.BackColor = (false != laserAimActive) ? Color.LimeGreen : Color.FromArgb(171, 171, 171);

            bool laserReadingReady = DeviceCommunication.Instance.GetLaserMeasurementReady();
            bool laserMeasureActive = DeviceCommunication.Instance.GetLaserMeasurementActivity();

            if (false != this.laserMeasurementRequested)
            {
               if (false != laserReadingReady)
               {
                  double laserMeasurement = DeviceCommunication.Instance.GetAverageLaserMeasurement();
                  this.LaserMeasurementValuePanel.BackColor = Color.Black;
                  this.LaserMeasurementValuePanel.ValueText = this.GetValueText(laserMeasurement, ParameterAccessor.Instance.LaserMeasurementConstant);
                  this.LaserTitleLabel.Text = "LASER MEASURE - COMPLETE";

                  this.LaserMeasureButton.Text = "MEASURE";
               }
               else
               {
                  this.LaserMeasurementValuePanel.BackColor = (false != this.messageFlasher) ? Color.DodgerBlue : Color.SteelBlue;

                  if (false != laserMeasureActive)
                  {
                     int laserMeasureRemainingCount = DeviceCommunication.Instance.GetLaserSampleRemainingCount();
                     string laserMeasureStatusText = string.Format("LASER MEASURE - READING ({0})", laserMeasureRemainingCount);
                     this.LaserTitleLabel.Text = laserMeasureStatusText;
                  }
                  else
                  {
                     string laserMeasureStatusText = string.Format("LASER MEASURE - STARTING");
                     this.LaserMeasurementValuePanel.ValueText = "";
                     this.laserMeasurementRecorded = false;
                  }

                  this.LaserMeasureButton.Text = "CANCEL";
               }
            }
            else
            {
               this.LaserTitleLabel.Text = "LASER MEASURE";
               this.LaserMeasureButton.Text = "MEASURE";
               this.LaserMeasurementValuePanel.BackColor = Color.Black;
            }
         }
         else
         {
            this.LaserMeasureButton.BackColor = Color.Red;
            this.LaserTitleLabel.Text = "LASER MEASURE - FAULTED";
         }


         this.RecordLaserMeasurementButton.Enabled = ((DeviceCommunication.Instance.GetLaserMeasurementReady() != false) && (false == this.laserMeasurementRecorded)) ? true : false;

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

         #endregion

         #region Laser Aiming

         int laserStepperXPosition = DeviceCommunication.Instance.GetLaserStepperXActualPosition();
         int laserStepperYPosition = DeviceCommunication.Instance.GetLaserStepperYActualPosition();
         int targetStepperPosition = DeviceCommunication.Instance.GetTargetStepperActualPosition();

         int laserXChange = this.laserXButtonChange;
         int laserYChange = this.laserYButtonChange;
         int targetChange = this.targetButtonChange;

         if (JoystickApplications.laserAim == this.joystickApplication)
         {
            laserXChange = joystickXChange;
            laserYChange = joystickYChange;
            targetChange = joystickThrottleChange;
         }

         if (laserXChange > 0)
         {
            laserStepperXPosition = ParameterAccessor.Instance.LaserXStepper.MaximumPosition;
         }
         else if (laserXChange < 0)
         {
            laserStepperXPosition = ParameterAccessor.Instance.LaserXStepper.MinimumPosition;
         }

         if (laserYChange > 0)
         {
            laserStepperYPosition = ParameterAccessor.Instance.LaserYStepper.MaximumPosition;
         }
         else if (laserYChange < 0)
         {
            laserStepperYPosition = ParameterAccessor.Instance.LaserYStepper.MinimumPosition;
         }

         if (targetChange > 0)
         {
            targetStepperPosition = ParameterAccessor.Instance.TargetStepper.MaximumPosition;
         }
         else if (targetChange < 0)
         {
            targetStepperPosition = ParameterAccessor.Instance.TargetStepper.MinimumPosition;
         }

         if (laserXChange != this.laserXRequestedChange)
         {
            if (0 != laserXChange)
            {
               DeviceCommunication.Instance.SetLaserStepperXPosition(laserStepperXPosition);
            }
            else
            {
               DeviceCommunication.Instance.StopLaserStepperX();
            }

            this.laserXRequestedChange = laserXChange;
         }

         if (laserYChange != this.laserYRequestedChange)
         {
            if (0 != laserYChange)
            {
               DeviceCommunication.Instance.SetLaserStepperYPosition(laserStepperYPosition);
            }
            else
            {
               DeviceCommunication.Instance.StopLaserStepperY();
            }

            this.laserYRequestedChange = laserYChange;
         }

         if (targetChange != this.targetRequestedChange)
         {
            if (0 != targetChange)
            {
               DeviceCommunication.Instance.SetTargetStepperPosition(targetStepperPosition);
            }
            else
            {
               DeviceCommunication.Instance.StopTargetStepper();
            }

            this.targetRequestedChange = targetChange;
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

      private void LaserWheelOffButton_Click(object sender, EventArgs e)
      {
         if (false == this.LaserWheelOffButton.HoldTimeoutEnable)
         {
            DeviceCommunication.Instance.SetLaserMovementMode(MovementModes.off);
         }
      }

      private void LaserWheelOffButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         DeviceCommunication.Instance.SetLaserMovementMode(MovementModes.off);
      }

      private void LaserWheelMoveButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         DeviceCommunication.Instance.SetLaserMovementMode(MovementModes.move);
      }

      private void LaserWheelSpeedToggleButton_Click(object sender, EventArgs e)
      {
         bool selection = !this.LaserWheelSpeedToggleButton.OptionASelected;
         this.laserMovementFastSelected = selection;
         this.LaserWheelSpeedToggleButton.OptionASelected = selection;
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

      #endregion

      #region Target Robot Movement Events

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

      #endregion

      #region Laser Actions Events

      private void SensorUpButton_MouseDown(object sender, MouseEventArgs e)
      {
         this.targetButtonChange = 1;
      }

      private void SensorUpButton_MouseUp(object sender, MouseEventArgs e)
      {
         this.targetButtonChange = 0;
      }

      private void SensorDownButton_MouseDown(object sender, MouseEventArgs e)
      {
         this.targetButtonChange = -1;
      }

      private void SensorDownButton_MouseUp(object sender, MouseEventArgs e)
      {
         this.targetButtonChange = 0;      
      }

      private void SensorCenterButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         DeviceCommunication.Instance.SetTargetCenter();
      }

      private void LaserUpButton_MouseDown(object sender, MouseEventArgs e)
      {
         this.laserYButtonChange = 1;
      }

      private void LaserUpButton_MouseUp(object sender, MouseEventArgs e)
      {
         this.laserYButtonChange = 0;
      }

      private void LaserDownButton_MouseDown(object sender, MouseEventArgs e)
      {
         this.laserYButtonChange = -1;
      }

      private void LaserDownButton_MouseUp(object sender, MouseEventArgs e)
      {
         this.laserYButtonChange = 0;
      }

      private void LaserLeftButton_MouseDown(object sender, MouseEventArgs e)
      {
         this.laserXButtonChange = 1;
      }

      private void LaserLeftButton_MouseUp(object sender, MouseEventArgs e)
      {
         this.laserXButtonChange = 0;
      }

      private void LaserRightButton_MouseDown(object sender, MouseEventArgs e)
      {
         this.laserXButtonChange = -1;
      }

      private void LaserRightButton_MouseUp(object sender, MouseEventArgs e)
      {
         this.laserXButtonChange = 0;
      }

      private void TargetCenterButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         DeviceCommunication.Instance.SetLaserCenter();
      }

      private void RecordLaserMeasurementButton_Click(object sender, EventArgs e)
      {
         this.laserMeasurementRecorded = true;
      }

      private void LaserSetupButton_Click(object sender, EventArgs e)
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

      private void LaserMeasureButton_Click(object sender, EventArgs e)
      {
         bool laserMeasureActivity = DeviceCommunication.Instance.GetLaserMeasurementActivity();

         if (false == laserMeasureActivity)
         {
            DeviceCommunication.Instance.StartLaserMeasurement();
            this.laserMeasurementRequested = true;
         }
         else
         {
            DeviceCommunication.Instance.CancelLaserMeasurement();
            this.laserMeasurementRequested = false;
         }
      }

      private void LaserJoystickEnableButton_Click(object sender, EventArgs e)
      {
         if (JoystickApplications.laserAim != this.joystickApplication)
         {
            this.joystickApplication = JoystickApplications.laserAim;
         }
         else
         {
            this.joystickApplication = JoystickApplications.none;
         }

         this.UpdateJoystickApplicationButtons();
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
      
         this.dimmerForm = new PopupDimmerForm();
         this.dimmerForm.Opacity = 0.65;
         this.dimmerForm.ShowInTaskbar = false;
      }

      #endregion

   }
}
