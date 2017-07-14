
namespace Weco.Ui.Controls
{
   using System;
   using System.Drawing;
   using System.Windows.Forms;

   public class TransparentPanel : Panel
   {
      #region Fields

      private int edgeWeight;

      private int alpha;
      private int opacity;

      #endregion

      #region Properties

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

      public int Opacity
      {
         get
         {
            return (this.opacity);
         }
         set
         {
            if (value > 100)
            {
               value = 100;
            }
            else if (value < 1)
            {
               value = 1;
            }

            this.opacity = value;

            if (this.Parent != null)
            {
               this.Parent.Invalidate(this.Bounds, true);
            }
         }
      }

      protected override CreateParams CreateParams
      {
         get
         {
            CreateParams cp = base.CreateParams;
            cp.ExStyle = cp.ExStyle | 0x20;
            return cp;
         }
      }

      #endregion

      #region Events

      protected override void OnPaint(PaintEventArgs e)
      {
         Graphics g = e.Graphics;
         Rectangle bounds = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

         Color frmColor = this.Parent.BackColor;
         Brush bckColor = default(Brush);

         this.alpha = (this.opacity * 255) / 100;
         bckColor = new SolidBrush(Color.FromArgb(alpha, this.BackColor));

         if (this.BackColor != Color.Transparent)
         {
            g.FillRectangle(bckColor, bounds);
         }

         if (base.BorderStyle == System.Windows.Forms.BorderStyle.None)
         {
            int edgeOffset = this.edgeWeight / 2;
            e.Graphics.DrawRectangle(new Pen(Color.Black, this.edgeWeight), edgeOffset, edgeOffset, this.ClientRectangle.Width - this.edgeWeight - 1, this.ClientRectangle.Height - this.edgeWeight - 1);
         }
      }

      #endregion

      #region Constructor

      public TransparentPanel()
         : base()
      {
         this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
         this.SetStyle(ControlStyles.Opaque, true);
         this.opacity = 100;
         this.BackColor = Color.Transparent;
      }

      #endregion

   }
}