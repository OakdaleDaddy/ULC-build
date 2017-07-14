namespace Weco.Ui.Controls
{
   using System;
   using System.Collections.Generic;
   using System.Drawing;
   using System.Drawing.Drawing2D;
   using System.Linq;
   using System.Text;
   using System.Threading.Tasks;
   using System.Windows.Forms;

   public class ClockwiseButton : Button
   {
      #region Definition

      public enum Directions
      {
         clockwise,
         counterClockwise,
      }

      public delegate void HoldTimeoutHandler(object sender, HoldTimeoutEventArgs e);

      #endregion

      #region Fields

      private bool focused;
      private bool pressed;

      private Timer holdTimer;
      private bool holdTimeout;

      private Directions direction;

      private float lineWeight;

      private Color holdArrorColor;
      private Color fillColor;
      private Color disabledBackColor;
      private Color disabledFillColor;

      #endregion

      #region Properties

      public event HoldTimeoutHandler HoldTimeout;

      public new event MouseEventHandler MouseClick;

      public int HoldTimeoutInterval { set; get; }
      public int HoldRepeatInterval { set; get; }
      public bool HoldRepeat { set; get; }

      public Directions Direction
      {
         set
         {
            this.direction = value;
            this.Invalidate();
         }

         get { return (this.direction); }
      }
      
      public float LineWeight
      {
         set
         {
            this.lineWeight = value;
            this.Invalidate();
         }

         get { return (this.lineWeight); }
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

      public Color FillColor
      {
         set
         {
            this.fillColor = value;
            this.Invalidate();
         }

         get
         {
            return (this.fillColor);
         }
      }

      public Color DisabledBackColor
      {
         set
         {
            this.disabledBackColor = value;
            this.Invalidate();
         }

         get
         {
            return (this.disabledBackColor);
         }
      }

      public Color DisabledFillColor
      {
         set
         {
            this.disabledFillColor = value;
            this.Invalidate();
         }

         get
         {
            return (this.disabledFillColor);
         }
      }

      #endregion

      #region Helper Functions

      private void PaintEdge(Graphics graphics)
      {
         if (false != this.pressed)
         {
            graphics.DrawRectangle(new Pen(Color.Black), 0, 0, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);
            graphics.DrawRectangle(new Pen(Color.FromKnownColor(KnownColor.ControlDarkDark)), 1, 1, this.ClientRectangle.Width - 3, this.ClientRectangle.Height - 3);
         }
         else if (false != focused)
         {
            graphics.DrawRectangle(new Pen(Color.Black), 0, 0, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);

            graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlLight), 1), 1, 1, this.ClientRectangle.Width - 3, 1);
            graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlLight), 1), 1, 2, 1, this.ClientRectangle.Height - 3);

            graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDarkDark), 1), 2, this.ClientRectangle.Height - 3, this.ClientRectangle.Width - 4, this.ClientRectangle.Height - 3);
            graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDarkDark), 1), this.ClientRectangle.Width - 3, 2, this.ClientRectangle.Width - 3, this.ClientRectangle.Height - 3);

            graphics.DrawLine(new Pen(Color.Black, 1), 0, this.ClientRectangle.Height - 2, this.ClientRectangle.Width - 3, this.ClientRectangle.Height - 2);
            graphics.DrawLine(new Pen(Color.Black, 1), this.ClientRectangle.Width - 2, 1, this.ClientRectangle.Width - 2, this.ClientRectangle.Height - 2);

         }
         else
         {
            graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlLight), 1), 0, 0, this.ClientRectangle.Width - 2, 0);
            graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlLight), 1), 0, 1, 0, this.ClientRectangle.Height - 2);

            graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDarkDark), 1), 1, this.ClientRectangle.Height - 2, this.ClientRectangle.Width - 3, this.ClientRectangle.Height - 2);
            graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDarkDark), 1), this.ClientRectangle.Width - 2, 1, this.ClientRectangle.Width - 2, this.ClientRectangle.Height - 2);

            graphics.DrawLine(new Pen(Color.Black, 1), 0, this.ClientRectangle.Height - 1, this.ClientRectangle.Width - 2, this.ClientRectangle.Height - 1);
            graphics.DrawLine(new Pen(Color.Black, 1), this.ClientRectangle.Width - 1, 0, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);
         }
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

      private void PaintClockwise(Graphics graphics)
      {
         int pressOffset = (false != this.pressed) ? 1 : 0;

         float xUnit = this.ClientRectangle.Width / 7;
         float yUnit = this.ClientRectangle.Height / 9f;

         PointF pB = new PointF(4f * xUnit + pressOffset, 6f * yUnit + pressOffset);
         PointF pC = new PointF(3f * xUnit + pressOffset, 6f * yUnit + pressOffset);
         PointF pD = new PointF(4.5f * xUnit + pressOffset, 8f * yUnit + pressOffset);
         PointF pE = new PointF(6f * xUnit + pressOffset, 6f * yUnit + pressOffset);
         PointF pF = new PointF(5f * xUnit + pressOffset, 6f * yUnit + pressOffset);


         GraphicsPath gp = new GraphicsPath();
         gp.AddArc(-3f * xUnit + pressOffset, yUnit + pressOffset, 8f * xUnit, 10f * yUnit, (float)-90, (float)90);
         gp.AddLine(pF, pE);
         gp.AddLine(pE, pD);
         gp.AddLine(pD, pC);
         gp.AddLine(pC, pB);
         gp.AddArc(-2f * xUnit + pressOffset, yUnit + pressOffset, 6f * xUnit, 10f * yUnit, (float)0, (float)-90);

         Color fillColor = (this.Enabled) ? this.FillColor : this.DisabledFillColor;
         graphics.FillPath(new SolidBrush(fillColor), gp);
         gp.Dispose();


         Pen pen = new Pen(this.ForeColor, this.LineWeight);
         pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
         pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;

         graphics.DrawArc(pen, -3f * xUnit + pressOffset, yUnit + pressOffset, 8f * xUnit, 10f * yUnit, (float)-90, (float)90);
         graphics.DrawArc(pen, -2f * xUnit + pressOffset, yUnit + pressOffset, 6f * xUnit, 10f * yUnit, (float)-90, (float)90);
         graphics.DrawLine(pen, pB, pC);
         graphics.DrawLine(pen, pC, pD);
         graphics.DrawLine(pen, pD, pE);
         graphics.DrawLine(pen, pE, pF);
      }

      private void PaintCounterClockwise(Graphics graphics)
      {
         int pressOffset = (false != this.pressed) ? 1 : 0;

         float xUnit = this.ClientRectangle.Width / 7;
         float yUnit = this.ClientRectangle.Height / 9f;

         PointF pB = new PointF(2f * xUnit + pressOffset, 6f * yUnit + pressOffset);
         PointF pC = new PointF(xUnit + pressOffset, 6f * yUnit + pressOffset);
         PointF pD = new PointF(2.5f * xUnit + pressOffset, 8f * yUnit + pressOffset);
         PointF pE = new PointF(4f * xUnit + pressOffset, 6f * yUnit + pressOffset);
         PointF pF = new PointF(3f * xUnit + pressOffset, 6f * yUnit + pressOffset);


         GraphicsPath gp = new GraphicsPath();
         gp.AddArc(2f * xUnit + pressOffset, yUnit + pressOffset, 8f * xUnit, 10f * yUnit, (float)-90, (float)-90);
         gp.AddLine(pB, pC);
         gp.AddLine(pC, pD);
         gp.AddLine(pD, pE);
         gp.AddLine(pE, pF);
         gp.AddArc(3f * xUnit + pressOffset, yUnit + pressOffset, 6f * xUnit, 10f * yUnit, (float)180, (float)90);

         Color fillColor = (this.Enabled) ? this.FillColor : this.DisabledFillColor;
         graphics.FillPath(new SolidBrush(fillColor), gp);
         gp.Dispose();


         Pen pen = new Pen(this.ForeColor, this.LineWeight);
         pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
         pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;

         graphics.DrawArc(pen, 2f * xUnit + pressOffset, yUnit + pressOffset, 8f * xUnit, 10f * yUnit, (float)-90, (float)-90);
         graphics.DrawArc(pen, 3f * xUnit + pressOffset, yUnit + pressOffset, 6f * xUnit, 10f * yUnit, (float)-90, (float)-90);
         graphics.DrawLine(pen, pB, pC);
         graphics.DrawLine(pen, pC, pD);
         graphics.DrawLine(pen, pD, pE);
         graphics.DrawLine(pen, pE, pF);
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
         int pressOffset = (false != this.pressed) ? 1 : 0;

         Color backColor = (false != this.Enabled) ? this.BackColor : this.DisabledBackColor;
         e.Graphics.FillRectangle(new SolidBrush(backColor), 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height);

         this.PaintEdge(e.Graphics);

         if ((0 != this.HoldTimeoutInterval) && (null != this.HoldTimeout))
         {
            this.PaintHoldIndicator(e.Graphics);
         }

         if (Directions.clockwise == this.Direction)
         {
            this.PaintClockwise(e.Graphics);
         }
         else if (Directions.counterClockwise == this.Direction)
         {
            this.PaintCounterClockwise(e.Graphics);
         }
      }

      #endregion

      #region Constructor

      public ClockwiseButton()      
      {
         this.Enter += this.UpDownButton_Enter;
         this.Leave += this.UpDownButton_Leave;

         this.MouseUp += this.UpDownButton_MouseUp;
         this.MouseDown += this.UpDownButton_MouseDown;
         base.MouseClick += this.UpDownButton_MouseClick;

         this.holdTimer = new Timer();
         this.holdTimer.Tick += this.UpDownButton_HoldTimeout;

         this.Direction = Directions.clockwise;
         this.LineWeight = 3;

         this.HoldArrorColor = Color.FromArgb(64, 64, 64);
         this.FillColor = Color.White;
         this.DisabledBackColor = Color.FromArgb(151, 151, 151);
         this.DisabledFillColor = Color.Gray;
      }

      #endregion

   }
}
