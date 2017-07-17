namespace Weco.Ui
{
   using System;

   using Weco.PCANLight;

   public class RobotBusParameters
   {
      public BusInterfaces BusInterface;
      public int BitRate;
      public int ConsumerHeartbeatRate;
      public int ProducerHeartbeatRate;
      public int ControllerBusId;
      public int LeftTrackBusId;
      public int HubBusId;
      public int RightTrackBusId;
      public int ControllerTraceMask;
      public int LeftTrackTraceMask;
      public int HubTraceMask;
      public int RightTrackTraceMask;

      public RobotBusParameters()
      {
         this.BusInterface = BusInterfaces.USBA;
         this.BitRate = 0;
         this.ConsumerHeartbeatRate = 0;
         this.ProducerHeartbeatRate = 0;
         this.ControllerBusId = 0;
         this.LeftTrackBusId = 0;
         this.HubBusId = 0;
         this.RightTrackBusId = 0;
         this.ControllerTraceMask = 0;
         this.LeftTrackTraceMask = 0;
         this.HubTraceMask = 0;
         this.RightTrackTraceMask = 0;
      }
   }
}