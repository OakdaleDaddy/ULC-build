
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

   public class DirectionalValuePanel : Control
   {
      #region Definitions

      public enum Directions
      {
         Forward,
         Reverse,
         Idle,
      }

      #endregion

      #region Fields

      private Directions direction;

      private Color valueBackColor;
      private Color valueForeColor;
      private string valueText;
      private ContentAlignment valueTextAlignment;

      private int arrowWidth;

      private Font idleFont;
      private Color idleBackColor;
      private Color idleForeColor;

      private Font activeFont;
      private Color activeBackColor;
      private Color activeForeColor;

      private string leftArrowText;
      private string rightArrowText;

      #endregion

      #region General Properties

      public Directions Direction
      {
         set
         {
            this.direction = value;
            this.Invalidate();
         }

         get { return (this.direction); }
      }

      public Color ValueBackColor
      {
         set
         {
            this.valueBackColor = value;
            this.Invalidate();
         }

         get { return (this.valueBackColor); }
      }

      public Color ValueForeColor
      {
         set
         {
            this.valueForeColor = value;
            this.Invalidate();
         }

         get { return (this.valueForeColor); }
      }

      public string ValueText
      {
         set
         {
            this.valueText = value;
            this.Invalidate();
         }

         get { return (this.valueText); }
      }

      public ContentAlignment ValueTextAlign
      {
         set
         {
            this.valueTextAlignment = value;
            this.Invalidate();
         }

         get { return (this.valueTextAlignment); }
      }

      public int ArrowWidth
      {
         set
         {
            this.arrowWidth = value;
            this.Invalidate();
         }

         get { return (this.arrowWidth); }
      }

      public int ValueWidth
      {
         get { return (this.ClientRectangle.Width - (2 * this.arrowWidth)); }
      }

      public Font IdleFont
      {
         set
         {
            this.idleFont = value;
            this.Invalidate();
         }

         get { return (this.idleFont); }
      }

      public Color IdleBackColor
      {
         set
         {
            this.idleBackColor = value;
            this.Invalidate();
         }

         get { return (this.idleBackColor); }
      }

      public Color IdleForeColor
      {
         set
         {
            this.idleForeColor = value;
            this.Invalidate();
         }

         get { return (this.idleForeColor); }
      }

      public Font ActiveFont
      {
         set
         {
            this.activeFont = value;
            this.Invalidate();
         }

         get { return (this.activeFont); }
      }

      public Color ActiveBackColor
      {
         set
         {
            this.activeBackColor = value;
            this.Invalidate();
         }

         get { return (this.activeBackColor); }
      }

      public Color ActiveForeColor
      {
         set
         {
            this.activeForeColor = value;
            this.Invalidate();
         }

         get { return (this.activeForeColor); }
      }

      public string LeftArrowText
      {
         set
         {
            this.leftArrowText = value;
            this.Invalidate();
         }

         get { return (this.leftArrowText); }
      }

      public string RightArrowText
      {
         set
         {
            this.rightArrowText = value;
            this.Invalidate();
         }

         get { return (this.rightArrowText); }
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

      #region Event Handlers

      protected override void OnPaint(PaintEventArgs e)
      {
         Rectangle valueRectangle = new Rectangle(this.ArrowWidth, 0, this.ValueWidth - 1, this.ClientRectangle.Height - 1);
         e.Graphics.FillRectangle(new SolidBrush(this.ValueBackColor), valueRectangle);

         e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDark), 1), this.ArrowWidth, 0, this.ArrowWidth + this.ValueWidth - 1, 0);
         e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDark), 1), this.ArrowWidth, 1, this.ArrowWidth, this.ClientRectangle.Height - 2);

         e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlLightLight), 1), this.ArrowWidth, this.ClientRectangle.Height - 1, this.ArrowWidth + this.ValueWidth - 1, this.ClientRectangle.Height - 1);
         e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlLightLight), 1), this.ArrowWidth + this.ValueWidth - 1, 1, this.ArrowWidth + this.ValueWidth - 1, this.ClientRectangle.Height - 1);

         StringFormat textFormat = this.GetStringFormat(this.ValueTextAlign);
         e.Graphics.DrawString(this.ValueText, this.Font, new SolidBrush(this.ValueForeColor), valueRectangle, textFormat);

         Color leftBackColor = this.IdleBackColor;
         Color leftForeColor = this.IdleForeColor;
         Font leftFont = this.IdleFont;

         Color rightBackColor = this.IdleBackColor;
         Color rightForeColor = this.IdleForeColor;
         Font rightFont = this.IdleFont;


         if (Directions.Reverse == this.Direction)
         {
            leftBackColor = this.ActiveBackColor;
            leftForeColor = this.ActiveForeColor;
            leftFont = this.ActiveFont;
         }
         else if (Directions.Forward == this.Direction)
         {
            rightBackColor = this.ActiveBackColor;
            rightForeColor = this.ActiveForeColor;
            rightFont = this.ActiveFont;
         }


         Point leftCenter = new Point { X = 0, Y = ((this.ClientRectangle.Height - 1) / 2) };
         Point rightTop = new Point { X = this.ArrowWidth, Y = 0 };
         Point rightBottom = new Point { X = this.ArrowWidth, Y = this.ClientRectangle.Height - 1 };
         Point[] leftArrowPoints = new Point[] { leftCenter, rightTop, rightBottom };
         e.Graphics.FillPolygon(new SolidBrush(leftBackColor), leftArrowPoints);
         e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDark), 2), leftCenter, rightTop);
         e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDark), 2), rightTop, rightBottom);
         e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDark), 2), rightBottom, leftCenter);

         Rectangle leftArrowTextRectangle = new Rectangle(3, 3, this.ArrowWidth - 6, this.ClientRectangle.Height - 7);
         StringFormat leftArrowTextFormat = this.GetStringFormat(ContentAlignment.MiddleRight);
         e.Graphics.DrawString(this.LeftArrowText, leftFont, new SolidBrush(leftForeColor), leftArrowTextRectangle, leftArrowTextFormat);


         Point leftTop = new Point { X = this.ArrowWidth + this.ValueWidth, Y = 0 };
         Point rightCenter = new Point { X = (2 * this.ArrowWidth) + this.ValueWidth, Y = ((this.ClientRectangle.Height - 1) / 2) };
         Point leftBottom = new Point { X = this.ArrowWidth + this.ValueWidth, Y = this.ClientRectangle.Height - 1 };
         Point[] rightArrowPoints = new Point[] { leftTop, rightCenter, leftBottom };
         e.Graphics.FillPolygon(new SolidBrush(rightBackColor), rightArrowPoints);
         e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDark), 2), leftTop, rightCenter);
         e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDark), 2), rightCenter, leftBottom);
         e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDark), 2), leftBottom, leftTop);

         Rectangle rightArrowTextRectangle = new Rectangle(this.ArrowWidth + this.ValueWidth + 3, 3, this.ArrowWidth - 6, this.ClientRectangle.Height - 7);
         StringFormat rightArrowTextFormat = this.GetStringFormat(ContentAlignment.MiddleLeft);
         e.Graphics.DrawString(this.RightArrowText, rightFont, new SolidBrush(rightForeColor), rightArrowTextRectangle, rightArrowTextFormat);
      }

      #endregion

      #region Constructor

      public DirectionalValuePanel()
         : base()
      {
         this.ValueBackColor = this.BackColor;
         this.ValueForeColor = Color.White;
         this.ValueTextAlign = ContentAlignment.MiddleCenter;

         this.IdleFont = SystemFonts.DefaultFont;
         this.IdleBackColor = this.BackColor;
         this.IdleForeColor = Color.White;

         this.ActiveFont = SystemFonts.DefaultFont;
         this.ActiveBackColor = this.BackColor;
         this.ActiveForeColor = Color.White;

         this.DoubleBuffered = true;
      }

      #endregion

   }
}
