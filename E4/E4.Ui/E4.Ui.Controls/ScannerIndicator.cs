
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

   public class ScannerIndicator : UserControl
   {
      #region Definition

      #endregion

      #region Fields

      private int controlEdgeWeight;
      private int indicatorEdgeWeight;
      private int missWeight;
      private int gridWeight;

      private int coordinateBits;
      private UInt32 coordinateValue;

      private Color controlEdgeColor;
      private Color gridColor;
      private Color indicatorEdgeColor;
      private Color missColor;
      private Color tickColor;

      #endregion

      #region Properties

      public int ControlEdgeWeight
      {
         set
         {
            this.controlEdgeWeight = value;
            this.Invalidate();
         }

         get
         {
            return (this.controlEdgeWeight);
         }
      }

      public int IndicatorEdgeWeight
      {
         set
         {
            this.indicatorEdgeWeight = value;
            this.Invalidate();
         }

         get
         {
            return (this.indicatorEdgeWeight);
         }
      }

      public int MissWeight
      {
         set
         {
            this.missWeight = value;
            this.Invalidate();
         }

         get
         {
            return (this.missWeight);
         }
      }

      public int GridWeight
      {
         set
         {
            this.gridWeight = value;
            this.Invalidate();
         }

         get
         {
            return (this.gridWeight);
         }
      }

      public int CoordinateBits
      {
         set
         {
            this.coordinateBits = value;
            this.Invalidate();
         }

         get
         {
            return (this.coordinateBits);
         }
      }

      public UInt32 CoordinateValue
      {
         set
         {
            this.coordinateValue = value;
            this.Invalidate();
         }

         get
         {
            return (this.coordinateValue);
         }
      }

      public Color ControlEdgeColor
      {
         set
         {
            this.controlEdgeColor = value;
            this.Invalidate();
         }

         get
         {
            return (this.controlEdgeColor);
         }
      }

      public Color GridColor
      {
         set
         {
            this.gridColor = value;
            this.Invalidate();
         }

         get
         {
            return (this.gridColor);
         }
      }

      public Color IndicatorEdgeColor
      {
         set
         {
            this.indicatorEdgeColor = value;
            this.Invalidate();
         }

         get
         {
            return (this.indicatorEdgeColor);
         }
      }
 
      public Color MissColor
      {
         set
         {
            this.missColor = value;
            this.Invalidate();
         }

         get
         {
            return (this.missColor);
         }
      }

      public Color TickColor
      {
         set
         {
            this.tickColor = value;
            this.Invalidate();
         }

         get
         {
            return (this.tickColor);
         }
      }

      #endregion

      #region Events

      protected override void OnPaint(PaintEventArgs e)
      {
         int xValue = 0;
         int yValue = 0;
         bool drawElements = false;

         float width = this.ClientRectangle.Width;
         float height = this.ClientRectangle.Height;
         float x = 0;
         float y = 0;

         if ((this.CoordinateBits * 2) <= 32)
         {
            UInt32 yMask = (UInt32)((1 << this.CoordinateBits) - 1);
            UInt32 xMask = (UInt32)(yMask << this.CoordinateBits);

            xValue = (int)((this.CoordinateValue & xMask) >> this.CoordinateBits);
            yValue = (int)(this.CoordinateValue & yMask);

            drawElements = true;
         }

         if (0 != this.ControlEdgeWeight)
         {
            float lineWeightAdjust = (this.ControlEdgeWeight / 2);
            float lineX = x + lineWeightAdjust;
            float lineY = y + lineWeightAdjust;
            float boxWidth = width - this.ControlEdgeWeight;
            float boxHeight = height - this.ControlEdgeWeight;

            e.Graphics.DrawRectangle(new Pen(this.ControlEdgeColor, (float)this.ControlEdgeWeight), lineX, lineY, boxWidth, boxHeight);

            x += this.ControlEdgeWeight;
            y += this.ControlEdgeWeight;
            width -= (2 * this.ControlEdgeWeight);
            height -= (2 * this.ControlEdgeWeight);
         }

         if (false != drawElements)
         {
            if (0 != this.MissWeight)
            {
               Color shownMissColor = this.BackColor;

               if ((0 == xValue) || (0 == yValue))
               {
                  shownMissColor = this.MissColor;
               }

               float lineWeightAdjust = (this.MissWeight / 2);
               float lineX = x + lineWeightAdjust;
               float lineY = y + lineWeightAdjust;
               float boxWidth = width - this.MissWeight;
               float boxHeight = height - this.MissWeight;

               e.Graphics.DrawRectangle(new Pen(shownMissColor, (float)this.MissWeight), lineX, lineY, boxWidth, boxHeight);

               x += this.MissWeight;
               y += this.MissWeight;
               width -= (2 * this.MissWeight);
               height -= (2 * this.MissWeight);
            }

            if (0 != this.IndicatorEdgeWeight)
            {
               float lineWeightAdjust = (this.IndicatorEdgeWeight / 2);
               float lineX = x + lineWeightAdjust;
               float lineY = y + lineWeightAdjust;
               float boxWidth = width - this.IndicatorEdgeWeight;
               float boxHeight = height - this.IndicatorEdgeWeight;

               e.Graphics.DrawRectangle(new Pen(this.IndicatorEdgeColor, (float)this.IndicatorEdgeWeight), lineX, lineY, boxWidth, boxHeight);

               x += this.IndicatorEdgeWeight;
               y += this.IndicatorEdgeWeight;
               width -= (2 * this.IndicatorEdgeWeight);
               height -= (2 * this.IndicatorEdgeWeight);
            }
         }
         
         e.Graphics.FillRectangle(new SolidBrush(this.BackColor), x, y, width, height);

         if (false != drawElements)
         {
            int numberOfRegions = ((1 << this.CoordinateBits) - 1);
            float horizontalSpacing = width / numberOfRegions;
            float veriticalSpacing = height / numberOfRegions;

            if ((0 != xValue) && (0 != yValue))
            {
               float indicatorLineWeightAdjust = (this.IndicatorEdgeWeight / 2);
               float boxX = x + ((xValue - 1) * horizontalSpacing);
               float boxY = y + ((yValue - 1) * veriticalSpacing);

               e.Graphics.FillRectangle(new SolidBrush(this.TickColor), boxX, boxY, horizontalSpacing, veriticalSpacing);
            }

            if (0 != this.GridWeight)
            {
               float lineWeightAdjust = (this.GridWeight / 2);
               float horizontalLength = width - this.GridWeight;
               float veriticalLength = height - this.GridWeight;

               int numberOfLines = (numberOfRegions - 1);
               
               for (int i = 1; i <= numberOfLines; i++)
               {
                  float p1X = x + (i * horizontalSpacing) + lineWeightAdjust;
                  float p1Y = y + lineWeightAdjust;

                  float p2X = x + (i * horizontalSpacing) + lineWeightAdjust;
                  float p2Y = y + veriticalLength + lineWeightAdjust;

                  e.Graphics.DrawLine(new Pen(this.GridColor, (float)this.GridWeight), p1X, p1Y, p2X, p2Y);
               }

               for (int i = 1; i <= numberOfLines; i++)
               {
                  float p1X = x + lineWeightAdjust;
                  float p1Y = y + (i * veriticalSpacing) + lineWeightAdjust;

                  float p2X = x + horizontalLength + lineWeightAdjust;
                  float p2Y = y + (i * veriticalSpacing) + lineWeightAdjust;

                  e.Graphics.DrawLine(new Pen(this.GridColor, (float)this.GridWeight), p1X, p1Y, p2X, p2Y);
               }
            }
         }
      }

      #endregion

      #region Constructor

      public ScannerIndicator()
         : base()
      {
         this.ControlEdgeWeight = 1;
         this.IndicatorEdgeWeight = 1;
         this.MissWeight = 1;
         this.GridWeight = 1;

         this.CoordinateBits = 4;
         this.CoordinateValue = 0;

         this.ControlEdgeColor = Color.Black;
         this.GridColor = Color.Red;
         this.IndicatorEdgeColor = Color.Red;
         this.MissColor = Color.Red;
         this.TickColor = Color.Red;

         this.DoubleBuffered = true;
      }

      #endregion
   }
}