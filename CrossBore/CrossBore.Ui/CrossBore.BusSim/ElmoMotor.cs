
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

   public partial class ElmoMotor : DeviceControl
   {
      byte[] DeviceName = { 0x57, 0x68, 0x69, 0x73, 0x74, 0x6C, 0x65 };

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

      private int nodeId;
      private bool active;

      private UInt32 sdoConsumerHeartbeat;
      private byte consumerHeartbeatNode;
      private UInt16 consumerHeartbeatTime;
      private bool consumerHeartbeatActive;
      private DateTime consumerHeartbeatTimeLimit;
      private UInt16 sdoConsumerHeartbeatMode;

      private UInt16 producerHeartbeatTime;
      private DateTime heartbeatLimit;

      private TPDOMapping[] tpdoMapping;
      private RPDOMapping[] rpdoMapping;

      private int userMode;
      private int motorState;

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

      private UInt32 inputValues;

      private DateTime updateTime;
      private DateTime lastUpdateTime;

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

      private void AdjustInput(bool inputHigh, byte mask)
      {
         UInt32 value = this.inputValues;

         if (false != inputHigh)
         {
            value |= (UInt32)(mask << 16);
         }
         else
         {
            value &= ~((UInt32)mask << 16);
         }

         if (this.inputValues != value)
         {
            this.inputValues = value;
            this.CheckTpdoMappings(0x60FD, 0);
         }
      }

      #endregion

      #region Delegates

      private bool TPdoMappableHandler(UInt16 index, byte subIndex)
      {
         bool result = false;

         if ((0x6041 == index) ||
             (0x606C == index) ||
             (0x6077 == index) ||
             (0x60FD == index))
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
         else if ((0x606C == index) ||
                  (0x60FD == index))
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

      private int UserMode
      {
         set
         {
            this.userMode = value;
            this.UserModeLabel.Text = this.userMode.ToString();
         }

         get
         {
            return (this.userMode);
         }
      }

      private int MotorState
      {
         set
         {
            this.motorState = value;
            this.MotorStateLabel.Text = this.motorState.ToString();
         }

         get
         {
            return (this.motorState);
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

      private UInt16 ConsumerHeartbeatMode
      {
         set
         {
            this.sdoConsumerHeartbeatMode = value;
            this.ConsumerHeartbeatModeTextBox.Text = this.sdoConsumerHeartbeatMode.ToString();
         }

         get
         {
            return (this.sdoConsumerHeartbeatMode);
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

         for (int i = 0; i < 4; i++)
         {
            this.tpdoMapping[i].Reset(i, this.nodeId);
            this.rpdoMapping[i].Reset(i, this.nodeId);
         }

         this.UserMode = 5;
         this.MotorState = 0;

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
         this.VelocityAcceleration = 400;
         this.VelocityDeceleration = 400;

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
         else if (0x6007 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.ConsumerHeartbeatMode);
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
         else if (0x60FD == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.inputValues);
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
         else if (0x6007 == index)
         {
            if (2 == length)
            {
               this.ConsumerHeartbeatMode = BitConverter.ToUInt16(buffer, offset);
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

      private void TransmitBinaryInterpreterResponse(char firstCommandChar, char secondCommandChar)
      {
         int cobId = this.GetCobId(COBTypes.TPDO2, this.nodeId);

         byte[] rspFrame = new byte[8];

         rspFrame[0] = (byte)firstCommandChar;
         rspFrame[1] = (byte)secondCommandChar;
         rspFrame[2] = 0;
         rspFrame[3] = 0;
         rspFrame[4] = 0;
         rspFrame[5] = 0;
         rspFrame[6] = 0;
         rspFrame[7] = 0;

         this.Transmit(cobId, rspFrame);
      }

      private void TransmitBinaryInterpreterResponse(char firstCommandChar, char secondCommandChar, int index, bool isError, Int32 data)
      {
         int cobId = this.GetCobId(COBTypes.TPDO2, this.nodeId);

         byte[] rspFrame = new byte[8];

         rspFrame[0] = (byte)firstCommandChar;
         rspFrame[1] = (byte)secondCommandChar;
         rspFrame[2] = (byte)((index >> 0) & 0xFF);
         rspFrame[3] = (byte)(((index >> 8) & 0x3F) | ((isError) ? 0x40 : 0x00));
         rspFrame[4] = (byte)((data >> 0) & 0xFF);
         rspFrame[5] = (byte)((data >> 8) & 0xFF);
         rspFrame[6] = (byte)((data >> 16) & 0xFF);
         rspFrame[7] = (byte)((data >> 24) & 0xFF);

         this.Transmit(cobId, rspFrame);
      }

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

#if false
         int css = (int)((frame[0] >> 5) & 0x7);

         if (0 == css)
         {
            // download SDO segment
         }
         else if (1 == css)
         {
            // initiate SDO download

            int n = (int)((frame[0] >> 2) & 0x3);
            int e = (int)((frame[0] >> 1) & 0x1);
            int s = (int)((frame[0] >> 0) & 0x1);
            UInt16 index = BitConverter.ToUInt16(frame, 1);
            byte subIndex = frame[3];

            if ((0 == e) && (0 == s))
            {
               // reserved
            }
            else if ((0 == e) && (1 == s))
            {
               // data = number of bytes to transfer
            }
            else if ((1 == e) && (1 == s))
            {
               bool valid = false;
               UInt32 data = 0;

               for (int i = (7 - n); i >= 4; i--)
               {
                  data = data << 8;
                  data |= frame[i];
               }

               if (0x1017 == index)
               {
                  valid = true;
                  this.SetProducerHeartbeatValue((UInt16)data);
               }
               else if ((0x1800 <= index) && (0x1803 >= index))
               {
                  int offset = (index - 0x1800);
                  valid = this.tpdoMapping[offset].StoreParameterData(subIndex, (4 - n), data);
               }
               else if ((0x1A00 <= index) && (0x1A03 >= index))
               {
                  int offset = (index - 0x1A00);
                  valid = this.tpdoMapping[offset].StoreMapData(subIndex, (4 - n), data);
               }

               if (false != valid)
               {
                  int cobId = this.GetCobId(COBTypes.TSDO, this.nodeId);

                  byte[] rspFrame = new byte[8];
                  rspFrame[0] = 0x60;
                  rspFrame[1] = (byte)((index >> 0) & 0xFF);
                  rspFrame[2] = (byte)((index >> 8) & 0xFF);
                  rspFrame[3] = subIndex;
                  rspFrame[4] = 0;
                  rspFrame[5] = 0;
                  rspFrame[6] = 0;
                  rspFrame[7] = 0;

                  this.Transmit(cobId, rspFrame);
               }
            }
            else if ((1 == e) && (0 == s))
            {
               // data = unspecified number of bytes to transfer
            }
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
#endif
      }

      private void ProcessPdo1Message(byte[] frame)
      {
         this.rpdoMapping[0].StoreFrameData(frame);
      }

      private void ProcessPdo2Message(byte[] frame)
      {
#if false
         this.rpdoMapping[1].StoreFrameData(frame);
#endif

#if true
         // binary interpretor

         if (4 == frame.Length)
         {
            // execute 

            char firstCommandChar = (char)frame[0];
            char secondCommandChar = (char)frame[1];
            bool error = false;
            int responseCode = 0;

#if false
            if (('B' == firstCommandChar) && ('G' == secondCommandChar))
            {
               if (2 == this.userMode)
               {
                  this.UserValueLabel.Text = this.userValue.ToString("0");
               }
               else
               {
                  error = true;
                  responseCode = 12;
               }
            }
            else
#endif
            {
               error = true;
               responseCode = 2;
            }

            if (false == error)
            {
               this.TransmitBinaryInterpreterResponse(firstCommandChar, secondCommandChar);
            }
            else
            {
               this.TransmitBinaryInterpreterResponse(firstCommandChar, secondCommandChar, 0, true, responseCode);
            }
         }
         else if (8 == frame.Length)
         {
            // set or query

            char firstCommandChar = (char)frame[0];
            char secondCommandChar = (char)frame[1];
            int index = (int)(((frame[3] & 0x3F) << 8) | frame[2]);
            bool query = ((frame[3] & 0x40) != 0) ? true : false;
            bool dataIsFloat = ((frame[3] & 0x80) != 0) ? true : false;

            bool error = false;
            int responseCode = 0;

            firstCommandChar = char.ToUpper(firstCommandChar);
            secondCommandChar = char.ToUpper(secondCommandChar);

            if (('M' == firstCommandChar) && ('O' == secondCommandChar))
            {
               if (0 == index)
               {
                  int motorState = BitConverter.ToInt32(frame, 4);

                  if ((0 == motorState) || (1 == motorState))
                  {
                     this.MotorState = motorState;
                     responseCode = motorState;
                  }
                  else
                  {
                     error = true;
                     responseCode = 3;
                  }
               }
               else
               {
                  error = true;
                  responseCode = 3;
               }
            }
            else if (('U' == firstCommandChar) && ('M' == secondCommandChar))
            {
               if (0 == index)
               {
                  if (0 != this.motorState)
                  {
                     error = true;
                     responseCode = 57;
                  }
                  else
                  {
                     int userMode = BitConverter.ToInt32(frame, 4);

                     if ((userMode >= 1) && (userMode <= 5))
                     {
                        responseCode = userMode;
                        this.UserMode = userMode;
                     }
                     else
                     {
                        error = true;
                        responseCode = 21;
                     }
                  }
               }
               else
               {
                  error = true;
                  responseCode = 3;
               }
            }
#if false
            else if (('J' == firstCommandChar) && ('V' == secondCommandChar))
            {
               if (0 == index)
               {
                  if (2 == this.userMode)
                  {
                     int velocity = BitConverter.ToInt32(frame, 4);
                     responseCode = velocity;
                     this.userValue = velocity;
                  }
                  else
                  {
                     error = true;
                     responseCode = 12;
                  }
               }
               else
               {
                  error = true;
                  responseCode = 3;
               }
            }
            else if (('B' == firstCommandChar) && ('T' == secondCommandChar))
            {
               if (0 == index)
               {
                  Int32 startTime = BitConverter.ToInt32(frame, 4);
                  responseCode = startTime;

                  if (2 == this.userMode)
                  {
                     this.UserValueLabel.Text = this.userValue.ToString("0");
                  }
                  else
                  {
                     error = true;
                     responseCode = 12;
                  }
               }
               else
               {
                  error = true;
                  responseCode = 3;
               }
            }
#endif
            else if (('T' == firstCommandChar) && ('C' == secondCommandChar))
            {
               if (0 == index)
               {
                  if (3 == this.userMode)
                  {
                     float current = BitConverter.ToSingle(frame, 4);
                     responseCode = BitConverter.ToInt32(frame, 4);
                     Int16 torqueCounts = (Int16)(current * 1000000 / this.MotorRatedCurrent);
                     this.TorqueTarget = torqueCounts;
                  }
                  else
                  {
                     error = true;
                     responseCode = 12;
                  }
               }
               else
               {
                  error = true;
                  responseCode = 3;
               }
            }
            else
            {
               error = true;
               responseCode = 2;
            }

            this.TransmitBinaryInterpreterResponse(firstCommandChar, secondCommandChar, index, error, responseCode);
         }
#endif
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

      #region User Event Process

      private void DIn0LevelCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.AdjustInput(this.DIn0LevelCheckBox.Checked, 0x01);
      }

      private void DIn1LevelCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.AdjustInput(this.DIn1LevelCheckBox.Checked, 0x02);
      }

      private void DIn2LevelCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.AdjustInput(this.DIn2LevelCheckBox.Checked, 0x04);
      }

      private void DIn3LevelCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.AdjustInput(this.DIn3LevelCheckBox.Checked, 0x08);
      }

      private void DIn4LevelCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.AdjustInput(this.DIn4LevelCheckBox.Checked, 0x10);
      }

      private void DIn5LevelCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.AdjustInput(this.DIn5LevelCheckBox.Checked, 0x20);
      }

      private void UnderVoltageButton_Click(object sender, EventArgs e)
      {
         this.Fault(0x00013120);
      }

      #endregion

      #region Constructor

      public ElmoMotor()
         : base()
      {
         this.InitializeComponent();

         this.deviceType = 0x00020192;
         this.deviceName = "Whistle";
         this.version = "v1.23";

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

         this.active = false;
         this.PowerDown();
      }

      #endregion

      #region Access Functions

      public override void LoadRegistry(RegistryKey appKey, string deviceTag)
      {
         object keyValue;

         keyValue = appKey.GetValue(deviceTag + "Enabled");
         this.EnabledCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "Description");
         this.DescriptionTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "NodeId");
         this.NodeIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "BusId");
         string busId = (null != keyValue) ? keyValue.ToString() : "";
         this.SetBusId(busId);

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
      }

      public override void SaveRegistry(RegistryKey appKey, string deviceTag)
      {
         appKey.SetValue(deviceTag + "Enabled", this.EnabledCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "Description", this.DescriptionTextBox.Text);
         appKey.SetValue(deviceTag + "NodeId", this.NodeIdTextBox.Text);
         appKey.SetValue(deviceTag + "BusId", this.GetBusId());

         appKey.SetValue(deviceTag + "DIn0Level", this.DIn0LevelCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "DIn1Level", this.DIn1LevelCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "DIn2Level", this.DIn2LevelCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "DIn3Level", this.DIn3LevelCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "DIn4Level", this.DIn4LevelCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "DIn5Level", this.DIn5LevelCheckBox.Checked ? "1" : "0");
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
         else if ("NodeId" == name)
         {
            this.NodeIdTextBox.Text = reader.Value;
         }
         else if ("BusId" == name)
         {
            this.SetBusId(reader.Value);
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
      }

      public override void Write(XmlWriter writer)
      {
         writer.WriteElementString("Enabled", (false != this.EnabledCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("Description", this.DescriptionTextBox.Text);
         writer.WriteElementString("NodeId", this.NodeIdTextBox.Text);
         writer.WriteElementString("BusId", this.GetBusId());

         writer.WriteElementString("DIn0Level", (false != this.DIn0LevelCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("DIn1Level", (false != this.DIn1LevelCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("DIn2Level", (false != this.DIn2LevelCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("DIn3Level", (false != this.DIn3LevelCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("DIn4Level", (false != this.DIn4LevelCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("DIn5Level", (false != this.DIn5LevelCheckBox.Checked) ? "1" : "0");
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
         
         int nodeId = 0;

         if (int.TryParse(this.NodeIdTextBox.Text, out nodeId) != false)
         {
            this.nodeId = nodeId;
            this.active = this.EnabledCheckBox.Checked;

            this.Reset();

            if (false == this.active)
            {
               this.DeviceStateLabel.Text = "DISABLED";
            }
         }
         else
         {
            this.DeviceStateLabel.Text = "ERROR";
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

               if (5 == this.UserMode)
               {
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
               else if (3 == this.UserMode)
               {
                  this.UpdateTorque(this.TorqueTarget, elapsedSeconds);
                  this.VelocityActual = 0;
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
