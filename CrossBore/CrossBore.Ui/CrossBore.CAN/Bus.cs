
namespace CrossBore.CAN
{
   using System;

   public class Bus
   {
      #region Fields

      private string name;

      #endregion

      #region Properties

      public string Name { get { return (this.name); } }
      public string Interface { set; get; }
      public byte NodeId { set; get; }

      public bool TraceHB { set; get; }
      public bool TraceSync { set; get; }

      #endregion

      #region Constructor

      public Bus()
      {
         this.name = "";
      }

      public Bus(string name, byte nodeId = 0)
      {
         this.name = name;
         this.NodeId = nodeId;
      }

      #endregion
   }
}