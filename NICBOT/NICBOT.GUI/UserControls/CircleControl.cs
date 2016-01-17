using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NICBOT.GUI
{
   public partial class CircleControl : UserControl
   {
      public CircleControl()
      {
         InitializeComponent();
      }

      //private int center;
      // difficult to force height = width


#if false
      protected override void OnPaint(PaintEventArgs e)
      {
#if true
         int reference;

         if (this.ClientRectangle.Width < this.ClientRectangle.Height)
         {
            reference = this.ClientRectangle.Width;
         }
         else
         {
            reference = this.ClientRectangle.Height;
         }

         int circleOffset = base.Font.Height;
         int center = reference / 2;
         int circleDiameter = reference - (2 * circleOffset);
         PointF centerPt = new PointF(center, center);
         
         Graphics g = e.Graphics;

         if (base.BorderStyle == System.Windows.Forms.BorderStyle.FixedSingle)
         {
            //e.Graphics.DrawRectangle(new Pen(Color.Black, 1f), 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height);
         }

         e.Graphics.DrawEllipse(new Pen(Color.Black, 1f), 0, 0, circleDiameter, circleDiameter);
         
         using (GraphicsPath gp = new GraphicsPath())
         {
            //gp.AddEllipse(0, 0, circleDiameter, circleDiameter);
            //this.Region = new Region(gp);

#if false
            using (Brush brush = new SolidBrush(Color.Yellow))
            {
               for (int i = 0; i < 360; i += 360 / 4)
               {
                  Matrix matrix = new Matrix();
                  matrix.RotateAt(i, centerPt);
                  g.Transform = matrix;
                  g.DrawLine(Pens.Yellow, center, center, center, center * 3 / 10);
                  g.DrawString(((int)(i * 4)).ToString(), this.Font, brush, center - 6, center * 5 / 100,
                     StringFormat.GenericTypographic);
               }

            }
#endif
         }

#if false
         using (GraphicsPath gp2 = new GraphicsPath())
         {
            using (Pen pen = new Pen(Color.Black, 12))
            {
               Matrix matrix = new Matrix();
               matrix.RotateAt(0, centerPt);
               g.Transform = matrix;
               pen.EndCap = LineCap.ArrowAnchor;
               g.DrawLine(pen, center, center, center, center / 8);
               g.DrawLine(pen, center, center, (center * 9) / 10, center);
               g.DrawLine(pen, center, center, (center * 11) / 10, center);
               g.DrawLine(pen, center, center, center, (center * 11) / 10);
            }
         }
#endif
#endif
      }
#endif

   }
}
