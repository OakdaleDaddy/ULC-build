
namespace NICBOT.GUI
{
   using System;
   using System.Collections;
   using System.Data;
   using System.IO;
   using System.Net;
   using System.Xml;

   using PCANLight;

   public class ParameterAccessor
   {
      #region Fields 

      private static ParameterAccessor instance = null;

      private bool setDefaults;

      public RobotApplications RobotApplication;

      public TruckBusParameters TruckBus;
      public RobotBusParameters RobotBus;

      public int JoystickDeadband;
      public double Latitude;
      public double Longitude;

      public IpEndpointParameters Trace;
      public IpEndpointParameters LocationServer;

      public IpEndpointParameters ThicknessSensor;
      public ValueParameter ThicknessConversionUnit;

      public IpEndpointParameters StressSensor;
      public ValueParameter StressConversionUnit;

      public DigitalScaleParameters FrontScale;
      public DigitalScaleParameters RearScale;

      public ValueParameter NitrogenPressureConversionUnit;
      public CautionParameter NitrogenPressureCaution;

      public ValueParameter RobotTotalCurrentConversionUnit;
      public ValueParameter LaunchTotalCurrentConversionUnit;

      public ValueParameter GuideExtensionSpeed;
      public ValueParameter GuideRetractionSpeed;
      public bool GuideMomentaryButtonAction;

      public ValueParameter ReelDistance;
      public ValueParameter ReelDistanceScale;
      public MovementForwardControls ReelMotionMode;
      public ValueParameter ReelReverseCurrent;
      public ValueParameter ReelReverseSpeed;      
      public ValueParameter ReelLockCurrent;
      public ValueParameter ReelCalibrationDistance;
      public ValueParameter ReelManualCurrent;
      public ValueParameter ReelManualSpeed;
      
      public bool FeederAutomaticTracking;
      public double FeederVelocityToRpm;
      public ValueParameter FeederTrackingCalibration;
      public ValueParameter FeederMaxSpeed;
      public ValueParameter FeederLowSpeedScale;
      public ValueParameter FeederLockCurrent;
      public ValueParameter FeederCurrentPer1kRPM;
      public ValueParameter FeederManualSpeed;
      public CautionParameter FeederCurrentCaution;

      public FeederMotorParameters TopFrontFeederMotor;
      public FeederMotorParameters TopRearFeederMotor;
      public FeederMotorParameters BottomFrontFeederMotor;
      public FeederMotorParameters BottomRearFeederMotor;
      
      public ValueParameter MovementMotorLockCurrent;
      public ValueParameter MovementMotorMaxCurrent;
      public ValueParameter MovementMotorMaxSpeed;
      public ValueParameter MovementMotorCurrentPer1kRPM;
      public ValueParameter MovementMotorLowSpeedScale;
      public ValueParameter MovementMotorManualJogDistance;
      public ValueParameter MovementMotorManualMoveSpeed;
      public double MovementMotorVelocityToRpm;

      public MovementMotorParameters TopFrontMovementMotor;
      public MovementMotorParameters TopRearMovementMotor;
      public MovementMotorParameters BottomFrontMovementMotor;
      public MovementMotorParameters BottomRearMovementMotor;
      public CautionParameter MovementCurrentCaution;
      public CautionParameter MovementTemperatureCaution;

      public bool FrontToolSelected;

      public DrillParameters FrontDrill;
      public DrillParameters RearDrill;

      public PumpParameters FrontPump;
      public PumpParameters RearPump;
      
      private ValueParameter[] robotLightLevels;
      private ValueParameter[] launchLightLevels;

      public OsdParameters Osd;

      #endregion

      #region Properties

      public static ParameterAccessor Instance
      {
         get
         {
            if (instance == null)
            {
               instance = new ParameterAccessor();
               instance.Initialize();
            }

            return instance;
         }
      }

      #endregion

      #region Helper Functions

      private void SetFeederMotorDefaults(ref FeederMotorParameters feederMotorParameters, string location, bool positivePusher, bool positionInversion)
      {
         feederMotorParameters.Location = location;

         feederMotorParameters.State = MotorStates.Enabled;
         feederMotorParameters.Direction = MotorDirections.Normal;

         feederMotorParameters.PositivePusher = positivePusher;
         feederMotorParameters.PositionInversion = positionInversion;
      }

      private void SetMovementMotorDefaults(ref MovementMotorParameters motorParameters, string location, MovementForwardControls axialMode, MovementForwardControls circumferentialMode, MovementForwardControls cornerAxialMode, MovementForwardControls launchAxialMode)
      {
         motorParameters.Location = location;

         motorParameters.State = MotorStates.Enabled;
         motorParameters.Direction = MotorDirections.Normal;
         motorParameters.AxialMode = axialMode;
         motorParameters.CircumferentialMode = circumferentialMode;
         motorParameters.CornerAxialMode = cornerAxialMode;
         motorParameters.LaunchAxialMode = launchAxialMode;
      }

      private void SetDrillDefaults(ref DrillParameters drillParameters, string location)
      {
         drillParameters.Location = location;

         drillParameters.Acceleration = new ValueParameter("Acceleration", "", 0, 0, 4294967295, 100, 2560, 2560);
         drillParameters.ErrorLimit = new ValueParameter("ErrorLimit", "", 0, 0, 65535, 1, 3000, 3000);
         drillParameters.ProportionalControlConstant = new ValueParameter("ProportionalControlConstant", "", 0, 0, 4294967295, 1, 0, 0);
         drillParameters.IntegralControlConstant = new ValueParameter("IntegralControlConstant", "", 0, 0, 4294967295, 1, 0, 0);
         drillParameters.DerivativeControlConstant = new ValueParameter("DerivativeControlConstant", "", 0, 0, 4294967295, 1, 0, 0);

         drillParameters.RotationSpeed = new ValueParameter("RotationSpeed", "RPM", 0, 0, 5500, 100, 0, 0);
         drillParameters.SearchSpeed = new ValueParameter("SearchSpeed", "mm/MIN", 1, 1, 190.5, 1, 30, 30);
         drillParameters.TravelSpeed = new ValueParameter("TravelSpeed", "mm/MIN", 1, 1, 190.5, 1, 60, 60); ;
         drillParameters.CuttingSpeed = new ValueParameter("CuttingSpeed", "mm/MIN", 1, 1, 190.5, 1, 15, 15); ;

         drillParameters.SpeedToVelocityCount = 32176;
       
         drillParameters.AutoOrign = false;
         drillParameters.PeckMode = false;
         drillParameters.PositionRetract = false;

         drillParameters.CuttingDepth = new ValueParameter("CuttingDepth", "mm", 1, 1, 63.5, 0.1, 18, 18); ;
         drillParameters.CuttingIncrement = new ValueParameter("CuttingIncrement", "mm", 1, 1, 63.5, 1, 2, 2); ;
         drillParameters.RetractDistance = new ValueParameter("RetractDistance", "mm", 1, 1, 63.5, 0.1, 5, 5); ;
         drillParameters.RetractPosition = new ValueParameter("RetractPosition", "mm", 1, 1, 63.5, 0.1, 0, 0); ;

         drillParameters.ExtendedDistance = new ValueParameter("ExtendedDistance", "mm", 1, 0, 63.5, 0.1, 0, 0); ;
      }

      private void SetPumpDefaults(ref PumpParameters pumpParameters)
      {
         pumpParameters.PressureAutoFill = true;
         pumpParameters.AutoNozzleRetraction = true;
         pumpParameters.AutoPressureRelief = true;

         pumpParameters.AutoFillVolume = new ValueParameter("AutoFillVolume", "mL", 0, 0, 1000, 1, 1, 1);
         pumpParameters.MaximumVolume = new ValueParameter("MaximumVolume", "mL", 0, 0, 1000, 1, 1, 1);
         pumpParameters.AutoFillPressure = new ValueParameter("AutoFillPressure", "PSI", 0, 0, 1000, 1, 1, 1);
         pumpParameters.MaximumPressure = new ValueParameter("MaximumPressure", "PSI", 0, 0, 1000, 1, 1, 1);

         pumpParameters.RelievedPressure = new ValueParameter("RelievedPressure", "PSI", 0, 0, 1000, 1, 1, 1);
         pumpParameters.ForwardSpeed = new ValueParameter("ForwardSpeed", "RPM", 0, 0, 500, 1, 200, 200);
         pumpParameters.ReverseSpeed = new ValueParameter("ReverseSpeed", "RPM", 0, 0, 500, 1, 200, 200);
         pumpParameters.MaximumSpeed = new ValueParameter("MaximumSpeed", "RPM", 0, 0, 500, 1, 500, 500);

         pumpParameters.SealantWeight = new ValueParameter("SealantWeight", "g/mL", 0, 0, 1000, 1, 1, 1);
         pumpParameters.FlowConstant = new ValueParameter("FlowConstant", "REV/L", 0, 0, 1000, 1, 1, 1);

         pumpParameters.RpmPerVolt = 50;
         pumpParameters.PsiPerVolt = 62.5;

         pumpParameters.AutoFillPressure.MaximumValue = pumpParameters.MaximumPressure.OperationalValue;
         pumpParameters.RelievedPressure.MaximumValue = pumpParameters.MaximumPressure.OperationalValue;
         pumpParameters.AutoFillVolume.MaximumValue = pumpParameters.MaximumVolume.OperationalValue;
         pumpParameters.ForwardSpeed.MaximumValue = pumpParameters.MaximumSpeed.OperationalValue;
         pumpParameters.ReverseSpeed.MaximumValue = pumpParameters.MaximumSpeed.OperationalValue;
      }

      private void SetOsdDefaults(ref OsdParameters osdParameters)
      {
         osdParameters.HozizontalOffset = 35;
         osdParameters.VerticalOffset = 21;

         osdParameters.ShowDate = false;
         osdParameters.ShowTime = false;
         osdParameters.ShowCameraId = false;
         osdParameters.ShowDescription = false;
         osdParameters.ShowPipeDisplacement = false;
         osdParameters.ShowPipePosition = false;

         osdParameters.Line1 = "";
         osdParameters.Line2 = "";
         osdParameters.Line3 = "";
         osdParameters.Line4 = "";
         osdParameters.Line5 = "";
      }

      private void SetLightDefaults(ref ValueParameter lightParameters)
      {
         lightParameters.Name = "Light";
         lightParameters.Unit = "";
         lightParameters.Precision = 0;
         lightParameters.MinimumValue = 1;
         lightParameters.MaximumValue = 100;
         lightParameters.DefaultValue = 15;
         lightParameters.OperationalValue = 15;
      }

      private void AssignDefaults(RobotApplications robotApplication = RobotApplications.repair)
      {
         this.RobotApplication = robotApplication;

         this.TruckBus = new TruckBusParameters();
         this.TruckBus.BusInterface = BusInterfaces.PCIA;
         this.TruckBus.BitRate = 50000;
         this.TruckBus.ConsumerHeartbeatRate = 3000;
         this.TruckBus.ProducerHeartbeatRate = 1000;
         this.TruckBus.ControllerBusId = 80;
         this.TruckBus.ReelMotorBusId = 70;
         this.TruckBus.ReelDigitalBusId = 17;
         this.TruckBus.ReelAnalogBusId = 19;
         this.TruckBus.ReelEncoderBusId = 21;
         this.TruckBus.FeederTopFrontMotorBusId = 64;
         this.TruckBus.FeederTopRearMotorBusId = 65;
         this.TruckBus.FeederBottomFrontMotorBusId = 66;
         this.TruckBus.FeederBottomRearMotorBusId = 67;
         this.TruckBus.FeederEncoderBusId = 22;
         this.TruckBus.GuideLeftMotorBusId = 68;
         this.TruckBus.GuideRightMotorBusId = 69;
         this.TruckBus.LaunchDigitalIoBusId = 16;
         this.TruckBus.LaunchAnalogIoBusId = 18;
         this.TruckBus.GpsBusId = 20;
         this.TruckBus.FrontPumpBusId = 71;
         this.TruckBus.FrontScaleRs232BusId = 96;
         this.TruckBus.RearPumpBusId = 72;
         this.TruckBus.RearScaleRs232BusId = 97;

         this.TruckBus.ControllerTraceMask = 0;
         this.TruckBus.ReelMotorTraceMask = 1;
         this.TruckBus.ReelDigitalTraceMask = 1;
         this.TruckBus.ReelAnalogTraceMask = 1;
         this.TruckBus.ReelEncoderTraceMask = 1;
         this.TruckBus.FeederTopFrontMotorTraceMask = 1;
         this.TruckBus.FeederTopRearMotorTraceMask = 1;
         this.TruckBus.FeederBottomFrontMotorTraceMask = 1;
         this.TruckBus.FeederBottomRearMotorTraceMask = 1;
         this.TruckBus.FeederEncoderTraceMask = 1;
         this.TruckBus.GuideLeftMotorTraceMask = 1;
         this.TruckBus.GuideRightMotorTraceMask = 1;
         this.TruckBus.LaunchDigitalIoTraceMask = 1;
         this.TruckBus.LaunchAnalogIoTraceMask = 1;
         this.TruckBus.GpsTraceMask = 1;
         this.TruckBus.FrontPumpTraceMask = 1;
         this.TruckBus.FrontScaleRs232TraceMask = 1;
         this.TruckBus.RearPumpTraceMask = 1;
         this.TruckBus.RearScaleRs232TraceMask = 1;

         this.JoystickDeadband = 5000;
         this.Latitude = double.NaN;
         this.Longitude = double.NaN;

         this.RobotBus = new RobotBusParameters();
         this.RobotBus.BusInterface = BusInterfaces.PCIB;
         this.RobotBus.BitRate = 50000;
         this.RobotBus.ConsumerHeartbeatRate = 3000;
         this.RobotBus.ProducerHeartbeatRate = 1000;
         this.RobotBus.ControllerBusId = 80;
         this.RobotBus.RobotBodyBusId = 32;
         this.RobotBus.RobotTopFrontWheelBusId = 49;
         this.RobotBus.RobotTopRearWheelBusId = 50;
         this.RobotBus.RobotBottomFrontWheelBusId = 51;
         this.RobotBus.RobotBottomRearWheelBusId = 52;
         this.RobotBus.ControllerTraceMask = 0;
         this.RobotBus.RobotBodyTraceMask = 1;
         this.RobotBus.RobotTopFrontWheelTraceMask = 1;
         this.RobotBus.RobotTopRearWheelTraceMask = 1;
         this.RobotBus.RobotBottomFrontWheelTraceMask = 1;
         this.RobotBus.RobotBottomRearWheelTraceMask = 1;

         this.FrontScale = new DigitalScaleParameters("FrontScale", 1, 9600);
         this.RearScale = new DigitalScaleParameters("RearScale", 2, 9600);

         this.Trace = new IpEndpointParameters("Trace", "127.0.0.1", 10000);
         this.LocationServer = new IpEndpointParameters("LocationServer", "0.0.0.0", 5050);
         
         this.ThicknessSensor = new IpEndpointParameters("ThicknessSensor", "192.168.1.101", 0);
         this.ThicknessConversionUnit = new ValueParameter("ThicknessConversionUnit", "mm", 3, 0, 100, 1, 1, 1);
         
         this.StressSensor = new IpEndpointParameters("StressSensor", "192.168.1.102", 0);
         this.StressConversionUnit = new ValueParameter("StressConversionUnit", "MPa", 3, 0, 100, 1, 1, 1);

         this.NitrogenPressureConversionUnit = new ValueParameter("NitrogenPressureConversionUnit", "PSI", 3, 0, 1000, 1, 100, 100);
         this.NitrogenPressureCaution = new CautionParameter("NitrogenPressureCaution", 450, 425, 375, 350);

         this.RobotTotalCurrentConversionUnit = new ValueParameter("RobotTotalCurrentConversionUnit", "A", 2, 0, 1000, 1, 100, 100);
         this.LaunchTotalCurrentConversionUnit = new ValueParameter("LaunchTotalCurrentConversionUnit", "A", 2, 0, 1000, 1, 100, 100);

         this.GuideExtensionSpeed = new ValueParameter("GuideExtensionSpeed", "RPM", 0, 1, 4500, 1, 1500, 1500);
         this.GuideRetractionSpeed = new ValueParameter("GuideRetractionSpeed", "RPM", 0, 1, 4500, 1, 1500, 1500);
         this.GuideMomentaryButtonAction = true;

         this.ReelDistance = new ValueParameter("ReelDistance", "m", 0, 0, 0, 0, 0, 0);
         this.ReelDistanceScale = new ValueParameter("ReelDistanceScale", "m", 6, 0, 1000, 0, 1, 1);
         this.ReelMotionMode = MovementForwardControls.current;
         this.ReelReverseCurrent = new ValueParameter("ReelReverseCurrent", "A", 1, 0.0, 6.5, 0.1, 5.0, 5.0);
         this.ReelReverseSpeed = new ValueParameter("ReelReverseSpeed", "RPM", 0, 0, 4500, 100, 500, 500);
         this.ReelLockCurrent = new ValueParameter("ReelLockCurrent", "A", 1, 0.0, 6.5, 0.1, 3.0, 3.0);
         this.ReelCalibrationDistance = new ValueParameter("ReelCalibrationDistance", "m", 0, 1, 100, 1, 1, 1);
         this.ReelManualCurrent = new ValueParameter("ReelManualCurrent", "A", 1, 0.0, 6.5, 0.1, 5.0, 5.0);
         this.ReelManualSpeed = new ValueParameter("ReelManualSpeed", "RPM", 0, 0, 4500, 100, 500, 500);

         this.FeederAutomaticTracking = false;
         this.FeederVelocityToRpm = (4000/12.05992); // 12.05992 m/MIN at 4000 RPM
         this.FeederTrackingCalibration = new ValueParameter("FeederTrackingCalibration", "%", 1, -100, 100, 0.1, 50, 50);
         this.FeederMaxSpeed = new ValueParameter("FeederMaxSpeed", "m/MIN", 2, 0, 12.05992, 1, 12, 12);
         this.FeederLowSpeedScale = new ValueParameter("FeederLowSpeedScale", "%", 0, 1, 100, 1, 30, 30);
         this.FeederLockCurrent = new ValueParameter("FeederLockCurrent", "A", 2, 0.01, 0.5, 0.0, 0.10, 0.10);
         this.FeederCurrentPer1kRPM = new ValueParameter("FeederCurrentPer1kRPM", "A", 1, 0.1, 3.0, 0.1, 0.8, 0.8);
         this.FeederManualSpeed = new ValueParameter("FeederManualSpeed", "m/MIN", 2, 0, 12.05992, 1, 7, 7);
         this.FeederCurrentCaution = new CautionParameter("FeederCurrentCaution", 6.0, 5.0, 0.0, 0.0);

         this.TopFrontFeederMotor = new FeederMotorParameters();
         this.SetFeederMotorDefaults(ref this.TopFrontFeederMotor, "TopFront", false, false);

         this.TopRearFeederMotor = new FeederMotorParameters();
         this.SetFeederMotorDefaults(ref this.TopRearFeederMotor, "TopRear", false, false);

         this.BottomFrontFeederMotor = new FeederMotorParameters();
         this.SetFeederMotorDefaults(ref this.BottomFrontFeederMotor, "BottomFront", true, true);

         this.BottomRearFeederMotor = new FeederMotorParameters();
         this.SetFeederMotorDefaults(ref this.BottomRearFeederMotor, "BottomRear", true, true);


         this.MovementMotorLockCurrent = new ValueParameter("MovementMotorLockCurrent", "A", 1, 0.1, 3.0, 0.1, 1.0, 1.0);
         this.MovementMotorMaxCurrent = new ValueParameter("MovementMotorMaxCurrent", "A", 1, 0.1, 3.0, 0.1, 3.0, 3.0);
         this.MovementMotorMaxSpeed = new ValueParameter("MovementMotorMaxSpeed", "m/MIN", 2, 0, 10, 1, 3.5, 3.5);
         this.MovementMotorCurrentPer1kRPM = new ValueParameter("MovementMotorCurrentPer1kRPM", "A", 1, 0.1, 3.0, 0.1, 1.0, 1.0);
         this.MovementMotorLowSpeedScale = new ValueParameter("MovementMotorLowSpeedScale", "%", 0, 1, 100, 1, 30, 30);
         this.MovementMotorManualJogDistance = new ValueParameter("MovementMotorManualJogDistance", "mm", 0, 1, 100, 1, 1, 1);
         this.MovementMotorManualMoveSpeed = new ValueParameter("MovementMotorManualMoveSpeed", "m/MIN", 2, 0, 10, 1, 2, 2);
         this.MovementMotorVelocityToRpm = (40000.0/19.04); // 19.04m/MIN at 40000 RPM, (0.476mm/(motor revolution)

         this.TopFrontMovementMotor = new MovementMotorParameters();
         this.SetMovementMotorDefaults(ref this.TopFrontMovementMotor, "TopFront", MovementForwardControls.velocity, MovementForwardControls.velocity, MovementForwardControls.velocity, MovementForwardControls.current);

         this.TopRearMovementMotor = new MovementMotorParameters();
         this.SetMovementMotorDefaults(ref this.TopRearMovementMotor, "TopRear", MovementForwardControls.velocity, MovementForwardControls.velocity, MovementForwardControls.velocity, MovementForwardControls.current);

         this.BottomFrontMovementMotor = new MovementMotorParameters();
         this.SetMovementMotorDefaults(ref this.BottomFrontMovementMotor, "BottomFront", MovementForwardControls.velocity, MovementForwardControls.velocity, MovementForwardControls.current, MovementForwardControls.current);

         this.BottomRearMovementMotor = new MovementMotorParameters();
         this.SetMovementMotorDefaults(ref this.BottomRearMovementMotor, "BottomRear", MovementForwardControls.velocity, MovementForwardControls.velocity, MovementForwardControls.current, MovementForwardControls.current);

         this.MovementCurrentCaution = new CautionParameter("MovementCurrentCaution", 15.0, 10.0, 0.0, 0.0);
         this.MovementTemperatureCaution = new CautionParameter("MovementTemperatureCaution", 130.0, 100.0, 0.0, 0.0);

         this.FrontToolSelected = true;

         this.FrontDrill = new DrillParameters();
         this.SetDrillDefaults(ref this.FrontDrill, "Front");

         this.RearDrill = new DrillParameters();
         this.SetDrillDefaults(ref this.RearDrill, "Rear");


         this.FrontPump = new PumpParameters();
         this.SetPumpDefaults(ref this.FrontPump);

         this.RearPump = new PumpParameters();
         this.SetPumpDefaults(ref this.RearPump);


         this.Osd = new OsdParameters();
         this.SetOsdDefaults(ref this.Osd);


         this.robotLightLevels = new ValueParameter[12];
         for (int i = 0; i < this.robotLightLevels.Length; i++)
         {
            ValueParameter value = new ValueParameter();
            this.SetLightDefaults(ref value);
            value.Name = "RobotLightLevel_" + (i + 1).ToString();
            this.robotLightLevels[i] = value;
         }

         this.launchLightLevels = new ValueParameter[4];
         for (int i = 0; i < this.launchLightLevels.Length; i++)
         {
            ValueParameter value = new ValueParameter();
            this.SetLightDefaults(ref value);
            value.Name = "LaunchLightLevel_" + (i + 1).ToString();
            this.launchLightLevels[i] = value;
         }
      }

      private void Initialize()
      {
         this.setDefaults = false;
         this.AssignDefaults();
      }

      #endregion

      #region Read Functions

      private void ReadElement(XmlReader reader, ref int value)
      {
         try
         {
            if (reader.Read())
            {
               value = int.Parse(reader.Value.Trim());
            }
         }
         catch { }
      }

      private void ReadElement(XmlReader reader, ref bool value)
      {
         try
         {
            if (reader.Read())
            {
               int fileValue = int.Parse(reader.Value.Trim());
               value = (0 != fileValue) ? true : false;
            }
         }
         catch { }
      }

      private string ReadString(XmlReader reader)
      {
         string result = "";

         try
         {
            if (reader.Read())
            {
               result = reader.Value.Trim();
            }
         }
         catch { }

         return (result);
      }
      
      private string ReadStringNoTrim(XmlReader reader)
      {
         string result = "";

         try
         {
            if (reader.Read())
            {
               result = reader.Value.ToString();
            }
         }
         catch { }

         if (result.Contains("\n") != false)
         {
            result = "";
         }

         return (result);
      }

      private bool ReadBool(XmlReader reader)
      {
         bool result = false;

         try
         {
            if (reader.Read())
            {
               int value = int.Parse(reader.Value.Trim());
               result = (0 != value) ? true : false;
            }
         }
         catch { }

         return (result);
      }

      private int ReadInt(XmlReader reader)
      {
         int result = 0;

         try
         {
            if (reader.Read())
            {
               result = int.Parse(reader.Value.Trim());
            }
         }
         catch { }

         return (result);
      }

      private double ReadDouble(XmlReader reader)
      {
         double result = double.NaN;

         try
         {
            if (reader.Read())
            {
               result = double.Parse(reader.Value.Trim());
            }
         }
         catch { }

         return (result);
      }

      private RobotApplications ReadRobotApplication(XmlReader reader)
      {
         RobotApplications result = RobotApplications.repair;

         try
         {
            if (reader.Read())
            {
               result = (RobotApplications)Enum.Parse(typeof(RobotApplications), reader.Value.Trim());
            }
         }
         catch { }

         return (result);
      }

      private BusInterfaces ReadBusInterface(XmlReader reader)
      {
         BusInterfaces result = BusInterfaces.USBA;

         try
         {
            if (reader.Read())
            {
               result = (BusInterfaces)Enum.Parse(typeof(BusInterfaces), reader.Value.Trim());
            }
         }
         catch { }

         return (result);
      }

      private MotorStates ReadMotorState(XmlReader reader)
      {
         MotorStates result = MotorStates.Disabled;

         try
         {
            if (reader.Read())
            {
               result = (MotorStates)Enum.Parse(typeof(MotorStates), reader.Value.Trim());
            }
         }
         catch { }

         return (result);
      }

      private MotorDirections ReadMotorDirection(XmlReader reader)
      {
         MotorDirections result = MotorDirections.Normal;

         try
         {
            if (reader.Read())
            {
               result = (MotorDirections)Enum.Parse(typeof(MotorDirections), reader.Value.Trim());
            }
         }
         catch { }

         return (result);
      }

      private MovementForwardControls ReadMotorForwardControl(XmlReader reader)
      {
         MovementForwardControls result = MovementForwardControls.velocity;

         try
         {
            if (reader.Read())
            {
               result = (MovementForwardControls)Enum.Parse(typeof(MovementForwardControls), reader.Value.Trim());
            }
         }
         catch { }

         return (result);
      }

      private IpEndpointParameters ReadIpEndpoint(XmlReader reader)
      {
         IpEndpointParameters result = null;
         bool readResult = true;

         string name = "";
         string address = "";
         int port = 0;

         for (; readResult; )
         {
            readResult = reader.Read();

            if (reader.IsStartElement())
            {
               switch (reader.Name)
               {
                  case "Name":
                  {
                     name = this.ReadString(reader);
                     break;
                  }
                  case "Address":
                  {
                     address = this.ReadString(reader);
                     break;
                  }
                  case "Port":
                  {
                     port = this.ReadInt(reader);
                     break;
                  }
               }
            }
            else
            {
               if ("IpEndpoint" == reader.Name)
               {
                  result = new IpEndpointParameters();

                  result.Name = name;
                  result.Address = address;
                  result.Port = port;
                  
                  break;
               }
            }
         }

         return (result);
      }

      // todo move this into object, i.e. static Read(XmlReader reader), static Write(XmlWriter)
      private TruckBusParameters ReadTruckBusParameters(XmlReader reader)
      {
         TruckBusParameters result = null;
         bool readResult = true;

         BusInterfaces busInterface = BusInterfaces.USBA;
         int bitRate = 0; // todo use second object to contain value before assignment
         int consumerHeartbeatRate = 0;
         int producerHeartbeatRate = 0;
         int controllerBusId = 0;
         int reelMotorBusId = 0;
         int reelDigitalBusId = 0;
         int reelAnalogBusId = 0;
         int reelEncoderBusId = 0;
         int feederTopFrontMotorBusId = 0;
         int feederTopRearMotorBusId = 0;
         int feederBottomFrontMotorBusId = 0;
         int feederBottomRearMotorBusId = 0;
         int feederEncoderBusId = 0;
         int guideLeftMotorBusId = 0;
         int guideRightMotorBusId = 0;
         int launchDigitalIoBusId = 0;
         int launchAnalogIoBusId = 0;
         int gpsBusId = 0;
         int frontPumpBusId = 0;
         int frontScaleRs232BusId = 0;
         int rearPumpBusId = 0;
         int rearScaleRs232BusId = 0;
         int controllerTraceMask = 0;
         int reelMotorTraceMask = 0;
         int reelDigitalTraceMask = 0;
         int reelAnalogTraceMask = 0;
         int reelEncoderTraceMask = 0;
         int feederTopFrontMotorTraceMask = 0;
         int feederTopRearMotorTraceMask = 0;
         int feederBottomFrontMotorTraceMask = 0;
         int feederBottomRearMotorTraceMask = 0;
         int feederEncoderTraceMask = 0;
         int guideLeftMotorTraceMask = 0;
         int guideRightMotorTraceMask = 0;
         int launchDigitalIoTraceMask = 0;
         int launchAnalogIoTraceMask = 0;
         int gpsTraceMask = 0;
         int frontPumpTraceMask = 0;
         int frontScaleRs232TraceMask = 0;
         int rearPumpTraceMask = 0;
         int rearScaleRs232TraceMask = 0;

         for (; readResult; )
         {
            readResult = reader.Read();

            if (reader.IsStartElement())
            {
               switch (reader.Name)
               {
                  case "BusInterface":
                  {
                     busInterface = this.ReadBusInterface(reader);
                     break;
                  }
                  case "BitRate":
                  {
                     bitRate = this.ReadInt(reader);
                     break;
                  }
                  case "ConsumerHeartbeatRate":
                  {
                     consumerHeartbeatRate = this.ReadInt(reader);
                     break;
                  }
                  case "ProducerHeartbeatRate":
                  {
                     producerHeartbeatRate = this.ReadInt(reader);
                     break;
                  }
                  case "ControllerBusId":
                  {
                     controllerBusId = this.ReadInt(reader);
                     break;
                  }
                  case "ReelMotorBusId":
                  {
                     reelMotorBusId = this.ReadInt(reader);
                     break;
                  }
                  case "ReelDigitalBusId":
                  {
                     reelDigitalBusId = this.ReadInt(reader);
                     break;
                  }
                  case "ReelAnalogBusId":
                  {
                     reelAnalogBusId = this.ReadInt(reader);
                     break;
                  }
                  case "ReelEncoderBusId":
                  {
                     reelEncoderBusId = this.ReadInt(reader);
                     break;
                  }
                  case "FeederTopFrontMotorBusId":
                  {
                     feederTopFrontMotorBusId = this.ReadInt(reader);
                     break;
                  }
                  case "FeederTopRearMotorBusId":
                  {
                     feederTopRearMotorBusId = this.ReadInt(reader);
                     break;
                  }
                  case "FeederBottomFrontMotorBusId":
                  {
                     feederBottomFrontMotorBusId = this.ReadInt(reader);
                     break;
                  }
                  case "FeederBottomRearMotorBusId":
                  {
                     feederBottomRearMotorBusId = this.ReadInt(reader);
                     break;
                  }
                  case "FeederEncoderBusId":
                  {
                     feederEncoderBusId = this.ReadInt(reader);
                     break;
                  }
                  case "GuideLeftMotorBusId":
                  {
                     guideLeftMotorBusId = this.ReadInt(reader);
                     break;
                  }
                  case "GuideRightMotorBusId":
                  {
                     guideRightMotorBusId = this.ReadInt(reader);
                     break;
                  }
                  case "LaunchDigitalIoBusId":
                  {
                     launchDigitalIoBusId = this.ReadInt(reader);
                     break;
                  }
                  case "LaunchAnalogIoBusId":
                  {
                     launchAnalogIoBusId = this.ReadInt(reader);
                     break;
                  }
                  case "GpsBusId":
                  {
                     gpsBusId = this.ReadInt(reader);
                     break;
                  }
                  case "FrontPumpBusId":
                  {
                     frontPumpBusId = this.ReadInt(reader);
                     break;
                  }
                  case "FrontScaleRs232BusId":
                  {
                     frontScaleRs232BusId = this.ReadInt(reader);
                     break;
                  }
                  case "RearPumpBusId":
                  {
                     rearPumpBusId = this.ReadInt(reader);
                     break;
                  }
                  case "RearScaleRs232BusId":
                  {
                     rearScaleRs232BusId = this.ReadInt(reader);
                     break;
                  }
                  case "ControllerTraceMask":
                  {
                     controllerTraceMask = this.ReadInt(reader);
                     break;
                  }
                  case "ReelMotorTraceMask":
                  {
                     reelMotorTraceMask = this.ReadInt(reader);
                     break;
                  }
                  case "ReelDigitalTraceMask":
                  {
                     reelDigitalTraceMask = this.ReadInt(reader);
                     break;
                  }
                  case "ReelAnalogTraceMask":
                  {
                     reelAnalogTraceMask = this.ReadInt(reader);
                     break;
                  }
                  case "ReelEncoderTraceMask":
                  {
                     reelEncoderTraceMask = this.ReadInt(reader);
                     break;
                  }
                  case "FeederTopFrontMotorTraceMask":
                  {
                     feederTopFrontMotorTraceMask = this.ReadInt(reader);
                     break;
                  }
                  case "FeederTopRearMotorTraceMask":
                  {
                     feederTopRearMotorTraceMask = this.ReadInt(reader);
                     break;
                  }
                  case "FeederBottomFrontMotorTraceMask":
                  {
                     feederBottomFrontMotorTraceMask = this.ReadInt(reader);
                     break;
                  }
                  case "FeederBottomRearMotorTraceMask":
                  {
                     feederBottomRearMotorTraceMask = this.ReadInt(reader);
                     break;
                  }
                  case "FeederEncoderTraceMask":
                  {
                     feederEncoderTraceMask = this.ReadInt(reader);
                     break;
                  }
                  case "GuideLeftMotorTraceMask":
                  {
                     guideLeftMotorTraceMask = this.ReadInt(reader);
                     break;
                  }
                  case "GuideRightMotorTraceMask":
                  {
                     guideRightMotorTraceMask = this.ReadInt(reader);
                     break;
                  }
                  case "LaunchDigitalIoTraceMask":
                  {
                     launchDigitalIoTraceMask = this.ReadInt(reader);
                     break;
                  }
                  case "LaunchAnalogIoTraceMask":
                  {
                     launchAnalogIoTraceMask = this.ReadInt(reader);
                     break;
                  }
                  case "GpsTraceMask":
                  {
                     gpsTraceMask = this.ReadInt(reader);
                     break;
                  }
                  case "FrontPumpTraceMask":
                  {
                     frontPumpTraceMask = this.ReadInt(reader);
                     break;
                  }
                  case "FrontScaleRs232TraceMask":
                  {
                     frontScaleRs232TraceMask = this.ReadInt(reader);
                     break;
                  }
                  case "RearPumpTraceMask":
                  {
                     rearPumpTraceMask = this.ReadInt(reader);
                     break;
                  }
                  case "RearScaleRs232TraceMask":
                  {
                     rearScaleRs232TraceMask = this.ReadInt(reader);
                     break;
                  }
               }
            }
            else
            {
               if ("TruckBus" == reader.Name)
               {
                  result = new TruckBusParameters();

                  result.BusInterface = busInterface;
                  result.BitRate = bitRate;
                  
                  result.ConsumerHeartbeatRate = consumerHeartbeatRate;
                  result.ProducerHeartbeatRate = producerHeartbeatRate;
                  result.ControllerBusId = controllerBusId;
                  
                  result.ReelMotorBusId = reelMotorBusId;
                  result.ReelDigitalBusId = reelDigitalBusId;
                  result.ReelAnalogBusId = reelAnalogBusId;
                  result.ReelEncoderBusId = reelEncoderBusId;
                  result.FeederTopFrontMotorBusId = feederTopFrontMotorBusId;
                  result.FeederTopRearMotorBusId = feederTopRearMotorBusId;
                  result.FeederBottomFrontMotorBusId = feederBottomFrontMotorBusId;
                  result.FeederBottomRearMotorBusId = feederBottomRearMotorBusId;
                  result.FeederEncoderBusId = feederEncoderBusId;
                  result.GuideLeftMotorBusId = guideLeftMotorBusId;
                  result.GuideRightMotorBusId = guideRightMotorBusId;
                  result.LaunchDigitalIoBusId = launchDigitalIoBusId;
                  result.LaunchAnalogIoBusId = launchAnalogIoBusId;
                  result.GpsBusId = gpsBusId;
                  result.FrontPumpBusId = frontPumpBusId;
                  result.FrontScaleRs232BusId = frontScaleRs232BusId;
                  result.RearPumpBusId = rearPumpBusId;
                  result.RearScaleRs232BusId = rearScaleRs232BusId;

                  result.ControllerTraceMask = controllerTraceMask;
                  result.ReelMotorTraceMask = reelMotorTraceMask;
                  result.ReelDigitalTraceMask = reelDigitalTraceMask;
                  result.ReelAnalogTraceMask = reelAnalogTraceMask;
                  result.ReelEncoderTraceMask = reelEncoderTraceMask;
                  result.FeederTopFrontMotorTraceMask = feederTopFrontMotorTraceMask;
                  result.FeederTopRearMotorTraceMask = feederTopRearMotorTraceMask;
                  result.FeederBottomFrontMotorTraceMask = feederBottomFrontMotorTraceMask;
                  result.FeederBottomRearMotorTraceMask = feederBottomRearMotorTraceMask;
                  result.FeederEncoderTraceMask = feederEncoderTraceMask;
                  result.GuideLeftMotorTraceMask = guideLeftMotorTraceMask;
                  result.GuideRightMotorTraceMask = guideRightMotorTraceMask;
                  result.LaunchDigitalIoTraceMask = launchDigitalIoTraceMask;
                  result.LaunchAnalogIoTraceMask = launchAnalogIoTraceMask;
                  result.GpsTraceMask = gpsTraceMask;
                  result.FrontPumpTraceMask = frontPumpTraceMask;
                  result.FrontScaleRs232TraceMask = frontScaleRs232TraceMask;
                  result.RearPumpTraceMask = rearPumpTraceMask;
                  result.RearScaleRs232TraceMask = rearScaleRs232TraceMask;
                  
                  break;
               }
            }
         }

         return (result);
      }

      private RobotBusParameters ReadRobotBusParameters(XmlReader reader)
      {
         RobotBusParameters result = null;
         bool readResult = true;

         BusInterfaces busInterface = BusInterfaces.USBA;
         int bitRate = 0;
         int consumerHeartbeatRate = 0;
         int producerHeartbeatRate = 0;
         int controllerBusId = 0;
         int robotBodyBusId = 0;
         int robotTopFrontWheelBusId = 0;
         int robotTopRearWheelBusId = 0;
         int robotBottomFrontWheelBusId = 0;
         int robotBottomRearWheelBusId = 0;
         int controllerTraceMask = 0;
         int robotBodyTraceMask = 0;
         int robotTopFrontWheelTraceMask = 0;
         int robotTopRearWheelTraceMask = 0;
         int robotBottomFrontWheelTraceMask = 0;
         int robotBottomRearWheelTraceMask = 0;

         for (; readResult; )
         {
            readResult = reader.Read();

            if (reader.IsStartElement())
            {
               switch (reader.Name)
               {
                  case "BusInterface":
                  {
                     busInterface = this.ReadBusInterface(reader);
                     break;
                  }
                  case "BitRate":
                  {
                     bitRate = this.ReadInt(reader);
                     break;
                  }
                  case "ConsumerHeartbeatRate":
                  {
                     consumerHeartbeatRate = this.ReadInt(reader);
                     break;
                  }
                  case "ProducerHeartbeatRate":
                  {
                     producerHeartbeatRate = this.ReadInt(reader);
                     break;
                  }
                  case "ControllerBusId":
                  {
                     controllerBusId = this.ReadInt(reader);
                     break;
                  }
                  case "RobotBodyBusId":
                  {
                     robotBodyBusId = this.ReadInt(reader);
                     break;
                  }
                  case "RobotTopFrontWheelBusId":
                  {
                     robotTopFrontWheelBusId = this.ReadInt(reader);
                     break;
                  }
                  case "RobotTopRearWheelBusId":
                  {
                     robotTopRearWheelBusId = this.ReadInt(reader);
                     break;
                  }
                  case "RobotBottomFrontWheelBusId":
                  {
                     robotBottomFrontWheelBusId = this.ReadInt(reader);
                     break;
                  }
                  case "RobotBottomRearWheelBusId":
                  {
                     robotBottomRearWheelBusId = this.ReadInt(reader);
                     break;
                  }
                  case "ControllerTraceMask":
                  {
                     controllerTraceMask = this.ReadInt(reader);
                     break;
                  }
                  case "RobotBodyTraceMask":
                  {
                     robotBodyTraceMask = this.ReadInt(reader);
                     break;
                  }
                  case "RobotTopFrontWheelTraceMask":
                  {
                     robotTopFrontWheelTraceMask = this.ReadInt(reader);
                     break;
                  }
                  case "RobotTopRearWheelTraceMask":
                  {
                     robotTopRearWheelTraceMask = this.ReadInt(reader);
                     break;
                  }
                  case "RobotBottomFrontWheelTraceMask":
                  {
                     robotBottomFrontWheelTraceMask = this.ReadInt(reader);
                     break;
                  }
                  case "RobotBottomRearWheelTraceMask":
                  {
                     robotBottomRearWheelTraceMask = this.ReadInt(reader);
                     break;
                  }
               }
            }
            else
            {
               if ("RobotBus" == reader.Name)
               {
                  result = new RobotBusParameters();

                  result.BusInterface = busInterface;
                  result.BitRate = bitRate;

                  result.ConsumerHeartbeatRate = consumerHeartbeatRate;
                  result.ProducerHeartbeatRate = producerHeartbeatRate;
                  result.ControllerBusId = controllerBusId;

                  result.RobotBodyBusId = robotBodyBusId;
                  result.RobotTopFrontWheelBusId = robotTopFrontWheelBusId;
                  result.RobotTopRearWheelBusId = robotTopRearWheelBusId;
                  result.RobotBottomFrontWheelBusId = robotBottomFrontWheelBusId;
                  result.RobotBottomRearWheelBusId = robotBottomRearWheelBusId;

                  result.ControllerTraceMask = controllerTraceMask;
                  result.RobotBodyTraceMask = robotBodyTraceMask;
                  result.RobotTopFrontWheelTraceMask = robotTopFrontWheelTraceMask;
                  result.RobotTopRearWheelTraceMask = robotTopRearWheelTraceMask;
                  result.RobotBottomFrontWheelTraceMask = robotBottomFrontWheelTraceMask;
                  result.RobotBottomRearWheelTraceMask = robotBottomRearWheelTraceMask;
                  
                  break;
               }
            }
         }

         return (result);
      }

      private DigitalScaleParameters ReadDigitalScaleParameters(XmlReader reader)
      {
         DigitalScaleParameters result = null;
         bool readResult = true;

         string location = "";
         int port = 0;
         int baudRate = 0;
         
         for (; readResult; )
         {
            readResult = reader.Read();

            if (reader.IsStartElement())
            {
               switch (reader.Name)
               {
                  case "Location":
                  {
                     location = this.ReadString(reader);
                     break;
                  }
                  case "Port":
                  {
                     port = this.ReadInt(reader);
                     break;
                  }
                  case "BaudRate":
                  {
                     baudRate = this.ReadInt(reader);
                     break;
                  }
               }
            }
            else
            {
               if ("DigitalScale" == reader.Name)
               {
                  result = new DigitalScaleParameters(location, port, baudRate);
                  break;
               }
            }
         }

         return (result);
      }

      private CautionParameter ReadCautionParameters(XmlReader reader)
      {
         CautionParameter result = null;
         bool readResult = true;

         string name = "";
         double dangerHighLimit = 0;
         double warningHighLimit = 0;
         double warningLowLimit = 0;
         double dangerLowLimit = 0;
         
         for (; readResult; )
         {
            readResult = reader.Read();

            if (reader.IsStartElement())
            {
               switch (reader.Name)
               {
                  case "Name":
                  {
                     name = this.ReadString(reader);
                     break;
                  }
                  case "DangerHigh":
                  {
                     dangerHighLimit = this.ReadDouble(reader);
                     break;
                  }
                  case "WarningHigh":
                  {
                     warningHighLimit = this.ReadDouble(reader);
                     break;
                  }
                  case "WarningLow":
                  {
                     warningLowLimit = this.ReadDouble(reader);
                     break;
                  }
                  case "DangerLow":
                  {
                     dangerLowLimit = this.ReadDouble(reader);
                     break;
                  }
               }
            }
            else
            {
               if ("Caution" == reader.Name)
               {
                  result = new CautionParameter(name, dangerHighLimit, warningHighLimit, warningLowLimit, dangerLowLimit);
                  break;
               }
            }
         }

         return (result);
      }

      private ValueParameter ReadValueParameters(XmlReader reader)
      {
         ValueParameter result = null;
         bool readResult = true;

         string name = "";
         string unit = "";
         int precision = 0;
         double minimumValue = 0;
         double maximumValue = 0;
         double stepValue = 0;
         double defaultValue = 0;
         double operationalValue = 0;

         for (; readResult; )
         {
            readResult = reader.Read();

            if (reader.IsStartElement())
            {
               switch (reader.Name)
               {
                  case "Name":
                  {
                     name = this.ReadString(reader);
                     break;
                  }
                  case "Unit":
                  {
                     unit = this.ReadString(reader);
                     break;
                  }
                  case "Precision":
                  {
                     precision = this.ReadInt(reader);
                     break;
                  }
                  case "MinimumValue":
                  {
                     minimumValue = this.ReadDouble(reader);
                     break;
                  }
                  case "MaximumValue":
                  {
                     maximumValue = this.ReadDouble(reader);
                     break;
                  }
                  case "StepValue":
                  {
                     stepValue = this.ReadDouble(reader);
                     break;
                  }
                  case "DefaultValue":
                  {
                     defaultValue = this.ReadDouble(reader);
                     break;
                  }
                  case "OperationalValue":
                  {
                     operationalValue = this.ReadDouble(reader);
                     break;
                  }
               }
            }
            else
            {
               if ("Value" == reader.Name)
               {
                  result = new ValueParameter(name, unit, precision, minimumValue, maximumValue, stepValue, defaultValue, operationalValue);
                  break;
               }
            }
         }

         return (result);
      }

      private ValueParameter ReadLightValueParameters(XmlReader reader)
      {
         ValueParameter result = new ValueParameter();
         bool readResult = true;

         this.SetLightDefaults(ref result);

         for (; readResult; )
         {
            readResult = reader.Read();

            if (reader.IsStartElement())
            {
               switch (reader.Name)
               {
                  case "Name":
                  {
                     result.Name = this.ReadString(reader);
                     break;
                  }
                  case "Unit":
                  {
                     result.Unit = this.ReadString(reader);
                     break;
                  }
                  case "Precision":
                  {
                     result.Precision = this.ReadInt(reader);
                     break;
                  }
                  case "MinimumValue":
                  {
                     result.MinimumValue = this.ReadDouble(reader);
                     break;
                  }
                  case "MaximumValue":
                  {
                     result.MaximumValue = this.ReadDouble(reader);
                     break;
                  }
                  case "StepValue":
                  {
                     result.StepValue = this.ReadDouble(reader);
                     break;
                  }
                  case "DefaultValue":
                  {
                     result.DefaultValue = this.ReadDouble(reader);
                     break;
                  }
                  case "OperationalValue":
                  {
                     result.OperationalValue = this.ReadDouble(reader);
                     break;
                  }
               }
            }
            else
            {
               if ("Value" == reader.Name)
               {
                  break;
               }
            }
         }

         return (result);
      }

      private FeederMotorParameters ReadFeederMotorParameters(XmlReader reader)
      {
         FeederMotorParameters result = new FeederMotorParameters();
         this.SetFeederMotorDefaults(ref result, "", false, false);

         FeederMotorParameters temp = new FeederMotorParameters();
         bool readResult = true;
        
         for (; readResult; )
         {
            readResult = reader.Read();

            if (reader.IsStartElement())
            {
               switch (reader.Name)
               {
                  case "Location":
                  {
                     temp.Location = this.ReadString(reader);
                     break;
                  }
                  case "State":
                  {
                     temp.State = this.ReadMotorState(reader);
                     break;
                  }
                  case "Direction":
                  {
                     temp.Direction = this.ReadMotorDirection(reader);
                     break;
                  }
                  case "PositivePusher":
                  {
                     temp.PositivePusher = this.ReadBool(reader);
                     break;
                  }
                  case "PositionInversion":
                  {
                     temp.PositionInversion = this.ReadBool(reader);
                     break;
                  }
               }
            }
            else
            {
               if ("FeederMotor" == reader.Name)
               {
                  result = temp;
                  break;
               }
            }
         }

         return (result);
      }

      private MovementMotorParameters ReadMovementMotorParameters(XmlReader reader)
      {
         MovementMotorParameters result = new MovementMotorParameters();
         bool readResult = true;

         this.SetMovementMotorDefaults(ref result, "", MovementForwardControls.velocity, MovementForwardControls.velocity, MovementForwardControls.velocity, MovementForwardControls.current);
        
         for (; readResult; )
         {
            readResult = reader.Read();

            if (reader.IsStartElement())
            {
               switch (reader.Name)
               {
                  case "Location":
                  {
                     result.Location = this.ReadString(reader);
                     break;
                  }
                  case "State":
                  {
                     result.State = this.ReadMotorState(reader);
                     break;
                  }
                  case "Direction":
                  {
                     result.Direction = this.ReadMotorDirection(reader);
                     break;
                  }
                  case "AxialMode":
                  {
                     result.AxialMode = this.ReadMotorForwardControl(reader);
                     break;
                  }
                  case "CircumferentialMode":
                  {
                     result.CircumferentialMode = this.ReadMotorForwardControl(reader);
                     break;
                  }
                  case "CornerAxialMode":
                  {
                     result.CornerAxialMode = this.ReadMotorForwardControl(reader);
                     break;
                  }
                  case "LaunchAxialMode":
                  {
                     result.LaunchAxialMode = this.ReadMotorForwardControl(reader);
                     break;
                  }
               }
            }
            else
            {
               if ("MovementMotor" == reader.Name)
               {
                  break;
               }
            }
         }

         return (result);
      }

      private DrillParameters ReadDrillParameters(XmlReader reader)
      {
         DrillParameters result = new DrillParameters();
         bool readResult = true;

         this.SetDrillDefaults(ref result, "");
        
         for (; readResult; )
         {
            readResult = reader.Read();

            if (reader.IsStartElement())
            {
               switch (reader.Name)
               {
                  case "Location":
                  {
                     result.Location = this.ReadString(reader);
                     break;
                  }
                  case "AutoOrign":
                  {
                     result.AutoOrign = this.ReadBool(reader);
                     break;
                  }
                  case "PeckMode":
                  {
                     result.PeckMode = this.ReadBool(reader);
                     break;
                  }
                  case "PositionRetract":
                  {
                     result.PositionRetract = this.ReadBool(reader);
                     break;
                  }
                  case "SpeedToVelocityCount":
                  {
                     result.SpeedToVelocityCount = this.ReadDouble(reader);
                     break;
                  }
                  case "Value":
                  {
                     ValueParameter valueParameters = this.ReadValueParameters(reader);

                     if (null != valueParameters)
                     {
                        switch (valueParameters.Name)
                        {
                           case "Acceleration":
                           {
                              result.Acceleration = valueParameters;
                              break;
                           }
                           case "ErrorLimit":
                           {
                              result.ErrorLimit = valueParameters;
                              break;
                           }
                           case "ProportionalControlConstant":
                           {
                              result.ProportionalControlConstant = valueParameters;
                              break;
                           }
                           case "IntegralControlConstant":
                           {
                              result.IntegralControlConstant = valueParameters;
                              break;
                           }
                           case "DerivativeControlConstant":
                           {
                              result.DerivativeControlConstant = valueParameters;
                              break;
                           }
                           case "RotationSpeed":
                           {
                              result.RotationSpeed = valueParameters;
                              break;
                           }
                           case "SearchSpeed":
                           {
                              result.SearchSpeed = valueParameters;
                              break;
                           }
                           case "TravelSpeed":
                           {
                              result.TravelSpeed = valueParameters;
                              break;
                           }
                           case "CuttingSpeed":
                           {
                              result.CuttingSpeed = valueParameters;
                              break;
                           }
                           case "CuttingDepth":
                           {
                              result.CuttingDepth = valueParameters;
                              break;
                           }
                           case "CuttingIncrement":
                           {
                              result.CuttingIncrement = valueParameters;
                              break;
                           }
                           case "RetractDistance":
                           {
                              result.RetractDistance = valueParameters;
                              break;
                           }
                           case "RetractPosition":
                           {
                              result.RetractPosition = valueParameters;
                              break;
                           }
                           case "ExtendedDistance":
                           {
                              result.ExtendedDistance = valueParameters;
                              break;
                           }
                        }
                     }

                     break;
                  }
               }
            }
            else
            {
               if ("Drill" == reader.Name)
               {
                  break;
               }
            }
         }

         return (result);
      }

      private PumpParameters ReadPumpParameters(XmlReader reader)
      {
         PumpParameters result = new PumpParameters();
         bool readResult = true;

         this.SetPumpDefaults(ref result);
        
         for (; readResult; )
         {
            readResult = reader.Read();

            if (reader.IsStartElement())
            {
               switch (reader.Name)
               {
                  case "Location":
                  {
                     result.Location = this.ReadString(reader);
                     break;
                  }
                  case "PressureAutoFill":
                  {
                     result.PressureAutoFill = this.ReadBool(reader);
                     break;
                  }
                  case "AutoNozzleRetraction":
                  {
                     result.AutoNozzleRetraction = this.ReadBool(reader);
                     break;
                  }
                  case "AutoPressureRelief":
                  {
                     result.AutoPressureRelief = this.ReadBool(reader);
                     break;
                  }
                  case "RpmPerVolt":
                  {
                     result.RpmPerVolt = this.ReadDouble(reader);
                     break;
                  }
                  case "PsiPerVolt":
                  {
                     result.PsiPerVolt = this.ReadDouble(reader);
                     break;
                  }
                  case "Value":
                  {
                     ValueParameter valueParameters = this.ReadValueParameters(reader);

                     if (null != valueParameters)
                     {
                        switch (valueParameters.Name)
                        {
                           case "AutoFillVolume":
                           {
                              result.AutoFillVolume = valueParameters;
                              break;
                           }
                           case "MaximumVolume":
                           {
                              result.MaximumVolume = valueParameters;
                              break;
                           }
                           case "AutoFillPressure":
                           {
                              result.AutoFillPressure = valueParameters;
                              break;
                           }
                           case "MaximumPressure":
                           {
                              result.MaximumPressure = valueParameters;
                              break;
                           }
                           case "RelievedPressure":
                           {
                              result.RelievedPressure = valueParameters;
                              break;
                           }
                           case "ForwardSpeed":
                           {
                              result.ForwardSpeed = valueParameters;
                              break;
                           }
                           case "ReverseSpeed":
                           {
                              result.ReverseSpeed = valueParameters;
                              break;
                           }
                           case "MaximumSpeed":
                           {
                              result.MaximumSpeed = valueParameters;
                              break;
                           }
                           case "SealantWeight":
                           {
                              result.SealantWeight = valueParameters;
                              break;
                           }
                           case "FlowConstant":
                           {
                              result.FlowConstant = valueParameters;
                              break;
                           }
                        }
                     }

                     break;
                  }
               }
            }
            else
            {
               if ("Pump" == reader.Name)
               {
                  break;
               }
            }
         }

         result.AutoFillPressure.MaximumValue = result.MaximumPressure.OperationalValue;
         result.RelievedPressure.MaximumValue = result.MaximumPressure.OperationalValue;
         result.AutoFillVolume.MaximumValue = result.MaximumVolume.OperationalValue;
         result.ForwardSpeed.MaximumValue = result.MaximumSpeed.OperationalValue;
         result.ReverseSpeed.MaximumValue = result.MaximumSpeed.OperationalValue;

         return (result);
      }

      private OsdParameters ReadOsdParameters(XmlReader reader)
      {
         OsdParameters temp = new OsdParameters();
         OsdParameters result = new OsdParameters();
         bool readResult = true;

         for (; readResult; )
         {
            readResult = reader.Read();

            if (reader.IsStartElement())
            {
               switch (reader.Name)
               {
                  case "HozizontalOffset":
                  {
                     temp.HozizontalOffset = this.ReadInt(reader);
                     break;
                  }
                  case "VerticalOffset":
                  {
                     temp.VerticalOffset = this.ReadInt(reader);
                     break;
                  }
                  case "ShowDate":
                  {
                     temp.ShowDate = this.ReadBool(reader);
                     break;
                  }
                  case "ShowTime":
                  {
                     temp.ShowTime = this.ReadBool(reader);
                     break;
                  }
                  case "ShowCameraId":
                  {
                     temp.ShowCameraId = this.ReadBool(reader);
                     break;
                  }
                  case "ShowDescription":
                  {
                     temp.ShowDescription = this.ReadBool(reader);
                     break;
                  }
                  case "ShowPipeDisplacement":
                  {
                     temp.ShowPipeDisplacement = this.ReadBool(reader);
                     break;
                  }
                  case "ShowPipePosition":
                  {
                     temp.ShowPipePosition = this.ReadBool(reader);
                     break;
                  }
                  case "Line1":
                  {
                     temp.Line1 = this.ReadStringNoTrim(reader);
                     break;
                  }
                  case "Line2":
                  {
                     temp.Line2 = this.ReadStringNoTrim(reader);
                     break;
                  }
                  case "Line3":
                  {
                     temp.Line3 = this.ReadStringNoTrim(reader);
                     break;
                  }
                  case "Line4":
                  {
                     temp.Line4 = this.ReadStringNoTrim(reader);
                     break;
                  }
                  case "Line5":
                  {
                     temp.Line5 = this.ReadStringNoTrim(reader);
                     break;
                  }
               }
            }
            else
            {
               if ("Osd" == reader.Name)
               {
                  result = temp;
                  break;
               }
            }
         }

         return (result);
      }

      private void ReadLightParameters(XmlReader reader)
      {
         bool readResult = true;

         for (; readResult; )
         {
            readResult = reader.Read();

            if (reader.IsStartElement())
            {
               switch (reader.Name)
               {
                  case "Value":
                  {
                     ValueParameter valueParameters = this.ReadLightValueParameters(reader);

                     if (null != valueParameters)
                     {
                        switch (valueParameters.Name)
                        {
                           case "RobotLightLevel_1":
                           {
                              this.robotLightLevels[0] = valueParameters;
                              break;
                           }
                           case "RobotLightLevel_2":
                           {
                              this.robotLightLevels[1] = valueParameters;
                              break;
                           }
                           case "RobotLightLevel_3":
                           {
                              this.robotLightLevels[2] = valueParameters;
                              break;
                           }
                           case "RobotLightLevel_4":
                           {
                              this.robotLightLevels[3] = valueParameters;
                              break;
                           }
                           case "RobotLightLevel_5":
                           {
                              this.robotLightLevels[4] = valueParameters;
                              break;
                           }
                           case "RobotLightLevel_6":
                           {
                              this.robotLightLevels[5] = valueParameters;
                              break;
                           }
                           case "RobotLightLevel_7":
                           {
                              this.robotLightLevels[6] = valueParameters;
                              break;
                           }
                           case "RobotLightLevel_8":
                           {
                              this.robotLightLevels[7] = valueParameters;
                              break;
                           }
                           case "RobotLightLevel_9":
                           {
                              this.robotLightLevels[8] = valueParameters;
                              break;
                           }
                           case "RobotLightLevel_10":
                           {
                              this.robotLightLevels[9] = valueParameters;
                              break;
                           }
                           case "RobotLightLevel_11":
                           {
                              this.robotLightLevels[10] = valueParameters;
                              break;
                           }
                           case "RobotLightLevel_12":
                           {
                              this.robotLightLevels[11] = valueParameters;
                              break;
                           }
                           case "LaunchLightLevel_1":
                           {
                              this.launchLightLevels[0] = valueParameters;
                              break;
                           }
                           case "LaunchLightLevel_2":
                           {
                              this.launchLightLevels[1] = valueParameters;
                              break;
                           }
                           case "LaunchLightLevel_3":
                           {
                              this.launchLightLevels[2] = valueParameters;
                              break;
                           }
                           case "LaunchLightLevel_4":
                           {
                              this.launchLightLevels[3] = valueParameters;
                              break;
                           }
                        }
                     }

                     break;
                  }
               }
            }
            else
            {
               if ("Lights" == reader.Name)
               {
                  break;
               }
            }
         }
      }

      private void ReadData(string filePath)
      {
         try
         {
            using (XmlReader reader = XmlReader.Create(filePath))
            {
               bool result = true;

               #region Drill Read
               {
                  for (; result;)
                  {
                     result = reader.Read();

                     if (reader.IsStartElement())
                     {
                        switch (reader.Name)
                        {
                           case "TruckBus":
                           {
                              TruckBusParameters truckBusParameters = this.ReadTruckBusParameters(reader);

                              if (null != truckBusParameters)
                              {
                                 this.TruckBus = truckBusParameters;
                              }

                              break;
                           }
                           case "RobotBus":
                           {
                              RobotBusParameters robotBusParameters = this.ReadRobotBusParameters(reader);

                              if (null != robotBusParameters)
                              {
                                 this.RobotBus = robotBusParameters;
                              }

                              break;
                           }
                           case "IpEndpoint":
                           {
                              IpEndpointParameters ipEndpointParameters = this.ReadIpEndpoint(reader);

                              if (null != ipEndpointParameters)
                              {
                                 switch (ipEndpointParameters.Name)
                                 {
                                    case "Trace":
                                    {
                                       this.Trace = ipEndpointParameters;
                                       break;
                                    }
                                    case "LocationServer":
                                    {
                                       this.LocationServer = ipEndpointParameters;
                                       break;
                                    }
                                    case "ThicknessSensor":
                                    {
                                       this.ThicknessSensor = ipEndpointParameters;
                                       break;
                                    }
                                    case "StressSensor":
                                    {
                                       this.StressSensor = ipEndpointParameters;
                                       break;
                                    }
                                 }
                              }

                              break;
                           }
                           case "DigitalScale":
                           {
                              DigitalScaleParameters digitalScaleParameters = this.ReadDigitalScaleParameters(reader);

                              if (null != digitalScaleParameters)
                              {
                                 switch (digitalScaleParameters.Location)
                                 {
                                    case "FrontScale":
                                    {
                                       this.FrontScale = digitalScaleParameters;
                                       break;
                                    }
                                    case "RearScale":
                                    {
                                       this.RearScale = digitalScaleParameters;
                                       break;
                                    }
                                 }
                              }

                              break;
                           }
                           case "Caution":
                           {
                              CautionParameter cautionParameter = this.ReadCautionParameters(reader);

                              if (null != cautionParameter)
                              {
                                 switch (cautionParameter.Name)
                                 {
                                    case "NitrogenPressureCaution":
                                    {
                                       this.NitrogenPressureCaution = cautionParameter;
                                       break;
                                    }
                                    case "FeederCurrentCaution":
                                    {
                                       this.FeederCurrentCaution = cautionParameter;
                                       break;
                                    }
                                    case "MovementCurrentCaution":
                                    {
                                       this.MovementCurrentCaution = cautionParameter;
                                       break;
                                    }
                                    case "MovementTemperatureCaution":
                                    {
                                       this.MovementTemperatureCaution = cautionParameter;
                                       break;
                                    }
                                 }
                              }

                              break;
                           }
                           case "Value":
                           {
                              ValueParameter valueParameter = this.ReadValueParameters(reader);

                              if (null != valueParameter)
                              {
                                 switch (valueParameter.Name)
                                 {
                                    case "ThicknessConversionUnit":
                                    {
                                       this.ThicknessConversionUnit = valueParameter;
                                       break;
                                    }
                                    case "StressConversionUnit":
                                    {
                                       this.StressConversionUnit = valueParameter;
                                       break;
                                    }
                                    case "NitrogenPressureConversionUnit":
                                    {
                                       this.NitrogenPressureConversionUnit = valueParameter;
                                       break;
                                    }
                                    case "RobotTotalCurrentConversionUnit":
                                    {
                                       this.RobotTotalCurrentConversionUnit = valueParameter;
                                       break;
                                    }
                                    case "LaunchTotalCurrentConversionUnit":
                                    {
                                       this.LaunchTotalCurrentConversionUnit = valueParameter;
                                       break;
                                    }
                                    case "GuideExtensionSpeed":
                                    {
                                       this.GuideExtensionSpeed = valueParameter;
                                       break;
                                    }
                                    case "GuideRetractionSpeed":
                                    {
                                       this.GuideRetractionSpeed = valueParameter;
                                       break;
                                    }
                                    case "ReelDistance":
                                    {
                                       this.ReelDistance = valueParameter;
                                       break;
                                    }
                                    case "ReelDistanceScale":
                                    {
                                       this.ReelDistanceScale = valueParameter;
                                       break;
                                    }
                                    case "ReelReverseCurrent":
                                    {
                                       this.ReelReverseCurrent = valueParameter;
                                       break;
                                    }
                                    case "ReelReverseSpeed":
                                    {
                                       this.ReelReverseSpeed = valueParameter;
                                       break;
                                    }
                                    case "ReelLockCurrent":
                                    {
                                       this.ReelLockCurrent = valueParameter;
                                       break;
                                    }
                                    case "ReelCalibrationDistance":
                                    {
                                       this.ReelCalibrationDistance = valueParameter;
                                       break;
                                    }
                                    case "ReelManualCurrent":
                                    {
                                       this.ReelManualCurrent = valueParameter;
                                       break;
                                    }
                                    case "ReelManualSpeed":
                                    {
                                       this.ReelManualSpeed = valueParameter;
                                       break;
                                    }                                       
                                    case "FeederTrackingCalibration":
                                    {
                                       this.FeederTrackingCalibration = valueParameter;
                                       break;
                                    }
                                    case "FeederMaxSpeed":
                                    {
                                       this.FeederMaxSpeed = valueParameter; 
                                       break;
                                    }
                                    case "FeederLowSpeedScale":
                                    {
                                       this.FeederLowSpeedScale = valueParameter;
                                       break;
                                    }
                                    case "FeederLockCurrent":
                                    {
                                       this.FeederLockCurrent = valueParameter; 
                                       break;
                                    }
                                    case "FeederCurrentPer1kRPM":
                                    {
                                       this.FeederCurrentPer1kRPM = valueParameter; 
                                       break;
                                    }
                                    case "FeederManualSpeed":
                                    {
                                       this.FeederManualSpeed = valueParameter;
                                       break;
                                    }
                                    case "MovementMotorLockCurrent":
                                    {
                                       this.MovementMotorLockCurrent = valueParameter;
                                       break;
                                    }
                                    case "MovementMotorMaxCurrent":
                                    {
                                       this.MovementMotorMaxCurrent = valueParameter;
                                       break;
                                    }
                                    case "MovementMotorMaxSpeed":
                                    {
                                       this.MovementMotorMaxSpeed = valueParameter;
                                       break;
                                    }
                                    case "MovementMotorCurrentPer1kRPM":
                                    {
                                       this.MovementMotorCurrentPer1kRPM = valueParameter;
                                       break;
                                    }
                                    case "MovementMotorLowSpeedScale":
                                    {
                                       this.MovementMotorLowSpeedScale = valueParameter;
                                       break;
                                    }
                                    case "MovementMotorManualJogDistance":
                                    {
                                       this.MovementMotorManualJogDistance = valueParameter;
                                       break;
                                    }
                                    case "MovementMotorManualMoveSpeed":
                                    {
                                       this.MovementMotorManualMoveSpeed = valueParameter;
                                       break;
                                    }
                                 }
                              }
                              
                              break;
                           }
                           case "RobotApplication":
                           {
                              this.RobotApplication = ReadRobotApplication(reader);
                              break;
                           }
                           case "JoystickDeadband":
                           {
                              this.JoystickDeadband = this.ReadInt(reader);
                              break;
                           }
                           case "Latitude":
                           {
                              this.Latitude = this.ReadDouble(reader);
                              break;
                           }
                           case "Longitude":
                           {
                              this.Longitude = this.ReadDouble(reader);
                              break;
                           }
                           case "GuideMomentaryButtonAction":
                           {
                              this.ReadElement(reader, ref this.GuideMomentaryButtonAction);
                              break;
                           }
                           case "ReelMotionMode":
                           {
                              this.ReelMotionMode = this.ReadMotorForwardControl(reader);
                              break;
                           }
                           case "FeederAutomaticTracking":
                           {
                              this.ReadElement(reader, ref this.FeederAutomaticTracking);
                              break;
                           }
                           case "FeederVelocityToRpm":
                           {
                              this.FeederVelocityToRpm = this.ReadDouble(reader);
                              break;
                           }
                           case "FeederMotor":
                           {
                              FeederMotorParameters feederMotorParameters = this.ReadFeederMotorParameters(reader);

                              if (null != feederMotorParameters)
                              {
                                 switch (feederMotorParameters.Location)
                                 {
                                    case "TopFront":
                                    {
                                       this.TopFrontFeederMotor = feederMotorParameters;
                                       break;
                                    }
                                    case "TopRear":
                                    {
                                       this.TopRearFeederMotor = feederMotorParameters;
                                       break;
                                    }
                                    case "BottomFront":
                                    {
                                       this.BottomFrontFeederMotor = feederMotorParameters;
                                       break;
                                    }
                                    case "BottomRear":
                                    {
                                       this.BottomRearFeederMotor = feederMotorParameters;
                                       break;
                                    }
                                 }
                              }

                              break;
                           }
                           case "MovementMotor":
                           {
                              MovementMotorParameters movementMotorParameters = this.ReadMovementMotorParameters(reader);

                              if (null != movementMotorParameters)
                              {
                                 switch (movementMotorParameters.Location)
                                 {
                                    case "TopFront":
                                    {
                                       this.TopFrontMovementMotor = movementMotorParameters;
                                       break;
                                    }
                                    case "TopRear":
                                    {
                                       this.TopRearMovementMotor = movementMotorParameters;
                                       break;
                                    }
                                    case "BottomFront":
                                    {
                                       this.BottomFrontMovementMotor = movementMotorParameters;
                                       break;
                                    }
                                    case "BottomRear":
                                    {
                                       this.BottomRearMovementMotor = movementMotorParameters;
                                       break;
                                    }
                                 }
                              }

                              break;
                           }
                           case "MovementMotorVelocityToRpm":
                           {
                              this.MovementMotorVelocityToRpm = this.ReadDouble(reader);
                              break;
                           }
                           case "FrontToolSelected":
                           {
                              this.FrontToolSelected = this.ReadBool(reader);
                              break;
                           }
                           case "Drill":
                           {
                              DrillParameters drillParameters = this.ReadDrillParameters(reader);

                              if (null != drillParameters)
                              {
                                 switch (drillParameters.Location)
                                 {
                                    case "Front":
                                    {
                                       this.FrontDrill = drillParameters;
                                       break;
                                    }
                                    case "Rear":
                                    {
                                       this.RearDrill = drillParameters;
                                       break;
                                    }
                                 }
                              }

                              break;
                           }
                           case "Pump":
                           {
                              PumpParameters pumpParameters = this.ReadPumpParameters(reader);

                              if (null != pumpParameters)
                              {
                                 switch (pumpParameters.Location)
                                 {
                                    case "Front":
                                    {
                                       this.FrontPump = pumpParameters;
                                       break;
                                    }
                                    case "Rear":
                                    {
                                       this.RearPump = pumpParameters;
                                       break;
                                    }
                                 }
                              }
                              
                              break;
                           }
                           case "Osd":
                           {
                              OsdParameters osdParameters = this.ReadOsdParameters(reader);

                              if (null != osdParameters)
                              {
                                 this.Osd = osdParameters;
                              }
                              
                              break;
                           }
                           case "Lights":
                           {
                              this.ReadLightParameters(reader);
                              break;
                           }
                        }
                     }
                     else
                     {
#if false
                        switch (reader.Name)
                        {
                           case "Drills":
                           {
                              break;
                           }
                           case "Drill":
                           {
                              break;
                           }
                        }
#endif
                     }
                  }
               }
               #endregion

               reader.Close();
               reader.Dispose();
            }
         }
         catch { }
      }

      #endregion

      #region Write Functions

      private void WriteElement(XmlWriter writer, string tag, string value)
      {
         writer.WriteElementString(tag, value);
      }

      private void WriteElement(XmlWriter writer, string tag, double value)
      {
         writer.WriteElementString(tag, value.ToString());
      }

      private void WriteElement(XmlWriter writer, string tag, int value)
      {
         writer.WriteElementString(tag, value.ToString());
      }

      private void WriteElement(XmlWriter writer, string tag, bool value)
      {
         string fileValue = (false != value) ? "1" : "0";
         writer.WriteElementString(tag, fileValue);
      }

      private void WriteTruckBusParameters(XmlWriter writer, TruckBusParameters truckBusParameters)
      {
         writer.WriteStartElement("TruckBus");

         writer.WriteComment("BusInterface from {USBA, USBB, PCIA, PCIB}");
         this.WriteElement(writer, "BusInterface", truckBusParameters.BusInterface.ToString());
         this.WriteElement(writer, "BitRate", truckBusParameters.BitRate);

         writer.WriteComment("Set heartbeat rates to 0 to disable while debugging");
         this.WriteElement(writer, "ConsumerHeartbeatRate", truckBusParameters.ConsumerHeartbeatRate);
         this.WriteElement(writer, "ProducerHeartbeatRate", truckBusParameters.ProducerHeartbeatRate);
         this.WriteElement(writer, "ControllerBusId", truckBusParameters.ControllerBusId);         
         
         this.WriteElement(writer, "ReelMotorBusId", truckBusParameters.ReelMotorBusId);
         this.WriteElement(writer, "ReelDigitalBusId", truckBusParameters.ReelDigitalBusId);
         this.WriteElement(writer, "ReelAnalogBusId", truckBusParameters.ReelAnalogBusId);
         this.WriteElement(writer, "ReelEncoderBusId", truckBusParameters.ReelEncoderBusId);
         this.WriteElement(writer, "FeederTopFrontMotorBusId", truckBusParameters.FeederTopFrontMotorBusId);
         this.WriteElement(writer, "FeederTopRearMotorBusId", truckBusParameters.FeederTopRearMotorBusId);
         this.WriteElement(writer, "FeederBottomFrontMotorBusId", truckBusParameters.FeederBottomFrontMotorBusId);
         this.WriteElement(writer, "FeederBottomRearMotorBusId", truckBusParameters.FeederBottomRearMotorBusId);
         this.WriteElement(writer, "FeederEncoderBusId", truckBusParameters.FeederEncoderBusId);
         this.WriteElement(writer, "GuideLeftMotorBusId", truckBusParameters.GuideLeftMotorBusId);
         this.WriteElement(writer, "GuideRightMotorBusId", truckBusParameters.GuideRightMotorBusId);
         this.WriteElement(writer, "LaunchDigitalIoBusId", truckBusParameters.LaunchDigitalIoBusId);
         this.WriteElement(writer, "LaunchAnalogIoBusId", truckBusParameters.LaunchAnalogIoBusId);
         this.WriteElement(writer, "GpsBusId", truckBusParameters.GpsBusId);
         this.WriteElement(writer, "FrontPumpBusId", truckBusParameters.FrontPumpBusId);
         this.WriteElement(writer, "FrontScaleRs232BusId", truckBusParameters.FrontScaleRs232BusId);
         this.WriteElement(writer, "RearPumpBusId", truckBusParameters.RearPumpBusId);
         this.WriteElement(writer, "RearScaleRs232BusId", truckBusParameters.RearScaleRs232BusId);

         this.WriteElement(writer, "ControllerTraceMask", truckBusParameters.ControllerTraceMask);
         this.WriteElement(writer, "ReelMotorTraceMask", truckBusParameters.ReelMotorTraceMask);
         this.WriteElement(writer, "ReelDigitalTraceMask", truckBusParameters.ReelDigitalTraceMask);
         this.WriteElement(writer, "ReelAnalogTraceMask", truckBusParameters.ReelAnalogTraceMask);
         this.WriteElement(writer, "ReelEncoderTraceMask", truckBusParameters.ReelEncoderTraceMask);
         this.WriteElement(writer, "FeederTopFrontMotorTraceMask", truckBusParameters.FeederTopFrontMotorTraceMask);
         this.WriteElement(writer, "FeederTopRearMotorTraceMask", truckBusParameters.FeederTopRearMotorTraceMask);
         this.WriteElement(writer, "FeederBottomFrontMotorTraceMask", truckBusParameters.FeederBottomFrontMotorTraceMask);
         this.WriteElement(writer, "FeederBottomRearMotorTraceMask", truckBusParameters.FeederBottomRearMotorTraceMask);
         this.WriteElement(writer, "FeederEncoderTraceMask", truckBusParameters.FeederEncoderTraceMask);
         this.WriteElement(writer, "GuideLeftMotorTraceMask", truckBusParameters.GuideLeftMotorTraceMask);
         this.WriteElement(writer, "GuideRightMotorTraceMask", truckBusParameters.GuideRightMotorTraceMask);
         this.WriteElement(writer, "LaunchDigitalIoTraceMask", truckBusParameters.LaunchDigitalIoTraceMask);
         this.WriteElement(writer, "LaunchAnalogIoTraceMask", truckBusParameters.LaunchAnalogIoTraceMask);
         this.WriteElement(writer, "GpsTraceMask", truckBusParameters.GpsTraceMask);
         this.WriteElement(writer, "FrontPumpTraceMask", truckBusParameters.FrontPumpTraceMask);
         this.WriteElement(writer, "FrontScaleRs232TraceMask", truckBusParameters.FrontScaleRs232TraceMask);
         this.WriteElement(writer, "RearPumpTraceMask", truckBusParameters.RearPumpTraceMask);
         this.WriteElement(writer, "RearScaleRs232TraceMask", truckBusParameters.RearScaleRs232TraceMask);

         writer.WriteEndElement();
      }

      private void WriteRobotBusParameters(XmlWriter writer, RobotBusParameters robotBusParameters)
      {
         writer.WriteStartElement("RobotBus");

         writer.WriteComment("BusInterface from {USBA, USBB, PCIA, PCIB}");
         this.WriteElement(writer, "BusInterface", robotBusParameters.BusInterface.ToString());
         this.WriteElement(writer, "BitRate", robotBusParameters.BitRate);

         writer.WriteComment("Set heartbeat rates to 0 to disable while debugging");
         this.WriteElement(writer, "ConsumerHeartbeatRate", robotBusParameters.ConsumerHeartbeatRate);         
         this.WriteElement(writer, "ProducerHeartbeatRate", robotBusParameters.ProducerHeartbeatRate);         
         this.WriteElement(writer, "ControllerBusId", robotBusParameters.ControllerBusId);         

         this.WriteElement(writer, "RobotBodyBusId", robotBusParameters.RobotBodyBusId);
         this.WriteElement(writer, "RobotTopFrontWheelBusId", robotBusParameters.RobotTopFrontWheelBusId);
         this.WriteElement(writer, "RobotTopRearWheelBusId", robotBusParameters.RobotTopRearWheelBusId);
         this.WriteElement(writer, "RobotBottomFrontWheelBusId", robotBusParameters.RobotBottomFrontWheelBusId);
         this.WriteElement(writer, "RobotBottomRearWheelBusId", robotBusParameters.RobotBottomRearWheelBusId);

         this.WriteElement(writer, "ControllerTraceMask", robotBusParameters.ControllerTraceMask);
         this.WriteElement(writer, "RobotBodyTraceMask", robotBusParameters.RobotBodyTraceMask);
         this.WriteElement(writer, "RobotTopFrontWheelTraceMask", robotBusParameters.RobotTopFrontWheelTraceMask);
         this.WriteElement(writer, "RobotTopRearWheelTraceMask", robotBusParameters.RobotTopRearWheelTraceMask);
         this.WriteElement(writer, "RobotBottomFrontWheelTraceMask", robotBusParameters.RobotBottomFrontWheelTraceMask);
         this.WriteElement(writer, "RobotBottomRearWheelTraceMask", robotBusParameters.RobotBottomRearWheelTraceMask);

         writer.WriteEndElement();
      }

      private void WriteIpEndpointParameters(XmlWriter writer, IpEndpointParameters ipEndpointParameters)
      {
         writer.WriteStartElement("IpEndpoint");

         this.WriteElement(writer, "Name", ipEndpointParameters.Name);
         this.WriteElement(writer, "Address", ipEndpointParameters.Address);
         this.WriteElement(writer, "Port", ipEndpointParameters.Port);

         writer.WriteEndElement();
      }

      private void WriteDigitalScaleParameters(XmlWriter writer, DigitalScaleParameters digitalScaleParameters)
      {
         writer.WriteStartElement("DigitalScale");

         this.WriteElement(writer, "Location", digitalScaleParameters.Location);
         this.WriteElement(writer, "Port", digitalScaleParameters.Port);
         this.WriteElement(writer, "BaudRate", digitalScaleParameters.BaudRate);

         writer.WriteEndElement();
      }

      private void WriteCautionParameters(XmlWriter writer, CautionParameter cautionParameter)
      {
         writer.WriteStartElement("Caution");

         this.WriteElement(writer, "Name", cautionParameter.Name);
         this.WriteElement(writer, "DangerHigh", cautionParameter.DangerHighLimit);
         this.WriteElement(writer, "WarningHigh", cautionParameter.WarningHighLimit);
         this.WriteElement(writer, "WarningLow", cautionParameter.WarningLowLimit);
         this.WriteElement(writer, "DangerLow", cautionParameter.DangerLowLimit);

         writer.WriteEndElement();
      }

      private void WriteValueParameters(XmlWriter writer, ValueParameter valueParameter)
      {
         writer.WriteStartElement("Value");

         this.WriteElement(writer, "Name", valueParameter.Name);
         this.WriteElement(writer, "Unit", valueParameter.Unit);
         this.WriteElement(writer, "Precision", valueParameter.Precision);
         this.WriteElement(writer, "MinimumValue", valueParameter.MinimumValue);
         this.WriteElement(writer, "MaximumValue", valueParameter.MaximumValue);
         this.WriteElement(writer, "StepValue", valueParameter.StepValue);
         this.WriteElement(writer, "DefaultValue", valueParameter.DefaultValue);
         this.WriteElement(writer, "OperationalValue", valueParameter.OperationalValue);

         writer.WriteEndElement();
      }

      private void WriteFeederMotorParameters(XmlWriter writer, FeederMotorParameters feederMotorParameters)
      {
         writer.WriteStartElement("FeederMotor");

         this.WriteElement(writer, "Location", feederMotorParameters.Location);

         this.WriteElement(writer, "State", feederMotorParameters.State.ToString());
         this.WriteElement(writer, "Direction", feederMotorParameters.Direction.ToString());
         this.WriteElement(writer, "PositivePusher", feederMotorParameters.PositivePusher);
         this.WriteElement(writer, "PositionInversion", feederMotorParameters.PositionInversion);

         writer.WriteEndElement();
      }

      private void WriteMovementMotorParameters(XmlWriter writer, MovementMotorParameters movementMotorParameters)
      {
         writer.WriteStartElement("MovementMotor");

         this.WriteElement(writer, "Location", movementMotorParameters.Location);

         this.WriteElement(writer, "State", movementMotorParameters.State.ToString());
         this.WriteElement(writer, "Direction", movementMotorParameters.Direction.ToString());
         this.WriteElement(writer, "AxialMode", movementMotorParameters.AxialMode.ToString());
         this.WriteElement(writer, "CircumferentialMode", movementMotorParameters.CircumferentialMode.ToString());
         this.WriteElement(writer, "CornerAxialMode", movementMotorParameters.CornerAxialMode.ToString());
         this.WriteElement(writer, "LaunchAxialMode", movementMotorParameters.LaunchAxialMode.ToString());

         writer.WriteEndElement();
      }

      private void WriteDrillParameters(XmlWriter writer, DrillParameters drillParameters)
      {
         writer.WriteStartElement("Drill");

         this.WriteElement(writer, "Location", drillParameters.Location);

         this.WriteValueParameters(writer, drillParameters.Acceleration);
         this.WriteValueParameters(writer, drillParameters.ErrorLimit);
         this.WriteValueParameters(writer, drillParameters.ProportionalControlConstant);
         this.WriteValueParameters(writer, drillParameters.IntegralControlConstant);
         this.WriteValueParameters(writer, drillParameters.DerivativeControlConstant);

         this.WriteValueParameters(writer, drillParameters.RotationSpeed);
         this.WriteValueParameters(writer, drillParameters.SearchSpeed);
         this.WriteValueParameters(writer, drillParameters.TravelSpeed);
         this.WriteValueParameters(writer, drillParameters.CuttingSpeed);

         this.WriteElement(writer, "SpeedToVelocityCount", drillParameters.SpeedToVelocityCount);

         this.WriteElement(writer, "AutoOrign", drillParameters.AutoOrign);
         this.WriteElement(writer, "PeckMode", drillParameters.PeckMode);
         this.WriteElement(writer, "PositionRetract", drillParameters.PositionRetract);

         this.WriteValueParameters(writer, drillParameters.CuttingDepth);
         this.WriteValueParameters(writer, drillParameters.CuttingIncrement);
         this.WriteValueParameters(writer, drillParameters.RetractDistance);
         this.WriteValueParameters(writer, drillParameters.RetractPosition);

         this.WriteValueParameters(writer, drillParameters.ExtendedDistance);

         writer.WriteEndElement();
      }

      private void WritePumpParameters(XmlWriter writer, PumpParameters pumpParameters)
      {
         writer.WriteStartElement("Pump");

         this.WriteElement(writer, "Location", pumpParameters.Location);

         this.WriteElement(writer, "PressureAutoFill", pumpParameters.PressureAutoFill);
         this.WriteElement(writer, "AutoNozzleRetraction", pumpParameters.AutoNozzleRetraction);
         this.WriteElement(writer, "AutoPressureRelief", pumpParameters.AutoPressureRelief);
         this.WriteElement(writer, "RpmPerVolt", pumpParameters.RpmPerVolt);
         this.WriteElement(writer, "PsiPerVolt", pumpParameters.PsiPerVolt);

         this.WriteValueParameters(writer, pumpParameters.AutoFillVolume);
         this.WriteValueParameters(writer, pumpParameters.MaximumVolume);
         this.WriteValueParameters(writer, pumpParameters.AutoFillPressure);
         this.WriteValueParameters(writer, pumpParameters.MaximumPressure);

         this.WriteValueParameters(writer, pumpParameters.RelievedPressure);
         this.WriteValueParameters(writer, pumpParameters.ForwardSpeed);
         this.WriteValueParameters(writer, pumpParameters.ReverseSpeed);
         this.WriteValueParameters(writer, pumpParameters.MaximumSpeed);

         this.WriteValueParameters(writer, pumpParameters.SealantWeight);
         this.WriteValueParameters(writer, pumpParameters.FlowConstant);
         
         writer.WriteEndElement();
      }

      private void WriteOsdParameters(XmlWriter writer, OsdParameters osdParameters)
      {
         writer.WriteStartElement("Osd");

         this.WriteElement(writer, "HozizontalOffset", osdParameters.HozizontalOffset);
         this.WriteElement(writer, "VerticalOffset", osdParameters.VerticalOffset);

         this.WriteElement(writer, "ShowDate", osdParameters.ShowDate);
         this.WriteElement(writer, "ShowTime", osdParameters.ShowTime);
         this.WriteElement(writer, "ShowCameraId", osdParameters.ShowCameraId);
         this.WriteElement(writer, "ShowDescription", osdParameters.ShowDescription);
         this.WriteElement(writer, "ShowPipeDisplacement", osdParameters.ShowPipeDisplacement);
         this.WriteElement(writer, "ShowPipePosition", osdParameters.ShowPipePosition);

         this.WriteElement(writer, "Line1", osdParameters.Line1);
         this.WriteElement(writer, "Line2", osdParameters.Line2);
         this.WriteElement(writer, "Line3", osdParameters.Line3);
         this.WriteElement(writer, "Line4", osdParameters.Line4);
         this.WriteElement(writer, "Line5", osdParameters.Line5);

         writer.WriteEndElement();
      }

      private void WriteLightParamters(XmlWriter writer)
      {
         writer.WriteStartElement("Lights");

         this.WriteValueParameters(writer, this.robotLightLevels[0]);
         this.WriteValueParameters(writer, this.robotLightLevels[1]);
         this.WriteValueParameters(writer, this.robotLightLevels[2]);
         this.WriteValueParameters(writer, this.robotLightLevels[3]);
         this.WriteValueParameters(writer, this.robotLightLevels[4]);
         this.WriteValueParameters(writer, this.robotLightLevels[5]);
         this.WriteValueParameters(writer, this.robotLightLevels[6]);
         this.WriteValueParameters(writer, this.robotLightLevels[7]);
         this.WriteValueParameters(writer, this.robotLightLevels[8]);
         this.WriteValueParameters(writer, this.robotLightLevels[9]);
         this.WriteValueParameters(writer, this.robotLightLevels[10]);
         this.WriteValueParameters(writer, this.robotLightLevels[11]);

         this.WriteValueParameters(writer, this.launchLightLevels[0]);
         this.WriteValueParameters(writer, this.launchLightLevels[1]);
         this.WriteValueParameters(writer, this.launchLightLevels[2]);
         this.WriteValueParameters(writer, this.launchLightLevels[3]);

         writer.WriteEndElement();
      }

      private void WriteData(string filePath)
      {
         XmlWriterSettings xmls = new XmlWriterSettings();
         xmls.Indent = true;

         using (XmlWriter writer = XmlWriter.Create(filePath, xmls))
         {
            writer.WriteStartDocument();
            writer.WriteStartElement("Configuration");

            writer.WriteComment("RobotApplication from {repair, inspect}");
            this.WriteElement(writer, "RobotApplication", this.RobotApplication.ToString());

            this.WriteTruckBusParameters(writer, this.TruckBus);
            this.WriteRobotBusParameters(writer, this.RobotBus);

            this.WriteElement(writer, "JoystickDeadband", this.JoystickDeadband);
            this.WriteElement(writer, "Latitude", this.Latitude);
            this.WriteElement(writer, "Longitude", this.Longitude);

            this.WriteIpEndpointParameters(writer, this.Trace);
            this.WriteIpEndpointParameters(writer, this.LocationServer);

            this.WriteIpEndpointParameters(writer, this.ThicknessSensor);
            this.WriteValueParameters(writer, this.ThicknessConversionUnit);

            this.WriteIpEndpointParameters(writer, this.StressSensor);
            this.WriteValueParameters(writer, this.StressConversionUnit);

            this.WriteDigitalScaleParameters(writer, this.FrontScale);
            this.WriteDigitalScaleParameters(writer, this.RearScale);

            this.WriteValueParameters(writer, this.NitrogenPressureConversionUnit);
            this.WriteCautionParameters(writer, this.NitrogenPressureCaution);

            this.WriteValueParameters(writer, this.RobotTotalCurrentConversionUnit);
            this.WriteValueParameters(writer, this.LaunchTotalCurrentConversionUnit);

            this.WriteValueParameters(writer, this.GuideExtensionSpeed);
            this.WriteValueParameters(writer, this.GuideRetractionSpeed);
            this.WriteElement(writer, "GuideMomentaryButtonAction", this.GuideMomentaryButtonAction);


            this.WriteValueParameters(writer, this.ReelDistance);
            this.WriteValueParameters(writer, this.ReelDistanceScale);
            this.WriteElement(writer, "ReelMotionMode", this.ReelMotionMode.ToString());
            this.WriteValueParameters(writer, this.ReelReverseCurrent);
            this.WriteValueParameters(writer, this.ReelReverseSpeed);
            this.WriteValueParameters(writer, this.ReelLockCurrent);
            this.WriteValueParameters(writer, this.ReelCalibrationDistance);
            this.WriteValueParameters(writer, this.ReelManualCurrent);
            this.WriteValueParameters(writer, this.ReelManualSpeed);            


            this.WriteElement(writer, "FeederAutomaticTracking", this.FeederAutomaticTracking);
            this.WriteElement(writer, "FeederVelocityToRpm", this.FeederVelocityToRpm);
            this.WriteValueParameters(writer, this.FeederTrackingCalibration);
            this.WriteValueParameters(writer, this.FeederMaxSpeed);
            this.WriteValueParameters(writer, this.FeederLowSpeedScale);
            this.WriteValueParameters(writer, this.FeederLockCurrent);
            this.WriteValueParameters(writer, this.FeederCurrentPer1kRPM);
            this.WriteValueParameters(writer, this.FeederManualSpeed);
            this.WriteCautionParameters(writer, this.FeederCurrentCaution);

            
            this.WriteFeederMotorParameters(writer, this.TopFrontFeederMotor);
            this.WriteFeederMotorParameters(writer, this.TopRearFeederMotor);
            this.WriteFeederMotorParameters(writer, this.BottomFrontFeederMotor);
            this.WriteFeederMotorParameters(writer, this.BottomRearFeederMotor);


            this.WriteValueParameters(writer, this.MovementMotorLockCurrent);
            this.WriteValueParameters(writer, this.MovementMotorMaxCurrent);
            this.WriteValueParameters(writer, this.MovementMotorMaxSpeed);
            this.WriteValueParameters(writer, this.MovementMotorCurrentPer1kRPM);
            this.WriteValueParameters(writer, this.MovementMotorLowSpeedScale);
            this.WriteValueParameters(writer, this.MovementMotorManualJogDistance);
            this.WriteValueParameters(writer, this.MovementMotorManualMoveSpeed);
            this.WriteElement(writer, "MovementMotorVelocityToRpm", this.MovementMotorVelocityToRpm);
            

            this.WriteMovementMotorParameters(writer, this.TopFrontMovementMotor);
            this.WriteMovementMotorParameters(writer, this.TopRearMovementMotor);
            this.WriteMovementMotorParameters(writer, this.BottomFrontMovementMotor);
            this.WriteMovementMotorParameters(writer, this.BottomRearMovementMotor);
            this.WriteCautionParameters(writer, this.MovementCurrentCaution);
            this.WriteCautionParameters(writer, this.MovementTemperatureCaution);

            
            this.WriteElement(writer, "FrontToolSelected", this.FrontToolSelected);

            this.WriteDrillParameters(writer, this.FrontDrill);
            this.WriteDrillParameters(writer, this.RearDrill);

            this.WritePumpParameters(writer, this.FrontPump);
            this.WritePumpParameters(writer, this.RearPump);

            this.WriteOsdParameters(writer, this.Osd);
            
            this.WriteLightParamters(writer);

            writer.WriteEndElement();
            writer.WriteEndDocument();
         }
      }

      #endregion

      #region Constructor

      private ParameterAccessor()
      {
      }

      #endregion

      #region Access Methods

      public void Read(string filePath)
      {
         if (false != this.setDefaults)
         {
            this.AssignDefaults(this.RobotApplication);
            this.setDefaults = false;
         }
         else
         {
            string configFile = filePath + ".ini";

            if (File.Exists(configFile) != false)
            {
               this.ReadData(filePath + ".ini");
            }
         }
      }

      public void Write(string filePath)
      {
         this.WriteData(filePath + ".ini");
      }

      /// <summary>
      /// Function to trigger assignment of default values.
      /// </summary>
      /// <remarks>
      /// Default values assigned on next read.
      /// </remarks>
      public void TriggerDefaults()
      {
         this.setDefaults = true;
      }

      public MovementMotorParameters GetMovementMotionParameters(int index)
      {
         MovementMotorParameters result = this.TopFrontMovementMotor;

         if (0 == index)
         {
            result = this.TopFrontMovementMotor;
         }
         else if (1 == index)
         {
            result = this.TopRearMovementMotor;
         }
         else if (2 == index)
         {
            result = this.BottomFrontMovementMotor;
         }
         else if (3 == index)
         {
            result = this.BottomRearMovementMotor;
         }

         return (result);
      }

      public MovementForwardControls GetMovementForwardControl(MovementMotorParameters parameters, MovementForwardModes mode)
      {
         MovementForwardControls result = MovementForwardControls.velocity;

         if (MovementForwardModes.normalAxial == mode)
         {
            result = parameters.AxialMode;
         }
         else if (MovementForwardModes.circumferential == mode)
         {
            result = parameters.CircumferentialMode;
         }
         else if (MovementForwardModes.cornerAxial == mode)
         {
            result = parameters.CornerAxialMode;
         }
         else if (MovementForwardModes.launchAxial == mode)
         {
            result = parameters.LaunchAxialMode;
         }

         return (result);
      }

      public double GetLightLevel(CameraLocations camera)
      {
         double result = 0;
         ValueParameter value = this.GetLightValue(camera);

         if (null != value)
         {
            result = value.OperationalValue;
         }

         return (result);
      }

      public ValueParameter GetLightValue(CameraLocations camera)
      {
         ValueParameter result = null;

         if (CameraLocations.robotFrontUpperBack == camera)
         {
            result = this.robotLightLevels[0];
         }
         else if (CameraLocations.robotLowerBack == camera)
         {
            result = this.robotLightLevels[1];
         }
         else if (CameraLocations.robotFrontUpperDown == camera)
         {
            result = this.robotLightLevels[2];
         }
         else if (CameraLocations.robotRearUpperForward == camera)
         {
            result = this.robotLightLevels[3];
         }
         else if (CameraLocations.robotRearUpperDown == camera)
         {
            result = this.robotLightLevels[4];
         }
         else if (CameraLocations.robotFffDrill == camera)
         {
            result = this.robotLightLevels[5];
         }
         else if (CameraLocations.robotRearUpperBack == camera)
         {
            result = this.robotLightLevels[6];
         }
         else if (CameraLocations.robotFrontUpperForward == camera)
         {
            result = this.robotLightLevels[7];
         }
         else if ((CameraLocations.robotSensorArm == camera) ||
                  (CameraLocations.robotRffDrill == camera))  
         {
            result = this.robotLightLevels[8];
         }
         else if (CameraLocations.robotLowerForward == camera)
         {
            result = this.robotLightLevels[9];
         }
         else if (CameraLocations.robotFrfDrill == camera)
         {
            result = this.robotLightLevels[10];
         }
         else if ((CameraLocations.robotSensorBay == camera) ||
                  (CameraLocations.robotRrfDrill == camera))
         {
            result = this.robotLightLevels[11];
         }
         else if (CameraLocations.launchLeftGuide == camera)
         {
            result = this.launchLightLevels[0];
         }
         else if (CameraLocations.launchRightGuide == camera)
         {
            result = this.launchLightLevels[1];
         }
         else if (CameraLocations.launchFeeder == camera)
         {
            result = this.launchLightLevels[2];
         }
         else if (CameraLocations.launchMain == camera)
         {
            result = this.launchLightLevels[3];
         }

         return (result);
      }

      #endregion

   }
}
