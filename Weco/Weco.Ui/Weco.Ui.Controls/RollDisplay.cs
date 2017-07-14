
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

   public class RollDisplay : UserControl
   {
      #region Definition

      public enum GaugeIndicators
      {
         arrow,
         rectangle,
      }

      #endregion

      #region Fields

      private Color gaugeBackColor;
      private Color gaugeEdgeColor;
      private int gaugeOuterNumberSpace;
      private int gaugeInnerNumberSpace;
      private float gaugeEdgeWeight;
      private GaugeIndicators gaugeIndicator;

      private Color indicatorColor;
      private float indicatorWeight;
      private float pinWeight;

      private bool showPosition;
      private float positionRoll;
      private float positionCcwLimit;
      private float positionCwLimit;

      private bool showPushIndicators;
      private float pushIndicatorEdgeWeight;
      private Color pushIndicatorEdgeColor;
      private Color leftPushIndicatorColor;
      private Color rightPushIndicatorColor;
      private float leftPushIndicatorAngle;
      private float rightPushIndicatorAngle;

      private bool rollVisible;
      private int rollWidth;
      private int rollHeight;
      private Font rollFont;
      private Color rollBackColor;
      private Color rollForeColor;

      private bool pitchVisible;
      private int pitchWidth;
      private int pitchHeight;
      private Font pitchFont;
      private Color pitchBackColor;
      private Color pitchForeColor;

      private bool yawVisible;
      private int yawWidth;
      private int yawHeight;
      private Font yawFont;
      private Color yawBackColor;
      private Color yawForeColor;

      private float roll;
      private float pitch;
      private float yaw;

      #endregion

      #region Properties

      public Color GaugeBackColor
      {
         get { return this.gaugeBackColor; }
         set { this.gaugeBackColor = value; this.Invalidate(); }
      }

      public Color GaugeEdgeColor
      {
         get { return this.gaugeEdgeColor; }
         set { this.gaugeEdgeColor = value; this.Invalidate(); }
      }

      public int GaugeOuterNumberSpace
      {
         get { return this.gaugeOuterNumberSpace; }
         set { this.gaugeOuterNumberSpace = value; this.Invalidate(); }
      }

      public int GaugeInnerNumberSpace
      {
         get { return this.gaugeInnerNumberSpace; }
         set { this.gaugeInnerNumberSpace = value; this.Invalidate(); }
      }

      /// <summary>
      /// limit to 1 to 15
      /// </summary>
      public float GaugeEdgeWeight
      {
         get { return this.gaugeEdgeWeight; }

         set
         {
            if (value > 15f)
            {
               value = 15f;
            }
            else if (value < 1f)
            {
               value = 1f;
            }

            this.gaugeEdgeWeight = value; 
            this.Invalidate();
         }
      }

      public GaugeIndicators GaugeIndicator
      {
         get { return this.gaugeIndicator; }
         set { this.gaugeIndicator = value; this.Invalidate(); }
      }

      public bool RollVisible
      {
         get { return this.rollVisible; }
         set { this.rollVisible = value; Invalidate(); }
      }

      public int RollWidth
      {
         get { return this.rollWidth; }
         set { this.rollWidth = value; Invalidate(); }
      }

      public int RollHeight
      {
         get { return this.rollHeight; }
         set { this.rollHeight = value; Invalidate(); }
      }

      public Font RollFont
      {
         get
         {
            return (this.rollFont);
         }

         set
         {
            this.rollFont = value;
            this.Invalidate();
         }
      }

      public Color RollBackColor
      {
         get
         {
            return (this.rollBackColor);
         }

         set
         {
            this.rollBackColor = value;
            this.Invalidate();
         }
      }

      public Color RollForeColor
      {
         get
         {
            return (this.rollForeColor);
         }

         set
         {
            this.rollForeColor = value;
            this.Invalidate();
         }
      }

      public bool PitchVisible
      {
         get { return this.pitchVisible; }
         set { this.pitchVisible = value; Invalidate(); }
      }

      public int PitchWidth
      {
         get { return this.pitchWidth; }
         set { this.pitchWidth = value; Invalidate(); }
      }

      public int PitchHeight
      {
         get { return this.pitchHeight; }
         set { this.pitchHeight = value; Invalidate(); }
      }

      public Font PitchFont
      {
         get
         {
            return (this.pitchFont);
         }

         set
         {
            this.pitchFont = value;
            this.Invalidate();
         }
      }

      public Color PitchBackColor
      {
         get
         {
            return (this.pitchBackColor);
         }

         set
         {
            this.pitchBackColor = value;
            this.Invalidate();
         }
      }

      public Color PitchForeColor
      {
         get
         {
            return (this.pitchForeColor);
         }

         set
         {
            this.pitchForeColor = value;
            this.Invalidate();
         }
      }

      public bool YawVisible
      {
         get { return this.yawVisible; }
         set { this.yawVisible = value; Invalidate(); }
      }

      public int YawWidth
      {
         get { return this.yawWidth; }
         set { this.yawWidth = value; Invalidate(); }
      }

      public int YawHeight
      {
         get { return this.yawHeight; }
         set { this.yawHeight = value; Invalidate(); }
      }

      public Font YawFont
      {
         get
         {
            return (this.yawFont);
         }

         set
         {
            this.yawFont = value;
            this.Invalidate();
         }
      }

      public Color YawBackColor
      {
         get
         {
            return (this.yawBackColor);
         }

         set
         {
            this.yawBackColor = value;
            this.Invalidate();
         }
      }

      public Color YawForeColor
      {
         get
         {
            return (this.yawForeColor);
         }

         set
         {
            this.yawForeColor = value;
            this.Invalidate();
         }
      }

      public Color IndicatorColor
      {
         get { return this.indicatorColor; }
         set { this.indicatorColor = value; this.Invalidate(); }
      }

      public float IndicatorWeight
      {
         get { return this.indicatorWeight; }
         set { this.indicatorWeight = value; this.Invalidate(); }
      }

      public float PinWeight
      {
         get { return this.pinWeight; }
         set { this.pinWeight = value; this.Invalidate(); }
      }

      public bool ShowPosition
      {
         get { return this.showPosition; }
         set { this.showPosition = value; this.Invalidate(); }
      }

      public float PositionRoll
      {
         get { return this.positionRoll; }
         set { this.positionRoll = value; this.Invalidate(); }
      }

      public float PositionCcwLimit
      {
         get { return this.positionCcwLimit; }
         set { this.positionCcwLimit = value; this.Invalidate(); }
      }

      public float PositionCwLimit
      {
         get { return this.positionCwLimit; }
         set { this.positionCwLimit = value; this.Invalidate(); }
      }

      public bool ShowPushIndicators
      {
         get { return this.showPushIndicators; }
         set { this.showPushIndicators = value; this.Invalidate(); }
      }

      public float PushIndicatorEdgeWeight
      {
         get { return this.pushIndicatorEdgeWeight; }
         set { this.pushIndicatorEdgeWeight = value; this.Invalidate(); }
      }

      public Color PushIndicatorEdgeColor
      {
         get { return this.pushIndicatorEdgeColor; }
         set { this.pushIndicatorEdgeColor = value; this.Invalidate(); }
      }

      public Color LeftPushIndicatorColor
      {
         get { return this.leftPushIndicatorColor; }
         set { this.leftPushIndicatorColor = value; this.Invalidate(); }
      }

      public Color RightPushIndicatorColor
      {
         get { return this.rightPushIndicatorColor; }
         set { this.rightPushIndicatorColor = value; this.Invalidate(); }
      }

      public float LeftPushIndicatorAngle
      {
         get { return this.leftPushIndicatorAngle; }
         set { this.leftPushIndicatorAngle = value; this.Invalidate(); }
      }

      public float RightPushIndicatorAngle
      {
         get { return this.rightPushIndicatorAngle; }
         set { this.rightPushIndicatorAngle = value; this.Invalidate(); }
      }
      
      public float Roll
      {
         get { return this.roll; }
         set { this.roll = value; this.Invalidate(); }
      }

      public float Pitch
      {
         get { return this.pitch; }
         set { this.pitch = value; this.Invalidate(); }
      }

      public float Yaw
      {
         get { return this.yaw; }
         set { this.yaw = value; this.Invalidate(); }
      }

      #endregion

      #region Helper Functions

      private double ToRadians(double degress)
      {
         double result = degress * Math.PI / 180.0;
         return (result);
      }

      private void DrawFixed3D(Graphics graphics, float left, float top, float width, float height)
      {
         width--;
         height--;

         graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDark), 1), left, top, left + width - 1, top);
         graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDark), 1), left, top + 1, left, top + height - 1);

         graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlLightLight), 1), left, top + height, left + width, top + height);
         graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlLightLight), 1), left + width, top, left + width, top + height);
      }

      private void PaintGauge(Graphics graphics, float centerX, float centerY, float circleDiameter)
      {
         float circleRadius = circleDiameter / 2;
         float x0 = (float)(centerX - circleRadius);
         float y0 = (float)(centerY - circleRadius);

         graphics.FillEllipse(new SolidBrush(this.GaugeBackColor), x0, y0, circleDiameter, circleDiameter);
         graphics.DrawEllipse(new Pen(this.GaugeEdgeColor, this.GaugeEdgeWeight), x0, y0, circleDiameter, circleDiameter);
      }

      private void PaintHours(Graphics graphics, float centerX, float centerY, float circleDiameter)
      {
         float circleRadius = circleDiameter / 2;
         float labelRadius = circleRadius + this.GaugeInnerNumberSpace;
         StringFormat format = new StringFormat(StringFormat.GenericTypographic);

         format.Alignment = StringAlignment.Near;
         format.LineAlignment = StringAlignment.Far;
         float x1 = (float)(centerX + (0.5 * labelRadius));
         float y1 = (float)(centerY - (0.87 * labelRadius));
         graphics.DrawString("1", this.Font, new SolidBrush(Color.White), x1, y1, format);

         format.Alignment = StringAlignment.Near;
         format.LineAlignment = StringAlignment.Far;
         float x2 = (float)(centerX + (0.87 * labelRadius));
         float y2 = (float)(centerY - (0.5 * labelRadius));
         graphics.DrawString("2", this.Font, new SolidBrush(Color.White), x2, y2, format);

         format.Alignment = StringAlignment.Near;
         format.LineAlignment = StringAlignment.Center;
         float x3 = (float)(centerX + labelRadius);
         float y3 = (float)(centerY);
         graphics.DrawString("3", this.Font, new SolidBrush(Color.White), x3, y3, format);

         format.Alignment = StringAlignment.Near;
         format.LineAlignment = StringAlignment.Near;
         float x4 = (float)(centerX + (0.87 * labelRadius));
         float y4 = (float)(centerY + (0.5 * labelRadius));
         graphics.DrawString("4", this.Font, new SolidBrush(Color.White), x4, y4, format);

         format.Alignment = StringAlignment.Near;
         format.LineAlignment = StringAlignment.Near;
         float x5 = (float)(centerX + (0.5 * labelRadius));
         float y5 = (float)(centerY + (0.87 * labelRadius));
         graphics.DrawString("5", this.Font, new SolidBrush(Color.White), x5, y5, format);

         format.Alignment = StringAlignment.Center;
         format.LineAlignment = StringAlignment.Near;
         float x6 = (float)(centerX);
         float y6 = (float)(centerY + labelRadius);
         graphics.DrawString("6", this.Font, new SolidBrush(Color.White), x6, y6, format);

         format.Alignment = StringAlignment.Far;
         format.LineAlignment = StringAlignment.Near;
         float x7 = (float)(centerX - (0.5 * labelRadius));
         float y7 = (float)(centerY + (0.87 * labelRadius));
         graphics.DrawString("7", this.Font, new SolidBrush(Color.White), x7, y7, format);

         format.Alignment = StringAlignment.Far;
         format.LineAlignment = StringAlignment.Near;
         float x8 = (float)(centerX - (0.87 * labelRadius));
         float y8 = (float)(centerY + (0.5 * labelRadius));
         graphics.DrawString("8", this.Font, new SolidBrush(Color.White), x8, y8, format);

         format.Alignment = StringAlignment.Far;
         format.LineAlignment = StringAlignment.Center;
         float x9 = (float)(centerX - labelRadius);
         float y9 = (float)(centerY);
         graphics.DrawString("9", this.Font, new SolidBrush(Color.White), x9, y9, format);

         format.Alignment = StringAlignment.Far;
         format.LineAlignment = StringAlignment.Far;
         float x10 = (float)(centerX - (0.87 * labelRadius));
         float y10 = (float)(centerY - (0.5 * labelRadius));
         graphics.DrawString("10", this.Font, new SolidBrush(Color.White), x10, y10, format);

         format.Alignment = StringAlignment.Far;
         format.LineAlignment = StringAlignment.Far;
         float x11 = (float)(centerX - (0.5 * labelRadius));
         float y11 = (float)(centerY - (0.87 * labelRadius));
         graphics.DrawString("11", this.Font, new SolidBrush(Color.White), x11, y11, format);

         format.Alignment = StringAlignment.Center;
         format.LineAlignment = StringAlignment.Far;
         float x12 = (float)(centerX);
         float y12 = (float)(centerY - labelRadius);
         graphics.DrawString("12", this.Font, new SolidBrush(Color.White), x12, y12, format);
      }

      private void PaintRoll(Graphics graphics)
      {
         StringFormat textFormat = new StringFormat(StringFormat.GenericTypographic);
         textFormat.Alignment = StringAlignment.Center;
         textFormat.LineAlignment = StringAlignment.Center;

         float left = this.Width - this.RollWidth - 1;
         float top = 1;

         RectangleF displayRectangle = new RectangleF(left, top, this.RollWidth, this.RollHeight);

         graphics.FillRectangle(new SolidBrush(this.RollBackColor), displayRectangle);
         this.DrawFixed3D(graphics, left, top, this.RollWidth, this.RollHeight);
         string rollString = string.Format("{0}°", this.Roll);
         graphics.DrawString(rollString, this.RollFont, new SolidBrush(this.RollForeColor), displayRectangle, textFormat);

         StringFormat labelFormat = new StringFormat(StringFormat.GenericTypographic);
         labelFormat.Alignment = StringAlignment.Center;
         labelFormat.LineAlignment = StringAlignment.Near;

         SizeF labelSize = graphics.MeasureString("Roll", this.Font);
         float labelLeft = this.Width - labelSize.Width - 1;
         float labelTop = this.RollHeight;
         RectangleF labelRectangle = new RectangleF(labelLeft, labelTop, labelSize.Width, labelSize.Height);

         graphics.DrawString("Roll", this.Font, new SolidBrush(Color.White), labelRectangle, labelFormat);
      }

      private void PaintPitch(Graphics graphics)
      {
         StringFormat textFormat = new StringFormat(StringFormat.GenericTypographic);
         textFormat.Alignment = StringAlignment.Center;
         textFormat.LineAlignment = StringAlignment.Center;

         float left = this.Width - this.PitchWidth - 1;
         float top = this.Height - this.PitchHeight - 1;

         RectangleF displayRectangle = new RectangleF(left, top, this.PitchWidth, this.PitchHeight);

         graphics.FillRectangle(new SolidBrush(this.PitchBackColor), displayRectangle);
         this.DrawFixed3D(graphics, left, top, this.PitchWidth, this.PitchHeight);
         string pitchString = string.Format("{0}°", this.Pitch);
         graphics.DrawString(pitchString, this.PitchFont, new SolidBrush(this.PitchForeColor), displayRectangle, textFormat);

         SizeF labelSize = graphics.MeasureString("Pitch", this.Font);
         float labelLeft = this.Width - labelSize.Width - 1;
         float labelTop = this.Height - this.PitchHeight - labelSize.Height - 1;
         RectangleF labelRectangle = new RectangleF(labelLeft, labelTop, labelSize.Width, labelSize.Height);

         StringFormat labelFormat = new StringFormat(StringFormat.GenericTypographic);
         labelFormat.Alignment = StringAlignment.Center;
         labelFormat.LineAlignment = StringAlignment.Center;

         graphics.DrawString("Pitch", this.Font, new SolidBrush(Color.White), labelRectangle, labelFormat);
      }

      private void PaintYaw(Graphics graphics)
      {
         StringFormat textFormat = new StringFormat(StringFormat.GenericTypographic);
         textFormat.Alignment = StringAlignment.Center;
         textFormat.LineAlignment = StringAlignment.Center;

         float left = 0;
         float top = this.Height - this.YawHeight - 1;

         RectangleF displayRectangle = new RectangleF(left, top, this.YawWidth, this.YawHeight);

         graphics.FillRectangle(new SolidBrush(this.YawBackColor), displayRectangle);
         this.DrawFixed3D(graphics, left, top, this.YawWidth, this.YawHeight);
         string yawString = string.Format("{0}°", this.Yaw);
         graphics.DrawString(yawString, this.YawFont, new SolidBrush(this.YawForeColor), displayRectangle, textFormat);

         SizeF labelSize = graphics.MeasureString("Yaw", this.Font);
         float labelLeft = 0;
         float labelTop = this.Height - this.YawHeight - labelSize.Height - 1;
         RectangleF labelRectangle = new RectangleF(labelLeft, labelTop, labelSize.Width, labelSize.Height);

         StringFormat labelFormat = new StringFormat(StringFormat.GenericTypographic);
         labelFormat.Alignment = StringAlignment.Center;
         labelFormat.LineAlignment = StringAlignment.Center;

         graphics.DrawString("Yaw", this.Font, new SolidBrush(Color.White), labelRectangle, labelFormat);
      }

      private void PaintArrow(Graphics graphics, float centerX, float centerY, float circleDiameter)
      {
         Matrix matrix = new Matrix();
         matrix.RotateAt(this.Roll, new PointF(centerX, centerY));
         graphics.Transform = matrix;

         float arrowWeight = this.IndicatorWeight;

         float circleRadius = circleDiameter / 2;
         float arrowWidth = (0.165f * circleDiameter);

         PointF leftTail = new PointF(centerX - (arrowWidth / 2), centerY - circleRadius + (arrowWidth / 2) + (arrowWeight / 2) + 1);
         PointF head = new PointF(centerX, centerY - circleRadius + (arrowWeight / 2) + 1);
         PointF rightTail = new PointF(centerX + (arrowWidth / 2), centerY - circleRadius + (arrowWidth / 2) + (arrowWeight / 2) + 1);
         PointF center = new PointF(centerX, centerY);

         PointF[] indicatorPoints = new PointF[] { leftTail, head, rightTail };

         float pinX = centerX - (this.PinWeight / 2);
         float pinY = centerY - (this.PinWeight / 2);

         graphics.DrawLines(new Pen(this.IndicatorColor, arrowWeight), indicatorPoints);
         graphics.DrawLine(new Pen(this.IndicatorColor, arrowWeight), head, center);
         graphics.FillEllipse(new SolidBrush(this.IndicatorColor), pinX, pinY, this.PinWeight, this.PinWeight);
      }

      private void PaintRectangle(Graphics graphics, float centerX, float centerY, float circleDiameter)
      {
         Matrix matrix = new Matrix();
         matrix.RotateAt(this.Roll, new PointF(centerX, centerY));
         graphics.Transform = matrix;

         float rectangleWeight = this.IndicatorWeight;

         float circleRadius = circleDiameter / 2;
         float rectangleWidth = (int)(0.165 * circleDiameter);

         float indicatorX = centerX - (rectangleWidth / 2);
         float indicatorY = centerY - circleRadius + (rectangleWidth / 2);
         float indicatorWidth = (0.165f * circleDiameter);
         float indicatorHeight = indicatorWidth / 2;

         float pinX = centerX - (this.PinWeight / 2);
         float pinY = centerY - (this.PinWeight / 2);

         PointF head = new PointF(centerX, centerY - circleRadius + rectangleWidth);
         PointF center = new PointF(centerX, centerY);

         PointF tipTop = new PointF(centerX, centerY - circleRadius);
         PointF tipBottom = new PointF(centerX, centerY - circleRadius + (rectangleWidth / 2));

         graphics.DrawRectangle(new Pen(this.IndicatorColor, rectangleWeight), indicatorX, indicatorY, indicatorWidth, indicatorHeight);
         graphics.DrawLine(new Pen(this.IndicatorColor, rectangleWeight), head, center);
         graphics.DrawLine(new Pen(this.IndicatorColor, rectangleWeight), tipTop, tipBottom);
         graphics.FillEllipse(new SolidBrush(this.IndicatorColor), pinX, pinY, this.PinWeight, this.PinWeight);
      }

      private void PaintPositionLimit(Graphics graphics, float centerX, float centerY, float circleDiameter)
      {
         Matrix matrix = new Matrix();
         matrix.RotateAt(this.PositionRoll, new PointF(centerX, centerY));
         graphics.Transform = matrix;

         float rightAngle = this.PositionCwLimit;
         float leftAngle = this.PositionCcwLimit;

         float circleRadius = circleDiameter / 2;
         float rightX = (float)(Math.Sin(this.ToRadians(rightAngle)) * circleRadius);
         float rightY = (float)(Math.Cos(this.ToRadians(rightAngle)) * circleRadius);
         float leftX = (float)(Math.Sin(this.ToRadians(leftAngle)) * circleRadius);
         float leftY = (float)(Math.Cos(this.ToRadians(leftAngle)) * circleRadius);

         PointF center = new PointF(centerX, centerY);
         PointF right = new PointF(centerX + rightX, centerY - rightY);
         PointF left = new PointF(centerX + leftX, centerY - leftY);

         graphics.DrawLine(new Pen(this.IndicatorColor, this.IndicatorWeight), center, right);
         graphics.DrawLine(new Pen(this.IndicatorColor, this.IndicatorWeight), center, left);
      }

      private void PaintPushIndicators(Graphics graphics, float centerX, float centerY, float circleDiameter)
      {
         float circleRadius = circleDiameter / 2;

         float indicatorDiameter = 30f;
         float indicatorRadius = indicatorDiameter / 2;
         float innerRadius = circleRadius - indicatorRadius;

         float leftX = (float)(Math.Sin(this.ToRadians(this.LeftPushIndicatorAngle)) * innerRadius);
         float leftY = (float)(Math.Cos(this.ToRadians(this.LeftPushIndicatorAngle)) * innerRadius);
         float rightX = (float)(Math.Sin(this.ToRadians(this.RightPushIndicatorAngle)) * innerRadius);
         float rightY = (float)(Math.Cos(this.ToRadians(this.RightPushIndicatorAngle)) * innerRadius);

         graphics.FillEllipse(new SolidBrush(this.LeftPushIndicatorColor), centerX - leftX - indicatorRadius, centerY - leftY - indicatorRadius, indicatorDiameter, indicatorDiameter);
         graphics.DrawEllipse(new Pen(this.PushIndicatorEdgeColor, this.PushIndicatorEdgeWeight), centerX - leftX - indicatorRadius, centerY - leftY - indicatorRadius, indicatorDiameter, indicatorDiameter);

         graphics.FillEllipse(new SolidBrush(this.RightPushIndicatorColor), centerX + rightX - indicatorRadius, centerY - rightY - indicatorRadius, indicatorDiameter, indicatorDiameter);
         graphics.DrawEllipse(new Pen(this.PushIndicatorEdgeColor, this.PushIndicatorEdgeWeight), centerX + rightX - indicatorRadius, centerY - rightY - indicatorRadius, indicatorDiameter, indicatorDiameter);
      }
      
      #endregion

      #region Events

      protected override void OnPaint(PaintEventArgs e)
      {
         e.Graphics.FillRectangle(new SolidBrush(this.BackColor), this.ClientRectangle);

         float labelSpace;
         float circleDiameter;

         if (this.ClientRectangle.Width < this.ClientRectangle.Height)
         {
            SizeF numberSize = e.Graphics.MeasureString("6", this.Font);
            labelSpace = (int)(this.GaugeOuterNumberSpace + numberSize.Width + this.GaugeInnerNumberSpace);
            circleDiameter = (int)(this.ClientRectangle.Width - (2 * labelSpace));
         }
         else
         {
            SizeF numberSize = e.Graphics.MeasureString("9", this.Font);
            labelSpace = (int)(this.GaugeOuterNumberSpace + numberSize.Height + this.GaugeInnerNumberSpace);
            circleDiameter = (int)(this.ClientRectangle.Height - (2 * labelSpace));
         }

         float centerX = (this.ClientRectangle.Width - 1) / 2;
         float centerY = ((this.ClientRectangle.Height - 1) / 2) - 1;

         this.PaintGauge(e.Graphics, centerX, centerY, circleDiameter);
         this.PaintHours(e.Graphics, centerX, centerY, circleDiameter);

         if (false != this.RollVisible)
         {
            this.PaintRoll(e.Graphics);
         }

         if (false != this.PitchVisible)
         {
            this.PaintPitch(e.Graphics);
         }

         if (false != this.YawVisible)
         {
            this.PaintYaw(e.Graphics);
         }

         if (GaugeIndicators.arrow == this.GaugeIndicator)
         {
            this.PaintArrow(e.Graphics, centerX, centerY, circleDiameter);
         }
         else if (GaugeIndicators.rectangle == this.GaugeIndicator)
         {
            this.PaintRectangle(e.Graphics, centerX, centerY, circleDiameter);
         }

         if (false != this.ShowPosition)
         {
            this.PaintPositionLimit(e.Graphics, centerX, centerY, circleDiameter);
         }

         if (false != this.ShowPushIndicators)
         {
            this.PaintPushIndicators(e.Graphics, centerX, centerY, circleDiameter);
         }
      }

      #endregion

      #region Constructor

      public RollDisplay()
         : base()
      {
         this.GaugeBackColor = Color.FromArgb(255, 255, 192);
         this.GaugeEdgeColor = Color.Black;
         this.GaugeOuterNumberSpace = 3;
         this.GaugeInnerNumberSpace = 3;
         this.GaugeEdgeWeight = 1;
         this.GaugeIndicator = GaugeIndicators.arrow;

         this.RollVisible = false;
         this.RollWidth = 55;
         this.RollHeight = 30;
         this.RollFont = new Font(this.Font.Name, 8.25f);
         this.RollBackColor = Color.Black;
         this.RollForeColor = Color.White;

         this.PitchVisible = false;
         this.PitchWidth = 55;
         this.PitchHeight = 30;
         this.PitchFont = new Font(this.Font.Name, 8.25f);
         this.PitchBackColor = Color.Black;
         this.PitchForeColor = Color.White;

         this.YawVisible = false;
         this.YawWidth = 55;
         this.YawHeight = 30;
         this.YawFont = new Font(this.Font.Name, 8.25f);
         this.YawBackColor = Color.Black;
         this.YawForeColor = Color.White;

         this.IndicatorColor = Color.Firebrick;
         this.IndicatorWeight = 3;
         this.PinWeight = 7;

         this.ShowPosition = false;
         this.PositionRoll = 0f;
         this.PositionCwLimit = 45f;
         this.PositionCcwLimit = -45f;

         this.ShowPushIndicators = false;
         this.PushIndicatorEdgeWeight = 1;
         this.PushIndicatorEdgeColor = Color.Black;
         this.LeftPushIndicatorColor = Color.Gray;
         this.RightPushIndicatorColor = Color.Gray;
         this.LeftPushIndicatorAngle = 60f;
         this.RightPushIndicatorAngle = 60f;

         this.DoubleBuffered = true;
      }

      #endregion
}
}
