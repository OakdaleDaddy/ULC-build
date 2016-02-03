namespace DYNO.GUI
{
   using System;

   using DYNO.PCANLight;

   public class SetupParameters
   {
      #region Properties

      public BusInterfaces BusInterface { set; get; }
      public int BitRate { set; get; }
      public int UutId { set; get; }
      public int EncoderId { set; get; }
      public int AnalogIoId { set; get; }
      public int DigialIoId { set; get; }

      public double VoltsPerPounds { set; get; }
      public double UutSpeedToRpm { set; get; }
      public double SupplyVoltageToAmps { set; get; }
      public double BodySpeedToRpm { set; get; }

      #endregion

      #region Helper Functions

      private void SetDefaults()
      {
         this.BusInterface = BusInterfaces.USBA;
         this.BitRate = 50000;
         this.UutId = 1;
         this.EncoderId = 2;
         this.AnalogIoId = 3;
         this.DigialIoId = 4;

         this.VoltsPerPounds = 1.0;
         this.UutSpeedToRpm = 1.0;
         this.SupplyVoltageToAmps = 1.0;
         this.BodySpeedToRpm = 1.0;
      }

      #endregion

      #region Constructor

      public SetupParameters()
      {
         this.SetDefaults();
      }

      #endregion
   }
}