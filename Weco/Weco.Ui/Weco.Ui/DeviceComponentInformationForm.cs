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

   using Weco.CAN;

   public partial class DeviceComponentInformationForm : Form
   {
      #region Fields

      private bool mouseDown;
      private Point mouseDownPoint;

      public int componentErrorCount;
      public int componentErrorIndex;

      #endregion

      #region Properties

      public string Title { set; get; }
      public CAN.DeviceComponent Component { set; get; }
      public Enum DeviceId { set; get; }
      public DeviceClearErrorRequest.ClearErrorHandler OnDeviceClearError { set; get; }

      #endregion

      #region Helper Functions

      private void ShowDeviceStatus()
      {
         if (null != this.Component)
         {
            if (null != this.Component.DeviceFaultReason)
            {
               this.FaultTextBox.Text = this.Component.DeviceFaultReason;
               this.FaultTextBox.BackColor = Color.Red;

               this.ErrorTextBox.Text = "device offline";
               this.ErrorTextBox.BackColor = Color.Yellow;
               this.ErrorIndexLabel.Visible = false;

               this.ClearFaultButton.Enabled = false;
               this.ClearErrorButton.Enabled = false;
               this.ErrorUpButton.Enabled = false;
               this.ErrorDownButton.Enabled = false;

            }
            else
            {
               if (null != this.Component.FaultRecord)
               {
                  this.FaultTextBox.Text = this.Component.FaultRecord.Description;
                  this.FaultTextBox.BackColor = Color.Red;
                  this.ClearFaultButton.Enabled = true;
               }
               else
               {
                  this.FaultTextBox.Text = "clear";
                  this.FaultTextBox.BackColor = Color.Lime;
                  this.ClearFaultButton.Enabled = false;
               }

               int errorCount = this.Component.ErrorList.Count;

               if (this.componentErrorCount != errorCount)
               {
                  this.componentErrorCount = errorCount;

                  if (this.componentErrorIndex < 0)
                  {
                     this.componentErrorIndex = 0;
                  }
                  else if (this.componentErrorIndex >= errorCount)
                  {
                     this.componentErrorIndex = errorCount - 1;
                  }

                  if (0 != errorCount)
                  {
                     this.ErrorTextBox.Text = this.Component.ErrorList[this.componentErrorIndex].Description;
                     this.ErrorTextBox.BackColor = Color.Yellow;

                     this.ErrorIndexLabel.Text = string.Format("{0}/{1}", (this.componentErrorIndex + 1), errorCount);
                     this.ErrorIndexLabel.Visible = true;

                     this.ErrorUpButton.Enabled = true;
                     this.ErrorDownButton.Enabled = true;
                     this.ClearErrorButton.Enabled = true;
                  }
                  else
                  {
                     this.ErrorTextBox.Text = "clear";
                     this.ErrorTextBox.BackColor = Color.Lime;
                     this.ErrorIndexLabel.Visible = false;

                     this.ErrorUpButton.Enabled = false;
                     this.ErrorDownButton.Enabled = false;
                     this.ClearErrorButton.Enabled = false;
                  }
               }
            }
         }
      }

      private void ClearErrorComplete()
      {
         this.ShowDeviceStatus();
      }

      #endregion

      #region Delegates

      private void ProcessDeviceClearErrorComplete(Enum deviceId)
      {
         if (Enum.Equals(deviceId, this.DeviceId) != false)
         {
            this.Invoke((MethodInvoker)(() => { this.ClearErrorComplete(); }));
         }
      }

      #endregion

      #region User Events

      private void ClearFaultButton_Click(object sender, EventArgs e)
      {
         if ((null != this.OnDeviceClearError) &&
             (null != this.Component) &&
             (null != this.Component.FaultRecord))
         {
            this.BackButton.Focus();
            this.ClearFaultButton.Enabled = false;
            this.ClearErrorButton.Enabled = false;

            UInt32 code = this.Component.FaultRecord.Code;
            DeviceClearErrorRequest.CompleteHandler onComplete = new DeviceClearErrorRequest.CompleteHandler(this.ProcessDeviceClearErrorComplete);
            this.OnDeviceClearError(this.DeviceId, code, onComplete);
         }
      }

      private void ClearErrorButton_Click(object sender, EventArgs e)
      {
         if ((null != this.OnDeviceClearError) &&
             (null != this.Component) &&
             (this.componentErrorIndex < this.Component.ErrorList.Count) &&
             (this.componentErrorIndex >= 0))
         {
            this.BackButton.Focus();
            this.ClearFaultButton.Enabled = false;
            this.ClearErrorButton.Enabled = false;

            UInt32 code = this.Component.ErrorList[this.componentErrorIndex].Code;
            DeviceClearErrorRequest.CompleteHandler onComplete = new DeviceClearErrorRequest.CompleteHandler(this.ProcessDeviceClearErrorComplete);
            this.OnDeviceClearError(this.DeviceId, code, onComplete);
         }
      }      

      private void ErrorUpButton_Click(object sender, EventArgs e)
      {
         if (this.componentErrorIndex > 0)
         {
            this.componentErrorIndex--;
            this.ErrorIndexLabel.Text = string.Format("{0}/{1}", (this.componentErrorIndex + 1), this.componentErrorCount);
            this.ErrorTextBox.Text = this.Component.ErrorList[this.componentErrorIndex].Description;
         }
      }

      private void ErrorDownButton_Click(object sender, EventArgs e)
      {
         if (this.componentErrorIndex < (this.componentErrorCount - 1))
         {
            this.componentErrorIndex++;
            this.ErrorIndexLabel.Text = string.Format("{0}/{1}", (this.componentErrorIndex + 1), this.componentErrorCount);
            this.ErrorTextBox.Text = this.Component.ErrorList[this.componentErrorIndex].Description;
         }
      }

      private void BackButton_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      #endregion

      #region Form Events

      private void CANDeviceInformationForm_Shown(object sender, EventArgs e)
      {
         this.TitleLabel.Text = this.Title;
         this.ShowDeviceStatus();
         this.UpdateTimer.Enabled = true;
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

      private void UpdateTimer_Tick(object sender, EventArgs e)
      {
         this.ShowDeviceStatus();
      }

      #endregion

      #region Constructor

      public DeviceComponentInformationForm()
      {
         this.InitializeComponent();
         this.componentErrorCount = -1;
         this.componentErrorIndex = -1;
      }

      #endregion

   }
}
