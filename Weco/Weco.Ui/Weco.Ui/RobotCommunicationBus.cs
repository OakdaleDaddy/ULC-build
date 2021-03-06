﻿namespace Weco.Ui
{
   using System;
   using System.Collections;
   using System.Text;
   using System.Threading;

   using Weco.CAN;
   using Weco.PCANLight;
   using Weco.Utilities;

   public class RobotCommunicationBus
   {
      #region Definitions

      private const int BootTimeoutPeriod = 1500;

      public enum BusComponentId
      {
         Bus,
         LeftTrackController,
         LeftTrackLight,
         LeftTrackMotor,
         RightTrackController,
         RightTrackLight,
         RightTrackMotor,
         HubController,
         HubCameraLights,
         HubPanMotor,
         HubTiltMotor,
      }

      #endregion

      #region Fields

      private static RobotCommunicationBus instance = null;

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

      private UlcRoboticsWecoTrackController leftTrackController;
      private UlcRoboticsWecoTrackController rightTrackController;
      private UlcRoboticsWecoHub hubController;

      private ArrayList deviceList;

      private DateTime controllerHeartbeatLimit;
      private bool controllerServiced;
      private bool stopAll;
      private object valueUpdate;

      private bool laserLocked;
      private bool laserMoved;
      private bool laserManualMovementMode;
      private MovementModes laserMovementMode;
      private double laserMovementRequest;
      private bool laserMovementTriggered;
      //private int laserTripStartValue;
      
      private WheelMotorStatus wheel0Status;
      private WheelMotorStatus wheel1Status;


      private int crawlerLeftTrackLightIntensitySetPoint;
      private int crawlerRightTrackLightIntensitySetPoint;

      private int crawlerHubLightIntensitySetPoint;
      //private int camaraLightIntensityRequested;

      private int crawlerLeftTrackLightChannelMaskSetPoint;
      private int crawlerRightTrackLightChannelMaskSetPoint;

      private int crawlerHubLightChannelMaskSetPoint;
      //private int cameraLightChannelMaskRequested;

      private int hubCameraSelectSetPoint;
      private int hubCameraSelectRequested;

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

         this.leftTrackController = new UlcRoboticsWecoTrackController("left track", (byte)ParameterAccessor.Instance.RobotBus.LeftTrackBusId);
         this.rightTrackController = new UlcRoboticsWecoTrackController("right track", (byte)ParameterAccessor.Instance.RobotBus.RightTrackBusId);
         this.hubController = new UlcRoboticsWecoHub("hub", (byte)ParameterAccessor.Instance.RobotBus.HubBusId);

         this.deviceList = new ArrayList();
         this.deviceList.Add(this.leftTrackController);
         this.deviceList.Add(this.rightTrackController);
         this.deviceList.Add(this.hubController);

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

         this.laserLocked = false;
         this.laserMoved = false;
         this.laserManualMovementMode = false;
         this.laserMovementMode = MovementModes.off;
         this.laserMovementRequest = 0;
         this.laserMovementTriggered = false;
         //this.laserTripStartValue = 0;

         this.wheel0Status = new WheelMotorStatus();
         this.wheel1Status = new WheelMotorStatus();
      }

      private void SendControllerHeartBeat()
      {
         int cobId = (int)(((int)COBTypes.ERROR << 7) | (ParameterAccessor.Instance.RobotBus.ControllerBusId & 0x7F));
         byte[] heartbeatMsg = new byte[1];

         heartbeatMsg[0] = 5;

         this.DeviceTransmit(cobId, heartbeatMsg);

         if (false != this.TraceHB)
         {
            this.DeviceTraceTransmit(cobId, heartbeatMsg);
         }
      }

#if false
      private int GetLightChannelMask(Controls.SystemLocations camera)
      {
         int result = 0;

         if (Controls.SystemLocations.crawlerFront == camera)
         {
            result = (int)(1 << 0);
         }
         else if (Controls.SystemLocations.crawlerRear == camera)
         {
            result = (int)(1 << 1);
         }

         return (result);
      }
#endif

#if false
      private Controls.SystemLocations GetCamera(int selection)
      {
         Controls.SystemLocations result = Controls.SystemLocations.crawlerFront;

         if (0 == selection)
         {
            result = Controls.SystemLocations.crawlerFront;
         }
         else if (1 == selection)
         {
            result = Controls.SystemLocations.crawlerRear;
         }

         return (result);
      }
#endif

      #endregion

      #region Properties

      public static RobotCommunicationBus Instance
      {
         get
         {
            if (instance == null)
            {
               instance = new RobotCommunicationBus();
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
         Tracer.WriteMedium(TraceGroup.RBUS, "", "rx {0:X3} {1}", cobId, sb.ToString());
      }

      private void DeviceTraceTransmit(int cobId, byte[] data)
      {
         StringBuilder sb = new StringBuilder();

         for (int i = 0; i < data.Length; i++)
         {
            sb.AppendFormat("{0:X2}", data[i]);
         }

         Tracer.WriteMedium(TraceGroup.RBUS, "", "tx {0:X3} {1}", cobId, sb.ToString());
      }

      private bool DeviceTransmit(int id, byte[] data)
      {
         CANResult transmitResult = PCANLight.Send(this.busInterfaceId, id, data);
         bool result = (transmitResult == CANResult.ERR_OK) ? true : false;

         return (result);
      }

      private void DeviceFault(string name, int nodeId, string reason)
      {
         Tracer.WriteError(TraceGroup.RBUS, "", "fault with \"{0}\", node={1}, reason={2}", name, nodeId, reason);
      }

      private void DeviceWarning(string name, int nodeId, string reason)
      {
         Tracer.WriteError(TraceGroup.RBUS, "", "warning with \"{0}\", node={1}, reason={2}", name, nodeId, reason);
      }

      #endregion

      #region Device Process Loop

      private void UpdateControllerHeartbeat()
      {
         if ((0 != ParameterAccessor.Instance.RobotBus.ProducerHeartbeatRate) &&
             (false != this.controllerServiced) &&
             (DateTime.Now > this.controllerHeartbeatLimit))
         {
            this.SendControllerHeartBeat();
            this.controllerHeartbeatLimit = this.controllerHeartbeatLimit.AddMilliseconds(ParameterAccessor.Instance.RobotBus.ProducerHeartbeatRate);
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

      #region Track Functions

      private void InitializeTrack(UlcRoboticsWecoTrackController controller)
      {
         controller.Initialize();

         // initialize controls
      }

      private void StartTrack(UlcRoboticsWecoTrackController controller)
      {
      }

      private void StartTracks()
      {
         this.StartTrack(this.leftTrackController);
         this.StartTrack(this.rightTrackController);
      }

      private void UpdateTracks()
      {
         // update left track LED
         // update right track LED

         // update left track motor
         // update right track motor

         // generate SYNC
      }

      #endregion

      #region Hub Functions

      private void InitializeHub()
      {
         this.hubController.Initialize();

         this.wheel0Status.Initialize();
         this.wheel1Status.Initialize();

         this.crawlerLeftTrackLightIntensitySetPoint = 0;
         this.crawlerRightTrackLightIntensitySetPoint = 0;
         this.crawlerHubLightIntensitySetPoint = 0;
         //this.camaraLightIntensityRequested = 0;

         this.crawlerLeftTrackLightChannelMaskSetPoint = 0;
         this.crawlerRightTrackLightChannelMaskSetPoint = 0;

         this.crawlerHubLightChannelMaskSetPoint = 0;
         //this.cameraLightChannelMaskRequested = 0;

         this.hubCameraSelectSetPoint = 0;
         this.hubCameraSelectRequested = 0;
         //this.cameraRequested = 0;
      }

      private void StartHub()
      {
      }

      private void EvaluateHubMotor(MotorComponent motor, WheelMotorParameters parameters, WheelMotorStatus status, ref double total, ref int count)
      {
      }

      private bool UpdateHubMotor(MotorComponent motor, WheelMotorStatus status, WheelMotorParameters parameters)
      {
         bool scheduled = false;

         if (null == motor.FaultReason)
         {
            DateTime now = DateTime.Now;
            bool positionObtained = false;
            bool velocityObtained = false;

            if (now > status.statusInvalidTimeLimit)
            {
               positionObtained = motor.PositionAttained;
               velocityObtained = motor.VelocityAttained;
            }

            if (false != this.stopAll)
            {
               if (MotorComponent.Modes.off != motor.Mode)
               {
                  motor.SetMode(MotorComponent.Modes.off);
               }

               status.state = WheelMotorStatus.States.stopped;
            }
            else if (status.state == WheelMotorStatus.States.stopped)
            {
               // nothing, reset needed to clear
            }
            else if (status.state == WheelMotorStatus.States.undefined)
            {
               motor.SetVelocityKp(parameters.Kp);
               motor.SetVelocityKi(parameters.Ki);
               motor.SetVelocityKd(parameters.Kd);
               motor.SetProfileVelocity(parameters.ProfileVelocity);
               motor.SetProfileAcceleration(parameters.ProfileAcceleration);
               motor.SetProfileDeceleration(parameters.ProfileDeceleration);
               status.state = WheelMotorStatus.States.off;
               Tracer.WriteHigh(TraceGroup.RBUS, "", "{0} set", motor.Name);
            }

            else if (status.state == WheelMotorStatus.States.turnOff)
            {
               motor.SetMode(MotorComponent.Modes.off);
               status.state = WheelMotorStatus.States.off;
               Tracer.WriteHigh(TraceGroup.RBUS, "", "{0} off", motor.Name);
            }
            else if (status.state == WheelMotorStatus.States.off)
            {
               if (WheelMotorStates.locked == parameters.MotorState)
               {
                  status.state = WheelMotorStatus.States.startPosition;
               }
               else if (WheelMotorStates.enabled == parameters.MotorState)
               {
                  if (false != this.laserMovementTriggered)
                  {
                     status.state = WheelMotorStatus.States.startVelocity;
                  }
                  else
                  {
                     status.state = WheelMotorStatus.States.startPosition;
                  }
               }
            }

            else if (status.state == WheelMotorStatus.States.startPosition)
            {
               motor.SetMode(MotorComponent.Modes.position);

               Tracer.WriteHigh(TraceGroup.RBUS, "", "{0} position mode", motor.Name);

               status.state = WheelMotorStatus.States.positioning;
            }
            else if (status.state == WheelMotorStatus.States.positioning)
            {
               if (false != status.stopNeeded)
               {
                  status.stopNeeded = false;

                  motor.Halt();
                  status.positionRequested = status.positionNeeded;
                  Tracer.WriteHigh(TraceGroup.RBUS, "", "{0} position stop", motor.Name);

                  positionObtained = false;
                  status.statusInvalidTimeLimit = now.AddMilliseconds(250);
                  status.state = WheelMotorStatus.States.stopPosition;
               }
               else if ((WheelMotorStates.enabled == parameters.MotorState) &&
                        (false != this.laserMovementTriggered))
               {
                  status.stopNeeded = true;
               }
               else if (WheelMotorStates.disabled == parameters.MotorState)
               {
                  status.stopNeeded = true;
               }
               else
               {
                  int neededPosition = status.positionNeeded;

                  if (WheelMotorStates.locked == parameters.MotorState)
                  {
                     neededPosition = motor.ActualPosition;
                  }

                  if (status.positionRequested != status.positionNeeded)
                  {
                     motor.ScheduleTargetPosition(status.positionNeeded);
                     scheduled = true;
                     status.positionRequested = status.positionNeeded;

                     Tracer.WriteHigh(TraceGroup.RBUS, "", "{0} position {1}", motor.Name, status.positionRequested);

                     positionObtained = false;
                     status.statusInvalidTimeLimit = now.AddMilliseconds(250);
                  }
               }
            }
            else if (status.state == WheelMotorStatus.States.stopPosition)
            {
               if (false != positionObtained)
               {
                  status.positionNeeded = motor.ActualPosition;
                  motor.ScheduleTargetPosition(status.positionNeeded);
                  scheduled = true;
                  status.positionRequested = status.positionNeeded;

                  motor.Run();

                  Tracer.WriteHigh(TraceGroup.RBUS, "", "{0} position stopped at {1}", motor.Name, motor.ActualPosition);

                  if ((WheelMotorStates.enabled == parameters.MotorState) &&
                      (false != this.laserMovementTriggered))
                  {
                     status.state = WheelMotorStatus.States.startVelocity;
                  }
                  else if (WheelMotorStates.disabled == parameters.MotorState)
                  {
                     status.state = WheelMotorStatus.States.turnOff;
                  }
                  else
                  {
                     status.state = WheelMotorStatus.States.positioning;
                  }
               }
            }

            else if (status.state == WheelMotorStatus.States.startVelocity)
            {
               if (ActuationModes.openloop == parameters.ActuationMode)
               {
                  motor.SetMode(MotorComponent.Modes.openLoop);
               }
               else
               {
                  motor.SetMode(MotorComponent.Modes.velocity);
               }

               Tracer.WriteHigh(TraceGroup.RBUS, "", "{0} velocity {1} mode", motor.Name, parameters.ActuationMode.ToString());

               status.velocityActuationMode = parameters.ActuationMode;
               status.state = WheelMotorStatus.States.velocity;
            }
            else if (status.state == WheelMotorStatus.States.velocity)
            {
               if ((WheelMotorStates.disabled == parameters.MotorState) ||
                   (WheelMotorStates.locked == parameters.MotorState) ||
                   (false == this.laserMovementTriggered) ||
                   (status.velocityActuationMode != parameters.ActuationMode))
               {
                  motor.ScheduleTargetVelocity(0);
                  scheduled = true;
                  status.velocityRequested = 0;
                  Tracer.WriteHigh(TraceGroup.RBUS, "", "{0} velocity stop", motor.Name);

                  velocityObtained = false;
                  status.statusInvalidTimeLimit = now.AddMilliseconds(250);
                  status.state = WheelMotorStatus.States.stopVelocity;
               }
               else if (this.laserMovementRequest != status.velocityRequested)
               {
                  ValueParameter movementParameter = ParameterAccessor.Instance.LaserWheelMaximumSpeed;

                  double movementRequestValue = this.laserMovementRequest * movementParameter.OperationalValue;
                  int positionInversionValue = (false == parameters.PositionInverted) ? 1 : -1;
                  int requestInversionValue = (false == parameters.RequestInverted) ? 1 : -1;
                  int velocityRpm = (int)(positionInversionValue * requestInversionValue * movementRequestValue * ParameterAccessor.Instance.LaserWheelVelocityToRpm);
                  motor.ScheduleTargetVelocity(velocityRpm);
                  scheduled = true;
                  status.velocityRequested = this.laserMovementRequest;

                  Tracer.WriteMedium(TraceGroup.RBUS, null, "{0} velocity={1:0.00} rpm={2}", motor.Name, movementRequestValue, velocityRpm);
               }
            }
            else if (status.state == WheelMotorStatus.States.stopVelocity)
            {
               if ((WheelMotorStates.enabled == parameters.MotorState) &&
                   (false != this.laserMovementTriggered))
               {
                  status.state = WheelMotorStatus.States.velocity;
               }
               else if (false != velocityObtained)
               {
                  status.positionNeeded = motor.ActualPosition;
                  motor.ScheduleTargetPosition(status.positionNeeded);
                  scheduled = true;
                  status.positionRequested = status.positionNeeded;

                  Tracer.WriteHigh(TraceGroup.RBUS, "", "{0} velocity stopped at {1}", motor.Name, motor.ActualPosition);

                  if (WheelMotorStates.disabled == parameters.MotorState)
                  {
                     status.state = WheelMotorStatus.States.turnOff;
                  }
                  else
                  {
                     status.state = WheelMotorStatus.States.startPosition;
                  }
               }
            }
         }

         return (scheduled);
      }

      private void UpdateHub()
      {
         if (this.hubCameraSelectRequested != this.hubCameraSelectSetPoint)
         {
            this.hubCameraSelectRequested = hubCameraSelectSetPoint;
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


         this.TraceMask = ParameterAccessor.Instance.RobotBus.ControllerTraceMask;

         this.stopAll = false;

         this.laserLocked = false;
         this.laserMoved = false;
         this.laserManualMovementMode = false;
         this.laserMovementMode = MovementModes.off;
         this.laserMovementRequest = 0;
         this.laserMovementTriggered = false;
         //this.laserTripStartValue = 0;
      }

      private void InitializeDevices()
      {
         this.InitializeTrack(this.leftTrackController);
         this.InitializeTrack(this.rightTrackController);
         this.InitializeHub();
      }

      private void StartBus()
      {
         this.busReady = false;

         if (false == this.busReady)
         {
            this.busInterfaceId = ParameterAccessor.Instance.RobotBus.BusInterface;
            CANResult startResult = PCANLight.Start(this.busInterfaceId, ParameterAccessor.Instance.RobotBus.BitRate, false, 0, FramesType.INIT_TYPE_ST, TraceGroup.RBUS, this.BusReceiveHandler);
            this.busReady = (CANResult.ERR_OK == startResult);
         }

         if (false != this.busReady)
         {
            DateTime now = DateTime.Now;
            DateTime flushTimeLimit = now.AddMilliseconds(500);
            DateTime idleReceiveTimeLimit = now.AddMilliseconds(100);
            int deviceReceiveCheck = this.deviceReceiveCount;

            Tracer.WriteMedium(TraceGroup.RBUS, "", "bus flush start");

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

            Tracer.WriteMedium(TraceGroup.RBUS, "", "bus flush done");
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
            Tracer.WriteMedium(TraceGroup.RBUS, "", "bus failure");

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

               if (BusComponentId.LeftTrackController == id)
               {
                  this.InitializeTrack(this.leftTrackController);
                  this.leftTrackController.Reset();
                  this.WaitDeviceHeartbeat(this.leftTrackController);
                  this.StartTrack(this.leftTrackController);
               }
               else if (BusComponentId.LeftTrackLight == id)
               {
                  // restart of component not done
               }
               else if (BusComponentId.LeftTrackMotor == id)
               {
                  // restart of component not done
               }
               else if (BusComponentId.RightTrackController == id)
               {
                  this.InitializeTrack(this.rightTrackController);
                  this.rightTrackController.Reset();
                  this.WaitDeviceHeartbeat(this.rightTrackController);
                  this.StartTrack(this.rightTrackController);
               }
               else if (BusComponentId.RightTrackLight == id)
               {
                  // restart of component not done
               }
               else if (BusComponentId.RightTrackMotor == id)
               {
                  // restart of component not done
               }
               else if (BusComponentId.HubController == id)
               {
                  this.InitializeHub();
                  this.hubController.Reset();
                  this.WaitDeviceHeartbeat(this.hubController);
                  this.StartHub();
               }
               else if (BusComponentId.HubCameraLights == id)
               {
                  // restart of component not done
               }
               else if (BusComponentId.HubPanMotor == id)
               {
                  // restart of component not done
               }
               else if (BusComponentId.HubTiltMotor == id)
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

               if (BusComponentId.LeftTrackController == id)
               {
                  this.leftTrackController.ClearWarning();
               }
               else if (BusComponentId.LeftTrackLight == id)
               {
                  bool wasFaulted = false;
                  this.leftTrackController.Light.ClearError(request.Code, ref wasFaulted);
               }
               else if (BusComponentId.LeftTrackMotor == id)
               {
                  bool wasFaulted = false;
                  this.leftTrackController.TrackMotor.ClearError(request.Code, ref wasFaulted);

                  if (false != wasFaulted)
                  {
                  }
               }
               else if (BusComponentId.RightTrackController == id)
               {
                  this.rightTrackController.ClearWarning();
               }
               else if (BusComponentId.RightTrackLight == id)
               {
                  bool wasFaulted = false;
                  this.rightTrackController.Light.ClearError(request.Code, ref wasFaulted);
               }
               else if (BusComponentId.RightTrackMotor == id)
               {
                  bool wasFaulted = false;
                  this.rightTrackController.TrackMotor.ClearError(request.Code, ref wasFaulted);

                  if (false != wasFaulted)
                  {
                  }
               }
               else if (BusComponentId.HubController == id)
               {
                  this.hubController.ClearWarning();
               }
               else if (BusComponentId.HubCameraLights == id)
               {
                  bool wasFaulted = false;
                  this.hubController.CameraLights.ClearError(request.Code, ref wasFaulted);
               }
               else if (BusComponentId.HubPanMotor == id)
               {
                  bool wasFaulted = false;
                  this.hubController.PanMotor.ClearError(request.Code, ref wasFaulted);

                  if (false != wasFaulted)
                  {
                     this.wheel0Status.state = WheelMotorStatus.States.off;
                  }
               }
               else if (BusComponentId.HubTiltMotor == id)
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
         this.controllerHeartbeatLimit = DateTime.Now.AddMilliseconds(ParameterAccessor.Instance.RobotBus.ProducerHeartbeatRate);

         this.StartTracks();
         this.StartHub();

         this.ready = true;

         for (; this.execute; )
         {
            lock (this.valueUpdate)
            {
               this.UpdateTracks();
               this.UpdateHub();
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

         ParameterAccessor.Instance.RobotBus.ControllerTraceMask = this.TraceMask;
      }

      private void Process()
      {
         try
         {
            this.running = true;
            Tracer.WriteHigh(TraceGroup.RBUS, null, "start");

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
            Tracer.WriteError(TraceGroup.RBUS, null, "process exception {0}", preException.Message);
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
            Tracer.WriteError(TraceGroup.RBUS, null, "post process exception {0}", postException.Message);
         }

         Tracer.WriteHigh(TraceGroup.RBUS, null, "stop");
         this.running = false;
      }

      #endregion

      #region Constructor

      private RobotCommunicationBus()
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
               result = "robot communication offline";
            }
            else if (this.GetFaultStatus(BusComponentId.LeftTrackController) != null)
            {
               result = "left track offline";
            }
            else if (this.GetFaultStatus(BusComponentId.LeftTrackLight) != null)
            {
               result = "left track light offline";
            }
            else if (this.GetFaultStatus(BusComponentId.LeftTrackMotor) != null)
            {
               result = "left track motor offline";
            }
            else if (this.GetFaultStatus(BusComponentId.RightTrackController) != null)
            {
               result = "right track offline";
            }
            else if (this.GetFaultStatus(BusComponentId.RightTrackLight) != null)
            {
               result = "right track light offline";
            }
            else if (this.GetFaultStatus(BusComponentId.RightTrackMotor) != null)
            {
               result = "right track motor offline";
            }
            else if (this.GetFaultStatus(BusComponentId.HubController) != null)
            {
               result = "hub offline";
            }
            else if (this.GetFaultStatus(BusComponentId.HubCameraLights) != null)
            {
               result = "hub camera/lights offline";
            }
            else if (this.GetFaultStatus(BusComponentId.HubPanMotor) != null)
            {
               result = "hub pan motor offline";
            }
            else if (this.GetFaultStatus(BusComponentId.HubTiltMotor) != null)
            {
               result = "hub tilt motor offline";
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
            else if (BusComponentId.LeftTrackController == id)
            {
               result = this.leftTrackController.FaultReason;
            }
            else if (BusComponentId.LeftTrackLight == id)
            {
               result = this.leftTrackController.Light.FaultReason;
            }
            else if (BusComponentId.LeftTrackMotor == id)
            {
               result = this.leftTrackController.TrackMotor.FaultReason;
            }
            else if (BusComponentId.RightTrackController == id)
            {
               result = this.rightTrackController.FaultReason;
            }
            else if (BusComponentId.RightTrackLight == id)
            {
               result = this.rightTrackController.Light.FaultReason;
            }
            else if (BusComponentId.RightTrackMotor == id)
            {
               result = this.rightTrackController.TrackMotor.FaultReason;
            }
            else if (BusComponentId.HubController == id)
            {
               result = this.hubController.FaultReason;
            }
            else if (BusComponentId.HubCameraLights == id)
            {
               result = this.hubController.CameraLights.FaultReason;
            }
            else if (BusComponentId.HubPanMotor == id)
            {
               result = this.hubController.PanMotor.FaultReason;
            }
            else if (BusComponentId.HubTiltMotor == id)
            {
               result = this.hubController.TiltMotor.FaultReason;
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
               result = "robot communication offline";
            }
            else if (this.GetWarningStatus(BusComponentId.LeftTrackController) != null)
            {
               result = "left track offline";
            }
            else if (this.GetWarningStatus(BusComponentId.LeftTrackLight) != null)
            {
               result = "left track light error";
            }
            else if (this.GetWarningStatus(BusComponentId.LeftTrackMotor) != null)
            {
               result = "left track motor error";
            }
            else if (this.GetWarningStatus(BusComponentId.RightTrackController) != null)
            {
               result = "right track  offline";
            }
            else if (this.GetWarningStatus(BusComponentId.RightTrackLight) != null)
            {
               result = "right track light error";
            }
            else if (this.GetWarningStatus(BusComponentId.RightTrackMotor) != null)
            {
               result = "right track motor error";
            }
            else if (this.GetWarningStatus(BusComponentId.HubController) != null)
            {
               result = "hub offline";
            }
            else if (this.GetWarningStatus(BusComponentId.HubCameraLights) != null)
            {
               result = "hub camera/lights error";
            }
            else if (this.GetWarningStatus(BusComponentId.HubPanMotor) != null)
            {
               result = "hub pan motor error";
            }
            else if (this.GetWarningStatus(BusComponentId.HubTiltMotor) != null)
            {
               result = "hub tilt motor error";
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
            else if (BusComponentId.LeftTrackController == id)
            {
               result = this.leftTrackController.Warning;
            }
            else if (BusComponentId.LeftTrackLight == id)
            {
               result = this.leftTrackController.Light.Warning;
            }
            else if (BusComponentId.LeftTrackMotor == id)
            {
               result = this.leftTrackController.TrackMotor.Warning;
            }
            else if (BusComponentId.RightTrackController == id)
            {
               result = this.rightTrackController.Warning;
            }
            else if (BusComponentId.RightTrackLight == id)
            {
               result = this.rightTrackController.Light.Warning;
            }
            else if (BusComponentId.RightTrackMotor == id)
            {
               result = this.rightTrackController.TrackMotor.Warning;
            }
            else if (BusComponentId.HubController == id)
            {
               result = this.hubController.Warning;
            }
            else if (BusComponentId.HubCameraLights == id)
            {
               result = this.hubController.CameraLights.Warning;
            }
            else if (BusComponentId.HubPanMotor == id)
            {
               result = this.hubController.PanMotor.Warning;
            }
            else if (BusComponentId.HubTiltMotor == id)
            {
               result = this.hubController.TiltMotor.Warning;
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
            if (BusComponentId.LeftTrackController == id)
            {
               result = this.leftTrackController;
            }
            else if (BusComponentId.LeftTrackLight == id)
            {
               result = this.leftTrackController.Light;
            }
            else if (BusComponentId.LeftTrackMotor == id)
            {
               result = this.leftTrackController.TrackMotor;
            }
            else if (BusComponentId.RightTrackController == id)
            {
               result = this.rightTrackController;
            }
            else if (BusComponentId.RightTrackLight == id)
            {
               result = this.rightTrackController.Light;
            }
            else if (BusComponentId.RightTrackMotor == id)
            {
               result = this.rightTrackController.TrackMotor;
            }
            else if (BusComponentId.HubController == id)
            {
               result = this.hubController;
            }
            else if (BusComponentId.HubCameraLights == id)
            {
               result = this.hubController.CameraLights;
            }
            else if (BusComponentId.HubPanMotor == id)
            {
               result = this.hubController.PanMotor;
            }
            else if (BusComponentId.HubTiltMotor == id)
            {
               result = this.hubController.TiltMotor;
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
         Tracer.WriteHigh(TraceGroup.RBUS, "", "Stop All");
         this.stopAll = true;
      }

      public void DoSync()
      {
         PCANLight.SendSync(this.busInterfaceId);
      }

      #endregion

      #region Laser Body Functions

      public double GetLaserMainRoll()
      {
         return (0);
      }

      public double GetLaserMainPitch()
      {
         return (0);
      }

      public double GetLaserMainYaw()
      {
         return (0);
      }

      #endregion

      #region Laser Movement Functions

      public void SetLaserMovementLock(bool locked)
      {
         this.laserLocked = locked;
      }

      public bool GetLaserMovementLock()
      {
         bool result = this.laserLocked;
         return (result);
      }

      public void ResetLaserMoved()
      {
         if (false != this.laserMoved)
         {
            this.laserMoved = false;
         }
      }

      public bool GetLaserMoved()
      {
         return (this.laserMoved);
      }

      public void SetLaserMovementManualMode(bool active)
      {
         Tracer.WriteHigh(TraceGroup.RBUS, "", "requested laser manual movement mode={0}", active);
         this.laserManualMovementMode = active;
      }

      public void SetLaserMovementMode(MovementModes mode)
      {
         Tracer.WriteHigh(TraceGroup.RBUS, "", "requested laser movement mode={0}", mode);
         this.laserMovementMode = mode;
      }

      public void SetLaserMovementPositionRequest(double request)
      {
         int adjustment = (int)(request * ParameterAccessor.Instance.LaserWheelDistanceToTicks);
         int invertor;

         invertor = (false == ParameterAccessor.Instance.LaserFrontWheel.PositionInverted) ? 1 : -1;
         invertor *= (false == ParameterAccessor.Instance.LaserFrontWheel.RequestInverted) ? 1 : -1;
         int frontAdjustment = (adjustment * invertor);

         invertor = (false == ParameterAccessor.Instance.LaserRearWheel.PositionInverted) ? 1 : -1;
         invertor *= (false == ParameterAccessor.Instance.LaserRearWheel.RequestInverted) ? 1 : -1;
         int rearAdjustment = (adjustment * invertor);

         lock (this.valueUpdate)
         {
            if (WheelMotorStates.enabled == ParameterAccessor.Instance.LaserFrontWheel.MotorState)
            {
            }

            if (WheelMotorStates.enabled == ParameterAccessor.Instance.LaserRearWheel.MotorState)
            {
            }

            this.laserMoved = true;
         }
      }

      public void SetLaserMovementVelocityRequest(double request, bool triggered)
      {
         this.laserMovementRequest = request;
         this.laserMovementTriggered = triggered;

         if (0 != request)
         {
            this.laserMoved = true;
         }
      }

      public bool GetLaserMovementManualMode()
      {
         return (this.laserManualMovementMode);
      }

      public MovementModes GetLaserMovementMode()
      {
         return (this.laserMovementMode);
      }

      public void GetLaserMovementRequestValues(ref ValueParameter movementParameter, ref double movementRequestValue)
      {
         movementParameter = ParameterAccessor.Instance.LaserWheelMaximumSpeed;
         movementRequestValue = this.laserMovementRequest * movementParameter.OperationalValue;
      }

      public double GetLaserMovementValue()
      {
         double result = 0;
         int count = 0;


         if (0 != count)
         {
            result /= count;
         }

         return (result);
      }

      public bool GetLaserMovementActivated()
      {
         bool result = ((false != this.laserMovementTriggered) && (MovementModes.move == this.laserMovementMode)) ? true : false;
         return (result);
      }

      public double GetLaserWheelCurrentValue(WheelLocations location)
      {
         double result = 0;

         if (WheelLocations.front == location)
         {
         }
         else if (WheelLocations.rear == location)
         {
         }

         return (result);
      }

      public double GetLaserWheelTemperatureValue(WheelLocations location)
      {
         double result = 0;

         if (WheelLocations.front == location)
         {
         }
         else if (WheelLocations.rear == location)
         {
         }

         return (result);
      }

      public double GetLaserWheelPositionValue(WheelLocations location)
      {
         double result = 0;

         if (WheelLocations.front == location)
         {
         }
         else if (WheelLocations.rear == location)
         {
         }

         result /= ParameterAccessor.Instance.LaserWheelDistanceToTicks;

         return (result);
      }

      public double GetLaserWheelTotalPositionValue()
      {
         double result = 0;
         result /= ParameterAccessor.Instance.LaserWheelDistanceToTicks;
         return (result);
      }

      public double GetLaserWheelTripPositionValue()
      {
         double result = 0;
         result /= ParameterAccessor.Instance.LaserWheelDistanceToTicks;
         return (result);
      }

      #endregion

      #region Lights and Camera
      
      public void SetLightLevel(Controls.SystemLocations systemLocation, int level)
      {
         lock (this.valueUpdate)
         {
            if (Controls.SystemLocations.crawlerLeft == systemLocation)
            {
               this.crawlerLeftTrackLightIntensitySetPoint = level;
            }
            else if (Controls.SystemLocations.crawlerRight == systemLocation)
            {
               this.crawlerRightTrackLightIntensitySetPoint = level;
            }
            else if ((Controls.SystemLocations.crawlerFront == systemLocation) ||
                     (Controls.SystemLocations.crawlerRear == systemLocation))
            {
               this.crawlerHubLightIntensitySetPoint = level;
            }         
         }
      }

      public int GetLightLevel(Controls.SystemLocations systemLocation)
      {
         int result = 0;

         if (Controls.SystemLocations.crawlerLeft == systemLocation)
         {
            result = this.crawlerLeftTrackLightIntensitySetPoint; ;
         }
         else if (Controls.SystemLocations.crawlerRight == systemLocation)
         {
            result = this.crawlerRightTrackLightIntensitySetPoint; ;
         }
         else if ((Controls.SystemLocations.crawlerFront == systemLocation) ||
                  (Controls.SystemLocations.crawlerRear == systemLocation))
         {
            result = this.crawlerHubLightIntensitySetPoint; ;
         }
         
         return (result);
      }

      public void SetLightChannelMask(Controls.SystemLocations systemLocation, int lightChannelMask)
      {
         if (Controls.SystemLocations.crawlerLeft == systemLocation) 
         {
            this.crawlerLeftTrackLightChannelMaskSetPoint = lightChannelMask;
         }
         else if (Controls.SystemLocations.crawlerRight == systemLocation) 
         {
            this.crawlerRightTrackLightChannelMaskSetPoint = lightChannelMask;
         }
         else if ((Controls.SystemLocations.crawlerFront == systemLocation) ||
                  (Controls.SystemLocations.crawlerRear == systemLocation))
         {
            this.crawlerHubLightChannelMaskSetPoint = lightChannelMask;
         }
      }
      
      public bool GetLightEnable(Controls.SystemLocations systemLocation)
      {
         bool result = false;
         int mask = 0;

         if ((Controls.SystemLocations.crawlerLeft == systemLocation) ||
             (Controls.SystemLocations.crawlerRight == systemLocation) ||
             (Controls.SystemLocations.crawlerFront == systemLocation))
         {
            mask = (int)(1 << 0);
         }
         else if (Controls.SystemLocations.crawlerRear == systemLocation)
         {
            mask = (int)(1 << 1);
         }

         if (Controls.SystemLocations.crawlerLeft == systemLocation)
         {
            result = ((this.crawlerLeftTrackLightChannelMaskSetPoint & mask) != 0) ? true : false;
         }
         else if (Controls.SystemLocations.crawlerRight == systemLocation)
         {
            result = ((this.crawlerRightTrackLightChannelMaskSetPoint & mask) != 0) ? true : false;
         }
         else if ((Controls.SystemLocations.crawlerFront == systemLocation) ||
                  (Controls.SystemLocations.crawlerRear == systemLocation))
         {
            result = ((this.crawlerHubLightChannelMaskSetPoint & mask) != 0) ? true : false;
         }

         return (result);
      }

      public void SetCrawlerCamera(Controls.SystemLocations systemLocation)
      {
         lock (this.valueUpdate)
         {
            this.hubCameraSelectSetPoint = ParameterAccessor.Instance.CrawlerHubCameraMaps.GetIndex(systemLocation);
         }
      }

      #endregion

      #region GPS Functions

      public double GetGpsLongitude()
      {
         double result = 0;// this.gps.Longitude;
         return (result);
      }

      public double GetGpsLatitude()
      {
         double result = 0;// this.gps.Latitude;
         return (result);
      }

      public DateTime GetGpsTime()
      {
         DateTime result = DateTime.Now;// this.gps.Utc;
         return (result);
      }

      #endregion

      #endregion
   }
}