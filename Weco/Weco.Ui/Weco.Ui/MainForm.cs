
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
         }
         else if (JoystickApplications.laserRobot == this.joystickApplication)
         {
            this.LaserRobotMovementJoystickEnableButton.Text = "MANUAL DRIVE";
            this.LaserRobotMovementJoystickEnableButton.BackColor = Color.Lime;
         }
         else if (JoystickApplications.targetRobot == this.joystickApplication)
         {
            this.LaserRobotMovementJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.LaserRobotMovementJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);
         }
         else if (JoystickApplications.topCamera == this.joystickApplication)
         {
            this.LaserRobotMovementJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.LaserRobotMovementJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);
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
            }

            cameraSelectParameters = ParameterAccessor.Instance.GetCameraSelectParameters(selected.Camera);
            int channelMask = (false != this.targetLightEnabled) ? cameraSelectParameters.LightChannelMask : 0;

            Tracer.WriteHigh(TraceGroup.UI, "", "target camera {0}", selected.Camera.ToString());
            DeviceCommunication.Instance.SetTargetCamera(selected.Camera);
            DeviceCommunication.Instance.SetCameraLightLevel(selected.Camera, cameraSelectParameters.LightIntensity);
            DeviceCommunication.Instance.SetCameraLightChannelMask(selected.Camera, channelMask);
            VideoStampOsd.Instance.SetCameraIdText(2, selected.Text);

            selected.RightVisible = true;
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

         this.LaserRobotLeftButton.BackColor = Color.FromArgb(171, 171, 171);
         this.LaserRobotRightButton.BackColor = Color.FromArgb(171, 171, 171);
         this.LaserRobotFrontWheelCurrentPanel.ValueText = "";
         this.LaserRobotFrontWheelTemperaturePanel.ValueText = "";
         this.LaserRobotTotalPositionPanel.ValueText = "";
         this.LaserRobotRearWheelCurrentPanel.ValueText = "";
         this.LaserRobotRearWheelTemperaturePanel.ValueText = "";
         this.LaserRobotTripPositionPanel.ValueText = "";
         this.LaserRobotJogDistanceButton.ValueText = "";
         this.LaserRobotMoveSpeedButton.ValueText = "";
         this.LaserWheelDirectionalValuePanel.ValueText = "";
         this.LaserWheelDirectionalValuePanel.Direction = Ui.Controls.DirectionalValuePanel.Directions.Idle;
         this.LaserRobotWheelMoveButton.ValueText = "";
         this.LaserRobotWheelMoveButton.LeftArrowVisible = false;
         this.LaserRobotWheelMoveButton.RightArrowVisible = false;
         this.LaserRobotWheelSpeedToggleButton.OptionASelected = this.laserMovementFastSelected;


         SessionRecord.Instance.Reset();


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

         this.LaserRobotJogDistanceButton.ValueText = this.GetValueText(ParameterAccessor.Instance.LaserWheelManualWheelDistance);
         this.LaserRobotMoveSpeedButton.ValueText = this.GetValueText(ParameterAccessor.Instance.LaserWheelManualWheelSpeed);

         for (int i = 0; i < this.cameraButtons.Length; i++)
         {
            this.cameraButtons[i].LeftVisible = false;
            this.cameraButtons[i].CenterVisible = false;
            this.cameraButtons[i].RightVisible = false;
         }

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

            this.LaserUpdateMovementControls();

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
         }

         #endregion

         #region Laser Robot

         string usbRelayFault = NumatoUsbRelay.Instance.FaultReason;
         bool laserLeftPush = NumatoUsbRelay.Instance.GetRelayState(0);
         bool laserRightPush = NumatoUsbRelay.Instance.GetRelayState(1);
         bool laserRobotLocked = DeviceCommunication.Instance.GetLaserMovementLock();


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
      }

      #endregion

      #region Video Events

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

         this.cameraButtons = new Controls.CameraSelectButton[2];
         this.cameraButtons[0] = this.LaserFrontCameraSelectButton;
         this.cameraButtons[1] = this.LaserRearCameraSelectButton;

         this.dimmerForm = new PopupDimmerForm();
         this.dimmerForm.Opacity = 0.65;
         this.dimmerForm.ShowInTaskbar = false;
      }

      #endregion

   }
}
