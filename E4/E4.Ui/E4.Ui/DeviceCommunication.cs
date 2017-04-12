namespace E4.Ui
{
   using System;
   using System.Threading;

   using E4.Utilities;

   public class DeviceCommunication
   {
      #region Fields

      private static DeviceCommunication instance = null;

      private bool execute;
      private Thread thread;
      private bool ready;
      private bool running;

      #endregion

      #region Properties

      public static DeviceCommunication Instance
      {
         get
         {
            if (instance == null)
            {
               instance = new DeviceCommunication();
               instance.Initialize();
            }

            return instance;
         }
      }

      public bool Ready
      {
         get
         {
            return (this.ready);
         }
      }

      public bool Running
      {
         get
         {
            return (this.running);
         }
      }

      #endregion

      #region Process

      private void InitializeValues()
      {
      }

      private void ExecuteProcessLoop()
      {
         this.ready = true;

         for (; this.execute; )
         {
            //this.UpdateMacro();
            Thread.Sleep(1);
         }
      }

      private void Process()
      {
         try
         {
            this.running = true;
            Tracer.WriteHigh(TraceGroup.COMM, null, "start");

            this.InitializeValues();

            Tracer.WriteHigh(TraceGroup.COMM, null, "osd init start");
            VideoStampOsd.Instance.Start(1, 9600);
            VideoStampOsd.Instance.SetDateAndTime(DateTime.Now);
            VideoStampOsd.Instance.Configure(ParameterAccessor.Instance.Osd);
            Tracer.WriteHigh(TraceGroup.COMM, null, "osd init complete");

            LaserCommunicationBus.Instance.Start();
            TargetCommunicationBus.Instance.Start();

            bool waiting = true;

            for (; waiting; )
            {
               bool busReady = true;

               Thread.Sleep(50);

               busReady = (busReady && LaserCommunicationBus.Instance.Ready);
               busReady = (busReady && TargetCommunicationBus.Instance.Ready);

               waiting = (waiting && this.execute);
               waiting = (waiting && !busReady);
            }

            this.ExecuteProcessLoop();
         }
         catch (Exception preException)
         {
            Tracer.WriteError(TraceGroup.COMM, null, "process exception {0}", preException.Message);
         }

         try
         {
            LaserCommunicationBus.Instance.Stop();
            TargetCommunicationBus.Instance.Stop();

            bool waiting = true;
            DateTime waitTimeLimit = DateTime.Now.AddSeconds(30);

            for (; waiting; )
            {
               bool busRunning = false;

               Thread.Sleep(50);

               busRunning = (busRunning || LaserCommunicationBus.Instance.Running);
               busRunning = (busRunning || TargetCommunicationBus.Instance.Running);

               waiting = (waiting && busRunning);

               if (DateTime.Now > waitTimeLimit)
               {
                  Tracer.WriteError(TraceGroup.COMM, null, "stop time limit exceeded");
                  waiting = false;
               }
            }

            VideoStampOsd.Instance.Stop();
            this.InitializeValues();
         }
         catch (Exception postException)
         {
            Tracer.WriteError(TraceGroup.COMM, null, "post process exception {0}", postException.Message);
         }

         this.ready = false;
         Tracer.WriteHigh(TraceGroup.COMM, null, "stop");
         this.thread = null;
         this.running = false;
      }

      #endregion

      #region Constructor

      private DeviceCommunication()
      {
      }

      #endregion

      #region Access Methods

      #region Control Functions

      private void Initialize()
      {
      }

      public void Start()
      {
         this.thread = new Thread(this.Process);
         this.thread.IsBackground = true;
         this.thread.Name = "Device Communication";

         this.ready = false;
         this.execute = true;
         this.thread.Start();
      }

      public void Stop()
      {
         this.execute = false;
      }

      public void Service()
      {
         LaserCommunicationBus.Instance.Service();
         TargetCommunicationBus.Instance.Service();
      }

      public string GetMainFaultStatus()
      {
         string result = LaserCommunicationBus.Instance.GetFaultStatus();
         return (result);
      }

      public string GetMainWarningStatus()
      {
         string result = LaserCommunicationBus.Instance.GetWarningStatus();
         return (result);
      }

      public string GetTargetFaultStatus()
      {
         string result = TargetCommunicationBus.Instance.GetFaultStatus();
         return (result);
      }

      public string GetTargetWarningStatus()
      {
         string result = TargetCommunicationBus.Instance.GetWarningStatus();
         return (result);
      }

      public void StopAll()
      {
#if false
         if (null != this.activeMacro)
         {
            this.activeMacro.Cancel();
         }
#endif

         LaserCommunicationBus.Instance.StopAll();
         TargetCommunicationBus.Instance.StopAll();
      }

      #endregion

      #region Laser Movement Functions

      public void SetLaserMovementManualMode(bool active)
      {
         LaserCommunicationBus.Instance.SetLaserMovementManualMode(active);
      }

      public void SetLaserMovementMode(MovementModes mode)
      {
         LaserCommunicationBus.Instance.SetLaserMovementMode(mode);
      }

      public void SetLaserMovementPositionRequest(double request)
      {
         LaserCommunicationBus.Instance.SetLaserMovementPositionRequest(request);
      }
     
      public void SetLaserMovementVelocityRequest(double request, bool triggered)
      {
         LaserCommunicationBus.Instance.SetLaserMovementVelocityRequest(request, triggered);
      }
     
      public bool GetLaserMovementManualMode()
      {
         return (LaserCommunicationBus.Instance.GetLaserMovementManualMode());
      }

      public MovementModes GetLaserMovementMode()
      {
         return (LaserCommunicationBus.Instance.GetLaserMovementMode());
      }

      public void GetLaserMovementRequestValues(ref ValueParameter movementParameter, ref double movementRequestValue)
      {
         LaserCommunicationBus.Instance.GetLaserMovementRequestValues(ref movementParameter, ref movementRequestValue);
      }

      public double GetLaserMovementValue()
      {
         return (LaserCommunicationBus.Instance.GetLaserMovementValue());
      }

      public bool GetLaserMovementActivated()
      {
         return (LaserCommunicationBus.Instance.GetLaserMovementActivated());
      }

      public double GetLaserWheelCurrentValue(WheelLocations location)
      {
         return (LaserCommunicationBus.Instance.GetLaserWheelCurrentValue(location));
      }

      public double GetLaserWheelTemperatureValue(WheelLocations location)
      {
         return (LaserCommunicationBus.Instance.GetLaserWheelTemperatureValue(location));
      }

      public double GetLaserWheelPositionValue(WheelLocations location)
      {
         return (LaserCommunicationBus.Instance.GetLaserWheelPositionValue(location));
      }

      public double GetLaserLinkVoltage()
      {
         return (LaserCommunicationBus.Instance.GetLaserLinkVoltage());
      }

      #endregion

      #region Laser Stepper Functions

      public void SetLaserCenter()
      {
         LaserCommunicationBus.Instance.SetLaserCenter();
      }

      public void SetLaserStepperXPosition(int position)
      {
         LaserCommunicationBus.Instance.SetLaserStepperXPosition(position);
      }

      public void StopLaserStepperX()
      {
         LaserCommunicationBus.Instance.StopLaserStepperX();
      }      

      public void SetLaserStepperYPosition(int position)
      {
         LaserCommunicationBus.Instance.SetLaserStepperYPosition(position);
      }

      public void StopLaserStepperY()
      {
         LaserCommunicationBus.Instance.StopLaserStepperY();
      }

      public int GetLaserStepperXActualPosition()
      {
         return(LaserCommunicationBus.Instance.GetLaserStepperXActualPosition());
      }

      public int GetLaserStepperYActualPosition()
      {
         return (LaserCommunicationBus.Instance.GetLaserStepperYActualPosition());
      }

      public bool LaserXPositionObtained()
      {
         return (LaserCommunicationBus.Instance.LaserXPositionObtained());
      }

      public bool LaserYPositionObtained()
      {
         return (LaserCommunicationBus.Instance.LaserYPositionObtained());
      }

      #endregion

      #region Target Movement Functions

      public void SetTargetMovementManualMode(bool active)
      {
         TargetCommunicationBus.Instance.SetTargetMovementManualMode(active);
      }

      public void SetTargetMovementMode(MovementModes mode)
      {
         TargetCommunicationBus.Instance.SetTargetMovementMode(mode);
      }

      public void SetTargetMovementPositionRequest(double request)
      {
         TargetCommunicationBus.Instance.SetTargetMovementPositionRequest(request);
      }

      public void SetTargetMovementVelocityRequest(double request, bool triggered)
      {
         TargetCommunicationBus.Instance.SetTargetMovementVelocityRequest(request, triggered);
      }

      public bool GetTargetMovementManualMode()
      {
         return (TargetCommunicationBus.Instance.GetTargetMovementManualMode());
      }

      public MovementModes GetTargetMovementMode()
      {
         return (TargetCommunicationBus.Instance.GetTargetMovementMode());
      }

      public void GetTargetMovementRequestValues(ref ValueParameter movementParameter, ref double movementRequestValue)
      {
         TargetCommunicationBus.Instance.GetTargetMovementRequestValues(ref movementParameter, ref movementRequestValue);
      }

      public double GetTargetMovementValue()
      {
         return (TargetCommunicationBus.Instance.GetTargetMovementValue());
      }

      public bool GetTargetMovementActivated()
      {
         return (TargetCommunicationBus.Instance.GetTargetMovementActivated());
      }

      public double GetTargetWheelCurrentValue(WheelLocations location)
      {
         return (TargetCommunicationBus.Instance.GetTargetWheelCurrentValue(location));
      }

      public double GetTargetWheelTemperatureValue(WheelLocations location)
      {
         return (TargetCommunicationBus.Instance.GetTargetWheelTemperatureValue(location));
      }

      public double GetTargetWheelPositionValue(WheelLocations location)
      {
         return (TargetCommunicationBus.Instance.GetTargetWheelPositionValue(location));
      }
      
      public double GetTargetLinkVoltage()
      {
         return (TargetCommunicationBus.Instance.GetTargetLinkVoltage());
      }
      
      #endregion

      #region Target Stepper Functions
      
      public void SetTargetCenter()
      {
         TargetCommunicationBus.Instance.SetTargetCenter();
      }

      public void SetTargetStepperPosition(int position)
      {
         TargetCommunicationBus.Instance.SetTargetStepperPosition(position);
      }

      public void StopTargetStepper()
      {
         TargetCommunicationBus.Instance.StopTargetStepper();
      }

      public int GetTargetStepperActualPosition()
      {
         return (TargetCommunicationBus.Instance.GetTargetStepperActualPosition());
      }

      public bool TargetPositionObtained()
      {
         return (TargetCommunicationBus.Instance.TargetPositionObtained());
      }

      #endregion

      #region Laser Functions

      public void SetLaserAim(bool on)
      {
         LaserCommunicationBus.Instance.SetLaserAim(on);
      }

      public bool GetLaserAim()
      {
         return (LaserCommunicationBus.Instance.GetLaserAim());
      }

      public void StartLaserMeasurement()
      {
         LaserCommunicationBus.Instance.StartLaserMeasurement();
      }

      public void CancelLaserMeasurement()
      {
         LaserCommunicationBus.Instance.CancelLaserMeasurement();
      }

      public bool GetLaserMeasurementActivity()
      {
         return (LaserCommunicationBus.Instance.GetLaserMeasurementActivity());
      }

      public int GetLaserSampleRemainingCount()
      {
         return (LaserCommunicationBus.Instance.GetLaserSampleRemainingCount());
      }

      public bool GetLaserMeasurementReady()
      {
         return (LaserCommunicationBus.Instance.GetLaserMeasurementReady());
      }

      public double GetAverageLaserMeasurement()
      {
         return (LaserCommunicationBus.Instance.GetAverageLaserMeasurement());
      }

      public UInt32 GetTargetScannerCoordinates()
      {
         return (TargetCommunicationBus.Instance.GetTargetScannerCoordinates());
      }

      public double GetTargetPitch()
      {
         return (TargetCommunicationBus.Instance.GetTargetPitch());
      }

      #endregion

      #endregion
   }
}