namespace E4.Ui
{
   using System;

   public class DeviceClearWarningRequest
   {
      #region Definitions

      public delegate void CompleteHandler(Enum deviceId);
      public delegate void ClearWarningHandler(Enum deviceId, CompleteHandler onComplete);

      #endregion

      #region Properties

      public Enum Id { set; get; }
      public CompleteHandler OnComplete { set; get; }

      #endregion

      #region Constructor

      public DeviceClearWarningRequest()
      {
         this.OnComplete = null;
         this.Id = default(Enum);
      }

      public DeviceClearWarningRequest(Enum id, CompleteHandler onComplete)
      {
         this.Id = id;
         this.OnComplete = onComplete;
      }

      #endregion
   }
}
