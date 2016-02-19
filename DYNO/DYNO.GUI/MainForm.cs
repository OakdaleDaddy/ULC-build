
namespace DYNO.GUI
{
   using System;
   using System.Collections;
   using System.Diagnostics;
   using System.Drawing;
   using System.Windows.Forms;

   using Microsoft.Win32;

   using DYNO.PCANLight;
   using DYNO.Utilities;

   public partial class MainForm : Form
   {
      #region Definition

      private const string RegistryCompanyName = "ULC Robotics";
      private const string RegistryApplicationName = "DYNO GUI";

      #endregion

      #region Fields

      private bool active;
      private bool testComplete;
      private Queue traceQueue;
      private DynoTest dynoTest;

      private bool settingTraceMask;

      private string titleString;

      #endregion

      #region Registry Functions

      private RegistryKey GetAppKey()
      {
         RegistryKey softwareKey;
         RegistryKey ownerKey;
         RegistryKey appKey;

         softwareKey = Registry.CurrentUser.OpenSubKey("Software", RegistryKeyPermissionCheck.ReadWriteSubTree);

         if (null == softwareKey)
         {
            softwareKey = Registry.CurrentUser.CreateSubKey("Software", RegistryKeyPermissionCheck.ReadWriteSubTree);
         }

         ownerKey = softwareKey.OpenSubKey(RegistryCompanyName, RegistryKeyPermissionCheck.ReadWriteSubTree);

         if (null == ownerKey)
         {
            ownerKey = softwareKey.CreateSubKey(RegistryCompanyName, RegistryKeyPermissionCheck.ReadWriteSubTree);
         }

         appKey = ownerKey.OpenSubKey(RegistryApplicationName, RegistryKeyPermissionCheck.ReadWriteSubTree);

         if (null == appKey)
         {
            appKey = ownerKey.CreateSubKey(RegistryApplicationName, RegistryKeyPermissionCheck.ReadWriteSubTree);
         }

         return (appKey);
      }

      private void ValidateWindowLocation(ref int top, ref int left, int defaultValueTop, int defaultValueLeft)
      {
         int topResult = defaultValueTop;
         int leftResult = defaultValueLeft;

         for (int i = 0; i < Screen.AllScreens.Length; i++)
         {
            int minimumLeft = Screen.AllScreens[i].Bounds.X;
            int maximumLeft = minimumLeft + Screen.AllScreens[i].Bounds.Width;
            int minimumTop = Screen.AllScreens[i].Bounds.Y;
            int maximumTop = minimumTop + Screen.AllScreens[i].Bounds.Height;

            if ((top >= minimumTop) && (top <= maximumTop) &&
                (left >= minimumLeft) && (left <= maximumLeft))
            {
               topResult = top;
               leftResult = left;

               break;
            }
         }

         top = topResult;
         left = leftResult;
      }

      private void LoadRegistry()
      {
         RegistryKey appKey;
         object keyValue;
         int defaultValue;
         int parsedValue;

         appKey = this.GetAppKey();

         #region Window Location

         keyValue = appKey.GetValue("Height");
         defaultValue = this.MinimumSize.Height;
         this.Height = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : defaultValue) : defaultValue;

         keyValue = appKey.GetValue("Width");
         defaultValue = this.MinimumSize.Width;
         this.Width = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : defaultValue) : defaultValue;

         keyValue = appKey.GetValue("Top");
         int defaultValueTop = (SystemInformation.PrimaryMonitorSize.Height - this.Height) / 2;
         int top = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : defaultValue) : defaultValue;

         keyValue = appKey.GetValue("Left");
         int defaultValueLeft = (SystemInformation.PrimaryMonitorSize.Width - this.Width) / 2;
         int left = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : defaultValue) : defaultValue;

         this.ValidateWindowLocation(ref top, ref left, defaultValueTop, defaultValueLeft);
         this.Top = top;
         this.Left = left;

         keyValue = appKey.GetValue("WindowState");
         this.WindowState = ((null != keyValue) && Enum.IsDefined(this.WindowState.GetType(), keyValue)) ? (FormWindowState)Enum.Parse(this.WindowState.GetType(), keyValue.ToString()) : FormWindowState.Normal;

         #endregion

         #region Session Information

         keyValue = appKey.GetValue("MainControlHeight");
         int defaultMainControlHeight = this.MainTabControl.Height;
         this.MainTabControl.Height = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : defaultMainControlHeight) : defaultMainControlHeight;
         
         keyValue = appKey.GetValue("RunTime");
         this.RunTimeTextBox.Text = (null != keyValue) ? keyValue.ToString() : "0/0/15";

         keyValue = appKey.GetValue("WheelSpeed");
         this.WheelSpeedTextBox.Text = (null != keyValue) ? keyValue.ToString() : "1";
         
         keyValue = appKey.GetValue("WheelStartLoad");
         this.WheelStartLoadTextBox.Text = (null != keyValue) ? keyValue.ToString() : "0.5";
         
         keyValue = appKey.GetValue("WheelStopLoad");
         this.WheelStopLoadTextBox.Text = (null != keyValue) ? keyValue.ToString() : "2.5";
         
         keyValue = appKey.GetValue("CurrentLimit");
         this.CurrentLimitTextBox.Text = (null != keyValue) ? keyValue.ToString() : "5.0";
         
         keyValue = appKey.GetValue("ThermalLimit");
         this.ThermalLimitTextBox.Text = (null != keyValue) ? keyValue.ToString() : "75";

         keyValue = appKey.GetValue("SlippageLimit");
         this.SlippageLimitTextBox.Text = (null != keyValue) ? keyValue.ToString() : "35";


         keyValue = appKey.GetValue("BusInterface");
         this.BusInterfaceComboBox.SelectedIndex = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 0) : 0;

         keyValue = appKey.GetValue("BaudRate");
         this.BaudComboBox.SelectedIndex = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 6) : 6;

         keyValue = appKey.GetValue("ConsumerHeartbeatNodeId");
         this.ConsumerHeartbeatNodeIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "80";

         keyValue = appKey.GetValue("ConsumerHeartbeatTime");
         this.ConsumerHeartbeatTimeTextBox.Text = (null != keyValue) ? keyValue.ToString() : "3000";

         keyValue = appKey.GetValue("ProducerHeartbeatTime");
         this.ProducerHeartbeatTimeTextBox.Text = (null != keyValue) ? keyValue.ToString() : "1000";

         keyValue = appKey.GetValue("UutId");
         this.UutIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "49";

         keyValue = appKey.GetValue("EncoderId");
         this.EncoderIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "22";

         keyValue = appKey.GetValue("AnalogIoId");
         this.AnalogIoIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "19";

         keyValue = appKey.GetValue("DigitalIoId");
         this.DigitalIoIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "23";


         keyValue = appKey.GetValue("UutSpeedToRpm");
         this.UutRpmToSpeedTextBox.Text = (null != keyValue) ? keyValue.ToString() : "1.0";

         keyValue = appKey.GetValue("BodySpeedToRpm");
         this.BodyRpmToSpeedTextBox.Text = (null != keyValue) ? keyValue.ToString() : "1.0";

         keyValue = appKey.GetValue("AnalogIoVoltsToSupplyAmpsSlope");
         this.AnalogIoVoltsToSupplyAmpsSlopeTextBox.Text = (null != keyValue) ? keyValue.ToString() : "1.0";

         keyValue = appKey.GetValue("AnalogIoVoltsToSupplyAmpsOffset");
         this.AnalogIoVoltsToSupplyAmpsOffsetTextBox.Text = (null != keyValue) ? keyValue.ToString() : "0.0";         

         keyValue = appKey.GetValue("AnalogIoVoltsToLoadPounds");
         this.AnalogIoVoltsToLoadPoundsTextBox.Text = (null != keyValue) ? keyValue.ToString() : "1.0";


         keyValue = appKey.GetValue("TraceMask");
         this.TraceMaskTextBox.Text = (null != keyValue) ? keyValue.ToString() : "0";


         keyValue = appKey.GetValue("AutoScroll");
         this.AutoScrollCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         #endregion
      }

      private void SaveRegistry()
      {
         RegistryKey appKey;

         appKey = this.GetAppKey();

         #region Window Location

         if (this.WindowState != FormWindowState.Minimized)
         {
            appKey.SetValue("Height", this.Height);
            appKey.SetValue("Width", this.Width);
            appKey.SetValue("Top", this.Top);
            appKey.SetValue("Left", this.Left);
         }

         appKey.SetValue("WindowState", this.WindowState);

         #endregion

         #region Session Information

         appKey.SetValue("MainControlHeight", this.MainTabControl.Height);

         appKey.SetValue("RunTime", this.RunTimeTextBox.Text);
         appKey.SetValue("WheelSpeed", this.WheelSpeedTextBox.Text);
         appKey.SetValue("WheelStartLoad", this.WheelStartLoadTextBox.Text);
         appKey.SetValue("WheelStopLoad", this.WheelStopLoadTextBox.Text);
         appKey.SetValue("CurrentLimit", this.CurrentLimitTextBox.Text);
         appKey.SetValue("ThermalLimit", this.ThermalLimitTextBox.Text);
         appKey.SetValue("SlippageLimit", this.SlippageLimitTextBox.Text);
         
         appKey.SetValue("BusInterface", this.BusInterfaceComboBox.SelectedIndex);
         appKey.SetValue("BaudRate", this.BaudComboBox.SelectedIndex);
         appKey.SetValue("ConsumerHeartbeatNodeId", this.ConsumerHeartbeatNodeIdTextBox.Text);
         appKey.SetValue("ConsumerHeartbeatTime", this.ConsumerHeartbeatTimeTextBox.Text);
         appKey.SetValue("ProducerHeartbeatTime", this.ProducerHeartbeatTimeTextBox.Text);
         appKey.SetValue("UutId", this.UutIdTextBox.Text);
         appKey.SetValue("EncoderId", this.EncoderIdTextBox.Text);
         appKey.SetValue("AnalogIoId", this.AnalogIoIdTextBox.Text);
         appKey.SetValue("DigitalIoId", this.DigitalIoIdTextBox.Text);

         appKey.SetValue("UutSpeedToRpm", this.UutRpmToSpeedTextBox.Text);
         appKey.SetValue("BodySpeedToRpm", this.BodyRpmToSpeedTextBox.Text);
         appKey.SetValue("AnalogIoVoltsToSupplyAmpsSlope", this.AnalogIoVoltsToSupplyAmpsSlopeTextBox.Text);
         appKey.SetValue("AnalogIoVoltsToSupplyAmpsOffset", this.AnalogIoVoltsToSupplyAmpsOffsetTextBox.Text);
         appKey.SetValue("AnalogIoVoltsToLoadPounds", this.AnalogIoVoltsToLoadPoundsTextBox.Text);

         appKey.SetValue("TraceMask", this.TraceMaskTextBox.Text);

         appKey.SetValue("AutoScroll", this.AutoScrollCheckBox.Checked ? "1" : "0");

         #endregion
      }

      #endregion

      #region Delegates

      private void OnSetTitle(string title)
      {
         this.titleString = title;
      }

      private void OnTestComplete()
      {
         this.testComplete = true;
      }

      #endregion

      #region Helper Functions

      private void UnlockForIdle()
      {
         this.ActivityButton.Text = "Start";

         this.RunTimeTextBox.Enabled = true;
         this.WheelSpeedTextBox.Enabled = true;
         this.WheelStartLoadTextBox.Enabled = true;
         this.WheelStopLoadTextBox.Enabled = true;
         this.CurrentLimitTextBox.Enabled = true;
         this.ThermalLimitTextBox.Enabled = true;
         this.SlippageLimitTextBox.Enabled = true;

         this.BaudComboBox.Enabled = true;
         this.BusInterfaceComboBox.Enabled = true;
         this.ConsumerHeartbeatNodeIdTextBox.Enabled = true;
         this.ConsumerHeartbeatTimeTextBox.Enabled = true;
         this.ProducerHeartbeatTimeTextBox.Enabled = true;
         this.UutIdTextBox.Enabled = true;
         this.EncoderIdTextBox.Enabled = true;
         this.AnalogIoIdTextBox.Enabled = true;
         this.DigitalIoIdTextBox.Enabled = true;

         this.UutRpmToSpeedTextBox.Enabled = true;
         this.BodyRpmToSpeedTextBox.Enabled = true;
         this.AnalogIoVoltsToSupplyAmpsSlopeTextBox.Enabled = true;
         this.AnalogIoVoltsToSupplyAmpsOffsetTextBox.Enabled = true;
         this.AnalogIoVoltsToLoadPoundsTextBox.Enabled = true;

         this.LoadParametersButton.Enabled = true;
         this.SaveParametersButton.Enabled = true;
      }

      private void LockForRun()
      {
         this.ActivityButton.Text = "Stop";

         this.RunTimeTextBox.Enabled = false;
         this.WheelSpeedTextBox.Enabled = false;
         this.WheelStartLoadTextBox.Enabled = false;
         this.WheelStopLoadTextBox.Enabled = false;
         this.CurrentLimitTextBox.Enabled = false;
         this.ThermalLimitTextBox.Enabled = false;
         this.SlippageLimitTextBox.Enabled = false;

         this.BaudComboBox.Enabled = false;
         this.BusInterfaceComboBox.Enabled = false;
         this.ConsumerHeartbeatNodeIdTextBox.Enabled = false;
         this.ConsumerHeartbeatTimeTextBox.Enabled = false;
         this.ProducerHeartbeatTimeTextBox.Enabled = false;
         this.UutIdTextBox.Enabled = false;
         this.EncoderIdTextBox.Enabled = false;
         this.AnalogIoIdTextBox.Enabled = false;
         this.DigitalIoIdTextBox.Enabled = false;

         this.UutRpmToSpeedTextBox.Enabled = false;
         this.BodyRpmToSpeedTextBox.Enabled = false;
         this.AnalogIoVoltsToSupplyAmpsSlopeTextBox.Enabled = false;
         this.AnalogIoVoltsToSupplyAmpsOffsetTextBox.Enabled = false;
         this.AnalogIoVoltsToLoadPoundsTextBox.Enabled = false;

         this.LoadParametersButton.Enabled = false;
         this.SaveParametersButton.Enabled = false;
      }

      private string ExtractBusParameters(SetupParameters setupParameters)
      {
         string result = null;

         #region Bus Interface Parsing

         setupParameters.BusInterface = (BusInterfaces)this.BusInterfaceComboBox.SelectedItem;

         #endregion

         #region Bus Rate Parsing

         string wheelSpeedString = this.BaudComboBox.Text;

         if (null != wheelSpeedString)
         {
            int interfaceRate = 0;

            if (int.TryParse(wheelSpeedString, out interfaceRate) != false)
            {
               setupParameters.BitRate = interfaceRate;
            }
            else
            {
               result = "invalid entry";
               this.BaudComboBox.BackColor = Color.Red;
            }
         }

         #endregion

         #region Consumer Heartbeat Node ID Parsing

         string consumerHeartbeatNodeIdString = this.ConsumerHeartbeatNodeIdTextBox.Text;

         if (null != consumerHeartbeatNodeIdString)
         {
            int consumerHeartbeatNodeId = 0;

            if (int.TryParse(consumerHeartbeatNodeIdString, out consumerHeartbeatNodeId) != false)
            {
               setupParameters.ConsumerHeartbeatNodeId = consumerHeartbeatNodeId;
            }
            else
            {
               result = "invalid entry";
               this.ConsumerHeartbeatNodeIdTextBox.BackColor = Color.Red;
            }
         }

         #endregion

         #region Consumer Heartbeat Time Parsing

         string consumerHeartbeatTimeString = this.ConsumerHeartbeatTimeTextBox.Text;

         if (null != consumerHeartbeatNodeIdString)
         {
            int consumerHeartbeatTime = 0;

            if (int.TryParse(consumerHeartbeatTimeString, out consumerHeartbeatTime) != false)
            {
               setupParameters.ConsumerHeartbeatTime = consumerHeartbeatTime;
            }
            else
            {
               result = "invalid entry";
               this.ConsumerHeartbeatTimeTextBox.BackColor = Color.Red;
            }
         }

         #endregion

         #region Consumer Heartbeat Time Parsing

         string producerHeartbeatTimeString = this.ProducerHeartbeatTimeTextBox.Text;

         if (null != producerHeartbeatTimeString)
         {
            int producerHeartbeatTime = 0;

            if (int.TryParse(producerHeartbeatTimeString, out producerHeartbeatTime) != false)
            {
               setupParameters.ProducerHeartbeatTime = producerHeartbeatTime;
            }
            else
            {
               result = "invalid entry";
               this.ProducerHeartbeatTimeTextBox.BackColor = Color.Red;
            }
         }

         #endregion

         #region UUT ID Parsing

         string uutIdString = this.UutIdTextBox.Text;

         if (null != uutIdString)
         {
            int uutId = 0;

            if (int.TryParse(uutIdString, out uutId) != false)
            {
               setupParameters.UutId = uutId;
            }
            else
            {
               result = "invalid entry";
               this.UutIdTextBox.BackColor = Color.Red;
            }
         }

         #endregion

         #region Encoder ID Parsing

         string encoderIdString = this.EncoderIdTextBox.Text;

         if (null != encoderIdString)
         {
            int encoderId = 0;

            if (int.TryParse(encoderIdString, out encoderId) != false)
            {
               setupParameters.EncoderId = encoderId;
            }
            else
            {
               result = "invalid entry";
               this.EncoderIdTextBox.BackColor = Color.Red;
            }
         }

         #endregion

         #region Analog IO ID Parsing

         string analogIoIdString = this.AnalogIoIdTextBox.Text;

         if (null != analogIoIdString)
         {
            int analogIoId = 0;

            if (int.TryParse(analogIoIdString, out analogIoId) != false)
            {
               setupParameters.AnalogIoId = analogIoId;
            }
            else
            {
               result = "invalid entry";
               this.AnalogIoIdTextBox.BackColor = Color.Red;
            }
         }

         #endregion

         #region Digital IO ID Parsing

         string digitalIoIdString = this.DigitalIoIdTextBox.Text;

         if (null != digitalIoIdString)
         {
            int digitalIoId = 0;

            if (int.TryParse(digitalIoIdString, out digitalIoId) != false)
            {
               setupParameters.DigialIoId = digitalIoId;
            }
            else
            {
               result = "invalid entry";
               this.DigitalIoIdTextBox.BackColor = Color.Red;
            }
         }

         #endregion

         #region UUT RPM to Speed Parsing

         string uutRpmToSpeedString = this.UutRpmToSpeedTextBox.Text;

         if (null != uutRpmToSpeedString)
         {
            double uutRpmToSpeed = 0.0;

            if ((double.TryParse(uutRpmToSpeedString, out uutRpmToSpeed) != false) &&
                (0 != uutRpmToSpeed))
            {
               setupParameters.UutRpmToSpeed = uutRpmToSpeed;
            }
            else
            {
               result = "invalid entry";
               this.UutRpmToSpeedTextBox.BackColor = Color.Red;
            }
         }

         #endregion

         #region Body RPM to Speed Parsing

         string bodyRpmToSpeedString = this.BodyRpmToSpeedTextBox.Text;

         if (null != bodyRpmToSpeedString)
         {
            double bodyRpmToSpeed = 0.0;

            if ((double.TryParse(bodyRpmToSpeedString, out bodyRpmToSpeed) != false) &&
                (0 != bodyRpmToSpeed))
            {
               setupParameters.BodyRpmToSpeed = bodyRpmToSpeed;
            }
            else
            {
               result = "invalid entry";
               this.BodyRpmToSpeedTextBox.BackColor = Color.Red;
            }
         }

         #endregion

         #region Analog IO Volts to Supply Amps Parsing

         string analogIoVoltsToSupplyAmpsSlopeString = this.AnalogIoVoltsToSupplyAmpsSlopeTextBox.Text;

         if (null != analogIoVoltsToSupplyAmpsSlopeString)
         {
            double analogIoVoltsToSupplyAmpsSlope = 0.0;

            if ((double.TryParse(analogIoVoltsToSupplyAmpsSlopeString, out analogIoVoltsToSupplyAmpsSlope) != false) &&
                (0 != analogIoVoltsToSupplyAmpsSlope))
            {
               setupParameters.AnalogIoVoltsToSupplyAmpsSlope = analogIoVoltsToSupplyAmpsSlope;
            }
            else
            {
               result = "invalid entry";
               this.AnalogIoVoltsToSupplyAmpsSlopeTextBox.BackColor = Color.Red;
            }
         }

         #endregion

         #region Analog IO Volts to Supply Amps Offset Parsing

         string analogIoVoltsToSupplyAmpsOffsetString = this.AnalogIoVoltsToSupplyAmpsOffsetTextBox.Text;

         if (null != analogIoVoltsToSupplyAmpsOffsetString)
         {
            double analogIoVoltsToSupplyAmpsOffset = 0.0;

            if (double.TryParse(analogIoVoltsToSupplyAmpsOffsetString, out analogIoVoltsToSupplyAmpsOffset) != false)
            {
               setupParameters.AnalogIoVoltsToSupplyAmpsOffset = analogIoVoltsToSupplyAmpsOffset;
            }
            else
            {
               result = "invalid entry";
               this.AnalogIoVoltsToSupplyAmpsOffsetTextBox.BackColor = Color.Red;
            }
         }

         #endregion

         #region Analog IO Volts to Load Pounds Parsing

         string analogIoVoltsToLoadPoundsString = this.AnalogIoVoltsToLoadPoundsTextBox.Text;

         if (null != analogIoVoltsToLoadPoundsString)
         {
            double analogIoVoltsToLoadPounds = 0.0;

            if ((double.TryParse(analogIoVoltsToLoadPoundsString, out analogIoVoltsToLoadPounds) != false) &&
                (0 != analogIoVoltsToLoadPounds))
            {
               setupParameters.AnalogIoVoltsToLoadPounds = analogIoVoltsToLoadPounds;
            }
            else
            {
               result = "invalid entry";
               this.AnalogIoVoltsToLoadPoundsTextBox.BackColor = Color.Red;
            }
         }

         #endregion

         return (result);
      }

      private void PopulateTestParameters(TestParameters testParameters)
      {
         if (double.IsNaN(testParameters.RunTime) == false)
         {
            double totalSeconds = testParameters.RunTime;
            int hours = (int)(testParameters.RunTime / 3600.0);
            totalSeconds -= (hours * 3600.0);
            int minutes = (int)(totalSeconds / 60.0);
            totalSeconds -= (minutes * 60.0);
            double seconds = totalSeconds;

            this.RunTimeTextBox.Text = string.Format("{0}/{1}/{2}", hours, minutes, seconds);
            this.RunTimeTextBox.BackColor = Color.FromKnownColor(KnownColor.Window);
         }
         else
         {
            this.RunTimeTextBox.Text = "";
            this.RunTimeTextBox.BackColor = Color.Red;
         }

         if (double.IsNaN(testParameters.WheelSpeed) == false)
         {
            this.WheelSpeedTextBox.Text = testParameters.WheelSpeed.ToString();
            this.WheelSpeedTextBox.BackColor = Color.FromKnownColor(KnownColor.Window);
         }
         else
         {
            this.WheelSpeedTextBox.Text = "";
            this.WheelSpeedTextBox.BackColor = Color.Red;
         }

         if (double.IsNaN(testParameters.WheelStartLoad) == false)
         {
            this.WheelStartLoadTextBox.Text = testParameters.WheelStartLoad.ToString();
            this.WheelStartLoadTextBox.BackColor = Color.FromKnownColor(KnownColor.Window);
         }
         else
         {
            this.WheelStartLoadTextBox.Text = "";
            this.WheelStartLoadTextBox.BackColor = Color.Red;
         }

         if (double.IsNaN(testParameters.WheelStopLoad) == false)
         {
            this.WheelStopLoadTextBox.Text = testParameters.WheelStopLoad.ToString();
            this.WheelStopLoadTextBox.BackColor = Color.FromKnownColor(KnownColor.Window);
         }
         else
         {
            this.WheelStopLoadTextBox.Text = "";
            this.WheelStopLoadTextBox.BackColor = Color.Red;
         }

         if (double.IsNaN(testParameters.CurrentLimit) == false)
         {
            this.CurrentLimitTextBox.Text = testParameters.CurrentLimit.ToString();
            this.CurrentLimitTextBox.BackColor = Color.FromKnownColor(KnownColor.Window);
         }
         else
         {
            this.CurrentLimitTextBox.Text = "";
            this.CurrentLimitTextBox.BackColor = Color.Red;
         }

         if (double.IsNaN(testParameters.ThermalLimit) == false)
         {
            this.ThermalLimitTextBox.Text = testParameters.ThermalLimit.ToString();
            this.ThermalLimitTextBox.BackColor = Color.FromKnownColor(KnownColor.Window);
         }
         else
         {
            this.ThermalLimitTextBox.Text = "";
            this.ThermalLimitTextBox.BackColor = Color.Red;
         }

         if (double.IsNaN(testParameters.SlippageLimit) == false)
         {
            this.SlippageLimitTextBox.Text = testParameters.SlippageLimit.ToString();
            this.SlippageLimitTextBox.BackColor = Color.FromKnownColor(KnownColor.Window);
         }
         else
         {
            this.SlippageLimitTextBox.Text = "";
            this.SlippageLimitTextBox.BackColor = Color.Red;
         }
      }

      private string ExtractTestParameters(TestParameters testParameters)
      {
         string result = null;

         #region Runtime Entry Parsing

         string runtimeString = this.RunTimeTextBox.Text;

         if (null != runtimeString)
         {
            string[] timeValues = runtimeString.Split(new char[] { '/' });

            if ((null != timeValues) && (3 == timeValues.Length))
            {
               bool validTime = true;
               double hours = 0;
               double minutes = 0;
               double seconds = 0;
               double totalTime = 0;

               if (double.TryParse(timeValues[0], out hours) == false)
               {
                  validTime = false;
               }

               if (double.TryParse(timeValues[1], out minutes) == false)
               {
                  validTime = false;
               }

               if (double.TryParse(timeValues[2], out seconds) == false)
               {
                  validTime = false;
               }

               if (false != validTime)
               {
                  totalTime = (hours * 3600.0) + (minutes * 60.0) + seconds;

                  if (0 == totalTime)
                  {
                     validTime = false;
                  }
               }

               if (false != validTime)
               {
                  testParameters.RunTime = totalTime;
               }
               else
               {
                  result = "invalid entry";
                  this.RunTimeTextBox.BackColor = Color.Red;
               }
            }
            else
            {
               result = "invalid entry";
               this.RunTimeTextBox.BackColor = Color.Red;
            }
         }

         #endregion

         #region Wheel Speed Entry Parsing

         string wheelSpeedString = this.WheelSpeedTextBox.Text;

         if (null != wheelSpeedString)
         {
            double wheelSpeed = 0;

            if (double.TryParse(wheelSpeedString, out wheelSpeed) != false)
            {
               if ((wheelSpeed >= -10) && (wheelSpeed <= 10))
               {
                  testParameters.WheelSpeed = wheelSpeed;
               }
               else
               {
                  result = "invalid entry, limited to {-10.0 .. +10.0}";
                  this.WheelSpeedTextBox.BackColor = Color.Red;
               }
            }
            else
            {
               result = "invalid entry";
               this.WheelSpeedTextBox.BackColor = Color.Red;
            }
         }

         #endregion

         #region Wheel Start Load Entry Parsing

         string wheelStartLoadString = this.WheelStartLoadTextBox.Text;

         if (null != wheelStartLoadString)
         {
            double wheelStartLoad = 0;

            if (double.TryParse(wheelStartLoadString, out wheelStartLoad) != false)
            {
               testParameters.WheelStartLoad= wheelStartLoad;
            }
            else
            {
               result = "invalid entry";
               this.WheelStartLoadTextBox.BackColor = Color.Red;
            }
         }

         #endregion

         #region Wheel Stop Load Entry Parsing

         string wheelStopLoadString = this.WheelStopLoadTextBox.Text;

         if (null != wheelStopLoadString)
         {
            double wheelStopLoad = 0;

            if (double.TryParse(wheelStopLoadString, out wheelStopLoad) != false)
            {
               testParameters.WheelStopLoad = wheelStopLoad;
            }
            else
            {
               result = "invalid entry";
               this.WheelStopLoadTextBox.BackColor = Color.Red;
            }
         }

         #endregion

         #region Current Limit Entry Parsing

         string currentLimitString = this.CurrentLimitTextBox.Text;

         if (null != currentLimitString)
         {
            double currentLimit = 0;

            if (double.TryParse(currentLimitString, out currentLimit) != false)
            {
               testParameters.CurrentLimit= currentLimit;
            }
            else
            {
               result = "invalid entry";
               this.CurrentLimitTextBox.BackColor = Color.Red;
            }
         }

         #endregion

         #region Thermal Limit Entry Parsing

         string thermalLimitString = this.ThermalLimitTextBox.Text;

         if (null != thermalLimitString)
         {
            double thermalLimit = 0;

            if (double.TryParse(thermalLimitString, out thermalLimit) != false)
            {
               testParameters.ThermalLimit = thermalLimit;
            }
            else
            {
               result = "invalid entry";
               this.ThermalLimitTextBox.BackColor = Color.Red;
            }
         }

         #endregion

         #region Slippage Limit Entry Parsing

         string slippageLimitString = this.SlippageLimitTextBox.Text;

         if (null != slippageLimitString)
         {
            double slippageLimit = 0;

            if (double.TryParse(slippageLimitString, out slippageLimit) != false)
            {
               testParameters.SlippageLimit = slippageLimit;
            }
            else
            {
               result = "invalid entry";
               this.SlippageLimitTextBox.BackColor = Color.Red;
            }
         }

         #endregion

         return (result);
      }

      private void UpdateTraceLevel(TraceGroup group, DYNO.Utilities.TraceLevel level)
      {
         if (false == this.settingTraceMask)
         {
            Tracer.SetGroupLevel(group, level);

            this.settingTraceMask = true;
            this.TraceMaskTextBox.Text = Tracer.MaskString;
            this.settingTraceMask = false;
         }
      }

      #endregion

      #region User Events

      private void ActivityButton_Click(object sender, EventArgs e)
      {
         if (false == this.active)
         {
            string result = null;
            SetupParameters busParameters = new SetupParameters();
            TestParameters testParameters = new TestParameters();

            if (null == result)
            {
               result = this.ExtractBusParameters(busParameters);

               if (null != result)
               {
                  this.MainTabControl.SelectedTab = this.SetupTabPage;
               }
            }

            if (null == result)
            {
               result = this.ExtractTestParameters(testParameters);

               if (null != result)
               {
                  this.MainTabControl.SelectedTab = this.TestTabPage;
               }
            }

            if (null == result)
            {
               this.testComplete = false;
               this.dynoTest.Start(busParameters, testParameters, ref result);
            }

            if (null == result)
            {
               this.StatusLabel.Text = "test started";
   
               this.active = true;
               this.LockForRun();
            }
            else
            {
               this.StatusLabel.Text = result;
            }
         }
         else
         {
            this.dynoTest.Stop();
            this.StatusLabel.Text = "test stopped";

            this.active = false;
            this.UnlockForIdle();
         }
      }

      private void ClearActivityLogButton_Click(object sender, EventArgs e)
      {
         this.ActivityRichTextBox.Clear();
      }

      private void LoadParametersButton_Click(object sender, EventArgs e)
      {
         OpenFileDialog openFileDialog = new OpenFileDialog();
         openFileDialog.RestoreDirectory = true;
         openFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
         openFileDialog.FilterIndex = 0;
         openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
         openFileDialog.FileName = "testParameters.xml";

         if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
         {
            TestParameters testParameters = new TestParameters();
            string result = testParameters.Load(openFileDialog.FileName);

            if (null == result)
            {
               this.PopulateTestParameters(testParameters);
               this.StatusLabel.Text = "parameters loaded";
            }
            else
            {
               this.StatusLabel.Text = result;
            }
         }
      }

      private void SaveParametersButton_Click(object sender, EventArgs e)
      {
         TestParameters testParameters = new TestParameters();

         string extractResult = this.ExtractTestParameters(testParameters);

         if (null == extractResult)
         {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            saveFileDialog.FileName = "testParameters.xml";

            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
               string saveResult = testParameters.Save(saveFileDialog.FileName);

               if (null == saveResult)
               {
                  this.StatusLabel.Text = "parameters saved";
               }
               else
               {
                  this.StatusLabel.Text = saveResult;
               }
            }
         }
         else
         {
            this.StatusLabel.Text = extractResult;
         }
      }

      private void CloseButton_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      #endregion

      #region Form Events

      private void MainForm_Shown(object sender, EventArgs e)
      {
         this.LoadRegistry();
         this.UpdateTimer.Enabled = true;
      }

      private void ParsedEntryControl_Enter(object sender, EventArgs e)
      {
         Control control = (Control)sender;

         if (Color.Red == control.BackColor)
         {
            control.BackColor = Color.FromKnownColor(KnownColor.Window);
         }
      }

      private void TraceMaskTextBox_TextChanged(object sender, EventArgs e)
      {
         if (false == this.settingTraceMask)
         {
            this.settingTraceMask = true;

            Tracer.MaskString = this.TraceMaskTextBox.Text;

            this.CanTraceComboBox.SelectedIndex = (int)Tracer.GetGroupLevel(TraceGroup.CANBUS);
            this.TestTraceComboBox.SelectedIndex = (int)Tracer.GetGroupLevel(TraceGroup.TEST);
            this.LogTraceComboBox.SelectedIndex = (int)Tracer.GetGroupLevel(TraceGroup.TEST);
            this.DeviceTraceComboBox.SelectedIndex = (int)Tracer.GetGroupLevel(TraceGroup.DEVICE);

            this.settingTraceMask = false;
         }
      }

      private void CanTraceComboBox_SelectedIndexChanged(object sender, EventArgs e)
      {
         this.UpdateTraceLevel(TraceGroup.CANBUS, (DYNO.Utilities.TraceLevel)this.CanTraceComboBox.SelectedIndex);
      }

      private void TestTraceComboBox_SelectedIndexChanged(object sender, EventArgs e)
      {
         this.UpdateTraceLevel(TraceGroup.TEST, (DYNO.Utilities.TraceLevel)this.TestTraceComboBox.SelectedIndex);
      }

      private void LogTraceComboBox_SelectedIndexChanged(object sender, EventArgs e)
      {
         this.UpdateTraceLevel(TraceGroup.LOG, (DYNO.Utilities.TraceLevel)this.LogTraceComboBox.SelectedIndex);
      }

      private void DeviceTraceComboBox_SelectedIndexChanged(object sender, EventArgs e)
      {
         this.UpdateTraceLevel(TraceGroup.DEVICE, (DYNO.Utilities.TraceLevel)this.DeviceTraceComboBox.SelectedIndex);
      }

      private void UpdateTimer_Tick(object sender, EventArgs e)
      {
         #region Clock Update

         DateTime now = DateTime.Now;
         string timeText = string.Format("{0:D2}:{1:D2}:{2:D2}.{3}", now.Hour, now.Minute, now.Second, (now.Millisecond / 100));
         this.TimeStatusLabel.Text = timeText;

         #endregion

         #region Test Monitor

         if ((false != this.active) && (false != this.testComplete))
         {
            this.StatusLabel.Text = "test completed";
            this.active = false;
            this.UnlockForIdle();
         }

         #endregion

         #region Trace Activity Update

         if ((null != this.titleString) && (this.titleString != this.ActivityTitleRichTextBox.Text))
         {
            this.ActivityTitleRichTextBox.Text = titleString;
         }

         bool inserted = false;

         while (0 != this.traceQueue.Count)
         {
            string activityString = (string)this.traceQueue.Dequeue();
            this.ActivityRichTextBox.AppendText(activityString + "\n");
            inserted = true;
         }

         if ((false != inserted) && (false != this.AutoScrollCheckBox.Checked))
         {
            this.ActivityRichTextBox.ScrollToCaret();
         }

         #endregion
      }

      private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
      {
         if (false != this.active)
         {
            DialogResult result = MessageBox.Show(this, "Session currently active.\n\nStop and close?", "dynoGUI", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            if (System.Windows.Forms.DialogResult.No == result)
            {
               e.Cancel = true;
            }
         }
      }

      private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
      {
         this.SaveRegistry();
      }

      #endregion

      #region Constructor

      public MainForm()
      {
         this.InitializeComponent();

         this.active = false;

         FileTraceListener fileTraceListener;
         fileTraceListener = new FileTraceListener();
         fileTraceListener.LogFilePath = @"C:\LOGS\DYNO";
         fileTraceListener.Prefix = "DYNO_";
         fileTraceListener.MaximumLines = 10000;
         Trace.Listeners.Add(fileTraceListener);

         this.traceQueue = new Queue();
         QueuedTraceListener queuedTraceListener = new QueuedTraceListener(this.traceQueue);
         Trace.Listeners.Add(queuedTraceListener);
         settingTraceMask = false;

         this.dynoTest = new DynoTest();
         this.dynoTest.OnSetTitle = new DynoTest.SetTitleHandler(this.OnSetTitle);
         this.dynoTest.OnComplete = new DynoTest.CompleteHandler(this.OnTestComplete);         

         Array interfaces = Enum.GetValues(typeof(BusInterfaces));

         this.BusInterfaceComboBox.Items.Clear();
         for (IEnumerator i = interfaces.GetEnumerator(); i.MoveNext(); )
         {
            this.BusInterfaceComboBox.Items.Add(i.Current);
         }
         this.BusInterfaceComboBox.SelectedIndex = 0;

         this.BaudComboBox.Items.Clear();
         this.BaudComboBox.Items.Add("10000");
         this.BaudComboBox.Items.Add("20000");
         this.BaudComboBox.Items.Add("50000");
         this.BaudComboBox.Items.Add("100000");
         this.BaudComboBox.Items.Add("125000");
         this.BaudComboBox.Items.Add("250000");
         this.BaudComboBox.Items.Add("500000");
         this.BaudComboBox.Items.Add("1000000");
         this.BaudComboBox.SelectedIndex = 2;

         this.StatusLabel.Text = "";
         this.UnlockForIdle();
      }

      #endregion

   }
}
