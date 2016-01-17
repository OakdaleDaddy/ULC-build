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
   public partial class SealantSetupForm : Form
   {
      #region Properties

      public ToolLocations ToolLocation { set; get; }
      public PumpParameters Pump { set; get; }

      #endregion

      #region Helper Functions

      private string GetValueText(ValueParameter parameter)
      {
         string doubleFormat = "N" + parameter.Precision.ToString();
         string result = parameter.OperationalValue.ToString(doubleFormat) + " " + parameter.Unit;
         return (result);
      }

      private int GetAbsoluteLeft(Control control)
      {
         int result = 0;

         while (null != control)
         {
            result += control.Left;
            control = control.Parent;
         }

         return (result);
      }

      private int GetAbsoluteTop(Control control)
      {
         int result = 0;

         while (null != control)
         {
            result += control.Top;
            control = control.Parent;
         }

         return (result);
      }

      private void SetDialogLocation(Control control, Form form)
      {
         int offsetX = (form.Width - control.Width) / 2;
         int offsetY = (form.Height - control.Height) / 2;
         int formLeft = this.GetAbsoluteLeft(control) - offsetX;
         int formTop = this.GetAbsoluteTop(control) - offsetY;
         int formLeftMaximum = Application.OpenForms[0].Left + Application.OpenForms[0].Width - form.Width - 1;
         int formTopMaximum = Application.OpenForms[0].Top + Application.OpenForms[0].Height - form.Height - 1;

         if (formLeft < Application.OpenForms[0].Left)
         {
            formLeft = Application.OpenForms[0].Left;
         }
         else if (formLeft > formLeftMaximum)
         {
            formLeft = formLeftMaximum;
         }

         if (formTop < Application.OpenForms[0].Top)
         {
            formTop = Application.OpenForms[0].Top;
         }
         else if (formTop > formTopMaximum)
         {
            formTop = formTopMaximum;
         }

         form.Left = formLeft;
         form.Top = formTop;
      }

      private DialogResult LaunchNumberEdit(ValueButton button, ValueParameter valueParameter)
      {
         NumberEntryForm numberEntryForm = new NumberEntryForm();
         this.SetDialogLocation(button, numberEntryForm);

         numberEntryForm.Title = button.Text;
         numberEntryForm.Unit = valueParameter.Unit;
         numberEntryForm.PostDecimalDigitCount = valueParameter.Precision;
         numberEntryForm.PresentValue = valueParameter.OperationalValue;
         numberEntryForm.DefaultValue = valueParameter.DefaultValue;
         numberEntryForm.MinimumValue = valueParameter.MinimumValue;
         numberEntryForm.MaximumValue = valueParameter.MaximumValue;

         DialogResult result = numberEntryForm.ShowDialog();

         if (result == System.Windows.Forms.DialogResult.OK)
         {
            valueParameter.OperationalValue = numberEntryForm.EnteredValue;
            button.ValueText = this.GetValueText(valueParameter);
         }

         return (result);
      }

      private void SetTitleText()
      {
         string locationDescription = (ToolLocations.front == this.ToolLocation) ? "FRONT" : "REAR";
         this.TitleLabel.Text = locationDescription + " SEALANT PUMP SETUP";
      }

      #endregion

      #region User Event Process

      private void NozzleSelectToggleButton_Click(object sender, EventArgs e)
      {
         this.ToolLocation = (ToolLocations.front == this.ToolLocation) ? ToolLocations.rear : ToolLocations.front;
         this.NozzleSelectToggleButton.OptionASelected = (ToolLocations.front == this.ToolLocation);
         this.SetTitleText();
      }

      private void AutoFillModeToggleButton_Click(object sender, EventArgs e)
      {
         this.Pump.PressureAutoFill = !this.Pump.PressureAutoFill;
         this.AutoFillModeToggleButton.OptionASelected = this.Pump.PressureAutoFill;
      }

      private void NozzleInsertionToggleButton_Click(object sender, EventArgs e)
      {
         this.Pump.AutoNozzleRetraction = !this.Pump.AutoNozzleRetraction;
         this.NozzleInsertionToggleButton.OptionASelected = this.Pump.AutoNozzleRetraction;
      }

      private void PressureReliefToggleButton_Click(object sender, EventArgs e)
      {
         this.Pump.AutoPressureRelief = !this.Pump.AutoPressureRelief;
         this.PressureReliefToggleButton.OptionASelected = this.Pump.AutoPressureRelief;
      }

      private void AutoFillVolumeValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, this.Pump.AutoFillVolume);
      }

      private void MaximumVolumeValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         DialogResult result = this.LaunchNumberEdit(valueButton, this.Pump.MaximumVolume);

         if (result == System.Windows.Forms.DialogResult.OK)
         {
            this.Pump.AutoFillVolume.MaximumValue = this.Pump.MaximumVolume.OperationalValue;

            if (this.Pump.AutoFillVolume.OperationalValue > this.Pump.AutoFillVolume.MaximumValue)
            {
               this.Pump.AutoFillVolume.OperationalValue = this.Pump.AutoFillVolume.MaximumValue;
               this.AutoFillVolumeValueButton.ValueText = this.GetValueText(this.Pump.AutoFillVolume);
            }
         }
      }

      private void AutoFillPressureValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, this.Pump.AutoFillPressure);
      }

      private void MaximumPressureValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         DialogResult result = this.LaunchNumberEdit(valueButton, this.Pump.MaximumPressure);

         if (result == System.Windows.Forms.DialogResult.OK)
         {
            this.Pump.AutoFillPressure.MaximumValue = this.Pump.MaximumPressure.OperationalValue;
            this.Pump.RelievedPressure.MaximumValue = this.Pump.MaximumPressure.OperationalValue;

            if (this.Pump.AutoFillPressure.OperationalValue > this.Pump.AutoFillPressure.MaximumValue)
            {
               this.Pump.AutoFillPressure.OperationalValue = this.Pump.AutoFillPressure.MaximumValue;
               this.AutoFillPressureValueButton.ValueText = this.GetValueText(this.Pump.AutoFillPressure);
            }

            if (this.Pump.RelievedPressure.OperationalValue > this.Pump.RelievedPressure.MaximumValue)
            {
               this.Pump.RelievedPressure.OperationalValue = this.Pump.RelievedPressure.MaximumValue;
               this.RelievedPressureValueButton.ValueText = this.GetValueText(this.Pump.RelievedPressure);
            }
         }
      }

      private void RelievedPressureValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, this.Pump.RelievedPressure);
      }

      private void ForwardSpeedValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, this.Pump.ForwardSpeed);
      }

      private void ReverseSpeedValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, this.Pump.ReverseSpeed);
      }

      private void MaximumSpeedValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         DialogResult result = this.LaunchNumberEdit(valueButton, this.Pump.MaximumSpeed);

         if (result == System.Windows.Forms.DialogResult.OK)
         {
            this.Pump.ForwardSpeed.MaximumValue = this.Pump.MaximumSpeed.OperationalValue;
            this.Pump.ReverseSpeed.MaximumValue = this.Pump.MaximumSpeed.OperationalValue;

            if (this.Pump.ForwardSpeed.OperationalValue > this.Pump.ForwardSpeed.MaximumValue)
            {
               this.Pump.ForwardSpeed.OperationalValue = this.Pump.ForwardSpeed.MaximumValue;
               this.ForwardSpeedValueButton.ValueText = this.GetValueText(this.Pump.ForwardSpeed);
            }

            if (this.Pump.ReverseSpeed.OperationalValue > this.Pump.ReverseSpeed.MaximumValue)
            {
               this.Pump.ReverseSpeed.OperationalValue = this.Pump.ReverseSpeed.MaximumValue;
               this.ReverseSpeedValueButton.ValueText = this.GetValueText(this.Pump.ReverseSpeed);
            }
         }
      }

      private void SealantWeightValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, this.Pump.SealantWeight);
      }

      private void FlowConstantValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, this.Pump.FlowConstant);
      }

      private void BackButton_Click(object sender, EventArgs e)
      {
         this.Close();
      }
      
      #endregion

      #region Form Event Process

      private void SealantSetupForm_Shown(object sender, EventArgs e)
      {
         if (null == this.Pump)
         {
            this.Pump = new PumpParameters();
         }

         this.SetTitleText();

         this.NozzleSelectToggleButton.OptionASelected = (ToolLocations.front == this.ToolLocation);

         this.AutoFillModeToggleButton.OptionASelected = (false != this.Pump.PressureAutoFill) ? true : false;
         this.NozzleInsertionToggleButton.OptionASelected = (false != this.Pump.AutoNozzleRetraction) ? true : false;
         this.PressureReliefToggleButton.OptionASelected = (false != this.Pump.AutoPressureRelief) ? true : false;

         this.AutoFillVolumeValueButton.ValueText = this.GetValueText(this.Pump.AutoFillVolume);
         this.MaximumVolumeValueButton.ValueText = this.GetValueText(this.Pump.MaximumVolume);
         this.AutoFillPressureValueButton.ValueText = this.GetValueText(this.Pump.AutoFillPressure);
         this.MaximumPressureValueButton.ValueText = this.GetValueText(this.Pump.MaximumPressure);

         this.RelievedPressureValueButton.ValueText = this.GetValueText(this.Pump.RelievedPressure);
         this.ForwardSpeedValueButton.ValueText = this.GetValueText(this.Pump.ForwardSpeed);
         this.ReverseSpeedValueButton.ValueText = this.GetValueText(this.Pump.ReverseSpeed);
         this.MaximumSpeedValueButton.ValueText = this.GetValueText(this.Pump.MaximumSpeed);

         this.SealantWeightValueButton.ValueText = this.GetValueText(this.Pump.SealantWeight);
         this.FlowConstantValueButton.ValueText = this.GetValueText(this.Pump.FlowConstant);
      }

      #endregion

      #region Constructor

      public SealantSetupForm()
      {
         this.InitializeComponent();
      }

      #endregion

   }
}
