
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

   public class NicBotComm
   {
      #region Definitions


      #endregion

      #region Fields 

      private static NicBotComm instance = null;

      private bool execute;
      private Thread thread;
      private bool ready;
      private bool running;

      private int pipePosition;

      private DrillAutoStates drillAutoState;
      private DrillStatus frontDrill;
      private DrillStatus rearDrill;

      #endregion

      #region Properties

      public static NicBotComm Instance
      {
         get
         {
            if (instance == null)
            {
               instance = new NicBotComm();
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

      public int PipePosition
      {
         get
         {
            return (this.pipePosition);
         }      
      }


      #endregion

      #region Helper Functions

#if false
      private MovementMotorParameters GetMovementMotor(MovementMotor motor)
      {
         MovementMotorParameters result = this.topFrontMovementMotor;

         if (MovementMotor.TopFront == motor)
         {
            result = this.topFrontMovementMotor;
         }
         else if (MovementMotor.TopRear == motor)
         {
            result = this.topRearMovementMotor;
         }
         else if (MovementMotor.BottomFront == motor)
         {
            result = this.bottomFrontMovementMotor;
         }
         else // BottomRear
         {
            result = this.bottomRearMovementMotor;
         }

         return (result);
      }
#endif

      private DrillStatus GetDrillStatus(ToolLocations location)
      {
         DrillStatus result = (location == ToolLocations.front) ? this.frontDrill : this.rearDrill;
         return (result);
      }

      #endregion

      #region Process 

      private void InitializeValues()
      {
         this.pipePosition = 0;

         this.drillAutoState = DrillAutoStates.off;
         //this.frontDrill.IndexPosition = 0;
         //this.frontDrill.IndexSetPoint = 0;
         this.frontDrill.OriginRequired = true;
         this.frontDrill.OriginSetPoint = -1;
         this.frontDrill.RotationSpeed = 0;

         //this.rearDrill.IndexPosition = 0;
         //this.rearDrill.IndexSetPoint = 0;
         this.rearDrill.OriginRequired = true;
         this.rearDrill.OriginSetPoint = -1;
         this.rearDrill.RotationSpeed = 0;
      }

      private void ExecuteProcessLoop()
      {
         this.ready = true;
         this.running = true;

         SimulatedPumpPressure.Front.CreateCavity();
         SimulatedPumpPressure.Rear.CreateCavity();
         PumpControl.Front.ResetVolume();
         PumpControl.Rear.ResetVolume();

         for (; this.execute; )
         {
            Thread.Sleep(100);
         }
      }
            
      private void Process()
      {
         try
         {
            Tracer.WriteHigh(TraceGroup.COMM, null, "start");

            this.InitializeValues();

            TruckCommBus.Instance.Start();

            if (false != ParameterAccessor.Instance.EnableRobotBus)
            {
               RobotCommBus.Instance.Start();
            }

            this.ExecuteProcessLoop();
         }
         catch (Exception preException)
         {
            Tracer.WriteError(TraceGroup.COMM, null, "process exception {0}", preException.Message);
         }

         try
         {
            TruckCommBus.Instance.Stop();
            RobotCommBus.Instance.Stop();
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

      private NicBotComm()
      {
      }

      #endregion

      #region Access Methods

      #region Control Functions

      private void Initialize()
      {
         this.frontDrill = new DrillStatus();
         this.rearDrill = new DrillStatus();
      }

      public void Start()
      {         
         this.thread = new Thread(this.Process);
         this.thread.IsBackground = true;
         this.thread.Name = "NICBOT Comm";

         this.ready = false;
         this.execute = true;
         this.thread.Start();
      }

      public void Stop()
      {
         this.execute = false;
      }

      public string GetStatus()
      {
         string result = null;

         if (null == result)
         {
            result = RobotCommBus.Instance.GetStatus();
         }

         if (null == result)
         {
            result = TruckCommBus.Instance.GetStatus();
         }

         return(result);
      }

      #endregion

      #region Main Air Functions

      public double GetNitrogenPressureReading1()
      {
         return (TruckCommBus.Instance.GetNitrogenPressureReading1());
      }

      public double GetNitrogenPressureReading2()
      {
         return (TruckCommBus.Instance.GetNitrogenPressureReading2());
      }

      public double GetRobotTotalCurrentReading()
      {
         return (TruckCommBus.Instance.GetRobotTotalCurrentReading());
      }

      public double GetLaunchTotalCurrentReading()
      {
         return (TruckCommBus.Instance.GetLaunchTotalCurrentReading());
      }

      #endregion

      #region Pump Functions

      public void StartAutoPump(ToolLocations toolLocation)
      {
         TruckCommBus.Instance.StartAutoPump(toolLocation);
      }

      public void StopAutoPump(ToolLocations toolLocation)
      {
         TruckCommBus.Instance.StopAutoPump(toolLocation);
      }

      public void PauseAutoPump(ToolLocations toolLocation)
      {
         TruckCommBus.Instance.PauseAutoPump(toolLocation);
      }

      public void ResumeAutoPump(ToolLocations toolLocation)
      {
         TruckCommBus.Instance.ResumeAutoPump(toolLocation);
      }

      public void SetPumpDirection(ToolLocations toolLocation, PumpDirections direction)
      {
         TruckCommBus.Instance.SetPumpDirection(toolLocation, direction);
      }

      public void SetPumpSpeed(ToolLocations toolLocation, double speedSetPoint)
      {
         TruckCommBus.Instance.SetPumpSpeed(toolLocation, speedSetPoint);
      }

      public void SetPumpPressure(ToolLocations toolLocation, double pressureSetPoint)
      {
         TruckCommBus.Instance.SetPumpPressure(toolLocation, pressureSetPoint);
      }

      public void StartPump(ToolLocations toolLocation)
      {
         TruckCommBus.Instance.StartPump(toolLocation);
      }

      public void StopPump(ToolLocations toolLocation)
      {
         TruckCommBus.Instance.StopPump(toolLocation);
      }

      public void RelievePumpPressure(ToolLocations toolLocation)
      {
         TruckCommBus.Instance.RelievePumpPressure(toolLocation);
      }

      public PumpAutoStates GetPumpAutoState(ToolLocations toolLocation)
      {
         return (TruckCommBus.Instance.GetPumpAutoState(toolLocation));
      }

      public PumpModes GetPumpMode(ToolLocations toolLocation)
      {
         return (TruckCommBus.Instance.GetPumpMode(toolLocation));
      }

      public double GetPumpSetPoint(ToolLocations toolLocation)
      {
         return (TruckCommBus.Instance.GetPumpSetPoint(toolLocation));
      }

      public PumpDirections GetPumpDirection(ToolLocations toolLocation)
      {
         return (TruckCommBus.Instance.GetPumpDirection(toolLocation));
      }

      public double GetPumpVolumePerSecond(ToolLocations toolLocation)
      {
         return (TruckCommBus.Instance.GetPumpVolumePerSecond(toolLocation));
      }

      public double GetPumpPressureReading(ToolLocations toolLocation)
      {
         return (TruckCommBus.Instance.GetPumpPressureReading(toolLocation));
      }

      public double GetReserviorWeightReading(ToolLocations toolLocation)
      {
         return (TruckCommBus.Instance.GetReserviorWeightReading(toolLocation));
      }

      public double GetPumpVolumeMeasure(ToolLocations toolLocation)
      {
         return (TruckCommBus.Instance.GetPumpVolumeMeasure(toolLocation));
      }

      public double GetPumpSpeedReading(ToolLocations toolLocation)
      {
         return (TruckCommBus.Instance.GetPumpSpeedReading(toolLocation));
      }

      public bool GetPumpActivity(ToolLocations toolLocation)
      {
         return (TruckCommBus.Instance.GetPumpActivity(toolLocation));
      }

      #endregion

      #region Reel Functions

      public void SetReelMode(ReelModes mode)
      {
         TruckCommBus.Instance.SetReelMode(mode);
      }

      public void SetReelManualMode(bool enable)
      {
         TruckCommBus.Instance.SetReelManualMode(enable);
      }

      public void SetReelManualCurrent(double setPoint)
      {
         TruckCommBus.Instance.SetReelManualCurrent(setPoint);
      }

      public void SetReelManualSpeed(double setPoint)
      {
         TruckCommBus.Instance.SetReelManualSpeed(setPoint);
      }      

      public void SetReelTotalDistance(double distance)
      {
         TruckCommBus.Instance.SetReelTotalDistance(distance);
      }

      public void SetReelTripDistance(double distance)
      {
         TruckCommBus.Instance.SetReelTripDistance(distance);
      }

      public void ResetReelTotalDistance()
      {
         TruckCommBus.Instance.ResetReelTotalDistance();
      }

      public void ResetReelTripDistance()
      {
         TruckCommBus.Instance.ResetReelTripDistance();
      }

      public void StartReelCalibration()
      {
         TruckCommBus.Instance.StartReelCalibration();
      }

      public void CalibrateReel(double distance)
      {
         TruckCommBus.Instance.CalibrateReel(distance);
      }

      public ReelModes GetReelMode()
      {
         return (TruckCommBus.Instance.GetReelMode());
      }

      public bool GetReelInLockMode()
      {
         return (TruckCommBus.Instance.GetReelInLockMode());
      }

      public bool GetReelInCurrentMode()
      {
         return (TruckCommBus.Instance.GetReelInCurrentMode());
      }

      public double GetReelCurrent()
      {
         return (TruckCommBus.Instance.GetReelCurrent());
      }

      public double GetReelSpeed()
      {
         return (TruckCommBus.Instance.GetReelSpeed());
      }      

      public double GetReelTotalDistance()
      {
         return (TruckCommBus.Instance.GetReelTotalDistance());
      }

      public double GetReelTripDistance()
      {
         return (TruckCommBus.Instance.GetReelTripDistance());
      }

      #endregion

      #region GPS Functions

      public double GetGpsLatitude()
      {
         return (TruckCommBus.Instance.GetGpsLatitude());
      }

      public double GetGpsLongitude()
      {
         return (TruckCommBus.Instance.GetGpsLongitude());
      }

      public DateTime GetGpsTime()
      {
         return (TruckCommBus.Instance.GetGpsTime());
      }

      #endregion

      #region Sensor Functions

      public void TriggerThicknessReading(double latitude, double longitude, DateTime dateTime, Directions direction, double displacement, double radialLocation)
      {
         TruckCommBus.Instance.TriggerThicknessReading(latitude, longitude, dateTime, direction, displacement, radialLocation);
      }

      public void TriggerStressReading(double latitude, double longitude, DateTime dateTime, Directions direction, double displacement, double radialLocation)
      {
         TruckCommBus.Instance.TriggerStressReading(latitude, longitude, dateTime, direction, displacement, radialLocation);
      }

      public bool GetThicknessReadingEnabled()
      {
         return (TruckCommBus.Instance.GetThicknessReadingEnabled());
      }

      public bool GetThicknessReadingPending()
      {
         return (TruckCommBus.Instance.GetThicknessReadingPending());
      }

      public double GetThicknessReading()
      {
         return (TruckCommBus.Instance.GetThicknessReading());
      }

      public bool GetStressReadingEnabled()
      {
         return (TruckCommBus.Instance.GetStressReadingEnabled());
      }

      public bool GetStressReadingPending()
      {
         return (TruckCommBus.Instance.GetStressReadingPending());
      }

      public double GetStressReading()
      {
         return (TruckCommBus.Instance.GetStressReading());
      }

      #endregion

      #region Feeder Functions

      public void SetFeederMode(FeederModes mode)
      {
         TruckCommBus.Instance.SetFeederMode(mode);
      }

      public void SetFeederVelocity(double velocity)
      {
         TruckCommBus.Instance.SetFeederVelocity(velocity);
      }

      public void SetFeederClamp(bool hold)
      {
         TruckCommBus.Instance.SetFeederClamp(hold);
      }

      public FeederModes GetFeederMode()
      {
         return (TruckCommBus.Instance.GetFeederMode() );
      }

      public double GetFeederVelocity()
      {
         return ( TruckCommBus.Instance.GetFeederVelocity() );
      }

      public bool GetFeederInLockMode()
      {
         return (TruckCommBus.Instance.GetFeederInLockMode());
      }

      public double GetFeederLockCurrent()
      {
         return (TruckCommBus.Instance.GetFeederLockCurrent());
      }

      public double GetTopFrontFeederCurrent()
      {
         return (TruckCommBus.Instance.GetTopFrontFeederCurrent());
      }

      public double GetTopRearFeederCurrent()
      {
         return (TruckCommBus.Instance.GetTopRearFeederCurrent());
      }

      public double GetBottomFrontFeederCurrent()
      {
         return (TruckCommBus.Instance.GetBottomFrontFeederCurrent());
      }

      public double GetBottomRearFeederCurrent()
      {
         return (TruckCommBus.Instance.GetBottomRearFeederCurrent());
      }

      public bool GetFeederClampSetPoint()
      {
         return (TruckCommBus.Instance.GetFeederClampSetPoint());
      }

      public bool GetFeederClamp()
      {
         return (TruckCommBus.Instance.GetFeederClamp());
      }

      public void EvaluateFeederParameters()
      {
         TruckCommBus.Instance.EvaluateFeederParameters();
      }

      #endregion

      #region Guide Functions

      public void SetGuideDirection(GuideLocations location, GuideDirections direction)
      {
         TruckCommBus.Instance.SetGuideDirection(location, direction);
      }

      public GuideDirections GetGuideDirection(GuideLocations location)
      {
         return (TruckCommBus.Instance.GetGuideDirection(location));
      }

      public bool GuideAtRetractLimit(GuideLocations location)
      {
         return (TruckCommBus.Instance.GuideAtRetractLimit(location));
      }

      public bool GuideAtExtendLimit(GuideLocations location)
      {
         return (TruckCommBus.Instance.GuideAtExtendLimit(location));
      }

      #endregion

      #region Solenoids Functions (body, wheels, drill cover, nozzle)

      public void SetSolenoid(Solenoids solenoid, bool activate)
      {
         RobotCommBus.Instance.SetSolenoid(solenoid, activate);
      }

      public void SetBodyPosition(BodyPositions bodyPosition)
      {
         RobotCommBus.Instance.SetBodyPosition(bodyPosition);
      }

      public void SetNozzleExtend(ToolLocations location, bool extended)
      {
         RobotCommBus.Instance.SetNozzleExtend(location, extended);
      }
      
      public bool GetSolenoidActive(Solenoids solenoid)
      {
         return (RobotCommBus.Instance.GetSolenoidActive(solenoid));
      }

      public double GetRobotPitch()
      {
         return (RobotCommBus.Instance.GetRobotPitch());
      }

      public double GetRobotRoll()
      {
         return (RobotCommBus.Instance.GetRobotRoll());
      }

      public BodyPositions GetBodyPosition()
      {
         return (RobotCommBus.Instance.GetBodyPosition());
      }

      public MovementWheelModes GetMovementWheelMode()
      {
         return (RobotCommBus.Instance.GetMovementWheelMode());
      }

      public bool GetNozzleExtended(ToolLocations location)
      {
         return (RobotCommBus.Instance.GetNozzleExtended(location));
      }

      public bool GetTopFrontReadyToLock()
      {
         return (RobotCommBus.Instance.GetTopFrontReadyToLock());
      }

      public bool GetTopRearReadyToLock()
      {
         return (RobotCommBus.Instance.GetTopRearReadyToLock());
      }

      public bool GetBottomFrontReadyToLock()
      {
         return (RobotCommBus.Instance.GetBottomFrontReadyToLock());
      }

      public bool GetBottomRearReadyToLock()
      {
         return (RobotCommBus.Instance.GetBottomRearReadyToLock());
      }

      #endregion

      #region Movement

      public void SetMovementMode(MovementModes mode)
      {
         RobotCommBus.Instance.SetMovementMode(mode);
      }

      public void SetMovementForwardMode(MovementForwardModes mode)
      {
         RobotCommBus.Instance.SetMovementForwardMode(mode);
      }

      public void SetMovementRequest(double request, bool triggered)
      {
         RobotCommBus.Instance.SetMovementRequest(request, triggered);
      }

      public void SetMovementManualVelocity(double manualVelocity)
      {
         RobotCommBus.Instance.SetMovementManualVelocity(manualVelocity);
      }

      public void SetMovementMotorDistance(double distance)
      {
         RobotCommBus.Instance.SetMovementMotorDistance(distance);
      }

      public bool GetMovementActivated()
      {
         return (RobotCommBus.Instance.GetMovementActivated());
      }
      
      public MovementModes GetMovementMode()
      {
         return (RobotCommBus.Instance.GetMovementMode());
      }

      public MovementForwardModes GetMovementForwardMode()
      {
         return (RobotCommBus.Instance.GetMovementForwardMode());
      }

      public MovementForwardControls GetMovementForwardControl()
      {
         return (RobotCommBus.Instance.GetMovementForwardControl());
      }

      public void GetMovementRequestValues(ref ValueParameter movementParameter, ref double movementRequestValue)
      {
         RobotCommBus.Instance.GetMovementRequestValues(ref movementParameter, ref movementRequestValue);
      }

      public double GetMovementValue()
      {
         return (RobotCommBus.Instance.GetMovementValue());
      }

      public double GetTopFrontMovementCurrent()
      {
         return (RobotCommBus.Instance.GetTopFrontMovementCurrent());
      }

      public double GetTopRearMovementCurrent()
      {
         return (RobotCommBus.Instance.GetTopRearMovementCurrent());
      }

      public double GetBottomFrontMovementCurrent()
      {
         return (RobotCommBus.Instance.GetBottomFrontMovementCurrent());
      }
         
      public double GetBottomRearMovementCurrent()
      {
         return (RobotCommBus.Instance.GetBottomRearMovementCurrent());
      }
      
      public double GetTopFrontMovementTemperature()
      {
         return (RobotCommBus.Instance.GetTopFrontMovementTemperature());
      }

      public double GetTopRearMovementTemperature()
      {
         return (RobotCommBus.Instance.GetTopRearMovementTemperature());
      }

      public double GetBottomFrontMovementTemperature()
      {
         return (RobotCommBus.Instance.GetBottomFrontMovementTemperature());
      }

      public double GetBottomRearMovementTemperature()
      {
         return (RobotCommBus.Instance.GetBottomRearMovementTemperature());
      }

      #endregion

      #region Drill

      public void StartAutoDrill(ToolLocations location)
      {
         if (DrillAutoStates.off == this.drillAutoState)
         {
            this.drillAutoState = DrillAutoStates.running;
         }
      }

      public void StopAutoDrill()
      {
         this.drillAutoState = DrillAutoStates.off;
      }

      public void PauseAutoDrill()
      {
         if (DrillAutoStates.running == this.drillAutoState)
         {
            this.drillAutoState = DrillAutoStates.paused;
         }
      }

      public void ResumeAutoDrill()
      {
         if (DrillAutoStates.paused == this.drillAutoState)
         {
            this.drillAutoState = DrillAutoStates.running;
         }
      }

#if false
      public bool DrillOriginReqiured(ToolLocations location)
      {
         DrillStatus status = this.GetDrillStatus(location);
         return(status.OriginRequired);
      }

      public bool DrillAtRetractionLimit(ToolLocations location)
      {
#if false
         DrillStatus status = this.GetDrillStatus(location);
         int limit = (location == ToolLocations.front) ? (int)ParameterAccessor.Instance.FrontDrill.ExtendedDistance.MinimumValue : (int)ParameterAccessor.Instance.RearDrill.ExtendedDistance.MinimumValue;
         
         bool result = (status.IndexPosition == limit) ? true : false;
         
         return(result);
#endif
         return (false);
      }

      public bool DrillAtExtensionLimit(ToolLocations location)
      {
#if false
         DrillStatus status = this.GetDrillStatus(location);
         int limit = (location == ToolLocations.front) ? (int)ParameterAccessor.Instance.FrontDrill.ExtendedDistance.MaximumValue : (int)ParameterAccessor.Instance.RearDrill.ExtendedDistance.MaximumValue;
         
         bool result = (status.IndexPosition == limit) ? true : false;
         
         return(result);
#endif
         return (false);
      }

      public int GetDrillOriginSetPoint(ToolLocations location)
      {
         DrillStatus status = this.GetDrillStatus(location);
         return (status.OriginSetPoint);
      }
#endif

      public void ConfigureDrillServo(ToolLocations location)
      {
         RobotCommBus.Instance.ConfigureDrillServo(location);
      }

      public void SetDrillRotationSpeed(ToolLocations location, double setPoint)
      {
         RobotCommBus.Instance.SetDrillRotationSpeed(location, setPoint);
      }

      public void RetractDrillToLimit(ToolLocations location)
      {
         RobotCommBus.Instance.RetractDrillToLimit(location);
      }

      public void StopDrill(ToolLocations location)
      {
         RobotCommBus.Instance.StopDrill(location);
      }

      public void SetDrillIndexSetPoint(ToolLocations location, double setPoint)
      {
         RobotCommBus.Instance.SetDrillIndexSetPoint(location, setPoint);
      }

      public void SetDrillOriginOffset(ToolLocations location)
      {
         RobotCommBus.Instance.SetDrillOriginOffset(location);

#if false
         DrillStatus status = this.GetDrillStatus(location);

         status.OriginSetPoint = status.IndexPosition;
         status.OriginRequired = false;
#endif
      }

      public void FindDrillOrigin(ToolLocations location)
      {
         //this.SetDrillIndexPosition(location, (int)ParameterAccessor.Instance.FrontDrill.ExtendedDistance.MaximumValue);
      }

      public void SetDrillCover(ToolLocations location, bool open)
      {
         Solenoids solenoid = (location == ToolLocations.front) ? Solenoids.frontDrillCover : Solenoids.rearDrillCover;
         this.SetSolenoid(solenoid, open);
      }

      public bool DrillCoverOpened(ToolLocations location)
      {
         Solenoids solenoid = (location == ToolLocations.front) ? Solenoids.frontDrillCover : Solenoids.rearDrillCover;
         bool opendedStatus = this.GetSolenoidActive(solenoid);
         return (opendedStatus);
      }

      public void SetLaserSight(ToolLocations location, bool sightOn)
      {
         RobotCommBus.Instance.SetLaserSight(location, sightOn);
      }

      public double GetDrillIndexSetPoint(ToolLocations location)
      {
         return (RobotCommBus.Instance.GetDrillIndexSetPoint(location));
      }

      public double GetDrillIndexPosition(ToolLocations location)
      {
         return (RobotCommBus.Instance.GetDrillIndexPosition(location));
      }

      public double GetDrillOriginOffset(ToolLocations location)
      {
         return (RobotCommBus.Instance.GetDrillOriginOffset(location));
      }

      public double GetDrillRotationSpeed(ToolLocations location)
      {
         return (RobotCommBus.Instance.GetDrillRotationSpeed(location));
      }

      public bool GetDrillError(ToolLocations location)
      {
         return (RobotCommBus.Instance.GetDrillError(location));
      }

      public bool GetLaserSight(ToolLocations location)
      {
         return (RobotCommBus.Instance.GetLaserSight(location));
      }

      public DrillAutoStates GetDrillAutoState()
      {
         return (this.drillAutoState);
      }

      #endregion

      #region Camera Functions

      public void SetLaunchCamera(CameraLocations camera)
      {
         TruckCommBus.Instance.SetLaunchCamera(camera);
      }

      public void SetRobotCameraA(CameraLocations camera)
      {
         RobotCommBus.Instance.SetRobotCameraA(camera);
      }

      public void SetRobotCameraB(CameraLocations camera)
      {
         RobotCommBus.Instance.SetRobotCameraB(camera);
      }

      public void SetCameraLightLevel(CameraLocations camera, int level)
      {
         if ((CameraLocations.launchLeftGuide == camera) ||
             (CameraLocations.launchRightGuide == camera) ||
             (CameraLocations.launchFeeder == camera) ||
             (CameraLocations.launchMain == camera))
         {
            TruckCommBus.Instance.SetCameraLightLevel(camera, level);
         }
         else
         {
            RobotCommBus.Instance.SetCameraLightLevel(camera, level);
         }
      }

      public CameraLocations GetLaunchCamera()
      {
         return (TruckCommBus.Instance.GetLaunchCamera());
      }

      public CameraLocations GetRobotCameraA()
      {
         return (RobotCommBus.Instance.GetRobotCameraA());
      }

      public CameraLocations GetRobotCameraB()
      {
         return (RobotCommBus.Instance.GetRobotCameraB());
      }

      #endregion

      #endregion
   }

   internal class DrillStatus
   {
      //public int IndexPosition;
      //public int IndexSetPoint;
      
      public bool OriginRequired;
      public int OriginSetPoint;

      public int RotationSpeed;

      public DrillStatus()
      {
         //this.IndexPosition = 0;
         //this.IndexSetPoint = 0;

         this.OriginRequired = true;
         this.OriginSetPoint = -1;

         this.RotationSpeed = 0;
      }
   }

}