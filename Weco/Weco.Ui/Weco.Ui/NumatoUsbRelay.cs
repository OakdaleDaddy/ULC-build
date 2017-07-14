
namespace Weco.Ui
{
   using System;
   using System.Collections;
   using System.IO.Ports;
   using System.Text;
   using System.Threading;

   using Weco.Utilities;

   public class NumatoUsbRelay
   {
      #region Definition

      private class Command
      {
         public int Channel { set; get; }
         public bool State { set; get; }

         public Command()
         {
         }

         public Command(int channel, bool state)
         {
            this.Channel = channel;
            this.State = state;
         }
      }

      #endregion

      #region Fields

      private static NumatoUsbRelay instance = null;

      private bool active;

      private SerialPort serialPort;
      private byte[] serialPortRxBuffer;

      private byte[] serialPortProcessBuffer;

      private DateTime serialVersionTimeLimit;
      private string faultReason;

      private bool execute;
      private AutoResetEvent executeEvent;
      private Thread thread;
      private bool readInitialState;
      private Queue commandQueue;
      private bool[] channelStates;
      private bool[] channelChanged;

      private bool responsePending;
      private DateTime responseTimeLimit;

      #endregion

      #region Properties

      public static NumatoUsbRelay Instance
      {
         get
         {
            if (null == instance)
            {
               instance = new NumatoUsbRelay();
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
         StringBuilder sb = new StringBuilder();
         string tag = "";
         string value = "";
         string response = Encoding.UTF8.GetString(buffer, 0, count);

         for (int i = 0; i < count; i++)
         {
            char ch = (char)buffer[i];

            if ('\n' == ch)
            {
               if ("" == tag)
               {
                  tag = sb.ToString();
               }
               else
               {
                  value = sb.ToString();
               }

               sb.Clear();
            }
            else if ('\r' != ch)
            {
               sb.Append(ch);
            }

            if ('>' == ch)
            {
               if ("relay readall" == tag)
               {
                  int initialState = 0;

                  if (int.TryParse(value, System.Globalization.NumberStyles.HexNumber, null, out initialState) != false)
                  {
                     for (int channelStateIndex = 0; channelStateIndex < this.channelStates.Length; channelStateIndex++)
                     {
                        int mask = 1 << channelStateIndex;
                        this.channelStates[channelStateIndex] = ((initialState & mask) != 0) ? true: false;
                     }
                  }

                  this.readInitialState = false;
               }

               this.responsePending = false;
            }
         }
      }

      private void WriteCommand(string command)
      {
         byte[] commandData = Encoding.UTF8.GetBytes(command);

         if (false != this.serialPort.IsOpen)
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
         this.readInitialState = true;
         this.responsePending = false;
         this.serialVersionTimeLimit = DateTime.Now.AddMilliseconds(-1);

         for (; execute; )
         {
            if (false == this.responsePending)
            {
               int commandCount = 0;

               commandCount = this.commandQueue.Count;

               if (false != this.readInitialState)
               {
                  this.WriteCommand("relay readall\r");
                  this.responsePending = true;
                  this.responseTimeLimit = DateTime.Now.AddMilliseconds(500);
               }
               else if (0 != commandCount)
               {
                  Command command = (Command)this.commandQueue.Dequeue();

                  if ((null != command) &&
                      (command.Channel >= 0) &&
                      (command.Channel <= 3))
                  {
                     string stateString = (command.State) ? "on" : "off";
                     string commandString = string.Format("relay {0} {1}\r", stateString, command.Channel);

                     this.responsePending = true;
                     this.responseTimeLimit = DateTime.Now.AddMilliseconds(500);
                     this.WriteCommand(commandString);

                     this.channelStates[command.Channel] = command.State;
                     this.channelChanged[command.Channel] = true;
                  }
               }
               else if (DateTime.Now > this.serialVersionTimeLimit)
               {
                  this.serialVersionTimeLimit = this.serialVersionTimeLimit.AddMilliseconds(30000);

                  this.responsePending = true;
                  this.responseTimeLimit = DateTime.Now.AddMilliseconds(500);
                  this.WriteCommand("ver\r");
               }
            }

            if (null == this.faultReason)
            {
               if ((false != this.responsePending) &&
                   (DateTime.Now > this.responseTimeLimit))
               {
                  Tracer.WriteMedium(TraceGroup.TBUS, "", "relay communication failure");
                  this.faultReason = "comm timeout";
                  this.responsePending = false;
               }
            }

            this.executeEvent.WaitOne(50);
         }
      }

      #endregion

      #region Constructor

      private NumatoUsbRelay()
      {
         this.serialPort = new SerialPort();

         this.serialPort.Parity = Parity.Even;
         this.serialPort.DataBits = 8;
         this.serialPort.StopBits = System.IO.Ports.StopBits.One;
         this.serialPort.Handshake = Handshake.None;
         this.serialPort.Encoding = Encoding.UTF8;
         this.serialPort.RtsEnable = true;
         this.serialPort.DataReceived += new SerialDataReceivedEventHandler(this.SerialPortRxDataHandler);

         this.serialPortRxBuffer = new byte[256];
         this.serialPortProcessBuffer = new byte[32];

         this.executeEvent = new AutoResetEvent(false);
         this.commandQueue = new Queue();
         this.channelStates = new bool[4];
         this.channelChanged = new bool[4];
      }

      #endregion

      #region Access Methods

      public void Initialize()
      {
         this.active = false;
         this.faultReason = null;
      }

      public void Start(int port)
      {
         if (port > 0)
         {
            if (false == this.active)
            {
               string serialPortName = "COM" + port.ToString();
               this.serialPort.PortName = serialPortName;
               this.serialPort.BaudRate = 115200;

               try
               {
                  this.serialPort.Open();
                  this.active = true;

                  this.thread = new Thread(this.Process);
                  this.thread.IsBackground = true;
                  this.thread.Name = "Numato Relay";

                  this.commandQueue.Clear();

                  this.execute = true;
                  this.thread.Start();
               }
               catch
               {
                  this.faultReason = "unable to open port";
               }
            }
         }
         else
         {
            this.faultReason = "disabled";
         }
      }

      public void Stop()
      {
         if (false != this.active)
         {
            this.execute = false;
            this.executeEvent.Set();
            this.thread.Join(3000);

            this.thread = null;

            this.serialPort.Close();

            this.active = false;
            this.faultReason = "closed";
         }
      }

      public void SetRelay(int channel, bool state)
      {
         this.commandQueue.Enqueue(new Command(channel, state));
         this.executeEvent.Set();
      }

      public bool GetRelayState(int channel)
      {
         bool result = false;
         
         if ((channel >= 0) &&
             (channel <= 3))
         {
            result = this.channelStates[channel];
         }

         return (result);
      }

      public void ResetChanged()
      {
         for (int i = 0; i < this.channelChanged.Length; i++)
         {
            this.channelChanged[i] = false;
         }
      }

      public bool GetChanged(int channel)
      {
         bool result = false;
         
         if ((channel >= 0) &&
             (channel <= 3))
         {
            result = this.channelChanged[channel];
         }

         return (result);
      }

      #endregion

   }
}
