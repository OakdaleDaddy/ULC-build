﻿using System;
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
   public partial class LightIntensitySelectForm : Form
   {
      public string LocationText { set; get; }
      public ValueParameter IntensityValue { set; get; }
      public CameraLocations Camera { set; get; }

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

      private DialogResult LaunchNumberEdit(Control control, string title)
      {         
         NumberEntryForm numberEntryForm = new NumberEntryForm();
         this.SetDialogLocation(control, numberEntryForm);

         numberEntryForm.Title = title;
         numberEntryForm.Unit = this.IntensityValue.Unit;
         numberEntryForm.PostDecimalDigitCount = this.IntensityValue.Precision;
         numberEntryForm.PresentValue = this.IntensityValue.OperationalValue;
         numberEntryForm.DefaultValue = this.IntensityValue.DefaultValue;
         numberEntryForm.MinimumValue = this.IntensityValue.MinimumValue;
         numberEntryForm.MaximumValue = this.IntensityValue.MaximumValue;

         DialogResult result = numberEntryForm.ShowDialog();

         if (System.Windows.Forms.DialogResult.OK == result)
         {
            this.IntensityValue.OperationalValue = numberEntryForm.EnteredValue;
         }

         return (result);
      }

      #endregion

      #region User Event Process

      private void UpButton_HoldTimeout(object sender, EventArgs e)
      {
         if (this.IntensityValue.OperationalValue < this.IntensityValue.MaximumValue)
         {
            this.IntensityValue.OperationalValue++;
            this.IntensityProgressBar.Value = (int)this.IntensityValue.OperationalValue;
            NicBotComm.Instance.SetCameraLightLevel(this.Camera, (int)this.IntensityValue.OperationalValue);
         }
      }

      private void DownButton_HoldTimeout(object sender, EventArgs e)
      {
         if (this.IntensityValue.OperationalValue > this.IntensityValue.MinimumValue)
         {
            this.IntensityValue.OperationalValue--;
            this.IntensityProgressBar.Value = (int)this.IntensityValue.OperationalValue;
            NicBotComm.Instance.SetCameraLightLevel(this.Camera, (int)this.IntensityValue.OperationalValue);
         }
      }

      private void UpButton_Click(object sender, EventArgs e)
      {
         if (this.IntensityValue.OperationalValue < this.IntensityValue.MaximumValue)
         {
            this.IntensityValue.OperationalValue++;
            this.IntensityProgressBar.Value = (int)this.IntensityValue.OperationalValue;
            NicBotComm.Instance.SetCameraLightLevel(this.Camera, (int)this.IntensityValue.OperationalValue);
         }
      }

      private void DownButton_Click(object sender, EventArgs e)
      {
         if (this.IntensityValue.OperationalValue > this.IntensityValue.MinimumValue)
         {
            this.IntensityValue.OperationalValue--;
            this.IntensityProgressBar.Value = (int)this.IntensityValue.OperationalValue;
            NicBotComm.Instance.SetCameraLightLevel(this.Camera, (int)this.IntensityValue.OperationalValue);
         }
      }

      private void IntensityProgressBar_MouseDown(object sender, MouseEventArgs e)
      {
         this.HoldTimer.Start();
      }

      private void IntensityProgressBar_MouseUp(object sender, MouseEventArgs e)
      {
         this.HoldTimer.Stop();
      }

      private void IntensityProgressBar_MouseLeave(object sender, EventArgs e)
      {
         this.HoldTimer.Stop();
      }

      private void BackButton_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      #endregion

      #region Form Event Process 

      private void LightIntensitySelectForm_Paint(object sender, PaintEventArgs e)
      {
         e.Graphics.DrawRectangle(new Pen(Color.Black, 3), 1, 1, this.ClientRectangle.Width - 3, this.ClientRectangle.Height - 3);
      }

      private void LightIntensitySelectForm_Shown(object sender, EventArgs e)
      {
         this.LocationLabel.Text = this.LocationText;

         this.IntensityProgressBar.Minimum = (int)this.IntensityValue.MinimumValue;
         this.IntensityProgressBar.Maximum = (int)this.IntensityValue.MaximumValue;
         this.IntensityProgressBar.Value = (int)this.IntensityValue.OperationalValue;
      }

      private void HoldTimer_Tick(object sender, EventArgs e)
      {
         DialogResult result = this.LaunchNumberEdit(this.IntensityProgressBar, this.LocationText + " LIGHT ADJUST");

         if (result == System.Windows.Forms.DialogResult.OK)
         {
            this.IntensityProgressBar.Value = (int)this.IntensityValue.OperationalValue;
            NicBotComm.Instance.SetCameraLightLevel(this.Camera, (int)this.IntensityValue.OperationalValue);
         }
      }

      #endregion

      #region Constructor

      public LightIntensitySelectForm()
      {
         this.InitializeComponent();
      }

      #endregion


   }
}
