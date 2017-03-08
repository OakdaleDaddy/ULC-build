
namespace E4.CAN
{
   public class PDO4Emit : CommAction
   {
      byte[] data;

      public PDO4Emit(byte[] data)
         : base(CommActionTypes.PDO4Emit)
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
         cobId = (int)(((int)COBTypes.RPDO4 << 7) | (nodeId & 0x7F));
         return (this.data);
      }

      public override void Process(int cobId, byte[] frame)
      {
      }
   }
}