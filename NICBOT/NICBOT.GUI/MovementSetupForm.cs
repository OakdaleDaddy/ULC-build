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
   public partial class MovementSetupForm : Form
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

      private DialogResult LaunchNumberEdit(ValueButton button, ValueParameter valueParameter, string title = null)
      {
         NumberEntryForm numberEntryForm = new NumberEntryForm();
         this.SetDialogLocation(button, numberEntryForm);

         numberEntryForm.Title = (null != title) ? title : button.Text;
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

      private void SetMovementButtons(ValueCycleButton stateButton, ValueToggleButton directionButton, ValueToggleButton axialButton, ValueToggleButton circumferentialButton, ValueToggleButton cornerAxialButton, ValueToggleButton launchAxialButton, MovementMotorParameters parameters)
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


         bool axialOption = false;

         if (MovementForwardControls.velocity == parameters.AxialMode)
         {
            axialOption = true;
         }
         else
         {
            axialOption = false;
         }

         axialButton.OptionASelected = axialOption;


         bool circumferentialOption = false;

         if (MovementForwardControls.velocity == parameters.CircumferentialMode)
         {
            circumferentialOption = true;
         }
         else
         {
            circumferentialOption = false;
         }

         circumferentialButton.OptionASelected = circumferentialOption;


         bool cornerAxialOption = false;

         if (MovementForwardControls.velocity == parameters.CornerAxialMode)
         {
            cornerAxialOption = true;
         }
         else
         {
            cornerAxialOption = false;
         }

         cornerAxialButton.OptionASelected = cornerAxialOption;


         bool launchAxialOption = false;

         if (MovementForwardControls.velocity == parameters.LaunchAxialMode)
         {
            launchAxialOption = true;
         }
         else
         {
            launchAxialOption = false;
         }

         launchAxialButton.OptionASelected = launchAxialOption;
      }

      private void SetMovementMotorState(MovementMotorParameters parameters, int selectedOption)
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

      private void SetMovementMotorDirection(MovementMotorParameters parameters, bool selectedOption)
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

      private void SetMovementMotorAxialMode(MovementMotorParameters parameters, bool selectedOption)
      {
         if (false != selectedOption)
         {
            parameters.AxialMode = MovementForwardControls.velocity;
         }
         else
         {
            parameters.AxialMode = MovementForwardControls.current;
         }
      }

      private void SetMovementMotorCircumferentialMode(MovementMotorParameters parameters, bool selectedOption)
      {
         if (false != selectedOption)
         {
            parameters.CircumferentialMode = MovementForwardControls.velocity;
         }
         else
         {
            parameters.CircumferentialMode = MovementForwardControls.current;
         }
      }

      private void SetMovementMotorCornerAxialMode(MovementMotorParameters parameters, bool selectedOption)
      {
         if (false != selectedOption)
         {
            parameters.CornerAxialMode = MovementForwardControls.velocity;
         }
         else
         {
            parameters.CornerAxialMode = MovementForwardControls.current;
         }
      }

      private void SetMovementMotorLaunchAxialMode(MovementMotorParameters parameters, bool selectedOption)
      {
         if (false != selectedOption)
         {
            parameters.LaunchAxialMode = MovementForwardControls.velocity;
         }
         else
         {
            parameters.LaunchAxialMode = MovementForwardControls.current;
         }
      }
      
      #endregion

      #region User Events

      #region Top Front Motor

      private void TopFrontStateCycleButton_Click(object sender, EventArgs e)
      {
         int selectedOption = (this.TopFrontStateCycleButton.SelectedOption < 3) ? this.TopFrontStateCycleButton.SelectedOption + 1 : 1;
         this.SetMovementMotorState(ParameterAccessor.Instance.TopFrontMovementMotor, selectedOption);
         this.TopFrontStateCycleButton.SelectedOption = selectedOption;
      }

      private void TopFrontDirectionToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.TopFrontDirectionToggleButton.OptionASelected;
         this.SetMovementMotorDirection(ParameterAccessor.Instance.TopFrontMovementMotor, selectedOption);
         this.TopFrontDirectionToggleButton.OptionASelected = selectedOption;
      }

      private void TopFrontNormalAxialModeToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.TopFrontNormalAxialModeToggleButton.OptionASelected;
         this.SetMovementMotorAxialMode(ParameterAccessor.Instance.TopFrontMovementMotor, selectedOption);
         this.TopFrontNormalAxialModeToggleButton.OptionASelected = selectedOption;
      }

      private void TopFrontCircumferentialToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.TopFrontCircumferentialToggleButton.OptionASelected;
         this.SetMovementMotorCircumferentialMode(ParameterAccessor.Instance.TopFrontMovementMotor, selectedOption);
         this.TopFrontCircumferentialToggleButton.OptionASelected = selectedOption;
      }

      private void TopFrontCornerAxialToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.TopFrontCornerAxialToggleButton.OptionASelected;
         this.SetMovementMotorCornerAxialMode(ParameterAccessor.Instance.TopFrontMovementMotor, selectedOption);
         this.TopFrontCornerAxialToggleButton.OptionASelected = selectedOption;
      }

      private void TopFrontLaunchAxialToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.TopFrontLaunchAxialToggleButton.OptionASelected;
         this.SetMovementMotorLaunchAxialMode(ParameterAccessor.Instance.TopFrontMovementMotor, selectedOption);
         this.TopFrontLaunchAxialToggleButton.OptionASelected = selectedOption;
      }

      #endregion

      #region Top Rear Motor

      private void TopRearStateCycleButton_Click(object sender, EventArgs e)
      {
         int selectedOption = (this.TopRearStateCycleButton.SelectedOption < 3) ? this.TopRearStateCycleButton.SelectedOption + 1 : 1;
         this.SetMovementMotorState(ParameterAccessor.Instance.TopRearMovementMotor, selectedOption);
         this.TopRearStateCycleButton.SelectedOption = selectedOption;
      }

      private void TopRearDirectionToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.TopRearDirectionToggleButton.OptionASelected;
         this.SetMovementMotorDirection(ParameterAccessor.Instance.TopRearMovementMotor, selectedOption);
         this.TopRearDirectionToggleButton.OptionASelected = selectedOption;
      }

      private void TopRearNormalAxialModeToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.TopRearNormalAxialModeToggleButton.OptionASelected;
         this.SetMovementMotorAxialMode(ParameterAccessor.Instance.TopRearMovementMotor, selectedOption);
         this.TopRearNormalAxialModeToggleButton.OptionASelected = selectedOption;
      }

      private void TopRearCircumferentialToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.TopRearCircumferentialToggleButton.OptionASelected;
         this.SetMovementMotorCircumferentialMode(ParameterAccessor.Instance.TopRearMovementMotor, selectedOption);
         this.TopRearCircumferentialToggleButton.OptionASelected = selectedOption;
      }

      private void TopRearCornerAxialToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.TopRearCornerAxialToggleButton.OptionASelected;
         this.SetMovementMotorCornerAxialMode(ParameterAccessor.Instance.TopRearMovementMotor, selectedOption);
         this.TopRearCornerAxialToggleButton.OptionASelected = selectedOption;
      }

      private void TopRearLaunchAxialToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.TopRearLaunchAxialToggleButton.OptionASelected;
         this.SetMovementMotorLaunchAxialMode(ParameterAccessor.Instance.TopRearMovementMotor, selectedOption);
         this.TopRearLaunchAxialToggleButton.OptionASelected = selectedOption;
      }

      #endregion

      #region Bottom Front Motor

      private void BottomFrontStateCycleButton_Click(object sender, EventArgs e)
      {
         int selectedOption = (this.BottomFrontStateCycleButton.SelectedOption < 3) ? this.BottomFrontStateCycleButton.SelectedOption + 1 : 1;
         this.SetMovementMotorState(ParameterAccessor.Instance.BottomFrontMovementMotor, selectedOption);
         this.BottomFrontStateCycleButton.SelectedOption = selectedOption;
      }

      private void BottomFrontDirectionToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.BottomFrontDirectionToggleButton.OptionASelected;
         this.SetMovementMotorDirection(ParameterAccessor.Instance.BottomFrontMovementMotor, selectedOption);
         this.BottomFrontDirectionToggleButton.OptionASelected = selectedOption;
      }

      private void BottomFrontNormalAxialModeToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.BottomFrontNormalAxialModeToggleButton.OptionASelected;
         this.SetMovementMotorAxialMode(ParameterAccessor.Instance.BottomFrontMovementMotor, selectedOption);
         this.BottomFrontNormalAxialModeToggleButton.OptionASelected = selectedOption;
      }

      private void BottomFrontCircumferentialToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.BottomFrontCircumferentialToggleButton.OptionASelected;
         this.SetMovementMotorCircumferentialMode(ParameterAccessor.Instance.BottomFrontMovementMotor, selectedOption);
         this.BottomFrontCircumferentialToggleButton.OptionASelected = selectedOption;
      }

      private void BottomFrontCornerAxialToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.BottomFrontCornerAxialToggleButton.OptionASelected;
         this.SetMovementMotorCornerAxialMode(ParameterAccessor.Instance.BottomFrontMovementMotor, selectedOption);
         this.BottomFrontCornerAxialToggleButton.OptionASelected = selectedOption;
      }

      private void BottomFrontLaunchAxialToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.BottomFrontLaunchAxialToggleButton.OptionASelected;
         this.SetMovementMotorLaunchAxialMode(ParameterAccessor.Instance.BottomFrontMovementMotor, selectedOption);
         this.BottomFrontLaunchAxialToggleButton.OptionASelected = selectedOption;
      }

      #endregion

      #region Bottom Rear Motor

      private void BottomRearStateCycleButton_Click(object sender, EventArgs e)
      {
         int selectedOption = (this.BottomRearStateCycleButton.SelectedOption < 3) ? this.BottomRearStateCycleButton.SelectedOption + 1 : 1;
         this.SetMovementMotorState(ParameterAccessor.Instance.BottomRearMovementMotor, selectedOption);
         this.BottomRearStateCycleButton.SelectedOption = selectedOption;
      }

      private void BottomRearDirectionToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.BottomRearDirectionToggleButton.OptionASelected;
         this.SetMovementMotorDirection(ParameterAccessor.Instance.BottomRearMovementMotor, selectedOption);
         this.BottomRearDirectionToggleButton.OptionASelected = selectedOption;
      }

      private void BottomRearNormalAxialModeToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.BottomRearNormalAxialModeToggleButton.OptionASelected;
         this.SetMovementMotorAxialMode(ParameterAccessor.Instance.BottomRearMovementMotor, selectedOption);
         this.BottomRearNormalAxialModeToggleButton.OptionASelected = selectedOption;
      }

      private void BottomRearCircumferentialToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.BottomRearCircumferentialToggleButton.OptionASelected;
         this.SetMovementMotorCircumferentialMode(ParameterAccessor.Instance.BottomRearMovementMotor, selectedOption);
         this.BottomRearCircumferentialToggleButton.OptionASelected = selectedOption;
      }

      private void BottomRearCornerAxialToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.BottomRearCornerAxialToggleButton.OptionASelected;
         this.SetMovementMotorCornerAxialMode(ParameterAccessor.Instance.BottomRearMovementMotor, selectedOption);
         this.BottomRearCornerAxialToggleButton.OptionASelected = selectedOption;
      }

      private void BottomRearLaunchAxialToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.BottomRearLaunchAxialToggleButton.OptionASelected;
         this.SetMovementMotorLaunchAxialMode(ParameterAccessor.Instance.BottomRearMovementMotor, selectedOption);
         this.BottomRearLaunchAxialToggleButton.OptionASelected = selectedOption;
      }

      #endregion

      private void LockCurrentValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, ParameterAccessor.Instance.MovementMotorLockCurrent, "MOTOR LOCK CURRENT");
      }

      private void MaxCurrentValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, ParameterAccessor.Instance.MovementMotorMaxCurrent, "MOTOR MAXIMUM CURRENT");
      }

      private void MaxSpeedValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, ParameterAccessor.Instance.MovementMotorMaxSpeed, "MOTOR MAXIMUM SPEED");
      }

      private void CurrentPer1KValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, ParameterAccessor.Instance.MovementMotorCurrentPer1kRPM);
      }

      private void LowSpeedScaleValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, ParameterAccessor.Instance.MovementMotorLowSpeedScale);
      }

      private void BackButton_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      #endregion

      #region Form Events

      private void MotorSetupForm_Shown(object sender, EventArgs e)
      {
         this.SetMovementButtons(this.TopFrontStateCycleButton, this.TopFrontDirectionToggleButton, this.TopFrontNormalAxialModeToggleButton, this.TopFrontCircumferentialToggleButton, this.TopFrontCornerAxialToggleButton, this.TopFrontLaunchAxialToggleButton, ParameterAccessor.Instance.TopFrontMovementMotor);
         this.SetMovementButtons(this.TopRearStateCycleButton, this.TopRearDirectionToggleButton, this.TopRearNormalAxialModeToggleButton, this.TopRearCircumferentialToggleButton, this.TopRearCornerAxialToggleButton, this.TopRearLaunchAxialToggleButton, ParameterAccessor.Instance.TopRearMovementMotor);
         this.SetMovementButtons(this.BottomFrontStateCycleButton, this.BottomFrontDirectionToggleButton, this.BottomFrontNormalAxialModeToggleButton, this.BottomFrontCircumferentialToggleButton, this.BottomFrontCornerAxialToggleButton, this.BottomFrontLaunchAxialToggleButton, ParameterAccessor.Instance.BottomFrontMovementMotor);
         this.SetMovementButtons(this.BottomRearStateCycleButton, this.BottomRearDirectionToggleButton, this.BottomRearNormalAxialModeToggleButton, this.BottomRearCircumferentialToggleButton, this.BottomRearCornerAxialToggleButton, this.BottomRearLaunchAxialToggleButton, ParameterAccessor.Instance.BottomRearMovementMotor);

         this.LockCurrentValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.MovementMotorLockCurrent);
         this.MaxCurrentValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.MovementMotorMaxCurrent);
         this.MaxSpeedValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.MovementMotorMaxSpeed);
         this.CurrentPer1KValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.MovementMotorCurrentPer1kRPM);
         this.LowSpeedScaleValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.MovementMotorLowSpeedScale);
      }

      #endregion

      #region Constructor

      public MovementSetupForm()
      {
         this.InitializeComponent();

         this.dimmerForm = new PopupDimmerForm();
         this.dimmerForm.Opacity = 0.65;
         this.dimmerForm.ShowInTaskbar = false;
      }

      #endregion

   }
}
