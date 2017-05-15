
namespace CanDemo.CAN
{
   using System;
   using System.Text;
   using System.Threading;

   public class DemoDevice : Device
   {
      #region Definition

      #endregion

      #region Field

      #endregion

      #region Properties

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
               // handle TPDO1 data
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
      }

      public override bool Configure()
      {
         bool result = false;

         if (null == this.FaultReason)
         {
            result = true;

            // specific configuration here, i.e. TPDO/RPDO configuration, device specific reads, ...

            result &= base.Configure(); // read type, name, version
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

      #endregion
   }
}