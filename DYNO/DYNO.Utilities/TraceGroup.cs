namespace DYNO.Utilities
{
   using System;

   public enum TraceGroup : int
   {
      CANBUS,
      TEST,
      LOG,
      DEVICE,
   }

   public static class TracePrefix
   {
      public static string Value(TraceGroup group)
      {
         string prefix = "***: ";

         switch (group)
         {
            case TraceGroup.CANBUS: prefix = "CAN: "; break;
            case TraceGroup.TEST: prefix = "TEST: "; break;
            case TraceGroup.LOG: prefix = "LOG: "; break;
            case TraceGroup.DEVICE: prefix = "DEVICE: "; break;
         }

         return (prefix);
      }
   }
}