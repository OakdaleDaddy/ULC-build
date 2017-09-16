
namespace CrossBore.CAN
{
   using System;

   public class BinaryInterpreterSet : CommAction
   {
      private string command;
      private int index;
      private bool valueIsFloat;
      private int integerValue;
      private float floatValue;

      public BinaryInterpreterSet(string command, int index, int data)
         : base(CommActionTypes.BinaryInterpreterSet)
      {
         if (null == command)
         {
            command = "";
         }

         if (command.Length < 2)
         {
            command += "  ";
         }

         this.command = command;
         this.index = index;

         this.integerValue = data;
         this.valueIsFloat = false;
         this.done = false;
      }

      public BinaryInterpreterSet(string command, int index, float data)
         : base(CommActionTypes.BinaryInterpreterSet)
      {
         if (null == command)
         {
            command = "";
         }

         if (command.Length < 2)
         {
            command += "  ";
         }

         this.command = command;
         this.index = index;

         this.floatValue = data;
         this.valueIsFloat = true;
         this.done = false;
      }

      public override bool ResponseNeeded()
      {
         return (true);
      }

      public override byte[] GetTransmitFrame(ref int cobId, int nodeId)
      {
         cobId = (int)(((int)COBTypes.RPDO2 << 7) | (nodeId & 0x7F));

         byte[] result = new byte[8];

         result[0] = (byte)this.command[0];
         result[1] = (byte)this.command[1];
         result[2] = (byte)((this.index >> 0) & 0xFF);
         result[3] = (byte)((this.index >> 8) & 0x3F);

         if (false == this.valueIsFloat)
         {
            result[4] = (byte)((this.integerValue >> 0) & 0xFF);
            result[5] = (byte)((this.integerValue >> 8) & 0xFF);
            result[6] = (byte)((this.integerValue >> 16) & 0xFF);
            result[7] = (byte)((this.integerValue >> 24) & 0xFF);
         }
         else
         {
            result[3] |= (byte)0x80;

            byte[] floatArray = BitConverter.GetBytes(this.floatValue);

            result[4] = floatArray[0];
            result[5] = floatArray[1];
            result[6] = floatArray[2];
            result[7] = floatArray[3];
         }

         return (result);
      }

      public override void Process(int cobId, byte[] frame)
      {
         COBTypes frameType = (COBTypes)((cobId >> 7) & 0xF);

         if ((null != frame) && (frame.Length >= 4))
         {
            ushort index = (ushort)(((frame[3] & 0x3F) << 8) | frame[2]);

            if ((COBTypes.TPDO2 == frameType) && (this.command[0] == frame[0]) && (this.command[1] == frame[1]) && (index == this.index))
            {
               this.done = true;
            }
         }
      }   
   }

}