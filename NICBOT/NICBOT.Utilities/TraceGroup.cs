namespace NICBOT.Utilities
{
   using System;

   public enum TraceGroup : int
   {
      COMM,
      PARAM,
      GUI,

      CANBUS,
      TBUS,
      RBUS,

      BODY,
      MOVEMENT,
      DRILL,
      SEALANT,
      PUMP,
   }

   public static class TracePrefix
   {
      public static string Value(TraceGroup group)
      {
         string prefix = "***: ";

         switch (group)
         {
            case TraceGroup.COMM: prefix = "COMM: "; break;
            case TraceGroup.PARAM: prefix = "PARAM: "; break;
            case TraceGroup.GUI: prefix = "GUI: "; break;

            case TraceGroup.CANBUS: prefix = "CAN: "; break;
            case TraceGroup.TBUS: prefix = "TBUS: "; break;
            case TraceGroup.RBUS: prefix = "RBUS: "; break;

            case TraceGroup.BODY: prefix = "BDY: "; break;
            case TraceGroup.MOVEMENT: prefix = "MVM: "; break;
            case TraceGroup.DRILL: prefix = "DRL: "; break;
            case TraceGroup.SEALANT: prefix = "SLT: "; break;
            case TraceGroup.PUMP: prefix = "PMP: "; break;         
        
         }

         return (prefix);
      }
   }
}