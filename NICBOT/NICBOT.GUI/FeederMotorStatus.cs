
namespace NICBOT.GUI
{
   using System;

   using NICBOT.CAN;

   public class FeederMotorStatus
   {
      public ElmoWhistleMotor.Modes requestedMode;
      public double requestedVelocity;
      public double requestedCurrent;

      public FeederMotorStatus()
      {
         this.Initialize();
      }

      public void Initialize()
      {
         this.requestedMode = ElmoWhistleMotor.Modes.undefined;
         this.requestedVelocity = 0;
         this.requestedCurrent = 0;
      }
   }
}