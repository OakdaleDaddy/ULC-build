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
   public class ValueToggleButton : Button
   {
      #region Fields

      private bool focused;
      private bool pressed;

      private Timer holdTimer;
      private bool holdTimeout;

      private bool optionASelected;
      private bool optionBSelected;

      private Font _optionSelectedFont;
      private Color _optionSelectedForeColor;
      private Color _optionSelectedBackColor;

      private Font _optionNonSelectedFont;
      private Color _optionNonSelectedForeColor;
      private Color _optionNonSelectedBackColor;

      private int _optionWidth;
      private int _optionHeight;

      private int _optionCenterWidth;
      private int _optionEdgeHeight;

      private string _optionAText;
      private string _optionBText;

      private Color _disabledBackColor;
      private Color _disabledOptionBackColor;
      private Color _disabledForeColor;

      #endregion

      #region Properties

      public delegate void HoldTimeoutHandler(object sender, HoldTimeoutEventArgs e);
      public event HoldTimeoutHandler HoldTimeout;

      public new event MouseEventHandler MouseClick;

      public bool HoldEnable { set; get; }
      public int HoldTimeoutInterval { set; get; }

      public bool AutomaticToggle { set; get; }

      public bool OptionASelected
      {
         set
         {
            this.optionASelected = value;

            if (false != this.AutomaticToggle)
            {
               this.optionBSelected = !value;
            }

            this.Invalidate();
         }

         get
         {
            return (this.optionASelected);
         }
      }

      public bool OptionBSelected
      {
         set
         {
            this.optionBSelected = value;

            if (false != this.AutomaticToggle)
            {
               this.optionASelected = !value;
            }

            this.Invalidate();
         }

         get
         {
            return (this.optionBSelected);
         }
      }

      public Font OptionSelectedFont
      {
         set
         {
            this._optionSelectedFont = value;
            this.Invalidate();
         }

         get
         {
            return (this._optionSelectedFont);
         }
      }

      public Color OptionSelectedForeColor
      {
         set
         {
            this._optionSelectedForeColor = value;
            this.Invalidate();
         }

         get
         {
            return (this._optionSelectedForeColor);
         }
      }

      public Color OptionSelectedBackColor
      {
         set
         {
            this._optionSelectedBackColor = value;
            this.Invalidate();
         }

         get
         {
            return (this._optionSelectedBackColor);
         }
      }

      public Font OptionNonSelectedFont
      {
         set
         {
            this._optionNonSelectedFont = value;
            this.Invalidate();
         }

         get
         {
            return (this._optionNonSelectedFont);
         }
      }

      public Color OptionNonSelectedForeColor
      {
         set
         {
            this._optionNonSelectedForeColor = value;
            this.Invalidate();
         }

         get
         {
            return (this._optionNonSelectedForeColor);
         }
      }

      public Color OptionNonSelectedBackColor
      {
         set
         {
            this._optionNonSelectedBackColor = value;

            this.Invalidate();
         }

         get
         {
            return (this._optionNonSelectedBackColor);
         }
      }

      public int OptionWidth
      {
         set
         {
            this._optionWidth = value;
            this.Invalidate();
         }

         get
         {
            return (this._optionWidth);
         }
      }

      public int OptionHeight
      {
         set
         {
            this._optionHeight = value;
            this.Invalidate();
         }

         get
         {
            return (this._optionHeight);
         }
      }

      public int OptionCenterWidth
      {
         set
         {
            this._optionCenterWidth = value;
            this.Invalidate();
         }

         get
         {
            return (this._optionCenterWidth);
         }
      }

      public int OptionEdgeHeight
      {
         set
         {
            this._optionEdgeHeight = value;
            this.Invalidate();
         }

         get
         {
            return (this._optionEdgeHeight);
         }
      }      

      public string OptionAText
      {
         set
         {
            this._optionAText = value;
            this.Invalidate();
         }

         get
         {
            return (this._optionAText);
         }
      }

      public string OptionBText
      {
         set
         {
            this._optionBText = value;
            this.Invalidate();
         }

         get
         {
            return (this._optionBText);
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
            return(base.Enabled);
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
            return(this._disabledBackColor);
         }
      }
      
      public Color DisabledOptionBackColor
      {
         set 
         {
            this._disabledOptionBackColor = value;
            this.Invalidate();
         }

         get
         {
            return(this._disabledOptionBackColor);
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
            return(this._disabledForeColor);
         }
      }
    
      #endregion

      #region Helper Functions

      private void DrawFixed3D(PaintEventArgs e, int left, int top, int width, int height)
      {
         width--;
         height--;

         e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDark), 1), left, top, left + width - 1, top);
         e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDark), 1), left, top + 1, left, top + height - 1);

         e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlLightLight), 1), left, top + height, left + width, top + height);
         e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlLightLight), 1), left + width, top, left + width, top + height);
      }

      #endregion

      #region Events

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
         this.holdTimeout = false;

         if ((false != this.HoldEnable) && (0 != this.HoldTimeoutInterval) && (null != this.HoldTimeout))
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

         int centerOffset = this.OptionCenterWidth / 2;
         int centerX = this.ClientRectangle.Width / 2;
         int optionY = this.ClientRectangle.Height - 1 - this.OptionEdgeHeight - this.OptionHeight;


         int optionAX = centerX - centerOffset - this.OptionWidth;
         Color enabledOptionABackTextColor = (false != this.OptionASelected) ? this.OptionSelectedBackColor : this.OptionNonSelectedBackColor;
         Color optionABackTextColor = (false != this.Enabled) ? enabledOptionABackTextColor : this.DisabledOptionBackColor;
         e.Graphics.FillRectangle(new SolidBrush(optionABackTextColor), optionAX + pressOffset, optionY + pressOffset, this.OptionWidth, this.OptionHeight);

         this.DrawFixed3D(e, optionAX + pressOffset, optionY + pressOffset, this.OptionWidth, this.OptionHeight);

         Rectangle optionATextRectangle = new Rectangle(optionAX + pressOffset, optionY + pressOffset, this.OptionWidth - 1, this.OptionHeight - 1);
         Font optionAFont = (false != this.OptionASelected) ? this.OptionSelectedFont : this.OptionNonSelectedFont;
         Color optionATextColor = (false != this.OptionASelected) ? this.OptionSelectedForeColor : this.OptionNonSelectedForeColor;
         e.Graphics.DrawString(this.OptionAText, optionAFont, new SolidBrush(optionATextColor), optionATextRectangle, textFormat);


         int optionBX = centerX + centerOffset;
         Color enabledOptionBBackTextColor = (false != this.OptionBSelected) ? this.OptionSelectedBackColor : this.OptionNonSelectedBackColor; ;
         Color optionBBackTextColor = (false != this.Enabled) ? enabledOptionBBackTextColor : this.DisabledOptionBackColor;
         e.Graphics.FillRectangle(new SolidBrush(optionBBackTextColor), optionBX + pressOffset, optionY + pressOffset, this.OptionWidth, this.OptionHeight);

         this.DrawFixed3D(e, optionBX + pressOffset, optionY + pressOffset, this.OptionWidth, this.OptionHeight);

         Rectangle optionBTextRectangle = new Rectangle(optionBX + pressOffset, optionY + pressOffset, this.OptionWidth - 1, this.OptionHeight - 1);
         Font optionBFont = (false != this.OptionBSelected) ? this.OptionSelectedFont : this.OptionNonSelectedFont;
         Color optionBTextColor = (false != this.OptionBSelected) ? this.OptionSelectedForeColor : this.OptionNonSelectedForeColor;
         e.Graphics.DrawString(this.OptionBText, optionBFont, new SolidBrush(optionBTextColor), optionBTextRectangle, textFormat);

         Color foreColor = (false != this.Enabled) ? this.ForeColor : this.DisabledForeColor;
         Rectangle buttonTextRectangle = new Rectangle(pressOffset, pressOffset, this.ClientRectangle.Width - 1, optionY);
         e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(foreColor), buttonTextRectangle, textFormat);
      }

      #endregion

      #region Constructor

      public ValueToggleButton()
      {
         this.OptionSelectedFont = System.Drawing.SystemFonts.DefaultFont;
         this.OptionSelectedForeColor = Color.White;
         this.OptionSelectedBackColor = Color.Black;

         this.OptionNonSelectedFont = System.Drawing.SystemFonts.DefaultFont;
         this.OptionNonSelectedForeColor = Color.White;
         this.OptionNonSelectedBackColor = Color.Black;

         this.Enter += ValueToggleButton_Enter;
         this.Leave += ValueToggleButton_Leave;

         this.holdTimer = new Timer();
         this.holdTimer.Tick += ValueToggleButton_HoldTimeout;

         this.MouseDown += ValueToggleButton_MouseDown;
         this.MouseUp += ValueToggleButton_MouseUp;
         base.MouseClick += ValueToggleButton_MouseClick;

         this.DisabledBackColor = Color.FromArgb(151, 151, 151);
         this.DisabledOptionBackColor = Color.FromArgb(51, 51, 51);
         this.DisabledForeColor = Color.Silver;

         this.DoubleBuffered = true;
         this.AutomaticToggle = true;
      }
      
      #endregion
   }
}
