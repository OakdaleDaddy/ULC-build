
namespace CrossBore.BusSim
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

   public partial class PeakDigitalIoDeviceControl : DeviceControl
   {
      #region Fields

      private UInt32 deviceType;
      private UInt32 errorStatus;
      private string deviceName;
      private string version;

      private byte nvBaudRateCode;
      private byte nvNodeId;

      private bool active;
      private byte baudRateCode;
      private byte nodeId;

      private UInt32 sdoConsumerHeartbeat;
      private byte consumerHeartbeatNode;
      private UInt16 consumerHeartbeatTime;
      private bool consumerHeartbeatActive;
      private DateTime consumerHeartbeatTimeLimit;

      private UInt16 producerHeartbeatTime;

      private TPDOMapping[] tpdoMapping;
      private RPDOMapping[] rpdoMapping;

      private DateTime heartbeatLimit;

      private byte digitalInput;

      private byte digitalOutput;
      private byte polarityDigitalOutput;
      private byte digitalOutputErrorMode;
      private byte digitalOutputErrorValue;

      #endregion

      #region Helper Functions

      private void CheckTpdoMappings(UInt16 pdoIndex, byte pdoSubIndex)
      {
         for (int i = 0; i < 4; i++)
         {
            if (this.tpdoMapping[i].Contains(pdoIndex, pdoSubIndex) != false)
            {
               this.tpdoMapping[i].Activate();
            }
         }
      }

      private void UpdateDigitalInput(byte mask, bool set)
      {
         if (false != set)
         {
            this.digitalInput |= mask;
         }
         else
         {
            this.digitalInput &= (byte)(~mask);
         }

         this.CheckTpdoMappings(0x6000, 1);
      }

      private void SendEmergency(string emergencyCode)
      {
         int cobId = this.GetCobId(COBTypes.EMGY, this.nodeId);
         byte[] emergencyMessage = new byte[((emergencyCode.Length + 1)/2)];
         StringBuilder sb = new StringBuilder();
         int length = emergencyMessage.Length;

         for (int i = 0; i < length; i++)
         {
            int firstIindex = (i * 2);
            int secondIndex = (i * 2) + 1;

            sb.Clear();

            if (firstIindex < emergencyCode.Length)
            {
               sb.Append(emergencyCode[firstIindex]);
            }

            if (secondIndex < emergencyCode.Length)
            {
               sb.Append(emergencyCode[secondIndex]);
            }

            byte value = 0;
            int storageIndex = length - i - 1;

            if (byte.TryParse(sb.ToString(), System.Globalization.NumberStyles.HexNumber, null, out value) != false)
            {
               emergencyMessage[storageIndex] = value;
            }
            else
            {
               emergencyMessage[storageIndex] = 0;
            }
         }

         this.Transmit(cobId, emergencyMessage);
      }

      private void UpdateOutputs(bool preOp)
      {
         if (false == preOp)
         {
            byte digitalOutputValue = (byte)(this.digitalOutput ^ this.polarityDigitalOutput);

            this.DOut0TextBox.Text = ((digitalOutputValue & 0x01) != 0) ? "1" : "0";
            this.DOut1TextBox.Text = ((digitalOutputValue & 0x02) != 0) ? "1" : "0";
            this.DOut2TextBox.Text = ((digitalOutputValue & 0x04) != 0) ? "1" : "0";
            this.DOut3TextBox.Text = ((digitalOutputValue & 0x08) != 0) ? "1" : "0";
         }
         else
         {
            if ((this.digitalOutputErrorMode & 0x01) != 0)
            {
               this.DOut0TextBox.Text = ((this.digitalOutputErrorValue & 0x01) != 0) ? "1" : "0";
            }

            if ((this.digitalOutputErrorMode & 0x02) != 0)
            {
               this.DOut1TextBox.Text = ((this.digitalOutputErrorValue & 0x02) != 0) ? "1" : "0";
            }

            if ((this.digitalOutputErrorMode & 0x04) != 0)
            {
               this.DOut2TextBox.Text = ((this.digitalOutputErrorValue & 0x04) != 0) ? "1" : "0";
            }

            if ((this.digitalOutputErrorMode & 0x08) != 0)
            {
               this.DOut3TextBox.Text = ((this.digitalOutputErrorValue & 0x08) != 0) ? "1" : "0";
            }
         }
      }

      #endregion

      #region Delegates

      private bool TPdoMappableHandler(UInt16 index, byte subIndex)
      {
         bool result = false;

         if ((0x6000 == index) && (1 == subIndex))
         {
            result = true;
         }

         return (result);
      }

      private int TPdoSizeHandler(UInt16 index, byte subIndex)
      {
         byte result = 0;

         if ((0x6000 == index) && (1 == subIndex))
         {
            result = 1;
         }

         return (result);
      }

      private int TPdoDataHandler(UInt16 index, byte subIndex, byte[] buffer, int offset)
      {
         byte[] pdoData = new byte[8];
         UInt32 dataLength = 0;
         this.LoadDeviceData(index, subIndex, pdoData, ref dataLength);

         for (int i = 0; i < dataLength; i++)
         {
            buffer[offset + i] = pdoData[i];
         }

         return ((int)dataLength);
      }

      private bool RPdoMappableHandler(UInt16 index, byte subIndex)
      {
         bool result = false;

         if ((0x6200 == index) && (1 == subIndex))
         {
            result = true;
         }

         return (result);
      }

      private int RPdoSizeHandler(UInt16 index, byte subIndex)
      {
         int result = 0;

         if ((0x6200 == index) && (1 == subIndex))
         {
            result = 1;
         }

         return (result);
      }

      private bool RPdoDataHandler(UInt16 index, byte subIndex, byte[] data, int offset, UInt32 length)
      {
         bool result = this.StoreDeviceData(index, subIndex, data, offset, length);
         return (result);
      }

      #endregion

      #region Device Specific Functions

      protected void Reset(bool fromPowerUp, bool preOpOnly)
      {
         this.DeviceStateLabel.Text = "PRE-OP";

         this.baudRateCode = this.nvBaudRateCode;
         this.nodeId = this.nvNodeId;
         this.NodeIdTextBox.Text = this.nvNodeId.ToString();

         for (int i = 0; i < 4; i++)
         {
            this.tpdoMapping[i].Reset(i, this.nodeId);
            this.rpdoMapping[i].Reset(i, this.nodeId);
         }

         this.consumerHeartbeatActive = false;
         this.consumerHeartbeatNode = 0;
         this.sdoConsumerHeartbeat = 0;
         this.producerHeartbeatTime = 0;

         if (false != this.active)
         {
            this.UpdateOutputs(true);

            if (false == preOpOnly)
            {
               int cobId = this.GetCobId(COBTypes.ERROR, this.nodeId);
               byte[] bootUpMsg = new byte[1];
               bootUpMsg[0] = 0;

               this.Transmit(cobId, bootUpMsg);
               this.SendEmergency("0000000000000000");
            }

            base.Reset();
         }
      }

      protected override void Start()
      {
         base.Start();

         for (int i = 0; i < 4; i++)
         {
            this.tpdoMapping[i].Start();
            this.rpdoMapping[i].Start();

            this.tpdoMapping[i].Activate();
         }

         this.DeviceStateLabel.Text = "RUNNING";
      }

      protected override void Stop()
      {
         base.Stop();

         for (int i = 0; i < 4; i++)
         {
            this.tpdoMapping[i].Stop();
            this.rpdoMapping[i].Stop();
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
         else if ((0x1016 == index) && (0 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (byte)1);
            valid = true;
         }
         else if ((0x1016 == index) && (1 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.sdoConsumerHeartbeat);
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
         else if ((0x1400 <= index) && (0x1403 >= index))
         {
            int offset = (index - 0x1400);
            this.rpdoMapping[offset].LoadParameterData(subIndex, buffer, ref dataLength);
            valid = true;
         }
         else if ((0x1600 <= index) && (0x1603 >= index))
         {
            int offset = (index - 0x1600);
            this.rpdoMapping[offset].LoadMapData(subIndex, buffer, ref dataLength);
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
         else if (0x6000 == index)
         {
            if (0 == subIndex)
            {
               dataLength = this.MoveDeviceData(buffer, (byte)1);
               valid = true;
            }
            else if (1 == subIndex)
            {
               dataLength = this.MoveDeviceData(buffer, this.digitalInput);
               valid = true;
            }            
         }            
         else if (0x6200 == index)
         {
            if (0 == subIndex)
            {
               dataLength = this.MoveDeviceData(buffer, (byte)1);
               valid = true;
            }
            else if (1 == subIndex)
            {
               dataLength = this.MoveDeviceData(buffer, this.digitalOutput);
               valid = true;
            }            
         }
         else if (0x6202 == index)
         {
            if (0 == subIndex)
            {
               dataLength = this.MoveDeviceData(buffer, (byte)1);
               valid = true;
            }
            else if (1 == subIndex)
            {
               dataLength = this.MoveDeviceData(buffer, this.polarityDigitalOutput);
               valid = true;
            }
         }
         else if (0x6206 == index)
         {
            if (0 == subIndex)
            {
               dataLength = this.MoveDeviceData(buffer, (byte)1);
               valid = true;
            }
            else if (1 == subIndex)
            {
               dataLength = this.MoveDeviceData(buffer, this.digitalOutputErrorMode);
               valid = true;
            }
         }
         else if (0x6207 == index)
         {
            if (0 == subIndex)
            {
               dataLength = this.MoveDeviceData(buffer, (byte)1);
               valid = true;
            }
            else if (1 == subIndex)
            {
               dataLength = this.MoveDeviceData(buffer, this.digitalOutputErrorValue);
               valid = true;
            }
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

         if ((0x1016 == index) && (1 == subIndex))
         {
            if (4 == length)
            {
               this.sdoConsumerHeartbeat = BitConverter.ToUInt32(buffer, offset);

               this.consumerHeartbeatNode = (byte)((this.sdoConsumerHeartbeat >> 16) & 0x7F);
               this.consumerHeartbeatTime = (UInt16)(this.sdoConsumerHeartbeat & 0xFFFF);
               this.consumerHeartbeatActive = false;

               valid = true;
            }
         }
         else if (0x1017 == index)
         {
            if (2 == length)
            {
               this.producerHeartbeatTime = BitConverter.ToUInt16(buffer, offset);
               this.heartbeatLimit = DateTime.Now.AddMilliseconds(this.producerHeartbeatTime);
               valid = true;
            }
         }
         else if ((0x1400 <= index) && (0x1403 >= index))
         {
            int pdoMapOffset = (index - 0x1400);
            UInt32 value = this.ExtractUInt32(buffer, offset, length);
            valid = this.rpdoMapping[pdoMapOffset].StoreParameterData(subIndex, (int)length, value);
         }
         else if ((0x1600 <= index) && (0x1603 >= index))
         {
            int pdoMapOffset = (index - 0x1600);
            UInt32 value = this.ExtractUInt32(buffer, offset, length);
            valid = this.rpdoMapping[pdoMapOffset].StoreMapData(subIndex, (int)length, value);
         }
         else if ((0x1800 <= index) && (0x1803 >= index))
         {
            int pdoMapOffset = (index - 0x1800);
            UInt32 value = this.ExtractUInt32(buffer, offset, length);
            valid = this.tpdoMapping[pdoMapOffset].StoreParameterData(subIndex, (int)length, value);
         }
         else if ((0x1A00 <= index) && (0x1A03 >= index))
         {
            int pdoMapOffset = (index - 0x1A00);
            UInt32 value = this.ExtractUInt32(buffer, offset, length);
            valid = this.tpdoMapping[pdoMapOffset].StoreMapData(subIndex, (int)length, value);
         }
         else if ((0x1F50 == index) && (3 == subIndex))
         {
            if (4 == length)
            {
               char ch1 = (char)(buffer[offset + 0]);
               char ch2 = (char)(buffer[offset + 1]);
               char ch3 = (char)(buffer[offset + 2]);
               char ch4 = (char)(buffer[offset + 3]);

               if (('B' == ch1) && ('P' == ch2) && ('S' == ch3))
               {
                  if (('0' == ch4) || ('1' == ch4) || ('2' == ch4) || ('3' == ch4) || ('4' == ch4) || ('6' == ch4))
                  {
                     nvBaudRateCode = (byte)(ch4 - 0x30);
                     valid = true;
                  }
               }
               else if (('N' == ch1) && ('I' == ch2))
               {
                  string idString = Encoding.UTF8.GetString(buffer, offset + 2, 2);
                  byte nodeId = 0;

                  if (byte.TryParse(idString, System.Globalization.NumberStyles.HexNumber, null, out nodeId) != false)
                  {
                     nvNodeId = nodeId;
                     valid = true;
                  }
               }
            }
         }
         else if (0x6200 == index)
         {
            if ((1 == length) &&
                (1 == subIndex))
            {
               this.digitalOutput = (byte)(this.ExtractUInt32(buffer, offset, length));
               this.UpdateOutputs(false);
               valid = true;
            }
         }
         else if (0x6202 == index)
         {
            if ((1 == length) &&
                (1 == subIndex))
            {
               this.polarityDigitalOutput = (byte)(this.ExtractUInt32(buffer, offset, length));
               valid = true;
            }
         }
         else if (0x6206 == index)
         {
            if ((1 == length) &&
                (1 == subIndex))
            {
               this.digitalOutputErrorMode = (byte)(this.ExtractUInt32(buffer, offset, length));
               valid = true;
            }
         }
         else if (0x6207 == index)
         {
            if ((1 == length) &&
                (1 == subIndex))
            {
               this.digitalOutputErrorValue = (byte)(this.ExtractUInt32(buffer, offset, length));
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
                  this.Reset(false, false);
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
         this.rpdoMapping[0].StoreFrameData(frame);
      }

      private void ProcessPdo2Message(byte[] frame)
      {
         this.rpdoMapping[1].StoreFrameData(frame);
      }

      private void ProcessPdo3Message(byte[] frame)
      {
         this.rpdoMapping[2].StoreFrameData(frame);
      }

      private void ProcessPdo4Message(byte[] frame)
      {
         this.rpdoMapping[3].StoreFrameData(frame);
      }

      #endregion

      #region Form Events

      private void DIn0LevelCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateDigitalInput(0x01, this.DIn0LevelCheckBox.Checked);
      }

      private void DIn1LevelCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateDigitalInput(0x02, this.DIn1LevelCheckBox.Checked);
      }

      private void DIn2LevelCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateDigitalInput(0x04, this.DIn2LevelCheckBox.Checked);
      }

      private void DIn3LevelCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateDigitalInput(0x08, this.DIn3LevelCheckBox.Checked);
      }

      private void DIn4LevelCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateDigitalInput(0x10, this.DIn4LevelCheckBox.Checked);
      }

      private void DIn5LevelCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateDigitalInput(0x20, this.DIn5LevelCheckBox.Checked);
      }

      private void DIn6LevelCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateDigitalInput(0x40, this.DIn6LevelCheckBox.Checked);
      }

      private void DIn7LevelCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.UpdateDigitalInput(0x80, this.DIn7LevelCheckBox.Checked);
      }

      #endregion

      #region Constructor

      public PeakDigitalIoDeviceControl()
         : base()
      {
         this.InitializeComponent();

         this.deviceType = 0x000B0191;
         this.errorStatus = 0;
         this.deviceName = "SIM-PC01";
         this.version = "R209";

         this.nvBaudRateCode = 4;
         this.nvNodeId = 64;

         this.tpdoMapping = new TPDOMapping[4];
         this.rpdoMapping = new RPDOMapping[4];

         for (int i = 0; i < 4; i++)
         {
            this.tpdoMapping[i] = new TPDOMapping();
            this.tpdoMapping[i].OnPdoMappable = new TPDOMapping.PdoMappableHandler(this.TPdoMappableHandler);
            this.tpdoMapping[i].OnPdoSize = new TPDOMapping.PdoSizeHandler(this.TPdoSizeHandler);
            this.tpdoMapping[i].OnPdoData = new TPDOMapping.PdoDataHandler(this.TPdoDataHandler);
         }

         for (int i = 0; i < 4; i++)
         {
            this.rpdoMapping[i] = new RPDOMapping();
            this.rpdoMapping[i].OnPdoMappable = new RPDOMapping.PdoMappableHandler(this.RPdoMappableHandler);
            this.rpdoMapping[i].OnPdoSize = new RPDOMapping.PdoSizeHandler(this.RPdoSizeHandler);
            this.rpdoMapping[i].OnPdoData = new RPDOMapping.PdoDataHandler(this.RPdoDataHandler);
         }
      }

      #endregion

      #region Access Functions

      public override void LoadRegistry(RegistryKey appKey, string deviceTag)
      {
         object keyValue;
         byte parsedValue = 0;

         keyValue = appKey.GetValue(deviceTag + "Enabled");
         this.EnabledCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "Description");
         this.DescriptionTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "BusId");
         string busId = (null != keyValue) ? keyValue.ToString() : "";
         this.SetBusId(busId);


         keyValue = appKey.GetValue(deviceTag + "nvBaudRateCode");
         this.nvBaudRateCode = (byte)((null != keyValue) ? (byte.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 4) : 4);

         keyValue = appKey.GetValue(deviceTag + "nvNodeId");
         this.nvNodeId = (byte)((null != keyValue) ? (byte.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 64) : 64);
         this.NodeIdTextBox.Text = this.nvNodeId.ToString();


         keyValue = appKey.GetValue(deviceTag + "DIn0Level");
         this.DIn0LevelCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "DIn1Level");
         this.DIn1LevelCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "DIn2Level");
         this.DIn2LevelCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "DIn3Level");
         this.DIn3LevelCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "DIn4Level");
         this.DIn4LevelCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "DIn5Level");
         this.DIn5LevelCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "DIn6Level");
         this.DIn6LevelCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "DIn7Level");
         this.DIn7LevelCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;
      }

      public override void SaveRegistry(RegistryKey appKey, string deviceTag)
      {
         appKey.SetValue(deviceTag + "Enabled", this.EnabledCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "Description", this.DescriptionTextBox.Text);
         appKey.SetValue(deviceTag + "BusId", this.GetBusId());

         appKey.SetValue(deviceTag + "nvBaudRateCode", this.nvBaudRateCode);
         appKey.SetValue(deviceTag + "nvNodeId", this.nvNodeId);

         appKey.SetValue(deviceTag + "DIn0Level", this.DIn0LevelCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "DIn1Level", this.DIn1LevelCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "DIn2Level", this.DIn2LevelCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "DIn3Level", this.DIn3LevelCheckBox.Checked ? "1" : "0");         
         appKey.SetValue(deviceTag + "DIn4Level", this.DIn4LevelCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "DIn5Level", this.DIn5LevelCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "DIn6Level", this.DIn6LevelCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "DIn7Level", this.DIn7LevelCheckBox.Checked ? "1" : "0");
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

         else if ("DIn0Level" == name)
         {
            this.DIn0LevelCheckBox.Checked = ("0" != reader.Value) ? true : false;
         }
         else if ("DIn1Level" == name)
         {
            this.DIn1LevelCheckBox.Checked = ("0" != reader.Value) ? true : false;
         }
         else if ("DIn2Level" == name)
         {
            this.DIn2LevelCheckBox.Checked = ("0" != reader.Value) ? true : false;
         }
         else if ("DIn3Level" == name)
         {
            this.DIn3LevelCheckBox.Checked = ("0" != reader.Value) ? true : false;
         }
         else if ("DIn4Level" == name)
         {
            this.DIn4LevelCheckBox.Checked = ("0" != reader.Value) ? true : false;
         }
         else if ("DIn5Level" == name)
         {
            this.DIn5LevelCheckBox.Checked = ("0" != reader.Value) ? true : false;
         }
         else if ("DIn6Level" == name)
         {
            this.DIn6LevelCheckBox.Checked = ("0" != reader.Value) ? true : false;
         }
         else if ("DIn7Level" == name)
         {
            this.DIn7LevelCheckBox.Checked = ("0" != reader.Value) ? true : false;
         }
      }

      public override void Write(XmlWriter writer)
      {
         writer.WriteElementString("Enabled", (false != this.EnabledCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("Description", this.DescriptionTextBox.Text);
         writer.WriteElementString("BusId", this.GetBusId());

         writer.WriteElementString("nvBaudRateCode", this.nvBaudRateCode.ToString());
         writer.WriteElementString("nvNodeId", this.nvNodeId.ToString());

         writer.WriteElementString("DIn0Level", (false != this.DIn0LevelCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("DIn1Level", (false != this.DIn1LevelCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("DIn2Level", (false != this.DIn2LevelCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("DIn3Level", (false != this.DIn3LevelCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("DIn4Level", (false != this.DIn4LevelCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("DIn5Level", (false != this.DIn5LevelCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("DIn6Level", (false != this.DIn6LevelCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("DIn7Level", (false != this.DIn7LevelCheckBox.Checked) ? "1" : "0");
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

         this.digitalInput = 0;

         this.digitalOutput = 0;
         this.polarityDigitalOutput = 0;
         this.digitalOutputErrorMode = 0;
         this.digitalOutputErrorValue = 0;

         this.active = this.EnabledCheckBox.Checked;
         this.Reset(true, false);

         if (false == this.active)
         {
            this.DeviceStateLabel.Text = "DISABLED";
         }
      }

      public override void PowerDown()
      {
         this.active = false;

         this.EnabledCheckBox.Enabled = true;
         this.DescriptionTextBox.Enabled = true;

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
            else if (COBTypes.ERROR == frameType)
            {
               if ((0 != this.sdoConsumerHeartbeat) &&
                   (nodeId == this.consumerHeartbeatNode))
               {
                  this.consumerHeartbeatActive = true;
                  this.consumerHeartbeatTimeLimit = DateTime.Now.AddMilliseconds(this.consumerHeartbeatTime);
               }
            }
         }
      }

      public override void UpdateDevice()
      {
         DateTime now = DateTime.Now;

         if ((false != this.consumerHeartbeatActive) &&
               (now > this.consumerHeartbeatTimeLimit) &&
               (DeviceStates.preop != this.deviceState))
         {
            this.SendEmergency("0000000000008130");
            this.Reset(false, true);
         }

         if (0 != this.producerHeartbeatTime)
         {
            if (now > this.heartbeatLimit)
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
         else
         {
         }
      }

      #endregion
   }
}
