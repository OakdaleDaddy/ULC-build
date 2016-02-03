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

   public class DynoTest
   {
      #region Definitions

      public delegate void CompleteHandler();
      private delegate void ProcessHandler();

      #endregion

      #region Fields

      private SetupParameters busParameters;
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
      private DateTime stateTimeLimit;
      private DateTime startTime;
      private DateTime currentTime;
      private string completionCause;

      private double wheelLoad;
      private double loadChangePerSecond;
      private double secondLimit;

      private double lastEncoderRotations;
      private DateTime lastEncoderTime;

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
         CANResult transmitResult = PCANLight.Send(this.busParameters.BusInterface, id, data);
         bool result = (transmitResult == CANResult.ERR_OK) ? true : false;
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
            this.logWriter.WriteLine(logString);
            this.logWriter.Flush();
         }
      }

      public void LogData()
      {
         TimeSpan intervalTimeSpan = this.currentTime - this.startTime;

         double elapsedTime = intervalTimeSpan.TotalSeconds;
         double intervalLoad = this.wheelLoad;
         double uutSpeed = ((double)this.uut.RPM) / this.busParameters.UutSpeedToRpm;
         double uutCurrent = (double)this.uut.Torque;
         double uutTemperature = (double)this.uut.Temperature;

         double supplyCurrentVoltage = ((double)this.analogIo.AnalogIn0 * 10) / 32767;
         double supplyCurrent = supplyCurrentVoltage * this.busParameters.SupplyVoltageToAmps;

         TimeSpan encoderTimeSpan = this.currentTime - this.lastEncoderTime;
         this.lastEncoderTime = this.currentTime;
         double currentEncodeRotations = this.encoder.Rotations;
         double rotationsPerInterval = currentEncodeRotations - this.lastEncoderRotations;
         double encoderRpm = (rotationsPerInterval / encoderTimeSpan.TotalSeconds) * 60;
         double bodySpeed = encoderRpm / this.busParameters.BodySpeedToRpm;

         string slippageString = "---";
         double slippageLimit = 0;

         if (0 != uutSpeed)
         {
            double slippage = double.NaN;
            slippage = ((uutSpeed - bodySpeed) / uutSpeed) * 100;
            slippageString = string.Format("{0:0}%", slippage);

            slippageLimit = uutSpeed * (100 - this.testParameters.SlippageLimit);
         }

         // this.Log("TIME, LOAD, UUT-SPEED, UUT-CURRENT, UUT-TEMPERATURE, SUPPLY-CURRENT, BODY-SPEED, SLIPPAGE");
         this.Log("{0,9:0.0}, {1,8:0.0}, {2,15:0.0}, {3,15:0.0}, {4,15:0.0}, {5,15:0.0}, {6,15:0.0}, {7,15}", elapsedTime, intervalLoad, uutSpeed, uutCurrent, uutTemperature, supplyCurrent, bodySpeed, slippageString);

         if (uutTemperature > this.testParameters.ThermalLimit)
         {
            this.completionCause = "uut temperature exceeded limit";
         }
         else if (uutCurrent > this.testParameters.CurrentLimit)
         {
            this.completionCause = "uut current exceeded limit";
         }
         else if (supplyCurrent > this.testParameters.CurrentLimit)
         {
            this.completionCause = "supply current exceeded limit";
         }
         else if (bodySpeed < slippageLimit)
         {
            this.completionCause = "slippage exceeded limit";
         }
      }

      public void RampLoad()
      {
         TimeSpan testTimeSpan = this.currentTime - this.startTime;
         this.wheelLoad = this.testParameters.WheelStartLoad + (this.loadChangePerSecond * testTimeSpan.TotalSeconds);

         double loadVoltage = (this.wheelLoad * this.busParameters.VoltsPerPounds);

         if (loadVoltage > 10)
         {
            loadVoltage = 10;
         }

         UInt16 loadSetPoint = (UInt16)(loadVoltage * 4095.0 / 10.0);
         Tracer.WriteMedium(TraceGroup.TEST, "", "load lb={0:0.0}, set={1}", this.wheelLoad, loadSetPoint);
         this.analogIo.SetOutput(0, loadSetPoint);
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
         this.startTime = DateTime.Now; // set to give valid start time

         if (this.testParameters.RunTime > 1)
         {
            this.loadChangePerSecond = (this.testParameters.WheelStopLoad - this.testParameters.WheelStartLoad) / (this.testParameters.RunTime - 1);
         }
         else
         {
            this.loadChangePerSecond = 0;
         }

         if (false != result)
         {
            result = this.CreateLogFile();

            if (false == result)
            {
               this.completionCause = "log creation failure";
            }
         }

         if (false != result)
         {
            this.Log("DYNO TEST");
            this.Log(" - wheel start load = {0}", this.testParameters.WheelStartLoad);
            this.Log(" - wheel end load = {0}", this.testParameters.WheelStopLoad);
            this.Log(" - wheel speed = {0}", this.testParameters.WheelSpeed);
            this.Log(" - time limit = {0}", this.testParameters.RunTime);
            this.Log(" - current limit = {0}", this.testParameters.CurrentLimit);
            this.Log(" - thermal limit = {0}", this.testParameters.ThermalLimit);
            this.Log(" - slippage limit = {0}", this.testParameters.SlippageLimit);

            Tracer.WriteMedium(TraceGroup.TEST, "", "enable setup voltage");
            this.digitalIo.SetOutput(0x01);

            double powerUpDelay = 3;
            Tracer.WriteMedium(TraceGroup.TEST, "", "wait {0} seconds", powerUpDelay);
            this.stateTimeLimit = this.currentTime.AddSeconds(powerUpDelay);
            this.Process = this.WaitSetupPowerUp;
         }
         else
         {
            this.Process = this.CompleteTest;
         }
      }

      private void WaitSetupPowerUp()
      {
         if (this.currentTime > this.stateTimeLimit)
         {
            bool result = this.StartBus();

            if (false != result)
            {
               this.startTime = this.currentTime;
               this.RampLoad();

               int requestedRpm = (int)(this.testParameters.WheelSpeed * this.busParameters.UutSpeedToRpm);
               Tracer.WriteMedium(TraceGroup.TEST, "", "uut speed={0}, rpm={1}", this.testParameters.WheelSpeed, requestedRpm);
               this.uut.SetVelocity(requestedRpm);

               this.startTime = DateTime.Now;
               this.secondLimit = 1.0;

               this.lastEncoderTime = this.startTime;
               this.lastEncoderRotations = this.encoder.Rotations;

               this.Log(" - UUT version = \"{0}\"", this.uut.DeviceVersion);
               this.Log(" - start = {0:D2}-{1:D2}-{2:D2} {3:D2}:{4:D2}:{5:D2}", this.startTime.Month, this.startTime.Day, this.startTime.Year, this.startTime.Hour, this.startTime.Minute, this.startTime.Second);
               this.Log("{0,9}, {1,8}, {2,15}, {3,15}, {4,15}, {5,15}, {6,15}, {7,15}", "TIME", "LOAD", "MOTOR-SPEED", "MOTOR-CURRENT", "TEMPERATURE", "EXT-CURRENT", "BODY-SPEED", "SLIPPAGE");

               this.Process = this.RunTest;
            }
            else
            {
               this.Process = this.CompleteTest;
            }
         }
         else if (false != completeEarly)
         {
            this.Process = this.CompleteTest;
            this.completionCause = "user abort";
         }           
      }

      private void RunTest()
      {
         TimeSpan totalRunSpan = currentTime - this.startTime;

         if (totalRunSpan.TotalSeconds >= this.secondLimit)
         {
            this.LogData();

            if (this.secondLimit < this.testParameters.RunTime)
            {
               this.RampLoad();
            }

            this.secondLimit += 1.0;
         }

         if (null != this.completionCause)
         {
            this.Process = this.CompleteTest;
         }
         else if (false != completeEarly)
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
         Tracer.WriteMedium(TraceGroup.TEST, "", "disable setup voltage");
         this.digitalIo.SetOutput(0x00);

         this.Log("END");
         TimeSpan ts = DateTime.Now - this.startTime;
         this.Log(" - completion cause = {0}", this.completionCause);
         this.Log(" - runtime = {0:0.0}", ts.TotalSeconds);

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
            this.currentTime = DateTime.Now;
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

      public void Start(SetupParameters busParameters, TestParameters testParameters, ref string result)
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