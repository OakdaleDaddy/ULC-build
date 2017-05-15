
namespace CanDemo.PCANLight
{
   using System;
   using System.Net;

   using CanDemo.Utilities;

   public class BusParameters
   {
      #region Definition

      public delegate void ReceiveDelegateHandler(CanFrame frame);

      #endregion

      #region Properties

      public BusInterfaces BusInterface { set; get; }
      public FramesType MessageType { set; get; }
      public TraceGroup Trace { set; get; }
      public ReceiveDelegateHandler ReceiveHandler { set; get; }

      #endregion

      #region Constructor

      public BusParameters(BusInterfaces busInterface, FramesType messageType, TraceGroup traceGroup, ReceiveDelegateHandler receiveHandler)
      {
         this.BusInterface = busInterface;
         this.MessageType = messageType;
         this.Trace = traceGroup;
         this.ReceiveHandler = receiveHandler;
      }
      
      #endregion

      #region Access Methods

      public override string ToString()
      {
         return (this.BusInterface.ToString());
      }

      public virtual string GetDescription()
      {
         return (this.BusInterface.ToString());
      }

      #endregion
   }

   public class PciBusParameters : BusParameters
   {
      public int BitRate { set; get; }

      public PciBusParameters(BusInterfaces busInterface, int bitRate, FramesType messageType, TraceGroup traceGroup, ReceiveDelegateHandler receiveHandler)
         : base(busInterface, messageType, traceGroup, receiveHandler)
      {
         this.BitRate = bitRate;
      }

      public override string GetDescription()
      {
         return (this.BusInterface.ToString() + ": " + this.BitRate.ToString() + " bps");
      }

   }

   public class UsbBusParameters : BusParameters
   {
      public int BitRate { set; get; }

      public UsbBusParameters(BusInterfaces busInterface, int bitRate, FramesType messageType, TraceGroup traceGroup, ReceiveDelegateHandler receiveHandler)
         : base(busInterface, messageType, traceGroup, receiveHandler)
      {
         this.BitRate = bitRate;
      }

      public override string GetDescription()
      {
         return (this.BusInterface.ToString() + ": " + this.BitRate.ToString() + " bps");
      }
   }

   public class IpGatewayBusParameters : BusParameters
   {
      public IPEndPoint TransmitEndPoint { set; get; }
      public IPEndPoint ReceiveEndPoint { set; get; }

      public IpGatewayBusParameters(BusInterfaces busInterface, IPEndPoint transmitEndPoint, IPEndPoint receiveEndPoint, FramesType messageType, TraceGroup traceGroup, ReceiveDelegateHandler receiveHandler)
         : base(busInterface, messageType, traceGroup, receiveHandler)
      {
         this.TransmitEndPoint = transmitEndPoint;
         this.ReceiveEndPoint = receiveEndPoint;
      }

      public IpGatewayBusParameters(BusInterfaces busInterface, string transmitIpAddressText, string transmitPortText, string receiveIpAddressText, string receivePortText, FramesType messageType, TraceGroup traceGroup, ReceiveDelegateHandler receiveHandler)
         : base(busInterface, messageType, traceGroup, receiveHandler)
      {
         IPAddress transmitAddress = null;
         int transmitPort = 0;
         IPEndPoint transmitEndPoint = null;

         if ((IPAddress.TryParse(transmitIpAddressText, out transmitAddress) != false) &&
             (int.TryParse(transmitPortText, out transmitPort) != false))
         {
            transmitEndPoint = new IPEndPoint(transmitAddress, transmitPort);
         }

         IPAddress receiveAddress = null;
         int receivePort = 0;
         IPEndPoint receiveEndPoint = null;

         if ((IPAddress.TryParse(receiveIpAddressText, out receiveAddress) != false) &&
             (int.TryParse(receivePortText, out receivePort) != false))
         {
            receiveEndPoint = new IPEndPoint(receiveAddress, receivePort);
         }

         this.TransmitEndPoint = transmitEndPoint;
         this.ReceiveEndPoint = receiveEndPoint;
      }

      public override string GetDescription()
      {
         return (this.BusInterface.ToString() + ": tx:" + this.TransmitEndPoint.ToString() + ": rx:" + this.ReceiveEndPoint.ToString());
      }
   }
}
