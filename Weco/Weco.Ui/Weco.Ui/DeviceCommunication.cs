
namespace Weco.Ui
{
   using System;
   using System.Threading;

   using Weco.Utilities;

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

            NumatoUsbRelay.Instance.Initialize();
            NumatoUsbRelay.Instance.Start(ParameterAccessor.Instance.UsbRelayPort);

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

            NumatoUsbRelay.Instance.Stop();

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

      #region Laser Body Functions

      public double GetLaserMainRoll()
      {
         return (LaserCommunicationBus.Instance.GetLaserMainRoll());
      }

      public double GetLaserMainPitch()
      {
         return (LaserCommunicationBus.Instance.GetLaserMainPitch());
      }

      public double GetLaserMainYaw()
      {
         return (LaserCommunicationBus.Instance.GetLaserMainYaw());
      }

      #endregion

      #region Laser Movement Functions

      public void SetLaserMovementLock(bool locked)
      {
         LaserCommunicationBus.Instance.SetLaserMovementLock(locked);
      }

      public bool GetLaserMovementLock()
      {
         return (LaserCommunicationBus.Instance.GetLaserMovementLock());
      }

      public void ResetLaserMoved()
      {
         LaserCommunicationBus.Instance.ResetLaserMoved();
      }

      public bool GetLaserMoved()
      {
         return (LaserCommunicationBus.Instance.GetLaserMoved());
      }

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

      public double GetLaserWheelTotalPositionValue()
      {
         return (LaserCommunicationBus.Instance.GetLaserWheelTotalPositionValue());
      }

      public double GetLaserWheelTripPositionValue()
      {
         return (LaserCommunicationBus.Instance.GetLaserWheelTripPositionValue());
      }

      #endregion

      #region Target Body Functions

      public double GetTargetMainRoll()
      {
         return (TargetCommunicationBus.Instance.GetTargetMainRoll());
      }

      public double GetTargetMainPitch()
      {
         return (TargetCommunicationBus.Instance.GetTargetMainPitch());
      }

      public double GetTargetMainYaw()
      {
         return (TargetCommunicationBus.Instance.GetTargetMainYaw());
      }

      public double GetTargetTopCameraRoll()
      {
         return (TargetCommunicationBus.Instance.GetTargetTopCameraRoll());
      }

      #endregion

      #region Target Movement Functions

      public void SetTargetMovementLock(bool locked)
      {
         TargetCommunicationBus.Instance.SetTargetMovementLock(locked);
      }

      public bool GetTargetMovementLock()
      {
         return (TargetCommunicationBus.Instance.GetTargetMovementLock());
      }

      public void ResetTargetMoved()
      {
         TargetCommunicationBus.Instance.ResetTargetMoved();
      }

      public bool GetTargetMoved()
      {
         return (TargetCommunicationBus.Instance.GetTargetMoved());
      }

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
      
      public double GetTargetWheelTotalPositionValue()
      {
         return (TargetCommunicationBus.Instance.GetTargetWheelTotalPositionValue());
      }

      public double GetTargetWheelTripPositionValue()
      {
         return (TargetCommunicationBus.Instance.GetTargetWheelTripPositionValue());
      }
      
      #endregion

      #region Lights and Camera

      public void SetLightLevel(Controls.SystemLocations systemLocation, int level)
      {
         if ((Controls.SystemLocations.crawlerLeft == systemLocation) ||
             (Controls.SystemLocations.crawlerRight == systemLocation) ||
             (Controls.SystemLocations.crawlerFront == systemLocation) ||
             (Controls.SystemLocations.crawlerRear == systemLocation))
         {
            LaserCommunicationBus.Instance.SetLightLevel(systemLocation, level);
         }
         else if ((Controls.SystemLocations.bulletLeft == systemLocation) ||
                  (Controls.SystemLocations.bulletRight == systemLocation) ||
                  (Controls.SystemLocations.bulletDown == systemLocation))
         {
            TargetCommunicationBus.Instance.SetLightLevel(level);
         }
      }

      public int GetLightLevel(Controls.SystemLocations systemLocation)
      {
         int result = 0;

         if ((Controls.SystemLocations.crawlerLeft == systemLocation) ||
             (Controls.SystemLocations.crawlerRight == systemLocation) ||
             (Controls.SystemLocations.crawlerFront == systemLocation) ||
             (Controls.SystemLocations.crawlerRear == systemLocation))
         {
            result = LaserCommunicationBus.Instance.GetLightLevel(systemLocation);
         }
         else if ((Controls.SystemLocations.bulletLeft == systemLocation) ||
                  (Controls.SystemLocations.bulletRight == systemLocation) ||
                  (Controls.SystemLocations.bulletDown == systemLocation))
         {
            result = TargetCommunicationBus.Instance.GetLightLevel();
         }

         return (result);
      }

      public void SetLightChannelMask(Controls.SystemLocations systemLocation, int mask)
      {
         if ((Controls.SystemLocations.crawlerLeft == systemLocation) ||
             (Controls.SystemLocations.crawlerRight == systemLocation) ||
             (Controls.SystemLocations.crawlerFront == systemLocation) ||
             (Controls.SystemLocations.crawlerRear == systemLocation))
         {
            LaserCommunicationBus.Instance.SetLightChannelMask(systemLocation, mask);
         }
         else if ((Controls.SystemLocations.bulletLeft == systemLocation) ||
                  (Controls.SystemLocations.bulletRight == systemLocation) ||
                  (Controls.SystemLocations.bulletDown == systemLocation))
         {
            TargetCommunicationBus.Instance.SetLightChannelMask(mask);
         }
      }

      public bool GetLightEnable(Controls.SystemLocations systemLocation)
      {
         bool result = false;

         if ((Controls.SystemLocations.crawlerLeft == systemLocation) ||
             (Controls.SystemLocations.crawlerRight == systemLocation) ||
             (Controls.SystemLocations.crawlerFront == systemLocation) ||
             (Controls.SystemLocations.crawlerRear == systemLocation))
         {
            result = LaserCommunicationBus.Instance.GetLightEnable(systemLocation);
         }
         else if ((Controls.SystemLocations.bulletLeft == systemLocation) ||
                  (Controls.SystemLocations.bulletRight == systemLocation) ||
                  (Controls.SystemLocations.bulletDown == systemLocation))
         {
            result = TargetCommunicationBus.Instance.GetLightEnable(systemLocation);
         }

         return (result);
      }

      public void SetCrawlerCamera(Controls.SystemLocations systemLocation)
      {
         LaserCommunicationBus.Instance.SetCrawlerCamera(systemLocation);
      }

      public void SetBulletCamera(Controls.SystemLocations systemLocation)
      {
         TargetCommunicationBus.Instance.SetBulletCamera(systemLocation);
      }

      #endregion

      #endregion
   }
}