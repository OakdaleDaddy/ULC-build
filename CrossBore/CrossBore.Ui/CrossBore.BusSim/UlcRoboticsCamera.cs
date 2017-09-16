
namespace NICBOT.BusSim
{
   using System;
   using System.Drawing;
   using System.Text;
   using System.Xml;

   using Microsoft.Win32;

   public partial class UlcRoboticsCamera : DeviceControl
   {
      #region Fields

      private bool active;

      private UInt32 deviceType;
      private UInt32 errorStatus;
      private string deviceName;
      private string version;

      private UInt16 producerHeartbeatTime;

      private byte state;
      private byte powerUpState;
      private byte timeoutState;

      private byte lightIntensity;
      private byte powerUpLightIntensity;
      private byte timeoutLightIntensity;

      private byte baudRateCode;
      private byte nodeId;
      private UInt16 timeout;
      private string location;

      private byte sdoNodeId;
      
      private byte nvPowerUpState;
      private byte nvTimeoutState;
      private byte nvPowerUpLightIntensity;
      private byte nvTimeoutLightIntensity;
      private byte nvBaudRateCode;
      private byte nvNodeId;
      private string nvLocation;
      private UInt16 nvTimeout;

      private DateTime timeoutLimit;
      private bool timedOut;

      private DateTime heartbeatLimit;      
      
      #endregion

      #region Helper Functions

      private void SetState(byte state)
      {
         this.state = state;
         this.CameraStateLabel.Text = (0 != this.state) ? "ON" : "OFF";
      }

      private void SetLightIntensity(byte intensity)
      {
         this.lightIntensity = intensity;
         this.LightIntensityLabel.Text = this.lightIntensity.ToString();
         this.LightLabel.BackColor = Color.FromArgb(this.lightIntensity, this.lightIntensity, this.lightIntensity);
      }

      #endregion

      #region Device Specific Functions

      protected override void Reset()
      {
         this.DeviceStateLabel.Text = "PRE-OP";

         this.powerUpState = this.nvPowerUpState;
         this.timeoutState = this.nvTimeoutState;
         this.powerUpLightIntensity = this.nvPowerUpLightIntensity;
         this.timeoutLightIntensity = this.nvTimeoutLightIntensity;
         this.baudRateCode = this.nvBaudRateCode;
         this.nodeId = this.nvNodeId;
         this.timeout = this.nvTimeout;
         this.location = this.nvLocation;

         this.sdoNodeId = this.nvNodeId;

         this.SetState(this.powerUpState);
         this.SetLightIntensity(this.powerUpLightIntensity);

         this.NodeIdTextBox.Text = this.nodeId.ToString();
         this.DescriptionTextBox.Text = this.nvLocation;

         this.producerHeartbeatTime = 0;

         int cobId = this.GetCobId(COBTypes.ERROR, this.nodeId);
         byte[] bootUpMsg = new byte[1];
         bootUpMsg[0] = 0;

         this.Transmit(cobId, bootUpMsg);

         base.Reset();
      }

      protected override void Start()
      {
         base.Start();
         this.DeviceStateLabel.Text = "RUNNING";

         this.timeoutLimit = DateTime.Now.AddMilliseconds(this.timeout);
         this.timedOut = false;

         this.heartbeatLimit = DateTime.Now.AddMilliseconds(this.producerHeartbeatTime);
      }

      protected override void Stop()
      {
         base.Stop();
         this.DeviceStateLabel.Text = "STOPPED";
         this.SetState(0);
         this.SetLightIntensity(0);
      }

      protected override bool LoadDeviceData(UInt16 index, byte subIndex, byte[] buffer, ref UInt32 dataLength)
      {
         bool valid = false;

         if (0x1000 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.deviceType);
            valid = true;
         }
         else if (0x1001 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.errorStatus);
            valid = true;
         }
         else if (0x1008 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.deviceName);
            valid = true;
         }
         else if (0x100A == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.version);
            valid = true;
         }
         else if (0x1017 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.producerHeartbeatTime);
            valid = true;
         }            
         else if ((0x1018 == index) && (0 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (byte)1);
            valid = true;
         }
         else if ((0x1018 == index) && (1 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (UInt32)0);
            valid = true;
         }
         else if ((0x2001 == index) && (0 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (byte)3);
            valid = true;
         }
         else if ((0x2001 == index) && (1 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.state);
            valid = true;
         }
         else if ((0x2001 == index) && (2 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.powerUpState);
            valid = true;
         }
         else if ((0x2001 == index) && (3 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.timeoutState);
            valid = true;
         }
         else if ((0x2002 == index) && (0 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (byte)3);
            valid = true;
         }
         else if ((0x2002 == index) && (1 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.lightIntensity);
            valid = true;
         }
         else if ((0x2002 == index) && (2 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.powerUpLightIntensity);
            valid = true;
         }
         else if ((0x2002 == index) && (3 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.timeoutLightIntensity);
            valid = true;
         }
         else if (0x2100 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.baudRateCode);
            valid = true;
         }
         else if (0x2101 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.sdoNodeId);
            valid = true;
         }
         else if (0x2102 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.timeout);
            valid = true;
         }
         else if (0x2103 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.location);
            valid = true;
         }
         else if (0x2105 == index)
         {
            dataLength = this.MoveDeviceData(buffer, 0x65766173);
            valid = true;
         }

         return (valid);
      }

      protected override bool EvaluateDeviceDataSize(UInt16 index, byte subIndex, UInt32 dataLength)
      {
         bool valid = false;

         if (0x2103 == index)
         {
            if (dataLength <= 128)
            {
               valid = true;
            }
         }

         return (valid);
      }

      protected override bool StoreDeviceData(UInt16 index, byte subIndex, byte[] buffer, int offset, UInt32 length)
      {
         bool valid = false;

         if (0x1017 == index)
         {
            if (2 == length)
            {
               this.producerHeartbeatTime = BitConverter.ToUInt16(buffer, offset);
               this.heartbeatLimit = this.heartbeatLimit.AddMilliseconds(this.producerHeartbeatTime);
               valid = true;
            }
         }
         else if ((0x2001 == index) && (1 == subIndex))
         {
            if (1 == length)
            {
               this.SetState(buffer[offset]);
               this.timeoutLimit = DateTime.Now.AddMilliseconds(this.timeout);
               this.timedOut = false;
               valid = true;
            }
         }
         else if ((0x2001 == index) && (2 == subIndex))
         {
            if (1 == length)
            {
               this.powerUpState = buffer[offset];
               valid = true;
            }
         }
         else if ((0x2001 == index) && (3 == subIndex))
         {
            if (1 == length)
            {
               this.timeoutState = buffer[offset];
               valid = true;
            }
         }
         else if ((0x2002 == index) && (1 == subIndex))
         {
            if (1 == length)
            {
               this.SetLightIntensity(buffer[offset]);
               this.timeoutLimit = DateTime.Now.AddMilliseconds(this.timeout);
               this.timedOut = false;
               valid = true;
            }
         }
         else if ((0x2002 == index) && (2 == subIndex))
         {
            if (1 == length)
            {
               this.powerUpLightIntensity = buffer[offset];
               valid = true;
            }
         }
         else if ((0x2002 == index) && (3 == subIndex))
         {
            if (1 == length)
            {
               this.timeoutLightIntensity = buffer[offset];
               valid = true;
            }
         }
         else if (0x2100 == index)
         {
            if (1 == length)
            {
               this.baudRateCode = buffer[offset];
               valid = true;
            }
         }
         else if (0x2101 == index)
         {
            if (1 == length)
            {
               this.sdoNodeId = buffer[offset];
               this.NodeIdTextBox.Text = this.nodeId.ToString();
               valid = true;
            }
         }
         else if (0x2102 == index)
         {
            if (2 == length)
            {
               this.timeout = BitConverter.ToUInt16(buffer, offset);
               valid = true;
            }
         }
         else if (0x2103 == index)
         {
            this.location = Encoding.UTF8.GetString(buffer, offset, (int)length);
            valid = true;
         }
         else if (0x2105 == index)
         {
            UInt32 value = BitConverter.ToUInt32(buffer, offset);

            if (0x65766173 == value)
            {
               this.nvPowerUpState = this.powerUpState;
               this.nvTimeoutState = this.timeoutState;
               this.nvPowerUpLightIntensity = this.powerUpLightIntensity;
               this.nvTimeoutLightIntensity = this.timeoutLightIntensity;
               this.nvBaudRateCode = this.baudRateCode;
               this.nvNodeId = this.sdoNodeId;
               this.nvTimeout = this.timeout;
               this.nvLocation = this.location;

               valid = true;
            }
         }

         return (valid);
      }
      
      #endregion

      #region Message Process Functions

      private void ProcessNetworkMessage(byte[] frame)
      {
         if (frame.Length >= 2)
         {
            if ((frame[1] == 0) || (frame[1] == this.nodeId))
            {
               if (0x81 == frame[0])
               {
                  this.Reset();
               }
               else if (0x01 == frame[0])
               {
                  this.Start();
               }
               else if (0x02 == frame[0])
               {
                  this.Stop();
               }
            }
         }
      }

      private void ProcessSdoMessage(byte[] frame)
      {
         int css = (int)((frame[0] >> 5) & 0x7);

         if (0 == css)
         {
            // download SDO segment

            this.ProcessDownload(this.nodeId, frame);
         }
         else if (1 == css)
         {
            // initiate SDO download

            this.InitiateDownload(this.nodeId, frame);
         }
         else if (2 == css)
         {
            // initiate SDO upload

            UInt16 index = BitConverter.ToUInt16(frame, 1);
            byte subIndex = frame[3];

            this.InitiateUpload(this.nodeId, index, subIndex);
         }
         else if (3 == css)
         {
            // upload SDO segment

            bool t = (((frame[0] >> 4) & 0x1) != 0) ? true : false;
            UInt16 index = BitConverter.ToUInt16(frame, 1);
            byte subIndex = frame[3];

            this.ProcessUpload(this.nodeId, t, index, subIndex);
         }
         else if (4 == css)
         {
            // abort

            UInt16 index = BitConverter.ToUInt16(frame, 1);
            byte subIndex = frame[3];
            UInt32 code = BitConverter.ToUInt32(frame, 4);

            this.AbortTransfer(this.nodeId, index, subIndex, code);
         }
      }

      private void ProcessPdo1Message(byte[] frame)
      {
      }

      private void ProcessPdo2Message(byte[] frame)
      {
      }

      private void ProcessPdo3Message(byte[] frame)
      {
      }

      private void ProcessPdo4Message(byte[] frame)
      {
      }

      #endregion

      #region Constructor

      public UlcRoboticsCamera()
         : base()
      {
         this.InitializeComponent();

         this.deviceType = 0;
         this.errorStatus = 0;
         this.deviceName = "Camera With Light";
         this.version = "v1.23 Jul 10 2015 18:56:35";
         
         this.nvPowerUpState = 0;
         this.nvTimeoutState = 0;
         this.nvPowerUpLightIntensity = 0;
         this.nvTimeoutLightIntensity = 0;
         this.nvBaudRateCode = 5;
         this.nvNodeId = 31;
         this.nvTimeout = 0;
         this.nvLocation = "";
        
         this.NodeIdTextBox.Text = this.nvNodeId.ToString();
         this.DescriptionTextBox.Text = this.nvLocation;
      }

      #endregion

      #region Access Functions

      public override void LoadRegistry(RegistryKey appKey, string deviceTag)
      {
         object keyValue;
         byte parsedValue = 0;

         keyValue = appKey.GetValue(deviceTag + "Enabled");
         this.EnabledCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "BusId");
         string busId = (null != keyValue) ? keyValue.ToString() : "";
         this.SetBusId(busId);

         keyValue = appKey.GetValue(deviceTag + "nvPowerUpState");
         this.nvPowerUpState = (byte)((null != keyValue) ? (byte.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 0) : 0);

         keyValue = appKey.GetValue(deviceTag + "nvTimeoutState");
         this.nvTimeoutState = (byte)((null != keyValue) ? (byte.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 0) : 0);

         keyValue = appKey.GetValue(deviceTag + "nvPowerUpLightIntensity");
         this.nvPowerUpLightIntensity = (byte)((null != keyValue) ? (byte.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 0) : 0);

         keyValue = appKey.GetValue(deviceTag + "nvTimeoutLightIntensity");
         this.nvTimeoutLightIntensity = (byte)((null != keyValue) ? (byte.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 0) : 0);

         keyValue = appKey.GetValue(deviceTag + "nvBaudRateCode");
         this.nvBaudRateCode = (byte)((null != keyValue) ? (byte.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 5) : 5);

         keyValue = appKey.GetValue(deviceTag + "nvNodeId");
         this.nvNodeId = (byte)((null != keyValue) ? (byte.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 31) : 31);
         this.NodeIdTextBox.Text = this.nvNodeId.ToString();

         keyValue = appKey.GetValue(deviceTag + "nvTimeout");
         this.nvTimeout = (byte)((null != keyValue) ? (byte.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 0) : 0);

         keyValue = appKey.GetValue(deviceTag + "nvLocation");
         this.nvLocation = (null != keyValue) ? keyValue.ToString() : "";
         this.DescriptionTextBox.Text = nvLocation;
      }

      public override void SaveRegistry(RegistryKey appKey, string deviceTag)
      {
         appKey.SetValue(deviceTag + "Enabled", this.EnabledCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "BusId", this.GetBusId());
         appKey.SetValue(deviceTag + "nvPowerUpState", this.nvPowerUpState);
         appKey.SetValue(deviceTag + "nvTimeoutState", this.nvTimeoutState);
         appKey.SetValue(deviceTag + "nvPowerUpLightIntensity", this.nvPowerUpLightIntensity);
         appKey.SetValue(deviceTag + "nvTimeoutLightIntensity", this.nvTimeoutLightIntensity);
         appKey.SetValue(deviceTag + "nvBaudRateCode", this.nvBaudRateCode);
         appKey.SetValue(deviceTag + "nvNodeId", this.nvNodeId);
         appKey.SetValue(deviceTag + "nvTimeout", this.nvTimeout);
         appKey.SetValue(deviceTag + "nvLocation", this.nvLocation);
      }

      public override void Read(XmlReader reader)
      {
         string name = reader.Name;
         reader.Read();

         if ("Enabled" == name)
         {
            this.EnabledCheckBox.Checked = ("0" != reader.Value) ? true : false;
         }
         else if ("BusId" == name)
         {
            this.SetBusId(reader.Value);
         }
         else if ("nvPowerUpState" == name)
         {
            byte.TryParse(reader.Value, out this.nvPowerUpState);
         }
         else if ("nvTimeoutState" == name)
         {
            byte.TryParse(reader.Value, out this.nvTimeoutState);
         }
         else if ("nvPowerUpLightIntensity" == name)
         {
            byte.TryParse(reader.Value, out this.nvPowerUpLightIntensity);
         }
         else if ("nvTimeoutLightIntensity" == name)
         {
            byte.TryParse(reader.Value, out this.nvTimeoutLightIntensity);
         }
         else if ("nvBaudRateCode" == name)
         {
            byte.TryParse(reader.Value, out this.nvBaudRateCode);
         }
         else if ("nvNodeId" == name)
         {
            byte.TryParse(reader.Value, out this.nvNodeId);
            this.NodeIdTextBox.Text = this.nvNodeId.ToString();
         }
         else if ("nvTimeout" == name)
         {
            UInt16.TryParse(reader.Value, out this.nvTimeout);
         }
         else if ("nvLocation" == name)
         {
            this.nvLocation = reader.Value;
            this.DescriptionTextBox.Text = nvLocation;
         }
      }

      public override void Write(XmlWriter writer)
      {
         writer.WriteElementString("Enabled", (false != this.EnabledCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("NodeId", this.NodeIdTextBox.Text);
         writer.WriteElementString("BusId", this.GetBusId());
         writer.WriteElementString("nvPowerUpState", this.nvPowerUpState.ToString());
         writer.WriteElementString("nvTimeoutState", this.nvTimeoutState.ToString());
         writer.WriteElementString("nvPowerUpLightIntensity", this.nvPowerUpLightIntensity.ToString());
         writer.WriteElementString("nvTimeoutLightIntensity", this.nvTimeoutLightIntensity.ToString());
         writer.WriteElementString("nvBaudRateCode", this.nvBaudRateCode.ToString());
         writer.WriteElementString("nvNodeId", this.nvNodeId.ToString());
         writer.WriteElementString("nvTimeout", this.nvTimeout.ToString());
         writer.WriteElementString("nvLocation", this.nvLocation);
      }

      public override void SetBusId(string busId)
      {
         this.BusIdTextBox.Text = busId;
         base.SetBusId(busId); ;
      }

      public override void PowerUp()
      {
         this.EnabledCheckBox.Enabled = false;
         this.DescriptionTextBox.Enabled = false;
         this.NodeIdTextBox.Enabled = false;

         this.active = this.EnabledCheckBox.Checked;

         if (false != this.active)
         {
            this.Reset();
         }
         else
         {
            this.DeviceStateLabel.Text = "DISABLED";
         }
      }

      public override void PowerDown()
      {
         this.active = false;

         this.EnabledCheckBox.Enabled = true;
         this.DescriptionTextBox.Enabled = true;
         this.NodeIdTextBox.Enabled = true;
         
         this.deviceState = DeviceStates.stopped;
         this.DeviceStateLabel.Text = "OFF";
      }

      public override void DeviceReceive(int cobId, byte[] frame)
      {
         if (false != this.active)
         {
            COBTypes frameType = (COBTypes)((cobId >> 7) & 0xF);
            int nodeId = (int)(cobId & 0x7F);

            if (COBTypes.NMT == frameType)
            {
               if ((0 == nodeId) || (nodeId == this.nodeId))
               {
                  this.ProcessNetworkMessage(frame);
               }
            }
            else if (COBTypes.RSDO == frameType)
            {
               if ((nodeId == this.nodeId) && (DeviceStates.stopped != this.deviceState))
               {
                  this.ProcessSdoMessage(frame);
               }
            }
            else if (COBTypes.RPDO1 == frameType)
            {
               if ((nodeId == this.nodeId) && (DeviceStates.running == this.deviceState))
               {
                  this.ProcessPdo1Message(frame);
               }
            }
            else if (COBTypes.RPDO2 == frameType)
            {
               if ((nodeId == this.nodeId) && (DeviceStates.running == this.deviceState))
               {
                  this.ProcessPdo2Message(frame);
               }
            }
            else if (COBTypes.RPDO3 == frameType)
            {
               if ((nodeId == this.nodeId) && (DeviceStates.running == this.deviceState))
               {
                  this.ProcessPdo3Message(frame);
               }
            }
            else if (COBTypes.RPDO4 == frameType)
            {
               if ((nodeId == this.nodeId) && (DeviceStates.running == this.deviceState))
               {
                  this.ProcessPdo4Message(frame);
               }
            }
         }
      }

      public override void UpdateDevice()
      {
         if (DeviceStates.running == this.deviceState)
         {
            if (0 != this.timeout)
            {
               if ((false == this.timedOut) &&
                   (DateTime.Now > this.timeoutLimit))
               {
                  this.timedOut = true;
                  this.SetState(this.timeoutState);
                  this.SetLightIntensity(this.timeoutLightIntensity);
               }
            }
         }

         if (0 != this.producerHeartbeatTime)
         {
            if (DateTime.Now > this.heartbeatLimit)
            {
               this.heartbeatLimit = this.heartbeatLimit.AddMilliseconds(this.producerHeartbeatTime);

               int cobId = this.GetCobId(COBTypes.ERROR, this.nodeId);
               byte[] heartbeatMsg = new byte[1];

               if (DeviceStates.preop == this.deviceState)
               {
                  heartbeatMsg[0] = 0x7F;
               }
               else if (DeviceStates.running == this.deviceState)
               {
                  heartbeatMsg[0] = 0x05;
               }
               else
               {
                  heartbeatMsg[0] = 0x04;
               }

               this.Transmit(cobId, heartbeatMsg);
            }
         }
      }

      #endregion
   }
}
