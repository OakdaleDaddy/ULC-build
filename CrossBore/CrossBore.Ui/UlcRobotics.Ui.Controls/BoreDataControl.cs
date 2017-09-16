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

      private float maximumScale;
      private float graphXOffset;
      private float graphYOffset;

      private Point crossLocation;
      private Size crossSize;

      private bool crossPressed;
      private int boundaryPressedIndex;
      private Size pressOffset;

      private UInt16[] boundaryValues;
      private UInt16[] sensorReadings;

      private PointF[] boundaryPoints;

      private int sensorReadingCount;
      private double[] sensorToBoundaryAngles;
      private PointF[] sensorToBoundaryPoints;

      private bool showSensorMark;
      private bool showSensorReadingLines;
      private bool showSensorBoundary;
      private bool showBoundary;
      private bool showBoundaryLimit;
      
      #endregion

      #region Helper Functions

      private void CalculateScaling()
      {
         int result = this.ClientRectangle.Width;

         if (this.ClientRectangle.Width > this.ClientRectangle.Height)
         {
            this.maximumScale = this.ClientRectangle.Height;
            this.graphXOffset = ((float)(this.ClientRectangle.Width - this.ClientRectangle.Height)) / 2f;
            this.graphYOffset = 0;
         }
         else
         {
            this.maximumScale = this.ClientRectangle.Width;
            this.graphXOffset = 0;
            this.graphYOffset = ((float)(this.ClientRectangle.Height - this.ClientRectangle.Width)) / 2f;
         }
      }

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
         if (null != g)
         {
            float y = this.Font.Height * line;

            string debugLine = string.Format(formatString, args);
            g.DrawString(debugLine, this.Font, new SolidBrush(this.ForeColor), 0, y, StringFormat.GenericTypographic);
         }
      }

      private bool PointWithin(Point point, PointF location, Size size, ref Size pressOffset)
      {
         bool result = false;

         int offsetX = (int)(point.X - location.X);
         int offsetY = (int)(point.Y - location.Y);

         if ((offsetX >= 0) &&
             (offsetX <= size.Width) &&
             (offsetY >= 0) &&
             (offsetY <= size.Height))
         {
            result = true;

            pressOffset.Width = offsetX;
            pressOffset.Height = offsetY;
         }

         //this.Trace("pressed {0}", result);

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

      private void CalculateBoundaryPointAngles()
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

      private void CalculateBoundaryPoints()
      {
         UInt16[] values = this.BoundaryValues;

         if (null != values)
         {
            int arrayLength = values.Length;

            float maximumScale = this.maximumScale;
            float minimumReading = (float)(UInt16.MaxValue / 2);

            float centerX = (maximumScale / 2) + this.graphXOffset;
            float centerY = (maximumScale / 2) + this.graphYOffset;
            float pointAngle = 0f;
            float degreesPerStep = 360f / arrayLength;

            PointF[] points = new PointF[arrayLength];

            for (int i = 0; i < arrayLength; i++)
            {
               float reading = values[i];

               if (reading < minimumReading)
               {
                  reading = minimumReading;
               }

               float maximum = ((reading - minimumReading) / UInt16.MaxValue) * maximumScale;

               float pointX = (float)(centerX + (maximum * Math.Sin(this.DegreeToRadian(pointAngle))));
               float pointY = (float)(centerY - (maximum * Math.Cos(this.DegreeToRadian(pointAngle))));
               points[i] = new PointF(pointX, pointY);

               pointAngle += degreesPerStep;
            }

            this.boundaryPoints = points;
         }
         else
         {
            this.boundaryPoints = null;
         }
      }

      private void CalculateSensorData(Graphics g)
      {
         float crossCenterX = this.CrossLocation.X + (this.CrossSize.Width / 2);
         float crossCenterY = this.CrossLocation.Y + (this.CrossSize.Height / 2);

         if (null != this.boundaryPoints)
         {
            int sensorArraySize = this.SensorReadingCount;
            this.CalculateBoundaryPointAngles();

            UInt16[] centerReadings = new UInt16[sensorArraySize];
            PointF[] centerPoints = new PointF[sensorArraySize];

            double pointAngle = 90.0;
            double angleDelta = 360.0 / sensorArraySize;
            double gM = this.maximumScale / 2;

            float p3X = crossCenterX;
            float p3Y = -crossCenterY;
            float p4X = p3X;
            float p4Y = p3Y;

            for (int i = 0; i < sensorArraySize; i++)
            {
               int p1Index = -1;
               int p2Index = -1;

               if (this.GetPointIndexPair(pointAngle, ref p1Index, ref p2Index) != false)
               {
                  float p1X = this.boundaryPoints[p1Index].X;
                  float p1Y = -this.boundaryPoints[p1Index].Y;
                  float p2X = this.boundaryPoints[p2Index].X;
                  float p2Y = -this.boundaryPoints[p2Index].Y;

                  double f1DeltaY = p2Y - p1Y;
                  double f1DeltaX = p2X - p1X;
                  double a = f1DeltaY / f1DeltaX;

                  if ((90 == pointAngle) ||
                      (270 == pointAngle))
                  {
                     p4X = p3X;
                     p4Y = (float)(p1Y + (a * (p3X - p1X)));
                  }
                  else
                  {
                     double tanA = Math.Tan(pointAngle * Math.PI / 180);

                     if (a != tanA)
                     {
                        p4X = (float)((p3Y - (tanA * p3X) - p1Y + (a * p1X)) / (a - tanA));
                        p4Y = (float)(p1Y + (a * (p4X - p1X)));
                     }
                     else
                     {
                        this.PaintDebug(g, 25, "ERROR2");
                     }
                  }

                  double xDelta = p4X - p3X;
                  double yDelta = p4Y - p3Y;
                  double gD = Math.Sqrt((xDelta * xDelta) + (yDelta * yDelta));
                  double gRatio = (gD / gM);
                  double s = (UInt16.MaxValue) - ((UInt16.MaxValue / 2) * gRatio);

                  if (s < 0)
                  {
                     s = 0;
                  }
                  else if (s > UInt16.MaxValue)
                  {
                     s = UInt16.MaxValue;
                  }

                  centerReadings[i] = (UInt16)s;
               }
               else
               {
                  this.PaintDebug(g, 25, "ERROR");
               }

               centerPoints[i] = new PointF(p4X, -p4Y);

               pointAngle -= angleDelta;

               if (pointAngle < 0)
               {
                  pointAngle += 360;
               }
            }

            this.sensorReadings = centerReadings;
            this.sensorToBoundaryPoints = centerPoints;
         }
         else
         {
            this.sensorReadings = null;
            this.sensorToBoundaryPoints = null;
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

      public int SensorReadingCount
      {
         get { return this.sensorReadingCount; }
         set { this.sensorReadingCount = value; this.Invalidate(); }
      }

      public UInt16[] BoundaryValues
      {
         get { return this.boundaryValues; }

         set
         {
            this.boundaryValues = value;
            this.CalculateBoundaryPoints();

#if false
            if (null != value)
            {
               int arrayLength = value.Length;

               float maximumScale = this.maximumScale;
               float minimumReading = (float)(UInt16.MaxValue / 2);

               float centerX = (maximumScale / 2) + this.graphXOffset;
               float centerY = (maximumScale / 2) + this.graphYOffset;
               float pointAngle = 0f;
               float degreesPerStep = 360f / arrayLength;

               PointF[] points = new PointF[arrayLength];

               for (int i = 0; i < value.Length; i++)
               {
                  float reading = value[i];

                  if (reading < minimumReading)
                  {
                     reading = minimumReading;
                  }

                  float maximum = ((reading - minimumReading) / UInt16.MaxValue) * maximumScale;

                  float pointX = (float)(centerX + (maximum * Math.Sin(this.DegreeToRadian(pointAngle))));
                  float pointY = (float)(centerY - (maximum * Math.Cos(this.DegreeToRadian(pointAngle))));
                  points[i] = new PointF(pointX, pointY);

                  pointAngle += degreesPerStep;
               }

               this.boundaryPoints = points;
            }
            else
            {
               this.boundaryPoints = null;
            }
#endif

            this.Invalidate();
         }
      }

      public UInt16[] SensorReadings
      {
         get
         {
            return (this.sensorReadings);
         }
      }

      public bool ShowSensorMark
      {
         get { return this.showSensorMark; }
         set { this.showSensorMark = value; this.Invalidate(); }
      }

      public bool ShowSensorReadingLines
      {
         get { return this.showSensorReadingLines; }
         set { this.showSensorReadingLines = value; this.Invalidate(); }
      }

      public bool ShowSensorBoundary
      {
         get { return this.showSensorBoundary; }
         set { this.showSensorBoundary = value; this.Invalidate(); }
      }

      public bool ShowBoundary
      {
         get { return this.showBoundary; }
         set { this.showBoundary = value; this.Invalidate(); }
      }

      public bool ShowBoundaryLimit
      {
         get { return this.showBoundaryLimit; }
         set { this.showBoundaryLimit = value; this.Invalidate(); }
      }
      
      #endregion

      #region Paint Functions

      private void PaintPoints(Graphics g, Color color, float weight, PointF[] points)
      {
         if (null != points)
         {
            Pen pointsPen = new Pen(color, weight);
            PointF previousPoint = points[0];

            for (int i = 0; i < points.Length; i++)
            {
               pointsPen = new Pen(color, weight);

               if (i > 0)
               {
                  g.DrawLine(pointsPen, previousPoint, points[i]);
               }

               previousPoint = points[i];
            }

            g.DrawLine(pointsPen, previousPoint, points[0]);
         }
      }

      private void PaintBoundary(Graphics g)
      {
         if (false != this.ShowBoundaryLimit)
         {
            g.DrawEllipse(new Pen(Color.FromKnownColor(KnownColor.ControlDarkDark), 3f), this.graphXOffset + 1.5f, this.graphYOffset + 1.5f, this.maximumScale - 3f, this.maximumScale - 3f);
         }

         if (false != this.ShowBoundary)
         {
            this.PaintPoints(g, Color.Red, 1f, this.boundaryPoints);
         }
      }

      private void PaintSensor(Graphics g)
      {
         float crossCenterX = this.CrossLocation.X + (this.CrossSize.Width / 2);
         float crossCenterY = this.CrossLocation.Y + (this.CrossSize.Height / 2);

         if (false != this.ShowSensorReadingLines)
         {
            if (null != this.sensorToBoundaryPoints)
            {
               Pen linePen = new Pen(Color.White, 1f);
               PointF centerPoint = new PointF(crossCenterX, crossCenterY);

               for (int i = 0; i < this.sensorToBoundaryPoints.Length; i++)
               {
                  g.DrawLine(linePen, centerPoint, this.sensorToBoundaryPoints[i]);
               }
            }
         }

         if (false != this.ShowSensorBoundary)
         {
            this.PaintPoints(g, Color.Yellow, 1f, this.sensorToBoundaryPoints);
         }

         if (false != this.ShowSensorMark)
         {
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
      }

      #endregion

      #region Events

      private void BoreDataControl_Resize(object sender, EventArgs e)
      {
         this.CalculateScaling();
         this.CalculateBoundaryPoints();
         this.Invalidate();
      }
      
      private void BoreDataControl_MouseDown(object sender, MouseEventArgs e)
      {
         if (this.PointWithin(e.Location, this.CrossLocation, this.CrossSize, ref this.pressOffset) != false)
         {
            this.Trace("sensor pressed");
            this.crossPressed = true;
         }
         else
         {
            if (null != this.boundaryPoints)
            {
               for (int i = 0; i < this.boundaryPoints.Length; i++)
               {
                  if (this.PointWithin(e.Location, this.boundaryPoints[i], this.CrossSize, ref this.pressOffset) != false)
                  {
                     this.Trace("boundary pressed {0}", i);
                     this.boundaryPressedIndex = i;
                     break;
                  }
               }
            }
         }
      }

      private void BoreDataControl_MouseUp(object sender, MouseEventArgs e)
      {
         if (false != this.crossPressed)
         {
            this.Trace("sensor released");
            this.crossPressed = false;
            this.Invalidate();
         }
         else if (boundaryPressedIndex >= 0)
         {
            this.Trace("boundary released");
            this.boundaryPressedIndex = -1;
            this.Invalidate();
         }
      }

      private void BoreDataControl_MouseMove(object sender, MouseEventArgs e)
      {
         if (false != this.crossPressed)
         {
            int left = e.X - this.pressOffset.Width;
            int leftLimit = this.ClientRectangle.Width - this.CrossSize.Width - 1;

            int top = e.Y - this.pressOffset.Height;
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
         else if (this.boundaryPressedIndex >= 0)
         {
         }
      }

      protected override void OnPaint(PaintEventArgs e)
      {
         e.Graphics.DrawRectangle(new Pen(Color.Black, 1), 0, 0, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);

         try
         {
            this.CalculateSensorData(e.Graphics);

            this.PaintBoundary(e.Graphics);
            this.PaintSensor(e.Graphics);
         }
         catch (Exception exception)
         {
            string message = exception.Message;
         }
      }

      #endregion

      #region Constructor

      public BoreDataControl()
         : base()
      {
         this.SensorReadingCount = 15;

         this.ShowSensorMark = true;
         this.ShowSensorReadingLines = true;
         this.ShowSensorBoundary = true;
         this.ShowBoundary = true;
         this.ShowBoundaryLimit = true;

         this.Resize += this.BoreDataControl_Resize;
         this.MouseDown += this.BoreDataControl_MouseDown;
         this.MouseUp += this.BoreDataControl_MouseUp;
         this.MouseMove += this.BoreDataControl_MouseMove;

         this.DoubleBuffered = true;
      }

      #endregion

   }
}
