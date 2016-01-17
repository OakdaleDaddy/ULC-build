namespace NICBOT.GUI
{
   using System;
   using NICBOT.Utilities;

   public class SimulatedPumpPressure
   {
      #region Fields

      private static SimulatedPumpPressure front = null;
      private static SimulatedPumpPressure rear = null;

      private ToolLocations location;
      private Random cavityRandomValue;

      private double cavityVolume;

      private DateTime cavityFilledTime;
      private bool cavityFilled;
      private int leakageAAgeLimit;
      private int leakageBAgeLimit;
      private double leakageAConstant;
      private double leakageBConstant;

      private int lastPressureReading;
      private bool lastPressureShown;

      #endregion

      #region Properties

      public static SimulatedPumpPressure Front
      {
         get
         {
            if (null == front)
            {
               front = new SimulatedPumpPressure(ToolLocations.front);
               front.Initialize();
            }

            return front;
         }
      }

      public static SimulatedPumpPressure Rear
      {
         get
         {
            if (null == rear)
            {
               rear = new SimulatedPumpPressure(ToolLocations.rear);
               rear.Initialize();
            }

            return rear;
         }
      }

      #endregion

      #region Helper Functions

      private void Initialize()
      {
         this.cavityRandomValue = new Random();

         this.lastPressureReading = 0;
         this.lastPressureShown = false;
      }

      private double GetSealantLeakage()
      {
         TimeSpan ts = DateTime.Now - this.cavityFilledTime;
         double age = ts.TotalSeconds;
         double leakage = 0;

         if (false != this.cavityFilled)
         {

            if (age > (this.leakageBAgeLimit + this.leakageAAgeLimit))
            {
               leakage = (this.leakageBConstant * this.leakageBAgeLimit);
               leakage += (this.leakageAConstant * this.leakageAAgeLimit);
            }
            else if (age > this.leakageAAgeLimit)
            {
               leakage = (this.leakageBConstant * (age - this.leakageAAgeLimit));
               leakage += (this.leakageAConstant * this.leakageAAgeLimit);
            }
            else
            {
               leakage = (this.leakageAConstant * age);
            }
         }

         //Tracer.WriteHigh(TraceGroup.PUMP, "", "sim {0} pressue leakage {1} {2}", this.location.ToString(), leakage, age);
         return (leakage);
      }

      #endregion

      #region Constructor

      private SimulatedPumpPressure(ToolLocations location)
      {
         this.location = location;
      }

      #endregion

      #region Access Functions

      public void CreateCavity()
      {
         this.cavityVolume = this.cavityRandomValue.Next(50, 400);

         this.cavityFilled = false;

         this.leakageAAgeLimit = cavityRandomValue.Next(25, 45);
         this.leakageBAgeLimit = cavityRandomValue.Next(25, 45);
         this.leakageAConstant = (double)cavityRandomValue.Next(50, 67) / 100;
         this.leakageBConstant = (double)cavityRandomValue.Next(10, 50) / 100;

         this.lastPressureShown = false;
         Tracer.WriteHigh(TraceGroup.PUMP, "", "{0} cavity {1} {2} {3} {4} {5}", this.location.ToString(), this.cavityVolume, this.leakageAAgeLimit, this.leakageBAgeLimit, this.leakageAConstant, this.leakageBConstant);
      }

      public double GetPressure(double sealantVolume)
      {
         double result = 0;
         bool nozzleExtended = NicBotComm.Instance.GetNozzleExtended(this.location);

         if (false != nozzleExtended)
         {
            if (0 == this.cavityVolume)
            {
               this.CreateCavity();
            }

            if (sealantVolume >= this.cavityVolume)
            {
               if (false == this.cavityFilled)
               {
                  this.cavityFilled = true;
                  this.cavityFilledTime = DateTime.Now;
               }

               double sealantLeakage = this.GetSealantLeakage();
               double adjustedSealantVolume = sealantVolume - sealantLeakage;

               if (adjustedSealantVolume >= this.cavityVolume)
               {
                  double e = 200; // compression constant
                  double effectiveVolume = this.cavityVolume / ((adjustedSealantVolume - this.cavityVolume) * e);
                  result = 1 / effectiveVolume;
               }
            }
         }

         int pressureToShow = (int)result;

         if ((false == this.lastPressureShown) || (pressureToShow != this.lastPressureReading))
         {
            Tracer.WriteHigh(TraceGroup.PUMP, "", "sim {0} pressue {1}", this.location.ToString(), pressureToShow);
            this.lastPressureReading = pressureToShow;
            this.lastPressureShown = true;
         }

         return (result);
      }

      #endregion

   }
}