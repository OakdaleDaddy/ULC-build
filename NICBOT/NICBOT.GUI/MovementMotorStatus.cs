
namespace NICBOT.GUI
{
   using System;

   using NICBOT.CAN;

   public class MovementMotorStatus
   {
      public UlcRoboticsNicbotWheel.Modes requestedMode;
      public double requestedVelocity;
      public double requestedCurrent;

      public MovementMotorStatus()
      {
         this.Initialize();
      }

      public void Initialize()
      {
         this.requestedMode = UlcRoboticsNicbotWheel.Modes.undefined;
         this.requestedVelocity = 0;
         this.requestedCurrent = 0;
      }
   }
}