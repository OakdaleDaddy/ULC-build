namespace Weco.Ui
{
   using System;
   using System.Collections.Generic;
   using System.IO;
   using System.Xml;

   using Weco.PCANLight;
   using Weco.Utilities;

   public class ParameterAccessor
   {
      #region Fields

      private static ParameterAccessor instance = null;

      private string filePath;
      private bool setDefaults;

      private int VersionCount;

      public LaserBusParameters LaserBus;
      public TargetBusParameters TargetBus;
      public bool RunTargetOnLaserBus;

      public IpEndpointParameters Trace;

      public string SessionDataPath;
      public int JoystickDeadband;
      public int JoystickIdleBand;
      public int UsbRelayPort;
      public double Latitude;
      public double Longitude;

      public WheelMotorParameters LaserFrontWheel;
      public WheelMotorParameters LaserRearWheel;
      public ValueParameter LaserWheelMaximumSpeed;
      public ValueParameter LaserWheelLowSpeedScale;
      public ValueParameter LaserWheelManualWheelDistance;
      public ValueParameter LaserWheelManualWheelSpeed;
      public double LaserWheelDistanceToTicks;
      public double LaserWheelVelocityToRpm;
      public double LaserWheelCountsToAmps;
      public double LaserLinkVoltageMultipler;
      public double LaserLightPercentToCount;
      public double LaserStepperPivotAngle;
      
      public CameraMaps CrawlerHubCameraMaps;
      public LightSelectParameters CrawlerFrontLight;
      public LightSelectParameters CrawlerRearLight;
      public LightSelectParameters CrawlerLeftLight;
      public LightSelectParameters CrawlerRightLight;
      public Controls.SystemLocations CrawlerHubSelectedCamera;

      public WheelMotorParameters TargetFrontWheel;
      public WheelMotorParameters TargetRearWheel;
      public ValueParameter TargetWheelMaximumSpeed;
      public ValueParameter TargetWheelLowSpeedScale;
      public ValueParameter TargetWheelManualWheelDistance;
      public ValueParameter TargetWheelManualWheelSpeed;
      public double TargetWheelDistanceToTicks;
      public double TargetWheelVelocityToRpm;
      public double TargetWheelCountsToAmps;
      public double TargetLinkVoltageMultipler;
      public double TargetLightPercentToCount;
      
      public CameraMaps BulletCameraMaps;
      public LightSelectParameters BulletLeftLight;
      public LightSelectParameters BulletRightLight;
      public LightSelectParameters BulletDownLight;
      public Controls.SystemLocations BulletSelectedCamera;
      public double TargetTopCameraCwLimit;
      public double TargetTopCameraCcwLimit;

      public bool LaserMeasureAutoFrequency;
      public ValueParameter LaserMeasureManualFrequency;
      public ValueParameter LaserSampleCount;
      public ValueParameter LaserMeasurementConstant;

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

      private string GetDefaultFilePath()
      {
         string result = this.filePath + ".defaults" + ".ini";
         return (result);
      }

      private void AssignDefaults()
      {
         this.VersionCount = 1; // update after each addition

         this.LaserBus = new LaserBusParameters();
         this.LaserBus.BusInterface = BusInterfaces.PCIA;
         this.LaserBus.BitRate = 50000;
         this.LaserBus.ConsumerHeartbeatRate = 3500;
         this.LaserBus.ProducerHeartbeatRate = 1000;
         this.LaserBus.ControllerBusId = 80;
         this.LaserBus.LaserBoardBusId = 32;
         this.LaserBus.GpsBusId = 20;
         this.LaserBus.ControllerTraceMask = 0;
         this.LaserBus.MainBoardTraceMask = 1;
         this.LaserBus.GpsTraceMask = 1;
         
         this.TargetBus = new TargetBusParameters();
         this.TargetBus.BusInterface = BusInterfaces.PCIB;
         this.TargetBus.BitRate = 50000;
         this.TargetBus.ConsumerHeartbeatRate = 3500;
         this.TargetBus.ProducerHeartbeatRate = 1000;
         this.TargetBus.ControllerBusId = 80;
         this.TargetBus.TargetBoardBusId = 32;
         this.TargetBus.ControllerTraceMask = 0;
         this.TargetBus.TargetBoardTraceMask = 1;

         this.RunTargetOnLaserBus = false;


         this.Trace = new IpEndpointParameters("Trace", "127.0.0.1", 10000);

         this.SessionDataPath = @"c:\SessionData";
         this.JoystickDeadband = 5000;
         this.JoystickIdleBand = 4000;
         this.UsbRelayPort = 1;
         this.Latitude = double.NaN;
         this.Longitude = double.NaN;


         this.LaserFrontWheel = new WheelMotorParameters();
         this.LaserFrontWheel.Location = "LaserFrontWheel";
         this.LaserFrontWheel.MotorState = WheelMotorStates.enabled;
         this.LaserFrontWheel.ActuationMode = ActuationModes.closedloop;
         this.LaserFrontWheel.RequestInverted = false;
         this.LaserFrontWheel.PositionInverted = false;
         this.LaserFrontWheel.ProfileVelocity = 500;
         this.LaserFrontWheel.ProfileAcceleration = 200;
         this.LaserFrontWheel.ProfileDeceleration = 200;
         this.LaserFrontWheel.Kp = 2600;
         this.LaserFrontWheel.Ki = 22000;
         this.LaserFrontWheel.Kd = 0;
         this.LaserFrontWheel.Polarity = 0;
         this.LaserFrontWheel.PositionNotationIndex = -3; // mm
         this.LaserFrontWheel.VelocityNotationIndex = -3; // mm
         this.LaserFrontWheel.VelocityDimensionIndex = 164; // /min
         this.LaserFrontWheel.AccelerationNotationIndex = 0;// none
         this.LaserFrontWheel.AccelerationDimensionIndex = 163; // /s
         this.LaserFrontWheel.PositionEncoderIncrements = 6;
         this.LaserFrontWheel.PositionEncoderMotorRevolutions = 1;
         this.LaserFrontWheel.VelocityEncoderIncrementsPerSecond = 6;
         this.LaserFrontWheel.VelocityEncoderMotorRevolutionsPerSecond = 1;
         this.LaserFrontWheel.GearRatioMotorRevolutions = 6;
         this.LaserFrontWheel.GearRatioShaftRevolutions = 1;
         this.LaserFrontWheel.FeedConstantFeed = 600;
         this.LaserFrontWheel.FeedConstantShaftRevolutions = 1;
         this.LaserFrontWheel.MotorPeakCurrentLimit = 5000;
         this.LaserFrontWheel.MaximumCurrent = 10000;
         this.LaserFrontWheel.MotorRatedCurrent = 1000;

         this.LaserRearWheel = new WheelMotorParameters();
         this.LaserRearWheel.Location = "LaserRearWheel";
         this.LaserRearWheel.MotorState = WheelMotorStates.enabled;
         this.LaserRearWheel.ActuationMode = ActuationModes.closedloop;
         this.LaserRearWheel.RequestInverted = false;
         this.LaserRearWheel.PositionInverted = true;
         this.LaserRearWheel.ProfileVelocity = 500;
         this.LaserRearWheel.ProfileAcceleration = 200;
         this.LaserRearWheel.ProfileDeceleration = 200;
         this.LaserRearWheel.Kp = 2600;
         this.LaserRearWheel.Ki = 22000;
         this.LaserRearWheel.Kd = 0;
         this.LaserRearWheel.Polarity = 0;
         this.LaserRearWheel.PositionNotationIndex = -3; // mm
         this.LaserRearWheel.VelocityNotationIndex = -3; // mm
         this.LaserRearWheel.VelocityDimensionIndex = 164; // /min
         this.LaserRearWheel.AccelerationNotationIndex = 0; // none
         this.LaserRearWheel.AccelerationDimensionIndex = 163; // /s
         this.LaserRearWheel.PositionEncoderIncrements = 6;
         this.LaserRearWheel.PositionEncoderMotorRevolutions = 1;
         this.LaserRearWheel.VelocityEncoderIncrementsPerSecond = 6;
         this.LaserRearWheel.VelocityEncoderMotorRevolutionsPerSecond = 1;
         this.LaserRearWheel.GearRatioMotorRevolutions = 6;
         this.LaserRearWheel.GearRatioShaftRevolutions = 1;
         this.LaserRearWheel.FeedConstantFeed = 600;
         this.LaserRearWheel.FeedConstantShaftRevolutions = 1;
         this.LaserRearWheel.MotorPeakCurrentLimit = 5000;
         this.LaserRearWheel.MaximumCurrent = 10000;
         this.LaserRearWheel.MotorRatedCurrent = 1000;

         this.LaserWheelMaximumSpeed = new ValueParameter("LaserWheelMaximumSpeed", "m/MIN", 2, 0, 10, 0.10, 3.5, 3.5);
         this.LaserWheelLowSpeedScale = new ValueParameter("LaserWheelLowSpeedScale", "%", 0, 1, 100, 1, 30, 30);
         this.LaserWheelManualWheelDistance = new ValueParameter("LaserWheelManualWheelDistance", "mm", 0, 1, 100, 1, 1, 1);
         this.LaserWheelManualWheelSpeed = new ValueParameter("LaserWheelManualWheelSpeed", "m/MIN", 2, 0, 10, 0.1, 2, 2);
         this.LaserWheelDistanceToTicks = 1;
         this.LaserWheelVelocityToRpm = 100;
         this.LaserWheelCountsToAmps = 1000;
         this.LaserLinkVoltageMultipler = 0.5;
         this.LaserLightPercentToCount = 1;
         this.LaserStepperPivotAngle = 8.5;

         CameraMap[] crawlerHubCameraMaps = new CameraMap[2];
         crawlerHubCameraMaps[0] = new CameraMap(1, Ui.Controls.SystemLocations.crawlerFront);
         crawlerHubCameraMaps[1] = new CameraMap(2, Ui.Controls.SystemLocations.crawlerRear);
         this.CrawlerHubCameraMaps = new CameraMaps("CrawlerHubCameraMaps", crawlerHubCameraMaps);

         this.CrawlerFrontLight = new LightSelectParameters("CrawlerFrontLight", 15, 1);
         this.CrawlerRearLight = new LightSelectParameters("CrawlerRearLight", 15, 2);
         this.CrawlerLeftLight = new LightSelectParameters("CrawlerLeftLight", 15, 1);
         this.CrawlerRightLight = new LightSelectParameters("CrawlerRightLight", 15, 1);
         this.CrawlerHubSelectedCamera = Controls.SystemLocations.crawlerFront;
         

         this.TargetFrontWheel = new WheelMotorParameters();
         this.TargetFrontWheel.Location = "TargetFrontWheel";
         this.TargetFrontWheel.MotorState = WheelMotorStates.enabled;
         this.TargetFrontWheel.ActuationMode = ActuationModes.closedloop;
         this.TargetFrontWheel.RequestInverted = false;
         this.TargetFrontWheel.PositionInverted = false;
         this.TargetFrontWheel.ProfileVelocity = 500;
         this.TargetFrontWheel.ProfileAcceleration = 200;
         this.TargetFrontWheel.ProfileDeceleration = 200;
         this.TargetFrontWheel.Kp = 2600;
         this.TargetFrontWheel.Ki = 22000;
         this.TargetFrontWheel.Kd = 0;
         this.TargetFrontWheel.Polarity = 0;
         this.TargetFrontWheel.PositionNotationIndex = -3; // mm
         this.TargetFrontWheel.VelocityNotationIndex = -3; // mm
         this.TargetFrontWheel.VelocityDimensionIndex = 164; // /min
         this.TargetFrontWheel.AccelerationNotationIndex = 0; // none
         this.TargetFrontWheel.AccelerationDimensionIndex = 163; // /s
         this.TargetFrontWheel.PositionEncoderIncrements = 6;
         this.TargetFrontWheel.PositionEncoderMotorRevolutions = 1;
         this.TargetFrontWheel.VelocityEncoderIncrementsPerSecond = 6;
         this.TargetFrontWheel.VelocityEncoderMotorRevolutionsPerSecond = 1;
         this.TargetFrontWheel.GearRatioMotorRevolutions = 6;
         this.TargetFrontWheel.GearRatioShaftRevolutions = 1;
         this.TargetFrontWheel.FeedConstantFeed = 600;
         this.TargetFrontWheel.FeedConstantShaftRevolutions = 1;
         this.TargetFrontWheel.MotorPeakCurrentLimit = 5000;
         this.TargetFrontWheel.MaximumCurrent = 10000;
         this.TargetFrontWheel.MotorRatedCurrent = 1000;

         this.TargetRearWheel = new WheelMotorParameters();
         this.TargetRearWheel.Location = "TargetRearWheel";
         this.TargetRearWheel.MotorState = WheelMotorStates.enabled;
         this.TargetRearWheel.ActuationMode = ActuationModes.closedloop;
         this.TargetRearWheel.RequestInverted = false;
         this.TargetRearWheel.PositionInverted = true;
         this.TargetRearWheel.ProfileVelocity = 500;
         this.TargetRearWheel.ProfileAcceleration = 200;
         this.TargetRearWheel.ProfileDeceleration = 200;
         this.TargetRearWheel.Kp = 2600;
         this.TargetRearWheel.Ki = 22000;
         this.TargetRearWheel.Kd = 0;
         this.TargetRearWheel.Polarity = 0;
         this.TargetRearWheel.PositionNotationIndex = -3; // mm
         this.TargetRearWheel.VelocityNotationIndex = -3; // mm
         this.TargetRearWheel.VelocityDimensionIndex = 164; // /min
         this.TargetRearWheel.AccelerationNotationIndex = 0; // none
         this.TargetRearWheel.AccelerationDimensionIndex = 163; // /s
         this.TargetRearWheel.PositionEncoderIncrements = 6;
         this.TargetRearWheel.PositionEncoderMotorRevolutions = 1;
         this.TargetRearWheel.VelocityEncoderIncrementsPerSecond = 6;
         this.TargetRearWheel.VelocityEncoderMotorRevolutionsPerSecond = 1;
         this.TargetRearWheel.GearRatioMotorRevolutions = 6;
         this.TargetRearWheel.GearRatioShaftRevolutions = 1;
         this.TargetRearWheel.FeedConstantFeed = 600;
         this.TargetRearWheel.FeedConstantShaftRevolutions = 1;
         this.TargetRearWheel.MotorPeakCurrentLimit = 5000;
         this.TargetRearWheel.MaximumCurrent = 10000;
         this.TargetRearWheel.MotorRatedCurrent = 1000;

         this.TargetWheelMaximumSpeed = new ValueParameter("TargetWheelMaximumSpeed", "m/MIN", 2, 0, 10, 0.10, 3.5, 3.5);
         this.TargetWheelLowSpeedScale = new ValueParameter("TargetWheelLowSpeedScale", "%", 0, 1, 100, 1, 30, 30);
         this.TargetWheelManualWheelDistance = new ValueParameter("TargetWheelManualWheelDistance", "mm", 0, 1, 100, 1, 1, 1);
         this.TargetWheelManualWheelSpeed = new ValueParameter("TargetWheelManualWheelSpeed", "m/MIN", 2, 0, 10, 0.1, 2, 2);
         this.TargetWheelDistanceToTicks = 1;
         this.TargetWheelVelocityToRpm = 100;
         this.TargetWheelCountsToAmps = 1000;
         this.TargetLinkVoltageMultipler = 0.5;
         this.TargetLightPercentToCount = 1;

         CameraMap[] bulletCameraMaps = new CameraMap[3];
         bulletCameraMaps[0] = new CameraMap(1, Ui.Controls.SystemLocations.bulletLeft);
         bulletCameraMaps[1] = new CameraMap(2, Ui.Controls.SystemLocations.bulletRight);
         bulletCameraMaps[2] = new CameraMap(3, Ui.Controls.SystemLocations.bulletDown);
         this.BulletCameraMaps = new CameraMaps("BulletCameraMaps", bulletCameraMaps);

         this.BulletLeftLight = new LightSelectParameters("BulletLeftLight", 15, 1);
         this.BulletRightLight = new LightSelectParameters("BulletRightLight", 15, 2);
         this.BulletDownLight = new LightSelectParameters("BulletDownLight", 15, 4);
         this.BulletSelectedCamera = Controls.SystemLocations.bulletDown;
         this.TargetTopCameraCwLimit = 45;
         this.TargetTopCameraCcwLimit = -45;


         this.LaserMeasureAutoFrequency = true;
         this.LaserMeasureManualFrequency = new ValueParameter("LaserMeasureManualFrequency", "Hz", 1, 0.10, 100.0, 0.1, 5.0, 5.0);
         this.LaserSampleCount = new ValueParameter("LaserSampleCount", "", 0, 1, 128, 1, 4, 4);
         this.LaserMeasurementConstant = new ValueParameter("LaserMeasurementConstant", "mm", 1, 1, 1, 1, 0.1, 0.1);
         

         this.Osd = new OsdParameters();
         this.SetOsdDefaults(ref this.Osd);
      }

      private void SetOsdDefaults(ref OsdParameters osdParameters)
      {
         osdParameters.HorizontalOffset = 35;
         osdParameters.VerticalOffset = 21;

         osdParameters.ShowDate = false;
         osdParameters.ShowTime = false;
         osdParameters.ShowDescription = false;
         osdParameters.ShowCameraId = false;

         osdParameters.Line1 = "";
         osdParameters.Line2 = "";
         osdParameters.Line3 = "";
         osdParameters.Line4 = "";
         osdParameters.Line5 = "";
         osdParameters.Line6 = "";
         osdParameters.Line7 = "";
         osdParameters.Line8 = "";
      }

      private void Initialize()
      {
         this.setDefaults = false;
         this.AssignDefaults();
      }

      #endregion

      #region Read Functions

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

      private WheelMotorStates ReadWheelMotorState(XmlReader reader)
      {
         WheelMotorStates result = WheelMotorStates.disabled;

         try
         {
            if (reader.Read())
            {
               result = (WheelMotorStates)Enum.Parse(typeof(WheelMotorStates), reader.Value.Trim());
            }
         }
         catch { }

         return (result);
      }

      private ActuationModes ReadActuationMode(XmlReader reader)
      {
         ActuationModes result = ActuationModes.closedloop;

         try
         {
            if (reader.Read())
            {
               result = (ActuationModes)Enum.Parse(typeof(ActuationModes), reader.Value.Trim());
            }
         }
         catch { }

         return (result);
      }

      private Controls.SystemLocations ReadSystemLocation(XmlReader reader)
      {
         Controls.SystemLocations result = Controls.SystemLocations.none;

         try
         {
            if (reader.Read())
            {
               result = (Controls.SystemLocations)Enum.Parse(typeof(Controls.SystemLocations), reader.Value.Trim());
            }
         }
         catch { }

         return (result);
      }

      private LaserBusParameters ReadLaserBusParameters(XmlReader reader)
      {
         LaserBusParameters result = null;
         bool readResult = true;

         BusInterfaces busInterface = BusInterfaces.USBA;
         int bitRate = 0;

         int consumerHeartbeatRate = 0;
         int producerHeartbeatRate = 0;
         int controllerBusId = 0;

         int laserBoardBusId = 0;
         int gpsBusId = 0;

         int controllerTraceMask = 0;
         int mainBoardTraceMask = 0;
         int gpsTraceMask = 0;

         for (; readResult; )
         {
            readResult = reader.Read();

            if (reader.IsStartElement())
            {
               if ("BusInterface" == reader.Name)
               {
                  busInterface = this.ReadBusInterface(reader);
               }
               else if ("BitRate" == reader.Name)
               {
                  bitRate = this.ReadInt(reader);
               }
               else if ("ConsumerHeartbeatRate" == reader.Name)
               {
                  consumerHeartbeatRate = this.ReadInt(reader);
               }
               else if ("ProducerHeartbeatRate" == reader.Name)
               {
                  producerHeartbeatRate = this.ReadInt(reader);
               }
               else if ("ControllerBusId" == reader.Name)
               {
                  controllerBusId = this.ReadInt(reader);
               }
               else if ("LaserBoardBusId" == reader.Name)
               {
                  laserBoardBusId = this.ReadInt(reader);
               }
               else if ("GpsBusId" == reader.Name)
               {
                  gpsBusId = this.ReadInt(reader);
               }                  
               else if ("ControllerTraceMask" == reader.Name)
               {
                  controllerTraceMask = this.ReadInt(reader);
               }
               else if ("MainBoardTraceMask" == reader.Name)
               {
                  mainBoardTraceMask = this.ReadInt(reader);
               }
               else if ("GpsTraceMask" == reader.Name)
               {
                  gpsTraceMask = this.ReadInt(reader);
               }               
            }
            else
            {
               if ("LaserBus" == reader.Name)
               {
                  result = new LaserBusParameters();

                  result.BusInterface = busInterface;
                  result.BitRate = bitRate;

                  result.ConsumerHeartbeatRate = consumerHeartbeatRate;
                  result.ProducerHeartbeatRate = producerHeartbeatRate;
                  result.ControllerBusId = controllerBusId;

                  result.LaserBoardBusId = laserBoardBusId;
                  result.GpsBusId = gpsBusId;

                  result.ControllerTraceMask = controllerTraceMask;
                  result.MainBoardTraceMask = mainBoardTraceMask;
                  result.GpsTraceMask = gpsTraceMask;

                  break;
               }
            }
         }

         return (result);
      }

      private TargetBusParameters ReadTargetBusParameters(XmlReader reader)
      {
         TargetBusParameters result = null;
         bool readResult = true;

         BusInterfaces busInterface = BusInterfaces.USBA;
         int bitRate = 0;

         int consumerHeartbeatRate = 0;
         int producerHeartbeatRate = 0;
         int controllerBusId = 0;

         int targetBoardBusId = 0;

         int controllerTraceMask = 0;
         int targetBoardTraceMask = 0;

         for (; readResult; )
         {
            readResult = reader.Read();

            if (reader.IsStartElement())
            {
               if ("BusInterface" == reader.Name)
               {
                  busInterface = this.ReadBusInterface(reader);
               }
               else if ("BitRate" == reader.Name)
               {
                  bitRate = this.ReadInt(reader);
               }
               else if ("ConsumerHeartbeatRate" == reader.Name)
               {
                  consumerHeartbeatRate = this.ReadInt(reader);
               }
               else if ("ProducerHeartbeatRate" == reader.Name)
               {
                  producerHeartbeatRate = this.ReadInt(reader);
               }
               else if ("ControllerBusId" == reader.Name)
               {
                  controllerBusId = this.ReadInt(reader);
               }
               else if ("TargetBoardBusId" == reader.Name)
               {
                  targetBoardBusId = this.ReadInt(reader);
               }
               else if ("ControllerTraceMask" == reader.Name)
               {
                  controllerTraceMask = this.ReadInt(reader);
               }
               else if ("TargetBoardTraceMask" == reader.Name)
               {
                  targetBoardTraceMask = this.ReadInt(reader);
               }
            }
            else
            {
               if ("TargetBus" == reader.Name)
               {
                  result = new TargetBusParameters();

                  result.BusInterface = busInterface;
                  result.BitRate = bitRate;

                  result.ConsumerHeartbeatRate = consumerHeartbeatRate;
                  result.ProducerHeartbeatRate = producerHeartbeatRate;
                  result.ControllerBusId = controllerBusId;

                  result.TargetBoardBusId = targetBoardBusId;

                  result.ControllerTraceMask = controllerTraceMask;
                  result.TargetBoardTraceMask = targetBoardTraceMask;

                  break;
               }
            }
         }

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
               if ("Name" == reader.Name)
               {
                  name = this.ReadString(reader);
               }
               else if ("Address" == reader.Name)
               {
                  address = this.ReadString(reader);
               }
               else if ("Port" == reader.Name)
               {
                  port = this.ReadInt(reader);
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
               if ("Name" == reader.Name)
               {
                  name = this.ReadString(reader);
               }
               else if ("Unit" == reader.Name)
               {
                  unit = this.ReadString(reader);
               }
               else if ("Precision" == reader.Name)
               {
                  precision = this.ReadInt(reader);
               }
               else if ("MinimumValue" == reader.Name)
               {
                  minimumValue = this.ReadDouble(reader);
               }
               else if ("MaximumValue" == reader.Name)
               {
                  maximumValue = this.ReadDouble(reader);
               }
               else if ("StepValue" == reader.Name)
               {
                  stepValue = this.ReadDouble(reader);
               }
               else if ("DefaultValue" == reader.Name)
               {
                  defaultValue = this.ReadDouble(reader);
               }
               else if ("OperationalValue" == reader.Name)
               {
                  operationalValue = this.ReadDouble(reader);
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

      private WheelMotorParameters ReadWheelMotorParameters(XmlReader reader)
      {
         WheelMotorParameters temp = new WheelMotorParameters();
         WheelMotorParameters result = new WheelMotorParameters();
         bool readResult = true;

         for (; readResult; )
         {
            readResult = reader.Read();

            if (reader.IsStartElement())
            {
               if ("Location" == reader.Name)
               {
                  temp.Location = this.ReadString(reader);
               }
               else if ("MotorState" == reader.Name)
               {
                  temp.MotorState = this.ReadWheelMotorState(reader);
               }
               else if ("ActuationMode" == reader.Name)
               {
                  temp.ActuationMode = this.ReadActuationMode(reader);
               }
               else if ("RequestInverted" == reader.Name)
               {
                  temp.RequestInverted = this.ReadBool(reader);
               }
               else if ("PositionInverted" == reader.Name)
               {
                  temp.PositionInverted = this.ReadBool(reader);
               }
               else if ("ProfileVelocity" == reader.Name)
               {
                  temp.ProfileVelocity = this.ReadInt(reader);
               }
               else if ("ProfileAcceleration" == reader.Name)
               {
                  temp.ProfileAcceleration = this.ReadInt(reader);
               }
               else if ("ProfileDeceleration" == reader.Name)
               {
                  temp.ProfileDeceleration = this.ReadInt(reader);
               }
               else if ("Kp" == reader.Name)
               {
                  temp.Kp = this.ReadInt(reader);
               }
               else if ("Ki" == reader.Name)
               {
                  temp.Ki = this.ReadInt(reader);
               }
               else if ("Kd" == reader.Name)
               {
                  temp.Kd = this.ReadInt(reader);
               }
               else if ("Polarity" == reader.Name)
               {
                  temp.Polarity = this.ReadInt(reader);
               }
               else if ("PositionNotationIndex" == reader.Name)
               {
                  temp.PositionNotationIndex = this.ReadInt(reader);
               }
               else if ("VelocityNotationIndex" == reader.Name)
               {
                  temp.VelocityNotationIndex = this.ReadInt(reader);
               }
               else if ("VelocityDimensionIndex" == reader.Name)
               {
                  temp.VelocityDimensionIndex = this.ReadInt(reader);
               }
               else if ("AccelerationNotationIndex" == reader.Name)
               {
                  temp.AccelerationNotationIndex = this.ReadInt(reader);
               }
               else if ("AccelerationDimensionIndex" == reader.Name)
               {
                  temp.AccelerationDimensionIndex = this.ReadInt(reader);
               }
               else if ("PositionEncoderIncrements" == reader.Name)
               {
                  temp.PositionEncoderIncrements = this.ReadInt(reader);
               }
               else if ("PositionEncoderMotorRevolutions" == reader.Name)
               {
                  temp.PositionEncoderMotorRevolutions = this.ReadInt(reader);
               }
               else if ("VelocityEncoderIncrementsPerSecond" == reader.Name)
               {
                  temp.VelocityEncoderIncrementsPerSecond = this.ReadInt(reader);
               }
               else if ("VelocityEncoderMotorRevolutionsPerSecond" == reader.Name)
               {
                  temp.VelocityEncoderMotorRevolutionsPerSecond = this.ReadInt(reader);
               }
               else if ("GearRatioMotorRevolutions" == reader.Name)
               {
                  temp.GearRatioMotorRevolutions = this.ReadInt(reader);
               }
               else if ("GearRatioShaftRevolutions" == reader.Name)
               {
                  temp.GearRatioShaftRevolutions = this.ReadInt(reader);
               }
               else if ("FeedConstantFeed" == reader.Name)
               {
                  temp.FeedConstantFeed = this.ReadInt(reader);
               }
               else if ("FeedConstantShaftRevolutions" == reader.Name)
               {
                  temp.FeedConstantShaftRevolutions = this.ReadInt(reader);
               }
               else if ("MotorPeakCurrentLimit" == reader.Name)
               {
                  temp.MotorPeakCurrentLimit = this.ReadInt(reader);
               }
               else if ("MaximumCurrent" == reader.Name)
               {
                  temp.MaximumCurrent = this.ReadInt(reader);
               }
               else if ("MotorRatedCurrent" == reader.Name)
               {
                  temp.MotorRatedCurrent = this.ReadInt(reader);
               }
            }
            else
            {
               if ("WheelMotorParameters" == reader.Name)
               {
                  result = temp;
                  break;
               }
            }
         }

         return (result);
      }

      private CameraMaps ReadCameraMaps(XmlReader reader)
      {
         CameraMaps result = new CameraMaps();
         string name = "";
         List<CameraMap> maps = new List<CameraMap>();
         CameraMap map = null;
         bool readResult = true;

         for (; readResult; )
         {
            readResult = reader.Read();

            if (reader.IsStartElement())
            {
               if ("Name" == reader.Name)
               {
                  name = this.ReadString(reader);
               }
               else if ("Index" == reader.Name)
               {
                  if (null == map)
                  {
                     map = new CameraMap();
                  }

                  map.Index = this.ReadInt(reader);
               }
               else if ("SystemLocation" == reader.Name)
               {
                  if (null == map)
                  {
                     map = new CameraMap();
                  }

                  map.SystemLocation = this.ReadSystemLocation(reader);
               }
            }
            else
            {
               if ("Map" == reader.Name)
               {
                  if (null != map)
                  {
                     maps.Add(map);
                     map = null;
                  }
               }
               else if ("CameraMaps" == reader.Name)
               {
                  result = new CameraMaps(name, maps.ToArray());
                  break;
               }
            }
         }

         return (result);
      }

      private LightSelectParameters ReadLightSelectParameters(XmlReader reader)
      {
         LightSelectParameters camaraSelectParameters = new LightSelectParameters();
         bool readResult = true;

         for (; readResult; )
         {
            readResult = reader.Read();

            if (reader.IsStartElement())
            {
               if ("Location" == reader.Name)
               {
                  camaraSelectParameters.Location = this.ReadString(reader);
               }
               else if ("LightIntensity" == reader.Name)
               {
                  camaraSelectParameters.LightIntensity = this.ReadInt(reader);
               }
               else if ("LightChannelMask" == reader.Name)
               {
                  camaraSelectParameters.LightChannelMask = this.ReadInt(reader);
               }
            }
            else
            {
               if ("LightSelectParameters" == reader.Name)
               {
                  break;
               }
            }
         }

         return (camaraSelectParameters);
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
               if ("HorizontalOffset" == reader.Name)
               {
                  temp.HorizontalOffset = this.ReadInt(reader);
               }
               else if ("VerticalOffset" == reader.Name)
               {
                  temp.VerticalOffset = this.ReadInt(reader);
               }
               else if ("ShowDate" == reader.Name)
               {
                  temp.ShowDate = this.ReadBool(reader);
               }
               else if ("ShowTime" == reader.Name)
               {
                  temp.ShowTime = this.ReadBool(reader);
               }
               else if ("ShowDescription" == reader.Name)
               {
                  temp.ShowDescription = this.ReadBool(reader);
               }
               else if ("ShowCameraId" == reader.Name)
               {
                  temp.ShowCameraId = this.ReadBool(reader);
               }                  
               else if ("Line1" == reader.Name)
               {
                  temp.Line1 = this.ReadStringNoTrim(reader);
               }
               else if ("Line2" == reader.Name)
               {
                  temp.Line2 = this.ReadStringNoTrim(reader);
               }
               else if ("Line3" == reader.Name)
               {
                  temp.Line3 = this.ReadStringNoTrim(reader);
               }
               else if ("Line4" == reader.Name)
               {
                  temp.Line4 = this.ReadStringNoTrim(reader);
               }
               else if ("Line5" == reader.Name)
               {
                  temp.Line5 = this.ReadStringNoTrim(reader);
               }
               else if ("Line6" == reader.Name)
               {
                  temp.Line6 = this.ReadStringNoTrim(reader);
               }
               else if ("Line7" == reader.Name)
               {
                  temp.Line7 = this.ReadStringNoTrim(reader);
               }
               else if ("Line8" == reader.Name)
               {
                  temp.Line8 = this.ReadStringNoTrim(reader);
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

      private void ReadData(string filePath)
      {
         try
         {
            using (XmlReader reader = XmlReader.Create(filePath))
            {
               bool result = true;

               for (; result; )
               {
                  result = reader.Read();

                  if (reader.IsStartElement())
                  {
                     if ("VersionCount" == reader.Name)
                     {
                        this.VersionCount = this.ReadInt(reader);
                     }
                     else if ("LaserBus" == reader.Name)
                     {
                        LaserBusParameters mainBusParameters = this.ReadLaserBusParameters(reader);

                        if (null != mainBusParameters)
                        {
                           this.LaserBus = mainBusParameters;
                        }
                     }
                     else if ("TargetBus" == reader.Name)
                     {
                        TargetBusParameters targetBusParameters = this.ReadTargetBusParameters(reader);

                        if (null != targetBusParameters)
                        {
                           this.TargetBus = targetBusParameters;
                        }
                     }
                     else if ("IpEndpoint" == reader.Name)
                     {
                        IpEndpointParameters ipEndpointParameters = this.ReadIpEndpoint(reader);

                        if (null != ipEndpointParameters)
                        {
                           if ("Trace" == ipEndpointParameters.Name)
                           {
                              this.Trace = ipEndpointParameters;
                           }
                        }
                     }
                     else if ("RunTargetOnLaserBus" == reader.Name)
                     {
                        this.RunTargetOnLaserBus = this.ReadBool(reader);
                     }
                     else if ("SessionDataPath" == reader.Name)
                     {
                        this.SessionDataPath = this.ReadString(reader);
                     }
                     else if ("JoystickDeadband" == reader.Name)
                     {
                        this.JoystickDeadband = this.ReadInt(reader);
                     }
                     else if ("JoystickIdleBand" == reader.Name)
                     {
                        this.JoystickIdleBand = this.ReadInt(reader);
                     }
                     else if ("UsbRelayPort" == reader.Name)
                     {
                        this.UsbRelayPort = this.ReadInt(reader);
                     }
                     else if ("Latitude" == reader.Name)
                     {
                        this.Latitude = this.ReadDouble(reader);
                     }
                     else if ("Longitude" == reader.Name)
                     {
                        this.Longitude = this.ReadDouble(reader);
                     }
                     else if ("LaserWheelDistanceToTicks" == reader.Name)
                     {
                        this.LaserWheelDistanceToTicks = this.ReadDouble(reader);
                     }
                     else if ("LaserWheelVelocityToRpm" == reader.Name)
                     {
                        this.LaserWheelVelocityToRpm = this.ReadDouble(reader);
                     }
                     else if ("LaserWheelCountsToAmps" == reader.Name)
                     {
                        this.LaserWheelCountsToAmps = this.ReadDouble(reader);
                     }
                     else if ("LaserLinkVoltageMultipler" == reader.Name)
                     {
                        this.LaserLinkVoltageMultipler = this.ReadDouble(reader);
                     }
                     else if ("LaserLightPercentToCount" == reader.Name)
                     {
                        this.LaserLightPercentToCount = this.ReadDouble(reader);
                     }
                     else if ("LaserStepperPivotAngle" == reader.Name)
                     {
                        this.LaserStepperPivotAngle = this.ReadDouble(reader);
                     }
                     else if ("TargetWheelDistanceToTicks" == reader.Name)
                     {
                        this.TargetWheelDistanceToTicks = this.ReadDouble(reader);
                     }
                     else if ("TargetWheelVelocityToRpm" == reader.Name)
                     {
                        this.TargetWheelVelocityToRpm = this.ReadDouble(reader);
                     }
                     else if ("TargetWheelCountsToAmps" == reader.Name)
                     {
                        this.TargetWheelCountsToAmps = this.ReadDouble(reader);
                     }
                     else if ("TargetLinkVoltageMultipler" == reader.Name)
                     {
                        this.TargetLinkVoltageMultipler = this.ReadDouble(reader);
                     }
                     else if ("TargetLightPercentToCount" == reader.Name)
                     {
                        this.TargetLightPercentToCount = this.ReadDouble(reader);
                     }
                     else if ("Value" == reader.Name)
                     {
                        ValueParameter valueParameter = this.ReadValueParameters(reader);

                        if ("LaserWheelMaximumSpeed" == valueParameter.Name)
                        {
                           this.LaserWheelMaximumSpeed = valueParameter;
                        }
                        else if ("LaserWheelLowSpeedScale" == valueParameter.Name)
                        {
                           this.LaserWheelLowSpeedScale = valueParameter;
                        }
                        else if ("LaserWheelManualWheelDistance" == valueParameter.Name)
                        {
                           this.LaserWheelManualWheelDistance = valueParameter;
                        }
                        else if ("LaserWheelManualWheelSpeed" == valueParameter.Name)
                        {
                           this.LaserWheelManualWheelSpeed = valueParameter;
                        }
                        else if ("TargetWheelMaximumSpeed" == valueParameter.Name)
                        {
                           this.TargetWheelMaximumSpeed = valueParameter;
                        }
                        else if ("TargetWheelLowSpeedScale" == valueParameter.Name)
                        {
                           this.TargetWheelLowSpeedScale = valueParameter;
                        }
                        else if ("TargetWheelManualWheelDistance" == valueParameter.Name)
                        {
                           this.TargetWheelManualWheelDistance = valueParameter;
                        }
                        else if ("TargetWheelManualWheelSpeed" == valueParameter.Name)
                        {
                           this.TargetWheelManualWheelSpeed = valueParameter;
                        }
                        else if ("LaserMeasureAutoFrequency" == reader.Name)
                        {
                           this.LaserMeasureAutoFrequency = this.ReadBool(reader);
                        }
                        else if ("LaserMeasureManualFrequency" == valueParameter.Name)
                        {
                           this.LaserMeasureManualFrequency = valueParameter;
                        }
                        else if ("LaserSampleCount" == valueParameter.Name)
                        {
                           this.LaserSampleCount = valueParameter;
                        }
                        else if ("LaserMeasurementConstant" == valueParameter.Name)
                        {
                           this.LaserMeasurementConstant = valueParameter;
                        }
                     }
                     else if ("WheelMotorParameters" == reader.Name)
                     {
                        WheelMotorParameters wheelMotorParameters = this.ReadWheelMotorParameters(reader);

                        if (null != wheelMotorParameters)
                        {
                           if ("LaserFrontWheel" == wheelMotorParameters.Location)
                           {
                              this.LaserFrontWheel = wheelMotorParameters;
                           }
                           else if ("LaserRearWheel" == wheelMotorParameters.Location)
                           {
                              this.LaserRearWheel = wheelMotorParameters;
                           }
                           if ("TargetFrontWheel" == wheelMotorParameters.Location)
                           {
                              this.TargetFrontWheel = wheelMotorParameters;
                           }
                           else if ("TargetRearWheel" == wheelMotorParameters.Location)
                           {
                              this.TargetRearWheel = wheelMotorParameters;
                           }
                        }
                     }
                     else if ("CameraMaps" == reader.Name)
                     {
                        CameraMaps cameraMaps = this.ReadCameraMaps(reader);

                        if (null != cameraMaps)
                        {
                           if ("CrawlerHubCameraMapping" == cameraMaps.Name)
                           {
                              this.CrawlerHubCameraMaps = cameraMaps;
                           }
                           else if ("BulletCameraMaps" == cameraMaps.Name)
                           {
                              this.BulletCameraMaps = cameraMaps;
                           }                           
                        }
                     }
                     else if ("CrawlerHubSelectedCamera" == reader.Name)
                     {
                        this.CrawlerHubSelectedCamera = this.ReadSystemLocation(reader);
                     }
                     else if ("BulletSelectedCamera" == reader.Name)
                     {
                        this.BulletSelectedCamera = this.ReadSystemLocation(reader);
                     }
                     else if ("TargetTopCameraCwLimit" == reader.Name)
                     {
                        this.TargetTopCameraCwLimit = this.ReadDouble(reader);
                     }
                     else if ("TargetTopCameraCcwLimit" == reader.Name)
                     {
                        this.TargetTopCameraCcwLimit = this.ReadDouble(reader);
                     }
                     else if ("LightSelectParameters" == reader.Name)
                     {
                        LightSelectParameters camaraSelectParameters = this.ReadLightSelectParameters(reader);

                        if (null != camaraSelectParameters)
                        {
                           if ("CrawlerFrontLight" == camaraSelectParameters.Location)
                           {
                              this.CrawlerFrontLight = camaraSelectParameters;
                           }
                           else if ("CrawlerRearLight" == camaraSelectParameters.Location)
                           {
                              this.CrawlerRearLight = camaraSelectParameters;
                           }
                           else if ("CrawlerLeftLight" == camaraSelectParameters.Location)
                           {
                              this.CrawlerLeftLight = camaraSelectParameters;
                           }
                           else if ("CrawlerRightLight" == camaraSelectParameters.Location)
                           {
                              this.CrawlerRightLight = camaraSelectParameters;
                           }
                           else if ("BulletLeftLight" == camaraSelectParameters.Location)
                           {
                              this.BulletLeftLight = camaraSelectParameters;
                           }
                           else if ("BulletRightLight" == camaraSelectParameters.Location)
                           {
                              this.BulletRightLight = camaraSelectParameters;
                           }
                           else if ("BulletDownLight" == camaraSelectParameters.Location)
                           {
                              this.BulletDownLight = camaraSelectParameters;
                           }
                        }
                     }
                     else if ("Osd" == reader.Name)
                     {
                        OsdParameters osdParameters = this.ReadOsdParameters(reader);

                        if (null != osdParameters)
                        {
                           this.Osd = osdParameters;
                        }
                     }
                  }
               }

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

      private void WriteElement(XmlWriter writer, string tag, bool value)
      {
         string fileValue = (false != value) ? "1" : "0";
         writer.WriteElementString(tag, fileValue);
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

      private void WriteLaserBusParameters(XmlWriter writer, LaserBusParameters laserBusParameters)
      {
         writer.WriteStartElement("LaserBus");

         writer.WriteComment("BusInterface from {USBA, USBB, PCIA, PCIB}");
         this.WriteElement(writer, "BusInterface", laserBusParameters.BusInterface.ToString());
         this.WriteElement(writer, "BitRate", laserBusParameters.BitRate);

         writer.WriteComment("set heartbeat rates to 0 to disable, restore consumer to 3500 and producer to 1000 (3:1)");
         this.WriteElement(writer, "ConsumerHeartbeatRate", laserBusParameters.ConsumerHeartbeatRate);
         this.WriteElement(writer, "ProducerHeartbeatRate", laserBusParameters.ProducerHeartbeatRate);
         this.WriteElement(writer, "ControllerBusId", laserBusParameters.ControllerBusId);

         this.WriteElement(writer, "LaserBoardBusId", laserBusParameters.LaserBoardBusId);
         this.WriteElement(writer, "GpsBusId", laserBusParameters.GpsBusId);

         this.WriteElement(writer, "ControllerTraceMask", laserBusParameters.ControllerTraceMask);
         this.WriteElement(writer, "MainBoardTraceMask", laserBusParameters.MainBoardTraceMask);
         this.WriteElement(writer, "GpsTraceMask", laserBusParameters.GpsTraceMask);         

         writer.WriteEndElement();
      }

      private void WriteTargetBusParameters(XmlWriter writer, TargetBusParameters targetBusParameters)
      {
         writer.WriteStartElement("TargetBus");

         writer.WriteComment("BusInterface from {USBA, USBB, PCIA, PCIB}");
         this.WriteElement(writer, "BusInterface", targetBusParameters.BusInterface.ToString());
         this.WriteElement(writer, "BitRate", targetBusParameters.BitRate);

         writer.WriteComment("set heartbeat rates to 0 to disable, restore consumer to 3500 and producer to 1000 (3:1)");
         this.WriteElement(writer, "ConsumerHeartbeatRate", targetBusParameters.ConsumerHeartbeatRate);
         this.WriteElement(writer, "ProducerHeartbeatRate", targetBusParameters.ProducerHeartbeatRate);
         this.WriteElement(writer, "ControllerBusId", targetBusParameters.ControllerBusId);

         this.WriteElement(writer, "TargetBoardBusId", targetBusParameters.TargetBoardBusId);

         this.WriteElement(writer, "ControllerTraceMask", targetBusParameters.ControllerTraceMask);
         this.WriteElement(writer, "TargetBoardTraceMask", targetBusParameters.TargetBoardTraceMask);

         writer.WriteEndElement();
      }

      private void WriteIpEndpointParameters(XmlWriter writer, IpEndpointParameters ipEndpointParameters)
      {
         writer.WriteStartElement("IpEndpoint");

         this.WriteElement(writer, "Name", ipEndpointParameters.Name);
         this.WriteElement(writer, "Address", ipEndpointParameters.Address);
         writer.WriteComment("set 0 to disable function");
         this.WriteElement(writer, "Port", ipEndpointParameters.Port);

         writer.WriteEndElement();
      }

      private void WriteWheelMotorParameters(XmlWriter writer, WheelMotorParameters wheelMotorParameters)
      {
         writer.WriteStartElement("WheelMotorParameters");

         this.WriteElement(writer, "Location", wheelMotorParameters.Location);

         this.WriteElement(writer, "MotorState", wheelMotorParameters.MotorState.ToString());
         this.WriteElement(writer, "ActuationMode", wheelMotorParameters.ActuationMode.ToString());
         this.WriteElement(writer, "RequestInverted", wheelMotorParameters.RequestInverted);
         this.WriteElement(writer, "PositionInverted", wheelMotorParameters.PositionInverted);

         this.WriteElement(writer, "ProfileVelocity", wheelMotorParameters.ProfileVelocity);
         this.WriteElement(writer, "ProfileAcceleration", wheelMotorParameters.ProfileAcceleration);
         this.WriteElement(writer, "ProfileDeceleration", wheelMotorParameters.ProfileDeceleration);

         this.WriteElement(writer, "Kp", wheelMotorParameters.Kp);
         this.WriteElement(writer, "Ki", wheelMotorParameters.Ki);
         this.WriteElement(writer, "Kd", wheelMotorParameters.Kd);

         this.WriteElement(writer, "Polarity", wheelMotorParameters.Polarity);
         this.WriteElement(writer, "PositionNotationIndex", wheelMotorParameters.PositionNotationIndex);
         this.WriteElement(writer, "VelocityNotationIndex", wheelMotorParameters.VelocityNotationIndex);
         this.WriteElement(writer, "VelocityDimensionIndex", wheelMotorParameters.VelocityDimensionIndex);
         this.WriteElement(writer, "AccelerationNotationIndex", wheelMotorParameters.AccelerationNotationIndex);
         this.WriteElement(writer, "AccelerationDimensionIndex", wheelMotorParameters.AccelerationDimensionIndex);
         this.WriteElement(writer, "PositionEncoderIncrements", wheelMotorParameters.PositionEncoderIncrements);
         this.WriteElement(writer, "PositionEncoderMotorRevolutions", wheelMotorParameters.PositionEncoderMotorRevolutions);
         this.WriteElement(writer, "VelocityEncoderIncrementsPerSecond", wheelMotorParameters.VelocityEncoderIncrementsPerSecond);
         this.WriteElement(writer, "VelocityEncoderMotorRevolutionsPerSecond", wheelMotorParameters.VelocityEncoderMotorRevolutionsPerSecond);
         this.WriteElement(writer, "GearRatioMotorRevolutions", wheelMotorParameters.GearRatioMotorRevolutions);
         this.WriteElement(writer, "GearRatioShaftRevolutions", wheelMotorParameters.GearRatioShaftRevolutions);
         this.WriteElement(writer, "FeedConstantFeed", wheelMotorParameters.FeedConstantFeed);
         this.WriteElement(writer, "FeedConstantShaftRevolutions", wheelMotorParameters.FeedConstantShaftRevolutions);
         this.WriteElement(writer, "MotorPeakCurrentLimit", wheelMotorParameters.MotorPeakCurrentLimit);
         this.WriteElement(writer, "MaximumCurrent", wheelMotorParameters.MaximumCurrent);
         this.WriteElement(writer, "MotorRatedCurrent", wheelMotorParameters.MotorRatedCurrent);

         writer.WriteEndElement();
      }

      private void WriteCameraMaps(XmlWriter writer, CameraMaps cameraMaps)
      {
         writer.WriteStartElement("CameraMaps");

         this.WriteElement(writer, "Name", cameraMaps.Name);

         writer.WriteStartElement("Maps");

         if (null != cameraMaps.Maps)
         {
            for (int i = 0; i < cameraMaps.Maps.Length; i++)
            {
               writer.WriteStartElement("Map");
               this.WriteElement(writer, "Index", cameraMaps.Maps[i].Index.ToString());
               this.WriteElement(writer, "SystemLocation", cameraMaps.Maps[i].SystemLocation.ToString());
               writer.WriteEndElement();
            }
         }

         writer.WriteEndElement();
         writer.WriteEndElement();
      }

      private void WriteLightSelectParameters(XmlWriter writer, LightSelectParameters lightSelectParameters)
      {
         writer.WriteStartElement("LightSelectParameters");

         this.WriteElement(writer, "Location", lightSelectParameters.Location);
         this.WriteElement(writer, "LightIntensity", lightSelectParameters.LightIntensity);
         this.WriteElement(writer, "LightChannelMask", lightSelectParameters.LightChannelMask);

         writer.WriteEndElement();
      }

      private void WriteOsdParameters(XmlWriter writer, OsdParameters osdParameters)
      {
         writer.WriteStartElement("Osd");

         this.WriteElement(writer, "HorizontalOffset", osdParameters.HorizontalOffset);
         this.WriteElement(writer, "VerticalOffset", osdParameters.VerticalOffset);

         this.WriteElement(writer, "ShowDate", osdParameters.ShowDate);
         this.WriteElement(writer, "ShowTime", osdParameters.ShowTime);
         this.WriteElement(writer, "ShowDescription", osdParameters.ShowDescription);
         this.WriteElement(writer, "ShowCameraId", osdParameters.ShowCameraId);

         this.WriteElement(writer, "Line1", osdParameters.Line1);
         this.WriteElement(writer, "Line2", osdParameters.Line2);
         this.WriteElement(writer, "Line3", osdParameters.Line3);
         this.WriteElement(writer, "Line4", osdParameters.Line4);
         this.WriteElement(writer, "Line5", osdParameters.Line5);
         this.WriteElement(writer, "Line6", osdParameters.Line6);
         this.WriteElement(writer, "Line7", osdParameters.Line7);
         this.WriteElement(writer, "Line8", osdParameters.Line8);

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

            this.WriteElement(writer, "VersionCount", this.VersionCount.ToString());

            this.WriteLaserBusParameters(writer, this.LaserBus);
            this.WriteTargetBusParameters(writer, this.TargetBus);
            this.WriteElement(writer, "RunTargetOnLaserBus", this.RunTargetOnLaserBus);

            this.WriteIpEndpointParameters(writer, this.Trace);

            this.WriteElement(writer, "SessionDataPath", this.SessionDataPath);
            this.WriteElement(writer, "JoystickDeadband", this.JoystickDeadband);
            this.WriteElement(writer, "JoystickIdleBand", this.JoystickIdleBand);
            this.WriteElement(writer, "UsbRelayPort", this.UsbRelayPort);
            this.WriteElement(writer, "Latitude", this.Latitude);
            this.WriteElement(writer, "Longitude", this.Longitude);

            this.WriteWheelMotorParameters(writer, this.LaserFrontWheel);
            this.WriteWheelMotorParameters(writer, this.LaserRearWheel);
            this.WriteValueParameters(writer, this.LaserWheelMaximumSpeed);
            this.WriteValueParameters(writer, this.LaserWheelLowSpeedScale);
            this.WriteValueParameters(writer, this.LaserWheelManualWheelDistance);
            this.WriteValueParameters(writer, this.LaserWheelManualWheelSpeed);
            this.WriteElement(writer, "LaserWheelDistanceToTicks", this.LaserWheelDistanceToTicks);
            this.WriteElement(writer, "LaserWheelVelocityToRpm", this.LaserWheelVelocityToRpm);
            this.WriteElement(writer, "LaserWheelCountsToAmps", this.LaserWheelCountsToAmps);            
            this.WriteElement(writer, "LaserLinkVoltageMultipler", this.LaserLinkVoltageMultipler);
            this.WriteElement(writer, "LaserLightPercentToCount", this.LaserLightPercentToCount);
            this.WriteElement(writer, "LaserStepperPivotAngle", this.LaserStepperPivotAngle);
            this.WriteCameraMaps(writer, this.CrawlerHubCameraMaps);
            this.WriteLightSelectParameters(writer, this.CrawlerFrontLight);
            this.WriteLightSelectParameters(writer, this.CrawlerRearLight);
            this.WriteLightSelectParameters(writer, this.CrawlerLeftLight);
            this.WriteLightSelectParameters(writer, this.CrawlerRightLight);
            this.WriteElement(writer, "CrawlerHubSelectedCamera", this.CrawlerHubSelectedCamera.ToString());

            this.WriteWheelMotorParameters(writer, this.TargetFrontWheel);
            this.WriteWheelMotorParameters(writer, this.TargetRearWheel);
            this.WriteValueParameters(writer, this.TargetWheelMaximumSpeed);
            this.WriteValueParameters(writer, this.TargetWheelLowSpeedScale);
            this.WriteValueParameters(writer, this.TargetWheelManualWheelDistance);
            this.WriteValueParameters(writer, this.TargetWheelManualWheelSpeed);
            this.WriteElement(writer, "TargetWheelDistanceToTicks", this.TargetWheelDistanceToTicks);
            this.WriteElement(writer, "TargetWheelVelocityToRpm", this.TargetWheelVelocityToRpm);
            this.WriteElement(writer, "TargetWheelCountsToAmps", this.TargetWheelCountsToAmps);            
            this.WriteElement(writer, "TargetLinkVoltageMultipler", this.TargetLinkVoltageMultipler);
            this.WriteElement(writer, "TargetLightPercentToCount", this.TargetLightPercentToCount);

            this.WriteCameraMaps(writer, this.BulletCameraMaps);
            this.WriteLightSelectParameters(writer, this.BulletLeftLight);
            this.WriteLightSelectParameters(writer, this.BulletRightLight);
            this.WriteLightSelectParameters(writer, this.BulletDownLight);
            this.WriteElement(writer, "BulletSelectedCamera", this.BulletSelectedCamera.ToString());
            this.WriteElement(writer, "TargetTopCameraCwLimit", this.TargetTopCameraCwLimit);
            this.WriteElement(writer, "TargetTopCameraCcwLimit", this.TargetTopCameraCcwLimit);


            this.WriteElement(writer, "LaserMeasureAutoFrequency", this.LaserMeasureAutoFrequency);
            this.WriteValueParameters(writer, this.LaserMeasureManualFrequency);
            this.WriteValueParameters(writer, this.LaserSampleCount);
            this.WriteValueParameters(writer, this.LaserMeasurementConstant);

            this.WriteOsdParameters(writer, this.Osd);

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
         this.filePath = filePath;
         string defaultFile = this.GetDefaultFilePath();
         this.AssignDefaults();
         int parameterVersionCount = this.VersionCount;

         if (File.Exists(defaultFile) == false)
         {
            this.WriteData(defaultFile);
         }
         else
         {
            this.ReadData(defaultFile);

            if (parameterVersionCount != this.VersionCount)
            {
               this.VersionCount = parameterVersionCount;
               this.WriteData(defaultFile);
            }
         }

         if (false != this.setDefaults)
         {
            this.ReadData(defaultFile);
            Tracer.WriteHigh(TraceGroup.PARAM, null, "Defaults retrieved.");
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

         this.VersionCount = parameterVersionCount;
      }

      public void Write(string filePath)
      {
         this.WriteData(filePath + ".ini");
      }

      public void TriggerDefaults()
      {
         this.setDefaults = true;
      }

      public void SaveDefaults()
      {
         string defaultFile = this.GetDefaultFilePath();
         this.WriteData(defaultFile);
      }

      public LightSelectParameters GetLightSelectParameters(Controls.SystemLocations systemLocation)
      {
         LightSelectParameters result = null;

         if (Controls.SystemLocations.crawlerFront == systemLocation)
         {
            result = this.CrawlerFrontLight;
         }
         else if (Controls.SystemLocations.crawlerRear == systemLocation)
         {
            result = this.CrawlerRearLight;
         }
         else if (Controls.SystemLocations.crawlerLeft == systemLocation)
         {
            result = this.CrawlerLeftLight;
         }
         else if (Controls.SystemLocations.crawlerRight == systemLocation)
         {
            result = this.CrawlerRightLight;
         }
         else if (Controls.SystemLocations.bulletLeft == systemLocation)
         {
            result = this.BulletLeftLight;
         }
         else if (Controls.SystemLocations.bulletRight == systemLocation)
         {
            result = this.BulletRightLight;
         }
         else if (Controls.SystemLocations.bulletDown == systemLocation)
         {
            result = this.BulletDownLight;
         }

         return (result);
      }

      #endregion
   }
}