
namespace NICBOT.GUI
{
   using System;

   public class PumpParameters
   {
      public string Location;

      public bool PressureAutoFill;
      public bool AutoNozzleRetraction;
      public bool AutoPressureRelief;
      public double RpmPerVolt;
      public double PsiPerVolt;

      public ValueParameter AutoFillVolume;
      public ValueParameter MaximumVolume;
      public ValueParameter AutoFillPressure;
      public ValueParameter MaximumPressure;

      public ValueParameter RelievedPressure;
      public ValueParameter ForwardSpeed;
      public ValueParameter ReverseSpeed;
      public ValueParameter MaximumSpeed;

      public ValueParameter SealantWeight;
      public ValueParameter FlowConstant;

      public PumpParameters()
      {
         this.PressureAutoFill = true;
         this.AutoNozzleRetraction = false;
         this.AutoPressureRelief = false;
         this.RpmPerVolt = 50;
         this.PsiPerVolt = 20;

         this.AutoFillVolume = default(ValueParameter);
         this.MaximumVolume = default(ValueParameter);
         this.AutoFillPressure = default(ValueParameter);
         this.MaximumPressure = default(ValueParameter);

         this.RelievedPressure = default(ValueParameter);
         this.ForwardSpeed = default(ValueParameter);
         this.ReverseSpeed = default(ValueParameter);
         this.MaximumSpeed = default(ValueParameter);

         this.SealantWeight = default(ValueParameter);
         this.FlowConstant = default(ValueParameter);
      }
   }
}