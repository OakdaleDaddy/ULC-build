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
         LaunchCardController,
         LaunchCardCameraLight,
         LaunchCardAnalogIo,
         BulletMotor,
         FeederLeftMotor,
         FeederRightMotor,
         ReelMotor,
         ReelEncoder,
         ReelDigitalIo,
         OsdRs232,
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

      private ElmoWhistleMotor bulletMotor;
      private ElmoWhistleMotor feederLeftMotor;
      private ElmoWhistleMotor feederRightMotor;
      private ElmoWhistleMotor reelMotor;
      private UlcRoboticsRs232 osdRs232;

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

         this.bulletMotor = new ElmoWhistleMotor("bullet motor", (byte)ParameterAccessor.Instance.TruckBus.BulletMotorBusId);
         this.feederLeftMotor = new ElmoWhistleMotor("feeder left motor", (byte)ParameterAccessor.Instance.TruckBus.FeederLeftMotorBusId);
         this.feederRightMotor = new ElmoWhistleMotor("feeder right motor", (byte)ParameterAccessor.Instance.TruckBus.FeederRightMotorBusId);
         this.reelMotor = new ElmoWhistleMotor("reel motor", (byte)ParameterAccessor.Instance.TruckBus.ReelMotorBusId);
         this.osdRs232 = new UlcRoboticsRs232("osd rs232", (byte)ParameterAccessor.Instance.TruckBus.OsdRs232BusId);

         this.deviceList = new ArrayList();
         this.deviceList.Add(this.bulletMotor);
         this.deviceList.Add(this.feederLeftMotor);
         this.deviceList.Add(this.feederRightMotor);
         this.deviceList.Add(this.reelMotor);
         this.deviceList.Add(this.osdRs232);
         
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

      #region OSD RS232 Functions

      private void InitializeOsd232()
      {
         this.osdRs232.Initialize();
      }

      private void StartOsd232()
      {
         this.osdRs232.Start(9600, 8, 0, 1);
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
         
         this.bulletMotor.NodeId = (byte)ParameterAccessor.Instance.TruckBus.BulletMotorBusId;
         this.feederLeftMotor.NodeId = (byte)ParameterAccessor.Instance.TruckBus.FeederLeftMotorBusId;
         this.feederRightMotor.NodeId = (byte)ParameterAccessor.Instance.TruckBus.FeederRightMotorBusId;
         this.reelMotor.NodeId = (byte)ParameterAccessor.Instance.TruckBus.ReelMotorBusId;
         this.osdRs232.NodeId = (byte)ParameterAccessor.Instance.TruckBus.OsdRs232BusId;
         
         this.TraceMask = ParameterAccessor.Instance.TruckBus.ControllerTraceMask;
         this.bulletMotor.TraceMask = ParameterAccessor.Instance.TruckBus.BulletMotorTraceMask;
         this.feederLeftMotor.TraceMask = ParameterAccessor.Instance.TruckBus.FeederLeftMotorTraceMask;
         this.feederRightMotor.TraceMask = ParameterAccessor.Instance.TruckBus.FeederRightMotorTraceMask;
         this.reelMotor.TraceMask = ParameterAccessor.Instance.TruckBus.ReelMotorTraceMask;
         this.osdRs232.TraceMask = ParameterAccessor.Instance.TruckBus.OsdRs232TraceMask;

         this.stopAll = false;
      }

      private void InitializeDevices()
      {
         this.InitializeOsd232();
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

               if (BusComponentId.LaunchCardController == id)
               {
               }
               else if (BusComponentId.LaunchCardCameraLight == id)
               {
                  // restart of component not done
               }
               else if (BusComponentId.LaunchCardAnalogIo == id)
               {
                  // restart of component not done
               }
               else if (BusComponentId.BulletMotor == id)
               {
                  //this.InitializeBulletMotor();
                  this.bulletMotor.Reset();
                  this.WaitDeviceHeartbeat(this.bulletMotor);
                  //this.StartBulletMotor();
               }
               else if (BusComponentId.FeederLeftMotor == id)
               {
                  //this.Initialize...
                  this.feederLeftMotor.Reset();
                  this.WaitDeviceHeartbeat(this.feederLeftMotor);
                  //this.Start...
               }
               else if (BusComponentId.FeederRightMotor == id)
               {
                  //this.Initialize...
                  this.feederRightMotor.Reset();
                  this.WaitDeviceHeartbeat(this.feederRightMotor);
                  //this.Start...
               }
               else if (BusComponentId.ReelMotor == id)
               {
                  //this.Initialize...
                  this.reelMotor.Reset();
                  this.WaitDeviceHeartbeat(this.reelMotor);
                  //this.Start...
               }
               else if (BusComponentId.ReelEncoder == id)
               {
               }
               else if (BusComponentId.ReelDigitalIo == id)
               {
               }
               else if (BusComponentId.OsdRs232 == id)
               {
                  this.InitializeOsd232();
                  this.osdRs232.Reset();
                  this.WaitDeviceHeartbeat(this.osdRs232);
                  this.StartOsd232();
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

               if (BusComponentId.LaunchCardController == id)
               {
               }
               else if (BusComponentId.LaunchCardCameraLight == id)
               {
               }
               else if (BusComponentId.LaunchCardAnalogIo == id)
               {
               }
               else if (BusComponentId.BulletMotor == id)
               {
                  this.bulletMotor.ClearWarning();
               }
               else if (BusComponentId.FeederLeftMotor == id)
               {
                  this.feederLeftMotor.ClearWarning();
               }
               else if (BusComponentId.FeederRightMotor == id)
               {
                  this.feederRightMotor.ClearWarning();
               }
               else if (BusComponentId.ReelMotor == id)
               {
                  this.reelMotor.ClearWarning();
               }
               else if (BusComponentId.ReelEncoder == id)
               {
               }
               else if (BusComponentId.ReelDigitalIo == id)
               {
               }
               else if (BusComponentId.OsdRs232 == id)
               {
                  this.osdRs232.ClearWarning();
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

         this.StartOsd232();

         VideoStampOsd.Instance.Start(new VideoStampOsd.SerialWriteHandler(this.osdRs232.WriteSerial));
         VideoStampOsd.Instance.SetDateAndTime(DateTime.Now);
         VideoStampOsd.Instance.Configure(ParameterAccessor.Instance.Osd);

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
         ParameterAccessor.Instance.TruckBus.BulletMotorTraceMask = this.bulletMotor.TraceMask;
         ParameterAccessor.Instance.TruckBus.FeederLeftMotorTraceMask = this.feederLeftMotor.TraceMask;
         ParameterAccessor.Instance.TruckBus.FeederRightMotorTraceMask = this.feederRightMotor.TraceMask;
         ParameterAccessor.Instance.TruckBus.ReelMotorTraceMask = this.reelMotor.TraceMask;
         ParameterAccessor.Instance.TruckBus.OsdRs232TraceMask = this.osdRs232.TraceMask;
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
            this.deviceThread.Name = "Truck CAN Devices";
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

            VideoStampOsd.Instance.Stop();

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
         this.thread.Name = "Truck Communications";

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
               result = "truck communication offline";
            }
            else if (this.GetFaultStatus(BusComponentId.LaunchCardController) != null)
            {
               result = "launch card offline";
            }
            else if (this.GetFaultStatus(BusComponentId.LaunchCardCameraLight) != null)
            {
               result = "launch card camera/led offline";
            }
            else if (this.GetFaultStatus(BusComponentId.LaunchCardAnalogIo) != null)
            {
               result = "launch card analog IO offline";
            }
            else if (this.GetFaultStatus(BusComponentId.BulletMotor) != null)
            {
               result = "bullet motor offline";
            }
            else if (this.GetFaultStatus(BusComponentId.FeederLeftMotor) != null)
            {
               result = "feeder left motor offline";
            }
            else if (this.GetFaultStatus(BusComponentId.FeederRightMotor) != null)
            {
               result = "feeder right motor offline";
            }
            else if (this.GetFaultStatus(BusComponentId.ReelMotor) != null)
            {
               result = "reel motor offline";
            }
            else if (this.GetFaultStatus(BusComponentId.ReelEncoder) != null)
            {
               result = "reel encoder offline";
            }
            else if (this.GetFaultStatus(BusComponentId.ReelDigitalIo) != null)
            {
               result = "reel digital IO offline";
            }
            else if (this.GetFaultStatus(BusComponentId.OsdRs232) != null)
            {
               result = "OSD RS232 offline";
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
            else if (BusComponentId.LaunchCardController == id)
            {
            }
            else if (BusComponentId.LaunchCardCameraLight == id)
            {
            }
            else if (BusComponentId.LaunchCardAnalogIo == id)
            {
            }
            else if (BusComponentId.BulletMotor == id)
            {
               result = this.bulletMotor.FaultReason;
            }
            else if (BusComponentId.FeederLeftMotor == id)
            {
               result = this.feederLeftMotor.FaultReason;
            }
            else if (BusComponentId.FeederRightMotor == id)
            {
               result = this.feederRightMotor.FaultReason;
            }
            else if (BusComponentId.ReelMotor == id)
            {
               result = this.reelMotor.FaultReason;
            }
            else if (BusComponentId.ReelEncoder == id)
            {
            }
            else if (BusComponentId.ReelDigitalIo == id)
            {
            }
            else if (BusComponentId.OsdRs232 == id)
            {
               result = this.osdRs232.FaultReason;
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
               result = "truck communication offline";
            }
            else if (this.GetWarningStatus(BusComponentId.LaunchCardController) != null)
            {
               result = "launch card offline";
            }
            else if (this.GetWarningStatus(BusComponentId.LaunchCardCameraLight) != null)
            {
               result = "launch card camera/led offline";
            }
            else if (this.GetWarningStatus(BusComponentId.LaunchCardAnalogIo) != null)
            {
               result = "launch card analog IO offline";
            }
            else if (this.GetWarningStatus(BusComponentId.BulletMotor) != null)
            {
               result = "bullet motor offline";
            }
            else if (this.GetWarningStatus(BusComponentId.FeederLeftMotor) != null)
            {
               result = "feeder left motor offline";
            }
            else if (this.GetWarningStatus(BusComponentId.FeederRightMotor) != null)
            {
               result = "feeder right motor offline";
            }
            else if (this.GetWarningStatus(BusComponentId.ReelMotor) != null)
            {
               result = "reel motor offline";
            }
            else if (this.GetWarningStatus(BusComponentId.ReelEncoder) != null)
            {
               result = "reel encoder offline";
            }
            else if (this.GetWarningStatus(BusComponentId.ReelDigitalIo) != null)
            {
               result = "reel digital IO offline";
            }
            else if (this.GetWarningStatus(BusComponentId.OsdRs232) != null)
            {
               result = "OSD RS232 offline";
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
            else if (BusComponentId.LaunchCardController == id)
            {
            }
            else if (BusComponentId.LaunchCardCameraLight == id)
            {
            }
            else if (BusComponentId.LaunchCardAnalogIo == id)
            {
            }
            else if (BusComponentId.BulletMotor == id)
            {
               result = this.bulletMotor.Warning;
            }
            else if (BusComponentId.FeederLeftMotor == id)
            {
               result = this.feederLeftMotor.Warning;
            }
            else if (BusComponentId.FeederRightMotor == id)
            {
               result = this.feederRightMotor.Warning;
            }
            else if (BusComponentId.ReelMotor == id)
            {
               result = this.reelMotor.Warning;
            }
            else if (BusComponentId.ReelEncoder == id)
            {
            }
            else if (BusComponentId.ReelDigitalIo == id)
            {
            }
            else if (BusComponentId.OsdRs232 == id)
            {
               result = this.osdRs232.Warning;
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
            if (BusComponentId.LaunchCardController == id)
            {
            }
            else if (BusComponentId.LaunchCardCameraLight == id)
            {
            }
            else if (BusComponentId.LaunchCardAnalogIo == id)
            {
            }
            else if (BusComponentId.BulletMotor == id)
            {
               result = this.bulletMotor;
            }
            else if (BusComponentId.FeederLeftMotor == id)
            {
               result = this.feederLeftMotor;
            }
            else if (BusComponentId.FeederRightMotor == id)
            {
               result = this.feederRightMotor;
            }
            else if (BusComponentId.ReelMotor == id)
            {
               result = this.reelMotor;
            }
            else if (BusComponentId.ReelEncoder == id)
            {
            }
            else if (BusComponentId.ReelDigitalIo == id)
            {
            }
            else if (BusComponentId.OsdRs232 == id)
            {
               result = this.osdRs232;
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