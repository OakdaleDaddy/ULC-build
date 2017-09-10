namespace UlcRobotics.Ui.Controls
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics;
   using System.Drawing;
   using System.Windows.Forms;

   public class BoreDataControl : Control
   {
      #region Fields

      private Point crossLocation;
      private Size crossSize;

      private bool crossPressed;
      private Size crossPressOffset;

      private UInt16[] boundaryReadings;
      private UInt16[] sensorReadings;

      private PointF[] boundaryPoints;
      private double[] sensorToBoundaryAngles;
      
      #endregion

      #region Helper Functions

      private double DegreeToRadian(double angle)
      {
         return Math.PI * angle / 180.0;
      }

      private void Trace(string formatString, params object[] args)
      {
         string traceLine = string.Format(formatString, args);
         System.Diagnostics.Trace.WriteLine(traceLine);
      }

      private void PaintDebug(Graphics g, int line, string formatString, params object[] args)
      {
         float y = this.Font.Height * line;

         string debugLine = string.Format(formatString, args);
         g.DrawString(debugLine, this.Font, new SolidBrush(this.ForeColor), 0, y, StringFormat.GenericTypographic);
      }

      private bool PointWithin(Point point, Point location, Size size, ref Size pressOffset)
      {
         bool result = false;

         int offsetX = point.X - location.X;
         int offsetY = point.Y - location.Y;

         if ((offsetX >= 0) &&
             (offsetX <= size.Width) &&
             (offsetY >= 0) &&
             (offsetY <= size.Height))
         {
            result = true;

            pressOffset.Width = offsetX;
            pressOffset.Height = offsetY;
         }

         this.Trace("pressed {0}", result);

         return (result);
      }

      private int GetMaximumScale()
      {
         int result = this.ClientRectangle.Width;

         if (this.ClientRectangle.Width > this.ClientRectangle.Height)
         {
            result = this.ClientRectangle.Height;
         }

         return (result);
      }

      private bool AngleInRange(double t, double a1, double a2)
      {
         bool result = false;

         if (a1 > a2)
         {
            if ((a1 > t) && (t >= a2))
            {
               result = true;
            }
         }
         else
         {
            if (a1 > t)
            {
               result = true;
            }
            else if (t >= a2)
            {
               result = true;
            }
         }

         return (result);
      }

      private bool GetPointIndexPair(double t, ref int p1Index, ref int p2Index)
      {
         bool result = false;

         if (null != this.sensorToBoundaryAngles)
         {
            double[] angles = this.sensorToBoundaryAngles;

            for (int i = 0; i < angles.Length; i++)
            {
               int indexA = (i > 0) ? (i - 1) : (angles.Length - 1);
               int indexB = i;

               if (this.AngleInRange(t, angles[indexA], angles[indexB]) != false)
               {
                  result = true;
                  p1Index = indexA;
                  p2Index = indexB;
                  break;
               }
            }
         }

         return (result);
      }

      private void SetBoundPointAngles()
      {
         if (null != this.boundaryPoints)
         {
            double crossCenterX = this.CrossLocation.X + (this.CrossSize.Width / 2);
            double crossCenterY = this.CrossLocation.Y + (this.CrossSize.Height / 2);

            PointF[] boundaryPoints = this.boundaryPoints;
            this.sensorToBoundaryAngles = new double[this.boundaryPoints.Length];

            for (int i = 0; i < boundaryPoints.Length; i++)
            {
               double deltaX = boundaryPoints[i].X - crossCenterX;
               double deltaY = crossCenterY - boundaryPoints[i].Y;
               double angle = Math.Atan2(deltaY, deltaX) * 180.0 / Math.PI;

               if (angle < 0)
               {
                  angle = 360 + angle ;
               }

               this.sensorToBoundaryAngles[i] = angle;
            }
         }
         else
         {
            this.sensorToBoundaryAngles = null;
         }
      }

      #endregion

      #region Properties

      public Point CrossLocation
      {
         get { return this.crossLocation; }
         set { this.crossLocation = value; this.Invalidate(); }
      }

      public Size CrossSize
      {
         get { return this.crossSize; }
         set { this.crossSize = value; this.Invalidate(); }
      }

      public UInt16[] BoundaryReadings
      {
         get { return this.boundaryReadings; }

         set
         {
            this.boundaryReadings = value;

            if (null != value)
            {
               int arrayLength = value.Length;

               float maximumScale = this.GetMaximumScale();
               float maximumReading = (float)(UInt16.MaxValue / 2);

               float xOffset = 0;
               float yOffset = 0;

               if (this.ClientRectangle.Width > this.ClientRectangle.Height)
               {
                  xOffset = (float)((this.ClientRectangle.Width - maximumScale) / 2);
                  yOffset = 0;
               }
               else
               {
                  xOffset = 0;
                  yOffset = (float)((this.ClientRectangle.Height - maximumScale) / 2);
               }
               
               float centerX = (maximumScale / 2) + xOffset;
               float centerY = (maximumScale / 2) + yOffset;
               float pointAngle = 0f;
               float degreesPerStep = 360 / arrayLength;

               this.boundaryPoints = new PointF[arrayLength];

               for (int i = 0; i < value.Length; i++)
               {
                  float maximum = ((maximumReading - value[i]) / maximumReading) * (maximumScale / 2);

                  float pointX = (float)(centerX + (maximum * Math.Sin(this.DegreeToRadian(pointAngle))));
                  float pointY = (float)(centerY - (maximum * Math.Cos(this.DegreeToRadian(pointAngle))));
                  this.boundaryPoints[i] = new PointF(pointX, pointY);

                  pointAngle += degreesPerStep;
               }
            }
            else
            {
               this.boundaryPoints = null;
            }

            this.Invalidate();
         }
      }

      #endregion

      #region Paint Functions

      private void PaintPipe(Graphics g)
      {
         g.DrawEllipse(new Pen(Color.FromKnownColor(KnownColor.ControlDarkDark), 3f), 1.5f, 1.5f, this.ClientRectangle.Width - 3f, this.ClientRectangle.Height - 3f);

         if (null != this.boundaryPoints)
         {
            UInt16[] points = this.BoundaryReadings;
            Pen pipePen = new Pen(Color.White, 1);

            Pen testPen = new Pen(Color.Red, 1);
            PointF previousPoint = new PointF(0f, 0f);

            for (int i = 0; i < points.Length; i++)
            {
               if (i > 0)
               {
                  g.DrawLine(testPen, previousPoint, this.boundaryPoints[i]);
               }

               previousPoint = this.boundaryPoints[i];
            }

            g.DrawLine(testPen, previousPoint, this.boundaryPoints[0]);
         }
      }

      private void PaintCenterMark(Graphics g)
      {
         float crossCenterX = this.CrossLocation.X + (this.CrossSize.Width / 2);
         float crossCenterY = this.CrossLocation.Y + (this.CrossSize.Height / 2);

         Pen linePen = new Pen(Color.White, 1f);

         if (null != this.boundaryPoints)
         {
            this.SetBoundPointAngles();

            double pointAngle = 90;
            double angleDelta = 360 / this.boundaryPoints.Length;

            for (int i = 0; i < this.boundaryPoints.Length; i++)
            {
               int colorStrength = 255 - (i * 4);
               linePen = new Pen(Color.FromArgb(colorStrength, colorStrength, colorStrength), 1f);

               int p1Index = -1;
               int p2Index = -1;

               if (this.GetPointIndexPair(pointAngle, ref p1Index, ref p2Index) != false)
               {
                  float p1X = this.boundaryPoints[p1Index].X;
                  float p1Y = -this.boundaryPoints[p1Index].Y;
                  float p2X = this.boundaryPoints[p2Index].X;
                  float p2Y = -this.boundaryPoints[p2Index].Y;
                  float p3X = crossCenterX;
                  float p3Y = -crossCenterY;
                  float p4X = p3X;
                  float p4Y = p3Y;

                  double f1DeltaY = p2Y - p1Y;
                  double f1DeltaX = p2X - p1X;
                  double a = f1DeltaY / f1DeltaX;

                  if ((90 == pointAngle) ||
                      (270 == pointAngle))
                  {
                     p4X = p3X;
                     p4Y = (float)(p1Y + (a * (p3X - p1X)));
                     g.DrawLine(linePen, p3X, -p3Y, p4X, -p4Y);
                  }
                  else
                  {
                     double tanA = Math.Tan(pointAngle * Math.PI / 180);

                     if (a != tanA)
                     {
                        p4X = (float)((p3Y - (tanA * p3X) - p1Y + (a * p1X)) / (a - tanA));
                        p4Y = (float)(p1Y + (a * (p4X - p1X)));
                        g.DrawLine(linePen, p3X, -p3Y, p4X, -p4Y);
                     }
                     else
                     {
                        this.PaintDebug(g, 25, "ERROR2");
                     }
                  }

                  
                  if (0 == i)
                  {
                     double xDelta = p4X - p3X;
                     double yDelta = p4Y - p3Y;
                     double gD = Math.Sqrt((xDelta * xDelta) + (yDelta * yDelta));
                     double gM = this.GetMaximumScale() / 2;
                     double rM = UInt16.MaxValue / 2;
                     double sM = UInt16.MaxValue;

                     double s = (sM * gD) / (2 * gM);
                     double r = rM - s;

                     if (r < 0)
                     {
                        r = rM - r;
                     }

                     this.PaintDebug(g, 0, "gM {0:0}", gM);
                     this.PaintDebug(g, 1, "gD {0:0}", gD);
                     this.PaintDebug(g, 2, " s {0:0}", s);
                     this.PaintDebug(g, 3, " r {0:0}", r);

                  }




                  //float maximumX = ((maximumReading - value[i]) / maximumReading) * ((this.ClientRectangle.Width - 1) / 2);

               }
               else
               {
                  this.PaintDebug(g, 25, "ERROR");
               }

               pointAngle -= angleDelta;

               if (pointAngle < 0)
               {
                  pointAngle += 360;
               }
            }
         }


         PointF croassA = new PointF(this.CrossLocation.X, crossCenterY);
         PointF croassB = new PointF(this.CrossLocation.X + this.CrossSize.Width, crossCenterY);
         PointF croassC = new PointF(crossCenterX, this.CrossLocation.Y);
         PointF croassD = new PointF(crossCenterX, this.CrossLocation.Y + this.CrossSize.Height);

         Pen crossPen = new Pen(Color.Black, 1);
         Color crossColor = (false != this.crossPressed) ? Color.Yellow : Color.White;
         g.FillEllipse(new SolidBrush(crossColor), this.CrossLocation.X, this.CrossLocation.Y, this.CrossSize.Width, this.CrossSize.Height);
         g.DrawLine(crossPen, croassA, croassB);
         g.DrawLine(crossPen, croassC, croassD);
         g.DrawEllipse(crossPen, this.CrossLocation.X, this.CrossLocation.Y, this.CrossSize.Width, this.CrossSize.Height);
      }

      #endregion

      #region Events

      private void BoreDataControl_MouseDown(object sender, MouseEventArgs e)
      {
         if (this.PointWithin(e.Location, this.CrossLocation, this.CrossSize, ref this.crossPressOffset) != false) 
         {
            this.crossPressed = true;
         }
      }

      private void BoreDataControl_MouseUp(object sender, MouseEventArgs e)
      {
         if (false != this.crossPressed)
         {
            this.crossPressed = false;
            this.Invalidate();
         }
      }

      private void BoreDataControl_MouseMove(object sender, MouseEventArgs e)
      {
         if (false != this.crossPressed)
         {
            int left = e.X - this.crossPressOffset.Width;
            int leftLimit = this.ClientRectangle.Width - this.CrossSize.Width - 1;

            int top = e.Y - this.crossPressOffset.Height;
            int topLimit = this.ClientRectangle.Height - this.CrossSize.Height -1;
            
            if (top < 0)
            {
               top = 0;
            }
            else if (top > topLimit)
            {
               top = topLimit;
            }

            if (left < 0)
            {
               left = 0;
            }
            else if (left > leftLimit)
            {
               left = leftLimit;
            }

            this.CrossLocation = new Point(left, top);

            this.Trace("cross location ({0}, {1})", left, top);
         }
      }

      protected override void OnPaint(PaintEventArgs e)
      {
         e.Graphics.DrawRectangle(new Pen(Color.Black, 1), 0, 0, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);

         this.PaintPipe(e.Graphics);
         this.PaintCenterMark(e.Graphics);
      }

      #endregion

      #region Constructor

      public BoreDataControl()
         : base()
      {
         this.crossLocation = new Point(1, 1);
         this.crossSize = new Size(16, 16);

         this.MouseDown += this.BoreDataControl_MouseDown;
         this.MouseUp += this.BoreDataControl_MouseUp;
         this.MouseMove += this.BoreDataControl_MouseMove;

         this.DoubleBuffered = true;
      }

      #endregion

   }
}
