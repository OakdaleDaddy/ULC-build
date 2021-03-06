﻿
namespace E4.DeviceTest
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.ComponentModel;
   using System.Data;
   using System.Diagnostics;
   using System.Drawing;
   using System.IO;
   using System.Linq;
   using System.Text;
   using System.Threading.Tasks;
   using System.Windows.Forms;

   using Microsoft.Win32;

   using E4.CAN;
   using E4.PCANLight;
   using E4.Utilities;

   public partial class MainForm : Form
   {
      #region Definition

      private const string RegistryCompanyName = "ULC Robotics";
      private const string RegistryApplicationName = "E4 Device Test";

      #endregion
      
      #region Fields

      private PciBusParameters pciABusParameters;
      private PciBusParameters pciBBusParameters;
      private UsbBusParameters usbABusParameters;
      private UsbBusParameters usbBBusParameters;
      private IpGatewayBusParameters ipgwABusParameters;
      private IpGatewayBusParameters ipgwBBusParameters;

      private bool active;
      private Queue traceQueue;
      
      private BusInterface busInterface;
      private Device device;
      private Device downloadDevice;
      private UlcRoboticsE4Main e4Main;

      private byte heartbeatNodeId;
      private bool heartbeatActive;
      private int heartbeatTime;
      private DateTime heartbeatTimeLimit;

      private bool downloadActive;
      private bool downloadComplete;
      private string downloadResult;
            
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


         keyValue = appKey.GetValue("PciABaudRate");
         int pciABaudRate = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 50000) : 50000;
         this.pciABusParameters = new PciBusParameters(BusInterfaces.PCIA, pciABaudRate, FramesType.INIT_TYPE_ST, TraceGroup.UI, null);

         keyValue = appKey.GetValue("PciBBaudRate");
         int pciBBaudRate = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 50000) : 50000;
         this.pciBBusParameters = new PciBusParameters(BusInterfaces.PCIB, pciBBaudRate, FramesType.INIT_TYPE_ST, TraceGroup.UI, null);

         keyValue = appKey.GetValue("UsbABaudRate");
         int usbABaudRate = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 50000) : 50000;
         this.usbABusParameters = new UsbBusParameters(BusInterfaces.USBA, usbABaudRate, FramesType.INIT_TYPE_ST, TraceGroup.UI, null);

         keyValue = appKey.GetValue("UsbBBaudRate");
         int usbBBaudRate = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 50000) : 50000;
         this.usbBBusParameters = new UsbBusParameters(BusInterfaces.USBB, usbBBaudRate, FramesType.INIT_TYPE_ST, TraceGroup.UI, null);

         keyValue = appKey.GetValue("IpGwATransmitAddress");
         string ipgwATransmitAddress = (null != keyValue) ? keyValue.ToString() : "127.0.0.1";
         keyValue = appKey.GetValue("IpGwATransmitPort");
         string ipgwATransmitPort = (null != keyValue) ? keyValue.ToString() : "7000";
         keyValue = appKey.GetValue("IpGwAReceiveAddress");
         string ipgwAReceiveAddress = (null != keyValue) ? keyValue.ToString() : "127.0.0.1";
         keyValue = appKey.GetValue("IpGwAReceivePort");
         string ipgwAReceivePort = (null != keyValue) ? keyValue.ToString() : "7000";
         this.ipgwABusParameters = new IpGatewayBusParameters(BusInterfaces.IPGWA, ipgwATransmitAddress, ipgwATransmitPort, ipgwAReceiveAddress, ipgwAReceivePort, FramesType.INIT_TYPE_ST, TraceGroup.UI, null);

         keyValue = appKey.GetValue("IpGwBTransmitAddress");
         string ipgwBTransmitAddress = (null != keyValue) ? keyValue.ToString() : "127.0.0.1";
         keyValue = appKey.GetValue("IpGwBTransmitPort");
         string ipgwBTransmitPort = (null != keyValue) ? keyValue.ToString() : "7001";
         keyValue = appKey.GetValue("IpGwBReceiveAddress");
         string ipgwBReceiveAddress = (null != keyValue) ? keyValue.ToString() : "127.0.0.1";
         keyValue = appKey.GetValue("IpGwBReceivePort");
         string ipgwBReceivePort = (null != keyValue) ? keyValue.ToString() : "7001";
         this.ipgwBBusParameters = new IpGatewayBusParameters(BusInterfaces.IPGWB, ipgwBTransmitAddress, ipgwBTransmitPort, ipgwBReceiveAddress, ipgwBReceivePort, FramesType.INIT_TYPE_ST, TraceGroup.UI, null);

         this.BusInterfaceComboBox.Items.Clear();
         this.BusInterfaceComboBox.Items.Add(this.pciABusParameters);
         this.BusInterfaceComboBox.Items.Add(this.pciBBusParameters);
         this.BusInterfaceComboBox.Items.Add(this.usbABusParameters);
         this.BusInterfaceComboBox.Items.Add(this.usbBBusParameters);
         this.BusInterfaceComboBox.Items.Add(this.ipgwABusParameters);
         this.BusInterfaceComboBox.Items.Add(this.ipgwBBusParameters);
         this.BusInterfaceComboBox.SelectedIndex = 0;


         keyValue = appKey.GetValue("BusInterface");
         this.BusInterfaceComboBox.SelectedIndex = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 0) : 0;


         keyValue = appKey.GetValue("ActiveNodeId");
         this.ActiveDeviceNodeIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue("HeartbeatNodeId");
         this.HeartbeatNodeIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "80";

         keyValue = appKey.GetValue("HeartbeatTime");
         this.HeartbeatTimeTextBox.Text = (null != keyValue) ? keyValue.ToString() : "1000";

         keyValue = appKey.GetValue("HeartbeatShown");
         this.ShowHeartbeatCheckBox.Checked = ("0" == ((null != keyValue) ? keyValue.ToString() : "0")) ? false : true;

         keyValue = appKey.GetValue("DownloadActiveNodeId");
         this.DownloadActiveNodeIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue("DownloadFile");
         this.DownloadFileTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";


         keyValue = appKey.GetValue("E4MainActiveNodeId");
         this.E4MainActiveNodeIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";
         
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

         appKey.SetValue("PciABaudRate", this.pciABusParameters.BitRate.ToString());
         appKey.SetValue("PciBBaudRate", this.pciBBusParameters.BitRate.ToString());
         appKey.SetValue("UsbABaudRate", this.usbABusParameters.BitRate.ToString());
         appKey.SetValue("UsbBBaudRate", this.usbBBusParameters.BitRate.ToString());

         if (null != this.ipgwABusParameters.TransmitEndPoint)
         {
            appKey.SetValue("IpGwATransmitAddress", this.ipgwABusParameters.TransmitEndPoint.Address.ToString());
            appKey.SetValue("IpGwATransmitPort", this.ipgwABusParameters.TransmitEndPoint.Port.ToString());
         }
         if (null != this.ipgwABusParameters.ReceiveEndPoint)
         {
            appKey.SetValue("IpGwAReceiveAddress", this.ipgwABusParameters.ReceiveEndPoint.Address.ToString());
            appKey.SetValue("IpGwAReceivePort", this.ipgwABusParameters.ReceiveEndPoint.Port.ToString());
         }

         if (null != this.ipgwBBusParameters.TransmitEndPoint)
         {
            appKey.SetValue("IpGwBTransmitAddress", this.ipgwBBusParameters.TransmitEndPoint.Address.ToString());
            appKey.SetValue("IpGwBTransmitPort", this.ipgwBBusParameters.TransmitEndPoint.Port.ToString());
         }
         if (null != this.ipgwABusParameters.ReceiveEndPoint)
         {
            appKey.SetValue("IpGwBReceiveAddress", this.ipgwBBusParameters.ReceiveEndPoint.Address.ToString());
            appKey.SetValue("IpGwBReceivePort", this.ipgwBBusParameters.ReceiveEndPoint.Port.ToString());
         }

         appKey.SetValue("BusInterface", this.BusInterfaceComboBox.SelectedIndex);
         appKey.SetValue("ActiveNodeId", this.ActiveDeviceNodeIdTextBox.Text);
         appKey.SetValue("HeartbeatNodeId", this.HeartbeatNodeIdTextBox.Text);
         appKey.SetValue("HeartbeatTime", this.HeartbeatTimeTextBox.Text);
         appKey.SetValue("HeartbeatShown", (this.ShowHeartbeatCheckBox.Checked != false) ? "1" : "0");

         appKey.SetValue("DownloadActiveNodeId", this.DownloadActiveNodeIdTextBox.Text);
         appKey.SetValue("DownloadFile", this.DownloadFileTextBox.Text);

         appKey.SetValue("E4MainActiveNodeId", this.E4MainActiveNodeIdTextBox.Text);

         #endregion
      }

      #endregion

      #region Helper Functions

      private void ControllerTraceTransmit(int cobId, byte[] data)
      {
         StringBuilder sb = new StringBuilder();

         for (int i = 0; i < data.Length; i++)
         {
            sb.AppendFormat("{0:X2}", data[i]);
         }

         Tracer.WriteMedium(TraceGroup.COMM, "", "tx {0:X3} {1}", cobId, sb.ToString());
      }

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

      private void RefreshDownloadFileTime()
      {
         string dateTimeDescription = "";

         if (File.Exists(this.DownloadFileTextBox.Text) != false)
         {
            DateTime dt = File.GetLastWriteTime(this.DownloadFileTextBox.Text);
            dateTimeDescription = string.Format("File Time: {0}/{1}/{2:D4} {3:D2}:{4:D2}:{5:D2}", dt.Month, dt.Day, dt.Year, dt.Hour, dt.Minute, dt.Second);
         }

         this.DownloadFileDateTimeLabel.Text = dateTimeDescription;
      }

      #endregion

      #region Delegate Functions

      private void DownloadCompleteHandler(string result)
      {
         this.downloadResult = result;
         this.downloadComplete = true;
      }

      #endregion

      #region Control Device Events
      
      private void BusInterfaceComboBox_SelectedIndexChanged(object sender, EventArgs e)
      {
         BusParameters busParameters = (BusParameters)this.BusInterfaceComboBox.SelectedItem;
         this.StatusLabel.Text = busParameters.GetDescription();
      }

      private void ActivityButton_Click(object sender, EventArgs e)
      {
         if (false == this.active)
         {
            string result = null;
            BusParameters busParameters = (BusParameters)this.BusInterfaceComboBox.SelectedItem;

            this.busInterface.Start(busParameters, ref result);

            if (null == result)
            {
               this.StatusLabel.Text = "Bus interface started.";

               this.active = true;
               this.ActivityButton.Text = "Stop";
               this.BusConfigurationButton.Enabled = false;
               this.BusInterfaceComboBox.Enabled = false;
            }
            else
            {
               this.StatusLabel.Text = result;
            }
         }
         else
         {
            this.busInterface.Stop();
            this.StatusLabel.Text = "Bus interface stopped.";

            this.active = false;
            this.ActivityButton.Text = "Start";
            this.BusConfigurationButton.Enabled = true;
            this.BusInterfaceComboBox.Enabled = true;
         }
      }

      private void BusConfigurationButton_Click(object sender, EventArgs e)
      {
         BusParameters busParameters = (BusParameters)this.BusInterfaceComboBox.SelectedItem;
         BusParametersForm busParametersForm = new BusParametersForm(busParameters);
         busParametersForm.ShowDialog();
         this.StatusLabel.Text = busParameters.GetDescription();
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
            this.StatusLabel.Text = "Device started.";
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
            this.StatusLabel.Text = "Device stopped.";
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
            this.StatusLabel.Text = "Device reset.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void DeviceClearFaultButton_Click(object sender, EventArgs e)
      {
         byte deviceNodeId = 0;

         if (byte.TryParse(this.ActiveDeviceNodeIdTextBox.Text, out deviceNodeId) != false)
         {
            this.device.NodeId = deviceNodeId;
            this.device.ClearFault();
            this.StatusLabel.Text = "Device fault cleared.";
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

      #region Device Download Events

      private void DownloadBrowseButton_Click(object sender, EventArgs e)
      {
         OpenFileDialog openFileDialog = new OpenFileDialog();

         if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
         {
            this.DownloadFileTextBox.Text = openFileDialog.FileName;
            this.RefreshDownloadFileTime();
         }
      }

      private void DownloadResetButton_Click(object sender, EventArgs e)
      {
         byte deviceNodeId = 0;

         if (byte.TryParse(this.DownloadActiveNodeIdTextBox.Text, out deviceNodeId) != false)
         {
            this.downloadDevice.NodeId = deviceNodeId;
            this.downloadDevice.Reset();
            this.StatusLabel.Text = "Device reset.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void DownloadClearFaultButton_Click(object sender, EventArgs e)
      {
         byte deviceNodeId = 0;

         if (byte.TryParse(this.DownloadActiveNodeIdTextBox.Text, out deviceNodeId) != false)
         {
            this.downloadDevice.NodeId = deviceNodeId;
            this.downloadDevice.ClearFault();
            this.StatusLabel.Text = "Device fault cleared.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void DownloadEraseButton_Click(object sender, EventArgs e)
      {
         byte deviceNodeId = 0;

         if (byte.TryParse(this.DownloadActiveNodeIdTextBox.Text, out deviceNodeId) != false)
         {
            this.downloadDevice.NodeId = deviceNodeId;

            byte[] eraseData = BitConverter.GetBytes((UInt32)0x73617265);
            bool result = this.downloadDevice.SDODownload(0x2FFF, 0x02, eraseData);

            if (false != result)
            {
               this.StatusLabel.Text = "Erase triggered.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to trigger erase.";
               this.DeviceUploadHexDataTextBox.Text = "";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void DownloadActivityButton_Click(object sender, EventArgs e)
      {
         if (false == this.downloadActive)
         {
            byte nodeId = 0;

            if (byte.TryParse(this.DownloadActiveNodeIdTextBox.Text, out nodeId) != false)
            {
               this.downloadDevice.NodeId = nodeId;
               this.downloadComplete = false;

               bool result = this.downloadDevice.DownloadImage(this.DownloadFileTextBox.Text, new Device.ImageDownloadCompleteHandler(this.DownloadCompleteHandler));

               if (false != result)
               {
                  this.downloadActive = true;

                  this.DownloadFileTextBox.Enabled = false;
                  this.DownloadBrowseButton.Enabled = false;
                  this.DownloadActivityButton.Text = "Stop";
                  this.DownloadProgressBar.Value = 0;
                  this.DownloadProgressBar.Visible = true;

                  this.StatusLabel.Text = "Downloading...";
               }
               else
               {
                  this.StatusLabel.Text = "Unable to start download.";
               }
            }
            else
            {
               this.StatusLabel.Text = "Invalid entry.";
            }
         }
         else
         {
            this.downloadDevice.StopImageDownload();
            this.downloadActive = false;

            this.DownloadProgressLabel.Text = "";
            this.DownloadProgressBar.Visible = false;
            this.DownloadFileTextBox.Enabled = true;
            this.DownloadBrowseButton.Enabled = true;
            this.DownloadActivityButton.Text = "Start";

            this.StatusLabel.Text = "Image download stopped.";
         }
      }

      #endregion

      #region Main Events

      #region NMT 

      private void E4MainConfigButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UlcRoboticsE4Main.UsageModes usageMode = UlcRoboticsE4Main.UsageModes.undefined;

         if (this.E4MainUsageComboBox.Text == "laser")
         {
            usageMode = UlcRoboticsE4Main.UsageModes.laser;
         }
         else if (this.E4MainUsageComboBox.Text == "target")
         {
            usageMode = UlcRoboticsE4Main.UsageModes.target;
         }

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;

            bool result = this.e4Main.Configure(usageMode);

            if (false != result)
            {
               this.StatusLabel.Text = "Main configured.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to configure main.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void E4MainStartButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;

            bool result = this.e4Main.Start();

            if (false != result)
            {
               this.StatusLabel.Text = "Main started.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to start main.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void E4MainStopButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            this.e4Main.Stop();
            this.StatusLabel.Text = "Main stopped.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void E4MainResetButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            this.e4Main.Reset();
            this.StatusLabel.Text = "Main reset.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void E4MainSyncButton_Click(object sender, EventArgs e)
      {
         this.busInterface.Sync();
      }

      #endregion

      #region Heartbeat

      private void SetE4MainConsumerHeartbeatButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         byte consumerHeartbeatNodeId = 0;
         UInt16 consumerHeartbeatTime = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (byte.TryParse(this.E4MainConsumerHeartbeatNodeIdTextBox.Text, out consumerHeartbeatNodeId) != false) &&
             (UInt16.TryParse(this.E4MainConsumerHeartbeatTimeTextBox.Text, out consumerHeartbeatTime) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.SetConsumerHeartbeat(consumerHeartbeatTime, consumerHeartbeatNodeId);

            if (false != result)
            {
               this.StatusLabel.Text = "Main consumer heartbeat set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set main consumer heartbeat.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainProducerHeartbeatButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt16 producerHeartbeatTime = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt16.TryParse(this.E4MainProducerHeartbeatTimeTextBox.Text, out producerHeartbeatTime) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.SetProducerHeartbeat(producerHeartbeatTime);

            if (false != result)
            {
               this.StatusLabel.Text = "Main producer heartbeat set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set main producer heartbeat.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }
      
      #endregion

      #region Camera

      private void GetE4MainCameraSelectButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            byte cameraSelect = 0;
            bool success = this.e4Main.GetCameraSelect(ref cameraSelect);

            if (false != success)
            {
               this.StatusLabel.Text = "Main camera select retrieved.";
               this.E4MainCameraTextBox.Text = string.Format("{0}", cameraSelect);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get main camera select.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainCameraSelectButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         byte videoSelect = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (byte.TryParse(this.E4MainCameraTextBox.Text, out videoSelect) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.SetCameraSelect(videoSelect);

            if (false != result)
            {
               this.StatusLabel.Text = "Main camera select set to " + videoSelect.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set main camera select set to " + videoSelect.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainCameraLedIntensityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) 
         {
            this.e4Main.NodeId = nodeId;
            UInt32 ledIntensityLevel = 0;
            bool success = this.e4Main.GetCameraLedIntensityLevel(ref ledIntensityLevel);

            if (false != success)
            {
               this.StatusLabel.Text = "Main camera LED intensity retrieved.";
               this.E4MainCameraLedIntensityTextBox.Text = string.Format("{0}", ledIntensityLevel);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get main camera LED intensity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainCameraLedIntensityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt32 ledIntensityLevel = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt32.TryParse(this.E4MainCameraLedIntensityTextBox.Text, out ledIntensityLevel) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.SetCameraLedIntensityLevel(ledIntensityLevel);

            if (false != result)
            {
               this.StatusLabel.Text = "Main camera LED intensity set to " + ledIntensityLevel.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set main camera LED intensity set to " + ledIntensityLevel.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void DecreaseE4MainCameraLedIntensityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt32 ledIntensityLevel = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt32.TryParse(this.E4MainCameraLedIntensityTextBox.Text, out ledIntensityLevel) != false))
         {
            this.e4Main.NodeId = nodeId;
            ledIntensityLevel = (byte)((ledIntensityLevel >= 8) ? (ledIntensityLevel - 8) : 0);
            bool result = this.e4Main.SetCameraLedIntensityLevel(ledIntensityLevel);

            if (false != result)
            {
               this.StatusLabel.Text = "Main camera light LED intensity decreased.";
               this.E4MainCameraLedIntensityTextBox.Text = ledIntensityLevel.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to decrease main camera LED intensity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void IncreaseE4MainCameraLedIntensityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt32 ledIntensityLevel = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt32.TryParse(this.E4MainCameraLedIntensityTextBox.Text, out ledIntensityLevel) != false))
         {
            this.e4Main.NodeId = nodeId;
            ledIntensityLevel = (byte)((ledIntensityLevel <= 0xFFFFFFFF) ? (ledIntensityLevel + 8) : 0xFFFFFFFF);
            bool result = this.e4Main.SetCameraLedIntensityLevel(ledIntensityLevel);

            if (false != result)
            {
               this.StatusLabel.Text = "Main camera light LED intensity increased.";
               this.E4MainCameraLedIntensityTextBox.Text = ledIntensityLevel.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to increase main camera LED intensity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainCameraLedChannelMaskButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            byte ledChannelMask = 0;
            bool success = this.e4Main.GetCameraLedChannelMask(ref ledChannelMask);

            if (false != success)
            {
               this.StatusLabel.Text = "Main camera LED channel mask retrieved.";
               this.E4MainCameraLedChannelMaskTextBox.Text = string.Format("{0:X2}", ledChannelMask);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get main camera LED channel mask.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainCameraLedChannelMaskButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         byte ledChannelMask = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (byte.TryParse(this.E4MainCameraLedChannelMaskTextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out ledChannelMask) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.SetCameraLedChannelMask(ledChannelMask);

            if (false != result)
            {
               this.StatusLabel.Text = "Main camera LED channel mask set set to " + ledChannelMask.ToString("X2") + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set main camera LED channel mask set to " + ledChannelMask.ToString("X2") + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }      

      #endregion

      #region IMU

      private void ReadE4MainMainIcuDirectionsButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            double roll = 0;
            double pitch = 0;
            double yaw = 0;
            bool success = this.e4Main.GetMainBoardRoll(ref roll);
            success &= this.e4Main.GetMainBoardPitch(ref pitch);
            success &= this.e4Main.GetMainBoardYaw(ref yaw);

            if (false != success)
            {
               this.StatusLabel.Text = "Main board direction retrieved.";
               this.E4MainMainBoardIcuReadRollTextBox.Text = string.Format("{0:0.0}", roll);
               this.E4MainMainBoardIcuReadPitchTextBox.Text = string.Format("{0:0.0}", pitch);
               this.E4MainMainBoardIcuReadYawTextBox.Text = string.Format("{0:0.0}", yaw);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get main board direction.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void ReadE4MainTargetIcuDirectionsButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            double roll = 0;
            double pitch = 0;
            double yaw = 0;
            bool success = this.e4Main.GetTargetBoardRoll(ref roll);
            success &= this.e4Main.GetTargetBoardPitch(ref pitch);
            success &= this.e4Main.GetTargetBoardYaw(ref yaw);

            if (false != success)
            {
               this.StatusLabel.Text = "Target board direction retrieved.";
               this.E4MainTargetBoardIcuReadRollTextBox.Text = string.Format("{0:0.0}", roll);
               this.E4MainTargetBoardIcuReadPitchTextBox.Text = string.Format("{0:0.0}", pitch);
               this.E4MainTargetBoardIcuReadYawTextBox.Text = string.Format("{0:0.0}", yaw);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get target board direction.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #region Laser Range Finder

      private void SetE4MainLaserAimOnButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) 
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.SetLaserAimOn();

            if (false != result)
            {
               this.StatusLabel.Text = "Laser aim on.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set laser aim on.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainLaserAimOffButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.SetLaserAimOff();

            if (false != result)
            {
               this.StatusLabel.Text = "Laser aim off.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set laser aim off.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainLaserTimeToMeasureButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            byte timeToMeasure = 0;
            bool success = this.e4Main.GetLaserTimeToMeasure(ref timeToMeasure);

            if (false != success)
            {
               this.StatusLabel.Text = "Laser time to measure retrieved.";
               this.E4MainLaserTimeToMeasureTextBox.Text = string.Format("{0}", timeToMeasure);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get laser time to measure.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainLaserTimeToMeasureButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         byte timeToMeasure = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (byte.TryParse(this.E4MainLaserTimeToMeasureTextBox.Text, out timeToMeasure) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.SetLaserTimeToMeasure(timeToMeasure);

            if (false != result)
            {
               this.StatusLabel.Text = "Laser time to measure set to " + timeToMeasure.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set laser time to measure to " + timeToMeasure.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainLaserControlByteButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            byte controlByte = 0;
            bool success = this.e4Main.GetLaserControlByte(ref controlByte);

            if (false != success)
            {
               this.StatusLabel.Text = "Laser control byte retrieved.";
               this.E4MainLaserControlByteTextBox.Text = string.Format("{0:X2}", controlByte);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get laser control byte.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainLaserControlByteButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         byte laserControlByte = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (byte.TryParse(this.E4MainLaserControlByteTextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out laserControlByte) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.SetLaserControlByte(laserControlByte);

            if (false != result)
            {
               this.StatusLabel.Text = "Laser control byte set to 0x" + laserControlByte.ToString("X2") + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set laser control byte to 0x" + laserControlByte.ToString("X2") + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainLaserReadDistanceButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            UInt32 distance = 0;
            bool success = this.e4Main.GetLaserDistance(ref distance);

            if (false != success)
            {
               this.StatusLabel.Text = "Laser distance retrieved.";
               this.E4MainLaserReadDistanceTextBox.Text = string.Format("{0}", distance);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get laserdistance.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainLaserRangeFinderTemperatureButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            byte temperature = 0;
            bool success = this.e4Main.GetLaserRangeFinderTemperature(ref temperature);

            if (false != success)
            {
               this.StatusLabel.Text = "Laser range finder temperature retrieved.";
               this.E4MainLaserRangeFinderTemperatureTextBox.Text = string.Format("{0}", temperature);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get laser distance.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #region Laser Scanner

      private void GetE4MainLaserScannerPositionButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            byte position = 0;
            bool success = this.e4Main.GetScannerCoordinates(ref position);

            if (false != success)
            {
               this.StatusLabel.Text = "Laser scanner position retrieved.";
               this.E4MainLaserReadScannerTextBox.Text = string.Format("{0:X2}", position);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get laser scanner position.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainLaserScannerTemperatureButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            byte temperature = 0;
            bool success = this.e4Main.GetScannerTemperature(ref temperature);

            if (false != success)
            {
               this.StatusLabel.Text = "Laser scanner temperature retrieved.";
               this.E4MainLaserScannerTemperatureTextBox.Text = string.Format("{0}", temperature);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get laser scanner temperature.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #region BLDC0 Events

      #region General

      private void SetE4MainBldc0ModeButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         MotorComponent.Modes mode = MotorComponent.Modes.undefined;

         if (this.E4MainBldc0ModeComboBox.Text == "off")
         {
            mode = MotorComponent.Modes.off;
         }
         else if (this.E4MainBldc0ModeComboBox.Text == "position")
         {
            mode = MotorComponent.Modes.position;
         }
         else if (this.E4MainBldc0ModeComboBox.Text == "velocity")
         {
            mode = MotorComponent.Modes.velocity;
         }

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc0.SetMode(mode);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC0 mode set to " + mode.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC0 mode to " + mode.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void ClearE4MainBldc0FaultButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            this.e4Main.Bldc0.ClearFault();
            this.StatusLabel.Text = "BLDC0 fault cleared.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void HaltE4MainBldc0Button_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc0.Halt();

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC0 halted.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to halt BLDC0.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void RunE4MainBldc0Button_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc0.Run();

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC0 run.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to run BLDC0.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #region Position Mode

      private void GetE4MainBldc0TargetPositionButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            Int32 targetPosition = 0;
            bool targetPositionRelative = false;
            bool success = this.e4Main.Bldc0.GetTargetPosition(ref targetPosition, ref targetPositionRelative);

            if (false != success)
            {
               this.StatusLabel.Text = "BLDC0 target position retrieved.";
               this.E4MainBldc0TargetPositionTextBox.Text = string.Format("{0}", targetPosition);
               this.E4MainBldc0TargetPositionRelativeCheckBox.Checked = targetPositionRelative;
            }
            else
            {
               this.StatusLabel.Text = "Unable to get BLDC0 target position.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainBldc0TargetPositionButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 targetPosition = 0;
         bool targetPositionRelative = this.E4MainBldc0TargetPositionRelativeCheckBox.Checked;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.E4MainBldc0TargetPositionTextBox.Text, out targetPosition) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc0.SetTargetPosition(targetPosition, targetPositionRelative);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC0 target position set to " + targetPosition.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC0 target position to " + targetPosition.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void ScheduleE4MainBldc0TargetPositionButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 targetPosition = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.E4MainBldc0TargetPositionTextBox.Text, out targetPosition) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc0.ScheduleTargetPosition(targetPosition);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC0 target position scheduled for " + targetPosition.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to schedule BLDC0 position for " + targetPosition.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainBldc0ProfileVelocityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            Int32 profileVelocity = 0;
            bool success = this.e4Main.Bldc0.GetProfileVelocity(ref profileVelocity);

            if (false != success)
            {
               this.StatusLabel.Text = "BLDC0 profile velocity retrieved.";
               this.E4MainBldc0ProfileVelocityTextBox.Text = string.Format("{0}", profileVelocity);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get BLDC0 profile velocity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainBldc0ProfileVelocityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 profileVelocity = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.E4MainBldc0ProfileVelocityTextBox.Text, out profileVelocity) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc0.SetProfileVelocity(profileVelocity);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC0 profile velocity set to " + profileVelocity.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC0 profile velocity to " + profileVelocity.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #region Velocity Mode

      private void GetE4MainBldc0TargetVelocityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            Int32 targetVelocity = 0;
            bool success = this.e4Main.Bldc0.GetTargetVelocity(ref targetVelocity);

            if (false != success)
            {
               this.StatusLabel.Text = "BLDC0 target velocity retrieved.";
               this.E4MainBldc0TargetVelocityTextBox.Text = string.Format("{0}", targetVelocity);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get BLDC0 target velocity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainBldc0TargetVelocityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 targetVelocity = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.E4MainBldc0TargetVelocityTextBox.Text, out targetVelocity) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc0.SetTargetVelocity(targetVelocity);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC0 target velocity set to " + targetVelocity.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC0 target velocity to " + targetVelocity.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void ScheduleE4MainBldc0TargetVelocityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 targetVelocity = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.E4MainBldc0TargetVelocityTextBox.Text, out targetVelocity) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc0.ScheduleTargetVelocity(targetVelocity);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC0 target velocity scheduled for " + targetVelocity.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to schedule BLDC0 velocity for " + targetVelocity.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainBldc0ProfileAccelerationButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            Int32 profileAcceleration = 0;
            bool success = this.e4Main.Bldc0.GetProfileAcceleration(ref profileAcceleration);

            if (false != success)
            {
               this.StatusLabel.Text = "BLDC0 profile acceleration retrieved.";
               this.E4MainBldc0ProfileAccelerationTextBox.Text = string.Format("{0}", profileAcceleration);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get BLDC0 profile acceleration.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainBldc0ProfileAccelerationButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 profileAcceleration = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.E4MainBldc0ProfileAccelerationTextBox.Text, out profileAcceleration) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc0.SetProfileAcceleration(profileAcceleration);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC0 profile acceleration set to " + profileAcceleration.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC0 profile acceleration to " + profileAcceleration.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainBldc0ProfileDecelerationButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            Int32 profileDeceleration = 0;
            bool success = this.e4Main.Bldc0.GetProfileDeceleration(ref profileDeceleration);

            if (false != success)
            {
               this.StatusLabel.Text = "BLDC0 profile deceleration retrieved.";
               this.E4MainBldc0ProfileDecelerationTextBox.Text = string.Format("{0}", profileDeceleration);
            }       
            else
            {
               this.StatusLabel.Text = "Unable to get BLDC0 profile deceleration.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainBldc0ProfileDecelerationButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 profileDeceleration = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.E4MainBldc0ProfileDecelerationTextBox.Text, out profileDeceleration) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc0.SetProfileDeceleration(profileDeceleration);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC0 profile deceleration set to " + profileDeceleration.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC0 profile deceleration to " + profileDeceleration.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #region Tuning

      private void GetE4MainBldc0KpButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            Int32 motorKp = 0;
            bool success = (false != this.E4MainBldc0TuningVelocityRadioButton.Checked) ? this.e4Main.Bldc0.GetVelocityKp(ref motorKp) : this.e4Main.Bldc0.GetPositionKp(ref motorKp);

            if (false != success)
            {
               this.StatusLabel.Text = "BLDC0 KP retrieved.";
               this.E4MainBldc0KpTextBox.Text = string.Format("{0}", motorKp);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get BLDC0 KP.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainBldc0KpButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 motorKp = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.E4MainBldc0KpTextBox.Text, out motorKp) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = (false != this.E4MainBldc0TuningVelocityRadioButton.Checked) ? this.e4Main.Bldc0.SetVelocityKp(motorKp) : this.e4Main.Bldc0.SetPositionKp(motorKp);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC0 KP set to " + motorKp.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC0 KP to " + motorKp.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainBldc0KiButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            Int32 motorKi = 0;
            bool success = (false != this.E4MainBldc0TuningVelocityRadioButton.Checked) ? this.e4Main.Bldc0.GetVelocityKi(ref motorKi) : this.e4Main.Bldc0.GetPositionKi(ref motorKi);

            if (false != success)
            {
               this.StatusLabel.Text = "BLDC0 KI retrieved.";
               this.E4MainBldc0KiTextBox.Text = string.Format("{0}", motorKi);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get BLDC0 KI.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainBldc0KiButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 motorKi = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.E4MainBldc0KiTextBox.Text, out motorKi) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = (false != this.E4MainBldc0TuningVelocityRadioButton.Checked) ? this.e4Main.Bldc0.SetVelocityKi(motorKi) : this.e4Main.Bldc0.SetPositionKi(motorKi);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC0 KI set to " + motorKi.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC0 KI to " + motorKi.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainBldc0KdButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            Int32 motorKd = 0;
            bool success = (false != this.E4MainBldc0TuningVelocityRadioButton.Checked) ? this.e4Main.Bldc0.GetVelocityKd(ref motorKd) : this.e4Main.Bldc0.GetPositionKd(ref motorKd);

            if (false != success)
            {
               this.StatusLabel.Text = "BLDC0 KD retrieved.";
               this.E4MainBldc0KdTextBox.Text = string.Format("{0}", motorKd);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get BLDC0 KD.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainBldc0KdButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 motorKd = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.E4MainBldc0KdTextBox.Text, out motorKd) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = (false != this.E4MainBldc0TuningVelocityRadioButton.Checked) ? this.e4Main.Bldc0.SetVelocityKd(motorKd) : this.e4Main.Bldc0.SetPositionKd(motorKd);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC0 KD set to " + motorKd.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC0 KD to " + motorKd.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainBldc0PositionWindowButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            UInt32 positionWindow = 0;
            bool success = this.e4Main.Bldc0.GetPositionWindow(ref positionWindow);

            if (false != success)
            {
               this.StatusLabel.Text = "BLDC0 position window retrieved.";
               this.E4MainBldc0PositionWindowTextBox.Text = string.Format("{0}", positionWindow);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get BLDC0 position window.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainBldc0PositionWindowButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt32 positionWindow = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt32.TryParse(this.E4MainBldc0PositionWindowTextBox.Text, out positionWindow) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc0.SetPositionWindow(positionWindow);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC0 position window set to " + positionWindow.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC0 position window to " + positionWindow.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainBldc0PositionWindowTimeButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            UInt16 positionWindowTime = 0;
            bool success = this.e4Main.Bldc0.GetPositionWindowTime(ref positionWindowTime);

            if (false != success)
            {
               this.StatusLabel.Text = "BLDC0 position window time retrieved.";
               this.E4MainBldc0PositionWindowTimeTextBox.Text = string.Format("{0}", positionWindowTime);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get BLDC0 position window time.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainBldc0PositionWindowTimeButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt16 positionWindowTime = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt16.TryParse(this.E4MainBldc0PositionWindowTimeTextBox.Text, out positionWindowTime) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc0.SetPositionWindowTime(positionWindowTime);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC0 position window time set to " + positionWindowTime.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC0 position window time to " + positionWindowTime.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainBldc0VelocityWindowButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            UInt16 velocityWindow = 0;
            bool success = this.e4Main.Bldc0.GetVelocityWindow(ref velocityWindow);

            if (false != success)
            {
               this.StatusLabel.Text = "BLDC0 velocity window retrieved.";
               this.E4MainBldc0VelocityWindowTextBox.Text = string.Format("{0}", velocityWindow);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get BLDC0 velocity window.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainBldc0VelocityWindowButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt16 velocityWindow = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt16.TryParse(this.E4MainBldc0VelocityWindowTextBox.Text, out velocityWindow) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc0.SetVelocityWindow(velocityWindow);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC0 velocity window set to " + velocityWindow.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC0 velocity window to " + velocityWindow.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainBldc0VelocityWindowTimeButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            UInt16 velocityWindowTime = 0;
            bool success = this.e4Main.Bldc0.GetVelocityWindowTime(ref velocityWindowTime);

            if (false != success)
            {
               this.StatusLabel.Text = "BLDC0 velocity window time retrieved.";
               this.E4MainBldc0VelocityWindowTimeTextBox.Text = string.Format("{0}", velocityWindowTime);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get BLDC0 velocity window time.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainBldc0VelocityWindowTimeButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt16 velocityWindowTime = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt16.TryParse(this.E4MainBldc0VelocityWindowTimeTextBox.Text, out velocityWindowTime) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc0.SetVelocityWindowTime(velocityWindowTime);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC0 velocity window time set to " + velocityWindowTime.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC0 velocity window time to " + velocityWindowTime.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainBldc0VelocityThresholdButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            UInt16 velocityThreshold = 0;
            bool success = this.e4Main.Bldc0.GetVelocityThreshold(ref velocityThreshold);

            if (false != success)
            {
               this.StatusLabel.Text = "BLDC0 velocity threshold retrieved.";
               this.E4MainBldc0VelocityThresholdTextBox.Text = string.Format("{0}", velocityThreshold);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get BLDC0 velocity threshold.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainBldc0VelocityThresholdButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt16 velocityThreshold = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt16.TryParse(this.E4MainBldc0VelocityThresholdTextBox.Text, out velocityThreshold) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc0.SetVelocityThreshold(velocityThreshold);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC0 velocity threshold set to " + velocityThreshold.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC0 velocity threshold to " + velocityThreshold.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainBldc0VelocityThresholdTimeButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            UInt16 velocityThresholdTime = 0;
            bool success = this.e4Main.Bldc0.GetVelocityThresholdTime(ref velocityThresholdTime);

            if (false != success)
            {
               this.StatusLabel.Text = "BLDC0 velocity threshold time retrieved.";
               this.E4MainBldc0VelocityThresholdTimeTextBox.Text = string.Format("{0}", velocityThresholdTime);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get BLDC0 velocity time threshold.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainBldc0VelocityThresholdTimeButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt16 velocityThresholdTime = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt16.TryParse(this.E4MainBldc0VelocityThresholdTimeTextBox.Text, out velocityThresholdTime) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc0.SetVelocityThresholdTime(velocityThresholdTime);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC0 velocity threshold time set to " + velocityThresholdTime.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC0 velocity time threshold to " + velocityThresholdTime.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #endregion

      #region BLDC1 Events

      #region General

      private void SetE4MainBldc1ModeButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         MotorComponent.Modes mode = MotorComponent.Modes.undefined;

         if (this.E4MainBldc1ModeComboBox.Text == "off")
         {
            mode = MotorComponent.Modes.off;
         }
         else if (this.E4MainBldc1ModeComboBox.Text == "position")
         {
            mode = MotorComponent.Modes.position;
         }
         else if (this.E4MainBldc1ModeComboBox.Text == "velocity")
         {
            mode = MotorComponent.Modes.velocity;
         }

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc1.SetMode(mode);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC1 mode set to " + mode.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC1 mode to " + mode.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void ClearE4MainBldc1FaultButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            this.e4Main.Bldc1.ClearFault();
            this.StatusLabel.Text = "BLDC1 fault cleared.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void HaltE4MainBldc1Button_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc1.Halt();

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC1 halted.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to halt BLDC1.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void RunE4MainBldc1Button_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc1.Run();

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC1 run.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to run BLDC1.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #region Position Mode

      private void GetE4MainBldc1TargetPositionButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            Int32 targetPosition = 0;
            bool targetPositionRelative = false;
            bool success = this.e4Main.Bldc1.GetTargetPosition(ref targetPosition, ref targetPositionRelative);

            if (false != success)
            {
               this.StatusLabel.Text = "BLDC1 target position retrieved.";
               this.E4MainBldc1TargetPositionTextBox.Text = string.Format("{0}", targetPosition);
               this.E4MainBldc1TargetPositionRelativeCheckBox.Checked = targetPositionRelative;
            }
            else
            {
               this.StatusLabel.Text = "Unable to get BLDC1 target position.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainBldc1TargetPositionButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 targetPosition = 0;
         bool targetPositionRelative = this.E4MainBldc1TargetPositionRelativeCheckBox.Checked;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.E4MainBldc1TargetPositionTextBox.Text, out targetPosition) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc1.SetTargetPosition(targetPosition, targetPositionRelative);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC1 target position set to " + targetPosition.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC1 target position to " + targetPosition.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void ScheduleE4MainBldc1TargetPositionButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 targetPosition = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.E4MainBldc1TargetPositionTextBox.Text, out targetPosition) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc1.ScheduleTargetPosition(targetPosition);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC1 target position scheduled for " + targetPosition.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to schedule BLDC1 position for " + targetPosition.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainBldc1ProfileVelocityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            Int32 profileVelocity = 0;
            bool success = this.e4Main.Bldc1.GetProfileVelocity(ref profileVelocity);

            if (false != success)
            {
               this.StatusLabel.Text = "BLDC1 profile velocity retrieved.";
               this.E4MainBldc1ProfileVelocityTextBox.Text = string.Format("{0}", profileVelocity);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get BLDC1 profile velocity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainBldc1ProfileVelocityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 profileVelocity = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.E4MainBldc1ProfileVelocityTextBox.Text, out profileVelocity) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc1.SetProfileVelocity(profileVelocity);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC1 profile velocity set to " + profileVelocity.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC1 profile velocity to " + profileVelocity.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #region Velocity Mode

      private void GetE4MainBldc1TargetVelocityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            Int32 targetVelocity = 0;
            bool success = this.e4Main.Bldc1.GetTargetVelocity(ref targetVelocity);

            if (false != success)
            {
               this.StatusLabel.Text = "BLDC1 target velocity retrieved.";
               this.E4MainBldc1TargetVelocityTextBox.Text = string.Format("{0}", targetVelocity);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get BLDC1 target velocity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainBldc1TargetVelocityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 targetVelocity = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.E4MainBldc1TargetVelocityTextBox.Text, out targetVelocity) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc1.SetTargetVelocity(targetVelocity);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC1 target velocity set to " + targetVelocity.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC1 target velocity to " + targetVelocity.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void ScheduleE4MainBldc1TargetVelocityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 targetVelocity = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.E4MainBldc1TargetVelocityTextBox.Text, out targetVelocity) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc1.ScheduleTargetVelocity(targetVelocity);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC1 target velocity scheduled for " + targetVelocity.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to schedule BLDC1 velocity for " + targetVelocity.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainBldc1ProfileAccelerationButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            Int32 profileAcceleration = 0;
            bool success = this.e4Main.Bldc1.GetProfileAcceleration(ref profileAcceleration);

            if (false != success)
            {
               this.StatusLabel.Text = "BLDC1 profile acceleration retrieved.";
               this.E4MainBldc1ProfileAccelerationTextBox.Text = string.Format("{0}", profileAcceleration);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get BLDC1 profile acceleration.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainBldc1ProfileAccelerationButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 profileAcceleration = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.E4MainBldc1ProfileAccelerationTextBox.Text, out profileAcceleration) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc1.SetProfileAcceleration(profileAcceleration);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC1 profile acceleration set to " + profileAcceleration.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC1 profile acceleration to " + profileAcceleration.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainBldc1ProfileDecelerationButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            Int32 profileDeceleration = 0;
            bool success = this.e4Main.Bldc1.GetProfileDeceleration(ref profileDeceleration);

            if (false != success)
            {
               this.StatusLabel.Text = "BLDC1 profile deceleration retrieved.";
               this.E4MainBldc1ProfileDecelerationTextBox.Text = string.Format("{0}", profileDeceleration);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get BLDC1 profile deceleration.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainBldc1ProfileDecelerationButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 profileDeceleration = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.E4MainBldc1ProfileDecelerationTextBox.Text, out profileDeceleration) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc1.SetProfileDeceleration(profileDeceleration);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC1 profile deceleration set to " + profileDeceleration.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC1 profile deceleration to " + profileDeceleration.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #region Tuning

      private void GetE4MainBldc1KpButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            Int32 motorKp = 0;
            bool success = (false != this.E4MainBldc1TuningVelocityRadioButton.Checked) ? this.e4Main.Bldc1.GetVelocityKp(ref motorKp) : this.e4Main.Bldc1.GetPositionKp(ref motorKp);

            if (false != success)
            {
               this.StatusLabel.Text = "BLDC1 KP retrieved.";
               this.E4MainBldc1KpTextBox.Text = string.Format("{0}", motorKp);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get BLDC1 KP.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainBldc1KpButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 motorKp = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.E4MainBldc1KpTextBox.Text, out motorKp) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = (false != this.E4MainBldc1TuningVelocityRadioButton.Checked) ? this.e4Main.Bldc1.SetVelocityKp(motorKp) : this.e4Main.Bldc1.SetPositionKp(motorKp);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC1 KP set to " + motorKp.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC1 KP to " + motorKp.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainBldc1KiButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            Int32 motorKi = 0;
            bool success = (false != this.E4MainBldc1TuningVelocityRadioButton.Checked) ? this.e4Main.Bldc1.GetVelocityKi(ref motorKi) : this.e4Main.Bldc1.GetPositionKi(ref motorKi);

            if (false != success)
            {
               this.StatusLabel.Text = "BLDC1 KI retrieved.";
               this.E4MainBldc1KiTextBox.Text = string.Format("{0}", motorKi);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get BLDC1 KI.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainBldc1KiButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 motorKi = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.E4MainBldc1KiTextBox.Text, out motorKi) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = (false != this.E4MainBldc1TuningVelocityRadioButton.Checked) ? this.e4Main.Bldc1.SetVelocityKi(motorKi) : this.e4Main.Bldc1.SetPositionKi(motorKi);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC1 KI set to " + motorKi.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC1 KI to " + motorKi.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainBldc1KdButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            Int32 motorKd = 0;
            bool success = (false != this.E4MainBldc1TuningVelocityRadioButton.Checked) ? this.e4Main.Bldc1.GetVelocityKd(ref motorKd) : this.e4Main.Bldc1.GetPositionKd(ref motorKd);

            if (false != success)
            {
               this.StatusLabel.Text = "BLDC1 KD retrieved.";
               this.E4MainBldc1KdTextBox.Text = string.Format("{0}", motorKd);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get BLDC1 KD.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainBldc1KdButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 motorKd = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.E4MainBldc1KdTextBox.Text, out motorKd) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = (false != this.E4MainBldc1TuningVelocityRadioButton.Checked) ? this.e4Main.Bldc1.SetVelocityKd(motorKd) : this.e4Main.Bldc1.SetPositionKd(motorKd);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC1 KD set to " + motorKd.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC1 KD to " + motorKd.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainBldc1PositionWindowButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            UInt32 positionWindow = 0;
            bool success = this.e4Main.Bldc1.GetPositionWindow(ref positionWindow);

            if (false != success)
            {
               this.StatusLabel.Text = "BLDC1 position window retrieved.";
               this.E4MainBldc1PositionWindowTextBox.Text = string.Format("{0}", positionWindow);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get BLDC1 position window.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainBldc1PositionWindowButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt32 positionWindow = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt32.TryParse(this.E4MainBldc1PositionWindowTextBox.Text, out positionWindow) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc1.SetPositionWindow(positionWindow);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC1 position window set to " + positionWindow.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC1 position window to " + positionWindow.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainBldc1PositionWindowTimeButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            UInt16 positionWindowTime = 0;
            bool success = this.e4Main.Bldc1.GetPositionWindowTime(ref positionWindowTime);

            if (false != success)
            {
               this.StatusLabel.Text = "BLDC1 position window time retrieved.";
               this.E4MainBldc1PositionWindowTimeTextBox.Text = string.Format("{0}", positionWindowTime);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get BLDC1 position window time.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainBldc1PositionWindowTimeButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt16 positionWindowTime = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt16.TryParse(this.E4MainBldc1PositionWindowTimeTextBox.Text, out positionWindowTime) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc1.SetPositionWindowTime(positionWindowTime);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC1 position window time set to " + positionWindowTime.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC1 position window time to " + positionWindowTime.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainBldc1VelocityWindowButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            UInt16 velocityWindow = 0;
            bool success = this.e4Main.Bldc1.GetVelocityWindow(ref velocityWindow);

            if (false != success)
            {
               this.StatusLabel.Text = "BLDC1 velocity window retrieved.";
               this.E4MainBldc1VelocityWindowTextBox.Text = string.Format("{0}", velocityWindow);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get BLDC1 velocity window.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainBldc1VelocityWindowButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt16 velocityWindow = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt16.TryParse(this.E4MainBldc1VelocityWindowTextBox.Text, out velocityWindow) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc1.SetVelocityWindow(velocityWindow);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC1 velocity window set to " + velocityWindow.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC1 velocity window to " + velocityWindow.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainBldc1VelocityWindowTimeButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            UInt16 velocityWindowTime = 0;
            bool success = this.e4Main.Bldc1.GetVelocityWindowTime(ref velocityWindowTime);

            if (false != success)
            {
               this.StatusLabel.Text = "BLDC1 velocity window time retrieved.";
               this.E4MainBldc1VelocityWindowTimeTextBox.Text = string.Format("{0}", velocityWindowTime);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get BLDC1 velocity window time.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainBldc1VelocityWindowTimeButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt16 velocityWindowTime = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt16.TryParse(this.E4MainBldc1VelocityWindowTimeTextBox.Text, out velocityWindowTime) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc1.SetVelocityWindowTime(velocityWindowTime);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC1 velocity window time set to " + velocityWindowTime.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC1 velocity window time to " + velocityWindowTime.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainBldc1VelocityThresholdButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            UInt16 velocityThreshold = 0;
            bool success = this.e4Main.Bldc1.GetVelocityThreshold(ref velocityThreshold);

            if (false != success)
            {
               this.StatusLabel.Text = "BLDC1 velocity threshold retrieved.";
               this.E4MainBldc1VelocityThresholdTextBox.Text = string.Format("{0}", velocityThreshold);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get BLDC1 velocity threshold.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainBldc1VelocityThresholdButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt16 velocityThreshold = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt16.TryParse(this.E4MainBldc1VelocityThresholdTextBox.Text, out velocityThreshold) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc1.SetVelocityThreshold(velocityThreshold);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC1 velocity threshold set to " + velocityThreshold.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC1 velocity threshold to " + velocityThreshold.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainBldc1VelocityThresholdTimeButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            UInt16 velocityThresholdTime = 0;
            bool success = this.e4Main.Bldc1.GetVelocityThresholdTime(ref velocityThresholdTime);

            if (false != success)
            {
               this.StatusLabel.Text = "BLDC1 velocity threshold time retrieved.";
               this.E4MainBldc1VelocityThresholdTimeTextBox.Text = string.Format("{0}", velocityThresholdTime);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get BLDC1 velocity time threshold.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainBldc1VelocityThresholdTimeButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt16 velocityThresholdTime = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt16.TryParse(this.E4MainBldc1VelocityThresholdTimeTextBox.Text, out velocityThresholdTime) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Bldc1.SetVelocityThresholdTime(velocityThresholdTime);

            if (false != result)
            {
               this.StatusLabel.Text = "BLDC1 velocity threshold time set to " + velocityThresholdTime.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set BLDC1 velocity time threshold to " + velocityThresholdTime.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #endregion

      #region Stepper 0 Events

      #region General

      private void SetE4MainStepper0ModeButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         MotorComponent.Modes mode = MotorComponent.Modes.undefined;

         if (this.E4MainStepper0ModeComboBox.Text == "off")
         {
            mode = MotorComponent.Modes.off;
         }
         else if (this.E4MainStepper0ModeComboBox.Text == "position")
         {
            mode = MotorComponent.Modes.position;
         }
         else if (this.E4MainStepper0ModeComboBox.Text == "homing")
         {
            mode = MotorComponent.Modes.homing;
         }

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Stepper0.SetMode(mode);

            if (false != result)
            {
               this.StatusLabel.Text = "Stepper0 mode set to " + mode.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Stepper0 mode to " + mode.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void ClearE4MainStepper0FaultButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            this.e4Main.Stepper0.ClearFault();
            this.StatusLabel.Text = "Stepper0 fault cleared.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void HaltE4MainStepper0Button_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Stepper0.Halt();

            if (false != result)
            {
               this.StatusLabel.Text = "Stepper0 halted.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to halt Stepper0.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void RunE4MainStepper0Button_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Stepper0.Run();

            if (false != result)
            {
               this.StatusLabel.Text = "Stepper0 run.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to run Stepper0.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void ReadE4MainStepper0PvtButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            Int32 actualPosition = 0;
            Int32 actualVelocity = 0;
            Int16 actualCurrent = 0;
            bool success = this.e4Main.Stepper0.GetActualPosition(ref actualPosition);
            success = success && this.e4Main.Stepper0.GetActualVelocity(ref actualVelocity);
            success = success && this.e4Main.Stepper0.GetActualCurrent(ref actualCurrent);

            if (false != success)
            {
               this.StatusLabel.Text = "Stepper0 PVT values retrieved.";
               this.E4MainStepper0ActualPositionTextBox.Text = string.Format("{0}", actualPosition);
               this.E4MainStepper0ActualVelocityTextBox.Text = string.Format("{0}", actualVelocity);
               this.E4MainStepper0ActualCurrentTextBox.Text = string.Format("{0}", actualCurrent);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get Stepper0 PVT values.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #region Position Mode

      private void GetE4MainStepper0TargetPositionButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            Int32 targetPosition = 0;
            bool targetPositionRelative = false;
            bool success = this.e4Main.Stepper0.GetTargetPosition(ref targetPosition, ref targetPositionRelative);

            if (false != success)
            {
               this.StatusLabel.Text = "Stepper0 target position retrieved.";
               this.E4MainStepper0TargetPositionTextBox.Text = string.Format("{0}", targetPosition);
               this.E4MainStepper0TargetPositionRelativeCheckBox.Checked = targetPositionRelative;
            }
            else
            {
               this.StatusLabel.Text = "Unable to get Stepper0 target position.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainStepper0TargetPositionButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 targetPosition = 0;
         bool targetPositionRelative = this.E4MainStepper0TargetPositionRelativeCheckBox.Checked;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.E4MainStepper0TargetPositionTextBox.Text, out targetPosition) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Stepper0.SetTargetPosition(targetPosition, targetPositionRelative);

            if (false != result)
            {
               this.StatusLabel.Text = "Stepper0 target position set to " + targetPosition.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Stepper0 target position to " + targetPosition.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainStepper0ProfileAccelerationButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            Int32 profileAcceleration = 0;
            bool success = this.e4Main.Stepper0.GetProfileAcceleration(ref profileAcceleration);

            if (false != success)
            {
               this.StatusLabel.Text = "Stepper0 profile acceleration retrieved.";
               this.E4MainStepper0ProfileAccelerationTextBox.Text = string.Format("{0}", profileAcceleration);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get Stepper0 profile acceleration.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainStepper0ProfileAccelerationButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 profileAcceleration = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.E4MainStepper0ProfileAccelerationTextBox.Text, out profileAcceleration) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Stepper0.SetProfileAcceleration(profileAcceleration);

            if (false != result)
            {
               this.StatusLabel.Text = "Stepper0 profile acceleration set to " + profileAcceleration.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Stepper0 profile acceleration to " + profileAcceleration.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainStepper0ProfileVelocityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            Int32 profileVelocity = 0;
            bool success = this.e4Main.Stepper0.GetProfileVelocity(ref profileVelocity);

            if (false != success)
            {
               this.StatusLabel.Text = "Stepper0 profile velocity retrieved.";
               this.E4MainStepper0ProfileVelocityTextBox.Text = string.Format("{0}", profileVelocity);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get Stepper0 profile velocity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainStepper0ProfileVelocityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 profileVelocity = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.E4MainStepper0ProfileVelocityTextBox.Text, out profileVelocity) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Stepper0.SetProfileVelocity(profileVelocity);

            if (false != result)
            {
               this.StatusLabel.Text = "Stepper0 profile velocity set to " + profileVelocity.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Stepper0 profile velocity to " + profileVelocity.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #region Homing Mode

      private void StartE4MainStepper0HomingButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Stepper0.StartHoming();

            if (false != result)
            {
               this.StatusLabel.Text = "Stepper0 homing started.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to start Stepper0 homing.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void StopE4MainStepper0HomingButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Stepper0.StopHoming();

            if (false != result)
            {
               this.StatusLabel.Text = "Stepper0 homing stopped.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to stop Stepper0 homing.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainStepper0HomingMethodButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            byte homingMethod = 0;
            bool success = this.e4Main.Stepper0.GetHomingMethod(ref homingMethod);

            if (false != success)
            {
               this.StatusLabel.Text = "Stepper0 homing method retrieved.";
               this.E4MainStepper0HomingMethodTextBox.Text = string.Format("{0}", homingMethod);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get Stepper0 homing method.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainStepper0HomingMethodButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         byte homingMethod = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (byte.TryParse(this.E4MainStepper0HomingMethodTextBox.Text, out homingMethod) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Stepper0.SetHomingMethod(homingMethod);

            if (false != result)
            {
               this.StatusLabel.Text = "Stepper0 homing method set to " + homingMethod.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Stepper0 homing method to " + homingMethod.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainStepper0HomingSwitchSpeedButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            UInt32 homingSwitchSpeed = 0;
            bool success = this.e4Main.Stepper0.GetHomingSwitchSpeed(ref homingSwitchSpeed);

            if (false != success)
            {
               this.StatusLabel.Text = "Stepper0 homing switch speed retrieved.";
               this.E4MainStepper0HomingSwitchSpeedTextBox.Text = string.Format("{0}", homingSwitchSpeed);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get Stepper0 homing switch speed.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainStepper0HomingSwitchSpeedButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt32 homingSwitchSpeed = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt32.TryParse(this.E4MainStepper0HomingSwitchSpeedTextBox.Text, out homingSwitchSpeed) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Stepper0.SetHomingSwitchSpeed(homingSwitchSpeed);

            if (false != result)
            {
               this.StatusLabel.Text = "Stepper0 homing switch speed set to " + homingSwitchSpeed.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Stepper0 homing switch speed to " + homingSwitchSpeed.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainStepper0HomingZeroSpeedButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            UInt32 homingZeroSpeed = 0;
            bool success = this.e4Main.Stepper0.GetHomingZeroSpeed(ref homingZeroSpeed);

            if (false != success)
            {
               this.StatusLabel.Text = "Stepper0 homing zero speed retrieved.";
               this.E4MainStepper0HomingZeroSpeedTextBox.Text = string.Format("{0}", homingZeroSpeed);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get Stepper0 homing zero speed.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainStepper0HomingZeroSpeedButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt32 homingZeroSpeed = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt32.TryParse(this.E4MainStepper0HomingZeroSpeedTextBox.Text, out homingZeroSpeed) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Stepper0.SetHomingZeroSpeed(homingZeroSpeed);

            if (false != result)
            {
               this.StatusLabel.Text = "Stepper0 homing zero speed set to " + homingZeroSpeed.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Stepper0 homing zero speed to " + homingZeroSpeed.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainStepper0HomingAccelerationButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            UInt32 homingAcceleration = 0;
            bool success = this.e4Main.Stepper0.GetHomingAcceleration(ref homingAcceleration);

            if (false != success)
            {
               this.StatusLabel.Text = "Stepper0 homing acceleration retrieved.";
               this.E4MainStepper0HomingAccelerationTextBox.Text = string.Format("{0}", homingAcceleration);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get Stepper0 homing acceleration.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainStepper0HomingAccelerationButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt32 homingAcceleration = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt32.TryParse(this.E4MainStepper0HomingAccelerationTextBox.Text, out homingAcceleration) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Stepper0.SetHomingAcceleration(homingAcceleration);

            if (false != result)
            {
               this.StatusLabel.Text = "Stepper0 homing acceleration set to " + homingAcceleration.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Stepper0 homing acceleration to " + homingAcceleration.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainStepper0HomeOffsetButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            Int32 homeOffset = 0;
            bool success = this.e4Main.Stepper0.GetHomeOffset(ref homeOffset);

            if (false != success)
            {
               this.StatusLabel.Text = "Stepper0 home offset retrieved.";
               this.E4MainStepper0HomeOffsetTextBox.Text = string.Format("{0}", homeOffset);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get Stepper0 home offset.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainStepper0HomeOffsetButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 homeOffset = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.E4MainStepper0HomeOffsetTextBox.Text, out homeOffset) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Stepper0.SetHomeOffset(homeOffset);

            if (false != result)
            {
               this.StatusLabel.Text = "Stepper0 home offset set to " + homeOffset.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Stepper0 home offset to " + homeOffset.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #endregion

      #region Stepper 1 Events

      #region General

      private void SetE4MainStepper1ModeButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         MotorComponent.Modes mode = MotorComponent.Modes.undefined;

         if (this.E4MainStepper1ModeComboBox.Text == "off")
         {
            mode = MotorComponent.Modes.off;
         }
         else if (this.E4MainStepper1ModeComboBox.Text == "position")
         {
            mode = MotorComponent.Modes.position;
         }
         else if (this.E4MainStepper1ModeComboBox.Text == "homing")
         {
            mode = MotorComponent.Modes.homing;
         }

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Stepper1.SetMode(mode);

            if (false != result)
            {
               this.StatusLabel.Text = "Stepper1 mode set to " + mode.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Stepper1 mode to " + mode.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void ClearE4MainStepper1FaultButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            this.e4Main.Stepper1.ClearFault();
            this.StatusLabel.Text = "Stepper1 fault cleared.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void HaltE4MainStepper1Button_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Stepper1.Halt();

            if (false != result)
            {
               this.StatusLabel.Text = "Stepper1 halted.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to halt Stepper1.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void RunE4MainStepper1Button_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Stepper1.Run();

            if (false != result)
            {
               this.StatusLabel.Text = "Stepper1 run.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to run Stepper1.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void ReadE4MainStepper1PvtButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            Int32 actualPosition = 0;
            Int32 actualVelocity = 0;
            Int16 actualCurrent = 0;
            bool success = this.e4Main.Stepper1.GetActualPosition(ref actualPosition);
            success = success && this.e4Main.Stepper1.GetActualVelocity(ref actualVelocity);
            success = success && this.e4Main.Stepper1.GetActualCurrent(ref actualCurrent);

            if (false != success)
            {
               this.StatusLabel.Text = "Stepper1 PVT values retrieved.";
               this.E4MainStepper1ActualPositionTextBox.Text = string.Format("{0}", actualPosition);
               this.E4MainStepper1ActualVelocityTextBox.Text = string.Format("{0}", actualVelocity);
               this.E4MainStepper1ActualCurrentTextBox.Text = string.Format("{0}", actualCurrent);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get Stepper1 PVT values.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #region Position Mode

      private void GetE4MainStepper1TargetPositionButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            Int32 targetPosition = 0;
            bool targetPositionRelative = false;
            bool success = this.e4Main.Stepper1.GetTargetPosition(ref targetPosition, ref targetPositionRelative);

            if (false != success)
            {
               this.StatusLabel.Text = "Stepper1 target position retrieved.";
               this.E4MainStepper1TargetPositionTextBox.Text = string.Format("{0}", targetPosition);
               this.E4MainStepper1TargetPositionRelativeCheckBox.Checked = targetPositionRelative;
            }
            else
            {
               this.StatusLabel.Text = "Unable to get Stepper1 target position.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainStepper1TargetPositionButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 targetPosition = 0;
         bool targetPositionRelative = this.E4MainStepper1TargetPositionRelativeCheckBox.Checked;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.E4MainStepper1TargetPositionTextBox.Text, out targetPosition) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Stepper1.SetTargetPosition(targetPosition, targetPositionRelative);

            if (false != result)
            {
               this.StatusLabel.Text = "Stepper1 target position set to " + targetPosition.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Stepper1 target position to " + targetPosition.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainStepper1ProfileAccelerationButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            Int32 profileAcceleration = 0;
            bool success = this.e4Main.Stepper1.GetProfileAcceleration(ref profileAcceleration);

            if (false != success)
            {
               this.StatusLabel.Text = "Stepper1 profile acceleration retrieved.";
               this.E4MainStepper1ProfileAccelerationTextBox.Text = string.Format("{0}", profileAcceleration);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get Stepper1 profile acceleration.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainStepper1ProfileAccelerationButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 profileAcceleration = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.E4MainStepper1ProfileAccelerationTextBox.Text, out profileAcceleration) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Stepper1.SetProfileAcceleration(profileAcceleration);

            if (false != result)
            {
               this.StatusLabel.Text = "Stepper1 profile acceleration set to " + profileAcceleration.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Stepper1 profile acceleration to " + profileAcceleration.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainStepper1ProfileVelocityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            Int32 profileVelocity = 0;
            bool success = this.e4Main.Stepper1.GetProfileVelocity(ref profileVelocity);

            if (false != success)
            {
               this.StatusLabel.Text = "Stepper1 profile velocity retrieved.";
               this.E4MainStepper1ProfileVelocityTextBox.Text = string.Format("{0}", profileVelocity);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get Stepper1 profile velocity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainStepper1ProfileVelocityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 profileVelocity = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.E4MainStepper1ProfileVelocityTextBox.Text, out profileVelocity) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Stepper1.SetProfileVelocity(profileVelocity);

            if (false != result)
            {
               this.StatusLabel.Text = "Stepper1 profile velocity set to " + profileVelocity.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Stepper1 profile velocity to " + profileVelocity.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #region Homing Mode

      private void StartE4MainStepper1HomingButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Stepper1.StartHoming();

            if (false != result)
            {
               this.StatusLabel.Text = "Stepper1 homing started.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to start Stepper1 homing.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void StopE4MainStepper1HomingButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Stepper1.StopHoming();

            if (false != result)
            {
               this.StatusLabel.Text = "Stepper1 homing stopped.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to stop Stepper1 homing.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainStepper1HomingMethodButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            byte homingMethod = 0;
            bool success = this.e4Main.Stepper1.GetHomingMethod(ref homingMethod);

            if (false != success)
            {
               this.StatusLabel.Text = "Stepper1 homing method retrieved.";
               this.E4MainStepper1HomingMethodTextBox.Text = string.Format("{0}", homingMethod);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get Stepper1 homing method.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainStepper1HomingMethodButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         byte homingMethod = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (byte.TryParse(this.E4MainStepper1HomingMethodTextBox.Text, out homingMethod) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Stepper1.SetHomingMethod(homingMethod);

            if (false != result)
            {
               this.StatusLabel.Text = "Stepper1 homing method set to " + homingMethod.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Stepper1 homing method to " + homingMethod.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainStepper1HomingSwitchSpeedButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            UInt32 homingSwitchSpeed = 0;
            bool success = this.e4Main.Stepper1.GetHomingSwitchSpeed(ref homingSwitchSpeed);

            if (false != success)
            {
               this.StatusLabel.Text = "Stepper1 homing switch speed retrieved.";
               this.E4MainStepper1HomingSwitchSpeedTextBox.Text = string.Format("{0}", homingSwitchSpeed);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get Stepper1 homing switch speed.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainStepper1HomingSwitchSpeedButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt32 homingSwitchSpeed = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt32.TryParse(this.E4MainStepper1HomingSwitchSpeedTextBox.Text, out homingSwitchSpeed) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Stepper1.SetHomingSwitchSpeed(homingSwitchSpeed);

            if (false != result)
            {
               this.StatusLabel.Text = "Stepper1 homing switch speed set to " + homingSwitchSpeed.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Stepper1 homing switch speed to " + homingSwitchSpeed.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainStepper1HomingZeroSpeedButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            UInt32 homingZeroSpeed = 0;
            bool success = this.e4Main.Stepper1.GetHomingZeroSpeed(ref homingZeroSpeed);

            if (false != success)
            {
               this.StatusLabel.Text = "Stepper1 homing zero speed retrieved.";
               this.E4MainStepper1HomingZeroSpeedTextBox.Text = string.Format("{0}", homingZeroSpeed);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get Stepper1 homing zero speed.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainStepper1HomingZeroSpeedButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt32 homingZeroSpeed = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt32.TryParse(this.E4MainStepper1HomingZeroSpeedTextBox.Text, out homingZeroSpeed) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Stepper1.SetHomingZeroSpeed(homingZeroSpeed);

            if (false != result)
            {
               this.StatusLabel.Text = "Stepper1 homing zero speed set to " + homingZeroSpeed.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Stepper1 homing zero speed to " + homingZeroSpeed.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainStepper1HomingAccelerationButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            UInt32 homingAcceleration = 0;
            bool success = this.e4Main.Stepper1.GetHomingAcceleration(ref homingAcceleration);

            if (false != success)
            {
               this.StatusLabel.Text = "Stepper1 homing acceleration retrieved.";
               this.E4MainStepper1HomingAccelerationTextBox.Text = string.Format("{0}", homingAcceleration);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get Stepper1 homing acceleration.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainStepper1HomingAccelerationButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt32 homingAcceleration = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt32.TryParse(this.E4MainStepper1HomingAccelerationTextBox.Text, out homingAcceleration) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Stepper1.SetHomingAcceleration(homingAcceleration);

            if (false != result)
            {
               this.StatusLabel.Text = "Stepper1 homing acceleration set to " + homingAcceleration.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Stepper1 homing acceleration to " + homingAcceleration.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetE4MainStepper1HomeOffsetButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.e4Main.NodeId = nodeId;
            Int32 homeOffset = 0;
            bool success = this.e4Main.Stepper1.GetHomeOffset(ref homeOffset);

            if (false != success)
            {
               this.StatusLabel.Text = "Stepper1 home offset retrieved.";
               this.E4MainStepper1HomeOffsetTextBox.Text = string.Format("{0}", homeOffset);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get Stepper1 home offset.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetE4MainStepper1HomeOffsetButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 homeOffset = 0;

         if ((byte.TryParse(this.E4MainActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.E4MainStepper1HomeOffsetTextBox.Text, out homeOffset) != false))
         {
            this.e4Main.NodeId = nodeId;
            bool result = this.e4Main.Stepper1.SetHomeOffset(homeOffset);

            if (false != result)
            {
               this.StatusLabel.Text = "Stepper1 home offset set to " + homeOffset.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set Stepper1 home offset to " + homeOffset.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #endregion

      #region Debug Events

      private void SetE4MainSdoTimeoutButton_Click(object sender, EventArgs e)
      {
         int sdoTimeout = 0;

         if (int.TryParse(this.E4MainSdoTimeoutTextBox.Text, out sdoTimeout) != false)
         {
            this.e4Main.SetCustomComTimeout(sdoTimeout);
            this.StatusLabel.Text = "Main SDO timeout set.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void E4MainDisableFaultResetButton_Click(object sender, EventArgs e)
      {
         this.e4Main.DisableFaultReset();
      }

      private void E4MainClearDeviceFaultButton_Click(object sender, EventArgs e)
      {
         this.e4Main.ClearFault();
         this.StatusLabel.Text = "Main fault cleared.";
      }

      #endregion

      #endregion

      #region Form Events

      private void MainForm_Shown(object sender, EventArgs e)
      {
         this.LoadRegistry();
         this.UpdateTimer.Enabled = true;
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

            if (false != this.ShowHeartbeatCheckBox.Checked)
            {
               this.ControllerTraceTransmit(cobId, heartbeatMsg);
            }
         }

         #endregion

         #region Trace Activity Update

         while (0 != this.traceQueue.Count)
         {
            string activityString = (string)this.traceQueue.Dequeue();
            this.ActivityRichTextBox.AppendText(activityString + "\n");
         }

         #endregion

         #region Download Update

         if (false != this.downloadActive)
         {
            if (false != this.downloadComplete)
            {
               if (null == this.downloadResult)
               {
                  this.StatusLabel.Text = "Download complete.";
               }
               else
               {
                  string downloadStatusString = string.Format("Download error: '{0}'", this.downloadResult);
                  this.StatusLabel.Text = downloadStatusString;
               }

               this.DownloadProgressLabel.Text = "";
               this.DownloadProgressBar.Visible = false;
               this.DownloadFileTextBox.Enabled = true;
               this.DownloadBrowseButton.Enabled = true;
               this.DownloadActivityButton.Text = "Start";
               this.downloadActive = false;
            }
            else
            {
               UInt32 imageSize = 0;
               UInt32 downloadPosition = 0;

               this.downloadDevice.GetImageDownloadStatus(ref imageSize, ref downloadPosition);

               if (0 != imageSize)
               {
                  double progress = downloadPosition * 100 / imageSize;
                  this.DownloadProgressLabel.Text = string.Format("{0:0}%", progress);
                  this.DownloadProgressBar.Value = (int)progress;
               }
            }
         }

         #endregion

         #region Main Update

         this.E4MainMcuTemperatureTextBox.Text = string.Format("{0:0}", this.e4Main.McuTemperature);
         this.E4MainDcLinkVoltageTextBox.Text = string.Format("{0:0}", this.e4Main.DcLinkVoltage);

         this.E4MainMainBoardIcuRollTextBox.Text = string.Format("{0:0.0}", this.e4Main.MainBoardImuRoll);
         this.E4MainMainBoardIcuPitchTextBox.Text = string.Format("{0:0.0}", this.e4Main.MainBoardImuPitch);
         this.E4MainMainBoardIcuYawTextBox.Text = string.Format("{0:0.0}", this.e4Main.MainBoardImuYaw);

         this.E4MainTargetBoardIcuRollTextBox.Text = string.Format("{0:0.0}", this.e4Main.TargetBoardImuRoll);
         this.E4MainTargetBoardIcuPitchTextBox.Text = string.Format("{0:0.0}", this.e4Main.TargetBoardImuPitch);
         this.E4MainTargetBoardIcuYawTextBox.Text = string.Format("{0:0.0}", this.e4Main.TargetBoardImuYaw);

         this.E4MainLaserStatusTextBox.Text = string.Format("{0:X2}", this.e4Main.LaserStatusByte);
         this.E4MainLaserSampleCountTextBox.Text = string.Format("{0:X2}", this.e4Main.LaserSampleNumber);
         this.E4MainLaserDistanceTextBox.Text = string.Format("{0}", this.e4Main.LaserMeasuredDistance);
         this.E4MainLaserScannerTextBox.Text = string.Format("{0:X2}", this.e4Main.ScannerCoordinates);

         this.E4MainBldc0TemperatureTextBox.Text = string.Format("{0:0}", this.e4Main.Bldc0.Temperature);
         this.E4MainBldc0StatusTextBox.Text = string.Format("{0:X4}", this.e4Main.Bldc0.Status);
         this.E4MainBldc0ActualPositionTextBox.Text = string.Format("{0}", this.e4Main.Bldc0.ActualPosition);
         this.E4MainBldc0ActualVelocityTextBox.Text = string.Format("{0}", this.e4Main.Bldc0.ActualVelocity);
         this.E4MainBldc0ActualCurrentTextBox.Text = string.Format("{0}", this.e4Main.Bldc0.ActualCurrent);         
         this.E4MainBldc0PositionAttainedLabel.BackColor = (null != this.e4Main.Warning) ? Color.Yellow : ((false != this.e4Main.Bldc0.PositionAttained) ? Color.LimeGreen : Color.DarkSlateGray); ;
         this.E4MainBldc0VelocityAttainedLabel.BackColor = (null != this.e4Main.Warning) ? Color.Yellow : ((false != this.e4Main.Bldc0.VelocityAttained) ? Color.LimeGreen : Color.DarkSlateGray); ;

         this.E4MainBldc1TemperatureTextBox.Text = string.Format("{0:0}", this.e4Main.Bldc1.Temperature);
         this.E4MainBldc1StatusTextBox.Text = string.Format("{0:X4}", this.e4Main.Bldc1.Status);
         this.E4MainBldc1ActualPositionTextBox.Text = string.Format("{0}", this.e4Main.Bldc1.ActualPosition);
         this.E4MainBldc1ActualVelocityTextBox.Text = string.Format("{0}", this.e4Main.Bldc1.ActualVelocity);
         this.E4MainBldc1ActualCurrentTextBox.Text = string.Format("{0}", this.e4Main.Bldc1.ActualCurrent);
         this.E4MainBldc1PositionAttainedLabel.BackColor = (null != this.e4Main.Warning) ? Color.Yellow : ((false != this.e4Main.Bldc1.PositionAttained) ? Color.LimeGreen : Color.DarkSlateGray); ;
         this.E4MainBldc1VelocityAttainedLabel.BackColor = (null != this.e4Main.Warning) ? Color.Yellow : ((false != this.e4Main.Bldc1.VelocityAttained) ? Color.LimeGreen : Color.DarkSlateGray); ;

         this.E4MainStepper0StatusTextBox.Text = string.Format("{0:X4}", this.e4Main.Stepper0.Status);
         this.E4MainStepper0PositionAttainedLabel.BackColor = (null != this.e4Main.Warning) ? Color.Yellow : ((false != this.e4Main.Stepper0.PositionAttained) ? Color.LimeGreen : Color.DarkSlateGray); ;
         this.E4MainStepper0HomingAttainedLabel.BackColor = (null != this.e4Main.Warning) ? Color.Yellow : ((false != this.e4Main.Stepper0.HomingAttained) ? Color.LimeGreen : Color.DarkSlateGray); ;

         this.E4MainStepper1StatusTextBox.Text = string.Format("{0:X4}", this.e4Main.Stepper1.Status);
         this.E4MainStepper1PositionAttainedLabel.BackColor = (null != this.e4Main.Warning) ? Color.Yellow : ((false != this.e4Main.Stepper1.PositionAttained) ? Color.LimeGreen : Color.DarkSlateGray); ;
         this.E4MainStepper1HomingAttainedLabel.BackColor = (null != this.e4Main.Warning) ? Color.Yellow : ((false != this.e4Main.Stepper1.HomingAttained) ? Color.LimeGreen : Color.DarkSlateGray); ;

         #endregion
      }

      private void MainForm_Activated(object sender, EventArgs e)
      {
         this.RefreshDownloadFileTime();
      }

      private void DownloadTabPage_Enter(object sender, EventArgs e)
      {
         this.RefreshDownloadFileTime();
      }

      private void E4MainBldc0Panel_Scroll(object sender, ScrollEventArgs e)
      {
         this.E4MainActiveNodeLabel.Focus();
         this.E4MainBldc0Panel.VerticalScroll.Value = e.NewValue;
      }

      private void E4MainBldc1Panel_Scroll(object sender, ScrollEventArgs e)
      {
         this.E4MainActiveNodeLabel.Focus();
         this.E4MainBldc1Panel.VerticalScroll.Value = e.NewValue;
      }

      private void E4MainStepper0Panel_Scroll(object sender, ScrollEventArgs e)
      {
         this.E4MainActiveNodeLabel.Focus();
         this.E4MainStepper0Panel.VerticalScroll.Value = e.NewValue;
      }

      private void E4MainStepper1Panel_Scroll(object sender, ScrollEventArgs e)
      {
         this.E4MainActiveNodeLabel.Focus();
         this.E4MainStepper1Panel.VerticalScroll.Value = e.NewValue;
      }

      private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
      {
         this.UpdateTimer.Enabled = false;
         this.SaveRegistry();
      }

      #endregion

      #region Constructor

      public MainForm()
      {
         this.InitializeComponent();
         this.StatusLabel.Text = "";

         this.device = new Device("device", 0);
         this.downloadDevice = new Device("download device", 0);
         this.e4Main = new UlcRoboticsE4Main("main", 0);

         this.busInterface = new BusInterface();
         this.busInterface.AddDevice(this.device);
         this.busInterface.AddDevice(this.downloadDevice);
         this.busInterface.AddDevice(this.e4Main);
                  
         this.traceQueue = new Queue();

         QueuedTraceListener queuedTraceListener = new QueuedTraceListener(this.traceQueue);
         Trace.Listeners.Add(queuedTraceListener);

         Tracer.MaskString = "FFFFFFFF";

         this.ActivityButton.Text = "Start";
         this.active = false;

         this.DownloadProgressLabel.Text = "";
         this.DownloadProgressBar.Visible = false;
         this.DownloadActivityButton.Text = "Start";
         this.downloadActive = false;

         this.HeartbeatActivityButton.Text = "Start";
         this.heartbeatActive = false;

         this.E4MainUsageComboBox.SelectedIndex = 0;

         this.E4MainBldc0ModeComboBox.SelectedIndex = 0;
         this.E4MainBldc0TuningVelocityRadioButton.Checked = true;
         this.E4MainBldc0EditHelperButton.Visible = false;

         this.E4MainBldc1ModeComboBox.SelectedIndex = 0;
         this.E4MainBldc1TuningVelocityRadioButton.Checked = true;
         this.E4MainBldc1EditHelperButton.Visible = false;

         this.E4MainStepper0ModeComboBox.SelectedIndex = 0;
         this.E4MainStepper0EditHelperButton.Visible = false;

         this.E4MainStepper1ModeComboBox.SelectedIndex = 0;
         this.E4MainStepper1EditHelperButton.Visible = false;
      }

      #endregion

   }
}
