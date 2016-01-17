using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NICBOT.Controls
{
   public class ValueToggleButtonX : Button
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

      #endregion

      #region Properties

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


         StringFormat textFormat = new StringFormat(StringFormat.GenericTypographic);
         textFormat.Alignment = StringAlignment.Center;
         textFormat.LineAlignment = StringAlignment.Center;

         int valueX = (this.ClientRectangle.Width - 1 - this.ValueWidth) / 2;
         int valueY = this.ClientRectangle.Height - 1 - this.ValueEdgeHeight - this.ValueHeight;

         e.Graphics.FillRectangle(new SolidBrush(this.ValueBackColor), valueX, valueY, this.ValueWidth, this.ValueHeight);

         e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDark), 1), valueX + pressOffset, valueY + pressOffset, valueX + this.ValueWidth - 1 + pressOffset, valueY + pressOffset);
         e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDark), 1), valueX + pressOffset, valueY + 1 + pressOffset, valueX + pressOffset, valueY + this.ValueHeight - 2 + pressOffset);

         e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlLightLight), 1), valueX + pressOffset, valueY + this.ValueHeight - 1 + pressOffset, valueX + this.ValueWidth - 1 + pressOffset, valueY + this.ValueHeight - 1 + pressOffset);
         e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlLightLight), 1), valueX + this.ValueWidth - 1 + pressOffset, valueY + 1 + pressOffset, valueX + this.ValueWidth - 1 + pressOffset, valueY + this.ValueHeight - 1 + pressOffset);


         Rectangle valueTextRectangle = new Rectangle(valueX + pressOffset, valueY + pressOffset, this.ValueWidth - 1, this.ValueHeight - 1);
         e.Graphics.DrawString(this.ValueText, this.ValueFont, new SolidBrush(this.ValueForeColor), valueTextRectangle, textFormat);

         Rectangle buttonTextRectangle = new Rectangle(pressOffset, pressOffset, this.ClientRectangle.Width - 1, valueY);
         e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), buttonTextRectangle, textFormat);
      }

      #endregion

      #region Constructor

      public ValueToggleButtonX()
         : base()
      {
         this.ValueFont = System.Drawing.SystemFonts.DialogFont;
         this.ValueBackColor = System.Drawing.SystemColors.Control;
         this.ValueForeColor = System.Drawing.SystemColors.ControlText;
                  
         this.Enter += ValueToggleButton_Enter;
         this.Leave += ValueToggleButton_Leave;

         this.holdTimer = new Timer();
         this.holdTimer.Tick += ValueToggleButton_HoldTimeout;

         this.MouseDown += ValueToggleButton_MouseDown;
         this.MouseUp += ValueToggleButton_MouseUp;
         base.MouseClick += ValueToggleButton_MouseClick;
      }

      #endregion
   }
}
