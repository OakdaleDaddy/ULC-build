
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
         crawler,
         camera,
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

      private bool crawlerTrackLightEnabled;
      private bool crawlerCameraLightEnabled;
      private bool bulletCameraLightEnabled;
      private Controls.CameraSelectButton selectedCrawlerCameraButton;
      private Controls.CameraSelectButton selectedBulletCameraButton;
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
            this.CrawlerJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.CrawlerJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);
            
            this.CameraJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.CameraJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);            
         }
         else if (JoystickApplications.crawler == this.joystickApplication)
         {
            this.CrawlerJoystickEnableButton.Text = "MANUAL DRIVE";
            this.CrawlerJoystickEnableButton.BackColor = Color.Lime;

            this.CameraJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.CameraJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);
         }
         else if (JoystickApplications.camera == this.joystickApplication)
         {
            this.CrawlerJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.CrawlerJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);

            this.CameraJoystickEnableButton.Text = "MANUAL DRIVE";
            this.CameraJoystickEnableButton.BackColor = Color.Lime;
         }
      }

      #endregion

      #region Laser Robot 

      private void LaserUpdateMovementControls()
      {
         MovementModes laserMovementMode = DeviceCommunication.Instance.GetLaserMovementMode();
 
         if (MovementModes.off == laserMovementMode)
         {
            this.CrawlerOffButton.BackColor = Color.Lime;
            this.CrawlerMoveButton.BackColor = Color.FromArgb(171, 171, 171);
         }
         else if (MovementModes.move == laserMovementMode)
         {
            this.CrawlerMoveButton.BackColor = Color.Lime;
            this.CrawlerOffButton.BackColor = Color.FromArgb(171, 171, 171);

            this.CrawlerOffButton.HoldTimeoutEnable = false;
         }
         else if (MovementModes.locked == laserMovementMode)
         {
            this.CrawlerOffButton.BackColor = Color.FromArgb(171, 171, 171);
            this.CrawlerMoveButton.BackColor = Color.FromArgb(171, 171, 171);

            this.CrawlerOffButton.HoldTimeoutEnable = true;
         }
      }
      
      #endregion

      #region Video

      private void AssignCrawlerCamera(Controls.CameraSelectButton selected)
      {
         if (selected != this.selectedCrawlerCameraButton)
         {
            LightSelectParameters cameraSelectParameters;

            if (null != this.selectedCrawlerCameraButton)
            {
               this.selectedCrawlerCameraButton.LeftVisible = false;
               cameraSelectParameters = ParameterAccessor.Instance.GetLightSelectParameters(this.selectedCrawlerCameraButton.SystemLocation);
               cameraSelectParameters.LightIntensity = DeviceCommunication.Instance.GetLightLevel(this.selectedCrawlerCameraButton.SystemLocation);
            }

            cameraSelectParameters = ParameterAccessor.Instance.GetLightSelectParameters(selected.SystemLocation);
            int channelMask = (false != this.crawlerCameraLightEnabled) ? cameraSelectParameters.LightChannelMask : 0;

            Tracer.WriteHigh(TraceGroup.UI, "", "crawler camera {0}", selected.SystemLocation.ToString());
            DeviceCommunication.Instance.SetCrawlerCamera(selected.SystemLocation);
            DeviceCommunication.Instance.SetLightLevel(selected.SystemLocation, cameraSelectParameters.LightIntensity);
            DeviceCommunication.Instance.SetLightChannelMask(selected.SystemLocation, channelMask);
            VideoStampOsd.Instance.SetCameraIdText(1, selected.Text);

            selected.LeftVisible = true;
            this.CrawlerFrontCameraSelectButton.CenterVisible = DeviceCommunication.Instance.GetLightEnable(Ui.Controls.SystemLocations.crawlerFront);
            this.CrawlerFrontCameraSelectButton.CenterLevel = cameraSelectParameters.LightIntensity;
            this.CrawlerRearCameraSelectButton.CenterVisible = DeviceCommunication.Instance.GetLightEnable(Ui.Controls.SystemLocations.crawlerRear);
            this.CrawlerRearCameraSelectButton.CenterLevel = cameraSelectParameters.LightIntensity;
            this.selectedCrawlerCameraButton = selected;
            ParameterAccessor.Instance.CrawlerHubSelectedCamera = selected.SystemLocation;
         }
      }

      private void AssignBulletCamera(Controls.CameraSelectButton selected)
      {
         if (selected != this.selectedBulletCameraButton)
         {
            LightSelectParameters cameraSelectParameters;

            if (null != this.selectedBulletCameraButton)
            {
               this.selectedBulletCameraButton.RightVisible = false;
               cameraSelectParameters = ParameterAccessor.Instance.GetLightSelectParameters(this.selectedBulletCameraButton.SystemLocation);
               cameraSelectParameters.LightIntensity = DeviceCommunication.Instance.GetLightLevel(this.selectedBulletCameraButton.SystemLocation);
            }

            cameraSelectParameters = ParameterAccessor.Instance.GetLightSelectParameters(selected.SystemLocation);
            int channelMask = (false != this.bulletCameraLightEnabled) ? cameraSelectParameters.LightChannelMask : 0;

            Tracer.WriteHigh(TraceGroup.UI, "", "bullet camera {0}", selected.SystemLocation.ToString());
            DeviceCommunication.Instance.SetBulletCamera(selected.SystemLocation);
            DeviceCommunication.Instance.SetLightLevel(selected.SystemLocation, cameraSelectParameters.LightIntensity);
            DeviceCommunication.Instance.SetLightChannelMask(selected.SystemLocation, channelMask);
            VideoStampOsd.Instance.SetCameraIdText(2, selected.Text);

            selected.RightVisible = true;
            this.BulletLeftCameraSelectButton.CenterVisible = DeviceCommunication.Instance.GetLightEnable(Ui.Controls.SystemLocations.bulletLeft);
            this.BulletLeftCameraSelectButton.CenterLevel = cameraSelectParameters.LightIntensity;
            this.BulletRightCameraSelectButton.CenterVisible = DeviceCommunication.Instance.GetLightEnable(Ui.Controls.SystemLocations.bulletRight);
            this.BulletRightCameraSelectButton.CenterLevel = cameraSelectParameters.LightIntensity;
            this.BulletDownCameraSelectButton.CenterVisible = DeviceCommunication.Instance.GetLightEnable(Ui.Controls.SystemLocations.bulletDown);
            this.BulletDownCameraSelectButton.CenterLevel = cameraSelectParameters.LightIntensity;
            this.selectedBulletCameraButton = selected;
            ParameterAccessor.Instance.BulletSelectedCamera = selected.SystemLocation;
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

         this.FeederAlternatePanel.Left = this.FeederPanel.Left;
         this.FeederAlternatePanel.Top = this.GetAbsoluteTop(this.FeederOffButton);
         this.FeederAlternatePanel.Visible = false;

         this.ReelAlternatePanel.Left = this.ReelPanel.Left;
         this.ReelAlternatePanel.Top = this.GetAbsoluteTop(this.ReelOffButton);
         this.ReelAlternatePanel.Visible = false;

         this.laserMovementFastSelected = true;

         this.crawlerTrackLightEnabled = true;
         this.crawlerCameraLightEnabled = true;
         this.bulletCameraLightEnabled = true;
         this.selectedCrawlerCameraButton = null;
         this.selectedBulletCameraButton = null;


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

         this.CrawlerAlternatePanel.Visible = false;

         this.CrawlerLeftMotorCurrentPanel.ValueText = "";
         this.CrawlerLeftMotorTemperaturePanel.ValueText = "";
         this.CrawlerRightMotorCurrentPanel.ValueText = "";
         this.CrawlerRightMotorTemperaturePanel.ValueText = "";
         this.CrawlerJogDistanceButton.ValueText = "";
         this.CrawlerMoveSpeedButton.ValueText = "";
         this.CrawlerDirectionalValuePanel.ValueText = "";
         this.CrawlerDirectionalValuePanel.Direction = Ui.Controls.DirectionalValuePanel.Directions.Idle;
         this.CrawlerMoveButton.ValueText = "";
         this.CrawlerMoveButton.LeftArrowVisible = false;
         this.CrawlerMoveButton.RightArrowVisible = false;
         this.CrawlerSpeedToggleButton.OptionASelected = this.laserMovementFastSelected;


         this.StopAllPanel.BackColor = Color.FromArgb(64, 64, 64);
         this.StopAllPanel.Refresh();
         this.StopAllButton.Refresh();

         this.Width = 1920;
         this.Height = 1080;

         // set next state

         this.UpdateTimer.Interval = 1;

         this.RobotStatusTextBox.Width = (this.TruckStatusTextBox.Left + this.TruckStatusTextBox.Width - this.RobotStatusTextBox.Left);
         this.TruckStatusTextBox.Visible = false;

         this.Process = this.ProcessStarting;
      }

      private void ProcessStarting()
      {
         // read parameters

         ParameterAccessor.Instance.Read(Application.ExecutablePath);
         this.traceListener.SetDestination(ParameterAccessor.Instance.Trace.Address, ParameterAccessor.Instance.Trace.Port);
         Tracer.WriteHigh(TraceGroup.UI, null, "starting");


         // process parameters

         if ((0 == ParameterAccessor.Instance.RobotBus.ConsumerHeartbeatRate) ||
             (0 == ParameterAccessor.Instance.RobotBus.ProducerHeartbeatRate) ||
             (0 == ParameterAccessor.Instance.TruckBus.ConsumerHeartbeatRate) ||
             (0 == ParameterAccessor.Instance.TruckBus.ProducerHeartbeatRate))
         {
            this.HeartbeatsDisabledLabel.Visible = true;
         }

         Joystick.Instance.Id = ParameterAccessor.Instance.JoystickId;

         this.LaserUpdateMovementControls();

         this.CrawlerJogDistanceButton.ValueText = this.GetValueText(ParameterAccessor.Instance.LaserWheelManualWheelDistance);
         this.CrawlerMoveSpeedButton.ValueText = this.GetValueText(ParameterAccessor.Instance.LaserWheelManualWheelSpeed);

         this.CrawlerLeftTrackLightButton.CenterVisible = false;
         this.CrawlerRightTrackLightButton.CenterVisible = false;

         for (int i = 0; i < this.cameraButtons.Length; i++)
         {
            this.cameraButtons[i].LeftVisible = false;
            this.cameraButtons[i].CenterVisible = false;
            this.cameraButtons[i].RightVisible = false;
         }

         this.CrawlerTrackLightEnableButton.BackColor = (false != this.crawlerTrackLightEnabled) ? Color.Lime : Color.FromArgb(171, 171, 171);
         this.CrawlerCameraLightEnableButton.BackColor = (false != this.crawlerCameraLightEnabled) ? Color.Lime : Color.FromArgb(171, 171, 171);
         this.BulletCameraLightEnableButton.BackColor = (false != this.bulletCameraLightEnabled) ? Color.Lime : Color.FromArgb(171, 171, 171);


         DeviceCommunication.Instance.Start();


         // set next state

         this.RobotStatusTextBox.Text = "starting";
         this.RobotStatusTextBox.BackColor = Color.Yellow;

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

            Controls.SystemLocations crawlerHubCameraLocation = ParameterAccessor.Instance.CrawlerHubSelectedCamera;
            Controls.SystemLocations bulletCameraLocation = ParameterAccessor.Instance.BulletSelectedCamera;

            for (int i = 0; i < this.cameraButtons.Length; i++)
            {
               if (crawlerHubCameraLocation == this.cameraButtons[i].SystemLocation)
               {
                  this.AssignCrawlerCamera(this.cameraButtons[i]);
               }

               if (bulletCameraLocation == this.cameraButtons[i].SystemLocation)
               {
                  this.AssignBulletCamera(this.cameraButtons[i]);
               }
            }

            int crawlerLeftTrackLightLevel = ParameterAccessor.Instance.CrawlerLeftLight.LightIntensity;
            int crawlerLeftTrackChannelMask = (false != this.crawlerCameraLightEnabled) ? ParameterAccessor.Instance.CrawlerLeftLight.LightChannelMask : 0;
            this.CrawlerLeftTrackLightButton.CenterLevel = crawlerLeftTrackLightLevel;
            this.CrawlerLeftTrackLightButton.CenterVisible = this.crawlerCameraLightEnabled;
            DeviceCommunication.Instance.SetLightLevel(Ui.Controls.SystemLocations.crawlerLeft, crawlerLeftTrackLightLevel);
            DeviceCommunication.Instance.SetLightChannelMask(Ui.Controls.SystemLocations.crawlerLeft, crawlerLeftTrackChannelMask);

            int crawlerRightTrackLightLevel = ParameterAccessor.Instance.CrawlerRightLight.LightIntensity;
            int crawlerRightTrackChannelMask = (false != this.crawlerCameraLightEnabled) ? ParameterAccessor.Instance.CrawlerRightLight.LightChannelMask : 0;
            this.CrawlerRightTrackLightButton.CenterLevel = crawlerRightTrackLightLevel;
            this.CrawlerRightTrackLightButton.CenterVisible = this.crawlerCameraLightEnabled;
            DeviceCommunication.Instance.SetLightLevel(Ui.Controls.SystemLocations.crawlerRight, crawlerRightTrackLightLevel);
            DeviceCommunication.Instance.SetLightChannelMask(Ui.Controls.SystemLocations.crawlerRight, crawlerRightTrackChannelMask);

            this.LaserUpdateMovementControls();

            this.RobotStatusTextBox.Width = (this.TruckStatusTextBox.Left - 8 - this.RobotStatusTextBox.Left);
            this.TruckStatusTextBox.Visible = true;

            
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
            this.RobotStatusTextBox.Text = "ready";
            this.RobotStatusTextBox.BackColor = Color.Lime;
         }
         else
         {
            this.RobotStatusTextBox.Text = laserStatus;
            this.RobotStatusTextBox.BackColor = (false == laserWarning) ? Color.Red : Color.Yellow;
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
            this.TruckStatusTextBox.Text = "ready";
            this.TruckStatusTextBox.BackColor = Color.Lime;
         }
         else
         {
            this.TruckStatusTextBox.Text = targetStatus;
            this.TruckStatusTextBox.BackColor = (false == targetWarning) ? Color.Red : Color.Yellow;
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

               if (this.joystickApplication == JoystickApplications.crawler)
               {
                  if (0 != joystickYAxis)
                  {
                     this.CrawlerSpeedToggleButton.Enabled = false;
                  }
                  else
                  {
                     this.CrawlerSpeedToggleButton.Enabled = true;
                  }
               }
            }

            if (this.joystickApplication != JoystickApplications.crawler)
            {
               if ((0 != joystickXAxis) ||
                   (0 != joystickYAxis))
               {
                  this.CrawlerJoystickEnableButton.Enabled = false;
               }
               else
               {
                  this.CrawlerJoystickEnableButton.Enabled = true;
               }
            }
         }

         #endregion

         #region Crawler

         string usbRelayFault = NumatoUsbRelay.Instance.FaultReason;
         bool laserLeftPush = NumatoUsbRelay.Instance.GetRelayState(0);
         bool laserRightPush = NumatoUsbRelay.Instance.GetRelayState(1);
         bool laserRobotLocked = DeviceCommunication.Instance.GetLaserMovementLock();


         if (null != usbRelayFault)
         {
         }
         else
         {
         }



         double laserWheelCurrent = 0;
         double laserWheelTemperature = 0;

         laserWheelCurrent = DeviceCommunication.Instance.GetLaserWheelCurrentValue(WheelLocations.front);
         this.CrawlerLeftMotorCurrentPanel.ValueText = this.GetValueText(laserWheelCurrent, 2, "A"); ;

         laserWheelTemperature = DeviceCommunication.Instance.GetLaserWheelTemperatureValue(WheelLocations.front);
         this.CrawlerLeftMotorTemperaturePanel.ValueText = this.GetValueText(laserWheelTemperature, 0, "°C");

         laserWheelCurrent = DeviceCommunication.Instance.GetLaserWheelCurrentValue(WheelLocations.rear);
         this.CrawlerRightMotorCurrentPanel.ValueText = this.GetValueText(laserWheelCurrent, 2, "A"); ;

         laserWheelTemperature = DeviceCommunication.Instance.GetLaserWheelTemperatureValue(WheelLocations.rear);
         this.CrawlerRightMotorTemperaturePanel.ValueText = this.GetValueText(laserWheelTemperature, 0, "°C");


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
            this.CrawlerDirectionalValuePanel.Direction = Ui.Controls.DirectionalValuePanel.Directions.Forward;
         }
         else if (laserMovementValue < 0)
         {
            this.CrawlerDirectionalValuePanel.Direction = Ui.Controls.DirectionalValuePanel.Directions.Reverse;
         }
         else
         {
            this.CrawlerDirectionalValuePanel.Direction = Ui.Controls.DirectionalValuePanel.Directions.Idle;
         }

         this.CrawlerDirectionalValuePanel.ValueText = this.GetValueText(laserMovementStatusDisplayValue, laserMovementParameter);


         bool laserMovementReverse = false;
         bool laserMovementForward = false;
         bool laserMovementSet = false;

         if (JoystickApplications.crawler == this.joystickApplication)
         {
            if (false == this.CrawlerAlternatePanel.Visible)
            {
               this.CrawlerAlternatePanel.Top = this.GetAbsoluteTop(this.CrawlerJogReverseButton);
               this.CrawlerAlternatePanel.Left = this.CrawlerlPanel.Left;

               this.CrawlerAlternatePanel.Visible = true;
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

            if (false != this.CrawlerMoveButton.Enabled)
            {
               double laserMovementDisplayValue = Math.Abs(laserMovementRequestValue);
               this.CrawlerMoveButton.LeftArrowVisible = laserMovementReverse;
               this.CrawlerMoveButton.RightArrowVisible = laserMovementForward;
               this.CrawlerMoveButton.ValueForeColor = (false != laserMovementActivated) ? Color.White : Color.FromKnownColor(KnownColor.ControlDarkDark);
               this.CrawlerMoveButton.ValueText = this.GetValueText(laserMovementDisplayValue, laserMovementParameter);
               laserMovementSet = true;
            }
         }
         else
         {
            if (false != this.CrawlerAlternatePanel.Visible)
            {
               this.CrawlerAlternatePanel.Visible = false;
            }
         }


         if (false == laserMovementSet)
         {
            bool laserMovementManual = DeviceCommunication.Instance.GetLaserMovementManualMode();

            if (false == laserMovementManual)
            {
               DeviceCommunication.Instance.SetLaserMovementVelocityRequest(0, false);
            }

            this.CrawlerMoveButton.LeftArrowVisible = false;
            this.CrawlerMoveButton.RightArrowVisible = false;
            this.CrawlerMoveButton.ValueForeColor = Color.FromKnownColor(KnownColor.ControlDarkDark);
            this.CrawlerMoveButton.ValueText = this.GetValueText(0, laserMovementParameter);
         }

         #endregion

         #region Feeder

         #endregion

         #region Reel

         #endregion

         #region Camera

         if (JoystickApplications.camera == this.joystickApplication)
         {
            if (false != this.CameraCenterButton.Enabled)
            {
               this.CameraUpButton.Enabled = false;
               this.CameraDownButton.Enabled = false;
               this.CameraLeftButton.Enabled = false;
               this.CameraRightButton.Enabled = false;
               this.CameraCenterButton.Enabled = false;
            }
         }
         else
         {
            if (false == this.CameraCenterButton.Enabled)
            {
               this.CameraUpButton.Enabled = true;
               this.CameraDownButton.Enabled = true;
               this.CameraLeftButton.Enabled = true;
               this.CameraRightButton.Enabled = true;
               this.CameraCenterButton.Enabled = true;
            }
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
         this.TruckStatusTextBox.Visible = false;
         this.RobotStatusTextBox.Width = (this.TruckStatusTextBox.Left + this.TruckStatusTextBox.Width - this.RobotStatusTextBox.Left);
         this.RobotStatusTextBox.Text = "stopping";
         this.RobotStatusTextBox.BackColor = Color.Yellow;
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

      #region Crawler Movement Events

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
         if (false == this.CrawlerOffButton.HoldTimeoutEnable)
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
         bool selection = !this.CrawlerSpeedToggleButton.OptionASelected;
         this.laserMovementFastSelected = selection;
         this.CrawlerSpeedToggleButton.OptionASelected = selection;
      }

      private void LaserRobotJogReverseButton_MouseDown(object sender, MouseEventArgs e)
      {
         DeviceCommunication.Instance.SetLaserMovementPositionRequest(-ParameterAccessor.Instance.LaserWheelManualWheelDistance.OperationalValue);
      }

      private void LaserRobotJogDistanceButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         DialogResult result = this.LaunchNumberEdit(this.CrawlerJogDistanceButton, ParameterAccessor.Instance.LaserWheelManualWheelDistance, "JOG DISTANCE");
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
         DialogResult result = this.LaunchNumberEdit(this.CrawlerMoveSpeedButton, ParameterAccessor.Instance.LaserWheelManualWheelSpeed, "MOVE SPEED");
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
         if (JoystickApplications.crawler != this.joystickApplication)
         {
            this.joystickApplication = JoystickApplications.crawler;
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

      #region Feeder Events

      private void FeederShowManualButton_Click(object sender, EventArgs e)
      {
         if (false == this.FeederAlternatePanel.Visible)
         {
            this.FeederAlternatePanel.Visible = true;
         }
      }

      private void FeederHideManualButton_Click(object sender, EventArgs e)
      {
         if (false != this.FeederAlternatePanel.Visible)
         {
            this.FeederAlternatePanel.Visible = false;
         }
      }

      #endregion

      #region Reel Events

      private void ReelShowManualButton_Click(object sender, EventArgs e)
      {
         if (false == this.ReelAlternatePanel.Visible)
         {
            this.ReelAlternatePanel.Visible = true;
         }
      }

      private void ReelHideManualButton_Click(object sender, EventArgs e)
      {
         if (false != this.ReelAlternatePanel.Visible)
         {
            this.ReelAlternatePanel.Visible = false;
         }
      }

      #endregion

      #region Video Events

      private void CrawlerTrackLightEnableButton_Click(object sender, EventArgs e)
      {
         this.crawlerTrackLightEnabled = !this.crawlerTrackLightEnabled;
         this.CrawlerTrackLightEnableButton.BackColor = (false != this.crawlerTrackLightEnabled) ? Color.Lime : Color.FromArgb(171, 171, 171);

         int crawlerLeftTrackChannelMask = (false != this.crawlerTrackLightEnabled) ? ParameterAccessor.Instance.CrawlerLeftLight.LightChannelMask : 0;
         DeviceCommunication.Instance.SetLightChannelMask(Ui.Controls.SystemLocations.crawlerLeft, crawlerLeftTrackChannelMask);

         int crawlerRightTrackChannelMask = (false != this.crawlerTrackLightEnabled) ? ParameterAccessor.Instance.CrawlerRightLight.LightChannelMask : 0;         
         DeviceCommunication.Instance.SetLightChannelMask(Ui.Controls.SystemLocations.crawlerRight, crawlerRightTrackChannelMask);

         this.CrawlerLeftTrackLightButton.CenterVisible = DeviceCommunication.Instance.GetLightEnable(Ui.Controls.SystemLocations.crawlerLeft);
         this.CrawlerRightTrackLightButton.CenterVisible = DeviceCommunication.Instance.GetLightEnable(Ui.Controls.SystemLocations.crawlerRight);
      }

      private void CrawlerCameraLightEnableButton_Click(object sender, EventArgs e)
      {
         this.crawlerCameraLightEnabled = !this.crawlerCameraLightEnabled;
         this.CrawlerCameraLightEnableButton.BackColor = (false != this.crawlerCameraLightEnabled) ? Color.Lime : Color.FromArgb(171, 171, 171);

         LightSelectParameters cameraSelectParameters = ParameterAccessor.Instance.GetLightSelectParameters(this.selectedCrawlerCameraButton.SystemLocation);
         int channelMask = (false != this.crawlerCameraLightEnabled) ? cameraSelectParameters.LightChannelMask : 0;
         DeviceCommunication.Instance.SetLightChannelMask(this.selectedCrawlerCameraButton.SystemLocation, channelMask);

         this.CrawlerFrontCameraSelectButton.CenterVisible = DeviceCommunication.Instance.GetLightEnable(Ui.Controls.SystemLocations.crawlerFront);
         this.CrawlerRearCameraSelectButton.CenterVisible = DeviceCommunication.Instance.GetLightEnable(Ui.Controls.SystemLocations.crawlerRear);
      }

      private void BulletCameraLightEnableButton_Click(object sender, EventArgs e)
      {
         this.bulletCameraLightEnabled = !this.bulletCameraLightEnabled;
         this.BulletCameraLightEnableButton.BackColor = (false != this.bulletCameraLightEnabled) ? Color.Lime : Color.FromArgb(171, 171, 171);

         LightSelectParameters cameraSelectParameters = ParameterAccessor.Instance.GetLightSelectParameters(this.selectedBulletCameraButton.SystemLocation);
         int channelMask = (false != this.bulletCameraLightEnabled) ? cameraSelectParameters.LightChannelMask : 0;
         DeviceCommunication.Instance.SetLightChannelMask(this.selectedBulletCameraButton.SystemLocation, channelMask);

         this.BulletLeftCameraSelectButton.CenterVisible = DeviceCommunication.Instance.GetLightEnable(Ui.Controls.SystemLocations.bulletLeft);
         this.BulletRightCameraSelectButton.CenterVisible = DeviceCommunication.Instance.GetLightEnable(Ui.Controls.SystemLocations.bulletRight);
         this.BulletDownCameraSelectButton.CenterVisible = DeviceCommunication.Instance.GetLightEnable(Ui.Controls.SystemLocations.bulletDown);
      }


      private void CameraButton_MouseClick(object sender, MouseEventArgs e)
      {
         Controls.CameraSelectButton button = (Controls.CameraSelectButton)sender;

         if ((Ui.Controls.SystemLocations.crawlerFront == button.SystemLocation) ||
             (Ui.Controls.SystemLocations.crawlerRear == button.SystemLocation))
         {
            this.AssignCrawlerCamera(button);
         }
         else if ((Ui.Controls.SystemLocations.bulletLeft == button.SystemLocation) ||
                  (Ui.Controls.SystemLocations.bulletRight == button.SystemLocation) ||
                  (Ui.Controls.SystemLocations.bulletDown == button.SystemLocation))
         {
            this.AssignBulletCamera(button);
         }
      }

      private void CameraButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         Controls.CameraSelectButton button = (Controls.CameraSelectButton)sender;

         if (false != button.CenterVisible)
         {
            int lightLevel = DeviceCommunication.Instance.GetLightLevel(button.SystemLocation);
            ValueParameter value = new ValueParameter("Light", "", 0, 0, 100, 1, 15, lightLevel);

            if (null != value)
            {
               LightIntensitySelectForm intensityForm = new LightIntensitySelectForm();
               this.SetDialogLocation(button, intensityForm);
               intensityForm.LocationText = button.Text;
               intensityForm.IntensityValue = value;
               intensityForm.SystemLocation = button.SystemLocation;
               this.DimBackground();
               intensityForm.ShowDialog();
               this.LightBackground();

               int lightIntensity = (int)intensityForm.IntensityValue.OperationalValue;

               if (Ui.Controls.SystemLocations.crawlerLeft == button.SystemLocation)
               {
                  ParameterAccessor.Instance.CrawlerLeftLight.LightIntensity = lightIntensity;
                  this.CrawlerLeftTrackLightButton.CenterLevel = lightIntensity;
               }
               else if (Ui.Controls.SystemLocations.crawlerRight == button.SystemLocation)
               {
                  ParameterAccessor.Instance.CrawlerRightLight.LightIntensity = lightIntensity;
                  this.CrawlerRightTrackLightButton.CenterLevel = lightIntensity;
               }
               else if ((Ui.Controls.SystemLocations.crawlerFront == button.SystemLocation) ||
                        (Ui.Controls.SystemLocations.crawlerRear == button.SystemLocation))
               {
                  if (Ui.Controls.SystemLocations.crawlerFront == button.SystemLocation)
                  {
                     ParameterAccessor.Instance.CrawlerFrontLight.LightIntensity = lightIntensity;
                  }
                  else
                  {
                     ParameterAccessor.Instance.CrawlerRearLight.LightIntensity = lightIntensity;
                  }

                  this.CrawlerFrontCameraSelectButton.CenterLevel = lightIntensity;
                  this.CrawlerRearCameraSelectButton.CenterLevel = lightIntensity;
               }
               else if ((Ui.Controls.SystemLocations.bulletLeft == button.SystemLocation) ||
                        (Ui.Controls.SystemLocations.bulletRight == button.SystemLocation) ||
                        (Ui.Controls.SystemLocations.bulletDown == button.SystemLocation))
               {
                  if (Ui.Controls.SystemLocations.bulletLeft == button.SystemLocation)
                  {
                     ParameterAccessor.Instance.BulletLeftLight.LightIntensity = lightIntensity;
                  }
                  else if (Ui.Controls.SystemLocations.bulletRight == button.SystemLocation)
                  {
                     ParameterAccessor.Instance.BulletRightLight.LightIntensity = lightIntensity;
                  }
                  else
                  {
                     ParameterAccessor.Instance.BulletDownLight.LightIntensity = lightIntensity;
                  }

                  this.BulletLeftCameraSelectButton.CenterLevel = lightIntensity;
                  this.BulletRightCameraSelectButton.CenterLevel = lightIntensity;
                  this.BulletDownCameraSelectButton.CenterLevel = lightIntensity;
               }
            }
         }

         e.Handled = true;
      }

      #endregion

      #region Camera Events

      private void CameraJoystickEnableButton_Click(object sender, EventArgs e)
      {
         if (JoystickApplications.camera != this.joystickApplication)
         {
            this.joystickApplication = JoystickApplications.camera;
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

         this.cameraButtons = new Controls.CameraSelectButton[5];
         this.cameraButtons[0] = this.CrawlerFrontCameraSelectButton;
         this.cameraButtons[1] = this.CrawlerRearCameraSelectButton;
         this.cameraButtons[2] = this.BulletLeftCameraSelectButton;
         this.cameraButtons[3] = this.BulletRightCameraSelectButton;
         this.cameraButtons[4] = this.BulletDownCameraSelectButton;

         this.dimmerForm = new PopupDimmerForm();
         this.dimmerForm.Opacity = 0.65;
         this.dimmerForm.ShowInTaskbar = false;
      }

      #endregion


   }
}
