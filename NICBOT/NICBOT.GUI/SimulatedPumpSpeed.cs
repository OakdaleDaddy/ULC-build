namespace NICBOT.GUI
{
   using System;
   using NICBOT.Utilities;

   public class SimulatedPumpSpeed
   {
      #region Fields

      private static SimulatedPumpSpeed front = null;
      private static SimulatedPumpSpeed rear = null;

      private ToolLocations location;
      private double lastSpeed;
      private DateTime updateTime;

      #endregion

      #region Properties

      public static SimulatedPumpSpeed Front
      {
         get
         {
            if (null == front)
            {
               front = new SimulatedPumpSpeed(ToolLocations.front);
               front.Initialize();
            }

            return front;
         }
      }

      public static SimulatedPumpSpeed Rear
      {
         get
         {
            if (null == rear)
            {
               rear = new SimulatedPumpSpeed(ToolLocations.rear);
               rear.Initialize();
            }

            return rear;
         }
      }

      #endregion

      #region Helper Functions

      private void Initialize()
      {
         this.lastSpeed = 0;
         this.updateTime = DateTime.Now;
      }

      #endregion

      #region Constructor 

      private SimulatedPumpSpeed(ToolLocations location)
      {
         this.location = location;
      }

      #endregion

      public double GetSpeed(bool on, double setting, PumpDirections direction)
      {
         double speed = this.lastSpeed;
         double requestedSpeed = (false != on) ? setting : 0;
         double requestedVelocity = (PumpDirections.forward == direction) ? requestedSpeed : -requestedSpeed;

         DateTime now = DateTime.Now;
         if (now > this.updateTime)
         {
            this.updateTime = now.AddMilliseconds(100);

            if (this.lastSpeed > requestedVelocity)
            {
               double difference = this.lastSpeed - requestedVelocity;
               double step = difference / 5;
               step = (step < 0.01) ? difference : step;
               speed = this.lastSpeed - step;
            }
            else if (this.lastSpeed < requestedVelocity)
            {
               double difference = requestedVelocity - this.lastSpeed;
               double step = difference / 5;
               step = (step < 0.01) ? difference : step;
               speed = this.lastSpeed + step;
            }

            this.lastSpeed = speed;
         }

         return (speed);
      }

   }
}