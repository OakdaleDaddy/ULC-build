using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NICBOT.GUI
{
   public partial class ValueButton : Button
   {
      #region Fields

      private bool focused;
      private bool pressed;

      private Timer holdTimer;
      private bool holdTimeout;

      private Font valueFont;
      private Color valueForeColor;
      private Color valueBackColor;

      private int valueWidth;
      private int valueHeight;
      private int valueEdgeHeight;

      private string valueText;

      private int arrowWidth;
      private bool leftArrowVisible;
      private Color leftArrorBackColor;
      private bool rightArrowVisible;
      private Color rightArrowBackColor;

      private Color _disabledBackColor;
      private Color _disabledValueBackColor;
      private Color _disabledForeColor;

      #endregion

      #region Properties

      public delegate void HoldTimeoutHandler(object sender, HoldTimeoutEventArgs e);
      public event HoldTimeoutHandler HoldTimeout;

      public new event MouseEventHandler MouseClick;

      public int HoldTimeoutInterval { set; get; }

      public Font ValueFont
      {
         set
         {
            this.valueFont = value;
            this.Invalidate();
         }

         get
         {
            return (this.valueFont);
         }
      }

      public Color ValueForeColor
      {
         set
         {
            this.valueForeColor = value;
            this.Invalidate();
         }

         get
         {
            return (this.valueForeColor);
         }
      }

      public Color ValueBackColor
      {
         set
         {
            this.valueBackColor = value;

            this.Invalidate();
         }

         get
         {
            return (this.valueBackColor);
         }
      }

      public int ValueWidth
      {
         set
         {
            this.valueWidth = value;
            this.Invalidate();
         }

         get
         {
            return (this.valueWidth);
         }
      }

      public int ValueHeight
      {
         set
         {
            this.valueHeight = value;
            this.Invalidate();
         }

         get
         {
            return (this.valueHeight);
         }
      }

      public int ValueEdgeHeight
      {
         set
         {
            this.valueEdgeHeight = value;
            this.Invalidate();
         }

         get
         {
            return (this.valueEdgeHeight);
         }
      }

      public string ValueText
      {
         set
         {
            this.valueText = value;
            this.Invalidate();
         }

         get
         {
            return (this.valueText);
         }
      }

      public int ArrowWidth
      {
         set
         {
            this.arrowWidth = value;
            this.Invalidate();
         }

         get
         {
            return (this.arrowWidth);
         }
      }

      public bool LeftArrowVisible
      {
         set
         {
            this.leftArrowVisible = value;
            this.Invalidate();
         }

         get
         {
            return (this.leftArrowVisible);
         }
      }

      public Color LeftArrowBackColor
      {
         set
         {
            this.leftArrorBackColor = value;
            this.Invalidate();
         }

         get
         {
            return (this.leftArrorBackColor);
         }
      }

      public bool RightArrowVisible
      {
         set
         {
            this.rightArrowVisible = value;
            this.Invalidate();
         }

         get
         {
            return (this.rightArrowVisible);
         }
      }

      public Color RightArrowBackColor
      {
         set
         {
            this.rightArrowBackColor = value;
            this.Invalidate();
         }

         get
         {
            return (this.rightArrowBackColor);
         }
      }

      public new bool Enabled
      {
         set
         {
            base.Enabled = value;
            this.Invalidate();
         }

         get
         {
            return (base.Enabled);
         }
      }

      public Color DisabledBackColor
      {
         set
         {
            this._disabledBackColor = value;
            this.Invalidate();
         }

         get
         {
            return (this._disabledBackColor);
         }
      }

      public Color DisabledValueBackColor
      {
         set
         {
            this._disabledValueBackColor = value;
            this.Invalidate();
         }

         get
         {
            return (this._disabledValueBackColor);
         }
      }

      public Color DisabledForeColor
      {
         set
         {
            this._disabledForeColor = value;
            this.Invalidate();
         }

         get
         {
            return (this._disabledForeColor);
         }
      }

      #endregion

      #region Helper Functions

      private void PaintHoldIndicator(Graphics graphics)
      {
         Point[] upperLeftArrow = null;
         Point[] upperRightArrow = null;
         Point[] lowerLeftArrow = null;
         Point[] lowerRightArrow = null;

         if (false != this.pressed)
         {
            upperLeftArrow = new Point[3] { new Point(3, 3), new Point(10, 3), new Point(3, 10) };
            upperRightArrow = new Point[3] { new Point(this.ClientRectangle.Width - 10, 3), new Point(this.ClientRectangle.Width - 3, 3), new Point(this.ClientRectangle.Width - 3, 10) };
            lowerLeftArrow = new Point[3] { new Point(3, this.ClientRectangle.Height - 11), new Point(3, this.ClientRectangle.Height - 3), new Point(10, this.ClientRectangle.Height - 3) };
            lowerRightArrow = new Point[3] { new Point(this.ClientRectangle.Width - 11, this.ClientRectangle.Height - 3), new Point(this.ClientRectangle.Width - 3, this.ClientRectangle.Height - 3), new Point(this.ClientRectangle.Width - 3, this.ClientRectangle.Height - 12) };
         }
         else if (false != this.focused)
         {
            upperLeftArrow = new Point[3] { new Point(2, 2), new Point(9, 2), new Point(2, 9) };
            upperRightArrow = new Point[3] { new Point(this.ClientRectangle.Width - 11, 2), new Point(this.ClientRectangle.Width - 4, 2), new Point(this.ClientRectangle.Width - 4, 9) };
            lowerLeftArrow = new Point[3] { new Point(2, this.ClientRectangle.Height - 12), new Point(2, this.ClientRectangle.Height - 4), new Point(9, this.ClientRectangle.Height - 4) };
            lowerRightArrow = new Point[3] { new Point(this.ClientRectangle.Width - 12, this.ClientRectangle.Height - 4), new Point(this.ClientRectangle.Width - 4, this.ClientRectangle.Height - 4), new Point(this.ClientRectangle.Width - 4, this.ClientRectangle.Height - 13) };
         }
         else
         {
            upperLeftArrow = new Point[3] { new Point(1, 1), new Point(8, 1), new Point(1, 8) };
            upperRightArrow = new Point[3] { new Point(this.ClientRectangle.Width - 10, 1), new Point(this.ClientRectangle.Width - 3, 1), new Point(this.ClientRectangle.Width - 3, 8) };
            lowerLeftArrow = new Point[3] { new Point(1, this.ClientRectangle.Height - 11), new Point(1, this.ClientRectangle.Height - 3), new Point(8, this.ClientRectangle.Height - 3) };
            lowerRightArrow = new Point[3] { new Point(this.ClientRectangle.Width - 11, this.ClientRectangle.Height - 3), new Point(this.ClientRectangle.Width - 3, this.ClientRectangle.Height - 3), new Point(this.ClientRectangle.Width - 3, this.ClientRectangle.Height - 12) };
         }

         SolidBrush arrowBrush = new SolidBrush(Color.Gray);
         graphics.FillPolygon(arrowBrush, upperLeftArrow);
         graphics.FillPolygon(arrowBrush, upperRightArrow);
         graphics.FillPolygon(arrowBrush, lowerLeftArrow);
         graphics.FillPolygon(arrowBrush, lowerRightArrow);
      }

      #endregion

      #region Event

      void ValueToggleButton_Enter(object sender, EventArgs e)
      {
         this.focused = true;
      }

      void ValueToggleButton_Leave(object sender, EventArgs e)
      {
         this.holdTimeout = false;
         this.focused = false;
      }

      void ValueToggleButton_MouseUp(object sender, MouseEventArgs e)
      {
         this.pressed = false;
         this.holdTimeout = false;
         this.holdTimer.Stop();
         this.Invalidate();
      }

      void ValueToggleButton_MouseDown(object sender, MouseEventArgs e)
      {
         this.pressed = true;
         if ((0 != this.HoldTimeoutInterval) && (null != this.HoldTimeout))
         {
            this.holdTimer.Interval = this.HoldTimeoutInterval;
            this.holdTimer.Start();
         }
         this.Invalidate();
      }

      void ValueToggleButton_MouseClick(object sender, MouseEventArgs e)
      {
         if (false == this.holdTimeout)
         {
            if (null != this.MouseClick)
            {
               this.MouseClick(this, e);
            }
         }
      }

      void ValueToggleButton_HoldTimeout(object sender, EventArgs e)
      {
         this.holdTimeout = true;
         this.holdTimer.Stop();

         this.pressed = false;
         this.Invalidate();

         if (null != this.HoldTimeout)
         {
            HoldTimeoutEventArgs holdEventArg = new HoldTimeoutEventArgs();
            this.HoldTimeout(this, holdEventArg);
            this.holdTimeout = !holdEventArg.Handled;
         }
      }

      protected override void OnPaint(PaintEventArgs e)
      {
         int pressOffset = 0;

         Color backColor = (false != this.Enabled) ? this.BackColor : this.DisabledBackColor;
         e.Graphics.FillRectangle(new SolidBrush(backColor), 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height);

         if (false != this.pressed)
         {
            pressOffset = 1;
            e.Graphics.DrawRectangle(new Pen(Color.Black), 0, 0, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);
            e.Graphics.DrawRectangle(new Pen(Color.FromKnownColor(KnownColor.ControlDarkDark)), 1, 1, this.ClientRectangle.Width - 3, this.ClientRectangle.Height - 3);
         }
         else if (false != this.focused)
         {
            e.Graphics.DrawRectangle(new Pen(Color.Black), 0, 0, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);

            e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlLight), 1), 1, 1, this.ClientRectangle.Width - 3, 1);
            e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlLight), 1), 1, 2, 1, this.ClientRectangle.Height - 3);

            e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDarkDark), 1), 2, this.ClientRectangle.Height - 3, this.ClientRectangle.Width - 4, this.ClientRectangle.Height - 3);
            e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDarkDark), 1), this.ClientRectangle.Width - 3, 2, this.ClientRectangle.Width - 3, this.ClientRectangle.Height - 3);

            e.Graphics.DrawLine(new Pen(Color.Black, 1), 0, this.ClientRectangle.Height - 2, this.ClientRectangle.Width - 3, this.ClientRectangle.Height - 2);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), this.ClientRectangle.Width - 2, 1, this.ClientRectangle.Width - 2, this.ClientRectangle.Height - 2);

         }
         else
         {
            e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlLight), 1), 0, 0, this.ClientRectangle.Width - 2, 0);
            e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlLight), 1), 0, 1, 0, this.ClientRectangle.Height - 2);

            e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDarkDark), 1), 1, this.ClientRectangle.Height - 2, this.ClientRectangle.Width - 3, this.ClientRectangle.Height - 2);
            e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDarkDark), 1), this.ClientRectangle.Width - 2, 1, this.ClientRectangle.Width - 2, this.ClientRectangle.Height - 2);

            e.Graphics.DrawLine(new Pen(Color.Black, 1), 0, this.ClientRectangle.Height - 1, this.ClientRectangle.Width - 2, this.ClientRectangle.Height - 1);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), this.ClientRectangle.Width - 1, 0, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);
         }

         if ((0 != this.HoldTimeoutInterval) && (null != this.HoldTimeout))
         {
            this.PaintHoldIndicator(e.Graphics);
         }

         StringFormat textFormat = new StringFormat(StringFormat.GenericTypographic);
         textFormat.Alignment = StringAlignment.Center;
         textFormat.LineAlignment = StringAlignment.Center;

         int valueX = (this.ClientRectangle.Width - 1 - this.ValueWidth) / 2;
         int valueY = this.ClientRectangle.Height - 1 - this.ValueEdgeHeight - this.ValueHeight;

         e.Graphics.FillRectangle(new SolidBrush(this.ValueBackColor), valueX + pressOffset, valueY + pressOffset, this.ValueWidth, this.ValueHeight);

         e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDark), 1), valueX + pressOffset, valueY + pressOffset, valueX + this.ValueWidth - 1 + pressOffset, valueY + pressOffset);
         e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDark), 1), valueX + pressOffset, valueY + 1 + pressOffset, valueX + pressOffset, valueY + this.ValueHeight - 2 + pressOffset);

         e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlLightLight), 1), valueX + pressOffset, valueY + this.ValueHeight - 1 + pressOffset, valueX + this.ValueWidth - 1 + pressOffset, valueY + this.ValueHeight - 1 + pressOffset);
         e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlLightLight), 1), valueX + this.ValueWidth - 1 + pressOffset, valueY + 1 + pressOffset, valueX + this.ValueWidth - 1 + pressOffset, valueY + this.ValueHeight - 1 + pressOffset);

         if (false != this.LeftArrowVisible)
         {
            Point leftCenter = new Point { X = valueX - this.ArrowWidth + pressOffset, Y = valueY + (this.ValueHeight / 2) + pressOffset };
            Point rightTop = new Point { X = valueX + pressOffset, Y = valueY + pressOffset };
            Point rightBottom = new Point { X = valueX + pressOffset, Y = valueY + this.ValueHeight + pressOffset };
            Point[] arrowPoints = new Point[] { leftCenter, rightTop, rightBottom };
            e.Graphics.FillPolygon(new SolidBrush(this.LeftArrowBackColor), arrowPoints);
         }

         if (false != this.RightArrowVisible)
         {
            Point leftTop = new Point { X = valueX + this.ValueWidth + pressOffset, Y = valueY + pressOffset };
            Point rightCenter = new Point { X = valueX + this.ValueWidth + this.ArrowWidth + pressOffset, Y = valueY + (this.ValueHeight / 2) + pressOffset };
            Point leftBottom = new Point { X = valueX + this.ValueWidth + pressOffset, Y = valueY + this.ValueHeight + pressOffset };
            Point[] arrowPoints = new Point[] { leftTop, rightCenter, leftBottom };
            e.Graphics.FillPolygon(new SolidBrush(this.RightArrowBackColor), arrowPoints);

#if false // debug output
            e.Graphics.DrawString(leftTop.Y.ToString(), this.Font, new SolidBrush(Color.Black), 0, 0);
#endif
         }

         Color valueForeColor = (false != this.Enabled) ? this.ValueForeColor : this.DisabledValueBackColor;
         Rectangle valueTextRectangle = new Rectangle(valueX + pressOffset, valueY + pressOffset, this.ValueWidth - 1, this.ValueHeight - 1);
         e.Graphics.DrawString(this.ValueText, this.ValueFont, new SolidBrush(valueForeColor), valueTextRectangle, textFormat);

         Color foreColor = (false != this.Enabled) ? this.ForeColor : this.DisabledForeColor;
         Rectangle buttonTextRectangle = new Rectangle(pressOffset, pressOffset, this.ClientRectangle.Width - 1, valueY);
         e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(foreColor), buttonTextRectangle, textFormat);
      }

      #endregion

      #region Constructor

      public ValueButton()
      {
         this.InitializeComponent();

         this.Enter += ValueToggleButton_Enter;
         this.Leave += ValueToggleButton_Leave;

         this.holdTimer = new Timer();
         this.holdTimer.Tick += ValueToggleButton_HoldTimeout;

         this.MouseDown += ValueToggleButton_MouseDown;
         this.MouseUp += ValueToggleButton_MouseUp;
         base.MouseClick += ValueToggleButton_MouseClick;

         this.ValueFont = SystemFonts.DefaultFont;
         this.ValueForeColor = Color.White;
         this.ValueBackColor = Color.Black;

         this.LeftArrowBackColor = Color.Black;
         this.RightArrowBackColor = Color.Black;

         this.DisabledBackColor = Color.FromArgb(151, 151, 151);
         this.DisabledValueBackColor = Color.FromArgb(51, 51, 51);
         this.DisabledForeColor = Color.Silver;
      
         this.DoubleBuffered = true;
      }

      #endregion
   }
}
