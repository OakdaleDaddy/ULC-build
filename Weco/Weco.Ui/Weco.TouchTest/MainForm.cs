
namespace Weco.TouchTest
{
   using System;
   using System.Collections.Generic;
   using System.ComponentModel;
   using System.Data;
   using System.Diagnostics;
   using System.Drawing;
   using System.Linq;
   using System.Text;
   using System.Threading.Tasks;
   using System.Windows.Forms;

   public partial class MainForm : TouchForm
   {
      #region User Events

      private void TestButton_Click(object sender, EventArgs e)
      {
         System.Diagnostics.Trace.WriteLine("click");
      }

      private void TestButton_MouseDown(object sender, MouseEventArgs e)
      {
         System.Diagnostics.Trace.WriteLine("mouse down");
      }

      private void TestButton_MouseUp(object sender, MouseEventArgs e)
      {
         System.Diagnostics.Trace.WriteLine("mouse up");
      }

      private void TestButton_MouseClick(object sender, MouseEventArgs e)
      {
         System.Diagnostics.Trace.WriteLine("mouse click");
      }

      private void BaseButton_Click(object sender, EventArgs e)
      {
         System.Diagnostics.Trace.WriteLine("click");
      }

      private void BaseButton_MouseDown(object sender, MouseEventArgs e)
      {
         System.Diagnostics.Trace.WriteLine("mouse down");
      }

      private void BaseButton_MouseUp(object sender, MouseEventArgs e)
      {
         System.Diagnostics.Trace.WriteLine("mouse up");
      }

      private void BaseButton_MouseClick(object sender, MouseEventArgs e)
      {
         System.Diagnostics.Trace.WriteLine("mouse click");
      }

      private void CloseButton_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      #endregion

      #region Form Events

      private void OnTouchDownHandler(object sender, WMTouchEventArgs e)
      {
         this.TouchIndicator.IndicatorColor = Color.Lime;
      }

      private void OnTouchUpHandler(object sender, WMTouchEventArgs e)
      {
         this.TouchIndicator.IndicatorColor = Color.FromArgb(0, 60, 15);
      }

      private void OnTouchMoveHandler(object sender, WMTouchEventArgs e)
      {
      }

      #endregion

      #region Constructor

      public MainForm()
      {
         this.InitializeComponent();

         this.Touchdown += this.OnTouchDownHandler;
         this.Touchup += this.OnTouchUpHandler;
         this.TouchMove += this.OnTouchMoveHandler;
      }

      #endregion

   }
}
