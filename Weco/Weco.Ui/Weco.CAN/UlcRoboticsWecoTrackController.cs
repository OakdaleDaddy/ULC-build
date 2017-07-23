
namespace Weco.CAN
{
   using System;
   using System.Text;
   using System.Threading;

   using Weco.Utilities;

   public class UlcRoboticsWecoTrackController : Device
   {
      #region Definition

      #endregion

      #region Fields

      private bool active;

      private DeviceComponent light;
      private MotorComponent trackMotor;

      #endregion

      #region Helper Functions

      private double GetSignedTemperature(byte reading)
      {
         double result = (reading < 127) ? reading : -256 + reading;
         return (result);
      }

      private bool EmitRpo1()
      {
         byte[] trackMotorTargetTorqueData = BitConverter.GetBytes(this.TrackMotor.TargetTorque);

         byte[] pdoData = new byte[trackMotorTargetTorqueData.Length];
         int index = 0;

         for (int i = 0; i < trackMotorTargetTorqueData.Length; i++)
         {
            pdoData[index++] = trackMotorTargetTorqueData[i];
         }

         bool result = this.ExchangeCommAction(new PDO1Emit(pdoData));
         return (result);
      }

      private bool EmitRpo2()
      {
         byte[] trackMotorTargetVelocityData = BitConverter.GetBytes(this.TrackMotor.TargetVelocity);

         byte[] pdoData = new byte[trackMotorTargetVelocityData.Length];
         int index = 0;

         for (int i = 0; i < trackMotorTargetVelocityData.Length; i++)
         {
            pdoData[index++] = trackMotorTargetVelocityData[i];
         }

         bool result = this.ExchangeCommAction(new PDO2Emit(pdoData));
         return (result);
      }

      private bool EmitRpo3()
      {
         byte[] trackMotorTargetPositionData = BitConverter.GetBytes(this.TrackMotor.TargetPosition);

         byte[] pdoData = new byte[trackMotorTargetPositionData.Length];
         int index = 0;

         for (int i = 0; i < trackMotorTargetPositionData.Length; i++)
         {
            pdoData[index++] = trackMotorTargetPositionData[i];
         }

         bool result = this.ExchangeCommAction(new PDO3Emit(pdoData));
         return (result);
      }

      private void TraceReceive(int nodeId, COBTypes frameType, int cobId, byte[] msg)
      {
         if (nodeId == this.NodeId)
         {
            base.TraceReceive(frameType, cobId, msg);
         }
         else if (nodeId == (this.NodeId + 1))
         {
            bool trace = false;

            if (COBTypes.TPDO1 == frameType) 
            {
               trace = this.TraceTPDO5;
            }
            else if (COBTypes.TPDO2 == frameType) 
            {
               trace = this.TraceTPDO6;
            }
            else if (COBTypes.TPDO3 == frameType) 
            {
               trace = this.TraceTPDO7;
            }
            else if (COBTypes.TPDO4 == frameType) 
            {
               trace = this.TraceTPDO8;
            }

            if (false != trace)
            {
               if (null != this.OnReceiveTrace)
               {
                  this.OnReceiveTrace(cobId, msg);
               }
            }
         }
      }

      #endregion

      #region Properties
            
      public DeviceComponent Light
      {
         get
         {
            if (null == this.light)
            {
               this.light = new DeviceComponent();
            }

            return (this.light);
         }
      }

      public MotorComponent TrackMotor
      {
         get
         {
            if (null == this.trackMotor)
            {
               this.trackMotor = new MotorComponent();
            }

            return (this.trackMotor);
         }
      }

      public bool TraceTPDO5 { set; get; }
      public bool TraceTPDO6 { set; get; }
      public bool TraceTPDO7 { set; get; }
      public bool TraceTPDO8 { set; get; }

      public new int TraceMask
      {
         set
         {
            base.TraceMask = value;

            this.TraceTPDO5 = ((value & 0x0400) != 0) ? true : false;
            this.TraceTPDO6 = ((value & 0x0800) != 0) ? true : false;
            this.TraceTPDO7 = ((value & 0x1000) != 0) ? true : false;
            this.TraceTPDO8 = ((value & 0x2000) != 0) ? true : false;
         }

         get
         {
            int result = base.TraceMask;

            result |= (false != this.TraceTPDO5) ? 0x0400 : 0;
            result |= (false != this.TraceTPDO6) ? 0x0800 : 0;
            result |= (false != this.TraceTPDO7) ? 0x1000 : 0;
            result |= (false != this.TraceTPDO8) ? 0x2000 : 0;

            return (result);
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
      public byte ScannerCoordinates { set; get; }

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

      protected override void EvaluateReceiveTrace(int cobId, byte[] msg, ref bool traced)
      {
         COBTypes frameType = (COBTypes)((cobId >> 7) & 0xF);
         int nodeId = (int)(cobId & 0x7F);

         if (false == traced)
         {
            if ((nodeId == this.NodeId) ||
                (nodeId == (this.NodeId + 1)))
            {
               this.TraceReceive(nodeId, frameType, cobId, msg);
               traced = true;
            }
         }
      }

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
                  UInt16 errorCode = BitConverter.ToUInt16(errorMsg, 0);
                  UInt32 emergencyRecordCode = BitConverter.ToUInt32(errorMsg, 0); 
                  byte errorRegister = errorMsg[2];
                  byte codeSpecificData = errorMsg[3];
                  int subSystem = errorRegister & 0x1F;
                  bool componentFault = ((errorRegister & 0x20) != 0) ? true : false;
                  bool deviceFault = true;
                  bool deviceWarning = false;
                  string reason = string.Format("emergency {0:X16}", errorValue);

                  #region Board Emergencies
                  if (0x1000 == errorCode)
                  {
                     reason = "generic error";
                  }
                  else if (0x6001 == errorCode)
                  {
                     UInt16 additionalData = BitConverter.ToUInt16(errorMsg, 3);
                     string resetTypeString = null;

                     deviceFault = false;

                     if (0 == additionalData)
                     {
                        resetTypeString = "power on reset";
                     }
                     else if (1 == additionalData)
                     {
                        resetTypeString = "watchdog reset";
                     }
                     else if (2 == additionalData)
                     {
                        resetTypeString = "software reset";
                     }
                     else if (3 == additionalData)
                     {
                        resetTypeString = "reset pin reset";
                     }
                     else if (4 == additionalData)
                     {
                        resetTypeString = "option byte reset";
                     }
                     else if (5 == additionalData)
                     {
                        resetTypeString = "direct jump reset";
                     }
                     else if (6 == additionalData)
                     {
                        resetTypeString = "brown out reset";
                     }
                     else
                     {
                        resetTypeString = "undefined reset";
                     }

                     if (null != resetTypeString)
                     {
                        Tracer.WriteHigh(TraceGroup.CANBUS, "", "{0} {1}", this.Name, resetTypeString);
                     }
                  }
                  else if (0x6100 == errorCode)
                  {
                     UInt32 crc = BitConverter.ToUInt32(errorMsg, 3);
                     reason = string.Format("bootloader CRC fail {0:X8}", crc);
                  }
                  else if (0x6200 == errorCode)
                  {
                     UInt32 crc = BitConverter.ToUInt32(errorMsg, 3);
                     reason = string.Format("application CRC fail {0:X8}", crc);
                  }
                  else if (0x6201 == errorCode)
                  {
                     reason = string.Format("application empty");
                  }
                  #endregion

                  #region Component Emergencies
                  else if (0x2340 == errorCode)
                  {
                     if (0 == subSystem)
                     {
                        deviceFault = false;

                        if (0 == codeSpecificData)
                        {
                           this.Light.RecordError(componentFault, emergencyRecordCode, "LEDs Shorted");
                        }
                     }
                  }
                  else if (0x3230 == errorCode)
                  {
                     if (0 == subSystem)
                     {
                        deviceFault = false;

                        if (0 == codeSpecificData)
                        {
                           this.Light.RecordError(componentFault, emergencyRecordCode, "LEDs Open");
                        }
                     }
                  }
                  else if (0x4210 == errorCode)
                  {
                     if (0 == subSystem)
                     {
                        deviceFault = false;
                        this.Light.RecordError(componentFault, emergencyRecordCode, "LED IC Excess Temperature");
                     }
                  }
                  else if (0x8311 == errorCode)
                  {
                     if (3 == subSystem)
                     {
                        deviceFault = false;
                        this.TrackMotor.RecordError(componentFault, emergencyRecordCode, "Average Current Error");
                     }
                  }
                  else if (0x8331 == errorCode)
                  {
                     if (3 == subSystem)
                     {
                        deviceFault = false;
                        this.TrackMotor.RecordError(componentFault, emergencyRecordCode, "Peak Current Error");
                     }
                  }
                  #endregion

                  if (false != deviceFault)
                  {
                     this.SetFault(reason, false);
                  }
                  else if (false != deviceWarning)
                  {
                     this.SetWarning(reason);
                  }
               }
            }
            else if (COBTypes.TPDO1 == frameType)
            {
               if ((null != msg) && (msg.Length >= 4))
               {
                  this.TrackMotor.Status = BitConverter.ToUInt16(msg, 0);
                  this.TrackMotor.ActualCurrent = BitConverter.ToInt16(msg, 2);

                  this.TrackMotor.ProcessStatus();
               }
            }
            else if (COBTypes.TPDO2 == frameType)
            {
               if ((null != msg) && (msg.Length >= 4))
               {
                  this.TrackMotor.ActualVelocity = BitConverter.ToInt32(msg, 0);
               }
            }
            else if (COBTypes.TPDO3 == frameType)
            {
               if ((null != msg) && (msg.Length >= 4))
               {
                  this.TrackMotor.ActualPosition = BitConverter.ToInt32(msg, 0);
               }
            }
            else if (COBTypes.TPDO4 == frameType)
            {
               if ((null != msg) && (msg.Length >= 2))
               {
                  this.McuTemperature = this.GetSignedTemperature(msg[0]);
                  this.TrackMotor.Temperature = this.GetSignedTemperature(msg[1]);
               }
            }
         }
         else if (nodeId == (this.NodeId + 1))
         {
            if (COBTypes.TPDO1 == frameType) // TPDO5
            {
            }
            else if (COBTypes.TPDO2 == frameType) // TPDO6
            {
            }
            else if (COBTypes.TPDO3 == frameType) // TPDO7
            {
            }
            else if (COBTypes.TPDO4 == frameType) // TPDO8
            {
            }
         }
      }

      protected override void EvaluateAction(CommAction action)
      {
         base.EvaluateAction(action);
      }

      #endregion

      #region Constructor

      public UlcRoboticsWecoTrackController(string name, byte nodeId)
         : base(name, nodeId)
      {
         #region Camera/LED Initialization

         this.Light.Name = this.Name + " light";
         this.Light.OnCommExchange = new DeviceComponent.CommExchangeHandler(this.ExchangeCommAction);
         this.Light.OnClearErrorCode = new DeviceComponent.ClearErrorCodeHandler(this.ClearErrorCode);

         #endregion

         #region Tracke BLDC Initialization

         this.TrackMotor.Name = this.Name + " track motor";

         this.TrackMotor.OnCommExchange = new DeviceComponent.CommExchangeHandler(this.ExchangeCommAction);
         this.TrackMotor.OnClearErrorCode = new DeviceComponent.ClearErrorCodeHandler(this.ClearErrorCode);
         this.TrackMotor.OnTargetPositionSchedule = new MotorComponent.TargetPositionScheduleHandler(this.EmitRpo3);
         this.TrackMotor.OnTargetVelocitySchedule = new MotorComponent.TargetVelocityScheduleHandler(this.EmitRpo2);

         this.TrackMotor.MotorPeakCurrentLimitLocation = 0x00641003;

         this.TrackMotor.ControlWordLocation = 0x00604000;
         this.TrackMotor.SetOperationalModeLocation = 0x00606000;

         this.TrackMotor.PolarityLocation = 0x00607E00;
         this.TrackMotor.PositionNotationIndexLocation = 0x00608900;
         this.TrackMotor.VelocityNotationIndexLocation = 0x00608B00;
         this.TrackMotor.VelocityDimensionIndexLocation = 0x00608C00;
         this.TrackMotor.AccelerationNotationIndexLocation = 0x00608D00;
         this.TrackMotor.AccelerationDimensionIndexLocation = 0x00608E00;
         this.TrackMotor.PositionEncoderIncrementsLocation = 0x00608F01;
         this.TrackMotor.PositionEncoderMotorRevolutionsLocation = 0x00608F02;
         this.TrackMotor.VelocityEncoderIncrementsPerSecondLocation = 0x00609001;
         this.TrackMotor.VelocityEncoderMotorRevolutionsPerSecondLocation = 0x00609002;
         this.TrackMotor.GearRatioMotorRevolutionsLocation = 0x00609101;
         this.TrackMotor.GearRatioShaftRevolutionsLocation = 0x00609102;
         this.TrackMotor.FeedConstantFeedLocation = 0x00609201;
         this.TrackMotor.FeedConstantShaftRevolutionsLocation = 0x00609202;

         this.TrackMotor.TargetPositionLocation = 0x00607A00;
         this.TrackMotor.ProfileVelocityLocation = 0x00608100;
         this.TrackMotor.ProfileAccelerationLocation = 0x00608300;
         this.TrackMotor.ProfileDecelerationLocation = 0x00608400;

         this.TrackMotor.PositionWindowLocation = 0x00606700;
         this.TrackMotor.PositionWindowTimeLocation = 0x00606800;
         this.TrackMotor.PositionKpLocation = 0x0060FB01;
         this.TrackMotor.PositionKiLocation = 0x0060FB02;
         this.TrackMotor.PositionKdLocation = 0x0060FB03;

         this.TrackMotor.MaximumCurrentLocation = 0x00607300;
         this.TrackMotor.MotorRatedCurrentLocation = 0x00607500;

         this.TrackMotor.VelocityWindowLocation = 0x00606D00;
         this.TrackMotor.VelocityWindowTimeLocation = 0x00606E00;
         this.TrackMotor.VelocityThresholdLocation = 0x00606F00;
         this.TrackMotor.VelocityThresholdTimeLocation = 0x00607000;
         this.TrackMotor.VelocityKpLocation = 0x0060F901;
         this.TrackMotor.VelocityKiLocation = 0x0060F902;
         this.TrackMotor.VelocityKdLocation = 0x0060F903;
         this.TrackMotor.TargetVelocityLocation = 0x0060FF00;

         #endregion
      }

      #endregion

      #region Access Methods

      #region General Functions

      public override void Initialize()
      {
         this.Light.Initialize();
         this.TrackMotor.Initialize();
         
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
            result &= this.SetTPDOMap(1, 1, 0x6041, 0x00, 2); // track motor status word
            result &= this.SetTPDOMap(1, 2, 0x6078, 0x00, 2); // track motor current actual
            result &= this.SetTPDOMapCount(1, 4);
            result &= this.SetTPDOEnable(1, true);

            // set TPDO2 on change with 200mS inhibit time
            result &= this.SetTPDOEnable(2, false);
            result &= this.SetTPDOMapCount(2, 0);
            result &= this.SetTPDOType(2, 254);
            result &= this.SetTPDOInhibitTime(2, 200);
            result &= this.SetTPDOMap(2, 1, 0x606C, 0x00, 4); // track motor velocity actual value
            result &= this.SetTPDOMapCount(2, 2);
            result &= this.SetTPDOEnable(2, true);

            // set TPDO3 on change with 200mS inhibit time
            result &= this.SetTPDOEnable(3, false);
            result &= this.SetTPDOMapCount(3, 0);
            result &= this.SetTPDOType(3, 254);
            result &= this.SetTPDOInhibitTime(3, 200);
            result &= this.SetTPDOMap(3, 1, 0x6064, 0x00, 4); // track motor position actual value
            result &= this.SetTPDOMapCount(3, 2);
            result &= this.SetTPDOEnable(3, true);

            // set TPDO4 on change with 200mS inhibit time
            result &= this.SetTPDOEnable(4, false);
            result &= this.SetTPDOMapCount(4, 0);
            result &= this.SetTPDOType(4, 254);
            result &= this.SetTPDOInhibitTime(4, 200);
            result &= this.SetTPDOMap(4, 1, 0x2311, 0x01, 1); // MCU temperature
            result &= this.SetTPDOMap(4, 2, 0x6410, 0x01, 1); // track motor temperature
            result &= this.SetTPDOMapCount(4, 2);
            result &= this.SetTPDOEnable(4, true);


            // set RPDO1 every SYNC
            result &= this.SetRPDOEnable(1, false);
            result &= this.SetRPDOMapCount(1, 0);
            result &= this.SetRPDOType(1, 1);
            result &= this.SetRPDOMap(1, 1, 0x6071, 0, 2); // track motor target torque
            result &= this.SetRPDOMapCount(1, 1);
            result &= this.SetRPDOEnable(1, true);

            // set RPDO2 every SYNC
            result &= this.SetRPDOEnable(2, false);
            result &= this.SetRPDOMapCount(2, 0);
            result &= this.SetRPDOType(2, 1);
            result &= this.SetRPDOMap(2, 1, 0x60FF, 0, 4); // track motor target velocity
            result &= this.SetRPDOMapCount(2, 1);
            result &= this.SetRPDOEnable(2, true);

            // set RPDO3 every SYNC
            result &= this.SetRPDOEnable(3, false);
            result &= this.SetRPDOMapCount(3, 0);
            result &= this.SetRPDOType(3, 1);
            result &= this.SetRPDOMap(3, 1, 0x607A, 0, 4); // track motor target position
            result &= this.SetRPDOMapCount(3, 1);
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

         this.Light.Reset();
         this.TrackMotor.Reset();
      }

      public override void Update()
      {
         if (false != active)
         {
         }

         base.Update();
      }

      public override void SetFault(string faultReason, bool resetDevice)
      {
         this.Light.SetFault("device offline");
         this.TrackMotor.SetFault("device offline");

         base.SetFault(faultReason, resetDevice);
      }

      #endregion

      #region LED Functions

      public bool GetLedIntensityLevel(ref UInt32 ledIntensityLevel)
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

      public bool SetLedIntensityLevel(UInt32 ledIntensityLevel)
      {
         this.pendingAction = new SDODownload(0x2303, 0x01, 4, ledIntensityLevel);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      public bool GetLedChannelMask(ref byte ledChannelMask)
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

      public bool SetLedChannelMask(byte ledChannelMask)
      {
         this.pendingAction = new SDODownload(0x2304, 0x01, 1, ledChannelMask);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      #endregion

      #region Emergency / Error Functions

      public bool ClearErrorCode(UInt32 errorCode)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x2400, 0, 1, (UInt32)1));

         return (result);
      }

      #endregion

      #endregion
   }
}