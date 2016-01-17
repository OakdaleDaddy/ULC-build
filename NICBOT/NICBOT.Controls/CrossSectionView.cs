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

namespace NICBOT.Controls
{
   public partial class CrossSectionView : UserControl
   {
      // Border color of the textbox 
      private Color borderColor = Color.Black;

      private Color gaugeBackColor = Color.White;
      private Color gaugeEdgeColor = Color.Black;
      private Color robotBodyColor = Color.DarkGray;
      private Color robotArmColor = Color.Gray;
      private Color robotWheelColor = Color.Teal;
      private Color robotTopWheelIndicatorColor = Color.White;
      private int gaugeEdgeWeight = 1;
      private int gaugeOuterNumberSpace = 0;
      private int gaugeInnerNumberSpace = 0;
      private int robotRoll = 0;
      private int robotPitch = 0;
      private bool axial;
      private bool pitchVisible;
      private int pitchWidth = 80;
      private int pitchHeight = 20;
      private Font pitchFont = System.Drawing.SystemFonts.DefaultFont;
      private Color pitchBackColor = Color.Black;
      private Color pitchForeColor = Color.White;
      private bool sensorVisible;
      private int sensorAngle;

      private BorderStyle borderStyle;

      public CrossSectionView()
      {
         base.BorderStyle = BorderStyle.None;
         this.BorderStyle = BorderStyle.None;
         this.InitializeComponent();
         this.DoubleBuffered = true;
      }

      // The border color property
      public Color BorderColor
      {
         get { return borderColor; }
         set { borderColor = value; Invalidate(); }
      }

      public Color GaugeBackColor
      {
         get { return this.gaugeBackColor; }
         set { this.gaugeBackColor = value; Invalidate(); }
      }

      public Color GaugeEdgekColor
      {
         get { return this.gaugeEdgeColor; }
         set { this.gaugeEdgeColor = value; Invalidate(); }
      }

      public int GaugeOuterNumberSpace
      {
         get { return this.gaugeOuterNumberSpace; }
         set { this.gaugeOuterNumberSpace = value; Invalidate(); }
      }

      public int GaugeInnerNumberSpace
      {
         get { return this.gaugeInnerNumberSpace; }
         set { this.gaugeInnerNumberSpace = value; Invalidate(); }
      }

      public int RobotRoll
      {
         get { return this.robotRoll; }

         set
         {
            this.robotRoll = value;
            this.Invalidate();
         }
      }

      public int RobotPitch
      {
         get { return this.robotPitch; }

         set
         {
            this.robotPitch = value;
            this.Invalidate();
         }
      }

      public bool Axial
      {
         get { return this.axial; }

         set
         {
            this.axial = value;
            this.Invalidate();
         }
      }
      
      public Color RobotBodyColor
      {
         get { return this.robotBodyColor; }
         set { this.robotBodyColor = value; Invalidate(); }
      }

      public Color RobotArmColor
      {
         get { return this.robotArmColor; }
         set { this.robotArmColor = value; Invalidate(); }
      }

      public Color RobotWheelColor
      {
         get { return this.robotWheelColor; }
         set { this.robotWheelColor = value; Invalidate(); }
      }

      public Color RobotTopWheelIndicatorColor
      {
         get { return this.robotTopWheelIndicatorColor; }
         set { this.robotTopWheelIndicatorColor = value; Invalidate(); }
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

      public bool SensorVisible
      {
         get { return this.sensorVisible; }
         set { this.sensorVisible = value; Invalidate(); }
      }

      public int SensorAngle
      {
         get { return this.sensorAngle; }
         set { this.sensorAngle = value; Invalidate(); }
      }

      /// <summary>
      /// limit to 1 to 15
      /// </summary>
      public int GaugeEdgeWeight
      {
         get { return this.gaugeEdgeWeight; }

         set 
         {
            if (value > 15)
            {
               value = 15;
            }
            else if (value < 1)
            {
               value = 1;
            }
            this.gaugeEdgeWeight = value; Invalidate(); 
         }
      }

      public new BorderStyle BorderStyle 
      {
         get { return this.borderStyle; }
         set 
         {
            this.borderStyle = value;
            Invalidate();
         }
      }

      private void DrawFixed3D(Graphics graphics, int left, int top, int width, int height)
      {
         width--;
         height--;

         graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDark), 1), left, top, left + width - 1, top);
         graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlDark), 1), left, top + 1, left, top + height - 1);

         graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlLightLight), 1), left, top + height, left + width, top + height);
         graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.ControlLightLight), 1), left + width, top, left + width, top + height);
      }

      private void PaintGauge(Graphics graphics, int centerX, int centerY, int circleDiameter)
      {
         int circleRadius = circleDiameter / 2;
         float x0 = (float)(centerX - circleRadius);
         float y0 = (float)(centerY - circleRadius);

         graphics.FillEllipse(new SolidBrush(gaugeBackColor), x0, y0, circleDiameter, circleDiameter);
         graphics.DrawEllipse(new Pen(this.gaugeEdgeColor, this.gaugeEdgeWeight), x0, y0, circleDiameter, circleDiameter);
      }

      private void PaintHours(Graphics graphics, int centerX, int centerY, int circleDiameter)
      {
         int circleRadius = circleDiameter / 2;
         int labelRadius = circleRadius + this.GaugeInnerNumberSpace;
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

      private void PaintDebug(Graphics graphics, string formatString, params object[] args)
      {
         string debugString = string.Format(formatString, args);
         StringFormat format = new StringFormat(StringFormat.GenericTypographic);
         format.Alignment = StringAlignment.Near;
         format.LineAlignment = StringAlignment.Near;
         graphics.DrawString(debugString, this.Font, new SolidBrush(Color.White), 0, 0, format);
      }

      private void PaintTopWheelIndicator(Graphics graphics, int upperX, int upperY, int diameter, int offset)
      {
         Point a = new Point(upperX + offset, upperY + (diameter / 2)); 
         Point b = new Point(upperX + (diameter / 2), upperY + offset); 
         Point c = new Point(upperX + diameter - offset, upperY + (diameter / 2));
         Point[] indicatorPoints = new Point[] { a, b, c };

         graphics.DrawLines(new Pen(this.RobotTopWheelIndicatorColor, 3), indicatorPoints); 
      }

      private void PaintPitch(Graphics graphics)
      {
         StringFormat textFormat = new StringFormat(StringFormat.GenericTypographic);
         textFormat.Alignment = StringAlignment.Center;
         textFormat.LineAlignment = StringAlignment.Center;

         int left = this.Width - this.PitchWidth - 1;
         int top = this.Height - this.PitchHeight - 1;

         Rectangle displayRectangle = new Rectangle(left, top, this.PitchWidth, this.PitchHeight);

         graphics.FillRectangle(new SolidBrush(this.PitchBackColor), displayRectangle);
         this.DrawFixed3D(graphics, left, top, this.PitchWidth, this.PitchHeight);
         string pitchString = string.Format("{0}°", this.RobotPitch);
         graphics.DrawString(pitchString, this.PitchFont, new SolidBrush(this.PitchForeColor), displayRectangle, textFormat);
      }

      private void PaintRobot(Graphics graphics, int centerX, int centerY, int circleDiameter)
      {
         int circleRadius = circleDiameter / 2;
         int robotWidth = (int)(0.5 * circleDiameter);
         int robotHeight = (int)(0.33 * circleDiameter);
         int robotWheelDiameter = (int)(0.5 * robotHeight);
         int robotArmHeight = (int)((circleDiameter - (2 * robotHeight) + 2)/2);
         int robotArmWidth = (int)(0.5 * robotWheelDiameter);
         StringFormat format = new StringFormat(StringFormat.GenericTypographic);

         Matrix matrix = new Matrix();
         matrix.RotateAt(-this.RobotRoll, new Point(centerX, centerY));
         graphics.Transform = matrix;

         int rx0 = centerX - (robotWidth / 2) - 1;
         int ry0 = (int)(centerY - (robotHeight / 2) - 1);
         graphics.FillRectangle(new SolidBrush(this.RobotBodyColor), rx0, ry0, robotWidth, robotHeight);
         graphics.DrawRectangle(new Pen(Color.Black), rx0, ry0, robotWidth, robotHeight);

         format.Alignment = StringAlignment.Center;
         format.LineAlignment = StringAlignment.Near;
         graphics.DrawString("top", this.Font, new SolidBrush(Color.Black), centerX, ry0 + 3, format);

         int upperArmX = (int)(centerX - (0.5 * robotArmWidth) - 1);
         int upperArmY = centerY - (int)(robotHeight * 0.5) - robotArmHeight - 1;
         graphics.FillRectangle(new SolidBrush(this.RobotArmColor), upperArmX, upperArmY, robotArmWidth, robotArmHeight);
         graphics.DrawRectangle(new Pen(Color.Black), upperArmX, upperArmY, robotArmWidth, robotArmHeight);

         int lowerArmX = (int)(centerX - (0.5 * robotArmWidth) - 1);
         int lowerArmY = centerY + (int)(robotHeight * 0.5) - 1;
         graphics.FillRectangle(new SolidBrush(this.robotArmColor), lowerArmX, lowerArmY, robotArmWidth, robotArmHeight);
         graphics.DrawRectangle(new Pen(Color.Black), lowerArmX, lowerArmY, robotArmWidth, robotArmHeight);

         if (false != this.Axial)
         {
            int upperWheelX = (int)(centerX - (robotWheelDiameter / 2) - 1);
            int upperWheelY = (int)(centerY - circleRadius + 1); // adjusted to fit
            graphics.FillRectangle(new SolidBrush(this.RobotWheelColor), upperWheelX, upperWheelY, robotWheelDiameter, robotWheelDiameter);
            graphics.DrawRectangle(new Pen(Color.Black), upperWheelX, upperWheelY, robotWheelDiameter, robotWheelDiameter);

            int lowerWheelX = (int)(centerX - (robotWheelDiameter / 2) - 1);
            int lowerWheelY = (int)(centerY + circleRadius - robotWheelDiameter - 1);
            graphics.FillRectangle(new SolidBrush(this.RobotWheelColor), lowerWheelX, lowerWheelY, robotWheelDiameter, robotWheelDiameter);
            graphics.DrawRectangle(new Pen(Color.Black), lowerWheelX, lowerWheelY, robotWheelDiameter, robotWheelDiameter);

            this.PaintTopWheelIndicator(graphics, upperWheelX, upperWheelY, robotWheelDiameter, 2);
         }
         else
         {
            int upperWheelX = (int)(centerX - (robotWheelDiameter / 2) - 1);
            int upperWheelY = (int)(centerY - circleRadius); // adjusted to fit
            graphics.FillEllipse(new SolidBrush(this.RobotWheelColor), upperWheelX, upperWheelY, robotWheelDiameter, robotWheelDiameter);
            graphics.DrawEllipse(new Pen(Color.Black), upperWheelX, upperWheelY, robotWheelDiameter, robotWheelDiameter);

            int lowerWheelX = (int)(centerX - (robotWheelDiameter / 2) - 1);
            int lowerWheelY = (int)(centerY + circleRadius - robotWheelDiameter - 1);
            graphics.FillEllipse(new SolidBrush(this.RobotWheelColor), lowerWheelX, lowerWheelY, robotWheelDiameter, robotWheelDiameter);
            graphics.DrawEllipse(new Pen(Color.Black), lowerWheelX, lowerWheelY, robotWheelDiameter, robotWheelDiameter);

            this.PaintTopWheelIndicator(graphics, upperWheelX, upperWheelY, robotWheelDiameter, 5);
         }
      }

      private void PaintSensor(Graphics graphics, int centerX, int centerY, int circleDiameter)
      {
         int circleRadius = circleDiameter / 2;
         int sensorWidth = (int)(0.42 * circleDiameter);
         int sensorHeight = (int)(0.27 * circleDiameter);
         StringFormat format = new StringFormat(StringFormat.GenericTypographic);

         Matrix sensorMatrix = new Matrix();
         sensorMatrix.RotateAt(-this.SensorAngle, new Point(centerX, centerY));
         graphics.Transform = sensorMatrix;

         int sensorXOffset = centerX - (sensorWidth / 2) - 1;
         int sensorYOffset = centerY - (sensorHeight / 2) - 1;
         SolidBrush sensorBrush = new SolidBrush(Color.FromArgb(64, Color.Orange));
         graphics.FillRectangle(sensorBrush, sensorXOffset, sensorYOffset, sensorWidth, sensorHeight);
         graphics.DrawRectangle(new Pen(Color.OrangeRed), sensorXOffset, sensorYOffset, sensorWidth, sensorHeight);

         format.Alignment = StringAlignment.Center;
         format.LineAlignment = StringAlignment.Far;
         int sensorTextY = centerY + (sensorHeight / 2);
         graphics.DrawString("sensor", this.Font, new SolidBrush(Color.Brown), centerX, sensorTextY, format);

         Matrix sensorPointerMatrix = new Matrix();
         sensorPointerMatrix.RotateAt(-this.SensorAngle, new Point(centerX, centerY));
         graphics.Transform = sensorPointerMatrix;

         int sensorLineSX = centerX;
         int sensorLineSY = centerY - (sensorHeight / 2);
         int sensorLineEX = centerX;
         int sensorLineEY = centerY - (circleDiameter / 2);

         Pen sensorLinePen = new Pen(Color.OrangeRed, 2);
         sensorLinePen.CustomEndCap = new AdjustableArrowCap(5, 5);
         graphics.DrawLine(sensorLinePen, sensorLineSX, sensorLineSY, sensorLineEX, sensorLineEY);

         int sensorPointerDiameter = 5;
         int sensorPointerX = centerX - (sensorPointerDiameter / 2) - 1;
         int sensorPointerY = centerY - circleRadius - sensorPointerDiameter;
         graphics.FillEllipse(new SolidBrush(Color.Red), sensorPointerX, sensorPointerY, sensorPointerDiameter, sensorPointerDiameter);
         graphics.DrawEllipse(new Pen(Color.Black), sensorPointerX, sensorPointerY, sensorPointerDiameter, sensorPointerDiameter);
      }

      protected override void OnPaint(PaintEventArgs e)
      {
         base.OnPaint(e);

         if (this.borderStyle == System.Windows.Forms.BorderStyle.FixedSingle)
         {
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, borderColor, ButtonBorderStyle.Solid);
         }
         
         int labelSpace;
         int circleDiameter;

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

         int centerX = (this.ClientRectangle.Width - 1) / 2;
         int centerY = ((this.ClientRectangle.Height - 1) / 2) - 1;

         this.PaintGauge(e.Graphics, centerX, centerY, circleDiameter);
         this.PaintHours(e.Graphics, centerX, centerY, circleDiameter);

         if (false != this.PitchVisible)
         {
            this.PaintPitch(e.Graphics);
         }

         this.PaintRobot(e.Graphics, centerX, centerY, circleDiameter);

         if (false != this.SensorVisible)
         {
            this.PaintSensor(e.Graphics, centerX, centerY, circleDiameter);
         }

      }
   }
}
