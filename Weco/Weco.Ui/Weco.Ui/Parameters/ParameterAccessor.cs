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

      public RobotBusParameters RobotBus;
      public TruckBusParameters TruckBus;

      public IpEndpointParameters Trace;

      public int JoystickId;
      public int JoystickDeadband;
      public int JoystickIdleBand;
      public int UsbRelayPort;

      public WheelMotorParameters LaserFrontWheel;
      public WheelMotorParameters LaserRearWheel;
      public ValueParameter LaserWheelMaximumSpeed;
      public ValueParameter LaserWheelLowSpeedScale;
      public ValueParameter LaserWheelManualWheelDistance;
      public ValueParameter LaserWheelManualWheelSpeed;
      public double LaserWheelDistanceToTicks;
      public double LaserWheelVelocityToRpm;
      public double LaserWheelCountsToAmps;
      
      public CameraMaps CrawlerHubCameraMaps;
      public LightSelectParameters CrawlerFrontLight;
      public LightSelectParameters CrawlerRearLight;
      public LightSelectParameters CrawlerLeftLight;
      public LightSelectParameters CrawlerRightLight;
      public Controls.SystemLocations CrawlerHubSelectedCamera;
      
      public CameraMaps BulletCameraMaps;
      public LightSelectParameters BulletLeftLight;
      public LightSelectParameters BulletRightLight;
      public LightSelectParameters BulletDownLight;
      public Controls.SystemLocations BulletSelectedCamera;

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

         this.RobotBus = new RobotBusParameters();
         this.RobotBus.BusInterface = BusInterfaces.PCIA;
         this.RobotBus.BitRate = 50000;
         this.RobotBus.ConsumerHeartbeatRate = 3500;
         this.RobotBus.ProducerHeartbeatRate = 1000;
         this.RobotBus.ControllerBusId = 208;
         this.RobotBus.LeftTrackBusId = 54;
         this.RobotBus.HubBusId = 50;
         this.RobotBus.RightTrackBusId = 56;
         this.RobotBus.ControllerTraceMask = 0;
         this.RobotBus.LeftTrackTraceMask = 1;
         this.RobotBus.HubTraceMask = 1;
         this.RobotBus.RightTrackTraceMask = 1;

         this.TruckBus = new TruckBusParameters();
         this.TruckBus.BusInterface = BusInterfaces.PCIB;
         this.TruckBus.BitRate = 50000;
         this.TruckBus.ConsumerHeartbeatRate = 3500;
         this.TruckBus.ProducerHeartbeatRate = 1000;
         this.TruckBus.ControllerBusId = 255;
         this.TruckBus.LaunchCardBusId = 64;
         this.TruckBus.BulletMotorBusId = 28;
         this.TruckBus.LeftFeederMotorBusId = 32;
         this.TruckBus.RightFeederMotorBusId = 34;
         this.TruckBus.ReelMotorBusId = 40;
         this.TruckBus.ReelEncoderBusId = 42;
         this.TruckBus.ControllerTraceMask = 0;
         this.TruckBus.LaunchCardTraceMask = 1;
         this.TruckBus.BulletMotorTraceMask = 1;
         this.TruckBus.LeftFeederMotorTraceMask = 1;
         this.TruckBus.RightFeederMotorTraceMask = 1;
         this.TruckBus.ReelMotorTraceMask = 1;
         this.TruckBus.ReelEncoderTraceMask = 1;

         this.Trace = new IpEndpointParameters("Trace", "127.0.0.1", 10000);

         this.JoystickId = 0;
         this.JoystickDeadband = 5000;
         this.JoystickIdleBand = 4000;
         this.UsbRelayPort = 1;


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


         CameraMap[] crawlerHubCameraMaps = new CameraMap[2];
         crawlerHubCameraMaps[0] = new CameraMap(1, Ui.Controls.SystemLocations.crawlerFront);
         crawlerHubCameraMaps[1] = new CameraMap(2, Ui.Controls.SystemLocations.crawlerRear);
         this.CrawlerHubCameraMaps = new CameraMaps("CrawlerHubCameraMaps", crawlerHubCameraMaps);

         this.CrawlerFrontLight = new LightSelectParameters("CrawlerFrontLight", 15, 1);
         this.CrawlerRearLight = new LightSelectParameters("CrawlerRearLight", 15, 2);
         this.CrawlerLeftLight = new LightSelectParameters("CrawlerLeftLight", 15, 1);
         this.CrawlerRightLight = new LightSelectParameters("CrawlerRightLight", 15, 1);
         this.CrawlerHubSelectedCamera = Controls.SystemLocations.crawlerFront;


         CameraMap[] bulletCameraMaps = new CameraMap[3];
         bulletCameraMaps[0] = new CameraMap(1, Ui.Controls.SystemLocations.bulletLeft);
         bulletCameraMaps[1] = new CameraMap(2, Ui.Controls.SystemLocations.bulletRight);
         bulletCameraMaps[2] = new CameraMap(3, Ui.Controls.SystemLocations.bulletDown);
         this.BulletCameraMaps = new CameraMaps("BulletCameraMaps", bulletCameraMaps);

         this.BulletLeftLight = new LightSelectParameters("BulletLeftLight", 15, 1);
         this.BulletRightLight = new LightSelectParameters("BulletRightLight", 15, 2);
         this.BulletDownLight = new LightSelectParameters("BulletDownLight", 15, 4);
         this.BulletSelectedCamera = Controls.SystemLocations.bulletDown;
         

         this.Osd = new OsdParameters();
         this.SetOsdDefaults(ref this.Osd);
      }

      private void SetOsdDefaults(ref OsdParameters osdParameters)
      {
         osdParameters.HorizontalOffset = 35;
         osdParameters.VerticalOffset = 21;

         osdParameters.ShowDate = false;
         osdParameters.ShowDistance = false;
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

      private RobotBusParameters ReadRobotBusParameters(XmlReader reader)
      {
         RobotBusParameters result = null;
         bool readResult = true;

         BusInterfaces busInterface = BusInterfaces.USBA;
         int bitRate = 0;
         int consumerHeartbeatRate = 0;
         int producerHeartbeatRate = 0;
         int controllerBusId = 0;
         int leftTrackBusId = 0;
         int hubBusId = 0;
         int rightTrackBusId = 0;
         int controllerTraceMask = 0;
         int leftTrackTraceMask = 0;
         int hubTraceMask = 0;
         int rightTrackTraceMask = 0;

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
               else if ("LeftTrackBusId" == reader.Name)
               {
                  leftTrackBusId = this.ReadInt(reader);
               }
               else if ("HubBusId" == reader.Name)
               {
                  hubBusId = this.ReadInt(reader);
               }
               else if ("RightTrackBusId" == reader.Name)
               {
                  rightTrackBusId = this.ReadInt(reader);
               }                  
               else if ("ControllerTraceMask" == reader.Name)
               {
                  controllerTraceMask = this.ReadInt(reader);
               }
               else if ("LeftTrackTraceMask" == reader.Name)
               {
                  leftTrackTraceMask = this.ReadInt(reader);
               }
               else if ("HubTraceMask" == reader.Name)
               {
                  hubTraceMask = this.ReadInt(reader);
               }
               else if ("RightTrackTraceMask" == reader.Name)
               {
                  rightTrackTraceMask = this.ReadInt(reader);
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
                  result.LeftTrackBusId = leftTrackBusId;
                  result.HubBusId = hubBusId;
                  result.RightTrackBusId = rightTrackBusId;
                  result.ControllerTraceMask = controllerTraceMask;
                  result.LeftTrackTraceMask = leftTrackTraceMask;
                  result.HubTraceMask = hubTraceMask;
                  result.RightTrackTraceMask = rightTrackTraceMask;

                  break;
               }
            }
         }

         return (result);
      }

      private TruckBusParameters ReadTruckBusParameters(XmlReader reader)
      {
         TruckBusParameters result = null;
         bool readResult = true;

         BusInterfaces busInterface = BusInterfaces.USBA;
         int bitRate = 0;
         int consumerHeartbeatRate = 0;
         int producerHeartbeatRate = 0;
         int controllerBusId = 0;
         int launchCardBusId = 0;
         int bulletMotorBusId = 0;
         int leftFeederMotorBusId = 0;
         int rightFeederMotorBusId = 0;
         int reelMotorBusId = 0;
         int reelEncoderBusId = 0;
         int controllerTraceMask = 0;
         int launchCardTraceMask = 0;
         int bulletMotorTraceMask = 0;
         int leftFeederMotorTraceMask = 0;
         int rightFeederMotorTraceMask = 0;
         int reelMotorTraceMask = 0;
         int reelEncoderTraceMask = 0;

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
               else if ("LaunchCardBusId" == reader.Name)
               {
                  launchCardBusId = this.ReadInt(reader);
               }
               else if ("BulletMotorBusId" == reader.Name)
               {
                  bulletMotorBusId = this.ReadInt(reader);
               }
               else if ("LeftFeederMotorBusId" == reader.Name)
               {
                  leftFeederMotorBusId = this.ReadInt(reader);
               }
               else if ("RightFeederMotorBusId" == reader.Name)
               {
                  rightFeederMotorBusId = this.ReadInt(reader);
               }
               else if ("ReelMotorBusId" == reader.Name)
               {
                  reelMotorBusId = this.ReadInt(reader);
               }
               else if ("ReelMotorBusId" == reader.Name)
               {
                  reelMotorBusId = this.ReadInt(reader);
               }
               else if ("ControllerTraceMask" == reader.Name)
               {
                  controllerTraceMask = this.ReadInt(reader);
               }
               else if ("LaunchCardTraceMask" == reader.Name)
               {
                  launchCardTraceMask = this.ReadInt(reader);
               }
               else if ("BulletMotorTraceMask" == reader.Name)
               {
                  bulletMotorTraceMask = this.ReadInt(reader);
               }
               else if ("LeftFeederMotorTraceMask" == reader.Name)
               {
                  leftFeederMotorTraceMask = this.ReadInt(reader);
               }
               else if ("RightFeederMotorTraceMask" == reader.Name)
               {
                  rightFeederMotorTraceMask = this.ReadInt(reader);
               }
               else if ("ReelMotorTraceMask" == reader.Name)
               {
                  reelMotorTraceMask = this.ReadInt(reader);
               }
               else if ("ReelEncoderTraceMask" == reader.Name)
               {
                  reelEncoderTraceMask = this.ReadInt(reader);
               }
            }
            else
            {
               if ("TargetBus" == reader.Name)
               {
                  result = new TruckBusParameters();

                  result.BusInterface = busInterface;
                  result.BitRate = bitRate;

                  result.ConsumerHeartbeatRate = consumerHeartbeatRate;
                  result.ProducerHeartbeatRate = producerHeartbeatRate;
                  result.ControllerBusId = controllerBusId;
                  result.LaunchCardBusId = launchCardBusId;
                  result.BulletMotorBusId = bulletMotorBusId;
                  result.LeftFeederMotorBusId = leftFeederMotorBusId;
                  result.RightFeederMotorBusId = rightFeederMotorBusId;
                  result.ReelMotorBusId = reelMotorBusId;
                  result.ReelEncoderBusId = reelEncoderBusId;
                  result.ControllerTraceMask = controllerTraceMask;
                  result.LaunchCardTraceMask = launchCardTraceMask;
                  result.BulletMotorTraceMask = bulletMotorTraceMask;
                  result.LeftFeederMotorTraceMask = leftFeederMotorTraceMask;
                  result.RightFeederMotorTraceMask = rightFeederMotorTraceMask;
                  result.ReelMotorTraceMask = reelMotorTraceMask;
                  result.ReelEncoderTraceMask = reelEncoderTraceMask;

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
               else if ("ShowDistance" == reader.Name)
               {
                  temp.ShowDistance = this.ReadBool(reader);
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
                     else if ("RobotBus" == reader.Name)
                     {
                        RobotBusParameters robotBusParameters = this.ReadRobotBusParameters(reader);

                        if (null != robotBusParameters)
                        {
                           this.RobotBus = robotBusParameters;
                        }
                     }
                     else if ("TargetBus" == reader.Name)
                     {
                        TruckBusParameters truckBusParameters = this.ReadTruckBusParameters(reader);

                        if (null != truckBusParameters)
                        {
                           this.TruckBus = truckBusParameters;
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
                     else if ("JoystickId" == reader.Name)
                     {
                        this.JoystickId = this.ReadInt(reader);
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

      private void WriteRobotBusParameters(XmlWriter writer, RobotBusParameters robotBusParameters)
      {
         writer.WriteStartElement("RobotBus");

         writer.WriteComment("BusInterface from {USBA, USBB, PCIA, PCIB}");
         this.WriteElement(writer, "BusInterface", robotBusParameters.BusInterface.ToString());
         this.WriteElement(writer, "BitRate", robotBusParameters.BitRate);
         writer.WriteComment("set heartbeat rates to 0 to disable, restore consumer to 3500 and producer to 1000 (3:1)");
         this.WriteElement(writer, "ConsumerHeartbeatRate", robotBusParameters.ConsumerHeartbeatRate);
         this.WriteElement(writer, "ProducerHeartbeatRate", robotBusParameters.ProducerHeartbeatRate);
         this.WriteElement(writer, "ControllerBusId", robotBusParameters.ControllerBusId);
         this.WriteElement(writer, "LeftTrackBusId", robotBusParameters.LeftTrackBusId);
         this.WriteElement(writer, "HubBusId", robotBusParameters.HubBusId);
         this.WriteElement(writer, "GpsBusId", robotBusParameters.RightTrackBusId);
         this.WriteElement(writer, "ControllerTraceMask", robotBusParameters.ControllerTraceMask);
         this.WriteElement(writer, "LeftTrackTraceMask", robotBusParameters.LeftTrackTraceMask);
         this.WriteElement(writer, "HubTraceMask", robotBusParameters.HubTraceMask);
         this.WriteElement(writer, "RightTrackTraceMask", robotBusParameters.RightTrackTraceMask);

         writer.WriteEndElement();
      }

      private void WriteTruckBusParameters(XmlWriter writer, TruckBusParameters truckBusParameters)
      {
         writer.WriteStartElement("TruckBus");

         writer.WriteComment("BusInterface from {USBA, USBB, PCIA, PCIB}");
         this.WriteElement(writer, "BusInterface", truckBusParameters.BusInterface.ToString());
         this.WriteElement(writer, "BitRate", truckBusParameters.BitRate);

         writer.WriteComment("set heartbeat rates to 0 to disable, restore consumer to 3500 and producer to 1000 (3:1)");
         this.WriteElement(writer, "ConsumerHeartbeatRate", truckBusParameters.ConsumerHeartbeatRate);
         this.WriteElement(writer, "ProducerHeartbeatRate", truckBusParameters.ProducerHeartbeatRate);
         this.WriteElement(writer, "ControllerBusId", truckBusParameters.ControllerBusId);
         this.WriteElement(writer, "LaunchCardBusId", truckBusParameters.LaunchCardBusId);
         this.WriteElement(writer, "BulletMotorBusId", truckBusParameters.BulletMotorBusId);
         this.WriteElement(writer, "LeftFeederMotorBusId", truckBusParameters.LeftFeederMotorBusId);
         this.WriteElement(writer, "RightFeederMotorBusId", truckBusParameters.RightFeederMotorBusId);
         this.WriteElement(writer, "ReelMotorBusId", truckBusParameters.ReelMotorBusId);
         this.WriteElement(writer, "ReelEncoderBusId", truckBusParameters.ReelEncoderBusId);
         this.WriteElement(writer, "ControllerTraceMask", truckBusParameters.ControllerTraceMask);
         this.WriteElement(writer, "LaunchCardTraceMask", truckBusParameters.LaunchCardTraceMask);
         this.WriteElement(writer, "BulletMotorTraceMask", truckBusParameters.BulletMotorTraceMask);
         this.WriteElement(writer, "LeftFeederMotorTraceMask", truckBusParameters.LeftFeederMotorTraceMask);
         this.WriteElement(writer, "RightFeederMotorTraceMask", truckBusParameters.RightFeederMotorTraceMask);
         this.WriteElement(writer, "ReelEncoderTraceMask", truckBusParameters.ReelEncoderTraceMask);
         this.WriteElement(writer, "ReelEncoderTraceMask", truckBusParameters.ReelEncoderTraceMask);

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
         this.WriteElement(writer, "ShowDistance", osdParameters.ShowDistance);         
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

            this.WriteRobotBusParameters(writer, this.RobotBus);
            this.WriteTruckBusParameters(writer, this.TruckBus);

            this.WriteIpEndpointParameters(writer, this.Trace);

            this.WriteElement(writer, "JoystickId", this.JoystickId);
            this.WriteElement(writer, "JoystickDeadband", this.JoystickDeadband);
            this.WriteElement(writer, "JoystickIdleBand", this.JoystickIdleBand);
            this.WriteElement(writer, "UsbRelayPort", this.UsbRelayPort);

            this.WriteWheelMotorParameters(writer, this.LaserFrontWheel);
            this.WriteWheelMotorParameters(writer, this.LaserRearWheel);
            this.WriteValueParameters(writer, this.LaserWheelMaximumSpeed);
            this.WriteValueParameters(writer, this.LaserWheelLowSpeedScale);
            this.WriteValueParameters(writer, this.LaserWheelManualWheelDistance);
            this.WriteValueParameters(writer, this.LaserWheelManualWheelSpeed);
            this.WriteElement(writer, "LaserWheelDistanceToTicks", this.LaserWheelDistanceToTicks);
            this.WriteElement(writer, "LaserWheelVelocityToRpm", this.LaserWheelVelocityToRpm);
            this.WriteElement(writer, "LaserWheelCountsToAmps", this.LaserWheelCountsToAmps);        
    
            this.WriteCameraMaps(writer, this.CrawlerHubCameraMaps);
            this.WriteLightSelectParameters(writer, this.CrawlerFrontLight);
            this.WriteLightSelectParameters(writer, this.CrawlerRearLight);
            this.WriteLightSelectParameters(writer, this.CrawlerLeftLight);
            this.WriteLightSelectParameters(writer, this.CrawlerRightLight);
            this.WriteElement(writer, "CrawlerHubSelectedCamera", this.CrawlerHubSelectedCamera.ToString());

            this.WriteCameraMaps(writer, this.BulletCameraMaps);
            this.WriteLightSelectParameters(writer, this.BulletLeftLight);
            this.WriteLightSelectParameters(writer, this.BulletRightLight);
            this.WriteLightSelectParameters(writer, this.BulletDownLight);
            this.WriteElement(writer, "BulletSelectedCamera", this.BulletSelectedCamera.ToString());

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