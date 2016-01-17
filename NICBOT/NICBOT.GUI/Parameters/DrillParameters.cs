
namespace NICBOT.GUI
{
   using System;

   public class DrillParameters
   {
      public string Location;

      public ValueParameter Acceleration;
      public ValueParameter ErrorLimit;
      public ValueParameter ProportionalControlConstant;
      public ValueParameter IntegralControlConstant;
      public ValueParameter DerivativeControlConstant;

      public ValueParameter RotationSpeed;
      public ValueParameter SearchSpeed;
      public ValueParameter TravelSpeed;
      public ValueParameter CuttingSpeed;

      public double SpeedToVelocityCount;

      public bool AutoOrign;
      public bool PeckMode;
      public bool PositionRetract;

      public ValueParameter CuttingDepth;
      public ValueParameter CuttingIncrement;
      public ValueParameter RetractDistance;
      public ValueParameter RetractPosition;

      public ValueParameter ExtendedDistance;

      public DrillParameters()
      {
         this.Location = "";

         this.Acceleration = default(ValueParameter);
         this.ErrorLimit = default(ValueParameter);
         this.ProportionalControlConstant = default(ValueParameter);
         this.IntegralControlConstant = default(ValueParameter);
         this.DerivativeControlConstant = default(ValueParameter);

         this.RotationSpeed = default(ValueParameter);
         this.SearchSpeed = default(ValueParameter);
         this.TravelSpeed = default(ValueParameter);
         this.CuttingSpeed = default(ValueParameter);

         this.SpeedToVelocityCount = 1;

         this.AutoOrign = true;
         this.PeckMode = false;
         this.PositionRetract = false;

         this.CuttingDepth = default(ValueParameter); ;
         this.CuttingIncrement = default(ValueParameter); ;
         this.RetractDistance = default(ValueParameter); ;
         this.RetractPosition = default(ValueParameter); ;

         this.ExtendedDistance = default(ValueParameter); ;
      }
   }
}