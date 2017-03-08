
namespace E4.CAN
{
   using System;

   public class SDODownload : CommAction
   {
      ushort index;
      byte subIndex;

      UInt32 length;      
      byte[] buffer;
      int offset;

      bool initiated;
      bool toggle;
      UInt32 sentCount;
      UInt32 lastSentLength;

      public SDODownload(ushort index, byte subIndex, byte[] buffer, int offset, int length)
         : base(CommActionTypes.SDODownload)
      {
         this.index = index;
         this.subIndex = subIndex;
         this.length = (UInt32)length;
         this.buffer = buffer;
         this.offset = offset;

         this.initiated = false;
         this.toggle = false;
         this.sentCount = 0;
         this.lastSentLength = 0;
         this.done = false;
      }

      public SDODownload(ushort index, byte subIndex, byte length, uint data)
         : base(CommActionTypes.SDODownload)
      {
         this.index = index;
         this.subIndex = subIndex;
         this.length = length;
         this.buffer = new byte[length];
         this.offset = 0;

         int shifter = 0;

         for (int i = 0; i < this.length; i++)
         {
            this.buffer[i] = (byte)((data >> shifter) & 0xFF);
            shifter += 8;
         }

         this.initiated = false;
         this.toggle = false;
         this.sentCount = 0;
         this.lastSentLength = 0;
         this.done = false;
      }

      public UInt32 SentCount { get { return (this.sentCount); } }

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
            if (this.length <= 4)
            {
               // send expedited frame

               //int cmdLength = (int)(4 + this.length);
               //result = new byte[cmdLength];
               result = new byte[8];
               int n = (int)(4 - this.length);

               result[0] = (byte)(0x20 | (n << 2) | 0x03);
               result[1] = (byte)((index >> 0) & 0xFF);
               result[2] = (byte)((index >> 8) & 0xFF);
               result[3] = (byte)subIndex;

               for (int i = 0; i < this.length; i++)
               {
                  result[4 + i] = this.buffer[i + this.offset];
               }

               this.lastSentLength = this.length;
            }
            else
            {
               // send frame with size

               result = new byte[8];

               result[0] = (byte)(0x20 | 0x01);
               result[1] = (byte)((index >> 0) & 0xFF);
               result[2] = (byte)((index >> 8) & 0xFF);
               result[3] = (byte)subIndex;
               result[4] = (byte)((this.length >> 0) & 0xFF);
               result[5] = (byte)((this.length >> 8) & 0xFF);
               result[6] = (byte)((this.length >> 16) & 0xFF);
               result[7] = (byte)((this.length >> 24) & 0xFF);
            }
         }
         else
         {
            UInt32 remaining = this.length - this.sentCount;
            UInt32 packetCount = remaining;

            if (packetCount > 7)
            {
               packetCount = 7;
            }
            else
            {
            }

            result = new byte[8];

            int n = (int)((packetCount < 7) ? (7 - packetCount) : 0);
            int t = (false != this.toggle) ? 1 : 0;
            int c = (remaining <= 7) ? 1 : 0;

            result[0] = (byte)((t << 4) | (n << 1) | c);

            for (int i = 0; i < packetCount; i++)
            {
               result[1 + i] = this.buffer[this.sentCount + this.offset + i];
            }

            this.lastSentLength = packetCount;
         }

         return (result);
      }

      public override void Process(int cobId, byte[] frame)
      {
         if ((null != frame) && (frame.Length >= 4))
         {
            COBTypes frameType = (COBTypes)((cobId >> 7) & 0xF);
            int scs = (int)((frame[0] >> 5) & 0x7);

            if (COBTypes.TSDO == frameType) 
            {
               if (3 == scs)
               {
                  ushort rspIndex = (ushort)((frame[2] << 8) | frame[1]);
                  byte rspSubIndex = frame[3];

                  if ((rspIndex == this.index) &&
                      (rspSubIndex == this.subIndex))
                  {
                     this.initiated = true;
                     this.sentCount += this.lastSentLength;

                     if (this.sentCount == this.length)
                     {
                        this.done = true;
                     }
                     else
                     {
                        this.transmit = true;
                     }
                  }
               }
               else if (1 == scs)
               {
                  this.sentCount += this.lastSentLength;

                  if (this.sentCount == this.length)
                  {
                     this.done = true;
                  }
                  else
                  {
                     this.toggle = !this.toggle;
                     this.transmit = true;
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
