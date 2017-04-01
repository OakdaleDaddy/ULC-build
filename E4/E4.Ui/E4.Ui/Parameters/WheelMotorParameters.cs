
namespace E4.Ui
{
   using System;

   public class WheelMotorParameters
   {
      public string Location;

      public WheelMotorStates MotorState;
      public bool RequestInverted;
      public bool PositionInverted;

      public WheelMotorParameters()
      {
         this.Location = "";

         this.MotorState = WheelMotorStates.enabled;
         this.RequestInverted = false;
         this.PositionInverted = false;
      }
   }
}