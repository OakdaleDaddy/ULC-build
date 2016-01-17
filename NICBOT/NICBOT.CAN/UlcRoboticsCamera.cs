namespace NICBOT.CAN
{
   using System;
   using System.Text;
   using System.Threading;

   public class UlcRoboticsCamera : Device
   {
      #region Field

      #endregion

      #region Properties

      #endregion

      #region Helper Functions

      #endregion

      #region Overrides

      protected override void EvaluateMessage(int cobId, byte[] msg)
      {
      }

      protected override void EvaluateAction(CommAction action)
      {
         base.EvaluateAction(action);
      }

      #endregion

      #region Constructor

      public UlcRoboticsCamera(string name, byte nodeId)
         : base(name, nodeId)
      {
      }

      #endregion

      #region Access Methods

      public override void Initialize()
      {
         base.Initialize();
      }

      public bool SetState(bool on)
      {
         UInt32 value = (UInt32)((false != on) ? 1 : 0);
         this.pendingAction = new SDODownload(0x2001, 1, 1, value);
         this.ScheduleAction(this.pendingAction, 200, 2);

         bool result = this.commEvent.WaitOne(500);
         return (result);
      }

      public bool SetLightIntensity(byte intensity)
      {
         UInt32 value = (UInt32)intensity;
         this.pendingAction = new SDODownload(0x2002, 1, 1, value);
         this.ScheduleAction(this.pendingAction, 200, 2);

         bool result = this.commEvent.WaitOne(500);
         return (result);
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

      public bool SetTimeout(UInt16 timeout)
      {
         this.pendingAction = new SDODownload(0x2102, 0, 2, timeout);
         this.ScheduleAction(this.pendingAction);

         bool result = this.commEvent.WaitOne(500);
         return (result);
      }

      public bool SetLocation(string location)
      {
         byte[] data = Encoding.UTF8.GetBytes(location);
         this.pendingAction = new SDODownload(0x2103, 0, data, 0, data.Length);
         this.ScheduleAction(this.pendingAction);

         bool result = this.commEvent.WaitOne(500);
         return (result);
      }

      public bool SetPowerUpState(byte powerUpState)
      {
         this.pendingAction = new SDODownload(0x2001, 2, 1, powerUpState);
         this.ScheduleAction(this.pendingAction);

         bool result = this.commEvent.WaitOne(500);
         return (result);
      }

      public bool SetTimeoutState(byte timeoutState)
      {
         this.pendingAction = new SDODownload(0x2001, 3, 1, timeoutState);
         this.ScheduleAction(this.pendingAction);

         bool result = this.commEvent.WaitOne(500);
         return (result);
      }

      public bool SetPowerUpIntensity(byte powerUpIntensity)
      {
         this.pendingAction = new SDODownload(0x2002, 2, 1, powerUpIntensity);
         this.ScheduleAction(this.pendingAction);

         bool result = this.commEvent.WaitOne(500);
         return (result);
      }

      public bool SetTimeoutIntensity(byte timeoutIntensity)
      {
         this.pendingAction = new SDODownload(0x2002, 3, 1, timeoutIntensity);
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

      public bool GetTimeout(ref UInt16 timeout)
      {
         SDOUpload upload = new SDOUpload(0x2102, 0);

         this.pendingAction = upload;
         this.ScheduleAction(this.pendingAction);

         bool actionResult = this.commEvent.WaitOne(500);

         if (false != actionResult)
         {
            if (null != upload.Data)
            {
               timeout = BitConverter.ToUInt16(upload.Data, 0);
            }
         }

         return (actionResult);
      }

      public bool GetLocation(ref string location)
      {
         SDOUpload upload = new SDOUpload(0x2103, 0);

         this.pendingAction = upload;
         this.ScheduleAction(this.pendingAction);

         bool actionResult = this.commEvent.WaitOne(500);

         if (false != actionResult)
         {
            if (null != upload.Data)
            {
               location = Encoding.UTF8.GetString(upload.Data);
            }
         }

         return (actionResult);
      }

      public bool GetPowerUpState(ref byte powerUpState)
      {
         SDOUpload upload = new SDOUpload(0x2001, 2);

         this.pendingAction = upload;
         this.ScheduleAction(this.pendingAction);

         bool actionResult = this.commEvent.WaitOne(500);

         if (false != actionResult)
         {
            if (null != upload.Data)
            {
               powerUpState = upload.Data[0];
            }
         }

         return (actionResult);
      }

      public bool GetTimeoutState(ref byte timeoutState)
      {
         SDOUpload upload = new SDOUpload(0x2001, 3);

         this.pendingAction = upload;
         this.ScheduleAction(this.pendingAction);

         bool actionResult = this.commEvent.WaitOne(500);

         if (false != actionResult)
         {
            if (null != upload.Data)
            {
               timeoutState = upload.Data[0];
            }
         }

         return (actionResult);
      }

      public bool GetPowerUpIntensity(ref byte powerUpIntensity)
      {
         SDOUpload upload = new SDOUpload(0x2002, 2);

         this.pendingAction = upload;
         this.ScheduleAction(this.pendingAction);

         bool actionResult = this.commEvent.WaitOne(500);

         if (false != actionResult)
         {
            if (null != upload.Data)
            {
               powerUpIntensity = upload.Data[0];
            }
         }

         return (actionResult);
      }

      public bool GetTimeoutIntensity(ref byte timeoutIntensity)
      {
         SDOUpload upload = new SDOUpload(0x2002, 3);

         this.pendingAction = upload;
         this.ScheduleAction(this.pendingAction);

         bool actionResult = this.commEvent.WaitOne(500);

         if (false != actionResult)
         {
            if (null != upload.Data)
            {
               timeoutIntensity = upload.Data[0];
            }
         }

         return (actionResult);
      }

      #endregion
   }
}