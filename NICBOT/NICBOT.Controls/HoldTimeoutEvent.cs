using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NICBOT.Controls
{
   public class HoldTimeoutEventArgs : EventArgs
   {
      public bool Handled { set; get; }

      public HoldTimeoutEventArgs()
         : base()
      {
         this.Handled = false;
      }
   }

   public delegate void HoldTimeoutHandler(object sender, HoldTimeoutEventArgs e);
}
