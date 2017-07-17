namespace Weco.Ui
{
   using System;

   using Weco.PCANLight;

   public class TruckBusParameters
   {
      public BusInterfaces BusInterface;
      public int BitRate;
      public int ConsumerHeartbeatRate;
      public int ProducerHeartbeatRate;
      public int ControllerBusId;      
      public int LaunchCardBusId;
      public int BulletMotorBusId;
      public int LeftFeederMotorBusId;
      public int RightFeederMotorBusId;
      public int ReelMotorBusId;
      public int ReelEncoderBusId;
      public int ControllerTraceMask;
      public int LaunchCardTraceMask;
      public int BulletMotorTraceMask;
      public int LeftFeederMotorTraceMask;
      public int RightFeederMotorTraceMask;
      public int ReelMotorTraceMask;
      public int ReelEncoderTraceMask;

      public TruckBusParameters()
      {
         this.BusInterface = BusInterfaces.USBA;
         this.BitRate = 0;
         this.ConsumerHeartbeatRate = 0;
         this.ProducerHeartbeatRate = 0;
         this.ControllerBusId = 0;
         this.LaunchCardBusId = 0;
         this.BulletMotorBusId = 0;
         this.LeftFeederMotorBusId = 0;
         this.RightFeederMotorBusId = 0;
         this.ReelMotorBusId = 0;
         this.ReelEncoderBusId = 0;
         this.ControllerTraceMask = 0;
         this.LaunchCardTraceMask = 0;
         this.BulletMotorTraceMask = 0;
         this.LeftFeederMotorTraceMask = 0;
         this.RightFeederMotorTraceMask = 0;
         this.ReelMotorTraceMask = 0;
         this.ReelEncoderTraceMask = 0;
      }
   }
}
