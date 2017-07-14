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

   public partial class CANDeviceInformationForm : Form
   {
      #region Definition

      #endregion

      #region Fields

      private string previousFaultStatus;
      private string previousWarningStatus;

      private bool mouseDown;
      private Point mouseDownPoint;

      #endregion

      #region Properties

      public string Title { set; get; }
      public CAN.Device Device { set; get; }
      public Enum DeviceId { set; get; }
      public DeviceRestartRequest.RestartHandler OnDeviceRestart { set; get; }
      public DeviceClearErrorRequest.ClearErrorHandler OnDeviceClearError { set; get; }

      #endregion

      #region Helper Functions

      private void SetLogSelect(Button button, bool selected)
      {
         if (false != selected)
         {
            button.BackColor = Color.Lime;
         }
         else
         {
            button.BackColor = Color.FromArgb(171, 171, 171);
         }
      }

      private void ShowDeviceStatus()
      {
         string faultStatus = this.Device.FaultReason;
         string warningStatus = this.Device.Warning;

         if ((this.previousFaultStatus != faultStatus) ||
             (this.previousWarningStatus != warningStatus)) 
         {
            if (null != faultStatus)
            {
               this.DeviceStatusTextBox.Text = faultStatus;
               this.DeviceStatusTextBox.BackColor = Color.Red;
            }
            else if (null != warningStatus)
            {
               this.DeviceStatusTextBox.Text = warningStatus;
               this.DeviceStatusTextBox.BackColor = Color.Yellow;
            }
            else
            {
               this.DeviceStatusTextBox.Text = "ready";
               this.DeviceStatusTextBox.BackColor = Color.Lime;
            }

            this.ClearWarningButton.Visible = (null != warningStatus) ? true : false;

            this.DeviceStatusTextBox.DeselectAll();
            this.HbButton.Focus();
            this.previousFaultStatus = faultStatus;
            this.previousWarningStatus = warningStatus;
         }
      }

      private void ShowDeviceInformation()
      {
         if (null != this.Device)
         {
            this.DeviceNameValuePanel.ValueText = this.Device.DeviceName;
            this.DeviceVersionValuePanel.ValueText = this.Device.DeviceVersion;
            this.DeviceTypeValuePanel.ValueText = string.Format("{0:X8}", this.Device.DeviceType);
            this.LoggingNameValuePanel.ValueText = this.Device.Name;
            this.NodeIdValuePanel.ValueText = this.Device.NodeId.ToString();

            this.SetLogSelect(this.SdoButton, this.Device.TraceSDO);
            this.SetLogSelect(this.HbButton, this.Device.TraceHB);
            this.SetLogSelect(this.Rpdo1Button, this.Device.TraceRPDO1);
            this.SetLogSelect(this.Rpdo2Button, this.Device.TraceRPDO2);
            this.SetLogSelect(this.Rpdo3Button, this.Device.TraceRPDO3);
            this.SetLogSelect(this.Rpdo4Button, this.Device.TraceRPDO4);
            this.SetLogSelect(this.Tpdo1Button, this.Device.TraceTPDO1);
            this.SetLogSelect(this.Tpdo2Button, this.Device.TraceTPDO2);
            this.SetLogSelect(this.Tpdo3Button, this.Device.TraceTPDO3);
            this.SetLogSelect(this.Tpdo4Button, this.Device.TraceTPDO4);
         }
      }

      private void RestartComplete()
      {
         this.ShowDeviceInformation();
         this.RestartButton.Enabled = true;
      }

      private void ProcessDeviceRestartComplete(Enum deviceId)
      {
         if (Enum.Equals(deviceId, this.DeviceId) != false)
         {
            this.Invoke((MethodInvoker)(() => { this.RestartComplete(); })); 
         }
      }

      private void ClearErrorComplete()
      {
         this.ShowDeviceInformation();
         this.ClearWarningButton.Enabled = true;
      }

      private void ProcessDeviceClearErrorComplete(Enum deviceId)
      {
         if (Enum.Equals(deviceId, this.DeviceId) != false)
         {
            this.Invoke((MethodInvoker)(() => { this.ClearErrorComplete(); }));
         }
      }

      #endregion

      #region User Events

      private void SdoButton_Click(object sender, EventArgs e)
      {
         if (null != this.Device)
         {
            this.Device.TraceSDO = !this.Device.TraceSDO;
            this.SetLogSelect(this.SdoButton, this.Device.TraceSDO);
         }
      }

      private void HbButton_Click(object sender, EventArgs e)
      {
         if (null != this.Device)
         {
            this.Device.TraceHB = !this.Device.TraceHB;
            this.SetLogSelect(this.HbButton, this.Device.TraceHB);
         }
      }

      private void Rpdo1Button_Click(object sender, EventArgs e)
      {
         if (null != this.Device)
         {
            this.Device.TraceRPDO1 = !this.Device.TraceRPDO1;
            this.SetLogSelect(this.Rpdo1Button, this.Device.TraceRPDO1);
         }
      }

      private void Tpdo1Button_Click(object sender, EventArgs e)
      {
         if (null != this.Device)
         {
            this.Device.TraceTPDO1 = !this.Device.TraceTPDO1;
            this.SetLogSelect(this.Tpdo1Button, this.Device.TraceTPDO1);
         }
      }

      private void Rpdo2Button_Click(object sender, EventArgs e)
      {
         if (null != this.Device)
         {
            this.Device.TraceRPDO2 = !this.Device.TraceRPDO2;
            this.SetLogSelect(this.Rpdo2Button, this.Device.TraceRPDO2);
         }
      }

      private void Tpdo2Button_Click(object sender, EventArgs e)
      {
         if (null != this.Device)
         {
            this.Device.TraceTPDO2 = !this.Device.TraceTPDO2;
            this.SetLogSelect(this.Tpdo2Button, this.Device.TraceTPDO2);
         }
      }

      private void Rpdo3Button_Click(object sender, EventArgs e)
      {
         if (null != this.Device)
         {
            this.Device.TraceRPDO3 = !this.Device.TraceRPDO3;
            this.SetLogSelect(this.Rpdo3Button, this.Device.TraceRPDO3);
         }
      }

      private void Tpdo3Button_Click(object sender, EventArgs e)
      {
         if (null != this.Device)
         {
            this.Device.TraceTPDO3 = !this.Device.TraceTPDO3;
            this.SetLogSelect(this.Tpdo3Button, this.Device.TraceTPDO3);
         }
      }

      private void Rpdo4Button_Click(object sender, EventArgs e)
      {
         if (null != this.Device)
         {
            this.Device.TraceRPDO4 = !this.Device.TraceRPDO4;
            this.SetLogSelect(this.Rpdo4Button, this.Device.TraceRPDO4);
         }
      }

      private void Tpdo4Button_Click(object sender, EventArgs e)
      {
         if (null != this.Device)
         {
            this.Device.TraceTPDO4 = !this.Device.TraceTPDO4;
            this.SetLogSelect(this.Tpdo4Button, this.Device.TraceTPDO4);
         }
      }

      private void RestartButton_Click(object sender, EventArgs e)
      {
         if (null != this.OnDeviceRestart)
         {
            this.HbButton.Focus();
            this.RestartButton.Enabled = false;

            DeviceRestartRequest.CompleteHandler onComplete = new DeviceRestartRequest.CompleteHandler(this.ProcessDeviceRestartComplete);
            this.OnDeviceRestart(this.DeviceId, onComplete);
         }
      }

      private void ClearWarningButton_Click(object sender, EventArgs e)
      {
         if (null != this.OnDeviceClearError)
         {
            this.HbButton.Focus();
            this.ClearWarningButton.Enabled = false;

            DeviceClearErrorRequest.CompleteHandler onComplete = new DeviceClearErrorRequest.CompleteHandler(this.ProcessDeviceClearErrorComplete);
            this.OnDeviceClearError(this.DeviceId, 0, onComplete);
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
         this.previousFaultStatus = "";
         this.previousWarningStatus = "";

         this.TitleLabel.Text = this.Title;
         this.ShowDeviceInformation();
         this.ShowDeviceStatus();
         this.RestartButton.Visible = (null != this.OnDeviceRestart) ? true : false;

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

      public CANDeviceInformationForm()
      {
         this.InitializeComponent();
      }

      #endregion
      
   }
}
