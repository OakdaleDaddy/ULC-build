
namespace E4.Ui
{
   using System;

   public class WheelMotorStatus
   {
      public enum States
      {
         off,
         stopped,

         startPosition,
         positioning,
         stopping,

         startVelocity,
         velocity,
      }

      public States state;

      public int positionRequested;
      public int positionNeeded;

      public double velocityRequested;
      public double velocityNeeded;

      public bool stopNeeded;

      public DateTime statusInvalidTimeLimit; // status from second motor is delayed from TPDO inhibbit time 

      public WheelMotorStatus()
      {
         this.Initialize();
      }

      public void Initialize()
      {
         this.state = States.off;

         this.positionRequested = 0;
         this.positionNeeded = 0;

         this.velocityRequested = 0;
         this.velocityNeeded = 0;

         this.stopNeeded = false;

         this.statusInvalidTimeLimit = DateTime.Now;
      }
   }
}