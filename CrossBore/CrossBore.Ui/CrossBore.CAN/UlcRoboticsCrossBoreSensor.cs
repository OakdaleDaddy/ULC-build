
namespace CrossBore.CAN
{
   using System;
   using System.Text;
   using System.Threading;

   using CrossBore.Utilities;

   public class UlcRoboticsCrossBoreSensor : Device
   {
      #region Fields

      #endregion

      #region Helper Functions

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
                  if (0x6001 == errorCode)
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

      public UlcRoboticsCrossBoreSensor(string name, byte nodeId)
         : base(name, nodeId)
      {
      }

      #endregion

      #region Access Methods

      #region General Functions

      public override void Initialize()
      {
         base.Initialize();
      }

      public override bool Configure()
      {
         bool result = false;

         if (null == this.FaultReason)
         {
            result = true;

            // set TPDO1 on sync with 200mS inhibit time
            result &= this.SetTPDOEnable(1, false);
            result &= this.SetTPDOMapCount(1, 0);
            result &= this.SetTPDOType(1, 1);
            result &= this.SetTPDOInhibitTime(1, 200);
            result &= this.SetTPDOMap(1, 1, 0x2400, 0x01, 2); // reading 1
            result &= this.SetTPDOMap(1, 2, 0x2400, 0x02, 2); // reading 2
            result &= this.SetTPDOMap(1, 3, 0x2400, 0x03, 2); // reading 3
            result &= this.SetTPDOMap(1, 4, 0x2400, 0x04, 2); // reading 4
            result &= this.SetTPDOMapCount(1, 4);
            result &= this.SetTPDOEnable(1, true);

            // set TPDO2 on sync with 200mS inhibit time
            result &= this.SetTPDOEnable(2, false);
            result &= this.SetTPDOMapCount(2, 0);
            result &= this.SetTPDOType(2, 1);
            result &= this.SetTPDOInhibitTime(2, 200);
            result &= this.SetTPDOMap(2, 1, 0x2400, 0x05, 2); // reading 5
            result &= this.SetTPDOMap(2, 2, 0x2400, 0x06, 2); // reading 6
            result &= this.SetTPDOMap(2, 3, 0x2400, 0x07, 2); // reading 7
            result &= this.SetTPDOMap(2, 4, 0x2400, 0x08, 2); // reading 8
            result &= this.SetTPDOMapCount(2, 4);
            result &= this.SetTPDOEnable(2, true);

            // set TPDO3 on sync with 200mS inhibit time
            result &= this.SetTPDOEnable(3, false);
            result &= this.SetTPDOMapCount(3, 0);
            result &= this.SetTPDOType(3, 1);
            result &= this.SetTPDOInhibitTime(3, 200);
            result &= this.SetTPDOMap(3, 1, 0x2400, 0x09, 2); // reading 9
            result &= this.SetTPDOMap(3, 2, 0x2400, 0x0A, 2); // reading 10
            result &= this.SetTPDOMap(3, 3, 0x2400, 0x0B, 2); // reading 11
            result &= this.SetTPDOMap(3, 4, 0x2400, 0x0C, 2); // reading 12
            result &= this.SetTPDOMapCount(3, 4);
            result &= this.SetTPDOEnable(3, true);

            // set TPDO4 on sync with 200mS inhibit time
            result &= this.SetTPDOEnable(4, false);
            result &= this.SetTPDOMapCount(4, 0);
            result &= this.SetTPDOType(4, 1);
            result &= this.SetTPDOInhibitTime(4, 200);
            result &= this.SetTPDOMap(4, 1, 0x2400, 0x0D, 2); // reading 13
            result &= this.SetTPDOMap(4, 2, 0x2400, 0x0E, 2); // reading 14
            result &= this.SetTPDOMap(4, 3, 0x2400, 0x0F, 2); // reading 14
            result &= this.SetTPDOMapCount(4, 3);
            result &= this.SetTPDOEnable(4, true);

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
         base.Reset();
      }

      public override void Update()
      {
         base.Update();
      }

      public override void SetFault(string faultReason, bool resetDevice)
      {
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

      #endregion
   }
}