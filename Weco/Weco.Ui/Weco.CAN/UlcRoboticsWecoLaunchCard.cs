namespace Weco.CAN
{
   using System;
   using System.Text;
   using System.Threading;

   using Weco.Utilities;

   public class UlcRoboticsWecoLaunchCard : Device
   {
      #region Fields

      private DeviceComponent cameraLights;
      private DeviceComponent analogIo;

      #endregion

      #region Properties

      public DeviceComponent CameraLights
      {
         get
         {
            if (null == this.cameraLights)
            {
               this.cameraLights = new DeviceComponent();
            }

            return (this.cameraLights);
         }
      }

      public DeviceComponent AnalogIo
      {
         get
         {
            if (null == this.analogIo)
            {
               this.analogIo = new DeviceComponent();
            }

            return (this.analogIo);
         }
      }

      #endregion

      #region Constructor

      public UlcRoboticsWecoLaunchCard(string name, byte nodeId)
         : base(name, nodeId)
      {
         #region Camera/Light Initialization

         this.CameraLights.Name = this.Name + " cameralight";
         this.CameraLights.OnCommExchange = new DeviceComponent.CommExchangeHandler(this.ExchangeCommAction);
         this.CameraLights.OnClearErrorCode = new DeviceComponent.ClearErrorCodeHandler(this.ClearErrorCode);

         #endregion

         #region Analog IO Initialization

         this.AnalogIo.Name = this.Name + " analogio";
         this.AnalogIo.OnCommExchange = new DeviceComponent.CommExchangeHandler(this.ExchangeCommAction);
         this.AnalogIo.OnClearErrorCode = new DeviceComponent.ClearErrorCodeHandler(this.ClearErrorCode);

         #endregion         
      }

      #endregion

      #region Access Methods

      #region General Functions

      public override void Initialize()
      {
         this.CameraLights.Initialize();
         this.AnalogIo.Initialize();

         base.Initialize();
      }

      public override bool Configure()
      {
         bool result = false;

         if (null == this.FaultReason)
         {
            result = true;

            result &= base.Configure();
         }

         return (result);
      }

      public override bool Start()
      {
         bool result = false;

         if (null == this.FaultReason)
         {
            result = base.Start();
         }

         return (result);
      }

      public override void Stop()
      {
         base.Stop();
      }

      public override void Reset()
      {
         this.CameraLights.Reset();
         this.AnalogIo.Reset();
         base.Reset();
      }

      public override void Update()
      {
         base.Update();
      }

      public override void SetFault(string faultReason, bool resetDevice)
      {
         this.CameraLights.SetFault("device offline");
         this.AnalogIo.SetFault("device offline");
         base.SetFault(faultReason, resetDevice);
      }

      #endregion

      #region Emergency / Error Functions

      public bool ClearErrorCode(UInt32 errorCode)
      {
         bool result = true;

         result &= this.ExchangeCommAction(new SDODownload(0x2400, 0, 1, (UInt32)1));

         return (result);
      }

      #endregion

      #endregion
   }
}