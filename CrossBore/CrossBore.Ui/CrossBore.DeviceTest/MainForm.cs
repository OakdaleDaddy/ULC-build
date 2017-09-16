
namespace CrossBore.DeviceTest
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

   using CrossBore.CAN;
   using CrossBore.PCANLight;
   using CrossBore.Utilities;

   public partial class MainForm : Form
   {
      #region Definition

      private const string RegistryCompanyName = "ULC Robotics";
      private const string RegistryApplicationName = "Weko Device Test";

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
      private UlcRoboticsWekoLaunchCard launchCard;
      private PeakDigitalIo digitalIo;
      private UlcRoboticsCrossBoreSensor crossBoreSensor;

      private byte heartbeatNodeId;
      private bool heartbeatActive;
      private int heartbeatTime;
      private DateTime heartbeatTimeLimit;

      private bool syncActive;
      private int syncTime;
      private DateTime syncTimeLimit;

      private bool downloadActive;
      private bool downloadComplete;
      private string downloadResult;

      private byte digitalIoInputs;

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


         keyValue = appKey.GetValue("AutoSyncTime");
         this.AutoSyncTimeTextBox.Text = (null != keyValue) ? keyValue.ToString() : "1000";


         keyValue = appKey.GetValue("DownloadActiveNodeId");
         this.DownloadActiveNodeIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue("DownloadFile");
         this.DownloadFileTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";


         keyValue = appKey.GetValue("PeakDigitalIoActiveNodeId");
         this.DigitalIoActiveNodeIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";


         keyValue = appKey.GetValue("WekoLaunchCardActiveNodeId");
         this.WekoLaunchCardActiveNodeIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";


         keyValue = appKey.GetValue("CrossBoreSensorActiveNodeId");
         this.CrossBoreSensorActiveNodeIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";
         
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

         appKey.SetValue("AutoSyncTime", this.AutoSyncTimeTextBox.Text);
         
         appKey.SetValue("DownloadActiveNodeId", this.DownloadActiveNodeIdTextBox.Text);
         appKey.SetValue("DownloadFile", this.DownloadFileTextBox.Text);

         appKey.SetValue("PeakDigitalIoActiveNodeId", this.DigitalIoActiveNodeIdTextBox.Text);

         appKey.SetValue("WekoLaunchCardActiveNodeId", this.WekoLaunchCardActiveNodeIdTextBox.Text);

         appKey.SetValue("CrossBoreSensorActiveNodeId", this.CrossBoreSensorActiveNodeIdTextBox.Text);

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

      private void DigitalIoChangeHandler(int nodeId, byte value)
      {
         this.digitalIoInputs = value;
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

      private void AutoSyncCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         if (false != this.AutoSyncCheckBox.Checked)
         {
            int time = 0;

            if (int.TryParse(this.AutoSyncTimeTextBox.Text, out time) != false)
            {
               this.syncActive = true;
               this.syncTime = time;
               this.syncTimeLimit = DateTime.Now.AddMilliseconds(this.syncTime);
               this.busInterface.Sync();
            }
            else
            {
               this.AutoSyncCheckBox.Checked = false;
            }
         }
         else
         {
            this.syncActive = false;
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

      private void DownloadStartBootloaderButton_Click(object sender, EventArgs e)
      {
         byte deviceNodeId = 0;

         if (byte.TryParse(this.DownloadActiveNodeIdTextBox.Text, out deviceNodeId) != false)
         {
            this.downloadDevice.NodeId = deviceNodeId;

            byte[] startData = new byte[1] { 0x00 };
            bool result = this.downloadDevice.SDODownload(0x1F51, 0x01, startData);

            if (false != result)
            {
               this.StatusLabel.Text = "Start bootloader triggered.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to trigger bootloader start.";
               this.DeviceUploadHexDataTextBox.Text = "";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void DownloadStartApplicationButton_Click(object sender, EventArgs e)
      {
         byte deviceNodeId = 0;

         if (byte.TryParse(this.DownloadActiveNodeIdTextBox.Text, out deviceNodeId) != false)
         {
            this.downloadDevice.NodeId = deviceNodeId;

            byte[] startData = new byte[1] { 0x01 };
            bool result = this.downloadDevice.SDODownload(0x1F51, 0x01, startData);

            if (false != result)
            {
               this.StatusLabel.Text = "Start application triggered.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to trigger application start.";
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
                  this.DownloadActivityButton.Text = "Stop Download";
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
            this.DownloadActivityButton.Text = "Start Download";

            this.StatusLabel.Text = "Image download stopped.";
         }
      }

      #endregion

      #region Launch Card Events

      private void WekoLaunchCardConfigButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.WekoLaunchCardActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.launchCard.NodeId = nodeId;

            bool result = this.launchCard.Configure();

            if (false != result)
            {
               this.StatusLabel.Text = "Launch card configured.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to configure launch card.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void WekoLaunchCardStartButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.WekoLaunchCardActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.launchCard.NodeId = nodeId;

            bool result = this.launchCard.Start();

            if (false != result)
            {
               this.StatusLabel.Text = "Launch card controller started.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to start launch card.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void WekoLaunchCardStopButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.WekoLaunchCardActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.launchCard.NodeId = nodeId;
            this.launchCard.Stop();
            this.StatusLabel.Text = "Launch card stopped.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void WekoLaunchCardResetButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.WekoLaunchCardActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.launchCard.NodeId = nodeId;
            this.launchCard.Reset();
            this.StatusLabel.Text = "Launch card reset.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetWekoLaunchCardConsumerHeartbeatButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         byte consumerHeartbeatNodeId = 0;
         UInt16 consumerHeartbeatTime = 0;

         if ((byte.TryParse(this.WekoLaunchCardActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (byte.TryParse(this.WekoLaunchCardConsumerHeartbeatNodeIdTextBox.Text, out consumerHeartbeatNodeId) != false) &&
             (UInt16.TryParse(this.WekoLaunchCardConsumerHeartbeatTimeTextBox.Text, out consumerHeartbeatTime) != false))
         {
            this.launchCard.NodeId = nodeId;
            bool result = this.launchCard.SetConsumerHeartbeat(consumerHeartbeatTime, consumerHeartbeatNodeId);

            if (false != result)
            {
               this.StatusLabel.Text = "Launch card consumer heartbeat set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set launch card consumer heartbeat.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetWekoLaunchCardProducerHeartbeatButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt16 producerHeartbeatTime = 0;

         if ((byte.TryParse(this.WekoLaunchCardActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt16.TryParse(this.WekoLaunchCardProducerHeartbeatTimeTextBox.Text, out producerHeartbeatTime) != false))
         {
            this.launchCard.NodeId = nodeId;
            bool result = this.launchCard.SetProducerHeartbeat(producerHeartbeatTime);

            if (false != result)
            {
               this.StatusLabel.Text = "Launch card producer heartbeat set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set launch card producer heartbeat.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetWekoLaunchCardCameraSelectButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.WekoLaunchCardActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.launchCard.NodeId = nodeId;
            byte cameraSelect = 0;
            bool success = this.launchCard.GetCameraSelect(ref cameraSelect);

            if (false != success)
            {
               this.StatusLabel.Text = "Launch card camera select retrieved.";
               this.WekoLaunchCardCameraSelectTextBox.Text = string.Format("{0}", cameraSelect);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get launch card camera select.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetWekoLaunchCardCameraSelectButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         byte cameraSelect = 0;

         if ((byte.TryParse(this.WekoLaunchCardActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (byte.TryParse(this.WekoLaunchCardCameraSelectTextBox.Text, out cameraSelect) != false))
         {
            this.launchCard.NodeId = nodeId;
            bool result = this.launchCard.SetCameraSelect(cameraSelect);

            if (false != result)
            {
               this.StatusLabel.Text = "Launch card camera select set to " + cameraSelect.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set launch card camera select";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetWekoLaunchCardCameraLedIntensityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         byte ledSelect = 0;

         if ((byte.TryParse(this.WekoLaunchCardActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (byte.TryParse(this.WekoLaunchCardLedSelectTextBox.Text, out ledSelect) != false))
         {
            this.launchCard.NodeId = nodeId;
            UInt32 ledIntensityLevel = 0;
            bool success = this.launchCard.GetLedIntensityLevel(ledSelect, ref ledIntensityLevel);

            if (false != success)
            {
               this.StatusLabel.Text = "Launch card LED intensity retrieved.";
               this.WekoLaunchCardLedIntensityTextBox.Text = string.Format("{0}", ledIntensityLevel);
            }
            else
            {
               this.StatusLabel.Text = "Unable to get launch card LED intensity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetWekoLaunchCardCameraLedIntensityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         byte ledSelect = 0;
         UInt32 ledIntensityLevel = 0;

         if ((byte.TryParse(this.WekoLaunchCardActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (byte.TryParse(this.WekoLaunchCardLedSelectTextBox.Text, out ledSelect) != false) &&
             (UInt32.TryParse(this.WekoLaunchCardLedIntensityTextBox.Text, out ledIntensityLevel) != false))
         {
            this.launchCard.NodeId = nodeId;
            bool result = this.launchCard.SetLedIntensityLevel(ledSelect, ledIntensityLevel);

            if (false != result)
            {
               this.StatusLabel.Text = "Launch card LED intensity set.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set launch card LED intensity";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void DecreaseWekoLaunchCardCameraLedIntensityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         byte ledSelect = 0;
         UInt32 ledIntensityLevel = 0;

         if ((byte.TryParse(this.WekoLaunchCardActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (byte.TryParse(this.WekoLaunchCardLedSelectTextBox.Text, out ledSelect) != false) &&
             (UInt32.TryParse(this.WekoLaunchCardLedIntensityTextBox.Text, out ledIntensityLevel) != false))
         {
            this.launchCard.NodeId = nodeId;
            ledIntensityLevel = (UInt32)((ledIntensityLevel >= 8) ? (ledIntensityLevel - 8) : 0);
            bool result = this.launchCard.SetLedIntensityLevel(ledSelect, ledIntensityLevel);

            if (false != result)
            {
               this.StatusLabel.Text = "Launch card light LED intensity decreased.";
               this.WekoLaunchCardLedIntensityTextBox.Text = ledIntensityLevel.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to decrease launch card LED intensity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void IncreaseWekoLaunchCardCameraLedIntensityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         byte ledSelect = 0;
         UInt32 ledIntensityLevel = 0;

         if ((byte.TryParse(this.WekoLaunchCardActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (byte.TryParse(this.WekoLaunchCardLedSelectTextBox.Text, out ledSelect) != false) &&
             (UInt32.TryParse(this.WekoLaunchCardLedIntensityTextBox.Text, out ledIntensityLevel) != false))
         {
            this.launchCard.NodeId = nodeId;
            ledIntensityLevel = (UInt32)((ledIntensityLevel <= 0xFFFFFFF) ? (ledIntensityLevel + 8) : 0xFFFFFFF);
            bool result = this.launchCard.SetLedIntensityLevel(ledSelect, ledIntensityLevel);

            if (false != result)
            {
               this.StatusLabel.Text = "Launch cardr light LED intensity increased.";
               this.WekoLaunchCardLedIntensityTextBox.Text = ledIntensityLevel.ToString();
            }
            else
            {
               this.StatusLabel.Text = "Unable to increase launch card LED intensity.";
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
               this.StatusLabel.Text = "Unable to configure Digital IO.";
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

      #region CrossBore Sensor Events

      private void ConfigCrossBoreSensorButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.CrossBoreSensorActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.crossBoreSensor.NodeId = nodeId;
            bool result = this.crossBoreSensor.Configure();

            if (false != result)
            {
               this.StatusLabel.Text = "CrossBore Sensor configured.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to configure CrossBore Sensor.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void StartCrossBoreSensorButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.CrossBoreSensorActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.crossBoreSensor.NodeId = nodeId;
            bool result = this.crossBoreSensor.Start();

            if (false != result)
            {
               this.StatusLabel.Text = "CrossBore Sensor started.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to start CrossBore Sensor.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void StopCrossBoreSensorButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.CrossBoreSensorActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.crossBoreSensor.NodeId = nodeId;
            this.crossBoreSensor.Stop();
            this.StatusLabel.Text = "CrossBore Sensor stopped.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void ResetCrossBoreSensorButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.CrossBoreSensorActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.crossBoreSensor.NodeId = nodeId;
            this.crossBoreSensor.Reset();
            this.StatusLabel.Text = "CrossBore Sensor reset.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void CrossBoreSensorSyncButton_Click(object sender, EventArgs e)
      {
         this.busInterface.Sync();
      }

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

         #region Sync Generation

         if ((false != this.syncActive) &&
             (DateTime.Now > this.syncTimeLimit))
         {
            this.syncTimeLimit = this.syncTimeLimit.AddMilliseconds(this.syncTime);
            this.busInterface.Sync();
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
               this.DownloadActivityButton.Text = "Start Download";
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

         #region Launch Controller Update

         this.WekoLaunchCardMcuTemperatureTextBox.Text = string.Format("{0:0}", this.launchCard.McuTemperature);

         #endregion

         #region Digital IO Updates

         this.DigitalIoInputsTextBox.Text = this.digitalIoInputs.ToString("X2");

         #endregion

         #region CrossBore Sensor Updates

         UInt16[] crossBoreSensorReadings = this.crossBoreSensor.Readings;

         if ((null != crossBoreSensorReadings) && (crossBoreSensorReadings.Length >= 15))
         {
            this.CrossBoreSensorReading0TextBox.Text = crossBoreSensorReadings[0].ToString();
            this.CrossBoreSensorReading1TextBox.Text = crossBoreSensorReadings[1].ToString();
            this.CrossBoreSensorReading2TextBox.Text = crossBoreSensorReadings[2].ToString();
            this.CrossBoreSensorReading3TextBox.Text = crossBoreSensorReadings[3].ToString();
         }

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

         this.digitalIo = new PeakDigitalIo("digital io", 0);
         this.digitalIo.OnInputChange = new PeakDigitalIo.InputChangeHandler(this.DigitalIoChangeHandler);

         this.launchCard = new UlcRoboticsWekoLaunchCard("launch card", 0);

         this.crossBoreSensor = new UlcRoboticsCrossBoreSensor("crossbore sensor", 0);


         this.busInterface = new BusInterface();
         this.busInterface.AddDevice(this.device);
         this.busInterface.AddDevice(this.downloadDevice);
         this.busInterface.AddDevice(this.digitalIo);
         this.busInterface.AddDevice(this.launchCard);
         this.busInterface.AddDevice(this.crossBoreSensor);

         this.device.TraceMask = 0xFFFF;
         this.downloadDevice.TraceMask = 0xFFFF;
         this.digitalIo.TraceMask = 0xFFFF;
         this.launchCard.TraceMask = 0xFFFF;
         this.crossBoreSensor.TraceMask = 0xFFFF;
                  
         this.traceQueue = new Queue();

         QueuedTraceListener queuedTraceListener = new QueuedTraceListener(this.traceQueue);
         Trace.Listeners.Add(queuedTraceListener);

         Tracer.MaskString = "FFFFFFFF";

         this.ActivityButton.Text = "Start";
         this.active = false;

         this.DownloadProgressLabel.Text = "";
         this.DownloadProgressBar.Visible = false;
         this.DownloadActivityButton.Text = "Start Download";
         this.downloadActive = false;

         this.HeartbeatActivityButton.Text = "Start";
         this.heartbeatActive = false;

         this.DigitalIoBaudComboBox.Items.Clear();
         this.DigitalIoBaudComboBox.Items.Add("1000000");
         this.DigitalIoBaudComboBox.Items.Add("800000");
         this.DigitalIoBaudComboBox.Items.Add("500000");
         this.DigitalIoBaudComboBox.Items.Add("250000");
         this.DigitalIoBaudComboBox.Items.Add("125000");
         this.DigitalIoBaudComboBox.Items.Add("50000");
         this.DigitalIoBaudComboBox.SelectedIndex = 5;
      }

      #endregion

   }
}
