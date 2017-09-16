
namespace CrossBore.BusSim
{
   using System;

   public class ErrorData
   {
      #region Properties

      public int Subsystem { set; get; }
      public UInt32 Code { set; get; }
      public UInt32 AdditionalData { set; get; }

      #endregion

      #region Constructor

      public ErrorData()
      {
      }

      public ErrorData(int subSystem, UInt32 code, UInt32 additionalData)
      {
         this.Subsystem = subSystem;
         this.Code = code;
         this.AdditionalData = additionalData;
      }

      #endregion
   }
}