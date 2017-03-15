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
   public partial class SystemStatusForm : Form
   {
      #region Definitions

      public delegate void SystemResetDelegate();
      public delegate void TraceListenerDestinationDelegate();
      
      #endregion

      #region Fields

      private PopupDimmerForm dimmerForm;

      private bool mouseDown;
      private Point mouseDownPoint;

      #endregion

      #region Helper Functions

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

      #endregion

      #region Properties

      public SystemResetDelegate SystemResetHandler { set; get; }
      public TraceListenerDestinationDelegate TraceListenerDestination { set; get; }

      #endregion

      #region User Events

      private void TriggerDefaultsButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         ParameterAccessor.Instance.TriggerDefaults();

         MessageForm messageForm = new MessageForm();
         messageForm.Title = "SYSTEM DEFAULTS";
         messageForm.Message = "DEFAULTS ASSIGNED ON THE NEXT SYSTEM START";

         this.SetDialogLocation(this.TriggerDefaultsButton, messageForm);

         this.DimBackground();
         messageForm.ShowDialog();
         this.LightBackground();
      }
      
      private void SaveDefaultsButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         ParameterAccessor.Instance.SaveDefaults();

         MessageForm messageForm = new MessageForm();
         messageForm.Title = "SYSTEM DEFAULTS";
         messageForm.Message = "CURRENT SETTINGS ASSIGNED AS DEFAULTS";

         this.SetDialogLocation(this.SaveDefaultsButton, messageForm);

         this.DimBackground();
         messageForm.ShowDialog();
         this.LightBackground();
      }

      private void ComponentStatusLabel_MouseDown(object sender, MouseEventArgs e)
      {
         this.mouseDownPoint = e.Location;
         this.mouseDown = true;
      }

      private void ComponentStatusLabel_MouseUp(object sender, MouseEventArgs e)
      {
         this.mouseDown = false;
      }

      private void ComponentStatusLabel_MouseMove(object sender, MouseEventArgs e)
      {
         if (false != this.mouseDown)
         {
            this.Top += (e.Y - mouseDownPoint.Y);
            this.Left += (e.X - mouseDownPoint.X);
         }
      }

      private void BackButton_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      #endregion

      #region Constructor

      public SystemStatusForm()
      {
         this.InitializeComponent();

         this.dimmerForm = new PopupDimmerForm();
         this.dimmerForm.Opacity = 0.65;
         this.dimmerForm.ShowInTaskbar = false;
      }

      #endregion

   }
}
