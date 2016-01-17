using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NICBOT.BusSim
{
   public partial class AnalogInputControl : UserControl
   {
      #region Fields

      private int reportValue;

      #endregion

      #region Helper Functions

      private int LimitedValue(int value)
      {
         int result = value;

         if (result < this.ValueTrackBar.Minimum)
         {
            result = this.ValueTrackBar.Minimum;
         }
         else if (result > this.ValueTrackBar.Maximum)
         {
            result = this.ValueTrackBar.Maximum;
         }

         return(result);
      }

      #endregion

      #region Properties

      public int Value
      {
         set
         {
            this.reportValue = LimitedValue(value);
            this.ReportTextBox.Text = this.reportValue.ToString();
            this.ValueTrackBar.Value = this.reportValue;
         }

         get
         {
            return (this.reportValue);
         }
      }

      public string ValueText
      {
         set
         {
            int setValue = 0;

            if (int.TryParse(value, out setValue) != false)
            {
               this.reportValue = setValue;
               this.ReportTextBox.Text = this.reportValue.ToString();
               this.ValueTrackBar.Value = this.reportValue;
            }
         }

         get
         {
            return (this.reportValue.ToString());
         }
      }

      public bool Follows
      {
         set
         {
            this.FollowCheckBox.Checked = value;
         }

         get
         {
            return (this.FollowCheckBox.Checked);
         }
      }

      public string Descriptor
      {
         set
         {
            this.DescriptorLabel.Text = value;
         }

         get
         {
            return (this.DescriptorLabel.Text);
         }
      }

      #endregion

      #region User Events

      private void SetButton_Click(object sender, EventArgs e)
      {
         int value = 0;

         if (int.TryParse(this.ValueEntryTextBox.Text, out value) != false)
         {
            this.reportValue = LimitedValue(value);

            this.ReportTextBox.Text = this.reportValue.ToString();
            this.ValueTrackBar.Value = this.reportValue;
         }
      }

      #endregion

      #region Form Events

      private void ValueTrackBar_Scroll(object sender, EventArgs e)
      {
         this.reportValue = this.ValueTrackBar.Value;
         this.ReportTextBox.Text = this.reportValue.ToString();
      }

      private void FollowCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         if (false != this.FollowCheckBox.Checked)
         {
            this.ValueEntryTextBox.Enabled = false;
            this.SetButton.Enabled = false;
            this.ValueTrackBar.Enabled = false;
         }
         else
         {
            this.ValueEntryTextBox.Enabled = true;
            this.SetButton.Enabled = true;
            this.ValueTrackBar.Enabled = true;
         }
      }

      #endregion

      #region Constructor

      public AnalogInputControl()
      {
         this.InitializeComponent();
         this.reportValue = 0;
         this.ValueEntryTextBox.Text = "";
         this.ReportTextBox.Text = this.reportValue.ToString();
         this.ValueTrackBar.Value = this.reportValue;
      }

      #endregion

   }
}
