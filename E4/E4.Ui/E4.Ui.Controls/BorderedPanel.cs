
namespace E4.Ui.Controls
{
   using System;
   using System.Collections.Generic;
   using System.ComponentModel;
   using System.Drawing;
   using System.Data;
   using System.Linq;
   using System.Text;
   using System.Threading.Tasks;
   using System.Windows.Forms;

   public class BorderedPanel : Panel
   {
      private int edgeWeight;

      public int EdgeWeight
      {
         set
         {
            this.edgeWeight = value;
            this.Invalidate();
         }

         get
         {
            return (this.edgeWeight);
         }
      }

      public BorderedPanel()
      {
      }

      protected override void OnPaint(PaintEventArgs e)
      {
         base.OnPaint(e);

         if (base.BorderStyle == System.Windows.Forms.BorderStyle.None)
         {
            int edgeOffset = this.edgeWeight / 2;
            e.Graphics.DrawRectangle(new Pen(Color.Black, this.edgeWeight), edgeOffset, edgeOffset, this.ClientRectangle.Width - this.edgeWeight, this.ClientRectangle.Height - this.edgeWeight);
         }
      }
   }
}
