
namespace CrossBore.CAN
{
   using System;

   public class EmergencyRecord
   {
      #region Properties

      public UInt32 Code { set; get; }
      public string Description { set; get; }

      #endregion

      #region Constructor

      public EmergencyRecord()
      {
      }

      public EmergencyRecord(UInt32 code, string description)
      {
         this.Code = code;
         this.Description = description;
      }

      #endregion
   }
}