namespace NICBOT.GUI
{
   using System;
   using System.IO.Ports;
   using System.Text;
   using System.Threading;

   using NICBOT.Utilities;

   public class FgDigitalScale
   {
      #region Definition

      public delegate void SerialWriteHandler(byte[] data, int offset, int count);

      #endregion

      #region Fields

      private static FgDigitalScale front = null;
      private static FgDigitalScale rear = null;

      private bool active;
      
      private SerialPort serialPort;
      private byte[] serialPortRxBuffer;

      private SerialWriteHandler serialWriteHandler;

      private byte[] serialPortProcessBuffer;
      private int serialPortProcessCount;

      private DateTime serialSampleTimeLimit;
      private DateTime serialRxTimeLimit;
      private string faultReason;

      private bool execute;
      private Thread thread;

      private double reading;

      #endregion

      #region Properties

      public static FgDigitalScale Front
      {
         get
         {
            if (null == front)
            {
               front = new FgDigitalScale();
               front.Initialize();
            }

            return front;
         }
      }

      public static FgDigitalScale Rear
      {
         get
         {
            if (null == rear)
            {
               rear = new FgDigitalScale();
               rear.Initialize();
            }

            return rear;
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

               if ('\n' == ch)
               {
                  if (this.serialPortProcessCount >= 6)
                  {
                     if (('S' == this.serialPortProcessBuffer[0]) &&
                         ('T' == this.serialPortProcessBuffer[1]))
                     {
                        if (('k' == this.serialPortProcessBuffer[this.serialPortProcessCount - 4]) &&
                            ('g' == this.serialPortProcessBuffer[this.serialPortProcessCount - 3]))
                        {
                           double value = 0;
                           string response = Encoding.UTF8.GetString(this.serialPortProcessBuffer, 3, 9);

                           if (double.TryParse(response, out value) != false)
                           {
                              this.reading = value;
                              this.faultReason = null;
                              this.serialRxTimeLimit = DateTime.Now.AddMilliseconds(2000);
                           }
                        }
                        else
                        {
                           if (null == this.faultReason)
                           {
                              this.faultReason = "invalid unit";
                           }
                        }
                     }
                     else if (('O' == this.serialPortProcessBuffer[0]) &&
                              ('L' == this.serialPortProcessBuffer[1]))
                     {
                        if (null == this.faultReason)
                        {
                           this.faultReason = "out of range";
                        }
                     }
                  }

                  this.serialPortProcessCount = 0;
               }
            }
         }
      }

      private void WriteCommand(string command)
      {
         byte[] commandData = Encoding.UTF8.GetBytes(command);

         if (null != this.serialWriteHandler)
         {
            this.serialWriteHandler(commandData, 0, commandData.Length);
         }
         else if (false != this.serialPort.IsOpen)
         {
            this.serialPort.Write(commandData, 0, commandData.Length);
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
         this.serialSampleTimeLimit = DateTime.Now.AddMilliseconds(500);
         this.serialRxTimeLimit = DateTime.Now.AddMilliseconds(2000);

         for (; execute; )
         {
            if (DateTime.Now > this.serialSampleTimeLimit)
            {
               this.serialSampleTimeLimit = this.serialSampleTimeLimit.AddMilliseconds(500);
               this.WriteCommand("Q\r\n");
            }
            
            if (null == this.faultReason)
            {
               if (DateTime.Now > this.serialRxTimeLimit)
               {
                  this.faultReason = "comm timeout";
               }
            }

            Thread.Sleep(50);
         }
      }

      #endregion

      #region Constructor

      private FgDigitalScale()
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
         this.serialPortProcessBuffer = new byte[32];
      }

      #endregion

      #region Access Methods

      public void Initialize()
      {
         this.active = false;
         this.faultReason = null;
      }

      public void Start(int port, int rate)
      {
         if (false == this.active)
         {
            this.serialWriteHandler = null;

            string serialPortName = "COM" + port.ToString();
            this.serialPort.PortName = serialPortName;
            this.serialPort.BaudRate = rate;

            try
            {
               this.serialPort.Open();
               this.active = true;

               this.thread = new Thread(this.Process);
               this.thread.IsBackground = true;
               this.thread.Name = "FG Scale";

               this.execute = true;
               this.thread.Start();
            }
            catch
            {
               this.faultReason = "unable to open port";
            }
         }
      }

      public void Start(SerialWriteHandler writeHandler)
      {
         if (false == this.active)
         {
            this.serialWriteHandler = writeHandler;

            try
            {
               this.active = true;

               this.thread = new Thread(this.Process);
               this.thread.IsBackground = true;
               this.thread.Name = "FG Scale";

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

            if (null != this.serialWriteHandler)
            {
               this.serialWriteHandler = null;
            }
            else
            {
               this.serialPort.Close();
            }

            this.active = false;
            this.faultReason = "closed";
         }
      }

      public void Receive(byte[] receiveBuffer, int receiveCount)
      {
         if (null != serialWriteHandler)
         {
            this.ProcessRxData(receiveBuffer, receiveCount);
         }
      }

      public double GetReading()
      {
         double result = double.NaN;
         
         if (null == this.faultReason)
         {
            result = this.reading;
         }

         return (result);
      }

      #endregion

   }
}