namespace E4.Utilities
{
   using System;

   public enum TraceGroup : int
   {
      COMM,
      UI,
      CANBUS,
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
            case TraceGroup.CANBUS: prefix = "CAN: "; break;        
         }

         return (prefix);
      }
   }
}