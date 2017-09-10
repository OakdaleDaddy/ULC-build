
namespace UlcRobotics.Ui.Controls
{
   using System;
   using System.Drawing;
   using System.Windows.Forms;

   public class TickIndicator : Control
   {
      #region Fields

      private int opacity;
      private ContentAlignment textAlign;

      private float lineWeight;
      private Color innerColor;

      private bool mouseDown;
      private Point mouseDownPoint;

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

      public float LineWeight
      {
         get { return this.lineWeight; }
         set { this.lineWeight = value; this.Invalidate(); }
      }

      public Color InnerColor
      {
         get { return this.innerColor; }
         set { this.innerColor = value; this.Invalidate(); }
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

      private void TickIndicator_MouseDown(object sender, MouseEventArgs e)
      {
         this.mouseDownPoint = e.Location;
         this.mouseDown = true;
      }

      private void TickIndicator_MouseUp(object sender, MouseEventArgs e)
      {
         this.mouseDown = false;
      }

      private void TickIndicator_MouseMove(object sender, MouseEventArgs e)
      {
         if (false != this.mouseDown)
         {
            int top = this.Top + (e.Y - mouseDownPoint.Y);
            int topLimit = this.Parent.ClientRectangle.Height - this.Height;

            int left = this.Left + (e.X - mouseDownPoint.X);
            int leftLimit = this.Parent.ClientRectangle.Width - this.Width;

            if (top < 0)
            {
               top = 0;
            }
            else if (top > topLimit)
            {
               top = topLimit;
            }

            if (left < 0)
            {
               left = 0;
            }
            else if (left > leftLimit)
            {
               left = leftLimit;
            }

            this.Top = top;
            this.Left = left;
            //this.Parent.Invalidate();
         }
      }

      protected override void OnPaint(PaintEventArgs e)
      {
#if false
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
#endif
         float lineWeight = (this.LineWeight > 0) ? this.LineWeight : 1;


         float startOffset = lineWeight;
         float width = this.ClientRectangle.Width - (lineWeight * 2);
         float height = this.ClientRectangle.Height - (lineWeight * 2);

         e.Graphics.FillEllipse(new SolidBrush(this.InnerColor), startOffset, startOffset, width, height);
         e.Graphics.DrawEllipse(new Pen(this.ForeColor, this.LineWeight), startOffset, startOffset, width, height);


//         StringFormat textFormat = this.GetStringFormat(this.TextAlign);
//         e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), this.ClientRectangle, textFormat);
      }

      #endregion

      #region Constructor

      public TickIndicator()
         : base()
      {
         this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
         this.SetStyle(ControlStyles.Opaque, true);
         this.opacity = 100;
         this.BackColor = Color.Transparent;
         this.TextAlign = ContentAlignment.MiddleCenter;

         this.MouseDown += this.TickIndicator_MouseDown;
         this.MouseUp += this.TickIndicator_MouseUp;
         this.MouseMove += this.TickIndicator_MouseMove;

         //this.DoubleBuffered = true;
      }

      #endregion

   }
}
