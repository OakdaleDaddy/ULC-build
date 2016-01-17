
namespace NICBOT.GUI
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

   using NICBOT.CAN;  
   using NICBOT.Utilities;

   public partial class SystemStatusForm : Form
   {
      #region Definitions

      public delegate void SystemResetDelegate();
      public delegate void TraceListenerDestinationDelegate();

      #endregion

      #region Properties

      public SystemResetDelegate SystemResetHandler { set; get; }
      public TraceListenerDestinationDelegate TraceListenerDestination { set; get; }
      
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

      private void SetComponentStatus(string status, TextBox control)
      {
         if (null != status)
         {
            control.Text = status;
            control.BackColor = Color.Red;
         }
         else
         {
            control.Text = "ready";
            control.BackColor = Color.LimeGreen;
         }
      }

      private void LaunchCANDeviceInformationForm(Label label, Device device)
      {
         if (null != device)
         {
            CANDeviceInformationForm deviceInformationForm = new CANDeviceInformationForm();
            this.SetDialogLocation(label, deviceInformationForm);

            deviceInformationForm.Title = label.Text;
            deviceInformationForm.Device = device;

            deviceInformationForm.ShowDialog();
         }
      }

      private void SetLogSelect(Button button, bool selected)
      {
         if (false != selected)
         {
            button.ForeColor = Color.White;
         }
         else
         {
            button.ForeColor = Color.Black;
         }
      }

      private void UpdateStatus()
      {
         this.SetComponentStatus(Joystick.Instance.FaultReason, this.JoystickStatusTextBox);
         this.SetComponentStatus(RobotCommBus.Instance.GetStatus(RobotCommBus.BusComponentId.Bus), this.RobotBusStatusTextBox);
         this.SetComponentStatus(RobotCommBus.Instance.GetStatus(RobotCommBus.BusComponentId.RobotBody), this.RobotBodyStatusTextBox);
         this.SetComponentStatus(RobotCommBus.Instance.GetStatus(RobotCommBus.BusComponentId.RobotTopFrontWheel), this.RobotTopFrontWheelStatusTextBox);
         this.SetComponentStatus(RobotCommBus.Instance.GetStatus(RobotCommBus.BusComponentId.RobotTopRearWheel), this.RobotTopRearWheelStatusTextBox);
         this.SetComponentStatus(RobotCommBus.Instance.GetStatus(RobotCommBus.BusComponentId.RobotBottomFrontWheel), this.RobotBottomFrontWheelStatusTextBox);
         this.SetComponentStatus(RobotCommBus.Instance.GetStatus(RobotCommBus.BusComponentId.RobotBottomRearWheel), this.RobotBottomRearWheelStatusTextBox);
         this.SetComponentStatus(TruckCommBus.Instance.GetStatus(TruckCommBus.BusComponentId.NitrogenSensor1), this.NitrogenSensor1StatusTextBox);
         this.SetComponentStatus(TruckCommBus.Instance.GetStatus(TruckCommBus.BusComponentId.NitrogenSensor2), this.NitrogenSensor2StatusTextBox);

         this.SetComponentStatus(TruckCommBus.Instance.GetStatus(TruckCommBus.BusComponentId.Gps), this.GpsStatusTextBox);
         this.SetComponentStatus(TruckCommBus.Instance.GetStatus(TruckCommBus.BusComponentId.Bus), this.TruckBusStatusTextBox);
         this.SetComponentStatus(TruckCommBus.Instance.GetStatus(TruckCommBus.BusComponentId.ReelMotor), this.ReelMotorStatusTextBox);
         this.SetComponentStatus(TruckCommBus.Instance.GetStatus(TruckCommBus.BusComponentId.ReelDigitalIo), this.ReelDigitalIoStatusTextBox);
         this.SetComponentStatus(TruckCommBus.Instance.GetStatus(TruckCommBus.BusComponentId.ReelAnalogIo), this.ReelAnalogIoStatusTextBox);
         this.SetComponentStatus(TruckCommBus.Instance.GetStatus(TruckCommBus.BusComponentId.ReelEncoder), this.ReelEncoderStatusTextBox);
         this.SetComponentStatus(TruckCommBus.Instance.GetStatus(TruckCommBus.BusComponentId.FeederTopFrontMotor), this.FeederTopFrontMotorStatusTextBox);
         this.SetComponentStatus(TruckCommBus.Instance.GetStatus(TruckCommBus.BusComponentId.FeederTopRearMotor), this.FeederTopRearMotorStatusTextBox);
         this.SetComponentStatus(TruckCommBus.Instance.GetStatus(TruckCommBus.BusComponentId.FeederBottomFrontMotor), this.FeederBottomFrontMotorStatusTextBox);
         this.SetComponentStatus(TruckCommBus.Instance.GetStatus(TruckCommBus.BusComponentId.FeederBottomRearMotor), this.FeederBottomRearMotorStatusTextBox);
         
                 
         // feeder encoder is off, no time to install
         //this.SetComponentStatus(TruckCommBus.Instance.GetStatus(TruckCommBus.BusComponentId.FeederEncoder), this.FeederEncoderStatusTextBox);
         this.FeederEncoderStatusTextBox.Text = "off";
         this.FeederEncoderStatusTextBox.BackColor = Color.Gray;
                 
         
         this.SetComponentStatus(TruckCommBus.Instance.GetStatus(TruckCommBus.BusComponentId.GuideLeftMotor), this.GuideLeftMotorStatusTextBox);
         this.SetComponentStatus(TruckCommBus.Instance.GetStatus(TruckCommBus.BusComponentId.GuideRightMotor), this.GuideRightMotorStatusTextBox);
         this.SetComponentStatus(TruckCommBus.Instance.GetStatus(TruckCommBus.BusComponentId.LaunchDigitalIo), this.LaunchDigitalIoStatusTextBox);
         this.SetComponentStatus(TruckCommBus.Instance.GetStatus(TruckCommBus.BusComponentId.LaunchAnalogIo), this.LaunchAnalogIoStatusTextBox);

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            this.SetComponentStatus(TruckCommBus.Instance.GetStatus(TruckCommBus.BusComponentId.FrontPumpMotor), this.FrontPumpMotorStatusTextBox);
            this.SetComponentStatus(TruckCommBus.Instance.GetStatus(TruckCommBus.BusComponentId.FrontPressureSensor), this.FrontPressureSensorStatusTextBox);
            this.SetComponentStatus(TruckCommBus.Instance.GetStatus(TruckCommBus.BusComponentId.FrontScaleRs232), this.FrontScaleRs232StatusTextBox);
            this.SetComponentStatus(TruckCommBus.Instance.GetStatus(TruckCommBus.BusComponentId.FrontDigitalScale), this.FrontScaleStatusTextBox);

            this.SetComponentStatus(TruckCommBus.Instance.GetStatus(TruckCommBus.BusComponentId.RearPumpMotor), this.RearPumpMotorStatusTextBox);
            this.SetComponentStatus(TruckCommBus.Instance.GetStatus(TruckCommBus.BusComponentId.RearPressureSensor), this.RearPressureSensorStatusTextBox);
            this.SetComponentStatus(TruckCommBus.Instance.GetStatus(TruckCommBus.BusComponentId.RearScaleRs232), this.RearScaleRs232StatusTextBox);
            this.SetComponentStatus(TruckCommBus.Instance.GetStatus(TruckCommBus.BusComponentId.RearDigitalScale), this.RearScaleStatusTextBox);

            this.ThicknessSensorSensorTextBox.Text = "off";
            this.ThicknessSensorSensorTextBox.BackColor = Color.Gray;

            this.StressSensorStatusTextBox.Text = "off";
            this.StressSensorStatusTextBox.BackColor = Color.Gray;
         }

         if (RobotApplications.inspect == ParameterAccessor.Instance.RobotApplication)
         {
            this.FrontPumpMotorStatusTextBox.Text = "off";
            this.FrontPumpMotorStatusTextBox.BackColor = Color.Gray;

            this.FrontPressureSensorStatusTextBox.Text = "off";
            this.FrontPressureSensorStatusTextBox.BackColor = Color.Gray;

            this.FrontScaleRs232StatusTextBox.Text = "off";
            this.FrontScaleRs232StatusTextBox.BackColor = Color.Gray;

            this.FrontScaleStatusTextBox.Text = "off";
            this.FrontScaleStatusTextBox.BackColor = Color.Gray;

            this.RearPumpMotorStatusTextBox.Text = "off";
            this.RearPumpMotorStatusTextBox.BackColor = Color.Gray;

            this.RearPressureSensorStatusTextBox.Text = "off";
            this.RearPressureSensorStatusTextBox.BackColor = Color.Gray;

            this.RearScaleRs232StatusTextBox.Text = "off";
            this.RearScaleRs232StatusTextBox.BackColor = Color.Gray;

            this.RearScaleStatusTextBox.Text = "off";
            this.RearScaleStatusTextBox.BackColor = Color.Gray;

            if (TruckCommBus.Instance.GetThicknessReadingEnabled() != false)
            {
               this.SetComponentStatus(TruckCommBus.Instance.GetStatus(TruckCommBus.BusComponentId.ThicknessSensor), this.ThicknessSensorSensorTextBox);
            }
            else
            {
               this.ThicknessSensorSensorTextBox.Text = "off";
               this.ThicknessSensorSensorTextBox.BackColor = Color.Gray;
            }

            if (TruckCommBus.Instance.GetStressReadingEnabled() != false)
            {
               this.SetComponentStatus(TruckCommBus.Instance.GetStatus(TruckCommBus.BusComponentId.StressSensor), this.StressSensorStatusTextBox);
            }
            else
            {
               this.StressSensorStatusTextBox.Text = "off";
               this.StressSensorStatusTextBox.BackColor = Color.Gray;
            }
         }

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

      #endregion

      #region User Event Functions

      private void RobotBodyLabel_Click(object sender, EventArgs e)
      {
         Device device = RobotCommBus.Instance.GetDevice(RobotCommBus.BusComponentId.RobotBody);
         this.LaunchCANDeviceInformationForm(this.RobotBodyLabel, device);
      }

      private void RobotTopFrontWheelLabel_Click(object sender, EventArgs e)
      {
         Device device = RobotCommBus.Instance.GetDevice(RobotCommBus.BusComponentId.RobotTopFrontWheel);
         this.LaunchCANDeviceInformationForm(this.RobotTopFrontWheelLabel, device);
      }

      private void RobotTopRearLabel_Click(object sender, EventArgs e)
      {
         Device device = RobotCommBus.Instance.GetDevice(RobotCommBus.BusComponentId.RobotTopRearWheel);
         this.LaunchCANDeviceInformationForm(this.RobotTopRearLabel, device);
      }

      private void RobotBottomFrontWheelLabel_Click(object sender, EventArgs e)
      {
         Device device = RobotCommBus.Instance.GetDevice(RobotCommBus.BusComponentId.RobotBottomFrontWheel);
         this.LaunchCANDeviceInformationForm(this.RobotBottomFrontWheelLabel, device);
      }

      private void RobotBottomRearWheelLabel_Click(object sender, EventArgs e)
      {
         Device device = RobotCommBus.Instance.GetDevice(RobotCommBus.BusComponentId.RobotBottomRearWheel);
         this.LaunchCANDeviceInformationForm(this.RobotBottomRearWheelLabel, device);
      }

      private void FrontPumpMotorLabel_Click(object sender, EventArgs e)
      {
         Device device = TruckCommBus.Instance.GetDevice(TruckCommBus.BusComponentId.FrontPumpMotor);
         this.LaunchCANDeviceInformationForm(this.FrontPumpMotorLabel, device);
      }

      private void FrontScaleRs232Label_Click(object sender, EventArgs e)
      {
         Device device = TruckCommBus.Instance.GetDevice(TruckCommBus.BusComponentId.FrontScaleRs232);
         this.LaunchCANDeviceInformationForm(this.FrontScaleRs232Label, device);
      }

      private void RearPumpMotorLabel_Click(object sender, EventArgs e)
      {
         Device device = TruckCommBus.Instance.GetDevice(TruckCommBus.BusComponentId.RearPumpMotor);
         this.LaunchCANDeviceInformationForm(this.RearPumpMotorLabel, device);
      }

      private void RearScaleRs232Label_Click(object sender, EventArgs e)
      {
         Device device = TruckCommBus.Instance.GetDevice(TruckCommBus.BusComponentId.RearScaleRs232);
         this.LaunchCANDeviceInformationForm(this.RearScaleRs232Label, device);
      }

      private void GpsLabel_Click(object sender, EventArgs e)
      {
         Device device = TruckCommBus.Instance.GetDevice(TruckCommBus.BusComponentId.Gps);
         this.LaunchCANDeviceInformationForm(this.GpsLabel, device);
      }

      private void ReelMotorLabel_Click(object sender, EventArgs e)
      {
         Device device = TruckCommBus.Instance.GetDevice(TruckCommBus.BusComponentId.ReelMotor);
         this.LaunchCANDeviceInformationForm(this.ReelMotorLabel, device);
      }

      private void ReelDigitalIoLabel_Click(object sender, EventArgs e)
      {
         Device device = TruckCommBus.Instance.GetDevice(TruckCommBus.BusComponentId.ReelDigitalIo);
         this.LaunchCANDeviceInformationForm(this.ReelDigitalIoLabel, device);
      }

      private void ReelAnalogIoLabel_Click(object sender, EventArgs e)
      {
         Device device = TruckCommBus.Instance.GetDevice(TruckCommBus.BusComponentId.ReelAnalogIo);
         this.LaunchCANDeviceInformationForm(this.ReelAnalogIoLabel, device);
      }

      private void ReelEncoderLabel_Click(object sender, EventArgs e)
      {
         Device device = TruckCommBus.Instance.GetDevice(TruckCommBus.BusComponentId.ReelEncoder);
         this.LaunchCANDeviceInformationForm(this.ReelEncoderLabel, device);
      }

      private void FeederTopFrontMotorLabel_Click(object sender, EventArgs e)
      {
         Device device = TruckCommBus.Instance.GetDevice(TruckCommBus.BusComponentId.FeederTopFrontMotor);
         this.LaunchCANDeviceInformationForm(this.FeederTopFrontMotorLabel, device);
      }

      private void FeederTopRearMotorLabel_Click(object sender, EventArgs e)
      {
         Device device = TruckCommBus.Instance.GetDevice(TruckCommBus.BusComponentId.FeederTopRearMotor);
         this.LaunchCANDeviceInformationForm(this.FeederTopRearMotorLabel, device);
      }

      private void FeederBottomFrontMotorLabel_Click(object sender, EventArgs e)
      {
         Device device = TruckCommBus.Instance.GetDevice(TruckCommBus.BusComponentId.FeederBottomFrontMotor);
         this.LaunchCANDeviceInformationForm(this.FeederBottomFrontMotorLabel, device);
      }

      private void FeederBottomRearMotorLabel_Click(object sender, EventArgs e)
      {
         Device device = TruckCommBus.Instance.GetDevice(TruckCommBus.BusComponentId.FeederBottomRearMotor);
         this.LaunchCANDeviceInformationForm(this.FeederBottomRearMotorLabel, device);
      }

      private void FeederEncoderLabel_Click(object sender, EventArgs e)
      {
         Device device = TruckCommBus.Instance.GetDevice(TruckCommBus.BusComponentId.FeederEncoder);
         this.LaunchCANDeviceInformationForm(this.FeederEncoderLabel, device);
      }

      private void GuideLeftMotorLabel_Click(object sender, EventArgs e)
      {
         Device device = TruckCommBus.Instance.GetDevice(TruckCommBus.BusComponentId.GuideLeftMotor);
         this.LaunchCANDeviceInformationForm(this.GuideLeftMotorLabel, device);
      }

      private void GuideRightMotorLabel_Click(object sender, EventArgs e)
      {
         Device device = TruckCommBus.Instance.GetDevice(TruckCommBus.BusComponentId.GuideRightMotor);
         this.LaunchCANDeviceInformationForm(this.GuideRightMotorLabel, device);
      }

      private void LaunchDigitalIoLabel_Click(object sender, EventArgs e)
      {
         Device device = TruckCommBus.Instance.GetDevice(TruckCommBus.BusComponentId.LaunchDigitalIo);
         this.LaunchCANDeviceInformationForm(this.LaunchDigitalIoLabel, device);
      }

      private void LaunchAnalogIoLabel_Click(object sender, EventArgs e)
      {
         Device device = TruckCommBus.Instance.GetDevice(TruckCommBus.BusComponentId.LaunchAnalogIo);
         this.LaunchCANDeviceInformationForm(this.LaunchAnalogIoLabel, device);
      }

      private void TriggerDefaultsButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         ParameterAccessor.Instance.TriggerDefaults();

         MessageForm messageForm = new MessageForm();
         messageForm.Title = "SYSTEM DEFAULTS";
         messageForm.Message = "DEFAULTS ASSIGNED ON THE NEXT SYSTEM START";

         this.SetDialogLocation(this.TriggerDefaultsButton, messageForm);
         messageForm.ShowDialog();
      }

      private void SystemResetButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         if (null != this.SystemResetHandler)
         {
            this.SystemResetHandler();
         }
      }

      private void LoggingAddressValueButton_Click(object sender, EventArgs e)
      {
         IpAddressEntryForm ipAddressEntryForm = new IpAddressEntryForm();
         this.SetDialogLocation(this.LoggingPortValueButton, ipAddressEntryForm);

         ipAddressEntryForm.Title = "LOGGING ADDRESS";
         ipAddressEntryForm.PresentValue = ParameterAccessor.Instance.Trace.Address;
         ipAddressEntryForm.DefaultValue = "127.0.0.1";

         DialogResult result = ipAddressEntryForm.ShowDialog();

         if (result == System.Windows.Forms.DialogResult.OK)
         {
            ParameterAccessor.Instance.Trace.Address = ipAddressEntryForm.EnteredValue;
            this.LoggingAddressValueButton.ValueText = ParameterAccessor.Instance.Trace.Address;
            this.SetTraceListenerDestination();
         }
      }

      private void LoggingPortValueButton_Click(object sender, EventArgs e)
      {
         NumberEntryForm numberEntryForm = new NumberEntryForm();
         this.SetDialogLocation(this.LoggingPortValueButton, numberEntryForm);

         numberEntryForm.Title = "LOGGING PORT";
         numberEntryForm.Unit = "";
         numberEntryForm.PostDecimalDigitCount = 0;
         numberEntryForm.PresentValue = ParameterAccessor.Instance.Trace.Port;
         numberEntryForm.DefaultValue = 10000; ;
         numberEntryForm.MinimumValue = 1;
         numberEntryForm.MaximumValue = 65535;

         DialogResult result = numberEntryForm.ShowDialog();

         if (result == System.Windows.Forms.DialogResult.OK)
         {
            ParameterAccessor.Instance.Trace.Port = (int)(numberEntryForm.EnteredValue);
            this.LoggingPortValueButton.ValueText = ParameterAccessor.Instance.Trace.Port.ToString();
            this.SetTraceListenerDestination();
         }
      }

      private void RobotBusHeartbeatButton_Click(object sender, EventArgs e)
      {
         RobotCommBus.Instance.TraceHB = !RobotCommBus.Instance.TraceHB;
         this.SetLogSelect(this.RobotBusHeartbeatButton, RobotCommBus.Instance.TraceHB);
      }

      private void TruckBusHeartbeatButton_Click(object sender, EventArgs e)
      {
         TruckCommBus.Instance.TraceHB = !TruckCommBus.Instance.TraceHB;
         this.SetLogSelect(this.TruckBusHeartbeatButton, TruckCommBus.Instance.TraceHB);
      }
      
      private void BackButton_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      #endregion

      #region Form Event Functions

      private void SystemStatusForm_Shown(object sender, EventArgs e)
      {
         this.UpdateStatus();
         this.ShowSettings();
         this.UpdateTimer.Enabled = true;

         this.SetLogSelect(this.RobotBusHeartbeatButton, RobotCommBus.Instance.TraceHB);
         this.SetLogSelect(this.TruckBusHeartbeatButton, TruckCommBus.Instance.TraceHB);
      }

      private void UpdateTimer_Tick(object sender, EventArgs e)
      {
         this.UpdateStatus();
         this.ShowSettings();
      }

      #endregion

      #region Constructor

      public SystemStatusForm()
      {
         this.InitializeComponent();
      }

      #endregion

   }
}
