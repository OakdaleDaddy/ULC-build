﻿namespace E4.BusSim
{
   using System;
   using System.Collections.Generic;
   using System.ComponentModel;
   using System.Drawing;
   using System.Data;
   using System.Linq;
   using System.Text;
   using System.Windows.Forms;

   public partial class MainMotor : UserControl
   {
      #region Definitions

      private enum DeviceStates
      {
         preop,
         running,
         stopped,
      }
      
      private enum MotorStates
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

      private enum HaltStates
      {
         notHalted,
         halting,
         halted,
      }

      private enum HomingRunStates
      {
         idle,
         toSwitch,
         stopFromToSwitch,
         fromSwitch,
         stopFromFromSwitch,
         moveOffset,
         attained,           
      }

      public delegate void TpdoCheckHandler(UInt16 index, byte subIndex);

      #endregion

      #region Fields

      #region Operational

      private DeviceStates deviceState;
      private MotorStates motorState;

      private DateTime updateTime;
      private DateTime lastUpdateTime;

      private HaltStates haltState;
      private HomingRunStates homingRunState;
      private double activeHomingAcceleration;
      private double activeHomingTargetVelocity;
      private double activeHomingTargetPosition;
      private bool homeDefined;
      private bool homingSwitchActive;

      #endregion

      #region Labels

      private Label MotorAbortConnectionOptionCodeLabel;
      private Label MotorErrorCodeLabel;
      private Label DigitalInputsLabel;
      private Label DigitalOutputsLabel;
      private Label DigitalOutputsMaskLabel;
      private Label MotorTypeLabel;
      private Label MotorTemperatureLabel;
      private Label MotorErrorTemperatureLabel;
      private Label MotorSupportedDriveModesLabel;
      private Label SingleDeviceTypeLabel;

      private Label ControlWordLabel;
      private Label StatusWordLabel;
      private Label SetModeLabel;
      private Label GetModeLabel;

      private Label HomeOffsetLabel;
      private Label HomingMethodLabel;
      private Label HomingSpeedHighestLabel;
      private Label HomingSwitchSpeedLabel;
      private Label HomingZeroSpeedLabel;
      private Label HomingAccelerationLabel;

      private Label PositionActualValueLabel;
      private Label PositionWindowLabel;
      private Label PositionWindowTimeLabel;
      private Label PositionControlParameterHighestLabel;
      private Label PositionProportionalGainCoefficientKpLabel;
      private Label PositionIntegralGainCoefficienKiLabel;
      private Label PositionDerivativeGainCoefficientKdLabel;

      private Label TargetPositionLabel;
      private Label ProfileVelocityLabel;
      private Label ProfileAccelerationLabel;
      private Label ProfileDecelerationLabel;

      private Label TargetTorqueLabel;
      private Label CurrentActualValueLabel;

      private Label VelocityActualValueLabel;
      private Label VelocityWindowLabel;
      private Label VelocityWindowTimeLabel;
      private Label VelocityThresholdLabel;
      private Label VelocityThresholdTimeLabel;
      private Label VelocityControlParameterHighestLabel;
      private Label VelocityProportionalGainCoefficientKpLabel;
      private Label VelocityIntegralGainCoefficienKiLabel;
      private Label VelocityDerivativeGainCoefficientKdLabel;
      private Label TargetVelocityLabel;

      #endregion

      #region Process Image 

      private Int16 motorAbortConnectionOption;
      private UInt16 motorErrorCode;
      private UInt32 digitalInputs;
      private UInt32 digitalOutputs;
      private UInt32 digitalOutputsMask;
      private UInt16 motorType;
      private byte motorTemperature;
      private byte motorErrorTemperature;
      private UInt32 motorSupportedDriveModes;
      private UInt32 singleDeviceType;
      
      private UInt16 controlWord;// rw pdo 6040
      private UInt16 statusWord; // ro pdo 6041
      private byte setMode; // rw pdo 6060
      private byte getMode; // rw pdo 6061

      private Int32 homeOffset; // rw pdo 607c
      private byte homingMethod; // rw pdo 6098
      private byte homingSpeedHighest; // ro 6099-0
      private UInt32 homingSwitchSpeed; // rw pdo 6099-1
      private UInt32 homingZeroSpeed; // rw pdo 6099-2
      private UInt32 homingAcceleration; // rw pdo 609a
      
      private Int32 positionActualValue; // ro pdo 6064
      private UInt32 positionWindow; // rw pdo 6067
      private UInt16 positionWindowTime; // rw pdo 6068
      private byte positionControlParameterHighest; // ro 60FB-0
      private Int32 positionProportionalGainCoefficientKp; // rw pdo 60FB-1
      private Int32 positionIntegralGainCoefficienKi; // rw pdo 60FB-2
      private Int32 positionDerivativeGainCoefficientKd; // rw pdo 60FB-3

      private Int32 targetPosition; // rw pdo 607A
      private UInt32 profileVelocity; // rw pdo 6081
      private UInt32 profileAcceleration; // rw pdo 6083
      private UInt32 profileDeceleration; // rw pdo 6084
      
      private Int16 targetTorque; // pdo rww 6071
      private Int16 currentActualValue; // pdo ro 6078

      private Int32 velocityActualValue; // ro pdo 606c
      private UInt16 velocityWindow; // rw pdo 606D
      private UInt16 velocityWindowTime; // rw pdo 606E
      private UInt16 velocityThreshold; // rw pdo 606F
      private UInt16 velocityThresholdTime; // rw pdo 6070
      private byte velocityControlParameterHighest; // ro 60F9-0
      private Int32 velocityProportionalGainCoefficientKp; // rw pdo 60F9-1
      private Int32 velocityIntegralGainCoefficienKi; // rw pdo 60F9-2
      private Int32 velocityDerivativeGainCoefficientKd; // rw pdo 60F9-3
      private Int32 targetVelocity; // rw pdo 60ff

      #endregion

      #endregion

      #region Helper Functions

      private void CreateLabel(int location, string name, out Label label, ref int top)
      {
         if (0 != location)
         {
            label = new Label();

            label.AutoSize = true;
            label.Location = new System.Drawing.Point(3, top);
            label.Name = name;
            label.Size = new System.Drawing.Size(219, 13);
            label.Text = "";
            label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.ValuePanel.Controls.Add(label);

            top += 16;
         }
         else
         {
            label = null;
         }
      }

      protected UInt32 MoveDeviceData(byte[] buffer, byte value, int offset = 0)
      {
         buffer[0 + offset] = value;
         return (1);
      }

      protected UInt32 MoveDeviceData(byte[] buffer, UInt16 value, int offset = 0)
      {
         byte[] source = BitConverter.GetBytes(value);

         for (int i = 0; i < source.Length; i++)
         {
            buffer[i + offset] = source[i];
         }

         return ((UInt32)source.Length);
      }

      protected UInt32 MoveDeviceData(byte[] buffer, Int16 value, int offset = 0)
      {
         byte[] source = BitConverter.GetBytes(value);

         for (int i = 0; i < source.Length; i++)
         {
            buffer[i + offset] = source[i];
         }

         return ((UInt32)source.Length);
      }

      protected UInt32 MoveDeviceData(byte[] buffer, UInt32 value, int offset = 0)
      {
         byte[] source = BitConverter.GetBytes(value);

         for (int i = 0; i < source.Length; i++)
         {
            buffer[i + offset] = source[i];
         }

         return ((UInt32)source.Length);
      }

      protected UInt32 MoveDeviceData(byte[] buffer, Int32 value, int offset = 0)
      {
         byte[] source = BitConverter.GetBytes(value);

         for (int i = 0; i < source.Length; i++)
         {
            buffer[i + offset] = source[i];
         }

         return ((UInt32)source.Length);
      }

      protected UInt32 MoveDeviceData(byte[] buffer, float value, int offset = 0)
      {
         byte[] source = BitConverter.GetBytes(value);

         for (int i = 0; i < source.Length; i++)
         {
            buffer[i + offset] = source[i];
         }

         return ((UInt32)source.Length);
      }

      protected UInt32 MoveDeviceData(byte[] buffer, string value, int offset = 0)
      {
         byte[] source = Encoding.UTF8.GetBytes(value);

         for (int i = 0; i < source.Length; i++)
         {
            buffer[i + offset] = source[i];
         }

         return ((UInt32)source.Length);
      }

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

      private void CheckTpdo(UInt16 index, byte subIndex)
      {
         if (null != this.OnTpdoCheck)
         {
            this.OnTpdoCheck(index, subIndex);
         }
      }

      private void SetValue(int location, Label label, string description, ref byte existingValue, byte assignedValue, int precision)
      {
         UInt16 index = (UInt16)((location >> 8) & 0xFFFF);
         byte subIndex = (byte)(location & 0xFF);

         this.SetLabelValue(index, subIndex, label, description, assignedValue, precision);

         if (existingValue != assignedValue)
         {
            existingValue = assignedValue;
            this.CheckTpdo(index, subIndex);
         }
      }

      private void SetValue(int location, Label label, string description, ref Int16 existingValue, Int16 assignedValue, int precision)
      {
         UInt16 index = (UInt16)((location >> 8) & 0xFFFF);
         byte subIndex = (byte)(location & 0xFF);

         this.SetLabelValue(index, subIndex, label, description, assignedValue, precision);

         if (existingValue != assignedValue)
         {
            existingValue = assignedValue;
            this.CheckTpdo(index, subIndex);
         }
      }

      private void SetValue(int location, Label label, string description, ref UInt16 existingValue, UInt16 assignedValue, int precision)
      {
         UInt16 index = (UInt16)((location >> 8) & 0xFFFF);
         byte subIndex = (byte)(location & 0xFF);

         this.SetLabelValue(index, subIndex, label, description, assignedValue, precision);

         if (existingValue != assignedValue)
         {
            existingValue = assignedValue;
            this.CheckTpdo(index, subIndex);
         }
      }

      private void SetValue(int location, Label label, string description, ref Int32 existingValue, Int32 assignedValue, int precision)
      {
         UInt16 index = (UInt16)((location >> 8) & 0xFFFF);
         byte subIndex = (byte)(location & 0xFF);

         this.SetLabelValue(index, subIndex, label, description, assignedValue, precision);

         if (existingValue != assignedValue)
         {
            existingValue = assignedValue;
            this.CheckTpdo(index, subIndex);
         }
      }

      private void SetValue(int location, Label label, string description, ref UInt32 existingValue, UInt32 assignedValue, int precision)
      {
         UInt16 index = (UInt16)((location >> 8) & 0xFFFF);
         byte subIndex = (byte)(location & 0xFF);

         this.SetLabelValue(index, subIndex, label, description, (int)assignedValue, precision);

         if (existingValue != assignedValue)
         {
            existingValue = assignedValue;
            this.CheckTpdo(index, subIndex);
         }
      }

      private bool SetControlWord(UInt16 value)
      {
         bool result = false;

         if (MotorStates.switchOnDisabled == this.motorState)
         {
            if (0x0000 == (UInt16)(value & 0x0082))
            {
               // no transition
               result = true;
            }
            else if (0x0006 == (UInt16)(value & 0x0087))
            {
               // transition 2
               this.motorState = MotorStates.readyToSwitchOn;
               result = true;
            }
         }
         else if (MotorStates.readyToSwitchOn == this.motorState)
         {
            UInt16 cmdValue = (UInt16)(value & 0x008F);

            if (0x0000 == (UInt16)(value & 0x0082))
            {
               // transition 7
               this.motorState = MotorStates.switchOnDisabled;
               result = true;
            }
            else if (0x0007 == (UInt16)(value & 0x008F))
            {
               // transition 3
               this.motorState = MotorStates.switchedOn;
               result = true;
            }
         }
         else if (MotorStates.switchedOn == this.motorState)
         {
            UInt16 cmdValue = (UInt16)(value & 0x008F);

            if (0x0006 == (UInt16)(value & 0x0087))
            {
               // transition 6
               this.motorState = MotorStates.readyToSwitchOn;
               result = true;
            }
            else if (0x000F == (UInt16)(value & 0x008F))
            {
               // transition 4
               this.motorState = MotorStates.operationalEnable;
               result = true;
            }
         }
         else if (MotorStates.operationalEnable == this.motorState)
         {
            if (0x0007 == (UInt16)(value & 0x008F))
            {
               // transition 5
               this.motorState = MotorStates.switchedOn;
               result = true;
            }
            else if (0x0006 == (UInt16)(value & 0x0087))
            {
               // transition 8
               this.motorState = MotorStates.readyToSwitchOn;
               result = true;
            }
            else if (0x0000 == (UInt16)(value & 0x0082))
            {
               // transition 9
               this.motorState = MotorStates.switchOnDisabled;
               result = true;
            }
            else if (0x0002 == (UInt16)(value & 0x0086))
            {
               // transition 11
               this.motorState = MotorStates.quickStopActive;
               result = true;
            }
            else if (0x000F == (UInt16)(value & 0x008F))
            {
               // no transition
               result = true;
            }
         }
         else if (MotorStates.quickStopActive == this.motorState)
         {
            if (0x0000 == (UInt16)(value & 0x0082))
            {
               // transition 12
               this.motorState = MotorStates.switchOnDisabled;
               result = true;
            }
            else if (0x000F == (UInt16)(value & 0x008F))
            {
               // transition 16
               this.motorState = MotorStates.operationalEnable;
               result = true;
            }
         }
         else if (MotorStates.notReadyToSwitchOnFaulted == this.motorState)
         {
            if (0x0080 == (UInt16)(value & 0x008F))
            {
               // transition 15
               this.motorState = MotorStates.switchOnDisabled;
               result = true;
            }
         }

         if (false != result)
         {
            this.ControlWord = value;

            if (MotorStates.readyToSwitchOn == this.motorState)
            {
               //this.positionFault = false;
               //this.ErrorCode = 0;
               this.haltState = HaltStates.notHalted;
               this.homingRunState = HomingRunStates.idle;
            }
         }

         return (result);
      }

      private UInt16 GetStatusWord()
      {
         UInt16 result = 0;

         if ((MotorStates.readyToSwitchOn == this.motorState) ||
             (MotorStates.switchedOn == this.motorState) ||
             (MotorStates.operationalEnable == this.motorState))
         {
            result |= 0x0001;
         }

         if ((MotorStates.switchedOn == this.motorState) ||
             (MotorStates.operationalEnable == this.motorState))
         {
            result |= 0x0002;
         }

         if (MotorStates.operationalEnable == this.motorState)
         {
            result |= 0x0004;
         }

         if ((MotorStates.faultReactionActive == this.motorState) ||
             (MotorStates.notReadyToSwitchOnFaulted == this.motorState))
         {
            result |= 0x0008;
         }

         if ((MotorStates.readyToSwitchOn == this.motorState) ||
             (MotorStates.switchedOn == this.motorState) ||
             (MotorStates.operationalEnable == this.motorState))
         {
            result |= 0x0010;
         }

         if ((MotorStates.switchOnDisabled != this.motorState) &&
             (MotorStates.quickStopActive != this.motorState))
         {
            result |= 0x0020;
         }

         if (MotorStates.switchOnDisabled == this.motorState)
         {
            result |= 0x0040;
         }

         if (false != this.homeDefined)
         {
            result |= 0x0100;
         }
         
         result |= 0x200;

         if (MotorStates.operationalEnable == this.motorState)
         {
            if (1 == this.GetMode)
            {
               if ((this.ControlWord & 0x0100) != 0)
               {
                  if (0 == this.VelocityActualValue)
                  {
                     result |= 0x400;
                  }
               }
               else if (this.TargetPosition == this.PositionActualValue)
               {
                  result |= 0x400;
               }
            }
            else if (3 == this.GetMode)
            {
               if (this.TargetVelocity == this.VelocityActualValue)
               {
                  result |= 0x400;
               }
            }
            else if ((6 == this.GetMode))
            {
               if ((this.ControlWord & 0x0100) != 0)
               {
                  if (0 == this.VelocityActualValue)
                  {
                     result |= 0x400;
                  }
               }
               else if (((this.ControlWord & 0x0010) != 0) &&
                        (HomingRunStates.attained == this.homingRunState))
               {
                  result |= 0x1400;
               }
            }
         }

         return (result);
      }

      private void UpdateVelocity(double target, double acceleration, double deceleration, double elapsedSeconds)
      {
         if (this.VelocityActualValue < target)
         {
            double amount = acceleration * elapsedSeconds;
            double adjusted = this.VelocityActualValue + amount;

            if (adjusted > target)
            {
               adjusted = target;
            }

            this.VelocityActualValue = (Int32)adjusted;
         }
         else if (this.VelocityActualValue > target)
         {
            double amount = deceleration * elapsedSeconds;
            double adjusted = this.VelocityActualValue - amount;

            if (adjusted < target)
            {
               adjusted = target;
            }

            this.VelocityActualValue = (Int32)adjusted;
         }
      }

      private void UpdatePosition(double elapsedSeconds)
      {
         double amount = this.VelocityActualValue * elapsedSeconds;
         double adjusted = this.PositionActualValue + amount;
         this.PositionActualValue = (Int32)adjusted;
      }

      private void UpdatePositionMode(double targetPosition, double velocityTarget, double acceleration, double deceleration, double elapsedSeconds)
      {
         bool halted = ((this.ControlWord & 0x0100) != 0) ? true : false;

         if (HaltStates.notHalted == this.haltState)
         {
            if (false != halted)
            {
               if (0 == this.VelocityActualValue)
               {
                  this.haltState = HaltStates.halted;
               }
               else
               {
                  this.activeHomingAcceleration = this.HomingAcceleration;
                  this.activeHomingTargetVelocity = 0;

                  this.haltState = HaltStates.halting;
               }
            }
            else
            {
               if (this.PositionActualValue != targetPosition)
               {
                  double remaining = targetPosition - this.PositionActualValue;

                  if (this.PositionActualValue > targetPosition)
                  {
                     velocityTarget = -velocityTarget;
                  }

                  if (0 != this.VelocityActualValue)
                  {
                     double timeRemaining = Math.Abs(remaining) / Math.Abs(this.VelocityActualValue);
                     double timeForSlow = (Math.Abs(this.VelocityActualValue) - 10) / deceleration;

                     if (timeRemaining < timeForSlow)
                     {
                        velocityTarget = (remaining > 0) ? 10 : -10;
                     }
                  }

                  this.UpdateVelocity(velocityTarget, acceleration, deceleration, elapsedSeconds);
                  this.UpdatePosition(elapsedSeconds);
               }
               else
               {
                  this.VelocityActualValue = 0;
               }
            }
         }
         else if (HaltStates.halting == this.haltState)
         {
            this.UpdateVelocity(0, acceleration, deceleration, elapsedSeconds);
            this.UpdatePosition(elapsedSeconds);

            if (0 == this.VelocityActualValue)
            {
               this.haltState = HaltStates.halted;
            }
         }
         else if (HaltStates.halted == this.haltState)
         {
            if (false == halted)
            {
               this.haltState = HaltStates.notHalted;
            }
         }
      }

      private void UpdateVelocityMode(double velocityTarget, double acceleration, double deceleration, double elapsedSeconds)
      {
         this.UpdateVelocity(velocityTarget, acceleration, deceleration, elapsedSeconds);
         this.UpdatePosition(elapsedSeconds);
      }

      private void UpdateHomingMode(double elapsedSeconds)
      {
         bool halted = ((this.ControlWord & 0x0100) != 0) ? true : false;
         bool active = ((this.ControlWord & 0x0010) != 0) ? true : false;

         this.UpdatePositionMode(this.activeHomingTargetPosition, this.activeHomingTargetVelocity, this.activeHomingAcceleration, this.activeHomingAcceleration, elapsedSeconds);

         if (HaltStates.notHalted == this.haltState)
         {
            if (false != halted)
            {
               if (0 == this.VelocityActualValue)
               {
                  this.haltState = HaltStates.halted;
               }
               else
               {
                  this.activeHomingAcceleration = this.HomingAcceleration;
                  this.activeHomingTargetVelocity = 0;

                  this.haltState = HaltStates.halting;
               }
            }
            else if (HomingRunStates.idle == this.homingRunState)
            {
               if ((false != active))
               {
                  this.activeHomingAcceleration = this.HomingAcceleration;
                  this.activeHomingTargetVelocity = this.HomingSwitchSpeed;
                  this.activeHomingTargetPosition = -2000000;

                  this.homingRunState = HomingRunStates.toSwitch;
               }
            }
            else
            {
               if (false == active)
               {
                  this.homingRunState = HomingRunStates.idle;
               }
               else if (HomingRunStates.toSwitch == this.homingRunState)
               {
                  if (false != this.homingSwitchActive)
                  {
                     this.activeHomingAcceleration = this.HomingAcceleration;
                     this.activeHomingTargetVelocity = 0;

                     this.homingRunState = HomingRunStates.stopFromToSwitch;
                  }
                  else
                  {
                     this.activeHomingTargetVelocity = this.HomingSwitchSpeed;
                  }
               }
               else if (HomingRunStates.stopFromToSwitch == this.homingRunState)
               {
                  if (0 == this.VelocityActualValue)
                  {
                     this.activeHomingAcceleration = this.HomingAcceleration;
                     this.activeHomingTargetVelocity = this.HomingZeroSpeed;
                     this.activeHomingTargetPosition = 4000000;

                     this.homingRunState = HomingRunStates.fromSwitch;
                  }
                  else
                  {
                     this.activeHomingTargetVelocity = 0;
                  }
               }
               else if (HomingRunStates.fromSwitch == this.homingRunState)
               {
                  if (false == this.homingSwitchActive)
                  {
                     this.activeHomingAcceleration = this.HomingAcceleration;
                     this.activeHomingTargetVelocity = 0;

                     this.homingRunState = HomingRunStates.stopFromFromSwitch;
                  }
                  else
                  {
                     this.activeHomingTargetVelocity = this.HomingZeroSpeed;
                  }
               }
               else if (HomingRunStates.stopFromFromSwitch == this.homingRunState)
               {
                  if (0 == this.VelocityActualValue)
                  {
                     this.activeHomingAcceleration = this.HomingAcceleration;
                     this.activeHomingTargetVelocity = this.HomingZeroSpeed;
                     this.activeHomingTargetPosition = this.PositionActualValue + this.HomeOffset;

                     this.homingRunState = HomingRunStates.moveOffset;
                  }
                  else
                  {
                     this.activeHomingTargetVelocity = 0;
                  }
               }
               else if (HomingRunStates.moveOffset == this.homingRunState)
               {
                  if (this.activeHomingTargetPosition == this.PositionActualValue)
                  {
                     this.activeHomingAcceleration = this.HomingAcceleration;
                     this.activeHomingTargetVelocity = 0;
                     this.activeHomingTargetPosition = 0;

                     this.homeDefined = true;

                     this.PositionActualValue = 0;

                     this.homingRunState = HomingRunStates.attained;
                  }
               }
               else if (HomingRunStates.attained == this.homingRunState)
               {
               }
            }
         }
         else if (HaltStates.halting == this.haltState)
         {
            if (0 == this.VelocityActualValue)
            {
               this.haltState = HaltStates.halted;
            }
         }
         else if (HaltStates.halted == this.haltState)
         {
            if (false == halted)
            {
               this.homingRunState = HomingRunStates.idle;
               this.haltState = HaltStates.notHalted;
            }
         }
      }

      #endregion

      #region Private Properties

      #region Common Entries
      private Int16 MotorAbortConnectionOption
      {
         set
         {
            this.SetValue(this.MotorAbortConnectionOptionLocation, this.MotorAbortConnectionOptionCodeLabel, "Motor Abort Connection Option Code", ref this.motorAbortConnectionOption, value, 4);
         }

         get
         {
            return (this.motorAbortConnectionOption);
         }
      }

      private UInt16 MotorErrorCode
      {
         set
         {
            this.SetValue(this.MotorErrorCodeLocation, this.MotorErrorCodeLabel, "Motor Error Code", ref this.motorErrorCode, value, 4);
         }

         get
         {
            return (this.motorErrorCode);
         }
      }

      private UInt32 DigitalInputs
      {
         set
         {
            this.SetValue(this.DigitalInputsLocation, this.DigitalInputsLabel, "Digital Inputs", ref this.digitalInputs, value, 8);
         }

         get
         {
            return (this.digitalInputs);
         }
      }

      private UInt32 DigitalOutputs
      {
         set
         {
            this.SetValue(this.DigitalOutputsLocation, this.DigitalOutputsLabel, "Digital Output", ref this.digitalOutputs, value, 8);
         }

         get
         {
            return (this.digitalOutputs);
         }
      }

      private UInt32 DigitalOutputsMask
      {
         set
         {
            this.SetValue(this.DigitalOutputsMaskLocation, this.DigitalOutputsMaskLabel, "Digital Output Mask", ref this.digitalOutputsMask, value, 8);
         }

         get
         {
            return (this.digitalOutputsMask);
         }
      }

      private UInt16 MotorType
      {
         set
         {
            this.SetValue(this.MotorTypeLocation, this.MotorTypeLabel, "Motor Type", ref this.motorType, value, 4);
         }

         get
         {
            return (this.motorType);
         }
      }

      private byte MotorTemperature
      {
         set
         {
            this.SetValue(this.MotorTemperatureLocation, this.MotorTemperatureLabel, "Motor Temperature", ref this.motorTemperature, value, 2);
         }

         get
         {
            return (this.motorTemperature);
         }
      }

      private byte MotorErrorTemperature
      {
         set
         {
            this.SetValue(this.MotorErrorTemperatureLocation, this.MotorErrorTemperatureLabel, "Motor Error Temperature", ref this.motorErrorTemperature, value, 2);
         }

         get
         {
            return (this.motorErrorTemperature);
         }
      }

      private UInt32 MotorSupportedDriveModes
      {
         set
         {
            this.SetValue(this.MotorSupportedDriveModesLocation, this.MotorSupportedDriveModesLabel, "Motor Supported Drive Modes", ref this.motorSupportedDriveModes, value, 8);
         }

         get
         {
            return (this.motorSupportedDriveModes);
         }
      }

      private UInt32 SingleDeviceType
      {
         set
         {
            this.SetValue(this.SingleDeviceTypeLocation, this.SingleDeviceTypeLabel, "Single Device Type", ref this.singleDeviceType, value, 8);
         }

         get
         {
            return (this.singleDeviceType);
         }
      }
      #endregion

      #region Device Control

      private UInt16 ControlWord
      {
         set
         {
            this.SetValue(this.ControlWordLocation, this.ControlWordLabel, "Control Word", ref this.controlWord, value, 4);
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
            this.SetValue(this.StatusWordLocation, this.StatusWordLabel, "Status Word", ref statusWord, value, 4);
         }

         get
         {
            return (this.statusWord);
         }
      }

      private byte SetMode
      {
         set
         {
            this.SetValue(this.SetModeLocation, this.SetModeLabel, "Set Mode", ref this.setMode, value, 2);
         }

         get
         {
            return (this.setMode);
         }
      }

      private byte GetMode
      {
         set
         {
            this.SetValue(this.GetModeLocation, this.GetModeLabel, "Get Mode", ref this.getMode, value, 2);
         }

         get
         {
            return (this.getMode);
         }
      }

      #endregion

      #region Homing Control

      private Int32 HomeOffset
      {
         set
         {
            this.SetValue(this.HomeOffsetLocation, this.HomeOffsetLabel, "Home Offset", ref this.homeOffset, value, 8);
         }

         get
         {
            return (this.homeOffset);
         }
      }

      private byte HomingMethod
      {
         set
         {
            this.SetValue(this.HomingMethodLocation, this.HomingMethodLabel, "Homing Method", ref this.homingMethod, value, 2);
         }

         get
         {
            return (this.homingMethod);
         }
      }

      private byte HomingSpeedHighest
      {
         set
         {
            this.SetValue(this.HomingSpeedHighestLocation, this.HomingSpeedHighestLabel, "Homing Speed Highest", ref this.homingSpeedHighest, value, 2);
         }

         get
         {
            return (this.homingSpeedHighest);
         }
      }

      private UInt32 HomingSwitchSpeed
      {
         set
         {
            this.SetValue(this.HomingSwitchSpeedLocation, this.HomingSwitchSpeedLabel, "Homing Switch Speed", ref this.homingSwitchSpeed, value, 8);
         }

         get
         {
            return (this.homingSwitchSpeed);
         }
      }

      private UInt32 HomingZeroSpeed
      {
         set
         {
            this.SetValue(this.HomingZeroSpeedLocation, this.HomingZeroSpeedLabel, "Homing Zero Speed", ref this.homingZeroSpeed, value, 8);
         }

         get
         {
            return (this.homingZeroSpeed);
         }
      }

      private UInt32 HomingAcceleration
      {
         set
         {
            this.SetValue(this.HomingAccelerationLocation, this.HomingAccelerationLabel, "Homing Acceleration", ref this.homingAcceleration, value, 8);
         }

         get
         {
            return (this.homingAcceleration);
         }
      }

      #endregion

      #region Position Control

      private Int32 PositionActualValue
      {
         set
         {
            this.SetValue(this.PositionActualValueLocation, this.PositionActualValueLabel, "Position Actual Value", ref this.positionActualValue, value, 8);
         }

         get
         {
            return (this.positionActualValue);
         }
      }


      private UInt32 PositionWindow
      {
         set
         {
            this.SetValue(this.PositionWindowLocation, this.PositionWindowLabel, "Position Window", ref this.positionWindow, value, 8);
         }

         get
         {
            return (this.positionWindow);
         }
      }


      private UInt16 PositionWindowTime
      {
         set
         {
            this.SetValue(this.PositionWindowTimeLocation, this.PositionWindowTimeLabel, "Position Window Time", ref this.positionWindowTime, value, 4);
         }

         get
         {
            return (this.positionWindowTime);
         }
      }

      private byte PositionControlParameterHighest
      {
         set
         {
            this.SetValue(this.PositionControlParameterHighestLocation, this.PositionControlParameterHighestLabel, "Position Control Parameter Highest", ref this.positionControlParameterHighest, value, 2);
         }

         get
         {
            return (this.positionControlParameterHighest);
         }
      }

      private Int32 PositionProportionalGainCoefficientKp
      {
         set
         {
            this.SetValue(this.PositionProportionalGainCoefficientKpLocation, this.PositionProportionalGainCoefficientKpLabel, "Position Proportional Gain Coefficient (KP)", ref this.positionProportionalGainCoefficientKp, value, 8);
         }

         get
         {
            return (this.positionProportionalGainCoefficientKp);
         }
      }

      private Int32 PositionIntegralGainCoefficienKi
      {
         set
         {
            this.SetValue(this.PositionIntegralGainCoefficienKiLocation, this.PositionIntegralGainCoefficienKiLabel, "Position Integral Gain Coefficien (KI)", ref this.positionIntegralGainCoefficienKi, value, 8);
         }

         get
         {
            return (this.positionIntegralGainCoefficienKi);
         }
      }

      private Int32 PositionDerivativeGainCoefficientKd
      {
         set
         {
            this.SetValue(this.PositionDerivativeGainCoefficientKdLocation, this.PositionDerivativeGainCoefficientKdLabel, "Position Derivative Gain Coefficient (KD)", ref this.positionDerivativeGainCoefficientKd, value, 8);
         }

         get
         {
            return (this.positionDerivativeGainCoefficientKd);
         }
      }

      #endregion

      #region Profile Positon Mode

      private Int32 TargetPosition
      {
         set
         {
            this.SetValue(this.TargetPositionLocation, this.TargetPositionLabel, "Target Position", ref this.targetPosition, value, 8);
         }

         get
         {
            return (this.targetPosition);
         }
      }


      private UInt32 ProfileVelocity
      {
         set
         {
            this.SetValue(this.ProfileVelocityLocation, this.ProfileVelocityLabel, "Profile Velocity", ref this.profileVelocity, value, 8);
         }

         get
         {
            return (this.profileVelocity);
         }
      }

      private UInt32 ProfileAcceleration
      {
         set
         {
            this.SetValue(this.ProfileAccelerationLocation, this.ProfileAccelerationLabel, "Profile Acceleration", ref this.profileAcceleration, value, 8);
         }

         get
         {
            return (this.profileAcceleration);
         }
      }

      private UInt32 ProfileDeceleration
      {
         set
         {
            this.SetValue(this.ProfileDecelerationLocation, this.ProfileDecelerationLabel, "Profile Deceleration", ref this.profileDeceleration, value, 8);
         }

         get
         {
            return (this.profileDeceleration);
         }
      }

      #endregion

      #region Profile Torque

      private Int16 TargetTorque
      {
         set
         {
            this.SetValue(this.TargetTorqueLocation, this.TargetTorqueLabel, "Target Torque", ref this.targetTorque, value, 4);
         }

         get
         {
            return (this.targetTorque);
         }
      }

      private Int16 CurrentActualValue
      {
         set
         {
            this.SetValue(this.CurrentActualValueLocation, this.CurrentActualValueLabel, "Current Actual Value", ref this.currentActualValue, value, 4);
         }

         get
         {
            return (this.currentActualValue);
         }
      }

      #endregion

      #region Profile Velocity Mode

      private Int32 VelocityActualValue
      {
         set
         {
            this.SetValue(this.VelocityActualValueLocation, this.VelocityActualValueLabel, "Velocity Actual Value", ref this.velocityActualValue, value, 8);
         }

         get
         {
            return (this.velocityActualValue);
         }
      }

      private UInt16 VelocityWindow
      {
         set
         {
            this.SetValue(this.VelocityWindowLocation, this.VelocityWindowLabel, "Velocity Window", ref this.velocityWindow, value, 4);
         }

         get
         {
            return (this.velocityWindow);
         }
      }

      private UInt16 VelocityWindowTime
      {
         set
         {
            this.SetValue(this.VelocityWindowTimeLocation, this.VelocityWindowTimeLabel, "Velocity Window Time", ref this.velocityWindowTime, value, 4);
         }

         get
         {
            return (this.velocityWindowTime);
         }
      }

      private UInt16 VelocityThreshold
      {
         set
         {
            this.SetValue(this.VelocityThresholdLocation, this.VelocityThresholdLabel, "Velocity Threshold", ref this.velocityThreshold, value, 4);
         }

         get
         {
            return (this.velocityThreshold);
         }
      }

      private UInt16 VelocityThresholdTime
      {
         set
         {
            this.SetValue(this.VelocityThresholdTimeLocation, this.VelocityThresholdTimeLabel, "Velocity Threshold Time", ref this.velocityThresholdTime, value, 4);
         }

         get
         {
            return (this.velocityThresholdTime);
         }
      }

      private byte VelocityControlParameterHighest
      {
         set
         {
            this.SetValue(this.VelocityControlParameterHighestLocation, this.VelocityControlParameterHighestLabel, "Velocity Control Parameter Highest", ref this.velocityControlParameterHighest, value, 2);
         }

         get
         {
            return (this.velocityControlParameterHighest);
         }
      }

      private Int32 VelocityProportionalGainCoefficientKp
      {
         set
         {
            this.SetValue(this.VelocityProportionalGainCoefficientKpLocation, this.VelocityProportionalGainCoefficientKpLabel, "Velocity Proportional Gain Coefficient (KP)", ref this.velocityProportionalGainCoefficientKp, value, 8);
         }

         get
         {
            return (this.velocityProportionalGainCoefficientKp);
         }
      }

      private Int32 VelocityIntegralGainCoefficienKi
      {
         set
         {
            this.SetValue(this.VelocityIntegralGainCoefficienKiLocation, this.VelocityIntegralGainCoefficienKiLabel, "Velocity Integral Gain Coefficien (KI)", ref this.velocityIntegralGainCoefficienKi, value, 8);
         }

         get
         {
            return (this.velocityIntegralGainCoefficienKi);
         }
      }

      private Int32 VelocityDerivativeGainCoefficientKd
      {
         set
         {
            this.SetValue(this.VelocityDerivativeGainCoefficientKdLocation, this.VelocityDerivativeGainCoefficientKdLabel, "Velocity Derivative Gain Coefficient (KD)", ref this.velocityDerivativeGainCoefficientKd, value, 8);
         }

         get
         {
            return (this.velocityDerivativeGainCoefficientKd);
         }
      }

      private Int32 TargetVelocity
      {
         set
         {
            this.SetValue(this.TargetVelocityLocation, this.TargetVelocityLabel, "Target Velocity", ref this.targetVelocity, value, 8);
         }

         get
         {
            return (this.targetVelocity);
         }
      }

      #endregion
      
      #endregion

      #region Public Properties

      public bool SupportVelocityMode { set; get; }
      public bool SupportPositionMode { set; get; }
      public bool SupportHomingMode { set; get; }

      public TpdoCheckHandler OnTpdoCheck { set; get; }

      public int MotorAbortConnectionOptionLocation { set; get; }
      public int MotorErrorCodeLocation { set; get; }
      public int DigitalInputsLocation { set; get; }
      public int DigitalOutputsHighestLocation { set; get; }
      public int DigitalOutputsLocation { set; get; }
      public int DigitalOutputsMaskLocation { set; get; }
      public int MotorTypeLocation { set; get; }
      public int MotorTemperatureHighestLocation { set; get; }
      public int MotorTemperatureLocation { set; get; }
      public int MotorErrorTemperatureLocation { set; get; }
      public int MotorSupportedDriveModesLocation { set; get; }
      public int SingleDeviceTypeLocation { set; get; }

      public int ControlWordLocation { set; get; }
      public int StatusWordLocation { set; get; }
      public int SetModeLocation { set; get; }
      public int GetModeLocation { set; get; }

      public int HomeOffsetLocation { set; get; }
      public int HomingMethodLocation { set; get; }
      public int HomingSpeedHighestLocation { set; get; }
      public int HomingSwitchSpeedLocation { set; get; }
      public int HomingZeroSpeedLocation { set; get; }
      public int HomingAccelerationLocation { set; get; }

      public int PositionActualValueLocation { set; get; }
      public int PositionWindowLocation { set; get; }
      public int PositionWindowTimeLocation  { set; get; }
      public int PositionControlParameterHighestLocation { set; get; }
      public int PositionProportionalGainCoefficientKpLocation { set; get; }
      public int PositionIntegralGainCoefficienKiLocation { set; get; }
      public int PositionDerivativeGainCoefficientKdLocation { set; get; }

      public int TargetPositionLocation { set; get; }
      public int ProfileVelocityLocation { set; get; }
      public int ProfileAccelerationLocation { set; get; }
      public int ProfileDecelerationLocation { set; get; }

      public int TargetTorqueLocation { set; get; }
      public int CurrentActualValueLocation { set; get; }

      public int VelocityActualValueLocation { set; get; }
      public int VelocityWindowLocation { set; get; }
      public int VelocityWindowTimeLocation { set; get; }
      public int VelocityThresholdLocation { set; get; }
      public int VelocityThresholdTimeLocation { set; get; }
      public int VelocityControlParameterHighestLocation { set; get; }
      public int VelocityProportionalGainCoefficientKpLocation { set; get; }
      public int VelocityIntegralGainCoefficienKiLocation { set; get; }
      public int VelocityDerivativeGainCoefficientKdLocation { set; get; }
      public int TargetVelocityLocation { set; get; }

      public bool AutoHomeEnabled 
      { 
         set
         {
            this.AutoHomEnabledCheckBox.Checked = value;
         }

         get
         {
            return (this.AutoHomEnabledCheckBox.Checked);
         }
      }

      #endregion

      #region User Events

      private void SetMotorTemperatureButton_Click(object sender, EventArgs e)
      {
         double motorTemperature = 0;

         if (double.TryParse(this.MotorTemperatureTextBox.Text, out motorTemperature) != false) 
         {
            this.MotorTemperature = (byte)motorTemperature;
         }
      }

      private void HomeSwitchButton_MouseDown(object sender, MouseEventArgs e)
      {
         this.homingSwitchActive = true;
      }

      private void HomeSwitchButton_MouseUp(object sender, MouseEventArgs e)
      {
         this.homingSwitchActive = false;
      }

      #endregion

      #region Form Events

      private void MainPanel_Scroll(object sender, ScrollEventArgs e)
      {
         this.FocusTakerLabel.Focus();
         this.MainPanel.VerticalScroll.Value = e.NewValue;
      }

      private void ValuePanel_Scroll(object sender, ScrollEventArgs e)
      {
         this.FocusTakerLabel.Focus();
         this.ValuePanel.VerticalScroll.Value = e.NewValue;
      }

      #endregion

      #region Constructor

      public MainMotor()
      {
         this.InitializeComponent();
      }

      #endregion

      #region Access Methods

      public void CreateLabels()
      {
         int top = 4;
         this.ValuePanel.SuspendLayout();

         this.CreateLabel(MotorAbortConnectionOptionLocation, "MotorAbortConnectionOptionCodeLabel", out this.MotorAbortConnectionOptionCodeLabel, ref top);
         this.CreateLabel(MotorErrorCodeLocation, "MotorErrorCodeLabel", out this.MotorErrorCodeLabel, ref top);
         this.CreateLabel(DigitalInputsLocation, "DigitalInputsLabel", out this.DigitalInputsLabel, ref top);
         this.CreateLabel(DigitalOutputsLocation, "DigitalOutputsLabel", out this.DigitalOutputsLabel, ref top);
         this.CreateLabel(DigitalOutputsMaskLocation, "DigitalOutputsMaskLabel", out this.DigitalOutputsMaskLabel, ref top);
         this.CreateLabel(MotorTypeLocation, "MotorTypeLabel", out this.MotorTypeLabel, ref top);
         this.CreateLabel(MotorTemperatureLocation, "MotorTemperatureLabel", out this.MotorTemperatureLabel, ref top);
         this.CreateLabel(MotorErrorTemperatureLocation, "MotorErrorTemperatureLabel", out this.MotorErrorTemperatureLabel, ref top);
         this.CreateLabel(MotorSupportedDriveModesLocation, "MotorSupportedDriveModesLabel", out this.MotorSupportedDriveModesLabel, ref top);
         this.CreateLabel(SingleDeviceTypeLocation, "SingleDeviceTypeLabel", out this.SingleDeviceTypeLabel, ref top);

         top += 8;

         this.CreateLabel(ControlWordLocation, "ControlWordLabel", out this.ControlWordLabel, ref top);
         this.CreateLabel(StatusWordLocation, "StatusWordLabel", out this.StatusWordLabel, ref top);
         this.CreateLabel(SetModeLocation, "SetModeLabel", out this.SetModeLabel, ref top);
         this.CreateLabel(GetModeLocation, "GetModeLabel", out this.GetModeLabel, ref top);

         top += 8;

         this.CreateLabel(HomeOffsetLocation, "HomeOffsetLabel", out this.HomeOffsetLabel, ref top);
         this.CreateLabel(HomingMethodLocation, "HomingMethodLabel", out this.HomingMethodLabel, ref top);
         this.CreateLabel(HomingSpeedHighestLocation, "HomingSpeedHighestLabel", out this.HomingSpeedHighestLabel, ref top);
         this.CreateLabel(HomingSwitchSpeedLocation, "HomingSwitchSpeedLabel", out this.HomingSwitchSpeedLabel, ref top);
         this.CreateLabel(HomingZeroSpeedLocation, "HomingZeroSpeedLabel", out this.HomingZeroSpeedLabel, ref top);
         this.CreateLabel(HomingAccelerationLocation, "HomingAccelerationLabel", out this.HomingAccelerationLabel, ref top);

         top += 8;

         this.CreateLabel(PositionActualValueLocation, "PositionActualValueLabel", out this.PositionActualValueLabel, ref top);
         this.CreateLabel(PositionWindowLocation, "PositionWindowLabel", out this.PositionWindowLabel, ref top);
         this.CreateLabel(PositionWindowTimeLocation, "PositionWindowTimeLabel", out this.PositionWindowTimeLabel, ref top);
         this.CreateLabel(PositionControlParameterHighestLocation, "PositionControlParameterHighestLabel", out this.PositionControlParameterHighestLabel, ref top);
         this.CreateLabel(PositionProportionalGainCoefficientKpLocation, "PositionProportionalGainCoefficientKpLabel", out this.PositionProportionalGainCoefficientKpLabel, ref top);
         this.CreateLabel(PositionIntegralGainCoefficienKiLocation, "PositionIntegralGainCoefficienKiLabel", out this.PositionIntegralGainCoefficienKiLabel, ref top);
         this.CreateLabel(PositionDerivativeGainCoefficientKdLocation, "PositionDerivativeGainCoefficientKdLabel", out this.PositionDerivativeGainCoefficientKdLabel, ref top);

         top += 8;

         this.CreateLabel(TargetPositionLocation, "TargetPositionLabel", out this.TargetPositionLabel, ref top);
         this.CreateLabel(ProfileVelocityLocation, "ProfileVelocityLabel", out this.ProfileVelocityLabel, ref top);
         this.CreateLabel(ProfileAccelerationLocation, "ProfileAccelerationLabel", out this.ProfileAccelerationLabel, ref top);
         this.CreateLabel(ProfileDecelerationLocation, "ProfileDecelerationLabel", out this.ProfileDecelerationLabel, ref top);

         top += 8;
         
         this.CreateLabel(TargetTorqueLocation, "TargetTorqueLabel", out this.TargetTorqueLabel, ref top);
         this.CreateLabel(CurrentActualValueLocation, "CurrentActualValueLabel", out this.CurrentActualValueLabel, ref top);

         top += 8;
                  
         this.CreateLabel(VelocityActualValueLocation, "VelocityActualValueLabel", out this.VelocityActualValueLabel, ref top);
         this.CreateLabel(VelocityWindowLocation, "VelocityWindowLabel", out this.VelocityWindowLabel, ref top);
         this.CreateLabel(VelocityWindowTimeLocation, "VelocityWindowTimeLabel", out this.VelocityWindowTimeLabel, ref top);
         this.CreateLabel(VelocityThresholdLocation, "VelocityThresholdLabel", out this.VelocityThresholdLabel, ref top);
         this.CreateLabel(VelocityThresholdTimeLocation, "VelocityThresholdTimeLabel", out this.VelocityThresholdTimeLabel, ref top);
         this.CreateLabel(VelocityControlParameterHighestLocation, "VelocityControlParameterHighestLabel", out this.VelocityControlParameterHighestLabel, ref top);
         this.CreateLabel(VelocityProportionalGainCoefficientKpLocation, "VelocityProportionalGainCoefficientKpLabel", out this.VelocityProportionalGainCoefficientKpLabel, ref top);
         this.CreateLabel(VelocityIntegralGainCoefficienKiLocation, "VelocityIntegralGainCoefficienKiLabel", out this.VelocityIntegralGainCoefficienKiLabel, ref top);
         this.CreateLabel(VelocityDerivativeGainCoefficientKdLocation, "VelocityDerivativeGainCoefficientKdLabel", out this.VelocityDerivativeGainCoefficientKdLabel, ref top);
         this.CreateLabel(TargetVelocityLocation, "TargetVelocityLabel", out this.TargetVelocityLabel, ref top);

         this.ValuePanel.ResumeLayout(false);
         this.ValuePanel.PerformLayout();

         this.FormMotorTemperatureLabel.Visible = (0 != MotorTemperatureLocation) ? true : false;
         this.MotorTemperatureTextBox.Visible = (0 != MotorTemperatureLocation) ? true : false;
         this.SetMotorTemperatureButton.Visible = (0 != MotorTemperatureLocation) ? true : false;         

         this.HomeSwitchButton.Visible = this.SupportHomingMode;
      }

      public bool RPdoMappable(UInt16 index, byte subIndex)
      {
         bool result = false;
         int location = (int)((index << 8) + subIndex);

         if ((location == this.MotorAbortConnectionOptionLocation) ||
             (location == this.DigitalOutputsLocation) ||
             (location == this.DigitalOutputsMaskLocation) ||
             (location == this.MotorTypeLocation) ||
             (location == this.MotorErrorTemperatureLocation) ||
             (location == this.SingleDeviceTypeLocation) ||

             (location == this.ControlWordLocation) ||
             (location == this.SetModeLocation) ||

             (location == this.HomeOffsetLocation) ||
             (location == this.HomingMethodLocation) ||
             (location == this.HomingSwitchSpeedLocation) ||
             (location == this.HomingZeroSpeedLocation) ||
             (location == this.HomingAccelerationLocation) ||

             (location == this.TargetTorqueLocation) ||

             (location == this.PositionWindowLocation) ||
             (location == this.PositionWindowTimeLocation) ||
             (location == this.PositionProportionalGainCoefficientKpLocation) ||
             (location == this.PositionIntegralGainCoefficienKiLocation) ||
             (location == this.PositionDerivativeGainCoefficientKdLocation) ||

             (location == this.TargetPositionLocation) ||
             (location == this.ProfileVelocityLocation) ||
             (location == this.ProfileAccelerationLocation) ||
             (location == this.ProfileDecelerationLocation) ||

             (location == this.VelocityWindowLocation) ||
             (location == this.VelocityWindowTimeLocation) ||
             (location == this.VelocityThresholdLocation) ||
             (location == this.VelocityThresholdTimeLocation) ||
             (location == this.VelocityProportionalGainCoefficientKpLocation) ||
             (location == this.VelocityIntegralGainCoefficienKiLocation) ||
             (location == this.VelocityDerivativeGainCoefficientKdLocation) ||
             (location == this.TargetVelocityLocation))
         {
            result = true;
         }

         return (result);
      }

      public int RPdoSize(UInt16 index, byte subIndex)
      {
         int result = 0;
         int location = (int)((index << 8) + subIndex);

         if ((location == this.MotorErrorTemperatureLocation) ||

             (location == this.SetModeLocation) ||

             (location == this.HomingMethodLocation))
         {
            result = 1;
         }
         else if ((location == this.MotorAbortConnectionOptionLocation) ||
                  (location == this.MotorTypeLocation) ||

                  (location == this.ControlWordLocation) ||

                  (location == this.PositionWindowTimeLocation) ||

                  (location == this.VelocityWindowLocation) ||
                  (location == this.VelocityWindowTimeLocation) ||
                  (location == this.VelocityThresholdLocation) ||
                  (location == this.VelocityThresholdTimeLocation) ||

                  (location == this.TargetTorqueLocation))
         {
            result = 2;
         }
         else if ((location == this.DigitalOutputsLocation) ||
                  (location == this.DigitalOutputsMaskLocation) ||
                  (location == this.SingleDeviceTypeLocation) ||

                  (location == this.PositionWindowLocation) ||
                  (location == this.PositionProportionalGainCoefficientKpLocation) ||
                  (location == this.PositionIntegralGainCoefficienKiLocation) ||
                  (location == this.PositionDerivativeGainCoefficientKdLocation) ||

                  (location == this.HomeOffsetLocation) ||
                  (location == this.HomingSwitchSpeedLocation) ||
                  (location == this.HomingZeroSpeedLocation) ||
                  (location == this.HomingAccelerationLocation) ||

                  (location == this.TargetPositionLocation) ||
                  (location == this.ProfileVelocityLocation) ||
                  (location == this.ProfileAccelerationLocation) ||
                  (location == this.ProfileDecelerationLocation) ||

                  (location == this.VelocityProportionalGainCoefficientKpLocation) ||
                  (location == this.VelocityIntegralGainCoefficienKiLocation) ||
                  (location == this.VelocityDerivativeGainCoefficientKdLocation) ||
                  (location == this.TargetVelocityLocation))
         {
            result = 4;
         }

         return (result);
      }

      public bool TPdoMappable(UInt16 index, byte subIndex)
      {
         bool result = false;
         int location = (int)((index << 8) + subIndex);

         if ((location == this.MotorAbortConnectionOptionLocation) ||
             (location == this.MotorErrorCodeLocation) ||
             (location == this.DigitalInputsLocation) ||
             (location == this.DigitalOutputsLocation) ||
             (location == this.DigitalOutputsMaskLocation) ||
             (location == this.MotorTypeLocation) ||
             (location == this.MotorTemperatureLocation) ||
             (location == this.MotorErrorTemperatureLocation) ||
             (location == this.SingleDeviceTypeLocation) ||

             (location == this.ControlWordLocation) ||
             (location == this.StatusWordLocation) ||
             (location == this.SetModeLocation) ||
             (location == this.GetModeLocation) ||

             (location == this.HomeOffsetLocation) ||
             (location == this.HomingMethodLocation) ||
             (location == this.HomingSwitchSpeedLocation) ||
             (location == this.HomingZeroSpeedLocation) ||
             (location == this.HomingAccelerationLocation) ||

             (location == this.PositionActualValueLocation) ||
             (location == this.PositionWindowLocation) ||
             (location == this.PositionWindowTimeLocation) ||
             (location == this.PositionProportionalGainCoefficientKpLocation) ||
             (location == this.PositionIntegralGainCoefficienKiLocation) ||
             (location == this.PositionDerivativeGainCoefficientKdLocation) ||

             (location == this.TargetPositionLocation) ||
             (location == this.ProfileVelocityLocation) ||
             (location == this.ProfileAccelerationLocation) ||
             (location == this.ProfileDecelerationLocation) ||

             (location == this.TargetTorqueLocation) ||
             (location == this.CurrentActualValueLocation) ||
            
             (location == this.VelocityActualValueLocation) ||
             (location == this.VelocityWindowLocation) ||
             (location == this.VelocityWindowTimeLocation) ||
             (location == this.VelocityThresholdLocation) ||
             (location == this.VelocityThresholdTimeLocation) ||
             (location == this.VelocityProportionalGainCoefficientKpLocation) ||
             (location == this.VelocityIntegralGainCoefficienKiLocation) ||
             (location == this.VelocityDerivativeGainCoefficientKdLocation) ||
             (location == this.TargetVelocityLocation))
         {
            result = true;
         }

         return (result);
      }

      public int TPdoSize(UInt16 index, byte subIndex)
      {
         int result = 0;
         int location = (int)((index << 8) + subIndex);

         if ((location == this.MotorTemperatureLocation) ||
             (location == this.MotorErrorTemperatureLocation) ||
             (location == this.SetModeLocation) ||
             (location == this.GetModeLocation) ||

             (location == this.HomingMethodLocation))
         {
            result = 1;
         }
         else if ((location == this.MotorAbortConnectionOptionLocation) ||
                  (location == this.MotorErrorCodeLocation) ||
                  (location == this.MotorTypeLocation) ||
         
                  (location == this.ControlWordLocation) ||
                  (location == this.StatusWordLocation) ||

                  (location == this.PositionWindowLocation) ||

                  (location == this.TargetTorqueLocation) ||
                  (location == this.CurrentActualValueLocation) ||

                  (location == this.VelocityWindowLocation) ||
                  (location == this.VelocityWindowTimeLocation) ||
                  (location == this.VelocityThresholdLocation) ||
                  (location == this.VelocityThresholdTimeLocation))
         {
            result = 2;
         }
         else if ((location == this.DigitalInputsLocation) ||
                  (location == this.DigitalOutputsLocation) ||
                  (location == this.DigitalOutputsMaskLocation) ||
                  (location == this.SingleDeviceTypeLocation) ||
   
                  (location == this.PositionActualValueLocation) ||
                  (location == this.PositionWindowTimeLocation) ||
                  (location == this.PositionProportionalGainCoefficientKpLocation) ||
                  (location == this.PositionIntegralGainCoefficienKiLocation) ||
                  (location == this.PositionDerivativeGainCoefficientKdLocation) ||

                  (location == this.HomeOffsetLocation) ||
                  (location == this.HomingSwitchSpeedLocation) ||
                  (location == this.HomingZeroSpeedLocation) ||
                  (location == this.HomingAccelerationLocation) ||

                  (location == this.TargetPositionLocation) ||
                  (location == this.ProfileVelocityLocation) ||
                  (location == this.ProfileAccelerationLocation) ||
                  (location == this.ProfileDecelerationLocation) ||

                  (location == this.VelocityActualValueLocation) ||
                  (location == this.VelocityProportionalGainCoefficientKpLocation) ||
                  (location == this.VelocityIntegralGainCoefficienKiLocation) ||
                  (location == this.VelocityDerivativeGainCoefficientKdLocation) ||
                  (location == this.TargetVelocityLocation))
         {
            result = 4;
         }

         return (result);
      }

      public bool StoreDeviceData(UInt16 index, byte subIndex, byte[] buffer, int offset, UInt32 length)
      {
         bool valid = false;
         int location = (int)((index << 8) + subIndex);

         if ((location == this.MotorAbortConnectionOptionLocation) && (2 == length))
         {
            this.MotorAbortConnectionOption = BitConverter.ToInt16(buffer, offset);
            valid = true;
         }
         else if ((location == this.MotorErrorCodeLocation) && (2 == length))
         {
            this.MotorErrorCode = BitConverter.ToUInt16(buffer, offset);
            valid = true;
         }
         else if ((location == this.DigitalOutputsLocation) && (4 == length))
         {
            this.DigitalOutputs = BitConverter.ToUInt32(buffer, offset);
            valid = true;
         }
         else if ((location == this.DigitalOutputsMaskLocation) && (4 == length))
         {
            this.DigitalOutputsMask = BitConverter.ToUInt32(buffer, offset);
            valid = true;
         }
         else if ((location == this.MotorTypeLocation) && (2 == length))
         {
            this.MotorType = BitConverter.ToUInt16(buffer, offset);
            valid = true;
         }
         else if ((location == this.MotorErrorTemperatureLocation) && (1 == length))
         {
            this.MotorErrorTemperature = buffer[offset];
            valid = true;
         }
         else if ((location == this.SingleDeviceTypeLocation) && (4 == length))
         {
            this.SingleDeviceType = BitConverter.ToUInt32(buffer, offset);
            valid = true;
         }

         else if ((location == this.ControlWordLocation) && (2 == length))
         {
            UInt16 value = BitConverter.ToUInt16(buffer, offset);
            valid = this.SetControlWord(value);
         }
         else if ((location == this.SetModeLocation) && (1 == length))
         {
            this.SetMode = buffer[offset];
            valid = true;
         }

         else if (location == this.HomeOffsetLocation)
         {
            this.HomeOffset = BitConverter.ToInt32(buffer, offset);
            valid = true;
         }
         else if (location == this.HomingMethodLocation)
         {
            this.HomingMethod = buffer[offset];
            valid = true;
         }
         else if (location == this.HomingSwitchSpeedLocation)
         {
            this.HomingSwitchSpeed = BitConverter.ToUInt32(buffer, offset);
            valid = true;
         }
         else if (location == this.HomingZeroSpeedLocation)
         {
            this.HomingZeroSpeed = BitConverter.ToUInt32(buffer, offset);
            valid = true;
         }
         else if (location == this.HomingAccelerationLocation)
         {
            this.HomingAcceleration = BitConverter.ToUInt32(buffer, offset);
            valid = true;
         }

         else if ((location == this.PositionWindowLocation) && (4 == length))
         {
            this.PositionWindow = BitConverter.ToUInt32(buffer, offset);
            valid = true;
         }
         else if ((location == this.PositionWindowTimeLocation) && (2 == length))
         {
            this.PositionWindowTime = BitConverter.ToUInt16(buffer, offset);
            valid = true;
         }
         else if ((location == this.PositionProportionalGainCoefficientKpLocation) && (4 == length))
         {
            this.PositionProportionalGainCoefficientKp = BitConverter.ToInt32(buffer, offset);
            valid = true;
         }
         else if ((location == this.PositionIntegralGainCoefficienKiLocation) && (4 == length))
         {
            this.PositionIntegralGainCoefficienKi = BitConverter.ToInt32(buffer, offset);
            valid = true;
         }
         else if ((location == this.PositionDerivativeGainCoefficientKdLocation) && (4 == length))
         {
            this.PositionDerivativeGainCoefficientKd = BitConverter.ToInt32(buffer, offset);
            valid = true;
         }

         else if ((location == this.TargetPositionLocation) && (4 == length))
         {
            this.TargetPosition = BitConverter.ToInt32(buffer, offset);
            valid = true;
         }
         else if ((location == this.ProfileVelocityLocation) && (4 == length))
         {
            this.ProfileVelocity = BitConverter.ToUInt32(buffer, offset);
            valid = true;
         }
         else if ((location == this.ProfileAccelerationLocation) && (4 == length))
         {
            this.ProfileAcceleration = BitConverter.ToUInt32(buffer, offset);
            valid = true;
         }
         else if ((location == this.ProfileDecelerationLocation) && (4 == length))
         {
            this.ProfileDeceleration = BitConverter.ToUInt32(buffer, offset);
            valid = true;
         }

         else if (location == this.TargetTorqueLocation)
         {
            this.TargetTorque = BitConverter.ToInt16(buffer, offset);
            valid = true;
         }

         else if ((location == this.VelocityWindowLocation) && (2 == length))
         {
            this.VelocityWindow = BitConverter.ToUInt16(buffer, offset);
            valid = true;
         }
         else if ((location == this.VelocityWindowTimeLocation) && (2 == length))
         {
            this.VelocityWindowTime = BitConverter.ToUInt16(buffer, offset);
            valid = true;
         }
         else if ((location == this.VelocityThresholdLocation) && (2 == length))
         {
            this.VelocityThreshold = BitConverter.ToUInt16(buffer, offset);
            valid = true;
         }
         else if ((location == this.VelocityThresholdTimeLocation) && (2 == length))
         {
            this.VelocityThresholdTime = BitConverter.ToUInt16(buffer, offset);
            valid = true;
         }
         else if ((location == this.VelocityProportionalGainCoefficientKpLocation) && (4 == length))
         {
            this.VelocityProportionalGainCoefficientKp = BitConverter.ToInt32(buffer, offset);
            valid = true;
         }
         else if ((location == this.VelocityIntegralGainCoefficienKiLocation) && (4 == length))
         {
            this.VelocityIntegralGainCoefficienKi = BitConverter.ToInt32(buffer, offset);
            valid = true;
         }
         else if ((location == this.VelocityDerivativeGainCoefficientKdLocation) && (4 == length))
         {
            this.VelocityDerivativeGainCoefficientKd = BitConverter.ToInt32(buffer, offset);
            valid = true;
         }
         else if ((location == this.TargetVelocityLocation) && (4 == length))
         {
            this.TargetVelocity = BitConverter.ToInt32(buffer, offset);
            valid = true;
         }        

         return (valid);
      }

      public bool LoadDeviceData(UInt16 index, byte subIndex, byte[] buffer, ref UInt32 dataLength)
      {
         bool valid = false;
         int location = (int)((index << 8) + subIndex);

         if (location == this.MotorAbortConnectionOptionLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.MotorAbortConnectionOption);
            valid = true;
         }
         else if (location == this.MotorErrorCodeLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.MotorErrorCode);
            valid = true;
         }
         else if (location == this.DigitalInputsLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.DigitalInputs);
            valid = true;
         }
         else if (location == this.DigitalOutputsHighestLocation)
         {
            dataLength = this.MoveDeviceData(buffer, (byte)2);
            valid = true;
         }
         else if (location == this.DigitalOutputsLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.DigitalOutputs);
            valid = true;
         }
         else if (location == this.DigitalOutputsMaskLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.DigitalOutputsMask);
            valid = true;
         }
         else if (location == this.MotorTypeLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.MotorType);
            valid = true;
         }
         else if (location == this.MotorTemperatureHighestLocation)
         {
            dataLength = this.MoveDeviceData(buffer, (byte)2);
            valid = true;
         }
         else if (location == this.MotorTemperatureLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.MotorTemperature);
            valid = true;
         }
         else if (location == this.MotorErrorTemperatureLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.MotorErrorTemperature);
            valid = true;
         }
         else if (location == this.MotorSupportedDriveModesLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.MotorSupportedDriveModes);
            valid = true;
         }
         else if (location == this.SingleDeviceTypeLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.SingleDeviceType);
            valid = true;
         }

         else if (location == this.ControlWordLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.ControlWord);
            valid = true;
         }
         else if (location == this.StatusWordLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.StatusWord);
            valid = true;
         }
         else if (location == this.SetModeLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.SetMode);
            valid = true;
         }
         else if (location == this.GetModeLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.GetMode);
            valid = true;
         }

         else if (location == this.HomeOffsetLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.HomeOffset);
            valid = true;
         }
         else if (location == this.HomingMethodLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.HomingMethod);
            valid = true;
         }
         else if (location == this.HomingSpeedHighestLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.HomingSpeedHighest);
            valid = true;
         }
         else if (location == this.HomingSwitchSpeedLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.HomingSwitchSpeed);
            valid = true;
         }
         else if (location == this.HomingZeroSpeedLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.HomingZeroSpeed);
            valid = true;
         }
         else if (location == this.HomingAccelerationLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.HomingAcceleration);
            valid = true;
         }

         else if (location == this.PositionActualValueLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.PositionActualValue);
            valid = true;
         }
         else if (location == this.PositionWindowLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.PositionWindow);
            valid = true;
         }
         else if (location == this.PositionWindowTimeLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.PositionWindowTime);
            valid = true;
         }
         else if (location == this.PositionControlParameterHighestLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.PositionControlParameterHighest);
            valid = true;
         }         
         else if (location == this.PositionProportionalGainCoefficientKpLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.PositionProportionalGainCoefficientKp);
            valid = true;
         }
         else if (location == this.PositionIntegralGainCoefficienKiLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.PositionIntegralGainCoefficienKi);
            valid = true;
         }
         else if (location == this.PositionDerivativeGainCoefficientKdLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.PositionDerivativeGainCoefficientKd);
            valid = true;
         }

         else if (location == this.TargetPositionLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.TargetPosition);
            valid = true;
         }
         else if (location == this.ProfileVelocityLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.ProfileVelocity);
            valid = true;
         }
         else if (location == this.ProfileAccelerationLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.ProfileAcceleration);
            valid = true;
         }
         else if (location == this.ProfileDecelerationLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.ProfileDeceleration);
            valid = true;
         }

         else if (location == this.TargetTorqueLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.TargetTorque);
            valid = true;
         }
         else if (location == this.CurrentActualValueLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.CurrentActualValue);
            valid = true;
         }

         else if (location == this.VelocityActualValueLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.VelocityActualValue);
            valid = true;
         }


         else if (location == this.VelocityWindowLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.VelocityWindow);
            valid = true;
         }
         else if (location == this.VelocityWindowTimeLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.VelocityWindowTime);
            valid = true;
         }
         else if (location == this.VelocityThresholdLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.VelocityThreshold);
            valid = true;
         }
         else if (location == this.VelocityThresholdTimeLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.VelocityThresholdTime);
            valid = true;
         }
         else if (location == this.VelocityControlParameterHighestLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.VelocityControlParameterHighest);
            valid = true;
         }
         else if (location == this.VelocityProportionalGainCoefficientKpLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.VelocityProportionalGainCoefficientKp);
            valid = true;
         }
         else if (location == this.VelocityIntegralGainCoefficienKiLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.VelocityIntegralGainCoefficienKi);
            valid = true;
         }
         else if (location == this.VelocityDerivativeGainCoefficientKdLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.VelocityDerivativeGainCoefficientKd);
            valid = true;
         }
         else if (location == this.TargetVelocityLocation)
         {
            dataLength = this.MoveDeviceData(buffer, this.TargetVelocity);
            valid = true;
         }

         return (valid);
      }

      public void Reset(bool fromPowerUp)
      {
         this.deviceState = DeviceStates.preop;
         this.motorState = MotorStates.notReadyToSwitchOn;

         if (false != fromPowerUp)
         {
            this.PositionActualValue = 0;

            this.MotorErrorCode = 0;
            this.DigitalInputs = 0;
            this.MotorTemperature = 23;
         }

         this.homeDefined |= this.AutoHomeEnabled;
         this.homingSwitchActive = false;

         this.MotorAbortConnectionOption = 0;
         this.DigitalOutputs = 0;
         this.DigitalOutputsMask = 0;
         this.MotorType = 0;
         this.MotorErrorTemperature = 0;
         this.MotorSupportedDriveModes = 0;
         this.SingleDeviceType = 0;

         this.ControlWord = 0;
         this.StatusWord = this.GetStatusWord();
         this.SetMode = 0xFF;
         this.GetMode = 0xFF;

         this.HomeOffset = 0;
         this.HomingMethod = 0;
         this.HomingSpeedHighest = 2;
         this.HomingSwitchSpeed = 1000;
         this.HomingZeroSpeed = 500;
         this.HomingAcceleration = 100;

         this.PositionWindow = 0;
         this.PositionWindowTime = 0;
         this.PositionControlParameterHighest = 3;
         this.PositionProportionalGainCoefficientKp = 0;
         this.PositionIntegralGainCoefficienKi = 0;
         this.PositionDerivativeGainCoefficientKd = 0;

         this.TargetPosition = 0;
         this.ProfileVelocity = 1000;
         this.ProfileAcceleration = 100;
         this.ProfileDeceleration = 100;

         this.TargetTorque = 0;
         this.CurrentActualValue = 0;

         this.VelocityActualValue = 0;
         this.VelocityWindow = 0;
         this.VelocityWindowTime = 0;
         this.VelocityThreshold = 0;
         this.VelocityThresholdTime = 0;
         this.VelocityControlParameterHighest = 3;
         this.VelocityProportionalGainCoefficientKp = 0;
         this.VelocityIntegralGainCoefficienKi = 0;
         this.VelocityDerivativeGainCoefficientKd = 0;         
         this.TargetVelocity = 0;

         DateTime now = DateTime.Now;
         this.updateTime = now.AddMilliseconds(100);
         this.lastUpdateTime = now;
      }

      public void Start()
      {
         this.deviceState = DeviceStates.running;
      }

      public void Stop()
      {
         this.deviceState = DeviceStates.stopped;
      }

      public void UpdateDevice()
      {
         DateTime now = DateTime.Now;

         if (this.GetMode != this.SetMode)
         {
            this.GetMode = this.SetMode;
         }

         if (MotorStates.notReadyToSwitchOn == this.motorState)
         {
            // transition 1
            this.motorState = MotorStates.switchOnDisabled;
         }
         else if (MotorStates.faultReactionActive == this.motorState)
         {
#if false
            // load fault error code
            this.ErrorStatus = (UInt32)(this.ErrorCode & 0xFFFFFFFF);

            // emit emergency code
            int cobId = this.GetCobId(COBTypes.EMGY, this.nodeId);
            byte[] emergencyMsg = BitConverter.GetBytes(this.ErrorCode);
            this.Transmit(cobId, emergencyMsg);
#endif

            // transition 14
            this.motorState = MotorStates.notReadyToSwitchOnFaulted;
         }

         this.StatusWord = this.GetStatusWord();

         if (now > this.updateTime)
         {
            TimeSpan ts = now - this.lastUpdateTime;
            this.lastUpdateTime = now;
            double elapsedSeconds = ts.TotalSeconds;
            this.updateTime = now.AddMilliseconds(100);

            bool operating = false;
            
            if ((DeviceStates.running == this.deviceState) && 
                (MotorStates.operationalEnable == this.motorState))
            {
               operating = true;
            }

            if (false != operating)
            {
               if ((1 == this.GetMode) && (false != this.SupportPositionMode))
               {
                  this.UpdatePositionMode(this.TargetPosition, this.ProfileVelocity, this.ProfileAcceleration, this.ProfileDeceleration, elapsedSeconds);
               }
               else if ((3 == this.GetMode) && (false != this.SupportVelocityMode))
               {
                  this.UpdateVelocityMode(this.TargetVelocity, this.ProfileAcceleration, this.ProfileDeceleration, elapsedSeconds);
               }
               else if ((6 == this.GetMode) && (false != this.SupportHomingMode))
               {
                  this.UpdateHomingMode(elapsedSeconds);
               }
            }
            else
            {
               this.UpdateVelocity(0, this.ProfileAcceleration, this.ProfileDeceleration, elapsedSeconds);
               this.UpdatePosition(elapsedSeconds);
            }
         }

      }

      #endregion

   }
}
