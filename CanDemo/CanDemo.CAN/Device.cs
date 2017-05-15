
namespace CanDemo.CAN
{
   using System;
   using System.Collections;
   using System.IO;
   using System.Text;
   using System.Threading;

   using CanDemo.Utilities;

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

      private enum DownloadSteps
      {
         start,
         waitReset,
         waitBootStart,
         waitFirmwareDownload,
         waitStartRequest,
      }

      public delegate void ReceiveTraceHandler(int id, byte[] data);
      public delegate void TransmitTraceHandler(int id, byte[] data);
      public delegate bool TransmitHandler(int id, byte[] data);
      public delegate void FaultHandler(string name, int nodeId, string reason);
      public delegate void WarningHandler(string name, int nodeId, string reason);
      public delegate void ImageDownloadCompleteHandler(string result);

      #endregion

      #region Fields

      private string name;
     // private byte nodeId;

      private string faultReason;
      private string warningString;

      private Queue actionQueue;
      private CommStates commState;
      private DateTime commTimeLimit;

      private CommAction action;

      private int commAttemptCount;

      protected AutoResetEvent commEvent;
      protected CommAction pendingAction;

      private bool faultResetEnabled;
      private bool faultResetNeeded;
      private bool faultResetActive;
      private DateTime faultResetTimeLimit;
      private int faultResetAttemptCount;
      private int faultResetAttempLimit;

      private bool downloadActive;
      private bool downloadCancel;
      private DownloadSteps downloadStep;
      private CommAction downloadAction;
      private string downloadFile;
      private ImageDownloadCompleteHandler downloadCompleteHandler;
      private DateTime downloadTimeLimit;
      private byte[] downloadData;
      private UInt32 downloadImageSize;
      private UInt32 downloadImagePosition;

      protected bool started;
      protected bool receiveBootupHeartbeart;
      private bool consumerHeartbeatActive;
      private int consumerHeartbeatSetting;
      private DateTime consumerHeartbeatTimeLimit;
      private int producerHeartbeatSetting;

      private bool customComTime;
      private int customComTimeLimit;

      #endregion

      #region Properties

      public ReceiveTraceHandler OnReceiveTrace { set; get; }
      public TransmitTraceHandler OnTransmitTrace { set; get; }
      public TransmitHandler OnTransmit { set; get; }
      public FaultHandler OnFault { set; get; }
      public WarningHandler OnWarning { set; get; }

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
      public string Warning { get { return (this.warningString); } }

      public int TransmitFailureHoldoffTime { set; get; }

      #endregion

      #region Helper Functions

      private void SendRawReset()
      {
         byte[] frameData = new byte[2];

         frameData[0] = 0x81;
         frameData[1] = this.NodeId;

         this.Transmit(0, frameData);
      }

      protected UInt16 GetTPDOParameterAddress(int pdo)
      {
         UInt16 result = (UInt16)(0x1800 + (pdo - 1));
#if false
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
#endif

         return (result);
      }

      protected UInt16 GetTPDOMapAddress(int pdo)
      {
         UInt16 result = (UInt16)(0x1A00 + (pdo - 1));

#if false
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
#endif

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

      protected void SignalFault()
      {
         if (null != this.OnFault)
         {
            this.OnFault(this.Name, this.NodeId, this.faultReason);
         }
      }

      protected void SignalWarning()
      {
         if (null != this.OnWarning)
         {
            this.OnWarning(this.Name, this.NodeId, this.warningString);
         }
      }

      protected bool ScheduleAction(CommAction action)
      {
         bool result = false;

         if (CommStates.error != this.commState)
         {
            lock (this)
            {
               this.actionQueue.Enqueue(action);
               result = true;
            }

            // this.Update(); todo how to determine calling context?
         }

         return (result);
      }

      protected bool ScheduleAction(CommAction action, int timeout, int attemptLimit)
      {
         bool result = false;

         if (CommStates.error != this.commState)
         {
            lock (this)
            {
               action.RetryTime = timeout;
               action.RetryAttemptLimit = attemptLimit;
               this.actionQueue.Enqueue(action);
               result = true;
            }

            // this.Update(); todo how to determine calling context?
         }

         return (result);
      }

      protected bool ExchangeCommAction(CommAction action, int timeout = 200, int attemptLimit = 2)
      {
         bool result = false;

         if (null == this.FaultReason)
         {
            this.pendingAction = action;
            this.ScheduleAction(action, timeout, attemptLimit);
            int exchangeLimit = (timeout * attemptLimit) + 50;

            if (false != this.customComTime)
            {
               exchangeLimit = (this.customComTimeLimit * attemptLimit) + 50;
            }

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

      protected bool ReadSDO(UInt16 index, byte subIndex, ref UInt16 value)
      {
         bool result = false;

         if (null == this.FaultReason)
         {
            SDOUpload upload = new SDOUpload(index, subIndex);
            result = this.ExchangeCommAction(upload);

            if ((false != result) &&
                (null != upload.Data) &&
                (2 <= upload.Data.Length))
            {
               value = BitConverter.ToUInt16(upload.Data, 0);
               result = true;
            }
            else
            {
               result = false;
            }
         }

         return (result);
      }

      protected bool ReadSDO(UInt16 index, byte subIndex, ref Int32 value)
      {
         bool result = false;

         if (null == this.FaultReason)
         {
            SDOUpload upload = new SDOUpload(index, subIndex);
            result = this.ExchangeCommAction(upload);

            if ((false != result) &&
                (null != upload.Data) &&
                (4 <= upload.Data.Length))
            {
               value = BitConverter.ToInt32(upload.Data, 0);
               result = true;
            }
            else
            {
               result = false;
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
                (4 <= upload.Data.Length))
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
      /// <param name="pdo">TPDO to set, 1..511</param>
      /// <param name="enabled">boolean, true to enable, false to disable</param>
      /// <returns>true when successful</returns>
      protected bool SetTPDOEnable(int pdo, bool enabled)
      {
         bool result = false;

         if ((pdo > 0) && (pdo <= 511))
         {
            int pdoOffset = ((pdo - 1) % 4) + 1;
            int pdoNodeOffset = (pdo - 1) / 4;

            UInt32 cobId = (UInt32)((pdoOffset * 0x100) + 0x80 + (this.NodeId + pdoNodeOffset));

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
      /// <param name="pdo">RPDO to set, 1..511</param>
      /// <param name="enabled">boolean, true to enable, false to disable</param>
      /// <returns>true when successful</returns>
      protected bool SetRPDOEnable(int pdo, bool enabled)
      {
         bool result = false;

         if ((pdo > 0) && (pdo <= 511))
         {
            int pdoOffset = ((pdo - 1) % 4) + 1;
            int pdoNodeOffset = (pdo - 1) / 4;

            UInt32 cobId = (UInt32)((pdoOffset * 0x100) + 0x100 + (this.NodeId + pdoNodeOffset));

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
         COBTypes frameType = (COBTypes)((cobId >> 7) & 0xF);
         int nodeId = (int)(cobId & 0x7F);

         if (nodeId == this.NodeId)
         {
            if (COBTypes.EMGY == frameType)
            {
               UInt64 errorCode = 0;
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

                  errorCode = BitConverter.ToUInt64(errorMsg, 0);
               }

               if (0 != errorCode)
               {
                  string faultReason = string.Format("emergency {0:X16}", errorCode);
                  this.SetFault(faultReason, false);
               }
            }
         }
      }

      protected virtual void EvaluateAction(CommAction action)
      {
         if (this.pendingAction == action)
         {
            this.pendingAction = null;
            this.commEvent.Set();
         }
         else if (this.downloadAction == action)
         {
            this.downloadAction = null;
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
         this.DeviceType = 0;
         this.DeviceName = "";
         this.DeviceVersion = "";
         this.faultReason = null;
         this.warningString = null;

         this.started = false;
         this.receiveBootupHeartbeart = false;
         this.consumerHeartbeatActive = false;
         this.consumerHeartbeatSetting = 0;

         this.actionQueue.Clear();
         this.commState = CommStates.idle;

         this.faultResetEnabled = true;
         this.faultResetNeeded = false;
         this.faultResetActive = false;

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
         this.started = true;
         return (true);
      }

      public virtual void Stop()
      {
         NetworkRequest networkRequest = new NetworkRequest(0x02, this.NodeId);
         this.ScheduleAction(networkRequest);
      }

      public virtual void Reset()
      {
         this.faultResetEnabled = true;
         this.faultResetNeeded = false;
         this.faultResetActive = false;

         this.started = false;
         this.receiveBootupHeartbeart = false;
         this.faultReason = null;
         this.warningString = null;
         this.actionQueue.Clear();
         this.commState = CommStates.idle;

         this.consumerHeartbeatActive = false;
         this.consumerHeartbeatSetting = 0;

         this.customComTime = false;
         this.customComTimeLimit = 0;

         NetworkRequest networkRequest = new NetworkRequest(0x81, this.NodeId);
         this.ScheduleAction(networkRequest);
      }

      public virtual void SetFault(string faultReason, bool resetDevice)
      {
         this.faultReason = faultReason;
         this.commState = CommStates.error;
         this.actionQueue.Clear();
         this.SignalFault();

         if (false != this.faultResetEnabled)
         {
            this.faultResetNeeded = resetDevice;
         }
      }

      public virtual void ClearFault()
      {
         this.faultReason = null;
         this.actionQueue.Clear();
         this.commState = CommStates.idle;
      }

      public virtual void SetWarning(string warningString)
      {
         this.warningString = warningString;
         this.SignalWarning();
      }

      public virtual void ClearWarning()
      {
         this.warningString = null;
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
         this.producerHeartbeatSetting = milliseconds;

         bool result = true;
         result &= this.ExchangeCommAction(new SDODownload(0x1017, 0, 2, (UInt16)milliseconds));
         return (result);
      }

      public bool DownloadImage(string file, ImageDownloadCompleteHandler onComplete)
      {
         bool result = false;

         if ((null == this.FaultReason) && (false == this.downloadActive))
         {
            this.downloadFile = file;
            this.downloadCompleteHandler = onComplete;
            this.downloadCancel = false;
            this.downloadActive = true;
            this.downloadStep = DownloadSteps.start;

            result = true;
         }

         return (result);
      }

      public void StopImageDownload()
      {
         this.downloadCancel = true;
      }

      public void GetImageDownloadStatus(ref UInt32 imageSize, ref UInt32 downloadPosition)
      {
         if (false != this.downloadActive)
         {
            imageSize = this.downloadImageSize;
            downloadPosition = this.downloadImagePosition;
         }
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
            byte msgCode = msg[0];

            if ((0x7F == msgCode) || (0x5 == msgCode) || (0x4 == msgCode))
            {
               trace = this.TraceHB;
            }
            else
            {
               trace = true;
            }
         }
         else if (COBTypes.TSDO == frameType)
         {
            if (DownloadSteps.waitFirmwareDownload != this.downloadStep)
            {
               trace = this.TraceSDO;
            }
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

         if ((COBTypes.NMT == frameType) ||
             (COBTypes.EMGY == frameType) ||
             (COBTypes.TS == frameType))
         {
            trace = true;
         }
         else if (COBTypes.RSDO == frameType)
         {
            if (DownloadSteps.waitFirmwareDownload != this.downloadStep)
            {
               trace = this.TraceSDO;
            }
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

      public void Update(int cobId, byte[] msg, ref bool traced)
      {
         COBTypes frameType = (COBTypes)((cobId >> 7) & 0xF);
         int nodeId = (int)(cobId & 0x7F);

         if ((nodeId == this.NodeId) && (false == traced))
         {
            this.TraceReceive(frameType, cobId, msg);
            traced = true;
         }

         if ((0 == nodeId) || (nodeId == this.NodeId))
         {
            if (COBTypes.ERROR == frameType)
            {
               if ((msg.Length > 0) && (0 == msg[0]))
               {
                  this.receiveBootupHeartbeart = true;

                  if ((null == this.FaultReason) &&
                      (false != this.started))
                  {
                     this.SetFault("watchdog reset", false);
                  }
               }

               if (CommStates.error != this.commState)
               {
                  if (0 != this.consumerHeartbeatSetting)
                  {
                     this.consumerHeartbeatTimeLimit = DateTime.Now.AddMilliseconds(this.consumerHeartbeatSetting);
                     this.consumerHeartbeatActive = true;
                  }
               }
            }
         }

         this.EvaluateMessage(cobId, msg);

         if ((0 == nodeId) || (nodeId == this.NodeId))
         {
            if (CommStates.error != this.commState)
            {
               if (null != this.action)
               {
                  this.action.Process(cobId, msg);
               }

               this.Update();
            }
         }
      }

      public virtual void Update()
      {
         CommStates processedState;

         #region Communication Process

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
                              double retryTime = this.action.RetryTime;

                              if (false != this.customComTime)
                              {
                                 retryTime = this.customComTimeLimit;
                              }

                              //Tracer.WriteHigh(TraceGroup.CANBUS, "", "tx pending");
                              this.commTimeLimit = DateTime.Now.AddMilliseconds(retryTime);
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
                     if (("MAIN" == this.DeviceName) ||
                         ("POSITIONER" == this.DeviceName) ||
                         ("WHEEL" == this.DeviceName))
                     {
                        this.commState = CommStates.error;
                        this.SetWarning("comm failure");
                     }
                     else
                     {
                        this.SetFault("comm failure", true);
                     }

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
                        this.SetFault("comm abort", true);
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
               if (("MAIN" == this.DeviceName) ||
                   ("POSITIONER" == this.DeviceName) || 
                   ("WHEEL" == this.DeviceName))
               {
                  if (null == this.Warning)
                  {
                     this.SetWarning("heartbeat missing");
                  }
               }
               else
               {
                  this.SetFault("heartbeat missing", true);
               }
            }
         }

         #endregion

         #region Reset Process

         if (false == this.faultResetActive)
         {
            if (false != this.faultResetNeeded)
            {
               this.faultResetNeeded = false;
               this.faultResetActive = true;
               this.receiveBootupHeartbeart = false;

               this.faultResetTimeLimit = DateTime.Now.AddMilliseconds(500);
               this.faultResetAttemptCount = 1;

               if ((0 == this.producerHeartbeatSetting) ||
                   (0 == this.consumerHeartbeatSetting))
               {
                  this.faultResetAttempLimit = 3;
               }
               else
               {
                  this.faultResetAttempLimit = (this.consumerHeartbeatSetting / this.producerHeartbeatSetting);
               }

               if (this.faultResetAttempLimit < 3)
               {
                  this.faultResetAttempLimit = 3;
               }

               this.SendRawReset();
            }
         }
         else if (false != this.receiveBootupHeartbeart)
         {
            this.faultResetActive = false;
         }
         else if ((false != this.faultResetActive) &&
                  (DateTime.Now > this.faultResetTimeLimit))
         {
            if (this.faultResetAttemptCount < this.faultResetAttempLimit)
            {
               this.SendRawReset();
               this.faultResetAttemptCount++;
               this.faultResetTimeLimit = DateTime.Now.AddMilliseconds(500);
            }
            else
            {
               this.faultResetActive = false;
            }
         }

         #endregion

         #region Download Process

         if (false != this.downloadActive)
         {
            string downloadResult = null;
            bool downloadComplete = false;
            
            if (null != this.FaultReason)
            {
               downloadComplete = true;
               downloadResult = "Device faulted: " + this.FaultReason;
            }
            else if (false != this.downloadCancel)
            {
               downloadComplete = true;
               downloadResult = "Cancelled";
            }
            else if (DownloadSteps.start == this.downloadStep)
            {
               if (File.Exists(this.downloadFile) != false)
               {
                  FileStream fs = File.Open(this.downloadFile, FileMode.Open);
                  BinaryReader br = new BinaryReader(fs);

                  this.downloadImagePosition = 0;
                  this.downloadImageSize = (UInt32)fs.Length;
                  this.downloadData = br.ReadBytes((int)this.downloadImageSize);

                  br.Close();
                  br.Dispose();

                  fs.Close();
                  fs.Dispose();

                  this.receiveBootupHeartbeart = false;
                  this.downloadAction = new SDODownload(0x1F51, 1, 1, 0);
                  bool result = this.ScheduleAction(this.downloadAction, 200, 1);

                  if (false != result)
                  {
                     this.downloadTimeLimit = DateTime.Now.AddMilliseconds(700);
                     this.downloadStep = DownloadSteps.waitReset;
                  }
                  else
                  {
                     downloadResult = "Unable to schedule reset.";
                     downloadComplete = true;
                  }
               }
               else
               {
                  downloadResult = "Unable to open file.";
                  downloadComplete = true;
               }
            }
            else if (DownloadSteps.waitReset == this.downloadStep)
            {
               if ((null == this.downloadAction) && (false != this.receiveBootupHeartbeart))
               {
                  this.downloadTimeLimit = DateTime.Now.AddMilliseconds(500);
                  this.downloadStep = DownloadSteps.waitBootStart;
               }
               else if (DateTime.Now > this.downloadTimeLimit)
               {
                  this.downloadTimeLimit = DateTime.Now.AddMilliseconds(500);
                  this.downloadStep = DownloadSteps.waitBootStart;
               }
            }
            else if (DownloadSteps.waitBootStart == this.downloadStep)
            {
               if (DateTime.Now > this.downloadTimeLimit)
               {
                  this.downloadAction = new SDODownload(0x1F50, 1, this.downloadData, 0, this.downloadData.Length);
                  bool result = this.ScheduleAction(this.downloadAction, 5000, 1);

                  if (false != result)
                  {
                     this.downloadStep = DownloadSteps.waitFirmwareDownload;
                  }
                  else
                  {
                     downloadResult = "Unable to schedule download.";
                     downloadComplete = true;
                  }
               }
            }
            else if (DownloadSteps.waitFirmwareDownload == this.downloadStep)
            {
               if (null != this.downloadAction)
               {
                  this.downloadImagePosition = ((SDODownload)this.downloadAction).SentCount;
               }
               else 
               {
                  this.downloadImagePosition = this.downloadImageSize;
                  this.downloadAction = new SDODownload(0x1F51, 1, 1, 1);
                  bool result = this.ScheduleAction(this.downloadAction);

                  if (false != result)
                  {
                     this.downloadTimeLimit = DateTime.Now.AddSeconds(2);
                     this.downloadStep = DownloadSteps.waitStartRequest;
                  }
                  else
                  {
                     downloadResult = "Unable to schedule start.";
                     downloadComplete = true;
                  }
               }
            }
            else if (DownloadSteps.waitStartRequest == this.downloadStep)
            {
               if (null == this.downloadAction)
               {
                  downloadResult = null;
                  downloadComplete = true;
               }
            }
            else
            {
               downloadResult = "Unknown step.";
               downloadComplete = true;
            }

            if (false != downloadComplete)
            {
               this.downloadData = null;
               this.downloadActive = false;
               this.downloadCompleteHandler(downloadResult);
            }
         }
         #endregion
      }

      public virtual void UploadLastTimeStampCorrection()
      {
      }

      public virtual void UploadLastSyncTime()
      {
      }

      public bool IsReceivedDownloadFrame(int cobId)
      {
         bool result = false;
         COBTypes frameType = (COBTypes)((cobId >> 7) & 0xF);
         int nodeId = (int)(cobId & 0x7F);

         if ((nodeId == this.NodeId) && 
             (COBTypes.TSDO == frameType) && 
             (DownloadSteps.waitFirmwareDownload == this.downloadStep))
         {
            result = true;
         }

         return(result);
     }

      public void SetCustomComTimeout(int timeLimit)
      {
         this.customComTimeLimit = timeLimit;
         this.customComTime = true;
      }

      public void DisableFaultReset()
      {
         this.faultResetEnabled = false;
      }

      #endregion
   }

}