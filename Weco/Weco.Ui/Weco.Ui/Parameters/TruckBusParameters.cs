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
      public int FeederLeftMotorBusId;
      public int FeederRightMotorBusId;
      public int ReelMotorBusId;
      public int ReelEncoderBusId;
      public int ReelDigitalIoBusId;
      public int OsdRs232BusId;
      public int ControllerTraceMask;
      public int LaunchCardTraceMask;
      public int BulletMotorTraceMask;
      public int FeederLeftMotorTraceMask;
      public int FeederRightMotorTraceMask;
      public int ReelMotorTraceMask;
      public int ReelEncoderTraceMask;
      public int ReelDigitalIoTraceMask;
      public int OsdRs232TraceMask;

      public TruckBusParameters()
      {
         this.BusInterface = BusInterfaces.USBA;
         this.BitRate = 0;
         this.ConsumerHeartbeatRate = 0;
         this.ProducerHeartbeatRate = 0;
         this.ControllerBusId = 0;
         this.LaunchCardBusId = 0;
         this.BulletMotorBusId = 0;
         this.FeederLeftMotorBusId = 0;
         this.FeederRightMotorBusId = 0;
         this.ReelMotorBusId = 0;
         this.ReelEncoderBusId = 0;
         this.ReelDigitalIoBusId = 0;
         this.OsdRs232BusId = 0;
         this.ControllerTraceMask = 0;
         this.LaunchCardTraceMask = 0;
         this.BulletMotorTraceMask = 0;
         this.FeederLeftMotorTraceMask = 0;
         this.FeederRightMotorTraceMask = 0;
         this.ReelMotorTraceMask = 0;
         this.ReelEncoderTraceMask = 0;
         this.ReelDigitalIoTraceMask = 0;
         this.OsdRs232TraceMask = 0;
      }
   }
}
