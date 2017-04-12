
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

      private enum VideoSelectModes
      {
         none,
         light,
         mainCamera,
         auxiliaryCamera,
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

      private VideoSelectModes videoSelectMode;
      private Controls.CameraSelectButton selectedMainCameraButton;
      private Controls.CameraSelectButton selectedAuxiliaryCameraButton;
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

            this.LaserJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.LaserJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);

            this.TargetRobotMovementJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.TargetRobotMovementJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);
         }
         else if (JoystickApplications.laserRobot == this.joystickApplication)
         {
            this.LaserRobotMovementJoystickEnableButton.Text = "MANUAL DRIVE";
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

            this.TargetRobotMovementJoystickEnableButton.Text = "MANUAL DRIVE";
            this.TargetRobotMovementJoystickEnableButton.BackColor = Color.Lime;
         }
         else if (JoystickApplications.laserAim == this.joystickApplication)
         {
            this.LaserRobotMovementJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.LaserRobotMovementJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);

            this.LaserJoystickEnableButton.Text = "MANUAL DRIVE";
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

      #region Video

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
            else if ((VideoSelectModes.mainCamera == this.videoSelectMode) ||
                     (VideoSelectModes.auxiliaryCamera == this.videoSelectMode))
            {
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
            this.MainMonitorSelectButton.BackColor = Color.FromArgb(171, 171, 171);
            this.AuxiliaryMonitorSelectButton.BackColor = Color.FromArgb(171, 171, 171);
         }
         else if (VideoSelectModes.mainCamera == this.videoSelectMode)
         {
            this.LightSelectButton.BackColor = Color.FromArgb(171, 171, 171);
            this.MainMonitorSelectButton.BackColor = Color.Lime;
            this.AuxiliaryMonitorSelectButton.BackColor = Color.FromArgb(171, 171, 171);
         }
         else if (VideoSelectModes.auxiliaryCamera == this.videoSelectMode)
         {
            this.LightSelectButton.BackColor = Color.FromArgb(171, 171, 171);
            this.MainMonitorSelectButton.BackColor = Color.FromArgb(171, 171, 171);
            this.AuxiliaryMonitorSelectButton.BackColor = Color.Lime;
         }
         else
         {
            this.LightSelectButton.BackColor = Color.FromArgb(171, 171, 171);
            this.MainMonitorSelectButton.BackColor = Color.FromArgb(171, 171, 171);
            this.AuxiliaryMonitorSelectButton.BackColor = Color.FromArgb(171, 171, 171);
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

         this.videoSelectMode = VideoSelectModes.none;


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

         this.DateTimeTextPanel.ValueText = "";

         this.LaserRobotAlternateMotionMotorPanel.Visible = false;

         this.LaserRobotFrontWheelCurrentPanel.ValueText = "";
         this.LaserRobotFrontWheelTemperaturePanel.ValueText = "";
         this.LaserRobotFrontWheelPositionPanel.ValueText = "";
         this.LaserRobotRearWheelCurrentPanel.ValueText = "";
         this.LaserRobotRearWheelTemperaturePanel.ValueText = "";
         this.LaserRobotRearWheelPositionPanel.ValueText = "";
         this.LaserRobotMotorLinkVoltagePanel.ValueText = "";
         this.LaserRobotJogDistanceButton.ValueText = "";
         this.LaserRobotMoveSpeedButton.ValueText = "";
         this.LaserWheelDirectionalValuePanel.ValueText = "";
         this.LaserWheelDirectionalValuePanel.Direction = Ui.Controls.DirectionalValuePanel.Directions.Idle;
         this.LaserRobotWheelMoveButton.ValueText = "";
         this.LaserRobotWheelMoveButton.LeftArrowVisible = false;
         this.LaserRobotWheelMoveButton.RightArrowVisible = false;
         this.LaserRobotWheelSpeedToggleButton.OptionASelected = this.laserMovementFastSelected;


         this.TargetRobotAlternateMotionMotorPanel.Visible = false;

         this.TargetRobotFrontWheelCurrentPanel.ValueText = "";
         this.TargetRobotFrontWheelTemperaturePanel.ValueText = "";
         this.TargetRobotFrontWheelPositionPanel.ValueText = "";
         this.TargetRobotRearWheelCurrentPanel.ValueText = "";
         this.TargetRobotRearWheelTemperaturePanel.ValueText = "";
         this.TargetRobotRearWheelPositionPanel.ValueText = "";
         this.TargetRobotMotorLinkVoltagePanel.ValueText = "";
         this.TargetRobotJogDistanceButton.ValueText = "";
         this.TargetRobotMoveSpeedButton.ValueText = "";
         this.TargetWheelDirectionalValuePanel.ValueText = "";
         this.TargetWheelDirectionalValuePanel.Direction = Ui.Controls.DirectionalValuePanel.Directions.Idle;
         this.TargetWheelMoveButton.ValueText = "";
         this.TargetWheelMoveButton.LeftArrowVisible = false;
         this.TargetWheelMoveButton.RightArrowVisible = false;
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

         this.UpdateCameraHoldEnable();
         this.UpdateVideoSelectorColor();

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

         DateTime dateTime = DateTime.Now.ToLocalTime();
         this.DateTimeTextPanel.ValueText = string.Format("{0:D2}-{1:D2}-{2:D4}   {3:D2}:{4:D2}:{5:D2}", dateTime.Month, dateTime.Day, dateTime.Year, dateTime.Hour, dateTime.Minute, dateTime.Second);

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

         double laserWheelCurrent = 0;
         double laserWheelTemperature = 0;
         double laserWheelPosition = 0;

         laserWheelCurrent = DeviceCommunication.Instance.GetLaserWheelCurrentValue(WheelLocations.front);
         this.LaserRobotFrontWheelCurrentPanel.ValueText = this.GetValueText(laserWheelCurrent, 2, "A"); ;

         laserWheelTemperature = DeviceCommunication.Instance.GetLaserWheelTemperatureValue(WheelLocations.front);
         this.LaserRobotFrontWheelTemperaturePanel.ValueText = this.GetValueText(laserWheelTemperature, 0, "°C");

         laserWheelPosition = DeviceCommunication.Instance.GetLaserWheelPositionValue(WheelLocations.front);
         this.LaserRobotFrontWheelPositionPanel.ValueText = this.GetValueText(laserWheelPosition, ParameterAccessor.Instance.LaserWheelManualWheelDistance);

         laserWheelCurrent = DeviceCommunication.Instance.GetLaserWheelCurrentValue(WheelLocations.rear);
         this.LaserRobotRearWheelCurrentPanel.ValueText = this.GetValueText(laserWheelCurrent, 2, "A"); ;

         laserWheelTemperature = DeviceCommunication.Instance.GetLaserWheelTemperatureValue(WheelLocations.rear);
         this.LaserRobotRearWheelTemperaturePanel.ValueText = this.GetValueText(laserWheelTemperature, 0, "°C");

         laserWheelPosition = DeviceCommunication.Instance.GetLaserWheelPositionValue(WheelLocations.rear);
         this.LaserRobotRearWheelPositionPanel.ValueText = this.GetValueText(laserWheelPosition, ParameterAccessor.Instance.LaserWheelManualWheelDistance);

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

         double targetWheelCurrent = 0;
         double targetWheelTemperature = 0;
         double targetWheelPosition = 0;

         targetWheelCurrent = DeviceCommunication.Instance.GetTargetWheelCurrentValue(WheelLocations.front);
         this.TargetRobotFrontWheelCurrentPanel.ValueText = this.GetValueText(targetWheelCurrent, 2, "A"); ;

         targetWheelTemperature = DeviceCommunication.Instance.GetTargetWheelTemperatureValue(WheelLocations.front);
         this.TargetRobotFrontWheelTemperaturePanel.ValueText = this.GetValueText(targetWheelTemperature, 0, "°C");

         targetWheelPosition = DeviceCommunication.Instance.GetTargetWheelPositionValue(WheelLocations.front);
         this.TargetRobotFrontWheelPositionPanel.ValueText = this.GetValueText(targetWheelPosition, ParameterAccessor.Instance.TargetWheelManualWheelDistance);

         targetWheelCurrent = DeviceCommunication.Instance.GetTargetWheelCurrentValue(WheelLocations.rear);
         this.TargetRobotRearWheelCurrentPanel.ValueText = this.GetValueText(targetWheelCurrent, 2, "A"); ;

         targetWheelTemperature = DeviceCommunication.Instance.GetTargetWheelTemperatureValue(WheelLocations.rear);
         this.TargetRobotRearWheelTemperaturePanel.ValueText = this.GetValueText(targetWheelTemperature, 0, "°C");

         targetWheelPosition = DeviceCommunication.Instance.GetTargetWheelPositionValue(WheelLocations.rear);
         this.TargetRobotRearWheelPositionPanel.ValueText = this.GetValueText(targetWheelPosition, ParameterAccessor.Instance.TargetWheelManualWheelDistance);

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

         string laserMeasureFault = LaserCommunicationBus.Instance.GetFaultStatus(LaserCommunicationBus.BusComponentId.LaserBoard);
         string targetMeasureFault = TargetCommunicationBus.Instance.GetFaultStatus(TargetCommunicationBus.BusComponentId.TargetBoard);

         this.LaserPitchTickPanel.ValueText = this.GetValueText(DeviceCommunication.Instance.GetLaserStepperYActualPosition());
         this.LaserYawTickPanel.ValueText = this.GetValueText(DeviceCommunication.Instance.GetLaserStepperXActualPosition());

         double targetPitchValue = DeviceCommunication.Instance.GetTargetPitch();
         this.SensorPitchPanel.ValueText = this.GetValueText(targetPitchValue, 1, "°");
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

      #region Video Events

      private void LightSelectButton_Click(object sender, EventArgs e)
      {
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
      }

      private void MainMonitorSelectButton_Click(object sender, EventArgs e)
      {
         if (VideoSelectModes.mainCamera == this.videoSelectMode)
         {
            this.videoSelectMode = VideoSelectModes.none;
         }
         else
         {
            this.videoSelectMode = VideoSelectModes.mainCamera;
         }

         this.UpdateCameraHoldEnable();
         this.UpdateVideoSelectorColor();
      }

      private void AuxiliaryMonitorSelectButton_Click(object sender, EventArgs e)
      {
         if (VideoSelectModes.auxiliaryCamera == this.videoSelectMode)
         {
            this.videoSelectMode = VideoSelectModes.none;
         }
         else
         {
            this.videoSelectMode = VideoSelectModes.auxiliaryCamera;
         }

         this.UpdateCameraHoldEnable();
         this.UpdateVideoSelectorColor();
      }

      private void CameraButton_MouseClick(object sender, MouseEventArgs e)
      {
         Controls.CameraSelectButton button = (Controls.CameraSelectButton)sender;

         if (VideoSelectModes.light == this.videoSelectMode)
         {
            button.CenterVisible = !button.CenterVisible;

            if (false != button.CenterVisible)
            {
               //DeviceCommunication.Instance.SetCameraLight(true, button.Camera);
            }
            else
            {
               //DeviceCommunication.Instance.SetCameraLight(false, button.Camera);
            }
         }
         else if (VideoSelectModes.mainCamera == this.videoSelectMode)
         {
            //this.AssignMainCamera(button);
         }
         else if (VideoSelectModes.auxiliaryCamera == this.videoSelectMode)
         {
            //this.AssignAuxiliaryCamera(button);
         }
      }

      private void CameraButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         Controls.CameraSelectButton button = (Controls.CameraSelectButton)sender;

         if (VideoSelectModes.light == this.videoSelectMode)
         {
            ValueParameter value = null; // ParameterAccessor.Instance.GetCameraLight(button.Camera);

            if (null != value)
            {
               if (false == button.CenterVisible)
               {
                  button.CenterVisible = true;
//                  DeviceCommunication.Instance.SetCameraLight(true, button.Camera);
               }

               LightIntensitySelectForm intensityForm = new LightIntensitySelectForm();
               this.SetDialogLocation(button, intensityForm);
               intensityForm.LocationText = button.Text;
               intensityForm.IntensityValue = value;
               intensityForm.Camera = button.Camera;
               this.DimBackground();
               intensityForm.ShowDialog();
               this.LightBackground();

               button.CenterLevel = (int)intensityForm.IntensityValue.OperationalValue; // assign all within set the same level
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

         this.cameraButtons = new Controls.CameraSelectButton[6];
         this.cameraButtons[0] = this.LaserFrontCameraSelectButton;
         this.cameraButtons[1] = this.LaserRearCameraSelectButton;
         this.cameraButtons[2] = this.LaserTopCameraSelectButton;
         this.cameraButtons[3] = this.TargetFrontCameraSelectButton;
         this.cameraButtons[4] = this.TargetRearCameraSelectButton;
         this.cameraButtons[5] = this.TargetTopCameraSelectButton;

         this.dimmerForm = new PopupDimmerForm();
         this.dimmerForm.Opacity = 0.65;
         this.dimmerForm.ShowInTaskbar = false;
      }

      #endregion

   }
}
