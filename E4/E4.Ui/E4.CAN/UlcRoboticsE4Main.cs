
namespace E4.CAN
{
   using System;
   using System.Text;
   using System.Threading;

   using E4.Utilities;

   public class UlcRoboticsE4Main : Device
   {
      #region Definition

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

      private UInt16 bldc0ControlWord;
      private MotorModes bldc0Mode;
      private Int16 bldc0TargetTorque;
      private Int32 bldc0TargetVelocity;
      private Int32 bldc0TargetPosition;

      private UInt16 bldc1ControlWord;
      private MotorModes bldc1Mode;
      private Int16 bldc1TargetTorque;
      private Int32 bldc1TargetVelocity;
      private Int32 bldc1TargetPosition;

      private UInt16 stepper0ControlWord;
      private MotorModes stepper0Mode;

      private UInt16 stepper1ControlWord;
      private MotorModes stepper1Mode;

      #endregion

      #region Helper Functions

      private double GetSignedTemperature(byte reading)
      {
         double result = (reading < 127) ? reading : -256 + reading;
         return (result);
      }

      private bool EmitRpo1()
      {
         byte[] bldc0ControlWordData = BitConverter.GetBytes(this.bldc0ControlWord);
         byte[] bldc0TargetTorqueData = BitConverter.GetBytes(this.bldc0TargetTorque);
         byte[] bldc1ControlWordData = BitConverter.GetBytes(this.bldc1ControlWord);
         byte[] bldc1TargetTorqueData = BitConverter.GetBytes(this.bldc1TargetTorque);

         byte[] pdoData = new byte[bldc0ControlWordData.Length + bldc0TargetTorqueData.Length + bldc1ControlWordData.Length + bldc1TargetTorqueData.Length];
         int index = 0;

         for (int i = 0; i < bldc0ControlWordData.Length; i++)
         {
            pdoData[index++] = bldc0ControlWordData[i];
         }

         for (int i = 0; i < bldc0TargetTorqueData.Length; i++)
         {
            pdoData[index++] = bldc0TargetTorqueData[i];
         }

         for (int i = 0; i < bldc1ControlWordData.Length; i++)
         {
            pdoData[index++] = bldc1ControlWordData[i];
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
         byte[] bldc0TargetVelocityData = BitConverter.GetBytes(this.bldc0TargetVelocity);
         byte[] bldc1TargetVelocityData = BitConverter.GetBytes(this.bldc1TargetVelocity);

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
         byte[] bldc0TargetPositionData = BitConverter.GetBytes(this.bldc0TargetPosition);
         byte[] bldc1TargetPositionData = BitConverter.GetBytes(this.bldc1TargetPosition);

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

      public UInt16 Bldc0Status { set; get; }
      public Int16 Bldc0ActualCurrent { set; get; }
      public Int32 Bldc0ActualVelocity { set; get; }
      public Int32 Bldc0ActualPosition { set; get; }
      public double Bldc0Temperature { set; get; }
      public bool Bldc0PositionAttained { set; get; }
      public bool Bldc0VelocityAttained { set; get; }

      public UInt16 Bldc1Status { set; get; }
      public Int16 Bldc1ActualCurrent { set; get; }
      public Int32 Bldc1ActualVelocity { set; get; }
      public Int32 Bldc1ActualPosition { set; get; }
      public double Bldc1Temperature { set; get; }
      public bool Bldc1PositionAttained { set; get; }
      public bool Bldc1VelocityAttained { set; get; }

      public UInt16 Stepper0Status { set; get; }
      public bool Stepper0PositionAttained { set; get; }
      public bool Stepper0HomingAttained { set; get; }
      public bool Stepper0HomeDefined { set; get; }

      public UInt16 Stepper1Status { set; get; }
      public bool Stepper1PositionAttained { set; get; }
      public bool Stepper1HomingAttained { set; get; }
      public bool Stepper1HomeDefined { set; get; }

      public double MainBoardImuRoll { set; get; }
      public double MainBoardImuPitch { set; get; }
      public double MainBoardImuYaw { set; get; }
      public double McuTemperature { set; get; }

      public double LaserMeasuredDistance { set; get; }
      public byte LaserScannerPosition { set; get; }

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
            }
            else if (COBTypes.TPDO1 == frameType)
            {
               if ((null != msg) && (msg.Length >= 8))
               {
                  this.Bldc0Status = BitConverter.ToUInt16(msg, 0);
                  this.Bldc0ActualCurrent = BitConverter.ToInt16(msg, 2);
                  this.Bldc1Status = BitConverter.ToUInt16(msg, 4);
                  this.Bldc1ActualCurrent = BitConverter.ToInt16(msg, 6);


                  bool bldc0Operational = ((this.Bldc0Status & 0x0004) != 0) ? true : false;

                  if (false != bldc0Operational)
                  {
                     if (MotorModes.position == this.bldc0Mode)
                     {
                        this.Bldc0PositionAttained = ((this.Bldc0Status & 0x0400) != 0) ? true : false;
                     }
                     else if (MotorModes.velocity == this.bldc0Mode)
                     {
                        this.Bldc0VelocityAttained = ((this.Bldc0Status & 0x0400) != 0) ? true : false;
                     }
                  }


                  bool bldc1Operational = ((this.Bldc1Status & 0x0004) != 0) ? true : false;

                  if (false != bldc1Operational)
                  {
                     if (MotorModes.position == this.bldc1Mode)
                     {
                        this.Bldc1PositionAttained = ((this.Bldc1Status & 0x0400) != 0) ? true : false;
                     }
                     else if (MotorModes.velocity == this.bldc1Mode)
                     {
                        this.Bldc1VelocityAttained = ((this.Bldc1Status & 0x0400) != 0) ? true : false;
                     }
                  }
               }
            }
            else if (COBTypes.TPDO2 == frameType)
            {
               if ((null != msg) && (msg.Length >= 8))
               {
                  this.Bldc0ActualVelocity = BitConverter.ToInt32(msg, 0);
                  this.Bldc1ActualVelocity = BitConverter.ToInt32(msg, 4);
               }
            }
            else if (COBTypes.TPDO3 == frameType)
            {
               if ((null != msg) && (msg.Length >= 8))
               {
                  this.Bldc0ActualPosition = BitConverter.ToInt32(msg, 0);
                  this.Bldc1ActualPosition = BitConverter.ToInt32(msg, 4);
               }               
            }
            else if (COBTypes.TPDO4 == frameType)
            {
               if ((null != msg) && (msg.Length >= 2))
               {
                  this.Bldc0Temperature = this.GetSignedTemperature(msg[0]);
                  this.Bldc1Temperature = this.GetSignedTemperature(msg[1]);
               }
            }
         }
         else if (nodeId == (this.NodeId + 1))
         {
            if (COBTypes.TPDO1 == frameType) // TPDO5
            {
               if ((null != msg) && (msg.Length >= 7))
               {                  
                  this.MainBoardImuRoll = ((double)BitConverter.ToInt16(msg, 0) / 16);
                  this.MainBoardImuPitch = ((double)BitConverter.ToInt16(msg, 2) / 16);
                  this.MainBoardImuYaw = ((double)BitConverter.ToInt16(msg, 4) / 16);
                  this.McuTemperature = this.GetSignedTemperature(msg[6]);
               }
            }
            else if (COBTypes.TPDO2 == frameType) // TPDO6
            {
               if ((null != msg) && (msg.Length >= 5))
               {
                  this.LaserMeasuredDistance = ((double)BitConverter.ToUInt32(msg, 0) / 1);
                  this.LaserScannerPosition = msg[4];
               }
            }
            else if (COBTypes.TPDO3 == frameType) // TPDO7
            {
               if ((null != msg) && (msg.Length >= 4))
               {
                  this.Stepper0Status = BitConverter.ToUInt16(msg, 0);
                  this.Stepper1Status = BitConverter.ToUInt16(msg, 2);


                  bool stepper0Operational = ((this.Stepper0Status & 0x0004) != 0) ? true : false;

                  if (false != stepper0Operational)
                  {
                     this.Stepper0HomeDefined = ((this.Stepper0Status & 0x0100) != 0) ? true : false;

                     if (MotorModes.position == this.stepper0Mode)
                     {
                        this.Stepper0PositionAttained = ((this.Stepper0Status & 0x0400) != 0) ? true : false;
                     }
                     else if (MotorModes.homing == this.stepper0Mode)
                     {
                        if (((this.stepper0ControlWord & 0x0010) != 0) && 
                            ((this.Stepper0Status & 0x0400) != 0))
                        {
                           this.Stepper0HomingAttained = true;
                        }
                        else
                        {
                           this.Stepper0HomingAttained = false;
                        }
                     }
                  }


                  bool stepper1Operational = ((this.Stepper1Status & 0x0004) != 0) ? true : false;

                  if (false != stepper1Operational)
                  {
                     this.Stepper1HomeDefined = ((this.Stepper0Status & 0x0100) != 0) ? true : false;

                     if (MotorModes.position == this.stepper1Mode)
                     {
                        this.Stepper1PositionAttained = ((this.Stepper1Status & 0x0400) != 0) ? true : false;
                     }
                     else if (MotorModes.homing == this.stepper1Mode)
                     {
                        if (((this.stepper1ControlWord & 0x0010) != 0) &&
                            ((this.Stepper1Status & 0x0400) != 0))
                        {
                           this.Stepper1HomingAttained = true;
                        }
                        else
                        {
                           this.Stepper1HomingAttained = false;
                        }
                     }
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
         this.bldc0TargetTorque = 0;
         this.bldc1TargetTorque = 0;
      }

      #endregion

      #region Access Methods

      #region General Functions

      public override void Initialize()
      {
         base.Initialize();
      }

      public override bool Configure()
      {
         bool result = false;

         if (null == this.FaultReason)
         {
            result = true;

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
            result &= this.SetTPDOMapCount(4, 2);
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
            result &= this.SetTPDOMap(6, 1, 0x2402, 0x00, 4); // laser measured distance
            result &= this.SetTPDOMap(6, 2, 0x2410, 0x01, 1); // laser scanner position
            result &= this.SetTPDOMapCount(6, 2);
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
            result &= this.SetRPDOMap(1, 1, 0x6040, 0, 2); // BLDC0 control word
            result &= this.SetRPDOMap(1, 2, 0x6071, 0, 2); // BLDC0 target torque
            result &= this.SetRPDOMap(1, 3, 0x6840, 0, 2); // BLDC1 control word
            result &= this.SetRPDOMap(1, 4, 0x6871, 0, 2); // BLDC1 target torque
            result &= this.SetRPDOMapCount(1, 4);
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

         this.Bldc0PositionAttained = false;
         this.Bldc0VelocityAttained = false;

         this.Bldc1PositionAttained = false;
         this.Bldc1VelocityAttained = false;

         this.Stepper0PositionAttained = false;
         this.Stepper0HomingAttained = false;
         this.Stepper0HomeDefined = false;

         this.Stepper1PositionAttained = false;
         this.Stepper1HomingAttained = false;
         this.Stepper1HomeDefined = false;
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

      #region BLDL0 Functions

      public bool SetBldc0ControlWord(UInt16 controlWord)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x6040, 0, 2, controlWord));

         if (false != result)
         {
            this.bldc0ControlWord = controlWord;
         }

         return (result);
      }

      public bool SetBldc0Mode(MotorModes mode)
      {
         bool result = true;

         if (mode != this.bldc0Mode)
         {
            this.Bldc0PositionAttained = false;
            this.Bldc0VelocityAttained = false;
         }

         if (MotorModes.off == mode)
         {
            result &= this.SetBldc0ControlWord(0x0);
         }
         else if (MotorModes.position == mode)
         {
            result &= this.SetBldc0ControlWord(0x6);
            result &= this.SetBldc0ControlWord(0x7);
            result &= this.ExchangeCommAction(new SDODownload(0x6060, 0, 1, (byte)1));
            result &= this.SetBldc0ControlWord(0xF);
         }
         else if (MotorModes.velocity == mode)
         {
            result &= this.SetBldc0ControlWord(0x6);
            result &= this.SetBldc0ControlWord(0x7);
            result &= this.ExchangeCommAction(new SDODownload(0x6060, 0, 1, (byte)3));
            result &= this.SetBldc0ControlWord(0xF);
         }
         else
         {
            result = false;
         }

         if (false != result)
         {
            this.bldc0Mode = mode;
         }

         return (result);
      }

      public bool GetBldc0TargetPosition(ref Int32 targetPosition)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x607A, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            targetPosition = BitConverter.ToInt32(upload.Data, 0);
            this.bldc0TargetPosition = targetPosition;
            result = true;
         }

         return (result);
      }

      public bool SetBldc0TargetPosition(Int32 targetPosition)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x607A, 0, 4, (UInt32)targetPosition));

         if (false != result)
         {
            this.bldc0TargetPosition = targetPosition;
         }

         return (result);
      }

      public bool ScheduleBldc0TargetPosition(Int32 targetPosition)
      {
         bool result = true;

         this.bldc0TargetPosition = targetPosition;
         result = this.EmitRpo3();

         return (result);
      }

      public bool GetBldc0ProfileVelocity(ref Int32 profileVelocity)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x6081, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            profileVelocity = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetBldc0ProfileVelocity(Int32 profileVelocity)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x6081, 0, 4, (UInt32)profileVelocity));

         return (result);
      }

      public bool GetBldc0TargetVelocity(ref Int32 targetVelocity)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x60FF, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            targetVelocity = BitConverter.ToInt32(upload.Data, 0);
            this.bldc0TargetVelocity = targetVelocity;
            result = true;
         }

         return (result);
      }

      public bool SetBldc0TargetVelocity(Int32 targetVelocity)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x60FF, 0, 4, (UInt32)targetVelocity));

         if (false != result)
         {
            this.bldc0TargetVelocity = targetVelocity;
         }

         return (result);
      }

      public bool ScheduleBldc0TargetVelocity(Int32 targetVelocity)
      {
         bool result = true;

         this.bldc0TargetVelocity = targetVelocity;
         result = this.EmitRpo2();

         return (result);
      }

      public bool GetBldc0ProfileAcceleration(ref Int32 profileAcceleration)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x6083, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            profileAcceleration = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetBldc0ProfileAcceleration(Int32 profileAcceleration)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x6083, 0, 4, (UInt32)profileAcceleration));

         return (result);
      }

      public bool GetBldc0ProfileDeceleration(ref Int32 profileDeceleration)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x6084, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            profileDeceleration = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetBldc0ProfileDeceleration(Int32 profileDeceleration)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x6084, 0, 4, (UInt32)profileDeceleration));

         return (result);
      }

      public bool GetBldc0VelocityKp(ref Int32 motorKp)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x60F9, 0x01);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            motorKp = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetBldc0VelocityKp(Int32 motorKp)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x60F9, 0x01, 4, (UInt32)motorKp));

         return (result);
      }

      public bool GetBldc0VelocityKi(ref Int32 motorKi)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x60F9, 0x02);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            motorKi = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetBldc0VelocityKi(Int32 motorKi)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x60F9, 0x02, 4, (UInt32)motorKi));

         return (result);
      }

      public bool GetBldc0VelocityKd(ref Int32 motorKd)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x60F9, 0x03);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            motorKd = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetBldc0VelocityKd(Int32 motorKd)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x60F9, 0x03, 4, (UInt32)motorKd));

         return (result);
      }

      public bool GetBldc0PositionKp(ref Int32 motorKp)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x60FB, 0x01);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            motorKp = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetBldc0PositionKp(Int32 motorKp)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x60FB, 0x01, 4, (UInt32)motorKp));

         return (result);
      }

      public bool GetBldc0PositionKi(ref Int32 motorKi)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x60FB, 0x02);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            motorKi = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetBldc0PositionKi(Int32 motorKi)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x60FB, 0x02, 4, (UInt32)motorKi));

         return (result);
      }

      public bool GetBldc0PositionKd(ref Int32 motorKd)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x60FB, 0x03);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            motorKd = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetBldc0PositionKd(Int32 motorKd)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x60FB, 0x03, 4, (UInt32)motorKd));

         return (result);
      }

      #endregion

      #region BLDC1 Functions

      public bool SetBldc1ControlWord(UInt16 controlWord)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x6840, 0, 2, controlWord));

         if (false != result)
         {
            this.bldc1ControlWord = controlWord;
         }

         return (result);
      }

      public bool SetBldc1Mode(MotorModes mode)
      {
         bool result = true;

         if (mode != this.bldc1Mode)
         {
            this.Bldc1PositionAttained = false;
            this.Bldc1VelocityAttained = false;
         }

         if (MotorModes.off == mode)
         {
            result &= this.SetBldc1ControlWord(0x0);
         }
         else if (MotorModes.position == mode)
         {
            result &= this.SetBldc1ControlWord(0x6);
            result &= this.SetBldc1ControlWord(0x7);
            result &= this.ExchangeCommAction(new SDODownload(0x6860, 0, 1, (byte)1));
            result &= this.SetBldc1ControlWord(0xF);
         }
         else if (MotorModes.velocity == mode)
         {
            result &= this.SetBldc1ControlWord(0x6);
            result &= this.SetBldc1ControlWord(0x7);
            result &= this.ExchangeCommAction(new SDODownload(0x6860, 0, 1, (byte)3));
            result &= this.SetBldc1ControlWord(0xF);
         }
         else
         {
            result = false;
         }

         if (false != result)
         {
            this.bldc1Mode = mode;
         }

         return (result);
      }

      public bool GetBldc1TargetPosition(ref Int32 targetPosition)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x687A, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            targetPosition = BitConverter.ToInt32(upload.Data, 0);
            this.bldc1TargetPosition = targetPosition;
            result = true;
         }

         return (result);
      }

      public bool SetBldc1TargetPosition(Int32 targetPosition)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x687A, 0, 4, (UInt32)targetPosition));

         if (false != result)
         {
            this.bldc1TargetPosition = targetPosition;
         }

         return (result);
      }

      public bool ScheduleBldc1TargetPosition(Int32 targetPosition)
      {
         bool result = true;

         this.bldc1TargetPosition = targetPosition;
         result = this.EmitRpo3();

         return (result);
      }

      public bool GetBldc1TargetVelocity(ref Int32 targetVelocity)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x68FF, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            targetVelocity = BitConverter.ToInt32(upload.Data, 0);
            this.bldc1TargetVelocity = targetVelocity;
            result = true;
         }

         return (result);
      }

      public bool SetBldc1TargetVelocity(Int32 targetVelocity)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x68FF, 0, 4, (UInt32)targetVelocity));

         if (false != result)
         {
            this.bldc1TargetVelocity = targetVelocity;
         }

         return (result);
      }

      public bool ScheduleBldc1TargetVelocity(Int32 targetVelocity)
      {
         bool result = true;

         this.bldc1TargetVelocity = targetVelocity;
         result = this.EmitRpo2();

         return (result);
      }

      public bool GetBldc1ProfileVelocity(ref Int32 profileVelocity)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x6881, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            profileVelocity = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetBldc1ProfileVelocity(Int32 profileVelocity)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x6881, 0, 4, (UInt32)profileVelocity));

         return (result);
      }

      public bool GetBldc1ProfileAcceleration(ref Int32 profileAcceleration)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x6883, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            profileAcceleration = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetBldc1ProfileAcceleration(Int32 profileAcceleration)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x6883, 0, 4, (UInt32)profileAcceleration));

         return (result);
      }

      public bool GetBldc1ProfileDeceleration(ref Int32 profileDeceleration)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x6884, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            profileDeceleration = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetBldc1ProfileDeceleration(Int32 profileDeceleration)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x6884, 0, 4, (UInt32)profileDeceleration));

         return (result);
      }

      public bool GetBldc1VelocityKp(ref Int32 motorKp)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x68F9, 0x01);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            motorKp = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetBldc1VelocityKp(Int32 motorKp)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x68F9, 0x01, 4, (UInt32)motorKp));

         return (result);
      }

      public bool GetBldc1VelocityKi(ref Int32 motorKi)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x68F9, 0x02);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            motorKi = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetBldc1VelocityKi(Int32 motorKi)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x68F9, 0x02, 4, (UInt32)motorKi));

         return (result);
      }

      public bool GetBldc1VelocityKd(ref Int32 motorKd)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x68F9, 0x03);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            motorKd = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetBldc1VelocityKd(Int32 motorKd)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x68F9, 0x03, 4, (UInt32)motorKd));

         return (result);
      }

      public bool GetBldc1PositionKp(ref Int32 motorKp)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x68FB, 0x01);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            motorKp = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetBldc1PositionKp(Int32 motorKp)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x68FB, 0x01, 4, (UInt32)motorKp));

         return (result);
      }

      public bool GetBldc1PositionKi(ref Int32 motorKi)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x68FB, 0x02);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            motorKi = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetBldc1PositionKi(Int32 motorKi)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x68FB, 0x02, 4, (UInt32)motorKi));

         return (result);
      }

      public bool GetBldc1PositionKd(ref Int32 motorKd)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x68FB, 0x03);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            motorKd = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetBldc1PositionKd(Int32 motorKd)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x68FB, 0x03, 4, (UInt32)motorKd));

         return (result);
      }

      #endregion

      #region Stepper0 Functions

      public bool SetStepper0ControlWord(UInt16 controlWord)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x7040, 0, 2, controlWord));

         if (false != result)
         {
            this.stepper0ControlWord = controlWord;
         }

         return (result);
      }

      public bool SetStepper0Mode(MotorModes mode)
      {
         bool result = true;

         if (mode != this.stepper0Mode)
         {
            this.Stepper0PositionAttained = false;
            this.Stepper0HomingAttained = false;
         }

         if (MotorModes.off == mode)
         {
            result &= this.SetStepper0ControlWord(0x0);
         }
         else if (MotorModes.position == mode)
         {
            result &= this.SetStepper0ControlWord(0x6);
            result &= this.SetStepper0ControlWord(0x7);
            result &= this.ExchangeCommAction(new SDODownload(0x7060, 0, 1, (byte)1));
            result &= this.SetStepper0ControlWord(0xF);
         }
         else if (MotorModes.homing == mode)
         {
            result &= this.SetStepper0ControlWord(0x6);
            result &= this.SetStepper0ControlWord(0x7);
            result &= this.ExchangeCommAction(new SDODownload(0x7060, 0, 1, (byte)6));
            result &= this.SetStepper0ControlWord(0xF);
         }
         else
         {
            result = false;
         }

         if (false != result)
         {
            this.stepper0Mode = mode;
         }

         return (result);
      }

      public bool GetStepper0ActualPosition(ref Int32 actualPosition)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x7064, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            actualPosition = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool GetStepper0ActualVelocity(ref Int32 actualPosition)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x706C, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            actualPosition = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool GetStepper0ActualCurrent(ref Int16 actualPosition)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x7078, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 2))
         {
            actualPosition = BitConverter.ToInt16(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool GetStepper0TargetPosition(ref Int32 targetPosition)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x707A, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            targetPosition = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetStepper0TargetPosition(Int32 targetPosition)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x707A, 0, 4, (UInt32)targetPosition));

         return (result);
      }

      public bool GetStepper0ProfileVelocity(ref Int32 profileVelocity)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x7081, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            profileVelocity = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetStepper0ProfileVelocity(Int32 profileVelocity)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x7081, 0, 4, (UInt32)profileVelocity));

         return (result);
      }

      public bool GetStepper0ProfileAcceleration(ref Int32 profileAcceleration)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x7083, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            profileAcceleration = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetStepper0ProfileAcceleration(Int32 profileAcceleration)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x7083, 0, 4, (UInt32)profileAcceleration));

         return (result);
      }

      public bool StartStepper0Homing()
      {
         bool result = true;

         if (MotorModes.homing == this.stepper0Mode)
         {
            UInt16 controlWord = this.stepper0ControlWord;
            controlWord |= 0x0010;
            result &= this.SetStepper0ControlWord(controlWord);

            if (false != result)
            {
               this.Stepper0HomingAttained = false;
            }
         }
         else
         {
            result = false;
         }

         return (result);
      }

      public bool StopStepper0Homing()
      {
         bool result = true;

         if (MotorModes.homing == this.stepper0Mode)
         {
            UInt16 controlWord = this.stepper0ControlWord;
            controlWord &= 0xFFEF;
            result &= this.SetStepper0ControlWord(controlWord);
         }
         else
         {
            result = false;
         }

         return (result);
      }

      public bool HaltStepper0Homing()
      {
         bool result = true;

         if (MotorModes.homing == this.stepper0Mode)
         {
            UInt16 controlWord = this.stepper0ControlWord;
            controlWord |= 0x0100;
            result &= this.SetStepper0ControlWord(controlWord);
         }
         else
         {
            result = false;
         }

         return (result);
      }

      public bool RunStepper0Homing()
      {
         bool result = true;

         if (MotorModes.homing == this.stepper0Mode)
         {
            UInt16 controlWord = this.stepper0ControlWord;
            controlWord &= 0xFEFF;
            result &= this.SetStepper0ControlWord(controlWord);
         }
         else
         {
            result = false;
         }

         return (result);
      }

      public bool GetStepper0HomingMethod(ref byte homingMethod)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x7098, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 1))
         {
            homingMethod = upload.Data[0];
            result = true;
         }

         return (result);
      }

      public bool SetStepper0HomingMethod(byte homingMethod)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x7098, 0, 1, (UInt32)homingMethod));

         return (result);
      }         

      public bool GetStepper0HomingSwitchSpeed(ref UInt32 homingSwitchSpeed)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x7099, 0x01);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            homingSwitchSpeed = BitConverter.ToUInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetStepper0HomingSwitchSpeed(UInt32 homingSwitchSpeed)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x7099, 0x01, 4, (UInt32)homingSwitchSpeed));

         return (result);
      }

      public bool GetStepper0HomingZeroSpeed(ref UInt32 homingZeroSpeed)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x7099, 0x02);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            homingZeroSpeed = BitConverter.ToUInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetStepper0HomingZeroSpeed(UInt32 homingZeroSpeed)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x7099, 0x02, 4, (UInt32)homingZeroSpeed));

         return (result);
      }

      public bool GetStepper0HomingAcceleration(ref UInt32 homingAcceleration)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x709A, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            homingAcceleration = BitConverter.ToUInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetStepper0HomingAcceleration(UInt32 homingAcceleration)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x709A, 0, 4, (UInt32)homingAcceleration));

         return (result);
      }

      public bool GetStepper0HomeOffset(ref Int32 homeOffset)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x707C, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            homeOffset = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetStepper0HomeOffset(Int32 homeOffset)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x707C, 0, 4, (UInt32)homeOffset));

         return (result);
      }

      #endregion

      #region Stepper1 Functions

      public bool SetStepper1ControlWord(UInt16 controlWord)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x7840, 0, 2, controlWord));

         if (false != result)
         {
            this.stepper1ControlWord = controlWord;
         }

         return (result);
      }

      public bool SetStepper1Mode(MotorModes mode)
      {
         bool result = true;

         if (mode != this.stepper1Mode)
         {
            this.Stepper1PositionAttained = false;
            this.Stepper1HomingAttained = false;
         }

         if (MotorModes.off == mode)
         {
            result &= this.SetStepper1ControlWord(0x0);
         }
         else if (MotorModes.position == mode)
         {
            result &= this.SetStepper1ControlWord(0x6);
            result &= this.SetStepper1ControlWord(0x7);
            result &= this.ExchangeCommAction(new SDODownload(0x7860, 0, 1, (byte)1));
            result &= this.SetStepper1ControlWord(0xF);
         }
         else if (MotorModes.homing == mode)
         {
            result &= this.SetStepper1ControlWord(0x6);
            result &= this.SetStepper1ControlWord(0x7);
            result &= this.ExchangeCommAction(new SDODownload(0x7860, 0, 1, (byte)6));
            result &= this.SetStepper1ControlWord(0xF);
         }
         else
         {
            result = false;
         }

         if (false != result)
         {
            this.stepper1Mode = mode;
         }

         return (result);
      }

      public bool GetStepper1ActualPosition(ref Int32 actualPosition)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x7864, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            actualPosition = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool GetStepper1ActualVelocity(ref Int32 actualPosition)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x786C, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            actualPosition = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool GetStepper1ActualCurrent(ref Int16 actualPosition)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x7878, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 2))
         {
            actualPosition = BitConverter.ToInt16(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool GetStepper1TargetPosition(ref Int32 targetPosition)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x787A, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            targetPosition = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetStepper1TargetPosition(Int32 targetPosition)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x787A, 0, 4, (UInt32)targetPosition));

         return (result);
      }

      public bool GetStepper1ProfileVelocity(ref Int32 profileVelocity)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x7881, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            profileVelocity = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetStepper1ProfileVelocity(Int32 profileVelocity)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x7881, 0, 4, (UInt32)profileVelocity));

         return (result);
      }

      public bool GetStepper1ProfileAcceleration(ref Int32 profileAcceleration)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x7883, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            profileAcceleration = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetStepper1ProfileAcceleration(Int32 profileAcceleration)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x7883, 0, 4, (UInt32)profileAcceleration));

         return (result);
      }

      public bool StartStepper1Homing()
      {
         bool result = true;

         if (MotorModes.homing == this.stepper1Mode)
         {
            UInt16 controlWord = this.stepper1ControlWord;
            controlWord |= 0x0010;
            result &= this.SetStepper1ControlWord(controlWord);

            if (false != result)
            {
               this.Stepper1HomingAttained = false;
            }
         }
         else
         {
            result = false;
         }

         return (result);
      }

      public bool StopStepper1Homing()
      {
         bool result = true;

         if (MotorModes.homing == this.stepper1Mode)
         {
            UInt16 controlWord = this.stepper1ControlWord;
            controlWord &= 0xFFEF;
            result &= this.SetStepper1ControlWord(controlWord);
         }
         else
         {
            result = false;
         }

         return (result);
      }

      public bool HaltStepper1Homing()
      {
         bool result = true;

         if (MotorModes.homing == this.stepper1Mode)
         {
            UInt16 controlWord = this.stepper1ControlWord;
            controlWord |= 0x0100;
            result &= this.SetStepper1ControlWord(controlWord);
         }
         else
         {
            result = false;
         }

         return (result);
      }

      public bool RunStepper1Homing()
      {
         bool result = true;

         if (MotorModes.homing == this.stepper1Mode)
         {
            UInt16 controlWord = this.stepper1ControlWord;
            controlWord &= 0xFEFF;
            result &= this.SetStepper1ControlWord(controlWord);
         }
         else
         {
            result = false;
         }

         return (result);
      }

      public bool GetStepper1HomingMethod(ref byte homingMethod)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x7898, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 1))
         {
            homingMethod = upload.Data[0];
            result = true;
         }

         return (result);
      }

      public bool SetStepper1HomingMethod(byte homingMethod)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x7898, 0, 1, (UInt32)homingMethod));

         return (result);
      }

      public bool GetStepper1HomingSwitchSpeed(ref UInt32 homingSwitchSpeed)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x7899, 0x01);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            homingSwitchSpeed = BitConverter.ToUInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetStepper1HomingSwitchSpeed(UInt32 homingSwitchSpeed)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x7899, 0x01, 4, (UInt32)homingSwitchSpeed));

         return (result);
      }

      public bool GetStepper1HomingZeroSpeed(ref UInt32 homingZeroSpeed)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x7899, 0x02);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            homingZeroSpeed = BitConverter.ToUInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetStepper1HomingZeroSpeed(UInt32 homingZeroSpeed)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x7899, 0x02, 4, (UInt32)homingZeroSpeed));

         return (result);
      }

      public bool GetStepper1HomingAcceleration(ref UInt32 homingAcceleration)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x789A, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            homingAcceleration = BitConverter.ToUInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetStepper1HomingAcceleration(UInt32 homingAcceleration)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x789A, 0, 4, (UInt32)homingAcceleration));

         return (result);
      }

      public bool GetStepper1HomeOffset(ref Int32 homeOffset)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x787C, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            homeOffset = BitConverter.ToInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetStepper1HomeOffset(Int32 homeOffset)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x787C, 0, 4, (UInt32)homeOffset));

         return (result);
      }

      #endregion

      #endregion
   }
}