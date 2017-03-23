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

      public MainBusParameters MainBus;
      public TargetBusParameters TargetBus;

      public IpEndpointParameters Trace;

      public int JoystickDeadband;
      public int JoystickIdleBand;

      public ValueParameter LaserSampleTime;
      public ValueParameter LaserSampleCount;
      public ValueParameter LaserMeasurementConstant;

      public StepperMotorParameters LaserXStepper;
      public StepperMotorParameters LaserYStepper;
      public StepperMotorParameters TargetStepper;

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
         this.VersionCount = 4; // update after each addition

         this.MainBus = new MainBusParameters();
         this.MainBus.BusInterface = BusInterfaces.PCIA;
         this.MainBus.BitRate = 50000;
         this.MainBus.ConsumerHeartbeatRate = 3500;
         this.MainBus.ProducerHeartbeatRate = 1000;         
         this.MainBus.ControllerBusId = 80;
         this.MainBus.MainBoardBusId = 32;         
         this.MainBus.ControllerTraceMask = 0;
         this.MainBus.MainBoardTraceMask = 1;
         
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

      private MainBusParameters ReadMainBusParameters(XmlReader reader)
      {
         MainBusParameters result = null;
         bool readResult = true;

         BusInterfaces busInterface = BusInterfaces.USBA;
         int bitRate = 0;

         int consumerHeartbeatRate = 0;
         int producerHeartbeatRate = 0;
         int controllerBusId = 0;

         int mainBoardBusId = 0;

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
               else if ("MainBoardBusId" == reader.Name)
               {
                  mainBoardBusId = this.ReadInt(reader);
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
               if ("MainBus" == reader.Name)
               {
                  result = new MainBusParameters();

                  result.BusInterface = busInterface;
                  result.BitRate = bitRate;

                  result.ConsumerHeartbeatRate = consumerHeartbeatRate;
                  result.ProducerHeartbeatRate = producerHeartbeatRate;
                  result.ControllerBusId = controllerBusId;

                  result.MainBoardBusId = mainBoardBusId;

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
                     else if ("MainBus" == reader.Name)
                     {
                        MainBusParameters mainBusParameters = this.ReadMainBusParameters(reader);

                        if (null != mainBusParameters)
                        {
                           this.MainBus = mainBusParameters;
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
                     else if ("Value" == reader.Name)
                     {
                        ValueParameter valueParameter = this.ReadValueParameters(reader);

                        if ("LaserSampleTime" == valueParameter.Name)
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

      private void WriteMainBusParameters(XmlWriter writer, MainBusParameters mainBusParameters)
      {
         writer.WriteStartElement("MainBus");

         writer.WriteComment("BusInterface from {USBA, USBB, PCIA, PCIB}");
         this.WriteElement(writer, "BusInterface", mainBusParameters.BusInterface.ToString());
         this.WriteElement(writer, "BitRate", mainBusParameters.BitRate);

         writer.WriteComment("set heartbeat rates to 0 to disable, restore consumer to 3500 and producer to 1000 (3:1)");
         this.WriteElement(writer, "ConsumerHeartbeatRate", mainBusParameters.ConsumerHeartbeatRate);
         this.WriteElement(writer, "ProducerHeartbeatRate", mainBusParameters.ProducerHeartbeatRate);
         this.WriteElement(writer, "ControllerBusId", mainBusParameters.ControllerBusId);

         this.WriteElement(writer, "MainBoardBusId", mainBusParameters.MainBoardBusId);

         this.WriteElement(writer, "ControllerTraceMask", mainBusParameters.ControllerTraceMask);
         this.WriteElement(writer, "MainBoardTraceMask", mainBusParameters.MainBoardTraceMask);

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

            this.WriteMainBusParameters(writer, this.MainBus);
            this.WriteTargetBusParameters(writer, this.TargetBus);

            this.WriteIpEndpointParameters(writer, this.Trace);

            this.WriteElement(writer, "JoystickDeadband", this.JoystickDeadband);
            this.WriteElement(writer, "JoystickIdleBand", this.JoystickIdleBand);

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