namespace DYNO.GUI
{
   using System;

   using DYNO.PCANLight;

   public class SetupParameters
   {
      #region Properties

      public BusInterfaces BusInterface { set; get; }
      public int BitRate { set; get; }
      public int ConsumerHeartbeatNodeId { set; get; }
      public int ConsumerHeartbeatTime { set; get; }
      public int ProducerHeartbeatTime { set; get; }
      public int UutId { set; get; }
      public int EncoderId { set; get; }
      public int AnalogIoId { set; get; }
      public int DigialIoId { set; get; }

      public double UutRpmToSpeed { set; get; }
      public double BodyRpmToSpeed { set; get; }
      public double AnalogIoVoltsToLoadPounds { set; get; }
      public double AnalogIoVoltsToSupplyAmps { set; get; }

      #endregion

      #region Helper Functions

      private void SetDefaults()
      {
         this.BusInterface = BusInterfaces.USBA;
         this.BitRate = 50000;
         this.ConsumerHeartbeatNodeId = 80;
         this.ConsumerHeartbeatTime = 3000;
         this.ProducerHeartbeatTime = 1000;
         this.UutId = 1;
         this.EncoderId = 2;
         this.AnalogIoId = 3;
         this.DigialIoId = 4;

         this.UutRpmToSpeed = 1.0;
         this.BodyRpmToSpeed = 1.0;
         this.AnalogIoVoltsToLoadPounds = 1.0;
         this.AnalogIoVoltsToSupplyAmps = 1.0;
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