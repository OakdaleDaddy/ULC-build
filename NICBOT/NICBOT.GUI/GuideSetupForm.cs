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
   public partial class GuideSetupForm : Form
   {
      #region Properties

      public ValueParameter ExtensionSpeed { set; get; }
      public ValueParameter RetractionSpeed { set; get; }
      public bool MomentaryButtonAction { set; get; }

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

      #endregion

      #region User Events

      private void ExtensionSpeedValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, this.ExtensionSpeed);
      }

      private void RetractionSpeedValueButton_Click(object sender, EventArgs e)
      {
         ValueButton valueButton = (ValueButton)sender;
         this.LaunchNumberEdit(valueButton, this.RetractionSpeed);
      }

      private void ButtonActionToggleButton_Click(object sender, EventArgs e)
      {
         this.MomentaryButtonAction = !this.MomentaryButtonAction;
         this.ButtonActionToggleButton.OptionASelected = this.MomentaryButtonAction;
      }

      private void BackButton_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      #endregion

      #region Form Events

      private void GuildSetupForm_Shown(object sender, EventArgs e)
      {
         if (null == this.ExtensionSpeed)
         {
            this.ExtensionSpeed = new ValueParameter();
         }

         if (null == this.RetractionSpeed)
         {
            this.RetractionSpeed = new ValueParameter();
         }

         this.ExtensionSpeedValueButton.ValueText = this.GetValueText(this.ExtensionSpeed);
         this.RetractionSpeedValueButton.ValueText = this.GetValueText(this.RetractionSpeed);
         this.ButtonActionToggleButton.OptionASelected = this.MomentaryButtonAction;
      }

      #endregion

      #region Constructor

      public GuideSetupForm()
      {
         InitializeComponent();
      }

      #endregion

   }
}
