
namespace Weco.Ui
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

   public partial class LaserRobotMovementSetupForm : Form
   {
      #region Fields

      private PopupDimmerForm dimmerForm;

      private bool mouseDown;
      private Point mouseDownPoint;

      #endregion

      #region Helper Functions

      private int GetMotorSelectionValue(WheelMotorStates motorState)
      {
         int result = 0;

         if (WheelMotorStates.enabled == motorState)
         {
            result = 1;
         }
         else if (WheelMotorStates.disabled == motorState)
         {
            result = 2;
         }
         else if (WheelMotorStates.locked == motorState)
         {
            result = 3;
         }
         else
         {
            result = 0;
         }

         return (result);
      }

      private void SetMovementButtons(Controls.ValueCycleButton stateButton, Controls.ValueToggleButton directionButton, WheelMotorParameters parameters)
      {
         int motorStateOption = this.GetMotorSelectionValue(parameters.MotorState);

         stateButton.TimedSelection = false;
         stateButton.SelectedOption = motorStateOption;
         stateButton.TimedSelection = true;

         directionButton.OptionBSelected = parameters.RequestInverted;
      }

      private void SetMovementMotorState(WheelMotorParameters parameters, int selectedOption)
      {
         if (1 == selectedOption)
         {
            parameters.MotorState = WheelMotorStates.enabled;
         }
         else if (2 == selectedOption)
         {
            parameters.MotorState = WheelMotorStates.disabled;
         }
         else
         {
            parameters.MotorState = WheelMotorStates.locked;
         }
      }

      private void ShowMotorState(Controls.ValueCycleButton button, WheelMotorStates motorState, bool ignoreSelectionTimeout)
      {
         bool timedSelection;

         if ((false != button.SelectionTimedOut) || (false != ignoreSelectionTimeout))
         {
            timedSelection = button.TimedSelection;
            button.TimedSelection = false;
            button.SelectedOption = this.GetMotorSelectionValue(motorState);
            button.TimedSelection = timedSelection;
         }
      }

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

      private DialogResult LaunchNumberEdit(Controls.ValueButton button, ValueParameter valueParameter, string title = null)
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

      #endregion

      #region User Events

      private void FrontStateCycleButton_MouseClick(object sender, MouseEventArgs e)
      {
         int selectedOption = (this.FrontStateCycleButton.SelectedOption < 3) ? this.FrontStateCycleButton.SelectedOption + 1 : 1;
         this.FrontStateCycleButton.SelectedOption = selectedOption;
      }

      private void FrontStateCycleButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         if (false == this.FrontStateCycleButton.SelectionTimedOut)
         {
            int selectedOption = this.FrontStateCycleButton.SelectedOption;
            this.SetMovementMotorState(ParameterAccessor.Instance.LaserFrontWheel, selectedOption);
            this.ShowMotorState(this.FrontStateCycleButton, ParameterAccessor.Instance.LaserFrontWheel.MotorState, true);
            e.Handled = true;
         }
      }

      private void FrontStateCycleButton_SelectionTimeout(object sender, EventArgs e)
      {
         this.ShowMotorState(this.FrontStateCycleButton, ParameterAccessor.Instance.LaserFrontWheel.MotorState, false);
      }

      private void FrontDirectionToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.FrontDirectionToggleButton.OptionBSelected;
         ParameterAccessor.Instance.LaserFrontWheel.RequestInverted = selectedOption;
         this.FrontDirectionToggleButton.OptionBSelected = selectedOption;
      }

      private void RearStateCycleButton_MouseClick(object sender, MouseEventArgs e)
      {
         int selectedOption = (this.RearStateCycleButton.SelectedOption < 3) ? this.RearStateCycleButton.SelectedOption + 1 : 1;
         this.RearStateCycleButton.SelectedOption = selectedOption;
      }

      private void RearStateCycleButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         if (false == this.RearStateCycleButton.SelectionTimedOut)
         {
            int selectedOption = this.RearStateCycleButton.SelectedOption;
            this.SetMovementMotorState(ParameterAccessor.Instance.LaserRearWheel, selectedOption);
            this.ShowMotorState(this.RearStateCycleButton, ParameterAccessor.Instance.LaserRearWheel.MotorState, true);
            e.Handled = true;
         }
      }

      private void RearStateCycleButton_SelectionTimeout(object sender, EventArgs e)
      {
         this.ShowMotorState(this.RearStateCycleButton, ParameterAccessor.Instance.LaserRearWheel.MotorState, false);
      }

      private void RearDirectionToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.RearDirectionToggleButton.OptionBSelected;
         ParameterAccessor.Instance.LaserRearWheel.RequestInverted = selectedOption;
         this.RearDirectionToggleButton.OptionBSelected = selectedOption;
      }

      private void MaxSpeedValueButton_Click(object sender, EventArgs e)
      {
         Controls.ValueButton valueButton = (Controls.ValueButton)sender;
         this.LaunchNumberEdit(valueButton, ParameterAccessor.Instance.LaserWheelMaximumSpeed, "MAXIMUM SPEED");
      }

      private void LowSpeedScaleValueButton_Click(object sender, EventArgs e)
      {
         Controls.ValueButton valueButton = (Controls.ValueButton)sender;
         this.LaunchNumberEdit(valueButton, ParameterAccessor.Instance.LaserWheelLowSpeedScale);
      }

      private void ActuationModeButton_Click(object sender, EventArgs e)
      {
         bool selected = !this.ActuationModeButton.OptionASelected;
         this.ActuationModeButton.OptionASelected = selected;

         ParameterAccessor.Instance.LaserFrontWheel.ActuationMode = (false != selected) ? ActuationModes.closedloop : ActuationModes.openloop;
         ParameterAccessor.Instance.LaserRearWheel.ActuationMode = (false != selected) ? ActuationModes.closedloop : ActuationModes.openloop;
      }

      private void BackButton_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      #endregion

      #region Form Events

      private void LaserRobotMovementSetupForm_Shown(object sender, EventArgs e)
      {
         this.SetMovementButtons(this.FrontStateCycleButton, this.FrontDirectionToggleButton, ParameterAccessor.Instance.LaserFrontWheel);
         this.SetMovementButtons(this.RearStateCycleButton, this.RearDirectionToggleButton, ParameterAccessor.Instance.LaserRearWheel);

         this.MaxSpeedValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.LaserWheelMaximumSpeed);
         this.LowSpeedScaleValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.LaserWheelLowSpeedScale);
         this.ActuationModeButton.OptionASelected = (ActuationModes.closedloop == ParameterAccessor.Instance.LaserFrontWheel.ActuationMode) ? true : false;
      }

      private void TitleLabel_MouseDown(object sender, MouseEventArgs e)
      {
         this.mouseDownPoint = e.Location;
         this.mouseDown = true;
      }

      private void TitleLabel_MouseUp(object sender, MouseEventArgs e)
      {
         this.mouseDown = false;
      }

      private void TitleLabel_MouseMove(object sender, MouseEventArgs e)
      {
         if (false != this.mouseDown)
         {
            this.Top += (e.Y - mouseDownPoint.Y);
            this.Left += (e.X - mouseDownPoint.X);
         }
      }

      #endregion

      #region Constructor

      public LaserRobotMovementSetupForm()
      {
         this.InitializeComponent();

         this.dimmerForm = new PopupDimmerForm();
         this.dimmerForm.Opacity = 0.65;
         this.dimmerForm.ShowInTaskbar = false;
      }

      #endregion

   }
}
