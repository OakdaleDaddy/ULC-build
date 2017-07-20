namespace Weco.Ui
{
   using System;
   using System.Collections;
   using System.Text;
   using System.Threading;

   using Weco.CAN;
   using Weco.PCANLight;
   using Weco.Utilities;

   public class TruckCommunicationBus
   {
      #region Definitions

      private const int BootTimeoutPeriod = 1500;

      public enum BusComponentId
      {
         Bus,
         TargetBoard,
         TargetBoardCameraLed,
         TargetBoardFrontWheel,
         TargetBoardRearWheel,
      }

      #endregion

      #region Fields

      private static TruckCommunicationBus instance = null;

      private bool execute;
      private Thread thread;
      private bool deviceExecute;
      private Thread deviceThread;
      private int deviceReceiveCount;
      private bool ready;
      private bool running;

      private BusInterfaces busInterfaceId;
      private bool busReady;
      private string busStatus;
      private Queue busReceiveQueue;
      private Queue deviceResetQueue;
      private Queue deviceClearErrorQueue;

      private ArrayList deviceList;

      private DateTime controllerHeartbeatLimit;
      private bool controllerServiced;
      private bool stopAll;
      private object valueUpdate;

      private WheelMotorStatus wheel0Status;
      private WheelMotorStatus wheel1Status;

      private int launchCardLightIntensitySetPoint;
      //private int camaraLightIntensityRequested;

      private int launchCardLightChannelMaskSetPoint;
      //private int cameraLightChannelMaskRequested;

      private int cameraSetPoint;
      //private int cameraRequested;

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
         this.deviceClearErrorQueue = new Queue();

         this.deviceList = new ArrayList();

         foreach (Device device in this.deviceList)
         {
            device.OnReceiveTrace = new Device.ReceiveTraceHandler(this.DeviceTraceReceive);
            device.OnTransmitTrace = new Device.TransmitTraceHandler(this.DeviceTraceTransmit);
            device.OnTransmit = new Device.TransmitHandler(this.DeviceTransmit);
            device.OnFault = new Device.FaultHandler(this.DeviceFault);
            device.OnWarning = new Device.WarningHandler(this.DeviceWarning);
         }

         this.controllerHeartbeatLimit = DateTime.Now.AddSeconds(30);
         this.controllerServiced = false;
         this.stopAll = false;
         this.valueUpdate = new object();

         //this.targetTripStartValue = 0;

         this.wheel0Status = new WheelMotorStatus();
         this.wheel1Status = new WheelMotorStatus();
      }

      private void SendControllerHeartBeat()
      {
         int cobId = (int)(((int)COBTypes.ERROR << 7) | (ParameterAccessor.Instance.TruckBus.ControllerBusId & 0x7F));
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

      public static TruckCommunicationBus Instance
      {
         get
         {
            if (instance == null)
            {
               instance = new TruckCommunicationBus();
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

         this.deviceReceiveCount++;
         Tracer.WriteMedium(TraceGroup.TBUS, "", "rx {0:X3} {1}", cobId, sb.ToString());
      }

      private void DeviceTraceTransmit(int cobId, byte[] data)
      {
         StringBuilder sb = new StringBuilder();

         for (int i = 0; i < data.Length; i++)
         {
            sb.AppendFormat("{0:X2}", data[i]);
         }

         Tracer.WriteMedium(TraceGroup.TBUS, "", "tx {0:X3} {1}", cobId, sb.ToString());
      }

      private bool DeviceTransmit(int id, byte[] data)
      {
         CANResult transmitResult = PCANLight.Send(this.busInterfaceId, id, data);
         bool result = (transmitResult == CANResult.ERR_OK) ? true : false;

         return (result);
      }

      private void DeviceFault(string name, int nodeId, string reason)
      {
         Tracer.WriteError(TraceGroup.TBUS, "", "fault with \"{0}\", node={1}, reason={2}", name, nodeId, reason);
      }

      private void DeviceWarning(string name, int nodeId, string reason)
      {
         Tracer.WriteError(TraceGroup.TBUS, "", "warning with \"{0}\", node={1}, reason={2}", name, nodeId, reason);
      }

      #endregion

      #region Device Process Loop

      private void UpdateControllerHeartbeat()
      {
         if ((0 != ParameterAccessor.Instance.TruckBus.ProducerHeartbeatRate) &&
             (false != this.controllerServiced) &&
             (DateTime.Now > this.controllerHeartbeatLimit))
         {
            this.SendControllerHeartBeat();
            this.controllerHeartbeatLimit = this.controllerHeartbeatLimit.AddMilliseconds(ParameterAccessor.Instance.TruckBus.ProducerHeartbeatRate);
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
               if (false != this.stopAll)
               { 
                  // stop all within device update
               }

               device.Update();
            }

            Thread.Sleep(1);
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
         this.deviceClearErrorQueue.Clear();


         this.TraceMask = ParameterAccessor.Instance.TruckBus.ControllerTraceMask;

         this.stopAll = false;
      }

      private void InitializeDevices()
      {
      }

      private void StartBus()
      {
         this.busReady = false;

         if (false == this.busReady)
         {
            this.busInterfaceId = ParameterAccessor.Instance.TruckBus.BusInterface;
            CANResult startResult = PCANLight.Start(this.busInterfaceId, ParameterAccessor.Instance.TruckBus.BitRate, false, 0, FramesType.INIT_TYPE_ST, TraceGroup.TBUS, this.BusReceiveHandler);
            this.busReady = (CANResult.ERR_OK == startResult);
         }

         if (false != this.busReady)
         {
            DateTime now = DateTime.Now;
            DateTime flushTimeLimit = now.AddMilliseconds(500);
            DateTime idleReceiveTimeLimit = now.AddMilliseconds(100);
            int deviceReceiveCheck = this.deviceReceiveCount;

            Tracer.WriteMedium(TraceGroup.TBUS, "", "bus flush start");

            for (; this.execute; )
            {
               now = DateTime.Now;

               if (deviceReceiveCheck != this.deviceReceiveCount)
               {
                  deviceReceiveCheck = this.deviceReceiveCount;
                  idleReceiveTimeLimit = DateTime.Now.AddMilliseconds(100);
               }
               else if (now > idleReceiveTimeLimit)
               {
                  break;
               }

               if (now > flushTimeLimit)
               {
                  break;
               }

               Thread.Sleep(1);
            }

            Tracer.WriteMedium(TraceGroup.TBUS, "", "bus flush done");
         }

         this.InitializeDevices();
         
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
            Tracer.WriteMedium(TraceGroup.TBUS, "", "bus failure");

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

               if (BusComponentId.TargetBoard == id)
               {
               }
               else if (BusComponentId.TargetBoardCameraLed == id)
               {
                  // restart of component not done
               }
               else if (BusComponentId.TargetBoardFrontWheel == id)
               {
                  // restart of component not done
               }
               else if (BusComponentId.TargetBoardRearWheel == id)
               {
                  // restart of component not done
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

      private void UpdateDeviceClearError()
      {
         int receiveCount = 0;
         DeviceClearErrorRequest request = null;

         do
         {
            lock (this)
            {
               request = null;
               receiveCount = this.deviceClearErrorQueue.Count;

               if (receiveCount > 0)
               {
                  request = (DeviceClearErrorRequest)this.deviceClearErrorQueue.Dequeue();
               }
            }

            if (null != request)
            {
               BusComponentId id = (BusComponentId)request.Id;

               if (BusComponentId.TargetBoard == id)
               {
               }
               else if (BusComponentId.TargetBoardCameraLed == id)
               {
                  //bool wasFaulted = false;
               }
               else if (BusComponentId.TargetBoardFrontWheel == id)
               {
                  bool wasFaulted = false;

                  if (false != wasFaulted)
                  {
                     this.wheel0Status.state = WheelMotorStatus.States.off;
                  }
               }
               else if (BusComponentId.TargetBoardRearWheel == id)
               {
                  bool wasFaulted = false;

                  if (false != wasFaulted)
                  {
                     this.wheel1Status.state = WheelMotorStatus.States.off;
                  }
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
         this.controllerHeartbeatLimit = DateTime.Now.AddMilliseconds(ParameterAccessor.Instance.TruckBus.ProducerHeartbeatRate);

         // start devices

         this.ready = true;

         for (; this.execute; )
         {
            lock (this.valueUpdate)
            {
               // update devices
            }

            this.UpdateDeviceReset();
            this.UpdateDeviceClearError();

            Thread.Sleep(1);
         }
      }

      private void CloseBus()
      {
         this.busReady = false;

         PCANLight.ResetBus(this.busInterfaceId);
         Thread.Sleep(200);
         PCANLight.Stop(this.busInterfaceId);

         ParameterAccessor.Instance.TruckBus.ControllerTraceMask = this.TraceMask;
      }

      private void Process()
      {
         try
         {
            this.running = true;
            Tracer.WriteHigh(TraceGroup.TBUS, null, "start");

            this.InitializeValues();

            this.deviceExecute = true;
            this.deviceThread = new Thread(this.DeviceProcess);
            this.deviceThread.IsBackground = true;
            this.deviceThread.Name = "Target CAN Devices";
            this.deviceThread.Start();

            this.StartBus();

            this.ExecuteProcessLoop();
         }
         catch (Exception preException)
         {
            Tracer.WriteError(TraceGroup.TBUS, null, "process exception {0}", preException.Message);
         }

         try
         {
            this.ready = false;

            this.deviceExecute = false;
            this.deviceThread.Join(3000);
            this.deviceThread = null;

            this.CloseBus();

            this.InitializeValues(); // clears previous session requests for next session
            this.InitializeDevices();
         }
         catch (Exception postException)
         {
            Tracer.WriteError(TraceGroup.TBUS, null, "post process exception {0}", postException.Message);
         }

         Tracer.WriteHigh(TraceGroup.TBUS, null, "stop");
         this.running = false;
      }

      #endregion

      #region Constructor

      private TruckCommunicationBus()
      {
      }

      #endregion

      #region Access Functions

      #region Control Functions

      public void Start()
      {
         this.thread = new Thread(this.Process);
         this.thread.IsBackground = true;
         this.thread.Name = "Target Communications";

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
               result = "target communication offline";
            }
            else if (this.GetFaultStatus(BusComponentId.TargetBoard) != null)
            {
               result = "target board offline";
            }
            else if (this.GetFaultStatus(BusComponentId.TargetBoardCameraLed) != null)
            {
               result = "target board camera/led offline";
            }
            else if (this.GetFaultStatus(BusComponentId.TargetBoardFrontWheel) != null)
            {
               result = "target board front wheel offline";
            }
            else if (this.GetFaultStatus(BusComponentId.TargetBoardRearWheel) != null)
            {
               result = "target board rear wheel offline";
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
            else if (BusComponentId.TargetBoard == id)
            {
            }
            else if (BusComponentId.TargetBoardCameraLed == id)
            {
            }
            else if (BusComponentId.TargetBoardFrontWheel == id)
            {
            }
            else if (BusComponentId.TargetBoardRearWheel == id)
            {
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
               result = "target communication offline";
            }
            else if (this.GetWarningStatus(BusComponentId.TargetBoard) != null)
            {
               result = "target board offline";
            }
            else if (this.GetWarningStatus(BusComponentId.TargetBoardCameraLed) != null)
            {
               result = "target board camera/led error";
            }
            else if (this.GetWarningStatus(BusComponentId.TargetBoardFrontWheel) != null)
            {
               result = "target board front wheel error";
            }
            else if (this.GetWarningStatus(BusComponentId.TargetBoardRearWheel) != null)
            {
               result = "target board rear wheel error";
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
            else if (BusComponentId.TargetBoard == id)
            {
            }
            else if (BusComponentId.TargetBoardCameraLed == id)
            {
            }
            else if (BusComponentId.TargetBoardFrontWheel == id)
            {
            }
            else if (BusComponentId.TargetBoardRearWheel == id)
            {
            }
         }
         else
         {
            result = "off";
         }

         return (result);
      }

      public object GetDevice(Enum deviceId)
      {
         object result = null;
         BusComponentId id = (BusComponentId)deviceId;

         if (false != this.Running)
         {
            if (BusComponentId.TargetBoard == id)
            {
            }
            else if (BusComponentId.TargetBoardCameraLed == id)
            {
            }
            else if (BusComponentId.TargetBoardFrontWheel == id)
            {
            }
            else if (BusComponentId.TargetBoardRearWheel == id)
            {
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

      public void ClearDeviceError(Enum deviceId, UInt32 code, DeviceClearErrorRequest.CompleteHandler onComplete)
      {
         lock (this)
         {
            DeviceClearErrorRequest request = new DeviceClearErrorRequest(deviceId, code, onComplete);
            this.deviceClearErrorQueue.Enqueue(request);
         }
      }

      public void StopAll()
      {
         Tracer.WriteHigh(TraceGroup.TBUS, "", "Stop All");
         this.stopAll = true;
      }

      #endregion

      #region Lights and Camera

      public void SetLightLevel(int level)
      {
         lock (this.valueUpdate)
         {
            this.launchCardLightIntensitySetPoint = level;
         }
      }

      public int GetLightLevel()
      {
         int result = this.launchCardLightIntensitySetPoint;
         return (result);
      }

      public void SetLightChannelMask(int lightChannelMask)
      {
         this.launchCardLightChannelMaskSetPoint = lightChannelMask;
      }

      public bool GetLightEnable(Controls.SystemLocations systemLocation)
      {
         int mask = 0;

         if (Controls.SystemLocations.bulletLeft == systemLocation)
         {
            mask = (int)(1 << 0);
         }
         else if (Controls.SystemLocations.bulletRight == systemLocation)
         {
            mask = (int)(1 << 1);
         }
         else if (Controls.SystemLocations.bulletDown == systemLocation)
         {
            mask = (int)(1 << 2);
         }

         bool result = ((this.launchCardLightChannelMaskSetPoint & mask) != 0) ? true : false;

         return (result);
      }

      public void SetBulletCamera(Controls.SystemLocations systemLocation)
      {
         lock (this.valueUpdate)
         {
            this.cameraSetPoint = ParameterAccessor.Instance.BulletCameraMaps.GetIndex(systemLocation);
         }
      }

      #endregion

      #endregion
   }
}