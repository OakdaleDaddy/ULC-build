
namespace NICBOT.GUI
{
   using System;
   using System.Collections.Generic;
   using System.ComponentModel;
   using System.Data;
   using System.Drawing;
   using System.Linq;
   using System.Text;
   using System.Threading;
   using System.Threading.Tasks;
   using System.Windows.Forms;

   public partial class NumberEntryForm : Form
   {
      #region Fields

      private string doubleFormat;
      private StringBuilder valueString;
      private bool negativeValue;
      private bool decimalEntered;
      private int postDecimalDigitCount;
      
      private bool showMinimumError;
      private bool showMaximumError;
      private int errorCounter;

      #endregion

      #region Properties

      public string Title { set; get; }
      public int PostDecimalDigitCount { set; get; }
      public string Unit { set; get; }

      public double PresentValue { set; get; }
      public double EnteredValue { set; get; }
      public double DefaultValue { set; get; }
      public double MinimumValue { set; get; }
      public double MaximumValue { set; get; }

      #endregion

      #region Helper Functions

      private void InitializeValueString()
      {
         this.valueString = new StringBuilder(this.EnteredValue.ToString(this.doubleFormat));

         if (this.EnteredValue < 0)
         {
            this.negativeValue = true;
         }

         if (this.valueString.ToString().Contains('.') != false)
         {
            this.decimalEntered = true;
            this.postDecimalDigitCount = this.PostDecimalDigitCount;
         }
         else
         {
            this.decimalEntered = false;
            this.postDecimalDigitCount = 0;
         }
      }
      
      private string GetValueText(double value)
      {
         string result = "";

         if (double.IsNaN(value) == false)
         {
            result += value.ToString(this.doubleFormat);
         }

         result += " " + this.Unit;

         return (result);
      }

      private double GetValue()
      {
         double result = double.NaN;

         if (double.TryParse(this.valueString.ToString(), out result) == false)
         {
            result = 0;
         }

         return (result);
      }

      private void AddCharacter(char ch)
      {
         if (false == this.decimalEntered)
         {
            int firstDigitEntryLength = (false != this.negativeValue) ? 2 : 1;

            if (this.valueString.Length < firstDigitEntryLength)
            {
               if ('.' == ch)
               {
                  this.valueString.Append('0');
                  this.valueString.Append(ch);
               }
               else
               {
                  this.valueString.Append(ch);
               }
            }
            else if (this.valueString.Length > firstDigitEntryLength)
            {
               this.valueString.Append(ch);
            }
            else 
            {
               if ('0' == this.valueString[firstDigitEntryLength-1])
               {
                  if ('.' == ch)
                  {
                     this.valueString.Append(ch);
                  }
                  else 
                  {
                     this.valueString[firstDigitEntryLength-1] = ch;
                  }
               }
               else
               {
                  this.valueString.Append(ch);
               }
            }
         }
         else if (this.postDecimalDigitCount < this.PostDecimalDigitCount)
         {
            this.postDecimalDigitCount++;
            this.valueString.Append(ch);
         }
      }

      private void UpdateEnteredDisplay()
      {
         this.EnteredValueLabel.Text = this.valueString.ToString() + " " + this.Unit;

         double value = GetValue();

         if ((double.IsNaN(value) != false) || (value < this.MinimumValue))
         {
            this.MinimumLabel.ForeColor = Color.Red;
         }
         else
         {
            this.MinimumLabel.ForeColor = Color.Black;
         }

         if ((double.IsNaN(value) != false) || (value > this.MaximumValue))
         {
            this.MaximumLabel.ForeColor = Color.Red;
         }
         else
         {
            this.MaximumLabel.ForeColor = Color.Black;
         }
      }

      #endregion

      #region User Event Handlers 

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
         this.UpdateEnteredDisplay();

         this.negativeValue = false;
         this.decimalEntered = false;
         this.postDecimalDigitCount = 0;
      }

      private void DeleteButton_Click(object sender, EventArgs e)
      {
         int length = this.valueString.Length;

         if (length > 0)
         {
            this.valueString.Remove(length - 1, 1);
         }

         if (false != this.decimalEntered)
         {
            if (0 == this.postDecimalDigitCount)
            {
               this.decimalEntered = false;
            }
            else
            {
               this.postDecimalDigitCount--;
            }
         }

         if (0 == this.valueString.Length)
         {
            this.negativeValue = false;
         }

         this.UpdateEnteredDisplay();
      }

      private void SignButton_Click(object sender, EventArgs e)
      {
         if (false == this.negativeValue)
         {
            this.valueString.Insert(0, '-');
            this.negativeValue = true;
         }
         else
         {
            this.valueString.Remove(0, 1);
            this.negativeValue = false;
         }

         this.UpdateEnteredDisplay();
      }

      private void DecimalButton_Click(object sender, EventArgs e)
      {
         if (false == this.decimalEntered)
         {
            this.AddCharacter('.');
            this.UpdateEnteredDisplay();
            this.decimalEntered = true;
         }
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
         double enteredValue = GetValue();

         if ((double.IsNaN(enteredValue) != false) || (enteredValue < this.MinimumValue))
         {
            this.showMinimumError = true;
            this.errorCounter = 0;
            this.UpdateTimer.Enabled = true;
         }
         else if (enteredValue > this.MaximumValue)
         {
            this.showMaximumError = true;
            this.errorCounter = 0;
            this.UpdateTimer.Enabled = true;
         }
         else
         {
            this.EnteredValue = enteredValue;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
         }
      }

      private void CanceledButton_Click(object sender, EventArgs e)
      {
         this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      }
      
      #endregion

      #region Form Event Handler

      private void NumberEntryForm_Shown(object sender, EventArgs e)
      {
         if (0 == this.PostDecimalDigitCount)
         {
            this.DecimalButton.Enabled = false;
         }
         else
         {
            this.DecimalButton.Enabled = true;
         }

         if (this.MinimumValue >= 0)
         {
            this.SignButton.Enabled = false;
         }
         else
         {
            this.SignButton.Enabled = true;
         }

         this.doubleFormat = "N" + this.PostDecimalDigitCount.ToString();
         this.EnteredValue = 0;

         this.MinimumLabel.Text = this.MinimumValue.ToString(doubleFormat);
         this.MaximumLabel.Text = this.MaximumValue.ToString(doubleFormat);


         this.TitleLabel.Text = this.Title;

         this.PresentValueLabel.Text = this.GetValueText(this.PresentValue);
         this.DefaultValueLabel.Text = this.GetValueText(this.DefaultValue);

         this.valueString = new StringBuilder();
         this.negativeValue = false;
         this.decimalEntered = false;
         this.postDecimalDigitCount = 0;

         this.UpdateEnteredDisplay();
      }

      private void UpdateTimer_Tick(object sender, EventArgs e)
      {
         if (false != this.showMinimumError)
         {
            this.MinimumLabel.ForeColor = ((this.errorCounter & 1) != 0) ? Color.Red : Color.Black;
            this.errorCounter++;

            if (this.errorCounter >= 6)
            {
               this.showMinimumError = false;
            }
         }
         else if (false != this.showMaximumError)
         {
            this.MaximumLabel.ForeColor = ((this.errorCounter & 1) != 0) ? Color.Red : Color.Black;
            this.errorCounter++;

            if (this.errorCounter >= 6)
            {
               this.showMaximumError = false;
            }
         }
         else
         {
            this.UpdateTimer.Enabled = false;
         }
      }
      
      #endregion

      #region Constructor

      public NumberEntryForm()
      {
         this.InitializeComponent();
      }

      #endregion


   }
}
