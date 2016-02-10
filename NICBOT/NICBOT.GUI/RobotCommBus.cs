
namespace NICBOT.GUI
{
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

   public class RobotCommBus
   {
      #region Definitions

      private const int BootTimeoutPeriod = 1500;

      public enum BusComponentId
      {
         Bus,
         RobotBody,
         RobotTopFrontWheel,
         RobotTopRearWheel,
         RobotBottomFrontWheel,
         RobotBottomRearWheel,
      }

      #endregion

      #region Fields

      private static RobotCommBus instance = null;

      private bool execute;
      private Thread thread;
      private Thread deviceThread;
      private bool ready;

      private BusInterfaces busInterfaceId;
      private bool busReady;
      private string busStatus;
      private Queue busReceiveQueue;
      private Queue deviceResetQueue;

      private UlcRoboticsNicbotBody robotBody;
      private UlcRoboticsNicbotWheel robotTopFrontWheel;
      private UlcRoboticsNicbotWheel robotTopRearWheel;
      private UlcRoboticsNicbotWheel robotBottomFrontWheel;
      private UlcRoboticsNicbotWheel robotBottomRearWheel;

      private ArrayList deviceList;

      private DateTime controllerHeartbeatLimit;
      private bool controllerServiced;

      private int robotSolenoidSetPointChangeCounter;
      private UInt16 robotSolenoidSetPoint;
      private UInt16 robotSolenoidRequested;

      private MovementModes movementMode;
      private MovementForwardModes movementForwardMode;

      /// <summary>
      /// indicator of button press, true when user presses button for motion, false otherwise
      /// </summary>
      private bool movementTriggered;

      private MovementWheelModes movementWheelModeChangeRequest;
      private MovementWheelModes movementWheelModeActual;

      private double movementRequest;
      private double movementManualVelocity;
      private MovementMotorStatus movementTopFrontWheelStatus;
      private MovementMotorStatus movementTopRearWheelStatus;
      private MovementMotorStatus movementBottomFrontWheelStatus;
      private MovementMotorStatus movementBottomRearWheelStatus;

      private double drillFrontOriginOffset;
      private double drillRearOriginOffset;
      private double drillFrontRotationSetPoint;
      private double drillFrontRotationRequested;
      private double drillRearRotationSetPoint;
      private double drillRearRotationRequested;
      private double drillFrontIndexSetPoint;
      private double drillFrontIndexRequested;
      private double drillFrontIndexSpeedRequested;
      private double drillRearIndexSetPoint;
      private double drillRearIndexRequested;
      private double drillRearIndexSpeedRequested;
      private bool drillFrontConfigurationNeeded;
      private bool drillRearConfigurationNeeded;
      private bool drillFrontRetractToLimit;
      private bool drillRearRetractToLimit;
      private bool drillFrontStop;
      private bool drillRearStop;
      private bool drillFrontLaserSetPoint;
      private bool drillFrontLaserRequested;
      private bool drillRearLaserSetPoint;
      private bool drillRearLaserRequested;

      private CameraLocations cameraA;
      private CameraLocations cameraB;
      private int videoASetPoint;
      private int videoARequested;
      private int videoBSetPoint;
      private int videoBRequested;
      private int[] cameraLightLevelSetPoints;
      private int[] cameraLightLevelRequests;

      #endregion

      #region Helper Functions

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

      private UInt16 GetSolenoidMask(Solenoids solenoid)
      {
         UInt16 result = 0;

         if (UlcRoboticsNicbotBody.Modes.repair == this.robotBody.Mode)
         {
            if (Solenoids.frontDrillCover == solenoid)
            {
               result = 0x0004;
            }
            else if (Solenoids.frontNozzleExtend == solenoid)
            {
               result = 0x4000;
            }
            else if (Solenoids.rearDrillCover == solenoid)
            {
               result = 0x0008;
            }
            else if (Solenoids.rearNozzleExtend == solenoid)
            {
               result = 0x8000;
            }
            else if (Solenoids.lowerArmRetract == solenoid)
            {
               result = 0x0002;
            }
            else if (Solenoids.lowerArmExtend == solenoid)
            {
               result = 0x0001;
            }
            else if (Solenoids.rearArmExtend == solenoid)
            {
               result = 0x0040;
            }
            else if (Solenoids.rearArmRetract == solenoid)
            {
               result = 0x0080;
            }
            else if (Solenoids.frontArmExtend == solenoid)
            {
               result = 0x2000;
            }
            else if (Solenoids.frontArmRetract == solenoid)
            {
               result = 0x1000;
            }
         }
         else if (UlcRoboticsNicbotBody.Modes.inspect == this.robotBody.Mode)
         {
            if (Solenoids.sensorRetract == solenoid)
            {
               result = 0x0001;
            }
            else if (Solenoids.sensorExtend == solenoid)
            {
               result = 0x0002;
            }
            else if (Solenoids.sensorArmStow == solenoid)
            {
               result = 0x0004;
            }
            else if (Solenoids.sensorArmDeploy == solenoid)
            {
               result = 0x0008;
            }
            else if (Solenoids.lowerArmRetract == solenoid)
            {
               result = 0x0010;
            }
            else if (Solenoids.lowerArmExtend == solenoid)
            {
               result = 0x0020;
            }
            else if (Solenoids.rearArmExtend == solenoid)
            {
               result = 0x1000;
            }
            else if (Solenoids.rearArmRetract == solenoid)
            {
               result = 0x2000;
            }
            else if (Solenoids.frontArmExtend == solenoid)
            {
               result = 0x4000;
            }
            else if (Solenoids.frontArmRetract == solenoid)
            {
               result = 0x8000;
            }
         }

         return (result);
      }

      /// <summary>
      /// Updates set point and evaluates wheel mode, adjusts wheel mode based on adjusted setpoint.
      /// </summary>
      /// <param name="adjustedSetPoint">adjusted solenoid set point</param>
      private void UpdateSolenoidSetPoint(UInt16 adjustedSolenoidSetPoint)
      {
         this.robotSolenoidSetPointChangeCounter++;

         //MovementWheelModes previousWheelMode = this.GetMovementWheelMode();
         BodyPositions previousBodyPosition = this.GetBodyPosition();

         this.robotSolenoidSetPoint = adjustedSolenoidSetPoint;

         //MovementWheelModes updatedWheelMode = this.GetMovementWheelMode();
         BodyPositions updatedBodyPosition = this.GetBodyPosition();

#if false
         if (updatedWheelMode != previousWheelMode)
         {
            if ((MovementWheelModes.neither == updatedWheelMode) ||
                (MovementWheelModes.both == updatedWheelMode))
            {
               MovementModes movementAction = this.GetMovementMode();

               if (movementAction == MovementModes.move)
               {
                  this.SetMovementMode(MovementModes.off);
               }
            }
            else if (MovementWheelModes.axial == updatedWheelMode)
            {
               MovementForwardModes movementForwardMode = this.GetMovementForwardMode();

               if (MovementForwardModes.circumferential == movementForwardMode)
               {
                  this.SetMovementForwardMode(MovementForwardModes.normalAxial);
               }
            }
            else if (MovementWheelModes.circumferential == updatedWheelMode)
            {
               MovementForwardModes movementForwardMode = this.GetMovementForwardMode();

               if (MovementForwardModes.circumferential != movementForwardMode)
               {
                  this.SetMovementForwardMode(MovementForwardModes.circumferential);
               }
            }
         }
#endif

         if (updatedBodyPosition != previousBodyPosition)
         {
            if (BodyPositions.opened == updatedBodyPosition)
            {
               PumpControl.Front.ResetVolume();
               PumpControl.Rear.ResetVolume();
               SimulatedPumpPressure.Front.CreateCavity();
               SimulatedPumpPressure.Rear.CreateCavity();
            }
         }

         this.robotSolenoidSetPointChangeCounter--;
      }

      private int GetVideoCameraId(CameraLocations camera)
      {
         int videoSetPoint = 0;

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            if (CameraLocations.robotFrontUpperBack == camera)
            {
               videoSetPoint = 2;
            }
            else if (CameraLocations.robotLowerBack == camera)
            {
               videoSetPoint = 12;
            }
            else if (CameraLocations.robotFrontUpperDown == camera)
            {
               videoSetPoint = 3;
            }
            else if (CameraLocations.robotRearUpperForward == camera)
            {
               videoSetPoint = 8;
            }
            else if (CameraLocations.robotRearUpperDown == camera)
            {
               videoSetPoint = 10;
            }
            else if (CameraLocations.robotFffDrill == camera)
            {
               videoSetPoint = 7;
            }
            else if (CameraLocations.robotRearUpperBack == camera)
            {
               videoSetPoint = 9;
            }
            else if (CameraLocations.robotFrontUpperForward == camera)
            {
//5 RRF DRILL
               videoSetPoint = 4;
            }
            else if (CameraLocations.robotRffDrill == camera)
            {
               videoSetPoint = 11;
            }
            else if (CameraLocations.robotLowerForward == camera)
            {
               videoSetPoint = 1;
            }
            else if (CameraLocations.robotFrfDrill == camera)
            {
               videoSetPoint = 6;
            }
            else if (CameraLocations.robotRrfDrill == camera)
            {
               videoSetPoint = 5;
            }
         }
         else if (RobotApplications.inspect == ParameterAccessor.Instance.RobotApplication)
         {
            if (CameraLocations.robotFrontUpperBack == camera)
            {
               videoSetPoint = 1;
            }
            else if (CameraLocations.robotLowerBack == camera)
            {
               videoSetPoint = 2;
            }
            else if (CameraLocations.robotFrontUpperDown == camera)
            {
               videoSetPoint = 3;
            }
            else if (CameraLocations.robotRearUpperForward == camera)
            {
               videoSetPoint = 4;
            }
            else if (CameraLocations.robotRearUpperDown == camera)
            {
               videoSetPoint = 5;
            }
            else if (CameraLocations.robotRearUpperBack == camera)
            {
               videoSetPoint = 7;
            }
            else if (CameraLocations.robotFrontUpperForward == camera)
            {
               videoSetPoint = 8;
            }
            else if (CameraLocations.robotSensorArm == camera)
            {
               videoSetPoint = 9;
            }
            else if (CameraLocations.robotLowerForward == camera)
            {
               videoSetPoint = 10;
            }
            else if (CameraLocations.robotFrfDrill == camera)
            {
               videoSetPoint = 11;
            }
            else if (CameraLocations.robotSensorBay == camera)
            {
               videoSetPoint = 12;
            }
         }

         return (videoSetPoint);
      }

      #endregion

      #region Properties

      public static RobotCommBus Instance
      {
         get
         {
            if (instance == null)
            {
               instance = new RobotCommBus();
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

      public bool Running { get { return (this.execute); } }

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

      private void DeviceError(string name, int nodeId, string reason)
      {
         Tracer.WriteError(TraceGroup.RBUS, "", "fault with \"{0}\", node={1}, reason={2}", name, nodeId, reason);
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

      #region Robot Body Functions 

      private void InitializeRobotBody()
      {
         this.robotBody.Initialize();
         this.robotSolenoidSetPointChangeCounter = 0;
         this.robotSolenoidSetPoint = this.robotBody.GetSolenoidCache();
         this.robotSolenoidRequested = this.robotSolenoidSetPoint;

         this.drillFrontOriginOffset = double.NaN;
         this.drillRearOriginOffset = double.NaN;
         this.drillFrontRotationRequested = 0;
         this.drillRearRotationRequested = 0;
         this.drillFrontIndexRequested = 0;
         this.drillFrontIndexSpeedRequested = 0;
         this.drillRearIndexRequested = 0;
         this.drillRearIndexSpeedRequested = 0;
         this.drillFrontConfigurationNeeded = false;
         this.drillRearConfigurationNeeded = false;
         this.drillFrontRetractToLimit = false;
         this.drillRearRetractToLimit = false;
         this.drillFrontStop = false;
         this.drillRearStop = false;
         this.drillFrontLaserRequested = false;
         this.drillRearLaserRequested = false;

         this.videoARequested = 0;
         this.videoBRequested = 0;

         for (int i = 0; i < this.cameraLightLevelSetPoints.Length; i++)
         {
            this.cameraLightLevelRequests[i] = 0;
         }

      }

      private void StartRobotBody()
      {
         this.robotBody.SetConsumerHeartbeat((UInt16)ParameterAccessor.Instance.RobotBus.ConsumerHeartbeatRate, (byte)ParameterAccessor.Instance.RobotBus.ControllerBusId);
         this.robotBody.SetProducerHeartbeat((UInt16)ParameterAccessor.Instance.RobotBus.ProducerHeartbeatRate);

         UlcRoboticsNicbotBody.Modes robotMode = UlcRoboticsNicbotBody.Modes.repair;

         if (RobotApplications.inspect == ParameterAccessor.Instance.RobotApplication)
         {
            robotMode = UlcRoboticsNicbotBody.Modes.inspect;
         }

         this.robotBody.Configure(robotMode);
         this.robotBody.Start();

         this.robotSolenoidSetPoint = this.robotBody.GetSolenoidCache();
         this.robotSolenoidRequested = this.robotSolenoidSetPoint;

         MovementWheelModes startWheelMode = MovementWheelModes.neither;
         bool lastAxial = this.robotBody.LastAxial;
         bool lastCircumferential = this.robotBody.LastCircumferential;

         if (false != lastAxial)
         {
            startWheelMode = MovementWheelModes.axial;
         }
         else if (false != lastCircumferential)
         {
            startWheelMode = MovementWheelModes.circumferential;
         }

         this.movementWheelModeChangeRequest = startWheelMode;
         this.movementWheelModeActual = startWheelMode;

         Thread.Sleep(50);

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            this.drillFrontConfigurationNeeded = true;
            this.drillRearConfigurationNeeded = true;
         }
      }

      private void UpdateRobotBody()
      {
         bool evaluateSolenoidSetPoint = false;
         UInt16 solenoidSetPoint = 0;

         if (this.videoASetPoint != this.videoARequested)
         {
            this.robotBody.SetVideoASelect((byte)this.videoASetPoint);
            this.videoARequested = this.videoASetPoint;
         }

         if (this.videoBSetPoint != this.videoBRequested)
         {
            this.robotBody.SetVideoBSelect((byte)this.videoBSetPoint);
            this.videoBRequested = this.videoBSetPoint;
         }

         for (int i = 0; i < this.cameraLightLevelSetPoints.Length; i++)
         {
            if (this.cameraLightLevelSetPoints[i] != this.cameraLightLevelRequests[i])
            {
               int level = (255 * this.cameraLightLevelSetPoints[i]) / 100;
               this.robotBody.SetCameraLightLevelt((byte)(i + 1), (byte)level);

               this.cameraLightLevelRequests[i] = this.cameraLightLevelSetPoints[i];
            }
         }

         this.movementWheelModeActual = this.movementWheelModeChangeRequest;

         lock (this)
         {
            if (0 == this.robotSolenoidSetPointChangeCounter)
            {
               evaluateSolenoidSetPoint = true;
               solenoidSetPoint = this.robotSolenoidSetPoint;
            }
         }

         if ((false != evaluateSolenoidSetPoint) &&
             (solenoidSetPoint != this.robotSolenoidRequested))
         {
            this.robotBody.SetSolenoids(solenoidSetPoint);
            this.robotSolenoidRequested = solenoidSetPoint;
         }

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            if (this.drillFrontRotationSetPoint != this.drillFrontRotationRequested)
            {

               this.robotBody.SetFrontDrillSpeed(this.drillFrontRotationSetPoint);
               this.drillFrontRotationRequested = this.drillFrontRotationSetPoint;
            }

            if (this.drillRearRotationSetPoint != this.drillRearRotationRequested)
            {
               this.robotBody.SetRearDrillSpeed(this.drillRearRotationSetPoint);
               this.drillRearRotationRequested = this.drillRearRotationSetPoint;
            }

            if (false != this.drillFrontConfigurationNeeded)
            {
               this.drillFrontIndexSpeedRequested = 0;
               this.robotBody.SetDrillServoAcceleration((UInt32)ParameterAccessor.Instance.FrontDrill.Acceleration.OperationalValue);
               this.robotBody.SetDrillServoErrorLimit((UInt16)ParameterAccessor.Instance.FrontDrill.ErrorLimit.OperationalValue);
               this.robotBody.SetDrillServoProportionalControlConstant((UInt32)ParameterAccessor.Instance.FrontDrill.ProportionalControlConstant.OperationalValue);
               this.robotBody.SetDrillServoIntegralControlConstant((UInt32)ParameterAccessor.Instance.FrontDrill.IntegralControlConstant.OperationalValue);
               this.robotBody.SetDrillServoDerivativeControlConstant((UInt32)ParameterAccessor.Instance.FrontDrill.DerivativeControlConstant.OperationalValue);
               this.drillFrontConfigurationNeeded = false;
            }

            if (false != this.drillRearConfigurationNeeded)
            {
               this.drillRearIndexSpeedRequested = 0;
               this.robotBody.SetDrillServoAcceleration((UInt32)ParameterAccessor.Instance.RearDrill.Acceleration.OperationalValue);
               this.robotBody.SetDrillServoErrorLimit((UInt16)ParameterAccessor.Instance.RearDrill.ErrorLimit.OperationalValue);
               this.robotBody.SetDrillServoProportionalControlConstant((UInt32)ParameterAccessor.Instance.RearDrill.ProportionalControlConstant.OperationalValue);
               this.robotBody.SetDrillServoIntegralControlConstant((UInt32)ParameterAccessor.Instance.RearDrill.IntegralControlConstant.OperationalValue);
               this.robotBody.SetDrillServoDerivativeControlConstant((UInt32)ParameterAccessor.Instance.RearDrill.DerivativeControlConstant.OperationalValue);
               this.drillRearConfigurationNeeded = false;
            }

            if (false != this.drillFrontRetractToLimit)
            {
               this.robotBody.SetFrontDrillRetractToLimit();
               this.drillFrontRetractToLimit = false;
            }

            if (false != this.drillRearRetractToLimit) 
            {
               this.robotBody.SetRearDrillRetractToLimit();
               this.drillRearRetractToLimit = false;
            }

            if (false != this.drillFrontStop)
            {
               this.robotBody.StopFrontDrill();
               this.drillFrontStop = false;
            }

            if (false != this.drillRearStop)
            {
               this.robotBody.StopRearDrill();
               this.drillRearStop = false;
            }

            if (this.drillFrontIndexSetPoint != this.drillFrontIndexRequested)
            {
               double indexerSpeed = 0;

               if (double.IsNaN(this.drillFrontOriginOffset) == false)
               {
                  if (this.drillFrontIndexSetPoint > this.drillFrontOriginOffset)
                  {
                     indexerSpeed = ParameterAccessor.Instance.FrontDrill.CuttingSpeed.OperationalValue;
                  }
                  else
                  {
                     indexerSpeed = ParameterAccessor.Instance.FrontDrill.TravelSpeed.OperationalValue;
                  }
               }
               else
               {
                  indexerSpeed = ParameterAccessor.Instance.FrontDrill.SearchSpeed.OperationalValue;
               }

               if (indexerSpeed != this.drillFrontIndexSpeedRequested)
               {
                  UInt32 velocityCount = (UInt32)(indexerSpeed * ParameterAccessor.Instance.FrontDrill.SpeedToVelocityCount);
                  this.robotBody.SetDrillServoTravelVelocity(velocityCount);
                  this.drillFrontIndexSpeedRequested = indexerSpeed;
               }

               this.robotBody.SetFrontDrillIndex(this.drillFrontIndexSetPoint);
               this.drillFrontIndexRequested = this.drillFrontIndexSetPoint;
            }

            if (this.drillRearIndexSetPoint != this.drillRearIndexRequested)
            {
               double indexerSpeed = 0;

               if (double.IsNaN(this.drillRearOriginOffset) == false)
               {
                  if (this.drillRearOriginOffset > this.drillRearIndexSetPoint)
                  {
                     indexerSpeed = ParameterAccessor.Instance.RearDrill.CuttingSpeed.OperationalValue;
                  }
                  else
                  {
                     indexerSpeed = ParameterAccessor.Instance.RearDrill.TravelSpeed.OperationalValue;
                  }
               }
               else
               {
                  indexerSpeed = ParameterAccessor.Instance.RearDrill.SearchSpeed.OperationalValue;
               }

               if (indexerSpeed != this.drillRearIndexSpeedRequested)
               {
                  UInt32 velocityCount = (UInt32)(indexerSpeed * ParameterAccessor.Instance.RearDrill.SpeedToVelocityCount);
                  this.robotBody.SetDrillServoTravelVelocity(velocityCount);
                  this.drillRearIndexSpeedRequested = indexerSpeed;
               }

               this.robotBody.SetRearDrillIndex(this.drillRearIndexSetPoint);
               this.drillRearIndexRequested = this.drillRearIndexSetPoint;
            }

            if (this.drillFrontLaserSetPoint != this.drillFrontLaserRequested)
            {
               this.robotBody.SetFrontLaser(this.drillFrontLaserSetPoint);
               this.drillFrontLaserRequested = this.drillFrontLaserSetPoint;
            }

            if (this.drillRearLaserSetPoint != this.drillRearLaserRequested)
            {
               this.robotBody.SetRearLaser(this.drillRearLaserSetPoint);
               this.drillRearLaserRequested = this.drillRearLaserSetPoint;
            }
         }

         if (RobotApplications.inspect == ParameterAccessor.Instance.RobotApplication)
         {
         }
      }

      #endregion

      #region Robot Wheels Functions

      private void StartRobotWheel(UlcRoboticsNicbotWheel motor)
      {
         motor.SetConsumerHeartbeat((UInt16)ParameterAccessor.Instance.RobotBus.ConsumerHeartbeatRate, (byte)ParameterAccessor.Instance.RobotBus.ControllerBusId);
         motor.SetProducerHeartbeat((UInt16)ParameterAccessor.Instance.RobotBus.ProducerHeartbeatRate);
         motor.Configure();
         motor.Start();
      }

      private void StartRobotWheels()
      {
         this.StartRobotWheel(this.robotTopFrontWheel);
         this.StartRobotWheel(this.robotTopRearWheel);
         this.StartRobotWheel(this.robotBottomFrontWheel);
         this.StartRobotWheel(this.robotBottomRearWheel);

         Thread.Sleep(50);
      }

      private void EvaluateMovementMotorValue(MovementForwardControls cumulativeForwardControl, UlcRoboticsNicbotWheel motor, MovementMotorParameters parameters, ref double total, ref int count)
      {
         if ((null == motor.FaultReason) &&
             (MotorStates.Enabled == parameters.State) &&
             (MovementModes.move == this.movementMode))
         {
            if (MovementForwardControls.velocity == cumulativeForwardControl)
            {
               MovementForwardControls motorForwordControl = ParameterAccessor.Instance.GetMovementForwardControl(parameters, this.movementForwardMode);

               if (motorForwordControl == cumulativeForwardControl)
               {
                  double motorVelocity = motor.RPM;
                  motorVelocity /= ParameterAccessor.Instance.MovementMotorVelocityToRpm;
                  motorVelocity *= (MotorDirections.Normal == parameters.Direction) ? 1 : -1;
                  total += motorVelocity;
                  count++;
               }
               else
               {
                  double velocityEqualivant = motor.Torque * 1000 / ParameterAccessor.Instance.MovementMotorCurrentPer1kRPM.OperationalValue / ParameterAccessor.Instance.MovementMotorVelocityToRpm;
                  total += velocityEqualivant;
                  count++;
               }
            }
            else
            {
               // NOTE: if any motor is set for velocity then cumulative mode is velocity
               // NOTE: current cumulative mode implies all motors are set for current
               total += motor.Torque;
               count++;
            }
         }
      }

      private bool UpdateRobotWheel(UlcRoboticsNicbotWheel motor, MovementMotorStatus status, MovementMotorParameters parameters)
      {
         bool scheduled = false;

         if (null == motor.FaultReason)
         {
            bool modeChange = false;
            UlcRoboticsNicbotWheel.Modes neededMode = UlcRoboticsNicbotWheel.Modes.undefined;
            double neededValue = 0;

            if (MotorStates.Disabled == parameters.State)
            {
               if (0 == status.requestedVelocity)
               {
                  neededMode = UlcRoboticsNicbotWheel.Modes.off;
                  neededValue = 0;
               }
               else
               {
                  neededMode = UlcRoboticsNicbotWheel.Modes.velocity;
                  neededValue = 0;
               }
            }
            else if (MotorStates.Enabled == parameters.State)
            {
               if (0 != this.movementManualVelocity)
               {
                  neededMode = UlcRoboticsNicbotWheel.Modes.velocity;
                  neededValue = this.movementManualVelocity;
               }
               else if (MovementModes.off == this.movementMode)
               {
                  if (0 == status.requestedVelocity)
                  {
                     neededMode = UlcRoboticsNicbotWheel.Modes.off;
                     neededValue = 0;
                  }
                  else
                  {
                     neededMode = UlcRoboticsNicbotWheel.Modes.velocity;
                     neededValue = 0;
                  }
               }
               else if (MovementModes.move == this.movementMode)
               {
                  MovementForwardControls cumulativeForwardControl = this.GetMovementForwardControl(); // applicable to all motors: velocity or current
                  double setPoint;

                  if (false == this.movementTriggered)
                  {
                     setPoint = 0;
                     neededMode = UlcRoboticsNicbotWheel.Modes.velocity;
                  }
                  else if (MovementForwardControls.velocity == cumulativeForwardControl)
                  {
                     MovementForwardControls motorForwordControl = ParameterAccessor.Instance.GetMovementForwardControl(parameters, this.movementForwardMode);

                     if (motorForwordControl == cumulativeForwardControl)
                     {
                        setPoint = this.movementRequest * ParameterAccessor.Instance.MovementMotorMaxSpeed.OperationalValue;
                        neededMode = UlcRoboticsNicbotWheel.Modes.velocity;
                     }
                     else
                     {
                        double velocitySetPoint = this.movementRequest * ParameterAccessor.Instance.MovementMotorMaxSpeed.OperationalValue;
                        setPoint = velocitySetPoint * ParameterAccessor.Instance.MovementMotorVelocityToRpm * ParameterAccessor.Instance.MovementMotorCurrentPer1kRPM.OperationalValue / 1000;
                        neededMode = UlcRoboticsNicbotWheel.Modes.current;
                     }
                  }
                  else
                  {
                     // NOTE: if any motor is set for velocity then cumulative mode is velocity
                     // NOTE: current cumulative mode implies all motors are set for current
                     setPoint = this.movementRequest * ParameterAccessor.Instance.MovementMotorMaxCurrent.OperationalValue;
                     neededMode = UlcRoboticsNicbotWheel.Modes.current;
                  }

#if false
                  bool offSetpointRequested = false;

                  if ((UlcRoboticsNicbotWheel.Modes.off == status.requestedMode) ||
                      (UlcRoboticsNicbotWheel.Modes.undefined == status.requestedMode) ||
                      ((UlcRoboticsNicbotWheel.Modes.velocity == status.requestedMode) && (0 == status.requestedVelocity)) ||
                      ((UlcRoboticsNicbotWheel.Modes.current == status.requestedMode) && (0 == status.requestedCurrent)))
                  {
                     offSetpointRequested = true;
                  }

                  if ((0 == setPoint) && (false != offSetpointRequested))
                  {
                     neededMode = UlcRoboticsNicbotWheel.Modes.off;
                     neededValue = 0;
                  }
                  else
                  {
                     neededValue = setPoint;
                  }
#endif
                  neededValue = setPoint;
               }
               else if (MovementModes.locked == this.movementMode)
               {
                  neededMode = UlcRoboticsNicbotWheel.Modes.current;
                  neededValue = ParameterAccessor.Instance.MovementMotorLockCurrent.OperationalValue;
               }
            }
            else if (MotorStates.Locked == parameters.State)
            {
               neededMode = UlcRoboticsNicbotWheel.Modes.current;
               neededValue = ParameterAccessor.Instance.MovementMotorLockCurrent.OperationalValue;
            }

            if (neededMode != status.requestedMode)
            {
               motor.SetMode(neededMode);

               status.requestedMode = neededMode;
               modeChange = true;
               Tracer.WriteMedium(TraceGroup.RBUS, null, "{0} mode={1}", motor.Name, neededMode.ToString());
            }

            if (UlcRoboticsNicbotWheel.Modes.velocity == status.requestedMode)
            {
               if ((neededValue != status.requestedVelocity) || (false != modeChange))
               {
                  int settingInversionValue = (MotorDirections.Normal == parameters.Direction) ? 1 : -1;
                  int velocityRpm = (int)(settingInversionValue * neededValue * ParameterAccessor.Instance.MovementMotorVelocityToRpm);
                  motor.ScheduleVelocity(velocityRpm);
                  scheduled = true;

                  status.requestedVelocity = neededValue;
                  Tracer.WriteMedium(TraceGroup.RBUS, null, "{0} velocity={1:0.00} rpm={2}", motor.Name, neededValue, velocityRpm);
               }
            }
            else if (UlcRoboticsNicbotWheel.Modes.current == status.requestedMode)
            {
               if ((neededValue != status.requestedCurrent) || (false != modeChange))
               {
                  int settingInversionValue = (MotorDirections.Normal == parameters.Direction) ? 1 : -1;
                  float torqueCurrent = (float)(settingInversionValue * neededValue);
                  motor.ScheduleTorque(torqueCurrent);
                  scheduled = true;

                  status.requestedCurrent = neededValue;
                  Tracer.WriteMedium(TraceGroup.RBUS, null, "{0} current {1}", motor.Name, torqueCurrent);
               }
            }
            else if (UlcRoboticsNicbotWheel.Modes.off == status.requestedMode)
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

      private void UpdateRobotWheels()
      {
         bool scheduled = false;

         scheduled |= this.UpdateRobotWheel(this.robotTopFrontWheel, this.movementTopFrontWheelStatus, ParameterAccessor.Instance.TopFrontMovementMotor);
         scheduled |= this.UpdateRobotWheel(this.robotTopRearWheel, this.movementTopRearWheelStatus, ParameterAccessor.Instance.TopRearMovementMotor);
         scheduled |= this.UpdateRobotWheel(this.robotBottomFrontWheel, this.movementBottomFrontWheelStatus, ParameterAccessor.Instance.BottomFrontMovementMotor);
         scheduled |= this.UpdateRobotWheel(this.robotBottomRearWheel, this.movementBottomRearWheelStatus, ParameterAccessor.Instance.BottomRearMovementMotor);

         if (false != scheduled)
         {
            PCANLight.SendSync(this.busInterfaceId);
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

         this.robotBody.NodeId = (byte)ParameterAccessor.Instance.RobotBus.RobotBodyBusId;
         this.robotTopFrontWheel.NodeId = (byte)ParameterAccessor.Instance.RobotBus.RobotTopFrontWheelBusId;
         this.robotTopRearWheel.NodeId = (byte)ParameterAccessor.Instance.RobotBus.RobotTopRearWheelBusId;
         this.robotBottomFrontWheel.NodeId = (byte)ParameterAccessor.Instance.RobotBus.RobotBottomFrontWheelBusId;
         this.robotBottomRearWheel.NodeId = (byte)ParameterAccessor.Instance.RobotBus.RobotBottomRearWheelBusId;

         this.TraceMask = ParameterAccessor.Instance.RobotBus.ControllerTraceMask;
         this.robotBody.TraceMask = ParameterAccessor.Instance.RobotBus.RobotBodyTraceMask;
         this.robotTopFrontWheel.TraceMask = ParameterAccessor.Instance.RobotBus.RobotTopFrontWheelTraceMask;
         this.robotTopRearWheel.TraceMask = ParameterAccessor.Instance.RobotBus.RobotTopRearWheelTraceMask;
         this.robotBottomFrontWheel.TraceMask = ParameterAccessor.Instance.RobotBus.RobotBottomFrontWheelTraceMask;
         this.robotBottomRearWheel.TraceMask = ParameterAccessor.Instance.RobotBus.RobotBottomRearWheelTraceMask;

         this.InitializeRobotBody();

         this.movementMode = MovementModes.off;
         this.movementForwardMode = MovementForwardModes.normalAxial;
         this.movementWheelModeChangeRequest = MovementWheelModes.neither;
         this.movementWheelModeActual = MovementWheelModes.neither;
         this.movementTopFrontWheelStatus.Initialize();
         this.movementTopRearWheelStatus.Initialize();
         this.movementBottomFrontWheelStatus.Initialize();
         this.movementBottomRearWheelStatus.Initialize();

         this.drillFrontRotationSetPoint = 0;
         this.drillRearRotationSetPoint = 0;
         this.drillFrontIndexSetPoint = 0;
         this.drillRearIndexSetPoint = 0;
         this.drillFrontLaserSetPoint = false;
         this.drillRearLaserSetPoint = false;

         this.cameraA = CameraLocations.robotFrontUpperBack;
         this.cameraB = CameraLocations.robotLowerBack;
         this.videoASetPoint = 0;
         this.videoBSetPoint = 0;

         for (int i = 0; i < this.cameraLightLevelSetPoints.Length; i++)
         {
            this.cameraLightLevelSetPoints[i] = 0;
         }
      }

      private void StartBus()
      {
         this.busReady = false;

         if (false == this.busReady)
         {
            this.busInterfaceId = ParameterAccessor.Instance.RobotBus.BusInterface;
            CANResult startResult = PCANLight.Start(this.busInterfaceId, ParameterAccessor.Instance.RobotBus.BitRate, FramesType.INIT_TYPE_ST, TraceGroup.RBUS, this.BusReceiveHandler);
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
            Tracer.WriteMedium(TraceGroup.RBUS, "", "bus failure");

            foreach (Device device in this.deviceList)
            {
               if (false == device.ReceiveBootupHeartbeat)
               {
                  device.Fault("interface not ready");
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

               if (BusComponentId.RobotBody == id)
               {
                  this.robotBody.Initialize();
                  this.InitializeRobotBody();
                  this.robotBody.Reset();
                  this.WaitDeviceHeartbeat(this.robotBody);
                  this.StartRobotBody();
               }
               else if (BusComponentId.RobotTopFrontWheel == id)
               {
                  this.robotTopFrontWheel.Initialize();
                  this.robotTopFrontWheel.Reset();
                  this.WaitDeviceHeartbeat(this.robotTopFrontWheel);
                  this.StartRobotWheel(this.robotTopFrontWheel);
               }
               else if (BusComponentId.RobotTopRearWheel == id)
               {
                  this.robotTopRearWheel.Initialize();
                  this.robotTopRearWheel.Reset();
                  this.WaitDeviceHeartbeat(this.robotTopRearWheel);
                  this.StartRobotWheel(this.robotTopRearWheel);
               }
               else if (BusComponentId.RobotBottomFrontWheel == id)
               {
                  this.robotBottomFrontWheel.Initialize();
                  this.robotBottomFrontWheel.Reset();
                  this.WaitDeviceHeartbeat(this.robotBottomFrontWheel);
                  this.StartRobotWheel(this.robotBottomFrontWheel);
               }
               else if (BusComponentId.RobotBottomRearWheel == id)
               {
                  this.robotBottomRearWheel.Initialize();
                  this.robotBottomRearWheel.Reset();
                  this.WaitDeviceHeartbeat(this.robotBottomRearWheel);
                  this.StartRobotWheel(this.robotBottomRearWheel);
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
         this.ready = true;

         this.controllerServiced = true;
         this.controllerHeartbeatLimit = DateTime.Now.AddMilliseconds(ParameterAccessor.Instance.RobotBus.ProducerHeartbeatRate);

         this.StartRobotBody();
         this.StartRobotWheels();

         for (; this.execute; )
         {
            this.UpdateRobotBody();
            this.UpdateRobotWheels();
            this.UpdateDeviceReset();

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
         ParameterAccessor.Instance.RobotBus.RobotBodyTraceMask = this.robotBody.TraceMask;
         ParameterAccessor.Instance.RobotBus.RobotTopFrontWheelTraceMask = this.robotTopFrontWheel.TraceMask;
         ParameterAccessor.Instance.RobotBus.RobotTopRearWheelTraceMask = this.robotTopRearWheel.TraceMask;
         ParameterAccessor.Instance.RobotBus.RobotBottomFrontWheelTraceMask = this.robotBottomFrontWheel.TraceMask;
         ParameterAccessor.Instance.RobotBus.RobotBottomRearWheelTraceMask = this.robotBottomRearWheel.TraceMask;
      }

      private void Process()
      {
         try
         {
            Tracer.WriteHigh(TraceGroup.RBUS, null, "start");

            this.InitializeValues();

            this.deviceThread = new Thread(this.DeviceProcess);
            this.deviceThread.IsBackground = true;
            this.deviceThread.Name = "Robot CAN Devices";
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
            this.CloseBus();

            this.deviceThread.Join(3000);
            this.deviceThread = null;

            this.InitializeValues(); // clears previous session requests for next session
         }
         catch (Exception postException)
         {
            Tracer.WriteError(TraceGroup.RBUS, null, "post process exception {0}", postException.Message);
         }

         this.ready = false;
         Tracer.WriteHigh(TraceGroup.RBUS, null, "stop");
      }

      #endregion

      #region Constructor

      private void Initialize()
      {
         this.busReceiveQueue = new Queue();
         this.deviceResetQueue = new Queue();

         this.robotBody = new UlcRoboticsNicbotBody("nicbot body", (byte)ParameterAccessor.Instance.RobotBus.RobotBodyBusId);
         this.robotTopFrontWheel = new UlcRoboticsNicbotWheel("nicbot tf-wheel", (byte)ParameterAccessor.Instance.RobotBus.RobotTopFrontWheelBusId);
         this.robotTopRearWheel = new UlcRoboticsNicbotWheel("nicbot tr-wheel", (byte)ParameterAccessor.Instance.RobotBus.RobotTopRearWheelBusId);
         this.robotBottomFrontWheel = new UlcRoboticsNicbotWheel("nicbot bf-wheel", (byte)ParameterAccessor.Instance.RobotBus.RobotBottomFrontWheelBusId);
         this.robotBottomRearWheel = new UlcRoboticsNicbotWheel("nicbot br-wheel", (byte)ParameterAccessor.Instance.RobotBus.RobotBottomRearWheelBusId);

         this.deviceList = new ArrayList();
         this.deviceList.Add(this.robotBody);
         this.deviceList.Add(this.robotTopFrontWheel);
         this.deviceList.Add(this.robotTopRearWheel);
         this.deviceList.Add(this.robotBottomFrontWheel);
         this.deviceList.Add(this.robotBottomRearWheel);

         foreach (Device device in this.deviceList)
         {
            device.OnReceiveTrace = new Device.ReceiveTraceHandler(this.DeviceTraceReceive);
            device.OnTransmitTrace = new Device.TransmitTraceHandler(this.DeviceTraceTransmit);
            device.OnTransmit = new Device.TransmitHandler(this.DeviceTransmit);
            device.OnCommError = new Device.CommErrorHandler(this.DeviceError);
         }

         this.movementTopFrontWheelStatus = new MovementMotorStatus();
         this.movementTopRearWheelStatus = new MovementMotorStatus();
         this.movementBottomFrontWheelStatus = new MovementMotorStatus();
         this.movementBottomRearWheelStatus = new MovementMotorStatus();

         this.cameraLightLevelSetPoints = new int[12];
         this.cameraLightLevelRequests = new int[12];
      }

      private RobotCommBus()
      {
      }

      #endregion

      #region Access Functions

      #region Control Functions

      public void Start()
      {
         this.thread = new Thread(this.Process);
         this.thread.IsBackground = true;
         this.thread.Name = "Robot Comm";

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

      /// <summary>
      /// Get commutative status of every on the bus. 
      /// </summary>
      /// <remarks>Each id supported is expected to be evaluated.</remarks>
      /// <returns>null when ok, string with description when faulted</returns>
      public string GetStatus()
      {
         string result = null;

         if (this.GetStatus(RobotCommBus.BusComponentId.Bus) != null)
         {
            result = "robot communication offline";
         }
         else if (this.GetStatus(RobotCommBus.BusComponentId.RobotBody) != null)
         {
            result = "robot body offline";
         }
         else if (this.GetStatus(RobotCommBus.BusComponentId.RobotTopFrontWheel) != null)
         {
            result = "robot top front wheel offline";
         }
         else if (this.GetStatus(RobotCommBus.BusComponentId.RobotTopRearWheel) != null)
         {
            result = "robot top rear wheel offline";
         }
         else if (this.GetStatus(RobotCommBus.BusComponentId.RobotBottomFrontWheel) != null)
         {
            result = "robot bottom front wheel offline";
         }
         else if (this.GetStatus(RobotCommBus.BusComponentId.RobotBottomRearWheel) != null)
         {
            result = "robot bottom rear wheel offline";
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
               result = (false != this.Running) ? this.busStatus : "off";
               break;
            }
            case BusComponentId.RobotBody:
            {
               result = (false != this.Running) ? this.robotBody.FaultReason : "off";
               break;
            }
            case BusComponentId.RobotTopFrontWheel:
            {
               result = (false != this.Running) ? this.robotTopFrontWheel.FaultReason : "off";
               break;
            }
            case BusComponentId.RobotTopRearWheel:
            {
               result = (false != this.Running) ? this.robotTopRearWheel.FaultReason : "off";
               break;
            }
            case BusComponentId.RobotBottomFrontWheel:
            {
               result = (false != this.Running) ? this.robotBottomFrontWheel.FaultReason : "off";
               break;
            }
            case BusComponentId.RobotBottomRearWheel:
            {
               result = (false != this.Running) ? this.robotBottomRearWheel.FaultReason : "off";
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
            case BusComponentId.RobotBody:
            {
               result = this.robotBody;
               break;
            }
            case BusComponentId.RobotTopFrontWheel:
            {
               result = this.robotTopFrontWheel;
               break;
            }
            case BusComponentId.RobotTopRearWheel:
            {
               result = this.robotTopRearWheel;
               break;
            }
            case BusComponentId.RobotBottomFrontWheel:
            {
               result = this.robotBottomFrontWheel;
               break;
            }
            case BusComponentId.RobotBottomRearWheel:
            {
               result = this.robotBottomRearWheel;
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

      #region Robot Solenoid Functions (body, wheels, drill cover, nozzle)

      /// <summary>
      /// Used to set one solenoid at a time.
      /// </summary>
      /// <remarks>      
      /// Configuring wheel solenoids to either both off or both on when action is move has the action set to off.
      /// Configuring wheel solenoids to axial when movement mode is circumferential has the mode set to normal axial.
      /// Configuring wheel solenoids to circumferential when movement mode is not circumferential has the mode set to circumfrential.
      /// </remarks>
      /// <param name="solenoid">solenoid to set</param>
      /// <param name="activate">true to turn on, false to turn off</param>
      public void SetSolenoid(Solenoids solenoid, bool activate)
      {
         UInt16 mask = this.GetSolenoidMask(solenoid);
         UInt16 adjustedSolenoidStatus = this.robotSolenoidSetPoint;

         if (false != activate)
         {
            adjustedSolenoidStatus |= mask;
         }
         else
         {
            adjustedSolenoidStatus &= (UInt16)~(mask);
         }

         this.UpdateSolenoidSetPoint(adjustedSolenoidStatus);
      }

      public void SetBodyPosition(BodyPositions bodyPosition)
      {
         UInt16 adjustedSolenoidStatus = this.robotSolenoidSetPoint;

         if (BodyPositions.off == bodyPosition)
         {
            adjustedSolenoidStatus = 0;
         }
         else if (BodyPositions.closed == bodyPosition)
         {
            adjustedSolenoidStatus &= (UInt16)~(this.GetSolenoidMask(Solenoids.frontDrillCover));
            adjustedSolenoidStatus &= (UInt16)~(this.GetSolenoidMask(Solenoids.rearDrillCover));
            adjustedSolenoidStatus &= (UInt16)~(this.GetSolenoidMask(Solenoids.frontNozzleExtend));
            adjustedSolenoidStatus &= (UInt16)~(this.GetSolenoidMask(Solenoids.rearNozzleExtend));
            adjustedSolenoidStatus &= (UInt16)~(this.GetSolenoidMask(Solenoids.frontArmExtend));
            adjustedSolenoidStatus &= (UInt16)~(this.GetSolenoidMask(Solenoids.rearArmExtend));
            adjustedSolenoidStatus &= (UInt16)~(this.GetSolenoidMask(Solenoids.lowerArmExtend));
            adjustedSolenoidStatus |= this.GetSolenoidMask(Solenoids.frontArmRetract);
            adjustedSolenoidStatus |= this.GetSolenoidMask(Solenoids.rearArmRetract);
            adjustedSolenoidStatus |= this.GetSolenoidMask(Solenoids.lowerArmRetract);
         }
         else if (BodyPositions.opened == bodyPosition)
         {
            adjustedSolenoidStatus |= this.GetSolenoidMask(Solenoids.frontArmExtend);
            adjustedSolenoidStatus |= this.GetSolenoidMask(Solenoids.rearArmExtend);
            adjustedSolenoidStatus |= this.GetSolenoidMask(Solenoids.lowerArmExtend);
            adjustedSolenoidStatus &= (UInt16)~(this.GetSolenoidMask(Solenoids.frontArmRetract));
            adjustedSolenoidStatus &= (UInt16)~(this.GetSolenoidMask(Solenoids.rearArmRetract));
            adjustedSolenoidStatus &= (UInt16)~(this.GetSolenoidMask(Solenoids.lowerArmRetract));
         }
         else if (BodyPositions.frontLoose == bodyPosition)
         {
            adjustedSolenoidStatus &= (UInt16)~(this.GetSolenoidMask(Solenoids.frontArmExtend));
            adjustedSolenoidStatus |= this.GetSolenoidMask(Solenoids.rearArmExtend);
            adjustedSolenoidStatus |= this.GetSolenoidMask(Solenoids.lowerArmExtend);
            adjustedSolenoidStatus &= (UInt16)~(this.GetSolenoidMask(Solenoids.frontArmRetract));
            adjustedSolenoidStatus &= (UInt16)~(this.GetSolenoidMask(Solenoids.rearArmRetract));
            adjustedSolenoidStatus &= (UInt16)~(this.GetSolenoidMask(Solenoids.lowerArmRetract));
         }
         else if (BodyPositions.rearLoose == bodyPosition)
         {
            adjustedSolenoidStatus |= this.GetSolenoidMask(Solenoids.frontArmExtend);
            adjustedSolenoidStatus &= (UInt16)~(this.GetSolenoidMask(Solenoids.rearArmExtend));
            adjustedSolenoidStatus |= this.GetSolenoidMask(Solenoids.lowerArmExtend);
            adjustedSolenoidStatus &= (UInt16)~(this.GetSolenoidMask(Solenoids.frontArmRetract));
            adjustedSolenoidStatus &= (UInt16)~(this.GetSolenoidMask(Solenoids.rearArmRetract));
            adjustedSolenoidStatus &= (UInt16)~(this.GetSolenoidMask(Solenoids.lowerArmRetract));
         }
         else if (BodyPositions.drill == bodyPosition)
         {
            adjustedSolenoidStatus |= this.GetSolenoidMask(Solenoids.frontArmExtend);
            adjustedSolenoidStatus |= this.GetSolenoidMask(Solenoids.rearArmExtend);
            adjustedSolenoidStatus &= (UInt16)~(this.GetSolenoidMask(Solenoids.lowerArmExtend));
            adjustedSolenoidStatus &= (UInt16)~(this.GetSolenoidMask(Solenoids.frontArmRetract));
            adjustedSolenoidStatus &= (UInt16)~(this.GetSolenoidMask(Solenoids.rearArmRetract));
            adjustedSolenoidStatus |= this.GetSolenoidMask(Solenoids.lowerArmRetract);
         }

         this.UpdateSolenoidSetPoint(adjustedSolenoidStatus);
      }

      /// <summary>
      /// Function to extend or retract nozzle.
      /// </summary>
      /// <remarks>
      /// Alters nozzle when drill is out of the way.
      /// </remarks>
      /// <param name="location">nozzle to control</param>
      /// <param name="extended">true to extend, false to retract</param>
      public void SetNozzleExtend(ToolLocations location, bool extended)
      {
         double drillPosition = this.GetDrillIndexPosition(location);
         
         if (2 >= drillPosition) 
         {
            Solenoids solenoid = (ToolLocations.front == location) ? Solenoids.frontNozzleExtend : Solenoids.rearNozzleExtend;
            this.SetSolenoid(solenoid, extended);
         }
      }

      /// <summary>
      /// Used to return state of solenoid.
      /// </summary>
      /// <remarks>Result based on set point.</remarks>
      /// <param name="solenoid">Solenoid to query</param>
      /// <returns>true if on, false if off</returns>
      public bool GetSolenoidActive(Solenoids solenoid)
      {
         UInt16 mask = this.GetSolenoidMask(solenoid);
         bool result = (0 != (this.robotSolenoidSetPoint & mask)) ? true : false;
         return (result);
      }

      public double GetRobotPitch()
      {
         return (this.robotBody.Pitch);
      }

      public double GetRobotRoll()
      {
         return (this.robotBody.Roll);
      }

      public BodyPositions GetBodyPosition()
      {
         bool frontDrillOpen = this.GetSolenoidActive(Solenoids.frontDrillCover);
         bool rearDrillOpen = this.GetSolenoidActive(Solenoids.rearDrillCover);
         bool frontNozzleExtended = this.GetSolenoidActive(Solenoids.frontNozzleExtend);
         bool rearNozzleExtended = this.GetSolenoidActive(Solenoids.rearNozzleExtend);

         bool frontArmExtended = this.GetSolenoidActive(Solenoids.frontArmExtend);
         bool rearArmExtended = this.GetSolenoidActive(Solenoids.rearArmExtend);
         bool lowerArmExtended = this.GetSolenoidActive(Solenoids.lowerArmExtend);

         bool frontArmRetracted = this.GetSolenoidActive(Solenoids.frontArmRetract);
         bool rearArmRetracted = this.GetSolenoidActive(Solenoids.rearArmRetract);
         bool lowerArmRetracted = this.GetSolenoidActive(Solenoids.lowerArmRetract);

         BodyPositions result = BodyPositions.manual;

         if ((false == frontDrillOpen) &&
             (false == rearDrillOpen) &&
             (false == frontNozzleExtended) &&
             (false == rearNozzleExtended) &&
             (false == frontArmExtended) &&
             (false == rearArmExtended) &&
             (false == lowerArmExtended) &&
             (false == frontArmRetracted) &&
             (false == rearArmRetracted) &&
             (false == lowerArmRetracted))
         {
            result = BodyPositions.off;
         }
         else if ((false == frontDrillOpen) &&
                  (false == rearDrillOpen) &&
                  (false == frontNozzleExtended) &&
                  (false == rearNozzleExtended) &&
                  (false == frontArmExtended) &&
                  (false == rearArmExtended) &&
                  (false == lowerArmExtended) &&
                  (false != frontArmRetracted) &&
                  (false != rearArmRetracted) &&
                  (false != lowerArmRetracted))
         {
            result = BodyPositions.closed;
         }
         else if ((false != frontArmExtended) &&
                  (false != rearArmExtended) &&
                  (false != lowerArmExtended) &&
                  (false == frontArmRetracted) &&
                  (false == rearArmRetracted) &&
                  (false == lowerArmRetracted))
         {
            result = BodyPositions.opened;
         }
         else if ((false == frontArmExtended) &&
                  (false != rearArmExtended) &&
                  (false != lowerArmExtended) &&
                  (false == frontArmRetracted) &&
                  (false == rearArmRetracted) &&
                  (false == lowerArmRetracted))
         {
            result = BodyPositions.frontLoose;
         }
         else if ((false != frontArmExtended) &&
                  (false == rearArmExtended) &&
                  (false != lowerArmExtended) &&
                  (false == frontArmRetracted) &&
                  (false == rearArmRetracted) &&
                  (false == lowerArmRetracted))
         {
            result = BodyPositions.rearLoose;
         }
         else if ((false != frontArmExtended) &&
                  (false != rearArmExtended) &&
                  (false == lowerArmExtended) &&
                  (false == frontArmRetracted) &&
                  (false == rearArmRetracted) &&
                  (false != lowerArmRetracted))
         {
            result = BodyPositions.drill;
         }          

         return (result);
      }

      public MovementWheelModes GetMovementWheelMode()
      {
         MovementWheelModes result = MovementWheelModes.neither;

         if (MovementWheelModes.neither != this.movementWheelModeActual)
         {
            result = this.movementWheelModeActual;
         }
         else if (MovementWheelModes.neither != this.movementWheelModeChangeRequest)
         {
            result = this.movementWheelModeChangeRequest;
         }
#if false
         MovementWheelModes startWheelMode = MovementWheelModes.neither;
         bool lastAxial = this.robotBody.LastAxial;
         bool lastCircumferential = this.robotBody.LastCircumferential;

         if (false != lastAxial)
         {
            startWheelMode = MovementWheelModes.axial;
         }
         else if (false != lastCircumferential)
         {
            startWheelMode = MovementWheelModes.circumferential;
         }

         this.movementWheelModeChangeRequest = startWheelMode;
         this.movementWheelModeActual = startWheelMode;

         this.movementWheelModeActual
         GetMovementWheelMode

         MovementWheelModes result = MovementWheelModes.neither;
         bool lastAxial = this.robotBody.LastAxial;
         bool lastCircumferential = this.robotBody.LastCircumferential;

         if (false != lastAxial)
         {
            result = MovementWheelModes.axial;
         }
         else if (false != lastCircumferential)
         {
            result = MovementWheelModes.circumferential;
         }
#endif
         return (result);
#if false
         bool wheelAxial = this.GetSolenoidActive(Solenoids.wheelAxial);
         bool wheelCircumferential = this.GetSolenoidActive(Solenoids.wheelCircumferential);
         MovementWheelModes result = MovementWheelModes.neither;

         if ((false == wheelAxial) && (false == wheelCircumferential))
         {
            result = MovementWheelModes.neither;
         }
         else if ((false == wheelAxial) && (false != wheelCircumferential))
         {
            result = MovementWheelModes.circumferential;
         }
         else if ((false != wheelAxial) && (false == wheelCircumferential))
         {
            result = MovementWheelModes.axial;
         }
         else
         {
            result = MovementWheelModes.both;
         }

         return (result);
#endif
      }

      public bool GetNozzleExtended(ToolLocations location)
      {
         Solenoids solenoid = (ToolLocations.front == location) ? Solenoids.frontNozzleExtend : Solenoids.rearNozzleExtend;
         bool result = this.GetSolenoidActive(solenoid);
         return (result);
      }

      public bool GetTopFrontReadyToLock()
      {
         return (this.robotBody.TopFrontReadyToLock);
      }

      public bool GetTopRearReadyToLock()
      {
         return (this.robotBody.TopRearReadyToLock);
      }

      public bool GetBottomFrontReadyToLock()
      {
         return (this.robotBody.BottomFrontReadyToLock);
      }

      public bool GetBottomRearReadyToLock()
      {
         return (this.robotBody.BottomRearReadyToLock);
      }

      #endregion

      #region Robot Movement Functions

      public void SetMovementMode(MovementModes mode)
      {
         Tracer.WriteHigh(TraceGroup.TBUS, "", "requested movement mode={0}", mode);
         this.movementMode = mode;
      }

      public void SetMovementForwardMode(MovementForwardModes mode)
      {
         if (mode == MovementForwardModes.circumferential)
         {
            this.movementWheelModeChangeRequest = MovementWheelModes.circumferential;
         }
         else
         {
            this.movementWheelModeChangeRequest = MovementWheelModes.axial;
         }

         this.movementForwardMode = mode;
      }

      public void SetMovementRequest(double request, bool triggered)
      {
         this.movementRequest = request;
         this.movementTriggered = triggered;
      }

      public void SetMovementManualVelocity(double manualVelocity)
      {
         this.movementManualVelocity = manualVelocity;
      }

      public void SetMovementMotorDistance(double distance)
      {
         Tracer.WriteHigh(TraceGroup.COMM, null, "movement distance {0}", distance);
      }

      public bool GetMovementActivated()
      {
         bool result = ((false != this.movementTriggered) && (MovementModes.move == this.movementMode)) ? true : false;
         return (result);
      }

      public MovementModes GetMovementMode()
      {
         return (this.movementMode);
      }

      public MovementForwardModes GetMovementForwardMode()
      {
         return (this.movementForwardMode);
      }

      public MovementForwardControls GetMovementForwardControl()
      {
         bool allCurrent = true;

         for (int i = 0; i < 4; i++)
         {
            MovementMotorParameters motorParams = ParameterAccessor.Instance.GetMovementMotionParameters(i);
            MovementForwardControls forwordControl = ParameterAccessor.Instance.GetMovementForwardControl(motorParams, this.movementForwardMode);

            if ((motorParams.State == MotorStates.Enabled) && (forwordControl == MovementForwardControls.velocity))
            {
               allCurrent = false;
               break;
            }
         }

         MovementForwardControls result = (false == allCurrent) ? MovementForwardControls.velocity : MovementForwardControls.current;
         return (result);
      }

      public void GetMovementRequestValues(ref ValueParameter movementParameter, ref double movementRequestValue)
      {
         MovementForwardControls forwardControl = this.GetMovementForwardControl();
         movementParameter = (MovementForwardControls.velocity == forwardControl) ? ParameterAccessor.Instance.MovementMotorMaxSpeed : ParameterAccessor.Instance.MovementMotorMaxCurrent;

         movementRequestValue = this.movementRequest * movementParameter.OperationalValue;
      }

      public double GetMovementValue()
      {
         double result = 0;
         int count = 0;

         MovementForwardControls cumulativeForwardControl = this.GetMovementForwardControl(); // applicable to all motors: velocity or current

         this.EvaluateMovementMotorValue(cumulativeForwardControl, this.robotTopFrontWheel, ParameterAccessor.Instance.TopFrontMovementMotor, ref result, ref count);
         this.EvaluateMovementMotorValue(cumulativeForwardControl, this.robotTopRearWheel, ParameterAccessor.Instance.TopRearMovementMotor, ref result, ref count);
         this.EvaluateMovementMotorValue(cumulativeForwardControl, this.robotBottomFrontWheel, ParameterAccessor.Instance.BottomFrontMovementMotor, ref result, ref count);
         this.EvaluateMovementMotorValue(cumulativeForwardControl, this.robotBottomRearWheel, ParameterAccessor.Instance.BottomRearMovementMotor, ref result, ref count);

         if (0 != count)
         {
            result /= count;
         }

         return (result);
      }

      public double GetTopFrontMovementCurrent()
      {
         return (this.robotTopFrontWheel.Torque);
      }

      public double GetTopRearMovementCurrent()
      {
         return (this.robotTopRearWheel.Torque);
      }

      public double GetBottomFrontMovementCurrent()
      {
         return (this.robotBottomFrontWheel.Torque);
      }

      public double GetBottomRearMovementCurrent()
      {
         return (this.robotBottomRearWheel.Torque);
      }

      public double GetTopFrontMovementTemperature()
      {
         return (this.robotTopFrontWheel.Temperature);
      }

      public double GetTopRearMovementTemperature()
      {
         return (this.robotTopRearWheel.Temperature);
      }

      public double GetBottomFrontMovementTemperature()
      {
         return (this.robotBottomFrontWheel.Temperature);
      }

      public double GetBottomRearMovementTemperature()
      {
         return (this.robotBottomRearWheel.Temperature);
      }

      #endregion

      #region Drill Functions

      public void ConfigureDrillServo(ToolLocations location)
      {
         if (ToolLocations.front == location)
         {
            this.drillFrontConfigurationNeeded = true;
         }
         else
         {
            this.drillRearConfigurationNeeded = true;
         }
      }

      public void SetDrillRotationSpeed(ToolLocations location, double setPoint)
      {
         if (ToolLocations.front == location)
         {
            this.drillFrontRotationSetPoint = setPoint;
         }
         else
         {
            this.drillRearRotationSetPoint = setPoint;
         }
      }

      public void RetractDrillToLimit(ToolLocations location)
      {
         if (ToolLocations.front == location)
         {
            this.drillFrontRetractToLimit = true;
         }
         else
         {
            this.drillRearRetractToLimit = true;
         }
      }

      public void StopDrill(ToolLocations location)
      {
         if (ToolLocations.front == location)
         {
            this.drillFrontStop = true;
         }
         else
         {
            this.drillRearStop = true;
         }
      }

      public void SetDrillIndexSetPoint(ToolLocations location, double setPoint)
      {
         if (ToolLocations.front == location)
         {
            this.drillFrontIndexSetPoint = setPoint;
         }
         else
         {
            this.drillRearIndexSetPoint = setPoint;
         }
      }

      public void SetDrillOriginOffset(ToolLocations location)
      {
         if (ToolLocations.front == location)
         {
            this.drillFrontOriginOffset = this.robotBody.FrontDrillIndex;
            this.drillFrontIndexSetPoint = this.drillFrontOriginOffset;
         }
         else
         {
            this.drillRearOriginOffset = this.robotBody.RearDrillIndex;
            this.drillRearIndexSetPoint = this.drillRearOriginOffset;
         }
      }

      public void SetLaserSight(ToolLocations location, bool sightOn)
      {
         if (ToolLocations.front == location)
         {
            this.drillFrontLaserSetPoint = sightOn;
         }
         else
         {
            this.drillRearLaserSetPoint = sightOn;
         }
      }

      public double GetDrillIndexSetPoint(ToolLocations location)
      {
         double result = 0;

         if (ToolLocations.front == location)
         {
            result = this.drillFrontIndexSetPoint;
         }
         else
         {
            result = this.drillRearIndexSetPoint;
         }

         return (result);
      }

      public double GetDrillIndexPosition(ToolLocations location)
      {
         double result = 0;

         if (ToolLocations.front == location)
         {
            result = this.robotBody.FrontDrillIndex;
         }
         else
         {
            result = this.robotBody.RearDrillIndex;
         }

         return (result);
      }

      public double GetDrillOriginOffset(ToolLocations location)
      {
         double result = 0;

         if (ToolLocations.front == location)
         {
            result = this.drillFrontOriginOffset;
         }
         else
         {
            result = this.drillRearOriginOffset;
         }

         return (result);
      }

      public double GetDrillRotationSpeed(ToolLocations location)
      {
         double result = 0;

         if (ToolLocations.front == location)
         {
            result = this.robotBody.FrontDrillSpeed;
         }
         else
         {
            result = this.robotBody.RearDrillSpeed;
         }

         return (result);
      }

      public bool GetDrillError(ToolLocations location)
      {
         bool result = false;

         if (ToolLocations.front == location)
         {
            result = this.robotBody.FrontDrillError;
         }
         else
         {
            result = this.robotBody.RearDrillError;
         }

         return (result);
      }

      public bool GetLaserSight(ToolLocations location)
      {
         bool result = false;

         if (ToolLocations.front == location)
         {
            result = this.drillFrontLaserSetPoint;
         }
         else
         {
            result = this.drillRearLaserSetPoint;
         }

         return (result);
      }

      #endregion

      #region Camera Functions

      public void SetRobotCameraA(CameraLocations camera)
      {
         this.cameraA = camera; 
         this.videoASetPoint = this.GetVideoCameraId(camera);
      }

      public void SetRobotCameraB(CameraLocations camera)
      {
         this.cameraB = camera;
         this.videoBSetPoint = this.GetVideoCameraId(camera);
      }

      public void SetCameraLightLevel(CameraLocations camera, int level)
      {
         int cameraIndex = this.GetVideoCameraId(camera);

         if (cameraIndex > 0)
         {
            this.cameraLightLevelSetPoints[cameraIndex - 1] = level;
         }
      }

      public CameraLocations GetRobotCameraA()
      {
         return (this.cameraA);
      }

      public CameraLocations GetRobotCameraB()
      {
         return (this.cameraB);
      }

      #endregion

      #endregion
   }
}
