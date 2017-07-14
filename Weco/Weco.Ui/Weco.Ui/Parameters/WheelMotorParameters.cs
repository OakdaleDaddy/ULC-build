
namespace Weco.Ui
{
   using System;

   public class WheelMotorParameters
   {
      public string Location;

      public WheelMotorStates MotorState;
      public ActuationModes ActuationMode;
      public bool RequestInverted;
      public bool PositionInverted;

      public int ProfileVelocity;
      public int ProfileAcceleration;
      public int ProfileDeceleration;

      public int Kp;
      public int Ki;
      public int Kd;

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

      public int MotorPeakCurrentLimit;
      public int MaximumCurrent;
      public int MotorRatedCurrent;

      public WheelMotorParameters()
      {
         this.Location = "";

         this.MotorState = WheelMotorStates.enabled;
         this.ActuationMode = ActuationModes.closedloop;
         this.RequestInverted = false;
         this.PositionInverted = false;

         this.ProfileVelocity = 0;
         this.ProfileAcceleration = 0;
         this.ProfileDeceleration = 0;

         this.Kp = 0;
         this.Ki = 0;
         this.Kd = 0;

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

         this.MotorPeakCurrentLimit = 0;
         this.MaximumCurrent = 0;
         this.MotorRatedCurrent = 0;
      }
   }
}