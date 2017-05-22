
namespace CanDemo.CAN
{
   using System;
   using System.Text;
   using System.Threading;

   public class DemoDevice : Device
   {
      #region Definition

      public enum Modes
      {
         off,
         velocity,
         undefined,
      }

      #endregion

      #region Field

      #endregion

      #region Properties

      public Int32 RPM { set; get; }

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
               if ((null != msg) && (msg.Length >= 4))
               {
                  Int32 velocityCounts = BitConverter.ToInt32(msg, 0);
                  this.RPM = (Int32)((double)velocityCounts * 2.5);
               }
            }
            else if (COBTypes.TPDO2 == frameType)
            {
               // handle TPDO2 data
            }
            else if (COBTypes.TPDO3 == frameType)
            {
               // handle TPDO3 data
            }
            else if (COBTypes.TPDO4 == frameType)
            {
               // handle TPDO4 data
            }
         }
      }

      protected override void EvaluateAction(CommAction action)
      {
         base.EvaluateAction(action);
      }

      #endregion

      #region Constructor

      public DemoDevice(string name, byte nodeId)
         : base(name, nodeId)
      {
         // add specific device construction
      }

      #endregion

      #region Access Methods

      public override void Initialize()
      {
         base.Initialize();
         this.RPM = 0;
      }

      public override bool Configure()
      {
         bool result = false;

         if (null == this.FaultReason)
         {
            result = true;

            // read type, name, version
            result &= base.Configure(); 

            // set velocity to TPDO1 every 200mS
            result &= this.SetTPDOMapCount(1, 0);
            result &= this.SetTPDOType(1, 255);
            result &= this.SetTPDOEventTime(1, 200);
            result &= this.SetTPDOMap(1, 1, 0x606c, 0, 4);
            result &= this.SetTPDOMapCount(1, 1);

            // set velocity to RPDO3 every SYNC
            result &= this.SetRPDOMapCount(3, 0);
            result &= this.SetRPDOType(3, 1);
            result &= this.SetRPDOMap(3, 1, 0x60FF, 0, 4);
            result &= this.SetRPDOMapCount(3, 1);
         }

         return (result);
      }

      public override bool Start()
      {
         bool result = false;

         if (null == this.FaultReason)
         {
            result = true;
            
            base.Start();
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
         this.RPM = 0;
      }

      public bool ReadVersion(ref string version)
      {
         bool result = true;

         SDOUpload upload = new SDOUpload(0x100A, 0);
         result &= this.ExchangeCommAction(upload);

         if ((false != result) &&
             (null != upload.Data))
         {
            version = Encoding.UTF8.GetString(upload.Data);
         }

         return (result);
      }

      public bool Get6040(ref UInt16 value)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x6040, 0x00);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 2))
         {
            value = BitConverter.ToUInt16(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool Set6040(UInt16 value)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x6040, 0, 2, (UInt32)value));

         return (result);
      }

      public bool Get6041(ref UInt16 value)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x6041, 0x00);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 2))
         {
            value = BitConverter.ToUInt16(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool Get6060(ref byte value)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x6060, 0x00);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 1))
         {
            value = upload.Data[0];
            result = true;
         }

         return (result);
      }

      public bool Set6060(byte value)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x6060, 0, 1, (UInt32)value));

         return (result);
      }

      public bool Get6061(ref byte value)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x6061, 0x00);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 1))
         {
            value = upload.Data[0];
            result = true;
         }

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

         return (result);
      }

      public bool GetTargetVelocity(ref Int32 velocityRpm)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x60FF, 0x00);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            velocityRpm = (Int32)(BitConverter.ToInt32(upload.Data, 0) * 2.5);
            result = true;
         }

         return (result);
      }

      public bool SetTargetVelocity(Int32 velocityRpm)
      {
         bool result = true;

         Int32 velocityCntPerSecond = (Int32)((double)velocityRpm / 2.5);
         result &= this.ExchangeCommAction(new SDODownload(0x60FF, 0, 4, (UInt32)velocityCntPerSecond));

         return (result);
      }

      public bool ScheduleTargetVelocity(Int32 velocityRpm)
      {
         bool result = true;

         Int32 velocityCntPerSecond = (Int32)((double)velocityRpm / 2.5);
         byte[] data = BitConverter.GetBytes(velocityCntPerSecond);
         result = this.ExchangeCommAction(new PDO3Emit(data));

         return (result);
      }

      #endregion
   }
}