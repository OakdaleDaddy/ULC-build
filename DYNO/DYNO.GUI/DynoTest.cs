namespace DYNO.GUI
{
   using System;
   using System.Collections;
   using System.IO;
   using System.Text;
   using System.Threading;

   using DYNO.PCANLight;
   using DYNO.CAN;
   using DYNO.Utilities;
   /*
Test Process:12 hours
 - uses separate device thread for CAN communications
 - uses separate thread for test control and logging.
 - communicates to 3 CAN devices
 - uses CAN interface to communicate to UUT (motor)
 - writes UUT for wheel speed and direction
 - reads UUT for motor speed and direction
 - reads UUT for motor current
 - reads UUT for temperature
 - uses CAN interface to communicate to Encoder
 - reads Encoder for distance
 - reads Encoder for speed
 - uses CAN interface to communicate to AnalogIO
 - reads AnalogIO for motor current
 - writes AnalogIO for wheel load
 - log stores values every second with relative time on one line separated by commas
 - log stores retrieved UUT speed and direction
 - log stores retrieved UUT current
 - log stores retrieved UUT temperature
 - log stores retrieved Encoder distance
 - log stores retrieved Encoder speed
 - log stores retrieved AnalogIO motor current
 - log stores set AnalogIO wheel load
    * */

   public class DynoTest
   {
      #region Definitions

      public delegate void CompleteHandler();
      private delegate void ProcessHandler();

      #endregion

      #region Fields

      private BusParameters busParameters;
      private TestParameters testParameters;

      private bool completeEarly;
      private bool execute;
      private Thread thread;

      private bool executeDevice;
      private Thread deviceThread;

      private UlcRoboticsNicbotWheel uut;
      private KublerRotaryEncoder encoder;
      private PeakAnalogIo analogIo;
      private PeakDigitalIo digitalIo;

      private ArrayList deviceList;
      private Queue busReceiveQueue;

      private TextWriter logWriter;
      private DateTime startTime;
      private DateTime lastDataTime;
      private string completionCause;

      #endregion

      #region Properties

      public CompleteHandler OnComplete;
      private ProcessHandler Process { set; get; }

      #endregion

      #region Delegates

      private void Complete()
      {
         if (null != this.OnComplete)
         {
            this.OnComplete();
         }
      }

      private void BusReceiveHandler(CanFrame frame)
      {
         lock (this)
         {
            this.busReceiveQueue.Enqueue(frame);
         }
      }

      private void DeviceTraceReceive(int cobId, byte[] data)
      {
         StringBuilder sb = new StringBuilder();

         for (int i = 0; i < data.Length; i++)
         {
            sb.AppendFormat("{0:X2}", data[i]);
         }

         Tracer.WriteMedium(TraceGroup.DEVICE, "", "rx {0:X3} {1}", cobId, sb.ToString());
      }

      private void DeviceTraceTransmit(int cobId, byte[] data)
      {
         StringBuilder sb = new StringBuilder();

         for (int i = 0; i < data.Length; i++)
         {
            sb.AppendFormat("{0:X2}", data[i]);
         }

         Tracer.WriteMedium(TraceGroup.DEVICE, "", "tx {0:X3} {1}", cobId, sb.ToString());
      }

      private bool DeviceTransmit(int id, byte[] data)
      {
         StringBuilder sb = new StringBuilder();

         for (int i = 0; i < data.Length; i++)
         {
            sb.AppendFormat("{0:X2}", data[i]);
         }

         CANResult transmitResult = PCANLight.Send(this.busParameters.BusInterface, id, data);
         bool result = (transmitResult == CANResult.ERR_OK) ? true : false;

         if (false != result)
         {
            Tracer.WriteMedium(TraceGroup.TEST, "", "tx {0:X3} {1}", id, sb.ToString());
         }

         return (result);
      }

      private void DeviceError(string name, int nodeId, string reason)
      {
         Tracer.WriteError(TraceGroup.TEST, "", "fault with \"{0}\", node={1}, reason={2}", name, nodeId, reason);
      }

      #endregion

      #region Helper Functions

      private string GenerateFileName()
      {
         DateTime dt = DateTime.Now;
         string result = string.Format("{0:d4}{1:d2}{2:d2}", dt.Year, dt.Month, dt.Day);
         return (result);
      }

      private bool CreateLogFile()
      {
         bool result = true;
         string logFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\TestLogs\\";

         if (Directory.Exists(logFilePath) == false)
         {
            Directory.CreateDirectory(logFilePath);
         }

         if (Directory.Exists(logFilePath) == false)
         {
            result = false;
            Tracer.WriteError(TraceGroup.TEST, "", "unable to create log folder");
         }
         else
         {
            string logPathName = logFilePath + "\\" + "DYNOLOG_" + this.GenerateFileName();
            int logFileCount = 0;

            for (int i = 0; ; i++)
            {
               if (File.Exists(logPathName + "-" + i + ".txt") == false)
               {
                  logFileCount = i;
                  break;
               }
            }

            this.logWriter = new StreamWriter(logPathName + "-" + logFileCount + ".txt", false);
         }

         return (result);
      }

      public void CloseLogFile()
      {
         if (null != this.logWriter)
         {
            this.logWriter.Close();
            this.logWriter.Dispose();
            this.logWriter = null;
         }
      }

      public void Log(string formatString, params object[] args)
      {
         string logString = string.Format(formatString, args);
         Tracer.Write(TraceGroup.LOG, logString);

         if (null != this.logWriter)
         {
            DateTime dt = DateTime.Now;
            string timeString = string.Format("{0:D2}/{1:D2}/{2:D2} {3:D2}:{4:D2}:{5:D2}.{6:D3} ", dt.Month, dt.Day, (dt.Year % 100), dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
            string traceString = timeString + logString;

            this.logWriter.WriteLine(traceString);
            this.logWriter.Flush();
         }
      }

      #endregion

      #region Device Process 

      private void ProcessCommFrames()
      {
         int receiveCount = 0;
         CanFrame frame = null;

         do
         {
            lock (this)
            {
               receiveCount = this.busReceiveQueue.Count;

               if (receiveCount > 0)
               {
                  frame = (CanFrame)this.busReceiveQueue.Dequeue();
               }
            }

            if (null != frame)
            {
               bool handled = false;

               foreach (Device device in this.deviceList)
               {
                  handled |= device.Update((int)frame.cobId, frame.data);
               }

               if (false == handled)
               {
                  this.DeviceTraceReceive((int)frame.cobId, frame.data);
               }

               frame = null;
            }
         }
         while (0 != receiveCount);
      }

      private void DeviceProcess()
      {
         this.uut.NodeId = (byte)this.busParameters.UutId;
         this.encoder.NodeId = (byte)this.busParameters.EncoderId;
         this.analogIo.NodeId = (byte)this.busParameters.AnalogIoId;
         this.digitalIo.NodeId = (byte)this.busParameters.DigialIoId;

         this.uut.TraceMask = 0xFF;
         this.encoder.TraceMask = 0xFF;
         this.analogIo.TraceMask = 0xFF;
         this.digitalIo.TraceMask = 0xFF;

         foreach (Device device in this.deviceList)
         {
            device.Initialize();
         }

         for (; this.executeDevice; )
         {
            this.ProcessCommFrames();

            foreach (Device device in this.deviceList)
            {
               device.Update();
            }

            Thread.Sleep(1);
         }
      }

      private bool StartBus()
      {
         bool result = true;

         this.executeDevice = true;
         this.deviceThread = new Thread(this.DeviceProcess);
         this.deviceThread.IsBackground = true;
         this.deviceThread.Name = "CAN Devices";
         this.deviceThread.Start();

         if (false != result)
         {
            CANResult startResult = PCANLight.Start(this.busParameters.BusInterface, this.busParameters.BitRate, FramesType.INIT_TYPE_ST, TraceGroup.CANBUS, this.BusReceiveHandler);

            if (CANResult.ERR_OK != startResult)
            {
               this.completionCause = "CAN interface failure";
               result = false;
            }
         }

         if (false != result)
         {
            PCANLight.ResetBus(this.busParameters.BusInterface);

            DateTime busStartLimit = DateTime.Now.AddMilliseconds(2000);

            for (; this.execute; )
            {
               bool allBooted = true;

               foreach (Device device in this.deviceList)
               {
                  allBooted = allBooted && device.ReceiveBootupHeartbeat;

                  if (false == allBooted)
                  {
                     break;
                  }
               }

               if ((false != allBooted) || (DateTime.Now > busStartLimit))
               {
                  break;
               }

               Thread.Sleep(1);
            }

            foreach (Device device in this.deviceList)
            {
               if (false == device.ReceiveBootupHeartbeat)
               {
                  device.Fault("boot timeout");

                  if (false != result)
                  {
                     this.completionCause = device.Name + " offline";
                     result = false;
                  }
               }
            }
         }

         if (false != result)
         {
            this.uut.Configure();
            this.uut.Start();

            this.encoder.Configure();
            this.encoder.Start();

            this.analogIo.Configure();
            this.analogIo.Start();

            this.digitalIo.Configure();
            this.digitalIo.Start();
         }

         return (result);
      }

      private void StopBus()
      {
         foreach (Device device in this.deviceList)
         {
            device.Stop();
         }

         Thread.Sleep(100); // wait for device to stop

         this.executeDevice = false;
         this.deviceThread.Join(3000);
         this.deviceThread = null;

         PCANLight.Stop(this.busParameters.BusInterface);
      }

      #endregion

      #region Process Functions

      private void StartTest()
      {
         bool result = true;

         Tracer.WriteMedium(TraceGroup.TEST, "", "test start");
         this.completionCause = null;
         this.startTime = DateTime.Now;
         this.lastDataTime = this.startTime;

         if (false != result)
         {
            result = this.CreateLogFile();

            if (false == result)
            {
               this.completionCause = "log creation failure";
            }
         }

         this.Log("DYNO TEST");
         this.Log(" - date = {0:D2}-{1:D2}-{2:D2}", this.startTime.Month, this.startTime.Day, this.startTime.Year);
         this.Log(" - time = {0:D2}:{1:D2}:{2:D2}", this.startTime.Hour, this.startTime.Minute, this.startTime.Second);
         this.Log(" - wheel speed = {0}", this.testParameters.WheelSpeed);
         this.Log(" - wheel start load = {0}", this.testParameters.WheelStartLoad);
         this.Log(" - wheel end load = {0}", this.testParameters.WheelStopLoad);
         this.Log(" - time limit = {0}", this.testParameters.RunTime);
         this.Log(" - current limit = {0}", this.testParameters.CurrentLimit);
         this.Log(" - thermal limit = {0}", this.testParameters.ThermalLimit);
         this.Log(" - slippage limit = {0}", this.testParameters.SlippageLimit);
         this.Log("BEGIN");

         if (false != result)
         {
            result = this.StartBus();
         }

         if (false != result)
         {
            this.Process = this.RunTest;
         }
         else
         {
            this.Process = this.CompleteTest;
         }
      }

      private void RunTest()
      {
         DateTime currentTime = DateTime.Now;
         TimeSpan totalRunSpan = currentTime - this.startTime;
         TimeSpan dataRunSpan = currentTime - this.lastDataTime;

         if (dataRunSpan.TotalMilliseconds >= 1000)
         {
            this.lastDataTime = currentTime;
            this.Log("data");
         }

         if (false != completeEarly)
         {
            this.Process = this.CompleteTest;
            this.completionCause = "user abort";
         }
         else if (totalRunSpan.TotalSeconds >= this.testParameters.RunTime)
         {
            this.Process = this.CompleteTest;
            this.completionCause = "time limit exceeded";
         }
      }

      private void CompleteTest()
      {
         this.Log("END");
         TimeSpan ts = DateTime.Now - this.startTime;
         this.Log(" - completion cause = {0}", this.completionCause);
         this.Log(" - runtime = {0}", ts.TotalSeconds);

         Tracer.WriteMedium(TraceGroup.TEST, "", "test complete");
         this.execute = false;

         this.Process = this.TestDone;
      }

      private void TestDone()
      {
      }

      private void TestProcess()
      {
         this.Process = this.StartTest;

         for (; execute; )
         {
            this.Process();
            Thread.Sleep(1);
         }

         this.StopBus();
         this.CloseLogFile();

         Tracer.WriteMedium(TraceGroup.TEST, "", "test stop");
         this.Complete();
      }

      #endregion

      #region Constructor

      public DynoTest()
      {
         this.uut = new UlcRoboticsNicbotWheel("uut", (byte)1);
         this.encoder = new KublerRotaryEncoder("encoder", (byte)2);
         this.analogIo = new PeakAnalogIo("analog IO", (byte)3);
         this.digitalIo = new PeakDigitalIo("digital IO", (byte)4);
         
         this.busReceiveQueue = new Queue();
         this.deviceList = new ArrayList();
         this.deviceList.Add(this.digitalIo);
         this.deviceList.Add(this.analogIo);
         this.deviceList.Add(this.encoder);
         this.deviceList.Add(this.uut);

         foreach (Device device in this.deviceList)
         {
            device.OnReceiveTrace = new Device.ReceiveTraceHandler(this.DeviceTraceReceive);
            device.OnTransmitTrace = new Device.TransmitTraceHandler(this.DeviceTraceTransmit);
            device.OnTransmit = new Device.TransmitHandler(this.DeviceTransmit);
            device.OnCommError = new Device.CommErrorHandler(this.DeviceError);
         }
      }

      #endregion

      #region Access Methods

      public void Start(BusParameters busParameters, TestParameters testParameters, ref string result)
      {
         if (null == result)
         {
            this.thread = new Thread(this.TestProcess);
            this.thread.IsBackground = true;
            this.thread.Name = "Test Process";

            this.busParameters = busParameters;
            this.testParameters = testParameters;

            this.completeEarly = false;
            this.execute = true;
            this.thread.Start();
         }
      }

      public void Stop()
      {
         if (null != this.thread)
         {
            this.completeEarly = true;
            bool closed = this.thread.Join(3000);

            if (false == closed)
            {
               this.execute = false;
               this.thread.Join(3000);
            }

            this.thread = null;
         }
      }

      #endregion
   }

}