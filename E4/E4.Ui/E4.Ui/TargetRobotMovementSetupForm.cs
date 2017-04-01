using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace E4.Ui
{
   public partial class TargetRobotMovementSetupForm : Form
   {
      #region Fields

      private PopupDimmerForm dimmerForm;

      private bool mouseDown;
      private Point mouseDownPoint;

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

      private void FrontDirectionToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.FrontDirectionToggleButton.OptionBSelected;
         ParameterAccessor.Instance.TargetFrontWheel.RequestInverted = selectedOption;
         this.FrontDirectionToggleButton.OptionBSelected = selectedOption;
      }

      private void RearDirectionToggleButton_Click(object sender, EventArgs e)
      {
         bool selectedOption = !this.RearDirectionToggleButton.OptionBSelected;
         ParameterAccessor.Instance.TargetRearWheel.RequestInverted = selectedOption;
         this.RearDirectionToggleButton.OptionBSelected = selectedOption;
      }

      private void MaxSpeedValueButton_Click(object sender, EventArgs e)
      {
         Controls.ValueButton valueButton = (Controls.ValueButton)sender;
         this.LaunchNumberEdit(valueButton, ParameterAccessor.Instance.TargetWheelMaximumSpeed, "MAXIMUM SPEED");
      }

      private void LowSpeedScaleValueButton_Click(object sender, EventArgs e)
      {
         Controls.ValueButton valueButton = (Controls.ValueButton)sender;
         this.LaunchNumberEdit(valueButton, ParameterAccessor.Instance.TargetWheelLowSpeedScale);
      }

      private void BackButton_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      #endregion

      #region Form Events

      private void TargetRobotMovementSetupForm_Shown(object sender, EventArgs e)
      {
         this.FrontDirectionToggleButton.OptionBSelected = ParameterAccessor.Instance.TargetFrontWheel.RequestInverted;
         this.RearDirectionToggleButton.OptionBSelected = ParameterAccessor.Instance.TargetRearWheel.RequestInverted;

         this.MaxSpeedValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.TargetWheelMaximumSpeed);
         this.LowSpeedScaleValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.TargetWheelLowSpeedScale);
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

      public TargetRobotMovementSetupForm()
      {
         this.InitializeComponent();

         this.dimmerForm = new PopupDimmerForm();
         this.dimmerForm.Opacity = 0.65;
         this.dimmerForm.ShowInTaskbar = false;
      }

      #endregion

   }
}
