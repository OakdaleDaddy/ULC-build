namespace DYNO.GUI
{
   using System;
   using System.Collections;
   using System.Text;
   using System.Threading;

   using DYNO.PCANLight;
   using DYNO.CAN;
   using DYNO.Utilities;
   /*
Test Process:12 hours
 - starts test when requested
 - stops test when requested
 - stops test when time limit is satisfied
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
 - log file auto named based date and test instance
 - log stores test parameters on start
 - log stores start time on start
 - log stores values every second with relative time on one line separated by commas
 - log stores retrieved UUT speed and direction
 - log stores retrieved UUT current
 - log stores retrieved UUT temperature
 - log stores retrieved Encoder distance
 - log stores retrieved Encoder speed
 - log stores retrieved AnalogIO motor current
 - log stores set AnalogIO wheel load
 - log stores test stop reason
    * */

   public class DynoTest
   {
      #region Definitions

      public delegate void CompleteHandler();

      #endregion

      #region Fields

      private bool execute;
      private Thread thread;

      private Queue busReceiveQueue;
      private BusParameters busParameters;
      private TestParameters testParameters;

      private ArrayList deviceList;

      #endregion

      #region Properties

      public CompleteHandler OnComplete;

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

      #region Process Functions

      #region Device Process Loop
      
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
         foreach (Device device in this.deviceList)
         {
            device.Initialize();
         }

         for (; this.execute; )
         {
            this.ProcessCommFrames();

            foreach (Device device in this.deviceList)
            {
               device.Update();
            }

            Thread.Sleep(1);
         }
      }

      #endregion


#if false
      private void InitializeDevices()
      {
         foreach (Device device in this.deviceList)
         {
            device.Initialize();
         }
      }

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
               StringBuilder sb = new StringBuilder();

               for (int i = 0; i < frame.data.Length; i++)
               {
                  sb.AppendFormat("{0:X2}", frame.data[i]);
               }

//               if (frame.cobId != 0x194)
               {
                  Tracer.WriteMedium(TraceGroup.TBUS, "", "rx {0:X3} {1}", frame.cobId, sb.ToString());
               }

               foreach (Device device in this.deviceList)
               {
                  device.Update((int)frame.cobId, frame.data);
               }

               frame = null;
            }
         }
         while (0 != receiveCount);
      }

      private void UpdateDevices()
      {
         foreach (Device device in this.deviceList)
         {
            device.Update();
         }
      }

#endif

      private void TestProcess()
      {
         Tracer.WriteMedium(TraceGroup.TEST, "", "test start");

         CANResult startResult = PCANLight.Start(this.busParameters.BusInterface, this.busParameters.BitRate, FramesType.INIT_TYPE_ST, TraceGroup.CANBUS, this.BusReceiveHandler);

         if (CANResult.ERR_OK != startResult) 
         {
            execute = false;
         }

         //this.InitializeDevices();

         for (; execute; )
         {
  //          this.ProcessCommFrames();
    //        this.UpdateDevices();
            Thread.Sleep(1);
         }

         PCANLight.Stop(this.busParameters.BusInterface);

         Tracer.WriteMedium(TraceGroup.TEST, "", "test stop");
         this.Complete();
      }

      #endregion

      #region Constructor

      public DynoTest()
      {
         this.busReceiveQueue = new Queue();
         this.deviceList = new ArrayList();
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

            this.execute = true;
            this.thread.Start();
         }
      }

      public void Stop()
      {
         if (null != this.thread)
         {
            this.execute = false;
            this.thread.Join(3000);

            this.thread = null;
         }
      }

      #endregion
   }

}