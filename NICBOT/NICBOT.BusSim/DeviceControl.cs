namespace NICBOT.BusSim
{
   using System;
   using System.Drawing;
   using System.Windows.Forms;
   using System.Text;
   using System.Xml;

   using Microsoft.Win32;

   public class DeviceControl : UserControl
   {
      #region Definition 

      protected enum DeviceStates
      {
         preop,
         running,
         stopped,
      }

      public delegate void SelectHandler(DeviceControl deviceControl, bool selected);
      public delegate bool DeviceTransmitHandler(int cobId, byte[] msg);

      #endregion

      #region Fields

      protected DeviceStates deviceState;
      protected bool selected;
      protected string busId;

      private bool transferActive;
      private bool transferUpload;
      private bool transferStarted;
      private UInt16 transferIndex;
      private byte transferSubIndex;
      private UInt32 transferLength;
      private UInt32 transferOffset;
      private bool transferToggle;
      private byte transferLastLength;
      private byte[] transferBuffer;

      #endregion

      #region Properties

      public SelectHandler OnSelect { set; get; }
      public DeviceTransmitHandler OnDeviceTransmit { set; get; }

      #endregion

      #region Helper Functions

      protected bool Transmit(int cobId, byte[] data)
      {
         bool result = false;

         if (null != this.OnDeviceTransmit)
         {
            result = this.OnDeviceTransmit(cobId, data);
         }

         return (result);
      }

      protected int GetCobId(COBTypes cobType, int nodeId)
      {
         int cobId = (int)(((int)cobType << 7) | (nodeId & 0x7F));
         return (cobId);
      }

      protected UInt32 MoveDeviceData(byte[] buffer, byte value, int offset = 0)
      {
         buffer[0 + offset] = value;
         return (1);
      }

      protected UInt32 MoveDeviceData(byte[] buffer, UInt16 value, int offset = 0)
      {
         byte[] source = BitConverter.GetBytes(value);

         for (int i = 0; i < source.Length; i++)
         {
            buffer[i + offset] = source[i];
         }

         return ((UInt32)source.Length);
      }

      protected UInt32 MoveDeviceData(byte[] buffer, Int16 value, int offset = 0)
      {
         byte[] source = BitConverter.GetBytes(value);

         for (int i = 0; i < source.Length; i++)
         {
            buffer[i + offset] = source[i];
         }

         return ((UInt32)source.Length);
      }
      protected UInt32 MoveDeviceData(byte[] buffer, UInt32 value, int offset = 0)
      {
         byte[] source = BitConverter.GetBytes(value);

         for (int i = 0; i < source.Length; i++)
         {
            buffer[i + offset] = source[i];
         }

         return ((UInt32)source.Length);
      }

      protected UInt32 MoveDeviceData(byte[] buffer, Int32 value, int offset = 0)
      {
         byte[] source = BitConverter.GetBytes(value);

         for (int i = 0; i < source.Length; i++)
         {
            buffer[i + offset] = source[i];
         }

         return ((UInt32)source.Length);
      }

      protected UInt32 MoveDeviceData(byte[] buffer, float value, int offset = 0)
      {
         byte[] source = BitConverter.GetBytes(value);

         for (int i = 0; i < source.Length; i++)
         {
            buffer[i + offset] = source[i];
         }

         return ((UInt32)source.Length);
      }

      protected UInt32 MoveDeviceData(byte[] buffer, string value, int offset = 0)
      {
         byte[] source = Encoding.UTF8.GetBytes(value);

         for (int i = 0; i < source.Length; i++)
         {
            buffer[i + offset] = source[i];
         }

         return ((UInt32)source.Length);
      }

      protected UInt32 ExtractUInt32(byte[] buffer, int offset, UInt32 length)
      {
         UInt32 result = 0;
         int shifter = 0;

         for (int i = 0; i < length; i++)
         {
            result |= (UInt32)(((UInt32)buffer[offset + i]) << shifter);
            shifter += 8;
         }

         return (result);
      }

      private void TransmitDownloadResponse(int nodeId)
      {
         if (false == this.transferStarted)
         {
            if (this.transferLength <= 4)
            {
               int cobId = this.GetCobId(COBTypes.TSDO, nodeId);

               byte[] rspFrame = new byte[8];
               rspFrame[0] = 0x60;
               rspFrame[1] = (byte)((this.transferIndex >> 0) & 0xFF);
               rspFrame[2] = (byte)((this.transferIndex >> 8) & 0xFF);
               rspFrame[3] = this.transferSubIndex;
               rspFrame[4] = 0;
               rspFrame[5] = 0;
               rspFrame[6] = 0;
               rspFrame[7] = 0;

               this.Transmit(cobId, rspFrame);
               this.transferActive = false;
            }
            else
            {
               int cobId = this.GetCobId(COBTypes.TSDO, nodeId);

               byte[] rspFrame = new byte[8];
               rspFrame[0] = 0x60;
               rspFrame[1] = (byte)((this.transferIndex >> 0) & 0xFF);
               rspFrame[2] = (byte)((this.transferIndex >> 8) & 0xFF);
               rspFrame[3] = this.transferSubIndex;
               rspFrame[4] = 0;
               rspFrame[5] = 0;
               rspFrame[6] = 0;
               rspFrame[7] = 0;

               this.Transmit(cobId, rspFrame);
               this.transferStarted = true;
            }
         }
         else
         {
            int cobId = this.GetCobId(COBTypes.TSDO, nodeId);

            byte[] rspFrame = new byte[8];
            rspFrame[0] = (byte)(0x20 | ((false != this.transferToggle) ? 0x10 : 0));
            rspFrame[1] = (byte)((this.transferIndex >> 0) & 0xFF);
            rspFrame[2] = (byte)((this.transferIndex >> 8) & 0xFF);
            rspFrame[3] = this.transferSubIndex;
            rspFrame[4] = 0;
            rspFrame[5] = 0;
            rspFrame[6] = 0;
            rspFrame[7] = 0;

            this.Transmit(cobId, rspFrame);
         }
      }

      private void TransmitUploadResponse(int nodeId)
      {
         if (false == this.transferStarted)
         {
            if (this.transferLength <= 4)
            {
               // send data

               int cobId = this.GetCobId(COBTypes.TSDO, nodeId);

               byte[] rspFrame = new byte[8];
               rspFrame[0] = (byte)((2 << 5) | ((4 - this.transferLength) << 2) | 3);
               rspFrame[1] = (byte)((this.transferIndex >> 0) & 0xFF);
               rspFrame[2] = (byte)((this.transferIndex >> 8) & 0xFF);
               rspFrame[3] = this.transferSubIndex;

               for (int i = 0; i < this.transferLength; i++)
               {
                  if (i < this.transferLength)
                  {
                     rspFrame[4 + i] = this.transferBuffer[i];
                  }
                  else
                  {
                     rspFrame[4 + i] = 0;
                  }
               }

               this.Transmit(cobId, rspFrame);
               this.transferActive = false;
            }
            else
            {
               // send length

               int cobId = this.GetCobId(COBTypes.TSDO, nodeId);

               byte[] rspFrame = new byte[8];
               rspFrame[0] = (byte)((2 << 5) | 1); ;
               rspFrame[1] = (byte)((this.transferIndex >> 0) & 0xFF);
               rspFrame[2] = (byte)((this.transferIndex >> 8) & 0xFF);
               rspFrame[3] = this.transferSubIndex;
               rspFrame[4] = (byte)((this.transferLength >> 0) & 0xFF);
               rspFrame[5] = (byte)((this.transferLength >> 8) & 0xFF);
               rspFrame[6] = (byte)((this.transferLength >> 16) & 0xFF);
               rspFrame[7] = (byte)((this.transferLength >> 24) & 0xFF);

               this.Transmit(cobId, rspFrame);
               this.transferStarted = true;
            }
         }
         else
         {
            // send segment

            int cobId = this.GetCobId(COBTypes.TSDO, nodeId);

            UInt32 remaining = this.transferLength - this.transferOffset;
            int n = (int)((remaining < 7) ? (7 - remaining) : 0);
            int t = (false != this.transferToggle) ? 1 : 0;
            int c = (remaining <= 7) ? 1 : 0;

            byte[] rspFrame = new byte[8];
            rspFrame[0] = (byte)((t << 4) | (n << 1) | c);

            for (int i = 0; i < 7; i++)
            {
               byte ch = 0;

               if (i < remaining)
               {
                  ch = this.transferBuffer[this.transferOffset + i];
               }

               rspFrame[1 + i] = ch;
            }

            this.Transmit(cobId, rspFrame);
            this.transferLastLength = (byte)(7 - n);
         }
      }

      private void TransmitTransferAbort(int nodeId, UInt16 index, byte subIndex, UInt32 code)
      {
         int cobId = this.GetCobId(COBTypes.TSDO, nodeId);

         byte[] rspFrame = new byte[8];
         rspFrame[0] = 0x80;
         rspFrame[1] = (byte)((index >> 0) & 0xFF);
         rspFrame[2] = (byte)((index >> 8) & 0xFF);
         rspFrame[3] = subIndex;
         rspFrame[4] = (byte)((code >> 0) & 0xFF);
         rspFrame[5] = (byte)((code >> 8) & 0xFF);
         rspFrame[6] = (byte)((code >> 16) & 0xFF);
         rspFrame[7] = (byte)((code >> 24) & 0xFF);

         this.Transmit(cobId, rspFrame);
      }

      #endregion

      #region Device Virtual Functions

      protected virtual void Reset()
      {
         this.deviceState = DeviceStates.preop;
         this.transferActive = false;
      }

      protected virtual void Start()
      {
         this.deviceState = DeviceStates.running;
      }

      protected virtual void Stop()
      {
         this.deviceState = DeviceStates.stopped;
      }

      protected virtual bool LoadDeviceData(UInt16 index, byte subIndex, byte[] buffer, ref UInt32 dataLength)
      {
         dataLength = 0;
         return (false);
      }

      protected virtual bool EvaluateDeviceDataSize(UInt16 index, byte subIndex, UInt32 dataLength)
      {
         return (false);
      }

      protected virtual bool StoreDeviceData(UInt16 index, byte subIndex, byte[] buffer, int offset, UInt32 dataLength)
      {
         return (false);
      }

      #endregion

      #region Upload Functions

      protected void InitiateDownload(int nodeId, byte[] frame)
      {
         if (false != this.transferActive)
         {
            UInt32 dataLength = this.transferOffset + this.transferLastLength;

            if (dataLength == this.transferLength)
            {
               this.transferActive = false;
            }
         }

         if (false == this.transferActive)
         {
            int n = (int)((frame[0] >> 2) & 0x3);
            int e = (int)((frame[0] >> 1) & 0x1);
            int s = (int)((frame[0] >> 0) & 0x1);
            UInt16 index = BitConverter.ToUInt16(frame, 1);
            byte subIndex = frame[3];

            if ((0 == e) && (0 == s))
            {
               // reserved

               this.TransmitTransferAbort(nodeId, index, subIndex, 0x08000000);
            }
            else if ((0 == e) && (1 == s))
            {
               // data = number of bytes to transfer

               UInt32 dataLength = BitConverter.ToUInt32(frame, 4);
               bool valid = this.EvaluateDeviceDataSize(index, subIndex, dataLength);

               if (false != valid)
               {
                  this.transferActive = true;
                  this.transferUpload = false;
                  this.transferStarted = false;
                  this.transferIndex = index;
                  this.transferSubIndex = subIndex;
                  this.transferToggle = false;
                  this.transferLength = dataLength;
                  this.transferOffset = 0;
                  this.transferLastLength = 0;

                  this.TransmitDownloadResponse(nodeId);
               }
               else
               {
                  this.TransmitTransferAbort(nodeId, index, subIndex, 0x08000000);
               }
            }
            else if ((1 == e) && (1 == s))
            {
               UInt32 dataLength = (UInt32)(4 - n);
               bool valid = this.StoreDeviceData(index, subIndex, frame, 4, dataLength);

               if (false != valid)
               {
                  this.transferActive = true;
                  this.transferUpload = false;
                  this.transferStarted = false;
                  this.transferIndex = index;
                  this.transferSubIndex = subIndex;
                  this.transferToggle = false;
                  this.transferLength = dataLength;
                  this.transferOffset = 0;
                  this.transferLastLength = 0;

                  this.TransmitDownloadResponse(nodeId);
               }
               else
               {
                  this.TransmitTransferAbort(nodeId, index, subIndex, 0x08000000);
               }
            }
            else if ((1 == e) && (0 == s))
            {
               // data = unspecified number of bytes to transfer

               UInt32 dataLength = (UInt32)(frame.Length - 4);
               bool valid = this.StoreDeviceData(index, subIndex, frame, 4, dataLength);

               if (false != valid)
               {
                  this.transferActive = true;
                  this.transferUpload = false;
                  this.transferStarted = false;
                  this.transferIndex = index;
                  this.transferSubIndex = subIndex;
                  this.transferToggle = false;
                  this.transferLength = dataLength;
                  this.transferOffset = 0;
                  this.transferLastLength = 0;

                  this.TransmitDownloadResponse(nodeId); 
               }
               else
               {
                  this.TransmitTransferAbort(nodeId, index, subIndex, 0x08000000);
               }
            }
         }
         else
         {
            this.TransmitTransferAbort(nodeId, this.transferIndex, this.transferSubIndex, 0x08000001);
            this.transferActive = false;
         }
      }

      protected void ProcessDownload(int nodeId, byte[] frame)
      {
         int t = (int)((frame[0] >> 4) & 0x1);
         int n = (int)((frame[0] >> 1) & 0x7);
         int c = (int)((frame[0] >> 0) & 0x1);

         if ((false != this.transferActive) &&
             (false != this.transferStarted) &&
             (false == this.transferUpload))
         {
            bool toggled = (0 != t);
            byte dataCount = (byte)(7 - n);

            if (toggled != this.transferToggle)
            {
               this.transferOffset += this.transferLastLength;
               this.transferToggle = toggled;
            }

            for (int i = 0; i < dataCount; i++)
            {
               this.transferBuffer[this.transferOffset + i] = frame[i + 1];
            }

            this.transferLastLength = dataCount;
            UInt32 dataLength = this.transferOffset + this.transferLastLength;

            if (dataLength == this.transferLength)
            {
               bool valid = this.StoreDeviceData(this.transferIndex, this.transferSubIndex, this.transferBuffer, 0, dataLength);

               if (false != valid)
               {
                  this.TransmitDownloadResponse(nodeId);
               }
               else
               {
                  this.TransmitTransferAbort(nodeId, this.transferIndex, this.transferSubIndex, 0x08000000);
                  this.transferActive = false;
               }
            }
            else
            {
               this.TransmitDownloadResponse(nodeId);
            }
         }
         else
         {
            this.TransmitTransferAbort(nodeId, this.transferIndex, this.transferSubIndex, 0x08000000);
            this.transferActive = false;
         }
      }

      protected void InitiateUpload(int nodeId, UInt16 index, byte subIndex)
      {
         if (false != this.transferActive)
         {
            UInt32 dataLength = this.transferOffset + this.transferLastLength;

            if (dataLength == this.transferLength)
            {
               this.transferActive = false;
            }
         }

         if (false == this.transferActive)
         {
            UInt32 dataLength = 0;
            bool valid = this.LoadDeviceData(index, subIndex, this.transferBuffer, ref dataLength);

            if (false != valid)
            {
               this.transferActive = true;
               this.transferUpload = true;
               this.transferStarted = false;
               this.transferIndex = index;
               this.transferSubIndex = subIndex;
               this.transferToggle = false;
               this.transferLength = dataLength;
               this.transferOffset = 0;
               this.transferLastLength = 0;

               this.TransmitUploadResponse(nodeId);
            }
            else
            {
               this.TransmitTransferAbort(nodeId, index, subIndex, 0x08000000);
            }
         }
         else
         {
            this.TransmitTransferAbort(nodeId, this.transferIndex, this.transferSubIndex, 0x08000001);
            this.transferActive = false;
         }
      }

      protected void ProcessUpload(int nodeId, bool toggled, UInt16 index, byte subIndex)
      {
         if ((false != this.transferActive) &&
             (false != this.transferStarted) &&
             (false != this.transferUpload) && 
             (index == this.transferIndex) &&
             (subIndex == this.transferSubIndex))
         {
            if (toggled != this.transferToggle)
            {
               this.transferOffset += this.transferLastLength;
               this.transferToggle = toggled;
            }

            this.TransmitUploadResponse(nodeId);
         }
         else
         {
            this.transferActive = false;
            this.TransmitTransferAbort(nodeId, this.transferIndex, this.transferSubIndex, 0x08000000);
         }
      }

      protected void AbortTransfer(int nodeId, UInt16 index, byte subIndex, UInt32 code)
      {
         if ((false != this.transferActive) &&
             (index == this.transferIndex) &&
             (subIndex == this.transferSubIndex))
         {
            this.TransmitTransferAbort(nodeId, this.transferIndex, this.transferSubIndex, code);
            this.transferActive = false;
         }
      }

      #endregion

      #region Control Events

      private void DeviceControl_Click(object sender, EventArgs e)
      {
         this.selected = !this.selected;
         this.BackColor = (false != this.selected) ? Color.DarkGray : Color.Gainsboro;

         if (null != this.OnSelect)
         {
            this.OnSelect(this, selected);
         }
      }

      #endregion

      #region Constructor 

      public DeviceControl()
         : base()
      {
         this.Click += new EventHandler(this.DeviceControl_Click);
         this.BackColor = Color.Gainsboro;
         this.selected = false;

         this.transferBuffer = new byte[256 * 1024];
      }

      #endregion

      #region Access Functions

      public virtual void LoadRegistry(RegistryKey appKey, string deviceTag)
      {
      }

      public virtual void SaveRegistry(RegistryKey appKey, string deviceTag)
      {
      }

      public virtual void Read(XmlReader reader)
      {
      }

      public virtual void Write(XmlWriter writer)
      {
      }

      public virtual void SetBusId(string busId)
      {
         this.busId = busId;
      }

      public virtual string GetBusId()
      {
         return (this.busId);
      }

      public virtual void PowerUp()
      {
      }

      public virtual void PowerDown()
      {
      }

      public virtual void DeviceReceive(int cobId, byte[] msg)
      {
      }

      public virtual void UpdateDevice()
      {
      }
            
      public void DeSelect()
      {
         this.selected = false;
         this.BackColor = Color.Gainsboro;
      }

      #endregion
   }
}