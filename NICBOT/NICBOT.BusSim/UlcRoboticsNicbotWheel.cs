
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

   public partial class UlcRoboticsNicbotWheel : DeviceControl
   {
      #region Definitions

      private enum States
      {
         notReadyToSwitchOn,
         switchOnDisabled,
         readyToSwitchOn,
         switchedOn,
         operationalEnable,
         quickStopActive,
         faultReactionActive,
         notReadyToSwitchOnFaulted,
      }

      #endregion

      #region Fields

      private UInt32 deviceType;
      private UInt32 errorStatus;
      private UInt64 errorCode;
      private string deviceName;
      private string version;

      private byte nvBaudRateCode;
      private byte nvNodeId;
      private byte nvNodeOffset;      

      private bool active;
      private byte baudRateCode;
      private byte nodeId;
      private byte sdoNodeId;
      private byte sdoNodeOffset;

      private UInt32 sdoConsumerHeartbeat;
      private byte consumerHeartbeatNode;
      private UInt16 consumerHeartbeatTime;
      private bool consumerHeartbeatActive;
      private DateTime consumerHeartbeatTimeLimit; 
      
      private UInt16 producerHeartbeatTime;
      private DateTime heartbeatLimit;

      private TPDOMapping[] tpdoMapping;
      private RPDOMapping[] rpdoMapping;

      private UInt16 controlWord;
      private UInt16 statusWord;
      private byte mode;
      private byte displayMode;

      private States state;

      private Int32 velocityTarget;
      private double velocityActual;
      private UInt32 velocityAcceleration;
      private UInt32 velocityDeceleration;

      private Int16 torqueTarget;
      private double torqueActual;
      private UInt32 motorRatedCurrent;
      private UInt32 motorRatedTorque;
      private UInt16 maximumTorque;
      private UInt16 maximumCurrent;
      private UInt32 torqueSlope;

      private DateTime updateTime;
      private DateTime lastUpdateTime;

      #endregion

      #region Helper Functions

      private void SetNodeId()
      {
         byte nodeId = 0;

         if ((127 >= this.nvNodeId) && (0 != this.nvNodeId))
         {
            nodeId = this.nvNodeId;
         }
         else
         {
            byte inputs = 0;
            byte.TryParse(this.InputsTextBox.Text, out inputs);

            nodeId = (byte)(this.nvNodeOffset + (inputs & 0x3));
         }

         this.nodeId = nodeId;
         this.NodeIdTextBox.Text = nodeId.ToString();
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

      private void Fault(UInt64 errorCode)
      {
         this.ErrorCode = errorCode;
         this.state = States.faultReactionActive;
      }

      private bool SetControlWord(UInt16 value)
      {
         bool result = false;

         if (States.switchOnDisabled == this.state)
         {
            if (0x0000 == (UInt16)(value & 0x0082))
            {
               // no transition
               result = true;
            }
            else if (0x0006 == (UInt16)(value & 0x0087))
            {
               // transition 2
               this.state = States.readyToSwitchOn;
               result = true;
            }
         }
         else if (States.readyToSwitchOn == this.state)
         {
            UInt16 cmdValue = (UInt16)(value & 0x008F);

            if (0x0000 == (UInt16)(value & 0x0082))
            {
               // transition 7
               this.state = States.switchOnDisabled;
               result = true;
            }
            else if (0x0007 == (UInt16)(value & 0x008F))
            {
               // transition 3
               this.state = States.switchedOn;
               result = true;
            }
         }
         else if (States.switchedOn == this.state)
         {
            UInt16 cmdValue = (UInt16)(value & 0x008F);

            if (0x0006 == (UInt16)(value & 0x0087))
            {
               // transition 6
               this.state = States.readyToSwitchOn;
               result = true;
            }
            else if (0x000F == (UInt16)(value & 0x008F))
            {
               // transition 4
               this.state = States.operationalEnable;
               result = true;
            }
         }
         else if (States.operationalEnable == this.state)
         {
            if (0x0007 == (UInt16)(value & 0x008F))
            {
               // transition 5
               this.state = States.switchedOn;
               result = true;
            }
            else if (0x0006 == (UInt16)(value & 0x0087))
            {
               // transition 8
               this.state = States.readyToSwitchOn;
               result = true;
            }
            else if (0x0000 == (UInt16)(value & 0x0082))
            {
               // transition 9
               this.state = States.switchOnDisabled;
               result = true;
            }
            else if (0x0002 == (UInt16)(value & 0x0086))
            {
               // transition 11
               this.state = States.quickStopActive;
               result = true;
            }
         }
         else if (States.quickStopActive == this.state)
         {
            if (0x0000 == (UInt16)(value & 0x0082))
            {
               // transition 12
               this.state = States.switchOnDisabled;
               result = true;
            }
            else if (0x000F == (UInt16)(value & 0x008F))
            {
               // transition 16
               this.state = States.operationalEnable;
               result = true;
            }
         }
         else if (States.notReadyToSwitchOnFaulted == this.state)
         {
            if (0x0080 == (UInt16)(value & 0x008F))
            {
               // transition 15
               this.state = States.switchOnDisabled;
               result = true;
            }
         }

         if (false != result)
         {
            this.ControlWord = value;
         }

         return (result);
      }

      private UInt16 GetStatusWord()
      {
         UInt16 result = 0;

         if ((States.readyToSwitchOn == this.state) ||
             (States.switchedOn == this.state) ||
             (States.operationalEnable == this.state))
         {
            result |= 0x0001;
         }

         if ((States.switchedOn == this.state) ||
             (States.operationalEnable == this.state))
         {
            result |= 0x0002;
         }

         if (States.operationalEnable == this.state)
         {
            result |= 0x0004;
         }

         if ((States.faultReactionActive == this.state) ||
             (States.notReadyToSwitchOnFaulted == this.state))
         {
            result |= 0x0008;
         }

         if ((States.readyToSwitchOn == this.state) ||
             (States.switchedOn == this.state) ||
             (States.operationalEnable == this.state))
         {
            result |= 0x0010;
         }

         if ((States.switchOnDisabled != this.state) &&
             (States.quickStopActive != this.state))
         {
            result |= 0x0020;
         }

         if (States.switchOnDisabled == this.state)
         {
            result |= 0x0040;
         }

         result |= 0x200;

         return (result);
      }

      private void UpdateVelocity(double target, double elapsedSeconds)
      {
         if (this.VelocityActual < target)
         {
            double amount = this.VelocityAcceleration * elapsedSeconds;
            double adjusted = this.VelocityActual + amount;

            if (adjusted > target)
            {
               adjusted = target;
            }

            this.VelocityActual = adjusted;
         }
         else if (this.VelocityActual > target)
         {
            double amount = this.VelocityDeceleration * elapsedSeconds;
            double adjusted = this.VelocityActual - amount;

            if (adjusted < target)
            {
               adjusted = target;
            }

            this.VelocityActual = adjusted;
         }
      }

      private void UpdateTorque(double target, double elapsedSeconds)
      {
         if (this.TorqueActual < target)
         {
            double amount = this.TorqueSlope * elapsedSeconds;
            double adjusted = this.TorqueActual + amount;

            if (adjusted > target)
            {
               adjusted = target;
            }

            this.TorqueActual = adjusted;
         }
         else if (this.TorqueActual > target)
         {
            double amount = this.TorqueSlope * elapsedSeconds;
            double adjusted = this.TorqueActual - amount;

            if (adjusted < target)
            {
               adjusted = target;
            }

            this.TorqueActual = adjusted;
         }
      }

      #endregion

      #region Delegates

      private bool TPdoMappableHandler(UInt16 index, byte subIndex)
      {
         bool result = false;

         if ((0x6041 == index) ||
             (0x606c == index) ||
             (0x6077 == index))
         {
            result = true;
         }

         return (result);
      }

      private int TPdoSizeHandler(UInt16 index, byte subIndex)
      {
         int result = 0;

         if ((0x6041 == index) ||
             (0x6077 == index))
         {
            result = 2;
         }
         else if (0x606c == index)
         {
            result = 4;
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

         if ((0x6040 == index) ||
             (0x6071 == index) ||
             (0x60FF == index))
         {
            result = true;
         }

         return (result);      
      }

      private int RPdoSizeHandler(UInt16 index, byte subIndex)
      {
         int result = 0;

         if ((0x6040 == index) ||
             (0x6071 == index))
         {
            result = 2;
         }
         else if (0x60FF == index)
         {
            result = 4;
         }

         return (result);
      }

      private bool RPdoDataHandler(UInt16 index, byte subIndex, byte[] data, int offset, UInt32 length)
      {
         bool result = this.StoreDeviceData(index, subIndex, data, offset, length);
         return (result);
      }

      #endregion

      #region Properties

      private UInt16 ControlWord
      {
         set
         {
            this.controlWord = value;
            this.ControlWordTextBox.Text = value.ToString("X4");
         }

         get
         {
            return (this.controlWord);
         }
      }

      private UInt16 StatusWord
      {
         set
         {
            if (value != this.statusWord)
            {
               this.statusWord = value;
               this.StatusTextBox.Text = value.ToString("X4");
               this.CheckTpdoMappings(0x6041, 0);
            }
         }

         get
         {
            return (this.statusWord);
         }
      }

      private byte Mode
      {
         set
         {
            this.mode = value;
            this.ModeTextBox.Text = value.ToString("X2");
         }

         get
         {
            return (this.mode);
         }
      }

      private byte DisplayMode
      {
         set
         {
            this.displayMode = value;
            this.DisplayModeTextBox.Text = value.ToString("X2");
         }

         get
         {
            return (this.displayMode);
         }
      }

      private UInt64 ErrorCode
      {
         set
         {
            this.errorCode = value;
            this.ErrorCodeTextBox.Text = this.errorCode.ToString("X16");
         }

         get
         {
            return (this.errorCode);
         }
      }

      private UInt32 ErrorStatus
      {
         set
         {
            this.errorStatus = value;
            this.ErrorStatusTextBox.Text = this.errorStatus.ToString("X8");
         }

         get
         {
            return (this.errorStatus);
         }
      }

      private UInt32 ConsumerHeartbeatTime
      {
         set
         {
            this.sdoConsumerHeartbeat = value;
            this.consumerHeartbeatNode = (byte)((this.sdoConsumerHeartbeat >> 16) & 0x7F);
            this.consumerHeartbeatTime = (UInt16)(this.sdoConsumerHeartbeat & 0xFFFF);
            this.consumerHeartbeatActive = false;

            this.ConsumerHeartbeatTimeTextBox.Text = string.Format("{0:X2} : {1}", this.consumerHeartbeatNode, this.consumerHeartbeatTime);
         }

         get
         {
            return (this.sdoConsumerHeartbeat);
         }
      }

      private UInt16 ProducerHeartbeatTime
      {
         set
         {
            this.producerHeartbeatTime = value;
            this.ProducerHeartbeatTimeTextBox.Text = this.producerHeartbeatTime.ToString();
            this.heartbeatLimit = this.heartbeatLimit.AddMilliseconds(this.producerHeartbeatTime);
         }

         get
         {
            return (this.producerHeartbeatTime);
         }
      }

      private Int32 VelocityTarget
      {
         set
         {
            this.velocityTarget = value;
            this.VelocityTargetTextBox.Text = value.ToString();
         }

         get
         {
            return (this.velocityTarget);
         }
      }

      private double VelocityActual
      {
         set
         {
            this.velocityActual = value;
            this.VelocityActualTextBox.Text = value.ToString("N3");
         }

         get
         {
            return (this.velocityActual);
         }
      }
      
      private UInt32 VelocityAcceleration
      {
         set
         {
            this.velocityAcceleration = value;
            this.VelocityAccelerationTextBox.Text = value.ToString();
         }

         get
         {
            return (this.velocityAcceleration);
         }
      }

      private UInt32 VelocityDeceleration
      {
         set
         {
            this.velocityDeceleration = value;
            this.VelocityDecelerationTextBox.Text = value.ToString();
         }

         get
         {
            return (this.velocityDeceleration);
         }
      }

      private Int16 TorqueTarget
      {
         set
         {
            this.torqueTarget = value;
            this.TorqueTargetTextBox.Text = value.ToString();
         }

         get
         {
            return (this.torqueTarget);
         }
      }

      private double TorqueActual
      {
         set
         {
            this.torqueActual = value;
            this.TorqueActualTextBox.Text = value.ToString("N3");
         }

         get
         {
            return (this.torqueActual);
         }
      }

      private UInt32 TorqueSlope
      {
         set
         {
            this.torqueSlope = value;
            this.TorqueSlopeTextBox.Text = value.ToString();
         }

         get
         {
            return (this.torqueSlope);
         }
      }

      private UInt32 MotorRatedCurrent
      {
         set
         {
            this.motorRatedCurrent = value;
            this.MotorRatedCurrentTextBox.Text = value.ToString();
         }

         get
         {
            return (this.motorRatedCurrent);
         }
      }

      private UInt32 MotorRatedTorque
      {
         set
         {
            this.motorRatedTorque = value;
            this.MotorRatedTorqueTextBox.Text = value.ToString(); 
         }

         get
         {
            return (this.motorRatedTorque);
         }
      }

      private UInt16 MaximumTorque
      {
         set
         {
            this.maximumTorque = value;
            this.MaximumTorqueTextBox.Text = value.ToString();
         }

         get
         {
            return (this.maximumTorque);
         }
      }
      
      private UInt16 MaximumCurrent
      {
         set
         {
            this.maximumCurrent = value;
            this.MaximumCurrentTextBox.Text = value.ToString();
         }

         get
         {
            return (this.maximumCurrent);
         }
      }

      #endregion

      #region Device Specific Functions

      protected override void Reset()
      {
         this.DeviceStateLabel.Text = "PRE-OP";

         this.baudRateCode = this.nvBaudRateCode;
         this.SetNodeId();

         this.sdoNodeId = this.nvNodeId;
         this.sdoNodeOffset = this.nvNodeOffset;

         for (int i = 0; i < 4; i++)
         {
            this.tpdoMapping[i].Reset(i, this.nodeId);
            this.rpdoMapping[i].Reset(i, this.nodeId);
         }

         this.ErrorCode = 0;
         this.ConsumerHeartbeatTime = 0;
         this.ProducerHeartbeatTime = 0;

         if (false != this.active)
         {
            int cobId = this.GetCobId(COBTypes.ERROR, this.nodeId);
            byte[] bootUpMsg = new byte[1];
            bootUpMsg[0] = 0;

            this.Transmit(cobId, bootUpMsg);
         }

         this.state = States.notReadyToSwitchOn;
         this.ControlWord = 0;
         this.StatusWord = this.GetStatusWord();
         this.Mode = 0xFF;
         this.DisplayMode = 0xFF;

         this.VelocityTarget = 0;
         this.VelocityActual = 0;
         this.VelocityAcceleration = 50;
         this.VelocityDeceleration = 50;

         this.TorqueTarget = 0;
         this.TorqueActual = 0;
         this.MotorRatedCurrent = 1000;
         this.MotorRatedTorque = 1000;
         this.MaximumTorque = 3000;
         this.MaximumCurrent = 3000;
         this.TorqueSlope = 500;

         base.Reset();
      }

      protected override void Start()
      {
         base.Start();

         for (int i = 0; i < 4; i++)
         {
            this.tpdoMapping[i].Start();
            this.rpdoMapping[i].Start();
         }

         this.DeviceStateLabel.Text = "RUNNING";

         DateTime now = DateTime.Now;
         this.heartbeatLimit = now.AddMilliseconds(this.ProducerHeartbeatTime);
         this.updateTime = now.AddMilliseconds(100);
         this.lastUpdateTime = now;
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
            dataLength = this.MoveDeviceData(buffer, this.ErrorStatus);
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
            dataLength = this.MoveDeviceData(buffer, this.sdoNodeOffset);
            valid = true;
         }
         else if (0x2105 == index)
         {
            dataLength = this.MoveDeviceData(buffer, 0x65766173);
            valid = true;
         }
         else if (0x6040 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.ControlWord);
            valid = true;
         }
         else if (0x6041 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.StatusWord);
            valid = true;
         }
         else if (0x6060 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.Mode);
            valid = true;
         }
         else if (0x6061 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.DisplayMode);
            valid = true;
         }
         else if (0x606C == index)
         {
            Int32 actualVelocity = (Int32)(this.VelocityActual);
            dataLength = this.MoveDeviceData(buffer, actualVelocity);
            valid = true;
         }
         else if (0x6071 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.TorqueTarget);
            valid = true;
         }
         else if (0x6072 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.MaximumTorque);
            valid = true;
         }
         else if (0x6073 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.MaximumCurrent);
            valid = true;
         }
         else if (0x6075 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.MotorRatedCurrent);
            valid = true;
         }
         else if (0x6076 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.MotorRatedTorque);
            valid = true;
         }
         else if (0x6077 == index)
         {
            Int16 actualTorque = (Int16)(this.TorqueActual);
            dataLength = this.MoveDeviceData(buffer, actualTorque);
            valid = true;
         }
         else if (0x6083 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.VelocityAcceleration);
            valid = true;
         }
         else if (0x6084 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.VelocityDeceleration);
            valid = true;
         }
         else if (0x6087 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.TorqueSlope);
            valid = true;
         }
         else if (0x60FF == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.VelocityTarget);
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

         if ((0x1016 == index) && (1 == subIndex))
         {
            if (4 == length)
            {
               this.ConsumerHeartbeatTime = BitConverter.ToUInt32(buffer, offset);
               valid = true;
            }
         }
         else if (0x1017 == index)
         {
            if (2 == length)
            {
               this.ProducerHeartbeatTime = BitConverter.ToUInt16(buffer, offset);
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
               valid = true;
            }
         }
         else if (0x2102 == index)
         {
            if (1 == length)
            {
               byte value = buffer[offset];

               if ((1 <= value) && (124 >= value))
               {
                  this.sdoNodeOffset = value;
                  valid = true;
               }
            }
         }
         else if (0x2105 == index)
         {
            UInt32 value = BitConverter.ToUInt32(buffer, offset);

            if (0x65766173 == value)
            {
               this.nvBaudRateCode = this.baudRateCode;
               this.nvNodeId = this.sdoNodeId;
               this.nvNodeOffset = this.sdoNodeOffset;

               valid = true;
            }
         }
         else if (0x6040 == index)
         {
            if (2 == length)
            {
               UInt16 value = BitConverter.ToUInt16(buffer, offset);
               valid = this.SetControlWord(value);
            }
         }
         else if (0x6060 == index)
         {
            if (1 == length)
            {
               this.Mode = buffer[offset];
               valid = true;
            }
         }
         else if (0x6071 == index)
         {
            if (2 == length)
            {
               this.TorqueTarget = BitConverter.ToInt16(buffer, offset);
               valid = true;
            }
         }
         else if (0x6072 == index)
         {
            if (2 == length)
            {
               this.MaximumTorque = BitConverter.ToUInt16(buffer, offset);
               valid = true;
            }
         }
         else if (0x6073 == index)
         {
            if (2 == length)
            {
               this.MaximumCurrent = BitConverter.ToUInt16(buffer, offset);
               valid = true;
            }
         }
         else if (0x6075 == index)
         {
            if (4 == length)
            {
               this.MotorRatedCurrent = BitConverter.ToUInt32(buffer, offset);
               valid = true;
            }
         }
         else if (0x6076 == index)
         {
            if (4 == length)
            {
               this.MotorRatedTorque = BitConverter.ToUInt32(buffer, offset);
               valid = true;
            }
         }
         else if (0x6083 == index)
         {
            if (4 == length)
            {
               this.VelocityAcceleration = BitConverter.ToUInt32(buffer, offset);
               valid = true;
            }
         }
         else if (0x6084 == index)
         {
            if (4 == length)
            {
               this.VelocityDeceleration = BitConverter.ToUInt32(buffer, offset);
               valid = true;
            }
         }
         else if (0x6087 == index)
         {
            if (4 == length)
            {
               this.TorqueSlope = BitConverter.ToUInt32(buffer, offset);
               valid = true;
            }
         }
         else if (0x60FF == index)
         {
            if (4 == length)
            {
               this.VelocityTarget = BitConverter.ToInt32(buffer, offset);
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

      #region Constructor

      public UlcRoboticsNicbotWheel()
      {
         this.InitializeComponent();

         this.deviceType = 0x00004020;
         this.deviceName = "NICBOT Wheel";
         this.version = "v1.00";

         this.nvBaudRateCode = 3;
         this.nvNodeId = 0xFF;
         this.nvNodeOffset = 0x30;

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

         this.SetNodeId();
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

         keyValue = appKey.GetValue(deviceTag + "Inputs");
         this.InputsTextBox.Text = (null != keyValue) ? keyValue.ToString() : "00";

         keyValue = appKey.GetValue(deviceTag + "nvBaudRateCode");
         this.nvBaudRateCode = (byte)((null != keyValue) ? (byte.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 3) : 3);

         keyValue = appKey.GetValue(deviceTag + "nvNodeId");
         this.nvNodeId = (byte)((null != keyValue) ? (byte.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 0xFF) : 0xFF);

         keyValue = appKey.GetValue(deviceTag + "nvNodeOffset");
         this.nvNodeOffset = (byte)((null != keyValue) ? (byte.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 1) : 1);

         this.SetNodeId();
      }

      public override void SaveRegistry(RegistryKey appKey, string deviceTag)
      {
         appKey.SetValue(deviceTag + "Enabled", this.EnabledCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "Description", this.DescriptionTextBox.Text);
         appKey.SetValue(deviceTag + "BusId", this.GetBusId());
         appKey.SetValue(deviceTag + "Inputs", this.InputsTextBox.Text);
         appKey.SetValue(deviceTag + "nvBaudRateCode", this.nvBaudRateCode);
         appKey.SetValue(deviceTag + "nvNodeId", this.nvNodeId);
         appKey.SetValue(deviceTag + "nvNodeOffset", this.nvNodeOffset);
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
         else if ("Inputs" == name)
         {
            byte value = 0;
            byte.TryParse(reader.Value, out value);
            this.InputsTextBox.Text = value.ToString("X2");
         }
         else if ("nvBaudRateCode" == name)
         {
            byte.TryParse(reader.Value, out this.nvBaudRateCode);
         }
         else if ("nvNodeId" == name)
         {
            byte.TryParse(reader.Value, out this.nvNodeId);
            this.SetNodeId();
         }
         else if ("nvNodeOffset" == name)
         {
            byte.TryParse(reader.Value, out this.nvNodeOffset);
            this.SetNodeId();
         }
      }

      public override void Write(XmlWriter writer)
      {
         writer.WriteElementString("Enabled", (false != this.EnabledCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("Description", this.DescriptionTextBox.Text);
         writer.WriteElementString("BusId", this.GetBusId());
         writer.WriteElementString("Inputs", this.InputsTextBox.Text);
         writer.WriteElementString("nvBaudRateCode", this.nvBaudRateCode.ToString());
         writer.WriteElementString("nvNodeId", this.nvNodeId.ToString());
         writer.WriteElementString("nvNodeOffset", this.nvNodeOffset.ToString());
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
         this.Reset();

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
                  this.rpdoMapping[i].SyncReceived();
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
             (0 == this.ErrorCode))
         {
            this.Fault(0x1000);
         }

         if (0 != this.ProducerHeartbeatTime)
         {
            if (now > this.heartbeatLimit)
            {
               this.heartbeatLimit = this.heartbeatLimit.AddMilliseconds(this.ProducerHeartbeatTime);

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
         
         if (false != active)
         {
            if (this.DisplayMode != this.Mode)
            {
               this.DisplayMode = this.Mode;
            }

            if (States.notReadyToSwitchOn == this.state)
            {
               // transition 1
               this.state = States.switchOnDisabled;
            }
            else if (States.faultReactionActive == this.state)
            {
               // load fault error code
               this.ErrorStatus = (UInt32)(this.ErrorCode & 0xFFFFFFFF);

               // emit emergency code
               int cobId = this.GetCobId(COBTypes.EMGY, this.nodeId);
               byte[] emergencyMsg = BitConverter.GetBytes(this.ErrorCode);
               this.Transmit(cobId, emergencyMsg);

               // transition 14
               this.state = States.notReadyToSwitchOnFaulted;
            }

            this.StatusWord = this.GetStatusWord();
         }

         if (DeviceStates.running == this.deviceState)
         {
            for (int i = 0; i < 4; i++)
            {
               this.rpdoMapping[i].Update();
            }

            if (now > this.updateTime)
            {
               TimeSpan ts = now - this.lastUpdateTime;
               this.lastUpdateTime = now;
               double elapsedSeconds = ts.TotalSeconds;
               this.updateTime = now.AddMilliseconds(100);

               if (States.operationalEnable == this.state)
               {
                  if (3 == this.DisplayMode)
                  {
                     this.UpdateVelocity(this.VelocityTarget, elapsedSeconds);
                  }
                  else if (4 == this.DisplayMode)
                  {
                     this.UpdateTorque(this.TorqueTarget, elapsedSeconds);
                  }
               }
               else
               {
                  this.VelocityActual = 0;
                  this.TorqueActual = 0;
               }
            }

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
            this.VelocityActual = 0;
            this.TorqueActual = 0;
         }
      }

      #endregion
   }
}
