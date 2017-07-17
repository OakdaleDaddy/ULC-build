
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

            RobotCommunicationBus.Instance.Start();
            TruckCommunicationBus.Instance.Start();

            NumatoUsbRelay.Instance.Initialize();
            NumatoUsbRelay.Instance.Start(ParameterAccessor.Instance.UsbRelayPort);

            bool waiting = true;

            for (; waiting; )
            {
               bool busReady = true;

               Thread.Sleep(50);

               busReady = (busReady && RobotCommunicationBus.Instance.Ready);
               busReady = (busReady && TruckCommunicationBus.Instance.Ready);

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
            RobotCommunicationBus.Instance.Stop();
            TruckCommunicationBus.Instance.Stop();

            NumatoUsbRelay.Instance.Stop();

            bool waiting = true;
            DateTime waitTimeLimit = DateTime.Now.AddSeconds(30);

            for (; waiting; )
            {
               bool busRunning = false;

               Thread.Sleep(50);

               busRunning = (busRunning || RobotCommunicationBus.Instance.Running);
               busRunning = (busRunning || TruckCommunicationBus.Instance.Running);

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
         RobotCommunicationBus.Instance.Service();
         TruckCommunicationBus.Instance.Service();
      }

      public string GetMainFaultStatus()
      {
         string result = RobotCommunicationBus.Instance.GetFaultStatus();
         return (result);
      }

      public string GetMainWarningStatus()
      {
         string result = RobotCommunicationBus.Instance.GetWarningStatus();
         return (result);
      }

      public string GetTargetFaultStatus()
      {
         string result = TruckCommunicationBus.Instance.GetFaultStatus();
         return (result);
      }

      public string GetTargetWarningStatus()
      {
         string result = TruckCommunicationBus.Instance.GetWarningStatus();
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

         RobotCommunicationBus.Instance.StopAll();
         TruckCommunicationBus.Instance.StopAll();
      }

      #endregion

      #region Laser Body Functions

      public double GetLaserMainRoll()
      {
         return (RobotCommunicationBus.Instance.GetLaserMainRoll());
      }

      public double GetLaserMainPitch()
      {
         return (RobotCommunicationBus.Instance.GetLaserMainPitch());
      }

      public double GetLaserMainYaw()
      {
         return (RobotCommunicationBus.Instance.GetLaserMainYaw());
      }

      #endregion

      #region Laser Movement Functions

      public void SetLaserMovementLock(bool locked)
      {
         RobotCommunicationBus.Instance.SetLaserMovementLock(locked);
      }

      public bool GetLaserMovementLock()
      {
         return (RobotCommunicationBus.Instance.GetLaserMovementLock());
      }

      public void ResetLaserMoved()
      {
         RobotCommunicationBus.Instance.ResetLaserMoved();
      }

      public bool GetLaserMoved()
      {
         return (RobotCommunicationBus.Instance.GetLaserMoved());
      }

      public void SetLaserMovementManualMode(bool active)
      {
         RobotCommunicationBus.Instance.SetLaserMovementManualMode(active);
      }

      public void SetLaserMovementMode(MovementModes mode)
      {
         RobotCommunicationBus.Instance.SetLaserMovementMode(mode);
      }

      public void SetLaserMovementPositionRequest(double request)
      {
         RobotCommunicationBus.Instance.SetLaserMovementPositionRequest(request);
      }
     
      public void SetLaserMovementVelocityRequest(double request, bool triggered)
      {
         RobotCommunicationBus.Instance.SetLaserMovementVelocityRequest(request, triggered);
      }
     
      public bool GetLaserMovementManualMode()
      {
         return (RobotCommunicationBus.Instance.GetLaserMovementManualMode());
      }

      public MovementModes GetLaserMovementMode()
      {
         return (RobotCommunicationBus.Instance.GetLaserMovementMode());
      }

      public void GetLaserMovementRequestValues(ref ValueParameter movementParameter, ref double movementRequestValue)
      {
         RobotCommunicationBus.Instance.GetLaserMovementRequestValues(ref movementParameter, ref movementRequestValue);
      }

      public double GetLaserMovementValue()
      {
         return (RobotCommunicationBus.Instance.GetLaserMovementValue());
      }

      public bool GetLaserMovementActivated()
      {
         return (RobotCommunicationBus.Instance.GetLaserMovementActivated());
      }

      public double GetLaserWheelCurrentValue(WheelLocations location)
      {
         return (RobotCommunicationBus.Instance.GetLaserWheelCurrentValue(location));
      }

      public double GetLaserWheelTemperatureValue(WheelLocations location)
      {
         return (RobotCommunicationBus.Instance.GetLaserWheelTemperatureValue(location));
      }

      public double GetLaserWheelPositionValue(WheelLocations location)
      {
         return (RobotCommunicationBus.Instance.GetLaserWheelPositionValue(location));
      }

      public double GetLaserWheelTotalPositionValue()
      {
         return (RobotCommunicationBus.Instance.GetLaserWheelTotalPositionValue());
      }

      public double GetLaserWheelTripPositionValue()
      {
         return (RobotCommunicationBus.Instance.GetLaserWheelTripPositionValue());
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
            RobotCommunicationBus.Instance.SetLightLevel(systemLocation, level);
         }
         else if ((Controls.SystemLocations.bulletLeft == systemLocation) ||
                  (Controls.SystemLocations.bulletRight == systemLocation) ||
                  (Controls.SystemLocations.bulletDown == systemLocation))
         {
            TruckCommunicationBus.Instance.SetLightLevel(level);
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
            result = RobotCommunicationBus.Instance.GetLightLevel(systemLocation);
         }
         else if ((Controls.SystemLocations.bulletLeft == systemLocation) ||
                  (Controls.SystemLocations.bulletRight == systemLocation) ||
                  (Controls.SystemLocations.bulletDown == systemLocation))
         {
            result = TruckCommunicationBus.Instance.GetLightLevel();
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
            RobotCommunicationBus.Instance.SetLightChannelMask(systemLocation, mask);
         }
         else if ((Controls.SystemLocations.bulletLeft == systemLocation) ||
                  (Controls.SystemLocations.bulletRight == systemLocation) ||
                  (Controls.SystemLocations.bulletDown == systemLocation))
         {
            TruckCommunicationBus.Instance.SetLightChannelMask(mask);
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
            result = RobotCommunicationBus.Instance.GetLightEnable(systemLocation);
         }
         else if ((Controls.SystemLocations.bulletLeft == systemLocation) ||
                  (Controls.SystemLocations.bulletRight == systemLocation) ||
                  (Controls.SystemLocations.bulletDown == systemLocation))
         {
            result = TruckCommunicationBus.Instance.GetLightEnable(systemLocation);
         }

         return (result);
      }

      public void SetCrawlerCamera(Controls.SystemLocations systemLocation)
      {
         RobotCommunicationBus.Instance.SetCrawlerCamera(systemLocation);
      }

      public void SetBulletCamera(Controls.SystemLocations systemLocation)
      {
         TruckCommunicationBus.Instance.SetBulletCamera(systemLocation);
      }

      #endregion

      #endregion
   }
}