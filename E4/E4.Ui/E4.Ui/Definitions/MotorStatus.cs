
namespace E4.Ui
{
   using System;

   public class MotorStatus
   {
      public int positionRequested;
      public int positionNeeded;
      public bool requestIssued;
      public bool requestEvaluated;
      public bool requestMissed;
      public bool homeNeeded;

      public int velocityNeeded;
      public int velocityRequested;

      public MotorStatus()
      {
         this.Initialize();
      }

      public void Initialize()
      {
         this.positionRequested = 0;
         this.positionNeeded = 0;
         this.requestIssued = false;
         this.requestEvaluated = false;
         this.requestMissed = false;
         this.homeNeeded = false;

         this.velocityNeeded = 0;
         this.velocityRequested = 0;
      }
   }
}