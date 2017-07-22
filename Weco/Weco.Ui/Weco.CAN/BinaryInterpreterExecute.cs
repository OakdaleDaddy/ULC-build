
namespace Weco.CAN
{
   using System;

   public class BinaryInterpreterExecute : CommAction
   {
      private string command;

      public BinaryInterpreterExecute(string command)
         : base(CommActionTypes.BinaryInterpreterExecute)
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
         result[2] = (byte)0;
         result[3] = (byte)0;

         return (result);
      }

      public override void Process(int cobId, byte[] frame)
      {
         COBTypes frameType = (COBTypes)((cobId >> 7) & 0xF);

         if ((null != frame) && (frame.Length >= 4))
         {
            if ((COBTypes.TPDO2 == frameType) && (this.command[0] == frame[0]) && (this.command[1] == frame[1]))
            {
               this.done = true;
            }
         }
      }
   }
}