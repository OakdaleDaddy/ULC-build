
namespace NICBOT.GUI
{
   using System;
   using PCANLight;

   public class RobotBusParameters
   {
      public BusInterfaces BusInterface;
      public int BitRate;

      public int ConsumerHeartbeatRate;
      public int ProducerHeartbeatRate;
      public int ControllerBusId;

      public int RobotBodyBusId;
      public int RobotTopFrontWheelBusId;
      public int RobotTopRearWheelBusId;
      public int RobotBottomFrontWheelBusId;
      public int RobotBottomRearWheelBusId;

      public int ControllerTraceMask;
      public int RobotBodyTraceMask;
      public int RobotTopFrontWheelTraceMask;
      public int RobotTopRearWheelTraceMask;
      public int RobotBottomFrontWheelTraceMask;
      public int RobotBottomRearWheelTraceMask;

      public RobotBusParameters()
      {
         this.BusInterface = BusInterfaces.USBA;
         this.BitRate = 0;

         this.ConsumerHeartbeatRate = 0;
         this.ProducerHeartbeatRate = 0;
         this.ControllerBusId = 0;

         this.RobotBodyBusId = 0;
         this.RobotTopFrontWheelBusId = 0;
         this.RobotTopRearWheelBusId = 0;
         this.RobotBottomFrontWheelBusId = 0;
         this.RobotBottomRearWheelBusId = 0;

         this.ControllerTraceMask = 0;
         this.RobotBodyTraceMask = 0;
         this.RobotTopFrontWheelTraceMask = 0;
         this.RobotTopRearWheelTraceMask = 0;
         this.RobotBottomFrontWheelTraceMask = 0;
         this.RobotBottomRearWheelTraceMask = 0;
      }
   }
}