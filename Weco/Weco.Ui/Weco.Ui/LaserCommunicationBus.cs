namespace Weco.Ui
{
   using System;
   using System.Collections;
   using System.Text;
   using System.Threading;

   using Weco.CAN;
   using Weco.PCANLight;
   using Weco.Utilities;

   public class LaserCommunicationBus
   {
      #region Definitions

      private const int BootTimeoutPeriod = 1500;

      public enum BusComponentId
      {
         Bus,
         LaserBoard,
         LaserBoardCameraLed,
         LaserBoardFrontWheel,
         LaserBoardRearWheel,
         LaserBoardLeftStepper,
         LaserBoardRightStepper,
      }

      #endregion

      #region Fields

      private static LaserCommunicationBus instance = null;

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

      private bool laserLocked;
      private bool laserMoved;
      private bool laserManualMovementMode;
      private MovementModes laserMovementMode;
      private double laserMovementRequest;
      private bool laserMovementTriggered;
      //private int laserTripStartValue;
      
      private WheelMotorStatus wheel0Status;
      private WheelMotorStatus wheel1Status;
      private StepperMotorStatus stepper0Status;
      private StepperMotorStatus stepper1Status;

      private bool laserAimSetPoint;
      private bool laserAimRequested;
      private bool needLaserMeasurementStart;
      //private bool needLaserMeasurementCancel;
      //private bool laserMeasureStartRequested;
      //private bool laserMeasureCancelRequested;
      //private bool laserBecameActive;
      //private int laserSampleCount;
      //private double laserAverageMeasurement;

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

         this.laserLocked = false;
         this.laserMoved = false;
         this.laserManualMovementMode = false;
         this.laserMovementMode = MovementModes.off;
         this.laserMovementRequest = 0;
         this.laserMovementTriggered = false;
         //this.laserTripStartValue = 0;

         this.wheel0Status = new WheelMotorStatus();
         this.wheel1Status = new WheelMotorStatus();
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

      private int GetCameraLightChannelMask(Controls.CameraLocations camera)
      {
         int result = 0;

         if (Controls.CameraLocations.crawlerFront == camera)
         {
            result = (int)(1 << 0);
         }
         else if (Controls.CameraLocations.crawlerRear == camera)
         {
            result = (int)(1 << 1);
         }

         return (result);
      }

      private int GetCameraSelectionValue(Controls.CameraLocations camera)
      {
         int result = 0;

         if (Controls.CameraLocations.crawlerFront == camera)
         {
            result = ParameterAccessor.Instance.LaserCameraMapping.Front;
         }
         else if (Controls.CameraLocations.crawlerRear == camera)
         {
            result = ParameterAccessor.Instance.LaserCameraMapping.Rear;
         }

         return (result);
      }

      private Controls.CameraLocations GetCamera(int selection)
      {
         Controls.CameraLocations result = Controls.CameraLocations.crawlerFront;

         if (0 == selection)
         {
            result = Controls.CameraLocations.crawlerFront;
         }
         else if (1 == selection)
         {
            result = Controls.CameraLocations.crawlerRear;
         }

         return (result);
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

         this.deviceReceiveCount++;
         Tracer.WriteMedium(TraceGroup.LBUS, "", "rx {0:X3} {1}", cobId, sb.ToString());
      }

      private void DeviceTraceTransmit(int cobId, byte[] data)
      {
         StringBuilder sb = new StringBuilder();

         for (int i = 0; i < data.Length; i++)
         {
            sb.AppendFormat("{0:X2}", data[i]);
         }

         Tracer.WriteMedium(TraceGroup.LBUS, "", "tx {0:X3} {1}", cobId, sb.ToString());
      }

      private bool DeviceTransmit(int id, byte[] data)
      {
         CANResult transmitResult = PCANLight.Send(this.busInterfaceId, id, data);
         bool result = (transmitResult == CANResult.ERR_OK) ? true : false;

         return (result);
      }

      private void DeviceFault(string name, int nodeId, string reason)
      {
         Tracer.WriteError(TraceGroup.LBUS, "", "fault with \"{0}\", node={1}, reason={2}", name, nodeId, reason);
      }

      private void DeviceWarning(string name, int nodeId, string reason)
      {
         Tracer.WriteError(TraceGroup.LBUS, "", "warning with \"{0}\", node={1}, reason={2}", name, nodeId, reason);
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

         this.laserAimSetPoint = false;
         this.laserAimRequested = false;
         this.needLaserMeasurementStart = false;
         //this.needLaserMeasurementCancel = false;
         //this.laserMeasureStartRequested = false;
         //this.laserMeasureCancelRequested = false;
         //this.laserBecameActive = false;
         //this.laserSampleCount = 0;
         //this.laserAverageMeasurement = 0;

         this.wheel0Status.Initialize();
         this.wheel1Status.Initialize();
         this.stepper0Status.Initialize();
         this.stepper1Status.Initialize();

         this.camaraLightIntensitySetPoint = 0;
         //this.camaraLightIntensityRequested = 0;

         this.cameraLightChannelMaskSetPoint = 0;
         //this.cameraLightChannelMaskRequested = 0;

         this.cameraSetPoint = 0;
         //this.cameraRequested = 0;
      }

      private void StartLaserBoard()
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
               Tracer.WriteHigh(TraceGroup.LBUS, "", "{0} set", motor.Name);
            }

            else if (status.state == WheelMotorStatus.States.turnOff)
            {
               motor.SetMode(MotorComponent.Modes.off);
               status.state = WheelMotorStatus.States.off;
               Tracer.WriteHigh(TraceGroup.LBUS, "", "{0} off", motor.Name);
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

               Tracer.WriteHigh(TraceGroup.LBUS, "", "{0} position mode", motor.Name);

               status.state = WheelMotorStatus.States.positioning;
            }
            else if (status.state == WheelMotorStatus.States.positioning)
            {
               if (false != status.stopNeeded)
               {
                  status.stopNeeded = false;

                  motor.Halt();
                  status.positionRequested = status.positionNeeded;
                  Tracer.WriteHigh(TraceGroup.LBUS, "", "{0} position stop", motor.Name);

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

                     Tracer.WriteHigh(TraceGroup.LBUS, "", "{0} position {1}", motor.Name, status.positionRequested);

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

                  Tracer.WriteHigh(TraceGroup.LBUS, "", "{0} position stopped at {1}", motor.Name, motor.ActualPosition);

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

               Tracer.WriteHigh(TraceGroup.LBUS, "", "{0} velocity {1} mode", motor.Name, parameters.ActuationMode.ToString());

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
                  Tracer.WriteHigh(TraceGroup.LBUS, "", "{0} velocity stop", motor.Name);

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

                  Tracer.WriteMedium(TraceGroup.LBUS, null, "{0} velocity={1:0.00} rpm={2}", motor.Name, movementRequestValue, velocityRpm);
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

                  Tracer.WriteHigh(TraceGroup.LBUS, "", "{0} velocity stopped at {1}", motor.Name, motor.ActualPosition);

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

            // need undefined state
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

               Tracer.WriteHigh(TraceGroup.LBUS, "", "{0} homing mode", motor.Name);

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

               if (false != status.centerNeeded)
               {
                  status.positionNeeded = parameters.CenterPosition;
                  status.positionTarget = parameters.CenterPosition;
                  motor.SetTargetPosition(status.positionNeeded, false);
                  status.positionRequested = status.positionNeeded;
               }
               else
               {
                  motor.SetTargetPosition(status.positionNeeded, false);
                  status.positionRequested = status.positionNeeded;
               }

               Tracer.WriteHigh(TraceGroup.LBUS, "", "{0} position {1}", motor.Name, status.positionRequested);

               motor.SetMode(MotorComponent.Modes.position);
               Tracer.WriteHigh(TraceGroup.LBUS, "", "{0} position mode", motor.Name);

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
                  status.positionTarget = parameters.CenterPosition;
                  motor.SetTargetPosition(status.positionNeeded, false);
                  status.positionRequested = status.positionNeeded;

                  Tracer.WriteHigh(TraceGroup.LBUS, "", "{0} position {1}", motor.Name, status.positionRequested);

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
                  Tracer.WriteHigh(TraceGroup.LBUS, "", "{0} stop", motor.Name);

                  positionObtained = false;
                  status.statusInvalidTimeLimit = now.AddMilliseconds(250);
                  status.actualNeeded = true;
                  status.state = StepperMotorStatus.States.stopping;
               }
               else if (status.positionRequested != status.positionNeeded)
               {
                  motor.SetTargetPosition(status.positionNeeded, false);
                  status.positionRequested = status.positionNeeded;

                  Tracer.WriteHigh(TraceGroup.LBUS, "", "{0} position {1}", motor.Name, status.positionRequested);

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

                  Tracer.WriteHigh(TraceGroup.LBUS, "", "{0} centered at {1}", motor.Name, status.actualPosition);

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

                  Tracer.WriteHigh(TraceGroup.LBUS, "", "{0} stopped at {1}", motor.Name, status.actualPosition);

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

      private void UpdateLaserBoard()
      {
      }

      #endregion

      #region GPS Functions

      private void InitializeGps()
      {
      }

      private void StartGps()
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


         this.TraceMask = ParameterAccessor.Instance.LaserBus.ControllerTraceMask;

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
         this.InitializeLaserBoard();
         this.InitializeGps();
      }

      private void StartBus()
      {
         this.busReady = false;

         if (false == this.busReady)
         {
            this.busInterfaceId = ParameterAccessor.Instance.LaserBus.BusInterface;
            CANResult startResult = PCANLight.Start(this.busInterfaceId, ParameterAccessor.Instance.LaserBus.BitRate, FramesType.INIT_TYPE_ST, TraceGroup.LBUS, this.BusReceiveHandler);
            this.busReady = (CANResult.ERR_OK == startResult);
         }

         if (false != this.busReady)
         {
            DateTime now = DateTime.Now;
            DateTime flushTimeLimit = now.AddMilliseconds(500);
            DateTime idleReceiveTimeLimit = now.AddMilliseconds(100);
            int deviceReceiveCheck = this.deviceReceiveCount;

            Tracer.WriteMedium(TraceGroup.LBUS, "", "bus flush start");

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

            Tracer.WriteMedium(TraceGroup.LBUS, "", "bus flush done");
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
            Tracer.WriteMedium(TraceGroup.LBUS, "", "bus failure");

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
                  this.StartLaserBoard();
               }
               else if (BusComponentId.LaserBoardCameraLed == id)
               {
                  // restart of component not done
               }
               else if (BusComponentId.LaserBoardFrontWheel == id)
               {
                  // restart of component not done
               }
               else if (BusComponentId.LaserBoardRearWheel == id)
               {
                  // restart of component not done
               }
               else if (BusComponentId.LaserBoardLeftStepper == id)
               {
                  // restart of component not done
               }
               else if (BusComponentId.LaserBoardRightStepper == id)
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

               if (BusComponentId.LaserBoard == id)
               {
               }
               else if (BusComponentId.LaserBoardCameraLed == id)
               {
                  //bool wasFaulted = false;
               }
               else if (BusComponentId.LaserBoardFrontWheel == id)
               {
                  bool wasFaulted = false;

                  if (false != wasFaulted)
                  {
                     this.wheel0Status.state = WheelMotorStatus.States.off;
                  }
               }
               else if (BusComponentId.LaserBoardRearWheel == id)
               {
                  bool wasFaulted = false;

                  if (false != wasFaulted)
                  {
                     this.wheel1Status.state = WheelMotorStatus.States.off;
                  }
               }
               else if (BusComponentId.LaserBoardLeftStepper == id)
               {
                  bool wasFaulted = false;

                  if (false != wasFaulted)
                  {
                     this.stepper0Status.state = StepperMotorStatus.States.off;
                  }
               }
               else if (BusComponentId.LaserBoardRightStepper == id)
               {
                  bool wasFaulted = false;

                  if (false != wasFaulted)
                  {
                     this.stepper1Status.state = StepperMotorStatus.States.off;
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
         this.controllerHeartbeatLimit = DateTime.Now.AddMilliseconds(ParameterAccessor.Instance.LaserBus.ProducerHeartbeatRate);

         this.StartLaserBoard();

         this.ready = true;

         for (; this.execute; )
         {
            lock (this.valueUpdate)
            {
               this.UpdateLaserBoard();
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

         ParameterAccessor.Instance.LaserBus.ControllerTraceMask = this.TraceMask;
      }

      private void Process()
      {
         try
         {
            this.running = true;
            Tracer.WriteHigh(TraceGroup.LBUS, null, "start");

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
            Tracer.WriteError(TraceGroup.LBUS, null, "process exception {0}", preException.Message);
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
            Tracer.WriteError(TraceGroup.LBUS, null, "post process exception {0}", postException.Message);
         }

         Tracer.WriteHigh(TraceGroup.LBUS, null, "stop");
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
            else if (this.GetFaultStatus(BusComponentId.LaserBoardCameraLed) != null)
            {
               result = "laser board camera/led offline";
            }
            else if (this.GetFaultStatus(BusComponentId.LaserBoardFrontWheel) != null)
            {
               result = "laser board front wheel offline";
            }
            else if (this.GetFaultStatus(BusComponentId.LaserBoardRearWheel) != null)
            {
               result = "laser board rear wheel offline";
            }
            else if (this.GetFaultStatus(BusComponentId.LaserBoardLeftStepper) != null)
            {
               result = "laser board left stepper offline";
            }
            else if (this.GetFaultStatus(BusComponentId.LaserBoardRightStepper) != null)
            {
               result = "laser board right stepper offline";
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
            else if (this.GetWarningStatus(BusComponentId.LaserBoardCameraLed) != null)
            {
               result = "laser board camera/led error";
            }
            else if (this.GetWarningStatus(BusComponentId.LaserBoardFrontWheel) != null)
            {
               result = "laser board front wheel error";
            }
            else if (this.GetWarningStatus(BusComponentId.LaserBoardRearWheel) != null)
            {
               result = "laser board rear wheel error";
            }
            else if (this.GetWarningStatus(BusComponentId.LaserBoardLeftStepper) != null)
            {
               result = "laser board left stepper error";
            }
            else if (this.GetWarningStatus(BusComponentId.LaserBoardRightStepper) != null)
            {
               result = "laser board right stepper error";
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
         Tracer.WriteHigh(TraceGroup.LBUS, "", "Stop All");
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
         Tracer.WriteHigh(TraceGroup.LBUS, "", "requested laser manual movement mode={0}", active);
         this.laserManualMovementMode = active;
      }

      public void SetLaserMovementMode(MovementModes mode)
      {
         Tracer.WriteHigh(TraceGroup.LBUS, "", "requested laser movement mode={0}", mode);
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

      public double GetLaserLinkVoltage()
      {
         double result = 0;
         return (result);
      }

      #endregion

      #region Laser Stepper Functions

      #region General

      public void SetLaserCenter()
      {
         this.stepper0Status.centerNeeded = true;
         this.stepper1Status.centerNeeded = true;
      }

      #endregion

      #region Left Stepper

      public void StopLaserLeftStepper()
      {
         this.stepper0Status.stopNeeded = true;
      }

      public void SetLaserLeftStepperTargetPosition(int position)
      {
         this.stepper0Status.positionTarget = position;
         this.stepper0Status.positionNeeded = position;
      }

      public int GetLaserLeftStepperTargetPosition()
      {
         int result = this.stepper0Status.positionTarget;
         return (result);
      }

      public int GetLaserLeftStepperPosition()
      {
         int result = this.stepper0Status.actualPosition;
         return (result);      
      }

      public bool GetLaserLeftStepperHomeSwitchActive()
      {
         bool result = false;
         return (result);
      }

      #endregion

      #region Right Stepper

      public void StopLaserRightStepper()
      {
         this.stepper1Status.stopNeeded = true;
      }

      public void SetLaserRightStepperTargetPosition(int position)
      {
         this.stepper1Status.positionTarget = position;
         this.stepper1Status.positionNeeded = position;
      }

      public int GetLaserRightStepperTargetPosition()
      {
         int result = stepper1Status.positionTarget;
         return (result);
      }

      public int GetLaserRightStepperPosition()
      {
         int result = this.stepper1Status.actualPosition;
         return (result);
      }

      public bool GetLaserRightStepperHomeSwitchActive()
      {
         bool result = false;
         return (result);
      }

      #endregion

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
         //this.needLaserMeasurementCancel = true;
      }

      public bool GetLaserMeasurementActivity()
      {
         bool result = false;

         if (false != this.running)
         {
         }

         return (result);
      }

      public int GetLaserSampleRemainingCount()
      {
         int result = 0;

         if (false != this.running)
         {
         }

         return (result);
      }

      public bool GetLaserMeasurementReady()
      {
         bool result = false;

         if (false != this.running)
         {
            if (false == this.needLaserMeasurementStart)
            {
            }
         }

         return (result);
      }

      public double GetAverageLaserMeasurement()
      {
         return (0);
         //return (this.laserAverageMeasurement);
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

      public void SetLaserCamera(Controls.CameraLocations camera)
      {
         lock (this.valueUpdate)
         {
            this.cameraSetPoint = this.GetCameraSelectionValue(camera);
         }
      }

      public Controls.CameraLocations GetLaserCamera()
      {
         Controls.CameraLocations result = this.GetCamera(this.cameraSetPoint);
         return (result);
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