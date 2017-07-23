
namespace Weco.CAN
{
   using System;
   using System.Text;
   using System.Threading;

   using Weco.Utilities;

   public class UlcRoboticsWecoHub : Device
   {
      #region Definition

      #endregion

      #region Fields

      private bool active;

      private DeviceComponent cameraLights;
      private MotorComponent panMotor;
      private MotorComponent tiltMotor;
      
      #endregion

      #region Helper Functions

      private double GetSignedTemperature(byte reading)
      {
         double result = (reading < 127) ? reading : -256 + reading;
         return (result);
      }

      private bool EmitRpo1()
      {
         byte[] panMotorTargetTorqueData = BitConverter.GetBytes(this.PanMotor.TargetTorque);
         byte[] tiltMotorTargetTorqueData = BitConverter.GetBytes(this.TiltMotor.TargetTorque);

         byte[] pdoData = new byte[panMotorTargetTorqueData.Length + tiltMotorTargetTorqueData.Length];
         int index = 0;

         for (int i = 0; i < panMotorTargetTorqueData.Length; i++)
         {
            pdoData[index++] = panMotorTargetTorqueData[i];
         }

         for (int i = 0; i < tiltMotorTargetTorqueData.Length; i++)
         {
            pdoData[index++] = tiltMotorTargetTorqueData[i];
         }

         bool result = this.ExchangeCommAction(new PDO1Emit(pdoData));
         return (result);
      }

      private bool EmitRpo2()
      {
         byte[] panMotorTargetVelocityData = BitConverter.GetBytes(this.PanMotor.TargetVelocity);
         byte[] tiltMotorTargetVelocityData = BitConverter.GetBytes(this.TiltMotor.TargetVelocity);

         byte[] pdoData = new byte[panMotorTargetVelocityData.Length + tiltMotorTargetVelocityData.Length];
         int index = 0;

         for (int i = 0; i < panMotorTargetVelocityData.Length; i++)
         {
            pdoData[index++] = panMotorTargetVelocityData[i];
         }

         for (int i = 0; i < tiltMotorTargetVelocityData.Length; i++)
         {
            pdoData[index++] = tiltMotorTargetVelocityData[i];
         }

         bool result = this.ExchangeCommAction(new PDO2Emit(pdoData));
         return (result);
      }

      private bool EmitRpo3()
      {
         byte[] panMotorTargetPositionData = BitConverter.GetBytes(this.PanMotor.TargetPosition);
         byte[] tiltMotorTargetPositionData = BitConverter.GetBytes(this.TiltMotor.TargetPosition);

         byte[] pdoData = new byte[panMotorTargetPositionData.Length + tiltMotorTargetPositionData.Length];
         int index = 0;

         for (int i = 0; i < panMotorTargetPositionData.Length; i++)
         {
            pdoData[index++] = panMotorTargetPositionData[i];
         }

         for (int i = 0; i < tiltMotorTargetPositionData.Length; i++)
         {
            pdoData[index++] = tiltMotorTargetPositionData[i];
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
            
      public DeviceComponent CameraLights
      {
         get
         {
            if (null == this.cameraLights)
            {
               this.cameraLights = new DeviceComponent();
            }

            return (this.cameraLights);
         }
      }

      public MotorComponent PanMotor
      {
         get
         {
            if (null == this.panMotor)
            {
               this.panMotor = new MotorComponent();
            }

            return (this.panMotor);
         }
      }

      public MotorComponent TiltMotor
      {
         get
         {
            if (null == this.tiltMotor)
            {
               this.tiltMotor = new MotorComponent();
            }

            return (this.tiltMotor);
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

      public double McuTemperature { set; get; }

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
                           this.CameraLights.RecordError(componentFault, emergencyRecordCode, "Front LEDs Shorted");
                        }
                        else if (1 == codeSpecificData)
                        {
                           this.CameraLights.RecordError(componentFault, emergencyRecordCode, "Rear LEDs Shorted");
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
                           this.CameraLights.RecordError(componentFault, emergencyRecordCode, "Front LEDs Open");
                        }
                        else if (1 == codeSpecificData)
                        {
                           this.CameraLights.RecordError(componentFault, emergencyRecordCode, "Rear LEDs Open");
                        }
                     }
                  }
                  else if (0x4210 == errorCode)
                  {
                     if (0 == subSystem)
                     {
                        deviceFault = false;
                        this.CameraLights.RecordError(componentFault, emergencyRecordCode, "LED IC Excess Temperature");
                     }
                  }
                  else if (0x8311 == errorCode)
                  {
                     if (3 == subSystem)
                     {
                        deviceFault = false;
                        this.PanMotor.RecordError(componentFault, emergencyRecordCode, "Average Current Error");
                     }
                     else if (4 == subSystem)
                     {
                        deviceFault = false;
                        this.TiltMotor.RecordError(componentFault, emergencyRecordCode, "Average Current Error");
                     }
                  }
                  else if (0x8331 == errorCode)
                  {
                     if (3 == subSystem)
                     {
                        deviceFault = false;
                        this.PanMotor.RecordError(componentFault, emergencyRecordCode, "Peak Current Error");
                     }
                     else if (4 == subSystem)
                     {
                        deviceFault = false;
                        this.TiltMotor.RecordError(componentFault, emergencyRecordCode, "Peak Current Error");
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
               if ((null != msg) && (msg.Length >= 8))
               {
                  this.PanMotor.Status = BitConverter.ToUInt16(msg, 0);
                  this.PanMotor.ActualCurrent = BitConverter.ToInt16(msg, 2);
                  this.TiltMotor.Status = BitConverter.ToUInt16(msg, 4);
                  this.TiltMotor.ActualCurrent = BitConverter.ToInt16(msg, 6);

                  this.PanMotor.ProcessStatus();
                  this.TiltMotor.ProcessStatus();
               }
            }
            else if (COBTypes.TPDO2 == frameType)
            {
               if ((null != msg) && (msg.Length >= 8))
               {
                  this.PanMotor.ActualVelocity = BitConverter.ToInt32(msg, 0);
                  this.TiltMotor.ActualVelocity = BitConverter.ToInt32(msg, 4);
               }
            }
            else if (COBTypes.TPDO3 == frameType)
            {
               if ((null != msg) && (msg.Length >= 8))
               {
                  this.PanMotor.ActualPosition = BitConverter.ToInt32(msg, 0);
                  this.TiltMotor.ActualPosition = BitConverter.ToInt32(msg, 4);
               }
            }
            else if (COBTypes.TPDO4 == frameType)
            {
               if ((null != msg) && (msg.Length >= 3))
               {
                  this.McuTemperature = this.GetSignedTemperature(msg[0]);
                  this.PanMotor.Temperature = this.GetSignedTemperature(msg[1]);
                  this.TiltMotor.Temperature = this.GetSignedTemperature(msg[2]);
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

      public UlcRoboticsWecoHub(string name, byte nodeId)
         : base(name, nodeId)
      {
         #region Camera/LED Initialization

         this.CameraLights.Name = this.Name + " cameralights";
         this.CameraLights.OnCommExchange = new DeviceComponent.CommExchangeHandler(this.ExchangeCommAction);
         this.CameraLights.OnClearErrorCode = new DeviceComponent.ClearErrorCodeHandler(this.ClearErrorCode);

         #endregion

         #region Pan Motor Initialization

         this.PanMotor.Name = this.Name + " pan motor";

         this.PanMotor.OnCommExchange = new DeviceComponent.CommExchangeHandler(this.ExchangeCommAction);
         this.PanMotor.OnClearErrorCode = new DeviceComponent.ClearErrorCodeHandler(this.ClearErrorCode);
         this.PanMotor.OnTargetPositionSchedule = new MotorComponent.TargetPositionScheduleHandler(this.EmitRpo3);
         this.PanMotor.OnTargetVelocitySchedule = new MotorComponent.TargetVelocityScheduleHandler(this.EmitRpo2);

         this.PanMotor.MotorPeakCurrentLimitLocation = 0x00641003;

         this.PanMotor.ControlWordLocation = 0x00604000;
         this.PanMotor.SetOperationalModeLocation = 0x00606000;

         this.PanMotor.PolarityLocation = 0x00607E00;
         this.PanMotor.PositionNotationIndexLocation = 0x00608900;
         this.PanMotor.VelocityNotationIndexLocation = 0x00608B00;
         this.PanMotor.VelocityDimensionIndexLocation = 0x00608C00;
         this.PanMotor.AccelerationNotationIndexLocation = 0x00608D00;
         this.PanMotor.AccelerationDimensionIndexLocation = 0x00608E00;
         this.PanMotor.PositionEncoderIncrementsLocation = 0x00608F01;
         this.PanMotor.PositionEncoderMotorRevolutionsLocation = 0x00608F02;
         this.PanMotor.VelocityEncoderIncrementsPerSecondLocation = 0x00609001;
         this.PanMotor.VelocityEncoderMotorRevolutionsPerSecondLocation = 0x00609002;
         this.PanMotor.GearRatioMotorRevolutionsLocation = 0x00609101;
         this.PanMotor.GearRatioShaftRevolutionsLocation = 0x00609102;
         this.PanMotor.FeedConstantFeedLocation = 0x00609201;
         this.PanMotor.FeedConstantShaftRevolutionsLocation = 0x00609202;

         this.PanMotor.TargetPositionLocation = 0x00607A00;
         this.PanMotor.ProfileVelocityLocation = 0x00608100;
         this.PanMotor.ProfileAccelerationLocation = 0x00608300;
         this.PanMotor.ProfileDecelerationLocation = 0x00608400;

         this.PanMotor.PositionWindowLocation = 0x00606700;
         this.PanMotor.PositionWindowTimeLocation = 0x00606800;
         this.PanMotor.PositionKpLocation = 0x0060FB01;
         this.PanMotor.PositionKiLocation = 0x0060FB02;
         this.PanMotor.PositionKdLocation = 0x0060FB03;

         this.PanMotor.MaximumCurrentLocation = 0x00607300;
         this.PanMotor.MotorRatedCurrentLocation = 0x00607500;

         this.PanMotor.VelocityWindowLocation = 0x00606D00;
         this.PanMotor.VelocityWindowTimeLocation = 0x00606E00;
         this.PanMotor.VelocityThresholdLocation = 0x00606F00;
         this.PanMotor.VelocityThresholdTimeLocation = 0x00607000;
         this.PanMotor.VelocityKpLocation = 0x0060F901;
         this.PanMotor.VelocityKiLocation = 0x0060F902;
         this.PanMotor.VelocityKdLocation = 0x0060F903;
         this.PanMotor.TargetVelocityLocation = 0x0060FF00;

         #endregion

         #region Tilt Motor Initialization

         this.TiltMotor.Name = this.Name + " tilt motor";

         this.TiltMotor.OnCommExchange = new DeviceComponent.CommExchangeHandler(this.ExchangeCommAction);
         this.TiltMotor.OnClearErrorCode = new DeviceComponent.ClearErrorCodeHandler(this.ClearErrorCode);
         this.TiltMotor.OnTargetPositionSchedule = new MotorComponent.TargetPositionScheduleHandler(this.EmitRpo3);
         this.TiltMotor.OnTargetVelocitySchedule = new MotorComponent.TargetVelocityScheduleHandler(this.EmitRpo2);

         this.TiltMotor.MotorPeakCurrentLimitLocation = 0x006C1003;

         this.TiltMotor.ControlWordLocation = 0x00684000;
         this.TiltMotor.SetOperationalModeLocation = 0x00686000;

         this.TiltMotor.PolarityLocation = 0x00687E00;
         this.TiltMotor.PositionNotationIndexLocation = 0x00688900;
         this.TiltMotor.VelocityNotationIndexLocation = 0x00688B00;
         this.TiltMotor.VelocityDimensionIndexLocation = 0x00688C00;
         this.TiltMotor.AccelerationNotationIndexLocation = 0x00688D00;
         this.TiltMotor.AccelerationDimensionIndexLocation = 0x00688E00;
         this.TiltMotor.PositionEncoderIncrementsLocation = 0x00688F01;
         this.TiltMotor.PositionEncoderMotorRevolutionsLocation = 0x00688F02;
         this.TiltMotor.VelocityEncoderIncrementsPerSecondLocation = 0x00689001;
         this.TiltMotor.VelocityEncoderMotorRevolutionsPerSecondLocation = 0x00689002;
         this.TiltMotor.GearRatioMotorRevolutionsLocation = 0x00689101;
         this.TiltMotor.GearRatioShaftRevolutionsLocation = 0x00689102;
         this.TiltMotor.FeedConstantFeedLocation = 0x00689201;
         this.TiltMotor.FeedConstantShaftRevolutionsLocation = 0x00689202;

         this.TiltMotor.TargetPositionLocation = 0x00687A00;
         this.TiltMotor.ProfileVelocityLocation = 0x00688100;
         this.TiltMotor.ProfileAccelerationLocation = 0x00688300;
         this.TiltMotor.ProfileDecelerationLocation = 0x00688400;

         this.TiltMotor.PositionWindowLocation = 0x00686700;
         this.TiltMotor.PositionWindowTimeLocation = 0x00686800;
         this.TiltMotor.PositionKpLocation = 0x0068FB01;
         this.TiltMotor.PositionKiLocation = 0x0068FB02;
         this.TiltMotor.PositionKdLocation = 0x0068FB03;

         this.TiltMotor.MaximumCurrentLocation = 0x00687300;
         this.TiltMotor.MotorRatedCurrentLocation = 0x00687500;

         this.TiltMotor.VelocityWindowLocation = 0x00686D00;
         this.TiltMotor.VelocityWindowTimeLocation = 0x00686E00;
         this.TiltMotor.VelocityThresholdLocation = 0x00686F00;
         this.TiltMotor.VelocityThresholdTimeLocation = 0x00687000;
         this.TiltMotor.VelocityKpLocation = 0x0068F901;
         this.TiltMotor.VelocityKiLocation = 0x0068F902;
         this.TiltMotor.VelocityKdLocation = 0x0068F903;
         this.TiltMotor.TargetVelocityLocation = 0x0068FF00;

         #endregion
      }

      #endregion

      #region Access Methods

      #region General Functions

      public override void Initialize()
      {
         this.CameraLights.Initialize();
         this.PanMotor.Initialize();
         this.TiltMotor.Initialize();
         
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
            result &= this.SetTPDOMap(1, 1, 0x6041, 0x00, 2); // pan motor status word
            result &= this.SetTPDOMap(1, 2, 0x6078, 0x00, 2); // pan motor current actual
            result &= this.SetTPDOMap(1, 3, 0x6841, 0x00, 2); // tilt motor status word
            result &= this.SetTPDOMap(1, 4, 0x6878, 0x00, 2); // tilt motor current actual
            result &= this.SetTPDOMapCount(1, 4);
            result &= this.SetTPDOEnable(1, true);

            // set TPDO2 on change with 200mS inhibit time
            result &= this.SetTPDOEnable(2, false);
            result &= this.SetTPDOMapCount(2, 0);
            result &= this.SetTPDOType(2, 254);
            result &= this.SetTPDOInhibitTime(2, 200);
            result &= this.SetTPDOMap(2, 1, 0x606C, 0x00, 4); // pan motor velocity actual value
            result &= this.SetTPDOMap(2, 2, 0x686C, 0x00, 4); // tilt motor velocity actual value
            result &= this.SetTPDOMapCount(2, 2);
            result &= this.SetTPDOEnable(2, true);

            // set TPDO3 on change with 200mS inhibit time
            result &= this.SetTPDOEnable(3, false);
            result &= this.SetTPDOMapCount(3, 0);
            result &= this.SetTPDOType(3, 254);
            result &= this.SetTPDOInhibitTime(3, 200);
            result &= this.SetTPDOMap(3, 1, 0x6064, 0x00, 4); // pan motor position actual value
            result &= this.SetTPDOMap(3, 2, 0x6864, 0x00, 4); // tilt motor position actual value
            result &= this.SetTPDOMapCount(3, 2);
            result &= this.SetTPDOEnable(3, true);

            // set TPDO4 on change with 200mS inhibit time
            result &= this.SetTPDOEnable(4, false);
            result &= this.SetTPDOMapCount(4, 0);
            result &= this.SetTPDOType(4, 254);
            result &= this.SetTPDOInhibitTime(4, 200);
            result &= this.SetTPDOMap(4, 1, 0x2311, 0x01, 1); // MCU temperature
            result &= this.SetTPDOMap(4, 2, 0x6410, 0x01, 1); // pan motor temperature
            result &= this.SetTPDOMap(4, 3, 0x6C10, 0x01, 1); // tilt motor temperature
            result &= this.SetTPDOMapCount(4, 3);
            result &= this.SetTPDOEnable(4, true);


            // set RPDO1 every SYNC
            result &= this.SetRPDOEnable(1, false);
            result &= this.SetRPDOMapCount(1, 0);
            result &= this.SetRPDOType(1, 1);
            result &= this.SetRPDOMap(1, 1, 0x6071, 0, 2); // pan motor target torque
            result &= this.SetRPDOMap(1, 2, 0x6871, 0, 2); // tilt motor target torque
            result &= this.SetRPDOMapCount(1, 2);
            result &= this.SetRPDOEnable(1, true);

            // set RPDO2 every SYNC
            result &= this.SetRPDOEnable(2, false);
            result &= this.SetRPDOMapCount(2, 0);
            result &= this.SetRPDOType(2, 1);
            result &= this.SetRPDOMap(2, 1, 0x60FF, 0, 4); // pan motor target velocity
            result &= this.SetRPDOMap(2, 2, 0x68FF, 0, 4); // tilt motor target velocity
            result &= this.SetRPDOMapCount(2, 2);
            result &= this.SetRPDOEnable(2, true);

            // set RPDO3 every SYNC
            result &= this.SetRPDOEnable(3, false);
            result &= this.SetRPDOMapCount(3, 0);
            result &= this.SetRPDOType(3, 1);
            result &= this.SetRPDOMap(3, 1, 0x607A, 0, 4); // pan motor target position
            result &= this.SetRPDOMap(3, 2, 0x687A, 0, 4); // tilt motor target position
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

         this.CameraLights.Reset();
         this.PanMotor.Reset();
         this.TiltMotor.Reset();
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
         this.CameraLights.SetFault("device offline");
         this.PanMotor.SetFault("device offline");
         this.TiltMotor.SetFault("device offline");

         base.SetFault(faultReason, resetDevice);
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