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
      }
      
      private void Initialize()
      {
         this.setDefaults = false;
         this.AssignDefaults();
      }

      #endregion

      #region Read Functions

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

         this.WriteElement(writer, "TargetBoardMainBusId", targetBusParameters.TargetBoardBusId);

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

      #endregion
   }
}