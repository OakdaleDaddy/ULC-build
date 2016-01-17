using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test2
{

   public class NicBotSideView : UserControl
   {
      #region Definitions

      public delegate void HoldTimeoutHandler2(object sender, HoldTimeoutEventArgs e);

      public enum RobotPosition
      {
         Off,
         Move,
         Drill,
      }

      #endregion

      #region Fields

      private int _verticalEdge;
      private int _horizontalEdge;
      private Color _insideColor;

      private Bitmap _offImage;
      private Color _offImageTransparentColor;
      private double _offImageScale;

      private Bitmap _moveImage;
      private Color _moveImageTransparentColor;
      private double _moveImageScale;

      private Bitmap _drillImage;
      private Color _drillImageTransparentColor;
      private double _drillImageScale;

      private RobotPosition _position;

      #endregion

      #region Properties

      public int VerticalEdge
      {
         set
         {
            this._verticalEdge = value;
            this.Invalidate();
         }

         get { return (this._verticalEdge); }
      }


      public int HorizontalEdge
      {
         set
         {
            this._horizontalEdge = value;
            this.Invalidate();
         }

         get { return (this._horizontalEdge); }
      }

      public Color InsideColor
      {
         set
         {
            this._insideColor = value;
            this.Invalidate();
         }

         get { return (this._insideColor); }
      }

      public Bitmap OffImage      
      { 
         set
         {
            if (null != value)
            {
               this._offImage = new Bitmap(value);
            }

            this.Invalidate();
         }

         get { return (this._offImage); }
      }

      public Color OffImageTransparentColor
      {
         set
         {
            this._offImageTransparentColor = value;
            this.Invalidate();
         }

         get { return (this._offImageTransparentColor); }
      }

      public double OffImageScale
      {
         set
         {
            this._offImageScale = value;
            this.Invalidate();
         }

         get { return (this._offImageScale); }
      }      

      public Bitmap MoveImage 
      {
         set
         {
            if (null != value)
            {
               this._moveImage = new Bitmap(value);
            }

            this.Invalidate();
         }

         get { return (this._moveImage); }
      }

      public Color MoveImageTransparentColor
      {
         set
         {
            this._moveImageTransparentColor = value;
            this.Invalidate();
         }

         get { return (this._moveImageTransparentColor); }
      }

      public double MoveImageScale
      {
         set
         {
            this._moveImageScale = value;
            this.Invalidate();
         }

         get { return (this._moveImageScale); }
      }

      public Bitmap DrillImage
      {
         set
         {
            if (null != value)
            {
               this._drillImage = new Bitmap(value);
            }

            this.Invalidate();
         }

         get { return (this._drillImage); }
      }

      public double DrillImageScale
      {
         set
         {
            this._drillImageScale = value;
            this.Invalidate();
         }

         get { return (this._drillImageScale); }
      }

      public Color DrillImageTransparentColor
      {
         set
         {
            this._drillImageTransparentColor = value;
            this.Invalidate();
         }

         get { return (this._drillImageTransparentColor); }
      }

      public RobotPosition Position
      {
         set
         {
            this._position = value;
            this.Invalidate();
         }

         get { return (this._position); }
      }

      private RobotPosition _fitHeightPosition;

      public RobotPosition FitHeightPosition
      {
         set
         {
            this._fitHeightPosition = value;
            this.Invalidate();
         }

         get { return (this._fitHeightPosition); }
      }

      #endregion

      #region Event Handlers

      protected override void OnPaint(PaintEventArgs e)
      {
         base.OnPaint(e);

         int insideWidth = this.ClientRectangle.Width - (2 * this.VerticalEdge);
         int insideHeight = this.ClientRectangle.Height - (2 * this.HorizontalEdge);
         e.Graphics.FillRectangle(new SolidBrush(this.InsideColor), this.VerticalEdge, this.HorizontalEdge, insideWidth, insideHeight);

         int refHeight = 0;
         int refWidth = 0;

         switch (this.FitHeightPosition)
         {
            default:
            case RobotPosition.Off:
            {
               if (null != this.OffImage)
               {
                  refHeight = this.OffImage.Height;
                  refWidth = this.OffImage.Width;
               }

               break;
            }
            case RobotPosition.Move:
            {
               if (null != this.MoveImage)
               {
                  refHeight = this.MoveImage.Height;
                  refWidth = this.MoveImage.Width;
               }

               break;
            }
            case RobotPosition.Drill:
            {
               if (null != this.DrillImage)
               {
                  refHeight = this.DrillImage.Height;
                  refWidth = this.DrillImage.Width;
               }

               break;
            }
         }
                  
         Bitmap bitMap = null;
         double scale = 1.0;

         switch (this.Position)
         {
            default:
            case RobotPosition.Off:
            {
               if (null != this.OffImage)
               {
                  bitMap = new Bitmap(this.OffImage);
                  bitMap.MakeTransparent(this.OffImageTransparentColor);
               }

               scale = this.OffImageScale;

               break;
            }
            case RobotPosition.Move:
            {
               if (null != this.MoveImage)
               {
                  bitMap = new Bitmap(this.MoveImage);
                  bitMap.MakeTransparent(this.MoveImageTransparentColor);
               }

               scale = this.MoveImageScale;

               break;
            }
            case RobotPosition.Drill:
            {
               if (null != this.DrillImage)
               {
                  bitMap = new Bitmap(this.DrillImage);
                  bitMap.MakeTransparent(this.DrillImageTransparentColor);
               }

               scale = this.DrillImageScale;

               break;
            }
         }

         if ((0 != refHeight) && (0 != refWidth) && (null != bitMap))
         {
            double imageWidth = bitMap.Width * scale;
            double imageHeight = bitMap.Height * scale;

            double heightMultiplier = (double)insideHeight / refHeight;
            int neededHeight = (int)(heightMultiplier * imageHeight);
            int neededWidth = (int)(((double)neededHeight * imageWidth) / imageHeight);

            int offsetY = insideHeight - neededHeight;

            int imageX = (this.ClientRectangle.Width / 2) - (neededWidth / 2);
            int imageY = this.HorizontalEdge + offsetY;
            Rectangle imageRect = new Rectangle(imageX, imageY, neededWidth, neededHeight);
            e.Graphics.DrawImage(bitMap, imageRect);


#if false // working display code
            int refImageHeight = insideHeight;
            int refImageWidth = (int)(((double)refImageHeight * refWidth) / refHeight);

            int refImageX = (this.ClientRectangle.Width / 2) - (refImageWidth / 2);
            int refImageY = this.HorizontalEdge;// +(insideHeight - bitMap.Height);
            Rectangle imageRect = new Rectangle(refImageX, refImageY, refImageWidth, refImageHeight);
            e.Graphics.DrawImage(bitMap, imageRect);
#endif

#if false // debug output 
            e.Graphics.DrawString(refHeight.ToString(), this.Font, new SolidBrush(Color.Black), this.VerticalEdge, this.ClientRectangle.Height - this.HorizontalEdge - (2 * this.HorizontalEdge));
            e.Graphics.DrawString(refWidth.ToString(), this.Font, new SolidBrush(Color.Black), this.VerticalEdge, this.ClientRectangle.Height - this.HorizontalEdge - (1 * this.HorizontalEdge));
#endif
         
         }
      }

      #endregion

      #region Constructor

      public NicBotSideView()
         : base()
      {
         this.VerticalEdge = 1;
         this.HorizontalEdge = 1;
         this.InsideColor = Color.Yellow;

         this.OffImage = null;
         this.OffImageTransparentColor = Color.White;
         this.OffImageScale = 1.0;

         this.MoveImage = null;
         this.MoveImageTransparentColor = Color.White;
         this.MoveImageScale = 1.0;

         this.DrillImage = null;
         this.DrillImageTransparentColor = Color.White;
         this.DrillImageScale = 1.0;

         this.DoubleBuffered = true;
      }

      #endregion

   }
}
