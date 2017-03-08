
namespace E4.BusSim
{
   using System;
   using System.Collections;
   using System.Text;

   public class TPDOMapping
   {
      #region Definitions

      public delegate bool PdoMappableHandler(UInt16 index, byte subIndex);
      public delegate int PdoSizeHandler(UInt16 index, byte subIndex);
      public delegate int PdoDataHandler(UInt16 index, byte subIndex, byte[] data, int offset);

      #endregion

      #region Properties

      public PdoMappableHandler OnPdoMappable { set; get; }
      public PdoSizeHandler OnPdoSize { set; get; }
      public PdoDataHandler OnPdoData { set; get; }

      #endregion

      #region Fields

      public byte mapCount;
      public UInt32[] mappings;

      private byte txType;
      private UInt32 cobId;
      private UInt16 inhibitTime;
      private UInt16 eventTime;

      private int syncCount;
      private DateTime txInhibitLimit;
      private DateTime txEventLimit;
      private bool txNeeded;

      private bool started;
      private bool active;

      #endregion

      #region Helper Functions

      private bool GetPdoMappable(UInt16 index, byte subIndex)
      {
         bool result = false;

         if (null != this.OnPdoMappable)
         {
            result = this.OnPdoMappable(index, subIndex);
         }

         return (result);
      }

      private int GetPdoSize(UInt16 index, byte subIndex)
      {
         int result = 0;

         if (null != this.OnPdoSize)
         {
            result = this.OnPdoSize(index, subIndex);
         }

         return (result);
      }

      private int LoadPdoData(UInt16 index, byte subIndex, byte[] data, int offset)
      {
         int result = 0;

         if (null != this.OnPdoData)
         {
            result = this.OnPdoData(index, subIndex, data, offset);
         }
         else
         {
            int size = this.GetPdoSize(index, subIndex);
            
            for (int i = 0; i < size; i++)
            {
               data[offset+i] = 0;
            }
         }

         return (result);
      }

      private byte GetPdoMapByteCount(UInt32 mapping)
      {
         byte mapByteCount = (byte)((mapping & 0xFF) / 8);
         return (mapByteCount);
      }

      private bool StoreMapping(int subIndex, UInt32 mapping)
      {
         bool result = false;

         UInt16 mapIndex = (UInt16)((mapping >> 16) & 0xFFFF);
         byte mapSubIndex = (byte)((mapping >> 8) & 0xFF);
         bool mappableObject = this.GetPdoMappable(mapIndex, mapSubIndex);

         if ((false != mappableObject) &&
               (subIndex >= 1) &&
               (subIndex <= 8))
         {
            byte pdoByteCount = this.GetPdoMapByteCount(mapping);

            for (int i = 1; i < subIndex; i++)
            {
               pdoByteCount += this.GetPdoMapByteCount(this.mappings[i - 1]);
            }

            if (pdoByteCount <= 8)
            {
               result = true;
            }
         }

         if (false != result)
         {
            this.mappings[subIndex - 1] = mapping;
         }

         return (result);
      }

      #endregion

      #region Constructor

      public TPDOMapping()
      {
         this.mappings = new UInt32[8];
      }

      #endregion
      
      #region Access Functions

      public bool StoreParameterData(byte subIndex, int byteCount, UInt32 value)
      {
         bool result = false;

         if (false == this.started)
         {
            if (1 == subIndex)
            {
               if (4 == byteCount)
               {
                  this.cobId = value;
                  result = true;
               }
            }
            else if (2 == subIndex)
            {
               if (1 == byteCount)
               {
                  this.txType = (byte)value;
                  result = true;
               }
            }
            else if (3 == subIndex)
            {
               if (2 == byteCount)
               {
                  this.inhibitTime = (UInt16)value;
                  result = true;
               }
            }
            else if (5 == subIndex)
            {
               if (2 == byteCount)
               {
                  this.eventTime = (UInt16)value;
                  result = true;
               }
            }
         }

         return (result);
      }

      public bool StoreMapData(byte subIndex, int byteCount, UInt32 value)
      {
         bool result = false;

         if (false == this.started)
         {
            if (0 == subIndex)
            {
               if (1 == byteCount)
               {
                  if (0 == value)
                  {
                     this.mapCount = (byte)value;
                     result = true;
                  }
                  else
                  {
                     byte pdoByteCount = 0;

                     for (int i = 0; i < value; i++)
                     {
                        pdoByteCount += this.GetPdoMapByteCount(this.mappings[i]);
                     }

                     if ((0 < pdoByteCount) && (8 >=pdoByteCount))
                     {
                        this.mapCount = (byte)value;
                        result = true;
                     }
                  }
               }
            }
            else if ((subIndex >= 1) && (subIndex <= 8))
            {
               if (4 == byteCount)
               {
                  result = this.StoreMapping(subIndex, value);
               }
            }
         }

         return (result);
      }

      public void LoadParameterData(byte subIndex, byte[] buffer, ref UInt32 dataCount)
      {
         if (0 == subIndex)
         {
            buffer[0] = 5;
            dataCount = 1;
         }
         else if (1 == subIndex)
         {
            buffer[0] = (byte)((this.cobId >> 0) & 0xFF);
            buffer[1] = (byte)((this.cobId >> 8) & 0xFF);
            buffer[2] = (byte)((this.cobId >> 16) & 0xFF);
            buffer[3] = (byte)((this.cobId >> 248) & 0xFF);
            dataCount = 4;
         }
         else if (2 == subIndex)
         {
            buffer[0] = this.txType;
            dataCount = 1;
         }
         else if (3 == subIndex)
         {
            buffer[0] = (byte)((this.inhibitTime >> 0) & 0xFF);
            buffer[1] = (byte)((this.inhibitTime >> 8) & 0xFF);
            dataCount = 2;
         }
         else if (5 == subIndex)
         {
            buffer[0] = (byte)((this.eventTime >> 0) & 0xFF);
            buffer[1] = (byte)((this.eventTime >> 8) & 0xFF);
            dataCount = 2;
         }
      }

      public void LoadMapData(byte subIndex, byte[] buffer, ref UInt32 dataCount)
      {
         if (0 == subIndex)
         {
            buffer[0] = this.mapCount;
            dataCount = 1;
         }
         else if ((subIndex >= 1) && (subIndex <= 8))
         {
            UInt32 mapping = this.mappings[subIndex - 1];

            buffer[0] = (byte)((mapping >> 0) & 0xFF);
            buffer[1] = (byte)((mapping >> 8) & 0xFF);
            buffer[2] = (byte)((mapping >> 16) & 0xFF);
            buffer[3] = (byte)((mapping >> 24) & 0xFF);

            dataCount = 4;
         }
      }

      public void Reset(int offset, int nodeId)
      {
         this.mapCount = 0;

         for (int i = 0; i < 8; i++)
         {
            this.mappings[i] = 0;
         }

         int nodeOffset = offset / 4;
         int nodeType = offset % 4;

         COBTypes cobType = COBTypes.TPDO1;

         if (0 == nodeType)
         {
            cobType = COBTypes.TPDO1;
         }
         else if (1 == nodeType)
         {
            cobType = COBTypes.TPDO2;
         }
         else if (2 == nodeType)
         {
            cobType = COBTypes.TPDO3;
         }
         else if (3 == nodeType)
         {
            cobType = COBTypes.TPDO4;
         }

         this.cobId = (UInt32)(0x40000000 | ((UInt32)cobType) << 7) | ((UInt32)(nodeId + nodeOffset) & 0x7F);

         this.inhibitTime = 0;
         this.eventTime = 0;

         this.syncCount = 0;
         this.txInhibitLimit = DateTime.Now;
         this.txEventLimit = DateTime.Now;
         this.txNeeded = false;

         this.started = false;
         this.active = false;
      }

      public void Start()
      {
         this.started = true;
         this.active = true;
         this.txInhibitLimit = DateTime.Now;
         this.txEventLimit = DateTime.Now.AddMilliseconds(this.eventTime);
         this.Activate();
      }

      public void Stop()
      {
         this.active = false;
      }

      public void SyncReceived()
      {
         if (false != this.active)
         {
            if (this.txType <= 240)
            {
               this.syncCount++;

               if (this.syncCount >= this.txType)
               {
                  this.txNeeded = true;
               }
            }
         }
      }

      public bool Contains(UInt16 pdoIndex, byte pdoSubIndex)
      {
         bool result = false;

         for (int i = 0; i < this.mapCount; i++)
         {
            UInt16 mapIndex = (UInt16)((this.mappings[i] >> 16) & 0xFFFF);
            byte mapSubIndex = (byte)((this.mappings[i] >> 8) & 0xFF);

            if ((pdoIndex == mapIndex) && (pdoSubIndex == mapSubIndex))
            {
               result = true;
               break;
            }
         }

         return (result);
      }

      public void Activate()
      {
         if ((false != this.active) &&
             (0 != this.mapCount))
         {
            this.txNeeded = true;
         }
      }

      public void Update(ref int cobId, ref byte[] frame)
      {
         if (false != this.active)
         {
            bool transmit = this.txNeeded;

            if ((0 != this.eventTime) && (DateTime.Now > this.txEventLimit))
            {
               this.txEventLimit = this.txEventLimit.AddMilliseconds(this.eventTime);

               if ((254 == this.txType) || (255 == this.txType))
               {
                  transmit = true;
               }
            }

            if ((false != transmit) && (DateTime.Now > this.txInhibitLimit))
            {
               int pdoSize = 0;

               for (int i = 0; i < this.mapCount; i++)
               {
                  pdoSize += this.GetPdoMapByteCount(this.mappings[i]);
               }

               if ((0 != mapCount) && (0 != pdoSize))
               {
                  cobId = (int)this.cobId;
                  frame = new byte[pdoSize];

                  int pdoOffset = 0;

                  for (int i = 0; i < mapCount; i++)
                  {
                     UInt16 mapIndex = (UInt16)((this.mappings[i] >> 16) & 0xFFFF);
                     byte mapSubIndex = (byte)((this.mappings[i] >> 8) & 0xFF);
                     pdoOffset += this.LoadPdoData(mapIndex, mapSubIndex, frame, pdoOffset);
                  }
               }
            }
         }
      }

      public void Transmitted()
      {
         this.txInhibitLimit = DateTime.Now.AddMilliseconds(this.inhibitTime/10);
         this.syncCount = 0;
         this.txNeeded = false;
      }

      #endregion
   }
}