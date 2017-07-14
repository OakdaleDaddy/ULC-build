
namespace Weco.BusSim
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.ComponentModel;
   using System.Data;
   using System.Drawing;
   using System.IO;
   using System.Linq;
   using System.Text;
   using System.Threading.Tasks;
   using System.Windows.Forms;
   using System.Xml;

   using Microsoft.Win32;

   using Weco.PCANLight;
   using Weco.Utilities;

   public partial class MainForm : Form
   {
      #region Definition

      private const string RegistryCompanyName = "ULC Robotics";
      private const string RegistryApplicationName = "Weco Bus Simulator";

      #endregion

      #region Fields

      private ArrayList deviceList;
      private int devicePanelWidth;
      private bool active;

      private bool busASelected;
      private BusInterfaces busInterfaceA;
      private Queue busAReceiveQueue;
      private ArrayList busADeviceList;

      private bool busBSelected;
      private BusInterfaces busInterfaceB;
      private Queue busBReceiveQueue;
      private ArrayList busBDeviceList;

      #endregion

      #region Helper Functions

      private void AddDevice(int nextControlTop, DeviceControl deviceControl)
      {
         deviceControl.Top = nextControlTop;
         deviceControl.Width = this.devicePanelWidth - DevicePanel.VerticalScroll.Value;
         deviceControl.OnSelect = new DeviceControl.SelectHandler(this.DevicePanelSelect);

         DeviceControl.DeviceTransmitHandler transmitHandler = null;
         
         if (deviceControl.GetBusId() == "B")
         {
            transmitHandler = new DeviceControl.DeviceTransmitHandler(this.DeviceTransmitBusB);
         }
         else
         {
            transmitHandler = new DeviceControl.DeviceTransmitHandler(this.DeviceTransmitBusA);         
         }

         deviceControl.OnDeviceTransmit = transmitHandler;

         this.DevicePanel.Controls.Add(deviceControl);
         this.deviceList.Add(deviceControl);

         if (deviceControl.GetBusId() == "B")
         {
            this.busBDeviceList.Add(deviceControl);
         }
         else
         {
            this.busADeviceList.Add(deviceControl);
         }

         this.MainForm_Resize(this, null); // updates controls for addition
      }

      private void ReadConfiguration(string filePath)
      {
         try
         {
            foreach (DeviceControl deviceControl in this.deviceList)
            {
               this.DevicePanel.Controls.Remove(deviceControl);
            }

            this.deviceList.Clear();
            this.busADeviceList.Clear();
            this.busBDeviceList.Clear();

            using (XmlReader reader = XmlReader.Create(filePath))
            {
               bool result = true;
               int nextControlTop = 0;
               DeviceControl deviceControl = null;

               for (; result; )
               {
                  result = reader.Read();

                  if (reader.IsStartElement() != false)
                  {
                     if (null != deviceControl)
                     {
                        deviceControl.Read(reader);
                     }
                     else if ("Type" == reader.Name)
                     {
                        reader.Read();

                        if ("Weco.BusSim.ElmoMotor" == reader.Value)
                        {
                           deviceControl = new ElmoMotor();
                        }
                        else if ("Weco.BusSim.UlcRoboticsWecoTrackController" == reader.Value)
                        {
                           deviceControl = new UlcRoboticsWecoTrackController();
                        }
                        else if ("Weco.BusSim.UlcRoboticsWecoHub" == reader.Value)
                        {
                           deviceControl = new UlcRoboticsWecoHub();
                        }
                     }
                  }
                  else
                  {
                     if (reader.Name.Contains("Device") != false)
                     {
                        this.AddDevice(nextControlTop, deviceControl);
                        nextControlTop += deviceControl.Height;
                        deviceControl = null;
                     }
                  }
               }

               reader.Close();
            }
         }
         catch { }
      }

      private void WriteConfiguration(string filePath)
      {
         XmlWriterSettings xmls = new XmlWriterSettings();
         xmls.Indent = true;

         using (XmlWriter writer = XmlWriter.Create(filePath, xmls))
         {
            writer.WriteStartDocument();
            writer.WriteStartElement("TruckSim");

            for (int i = 0; i < this.deviceList.Count; i++)
            {
               DeviceControl deviceControl = (DeviceControl)this.deviceList[i];
               string deviceTag = "Device" + i.ToString();

               writer.WriteStartElement(deviceTag);
               writer.WriteElementString("Type", deviceControl.GetType().ToString());
               deviceControl.Write(writer);

               writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();
         }
      }

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

         keyValue = appKey.GetValue("SelectedBusA");
         this.BusAInterfaceComboBox.SelectedIndex = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 0) : 0;

         keyValue = appKey.GetValue("SelectedBusB");
         this.BusBInterfaceComboBox.SelectedIndex = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 0) : 0;

         keyValue = appKey.GetValue("NumDevices");
         int numDevices = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 0) : 0;

         int nextControlTop = 0;

         for (int i = 0; i < numDevices; i++)
         {
            string deviceTag = "Device" + i.ToString();
            keyValue = appKey.GetValue(deviceTag);
            string deviceType = (null != keyValue) ? keyValue.ToString() : "";
            DeviceControl deviceControl = null;

            if ("Weco.BusSim.ElmoMotor" == deviceType)
            {
               deviceControl = new ElmoMotor();
            }
            else if ("Weco.BusSim.UlcRoboticsWecoTrackController" == deviceType)
            {
               deviceControl = new UlcRoboticsWecoTrackController();
            }
            else if ("Weco.BusSim.UlcRoboticsWecoHub" == deviceType)
            {
               deviceControl = new UlcRoboticsWecoHub();
            }

            if (null != deviceControl)
            {
               deviceControl.LoadRegistry(appKey, deviceTag);
               this.AddDevice(nextControlTop, deviceControl);
               nextControlTop += deviceControl.Height;
            }
         }

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

         appKey.SetValue("SelectedBusA", this.BusAInterfaceComboBox.SelectedIndex.ToString());
         appKey.SetValue("SelectedBusB", this.BusBInterfaceComboBox.SelectedIndex.ToString());
         appKey.SetValue("NumDevices", this.deviceList.Count.ToString());

         for (int i = 0; i < this.deviceList.Count; i++)
         {
            DeviceControl deviceControl = (DeviceControl)this.deviceList[i];
            string deviceTag = "Device" + i.ToString();
            appKey.SetValue(deviceTag, deviceControl.GetType());
            deviceControl.SaveRegistry(appKey, deviceTag);
         }
         
         #endregion
      }

      #endregion

      #region Delegates

      private DeviceControl selectedDevice;

      private void DevicePanelSelect(DeviceControl deviceControl, bool selected)
      {
         if (false != selected)
         {
            if (deviceControl != this.selectedDevice)
            {
               if (null != this.selectedDevice)
               {
                  this.selectedDevice.DeSelect();
               }
            }

            this.selectedDevice = deviceControl;
         }
         else
         {
            this.selectedDevice = null;
         }
      }

      private bool DeviceTransmitBusA(int id, byte[] data)
      {
         CANResult transmitResult = PCANLight.Send(this.busInterfaceA, id, data);
         bool result = (transmitResult == CANResult.ERR_OK) ? true : false;

         return (result);
      }

      private bool DeviceTransmitBusB(int id, byte[] data)
      {
         CANResult transmitResult = PCANLight.Send(this.busInterfaceB, id, data);
         bool result = (transmitResult == CANResult.ERR_OK) ? true : false;

         return (result);
      }

      private void BusAReceiveHandler(CanFrame frame)
      {
         lock (this)
         {
            this.busAReceiveQueue.Enqueue(frame);
         }
      }

      private void BusBReceiveHandler(CanFrame frame)
      {
         lock (this)
         {
            this.busBReceiveQueue.Enqueue(frame);
         }
      }

      #endregion

      #region User Events

      private void ActivityButton_Click(object sender, EventArgs e)
      {
         if (false == this.active)
         {
            this.busAReceiveQueue.Clear();
            this.busBReceiveQueue.Clear();

            this.busASelected = (0 != this.BusAInterfaceComboBox.SelectedIndex);
            this.busBSelected = (0 != this.BusBInterfaceComboBox.SelectedIndex);

            if (false != this.busASelected)
            {
               this.busInterfaceA = (BusInterfaces)this.BusAInterfaceComboBox.SelectedItem;
               CANResult startResult = PCANLight.Start(this.busInterfaceA, 50000, FramesType.INIT_TYPE_ST, TraceGroup.COMM, this.BusAReceiveHandler);
               this.busASelected = (CANResult.ERR_OK == startResult);

               if (false != this.busASelected)
               {
                  foreach (DeviceControl deviceControl in this.busADeviceList)
                  {
                     deviceControl.PowerUp();
                  }
               }
               else
               {
                  this.StatusLabel.Text = "Unable to start bus A interface.";
               }
            }

            if (false != this.busBSelected)
            {
               this.busInterfaceB = (BusInterfaces)this.BusBInterfaceComboBox.SelectedItem;
               CANResult startResult = PCANLight.Start(this.busInterfaceB, 50000, FramesType.INIT_TYPE_ST, TraceGroup.COMM, this.BusBReceiveHandler);
               this.busBSelected = (CANResult.ERR_OK == startResult);

               if (false != this.busBSelected)
               {
                  foreach (DeviceControl deviceControl in this.busBDeviceList)
                  {
                     deviceControl.PowerUp();
                  }
               }
               else
               {
                  this.StatusLabel.Text = "Unable to start bus B interface.";
               }
            }

            if ((false != busASelected) || (false != busBSelected))
            {
               if ((false != this.busASelected) && (false != this.busBSelected))
               {
                  this.StatusLabel.Text = "Buses powered.";
               }
               else if (false != this.busASelected)
               {
                  this.StatusLabel.Text = "Bus A powered.";
               }
               else if (false != this.busBSelected)
               {
                  this.StatusLabel.Text = "Bus B powered.";
               }

               this.active = true;
               this.ActivityButton.Text = "Power Down";
               this.BusAInterfaceComboBox.Enabled = false;
               this.BusBInterfaceComboBox.Enabled = false;
               this.AddDeviceButton.Enabled = false;
               this.DeviceComboBox.Enabled = false;
               this.BusComboBox.Enabled = false;
               this.RemoveButton.Enabled = false;
               this.ReadButton.Enabled = false;
            }
            else
            {
               this.StatusLabel.Text = "Unable to power up.";
            }
         }
         else
         {
            if (false != busASelected)
            {
               foreach (DeviceControl deviceControl in this.busADeviceList)
               {
                  deviceControl.PowerDown();
               }

               PCANLight.Stop(this.busInterfaceA);
            }

            if (false != busBSelected)
            {
               foreach (DeviceControl deviceControl in this.busBDeviceList)
               {
                  deviceControl.PowerDown();
               }

               PCANLight.Stop(this.busInterfaceB);
            }

            this.StatusLabel.Text = "Devices powered down.";

            this.active = false;
            this.ActivityButton.Text = "Power Up";
            this.BusAInterfaceComboBox.Enabled = true;
            this.BusBInterfaceComboBox.Enabled = true;
            this.AddDeviceButton.Enabled = true;
            this.DeviceComboBox.Enabled = true;
            this.BusComboBox.Enabled = true;
            this.RemoveButton.Enabled = true;
            this.ReadButton.Enabled = true;
         }
      }

      private void ReadButton_Click(object sender, EventArgs e)
      {
         OpenFileDialog openFileDialog = new OpenFileDialog();
         openFileDialog.RestoreDirectory = true;
         openFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
         openFileDialog.FilterIndex = 0;
         openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
         openFileDialog.FileName = "trucksim.xml";

         DialogResult result = openFileDialog.ShowDialog();

         if (DialogResult.OK == result)
         {
            this.ReadConfiguration(openFileDialog.FileName);
         }
      }
      
      private void WriteButton_Click(object sender, EventArgs e)
      {
         SaveFileDialog saveFileDialog = new SaveFileDialog();
         saveFileDialog.RestoreDirectory = true;
         saveFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
         saveFileDialog.FilterIndex = 0;
         saveFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
         saveFileDialog.FileName = "trucksim.xml";

         DialogResult result = saveFileDialog.ShowDialog();

         if (DialogResult.OK == result)
         {
            this.WriteConfiguration(saveFileDialog.FileName);
         }
      }

      private void MoveUpButton_Click(object sender, EventArgs e)
      {
         if (null != this.selectedDevice)
         {
            DeviceControl previousControl = null;

            for (int i = 0; i < this.deviceList.Count; i++)
            {
               DeviceControl deviceControl = (DeviceControl)this.deviceList[i];

               if (deviceControl == this.selectedDevice)
               {
                  if (i > 0)
                  {
                     this.deviceList.Remove(deviceControl);
                     this.deviceList.Insert(i - 1, deviceControl);

                     deviceControl.Top = previousControl.Top;
                     previousControl.Top = deviceControl.Top + deviceControl.Height;

                     deviceControl.Select();
                  }

                  break;
               }

               previousControl = deviceControl;
            }
         }
      }

      private void MoveDownButton_Click(object sender, EventArgs e)
      {
         if (null != this.selectedDevice)
         {
            DeviceControl movingControl = null;

            for (int i = 0; i < this.deviceList.Count; i++)
            {
               DeviceControl deviceControl = (DeviceControl)this.deviceList[i];

               if (null != movingControl)
               {
                  this.deviceList.Remove(movingControl);
                  this.deviceList.Insert(i, movingControl);

                  deviceControl.Top = movingControl.Top;
                  movingControl.Top = deviceControl.Top + deviceControl.Height;

                  movingControl.Select();

                  break;
               }
               else if (deviceControl == this.selectedDevice)
               {
                  movingControl = deviceControl;
               }
            }
         }
      }

      private void AddDeviceButton_Click(object sender, EventArgs e)
      {
         int nextControlTop = 0;

         foreach (DeviceControl storedDeviceControl in this.deviceList)
         {
            nextControlTop += storedDeviceControl.Height;
         }

         nextControlTop -= this.DevicePanel.VerticalScroll.Value;

         DeviceControl deviceControl = null;
         string deviceType = (string)this.DeviceComboBox.SelectedItem;

         if ("ElmoMotor" == deviceType)
         {
            deviceControl = new ElmoMotor();
         }
         else if ("UlcRoboticsWecoTrackController" == deviceType)
         {
            deviceControl = new UlcRoboticsWecoTrackController();
         }
         else if ("UlcRoboticsWecoHub" == deviceType)
         {
            deviceControl = new UlcRoboticsWecoHub();
         }         

         if (null != deviceControl)
         {
            string busId = (0 != this.BusComboBox.SelectedIndex) ? "B" : "A";
            deviceControl.SetBusId(busId);
            this.AddDevice(nextControlTop, deviceControl);
         }
      }

      private void RemoveButton_Click(object sender, EventArgs e)
      {
         if (null != this.selectedDevice)
         {
            bool removeDevice = true;
            int nextControlTop = 0 - this.DevicePanel.VerticalScroll.Value;
            DeviceControl deviceToRemove = null;

            for (int i = 0; i < this.deviceList.Count; i++)
            {
               DeviceControl deviceControl = (DeviceControl)this.deviceList[i];

               if (false != removeDevice)
               {
                  if (deviceControl == this.selectedDevice)
                  {
                     deviceToRemove = deviceControl;
                     removeDevice = false;
                  }
                  else
                  {
                     nextControlTop += deviceControl.Height;
                  }
               }
               else
               {
                  deviceControl.Top = nextControlTop;
                  nextControlTop += deviceControl.Height;
               }
            }

            if (null != deviceToRemove)
            {
               this.deviceList.Remove(deviceToRemove);
               this.DevicePanel.Controls.Remove(deviceToRemove);

               if (this.busBDeviceList.Contains(deviceToRemove) != false)
               {
                  this.busBDeviceList.Remove(deviceToRemove);
               }
               else
               {
                  this.busADeviceList.Remove(deviceToRemove);
               }

               this.selectedDevice = null;
            }

            int verticalScrollBarAdjust = (nextControlTop > this.DevicePanel.Height) ? System.Windows.Forms.SystemInformation.VerticalScrollBarWidth : 0;
            this.devicePanelWidth = this.DevicePanel.Width - verticalScrollBarAdjust - 4;

            foreach (DeviceControl deviceControl in this.deviceList)
            {
               deviceControl.Width = this.devicePanelWidth;
            }
         }
      }

      private void ClearButton_Click(object sender, EventArgs e)
      {
         foreach (DeviceControl deviceControl in this.deviceList)
         {
            this.DevicePanel.Controls.Remove(deviceControl);
         }

         this.deviceList.Clear();
         this.busADeviceList.Clear();
         this.busBDeviceList.Clear();
      }

      #endregion

      #region Form Events

      private void MainForm_Shown(object sender, EventArgs e)
      {
         this.LoadRegistry();
         this.UpdateTimer.Enabled = true;
      }

      private void DevicePanel_Scroll(object sender, ScrollEventArgs e)
      {
         // allows tracking scrollbar
         this.DevicePanel.VerticalScroll.Value = e.NewValue;
         this.DevicePanel.Focus();
      }

      private void MainForm_Resize(object sender, EventArgs e)
      {
         int verticalScrollBarAdjust = (false != this.DevicePanel.VerticalScroll.Visible) ? System.Windows.Forms.SystemInformation.VerticalScrollBarWidth : 0;
         this.devicePanelWidth = this.DevicePanel.Width - verticalScrollBarAdjust - 4;

         foreach (DeviceControl deviceControl in this.deviceList)
         {
            deviceControl.Width = this.devicePanelWidth;
         }
      }

      private void UpdateTimer_Tick(object sender, EventArgs e)
      {
         #region Clock Update

         DateTime now = DateTime.Now;
         string timeText = string.Format("{0:D2}:{1:D2}:{2:D2}.{3}", now.Hour, now.Minute, now.Second, (now.Millisecond/100));
         this.TimeStatusLabel.Text = timeText;

         #endregion

         #region Bus A Process

         if (false != this.busASelected)
         {
            CanFrame frame = null;
            int receiveCount = 0;

            do
            {
               lock (this)
               {
                  receiveCount = this.busAReceiveQueue.Count;

                  if (receiveCount > 0)
                  {
                     frame = (CanFrame)this.busAReceiveQueue.Dequeue();
                  }
               }

               if (null != frame)
               {
                  foreach (DeviceControl deviceControl in this.busADeviceList)
                  {
                     deviceControl.DeviceReceive((int)frame.cobId, frame.data);
                  }

                  frame = null;
               }
            }
            while (0 != receiveCount);

            foreach (DeviceControl deviceControl in this.busADeviceList)
            {
               deviceControl.UpdateDevice();
            }
         }
#endregion

         #region Bus B Process

         if (false != this.busBSelected)
         {
            CanFrame frame = null;
            int receiveCount = 0;

            do
            {
               lock (this)
               {
                  receiveCount = this.busBReceiveQueue.Count;

                  if (receiveCount > 0)
                  {
                     frame = (CanFrame)this.busBReceiveQueue.Dequeue();
                  }
               }

               if (null != frame)
               {
                  foreach (DeviceControl deviceControl in this.busBDeviceList)
                  {
                     deviceControl.DeviceReceive((int)frame.cobId, frame.data);
                  }

                  frame = null;
               }
            }
            while (0 != receiveCount);

            foreach (DeviceControl deviceControl in this.busBDeviceList)
            {
               deviceControl.UpdateDevice();
            }
         }

         #endregion
      }

      private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
      {
         this.UpdateTimer.Enabled = false;
         this.SaveRegistry();
      }

      #endregion

      #region Constructor

      public MainForm()
      {
         this.InitializeComponent();

         this.deviceList = new ArrayList();
         this.active = false;
         this.ActivityButton.Text = "Power Up";
         this.StatusLabel.Text = "";

         this.busADeviceList = new ArrayList();
         this.busAReceiveQueue = new Queue();

         this.busBDeviceList = new ArrayList();
         this.busBReceiveQueue = new Queue();
        
         Array interfaces = Enum.GetValues(typeof(BusInterfaces));

         this.BusAInterfaceComboBox.Items.Clear();
         this.BusAInterfaceComboBox.Items.Add("none");
         for (IEnumerator i = interfaces.GetEnumerator(); i.MoveNext(); )
         {
            this.BusAInterfaceComboBox.Items.Add(i.Current);
         }
         this.BusAInterfaceComboBox.SelectedIndex = 1;

         this.BusBInterfaceComboBox.Items.Clear();
         this.BusBInterfaceComboBox.Items.Add("none");
         for (IEnumerator i = interfaces.GetEnumerator(); i.MoveNext(); )
         {
            this.BusBInterfaceComboBox.Items.Add(i.Current);
         }
         this.BusBInterfaceComboBox.SelectedIndex = 0;

         this.DeviceComboBox.Items.Clear();
         this.DeviceComboBox.Items.Add("ElmoMotor");
         this.DeviceComboBox.Items.Add("UlcRoboticsWecoTrackController");
         this.DeviceComboBox.Items.Add("UlcRoboticsWecoHub");
         this.DeviceComboBox.SelectedIndex = 0;

         this.BusComboBox.Items.Clear();
         this.BusComboBox.Items.Add("A");
         this.BusComboBox.Items.Add("B");
         this.BusComboBox.SelectedIndex = 0;
      }

      #endregion
   }
}
