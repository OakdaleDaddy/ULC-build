
namespace CrossBore.CAN
{
   using System;
   using System.Threading;

   using CrossBore.Utilities;

   public class KublerRotaryEncoder : Device
   {
      #region Definition

      private enum EncoderTypes
      {
         unknown,
         singleTurn,
         multiTurn,
      }

      #endregion

      #region Field

      private bool rotationInitialized;
      private UInt16 lastPosition;
      private UInt16 rotationThreshold;
      private UInt32 lastLongPosition;
      private UInt32 rotationLongThreshold;
      private Int32 rotationDelta;
      private Int32 wholeRotations;

      private EncoderTypes encoderType;

      #endregion

      #region Properties

      public double Rotations { set; get; }
      public double Speed { set; get; }
      public UInt32 Position { set; get; }

      #endregion

      #region Helper Functions

      private void UpdateRotations(UInt16 position, Int16 speed)
      {
         this.Speed = speed;
         
         if (false != this.rotationInitialized)
         {
            Int16 delta = (Int16)(position - this.lastPosition);
            this.rotationDelta += delta;

            if (this.rotationDelta > 65535)
            {
               this.wholeRotations++;
               this.rotationDelta -= 65535;
            }
            else if (this.rotationDelta < -65535)
            {
               this.wholeRotations--;
               this.rotationDelta += 65535;
            }

            this.Rotations = this.wholeRotations + ((double)this.rotationDelta / 65535);

            this.Position = (UInt32)(position - this.rotationThreshold);
            this.lastPosition = position;
         }
         else
         {
            this.lastPosition = (UInt16)position;
            this.rotationThreshold = (UInt16)position;

            this.rotationDelta = 0;
            this.wholeRotations = 0;
            this.rotationInitialized = true;
         }
      }

      private void UpdateRotations(UInt32 position, Int16 speed)
      {
         this.Speed = speed;

         if (false != this.rotationInitialized)
         {
            Int32 delta = (Int32)(position - this.lastLongPosition);
            this.rotationDelta += delta;

            //Tracer.WriteHigh(TraceGroup.TBUS, "", "delta {0}", delta);

            if (this.rotationDelta > 65535)
            {
               this.wholeRotations++;
               this.rotationDelta -= 65535;
            }
            else if (this.rotationDelta < -65535)
            {
               this.wholeRotations--;
               this.rotationDelta += 65535;
            }

            this.Rotations = this.wholeRotations + ((double)this.rotationDelta / 65535);

            this.Position = (UInt16)(position - this.rotationLongThreshold);
            this.lastLongPosition = position;
         }
         else
         {
            this.lastLongPosition = (UInt32)position;
            this.rotationLongThreshold = (UInt32)position;

            this.rotationDelta = 0;
            this.wholeRotations = 0;
            this.rotationInitialized = true;

            //Tracer.WriteHigh(TraceGroup.TBUS, "", "position {0}", this.lastLongPosition);
         }
      }


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
                  UInt16 errorCode = BitConverter.ToUInt16(errorMsg, 0);
                  string reason = string.Format("emergency {0:X16}", errorValue);

                  if (0x8110 == errorCode)
                  {
                     reason = "CAN message lost";
                  }
                  else if (0x8120 == errorCode)
                  {
                     reason = "CAN passive error mode";
                  }
                  else if (0x8130 == errorCode)
                  {
                     reason = "controller heartbeat lost";
                  }
                  else if (0x8200 == errorCode)
                  {
                     reason = "unknown network command";
                  }
                  else if (0x8210 == errorCode)
                  {
                     reason = "invalid PDO length";
                  }

                  this.SetFault(reason, false);
               }
            }
            else if (COBTypes.TPDO1 == frameType)
            {
               if (msg.Length >= 6)
               {
                  if (EncoderTypes.singleTurn == this.encoderType)
                  {
                     UInt16 position = BitConverter.ToUInt16(msg, 0);
                     Int16 speed = BitConverter.ToInt16(msg, 4);
                     this.UpdateRotations(position, speed);
                  }
                  else
                  {
                     UInt32 position = BitConverter.ToUInt32(msg, 0);
                     Int16 speed = BitConverter.ToInt16(msg, 4);
                     this.UpdateRotations(position, speed);
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

      public KublerRotaryEncoder(string name, byte nodeId)
         : base(name, nodeId)
      {
      }

      #endregion

      #region Access Methods

      public override void Initialize()
      {
         base.Initialize();

         this.Speed = 0;
         this.Position = 0;
         this.Rotations = 0;

         this.rotationInitialized = false;
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
         this.ScheduleAction(this.pendingAction);

         bool result = this.commEvent.WaitOne(500);
         return (result);
      }

      public int GetDeviceBaudRate()
      {
         int result = 0;
         SDOUpload upload = new SDOUpload(0x2100, 0);

         this.pendingAction = upload;
         this.ScheduleAction(this.pendingAction);

         bool actionResult = this.commEvent.WaitOne(500);

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

      public bool SetDeviceNodeId(byte nodeId)
      {
         this.pendingAction = new SDODownload(0x2101, 0, 1, nodeId);
         this.ScheduleAction(this.pendingAction);

         bool result = this.commEvent.WaitOne(500);
         return (result);
      }

      public byte GetDeviceNodeId()
      {
         byte result = 0;
         SDOUpload upload = new SDOUpload(0x2101, 0);

         this.pendingAction = upload;
         this.ScheduleAction(this.pendingAction);

         bool actionResult = this.commEvent.WaitOne(500);

         if (false != actionResult)
         {
            if (null != upload.Data)
            {
               result = upload.Data[0];
            }
         }

         return (result);
      }

      public bool SaveBusConfiguration()
      {
         this.pendingAction = new SDODownload(0x2105, 0, 4, 0x65766173);
         this.ScheduleAction(this.pendingAction, 200, 2);

         bool result = this.commEvent.WaitOne(500);
         return (result);
      }

      public bool GetDeviceType(ref UInt32 deviceType)
      {
         SDOUpload upload = new SDOUpload(0x1000, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if (false != actionResult)
         {
            if ((null != upload.Data) && (upload.Data.Length >= 4))
            {
               deviceType = BitConverter.ToUInt32(upload.Data, 0);
            }
            else
            {
               actionResult = false;
            }
         }

         return (actionResult);
      }

      public override bool Configure()
      {
         bool result = false;

         if (null == this.FaultReason)
         {
            result = true;

            // common configuration
            result &= base.Configure();

            if (0x00010196 == this.DeviceType)
            {
               this.encoderType = EncoderTypes.singleTurn;

               result = true;
               result &= this.SetTPDOMapCount(1, 0);
               result &= this.SetTPDOType(1, 255);
               result &= this.SetTPDOEventTime(1, 100);
               result &= this.SetTPDOMap(1, 1, 0x6004, 0, 4);
               result &= this.SetTPDOMap(1, 2, 0x6030, 1, 2);
               result &= this.SetTPDOMapCount(1, 2);
            }
            else if (0x00020196 == this.DeviceType)
            {
               this.encoderType = EncoderTypes.multiTurn;

               result = true;
               result &= this.SetTPDOMapCount(1, 0);
               result &= this.SetTPDOType(1, 255);
               result &= this.SetTPDOEventTime(1, 500);
               result &= this.SetTPDOMap(1, 1, 0x6004, 0, 4);
               result &= this.SetTPDOMap(1, 2, 0x6030, 1, 2);
               result &= this.SetTPDOMapCount(1, 2);
            }
            else if (0x000B0196 == this.DeviceType)
            {
               this.encoderType = EncoderTypes.multiTurn;

               result = true;
               result &= this.SetTPDOMapCount(1, 0);

               result &= this.SetTPDOEnable(1, false);
               result &= this.SetTPDOType(1, 254);
               result &= this.SetTPDOInhibitTime(1, 250); // TPDO must be disabled before setting inhibit time
               result &= this.SetTPDOEnable(1, true);

               result &= this.SetTPDOMap(1, 1, 0x6004, 0, 4);
               result &= this.SetTPDOMap(1, 2, 0x6030, 1, 2);
               result &= this.SetTPDOMapCount(1, 2);
            }
            else
            {
               this.encoderType = EncoderTypes.unknown;
               this.SetFault("unsupported encoder", true);
            }
         }

         return (result);
      }

      public override void Reset()
      {
         this.Speed = 0;
         this.Position = 0;
         this.Rotations = 0;

         base.Reset();
      }

      #endregion
   }
}