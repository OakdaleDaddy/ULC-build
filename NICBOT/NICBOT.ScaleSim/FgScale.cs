namespace NICBOT.ScaleSim
{
   using System;
   using System.IO.Ports;
   using System.Text;
   using System.Threading;

   public class FgScale
   {
      #region Fields

      private static FgScale instance = null;
      private SerialPort serialPort;
      private byte[] serialPortRxBuffer;

      private byte[] serialPortProcessBuffer;
      private int serialPortProcessCount;

      private int mode;
      private int unit;
      private bool active;

      private bool execute;
      private Thread thread;

      private bool outOfRange;
      private double reading;


      #endregion

      #region Properties

      public static FgScale Instance
      {
         get
         {
            if (null == instance)
            {
               instance = new FgScale();
               instance.Initialize();
            }

            return instance;
         }
      }

      #endregion

      #region Helper Functions

      private string GetReadingText()
      {
         string result = "";
         string unit = "";

         if (0 == this.unit)
         {
            unit = " lb";
         }
         else if (1 == this.unit)
         {
            unit = " oz";
         }
         else
         {
            unit = " kg";
         }

         if (false == this.outOfRange)
         {
            char signChar = (this.reading < 0) ? '-' : '+';

            if (1 == this.unit)
            {
               result = string.Format("ST,{0}{1:000000.0}{2}\r\n", signChar, Math.Abs(this.reading), unit);
            }
            else
            {
               result = string.Format("ST,{0}{1:00000.00}{2}\r\n", signChar, Math.Abs(this.reading), unit);
            }
         }
         else
         {
            if (1 == this.unit)
            {
               result = string.Format("OL,+999999.9{0}\r\n", unit);
            }
            else
            {
               result = string.Format("OL,+99999.99{0}\r\n", unit);
            }
         }

         return (result);
      }

      #endregion

      #region Delegates

      private void SerialPortRxDataHandler(object o, SerialDataReceivedEventArgs args)
      {
         if (SerialData.Chars == args.EventType)
         {
            int receiveCount = this.serialPort.Read(this.serialPortRxBuffer, 0, this.serialPortRxBuffer.Length);

            for (int i = 0; i< receiveCount ;i++)            
            {
               if (this.serialPortProcessCount < this.serialPortProcessBuffer.Length)
               {
                  byte ch = this.serialPortRxBuffer[i];
                  this.serialPortProcessBuffer[this.serialPortProcessCount] = ch;
                  this.serialPortProcessCount++;

                  if ('\n' == ch)
                  {
                     if (1 == this.mode)
                     {
                        if ('Q' == this.serialPortProcessBuffer[0])
                        {
                           string readingText = this.GetReadingText();
                           this.serialPort.Write(readingText);
                        }
                        else if ('Z' == this.serialPortProcessBuffer[0])
                        {
                           this.serialPort.Write("Z\r\n");
                        }
                        else if ('T' == this.serialPortProcessBuffer[0])
                        {
                           this.serialPort.Write("T\r\n");
                        }
                        else
                        {
                           this.serialPort.Write("?\r\n");
                        }
                     }

                     this.serialPortProcessCount = 0;
                  }
               }
            }
         }
      }

      #endregion

      #region Process Functions

      private void Process()
      {
         DateTime streamOutputTimeLimit = DateTime.Now.AddMilliseconds(100);

         for (; execute; )
         {
            if (0 == this.mode)
            {
               if (DateTime.Now > streamOutputTimeLimit)
               {
                  streamOutputTimeLimit = streamOutputTimeLimit.AddMilliseconds(100);
                  string readingText = this.GetReadingText();
                  this.serialPort.Write(readingText);
               }
            }

            Thread.Sleep(1);
         }
      }

      #endregion

      #region Constructor

      private FgScale()
      {
         this.serialPort = new SerialPort();

         this.serialPort.Parity = Parity.Even;
         this.serialPort.DataBits = 7;
         this.serialPort.StopBits = System.IO.Ports.StopBits.One;
         this.serialPort.Handshake = Handshake.None;
         this.serialPort.Encoding = Encoding.UTF8;
         this.serialPort.RtsEnable = true;
         this.serialPort.DataReceived += new SerialDataReceivedEventHandler(this.SerialPortRxDataHandler);

         this.serialPortRxBuffer = new byte[256];
         this.serialPortProcessBuffer = new byte[16];
      }

      #endregion

      #region Access Methods

      public void Initialize()
      {
      }

      public void Start(int port, int baud, int mode, int unit, ref string result)
      {
         if (null == result)
         {
            string serialPortName = "COM" + port.ToString();
            this.serialPort.PortName = serialPortName;
            this.serialPort.BaudRate = baud;

            try
            {
               this.serialPort.Open();
               this.active = true;

               this.mode = mode;
               this.unit = unit;

               this.thread = new Thread(this.Process);
               this.thread.IsBackground = true;
               this.thread.Name = "FG Scale";

               this.execute = true;
               this.thread.Start();
            }
            catch
            {
               result = "Unable to open port";
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
         }
      }

      public void SetOutOfRange(bool outOfRange)
      {
         this.outOfRange = outOfRange;
      }

      public void SetReading(double reading)
      {
         this.reading = reading;
         this.outOfRange = false;
      }

      #endregion
   }

}