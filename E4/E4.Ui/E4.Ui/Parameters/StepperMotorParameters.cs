
namespace E4.Ui
{
   using System;

   public class StepperMotorParameters
   {
      public string Location;

      public int HomeOffset;
      public int HomingSwitchVelocity;
      public int HomingZeroVelocity;
      public int HomingAcceleration;

      public int ProfileVelocity;
      public int ProfileAcceleration;

      public int MaximumPosition;
      public int CenterPosition;
      public int MinimumPosition;

      public StepperMotorParameters()
      {
         this.Location = "";

         this.HomeOffset = 0;
         this.HomingSwitchVelocity = 0;
         this.HomingZeroVelocity = 0;
         this.HomingAcceleration = 0;

         this.ProfileVelocity = 0;
         this.ProfileAcceleration = 0;

         this.MaximumPosition = 0;
         this.CenterPosition = 0;
         this.MinimumPosition = 0;
      }
   }
}