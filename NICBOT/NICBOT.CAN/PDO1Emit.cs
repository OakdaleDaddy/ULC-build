
namespace NICBOT.CAN
{
   public class PDO1Emit : CommAction
   {
      byte[] data;

      public PDO1Emit(byte[] data)
         : base(CommActionTypes.PDO1Emit)
      {
         this.data = data;
         this.done = false;
      }

      public override bool ResponseNeeded()
      {
         return (false);
      }

      public override byte[] GetTransmitFrame(ref int cobId, int nodeId)
      {
         cobId = (int)(((int)COBTypes.RPDO1 << 7) | (nodeId & 0x7F));
         return (this.data);
      }

      public override void Process(int cobId, byte[] frame)
      {
      }
   }
}