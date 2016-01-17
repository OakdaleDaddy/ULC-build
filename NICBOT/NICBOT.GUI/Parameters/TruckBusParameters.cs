
namespace NICBOT.GUI
{
   using System;
   using PCANLight;

   public class TruckBusParameters
   {
      public BusInterfaces BusInterface;
      public int BitRate;

      public int ConsumerHeartbeatRate;
      public int ProducerHeartbeatRate;
      public int ControllerBusId;

      public int ReelMotorBusId;
      public int ReelDigitalBusId;
      public int ReelAnalogBusId;
      public int ReelEncoderBusId;
      public int FeederTopFrontMotorBusId;
      public int FeederTopRearMotorBusId;
      public int FeederBottomFrontMotorBusId;
      public int FeederBottomRearMotorBusId;
      public int FeederEncoderBusId;
      public int GuideLeftMotorBusId;
      public int GuideRightMotorBusId;
      public int LaunchDigitalIoBusId;
      public int LaunchAnalogIoBusId;
      public int GpsBusId;
      public int FrontPumpBusId;
      public int FrontScaleRs232BusId;
      public int RearPumpBusId;
      public int RearScaleRs232BusId;

      public int ControllerTraceMask;
      public int ReelMotorTraceMask;
      public int ReelDigitalTraceMask;
      public int ReelAnalogTraceMask;
      public int ReelEncoderTraceMask;
      public int FeederTopFrontMotorTraceMask;
      public int FeederTopRearMotorTraceMask;
      public int FeederBottomFrontMotorTraceMask;
      public int FeederBottomRearMotorTraceMask;
      public int FeederEncoderTraceMask;
      public int GuideLeftMotorTraceMask;
      public int GuideRightMotorTraceMask;
      public int LaunchDigitalIoTraceMask;
      public int LaunchAnalogIoTraceMask;
      public int GpsTraceMask;
      public int FrontPumpTraceMask;
      public int FrontScaleRs232TraceMask;
      public int RearPumpTraceMask;
      public int RearScaleRs232TraceMask;

      public TruckBusParameters()
      {
         this.BusInterface = BusInterfaces.USBA;
         this.BitRate = 0;

         this.ConsumerHeartbeatRate = 0;
         this.ProducerHeartbeatRate = 0;
         this.ControllerBusId = 0;

         this.ReelMotorBusId = 0;
         this.ReelDigitalBusId = 0;
         this.ReelAnalogBusId = 0;
         this.ReelEncoderBusId = 0;
         this.FeederTopFrontMotorBusId = 0;
         this.FeederTopRearMotorBusId = 0;
         this.FeederBottomFrontMotorBusId = 0;
         this.FeederBottomRearMotorBusId = 0;
         this.FeederEncoderBusId = 0;
         this.GuideLeftMotorBusId = 0;
         this.GuideRightMotorBusId = 0;
         this.LaunchDigitalIoBusId = 0;
         this.LaunchAnalogIoBusId = 0;
         this.GpsBusId = 0;
         this.FrontPumpBusId = 0;
         this.FrontScaleRs232BusId = 0;
         this.RearPumpBusId = 0;
         this.RearScaleRs232BusId = 0;

         this.ControllerTraceMask = 0;
         this.ReelMotorTraceMask = 0;
         this.ReelDigitalTraceMask = 0;
         this.ReelAnalogTraceMask = 0;
         this.ReelEncoderTraceMask = 0;
         this.FeederTopFrontMotorTraceMask = 0;
         this.FeederTopRearMotorTraceMask = 0;
         this.FeederBottomFrontMotorTraceMask = 0;
         this.FeederBottomRearMotorTraceMask = 0;
         this.FeederEncoderTraceMask = 0;
         this.GuideLeftMotorTraceMask = 0;
         this.GuideRightMotorTraceMask = 0;
         this.LaunchDigitalIoTraceMask = 0;
         this.LaunchAnalogIoTraceMask = 0;
         this.GpsTraceMask = 0;
         this.FrontPumpTraceMask = 0;
         this.FrontScaleRs232TraceMask = 0;
         this.RearPumpTraceMask = 0;
         this.RearScaleRs232TraceMask = 0;
      }
   }
}
