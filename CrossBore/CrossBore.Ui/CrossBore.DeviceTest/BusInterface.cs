
namespace CrossBore.DeviceTest
{
   using System;
   using System.Collections;
   using System.Text;
   using System.Threading;

   using CrossBore.PCANLight;
   using CrossBore.CAN;
   using CrossBore.Utilities;

   public class BusInterface
   {
      #region Fields

      private bool execute;
      private Thread thread;

      private Queue busReceiveQueue;
      private BusParameters busParameters;

      private ArrayList deviceList;

      #endregion

      #region Delegates

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

         Tracer.WriteMedium(TraceGroup.COMM, "", "rx {0:X3} {1}", cobId, sb.ToString());
      }

      private void DeviceTraceTransmit(int cobId, byte[] data)
      {
         StringBuilder sb = new StringBuilder();

         for (int i = 0; i < data.Length; i++)
         {
            sb.AppendFormat("{0:X2}", data[i]);
         }

         Tracer.WriteMedium(TraceGroup.COMM, "", "tx {0:X3} {1}", cobId, sb.ToString());
      }

      private bool DeviceTransmit(int id, byte[] data)
      {
         CANResult transmitResult = PCANLight.Send(this.busParameters.BusInterface, id, data);
         bool result = (transmitResult == CANResult.ERR_OK) ? true : false;

         return (result);
      }

      private void DeviceFault(string name, int nodeId, string reason)
      {
         Tracer.WriteError(TraceGroup.COMM, "", "fault with \"{0}\", node={1}, reason={2}", name, nodeId, reason);
      }

      private void DeviceWarning(string name, int nodeId, string reason)
      {
         Tracer.WriteError(TraceGroup.COMM, "", "warning with \"{0}\", node={1}, reason={2}", name, nodeId, reason);
      }

      #endregion

      #region Process Functions

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
               bool traced = false;

               foreach (Device device in this.deviceList)
               {
                  traced = traced | device.IsReceivedDownloadFrame((int)frame.cobId);
               }

               foreach (Device device in this.deviceList)
               {
                  device.Update((int)frame.cobId, frame.data, ref traced);
               }

               if (false == traced)
               {
                  this.DeviceTraceReceive((int)frame.cobId, frame.data);
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

      private void DeviceProcess()
      {
         Tracer.WriteMedium(TraceGroup.COMM, "", "bus start");
         this.InitializeDevices();

         for (; execute; )
         {
            this.ProcessCommFrames();
            this.UpdateDevices();
            Thread.Sleep(1);
         }
         Tracer.WriteMedium(TraceGroup.COMM, "", "bus stop");
      }

      #endregion

      #region Constructor

      public BusInterface()
      {
         this.busReceiveQueue = new Queue();
         this.deviceList = new ArrayList();
      }

      #endregion

      #region Access Methods

      public void Start(BusParameters busParameters, ref string result)
      {
         if (null == result)
         {
            this.busParameters = busParameters;

            this.busParameters.MessageType = FramesType.INIT_TYPE_ST;
            this.busParameters.Trace = TraceGroup.COMM;
            this.busParameters.ReceiveHandler = new BusParameters.ReceiveDelegateHandler(this.BusReceiveHandler);
            
            CANResult startResult = PCANLight.Start(this.busParameters);
            result = (CANResult.ERR_OK != startResult) ? "Unable to start" : null;
         }

         if (null == result)
         {
            this.thread = new Thread(this.DeviceProcess);
            this.thread.IsBackground = true;
            this.thread.Name = "Bus Interface";

            this.execute = true;
            this.thread.Start();
         }
      }

      public void Stop()
      {
         if (null != this.thread)
         {
            PCANLight.Stop(this.busParameters.BusInterface);

            this.execute = false;
            this.thread.Join(3000);

            this.thread = null;
         }
      }

      public void AddDevice(Device device)
      {
         device.OnReceiveTrace = new Device.ReceiveTraceHandler(this.DeviceTraceReceive);
         device.OnTransmitTrace = new Device.TransmitTraceHandler(this.DeviceTraceTransmit);
         device.OnTransmit = new Device.TransmitHandler(this.DeviceTransmit);
         device.OnFault = new Device.FaultHandler(this.DeviceFault);
         device.OnWarning = new Device.WarningHandler(this.DeviceWarning);

         this.deviceList.Add(device);        
      }

      public void RemoveDevice(Device device)
      {
         lock (this)
         {
            device.OnTransmit = null;
            device.OnFault = null;
            device.OnWarning = null;

            if (this.deviceList.Contains(device) != false)
            {
               this.deviceList.Remove(device);
            }
         }
      }

      public void Sync()
      {
         PCANLight.SendSync(this.busParameters.BusInterface);
      }

      public void Transmit(int id, byte[] data)
      {
         this.DeviceTransmit(id, data);
      }

      #endregion
   }

}