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
   public class NicBotSideView : UserControl
   {
      #region Fields

      private Color _descriptionColor;
      private Font _descriptionFont;
      private int _verticalEdge;
      private int _horizontalEdge;
      private Color _insideColor;
      private BodyPositions _position;
      private BodyPositions _fitHeightPosition;

      private Bitmap _offImage;
      private Color _offImageTransparentColor;
      private double _offImageScale;
      private string _offText;
      private ContentAlignment _offTextAlignment;

      private Bitmap _closedImage;
      private Color _closedImageTransparentColor;
      private double _closedImageScale;
      private string _closedText;
      private ContentAlignment _closedTextAlignment;

      private Bitmap _openedImage;
      private Color _openedImageTransparentColor;
      private double _openedImageScale;
      private string _openedText;
      private ContentAlignment _openedTextAlignment;

      private Bitmap _frontLooseImage;
      private Color _frontLooseImageTransparentColor;
      private double _frontLooseImageScale;
      private string _frontLooseText;
      private ContentAlignment _frontLooseTextAlignment;

      private Bitmap _rearLooseImage;
      private Color _rearLooseImageTransparentColor;
      private double _rearLooseImageScale;
      private string _rearLooseText;
      private ContentAlignment _rearLooseTextAlignment;

      private Bitmap _drillImage;
      private Color _drillImageTransparentColor;
      private double _drillImageScale;
      private string _drillText;
      private ContentAlignment _drillTextAlignment;

      private Bitmap _manualImage;
      private Color _manualImageTransparentColor;
      private double _manualImageScale;
      private string _manualText;
      private ContentAlignment _manualTextAlignment;

      #endregion

      #region General Properties

      public Font DescriptionFont
      {
         set
         {
            this._descriptionFont = value;
            this.Invalidate();
         }

         get { return (this._descriptionFont); }
      }

      public Color DescriptionColor
      {
         set
         {
            this._descriptionColor = value;
            this.Invalidate();
         }

         get { return (this._descriptionColor); }
      }

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

      public BodyPositions Position
      {
         set
         {
            this._position = value;
            this.Invalidate();
         }

         get { return (this._position); }
      }

      public BodyPositions FitHeightPosition
      {
         set
         {
            this._fitHeightPosition = value;
            this.Invalidate();
         }

         get { return (this._fitHeightPosition); }
      }

      #endregion

      #region Off Properties

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

      public string OffText
      {
         set
         {
            this._offText = value;
            this.Invalidate();
         }

         get { return (this._offText); }
      }

      public ContentAlignment OffTextAlignment
      {
         set
         {
            this._offTextAlignment = value;
            this.Invalidate();
         }

         get { return (this._offTextAlignment); }
      }

      #endregion

      #region Closed Properties

      public Bitmap ClosedImage
      {
         set
         {
            if (null != value)
            {
               this._closedImage = new Bitmap(value);
            }

            this.Invalidate();
         }

         get { return (this._closedImage); }
      }

      public Color ClosedImageTransparentColor
      {
         set
         {
            this._closedImageTransparentColor = value;
            this.Invalidate();
         }

         get { return (this._closedImageTransparentColor); }
      }

      public double ClosedImageScale
      {
         set
         {
            this._closedImageScale = value;
            this.Invalidate();
         }

         get { return (this._closedImageScale); }
      }
      
      public string ClosedText
      {
         set
         {
            this._closedText = value;
            this.Invalidate();
         }

         get { return (this._closedText); }
      }

      public ContentAlignment ClosedTextAlignment
      {
         set
         {
            this._closedTextAlignment = value;
            this.Invalidate();
         }

         get { return (this._closedTextAlignment); }
      }

      #endregion

      #region Opened Properties

      public Bitmap OpenedImage
      {
         set
         {
            if (null != value)
            {
               this._openedImage = new Bitmap(value);
            }

            this.Invalidate();
         }

         get { return (this._openedImage); }
      }

      public Color OpenedImageTransparentColor
      {
         set
         {
            this._openedImageTransparentColor = value;
            this.Invalidate();
         }

         get { return (this._openedImageTransparentColor); }
      }

      public double OpenedImageScale
      {
         set
         {
            this._openedImageScale = value;
            this.Invalidate();
         }

         get { return (this._openedImageScale); }
      }

      public string OpenedText
      {
         set
         {
            this._openedText = value;
            this.Invalidate();
         }

         get { return (this._openedText); }
      }

      public ContentAlignment OpenedTextAlignment
      {
         set
         {
            this._openedTextAlignment = value;
            this.Invalidate();
         }

         get { return (this._openedTextAlignment); }      
      }

      #endregion

      #region Front Loose Properties

      public Bitmap FrontLooseImage
      {
         set
         {
            if (null != value)
            {
               this._frontLooseImage = new Bitmap(value);
            }

            this.Invalidate();
         }

         get { return (this._frontLooseImage); }
      }

      public Color FrontLooseImageTransparentColor
      {
         set
         {
            this._frontLooseImageTransparentColor = value;
            this.Invalidate();
         }

         get { return (this._frontLooseImageTransparentColor); }
      }

      public double FrontLooseImageScale
      {
         set
         {
            this._frontLooseImageScale = value;
            this.Invalidate();
         }

         get { return (this._frontLooseImageScale); }
      }

      public string FrontLooseText
      {
         set
         {
            this._frontLooseText = value;
            this.Invalidate();
         }

         get { return (this._frontLooseText); }
      }

      public ContentAlignment FrontLooseTextAlignment
      {
         set
         {
            this._frontLooseTextAlignment = value;
            this.Invalidate();
         }

         get { return (this._frontLooseTextAlignment); }
      }

      #endregion

      #region Rear Loose Properties

      public Bitmap RearLooseImage
      {
         set
         {
            if (null != value)
            {
               this._rearLooseImage = new Bitmap(value);
            }

            this.Invalidate();
         }

         get { return (this._rearLooseImage); }
      }

      public Color RearLooseImageTransparentColor
      {
         set
         {
            this._rearLooseImageTransparentColor = value;
            this.Invalidate();
         }

         get { return (this._rearLooseImageTransparentColor); }
      }

      public double RearLooseImageScale
      {
         set
         {
            this._rearLooseImageScale = value;
            this.Invalidate();
         }

         get { return (this._rearLooseImageScale); }
      }

      public string RearLooseText
      {
         set
         {
            this._rearLooseText = value;
            this.Invalidate();
         }

         get { return (this._rearLooseText); }
      }

      public ContentAlignment RearLooseTextAlignment
      {
         set
         {
            this._rearLooseTextAlignment = value;
            this.Invalidate();
         }

         get { return (this._rearLooseTextAlignment); }
      }

      #endregion

      #region Drill Properties

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

      public Color DrillImageTransparentColor
      {
         set
         {
            this._drillImageTransparentColor = value;
            this.Invalidate();
         }

         get { return (this._drillImageTransparentColor); }
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

      public string DrillText
      {
         set
         {
            this._drillText = value;
            this.Invalidate();
         }

         get { return (this._drillText); }
      }

      public ContentAlignment DrillTextAlignment
      {
         set
         {
            this._drillTextAlignment = value;
            this.Invalidate();
         }

         get { return (this._drillTextAlignment); }
      }

      #endregion

      #region Manual Properties

      public Bitmap ManualImage
      {
         set
         {
            if (null != value)
            {
               this._manualImage = new Bitmap(value);
            }

            this.Invalidate();
         }

         get { return (this._manualImage); }
      }

      public Color ManualImageTransparentColor
      {
         set
         {
            this._manualImageTransparentColor = value;
            this.Invalidate();
         }

         get { return (this._manualImageTransparentColor); }
      }

      public double ManualImageScale
      {
         set
         {
            this._manualImageScale = value;
            this.Invalidate();
         }

         get { return (this._manualImageScale); }
      }

      public string ManualText
      {
         set
         {
            this._manualText = value;
            this.Invalidate();
         }

         get { return (this._manualText); }
      }

      public ContentAlignment ManualTextAlignment
      {
         set
         {
            this._manualTextAlignment = value;
            this.Invalidate();
         }

         get { return (this._manualTextAlignment); }
      }

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

         return(result);
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
            case BodyPositions.off:
            {
               if (null != this.OffImage)
               {
                  refHeight = this.OffImage.Height;
                  refWidth = this.OffImage.Width;
               }

               break;
            }
            case BodyPositions.closed:
            {
               if (null != this.ClosedImage)
               {
                  refHeight = this.ClosedImage.Height;
                  refWidth = this.ClosedImage.Width;
               }

               break;
            }
            case BodyPositions.opened:
            {
               if (null != this.OpenedImage)
               {
                  refHeight = this.OpenedImage.Height;
                  refWidth = this.OpenedImage.Width;
               }

               break;
            }
            case BodyPositions.frontLoose:
            {
               if (null != this.FrontLooseImage)
               {
                  refHeight = this.FrontLooseImage.Height;
                  refWidth = this.FrontLooseImage.Width;
               }

               break;
            }
            case BodyPositions.rearLoose:
            {
               if (null != this.RearLooseImage)
               {
                  refHeight = this.RearLooseImage.Height;
                  refWidth = this.RearLooseImage.Width;
               }

               break;
            }
            case BodyPositions.drill:
            {
               if (null != this.DrillImage)
               {
                  refHeight = this.DrillImage.Height;
                  refWidth = this.DrillImage.Width;
               }

               break;
            }
            case BodyPositions.manual:
            {
               if (null != this.ManualImage)
               {
                  refHeight = this.ManualImage.Height;
                  refWidth = this.ManualImage.Width;
               }

               break;
            }
         }
                  
         Bitmap bitMap = null;
         double scale = 1.0;
         string text = "";
         ContentAlignment textAlignment = ContentAlignment.MiddleCenter;

         switch (this.Position)
         {
            default:
            case BodyPositions.off:
            {
               if (null != this.OffImage)
               {
                  bitMap = new Bitmap(this.OffImage);
                  bitMap.MakeTransparent(this.OffImageTransparentColor);
               }

               scale = this.OffImageScale;
               text = this.OffText;
               textAlignment = this.OffTextAlignment;

               break;
            }
            case BodyPositions.closed:
            {
               if (null != this.ClosedImage)
               {
                  bitMap = new Bitmap(this.ClosedImage);
                  bitMap.MakeTransparent(this.ClosedImageTransparentColor);
               }

               scale = this.ClosedImageScale;
               text = this.ClosedText;
               textAlignment = this.ClosedTextAlignment;

               break;
            }
            case BodyPositions.opened:
            {
               if (null != this.OpenedImage)
               {
                  bitMap = new Bitmap(this.OpenedImage);
                  bitMap.MakeTransparent(this.OpenedImageTransparentColor);
               }

               scale = this.OpenedImageScale;
               text = this.OpenedText;
               textAlignment = this.OpenedTextAlignment;

               break;
            }
            case BodyPositions.frontLoose:
            {
               if (null != this.FrontLooseImage)
               {
                  bitMap = new Bitmap(this.FrontLooseImage);
                  bitMap.MakeTransparent(this.FrontLooseImageTransparentColor);
               }

               scale = this.FrontLooseImageScale;
               text = this.FrontLooseText;
               textAlignment = this.FrontLooseTextAlignment;

               break;
            }
            case BodyPositions.rearLoose:
            {
               if (null != this.RearLooseImage)
               {
                  bitMap = new Bitmap(this.RearLooseImage);
                  bitMap.MakeTransparent(this.RearLooseImageTransparentColor);
               }

               scale = this.RearLooseImageScale;
               text = this.RearLooseText;
               textAlignment = this.RearLooseTextAlignment;

               break;
            }
            case BodyPositions.drill:
            {
               if (null != this.DrillImage)
               {
                  bitMap = new Bitmap(this.DrillImage);
                  bitMap.MakeTransparent(this.DrillImageTransparentColor);
               }

               scale = this.DrillImageScale;
               text = this.DrillText;
               textAlignment = this.DrillTextAlignment;

               break;
            }
            case BodyPositions.manual:
            {
               if (null != this.ManualImage)
               {
                  bitMap = new Bitmap(this.ManualImage);
                  bitMap.MakeTransparent(this.ManualImageTransparentColor);
               }

               scale = this.ManualImageScale;
               text = this.ManualText;
               textAlignment = this.ManualTextAlignment;

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

         if ("" != text)
         {
            Rectangle descriptionRect = new Rectangle(this.VerticalEdge, this.HorizontalEdge, insideWidth, insideHeight);
            StringFormat descripionFormat = this.GetStringFormat(textAlignment);
            e.Graphics.DrawString(text, this.DescriptionFont, new SolidBrush(this.DescriptionColor), descriptionRect, descripionFormat);
         }
      }

      #endregion

      #region Constructor

      public NicBotSideView()
         : base()
      {
         this.DescriptionFont = SystemFonts.DefaultFont;
         this.DescriptionColor = Color.Black;
         this.VerticalEdge = 1;
         this.HorizontalEdge = 1;
         this.InsideColor = Color.Yellow;

         this.OffImage = null;
         this.OffImageTransparentColor = Color.White;
         this.OffImageScale = 1.0;
         this.OffText = "";
         this.OffTextAlignment = ContentAlignment.MiddleCenter;

         this.ClosedImage = null;
         this.ClosedImageTransparentColor = Color.White;
         this.ClosedImageScale = 1.0;
         this.ClosedText = "";
         this.ClosedTextAlignment = ContentAlignment.MiddleCenter;

         this.OpenedImage = null;
         this.OpenedImageTransparentColor = Color.White;
         this.OpenedImageScale = 1.0;
         this.OpenedText = "";
         this.OpenedTextAlignment = ContentAlignment.MiddleCenter;

         this.FrontLooseImage = null;
         this.FrontLooseImageTransparentColor = Color.White;
         this.FrontLooseImageScale = 1.0;
         this.FrontLooseText = "";
         this.FrontLooseTextAlignment = ContentAlignment.MiddleCenter;

         this.RearLooseImage = null;
         this.RearLooseImageTransparentColor = Color.White;
         this.RearLooseImageScale = 1.0;
         this.RearLooseText = "";
         this.RearLooseTextAlignment = ContentAlignment.MiddleCenter;

         this.DrillImage = null;
         this.DrillImageTransparentColor = Color.White;
         this.DrillImageScale = 1.0;
         this.DrillText = "";
         this.DrillTextAlignment = ContentAlignment.MiddleCenter;

         this.ManualImage = null;
         this.ManualImageTransparentColor = Color.White;
         this.ManualImageScale = 1.0;
         this.ManualText = "";
         this.ManualTextAlignment = ContentAlignment.MiddleCenter;

         this.DoubleBuffered = true;      
      }

      #endregion
   }
}
