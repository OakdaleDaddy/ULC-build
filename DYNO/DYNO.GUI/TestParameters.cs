namespace DYNO.GUI
{
   using System;
   using System.Xml;

   public class TestParameters
   {
      #region Properties

      public double RunTime { set; get; }
      public double WheelSpeed { set; get; }
      public double WheelStartLoad { set; get; }
      public double WheelStopLoad { set; get; }
      public double CurrentLimit { set; get; }
      public double ThermalLimit { set; get; }
      public double SlippageLimit { set; get; }

      #endregion

      #region Helper Functions

      private void SetDefaults()
      {
         this.RunTime = 0.0;
         this.WheelSpeed = 0.0;
         this.WheelStartLoad = 0.0;
         this.WheelStopLoad = 0.0;
         this.CurrentLimit = 0.0;
         this.ThermalLimit = 0.0;
         this.SlippageLimit = 0.0;
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

      private void WriteElement(XmlWriter writer, string tag, double value)
      {
         writer.WriteElementString(tag, value.ToString());
      }

      #endregion

      #region Constructor

      public TestParameters()
      {
         this.SetDefaults();
      }

      #endregion

      #region Access Methods

      public string Load(string fileLocation)
      {
         string result = null;

         this.SetDefaults();

         try
         {
            using (XmlReader reader = XmlReader.Create(fileLocation))
            {
               bool readResult = true;

               for (; readResult; )
               {
                  readResult = reader.Read();

                  if (reader.IsStartElement())
                  {
                     switch (reader.Name)
                     {
                        case "RunTime":
                        {
                           this.RunTime = this.ReadDouble(reader);
                           break;
                        }
                        case "WheelSpeed":
                        {
                           this.WheelSpeed = this.ReadDouble(reader);
                           break;
                        }
                        case "WheelStartLoad":
                        {
                           this.WheelStartLoad = this.ReadDouble(reader);
                           break;
                        }
                        case "WheelStopLoad":
                        {
                           this.WheelStopLoad = this.ReadDouble(reader);
                           break;
                        }
                        case "CurrentLimit":
                        {
                           this.CurrentLimit = this.ReadDouble(reader);
                           break;
                        }
                        case "ThermalLimit":
                        {
                           this.ThermalLimit = this.ReadDouble(reader);
                           break;
                        }
                        case "SlippageLimit":
                        {
                           this.SlippageLimit = this.ReadDouble(reader);
                           break;
                        }
                     }
                  }
                  else
                  {
                  }
               }

               reader.Close();
               reader.Dispose();
            }
         }
         catch 
         {
            result = "unable to read file";
         }

         return (result);
      }

      public string Save(string fileLocation)
      {
         string result = null;

         XmlWriterSettings xmls = new XmlWriterSettings();
         xmls.Indent = true;

         using (XmlWriter writer = XmlWriter.Create(fileLocation, xmls))
         {
            writer.WriteStartDocument();
            writer.WriteStartElement("TestParameters");

            this.WriteElement(writer, "RunTime", this.RunTime);
            this.WriteElement(writer, "WheelSpeed", this.WheelSpeed);
            this.WriteElement(writer, "WheelStartLoad", this.WheelStartLoad);
            this.WriteElement(writer, "WheelStopLoad", this.WheelStopLoad);
            this.WriteElement(writer, "CurrentLimit", this.CurrentLimit);
            this.WriteElement(writer, "ThermalLimit", this.ThermalLimit);
            this.WriteElement(writer, "SlippageLimit", this.SlippageLimit);
            
            writer.WriteEndElement();
            writer.WriteEndDocument();
         }

         return (result);
      }

      #endregion
   }

}