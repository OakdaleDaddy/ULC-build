
namespace Weco.Ui
{
   using System;

   public class SessionMeasurementData : SessionData
   {
      public enum Types
      {
         between, 
         service,
      }

      public double Distance { set; get; }
      public Types Type { set; get; }

      public SessionMeasurementData()
         : base()
      {
      }

      public SessionMeasurementData(double distance, Types type)
         : base()
      {
         this.Distance = distance;
         this.Type = type;
      }

      public override string GetCsvData()
      {
         string baseCsvData = base.GetCsvData();
         DateTime dt = this.DataTime;
         string result = string.Format("measure,{0},{1},{2:0.00}", baseCsvData, this.Type.ToString(), this.Distance);
         return (result);
      }

      public override string GetDescription()
      {
         string formatString = "{0:0";
         int precision = ParameterAccessor.Instance.LaserMeasurementConstant.Precision;

         if (ParameterAccessor.Instance.LaserMeasurementConstant.Precision > 0)
         {
            formatString += ".";

            for (int i = 0; i < precision; i++)
            {
               formatString += "0";
            }
         }

         formatString += "} {1}";

         string measurementString = string.Format(formatString, this.Distance, ParameterAccessor.Instance.LaserMeasurementConstant.Unit);
         string result = string.Format("measure: {0} {1}", this.Type.ToString(), measurementString);

         return (result);
      }
   }

}