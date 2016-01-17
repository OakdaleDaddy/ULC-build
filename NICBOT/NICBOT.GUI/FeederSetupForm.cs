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
   public partial class FeederSetupForm : Form
   {
      #region Fields

      private PopupDimmerForm dimmerForm;

      #endregion

      #region Helper Functions

      private string GetValueText(ValueParameter parameter)
      {
         string signString = "";

         if (parameter.MinimumValue < 0)
         {
            if (parameter.OperationalValue > 0)
            {
               signString = "+";
            }
         }

         string doubleFormat = "N" + parameter.Precision.ToString();
         string result = signString + parameter.OperationalValue.ToString(doubleFormat) + " " + parameter.Unit;
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

      private void SetFeederButtons(ValueCycleButton stateButton, ValueToggleButton directionButton, FeederMotorParameters parameters)
      {         
         int stateOption = 0;

         if (MotorStates.Enabled == parameters.State)
         {
            stateOption = 1;
         }
         else if (MotorStates.Disabled == parameters.State)
         {
            stateOption = 2;
         }
         else if (MotorStates.Locked == parameters.State)
         {
            stateOption = 3;
         }

         stateButton.SelectedOption = stateOption;


         bool directionOption = false;

         if (MotorDirections.Normal == parameters.Direction)
         {
            directionOption = true;
         }
         else 
         {
            directionOption = false;
         }

         directionButton.OptionASelected = directionOption;
      }

      private void SetFeederMotorState(FeederMotorParameters parameters, int selectedOption)
      {
         if (1 == selectedOption)
         {
            parameters.State = MotorStates.Enabled;
         }
         else if (2 == selectedOption)
         {
            parameters.State = MotorStates.Disabled;
         }
         else
         {
            parameters.State = MotorStates.Locked;
         }
      }

      private void SetFeederMotorDirection(FeederMotorParameters parameters, bool selectedOption)
      {
         if (false != selectedOption)
         {
            parameters.Direction = MotorDirections.Normal;
         }
         else
         {
            parameters.Direction = MotorDirections.Inverse;
         }
      }

      #endregion

      #region User Events

      private void TopFrontStateCycleButton_Click(object sender, EventArgs e)
      {
         int selectedOption = (this.TopFrontStateCycleButton.SelectedOption < 3) ? this.TopFrontStateCycleButton.SelectedOption + 1 : 1;
         this.SetFeederMotorState(ParameterAccessor.Instance.TopFrontFeederMotor, selectedOption);
         this.TopFrontStateCycleButton.SelectedOption = selectedOption;
      }

      private void TopFrontDirectionToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.TopFrontDirectionToggleButton.OptionASelected;
         this.SetFeederMotorDirection(ParameterAccessor.Instance.TopFrontFeederMotor, selectedOption);
         this.TopFrontDirectionToggleButton.OptionASelected = selectedOption;
      }

      private void TopRearStateCycleButton_Click(object sender, EventArgs e)
      {
         int selectedOption = (this.TopRearStateCycleButton.SelectedOption < 3) ? this.TopRearStateCycleButton.SelectedOption + 1 : 1;
         this.SetFeederMotorState(ParameterAccessor.Instance.TopRearFeederMotor, selectedOption);
         this.TopRearStateCycleButton.SelectedOption = selectedOption;
      }

      private void TopRearDirectionToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.TopRearDirectionToggleButton.OptionASelected;
         this.SetFeederMotorDirection(ParameterAccessor.Instance.TopRearFeederMotor, selectedOption);
         this.TopRearDirectionToggleButton.OptionASelected = selectedOption;
      }

      private void BottomFrontStateCycleButton_Click(object sender, EventArgs e)
      {
         int selectedOption = (this.BottomFrontStateCycleButton.SelectedOption < 3) ? this.BottomFrontStateCycleButton.SelectedOption + 1 : 1;
         this.SetFeederMotorState(ParameterAccessor.Instance.BottomFrontFeederMotor, selectedOption);
         this.BottomFrontStateCycleButton.SelectedOption = selectedOption;
      }

      private void BottomFrontDirectionToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.BottomFrontDirectionToggleButton.OptionASelected;
         this.SetFeederMotorDirection(ParameterAccessor.Instance.BottomFrontFeederMotor, selectedOption);
         this.BottomFrontDirectionToggleButton.OptionASelected = selectedOption;
      }

      private void BottomRearStateCycleButton_Click(object sender, EventArgs e)
      {
         int selectedOption = (this.BottomRearStateCycleButton.SelectedOption < 3) ? this.BottomRearStateCycleButton.SelectedOption + 1 : 1;
         this.SetFeederMotorState(ParameterAccessor.Instance.BottomRearFeederMotor, selectedOption);
         this.BottomRearStateCycleButton.SelectedOption = selectedOption;
      }

      private void BottomRearDirectionToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.BottomRearDirectionToggleButton.OptionASelected;
         this.SetFeederMotorDirection(ParameterAccessor.Instance.BottomRearFeederMotor, selectedOption);
         this.BottomRearDirectionToggleButton.OptionASelected = selectedOption;
      }

      private void SpeedTrackingToggleButton_Click(object sender, EventArgs e)
      {
         ParameterAccessor.Instance.FeederAutomaticTracking = !ParameterAccessor.Instance.FeederAutomaticTracking;
         this.SpeedTrackingToggleButton.OptionASelected = ParameterAccessor.Instance.FeederAutomaticTracking;
      }

      private void TrackingCalibrationValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, ParameterAccessor.Instance.FeederTrackingCalibration);
      }

      private void LowSpeedScaleValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, ParameterAccessor.Instance.FeederLowSpeedScale);
      }

      private void CurrentPer1KValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, ParameterAccessor.Instance.FeederCurrentPer1kRPM);
      }

      private void LockCurrentValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, ParameterAccessor.Instance.FeederLockCurrent);
      }

      private void MaxSpeedValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, ParameterAccessor.Instance.FeederMaxSpeed);
      }

      private void BackButton_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      #endregion

      #region Form Events

      private void FeederSetupForm_Shown(object sender, EventArgs e)
      {
         this.SetFeederButtons(this.TopFrontStateCycleButton, this.TopFrontDirectionToggleButton, ParameterAccessor.Instance.TopFrontFeederMotor);
         this.SetFeederButtons(this.TopRearStateCycleButton, this.TopRearDirectionToggleButton, ParameterAccessor.Instance.TopRearFeederMotor);
         this.SetFeederButtons(this.BottomFrontStateCycleButton, this.BottomFrontDirectionToggleButton, ParameterAccessor.Instance.BottomFrontFeederMotor);
         this.SetFeederButtons(this.BottomRearStateCycleButton, this.BottomRearDirectionToggleButton, ParameterAccessor.Instance.BottomRearFeederMotor);

         this.SpeedTrackingToggleButton.OptionASelected = ParameterAccessor.Instance.FeederAutomaticTracking;
         this.TrackingCalibrationValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.FeederTrackingCalibration);
         this.LowSpeedScaleValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.FeederLowSpeedScale);
         this.CurrentPer1KValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.FeederCurrentPer1kRPM);
         this.LockCurrentValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.FeederLockCurrent);
         this.MaxSpeedValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.FeederMaxSpeed);
      }

      #endregion

      #region Constructor

      public FeederSetupForm()
      {
         this.InitializeComponent();

         this.dimmerForm = new PopupDimmerForm();
         this.dimmerForm.Opacity = 0.65;
         this.dimmerForm.ShowInTaskbar = false;
      }

      #endregion

   }
}
