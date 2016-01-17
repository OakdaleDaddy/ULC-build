
namespace NICBOT.GUI
{
   using System;

   public class FeederMotorParameters
   {
      public string Location;

      public MotorStates State;
      public MotorDirections Direction;

      /// <summary>
      /// true when a positive motion request gives velocity mode and a negative motion request gives current mode
      /// </summary>
      public bool PositivePusher;

      /// <summary>
      /// true when a positive motion request gives issues a negative request to the motor
      /// </summary>
      public bool PositionInversion;

      public FeederMotorParameters()
      {
         this.Location = "";

         this.State = default(MotorStates);
         this.Direction = default(MotorDirections);
         this.PositivePusher = default(bool);
         this.PositionInversion = default(bool);
      }

#if false
      public void Set(FeederMotorParameters parameters)
      {
         this.Location = parameters.Location;
         this.State = parameters.State;
         this.Direction = parameters.Direction;
      }
#endif
   }
}