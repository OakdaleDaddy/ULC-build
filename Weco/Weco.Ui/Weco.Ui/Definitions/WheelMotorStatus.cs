
namespace Weco.Ui
{
   using System;

   public class WheelMotorStatus
   {
      public enum States
      {
         stopped,
         undefined,

         turnOff,
         off,

         startPosition,
         positioning,
         stopPosition,

         startVelocity,
         velocity,
         stopVelocity,
      }

      public States state;

      public int positionRequested;
      public int positionNeeded;

      public double velocityRequested;
      public double velocityNeeded;
      public ActuationModes velocityActuationMode;

      public bool stopNeeded;

      public DateTime statusInvalidTimeLimit; // status from second motor is delayed from TPDO inhibbit time 

      public WheelMotorStatus()
      {
         this.Initialize();
      }

      public void Initialize()
      {
         this.state = States.undefined;

         this.positionRequested = 0;
         this.positionNeeded = 0;

         this.velocityRequested = 0;
         this.velocityNeeded = 0;
         this.velocityActuationMode = ActuationModes.closedloop;

         this.stopNeeded = false;

         this.statusInvalidTimeLimit = DateTime.Now;
      }
   }
}