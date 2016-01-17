namespace NICBOT.CAN
{
   using System;
   using System.Text;
   using NICBOT.Utilities;

   public class PeakAnalogIo : Device
   {
      #region Fields

      private Int16[] inputValues;

      #endregion

      #region Properties

      public int OutputSendPeriod { set; get; }

      public Int16 AnalogIn0 { get { return (this.inputValues[0]); } }
      public Int16 AnalogIn1 { get { return (this.inputValues[1]); } }
      public Int16 AnalogIn2 { get { return (this.inputValues[2]); } }
      public Int16 AnalogIn3 { get { return (this.inputValues[3]); } }
      public Int16 AnalogIn4 { get { return (this.inputValues[4]); } }
      public Int16 AnalogIn5 { get { return (this.inputValues[5]); } }
      public Int16 AnalogIn6 { get { return (this.inputValues[6]); } }
      public Int16 AnalogIn7 { get { return (this.inputValues[7]); } }

      #endregion

      #region Overrides

      protected override void EvaluateMessage(int cobId, byte[] msg)
      {
         COBTypes frameType = (COBTypes)((cobId >> 7) & 0xF);
         int nodeId = (int)(cobId & 0x7F);

         if (COBTypes.TPDO2 == frameType)
         {
            this.inputValues[0] = (Int16)((msg[1] << 8) | msg[0]);
            this.inputValues[1] = (Int16)((msg[3] << 8) | msg[2]);
            this.inputValues[2] = (Int16)((msg[5] << 8) | msg[4]);
            this.inputValues[3] = (Int16)((msg[7] << 8) | msg[6]);
         }
         else if (COBTypes.TPDO3 == frameType)
         {
            this.inputValues[4] = (Int16)((msg[1] << 8) | msg[0]);
            this.inputValues[5] = (Int16)((msg[3] << 8) | msg[2]);
            this.inputValues[6] = (Int16)((msg[5] << 8) | msg[4]);
            this.inputValues[7] = (Int16)((msg[7] << 8) | msg[6]);
         }
      }

      #endregion

      #region Constructor

      public PeakAnalogIo(string name, byte nodeId)
         : base(name, nodeId)
      {
         this.inputValues = new Int16[8];
      }

      #endregion

      #region Access Methods

      public override void Initialize()
      {
         base.Initialize();

         if (null != this.inputValues)
         {
            for (int i = 0; i < this.inputValues.Length; i++)
            {
               this.inputValues[i] = 0;
            }
         }
      }

      public bool SetDeviceBaudRate(int rate)
      {
         int rateCode = 4;

         if (1000000 == rate)
         {
            rateCode = 0;
         }
         else if (800000 == rate)
         {
            rateCode = 1;
         }
         else if (500000 == rate)
         {
            rateCode = 2;
         }
         else if (250000 == rate)
         {
            rateCode = 3;
         }
         else if (125000 == rate)
         {
            rateCode = 4;
         }
         else if (50000 == rate)
         {
            rateCode = 6;
         }

         string cmd = string.Format("BPS{0}", rateCode);
         byte[] cmdData = Encoding.UTF8.GetBytes(cmd);

         this.pendingAction = new SDODownload(0x1F50, 3, cmdData, 0, cmdData.Length);
         bool result = this.ExchangeCommAction(this.pendingAction);

         return (result);
      }

      public bool SetDeviceNodeId(byte nodeId)
      {
         string cmd = string.Format("NI{0:X2}", nodeId);
         byte[] cmdData = Encoding.UTF8.GetBytes(cmd);

         this.pendingAction = new SDODownload(0x1F50, 3, cmdData, 0, cmdData.Length);
         bool result = this.ExchangeCommAction(this.pendingAction);

         return (result);
      }

      public override bool Configure()
      {
         bool result = false;

         if (null == this.FaultReason)
         {
            result = true;

            // common configuration
            result &= base.Configure();

            // disable RPDO
            result &= this.SetRPDOEnable(1, false);
            result &= this.SetRPDOEnable(2, false);
            result &= this.SetRPDOEnable(3, false);
            result &= this.SetRPDOEnable(4, false);

            // disable TPDO 
            result &= this.SetTPDOEnable(1, false);
            result &= this.SetTPDOEnable(2, false);
            result &= this.SetTPDOEnable(3, false);
            result &= this.SetTPDOEnable(4, false);
                        
            // clear TPDO map count
            result &= this.SetTPDOMapCount(1, 0);
            result &= this.SetTPDOMapCount(2, 0);
            result &= this.SetTPDOMapCount(3, 0);
            result &= this.SetTPDOMapCount(4, 0);

            // set TPDO2 on event, 250mS inhibit, analag inputs 1..4 
            result &= this.SetTPDOType(2, 255);
            result &= this.SetTPDOInhibitTime(2, 250);
            result &= this.SetTPDOMap(2, 1, 0x6401, 1, 2);
            result &= this.SetTPDOMap(2, 2, 0x6401, 2, 2);
            result &= this.SetTPDOMap(2, 3, 0x6401, 3, 2);
            result &= this.SetTPDOMap(2, 4, 0x6401, 4, 2);
            result &= this.SetTPDOMapCount(2, 4);

            // set TPDO3 on event, 250mS inhibit, analag inputs 5..8
            result &= this.SetTPDOType(3, 255);
            result &= this.SetTPDOInhibitTime(3, 250);
            result &= this.SetTPDOMap(3, 1, 0x6401, 5, 2);
            result &= this.SetTPDOMap(3, 2, 0x6401, 6, 2);
            result &= this.SetTPDOMap(3, 3, 0x6401, 7, 2);
            result &= this.SetTPDOMap(3, 4, 0x6401, 8, 2);
            result &= this.SetTPDOMapCount(3, 4);

            // enable TPDO2 and TPDO3 
            result &= this.SetTPDOEnable(2, true);
            result &= this.SetTPDOEnable(3, true);

            // enable interrupt delta 
            result &= this.ExchangeCommAction(new SDODownload(0x6423, 0, 1, (byte)0x01));

            // set interrupt delta 
            result &= this.ExchangeCommAction(new SDODownload(0x6426, 1, 4, (UInt32)0x100));
            result &= this.ExchangeCommAction(new SDODownload(0x6426, 2, 4, (UInt32)0x100));
            result &= this.ExchangeCommAction(new SDODownload(0x6426, 3, 4, (UInt32)0x100));
            result &= this.ExchangeCommAction(new SDODownload(0x6426, 4, 4, (UInt32)0x100));
            result &= this.ExchangeCommAction(new SDODownload(0x6426, 5, 4, (UInt32)0x100));
            result &= this.ExchangeCommAction(new SDODownload(0x6426, 6, 4, (UInt32)0x100));
            result &= this.ExchangeCommAction(new SDODownload(0x6426, 7, 4, (UInt32)0x100));
            result &= this.ExchangeCommAction(new SDODownload(0x6426, 8, 4, (UInt32)0x100));
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
            
      public override void Fault(string faultReason)
      {
         for (int i = 0; i < this.inputValues.Length; i++)
         {
            this.inputValues[i] = 0;
         }

         base.Fault(faultReason);
      }

      public bool SetOutput(int index, UInt16 value)
      {
         bool result = false;

         if (index < 4)
         {
            result = this.ExchangeCommAction(new SDODownload(0x6411, (byte)(index + 1), 2, value));

            //byte[] pdoBytes = BitConverter.GetBytes(value);
            //result = this.ExchangeCommAction(new PDO1Emit(pdoBytes));
         }

         return (result);
      }

      #endregion
   }
}