
namespace NICBOT.DeviceTest
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.ComponentModel;
   using System.Data;
   using System.Diagnostics;
   using System.Drawing;
   using System.Linq;
   using System.Text;
   using System.Threading.Tasks;
   using System.Windows.Forms;

   using Microsoft.Win32;

   using NICBOT.CAN;
   using NICBOT.PCANLight;
   using NICBOT.Utilities;

   public partial class MainForm : Form
   {
      #region Definition

      private const string RegistryCompanyName = "ULC Robotics";
      private const string RegistryApplicationName = "NICBOT Encoder Interface";

      #endregion
      
      #region Fields

      private bool active;
      private Queue traceQueue;
      
      private BusInterface busInterface;
      private Device device;
      private PeakDigitalIo digitalIo;
      private PeakAnalogIo analogIo;
      private KublerRotaryEncoder encoder;
      private ElmoWhistleMotor motor;
      private UlcRoboticsCamera camera;
      private UlcRoboticsGps gps;
      private UlcRoboticsNicbotBody nicbotBody;
      private UlcRoboticsNicbotWheel nicbotWheel;
      private UlcRoboticsRs232 rs232;

      private byte heartbeatNodeId;
      private bool heartbeatActive;
      private int heartbeatTime;
      private DateTime heartbeatTimeLimit;

      private byte digitalIoInputs;
      private byte motorInputs;

      private DateTime cameraAutoSendTimeLimit;

      private bool nicbotBodyAssigningSolenoidValues;
      
      private Queue rs232RxQueue;
      
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
         defaultValue = (SystemInformation.PrimaryMonitorSize.Height - this.Height) / 2;
         this.Top = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : defaultValue) : defaultValue;

         keyValue = appKey.GetValue("Left");
         defaultValue = (SystemInformation.PrimaryMonitorSize.Width - this.Width) / 2;
         this.Left = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : defaultValue) : defaultValue;

         keyValue = appKey.GetValue("WindowState");
         this.WindowState = ((null != keyValue) && Enum.IsDefined(this.WindowState.GetType(), keyValue)) ? (FormWindowState)Enum.Parse(this.WindowState.GetType(), keyValue.ToString()) : FormWindowState.Normal;

         #endregion

         #region Session Information

         keyValue = appKey.GetValue("MainControlHeight");
         int defaultMainControlHeight = this.MainTabControl.Height;
         this.MainTabControl.Height = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : defaultMainControlHeight) : defaultMainControlHeight;
         
         keyValue = appKey.GetValue("BusInterface");
         this.BusInterfaceComboBox.SelectedIndex = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 0) : 0;

         keyValue = appKey.GetValue("BaudRate");
         this.BaudComboBox.SelectedIndex = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 6) : 6;

         keyValue = appKey.GetValue("ActiveNodeId");
         this.ActiveDeviceNodeIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue("HeartbeatNodeId");
         this.HeartbeatNodeIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "80";

         keyValue = appKey.GetValue("HeartbeatTime");
         this.HeartbeatTimeTextBox.Text = (null != keyValue) ? keyValue.ToString() : "1000";

         keyValue = appKey.GetValue("DigitalIoActiveNodeId");
         this.DigitalIoActiveNodeIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";
         
         keyValue = appKey.GetValue("AnalogIoActiveNodeId");
         this.AnalogIoActiveNodeIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue("EncoderActiveNodeId");
         this.ActiveEncoderNodeIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue("MotorActiveNodeId");
         this.ActiveMotorNodeIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue("GpsActiveNodeId");
         this.ActiveGpsNodeIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue("NicbotBodyActiveNodeId");
         this.NicbotBodyActiveNodeIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue("NicbotWheelActiveNodeId");
         this.NicbotWheelActiveNodeIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue("Rs232ActiveNodeId");
         this.ActiveRs232NodeIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";
         
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

         appKey.SetValue("BusInterface", this.BusInterfaceComboBox.SelectedIndex);
         appKey.SetValue("BaudRate", this.BaudComboBox.SelectedIndex);
         appKey.SetValue("ActiveNodeId", this.ActiveDeviceNodeIdTextBox.Text);
         appKey.SetValue("HeartbeatNodeId", this.HeartbeatNodeIdTextBox.Text);
         appKey.SetValue("HeartbeatTime", this.HeartbeatTimeTextBox.Text);

         appKey.SetValue("DigitalIoActiveNodeId", this.DigitalIoActiveNodeIdTextBox.Text);
         appKey.SetValue("AnalogIoActiveNodeId", this.AnalogIoActiveNodeIdTextBox.Text);
         appKey.SetValue("EncoderActiveNodeId", this.ActiveEncoderNodeIdTextBox.Text);
         appKey.SetValue("MotorActiveNodeId", this.ActiveMotorNodeIdTextBox.Text);
         appKey.SetValue("GpsActiveNodeId", this.ActiveGpsNodeIdTextBox.Text);
         appKey.SetValue("NicbotBodyActiveNodeId", this.NicbotBodyActiveNodeIdTextBox.Text);
         appKey.SetValue("NicbotWheelActiveNodeId", this.NicbotWheelActiveNodeIdTextBox.Text);
         appKey.SetValue("Rs232ActiveNodeId", this.ActiveRs232NodeIdTextBox.Text);

         #endregion
      }

      #endregion

      #region Helper Functions

      private string GetDataString(byte[] data)
      {
         StringBuilder sb = new StringBuilder();

         for (int i = 0; i < data.Length; i++)
         {
            sb.AppendFormat("{0:X2}", data[i]);
         }

         return (sb.ToString());
      }

      private byte[] GetDataArray(string text)
      {
         byte[] result = new byte[(text.Length+1) / 2];
         int count = 0;

         for (int i = 0; i < text.Length; i+=2)
         {
            char a = (i < text.Length) ? text[i] : ' ';
            char b = ((i+1) < text.Length) ? text[(i+1)] : ' ';
            
            StringBuilder sb = new StringBuilder();
            sb.Append(a);
            sb.Append(b);

            byte value = 0;
            byte.TryParse(sb.ToString(), System.Globalization.NumberStyles.HexNumber, null, out value);
            result[count++] = value;
         }

         return (result);
      }

      private void UpdateNicbotSolenoidSelect(int solenoidId, bool active)
      {
         if (false == this.nicbotBodyAssigningSolenoidValues)
         {
            byte nicbotNodeId = 0;
            UInt16 solenoidMask = (UInt16)(1 << solenoidId);
            UInt16 solenoidCache = this.nicbotBody.GetSolenoidCache();

            if (false != active)
            {
               solenoidCache |= solenoidMask;
            }
            else
            {
               solenoidCache &= (UInt16)(~solenoidMask);
            }

            if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
            {
               this.nicbotBody.NodeId = nicbotNodeId;
               bool result = this.nicbotBody.SetSolenoids(solenoidCache);

               if (false != result)
               {
                  this.StatusLabel.Text = "NICBOT solenoid set.";
               }
               else
               {
                  this.StatusLabel.Text = "Unable to set NICBOT solenoid.";
               }
            }
            else
            {
               this.StatusLabel.Text = "Invalid entry.";
            }
         }
      }

      #endregion

      #region Delegate Functions

      private void DigitalIoChangeHandler(int nodeId, byte value)
      {
         this.digitalIoInputs = value;
      }

      private void MotorInputChangeHandler(int nodeId, byte value)
      {
         this.motorInputs = value;
      }

      private void Rs232ReceiveHandler(byte[] buffer, int length)
      {
         for (int i = 0; i < length; i++)
         {
            this.rs232RxQueue.Enqueue(buffer[i]);
         }
      }

      #endregion

      #region Control Device Events
      
      private void ActivityButton_Click(object sender, EventArgs e)
      {
         if (false == this.active)
         {
            string result = null;
            int interfaceRate = 0;

            BusInterfaces interfaceId = (BusInterfaces)this.BusInterfaceComboBox.SelectedItem;

            if (int.TryParse(this.BaudComboBox.Text, out interfaceRate) != false)
            {
               this.busInterface.Start(interfaceId, interfaceRate, ref result);

               if (null == result)
               {
                  this.StatusLabel.Text = "Bus interface started.";

                  this.active = true;
                  this.ActivityButton.Text = "Stop";
                  this.BaudComboBox.Enabled = false;
                  this.BusInterfaceComboBox.Enabled = false;
               }
               else
               {
                  this.StatusLabel.Text = result;
               }
            }
            else
            {
               this.StatusLabel.Text = "Invalid rate.";
            }
         }
         else
         {
            this.busInterface.Stop();
            this.StatusLabel.Text = "Bus interface stopped.";

            this.active = false;
            this.ActivityButton.Text = "Start";
            this.BaudComboBox.Enabled = true;
            this.BusInterfaceComboBox.Enabled = true;
         }
      }

      private void ClearButton_Click(object sender, EventArgs e)
      {
         this.ActivityRichTextBox.Clear();
      }

      private void HeartbeatActivityButton_Click(object sender, EventArgs e)
      {
         if (false == this.heartbeatActive)
         {
            int time = 0;
            byte nodeId = 0;

            if ((byte.TryParse(this.HeartbeatNodeIdTextBox.Text, out nodeId) != false) &&
                (int.TryParse(this.HeartbeatTimeTextBox.Text, out time) != false))
            {
               this.heartbeatActive = true;
               this.heartbeatNodeId = nodeId;
               this.heartbeatTime = time;
               this.heartbeatTimeLimit = DateTime.Now.AddMilliseconds(this.heartbeatTime);
               this.HeartbeatActivityButton.Text = "Stop";
            }
            else
            {
               this.StatusLabel.Text = "invalid entry";
            }
         }
         else
         {
            this.heartbeatActive = false;
            this.HeartbeatActivityButton.Text = "Start";
         }
      }

      private void DeviceStartButton_Click(object sender, EventArgs e)
      {
         byte deviceNodeId = 0;

         if (byte.TryParse(this.ActiveDeviceNodeIdTextBox.Text, out deviceNodeId) != false)
         {
            this.device.NodeId = deviceNodeId;
            this.device.Start();
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void DeviceStopButton_Click(object sender, EventArgs e)
      {
         byte deviceNodeId = 0;

         if (byte.TryParse(this.ActiveDeviceNodeIdTextBox.Text, out deviceNodeId) != false)
         {
            this.device.NodeId = deviceNodeId;
            this.device.Stop();
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void DeviceResetButton_Click(object sender, EventArgs e)
      {
         byte deviceNodeId = 0;

         if (byte.TryParse(this.ActiveDeviceNodeIdTextBox.Text, out deviceNodeId) != false)
         {
            this.device.NodeId = deviceNodeId;
            this.device.Reset();
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void DeviceUploadButton_Click(object sender, EventArgs e)
      {
         UInt16 index = 0;
         byte subIndex = 0;
         byte activeNodeId = 0;

         if ((byte.TryParse(this.ActiveDeviceNodeIdTextBox.Text, out activeNodeId) != false) &&
             (UInt16.TryParse(this.DeviceUploadIndexTextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out index) != false) &&
             (byte.TryParse(this.DeviceUploadSubIndexTextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out subIndex) != false))
         {
            this.device.NodeId = activeNodeId;

            byte[] result = this.device.SDOUpLoad(index, subIndex);

            if (null != result)
            {
               this.StatusLabel.Text = "Upload complete.";
               this.DeviceUploadHexDataTextBox.Text = this.GetDataString(result);
               this.DeviceUploadStringDataTextBox.Text = Encoding.UTF8.GetString(result);
            }
            else
            {
               this.StatusLabel.Text = "Unable to upload data.";
               this.DeviceUploadHexDataTextBox.Text = "";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void DeviceDownloadHexButton_Click(object sender, EventArgs e)
      {
         byte activeNodeId = 0;
         UInt16 index = 0;
         byte subIndex = 0;
         byte[] data = this.GetDataArray(this.DeviceDownloadHexDataTextBox.Text);

         if ((byte.TryParse(this.ActiveDeviceNodeIdTextBox.Text, out activeNodeId) != false) &&
             (UInt16.TryParse(this.DeviceDownloadIndexTextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out index) != false) &&
             (byte.TryParse(this.DeviceDownloadSubIndexTextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out subIndex) != false) &&
             (null != data))
         {
            this.device.NodeId = activeNodeId;
            bool result = this.device.SDODownload(index, subIndex, data);

            if (false != result)
            {
               this.StatusLabel.Text = "Download complete.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to download data.";
               this.DeviceUploadHexDataTextBox.Text = "";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void DeviceDownloadStringButton_Click(object sender, EventArgs e)
      {
         byte activeNodeId = 0;
         UInt16 index = 0;
         byte subIndex = 0;
         byte[] data = Encoding.UTF8.GetBytes(this.DeviceDownloadStringDataTextBox.Text);

         if ((byte.TryParse(this.ActiveDeviceNodeIdTextBox.Text, out activeNodeId) != false) &&
             (UInt16.TryParse(this.DeviceDownloadIndexTextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out index) != false) &&
             (byte.TryParse(this.DeviceDownloadSubIndexTextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out subIndex) != false) &&
             (null != data))
         {
            this.device.NodeId = activeNodeId;
            bool result = this.device.SDODownload(index, subIndex, data);

            if (false != result)
            {
               this.StatusLabel.Text = "Download complete.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to download data.";
               this.DeviceUploadHexDataTextBox.Text = "";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #region Digital IO Events

      private void DigitalIoConfigButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.DigitalIoActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.digitalIo.NodeId = nodeId;
            bool result = this.digitalIo.Configure();

            if (false != result)
            {
               this.StatusLabel.Text = "Digital IO configured.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to configure Digital IO .";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void DigitalIoStartButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.DigitalIoActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.digitalIo.NodeId = nodeId;
            bool result = this.digitalIo.Start();

            if (false != result)
            {
               this.StatusLabel.Text = "Digital IO started.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to start Digital IO .";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void DigitalIoStopButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.DigitalIoActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.digitalIo.NodeId = nodeId;
            this.digitalIo.Stop();
            this.StatusLabel.Text = "Digital IO stopped.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void DigitalIoResetButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.DigitalIoActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.digitalIo.NodeId = nodeId;
            this.digitalIo.Reset();
            this.StatusLabel.Text = "Digital IO reset.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetDigitalIoBaudRateButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         int deviceRate = 0;

         if ((byte.TryParse(this.DigitalIoActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (int.TryParse(this.DigitalIoBaudComboBox.Text, out deviceRate) != false))
         {
            this.digitalIo.NodeId = nodeId;
            bool result = this.digitalIo.SetDeviceBaudRate(deviceRate);

            if (false != result)
            {
               this.StatusLabel.Text = "Digital IO baudrate set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Digital IO bbaudrate.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetDigitalIoNodeIdButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         byte deviceNodeId = 0;

         if ((byte.TryParse(this.DigitalIoActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (byte.TryParse(this.DigitalIoNodeIdTextBox.Text, out deviceNodeId) != false))
         {
            this.digitalIo.NodeId = nodeId;
            bool result = this.digitalIo.SetDeviceNodeId(deviceNodeId);

            if (false != result)
            {
               this.StatusLabel.Text = "Digital IO node ID set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Digital IO node ID.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetDigitalIoOutputsButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         byte outputValue = 0;

         if ((byte.TryParse(this.DigitalIoActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (byte.TryParse(this.DigitalIoOutputTextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out outputValue) != false))
         {
            this.digitalIo.NodeId = nodeId;
            bool result = this.digitalIo.SetOutput(outputValue);

            if (false != result)
            {
               this.StatusLabel.Text = "Digital IO outputs set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Digital IO outputs.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetDigitalIoConsumerHeartbeatButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         byte consumerHeartbeatNodeId = 0;
         UInt16 consumerHeartbeatTime = 0;

         if ((byte.TryParse(this.DigitalIoActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (byte.TryParse(this.DigitalIoConsumerHeartbeatNodeIdTextBox.Text, out consumerHeartbeatNodeId) != false) &&
             (UInt16.TryParse(this.DigitalIoConsumerHeartbeatTimeTextBox.Text, out consumerHeartbeatTime) != false))
         {
            this.digitalIo.NodeId = nodeId;
            bool result = this.digitalIo.SetConsumerHeartbeat(consumerHeartbeatTime, consumerHeartbeatNodeId);

            if (false != result)
            {
               this.StatusLabel.Text = "Digital IO consumer heartbeat set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Digital IO consumer heartbeat.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetDigitalIoProducerHeartbeatButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt16 producerHeartbeatTime = 0;

         if ((byte.TryParse(this.DigitalIoActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt16.TryParse(this.DigitalIoProducerHeartbeatTimeTextBox.Text, out producerHeartbeatTime) != false))
         {
            this.digitalIo.NodeId = nodeId;
            bool result = this.digitalIo.SetProducerHeartbeat(producerHeartbeatTime);

            if (false != result)
            {
               this.StatusLabel.Text = "Digital IO producer heartbeat set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Digital IO producer heartbeat.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #region Analog IO Events

      private void AnalogIoConfigButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.AnalogIoActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.analogIo.NodeId = nodeId;
            bool result = this.analogIo.Configure();

            if (false != result)
            {
               this.StatusLabel.Text = "Analog IO configured.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to configure Analog IO .";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void AnalogIoStartButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.AnalogIoActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.analogIo.NodeId = nodeId;
            bool result = this.analogIo.Start();

            if (false != result)
            {
               this.StatusLabel.Text = "Analog IO started.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to start Analog IO .";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void AnalogIoStopButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.AnalogIoActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.analogIo.NodeId = nodeId;
            this.analogIo.Stop();
            this.StatusLabel.Text = "Analog IO stopped.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void AnalogIoResetButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.AnalogIoActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.analogIo.NodeId = nodeId;
            this.analogIo.Reset();
            this.StatusLabel.Text = "Analog IO reset.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetAnalogIoBaudRateButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         int deviceRate = 0;

         if ((byte.TryParse(this.AnalogIoActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (int.TryParse(this.AnalogIoBaudComboBox.Text, out deviceRate) != false))
         {
            this.analogIo.NodeId = nodeId;
            bool result = this.analogIo.SetDeviceBaudRate(deviceRate);

            if (false != result)
            {
               this.StatusLabel.Text = "Analog IO baudrate set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Analog IO bbaudrate.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetAnalogIoNodeIdButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         byte deviceNodeId = 0;

         if ((byte.TryParse(this.AnalogIoActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (byte.TryParse(this.AnalogIoNodeIdTextBox.Text, out deviceNodeId) != false))
         {
            this.analogIo.NodeId = nodeId;
            bool result = this.analogIo.SetDeviceNodeId(deviceNodeId);

            if (false != result)
            {
               this.StatusLabel.Text = "Analog IO node ID set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Analog IO node ID.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetAnalogIoConsumerHeartbeatButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         byte consumerHeartbeatNodeId = 0;
         UInt16 consumerHeartbeatTime = 0;

         if ((byte.TryParse(this.AnalogIoActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (byte.TryParse(this.AnalogIoConsumerHeartbeatNodeIdTextBox.Text, out consumerHeartbeatNodeId) != false) &&
             (UInt16.TryParse(this.AnalogIoConsumerHeartbeatTimeTextBox.Text, out consumerHeartbeatTime) != false))
         {
            this.analogIo.NodeId = nodeId;
            bool result = this.analogIo.SetConsumerHeartbeat(consumerHeartbeatTime, consumerHeartbeatNodeId);

            if (false != result)
            {
               this.StatusLabel.Text = "Analog IO consumer heartbeat set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Analog IO consumer heartbeat.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetAnalogIoProducerHeartbeatButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt16 producerHeartbeatTime = 0;

         if ((byte.TryParse(this.AnalogIoActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt16.TryParse(this.AnalogIoProducerHeartbeatTimeTextBox.Text, out producerHeartbeatTime) != false))
         {
            this.analogIo.NodeId = nodeId;
            bool result = this.analogIo.SetProducerHeartbeat(producerHeartbeatTime);

            if (false != result)
            {
               this.StatusLabel.Text = "Analog IO producer heartbeat set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Analog IO producer heartbeat.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetAnalogIoOutput0Button_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt16 outputValue = 0;

         if ((byte.TryParse(this.AnalogIoActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt16.TryParse(this.AnalogIoOutput0TextBox.Text, out outputValue) != false))
         {
            this.analogIo.NodeId = nodeId;
            bool result = this.analogIo.SetOutput(0, outputValue);

            if (false != result)
            {
               this.StatusLabel.Text = "Analog IO output 0 set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Analog IO output 0.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetAnalogIoOutput1Button_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt16 outputValue = 0;

         if ((byte.TryParse(this.AnalogIoActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt16.TryParse(this.AnalogIoOutput1TextBox.Text, out outputValue) != false))
         {
            this.analogIo.NodeId = nodeId;
            bool result = this.analogIo.SetOutput(1, outputValue);

            if (false != result)
            {
               this.StatusLabel.Text = "Analog IO output 1 set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Analog IO output 1.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetAnalogIoOutput2Button_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt16 outputValue = 0;

         if ((byte.TryParse(this.AnalogIoActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt16.TryParse(this.AnalogIoOutput2TextBox.Text, out outputValue) != false))
         {
            this.analogIo.NodeId = nodeId;
            bool result = this.analogIo.SetOutput(2, outputValue);

            if (false != result)
            {
               this.StatusLabel.Text = "Analog IO output 2 set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Analog IO output 2.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetAnalogIoOutput3Button_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt16 outputValue = 0;

         if ((byte.TryParse(this.AnalogIoActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt16.TryParse(this.AnalogIoOutput3TextBox.Text, out outputValue) != false))
         {
            this.analogIo.NodeId = nodeId;
            bool result = this.analogIo.SetOutput(3, outputValue);

            if (false != result)
            {
               this.StatusLabel.Text = "Analog IO output 3 set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Analog IO output 3.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #region Encoder Device Events

      private void EncoderConfigButton_Click(object sender, EventArgs e)
      {
         byte encoderNodeId = 0;

         if (byte.TryParse(this.ActiveEncoderNodeIdTextBox.Text, out encoderNodeId) != false)
         {
            this.encoder.NodeId = encoderNodeId;
            bool result = this.encoder.Configure();

            if (false != result)
            {
               this.StatusLabel.Text = "Encoder configured.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to configure encoder.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void EncoderStartButton_Click(object sender, EventArgs e)
      {
         byte encoderNodeId = 0;

         if (byte.TryParse(this.ActiveEncoderNodeIdTextBox.Text, out encoderNodeId) != false)
         {
            this.encoder.NodeId = encoderNodeId;
            bool result = this.encoder.Start();

            if (false != result)
            {
               this.StatusLabel.Text = "Encoder started.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to start encoder.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void EncoderStopButton_Click(object sender, EventArgs e)
      {
         byte encoderNodeId = 0;

         if (byte.TryParse(this.ActiveEncoderNodeIdTextBox.Text, out encoderNodeId) != false)
         {
            this.encoder.NodeId = encoderNodeId;
            this.encoder.Stop();
            this.StatusLabel.Text = "Encoder stopped.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void EncoderResetButton_Click(object sender, EventArgs e)
      {
         byte encoderNodeId = 0;

         if (byte.TryParse(this.ActiveEncoderNodeIdTextBox.Text, out encoderNodeId) != false)
         {
            this.encoder.NodeId = encoderNodeId;
            this.encoder.Reset();
            this.StatusLabel.Text = "Encoder reset.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetDeviceBaudRateButton_Click(object sender, EventArgs e)
      {
         byte encoderNodeId = 0;

         if (byte.TryParse(this.ActiveEncoderNodeIdTextBox.Text, out encoderNodeId) != false)
         {
            this.encoder.NodeId = encoderNodeId;
            int deviceRate = this.encoder.GetDeviceBaudRate();

            if (0 != deviceRate)
            {
               this.StatusLabel.Text = "Baudrate retrieved.";
               this.EncoderBaudComboBox.Text = deviceRate.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get baudrate.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetDeviceBaudRateButton_Click(object sender, EventArgs e)
      {
         byte encoderNodeId = 0;
         int deviceRate = 0;

         if ((byte.TryParse(this.ActiveEncoderNodeIdTextBox.Text, out encoderNodeId) != false) &&
             (int.TryParse(this.EncoderBaudComboBox.Text, out deviceRate) != false))
         {
            this.encoder.NodeId = encoderNodeId;
            bool result = this.encoder.SetDeviceBaudRate(deviceRate);

            if (false != result)
            {
               this.StatusLabel.Text = "Baudrate set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set baudrate.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetDeviceNodeIdButton_Click(object sender, EventArgs e)
      {
         byte encoderNodeId = 0;

         if (byte.TryParse(this.ActiveEncoderNodeIdTextBox.Text, out encoderNodeId) != false)
         {
            this.encoder.NodeId = encoderNodeId;
            int deviceNodeId = this.encoder.GetDeviceNodeId();

            if (0 != deviceNodeId)
            {
               this.StatusLabel.Text = "Node ID retrieved.";
               this.EncoderNodeIdTextBox.Text = deviceNodeId.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get node ID.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetDeviceNodeIdButton_Click(object sender, EventArgs e)
      {
         byte encoderNodeId = 0;
         byte deviceNodeId = 0;

         if ((byte.TryParse(this.ActiveEncoderNodeIdTextBox.Text, out encoderNodeId) != false) &&
             (byte.TryParse(this.EncoderNodeIdTextBox.Text, out deviceNodeId) != false))
         {
            this.encoder.NodeId = encoderNodeId;
            bool result = this.encoder.SetDeviceNodeId(deviceNodeId);

            if (false != result)
            {
               this.StatusLabel.Text = "Node ID set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set node ID.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SaveBusConfigurationButton_Click(object sender, EventArgs e)
      {
         byte encoderNodeId = 0;
         
         if (byte.TryParse(this.ActiveEncoderNodeIdTextBox.Text, out encoderNodeId) != false) 
         {
            this.encoder.NodeId = encoderNodeId;
            bool result = this.encoder.SaveBusConfiguration();

            if (false != result)
            {
               this.StatusLabel.Text = "Bus configuration saved.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to save bus configuration.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #region Motor Device Events

      private void MotorStartButton_Click_1(object sender, EventArgs e)
      {
         byte motorNodeId = 0;

         if (byte.TryParse(this.ActiveMotorNodeIdTextBox.Text, out motorNodeId) != false)
         {
            this.motor.NodeId = motorNodeId;
            this.motor.Start();
            this.StatusLabel.Text = "Motor started.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void MotorStopButton_Click_1(object sender, EventArgs e)
      {
         byte motorNodeId = 0;

         if (byte.TryParse(this.ActiveMotorNodeIdTextBox.Text, out motorNodeId) != false)
         {
            this.motor.NodeId = motorNodeId;
            this.motor.Stop();
            this.StatusLabel.Text = "Motor stopped.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void MotorResetButton_Click_1(object sender, EventArgs e)
      {
         byte motorNodeId = 0;

         if (byte.TryParse(this.ActiveMotorNodeIdTextBox.Text, out motorNodeId) != false)
         {
            this.motor.NodeId = motorNodeId;
            this.motor.Reset();
            this.StatusLabel.Text = "Motor reset.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetMotorModeButton_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;
         ElmoWhistleMotor.Modes mode = ElmoWhistleMotor.Modes.undefined;

         if (this.MotorModeComboBox.Text == "off")
         {
            mode = ElmoWhistleMotor.Modes.off;
         }
         else if (this.MotorModeComboBox.Text == "velocity")
         {
            mode = ElmoWhistleMotor.Modes.velocity;
         }
         else if (this.MotorModeComboBox.Text == "current")
         {
            mode = ElmoWhistleMotor.Modes.current;
         }

         if (byte.TryParse(this.ActiveMotorNodeIdTextBox.Text, out motorNodeId) != false)
         {
            this.motor.NodeId = motorNodeId;
            bool result = this.motor.SetMode(mode);

            if (false != result)
            {
               this.StatusLabel.Text = "Motor mode set to " + mode.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set motor mode to " + mode.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetMotorVelocityButton_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;
         Int32 velocity = 0;

         if ((byte.TryParse(this.ActiveMotorNodeIdTextBox.Text, out motorNodeId) != false) &&
             (Int32.TryParse(this.MotorVelocityTextBox.Text, out velocity) != false))
         {
            this.motor.NodeId = motorNodeId;
            bool result = this.motor.SetVelocity(velocity);

            if (false != result)
            {
               this.StatusLabel.Text = "Motor velocity set to " + velocity.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set motor velocity to " + velocity.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void ScheduleMotorVelocityButton_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;
         Int32 velocity = 0;

         if ((byte.TryParse(this.ActiveMotorNodeIdTextBox.Text, out motorNodeId) != false) &&
             (Int32.TryParse(this.MotorVelocityTextBox.Text, out velocity) != false))
         {
            this.motor.NodeId = motorNodeId;
            bool result = this.motor.ScheduleVelocity(velocity);

            if (false != result)
            {
               this.StatusLabel.Text = "Motor velocity scheduled for " + velocity.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to schedule motor velocity for " + velocity.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetMotorTorqueButton_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;
         float torque = 0;

         if ((byte.TryParse(this.ActiveMotorNodeIdTextBox.Text, out motorNodeId) != false) &&
             (float.TryParse(this.MotorTorqueTextBox.Text, out torque) != false))
         {
            this.motor.NodeId = motorNodeId;
            bool result = this.motor.SetTorque(torque);

            if (false != result)
            {
               this.StatusLabel.Text = "Motor torque set to " + torque.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set motor torque to " + torque.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void ScheduleMotorTorqueButton_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;
         float torque = 0;

         if ((byte.TryParse(this.ActiveMotorNodeIdTextBox.Text, out motorNodeId) != false) &&
             (float.TryParse(this.MotorTorqueTextBox.Text, out torque) != false))
         {
            this.motor.NodeId = motorNodeId;
            bool result = this.motor.ScheduleTorque(torque);

            if (false != result)
            {
               this.StatusLabel.Text = "Motor torque schedule for " + torque.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to schedule motor torque for " + torque.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void MotorSyncButton_Click(object sender, EventArgs e)
      {
         this.busInterface.Sync();
      }

      #endregion

      #region Motor Device Test Events

      private void MotorStartButton_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;

         if (byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false)
         {
            this.motor.NodeId = motorNodeId;
            this.motor.Start();
            this.StatusLabel.Text = "Motor started.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void MotorStopButton_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;

         if (byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false)
         {
            this.motor.NodeId = motorNodeId;
            this.motor.Stop();
            this.StatusLabel.Text = "Motor stopped.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void MotorResetButton_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;

         if (byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false)
         {
            this.motor.NodeId = motorNodeId;
            this.motor.Reset();
            this.StatusLabel.Text = "Motor reset.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void TestMotorButton_Click(object sender, EventArgs e)
      {
         this.motor.Test();
      }

      private void SetMotorUserModeButton_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;
         int unitMode = 0;

         if ((byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false) &&
             (int.TryParse(this.MotorUnitModeTextBox.Text, out unitMode) != false))
         {
            this.motor.NodeId = motorNodeId;
            bool result = this.motor.SetMode(unitMode);

            if (false != result)
            {
               this.StatusLabel.Text = string.Format("Unit mode set to {0}.", unitMode);
            }
            else
            {
               this.StatusLabel.Text = "Unable to set unit mode.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetMotor_6040_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;

         if (byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false)
         {
            this.motor.NodeId = motorNodeId;
            byte[] result = this.motor.SDOUpLoad(0x6040, 0);

            if (null != result)
            {
               UInt32 value = BitConverter.ToUInt16(result, 0);
               this.Motor_6040_TextBox.Text = string.Format("{0:X4}", value);
               this.StatusLabel.Text = "6040h read.";
            }
            else
            {
               this.Motor_6040_TextBox.Text = "";
               this.StatusLabel.Text = "Unable to read 6040h.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetMotor_6040_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;
         UInt16 value = 0;

         if ((byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false) &&
             (UInt16.TryParse(this.Motor_6040_TextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out value) != false))
         {
            byte[] valueArray = BitConverter.GetBytes(value);
            this.motor.NodeId = motorNodeId;
            bool result = this.motor.SDODownload(0x6040, 0, valueArray);

            if (false != result)
            {
               this.StatusLabel.Text = string.Format("6040h set to {0:X8}.", value);
            }
            else
            {
               this.StatusLabel.Text = "Unable to set 6040h.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetMotor_6041_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;

         if (byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false)
         {
            this.motor.NodeId = motorNodeId;
            byte[] result = this.motor.SDOUpLoad(0x6041, 0);

            if (null != result)
            {
               UInt32 value = BitConverter.ToUInt16(result, 0);
               this.Motor_6041_TextBox.Text = string.Format("{0:X4}", value);
               this.StatusLabel.Text = "6040h read.";
            }
            else
            {
               this.Motor_6041_TextBox.Text = "";
               this.StatusLabel.Text = "Unable to read 6041h.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetMotor_6060_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;

         if (byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false)
         {
            this.motor.NodeId = motorNodeId;
            byte[] result = this.motor.SDOUpLoad(0x6060, 0);

            if (null != result)
            {
               this.Motor_6060_TextBox.Text = string.Format("{0:X2}", result[0]);
               this.StatusLabel.Text = "6060h read.";
            }
            else
            {
               this.Motor_6060_TextBox.Text = "";
               this.StatusLabel.Text = "Unable to read 6060h.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetMotor_6060_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;
         byte value = 0;

         if ((byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false) &&
             (byte.TryParse(this.Motor_6060_TextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out value) != false))
         {
            byte[] valueArray = BitConverter.GetBytes(value);
            this.motor.NodeId = motorNodeId;
            bool result = this.motor.SDODownload(0x6060, 0, valueArray);

            if (false != result)
            {
               this.StatusLabel.Text = string.Format("6060h set to {0:X2}.", value);
            }
            else
            {
               this.StatusLabel.Text = "Unable to set 6060h.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetMotor_6061_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;

         if (byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false)
         {
            this.motor.NodeId = motorNodeId;
            byte[] result = this.motor.SDOUpLoad(0x6061, 0);

            if (null != result)
            {
               this.Motor_6061_TextBox.Text = string.Format("{0:X2}", result[0]);
               this.StatusLabel.Text = "6061h read.";
            }
            else
            {
               this.Motor_6061_TextBox.Text = "";
               this.StatusLabel.Text = "Unable to read 6061h.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetMotor_6069_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;

         if (byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false)
         {
            this.motor.NodeId = motorNodeId;
            byte[] result = this.motor.SDOUpLoad(0x6069, 0);

            if (null != result)
            {
               Int32 value = BitConverter.ToInt32(result, 0);
               this.Motor_6069_TextBox.Text = string.Format("{0:X8}", value);
               this.StatusLabel.Text = "6069h read.";
            }
            else
            {
               this.Motor_6069_TextBox.Text = "";
               this.StatusLabel.Text = "Unable to read 6069h.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetMotor_6069_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;
         Int32 value = 0;

         if ((byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false) &&
             (Int32.TryParse(this.Motor_6069_TextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out value) != false))
         {
            byte[] valueArray = BitConverter.GetBytes(value);
            this.motor.NodeId = motorNodeId;
            bool result = this.motor.SDODownload(0x6069, 0, valueArray);

            if (false != result)
            {
               this.StatusLabel.Text = string.Format("6069h set to {0:X8}.", value);
            }
            else
            {
               this.StatusLabel.Text = "Unable to set 6069h.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetMotor_606A_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;

         if (byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false)
         {
            this.motor.NodeId = motorNodeId;
            byte[] result = this.motor.SDOUpLoad(0x606A, 0);

            if (null != result)
            {
               Int16 value = BitConverter.ToInt16(result, 0);
               this.Motor_606A_TextBox.Text = string.Format("{0:X4}", value);
               this.StatusLabel.Text = "606Ah read.";
            }
            else
            {
               this.Motor_606A_TextBox.Text = "";
               this.StatusLabel.Text = "Unable to read 606Ah.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetMotor_606A_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;
         Int16 value = 0;

         if ((byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false) &&
             (Int16.TryParse(this.Motor_606A_TextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out value) != false))
         {
            byte[] valueArray = BitConverter.GetBytes(value);
            this.motor.NodeId = motorNodeId;
            bool result = this.motor.SDODownload(0x606A, 0, valueArray);

            if (false != result)
            {
               this.StatusLabel.Text = string.Format("606Ah set to {0:X4}.", value);
            }
            else
            {
               this.StatusLabel.Text = "Unable to set 606Ah.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetMotor_606B_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;

         if (byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false)
         {
            this.motor.NodeId = motorNodeId;
            byte[] result = this.motor.SDOUpLoad(0x606B, 0);

            if (null != result)
            {
               Int32 value = BitConverter.ToInt32(result, 0);
               this.Motor_606B_TextBox.Text = string.Format("{0:X8}", value);
               this.StatusLabel.Text = "606Bh read.";
            }
            else
            {
               this.Motor_606B_TextBox.Text = "";
               this.StatusLabel.Text = "Unable to read 606Bh.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetMotor_606C_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;

         if (byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false)
         {
            this.motor.NodeId = motorNodeId;
            byte[] result = this.motor.SDOUpLoad(0x606C, 0);

            if (null != result)
            {
               Int32 value = BitConverter.ToInt32(result, 0);
               this.Motor_606C_TextBox.Text = string.Format("{0:X8}", value);
               this.StatusLabel.Text = "606Ch read.";
            }
            else
            {
               this.Motor_606C_TextBox.Text = "";
               this.StatusLabel.Text = "Unable to read 606Ch.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetMotor_606D_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;

         if (byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false)
         {
            this.motor.NodeId = motorNodeId;
            byte[] result = this.motor.SDOUpLoad(0x606D, 0);

            if (null != result)
            {
               UInt16 value = BitConverter.ToUInt16(result, 0);
               this.Motor_606D_TextBox.Text = string.Format("{0:X4}", value);
               this.StatusLabel.Text = "606Dh read.";
            }
            else
            {
               this.Motor_606D_TextBox.Text = "";
               this.StatusLabel.Text = "Unable to read 606Dh.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetMotor_606D_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;
         UInt16 value = 0;

         if ((byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false) &&
             (UInt16.TryParse(this.Motor_606D_TextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out value) != false))
         {
            byte[] valueArray = BitConverter.GetBytes(value);
            this.motor.NodeId = motorNodeId;
            bool result = this.motor.SDODownload(0x606D, 0, valueArray);

            if (false != result)
            {
               this.StatusLabel.Text = string.Format("606Dh set to {0:X4}.", value);
            }
            else
            {
               this.StatusLabel.Text = "Unable to set 606Dh.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetMotor_606E_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;

         if (byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false)
         {
            this.motor.NodeId = motorNodeId;
            byte[] result = this.motor.SDOUpLoad(0x606E, 0);

            if (null != result)
            {
               UInt16 value = BitConverter.ToUInt16(result, 0);
               this.Motor_606E_TextBox.Text = string.Format("{0:X4}", value);
               this.StatusLabel.Text = "606Eh read.";
            }
            else
            {
               this.Motor_606E_TextBox.Text = "";
               this.StatusLabel.Text = "Unable to read 606Eh.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetMotor_606E_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;
         UInt16 value = 0;

         if ((byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false) &&
             (UInt16.TryParse(this.Motor_606E_TextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out value) != false))
         {
            byte[] valueArray = BitConverter.GetBytes(value);
            this.motor.NodeId = motorNodeId;
            bool result = this.motor.SDODownload(0x606E, 0, valueArray);

            if (false != result)
            {
               this.StatusLabel.Text = string.Format("606Eh set to {0:X4}.", value);
            }
            else
            {
               this.StatusLabel.Text = "Unable to set 606Eh.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetMotor_606F_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;

         if (byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false)
         {
            this.motor.NodeId = motorNodeId;
            byte[] result = this.motor.SDOUpLoad(0x606F, 0);

            if (null != result)
            {
               UInt16 value = BitConverter.ToUInt16(result, 0);
               this.Motor_606F_TextBox.Text = string.Format("{0:X4}", value);
               this.StatusLabel.Text = "606Fh read.";
            }
            else
            {
               this.Motor_606F_TextBox.Text = "";
               this.StatusLabel.Text = "Unable to read 606Fh.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetMotor_606F_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;
         UInt16 value = 0;

         if ((byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false) &&
             (UInt16.TryParse(this.Motor_606F_TextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out value) != false))
         {
            byte[] valueArray = BitConverter.GetBytes(value);
            this.motor.NodeId = motorNodeId;
            bool result = this.motor.SDODownload(0x606F, 0, valueArray);

            if (false != result)
            {
               this.StatusLabel.Text = string.Format("606Fh set to {0:X4}.", value);
            }
            else
            {
               this.StatusLabel.Text = "Unable to set 606Fh.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetMotor_6070_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;

         if (byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false)
         {
            this.motor.NodeId = motorNodeId;
            byte[] result = this.motor.SDOUpLoad(0x6070, 0);

            if (null != result)
            {
               UInt16 value = BitConverter.ToUInt16(result, 0);
               this.Motor_6070_TextBox.Text = string.Format("{0:X4}", value);
               this.StatusLabel.Text = "6070h read.";
            }
            else
            {
               this.Motor_6070_TextBox.Text = "";
               this.StatusLabel.Text = "Unable to read 6070h.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetMotor_6070_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;
         UInt16 value = 0;

         if ((byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false) &&
             (UInt16.TryParse(this.Motor_6070_TextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out value) != false))
         {
            byte[] valueArray = BitConverter.GetBytes(value);
            this.motor.NodeId = motorNodeId;
            bool result = this.motor.SDODownload(0x6070, 0, valueArray);

            if (false != result)
            {
               this.StatusLabel.Text = string.Format("6070h set to {0:X4}.", value);
            }
            else
            {
               this.StatusLabel.Text = "Unable to set 6070h.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetMotor_6083_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;

         if (byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false)
         {
            this.motor.NodeId = motorNodeId;
            byte[] result = this.motor.SDOUpLoad(0x6083, 0);

            if (null != result)
            {
               UInt32 value = BitConverter.ToUInt32(result, 0);
               this.Motor_6083_TextBox.Text = string.Format("{0:X8}", value);
               this.StatusLabel.Text = "6083h read.";
            }
            else
            {
               this.Motor_60FF_TextBox.Text = "";
               this.StatusLabel.Text = "Unable to read 6083h.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetMotor_6083_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;
         Int32 value = 0;

         if ((byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false) &&
             (Int32.TryParse(this.Motor_6083_TextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out value) != false))
         {
            byte[] valueArray = BitConverter.GetBytes(value);
            this.motor.NodeId = motorNodeId;
            bool result = this.motor.SDODownload(0x6083, 0, valueArray);

            if (false != result)
            {
               this.StatusLabel.Text = string.Format("6083Fh set to {0:X8}.", value);
            }
            else
            {
               this.StatusLabel.Text = "Unable to set 6083Fh.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetMotor_6084_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;

         if (byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false)
         {
            this.motor.NodeId = motorNodeId;
            byte[] result = this.motor.SDOUpLoad(0x6084, 0);

            if (null != result)
            {
               UInt32 value = BitConverter.ToUInt32(result, 0);
               this.Motor_6084_TextBox.Text = string.Format("{0:X8}", value);
               this.StatusLabel.Text = "6084h read.";
            }
            else
            {
               this.Motor_60FF_TextBox.Text = "";
               this.StatusLabel.Text = "Unable to read 6084h.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetMotor_6084_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;
         Int32 value = 0;

         if ((byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false) &&
             (Int32.TryParse(this.Motor_6084_TextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out value) != false))
         {
            byte[] valueArray = BitConverter.GetBytes(value);
            this.motor.NodeId = motorNodeId;
            bool result = this.motor.SDODownload(0x6084, 0, valueArray);

            if (false != result)
            {
               this.StatusLabel.Text = string.Format("6084Fh set to {0:X8}.", value);
            }
            else
            {
               this.StatusLabel.Text = "Unable to set 6084Fh.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetMotor_60FF_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;

         if (byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false)
         {
            this.motor.NodeId = motorNodeId;
            byte[] result = this.motor.SDOUpLoad(0x60FF, 0);

            if (null != result)
            {
               Int32 value = BitConverter.ToInt32(result, 0);
               this.Motor_60FF_TextBox.Text = string.Format("{0:X8}", value);
               this.StatusLabel.Text = "60FFh read.";
            }
            else
            {
               this.Motor_60FF_TextBox.Text = "";
               this.StatusLabel.Text = "Unable to read 60FFh.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetMotor_60FF_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;
         Int32 value = 0;

         if ((byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false) &&
             (Int32.TryParse(this.Motor_60FF_TextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out value) != false))
         {
            byte[] valueArray = BitConverter.GetBytes(value);
            this.motor.NodeId = motorNodeId;
            bool result = this.motor.SDODownload(0x60FF, 0, valueArray);

            if (false != result)
            {
               this.StatusLabel.Text = string.Format("60FFh set to {0:X8}.", value);
            }
            else
            {
               this.StatusLabel.Text = "Unable to set 60FFh.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetMotor_6502_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;

         if (byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false)
         {
            this.motor.NodeId = motorNodeId;

            byte[] result = this.motor.SDOUpLoad(0x6502, 0);

            if (null != result)
            {
               UInt32 value = BitConverter.ToUInt32(result, 0);
               this.Motor_6502_TextBox.Text = string.Format("{0:X8}", value);
               this.StatusLabel.Text = "6502h read.";
            }
            else
            {
               this.Motor_6502_TextBox.Text = "";
               this.StatusLabel.Text = "Unable to read 6502h.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetMotor_6504_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;

         if (byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false)
         {
            this.motor.NodeId = motorNodeId;

            byte[] result = this.motor.SDOUpLoad(0x6504, 0);

            if (null != result)
            {
               string value = Encoding.UTF8.GetString(result);
               this.Motor_6504_TextBox.Text = value;
               this.StatusLabel.Text = "6504h read.";
            }
            else
            {
               this.Motor_6504_TextBox.Text = "";
               this.StatusLabel.Text = "Unable to read 6504h.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetMotor_6505_Button_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;

         if (byte.TryParse(this.MotorTestActiveNodeIdTextBox.Text, out motorNodeId) != false)
         {
            this.motor.NodeId = motorNodeId;

            byte[] result = this.motor.SDOUpLoad(0x6505, 0);

            if (null != result)
            {
               string value = Encoding.UTF8.GetString(result);
               this.Motor_6505_TextBox.Text = value;
               this.StatusLabel.Text = "6505h read.";
            }
            else
            {
               this.Motor_6505_TextBox.Text = "";
               this.StatusLabel.Text = "Unable to read 6505h.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }
      
      #endregion

      #region Camera Device Events

      private void CameraStartButton_Click(object sender, EventArgs e)
      {
         byte cameraNodeId = 0;

         if (byte.TryParse(this.CameraActiveNodeIdTextBox.Text, out cameraNodeId) != false)
         {
            this.camera.NodeId = cameraNodeId;
            this.camera.Start();
            this.StatusLabel.Text = "Camera started.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void CameraStopButton_Click(object sender, EventArgs e)
      {
         byte cameraNodeId = 0;

         if (byte.TryParse(this.CameraActiveNodeIdTextBox.Text, out cameraNodeId) != false)
         {
            this.camera.NodeId = cameraNodeId;
            this.camera.Stop();
            this.StatusLabel.Text = "Camera stopped.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void CameraResetButton_Click(object sender, EventArgs e)
      {
         byte cameraNodeId = 0;

         if (byte.TryParse(this.CameraActiveNodeIdTextBox.Text, out cameraNodeId) != false)
         {
            this.camera.NodeId = cameraNodeId;
            this.camera.Reset();
            this.StatusLabel.Text = "Camera reset.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void CameraOnButton_Click(object sender, EventArgs e)
      {
         byte cameraNodeId = 0;

         if (byte.TryParse(this.CameraActiveNodeIdTextBox.Text, out cameraNodeId) != false)
         {
            this.camera.NodeId = cameraNodeId;
            bool result = this.camera.SetState(true);

            if (false != result)
            {
               this.StatusLabel.Text = "Camera on.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to camera on.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void CameraOffButton_Click(object sender, EventArgs e)
      {
         byte cameraNodeId = 0;

         if (byte.TryParse(this.CameraActiveNodeIdTextBox.Text, out cameraNodeId) != false)
         {
            this.camera.NodeId = cameraNodeId;
            bool result = this.camera.SetState(false);

            if (false != result)
            {
               this.StatusLabel.Text = "Camera off.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to camera off.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void CameraLightIntensitySetButton_Click(object sender, EventArgs e)
      {
         byte cameraNodeId = 0;
         byte intensity = 0;

         if ((byte.TryParse(this.CameraActiveNodeIdTextBox.Text, out cameraNodeId) != false) &&
             (byte.TryParse(this.CameraLightIntensityTextBox.Text, out intensity) != false) &&
             (intensity >= this.CameraLightIntensityTrackBar.Minimum) &&
             (intensity <= this.CameraLightIntensityTrackBar.Maximum))
         {
            this.camera.NodeId = cameraNodeId;
            bool result = this.camera.SetLightIntensity(intensity);

            if (false != result)
            {
               this.StatusLabel.Text = "Light intensity set.";
               this.CameraLightIntensityTrackBar.Value = intensity;
            }
            else
            {
               this.StatusLabel.Text = "Unable to set light intensity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void CameraAutoSetCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         if (false != this.CameraAutoSetCheckBox.Checked)
         {
            this.cameraAutoSendTimeLimit = DateTime.Now.AddSeconds(1);
         }
      }

      private void CameraLightIntensityTrackBar_Scroll(object sender, EventArgs e)
      {
         byte cameraNodeId = 0;

         if (byte.TryParse(this.CameraActiveNodeIdTextBox.Text, out cameraNodeId) != false)
         {
            byte intensity = (byte)this.CameraLightIntensityTrackBar.Value;
            this.camera.NodeId = cameraNodeId;
            bool result = this.camera.SetLightIntensity(intensity);

            if (false != result)
            {
               this.StatusLabel.Text = "Light intensity set.";
               this.CameraLightIntensityTextBox.Text = intensity.ToString();
            }
         }
      }

      private void GetCameraBaudRateButton_Click(object sender, EventArgs e)
      {
         byte cameraNodeId = 0;

         if (byte.TryParse(this.CameraActiveNodeIdTextBox.Text, out cameraNodeId) != false)
         {
            this.camera.NodeId = cameraNodeId;
            int deviceRate = this.camera.GetDeviceBaudRate();

            if (0 != deviceRate)
            {
               this.StatusLabel.Text = "Baudrate retrieved.";
               this.CameraBaudComboBox.Text = deviceRate.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get baudrate.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetCameraBaudRateButton_Click(object sender, EventArgs e)
      {
         byte cameraNodeId = 0;
         int deviceRate = 0;

         if ((byte.TryParse(this.CameraActiveNodeIdTextBox.Text, out cameraNodeId) != false) &&
             (int.TryParse(this.CameraBaudComboBox.Text, out deviceRate) != false))
         {
            this.camera.NodeId = cameraNodeId;
            bool result = this.camera.SetDeviceBaudRate(deviceRate);

            if (false != result)
            {
               this.StatusLabel.Text = "Baudrate set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set baudrate.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetCameraNodeIdButton_Click(object sender, EventArgs e)
      {
         byte cameraNodeId = 0;

         if (byte.TryParse(this.CameraActiveNodeIdTextBox.Text, out cameraNodeId) != false)
         {
            this.camera.NodeId = cameraNodeId;
            byte deviceNodeId = 0;
            bool success = this.camera.GetDeviceNodeId(ref deviceNodeId);

            if (false != success)
            {
               this.StatusLabel.Text = "Node ID retrieved.";
               this.CameraNodeIdTextBox.Text = deviceNodeId.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get node ID.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetCameraNodeIdButton_Click(object sender, EventArgs e)
      {
         byte cameraNodeId = 0;
         byte deviceNodeId = 0;

         if ((byte.TryParse(this.CameraActiveNodeIdTextBox.Text, out cameraNodeId) != false) &&
             (byte.TryParse(this.CameraNodeIdTextBox.Text, out deviceNodeId) != false))
         {
            this.camera.NodeId = cameraNodeId;
            bool result = this.camera.SetDeviceNodeId(deviceNodeId);

            if (false != result)
            {
               this.StatusLabel.Text = "Node ID set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set node ID.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetCameraTimeoutButton_Click(object sender, EventArgs e)
      {
         byte cameraNodeId = 0;

         if (byte.TryParse(this.CameraActiveNodeIdTextBox.Text, out cameraNodeId) != false)
         {
            this.camera.NodeId = cameraNodeId;
            UInt16 timeout = 0;
            bool success = this.camera.GetTimeout(ref timeout);

            if (false != success)
            {
               this.StatusLabel.Text = "Timeout retrieved.";
               this.CameraTimeoutTextBox.Text = timeout.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get timeout.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetCameraTimeoutButton_Click(object sender, EventArgs e)
      {
         byte cameraNodeId = 0;
         UInt16 timeout = 0;

         if ((byte.TryParse(this.CameraActiveNodeIdTextBox.Text, out cameraNodeId) != false) &&
             (UInt16.TryParse(this.CameraTimeoutTextBox.Text, out timeout) != false))
         {
            this.camera.NodeId = cameraNodeId;
            bool result = this.camera.SetTimeout(timeout);

            if (false != result)
            {
               this.StatusLabel.Text = "Timeout set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set timeout.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetCameraLocationButton_Click(object sender, EventArgs e)
      {
         byte cameraNodeId = 0;

         if (byte.TryParse(this.CameraActiveNodeIdTextBox.Text, out cameraNodeId) != false)
         {
            this.camera.NodeId = cameraNodeId;
            string location = "";
            bool success = this.camera.GetLocation(ref location);

            if (false != success)
            {
               this.StatusLabel.Text = "Location retrieved.";
               this.CameraLocationTextBox.Text = location;
            }
            else
            {
               this.StatusLabel.Text = "Unable to get location.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetCameraLocationButton_Click(object sender, EventArgs e)
      {
         byte cameraNodeId = 0;
         string location = "";

         if (byte.TryParse(this.CameraActiveNodeIdTextBox.Text, out cameraNodeId) != false)
         {
            this.camera.NodeId = cameraNodeId;
            location = this.CameraLocationTextBox.Text;
            bool result = this.camera.SetLocation(location);

            if (false != result)
            {
               this.StatusLabel.Text = "Location set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set location.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetCameraPowerUpStateButton_Click(object sender, EventArgs e)
      {
         byte cameraNodeId = 0;

         if (byte.TryParse(this.CameraActiveNodeIdTextBox.Text, out cameraNodeId) != false)
         {
            this.camera.NodeId = cameraNodeId;
            byte powerUpState = 0;
            bool success = this.camera.GetPowerUpState(ref powerUpState);

            if (false != success)
            {
               this.StatusLabel.Text = "Power up state retrieved.";
               this.CameraPowerUpStateTextBox.Text = powerUpState.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get power up state.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetCameraPowerUpStateButton_Click(object sender, EventArgs e)
      {
         byte cameraNodeId = 0;
         byte powerUpState = 0;

         if ((byte.TryParse(this.CameraActiveNodeIdTextBox.Text, out cameraNodeId) != false) &&
             (byte.TryParse(this.CameraPowerUpStateTextBox.Text, out powerUpState) != false))
         {
            this.camera.NodeId = cameraNodeId;
            bool result = this.camera.SetPowerUpState(powerUpState);

            if (false != result)
            {
               this.StatusLabel.Text = "Power up state set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set power up state.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetCameraTimeoutStateButton_Click(object sender, EventArgs e)
      {
         byte cameraNodeId = 0;

         if (byte.TryParse(this.CameraActiveNodeIdTextBox.Text, out cameraNodeId) != false)
         {
            this.camera.NodeId = cameraNodeId;
            byte timeoutState = 0;
            bool success = this.camera.GetTimeoutState(ref timeoutState);

            if (false != success)
            {
               this.StatusLabel.Text = "Timeout state retrieved.";
               this.CameraTimeoutStateTextBox.Text = timeoutState.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get timeout state.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetCameraTimeoutStateButton_Click(object sender, EventArgs e)
      {
         byte cameraNodeId = 0;
         byte timeoutState = 0;

         if ((byte.TryParse(this.CameraActiveNodeIdTextBox.Text, out cameraNodeId) != false) &&
             (byte.TryParse(this.CameraTimeoutStateTextBox.Text, out timeoutState) != false))
         {
            this.camera.NodeId = cameraNodeId;
            bool result = this.camera.SetTimeoutState(timeoutState);

            if (false != result)
            {
               this.StatusLabel.Text = "Timeout state set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set timeout state.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetCameraPowerUpIntensityButton_Click(object sender, EventArgs e)
      {
         byte cameraNodeId = 0;

         if (byte.TryParse(this.CameraActiveNodeIdTextBox.Text, out cameraNodeId) != false)
         {
            this.camera.NodeId = cameraNodeId;
            byte powerUpIntensity = 0;
            bool success = this.camera.GetPowerUpIntensity(ref powerUpIntensity);

            if (false != success)
            {
               this.StatusLabel.Text = "Power up intensity retrieved.";
               this.CameraPowerUpIntensityTextBox.Text = powerUpIntensity.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get power up intensity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetCameraPowerUpIntensityButton_Click(object sender, EventArgs e)
      {
         byte cameraNodeId = 0;
         byte powerUpIntensity = 0;

         if ((byte.TryParse(this.CameraActiveNodeIdTextBox.Text, out cameraNodeId) != false) &&
             (byte.TryParse(this.CameraPowerUpIntensityTextBox.Text, out powerUpIntensity) != false))
         {
            this.camera.NodeId = cameraNodeId;
            bool result = this.camera.SetPowerUpIntensity(powerUpIntensity);

            if (false != result)
            {
               this.StatusLabel.Text = "Power up intensity set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set power up intensity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetCameraTimeoutIntensityButton_Click(object sender, EventArgs e)
      {
         byte cameraNodeId = 0;

         if (byte.TryParse(this.CameraActiveNodeIdTextBox.Text, out cameraNodeId) != false)
         {
            this.camera.NodeId = cameraNodeId;
            byte timeoutIntensity = 0;
            bool success = this.camera.GetTimeoutIntensity(ref timeoutIntensity);

            if (false != success)
            {
               this.StatusLabel.Text = "Timeout intensity retrieved.";
               this.CameraTimeoutIntensityTextBox.Text = timeoutIntensity.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get timeout intensity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetCameraTimeoutIntensityButton_Click(object sender, EventArgs e)
      {
         byte cameraNodeId = 0;
         byte timeoutIntensity = 0;

         if ((byte.TryParse(this.CameraActiveNodeIdTextBox.Text, out cameraNodeId) != false) &&
             (byte.TryParse(this.CameraTimeoutIntensityTextBox.Text, out timeoutIntensity) != false))
         {
            this.camera.NodeId = cameraNodeId;
            bool result = this.camera.SetTimeoutIntensity(timeoutIntensity);

            if (false != result)
            {
               this.StatusLabel.Text = "Timeout intensity set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set timeout intensity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SaveCameraConfigurationButton_Click(object sender, EventArgs e)
      {
         byte cameraNodeId = 0;

         if (byte.TryParse(this.CameraActiveNodeIdTextBox.Text, out cameraNodeId) != false)
         {
            this.camera.NodeId = cameraNodeId;
            bool result = this.camera.SaveConfiguration();

            if (false != result)
            {
               this.StatusLabel.Text = "Configuration saved.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to save configuration.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #region GPS Device Events

      private void GpsStartButton_Click(object sender, EventArgs e)
      {
         byte gpsNodeId = 0;

         if (byte.TryParse(this.ActiveGpsNodeIdTextBox.Text, out gpsNodeId) != false)
         {
            this.gps.NodeId = gpsNodeId;
            this.gps.Start();
            this.StatusLabel.Text = "GPS started.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GpsStopButton_Click(object sender, EventArgs e)
      {
         byte gpsNodeId = 0;

         if (byte.TryParse(this.ActiveGpsNodeIdTextBox.Text, out gpsNodeId) != false)
         {
            this.gps.NodeId = gpsNodeId;
            this.gps.Stop();
            this.StatusLabel.Text = "GPS stopped.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GpsResetButton_Click(object sender, EventArgs e)
      {
         byte gpsNodeId = 0;

         if (byte.TryParse(this.ActiveGpsNodeIdTextBox.Text, out gpsNodeId) != false)
         {
            this.gps.NodeId = gpsNodeId;
            this.gps.Reset();
            this.StatusLabel.Text = "GPS reset.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetGpsBaudRateButton_Click(object sender, EventArgs e)
      {
         byte gpsNodeId = 0;

         if (byte.TryParse(this.ActiveGpsNodeIdTextBox.Text, out gpsNodeId) != false)
         {
            this.gps.NodeId = gpsNodeId;
            int deviceRate = this.gps.GetDeviceBaudRate();

            if (0 != deviceRate)
            {
               this.StatusLabel.Text = "GPS baudrate retrieved.";
               this.GpsBaudComboBox.Text = deviceRate.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get GPS baudrate.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetGpsBaudRateButton_Click(object sender, EventArgs e)
      {
         byte gpsNodeId = 0;
         int deviceRate = 0;

         if ((byte.TryParse(this.ActiveGpsNodeIdTextBox.Text, out gpsNodeId) != false) &&
             (int.TryParse(this.GpsBaudComboBox.Text, out deviceRate) != false))
         {
            this.gps.NodeId = gpsNodeId;
            bool result = this.gps.SetDeviceBaudRate(deviceRate);

            if (false != result)
            {
               this.StatusLabel.Text = "GPS baudrate set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set GPS baudrate.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetGpsNodeIdButton_Click(object sender, EventArgs e)
      {
         byte gpsNodeId = 0;

         if (byte.TryParse(this.ActiveGpsNodeIdTextBox.Text, out gpsNodeId) != false)
         {
            this.gps.NodeId = gpsNodeId;
            byte deviceNodeId = 0;
            bool success = this.gps.GetDeviceNodeId(ref deviceNodeId);

            if (false != success)
            {
               this.StatusLabel.Text = "GPS node ID retrieved.";
               this.GpsNodeIdTextBox.Text = deviceNodeId.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get GPS node ID.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetGpsNodeIdButton_Click(object sender, EventArgs e)
      {
         byte gpsNodeId = 0;
         byte deviceNodeId = 0;

         if ((byte.TryParse(this.ActiveGpsNodeIdTextBox.Text, out gpsNodeId) != false) &&
             (byte.TryParse(this.GpsNodeIdTextBox.Text, out deviceNodeId) != false))
         {
            this.gps.NodeId = gpsNodeId;
            bool result = this.gps.SetDeviceNodeId(deviceNodeId);

            if (false != result)
            {
               this.StatusLabel.Text = "GPS node ID set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set GPS node ID.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SaveGpsConfigurationButton_Click(object sender, EventArgs e)
      {
         byte gpsNodeId = 0;

         if (byte.TryParse(this.ActiveGpsNodeIdTextBox.Text, out gpsNodeId) != false)
         {
            this.gps.NodeId = gpsNodeId;
            bool result = this.gps.SaveConfiguration();

            if (false != result)
            {
               this.StatusLabel.Text = "GPS configuration saved.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to save GPS configuration.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #region NICBOT Body Events

      #region General Control

      private void NicbotConfigButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;

            bool result = this.nicbotBody.Configure(UlcRoboticsNicbotBody.Modes.unknown);
            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT body configured.";

               if (UlcRoboticsNicbotBody.Modes.repair == this.nicbotBody.Mode)
               {
                  this.NicbotBodyRepairGroupBox.Visible = true;
                  this.NicbotBodyInspectGroupBox.Visible = false;
                  this.NicbotBodyRepairGroupBox.Top = this.SetNicbotVideoBButton.Top + 28;
               }
               else if (UlcRoboticsNicbotBody.Modes.inspect == this.nicbotBody.Mode)
               {
                  this.NicbotBodyRepairGroupBox.Visible = false;
                  this.NicbotBodyInspectGroupBox.Visible = true;
                  this.NicbotBodyInspectGroupBox.Top = this.SetNicbotVideoBButton.Top + 28;

                  UInt16 solenoidCache = this.nicbotBody.GetSolenoidCache();
                  this.nicbotBodyAssigningSolenoidValues = true;
                  this.NicbotInspectSensorRetractCheckBox.Checked = ((solenoidCache & 0x0001) != 0) ? true : false;
                  this.NicbotInspectSensorExtendCheckBox.Checked = ((solenoidCache & 0x0002) != 0) ? true : false;
                  this.NicbotInspectSensorArmStowCheckBox.Checked = ((solenoidCache & 0x0004) != 0) ? true : false;
                  this.NicbotInspectSensorArmDeployCheckBox.Checked = ((solenoidCache & 0x0008) != 0) ? true : false;
                  this.NicbotInspectFrontArmExtendCheckBox.Checked = ((solenoidCache & 0x4000) != 0) ? true : false;
                  this.NicbotInspectFrontArmRetractCheckBox.Checked = ((solenoidCache & 0x8000) != 0) ? true : false;
                  this.NicbotInspectRearArmExtendCheckBox.Checked = ((solenoidCache & 0x1000) != 0) ? true : false;
                  this.NicbotInspectRearArmRetractCheckBox.Checked = ((solenoidCache & 0x2000) != 0) ? true : false;
                  this.NicbotInspectLowerArmsRetractCheckBox.Checked = ((solenoidCache & 0x0010) != 0) ? true : false;
                  this.NicbotInspectLowerArmsExtendCheckBox.Checked = ((solenoidCache & 0x0020) != 0) ? true : false;
                  this.NicbotInspectWheelCircumfernceCheckBox.Checked = ((solenoidCache & 0x0040) != 0) ? true : false;
                  this.NicbotInspectWheelAxialCheckBox.Checked = ((solenoidCache & 0x0080) != 0) ? true : false;
                  this.NicbotInspectWheelLockCheckBox.Checked = ((solenoidCache & 0x0800) != 0) ? true : false;
                  this.nicbotBodyAssigningSolenoidValues = false;
               }
               else
               {
                  this.NicbotBodyRepairGroupBox.Visible = false;
                  this.NicbotBodyInspectGroupBox.Visible = false;
               }
            }
            else
            {
               this.StatusLabel.Text = "Unable to configure NICBOT body.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void NicbotStartButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            
            bool result = this.nicbotBody.Start();
            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT body started.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to start NICBOT body.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void NicbotStopButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            this.nicbotBody.Stop();
            this.StatusLabel.Text = "NICBOT body stopped.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void NicbotResetButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            this.nicbotBody.Reset();
            this.StatusLabel.Text = "NICBOT body reset.";

            this.NicbotBodyRepairGroupBox.Visible = false;
            this.NicbotBodyInspectGroupBox.Visible = false;
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetNicbotBaudButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            int deviceRate = this.nicbotBody.GetDeviceBaudRate();

            if (0 != deviceRate)
            {
               this.StatusLabel.Text = "NICBOT baudrate retrieved.";
               this.NicbotBodyBaudComboBox.Text = deviceRate.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get NICBOT baudrate.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotBaudButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         int deviceRate = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (int.TryParse(this.NicbotBodyBaudComboBox.Text, out deviceRate) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetDeviceBaudRate(deviceRate);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT baudrate set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT baudrate.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetNicbotNodeIdButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            byte deviceNodeId = 0;
            bool success = this.nicbotBody.GetDeviceNodeId(ref deviceNodeId);

            if (false != success)
            {
               this.StatusLabel.Text = "NICBOT node ID retrieved.";
               this.NicbotBodyNodeIdTextBox.Text = deviceNodeId.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get NICBOT node ID.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotNodeIdButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         byte deviceNodeId = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (byte.TryParse(this.NicbotBodyNodeIdTextBox.Text, out deviceNodeId) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetDeviceNodeId(deviceNodeId);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT node ID set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT node ID.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetNicbotModeButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            byte deviceMode = 0;
            bool success = this.nicbotBody.GetDeviceMode(ref deviceMode);

            if (false != success)
            {
               this.StatusLabel.Text = "NICBOT mode retrieved.";
               this.NicbotBodyModeTextBox.Text = deviceMode.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get NICBOT mode.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotModeButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         byte deviceMode = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (byte.TryParse(this.NicbotBodyModeTextBox.Text, out deviceMode) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetDeviceMode(deviceMode);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT mode set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT mode.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SaveNicbotConfigurationButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SaveConfiguration();

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT configuration saved.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to save NICBOT configuration.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #region Video Control

      private void SetNicbotVideoAButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         byte videoSelect = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (byte.TryParse(this.NicbotVideoATextBox.Text, out videoSelect) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetVideoASelect(videoSelect);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT video A set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT video A.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotVideoBButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         byte videoSelect = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (byte.TryParse(this.NicbotVideoBTextBox.Text, out videoSelect) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetVideoBSelect(videoSelect);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT video B set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT video B.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotLightLevelButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         byte cameraSelect = 0;
         byte lightLevel = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (byte.TryParse(this.NicbotLightLevelCameraSelectTextBox.Text, out cameraSelect) != false) &&
             (byte.TryParse(this.NicbotLightLevelTextBox.Text, out lightLevel) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetCameraLightLevelt(cameraSelect, lightLevel);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT camera light set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT camera light.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotLightOnButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         byte cameraSelect = 0;
         byte lightLevel = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (byte.TryParse(this.NicbotLightLevelCameraSelectTextBox.Text, out cameraSelect) != false) &&
             (byte.TryParse(this.NicbotLightLevelTextBox.Text, out lightLevel) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetCameraLightLevelt(cameraSelect, lightLevel);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT camera light set on.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT camera light on.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotLightOffButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         byte cameraSelect = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (byte.TryParse(this.NicbotLightLevelCameraSelectTextBox.Text, out cameraSelect) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetCameraLightLevelt(cameraSelect, 0);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT camera light set off.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT camera light off.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void IncreaseNicbotLightLevelButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         byte cameraSelect = 0;
         byte lightLevel = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (byte.TryParse(this.NicbotLightLevelCameraSelectTextBox.Text, out cameraSelect) != false) &&
             (byte.TryParse(this.NicbotLightLevelTextBox.Text, out lightLevel) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            lightLevel = (byte)((lightLevel <= 247) ? (lightLevel + 8) : 255);
            bool result = this.nicbotBody.SetCameraLightLevelt(cameraSelect, lightLevel);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT camera light increased.";
               this.NicbotLightLevelTextBox.Text = lightLevel.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to increase NICBOT camera light.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void DecreaseNicbotLightLevelButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         byte cameraSelect = 0;
         byte lightLevel = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (byte.TryParse(this.NicbotLightLevelCameraSelectTextBox.Text, out cameraSelect) != false) &&
             (byte.TryParse(this.NicbotLightLevelTextBox.Text, out lightLevel) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            lightLevel = (byte)((lightLevel >= 8) ? (lightLevel - 8) : 0);
            bool result = this.nicbotBody.SetCameraLightLevelt(cameraSelect, lightLevel);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT camera light decreased.";
               this.NicbotLightLevelTextBox.Text = lightLevel.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to decrease NICBOT camera light.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #region Supervision Control 

      private void SetNicbotConsumerHeartbeatButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         byte consumerHeartbeatNodeId = 0;
         UInt16 consumerHeartbeatTime = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (byte.TryParse(this.NicbotConsumerHeartbeatNodeIdTextBox.Text, out consumerHeartbeatNodeId) != false) &&
             (UInt16.TryParse(this.NicbotConsumerHeartbeatTimeTextBox.Text, out consumerHeartbeatTime) != false))
         {
            this.nicbotBody.NodeId = nodeId;
            bool result = this.nicbotBody.SetConsumerHeartbeat(consumerHeartbeatTime, consumerHeartbeatNodeId);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT consumer heartbeat set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT consumer heartbeat.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotProducerHeartbeatButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt16 producerHeartbeatTime = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt16.TryParse(this.NicbotProducerHeartbeatTimeTextBox.Text, out producerHeartbeatTime) != false))
         {
            this.nicbotBody.NodeId = nodeId;
            bool result = this.nicbotBody.SetProducerHeartbeat(producerHeartbeatTime);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT producer heartbeat set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT producer heartbeat.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #region Repair

      #region Solenoid Control

      private void NicbotRepairFrontDrillCoverCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateNicbotSolenoidSelect(2, this.NicbotRepairFrontDrillCoverCheckBox.Checked);
      }

      private void NicbotRepairFrontNozzleCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateNicbotSolenoidSelect(14, this.NicbotRepairFrontNozzleCheckBox.Checked);
      }

      private void NicbotRepairRearDrillCoverCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateNicbotSolenoidSelect(3, this.NicbotRepairRearDrillCoverCheckBox.Checked);
      }

      private void NicbotRepairRearNozzleCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateNicbotSolenoidSelect(15, this.NicbotRepairRearNozzleCheckBox.Checked);
      }

      private void NicbotRepairFrontArmExtendCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateNicbotSolenoidSelect(13, this.NicbotRepairFrontArmExtendCheckBox.Checked);
      }

      private void NicbotRepairFrontArmRetractCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateNicbotSolenoidSelect(12, this.NicbotRepairFrontArmRetractCheckBox.Checked);
      }

      private void NicbotRepairRearArmExtendCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateNicbotSolenoidSelect(6, this.NicbotRepairRearArmExtendCheckBox.Checked);
      }

      private void NicbotRepairRearArmRetractCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateNicbotSolenoidSelect(7, this.NicbotRepairRearArmRetractCheckBox.Checked);
      }

      private void NicbotRepairLowerArmExtendCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateNicbotSolenoidSelect(0, this.NicbotRepairLowerArmExtendCheckBox.Checked);
      }

      private void NicbotRepairLowerArmRetractCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateNicbotSolenoidSelect(1, this.NicbotRepairLowerArmRetractCheckBox.Checked);
      }

      private void NicbotRepairWheelCircumfernceCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateNicbotSolenoidSelect(4, this.NicbotRepairWheelCircumfernceCheckBox.Checked);
      }

      private void NicbotRepairWheelAxialCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateNicbotSolenoidSelect(5, this.NicbotRepairWheelAxialCheckBox.Checked);
      }

      private void NicbotRepairWheelLockCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateNicbotSolenoidSelect(11, this.NicbotRepairWheelLockCheckBox.Checked);
      }

      #endregion

      #region Drill Control

      private void NicbotRepairFrontLaserOnButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetFrontLaser(true);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT front laser activated.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to activate NICBOT front laser.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void NicbotRepairFrontLaserOffButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetFrontLaser(false);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT front laser deactivated.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to deactivate NICBOT front laser.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void NicbotRepairRearLaserOnButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetRearLaser(true);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT rear laser activated.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to activate NICBOT rear laser.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void NicbotRepairRearLaserOffButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetRearLaser(false);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT rear laser deactivated.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to deactivate NICBOT rear laser.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotFrontDrillSpeedButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         Int16 drillSpeed = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (Int16.TryParse(this.NicbotFrontDrillSpeedTextBox.Text, out drillSpeed) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetFrontDrillSpeed(drillSpeed);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT front drill speed set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT front drill speed.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotFrontDrillIndexButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         double drillIndex = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (double.TryParse(this.NicbotFrontDrillIndexTextBox.Text, out drillIndex) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetFrontDrillIndex(drillIndex);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT front drill index set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT front drill index.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotRepairFrontRetractButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetFrontDrillRetractToLimit();

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT front drill retract to limit set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT front drill retract to limit.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotRepairFrontOriginButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetFrontDrillMoveToOrigin();

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT front drill move to origin set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT front drill move to origin.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotRepairFrontStopButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.StopFrontDrill();

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT front drill stopped.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to stop NICBOT front drill.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetNicbotRepairFrontServoStatusButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            byte servoStatus = 0;
            bool success = this.nicbotBody.GetFrontDrillServoStatus(ref servoStatus);

            if (false != success)
            {
               this.StatusLabel.Text = "NICBOT front drill servo status retrieved.";
               this.NicbotRepairFrontDrilServoStatusTextBox.Text = string.Format("{0:X2}", servoStatus);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get NICBOT front drill servo status.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotRearDrillSpeedButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         Int16 drillSpeed = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (Int16.TryParse(this.NicbotRearDrillSpeedTextBox.Text, out drillSpeed) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetRearDrillSpeed(drillSpeed);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT rear drill speed set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT rear drill speed.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotRearDrillIndexButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         double drillIndex = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (double.TryParse(this.NicbotRearDrillIndexTextBox.Text, out drillIndex) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetRearDrillIndex(drillIndex);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT rear drill index set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT rear drill index.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotRepairRearRetractButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetRearDrillRetractToLimit();

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT rear drill retract to limit set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT rear drill retract to limit.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotRepairRearOriginButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetRearDrillMoveToOrigin();

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT rear drill move to origin set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT rear drill move to origin.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotRepairRearStopButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.StopRearDrill();

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT rear drill stopped.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to stop NICBOT rear drill.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetNicbotRepairRearServoStatusButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            byte servoStatus = 0;
            bool success = this.nicbotBody.GetRearDrillServoStatus(ref servoStatus);

            if (false != success)
            {
               this.StatusLabel.Text = "NICBOT rear drill servo status retrieved.";
               this.NicbotRepairRearDrilServoStatusTextBox.Text = string.Format("{0:X2}", servoStatus);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get NICBOT rear drill servo status.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetNicbotRepairPulsesPerUnitbutton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            UInt32 pulsesPerUnit = 0;
            bool success = this.nicbotBody.GetDrillServoPulsesPerUnit(ref pulsesPerUnit);

            if (false != success)
            {
               this.StatusLabel.Text = "NICBOT drill servo pulses per unit retrieved.";
               this.NicbotRepairPulsesPerUnitTextBox.Text = pulsesPerUnit.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get NICBOT drill servo pulses per unit.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotRepairPulsesPerUnitbutton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         UInt32 pulsesPerUnit = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (UInt32.TryParse(this.NicbotRepairPulsesPerUnitTextBox.Text, out pulsesPerUnit) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetDrillServoPulsesPerUnit(pulsesPerUnit);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT drill servo pulses per unit set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT drill servo pulses per unit.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetNicbotRepairServoErrorLimitButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            UInt16 errorLimit = 0;
            bool success = this.nicbotBody.GetDrillServoErrorLimit(ref errorLimit);

            if (false != success)
            {
               this.StatusLabel.Text = "NICBOT drill servo error limit retrieved.";
               this.NicbotRepairServoErrorLimitTextBox.Text = errorLimit.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get NICBOT drill servo error limit.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotRepairServoErrorLimitButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         UInt16 errorLimit = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (UInt16.TryParse(this.NicbotRepairServoErrorLimitTextBox.Text, out errorLimit) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetDrillServoErrorLimit(errorLimit);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT drill servo error limit set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT drill servo error limit.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetNicbotRepairServoAccelerationButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            UInt32 acceleration = 0;
            bool success = this.nicbotBody.GetDrillServoAcceleration(ref acceleration);

            if (false != success)
            {
               this.StatusLabel.Text = "NICBOT drill servo acceleration retrieved.";
               this.NicbotRepairServoAccelerationTextBox.Text = acceleration.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get NICBOT drill servo acceleration.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotRepairServoAccelerationButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         UInt32 acceleration = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (UInt32.TryParse(this.NicbotRepairServoAccelerationTextBox.Text, out acceleration) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetDrillServoAcceleration(acceleration);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT drill servo acceleration set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT drill servo acceleration.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetNicbotRepairServoKpButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            UInt32 servoKp = 0;
            bool success = this.nicbotBody.GetDrillServoProportionalControlConstant(ref servoKp);

            if (false != success)
            {
               this.StatusLabel.Text = "NICBOT drill servo KP retrieved.";
               this.NicbotRepairServoKpTextBox.Text = servoKp.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get NICBOT drill servo KP.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotRepairServoKpButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         UInt32 servoKp = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (UInt32.TryParse(this.NicbotRepairServoKpTextBox.Text, out servoKp) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetDrillServoProportionalControlConstant(servoKp);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT drill servo KP set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT drill servo KP.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetNicbotRepairServoKiButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            UInt32 servoKi = 0;
            bool success = this.nicbotBody.GetDrillServoIntegralControlConstant(ref servoKi);

            if (false != success)
            {
               this.StatusLabel.Text = "NICBOT drill servo KI retrieved.";
               this.NicbotRepairServoKiTextBox.Text = servoKi.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get NICBOT drill servo KI.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotRepairServoKiButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         UInt32 servoKi = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (UInt32.TryParse(this.NicbotRepairServoKiTextBox.Text, out servoKi) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetDrillServoIntegralControlConstant(servoKi);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT drill servo KI set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT drill servo KI.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetNicbotRepairServoKdButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            UInt32 servoKd = 0;
            bool success = this.nicbotBody.GetDrillServoDerivativeControlConstant(ref servoKd);

            if (false != success)
            {
               this.StatusLabel.Text = "NICBOT drill servo KD retrieved.";
               this.NicbotRepairServoKdTextBox.Text = servoKd.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get NICBOT drill servo KD.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotRepairServoKdButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         UInt32 servoKd = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (UInt32.TryParse(this.NicbotRepairServoKdTextBox.Text, out servoKd) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetDrillServoDerivativeControlConstant(servoKd);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT drill servo KD set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT drill servo KD.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetNicbotRepairServoHomingVelocityButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            UInt32 homingVelocity = 0;
            bool success = this.nicbotBody.GetDrillServoHomingVelocity(ref homingVelocity);

            if (false != success)
            {
               this.StatusLabel.Text = "NICBOT drill servo homing velocity.";
               this.NicbotRepairServoHomingVelocityTextBox.Text = homingVelocity.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get NICBOT drill servo homing velocity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotRepairServoHomingVelocityButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         UInt32 homingVelocity = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (UInt32.TryParse(this.NicbotRepairServoHomingVelocityTextBox.Text, out homingVelocity) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetDrillServoHomingVelocity(homingVelocity);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT drill servo homing velocity set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT drill servo homing velocity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetNicbotRepairServoHomeBackoffCountButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            UInt32 homebackoffCount = 0;
            bool success = this.nicbotBody.GetDrillServoHomingBackoffCount(ref homebackoffCount);

            if (false != success)
            {
               this.StatusLabel.Text = "NICBOT drill servo pulses home backoff count.";
               this.NicbotRepairServoHomeBackoffCountTextBox.Text = homebackoffCount.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get NICBOT drill servo home backoff count.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotRepairServoHomeBackoffCountButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         UInt32 homebackoffCount = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (UInt32.TryParse(this.NicbotRepairServoHomeBackoffCountTextBox.Text, out homebackoffCount) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetDrillServoHomingBackoffCount(homebackoffCount);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT drill servo pulses home backoff count.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT drill servo home backoff count.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetNicbotRepairServoTravelVelocityButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            UInt32 travelVelocity = 0;
            bool success = this.nicbotBody.GetDrillServoTravelVelocity(ref travelVelocity);

            if (false != success)
            {
               this.StatusLabel.Text = "NICBOT drill servo travel velocity.";
               this.NicbotRepairServoTravelVelocityTextBox.Text = travelVelocity.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get NICBOT drill servo travel velocity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotRepairServoTravelVelocityButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         UInt32 travelVelocity = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (UInt32.TryParse(this.NicbotRepairServoTravelVelocityTextBox.Text, out travelVelocity) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetDrillServoTravelVelocity(travelVelocity);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT drill servo pulses travel velocity.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT drill servo travel velocity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotAutoDrillParametersButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         double searchSpeed = 0;
         double travelSpeed = 0;
         double rotationSpeed = 0;
         double cuttingSpeed = 0;
         double cuttingDepth = 0;
         double peckIncrement = 0;
         double retractionDistance = 0;
         double retractionPosition = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (double.TryParse(this.NicbotAutoDrillSearchSpeedTextBox.Text, out searchSpeed) != false) &&
             (double.TryParse(this.NicbotAutoDrillTravelSpeedTextBox.Text, out travelSpeed) != false) &&
             (double.TryParse(this.NicbotAutoDrillRotationSpeedTextBox.Text, out rotationSpeed) != false) &&
             (double.TryParse(this.NicbotAutoDrillCuttingSpeedTextBox.Text, out cuttingSpeed) != false) &&
             (double.TryParse(this.NicbotAutoDrillCuttingDepthTextBox.Text, out cuttingDepth) != false) &&
             (double.TryParse(this.NicbotAutoDrillPeckIncrementTextBox.Text, out peckIncrement) != false) &&
             (double.TryParse(this.NicbotAutoDrillPeckRetractionDistanceTextBox.Text, out retractionDistance) != false) &&
             (double.TryParse(this.NicbotAutoDrillPeckRetractionPositionTextBox.Text, out retractionPosition) != false))
         {
            bool autoOrigin = this.NicbotAutoDrillOriginCheckBox.Checked;
            bool peckMode = this.NicbotAutoDrillPeckModeRadioButton.Checked;
            bool distancePosition = this.NicbotAutoDrillPositionRetractionRadioButton.Checked;

            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetAutoDrillParameters(autoOrigin, peckMode, distancePosition, searchSpeed, travelSpeed, rotationSpeed, cuttingSpeed, cuttingDepth, peckIncrement, retractionDistance, retractionPosition);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT auto drill parameters set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT auto drill parameters.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotOriginHuntOnButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetAutoDrillOriginHunt(true);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT origin hunt activated.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to activate NICBOT origin hunt.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotOriginHuntOffButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetAutoDrillOriginHunt(false);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT origin hunt deactivated.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to deactivate NICBOT origin hunt.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotAutoStartButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetAutoDrillRunning(true);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT drill auto cut activated.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to activate NICBOT drill auto cut.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotAutoActivityButton_Click(object sender, EventArgs e)
      {
#if false
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;

            if (false == this.nicbotBody.AutoDrillCutPaused)
            {
               bool result = this.nicbotBody.SetAutoDrillPause(true);

               if (false != result)
               {
                  this.StatusLabel.Text = "NICBOT drill auto cut paused.";
               }
               else
               {
                  this.StatusLabel.Text = "Unable to pause NICBOT drill auto cut.";
               }
            }
            else
            {
               bool result = this.nicbotBody.SetAutoDrillPause(false);

               if (false != result)
               {
                  this.StatusLabel.Text = "NICBOT drill auto cut resumed.";
               }
               else
               {
                  this.StatusLabel.Text = "Unable to resume NICBOT drill auto cut.";
               }
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
#endif
      }

      private void SetNicbotAutoStopButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetAutoDrillRunning(false);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT drill auto cut deactivated.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to deactivate NICBOT drill auto cut.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #endregion

      #region Inspection

      #region Solenoid Control

      private void NicbotInspectSensorRetractCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateNicbotSolenoidSelect(0, this.NicbotInspectSensorRetractCheckBox.Checked);
      }

      private void NicbotInspectSensorExtendCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateNicbotSolenoidSelect(1, this.NicbotInspectSensorExtendCheckBox.Checked);
      }

      private void NicbotInspectSensorArmStowCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateNicbotSolenoidSelect(2, this.NicbotInspectSensorArmStowCheckBox.Checked);
      }

      private void NicbotInspectSensorArmDeployCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateNicbotSolenoidSelect(3, this.NicbotInspectSensorArmDeployCheckBox.Checked);
      }

      private void NicbotInspectFrontArmExtendCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateNicbotSolenoidSelect(14, this.NicbotInspectFrontArmExtendCheckBox.Checked);
      }

      private void NicbotInspectFrontArmRetractCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateNicbotSolenoidSelect(15, this.NicbotInspectFrontArmRetractCheckBox.Checked);
      }

      private void NicbotInspectRearArmExtendCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateNicbotSolenoidSelect(12, this.NicbotInspectRearArmExtendCheckBox.Checked);
      }

      private void NicbotInspectRearArmRetractCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateNicbotSolenoidSelect(13, this.NicbotInspectRearArmRetractCheckBox.Checked);
      }

      private void NicbotInspectLowerArmsExtendCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateNicbotSolenoidSelect(5, this.NicbotInspectLowerArmsExtendCheckBox.Checked);
      }

      private void NicbotInspectLowerArmsRetractCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateNicbotSolenoidSelect(4, this.NicbotInspectLowerArmsRetractCheckBox.Checked);
      }

      private void NicbotInspectWheelCircumfernceCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateNicbotSolenoidSelect(6, this.NicbotInspectWheelCircumfernceCheckBox.Checked);
      }

      private void NicbotInspectWheelAxialCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateNicbotSolenoidSelect(7, this.NicbotInspectWheelAxialCheckBox.Checked);
      }

      private void NicbotInspectWheelLockCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateNicbotSolenoidSelect(11, this.NicbotInspectWheelLockCheckBox.Checked);
      }

      #endregion

      #region Sensor Control

      private void GetNicbotSensorServoStatusButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            byte servoStatus = 0;
            bool success = this.nicbotBody.GetSensorServoStatus(ref servoStatus);

            if (false != success)
            {
               this.StatusLabel.Text = "NICBOT sensor servo status retrieved.";
               this.NicbotSensorServoStatusTextBox.Text = string.Format("{0:X2}", servoStatus);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get NICBOT sensor servo status.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotSensorIndexButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         UInt16 sensorIndex = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (UInt16.TryParse(this.NicbotSensorIndexTextBox.Text, out sensorIndex) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetSensorIndex(sensorIndex);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT sensor index set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT sensor index.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void DisableNicbotSensorMotorButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetSensorMotorDisable();

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT sensor motor disabled.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to NICBOT disable sensor motor.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotSensorHomeButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetSensorHome();

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT sensor home set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT home index.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetNicbotSensorServoErrorLimitButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            UInt16 errorLimit = 0;
            bool success = this.nicbotBody.GetSensorServoErrorLimit(ref errorLimit);

            if (false != success)
            {
               this.StatusLabel.Text = "NICBOT sensor servo error limit retrieved.";
               this.NicbotSensorServoErrorLimitTextBox.Text = errorLimit.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get NICBOT sensor servo error limit.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotSensorServoErrorLimitButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         UInt16 errorLimit = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (UInt16.TryParse(this.NicbotSensorServoErrorLimitTextBox.Text, out errorLimit) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetSensorServoErrorLimit(errorLimit);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT sensor servo error limit set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT sensor servo error limit.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetNicbotSensorAccelerationButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            UInt32 acceleration = 0;
            bool success = this.nicbotBody.GetSensorAcceleration(ref acceleration);

            if (false != success)
            {
               this.StatusLabel.Text = "NICBOT sensor acceleration retrieved.";
               this.NicbotSensorAccelerationTextBox.Text = acceleration.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get NICBOT sensor acceleration.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotSensorAccelerationButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         UInt32 acceleration = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (UInt32.TryParse(this.NicbotSensorAccelerationTextBox.Text, out acceleration) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetSensorAcceleration(acceleration);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT sensor acceleration set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT sensor acceleration.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetNicbotSensorHomingVelocityButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            UInt32 velocity = 0;
            bool success = this.nicbotBody.GetSensorHomingVelocity(ref velocity);

            if (false != success)
            {
               this.StatusLabel.Text = "NICBOT sensor homing velocity retrieved.";
               this.NicbotSensorHomingVelocityTextBox.Text = velocity.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get NICBOT sensor homing velocity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotSensorHomingVelocityButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         UInt32 velocity = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (UInt32.TryParse(this.NicbotSensorHomingVelocityTextBox.Text, out velocity) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetSensorHomingVelocity(velocity);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT sensor homing velocity set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT sensor homing velocity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetNicbotSensorHomingBackoffButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            UInt32 backoff = 0;
            bool success = this.nicbotBody.GetSensorHomingBackoff(ref backoff);

            if (false != success)
            {
               this.StatusLabel.Text = "NICBOT sensor homing backoff retrieved.";
               this.NicbotSensorHomingBackoffTextBox.Text = backoff.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get NICBOT sensor homing backoff.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotSensorHomingBackoffButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         UInt32 backoff = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (UInt32.TryParse(this.NicbotSensorHomingBackoffTextBox.Text, out backoff) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetSensorHomingBackoff(backoff);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT sensor homing backoff set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT sensor homing backoff.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetNicbotSensorTravelVelocityButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            UInt32 velocity = 0;
            bool success = this.nicbotBody.GetSensorTravelVelocity(ref velocity);

            if (false != success)
            {
               this.StatusLabel.Text = "NICBOT sensor travel velocity retrieved.";
               this.NicbotSensorTravelVelocityTextBox.Text = velocity.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get NICBOT sensor travel velocity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotSensorTravelVelocityButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         UInt32 velocity = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (UInt32.TryParse(this.NicbotSensorTravelVelocityTextBox.Text, out velocity) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetSensorTravelVelocity(velocity);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT sensor travel velocity set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT sensor travel velocity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetNicbotSensorPulsesPerDegreeButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;

         if (byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false)
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            UInt32 pulsesPerDegree = 0;
            bool success = this.nicbotBody.GetSensorServoPulsePerDegree(ref pulsesPerDegree);

            if (false != success)
            {
               this.StatusLabel.Text = "NICBOT sensor pulses per degree retrieved.";
               this.NicbotSensorPulsesPerDegeeTextBox.Text = pulsesPerDegree.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get NICBOT sensor pulses per degree.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotSensorPulsesPerDegreeButton_Click(object sender, EventArgs e)
      {
         byte nicbotNodeId = 0;
         UInt32 pulsesPerDegree = 0;

         if ((byte.TryParse(this.NicbotBodyActiveNodeIdTextBox.Text, out nicbotNodeId) != false) &&
             (UInt32.TryParse(this.NicbotSensorPulsesPerDegeeTextBox.Text, out pulsesPerDegree) != false))
         {
            this.nicbotBody.NodeId = nicbotNodeId;
            bool result = this.nicbotBody.SetSensorServoPulsePerDegree(pulsesPerDegree);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT sensor pulses per degree set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT sensor pulses per degree.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #endregion

      #endregion

      #region NICBOT Wheel Events

      private void NicbotWheelConfigButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.NicbotWheelActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.nicbotWheel.NodeId = nodeId;
            bool result = this.nicbotWheel.Configure();

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT wheel configured.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to configure NICBOT wheel.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void NicbotWheelStartButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.NicbotWheelActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.nicbotWheel.NodeId = nodeId;
            bool result = this.nicbotWheel.Start();

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT wheel started.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to start NICBOT wheel.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void NicbotWheelStopButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.NicbotWheelActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.nicbotWheel.NodeId = nodeId;
            this.nicbotWheel.Stop();
            this.StatusLabel.Text = "NICBOT wheel stopped.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void NicbotWheelResetButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.NicbotWheelActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.nicbotWheel.NodeId = nodeId;
            this.nicbotWheel.Reset();
            this.StatusLabel.Text = "NICBOT wheel reset.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetNicbotWheelBaudButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.NicbotWheelActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.nicbotWheel.NodeId = nodeId;
            int deviceRate = this.nicbotWheel.GetDeviceBaudRate();

            if (0 != deviceRate)
            {
               this.StatusLabel.Text = "NICBOT wheel baudrate retrieved.";
               this.NicbotWheelBaudComboBox.Text = deviceRate.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get NICBOT wheel baudrate.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotWheelBaudButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         int deviceRate = 0;

         if ((byte.TryParse(this.NicbotWheelActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (int.TryParse(this.NicbotWheelBaudComboBox.Text, out deviceRate) != false))
         {
            this.nicbotWheel.NodeId = nodeId;
            bool result = this.nicbotWheel.SetDeviceBaudRate(deviceRate);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT wheel baudrate set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT wheel baudrate.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetNicbotWheelNodeButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.NicbotWheelActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.nicbotWheel.NodeId = nodeId;
            byte deviceNodeId = 0;
            bool success = this.nicbotWheel.GetDeviceNodeId(ref deviceNodeId);

            if (false != success)
            {
               this.StatusLabel.Text = "NICBOT wheel node ID retrieved.";
               this.NicbotWheelNodeIdTextBox.Text = deviceNodeId.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get NICBOT wheel node ID.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotWheelNodeButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         byte deviceNodeId = 0;

         if ((byte.TryParse(this.NicbotWheelActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (byte.TryParse(this.NicbotWheelNodeIdTextBox.Text, out deviceNodeId) != false))
         {
            this.nicbotWheel.NodeId = nodeId;
            bool result = this.nicbotWheel.SetDeviceNodeId(deviceNodeId);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT wheel node ID set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT wheel node ID.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetNicbotWheelOffsetButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.NicbotWheelActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.nicbotWheel.NodeId = nodeId;
            byte deviceOffset = 0;
            bool success = this.nicbotWheel.GetDeviceNodeOffset(ref deviceOffset);

            if (false != success)
            {
               this.StatusLabel.Text = "NICBOT wheel node offset retrieved.";
               this.NicbotWheelOffsetTextBox.Text = deviceOffset.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get NICBOT wheel offset.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotWheelOffsetButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         byte deviceOffset = 0;

         if ((byte.TryParse(this.NicbotWheelActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (byte.TryParse(this.NicbotWheelOffsetTextBox.Text, out deviceOffset) != false))
         {
            this.nicbotWheel.NodeId = nodeId;
            bool result = this.nicbotWheel.SetDeviceNodeOffset(deviceOffset);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT wheel offset set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT offset ID.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SaveNicbotWheelConfigurationButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.NicbotWheelActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.nicbotWheel.NodeId = nodeId;
            bool result = this.nicbotWheel.SaveConfiguration();

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT wheel configuration saved.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to save NICBOT wheel configuration.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotWheelConsumerHeartbeatButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         byte consumerHeartbeatNodeId = 0;
         UInt16 consumerHeartbeatTime = 0;

         if ((byte.TryParse(this.NicbotWheelActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (byte.TryParse(this.NicbotWheelConsumerHeartbeatNodeIdTextBox.Text, out consumerHeartbeatNodeId) != false) &&
             (UInt16.TryParse(this.NicbotWheelConsumerHeartbeatTimeTextBox.Text, out consumerHeartbeatTime) != false))
         {
            this.nicbotWheel.NodeId = nodeId;
            bool result = this.nicbotWheel.SetConsumerHeartbeat(consumerHeartbeatTime, consumerHeartbeatNodeId);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT wheel consumer heartbeat set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT wheel consumer heartbeat.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotWheelProducerHeartbeatButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt16 producerHeartbeatTime = 0;

         if ((byte.TryParse(this.NicbotWheelActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt16.TryParse(this.NicbotWheelProducerHeartbeatTimeTextBox.Text, out producerHeartbeatTime) != false))
         {
            this.nicbotWheel.NodeId = nodeId;
            bool result = this.nicbotWheel.SetProducerHeartbeat(producerHeartbeatTime);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT wheel producer heartbeat set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT wheel producer heartbeat.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotWheelModeButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UlcRoboticsNicbotWheel.Modes mode = UlcRoboticsNicbotWheel.Modes.undefined;

         if (this.NicbotWheelModeComboBox.Text == "off")
         {
            mode = UlcRoboticsNicbotWheel.Modes.off;
         }
         else if (this.NicbotWheelModeComboBox.Text == "velocity")
         {
            mode = UlcRoboticsNicbotWheel.Modes.velocity;
         }
         else if (this.NicbotWheelModeComboBox.Text == "current")
         {
            mode = UlcRoboticsNicbotWheel.Modes.current;
         }

         if (byte.TryParse(this.NicbotWheelActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.nicbotWheel.NodeId = nodeId;
            bool result = this.nicbotWheel.SetMode(mode);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT wheel mode set to " + mode.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT wheel mode to " + mode.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotWheelVelocityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 velocity = 0;

         if ((byte.TryParse(this.NicbotWheelActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.NicbotWheelVelocityTextBox.Text, out velocity) != false))
         {
            this.nicbotWheel.NodeId = nodeId;
            bool result = this.nicbotWheel.SetVelocity(velocity);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT wheel velocity set to " + velocity.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT wheel velocity to " + velocity.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void ScheduleNicbotWheelVelocityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 velocity = 0;

         if ((byte.TryParse(this.NicbotWheelActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.NicbotWheelVelocityTextBox.Text, out velocity) != false))
         {
            this.nicbotWheel.NodeId = nodeId;
            bool result = this.nicbotWheel.ScheduleVelocity(velocity);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT wheel velocity scheduled for " + velocity.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to schedule NICBOT wheel velocity for " + velocity.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetNicbotWheelTorqueButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         float torque = 0;

         if ((byte.TryParse(this.NicbotWheelActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (float.TryParse(this.NicbotWheelTorqueTextBox.Text, out torque) != false))
         {
            this.nicbotWheel.NodeId = nodeId;
            bool result = this.nicbotWheel.SetTorque(torque);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT wheel torque set to " + torque.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set NICBOT wheel torque to " + torque.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void ScheduleNicbotWheelTorqueButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         float torque = 0;

         if ((byte.TryParse(this.ActiveMotorNodeIdTextBox.Text, out nodeId) != false) &&
             (float.TryParse(this.NicbotWheelTorqueTextBox.Text, out torque) != false))
         {
            this.nicbotWheel.NodeId = nodeId;
            bool result = this.nicbotWheel.ScheduleTorque(torque);

            if (false != result)
            {
               this.StatusLabel.Text = "NICBOT wheel torque schedule for " + torque.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to schedule NICBOT wheel torque for " + torque.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #region RS232 Events

      private void Rs232StartButton_Click(object sender, EventArgs e)
      {
         byte rs232NodeId = 0;

         if (byte.TryParse(this.ActiveRs232NodeIdTextBox.Text, out rs232NodeId) != false)
         {
            this.rs232.NodeId = rs232NodeId;
            this.rs232.Start();
            this.StatusLabel.Text = "RS232 started.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void Rs232StopButton_Click(object sender, EventArgs e)
      {
         byte rs232NodeId = 0;

         if (byte.TryParse(this.ActiveRs232NodeIdTextBox.Text, out rs232NodeId) != false)
         {
            this.rs232.NodeId = rs232NodeId;
            this.rs232.Stop();
            this.StatusLabel.Text = "RS232 stopped.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void Rs232ResetButton_Click(object sender, EventArgs e)
      {
         byte rs232NodeId = 0;

         if (byte.TryParse(this.ActiveRs232NodeIdTextBox.Text, out rs232NodeId) != false)
         {
            this.rs232.NodeId = rs232NodeId;
            this.rs232.Reset();
            this.StatusLabel.Text = "RS232 reset.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetRs232BitRateButton_Click(object sender, EventArgs e)
      {
         byte rs232NodeId = 0;

         if (byte.TryParse(this.ActiveRs232NodeIdTextBox.Text, out rs232NodeId) != false)
         {
            this.rs232.NodeId = rs232NodeId;
            int deviceRate = this.rs232.GetDeviceBaudRate();

            if (0 != deviceRate)
            {
               this.StatusLabel.Text = "RS232 baudrate retrieved.";
               this.Rs232BaudComboBox.Text = deviceRate.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get RS232 baudrate.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetRs232BitRateButton_Click(object sender, EventArgs e)
      {
         byte rs232NodeId = 0;
         int deviceRate = 0;

         if ((byte.TryParse(this.ActiveRs232NodeIdTextBox.Text, out rs232NodeId) != false) &&
             (int.TryParse(this.Rs232BaudComboBox.Text, out deviceRate) != false))
         {
            this.rs232.NodeId = rs232NodeId;
            bool result = this.rs232.SetDeviceBaudRate(deviceRate);

            if (false != result)
            {
               this.StatusLabel.Text = "RS232 baudrate set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set RS232 baudrate.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetRs232NodeIdButton_Click(object sender, EventArgs e)
      {
         byte rs232NodeId = 0;

         if (byte.TryParse(this.ActiveRs232NodeIdTextBox.Text, out rs232NodeId) != false)
         {
            this.rs232.NodeId = rs232NodeId;
            byte deviceNodeId = 0;
            bool result = this.rs232.GetDeviceNodeId(ref deviceNodeId);

            if (false != result)
            {
               this.StatusLabel.Text = "RS232 node ID retrieved.";
               this.Rs232NodeIdTextBox.Text = deviceNodeId.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get RS232 node ID.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetRs232NodeIdButton_Click(object sender, EventArgs e)
      {
         byte rs232NodeId = 0;
         byte deviceNodeId = 0;

         if ((byte.TryParse(this.ActiveRs232NodeIdTextBox.Text, out rs232NodeId) != false) &&
             (byte.TryParse(this.Rs232NodeIdTextBox.Text, out deviceNodeId) != false))
         {
            this.rs232.NodeId = rs232NodeId;
            bool result = this.rs232.SetDeviceNodeId(deviceNodeId);

            if (false != result)
            {
               this.StatusLabel.Text = "RS232 node ID set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set RS232 node ID.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetRs232NodeOffsetButton_Click(object sender, EventArgs e)
      {
         byte rs232NodeId = 0;

         if (byte.TryParse(this.ActiveRs232NodeIdTextBox.Text, out rs232NodeId) != false)
         {
            this.rs232.NodeId = rs232NodeId;
            byte deviceNodeOffset = 0;
            bool result = this.rs232.GetDeviceNodeOffset(ref deviceNodeOffset);

            if (false != result)
            {
               this.StatusLabel.Text = "RS232 node offset retrieved.";
               this.Rs232NodeOffsetTextBox.Text = deviceNodeOffset.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to get RS232 node offset.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetRs232NodeOffsetButton_Click(object sender, EventArgs e)
      {
         byte rs232NodeId = 0;
         byte deviceNodeOffset = 0;

         if ((byte.TryParse(this.ActiveRs232NodeIdTextBox.Text, out rs232NodeId) != false) &&
             (byte.TryParse(this.Rs232NodeOffsetTextBox.Text, out deviceNodeOffset) != false))
         {
            this.rs232.NodeId = rs232NodeId;
            bool result = this.rs232.SetDeviceNodeOffset(deviceNodeOffset);

            if (false != result)
            {
               this.StatusLabel.Text = "RS232 node offset set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set RS232 node offset.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SaveRs232ConfigurationButton_Click(object sender, EventArgs e)
      {
         byte rs232NodeId = 0;

         if (byte.TryParse(this.ActiveRs232NodeIdTextBox.Text, out rs232NodeId) != false)
         {
            this.rs232.NodeId = rs232NodeId;
            bool result = this.rs232.SaveConfiguration();

            if (false != result)
            {
               this.StatusLabel.Text = "RS232 configuration saved.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to save RS232 configuration.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void Rs232CommandSampleButton_Click(object sender, EventArgs e)
      {
         byte[] buffer = Encoding.UTF8.GetBytes("Q\r\n");
         this.rs232.WriteSerial(buffer, 0, buffer.Length);
      }

      private void Rs232CommRichTextBox_KeyPress(object sender, KeyPressEventArgs e)
      {
         byte[] buffer = new byte[1] { (byte)e.KeyChar };
         this.rs232.WriteSerial(buffer, 0, 1);
         e.Handled = true;
      }

      private void Rs232CommRichTextBox_KeyDown(object sender, KeyEventArgs e)
      {
         if (e.KeyCode == Keys.Return)
         {
            byte[] buffer = new byte[1] { (byte)'\r' };
            this.rs232.WriteSerial(buffer, 0, 1);

            e.SuppressKeyPress = true;
            e.Handled = true;
         }
      }

      private void Rs232StartWithSerialButton_Click(object sender, EventArgs e)
      {
         byte rs232NodeId = 0;
         UInt32 serialBaud = 0;
         byte serialData = 0;
         byte serialParity = 0;
         byte serialStop = 0;

         if ((byte.TryParse(this.ActiveRs232NodeIdTextBox.Text, out rs232NodeId) != false) &&
             (UInt32.TryParse(this.Rs232SerialBaudTextBox.Text, out serialBaud) != false) &&
             (byte.TryParse(this.Rs232SerialDataTextBox.Text, out serialData) != false) &&
             (byte.TryParse(this.Rs232SerialParityTextBox.Text, out serialParity) != false) &&
             (byte.TryParse(this.Rs232SerialStopTextBox.Text, out serialStop) != false))
         {
            this.rs232.NodeId = rs232NodeId;
            this.rs232.Start(serialBaud, serialData, serialParity, serialStop);
            this.StatusLabel.Text = "RS232 started.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #region Form Events

      private void MainForm_Shown(object sender, EventArgs e)
      {
         this.LoadRegistry();
         this.UpdateTimer.Enabled = true;
         this.NicbotBodyRepairGroupBox.Visible = false;
         this.NicbotBodyInspectGroupBox.Visible = false;
      }

      private void MainForm_Resize(object sender, EventArgs e)
      {
         int activityHeight = this.ClientSize.Height - this.MainTabControl.Height- this.MainStatusStrip.Height;
         this.MainActivityPanel.Height = activityHeight;
      }

      private void UpdateTimer_Tick(object sender, EventArgs e)
      {
         #region Clock Update

         DateTime now = DateTime.Now;
         string timeText = string.Format("{0:D2}:{1:D2}:{2:D2}.{3}", now.Hour, now.Minute, now.Second, (now.Millisecond/100));
         this.TimeStatusLabel.Text = timeText;

         #endregion

         #region Heartbeat Generation

         if ((false != this.heartbeatActive) &&
             (DateTime.Now > this.heartbeatTimeLimit))
         {
            this.heartbeatTimeLimit = this.heartbeatTimeLimit.AddMilliseconds(this.heartbeatTime);

            int cobId = (int)(((int)COBTypes.ERROR << 7) | (this.heartbeatNodeId & 0x7F));
            byte[] heartbeatMsg = new byte[1];
            heartbeatMsg[0] = 0x05;
            this.busInterface.Transmit(cobId, heartbeatMsg);
         }

         #endregion

         #region Trace Activity Update

         while (0 != this.traceQueue.Count)
         {
            string activityString = (string)this.traceQueue.Dequeue();
            this.ActivityRichTextBox.AppendText(activityString + "\n");
         }

         #endregion

         #region Digital IO Updates

         this.DigitalIoInputsTextBox.Text = this.digitalIoInputs.ToString("X2");

         #endregion

         #region Analog IO Updates

         this.AnalogIoInput0TextBox.Text = this.analogIo.AnalogIn0.ToString();
         this.AnalogIoInput1TextBox.Text = this.analogIo.AnalogIn1.ToString();
         this.AnalogIoInput2TextBox.Text = this.analogIo.AnalogIn2.ToString();
         this.AnalogIoInput3TextBox.Text = this.analogIo.AnalogIn3.ToString();
         this.AnalogIoInput4TextBox.Text = this.analogIo.AnalogIn4.ToString();
         this.AnalogIoInput5TextBox.Text = this.analogIo.AnalogIn5.ToString();
         this.AnalogIoInput6TextBox.Text = this.analogIo.AnalogIn6.ToString();
         this.AnalogIoInput7TextBox.Text = this.analogIo.AnalogIn7.ToString();

         #endregion

         #region Encoder Update

         this.DevicePositionTextBox.Text = this.encoder.Position.ToString();
         this.DeviceSpeedTextBox.Text = string.Format("{0:0.0}", this.encoder.Speed);
         this.DeviceRotationTextBox.Text = string.Format("{0:0.000}", this.encoder.Rotations);

         #endregion

         #region Motor Update

         this.MotorActualVelocityTextBox.Text = this.motor.RPM.ToString();
         this.MotorActualTorqueTextBox.Text = this.motor.Torque.ToString("N3");
         this.MotorDigitalInputsTextBox.Text = this.motorInputs.ToString("X2");

         #endregion

         #region Camera Update

         if ((false != this.active) &&
             (false != this.CameraAutoSetCheckBox.Checked) &&
             (DateTime.Now > this.cameraAutoSendTimeLimit))
         {
            byte cameraNodeId = 0;

            if (byte.TryParse(this.CameraActiveNodeIdTextBox.Text, out cameraNodeId) != false)
            {
               byte intensity = (byte)this.CameraLightIntensityTrackBar.Value;
               this.camera.NodeId = cameraNodeId;
               this.camera.SetLightIntensity(intensity);
            }

            this.cameraAutoSendTimeLimit = this.cameraAutoSendTimeLimit.AddSeconds(1);
         }

         #endregion

         #region GPS Update

         this.GpsAntennaTextBox.Text = (false != this.gps.Antenna) ? "1" : "0";
         this.GpsSatellitesTextBox.Text = this.gps.Satellites.ToString();

         double latitude = this.gps.Latitude;
         if (double.IsNaN(latitude) == false)
         {
            if (false != this.GpsDegreesRadioButton.Checked)
            {
               this.GpsLatitudeTextBox.Text = string.Format("{0:0.000000}", latitude);
            }
            else
            {
               char direction = (latitude > 0) ? 'N' : 'S';
               double value = Math.Abs(latitude);
               int degrees = (int)value;
               value -= degrees;
               value *= 60;
               int minutes = (int)(value);
               value -= minutes;
               value *= 60;
               int seconds = (int)value;
               this.GpsLatitudeTextBox.Text = string.Format("{0}° {1}' {2}\" {3}", degrees, minutes, seconds, direction);
            }
         }
         else
         {
            this.GpsLatitudeTextBox.Text = "";
         }

         double longitude = this.gps.Longitude;
         if (double.IsNaN(longitude) == false)
         {
            if (false != this.GpsDegreesRadioButton.Checked)
            {
               this.GpsLongitudeTextBox.Text = string.Format("{0:0.000000}", longitude);
            }
            else
            {
               char direction = (longitude > 0) ? 'E' : 'W';
               double value = Math.Abs(longitude);
               int degrees = (int)value;
               value -= degrees;
               value *= 60;
               int minutes = (int)(value);
               value -= minutes;
               value *= 60;
               int seconds = (int)value;
               this.GpsLongitudeTextBox.Text = string.Format("{0}° {1}' {2}\" {3}", degrees, minutes, seconds, direction);
            }
         }
         else
         {
            this.GpsLongitudeTextBox.Text = "";
         }

         DateTime utc = this.gps.Utc;

         if (utc.Year > 2000) 
         {
            this.GpsUtcTextBox.Text = string.Format("{0:D2}-{1:D2}-{2:D2} {3:D4}:{4:D2}:{5:D2}", utc.Month, utc.Day, utc.Year, utc.Hour, utc.Minute, utc.Second);
         }
         else
         {
            this.GpsUtcTextBox.Text = "";
         }

         #endregion

         #region NICBOT Body Update

         this.NicbotAccelerometerXTextBox.Text = this.nicbotBody.AccelerometerX.ToString("0");
         this.NicbotAccelerometerYTextBox.Text = this.nicbotBody.AccelerometerY.ToString("0");
         this.NicbotAccelerometerZTextBox.Text = this.nicbotBody.AccelerometerZ.ToString("0");
         this.NicbotRollTextBox.Text = this.nicbotBody.Roll.ToString("0.000");
         this.NicbotPitchTextBox.Text = this.nicbotBody.Pitch.ToString("0.000");

         if (UlcRoboticsNicbotBody.Modes.repair == this.nicbotBody.Mode)
         {

#if false            
            this.NicbotAutoDrillOriginFoundLabel.BackColor = (false != this.nicbotBody.AutoDrillOriginFound) ? Color.Green : Color.DarkSlateGray;
            this.NicbotAutoDrillCutCompleteLabel.BackColor = (false != this.nicbotBody.AutoDrillCutComplete) ? Color.Green : Color.DarkSlateGray;
            this.NicbotAutoDrillOriginHuntingLabel.BackColor = (false != this.nicbotBody.AutoDrillOriginHunting) ? Color.Yellow : Color.DarkSlateGray;

            if (false == this.nicbotBody.AutoDrillCutPaused)
            {
               this.SetNicbotAutoActivityButton.Text = "Pause";

               this.NicbotStatusPausedLabel.Text = "";
               this.NicbotStatusPausedLabel.BackColor = Color.DarkSlateGray;
            }
            else
            {
               this.SetNicbotAutoActivityButton.Text = "Resume";

               this.NicbotStatusPausedLabel.Text = "paused";
               this.NicbotStatusPausedLabel.BackColor = Color.Yellow;
            }

            if (false != this.nicbotBody.AutoDrillCutRunning)
            {
               this.NicbotDrillAutoStateLabel.Text = "running";
               this.NicbotDrillAutoStateLabel.BackColor = Color.LimeGreen;
            }
            else
            {
               this.NicbotDrillAutoStateLabel.Text = "off";
               this.NicbotDrillAutoStateLabel.BackColor = Color.DarkSlateGray;
            }

            if (false != this.nicbotBody.ToolRearLocation)
            {
               this.NicbotToolLocationLabel.Text = "rear";
               this.NicbotToolLocationLabel.BackColor = Color.Silver;
            }
            else
            {
               this.NicbotToolLocationLabel.Text = "front";
               this.NicbotToolLocationLabel.BackColor = Color.Silver;
            }
#endif

            this.NicbotActualFrontDrillSpeedTextBox.Text = this.nicbotBody.FrontDrillSpeed.ToString();
            this.NicbotActualFrontDrillIndexTextBox.Text = this.nicbotBody.FrontDrillIndex.ToString();
            this.NicbotActualRearDrillSpeedTextBox.Text = this.nicbotBody.RearDrillSpeed.ToString();
            this.NicbotActualRearDrillIndexTextBox.Text = this.nicbotBody.RearDrillIndex.ToString();

            this.NicbotRepairFrontDrillExtendLabel.BackColor = (false != this.nicbotBody.FrontDrillAtExtensionLimit) ? Color.Yellow : Color.DarkSlateGray;
            this.NicbotRepairFrontDrillRetractLabel.BackColor = (false != this.nicbotBody.FrontDrillAtRetractionLimit) ? Color.Yellow : Color.DarkSlateGray;
            this.NicbotRepairRearDrillExtendLabel.BackColor = (false != this.nicbotBody.RearDrillAtExtensionLimit) ? Color.Yellow : Color.DarkSlateGray;
            this.NicbotRepairRearDrillRetractLabel.BackColor = (false != this.nicbotBody.RearDrillAtRetractionLimit) ? Color.Yellow : Color.DarkSlateGray;

            this.NicbotRepairFrontUpperLimitLabel.BackColor = (false != this.nicbotBody.TopFrontReadyToLock) ? Color.Green : Color.DarkSlateGray;
            this.NicbotRepairFrontLowerLimitLabel.BackColor = (false != this.nicbotBody.BottomFrontReadyToLock) ? Color.Green : Color.DarkSlateGray;
            this.NicbotRepairRearUpperLimitLabel.BackColor = (false != this.nicbotBody.TopRearReadyToLock) ? Color.Green : Color.DarkSlateGray;
            this.NicbotRepairRearLowerLimitLabel.BackColor = (false != this.nicbotBody.BottomRearReadyToLock) ? Color.Green : Color.DarkSlateGray;

            this.NicbotRepairLastAxialLabel.BackColor = (false != this.nicbotBody.LastAxial) ? Color.Green : Color.DarkSlateGray;
            this.NicbotRepairLastCircumfertialLabel.BackColor = (false != this.nicbotBody.LastCircumferential) ? Color.Green : Color.DarkSlateGray;
         }

         if (UlcRoboticsNicbotBody.Modes.inspect == this.nicbotBody.Mode)
         {
            this.NicbotActualSensorIndexTextBox.Text = this.nicbotBody.SensorIndex.ToString();

            this.NicbotSensorCcwLimitLabel.BackColor = (false != this.nicbotBody.SensorAtCcwLimit) ? Color.Yellow : Color.DarkSlateGray;
            this.NicbotSensorCwLimitLabel.BackColor = (false != this.nicbotBody.SensorAtCwLimit) ? Color.Yellow : Color.DarkSlateGray;

            this.NicbotInspectFrontUpperLimitLabel.BackColor = (false != this.nicbotBody.TopFrontReadyToLock) ? Color.Green : Color.DarkSlateGray;
            this.NicbotInspectFrontLowerLimitLabel.BackColor = (false != this.nicbotBody.BottomFrontReadyToLock) ? Color.Green : Color.DarkSlateGray;
            this.NicbotInspectRearUpperLimitLabel.BackColor = (false != this.nicbotBody.TopRearReadyToLock) ? Color.Green : Color.DarkSlateGray;
            this.NicbotInspectRearLowerLimitLabel.BackColor = (false != this.nicbotBody.BottomRearReadyToLock) ? Color.Green : Color.DarkSlateGray;

            this.NicbotInspectLastAxialLabel.BackColor = (false != this.nicbotBody.LastAxial) ? Color.Green : Color.DarkSlateGray;
            this.NicbotInspectLastCircumfertialLabel.BackColor = (false != this.nicbotBody.LastCircumferential) ? Color.Green : Color.DarkSlateGray;
         }

         #endregion

         #region NICBOT Wheel Update

         this.NicbotWheelTemperatureTextBox.Text = this.nicbotWheel.Temperature.ToString("N1");
         this.NicbotWheelActualVelocityTextBox.Text = this.nicbotWheel.RPM.ToString();
         this.NicbotWheelActualTorqueTextBox.Text = this.nicbotWheel.Torque.ToString("N3");

         #endregion

         #region RS232 Update

         while (0 != this.rs232RxQueue.Count)
         {
            char ch = (char)((byte)this.rs232RxQueue.Dequeue());
            this.Rs232CommRichTextBox.AppendText(ch.ToString());
         }

         #endregion
      }

      private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
      {
         this.UpdateTimer.Enabled = false;
         this.SaveRegistry();
      }

      private void MotorControlPanel_Scroll(object sender, ScrollEventArgs e)
      {
         this.MotorTestActiveNodeIdTextBox.Focus();
         this.MotorControlPanel.VerticalScroll.Value = e.NewValue;
      }

      private void NicbotControlPanel_Scroll(object sender, ScrollEventArgs e)
      {
         this.NicbotBodyActiveNodeIdTextBox.Focus();
         this.NicbotControlPanel.VerticalScroll.Value = e.NewValue;
      }

      #endregion

      #region Constructor

      public MainForm()
      {
         this.InitializeComponent();
         this.StatusLabel.Text = "";

         this.device = new Device("device", 0);

         this.digitalIo = new PeakDigitalIo("digital IO", 0);
         this.digitalIo.OnInputChange = new PeakDigitalIo.InputChangeHandler(this.DigitalIoChangeHandler);

         this.analogIo = new PeakAnalogIo("analog IO", 0);

         this.encoder = new KublerRotaryEncoder("encoder", 0);
         
         this.motor = new ElmoWhistleMotor("motor", 0);
         this.motor.OnInputChange = new ElmoWhistleMotor.InputChangeHandler(this.MotorInputChangeHandler);
         
         this.camera = new UlcRoboticsCamera("camera", 0);
         this.gps = new UlcRoboticsGps("gps", 0);
         this.nicbotBody = new UlcRoboticsNicbotBody("nicbot body", 0);
         this.nicbotWheel = new UlcRoboticsNicbotWheel("nicbot wheel", 0);
                  
         this.rs232 = new UlcRoboticsRs232("rs232", 0);
         this.rs232.OnSerialReceive = new UlcRoboticsRs232.SerialReceiveHandler(this.Rs232ReceiveHandler);
         this.rs232RxQueue = new Queue();
         
         this.busInterface = new BusInterface();
         this.busInterface.AddDevice(this.device);
         this.busInterface.AddDevice(this.digitalIo);
         this.busInterface.AddDevice(this.analogIo);
         this.busInterface.AddDevice(this.encoder);
         this.busInterface.AddDevice(this.motor);
         this.busInterface.AddDevice(this.camera);
         this.busInterface.AddDevice(this.gps);
         this.busInterface.AddDevice(this.nicbotBody);
         this.busInterface.AddDevice(this.nicbotWheel);
         this.busInterface.AddDevice(this.rs232);

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
         this.BaudComboBox.SelectedIndex = 5;


         this.DigitalIoBaudComboBox.Items.Clear();
         this.DigitalIoBaudComboBox.Items.Add("1000000");
         this.DigitalIoBaudComboBox.Items.Add("800000");
         this.DigitalIoBaudComboBox.Items.Add("500000");
         this.DigitalIoBaudComboBox.Items.Add("250000");
         this.DigitalIoBaudComboBox.Items.Add("125000");
         this.DigitalIoBaudComboBox.Items.Add("50000");
         this.DigitalIoBaudComboBox.SelectedIndex = 5;


         this.AnalogIoBaudComboBox.Items.Clear();
         this.AnalogIoBaudComboBox.Items.Add("1000000");
         this.AnalogIoBaudComboBox.Items.Add("800000");
         this.AnalogIoBaudComboBox.Items.Add("500000");
         this.AnalogIoBaudComboBox.Items.Add("250000");
         this.AnalogIoBaudComboBox.Items.Add("125000");
         this.AnalogIoBaudComboBox.Items.Add("50000");
         this.AnalogIoBaudComboBox.SelectedIndex = 5;


         this.EncoderBaudComboBox.Items.Clear();
         this.EncoderBaudComboBox.Items.Add("10000");
         this.EncoderBaudComboBox.Items.Add("20000");
         this.EncoderBaudComboBox.Items.Add("50000");
         this.EncoderBaudComboBox.Items.Add("100000");
         this.EncoderBaudComboBox.Items.Add("125000");
         this.EncoderBaudComboBox.Items.Add("250000");
         this.EncoderBaudComboBox.Items.Add("500000");
         this.EncoderBaudComboBox.Items.Add("1000000");
         this.EncoderBaudComboBox.SelectedIndex = 6;

         this.MotorModeComboBox.SelectedIndex = 0;

         this.CameraBaudComboBox.Items.Clear();
         this.CameraBaudComboBox.Items.Add("10000");
         this.CameraBaudComboBox.Items.Add("20000");
         this.CameraBaudComboBox.Items.Add("50000");
         this.CameraBaudComboBox.Items.Add("125000");
         this.CameraBaudComboBox.Items.Add("250000");
         this.CameraBaudComboBox.Items.Add("500000");
         this.CameraBaudComboBox.Items.Add("1000000");
         this.CameraBaudComboBox.SelectedIndex = 5;


         this.GpsBaudComboBox.Items.Clear();
         this.GpsBaudComboBox.Items.Add("10000");
         this.GpsBaudComboBox.Items.Add("20000");
         this.GpsBaudComboBox.Items.Add("50000");
         this.GpsBaudComboBox.Items.Add("125000");
         this.GpsBaudComboBox.Items.Add("250000");
         this.GpsBaudComboBox.Items.Add("500000");
         this.GpsBaudComboBox.Items.Add("1000000");
         this.GpsBaudComboBox.SelectedIndex = 5;


         this.NicbotBodyBaudComboBox.Items.Clear();
         this.NicbotBodyBaudComboBox.Items.Add("10000");
         this.NicbotBodyBaudComboBox.Items.Add("20000");
         this.NicbotBodyBaudComboBox.Items.Add("50000");
         this.NicbotBodyBaudComboBox.Items.Add("100000");
         this.NicbotBodyBaudComboBox.Items.Add("125000");
         this.NicbotBodyBaudComboBox.Items.Add("250000");
         this.NicbotBodyBaudComboBox.Items.Add("500000");
         this.NicbotBodyBaudComboBox.Items.Add("1000000");
         this.NicbotBodyBaudComboBox.SelectedIndex = 5;


         this.NicbotWheelModeComboBox.SelectedIndex = 0;
         
         this.NicbotWheelBaudComboBox.Items.Clear();
         this.NicbotWheelBaudComboBox.Items.Add("10000");
         this.NicbotWheelBaudComboBox.Items.Add("20000");
         this.NicbotWheelBaudComboBox.Items.Add("50000");
         this.NicbotWheelBaudComboBox.Items.Add("100000");
         this.NicbotWheelBaudComboBox.Items.Add("125000");
         this.NicbotWheelBaudComboBox.Items.Add("250000");
         this.NicbotWheelBaudComboBox.Items.Add("500000");
         this.NicbotWheelBaudComboBox.Items.Add("1000000");
         this.NicbotWheelBaudComboBox.SelectedIndex = 5;


         this.Rs232BaudComboBox.Items.Clear();
         this.Rs232BaudComboBox.Items.Add("10000");
         this.Rs232BaudComboBox.Items.Add("20000");
         this.Rs232BaudComboBox.Items.Add("50000");
         this.Rs232BaudComboBox.Items.Add("125000");
         this.Rs232BaudComboBox.Items.Add("250000");
         this.Rs232BaudComboBox.Items.Add("500000");
         this.Rs232BaudComboBox.Items.Add("1000000");
         this.Rs232BaudComboBox.SelectedIndex = 5;

         this.NicbotAutoDrillOriginCheckBox.Checked = false;
         this.NicbotAutoDrillContinuousModeRadioButton.Checked = true;
         this.NicbotAutoDrillDistanceRetractionRadioButton.Checked = true;

         this.traceQueue = new Queue();

         QueuedTraceListener queuedTraceListener = new QueuedTraceListener(this.traceQueue);
         Trace.Listeners.Add(queuedTraceListener);

         Tracer.MaskString = "FFFFFFFF";

         this.ActivityButton.Text = "Start";
         this.active = false;

         this.HeartbeatActivityButton.Text = "Start";
         this.heartbeatActive = false;

         this.GpsDegreesRadioButton.Checked = true;
      }

      #endregion

   }
}
