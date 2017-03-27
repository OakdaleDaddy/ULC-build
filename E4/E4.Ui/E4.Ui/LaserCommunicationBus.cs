namespace E4.Ui
{
   using System;
   using System.Collections;
   using System.Text;
   using System.Threading;

   using E4.CAN;
   using E4.PCANLight;
   using E4.Utilities;

   public class LaserCommunicationBus
   {
      #region Definitions

      private const int BootTimeoutPeriod = 1500;

      public enum BusComponentId
      {
         Bus,
         LaserBoard,
      }

      #endregion

      #region Fields

      private static LaserCommunicationBus instance = null;

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

      private UlcRoboticsE4Main laserBoard;

      //private StepperMotorStatus bldc0Status;
      //private StepperMotorStatus bldc1Status;
      private StepperMotorStatus stepper0Status;
      private StepperMotorStatus stepper1Status;

      private bool laserAimSetPoint;
      private bool laserAimRequested;
      private bool needLaserMeasurementStart;
      private bool needLaserMeasurementCancel;
      private bool laserMeasureStartRequested;
      private bool laserMeasureCancelRequested;
      private bool laserBecameActive;
      private int laserSampleCount;
      private double laserAverageMeasurement;

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

         this.laserBoard = new UlcRoboticsE4Main("laser board", (byte)ParameterAccessor.Instance.LaserBus.LaserBoardBusId);

         this.deviceList = new ArrayList();
         this.deviceList.Add(this.laserBoard);
         
         foreach (Device device in this.deviceList)
         {
            device.OnReceiveTrace = new Device.ReceiveTraceHandler(this.DeviceTraceReceive);
            device.OnTransmitTrace = new Device.TransmitTraceHandler(this.DeviceTraceTransmit);
            device.OnTransmit = new Device.TransmitHandler(this.DeviceTransmit);
            device.OnFault = new Device.FaultHandler(this.DeviceFault);
            device.OnWarning = new Device.WarningHandler(this.DeviceWarning);
         }

         //this.bldc0Status = new StepperMotorStatus();
         //this.bldc1Status = new StepperMotorStatus();
         this.stepper0Status = new StepperMotorStatus();
         this.stepper1Status = new StepperMotorStatus();
      }

      private void SendControllerHeartBeat()
      {
         int cobId = (int)(((int)COBTypes.ERROR << 7) | (ParameterAccessor.Instance.LaserBus.ControllerBusId & 0x7F));
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

      public static LaserCommunicationBus Instance
      {
         get
         {
            if (instance == null)
            {
               instance = new LaserCommunicationBus();
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
         if ((0 != ParameterAccessor.Instance.LaserBus.ProducerHeartbeatRate) &&
             (false != this.controllerServiced) &&
             (DateTime.Now > this.controllerHeartbeatLimit))
         {
            this.SendControllerHeartBeat();
            this.controllerHeartbeatLimit = this.controllerHeartbeatLimit.AddMilliseconds(ParameterAccessor.Instance.LaserBus.ProducerHeartbeatRate);
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

      #region Laser Board Functions

      private void InitializeLaserBoard()
      {
         this.laserBoard.Initialize();

         this.laserAimSetPoint = false;
         this.laserAimRequested = false;
         this.needLaserMeasurementStart = false;
         this.needLaserMeasurementCancel = false;
         this.laserMeasureStartRequested = false;
         this.laserMeasureCancelRequested = false;
         this.laserBecameActive = false;
         this.laserSampleCount = 0;
         this.laserAverageMeasurement = 0;

         //this.bldc0Status.Initialize();
         //this.bldc1Status.Initialize();
         this.stepper0Status.Initialize();
         this.stepper1Status.Initialize();
      }

      private void StartLaserBoard()
      {
         this.laserBoard.SetConsumerHeartbeat((UInt16)ParameterAccessor.Instance.LaserBus.ConsumerHeartbeatRate, (byte)ParameterAccessor.Instance.LaserBus.ControllerBusId);
         this.laserBoard.SetProducerHeartbeat((UInt16)ParameterAccessor.Instance.LaserBus.ProducerHeartbeatRate);

         this.laserBoard.Configure(UlcRoboticsE4Main.UsageModes.laser);
         this.laserBoard.Start();

         Thread.Sleep(50);

         this.stepper0Status.homeNeeded = (false == laserBoard.Stepper0.HomingAttained) ? true : false;
         this.stepper1Status.homeNeeded = (false == laserBoard.Stepper1.HomingAttained) ? true : false;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="motor"></param>
      /// <param name="status"></param>
      /// <param name="parameters"></param>
      /// <remarks>
      /// request stop if stop all is requested
      /// request home if home is not defined
      /// request center after home is defined
      /// </remarks>
      private void UpdateStepper(MotorComponent motor, StepperMotorStatus status, StepperMotorParameters parameters)
      {
         if (false != this.stopAll)
         {
            if (MotorComponent.Modes.homing == motor.Mode)
            {
               motor.StopHoming();
            }

            if (MotorComponent.Modes.off != motor.Mode)
            {
               motor.SetMode(MotorComponent.Modes.off);
            }
         }
         else if (false != status.homeNeeded)
         {
            status.homeNeeded = false;
            status.centerNeeded = true;

            motor.SetHomeOffset(parameters.HomeOffset);
            motor.SetHomingSwitchSpeed((UInt32)parameters.HomingSwitchVelocity);
            motor.SetHomingZeroSpeed((UInt32)parameters.HomingZeroVelocity);
            motor.SetHomingAcceleration((UInt32)parameters.HomingAcceleration);
            motor.SetMode(MotorComponent.Modes.homing);
            motor.StartHoming();
            Tracer.WriteHigh(TraceGroup.MBUS, "", "{0} homing", motor.Name);

            status.positionRequested = 0;
         }
         else if ((false != motor.HomingAttained) && (MotorComponent.Modes.position != motor.Mode))
         {
            motor.SetMode(MotorComponent.Modes.position);
            motor.SetProfileVelocity(parameters.ProfileVelocity);
            motor.SetProfileAcceleration(parameters.ProfileAcceleration);
            Tracer.WriteHigh(TraceGroup.MBUS, "", "{0} position mode", motor.Name);
         }
         else if (false != status.centerNeeded)
         {
            status.positionNeeded = parameters.CenterPosition;

            if (status.positionRequested != status.positionNeeded)
            {
               motor.SetTargetPosition(status.positionNeeded, false);
               status.positionRequested = status.positionNeeded;
               status.actualNeeded = true;
               Tracer.WriteHigh(TraceGroup.MBUS, "", "{0} position requested {1}", motor.Name, status.positionRequested);
            }
            else if ((false != motor.PositionAttained) && (false != status.actualNeeded))
            {
               status.centerNeeded = false;
               motor.GetActualPosition(ref status.actualPosition);
               status.actualNeeded = false;
            }         
         }
         else
         {
            if (status.positionRequested != status.positionNeeded)
            {
               motor.SetTargetPosition(status.positionNeeded, false);
               status.positionRequested = status.positionNeeded;
               status.actualNeeded = true;
               Tracer.WriteHigh(TraceGroup.MBUS, "", "{0} position requested {1}", motor.Name, status.positionRequested);
            }
            else if ((false != motor.PositionAttained) && (false != status.actualNeeded))
            {
               motor.GetActualPosition(ref status.actualPosition);
               status.actualNeeded = false;
            }
         }

         if (DateTime.Now > status.readTimeLimit)
         {
            motor.GetActualPosition(ref status.actualPosition);
            status.readTimeLimit = DateTime.Now.AddMilliseconds(250);
         }
      }

      private void UpdateLaserBoard()
      {
         if ((null == this.laserBoard.FaultReason) &&
             (null == this.laserBoard.Warning))
         {
            #region Motor Control

            //this.UpdateBldc(this.laserBoard.Bldc0, this.bldc0Status, this.bldc0Parameters);
            //this.UpdateBldc(this.laserBoard.Bldc1, this.bldc1Status, this.bldc1Parameters);
            this.UpdateStepper(this.laserBoard.Stepper0, this.stepper0Status, ParameterAccessor.Instance.LaserXStepper);
            this.UpdateStepper(this.laserBoard.Stepper1, this.stepper1Status, ParameterAccessor.Instance.LaserYStepper);

            #endregion

            #region Laser Control

            if (this.laserAimSetPoint != this.laserAimRequested)
            {
               if (false != this.laserAimSetPoint)
               {
                  this.laserBoard.SetLaserAimOn();
               }
               else
               {
                  this.laserBoard.SetLaserAimOff();
               }

               this.laserAimRequested = this.laserAimSetPoint;
            }

            if ((false != this.needLaserMeasurementStart) &&
                (false == this.laserMeasureStartRequested))
            {
               this.laserSampleCount = (int)ParameterAccessor.Instance.LaserSampleCount.OperationalValue;
               this.laserBoard.StartLaserMeasurement(this.laserSampleCount, ParameterAccessor.Instance.LaserSampleTime.OperationalValue);
               this.laserMeasureStartRequested = true;

               this.laserBecameActive = false;
               this.needLaserMeasurementCancel = false;
               this.laserMeasureCancelRequested = false;
            }

            if ((false != this.needLaserMeasurementCancel) &&
                (false == this.laserMeasureCancelRequested))
            {
               this.laserBoard.CancelLaserMeasurement();
               this.laserMeasureCancelRequested = true;

               this.needLaserMeasurementStart = false;
               this.laserMeasureStartRequested = false;
            }

            bool laserMeasurementActive = this.laserBoard.LaserMeasurementActivity;

            if (false != laserMeasurementActive)
            {
               this.laserBecameActive = true;
            }

            if ((false != this.laserMeasureStartRequested) &&
                (false != this.laserBecameActive) &&
                (false == laserMeasurementActive))
            {
               UInt32 laserMeasurementCounts = this.laserBoard.GetAverageLaserDistance();
               this.laserAverageMeasurement = laserMeasurementCounts * ParameterAccessor.Instance.LaserMeasurementConstant.OperationalValue;

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

         this.laserBoard.NodeId = (byte)ParameterAccessor.Instance.LaserBus.LaserBoardBusId;

         this.TraceMask = ParameterAccessor.Instance.LaserBus.ControllerTraceMask;
         this.laserBoard.TraceMask = ParameterAccessor.Instance.LaserBus.MainBoardTraceMask;

         this.stopAll = false;

         this.InitializeLaserBoard();
      }

      private void StartBus()
      {
         this.busReady = false;

         if (false == this.busReady)
         {
            this.busInterfaceId = ParameterAccessor.Instance.LaserBus.BusInterface;
            CANResult startResult = PCANLight.Start(this.busInterfaceId, ParameterAccessor.Instance.LaserBus.BitRate, FramesType.INIT_TYPE_ST, TraceGroup.MBUS, this.BusReceiveHandler);
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

               if (BusComponentId.LaserBoard == id)
               {
                  this.InitializeLaserBoard();
                  this.laserBoard.Initialize();
                  this.laserBoard.Reset();
                  this.WaitDeviceHeartbeat(this.laserBoard);
                  this.StartLaserBoard();
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

               if (BusComponentId.LaserBoard == id)
               {
                  this.laserBoard.ClearWarning();
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
         this.controllerHeartbeatLimit = DateTime.Now.AddMilliseconds(ParameterAccessor.Instance.LaserBus.ProducerHeartbeatRate);

         this.StartLaserBoard();

         this.ready = true;

         for (; this.execute; )
         {
            this.UpdateLaserBoard();
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

         ParameterAccessor.Instance.LaserBus.ControllerTraceMask = this.TraceMask;
         ParameterAccessor.Instance.LaserBus.MainBoardTraceMask = this.laserBoard.TraceMask;
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

      private LaserCommunicationBus()
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
               result = "laser communication offline";
            }
            else if (this.GetFaultStatus(BusComponentId.LaserBoard) != null)
            {
               result = "laser board offline";
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
            else if (BusComponentId.LaserBoard == id)
            {
               result = this.laserBoard.FaultReason;
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
               result = "laser communication offline";
            }
            else if (this.GetWarningStatus(BusComponentId.LaserBoard) != null)
            {
               result = "laser board offline";
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
            else if (BusComponentId.LaserBoard == id)
            {
               result = this.laserBoard.Warning;
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
            if (BusComponentId.LaserBoard == id)
            {
               result = this.laserBoard;
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
            result = this.laserBoard.LaserMeasurementActivity;
         }

         return (result);
      }

      public int GetLaserSampleRemainingCount()
      {
         int result = 0;

         if (false != this.running)
         {
            result = this.laserSampleCount - this.laserBoard.LaserSampleNumber;
         }

         return (result);
      }

      public bool GetLaserMeasurementReady()
      {
         bool result = false;

         if (false != this.running)
         {
            result = this.laserBoard.LaserMeasurementReady;
         }

         return (result);
      }

      public double GetAverageLaserMeasurement()
      {
         return (this.laserAverageMeasurement);
      }

      #endregion

      #region Laser Stepper Functions

      public void SetLaserStepperXPosition(int position)
      {
         this.stepper0Status.positionNeeded = position;
      }

      public void SetLaserStepperYPosition(int position)
      {
         this.stepper1Status.positionNeeded = position;
      }

      public int GetLaserStepperXActualPosition()
      {
         int result = this.stepper0Status.actualPosition;
         return (result);
      }

      public int GetLaserStepperYActualPosition()
      {
         int result = this.stepper1Status.actualPosition;
         return (result);      
      }

      public bool LaserXPositionObtained()
      {
         bool result = false;

         if ((this.stepper0Status.positionRequested == this.stepper0Status.positionNeeded) &&
             (false != this.laserBoard.Stepper0.PositionAttained))
         {
            result = true;
         }

         return (result);
      }

      public bool LaserYPositionObtained()
      {
         bool result = false;

         if ((this.stepper1Status.positionRequested == this.stepper1Status.positionNeeded) &&
             (false != this.laserBoard.Stepper1.PositionAttained))
         {
            result = true;
         }

         return (result);
      }

      #endregion

      #endregion
   }
}