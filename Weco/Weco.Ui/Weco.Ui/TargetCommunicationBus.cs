namespace Weco.Ui
{
   using System;
   using System.Collections;
   using System.Text;
   using System.Threading;

   using Weco.CAN;
   using Weco.PCANLight;
   using Weco.Utilities;

   public class TargetCommunicationBus
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
         TargetBoardCameraStepper,
      }

      #endregion

      #region Fields

      private static TargetCommunicationBus instance = null;

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

      private bool targetLocked;
      private bool targetMoved;
      private bool targetManualMovementMode;
      private MovementModes targetMovementMode;
      private double targetMovementRequest;
      private bool targetMovementTriggered;
      //private int targetTripStartValue;

      private WheelMotorStatus wheel0Status;
      private WheelMotorStatus wheel1Status;
      private StepperMotorStatus stepperStatus;

      private int camaraLightIntensitySetPoint;
      //private int camaraLightIntensityRequested;

      private int cameraLightChannelMaskSetPoint;
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

         this.targetLocked = false;
         this.targetMoved = false;
         this.targetManualMovementMode = false;
         this.targetMovementMode = MovementModes.off;
         this.targetMovementRequest = 0;
         this.targetMovementTriggered = false;
         //this.targetTripStartValue = 0;

         this.wheel0Status = new WheelMotorStatus();
         this.wheel1Status = new WheelMotorStatus();
         this.stepperStatus = new StepperMotorStatus();
      }

      private void SendControllerHeartBeat()
      {
         int cobId = (int)(((int)COBTypes.ERROR << 7) | (ParameterAccessor.Instance.TargetBus.ControllerBusId & 0x7F));
         byte[] heartbeatMsg = new byte[1];

         heartbeatMsg[0] = 5;

         this.DeviceTransmit(cobId, heartbeatMsg);

         if (false != this.TraceHB)
         {
            this.DeviceTraceTransmit(cobId, heartbeatMsg);
         }
      }

      private int GetCameraLightChannelMask(Controls.CameraLocations camera)
      {
         int result = 0;

         if (Controls.CameraLocations.bulletLeft == camera)
         {
            result = (int)(1 << 0);
         }
         else if (Controls.CameraLocations.bulletRight == camera)
         {
            result = (int)(1 << 1);
         }
         else if (Controls.CameraLocations.bulletDown == camera)
         {
            result = (int)(1 << 2);
         }

         return (result);
      }

      private int GetCameraSelectionValue(Controls.CameraLocations camera)
      {
         int result = 0;

         if (Controls.CameraLocations.bulletLeft == camera)
         {
            result = ParameterAccessor.Instance.TargetCameraMapping.Front;
         }
         else if (Controls.CameraLocations.bulletRight == camera)
         {
            result = ParameterAccessor.Instance.TargetCameraMapping.Rear;
         }
         else if (Controls.CameraLocations.bulletDown == camera)
         {
            result = ParameterAccessor.Instance.TargetCameraMapping.Top;
         }

         return (result);
      }

      private Controls.CameraLocations GetCamera(int selection)
      {
         Controls.CameraLocations result = Controls.CameraLocations.none;

         if (0 == selection)
         {
            result = Controls.CameraLocations.bulletLeft;
         }
         else if (1 == selection)
         {
            result = Controls.CameraLocations.bulletRight;
         }
         else if (2 == selection)
         {
            result = Controls.CameraLocations.bulletDown;
         }

         return (result);
      }

      #endregion

      #region Properties

      public static TargetCommunicationBus Instance
      {
         get
         {
            if (instance == null)
            {
               instance = new TargetCommunicationBus();
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
         if ((0 != ParameterAccessor.Instance.TargetBus.ProducerHeartbeatRate) &&
             (false != this.controllerServiced) &&
             (DateTime.Now > this.controllerHeartbeatLimit))
         {
            this.SendControllerHeartBeat();
            this.controllerHeartbeatLimit = this.controllerHeartbeatLimit.AddMilliseconds(ParameterAccessor.Instance.TargetBus.ProducerHeartbeatRate);
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

      #region Target Board Functions

      private void InitializeTargetBoard()
      {
         this.wheel0Status.Initialize();
         this.wheel1Status.Initialize();
         this.stepperStatus.Initialize();

         this.camaraLightIntensitySetPoint = 0;
         //this.camaraLightIntensityRequested = 0;

         this.cameraLightChannelMaskSetPoint = 0;
         //this.cameraLightChannelMaskRequested = 0;

         this.cameraSetPoint = 0;
         //this.cameraRequested = 0;
      }

      private void StartTargetBoard()
      {
      }

      private void EvaluateWheel(MotorComponent motor, WheelMotorParameters parameters, WheelMotorStatus status, ref double total, ref int count)
      {
      }

      private bool UpdateWheel(MotorComponent motor, WheelMotorStatus status, WheelMotorParameters parameters)
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
               Tracer.WriteHigh(TraceGroup.TBUS, "", "{0} set", motor.Name);
            }

            else if (status.state == WheelMotorStatus.States.turnOff)
            {
               motor.SetMode(MotorComponent.Modes.off);
               status.state = WheelMotorStatus.States.off;
               Tracer.WriteHigh(TraceGroup.TBUS, "", "{0} off", motor.Name);
            }
            else if (status.state == WheelMotorStatus.States.off)
            {
               if (WheelMotorStates.locked == parameters.MotorState)
               {
                  status.state = WheelMotorStatus.States.startPosition;
               }
               else if (WheelMotorStates.enabled == parameters.MotorState)
               {
                  if (false != this.targetMovementTriggered)
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

               Tracer.WriteHigh(TraceGroup.TBUS, "", "{0} position mode", motor.Name);

               status.state = WheelMotorStatus.States.positioning;
            }
            else if (status.state == WheelMotorStatus.States.positioning)
            {
               if (false != status.stopNeeded)
               {
                  status.stopNeeded = false;

                  motor.Halt();
                  status.positionRequested = status.positionNeeded;
                  Tracer.WriteHigh(TraceGroup.TBUS, "", "{0} position stop", motor.Name);

                  positionObtained = false;
                  status.statusInvalidTimeLimit = now.AddMilliseconds(250);
                  status.state = WheelMotorStatus.States.stopPosition;
               }
               else if (WheelMotorStates.disabled == parameters.MotorState)
               {
                  status.stopNeeded = true;
               }
               else if ((WheelMotorStates.enabled == parameters.MotorState) &&
                        (false != this.targetMovementTriggered))
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

                     Tracer.WriteHigh(TraceGroup.TBUS, "", "{0} position {1}", motor.Name, status.positionRequested);

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

                  Tracer.WriteHigh(TraceGroup.TBUS, "", "{0} position stopped at {1}", motor.Name, motor.ActualPosition);

                  if (WheelMotorStates.disabled == parameters.MotorState)
                  {
                     status.state = WheelMotorStatus.States.turnOff;
                  }
                  else if ((WheelMotorStates.enabled == parameters.MotorState) &&
                           (false != this.targetMovementTriggered))
                  {
                     status.state = WheelMotorStatus.States.startVelocity;
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

               Tracer.WriteHigh(TraceGroup.TBUS, "", "{0} velocity {1} mode", motor.Name, parameters.ActuationMode.ToString());

               status.velocityActuationMode = parameters.ActuationMode;
               status.state = WheelMotorStatus.States.velocity;
            }
            else if (status.state == WheelMotorStatus.States.velocity)
            {
               if ((WheelMotorStates.disabled == parameters.MotorState) ||
                   (WheelMotorStates.locked == parameters.MotorState) ||
                   (false == this.targetMovementTriggered) ||
                   (status.velocityActuationMode != parameters.ActuationMode))
               {
                  motor.ScheduleTargetVelocity(0);
                  scheduled = true;
                  status.velocityRequested = 0;
                  Tracer.WriteHigh(TraceGroup.TBUS, "", "{0} velocity stop", motor.Name);

                  velocityObtained = false;
                  status.statusInvalidTimeLimit = now.AddMilliseconds(250);
                  status.state = WheelMotorStatus.States.stopVelocity;
               }
               else if (this.targetMovementRequest != status.velocityRequested)
               {
                  ValueParameter movementParameter = ParameterAccessor.Instance.TargetWheelMaximumSpeed;

                  double movementRequestValue = this.targetMovementRequest * movementParameter.OperationalValue;
                  int positionInversionValue = (false == parameters.PositionInverted) ? 1 : -1;
                  int requestInversionValue = (false == parameters.RequestInverted) ? 1 : -1;
                  int velocityRpm = (int)(positionInversionValue * requestInversionValue * movementRequestValue * ParameterAccessor.Instance.TargetWheelVelocityToRpm);
                  motor.ScheduleTargetVelocity(velocityRpm);
                  scheduled = true;
                  status.velocityRequested = this.targetMovementRequest;

                  Tracer.WriteMedium(TraceGroup.TBUS, null, "{0} velocity={1:0.00} rpm={2}", motor.Name, movementRequestValue, velocityRpm);
               }
            }
            else if (status.state == WheelMotorStatus.States.stopVelocity)
            {
               if ((WheelMotorStates.enabled == parameters.MotorState) &&
                   (false != this.targetMovementTriggered))
               {
                  status.state = WheelMotorStatus.States.velocity;
               }
               else if (false != velocityObtained)
               {
                  status.positionNeeded = motor.ActualPosition;
                  motor.ScheduleTargetPosition(status.positionNeeded);
                  scheduled = true;
                  status.positionRequested = status.positionNeeded;

                  Tracer.WriteHigh(TraceGroup.TBUS, "", "{0} velocity stopped at {1}", motor.Name, motor.ActualPosition);

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

      private bool UpdateStepper(MotorComponent motor, StepperMotorStatus status, StepperMotorParameters parameters)
      {
         bool scheduled = false;

         if (null == motor.FaultReason)
         {
            DateTime now = DateTime.Now;
            bool positionObtained = false;

            if (now > status.statusInvalidTimeLimit)
            {
               positionObtained = motor.PositionAttained;
            }

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

               status.state = StepperMotorStatus.States.stopped;
            }
            else if (status.state == StepperMotorStatus.States.off)
            {
               if (false != status.homeNeeded)
               {
                  status.state = StepperMotorStatus.States.startHoming;
               }
               else
               {
                  status.state = StepperMotorStatus.States.startPosition;
               }
            }
            else if (status.state == StepperMotorStatus.States.stopped)
            {
               // nothing, reset needed to clear
            }

            else if (status.state == StepperMotorStatus.States.startHoming)
            {
               status.homeNeeded = false;

               motor.SetHomingMethod((byte)parameters.HomingMethod);
               motor.SetHomeOffset(parameters.HomeOffset);
               motor.SetHomingSwitchSpeed((UInt32)parameters.HomingSwitchVelocity);
               motor.SetHomingZeroSpeed((UInt32)parameters.HomingZeroVelocity);
               motor.SetHomingAcceleration((UInt32)parameters.HomingAcceleration);

               motor.SetMode(MotorComponent.Modes.homing);
               motor.StartHoming();

               Tracer.WriteHigh(TraceGroup.TBUS, "", "{0} homing mode", motor.Name);

               positionObtained = false;
               status.statusInvalidTimeLimit = now.AddMilliseconds(250);
               status.actualNeeded = true;
               status.state = StepperMotorStatus.States.homing;
            }
            else if (status.state == StepperMotorStatus.States.homing)
            {
               if (false != motor.HomingAttained)
               {
                  status.centerNeeded = true;
                  status.state = StepperMotorStatus.States.startPosition;
               }
            }

            else if (status.state == StepperMotorStatus.States.startPosition)
            {
               motor.SetProfileVelocity(parameters.ProfileVelocity);
               motor.SetProfileAcceleration(parameters.ProfileAcceleration);
               motor.SetMode(MotorComponent.Modes.position);

               Tracer.WriteHigh(TraceGroup.TBUS, "", "{0} position mode", motor.Name);

               status.state = StepperMotorStatus.States.positioning;
            }
            else if (status.state == StepperMotorStatus.States.positioning)
            {
               if (false != status.homeNeeded)
               {
                  status.state = StepperMotorStatus.States.startHoming;
               }
               else if (false != status.centerNeeded)
               {
                  status.centerNeeded = false;

                  status.positionNeeded = parameters.CenterPosition;
                  motor.SetTargetPosition(status.positionNeeded, false);
                  status.positionRequested = status.positionNeeded;

                  Tracer.WriteHigh(TraceGroup.TBUS, "", "{0} position {1}", motor.Name, status.positionRequested);

                  positionObtained = false;
                  status.statusInvalidTimeLimit = now.AddMilliseconds(250);
                  status.actualNeeded = true;
                  status.state = StepperMotorStatus.States.centering;
               }
               else if (false != status.stopNeeded)
               {
                  status.stopNeeded = false;

                  motor.Halt();
                  status.positionRequested = status.positionNeeded;
                  Tracer.WriteHigh(TraceGroup.TBUS, "", "{0} stop", motor.Name);

                  positionObtained = false;
                  status.statusInvalidTimeLimit = now.AddMilliseconds(250);
                  status.actualNeeded = true;
                  status.state = StepperMotorStatus.States.stopping;
               }
               else if (status.positionRequested != status.positionNeeded)
               {
                  motor.SetTargetPosition(status.positionNeeded, false);
                  status.positionRequested = status.positionNeeded;

                  Tracer.WriteHigh(TraceGroup.TBUS, "", "{0} position {1}", motor.Name, status.positionRequested);

                  positionObtained = false;
                  status.statusInvalidTimeLimit = now.AddMilliseconds(250);
                  status.actualNeeded = true;
               }
            }
            else if (status.state == StepperMotorStatus.States.centering)
            {
               if (false != positionObtained)
               {
                  motor.GetActualPosition(ref status.actualPosition);

                  status.positionNeeded = status.actualPosition;
                  status.positionRequested = status.positionNeeded;

                  Tracer.WriteHigh(TraceGroup.TBUS, "", "{0} centered at {1}", motor.Name, status.actualPosition);

                  status.state = StepperMotorStatus.States.positioning;
               }
            }
            else if (status.state == StepperMotorStatus.States.stopping)
            {
               if (false != positionObtained)
               {
                  motor.GetActualPosition(ref status.actualPosition);

                  status.positionNeeded = status.actualPosition;
                  motor.SetTargetPosition(status.positionNeeded, false);
                  status.positionRequested = status.positionNeeded;

                  motor.Run();

                  Tracer.WriteHigh(TraceGroup.TBUS, "", "{0} stopped at {1}", motor.Name, status.actualPosition);

                  status.state = StepperMotorStatus.States.positioning;
               }
            }

            if ((false != status.actualNeeded) &&
                (now > status.readTimeLimit))
            {
               motor.GetActualPosition(ref status.actualPosition);

               if (false != positionObtained)
               {
                  status.actualNeeded = false;
               }
               else
               {
                  status.readTimeLimit = now.AddMilliseconds(250);
               }
            }
         }

         return (scheduled);
      }

      private void UpdateTargetBoard()
      {
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


         this.TraceMask = ParameterAccessor.Instance.TargetBus.ControllerTraceMask;

         this.stopAll = false;

         this.targetLocked = false;
         this.targetMoved = false;
         this.targetManualMovementMode = false;
         this.targetMovementMode = MovementModes.off;
         this.targetMovementRequest = 0;
         this.targetMovementTriggered = false;
         //this.targetTripStartValue = 0;
      }

      private void InitializeDevices()
      {
         this.InitializeTargetBoard();
      }

      private void StartBus()
      {
         this.busReady = false;

         if (false == this.busReady)
         {
            this.busInterfaceId = ParameterAccessor.Instance.TargetBus.BusInterface;
            CANResult startResult = PCANLight.Start(this.busInterfaceId, ParameterAccessor.Instance.TargetBus.BitRate, FramesType.INIT_TYPE_ST, TraceGroup.TBUS, this.BusReceiveHandler);
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
         
         // wait for laser bus when target board is over there
         if (false != ParameterAccessor.Instance.RunTargetOnLaserBus)
         {
            for (; this.execute; )
            {
               if (false != LaserCommunicationBus.Instance.Ready)
               {
                  break;
               }
            }
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
                  this.InitializeTargetBoard();
                  this.StartTargetBoard();
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
               else if (BusComponentId.TargetBoardCameraStepper == id)
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
               else if (BusComponentId.TargetBoardCameraStepper == id)
               {
                  bool wasFaulted = false;

                  if (false != wasFaulted)
                  {
                     this.stepperStatus.state = StepperMotorStatus.States.off;
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
         this.controllerHeartbeatLimit = DateTime.Now.AddMilliseconds(ParameterAccessor.Instance.TargetBus.ProducerHeartbeatRate);

         this.StartTargetBoard();

         this.ready = true;

         for (; this.execute; )
         {
            lock (this.valueUpdate)
            {
               this.UpdateTargetBoard();
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

         ParameterAccessor.Instance.TargetBus.ControllerTraceMask = this.TraceMask;
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

      private TargetCommunicationBus()
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
            if ((this.GetFaultStatus(BusComponentId.Bus) != null) &&
                (false == ParameterAccessor.Instance.RunTargetOnLaserBus))
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
            else if (this.GetFaultStatus(BusComponentId.TargetBoardCameraStepper) != null)
            {
               result = "target board stepper offline";
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
            else if (BusComponentId.TargetBoardCameraStepper == id)
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
            else if (this.GetWarningStatus(BusComponentId.TargetBoardCameraStepper) != null)
            {
               result = "target board stepper error";
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
            else if (BusComponentId.TargetBoardCameraStepper == id)
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
            else if (BusComponentId.TargetBoardCameraStepper == id)
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

      #region Target Body Functions

      public double GetTargetMainRoll()
      {
         return (0);
      }

      public double GetTargetMainPitch()
      {
         return (0);
      }

      public double GetTargetMainYaw()
      {
         return (0);
      }

      public double GetTargetTopCameraRoll()
      {
         return (0);
      }

      #endregion

      #region Target Movement Functions

      public void SetTargetMovementLock(bool locked)
      {
         this.targetLocked = locked;
      }

      public bool GetTargetMovementLock()
      {
         bool result = this.targetLocked;
         return (result);
      }

      public void ResetTargetMoved()
      {
         if (false != this.targetMoved)
         {
            this.targetMoved = false;
         }
      }

      public bool GetTargetMoved()
      {
         return (this.targetMoved);
      }

      public void SetTargetMovementManualMode(bool active)
      {
         Tracer.WriteHigh(TraceGroup.TBUS, "", "requested target manual movement mode={0}", active);
         this.targetManualMovementMode = active;
      }

      public void SetTargetMovementMode(MovementModes mode)
      {
         Tracer.WriteHigh(TraceGroup.TBUS, "", "requested target movement mode={0}", mode);
         this.targetMovementMode = mode;
      }

      public void SetTargetMovementPositionRequest(double request)
      {
         int adjustment = (int)(request * ParameterAccessor.Instance.TargetWheelDistanceToTicks);
         int invertor;

         invertor = (false == ParameterAccessor.Instance.TargetFrontWheel.PositionInverted) ? 1 : -1;
         invertor *= (false == ParameterAccessor.Instance.TargetFrontWheel.RequestInverted) ? 1 : -1;
         int frontAdjustment = (adjustment * invertor);

         invertor = (false == ParameterAccessor.Instance.TargetRearWheel.PositionInverted) ? 1 : -1;
         invertor *= (false == ParameterAccessor.Instance.TargetRearWheel.RequestInverted) ? 1 : -1;
         int rearAdjustment = (adjustment * invertor);

         lock (this.valueUpdate)
         {
            if (WheelMotorStates.enabled == ParameterAccessor.Instance.TargetFrontWheel.MotorState)
            {
            }

            if (WheelMotorStates.enabled == ParameterAccessor.Instance.TargetRearWheel.MotorState)
            {
            }

            this.targetMoved = true;
         }
      }

      public void SetTargetMovementVelocityRequest(double request, bool triggered)
      {
         this.targetMovementRequest = request;
         this.targetMovementTriggered = triggered;

         if (0 != request)
         {
            this.targetMoved = true;
         }
      }

      public bool GetTargetMovementManualMode()
      {
         return (this.targetManualMovementMode);
      }

      public MovementModes GetTargetMovementMode()
      {
         return (this.targetMovementMode);
      }

      public void GetTargetMovementRequestValues(ref ValueParameter movementParameter, ref double movementRequestValue)
      {
         movementParameter = ParameterAccessor.Instance.TargetWheelMaximumSpeed;
         movementRequestValue = this.targetMovementRequest * movementParameter.OperationalValue;
      }

      public double GetTargetMovementValue()
      {
         double result = 0;
         int count = 0;


         if (0 != count)
         {
            result /= count;
         }

         return (result);
      }

      public bool GetTargetMovementActivated()
      {
         bool result = ((false != this.targetMovementTriggered) && (MovementModes.move == this.targetMovementMode)) ? true : false;
         return (result);
      }

      public double GetTargetWheelCurrentValue(WheelLocations location)
      {
         double result = 0;

         if (WheelLocations.front == location)
         {
            double current = 0;// this.targetBoard.Bldc0.ActualCurrent;
            current /= ParameterAccessor.Instance.TargetWheelCountsToAmps;
            result = current;
         }
         else if (WheelLocations.rear == location)
         {
            double current = 0;// this.targetBoard.Bldc1.ActualCurrent;
            current /= ParameterAccessor.Instance.TargetWheelCountsToAmps;
            result = current;
         }

         return (result);
      }

      public double GetTargetWheelTemperatureValue(WheelLocations location)
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

      public double GetTargetWheelPositionValue(WheelLocations location)
      {
         double result = 0;

         if (WheelLocations.front == location)
         {
         }
         else if (WheelLocations.rear == location)
         {
         }

         result /= ParameterAccessor.Instance.TargetWheelDistanceToTicks;

         return (result);
      }

      public double GetTargetWheelTotalPositionValue()
      {
         double result = 0;// this.targetBoard.Bldc0.ActualPosition;
         result /= ParameterAccessor.Instance.TargetWheelDistanceToTicks;
         return (result);
      }

      public double GetTargetWheelTripPositionValue()
      {
         double result = 0;// this.targetBoard.Bldc0.ActualPosition - this.targetTripStartValue;
         result /= ParameterAccessor.Instance.TargetWheelDistanceToTicks;
         return (result);
      }

      public double GetTargetLinkVoltage()
      {
         double result = 0;// this.targetBoard.DcLinkVoltage * ParameterAccessor.Instance.TargetLinkVoltageMultipler;
         return (result);
      }

      #endregion

      #region Target Stepper Functions

      public void SetTargetCenter()
      {
         this.stepperStatus.centerNeeded = true;
      }

      public void SetTargetStepperPosition(int position)
      {
         this.stepperStatus.positionNeeded = position;
      }

      public void StopTargetStepper()
      {
         this.stepperStatus.stopNeeded = true;
      }

      public int GetTargetStepperActualPosition()
      {
         int result = this.stepperStatus.actualPosition;
         return (result);
      }

      public bool GetTopCameraStepperHomeSwitchActive()
      {
         bool result = false;// this.targetBoard.Stepper0.HomeSwitchActive;
         return (result);
      }

      public bool TargetPositionObtained()
      {
         bool result = false;// this.targetBoard.Stepper0.PositionAttained;
         return (result);
      }

      #endregion

      #region Scanner Functions

      public UInt32 GetTargetScannerCoordinates()
      {
         return (0);
      }

      #endregion

      #region Lights and Camera

      public void SetCameraLightLevel(int level)
      {
         lock (this.valueUpdate)
         {
            this.camaraLightIntensitySetPoint = level;
         }
      }

      public int GetCameraLightLevel()
      {
         int result = this.camaraLightIntensitySetPoint;
         return (result);
      }

      public void SetCameraLightChannelMask(int cameraLightChannelMask)
      {
         this.cameraLightChannelMaskSetPoint = cameraLightChannelMask;
      }

      public int GetCameraLightChannelMask()
      {
         int result = this.cameraLightChannelMaskSetPoint;
         return (result);
      }

      public void SetCameraLightEnable(Controls.CameraLocations camera, bool enabled)
      {
         lock (this.valueUpdate)
         {
            int mask = this.GetCameraLightChannelMask(camera);

            if (false != enabled)
            {
               this.cameraLightChannelMaskSetPoint |= mask;
            }
            else
            {
               this.cameraLightChannelMaskSetPoint &= ~mask;
            }
         }
      }

      public bool GetCameraLightEnable(Controls.CameraLocations camera)
      {
         int mask = this.GetCameraLightChannelMask(camera);
         bool result = ((this.cameraLightChannelMaskSetPoint & mask) != 0) ? true : false;

         return (result);
      }

      public void SetTargetCamera(Controls.CameraLocations camera)
      {
         lock (this.valueUpdate)
         {
            this.cameraSetPoint = this.GetCameraSelectionValue(camera);
         }
      }

      public Controls.CameraLocations GetTargetCamera()
      {
         Controls.CameraLocations result = this.GetCamera(this.cameraSetPoint);
         return (result);
      }

      #endregion

      #endregion
   }
}