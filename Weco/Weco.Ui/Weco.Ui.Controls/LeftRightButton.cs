
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

   public class LeftRightButton : Button
   {
      #region Definitions

      #endregion

      #region Fields

      private bool focused;
      private bool pressed;

      private Timer holdTimer;
      private bool holdTimeout;

      private bool leftRight;
      private bool highlightVisible;
      private bool textVisible;
      private int textOffset;
      private int edgeSpace;
      private int highLightOffset;
      private int highLightWeight;

      private Color arrowColor;
      private Color arrowHighlightColor;

      private Color _holdArrorColor;

      private Color _disabledBackColor;
      private Color _disabledArrowColor;
      private Color _disabledForeColor;

      #endregion

      #region Properties

      public delegate void HoldTimeoutHandler(object sender, HoldTimeoutEventArgs e);
      public event HoldTimeoutHandler HoldTimeout;

      public new event MouseEventHandler MouseClick;

      public int HoldTimeoutInterval { set; get; }
      public int HoldRepeatInterval { set; get; }
      public bool HoldRepeat { set; get; }

      public bool LeftRight
      {
         set
         {
            this.leftRight = value;
            this.Invalidate();
         }

         get { return (this.leftRight); }
      }

      public bool HighlightVisible
      {
         set
         {
            this.highlightVisible = value;
            this.Invalidate();
         }

         get { return (this.highlightVisible); }
      }

      public bool TextVisible
      {
         set
         {
            this.textVisible = value;
            this.Invalidate();
         }

         get { return (this.textVisible); }
      }

      public int TextOffset
      {
         set
         {
            this.textOffset = value;
            this.Invalidate();
         }

         get { return (this.textOffset); }
      }

      public int EdgeSpace
      {
         set
         {
            this.edgeSpace = value;
            this.Invalidate();
         }

         get { return (this.edgeSpace); }
      }

      public Color ArrowColor
      {
         set
         {
            this.arrowColor = value;
            this.Invalidate();
         }

         get { return (this.arrowColor); }
      }

      public int HighLightOffset
      {
         set
         {
            this.highLightOffset = value;
            this.Invalidate();
         }

         get { return (this.highLightOffset); }
      }

      public int HighLightWeight
      {
         set
         {
            if (value < 1)
            {
               value = 1;
            }

            this.highLightWeight = value;
            this.Invalidate();
         }

         get { return (this.highLightWeight); }
      }

      public Color ArrowHighlightColor
      {
         set
         {
            this.arrowHighlightColor = value;
            this.Invalidate();
         }

         get { return (this.arrowHighlightColor); }
      }

      public Color HoldArrorColor
      {
         set
         {
            this._holdArrorColor = value;
            this.Invalidate();
         }

         get
         {
            return (this._holdArrorColor);
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

      public Color DisabledArrowColor
      {
         set
         {
            this._disabledArrowColor = value;
            this.Invalidate();
         }

         get
         {
            return (this._disabledArrowColor);
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

         SolidBrush arrowBrush = new SolidBrush(this.HoldArrorColor);
         graphics.FillPolygon(arrowBrush, upperLeftArrow);
         graphics.FillPolygon(arrowBrush, upperRightArrow);
         graphics.FillPolygon(arrowBrush, lowerLeftArrow);
         graphics.FillPolygon(arrowBrush, lowerRightArrow);
      }

      #endregion

      #region Event Handlers

      void LeftRightButton_MouseUp(object sender, MouseEventArgs e)
      {
         this.pressed = false;
         this.holdTimer.Stop();
         this.Invalidate();
      }

      void LeftRightButton_MouseDown(object sender, MouseEventArgs e)
      {
         this.pressed = true;
         this.Invalidate();

         if (0 != this.HoldTimeoutInterval)
         {
            this.holdTimer.Interval = this.HoldTimeoutInterval;
            this.holdTimer.Start();
         }
      }

      private void LeftRightButton_MouseClick(object sender, MouseEventArgs e)
      {
         if (false == this.holdTimeout)
         {
            if (null != this.MouseClick)
            {
               this.MouseClick(this, e);
            }
         }
      }

      void LeftRightButton_Enter(object sender, EventArgs e)
      {
         this.focused = true;
         this.Invalidate();
      }

      void LeftRightButton_Leave(object sender, EventArgs e)
      {
         this.focused = false;
         this.Invalidate();
      }

      void LeftRightButton_HoldTimeout(object sender, EventArgs e)
      {
         bool holdRepeat = this.HoldRepeat;

         this.holdTimeout = true;
         this.holdTimer.Stop();

         if (false == holdRepeat)
         {
            this.pressed = false;
            this.Invalidate();
         }

         if (null != this.HoldTimeout)
         {
            this.HoldTimeout(this, new HoldTimeoutEventArgs());
         }

         if (false != holdRepeat)
         {
            if (0 != this.holdTimer.Interval)
            {
               this.holdTimer.Interval = this.HoldRepeatInterval;
               this.holdTimer.Start();
            }
         }
      }

      private double ToDegrees(double radians)
      {
         double result = radians * 180.0 / Math.PI;
         return (result);
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
         else if (false != focused)
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

         if (false != this.leftRight)
         {
            // left

            Point leftMiddle = new Point { X = this.edgeSpace + pressOffset, Y = (this.ClientRectangle.Height / 2) + pressOffset };
            Point rightTop = new Point { X = this.ClientRectangle.Width - this.edgeSpace + pressOffset, Y = this.edgeSpace + pressOffset };
            Point rightBottom = new Point { X = this.ClientRectangle.Width - this.edgeSpace + pressOffset, Y = this.ClientRectangle.Height - this.edgeSpace + pressOffset };
            Point[] arrowPoints = new Point[] { leftMiddle, rightTop, rightBottom };
            Color arrowColor = (false != this.Enabled) ? this.ArrowColor : this.DisabledArrowColor;
            e.Graphics.FillPolygon(new SolidBrush(arrowColor), arrowPoints);

            double arrowHeight = this.ClientRectangle.Height - (2 * this.edgeSpace);
            double arrowWidth = this.ClientRectangle.Width - (2 * this.edgeSpace);
            double arrowAngle = Math.Atan((arrowHeight / 2) / arrowWidth);
            double highlightBase = (arrowWidth) - (2 * this.highLightOffset);
            double highlightHeight = (int)(Math.Tan(arrowAngle) * highlightBase);

            if (false != this.highlightVisible)
            {
               Point highlightTop = new Point { X = this.edgeSpace + this.highLightOffset + (int)highlightBase + pressOffset, Y = (this.ClientRectangle.Height / 2) - (int)highlightHeight + pressOffset };
               Point highlightBottom = new Point { X = this.edgeSpace + this.highLightOffset + pressOffset, Y = (this.ClientRectangle.Height / 2) + pressOffset };
               Color highlightColor = (false != this.Enabled) ? this.ArrowHighlightColor : this.DisabledForeColor;
               e.Graphics.DrawLine(new Pen(highlightColor, this.HighLightWeight), highlightTop, highlightBottom);
            }

            if (false != this.textVisible)
            {
            }
         }
         else
         {
            // right

            Point rightMiddle = new Point { X = this.ClientRectangle.Width - this.edgeSpace + pressOffset, Y = (this.ClientRectangle.Height / 2) + pressOffset };
            Point leftTop = new Point { X = this.edgeSpace + pressOffset, Y = this.edgeSpace + pressOffset };
            Point leftBottom = new Point { X = this.edgeSpace + pressOffset, Y = this.ClientRectangle.Height - this.edgeSpace + pressOffset };
            Point[] arrowPoints = new Point[] { rightMiddle, leftTop, leftBottom };
            Color arrowColor = (false != this.Enabled) ? this.ArrowColor : this.DisabledArrowColor;
            e.Graphics.FillPolygon(new SolidBrush(arrowColor), arrowPoints);

            double arrowHeight = this.ClientRectangle.Height - (2 * this.edgeSpace);
            double arrowWidth = this.ClientRectangle.Width - (2 * this.edgeSpace);
            double arrowAngle = Math.Atan((arrowHeight / 2) / arrowWidth);
            double highlightBase = (arrowWidth) - (2 * this.highLightOffset);
            double highlightHeight = (int)(Math.Tan(arrowAngle) * highlightBase);

            if (false != this.highlightVisible)
            {
               Point highlightTop = new Point { X = this.ClientRectangle.Width - this.edgeSpace - this.highLightOffset + pressOffset, Y = (this.ClientRectangle.Height / 2) };
               Point highlightBottom = new Point { X = ClientRectangle.Width - this.edgeSpace - this.highLightOffset - (int)highlightBase + pressOffset, Y = (this.ClientRectangle.Height / 2) + (int)highlightHeight  + pressOffset };
               Color highlightColor = (false != this.Enabled) ? this.ArrowHighlightColor : this.DisabledForeColor;
               e.Graphics.DrawLine(new Pen(highlightColor, this.HighLightWeight), highlightTop, highlightBottom);
            }

            if (false != this.textVisible)
            {
            }
         }
      }

      #endregion

      public LeftRightButton()
      {
         this.HighlightVisible = true;
         this.HighLightWeight = 1;

         this.Enter += this.LeftRightButton_Enter;
         this.Leave += this.LeftRightButton_Leave;

         this.MouseUp += this.LeftRightButton_MouseUp;
         this.MouseDown += this.LeftRightButton_MouseDown;
         base.MouseClick += this.LeftRightButton_MouseClick;

         this.holdTimer = new Timer();
         this.holdTimer.Tick += this.LeftRightButton_HoldTimeout;

         this.ArrowColor = Color.Black;
         this.ArrowHighlightColor = Color.DarkGray;

         this.HoldArrorColor = Color.Gray;

         this.DisabledBackColor = Color.FromArgb(151, 151, 151);
         this.DisabledArrowColor = Color.FromArgb(51, 51, 51);
         this.DisabledForeColor = Color.Silver;
      }

   }
}
