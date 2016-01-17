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

   public class TextPanel : Panel
   {
      #region Definitions

      public delegate void HoldTimeoutHandler2(object sender, HoldTimeoutEventArgs e);

      #endregion

      #region Fields

      private string _valueText;
      private ContentAlignment _valueTextAlignment;

      private bool pressed;
      private Timer holdTimer;

      #endregion

      #region Properties

      public string ValueText 
      { 
         set
         {
            this._valueText = value;
            this.Invalidate();
         }

         get { return (this._valueText); }
      }

      public ContentAlignment ValueTextAlign
      {
         set
         {
            this._valueTextAlignment = value;
            this.Invalidate();
         }

         get { return (this._valueTextAlignment); }
      }

      public event HoldTimeoutHandler2 HoldTimeout;

      public bool HoldTimeoutEnable { set; get; }
      public int HoldTimeoutInterval { set; get; }

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

      #endregion

      #region Event Handlers 
      
      void TextPanel_MouseUp(object sender, MouseEventArgs e)
      {
         this.pressed = false;
         this.holdTimer.Stop();
         
         this.Invalidate();
      }

      void TextPanel_MouseDown(object sender, MouseEventArgs e)
      {
         this.pressed = true;

         if ((false != this.HoldTimeoutEnable) && (0 != this.HoldTimeoutInterval) && (null != this.HoldTimeout))
         {
            this.holdTimer.Interval = this.HoldTimeoutInterval;
            this.holdTimer.Start();
         }

         this.Invalidate();
      }

      void TextPanelHoldTimer_Tick(object sender, EventArgs e)
      {
         this.holdTimer.Stop();

         if (null != this.HoldTimeout)
         {
            HoldTimeoutEventArgs holdEventArg = new HoldTimeoutEventArgs();
            this.HoldTimeout(this, holdEventArg);
         }
      }

      protected override void OnPaint(PaintEventArgs e)
      {
         base.OnPaint(e);

         int pressOffset = (false != this.pressed) ? 1 : 0;

         Rectangle textRectangle = new Rectangle(pressOffset, pressOffset, this.ClientRectangle.Width - (1 + pressOffset), this.ClientRectangle.Height - (1 + pressOffset));
         StringFormat textFormat = this.GetStringFormat(this.ValueTextAlign);
         e.Graphics.DrawString(this.ValueText, this.Font, new SolidBrush(this.ForeColor), textRectangle, textFormat);
      }

      #endregion

      #region Constructor

      public TextPanel()
         : base()
      {
         this.MouseDown += TextPanel_MouseDown;
         this.MouseUp += TextPanel_MouseUp;
      
         this.holdTimer = new Timer();
         this.holdTimer.Tick += TextPanelHoldTimer_Tick;

         this.DoubleBuffered = true;
      }

      #endregion

   }
}
