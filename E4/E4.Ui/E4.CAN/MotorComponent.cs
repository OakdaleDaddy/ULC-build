using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E4.CAN
{
   public class MotorComponent
   {
      #region Definition

      public enum Modes
      {
         off,
         velocity,
         position,
         homing,
         undefined,
      }

      public delegate bool CommExchangeHandler(CommAction action, int timeout, int attemptLimit);
      public delegate bool TargetPositionScheduleHandler();
      public delegate bool TargetVelocityScheduleHandler();

      #endregion

      #region Fields

      private bool positionAttained;
      private bool velocityAttained;
      private bool targetPositionRelative;
      private bool homingAttained;
      private bool homeDefined;

      public UInt16 controlWord;
      private Modes mode;

      #endregion

      #region Helper Functions

      private bool ExchangeCommAction(CommAction action, int timeout = 200, int attemptLimit = 2)
      {
         bool result = false;

         if (null != this.OnCommExchange)
         {
            result = this.OnCommExchange(action, timeout, attemptLimit);
         }

         return (result);          
      }

      private bool ScheduleTargetPosition()
      {
         bool result = false;

         if (null != this.OnTargetPositionSchedule)
         {
            result = this.OnTargetPositionSchedule();
         }

         return (result);
      }

      private bool ScheduleTargetVelocity()
      {
         bool result = false;

         if (null != this.OnTargetVelocitySchedule)
         {
            result = this.OnTargetVelocitySchedule();
         }

         return (result);
      }

      private SDOUpload CreateSdoUpload(int location)
      {
         UInt16 index = (UInt16)((location >> 8) & 0xFFFF);
         byte subIndex = (byte)(location & 0xFF);
         SDOUpload result = new SDOUpload(index, subIndex);
         return (result);
      }

      private bool ExchangeSdoDownload(int location, byte length, UInt32 data)
      {
         bool result = false;

         if (0 != location)
         {
            UInt16 index = (UInt16)((location >> 8) & 0xFFFF);
            byte subIndex = (byte)(location & 0xFF);
            result = ExchangeCommAction(new SDODownload(index, subIndex, length, data));
         }

         return (result);
      }

      private bool SetControlWord(UInt16 controlWord)
      {
         bool result = true;

         result &= this.ExchangeSdoDownload(this.ControlWordLocation, 2, controlWord);

         if (false != result)
         {
            this.controlWord = controlWord;
         }

         return (result);
      }

      #endregion

      #region Definition Properties

      public string Name { set; get; }

      public CommExchangeHandler OnCommExchange { set; get; }
      public TargetPositionScheduleHandler OnTargetPositionSchedule { set; get; }
      public TargetVelocityScheduleHandler OnTargetVelocitySchedule { set; get; }

      public int ControlWordLocation { set; get; } // 0x6040
      public int SetOperationalModeLocation { set; get; } // 0x6060

      public int HomeOffsetLocation { set; get; } // 0x607C
      public int HomingMethodLocation { set; get; } // 0x6098
      public int HomingSwitchSpeedLocation { set; get; } // 0x6099-1
      public int HomingZeroSpeedLocation { set; get; } // 0x6099-2
      public int HomingAccelerationLocation { set; get; } // 0x609A

      public int TargetPositionLocation { set; get; } // 0x607A
      public int ProfileVelocityLocation { set; get; } // 0x6081
      public int ProfileAccelerationLocation { set; get; } // 0x6083
      public int ProfileDecelerationLocation { set; get; } // 0x6084

      public int ActualPositionLocation { set; get; } // 0x6064
      public int PositionWindowLocation { set; get; } // 0x6067
      public int PositionWindowTimeLocation { set; get; } // 0x6068
      public int PositionKpLocation { set; get; } // 0x60FB-01
      public int PositionKiLocation { set; get; } // 0x60FB-02
      public int PositionKdLocation { set; get; } // 0x60FB-03

      public int ActualCurrentLocation { set; get; } // 0x6078

      public int ActualVelocityLocation { set; get; } // 0x606C
      public int VelocityWindowLocation { set; get; } // 0x606D
      public int VelocityWindowTimeLocation { set; get; } // 0x606E
      public int VelocityThresholdLocation { set; get; } // 0x606F
      public int VelocityThresholdTimeLocation { set; get; } // 0x6070
      public int VelocityKpLocation { set; get; } // 0x60F9-01
      public int VelocityKiLocation { set; get; } // 0x60F9-02
      public int VelocityKdLocation { set; get; } // 0x60F9-03
      public int TargetVelocityLocation { set; get; } // 0x60FF

      #endregion  

      #region Operational Properties

      public Modes Mode
      {
         get
         {
            return (this.mode);
         }
      }

      public bool PositionAttained
      {
         get
         {
            return (this.positionAttained);
         }
      }

      public bool VelocityAttained
      {
         get
         {
            return (this.velocityAttained);
         }
      }

      public bool TargetPositionRelative
      {
         get
         {
            return (this.targetPositionRelative);
         }
      }

      public bool HomingAttained
      {
         get
         {
            return (this.homingAttained);
         }
      }

      public bool HomeDefined
      {
         get
         {
            return (this.homeDefined);
         }
      }

      public UInt16 Status { set; get; }
      public double Temperature { set; get; }

      public Int16 ActualCurrent { set; get; }
      public Int32 ActualVelocity { set; get; }
      public Int32 ActualPosition { set; get; }

      public Int16 TargetTorque { set; get; }
      public Int32 TargetVelocity { set; get; }
      public Int32 TargetPosition { set; get; }

      #endregion

      #region Constructor

      public MotorComponent()
      {
         this.Reset();
      }

      #endregion

      #region Access Methods

      public virtual void Initialize()
      {
         this.Reset();
      }

      public virtual void Reset()
      {
         this.positionAttained = false;
         this.velocityAttained = false;
         this.targetPositionRelative = false;
         this.homingAttained = false;
         this.homeDefined = false;

         this.controlWord = 0;
         this.mode = Modes.undefined;

         this.Status = 0;
         this.Temperature = 0.0;
      
         this.ActualCurrent = 0;
         this.ActualVelocity = 0;
         this.ActualPosition = 0;

         this.TargetTorque = 0;
         this.TargetVelocity = 0;
         this.TargetPosition = 0;
      }

      public void ProcessStatus()
      {
         bool operational = ((this.Status & 0x0004) != 0) ? true : false;

         if (false != operational)
         {
            this.homeDefined = ((this.Status & 0x0100) != 0) ? true : false;

            if (Modes.position == this.mode)
            {
               this.positionAttained = ((this.Status & 0x0400) != 0) ? true : false;
            }
            else if (Modes.homing == this.mode)
            {
               if (((this.controlWord & 0x0010) != 0) &&
                   ((this.Status & 0x0400) != 0))
               {
                  this.homingAttained = true;
               }
               else
               {
                  this.homingAttained = false;
               }
            }
            else if (Modes.velocity == this.mode)
            {
               this.velocityAttained = ((this.Status & 0x0400) != 0) ? true : false;
            }
         }
      }

      public bool SetMode(Modes mode)
      {
         bool result = true;

         if (mode != this.mode)
         {
            this.positionAttained = false;
            this.velocityAttained = false;
            this.velocityAttained = false;
         }

         if (Modes.off == mode)
         {
            result &= this.SetControlWord(0x0);
         }
         else if (Modes.position == mode)
         {
            int relativePositionBit = (false != this.TargetPositionRelative) ? 0x0040 : 0;

            result &= this.SetControlWord((UInt16)(0x6 | relativePositionBit));
            result &= this.SetControlWord((UInt16)(0x7 | relativePositionBit));
            result &= this.ExchangeSdoDownload(this.SetOperationalModeLocation, 1, 1);
            result &= this.SetControlWord((UInt16)(0xF | relativePositionBit));
         }
         else if (Modes.homing == mode)
         {
            result &= this.SetControlWord(0x6);
            result &= this.SetControlWord(0x7);
            result &= this.ExchangeSdoDownload(this.SetOperationalModeLocation, 1, 6);
            result &= this.SetControlWord(0xF);
         }
         else if (Modes.velocity == mode)
         {
            result &= this.SetControlWord(0x6);
            result &= this.SetControlWord(0x7);
            result &= this.ExchangeSdoDownload(this.SetOperationalModeLocation, 1, 3);
            result &= this.SetControlWord(0xF);
         }
         else
         {
            result = false;
         }

         if (false != result)
         {
            this.mode = mode;
         }

         return (result);
      }

      public bool ClearFault()
      {
         bool result = true;

         this.positionAttained = false;
         this.velocityAttained = false;

         result &= this.SetControlWord(0x80);
         result &= this.SetControlWord(0x0);

         if (false != result)
         {
            this.mode = Modes.off;
         }

         return (result);
      }

      public bool GetActualPosition(ref Int32 actualPosition)
      {
         bool result = false;
         SDOUpload upload = this.CreateSdoUpload(this.ActualPositionLocation);
         bool actionResult = this.ExchangeCommAction(upload);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            actualPosition = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool GetActualVelocity(ref Int32 actualPosition)
      {
         bool result = false;
         SDOUpload upload = this.CreateSdoUpload(this.ActualVelocityLocation);
         bool actionResult = this.ExchangeCommAction(upload);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            actualPosition = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool GetActualCurrent(ref Int16 actualPosition)
      {
         bool result = false;
         SDOUpload upload = this.CreateSdoUpload(this.ActualCurrentLocation);
         bool actionResult = this.ExchangeCommAction(upload);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 2))
         {
            actualPosition = BitConverter.ToInt16(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool GetTargetPosition(ref Int32 targetPosition, ref bool positionRelative)
      {
         bool result = false;
         SDOUpload upload = this.CreateSdoUpload(this.TargetPositionLocation);
         bool actionResult = this.ExchangeCommAction(upload);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            targetPosition = BitConverter.ToInt32(upload.Data, 0);
            positionRelative = this.targetPositionRelative;
            this.TargetPosition = targetPosition;
            result = true;
         }

         return (result);
      }

      public bool SetTargetPosition(Int32 targetPosition, bool positionRelative)
      {
         bool result = true;

         if (positionRelative != this.targetPositionRelative)
         {
            UInt16 controlWord = this.controlWord;

            if (false != positionRelative)
            {
               controlWord |= 0x0040;
            }
            else
            {
               controlWord &= 0xFFBF;
            }

            result &= this.SetControlWord(controlWord);
         }

         result &= this.ExchangeSdoDownload(this.TargetPositionLocation, 4, (UInt32)targetPosition);

         if (false != result)
         {
            this.TargetPosition = targetPosition;
            this.targetPositionRelative = positionRelative;
         }

         return (result);
      }

      public bool ScheduleTargetPosition(Int32 targetPosition)
      {
         bool result = true;

         this.TargetPosition = targetPosition;
         result = this.ScheduleTargetPosition();

         return (result);
      }

      public bool GetProfileVelocity(ref Int32 profileVelocity)
      {
         bool result = false;
         SDOUpload upload = this.CreateSdoUpload(this.ProfileVelocityLocation);
         bool actionResult = this.ExchangeCommAction(upload);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            profileVelocity = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetProfileVelocity(Int32 profileVelocity)
      {
         bool result = true;

         result &= this.ExchangeSdoDownload(this.ProfileVelocityLocation, 4, (UInt32)profileVelocity);

         return (result);
      }

      public bool GetTargetVelocity(ref Int32 targetVelocity)
      {
         bool result = false;
         SDOUpload upload = this.CreateSdoUpload(this.TargetVelocityLocation);
         bool actionResult = this.ExchangeCommAction(upload);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            targetVelocity = BitConverter.ToInt32(upload.Data, 0);
            this.TargetVelocity = targetVelocity;
            result = true;
         }

         return (result);
      }

      public bool SetTargetVelocity(Int32 targetVelocity)
      {
         bool result = true;

         result &= this.ExchangeSdoDownload(this.TargetVelocityLocation, 4, (UInt32)targetVelocity);

         if (false != result)
         {
            this.TargetVelocity = targetVelocity;
         }

         return (result);
      }

      public bool ScheduleTargetVelocity(Int32 targetVelocity)
      {
         bool result = true;

         this.TargetVelocity = targetVelocity;
         result = this.ScheduleTargetVelocity();

         return (result);
      }

      public bool GetProfileAcceleration(ref Int32 profileAcceleration)
      {
         bool result = false;
         SDOUpload upload = this.CreateSdoUpload(this.ProfileAccelerationLocation);
         bool actionResult = this.ExchangeCommAction(upload);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            profileAcceleration = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetProfileAcceleration(Int32 profileAcceleration)
      {
         bool result = true;

         result &= this.ExchangeSdoDownload(this.ProfileAccelerationLocation, 4, (UInt32)profileAcceleration);

         return (result);
      }

      public bool GetProfileDeceleration(ref Int32 profileDeceleration)
      {
         bool result = false;
         SDOUpload upload = this.CreateSdoUpload(this.ProfileDecelerationLocation);
         bool actionResult = this.ExchangeCommAction(upload);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            profileDeceleration = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetProfileDeceleration(Int32 profileDeceleration)
      {
         bool result = true;

         result &= this.ExchangeSdoDownload(this.ProfileDecelerationLocation, 4, (UInt32)profileDeceleration);

         return (result);
      }

      public bool GetVelocityKp(ref Int32 motorKp)
      {
         bool result = false;
         SDOUpload upload = this.CreateSdoUpload(this.VelocityKpLocation);
         bool actionResult = this.ExchangeCommAction(upload);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            motorKp = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetVelocityKp(Int32 motorKp)
      {
         bool result = true;

         result &= this.ExchangeSdoDownload(this.VelocityKpLocation, 4, (UInt32)motorKp);

         return (result);
      }

      public bool GetVelocityKi(ref Int32 motorKi)
      {
         bool result = false;
         SDOUpload upload = this.CreateSdoUpload(this.VelocityKiLocation);
         bool actionResult = this.ExchangeCommAction(upload);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            motorKi = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetVelocityKi(Int32 motorKi)
      {
         bool result = true;

         result &= this.ExchangeSdoDownload(this.VelocityKiLocation, 4, (UInt32)motorKi);

         return (result);
      }

      public bool GetVelocityKd(ref Int32 motorKd)
      {
         bool result = false;
         SDOUpload upload = this.CreateSdoUpload(this.VelocityKdLocation);
         bool actionResult = this.ExchangeCommAction(upload);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            motorKd = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetVelocityKd(Int32 motorKd)
      {
         bool result = true;

         result &= this.ExchangeSdoDownload(this.VelocityKdLocation, 4, (UInt32)motorKd);

         return (result);
      }

      public bool GetPositionKp(ref Int32 motorKp)
      {
         bool result = false;
         SDOUpload upload = this.CreateSdoUpload(this.PositionKpLocation);
         bool actionResult = this.ExchangeCommAction(upload);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            motorKp = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetPositionKp(Int32 motorKp)
      {
         bool result = true;

         result &= this.ExchangeSdoDownload(this.PositionKpLocation, 4, (UInt32)motorKp);

         return (result);
      }

      public bool GetPositionKi(ref Int32 motorKi)
      {
         bool result = false;
         SDOUpload upload = this.CreateSdoUpload(this.PositionKiLocation);
         bool actionResult = this.ExchangeCommAction(upload);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            motorKi = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetPositionKi(Int32 motorKi)
      {
         bool result = true;

         result &= this.ExchangeSdoDownload(this.PositionKiLocation, 4, (UInt32)motorKi);

         return (result);
      }

      public bool GetPositionKd(ref Int32 motorKd)
      {
         bool result = false;
         SDOUpload upload = this.CreateSdoUpload(this.PositionKdLocation);
         bool actionResult = this.ExchangeCommAction(upload);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            motorKd = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetPositionKd(Int32 motorKd)
      {
         bool result = true;

         result &= this.ExchangeSdoDownload(this.PositionKdLocation, 4, (UInt32)motorKd);

         return (result);
      }
     
      public bool GetPositionWindow(ref UInt32 positionWindow)
      {
         bool result = false;
         SDOUpload upload = this.CreateSdoUpload(this.PositionWindowLocation);
         bool actionResult = this.ExchangeCommAction(upload);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            positionWindow = BitConverter.ToUInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetPositionWindow(UInt32 positionWindow)
      {
         bool result = true;

         result &= this.ExchangeSdoDownload(this.PositionWindowLocation, 4, (UInt32)positionWindow);

         return (result);
      }

      public bool GetPositionWindowTime(ref UInt16 positionWindowTime)
      {
         bool result = false;
         SDOUpload upload = this.CreateSdoUpload(this.PositionWindowTimeLocation);
         bool actionResult = this.ExchangeCommAction(upload);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 2))
         {
            positionWindowTime = BitConverter.ToUInt16(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetPositionWindowTime(UInt16 positionWindowTime)
      {
         bool result = true;

         result &= this.ExchangeSdoDownload(this.PositionWindowTimeLocation, 2, (UInt32)positionWindowTime);

         return (result);
      }

      public bool GetVelocityWindow(ref UInt16 velocityWindow)
      {
         bool result = false;
         SDOUpload upload = this.CreateSdoUpload(this.VelocityWindowLocation);
         bool actionResult = this.ExchangeCommAction(upload);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 2))
         {
            velocityWindow = BitConverter.ToUInt16(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetVelocityWindow(UInt16 velocityWindow)
      {
         bool result = true;

         result &= this.ExchangeSdoDownload(this.VelocityWindowLocation, 2, (UInt32)velocityWindow);

         return (result);
      }

      public bool GetVelocityWindowTime(ref UInt16 velocityWindowTime)
      {
         bool result = false;
         SDOUpload upload = this.CreateSdoUpload(this.VelocityWindowTimeLocation);
         bool actionResult = this.ExchangeCommAction(upload);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 2))
         {
            velocityWindowTime = BitConverter.ToUInt16(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetVelocityWindowTime(UInt16 velocityWindowTime)
      {
         bool result = true;

         result &= this.ExchangeSdoDownload(this.VelocityWindowTimeLocation, 2, (UInt32)velocityWindowTime);

         return (result);
      }

      public bool GetVelocityThreshold(ref UInt16 velocityThreshold)
      {
         bool result = false;
         SDOUpload upload = this.CreateSdoUpload(this.VelocityThresholdLocation);
         bool actionResult = this.ExchangeCommAction(upload);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 2))
         {
            velocityThreshold = BitConverter.ToUInt16(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetVelocityThreshold(UInt16 velocityThreshold)
      {
         bool result = true;

         result &= this.ExchangeSdoDownload(this.VelocityThresholdLocation, 2, (UInt32)velocityThreshold);

         return (result);
      }

      public bool GetVelocityThresholdTime(ref UInt16 velocityThresholdTime)
      {
         bool result = false;
         SDOUpload upload = this.CreateSdoUpload(this.VelocityThresholdTimeLocation);
         bool actionResult = this.ExchangeCommAction(upload);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 2))
         {
            velocityThresholdTime = BitConverter.ToUInt16(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetVelocityThresholdTime(UInt16 velocityThresholdTime)
      {
         bool result = true;

         result &= this.ExchangeSdoDownload(this.VelocityThresholdTimeLocation, 2, (UInt32)velocityThresholdTime);

         return (result);
      }

      public bool StartHoming()
      {
         bool result = true;

         if (Modes.homing == this.Mode)
         {
            UInt16 controlWord = this.controlWord;
            controlWord |= 0x0010;
            result &= this.SetControlWord(controlWord);

            if (false != result)
            {
               this.homingAttained = false;
            }
         }
         else
         {
            result = false;
         }

         return (result);
      }

      public bool StopHoming()
      {
         bool result = true;

         if (Modes.homing == this.Mode)
         {
            UInt16 controlWord = this.controlWord;
            controlWord &= 0xFFEF;
            result &= this.SetControlWord(controlWord);
         }
         else
         {
            result = false;
         }

         return (result);
      }

      public bool HaltHoming()
      {
         bool result = true;

         if (Modes.homing == this.Mode)
         {
            UInt16 controlWord = this.controlWord;
            controlWord |= 0x0100;
            result &= this.SetControlWord(controlWord);
         }
         else
         {
            result = false;
         }

         return (result);
      }

      public bool RunHoming()
      {
         bool result = true;

         if (Modes.homing == this.Mode)
         {
            UInt16 controlWord = this.controlWord;
            controlWord &= 0xFEFF;
            result &= this.SetControlWord(controlWord);
         }
         else
         {
            result = false;
         }

         return (result);
      }

      public bool GetHomingMethod(ref byte homingMethod)
      {
         bool result = false;
         SDOUpload upload = this.CreateSdoUpload(this.HomingMethodLocation);
         bool actionResult = this.ExchangeCommAction(upload);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 1))
         {
            homingMethod = upload.Data[0];
            result = true;
         }

         return (result);
      }

      public bool SetHomingMethod(byte homingMethod)
      {
         bool result = true;

         result &= this.ExchangeSdoDownload(this.HomingMethodLocation, 1, (UInt32)homingMethod);

         return (result);
      }

      public bool GetHomingSwitchSpeed(ref UInt32 homingSwitchSpeed)
      {
         bool result = false;
         SDOUpload upload = this.CreateSdoUpload(this.HomingSwitchSpeedLocation);
         bool actionResult = this.ExchangeCommAction(upload);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            homingSwitchSpeed = BitConverter.ToUInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetHomingSwitchSpeed(UInt32 homingSwitchSpeed)
      {
         bool result = true;

         result &= this.ExchangeSdoDownload(this.HomingSwitchSpeedLocation, 4, (UInt32)homingSwitchSpeed);

         return (result);
      }

      public bool GetHomingZeroSpeed(ref UInt32 homingZeroSpeed)
      {
         bool result = false;
         SDOUpload upload = this.CreateSdoUpload(this.HomingZeroSpeedLocation);
         bool actionResult = this.ExchangeCommAction(upload);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            homingZeroSpeed = BitConverter.ToUInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetHomingZeroSpeed(UInt32 homingZeroSpeed)
      {
         bool result = true;

         result &= this.ExchangeSdoDownload(this.HomingZeroSpeedLocation, 4, (UInt32)homingZeroSpeed);

         return (result);
      }

      public bool GetHomingAcceleration(ref UInt32 homingAcceleration)
      {
         bool result = false;
         SDOUpload upload = this.CreateSdoUpload(this.HomingAccelerationLocation);
         bool actionResult = this.ExchangeCommAction(upload);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            homingAcceleration = BitConverter.ToUInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetHomingAcceleration(UInt32 homingAcceleration)
      {
         bool result = true;

         result &= this.ExchangeSdoDownload(this.HomingAccelerationLocation, 4, (UInt32)homingAcceleration);

         return (result);
      }

      public bool GetHomeOffset(ref Int32 homeOffset)
      {
         bool result = false;
         SDOUpload upload = this.CreateSdoUpload(this.HomeOffsetLocation);
         bool actionResult = this.ExchangeCommAction(upload);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            homeOffset = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetHomeOffset(Int32 homeOffset)
      {
         bool result = true;

         result &= this.ExchangeSdoDownload(this.HomeOffsetLocation, 4, (UInt32)homeOffset);

         return (result);
      }

      #endregion

   }
}
