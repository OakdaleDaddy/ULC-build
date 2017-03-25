
namespace E4.CAN
{
   using System;
   using System.Text;
   using System.Threading;

   using E4.Utilities;

   public class UlcRoboticsE4Main : Device
   {
      #region Definition

      public enum UsageModes
      {
         undefined,
         laser,
         target,
      }

      public enum MotorModes
      {
         off,
         velocity,
         position,
         homing,
         undefined,
      }

      #endregion

      #region Fields

      private bool active;
      private UsageModes usageMode;

      private MotorComponent bldc0;
      private MotorComponent bldc1;
      private MotorComponent stepper0;
      private MotorComponent stepper1;

      private UInt32[] laserDistanceMeasurements;
      private byte laserDistanceMeasurementCount;

      #endregion

      #region Helper Functions

      private double GetSignedTemperature(byte reading)
      {
         double result = (reading < 127) ? reading : -256 + reading;
         return (result);
      }

      private bool EmitRpo1()
      {
         byte[] bldc0TargetTorqueData = BitConverter.GetBytes(this.Bldc0.TargetTorque);
         byte[] bldc1TargetTorqueData = BitConverter.GetBytes(this.Bldc1.TargetTorque);

         byte[] pdoData = new byte[bldc0TargetTorqueData.Length + bldc1TargetTorqueData.Length];
         int index = 0;

         for (int i = 0; i < bldc0TargetTorqueData.Length; i++)
         {
            pdoData[index++] = bldc0TargetTorqueData[i];
         }

         for (int i = 0; i < bldc1TargetTorqueData.Length; i++)
         {
            pdoData[index++] = bldc1TargetTorqueData[i];
         }

         bool result = this.ExchangeCommAction(new PDO1Emit(pdoData));
         return (result);
      }

      private bool EmitRpo2()
      {
         byte[] bldc0TargetVelocityData = BitConverter.GetBytes(this.Bldc0.TargetVelocity);
         byte[] bldc1TargetVelocityData = BitConverter.GetBytes(this.Bldc1.TargetVelocity);

         byte[] pdoData = new byte[bldc0TargetVelocityData.Length + bldc1TargetVelocityData.Length];
         int index = 0;

         for (int i = 0; i < bldc0TargetVelocityData.Length; i++)
         {
            pdoData[index++] = bldc0TargetVelocityData[i];
         }

         for (int i = 0; i < bldc1TargetVelocityData.Length; i++)
         {
            pdoData[index++] = bldc1TargetVelocityData[i];
         }

         bool result = this.ExchangeCommAction(new PDO2Emit(pdoData));
         return (result);
      }

      private bool EmitRpo3()
      {
         byte[] bldc0TargetPositionData = BitConverter.GetBytes(this.Bldc0.TargetPosition);
         byte[] bldc1TargetPositionData = BitConverter.GetBytes(this.Bldc1.TargetPosition);

         byte[] pdoData = new byte[bldc0TargetPositionData.Length + bldc1TargetPositionData.Length];
         int index = 0;

         for (int i = 0; i < bldc0TargetPositionData.Length; i++)
         {
            pdoData[index++] = bldc0TargetPositionData[i];
         }

         for (int i = 0; i < bldc1TargetPositionData.Length; i++)
         {
            pdoData[index++] = bldc1TargetPositionData[i];
         }

         bool result = this.ExchangeCommAction(new PDO3Emit(pdoData));
         return (result);
      }

      #endregion

      #region Properties

      public MotorComponent Bldc0
      {
         get
         {
            if (null == this.bldc0)
            {
               this.bldc0 = new MotorComponent();
            }

            return (this.bldc0);
         }
      }

      public MotorComponent Bldc1
      {
         get
         {
            if (null == this.bldc1)
            {
               this.bldc1 = new MotorComponent();
            }

            return (this.bldc1);
         }
      }

      public MotorComponent Stepper0
      {
         get
         {
            if (null == this.stepper0)
            {
               this.stepper0 = new MotorComponent();
            }

            return (this.stepper0);
         }
      }

      public MotorComponent Stepper1
      {
         get
         {
            if (null == this.stepper1)
            {
               this.stepper1 = new MotorComponent();
            }

            return (this.stepper1);
         }
      }

      #region General

      public double MainBoardImuRoll { set; get; }
      public double MainBoardImuPitch { set; get; }
      public double MainBoardImuYaw { set; get; }
      public double TargetBoardImuRoll { set; get; }
      public double TargetBoardImuPitch { set; get; }
      public double TargetBoardImuYaw { set; get; }
      public double McuTemperature { set; get; }
      public byte DcLinkVoltage { set; get; }

      public byte LaserStatusByte { set; get; }
      public byte LaserSampleNumber { set; get; }
      public UInt32 LaserMeasuredDistance { set; get; }
      public byte LaserScannerPosition { set; get; }

      public bool LaserMeasurementActivity
      {
         get
         {
            bool result = ((this.LaserStatusByte & 0x02) != 0) ? true : false;
            return (result);
         }
      }

      public bool LaserMeasurementReady
      {
         get
         {
            bool result = ((this.LaserStatusByte & 0x01) != 0) ? true : false;
            return (result);
         }
      }

      #endregion

      #endregion

      #region Overrides

      protected override void EvaluateMessage(int cobId, byte[] msg)
      {
         COBTypes frameType = (COBTypes)((cobId >> 7) & 0xF);
         int nodeId = (int)(cobId & 0x7F);

         if (nodeId == this.NodeId)
         {
            if (COBTypes.EMGY == frameType)
            {
               UInt64 errorValue = 0;
               byte[] errorMsg = new byte[8];

               if (null != msg)
               {
                  for (int i = 0; i < msg.Length; i++)
                  {
                     errorMsg[i] = msg[i];
                  }

                  for (int i = msg.Length; i < 8; i++)
                  {
                     errorMsg[i] = 0;
                  }

                  errorValue = BitConverter.ToUInt64(errorMsg, 0);
               }

               if (0 != errorValue)
               {
                  UInt16 errorCode = (UInt16)errorValue;
                  bool warning = false;
                  string reason = string.Format("emergency {0:X16}", errorValue);

                  // define strings here

                  if (false == warning)
                  {
                     this.SetFault(reason, false);
                  }
                  else
                  {
                     this.SetWarning(reason);
                  }
               }
               else
               {
                  this.ClearFault();
                  this.ClearWarning();
               }
            }
            else if (COBTypes.TPDO1 == frameType)
            {
               if (UsageModes.laser == this.usageMode)
               {
                  if ((null != msg) && (msg.Length >= 8))
                  {
                     this.Bldc0.Status = BitConverter.ToUInt16(msg, 0);
                     this.Bldc0.ActualCurrent = BitConverter.ToInt16(msg, 2);
                     this.Bldc1.Status = BitConverter.ToUInt16(msg, 4);
                     this.Bldc1.ActualCurrent = BitConverter.ToInt16(msg, 6);

                     this.Bldc0.ProcessStatus();
                     this.Bldc1.ProcessStatus();
                  }
               }
               else if (UsageModes.target == this.usageMode)
               {
                  if ((null != msg) && (msg.Length >= 8))
                  {
                     this.Bldc0.Status = BitConverter.ToUInt16(msg, 0);
                     this.Bldc0.ActualCurrent = BitConverter.ToInt16(msg, 2);
                     this.Bldc1.Status = BitConverter.ToUInt16(msg, 4);
                     this.Bldc1.ActualCurrent = BitConverter.ToInt16(msg, 6);

                     this.Bldc0.ProcessStatus();
                     this.Bldc1.ProcessStatus();
                  }
               }
            }
            else if (COBTypes.TPDO2 == frameType)
            {
               if (UsageModes.laser == this.usageMode)
               {
                  if ((null != msg) && (msg.Length >= 8))
                  {
                     this.Bldc0.ActualVelocity = BitConverter.ToInt32(msg, 0);
                     this.Bldc1.ActualVelocity = BitConverter.ToInt32(msg, 4);
                  }
               }
               else if (UsageModes.target == this.usageMode)
               {
                  if ((null != msg) && (msg.Length >= 8))
                  {
                     this.Bldc0.ActualVelocity = BitConverter.ToInt32(msg, 0);
                     this.Bldc1.ActualVelocity = BitConverter.ToInt32(msg, 4);
                  }
               }
            }
            else if (COBTypes.TPDO3 == frameType)
            {
               if (UsageModes.laser == this.usageMode)
               {
                  if ((null != msg) && (msg.Length >= 8))
                  {
                     this.Bldc0.ActualPosition = BitConverter.ToInt32(msg, 0);
                     this.Bldc1.ActualPosition = BitConverter.ToInt32(msg, 4);
                  }
               }
               else if (UsageModes.target == this.usageMode)
               {
                  if ((null != msg) && (msg.Length >= 8))
                  {
                     this.Bldc0.ActualPosition = BitConverter.ToInt32(msg, 0);
                     this.Bldc1.ActualPosition = BitConverter.ToInt32(msg, 4);
                  }
               }
            }
            else if (COBTypes.TPDO4 == frameType)
            {
               if (UsageModes.laser == this.usageMode)
               {
                  if ((null != msg) && (msg.Length >= 3))
                  {
                     this.Bldc0.Temperature = this.GetSignedTemperature(msg[0]);
                     this.Bldc1.Temperature = this.GetSignedTemperature(msg[1]);
                     this.DcLinkVoltage = msg[2];
                  }
               }
               else if (UsageModes.target == this.usageMode)
               {
                  if ((null != msg) && (msg.Length >= 3))
                  {
                     this.Bldc0.Temperature = this.GetSignedTemperature(msg[0]);
                     this.Bldc1.Temperature = this.GetSignedTemperature(msg[1]);
                     this.DcLinkVoltage = msg[2];
                  }
               }
            }
         }
         else if (nodeId == (this.NodeId + 1))
         {
            if (COBTypes.TPDO1 == frameType) // TPDO5
            {
               if (UsageModes.laser == this.usageMode)
               {
                  if ((null != msg) && (msg.Length >= 7))
                  {
                     this.MainBoardImuRoll = ((double)BitConverter.ToInt16(msg, 0) / 16);
                     this.MainBoardImuPitch = ((double)BitConverter.ToInt16(msg, 2) / 16);
                     this.MainBoardImuYaw = ((double)BitConverter.ToInt16(msg, 4) / 16);
                     this.McuTemperature = this.GetSignedTemperature(msg[6]);
                  }
               }
               else if (UsageModes.target == this.usageMode)
               {
                  if ((null != msg) && (msg.Length >= 7))
                  {
                     this.MainBoardImuRoll = ((double)BitConverter.ToInt16(msg, 0) / 16);
                     this.MainBoardImuPitch = ((double)BitConverter.ToInt16(msg, 2) / 16);
                     this.MainBoardImuYaw = ((double)BitConverter.ToInt16(msg, 4) / 16);
                     this.McuTemperature = this.GetSignedTemperature(msg[6]);
                  }
               }
            }
            else if (COBTypes.TPDO2 == frameType) // TPDO6
            {
               if (UsageModes.laser == this.usageMode)
               {
                  if ((null != msg) && (msg.Length >= 6))
                  {
                     this.LaserStatusByte = msg[0];
                     this.LaserSampleNumber = msg[1];
                     UInt32 measuredSample = BitConverter.ToUInt32(msg, 2);

                     int storageIndex = this.LaserSampleNumber - 1;

                     if ((storageIndex > 0) && (storageIndex < this.laserDistanceMeasurements.Length))
                     {
                        this.laserDistanceMeasurements[storageIndex] = measuredSample;
                     }

                     this.LaserMeasuredDistance = measuredSample;                  
                  }
               }
               else if (UsageModes.target == this.usageMode)
               {
                  if ((null != msg) && (msg.Length >= 7))
                  {
                     this.TargetBoardImuRoll = ((double)BitConverter.ToInt16(msg, 0) / 16);
                     this.TargetBoardImuPitch = ((double)BitConverter.ToInt16(msg, 2) / 16);
                     this.TargetBoardImuYaw = ((double)BitConverter.ToInt16(msg, 4) / 16);
                     this.LaserScannerPosition = msg[6];
                  }
               }
            }
            else if (COBTypes.TPDO3 == frameType) // TPDO7
            {
               if (UsageModes.laser == this.usageMode)
               {
                  if ((null != msg) && (msg.Length >= 4))
                  {
                     this.Stepper0.Status = BitConverter.ToUInt16(msg, 0);
                     this.Stepper1.Status = BitConverter.ToUInt16(msg, 2);

                     this.Stepper0.ProcessStatus();
                     this.Stepper1.ProcessStatus();
                  }
               }
               else if (UsageModes.target == this.usageMode)
               {
                  if ((null != msg) && (msg.Length >= 2))
                  {
                     this.Stepper0.Status = BitConverter.ToUInt16(msg, 0);
                     this.Stepper0.ProcessStatus();
                  }
               }
            }
            else if (COBTypes.TPDO4 == frameType) // TPDO8
            {
               if ((null != msg) && (msg.Length >= 8))
               {
               }
            }
         }
      }

      protected override void EvaluateAction(CommAction action)
      {
         base.EvaluateAction(action);
      }

      #endregion

      #region Constructor

      public UlcRoboticsE4Main(string name, byte nodeId)
         : base(name, nodeId)
      {
         #region BLDC0 Initialization

         this.Bldc0.Name = "bldc0";

         this.Bldc0.OnCommExchange = new MotorComponent.CommExchangeHandler(this.ExchangeCommAction);
         this.Bldc0.OnTargetPositionSchedule = new MotorComponent.TargetPositionScheduleHandler(this.EmitRpo3);
         this.Bldc0.OnTargetVelocitySchedule = new MotorComponent.TargetVelocityScheduleHandler(this.EmitRpo2);

         this.Bldc0.ControlWordLocation = 0x00604000;
         this.Bldc0.SetOperationalModeLocation = 0x00606000;

         this.Bldc0.TargetPositionLocation = 0x00607A00;
         this.Bldc0.ProfileVelocityLocation = 0x00608100;
         this.Bldc0.ProfileAccelerationLocation = 0x00608300;
         this.Bldc0.ProfileDecelerationLocation = 0x00608400;

         this.Bldc0.PositionWindowLocation = 0x00606700;
         this.Bldc0.PositionWindowTimeLocation = 0x00606800;
         this.Bldc0.PositionKpLocation = 0x0060FB01;
         this.Bldc0.PositionKiLocation = 0x0060FB02;
         this.Bldc0.PositionKdLocation = 0x0060FB03;

         this.Bldc0.VelocityWindowLocation = 0x00606D00;
         this.Bldc0.VelocityWindowTimeLocation = 0x00606E00;
         this.Bldc0.VelocityThresholdLocation = 0x00606F00;
         this.Bldc0.VelocityThresholdTimeLocation = 0x00607000;
         this.Bldc0.VelocityKpLocation = 0x0060F901;
         this.Bldc0.VelocityKiLocation = 0x0060F902;
         this.Bldc0.VelocityKdLocation = 0x0060F903;
         this.Bldc0.TargetVelocityLocation = 0x0060FF00;

         #endregion

         #region BLDC1 Initialization

         this.Bldc1.Name = "bldc1";

         this.Bldc1.OnCommExchange = new MotorComponent.CommExchangeHandler(this.ExchangeCommAction);
         this.Bldc1.OnTargetPositionSchedule = new MotorComponent.TargetPositionScheduleHandler(this.EmitRpo3);
         this.Bldc1.OnTargetVelocitySchedule = new MotorComponent.TargetVelocityScheduleHandler(this.EmitRpo2);

         this.Bldc1.ControlWordLocation = 0x00684000;
         this.Bldc1.SetOperationalModeLocation = 0x00686000;

         this.Bldc1.TargetPositionLocation = 0x00687A00;
         this.Bldc1.ProfileVelocityLocation = 0x00688100;
         this.Bldc1.ProfileAccelerationLocation = 0x00688300;
         this.Bldc1.ProfileDecelerationLocation = 0x00688400;

         this.Bldc1.PositionWindowLocation = 0x00686700;
         this.Bldc1.PositionWindowTimeLocation = 0x00686800;
         this.Bldc1.PositionKpLocation = 0x0068FB01;
         this.Bldc1.PositionKiLocation = 0x0068FB02;
         this.Bldc1.PositionKdLocation = 0x0068FB03;

         this.Bldc1.VelocityWindowLocation = 0x00686D00;
         this.Bldc1.VelocityWindowTimeLocation = 0x00686E00;
         this.Bldc1.VelocityThresholdLocation = 0x00686F00;
         this.Bldc1.VelocityThresholdTimeLocation = 0x00687000;
         this.Bldc1.VelocityKpLocation = 0x0068F901;
         this.Bldc1.VelocityKiLocation = 0x0068F902;
         this.Bldc1.VelocityKdLocation = 0x0068F903;
         this.Bldc1.TargetVelocityLocation = 0x0068FF00;

         #endregion

         #region Stepper0 Initialization

         this.Stepper0.Name = "stepper0";

         this.Stepper0.OnCommExchange = new MotorComponent.CommExchangeHandler(this.ExchangeCommAction);

         this.Stepper0.ControlWordLocation = 0x00704000;
         this.Stepper0.SetOperationalModeLocation = 0x00706000;

         this.Stepper0.HomeOffsetLocation = 0x00707C00;
         this.Stepper0.HomingMethodLocation = 0x00709800;
         this.Stepper0.HomingSwitchSpeedLocation = 0x00709901;
         this.Stepper0.HomingZeroSpeedLocation = 0x00709902;
         this.Stepper0.HomingAccelerationLocation = 0x00709A00;

         this.Stepper0.TargetPositionLocation = 0x00707A00;
         this.Stepper0.ProfileVelocityLocation = 0x00708100;
         this.Stepper0.ProfileAccelerationLocation = 0x00708300;
         this.Stepper0.ProfileDecelerationLocation = 0x00708400;

         this.Stepper0.ActualPositionLocation = 0x00706400;

         this.Stepper0.ActualCurrentLocation = 0x00707800;

         this.Stepper0.ActualVelocityLocation = 0x00706C00;
         this.Stepper0.TargetVelocityLocation = 0x0070FF00;

         #endregion

         #region Stepper1 Initialization

         this.Stepper1.Name = "stepper1";

         this.Stepper1.OnCommExchange = new MotorComponent.CommExchangeHandler(this.ExchangeCommAction);

         this.Stepper1.ControlWordLocation = 0x00784000;
         this.Stepper1.SetOperationalModeLocation = 0x00786000;

         this.Stepper1.HomeOffsetLocation = 0x00787C00;
         this.Stepper1.HomingMethodLocation = 0x00789800;
         this.Stepper1.HomingSwitchSpeedLocation = 0x00789901;
         this.Stepper1.HomingZeroSpeedLocation = 0x00789902;
         this.Stepper1.HomingAccelerationLocation = 0x00789A00;

         this.Stepper1.TargetPositionLocation = 0x00787A00;
         this.Stepper1.ProfileVelocityLocation = 0x00788100;
         this.Stepper1.ProfileAccelerationLocation = 0x00788300;
         this.Stepper1.ProfileDecelerationLocation = 0x00788400;

         this.Stepper1.ActualPositionLocation = 0x00786400;

         this.Stepper1.ActualCurrentLocation = 0x00787800;

         this.Stepper1.ActualVelocityLocation = 0x00786C00;
         this.Stepper1.TargetVelocityLocation = 0x0078FF00;

         #endregion

         this.laserDistanceMeasurements = new UInt32[128];
      }

      #endregion

      #region Access Methods

      #region General Functions

      public override void Initialize()
      {
         this.usageMode = UsageModes.undefined;
         base.Initialize();
      }

      public bool Configure(UsageModes usage)
      {
         bool result = false;

         if (null == this.FaultReason)
         {
            result = true;
            this.usageMode = usage;

            if (UsageModes.laser == this.usageMode)
            {
               // set TPDO1 on change with 200mS inhibit time
               result &= this.SetTPDOEnable(1, false);
               result &= this.SetTPDOMapCount(1, 0);
               result &= this.SetTPDOType(1, 254);
               result &= this.SetTPDOInhibitTime(1, 200);
               result &= this.SetTPDOMap(1, 1, 0x6041, 0x00, 2); // BLDC0 status word
               result &= this.SetTPDOMap(1, 2, 0x6078, 0x00, 2); // BLDC0 current actual
               result &= this.SetTPDOMap(1, 3, 0x6841, 0x00, 2); // BLDC1 status word
               result &= this.SetTPDOMap(1, 4, 0x6878, 0x00, 2); // BLDC1 current actual
               result &= this.SetTPDOMapCount(1, 4);
               result &= this.SetTPDOEnable(1, true);

               // set TPDO2 on change with 200mS inhibit time
               result &= this.SetTPDOEnable(2, false);
               result &= this.SetTPDOMapCount(2, 0);
               result &= this.SetTPDOType(2, 254);
               result &= this.SetTPDOInhibitTime(2, 200);
               result &= this.SetTPDOMap(2, 1, 0x606C, 0x00, 4); // BLDC0 velocity actual value
               result &= this.SetTPDOMap(2, 2, 0x686C, 0x00, 4); // BLDC1 velocity actual value
               result &= this.SetTPDOMapCount(2, 2);
               result &= this.SetTPDOEnable(2, true);

               // set TPDO3 on change with 200mS inhibit time
               result &= this.SetTPDOEnable(3, false);
               result &= this.SetTPDOMapCount(3, 0);
               result &= this.SetTPDOType(3, 254);
               result &= this.SetTPDOInhibitTime(3, 200);
               result &= this.SetTPDOMap(3, 1, 0x6064, 0x00, 4); // BLDC0 position actual value
               result &= this.SetTPDOMap(3, 2, 0x6864, 0x00, 4); // BLDC1 position actual value
               result &= this.SetTPDOMapCount(3, 2);
               result &= this.SetTPDOEnable(3, true);

               // set TPDO4 on change with 200mS inhibit time
               result &= this.SetTPDOEnable(4, false);
               result &= this.SetTPDOMapCount(4, 0);
               result &= this.SetTPDOType(4, 254);
               result &= this.SetTPDOInhibitTime(4, 200);
               result &= this.SetTPDOMap(4, 1, 0x6410, 0x01, 1); // BLDC0 temperature
               result &= this.SetTPDOMap(4, 2, 0x6C10, 0x01, 1); // BLDC1 temperature
               result &= this.SetTPDOMap(4, 3, 0x2000, 0x00, 1); // DC Link Voltage
               result &= this.SetTPDOMapCount(4, 3);
               result &= this.SetTPDOEnable(4, true);

               // set TPDO5 on change with 200mS inhibit time
               result &= this.SetTPDOEnable(5, false);
               result &= this.SetTPDOMapCount(5, 0);
               result &= this.SetTPDOType(5, 254);
               result &= this.SetTPDOInhibitTime(5, 200);
               result &= this.SetTPDOMap(5, 1, 0x2441, 0x01, 2); // Main Board IMU roll
               result &= this.SetTPDOMap(5, 2, 0x2441, 0x02, 2); // Main Board IMU pitch
               result &= this.SetTPDOMap(5, 3, 0x2441, 0x03, 2); // Main Board IMU yaw
               result &= this.SetTPDOMap(5, 4, 0x2311, 0x01, 1); // MCU temperature
               result &= this.SetTPDOMapCount(5, 4);
               result &= this.SetTPDOEnable(5, true);

               // set TPDO6 on change with 200mS inhibit time
               result &= this.SetTPDOEnable(6, false);
               result &= this.SetTPDOMapCount(6, 0);
               result &= this.SetTPDOType(6, 254);
               result &= this.SetTPDOInhibitTime(6, 200);
               result &= this.SetTPDOMap(6, 1, 0x2405, 0x00, 1); // laser status byte
               result &= this.SetTPDOMap(6, 2, 0x2402, 0x01, 1); // laser measure sample
               result &= this.SetTPDOMap(6, 3, 0x2402, 0x02, 4); // laser measured distance
               result &= this.SetTPDOMapCount(6, 3);
               result &= this.SetTPDOEnable(6, true);

               // set TPDO7 on change with 200mS inhibit time
               result &= this.SetTPDOEnable(7, false);
               result &= this.SetTPDOMapCount(7, 0);
               result &= this.SetTPDOType(7, 254);
               result &= this.SetTPDOInhibitTime(7, 200);
               result &= this.SetTPDOMap(7, 1, 0x7041, 0x00, 2); // stepper0 status word
               result &= this.SetTPDOMap(7, 2, 0x7841, 0x00, 2); // stepper1 status word
               result &= this.SetTPDOMapCount(7, 2);
               result &= this.SetTPDOEnable(7, true);


               // set RPDO1 every SYNC
               result &= this.SetRPDOEnable(1, false);
               result &= this.SetRPDOMapCount(1, 0);
               result &= this.SetRPDOType(1, 1);
               result &= this.SetRPDOMap(1, 1, 0x6071, 0, 2); // BLDC0 target torque
               result &= this.SetRPDOMap(1, 2, 0x6871, 0, 2); // BLDC1 target torque
               result &= this.SetRPDOMapCount(1, 2);
               result &= this.SetRPDOEnable(1, true);

               // set RPDO2 every SYNC
               result &= this.SetRPDOEnable(2, false);
               result &= this.SetRPDOMapCount(2, 0);
               result &= this.SetRPDOType(2, 1);
               result &= this.SetRPDOMap(2, 1, 0x60FF, 0, 4); // BLDC0 target velocity
               result &= this.SetRPDOMap(2, 2, 0x68FF, 0, 4); // BLDC1 target velocity
               result &= this.SetRPDOMapCount(2, 2);
               result &= this.SetRPDOEnable(2, true);

               // set RPDO3 every SYNC
               result &= this.SetRPDOEnable(3, false);
               result &= this.SetRPDOMapCount(3, 0);
               result &= this.SetRPDOType(3, 1);
               result &= this.SetRPDOMap(3, 1, 0x607A, 0, 4); // BLDC0 target position
               result &= this.SetRPDOMap(3, 2, 0x687A, 0, 4); // BLDC1 target position
               result &= this.SetRPDOMapCount(3, 2);
               result &= this.SetRPDOEnable(3, true);
            }
            else if (UsageModes.target == this.usageMode)
            {
               // set TPDO1 on change with 200mS inhibit time
               result &= this.SetTPDOEnable(1, false);
               result &= this.SetTPDOMapCount(1, 0);
               result &= this.SetTPDOType(1, 254);
               result &= this.SetTPDOInhibitTime(1, 200);
               result &= this.SetTPDOMap(1, 1, 0x6041, 0x00, 2); // BLDC0 status word
               result &= this.SetTPDOMap(1, 2, 0x6078, 0x00, 2); // BLDC0 current actual
               result &= this.SetTPDOMap(1, 3, 0x6841, 0x00, 2); // BLDC1 status word
               result &= this.SetTPDOMap(1, 4, 0x6878, 0x00, 2); // BLDC1 current actual
               result &= this.SetTPDOMapCount(1, 4);
               result &= this.SetTPDOEnable(1, true);

               // set TPDO2 on change with 200mS inhibit time
               result &= this.SetTPDOEnable(2, false);
               result &= this.SetTPDOMapCount(2, 0);
               result &= this.SetTPDOType(2, 254);
               result &= this.SetTPDOInhibitTime(2, 200);
               result &= this.SetTPDOMap(2, 1, 0x606C, 0x00, 4); // BLDC0 velocity actual value
               result &= this.SetTPDOMap(2, 2, 0x686C, 0x00, 4); // BLDC1 velocity actual value
               result &= this.SetTPDOMapCount(2, 2);
               result &= this.SetTPDOEnable(2, true);

               // set TPDO3 on change with 200mS inhibit time
               result &= this.SetTPDOEnable(3, false);
               result &= this.SetTPDOMapCount(3, 0);
               result &= this.SetTPDOType(3, 254);
               result &= this.SetTPDOInhibitTime(3, 200);
               result &= this.SetTPDOMap(3, 1, 0x6064, 0x00, 4); // BLDC0 position actual value
               result &= this.SetTPDOMap(3, 2, 0x6864, 0x00, 4); // BLDC1 position actual value
               result &= this.SetTPDOMapCount(3, 2);
               result &= this.SetTPDOEnable(3, true);

               // set TPDO4 on change with 200mS inhibit time
               result &= this.SetTPDOEnable(4, false);
               result &= this.SetTPDOMapCount(4, 0);
               result &= this.SetTPDOType(4, 254);
               result &= this.SetTPDOInhibitTime(4, 200);
               result &= this.SetTPDOMap(4, 1, 0x6410, 0x01, 1); // BLDC0 temperature
               result &= this.SetTPDOMap(4, 2, 0x6C10, 0x01, 1); // BLDC1 temperature
               result &= this.SetTPDOMap(4, 3, 0x2000, 0x00, 1); // DC Link Voltage
               result &= this.SetTPDOMapCount(4, 3);
               result &= this.SetTPDOEnable(4, true);

               // set TPDO5 on change with 200mS inhibit time
               result &= this.SetTPDOEnable(5, false);
               result &= this.SetTPDOMapCount(5, 0);
               result &= this.SetTPDOType(5, 254);
               result &= this.SetTPDOInhibitTime(5, 200);
               result &= this.SetTPDOMap(5, 1, 0x2441, 0x01, 2); // Main Board IMU roll
               result &= this.SetTPDOMap(5, 2, 0x2441, 0x02, 2); // Main Board IMU pitch
               result &= this.SetTPDOMap(5, 3, 0x2441, 0x03, 2); // Main Board IMU yaw
               result &= this.SetTPDOMap(5, 4, 0x2311, 0x01, 1); // MCU temperature
               result &= this.SetTPDOMapCount(5, 4);
               result &= this.SetTPDOEnable(5, true);

               // set TPDO6 on change with 200mS inhibit time
               result &= this.SetTPDOEnable(6, false);
               result &= this.SetTPDOMapCount(6, 0);
               result &= this.SetTPDOType(6, 254);
               result &= this.SetTPDOInhibitTime(6, 200);
               result &= this.SetTPDOMap(6, 1, 0x2445, 0x01, 2); // Target Board IMU roll
               result &= this.SetTPDOMap(6, 2, 0x2445, 0x02, 2); // Target Board IMU pitch
               result &= this.SetTPDOMap(6, 3, 0x2445, 0x03, 2); // Target Board IMU yaw
               result &= this.SetTPDOMap(6, 4, 0x2410, 0x01, 1); // laser scanner position
               result &= this.SetTPDOMapCount(6, 4);
               result &= this.SetTPDOEnable(6, true);

               // set TPDO7 on change with 200mS inhibit time
               result &= this.SetTPDOEnable(7, false);
               result &= this.SetTPDOMapCount(7, 0);
               result &= this.SetTPDOType(7, 254);
               result &= this.SetTPDOInhibitTime(7, 200);
               result &= this.SetTPDOMap(7, 1, 0x7041, 0x00, 2); // stepper0 status word
               result &= this.SetTPDOMapCount(7, 1);
               result &= this.SetTPDOEnable(7, true);


               // set RPDO1 every SYNC
               result &= this.SetRPDOEnable(1, false);
               result &= this.SetRPDOMapCount(1, 0);
               result &= this.SetRPDOType(1, 1);
               result &= this.SetRPDOMap(1, 1, 0x6071, 0, 2); // BLDC0 target torque
               result &= this.SetRPDOMap(1, 2, 0x6871, 0, 2); // BLDC1 target torque
               result &= this.SetRPDOMapCount(1, 2);
               result &= this.SetRPDOEnable(1, true);

               // set RPDO2 every SYNC
               result &= this.SetRPDOEnable(2, false);
               result &= this.SetRPDOMapCount(2, 0);
               result &= this.SetRPDOType(2, 1);
               result &= this.SetRPDOMap(2, 1, 0x60FF, 0, 4); // BLDC0 target velocity
               result &= this.SetRPDOMap(2, 2, 0x68FF, 0, 4); // BLDC1 target velocity
               result &= this.SetRPDOMapCount(2, 2);
               result &= this.SetRPDOEnable(2, true);

               // set RPDO3 every SYNC
               result &= this.SetRPDOEnable(3, false);
               result &= this.SetRPDOMapCount(3, 0);
               result &= this.SetRPDOType(3, 1);
               result &= this.SetRPDOMap(3, 1, 0x607A, 0, 4); // BLDC0 target position
               result &= this.SetRPDOMap(3, 2, 0x687A, 0, 4); // BLDC1 target position
               result &= this.SetRPDOMapCount(3, 2);
               result &= this.SetRPDOEnable(3, true);
            }
            else
            {
               this.SetFault("usage undefined", false);
               result = false;
            }

            result &= base.Configure();
         }

         return (result);
      }

      public override bool Start()
      {
         bool result = false;

         if (null == this.FaultReason)
         {
            result = base.Start();
            this.active = result;
         }

         return (result);
      }

      public override void Stop()
      {
         base.Stop();
         this.active = false;
      }

      public override void Reset()
      {
         base.Reset();

         this.active = false;

         this.Bldc0.Reset();
         this.Bldc1.Reset();
         this.Stepper0.Reset();
         this.Stepper1.Reset();

         for (int i = 0; i < this.laserDistanceMeasurements.Length; i++)
         {
            this.laserDistanceMeasurements[i] = 0;
         }

         laserDistanceMeasurementCount = 0;
      }

      public override void Update()
      {
         if (false != active)
         {
         }

         base.Update();
      }

      #endregion

      #region Camera Functions

      public bool GetCameraSelect(ref byte videoSelect)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x2301, 1);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 1))
         {
            videoSelect = upload.Data[0];
            result = true;
         }

         return (result);
      }

      public bool SetCameraSelect(byte select)
      {
         this.pendingAction = new SDODownload(0x2301, 1, 1, select);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      public bool GetCameraLedIntensityLevel(ref UInt32 ledIntensityLevel)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x2303, 0x01);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            ledIntensityLevel = BitConverter.ToUInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetCameraLedIntensityLevel(UInt32 ledIntensityLevel)
      {
         this.pendingAction = new SDODownload(0x2303, 0x01, 4, ledIntensityLevel);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      public bool GetCameraLedChannelMask(ref byte ledChannelMask)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x2304, 0x01);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 1))
         {
            ledChannelMask = upload.Data[0];
            result = true;
         }

         return (result);
      }

      public bool SetCameraLedChannelMask(byte ledChannelMask)
      {
         this.pendingAction = new SDODownload(0x2304, 0x01, 1, ledChannelMask);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      #endregion

      #region IMU Functions

      public bool GetMainBoardRoll(ref double roll)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x2441, 0x01);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 2))
         {
            roll = ((double)BitConverter.ToInt16(upload.Data, 0) / 16);
            result = true;
         }

         return (result);
      }

      public bool GetMainBoardPitch(ref double pitch)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x2441, 0x02);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 2))
         {
            pitch = ((double)BitConverter.ToInt16(upload.Data, 0) / 16);
            result = true;
         }

         return (result);
      }

      public bool GetMainBoardYaw(ref double yaw)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x2441, 0x03);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 2))
         {
            yaw = ((double)BitConverter.ToInt16(upload.Data, 0) / 16);
            result = true;
         }

         return (result);
      }

      public bool GetTargetBoardRoll(ref double roll)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x2445, 0x01);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 2))
         {
            roll = ((double)BitConverter.ToInt16(upload.Data, 0) / 16);
            result = true;
         }

         return (result);
      }

      public bool GetTargetBoardPitch(ref double pitch)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x2445, 0x02);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 2))
         {
            pitch = ((double)BitConverter.ToInt16(upload.Data, 0) / 16);
            result = true;
         }

         return (result);
      }

      public bool GetTargetBoardYaw(ref double yaw)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x2445, 0x03);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 2))
         {
            yaw = ((double)BitConverter.ToInt16(upload.Data, 0) / 16);
            result = true;
         }

         return (result);
      }

      #endregion

      #region Laser Range Finder Functions

      public bool SetLaserAimOn()
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x2400, 0, 1, (UInt32)1));

         return (result);
      }

      public bool SetLaserAimOff()
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x2400, 0, 1, (UInt32)0));

         return (result);
      }

      public bool GetLaserTimeToMeasure(ref byte timeToMeasure)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x2401, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 1))
         {
            timeToMeasure = upload.Data[0];
            result = true;
         }

         return (result);
      }

      public bool SetLaserTimeToMeasure(byte timeToMeasure)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x2401, 0, 1, (UInt32)timeToMeasure));

         return (result);
      }

      public bool GetLaserControlByte(ref byte controlByte)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x2404, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 1))
         {
            controlByte = upload.Data[0];
            result = true;
         }

         return (result);
      }

      public bool SetLaserControlByte(byte controlByte)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x2404, 0, 1, (UInt32)controlByte));

         return (result);
      }

      public bool GetLaserDistance(ref UInt32 distance)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x2402, 0x02);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            distance = BitConverter.ToUInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool GetLaserRangeFinderTemperature(ref byte temperature)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x2403, 0x01);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 1))
         {
            temperature = upload.Data[0];
            result = true;
         }

         return (result);
      }

      public bool StartLaserMeasurement(int sampleCount, double sampleTime)
      {
         bool result = true;
         byte timeToMeasureBits = (byte)(sampleTime / 0.100);
         byte sampleCountBits = (byte)((sampleCount > 0) ? (sampleCount - 1) : 0);
         byte controlValue = (byte)(0x80 | sampleCountBits);

         result &= this.SetLaserTimeToMeasure(timeToMeasureBits);
         result &= this.SetLaserControlByte(0x00);
         result &= this.SetLaserControlByte(controlValue);
         
         return (result);
      }

      public bool CancelLaserMeasurement()
      {
         bool result = this.SetLaserControlByte(0x00);

         return (result);
      }

      public UInt32 GetAverageLaserDistance()
      {
         UInt32 result = this.LaserMeasuredDistance;
         byte sampleCount = this.laserDistanceMeasurementCount;

         if (0 != sampleCount)
         {
            UInt64 total = 0;

            for (int i = 0; i < sampleCount; i++)
            {
               total += this.laserDistanceMeasurements[i];
            }

            result = (UInt32)(total / sampleCount);
         }

         return (result);
      }

      #endregion

      #region Laser Scanner Functions

      public bool GetLaserScannerPosition(ref byte psotion)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x2410, 0x01);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 1))
         {
            psotion = upload.Data[0];
            result = true;
         }

         return (result);
      }

      public bool GetLaserScannerTemperature(ref byte temperature)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x2411, 0x01);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 1))
         {
            temperature = upload.Data[0];
            result = true;
         }

         return (result);
      }

      #endregion

      #endregion
   }
}