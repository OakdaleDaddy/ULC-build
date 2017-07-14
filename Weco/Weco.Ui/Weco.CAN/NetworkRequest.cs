
namespace Weco.CAN
{
   public class NetworkRequest : CommAction
   {
      byte state;
      byte nodeId;

      public NetworkRequest(byte state, byte nodeId)
         : base(CommActionTypes.NetworkRequest)
      {
         this.state = state;
         this.nodeId = nodeId;
         this.done = false;
      }

      public override bool ResponseNeeded()
      {
         return (false);
      }

      public override byte[] GetTransmitFrame(ref int cobId, int nodeId)
      {
         byte[] result = new byte[2];

         cobId = 0;
         result[0] = this.state;
         result[1] = this.nodeId;

         return (result);
      }

      public override void Process(int cobId, byte[] frame)
      {
      }
   }
}