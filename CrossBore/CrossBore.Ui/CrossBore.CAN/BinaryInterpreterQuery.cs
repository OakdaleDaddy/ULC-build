
namespace CrossBore.CAN
{
   using System;

   public class BinaryInterpreterQuery : CommAction
   {
      private string command;
      private int index;

      private int integerValue;
      private float floatValue;
      private bool valueIsFloat;

      public string Command { get { return (this.command); } }
      public int IntegerValue { get { return (this.integerValue); } }
      public float FloatValue { get { return (this.floatValue); } }
      public bool ValueIsFloat { get { return (this.valueIsFloat); } }

      public BinaryInterpreterQuery(string command, int index)
         : base(CommActionTypes.BinaryInterpreterQuery)
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
         this.done = false;
      }

      public override bool ResponseNeeded()
      {
         return (true);
      }

      public override byte[] GetTransmitFrame(ref int cobId, int nodeId)
      {
         cobId = (int)(((int)COBTypes.RPDO2 << 7) | (nodeId & 0x7F));

         byte[] result = new byte[4];

         result[0] = (byte)this.command[0];
         result[1] = (byte)this.command[1];
         result[2] = (byte)((index >> 0) & 0xFF);
         result[3] = (byte)((index >> 8) & 0x3F);

         return (result);
      }

      public override void Process(int cobId, byte[] frame)
      {
         COBTypes frameType = (COBTypes)((cobId >> 7) & 0xF);

         if ((null != frame) && (frame.Length >= 8))
         {
            ushort index = (ushort)(((frame[3] & 0x3F) << 8) | frame[2]);

            if ((COBTypes.TPDO2 == frameType) && (this.command[0] == frame[0]) && (this.command[1] == frame[1]) && (index == this.index))
            {
               if ((frame[3] & 0x80) == 0)
               {
                  this.valueIsFloat = false;
                  this.integerValue = BitConverter.ToInt32(frame, 4);
               }
               else
               {
                  this.valueIsFloat = true;
                  this.floatValue = BitConverter.ToSingle(frame, 4);
               }

               this.done = true;
            }
         }
      }
   }
}