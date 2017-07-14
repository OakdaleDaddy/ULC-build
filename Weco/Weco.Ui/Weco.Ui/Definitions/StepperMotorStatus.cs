
namespace Weco.Ui
{
   using System;

   public class StepperMotorStatus
   {
      public enum States
      {
         off,
         stopped,

         startHoming,
         homing,

         startPosition,
         positioning,
         centering,
         stopping,
      }

      public States state;

      public int positionRequested;
      public int positionNeeded;
      public int positionTarget;
      
      public bool homeNeeded;
      public bool centerNeeded;
      public bool actualNeeded;
      public bool stopNeeded;

      public int actualPosition;
      public DateTime readTimeLimit;
      public DateTime statusInvalidTimeLimit; // status from second motor is delayed from TPDO inhibbit time 

      public StepperMotorStatus()
      {
         this.Initialize();
      }

      public void Initialize()
      {
         this.state = States.off;

         this.positionRequested = 0;
         this.positionNeeded = 0;
         this.positionTarget = 0;

         this.homeNeeded = false;
         this.centerNeeded = false;
         this.actualNeeded = false;
         this.stopNeeded = false;

         this.actualPosition = 0;
         this.readTimeLimit = DateTime.Now;
         this.statusInvalidTimeLimit = DateTime.Now;
      }
   }
}