namespace NICBOT.DeviceTest
{
   using System;
   using System.Collections;
   using System.Text;
   using System.Threading;

   using NICBOT.PCANLight;
   using NICBOT.CAN;
   using NICBOT.Utilities;

   public class BusInterface
   {
      #region Fields

      private bool execute;
      private Thread thread;

      private Queue busReceiveQueue;
      private BusInterfaces busInterface;

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

      private bool DeviceTransmit(int id, byte[] data)
      {
         StringBuilder sb = new StringBuilder();

         for (int i = 0; i < data.Length; i++)
         {
            sb.AppendFormat("{0:X2}", data[i]);
         }

         CANResult transmitResult = PCANLight.Send(this.busInterface, id, data);
         bool result = (transmitResult == CANResult.ERR_OK) ? true : false;

         if (false != result)
         {
            Tracer.WriteMedium(TraceGroup.TBUS, "", "tx {0:X3} {1}", id, sb.ToString());
         }

         return (result);
      }

      private void DeviceError(string name, int nodeId, string reason)
      {
         Tracer.WriteError(TraceGroup.TBUS, "", "fault with \"{0}\", node={1}, reason={2}", name, nodeId, reason);
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

      private void DeviceProcess()
      {
         Tracer.WriteMedium(TraceGroup.TBUS, "", "bus start");
         this.InitializeDevices();

         for (; execute; )
         {
            this.ProcessCommFrames();
            this.UpdateDevices();
            Thread.Sleep(1);
         }
         Tracer.WriteMedium(TraceGroup.TBUS, "", "bus stop");
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

      public void Start(BusInterfaces busInterface, int bitRate, ref string result)
      {
         if (null == result)
         {
            this.busInterface = busInterface;
            CANResult startResult = PCANLight.Start(this.busInterface, bitRate, FramesType.INIT_TYPE_ST, TraceGroup.TBUS, this.BusReceiveHandler);
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
            PCANLight.Stop(this.busInterface);

            this.execute = false;
            this.thread.Join(3000);

            this.thread = null;
         }
      }

      public void AddDevice(Device device)
      {
         device.OnTransmit = new Device.TransmitHandler(this.DeviceTransmit);
         device.OnCommError = new Device.CommErrorHandler(this.DeviceError);

         this.deviceList.Add(device);        
      }

      public void RemoveDevice(Device device)
      {
         lock (this)
         {
            device.OnTransmit = null;
            device.OnCommError = null;

            if (this.deviceList.Contains(device) != false)
            {
               this.deviceList.Remove(device);
            }
         }
      }

      public void Sync()
      {
         PCANLight.SendSync(this.busInterface);
      }

      public void Transmit(int id, byte[] data)
      {
         this.DeviceTransmit(id, data);
      }

      #endregion
   }

}