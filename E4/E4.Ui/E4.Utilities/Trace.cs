namespace E4.Utilities
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Diagnostics;
   using System.IO;
   using System.Linq;
   using System.Net;
   using System.Net.Sockets;
   using System.Text;
   using System.Threading;

   #region TraceInfo

   public class TraceInfo
   {
      public DateTime dateTime;
      public string description;

      public override string ToString()
      {
         DateTime dt = this.dateTime;
         string timeString = string.Format("{0:D2}/{1:D2}/{2:D2} {3:D2}:{4:D2}:{5:D2}.{6:D3} ", dt.Month, dt.Day, (dt.Year % 100), dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
         string traceString = timeString + this.description;
         return (traceString);
      }
   }

   #endregion

   #region Tracer

   public enum TraceLevel : int
   {
      error,
      high,
      medium,
      low,
   }

   public static class Tracer
   {
      #region Definitions

      public delegate void OnMaskUpdateHandler();

      #endregion

      #region Fields

      private static byte[] mask;
      private static EventWaitHandle maskUpdateSignal;

      #endregion

      #region Properties

      public static OnMaskUpdateHandler MaskUpdateHandler { set; get; }
      public static string Name { set; get; }

      public static string MaskString
      {
         set
         {
            Tracer.mask = Tracer.ExtractByteArray(value);
         }

         get
         {
            if (null == Tracer.mask)
            {
               int enumCount = Enum.GetNames(Type.GetType("Telular.UDLS.Utilities.TraceGroup")).Length;
               int neededBytes = (enumCount + 3) / 4;
               Tracer.mask = new byte[neededBytes];
            }

            return (Tracer.ArrayDescriptor(Tracer.mask));
         }
      }

      #endregion

      #region Helper Functions

      private static byte[] ExtractByteArray(string text)
      {
         byte[] result = null;
         StringBuilder sb = new StringBuilder();
         bool valid = true;
         byte value = 0;
         int count = 0;

         if (null != text)
         {
            for (int i = 0; i < 2; i++)
            {
               // 2 passes, first to validate and count, second to store

               count = 0;

               for (int j = 0; j < text.Length; j += 2)
               {
                  sb.Remove(0, sb.Length);

                  if ((j + 0) < text.Length)
                  {
                     sb.Append(text[j + 0]);
                  }

                  if ((j + 1) < text.Length)
                  {
                     sb.Append(text[j + 1]);
                  }

                  if (byte.TryParse(sb.ToString(), System.Globalization.NumberStyles.HexNumber, null, out value) != false)
                  {
                     if (0 != i)
                     {
                        result[count] = value;
                     }

                     count++;
                  }
                  else
                  {
                     valid = false;
                     break;
                  }
               }

               if (0 == count)
               {
                  valid = false;
               }

               if (false != valid)
               {
                  if (0 == i)
                  {
                     result = new byte[count];
                  }
               }
               else
               {
                  break;
               }
            }
         }

         return (result);
      }

      private static string ArrayDescriptor(byte[] data)
      {
         string dataString = "";

         if (null != data)
         {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
               sb.AppendFormat("{0:X2}", data[i]);
            }

            dataString = sb.ToString();
         }

         return (dataString);
      }

      private static TraceLevel GroupLevel(TraceGroup group)
      {
         int groupSetting = 0;

         int index = ((int)group * 2);
         int offset = (index / 8);
         int shift = (index % 8);

         if ((null != Tracer.mask) && (offset < Tracer.mask.Length))
         {
            groupSetting = (Tracer.mask[offset] >> shift) & 0x3;
         }

         return ((TraceLevel)groupSetting);
      }

      private static bool Active(TraceGroup group, TraceLevel level)
      {
         bool result = false;

         if (level == TraceLevel.error)
         {
            result = true;
         }
         else
         {
            TraceLevel groupLevel = Tracer.GroupLevel(group);

            if (level <= groupLevel)
            {
               result = true;
            }
         }

         return (result);
      }

      private static void Write(string formatString, params object[] args)
      {
         if (null == Tracer.maskUpdateSignal) 
         {
            if (string.IsNullOrEmpty(Tracer.Name) == false)
            {
               bool created = false;
               Tracer.maskUpdateSignal = new EventWaitHandle(false, EventResetMode.ManualReset, Tracer.Name, out created);
            }
         }
         else 
         {
            bool signaled = Tracer.maskUpdateSignal.WaitOne(0);

            if (false != signaled)
            {
               Tracer.maskUpdateSignal.Reset();

               if (null != Tracer.MaskUpdateHandler)
               {
                  Tracer.MaskUpdateHandler();
               }
            }
         }

         TraceInfo info = new TraceInfo();
         info.dateTime = System.DateTime.Now;
         info.description = string.Format(formatString, args);

         System.Diagnostics.Trace.WriteLine(info, "");
      }

      #endregion

      #region Access Methods

      public static TraceLevel GetGroupLevel(TraceGroup group)
      {
         return (Tracer.GroupLevel(group));
      }

      public static void SetGroupLevel(TraceGroup group, TraceLevel level)
      {
         int index = ((int)group * 2);
         int offset = (index / 8);
         int shift = (index % 8);

         if ((null == Tracer.mask) || (offset >= Tracer.mask.Length))
         {
            if (null == Tracer.mask)
            {
               Tracer.mask = new byte[offset + 1];
            }

            if (offset >= Tracer.mask.Length)
            {
               Array.Resize<byte>(ref Tracer.mask, offset + 1);
            }
         }

         byte value = Tracer.mask[offset];
         value &= (byte)(~(0x3 << shift));
         value |= (byte)(((byte)level) << shift);
         Tracer.mask[offset] = value;
      }

      public static void Write(TraceGroup groupId, string formatString, params object[] args)
      {
         string prefix = TracePrefix.Value(groupId);
         Tracer.Write(prefix + formatString, args);
      }

      public static void WriteLow(TraceGroup groupId, string identifier, string formatString, params object[] args)
      {
         if (Tracer.Active(groupId, TraceLevel.low) != false)
         {
            string prefix = TracePrefix.Value(groupId);
            string id = (null != identifier) ? (identifier + " ") : string.Empty;
            Tracer.Write(id + prefix + formatString, args);
         }
      }

      public static void WriteMedium(TraceGroup groupId, string identifier, string formatString, params object[] args)
      {
         if (Tracer.Active(groupId, TraceLevel.medium) != false)
         {
            string prefix = TracePrefix.Value(groupId);
            string id = (null != identifier) ? (identifier + " ") : string.Empty;
            Tracer.Write(id + prefix + formatString, args);
         }
      }

      public static void WriteHigh(TraceGroup groupId, string identifier, string formatString, params object[] args)
      {
         if (Tracer.Active(groupId, TraceLevel.high) != false)
         {
            string prefix = TracePrefix.Value(groupId);
            string id = (null != identifier) ? (identifier + " ") : string.Empty;
            Tracer.Write(id + prefix + formatString, args);
         }
      }

      public static void WriteError(TraceGroup groupId, string identifier, string formatString, params object[] args)
      {
         if (Tracer.Active(groupId, TraceLevel.error) != false)
         {
            string prefix = TracePrefix.Value(groupId);
            string id = (null != identifier) ? (identifier + " ") : string.Empty;
            Tracer.Write(id + prefix + formatString, args);
         }
      }

      #endregion
   }

   #endregion

   #region FileTraceListener

   public class FileTraceListener : TraceListener
   {
      #region Fields

      private TextWriter writer;
      private DateTime writerTime;
      private int lineCount;
      private int fileCount;

      #endregion

      #region Properties

      public string LogFilePath { set; get; }
      public string Prefix { set; get; }
      public long MaximumLines { set; get; }

      #endregion

      #region Helper Functions

      private string GenerateFileName()
      {
         string result = string.Format("{0:d4}{1:d2}{2:d2}", this.writerTime.Year, this.writerTime.Month, this.writerTime.Day);
         return (result);
      }

      private void SetWriter()
      {
         if (null != this.writer)
         {
            this.writer.Close();
            this.writer.Dispose();
            this.writer = null;
         }

         if (string.IsNullOrEmpty(this.LogFilePath) != false)
         {
            this.LogFilePath = "C:\\Logs\\";
         }

         if (Directory.Exists(this.LogFilePath) == false)
         {
            Directory.CreateDirectory(this.LogFilePath);
         }

         if (Directory.Exists(this.LogFilePath) == false)
         {
            throw new Exception("Cannot create the folder: " + this.LogFilePath);
         }
         else
         {
            this.writerTime = DateTime.Now;
            string logPathName = this.LogFilePath + "\\" + this.Prefix + this.GenerateFileName();

            for (int i = 0; ; i++)
            {
               if (File.Exists(logPathName + "-" + i + ".txt") == false)
               {
                  this.fileCount = i;
                  break;
               }
            }

            this.writer = new StreamWriter(logPathName + "-" + this.fileCount + ".txt", false);
            this.lineCount = 0;
         }
      }

      private void CreateNewWriter()
      {
         this.fileCount = 0;
         this.lineCount = 0;
         this.SetWriter();
      }

      private void CreateNextWriter()
      {
         this.fileCount++;
         this.lineCount = 0;
         this.SetWriter();
      }

      private string ComposeTrace(Object obj, string category)
      {
         TraceInfo info = (TraceInfo)obj;
         DateTime dt = info.dateTime;

         string timeString = string.Format("{0:D2}/{1:D2}/{2:D2} {3:D2}:{4:D2}:{5:D2}.{6:D3} ", dt.Month, dt.Day, (dt.Year % 100), dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
         string traceString = timeString + info.description;

         return (traceString);
      }

      #endregion

      #region Constructors

      public FileTraceListener()
      {
         this.Prefix = "";

         this.writerTime = DateTime.Now.AddDays(-1);
         this.writer = null;
         this.lineCount = 0;
         this.fileCount = 0;
      }

      #endregion

      #region Methods

      public override void Write(string message)
      {
      }

      public override void WriteLine(string message)
      {
      }

      public override void WriteLine(Object obj, string category)
      {
         try
         {
            if (DateTime.Now.Day != this.writerTime.Day)
            {
               this.CreateNewWriter();
            }

            if (null != this.writer)
            {
               string traceString = this.ComposeTrace(obj, category);
               this.writer.WriteLine(traceString);
               this.writer.Flush();
               this.lineCount++;

               if (this.lineCount > this.MaximumLines)
               {
                  this.CreateNextWriter();
               }
            }
         }
         catch (Exception)
         {
            ;
         }
      }

      #endregion

   }

   #endregion

   #region UdpTraceListener

   public class UdpTraceListener : TraceListener
   {
      #region Fields

      private IPEndPoint destination;
      private UdpClient writer;

      private int sequenceId;

      #endregion

      #region Helper Functions

      private string ComposeTrace(Object obj, string category)
      {
         TraceInfo info = (TraceInfo)obj;
         DateTime dt = info.dateTime;

         string timeString = string.Format("{0:D2}/{1:D2}/{2:D2} {3:D2}:{4:D2}:{5:D2}.{6:D3} ", dt.Month, dt.Day, (dt.Year % 100), dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
         string traceString = timeString + info.description;

         return (traceString);
      }

      #endregion

      #region Constructors

      public UdpTraceListener(string ipAddress, int port)
      {
         IPAddress address = null;

         if (IPAddress.TryParse(ipAddress, out address) != false)
         {
            this.destination = new IPEndPoint(address, port);
         }

         this.sequenceId = 0;
      }

      #endregion

      #region Methods

      public override void Write(string message)
      {
      }

      public override void WriteLine(string message)
      {
      }

      public override void WriteLine(Object obj, string category)
      {
         if (null != this.destination)
         {
            if (null == this.writer)
            {
               try
               {
                  this.writer = new UdpClient();
               }
               catch { }
            }

            if (null != this.writer)
            {
               string alog = ComposeTrace(obj, category);
               byte[] traceArray = Encoding.UTF8.GetBytes(alog);

               int length = traceArray.Length + 4;
               byte[] datagram = new byte[traceArray.Length + 4];

               datagram[0] = (byte)((length >> 8) & 0xFF);
               datagram[1] = (byte)(length & 0xFF);
               datagram[2] = (byte)((this.sequenceId >> 8) & 0xFF);
               datagram[3] = (byte)(this.sequenceId & 0xFF);

               for (int i = 0; i < traceArray.Length; i++)
               {
                  datagram[4 + i] = traceArray[i];
               }

               this.writer.Send(datagram, length, this.destination);
               this.sequenceId++;
            }
         }
      }

      public override void Close()
      {
         if (null != this.writer)
         {
            this.writer.Close();
            this.writer = null;
         }
      }

      public void SetDestination(string ipAddress, int port)
      {
         IPAddress address = null;

         if (IPAddress.TryParse(ipAddress, out address) != false)
         {
            this.destination = new IPEndPoint(address, port);
         }      
      }

      #endregion
   }

   #endregion

   #region QueuedTraceListener

   public class QueuedTraceListener : TraceListener
   {
      #region Fields

      private Queue traceQueue;

      #endregion

      #region Constructors

      public QueuedTraceListener(Queue traceQueue)
      {
         this.traceQueue = traceQueue;
      }

      #endregion

      #region Methods

      public override void Write(string message)
      {
      }

      public override void WriteLine(string message)
      {
      }

      public override void WriteLine(Object obj, string category)
      {
         TraceInfo info = (TraceInfo)obj;
         DateTime dt = info.dateTime;

         string timeString = string.Format("{0:D2}/{1:D2}/{2:D2} {3:D2}:{4:D2}:{5:D2}.{6:D3} ", dt.Month, dt.Day, (dt.Year % 100), dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
         string traceString = timeString + info.description;

         this.traceQueue.Enqueue(traceString);
      }

      #endregion
   }

   #endregion
}
