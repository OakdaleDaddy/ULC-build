namespace UlcRobotics.Ui.Controls
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics;
   using System.Drawing;
   using System.Windows.Forms;

   public class BoreSensorControl : Control
   {
      #region Fields

      private float maximumScale;
      private float graphXOffset;
      private float graphYOffset;

      private UInt16[] sensorReadings;
      private PointF[] sensorPoints;

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

      private void CalculateSensorPoints()
      {
         UInt16[] readings = this.SensorReadings;

         if (null != readings)
         {
            int arrayLength = readings.Length;

            float maximumScale = this.maximumScale;

            float centerX = (maximumScale / 2) + this.graphXOffset;
            float centerY = (maximumScale / 2) + this.graphYOffset;
            float pointAngle = 0f;
            float degreesPerStep = 360f / arrayLength;

            PointF[] points = new PointF[arrayLength];

            for (int i = 0; i < arrayLength; i++)
            {
               float reading = readings[i];
               float maximum = ((UInt16.MaxValue - reading) / UInt16.MaxValue) * (maximumScale / 2);

               float pointX = (float)(centerX + (maximum * Math.Sin(this.DegreeToRadian(pointAngle))));
               float pointY = (float)(centerY - (maximum * Math.Cos(this.DegreeToRadian(pointAngle))));
               points[i] = new PointF(pointX, pointY);

               pointAngle += degreesPerStep;
            }

            this.sensorPoints = points;
         }
         else
         {
            this.sensorPoints = null;
         }
      }

      #endregion

      #region Properties

      public UInt16[] SensorReadings
      {
         get
         {
            return (this.sensorReadings);
         }

         set
         {
            this.sensorReadings = value;
            this.CalculateScaling();
            this.CalculateSensorPoints();
            this.Invalidate();
         }
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

      private void PaintSensor(Graphics g)
      {
         if (null != this.sensorPoints)
         {
            float maximumScale = this.maximumScale;

            float centerX = (maximumScale / 2) + this.graphXOffset;
            float centerY = (maximumScale / 2) + this.graphYOffset;

            Pen linePen = new Pen(Color.White, 1f);
            PointF centerPoint = new PointF(centerX, centerY);

            for (int i = 0; i < this.sensorPoints.Length; i++)
            {
               g.DrawLine(linePen, centerPoint, this.sensorPoints[i]);
            }
         }
      }

      #endregion

      #region Events

      private void Control_Resize(object sender, EventArgs e)
      {
         this.CalculateScaling();
         this.CalculateSensorPoints();
         this.Invalidate();
      }

      protected override void OnPaint(PaintEventArgs e)
      {
         try
         {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 1), 0, 0, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);
            this.PaintSensor(e.Graphics);
            this.PaintPoints(e.Graphics, Color.Red, 1f, this.sensorPoints);
         }
         catch (Exception exception)
         {
            string message = exception.Message;
         }
      }

      #endregion

      #region Constructor

      public BoreSensorControl()
         : base()
      {
         this.Resize += this.Control_Resize;
         
         this.DoubleBuffered = true;
      }

      #endregion
   }
}