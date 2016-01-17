
namespace NICBOT.GUI
{
   using System;

   public class MovementMotorParameters
   {
      public string Location;

      public MotorStates State;
      public MotorDirections Direction;
      public MovementForwardControls AxialMode;
      public MovementForwardControls CircumferentialMode;
      public MovementForwardControls CornerAxialMode;
      public MovementForwardControls LaunchAxialMode;

      public MovementMotorParameters()
      {
         this.Location = "";

         this.State = default(MotorStates);
         this.Direction = default(MotorDirections);
         this.AxialMode = default(MovementForwardControls); ;
         this.CircumferentialMode = default(MovementForwardControls); ;
         this.CornerAxialMode = default(MovementForwardControls); ;
         this.LaunchAxialMode = default(MovementForwardControls); ;
      }

      public void Set(MovementMotorParameters parameters)
      {
         this.Location = parameters.Location;
         this.State = parameters.State;
         this.Direction = parameters.Direction;
         this.AxialMode = parameters.AxialMode;
         this.CircumferentialMode = parameters.CircumferentialMode;
         this.CornerAxialMode = parameters.CornerAxialMode;
         this.LaunchAxialMode = parameters.LaunchAxialMode;
      }
   }
}