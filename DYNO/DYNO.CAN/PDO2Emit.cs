
namespace DYNO.CAN
{
   public class PDO2Emit : CommAction
   {
      byte[] data;

      public PDO2Emit(byte[] data)
         : base(CommActionTypes.PDO2Emit)
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
         cobId = (int)(((int)COBTypes.RPDO2 << 7) | (nodeId & 0x7F));
         return (this.data);
      }

      public override void Process(int cobId, byte[] frame)
      {
      }
   }
}