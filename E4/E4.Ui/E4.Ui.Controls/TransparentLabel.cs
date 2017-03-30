
namespace E4.Ui.Controls
{
   using System;
   using System.Drawing;
   using System.Windows.Forms;

   public class TransparentLabel : Control
   {
      #region Fields

      private int alpha;
      private int opacity;
      private ContentAlignment textAlign;

      #endregion

      #region Properties

      public ContentAlignment TextAlign
      {
         get
         {
            return (this.textAlign);
         }

         set
         {
            this.textAlign = value;
            this.Invalidate();
         }
      }

      public new string Text
      {
         get
         {
            return (base.Text);
         }

         set
         {
            base.Text = value;
            this.Invalidate();
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

            this.Invalidate();
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

      #region Helper Functions

      private StringFormat GetStringFormat(ContentAlignment alignment)
      {
         StringFormat result = new StringFormat(StringFormat.GenericTypographic);

         switch (alignment)
         {
            case ContentAlignment.BottomCenter:
               {
                  result.Alignment = StringAlignment.Center;
                  result.LineAlignment = StringAlignment.Far;
                  break;
               }
            case ContentAlignment.BottomLeft:
               {
                  result.Alignment = StringAlignment.Near;
                  result.LineAlignment = StringAlignment.Far;
                  break;
               }
            case ContentAlignment.BottomRight:
               {
                  result.Alignment = StringAlignment.Far;
                  result.LineAlignment = StringAlignment.Far;
                  break;
               }
            default:
            case ContentAlignment.MiddleCenter:
               {
                  result.Alignment = StringAlignment.Center;
                  result.LineAlignment = StringAlignment.Center;
                  break;
               }
            case ContentAlignment.MiddleLeft:
               {
                  result.Alignment = StringAlignment.Near;
                  result.LineAlignment = StringAlignment.Center;
                  break;
               }
            case ContentAlignment.MiddleRight:
               {
                  result.Alignment = StringAlignment.Far;
                  result.LineAlignment = StringAlignment.Center;
                  break;
               }
            case ContentAlignment.TopCenter:
               {
                  result.Alignment = StringAlignment.Center;
                  result.LineAlignment = StringAlignment.Near;
                  break;
               }
            case ContentAlignment.TopLeft:
               {
                  result.Alignment = StringAlignment.Near;
                  result.LineAlignment = StringAlignment.Near;
                  break;
               }
            case ContentAlignment.TopRight:
               {
                  result.Alignment = StringAlignment.Far;
                  result.LineAlignment = StringAlignment.Near;
                  break;
               }
         }

         return (result);
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

         StringFormat textFormat = this.GetStringFormat(this.TextAlign);
         e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), this.ClientRectangle, textFormat);
      }

      #endregion

      #region Constructor

      public TransparentLabel()
         : base()
      {
         this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
         this.SetStyle(ControlStyles.Opaque, true);
         this.opacity = 100;
         this.BackColor = Color.Transparent;
         this.TextAlign = ContentAlignment.MiddleCenter;
      }

      #endregion

   }
}