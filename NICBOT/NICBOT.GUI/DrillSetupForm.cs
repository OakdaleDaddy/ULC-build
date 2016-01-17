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
   public partial class DrillSetupForm : Form
   {
      #region Fields

      private ToolLocations drillLocation;
      private DrillParameters selectedDrill;

      private PopupDimmerForm dimmerForm;

      #endregion

      #region Properties

      public ToolLocations DrillLocation
      {
         set
         {
            this.drillLocation = value;

            if (ToolLocations.front == value)
            {
               this.selectedDrill = this.FrontDrillParameters;
            }
            else
            {
               this.selectedDrill = this.RearDrillParameters;
            }

            this.DrillSpeedValueButton.ValueText = this.GetValueText(this.selectedDrill.RotationSpeed);
            this.AutoOriginToggleButton.OptionASelected = this.selectedDrill.AutoOrign;
            this.DrillSearchSpeedValueButton.ValueText = this.GetValueText(this.selectedDrill.SearchSpeed);
            this.DrillModeToggleButton.OptionASelected = (false == this.selectedDrill.PeckMode) ? true : false;
            this.DrillCuttingSpeedValueButton.ValueText = this.GetValueText(this.selectedDrill.CuttingSpeed);
            this.DrillTravelSpeedValueButton.ValueText = this.GetValueText(this.selectedDrill.TravelSpeed);
            this.DrillCuttingDepthValueButton.ValueText = this.GetValueText(this.selectedDrill.CuttingDepth);
            this.DrillCuttingIncrementValueButton.ValueText = this.GetValueText(this.selectedDrill.CuttingIncrement);
            this.DrillRetractModeToggleButton.OptionASelected = (false == this.selectedDrill.PositionRetract) ? true : false;
            this.DrillRetractDistanceValueButton.ValueText = this.GetValueText(this.selectedDrill.RetractDistance);
            this.DrillRetractPositionValueButton.ValueText = this.GetValueText(this.selectedDrill.RetractPosition);

            this.ServoProportionalControlConstantValueButton.ValueText = this.GetValueText(this.selectedDrill.ProportionalControlConstant);
            this.ServoIntegralControlConstantValueButton.ValueText = this.GetValueText(this.selectedDrill.IntegralControlConstant);
            this.ServoDerivativeControlConstantValueButton.ValueText = this.GetValueText(this.selectedDrill.DerivativeControlConstant);
            this.ServoErrorLimitValueButton.ValueText = this.GetValueText(this.selectedDrill.ErrorLimit);
            this.ServoAccelerationValueButton.ValueText = this.GetValueText(this.selectedDrill.Acceleration);
         }

         get { return (this.drillLocation); }
      }

      public DrillParameters FrontDrillParameters { set; get; }
      public DrillParameters RearDrillParameters { set; get; }

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

      private void DimBackground()
      {
         this.dimmerForm.Top = this.Top;
         this.dimmerForm.Left = this.Left;
         this.dimmerForm.Height = this.Height;
         this.dimmerForm.Width = this.Width;
         this.dimmerForm.Show();
      }

      private void LightBackground()
      {
         this.dimmerForm.Hide();
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

         this.DimBackground();
         DialogResult result = numberEntryForm.ShowDialog();
         this.LightBackground();

         if (result == System.Windows.Forms.DialogResult.OK)
         {
            valueParameter.OperationalValue = numberEntryForm.EnteredValue;
            button.ValueText = this.GetValueText(valueParameter);
         }

         return (result);
      }

      private void SetTitleText()
      {
         string locationDescription = (ToolLocations.front == this.DrillLocation) ? "FRONT" : "REAR";
         this.TitleLabel.Text = locationDescription + " DRILL SETUP";
      }

      #endregion

      #region User Event Handlers

      private void DrillSelectToggleButton_Click(object sender, EventArgs e)
      {
         this.DrillLocation = (ToolLocations.front == this.DrillLocation) ? ToolLocations.rear : ToolLocations.front;
         this.DrillSelectToggleButton.OptionASelected = (ToolLocations.front == this.DrillLocation);
         this.SetTitleText();
      }

      private void DrillModeToggleButton_Click(object sender, EventArgs e)
      {
         this.selectedDrill.PeckMode = !this.selectedDrill.PeckMode;
         this.DrillModeToggleButton.OptionASelected = (false == this.selectedDrill.PeckMode) ? true : false;
      }

      private void AutoOriginToggleButton_Click(object sender, EventArgs e)
      {
         this.selectedDrill.AutoOrign = !this.selectedDrill.AutoOrign;
         this.AutoOriginToggleButton.OptionASelected = this.selectedDrill.AutoOrign;
      }

      private void DrillSpeedValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, this.selectedDrill.RotationSpeed);
      }

      private void DrillCuttingDepthValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, this.selectedDrill.CuttingDepth);
      }

      private void DrillCuttingSpeedValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, this.selectedDrill.CuttingSpeed);
      }

      private void DrillTravelSpeedValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, this.selectedDrill.TravelSpeed);
      }

      private void DrillSearchSpeedValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, this.selectedDrill.SearchSpeed);
      }

      private void DrillCuttingIncrementValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, this.selectedDrill.CuttingIncrement);
      }

      private void DrillRetractModeToggleButton_Click(object sender, EventArgs e)
      {
         this.selectedDrill.PositionRetract = !this.selectedDrill.PositionRetract;
         this.DrillRetractModeToggleButton.OptionASelected = (false == this.selectedDrill.PositionRetract) ? true : false;
      }

      private void DrillRetractDistanceValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, this.selectedDrill.RetractDistance);
      }

      private void DrillRetractPositionValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, this.selectedDrill.RetractPosition);
      }

      private void ServoProportionalControlConstantValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, this.selectedDrill.ProportionalControlConstant);
      }

      private void ServoIntegralControlConstantValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, this.selectedDrill.IntegralControlConstant);
      }

      private void ServoDerivativeControlConstantValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, this.selectedDrill.DerivativeControlConstant);
      }

      private void ServoErrorLimitValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, this.selectedDrill.ErrorLimit);
      }

      private void ServoAccelerationValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, this.selectedDrill.Acceleration);
      }

      private void BackButton_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      #endregion

      #region Form Event Handlers

      private void DrillAutoSetupForm_Shown(object sender, EventArgs e)
      {
         if (null == this.FrontDrillParameters)
         {
            this.FrontDrillParameters = new DrillParameters();
         }

         if (null == this.RearDrillParameters)
         {
            this.RearDrillParameters = new DrillParameters();
         }

         if (ToolLocations.front == this.DrillLocation)
         {
            this.selectedDrill = this.FrontDrillParameters;
         }
         else
         {
            this.selectedDrill = this.RearDrillParameters;
         }

         this.SetTitleText();
         this.DrillSelectToggleButton.OptionASelected = (ToolLocations.front == this.DrillLocation);
      }

      #endregion

      #region Constructor

      public DrillSetupForm()
      {
         this.InitializeComponent();

         this.dimmerForm = new PopupDimmerForm();
         this.dimmerForm.Opacity = 0.65;
         this.dimmerForm.ShowInTaskbar = false;
      }

      #endregion

   }
}
