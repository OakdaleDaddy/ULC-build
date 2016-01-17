

namespace NICBOT.GUI
{
   using System;
   using System.Drawing;
   using System.Drawing.Drawing2D;
   using System.Windows.Forms;

   public class RotatableLabel : Control
   {
      #region Fields

      private ContentAlignment textAlign;
      private int angle;

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

      public int Angle
      {
         get { return this.angle; }

         set
         {
            this.angle = value;
            this.Invalidate();
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
         Matrix matrix = new Matrix();
         int centerX = this.ClientRectangle.Width / 2;
         int centerY = this.ClientRectangle.Height / 2;
         matrix.RotateAt(-this.Angle, new Point(centerX, centerY));
         e.Graphics.Transform = matrix;

         e.Graphics.FillRectangle(new SolidBrush(this.BackColor), this.ClientRectangle);

         StringFormat textFormat = this.GetStringFormat(this.TextAlign);
         e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), this.ClientRectangle, textFormat);
      }
      
      #endregion

      #region Constructor

      public RotatableLabel()
         : base()
      {
         this.TextAlign = ContentAlignment.MiddleCenter;
      }

      #endregion

   }
}