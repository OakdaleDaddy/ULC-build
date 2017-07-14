
namespace Weco.Ui
{
   using System;

   public class SessionMovementData : SessionData
   {
      public double LaserFront { set; get; }
      public double LaserRear { set; get; }
      public double TargetFront { set; get; }
      public double TargetRear { set; get; }

      public SessionMovementData()
         : base()
      {
      }

      public SessionMovementData(double laserFront, double laserRear, double targetFront, double targetRear)
         : base()
      {
         this.LaserFront = laserFront;
         this.LaserRear = laserRear;
         this.TargetFront = targetFront;
         this.TargetRear = targetRear;
      }

      public override string GetCsvData()
      {
         string baseCsvData = base.GetCsvData();
         DateTime dt = this.DataTime;
         string result = string.Format("movement,{0},{1:0},{2:0},{3:0},{4:0}", baseCsvData, this.LaserFront, this.LaserRear, this.TargetFront, this.TargetRear);
         return (result);
      }

      public override string GetDescription()
      {
         string result = string.Format("movement: laser=({0},{1}), target=({2},{3})", this.LaserFront, this.LaserRear, this.TargetFront, this.TargetRear);
         return (result);
      }
   }

}