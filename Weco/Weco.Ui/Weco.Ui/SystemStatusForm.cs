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
   using Weco.Utilities;

   public partial class SystemStatusForm : Form
   {
      #region Definitions

      public delegate void SystemResetDelegate();
      public delegate void TraceListenerDestinationDelegate();

      private delegate string DeviceStatusHandler(Enum deviceId, ref bool warning);
      private delegate object DeviceRetrievalHandler(Enum deviceId);
      
      #endregion

      #region Fields

      private bool showVersion;

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

      private void SetComponentStatus(string status, bool warning, TextBox control)
      {
         control.ForeColor = Color.Black;

         if (null != status)
         {
            control.Text = status;

            if ("off" == status)
            {
               control.BackColor = Color.Gray;
            }
            else
            {
               control.BackColor = (false == warning) ? Color.Red : Color.Yellow;
            }
         }
         else
         {
            control.Text = "ready";
            control.BackColor = Color.Lime;
         }
      }

      private void SetVersionText(string versionText, TextBox control)
      {
         if (null != versionText)
         {
            control.ForeColor = Color.White;
            control.BackColor = Color.Black;
            control.Text = versionText;
         }
         else
         {
            control.BackColor = Color.Gray;
            control.ForeColor = Color.Black;
            control.Text = "---";
         }
      }

      private void SetComponentText(string faultString, string warningString, string versionString, TextBox control)
      {
         if (false == this.showVersion)
         {
            string textString = null;
            bool warning = false;

            if (null != warningString)
            {
               textString = faultString;
               warning = true;
            }
            else
            {
               textString = faultString;
            }

            this.SetComponentStatus(textString, warning, control);
         }
         else
         {
            this.SetVersionText(versionString, control);
         }
      }

      private void SetComponentText(Enum deviceId, DeviceStatusHandler onDeviceStatus, DeviceRetrievalHandler onDeviceRetrive, TextBox control)
      {
         if (false == this.showVersion)
         {
            bool warning = false;
            string status = onDeviceStatus(deviceId, ref warning);
            this.SetComponentStatus(status, warning, control);
         }
         else
         {
            string version = null;

            if (null != onDeviceRetrive)
            {
               object deviceObject = onDeviceRetrive(deviceId);

               if (null != deviceObject)
               {
                  if (deviceObject.GetType().IsSubclassOf(typeof(Device)) != false)
                  {
                     Device device = (Device)deviceObject;

                     if (null != device)
                     {
                        version = device.DeviceVersion;
                     }
                  }
               }
            }

            this.SetVersionText(version, control);
         }
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

      private void LaunchCANDeviceInformationForm(Label label, Enum deviceId, DeviceRetrievalHandler onDeviceRetrive, DeviceRestartRequest.RestartHandler onRestartHandler, DeviceClearErrorRequest.ClearErrorHandler onClearErrorHandler)
      {
         object deviceObject = onDeviceRetrive(deviceId);

         if (null != deviceObject)
         {
            if (deviceObject.GetType().IsSubclassOf(typeof(Device)) != false)
            {
               Device device = (Device)deviceObject;

               CANDeviceInformationForm deviceInformationForm = new CANDeviceInformationForm();
               this.SetDialogLocation(label, deviceInformationForm);

               deviceInformationForm.Title = label.Text;
               deviceInformationForm.Device = device;
               deviceInformationForm.DeviceId = deviceId;

               deviceInformationForm.OnDeviceRestart = onRestartHandler;
               deviceInformationForm.OnDeviceClearError = onClearErrorHandler;

               this.DimBackground();
               deviceInformationForm.ShowDialog();
               this.LightBackground();
            }
            else if ((deviceObject.GetType() == typeof(DeviceComponent) ||
                     (deviceObject.GetType().IsSubclassOf(typeof(DeviceComponent)) != false)))
            {
               DeviceComponent deviceComponent = (DeviceComponent)deviceObject;

               DeviceComponentInformationForm deviceComponentInformationForm = new DeviceComponentInformationForm();
               this.SetDialogLocation(label, deviceComponentInformationForm);

               deviceComponentInformationForm.Title = label.Text;
               deviceComponentInformationForm.Component = deviceComponent;
               deviceComponentInformationForm.DeviceId = deviceId;
               deviceComponentInformationForm.OnDeviceClearError = onClearErrorHandler;

               this.DimBackground();
               deviceComponentInformationForm.ShowDialog();
               this.LightBackground();
            }
         }
      }

      private DialogResult LaunchNumberEdit(ref double value, Control control, string title, int postDecimalDigitCount, string unit, double defaultValue, double mimimumValue, double maximumValue)
      {
         NumberEntryForm numberEntryForm = new NumberEntryForm();
         this.SetDialogLocation(control, numberEntryForm);

         numberEntryForm.Title = title;
         numberEntryForm.PostDecimalDigitCount = postDecimalDigitCount;
         numberEntryForm.Unit = unit;
         numberEntryForm.PresentValue = value;
         numberEntryForm.DefaultValue = defaultValue;
         numberEntryForm.MinimumValue = mimimumValue;
         numberEntryForm.MaximumValue = maximumValue;

         this.DimBackground();
         DialogResult result = numberEntryForm.ShowDialog();
         this.LightBackground();
         value = numberEntryForm.EnteredValue;

         return (result);
      }

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

      private void UpdateText()
      {
         this.SetComponentText(Joystick.Instance.FaultReason, null, null, this.JoystickStatusTextBox);
         this.SetComponentText(LaserCommunicationBus.BusComponentId.Bus, LaserCommunicationBus.Instance.GetStatus, LaserCommunicationBus.Instance.GetDevice, this.LaserBusStatusTextBox);
         this.SetComponentText(LaserCommunicationBus.BusComponentId.LaserBoard, LaserCommunicationBus.Instance.GetStatus, LaserCommunicationBus.Instance.GetDevice, this.LaserBoardStatusTextBox);
         this.SetComponentText(LaserCommunicationBus.BusComponentId.LaserBoardCameraLed, LaserCommunicationBus.Instance.GetStatus, LaserCommunicationBus.Instance.GetDevice, this.LaserBoardCameraLedStatusTextBox);
         this.SetComponentText(LaserCommunicationBus.BusComponentId.LaserBoardFrontWheel, LaserCommunicationBus.Instance.GetStatus, LaserCommunicationBus.Instance.GetDevice, this.LaserBoardFrontWheelStatusTextBox);
         this.SetComponentText(LaserCommunicationBus.BusComponentId.LaserBoardRearWheel, LaserCommunicationBus.Instance.GetStatus, LaserCommunicationBus.Instance.GetDevice, this.LaserBoardRearWheelStatusTextBox);

         this.SetComponentText(NumatoUsbRelay.Instance.FaultReason, null, null, this.UsbRelayStatusTextBox);

         if (false == ParameterAccessor.Instance.RunTargetOnLaserBus)
         {
            this.SetComponentText(TargetCommunicationBus.BusComponentId.Bus, TargetCommunicationBus.Instance.GetStatus, TargetCommunicationBus.Instance.GetDevice, this.TargetBusStatusTextBox);
         }
         else
         {
            this.TargetBusStatusTextBox.Text = "off";
            this.TargetBusStatusTextBox.BackColor = Color.Gray;
         }

         this.SetComponentText(TargetCommunicationBus.BusComponentId.TargetBoard, TargetCommunicationBus.Instance.GetStatus, TargetCommunicationBus.Instance.GetDevice, this.TargetBoardStatusTextBox);
         this.SetComponentText(TargetCommunicationBus.BusComponentId.TargetBoardCameraLed, TargetCommunicationBus.Instance.GetStatus, TargetCommunicationBus.Instance.GetDevice, this.TargetBoardCameraLedStatusTextBox);
         this.SetComponentText(TargetCommunicationBus.BusComponentId.TargetBoardFrontWheel, TargetCommunicationBus.Instance.GetStatus, TargetCommunicationBus.Instance.GetDevice, this.TargetBoardFrontWheelStatusTextBox);
         this.SetComponentText(TargetCommunicationBus.BusComponentId.TargetBoardRearWheel, TargetCommunicationBus.Instance.GetStatus, TargetCommunicationBus.Instance.GetDevice, this.TargetBoardRearWheelStatusTextBox);

         double latitude = ParameterAccessor.Instance.Latitude;
         if (double.IsNaN(latitude) == false)
         {
            this.SensorLatitudeTextPanel.ValueText = latitude.ToString("N4");
         }
         else
         {
            this.SensorLatitudeTextPanel.ValueText = "---";
         }

         double longitude = ParameterAccessor.Instance.Longitude;
         if (double.IsNaN(latitude) == false)
         {
            this.SensorLongitudeTextPanel.ValueText = longitude.ToString("N4");
         }
         else
         {
            this.SensorLongitudeTextPanel.ValueText = "---";
         }
      }

      private void ShowSettings()
      {
         this.LoggingAddressValueButton.ValueText = ParameterAccessor.Instance.Trace.Address;
         this.LoggingPortValueButton.ValueText = ParameterAccessor.Instance.Trace.Port.ToString();
      }

      private void SetTraceListenerDestination()
      {
         if (null != this.TraceListenerDestination)
         {
            this.TraceListenerDestination();
         }
      }

      private void ResetSystem()
      {
         if (null != this.SystemResetHandler)
         {
            this.SystemResetHandler();
         }
      }

      #endregion

      #region Properties

      public SystemResetDelegate SystemResetHandler { set; get; }
      public TraceListenerDestinationDelegate TraceListenerDestination { set; get; }

      #endregion

      #region User Events

      private void LaserBoardLabel_Click(object sender, EventArgs e)
      {
         this.LaunchCANDeviceInformationForm(this.LaserBoardLabel, LaserCommunicationBus.BusComponentId.LaserBoard, LaserCommunicationBus.Instance.GetDevice, LaserCommunicationBus.Instance.RestartDevice, LaserCommunicationBus.Instance.ClearDeviceError);
      }

      private void LaserBoardCameraLedLabel_Click(object sender, EventArgs e)
      {
         this.LaunchCANDeviceInformationForm(this.LaserBoardCameraLedLabel, LaserCommunicationBus.BusComponentId.LaserBoardCameraLed, LaserCommunicationBus.Instance.GetDevice, null, LaserCommunicationBus.Instance.ClearDeviceError);
      }

      private void LaserBoardFrontWheelLabel_Click(object sender, EventArgs e)
      {
         this.LaunchCANDeviceInformationForm(this.LaserBoardFrontWheelLabel, LaserCommunicationBus.BusComponentId.LaserBoardFrontWheel, LaserCommunicationBus.Instance.GetDevice, null, LaserCommunicationBus.Instance.ClearDeviceError);
      }

      private void LaserBoardRearWheelLabel_Click(object sender, EventArgs e)
      {
         this.LaunchCANDeviceInformationForm(this.LaserBoardRearWheelLabel, LaserCommunicationBus.BusComponentId.LaserBoardRearWheel, LaserCommunicationBus.Instance.GetDevice, null, LaserCommunicationBus.Instance.ClearDeviceError);
      }

      private void LaserBoardLeftStepperLabel_Click(object sender, EventArgs e)
      {
      }

      private void LaserBoardRightStepperLabel_Click(object sender, EventArgs e)
      {
      }

      private void GpsLabel_Click(object sender, EventArgs e)
      {
         //this.LaunchCANDeviceInformationForm(this.GpsLabel, LaserCommunicationBus.BusComponentId.Gps, LaserCommunicationBus.Instance.GetDevice, LaserCommunicationBus.Instance.RestartDevice, LaserCommunicationBus.Instance.ClearDeviceError);
      }

      private void TargetBoardLabel_Click(object sender, EventArgs e)
      {
         this.LaunchCANDeviceInformationForm(this.TargetBoardLabel, TargetCommunicationBus.BusComponentId.TargetBoard, TargetCommunicationBus.Instance.GetDevice, TargetCommunicationBus.Instance.RestartDevice, TargetCommunicationBus.Instance.ClearDeviceError);
      }

      private void TargetBoardCameraLedLabel_Click(object sender, EventArgs e)
      {
         this.LaunchCANDeviceInformationForm(this.TargetBoardCameraLedLabel, TargetCommunicationBus.BusComponentId.TargetBoardCameraLed, TargetCommunicationBus.Instance.GetDevice, null, TargetCommunicationBus.Instance.ClearDeviceError);
      }

      private void TargetBoardFrontWheelLabel_Click(object sender, EventArgs e)
      {
         this.LaunchCANDeviceInformationForm(this.TargetBoardFrontWheelLabel, TargetCommunicationBus.BusComponentId.TargetBoardFrontWheel, TargetCommunicationBus.Instance.GetDevice, null, TargetCommunicationBus.Instance.ClearDeviceError);
      }

      private void TargetBoardRearWheelLabel_Click(object sender, EventArgs e)
      {
         this.LaunchCANDeviceInformationForm(this.TargetBoardRearWheelLabel, TargetCommunicationBus.BusComponentId.TargetBoardRearWheel, TargetCommunicationBus.Instance.GetDevice, null, TargetCommunicationBus.Instance.ClearDeviceError);
      }

      private void TargetBoardCameraStepperLabel_Click(object sender, EventArgs e)
      {
      }

      private void TriggerDefaultsButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         ParameterAccessor.Instance.TriggerDefaults();

         MessageForm messageForm = new MessageForm();
         messageForm.Title = "SYSTEM DEFAULTS";
         messageForm.Message = "DEFAULTS ASSIGNED ON THE NEXT SYSTEM START";
         messageForm.Buttons = MessageBoxButtons.OK;

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
         messageForm.Buttons = MessageBoxButtons.OK;

         this.SetDialogLocation(this.SaveDefaultsButton, messageForm);

         this.DimBackground();
         messageForm.ShowDialog();
         this.LightBackground();
      }

      private void LoggingAddressValueButton_Click(object sender, EventArgs e)
      {
         IpAddressEntryForm ipAddressEntryForm = new IpAddressEntryForm();
         this.SetDialogLocation(this.LoggingPortValueButton, ipAddressEntryForm);

         ipAddressEntryForm.Title = "LOGGING ADDRESS";
         ipAddressEntryForm.PresentValue = ParameterAccessor.Instance.Trace.Address;
         ipAddressEntryForm.DefaultValue = "127.0.0.1";

         this.DimBackground();
         DialogResult result = ipAddressEntryForm.ShowDialog();
         this.LightBackground();

         if (result == System.Windows.Forms.DialogResult.OK)
         {
            ParameterAccessor.Instance.Trace.Address = ipAddressEntryForm.EnteredValue;
            this.LoggingAddressValueButton.ValueText = ParameterAccessor.Instance.Trace.Address;
            this.SetTraceListenerDestination();
         }
      }

      private void LoggingPortValueButton_Click(object sender, EventArgs e)
      {
         double value = ParameterAccessor.Instance.Trace.Port;
         DialogResult result = this.LaunchNumberEdit(ref value, this.LoggingPortValueButton, "LOGGING PORT", 0, "", 10000, 1, 65535);

         if (result == System.Windows.Forms.DialogResult.OK)
         {
            ParameterAccessor.Instance.Trace.Port = (int)value;
            this.LoggingPortValueButton.ValueText = ParameterAccessor.Instance.Trace.Port.ToString();
            this.SetTraceListenerDestination();
         }
      }

      private void MainBusHeartbeatButton_Click(object sender, EventArgs e)
      {
         LaserCommunicationBus.Instance.TraceHB = !LaserCommunicationBus.Instance.TraceHB;
         this.SetLogSelect(this.MainBusHeartbeatButton, LaserCommunicationBus.Instance.TraceHB);
      }

      private void TargetBusHeartbeatButton_Click(object sender, EventArgs e)
      {
         TargetCommunicationBus.Instance.TraceHB = !TargetCommunicationBus.Instance.TraceHB;
         this.SetLogSelect(this.TargetBusHeartbeatButton, TargetCommunicationBus.Instance.TraceHB);
      }

      private void SensorLatitudeTextPanel_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         double latitude = ParameterAccessor.Instance.Latitude;
         DialogResult result = this.LaunchNumberEdit(ref latitude, this.SensorLatitudeTextPanel, "LATITUDE", 4, "°", 40.812838, -90, 90);

         if (result == System.Windows.Forms.DialogResult.OK)
         {
            ParameterAccessor.Instance.Latitude = latitude;
            this.SensorLatitudeTextPanel.ValueText = latitude.ToString("N4");
         }
      }

      private void SensorLongitudeTextPanel_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         double longitude = ParameterAccessor.Instance.Longitude;
         DialogResult result = this.LaunchNumberEdit(ref longitude, this.SensorLongitudeTextPanel, "LONGITUDE", 4, "°", -73.254690, -180, 180);

         if (result == System.Windows.Forms.DialogResult.OK)
         {
            ParameterAccessor.Instance.Longitude = longitude;
            this.SensorLongitudeTextPanel.ValueText = longitude.ToString("N4");
         }
      }

      private void ShowToggleButton_Click(object sender, EventArgs e)
      {
         if (false == this.showVersion)
         {
            this.showVersion = true;
            this.ShowToggleButton.Text = "SHOW STATUS";
         }
         else
         {
            this.showVersion = false;
            this.ShowToggleButton.Text = "SHOW VERSION";
         }

         this.UpdateText();
      }

      private void SystemResetButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         this.ResetSystem();
      }

      private void BackButton_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      #endregion

      #region Form Events

      private void SystemStatusForm_Shown(object sender, EventArgs e)
      {
         this.showVersion = false;
         this.ShowToggleButton.Text = "SHOW VERSION";

         this.UpdateText();
         this.ShowSettings();
         this.UpdateTimer.Enabled = true;

         this.SetLogSelect(this.MainBusHeartbeatButton, LaserCommunicationBus.Instance.TraceHB);
         this.SetLogSelect(this.TargetBusHeartbeatButton, TargetCommunicationBus.Instance.TraceHB);
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

      private void UpdateTimer_Tick(object sender, EventArgs e)
      {
         this.UpdateText();
         this.ShowSettings();
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
