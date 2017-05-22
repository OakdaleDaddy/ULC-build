
namespace CanDemo.DeviceTest
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

   using CanDemo.CAN;
   using CanDemo.PCANLight;
   using CanDemo.Utilities;

   public partial class MainForm : Form
   {
      #region Definition

      private const string RegistryCompanyName = "ULC Robotics";
      private const string RegistryApplicationName = "CANDemo Device Test";

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
      private DemoDevice testDevice;

      private byte heartbeatNodeId;
      private bool heartbeatActive;
      private int heartbeatTime;
      private DateTime heartbeatTimeLimit;

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
         this.BusInterfaceComboBox.SelectedIndex = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 2) : 2;


         keyValue = appKey.GetValue("ActiveNodeId");
         this.ActiveDeviceNodeIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue("HeartbeatNodeId");
         this.HeartbeatNodeIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "80";

         keyValue = appKey.GetValue("HeartbeatTime");
         this.HeartbeatTimeTextBox.Text = (null != keyValue) ? keyValue.ToString() : "1000";

         keyValue = appKey.GetValue("HeartbeatShown");
         this.ShowHeartbeatCheckBox.Checked = ("0" == ((null != keyValue) ? keyValue.ToString() : "0")) ? false : true;


         keyValue = appKey.GetValue("TestActiveNodeId");
         this.TestActiveNodeIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";
         
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

         appKey.SetValue("TestActiveNodeId", this.TestActiveNodeIdTextBox.Text);

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

      #endregion

      #region Delegate Functions

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

      #region Test Device Events

      private void TestConfigButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.TestActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.testDevice.NodeId = nodeId;

            bool result = this.testDevice.Configure();

            if (false != result)
            {
               this.StatusLabel.Text = "Test configured.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to configure test.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void TestStartButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.TestActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.testDevice.NodeId = nodeId;

            bool result = this.testDevice.Start();

            if (false != result)
            {
               this.StatusLabel.Text = "Test started.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to start test.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void TestStopButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.TestActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.testDevice.NodeId = nodeId;
            this.testDevice.Stop();
            this.StatusLabel.Text = "Test stopped.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void TestResetButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.TestActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.testDevice.NodeId = nodeId;
            this.testDevice.Reset();
            this.StatusLabel.Text = "Test reset.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void ReadTestVersionButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.TestActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.testDevice.NodeId = nodeId;

            string version = null;
            bool result = this.testDevice.ReadVersion(ref version);

            if (false != result)
            {
               this.TestVersionTextBox.Text = version;
               this.StatusLabel.Text = "Test version retrieved.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to read test version.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetTest6040Button_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.TestActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.testDevice.NodeId = nodeId;

            UInt16 value = 0;
            bool result = this.testDevice.Get6040(ref value);

            if (false != result)
            {
               this.Test6040TextBox.Text = string.Format("{0:X4}", value);
               this.StatusLabel.Text = "Test 0x6040 retrieved.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to read test 0x6040.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetTest6040Button_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         UInt16 value = 0;

         if ((byte.TryParse(this.TestActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (UInt16.TryParse(this.Test6040TextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out value) != false))
         {
            this.testDevice.NodeId = nodeId;
            bool result = this.testDevice.Set6040(value);

            if (false != result)
            {
               this.StatusLabel.Text = string.Format("Test 0x6040 set to {0:X4}.", value);
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

      private void GetTest6041Button_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.TestActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.testDevice.NodeId = nodeId;

            UInt16 value = 0;
            bool result = this.testDevice.Get6041(ref value);

            if (false != result)
            {
               this.Test6041TextBox.Text = string.Format("{0:X4}", value);
               this.StatusLabel.Text = "Test 0x6041 retrieved.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to read test 0x6041.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetTest6060Button_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.TestActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.testDevice.NodeId = nodeId;

            byte value = 0;
            bool result = this.testDevice.Get6060(ref value);

            if (false != result)
            {
               this.Test6060TextBox.Text = string.Format("{0:X2}", value);
               this.StatusLabel.Text = "Test 0x6060 retrieved.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to read test 0x6060.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetTest6060Button_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         byte value = 0;

         if ((byte.TryParse(this.TestActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (byte.TryParse(this.Test6060TextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out value) != false))
         {
            this.testDevice.NodeId = nodeId;
            bool result = this.testDevice.Set6060(value);

            if (false != result)
            {
               this.StatusLabel.Text = string.Format("Test 0x6060 set to {0:X2}.", value);
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

      private void GetTest6061Button_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.TestActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.testDevice.NodeId = nodeId;

            byte value = 0;
            bool result = this.testDevice.Get6061(ref value);

            if (false != result)
            {
               this.Test6061TextBox.Text = string.Format("{0:X2}", value);
               this.StatusLabel.Text = "Test 0x6061 retrieved.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to read test 0x6061.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetTestModeButton_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;
         DemoDevice.Modes mode = DemoDevice.Modes.undefined;

         if (this.TestModeComboBox.Text == "off")
         {
            mode = DemoDevice.Modes.off;
         }
         else if (this.TestModeComboBox.Text == "velocity")
         {
            mode = DemoDevice.Modes.velocity;
         }

         if (byte.TryParse(this.TestActiveNodeIdTextBox.Text, out motorNodeId) != false)
         {
            this.testDevice.NodeId = motorNodeId;
            bool result = this.testDevice.SetMode(mode);

            if (false != result)
            {
               this.StatusLabel.Text = "Test mode set to " + mode.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to set test mode to " + mode.ToString() + ".";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void GetTestTargetVelocityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;

         if (byte.TryParse(this.TestActiveNodeIdTextBox.Text, out nodeId) != false)
         {
            this.testDevice.NodeId = nodeId;

            Int32 value = 0;
            bool result = this.testDevice.GetTargetVelocity(ref value);

            if (false != result)
            {
               this.TestTargetVelocityTextBox.Text = string.Format("{0}", value);
               this.StatusLabel.Text = "Test target velocity retrieved.";
            }
            else
            {
               this.StatusLabel.Text = "Unable to read test target velocity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void SetTestTargetVelocityButton_Click(object sender, EventArgs e)
      {
         byte nodeId = 0;
         Int32 value = 0;

         if ((byte.TryParse(this.TestActiveNodeIdTextBox.Text, out nodeId) != false) &&
             (Int32.TryParse(this.TestTargetVelocityTextBox.Text, out value) != false))
         {
            this.testDevice.NodeId = nodeId;
            bool result = this.testDevice.SetTargetVelocity(value);

            if (false != result)
            {
               this.StatusLabel.Text = string.Format("Test target velocity set to {0}.", value);
            }
            else
            {
               this.StatusLabel.Text = "Unable to set test target velocity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void ScheduleTestTargetVelocityButton_Click(object sender, EventArgs e)
      {
         byte motorNodeId = 0;
         Int32 value = 0;

         if ((byte.TryParse(this.TestActiveNodeIdTextBox.Text, out motorNodeId) != false) &&
             (Int32.TryParse(this.TestTargetVelocityTextBox.Text, out value) != false))
         {
            this.testDevice.NodeId = motorNodeId;
            bool result = this.testDevice.ScheduleTargetVelocity(value);

            if (false != result)
            {
               this.StatusLabel.Text = "Test target velocity scheduled for " + value.ToString() + ".";
            }
            else
            {
               this.StatusLabel.Text = "Unable to schedule test target velocity.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void TestSyncButton_Click(object sender, EventArgs e)
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

         #region Trace Activity Update

         while (0 != this.traceQueue.Count)
         {
            string activityString = (string)this.traceQueue.Dequeue();
            this.ActivityRichTextBox.AppendText(activityString + "\n");
         }

         #endregion

         #region Test Update

         this.TestActualVelocityTextBox.Text = this.testDevice.RPM.ToString();

         #endregion
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
         this.testDevice = new DemoDevice("test", 0);

         this.busInterface = new BusInterface();
         this.busInterface.AddDevice(this.device);
         this.busInterface.AddDevice(this.testDevice);
                  
         this.traceQueue = new Queue();

         QueuedTraceListener queuedTraceListener = new QueuedTraceListener(this.traceQueue);
         Trace.Listeners.Add(queuedTraceListener);

         Tracer.MaskString = "FFFFFFFF";

         this.ActivityButton.Text = "Start";
         this.active = false;

         this.HeartbeatActivityButton.Text = "Start";
         this.heartbeatActive = false;
      }

      #endregion

   }
}
