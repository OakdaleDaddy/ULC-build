
namespace CrossBore.CAN
{
   using System;
   using System.Text;
   using System.Threading;

   using CrossBore.Utilities;

   public class UlcRoboticsWekoLaunchCard : Device
   {
      #region Fields

      private DeviceComponent cameraLights;
      private DeviceComponent analogIo;

      #endregion

      #region Helper Functions

      private double GetSignedTemperature(byte reading)
      {
         double result = (reading < 127) ? reading : -256 + reading;
         return (result);
      }

      private void TraceReceive(int nodeId, COBTypes frameType, int cobId, byte[] msg)
      {
         if (nodeId == this.NodeId)
         {
            base.TraceReceive(frameType, cobId, msg);
         }
         else if (nodeId == (this.NodeId + 1))
         {
            bool trace = false;

            if (COBTypes.TPDO1 == frameType)
            {
               trace = this.TraceTPDO5;
            }
            else if (COBTypes.TPDO2 == frameType)
            {
               trace = this.TraceTPDO6;
            }
            else if (COBTypes.TPDO3 == frameType)
            {
               trace = this.TraceTPDO7;
            }
            else if (COBTypes.TPDO4 == frameType)
            {
               trace = this.TraceTPDO8;
            }

            if (false != trace)
            {
               if (null != this.OnReceiveTrace)
               {
                  this.OnReceiveTrace(cobId, msg);
               }
            }
         }
      }

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

      public bool TraceTPDO5 { set; get; }
      public bool TraceTPDO6 { set; get; }
      public bool TraceTPDO7 { set; get; }
      public bool TraceTPDO8 { set; get; }

      public new int TraceMask
      {
         set
         {
            base.TraceMask = value;

            this.TraceTPDO5 = ((value & 0x0400) != 0) ? true : false;
            this.TraceTPDO6 = ((value & 0x0800) != 0) ? true : false;
            this.TraceTPDO7 = ((value & 0x1000) != 0) ? true : false;
            this.TraceTPDO8 = ((value & 0x2000) != 0) ? true : false;
         }

         get
         {
            int result = base.TraceMask;

            result |= (false != this.TraceTPDO5) ? 0x0400 : 0;
            result |= (false != this.TraceTPDO6) ? 0x0800 : 0;
            result |= (false != this.TraceTPDO7) ? 0x1000 : 0;
            result |= (false != this.TraceTPDO8) ? 0x2000 : 0;

            return (result);
         }
      }

      public double McuTemperature { set; get; }

      #endregion

      #region Overrides

      protected override void EvaluateReceiveTrace(int cobId, byte[] msg, ref bool traced)
      {
         COBTypes frameType = (COBTypes)((cobId >> 7) & 0xF);
         int nodeId = (int)(cobId & 0x7F);

         if (false == traced)
         {
            if ((nodeId == this.NodeId) ||
                (nodeId == (this.NodeId + 1)))
            {
               this.TraceReceive(nodeId, frameType, cobId, msg);
               traced = true;
            }
         }
      }

      protected override void EvaluateMessage(int cobId, byte[] msg)
      {
         COBTypes frameType = (COBTypes)((cobId >> 7) & 0xF);
         int nodeId = (int)(cobId & 0x7F);

         if (nodeId == this.NodeId)
         {
            if (COBTypes.EMGY == frameType)
            {
               UInt64 errorValue = 0;
               byte[] errorMsg = new byte[8];

               if (null != msg)
               {
                  for (int i = 0; i < msg.Length; i++)
                  {
                     errorMsg[i] = msg[i];
                  }

                  for (int i = msg.Length; i < 8; i++)
                  {
                     errorMsg[i] = 0;
                  }

                  errorValue = BitConverter.ToUInt64(errorMsg, 0);
               }

               if (0 != errorValue)
               {
                  UInt16 errorCode = BitConverter.ToUInt16(errorMsg, 0);
                  UInt32 emergencyRecordCode = BitConverter.ToUInt32(errorMsg, 0);
                  byte errorRegister = errorMsg[2];
                  byte codeSpecificData = errorMsg[3];
                  int subSystem = errorRegister & 0x1F;
                  bool componentFault = ((errorRegister & 0x20) != 0) ? true : false;
                  bool deviceFault = true;
                  bool deviceWarning = false;
                  string reason = string.Format("emergency {0:X16}", errorValue);

                  #region Board Emergencies
                  if (0x1000 == errorCode)
                  {
                     reason = "generic error";
                  }
                  else if (0x6001 == errorCode)
                  {
                     UInt16 additionalData = BitConverter.ToUInt16(errorMsg, 3);
                     string resetTypeString = null;

                     deviceFault = false;

                     if (0 == additionalData)
                     {
                        resetTypeString = "power on reset";
                     }
                     else if (1 == additionalData)
                     {
                        resetTypeString = "watchdog reset";
                     }
                     else if (2 == additionalData)
                     {
                        resetTypeString = "software reset";
                     }
                     else if (3 == additionalData)
                     {
                        resetTypeString = "reset pin reset";
                     }
                     else if (4 == additionalData)
                     {
                        resetTypeString = "option byte reset";
                     }
                     else if (5 == additionalData)
                     {
                        resetTypeString = "direct jump reset";
                     }
                     else if (6 == additionalData)
                     {
                        resetTypeString = "brown out reset";
                     }
                     else
                     {
                        resetTypeString = "undefined reset";
                     }

                     if (null != resetTypeString)
                     {
                        Tracer.WriteHigh(TraceGroup.CANBUS, "", "{0} {1}", this.Name, resetTypeString);
                     }
                  }
                  else if (0x6100 == errorCode)
                  {
                     UInt32 crc = BitConverter.ToUInt32(errorMsg, 3);
                     reason = string.Format("bootloader CRC fail {0:X8}", crc);
                  }
                  else if (0x6200 == errorCode)
                  {
                     UInt32 crc = BitConverter.ToUInt32(errorMsg, 3);
                     reason = string.Format("application CRC fail {0:X8}", crc);
                  }
                  else if (0x6201 == errorCode)
                  {
                     reason = string.Format("application empty");
                  }
                  else if (0x8130 == errorCode)
                  {
                     reason = "controller heartbeat lost";
                  }
                  #endregion

                  #region Component Emergencies
                  else if (0x0000 == errorCode)
                  {
                     if (0x80 == errorRegister)
                     {
                        // subsystem status changed
                        deviceFault = false;
                     }
                  }
                  else if (0x2340 == errorCode)
                  {
                     if (0 == subSystem)
                     {
                        deviceFault = false;

                        if (0 == codeSpecificData)
                        {
                           this.CameraLights.RecordError(componentFault, emergencyRecordCode, "Left LED Shorted");
                        }
                        else if (1 == codeSpecificData)
                        {
                           this.CameraLights.RecordError(componentFault, emergencyRecordCode, "Right LED Shorted");
                        }
                        else if (2 == codeSpecificData)
                        {
                           this.CameraLights.RecordError(componentFault, emergencyRecordCode, "Down LED Shorted");
                        }
                        else if (3 == codeSpecificData)
                        {
                           this.CameraLights.RecordError(componentFault, emergencyRecordCode, "LED3 Shorted");
                        }
                        else if (4 == codeSpecificData)
                        {
                           this.CameraLights.RecordError(componentFault, emergencyRecordCode, "LED4 Shorted");
                        }
                        else if (5 == codeSpecificData)
                        {
                           this.CameraLights.RecordError(componentFault, emergencyRecordCode, "LED5 Shorted");
                        }
                     }
                  }
                  else if (0x3230 == errorCode)
                  {
                     if (0 == subSystem)
                     {
                        deviceFault = false;

                        if (0 == codeSpecificData)
                        {
                           this.CameraLights.RecordError(componentFault, emergencyRecordCode, "Left LED Open");
                        }
                        else if (1 == codeSpecificData)
                        {
                           this.CameraLights.RecordError(componentFault, emergencyRecordCode, "Right LED Open");
                        }
                        else if (2 == codeSpecificData)
                        {
                           this.CameraLights.RecordError(componentFault, emergencyRecordCode, "Down LED Open");
                        }
                        else if (3 == codeSpecificData)
                        {
                           this.CameraLights.RecordError(componentFault, emergencyRecordCode, "LED3 Open");
                        }
                        else if (4 == codeSpecificData)
                        {
                           this.CameraLights.RecordError(componentFault, emergencyRecordCode, "LED4 Open");
                        }
                        else if (5 == codeSpecificData)
                        {
                           this.CameraLights.RecordError(componentFault, emergencyRecordCode, "LED5 Open");
                        }
                     }
                  }
                  else if (0x4210 == errorCode)
                  {
                     if (0 == subSystem)
                     {
                        deviceFault = false;
                        this.CameraLights.RecordError(componentFault, emergencyRecordCode, "LED IC Excess Temperature");
                     }
                  }
                  #endregion

                  if (false != deviceFault)
                  {
                     this.SetFault(reason, false);
                  }
                  else if (false != deviceWarning)
                  {
                     this.SetWarning(reason);
                  }
               }
            }
            else if (COBTypes.TPDO1 == frameType)
            {
               if ((null != msg) && (msg.Length >= 1))
               {
                  this.McuTemperature = this.GetSignedTemperature(msg[0]);
               }
            }
            else if (COBTypes.TPDO2 == frameType)
            {
            }
            else if (COBTypes.TPDO3 == frameType)
            {
            }
            else if (COBTypes.TPDO4 == frameType)
            {
            }
         }
         else if (nodeId == (this.NodeId + 1))
         {
            if (COBTypes.TPDO1 == frameType) // TPDO5
            {
            }
            else if (COBTypes.TPDO2 == frameType) // TPDO6
            {
            }
            else if (COBTypes.TPDO3 == frameType) // TPDO7
            {
            }
            else if (COBTypes.TPDO4 == frameType) // TPDO8
            {
            }
         }
      }

      protected override void EvaluateAction(CommAction action)
      {
         base.EvaluateAction(action);
      }

      #endregion

      #region Constructor

      public UlcRoboticsWekoLaunchCard(string name, byte nodeId)
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

            // set TPDO1 on change with 200mS inhibit time
            result &= this.SetTPDOEnable(1, false);
            result &= this.SetTPDOMapCount(1, 0);
            result &= this.SetTPDOType(1, 254);
            result &= this.SetTPDOInhibitTime(1, 200);
            result &= this.SetTPDOMap(1, 1, 0x2311, 0x01, 1); // MCU temperature
            result &= this.SetTPDOMapCount(1, 1);
            result &= this.SetTPDOEnable(1, true);

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

         result &= this.ExchangeCommAction(new SDODownload(0x5003, 0, 4, errorCode));

         return (result);
      }

      #endregion

      #region Camera Functions

      public bool GetCameraSelect(ref byte videoSelect)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x2301, 1);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 1))
         {
            videoSelect = upload.Data[0];
            result = true;
         }

         return (result);
      }

      public bool SetCameraSelect(byte select)
      {
         this.pendingAction = new SDODownload(0x2301, 1, 1, select);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      public bool GetLedIntensityLevel(byte ledSelect, ref UInt32 ledIntensityLevel)
      {
         bool result = false;
         SDOUpload upload = new SDOUpload(0x2303, ledSelect);
         this.pendingAction = upload;
         bool actionResult = this.ExchangeCommAction(this.pendingAction);

         if ((false != actionResult) && (null != upload.Data) && (upload.Data.Length >= 4))
         {
            ledIntensityLevel = BitConverter.ToUInt32(upload.Data, 0);
            result = true;
         }

         return (result);
      }

      public bool SetLedIntensityLevel(byte ledSelect, UInt32 ledIntensityLevel)
      {
         this.pendingAction = new SDODownload(0x2303, ledSelect, 4, ledIntensityLevel);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      #endregion

      #endregion
   }
}