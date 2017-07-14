namespace Weco.Ui
{
   using System;
   using System.Collections.Generic;
   using System.IO.Ports;
   using System.Linq;
   using System.Text;
   using System.Threading;
   using System.Threading.Tasks;

   public class VideoStampOsd
   {
      #region Definitions

      public enum DateFormat
      {
         mmddyy,
         ddmmyy,
      };

      public enum TimeFormat
      {
         twentyFourHour,
         twelveHour,
      };

      #endregion

      #region Fields

      private static VideoStampOsd instance = null;

      private bool active;

      private SerialPort serialPort;
      private byte[] serialPortRxBuffer;

      private byte[] serialPortProcessBuffer;
      private int serialPortProcessCount;

      private string faultReason;

      private bool execute;
      private Thread thread;

      private bool descriptionVisible;
      private string[] descriptionText;

      private bool cameraIdVisible;
      private string channel1CameraId;
      private string channel2CameraId;

      private bool configured;

      #endregion

      #region Properties

      public static VideoStampOsd Instance
      {
         get
         {
            if (null == instance)
            {
               instance = new VideoStampOsd();
               instance.Initialize();
            }

            return instance;
         }
      }

      public string FaultReason { get { return (this.faultReason); } }

      #endregion

      #region Helper Functions

      private void ProcessRxData(byte[] buffer, int count)
      {
         for (int i = 0; i < count; i++)
         {
            if (this.serialPortProcessCount < this.serialPortProcessBuffer.Length)
            {
               byte ch = buffer[i];
               this.serialPortProcessBuffer[this.serialPortProcessCount] = ch;
               this.serialPortProcessCount++;
            }
         }
      }

      private void WriteTimeDisplayCommand(bool visible)
      {
         if (false != this.active)
         {
            byte[] cmd = new byte[3];
            cmd[0] = 0xF2;
            cmd[1] = (byte)((false != visible) ? 1 : 0);
            this.serialPort.Write(cmd, 0, cmd.Length);
         }
      }

      private void WriteDateDisplayCommand(bool visible)
      {
         if (false != this.active)
         {
            byte[] cmd = new byte[3];
            cmd[0] = 0xF3;
            cmd[1] = (byte)((false != visible) ? 1 : 0);
            this.serialPort.Write(cmd, 0, cmd.Length);
         }
      }

      #endregion

      #region Delegates

      private void SerialPortRxDataHandler(object o, SerialDataReceivedEventArgs args)
      {
         if (SerialData.Chars == args.EventType)
         {
            int receiveCount = this.serialPort.Read(this.serialPortRxBuffer, 0, this.serialPortRxBuffer.Length);
            this.ProcessRxData(this.serialPortRxBuffer, receiveCount);
         }
      }

      #endregion

      #region Process Functions

      private void Process()
      {
         this.faultReason = null;

         for (; execute; )
         {
            Thread.Sleep(50);
         }
      }

      #endregion

      #region Constructor

      private VideoStampOsd()
      {
         this.serialPort = new SerialPort();

         this.serialPort.Parity = Parity.None;
         this.serialPort.DataBits = 8;
         this.serialPort.StopBits = System.IO.Ports.StopBits.One;
         this.serialPort.Handshake = Handshake.None;
         this.serialPort.Encoding = Encoding.UTF8;
         this.serialPort.RtsEnable = true;
         this.serialPort.DataReceived += new SerialDataReceivedEventHandler(this.SerialPortRxDataHandler);

         this.serialPortRxBuffer = new byte[256];
         this.serialPortProcessBuffer = new byte[32];
      }

      #endregion

      #region Access Methods

      public void Initialize()
      {
         this.active = false;
         this.faultReason = null;

         this.descriptionText = new string[8];

         for (int i = 0; i < this.descriptionText.Length; i++)
         {
            this.descriptionText[i] = "";
         }

         this.descriptionVisible = false;

         this.cameraIdVisible = false;
         this.channel1CameraId = "";
         this.channel2CameraId = "";

         this.configured = false;
      }

      public void Start(int port, int rate)
      {
         if (false == this.active)
         {
            string serialPortName = "COM" + port.ToString();
            this.serialPort.PortName = serialPortName;
            this.serialPort.BaudRate = rate;

            try
            {
               this.serialPort.Open();
               this.active = true;

               this.thread = new Thread(this.Process);
               this.thread.IsBackground = true;
               this.thread.Name = "VideoStamp OSD";

               this.execute = true;
               this.thread.Start();
            }
            catch
            {
               this.faultReason = "unable to open port";
            }
         }
      }

      public void Stop()
      {
         if (false != this.active)
         {
            this.execute = false;
            this.thread.Join(3000);

            this.thread = null;

            this.serialPort.Close();

            this.active = false;
            this.configured = false;
            this.faultReason = "closed";
         }
      }

      public void Configure(OsdParameters osdParameters)
      {
         this.SetVideoChannel(0); // all channels
         this.SetScreenHorizontalPositionOffset(osdParameters.HorizontalOffset);
         this.SetScreenVerticalPositionOffset(osdParameters.VerticalOffset);

         this.ClearScreen();

         this.SetDateVisible(osdParameters.ShowDate);
         Thread.Sleep(50);
         this.SetTimeVisible(osdParameters.ShowTime);
         Thread.Sleep(50);

         this.SetDescriptionText(1, osdParameters.Line1);
         this.SetDescriptionText(2, osdParameters.Line2);
         this.SetDescriptionText(3, osdParameters.Line3);
         this.SetDescriptionText(4, osdParameters.Line4);
         this.SetDescriptionText(5, osdParameters.Line5);

         this.SetDescriptionVisible(osdParameters.ShowDescription);
         Thread.Sleep(50);
         this.SetCameraIdVisible(osdParameters.ShowCameraId);

         this.configured = true;
      }

      public void SetVideoChannel(int channel)
      {
         if (false != this.active)
         {
            byte[] cmd = new byte[2];
            cmd[0] = 0xE0;
            cmd[1] = (byte)channel;
            this.serialPort.Write(cmd, 0, cmd.Length);
         }
      }

      public void ClearScreen()
      {
         if (false != this.active)
         {
            byte[] cmd = new byte[1];
            cmd[0] = 0xE3;
            this.serialPort.Write(cmd, 0, cmd.Length);
         }
      }

      public void SetCursorPosition(int x, int y)
      {
         if (false != this.active)
         {
            byte[] cmd = new byte[3];
            cmd[0] = 0xE5;
            cmd[1] = (byte)x;
            cmd[2] = (byte)y;
            this.serialPort.Write(cmd, 0, cmd.Length);
         }
      }

      public void SendText(string text)
      {
         if (false != this.active)
         {
            byte[] cmd = Encoding.UTF8.GetBytes(text);
            this.serialPort.Write(cmd, 0, cmd.Length);
         }
      }

      public void SetScreenHorizontalPositionOffset(int offset)
      {
         if (false != this.active)
         {
            byte[] cmd = new byte[2];
            cmd[0] = 0xEC;
            cmd[1] = (byte)(offset & 0x3F);
            this.serialPort.Write(cmd, 0, cmd.Length);
         }
      }

      public void SetScreenVerticalPositionOffset(int offset)
      {
         if (false != this.active)
         {
            byte[] cmd = new byte[2];
            cmd[0] = 0xED;
            cmd[1] = (byte)(offset & 0xFF);
            this.serialPort.Write(cmd, 0, cmd.Length);
         }
      }

      public void SetDateAndTime(DateTime time)
      {
         if (false != this.active)
         {
            string dateTime = string.Format("{0:D2}{1:D2}{2:D2}{3:D2}{4:D2}{5:D2}", time.Month, time.Day, time.Year % 100, time.Hour, time.Minute, time.Second);
            byte[] data = Encoding.UTF8.GetBytes(dateTime);

            byte[] cmd = new byte[13];
            cmd[0] = 0xEE;

            for (int i = 0; i < data.Length; i++)
            {
               cmd[i + 1] = data[i];
            }

            this.serialPort.Write(cmd, 0, cmd.Length);
         }
      }

      public void SetDateAndTimeFormat(DateFormat dateFormat, TimeFormat timeFormat)
      {
         if (false != this.active)
         {
            byte[] cmd = new byte[3];
            cmd[0] = 0xEF;
            cmd[1] = (byte)((DateFormat.ddmmyy == dateFormat) ? 0 : 1);
            cmd[2] = (byte)((TimeFormat.twentyFourHour == timeFormat) ? 0 : 1);
            this.serialPort.Write(cmd, 0, cmd.Length);
         }
      }

      public void SetTimeDisplayPosition(int x, int y)
      {
         if (false != this.active)
         {
            byte[] cmd = new byte[3];
            cmd[0] = 0xF0;
            cmd[1] = (byte)x;
            cmd[2] = (byte)y;
            this.serialPort.Write(cmd, 0, cmd.Length);
         }
      }

      public void SetDateDisplayPosition(int x, int y)
      {
         if (false != this.active)
         {
            byte[] cmd = new byte[3];
            cmd[0] = 0xF1;
            cmd[1] = (byte)x;
            cmd[2] = (byte)y;
            this.serialPort.Write(cmd, 0, cmd.Length);
         }
      }

      public void SetDateVisible(bool visible)
      {
         if (false != visible)
         {
            this.SetDateDisplayPosition(0, 0);
            this.WriteDateDisplayCommand(true);
         }
         else
         {
            this.WriteDateDisplayCommand(false);
            this.SetVideoChannel(0);
            this.SetCursorPosition(0, 0);
            this.SendText("        ");
         }
      }

      public void SetTimeVisible(bool visible)
      {
         if (false != visible)
         {
            this.SetTimeDisplayPosition(19, 0);
            this.WriteTimeDisplayCommand(true);
         }
         else
         {
            this.WriteTimeDisplayCommand(false);
            this.SetVideoChannel(0);
            this.SetCursorPosition(19, 0);
            this.SendText("        ");
         }
      }

      public void SetDescriptionVisible(bool visible)
      {
         if (visible != this.descriptionVisible)
         {
            this.descriptionVisible = visible;

            if (false != visible)
            {
               this.SetVideoChannel(0);

               this.SetCursorPosition(0, 3);
               this.SendText(this.descriptionText[0]);
               this.SetCursorPosition(0, 4);
               this.SendText(this.descriptionText[1]);
               this.SetCursorPosition(0, 5);
               this.SendText(this.descriptionText[2]);
               this.SetCursorPosition(0, 6);
               this.SendText(this.descriptionText[3]);
               this.SetCursorPosition(0, 7);
               this.SendText(this.descriptionText[4]);
               this.SetCursorPosition(0, 8);
               this.SendText(this.descriptionText[5]);
               this.SetCursorPosition(0, 9);
               this.SendText(this.descriptionText[6]);
               this.SetCursorPosition(0, 10);
               this.SendText(this.descriptionText[7]);
            }
            else
            {
               this.SetVideoChannel(0);

               this.SetCursorPosition(0, 3);
               this.SendText("                             ");
               this.SetCursorPosition(0, 4);
               this.SendText("                             ");
               this.SetCursorPosition(0, 5);
               this.SendText("                             ");
               this.SetCursorPosition(0, 6);
               this.SendText("                             ");
               this.SetCursorPosition(0, 7);
               this.SendText("                             ");
               this.SetCursorPosition(0, 8);
               this.SendText("                             ");
               this.SetCursorPosition(0, 9);
               this.SendText("                             ");
               this.SetCursorPosition(0, 10);
               this.SendText("                             ");
            }
         }
      }

      public void SetDescriptionText(int line, string text)
      {
         if ((line >= 1) && (line <= 8))
         {
            string result = "";
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 29; i++)
            {
               char ch = ' ';

               if ((null != text) && (i < text.Length))
               {
                  char x = text[i];

                  if ((x >= ' ') && (x <= '~'))
                  {
                     ch = x;
                  }
               }

               sb.Append(ch);
            }

            result = sb.ToString();

            int index = line - 1;

            if (result != this.descriptionText[index])
            {
               this.descriptionText[index] = result;

               if ((false != this.configured) &&
                   (false != this.descriptionVisible))
               {
                  this.SetVideoChannel(0);
                  this.SetCursorPosition(0, 3 + index);
                  this.SendText(result);
               }
            }
         }
      }

      public void SetCameraIdVisible(bool visible)
      {
         this.cameraIdVisible = visible;

         if (false != visible)
         {
            this.SetVideoChannel(1);
            this.SetCursorPosition(0, 1);
            this.SendText(this.channel1CameraId);

            this.SetVideoChannel(2);
            this.SetCursorPosition(0, 1);
            this.SendText(this.channel2CameraId);
         }
         else
         {
            this.SetVideoChannel(1);
            this.SetCursorPosition(0, 1);
            this.SendText("                             ");

            this.SetVideoChannel(2);
            this.SetCursorPosition(0, 1);
            this.SendText("                             ");

            this.SetVideoChannel(3);
            this.SetCursorPosition(0, 1);
            this.SendText("                             ");
         }
      }

      public void SetCameraIdText(int channel, string text)
      {
         bool issueCommand = false;
         string result = "";

         if (null != text)
         {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
               char ch = text[i];

               if ((ch >= ' ') && (ch <= '~'))
               {
                  sb.Append(ch);
               }
            }

            text = sb.ToString();

            if (text.Length > 29)
            {
               result = text.Substring(0, 29);
            }
            else
            {
               result += text;

               for (int i = text.Length; i < 29; i++)
               {
                  result += " ";
               }
            }
         }

         if (1 == channel)
         {
            if (result != this.channel1CameraId)
            {
               this.channel1CameraId = result;
               issueCommand = true;
            }
         }
         else if (2 == channel)
         {
            if (result != this.channel2CameraId)
            {
               this.channel2CameraId = result;
               issueCommand = true;
            }
         }

         if ((false != this.configured) &&
             (false != issueCommand) &&
             (false != this.cameraIdVisible))
         {
            this.SetVideoChannel(channel);
            this.SetCursorPosition(0, 1);
            this.SendText(result);
         }
      }

      public void IncreaseVeriticalOffset(OsdParameters osdParameters)
      {
         if (osdParameters.VerticalOffset > 0)
         {
            osdParameters.VerticalOffset--;
            this.SetScreenVerticalPositionOffset(osdParameters.VerticalOffset);
         }
      }

      public void DecreaseVeriticalOffset(OsdParameters osdParameters)
      {
         if (osdParameters.VerticalOffset < 31)
         {
            osdParameters.VerticalOffset++;
            this.SetScreenVerticalPositionOffset(osdParameters.VerticalOffset);
         }
      }

      public void DecreaseHorizontalOffset(OsdParameters osdParameters)
      {
         if (osdParameters.HorizontalOffset > 0)
         {
            osdParameters.HorizontalOffset--;
            this.SetScreenHorizontalPositionOffset(osdParameters.HorizontalOffset);
         }
      }

      public void IncreaseHorizontalOffset(OsdParameters osdParameters)
      {
         if (osdParameters.HorizontalOffset < 63)
         {
            osdParameters.HorizontalOffset++;
            this.SetScreenHorizontalPositionOffset(osdParameters.HorizontalOffset);
         }
      }

      #endregion
   }
}
