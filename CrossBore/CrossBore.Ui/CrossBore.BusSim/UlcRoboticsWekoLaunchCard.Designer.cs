
namespace CrossBore.BusSim
{
   partial class UlcRoboticsWekoLaunchCard
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
         this.McuTemperatureLabel = new System.Windows.Forms.Label();
         this.LedIntensity2Label = new System.Windows.Forms.Label();
         this.CameraSelectLabel = new System.Windows.Forms.Label();
         this.LedIntensity1Label = new System.Windows.Forms.Label();
         this.LedIntensity3Label = new System.Windows.Forms.Label();
         this.LedIntensity4Label = new System.Windows.Forms.Label();
         this.LedIntensity6Label = new System.Windows.Forms.Label();
         this.LedIntensity5Label = new System.Windows.Forms.Label();
         this.SetMcuTemperatureButton = new System.Windows.Forms.Button();
         this.label6 = new System.Windows.Forms.Label();
         this.McuTemperatureTextBox = new System.Windows.Forms.TextBox();
         this.SubSystemStatusLabel = new System.Windows.Forms.Label();
         this.EmergencyCrcTextBox = new System.Windows.Forms.TextBox();
         this.label17 = new System.Windows.Forms.Label();
         this.AppFlashEmptyErrorButton = new System.Windows.Forms.Button();
         this.AppCrcErrorButton = new System.Windows.Forms.Button();
         this.BootCrcErrorButton = new System.Windows.Forms.Button();
         this.label16 = new System.Windows.Forms.Label();
         this.GeneralEmergencyButton = new System.Windows.Forms.Button();
         this.LedIcExcessTemperatureErrorButton = new System.Windows.Forms.Button();
         this.LedShortedErrorButton = new System.Windows.Forms.Button();
         this.LedOpenErrorButton = new System.Windows.Forms.Button();
         this.label7 = new System.Windows.Forms.Label();
         this.LedIdTextBox = new System.Windows.Forms.TextBox();
         this.PdoPanel = new System.Windows.Forms.Panel();
         this.McuErrorTemperatureLabel = new System.Windows.Forms.Label();
         this.ProducerHeartbeatTimeLabel = new System.Windows.Forms.Label();
         this.ConsumerHeartbeatTimeLabel = new System.Windows.Forms.Label();
         this.ResetButton = new System.Windows.Forms.Button();
         this.label15 = new System.Windows.Forms.Label();
         this.EmergencyResetDataTextBox = new System.Windows.Forms.TextBox();
         this.EmergencyResetCheckBox = new System.Windows.Forms.CheckBox();
         this.PdoPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // BusIdTextBox
         // 
         this.BusIdTextBox.Location = new System.Drawing.Point(245, 4);
         this.BusIdTextBox.MaxLength = 3;
         this.BusIdTextBox.Name = "BusIdTextBox";
         this.BusIdTextBox.ReadOnly = true;
         this.BusIdTextBox.Size = new System.Drawing.Size(15, 20);
         this.BusIdTextBox.TabIndex = 125;
         this.BusIdTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // DeviceStateLabel
         // 
         this.DeviceStateLabel.BackColor = System.Drawing.SystemColors.Control;
         this.DeviceStateLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.DeviceStateLabel.Location = new System.Drawing.Point(264, 4);
         this.DeviceStateLabel.Name = "DeviceStateLabel";
         this.DeviceStateLabel.Size = new System.Drawing.Size(66, 20);
         this.DeviceStateLabel.TabIndex = 124;
         this.DeviceStateLabel.Text = "OFF";
         this.DeviceStateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // EnabledCheckBox
         // 
         this.EnabledCheckBox.AutoSize = true;
         this.EnabledCheckBox.Location = new System.Drawing.Point(5, 7);
         this.EnabledCheckBox.Name = "EnabledCheckBox";
         this.EnabledCheckBox.Size = new System.Drawing.Size(15, 14);
         this.EnabledCheckBox.TabIndex = 123;
         this.EnabledCheckBox.UseVisualStyleBackColor = true;
         // 
         // DescriptionTextBox
         // 
         this.DescriptionTextBox.Location = new System.Drawing.Point(24, 4);
         this.DescriptionTextBox.MaxLength = 65535;
         this.DescriptionTextBox.Name = "DescriptionTextBox";
         this.DescriptionTextBox.Size = new System.Drawing.Size(150, 20);
         this.DescriptionTextBox.TabIndex = 122;
         this.DescriptionTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // NodeIdTextBox
         // 
         this.NodeIdTextBox.Location = new System.Drawing.Point(217, 4);
         this.NodeIdTextBox.MaxLength = 3;
         this.NodeIdTextBox.Name = "NodeIdTextBox";
         this.NodeIdTextBox.Size = new System.Drawing.Size(25, 20);
         this.NodeIdTextBox.TabIndex = 120;
         this.NodeIdTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(180, 7);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(36, 13);
         this.label1.TabIndex = 121;
         this.label1.Text = "Node:";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // McuTemperatureLabel
         // 
         this.McuTemperatureLabel.AutoSize = true;
         this.McuTemperatureLabel.Location = new System.Drawing.Point(3, 3);
         this.McuTemperatureLabel.Name = "McuTemperatureLabel";
         this.McuTemperatureLabel.Size = new System.Drawing.Size(141, 13);
         this.McuTemperatureLabel.TabIndex = 217;
         this.McuTemperatureLabel.Text = "0x2311-1 MCU Temperature";
         this.McuTemperatureLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // LedIntensity2Label
         // 
         this.LedIntensity2Label.AutoSize = true;
         this.LedIntensity2Label.Location = new System.Drawing.Point(261, 43);
         this.LedIntensity2Label.Name = "LedIntensity2Label";
         this.LedIntensity2Label.Size = new System.Drawing.Size(126, 13);
         this.LedIntensity2Label.TabIndex = 220;
         this.LedIntensity2Label.Text = "0x2303-2 LED Intensity 2";
         this.LedIntensity2Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // CameraSelectLabel
         // 
         this.CameraSelectLabel.AutoSize = true;
         this.CameraSelectLabel.Location = new System.Drawing.Point(261, 3);
         this.CameraSelectLabel.Name = "CameraSelectLabel";
         this.CameraSelectLabel.Size = new System.Drawing.Size(114, 13);
         this.CameraSelectLabel.TabIndex = 218;
         this.CameraSelectLabel.Text = "0x2301 Camera Select";
         this.CameraSelectLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // LedIntensity1Label
         // 
         this.LedIntensity1Label.AutoSize = true;
         this.LedIntensity1Label.Location = new System.Drawing.Point(261, 23);
         this.LedIntensity1Label.Name = "LedIntensity1Label";
         this.LedIntensity1Label.Size = new System.Drawing.Size(126, 13);
         this.LedIntensity1Label.TabIndex = 219;
         this.LedIntensity1Label.Text = "0x2303-1 LED Intensity 1";
         this.LedIntensity1Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // LedIntensity3Label
         // 
         this.LedIntensity3Label.AutoSize = true;
         this.LedIntensity3Label.Location = new System.Drawing.Point(261, 63);
         this.LedIntensity3Label.Name = "LedIntensity3Label";
         this.LedIntensity3Label.Size = new System.Drawing.Size(126, 13);
         this.LedIntensity3Label.TabIndex = 221;
         this.LedIntensity3Label.Text = "0x2303-3 LED Intensity 3";
         this.LedIntensity3Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // LedIntensity4Label
         // 
         this.LedIntensity4Label.AutoSize = true;
         this.LedIntensity4Label.Location = new System.Drawing.Point(261, 83);
         this.LedIntensity4Label.Name = "LedIntensity4Label";
         this.LedIntensity4Label.Size = new System.Drawing.Size(126, 13);
         this.LedIntensity4Label.TabIndex = 222;
         this.LedIntensity4Label.Text = "0x2303-4 LED Intensity 4";
         this.LedIntensity4Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // LedIntensity6Label
         // 
         this.LedIntensity6Label.AutoSize = true;
         this.LedIntensity6Label.Location = new System.Drawing.Point(261, 123);
         this.LedIntensity6Label.Name = "LedIntensity6Label";
         this.LedIntensity6Label.Size = new System.Drawing.Size(126, 13);
         this.LedIntensity6Label.TabIndex = 224;
         this.LedIntensity6Label.Text = "0x2303-6 LED Intensity 6";
         this.LedIntensity6Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // LedIntensity5Label
         // 
         this.LedIntensity5Label.AutoSize = true;
         this.LedIntensity5Label.Location = new System.Drawing.Point(261, 103);
         this.LedIntensity5Label.Name = "LedIntensity5Label";
         this.LedIntensity5Label.Size = new System.Drawing.Size(126, 13);
         this.LedIntensity5Label.TabIndex = 223;
         this.LedIntensity5Label.Text = "0x2303-5 LED Intensity 5";
         this.LedIntensity5Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // SetMcuTemperatureButton
         // 
         this.SetMcuTemperatureButton.Location = new System.Drawing.Point(249, 117);
         this.SetMcuTemperatureButton.Name = "SetMcuTemperatureButton";
         this.SetMcuTemperatureButton.Size = new System.Drawing.Size(35, 23);
         this.SetMcuTemperatureButton.TabIndex = 231;
         this.SetMcuTemperatureButton.Text = "Set";
         this.SetMcuTemperatureButton.UseVisualStyleBackColor = true;
         this.SetMcuTemperatureButton.Click += new System.EventHandler(this.SetMcuTemperatureButton_Click);
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(122, 122);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(94, 13);
         this.label6.TabIndex = 229;
         this.label6.Text = "MCU Temperature";
         // 
         // McuTemperatureTextBox
         // 
         this.McuTemperatureTextBox.Location = new System.Drawing.Point(218, 119);
         this.McuTemperatureTextBox.MaxLength = 0;
         this.McuTemperatureTextBox.Name = "McuTemperatureTextBox";
         this.McuTemperatureTextBox.Size = new System.Drawing.Size(25, 20);
         this.McuTemperatureTextBox.TabIndex = 230;
         this.McuTemperatureTextBox.Text = "23";
         this.McuTemperatureTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // SubSystemStatusLabel
         // 
         this.SubSystemStatusLabel.AutoSize = true;
         this.SubSystemStatusLabel.Location = new System.Drawing.Point(3, 43);
         this.SubSystemStatusLabel.Name = "SubSystemStatusLabel";
         this.SubSystemStatusLabel.Size = new System.Drawing.Size(134, 13);
         this.SubSystemStatusLabel.TabIndex = 289;
         this.SubSystemStatusLabel.Text = "0x5000 Sub System Status";
         this.SubSystemStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // EmergencyCrcTextBox
         // 
         this.EmergencyCrcTextBox.Location = new System.Drawing.Point(152, 32);
         this.EmergencyCrcTextBox.MaxLength = 8;
         this.EmergencyCrcTextBox.Name = "EmergencyCrcTextBox";
         this.EmergencyCrcTextBox.Size = new System.Drawing.Size(59, 20);
         this.EmergencyCrcTextBox.TabIndex = 291;
         this.EmergencyCrcTextBox.Text = "00000000";
         this.EmergencyCrcTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label17
         // 
         this.label17.AutoSize = true;
         this.label17.Location = new System.Drawing.Point(209, 32);
         this.label17.Name = "label17";
         this.label17.Size = new System.Drawing.Size(13, 13);
         this.label17.TabIndex = 296;
         this.label17.Text = "h";
         // 
         // AppFlashEmptyErrorButton
         // 
         this.AppFlashEmptyErrorButton.Location = new System.Drawing.Point(5, 117);
         this.AppFlashEmptyErrorButton.Name = "AppFlashEmptyErrorButton";
         this.AppFlashEmptyErrorButton.Size = new System.Drawing.Size(105, 23);
         this.AppFlashEmptyErrorButton.TabIndex = 295;
         this.AppFlashEmptyErrorButton.Text = "App Flash Empty";
         this.AppFlashEmptyErrorButton.UseVisualStyleBackColor = true;
         this.AppFlashEmptyErrorButton.Click += new System.EventHandler(this.AppFlashEmptyErrorButton_Click);
         // 
         // AppCrcErrorButton
         // 
         this.AppCrcErrorButton.Location = new System.Drawing.Point(5, 88);
         this.AppCrcErrorButton.Name = "AppCrcErrorButton";
         this.AppCrcErrorButton.Size = new System.Drawing.Size(105, 23);
         this.AppCrcErrorButton.TabIndex = 294;
         this.AppCrcErrorButton.Text = "App CRC Error";
         this.AppCrcErrorButton.UseVisualStyleBackColor = true;
         this.AppCrcErrorButton.Click += new System.EventHandler(this.AppCrcErrorButton_Click);
         // 
         // BootCrcErrorButton
         // 
         this.BootCrcErrorButton.Location = new System.Drawing.Point(5, 59);
         this.BootCrcErrorButton.Name = "BootCrcErrorButton";
         this.BootCrcErrorButton.Size = new System.Drawing.Size(105, 23);
         this.BootCrcErrorButton.TabIndex = 293;
         this.BootCrcErrorButton.Text = "Boot CRC Error";
         this.BootCrcErrorButton.UseVisualStyleBackColor = true;
         this.BootCrcErrorButton.Click += new System.EventHandler(this.BootCrcErrorButton_Click);
         // 
         // label16
         // 
         this.label16.AutoSize = true;
         this.label16.Location = new System.Drawing.Point(121, 35);
         this.label16.Name = "label16";
         this.label16.Size = new System.Drawing.Size(29, 13);
         this.label16.TabIndex = 292;
         this.label16.Text = "CRC";
         this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // GeneralEmergencyButton
         // 
         this.GeneralEmergencyButton.Location = new System.Drawing.Point(5, 30);
         this.GeneralEmergencyButton.Name = "GeneralEmergencyButton";
         this.GeneralEmergencyButton.Size = new System.Drawing.Size(105, 23);
         this.GeneralEmergencyButton.TabIndex = 290;
         this.GeneralEmergencyButton.Text = "General";
         this.GeneralEmergencyButton.UseVisualStyleBackColor = true;
         this.GeneralEmergencyButton.Click += new System.EventHandler(this.GeneralEmergencyButton_Click);
         // 
         // LedIcExcessTemperatureErrorButton
         // 
         this.LedIcExcessTemperatureErrorButton.Location = new System.Drawing.Point(315, 59);
         this.LedIcExcessTemperatureErrorButton.Name = "LedIcExcessTemperatureErrorButton";
         this.LedIcExcessTemperatureErrorButton.Size = new System.Drawing.Size(60, 23);
         this.LedIcExcessTemperatureErrorButton.TabIndex = 301;
         this.LedIcExcessTemperatureErrorButton.Text = "Temp";
         this.LedIcExcessTemperatureErrorButton.UseVisualStyleBackColor = true;
         this.LedIcExcessTemperatureErrorButton.Click += new System.EventHandler(this.LedIcExcessTemperatureErrorButton_Click);
         // 
         // LedShortedErrorButton
         // 
         this.LedShortedErrorButton.Location = new System.Drawing.Point(183, 59);
         this.LedShortedErrorButton.Name = "LedShortedErrorButton";
         this.LedShortedErrorButton.Size = new System.Drawing.Size(60, 23);
         this.LedShortedErrorButton.TabIndex = 299;
         this.LedShortedErrorButton.Text = "Shorted";
         this.LedShortedErrorButton.UseVisualStyleBackColor = true;
         this.LedShortedErrorButton.Click += new System.EventHandler(this.LedShortedErrorButton_Click);
         // 
         // LedOpenErrorButton
         // 
         this.LedOpenErrorButton.Location = new System.Drawing.Point(249, 59);
         this.LedOpenErrorButton.Name = "LedOpenErrorButton";
         this.LedOpenErrorButton.Size = new System.Drawing.Size(60, 23);
         this.LedOpenErrorButton.TabIndex = 297;
         this.LedOpenErrorButton.Text = "Open";
         this.LedOpenErrorButton.UseVisualStyleBackColor = true;
         this.LedOpenErrorButton.Click += new System.EventHandler(this.LedOpenErrorButton_Click);
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(122, 64);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(28, 13);
         this.label7.TabIndex = 302;
         this.label7.Text = "LED";
         // 
         // LedIdTextBox
         // 
         this.LedIdTextBox.Location = new System.Drawing.Point(152, 61);
         this.LedIdTextBox.MaxLength = 0;
         this.LedIdTextBox.Name = "LedIdTextBox";
         this.LedIdTextBox.Size = new System.Drawing.Size(25, 20);
         this.LedIdTextBox.TabIndex = 303;
         this.LedIdTextBox.Text = "23";
         this.LedIdTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // PdoPanel
         // 
         this.PdoPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.PdoPanel.AutoScroll = true;
         this.PdoPanel.Controls.Add(this.McuErrorTemperatureLabel);
         this.PdoPanel.Controls.Add(this.ProducerHeartbeatTimeLabel);
         this.PdoPanel.Controls.Add(this.ConsumerHeartbeatTimeLabel);
         this.PdoPanel.Controls.Add(this.SubSystemStatusLabel);
         this.PdoPanel.Controls.Add(this.McuTemperatureLabel);
         this.PdoPanel.Controls.Add(this.LedIntensity1Label);
         this.PdoPanel.Controls.Add(this.CameraSelectLabel);
         this.PdoPanel.Controls.Add(this.LedIntensity2Label);
         this.PdoPanel.Controls.Add(this.LedIntensity3Label);
         this.PdoPanel.Controls.Add(this.LedIntensity4Label);
         this.PdoPanel.Controls.Add(this.LedIntensity5Label);
         this.PdoPanel.Controls.Add(this.LedIntensity6Label);
         this.PdoPanel.Location = new System.Drawing.Point(381, 30);
         this.PdoPanel.Name = "PdoPanel";
         this.PdoPanel.Size = new System.Drawing.Size(504, 113);
         this.PdoPanel.TabIndex = 304;
         // 
         // McuErrorTemperatureLabel
         // 
         this.McuErrorTemperatureLabel.AutoSize = true;
         this.McuErrorTemperatureLabel.Location = new System.Drawing.Point(3, 23);
         this.McuErrorTemperatureLabel.Name = "McuErrorTemperatureLabel";
         this.McuErrorTemperatureLabel.Size = new System.Drawing.Size(166, 13);
         this.McuErrorTemperatureLabel.TabIndex = 292;
         this.McuErrorTemperatureLabel.Text = "0x2311-2 MCU Error Temperature";
         this.McuErrorTemperatureLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // ProducerHeartbeatTimeLabel
         // 
         this.ProducerHeartbeatTimeLabel.AutoSize = true;
         this.ProducerHeartbeatTimeLabel.Location = new System.Drawing.Point(3, 83);
         this.ProducerHeartbeatTimeLabel.Name = "ProducerHeartbeatTimeLabel";
         this.ProducerHeartbeatTimeLabel.Size = new System.Drawing.Size(164, 13);
         this.ProducerHeartbeatTimeLabel.TabIndex = 291;
         this.ProducerHeartbeatTimeLabel.Text = "0x1017 Producer Heartbeat Time";
         this.ProducerHeartbeatTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // ConsumerHeartbeatTimeLabel
         // 
         this.ConsumerHeartbeatTimeLabel.AutoSize = true;
         this.ConsumerHeartbeatTimeLabel.Location = new System.Drawing.Point(3, 63);
         this.ConsumerHeartbeatTimeLabel.Name = "ConsumerHeartbeatTimeLabel";
         this.ConsumerHeartbeatTimeLabel.Size = new System.Drawing.Size(168, 13);
         this.ConsumerHeartbeatTimeLabel.TabIndex = 290;
         this.ConsumerHeartbeatTimeLabel.Text = "0x1016 Consumer Heartbeat Time";
         this.ConsumerHeartbeatTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // ResetButton
         // 
         this.ResetButton.Location = new System.Drawing.Point(580, 3);
         this.ResetButton.Name = "ResetButton";
         this.ResetButton.Size = new System.Drawing.Size(47, 23);
         this.ResetButton.TabIndex = 306;
         this.ResetButton.Text = "Reset";
         this.ResetButton.UseVisualStyleBackColor = true;
         this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
         // 
         // label15
         // 
         this.label15.AutoSize = true;
         this.label15.Location = new System.Drawing.Point(336, 7);
         this.label15.Name = "label15";
         this.label15.Size = new System.Drawing.Size(79, 13);
         this.label15.TabIndex = 307;
         this.label15.Text = "Additional Data";
         // 
         // EmergencyResetDataTextBox
         // 
         this.EmergencyResetDataTextBox.Location = new System.Drawing.Point(417, 4);
         this.EmergencyResetDataTextBox.MaxLength = 5;
         this.EmergencyResetDataTextBox.Name = "EmergencyResetDataTextBox";
         this.EmergencyResetDataTextBox.Size = new System.Drawing.Size(26, 20);
         this.EmergencyResetDataTextBox.TabIndex = 308;
         this.EmergencyResetDataTextBox.Text = "0";
         this.EmergencyResetDataTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // EmergencyResetCheckBox
         // 
         this.EmergencyResetCheckBox.AutoSize = true;
         this.EmergencyResetCheckBox.Location = new System.Drawing.Point(449, 6);
         this.EmergencyResetCheckBox.Name = "EmergencyResetCheckBox";
         this.EmergencyResetCheckBox.Size = new System.Drawing.Size(125, 17);
         this.EmergencyResetCheckBox.TabIndex = 305;
         this.EmergencyResetCheckBox.Text = "Emergency on Reset";
         this.EmergencyResetCheckBox.UseVisualStyleBackColor = true;
         // 
         // UlcRoboticsWekoLaunchCard
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.Controls.Add(this.ResetButton);
         this.Controls.Add(this.label15);
         this.Controls.Add(this.EmergencyResetDataTextBox);
         this.Controls.Add(this.EmergencyResetCheckBox);
         this.Controls.Add(this.PdoPanel);
         this.Controls.Add(this.label7);
         this.Controls.Add(this.LedIdTextBox);
         this.Controls.Add(this.LedIcExcessTemperatureErrorButton);
         this.Controls.Add(this.LedShortedErrorButton);
         this.Controls.Add(this.LedOpenErrorButton);
         this.Controls.Add(this.EmergencyCrcTextBox);
         this.Controls.Add(this.label17);
         this.Controls.Add(this.AppFlashEmptyErrorButton);
         this.Controls.Add(this.AppCrcErrorButton);
         this.Controls.Add(this.BootCrcErrorButton);
         this.Controls.Add(this.label16);
         this.Controls.Add(this.GeneralEmergencyButton);
         this.Controls.Add(this.SetMcuTemperatureButton);
         this.Controls.Add(this.label6);
         this.Controls.Add(this.McuTemperatureTextBox);
         this.Controls.Add(this.BusIdTextBox);
         this.Controls.Add(this.DeviceStateLabel);
         this.Controls.Add(this.EnabledCheckBox);
         this.Controls.Add(this.DescriptionTextBox);
         this.Controls.Add(this.NodeIdTextBox);
         this.Controls.Add(this.label1);
         this.Name = "UlcRoboticsWekoLaunchCard";
         this.Size = new System.Drawing.Size(888, 146);
         this.PdoPanel.ResumeLayout(false);
         this.PdoPanel.PerformLayout();
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
      private System.Windows.Forms.Label McuTemperatureLabel;
      private System.Windows.Forms.Label LedIntensity2Label;
      private System.Windows.Forms.Label CameraSelectLabel;
      private System.Windows.Forms.Label LedIntensity1Label;
      private System.Windows.Forms.Label LedIntensity3Label;
      private System.Windows.Forms.Label LedIntensity4Label;
      private System.Windows.Forms.Label LedIntensity6Label;
      private System.Windows.Forms.Label LedIntensity5Label;
      private System.Windows.Forms.Button SetMcuTemperatureButton;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.TextBox McuTemperatureTextBox;
      private System.Windows.Forms.Label SubSystemStatusLabel;
      private System.Windows.Forms.TextBox EmergencyCrcTextBox;
      private System.Windows.Forms.Label label17;
      private System.Windows.Forms.Button AppFlashEmptyErrorButton;
      private System.Windows.Forms.Button AppCrcErrorButton;
      private System.Windows.Forms.Button BootCrcErrorButton;
      private System.Windows.Forms.Label label16;
      private System.Windows.Forms.Button GeneralEmergencyButton;
      private System.Windows.Forms.Button LedIcExcessTemperatureErrorButton;
      private System.Windows.Forms.Button LedShortedErrorButton;
      private System.Windows.Forms.Button LedOpenErrorButton;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.TextBox LedIdTextBox;
      private System.Windows.Forms.Panel PdoPanel;
      private System.Windows.Forms.Button ResetButton;
      private System.Windows.Forms.Label label15;
      private System.Windows.Forms.TextBox EmergencyResetDataTextBox;
      private System.Windows.Forms.CheckBox EmergencyResetCheckBox;
      private System.Windows.Forms.Label ProducerHeartbeatTimeLabel;
      private System.Windows.Forms.Label ConsumerHeartbeatTimeLabel;
      private System.Windows.Forms.Label McuErrorTemperatureLabel;
   }
}
