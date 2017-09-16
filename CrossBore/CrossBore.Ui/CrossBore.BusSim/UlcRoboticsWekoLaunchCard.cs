
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

   public partial class UlcRoboticsWekoLaunchCard : DeviceControl
   {
      #region Fields

      private UInt32 deviceType;
      private UInt32 errorStatus;
      private string deviceName;
      private string version;

      private byte nvNodeId;

      private bool active;
      private byte nodeId;

      private UInt32 consumerHeartbeatTime;
      private UInt16 producerHeartbeatTime;

      private TPDOMapping[] tpdoMapping;
      private RPDOMapping[] rpdoMapping;

      private List<ErrorData> errorCodeList;
      private UInt32 subSystemStatus;

      private byte cameraSelect;
      private UInt16[] ledIntensity;

      private byte mcuTemperature;
      private byte mcuErrorTemperature;

      private byte workingConsumerHeartbeatNode;
      private UInt16 workingConsumerHeartbeatTime;
      private bool workingConsumerHeartbeatActive;
      private DateTime workingConsumerHeartbeatTimeLimit;

      private DateTime workingProducerHeartbeatTimeLimit;

      #endregion

      #region Helper Functions

      private void SetLabelValue(UInt16 index, byte subIndex, Label label, string description, int value, int precision)
      {
         if (null != label)
         {
            string valueFormatString = "{" + string.Format("0:X{0}", precision) + "}";
            string valueString = string.Format(valueFormatString, value);

            string indexString = string.Format("0x{0:X4}", index);
            string subIndexString = (0 == subIndex) ? " " : string.Format("-{0:X2} ", subIndex);

            label.Text = indexString + subIndexString + description + "..." + valueString;
         }
      }

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

      private void SetValue(UInt16 index, byte subIndex, Label label, string description, ref byte existingValue, byte assignedValue, int precision)
      {
         this.SetLabelValue(index, subIndex, label, description, assignedValue, precision);

         if (existingValue != assignedValue)
         {
            existingValue = assignedValue;
            this.CheckTpdoMappings(index, subIndex);
         }
      }

      private void SetValue(UInt16 index, byte subIndex, Label label, string description, ref Int16 existingValue, Int16 assignedValue, int precision)
      {
         this.SetLabelValue(index, subIndex, label, description, assignedValue, precision);

         if (existingValue != assignedValue)
         {
            existingValue = assignedValue;
            this.CheckTpdoMappings(index, subIndex);
         }
      }

      private void SetValue(UInt16 index, byte subIndex, Label label, string description, ref UInt16 existingValue, UInt16 assignedValue, int precision)
      {
         this.SetLabelValue(index, subIndex, label, description, assignedValue, precision);

         if (existingValue != assignedValue)
         {
            existingValue = assignedValue;
            this.CheckTpdoMappings(index, subIndex);
         }
      }

      private void SetValue(UInt16 index, byte subIndex, Label label, string description, ref Int32 existingValue, Int32 assignedValue, int precision)
      {
         this.SetLabelValue(index, subIndex, label, description, assignedValue, precision);

         if (existingValue != assignedValue)
         {
            existingValue = assignedValue;
            this.CheckTpdoMappings(index, subIndex);
         }
      }

      private void SetValue(UInt16 index, byte subIndex, Label label, string description, ref UInt32 existingValue, UInt32 assignedValue, int precision)
      {
         this.SetLabelValue(index, subIndex, label, description, (int)assignedValue, precision);

         if (existingValue != assignedValue)
         {
            existingValue = assignedValue;
            this.CheckTpdoMappings(index, subIndex);
         }
      }

      private byte GetErrorRegister(int index, bool fault)
      {
         byte result = (byte)(index & 0x1F);
         result |= (byte)((false != fault) ? 0x20 : 0x00);
         result |= 0x80;
         return (result);
      }

      private void StoreError(int subSystem, bool fault, UInt32 errorCode, UInt32 additionalData)
      {
         ErrorData errorData = null;

         for (int i = 0; i < this.errorCodeList.Count; i++)
         {
            if (errorCode == this.errorCodeList[i].Code)
            {
               errorData = this.errorCodeList[i];
               break;
            }
         }

         if (null == errorData)
         {
            this.errorCodeList.Add(new ErrorData(subSystem, errorCode, additionalData));
         }

         this.SubSystemStatus |= (UInt32)(1 << subSystem);
      }

      private void RemoveError(UInt32 errorCode)
      {
         ErrorData errorData = null;

         for (int i = 0; i < this.errorCodeList.Count; i++)
         {
            if (errorCode == this.errorCodeList[i].Code)
            {
               errorData = this.errorCodeList[i];
               break;
            }
         }

         if (null != errorData)
         {
            this.errorCodeList.Remove(errorData);
         }

         UInt32 subSystemStatus = this.SubSystemStatus;
         UInt32 subSystemCache = 0;

         for (int i = 0; i < this.errorCodeList.Count; i++)
         {
            UInt32 code = this.errorCodeList[i].Code;
            int subSystemIndex = (int)((code >> 16) & 0x1F);
            subSystemCache |= (UInt32)(1 << subSystemIndex);
         }

         if (subSystemCache != this.SubSystemStatus)
         {
            this.SubSystemStatus = subSystemCache;

            int emergencyCobId = this.GetCobId(COBTypes.EMGY, this.nodeId);
            byte[] emergencyMsg = new byte[8];

            emergencyMsg[0] = 0;
            emergencyMsg[1] = 0;
            emergencyMsg[2] = 0x80;
            emergencyMsg[3] = 0;

            byte[] subSystemStatusBytes = BitConverter.GetBytes(this.SubSystemStatus);
            emergencyMsg[4] = subSystemStatusBytes[0];
            emergencyMsg[5] = subSystemStatusBytes[1];
            emergencyMsg[6] = subSystemStatusBytes[2];
            emergencyMsg[7] = subSystemStatusBytes[3];

            this.Transmit(emergencyCobId, emergencyMsg);
         }
      }

      private void SendEmergency(string emergencyCode)
      {
         int cobId = this.GetCobId(COBTypes.EMGY, this.nodeId);
         byte[] emergencyMessage = new byte[((emergencyCode.Length + 1) / 2)];
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
      
      #endregion

      #region Delegates

      private bool TPdoMappableHandler(UInt16 index, byte subIndex)
      {
         bool result = false;

         if ((0x2311 == index) && (0x01 == subIndex))
         {
            result = true;
         }

         return (result);
      }

      private int TPdoSizeHandler(UInt16 index, byte subIndex)
      {
         byte result = 0;

         if ((0x2311 == index) && (0x01 == subIndex))
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

         return (result);
      }

      private int RPdoSizeHandler(UInt16 index, byte subIndex)
      {
         int result = 0;

         return (result);
      }

      private bool RPdoDataHandler(UInt16 index, byte subIndex, byte[] data, int offset, UInt32 length)
      {
         bool result = this.StoreDeviceData(index, subIndex, data, offset, length);
         return (result);
      }

      #endregion

      #region Properties

      #region Communication

      private UInt32 ConsumerHeartbeatTime
      {
         set
         {
            this.SetValue(0x1016, 0x00, this.ConsumerHeartbeatTimeLabel, "Consumer Heartbeat Time", ref this.consumerHeartbeatTime, value, 8);

            this.workingConsumerHeartbeatNode = (byte)((this.consumerHeartbeatTime >> 16) & 0x7F);
            this.workingConsumerHeartbeatTime = (UInt16)(this.consumerHeartbeatTime & 0xFFFF);
            this.workingConsumerHeartbeatActive = false;
         }

         get
         {
            return (this.consumerHeartbeatTime);
         }
      }

      private UInt16 ProducerHeartbeatTime
      {
         set
         {
            this.SetValue(0x1017, 0x00, ProducerHeartbeatTimeLabel, "Producer Heartbeat Time", ref this.producerHeartbeatTime, value, 4);

            this.workingProducerHeartbeatTimeLimit = DateTime.Now.AddMilliseconds(this.producerHeartbeatTime);
         }

         get
         {
            return (this.producerHeartbeatTime);
         }
      }

      #endregion

      #region Emergency

      private UInt32 SubSystemStatus
      {
         set
         {
            this.SetValue(0x5000, 0x00, this.SubSystemStatusLabel, "Sub System Status", ref this.subSystemStatus, value, 8);
         }

         get
         {
            return (this.subSystemStatus);
         }
      }

      #endregion

      #region Camera

      private byte CameraSelect
      {
         set
         {
            this.SetValue(0x2301, 0x00, this.CameraSelectLabel, "Camera Select", ref this.cameraSelect, value, 2);
         }

         get
         {
            return (this.cameraSelect);
         }
      }

      private UInt16 LedIntensity1
      {
         set
         {
            this.SetValue(0x2303, 0x01, this.LedIntensity1Label, "LED 1 Intensity", ref this.ledIntensity[0], value, 4);
         }

         get
         {
            return (this.ledIntensity[0]);
         }
      }

      private UInt16 LedIntensity2
      {
         set
         {
            this.SetValue(0x2303, 0x02, this.LedIntensity2Label, "LED 2 Intensity", ref this.ledIntensity[1], value, 4);
         }

         get
         {
            return (this.ledIntensity[1]);
         }
      }

      private UInt16 LedIntensity3
      {
         set
         {
            this.SetValue(0x2303, 0x02, this.LedIntensity3Label, "LED 3 Intensity", ref this.ledIntensity[2], value, 4);
         }

         get
         {
            return (this.ledIntensity[2]);
         }
      }

      private UInt16 LedIntensity4
      {
         set
         {
            this.SetValue(0x2303, 0x02, this.LedIntensity4Label, "LED 4 Intensity", ref this.ledIntensity[3], value, 4);
         }

         get
         {
            return (this.ledIntensity[3]);
         }
      }

      private UInt16 LedIntensity5
      {
         set
         {
            this.SetValue(0x2303, 0x02, this.LedIntensity5Label, "LED 5 Intensity", ref this.ledIntensity[4], value, 4);
         }

         get
         {
            return (this.ledIntensity[4]);
         }
      }

      private UInt16 LedIntensity6
      {
         set
         {
            this.SetValue(0x2303, 0x02, this.LedIntensity6Label, "LED 6 Intensity", ref this.ledIntensity[5], value, 4);
         }

         get
         {
            return (this.ledIntensity[5]);
         }
      }

      #endregion

      #region MCU

      private byte McuTemperature
      {
         set
         {
            this.SetValue(0x2311, 0x01, this.McuTemperatureLabel, "MCU Temperature", ref this.mcuTemperature, value, 2);
         }

         get
         {
            return (this.mcuTemperature);
         }
      }

      private byte McuErrorTemperature
      {
         set
         {
            this.SetValue(0x2311, 0x02, McuErrorTemperatureLabel, "MCU Error Temperature", ref this.mcuErrorTemperature, value, 2);
         }

         get
         {
            return (this.mcuErrorTemperature);
         }
      }

      #endregion

      #endregion

      #region Device Specific Functions

      protected void Reset(bool fromPowerUp)
      {
         if (false != this.active)
         {
            this.DeviceStateLabel.Text = "PRE-OP";

            this.nodeId = this.nvNodeId;
            this.NodeIdTextBox.Text = this.nodeId.ToString();

            if (false != fromPowerUp)
            {
               this.CameraSelect = 0;
               this.LedIntensity1 = 15;
               this.LedIntensity2 = 15;
               this.LedIntensity3 = 15;
               this.LedIntensity4 = 15;
               this.LedIntensity5 = 15;
               this.LedIntensity6 = 15;

               this.McuTemperature = 23;
               this.McuErrorTemperature = 0;
            }

            this.ConsumerHeartbeatTime = 0;
            this.ProducerHeartbeatTime = 0;

            for (int i = 0; i < this.rpdoMapping.Length; i++)
            {
               this.rpdoMapping[i].Reset(i, this.nodeId);
            }

            for (int i = 0; i < this.tpdoMapping.Length; i++)
            {
               this.tpdoMapping[i].Reset(i, this.nodeId);
            }

            this.SubSystemStatus = 0;
            this.errorCodeList.Clear();
            
            if (false != this.EmergencyResetCheckBox.Checked)
            {
               UInt16 additionalData = 0;

               if (false == fromPowerUp)
               {
                  UInt16.TryParse(this.EmergencyResetDataTextBox.Text, out additionalData);
               }

               byte[] additionalDataBytes = BitConverter.GetBytes(additionalData);

               int emergencyCobId = this.GetCobId(COBTypes.EMGY, this.nodeId);
               byte[] emergencyMsg = new byte[8];
               emergencyMsg[0] = 0x01;
               emergencyMsg[1] = 0x60;
               emergencyMsg[2] = 0x01;
               emergencyMsg[3] = additionalDataBytes[0];
               emergencyMsg[4] = additionalDataBytes[1];
               emergencyMsg[5] = 0x00;
               emergencyMsg[6] = 0x00;
               emergencyMsg[7] = 0x00;

               this.Transmit(emergencyCobId, emergencyMsg);
            }

            int resetCobId = this.GetCobId(COBTypes.ERROR, this.nodeId);
            byte[] resetMsg = new byte[1];
            resetMsg[0] = 0;
            this.Transmit(resetCobId, resetMsg);

            base.Reset();
         }
      }

      protected override void Start()
      {
         if (false != this.active)
         {
            base.Start();

            for (int i = 0; i < this.rpdoMapping.Length; i++)
            {
               this.rpdoMapping[i].Start();
            }

            for (int i = 0; i < this.tpdoMapping.Length; i++)
            {
               this.tpdoMapping[i].Start();
            }

            this.DeviceStateLabel.Text = "RUNNING";

            DateTime now = DateTime.Now;
            this.workingProducerHeartbeatTimeLimit = now.AddMilliseconds(this.producerHeartbeatTime);
         }
      }

      protected override void Stop()
      {
         if (false != this.active)
         {
            base.Stop();

            for (int i = 0; i < this.rpdoMapping.Length; i++)
            {
               this.rpdoMapping[i].Stop();
            }

            for (int i = 0; i < this.tpdoMapping.Length; i++)
            {
               this.tpdoMapping[i].Stop();
            }

            this.DeviceStateLabel.Text = "STOPPED";
         }
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
         else if ((0x1016 == index) && (0x00 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (byte)1);
            valid = true;
         }
         else if ((0x1016 == index) && (0x01 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.ConsumerHeartbeatTime);
            valid = true;
         }
         else if (0x1017 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.ProducerHeartbeatTime);
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
         else if (0x2301 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.CameraSelect);
            valid = true;
         }
         else if ((0x2303 == index) && (0x00 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (byte)6);
            valid = true;
         }
         else if ((0x2303 == index) && (0x01 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.LedIntensity1);
            valid = true;
         }
         else if ((0x2303 == index) && (0x02 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.LedIntensity2);
            valid = true;
         }
         else if ((0x2303 == index) && (0x03 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.LedIntensity3);
            valid = true;
         }
         else if ((0x2303 == index) && (0x04 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.LedIntensity4);
            valid = true;
         }
         else if ((0x2303 == index) && (0x05 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.LedIntensity5);
            valid = true;
         }
         else if ((0x2303 == index) && (0x06 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.LedIntensity6);
            valid = true;
         }
         else if ((0x2311 == index) && (0x00 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (byte)2);
            valid = true;
         }
         else if ((0x2311 == index) && (0x01 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.McuTemperature);
            valid = true;
         }
         else if ((0x2311 == index) && (0x02 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.McuErrorTemperature);
            valid = true;
         }
         else if (0x5000 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.SubSystemStatus);
            valid = true;
         }
         else if (0x5001 == index)
         {
            UInt32 code = 0;

            if (subIndex < this.errorCodeList.Count)
            {
               code = this.errorCodeList[subIndex].Code;
            }

            dataLength = this.MoveDeviceData(buffer, code);
            valid = true;
         }
         else if (0x5002 == index)
         {
            UInt32 code = 0;

            if (subIndex < this.errorCodeList.Count)
            {
               code = this.errorCodeList[subIndex].AdditionalData;
            }

            dataLength = this.MoveDeviceData(buffer, code);
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

         if ((0x1016 == index) && (0x01 == subIndex) && (4 == length))
         {
            this.ConsumerHeartbeatTime = BitConverter.ToUInt32(buffer, offset);
            valid = true;
         }
         else if ((0x1017 == index) && (2 == length))
         {
            this.ProducerHeartbeatTime = BitConverter.ToUInt16(buffer, offset);
            valid = true;
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
         else if ((0x2301 == index) && (1 == length))
         {
            byte selectedValue = buffer[offset];

            if ((selectedValue >= 0) && (selectedValue <= 6))
            {
               this.CameraSelect = selectedValue;
               valid = true;
            }
         }
         else if ((0x2303 == index) && (0x01 == subIndex) && (2 == length))
         {
            this.LedIntensity1 = BitConverter.ToUInt16(buffer, offset);
            valid = true;
         }
         else if ((0x2303 == index) && (0x02 == subIndex) && (2 == length))
         {
            this.LedIntensity2 = BitConverter.ToUInt16(buffer, offset);
            valid = true;
         }
         else if ((0x2303 == index) && (0x03 == subIndex) && (2 == length))
         {
            this.LedIntensity3 = BitConverter.ToUInt16(buffer, offset);
            valid = true;
         }
         else if ((0x2303 == index) && (0x04 == subIndex) && (2 == length))
         {
            this.LedIntensity4 = BitConverter.ToUInt16(buffer, offset);
            valid = true;
         }
         else if ((0x2303 == index) && (0x05 == subIndex) && (2 == length))
         {
            this.LedIntensity5 = BitConverter.ToUInt16(buffer, offset);
            valid = true;
         }
         else if ((0x2303 == index) && (0x06 == subIndex) && (2 == length))
         {
            this.LedIntensity6 = BitConverter.ToUInt16(buffer, offset);
            valid = true;
         }
         else if ((0x2311 == index) && (0x02 == subIndex) && (1 == length))
         {
            this.McuErrorTemperature = buffer[offset];
            valid = true;
         }
         else if ((0x5003 == index) && (0x00 == subIndex) && (4 == length))
         {
            UInt32 code = BitConverter.ToUInt32(buffer, offset);
            this.RemoveError(code);
            valid = true;
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
                  this.Reset(false);
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

      #region User Events

      private void ResetButton_Click(object sender, EventArgs e)
      {
         this.Reset(false);
      }

      private void SetMcuTemperatureButton_Click(object sender, EventArgs e)
      {
         double temperature = 0;

         if (double.TryParse(this.McuTemperatureTextBox.Text, out temperature) != false)
         {
            this.McuTemperature = (byte)temperature;
         }
      }

      private void GeneralEmergencyButton_Click(object sender, EventArgs e)
      {
         if (false != this.active)
         {
            UInt16 emergencyCode = 0x1000;
            byte[] emergencyCodeBytes = BitConverter.GetBytes(emergencyCode);

            int emergencyCobId = this.GetCobId(COBTypes.EMGY, this.nodeId);
            byte[] emergencyMsg = new byte[8];
            emergencyMsg[0] = emergencyCodeBytes[0];
            emergencyMsg[1] = emergencyCodeBytes[1];
            emergencyMsg[2] = 0x01;
            emergencyMsg[3] = 0x00;
            emergencyMsg[4] = 0x00;
            emergencyMsg[5] = 0x00;
            emergencyMsg[6] = 0x00;
            emergencyMsg[7] = 0x00;

            this.Transmit(emergencyCobId, emergencyMsg);
         }
      }

      private void BootCrcErrorButton_Click(object sender, EventArgs e)
      {
         if (false != this.active)
         {
            UInt16 emergencyCode = 0x6100;
            UInt32 crc = 0;
            UInt32.TryParse(this.EmergencyCrcTextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out crc);
            byte[] emergencyCodeBytes = BitConverter.GetBytes(emergencyCode);
            byte[] crcBytes = BitConverter.GetBytes(crc);

            int emergencyCobId = this.GetCobId(COBTypes.EMGY, this.nodeId);
            byte[] emergencyMsg = new byte[8];
            emergencyMsg[0] = emergencyCodeBytes[0];
            emergencyMsg[1] = emergencyCodeBytes[1];
            emergencyMsg[2] = 0x01;
            emergencyMsg[3] = crcBytes[0];
            emergencyMsg[4] = crcBytes[1];
            emergencyMsg[5] = crcBytes[2];
            emergencyMsg[6] = crcBytes[3];
            emergencyMsg[7] = 0x00;

            this.Transmit(emergencyCobId, emergencyMsg);
         }
      }

      private void AppCrcErrorButton_Click(object sender, EventArgs e)
      {
         if (false != this.active)
         {
            UInt16 emergencyCode = 0x6200;
            UInt32 crc = 0;
            UInt32.TryParse(this.EmergencyCrcTextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out crc);
            byte[] emergencyCodeBytes = BitConverter.GetBytes(emergencyCode);
            byte[] crcBytes = BitConverter.GetBytes(crc);

            int emergencyCobId = this.GetCobId(COBTypes.EMGY, this.nodeId);
            byte[] emergencyMsg = new byte[8];
            emergencyMsg[0] = emergencyCodeBytes[0];
            emergencyMsg[1] = emergencyCodeBytes[1];
            emergencyMsg[2] = 0x01;
            emergencyMsg[3] = crcBytes[0];
            emergencyMsg[4] = crcBytes[1];
            emergencyMsg[5] = crcBytes[2];
            emergencyMsg[6] = crcBytes[3];
            emergencyMsg[7] = 0x00;

            this.Transmit(emergencyCobId, emergencyMsg);
         }
      }

      private void AppFlashEmptyErrorButton_Click(object sender, EventArgs e)
      {
         if (false != this.active)
         {
            UInt16 emergencyCode = 0x6201;
            byte[] emergencyCodeBytes = BitConverter.GetBytes(emergencyCode);

            int emergencyCobId = this.GetCobId(COBTypes.EMGY, this.nodeId);
            byte[] emergencyMsg = new byte[8];
            emergencyMsg[0] = emergencyCodeBytes[0];
            emergencyMsg[1] = emergencyCodeBytes[1];
            emergencyMsg[2] = 0x01;
            emergencyMsg[3] = 0x00;
            emergencyMsg[4] = 0x00;
            emergencyMsg[5] = 0x00;
            emergencyMsg[6] = 0x00;
            emergencyMsg[7] = 0x00;

            this.Transmit(emergencyCobId, emergencyMsg);
         }
      }

      private void LedShortedErrorButton_Click(object sender, EventArgs e)
      {
         byte ledId = 0;

         if ((false != this.active) &&
             (byte.TryParse(this.LedIdTextBox.Text, out ledId) != false) &&
             (ledId >= 1) &&
             (ledId <= 6))
         {
            int subSystem = 0;
            UInt16 emergencyCode = 0x2340;
            byte[] emergencyCodeBytes = BitConverter.GetBytes(emergencyCode);
            byte codeSpecificData = ledId;
            UInt32 additionalData = 0;

            int emergencyCobId = this.GetCobId(COBTypes.EMGY, this.nodeId);
            byte[] emergencyMsg = new byte[8];

            emergencyMsg[0] = emergencyCodeBytes[0];
            emergencyMsg[1] = emergencyCodeBytes[1];
            emergencyMsg[2] = this.GetErrorRegister(subSystem, false);
            emergencyMsg[3] = codeSpecificData;

            UInt32 errorCode = BitConverter.ToUInt32(emergencyMsg, 0);
            this.StoreError(subSystem, false, errorCode, additionalData);

            byte[] subSystemStatusBytes = BitConverter.GetBytes(this.SubSystemStatus);
            emergencyMsg[4] = subSystemStatusBytes[0];
            emergencyMsg[5] = subSystemStatusBytes[1];
            emergencyMsg[6] = subSystemStatusBytes[2];
            emergencyMsg[7] = subSystemStatusBytes[3];

            this.Transmit(emergencyCobId, emergencyMsg);
         }
      }

      private void LedOpenErrorButton_Click(object sender, EventArgs e)
      {
         byte ledId = 0;

         if ((false != this.active) &&
             (byte.TryParse(this.LedIdTextBox.Text, out ledId) != false) &&
             (ledId >= 1) &&
             (ledId <= 6))
         {
            int subSystem = 0;
            UInt16 emergencyCode = 0x3230;
            byte[] emergencyCodeBytes = BitConverter.GetBytes(emergencyCode);
            byte codeSpecificData = ledId;
            UInt32 additionalData = 0;

            int emergencyCobId = this.GetCobId(COBTypes.EMGY, this.nodeId);
            byte[] emergencyMsg = new byte[8];

            emergencyMsg[0] = emergencyCodeBytes[0];
            emergencyMsg[1] = emergencyCodeBytes[1];
            emergencyMsg[2] = this.GetErrorRegister(subSystem, false);
            emergencyMsg[3] = codeSpecificData;

            UInt32 errorCode = BitConverter.ToUInt32(emergencyMsg, 0);
            this.StoreError(subSystem, false, errorCode, additionalData);

            byte[] subSystemStatusBytes = BitConverter.GetBytes(this.SubSystemStatus);
            emergencyMsg[4] = subSystemStatusBytes[0];
            emergencyMsg[5] = subSystemStatusBytes[1];
            emergencyMsg[6] = subSystemStatusBytes[2];
            emergencyMsg[7] = subSystemStatusBytes[3];

            this.Transmit(emergencyCobId, emergencyMsg);
         }
      }

      private void LedIcExcessTemperatureErrorButton_Click(object sender, EventArgs e)
      {
         if (false != this.active)
         {
            int subSystem = 0;
            UInt16 emergencyCode = 0x4210;
            byte[] emergencyCodeBytes = BitConverter.GetBytes(emergencyCode);
            byte codeSpecificData = 0;
            UInt32 additionalData = 0;

            int emergencyCobId = this.GetCobId(COBTypes.EMGY, this.nodeId);
            byte[] emergencyMsg = new byte[8];

            emergencyMsg[0] = emergencyCodeBytes[0];
            emergencyMsg[1] = emergencyCodeBytes[1];
            emergencyMsg[2] = this.GetErrorRegister(subSystem, false);
            emergencyMsg[3] = codeSpecificData;

            UInt32 errorCode = BitConverter.ToUInt32(emergencyMsg, 0);
            this.StoreError(subSystem, false, errorCode, additionalData);

            byte[] subSystemStatusBytes = BitConverter.GetBytes(this.SubSystemStatus);
            emergencyMsg[4] = subSystemStatusBytes[0];
            emergencyMsg[5] = subSystemStatusBytes[1];
            emergencyMsg[6] = subSystemStatusBytes[2];
            emergencyMsg[7] = subSystemStatusBytes[3];

            this.Transmit(emergencyCobId, emergencyMsg);
         }
      }

      #endregion

      #region Form Events

      #endregion

      #region Constructor

      public UlcRoboticsWekoLaunchCard()
         : base()
      {
         this.InitializeComponent();

         this.deviceType = 0x000B0191;
         this.errorStatus = 0;
         this.deviceName = "SIM-LC";
         this.version = "v1.0";

         this.nvNodeId = 64;

         this.tpdoMapping = new TPDOMapping[4];
         this.rpdoMapping = new RPDOMapping[4];

         for (int i = 0; i < this.tpdoMapping.Length; i++)
         {
            this.tpdoMapping[i] = new TPDOMapping();
            this.tpdoMapping[i].OnPdoMappable = new TPDOMapping.PdoMappableHandler(this.TPdoMappableHandler);
            this.tpdoMapping[i].OnPdoSize = new TPDOMapping.PdoSizeHandler(this.TPdoSizeHandler);
            this.tpdoMapping[i].OnPdoData = new TPDOMapping.PdoDataHandler(this.TPdoDataHandler);
         }

         for (int i = 0; i < this.rpdoMapping.Length; i++)
         {
            this.rpdoMapping[i] = new RPDOMapping();
            this.rpdoMapping[i].OnPdoMappable = new RPDOMapping.PdoMappableHandler(this.RPdoMappableHandler);
            this.rpdoMapping[i].OnPdoSize = new RPDOMapping.PdoSizeHandler(this.RPdoSizeHandler);
            this.rpdoMapping[i].OnPdoData = new RPDOMapping.PdoDataHandler(this.RPdoDataHandler);
         }

         this.errorCodeList = new List<ErrorData>();
         this.ledIntensity = new UInt16[6];
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


         keyValue = appKey.GetValue(deviceTag + "nvNodeId");
         this.nvNodeId = (byte)((null != keyValue) ? (byte.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 64) : 64);
         this.NodeIdTextBox.Text = this.nvNodeId.ToString();
      }

      public override void SaveRegistry(RegistryKey appKey, string deviceTag)
      {
         appKey.SetValue(deviceTag + "Enabled", this.EnabledCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "Description", this.DescriptionTextBox.Text);
         appKey.SetValue(deviceTag + "BusId", this.GetBusId());

         appKey.SetValue(deviceTag + "nvNodeId", this.nvNodeId);
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

         else if ("nvNodeId" == name)
         {
            byte.TryParse(reader.Value, out this.nvNodeId);
            this.NodeIdTextBox.Text = this.nvNodeId.ToString();
         }
      }

      public override void Write(XmlWriter writer)
      {
         writer.WriteElementString("Enabled", (false != this.EnabledCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("Description", this.DescriptionTextBox.Text);
         writer.WriteElementString("BusId", this.GetBusId());

         writer.WriteElementString("nvNodeId", this.nvNodeId.ToString());
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

         this.active = this.EnabledCheckBox.Checked;
         this.Reset(true);

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
               if ((0 != this.ConsumerHeartbeatTime) &&
                   (nodeId == this.workingConsumerHeartbeatNode))
               {
                  this.workingConsumerHeartbeatActive = true;
                  this.workingConsumerHeartbeatTimeLimit = DateTime.Now.AddMilliseconds(this.workingConsumerHeartbeatTime);
               }
            }
         }
      }

      public override void UpdateDevice()
      {
         if (false != this.active)
         {
            DateTime now = DateTime.Now;

            if ((false != this.workingConsumerHeartbeatActive) &&
                (now > this.workingConsumerHeartbeatTimeLimit) &&
                (DeviceStates.preop != this.deviceState))
            {
               UInt16 emergencyCode = 0x8130;
               byte[] emergencyCodeBytes = BitConverter.GetBytes(emergencyCode);

               int emergencyCobId = this.GetCobId(COBTypes.EMGY, this.nodeId);
               byte[] emergencyMsg = new byte[8];
               emergencyMsg[0] = emergencyCodeBytes[0];
               emergencyMsg[1] = emergencyCodeBytes[1];
               emergencyMsg[2] = 0x01;
               emergencyMsg[3] = 0x00;
               emergencyMsg[4] = 0x00;
               emergencyMsg[5] = 0x00;
               emergencyMsg[6] = 0x00;
               emergencyMsg[7] = 0x00;

               this.Transmit(emergencyCobId, emergencyMsg);

               this.Reset(false);
            }

            if (0 != this.ProducerHeartbeatTime)
            {
               if (now > this.workingProducerHeartbeatTimeLimit)
               {
                  this.workingProducerHeartbeatTimeLimit = this.workingProducerHeartbeatTimeLimit.AddMilliseconds(this.ProducerHeartbeatTime);

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
               #region RPDO Activation Check

               for (int i = 0; i < this.rpdoMapping.Length; i++)
               {
                  this.rpdoMapping[i].Update();
               }

               #endregion

               #region TPDO Activation Check

               for (int i = 0; i < this.tpdoMapping.Length; i++)
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

               #endregion
            }
            else
            {
            }
         }
      }

      #endregion

   }
}
