using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NICBOT.GUI
{
   public class LineControl : Control
   {
      public enum LineDrawType
      {
         Top,
         Bottom,
         Left,
         Right,
         DiagonalUp,
         DiagonalDown,
      }

      public bool drag = false;
      public bool enab = false;
      private int m_opacity = 100;

      private int alpha; 
      private bool _showBackground;

      private LineDrawType _lineType;
      private int _lineWeight;

      private Color _edgeColor;
      private bool _showEdge;

      public LineDrawType LineType
      {
         set
         {
            this._lineType = value;
            this.Invalidate();
         }

         get { return (this._lineType); }
      }

      public int LineWeight
      {
         set
         {
            this._lineWeight = value;
            this.Invalidate();
         }

         get { return (this._lineWeight); }
      }

      public Color EdgeColor
      {
         set
         {
            this._edgeColor = value;
            this.Invalidate();
         }

         get { return (this._edgeColor); }
      }

      public bool ShowEdge
      {
         set
         {
            this._showEdge = value;
            this.Invalidate();
         }

         get { return (this._showEdge); }
      }

      public LineControl()
         : base()
      {
         SetStyle(ControlStyles.SupportsTransparentBackColor, true);
         SetStyle(ControlStyles.Opaque, true);
         this.BackColor = Color.Transparent;

         this.LineType = LineControl.LineDrawType.Top;
         this.ShowBackground = false;
         this.LineWeight = 1;
         this.ShowEdge = false;
         this.EdgeColor = Color.Black;         
      }

      public bool ShowBackground
      {
         set
         {
            this._showBackground = value;
            this.Invalidate();
         }

         get { return (this._showBackground); }
      }

      public int Opacity
      {
         get
         {
            if (m_opacity > 100)
            {
               m_opacity = 100;
            }
            else if (m_opacity < 1)
            {
               m_opacity = 1;
            }
            return this.m_opacity;
         }
         set
         {
            this.m_opacity = value;
            if (this.Parent != null)
            {
               Parent.Invalidate(this.Bounds, true);
            }
         }
      }

      protected override CreateParams CreateParams
      {
         get
         {
            CreateParams cp = base.CreateParams;
            cp.ExStyle = cp.ExStyle | 0x20;
            return cp;
         }
      }

      protected override void OnPaint(PaintEventArgs e)
      {
         Graphics g = e.Graphics;
         Rectangle bounds = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

         Color frmColor = this.Parent.BackColor;
         Brush bckColor = default(Brush);

         alpha = (m_opacity * 255) / 100;

         if (drag)
         {
            Color dragBckColor = default(Color);

            if (BackColor != Color.Transparent)
            {
               int Rb = BackColor.R * alpha / 255 + frmColor.R * (255 - alpha) / 255;
               int Gb = BackColor.G * alpha / 255 + frmColor.G * (255 - alpha) / 255;
               int Bb = BackColor.B * alpha / 255 + frmColor.B * (255 - alpha) / 255;
               dragBckColor = Color.FromArgb(Rb, Gb, Bb);
            }
            else
            {
               dragBckColor = frmColor;
            }

            alpha = 255;
            bckColor = new SolidBrush(Color.FromArgb(alpha, dragBckColor));
         }
         else
         {
            bckColor = new SolidBrush(Color.FromArgb(alpha, this.BackColor));
         }

         if (this.BackColor != Color.Transparent | drag)
         {
            g.FillRectangle(bckColor, bounds);
         }

         Point a, b;
         Point edgeA, edgeB, edgeC, edgeD;

         edgeA = new Point(0, 0);
         edgeB = new Point(0, 0);
         edgeC = new Point(0, 0);
         edgeD = new Point(0, 0);

         switch (this.LineType)
         {
            default:
            case LineDrawType.Top:
            {
               int offset = this.LineWeight / 2;

               a = new Point(0, offset);
               b = new Point(this.ClientRectangle.Width, offset);

               edgeA = new Point(0, 0);
               edgeB = new Point(this.ClientRectangle.Width, 0);

               edgeC = new Point(0, this.LineWeight - 1);
               edgeD = new Point(this.ClientRectangle.Width, this.LineWeight - 1);
               
               break;
            }
            case LineDrawType.Bottom:
            {
               int offset = (this.LineWeight - 1) / 2;

               a = new Point(0, this.ClientRectangle.Height - (int)offset - 1);
               b = new Point(this.ClientRectangle.Width, this.ClientRectangle.Height - (int)offset - 1);

               edgeA = new Point(0, this.ClientRectangle.Height - this.LineWeight);
               edgeB = new Point(this.ClientRectangle.Width, this.ClientRectangle.Height - this.LineWeight);

               edgeC = new Point(0, this.ClientRectangle.Height - 1);
               edgeD = new Point(this.ClientRectangle.Width, this.ClientRectangle.Height - 1);
               
               break;
            }
            case LineDrawType.Left:
            {
               int offset = this.LineWeight / 2;

               a = new Point(offset, 0);
               b = new Point(offset, this.ClientRectangle.Height);

               edgeA = new Point(0, 0);
               edgeB = new Point(0, this.ClientRectangle.Height - 1);

               edgeC = new Point(this.LineWeight - 1, 0);
               edgeD = new Point(this.LineWeight - 1, this.ClientRectangle.Height - 1);

               break;
            }
            case LineDrawType.Right:
            {
               int offset = (this.LineWeight - 1) / 2;
               a = new Point(this.ClientRectangle.Width - offset - 1, 0);
               b = new Point(this.ClientRectangle.Width - offset - 1, this.ClientRectangle.Height);

               edgeA = new Point(this.ClientRectangle.Width - 1, 0);
               edgeB = new Point(this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);

               edgeC = new Point(this.ClientRectangle.Width - this.LineWeight, 0);
               edgeD = new Point(this.ClientRectangle.Width - this.LineWeight, this.ClientRectangle.Height - 1);

               break;
            }
            case LineDrawType.DiagonalUp:
            {
               double tan_a = (double)this.ClientRectangle.Width / (double)this.ClientRectangle.Height;
               double angle_a = Math.Atan(tan_a);
               double angle_b = (Math.PI / 2) - angle_a;

               int offsetC = this.LineWeight / 2;
               int offsetA1 = (int)((offsetC + 1) / Math.Sin(angle_a));
               int offsetA2 = (int)((offsetC) / Math.Sin(angle_a));
               int offsetB1 = (int)((offsetC) / Math.Sin(angle_b));
               int offsetB2 = (int)((offsetC + 1) / Math.Sin(angle_b));

               a = new Point(0, this.ClientRectangle.Height - 1);
               b = new Point(this.ClientRectangle.Width - 1, 0);

               edgeA = new Point(0, this.ClientRectangle.Height - offsetA1);
               edgeB = new Point(this.ClientRectangle.Width - offsetB2, 0);

               edgeC = new Point(offsetB1, this.ClientRectangle.Height);
               edgeD = new Point(this.ClientRectangle.Width, offsetA2);

               break;
            }
            case LineDrawType.DiagonalDown:
            {
               double tan_a = (double)this.ClientRectangle.Height / (double)this.ClientRectangle.Width;
               double angle_a = Math.Atan(tan_a);
               double angle_b = (Math.PI / 2) - angle_a;

               int offsetC = this.LineWeight / 2;
               int offsetA1 = (int)((offsetC) / Math.Sin(angle_a));
               int offsetA2 = (int)((offsetC + 1) / Math.Sin(angle_a));
               int offsetB1 = (int)((offsetC + 1) / Math.Sin(angle_b));
               int offsetB2 = (int)((offsetC) / Math.Sin(angle_b));

               a = new Point(0, 0);
               b = new Point(this.ClientRectangle.Width, this.ClientRectangle.Height);

               edgeA = new Point(offsetA1, 0);
               edgeB = new Point(this.ClientRectangle.Width, this.ClientRectangle.Height - offsetB2);

               edgeC = new Point(0, offsetB1 - 1);
               edgeD = new Point(this.ClientRectangle.Width - offsetA2, this.ClientRectangle.Height - 1);
               
               break;
            }
         }

         e.Graphics.DrawLine(new Pen(this.ForeColor, this.LineWeight), a, b);

         if (false != this.ShowEdge)
         {
            e.Graphics.DrawLine(new Pen(this.EdgeColor, 1), edgeA, edgeB);
            e.Graphics.DrawLine(new Pen(this.EdgeColor, 1), edgeC, edgeD);
         }

         bckColor.Dispose();
         g.Dispose();
      }

      protected override void OnBackColorChanged(EventArgs e)
      {
         if (this.Parent != null)
         {
            Parent.Invalidate(this.Bounds, true);
         }
         base.OnBackColorChanged(e);
      }

      protected override void OnParentBackColorChanged(EventArgs e)
      {
         this.Invalidate();
         base.OnParentBackColorChanged(e);
      }
   }
}
