using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NICBOT.GUI
{
   public partial class HourMinuteEntryForm : Form
   {
      #region Fields

      private StringBuilder valueString;

      #endregion

      #region Properties

      public string Title { set; get; }

      public string PresentValue { set; get; }
      public string EnteredValue { set; get; }
      public string DefaultValue { set; get; }

      #endregion

      #region Helper Functions

      private bool hourTenOrMore;

      private void AddCharacter(char ch)
      {
         int length = this.valueString.Length;

         if (0 == length)
         {
            if ('0' == ch)
            {
               this.valueString.Append(ch);
               this.hourTenOrMore = false;
            }
            else if ('1' == ch)
            {
               this.valueString.Append(ch);
               this.hourTenOrMore = true;
            }
            else if ((ch >= '2') && (ch <= '9'))
            {
               this.valueString.Append('0');
               this.valueString.Append(ch);
               this.valueString.Append(':');
               this.hourTenOrMore = false;
            }
            else if (':' == ch)
            {
               this.valueString.Append('0');
               this.valueString.Append('0');
               this.valueString.Append(':');
               this.hourTenOrMore = false;
            }
         }
         else if (1 == length)
         {
            if (':' == ch)
            {
               char temp = this.valueString[0];
               this.valueString.Clear();
               
               this.valueString.Append('0');
               this.valueString.Append(temp);
               this.valueString.Append(':');
            }
            else if (false != this.hourTenOrMore)
            {
               if (('0' == ch) || ('1' == ch))
               {
                  this.valueString.Append(ch);
                  this.valueString.Append(':');
               }
            }
            else
            {
               if ((ch >= '0') && (ch <= '9'))
               {
                  this.valueString.Append(ch);
                  this.valueString.Append(':');
               }
            }
         }
         else if (3 == length)
         {
            if ((ch >= '0') && (ch <= '5'))
            {
               this.valueString.Append(ch);
            }
            else if ((ch >= '6') && (ch <= '9'))
            {
               this.valueString.Append('0');
               this.valueString.Append(ch);
            }
         }
         else if (4 == length)
         {
            if ((ch >= '0') && (ch <= '9'))
            {
               this.valueString.Append(ch);
               this.AcceptedButton.Enabled = true;
            }
         }

         this.EnteredValueLabel.Text = this.valueString.ToString();
      }

      #endregion

      #region User Events

      private void DefaultButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         this.valueString.Clear();
         this.valueString.Append(this.DefaultValue);
         this.EnteredValueLabel.Text = this.valueString.ToString();
         this.AcceptedButton.Enabled = true;

         e.Handled = true;
      }

      private void ClearButton_Click(object sender, EventArgs e)
      {
         this.EnteredValueLabel.Text = "";
         this.valueString.Clear();
         this.AcceptedButton.Enabled = false;
      }

      private void DeleteButton_Click(object sender, EventArgs e)
      {
         int length = this.valueString.Length;

         if (3 == length)
         {
            this.valueString.Remove(length - 2, 2);
         }
         else if (length > 0)
         {
            this.valueString.Remove(length - 1, 1);
         }

         this.AcceptedButton.Enabled = false;
         this.EnteredValueLabel.Text = this.valueString.ToString();
      }

      private void ColonButton_Click(object sender, EventArgs e)
      {
         this.AddCharacter(':');
      }

      private void ZeroButton_Click(object sender, EventArgs e)
      {
         this.AddCharacter('0');
      }

      private void OneButton_Click(object sender, EventArgs e)
      {
         this.AddCharacter('1');
      }

      private void TwoButton_Click(object sender, EventArgs e)
      {
         this.AddCharacter('2');
      }

      private void ThreeButton_Click(object sender, EventArgs e)
      {
         this.AddCharacter('3');
      }

      private void FourButton_Click(object sender, EventArgs e)
      {
         this.AddCharacter('4');
      }

      private void FiveButton_Click(object sender, EventArgs e)
      {
         this.AddCharacter('5');
      }

      private void SixButton_Click(object sender, EventArgs e)
      {
         this.AddCharacter('6');
      }

      private void SevenButton_Click(object sender, EventArgs e)
      {
         this.AddCharacter('7');
      }

      private void EightButton_Click(object sender, EventArgs e)
      {
         this.AddCharacter('8');
      }

      private void NineButton_Click(object sender, EventArgs e)
      {
         this.AddCharacter('9');
      }

      private void AcceptedButton_Click(object sender, EventArgs e)
      {
         this.EnteredValue = this.valueString.ToString();
         this.DialogResult = System.Windows.Forms.DialogResult.OK;
      }

      private void CanceledButton_Click(object sender, EventArgs e)
      {
         this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      }

      #endregion

      #region Form Events

      private void HourMinuteEntryForm_Shown(object sender, EventArgs e)
      {
         this.TitleLabel.Text = this.Title;

         this.PresentValueLabel.Text = this.PresentValue;
         this.DefaultValueLabel.Text = this.DefaultValue;
         this.EnteredValueLabel.Text = "";

         this.AcceptedButton.Enabled = false;
      }

      #endregion

      #region Constructor

      public HourMinuteEntryForm()
      {
         this.InitializeComponent();

         this.valueString = new StringBuilder();
      }

      #endregion

   }
}
