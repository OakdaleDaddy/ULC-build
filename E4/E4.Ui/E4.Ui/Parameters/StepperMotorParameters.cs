
namespace E4.Ui
{
   using System;

   public class StepperMotorParameters
   {
      public string Location;

      public int HomeOffset;
      public int Maximum;

      public StepperMotorParameters()
      {
         this.Location = "";
         this.HomeOffset = 0;
         this.Maximum = 0;
      }

      public void Set(StepperMotorParameters parameters)
      {
         this.Location = parameters.Location;
         this.HomeOffset = parameters.HomeOffset;
         this.Maximum = parameters.Maximum;
      }
   }
}