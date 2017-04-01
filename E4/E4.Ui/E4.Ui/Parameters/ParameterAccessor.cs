namespace E4.Ui
{
   using System;
   using System.IO;
   using System.Xml;

   using E4.PCANLight;
   using E4.Utilities;

   public class ParameterAccessor
   {
      #region Fields

      private static ParameterAccessor instance = null;

      private string filePath;
      private bool setDefaults;

      private int VersionCount;

      public LaserBusParameters LaserBus;
      public TargetBusParameters TargetBus;

      public IpEndpointParameters Trace;

      public int JoystickDeadband;
      public int JoystickIdleBand;

      public WheelMotorParameters LaserFrontWheel;
      public WheelMotorParameters LaserRearWheel;
      public StepperMotorParameters LaserXStepper;
      public StepperMotorParameters LaserYStepper;
      public ValueParameter LaserWheelMaximumSpeed;
      public ValueParameter LaserWheelLowSpeedScale;
      public double LaserWheelVelocityToRpm;

      public WheelMotorParameters TargetFrontWheel;
      public WheelMotorParameters TargetRearWheel;
      public StepperMotorParameters TargetStepper;
      public ValueParameter TargetWheelMaximumSpeed;
      public ValueParameter TargetWheelLowSpeedScale;
      public double TargetWheelVelocityToRpm;

      public ValueParameter LaserSampleTime;
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
         this.VersionCount = 10; // update after each addition

         this.LaserBus = new LaserBusParameters();
         this.LaserBus.BusInterface = BusInterfaces.PCIA;
         this.LaserBus.BitRate = 50000;
         this.LaserBus.ConsumerHeartbeatRate = 3500;
         this.LaserBus.ProducerHeartbeatRate = 1000;
         this.LaserBus.ControllerBusId = 80;
         this.LaserBus.LaserBoardBusId = 32;
         this.LaserBus.ControllerTraceMask = 0;
         this.LaserBus.MainBoardTraceMask = 1;
         
         this.TargetBus = new TargetBusParameters();
         this.TargetBus.BusInterface = BusInterfaces.PCIB;
         this.TargetBus.BitRate = 50000;
         this.TargetBus.ConsumerHeartbeatRate = 3500;
         this.TargetBus.ProducerHeartbeatRate = 1000;
         this.TargetBus.ControllerBusId = 80;
         this.TargetBus.TargetBoardBusId = 32;
         this.TargetBus.ControllerTraceMask = 0;
         this.TargetBus.TargetBoardTraceMask = 1;


         this.Trace = new IpEndpointParameters("Trace", "127.0.0.1", 10000);


         this.JoystickDeadband = 5000;
         this.JoystickIdleBand = 4000;


         this.LaserFrontWheel = new WheelMotorParameters();
         this.LaserFrontWheel.Location = "LaserFrontWheel";
         this.LaserFrontWheel.MotorState = WheelMotorStates.enabled;
         this.LaserFrontWheel.RequestInverted = false;
         this.LaserFrontWheel.PositionInverted = false;
         this.LaserFrontWheel.ProfileVelocity = 500;
         this.LaserFrontWheel.ProfileAcceleration = 200;
         this.LaserFrontWheel.ProfileDeceleration = 200;

         this.LaserRearWheel = new WheelMotorParameters();
         this.LaserRearWheel.Location = "LaserRearWheel";
         this.LaserRearWheel.MotorState = WheelMotorStates.enabled;
         this.LaserRearWheel.RequestInverted = false;
         this.LaserRearWheel.PositionInverted = true;
         this.LaserRearWheel.ProfileVelocity = 500;
         this.LaserRearWheel.ProfileAcceleration = 200;
         this.LaserRearWheel.ProfileDeceleration = 200;

         this.LaserXStepper = new StepperMotorParameters();
         this.LaserXStepper.Location = "LaserXStepper";
         this.LaserXStepper.HomeOffset = 2000;
         this.LaserXStepper.HomingSwitchVelocity = 1000;
         this.LaserXStepper.HomingZeroVelocity = 500;
         this.LaserXStepper.HomingAcceleration = 200;
         this.LaserXStepper.ProfileVelocity = 500;
         this.LaserXStepper.ProfileAcceleration = 200;
         this.LaserXStepper.MaximumPosition = 10000;
         this.LaserXStepper.CenterPosition = 5000;
         this.LaserXStepper.MinimumPosition = 0;

         this.LaserYStepper = new StepperMotorParameters();
         this.LaserYStepper.Location = "LaserYStepper";
         this.LaserYStepper.HomeOffset = 2000;
         this.LaserYStepper.HomingSwitchVelocity = 1000;
         this.LaserYStepper.HomingZeroVelocity = 500;
         this.LaserYStepper.HomingAcceleration = 200;
         this.LaserYStepper.ProfileVelocity = 500;
         this.LaserYStepper.ProfileAcceleration = 200;
         this.LaserYStepper.MaximumPosition = 10000;
         this.LaserYStepper.CenterPosition = 5000;
         this.LaserYStepper.MinimumPosition = 0;

         this.LaserWheelMaximumSpeed = new ValueParameter("LaserWheelMaximumSpeed", "m/MIN", 2, 0, 10, 0.10, 3.5, 3.5);
         this.LaserWheelLowSpeedScale = new ValueParameter("LaserWheelLowSpeedScale", "%", 0, 1, 100, 1, 30, 30);
         this.LaserWheelVelocityToRpm = 100;


         this.TargetFrontWheel = new WheelMotorParameters();
         this.TargetFrontWheel.Location = "TargetFrontWheel";
         this.TargetFrontWheel.MotorState = WheelMotorStates.enabled;
         this.TargetFrontWheel.RequestInverted = false;
         this.TargetFrontWheel.PositionInverted = false;
         this.TargetFrontWheel.ProfileVelocity = 500;
         this.TargetFrontWheel.ProfileAcceleration = 200;
         this.TargetFrontWheel.ProfileDeceleration = 200;

         this.TargetRearWheel = new WheelMotorParameters();
         this.TargetRearWheel.Location = "TargetRearWheel";
         this.TargetRearWheel.MotorState = WheelMotorStates.enabled;
         this.TargetRearWheel.RequestInverted = false;
         this.TargetRearWheel.PositionInverted = true;
         this.TargetRearWheel.ProfileVelocity = 500;
         this.TargetRearWheel.ProfileAcceleration = 200;
         this.TargetRearWheel.ProfileDeceleration = 200;

         this.TargetStepper = new StepperMotorParameters();
         this.TargetStepper.Location = "TargetStepper";
         this.TargetStepper.HomeOffset = 2000;
         this.TargetStepper.HomingSwitchVelocity = 1000;
         this.TargetStepper.HomingZeroVelocity = 500;
         this.TargetStepper.HomingAcceleration = 200;
         this.TargetStepper.ProfileVelocity = 500;
         this.TargetStepper.ProfileAcceleration = 200;
         this.TargetStepper.MaximumPosition = 10000;
         this.TargetStepper.CenterPosition = 5000;
         this.TargetStepper.MinimumPosition = 0;

         this.TargetWheelMaximumSpeed = new ValueParameter("TargetWheelMaximumSpeed", "m/MIN", 2, 0, 10, 0.10, 3.5, 3.5);
         this.TargetWheelLowSpeedScale = new ValueParameter("TargetWheelLowSpeedScale", "%", 0, 1, 100, 1, 30, 30);
         this.TargetWheelVelocityToRpm = 100;


         this.LaserSampleTime = new ValueParameter("LaserSampleTime", "s", 2, 0.15, 3.75, 0.15, 0.9, 0.9);
         this.LaserSampleCount = new ValueParameter("LaserSampleCount", "", 0, 1, 128, 1, 4, 4);
         this.LaserMeasurementConstant = new ValueParameter("LaserMeasurementConstant", "mm", 0, 1, 1, 1, 1, 1);
         

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

         int controllerTraceMask = 0;
         int mainBoardTraceMask = 0;

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
               else if ("ControllerTraceMask" == reader.Name)
               {
                  controllerTraceMask = this.ReadInt(reader);
               }
               else if ("MainBoardTraceMask" == reader.Name)
               {
                  mainBoardTraceMask = this.ReadInt(reader);
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

                  result.ControllerTraceMask = controllerTraceMask;
                  result.MainBoardTraceMask = mainBoardTraceMask;

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

      private StepperMotorParameters ReadStepperMotorParameters(XmlReader reader)
      {
         StepperMotorParameters temp = new StepperMotorParameters();
         StepperMotorParameters result = new StepperMotorParameters();
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
               else if ("HomeOffset" == reader.Name)
               {
                  temp.HomeOffset = this.ReadInt(reader);
               }
               else if ("HomingSwitchVelocity" == reader.Name)
               {
                  temp.HomingSwitchVelocity = this.ReadInt(reader);
               }
               else if ("HomingZeroVelocity" == reader.Name)
               {
                  temp.HomingZeroVelocity = this.ReadInt(reader);
               }
               else if ("HomingAcceleration" == reader.Name)
               {
                  temp.HomingAcceleration = this.ReadInt(reader);
               }
               else if ("ProfileVelocity" == reader.Name)
               {
                  temp.ProfileVelocity = this.ReadInt(reader);
               }
               else if ("ProfileAcceleration" == reader.Name)
               {
                  temp.ProfileAcceleration = this.ReadInt(reader);
               }
               else if ("MaximumPosition" == reader.Name)
               {
                  temp.MaximumPosition = this.ReadInt(reader);
               }
               else if ("CenterPosition" == reader.Name)
               {
                  temp.CenterPosition = this.ReadInt(reader);
               }
               else if ("MinimumPosition" == reader.Name)
               {
                  temp.MinimumPosition = this.ReadInt(reader);
               }
            }
            else
            {
               if ("StepperMotorParameters" == reader.Name)
               {
                  result = temp;
                  break;
               }
            }
         }

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
                     else if ("JoystickDeadband" == reader.Name)
                     {
                        this.JoystickDeadband = this.ReadInt(reader);
                     }
                     else if ("JoystickIdleBand" == reader.Name)
                     {
                        this.JoystickIdleBand = this.ReadInt(reader);
                     }
                     else if ("LaserWheelVelocityToRpm" == reader.Name)
                     {
                        this.LaserWheelVelocityToRpm = this.ReadDouble(reader);
                     }
                     else if ("TargetWheelVelocityToRpm" == reader.Name)
                     {
                        this.TargetWheelVelocityToRpm = this.ReadDouble(reader);
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
                        else if ("TargetWheelMaximumSpeed" == valueParameter.Name)
                        {
                           this.TargetWheelMaximumSpeed = valueParameter;
                        }
                        else if ("TargetWheelLowSpeedScale" == valueParameter.Name)
                        {
                           this.TargetWheelLowSpeedScale = valueParameter;
                        }
                        else if ("LaserSampleTime" == valueParameter.Name)
                        {
                           this.LaserSampleTime = valueParameter;
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
                     else if ("StepperMotorParameters" == reader.Name)
                     {
                        StepperMotorParameters stepperMotorParameters = this.ReadStepperMotorParameters(reader);

                        if (null != stepperMotorParameters)
                        {
                           if ("LaserXStepper" == stepperMotorParameters.Location)
                           {
                              this.LaserXStepper = stepperMotorParameters;
                           }
                           else if ("LaserYStepper" == stepperMotorParameters.Location)
                           {
                              this.LaserYStepper = stepperMotorParameters;
                           }
                           else if ("TargetStepper" == stepperMotorParameters.Location)
                           {
                              this.TargetStepper = stepperMotorParameters;
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

         this.WriteElement(writer, "ControllerTraceMask", laserBusParameters.ControllerTraceMask);
         this.WriteElement(writer, "MainBoardTraceMask", laserBusParameters.MainBoardTraceMask);

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
         this.WriteElement(writer, "RequestInverted", wheelMotorParameters.RequestInverted);
         this.WriteElement(writer, "PositionInverted", wheelMotorParameters.PositionInverted);

         this.WriteElement(writer, "ProfileVelocity", wheelMotorParameters.ProfileVelocity);
         this.WriteElement(writer, "ProfileAcceleration", wheelMotorParameters.ProfileAcceleration);
         this.WriteElement(writer, "ProfileDeceleration", wheelMotorParameters.ProfileDeceleration);

         writer.WriteEndElement();
      }

      private void WriteStepperMotorParameters(XmlWriter writer, StepperMotorParameters stepperMotorParameters)
      {
         writer.WriteStartElement("StepperMotorParameters");

         this.WriteElement(writer, "Location", stepperMotorParameters.Location);

         this.WriteElement(writer, "HomeOffset", stepperMotorParameters.HomeOffset);
         this.WriteElement(writer, "HomingSwitchVelocity", stepperMotorParameters.HomingSwitchVelocity);
         this.WriteElement(writer, "HomingZeroVelocity", stepperMotorParameters.HomingZeroVelocity);
         this.WriteElement(writer, "HomingAcceleration", stepperMotorParameters.HomingAcceleration);         

         this.WriteElement(writer, "ProfileVelocity", stepperMotorParameters.ProfileVelocity);
         this.WriteElement(writer, "ProfileAcceleration", stepperMotorParameters.ProfileAcceleration);

         this.WriteElement(writer, "MaximumPosition", stepperMotorParameters.MaximumPosition);
         this.WriteElement(writer, "CenterPosition", stepperMotorParameters.CenterPosition);
         this.WriteElement(writer, "MinimumPosition", stepperMotorParameters.MinimumPosition);

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

            this.WriteIpEndpointParameters(writer, this.Trace);

            this.WriteElement(writer, "JoystickDeadband", this.JoystickDeadband);
            this.WriteElement(writer, "JoystickIdleBand", this.JoystickIdleBand);

            this.WriteWheelMotorParameters(writer, this.LaserFrontWheel);
            this.WriteWheelMotorParameters(writer, this.LaserRearWheel);
            this.WriteStepperMotorParameters(writer, this.LaserXStepper);
            this.WriteStepperMotorParameters(writer, this.LaserYStepper);
            this.WriteValueParameters(writer, this.LaserWheelMaximumSpeed);
            this.WriteValueParameters(writer, this.LaserWheelLowSpeedScale);
            this.WriteElement(writer, "LaserWheelVelocityToRpm", this.LaserWheelVelocityToRpm);

            this.WriteWheelMotorParameters(writer, this.TargetFrontWheel);
            this.WriteWheelMotorParameters(writer, this.TargetRearWheel);
            this.WriteStepperMotorParameters(writer, this.TargetStepper);
            this.WriteValueParameters(writer, this.TargetWheelMaximumSpeed);
            this.WriteValueParameters(writer, this.TargetWheelLowSpeedScale);
            this.WriteElement(writer, "TargetWheelVelocityToRpm", this.TargetWheelVelocityToRpm);

            this.WriteValueParameters(writer, this.LaserSampleTime);
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

      #endregion
   }
}