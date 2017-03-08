
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

   public class ValueCycleButton : Button
   {
      #region Fields

      private bool focused;
      private bool pressed;

      private Timer holdTimer;
      private bool holdTimeout;

      private bool _timedSelection;
      private Timer selectionTimer;
      private bool selectionTimeout;

      private int selectedOption;
      private Font _optionSelectedFont;
      private Color _optionSelectedForeColor;
      private Color _optionSelectedBackColor;

      private Font _optionNonSelectedFont;
      private Color _optionNonSelectedForeColor;
      private Color _optionNonSelectedBackColor;

      private Color _optionSelectingForeColor;
      private Color _optionSelectingBackColor;

      private int _optionWidth;
      private int _optionHeight;

      private int _optionOptionSpace;
      private int _optionEdgeSpace;

      private string _optionAText;
      private string _optionBText;
      private string _optionCText;

      private Color _disabledBackColor;
      private Color _disabledOptionBackColor;
      private Color _disabledForeColor;

      #endregion

      #region Properties

      public delegate void HoldTimeoutHandler(object sender, HoldTimeoutEventArgs e);
      public delegate void SelectionTimeoutHandler(object sender, EventArgs e);
      public event HoldTimeoutHandler HoldTimeout;
      public event SelectionTimeoutHandler SelectionTimeout;

      public new event MouseEventHandler MouseClick;

      public bool HoldEnable { set; get; }
      public int HoldTimeoutInterval { set; get; }

      public bool TimedSelection
      {
         set
         {
            this._timedSelection = value;

            if (false == value)
            {
               this.selectionTimer.Stop();
               this.selectionTimeout = false;
            }
         }

         get
         {
            return (this._timedSelection);
         }
      }

      public bool SelectionTimedOut { get { return (this.selectionTimeout); } }
      public int SelectionTimeoutInterval { set; get; }

      public int SelectedOption
      {
         set
         {
            if ((0 != this.SelectionTimeoutInterval) && (false != this.TimedSelection))
            {
               this.selectionTimer.Stop();
               this.selectionTimeout = false;
               this.selectionTimer.Interval = this.SelectionTimeoutInterval;
               this.selectionTimer.Start();
            }
            else
            {
               this.selectionTimeout = true;
            }

            this.selectedOption = value;
            this.Invalidate();
         }

         get
         {
            return (this.selectedOption);
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


      public Color OptionSelectingForeColor
      {
         set
         {
            this._optionSelectingForeColor = value;
            this.Invalidate();
         }

         get
         {
            return (this._optionSelectingForeColor);
         }
      }


      public Color OptionSelectingBackColor
      {
         set
         {
            this._optionSelectingBackColor = value;
            this.Invalidate();
         }

         get
         {
            return (this._optionSelectingBackColor);
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

      public int OptionOptionSpace
      {
         set
         {
            this._optionOptionSpace = value;
            this.Invalidate();
         }

         get
         {
            return (this._optionOptionSpace);
         }
      }

      public int OptionEdgeSpace
      {
         set
         {
            this._optionEdgeSpace = value;
            this.Invalidate();
         }

         get
         {
            return (this._optionEdgeSpace);
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

      public string OptionCText
      {
         set
         {
            this._optionCText = value;
            this.Invalidate();
         }

         get
         {
            return (this._optionCText);
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

      public Color DisabledOptionBackColor
      {
         set
         {
            this._disabledOptionBackColor = value;
            this.Invalidate();
         }

         get
         {
            return (this._disabledOptionBackColor);
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

      private void ValueToggleButton_HoldTimeout(object sender, EventArgs e)
      {
         //Tracer.WriteHigh(TraceGroup.GUI, null, "hold timeout");

         this.holdTimeout = true;
         this.holdTimer.Stop();

         if (null != this.HoldTimeout)
         {
            HoldTimeoutEventArgs holdEventArg = new HoldTimeoutEventArgs();
            this.HoldTimeout(this, holdEventArg);
            this.holdTimeout = holdEventArg.Handled;
         }
      }

      private void SelectionTimer_Timeout(object sender, EventArgs e)
      {
         this.selectionTimer.Stop();

         if (false == this.pressed)
         {
            this.selectionTimeout = true;
            this.Invalidate();

            if (null != this.SelectionTimeout)
            {
               this.SelectionTimeout(this, e);
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


         StringFormat textFormat = new StringFormat(StringFormat.GenericTypographic);
         textFormat.Alignment = StringAlignment.Center;
         textFormat.LineAlignment = StringAlignment.Center;

         #region Option A

         Color optionATextColor = default(Color);
         Color enabledOptionABackTextColor = default(Color);

         if (1 == this.SelectedOption)
         {
            if ((false == this.TimedSelection) || (false != this.selectionTimeout))
            {
               optionATextColor = this.OptionSelectedForeColor;
               enabledOptionABackTextColor = this.OptionSelectedBackColor;
            }
            else
            {
               optionATextColor = this.OptionSelectingForeColor;
               enabledOptionABackTextColor = this.OptionSelectingBackColor;
            }
         }
         else
         {
            optionATextColor = this.OptionNonSelectedForeColor;
            enabledOptionABackTextColor = this.OptionNonSelectedBackColor;
         }

         Color optionABackTextColor = (false != this.Enabled) ? enabledOptionABackTextColor : this.DisabledOptionBackColor;

         int optionALeft = ((this.ClientRectangle.Width - 1 - this.OptionWidth) / 2) + pressOffset;
         int optionATop = this.OptionEdgeSpace + pressOffset;

         e.Graphics.FillRectangle(new SolidBrush(optionABackTextColor), optionALeft, optionATop, this.OptionWidth, this.OptionHeight);
         this.DrawFixed3D(e, optionALeft, optionATop, this.OptionWidth, this.OptionHeight);

         Font optionAFont = (1 == this.SelectedOption) ? this.OptionSelectedFont : this.OptionNonSelectedFont;
         Rectangle optionATextRectangle = new Rectangle(optionALeft, optionATop, this.OptionWidth - 1, this.OptionHeight - 1);
         e.Graphics.DrawString(this.OptionAText, optionAFont, new SolidBrush(optionATextColor), optionATextRectangle, textFormat);

         #endregion

         #region Option B

         Color optionBTextColor = default(Color);
         Color enabledOptionBBackTextColor = default(Color);

         if (2 == this.SelectedOption)
         {
            if ((false == this.TimedSelection) || (false != this.selectionTimeout))
            {
               optionBTextColor = this.OptionSelectedForeColor;
               enabledOptionBBackTextColor = this.OptionSelectedBackColor;
            }
            else
            {
               optionBTextColor = this.OptionSelectingForeColor;
               enabledOptionBBackTextColor = this.OptionSelectingBackColor;
            }
         }
         else
         {
            optionBTextColor = this.OptionNonSelectedForeColor;
            enabledOptionBBackTextColor = this.OptionNonSelectedBackColor;
         }

         Color optionBBackTextColor = (false != this.Enabled) ? enabledOptionBBackTextColor : this.DisabledOptionBackColor;

         int optionBLeft = ((this.ClientRectangle.Width - 1 - this.OptionWidth) / 2) + pressOffset;
         int optionBTop = this.OptionEdgeSpace + this.OptionHeight + this.OptionOptionSpace + pressOffset;

         e.Graphics.FillRectangle(new SolidBrush(optionBBackTextColor), optionBLeft, optionBTop, this.OptionWidth, this.OptionHeight);
         this.DrawFixed3D(e, optionBLeft, optionBTop, this.OptionWidth, this.OptionHeight);

         Font optionBFont = (2 == this.SelectedOption) ? this.OptionSelectedFont : this.OptionNonSelectedFont;
         Rectangle optionBTextRectangle = new Rectangle(optionBLeft, optionBTop, this.OptionWidth - 1, this.OptionHeight - 1);
         e.Graphics.DrawString(this.OptionBText, optionBFont, new SolidBrush(optionBTextColor), optionBTextRectangle, textFormat);

         #endregion

         #region Option C

         Color optionCTextColor = default(Color);
         Color enabledOptionCBackTextColor = default(Color);

         if (3 == this.SelectedOption)
         {
            if ((false == this.TimedSelection) || (false != this.selectionTimeout))
            {
               optionCTextColor = this.OptionSelectedForeColor;
               enabledOptionCBackTextColor = this.OptionSelectedBackColor;
            }
            else
            {
               optionCTextColor = this.OptionSelectingForeColor;
               enabledOptionCBackTextColor = this.OptionSelectingBackColor;
            }
         }
         else
         {
            optionCTextColor = this.OptionNonSelectedForeColor;
            enabledOptionCBackTextColor = this.OptionNonSelectedBackColor;
         }

         Color optionCBackTextColor = (false != this.Enabled) ? enabledOptionCBackTextColor : this.DisabledOptionBackColor;

         int optionCLeft = ((this.ClientRectangle.Width - 1 - this.OptionWidth) / 2) + pressOffset;
         int optionCTop = this.OptionEdgeSpace + (this.OptionHeight * 2) + (this.OptionOptionSpace * 2) + pressOffset;

         e.Graphics.FillRectangle(new SolidBrush(optionCBackTextColor), optionCLeft, optionCTop, this.OptionWidth, this.OptionHeight);
         this.DrawFixed3D(e, optionCLeft, optionCTop, this.OptionWidth, this.OptionHeight);

         Font optionCFont = (3 == this.SelectedOption) ? this.OptionSelectedFont : this.OptionNonSelectedFont;
         Rectangle optionCTextRectangle = new Rectangle(optionCLeft, optionCTop, this.OptionWidth - 1, this.OptionHeight - 1);
         e.Graphics.DrawString(this.OptionCText, optionCFont, new SolidBrush(optionCTextColor), optionCTextRectangle, textFormat);

         #endregion

         #region Main Text

         Color foreColor = (false != this.Enabled) ? this.ForeColor : this.DisabledForeColor;
         int textLeft = pressOffset;
         int textTop = this.OptionEdgeSpace + (this.OptionHeight * 3) + (this.OptionOptionSpace * 2) + (2 * pressOffset);
         int textWidth = this.ClientRectangle.Width - 1 - 4;
         int textHeight = this.ClientRectangle.Height - 1 - textTop - 2;
         Rectangle buttonTextRectangle = new Rectangle(textLeft, textTop, textWidth, textHeight);
         e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(foreColor), buttonTextRectangle, textFormat);

         #endregion
      }

      #endregion

      #region Constructor

      public ValueCycleButton()
      {
         this.OptionSelectedFont = System.Drawing.SystemFonts.DefaultFont;
         this.OptionSelectedForeColor = Color.White;
         this.OptionSelectedBackColor = Color.Black;

         this.OptionNonSelectedFont = System.Drawing.SystemFonts.DefaultFont;
         this.OptionNonSelectedForeColor = Color.White;
         this.OptionNonSelectedBackColor = Color.Black;

         this.OptionEdgeSpace = 3;
         this.OptionHeight = this.OptionNonSelectedFont.Height;
         this.OptionWidth = 15;

         this.Enter += ValueToggleButton_Enter;
         this.Leave += ValueToggleButton_Leave;

         this.holdTimer = new Timer();
         this.holdTimer.Tick += ValueToggleButton_HoldTimeout;

         this.selectionTimer = new Timer();
         this.selectionTimer.Tick += SelectionTimer_Timeout;

         this.MouseDown += ValueToggleButton_MouseDown;
         this.MouseUp += ValueToggleButton_MouseUp;
         base.MouseClick += ValueToggleButton_MouseClick;

         this.DisabledBackColor = Color.FromArgb(151, 151, 151);
         this.DisabledOptionBackColor = Color.FromArgb(51, 51, 51);
         this.DisabledForeColor = Color.Silver;

         this.OptionEdgeSpace = 3;
         this.OptionHeight = this.OptionNonSelectedFont.Height;
         this.OptionWidth = 15;

         this.TimedSelection = false;
         this.SelectionTimeoutInterval = 0;
         this.selectionTimeout = true;
      }

      #endregion
   }
}
