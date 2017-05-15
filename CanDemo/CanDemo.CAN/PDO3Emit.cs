
namespace CanDemo.CAN
{
   public class PDO3Emit : CommAction
   {
      byte[] data;

      public PDO3Emit(byte[] data)
         : base(CommActionTypes.PDO3Emit)
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
         cobId = (int)(((int)COBTypes.RPDO3 << 7) | (nodeId & 0x7F));
         return (this.data);
      }

      public override void Process(int cobId, byte[] frame)
      {
      }
   }
}