namespace NICBOT.CAN
{
   using System;
   using System.Text;
   using System.Threading;

   public class UlcRoboticsNicbotBody : Device
   {
      #region Definition

      public enum Modes
      {
         unknown,
         repair,
         inspect,
      }
      
      #endregion

      #region Field

      private Modes mode;
      private bool active;

      private UInt16 controlWord;
      private UInt16 solenoidCache;

      #endregion

      #region Helper Functions

      private bool SetControlWord(UInt16 adjustedValue)
      {
         bool result = false;

         if (adjustedValue != this.controlWord)
         {
            this.pendingAction = new SDODownload(0x2500, 0, 2, adjustedValue);
            result = this.ExchangeCommAction(this.pendingAction);

            if (false != result)
            {
               this.controlWord = adjustedValue;
            }
         }
         else
         {
            result = true;
         }

         return (result);
      }

      private void ProcessDeviceStatus(UInt16 statusValue)
      {
         this.LastCircumferential = ((statusValue & 0x0040) != 0) ? true : false;
         this.LastAxial = ((statusValue & 0x0080) != 0) ? true : false;
         this.TopFrontReadyToLock = ((statusValue & 0x8000) != 0) ? true : false;
         this.TopRearReadyToLock = ((statusValue & 0x2000) != 0) ? true : false;
         this.BottomFrontReadyToLock = ((statusValue & 0x1000) != 0) ? true : false;
         this.BottomRearReadyToLock = ((statusValue & 0x4000) != 0) ? true : false;

         if (Modes.repair == this.mode)
         {
            this.FrontDrillError = ((statusValue & 0x0001) != 0) ? true : false;
            this.FrontDrillComplete = ((statusValue & 0x0002) != 0) ? true : false;

            this.RearDrillError = ((statusValue & 0x0008) != 0) ? true : false;
            this.RearDrillComplete = ((statusValue & 0x0010) != 0) ? true : false;

            this.RearDrillAtRetractionLimit = ((statusValue & 0x0100) != 0) ? true : false;
            this.RearDrillAtExtensionLimit = ((statusValue & 0x0200) != 0) ? true : false;
            this.FrontDrillAtRetractionLimit = ((statusValue & 0x0400) != 0) ? true : false;
            this.FrontDrillAtExtensionLimit = ((statusValue & 0x0800) != 0) ? true : false;
         }
         else if (Modes.inspect == this.mode)
         {
        }
      }

      private bool ReadDeviceStatus()
      {
         UInt16 deviceStatus = 0;
         bool result = this.Read(0x2501, 0, ref deviceStatus);

         if (false != result)
         {
            this.ProcessDeviceStatus(deviceStatus);
         }

         return (result);
      }

      #endregion

      #region Properties

      public Modes Mode { get { return (this.mode); } }

      public bool FrontDrillError { set; get; }
      public bool FrontDrillComplete { set; get; }
      public bool FrontDrillAtRetractionLimit { set; get; }
      public bool FrontDrillAtExtensionLimit { set; get; }
      
      public bool RearDrillError { set; get; }
      public bool RearDrillComplete { set; get; }
      public bool RearDrillAtRetractionLimit { set; get; }
      public bool RearDrillAtExtensionLimit { set; get; }

#if false      
      public bool AutoDrillOriginFound { set; get; }
      public bool AutoDrillCutComplete { set; get; }
      public bool AutoDrillOriginHunting { set; get; }
      public bool AutoDrillCutRunning { set; get; }
      public bool AutoDrillCutPaused { set; get; }
#endif

      public bool TopFrontReadyToLock { set; get; }
      public bool TopRearReadyToLock { set; get; }
      public bool BottomFrontReadyToLock { set; get; }
      public bool BottomRearReadyToLock { set; get; }
      public bool LastCircumferential { set; get; }
      public bool LastAxial { set; get; }
      
      public double FrontDrillSpeed { set; get; }
      public double FrontDrillIndex { set; get; }

      public double RearDrillSpeed { set; get; }
      public double RearDrillIndex { set; get; }

      public double AccelerometerX { set; get; }
      public double AccelerometerY { set; get; }
      public double AccelerometerZ { set; get; }

      public double Roll { set; get; }
      public double Pitch { set; get; }

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
               UInt64 errorCode = 0;

               if ((null != msg) && (8 >= msg.Length))
               {
                  errorCode = BitConverter.ToUInt64(msg, 0);
               }

               if (0 != errorCode)
               {
                  string faultReasong = string.Format("emergency {0:X16}", errorCode);
                  this.Fault(faultReasong);
               }
            }
            else if (COBTypes.TPDO1 == frameType)
            {
               if ((null != msg) && (msg.Length >= 4))
               {
                  Int16 frontValue = BitConverter.ToInt16(msg, 0);
                  Int16 rearValue = BitConverter.ToInt16(msg, 2);
                  this.FrontDrillSpeed = frontValue;
                  this.RearDrillSpeed = rearValue;
               }
            }
            else if (COBTypes.TPDO2 == frameType)
            {
               if ((null != msg) && (msg.Length >= 4))
               {
                  Int16 frontValue = BitConverter.ToInt16(msg, 0);
                  Int16 rearValue = BitConverter.ToInt16(msg, 2);
                  this.FrontDrillIndex = (double)frontValue / 10;
                  this.RearDrillIndex = (double)rearValue / 10;
               }
            }
            else if (COBTypes.TPDO3 == frameType)
            {
            }
            else if (COBTypes.TPDO4 == frameType)
            {
               if ((null != msg) && (msg.Length >= 8))
               {
                  UInt16 statusValue = BitConverter.ToUInt16(msg, 0);
                  Int16 accelerometerX = BitConverter.ToInt16(msg, 2);
                  Int16 accelerometerY = BitConverter.ToInt16(msg, 4);
                  Int16 accelerometerZ = BitConverter.ToInt16(msg, 6);

                  this.AccelerometerX = accelerometerX;
                  this.AccelerometerY = accelerometerY;
                  this.AccelerometerZ = accelerometerZ;

                  double x2 = accelerometerX * accelerometerX;
                  double y2 = accelerometerY * accelerometerY;
                  double z2 = accelerometerZ * accelerometerZ;

                  this.ProcessDeviceStatus(statusValue);

                  if (Modes.repair == this.mode)
                  {
                     this.Pitch = Math.Atan2(-accelerometerX, Math.Sqrt(y2 + z2)) * 180 / Math.PI;

                     double rollValue = (Math.Atan2(accelerometerY, accelerometerZ) * 180 / Math.PI);

                     if (rollValue < 0)
                     {
                        rollValue = 360 + rollValue;
                     }

                     this.Roll = rollValue;
                  }
                  else if (Modes.inspect == this.mode)
                  {
                     this.Pitch = Math.Atan2(accelerometerY, Math.Sqrt(x2 + z2)) * 180 / Math.PI;

                     double rollValue = (Math.Atan2(accelerometerX, accelerometerZ) * 180 / Math.PI);

                     if (rollValue >= 90)
                     {
                        rollValue -= 90;
                     }
                     else
                     {
                        rollValue += 270;
                     }

                     this.Roll = rollValue;
                  }
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

      public UlcRoboticsNicbotBody(string name, byte nodeId)
         : base(name, nodeId)
      {
      }

      #endregion

      #region Access Methods

      public override void Initialize()
      {
         base.Initialize();
      }

      public bool SetDeviceBaudRate(int rate)
      {
         UInt32 rateCode = 0;

         if (10000 == rate)
         {
            rateCode = 0;
         }
         else if (20000 == rate)
         {
            rateCode = 1;
         }
         else if (50000 == rate)
         {
            rateCode = 2;
         }
         else if (100000 == rate)
         {
            rateCode = 3;
         }
         else if (125000 == rate)
         {
            rateCode = 4;
         }
         else if (250000 == rate)
         {
            rateCode = 5;
         }
         else if (500000 == rate)
         {
            rateCode = 6;
         }
         else if (1000000 == rate)
         {
            rateCode = 7;
         }

         this.pendingAction = new SDODownload(0x2100, 0, 1, rateCode);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      public bool SetDeviceNodeId(byte nodeId)
      {
         this.pendingAction = new SDODownload(0x2101, 0, 1, nodeId);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      public bool SetDeviceMode(byte mode)
      {
         this.pendingAction = new SDODownload(0x2102, 0, 1, mode);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      public bool SetVideoASelect(byte select)
      {
         this.pendingAction = new SDODownload(0x2301, 1, 1, select);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      public bool SetVideoBSelect(byte select)
      {
         this.pendingAction = new SDODownload(0x2301, 2, 1, select);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      public bool SetCameraLightLevelt(byte cameraId, byte lightLevel)
      {
         this.pendingAction = new SDODownload(0x2303, cameraId, 1, lightLevel);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      public bool SetSolenoids(UInt16 solenoidControl)
      {
         this.pendingAction = new SDODownload(0x2304, 0, 2, solenoidControl);
         bool result = this.ExchangeCommAction(this.pendingAction);

         if (false != result)
         {
            this.solenoidCache = solenoidControl;
         }

         return (result);
      }

      public bool SetDrillServoAcceleration(UInt32 acceleration)
      {
         bool result = false;

         if (Modes.repair == this.mode)
         {
            SDODownload download = new SDODownload(0x2340, 0, 4, acceleration);
            result = this.ExchangeCommAction(download);
         }

         return (result);
      }

      public bool SetDrillServoProportionalControlConstant(UInt32 proportionalControlConstant)
      {
         bool result = false;

         if (Modes.repair == this.mode)
         {
            SDODownload download = new SDODownload(0x233D, 0, 4, proportionalControlConstant);
            result = this.ExchangeCommAction(download);
         }

         return (result);
      }

      public bool SetDrillServoIntegralControlConstant(UInt32 integralControlConstant)
      {
         bool result = false;

         if (Modes.repair == this.mode)
         {
            SDODownload download = new SDODownload(0x233E, 0, 4, integralControlConstant);
            result = this.ExchangeCommAction(download);
         }

         return (result);
      }

      public bool SetDrillServoDerivativeControlConstant(UInt32 derivativeControlConstant)
      {
         bool result = false;

         if (Modes.repair == this.mode)
         {
            SDODownload download = new SDODownload(0x233F, 0, 4, derivativeControlConstant);
            result = this.ExchangeCommAction(download);
         }

         return (result);
      }

      public bool SetDrillServoHomingVelocity(UInt32 homingVelocity)
      {
         bool result = false;

         if (Modes.repair == this.mode)
         {
            SDODownload download = new SDODownload(0x2341, 0, 4, homingVelocity);
            result = this.ExchangeCommAction(download);
         }

         return (result);
      }

      public bool SetDrillServoHomingBackoffCount(UInt32 backoffCount)
      {
         bool result = false;

         if (Modes.repair == this.mode)
         {
            SDODownload download = new SDODownload(0x2342, 0, 4, backoffCount);
            result = this.ExchangeCommAction(download);
         }

         return (result);
      }

      public bool SetDrillServoTravelVelocity(UInt32 travelVelocity)
      {
         bool result = false;

         if (Modes.repair == this.mode)
         {
            SDODownload download = new SDODownload(0x2343, 0, 4, travelVelocity);
            result = this.ExchangeCommAction(download);
         }

         return (result);
      }

      public bool SetDrillServoErrorLimit(UInt16 errorLimit)
      {
         bool result = false;

         if (Modes.repair == this.mode)
         {
            SDODownload download = new SDODownload(0x2344, 0, 2, errorLimit);
            result = this.ExchangeCommAction(download);
         }

         return (result);
      }

      public bool SetDrillServoPulsesPerUnit(UInt32 pulsesPerUnit)
      {
         bool result = false;

         if (Modes.repair == this.mode)
         {
            SDODownload download = new SDODownload(0x2345, 0, 4, pulsesPerUnit);
            result = this.ExchangeCommAction(download);
         }

         return (result);
      }

      public bool SetFrontDrillSpeed(double drillSpeedRpm)
      {
         Int16 setPoint = (Int16)drillSpeedRpm;
         this.pendingAction = new SDODownload(0x2311, 0, 2, (UInt32)setPoint);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      public bool StopFrontDrill()
      {
         bool result = false;

         if (Modes.repair == this.mode)
         {
            result = true;
            UInt16 adjustedValue = this.controlWord;

            adjustedValue &= (UInt16)(0xFF01);
            result = this.ExchangeCommAction(new SDODownload(0x2500, 0, 2, adjustedValue));

            adjustedValue |= (UInt16)(0x0002);
            result = this.ExchangeCommAction(new SDODownload(0x2500, 0, 2, adjustedValue));
         }

         return (result);
      }
      
      /// <summary>
      /// Function to set front drill index.
      /// </summary>
      /// <param name="drillIndex">mm</param>
      /// <returns>true for success, false otherwise</returns>
      public bool SetFrontDrillIndex(double drillIndex)
      {
         Int16 setPoint = (Int16)(drillIndex * 10);
         this.pendingAction = new SDODownload(0x2312, 0, 2, (UInt32)setPoint);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }      

      public bool SetFrontDrillRetractToLimit()
      {
         bool result = false;

         if (Modes.repair == this.mode)
         {
            result = true;
            UInt16 adjustedValue = this.controlWord;

            adjustedValue &= (UInt16)(0xFF01);
            result = this.ExchangeCommAction(new SDODownload(0x2500, 0, 2, adjustedValue));

            adjustedValue |= (UInt16)(0x0004);
            result = this.ExchangeCommAction(new SDODownload(0x2500, 0, 2, adjustedValue));
         }

         return (result);
      }

      public bool SetFrontDrillMoveToOrigin()
      {
         bool result = false;

         if (Modes.repair == this.mode)
         {
            result = true;
            UInt16 adjustedValue = this.controlWord;

            adjustedValue &= (UInt16)(0xFF01);
            result = this.ExchangeCommAction(new SDODownload(0x2500, 0, 2, adjustedValue));

            adjustedValue |= (UInt16)(0x0008);
            result = this.ExchangeCommAction(new SDODownload(0x2500, 0, 2, adjustedValue));
         }

         return (result);
      }      

      public bool SetRearDrillSpeed(double drillSpeedRpm)
      {
         Int16 setPoint = (Int16)drillSpeedRpm;
         this.pendingAction = new SDODownload(0x2313, 0, 2, (UInt32)setPoint);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      public bool StopRearDrill()
      {
         bool result = false;

         if (Modes.repair == this.mode)
         {
            result = true;
            UInt16 adjustedValue = this.controlWord;
            
            adjustedValue &= (UInt16)(0x01FF);
            result = this.ExchangeCommAction(new SDODownload(0x2500, 0, 2, adjustedValue));

            adjustedValue |= (UInt16)(0x0200);
            result = this.ExchangeCommAction(new SDODownload(0x2500, 0, 2, adjustedValue));
         }

         return (result);
      }

      /// <summary>
      /// Function to set rear drill index.
      /// </summary>
      /// <param name="drillIndex">mm</param>
      /// <returns>true for success, false otherwise</returns>
      public bool SetRearDrillIndex(double drillIndex)
      {
         Int16 setPoint = (Int16)(drillIndex * 10);
         this.pendingAction = new SDODownload(0x2314, 0, 2, (UInt32)setPoint);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      public bool SetRearDrillRetractToLimit()
      {
         bool result = false;

         if (Modes.repair == this.mode)
         {
            result = true;
            UInt16 adjustedValue = this.controlWord;

            adjustedValue &= (UInt16)(0x01FF);
            result = this.ExchangeCommAction(new SDODownload(0x2500, 0, 2, adjustedValue));

            adjustedValue |= (UInt16)(0x0400);
            result = this.ExchangeCommAction(new SDODownload(0x2500, 0, 2, adjustedValue));
         }

         return (result);
      }

      public bool SetRearDrillMoveToOrigin()
      {
         bool result = false;

         if (Modes.repair == this.mode)
         {
            result = true;
            UInt16 adjustedValue = this.controlWord;

            adjustedValue &= (UInt16)(0x01FF);
            result = this.ExchangeCommAction(new SDODownload(0x2500, 0, 2, adjustedValue));

            adjustedValue |= (UInt16)(0x0800);
            result = this.ExchangeCommAction(new SDODownload(0x2500, 0, 2, adjustedValue));
         }

         return (result);
      }      

      public bool SetFrontLaser(bool state)
      {
         bool result = false;

         if (Modes.repair == this.mode)
         {
            UInt16 adjustedValue;

            if (false != state)
            {
               adjustedValue = (UInt16)(this.controlWord | 0x0001);
            }
            else
            {
               adjustedValue = (UInt16)(this.controlWord & ~0x0001);
            }

            result = this.SetControlWord(adjustedValue);
         }

         return (result);
      }

      public bool SetRearLaser(bool state)
      {
         bool result = false;

         if (Modes.repair == this.mode)
         {
            UInt16 adjustedValue;

            if (false != state)
            {
               adjustedValue = (UInt16)(this.controlWord | 0x0100);
            }
            else
            {
               adjustedValue = (UInt16)(this.controlWord & ~0x0100);
            }

            result = this.SetControlWord(adjustedValue);
         }

         return (result);
      }

      public bool SetAutoDrillParameters(bool autoOrign, bool peckMode, bool distancePosition, double searchSpeed, double travelSpeed, double rotationSpeed, double cuttingSpeed, double cuttingDepth, double peckIncrement, double retractionDistace, double retractionPosition)
      {
         bool result = false;

         if (Modes.repair == this.mode)
         {
            result = true;

            byte drillControl = 0;
            drillControl |= (byte)((false != peckMode) ? 0x01 : 0);
            drillControl |= (byte)((false != autoOrign) ? 0x10 : 0);
            drillControl |= (byte)((false != distancePosition) ? 0x20 : 0);
            result &= this.ExchangeCommAction(new SDODownload(0x2331, 1, 1, drillControl));

            UInt16 searchSpeedValue = (UInt16)(searchSpeed * 10);
            result &= this.ExchangeCommAction(new SDODownload(0x2331, 2, 2, searchSpeedValue));

            UInt16 travelSpeedValue = (UInt16)(travelSpeed * 10);
            result &= this.ExchangeCommAction(new SDODownload(0x2331, 3, 2, travelSpeedValue));

            UInt16 rotationSpeedValue = (UInt16)(rotationSpeed * 1);
            result &= this.ExchangeCommAction(new SDODownload(0x2331, 4, 2, rotationSpeedValue));

            UInt16 cuttingSpeedValue = (UInt16)(cuttingSpeed * 10);
            result &= this.ExchangeCommAction(new SDODownload(0x2331, 5, 2, cuttingSpeedValue));

            UInt16 cuttingDepthValue = (UInt16)(cuttingDepth * 10);
            result &= this.ExchangeCommAction(new SDODownload(0x2331, 6, 2, cuttingDepthValue));

            UInt16 peckIncrementValue = (UInt16)(peckIncrement * 10);
            result &= this.ExchangeCommAction(new SDODownload(0x2331, 7, 2, peckIncrementValue));

            UInt16 retractionDistaceValue = (UInt16)(retractionDistace * 10);
            result &= this.ExchangeCommAction(new SDODownload(0x2331, 8, 2, retractionDistaceValue));

            UInt16 retractionPositionValue = (UInt16)(retractionPosition * 10);
            result &= this.ExchangeCommAction(new SDODownload(0x2331, 9, 2, retractionPositionValue));
         }

         return (result);
      }

      public bool SetAutoDrillOriginHunt(bool state)
      {
         bool result = false;

         if (Modes.repair == this.mode)
         {
            UInt16 adjustedValue;

            if (false != state)
            {
               adjustedValue = (UInt16)(this.controlWord | 0x0002);
            }
            else
            {
               adjustedValue = (UInt16)(this.controlWord & ~0x0002);
            }

            result = this.SetControlWord(adjustedValue);
         }

         return (result);
      }

      public bool SetAutoDrillRunning(bool state)
      {
         bool result = false;

         if (Modes.repair == this.mode)
         {
            UInt16 adjustedValue;

            if (false != state)
            {
               adjustedValue = (UInt16)(this.controlWord | 0x0004);
            }
            else
            {
               adjustedValue = (UInt16)(this.controlWord & ~0x0004);
            }

            result = this.SetControlWord(adjustedValue);
         }

         return (result);
      }

      public bool SetAutoDrillPause(bool state)
      {
         bool result = false;

         if (Modes.repair == this.mode)
         {
            UInt16 adjustedValue;

            if (false != state)
            {
               adjustedValue = (UInt16)(this.controlWord | 0x0008);
            }
            else
            {
               adjustedValue = (UInt16)(this.controlWord & ~0x0008);
            }

            result = this.SetControlWord(adjustedValue);
         }

         return (result);
      }

      public bool SaveConfiguration()
      {
         this.pendingAction = new SDODownload(0x2105, 0, 4, 0x65766173);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      public int GetDeviceBaudRate()
      {
         int result = 0;
         SDOUpload upload = new SDOUpload(0x2100, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if (false != actionResult)
         {
            if (null != upload.Data)
            {
               byte baudCode = upload.Data[0];

               if (0 == baudCode)
               {
                  result = 10000;
               }
               else if (1 == baudCode)
               {
                  result = 20000;
               }
               else if (2 == baudCode)
               {
                  result = 50000;
               }
               else if (3 == baudCode)
               {
                  result = 100000;
               }
               else if (4 == baudCode)
               {
                  result = 125000;
               }
               else if (5 == baudCode)
               {
                  result = 250000;
               }
               else if (6 == baudCode)
               {
                  result = 500000;
               }
               else if (7 == baudCode)
               {
                  result = 1000000;
               }
            }
         }

         return (result);
      }

      public bool GetDeviceNodeId(ref byte deviceId)
      {
         SDOUpload upload = new SDOUpload(0x2101, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if (false != actionResult)
         {
            if (null != upload.Data)
            {
               deviceId = upload.Data[0];
            }
         }

         return (actionResult);
      }

      public bool GetDeviceMode(ref byte mode)
      {
         SDOUpload upload = new SDOUpload(0x2102, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if (false != actionResult)
         {
            if (null != upload.Data)
            {
               mode = upload.Data[0];
            }
         }

         return (actionResult);
      }

      public UInt16 GetSolenoidCache()
      {
         return (this.solenoidCache);
      }

      public bool GetDrillServoAcceleration(ref UInt32 acceleration)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x2340, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            acceleration = BitConverter.ToUInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool GetDrillServoProportionalControlConstant(ref UInt32 proportionalControlConstant)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x233D, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            proportionalControlConstant = BitConverter.ToUInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool GetDrillServoIntegralControlConstant(ref UInt32 integralControlConstant)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x233E, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            integralControlConstant = BitConverter.ToUInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool GetDrillServoDerivativeControlConstant(ref UInt32 derivativeControlConstant)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x233F, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            derivativeControlConstant = BitConverter.ToUInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool GetDrillServoHomingVelocity(ref UInt32 homingVelocity)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x2341, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            homingVelocity = BitConverter.ToUInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool GetDrillServoHomingBackoffCount(ref UInt32 backoffCount)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x2342, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            backoffCount = BitConverter.ToUInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool GetDrillServoTravelVelocity(ref UInt32 travelVelocity)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x2343, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            travelVelocity = BitConverter.ToUInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool GetDrillServoErrorLimit(ref UInt16 errorLimit)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x2344, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 2))
         {
            errorLimit = BitConverter.ToUInt16(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool GetDrillServoPulsesPerUnit(ref UInt32 pulsesPerUnit)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x2345, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            pulsesPerUnit = BitConverter.ToUInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool GetFrontDrillServoStatus(ref byte status)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x2346, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 1))
         {
            status = upload.Data[0];
            result = true;
         }

         return (result);
      }

      public bool GetRearDrillServoStatus(ref byte status)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x2347, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 1))
         {
            status = upload.Data[0];
            result = true;
         }

         return (result);
      }

      public bool Configure(Modes neededMode)
      {
         bool result = false;

         if (null == this.FaultReason)
         {
            byte modeValue = 0;

            // initialize 
            result = true;
            
            // common configuration
            result &= base.Configure();

            // get mode
            if (this.GetDeviceMode(ref modeValue) != false)
            {
               // evaluate
               if (0 == modeValue)
               {
                  // assign mode
                  this.mode = Modes.repair;

                  // check needed mode
                  if ((neededMode != Modes.unknown) &&
                      (neededMode != Modes.repair))
                  {
                     result = false;
                     this.Fault("mode mismatch");
                  }
                  else
                  {
                     // solenoid 
                     result &= this.Read(0x2304, 0, ref this.solenoidCache);

                     // status
                     result &= this.ReadDeviceStatus();

                     // drill speeds
                     result &= this.SetTPDOEnable(1, false);
                     result &= this.SetTPDOMapCount(1, 0);
                     result &= this.SetTPDOType(1, 254);
                     result &= this.SetTPDOInhibitTime(1, 100);
                     result &= this.SetTPDOMap(1, 1, 0x2411, 0, 2);
                     result &= this.SetTPDOMap(1, 2, 0x2413, 0, 2);
                     result &= this.SetTPDOMapCount(1, 2);
                     result &= this.SetTPDOEnable(1, true);

                     // drill index
                     result &= this.SetTPDOEnable(2, false);
                     result &= this.SetTPDOMapCount(2, 0);
                     result &= this.SetTPDOType(2, 254);
                     result &= this.SetTPDOInhibitTime(2, 100);
                     result &= this.SetTPDOMap(2, 1, 0x2412, 0, 2);
                     result &= this.SetTPDOMap(2, 2, 0x2414, 0, 2);
                     result &= this.SetTPDOMapCount(2, 2);
                     result &= this.SetTPDOEnable(2, true);

                     // accelerometer and status
                     result &= this.SetTPDOEnable(4, false);
                     result &= this.SetTPDOMapCount(4, 0);
                     result &= this.SetTPDOType(4, 254);
                     result &= this.SetTPDOInhibitTime(4, 100);
                     result &= this.SetTPDOMap(4, 1, 0x2501, 0, 2);
                     result &= this.SetTPDOMap(4, 2, 0x2441, 0, 2);
                     result &= this.SetTPDOMap(4, 3, 0x2442, 0, 2);
                     result &= this.SetTPDOMap(4, 4, 0x2443, 0, 2);
                     result &= this.SetTPDOMapCount(4, 4);
                     result &= this.SetTPDOEnable(4, true);
                  }
               }
               else if (1 == modeValue)
               {
                  // assign mode
                  this.mode = Modes.inspect;

                  // check needed mode
                  if ((neededMode != Modes.unknown) &&
                      (neededMode != Modes.inspect))
                  {
                     result = false;
                     this.Fault("mode mismatch");
                  }
                  else
                  {
                     // solenoid 
                     result &= this.Read(0x2304, 0, ref this.solenoidCache);

                     // status
                     result &= this.ReadDeviceStatus();

                     // accelerometer and status
                     result &= this.SetTPDOEnable(4, false);
                     result &= this.SetTPDOMapCount(4, 0);
                     result &= this.SetTPDOType(4, 254);
                     result &= this.SetTPDOInhibitTime(4, 100);
                     result &= this.SetTPDOMap(4, 1, 0x2501, 0, 2);
                     result &= this.SetTPDOMap(4, 2, 0x2441, 0, 2);
                     result &= this.SetTPDOMap(4, 3, 0x2442, 0, 2);
                     result &= this.SetTPDOMap(4, 4, 0x2443, 0, 2);
                     result &= this.SetTPDOMapCount(4, 4);
                     result &= this.SetTPDOEnable(4, true);
                  }
               }
               else
               {
                  this.mode = Modes.unknown;

                  result = false;
                  this.Fault("unsupported mode");
               }
            }
         }

         return (result);
      }

      public override bool Start()
      {
         bool result = false;

         if (null == this.FaultReason)
         {
            if (Modes.unknown != this.Mode)
            {
               result = base.Start();
               this.active = result;
            }
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
         this.controlWord = 0;
         this.mode = Modes.unknown;
      }

      public override void Update()
      {
         if (false != active)
         {
         }

         base.Update();
      }

      #endregion
   }
}