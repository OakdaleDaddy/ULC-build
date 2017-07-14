
namespace Weco.Ui
{
   using System;

   public class ValueParameter
   {
      public string Name;
      public string Unit;
      public int Precision;
      public double MinimumValue;
      public double MaximumValue;
      public double StepValue;
      public double DefaultValue;
      public double OperationalValue;

      public ValueParameter()
      {
         this.Name = "";
         this.Unit = "";
         this.Precision = 0;
         this.MinimumValue = 0;
         this.MaximumValue = 0;
         this.StepValue = 0;
         this.DefaultValue = 0;
         this.OperationalValue = 0;
      }

      public ValueParameter(string name, string unit, int precision, double minimumValue, double maximumValue, double stepValue, double defaultValue, double operationalValue)
      {
         this.Name = name;
         this.Unit = unit;
         this.Precision = precision;
         this.MinimumValue = minimumValue;
         this.MaximumValue = maximumValue;
         this.StepValue = stepValue;
         this.DefaultValue = defaultValue;
         this.OperationalValue = operationalValue;
      }

      public ValueParameter(ValueParameter parameter)
      {
         this.Name = parameter.Name;
         this.Unit = parameter.Unit;
         this.Precision = parameter.Precision;
         this.MinimumValue = parameter.MinimumValue;
         this.MaximumValue = parameter.MaximumValue;
         this.StepValue = parameter.StepValue;
         this.DefaultValue = parameter.DefaultValue;
         this.OperationalValue = parameter.OperationalValue;
      }
   }
}