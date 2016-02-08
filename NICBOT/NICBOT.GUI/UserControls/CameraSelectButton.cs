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
   public partial class CameraSelectButton : Button
   {
      #region Definitions

      public delegate void HoldTimeoutHandler(object sender, HoldTimeoutEventArgs e);

      #endregion

      #region Fields

      private bool focused;
      private bool pressed;

      private Timer holdTimer;
      private bool holdTimeout;

      private int indicatorEdgeSpace;
      private int indicatorBetweenSpace;

      private bool leftVisible;
      private Color leftColor;

      private bool centerVisible;
      private bool centerVisibleIndependent;
      private int centerLevel;
      private Color centerForeColor;
      private Color centerBackColor;

      private bool rightVisible;
      private Color rightColor;

      private Color holdArrorColor;

      #endregion

      #region Properties

      public event HoldTimeoutHandler HoldTimeout;

      public new event MouseEventHandler MouseClick;

      public CameraLocations Camera { set; get; }
      public bool HoldTimeoutEnable { set; get; }
      public int HoldTimeoutInterval { set; get; }
      public int HoldRepeatInterval { set; get; }
      public bool HoldRepeat { set; get; }
      
      public int IndicatorEdgeSpace
      {
         set
         {
            this.indicatorEdgeSpace = value;
            this.Invalidate();
         }

         get
         {
            return (this.indicatorEdgeSpace);
         }
      }

      public int IndicatorBetweenSpace
      {
         set
         {
            this.indicatorBetweenSpace = value;
            this.Invalidate();
         }

         get
         {
            return (this.indicatorBetweenSpace);
         }
      }

      public bool LeftVisible
      {
         set
         {
            this.leftVisible = value;
            this.Invalidate();
         }

         get
         {
            return (this.leftVisible);
         }
      }

      public Color LeftColor
      {
         set
         {
            this.leftColor = value;
            this.Invalidate();
         }

         get
         {
            return (this.leftColor);
         }
      }

      public bool CenterVisible
      {
         set
         {
            this.centerVisible = value;

            if (false == value)
            {
               this.centerVisibleIndependent = false;
            }
            else if ((false == this.LeftVisible) && (false == this.rightVisible))
            {
               this.centerVisibleIndependent = true;
            }

            this.Invalidate();
         }

         get
         {
            return (this.centerVisible);
         }
      }

      public bool CenterVisibleIndependent
      {
         get
         {
            return (this.centerVisibleIndependent);
         }
      }

      public int CenterLevel
      {
         set
         {
            if (value > 100)
            {
               value = 100;
            }

            if (value < 0)
            {
               value = 0;
            }

            this.centerLevel = value;
            this.Invalidate();
         }

         get
         {
            return (this.centerLevel);
         }
      }

      public Color CenterForeColor
      {
         set
         {
            this.centerForeColor = value;
            this.Invalidate();
         }

         get
         {
            return (this.centerForeColor);
         }
      }

      public Color CenterBackColor
      {
         set
         {
            this.centerBackColor = value;
            this.Invalidate();
         }

         get
         {
            return (this.centerBackColor);
         }
      }

      public bool RightVisible
      {
         set
         {
            this.rightVisible = value;
            this.Invalidate();
         }

         get
         {
            return (this.rightVisible);
         }
      }

      public Color RightColor
      {
         set
         {
            this.rightColor = value;
            this.Invalidate();
         }

         get
         {
            return (this.rightColor);
         }
      }

      public Color HoldArrorColor
      {
         set
         {
            this.holdArrorColor = value;
            this.Invalidate();
         }

         get
         {
            return (this.holdArrorColor);
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

      #region Event Process

      void CameraSelectButton_Leave(object sender, EventArgs e)
      {
         this.holdTimeout = false;
         this.focused = false;
      }

      void CameraSelectButton_Enter(object sender, EventArgs e)
      {
         this.focused = true;
      }

      private void CameraSelectButton_MouseClick(object sender, MouseEventArgs e)
      {
         if (false == this.holdTimeout)
         {
            if (null != this.MouseClick)
            {
               this.MouseClick(this, e);
            }
         }
      }

      private void CameraSelectButton_MouseUp(object sender, MouseEventArgs e)
      {
         this.pressed = false;
         this.holdTimeout = false;
         this.holdTimer.Stop();
         this.Invalidate();
      }

      private void CameraSelectButton_MouseDown(object sender, MouseEventArgs e)
      {
         this.pressed = true;
         if ((false != this.HoldTimeoutEnable) && (0 != this.HoldTimeoutInterval) && (null != this.HoldTimeout))
         {
            this.holdTimer.Interval = this.HoldTimeoutInterval;
            this.holdTimer.Start();
         }
         this.Invalidate();
      }

      private void CameraSelectButton_HoldTimeout(object sender, EventArgs e)
      {
         bool holdRepeat = this.HoldRepeat;

         this.holdTimeout = true;
         this.holdTimer.Stop();

         if (null != this.HoldTimeout)
         {
            HoldTimeoutEventArgs holdEventArg = new HoldTimeoutEventArgs();
            this.HoldTimeout(this, holdEventArg);
            this.holdTimeout = !holdEventArg.Handled;
            this.pressed = !holdEventArg.Handled;
         }

         if ((false != holdRepeat) && (false != this.holdTimeout))
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

         e.Graphics.FillRectangle(new SolidBrush(this.BackColor), 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height);

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

         if ((false != this.HoldTimeoutEnable) && (0 != this.HoldTimeoutInterval) && (null != this.HoldTimeout))
         {
            this.PaintHoldIndicator(e.Graphics);
         }
         
         int spaceSize = (this.indicatorEdgeSpace * 2) + (this.indicatorBetweenSpace * 2);
         int leftRightIndicatorWidth = (int)((this.ClientRectangle.Width - spaceSize) / 4);
         int indicatorHeight = (int)(this.ClientRectangle.Height / 8);
         int centerIndicatorX = this.indicatorEdgeSpace + leftRightIndicatorWidth + this.indicatorBetweenSpace - 1;
         int centerIndicatorWidth = 2 * leftRightIndicatorWidth;
         int rightIndicatorX = this.indicatorEdgeSpace + leftRightIndicatorWidth + this.indicatorBetweenSpace + centerIndicatorWidth + this.indicatorBetweenSpace - 1;

         if (false != this.leftVisible)
         {
            e.Graphics.FillRectangle(new SolidBrush(this.leftColor), this.indicatorEdgeSpace + pressOffset - 1, this.indicatorEdgeSpace + pressOffset - 1, leftRightIndicatorWidth, indicatorHeight);
            e.Graphics.DrawRectangle(new Pen(Color.Black), this.indicatorEdgeSpace + pressOffset - 1, this.indicatorEdgeSpace + pressOffset - 1, leftRightIndicatorWidth, indicatorHeight);
         }

         if (false != this.centerVisible)
         {
            int centerScaleForeX = (centerIndicatorWidth * this.centerLevel) / 100;
            int centerScaleBackX = centerIndicatorWidth - centerScaleForeX;
            e.Graphics.FillRectangle(new SolidBrush(this.centerForeColor), centerIndicatorX + pressOffset, this.indicatorEdgeSpace + pressOffset - 1, centerScaleForeX, indicatorHeight);
            e.Graphics.FillRectangle(new SolidBrush(this.centerBackColor), centerIndicatorX + centerScaleForeX + pressOffset, this.indicatorEdgeSpace + pressOffset - 1, centerScaleBackX, indicatorHeight);
            e.Graphics.DrawRectangle(new Pen(Color.Black), centerIndicatorX + pressOffset, this.indicatorEdgeSpace + pressOffset - 1, centerIndicatorWidth, indicatorHeight);
         }

         if (false != this.rightVisible)
         {
            e.Graphics.FillRectangle(new SolidBrush(this.rightColor), rightIndicatorX + pressOffset, this.indicatorEdgeSpace + pressOffset - 1, leftRightIndicatorWidth, indicatorHeight);
            e.Graphics.DrawRectangle(new Pen(Color.Black), rightIndicatorX + pressOffset, this.indicatorEdgeSpace + pressOffset - 1, leftRightIndicatorWidth, indicatorHeight);
         }

         int textStartY = this.indicatorEdgeSpace + indicatorHeight + 2 - 1;
         int textWidth = this.ClientRectangle.Width - this.indicatorEdgeSpace;
         int textHeight = this.ClientRectangle.Height - textStartY - this.indicatorEdgeSpace;
         Rectangle textRect = new Rectangle(this.indicatorEdgeSpace + pressOffset - 1, textStartY + pressOffset, textWidth, textHeight);

         StringFormat textFormat = this.GetStringFormat(this.TextAlign);
         e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), textRect, textFormat);
      }

      #endregion

      #region Constructor

      public CameraSelectButton()
      {
         this.InitializeComponent();

         this.HoldTimeoutEnable = true;

         this.indicatorEdgeSpace = 4;
         this.indicatorBetweenSpace = 2;

         this.Enter += this.CameraSelectButton_Enter;
         this.Leave += this.CameraSelectButton_Leave;

         this.holdTimer = new Timer();
         this.holdTimer.Tick += this.CameraSelectButton_HoldTimeout;

         this.MouseDown += this.CameraSelectButton_MouseDown;
         this.MouseUp += this.CameraSelectButton_MouseUp;
         base.MouseClick += this.CameraSelectButton_MouseClick;

         this.HoldArrorColor = Color.Gray;
      }

      #endregion
   }

}
