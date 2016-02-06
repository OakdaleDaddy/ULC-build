
namespace NICBOT.GUI
{
   // test fpr submit

   using System;
   using System.Collections;
   using System.Data;
   using System.IO;
   using System.Text;
   using System.Threading;
   using System.Xml;

   using NICBOT.CAN;
   using NICBOT.PCANLight;
   using NICBOT.Utilities;

   public class TruckCommBus
   {
      #region Definitions

      private const int BootTimeoutPeriod= 1000;

      //private bool SimulatePumpSpeed = true;
      //private bool SimulatePumpPressure = true;

      public enum BusComponentId
      {
         Bus,
         ReelMotor,
         ReelDigitalIo,
         ReelAnalogIo,
         ReelEncoder,
         FeederTopFrontMotor,
         FeederTopRearMotor,
         FeederBottomFrontMotor,
         FeederBottomRearMotor,
         FeederEncoder,
         GuideLeftMotor,
         GuideRightMotor,
         LaunchDigitalIo,
         LaunchAnalogIo,
         Gps,
         NitrogenSensor1,
         NitrogenSensor2,
         RobotTotalCurrentSensor,
         LaunchTotalCurrentSensor,
         FrontPumpMotor,
         FrontPressureSensor,
         FrontScaleRs232,
         FrontDigitalScale,
         RearPumpMotor,
         RearPressureSensor,
         RearScaleRs232,
         RearDigitalScale,
         ThicknessSensor,
         StressSensor,      
      }

      #endregion

      #region Fields

      private static TruckCommBus instance = null;

      private bool execute;
      private Thread thread;
      private Thread deviceThread;
      private bool ready;

      private BusInterfaces busInterfaceId;
      private bool busReady;
      private string busStatus;
      private Queue busReceiveQueue;
      private Queue deviceResetQueue;
      
      private ElmoWhistleMotor reelMotor;
      private PeakDigitalIo reelDigitalIo;
      private PeakAnalogIo reelAnalogIo;
      private KublerRotaryEncoder reelEncoder;
      private ElmoWhistleMotor feederTopFrontMotor;
      private ElmoWhistleMotor feederTopRearMotor;
      private ElmoWhistleMotor feederBottomFrontMotor;
      private ElmoWhistleMotor feederBottomRearMotor;
      private KublerRotaryEncoder feederEncoder;
      private ElmoWhistleMotor guideLeftMotor;
      private ElmoWhistleMotor guideRightMotor;
      private PeakDigitalIo launchDigitalIo;
      private PeakAnalogIo launchAnalogIo;
      private UlcRoboticsGps gps;
      private ElmoWhistleMotor frontPumpMotor;
      private UlcRoboticsRs232 frontScaleRs232;
      private ElmoWhistleMotor rearPumpMotor;
      private UlcRoboticsRs232 rearScaleRs232;

      private ArrayList deviceList;

      private DateTime controllerHeartbeatLimit;
      private bool controllerServiced;

      private string nitrogenSensor1Fault;
      private string nitrogenSensor2Fault;
      private string robotTotalCurrentFault;
      private string launchTotalCurrentFault;
      private string frontPressureFault;
      private string rearPressureFault;

      private double nitrogenPressureReading1;
      private double nitrogenPressureReading2;
      private double robotTotalCurrentReading;
      private double launchTotalCurrentReading;
      private double frontPumpSpeedReading;
      private double rearPumpSpeedReading;
      private double frontPumpPressureReading;
      private double rearPumpPressureReading;
      
      private ReelModes reelModeSetPoint;
      private ReelModes reelNonManualModeSetPoint;
      private ElmoWhistleMotor.ControlModes reelRequestedControlMode;
      private ElmoWhistleMotor.Modes reelRequestedMode;
      private double reelManualCurrentSetPoint;
      private double reelManualSpeedSetPoint;
      private double reelRequestedCurrent;
      private double reelRequestedSpeed;
      private double reelTotalStart;
      private double reelTripStart;
      private double reelCalibrationStart;
      private bool reelCalibrationEnabled; 

      private FeederModes feederModeSetPoint; // applicable to 4 motors
      private double feederVelocitySetPoint; // applicable to 4 motors
      private bool evaluateFeederParameters; // forces evaluation of set points
      private DateTime feederTraceTimeLimit; // limits log output 

      private FeederMotorStatus feederTopFrontStatus;
      private FeederMotorStatus feederTopRearStatus;
      private FeederMotorStatus feederBottomFrontStatus;
      private FeederMotorStatus feederBottomRearStatus;

      private GuideMotorStatus guideLeftStatus;
      private GuideMotorStatus guideRightStatus;

      private CameraLocations selectedLaunchCamera;
      private int[] launchCameraLightIntensities;
      private int[] launchCameraLightIntensityRequests;

      private byte reelDigitalOutRequested;
      private byte launchDigitalOutRequested;

      //private bool feederClampInitialized;
      //private bool feederClampAdjusting;
      //private bool feederClampSetPoint;
      //private DateTime feederClampSetPointTimeLimit;

      //private bool feederClampActual;

      private ElmoWhistleMotor.Modes frontPumpRequestedMode;
      private double frontPumpRequestedSpeed;
      private ElmoWhistleMotor.Modes rearPumpRequestedMode;
      private double rearPumpRequestedSpeed;

      #endregion

      #region Helper Functions

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

      public static TruckCommBus Instance
      {
         get
         {
            if (instance == null)
            {
               instance = new TruckCommBus();
               instance.Initialize();
            }

            return instance;
         }
      }

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

      private void DeviceError(string name, int nodeId, string reason)
      {
         Tracer.WriteError(TraceGroup.TBUS, "", "fault with \"{0}\", node={1}, reason={2}", name, nodeId, reason);

         if (nodeId == ParameterAccessor.Instance.TruckBus.ReelAnalogBusId)
         {
            this.nitrogenSensor1Fault = "interface not ready";
            this.nitrogenSensor2Fault = "interface not ready";
            this.robotTotalCurrentFault = "interface not ready";
            this.launchTotalCurrentFault = "interface not ready"; 
            this.frontPressureFault = "interface not ready";
            this.rearPressureFault = "interface not ready";
         }
      }

      private void ElmoMotorInputChangeHandler(int nodeId, byte inputValue)
      {
         if (ParameterAccessor.Instance.TruckBus.GuideLeftMotorBusId == nodeId)
         {
            this.guideLeftStatus.RetractionLimit = ((inputValue & 0x10) == 0) ? false : true;
            this.guideLeftStatus.ExtensionLimit = ((inputValue & 0x20) == 0) ? false : true;
         }
         else if (ParameterAccessor.Instance.TruckBus.GuideRightMotorBusId == nodeId)
         {
            this.guideRightStatus.RetractionLimit = ((inputValue & 0x10) == 0) ? false : true;
            this.guideRightStatus.ExtensionLimit = ((inputValue & 0x20) == 0) ? false : true;
         }
      }

      private void DigitalIoInputChangeHandler(int nodeId, byte inputValue)
      {
#if false
         PeakDigitalIo device = null;

         if (ParameterAccessor.Instance.TruckBus.ReelDigitalBusId == nodeId)
         {
            device = this.reelDigitalIo;
            this.mainAirOn = ((inputValue & 0x10) != 0) ? true : false;
            
         }
         else if (ParameterAccessor.Instance.TruckBus.LaunchDigitalIoBusId == nodeId)
         {
            device = this.launchDigitalIo;
            this.feederClampActual = ((inputValue & 0x01) != 0) ? true : false;

            if (false == this.feederClampInitialized)
            {
               this.feederClampSetPoint = !this.feederClampActual;
               this.feederClampInitialized = true;
            }
            else if (false == this.feederClampAdjusting)
            {
               this.feederClampSetPoint = !this.feederClampActual;
            }
            else if (this.feederClampActual != this.feederClampSetPoint)
            {
               this.feederClampAdjusting = false;
            }
         }

         if (null != device)
         {
            Tracer.WriteMedium(TraceGroup.TBUS, "", "{0} input {1:X2}", device.Name, inputValue);
         }
#endif
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
               bool handled = false;
               
               foreach (Device device in this.deviceList)
               {
                  handled |= device.Update((int)frame.cobId, frame.data);
               }

               if (false == handled)
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

         for (; this.execute; )
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

      #region Reel Functions

      #region Reel Motor Functions

      private void InitializeReelMotor()
      {
         this.reelMotor.Initialize();
         this.reelRequestedControlMode = ElmoWhistleMotor.ControlModes.singleLoopPosition;
         this.reelRequestedMode = ElmoWhistleMotor.Modes.undefined;
         this.reelRequestedCurrent = 0;
         this.reelRequestedSpeed = 0;
      }

      private void StartReelMotor()
      {
         if (null == this.reelMotor.FaultReason)
         {
            this.reelMotor.SetConsumerHeartbeat((UInt16)ParameterAccessor.Instance.TruckBus.ConsumerHeartbeatRate, (byte)ParameterAccessor.Instance.TruckBus.ControllerBusId);
            this.reelMotor.SetProducerHeartbeat((UInt16)ParameterAccessor.Instance.TruckBus.ProducerHeartbeatRate);
            this.reelMotor.Start();

            Thread.Sleep(100); // need delay beteen start and current mode
            this.reelMotor.SetMode(ElmoWhistleMotor.Modes.current);

            Tracer.WriteMedium(TraceGroup.TBUS, null, "{0} started", this.reelMotor.Name);
         }
      }

      private void UpdateReelMotor()
      {
         if (null == this.reelMotor.FaultReason)
         {
            ElmoWhistleMotor.ControlModes neededControlMode = ElmoWhistleMotor.ControlModes.singleLoopPosition;
            ElmoWhistleMotor.Modes neededMode = ElmoWhistleMotor.Modes.current;
            double neededValue = 0;
            bool modeChange = false;

            if (ReelModes.off == this.reelModeSetPoint)
            {
               neededControlMode = ElmoWhistleMotor.ControlModes.singleLoopPosition;
               neededMode = ElmoWhistleMotor.Modes.off;
               neededValue = 0;
            }
            else if (ReelModes.reverse == this.reelModeSetPoint)
            {
               if (MovementForwardControls.current == ParameterAccessor.Instance.ReelMotionMode)
               {
                  neededControlMode = ElmoWhistleMotor.ControlModes.singleLoopPosition;
                  neededMode = ElmoWhistleMotor.Modes.current;
                  neededValue = ParameterAccessor.Instance.ReelReverseCurrent.OperationalValue;
               }
               else
               {
                  neededControlMode = ElmoWhistleMotor.ControlModes.singleLoopPosition;
                  neededMode = ElmoWhistleMotor.Modes.velocity;
                  neededValue = ParameterAccessor.Instance.ReelReverseSpeed.OperationalValue;
               }
            }
            else if (ReelModes.locked == this.reelModeSetPoint)
            {
               neededControlMode = ElmoWhistleMotor.ControlModes.microStepper;
               neededValue = ParameterAccessor.Instance.ReelLockCurrent.OperationalValue;
            }
            else if (ReelModes.manual == this.reelModeSetPoint)
            {
               if (MovementForwardControls.current == ParameterAccessor.Instance.ReelMotionMode)
               {
                  neededControlMode = ElmoWhistleMotor.ControlModes.singleLoopPosition;
                  neededMode = ElmoWhistleMotor.Modes.current;
                  neededValue = reelManualCurrentSetPoint * -1;
               }
               else
               {
                  neededControlMode = ElmoWhistleMotor.ControlModes.singleLoopPosition;
                  neededMode = ElmoWhistleMotor.Modes.velocity;
                  neededValue = reelManualSpeedSetPoint * -1;
               }
            }

            if ((neededControlMode != this.reelRequestedControlMode) ||
                (neededMode != this.reelRequestedMode))
            {
               bool requestedZero = false;
               bool atZero = false;

               if (((ElmoWhistleMotor.ControlModes.microStepper == this.reelRequestedControlMode) && (0 == this.reelRequestedCurrent)) ||
                   ((ElmoWhistleMotor.ControlModes.singleLoopPosition == this.reelRequestedControlMode) && (ElmoWhistleMotor.Modes.undefined == this.reelRequestedMode)) ||
                   ((ElmoWhistleMotor.ControlModes.singleLoopPosition == this.reelRequestedControlMode) && (ElmoWhistleMotor.Modes.off == this.reelRequestedMode)) ||
                   ((ElmoWhistleMotor.ControlModes.singleLoopPosition == this.reelRequestedControlMode) && (ElmoWhistleMotor.Modes.current == this.reelRequestedMode) && (0 == this.reelRequestedCurrent)) ||
                   ((ElmoWhistleMotor.ControlModes.singleLoopPosition == this.reelRequestedControlMode) && (ElmoWhistleMotor.Modes.velocity == this.reelRequestedMode) && (0 == this.reelRequestedSpeed)))
               {
                  requestedZero = true;
               }

               if (((ElmoWhistleMotor.ControlModes.microStepper == this.reelRequestedControlMode) && (0 == this.reelMotor.Torque)) ||
                   ((ElmoWhistleMotor.ControlModes.singleLoopPosition == this.reelRequestedControlMode) && (ElmoWhistleMotor.Modes.undefined == this.reelRequestedMode)) ||
                   ((ElmoWhistleMotor.ControlModes.singleLoopPosition == this.reelRequestedControlMode) && (ElmoWhistleMotor.Modes.off == this.reelRequestedMode)) ||
                   ((ElmoWhistleMotor.ControlModes.singleLoopPosition == this.reelRequestedControlMode) && (ElmoWhistleMotor.Modes.current == this.reelRequestedMode) && (0 == this.reelMotor.Torque)) ||
                   ((ElmoWhistleMotor.ControlModes.singleLoopPosition == this.reelRequestedControlMode) && (ElmoWhistleMotor.Modes.velocity == this.reelRequestedMode) && (0 == this.reelMotor.RPM)))
               {
                  atZero = true;
               }

               if ((false == requestedZero) || (false == atZero))
               {
                  neededValue = 0;
               }
               else
               {
                  this.reelMotor.SetControlMode(neededControlMode);
                  this.reelRequestedControlMode = neededControlMode;

                  if (ElmoWhistleMotor.ControlModes.singleLoopPosition == this.reelRequestedControlMode)
                  {
                     this.reelMotor.SetMode(neededMode);
                     this.reelRequestedMode = neededMode;
                  }
                  else
                  {
                     // mode assignment not needed for Micro Stepper mode
                     this.reelRequestedMode = neededMode;
                  }

                  Tracer.WriteMedium(TraceGroup.TBUS, null, "{0} control={1} mode={2}", this.reelMotor.Name, this.reelRequestedControlMode, this.reelRequestedMode);
                  modeChange = true;
               }
            }

            if (((neededControlMode == this.reelRequestedControlMode) && (neededMode == this.reelRequestedMode)) ||
                (0 == neededValue))
            {
               if (ElmoWhistleMotor.ControlModes.singleLoopPosition == this.reelRequestedControlMode)
               {
                  if (ElmoWhistleMotor.Modes.current == this.reelRequestedMode)
                  {
                     if ((neededValue != this.reelRequestedCurrent) || (false != modeChange))
                     {
                        float torqueCurrent = (float)neededValue;
                        this.reelMotor.SetTorque(torqueCurrent);
                        this.reelRequestedCurrent = neededValue;
                        Tracer.WriteMedium(TraceGroup.TBUS, null, "{0} current {1}", this.reelMotor.Name, neededValue);
                     }
                  }
                  else if (ElmoWhistleMotor.Modes.velocity == this.reelRequestedMode)
                  {
                     if ((neededValue != this.reelRequestedSpeed) || (false != modeChange))
                     {
                        int velocity = (int)neededValue;
                        this.reelMotor.SetVelocity(velocity);
                        this.reelRequestedSpeed = neededValue;
                        Tracer.WriteMedium(TraceGroup.TBUS, null, "{0} speed {1}", this.reelMotor.Name, neededValue);
                     }
                  }
               }
               else if (ElmoWhistleMotor.ControlModes.microStepper == this.reelRequestedControlMode)
               {
                  if ((neededValue != this.reelRequestedCurrent) || (false != modeChange))
                  {
                     float torqueCurrent = (float)neededValue;
                     this.reelMotor.SetStepperCurrent(torqueCurrent);
                     this.reelRequestedCurrent = neededValue;
                     Tracer.WriteMedium(TraceGroup.TBUS, null, "{0} lock current {1}", this.reelMotor.Name, neededValue);
                  }
               }
            }
         }
      }

      #endregion

      #region Reel Digital IO Functions

      private void InitilizeReelDigitalIo()
      {
         this.reelDigitalIo.Initialize();
         this.reelDigitalOutRequested = 0;
      }

      private void StartReelDigitalIo()
      {
         this.reelDigitalIo.Configure();
         this.reelDigitalIo.Start();
      }

      private void UpdateReelDigitalIo()
      {
         byte neededOutValue = 0;

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
#if false
            if (PumpControl.Front.GetOnState() != false)
            {
               if (PumpControl.Front.GetDirection() == PumpDirections.reverse)
               {
                  neededOutValue |= 0x01;
               }

               neededOutValue |= 0x02;
            }

            if (PumpControl.Rear.GetOnState() != false)
            {
               if (PumpControl.Rear.GetDirection() == PumpDirections.reverse)
               {
                  neededOutValue |= 0x04;
               }

               neededOutValue |= 0x08;
            }
#endif
         }

         if (neededOutValue != this.reelDigitalOutRequested)
         {
            this.reelDigitalIo.SetOutput(neededOutValue);
            this.reelDigitalOutRequested = neededOutValue;
            Tracer.WriteMedium(TraceGroup.TBUS, null, "reel digital out {0:X2}", neededOutValue);
         }
      }

      #endregion

      #region Reel Analog IO Functions

      private void InitilizeReelAnalogIo()
      {
         this.reelAnalogIo.Initialize();
         this.nitrogenPressureReading1 = double.NaN;
         this.nitrogenPressureReading2 = double.NaN;
         this.robotTotalCurrentReading = double.NaN;
         this.launchTotalCurrentReading = double.NaN;
         this.frontPumpPressureReading = double.NaN;
         this.rearPumpPressureReading = double.NaN;
      }

      private void StartReelAnalogIo()
      {
         this.reelAnalogIo.Configure();
         this.reelAnalogIo.Start();
      }

      private void UpdateReelAnalogIo()
      {
         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
#if false
            if (false == SimulatePumpSpeed)
            {
               double frontPumpSpeedVoltage = ((double)this.reelAnalogIo.AnalogIn0 * 10) / 4095;
               this.frontPumpSpeedReading = frontPumpSpeedVoltage * ParameterAccessor.Instance.FrontPump.RpmPerVolt;

               double rearPumpSpeedVoltage = ((double)this.reelAnalogIo.AnalogIn1 * 10) / 4095;
               this.rearPumpSpeedReading = rearPumpSpeedVoltage * ParameterAccessor.Instance.FrontPump.RpmPerVolt;
            }
            else
            {
               this.frontPumpSpeedReading = SimulatedPumpSpeed.Front.GetSpeed(PumpControl.Front.GetOnState(), PumpControl.Front.GetSpeedSetting(), PumpControl.Front.GetDirection());
               this.rearPumpSpeedReading = SimulatedPumpSpeed.Rear.GetSpeed(PumpControl.Rear.GetOnState(), PumpControl.Rear.GetSpeedSetting(), PumpControl.Rear.GetDirection());
            }
#endif

#if false
            if (false == SimulatePumpPressure)
            {
               double frontPumpPressureVoltage = ((double)this.reelAnalogIo.AnalogIn3 * 10) / 4095;
               this.frontPumpPressureReading = frontPumpPressureVoltage * ParameterAccessor.Instance.FrontPump.PsiPerVolt;

               double rearPumpPressureVoltage = ((double)this.reelAnalogIo.AnalogIn4 * 10) / 4095;
               this.rearPumpPressureReading = rearPumpPressureVoltage * ParameterAccessor.Instance.FrontPump.PsiPerVolt;
            }
            else
            {
               this.frontPumpPressureReading = SimulatedPumpPressure.Front.GetPressure(PumpControl.Front.GetMeasuredVolume());
               this.rearPumpPressureReading = SimulatedPumpPressure.Rear.GetPressure(PumpControl.Rear.GetMeasuredVolume());
            }
#endif
            double frontPumpPressureVoltage = ((double)this.reelAnalogIo.AnalogIn2 * 10) / 32767;

            if (frontPumpPressureVoltage >= 2.0)
            {
               this.frontPumpPressureReading = (frontPumpPressureVoltage - 2) * ParameterAccessor.Instance.FrontPump.PsiPerVolt;
               this.frontPressureFault = null;
            }
            else if (frontPumpPressureVoltage >= 1.5)
            {
               this.frontPumpPressureReading = 0;
               this.frontPressureFault = null;
            }
            else
            {
               this.frontPumpPressureReading = double.NaN;
               this.frontPressureFault = "disconnected";
            }

            double rearPumpPressureVoltage = ((double)this.reelAnalogIo.AnalogIn3 * 10) / 32767;

            if (rearPumpPressureVoltage >= 2.0)
            {
               this.rearPumpPressureReading = (rearPumpPressureVoltage - 2) * ParameterAccessor.Instance.RearPump.PsiPerVolt;
               this.rearPressureFault = null;
            }
            else if (rearPumpPressureVoltage >= 1.5)
            {
               this.rearPumpPressureReading = 0;
               this.rearPressureFault = null;
            }
            else
            {
               this.rearPumpPressureReading = double.NaN;
               this.rearPressureFault = "disconnected";
            }
         }

         double nitrogenPressure1Voltage = ((double)this.reelAnalogIo.AnalogIn0 * 10) / 32767;

         if (nitrogenPressure1Voltage >= 2.0)
         {
            this.nitrogenPressureReading1 = (nitrogenPressure1Voltage - 2) * ParameterAccessor.Instance.NitrogenPressureConversionUnit.OperationalValue;
            this.nitrogenSensor1Fault = null;
         }
         else
         {
            this.nitrogenPressureReading1 = double.NaN;
            this.nitrogenSensor1Fault = "disconnected";
         }

         double nitrogenPressure2Voltage = ((double)this.reelAnalogIo.AnalogIn1 * 10) / 32767;

         if (nitrogenPressure2Voltage >= 2.0)
         {
            this.nitrogenPressureReading2 = (nitrogenPressure2Voltage - 2) * ParameterAccessor.Instance.NitrogenPressureConversionUnit.OperationalValue;
            this.nitrogenSensor2Fault = null;
         }
         else
         {
            this.nitrogenPressureReading2 = double.NaN;
            this.nitrogenSensor2Fault = "disconnected";
         }

         double robotTotalCurrentVoltage = ((double)this.reelAnalogIo.AnalogIn4 * 10) / 32767;

         if (robotTotalCurrentVoltage >= 2.0)
         {
            this.robotTotalCurrentReading = (robotTotalCurrentVoltage - 2) * ParameterAccessor.Instance.RobotTotalCurrentConversionUnit.OperationalValue;
            this.robotTotalCurrentFault = null;
         }
         else
         {
            this.robotTotalCurrentReading = double.NaN;
            this.robotTotalCurrentFault = "disconnected";
         }

         double launchTotalCurrentVoltage = ((double)this.reelAnalogIo.AnalogIn5 * 10) / 32767;

         if (robotTotalCurrentVoltage >= 2.0)
         {
            this.launchTotalCurrentReading = (launchTotalCurrentVoltage - 2) * ParameterAccessor.Instance.LaunchTotalCurrentConversionUnit.OperationalValue;
            this.launchTotalCurrentFault = null;
         }
         else
         {
            this.robotTotalCurrentReading = double.NaN;
            this.launchTotalCurrentFault = "disconnected";
         }
      }

      #endregion

      #region Reel Encoder Functions
      
      private void InitializeReelEncoder()
      {
         this.reelEncoder.Initialize();
         this.reelCalibrationStart = 0;
         this.reelCalibrationEnabled = false;
      }

      private void StartReelEncoder()
      {
         this.reelEncoder.Start();
      }

      private void UpdateReelEncoder()
      {
      }

      #endregion

      private void InitializeReel()
      {
         this.InitializeReelMotor();
         this.InitilizeReelDigitalIo();
         this.InitilizeReelAnalogIo();
         this.InitializeReelEncoder();
      }

      private void StartReel()
      {
         this.StartReelMotor();
         this.StartReelDigitalIo();
         this.StartReelAnalogIo();
         this.StartReelEncoder();
      }

      private void UpdateReel()
      {
         this.UpdateReelMotor();
         this.UpdateReelDigitalIo();
         this.UpdateReelAnalogIo();
         this.UpdateReelEncoder();
      }

      #endregion

      #region Feeder Functions

      private void StartFeederMotor(ElmoWhistleMotor motor)
      {
         if (null == motor.FaultReason)
         {
            motor.SetConsumerHeartbeat((UInt16)ParameterAccessor.Instance.TruckBus.ConsumerHeartbeatRate, (byte)ParameterAccessor.Instance.TruckBus.ControllerBusId);
            motor.SetProducerHeartbeat((UInt16)ParameterAccessor.Instance.TruckBus.ProducerHeartbeatRate);
            motor.Start();

            Tracer.WriteMedium(TraceGroup.TBUS, null, "{0} started", motor.Name);
         }
      }

      private void InitializeFeeder()
      {
         this.evaluateFeederParameters = false;

         this.feederTopFrontMotor.Initialize();
         this.feederTopFrontStatus.Initialize();

         this.feederTopRearMotor.Initialize();
         this.feederTopRearStatus.Initialize();

         this.feederBottomFrontMotor.Initialize();
         this.feederBottomFrontStatus.Initialize();

         this.feederBottomRearMotor.Initialize();
         this.feederBottomRearStatus.Initialize();
      }

      private void StartFeeder()
      {
         this.StartFeederMotor(this.feederTopFrontMotor);
         this.StartFeederMotor(this.feederTopRearMotor);
         this.StartFeederMotor(this.feederBottomFrontMotor);
         this.StartFeederMotor(this.feederBottomRearMotor);
      }

      private bool UpdateFeederMotor(ElmoWhistleMotor motor, FeederMotorStatus status, FeederMotorParameters parameters)
      {
         bool scheduled = false;

         if (null == motor.FaultReason)
         {
            bool modeChange = false;
            ElmoWhistleMotor.Modes neededMode = ElmoWhistleMotor.Modes.undefined;
            double neededValue = 0;

            if (MotorStates.Disabled == parameters.State)
            {
               if (0 == status.requestedVelocity)
               {
                  neededMode = ElmoWhistleMotor.Modes.off;
                  neededValue = 0;
               }
               else
               {
                  neededMode = ElmoWhistleMotor.Modes.velocity;
                  neededValue = 0;
               }
            }
            else if (MotorStates.Enabled == parameters.State)
            {
               if (FeederModes.off == this.feederModeSetPoint)
               {
                  if (0 == status.requestedVelocity)
                  {
                     neededMode = ElmoWhistleMotor.Modes.off;
                     neededValue = 0;
                  }
                  else
                  {
                     neededMode = ElmoWhistleMotor.Modes.velocity;
                     neededValue = 0;
                  }
               }
               else if (FeederModes.move == this.feederModeSetPoint)
               {
                  bool velocityMode = true;

                  if (((false != parameters.PositivePusher) && (this.feederVelocitySetPoint < 0)) ||
                      ((false == parameters.PositivePusher) && (this.feederVelocitySetPoint > 0)))
                  {
                     velocityMode = false;
                  }

                  if (false != velocityMode)
                  {
                     neededMode = ElmoWhistleMotor.Modes.velocity;
                     neededValue = this.feederVelocitySetPoint;
                  }
                  else
                  {
                     double neededCurrent = ParameterAccessor.Instance.FeederCurrentPer1kRPM.OperationalValue * this.feederVelocitySetPoint * ParameterAccessor.Instance.FeederVelocityToRpm / 1000;
                     neededMode = ElmoWhistleMotor.Modes.current;
                     neededValue = neededCurrent;
                  }
               }
               else if (FeederModes.locked == this.feederModeSetPoint)
               {
                  neededMode = ElmoWhistleMotor.Modes.current;
                  neededValue = ParameterAccessor.Instance.FeederLockCurrent.OperationalValue;
               }
            }
            else if (MotorStates.Locked == parameters.State)
            {
               neededMode = ElmoWhistleMotor.Modes.current;
               neededValue = ParameterAccessor.Instance.FeederLockCurrent.OperationalValue;
            }

            if (neededMode != status.requestedMode)
            {
               bool requestedZero = false;
               bool atZero = false;

               if ((ElmoWhistleMotor.Modes.undefined == status.requestedMode) ||
                   (ElmoWhistleMotor.Modes.off == status.requestedMode) ||
                   ((ElmoWhistleMotor.Modes.velocity == status.requestedMode) && (0 == status.requestedVelocity)) ||
                   ((ElmoWhistleMotor.Modes.current == status.requestedMode) && (0 == status.requestedCurrent)))
               {
                  requestedZero = true;
               }

               if ((ElmoWhistleMotor.Modes.undefined == status.requestedMode) ||
                   (ElmoWhistleMotor.Modes.off == status.requestedMode) ||
                   ((ElmoWhistleMotor.Modes.velocity == status.requestedMode) && (0 == motor.RPM)) ||
                   ((ElmoWhistleMotor.Modes.current == status.requestedMode) && (0 == motor.Torque)))
               {
                  atZero = true;
               }

               if ((false == requestedZero) || (false == atZero))
               {
                  neededValue = 0;
               }
               else
               {
                  motor.SetMode(neededMode);

                  status.requestedMode = neededMode;
                  modeChange = true;
                  Tracer.WriteMedium(TraceGroup.TBUS, null, "{0} {1}", motor.Name, neededMode.ToString());
               }
            }

            if (ElmoWhistleMotor.Modes.velocity == status.requestedMode)
            {
               if ((neededValue != status.requestedVelocity) || (false != modeChange) || (false != this.evaluateFeederParameters))
               {
                  int settingInversionValue = (MotorDirections.Normal == parameters.Direction) ? 1 : -1;
                  int positionInversionValue = (false == parameters.PositionInversion) ? 1 : -1;
                  int velocityRpm = (int)(settingInversionValue * positionInversionValue * neededValue * ParameterAccessor.Instance.FeederVelocityToRpm);
                  motor.ScheduleVelocity(velocityRpm);
                  scheduled = true;

                  status.requestedVelocity = neededValue;
                  Tracer.WriteMedium(TraceGroup.TBUS, null, "{0} velocity {1:0.00} {2}", motor.Name, neededValue, velocityRpm);
               }
            }
            else if (ElmoWhistleMotor.Modes.current == status.requestedMode)
            {
               if ((neededValue != status.requestedCurrent) || (false != modeChange) || (false != this.evaluateFeederParameters))
               {
                  int settingInversionValue = (MotorDirections.Normal == parameters.Direction) ? 1 : -1;
                  int positionInversionValue = (false == parameters.PositionInversion) ? 1 : -1;
                  float torqueCurrent = (float)(settingInversionValue * positionInversionValue * neededValue);
                  motor.ScheduleTorque(torqueCurrent);
                  scheduled = true;

                  status.requestedCurrent = neededValue;
                  Tracer.WriteMedium(TraceGroup.TBUS, null, "{0} current {1:0.000} {2:0.000}", motor.Name, neededValue, torqueCurrent);
               }
            }
            else if (ElmoWhistleMotor.Modes.off == status.requestedMode)
            {
               if (false != modeChange)
               {
                  motor.ScheduleVelocity(0);
                  scheduled = true;

                  status.requestedVelocity = 0;
                  Tracer.WriteMedium(TraceGroup.RBUS, null, "{0} velocity=0 rpm=0", motor.Name, 0, 0);
               }
            }
         }

         return (scheduled);
      }

      private void UpdateFeeder()
      {
         // actual:
         //    positive = CCW, negative = CW

         // expected:
         //    TF normal = CW, inverse = CCW
         //    TR normal = CCW, inverse = CW
         //    BF normal = CW, inverse = CCW
         //    BR normal = CCW, inverse = CW
         bool scheduled = false;

         scheduled |= this.UpdateFeederMotor(this.feederTopFrontMotor, this.feederTopFrontStatus, ParameterAccessor.Instance.TopFrontFeederMotor);
         scheduled |= this.UpdateFeederMotor(this.feederTopRearMotor, this.feederTopRearStatus, ParameterAccessor.Instance.TopRearFeederMotor);
         scheduled |= this.UpdateFeederMotor(this.feederBottomFrontMotor, this.feederBottomFrontStatus, ParameterAccessor.Instance.BottomFrontFeederMotor);
         scheduled |= this.UpdateFeederMotor(this.feederBottomRearMotor, this.feederBottomRearStatus, ParameterAccessor.Instance.BottomRearFeederMotor);

         if (false != scheduled)
         {
            PCANLight.SendSync(this.busInterfaceId);
         }

         this.evaluateFeederParameters = false;
      }

      private void EvaluateFeederMotorVelocity(ElmoWhistleMotor motor, FeederMotorStatus status, FeederMotorParameters parameters, ref double total, ref int count)
      {
         if ((null == motor.FaultReason) &&
             (MotorStates.Enabled == parameters.State))
             //(FeederModes.move == this.feederModeSetPoint))
         {
            double motorContribution = motor.RPM;

            if (DateTime.Now > this.feederTraceTimeLimit)
            {
               Tracer.WriteHigh(TraceGroup.TBUS, "", "{0} motor, velocity={1}, torque={2}", motor.Name, motor.RPM, motor.Torque);
               this.feederTraceTimeLimit = DateTime.Now.AddMilliseconds(1000);
            }

            if (status.requestedMode == ElmoWhistleMotor.Modes.current) 
            {
               motorContribution = motor.Torque * 1000 / ParameterAccessor.Instance.FeederCurrentPer1kRPM.OperationalValue;
            }

            int settingInversionValue = (MotorDirections.Normal == parameters.Direction) ? 1 : -1;
            int positionInversionValue = (false == parameters.PositionInversion) ? 1 : -1;
            total += (motorContribution * settingInversionValue * positionInversionValue) / ParameterAccessor.Instance.FeederVelocityToRpm;

            count++;
         }
      }

      #endregion

      #region Guide Motor Functions

      private void StartGuideMotor(ElmoWhistleMotor motor)
      {
         if (null == motor.FaultReason)
         {
            motor.OnInputChange = new ElmoWhistleMotor.InputChangeHandler(this.ElmoMotorInputChangeHandler);

            motor.SetConsumerHeartbeat((UInt16)ParameterAccessor.Instance.TruckBus.ConsumerHeartbeatRate, (byte)ParameterAccessor.Instance.TruckBus.ControllerBusId);
            motor.SetProducerHeartbeat((UInt16)ParameterAccessor.Instance.TruckBus.ProducerHeartbeatRate);
            motor.Start();

            motor.SetMode(ElmoWhistleMotor.Modes.velocity);

            Tracer.WriteMedium(TraceGroup.TBUS, null, "{0} started", motor.Name);
         }
      }

      private void InitializeGuide()
      {
         this.guideLeftMotor.Initialize();
         this.guideLeftStatus.Initialize();

         this.guideRightMotor.Initialize();
         this.guideRightStatus.Initialize();
      }

      private void StartGuide()
      {
         this.StartGuideMotor(this.guideLeftMotor);
         this.StartGuideMotor(this.guideRightMotor);
      }

      private void UpdateGuideMotor(ElmoWhistleMotor motor, GuideMotorStatus status)
      {
         if (null == motor.FaultReason)
         {
            double neededValue = 0;

            if (GuideDirections.retract == status.direction)
            {
               if (status.RetractionLimit == false)
               {
                  neededValue = ParameterAccessor.Instance.GuideRetractionSpeed.OperationalValue;
               }
               else
               {
                  status.direction = GuideDirections.off;
                  neededValue = 0;
               }
            }
            else if (GuideDirections.extend == status.direction)
            {
               if (status.ExtensionLimit == false)
               {
                  neededValue = ParameterAccessor.Instance.GuideExtensionSpeed.OperationalValue * -1;
               }
               else
               {
                  status.direction = GuideDirections.off;
                  neededValue = 0;
               }
            }
            else
            {
               neededValue = 0;
            }

            if (status.requestedVelocity != neededValue)
            {
               int velocityRpm = (int)(neededValue);
               motor.SetVelocity(velocityRpm);
               status.requestedVelocity = neededValue;
               Tracer.WriteMedium(TraceGroup.TBUS, null, "{0} velocity {1}", motor.Name, neededValue);
            }
         }
      }

      private void UpdateGuide()
      {
         this.UpdateGuideMotor(this.guideLeftMotor, this.guideLeftStatus);
         this.UpdateGuideMotor(this.guideRightMotor, this.guideRightStatus);
      }

      #endregion

      #region Launch Tube Functions

      #region Launch Tube Digital IO Functions

      private void InitilizeLaunchTubeDigitalIo()
      {
         this.launchDigitalIo.Initialize();

         this.selectedLaunchCamera = CameraLocations.launchLeftGuide; // launch digital IO
         this.launchDigitalOutRequested = 0;
      }

      private void StartLaunchTubeDigitalIo()
      {
         this.launchDigitalIo.Configure();
         this.launchDigitalIo.Start();
      }

      #endregion

      #region Launch Tube Analog IO Functions

      private void InitilizeLaunchTubeAnalogIo()
      {
         this.launchAnalogIo.Initialize();

         for (int i = 0; i < this.launchCameraLightIntensityRequests.Length; i++)
         {
            this.launchCameraLightIntensityRequests[i] = -1;
         }
      }

      private void StartLaunchTubeAnalogIo()
      {
         this.launchAnalogIo.Configure();
         this.launchAnalogIo.Start();
      }

      #endregion

      private void InitializeLaunchTube()
      {
         this.InitilizeLaunchTubeDigitalIo();
         this.InitilizeLaunchTubeAnalogIo();
      }

      private void StartLaunchTube()
      {
         this.StartLaunchTubeDigitalIo();
         this.StartLaunchTubeAnalogIo();
      }

      private void UpdateLaunchTube()
      {
         #region Digital IO Update

         byte neededOutValue = 0;

         if (CameraLocations.launchRightGuide == this.selectedLaunchCamera)
         {
            neededOutValue |= 0x02;
         }
         else if (CameraLocations.launchFeeder == this.selectedLaunchCamera)
         {
            neededOutValue |= 0x01;
         }
         else if (CameraLocations.launchMain == this.selectedLaunchCamera)
         {
            neededOutValue |= 0x03;
         }

         if (neededOutValue != this.launchDigitalOutRequested)
         {
            this.launchDigitalIo.SetOutput(neededOutValue);
            this.launchDigitalOutRequested = neededOutValue;
            Tracer.WriteMedium(TraceGroup.TBUS, null, "launch digital out {0:X2}", neededOutValue);
         }

         #endregion

         #region Analog IO Update

         for (int i = 0; i < this.launchCameraLightIntensities.Length; i++)
         {
            if (this.launchCameraLightIntensities[i] != this.launchCameraLightIntensityRequests[i])
            {
               UInt16 setPoint = (UInt16)(this.launchCameraLightIntensities[i] * 4095 / 100);
               this.launchAnalogIo.SetOutput(i, setPoint);
               this.launchCameraLightIntensityRequests[i] = this.launchCameraLightIntensities[i];
               Tracer.WriteMedium(TraceGroup.TBUS, null, "camera {0} light set to {1}", i + 1, setPoint);
            }
         }

         #endregion
      }

      #endregion

      #region GPS Functions

      private void InitializeGps()
      {
         this.gps.Initialize();
      }

      private void StartGps()
      {
         this.gps.Start();
      }

      #endregion

      #region Pump Functions

      #region Front Pump Functions

      private void InitializeFrontPump()
      {
         this.frontPumpSpeedReading = 0;
         PumpControl.Front.Initialize();
         this.frontPumpRequestedMode = ElmoWhistleMotor.Modes.off;
         this.frontPumpRequestedSpeed = 0;
      }

      private void StartFrontPump()
      {
         this.frontPumpMotor.SetConsumerHeartbeat((UInt16)ParameterAccessor.Instance.TruckBus.ConsumerHeartbeatRate, (byte)ParameterAccessor.Instance.TruckBus.ControllerBusId);
         this.frontPumpMotor.SetProducerHeartbeat((UInt16)ParameterAccessor.Instance.TruckBus.ProducerHeartbeatRate);
         this.frontPumpMotor.Start();
      }

      #endregion

      #region Rear Pump Functions

      private void InitializeRearPump()
      {
         this.rearPumpSpeedReading = 0;
         PumpControl.Rear.Initialize();
         this.rearPumpRequestedMode = ElmoWhistleMotor.Modes.off;
         this.rearPumpRequestedSpeed = 0;
      }

      private void StartRearPump()
      {
         this.rearPumpMotor.SetConsumerHeartbeat((UInt16)ParameterAccessor.Instance.TruckBus.ConsumerHeartbeatRate, (byte)ParameterAccessor.Instance.TruckBus.ControllerBusId);
         this.rearPumpMotor.SetProducerHeartbeat((UInt16)ParameterAccessor.Instance.TruckBus.ProducerHeartbeatRate);
         this.rearPumpMotor.Start();
      }

      #endregion

      private void InitializePumps()
      {
         this.InitializeFrontPump();
         this.InitializeRearPump();
      }

      private void StartPumps()
      {
         this.StartFrontPump();
         this.StartRearPump();
      }

      private void UpdatePumpMotor(ElmoWhistleMotor motor, double setPoint, ref ElmoWhistleMotor.Modes requestedMode, ref double requestedSetPoint)
      {
         if (null == motor.FaultReason)
         {
            bool modeChange = false;
            ElmoWhistleMotor.Modes neededMode = ElmoWhistleMotor.Modes.undefined;
            double neededValue = 0;

            if (0 == setPoint)
            {
               if (0 == requestedSetPoint)
               {
                  neededMode = ElmoWhistleMotor.Modes.off;
                  neededValue = 0;
               }
               else
               {
                  neededMode = ElmoWhistleMotor.Modes.velocity;
                  neededValue = 0;
               }
            }
            else
            {
               neededMode = ElmoWhistleMotor.Modes.velocity;
               neededValue = setPoint;
            }

            if (neededMode != requestedMode)
            {
               motor.SetMode(neededMode);
               requestedMode = neededMode;
               modeChange = true;
               Tracer.WriteMedium(TraceGroup.TBUS, null, "{0} {1}", motor.Name, requestedMode.ToString());
            }

            if (ElmoWhistleMotor.Modes.velocity == requestedMode)
            {
               if ((neededValue != requestedSetPoint) || (false != modeChange))
               {
                  motor.SetVelocity((int)neededValue);
                  requestedSetPoint = neededValue;
                  Tracer.WriteMedium(TraceGroup.TBUS, null, "{0} velocity {1} {2}", motor.Name, neededValue, requestedSetPoint);
               }
            }
         }           
      }

      private void UpdatePumps()
      {
         this.frontPumpSpeedReading = this.frontPumpMotor.RPM;
         this.rearPumpSpeedReading = this.rearPumpMotor.RPM;

         PumpControl.Front.Update(this.frontPumpPressureReading, this.frontPumpSpeedReading);
         PumpControl.Rear.Update(this.rearPumpPressureReading, this.rearPumpSpeedReading);

         double frontPumpVelocity = PumpControl.Front.GetSpeedSetting();
         frontPumpVelocity *= (PumpControl.Front.GetDirection() == PumpDirections.forward) ? 1 : -1;
         this.UpdatePumpMotor(this.frontPumpMotor, frontPumpVelocity, ref frontPumpRequestedMode, ref frontPumpRequestedSpeed);

         double rearPumpVelocity = PumpControl.Rear.GetSpeedSetting();
         rearPumpVelocity *= (PumpControl.Rear.GetDirection() == PumpDirections.forward) ? 1 : -1;
         this.UpdatePumpMotor(this.rearPumpMotor, rearPumpVelocity, ref rearPumpRequestedMode, ref rearPumpRequestedSpeed);
      }

      #endregion

      #region RS232 Scale Functions

      #region Front RS232 Scale Functions

      private void InitializeFrontRs232Scale()
      {
         this.frontScaleRs232.Initialize();
      }

      private void StartFrontRs232Scale()
      {
         this.frontScaleRs232.Start(9600, 7, 2, 1);
      }

      #endregion

      #region Rear RS232 Scale Functions

      private void InitializeRearRs232Scale()
      {
         this.rearScaleRs232.Initialize();
      }

      private void StartRearRs232Scale()
      {
         this.rearScaleRs232.Start(9600, 7, 2, 1);
      }

      #endregion

      private void InitializeRs232Scales()
      {
         this.InitializeFrontRs232Scale();
         this.InitializeRearRs232Scale();
      }

      private void StartRs232Scales()
      {
         this.StartFrontRs232Scale();
         this.StartRearRs232Scale();
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
               break;
            }
            else if (DateTime.Now > limit)
            {
               device.Fault("boot timeout");
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
         
         this.reelMotor.NodeId = (byte)ParameterAccessor.Instance.TruckBus.ReelMotorBusId;
         this.reelDigitalIo.NodeId = (byte)ParameterAccessor.Instance.TruckBus.ReelDigitalBusId;
         this.reelAnalogIo.NodeId = (byte)ParameterAccessor.Instance.TruckBus.ReelAnalogBusId;
         this.reelEncoder.NodeId = (byte)ParameterAccessor.Instance.TruckBus.ReelEncoderBusId;
         this.feederTopFrontMotor.NodeId = (byte)ParameterAccessor.Instance.TruckBus.FeederTopFrontMotorBusId;
         this.feederTopRearMotor.NodeId = (byte)ParameterAccessor.Instance.TruckBus.FeederTopRearMotorBusId;
         this.feederBottomFrontMotor.NodeId = (byte)ParameterAccessor.Instance.TruckBus.FeederBottomFrontMotorBusId;
         this.feederBottomRearMotor.NodeId = (byte)ParameterAccessor.Instance.TruckBus.FeederBottomRearMotorBusId;
         this.feederEncoder.NodeId = (byte)ParameterAccessor.Instance.TruckBus.FeederEncoderBusId;
         this.guideLeftMotor.NodeId = (byte)ParameterAccessor.Instance.TruckBus.GuideLeftMotorBusId;
         this.guideRightMotor.NodeId = (byte)ParameterAccessor.Instance.TruckBus.GuideRightMotorBusId;
         this.launchDigitalIo.NodeId = (byte)ParameterAccessor.Instance.TruckBus.LaunchDigitalIoBusId;
         this.launchAnalogIo.NodeId = (byte)ParameterAccessor.Instance.TruckBus.LaunchAnalogIoBusId;
         this.gps.NodeId = (byte)ParameterAccessor.Instance.TruckBus.GpsBusId;

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            this.frontPumpMotor.NodeId = (byte)ParameterAccessor.Instance.TruckBus.FrontPumpBusId;
            this.frontScaleRs232.NodeId = (byte)ParameterAccessor.Instance.TruckBus.FrontScaleRs232BusId;

            this.rearPumpMotor.NodeId = (byte)ParameterAccessor.Instance.TruckBus.RearPumpBusId;
            this.rearScaleRs232.NodeId = (byte)ParameterAccessor.Instance.TruckBus.RearScaleRs232BusId;
         }

         this.TraceMask = ParameterAccessor.Instance.TruckBus.ControllerTraceMask;
         this.reelMotor.TraceMask = ParameterAccessor.Instance.TruckBus.ReelMotorTraceMask;
         this.reelDigitalIo.TraceMask = ParameterAccessor.Instance.TruckBus.ReelDigitalTraceMask;
         this.reelAnalogIo.TraceMask = ParameterAccessor.Instance.TruckBus.ReelAnalogTraceMask;
         this.reelEncoder.TraceMask = ParameterAccessor.Instance.TruckBus.ReelEncoderTraceMask;
         this.feederTopFrontMotor.TraceMask = ParameterAccessor.Instance.TruckBus.FeederTopFrontMotorTraceMask;
         this.feederTopRearMotor.TraceMask = ParameterAccessor.Instance.TruckBus.FeederTopRearMotorTraceMask;
         this.feederBottomFrontMotor.TraceMask = ParameterAccessor.Instance.TruckBus.FeederBottomFrontMotorTraceMask;
         this.feederBottomRearMotor.TraceMask = ParameterAccessor.Instance.TruckBus.FeederBottomRearMotorTraceMask;
         this.feederEncoder.TraceMask = ParameterAccessor.Instance.TruckBus.FeederEncoderTraceMask;
         this.guideLeftMotor.TraceMask = ParameterAccessor.Instance.TruckBus.GuideLeftMotorTraceMask;
         this.guideRightMotor.TraceMask = ParameterAccessor.Instance.TruckBus.GuideRightMotorTraceMask;
         this.launchDigitalIo.TraceMask = ParameterAccessor.Instance.TruckBus.LaunchDigitalIoTraceMask;
         this.launchAnalogIo.TraceMask = ParameterAccessor.Instance.TruckBus.LaunchAnalogIoTraceMask;
         this.gps.TraceMask = ParameterAccessor.Instance.TruckBus.GpsTraceMask;

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            this.frontPumpMotor.TraceMask = ParameterAccessor.Instance.TruckBus.FrontPumpTraceMask;
            this.frontScaleRs232.TraceMask = ParameterAccessor.Instance.TruckBus.FrontScaleRs232TraceMask;

            this.rearPumpMotor.TraceMask = ParameterAccessor.Instance.TruckBus.RearPumpTraceMask;
            this.rearScaleRs232.TraceMask = ParameterAccessor.Instance.TruckBus.RearScaleRs232TraceMask;
         }


         this.InitializeReel();

         this.reelModeSetPoint = ReelModes.off;
         this.reelNonManualModeSetPoint = this.reelModeSetPoint;
         this.reelManualCurrentSetPoint = 0;
         this.reelTotalStart = 0;
         this.reelTripStart = 0;


         this.InitializeFeeder();

         this.feederModeSetPoint = FeederModes.off;
         this.feederVelocitySetPoint = 0;


         this.InitializeGuide();
         this.InitializeLaunchTube();
         this.InitializeGps();

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            this.InitializePumps();
            this.InitializeRs232Scales();
         }
      }

      private void StartBus()
      {
         this.busReady = false;

         if (false == this.busReady)
         {
            this.busInterfaceId = ParameterAccessor.Instance.TruckBus.BusInterface;
            CANResult startResult = PCANLight.Start(this.busInterfaceId, ParameterAccessor.Instance.TruckBus.BitRate, FramesType.INIT_TYPE_ST, TraceGroup.TBUS, this.BusReceiveHandler);
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
                  device.Fault("boot timeout");
               }
            }
         }
         else
         {
            this.busStatus = "interface failure";
            Tracer.WriteMedium(TraceGroup.TBUS, "", "bus failure");

            foreach (Device device in this.deviceList)
            {
               device.Fault("interface not ready");
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
               receiveCount = this.deviceResetQueue.Count;

               if (receiveCount > 0)
               {
                  request = (DeviceRestartRequest)this.deviceResetQueue.Dequeue();
               }
            }

            if (null != request)
            {
               BusComponentId id = (BusComponentId)request.Id;

               if (BusComponentId.ReelMotor == id)
               {
                  this.InitializeReelMotor();
                  this.reelMotor.Reset();
                  this.WaitDeviceHeartbeat(this.reelMotor);
                  this.StartReelMotor();
               }
               else if (BusComponentId.ReelDigitalIo == id)
               {
                  this.InitilizeReelDigitalIo();
                  this.reelDigitalIo.Reset();
                  this.WaitDeviceHeartbeat(this.reelDigitalIo);
                  this.StartReelDigitalIo();
               }
               else if (BusComponentId.ReelAnalogIo == id)
               {
                  this.InitilizeReelAnalogIo();
                  this.reelAnalogIo.Reset();
                  this.WaitDeviceHeartbeat(this.reelAnalogIo);
                  this.StartReelAnalogIo();
               }
               else if (BusComponentId.ReelEncoder == id)
               {
                  this.InitializeReelEncoder();
                  this.reelEncoder.Reset();
                  this.WaitDeviceHeartbeat(this.reelEncoder);
                  this.StartReelEncoder();
               }
               else if (BusComponentId.FeederTopFrontMotor == id)
               {
                  this.evaluateFeederParameters = false;
                  this.feederTopFrontMotor.Initialize();
                  this.feederTopFrontStatus.Initialize();
                  this.WaitDeviceHeartbeat(this.feederTopFrontMotor);
                  this.feederTopFrontMotor.Reset();
                  this.StartFeederMotor(this.feederTopFrontMotor);
               }
               else if (BusComponentId.FeederTopRearMotor == id)
               {
                  this.evaluateFeederParameters = false;
                  this.feederTopRearMotor.Initialize();
                  this.feederTopRearStatus.Initialize();
                  this.WaitDeviceHeartbeat(this.feederTopRearMotor);
                  this.feederTopRearMotor.Reset();
                  this.StartFeederMotor(this.feederTopRearMotor);
               }
               else if (BusComponentId.FeederBottomFrontMotor == id)
               {
                  this.evaluateFeederParameters = false;
                  this.feederBottomFrontMotor.Initialize();
                  this.feederBottomFrontStatus.Initialize();
                  this.feederBottomFrontMotor.Reset();
                  this.WaitDeviceHeartbeat(this.feederBottomFrontMotor);
                  this.StartFeederMotor(this.feederBottomFrontMotor);
               }
               else if (BusComponentId.FeederBottomRearMotor == id)
               {
                  this.evaluateFeederParameters = false;
                  this.feederBottomRearMotor.Initialize();
                  this.feederBottomRearStatus.Initialize();
                  this.feederBottomRearMotor.Reset();
                  this.WaitDeviceHeartbeat(this.feederBottomRearMotor);
                  this.StartFeederMotor(this.feederBottomRearMotor);
               }
               else if (BusComponentId.FeederEncoder == id)
               {
#if false // currently off
                  // not initialized
                  this.feederEncoder.Reset();
                  // wait for reset heartbeat?
                  // not started
#endif
               }
               else if (BusComponentId.GuideLeftMotor == id)
               {
                  this.guideLeftMotor.Initialize();
                  this.guideLeftStatus.Initialize();
                  this.guideLeftMotor.Reset();
                  this.WaitDeviceHeartbeat(this.guideLeftMotor);
                  this.StartGuideMotor(this.guideLeftMotor);
               }
               else if (BusComponentId.GuideRightMotor == id)
               {
                  this.guideRightMotor.Initialize();
                  this.guideRightStatus.Initialize();
                  this.guideRightMotor.Reset();
                  this.WaitDeviceHeartbeat(this.guideRightMotor);
                  this.StartGuideMotor(this.guideRightMotor);
               }
               else if (BusComponentId.LaunchDigitalIo == id)
               {
                  this.InitilizeLaunchTubeDigitalIo();
                  this.launchDigitalIo.Reset();
                  this.WaitDeviceHeartbeat(this.launchDigitalIo);
                  this.StartLaunchTubeDigitalIo();

               }
               else if (BusComponentId.LaunchAnalogIo == id)
               {
                  this.InitilizeLaunchTubeAnalogIo();
                  this.launchAnalogIo.Reset();
                  this.WaitDeviceHeartbeat(this.launchAnalogIo);
                  this.StartLaunchTubeAnalogIo();
               }
               else if (BusComponentId.Gps == id)
               {
                  this.InitializeGps();
                  this.gps.Reset();
                  this.WaitDeviceHeartbeat(this.gps);
                  this.StartGps();
               }
               else if (BusComponentId.FrontPumpMotor == id)
               {
                  if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
                  {
                     this.InitializeFrontPump();
                     this.frontPumpMotor.Reset();
                     this.WaitDeviceHeartbeat(this.frontPumpMotor);
                     this.StartFrontPump();
                  }
               }
               else if (BusComponentId.FrontScaleRs232 == id)
               {
                  if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
                  {
                     this.InitializeFrontRs232Scale();
                     this.frontScaleRs232.Reset();
                     this.WaitDeviceHeartbeat(this.frontScaleRs232);
                     this.StartFrontRs232Scale();
                  }
               }
               else if (BusComponentId.RearPumpMotor == id)
               {
                  if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
                  {
                     this.InitializeRearPump();
                     this.rearPumpMotor.Reset();
                     this.WaitDeviceHeartbeat(this.rearPumpMotor);
                     this.StartRearPump();
                  }
               }
               else if (BusComponentId.RearScaleRs232 == id)
               {
                  if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
                  {
                     this.InitializeRearRs232Scale();
                     this.rearScaleRs232.Reset();
                     this.WaitDeviceHeartbeat(this.rearScaleRs232);
                     this.StartRearRs232Scale();
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
            }
         }
         while (0 != receiveCount);
      }

      private void ExecuteProcessLoop()
      {
         this.ready = true;

         this.controllerServiced = true;
         this.controllerHeartbeatLimit = DateTime.Now.AddMilliseconds(ParameterAccessor.Instance.TruckBus.ProducerHeartbeatRate);

         this.StartReel();
         this.StartFeeder();
         this.StartGuide();
         this.StartLaunchTube();
         this.StartGps();

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            this.StartPumps();
            this.StartRs232Scales();
         }

         for (; this.execute; )
         {
            this.UpdateReel();
            this.UpdateFeeder();
            this.UpdateGuide();
            this.UpdateLaunchTube();

            if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
            {
               this.UpdatePumps();
            }

            this.UpdateDeviceReset();

            Thread.Sleep(1);
         }
      }

      private void CloseBus()
      {
         this.busReady = false;

         PCANLight.ResetBus(this.busInterfaceId);
         Thread.Sleep(1000);
         PCANLight.Stop(this.busInterfaceId);

         ParameterAccessor.Instance.TruckBus.ControllerTraceMask = this.TraceMask;
         ParameterAccessor.Instance.TruckBus.ReelMotorTraceMask = this.reelMotor.TraceMask;
         ParameterAccessor.Instance.TruckBus.ReelDigitalTraceMask = this.reelDigitalIo.TraceMask;
         ParameterAccessor.Instance.TruckBus.ReelAnalogTraceMask = this.reelAnalogIo.TraceMask;
         ParameterAccessor.Instance.TruckBus.ReelEncoderTraceMask = this.reelEncoder.TraceMask;
         ParameterAccessor.Instance.TruckBus.FeederTopFrontMotorTraceMask = this.feederTopFrontMotor.TraceMask;
         ParameterAccessor.Instance.TruckBus.FeederTopRearMotorTraceMask = this.feederTopRearMotor.TraceMask;
         ParameterAccessor.Instance.TruckBus.FeederBottomFrontMotorTraceMask = this.feederBottomFrontMotor.TraceMask;
         ParameterAccessor.Instance.TruckBus.FeederBottomRearMotorTraceMask = this.feederBottomRearMotor.TraceMask;
         ParameterAccessor.Instance.TruckBus.FeederEncoderTraceMask = this.feederEncoder.TraceMask;
         ParameterAccessor.Instance.TruckBus.GuideLeftMotorTraceMask = this.guideLeftMotor.TraceMask;
         ParameterAccessor.Instance.TruckBus.GuideRightMotorTraceMask = this.guideRightMotor.TraceMask;
         ParameterAccessor.Instance.TruckBus.LaunchDigitalIoTraceMask = this.launchDigitalIo.TraceMask;
         ParameterAccessor.Instance.TruckBus.LaunchAnalogIoTraceMask = this.launchAnalogIo.TraceMask;
         ParameterAccessor.Instance.TruckBus.GpsTraceMask = this.gps.TraceMask;

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            ParameterAccessor.Instance.TruckBus.FrontPumpTraceMask = this.frontPumpMotor.TraceMask;
            ParameterAccessor.Instance.TruckBus.FrontScaleRs232TraceMask = this.frontScaleRs232.TraceMask;
            
            ParameterAccessor.Instance.TruckBus.RearPumpTraceMask = this.rearPumpMotor.TraceMask;
            ParameterAccessor.Instance.TruckBus.RearScaleRs232TraceMask = this.rearScaleRs232.TraceMask;
         }
      }

      #endregion

      #region Main Process Loop

      private void Process()
      {
         try
         {
            Tracer.WriteHigh(TraceGroup.TBUS, null, "start");

            this.InitializeValues();

            Tracer.WriteHigh(TraceGroup.GUI, null, "video init start");
            VideoStampOsd.Instance.Start(1, 9600);
            VideoStampOsd.Instance.SetDateAndTime(DateTime.Now);
            VideoStampOsd.Instance.Configure(ParameterAccessor.Instance.Osd);
            Tracer.WriteHigh(TraceGroup.GUI, null, "video init complete");

            this.deviceThread = new Thread(this.DeviceProcess);
            this.deviceThread.IsBackground = true;
            this.deviceThread.Name = "Truck CAN Devices";
            this.deviceThread.Start();

            this.StartBus();

            if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
            {
               FgDigitalScale.Front.Initialize();
               FgDigitalScale.Rear.Initialize();

               FgDigitalScale.Front.Start(new FgDigitalScale.SerialWriteHandler(this.frontScaleRs232.WriteSerial));
               FgDigitalScale.Rear.Start(new FgDigitalScale.SerialWriteHandler(this.rearScaleRs232.WriteSerial));
            }

            if (this.GetThicknessReadingEnabled() != false)
            {
               ThicknessSensor.Instance.Start(ParameterAccessor.Instance.ThicknessSensor.Address, ParameterAccessor.Instance.ThicknessSensor.Port);
            }

            if (this.GetStressReadingEnabled() != false)
            {
               StressSensor.Instance.Start(ParameterAccessor.Instance.StressSensor.Address, ParameterAccessor.Instance.StressSensor.Port);
            }

            this.ExecuteProcessLoop();
         }
         catch (Exception preException)
         {
            this.busStatus = "interface exception";

            foreach (Device device in this.deviceList)
            {
               device.Fault("interface not ready");
            }

            Tracer.WriteError(TraceGroup.TBUS, null, "process exception {0}", preException.Message);
         }

         try
         {
            this.execute = false;

            this.CloseBus();

            this.deviceThread.Join(3000);
            this.deviceThread = null;

            if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
            {
               FgDigitalScale.Front.Stop();
               FgDigitalScale.Rear.Stop();
            }

            if (RobotApplications.inspect == ParameterAccessor.Instance.RobotApplication)
            {
               ThicknessSensor.Instance.Stop();
               StressSensor.Instance.Stop();
            }

            VideoStampOsd.Instance.Stop();
            this.InitializeValues(); // clears previous session requests for next session
         }
         catch (Exception postException)
         {
            this.busStatus = "interface exception";
            Tracer.WriteError(TraceGroup.TBUS, null, "post process exception {0}", postException.Message);
         }

         this.ready = false;
         Tracer.WriteHigh(TraceGroup.TBUS, null, "stop");
      }

      #endregion

      #region Constructor

      private void Initialize()
      {
         this.busReceiveQueue = new Queue();
         this.deviceResetQueue = new Queue();

         this.reelMotor = new ElmoWhistleMotor("reel motor", (byte)ParameterAccessor.Instance.TruckBus.ReelMotorBusId);
         this.reelDigitalIo = new PeakDigitalIo("reel digital IO", (byte)ParameterAccessor.Instance.TruckBus.ReelDigitalBusId);
         this.reelAnalogIo = new PeakAnalogIo("reel analog IO", (byte)ParameterAccessor.Instance.TruckBus.ReelAnalogBusId);
         this.reelEncoder = new KublerRotaryEncoder("reel encoder", (byte)ParameterAccessor.Instance.TruckBus.ReelEncoderBusId);
         this.feederTopFrontMotor = new ElmoWhistleMotor("feeder tf-motor", (byte)ParameterAccessor.Instance.TruckBus.FeederTopFrontMotorBusId);
         this.feederTopRearMotor = new ElmoWhistleMotor("feeder tr-motor", (byte)ParameterAccessor.Instance.TruckBus.FeederTopRearMotorBusId);
         this.feederBottomFrontMotor = new ElmoWhistleMotor("feeder bf-motor", (byte)ParameterAccessor.Instance.TruckBus.FeederBottomFrontMotorBusId);
         this.feederBottomRearMotor = new ElmoWhistleMotor("feeder br-motor", (byte)ParameterAccessor.Instance.TruckBus.FeederBottomRearMotorBusId);
         this.feederEncoder = new KublerRotaryEncoder("feeder encoder", (byte)ParameterAccessor.Instance.TruckBus.FeederEncoderBusId);
         this.guideLeftMotor = new ElmoWhistleMotor("guide l-motor", (byte)ParameterAccessor.Instance.TruckBus.GuideLeftMotorBusId);
         this.guideRightMotor = new ElmoWhistleMotor("guide r-motor", (byte)ParameterAccessor.Instance.TruckBus.GuideRightMotorBusId);
         this.launchDigitalIo = new PeakDigitalIo("launch digital IO", (byte)ParameterAccessor.Instance.TruckBus.LaunchDigitalIoBusId);
         this.launchAnalogIo = new PeakAnalogIo("launch analog IO", (byte)ParameterAccessor.Instance.TruckBus.LaunchAnalogIoBusId);
         this.gps = new UlcRoboticsGps("gps", (byte)ParameterAccessor.Instance.TruckBus.GpsBusId);

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            this.frontPumpMotor = new ElmoWhistleMotor("front pump motor", (byte)ParameterAccessor.Instance.TruckBus.FrontPumpBusId);
            this.frontScaleRs232 = new UlcRoboticsRs232("front scale rs232", (byte)ParameterAccessor.Instance.TruckBus.FrontScaleRs232BusId);
            this.frontScaleRs232.OnSerialReceive = new UlcRoboticsRs232.SerialReceiveHandler(FgDigitalScale.Front.Receive);

            this.rearPumpMotor = new ElmoWhistleMotor("rear pump motor", (byte)ParameterAccessor.Instance.TruckBus.RearPumpBusId);
            this.rearScaleRs232 = new UlcRoboticsRs232("rear scale rs232", (byte)ParameterAccessor.Instance.TruckBus.RearScaleRs232BusId);
            this.rearScaleRs232.OnSerialReceive = new UlcRoboticsRs232.SerialReceiveHandler(FgDigitalScale.Rear.Receive);
         }

         this.deviceList = new ArrayList();
         this.deviceList.Add(this.reelMotor);
         this.deviceList.Add(this.reelEncoder);
         this.deviceList.Add(this.feederTopFrontMotor);
         this.deviceList.Add(this.feederTopRearMotor);
         this.deviceList.Add(this.feederBottomFrontMotor);
         this.deviceList.Add(this.feederBottomRearMotor);
         this.deviceList.Add(this.feederEncoder);
         this.deviceList.Add(this.guideLeftMotor);
         this.deviceList.Add(this.guideRightMotor);
         this.deviceList.Add(this.reelDigitalIo);
         this.deviceList.Add(this.launchDigitalIo);
         this.deviceList.Add(this.reelAnalogIo);
         this.deviceList.Add(this.launchAnalogIo);
         this.deviceList.Add(this.gps);

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            this.deviceList.Add(this.frontPumpMotor);
            this.deviceList.Add(this.frontScaleRs232);

            this.deviceList.Add(this.rearPumpMotor);
            this.deviceList.Add(this.rearScaleRs232);
         }

         foreach (Device device in this.deviceList)
         {
            device.OnReceiveTrace = new Device.ReceiveTraceHandler(this.DeviceTraceReceive);
            device.OnTransmitTrace = new Device.TransmitTraceHandler(this.DeviceTraceTransmit);
            device.OnTransmit = new Device.TransmitHandler(this.DeviceTransmit);
            device.OnCommError = new Device.CommErrorHandler(this.DeviceError);
         }

         this.reelDigitalIo.OnInputChange = new PeakDigitalIo.InputChangeHandler(this.DigitalIoInputChangeHandler);
         this.launchDigitalIo.OnInputChange = new PeakDigitalIo.InputChangeHandler(this.DigitalIoInputChangeHandler);

         this.feederTopFrontStatus = new FeederMotorStatus();
         this.feederTopRearStatus = new FeederMotorStatus();
         this.feederBottomFrontStatus = new FeederMotorStatus();
         this.feederBottomRearStatus = new FeederMotorStatus();

         this.guideLeftStatus = new GuideMotorStatus();
         this.guideRightStatus = new GuideMotorStatus();
      }

      private TruckCommBus()
      {
         this.launchCameraLightIntensities = new int[4];
         this.launchCameraLightIntensityRequests = new int[4];
      }

      #endregion

      #region Access Functions

      public void Start()
      {
         this.thread = new Thread(this.Process);
         this.thread.IsBackground = true;
         this.thread.Name = "Truck Comm";

         this.ready = false; 
         this.execute = true;
         this.thread.Start();

         for (int i = 0; i < 2000; i++)
         {
            if (false != this.ready)
            {
               break;
            }

            Thread.Sleep(1);
         }
      }

      public void Stop()
      {
         this.execute = false;
         this.thread.Join(3000);

         this.thread = null;
      }

      public void Service()
      {
         this.controllerServiced = true;
      }

      /// <summary>
      /// Get commutative status of every on the bus. 
      /// </summary>
      /// <remarks>Each id supported is expected to be evaluated.</remarks>
      /// <returns>null when ok, string with description when faulted</returns>
      public string GetStatus()
      {
         string result = null;

         if (this.GetStatus(BusComponentId.Bus) != null)
         {
            result = "truck communication offline";
         }
         else if (this.GetStatus(BusComponentId.ReelMotor) != null) 
         {
            result = "tether reel motor offline";
         }
         else if (this.GetStatus(BusComponentId.ReelDigitalIo) != null) 
         {
            result = "tether reel digital IO offline";
         }
         else if (this.GetStatus(BusComponentId.ReelAnalogIo) != null) 
         {
            result = "tether reel analog IO offline";
         }
         else if (this.GetStatus(BusComponentId.ReelEncoder) != null) 
         {
            result = "tether reel encoder offline";
         }


         else if (this.GetStatus(BusComponentId.FeederTopFrontMotor) != null)
         {
            result = "tether feeder top front motor offline";
         }
         else if (this.GetStatus(BusComponentId.FeederTopRearMotor) != null)
         {
            result = "tether feeder top rear motor offline";
         }
         else if (this.GetStatus(BusComponentId.FeederBottomFrontMotor) != null)
         {
            result = "tether feeder bottom front motor offline";
         }
         else if (this.GetStatus(BusComponentId.FeederBottomRearMotor) != null)
         {
            result = "tether feeder bottom rear motor offline";
         }
#if false
         else if (this.GetStatus(BusComponentId.FeederEncoder) != null)
         {
            result = "tether feeder encoder motor offline";
         }
#endif
         else if (this.GetStatus(BusComponentId.GuideLeftMotor) != null)
         {
            result = "tether guide left motor offline";
         }
         else if (this.GetStatus(BusComponentId.GuideRightMotor) != null)
         {
            result = "tether guide right motor offline";
         }
         else if (this.GetStatus(BusComponentId.LaunchDigitalIo) != null)
         {
            result = "launch digital IO offline";
         }
         else if (this.GetStatus(BusComponentId.LaunchAnalogIo) != null)
         {
            result = "launch analog IO offline";
         }
         else if (this.GetStatus(BusComponentId.Gps) != null)
         {
            result = "GPS offline";
         }
         else if ((this.GetStatus(BusComponentId.NitrogenSensor1) != null) ||
                  (this.GetStatus(BusComponentId.NitrogenSensor2) != null))
         {
            result = "nitrogen sensor offline";
         }
         else if ((this.GetStatus(BusComponentId.FrontPumpMotor) != null) ||
                  (this.GetStatus(BusComponentId.FrontPressureSensor) != null))
         {
            result = "front pump offline";
         }
         else if ((this.GetStatus(BusComponentId.FrontScaleRs232) != null) ||
                  (this.GetStatus(BusComponentId.FrontDigitalScale) != null))
         {
            result = "front scale offline";
         }
         else if ((this.GetStatus(BusComponentId.RearPumpMotor) != null) ||
                  (this.GetStatus(BusComponentId.RearPressureSensor) != null))
         {
            result = "rear pump offline";
         }
         else if ((this.GetStatus(BusComponentId.RearScaleRs232) != null) ||
                  (this.GetStatus(BusComponentId.RearDigitalScale) != null))
         {
            result = "rear scale offline";
         }
         else if ((this.GetStatus(BusComponentId.ThicknessSensor) != null) ||
                  (this.GetStatus(BusComponentId.StressSensor) != null))
         {
            result = "measurement sensor offline";
         }

         return (result);
      }

      /// <summary>
      /// Get status of a particular component.
      /// </summary>
      /// <param name="id">component to query</param>
      /// <returns>null when ok, string with description when faulted</returns>
      public string GetStatus(BusComponentId id)
      {
         string result = null;

         switch (id)
         {
            case BusComponentId.Bus:
            {
               result = this.busStatus;
               break;
            }
            case BusComponentId.ReelMotor:
            {
               result = this.reelMotor.FaultReason;
               break;
            }
            case BusComponentId.ReelDigitalIo:
            {
               result = this.reelDigitalIo.FaultReason;
               break;
            }
            case BusComponentId.ReelAnalogIo:
            {
               result = this.reelAnalogIo.FaultReason;
               break;
            }
            case BusComponentId.ReelEncoder:
            {
               result = this.reelEncoder.FaultReason;
               break;
            }
            case BusComponentId.FeederTopFrontMotor:
            {
               result = this.feederTopFrontMotor.FaultReason;
               break;
            }
            case BusComponentId.FeederTopRearMotor:
            {
               result = this.feederTopRearMotor.FaultReason;
               break;
            }
            case BusComponentId.FeederBottomFrontMotor:
            {
               result = this.feederBottomFrontMotor.FaultReason;
               break;
            }
            case BusComponentId.FeederBottomRearMotor:
            {
               result = this.feederBottomRearMotor.FaultReason;
               break;
            }
            case BusComponentId.FeederEncoder:
            {
               result = this.feederEncoder.FaultReason;
               break;
            }
            case BusComponentId.GuideLeftMotor:
            {
               result = this.guideLeftMotor.FaultReason;
               break;
            }
            case BusComponentId.GuideRightMotor:
            {
               result = this.guideRightMotor.FaultReason;
               break;
            }
            case BusComponentId.LaunchDigitalIo:
            {
               result = this.launchDigitalIo.FaultReason;
               break;
            }
            case BusComponentId.LaunchAnalogIo:
            {
               result = this.launchAnalogIo.FaultReason;
               break;
            }
            case BusComponentId.Gps:
            {
               result = this.gps.FaultReason;
               break;
            }
            case BusComponentId.NitrogenSensor1:
            {
               result = this.nitrogenSensor1Fault;
               break;
            }
            case BusComponentId.NitrogenSensor2:
            {
               result = this.nitrogenSensor2Fault;
               break;
            }
            case BusComponentId.RobotTotalCurrentSensor:
            {
               result = this.robotTotalCurrentFault;
               break;
            }
            case BusComponentId.LaunchTotalCurrentSensor:
            {
               result = this.launchTotalCurrentFault;
               break;
            }
            case BusComponentId.FrontPumpMotor:
            {
               if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
               {
                  result = this.frontPumpMotor.FaultReason;
               }

               break;
            }
            case BusComponentId.FrontPressureSensor:
            {
               if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
               {
                  result = this.frontPressureFault;
               }

               break;
            }
            case BusComponentId.FrontScaleRs232:
            {
               if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
               {
                  result = this.frontScaleRs232.FaultReason;
               }

               break;
            }
            case BusComponentId.FrontDigitalScale:
            {
               if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
               {
                  result = FgDigitalScale.Front.FaultReason;
               }

               break;
            }
            case BusComponentId.RearPumpMotor:
            {
               if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
               {
                  result = this.rearPumpMotor.FaultReason;
               }

               break;
            }
            case BusComponentId.RearPressureSensor:
            {
               if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
               {
                  result = this.rearPressureFault;
               }

               break;
            }
            case BusComponentId.RearScaleRs232:
            {
               if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
               {
                  result = this.rearScaleRs232.FaultReason;
               }

               break;
            }
            case BusComponentId.RearDigitalScale:
            {
               if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
               {
                  result = FgDigitalScale.Rear.FaultReason;
               }

               break;
            }
            case BusComponentId.ThicknessSensor:
            {
               if (RobotApplications.inspect == ParameterAccessor.Instance.RobotApplication)
               {
                  result = ThicknessSensor.Instance.FaultReason;
               }

               break;
            }
            case BusComponentId.StressSensor:
            {
               if (RobotApplications.inspect == ParameterAccessor.Instance.RobotApplication)
               {
                  result = StressSensor.Instance.FaultReason;
               }

               break;
            }
         }

         return (result);
      }

      public Device GetDevice(Enum deviceId)
      {
         Device result = null;
         BusComponentId id = (BusComponentId)deviceId;

         switch (id)
         {
            case BusComponentId.ReelMotor:
            {
               result = this.reelMotor;
               break;
            }
            case BusComponentId.ReelDigitalIo:
            {
               result = this.reelDigitalIo;
               break;
            }
            case BusComponentId.ReelAnalogIo:
            {
               result = this.reelAnalogIo;
               break;
            }
            case BusComponentId.ReelEncoder:
            {
               result = this.reelEncoder;
               break;
            }
            case BusComponentId.FeederTopFrontMotor:
            {
               result = this.feederTopFrontMotor;
               break;
            }
            case BusComponentId.FeederTopRearMotor:
            {
               result = this.feederTopRearMotor;
               break;
            }
            case BusComponentId.FeederBottomFrontMotor:
            {
               result = this.feederBottomFrontMotor;
               break;
            }
            case BusComponentId.FeederBottomRearMotor:
            {
               result = this.feederBottomRearMotor;
               break;
            }
            case BusComponentId.FeederEncoder:
            {
               result = this.feederEncoder;
               break;
            }
            case BusComponentId.GuideLeftMotor:
            {
               result = this.guideLeftMotor;
               break;
            }
            case BusComponentId.GuideRightMotor:
            {
               result = this.guideRightMotor;
               break;
            }
            case BusComponentId.LaunchDigitalIo:
            {
               result = this.launchDigitalIo;
               break;
            }
            case BusComponentId.LaunchAnalogIo:
            {
               result = this.launchAnalogIo;
               break;
            }
            case BusComponentId.Gps:
            {
               result = this.gps;
               break;
            }
            case BusComponentId.FrontPumpMotor:
            {
               if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
               {
                  result = this.frontPumpMotor;
               }

               break;
            }
            case BusComponentId.FrontScaleRs232:
            {
               if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
               {
                  result = this.frontScaleRs232;
               }

               break;
            }
            case BusComponentId.RearPumpMotor:
            {
               if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
               {
                  result = this.rearPumpMotor;
               }

               break;
            }
            case BusComponentId.RearScaleRs232:
            {
               if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
               {
                  result = this.rearScaleRs232;
               }

               break;
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


      #endregion

      #region Sensor Functions

      public double GetNitrogenPressureReading1()
      {
         return (this.nitrogenPressureReading1);
      }

      public double GetNitrogenPressureReading2()
      {
         return (this.nitrogenPressureReading2);
      }

      public double GetRobotTotalCurrentReading()
      {
         return (this.robotTotalCurrentReading);
      }

      public double GetLaunchTotalCurrentReading()
      {
         return (this.launchTotalCurrentReading);
      }
      
      #endregion

      #region Pump Functions

      public void StartAutoPump(ToolLocations toolLocation)
      {
         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            PumpControl pumpControl = (ToolLocations.front == toolLocation) ? PumpControl.Front : PumpControl.Rear;
            pumpControl.StartAuto();
         }
      }

      public void PauseAutoPump(ToolLocations toolLocation)
      {
         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            PumpControl pumpControl = (ToolLocations.front == toolLocation) ? PumpControl.Front : PumpControl.Rear;
            pumpControl.PauseAuto();
         }
      }

      public void ResumeAutoPump(ToolLocations toolLocation)
      {
         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            PumpControl pumpControl = (ToolLocations.front == toolLocation) ? PumpControl.Front : PumpControl.Rear;
            pumpControl.ResumeAuto();
         }
      }

      public void StopAutoPump(ToolLocations toolLocation)
      {
         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            PumpControl pumpControl = (ToolLocations.front == toolLocation) ? PumpControl.Front : PumpControl.Rear;
            pumpControl.StopAuto();
         }
      }

      public void SetPumpDirection(ToolLocations toolLocation, PumpDirections direction)
      {
         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            PumpControl pumpControl = (ToolLocations.front == toolLocation) ? PumpControl.Front : PumpControl.Rear;
            pumpControl.SetDirection(direction);
         }
      }

      public void SetPumpSpeed(ToolLocations toolLocation, double speed)
      {
         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            PumpControl pumpControl = (ToolLocations.front == toolLocation) ? PumpControl.Front : PumpControl.Rear;
            pumpControl.SetSpeed(speed);
         }
      }

      public void SetPumpPressure(ToolLocations toolLocation, double pressure)
      {
         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            PumpControl pumpControl = (ToolLocations.front == toolLocation) ? PumpControl.Front : PumpControl.Rear;
            pumpControl.SetPressure(pressure);
         }
      }

      public void StartPump(ToolLocations toolLocation)
      {
         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            PumpControl pumpControl = (ToolLocations.front == toolLocation) ? PumpControl.Front : PumpControl.Rear;
            pumpControl.Start();
         }
      }

      public void StopPump(ToolLocations toolLocation)
      {
         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            PumpControl pumpControl = (ToolLocations.front == toolLocation) ? PumpControl.Front : PumpControl.Rear;
            pumpControl.Stop();
         }
      }

      public void RelievePumpPressure(ToolLocations toolLocation)
      {
         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            PumpControl pumpControl = (ToolLocations.front == toolLocation) ? PumpControl.Front : PumpControl.Rear;
            pumpControl.RelievePressure();
         }
      }

      public PumpAutoStates GetPumpAutoState(ToolLocations toolLocation)
      {
         PumpAutoStates result = PumpAutoStates.off;

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            PumpControl pumpControl = (ToolLocations.front == toolLocation) ? PumpControl.Front : PumpControl.Rear;
            result = pumpControl.GetAutoState();
         }

         return (result);
      }

      public PumpModes GetPumpMode(ToolLocations toolLocation)
      {
         PumpModes result = PumpModes.speed;

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            PumpControl pumpControl = (ToolLocations.front == toolLocation) ? PumpControl.Front : PumpControl.Rear;
            result = pumpControl.GetMode();
         }

         return (result);
      }

      public PumpDirections GetPumpDirection(ToolLocations toolLocation)
      {
         PumpDirections result = PumpDirections.forward;

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            PumpControl pumpControl = (ToolLocations.front == toolLocation) ? PumpControl.Front : PumpControl.Rear;
            result = pumpControl.GetDirection();
         }

         return (result);
      }

      public double GetPumpSetPoint(ToolLocations toolLocation)
      {
         double result = 0;

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            PumpControl pumpControl = (ToolLocations.front == toolLocation) ? PumpControl.Front : PumpControl.Rear;
            result = pumpControl.GetSetPoint();
         }

         return (result);
      }

      public double GetPumpVolumePerSecond(ToolLocations toolLocation)
      {
         double result = 0;

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            PumpControl pumpControl = (ToolLocations.front == toolLocation) ? PumpControl.Front : PumpControl.Rear;
            result = pumpControl.GetVolumePerSecond();
         }

         return (result);
      }

      public double GetReserviorWeightReading(ToolLocations toolLocation)
      {
         double result = 0;

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            FgDigitalScale digitalScale = (ToolLocations.front == toolLocation) ? FgDigitalScale.Front : FgDigitalScale.Rear;
            result = digitalScale.GetReading();
         }

         return (result);
      }

      public double GetPumpVolumeMeasure(ToolLocations toolLocation)
      {
         double result = 0;

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            PumpControl pumpControl = (ToolLocations.front == toolLocation) ? PumpControl.Front : PumpControl.Rear;
            result = pumpControl.GetMeasuredVolume();
         }

         return (result);
      }

      public double GetPumpPressureReading(ToolLocations toolLocation)
      {
         double result = 0;

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            result  = (ToolLocations.front == toolLocation) ? this.frontPumpPressureReading : this.rearPumpPressureReading;
         }

         return (result);
      }

      public double GetPumpSpeedReading(ToolLocations toolLocation)
      {
         double result = 0;

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            result = (ToolLocations.front == toolLocation) ? this.frontPumpSpeedReading : this.rearPumpSpeedReading;
         }

         return (result);
      }

      public bool GetPumpActivity(ToolLocations toolLocation)
      {
         bool result = false;

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            PumpControl pumpControl = (ToolLocations.front == toolLocation) ? PumpControl.Front : PumpControl.Rear;
            result = pumpControl.GetActivity();
         }

         return (result);
      }

      #endregion

      #region Reel Functions

      public void SetReelMode(ReelModes mode)
      {
         if (ReelModes.manual != mode)
         {
            if (ReelModes.manual != this.reelModeSetPoint)
            {
               this.reelModeSetPoint = mode;
            }
            else
            {
               this.reelNonManualModeSetPoint = mode;
            }
         }
      }

      public void SetReelManualMode(bool enable)
      {
         if (false != enable)
         {
            this.reelNonManualModeSetPoint = this.reelModeSetPoint;
            this.reelModeSetPoint = ReelModes.manual;
         }
         else
         {
            this.reelModeSetPoint = this.reelNonManualModeSetPoint;
         }
      }

      public void SetReelManualCurrent(double current)
      {
         this.reelManualCurrentSetPoint = current;
      }
      
      public void SetReelManualSpeed(double speed)
      {
         this.reelManualSpeedSetPoint = speed;
      }      

      public void SetReelTotalDistance(double distance)
      {
         double rotations = distance / ParameterAccessor.Instance.ReelDistanceScale.OperationalValue;
         this.reelTotalStart = this.reelEncoder.Rotations - rotations;
      }

      public void SetReelTripDistance(double distance)
      {
         double rotations = distance / ParameterAccessor.Instance.ReelDistanceScale.OperationalValue;
         this.reelTripStart = this.reelEncoder.Rotations - rotations;
      }

      public void ResetReelTotalDistance()
      {
         this.reelTotalStart = this.reelEncoder.Rotations;
      }

      public void ResetReelTripDistance()
      {
         this.reelTripStart = this.reelEncoder.Rotations;
      }

      public void StartReelCalibration()
      {
         this.reelCalibrationStart = this.reelEncoder.Rotations;
         this.reelCalibrationEnabled = true;
      }

      public void CalibrateReel(double distance)
      {
         if (false != this.reelCalibrationEnabled)
         {
            double rotations = this.reelEncoder.Rotations - this.reelCalibrationStart;

            if (0 != rotations)
            {
               ParameterAccessor.Instance.ReelDistanceScale.OperationalValue = distance / rotations;
               Tracer.WriteHigh(TraceGroup.TBUS, "", "reel calibration done {0}", ParameterAccessor.Instance.ReelDistanceScale.OperationalValue); 
            }

            this.reelCalibrationEnabled = false;
         }
      }

      public ReelModes GetReelMode()
      {
         return (this.reelModeSetPoint);
      }

      public bool ReelInCurrentMode()
      {
         bool result = false;

         if ((ElmoWhistleMotor.Modes.off == this.reelRequestedMode) ||
             (ElmoWhistleMotor.Modes.undefined == this.reelRequestedMode))
         {
            result = (MovementForwardControls.current == ParameterAccessor.Instance.ReelMotionMode) ? true : false;
         }
         else if (ElmoWhistleMotor.Modes.current == this.reelRequestedMode)
         {
            result = true;
         }

         return (result);
      }

      public double GetReelCurrent()
      {
         return (this.reelMotor.Torque * -1);
      }

      public double GetReelSpeed()
      {
         return (this.reelMotor.RPM * -1);
      }      

      public double GetReelTotalDistance()
      {
         double rotations = this.reelEncoder.Rotations - this.reelTotalStart;
         double distance = rotations * ParameterAccessor.Instance.ReelDistanceScale.OperationalValue;
         return (distance);
      }

      public double GetReelTripDistance()
      {
         double rotations = this.reelEncoder.Rotations - this.reelTripStart;
         double distance = rotations * ParameterAccessor.Instance.ReelDistanceScale.OperationalValue;
         return (distance);
      }

      #endregion

      #region Feeder Functions

      public void SetFeederMode(FeederModes mode)
      {
         this.feederModeSetPoint = mode;
      }

      public void SetFeederVelocity(double feederVelocity)
      {
         this.feederVelocitySetPoint = feederVelocity;
      }

      public void SetFeederClamp(bool setPoint)
      {
#if false
         lock(this)
         {
            if (setPoint == this.feederClampActual)
            {
               this.feederClampAdjusting = true;
               this.feederClampSetPoint = setPoint;
               this.feederClampSetPointTimeLimit = DateTime.Now.AddSeconds(10);
            }
         }
#endif
      }

      public FeederModes GetFeederMode()
      {
         return (this.feederModeSetPoint);
      }

      public double GetFeederVelocity()
      {
         double result = 0;
         int count = 0;

         this.EvaluateFeederMotorVelocity(this.feederTopFrontMotor, this.feederTopFrontStatus, ParameterAccessor.Instance.TopFrontFeederMotor, ref result, ref count);
         this.EvaluateFeederMotorVelocity(this.feederTopRearMotor, this.feederTopRearStatus, ParameterAccessor.Instance.TopRearFeederMotor, ref result, ref count);
         this.EvaluateFeederMotorVelocity(this.feederBottomFrontMotor, this.feederBottomFrontStatus, ParameterAccessor.Instance.BottomFrontFeederMotor, ref result, ref count);
         this.EvaluateFeederMotorVelocity(this.feederBottomRearMotor, this.feederBottomRearStatus, ParameterAccessor.Instance.BottomRearFeederMotor, ref result, ref count);

         if (0 != count)
         {
            result /= count;
         }

         return (result);
      }

      public double GetTopFrontFeederCurrent()
      {
         return (this.feederTopFrontMotor.Torque);
      }

      public double GetTopRearFeederCurrent()
      {
         return (this.feederTopRearMotor.Torque);
      }

      public double GetBottomFrontFeederCurrent()
      {
         return (this.feederBottomFrontMotor.Torque);
      }

      public double GetBottomRearFeederCurrent()
      {
         return (this.feederBottomRearMotor.Torque);
      }

      public bool GetFeederClampSetPoint()
      {
         return (false);
         //return (this.feederClampSetPoint);
      }

      public bool GetFeederClamp()
      {
         //return (this.feederClampActual);
         return (false);
      }

      /// <summary>
      /// Forces evaluation of settings.
      /// </summary>
      public void EvaluateFeederParameters()
      {
         this.evaluateFeederParameters = true;
      }

      #endregion

      #region Guide Functions

      public void SetGuideDirection(GuideLocations location, GuideDirections direction)
      {
         if (GuideLocations.left == location)
         {
            this.guideLeftStatus.direction = direction;
         }
         else if (GuideLocations.right == location)
         {
            this.guideRightStatus.direction = direction;
         }
      }

      public GuideDirections GetGuideDirection(GuideLocations location)
      {
         GuideDirections result = GuideDirections.off;

         if (GuideLocations.left == location)
         {
            result = this.guideLeftStatus.direction;
         }
         else if (GuideLocations.right == location)
         {
            result = this.guideRightStatus.direction;
         }

         return (result);
      }

      public bool GuideAtRetractLimit(GuideLocations location)
      {
         bool result = false;

         if (GuideLocations.left == location)
         {
            result = this.guideLeftStatus.RetractionLimit;
         }
         else if (GuideLocations.right == location)
         {
            result = this.guideRightStatus.RetractionLimit;
         }

         return (result);
      }

      public bool GuideAtExtendLimit(GuideLocations location)
      {
         bool result = false;

         if (GuideLocations.left == location)
         {
            result = this.guideLeftStatus.ExtensionLimit;
         }
         else if (GuideLocations.right == location)
         {
            result = this.guideRightStatus.ExtensionLimit;
         }

         return (result);
      }


      #endregion

      #region GPS Functions

      public double GetGpsLongitude()
      {
         double result = this.gps.Longitude;
         return (result);
      }

      public double GetGpsLatitude()
      {
         double result = this.gps.Latitude;
         return (result);
      }

      public DateTime GetGpsTime()
      {
         DateTime result = this.gps.Utc;
         return (result);
      }

      #endregion

      #region Sensor Functions

      public void TriggerThicknessReading(double latitude, double longitude, DateTime dateTime, Directions direction, double displacement, double radialLocation)
      {
         ThicknessSensor.Instance.TriggerReading(latitude, longitude, dateTime, direction, displacement, radialLocation);
      }

      public void TriggerStressReading(double latitude, double longitude, DateTime dateTime, Directions direction, double displacement, double radialLocation)
      {
         StressSensor.Instance.TriggerReading(latitude, longitude, dateTime, direction, displacement, radialLocation);
      }

      public bool GetThicknessReadingEnabled()
      {
         bool result = false;

         if ((RobotApplications.inspect == ParameterAccessor.Instance.RobotApplication) &&
             (0 != ParameterAccessor.Instance.ThicknessSensor.Port))
         {
            result = true;
         }

         return (result);
      }

      public bool GetThicknessReadingPending()
      {
         return (ThicknessSensor.Instance.Pending());
      }

      public double GetThicknessReading()
      {
         return (ThicknessSensor.Instance.GetReading());
      }

      public bool GetStressReadingEnabled()
      {
         bool result = false;

         if ((RobotApplications.inspect == ParameterAccessor.Instance.RobotApplication) &&
             (0 != ParameterAccessor.Instance.StressSensor.Port))
         {
            result = true;
         }

         return (result);
      }

      public bool GetStressReadingPending()
      {
         return (StressSensor.Instance.Pending());
      }

      public double GetStressReading()
      {
         return (StressSensor.Instance.GetReading());
      }

      #endregion

      #region Camera Functions

      public void SetLaunchCamera(CameraLocations camera)
      {
         this.selectedLaunchCamera = camera;
      }

      public void SetCameraLightLevel(CameraLocations camera, int level)
      {
         int index = -1;

         if (CameraLocations.launchLeftGuide == camera)
         {
            index = 2; // 0;
         }
         else if (CameraLocations.launchRightGuide == camera)
         {
            index = 3; // 1;
         }
         else if (CameraLocations.launchFeeder == camera)
         {
            index = 0; // 2;
         }
         else if (CameraLocations.launchMain == camera)
         {
            index = 1; // 3;
         }

         if (index >= 0)
         {
            this.launchCameraLightIntensities[index] = level;
         }
      }

      public CameraLocations GetLaunchCamera()
      {
         return (this.selectedLaunchCamera);
      }

      #endregion
   }

}

