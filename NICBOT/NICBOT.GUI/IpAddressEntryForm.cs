namespace NICBOT.GUI
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

   public partial class IpAddressEntryForm : Form
   {
      #region Fields

      private StringBuilder valueString;

      private int[] fieldDigitCounts;
      private int[] fieldValues;
      private int fieldIndex;

      #endregion

      #region Properties

      public string Title { set; get; }

      public string PresentValue { set; get; }
      public string EnteredValue { set; get; }
      public string DefaultValue { set; get; }

      #endregion

      #region Helper Functions

      private void UpdateEnteredDisplay()
      {
         this.RenderValue();
         this.EnteredValueLabel.Text = this.valueString.ToString();

         if (this.fieldIndex == (this.fieldDigitCounts.Length - 1) && (this.fieldDigitCounts[(this.fieldDigitCounts.Length - 1)] > 0))
         {
            this.AcceptedButton.Enabled = true;
         }
         else
         {
            this.AcceptedButton.Enabled = false;
         }
      }

      private void RenderValue()
      {
         this.valueString.Clear();

         for (int i = 0; i < this.fieldValues.Length; i++)
         {
            if (this.fieldDigitCounts[i] > 0)
            {
               this.valueString.AppendFormat("{0}", this.fieldValues[i]);
            }

            if (i < (this.fieldValues.Length - 1))
            {
               this.valueString.Append(".");
            }
         }
      }

      private int GetValueDigitCount(int value)
      {
         int result = 1;

         if (value > 99)
         {
            result = 3;
         }
         else if (value > 9)
         {
            result = 2;
         }

         return (result);
      }

      private void InitializeValueString()
      {
         string[] fields = this.EnteredValue.Split(new char[] {'.'} );

         for (int i = 0; i < this.fieldDigitCounts.Length; i++)
         {
            this.fieldDigitCounts[i] = 1;
            this.fieldValues[i] = 0;

            if ((null != fields) && (i < fields.Length))
            {
               int value = 0;

               if (int.TryParse(fields[i], out value) != false)
               {
                  if (value > 255)
                  {
                     value = 255;
                  }

                  this.fieldValues[i] = value;
                  this.fieldDigitCounts[i] = this.GetValueDigitCount(value);
               }
            }
         }

         this.fieldIndex = this.fieldDigitCounts.Length-1;
      }

      private void AddCharacter(char ch)
      {
         if ('.' == ch)
         {
            if (this.fieldIndex < (this.fieldDigitCounts.Length - 1))
            {
               this.fieldIndex++;
            }
         }
         else if ((ch >= '0') && (ch <= '9'))
         {
            int addedValue = (ch - '0');
            int updatedFieldValue = (this.fieldValues[this.fieldIndex] * 10) + addedValue;

            if (updatedFieldValue > 255)
            {
               if (this.fieldIndex < (this.fieldDigitCounts.Length - 1))
               {
                  this.fieldIndex++;
                  this.fieldValues[this.fieldIndex] = addedValue;
               }
            }
            else
            {
               this.fieldValues[this.fieldIndex] = updatedFieldValue;              
            }

            this.fieldDigitCounts[this.fieldIndex] = this.GetValueDigitCount(this.fieldValues[this.fieldIndex]);
         }
      }

      #endregion

      #region User Events

      private void DefaultButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         this.EnteredValue = this.DefaultValue;
         this.InitializeValueString();
         this.UpdateEnteredDisplay();

         e.Handled = true;
      }

      private void ClearButton_Click(object sender, EventArgs e)
      {
         this.valueString.Clear();

         for (int i = 0; i < this.fieldDigitCounts.Length; i++)
         {
            this.fieldDigitCounts[i] = 0;
            this.fieldValues[i] = 0;
         }

         this.fieldIndex = 0;
         this.UpdateEnteredDisplay();
      }

      private void DeleteButton_Click(object sender, EventArgs e)
      {
         if (this.fieldDigitCounts[this.fieldIndex] > 0)
         {
            this.fieldValues[this.fieldIndex] = this.fieldValues[this.fieldIndex] / 10;
            this.fieldDigitCounts[this.fieldIndex]--;

            if ((0 == this.fieldDigitCounts[this.fieldIndex]) && (this.fieldIndex > 0))
            {
               this.fieldIndex--;
            }
         }

         this.UpdateEnteredDisplay();
      }

      private void DecimalButton_Click(object sender, EventArgs e)
      {
         this.AddCharacter('.');
         this.UpdateEnteredDisplay();
      }

      private void ZeroButton_Click(object sender, EventArgs e)
      {
         this.AddCharacter('0');
         this.UpdateEnteredDisplay();
      }

      private void OneButton_Click(object sender, EventArgs e)
      {
         this.AddCharacter('1');
         this.UpdateEnteredDisplay();
      }

      private void TwoButton_Click(object sender, EventArgs e)
      {
         this.AddCharacter('2');
         this.UpdateEnteredDisplay();
      }

      private void ThreeButton_Click(object sender, EventArgs e)
      {
         this.AddCharacter('3');
         this.UpdateEnteredDisplay();
      }

      private void FourButton_Click(object sender, EventArgs e)
      {
         this.AddCharacter('4');
         this.UpdateEnteredDisplay();
      }

      private void FiveButton_Click(object sender, EventArgs e)
      {
         this.AddCharacter('5');
         this.UpdateEnteredDisplay();
      }

      private void SixButton_Click(object sender, EventArgs e)
      {
         this.AddCharacter('6');
         this.UpdateEnteredDisplay();
      }

      private void SevenButton_Click(object sender, EventArgs e)
      {
         this.AddCharacter('7');
         this.UpdateEnteredDisplay();
      }

      private void EightButton_Click(object sender, EventArgs e)
      {
         this.AddCharacter('8');
         this.UpdateEnteredDisplay();
      }

      private void NineButton_Click(object sender, EventArgs e)
      {
         this.AddCharacter('9');
         this.UpdateEnteredDisplay();
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

      private void IpAddressEntryForm_Shown(object sender, EventArgs e)
      {
         this.TitleLabel.Text = this.Title;

         this.PresentValueLabel.Text = this.PresentValue;
         this.DefaultValueLabel.Text = this.DefaultValue;

         this.valueString.Clear();
         this.UpdateEnteredDisplay();
      }

      #endregion

      #region Constructor

      public IpAddressEntryForm()
      {
         this.InitializeComponent();
         this.fieldDigitCounts = new int[4];
         this.fieldValues = new int[4];
         this.valueString = new StringBuilder();
      }

      #endregion


   }
}
