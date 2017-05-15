
namespace CanDemo.CAN
{
   using System;

   public class SDOUpload : CommAction
   {
      ushort index;
      byte subIndex;
      byte[] data;

      bool initiated;
      bool toggle;
      int count;

      public ushort Index { get { return (this.index); } }
      public byte SubIndex { get { return (this.subIndex); } }
      public byte[] Data { get { return (this.data); } }
      
      public SDOUpload(ushort index, byte subIndex)
         : base(CommActionTypes.SDOUpload)
      {
         this.index = index;
         this.subIndex = subIndex;
         
         this.initiated = false;
         this.toggle = false;
         this.count = 0;
         this.done = false;
      }

      public override bool ResponseNeeded()
      {
         return (true);
      }

      public override byte[] GetTransmitFrame(ref int cobId, int nodeId)
      {
         byte[] result = null;
         cobId = (int)(((int)COBTypes.RSDO << 7) | (nodeId & 0x7F));

         if (false == this.initiated)
         {
            result = new byte[4];

            result[0] = (byte)0x40;
            result[1] = (byte)((index >> 0) & 0xFF);
            result[2] = (byte)((index >> 8) & 0xFF);
            result[3] = (byte)subIndex;
         }
         else
         {
            result = new byte[4];

            result[0] = (byte)0x60;
            result[1] = (byte)((index >> 0) & 0xFF);
            result[2] = (byte)((index >> 8) & 0xFF);
            result[3] = (byte)subIndex;

            if (false != toggle)
            {
               result[0] |= 0x10;
            }
         }

         return (result);
      }

      public override void Process(int cobId, byte[] frame)
      {
         COBTypes frameType = (COBTypes)((cobId >> 7) & 0xF);

         if (COBTypes.TSDO == frameType)
         {
            if (false == this.initiated)
            {
               if ((null != frame) && (frame.Length >= 4))
               {
                  int scs = (int)((frame[0] >> 5) & 0x7);
                  int n = (int)((frame[0] >> 2) & 0x3);
                  int e = (int)((frame[0] >> 1) & 0x1);
                  int s = (int)((frame[0] >> 0) & 0x1);
                  ushort rspIndex = (ushort)((frame[2] << 8) | frame[1]);
                  byte rspSubIndex = frame[3];

                  if ((2 == scs) && (rspIndex == this.index) && (rspSubIndex == this.subIndex))
                  {
                     if (1 == e)
                     {
                        int length;

                        if (1 == s)
                        {
                           length = 4 - n;
                        }
                        else
                        {
                           length = 4;
                        }

                        this.data = new byte[length];

                        for (int i = 0; i < length; i++)
                        {
                           this.data[i] = frame[4 + i];
                        }

                        this.done = true;
                     }
                     else
                     {
                        if (1 == s)
                        {
                           int length = BitConverter.ToInt32(frame, 4);
                           this.data = new byte[length];

                           this.initiated = true;
                           this.transmit = true;
                        }
                        else
                        {
                           this.done = true;
                        }
                     }
                  }
                  else if (4 == scs)
                  {
                     UInt32 abortCode = BitConverter.ToUInt32(frame, 4);
                     this.aborted = true;
                     this.done = true;
                  }
               }
            }
            else
            {
               if ((null != frame) && (frame.Length >= 4))
               {
                  int scs = (int)((frame[0] >> 5) & 0x7);
                  int n = 7 - (int)((frame[0] >> 1) & 0x7);
                  bool t = ((frame[0] & 0x10) != 0) ? true : false;
                  bool c = ((frame[0] & 0x01) != 0) ? true : false;

                  if ((0 == scs) && (t == this.toggle))
                  {
                     for (int i = 0; i < n; i++)
                     {
                        int sourceIndex = 1 + i;

                        if ((this.count < this.data.Length) && (sourceIndex < frame.Length))
                        {
                           this.data[this.count] = frame[sourceIndex];
                           this.count++;
                        }
                     }

                     if (false == c)
                     {
                        this.toggle = !this.toggle;
                        this.transmit = true;
                     }
                     else
                     {
                        this.done = true;
                     }
                  }
                  else if (4 == scs)
                  {
                     UInt32 abortCode = BitConverter.ToUInt32(frame, 4);
                     this.aborted = true;
                     this.done = true;
                  }
               }
            }
         }
      }
   }
}