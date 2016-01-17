namespace NICBOT.CAN
{
   using System;
   using System.Text;
   using NICBOT.Utilities;

   public class PeakDigitalIo : Device
   {
      #region Definition

      public delegate void InputChangeHandler(int nodeId, byte value);

      #endregion

      #region Fields

      private byte inputValue;
      private bool initialInputReportNeeded;

      #endregion

      #region Properties

      public byte InputValue { get { return (this.inputValue); } }

      public InputChangeHandler OnInputChange { set; get; }

      #endregion

      #region Overrides

      protected override void EvaluateMessage(int cobId, byte[] msg)
      {
         COBTypes frameType = (COBTypes)((cobId >> 7) & 0xF);
         int nodeId = (int)(cobId & 0x7F);

         if (COBTypes.EMGY == frameType)
         {
            UInt64 errorCode = 0;

            if ((null != msg) && (8 >= msg.Length))
            {
               errorCode = BitConverter.ToUInt64(msg, 0);
            }

            if (0 != errorCode)
            {
               string faultReason = string.Format("emergency {0:X16}", errorCode);
               this.Fault(faultReason);
            }
         }
         else if (COBTypes.TPDO1 == frameType)
         {
            byte lastInputValue = this.inputValue;
            this.inputValue = msg[0];

            bool reportNeeded = (false != initialInputReportNeeded) || (this.inputValue != lastInputValue);
            this.initialInputReportNeeded = false;

            if ((null != this.OnInputChange) && (false != reportNeeded))
            {
               this.OnInputChange(this.NodeId, this.inputValue);
            }
         }
      }

      #endregion

      #region Constructor

      public PeakDigitalIo(string name, byte nodeId)
         : base(name, nodeId)
      {
      }

      #endregion

      #region Access Methods

      public override void Initialize()
      {
         base.Initialize();
         this.inputValue = 0;
         this.initialInputReportNeeded = true;
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

            // set polarity
            result &= this.ExchangeCommAction(new SDODownload(0x6202, 1, 1, 0x0));

            // set non-operational state sets defaults
            result &= this.ExchangeCommAction(new SDODownload(0x6206, 1, 1, 0xF));

            // set defaults
            result &= this.ExchangeCommAction(new SDODownload(0x6207, 1, 1, 0x0));

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

      public bool SetOutput(byte value)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x6200, 1, 1, (UInt32)value));

         return (result);
      }

      #endregion
   }
}