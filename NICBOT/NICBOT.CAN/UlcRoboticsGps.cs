namespace NICBOT.CAN
{
   using System;
   using System.Text;
   using System.Threading;

   public class UlcRoboticsGps : Device
   {
      #region Definition



      #endregion

      #region Field

      private bool active;
      private DateTime deviceReceiveTimeLimit;

      #endregion

      #region Properties

      public bool Antenna { set; get; }
      public int Satellites { set; get; }
      public double Latitude { set; get; }
      public double Longitude { set; get; }
      public DateTime Utc { set; get; }

      #endregion

      #region Overrides

      protected override void EvaluateMessage(int cobId, byte[] msg)
      {
         COBTypes frameType = (COBTypes)((cobId >> 7) & 0xF);
         int nodeId = (int)(cobId & 0x7F);

         if (nodeId == this.NodeId)
         {
            if (COBTypes.TPDO1 == frameType)
            {
               if ((null != msg) && (3 == msg.Length))
               {
                  this.Antenna = (3 == msg[2]) ? true : false;
                  this.Satellites = msg[1];

                  if ((false == this.Antenna) || (0 == this.Satellites))
                  {
                     this.Longitude = double.NaN;
                     this.Latitude = double.NaN;
                  }
               }
            }
            else if (COBTypes.TPDO2 == frameType)
            {
               if ((null != msg) && (7 == msg.Length))
               {
                  if ((false != this.Antenna) && (0 != this.Satellites))
                  {
                     float minutes = BitConverter.ToSingle(msg, 0);
                     int degrees = BitConverter.ToInt16(msg, 4);
                     byte direction = msg[6];

                     double longitude = degrees + (minutes / 60);

                     if ('W' == direction)
                     {
                        longitude *= -1;
                     }

                     this.Longitude = longitude;
                  }
               }
            }
            else if (COBTypes.TPDO3 == frameType)
            {
               if ((null != msg) && (7 == msg.Length))
               {
                  if ((false != this.Antenna) && (0 != this.Satellites))
                  {
                     float minutes = BitConverter.ToSingle(msg, 0);
                     int degrees = BitConverter.ToInt16(msg, 4);
                     byte direction = msg[6];

                     double latitude = degrees + (minutes / 60);

                     if ('S' == direction)
                     {
                        latitude *= -1;
                     }

                     this.Latitude = latitude;
                  }
               }
            }
            else if (COBTypes.TPDO4 == frameType)
            {
               if ((null != msg) && (6 == msg.Length))
               {
                  this.deviceReceiveTimeLimit = DateTime.Now.AddSeconds(3);

                  if (0 != msg[5])
                  {
                     this.Utc = new DateTime(msg[3] + 2000, msg[4], msg[5], msg[0], msg[1], msg[2]);
                  }
                  else
                  {
                     this.Utc = default(DateTime);
                  }
               }
            }
         }
      }

      protected override void EvaluateAction(CommAction action)
      {
         base.EvaluateAction(action);
      }

      #endregion

      #region Constructor

      public UlcRoboticsGps(string name, byte nodeId)
         : base(name, nodeId)
      {
      }

      #endregion

      #region Access Methods

      public override void Initialize()
      {
         this.Antenna = false;
         this.Satellites = 0;
         this.Latitude = double.NaN;
         this.Longitude = double.NaN;
         this.Utc = default(DateTime);

         base.Initialize();
      }

      public bool SetDeviceBaudRate(int rate)
      {
         UInt32 rateCode = 0;

         if (10000 == rate)
         {
            rateCode = 0;
         }
         else if (20000 == rate)
         {
            rateCode = 1;
         }
         else if (50000 == rate)
         {
            rateCode = 3;
         }
         else if (125000 == rate)
         {
            rateCode = 4;
         }
         else if (250000 == rate)
         {
            rateCode = 5;
         }
         else if (500000 == rate)
         {
            rateCode = 6;
         }
         else if (1000000 == rate)
         {
            rateCode = 7;
         }

         this.pendingAction = new SDODownload(0x2100, 0, 1, rateCode);
         this.ScheduleAction(this.pendingAction);

         bool result = this.commEvent.WaitOne(500);
         return (result);
      }

      public bool SetDeviceNodeId(byte nodeId)
      {
         this.pendingAction = new SDODownload(0x2101, 0, 1, nodeId);
         this.ScheduleAction(this.pendingAction);

         bool result = this.commEvent.WaitOne(500);
         return (result);
      }

      public bool SaveConfiguration()
      {
         this.pendingAction = new SDODownload(0x2105, 0, 4, 0x65766173);
         this.ScheduleAction(this.pendingAction, 200, 2);

         bool result = this.commEvent.WaitOne(500);
         return (result);
      }

      public int GetDeviceBaudRate()
      {
         int result = 0;
         SDOUpload upload = new SDOUpload(0x2100, 0);

         this.pendingAction = upload;
         this.ScheduleAction(this.pendingAction);

         bool actionResult = this.commEvent.WaitOne(500);

         if (false != actionResult)
         {
            if (null != upload.Data)
            {
               byte baudCode = upload.Data[0];

               if (0 == baudCode)
               {
                  result = 10000;
               }
               else if (1 == baudCode)
               {
                  result = 20000;
               }
               else if (3 == baudCode)
               {
                  result = 50000;
               }
               else if (4 == baudCode)
               {
                  result = 125000;
               }
               else if (5 == baudCode)
               {
                  result = 250000;
               }
               else if (6 == baudCode)
               {
                  result = 500000;
               }
               else if (7 == baudCode)
               {
                  result = 1000000;
               }
            }
         }

         return (result);
      }

      public bool GetDeviceNodeId(ref byte deviceId)
      {
         SDOUpload upload = new SDOUpload(0x2101, 0);

         this.pendingAction = upload;
         this.ScheduleAction(this.pendingAction);

         bool actionResult = this.commEvent.WaitOne(500);

         if (false != actionResult)
         {
            if (null != upload.Data)
            {
               deviceId = upload.Data[0];
            }
         }

         return (actionResult);
      }

      public override bool Start()
      {
         bool result = false;

         if (null == this.FaultReason)
         {
            result = true;

            // get device type
            UInt32 deviceType = 0;
            result &= this.ReadDeviceType(ref deviceType);
            this.DeviceType = deviceType;

            // get device name
            string deviceName = string.Empty;
            result &= this.ReadDeviceName(ref deviceName);
            this.DeviceName = deviceName;

            // get device version
            string deviceVersion = string.Empty;
            result &= this.ReadDeviceVersion(ref deviceVersion);
            this.DeviceVersion = deviceVersion;

            this.SetTPDOMapCount(1, 0);
            this.SetTPDOType(1, 255);
            this.SetTPDOEventTime(1, 5000);
            this.SetTPDOMap(1, 1, 0x2201, 1, 1);
            this.SetTPDOMap(1, 2, 0x2201, 2, 1);
            this.SetTPDOMap(1, 3, 0x2201, 3, 1);
            this.SetTPDOMapCount(1, 3);

            this.SetTPDOMapCount(2, 0);
            this.SetTPDOType(2, 255);
            this.SetTPDOEventTime(2, 5000);
            this.SetTPDOMap(2, 1, 0x2202, 0, 7);
            this.SetTPDOMapCount(2, 1);

            this.SetTPDOMapCount(3, 0);
            this.SetTPDOType(3, 255);
            this.SetTPDOEventTime(3, 5000);
            this.SetTPDOMap(3, 1, 0x2203, 0, 7);
            this.SetTPDOMapCount(3, 1);

            this.SetTPDOMapCount(4, 0);
            this.SetTPDOType(4, 255);
            this.SetTPDOEventTime(4, 1000);
            this.SetTPDOMap(4, 1, 0x2205, 0, 6);
            this.SetTPDOMapCount(4, 1);

            base.Start();
            this.active = true;
            this.deviceReceiveTimeLimit = DateTime.Now.AddSeconds(10);
         }

         return (result);
      }

      public override void Stop()
      {
 	      base.Stop();
         this.active = false;
      }

      public override void Reset()
      {
         this.Antenna = false;
         this.Satellites = 0;
         this.Latitude = double.NaN;
         this.Longitude = double.NaN;
         this.Utc = default(DateTime);
         
         base.Reset();
         this.active = false;
      }

      public override void Update()
      {
         if (false != active)
         {
            if (DateTime.Now > this.deviceReceiveTimeLimit)
            {
               this.Fault("periodic report missing");
               this.active = false;

               this.Antenna = false;
               this.Satellites = 0;
               this.Latitude = double.NaN;
               this.Longitude = double.NaN;
               this.Utc = default(DateTime);
            }
         }

         base.Update();
      }
     
      #endregion
   }
}