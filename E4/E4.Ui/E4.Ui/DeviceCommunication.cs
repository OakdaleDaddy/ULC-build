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

            MainCommunicationBus.Instance.Start();
            TargetCommunicationBus.Instance.Start();

            bool waiting = true;

            for (; waiting; )
            {
               bool busReady = true;

               Thread.Sleep(50);

               busReady = (busReady && MainCommunicationBus.Instance.Ready);
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
            MainCommunicationBus.Instance.Stop();
            TargetCommunicationBus.Instance.Stop();

            bool waiting = true;
            DateTime waitTimeLimit = DateTime.Now.AddSeconds(30);

            for (; waiting; )
            {
               bool busRunning = false;

               Thread.Sleep(50);

               busRunning = (busRunning || MainCommunicationBus.Instance.Running);
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
         MainCommunicationBus.Instance.Service();
         TargetCommunicationBus.Instance.Service();
      }

      public string GetMainFaultStatus()
      {
         string result = MainCommunicationBus.Instance.GetFaultStatus();
         return (result);
      }

      public string GetMainWarningStatus()
      {
         string result = MainCommunicationBus.Instance.GetWarningStatus();
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

         MainCommunicationBus.Instance.StopAll();
         TargetCommunicationBus.Instance.StopAll();
      }

      #endregion

      #region Laser Functions

      public void SetLaserAim(bool on)
      {
         MainCommunicationBus.Instance.SetLaserAim(on);      
      }

      public bool GetLaserAim()
      {
         return (MainCommunicationBus.Instance.GetLaserAim());
      }

      public void StartLaserMeasurement()
      {
         MainCommunicationBus.Instance.StartLaserMeasurement();      
      }

      public void CancelLaserMeasurement()
      {
         MainCommunicationBus.Instance.CancelLaserMeasurement();      
      }

      public bool GetLaserMeasurementActivity()
      {
         return (MainCommunicationBus.Instance.GetLaserMeasurementActivity());
      }

      public int GetLaserSampleRemainingCount()
      {
         return (MainCommunicationBus.Instance.GetLaserSampleRemainingCount());
      }

      public bool GetLaserMeasurementReady()
      {
         return (MainCommunicationBus.Instance.GetLaserMeasurementReady());
      }

      public double GetLaserMeasurement()
      {
         return (MainCommunicationBus.Instance.GetLaserMeasurement());
      }

      #endregion

      #endregion
   }
}