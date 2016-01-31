namespace DYNO.GUI
{
   using System;

   using DYNO.PCANLight;

   public class BusParameters
   {
      #region Properties

      public BusInterfaces BusInterface { set; get; }
      public int BitRate { set; get; }
      public int UutId { set; get; }
      public int EncoderId { set; get; }
      public int AnalogIoId { set; get; }
      public int DigialIoId { set; get; }


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
      }

      #endregion

      #region Constructor

      public BusParameters()
      {
         this.SetDefaults();
      }

      #endregion
   }
}