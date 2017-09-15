
namespace UlcRobotics.Ui.ControlTest
{
   using System;
   using System.Collections.Generic;
   using System.ComponentModel;
   using System.Data;
   using System.Drawing;
   using System.Linq;
   using System.Text;
   using System.Threading.Tasks;
   using System.Windows.Forms;

   using UlcRobotics.Ui.Controls;

   public partial class MainForm : Form
   {
      #region Fields

      #endregion

      #region Events

      #endregion

      #region Constructor

      public MainForm()
      {
         this.InitializeComponent();
      }

      #endregion

      private void TickIndicator_Click(object sender, EventArgs e)
      {

      }

      private void PipeSetButton_Click(object sender, EventArgs e)
      {
         UInt16 p1 = 0;
         UInt16 p2 = 0;
         UInt16 p3 = 0;
         UInt16 p4 = 0;

         if ((UInt16.TryParse(this.PipeP1TextBox.Text, out p1) != false) &&
             (UInt16.TryParse(this.PipeP2TextBox.Text, out p2) != false) &&
             (UInt16.TryParse(this.PipeP3TextBox.Text, out p3) != false) &&
             (UInt16.TryParse(this.PipeP4TextBox.Text, out p4) != false))
         {
            UInt16[] points = new UInt16[4] { p1, p2, p3, p4 };
            this.TestBoreDataControl.BoundaryReadings = points;
         }
      }

      private void PipeBSetButton_Click(object sender, EventArgs e)
      {
         UInt16 p1 = 0;
         UInt16 p2 = 0;
         UInt16 p3 = 0;
         UInt16 p4 = 0;
         UInt16 p5 = 0;
         UInt16 p6 = 0;
         UInt16 p7 = 0;
         UInt16 p8 = 0;
         UInt16 p9 = 0;
         UInt16 p10 = 0;
         UInt16 p11 = 0;
         UInt16 p12 = 0;
         UInt16 p13 = 0;
         UInt16 p14 = 0;
         UInt16 p15 = 0;

         if ((UInt16.TryParse(this.PipeBP1TextBox.Text, out p1) != false) &&
             (UInt16.TryParse(this.PipeBP2TextBox.Text, out p2) != false) &&
             (UInt16.TryParse(this.PipeBP3TextBox.Text, out p3) != false) &&
             (UInt16.TryParse(this.PipeBP4TextBox.Text, out p4) != false) &&
             (UInt16.TryParse(this.PipeBP5TextBox.Text, out p5) != false) &&
             (UInt16.TryParse(this.PipeBP6TextBox.Text, out p6) != false) &&
             (UInt16.TryParse(this.PipeBP7TextBox.Text, out p7) != false) &&
             (UInt16.TryParse(this.PipeBP8TextBox.Text, out p8) != false) &&
             (UInt16.TryParse(this.PipeBP9TextBox.Text, out p9) != false) &&
             (UInt16.TryParse(this.PipeBP10TextBox.Text, out p10) != false) &&
             (UInt16.TryParse(this.PipeBP11TextBox.Text, out p11) != false) &&
             (UInt16.TryParse(this.PipeBP12TextBox.Text, out p12) != false) &&
             (UInt16.TryParse(this.PipeBP13TextBox.Text, out p13) != false) &&
             (UInt16.TryParse(this.PipeBP14TextBox.Text, out p14) != false) &&
             (UInt16.TryParse(this.PipeBP15TextBox.Text, out p15) != false))
         {
            UInt16[] points = new UInt16[15] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15 };
            this.TestBoreDataControl.BoundaryReadings = points;
         }
      }

      private void ReadButton_Click(object sender, EventArgs e)
      {
         this.ReadingRichTextBox.Clear();

         UInt16[] readings = this.TestBoreDataControl.SensorReadings;

         if (null != readings)
         {
            for (int i = 0; i < readings.Length; i++)
            {
               string text = string.Format("{0}: {1}\n", i, readings[i]);
               this.ReadingRichTextBox.AppendText(text);
            }
         }
      }

      private void PipeBP4TextBox_TextChanged(object sender, EventArgs e)
      {

      }

   }
}
 