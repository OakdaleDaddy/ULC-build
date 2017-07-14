
namespace Weco.Ui.Controls
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

   public class PositionIndicator : UserControl
   {
      #region Definition

      public enum TickMotions
      {
         horizontal,
         vertical,
      }

      #endregion

      #region Fields

      private TickMotions tickMotion;
      private int edgeWeight;
      private int tickWeight;

      private int maximumPosition;
      private int minimumPosition;
      private int position;

      private Color edgeColor;
      private Color tickColor;

      #endregion

      #region Properties

      public TickMotions TickMotion
      {
         set
         {
            this.tickMotion = value;
            this.Invalidate();
         }

         get
         {
            return (this.tickMotion);
         }
      }

      public int EdgeWeight
      {
         set
         {
            this.edgeWeight = value;
            this.Invalidate();
         }

         get
         {
            return (this.edgeWeight);
         }
      }

      public int TickWeight
      {
         set
         {
            this.tickWeight = value;
            this.Invalidate();
         }

         get
         {
            return (this.tickWeight);
         }
      }

      public int MaximumPosition
      {
         set
         {
            this.maximumPosition = value;
            this.Invalidate();
         }

         get
         {
            return (this.maximumPosition);
         }
      }

      public int MinimumPosition
      {
         set
         {
            this.minimumPosition = value;
            this.Invalidate();
         }

         get
         {
            return (this.minimumPosition);
         }
      }

      public int Position
      {
         set
         {
            this.position = value;
            this.Invalidate();
         }

         get
         {
            return (this.position);
         }
      }

      public Color EdgeColor
      {
         set
         {
            this.edgeColor = value;
            this.Invalidate();
         }

         get
         {
            return (this.edgeColor);
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
         base.OnPaint(e);

         int width = this.ClientRectangle.Width - 1;
         int height = this.ClientRectangle.Height - 1;


         if (0 != this.EdgeWeight)
         {
            e.Graphics.DrawRectangle(new Pen(this.EdgeColor, (float)this.EdgeWeight), 0, 0, width, height);
            width -= (2 * this.EdgeWeight);
            height -= (2 * this.EdgeWeight);
         }

         e.Graphics.FillRectangle(new SolidBrush(this.BackColor), this.EdgeWeight, this.EdgeWeight, width, height);

         float position = (float)this.Position;
         float maximum = (float)this.MaximumPosition;
         float minimum = (float)this.MinimumPosition;

         if (minimum >= maximum)
         {
            minimum = maximum - 1;
         }

         if (maximum <= minimum)
         {
            maximum = minimum + 1;
         }

         if (position > maximum)
         {
            position = maximum;
         }
         else if (position < minimum)
         {
            position = minimum;
         }

         if (TickMotions.vertical == this.TickMotion)
         {
            if (height > 0)
            {
               float p1X = this.EdgeWeight;
               float p2X = this.EdgeWeight + width;

               float tickWeight = (maximum - minimum) / (float)height;
               float tickDifference = (maximum - position) / tickWeight;
               float pY = (float)this.EdgeWeight + tickDifference;

               e.Graphics.DrawLine(new Pen(this.TickColor, (float)this.TickWeight), p1X, pY, p2X, pY);
            }
         }
         else
         {
            if (width > 0)
            {
               float p1Y = this.EdgeWeight;
               float p2Y = this.EdgeWeight + height;

               float tickWeight = (maximum - minimum) / (float)width;
               float tickDifference = (maximum - position) / tickWeight;
               float pX = (float)this.EdgeWeight + tickDifference;

               e.Graphics.DrawLine(new Pen(this.TickColor, (float)this.TickWeight), pX, p1Y, pX, p2Y);
            }
         }
      }

      #endregion

      #region Constructor

      public PositionIndicator()
         : base()
      {
         this.EdgeColor = Color.Black;
         this.TickColor = Color.Red;

         this.DoubleBuffered = true;
      }

      #endregion
   }
}