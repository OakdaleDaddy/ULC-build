
namespace Weco.Ui
{
   using System;

   public class SessionData
   {
      public DateTime DataTime { set; get; }

      public SessionData()
      {
         this.DataTime = DateTime.Now;
      }

      public virtual string GetCsvData()
      {
         DateTime dt = this.DataTime;
         string result = string.Format("{0:D4}-{1:D2}-{2:D2},{3:D2}:{4:D2}:{5:D2}", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
         return (result);
      }

      public virtual string GetDescription()
      {
         string result = "";
         return (result);
      }

   }

}
