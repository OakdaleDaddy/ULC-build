namespace E4.Ui
{
   using System;
   using System.Collections;
   using System.Text;
   using System.Threading;

   using E4.CAN;
   using E4.PCANLight;
   using E4.Utilities;

   public class MainCommunicationBus
   {
      #region Definitions

      private const int BootTimeoutPeriod = 1500;

      public enum BusComponentId
      {
         Bus,
         MainBoard,
      }

      #endregion

      #region Fields

      private static MainCommunicationBus instance = null;

      private bool execute;
      private Thread thread;
      private bool deviceExecute;
      private Thread deviceThread;
      private bool ready;
      private bool running;

      private BusInterfaces busInterfaceId;
      private bool busReady;
      private string busStatus;
      private Queue busReceiveQueue;
      private Queue deviceResetQueue;
      private Queue deviceClearWarningQueue;

      private DateTime controllerHeartbeatLimit;
      private bool controllerServiced;
      private bool stopAll;

      private ArrayList deviceList;

      private UlcRoboticsE4Main mainBoard;

      private bool laserAimSetPoint;
      private bool laserAimRequested;
      private bool needLaserMeasurementStart;
      private bool needLaserMeasurementCancel;
      private bool laserMeasureStartRequested;
      private bool laserMeasureCancelRequested;
      private bool laserBecameActive;
      private int laserSampleCount;
      private double laserMeasurement;

      #endregion

      #region Helper Functions

      private void Initialize()
      {
         this.execute = false;
         this.thread = null;
         this.deviceExecute = false;
         this.deviceThread = null;
         this.ready = false;
         this.running = false;

         this.busInterfaceId = BusInterfaces.USBA;
         this.busReady = false;
         this.busStatus = null;

         this.busReceiveQueue = new Queue();
         this.deviceResetQueue = new Queue();
         this.deviceClearWarningQueue = new Queue();

         this.controllerHeartbeatLimit = DateTime.Now.AddSeconds(30);
         this.controllerServiced = false;
         this.stopAll = false;

         this.mainBoard = new UlcRoboticsE4Main("main board", (byte)ParameterAccessor.Instance.MainBus.MainBoardBusId);

         this.deviceList = new ArrayList();
         this.deviceList.Add(this.mainBoard);
         
         foreach (Device device in this.deviceList)
         {
            device.OnReceiveTrace = new Device.ReceiveTraceHandler(this.DeviceTraceReceive);
            device.OnTransmitTrace = new Device.TransmitTraceHandler(this.DeviceTraceTransmit);
            device.OnTransmit = new Device.TransmitHandler(this.DeviceTransmit);
            device.OnFault = new Device.FaultHandler(this.DeviceFault);
            device.OnWarning = new Device.WarningHandler(this.DeviceWarning);
         }
      }

      private void SendControllerHeartBeat()
      {
         int cobId = (int)(((int)COBTypes.ERROR << 7) | (ParameterAccessor.Instance.MainBus.ControllerBusId & 0x7F));
         byte[] heartbeatMsg = new byte[1];

         heartbeatMsg[0] = 5;

         this.DeviceTransmit(cobId, heartbeatMsg);

         if (false != this.TraceHB)
         {
            this.DeviceTraceTransmit(cobId, heartbeatMsg);
         }
      }

      #endregion

      #region Properties

      public static MainCommunicationBus Instance
      {
         get
         {
            if (instance == null)
            {
               instance = new MainCommunicationBus();
               instance.Initialize();
            }

            return instance;
         }
      }

      public bool Ready { get { return (this.ready); } }
      public bool Running { get { return (this.running); } }

      public bool TraceHB { set; get; }

      public int TraceMask
      {
         set
         {
            this.TraceHB = ((value & 0x0002) != 0) ? true : false;
         }

         get
         {
            int result = 0;

            result |= (false != this.TraceHB) ? 0x0002 : 0;

            return (result);
         }
      }

      #endregion

      #region Delegates

      private void BusReceiveHandler(CanFrame frame)
      {
         lock (this)
         {
            this.busReceiveQueue.Enqueue(frame);
         }
      }

      private void DeviceTraceReceive(int cobId, byte[] data)
      {
         StringBuilder sb = new StringBuilder();

         for (int i = 0; i < data.Length; i++)
         {
            sb.AppendFormat("{0:X2}", data[i]);
         }

         Tracer.WriteMedium(TraceGroup.MBUS, "", "rx {0:X3} {1}", cobId, sb.ToString());
      }

      private void DeviceTraceTransmit(int cobId, byte[] data)
      {
         StringBuilder sb = new StringBuilder();

         for (int i = 0; i < data.Length; i++)
         {
            sb.AppendFormat("{0:X2}", data[i]);
         }

         Tracer.WriteMedium(TraceGroup.MBUS, "", "tx {0:X3} {1}", cobId, sb.ToString());
      }

      private bool DeviceTransmit(int id, byte[] data)
      {
         CANResult transmitResult = PCANLight.Send(this.busInterfaceId, id, data);
         bool result = (transmitResult == CANResult.ERR_OK) ? true : false;

         return (result);
      }

      private void DeviceFault(string name, int nodeId, string reason)
      {
         Tracer.WriteError(TraceGroup.MBUS, "", "fault with \"{0}\", node={1}, reason={2}", name, nodeId, reason);
      }

      private void DeviceWarning(string name, int nodeId, string reason)
      {
         Tracer.WriteError(TraceGroup.MBUS, "", "warning with \"{0}\", node={1}, reason={2}", name, nodeId, reason);
      }

      #endregion

      #region Device Process Loop

      private void UpdateControllerHeartbeat()
      {
         if ((0 != ParameterAccessor.Instance.MainBus.ProducerHeartbeatRate) &&
             (false != this.controllerServiced) &&
             (DateTime.Now > this.controllerHeartbeatLimit))
         {
            this.SendControllerHeartBeat();
            this.controllerHeartbeatLimit = this.controllerHeartbeatLimit.AddMilliseconds(ParameterAccessor.Instance.MainBus.ProducerHeartbeatRate);
         }
      }

      private void ProcessCommFrames()
      {
         int receiveCount = 0;
         CanFrame frame = null;

         do
         {
            lock (this)
            {
               receiveCount = this.busReceiveQueue.Count;

               if (receiveCount > 0)
               {
                  frame = (CanFrame)this.busReceiveQueue.Dequeue();
               }
            }

            if (null != frame)
            {
               bool traced = false;

               foreach (Device device in this.deviceList)
               {
                  device.Update((int)frame.cobId, frame.data, ref traced);
               }

               if (false == traced)
               {
                  this.DeviceTraceReceive((int)frame.cobId, frame.data);
               }

               frame = null;
            }
         }
         while (0 != receiveCount);
      }

      private void DeviceProcess()
      {
         foreach (Device device in this.deviceList)
         {
            device.Initialize();
         }

         for (; this.deviceExecute; )
         {
            this.UpdateControllerHeartbeat();
            this.ProcessCommFrames();

            foreach (Device device in this.deviceList)
            {
               device.Update();
            }

            Thread.Sleep(1);
         }
      }

      #endregion

      #region Main Board Functions

      private void InitializeMainBoard()
      {
         this.mainBoard.Initialize();

         this.laserAimSetPoint = false;
         this.laserAimRequested = false;
         this.needLaserMeasurementStart = false;
         this.needLaserMeasurementCancel = false;
         this.laserMeasureStartRequested = false;
         this.laserMeasureCancelRequested = false;
         this.laserBecameActive = false;
         this.laserSampleCount = 0;
         this.laserMeasurement = 0;
      }

      private void StartMainBoard()
      {
         this.mainBoard.SetConsumerHeartbeat((UInt16)ParameterAccessor.Instance.MainBus.ConsumerHeartbeatRate, (byte)ParameterAccessor.Instance.MainBus.ControllerBusId);
         this.mainBoard.SetProducerHeartbeat((UInt16)ParameterAccessor.Instance.MainBus.ProducerHeartbeatRate);

         this.mainBoard.Configure(UlcRoboticsE4Main.UsageModes.laser);
         this.mainBoard.Start();

         Thread.Sleep(50);
      }

      private void UpdateMainBoard()
      {
         if ((null == this.mainBoard.FaultReason) &&
             (null == this.mainBoard.Warning))
         {
            if (false != this.stopAll)
            {
               // stop motors...
            }
            else
            {
               //this.UpdateBldc(this.mainBoard.Bldc0, this.bldc0Status, this.bldc0Parameters);
               //this.UpdateBldc(this.mainBoard.Bldc1, this.bldc1Status, this.bldc1Parameters);
               //this.UpdateStepper(this.mainBoard.Stepper0, this.stepper0Status, this.stepper0Parameters);
               //this.UpdateStepper(this.mainBoard.Stepper1, this.stepper1Status, this.stepper1Parameters);
            }

            #region Laser Control 

            if (this.laserAimSetPoint != this.laserAimRequested)
            {
               if (false != this.laserAimSetPoint)
               {
                  this.mainBoard.SetLaserAimOn();
               }
               else
               {
                  this.mainBoard.SetLaserAimOff();
               }

               this.laserAimRequested = this.laserAimSetPoint;
            }

            if ((false != this.needLaserMeasurementStart) &&
                (false == this.laserMeasureStartRequested))
            {
               this.laserSampleCount = (int)ParameterAccessor.Instance.LaserSampleCount.OperationalValue;
               this.mainBoard.StartLaserMeasurement(this.laserSampleCount, ParameterAccessor.Instance.LaserSampleTime.OperationalValue);
               this.laserMeasureStartRequested = true;

               this.laserBecameActive = false;
               this.needLaserMeasurementCancel = false;
               this.laserMeasureCancelRequested = false;
            }

            if ((false != this.needLaserMeasurementCancel) &&
                (false == this.laserMeasureCancelRequested))
            {
               this.mainBoard.CancelLaserMeasurement();
               this.laserMeasureCancelRequested = true;

               this.needLaserMeasurementStart = false;
               this.laserMeasureStartRequested = false;
            }

            bool laserMeasurementActive = this.mainBoard.LaserMeasurementActivity;

            if (false != laserMeasurementActive)
            {
               this.laserBecameActive = true;
            }

            if ((false != this.laserMeasureStartRequested) &&
                (false != this.laserBecameActive) &&
                (false == laserMeasurementActive))
            {
               UInt32 laserMeasurementCounts = 0;
               this.mainBoard.GetLaserDistance(ref laserMeasurementCounts);
               this.laserMeasurement = laserMeasurementCounts * ParameterAccessor.Instance.LaserMeasurementConstant.OperationalValue;

               this.needLaserMeasurementStart = false;
               this.laserMeasureStartRequested = false;
            }

            #endregion

         }
      }

      #endregion

      #region Process Support Functions

      private void WaitDeviceHeartbeat(Device device)
      {
         DateTime limit = DateTime.Now.AddMilliseconds(BootTimeoutPeriod);

         for (; ; )
         {
            if (false != device.ReceiveBootupHeartbeat)
            {
               Thread.Sleep(250);
               break;
            }
            else if (DateTime.Now > limit)
            {
               device.SetFault("boot timeout", false);
               break;
            }

            Thread.Sleep(50);
         }
      }

      private void InitializeValues()
      {
         this.busReady = false;
         this.busStatus = null;
         this.busReceiveQueue.Clear();
         this.deviceResetQueue.Clear();
         this.deviceClearWarningQueue.Clear();

         this.mainBoard.NodeId = (byte)ParameterAccessor.Instance.MainBus.MainBoardBusId;

         this.TraceMask = ParameterAccessor.Instance.MainBus.ControllerTraceMask;
         this.mainBoard.TraceMask = ParameterAccessor.Instance.MainBus.MainBoardTraceMask;

         this.stopAll = false;

         this.InitializeMainBoard();
      }

      private void StartBus()
      {
         this.busReady = false;

         if (false == this.busReady)
         {
            this.busInterfaceId = ParameterAccessor.Instance.MainBus.BusInterface;
            CANResult startResult = PCANLight.Start(this.busInterfaceId, ParameterAccessor.Instance.MainBus.BitRate, FramesType.INIT_TYPE_ST, TraceGroup.MBUS, this.BusReceiveHandler);
            this.busReady = (CANResult.ERR_OK == startResult);
         }

         if (false != this.busReady)
         {
            PCANLight.ResetBus(this.busInterfaceId);

            DateTime busStartLimit = DateTime.Now.AddMilliseconds(BootTimeoutPeriod);

            for (; this.execute; )
            {
               bool allBooted = true;

               foreach (Device device in this.deviceList)
               {
                  allBooted = allBooted && device.ReceiveBootupHeartbeat;

                  if (false == allBooted)
                  {
                     break;
                  }
               }

               if ((false != allBooted) || (DateTime.Now > busStartLimit))
               {
                  break;
               }

               Thread.Sleep(1);
            }

            foreach (Device device in this.deviceList)
            {
               if (false == device.ReceiveBootupHeartbeat)
               {
                  device.SetFault("boot timeout", false);
               }
            }
         }
         else
         {
            this.busStatus = "interface failure";
            Tracer.WriteMedium(TraceGroup.MBUS, "", "bus failure");

            foreach (Device device in this.deviceList)
            {
               if (false == device.ReceiveBootupHeartbeat)
               {
                  device.SetFault("interface not ready", false);
               }
            }
         }
      }

      private void UpdateDeviceReset()
      {
         int receiveCount = 0;
         DeviceRestartRequest request = null;

         do
         {
            lock (this)
            {
               request = null;
               receiveCount = this.deviceResetQueue.Count;

               if (receiveCount > 0)
               {
                  request = (DeviceRestartRequest)this.deviceResetQueue.Dequeue();
               }
            }

            if (null != request)
            {
               BusComponentId id = (BusComponentId)request.Id;

               if (BusComponentId.MainBoard == id)
               {
                  this.InitializeMainBoard();
                  this.mainBoard.Initialize();
                  this.mainBoard.Reset();
                  this.WaitDeviceHeartbeat(this.mainBoard);
                  this.StartMainBoard();
               }

               if (null != request.OnComplete)
               {
                  try
                  {
                     request.OnComplete(id);
                  }
                  catch { }
               }

               request = null;
            }
         }
         while (0 != receiveCount);
      }

      private void UpdateDeviceClearWarning()
      {
         int receiveCount = 0;
         DeviceClearWarningRequest request = null;

         do
         {
            lock (this)
            {
               request = null;
               receiveCount = this.deviceClearWarningQueue.Count;

               if (receiveCount > 0)
               {
                  request = (DeviceClearWarningRequest)this.deviceClearWarningQueue.Dequeue();
               }
            }

            if (null != request)
            {
               BusComponentId id = (BusComponentId)request.Id;

               if (BusComponentId.MainBoard == id)
               {
                  this.mainBoard.ClearWarning();
               }

               if (null != request.OnComplete)
               {
                  try
                  {
                     request.OnComplete(id);
                  }
                  catch { }
               }

               request = null;
            }
         }
         while (0 != receiveCount);
      }

      private void ExecuteProcessLoop()
      {
         this.controllerServiced = true;
         this.controllerHeartbeatLimit = DateTime.Now.AddMilliseconds(ParameterAccessor.Instance.MainBus.ProducerHeartbeatRate);

         this.StartMainBoard();

         this.ready = true;

         for (; this.execute; )
         {
            this.UpdateMainBoard();
            this.UpdateDeviceReset();
            this.UpdateDeviceClearWarning();

            Thread.Sleep(1);
         }
      }

      private void CloseBus()
      {
         this.busReady = false;

         PCANLight.ResetBus(this.busInterfaceId);
         Thread.Sleep(200);
         PCANLight.Stop(this.busInterfaceId);

         ParameterAccessor.Instance.MainBus.ControllerTraceMask = this.TraceMask;
         ParameterAccessor.Instance.MainBus.MainBoardTraceMask = this.mainBoard.TraceMask;
      }

      private void Process()
      {
         try
         {
            this.running = true;
            Tracer.WriteHigh(TraceGroup.MBUS, null, "start");

            this.InitializeValues();

            this.deviceExecute = true;
            this.deviceThread = new Thread(this.DeviceProcess);
            this.deviceThread.IsBackground = true;
            this.deviceThread.Name = "Main CAN Devices";
            this.deviceThread.Start();

            this.StartBus();

            this.ExecuteProcessLoop();
         }
         catch (Exception preException)
         {
            Tracer.WriteError(TraceGroup.MBUS, null, "process exception {0}", preException.Message);
         }

         try
         {
            this.ready = false;

            this.deviceExecute = false;
            this.deviceThread.Join(3000);
            this.deviceThread = null;

            this.CloseBus();

            this.InitializeValues(); // clears previous session requests for next session
         }
         catch (Exception postException)
         {
            Tracer.WriteError(TraceGroup.MBUS, null, "post process exception {0}", postException.Message);
         }

         Tracer.WriteHigh(TraceGroup.MBUS, null, "stop");
         this.running = false;
      }

      #endregion

      #region Constructor

      private MainCommunicationBus()
      {
      }

      #endregion

      #region Access Functions

      #region Control Functions

      public void Start()
      {
         this.thread = new Thread(this.Process);
         this.thread.IsBackground = true;
         this.thread.Name = "Main Communications";

         this.ready = false;
         this.execute = true;
         this.thread.Start();
      }

      public void Stop()
      {
         if (null != this.thread)
         {
            this.execute = false;
            this.thread.Join(3000);

            this.thread = null;
         }
      }

      public void Service()
      {
         this.controllerServiced = true;
      }

      public string GetStatus(Enum generalId, ref bool warning)
      {
         BusComponentId id = (BusComponentId)generalId;
         string result = this.GetFaultStatus(id);

         if (null == result)
         {
            result = this.GetWarningStatus(id);
            warning = true;
         }
         else
         {
            warning = false;
         }

         return (result);
      }

      /// <summary>
      /// Get cumulative status of every on the bus. 
      /// </summary>
      /// <remarks>Each id supported is expected to be evaluated.</remarks>
      /// <returns>null when ok, string with description when faulted</returns>
      public string GetFaultStatus()
      {
         string result = null;

         if (false != this.Running)
         {
            if (this.GetFaultStatus(BusComponentId.Bus) != null)
            {
               result = "main communication offline";
            }
            else if (this.GetFaultStatus(BusComponentId.MainBoard) != null)
            {
               result = "main board offline";
            }
         }

         return (result);
      }

      /// <summary>
      /// Get status of a particular component.
      /// </summary>
      /// <param name="id">component to query</param>
      /// <returns>null when ok, string with description when faulted</returns>
      public string GetFaultStatus(BusComponentId id)
      {
         string result = null;

         if (false != this.Running)
         {
            if (BusComponentId.Bus == id)
            {
               result = this.busStatus;
            }
            else if (BusComponentId.MainBoard == id)
            {
               result = this.mainBoard.FaultReason;
            }
         }
         else
         {
            result = "off";
         }

         return (result);
      }

      /// <summary>
      /// Get cumulative status of every on the bus. 
      /// </summary>
      /// <remarks>Each id supported is expected to be evaluated.</remarks>
      /// <returns>null when ok, string with description when faulted</returns>
      public string GetWarningStatus()
      {
         string result = null;

         if (false != this.Running)
         {
            if (this.GetWarningStatus(BusComponentId.Bus) != null)
            {
               result = "main communication offline";
            }
            else if (this.GetWarningStatus(BusComponentId.MainBoard) != null)
            {
               result = "main board offline";
            }
         }

         return (result);
      }

      /// <summary>
      /// Get status of a particular component.
      /// </summary>
      /// <param name="id">component to query</param>
      /// <returns>null when ok, string with description when faulted</returns>
      public string GetWarningStatus(BusComponentId id)
      {
         string result = null;

         if (false != this.Running)
         {
            if (BusComponentId.Bus == id)
            {
            }
            else if (BusComponentId.MainBoard == id)
            {
               result = this.mainBoard.Warning;
            }
         }
         else
         {
            result = "off";
         }

         return (result);
      }

      public Device GetDevice(Enum deviceId)
      {
         Device result = null;
         BusComponentId id = (BusComponentId)deviceId;

         if (false != this.Running)
         {
            if (BusComponentId.MainBoard == id)
            {
               result = this.mainBoard;
            }
         }

         return (result);
      }

      public void RestartDevice(Enum deviceId, DeviceRestartRequest.CompleteHandler onComplete)
      {
         lock (this)
         {
            DeviceRestartRequest request = new DeviceRestartRequest(deviceId, onComplete);
            this.deviceResetQueue.Enqueue(request);
         }
      }

      public void ClearDeviceWarning(Enum deviceId, DeviceClearWarningRequest.CompleteHandler onComplete)
      {
         lock (this)
         {
            DeviceClearWarningRequest request = new DeviceClearWarningRequest(deviceId, onComplete);
            this.deviceClearWarningQueue.Enqueue(request);
         }
      }

      public void StopAll()
      {
         Tracer.WriteHigh(TraceGroup.MBUS, "", "Stop All");
         this.stopAll = true;
      }

      #endregion

      #region Laser Functions

      public void SetLaserAim(bool on)
      {
         this.laserAimSetPoint = on;
      }

      public bool GetLaserAim()
      {
         return (this.laserAimRequested);
      }

      public void StartLaserMeasurement()
      {
         this.needLaserMeasurementStart = true;
      }

      public void CancelLaserMeasurement()
      {
         this.needLaserMeasurementCancel = true;
      }

      public bool GetLaserMeasurementActivity()
      {
         bool result = false;

         if (false != this.running)
         {
            result = this.mainBoard.LaserMeasurementActivity;
         }

         return (result);
      }

      public int GetLaserSampleRemainingCount()
      {
         int result = 0;

         if (false != this.running)
         {
            result = this.laserSampleCount - this.mainBoard.LaserReadingCount;
         }

         return (result);
      }

      public bool GetLaserMeasurementReady()
      {
         bool result = false;

         if (false != this.running)
         {
            result = this.mainBoard.LaserMeasurementReady;
         }

         return (result);
      }

      public double GetLaserMeasurement()
      {
         return (this.laserMeasurement);
      }

      #endregion

      #endregion
   }
}