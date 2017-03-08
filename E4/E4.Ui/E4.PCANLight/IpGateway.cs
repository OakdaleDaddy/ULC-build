
namespace E4.PCANLight
{
   using System;
   using System.Runtime.InteropServices;
   using System.Net;
   using System.Net.Sockets;
   using System.Text;
   using System.Threading;

   public class IpGateway
   {
      #region Definition

      // CAN message
      //
      [StructLayout(LayoutKind.Sequential, Pack = 1)]
      public struct TPCANMsg
      {
         public uint ID;           // 11/29 bit identifier
         public byte MSGTYPE;      // Bits from MSGTYPE_*
         public byte LEN;          // Data Length Code of the Msg (0..8)
         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] // ?
         public byte[] DATA;       // Data 0 .. 7		
      }

      // Timestamp of a receive/transmit event
      // Total microseconds = micros + 1000 * millis + 0xFFFFFFFF * 1000 * millis_overflow
      //
      [StructLayout(LayoutKind.Sequential, Pack = 1)]
      public struct TPCANTimestamp
      {
         public uint millis;             // Base-value: milliseconds: 0.. 2^32-1
         public ushort millis_overflow;  // Roll-arounds of millis
         public ushort micros;           // Microseconds: 0..999
      }

      private TPCANMsg EmptyMessage = new TPCANMsg();
      private TPCANTimestamp EmptyTimestamp = new TPCANTimestamp();

      private EventWaitHandle receiveEventWaitHandle;

      private IPEndPoint transmitEndPoint;
      private IPEndPoint receiveEndPoint;
      private UdpClient socket;

      private AsyncCallback socketCb;
      private object socketLock;

      private RingBuffer<UdpReceive> receiveBuffer;

      #endregion

      #region Fields

      private static IpGateway ipgw1 = null;
      private static IpGateway ipgw2 = null;

      #endregion

      #region Properties

      public static IpGateway IPGW1
      {
         get
         {
            if (null == ipgw1)
            {
               ipgw1 = new IpGateway();
               ipgw1.Initialize();
            }

            return ipgw1;
         }
      }

      public static IpGateway IPGW2
      {
         get
         {
            if (null == ipgw2)
            {
               ipgw2 = new IpGateway();
               ipgw2.Initialize();
            }

            return ipgw2;
         }
      }

      #endregion

      #region Delegates

      private void SocketReceiveCallback(IAsyncResult ar)
      {
         UdpClient u = (UdpClient)(ar.AsyncState);

         lock (this.socketLock)
         {
            if (null != u.Client)
            {
               IPEndPoint endPoint = new IPEndPoint(0, 0);
               byte[] receiveBytes = u.EndReceive(ar, ref endPoint);
               this.socket.BeginReceive(this.socketCb, this.socket);

               UdpReceive r = new UdpReceive();
               r.Endpoint = endPoint;
               r.Data = receiveBytes;

               this.receiveBuffer.Store(r);

               this.receiveEventWaitHandle.Set();
            }
         }
      }

      #endregion

      #region Helper Functions

      private void TransferMessage(byte[] data, ref TPCANMsg message)
      {
         message.ID = ((uint)data[24] << 24) | ((uint)data[25] << 16) | ((uint)data[26] << 8) | ((uint)data[27] << 0);
         message.MSGTYPE = data[3];
         message.LEN = data[21];

         message.DATA = new byte[message.LEN];

         for (int i = 0; i < message.LEN; i++)
         {
            message.DATA[i] = data[28 + i];
         }
      }

      private void TransferTimestamp(byte[] data, ref TPCANTimestamp timestamp)
      {
         UInt32 l = ((uint)data[12] << 24) | ((uint)data[13] << 16) | ((uint)data[14] << 8) | ((uint)data[15] << 0);
         UInt32 h = ((uint)data[16] << 24) | ((uint)data[17] << 16) | ((uint)data[18] << 8) | ((uint)data[19] << 0);
         UInt64 x = ((UInt64)h << 32) | ((UInt64)l);

         UInt16 microSeconds = (UInt16)(x % 1000);
         UInt64 milliSecondsLarge = x / 1000;

         UInt32 milliSeconds = (UInt32)(milliSecondsLarge & 0xFFFFFFFF);
         UInt16 milliSecondsOverflow = (UInt16)(milliSecondsLarge >> 32);

         timestamp.micros = microSeconds;
         timestamp.millis = milliSeconds;
         timestamp.millis_overflow = milliSecondsOverflow;
      }

      private void CreateSocket()
      {
         bool error = false;
          
         try
         {
            if ((null != this.transmitEndPoint) &&
                (null != this.receiveEndPoint))
            {
               this.socket = new UdpClient(this.receiveEndPoint);
               this.socket.Client.Blocking = false;
               this.socket.BeginReceive(this.socketCb, this.socket);
            }
         }
         catch (Exception createException)
         {
            string message = createException.Message;
            error = true;
         }

         if (false != error)
         {
            this.CloseSocket();
         }
      }

      private void CloseSocket()
      {
         lock (this.socketLock)
         {
            if (null != this.socket)
            {
               this.socket.Close();
               this.socket = null;
            }
         }
      }

      #endregion

      #region Constructor

      public IpGateway()
      {
      }

      #endregion

      #region Access Methods

      public void Initialize()
      {
         this.socketLock = new object();
         this.socketCb = new AsyncCallback(this.SocketReceiveCallback);
         this.receiveBuffer = new RingBuffer<UdpReceive>();
      }

      public CANResult Init(int msgType, IPEndPoint transmitEndPoint, IPEndPoint receiveEndPoint)
      {
         CANResult result = CANResult.ERR_OK;

         this.receiveBuffer.Reset();

         this.transmitEndPoint = transmitEndPoint;
         this.receiveEndPoint = receiveEndPoint;
         this.CreateSocket();

         if (null == this.socket)
         {
            result = CANResult.ERR_PARMTYP;
         }

         return (result);
      }

      public CANResult VersionInfo(StringBuilder sb)
      {
         CANResult result = CANResult.ERR_OK;

         if (null != sb)
         {
            sb.Append("Interface to access PCAN-Wireless Gateway");
            result = CANResult.ERR_OK;
         }
         else
         {
            result = CANResult.ERR_PARMTYP;
         }

         return (result);
      }

      public CANResult DLLVersionInfo(StringBuilder sb)
      {
         CANResult result = CANResult.ERR_OK;

         if (null != sb)
         {
            sb.Append("v1.0");
            result = CANResult.ERR_OK;
         }
         else
         {
            result = CANResult.ERR_PARMTYP;
         }

         return (result);
      }

      public CANResult ResetClient()
      {
         CANResult result = CANResult.ERR_OK;

         this.CloseSocket();
         this.receiveBuffer.Reset();
         this.CreateSocket();

         return (result);
      }

      public CANResult MsgFilter(uint from, uint to, int msgType)
      {
         CANResult result = CANResult.ERR_OK;

         return (result);
      }

      public CANResult ResetFilter()
      {
         CANResult result = CANResult.ERR_OK;

         return (result);
      }

      public CANResult SetRcvEvent(EventWaitHandle receiveEventWaitHandle)
      {
         CANResult result = CANResult.ERR_OK;

         this.receiveEventWaitHandle = receiveEventWaitHandle;

         return (result);
      }

      public CANResult Read(out TPCANMsg message)
      {
         CANResult result = CANResult.ERR_QRCVEMPTY;
         message = EmptyMessage;

         try
         {
            if (this.receiveBuffer.Contains() != false)
            {
               UdpReceive r = (UdpReceive)this.receiveBuffer.Get();

               if (r.Data.Length >= 36)
               {
                  this.TransferMessage(r.Data, ref message);
                  result = CANResult.ERR_OK;
               }
               
               if (receiveBuffer.Contains() == false)
               {
                  result |= CANResult.ERR_QRCVEMPTY;
               }
            }
         }
         catch (Exception readException)
         {
            string exceptionMessage = readException.Message;
            result = CANResult.ERR_ILLNET;
         }

         return (result);
      }

      public CANResult ReadEx(out TPCANMsg message, out TPCANTimestamp timeStamp)
      {
         CANResult result = CANResult.ERR_QRCVEMPTY;
         message = EmptyMessage;
         timeStamp = EmptyTimestamp;

         try
         {
            if (this.receiveBuffer.Contains() != false)
            {
               UdpReceive r = (UdpReceive)this.receiveBuffer.Get();

               if (r.Data.Length >= 36)
               {
                  this.TransferMessage(r.Data, ref message);
                  this.TransferTimestamp(r.Data, ref timeStamp);
                  result = CANResult.ERR_OK;
               }

               if (receiveBuffer.Contains() == false)
               {
                  result |= CANResult.ERR_QRCVEMPTY;
               }
            }
         }
         catch (Exception readException)
         {
            string exceptionMessage = readException.Message;
            result = CANResult.ERR_ILLNET;
         }

         return (result);
      }

      public CANResult Write(ref TPCANMsg message)
      {
         CANResult result = CANResult.ERR_ILLNET;

         try
         {
            byte[] datagram = new byte[36];

            // length
            datagram[0] = 0x00;
            datagram[1] = 0x24;

            // type
            datagram[2] = 0x00;
            datagram[3] = 0x80;

            // tag
            datagram[4] = 0x54;
            datagram[5] = 0x00;
            datagram[6] = 0x00;
            datagram[7] = 0x00;
            datagram[8] = 0x52;
            datagram[9] = 0x00;
            datagram[10] = 0x00;
            datagram[11] = 0x00;

            // timestamp low
            datagram[12] = 0x00;
            datagram[13] = 0x00;
            datagram[14] = 0x00;
            datagram[15] = 0x00;

            // timestamp high
            datagram[16] = 0x00;
            datagram[17] = 0x00;
            datagram[18] = 0x00;
            datagram[19] = 0x00;

            // channel
            datagram[20] = 0x00;

            // DLC
            datagram[21] = message.LEN;

            // flags
            datagram[22] = 0x00;
            datagram[23] = 0x00;

            // CAN-ID
            datagram[24] = (byte)((message.ID >> 24) & 0xFF);
            datagram[25] = (byte)((message.ID >> 16) & 0xFF);
            datagram[26] = (byte)((message.ID >> 8) & 0xFF);
            datagram[27] = (byte)((message.ID >> 0) & 0xFF);

            // data
            for (int i = 0; i < 8; i++)
            {
               byte ch = 0;

               if (i < message.DATA.Length)
               {
                  ch = message.DATA[i];
               }

               datagram[28 + i] = ch;
            }

            this.socket.Send(datagram, datagram.Length, this.transmitEndPoint);
            result = CANResult.ERR_OK;
         }
         catch (Exception writeException)
         {
            string exceptionMessage = writeException.Message;
         }

         return (result);
      }

      public CANResult Status()
      {
         CANResult result = CANResult.ERR_OK;

         return (result);
      }

      public CANResult Close()
      {
         CANResult result = CANResult.ERR_OK;

         this.CloseSocket();

         return(result);
      }

      #endregion

   }
}
