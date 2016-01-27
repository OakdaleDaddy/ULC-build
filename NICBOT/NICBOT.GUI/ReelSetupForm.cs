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
   public partial class ReelSetupForm : Form
   {
      #region Fields

      private PopupDimmerForm dimmerForm;

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

      #endregion

      #region User Events

      private void MotionControllModeToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.MotionControllModeToggleButton.OptionASelected;
         ParameterAccessor.Instance.ReelMotionMode = (false != selectedOption) ? MovementForwardControls.velocity : MovementForwardControls.current; 
         this.MotionControllModeToggleButton.OptionASelected = selectedOption;
      }

      private void ReverseCurrentValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, ParameterAccessor.Instance.ReelReverseCurrent);
         this.ReverseCurrentValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.ReelReverseCurrent);
      }

      private void ReverseSpeedValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, ParameterAccessor.Instance.ReelReverseSpeed);
         this.ReverseSpeedValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.ReelReverseSpeed);
      }      

      private void LockCurrentValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, ParameterAccessor.Instance.ReelLockCurrent);
         this.LockCurrentValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.ReelLockCurrent);
      }

      private void CalibrationDistanceValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, ParameterAccessor.Instance.ReelCalibrationDistance);
         this.CalibrationDistanceValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.ReelCalibrationDistance);
      }

      private void BackButton_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      #endregion

      #region Form Events

      private void TetherSetupForm_Shown(object sender, EventArgs e)
      {
         bool axialOption;

         if (MovementForwardControls.velocity == ParameterAccessor.Instance.ReelMotionMode)
         {
            axialOption = true;
         }
         else
         {
            axialOption = false;
         }

         this.MotionControllModeToggleButton.OptionASelected = axialOption;

         this.ReverseCurrentValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.ReelReverseCurrent);
         this.ReverseSpeedValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.ReelReverseSpeed);
         this.LockCurrentValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.ReelLockCurrent);
         this.CalibrationDistanceValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.ReelCalibrationDistance);
      }

      #endregion

      #region Constructor

      public ReelSetupForm()
      {
         this.InitializeComponent();
         
         this.dimmerForm = new PopupDimmerForm();
         this.dimmerForm.Opacity = 0.65;
         this.dimmerForm.ShowInTaskbar = false;
      }

      #endregion

   }
}
