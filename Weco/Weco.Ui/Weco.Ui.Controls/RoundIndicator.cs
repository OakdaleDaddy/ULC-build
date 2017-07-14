namespace Weco.Ui.Controls
{
   using System;
   using System.Collections.Generic;
   using System.ComponentModel;
   using System.Drawing;
   using System.Drawing.Drawing2D;
   using System.Data;
   using System.Linq;
   using System.Text;
   using System.Threading.Tasks;
   using System.Windows.Forms;

   public class RoundIndicator : Control
   {
      #region Fields

      private float outerLineWeight;
      private float innerLineWeight;
      private float indicatorLineWeight;

      private float outerSpacing;
      private float innerSpacing;

      private Color outerColor;
      private Color innerColor;
      private Color indicatorColor;

      #endregion

      #region Properties

      public float OuterLineWeight
      {
         get { return this.outerLineWeight; }
         set { this.outerLineWeight = value; this.Invalidate(); }
      }

      public float InnerLineWeight
      {
         get { return this.innerLineWeight; }
         set { this.innerLineWeight = value; this.Invalidate(); }
      }

      public float IndicatorLineWeight
      {
         get { return this.indicatorLineWeight; }
         set { this.indicatorLineWeight = value; this.Invalidate(); }
      }

      public float OuterSpacing
      {
         get { return this.outerSpacing; }
         set { this.outerSpacing = value; this.Invalidate(); }
      }

      public float InnerSpacing
      {
         get { return this.innerSpacing; }
         set { this.innerSpacing = value; this.Invalidate(); }
      }            

      public Color OuterColor
      {
         get { return this.outerColor; }
         set { this.outerColor = value; this.Invalidate(); }
      }

      public Color InnerColor
      {
         get { return this.innerColor; }
         set { this.innerColor = value; this.Invalidate(); }
      }

      public Color IndicatorColor
      {
         get { return this.indicatorColor; }
         set { this.indicatorColor = value; this.Invalidate(); }
      }

      #endregion

      #region Events

      protected override void OnPaint(PaintEventArgs e)
      {
         float outerWidth = this.ClientRectangle.Width - 1;
         float outerHeight = this.ClientRectangle.Height - 1;

         float innerWidth = outerWidth - (2 * this.OuterSpacing);
         float innerHeight = outerHeight - (2 * this.OuterSpacing);

         float indicatorWidth = innerWidth - (2 * this.InnerSpacing);
         float indicatorHeight = innerHeight - (2 * this.InnerSpacing);

         e.Graphics.FillEllipse(new SolidBrush(this.OuterColor), 0, 0, outerWidth, outerHeight);
         e.Graphics.DrawEllipse(new Pen(this.ForeColor, this.OuterLineWeight), 0, 0, outerWidth, outerHeight);

         if ((innerWidth > 0) && (innerHeight > 0))
         {
            float innerX = this.OuterSpacing;
            float innerY = this.OuterSpacing;
            e.Graphics.FillEllipse(new SolidBrush(this.InnerColor), innerX, innerY, innerWidth, innerHeight);
            e.Graphics.DrawEllipse(new Pen(this.ForeColor, this.InnerLineWeight), innerX, innerY, innerWidth, innerHeight);
         }

         if ((indicatorWidth > 0) && (indicatorHeight > 0))
         {
            float indicatorX = this.OuterSpacing + this.InnerSpacing;
            float indicatorY = this.OuterSpacing + this.InnerSpacing;
            e.Graphics.FillEllipse(new SolidBrush(this.IndicatorColor), indicatorX, indicatorY, indicatorWidth, indicatorHeight);
            e.Graphics.DrawEllipse(new Pen(this.ForeColor, this.IndicatorLineWeight), indicatorX, indicatorY, indicatorWidth, indicatorHeight);
         }
      }

      #endregion

      #region Constructor

      public RoundIndicator()
         : base()
      {
         this.OuterLineWeight = 1;
         this.InnerLineWeight = 1;
         this.IndicatorLineWeight = 1;
         
         this.OuterSpacing = 6;
         this.InnerSpacing = 3;
         
         this.OuterColor = Color.FromArgb(127, 127, 127);
         this.InnerColor = Color.FromArgb(95, 95, 95);
         this.IndicatorColor = Color.FromArgb(0, 60, 15);

         this.DoubleBuffered = true;
      }

      #endregion
   }
}