
namespace NICBOT.CAN
{
   using System;
   using System.Text;
   using System.Threading;

   public class ElmoWhistleMotor : Device
   {
      #region Definition

      public enum ControlModes
      {
         microStepper,
         singleLoopPosition,
      }

      public enum Modes
      {
         off,
         current,
         velocity,
         undefined,
      }

      public delegate void InputChangeHandler(int nodeId, byte value);

      #endregion

      #region Fields

      private byte inputValue;
      private bool initialInputReportNeeded;

      private UInt32 motorCurrentRate;

      private Int32[] velocityCountBuffer;
      private Int16[] torqueCountBuffer;
      private int sampleIndex;

      private Modes mode;

      #endregion

      #region Properties

      public InputChangeHandler OnInputChange { set; get; }

      //public Modes Mode { get { return (this.mode); } }
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

         this.sampleIndex = 0;
         this.RPM = 0;
         this.Torque = 0;
      }

      private void ProcessReadings(Int32 velocityCounts, Int16 torqueCounts)
      {
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
         this.RPM = (Int32)((double)velocityCountAverage * 2.5);

         Int32 torqueCountTotal = 0;
         for (int i = 0; i < bufferSize; i++)
         {
            torqueCountTotal += this.torqueCountBuffer[i];
         }
         Int16 torqueCountAverage = (Int16)(torqueCountTotal / bufferSize);
         this.Torque = ((float)(torqueCountAverage * motorCurrentRate) / 1000000);
      }

      private void ProcessDigitInputs(byte inputValue)
      {
         byte lastInputValue = this.inputValue;
         this.inputValue = inputValue;

         bool reportNeeded = (false != initialInputReportNeeded) || (this.inputValue != lastInputValue);
         this.initialInputReportNeeded = false;

         if ((null != this.OnInputChange) && (false != reportNeeded))
         {
            this.OnInputChange(this.NodeId, this.inputValue);
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
               UInt64 errorCode = 0;

               if ((null != msg) && (8 >= msg.Length))
               {
                  errorCode = BitConverter.ToUInt64(msg, 0);
               }

               string faultReason = string.Format("emergency {0:X16}", errorCode);
               this.Fault(faultReason);
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
            else if (COBTypes.TPDO3 == frameType)
            {
               this.ProcessDigitInputs(msg[2]);
            }
         }
      }

      #endregion

      #region Constructor

      public ElmoWhistleMotor(string name, byte nodeId)
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

         if (false != result)
         {
            this.mode = mode;
         }

         return (result);
      }

      public bool SetVelocity(Int32 velocityRpm)
      {
         bool result = true;

         Int32 velocityCntPerSecond = (Int32)((double)velocityRpm / 2.5);
         result &= this.ExchangeCommAction(new SDODownload(0x60FF, 0, 4, (UInt32)velocityCntPerSecond));

         return (result);
      }

      public bool ScheduleVelocity(Int32 velocityRpm)
      {
         bool result = true;

         Int32 velocityCntPerSecond = (Int32)((double)velocityRpm / 2.5);
         byte[] data = BitConverter.GetBytes(velocityCntPerSecond);
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

      public bool SetControlMode(ControlModes mode)
      {
         int modeValue = -1;
         bool result = false;

         if (ControlModes.microStepper == mode)
         {
            modeValue = 3;
         }
         else if (ControlModes.singleLoopPosition == mode)
         {
            modeValue = 5;
         }

         if (modeValue > 0)
         {
            result = true;
            result &= this.ExchangeCommAction(new BinaryInterpreterSet("MO", 0, 0));
            result &= this.ExchangeCommAction(new BinaryInterpreterSet("UM", 0, modeValue));
            result &= this.ExchangeCommAction(new BinaryInterpreterSet("MO", 0, 1));
         }

         return (result);
      }

      public bool SetStepperCurrent(float current)
      {
         bool result = true;
         result &= this.ExchangeCommAction(new BinaryInterpreterSet("TC", 0, current));
         return (result);
      }

      public bool SetMode(int mode)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new BinaryInterpreterSet("MO", 0, 0));
         result &= this.ExchangeCommAction(new BinaryInterpreterSet("UM", 0, mode));
         result &= this.ExchangeCommAction(new BinaryInterpreterSet("MO", 0, 1));

         return (result);
      }

      public void Test()
      {
         bool result = true;

         result &= this.ExchangeCommAction(new BinaryInterpreterSet("MO", 0, 0));
         result &= this.ExchangeCommAction(new BinaryInterpreterSet("UM", 0, 5));
         result &= this.ExchangeCommAction(new BinaryInterpreterSet("MO", 0, 1));

         // SDO download
         // SET 0X6040 = 6 TRANSITION 2 - READY TO SWITCH ON
         //td 1 2 0x67f 0X2B 0X40 0X60 0X00 0X06 0X00 0X00 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x6040, 0, 2, (UInt16)0x6));

         // SDO DOWNLOAD
         // SET 0X6040 = 7 TRANSITION 3 - SWITCHED ON
         //td 1 2 0x67f 0X2B 0X40 0X60 0X00 0X07 0X00 0X00 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x6040, 0, 2, (UInt16)0x7));

         // SDO DOWNLOAD
         // SET 0X6040 = F TRANSITION 4 - OPERATION ENABLE MO=1
         //td 1 2 0x67f 0X2B 0X40 0X60 0X00 0X0F 0X00 0X00 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x6040, 0, 2, (UInt16)0xF));

         // SDO download
         // SET 0X6060 =6 Homing  MODE
         //td 1 2 0x67f 0x2F 0X60 0X60 0X00 0X06 0X00 0X00 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x6060, 0, 1, (byte)0x6));

         // SDO DOWNLOAD
         // SET 0X607C = 0 HOME OFFSET (COUNTS)
         //td 1 2 0x67f 0X23 0X7C 0X60 0X00 0X00 0X00 0X00 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x607C, 0, 4, (Int32)0));

         // SDO DOWNLOAD
         // SET 0X6098 = 33 HOMING METHOD HOME TO INDEX NEGATIVE DIRECTION
         //td 1 2 0x67f 0X2F 0X98 0X60 0X00 0X21 0X00 0X00 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x6098, 0, 1, (byte)33));

         // SDO DOWNLOAD
         // SET 0X6099.1 = 5000 SWITCH SEARCH SPEED
         //td 1 2 0x67f 0X2B 0X99 0X60 0X01 0X88 0X13 0X00 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x6099, 1, 4, (UInt32)5000));

         // SDO DOWNLOAD
         // SET 0X6099.2 = 5000 INDEX SEARCH SPEED
         //td 1 2 0x67f 0X2B 0X99 0X60 0X02 0X80 0X20 0X00 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x6099, 2, 4, (UInt32)5000));

         // SDO DOWNLOAD
         // SET 0X609A = 500000 HOMING ACCELERATION
         //td 1 2 0x67f 0X2B 0X9A 0X60 0X00 0X20 0XA1 0X07 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x609A, 0, 4, (UInt32)500000));

         // SDO DOWNLOAD
         // SET 0X6040 = 1F START HOMING MODE
         //td 1 2 0x67f 0X2B 0X40 0X60 0X00 0X1F 0X00 0X00 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x6040, 0, 2, (UInt16)0x1F));

         //delay 3000
         Thread.Sleep(3000);

         //WAIT FOR HOMING TO COMPLETE
         // SDO DOWNLOAD
         // SET 0X6040 = F TRANSITION 4 - OPERATION ENABLE MO=1
         //td 1 2 0x67f 0X2B 0X40 0X60 0X00 0X0F 0X01 0X00 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x6040, 0, 2, (UInt16)0xF));

         // SDO download
         // SET 0X6060 = 3 PROFILE velocity MODE
         //td 1 2 0x67f 0x2F 0X60 0X60 0X00 0X03 0X00 0X00 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x6060, 0, 1, (byte)0x3));


         // SDO UPload
         // READ 0X6061 OPERATING MODE
         //td 1 2 0x67f 0x40 0X61 0X60 0X00 0X00 0X00 0X00 0X00
         result &= this.ExchangeCommAction(new SDOUpload(0x6061, 0));

         for (int i = 0; i < 10; i++)
         {
            // SDO UPload
            // READ 0X6041 STATUS WORD
            //td 1 2 0x67f 0x40 0X41 0X60 0X00 0X00 0X00 0X00 0X00
            result &= this.ExchangeCommAction(new SDOUpload(0x6041, 0));

            Thread.Sleep(500);
         }
         //repeat 10

         // SDO DOWNLOAD

         // SDO DOWNLOAD
         // SET 0X60FF = 5000 target VELOCITY COUNTS/SEC
         //td 1 2 0x67f 0X23 0Xff 0X60 0X00 0X88 0X13 0X00 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x60FF, 0, 4, (Int32)5000));

         // SDO DOWNLOAD
         // SET 0X6040 = 3F NEW SET POINT, ADOPT IMMEDIATELY- REQUIRED FIRST MOTION AFTER HOMING ONLY (ERRATA - B8NOV2004 FW)
         //td 1 2 0x67f 0X2B 0X40 0X60 0X00 0X3F 0X00 0X00 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x6040, 0, 2, (UInt16)0x3F));

         // SDO DOWNLOAD
         // SET 0X6040 = F TRANSITION 4 - OPERATION ENABLE MO=1
         //td 1 2 0x67f 0X2B 0X40 0X60 0X00 0X0F 0X00 0X00 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x6040, 0, 2, (UInt16)0xF));

         //delay 3000
         Thread.Sleep(3000);


         // SDO DOWNLOAD
         // SET 0X60FF = 5000 target VELOCITY COUNTS/SEC
         //td 1 2 0x67f 0X23 0Xff 0X60 0X00 0X88 0X13 0X00 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x60FF, 0, 4, (Int32)5000));

         // SDO DOWNLOAD
         // SET 0X6040 = 3F NEW SET POINT, ADOPT IMMEDIATELY- REQUIRED FIRST MOTION AFTER HOMING ONLY (ERRATA - B8NOV2004 FW)
         //td 1 2 0x67f 0X2B 0X40 0X60 0X00 0X3F 0X00 0X00 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x6040, 0, 2, (UInt16)0x3F));

         // SDO DOWNLOAD
         // SET 0X6040 = F TRANSITION 4 - OPERATION ENABLE MO=1
         //td 1 2 0x67f 0X2B 0X40 0X60 0X00 0X0F 0X00 0X00 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x6040, 0, 2, (UInt16)0xF));

         //delay 2000
         Thread.Sleep(2000);


         // SDO DOWNLOAD
         // SET 0X60FF =  target VELOCITY COUNTS/SEC
         //td 1 2 0x67f 0X23 0Xff 0X60 0X00 0X88 0X23 0X00 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x60FF, 0, 4, (Int32)0x60000));

         // SDO DOWNLOAD
         // SET 0X6040 = 3F NEW SET POINT, ADOPT IMMEDIATELY- REQUIRED FIRST MOTION AFTER HOMING ONLY (ERRATA - B8NOV2004 FW)
         //td 1 2 0x67f 0X2B 0X40 0X60 0X00 0X3F 0X00 0X00 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x6040, 0, 2, (UInt16)0x3F));

         // SDO DOWNLOAD
         // SET 0X6040 = F TRANSITION 4 - OPERATION ENABLE MO=1
         //td 1 2 0x67f 0X2B 0X40 0X60 0X00 0X0F 0X00 0X00 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x6040, 0, 2, (UInt16)0xF));

         //delay 2000
         Thread.Sleep(2000);


         // SDO DOWNLOAD
         // SET 0X60FF =  target VELOCITY COUNTS/SEC
         //td 1 2 0x67f 0X23 0Xff 0X60 0X00 0X88 0X03 0X00 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x60FF, 0, 4, (Int32)0));

         // SDO DOWNLOAD
         // SET 0X6040 = 3F NEW SET POINT, ADOPT IMMEDIATELY- REQUIRED FIRST MOTION AFTER HOMING ONLY (ERRATA - B8NOV2004 FW)
         //td 1 2 0x67f 0X2B 0X40 0X60 0X00 0X3F 0X00 0X00 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x6040, 0, 2, (UInt16)0x3F));

         // SDO DOWNLOAD
         // SET 0X6040 = F TRANSITION 4 - OPERATION ENABLE MO=1
         //td 1 2 0x67f 0X2B 0X40 0X60 0X00 0X0F 0X00 0X00 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x6040, 0, 2, (UInt16)0xF));

         //delay 3000
         Thread.Sleep(3000);


         // SDO DOWNLOAD
         // SET 0X607A = -1000 TARGET VELOCITY COUNTS/SEC
         //td 1 2 0x67f 0X23 0Xff 0X60 0X00 0X18 0XFC 0XFF 0XFF
         int a = -1000;
         result &= this.ExchangeCommAction(new SDODownload(0x607A, 0, 4, (UInt32)a));

         // SDO DOWNLOAD
         // SET 0X6040 = 1F NEW SET POINT
         //td 1 2 0x67f 0X2B 0X40 0X60 0X00 0X3F 0X00 0X00 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x6040, 0, 2, (UInt16)0x1F));

         // SDO DOWNLOAD
         // SET 0X6040 = F TRANSITION 4 - OPERATION ENABLE MO=1
         //td 1 2 0x67f 0X2B 0X40 0X60 0X00 0X0F 0X00 0X00 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x6040, 0, 2, (UInt16)0xF));

         //delay 3000
         Thread.Sleep(3000);


         // SDO DOWNLOAD
         // SET 0X607A = 0 TARGET VELOCITY COUNTS/SEC
         //td 1 2 0x67f 0X23 0Xff 0X60 0X00 0X00 0X00 0X00 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x607A, 0, 4, (UInt32)0));

         // SDO DOWNLOAD
         // SET 0X6040 = 1F NEW SET POINT
         //td 1 2 0x67f 0X2B 0X40 0X60 0X00 0X1F 0X00 0X00 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x6040, 0, 2, (UInt16)0x1F));

         // SDO DOWNLOAD
         // SET 0X6040 = F TRANSITION 4 - OPERATION ENABLE MO=1
         //td 1 2 0x67f 0X2B 0X40 0X60 0X00 0X0F 0X00 0X00 0X00
         result &= this.ExchangeCommAction(new SDODownload(0x6040, 0, 2, (UInt16)0xF));


         //td 1 0 0x000 0x80 0x00 0x00 0x00 0x00 0x00 0x00 0x00
         //delay 1000
         Thread.Sleep(1000);
         //endrep
         //endrep
      }

      public override bool Configure()
      {
         // todo move code from start to here
         return base.Configure();
      }

      public override bool Start()
      {
         this.ResetReadings();
         bool result = true;

         // configure...
         result &= base.Configure();

         // set velocity and torque to TPDO1 every 200mS
         result &= this.SetTPDOMapCount(1, 0);
         result &= this.SetTPDOType(1, 255); // velocity/torque on change does not work, need to poll
         result &= this.SetTPDOEventTime(1, 200);
         result &= this.SetTPDOMap(1, 1, 0x606c, 0, 4);
         result &= this.SetTPDOMap(1, 2, 0x6077, 0, 2);
         result &= this.SetTPDOMapCount(1, 2);

         // leave TPDO2 for binary interpreter
         
         // set digitial input to TPDO3 on change
         result &= this.SetTPDOMapCount(3, 0);
         result &= this.SetTPDOType(3, 254); // note devices do not support time and delta changes at the same time
         result &= this.SetTPDOMap(3, 1, 0x60FD, 0, 4);
         result &= this.SetTPDOMapCount(3, 1);
 
#if false // status
         // set status to TPDO4 on change, maximum rate is 250mS
         result &= this.SetTPDOMapCount(4, 0);
         result &= this.SetTPDOType(4, 254);
         result &= this.SetTPDOInhibitTime(4, 250);
         result &= this.SetTPDOMap(4, 1, 0x6041, 0, 2);
         result &= this.SetTPDOMapCount(4, 1);
#endif

         // leave RPDO2 for binary interpreter

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
         
         base.Start();

         // set switch on disabled 
         result &= this.ExchangeCommAction(new SDODownload(0x6040, 0, 2, (UInt16)0x0)); // todo move this to configure

         this.motorCurrentRate = 0;
         result &= this.ReadSDO(0x6075, 0, ref this.motorCurrentRate);

         // sample digital inputs
         UInt32 digitInputs = 0;
         result &= this.ReadSDO(0x60FD, 0, ref digitInputs);

         if (false != result)
         {
            this.initialInputReportNeeded = true;
            this.ProcessDigitInputs((byte)(digitInputs >> 16));
         }

         return (result);
      }

      public override bool SetConsumerHeartbeat(UInt16 milliseconds, byte nodeId)
      {
         bool result = true;
          
         result &= this.ExchangeCommAction(new SDODownload(0x6007, 0, 2, 3)); // set to quick stop on fault
         result &= base.SetConsumerHeartbeat(milliseconds, nodeId);

         return (result);
      }

      public override void Fault(string faultReason)
      {
         this.ResetReadings();
         base.Fault(faultReason);
      }
      
      #endregion
   }
}