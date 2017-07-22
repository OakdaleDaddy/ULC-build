namespace Weco.CAN
{
   using System;
   using System.Text;
   using System.Threading;

   public class UlcRoboticsRs232 : Device
   {
      #region Definition

      public delegate void SerialReceiveHandler(byte[] data, int length);

      #endregion

      #region Field

      private bool active;

      #endregion

      #region Helper Functions

      #endregion

      #region Properties

      public SerialReceiveHandler OnSerialReceive { set; get; }

      #endregion

      #region Overrides

      protected override void EvaluateMessage(int cobId, byte[] msg)
      {
         COBTypes frameType = (COBTypes)((cobId >> 7) & 0xF);
         int nodeId = (int)(cobId & 0x7F);

         if (nodeId == this.NodeId)
         {
            if (COBTypes.TPDO1 == frameType)
            {
               if (null != this.OnSerialReceive)
               {
                  this.OnSerialReceive(msg, msg.Length);
               }
            }
            else if (COBTypes.TPDO2 == frameType)
            {
            }
            else if (COBTypes.TPDO3 == frameType)
            {
            }
            else if (COBTypes.TPDO4 == frameType)
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

      public UlcRoboticsRs232(string name, byte nodeId)
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

      public bool SetDeviceNodeOffset(byte nodeOffset)
      {
         this.pendingAction = new SDODownload(0x2102, 0, 1, nodeOffset);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      public bool SaveConfiguration()
      {
         this.pendingAction = new SDODownload(0x2105, 0, 4, 0x65766173);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      public bool SetSerialBaud(UInt32 serialBaud)
      {
         this.pendingAction = new SDODownload(0x2200, 1, 4, serialBaud);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      public bool SetSerialDataBits(byte serialDataBits)
      {
         this.pendingAction = new SDODownload(0x2200, 2, 1, serialDataBits);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      public bool SetSerialStopBits(byte serialStopBits)
      {
         this.pendingAction = new SDODownload(0x2200, 3, 1, serialStopBits);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      public bool SetSerialParity(byte serialParity)
      {
         this.pendingAction = new SDODownload(0x2200, 4, 1, serialParity);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      public void WriteSerial(byte[] buffer, int offset, int length)
      {
         for (int i = 0; i < length; )
         {
            int pdoLength = length - i;
            int size = (pdoLength <= 8) ? pdoLength : 8;
            byte[] dataFrame = new byte[size];

            for (int j = 0; j < size; j++)
            {
               dataFrame[j] = buffer[offset + i + j];
            }

            PDO1Emit pdo1Emission = new PDO1Emit(dataFrame);
            this.ScheduleAction(pdo1Emission);

            i += size;
         }
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
               else if (3 == baudCode)
               {
                  result = 50000;
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

      public override bool Start()
      {
         bool result = false;

         if (null == this.FaultReason)
         {
            base.Start();
            this.active = true;
            result = true;
         }

         return (result);
      }

      /// <summary>
      /// Function to configure serial parameters and start device.
      /// </summary>
      /// <param name="serialBaud">baudrate of serial connection</param>
      /// <param name="serialDataBits">number of data bits of serial connection, {5..8}</param>
      /// <param name="serialParity">parity of serial connection, 0=none, 1=odd, 2=even</param>
      /// <param name="seritalStopBits">number of stop bits {1,2}</param>
      public void Start(UInt32 serialBaud, byte serialDataBits, byte serialParity, byte serialStopBits)
      {
         this.SetSerialBaud(serialBaud);
         this.SetSerialDataBits(serialDataBits);
         this.SetSerialParity(serialParity);
         this.SetSerialStopBits(serialStopBits);

         this.Start();
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