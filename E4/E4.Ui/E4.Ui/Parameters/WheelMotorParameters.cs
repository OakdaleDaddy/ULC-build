
namespace E4.Ui
{
   using System;

   public class WheelMotorParameters
   {
      public string Location;

      public WheelMotorStates MotorState;
      public bool RequestInverted;
      public bool PositionInverted;

      public int ProfileVelocity;
      public int ProfileAcceleration;
      public int ProfileDeceleration;

      public WheelMotorParameters()
      {
         this.Location = "";

         this.MotorState = WheelMotorStates.enabled;
         this.RequestInverted = false;
         this.PositionInverted = false;

         this.ProfileVelocity = 0;
         this.ProfileAcceleration = 0;
         this.ProfileDeceleration = 0;
      }
   }
}