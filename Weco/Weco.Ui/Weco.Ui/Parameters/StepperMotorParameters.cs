
namespace Weco.Ui
{
   using System;

   public class StepperMotorParameters
   {
      public string Location;

      public int HomingMethod;
      public int HomeOffset;
      public int HomingSwitchVelocity;
      public int HomingZeroVelocity;
      public int HomingAcceleration;

      public int ProfileVelocity;
      public int ProfileAcceleration;

      public int MaximumPosition;
      public int CenterPosition;
      public int MinimumPosition;

      public int Polarity;
      public int PositionNotationIndex;
      public int VelocityNotationIndex;
      public int VelocityDimensionIndex;
      public int AccelerationNotationIndex;
      public int AccelerationDimensionIndex;
      public int PositionEncoderIncrements;
      public int PositionEncoderMotorRevolutions;
      public int VelocityEncoderIncrementsPerSecond;
      public int VelocityEncoderMotorRevolutionsPerSecond;
      public int GearRatioMotorRevolutions;
      public int GearRatioShaftRevolutions;
      public int FeedConstantFeed;
      public int FeedConstantShaftRevolutions;

      public StepperMotorParameters()
      {
         this.Location = "";

         this.HomingMethod = 0;
         this.HomeOffset = 0;
         this.HomingSwitchVelocity = 0;
         this.HomingZeroVelocity = 0;
         this.HomingAcceleration = 0;

         this.ProfileVelocity = 0;
         this.ProfileAcceleration = 0;

         this.MaximumPosition = 0;
         this.CenterPosition = 0;
         this.MinimumPosition = 0;

         this.Polarity = 0;
         this.PositionNotationIndex = 0;
         this.VelocityNotationIndex = 0;
         this.VelocityDimensionIndex = 0;
         this.AccelerationNotationIndex = 0;
         this.AccelerationDimensionIndex = 0;
         this.PositionEncoderIncrements = 0;
         this.PositionEncoderMotorRevolutions = 0;
         this.VelocityEncoderIncrementsPerSecond = 0;
         this.VelocityEncoderMotorRevolutionsPerSecond = 0;
         this.GearRatioMotorRevolutions = 0;
         this.GearRatioShaftRevolutions = 0;
         this.FeedConstantFeed = 0;
         this.FeedConstantShaftRevolutions = 0;
      }
   }
}