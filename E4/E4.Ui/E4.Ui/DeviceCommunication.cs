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

            //MainCommunicationBus.Instance.Start();
            //TargeCommunicationBus.Instance.Start();

            bool waiting = true;

            for (; waiting; )
            {
               bool busReady = true;

               Thread.Sleep(50);

               //busReady = (busReady && MainCommunicationBus.Instance.Ready);
               //busReady = (busReady && TargeCommunicationBus.Instance.Ready);

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
            //MainCommunicationBus.Instance.Stop();
            //TargeCommunicationBus.Instance.Stop();

            bool waiting = true;
            DateTime waitTimeLimit = DateTime.Now.AddSeconds(30);

            for (; waiting; )
            {
               bool busRunning = false;

               Thread.Sleep(50);

               //busRunning = (busRunning || MainCommunicationBus.Instance.Running);
               //busRunning = (busRunning || TargeCommunicationBus.Instance.Running);

               waiting = (waiting && busRunning);

               if (DateTime.Now > waitTimeLimit)
               {
                  Tracer.WriteError(TraceGroup.COMM, null, "stop time limit exceeded");
                  waiting = false;
               }
            }
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
         //MainCommunicationBus.Instance.Service();
         //TargetCommunication.Instance.Service();
      }

      public string GetMainFaultStatus()
      {
         string result = null; // MainCommunicationBus.Instance.GetFaultStatus();
         return (result);
      }

      public string GetMainWarningStatus()
      {
         string result = null; // MainCommunicationBus.Instance.GetWarningStatus();
         return (result);
      }

      public string GetTargetFaultStatus()
      {
         string result = null; // TargetCommunication.Instance.GetFaultStatus();
         return (result);
      }

      public string GetTargetWarningStatus()
      {
         string result = null; // TargetCommunication.Instance.GetWarningStatus();
         return (result);
      }

      #endregion

      #endregion
   }
}