
namespace NICBOT.GUI
{
   using System;

   public class CautionParameter
   {
      public string Name;
      public double DangerHighLimit;
      public double WarningHighLimit;
      public double WarningLowLimit;
      public double DangerLowLimit;

      public CautionParameter()
      {
         this.Name = "";
         this.DangerHighLimit = 0;
         this.WarningHighLimit = 0;
         this.WarningLowLimit = 0;
         this.DangerLowLimit = 0;
      }

      public CautionParameter(string name, double dangerHighLimit, double warningHighLimit, double warningLowLimit, double dangerLowLimit)
      {
         this.Name = name;
         this.DangerHighLimit = dangerHighLimit;
         this.WarningHighLimit = warningHighLimit;
         this.WarningLowLimit = warningLowLimit;
         this.DangerLowLimit = dangerLowLimit;
      }
   }
}
