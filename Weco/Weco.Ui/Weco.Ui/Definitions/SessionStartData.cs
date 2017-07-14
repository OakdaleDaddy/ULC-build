
namespace Weco.Ui
{
   using System;

   public class SessionStartData : SessionData
   {
      public double LaserFront { set; get; }
      public double LaserRear { set; get; }
      public double TargetFront { set; get; }
      public double TargetRear { set; get; }
      public double Latitude { set; get; }
      public double Longitude { set; get; }

      public SessionStartData()
         : base()
      {
      }

      public SessionStartData(double laserFront, double laserRear, double targetFront, double targetRear, double latitude, double longitude)
         : base()
      {
         this.LaserFront = laserFront;
         this.LaserRear = laserRear;
         this.TargetFront = targetFront;
         this.TargetRear = targetRear;
         this.Latitude = latitude;
         this.Longitude = longitude;
      }

      public override string GetCsvData()
      {
         string baseCsvData = base.GetCsvData();
         DateTime dt = this.DataTime;
         string result = string.Format("start,{0},{1:0},{2:0},{3:0},{4:0},{5:0.000000},{6:0.000000}", baseCsvData, this.LaserFront, this.LaserRear, this.TargetFront, this.TargetRear, this.Latitude, this.Longitude);
         return (result);
      }

      public override string GetDescription()
      {
         string result = string.Format("start: laser=({0},{1}), target=({2},{3})", this.LaserFront, this.LaserRear, this.TargetFront, this.TargetRear);
         return (result);
      }
   }

}