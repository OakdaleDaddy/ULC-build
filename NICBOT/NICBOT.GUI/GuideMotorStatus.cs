
namespace NICBOT.GUI
{
   using System;

   using NICBOT.CAN;

   public class GuideMotorStatus
   {
      public GuideDirections direction;
      public double requestedVelocity;

      public bool RetractionLimit { set; get; }
      public bool ExtensionLimit { set; get; }

      public GuideMotorStatus()
      {
         this.Initialize();
      }

      public void Initialize()
      {
         this.direction = GuideDirections.off;
         this.requestedVelocity = 0;
      }
   }
}