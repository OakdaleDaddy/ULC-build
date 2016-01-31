namespace DYNO.CAN
{
   using System;
   using System.Text;
   using System.Threading;

   public class UlcRoboticsNicbotWheel : Device
   {
      #region Definition

      public enum Modes
      {
         off,
         current,
         velocity,
         undefined,
      }
      
      #endregion

      #region Fields

      private UInt32 motorCurrentRate;

      private Int32[] velocityCountBuffer;
      private Int16[] torqueCountBuffer;
      //private int sampleIndex;

      #endregion

      #region Properties

      public float Temperature { set; get; }
      public Int32 RPM { set; get; }
      public float Torque { set; get; }

      #endregion

      #region Helper Functions

      private void ResetReadings()
      {
         int bufferSize = this.velocityCountBuffer.Length;

         for (int i = 0; i < bufferSize; i++)
         {
            this.velocityCountBuffer[i] = 0;
            this.torqueCountBuffer[i] = 0;
         }

         //this.sampleIndex = 0;
         this.RPM = 0;
         this.Torque = 0;
      }

      private void ProcessReadings(Int32 velocityCounts, Int16 torqueCounts)
      {
         this.RPM = (Int32)((double)velocityCounts);
         this.Torque = ((float)(torqueCounts * motorCurrentRate) / 1000000);
#if false
         int bufferSize = this.velocityCountBuffer.Length;
         int index = this.sampleIndex++;

         if (this.sampleIndex >= bufferSize)
         {
            this.sampleIndex = 0;
         }

         this.velocityCountBuffer[index] = velocityCounts;
         this.torqueCountBuffer[index] = torqueCounts;

         Int64 velocityCountTotal = 0;
         for (int i = 0; i < bufferSize; i++)
         {
            velocityCountTotal += this.velocityCountBuffer[i];
         }
         Int32 velocityCountAverage = (Int32)(velocityCountTotal / bufferSize);
         this.Velocity = (Int32)((double)velocityCountAverage);

         Int32 torqueCountTotal = 0;
         for (int i = 0; i < bufferSize; i++)
         {
            torqueCountTotal += this.torqueCountBuffer[i];
         }
         Int16 torqueCountAverage = (Int16)(torqueCountTotal / bufferSize);
         this.Torque = ((float)(torqueCountAverage * motorCurrentRate) / 1000000);
#endif
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
               UInt64 errorCode = 0;

               if ((null != msg) && (8 >= msg.Length))
               {
                  errorCode = BitConverter.ToUInt64(msg, 0);
               }

               string faultReasong = string.Format("emergency {0:X16}", errorCode);
               this.Fault(faultReasong);
            }
            else if (COBTypes.TPDO1 == frameType)
            {
               if ((null != msg) && (msg.Length >= 6))
               {
                  Int32 velocityCounts = BitConverter.ToInt32(msg, 0);
                  Int16 torqueCounts = BitConverter.ToInt16(msg, 4);

                  this.ProcessReadings(velocityCounts, torqueCounts);
               }
            }
            else if (COBTypes.TPDO2 == frameType)
            {
               if ((null != msg) && (msg.Length >= 1))
               {
                  byte temperatureCounts = msg[0];
                  this.Temperature = (float)(temperatureCounts * 1.0); 
               }
            }
            else if (COBTypes.TPDO3 == frameType)
            {
            }
            else if (COBTypes.TPDO4 == frameType)
            {
            }
         }
      }

      #endregion

      #region Constructor

      public UlcRoboticsNicbotWheel(string name, byte nodeId)
         : base(name, nodeId)
      {
         this.velocityCountBuffer = new Int32[5];
         this.torqueCountBuffer = new Int16[5];
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
         bool result = this.ExchangeCommAction(new SDODownload(0x2101, 0, 1, nodeId));
         return (result);
      }

      public bool SetDeviceNodeOffset(byte nodeOffset)
      {
         bool result = this.ExchangeCommAction(new SDODownload(0x2102, 0, 1, nodeOffset));
         return (result);
      }

      public bool SetMode(Modes mode)
      {
         bool result = true;

         if (Modes.off == mode)
         {
            result &= this.ExchangeCommAction(new SDODownload(0x6040, 0, 2, (UInt16)0x0));
         }
         else if (Modes.velocity == mode)
         {
            result &= this.ExchangeCommAction(new SDODownload(0x6040, 0, 2, (UInt16)0x6));
            result &= this.ExchangeCommAction(new SDODownload(0x6040, 0, 2, (UInt16)0x7));
            result &= this.ExchangeCommAction(new SDODownload(0x6040, 0, 2, (UInt16)0xF));
            result &= this.ExchangeCommAction(new SDODownload(0x6060, 0, 1, (byte)3));
         }
         else if (Modes.current == mode)
         {
            result &= this.ExchangeCommAction(new SDODownload(0x6040, 0, 2, (UInt16)0x6));
            result &= this.ExchangeCommAction(new SDODownload(0x6040, 0, 2, (UInt16)0x7));
            result &= this.ExchangeCommAction(new SDODownload(0x6040, 0, 2, (UInt16)0xF));
            result &= this.ExchangeCommAction(new SDODownload(0x6060, 0, 1, (byte)4));
         }

         return (result);
      }

      public bool SetVelocity(Int32 velocityRpm)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x60FF, 0, 4, (UInt32)velocityRpm));

         return (result);
      }

      public bool ScheduleVelocity(Int32 velocityRpm)
      {
         bool result = true;

         byte[] data = BitConverter.GetBytes(velocityRpm);
         result = this.ExchangeCommAction(new PDO3Emit(data));

         return (result);
      }

      public bool SetTorque(float current)
      {
         bool result = true;

         if (0 != this.motorCurrentRate)
         {
            Int16 torqueCounts = (Int16)(current * 1000000 / this.motorCurrentRate);
            result &= this.ExchangeCommAction(new SDODownload(0x6071, 0, 2, (UInt32)torqueCounts));
         }
         else
         {
            result = false;
         }

         return (result);
      }

      public bool ScheduleTorque(float current)
      {
         bool result = true;

         if (0 != this.motorCurrentRate)
         {
            Int16 torqueCounts = (Int16)(current * 1000000 / this.motorCurrentRate);
            byte[] data = BitConverter.GetBytes(torqueCounts);
            result = this.ExchangeCommAction(new PDO4Emit(data));
         }
         else
         {
            result = false;
         }

         return (result);
      }

      public bool SaveConfiguration()
      {
         bool result = this.ExchangeCommAction(new SDODownload(0x2105, 0, 4, 0x65766173));
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

      public bool GetDeviceNodeOffset(ref byte nodeOffset)
      {
         SDOUpload upload = new SDOUpload(0x2102, 0);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if (false != actionResult)
         {
            if (null != upload.Data)
            {
               nodeOffset = upload.Data[0];
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

            // set velocity and torque to TPDO1 every 200mS
            result &= this.SetTPDOMapCount(1, 0);
            //result &= this.SetTPDOType(1, 255); // velocity/torque on change does not work, need to poll
            //result &= this.SetTPDOEventTime(1, 200);
            result &= this.SetTPDOType(1, 254);
            result &= this.SetTPDOInhibitTime(1, 100);
            result &= this.SetTPDOMap(1, 1, 0x606c, 0, 4);
            result &= this.SetTPDOMap(1, 2, 0x6077, 0, 2);
            result &= this.SetTPDOMapCount(1, 2);

            // set temperature to RPDO2 on change
            result &= this.SetTPDOMapCount(2, 0);
            result &= this.SetTPDOType(2, 254);
            result &= this.SetTPDOInhibitTime(2, 100);
            result &= this.SetTPDOMap(2, 1, 0x2301, 0, 1);
            result &= this.SetTPDOMapCount(2, 1);

            // set velocity to RPDO3 every SYNC
            result &= this.SetRPDOMapCount(3, 0);
            result &= this.SetRPDOType(3, 1);
            result &= this.SetRPDOMap(3, 1, 0x60FF, 0, 4);
            result &= this.SetRPDOMapCount(3, 1);

            // set torque to RPDO4 every SYNC
            result &= this.SetRPDOMapCount(4, 0);
            result &= this.SetRPDOType(4, 1);
            result &= this.SetRPDOMap(4, 1, 0x6071, 0, 2);
            result &= this.SetRPDOMapCount(4, 1); 

            // read motor current rate
            this.motorCurrentRate = 0;
            result &= this.ReadSDO(0x6075, 0, ref this.motorCurrentRate);

            // set switch on disabled
            result &= this.ExchangeCommAction(new SDODownload(0x6040, 0, 2, (UInt16)0x0));

            // common configuration
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
         }

         return (result);
      }

      public override void Stop()
      {
         base.Stop();
      }

      public override void Reset()
      {
         base.Reset();
      }

      public override void Update()
      {
         base.Update();
      }

      #endregion
   }
}