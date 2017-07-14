
namespace Weco.DeviceTest
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

   using Weco.CAN;
   using Weco.PCANLight;
   using Weco.Utilities;

   public partial class MainForm : Form
   {
      #region Definition

      private const string RegistryCompanyName = "ULC Robotics";
      private const string RegistryApplicationName = "Weco Device Test";

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
      private UlcRoboticsWecoTrackController trackController;

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
         int top = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : defaultValueTop) : defaultValueTop;

         keyValue = appKey.GetValue("Left");
         int defaultValueLeft = (SystemInformation.PrimaryMonitorSize.Width - this.Width) / 2;
         int left = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : defaultValueLeft) : defaultValueLeft;

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
         keyValue = appKey.GetValue("UsbAUseNodeId");
         bool usbAUseNodeId = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;
         keyValue = appKey.GetValue("UsbANodeId");
         int usbANodeId = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 255) : 255;
         this.usbABusParameters = new UsbBusParameters(BusInterfaces.USBA, usbABaudRate, usbAUseNodeId, usbANodeId, FramesType.INIT_TYPE_ST, TraceGroup.UI, null);

         keyValue = appKey.GetValue("UsbBBaudRate");
         int usbBBaudRate = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 50000) : 50000;
         keyValue = appKey.GetValue("UsbBUseNodeId");
         bool usbBUseNodeId = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;
         keyValue = appKey.GetValue("UsbBNodeId");
         int usbBNodeId = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 255) : 255;
         this.usbBBusParameters = new UsbBusParameters(BusInterfaces.USBB, usbBBaudRate, usbBUseNodeId, usbBNodeId, FramesType.INIT_TYPE_ST, TraceGroup.UI, null);

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


         keyValue = appKey.GetValue("WecoTrackControllerActiveNodeId");
         this.WecoTrackControllerActiveNodeIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";


         keyValue = appKey.GetValue("WecoHubActiveNodeId");
         this.WecoHubActiveNodeIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

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
         appKey.SetValue("UsbAUseNodeId", this.usbABusParameters.UseNodeId ? "1" : "0");
         appKey.SetValue("UsbANodeId", this.usbABusParameters.NodeId.ToString());

         appKey.SetValue("UsbBBaudRate", this.usbBBusParameters.BitRate.ToString());
         appKey.SetValue("UsbBUseNodeId", this.usbBBusParameters.UseNodeId ? "1" : "0");
         appKey.SetValue("UsbBNodeId", this.usbBBusParameters.NodeId.ToString());

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

         appKey.SetValue("WecoTrackControllerActiveNodeId", this.WecoTrackControllerActiveNodeIdTextBox.Text);

         appKey.SetValue("WecoHubActiveNodeId", this.WecoHubActiveNodeIdTextBox.Text);         

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

      private void DownloadStartButton_Click(object sender, EventArgs e)
      {
         byte deviceNodeId = 0;

         if (byte.TryParse(this.DownloadActiveNodeIdTextBox.Text, out deviceNodeId) != false)
         {
            this.downloadDevice.NodeId = deviceNodeId;

            byte[] startData = new byte[1] { 0x02 };
            bool result = this.downloadDevice.SDODownload(0x1F51, 0x00, startData);

            if (false != result)
            {
               this.StatusLabel.Text = "Start triggered.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to trigger start.";
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

      #region Track Controller Events

      #region NMT 

      private void WecoTrackControllerConfigButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.trackController.NodeId = nodeId;

            bool result = this.trackController.Configure();

            if (false != result)
            {
               this.StatusLabel.Text = "Track controller configured.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to configure track controller.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void WecoTrackControllerStartButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.trackController.NodeId = nodeId;

            bool result = this.trackController.Start();

            if (false != result)
            {
               this.StatusLabel.Text = "Track controller started.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to start track controller.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void WecoTrackControllerStopButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.trackController.NodeId = nodeId;
            this.trackController.Stop();
            this.StatusLabel.Text = "Track controller stopped.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void WecoTrackControllerResetButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.trackController.NodeId = nodeId;
            this.trackController.Reset();
            this.StatusLabel.Text = "Track controller reset.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void WecoTrackControllerSyncButton_Click(object sender, EventArgs e)
      {
         this.busInterface.Sync();
      }

      #endregion

      #region Heartbeat

      private void SetWecoTrackControllerConsumerHeartbeatButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         byte consumerHeartbeatNodeId = 0;
         UInt16 consumerHeartbeatTime = 0;

         if ((byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (byte.TryParse(this.WecoTrackControllerConsumerHeartbeatNodeIdTextBox.Text, out consumerHeartbeatNodeId) != false) &&
             (UInt16.TryParse(this.WecoTrackControllerConsumerHeartbeatTimeTextBox.Text, out consumerHeartbeatTime) != false))
         {
            this.trackController.NodeId = nodeId;
            bool result = this.trackController.SetConsumerHeartbeat(consumerHeartbeatTime, consumerHeartbeatNodeId);

            if (false != result)
            {
               this.StatusLabel.Text = "Track controller consumer heartbeat set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set track controller consumer heartbeat.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetWecoTrackControllerProducerHeartbeatButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt16 producerHeartbeatTime = 0;

         if ((byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt16.TryParse(this.WecoTrackControllerProducerHeartbeatTimeTextBox.Text, out producerHeartbeatTime) != false))
         {
            this.trackController.NodeId = nodeId;
            bool result = this.trackController.SetProducerHeartbeat(producerHeartbeatTime);

            if (false != result)
            {
               this.StatusLabel.Text = "Track controller producer heartbeat set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set track controller producer heartbeat.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }
      
      #endregion

      #region LED

      private void GetWecoTrackControllerLedIntensityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false) 
         {
            this.trackController.NodeId = nodeId;
            UInt32 ledIntensityLevel = 0;
            bool success = this.trackController.GetLedIntensityLevel(ref ledIntensityLevel);

            if (false != success)
            {
               this.StatusLabel.Text = "Track controller LED intensity retrieved.";
               this.WecoTrackControllerLedIntensityTextBox.Text = string.Format("{0}", ledIntensityLevel);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get track controller LED intensity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetWecoTrackControllerLedIntensityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt32 ledIntensityLevel = 0;

         if ((byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt32.TryParse(this.WecoTrackControllerLedIntensityTextBox.Text, out ledIntensityLevel) != false))
         {
            this.trackController.NodeId = nodeId;
            bool result = this.trackController.SetLedIntensityLevel(ledIntensityLevel);

            if (false != result)
            {
               this.StatusLabel.Text = "Track controller LED intensity set to " + ledIntensityLevel.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set track controller LED intensity set to " + ledIntensityLevel.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void DecreaseWecoTrackControllerLedIntensityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt32 ledIntensityLevel = 0;

         if ((byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt32.TryParse(this.WecoTrackControllerLedIntensityTextBox.Text, out ledIntensityLevel) != false))
         {
            this.trackController.NodeId = nodeId;
            ledIntensityLevel = (byte)((ledIntensityLevel >= 8) ? (ledIntensityLevel - 8) : 0);
            bool result = this.trackController.SetLedIntensityLevel(ledIntensityLevel);

            if (false != result)
            {
               this.StatusLabel.Text = "Track controller light LED intensity decreased.";
               this.WecoTrackControllerLedIntensityTextBox.Text = ledIntensityLevel.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to decrease track controller LED intensity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void IncreaseWecoTrackControllerLedIntensityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt32 ledIntensityLevel = 0;

         if ((byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt32.TryParse(this.WecoTrackControllerLedIntensityTextBox.Text, out ledIntensityLevel) != false))
         {
            this.trackController.NodeId = nodeId;
            ledIntensityLevel = (byte)((ledIntensityLevel <= 0xFFFFFFFF) ? (ledIntensityLevel + 8) : 0xFFFFFFFF);
            bool result = this.trackController.SetLedIntensityLevel(ledIntensityLevel);

            if (false != result)
            {
               this.StatusLabel.Text = "Track controller light LED intensity increased.";
               this.WecoTrackControllerLedIntensityTextBox.Text = ledIntensityLevel.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to increase track controller LED intensity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetWecoTrackControllerLedChannelMaskButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.trackController.NodeId = nodeId;
            byte ledChannelMask = 0;
            bool success = this.trackController.GetLedChannelMask(ref ledChannelMask);

            if (false != success)
            {
               this.StatusLabel.Text = "Track controller LED channel mask retrieved.";
               this.WecoTrackControllerLedChannelMaskTextBox.Text = string.Format("{0:X2}", ledChannelMask);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get track controller LED channel mask.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetWecoTrackControllerLedChannelMaskButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         byte ledChannelMask = 0;

         if ((byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (byte.TryParse(this.WecoTrackControllerLedChannelMaskTextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out ledChannelMask) != false))
         {
            this.trackController.NodeId = nodeId;
            bool result = this.trackController.SetLedChannelMask(ledChannelMask);

            if (false != result)
            {
               this.StatusLabel.Text = "Track controller LED channel mask set set to " + ledChannelMask.ToString("X2") + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set track controller LED channel mask set to " + ledChannelMask.ToString("X2") + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }      

      #endregion

      #region Track Motor Events

      #region General

      private void SetWecoTrackControllerTrackMotorModeButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         MotorComponent.Modes mode = MotorComponent.Modes.undefined;

         if (this.WecoTrackControllerTrackMotorModeComboBox.Text == "off")
         {
            mode = MotorComponent.Modes.off;
         }
         else if (this.WecoTrackControllerTrackMotorModeComboBox.Text == "position")
         {
            mode = MotorComponent.Modes.position;
         }
         else if (this.WecoTrackControllerTrackMotorModeComboBox.Text == "velocity")
         {
            mode = MotorComponent.Modes.velocity;
         }

         if (byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.trackController.NodeId = nodeId;
            bool result = this.trackController.TrackMotor.SetMode(mode);

            if (false != result)
            {
               this.StatusLabel.Text = "Track motor mode set to " + mode.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set track motor mode to " + mode.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void ClearWecoTrackControllerTrackMotorFaultButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.trackController.NodeId = nodeId;
            this.trackController.TrackMotor.ClearFault();
            this.StatusLabel.Text = "Track motor fault cleared.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void HaltWecoTrackControllerTrackMotorButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.trackController.NodeId = nodeId;
            bool result = this.trackController.TrackMotor.Halt();

            if (false != result)
            {
               this.StatusLabel.Text = "Track motor halted.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to halt track motor.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void RunWecoTrackControllerTrackMotorButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.trackController.NodeId = nodeId;
            bool result = this.trackController.TrackMotor.Run();

            if (false != result)
            {
               this.StatusLabel.Text = "Track motor run.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to run track motor.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #region Position Mode

      private void GetWecoTrackControllerTrackMotorTargetPositionButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.trackController.NodeId = nodeId;
            Int32 targetPosition = 0;
            bool targetPositionRelative = false;
            bool success = this.trackController.TrackMotor.GetTargetPosition(ref targetPosition, ref targetPositionRelative);

            if (false != success)
            {
               this.StatusLabel.Text = "Track motor target position retrieved.";
               this.WecoTrackControllerTrackMotorTargetPositionTextBox.Text = string.Format("{0}", targetPosition);
               this.WecoTrackControllerTrackMotorTargetPositionRelativeCheckBox.Checked = targetPositionRelative;
            }
            else
            {
               this.StatusLabel.Text = "Unable to get track motor target position.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetWecoTrackControllerTrackMotorTargetPositionButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 targetPosition = 0;
         bool targetPositionRelative = this.WecoTrackControllerTrackMotorTargetPositionRelativeCheckBox.Checked;

         if ((byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.WecoTrackControllerTrackMotorTargetPositionTextBox.Text, out targetPosition) != false))
         {
            this.trackController.NodeId = nodeId;
            bool result = this.trackController.TrackMotor.SetTargetPosition(targetPosition, targetPositionRelative);

            if (false != result)
            {
               this.StatusLabel.Text = "Track motor target position set to " + targetPosition.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set track motor target position to " + targetPosition.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void ScheduleWecoTrackControllerTrackMotorTargetPositionButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 targetPosition = 0;

         if ((byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.WecoTrackControllerTrackMotorTargetPositionTextBox.Text, out targetPosition) != false))
         {
            this.trackController.NodeId = nodeId;
            bool result = this.trackController.TrackMotor.ScheduleTargetPosition(targetPosition);

            if (false != result)
            {
               this.StatusLabel.Text = "Track motor target position scheduled for " + targetPosition.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to schedule track motor position for " + targetPosition.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetWecoTrackControllerTrackMotorProfileVelocityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.trackController.NodeId = nodeId;
            Int32 profileVelocity = 0;
            bool success = this.trackController.TrackMotor.GetProfileVelocity(ref profileVelocity);

            if (false != success)
            {
               this.StatusLabel.Text = "Track motor profile velocity retrieved.";
               this.WecoTrackControllerTrackMotorProfileVelocityTextBox.Text = string.Format("{0}", profileVelocity);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get track motor profile velocity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetWecoTrackControllerTrackMotorProfileVelocityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 profileVelocity = 0;

         if ((byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.WecoTrackControllerTrackMotorProfileVelocityTextBox.Text, out profileVelocity) != false))
         {
            this.trackController.NodeId = nodeId;
            bool result = this.trackController.TrackMotor.SetProfileVelocity(profileVelocity);

            if (false != result)
            {
               this.StatusLabel.Text = "Track motor profile velocity set to " + profileVelocity.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set track motor profile velocity to " + profileVelocity.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #region Velocity Mode

      private void GetWecoTrackControllerTrackMotorTargetVelocityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.trackController.NodeId = nodeId;
            Int32 targetVelocity = 0;
            bool success = this.trackController.TrackMotor.GetTargetVelocity(ref targetVelocity);

            if (false != success)
            {
               this.StatusLabel.Text = "Track motor target velocity retrieved.";
               this.WecoTrackControllerTrackMotorTargetVelocityTextBox.Text = string.Format("{0}", targetVelocity);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get track motor target velocity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetWecoTrackControllerTrackMotorTargetVelocityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 targetVelocity = 0;

         if ((byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.WecoTrackControllerTrackMotorTargetVelocityTextBox.Text, out targetVelocity) != false))
         {
            this.trackController.NodeId = nodeId;
            bool result = this.trackController.TrackMotor.SetTargetVelocity(targetVelocity);

            if (false != result)
            {
               this.StatusLabel.Text = "Track motor target velocity set to " + targetVelocity.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set track motor target velocity to " + targetVelocity.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void ScheduleWecoTrackControllerTrackMotorTargetVelocityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 targetVelocity = 0;

         if ((byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.WecoTrackControllerTrackMotorTargetVelocityTextBox.Text, out targetVelocity) != false))
         {
            this.trackController.NodeId = nodeId;
            bool result = this.trackController.TrackMotor.ScheduleTargetVelocity(targetVelocity);

            if (false != result)
            {
               this.StatusLabel.Text = "Track motor target velocity scheduled for " + targetVelocity.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to schedule track motor velocity for " + targetVelocity.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetWecoTrackControllerTrackMotorProfileAccelerationButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.trackController.NodeId = nodeId;
            Int32 profileAcceleration = 0;
            bool success = this.trackController.TrackMotor.GetProfileAcceleration(ref profileAcceleration);

            if (false != success)
            {
               this.StatusLabel.Text = "Track motor profile acceleration retrieved.";
               this.WecoTrackControllerTrackMotorProfileAccelerationTextBox.Text = string.Format("{0}", profileAcceleration);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get track motor profile acceleration.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetWecoTrackControllerTrackMotorProfileAccelerationButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 profileAcceleration = 0;

         if ((byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.WecoTrackControllerTrackMotorProfileAccelerationTextBox.Text, out profileAcceleration) != false))
         {
            this.trackController.NodeId = nodeId;
            bool result = this.trackController.TrackMotor.SetProfileAcceleration(profileAcceleration);

            if (false != result)
            {
               this.StatusLabel.Text = "Track motor profile acceleration set to " + profileAcceleration.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set track motor profile acceleration to " + profileAcceleration.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetWecoTrackControllerTrackMotorProfileDecelerationButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.trackController.NodeId = nodeId;
            Int32 profileDeceleration = 0;
            bool success = this.trackController.TrackMotor.GetProfileDeceleration(ref profileDeceleration);

            if (false != success)
            {
               this.StatusLabel.Text = "Track motor profile deceleration retrieved.";
               this.WecoTrackControllerTrackMotorProfileDecelerationTextBox.Text = string.Format("{0}", profileDeceleration);
            }       
            else
            {
               this.StatusLabel.Text = "Unable to get track motor profile deceleration.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetWecoTrackControllerTrackMotorProfileDecelerationButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 profileDeceleration = 0;

         if ((byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.WecoTrackControllerTrackMotorProfileDecelerationTextBox.Text, out profileDeceleration) != false))
         {
            this.trackController.NodeId = nodeId;
            bool result = this.trackController.TrackMotor.SetProfileDeceleration(profileDeceleration);

            if (false != result)
            {
               this.StatusLabel.Text = "Track motor profile deceleration set to " + profileDeceleration.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set track motor profile deceleration to " + profileDeceleration.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #region Tuning

      private void GetWecoTrackControllerTrackMotorKpButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.trackController.NodeId = nodeId;
            Int32 motorKp = 0;
            bool success = (false != this.WecoTrackControllerTrackMotorTuningVelocityRadioButton.Checked) ? this.trackController.TrackMotor.GetVelocityKp(ref motorKp) : this.trackController.TrackMotor.GetPositionKp(ref motorKp);

            if (false != success)
            {
               this.StatusLabel.Text = "Track motor KP retrieved.";
               this.WecoTrackControllerTrackMotorKpTextBox.Text = string.Format("{0}", motorKp);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get track motor KP.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetWecoTrackControllerTrackMotorKpButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 motorKp = 0;

         if ((byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.WecoTrackControllerTrackMotorKpTextBox.Text, out motorKp) != false))
         {
            this.trackController.NodeId = nodeId;
            bool result = (false != this.WecoTrackControllerTrackMotorTuningVelocityRadioButton.Checked) ? this.trackController.TrackMotor.SetVelocityKp(motorKp) : this.trackController.TrackMotor.SetPositionKp(motorKp);

            if (false != result)
            {
               this.StatusLabel.Text = "Track motor KP set to " + motorKp.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set track motor KP to " + motorKp.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetWecoTrackControllerTrackMotorKiButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.trackController.NodeId = nodeId;
            Int32 motorKi = 0;
            bool success = (false != this.WecoTrackControllerTrackMotorTuningVelocityRadioButton.Checked) ? this.trackController.TrackMotor.GetVelocityKi(ref motorKi) : this.trackController.TrackMotor.GetPositionKi(ref motorKi);

            if (false != success)
            {
               this.StatusLabel.Text = "Track motor KI retrieved.";
               this.WecoTrackControllerTrackMotorKiTextBox.Text = string.Format("{0}", motorKi);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get track motor KI.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetWecoTrackControllerTrackMotorKiButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 motorKi = 0;

         if ((byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.WecoTrackControllerTrackMotorKiTextBox.Text, out motorKi) != false))
         {
            this.trackController.NodeId = nodeId;
            bool result = (false != this.WecoTrackControllerTrackMotorTuningVelocityRadioButton.Checked) ? this.trackController.TrackMotor.SetVelocityKi(motorKi) : this.trackController.TrackMotor.SetPositionKi(motorKi);

            if (false != result)
            {
               this.StatusLabel.Text = "Track motor KI set to " + motorKi.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set track motor KI to " + motorKi.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetWecoTrackControllerTrackMotorKdButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.trackController.NodeId = nodeId;
            Int32 motorKd = 0;
            bool success = (false != this.WecoTrackControllerTrackMotorTuningVelocityRadioButton.Checked) ? this.trackController.TrackMotor.GetVelocityKd(ref motorKd) : this.trackController.TrackMotor.GetPositionKd(ref motorKd);

            if (false != success)
            {
               this.StatusLabel.Text = "Track motor KD retrieved.";
               this.WecoTrackControllerTrackMotorKdTextBox.Text = string.Format("{0}", motorKd);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get track motor KD.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetWecoTrackControllerTrackMotorKdButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 motorKd = 0;

         if ((byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.WecoTrackControllerTrackMotorKdTextBox.Text, out motorKd) != false))
         {
            this.trackController.NodeId = nodeId;
            bool result = (false != this.WecoTrackControllerTrackMotorTuningVelocityRadioButton.Checked) ? this.trackController.TrackMotor.SetVelocityKd(motorKd) : this.trackController.TrackMotor.SetPositionKd(motorKd);

            if (false != result)
            {
               this.StatusLabel.Text = "Track motor KD set to " + motorKd.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set track motor KD to " + motorKd.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetWecoTrackControllerTrackMotorPositionWindowButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.trackController.NodeId = nodeId;
            UInt32 positionWindow = 0;
            bool success = this.trackController.TrackMotor.GetPositionWindow(ref positionWindow);

            if (false != success)
            {
               this.StatusLabel.Text = "Track motor position window retrieved.";
               this.WecoTrackControllerTrackMotorPositionWindowTextBox.Text = string.Format("{0}", positionWindow);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get track motor position window.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetWecoTrackControllerTrackMotorPositionWindowButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt32 positionWindow = 0;

         if ((byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt32.TryParse(this.WecoTrackControllerTrackMotorPositionWindowTextBox.Text, out positionWindow) != false))
         {
            this.trackController.NodeId = nodeId;
            bool result = this.trackController.TrackMotor.SetPositionWindow(positionWindow);

            if (false != result)
            {
               this.StatusLabel.Text = "Track motor position window set to " + positionWindow.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set track motor position window to " + positionWindow.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetWecoTrackControllerTrackMotorPositionWindowTimeButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.trackController.NodeId = nodeId;
            UInt16 positionWindowTime = 0;
            bool success = this.trackController.TrackMotor.GetPositionWindowTime(ref positionWindowTime);

            if (false != success)
            {
               this.StatusLabel.Text = "Track motor position window time retrieved.";
               this.WecoTrackControllerTrackMotorPositionWindowTimeTextBox.Text = string.Format("{0}", positionWindowTime);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get track motor position window time.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetWecoTrackControllerTrackMotorPositionWindowTimeButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt16 positionWindowTime = 0;

         if ((byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt16.TryParse(this.WecoTrackControllerTrackMotorPositionWindowTimeTextBox.Text, out positionWindowTime) != false))
         {
            this.trackController.NodeId = nodeId;
            bool result = this.trackController.TrackMotor.SetPositionWindowTime(positionWindowTime);

            if (false != result)
            {
               this.StatusLabel.Text = "Track motor position window time set to " + positionWindowTime.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set track motor position window time to " + positionWindowTime.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetWecoTrackControllerTrackMotorVelocityWindowButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.trackController.NodeId = nodeId;
            UInt16 velocityWindow = 0;
            bool success = this.trackController.TrackMotor.GetVelocityWindow(ref velocityWindow);

            if (false != success)
            {
               this.StatusLabel.Text = "Track motor velocity window retrieved.";
               this.WecoTrackControllerTrackMotorVelocityWindowTextBox.Text = string.Format("{0}", velocityWindow);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get track motor velocity window.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetWecoTrackControllerTrackMotorVelocityWindowButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt16 velocityWindow = 0;

         if ((byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt16.TryParse(this.WecoTrackControllerTrackMotorVelocityWindowTextBox.Text, out velocityWindow) != false))
         {
            this.trackController.NodeId = nodeId;
            bool result = this.trackController.TrackMotor.SetVelocityWindow(velocityWindow);

            if (false != result)
            {
               this.StatusLabel.Text = "Track motor velocity window set to " + velocityWindow.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set track motor velocity window to " + velocityWindow.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetWecoTrackControllerTrackMotorVelocityWindowTimeButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.trackController.NodeId = nodeId;
            UInt16 velocityWindowTime = 0;
            bool success = this.trackController.TrackMotor.GetVelocityWindowTime(ref velocityWindowTime);

            if (false != success)
            {
               this.StatusLabel.Text = "Track motor velocity window time retrieved.";
               this.WecoTrackControllerTrackMotorVelocityWindowTimeTextBox.Text = string.Format("{0}", velocityWindowTime);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get track motor velocity window time.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetWecoTrackControllerTrackMotorVelocityWindowTimeButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt16 velocityWindowTime = 0;

         if ((byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt16.TryParse(this.WecoTrackControllerTrackMotorVelocityWindowTimeTextBox.Text, out velocityWindowTime) != false))
         {
            this.trackController.NodeId = nodeId;
            bool result = this.trackController.TrackMotor.SetVelocityWindowTime(velocityWindowTime);

            if (false != result)
            {
               this.StatusLabel.Text = "Track motor velocity window time set to " + velocityWindowTime.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set track motor velocity window time to " + velocityWindowTime.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetWecoTrackControllerTrackMotorVelocityThresholdButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.trackController.NodeId = nodeId;
            UInt16 velocityThreshold = 0;
            bool success = this.trackController.TrackMotor.GetVelocityThreshold(ref velocityThreshold);

            if (false != success)
            {
               this.StatusLabel.Text = "Track motor velocity threshold retrieved.";
               this.WecoTrackControllerTrackMotorVelocityThresholdTextBox.Text = string.Format("{0}", velocityThreshold);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get track motor velocity threshold.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetWecoTrackControllerTrackMotorVelocityThresholdButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt16 velocityThreshold = 0;

         if ((byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt16.TryParse(this.WecoTrackControllerTrackMotorVelocityThresholdTextBox.Text, out velocityThreshold) != false))
         {
            this.trackController.NodeId = nodeId;
            bool result = this.trackController.TrackMotor.SetVelocityThreshold(velocityThreshold);

            if (false != result)
            {
               this.StatusLabel.Text = "Track motor velocity threshold set to " + velocityThreshold.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set track motor velocity threshold to " + velocityThreshold.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetWecoTrackControllerTrackMotorVelocityThresholdTimeButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.trackController.NodeId = nodeId;
            UInt16 velocityThresholdTime = 0;
            bool success = this.trackController.TrackMotor.GetVelocityThresholdTime(ref velocityThresholdTime);

            if (false != success)
            {
               this.StatusLabel.Text = "Track motor velocity threshold time retrieved.";
               this.WecoTrackControllerTrackMotorVelocityThresholdTimeTextBox.Text = string.Format("{0}", velocityThresholdTime);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get track motor velocity time threshold.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetWecoTrackControllerTrackMotorVelocityThresholdTimeButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt16 velocityThresholdTime = 0;

         if ((byte.TryParse(this.WecoTrackControllerActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt16.TryParse(this.WecoTrackControllerTrackMotorVelocityThresholdTimeTextBox.Text, out velocityThresholdTime) != false))
         {
            this.trackController.NodeId = nodeId;
            bool result = this.trackController.TrackMotor.SetVelocityThresholdTime(velocityThresholdTime);

            if (false != result)
            {
               this.StatusLabel.Text = "Track motor velocity threshold time set to " + velocityThresholdTime.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set track motor velocity time threshold to " + velocityThresholdTime.ToString() + ".";
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

      private void SetWecoTrackControllerSdoTimeoutButton_Click(object sender, EventArgs e)
      {
         int sdoTimeout = 0;

         if (int.TryParse(this.WecoTrackControllerSdoTimeoutTextBox.Text, out sdoTimeout) != false)
         {
            this.trackController.SetCustomComTimeout(sdoTimeout);
            this.StatusLabel.Text = "Track controller SDO timeout set.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void DisableWecoTrackControllerFaultResetButton_Click(object sender, EventArgs e)
      {
         this.trackController.DisableFaultReset();
         this.StatusLabel.Text = "Track controller fault reset disabled.";
      }

      private void ClearWecoTrackControllerDeviceFaultButton_Click(object sender, EventArgs e)
      {
         this.trackController.ClearFault();
         this.StatusLabel.Text = "Track controller fault cleared.";
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

         this.WecoTrackControllerMcuTemperatureTextBox.Text = string.Format("{0:0}", this.trackController.McuTemperature);

         this.WecoTrackControllerTrackMotorTemperatureTextBox.Text = string.Format("{0:0}", this.trackController.TrackMotor.Temperature);
         this.WecoTrackControllerTrackMotorStatusTextBox.Text = string.Format("{0:X4}", this.trackController.TrackMotor.Status);
         this.WecoTrackControllerTrackMotorActualPositionTextBox.Text = string.Format("{0}", this.trackController.TrackMotor.ActualPosition);
         this.WecoTrackControllerTrackMotorActualVelocityTextBox.Text = string.Format("{0}", this.trackController.TrackMotor.ActualVelocity);
         this.WecoTrackControllerTrackMotorActualCurrentTextBox.Text = string.Format("{0}", this.trackController.TrackMotor.ActualCurrent);
         this.WecoTrackControllerTrackMotorPositionAttainedLabel.BackColor = (null != this.trackController.Warning) ? Color.Yellow : ((false != this.trackController.TrackMotor.PositionAttained) ? Color.LimeGreen : Color.DarkSlateGray); ;
         this.WecoTrackControllerTrackMotorVelocityAttainedLabel.BackColor = (null != this.trackController.Warning) ? Color.Yellow : ((false != this.trackController.TrackMotor.VelocityAttained) ? Color.LimeGreen : Color.DarkSlateGray); ;

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

      private void WecoTrackControllerTrackMotorPanel_Scroll(object sender, ScrollEventArgs e)
      {
         this.WecoTrackControllerActiveNodeLabel.Focus();
         this.WecoTrackControllerTrackMotorPanel.VerticalScroll.Value = e.NewValue;
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
         this.trackController = new UlcRoboticsWecoTrackController("track controller", 0);

         this.busInterface = new BusInterface();
         this.busInterface.AddDevice(this.device);
         this.busInterface.AddDevice(this.downloadDevice);
         this.busInterface.AddDevice(this.trackController);
                  
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

         this.WecoTrackControllerTrackMotorModeComboBox.SelectedIndex = 0;
         this.WecoTrackControllerTrackMotorTuningVelocityRadioButton.Checked = true;
         this.WecoTrackControllerTrackMotorEditHelperButton.Visible = false;
      }

      #endregion

   }
}
