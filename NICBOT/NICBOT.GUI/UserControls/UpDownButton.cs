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
   public partial class UpDownButton : Button
   {
      #region Definitions

      #endregion

      #region Fields

      private bool focused;
      private bool pressed;

      private Timer holdTimer;
      private bool holdTimeout;

      private bool upDown;
      private bool highlightVisible;
      private bool textVisible;
      private int textOffset;
      private int edgeSpace;
      private int highLightOffset;
      private int highLightWeight;

      private Color arrowColor;
      private Color arrowHighlightColor;

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

      public bool UpDown
      {
         set
         {
            this.upDown = value;
            this.Invalidate();
         }

         get { return (this.upDown); }
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

      #region Event Handlers

      void UpDownButton_MouseUp(object sender, MouseEventArgs e)
      {
         this.pressed = false;
         this.holdTimer.Stop();
         this.Invalidate();
      }

      void UpDownButton_MouseDown(object sender, MouseEventArgs e)
      {
         this.pressed = true;
         this.Invalidate();

         if (0 != this.HoldTimeoutInterval)
         {
            this.holdTimer.Interval = this.HoldTimeoutInterval;
            this.holdTimer.Start();
         }
      }

      private void UpDownButton_MouseClick(object sender, MouseEventArgs e)
      {
         if (false == this.holdTimeout)
         {
            if (null != this.MouseClick)
            {
               this.MouseClick(this, e);
            }
         }
      }

      void UpDownButton_Enter(object sender, EventArgs e)
      {
         this.focused = true;
         this.Invalidate();
      }

      void UpDownButton_Leave(object sender, EventArgs e)
      {
         this.focused = false;
         this.Invalidate();
      }

      void UpDownButton_HoldTimeout(object sender, EventArgs e)
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

         if (false != this.upDown)
         {
            Point topCenter = new Point { X = (this.ClientRectangle.Width / 2) + pressOffset, Y = this.edgeSpace + pressOffset };
            Point bottomLeft = new Point { X = this.edgeSpace + pressOffset, Y = (this.ClientRectangle.Height - this.edgeSpace) + pressOffset };
            Point bottomRight = new Point { X = (this.ClientRectangle.Width - this.edgeSpace) + pressOffset, Y = (this.ClientRectangle.Height - this.edgeSpace) + pressOffset };
            Point[] arrowPoints = new Point[] { topCenter, bottomLeft, bottomRight };
            Color arrowColor = (false != this.Enabled) ? this.ArrowColor : this.DisabledArrowColor;
            e.Graphics.FillPolygon(new SolidBrush(arrowColor), arrowPoints);

            double arrowHeight = this.ClientRectangle.Height - (2 * this.edgeSpace);
            double arrowWidth = this.ClientRectangle.Width - (2 * this.edgeSpace);
            double arrowAngle = Math.Atan((arrowWidth / 2)/ arrowHeight);
            double highlightHeight = arrowHeight - (2 * this.highLightOffset);
            double highlightBase = (int)(Math.Tan(arrowAngle) * highlightHeight);

            if (false != this.highlightVisible)
            {
               Point highlightTop = new Point { X = (this.ClientRectangle.Width / 2) + pressOffset, Y = this.edgeSpace + this.highLightOffset + pressOffset };
               Point highlightBottom = new Point { X = (this.ClientRectangle.Width / 2) - (int)highlightBase + pressOffset, Y = this.ClientRectangle.Height - this.edgeSpace - this.highLightOffset + pressOffset };
               Color highlightColor = (false != this.Enabled) ? this.ArrowHighlightColor : this.DisabledForeColor;
               e.Graphics.DrawLine(new Pen(highlightColor, this.HighLightWeight), highlightTop, highlightBottom);
            }

            if (false != this.textVisible)
            {
               StringFormat textFormat = new StringFormat(StringFormat.GenericTypographic);
               textFormat.Alignment = StringAlignment.Center;
               textFormat.LineAlignment = StringAlignment.Far;
               Rectangle textRectangle = new Rectangle(pressOffset, pressOffset, this.ClientRectangle.Width - 1 + pressOffset, (this.ClientRectangle.Height - this.edgeSpace - this.textOffset) + pressOffset);
               Color textColor = (false != this.Enabled) ? this.ForeColor : this.DisabledForeColor;
               e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(textColor), textRectangle, textFormat);
            }
         }
         else
         {
            Point topLeft = new Point { X = this.edgeSpace + pressOffset, Y = this.edgeSpace + pressOffset };
            Point topRight = new Point { X = (this.ClientRectangle.Width - this.edgeSpace) + pressOffset, Y = this.edgeSpace + pressOffset };
            Point bottomCenter = new Point { X = (this.ClientRectangle.Width / 2) + pressOffset, Y = (this.ClientRectangle.Height - this.edgeSpace) + pressOffset };
            Point[] arrowPoints = new Point[] { topLeft, topRight, bottomCenter };
            Color arrowColor = (false != this.Enabled) ? this.ArrowColor : this.DisabledArrowColor;
            e.Graphics.FillPolygon(new SolidBrush(arrowColor), arrowPoints);

            double arrowHeight = this.ClientRectangle.Height - (2 * this.edgeSpace);
            double arrowWidth = this.ClientRectangle.Width - (2 * this.edgeSpace);
            double arrowAngle = Math.Atan((arrowWidth / 2) / arrowHeight);
            double highlightHeight = arrowHeight - (2 * this.highLightOffset);
            double highlightBase = (int)(Math.Tan(arrowAngle) * highlightHeight);

            if (false != this.highlightVisible)
            {
               Point highlightTop = new Point { X = (this.ClientRectangle.Width / 2) + (int)highlightBase + pressOffset, Y = this.edgeSpace + this.highLightOffset + pressOffset };
               Point highlightBottom = new Point { X = (this.ClientRectangle.Width / 2) + pressOffset, Y = this.ClientRectangle.Height - this.edgeSpace - this.highLightOffset + pressOffset };
               Color highlightColor = (false != this.Enabled) ? this.ArrowHighlightColor : this.DisabledForeColor;
               e.Graphics.DrawLine(new Pen(highlightColor, this.HighLightWeight), highlightTop, highlightBottom);
            }

            if (false != this.textVisible)
            {
               StringFormat textFormat = new StringFormat(StringFormat.GenericTypographic);
               textFormat.Alignment = StringAlignment.Center;
               textFormat.LineAlignment = StringAlignment.Near;
               Rectangle textRectangle = new Rectangle(pressOffset, this.edgeSpace + this.textOffset + pressOffset, this.ClientRectangle.Width - 1 + pressOffset, this.ClientRectangle.Height - (this.edgeSpace + pressOffset + 2));
               Color textColor = (false != this.Enabled) ? this.ForeColor : this.DisabledForeColor;
               e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(textColor), textRectangle, textFormat);
            }
         }
      }

      #endregion

      public UpDownButton()
      {
         this.InitializeComponent();

         this.HighlightVisible = true;
         this.HighLightWeight = 1;

         this.Enter += this.UpDownButton_Enter;
         this.Leave += this.UpDownButton_Leave;

         this.MouseUp += this.UpDownButton_MouseUp;
         this.MouseDown += this.UpDownButton_MouseDown;
         base.MouseClick += this.UpDownButton_MouseClick;

         this.holdTimer = new Timer();
         this.holdTimer.Tick += this.UpDownButton_HoldTimeout;

         this.ArrowColor = Color.Black;
         this.ArrowHighlightColor = Color.DarkGray;

         this.DisabledBackColor = Color.FromArgb(151, 151, 151);
         this.DisabledArrowColor = Color.FromArgb(51, 51, 51);
         this.DisabledForeColor = Color.Silver;
      }

   }
}
