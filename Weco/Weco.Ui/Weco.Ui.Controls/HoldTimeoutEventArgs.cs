
namespace Weco.Ui.Controls
{
   using System;

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
