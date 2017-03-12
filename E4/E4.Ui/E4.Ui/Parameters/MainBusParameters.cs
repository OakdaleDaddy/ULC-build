namespace E4.Ui
{
   using System;
   
   using E4.PCANLight;

   public class MainBusParameters
   {
      public BusInterfaces BusInterface;
      public int BitRate;

      public int ConsumerHeartbeatRate;
      public int ProducerHeartbeatRate;
      public int ControllerBusId;

      public int MainBoardBusId;

      public int ControllerTraceMask;
      public int MainBoardTraceMask;

      public MainBusParameters()
      {
         this.BusInterface = BusInterfaces.USBA;
         this.BitRate = 0;

         this.ConsumerHeartbeatRate = 0;
         this.ProducerHeartbeatRate = 0;
         this.ControllerBusId = 0;

         this.MainBoardBusId = 0;

         this.ControllerTraceMask = 0;
         this.MainBoardTraceMask = 0;
      }
   }
}