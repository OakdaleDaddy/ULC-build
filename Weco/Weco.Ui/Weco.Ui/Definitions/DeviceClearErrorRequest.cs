namespace Weco.Ui
{
   using System;

   public class DeviceClearErrorRequest
   {
      #region Definitions

      public delegate void CompleteHandler(Enum deviceId);
      public delegate void ClearErrorHandler(Enum deviceId, UInt32 code, CompleteHandler onComplete);

      #endregion

      #region Properties

      public Enum Id { set; get; }
      public UInt32 Code { set; get; }
      public CompleteHandler OnComplete { set; get; }

      #endregion

      #region Constructor

      public DeviceClearErrorRequest()
      {
         this.OnComplete = null;
         this.Code = 0;
         this.Id = default(Enum);
      }

      public DeviceClearErrorRequest(Enum id, UInt32 code, CompleteHandler onComplete)
      {
         this.Id = id;
         this.Code = code;
         this.OnComplete = onComplete;
      }

      #endregion
   }
}
