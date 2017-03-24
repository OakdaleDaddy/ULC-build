
namespace E4.Ui
{
   using System;

   public class StepperMotorStatus
   {
      public int positionRequested;
      public int positionNeeded;
      public bool homeNeeded;

      public int actualPosition;
      public DateTime readTimeLimit;

      public StepperMotorStatus()
      {
         this.Initialize();
      }

      public void Initialize()
      {
         this.positionRequested = 0;
         this.positionNeeded = 0;
         this.homeNeeded = false;

         this.actualPosition = 0;
         this.readTimeLimit = DateTime.Now;
      }
   }
}