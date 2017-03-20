using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace E4.Ui.ControlTest
{
   public partial class MainForm : Form
   {
      #region Scanner Indicator Events

      private void FaultCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         if (false != this.FaultCheckBox.Checked)
         {
            this.TestScannerIndicator.MissColor = Color.Red;
            this.TestScannerIndicator.BackColor = Color.Red;
            this.TestScannerIndicator.CoordinateValue = 0;
         }
         else
         {
            this.TestScannerIndicator.MissColor = Color.FromArgb(140, 0, 0);
            this.TestScannerIndicator.BackColor = Color.FromKnownColor(KnownColor.ControlDark);
            byte value = (byte)((this.XTrackBar.Value << 4) | (16 - this.YTrackBar.Value));
            this.TestScannerIndicator.CoordinateValue = value;
         }
      }

      private void ZeroCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         if (false == this.FaultCheckBox.Checked)
         {
            if (false != this.ZeroCheckBox.Checked)
            {
               this.TestScannerIndicator.CoordinateValue = 0;
            }
            else
            {
               byte value = (byte)((this.XTrackBar.Value << 4) | (16 - this.YTrackBar.Value));
               this.TestScannerIndicator.CoordinateValue = value;
            }
         }
      }

      private void XTrackBar_Scroll(object sender, EventArgs e)
      {
         if (false == this.FaultCheckBox.Checked)
         {
            byte value = (byte)((this.XTrackBar.Value << 4) | (16 - this.YTrackBar.Value));
            this.TestScannerIndicator.CoordinateValue = value;
         }
      }

      private void YTrackBar_Scroll(object sender, EventArgs e)
      {
         if (false == this.FaultCheckBox.Checked)
         {
            byte value = (byte)((this.XTrackBar.Value << 4) | (16 - this.YTrackBar.Value));
            this.TestScannerIndicator.CoordinateValue = value;
         }
      }

      #endregion

      #region Position Indicator Events

      private void SetButton_Click(object sender, EventArgs e)
      {
         int value = 0;

         if (int.TryParse(this.ValueTextBox.Text, out value) != false)
         {
            this.TestPositionIndicator.Position = value;
         }
      }

      #endregion

      #region Constructor

      public MainForm()
      {
         this.InitializeComponent();

         this.FaultCheckBox_CheckedChanged(this, EventArgs.Empty);
         this.XTrackBar_Scroll(this, EventArgs.Empty);
      }

      #endregion


   }
}
