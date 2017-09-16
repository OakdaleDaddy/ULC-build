
namespace CrossBore.CAN
{
   using System;
   using System.Collections.Generic;

   using CrossBore.Utilities;

   public class DeviceComponent
   {
      #region Definition

      public delegate bool CommExchangeHandler(CommAction action, int timeout, int attemptLimit);
      public delegate bool ClearErrorCodeHandler(UInt32 errorCode);

      #endregion

      #region Fields

      private string faultReason;

      #endregion

      #region Definition Properties

      public CommExchangeHandler OnCommExchange { set; get; }
      public ClearErrorCodeHandler OnClearErrorCode { set; get; }

      #endregion

      #region Operational Properties

      public string Name { set; get; }
      public EmergencyRecord FaultRecord { set; get; }
      public List<EmergencyRecord> ErrorList { set; get; }

      public string DeviceFaultReason
      {
         get
         {
            return (this.faultReason);
         }
      }

      public string FaultReason
      {
         get
         {
            string result = null;

            if (null != this.faultReason)
            {
               result = this.faultReason;
            }
            else if (null != this.FaultRecord)
            {
               result = this.FaultRecord.Description;
            }

            return (result);
         }
      }

      public string Warning
      {
         get
         {
            string result = null;

            if (0 != this.ErrorList.Count)
            {
               if (1 == this.ErrorList.Count)
               {
                  result = this.ErrorList[0].Description;
               }
               else
               {
                  result = string.Format("{0} errors", this.ErrorList.Count);
               }
            }

            return (result);
         }
      }

      #endregion

      #region Constructor

      public DeviceComponent()
      {
         this.FaultRecord = null;
         this.ErrorList = new List<EmergencyRecord>();
      }

      #endregion

      #region Protected Methods

      protected bool ExchangeCommAction(CommAction action, int timeout = 200, int attemptLimit = 2)
      {
         bool result = false;

         if (null != this.OnCommExchange)
         {
            result = this.OnCommExchange(action, timeout, attemptLimit);
         }

         return (result);
      }

      protected bool ClearErrorCode(UInt32 errorCode)
      {
         bool result = false;

         if (null != this.OnClearErrorCode)
         {
            result = this.OnClearErrorCode(errorCode);
         }

         return (result);
      }

      protected SDOUpload CreateSdoUpload(int location)
      {
         UInt16 index = (UInt16)((location >> 8) & 0xFFFF);
         byte subIndex = (byte)(location & 0xFF);
         SDOUpload result = new SDOUpload(index, subIndex);
         return (result);
      }

      protected bool ExchangeSdoDownload(int location, byte length, UInt32 data)
      {
         bool result = false;

         if (0 != location)
         {
            UInt16 index = (UInt16)((location >> 8) & 0xFFFF);
            byte subIndex = (byte)(location & 0xFF);
            result = this.ExchangeCommAction(new SDODownload(index, subIndex, length, data));
         }

         return (result);
      }

      #endregion

      #region Access Methods

      public virtual void Initialize()
      {
         this.Reset();
      }

      public virtual void Reset()
      {
         this.faultReason = null;
         this.FaultRecord = null;
         this.ErrorList.Clear();
      }

      public virtual void SetFault(string faultReason)
      {
         this.faultReason = faultReason;
      }

      public virtual void RecordError(bool fault, UInt32 code, string description)
      {
         string errorTypeString = null;

         if (false != fault)
         {
            if (null == this.FaultRecord)
            {
               this.FaultRecord = new EmergencyRecord(code, description);
               errorTypeString = "fault";
            }
         }
         else
         {
            bool found = false;

            for (int i = 0; i < this.ErrorList.Count; i++)
            {
               if (code == this.ErrorList[i].Code)
               {
                  found = true;
                  break;
               }
            }

            if (false == found)
            {
               this.ErrorList.Add(new EmergencyRecord(code, description));
               errorTypeString = "error";
            }
         }

         if (null != errorTypeString)
         {
            Tracer.WriteHigh(TraceGroup.CANBUS, "", "{0} {1} {2}", this.Name, errorTypeString, description);
         }
      }

      public virtual bool ClearError(UInt32 code, ref bool wasFaulted)
      {
         bool result = false;
         EmergencyRecord record = null;

         if (null != this.FaultRecord)
         {
            if (code == this.FaultRecord.Code)
            {
               record = this.FaultRecord;
               wasFaulted = true;
            }
         }

         if (null == record)
         {
            for (int i = 0; i < this.ErrorList.Count; i++)
            {
               if (code == this.ErrorList[i].Code)
               {
                  record = this.ErrorList[i];
                  break;
               }
            }
         }

         if (null != record)
         {
            result = this.ClearErrorCode(record.Code);

            if (false != result)
            {
               if (false != wasFaulted)
               {
                  this.FaultRecord = null;
               }
               else
               {
                  this.ErrorList.Remove(record);
               }
            }
         }

         return (result);
      }

      #endregion
   }
}