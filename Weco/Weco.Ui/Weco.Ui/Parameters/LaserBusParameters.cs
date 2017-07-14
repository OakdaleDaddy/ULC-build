namespace Weco.Ui
{
   using System;

   using Weco.PCANLight;

   public class LaserBusParameters
   {
      public BusInterfaces BusInterface;
      public int BitRate;

      public int ConsumerHeartbeatRate;
      public int ProducerHeartbeatRate;
      public int ControllerBusId;

      public int LaserBoardBusId;
      public int GpsBusId;

      public int ControllerTraceMask;
      public int MainBoardTraceMask;
      public int GpsTraceMask;

      public LaserBusParameters()
      {
         this.BusInterface = BusInterfaces.USBA;
         this.BitRate = 0;

         this.ConsumerHeartbeatRate = 0;
         this.ProducerHeartbeatRate = 0;
         this.ControllerBusId = 0;

         this.LaserBoardBusId = 0;
         this.GpsBusId = 0;

         this.ControllerTraceMask = 0;
         this.MainBoardTraceMask = 0;
         this.GpsTraceMask = 0;
      }
   }
}