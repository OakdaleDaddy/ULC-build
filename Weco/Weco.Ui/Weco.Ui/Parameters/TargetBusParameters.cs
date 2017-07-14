namespace Weco.Ui
{
   using System;

   using Weco.PCANLight;

   public class TargetBusParameters
   {
      public BusInterfaces BusInterface;
      public int BitRate;

      public int ConsumerHeartbeatRate;
      public int ProducerHeartbeatRate;
      public int ControllerBusId;

      public int TargetBoardBusId;

      public int ControllerTraceMask;
      public int TargetBoardTraceMask;

      public TargetBusParameters()
      {
         this.BusInterface = BusInterfaces.USBA;
         this.BitRate = 0;

         this.ConsumerHeartbeatRate = 0;
         this.ProducerHeartbeatRate = 0;
         this.ControllerBusId = 0;

         this.TargetBoardBusId = 0;

         this.ControllerTraceMask = 0;
         this.TargetBoardTraceMask = 0;
      }
   }
}
