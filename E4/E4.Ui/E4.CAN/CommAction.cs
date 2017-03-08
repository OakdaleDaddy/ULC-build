
namespace E4.CAN
{
   public class CommAction
   {
      private CommActionTypes type;
      protected bool done;
      protected bool transmit;
      protected bool aborted;

      public CommActionTypes Type { get { return (this.type); } }
      public bool Done { get { return (this.done); } }
      public bool Aborted { get { return (this.aborted); } }
      public bool Transmit { get { return (this.transmit); } }

      public int RetryTime { get; set; }
      public int RetryAttemptLimit { get; set; }

      public CommAction(CommActionTypes action)
      {
         this.type = action;
         this.transmit = false;
         this.done = true;
         this.aborted = false;

         this.RetryTime = 100;
         this.RetryAttemptLimit = 3;
      }

      public virtual bool ResponseNeeded()
      {
         return (false);
      }

      public virtual byte[] GetTransmitFrame(ref int cobId, int nodeId)
      {
         cobId = nodeId;
         return (null);
      }

      public virtual void Process(int cobId, byte[] frame)
      {
      }

      public void TransmitComplete()
      {
         this.transmit = false;
      }
   }
}