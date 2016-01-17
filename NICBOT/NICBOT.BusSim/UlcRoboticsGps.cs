
namespace NICBOT.BusSim
{
   using System;
   using System.Collections.Generic;
   using System.ComponentModel;
   using System.Drawing;
   using System.Data;
   using System.Linq;
   using System.Text;
   using System.Windows.Forms;
   using System.Xml;

   using Microsoft.Win32;

   public partial class UlcRoboticsGps : DeviceControl
   {
      #region Fields

      private bool active;

      private UInt32 deviceType;
      private UInt32 errorStatus;
      private string deviceName;
      private string version;
      
      private byte nvBaudRateCode;
      private byte nvNodeId;
      
      private byte baudRateCode;
      private byte nodeId;
      private byte sdoNodeId;
      private UInt16 producerHeartbeatTime;

      private int status;
      private int satilliteCount;
      private int method;

      private double latitude;
      private double longitude;

      private TPDOMapping[] tpdoMapping;
      private DateTime heartbeatLimit;      

      #endregion

      #region Delegates

      private bool TPdoMappableHandler(UInt16 index, byte subIndex)
      {
         bool result = false;

         if (0x2201 == index)
         {
            if ((subIndex >= 1) && (subIndex <= 5))
            {
               result = true;
            }
         }
         else if ((0x2202 == index) ||
                  (0x2203 == index) ||
                  (0x2205 == index))
         {
            result = true;
         }

         return (result);
      }

      private int TPdoSizeHandler(UInt16 index, byte subIndex)
      {
         byte result = 0;

         if (0x2201 == index)
         {
            if ((subIndex >= 1) && (subIndex <= 3))
            {
               result = 1;
            }
            else if ((4 == subIndex) ||
                     (5 == subIndex))
            {
               result = 4;
            }
         }
         else if ((0x2202 == index) ||
                  (0x2203 == index))
         {
            result = 7;
         }
         else if (0x2205 == index)
         {
            result = 6;
         }

         return (result);
      }

      private int TPdoDataHandler(UInt16 index, byte subIndex, byte[] buffer, int offset)
      {
         byte[] pdoData = new byte[8];
         UInt32 dataLength = 0;
         this.LoadDeviceData(index, subIndex, pdoData, ref dataLength);

         for (int i=0; i<dataLength; i++)
         {
            buffer[offset+i] = pdoData[i];
         }

         return ((int)dataLength);
      }

      #endregion

      #region Device Specific Functions

      protected override void Reset()
      {
         this.DeviceStateLabel.Text = "PRE-OP";

         this.baudRateCode = this.nvBaudRateCode;
         this.nodeId = this.nvNodeId;

         this.sdoNodeId = this.nvNodeId;


         for (int i = 0; i < 4; i++)
         {
            this.tpdoMapping[i].Reset(i, this.nodeId);
         }

         this.producerHeartbeatTime = 0;

         this.NodeIdTextBox.Text = this.nodeId.ToString();

         int cobId = this.GetCobId(COBTypes.ERROR, this.nodeId);
         byte[] bootUpMsg = new byte[1];
         bootUpMsg[0] = 0;

         this.Transmit(cobId, bootUpMsg);

         base.Reset();
      }

      protected override void Start()
      {
         base.Start();

         for (int i = 0; i < 4; i++)
         {
            this.tpdoMapping[i].Start();
         }       

         this.DeviceStateLabel.Text = "RUNNING";

         this.heartbeatLimit = DateTime.Now.AddMilliseconds(this.producerHeartbeatTime);
      }

      protected override void Stop()
      {
         base.Stop();

         for (int i = 0; i < 4; i++)
         {
            this.tpdoMapping[i].Stop();
         }
         
         this.DeviceStateLabel.Text = "STOPPED";
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
         else if ((0x1800 <= index) && (0x1803 >= index))
         {
            int offset = (index - 0x1800);
            this.tpdoMapping[offset].LoadParameterData(subIndex, buffer, ref dataLength);
            valid = true;
         }
         else if ((0x1A00 <= index) && (0x1A03 >= index))
         {
            int offset = (index - 0x1A00);
            this.tpdoMapping[offset].LoadMapData(subIndex, buffer, ref dataLength);
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
         else if (0x2105 == index)
         {
            dataLength = this.MoveDeviceData(buffer, 0x65766173);
            valid = true;
         }
         else if ((0x2201 == index) && (0 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (byte)5);
            valid = true;
         }
         else if ((0x2201 == index) && (1 == subIndex))
         {
            byte x = (byte)this.status;
            dataLength = this.MoveDeviceData(buffer, x);
            valid = true;
         }
         else if ((0x2201 == index) && (2 == subIndex))
         {
            byte x = (byte)this.satilliteCount;
            dataLength = this.MoveDeviceData(buffer, x);
            valid = true;
         }
         else if ((0x2201 == index) && (3 == subIndex))
         {
            byte x = (byte)this.method;
            dataLength = this.MoveDeviceData(buffer, x);
            valid = true;
         }
         else if ((0x2201 == index) && (4 == subIndex))
         {
            float x = 0;
            dataLength = this.MoveDeviceData(buffer, x);
            valid = true;
         }
         else if ((0x2201 == index) && (5 == subIndex))
         {
            float x = 0;
            dataLength = this.MoveDeviceData(buffer, x);
            valid = true;
         }
         else if (0x2202 == index)
         {
            if ((false != this.UtcValidCheckBox.Checked) &&
                (0 != this.satilliteCount))
            {
               double value = Math.Abs(this.longitude);
               UInt16 degrees = (UInt16)value;
               float minutes = (float)((value - degrees) * 60);
               byte direction = (byte)((this.longitude > 0) ? 'E' : 'W');

               this.MoveDeviceData(buffer, minutes, 0);
               this.MoveDeviceData(buffer, degrees, 4);
               this.MoveDeviceData(buffer, direction, 6);
            }
            else
            {
               buffer[0] = 0;
               buffer[1] = 0;
               buffer[2] = 0;
               buffer[3] = 0;
               buffer[4] = 0;
               buffer[5] = 0;
               buffer[6] = 0;
            }

            dataLength = 7;
            valid = true;
         }
         else if (0x2203 == index)
         {
            if ((false != this.UtcValidCheckBox.Checked) &&
                (0 != this.satilliteCount))
            {
               double value = Math.Abs(this.latitude);
               UInt16 degrees = (UInt16)value;
               float minutes = (float)((value - degrees) * 60);
               byte direction = (byte)((this.latitude > 0) ? 'N' : 'S');

               this.MoveDeviceData(buffer, minutes, 0);
               this.MoveDeviceData(buffer, degrees, 4);
               this.MoveDeviceData(buffer, direction, 6);
            }
            else
            {
               buffer[0] = 0;
               buffer[1] = 0;
               buffer[2] = 0;
               buffer[3] = 0;
               buffer[4] = 0;
               buffer[5] = 0;
               buffer[6] = 0;
            }

            dataLength = 7;
            valid = true;
         }
         else if (0x2205 == index)
         {
            if ((false != this.UtcValidCheckBox.Checked) &&
                (0 != this.satilliteCount))
            {
               DateTime now = DateTime.UtcNow;

               buffer[0] = (byte)now.Hour;
               buffer[1] = (byte)now.Minute;
               buffer[2] = (byte)now.Second;
               buffer[3] = (byte)(now.Year % 100);
               buffer[4] = (byte)now.Month;
               buffer[5] = (byte)now.Day;
            }
            else
            {
               buffer[0] = 0;
               buffer[1] = 0;
               buffer[2] = 0;
               buffer[3] = 0;
               buffer[4] = 0;
               buffer[5] = 0;
            }

            dataLength = 6;
            valid = true;
         }

         return (valid);
      }

      protected override bool EvaluateDeviceDataSize(UInt16 index, byte subIndex, UInt32 dataLength)
      {
         bool valid = false;

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
         else if ((0x1800 <= index) && (0x1803 >= index))
         {
            int pdoMapOffset = (index - 0x1800);
            UInt32 value = 0;
            int shifter = 0;

            for (int i = 0; i < length; i++)
            {
               value |= ((UInt32)buffer[offset + i]) << shifter;
               shifter += 8;
            }

            valid = this.tpdoMapping[pdoMapOffset].StoreParameterData(subIndex, (int)length, value);
         }
         else if ((0x1A00 <= index) && (0x1A03 >= index))
         {
            int pdoMapOffset = (index - 0x1A00);
            UInt32 value = 0;
            int shifter = 0;

            for (int i = 0; i < length; i++)
            {
               value |= ((UInt32)buffer[offset + i]) << shifter;
               shifter += 8;
            }

            valid = this.tpdoMapping[pdoMapOffset].StoreMapData(subIndex, (int)length, value);
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
         else if (0x2105 == index)
         {
            UInt32 value = BitConverter.ToUInt32(buffer, offset);

            if (0x65766173 == value)
            {
               this.nvBaudRateCode = this.baudRateCode;
               this.nvNodeId = this.sdoNodeId;

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

      #region User Events

      private void SetStatusButton_Click(object sender, EventArgs e)
      {
         int status = 0;

         if (int.TryParse(this.StatusTextBox.Text, out status) != false)
         {
            this.status = status;
         }
      }

      private void SetSatelliteButton_Click(object sender, EventArgs e)
      {
         int satilliteCount = 0;

         if (int.TryParse(this.SatelliteTextBox.Text, out satilliteCount) != false)
         {
            this.satilliteCount = satilliteCount;
         }
      }

      private void SetMethodButton_Click(object sender, EventArgs e)
      {
         int method = 0;

         if (int.TryParse(this.MethodTextBox.Text, out method) != false)
         {
            this.method = method;
         }
      }

      private void SetLatitudeButton_Click(object sender, EventArgs e)
      {
         double latitude = 0;

         if (double.TryParse(this.LatitudeEntryTextBox.Text, out latitude) != false)
         {
            if ((latitude <= 90) && (latitude >= -90))
            {
               this.latitude = latitude;
               this.LatitudeReportTextBox.Text = this.latitude.ToString();
               this.LatitudeTrackBar.Value = (int)(latitude * 1000000);
            }
         }
      }

      private void SetLongitudeButton_Click(object sender, EventArgs e)
      {
         double longitude = 0;

         if (double.TryParse(this.LongitudeEntryTextBox.Text, out longitude) != false)
         {
            if ((longitude <= 180) && (longitude >= -180))
            {
               this.longitude = longitude;
               this.LongitudeReportTextBox.Text = this.longitude.ToString();
               this.LongitudeTrackBar.Value = (int)(longitude * 1000000);
            }
         }
      }

      private void LatitudeTrackBar_Scroll(object sender, EventArgs e)
      {
         double latitude = ((double)this.LatitudeTrackBar.Value) / 1000000;
         this.latitude = latitude;
         this.LatitudeReportTextBox.Text = this.latitude.ToString();
      }

      private void LongitudeTrackBar_Scroll(object sender, EventArgs e)
      {
         double longitude = ((double)this.LongitudeTrackBar.Value) / 1000000;
         this.longitude = longitude;
         this.LongitudeReportTextBox.Text = this.longitude.ToString();
      }

      #endregion

      #region Constructor

      public UlcRoboticsGps()
         : base()
      {
         this.InitializeComponent();

         this.deviceType = 0x12345678;
         this.errorStatus = 0;
         this.deviceName = "PCAN-GPS by ULC Robotics";
         this.version = "v1.23 Jul 10 2015 18:56:35";

         this.nvBaudRateCode = 5;
         this.nvNodeId = 31;

         this.tpdoMapping = new TPDOMapping[4];

         for (int i = 0; i < 4; i++)
         {
            this.tpdoMapping[i] = new TPDOMapping();
            this.tpdoMapping[i].OnPdoMappable = new TPDOMapping.PdoMappableHandler(this.TPdoMappableHandler);
            this.tpdoMapping[i].OnPdoSize = new TPDOMapping.PdoSizeHandler(this.TPdoSizeHandler);
            this.tpdoMapping[i].OnPdoData = new TPDOMapping.PdoDataHandler(this.TPdoDataHandler);
         }

         this.NodeIdTextBox.Text = this.nvNodeId.ToString();
      }

      #endregion

      #region Access Functions

      public override void LoadRegistry(RegistryKey appKey, string deviceTag)
      {
         object keyValue;
         byte parsedValue = 0;
         int parsedInt = 0;
         double parsedDouble = 0;

         keyValue = appKey.GetValue(deviceTag + "Enabled");
         this.EnabledCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "Description");
         this.DescriptionTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "BusId");
         string busId = (null != keyValue) ? keyValue.ToString() : "";
         this.SetBusId(busId);

         keyValue = appKey.GetValue(deviceTag + "nvBaudRateCode");
         this.nvBaudRateCode = (byte)((null != keyValue) ? (byte.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 6) : 6);

         keyValue = appKey.GetValue(deviceTag + "nvNodeId");
         this.nvNodeId = (byte)((null != keyValue) ? (byte.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 31) : 31);
         this.NodeIdTextBox.Text = this.nvNodeId.ToString();

         keyValue = appKey.GetValue(deviceTag + "Status");
         this.status = (int)((null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedInt) ? parsedInt : 0) : 0);
         this.StatusTextBox.Text = this.status.ToString();

         keyValue = appKey.GetValue(deviceTag + "SatilliteCount");
         this.satilliteCount = (int)((null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedInt) ? parsedInt : 0) : 0);
         this.SatelliteTextBox.Text = this.satilliteCount.ToString();

         keyValue = appKey.GetValue(deviceTag + "Method");
         this.method = (int)((null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedInt) ? parsedInt : 0) : 0);
         this.MethodTextBox.Text = this.method.ToString();

         keyValue = appKey.GetValue(deviceTag + "UtcValid");
         this.UtcValidCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "Latitude");
         this.latitude = (double)((null != keyValue) ? (double.TryParse(keyValue.ToString(), out parsedDouble) ? parsedDouble : 0) : 0);
         this.LatitudeEntryTextBox.Text = this.latitude.ToString();
         this.LatitudeReportTextBox.Text = this.latitude.ToString();
         this.LatitudeTrackBar.Value = (int)(this.latitude * 1000000);

         keyValue = appKey.GetValue(deviceTag + "Longitude");
         this.longitude = (double)((null != keyValue) ? (double.TryParse(keyValue.ToString(), out parsedDouble) ? parsedDouble : 0) : 0);
         this.LongitudeEntryTextBox.Text = this.longitude.ToString();
         this.LongitudeReportTextBox.Text = this.longitude.ToString();
         this.LongitudeTrackBar.Value = (int)(this.longitude * 1000000);
      }

      public override void SaveRegistry(RegistryKey appKey, string deviceTag)
      {
         appKey.SetValue(deviceTag + "Enabled", this.EnabledCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "Description", this.DescriptionTextBox.Text);
         appKey.SetValue(deviceTag + "BusId", this.GetBusId());
         appKey.SetValue(deviceTag + "nvBaudRateCode", this.nvBaudRateCode);
         appKey.SetValue(deviceTag + "nvNodeId", this.nvNodeId);
         appKey.SetValue(deviceTag + "Status", this.status);
         appKey.SetValue(deviceTag + "SatilliteCount", this.satilliteCount);
         appKey.SetValue(deviceTag + "Method", this.method);
         appKey.SetValue(deviceTag + "UtcValid", this.UtcValidCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "Latitude", this.latitude);
         appKey.SetValue(deviceTag + "Longitude", this.longitude);
      }

      public override void Read(XmlReader reader)
      {
         string name = reader.Name;
         reader.Read();

         if ("Enabled" == name)
         {
            this.EnabledCheckBox.Checked = ("0" != reader.Value) ? true : false;
         }
         else if ("Description" == name)
         {
            this.DescriptionTextBox.Text = reader.Value;
         }
         else if ("BusId" == name)
         {
            this.SetBusId(reader.Value);
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
         else if ("Status" == name)
         {
            int.TryParse(reader.Value, out this.status);
            this.StatusTextBox.Text = this.status.ToString();
         }
         else if ("SatilliteCount" == name)
         {
            int.TryParse(reader.Value, out this.satilliteCount);
            this.SatelliteTextBox.Text = this.satilliteCount.ToString();
         }
         else if ("Method" == name)
         {
            int.TryParse(reader.Value, out this.method);
            this.MethodTextBox.Text = this.method.ToString();
         }
         else if ("UtcValid" == name)
         {
            this.UtcValidCheckBox.Checked = ("0" != reader.Value) ? true : false;
         }
         else if ("Latitude" == name)
         {
            double.TryParse(reader.Value, out this.latitude);
            this.LatitudeEntryTextBox.Text = this.latitude.ToString();
            this.LatitudeReportTextBox.Text = this.latitude.ToString();
            this.LatitudeTrackBar.Value = (int)(this.latitude * 1000000);
         }
         else if ("Longitude" == name)
         {
            double.TryParse(reader.Value, out this.longitude);
            this.LongitudeEntryTextBox.Text = this.longitude.ToString();
            this.LongitudeReportTextBox.Text = this.longitude.ToString();
            this.LongitudeTrackBar.Value = (int)(this.longitude * 1000000);
         }
      }

      public override void Write(XmlWriter writer)
      {
         writer.WriteElementString("Enabled", (false != this.EnabledCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("Description", this.DescriptionTextBox.Text);
         writer.WriteElementString("NodeId", this.NodeIdTextBox.Text);
         writer.WriteElementString("BusId", this.GetBusId());
         writer.WriteElementString("nvBaudRateCode", this.nvBaudRateCode.ToString());
         writer.WriteElementString("nvNodeId", this.nvNodeId.ToString());
         writer.WriteElementString("Status", this.status.ToString());
         writer.WriteElementString("SatilliteCount", this.satilliteCount.ToString());
         writer.WriteElementString("Method", this.method.ToString());
         writer.WriteElementString("UtcValid", (false != this.UtcValidCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("Latitude", this.latitude.ToString());
         writer.WriteElementString("Longitude", this.longitude.ToString());
      }

      public override void SetBusId(string busId)
      {
         this.BusIdTextBox.Text = busId;
         base.SetBusId(busId);
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
            else if (COBTypes.SYNC == frameType)
            {
               for (int i = 0; i < 4; i++)
               {
                  this.tpdoMapping[i].SyncReceived();
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

         if (DeviceStates.running == this.deviceState)
         {
            for (int i = 0; i < 4; i++)
            {
               int cobId = 0;
               byte[] frame = null;
               this.tpdoMapping[i].Update(ref cobId, ref frame);

               if (null != frame)
               {
                  bool transmitted = this.Transmit(cobId, frame);

                  if (false != transmitted)
                  {
                     this.tpdoMapping[i].Transmitted();
                  }
               }
            }
         }
      }

      #endregion

   }
}
