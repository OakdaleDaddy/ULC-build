﻿
namespace NICBOT.CAN
{
   using System;
   using System.Collections;
   using System.Text;
   using System.Threading;

   using NICBOT.Utilities;

   public class Device
   {
      #region Definitions 

      private enum CommStates
      {
         idle,
         tx,
         txWait,
         rxWait,
         error,
      }

      public delegate void ReceiveTraceHandler(int id, byte[] data);
      public delegate void TransmitTraceHandler(int id, byte[] data);
      public delegate bool TransmitHandler(int id, byte[] data);
      public delegate void CommErrorHandler(string name, int nodeId, string reason);

      #endregion

      #region Fields

      private string name;
     // private byte nodeId;

      private string faultReason;

      private Queue actionQueue;
      private CommStates commState;
      private DateTime commTimeLimit;

      private CommAction action;

      private int commAttemptCount;

      protected AutoResetEvent commEvent;
      protected CommAction pendingAction;

      protected bool receiveBootupHeartbeart;
      private bool consumerHeartbeatActive;
      private int consumerHeartbeatSetting;
      private DateTime consumerHeartbeatTimeLimit;

      #endregion

      #region Properties

      public ReceiveTraceHandler OnReceiveTrace { set; get; }
      public TransmitTraceHandler OnTransmitTrace { set; get; }
      public TransmitHandler OnTransmit { set; get; }
      public CommErrorHandler OnCommError { set; get; }

      public string Name { get { return (this.name); } }
      public byte NodeId { set; get; }
      //public byte NodeId { set { this.nodeId = value; } get { return (this.nodeId); } } // todo reduce this?

      public UInt32 DeviceType { set; get; }
      public string DeviceName { set; get; }
      public string DeviceVersion { set; get; }

      public bool TraceSDO { set; get; }
      public bool TraceHB { set; get; }
      public bool TraceTPDO1 { set; get; }
      public bool TraceRPDO1 { set; get; }
      public bool TraceTPDO2 { set; get; }
      public bool TraceRPDO2 { set; get; }
      public bool TraceTPDO3 { set; get; }
      public bool TraceRPDO3 { set; get; }
      public bool TraceTPDO4 { set; get; }
      public bool TraceRPDO4 { set; get; }

      public int TraceMask
      {
         set
         {
            this.TraceSDO = ((value & 0x0001) != 0) ? true : false;
            this.TraceHB = ((value & 0x0002) != 0) ? true : false;
            this.TraceTPDO1 = ((value & 0x0004) != 0) ? true : false;
            this.TraceRPDO1 = ((value & 0x0008) != 0) ? true : false;
            this.TraceTPDO2 = ((value & 0x0010) != 0) ? true : false;
            this.TraceRPDO2 = ((value & 0x0020) != 0) ? true : false;
            this.TraceTPDO3 = ((value & 0x0040) != 0) ? true : false;
            this.TraceRPDO3 = ((value & 0x0080) != 0) ? true : false;
            this.TraceTPDO4 = ((value & 0x0100) != 0) ? true : false;
            this.TraceRPDO4 = ((value & 0x0200) != 0) ? true : false;            
         }

         get
         {
            int result = 0;

            result |= (false != this.TraceSDO) ? 0x0001 : 0;
            result |= (false != this.TraceHB) ? 0x0002 : 0;
            result |= (false != this.TraceTPDO1) ? 0x0004 : 0;
            result |= (false != this.TraceRPDO1) ? 0x0008 : 0;
            result |= (false != this.TraceTPDO2) ? 0x0010 : 0;
            result |= (false != this.TraceRPDO2) ? 0x0020 : 0;
            result |= (false != this.TraceTPDO3) ? 0x0040 : 0;
            result |= (false != this.TraceRPDO3) ? 0x0080 : 0;
            result |= (false != this.TraceTPDO4) ? 0x0100 : 0;
            result |= (false != this.TraceRPDO4) ? 0x0200 : 0;

            return (result);
         }
      }

      public bool ReceiveBootupHeartbeat { get { return (this.receiveBootupHeartbeart); } }
      public string FaultReason { get { return (this.faultReason); } }

      public int TransmitFailureHoldoffTime { set; get; }

      #endregion

      #region Helper Functions

      protected UInt16 GetTPDOParameterAddress(int pdo)
      {
         UInt16 result = 0;

         if (1 == pdo)
         {
            result = 0x1800;
         }
         else if (2 == pdo)
         {
            result = 0x1801;
         }
         else if (3 == pdo)
         {
            result = 0x1802;
         }
         else if (4 == pdo)
         {
            result = 0x1803;
         }

         return (result);
      }

      protected UInt16 GetTPDOMapAddress(int pdo)
      {
         UInt16 result = 0;

         if (1 == pdo)
         {
            result = 0x1A00;
         }
         else if (2 == pdo)
         {
            result = 0x1A01;
         }
         else if (3 == pdo)
         {
            result = 0x1A02;
         }
         else if (4 == pdo)
         {
            result = 0x1A03;
         }

         return (result);
      }

      protected UInt16 GetRPDOParameterAddress(int pdo)
      {
         UInt16 result = 0;

         if (1 == pdo)
         {
            result = 0x1400;
         }
         else if (2 == pdo)
         {
            result = 0x1401;
         }
         else if (3 == pdo)
         {
            result = 0x1402;
         }
         else if (4 == pdo)
         {
            result = 0x1403;
         }

         return (result);
      }

      protected UInt16 GetRPDOMapAddress(int pdo)
      {
         UInt16 result = 0;

         if (1 == pdo)
         {
            result = 0x1600;
         }
         else if (2 == pdo)
         {
            result = 0x1601;
         }
         else if (3 == pdo)
         {
            result = 0x1602;
         }
         else if (4 == pdo)
         {
            result = 0x1603;
         }

         return (result);
      }

      private bool Transmit(int cobId, byte[] data)
      {
         bool result = false;

         if (null != this.OnTransmit)
         {
            result = this.OnTransmit(cobId, data);

            if (false != result)
            {
               this.TraceTransmit(cobId, data);
            }
         }

         return (result);
      }

      protected void SignalCommError()
      {
         if (null != this.OnCommError)
         {
            this.OnCommError(this.Name, this.NodeId, this.faultReason);
         }
      }

      protected void ScheduleAction(CommAction action)
      {
         if (CommStates.error != this.commState)
         {
            lock (this)
            {
               this.actionQueue.Enqueue(action);
            }

            // this.Update(); todo how to determine calling context?
         }
      }

      protected void ScheduleAction(CommAction action, int timeout, int attemptLimit)
      {
         if (CommStates.error != this.commState)
         {
            lock (this)
            {
               action.RetryTime = timeout;
               action.RetryAttemptLimit = attemptLimit;
               this.actionQueue.Enqueue(action);
            }

            // this.Update(); todo how to determine calling context?
         }
      }

      protected bool ExchangeCommAction(CommAction action, int timeout = 200, int attemptLimit = 2)
      {
         bool result = false;

         if (null == this.FaultReason)
         {
            this.pendingAction = action;
            this.ScheduleAction(action, timeout, attemptLimit);
            int exchangeLimit = (timeout * attemptLimit) + 50;
            result = this.commEvent.WaitOne(exchangeLimit);

            if (false != result)
            {
               if (false != action.Aborted)
               {
                  result = false;
               }
            }
            else
            {
               Tracer.WriteHigh(TraceGroup.CANBUS, "", "exchange failure");
            }
         }

         return (result);
      }

      protected bool ReadSDO(UInt16 index, byte subIndex, ref UInt32 value)
      {
         bool result = false;

         if (null == this.FaultReason)
         {
            SDOUpload upload = new SDOUpload(index, subIndex);
            result = this.ExchangeCommAction(upload);

            if ((false != result) &&
                (null != upload.Data) &&
                (4 >= upload.Data.Length))
            {
               value = BitConverter.ToUInt32(upload.Data, 0);
               result = true;
            }
            else
            {
               result = false;
            }
         }

         return (result);
      }

      #region TPDO Map Functions

      /// <summary>
      /// Sets TPDO mapping count.  Functional while in device pre-operational state.
      /// </summary>
      /// <param name="pdo">TPDO to set, 1..4</param>
      /// <param name="count">number of mappings</param>
      /// <returns>true when successful</returns>
      protected bool SetTPDOMapCount(byte pdo, UInt32 count)
      {
         UInt16 mapIndex = this.GetTPDOMapAddress(pdo);
         this.pendingAction = new SDODownload(mapIndex, 0, 1, count);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      /// <summary>
      /// Sets TPDO mapping.  Functional while in device pre-operational state.
      /// </summary>
      /// <param name="pdo">TPDO to set, 1..4</param>
      /// <param name="map">map to set, 1..8</param>
      /// <param name="index">index to map</param>
      /// <param name="subIndex">sub index to to map</param>
      /// <param name="octetCount">number of bytes to map</param>
      /// <returns>true when successful</returns>
      protected bool SetTPDOMap(byte pdo, byte map, UInt16 index, byte subIndex, byte octetCount)
      {
         UInt16 mapIndex = this.GetTPDOMapAddress(pdo);
         UInt32 mapDefinition = (UInt32)((index << 16) | (subIndex << 8) | (octetCount * 8));

         this.pendingAction = new SDODownload(mapIndex, map, 4, mapDefinition);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      /// <summary>
      /// Sets TPDO enable.  Functional while in device pre-operational state.
      /// </summary>
      /// <param name="pdo">TPDO to set, 1..4</param>
      /// <param name="enabled">boolean, true to enable, false to disable</param>
      /// <returns>true when successful</returns>
      protected bool SetTPDOEnable(byte pdo, bool enabled)
      {
         bool result = false;

         if ((pdo >= 1) && (pdo <= 4))
         {
            UInt32 cobId = (UInt32)((pdo * 0x100) + 0x80 + this.NodeId);

            if (false == enabled)
            {
               cobId += 0x80000000;
            }

            UInt16 index = this.GetTPDOParameterAddress(pdo);
            this.pendingAction = new SDODownload(index, 1, 4, cobId);
            result = this.ExchangeCommAction(this.pendingAction);
         }

         return (result);
      }

      /// <summary>
      /// Sets TPDO transmission type.  Functional while in device pre-operational state.
      /// </summary>
      /// <param name="pdo">TPDO to set, 1..4</param>
      /// <param name="typeValue">value to program into transmission type object</param>
      /// <returns>true when successful</returns>
      protected bool SetTPDOType(byte pdo, byte typeValue)
      {
         UInt16 index = this.GetTPDOParameterAddress(pdo);

         this.pendingAction = new SDODownload(index, 2, 1, typeValue);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      /// <summary>
      /// Sets TPDO inhibit time.  Functional while in device pre-operational state.
      /// </summary>
      /// <param name="pdo">TPDO to set, 1..4</param>
      /// <param name="mS">number of milliseconds required to pass before another TPDO transmission can occur</param>
      /// <returns>true when successful</returns>
      protected bool SetTPDOInhibitTime(byte pdo, UInt32 mS)
      {
         UInt16 index = this.GetTPDOParameterAddress(pdo);

         this.pendingAction = new SDODownload(index, 3, 2, mS*10);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      /// <summary>
      /// Sets TPDO event time.  Functional while in device pre-operational state.
      /// </summary>
      /// <param name="pdo">TPDO to set, 1..4</param>
      /// <param name="mS">number of milliseconds required for time event</param>
      /// <returns>true when successful</returns>
      protected bool SetTPDOEventTime(byte pdo, UInt32 mS)
      {
         UInt16 index = this.GetTPDOParameterAddress(pdo);

         this.pendingAction = new SDODownload(index, 5, 2, mS);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      #endregion

      #region RPDO Map Functions

      /// <summary>
      /// Sets RPDO mapping count.  Functional while in device pre-operational state.
      /// </summary>
      /// <param name="pdo">RPDO to set, 1..4</param>
      /// <param name="count">number of mappings</param>
      /// <returns>true when successful</returns>
      protected bool SetRPDOMapCount(byte pdo, UInt32 count)
      {
         UInt16 mapIndex = this.GetRPDOMapAddress(pdo);
         this.pendingAction = new SDODownload(mapIndex, 0, 1, count);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      /// <summary>
      /// Sets RPDO mapping.  Functional while in device pre-operational state.
      /// </summary>
      /// <param name="pdo">RPDO to set, 1..4</param>
      /// <param name="map">map to set, 1..8</param>
      /// <param name="index">index to map</param>
      /// <param name="subIndex">sub index to to map</param>
      /// <param name="octetCount">number of bytes to map</param>
      /// <returns>true when successful</returns>
      protected bool SetRPDOMap(byte pdo, byte map, UInt16 index, byte subIndex, byte octetCount)
      {
         UInt16 mapIndex = this.GetRPDOMapAddress(pdo);
         UInt32 mapDefinition = (UInt32)((index << 16) | (subIndex << 8) | (octetCount * 8));

         this.pendingAction = new SDODownload(mapIndex, map, 4, mapDefinition);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

#if false
      /// <summary>
      /// Sets RPDO connection object id.  Functional while in device pre-operational state.
      /// </summary>
      /// <param name="pdo">TPDO to set, 1..4</param>
      /// <param name="typeValue">value to program into transmission type object</param>
      /// <returns>true when successful</returns>
      protected bool SetRPDOCobId(byte pdo, UInt32 cobId)
      {
         UInt16 index = this.GetRPDOParameterAddress(pdo);

         this.pendingAction = new SDODownload(index, 1, 4, cobId);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }
#endif

      /// <summary>
      /// Sets RPDO eanble.  Functional while in device pre-operational state.
      /// </summary>
      /// <param name="pdo">RPDO to set, 1..4</param>
      /// <param name="enabled">boolean, true to enable, false to disable</param>
      /// <returns>true when successful</returns>
      protected bool SetRPDOEnable(byte pdo, bool enabled)
      {
         bool result = false;

         if ((pdo >= 1) && (pdo <= 4))
         {
            UInt32 cobId = (UInt32)((pdo * 0x100) + 0x100 + this.NodeId);

            if (false == enabled)
            {
               cobId += 0x80000000;
            }

            UInt16 index = this.GetRPDOParameterAddress(pdo);
            this.pendingAction = new SDODownload(index, 1, 4, cobId);
            result = this.ExchangeCommAction(this.pendingAction);
         }

         return (result);
      }

      /// <summary>
      /// Sets RPDO transmission type.  Functional while in device pre-operational state.
      /// </summary>
      /// <param name="pdo">TPDO to set, 1..4</param>
      /// <param name="typeValue">value to program into transmission type object</param>
      /// <returns>true when successful</returns>
      protected bool SetRPDOType(byte pdo, byte typeValue)
      {
         UInt16 index = this.GetRPDOParameterAddress(pdo);

         this.pendingAction = new SDODownload(index, 2, 1, typeValue);
         bool result = this.ExchangeCommAction(this.pendingAction);
         return (result);
      }

      #endregion

      /// <summary>
      /// Function defined to allow process of received messages from the derived objects.
      /// Base object processing done within update method. 
      /// </summary>
      /// <param name="cobId">COD-ID of message</param>
      /// <param name="msg">message bytes</param>
      protected virtual void EvaluateMessage(int cobId, byte[] msg)
      {
      }

      protected virtual void EvaluateAction(CommAction action)
      {
         if (this.pendingAction == action)
         {
            this.pendingAction = null;
            this.commEvent.Set();
         }
      }

      #endregion

      #region Constructor

      public Device(string name, byte nodeId)
      {
         this.name = name;
         this.NodeId = nodeId;

         this.actionQueue = new Queue();
         this.commEvent = new AutoResetEvent(false);

         this.Initialize();
      }

      #endregion

      #region Access Methods

      public virtual void Initialize()
      {
         this.faultReason = null;

         this.receiveBootupHeartbeart = false;
         this.consumerHeartbeatActive = false;
         this.consumerHeartbeatSetting = 0;

         this.actionQueue.Clear();
         this.commState = CommStates.idle;

         this.TransmitFailureHoldoffTime = 50;
      }

      public virtual bool SDODownload(UInt16 index, byte subIndex, byte[] data)
      {
         SDODownload download = new SDODownload(index, subIndex, data, 0, data.Length);

         this.pendingAction = download;
         this.ScheduleAction(this.pendingAction);

         this.commEvent.Reset();
         bool actionResult = this.commEvent.WaitOne(500);

         return (actionResult);
      }

      public virtual byte[] SDOUpLoad(UInt16 index, byte subIndex)
      {
         byte[] result = null;
         SDOUpload upload = new SDOUpload(index, subIndex);

         this.pendingAction = upload;
         this.ScheduleAction(this.pendingAction);

         this.commEvent.Reset();
         bool actionResult = this.commEvent.WaitOne(500);

         if (false != actionResult)
         {
            result = upload.Data;
         }

         return (result);
      }

      /// <summary>
      /// Function to setup device before start.
      /// </summary>
      /// <returns></returns>
      public virtual bool Configure()
      {
         bool result = true;

         // get device type
         UInt32 deviceType = 0;
         result &= this.ReadDeviceType(ref deviceType);
         this.DeviceType = deviceType;

         // get device name
         string deviceName = string.Empty;
         result &= this.ReadDeviceName(ref deviceName);
         this.DeviceName = deviceName;

         // get device version
         string deviceVersion = string.Empty;
         result &= this.ReadDeviceVersion(ref deviceVersion);
         this.DeviceVersion = deviceVersion;

         return (result);
      }

      public virtual bool Start()
      {
         NetworkRequest networkRequest = new NetworkRequest(0x01, this.NodeId);
         this.ScheduleAction(networkRequest);
         return (true);
      }

      public virtual void Stop()
      {
         NetworkRequest networkRequest = new NetworkRequest(0x02, this.NodeId);
         this.ScheduleAction(networkRequest);
      }

      public virtual void Reset()
      {
         this.receiveBootupHeartbeart = false;
         this.faultReason = null;
         this.actionQueue.Clear();
         this.commState = CommStates.idle;

         this.consumerHeartbeatActive = false;
         this.consumerHeartbeatSetting = 0;

         NetworkRequest networkRequest = new NetworkRequest(0x81, this.NodeId);
         this.ScheduleAction(networkRequest);
      }

      public virtual void Fault(string faultReason)
      {
         this.faultReason = faultReason;
         this.commState = CommStates.error;
         this.actionQueue.Clear();
         this.SignalCommError();
         this.Stop();
      }

      public bool Read(UInt16 index, byte subIndex, ref UInt16 value)
      {
         bool result = true;

         SDOUpload upload = new SDOUpload(index, subIndex);
         result &= this.ExchangeCommAction(upload);

         if ((false != result) &&
             (null != upload.Data) &&
             (2 >= upload.Data.Length))
         {
            value = BitConverter.ToUInt16(upload.Data, 0);
         }

         return (result);
      }
      public bool ReadDeviceType(ref UInt32 deviceType)
      {
         bool result = true;

         SDOUpload upload = new SDOUpload(0x1000, 0);
         result &= this.ExchangeCommAction(upload);

         if ((false != result) &&
             (null != upload.Data) &&
             (4 >= upload.Data.Length))
         {
            deviceType = BitConverter.ToUInt32(upload.Data, 0);
         }

         return (result);
      }

      public bool ReadDeviceName(ref string deviceName)
      {
         bool result = true;

         SDOUpload upload = new SDOUpload(0x1008, 0);
         result &= this.ExchangeCommAction(upload);

         if ((false != result) &&
             (null != upload.Data))
         {
            deviceName = Encoding.UTF8.GetString(upload.Data);
         }

         return (result);
      }

      public bool ReadDeviceVersion(ref string deviceVersion)
      {
         bool result = true;

         SDOUpload upload = new SDOUpload(0x100A, 0);
         result &= this.ExchangeCommAction(upload);

         if ((false != result) &&
             (null != upload.Data))
         {
            deviceVersion = Encoding.UTF8.GetString(upload.Data);
         }

         return (result);
      }

      public virtual bool SetConsumerHeartbeat(UInt16 milliseconds, byte nodeId)
      {
         this.consumerHeartbeatSetting = milliseconds;
         this.consumerHeartbeatTimeLimit = DateTime.Now.AddMilliseconds(milliseconds);
         bool result = true;

         if (0 != this.consumerHeartbeatSetting)
         {
            UInt32 control = (UInt32)((nodeId << 16) | milliseconds);
            result &= this.ExchangeCommAction(new SDODownload(0x1016, 1, 4, control));
         }

         return (result);
      }

      public bool SetProducerHeartbeat(UInt16 milliseconds)
      {
         bool result = true;
         result &= this.ExchangeCommAction(new SDODownload(0x1017, 0, 2, (UInt16)milliseconds));
         return (result);
      }

      private void TraceReceive(COBTypes frameType, int cobId, byte[] msg)
      {
         bool trace = false;

         if ((COBTypes.NMT == frameType) ||
             (COBTypes.EMGY == frameType) ||
             (COBTypes.TS == frameType)) 
         {
            trace = true;
         }
         else if (COBTypes.ERROR == frameType)
         {
            trace = this.TraceHB;
         }
         else if (COBTypes.TSDO == frameType)
         {
            trace = this.TraceSDO;
         }
         else if (COBTypes.TPDO1 == frameType) 
         {
            trace = this.TraceTPDO1;
         }
         else if (COBTypes.TPDO2 == frameType) 
         {
            trace = this.TraceTPDO2;
         }
         else if (COBTypes.TPDO3 == frameType) 
         {
            trace = this.TraceTPDO3;
         }
         else if (COBTypes.TPDO4 == frameType) 
         {
            trace = this.TraceTPDO4;
         }

         if (false != trace)
         {
            if (null != this.OnReceiveTrace)
            {
               this.OnReceiveTrace(cobId, msg);
            }
         }
      }

      private void TraceTransmit(int cobId, byte[] msg)
      {
         COBTypes frameType = (COBTypes)((cobId >> 7) & 0xF);
         bool trace = false;

         if (COBTypes.RSDO == frameType)
         {
            trace = this.TraceSDO;
         }
         else if (COBTypes.RPDO1 == frameType)
         {
            trace = this.TraceRPDO1;
         }
         else if (COBTypes.RPDO2 == frameType)
         {
            trace = this.TraceRPDO2;
         }
         else if (COBTypes.RPDO3 == frameType)
         {
            trace = this.TraceRPDO3;
         }
         else if (COBTypes.RPDO4 == frameType)
         {
            trace = this.TraceRPDO4;
         }

         if (false != trace)
         {
            if (null != this.OnTransmitTrace)
            {
               this.OnTransmitTrace(cobId, msg);
            }
         }
      }

      public bool Update(int cobId, byte[] msg)
      {
         bool result = false;
         COBTypes frameType = (COBTypes)((cobId >> 7) & 0xF);
         int nodeId = (int)(cobId & 0x7F);

         if (nodeId == this.NodeId)
         {
            this.TraceReceive(frameType, cobId, msg);
            result = true;
         }

         if (CommStates.error != this.commState)
         {
            if ((0 == nodeId) || (nodeId == this.NodeId))
            {
               if (COBTypes.ERROR == frameType) 
               {
                  if ((msg.Length > 0) && (0 == msg[0]))
                  {
                     this.receiveBootupHeartbeart = true;
                  }

                  if (0 != this.consumerHeartbeatSetting)
                  {
                     this.consumerHeartbeatTimeLimit = DateTime.Now.AddMilliseconds(this.consumerHeartbeatSetting);
                     this.consumerHeartbeatActive = true;
                  }
               }

               this.EvaluateMessage(cobId, msg);

               if (null != this.action)
               {
                  this.action.Process(cobId, msg);
               }

               this.Update();
            }
         }

         return (result);
      }

      public virtual void Update()
      {
         CommStates processedState;

         do 
         {
            processedState = this.commState;

            switch (this.commState)
            {
               case CommStates.idle:
               {
                  lock (this)
                  {
                     if (this.actionQueue.Count != 0)
                     {
                        this.action = (CommAction)this.actionQueue.Dequeue();
                     }
                  }

                  if (null != this.action)
                  {
                     this.commAttemptCount = 0;
                     this.commState = CommStates.tx;
                     //Tracer.WriteHigh(TraceGroup.CANBUS, "", "action start");
                  }

                  break;
               }
               case CommStates.tx:
               {
                  this.commAttemptCount++;

                  if (this.commAttemptCount <= this.action.RetryAttemptLimit)
                  {
                     int cobId = 0;
                     byte[] frameData = this.action.GetTransmitFrame(ref cobId, this.NodeId);

                     if (null != frameData)
                     {
                        bool txResult = this.Transmit(cobId, frameData);

                        if (false != txResult)
                        {
                           this.action.TransmitComplete();

                           bool responseNeeded = this.action.ResponseNeeded();

                           if (false != responseNeeded)
                           {
                              //Tracer.WriteHigh(TraceGroup.CANBUS, "", "tx pending");
                              this.commTimeLimit = DateTime.Now.AddMilliseconds(this.action.RetryTime);
                              this.commState = CommStates.rxWait;
                           }
                           else
                           {
                              this.EvaluateAction(this.action);
                              this.action = null;
                              this.commState = CommStates.idle;
                           }
                        }
                        else
                        {
                           Tracer.WriteHigh(TraceGroup.CANBUS, "", "tx fail");
                           this.commTimeLimit = DateTime.Now.AddMilliseconds(this.TransmitFailureHoldoffTime);
                           this.commState = CommStates.txWait;
                        }
                     }
                     else
                     {
                        this.action = null;
                        this.commState = CommStates.idle;
                     }
                  }
                  else
                  {
                     this.Fault("comm failure");
                     this.action = null;
                  }

                  break;
               }
               case CommStates.txWait:
               {
                  if (DateTime.Now > this.commTimeLimit)
                  {
                     this.commState = CommStates.tx;
                  }

                  break;
               }
               case CommStates.rxWait:
               {
                  if (false != this.action.Done)
                  {
                     this.EvaluateAction(this.action);

                     if (false != this.action.Aborted)
                     {
                        this.Fault("comm abort");
                     }

                     this.action = null;
                     this.commState = CommStates.idle;
                  }
                  else if (false != this.action.Transmit)
                  {
                     this.commAttemptCount = 0;
                     this.commState = CommStates.tx;
                  }
                  else if (DateTime.Now > this.commTimeLimit)
                  {
                     this.commState = CommStates.tx;
                     Tracer.WriteHigh(TraceGroup.CANBUS, "", "response timeout");
                  }

                  break;
               }
               case CommStates.error:
               {
                  break;
               }                
            }

         }
         while (processedState != this.commState);

         if (null == this.FaultReason)
         {
            if ((false != this.consumerHeartbeatActive) &&
                (DateTime.Now > this.consumerHeartbeatTimeLimit))
            {
               this.Fault("heartbeat missing");
            }
         }
      }

      public virtual void UploadLastTimeStampCorrection()
      {
      }

      public virtual void UploadLastSyncTime()
      {
      }

      #endregion
   }

}