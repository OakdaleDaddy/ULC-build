namespace Weco.Utilities
{
   using System;

   public enum TraceGroup : int
   {
      COMM,
      UI,
      SESSION,
      PARAM,
      CANBUS,
      RBUS,
      TBUS,
   }

   public static class TracePrefix
   {
      public static string Value(TraceGroup group)
      {
         string prefix = "***: ";

         switch (group)
         {
            case TraceGroup.COMM: prefix = "COMM: "; break;
            case TraceGroup.UI: prefix = "UI: "; break;
            case TraceGroup.SESSION: prefix = "SESS: "; break;
            case TraceGroup.PARAM: prefix = "PARAM: "; break;
            case TraceGroup.CANBUS: prefix = "CAN: "; break;
            case TraceGroup.RBUS: prefix = "RBUS: "; break;
            case TraceGroup.TBUS: prefix = "TBUS: "; break;
         }

         return (prefix);
      }
   }
}