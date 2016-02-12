
namespace NICBOT.GUI
{
   using System;

   using NICBOT.CAN;

   public class FeederMotorStatus
   {
      public ElmoWhistleMotor.ControlModes requestedControlMode;
      public ElmoWhistleMotor.Modes requestedMode;
      public double requestedVelocity;
      public double requestedCurrent;

      public FeederMotorStatus()
      {
         this.Initialize();
      }

      public void Initialize()
      {
         this.requestedControlMode = ElmoWhistleMotor.ControlModes.singleLoopPosition;
         this.requestedMode = ElmoWhistleMotor.Modes.undefined;
         this.requestedVelocity = 0;
         this.requestedCurrent = 0;
      }
   }
}