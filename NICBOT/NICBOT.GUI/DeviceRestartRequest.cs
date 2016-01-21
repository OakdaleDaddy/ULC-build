namespace NICBOT.GUI
{
   using System;

   public class DeviceRestartRequest
   {
      #region Definitions

      public delegate void CompleteHandler(Enum deviceId);
      public delegate void RestartHandler(Enum deviceId, CompleteHandler onComplete);

      #endregion

      #region Properties

      public Enum Id { set; get; }
      public CompleteHandler OnComplete { set; get; }

      #endregion

      #region Constructor

      public DeviceRestartRequest()
      {
         this.OnComplete = null;
         this.Id = default(Enum);
      }

      public DeviceRestartRequest(Enum id, CompleteHandler onComplete)
      {
         this.Id = id;
         this.OnComplete = onComplete;
      }

      #endregion
   }
}
