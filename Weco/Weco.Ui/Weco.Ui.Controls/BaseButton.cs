﻿
namespace Weco.Ui.Controls
{
   using System;
   using System.Drawing;
   using System.Text;
   using System.Windows.Forms;
   using System.Runtime.InteropServices;

   public class BaseButton : Button
   {
      #region Definition

      private const int WM_TOUCH = 0x0240;
      private const int WM_LBUTTONDOWN = 0x0201;
      private const int WM_LBUTTONUP = 0x0202;

      #endregion

      #region Fields

      private bool _focused;
      private bool _pressed;

      private Color _holdArrorColor;
      private Color _disabledBackColor;
      private Color _disabledForeColor;

      #endregion

      #region Properties

      protected bool ShowHold { set; get; }

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

      private bool DecodeTouch(ref Message m)
      {
         bool result = false;

         int inputCount = (int)(m.WParam.ToInt32() & 0xFFFF);
         
         user32.TOUCHINPUT[] inputs; // Array of TOUCHINPUT structures
         inputs = new user32.TOUCHINPUT[inputCount]; // Allocate the storage for the parameters of the per-contact messages

         // Unpack message parameters into the array of TOUCHINPUT structures, each
         // representing a message for one single contact.
         int touchInputSize = Marshal.SizeOf(new user32.TOUCHINPUT());

         if (!user32.GetTouchInputInfo(m.LParam, inputCount, inputs, touchInputSize))
         {
            // Get touch info failed.
            return false;
         }

         user32.TOUCHINPUT ti = inputs[0];

         if ((ti.dwFlags & user32.TOUCHEVENTF_DOWN) != 0)
         {
            if (false == this._pressed)
            {
               IntPtr wParam = new IntPtr(1);
               long lValue = ((ti.cyContact / 100) << 32) | ((ti.cxContact / 100) << 16);
               IntPtr lParam = new IntPtr(lValue);
               user32.SendMessage(this.Handle, WM_LBUTTONDOWN, wParam, lParam);

               result = true;
            }
         }
         else if ((ti.dwFlags & user32.TOUCHEVENTF_UP) != 0)
         {
            // assumes system is generating the BN_CLICKED event on release
            result = true;
         }

         user32.CloseTouchInputHandle(m.LParam);

         return (result);
      }

      private void PaintHoldIndicator(Graphics graphics)
      {
         Point[] upperLeftArrow = null;
         Point[] upperRightArrow = null;
         Point[] lowerLeftArrow = null;
         Point[] lowerRightArrow = null;

         if (false != this._pressed)
         {
            upperLeftArrow = new Point[3] { new Point(3, 3), new Point(10, 3), new Point(3, 10) };
            upperRightArrow = new Point[3] { new Point(this.ClientRectangle.Width - 10, 3), new Point(this.ClientRectangle.Width - 3, 3), new Point(this.ClientRectangle.Width - 3, 10) };
            lowerLeftArrow = new Point[3] { new Point(3, this.ClientRectangle.Height - 11), new Point(3, this.ClientRectangle.Height - 3), new Point(10, this.ClientRectangle.Height - 3) };
            lowerRightArrow = new Point[3] { new Point(this.ClientRectangle.Width - 11, this.ClientRectangle.Height - 3), new Point(this.ClientRectangle.Width - 3, this.ClientRectangle.Height - 3), new Point(this.ClientRectangle.Width - 3, this.ClientRectangle.Height - 12) };
         }
         else if (false != this._focused)
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

      #region Events

      protected override void WndProc(ref Message m)
      {
         bool handled = false;
         bool process = true;

         if (WM_TOUCH == m.Msg)
         {
            handled = this.DecodeTouch(ref m);
         }
         else if (WM_LBUTTONDOWN == m.Msg)
         {
            if (false != this._pressed)
            {
               handled = true;
               process = false;
            }
         }

         if (false != process)
         {
            base.WndProc(ref m);
         }

         if (false != handled)
         {
            m.Result = new System.IntPtr(1);
         }
      }

      void BaseButton_Enter(object sender, EventArgs e)
      {
         this._focused = true;
         this.Invalidate();
      }

      void BaseButton_Leave(object sender, EventArgs e)
      {
         this._focused = false;
         this._pressed = false;
         this.Invalidate();
      }

      void BaseButton_MouseDown(object sender, MouseEventArgs e)
      {
         this._pressed = true;
         this.Invalidate();
      }

      void BaseButton_MouseUp(object sender, MouseEventArgs e)
      {
         this._pressed = false;
         this.Invalidate();
      }

      protected override void OnPaint(PaintEventArgs e)
      {
         int pressOffset = 0;

         Color backColor = (false != this.Enabled) ? this.BackColor : this.DisabledBackColor;
         e.Graphics.FillRectangle(new SolidBrush(backColor), 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height);

         if (false != this._pressed)
         {
            pressOffset = 1;
            e.Graphics.DrawRectangle(new Pen(Color.Black), 0, 0, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);
            e.Graphics.DrawRectangle(new Pen(Color.FromKnownColor(KnownColor.ControlDarkDark)), 1, 1, this.ClientRectangle.Width - 3, this.ClientRectangle.Height - 3);
         }
         else if (false != this._focused)
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

         if (false != this.ShowHold)
         {
            this.PaintHoldIndicator(e.Graphics);
         }

         Color foreColor = (false != this.Enabled) ? this.ForeColor : this.DisabledForeColor;
         Rectangle buttonTextRectangle = new Rectangle(pressOffset, pressOffset, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);
         StringFormat textFormat = this.GetStringFormat(this.TextAlign);
         e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(foreColor), buttonTextRectangle, textFormat);
      }

      #endregion

      #region Constructor

      public BaseButton()
      {
         bool registerResult = user32.RegisterTouchWindow(this.Handle, 0);

         this.Enter += this.BaseButton_Enter;
         this.Leave += this.BaseButton_Leave;

         this.MouseDown += this.BaseButton_MouseDown;
         this.MouseUp += this.BaseButton_MouseUp;

         this.HoldArrorColor = Color.Gray;
         this.DisabledBackColor = Color.FromArgb(151, 151, 151);
         this.DisabledForeColor = Color.Silver;
      }

      #endregion

      #region Access Functions

      protected void Release()
      {
         this._pressed = false;
         this.Invalidate();
      }

      #endregion
   }
}
