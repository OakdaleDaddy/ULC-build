
namespace Weco.BusSim
{
   partial class UlcRoboticsWecoTrackController
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
         this.CameraLedIntensityLabel = new System.Windows.Forms.Label();
         this.McuTemperatureLabel = new System.Windows.Forms.Label();
         this.SetMcuTemperatureButton = new System.Windows.Forms.Button();
         this.McuTemperatureTextBox = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.MainTabControl = new System.Windows.Forms.TabControl();
         this.InterfaceTabPage = new System.Windows.Forms.TabPage();
         this.EmergencyTabPage = new System.Windows.Forms.TabPage();
         this.SubSystemStatusLabel = new System.Windows.Forms.Label();
         this.LedIcExcessTemperatureErrorButton = new System.Windows.Forms.Button();
         this.FrontLedShortedErrorButton = new System.Windows.Forms.Button();
         this.FrontLedOpenErrorButton = new System.Windows.Forms.Button();
         this.EmergencyCrcTextBox = new System.Windows.Forms.TextBox();
         this.label17 = new System.Windows.Forms.Label();
         this.AppFlashEmptyErrorButton = new System.Windows.Forms.Button();
         this.AppCrcErrorButton = new System.Windows.Forms.Button();
         this.BootCrcErrorButton = new System.Windows.Forms.Button();
         this.label16 = new System.Windows.Forms.Label();
         this.GeneralEmergencyButton = new System.Windows.Forms.Button();
         this.TrackMotorTabPage = new System.Windows.Forms.TabPage();
         this.TrackMotor = new Weco.BusSim.BldcMotor();
         this.CommunicationTabPage = new System.Windows.Forms.TabPage();
         this.CameraProcessImageTabPage = new System.Windows.Forms.TabPage();
         this.CameraProcessImagePanel = new System.Windows.Forms.Panel();
         this.McuProcessImageTabPage = new System.Windows.Forms.TabPage();
         this.McuProcessImagePanel = new System.Windows.Forms.Panel();
         this.EmergencyResetCheckBox = new System.Windows.Forms.CheckBox();
         this.EmergencyResetDataTextBox = new System.Windows.Forms.TextBox();
         this.label15 = new System.Windows.Forms.Label();
         this.ResetButton = new System.Windows.Forms.Button();
         this.CommunicationProcessImagePanel.SuspendLayout();
         this.MainTabControl.SuspendLayout();
         this.InterfaceTabPage.SuspendLayout();
         this.EmergencyTabPage.SuspendLayout();
         this.TrackMotorTabPage.SuspendLayout();
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
         this.CommunicationProcessImagePanel.Size = new System.Drawing.Size(298, 85);
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
         // CameraLedIntensityLabel
         // 
         this.CameraLedIntensityLabel.AutoSize = true;
         this.CameraLedIntensityLabel.Location = new System.Drawing.Point(3, 3);
         this.CameraLedIntensityLabel.Name = "CameraLedIntensityLabel";
         this.CameraLedIntensityLabel.Size = new System.Drawing.Size(156, 13);
         this.CameraLedIntensityLabel.TabIndex = 170;
         this.CameraLedIntensityLabel.Text = "0x2303-1 Camera LED Intensity";
         this.CameraLedIntensityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // McuTemperatureLabel
         // 
         this.McuTemperatureLabel.AutoSize = true;
         this.McuTemperatureLabel.Location = new System.Drawing.Point(3, 3);
         this.McuTemperatureLabel.Name = "McuTemperatureLabel";
         this.McuTemperatureLabel.Size = new System.Drawing.Size(141, 13);
         this.McuTemperatureLabel.TabIndex = 216;
         this.McuTemperatureLabel.Text = "0x2311-1 MCU Temperature";
         this.McuTemperatureLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
         this.MainTabControl.Controls.Add(this.TrackMotorTabPage);
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
         // EmergencyTabPage
         // 
         this.EmergencyTabPage.BackColor = System.Drawing.Color.Gainsboro;
         this.EmergencyTabPage.Controls.Add(this.SubSystemStatusLabel);
         this.EmergencyTabPage.Controls.Add(this.LedIcExcessTemperatureErrorButton);
         this.EmergencyTabPage.Controls.Add(this.FrontLedShortedErrorButton);
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
         this.EmergencyTabPage.Size = new System.Drawing.Size(997, 143);
         this.EmergencyTabPage.TabIndex = 12;
         this.EmergencyTabPage.Text = "Emergency";
         // 
         // SubSystemStatusLabel
         // 
         this.SubSystemStatusLabel.AutoSize = true;
         this.SubSystemStatusLabel.Location = new System.Drawing.Point(122, 40);
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
         // FrontLedShortedErrorButton
         // 
         this.FrontLedShortedErrorButton.Location = new System.Drawing.Point(241, 8);
         this.FrontLedShortedErrorButton.Name = "FrontLedShortedErrorButton";
         this.FrontLedShortedErrorButton.Size = new System.Drawing.Size(105, 23);
         this.FrontLedShortedErrorButton.TabIndex = 284;
         this.FrontLedShortedErrorButton.Text = "LED Shorted";
         this.FrontLedShortedErrorButton.UseVisualStyleBackColor = true;
         this.FrontLedShortedErrorButton.Click += new System.EventHandler(this.FrontLedShortedErrorButton_Click);
         // 
         // FrontLedOpenErrorButton
         // 
         this.FrontLedOpenErrorButton.Location = new System.Drawing.Point(353, 8);
         this.FrontLedOpenErrorButton.Name = "FrontLedOpenErrorButton";
         this.FrontLedOpenErrorButton.Size = new System.Drawing.Size(105, 23);
         this.FrontLedOpenErrorButton.TabIndex = 281;
         this.FrontLedOpenErrorButton.Text = "LED Open";
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
         // TrackMotorTabPage
         // 
         this.TrackMotorTabPage.BackColor = System.Drawing.Color.Gainsboro;
         this.TrackMotorTabPage.Controls.Add(this.TrackMotor);
         this.TrackMotorTabPage.Location = new System.Drawing.Point(4, 22);
         this.TrackMotorTabPage.Name = "TrackMotorTabPage";
         this.TrackMotorTabPage.Padding = new System.Windows.Forms.Padding(3);
         this.TrackMotorTabPage.Size = new System.Drawing.Size(997, 143);
         this.TrackMotorTabPage.TabIndex = 3;
         this.TrackMotorTabPage.Text = "Track Motor";
         // 
         // TrackMotor
         // 
         this.TrackMotor.AccelerationDimensionIndexLocation = 0;
         this.TrackMotor.AccelerationNotationIndexLocation = 0;
         this.TrackMotor.AutoHomeEnabled = false;
         this.TrackMotor.ControlWordLocation = 0;
         this.TrackMotor.CurrentActualValueLocation = 0;
         this.TrackMotor.DigitalInputsLocation = 0;
         this.TrackMotor.DigitalOutputsHighestLocation = 0;
         this.TrackMotor.DigitalOutputsLocation = 0;
         this.TrackMotor.DigitalOutputsMaskLocation = 0;
         this.TrackMotor.FeedConstantFeedLocation = 0;
         this.TrackMotor.FeedConstantHighestLocation = 0;
         this.TrackMotor.FeedConstantShaftRevolutionsLocation = 0;
         this.TrackMotor.GearRatioHighestLocation = 0;
         this.TrackMotor.GearRatioMotorRevolutionsLocation = 0;
         this.TrackMotor.GearRatioShaftRevolutionsLocation = 0;
         this.TrackMotor.GetModeLocation = 0;
         this.TrackMotor.HomeOffsetLocation = 0;
         this.TrackMotor.HomingAccelerationLocation = 0;
         this.TrackMotor.HomingMethodLocation = 0;
         this.TrackMotor.HomingSpeedHighestLocation = 0;
         this.TrackMotor.HomingSwitchSpeedLocation = 0;
         this.TrackMotor.HomingZeroSpeedLocation = 0;
         this.TrackMotor.Location = new System.Drawing.Point(3, 3);
         this.TrackMotor.MaximumCurrentLocation = 0;
         this.TrackMotor.MotorAbortConnectionOptionLocation = 0;
         this.TrackMotor.MotorErrorCodeLocation = 0;
         this.TrackMotor.MotorErrorTemperatureLocation = 0;
         this.TrackMotor.MotorPeakCurrentLimitLocation = 0;
         this.TrackMotor.MotorRatedCurrentLocation = 0;
         this.TrackMotor.MotorSupportedDriveModesLocation = 0;
         this.TrackMotor.MotorTemperatureHighestLocation = 0;
         this.TrackMotor.MotorTemperatureLocation = 0;
         this.TrackMotor.MotorTypeLocation = 0;
         this.TrackMotor.Name = "TrackMotor";
         this.TrackMotor.OnStoreError = null;
         this.TrackMotor.OnTpdoCheck = null;
         this.TrackMotor.PolarityLocation = 0;
         this.TrackMotor.PositionActualValueLocation = 0;
         this.TrackMotor.PositionControlParameterHighestLocation = 0;
         this.TrackMotor.PositionDerivativeGainCoefficientKdLocation = 0;
         this.TrackMotor.PositionEncoderIncrementsLocation = 0;
         this.TrackMotor.PositionEncoderMotorRevolutionsLocation = 0;
         this.TrackMotor.PositionEncoderResolutionHighestLocation = 0;
         this.TrackMotor.PositionIntegralGainCoefficienKiLocation = 0;
         this.TrackMotor.PositionNotationIndexLocation = 0;
         this.TrackMotor.PositionProportionalGainCoefficientKpLocation = 0;
         this.TrackMotor.PositionWindowLocation = 0;
         this.TrackMotor.PositionWindowTimeLocation = 0;
         this.TrackMotor.ProfileAccelerationLocation = 0;
         this.TrackMotor.ProfileDecelerationLocation = 0;
         this.TrackMotor.ProfileVelocityLocation = 0;
         this.TrackMotor.SetModeLocation = 0;
         this.TrackMotor.SingleDeviceTypeLocation = 0;
         this.TrackMotor.Size = new System.Drawing.Size(418, 131);
         this.TrackMotor.StatusWordLocation = 0;
         this.TrackMotor.SubSystemIndex = 0;
         this.TrackMotor.SupportHomingMode = false;
         this.TrackMotor.SupportPositionMode = false;
         this.TrackMotor.SupportVelocityMode = false;
         this.TrackMotor.TabIndex = 0;
         this.TrackMotor.TargetPositionLocation = 0;
         this.TrackMotor.TargetTorqueLocation = 0;
         this.TrackMotor.TargetVelocityLocation = 0;
         this.TrackMotor.VelocityActualValueLocation = 0;
         this.TrackMotor.VelocityControlParameterHighestLocation = 0;
         this.TrackMotor.VelocityDerivativeGainCoefficientKdLocation = 0;
         this.TrackMotor.VelocityDimensionIndexLocation = 0;
         this.TrackMotor.VelocityEncoderIncrementsPerSecondLocation = 0;
         this.TrackMotor.VelocityEncoderResolutionHighestLocation = 0;
         this.TrackMotor.VelocityEncoderRevolutionsPerSecondLocation = 0;
         this.TrackMotor.VelocityIntegralGainCoefficienKiLocation = 0;
         this.TrackMotor.VelocityNotationIndexLocation = 0;
         this.TrackMotor.VelocityProportionalGainCoefficientKpLocation = 0;
         this.TrackMotor.VelocityThresholdLocation = 0;
         this.TrackMotor.VelocityThresholdTimeLocation = 0;
         this.TrackMotor.VelocityWindowLocation = 0;
         this.TrackMotor.VelocityWindowTimeLocation = 0;
         // 
         // CommunicationTabPage
         // 
         this.CommunicationTabPage.BackColor = System.Drawing.Color.Gainsboro;
         this.CommunicationTabPage.Controls.Add(this.CommunicationProcessImagePanel);
         this.CommunicationTabPage.Location = new System.Drawing.Point(4, 22);
         this.CommunicationTabPage.Name = "CommunicationTabPage";
         this.CommunicationTabPage.Padding = new System.Windows.Forms.Padding(3);
         this.CommunicationTabPage.Size = new System.Drawing.Size(997, 143);
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
         this.CameraProcessImageTabPage.Size = new System.Drawing.Size(997, 143);
         this.CameraProcessImageTabPage.TabIndex = 7;
         this.CameraProcessImageTabPage.Text = "LED PDOs";
         // 
         // CameraProcessImagePanel
         // 
         this.CameraProcessImagePanel.AutoScroll = true;
         this.CameraProcessImagePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.CameraProcessImagePanel.Controls.Add(this.CameraLedIntensityLabel);
         this.CameraProcessImagePanel.Location = new System.Drawing.Point(6, 6);
         this.CameraProcessImagePanel.Name = "CameraProcessImagePanel";
         this.CameraProcessImagePanel.Size = new System.Drawing.Size(260, 77);
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
         this.McuProcessImageTabPage.Size = new System.Drawing.Size(997, 143);
         this.McuProcessImageTabPage.TabIndex = 11;
         this.McuProcessImageTabPage.Text = "MCU PDOs";
         // 
         // McuProcessImagePanel
         // 
         this.McuProcessImagePanel.AutoScroll = true;
         this.McuProcessImagePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.McuProcessImagePanel.Controls.Add(this.McuTemperatureLabel);
         this.McuProcessImagePanel.Location = new System.Drawing.Point(6, 6);
         this.McuProcessImagePanel.Name = "McuProcessImagePanel";
         this.McuProcessImagePanel.Size = new System.Drawing.Size(432, 102);
         this.McuProcessImagePanel.TabIndex = 0;
         this.McuProcessImagePanel.Scroll += new System.Windows.Forms.ScrollEventHandler(this.McuProcessImagePanel_Scroll);
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
         // UlcRoboticsWecoTrackController
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
         this.Name = "UlcRoboticsWecoTrackController";
         this.Size = new System.Drawing.Size(1005, 197);
         this.CommunicationProcessImagePanel.ResumeLayout(false);
         this.CommunicationProcessImagePanel.PerformLayout();
         this.MainTabControl.ResumeLayout(false);
         this.InterfaceTabPage.ResumeLayout(false);
         this.InterfaceTabPage.PerformLayout();
         this.EmergencyTabPage.ResumeLayout(false);
         this.EmergencyTabPage.PerformLayout();
         this.TrackMotorTabPage.ResumeLayout(false);
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
      private System.Windows.Forms.Label CameraLedIntensityLabel;
      private System.Windows.Forms.Label McuTemperatureLabel;
      private System.Windows.Forms.Button SetMcuTemperatureButton;
      private System.Windows.Forms.TextBox McuTemperatureTextBox;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TabControl MainTabControl;
      private System.Windows.Forms.TabPage InterfaceTabPage;
      private System.Windows.Forms.TabPage CommunicationTabPage;
      private System.Windows.Forms.TabPage TrackMotorTabPage;
      private System.Windows.Forms.TabPage CameraProcessImageTabPage;
      private System.Windows.Forms.Panel CameraProcessImagePanel;
      private System.Windows.Forms.TabPage McuProcessImageTabPage;
      private System.Windows.Forms.Panel McuProcessImagePanel;
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
      private System.Windows.Forms.Button FrontLedShortedErrorButton;
      private System.Windows.Forms.Button FrontLedOpenErrorButton;
      private System.Windows.Forms.Label SubSystemStatusLabel;
      private BldcMotor TrackMotor;
   }
}
