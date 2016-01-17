using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NICBOT.GUI
{
   public class KeyButton : Button
   {
      private bool focused;
      private bool pressed;

      private bool _showShiftValue;
      private string _shiftValue;
      private int _valueEdge;
      private ContentAlignment _shiftAlignment;

      public bool ShowShiftValue 
      { 
         set
         {
            this._showShiftValue = value;
            this.Invalidate();
         }
         
         get { return(this._showShiftValue); }
      }

      public string ShiftValue
      {
         set
         {
            this._shiftValue = value;
            this.Invalidate();
         }

         get { return (this._shiftValue); }
      }

      public int ValueEdge
      {
         set
         {
            this._valueEdge = value;
            this.Invalidate();
         }

         get { return (this._valueEdge); }
      }

      public ContentAlignment ShiftAlignment
      {
         set
         {
            this._shiftAlignment = value;
            this.Invalidate();
         }

         get { return (this._shiftAlignment); }
      }

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

      public KeyButton()
         : base()
      {
         this.Enter +=KeyButton_Enter;
         this.Leave += KeyButton_Leave;

         this.MouseDown += KeyButton_MouseDown;
         this.MouseUp += KeyButton_MouseUp;

         this.ShiftAlignment = this.TextAlign;
      }

      void KeyButton_MouseUp(object sender, MouseEventArgs e)
      {
         this.pressed = false;
         this.Invalidate();
      }

      void KeyButton_MouseDown(object sender, MouseEventArgs e)
      {
         this.pressed = true;
         this.Invalidate();
      }

      void KeyButton_Leave(object sender, EventArgs e)
      {
         this.focused = false;
         this.Invalidate();
      }

      void KeyButton_Enter(object sender, EventArgs e)
      {
         this.focused = true;
         this.Invalidate();
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

         if (false == this.ShowShiftValue)
         {
            StringFormat textFormat = this.GetStringFormat(this.TextAlign);
            Rectangle textRect = new Rectangle(this.ValueEdge + pressOffset, this.ValueEdge + pressOffset, this.ClientRectangle.Width - (2 * this.ValueEdge) - 1, this.ClientRectangle.Height - (2 * this.ValueEdge) - 1);
            e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), textRect, textFormat);
         }
         else
         {
            int rectWidth = this.ClientRectangle.Width - (2 * this.ValueEdge) - 1;
            int rectHeight = (this.ClientRectangle.Height - (2 * this.ValueEdge)) / 2 - 1;

            StringFormat topFormat = this.GetStringFormat(this.ShiftAlignment);
            Rectangle topRect = new Rectangle(this.ValueEdge + pressOffset, this.ValueEdge + pressOffset, rectWidth, rectHeight);
            e.Graphics.DrawString(this.ShiftValue, this.Font, new SolidBrush(this.ForeColor), topRect, topFormat);

            StringFormat bottomFormat = this.GetStringFormat(this.TextAlign);
            Rectangle bottomRect = new Rectangle(this.ValueEdge + pressOffset, this.ValueEdge + rectHeight + pressOffset, rectWidth, rectHeight);
            e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), bottomRect, bottomFormat);
         }
      }
   }
}
