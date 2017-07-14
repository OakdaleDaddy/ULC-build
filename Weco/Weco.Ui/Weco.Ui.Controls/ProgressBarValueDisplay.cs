
namespace Weco.Ui.Controls
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

   public class ProgressBarValueDisplay : Control
   {
      #region Fields

      private int minimum;
      private int maximum;
      private int value;

      private Color barColor;
      private Color textColor;

      private ContentAlignment textAlign;

      #endregion

      #region Helper Functions

      private StringFormat GetStringFormat(ContentAlignment alignment)
      {
         StringFormat result = new StringFormat(StringFormat.GenericTypographic);

         if (ContentAlignment.BottomCenter == alignment)
         {
            result.Alignment = StringAlignment.Center;
            result.LineAlignment = StringAlignment.Far;
         }
         else if (ContentAlignment.BottomLeft == alignment)
         {
            result.Alignment = StringAlignment.Near;
            result.LineAlignment = StringAlignment.Far;
         }
         else if (ContentAlignment.BottomRight == alignment)
         {
            result.Alignment = StringAlignment.Far;
            result.LineAlignment = StringAlignment.Far;
         }
         else if (ContentAlignment.BottomRight == alignment)
         {
            result.Alignment = StringAlignment.Far;
            result.LineAlignment = StringAlignment.Far;
         }
         else if (ContentAlignment.MiddleLeft == alignment)
         {
            result.Alignment = StringAlignment.Near;
            result.LineAlignment = StringAlignment.Center;
         }
         else if (ContentAlignment.MiddleRight == alignment)
         {
            result.Alignment = StringAlignment.Far;
            result.LineAlignment = StringAlignment.Center;
         }
         else if (ContentAlignment.TopCenter == alignment)
         {
            result.Alignment = StringAlignment.Center;
            result.LineAlignment = StringAlignment.Near;
         }
         else if (ContentAlignment.TopLeft == alignment)
         {
            result.Alignment = StringAlignment.Near;
            result.LineAlignment = StringAlignment.Near;
         }
         else if (ContentAlignment.TopRight == alignment)
         {
            result.Alignment = StringAlignment.Far;
            result.LineAlignment = StringAlignment.Near;
         }
         else // ContentAlignment.MiddleCenter
         {
            result.Alignment = StringAlignment.Center;
            result.LineAlignment = StringAlignment.Center;
         }

         return (result);
      }

      #endregion

      #region Properties

      public int Minimum
      {
         set
         {
            this.minimum = value;
            this.Invalidate();
         }

         get { return (this.minimum); }
      }

      public int Maximum
      {
         set
         {
            this.maximum = value;
            this.Invalidate();
         }

         get { return (this.maximum); }
      }

      public int Value
      {
         set
         {
            if (value > this.Maximum)
            {
               value = this.Maximum;
            }
            else if (value < this.Minimum)
            {
               value = this.Minimum;
            }

            this.value = value;
            this.Invalidate();
         }

         get { return (this.value); }
      }

      public Color BarColor
      {
         set
         {
            this.barColor = value;
            this.Invalidate();
         }

         get { return (this.barColor); }
      }

      public Color TextColor
      {
         set
         {
            this.textColor = value;
            this.Invalidate();
         }

         get { return (this.textColor); }
      }

      public ContentAlignment TextAlign
      {
         set
         {
            this.textAlign = value;
            this.Invalidate();
         }

         get { return (this.textAlign); }
      }

      #endregion

      #region Events

      protected override void OnPaint(PaintEventArgs e)
      {
         e.Graphics.FillRectangle(new SolidBrush(this.BackColor), 0, 0, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);

         e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDark), 1), 0, 0, this.ClientRectangle.Width - 1, 0);
         e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDark), 1), 0, 1, 0, this.ClientRectangle.Height - 2);

         e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlLightLight), 1), 0, this.ClientRectangle.Height - 1, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);
         e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlLightLight), 1), this.ClientRectangle.Width - 1, 1, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);

         int range = this.maximum - this.minimum;
         int offset = this.value - this.minimum;
         int totalBarWidth = this.ClientRectangle.Width - 4;
         int barHeight = this.ClientRectangle.Height - 4;
         int barWidth = 0;

         if (0 != range)
         {
            barWidth = offset * totalBarWidth / range;
         }

         e.Graphics.FillRectangle(new SolidBrush(this.barColor), 2, 2, barWidth, barHeight);

         StringFormat textFormat = this.GetStringFormat(this.TextAlign);
         e.Graphics.DrawString(this.value.ToString(), this.Font, new SolidBrush(this.textColor), this.ClientRectangle, textFormat);
      }

      #endregion

      #region Constructor

      public ProgressBarValueDisplay()
         : base()
      {
         this.Minimum = 0;
         this.Value = 50;
         this.Maximum = 100;

         this.BarColor = Color.Yellow;
         this.TextColor = Color.Black;
         
         this.TextAlign = ContentAlignment.MiddleCenter;

         this.DoubleBuffered = true;
      }

      #endregion
   }
}
