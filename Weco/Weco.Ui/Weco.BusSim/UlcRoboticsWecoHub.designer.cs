namespace Weco.BusSim
{
   partial class UlcRoboticsWecoHub
   {
      /// <summary> 
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary> 
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.BusIdTextBox = new System.Windows.Forms.TextBox();
         this.DeviceStateLabel = new System.Windows.Forms.Label();
         this.EnabledCheckBox = new System.Windows.Forms.CheckBox();
         this.DescriptionTextBox = new System.Windows.Forms.TextBox();
         this.NodeIdTextBox = new System.Windows.Forms.TextBox();
         this.label1 = new System.Windows.Forms.Label();
         this.ErrorRegisterLabel = new System.Windows.Forms.Label();
         this.ManufacturerStatusRegisterLabel = new System.Windows.Forms.Label();
         this.PredefinedErrorField1Label = new System.Windows.Forms.Label();
         this.PredefinedErrorField2Label = new System.Windows.Forms.Label();
         this.PredefinedErrorField3Label = new System.Windows.Forms.Label();
         this.PredefinedErrorField4Label = new System.Windows.Forms.Label();
         this.CommunicationCyclePeriodLabel = new System.Windows.Forms.Label();
         this.SynchronousWindowLengthLabel = new System.Windows.Forms.Label();
         this.GuardTimeLabel = new System.Windows.Forms.Label();
         this.LifeTimeFactorLabel = new System.Windows.Forms.Label();
         this.EmergencyInhibitTimeLabel = new System.Windows.Forms.Label();
         this.CommunicationProcessImagePanel = new System.Windows.Forms.Panel();
         this.ProducerHeartbeatTimeLabel = new System.Windows.Forms.Label();
         this.ConsumerHeartbeatTimeLabel = new System.Windows.Forms.Label();
         this.CameraSelectLabel = new System.Windows.Forms.Label();
         this.CameraLedDefaultIntensityLabel = new System.Windows.Forms.Label();
         this.CameraLedIntensityLabel = new System.Windows.Forms.Label();
         this.CameraLedChannelMaskLabel = new System.Windows.Forms.Label();
         this.McuTemperatureLabel = new System.Windows.Forms.Label();
         this.McuErrorTemperatureLabel = new System.Windows.Forms.Label();
         this.OutputsLabel = new System.Windows.Forms.Label();
         this.PanMotorMotor = new Weco.BusSim.BldcMotor();
         this.TiltMotorMotor = new Weco.BusSim.BldcMotor();
         this.SetMcuTemperatureButton = new System.Windows.Forms.Button();
         this.McuTemperatureTextBox = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.MainTabControl = new System.Windows.Forms.TabControl();
         this.InterfaceTabPage = new System.Windows.Forms.TabPage();
         this.SetDcVoltageByteButton = new System.Windows.Forms.Button();
         this.label12 = new System.Windows.Forms.Label();
         this.DcVoltageByteTextBox = new System.Windows.Forms.TextBox();
         this.EmergencyTabPage = new System.Windows.Forms.TabPage();
         this.SubSystemStatusLabel = new System.Windows.Forms.Label();
         this.LedIcExcessTemperatureErrorButton = new System.Windows.Forms.Button();
         this.RearLedShortedErrorButton = new System.Windows.Forms.Button();
         this.FrontLedShortedErrorButton = new System.Windows.Forms.Button();
         this.RearLedOpenErrorButton = new System.Windows.Forms.Button();
         this.FrontLedOpenErrorButton = new System.Windows.Forms.Button();
         this.EmergencyCrcTextBox = new System.Windows.Forms.TextBox();
         this.label17 = new System.Windows.Forms.Label();
         this.AppFlashEmptyErrorButton = new System.Windows.Forms.Button();
         this.AppCrcErrorButton = new System.Windows.Forms.Button();
         this.BootCrcErrorButton = new System.Windows.Forms.Button();
         this.label16 = new System.Windows.Forms.Label();
         this.GeneralEmergencyButton = new System.Windows.Forms.Button();
         this.PanBldcTabPage = new System.Windows.Forms.TabPage();
         this.TileBldcTabPage = new System.Windows.Forms.TabPage();
         this.CommunicationTabPage = new System.Windows.Forms.TabPage();
         this.CameraProcessImageTabPage = new System.Windows.Forms.TabPage();
         this.CameraProcessImagePanel = new System.Windows.Forms.Panel();
         this.McuProcessImageTabPage = new System.Windows.Forms.TabPage();
         this.McuProcessImagePanel = new System.Windows.Forms.Panel();
         this.DcLinkVoltageByteLabel = new System.Windows.Forms.Label();
         this.EmergencyResetCheckBox = new System.Windows.Forms.CheckBox();
         this.EmergencyResetDataTextBox = new System.Windows.Forms.TextBox();
         this.label15 = new System.Windows.Forms.Label();
         this.ResetButton = new System.Windows.Forms.Button();
         this.CommunicationProcessImagePanel.SuspendLayout();
         this.MainTabControl.SuspendLayout();
         this.InterfaceTabPage.SuspendLayout();
         this.EmergencyTabPage.SuspendLayout();
         this.PanBldcTabPage.SuspendLayout();
         this.TileBldcTabPage.SuspendLayout();
         this.CommunicationTabPage.SuspendLayout();
         this.CameraProcessImageTabPage.SuspendLayout();
         this.CameraProcessImagePanel.SuspendLayout();
         this.McuProcessImageTabPage.SuspendLayout();
         this.McuProcessImagePanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // BusIdTextBox
         // 
         this.BusIdTextBox.Location = new System.Drawing.Point(245, 4);
         this.BusIdTextBox.MaxLength = 3;
         this.BusIdTextBox.Name = "BusIdTextBox";
         this.BusIdTextBox.ReadOnly = true;
         this.BusIdTextBox.Size = new System.Drawing.Size(15, 20);
         this.BusIdTextBox.TabIndex = 142;
         this.BusIdTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // DeviceStateLabel
         // 
         this.DeviceStateLabel.BackColor = System.Drawing.SystemColors.Control;
         this.DeviceStateLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.DeviceStateLabel.Location = new System.Drawing.Point(264, 4);
         this.DeviceStateLabel.Name = "DeviceStateLabel";
         this.DeviceStateLabel.Size = new System.Drawing.Size(66, 20);
         this.DeviceStateLabel.TabIndex = 141;
         this.DeviceStateLabel.Text = "OFF";
         this.DeviceStateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // EnabledCheckBox
         // 
         this.EnabledCheckBox.AutoSize = true;
         this.EnabledCheckBox.Location = new System.Drawing.Point(5, 7);
         this.EnabledCheckBox.Name = "EnabledCheckBox";
         this.EnabledCheckBox.Size = new System.Drawing.Size(15, 14);
         this.EnabledCheckBox.TabIndex = 140;
         this.EnabledCheckBox.UseVisualStyleBackColor = true;
         // 
         // DescriptionTextBox
         // 
         this.DescriptionTextBox.Location = new System.Drawing.Point(24, 4);
         this.DescriptionTextBox.MaxLength = 65535;
         this.DescriptionTextBox.Name = "DescriptionTextBox";
         this.DescriptionTextBox.Size = new System.Drawing.Size(150, 20);
         this.DescriptionTextBox.TabIndex = 139;
         this.DescriptionTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // NodeIdTextBox
         // 
         this.NodeIdTextBox.Location = new System.Drawing.Point(217, 4);
         this.NodeIdTextBox.MaxLength = 3;
         this.NodeIdTextBox.Name = "NodeIdTextBox";
         this.NodeIdTextBox.Size = new System.Drawing.Size(25, 20);
         this.NodeIdTextBox.TabIndex = 137;
         this.NodeIdTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(180, 7);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(36, 13);
         this.label1.TabIndex = 138;
         this.label1.Text = "Node:";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // ErrorRegisterLabel
         // 
         this.ErrorRegisterLabel.AutoSize = true;
         this.ErrorRegisterLabel.Location = new System.Drawing.Point(3, 3);
         this.ErrorRegisterLabel.Name = "ErrorRegisterLabel";
         this.ErrorRegisterLabel.Size = new System.Drawing.Size(109, 13);
         this.ErrorRegisterLabel.TabIndex = 144;
         this.ErrorRegisterLabel.Text = "0x1001 Error Register";
         this.ErrorRegisterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // ManufacturerStatusRegisterLabel
         // 
         this.ManufacturerStatusRegisterLabel.AutoSize = true;
         this.ManufacturerStatusRegisterLabel.Location = new System.Drawing.Point(3, 23);
         this.ManufacturerStatusRegisterLabel.Name = "ManufacturerStatusRegisterLabel";
         this.ManufacturerStatusRegisterLabel.Size = new System.Drawing.Size(183, 13);
         this.ManufacturerStatusRegisterLabel.TabIndex = 145;
         this.ManufacturerStatusRegisterLabel.Text = "0x1002 Manufacturer Status Register";
         this.ManufacturerStatusRegisterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // PredefinedErrorField1Label
         // 
         this.PredefinedErrorField1Label.AutoSize = true;
         this.PredefinedErrorField1Label.Location = new System.Drawing.Point(3, 43);
         this.PredefinedErrorField1Label.Name = "PredefinedErrorField1Label";
         this.PredefinedErrorField1Label.Size = new System.Drawing.Size(167, 13);
         this.PredefinedErrorField1Label.TabIndex = 147;
         this.PredefinedErrorField1Label.Text = "0x1003-1 Pre-defined Error Field 1";
         this.PredefinedErrorField1Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // PredefinedErrorField2Label
         // 
         this.PredefinedErrorField2Label.AutoSize = true;
         this.PredefinedErrorField2Label.Location = new System.Drawing.Point(3, 63);
         this.PredefinedErrorField2Label.Name = "PredefinedErrorField2Label";
         this.PredefinedErrorField2Label.Size = new System.Drawing.Size(167, 13);
         this.PredefinedErrorField2Label.TabIndex = 149;
         this.PredefinedErrorField2Label.Text = "0x1003-2 Pre-defined Error Field 2";
         this.PredefinedErrorField2Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // PredefinedErrorField3Label
         // 
         this.PredefinedErrorField3Label.AutoSize = true;
         this.PredefinedErrorField3Label.Location = new System.Drawing.Point(3, 83);
         this.PredefinedErrorField3Label.Name = "PredefinedErrorField3Label";
         this.PredefinedErrorField3Label.Size = new System.Drawing.Size(167, 13);
         this.PredefinedErrorField3Label.TabIndex = 150;
         this.PredefinedErrorField3Label.Text = "0x1003-3 Pre-defined Error Field 3";
         this.PredefinedErrorField3Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // PredefinedErrorField4Label
         // 
         this.PredefinedErrorField4Label.AutoSize = true;
         this.PredefinedErrorField4Label.Location = new System.Drawing.Point(3, 103);
         this.PredefinedErrorField4Label.Name = "PredefinedErrorField4Label";
         this.PredefinedErrorField4Label.Size = new System.Drawing.Size(167, 13);
         this.PredefinedErrorField4Label.TabIndex = 151;
         this.PredefinedErrorField4Label.Text = "0x1003-4 Pre-defined Error Field 4";
         this.PredefinedErrorField4Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // CommunicationCyclePeriodLabel
         // 
         this.CommunicationCyclePeriodLabel.AutoSize = true;
         this.CommunicationCyclePeriodLabel.Location = new System.Drawing.Point(3, 123);
         this.CommunicationCyclePeriodLabel.Name = "CommunicationCyclePeriodLabel";
         this.CommunicationCyclePeriodLabel.Size = new System.Drawing.Size(179, 13);
         this.CommunicationCyclePeriodLabel.TabIndex = 155;
         this.CommunicationCyclePeriodLabel.Text = "0x1006 Communication Cycle Period";
         this.CommunicationCyclePeriodLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // SynchronousWindowLengthLabel
         // 
         this.SynchronousWindowLengthLabel.AutoSize = true;
         this.SynchronousWindowLengthLabel.Location = new System.Drawing.Point(3, 143);
         this.SynchronousWindowLengthLabel.Name = "SynchronousWindowLengthLabel";
         this.SynchronousWindowLengthLabel.Size = new System.Drawing.Size(185, 13);
         this.SynchronousWindowLengthLabel.TabIndex = 157;
         this.SynchronousWindowLengthLabel.Text = "0x1007 Synchronous Window Length";
         this.SynchronousWindowLengthLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // GuardTimeLabel
         // 
         this.GuardTimeLabel.AutoSize = true;
         this.GuardTimeLabel.Location = new System.Drawing.Point(3, 163);
         this.GuardTimeLabel.Name = "GuardTimeLabel";
         this.GuardTimeLabel.Size = new System.Drawing.Size(101, 13);
         this.GuardTimeLabel.TabIndex = 159;
         this.GuardTimeLabel.Text = "0x100C Guard Time";
         this.GuardTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // LifeTimeFactorLabel
         // 
         this.LifeTimeFactorLabel.AutoSize = true;
         this.LifeTimeFactorLabel.Location = new System.Drawing.Point(3, 183);
         this.LifeTimeFactorLabel.Name = "LifeTimeFactorLabel";
         this.LifeTimeFactorLabel.Size = new System.Drawing.Size(123, 13);
         this.LifeTimeFactorLabel.TabIndex = 161;
         this.LifeTimeFactorLabel.Text = "0x100D Life Time Factor";
         this.LifeTimeFactorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // EmergencyInhibitTimeLabel
         // 
         this.EmergencyInhibitTimeLabel.AutoSize = true;
         this.EmergencyInhibitTimeLabel.Location = new System.Drawing.Point(3, 203);
         this.EmergencyInhibitTimeLabel.Name = "EmergencyInhibitTimeLabel";
         this.EmergencyInhibitTimeLabel.Size = new System.Drawing.Size(155, 13);
         this.EmergencyInhibitTimeLabel.TabIndex = 163;
         this.EmergencyInhibitTimeLabel.Text = "0x1015 Emergency Inhibit Time";
         this.EmergencyInhibitTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // CommunicationProcessImagePanel
         // 
         this.CommunicationProcessImagePanel.AutoScroll = true;
         this.CommunicationProcessImagePanel.BackColor = System.Drawing.Color.Gainsboro;
         this.CommunicationProcessImagePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.CommunicationProcessImagePanel.Controls.Add(this.ProducerHeartbeatTimeLabel);
         this.CommunicationProcessImagePanel.Controls.Add(this.ConsumerHeartbeatTimeLabel);
         this.CommunicationProcessImagePanel.Controls.Add(this.ManufacturerStatusRegisterLabel);
         this.CommunicationProcessImagePanel.Controls.Add(this.ErrorRegisterLabel);
         this.CommunicationProcessImagePanel.Controls.Add(this.EmergencyInhibitTimeLabel);
         this.CommunicationProcessImagePanel.Controls.Add(this.LifeTimeFactorLabel);
         this.CommunicationProcessImagePanel.Controls.Add(this.PredefinedErrorField1Label);
         this.CommunicationProcessImagePanel.Controls.Add(this.GuardTimeLabel);
         this.CommunicationProcessImagePanel.Controls.Add(this.PredefinedErrorField2Label);
         this.CommunicationProcessImagePanel.Controls.Add(this.PredefinedErrorField3Label);
         this.CommunicationProcessImagePanel.Controls.Add(this.SynchronousWindowLengthLabel);
         this.CommunicationProcessImagePanel.Controls.Add(this.PredefinedErrorField4Label);
         this.CommunicationProcessImagePanel.Controls.Add(this.CommunicationCyclePeriodLabel);
         this.CommunicationProcessImagePanel.Location = new System.Drawing.Point(6, 6);
         this.CommunicationProcessImagePanel.Name = "CommunicationProcessImagePanel";
         this.CommunicationProcessImagePanel.Size = new System.Drawing.Size(298, 108);
         this.CommunicationProcessImagePanel.TabIndex = 165;
         this.CommunicationProcessImagePanel.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ProcessImagePanel_Scroll);
         // 
         // ProducerHeartbeatTimeLabel
         // 
         this.ProducerHeartbeatTimeLabel.AutoSize = true;
         this.ProducerHeartbeatTimeLabel.Location = new System.Drawing.Point(3, 243);
         this.ProducerHeartbeatTimeLabel.Name = "ProducerHeartbeatTimeLabel";
         this.ProducerHeartbeatTimeLabel.Size = new System.Drawing.Size(164, 13);
         this.ProducerHeartbeatTimeLabel.TabIndex = 167;
         this.ProducerHeartbeatTimeLabel.Text = "0x1017 Producer Heartbeat Time";
         this.ProducerHeartbeatTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // ConsumerHeartbeatTimeLabel
         // 
         this.ConsumerHeartbeatTimeLabel.AutoSize = true;
         this.ConsumerHeartbeatTimeLabel.Location = new System.Drawing.Point(3, 223);
         this.ConsumerHeartbeatTimeLabel.Name = "ConsumerHeartbeatTimeLabel";
         this.ConsumerHeartbeatTimeLabel.Size = new System.Drawing.Size(168, 13);
         this.ConsumerHeartbeatTimeLabel.TabIndex = 165;
         this.ConsumerHeartbeatTimeLabel.Text = "0x1016 Consumer Heartbeat Time";
         this.ConsumerHeartbeatTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // CameraSelectLabel
         // 
         this.CameraSelectLabel.AutoSize = true;
         this.CameraSelectLabel.Location = new System.Drawing.Point(3, 3);
         this.CameraSelectLabel.Name = "CameraSelectLabel";
         this.CameraSelectLabel.Size = new System.Drawing.Size(114, 13);
         this.CameraSelectLabel.TabIndex = 166;
         this.CameraSelectLabel.Text = "0x2301 Camera Select";
         this.CameraSelectLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // CameraLedDefaultIntensityLabel
         // 
         this.CameraLedDefaultIntensityLabel.AutoSize = true;
         this.CameraLedDefaultIntensityLabel.Location = new System.Drawing.Point(3, 23);
         this.CameraLedDefaultIntensityLabel.Name = "CameraLedDefaultIntensityLabel";
         this.CameraLedDefaultIntensityLabel.Size = new System.Drawing.Size(193, 13);
         this.CameraLedDefaultIntensityLabel.TabIndex = 168;
         this.CameraLedDefaultIntensityLabel.Text = "0x2302-1 Camera LED Default Intensity";
         this.CameraLedDefaultIntensityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // CameraLedIntensityLabel
         // 
         this.CameraLedIntensityLabel.AutoSize = true;
         this.CameraLedIntensityLabel.Location = new System.Drawing.Point(3, 43);
         this.CameraLedIntensityLabel.Name = "CameraLedIntensityLabel";
         this.CameraLedIntensityLabel.Size = new System.Drawing.Size(156, 13);
         this.CameraLedIntensityLabel.TabIndex = 170;
         this.CameraLedIntensityLabel.Text = "0x2303-1 Camera LED Intensity";
         this.CameraLedIntensityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // CameraLedChannelMaskLabel
         // 
         this.CameraLedChannelMaskLabel.AutoSize = true;
         this.CameraLedChannelMaskLabel.Location = new System.Drawing.Point(3, 63);
         this.CameraLedChannelMaskLabel.Name = "CameraLedChannelMaskLabel";
         this.CameraLedChannelMaskLabel.Size = new System.Drawing.Size(185, 13);
         this.CameraLedChannelMaskLabel.TabIndex = 172;
         this.CameraLedChannelMaskLabel.Text = "0x2304-1 Camera LED Channel Mask";
         this.CameraLedChannelMaskLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // McuTemperatureLabel
         // 
         this.McuTemperatureLabel.AutoSize = true;
         this.McuTemperatureLabel.Location = new System.Drawing.Point(3, 28);
         this.McuTemperatureLabel.Name = "McuTemperatureLabel";
         this.McuTemperatureLabel.Size = new System.Drawing.Size(141, 13);
         this.McuTemperatureLabel.TabIndex = 216;
         this.McuTemperatureLabel.Text = "0x2311-1 MCU Temperature";
         this.McuTemperatureLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // McuErrorTemperatureLabel
         // 
         this.McuErrorTemperatureLabel.AutoSize = true;
         this.McuErrorTemperatureLabel.Location = new System.Drawing.Point(3, 48);
         this.McuErrorTemperatureLabel.Name = "McuErrorTemperatureLabel";
         this.McuErrorTemperatureLabel.Size = new System.Drawing.Size(166, 13);
         this.McuErrorTemperatureLabel.TabIndex = 217;
         this.McuErrorTemperatureLabel.Text = "0x2311-2 MCU Error Temperature";
         this.McuErrorTemperatureLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // OutputsLabel
         // 
         this.OutputsLabel.AutoSize = true;
         this.OutputsLabel.Location = new System.Drawing.Point(3, 3);
         this.OutputsLabel.Name = "OutputsLabel";
         this.OutputsLabel.Size = new System.Drawing.Size(91, 13);
         this.OutputsLabel.TabIndex = 220;
         this.OutputsLabel.Text = "0x2310-1 Outputs";
         this.OutputsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // PanMotorMotor
         // 
         this.PanMotorMotor.AccelerationDimensionIndexLocation = 0;
         this.PanMotorMotor.AccelerationNotationIndexLocation = 0;
         this.PanMotorMotor.AutoHomeEnabled = false;
         this.PanMotorMotor.ControlWordLocation = 0;
         this.PanMotorMotor.CurrentActualValueLocation = 0;
         this.PanMotorMotor.DigitalInputsLocation = 0;
         this.PanMotorMotor.DigitalOutputsHighestLocation = 0;
         this.PanMotorMotor.DigitalOutputsLocation = 0;
         this.PanMotorMotor.DigitalOutputsMaskLocation = 0;
         this.PanMotorMotor.FeedConstantFeedLocation = 0;
         this.PanMotorMotor.FeedConstantHighestLocation = 0;
         this.PanMotorMotor.FeedConstantShaftRevolutionsLocation = 0;
         this.PanMotorMotor.GearRatioHighestLocation = 0;
         this.PanMotorMotor.GearRatioMotorRevolutionsLocation = 0;
         this.PanMotorMotor.GearRatioShaftRevolutionsLocation = 0;
         this.PanMotorMotor.GetModeLocation = 0;
         this.PanMotorMotor.HomeOffsetLocation = 0;
         this.PanMotorMotor.HomingAccelerationLocation = 0;
         this.PanMotorMotor.HomingMethodLocation = 0;
         this.PanMotorMotor.HomingSpeedHighestLocation = 0;
         this.PanMotorMotor.HomingSwitchSpeedLocation = 0;
         this.PanMotorMotor.HomingZeroSpeedLocation = 0;
         this.PanMotorMotor.Location = new System.Drawing.Point(8, 6);
         this.PanMotorMotor.MaximumCurrentLocation = 0;
         this.PanMotorMotor.MotorAbortConnectionOptionLocation = 0;
         this.PanMotorMotor.MotorErrorCodeLocation = 0;
         this.PanMotorMotor.MotorErrorTemperatureLocation = 0;
         this.PanMotorMotor.MotorPeakCurrentLimitLocation = 0;
         this.PanMotorMotor.MotorRatedCurrentLocation = 0;
         this.PanMotorMotor.MotorSupportedDriveModesLocation = 0;
         this.PanMotorMotor.MotorTemperatureHighestLocation = 0;
         this.PanMotorMotor.MotorTemperatureLocation = 0;
         this.PanMotorMotor.MotorTypeLocation = 0;
         this.PanMotorMotor.Name = "PanMotorMotor";
         this.PanMotorMotor.OnStoreError = null;
         this.PanMotorMotor.OnTpdoCheck = null;
         this.PanMotorMotor.PolarityLocation = 0;
         this.PanMotorMotor.PositionActualValueLocation = 0;
         this.PanMotorMotor.PositionControlParameterHighestLocation = 0;
         this.PanMotorMotor.PositionDerivativeGainCoefficientKdLocation = 0;
         this.PanMotorMotor.PositionEncoderIncrementsLocation = 0;
         this.PanMotorMotor.PositionEncoderMotorRevolutionsLocation = 0;
         this.PanMotorMotor.PositionEncoderResolutionHighestLocation = 0;
         this.PanMotorMotor.PositionIntegralGainCoefficienKiLocation = 0;
         this.PanMotorMotor.PositionNotationIndexLocation = 0;
         this.PanMotorMotor.PositionProportionalGainCoefficientKpLocation = 0;
         this.PanMotorMotor.PositionWindowLocation = 0;
         this.PanMotorMotor.PositionWindowTimeLocation = 0;
         this.PanMotorMotor.ProfileAccelerationLocation = 0;
         this.PanMotorMotor.ProfileDecelerationLocation = 0;
         this.PanMotorMotor.ProfileVelocityLocation = 0;
         this.PanMotorMotor.SetModeLocation = 0;
         this.PanMotorMotor.SingleDeviceTypeLocation = 0;
         this.PanMotorMotor.Size = new System.Drawing.Size(486, 109);
         this.PanMotorMotor.StatusWordLocation = 0;
         this.PanMotorMotor.SubSystemIndex = 0;
         this.PanMotorMotor.SupportHomingMode = false;
         this.PanMotorMotor.SupportPositionMode = false;
         this.PanMotorMotor.SupportVelocityMode = false;
         this.PanMotorMotor.TabIndex = 222;
         this.PanMotorMotor.TargetPositionLocation = 0;
         this.PanMotorMotor.TargetTorqueLocation = 0;
         this.PanMotorMotor.TargetVelocityLocation = 0;
         this.PanMotorMotor.VelocityActualValueLocation = 0;
         this.PanMotorMotor.VelocityControlParameterHighestLocation = 0;
         this.PanMotorMotor.VelocityDerivativeGainCoefficientKdLocation = 0;
         this.PanMotorMotor.VelocityDimensionIndexLocation = 0;
         this.PanMotorMotor.VelocityEncoderIncrementsPerSecondLocation = 0;
         this.PanMotorMotor.VelocityEncoderResolutionHighestLocation = 0;
         this.PanMotorMotor.VelocityEncoderRevolutionsPerSecondLocation = 0;
         this.PanMotorMotor.VelocityIntegralGainCoefficienKiLocation = 0;
         this.PanMotorMotor.VelocityNotationIndexLocation = 0;
         this.PanMotorMotor.VelocityProportionalGainCoefficientKpLocation = 0;
         this.PanMotorMotor.VelocityThresholdLocation = 0;
         this.PanMotorMotor.VelocityThresholdTimeLocation = 0;
         this.PanMotorMotor.VelocityWindowLocation = 0;
         this.PanMotorMotor.VelocityWindowTimeLocation = 0;
         // 
         // TiltMotorMotor
         // 
         this.TiltMotorMotor.AccelerationDimensionIndexLocation = 0;
         this.TiltMotorMotor.AccelerationNotationIndexLocation = 0;
         this.TiltMotorMotor.AutoHomeEnabled = false;
         this.TiltMotorMotor.ControlWordLocation = 0;
         this.TiltMotorMotor.CurrentActualValueLocation = 0;
         this.TiltMotorMotor.DigitalInputsLocation = 0;
         this.TiltMotorMotor.DigitalOutputsHighestLocation = 0;
         this.TiltMotorMotor.DigitalOutputsLocation = 0;
         this.TiltMotorMotor.DigitalOutputsMaskLocation = 0;
         this.TiltMotorMotor.FeedConstantFeedLocation = 0;
         this.TiltMotorMotor.FeedConstantHighestLocation = 0;
         this.TiltMotorMotor.FeedConstantShaftRevolutionsLocation = 0;
         this.TiltMotorMotor.GearRatioHighestLocation = 0;
         this.TiltMotorMotor.GearRatioMotorRevolutionsLocation = 0;
         this.TiltMotorMotor.GearRatioShaftRevolutionsLocation = 0;
         this.TiltMotorMotor.GetModeLocation = 0;
         this.TiltMotorMotor.HomeOffsetLocation = 0;
         this.TiltMotorMotor.HomingAccelerationLocation = 0;
         this.TiltMotorMotor.HomingMethodLocation = 0;
         this.TiltMotorMotor.HomingSpeedHighestLocation = 0;
         this.TiltMotorMotor.HomingSwitchSpeedLocation = 0;
         this.TiltMotorMotor.HomingZeroSpeedLocation = 0;
         this.TiltMotorMotor.Location = new System.Drawing.Point(6, 6);
         this.TiltMotorMotor.MaximumCurrentLocation = 0;
         this.TiltMotorMotor.MotorAbortConnectionOptionLocation = 0;
         this.TiltMotorMotor.MotorErrorCodeLocation = 0;
         this.TiltMotorMotor.MotorErrorTemperatureLocation = 0;
         this.TiltMotorMotor.MotorPeakCurrentLimitLocation = 0;
         this.TiltMotorMotor.MotorRatedCurrentLocation = 0;
         this.TiltMotorMotor.MotorSupportedDriveModesLocation = 0;
         this.TiltMotorMotor.MotorTemperatureHighestLocation = 0;
         this.TiltMotorMotor.MotorTemperatureLocation = 0;
         this.TiltMotorMotor.MotorTypeLocation = 0;
         this.TiltMotorMotor.Name = "TiltMotorMotor";
         this.TiltMotorMotor.OnStoreError = null;
         this.TiltMotorMotor.OnTpdoCheck = null;
         this.TiltMotorMotor.PolarityLocation = 0;
         this.TiltMotorMotor.PositionActualValueLocation = 0;
         this.TiltMotorMotor.PositionControlParameterHighestLocation = 0;
         this.TiltMotorMotor.PositionDerivativeGainCoefficientKdLocation = 0;
         this.TiltMotorMotor.PositionEncoderIncrementsLocation = 0;
         this.TiltMotorMotor.PositionEncoderMotorRevolutionsLocation = 0;
         this.TiltMotorMotor.PositionEncoderResolutionHighestLocation = 0;
         this.TiltMotorMotor.PositionIntegralGainCoefficienKiLocation = 0;
         this.TiltMotorMotor.PositionNotationIndexLocation = 0;
         this.TiltMotorMotor.PositionProportionalGainCoefficientKpLocation = 0;
         this.TiltMotorMotor.PositionWindowLocation = 0;
         this.TiltMotorMotor.PositionWindowTimeLocation = 0;
         this.TiltMotorMotor.ProfileAccelerationLocation = 0;
         this.TiltMotorMotor.ProfileDecelerationLocation = 0;
         this.TiltMotorMotor.ProfileVelocityLocation = 0;
         this.TiltMotorMotor.SetModeLocation = 0;
         this.TiltMotorMotor.SingleDeviceTypeLocation = 0;
         this.TiltMotorMotor.Size = new System.Drawing.Size(486, 108);
         this.TiltMotorMotor.StatusWordLocation = 0;
         this.TiltMotorMotor.SubSystemIndex = 0;
         this.TiltMotorMotor.SupportHomingMode = false;
         this.TiltMotorMotor.SupportPositionMode = false;
         this.TiltMotorMotor.SupportVelocityMode = false;
         this.TiltMotorMotor.TabIndex = 223;
         this.TiltMotorMotor.TargetPositionLocation = 0;
         this.TiltMotorMotor.TargetTorqueLocation = 0;
         this.TiltMotorMotor.TargetVelocityLocation = 0;
         this.TiltMotorMotor.VelocityActualValueLocation = 0;
         this.TiltMotorMotor.VelocityControlParameterHighestLocation = 0;
         this.TiltMotorMotor.VelocityDerivativeGainCoefficientKdLocation = 0;
         this.TiltMotorMotor.VelocityDimensionIndexLocation = 0;
         this.TiltMotorMotor.VelocityEncoderIncrementsPerSecondLocation = 0;
         this.TiltMotorMotor.VelocityEncoderResolutionHighestLocation = 0;
         this.TiltMotorMotor.VelocityEncoderRevolutionsPerSecondLocation = 0;
         this.TiltMotorMotor.VelocityIntegralGainCoefficienKiLocation = 0;
         this.TiltMotorMotor.VelocityNotationIndexLocation = 0;
         this.TiltMotorMotor.VelocityProportionalGainCoefficientKpLocation = 0;
         this.TiltMotorMotor.VelocityThresholdLocation = 0;
         this.TiltMotorMotor.VelocityThresholdTimeLocation = 0;
         this.TiltMotorMotor.VelocityWindowLocation = 0;
         this.TiltMotorMotor.VelocityWindowTimeLocation = 0;
         // 
         // SetMcuTemperatureButton
         // 
         this.SetMcuTemperatureButton.Location = new System.Drawing.Point(135, 6);
         this.SetMcuTemperatureButton.Name = "SetMcuTemperatureButton";
         this.SetMcuTemperatureButton.Size = new System.Drawing.Size(35, 23);
         this.SetMcuTemperatureButton.TabIndex = 228;
         this.SetMcuTemperatureButton.Text = "Set";
         this.SetMcuTemperatureButton.UseVisualStyleBackColor = true;
         this.SetMcuTemperatureButton.Click += new System.EventHandler(this.SetMcuTemperatureButton_Click);
         // 
         // McuTemperatureTextBox
         // 
         this.McuTemperatureTextBox.Location = new System.Drawing.Point(104, 8);
         this.McuTemperatureTextBox.MaxLength = 0;
         this.McuTemperatureTextBox.Name = "McuTemperatureTextBox";
         this.McuTemperatureTextBox.Size = new System.Drawing.Size(25, 20);
         this.McuTemperatureTextBox.TabIndex = 227;
         this.McuTemperatureTextBox.Text = "23";
         this.McuTemperatureTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(8, 11);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(94, 13);
         this.label2.TabIndex = 226;
         this.label2.Text = "MCU Temperature";
         // 
         // MainTabControl
         // 
         this.MainTabControl.Controls.Add(this.InterfaceTabPage);
         this.MainTabControl.Controls.Add(this.EmergencyTabPage);
         this.MainTabControl.Controls.Add(this.PanBldcTabPage);
         this.MainTabControl.Controls.Add(this.TileBldcTabPage);
         this.MainTabControl.Controls.Add(this.CommunicationTabPage);
         this.MainTabControl.Controls.Add(this.CameraProcessImageTabPage);
         this.MainTabControl.Controls.Add(this.McuProcessImageTabPage);
         this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.MainTabControl.Location = new System.Drawing.Point(0, 28);
         this.MainTabControl.Name = "MainTabControl";
         this.MainTabControl.SelectedIndex = 0;
         this.MainTabControl.Size = new System.Drawing.Size(1005, 169);
         this.MainTabControl.TabIndex = 243;
         // 
         // InterfaceTabPage
         // 
         this.InterfaceTabPage.BackColor = System.Drawing.Color.Gainsboro;
         this.InterfaceTabPage.Controls.Add(this.SetDcVoltageByteButton);
         this.InterfaceTabPage.Controls.Add(this.label12);
         this.InterfaceTabPage.Controls.Add(this.DcVoltageByteTextBox);
         this.InterfaceTabPage.Controls.Add(this.SetMcuTemperatureButton);
         this.InterfaceTabPage.Controls.Add(this.label2);
         this.InterfaceTabPage.Controls.Add(this.McuTemperatureTextBox);
         this.InterfaceTabPage.Location = new System.Drawing.Point(4, 22);
         this.InterfaceTabPage.Name = "InterfaceTabPage";
         this.InterfaceTabPage.Padding = new System.Windows.Forms.Padding(3);
         this.InterfaceTabPage.Size = new System.Drawing.Size(997, 143);
         this.InterfaceTabPage.TabIndex = 2;
         this.InterfaceTabPage.Text = "Interface";
         // 
         // SetDcVoltageByteButton
         // 
         this.SetDcVoltageByteButton.Location = new System.Drawing.Point(135, 31);
         this.SetDcVoltageByteButton.Name = "SetDcVoltageByteButton";
         this.SetDcVoltageByteButton.Size = new System.Drawing.Size(35, 23);
         this.SetDcVoltageByteButton.TabIndex = 252;
         this.SetDcVoltageByteButton.Text = "Set";
         this.SetDcVoltageByteButton.UseVisualStyleBackColor = true;
         this.SetDcVoltageByteButton.Click += new System.EventHandler(this.SetDcVoltageByteButton_Click);
         // 
         // label12
         // 
         this.label12.AutoSize = true;
         this.label12.Location = new System.Drawing.Point(17, 36);
         this.label12.Name = "label12";
         this.label12.Size = new System.Drawing.Size(85, 13);
         this.label12.TabIndex = 250;
         this.label12.Text = "DC Voltage Byte";
         // 
         // DcVoltageByteTextBox
         // 
         this.DcVoltageByteTextBox.Location = new System.Drawing.Point(104, 33);
         this.DcVoltageByteTextBox.MaxLength = 0;
         this.DcVoltageByteTextBox.Name = "DcVoltageByteTextBox";
         this.DcVoltageByteTextBox.Size = new System.Drawing.Size(25, 20);
         this.DcVoltageByteTextBox.TabIndex = 251;
         this.DcVoltageByteTextBox.Text = "0";
         this.DcVoltageByteTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // EmergencyTabPage
         // 
         this.EmergencyTabPage.BackColor = System.Drawing.Color.Gainsboro;
         this.EmergencyTabPage.Controls.Add(this.SubSystemStatusLabel);
         this.EmergencyTabPage.Controls.Add(this.LedIcExcessTemperatureErrorButton);
         this.EmergencyTabPage.Controls.Add(this.RearLedShortedErrorButton);
         this.EmergencyTabPage.Controls.Add(this.FrontLedShortedErrorButton);
         this.EmergencyTabPage.Controls.Add(this.RearLedOpenErrorButton);
         this.EmergencyTabPage.Controls.Add(this.FrontLedOpenErrorButton);
         this.EmergencyTabPage.Controls.Add(this.EmergencyCrcTextBox);
         this.EmergencyTabPage.Controls.Add(this.label17);
         this.EmergencyTabPage.Controls.Add(this.AppFlashEmptyErrorButton);
         this.EmergencyTabPage.Controls.Add(this.AppCrcErrorButton);
         this.EmergencyTabPage.Controls.Add(this.BootCrcErrorButton);
         this.EmergencyTabPage.Controls.Add(this.label16);
         this.EmergencyTabPage.Controls.Add(this.GeneralEmergencyButton);
         this.EmergencyTabPage.Location = new System.Drawing.Point(4, 22);
         this.EmergencyTabPage.Name = "EmergencyTabPage";
         this.EmergencyTabPage.Padding = new System.Windows.Forms.Padding(3);
         this.EmergencyTabPage.Size = new System.Drawing.Size(997, 204);
         this.EmergencyTabPage.TabIndex = 12;
         this.EmergencyTabPage.Text = "Emergency";
         // 
         // SubSystemStatusLabel
         // 
         this.SubSystemStatusLabel.AutoSize = true;
         this.SubSystemStatusLabel.Location = new System.Drawing.Point(122, 69);
         this.SubSystemStatusLabel.Name = "SubSystemStatusLabel";
         this.SubSystemStatusLabel.Size = new System.Drawing.Size(134, 13);
         this.SubSystemStatusLabel.TabIndex = 288;
         this.SubSystemStatusLabel.Text = "0x5000 Sub System Status";
         this.SubSystemStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // LedIcExcessTemperatureErrorButton
         // 
         this.LedIcExcessTemperatureErrorButton.Location = new System.Drawing.Point(464, 8);
         this.LedIcExcessTemperatureErrorButton.Name = "LedIcExcessTemperatureErrorButton";
         this.LedIcExcessTemperatureErrorButton.Size = new System.Drawing.Size(105, 23);
         this.LedIcExcessTemperatureErrorButton.TabIndex = 287;
         this.LedIcExcessTemperatureErrorButton.Text = "LED Excess Temp";
         this.LedIcExcessTemperatureErrorButton.UseVisualStyleBackColor = true;
         this.LedIcExcessTemperatureErrorButton.Click += new System.EventHandler(this.LedIcExcessTemperatureErrorButton_Click);
         // 
         // RearLedShortedErrorButton
         // 
         this.RearLedShortedErrorButton.Location = new System.Drawing.Point(240, 37);
         this.RearLedShortedErrorButton.Name = "RearLedShortedErrorButton";
         this.RearLedShortedErrorButton.Size = new System.Drawing.Size(105, 23);
         this.RearLedShortedErrorButton.TabIndex = 285;
         this.RearLedShortedErrorButton.Text = "Rear LED Shorted";
         this.RearLedShortedErrorButton.UseVisualStyleBackColor = true;
         this.RearLedShortedErrorButton.Click += new System.EventHandler(this.RearLedShortedErrorButton_Click);
         // 
         // FrontLedShortedErrorButton
         // 
         this.FrontLedShortedErrorButton.Location = new System.Drawing.Point(241, 8);
         this.FrontLedShortedErrorButton.Name = "FrontLedShortedErrorButton";
         this.FrontLedShortedErrorButton.Size = new System.Drawing.Size(105, 23);
         this.FrontLedShortedErrorButton.TabIndex = 284;
         this.FrontLedShortedErrorButton.Text = "Front LED Shorted";
         this.FrontLedShortedErrorButton.UseVisualStyleBackColor = true;
         this.FrontLedShortedErrorButton.Click += new System.EventHandler(this.FrontLedShortedErrorButton_Click);
         // 
         // RearLedOpenErrorButton
         // 
         this.RearLedOpenErrorButton.Location = new System.Drawing.Point(352, 37);
         this.RearLedOpenErrorButton.Name = "RearLedOpenErrorButton";
         this.RearLedOpenErrorButton.Size = new System.Drawing.Size(105, 23);
         this.RearLedOpenErrorButton.TabIndex = 282;
         this.RearLedOpenErrorButton.Text = "Rear LED Open";
         this.RearLedOpenErrorButton.UseVisualStyleBackColor = true;
         this.RearLedOpenErrorButton.Click += new System.EventHandler(this.RearLedOpenErrorButton_Click);
         // 
         // FrontLedOpenErrorButton
         // 
         this.FrontLedOpenErrorButton.Location = new System.Drawing.Point(353, 8);
         this.FrontLedOpenErrorButton.Name = "FrontLedOpenErrorButton";
         this.FrontLedOpenErrorButton.Size = new System.Drawing.Size(105, 23);
         this.FrontLedOpenErrorButton.TabIndex = 281;
         this.FrontLedOpenErrorButton.Text = "Front LED Open";
         this.FrontLedOpenErrorButton.UseVisualStyleBackColor = true;
         this.FrontLedOpenErrorButton.Click += new System.EventHandler(this.FrontLedOpenErrorButton_Click);
         // 
         // EmergencyCrcTextBox
         // 
         this.EmergencyCrcTextBox.Location = new System.Drawing.Point(153, 8);
         this.EmergencyCrcTextBox.MaxLength = 8;
         this.EmergencyCrcTextBox.Name = "EmergencyCrcTextBox";
         this.EmergencyCrcTextBox.Size = new System.Drawing.Size(59, 20);
         this.EmergencyCrcTextBox.TabIndex = 275;
         this.EmergencyCrcTextBox.Text = "00000000";
         this.EmergencyCrcTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label17
         // 
         this.label17.AutoSize = true;
         this.label17.Location = new System.Drawing.Point(210, 8);
         this.label17.Name = "label17";
         this.label17.Size = new System.Drawing.Size(13, 13);
         this.label17.TabIndex = 280;
         this.label17.Text = "h";
         // 
         // AppFlashEmptyErrorButton
         // 
         this.AppFlashEmptyErrorButton.Location = new System.Drawing.Point(6, 93);
         this.AppFlashEmptyErrorButton.Name = "AppFlashEmptyErrorButton";
         this.AppFlashEmptyErrorButton.Size = new System.Drawing.Size(105, 23);
         this.AppFlashEmptyErrorButton.TabIndex = 279;
         this.AppFlashEmptyErrorButton.Text = "App Flash Empty";
         this.AppFlashEmptyErrorButton.UseVisualStyleBackColor = true;
         this.AppFlashEmptyErrorButton.Click += new System.EventHandler(this.AppFlashEmptyErrorButton_Click);
         // 
         // AppCrcErrorButton
         // 
         this.AppCrcErrorButton.Location = new System.Drawing.Point(6, 64);
         this.AppCrcErrorButton.Name = "AppCrcErrorButton";
         this.AppCrcErrorButton.Size = new System.Drawing.Size(105, 23);
         this.AppCrcErrorButton.TabIndex = 278;
         this.AppCrcErrorButton.Text = "App CRC Error";
         this.AppCrcErrorButton.UseVisualStyleBackColor = true;
         this.AppCrcErrorButton.Click += new System.EventHandler(this.AppCrcErrorButton_Click);
         // 
         // BootCrcErrorButton
         // 
         this.BootCrcErrorButton.Location = new System.Drawing.Point(6, 35);
         this.BootCrcErrorButton.Name = "BootCrcErrorButton";
         this.BootCrcErrorButton.Size = new System.Drawing.Size(105, 23);
         this.BootCrcErrorButton.TabIndex = 277;
         this.BootCrcErrorButton.Text = "Boot CRC Error";
         this.BootCrcErrorButton.UseVisualStyleBackColor = true;
         this.BootCrcErrorButton.Click += new System.EventHandler(this.BootCrcErrorButton_Click);
         // 
         // label16
         // 
         this.label16.AutoSize = true;
         this.label16.Location = new System.Drawing.Point(122, 11);
         this.label16.Name = "label16";
         this.label16.Size = new System.Drawing.Size(29, 13);
         this.label16.TabIndex = 276;
         this.label16.Text = "CRC";
         this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // GeneralEmergencyButton
         // 
         this.GeneralEmergencyButton.Location = new System.Drawing.Point(6, 6);
         this.GeneralEmergencyButton.Name = "GeneralEmergencyButton";
         this.GeneralEmergencyButton.Size = new System.Drawing.Size(105, 23);
         this.GeneralEmergencyButton.TabIndex = 267;
         this.GeneralEmergencyButton.Text = "General";
         this.GeneralEmergencyButton.UseVisualStyleBackColor = true;
         this.GeneralEmergencyButton.Click += new System.EventHandler(this.GeneralEmergencyButton_Click);
         // 
         // PanBldcTabPage
         // 
         this.PanBldcTabPage.BackColor = System.Drawing.Color.Gainsboro;
         this.PanBldcTabPage.Controls.Add(this.PanMotorMotor);
         this.PanBldcTabPage.Location = new System.Drawing.Point(4, 22);
         this.PanBldcTabPage.Name = "PanBldcTabPage";
         this.PanBldcTabPage.Padding = new System.Windows.Forms.Padding(3);
         this.PanBldcTabPage.Size = new System.Drawing.Size(997, 204);
         this.PanBldcTabPage.TabIndex = 3;
         this.PanBldcTabPage.Text = "Pan BLDC";
         // 
         // TileBldcTabPage
         // 
         this.TileBldcTabPage.BackColor = System.Drawing.Color.Gainsboro;
         this.TileBldcTabPage.Controls.Add(this.TiltMotorMotor);
         this.TileBldcTabPage.Location = new System.Drawing.Point(4, 22);
         this.TileBldcTabPage.Name = "TileBldcTabPage";
         this.TileBldcTabPage.Padding = new System.Windows.Forms.Padding(3);
         this.TileBldcTabPage.Size = new System.Drawing.Size(997, 204);
         this.TileBldcTabPage.TabIndex = 4;
         this.TileBldcTabPage.Text = "Tilt BLDC";
         // 
         // CommunicationTabPage
         // 
         this.CommunicationTabPage.BackColor = System.Drawing.Color.Gainsboro;
         this.CommunicationTabPage.Controls.Add(this.CommunicationProcessImagePanel);
         this.CommunicationTabPage.Location = new System.Drawing.Point(4, 22);
         this.CommunicationTabPage.Name = "CommunicationTabPage";
         this.CommunicationTabPage.Padding = new System.Windows.Forms.Padding(3);
         this.CommunicationTabPage.Size = new System.Drawing.Size(997, 204);
         this.CommunicationTabPage.TabIndex = 0;
         this.CommunicationTabPage.Text = "Communication PDOs";
         // 
         // CameraProcessImageTabPage
         // 
         this.CameraProcessImageTabPage.BackColor = System.Drawing.Color.Gainsboro;
         this.CameraProcessImageTabPage.Controls.Add(this.CameraProcessImagePanel);
         this.CameraProcessImageTabPage.Location = new System.Drawing.Point(4, 22);
         this.CameraProcessImageTabPage.Name = "CameraProcessImageTabPage";
         this.CameraProcessImageTabPage.Padding = new System.Windows.Forms.Padding(3);
         this.CameraProcessImageTabPage.Size = new System.Drawing.Size(997, 204);
         this.CameraProcessImageTabPage.TabIndex = 7;
         this.CameraProcessImageTabPage.Text = "Camera PDOs";
         // 
         // CameraProcessImagePanel
         // 
         this.CameraProcessImagePanel.AutoScroll = true;
         this.CameraProcessImagePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.CameraProcessImagePanel.Controls.Add(this.CameraSelectLabel);
         this.CameraProcessImagePanel.Controls.Add(this.CameraLedDefaultIntensityLabel);
         this.CameraProcessImagePanel.Controls.Add(this.CameraLedIntensityLabel);
         this.CameraProcessImagePanel.Controls.Add(this.CameraLedChannelMaskLabel);
         this.CameraProcessImagePanel.Location = new System.Drawing.Point(6, 6);
         this.CameraProcessImagePanel.Name = "CameraProcessImagePanel";
         this.CameraProcessImagePanel.Size = new System.Drawing.Size(260, 101);
         this.CameraProcessImagePanel.TabIndex = 0;
         this.CameraProcessImagePanel.Scroll += new System.Windows.Forms.ScrollEventHandler(this.CameraProcessImagePanel_Scroll);
         // 
         // McuProcessImageTabPage
         // 
         this.McuProcessImageTabPage.BackColor = System.Drawing.Color.Gainsboro;
         this.McuProcessImageTabPage.Controls.Add(this.McuProcessImagePanel);
         this.McuProcessImageTabPage.Location = new System.Drawing.Point(4, 22);
         this.McuProcessImageTabPage.Name = "McuProcessImageTabPage";
         this.McuProcessImageTabPage.Padding = new System.Windows.Forms.Padding(3);
         this.McuProcessImageTabPage.Size = new System.Drawing.Size(997, 204);
         this.McuProcessImageTabPage.TabIndex = 11;
         this.McuProcessImageTabPage.Text = "MCU PDOs";
         // 
         // McuProcessImagePanel
         // 
         this.McuProcessImagePanel.AutoScroll = true;
         this.McuProcessImagePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.McuProcessImagePanel.Controls.Add(this.DcLinkVoltageByteLabel);
         this.McuProcessImagePanel.Controls.Add(this.McuTemperatureLabel);
         this.McuProcessImagePanel.Controls.Add(this.OutputsLabel);
         this.McuProcessImagePanel.Controls.Add(this.McuErrorTemperatureLabel);
         this.McuProcessImagePanel.Location = new System.Drawing.Point(6, 6);
         this.McuProcessImagePanel.Name = "McuProcessImagePanel";
         this.McuProcessImagePanel.Size = new System.Drawing.Size(432, 135);
         this.McuProcessImagePanel.TabIndex = 0;
         this.McuProcessImagePanel.Scroll += new System.Windows.Forms.ScrollEventHandler(this.McuProcessImagePanel_Scroll);
         // 
         // DcLinkVoltageByteLabel
         // 
         this.DcLinkVoltageByteLabel.AutoSize = true;
         this.DcLinkVoltageByteLabel.Location = new System.Drawing.Point(3, 73);
         this.DcLinkVoltageByteLabel.Name = "DcLinkVoltageByteLabel";
         this.DcLinkVoltageByteLabel.Size = new System.Drawing.Size(146, 13);
         this.DcLinkVoltageByteLabel.TabIndex = 221;
         this.DcLinkVoltageByteLabel.Text = "0x2000 DC Link Voltage Byte";
         this.DcLinkVoltageByteLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // EmergencyResetCheckBox
         // 
         this.EmergencyResetCheckBox.AutoSize = true;
         this.EmergencyResetCheckBox.Location = new System.Drawing.Point(449, 6);
         this.EmergencyResetCheckBox.Name = "EmergencyResetCheckBox";
         this.EmergencyResetCheckBox.Size = new System.Drawing.Size(125, 17);
         this.EmergencyResetCheckBox.TabIndex = 259;
         this.EmergencyResetCheckBox.Text = "Emergency on Reset";
         this.EmergencyResetCheckBox.UseVisualStyleBackColor = true;
         // 
         // EmergencyResetDataTextBox
         // 
         this.EmergencyResetDataTextBox.Location = new System.Drawing.Point(417, 4);
         this.EmergencyResetDataTextBox.MaxLength = 5;
         this.EmergencyResetDataTextBox.Name = "EmergencyResetDataTextBox";
         this.EmergencyResetDataTextBox.Size = new System.Drawing.Size(26, 20);
         this.EmergencyResetDataTextBox.TabIndex = 266;
         this.EmergencyResetDataTextBox.Text = "0";
         this.EmergencyResetDataTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label15
         // 
         this.label15.AutoSize = true;
         this.label15.Location = new System.Drawing.Point(336, 7);
         this.label15.Name = "label15";
         this.label15.Size = new System.Drawing.Size(79, 13);
         this.label15.TabIndex = 266;
         this.label15.Text = "Additional Data";
         // 
         // ResetButton
         // 
         this.ResetButton.Location = new System.Drawing.Point(580, 3);
         this.ResetButton.Name = "ResetButton";
         this.ResetButton.Size = new System.Drawing.Size(47, 23);
         this.ResetButton.TabIndex = 266;
         this.ResetButton.Text = "Reset";
         this.ResetButton.UseVisualStyleBackColor = true;
         this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
         // 
         // UlcRoboticsWecoHub
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.Gainsboro;
         this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.Controls.Add(this.ResetButton);
         this.Controls.Add(this.label15);
         this.Controls.Add(this.EmergencyResetDataTextBox);
         this.Controls.Add(this.EmergencyResetCheckBox);
         this.Controls.Add(this.MainTabControl);
         this.Controls.Add(this.BusIdTextBox);
         this.Controls.Add(this.DeviceStateLabel);
         this.Controls.Add(this.EnabledCheckBox);
         this.Controls.Add(this.DescriptionTextBox);
         this.Controls.Add(this.NodeIdTextBox);
         this.Controls.Add(this.label1);
         this.Name = "UlcRoboticsWecoHub";
         this.Size = new System.Drawing.Size(1005, 197);
         this.CommunicationProcessImagePanel.ResumeLayout(false);
         this.CommunicationProcessImagePanel.PerformLayout();
         this.MainTabControl.ResumeLayout(false);
         this.InterfaceTabPage.ResumeLayout(false);
         this.InterfaceTabPage.PerformLayout();
         this.EmergencyTabPage.ResumeLayout(false);
         this.EmergencyTabPage.PerformLayout();
         this.PanBldcTabPage.ResumeLayout(false);
         this.TileBldcTabPage.ResumeLayout(false);
         this.CommunicationTabPage.ResumeLayout(false);
         this.CameraProcessImageTabPage.ResumeLayout(false);
         this.CameraProcessImagePanel.ResumeLayout(false);
         this.CameraProcessImagePanel.PerformLayout();
         this.McuProcessImageTabPage.ResumeLayout(false);
         this.McuProcessImagePanel.ResumeLayout(false);
         this.McuProcessImagePanel.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.TextBox BusIdTextBox;
      private System.Windows.Forms.Label DeviceStateLabel;
      private System.Windows.Forms.CheckBox EnabledCheckBox;
      private System.Windows.Forms.TextBox DescriptionTextBox;
      private System.Windows.Forms.TextBox NodeIdTextBox;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label ErrorRegisterLabel;
      private System.Windows.Forms.Label ManufacturerStatusRegisterLabel;
      private System.Windows.Forms.Label PredefinedErrorField1Label;
      private System.Windows.Forms.Label PredefinedErrorField2Label;
      private System.Windows.Forms.Label PredefinedErrorField3Label;
      private System.Windows.Forms.Label PredefinedErrorField4Label;
      private System.Windows.Forms.Label CommunicationCyclePeriodLabel;
      private System.Windows.Forms.Label SynchronousWindowLengthLabel;
      private System.Windows.Forms.Label GuardTimeLabel;
      private System.Windows.Forms.Label LifeTimeFactorLabel;
      private System.Windows.Forms.Label EmergencyInhibitTimeLabel;
      private System.Windows.Forms.Panel CommunicationProcessImagePanel;
      private System.Windows.Forms.Label ConsumerHeartbeatTimeLabel;
      private System.Windows.Forms.Label ProducerHeartbeatTimeLabel;
      private System.Windows.Forms.Label CameraSelectLabel;
      private System.Windows.Forms.Label CameraLedDefaultIntensityLabel;
      private System.Windows.Forms.Label CameraLedIntensityLabel;
      private System.Windows.Forms.Label CameraLedChannelMaskLabel;
      private System.Windows.Forms.Label McuTemperatureLabel;
      private System.Windows.Forms.Label McuErrorTemperatureLabel;
      private System.Windows.Forms.Label OutputsLabel;
      private BldcMotor PanMotorMotor;
      private BldcMotor TiltMotorMotor;
      private System.Windows.Forms.Button SetMcuTemperatureButton;
      private System.Windows.Forms.TextBox McuTemperatureTextBox;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TabControl MainTabControl;
      private System.Windows.Forms.TabPage InterfaceTabPage;
      private System.Windows.Forms.TabPage CommunicationTabPage;
      private System.Windows.Forms.TabPage PanBldcTabPage;
      private System.Windows.Forms.TabPage TileBldcTabPage;
      private System.Windows.Forms.TabPage CameraProcessImageTabPage;
      private System.Windows.Forms.Panel CameraProcessImagePanel;
      private System.Windows.Forms.TabPage McuProcessImageTabPage;
      private System.Windows.Forms.Panel McuProcessImagePanel;
      private System.Windows.Forms.Label DcLinkVoltageByteLabel;
      private System.Windows.Forms.Button SetDcVoltageByteButton;
      private System.Windows.Forms.Label label12;
      private System.Windows.Forms.TextBox DcVoltageByteTextBox;
      private System.Windows.Forms.CheckBox EmergencyResetCheckBox;
      private System.Windows.Forms.TabPage EmergencyTabPage;
      private System.Windows.Forms.TextBox EmergencyResetDataTextBox;
      private System.Windows.Forms.Label label15;
      private System.Windows.Forms.Button ResetButton;
      private System.Windows.Forms.Button GeneralEmergencyButton;
      private System.Windows.Forms.Button AppCrcErrorButton;
      private System.Windows.Forms.Button BootCrcErrorButton;
      private System.Windows.Forms.Label label16;
      private System.Windows.Forms.TextBox EmergencyCrcTextBox;
      private System.Windows.Forms.Button AppFlashEmptyErrorButton;
      private System.Windows.Forms.Label label17;
      private System.Windows.Forms.Button LedIcExcessTemperatureErrorButton;
      private System.Windows.Forms.Button RearLedShortedErrorButton;
      private System.Windows.Forms.Button FrontLedShortedErrorButton;
      private System.Windows.Forms.Button RearLedOpenErrorButton;
      private System.Windows.Forms.Button FrontLedOpenErrorButton;
      private System.Windows.Forms.Label SubSystemStatusLabel;
   }
}
