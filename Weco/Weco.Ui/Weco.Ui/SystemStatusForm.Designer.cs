namespace Weco.Ui
{
   partial class SystemStatusForm
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

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.components = new System.ComponentModel.Container();
         this.MainPanel = new Weco.Ui.Controls.BorderedPanel();
         this.SelectPanel = new Weco.Ui.Controls.BorderedPanel();
         this.SystemResetButton = new Weco.Ui.Controls.HoldButton();
         this.ShowToggleButton = new Weco.Ui.Controls.BaseButton();
         this.LoggingPanel = new Weco.Ui.Controls.BorderedPanel();
         this.TargetBusHeartbeatButton = new Weco.Ui.Controls.BaseButton();
         this.MainBusHeartbeatButton = new Weco.Ui.Controls.BaseButton();
         this.LoggingPortValueButton = new Weco.Ui.Controls.ValueButton();
         this.LoggingAddressValueButton = new Weco.Ui.Controls.ValueButton();
         this.label14 = new System.Windows.Forms.Label();
         this.DeviceStatusPanel = new Weco.Ui.Controls.BorderedPanel();
         this.DeviceStatusBPanel = new Weco.Ui.Controls.BorderedPanel();
         this.ReelDigitalIoStatusTextBox = new System.Windows.Forms.TextBox();
         this.ReelDigitalIoLabel = new System.Windows.Forms.Label();
         this.OsdRs232StatusTextBox = new System.Windows.Forms.TextBox();
         this.OsdRs232Label = new System.Windows.Forms.Label();
         this.ReelEncoderStatusTextBox = new System.Windows.Forms.TextBox();
         this.ReelEncoderLabel = new System.Windows.Forms.Label();
         this.ReelMotorStatusTextBox = new System.Windows.Forms.TextBox();
         this.ReelMotorLabel = new System.Windows.Forms.Label();
         this.BulletMotorStatusTextBox = new System.Windows.Forms.TextBox();
         this.BulletMotorLabel = new System.Windows.Forms.Label();
         this.FeederRightMotorStatusTextBox = new System.Windows.Forms.TextBox();
         this.FeederRightMotorLabel = new System.Windows.Forms.Label();
         this.FeederLeftMotorStatusTextBox = new System.Windows.Forms.TextBox();
         this.FeederLeftMotorLabel = new System.Windows.Forms.Label();
         this.LaunchCardAnalogIoStatusTextBox = new System.Windows.Forms.TextBox();
         this.LaunchCardAnalogIoLabel = new System.Windows.Forms.Label();
         this.LaunchCardCameraLightsStatusTextBox = new System.Windows.Forms.TextBox();
         this.LaunchCardCameraLightsLabel = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.LaunchCardControllerLabel = new System.Windows.Forms.Label();
         this.TargetBusStatusTextBox = new System.Windows.Forms.TextBox();
         this.LaunchCardControllerStatusTextBox = new System.Windows.Forms.TextBox();
         this.DeviceStatusAPanel = new Weco.Ui.Controls.BorderedPanel();
         this.HubTiltMotorStatusTextBox = new System.Windows.Forms.TextBox();
         this.HubTiltMotorLabel = new System.Windows.Forms.Label();
         this.HubPanMotorStatusTextBox = new System.Windows.Forms.TextBox();
         this.HubPanMotorLabel = new System.Windows.Forms.Label();
         this.HubCameraLightsStatusTextBox = new System.Windows.Forms.TextBox();
         this.HubCameraLightsLabel = new System.Windows.Forms.Label();
         this.RightTrackMotorStatusTextBox = new System.Windows.Forms.TextBox();
         this.RightTrackMotorLabel = new System.Windows.Forms.Label();
         this.RightTrackLightStatusTextBox = new System.Windows.Forms.TextBox();
         this.RightTrackLightLabel = new System.Windows.Forms.Label();
         this.RightTrackControllerStatusTextBox = new System.Windows.Forms.TextBox();
         this.RightTrackControllerLabel = new System.Windows.Forms.Label();
         this.LeftTrackMotorStatusTextBox = new System.Windows.Forms.TextBox();
         this.LeftTrackMotorLabel = new System.Windows.Forms.Label();
         this.LeftTrackLightStatusTextBox = new System.Windows.Forms.TextBox();
         this.LeftTrackLightLabel = new System.Windows.Forms.Label();
         this.HubControllerStatusTextBox = new System.Windows.Forms.TextBox();
         this.HubControllerLabel = new System.Windows.Forms.Label();
         this.JoystickLabel = new System.Windows.Forms.Label();
         this.RobotBusStatusTextBox = new System.Windows.Forms.TextBox();
         this.LeftTrackControllerStatusTextBox = new System.Windows.Forms.TextBox();
         this.LeftTrackControllerLabel = new System.Windows.Forms.Label();
         this.RobotBusLabel = new System.Windows.Forms.Label();
         this.JoystickStatusTextBox = new System.Windows.Forms.TextBox();
         this.ComponentStatusLabel = new System.Windows.Forms.Label();
         this.SettingsPanel = new Weco.Ui.Controls.BorderedPanel();
         this.SaveDefaultsButton = new Weco.Ui.Controls.HoldButton();
         this.TriggerDefaultsButton = new Weco.Ui.Controls.HoldButton();
         this.label15 = new System.Windows.Forms.Label();
         this.BackButton = new Weco.Ui.Controls.BaseButton();
         this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
         this.MainPanel.SuspendLayout();
         this.SelectPanel.SuspendLayout();
         this.LoggingPanel.SuspendLayout();
         this.DeviceStatusPanel.SuspendLayout();
         this.DeviceStatusBPanel.SuspendLayout();
         this.DeviceStatusAPanel.SuspendLayout();
         this.SettingsPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // MainPanel
         // 
         this.MainPanel.BackColor = System.Drawing.Color.SeaGreen;
         this.MainPanel.Controls.Add(this.SelectPanel);
         this.MainPanel.Controls.Add(this.LoggingPanel);
         this.MainPanel.Controls.Add(this.DeviceStatusPanel);
         this.MainPanel.Controls.Add(this.SettingsPanel);
         this.MainPanel.Controls.Add(this.BackButton);
         this.MainPanel.EdgeWeight = 3;
         this.MainPanel.Location = new System.Drawing.Point(0, 0);
         this.MainPanel.Name = "MainPanel";
         this.MainPanel.Size = new System.Drawing.Size(1781, 743);
         this.MainPanel.TabIndex = 0;
         // 
         // SelectPanel
         // 
         this.SelectPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
         this.SelectPanel.Controls.Add(this.SystemResetButton);
         this.SelectPanel.Controls.Add(this.ShowToggleButton);
         this.SelectPanel.EdgeWeight = 3;
         this.SelectPanel.Location = new System.Drawing.Point(1490, 505);
         this.SelectPanel.Name = "SelectPanel";
         this.SelectPanel.Size = new System.Drawing.Size(275, 89);
         this.SelectPanel.TabIndex = 8;
         // 
         // SystemResetButton
         // 
         this.SystemResetButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SystemResetButton.DisabledBackColor = System.Drawing.SystemColors.ControlDarkDark;
         this.SystemResetButton.DisabledForeColor = System.Drawing.Color.Gray;
         this.SystemResetButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.SystemResetButton.ForeColor = System.Drawing.Color.Black;
         this.SystemResetButton.HoldArrorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.SystemResetButton.HoldTimeoutEnable = true;
         this.SystemResetButton.HoldTimeoutInterval = 100;
         this.SystemResetButton.Location = new System.Drawing.Point(142, 12);
         this.SystemResetButton.Name = "SystemResetButton";
         this.SystemResetButton.Size = new System.Drawing.Size(107, 67);
         this.SystemResetButton.TabIndex = 205;
         this.SystemResetButton.Text = "SYSTEM RESET";
         this.SystemResetButton.UseVisualStyleBackColor = false;
         this.SystemResetButton.HoldTimeout += new Weco.Ui.Controls.HoldTimeoutHandler(this.SystemResetButton_HoldTimeout);
         // 
         // ShowToggleButton
         // 
         this.ShowToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ShowToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ShowToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ShowToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.ShowToggleButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.ShowToggleButton.Location = new System.Drawing.Point(27, 12);
         this.ShowToggleButton.Name = "ShowToggleButton";
         this.ShowToggleButton.Size = new System.Drawing.Size(107, 67);
         this.ShowToggleButton.TabIndex = 204;
         this.ShowToggleButton.Text = "SHOW VERSION";
         this.ShowToggleButton.UseVisualStyleBackColor = false;
         this.ShowToggleButton.Click += new System.EventHandler(this.ShowToggleButton_Click);
         // 
         // LoggingPanel
         // 
         this.LoggingPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
         this.LoggingPanel.Controls.Add(this.TargetBusHeartbeatButton);
         this.LoggingPanel.Controls.Add(this.MainBusHeartbeatButton);
         this.LoggingPanel.Controls.Add(this.LoggingPortValueButton);
         this.LoggingPanel.Controls.Add(this.LoggingAddressValueButton);
         this.LoggingPanel.Controls.Add(this.label14);
         this.LoggingPanel.EdgeWeight = 3;
         this.LoggingPanel.Location = new System.Drawing.Point(1490, 154);
         this.LoggingPanel.Name = "LoggingPanel";
         this.LoggingPanel.Size = new System.Drawing.Size(275, 205);
         this.LoggingPanel.TabIndex = 7;
         // 
         // TargetBusHeartbeatButton
         // 
         this.TargetBusHeartbeatButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.TargetBusHeartbeatButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.TargetBusHeartbeatButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.TargetBusHeartbeatButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.TargetBusHeartbeatButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.TargetBusHeartbeatButton.Location = new System.Drawing.Point(142, 122);
         this.TargetBusHeartbeatButton.Name = "TargetBusHeartbeatButton";
         this.TargetBusHeartbeatButton.Size = new System.Drawing.Size(107, 67);
         this.TargetBusHeartbeatButton.TabIndex = 204;
         this.TargetBusHeartbeatButton.Text = "TARGET      BUS HB";
         this.TargetBusHeartbeatButton.UseVisualStyleBackColor = false;
         this.TargetBusHeartbeatButton.Click += new System.EventHandler(this.TargetBusHeartbeatButton_Click);
         // 
         // MainBusHeartbeatButton
         // 
         this.MainBusHeartbeatButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.MainBusHeartbeatButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.MainBusHeartbeatButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.MainBusHeartbeatButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.MainBusHeartbeatButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.MainBusHeartbeatButton.Location = new System.Drawing.Point(27, 122);
         this.MainBusHeartbeatButton.Name = "MainBusHeartbeatButton";
         this.MainBusHeartbeatButton.Size = new System.Drawing.Size(107, 67);
         this.MainBusHeartbeatButton.TabIndex = 203;
         this.MainBusHeartbeatButton.Text = "MAIN      BUS HB";
         this.MainBusHeartbeatButton.UseVisualStyleBackColor = false;
         this.MainBusHeartbeatButton.Click += new System.EventHandler(this.MainBusHeartbeatButton_Click);
         // 
         // LoggingPortValueButton
         // 
         this.LoggingPortValueButton.ArrowWidth = 0;
         this.LoggingPortValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.LoggingPortValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.LoggingPortValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.LoggingPortValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.LoggingPortValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.LoggingPortValueButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.LoggingPortValueButton.HoldTimeoutInterval = 0;
         this.LoggingPortValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.LoggingPortValueButton.LeftArrowVisible = false;
         this.LoggingPortValueButton.Location = new System.Drawing.Point(142, 47);
         this.LoggingPortValueButton.Name = "LoggingPortValueButton";
         this.LoggingPortValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.LoggingPortValueButton.RightArrowVisible = false;
         this.LoggingPortValueButton.Size = new System.Drawing.Size(107, 67);
         this.LoggingPortValueButton.TabIndex = 202;
         this.LoggingPortValueButton.Text = "PORT";
         this.LoggingPortValueButton.UseVisualStyleBackColor = false;
         this.LoggingPortValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.LoggingPortValueButton.ValueEdgeHeight = 8;
         this.LoggingPortValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
         this.LoggingPortValueButton.ValueForeColor = System.Drawing.Color.White;
         this.LoggingPortValueButton.ValueHeight = 22;
         this.LoggingPortValueButton.ValueText = "65535";
         this.LoggingPortValueButton.ValueWidth = 65;
         this.LoggingPortValueButton.Click += new System.EventHandler(this.LoggingPortValueButton_Click);
         // 
         // LoggingAddressValueButton
         // 
         this.LoggingAddressValueButton.ArrowWidth = 0;
         this.LoggingAddressValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.LoggingAddressValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.LoggingAddressValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.LoggingAddressValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.LoggingAddressValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.LoggingAddressValueButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.LoggingAddressValueButton.HoldTimeoutInterval = 0;
         this.LoggingAddressValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.LoggingAddressValueButton.LeftArrowVisible = false;
         this.LoggingAddressValueButton.Location = new System.Drawing.Point(27, 47);
         this.LoggingAddressValueButton.Name = "LoggingAddressValueButton";
         this.LoggingAddressValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.LoggingAddressValueButton.RightArrowVisible = false;
         this.LoggingAddressValueButton.Size = new System.Drawing.Size(107, 67);
         this.LoggingAddressValueButton.TabIndex = 201;
         this.LoggingAddressValueButton.Text = "ADDRESS";
         this.LoggingAddressValueButton.UseVisualStyleBackColor = false;
         this.LoggingAddressValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.LoggingAddressValueButton.ValueEdgeHeight = 8;
         this.LoggingAddressValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
         this.LoggingAddressValueButton.ValueForeColor = System.Drawing.Color.White;
         this.LoggingAddressValueButton.ValueHeight = 22;
         this.LoggingAddressValueButton.ValueText = "255.255.255.255";
         this.LoggingAddressValueButton.ValueWidth = 97;
         this.LoggingAddressValueButton.Click += new System.EventHandler(this.LoggingAddressValueButton_Click);
         // 
         // label14
         // 
         this.label14.BackColor = System.Drawing.Color.Teal;
         this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label14.ForeColor = System.Drawing.Color.White;
         this.label14.Location = new System.Drawing.Point(16, 16);
         this.label14.Name = "label14";
         this.label14.Size = new System.Drawing.Size(243, 23);
         this.label14.TabIndex = 200;
         this.label14.Text = "LOGGING";
         this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // DeviceStatusPanel
         // 
         this.DeviceStatusPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
         this.DeviceStatusPanel.Controls.Add(this.DeviceStatusBPanel);
         this.DeviceStatusPanel.Controls.Add(this.DeviceStatusAPanel);
         this.DeviceStatusPanel.Controls.Add(this.ComponentStatusLabel);
         this.DeviceStatusPanel.EdgeWeight = 3;
         this.DeviceStatusPanel.Location = new System.Drawing.Point(16, 16);
         this.DeviceStatusPanel.Name = "DeviceStatusPanel";
         this.DeviceStatusPanel.Size = new System.Drawing.Size(1466, 711);
         this.DeviceStatusPanel.TabIndex = 6;
         // 
         // DeviceStatusBPanel
         // 
         this.DeviceStatusBPanel.Controls.Add(this.ReelDigitalIoStatusTextBox);
         this.DeviceStatusBPanel.Controls.Add(this.ReelDigitalIoLabel);
         this.DeviceStatusBPanel.Controls.Add(this.OsdRs232StatusTextBox);
         this.DeviceStatusBPanel.Controls.Add(this.OsdRs232Label);
         this.DeviceStatusBPanel.Controls.Add(this.ReelEncoderStatusTextBox);
         this.DeviceStatusBPanel.Controls.Add(this.ReelEncoderLabel);
         this.DeviceStatusBPanel.Controls.Add(this.ReelMotorStatusTextBox);
         this.DeviceStatusBPanel.Controls.Add(this.ReelMotorLabel);
         this.DeviceStatusBPanel.Controls.Add(this.BulletMotorStatusTextBox);
         this.DeviceStatusBPanel.Controls.Add(this.BulletMotorLabel);
         this.DeviceStatusBPanel.Controls.Add(this.FeederRightMotorStatusTextBox);
         this.DeviceStatusBPanel.Controls.Add(this.FeederRightMotorLabel);
         this.DeviceStatusBPanel.Controls.Add(this.FeederLeftMotorStatusTextBox);
         this.DeviceStatusBPanel.Controls.Add(this.FeederLeftMotorLabel);
         this.DeviceStatusBPanel.Controls.Add(this.LaunchCardAnalogIoStatusTextBox);
         this.DeviceStatusBPanel.Controls.Add(this.LaunchCardAnalogIoLabel);
         this.DeviceStatusBPanel.Controls.Add(this.LaunchCardCameraLightsStatusTextBox);
         this.DeviceStatusBPanel.Controls.Add(this.LaunchCardCameraLightsLabel);
         this.DeviceStatusBPanel.Controls.Add(this.label1);
         this.DeviceStatusBPanel.Controls.Add(this.LaunchCardControllerLabel);
         this.DeviceStatusBPanel.Controls.Add(this.TargetBusStatusTextBox);
         this.DeviceStatusBPanel.Controls.Add(this.LaunchCardControllerStatusTextBox);
         this.DeviceStatusBPanel.EdgeWeight = 1;
         this.DeviceStatusBPanel.Location = new System.Drawing.Point(737, 47);
         this.DeviceStatusBPanel.Name = "DeviceStatusBPanel";
         this.DeviceStatusBPanel.Size = new System.Drawing.Size(705, 648);
         this.DeviceStatusBPanel.TabIndex = 203;
         // 
         // ReelDigitalIoStatusTextBox
         // 
         this.ReelDigitalIoStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.ReelDigitalIoStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.ReelDigitalIoStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.ReelDigitalIoStatusTextBox.Location = new System.Drawing.Point(317, 326);
         this.ReelDigitalIoStatusTextBox.Name = "ReelDigitalIoStatusTextBox";
         this.ReelDigitalIoStatusTextBox.ReadOnly = true;
         this.ReelDigitalIoStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.ReelDigitalIoStatusTextBox.TabIndex = 250;
         this.ReelDigitalIoStatusTextBox.Text = "not connected";
         this.ReelDigitalIoStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // ReelDigitalIoLabel
         // 
         this.ReelDigitalIoLabel.BackColor = System.Drawing.Color.Teal;
         this.ReelDigitalIoLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.ReelDigitalIoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ReelDigitalIoLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.ReelDigitalIoLabel.Location = new System.Drawing.Point(8, 328);
         this.ReelDigitalIoLabel.Name = "ReelDigitalIoLabel";
         this.ReelDigitalIoLabel.Size = new System.Drawing.Size(301, 23);
         this.ReelDigitalIoLabel.TabIndex = 249;
         this.ReelDigitalIoLabel.Text = "REEL DIGITAL IO";
         this.ReelDigitalIoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.ReelDigitalIoLabel.Click += new System.EventHandler(this.ReelDigitalIoLabel_Click);
         // 
         // OsdRs232StatusTextBox
         // 
         this.OsdRs232StatusTextBox.BackColor = System.Drawing.Color.Red;
         this.OsdRs232StatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.OsdRs232StatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.OsdRs232StatusTextBox.Location = new System.Drawing.Point(317, 358);
         this.OsdRs232StatusTextBox.Name = "OsdRs232StatusTextBox";
         this.OsdRs232StatusTextBox.ReadOnly = true;
         this.OsdRs232StatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.OsdRs232StatusTextBox.TabIndex = 248;
         this.OsdRs232StatusTextBox.Text = "not connected";
         this.OsdRs232StatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // OsdRs232Label
         // 
         this.OsdRs232Label.BackColor = System.Drawing.Color.Teal;
         this.OsdRs232Label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.OsdRs232Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.OsdRs232Label.ForeColor = System.Drawing.Color.Gainsboro;
         this.OsdRs232Label.Location = new System.Drawing.Point(8, 360);
         this.OsdRs232Label.Name = "OsdRs232Label";
         this.OsdRs232Label.Size = new System.Drawing.Size(301, 23);
         this.OsdRs232Label.TabIndex = 247;
         this.OsdRs232Label.Text = "OSD RS232";
         this.OsdRs232Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.OsdRs232Label.Click += new System.EventHandler(this.OsdRs232Label_Click);
         // 
         // ReelEncoderStatusTextBox
         // 
         this.ReelEncoderStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.ReelEncoderStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.ReelEncoderStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.ReelEncoderStatusTextBox.Location = new System.Drawing.Point(317, 294);
         this.ReelEncoderStatusTextBox.Name = "ReelEncoderStatusTextBox";
         this.ReelEncoderStatusTextBox.ReadOnly = true;
         this.ReelEncoderStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.ReelEncoderStatusTextBox.TabIndex = 246;
         this.ReelEncoderStatusTextBox.Text = "not connected";
         this.ReelEncoderStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // ReelEncoderLabel
         // 
         this.ReelEncoderLabel.BackColor = System.Drawing.Color.Teal;
         this.ReelEncoderLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.ReelEncoderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ReelEncoderLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.ReelEncoderLabel.Location = new System.Drawing.Point(8, 296);
         this.ReelEncoderLabel.Name = "ReelEncoderLabel";
         this.ReelEncoderLabel.Size = new System.Drawing.Size(301, 23);
         this.ReelEncoderLabel.TabIndex = 245;
         this.ReelEncoderLabel.Text = "REEL ENCODER";
         this.ReelEncoderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.ReelEncoderLabel.Click += new System.EventHandler(this.ReelEncoderLabel_Click);
         // 
         // ReelMotorStatusTextBox
         // 
         this.ReelMotorStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.ReelMotorStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.ReelMotorStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.ReelMotorStatusTextBox.Location = new System.Drawing.Point(317, 262);
         this.ReelMotorStatusTextBox.Name = "ReelMotorStatusTextBox";
         this.ReelMotorStatusTextBox.ReadOnly = true;
         this.ReelMotorStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.ReelMotorStatusTextBox.TabIndex = 244;
         this.ReelMotorStatusTextBox.Text = "not connected";
         this.ReelMotorStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // ReelMotorLabel
         // 
         this.ReelMotorLabel.BackColor = System.Drawing.Color.Teal;
         this.ReelMotorLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.ReelMotorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ReelMotorLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.ReelMotorLabel.Location = new System.Drawing.Point(8, 264);
         this.ReelMotorLabel.Name = "ReelMotorLabel";
         this.ReelMotorLabel.Size = new System.Drawing.Size(301, 23);
         this.ReelMotorLabel.TabIndex = 243;
         this.ReelMotorLabel.Text = "REEL MOTOR";
         this.ReelMotorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.ReelMotorLabel.Click += new System.EventHandler(this.ReelMotorLabel_Click);
         // 
         // BulletMotorStatusTextBox
         // 
         this.BulletMotorStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.BulletMotorStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.BulletMotorStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.BulletMotorStatusTextBox.Location = new System.Drawing.Point(317, 166);
         this.BulletMotorStatusTextBox.Name = "BulletMotorStatusTextBox";
         this.BulletMotorStatusTextBox.ReadOnly = true;
         this.BulletMotorStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.BulletMotorStatusTextBox.TabIndex = 242;
         this.BulletMotorStatusTextBox.Text = "not connected";
         this.BulletMotorStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // BulletMotorLabel
         // 
         this.BulletMotorLabel.BackColor = System.Drawing.Color.Teal;
         this.BulletMotorLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.BulletMotorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BulletMotorLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.BulletMotorLabel.Location = new System.Drawing.Point(8, 168);
         this.BulletMotorLabel.Name = "BulletMotorLabel";
         this.BulletMotorLabel.Size = new System.Drawing.Size(301, 23);
         this.BulletMotorLabel.TabIndex = 241;
         this.BulletMotorLabel.Text = "BULLET MOTOR";
         this.BulletMotorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.BulletMotorLabel.Click += new System.EventHandler(this.BulletMotorLabel_Click);
         // 
         // FeederRightMotorStatusTextBox
         // 
         this.FeederRightMotorStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.FeederRightMotorStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.FeederRightMotorStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.FeederRightMotorStatusTextBox.Location = new System.Drawing.Point(317, 230);
         this.FeederRightMotorStatusTextBox.Name = "FeederRightMotorStatusTextBox";
         this.FeederRightMotorStatusTextBox.ReadOnly = true;
         this.FeederRightMotorStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.FeederRightMotorStatusTextBox.TabIndex = 240;
         this.FeederRightMotorStatusTextBox.Text = "not connected";
         this.FeederRightMotorStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // FeederRightMotorLabel
         // 
         this.FeederRightMotorLabel.BackColor = System.Drawing.Color.Teal;
         this.FeederRightMotorLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.FeederRightMotorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.FeederRightMotorLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.FeederRightMotorLabel.Location = new System.Drawing.Point(8, 232);
         this.FeederRightMotorLabel.Name = "FeederRightMotorLabel";
         this.FeederRightMotorLabel.Size = new System.Drawing.Size(301, 23);
         this.FeederRightMotorLabel.TabIndex = 239;
         this.FeederRightMotorLabel.Text = "FEEDER RIGHT MOTOR";
         this.FeederRightMotorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.FeederRightMotorLabel.Click += new System.EventHandler(this.FeederRightMotorLabel_Click);
         // 
         // FeederLeftMotorStatusTextBox
         // 
         this.FeederLeftMotorStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.FeederLeftMotorStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.FeederLeftMotorStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.FeederLeftMotorStatusTextBox.Location = new System.Drawing.Point(317, 198);
         this.FeederLeftMotorStatusTextBox.Name = "FeederLeftMotorStatusTextBox";
         this.FeederLeftMotorStatusTextBox.ReadOnly = true;
         this.FeederLeftMotorStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.FeederLeftMotorStatusTextBox.TabIndex = 238;
         this.FeederLeftMotorStatusTextBox.Text = "not connected";
         this.FeederLeftMotorStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // FeederLeftMotorLabel
         // 
         this.FeederLeftMotorLabel.BackColor = System.Drawing.Color.Teal;
         this.FeederLeftMotorLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.FeederLeftMotorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.FeederLeftMotorLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.FeederLeftMotorLabel.Location = new System.Drawing.Point(8, 200);
         this.FeederLeftMotorLabel.Name = "FeederLeftMotorLabel";
         this.FeederLeftMotorLabel.Size = new System.Drawing.Size(301, 23);
         this.FeederLeftMotorLabel.TabIndex = 237;
         this.FeederLeftMotorLabel.Text = "FEEDER LEFT MOTOR";
         this.FeederLeftMotorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.FeederLeftMotorLabel.Click += new System.EventHandler(this.FeederLeftMotorLabel_Click);
         // 
         // LaunchCardAnalogIoStatusTextBox
         // 
         this.LaunchCardAnalogIoStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.LaunchCardAnalogIoStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.LaunchCardAnalogIoStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.LaunchCardAnalogIoStatusTextBox.Location = new System.Drawing.Point(317, 134);
         this.LaunchCardAnalogIoStatusTextBox.Name = "LaunchCardAnalogIoStatusTextBox";
         this.LaunchCardAnalogIoStatusTextBox.ReadOnly = true;
         this.LaunchCardAnalogIoStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.LaunchCardAnalogIoStatusTextBox.TabIndex = 236;
         this.LaunchCardAnalogIoStatusTextBox.Text = "not connected";
         this.LaunchCardAnalogIoStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // LaunchCardAnalogIoLabel
         // 
         this.LaunchCardAnalogIoLabel.BackColor = System.Drawing.Color.Teal;
         this.LaunchCardAnalogIoLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.LaunchCardAnalogIoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LaunchCardAnalogIoLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.LaunchCardAnalogIoLabel.Location = new System.Drawing.Point(8, 136);
         this.LaunchCardAnalogIoLabel.Name = "LaunchCardAnalogIoLabel";
         this.LaunchCardAnalogIoLabel.Size = new System.Drawing.Size(301, 23);
         this.LaunchCardAnalogIoLabel.TabIndex = 235;
         this.LaunchCardAnalogIoLabel.Text = "LAUNCH CARD ANALOG IO";
         this.LaunchCardAnalogIoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.LaunchCardAnalogIoLabel.Click += new System.EventHandler(this.LaunchCardAnalogIoLabel_Click);
         // 
         // LaunchCardCameraLightsStatusTextBox
         // 
         this.LaunchCardCameraLightsStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.LaunchCardCameraLightsStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.LaunchCardCameraLightsStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.LaunchCardCameraLightsStatusTextBox.Location = new System.Drawing.Point(317, 102);
         this.LaunchCardCameraLightsStatusTextBox.Name = "LaunchCardCameraLightsStatusTextBox";
         this.LaunchCardCameraLightsStatusTextBox.ReadOnly = true;
         this.LaunchCardCameraLightsStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.LaunchCardCameraLightsStatusTextBox.TabIndex = 230;
         this.LaunchCardCameraLightsStatusTextBox.Text = "not connected";
         this.LaunchCardCameraLightsStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // LaunchCardCameraLightsLabel
         // 
         this.LaunchCardCameraLightsLabel.AccessibleDescription = "111";
         this.LaunchCardCameraLightsLabel.BackColor = System.Drawing.Color.Teal;
         this.LaunchCardCameraLightsLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.LaunchCardCameraLightsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LaunchCardCameraLightsLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.LaunchCardCameraLightsLabel.Location = new System.Drawing.Point(8, 104);
         this.LaunchCardCameraLightsLabel.Name = "LaunchCardCameraLightsLabel";
         this.LaunchCardCameraLightsLabel.Size = new System.Drawing.Size(301, 23);
         this.LaunchCardCameraLightsLabel.TabIndex = 229;
         this.LaunchCardCameraLightsLabel.Text = "LAUNCH CARD CAMERA/LIGHTS";
         this.LaunchCardCameraLightsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.LaunchCardCameraLightsLabel.Click += new System.EventHandler(this.LaunchCardCameraLightsLabel_Click);
         // 
         // label1
         // 
         this.label1.BackColor = System.Drawing.Color.Teal;
         this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label1.ForeColor = System.Drawing.Color.Gainsboro;
         this.label1.Location = new System.Drawing.Point(8, 40);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(301, 23);
         this.label1.TabIndex = 182;
         this.label1.Text = "TARGET BUS";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // LaunchCardControllerLabel
         // 
         this.LaunchCardControllerLabel.BackColor = System.Drawing.Color.Teal;
         this.LaunchCardControllerLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.LaunchCardControllerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LaunchCardControllerLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.LaunchCardControllerLabel.Location = new System.Drawing.Point(8, 72);
         this.LaunchCardControllerLabel.Name = "LaunchCardControllerLabel";
         this.LaunchCardControllerLabel.Size = new System.Drawing.Size(301, 23);
         this.LaunchCardControllerLabel.TabIndex = 183;
         this.LaunchCardControllerLabel.Text = "LAUNCH CARD CONTROLLER";
         this.LaunchCardControllerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.LaunchCardControllerLabel.Click += new System.EventHandler(this.LaunchCardControllerLabel_Click);
         // 
         // TargetBusStatusTextBox
         // 
         this.TargetBusStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.TargetBusStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.TargetBusStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.TargetBusStatusTextBox.Location = new System.Drawing.Point(317, 38);
         this.TargetBusStatusTextBox.Name = "TargetBusStatusTextBox";
         this.TargetBusStatusTextBox.ReadOnly = true;
         this.TargetBusStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.TargetBusStatusTextBox.TabIndex = 181;
         this.TargetBusStatusTextBox.Text = "not connected";
         this.TargetBusStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // LaunchCardControllerStatusTextBox
         // 
         this.LaunchCardControllerStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.LaunchCardControllerStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.LaunchCardControllerStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.LaunchCardControllerStatusTextBox.Location = new System.Drawing.Point(317, 70);
         this.LaunchCardControllerStatusTextBox.Name = "LaunchCardControllerStatusTextBox";
         this.LaunchCardControllerStatusTextBox.ReadOnly = true;
         this.LaunchCardControllerStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.LaunchCardControllerStatusTextBox.TabIndex = 180;
         this.LaunchCardControllerStatusTextBox.Text = "not connected";
         this.LaunchCardControllerStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // DeviceStatusAPanel
         // 
         this.DeviceStatusAPanel.Controls.Add(this.HubTiltMotorStatusTextBox);
         this.DeviceStatusAPanel.Controls.Add(this.HubTiltMotorLabel);
         this.DeviceStatusAPanel.Controls.Add(this.HubPanMotorStatusTextBox);
         this.DeviceStatusAPanel.Controls.Add(this.HubPanMotorLabel);
         this.DeviceStatusAPanel.Controls.Add(this.HubCameraLightsStatusTextBox);
         this.DeviceStatusAPanel.Controls.Add(this.HubCameraLightsLabel);
         this.DeviceStatusAPanel.Controls.Add(this.RightTrackMotorStatusTextBox);
         this.DeviceStatusAPanel.Controls.Add(this.RightTrackMotorLabel);
         this.DeviceStatusAPanel.Controls.Add(this.RightTrackLightStatusTextBox);
         this.DeviceStatusAPanel.Controls.Add(this.RightTrackLightLabel);
         this.DeviceStatusAPanel.Controls.Add(this.RightTrackControllerStatusTextBox);
         this.DeviceStatusAPanel.Controls.Add(this.RightTrackControllerLabel);
         this.DeviceStatusAPanel.Controls.Add(this.LeftTrackMotorStatusTextBox);
         this.DeviceStatusAPanel.Controls.Add(this.LeftTrackMotorLabel);
         this.DeviceStatusAPanel.Controls.Add(this.LeftTrackLightStatusTextBox);
         this.DeviceStatusAPanel.Controls.Add(this.LeftTrackLightLabel);
         this.DeviceStatusAPanel.Controls.Add(this.HubControllerStatusTextBox);
         this.DeviceStatusAPanel.Controls.Add(this.HubControllerLabel);
         this.DeviceStatusAPanel.Controls.Add(this.JoystickLabel);
         this.DeviceStatusAPanel.Controls.Add(this.RobotBusStatusTextBox);
         this.DeviceStatusAPanel.Controls.Add(this.LeftTrackControllerStatusTextBox);
         this.DeviceStatusAPanel.Controls.Add(this.LeftTrackControllerLabel);
         this.DeviceStatusAPanel.Controls.Add(this.RobotBusLabel);
         this.DeviceStatusAPanel.Controls.Add(this.JoystickStatusTextBox);
         this.DeviceStatusAPanel.EdgeWeight = 1;
         this.DeviceStatusAPanel.Location = new System.Drawing.Point(24, 47);
         this.DeviceStatusAPanel.Name = "DeviceStatusAPanel";
         this.DeviceStatusAPanel.Size = new System.Drawing.Size(705, 648);
         this.DeviceStatusAPanel.TabIndex = 202;
         // 
         // HubTiltMotorStatusTextBox
         // 
         this.HubTiltMotorStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.HubTiltMotorStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.HubTiltMotorStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.HubTiltMotorStatusTextBox.Location = new System.Drawing.Point(317, 358);
         this.HubTiltMotorStatusTextBox.Name = "HubTiltMotorStatusTextBox";
         this.HubTiltMotorStatusTextBox.ReadOnly = true;
         this.HubTiltMotorStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.HubTiltMotorStatusTextBox.TabIndex = 242;
         this.HubTiltMotorStatusTextBox.Text = "not connected";
         this.HubTiltMotorStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // HubTiltMotorLabel
         // 
         this.HubTiltMotorLabel.BackColor = System.Drawing.Color.Teal;
         this.HubTiltMotorLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.HubTiltMotorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.HubTiltMotorLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.HubTiltMotorLabel.Location = new System.Drawing.Point(8, 360);
         this.HubTiltMotorLabel.Name = "HubTiltMotorLabel";
         this.HubTiltMotorLabel.Size = new System.Drawing.Size(301, 23);
         this.HubTiltMotorLabel.TabIndex = 241;
         this.HubTiltMotorLabel.Text = "HUB TILT MOTOR";
         this.HubTiltMotorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.HubTiltMotorLabel.Click += new System.EventHandler(this.HubTiltMotorLabel_Click);
         // 
         // HubPanMotorStatusTextBox
         // 
         this.HubPanMotorStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.HubPanMotorStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.HubPanMotorStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.HubPanMotorStatusTextBox.Location = new System.Drawing.Point(317, 326);
         this.HubPanMotorStatusTextBox.Name = "HubPanMotorStatusTextBox";
         this.HubPanMotorStatusTextBox.ReadOnly = true;
         this.HubPanMotorStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.HubPanMotorStatusTextBox.TabIndex = 240;
         this.HubPanMotorStatusTextBox.Text = "not connected";
         this.HubPanMotorStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // HubPanMotorLabel
         // 
         this.HubPanMotorLabel.BackColor = System.Drawing.Color.Teal;
         this.HubPanMotorLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.HubPanMotorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.HubPanMotorLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.HubPanMotorLabel.Location = new System.Drawing.Point(8, 328);
         this.HubPanMotorLabel.Name = "HubPanMotorLabel";
         this.HubPanMotorLabel.Size = new System.Drawing.Size(301, 23);
         this.HubPanMotorLabel.TabIndex = 239;
         this.HubPanMotorLabel.Text = "HUB PAN MOTOR";
         this.HubPanMotorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.HubPanMotorLabel.Click += new System.EventHandler(this.HubPanMotorLabel_Click);
         // 
         // HubCameraLightsStatusTextBox
         // 
         this.HubCameraLightsStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.HubCameraLightsStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.HubCameraLightsStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.HubCameraLightsStatusTextBox.Location = new System.Drawing.Point(317, 294);
         this.HubCameraLightsStatusTextBox.Name = "HubCameraLightsStatusTextBox";
         this.HubCameraLightsStatusTextBox.ReadOnly = true;
         this.HubCameraLightsStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.HubCameraLightsStatusTextBox.TabIndex = 238;
         this.HubCameraLightsStatusTextBox.Text = "not connected";
         this.HubCameraLightsStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // HubCameraLightsLabel
         // 
         this.HubCameraLightsLabel.BackColor = System.Drawing.Color.Teal;
         this.HubCameraLightsLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.HubCameraLightsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.HubCameraLightsLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.HubCameraLightsLabel.Location = new System.Drawing.Point(8, 296);
         this.HubCameraLightsLabel.Name = "HubCameraLightsLabel";
         this.HubCameraLightsLabel.Size = new System.Drawing.Size(301, 23);
         this.HubCameraLightsLabel.TabIndex = 237;
         this.HubCameraLightsLabel.Text = "HUB CAMERA/LIGHTS";
         this.HubCameraLightsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.HubCameraLightsLabel.Click += new System.EventHandler(this.HubCameraLightsLabel_Click);
         // 
         // RightTrackMotorStatusTextBox
         // 
         this.RightTrackMotorStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.RightTrackMotorStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.RightTrackMotorStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.RightTrackMotorStatusTextBox.Location = new System.Drawing.Point(317, 230);
         this.RightTrackMotorStatusTextBox.Name = "RightTrackMotorStatusTextBox";
         this.RightTrackMotorStatusTextBox.ReadOnly = true;
         this.RightTrackMotorStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.RightTrackMotorStatusTextBox.TabIndex = 236;
         this.RightTrackMotorStatusTextBox.Text = "not connected";
         this.RightTrackMotorStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // RightTrackMotorLabel
         // 
         this.RightTrackMotorLabel.BackColor = System.Drawing.Color.Teal;
         this.RightTrackMotorLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.RightTrackMotorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.RightTrackMotorLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.RightTrackMotorLabel.Location = new System.Drawing.Point(8, 232);
         this.RightTrackMotorLabel.Name = "RightTrackMotorLabel";
         this.RightTrackMotorLabel.Size = new System.Drawing.Size(301, 23);
         this.RightTrackMotorLabel.TabIndex = 235;
         this.RightTrackMotorLabel.Text = "RIGHT TRACK MOTOR";
         this.RightTrackMotorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.RightTrackMotorLabel.Click += new System.EventHandler(this.RightTrackMotorLabel_Click);
         // 
         // RightTrackLightStatusTextBox
         // 
         this.RightTrackLightStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.RightTrackLightStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.RightTrackLightStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.RightTrackLightStatusTextBox.Location = new System.Drawing.Point(317, 198);
         this.RightTrackLightStatusTextBox.Name = "RightTrackLightStatusTextBox";
         this.RightTrackLightStatusTextBox.ReadOnly = true;
         this.RightTrackLightStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.RightTrackLightStatusTextBox.TabIndex = 234;
         this.RightTrackLightStatusTextBox.Text = "not connected";
         this.RightTrackLightStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // RightTrackLightLabel
         // 
         this.RightTrackLightLabel.BackColor = System.Drawing.Color.Teal;
         this.RightTrackLightLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.RightTrackLightLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.RightTrackLightLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.RightTrackLightLabel.Location = new System.Drawing.Point(8, 200);
         this.RightTrackLightLabel.Name = "RightTrackLightLabel";
         this.RightTrackLightLabel.Size = new System.Drawing.Size(301, 23);
         this.RightTrackLightLabel.TabIndex = 233;
         this.RightTrackLightLabel.Text = "RIGHT TRACK LIGHT";
         this.RightTrackLightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.RightTrackLightLabel.Click += new System.EventHandler(this.RightTrackLightLabel_Click);
         // 
         // RightTrackControllerStatusTextBox
         // 
         this.RightTrackControllerStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.RightTrackControllerStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.RightTrackControllerStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.RightTrackControllerStatusTextBox.Location = new System.Drawing.Point(317, 166);
         this.RightTrackControllerStatusTextBox.Name = "RightTrackControllerStatusTextBox";
         this.RightTrackControllerStatusTextBox.ReadOnly = true;
         this.RightTrackControllerStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.RightTrackControllerStatusTextBox.TabIndex = 232;
         this.RightTrackControllerStatusTextBox.Text = "not connected";
         this.RightTrackControllerStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // RightTrackControllerLabel
         // 
         this.RightTrackControllerLabel.BackColor = System.Drawing.Color.Teal;
         this.RightTrackControllerLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.RightTrackControllerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.RightTrackControllerLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.RightTrackControllerLabel.Location = new System.Drawing.Point(8, 168);
         this.RightTrackControllerLabel.Name = "RightTrackControllerLabel";
         this.RightTrackControllerLabel.Size = new System.Drawing.Size(301, 23);
         this.RightTrackControllerLabel.TabIndex = 231;
         this.RightTrackControllerLabel.Text = "RIGHT TRACK CONTROLLER";
         this.RightTrackControllerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.RightTrackControllerLabel.Click += new System.EventHandler(this.RightTrackControllerLabel_Click);
         // 
         // LeftTrackMotorStatusTextBox
         // 
         this.LeftTrackMotorStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.LeftTrackMotorStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.LeftTrackMotorStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.LeftTrackMotorStatusTextBox.Location = new System.Drawing.Point(317, 134);
         this.LeftTrackMotorStatusTextBox.Name = "LeftTrackMotorStatusTextBox";
         this.LeftTrackMotorStatusTextBox.ReadOnly = true;
         this.LeftTrackMotorStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.LeftTrackMotorStatusTextBox.TabIndex = 230;
         this.LeftTrackMotorStatusTextBox.Text = "not connected";
         this.LeftTrackMotorStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // LeftTrackMotorLabel
         // 
         this.LeftTrackMotorLabel.BackColor = System.Drawing.Color.Teal;
         this.LeftTrackMotorLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.LeftTrackMotorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LeftTrackMotorLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.LeftTrackMotorLabel.Location = new System.Drawing.Point(8, 136);
         this.LeftTrackMotorLabel.Name = "LeftTrackMotorLabel";
         this.LeftTrackMotorLabel.Size = new System.Drawing.Size(301, 23);
         this.LeftTrackMotorLabel.TabIndex = 229;
         this.LeftTrackMotorLabel.Text = "LEFT TRACK MOTOR";
         this.LeftTrackMotorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.LeftTrackMotorLabel.Click += new System.EventHandler(this.LeftTrackMotorLabel_Click);
         // 
         // LeftTrackLightStatusTextBox
         // 
         this.LeftTrackLightStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.LeftTrackLightStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.LeftTrackLightStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.LeftTrackLightStatusTextBox.Location = new System.Drawing.Point(317, 102);
         this.LeftTrackLightStatusTextBox.Name = "LeftTrackLightStatusTextBox";
         this.LeftTrackLightStatusTextBox.ReadOnly = true;
         this.LeftTrackLightStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.LeftTrackLightStatusTextBox.TabIndex = 228;
         this.LeftTrackLightStatusTextBox.Text = "not connected";
         this.LeftTrackLightStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // LeftTrackLightLabel
         // 
         this.LeftTrackLightLabel.BackColor = System.Drawing.Color.Teal;
         this.LeftTrackLightLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.LeftTrackLightLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LeftTrackLightLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.LeftTrackLightLabel.Location = new System.Drawing.Point(8, 104);
         this.LeftTrackLightLabel.Name = "LeftTrackLightLabel";
         this.LeftTrackLightLabel.Size = new System.Drawing.Size(301, 23);
         this.LeftTrackLightLabel.TabIndex = 227;
         this.LeftTrackLightLabel.Text = "LEFT TRACK LIGHT";
         this.LeftTrackLightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.LeftTrackLightLabel.Click += new System.EventHandler(this.LeftTrackLightLabel_Click);
         // 
         // HubControllerStatusTextBox
         // 
         this.HubControllerStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.HubControllerStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.HubControllerStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.HubControllerStatusTextBox.Location = new System.Drawing.Point(317, 262);
         this.HubControllerStatusTextBox.Name = "HubControllerStatusTextBox";
         this.HubControllerStatusTextBox.ReadOnly = true;
         this.HubControllerStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.HubControllerStatusTextBox.TabIndex = 226;
         this.HubControllerStatusTextBox.Text = "not connected";
         this.HubControllerStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // HubControllerLabel
         // 
         this.HubControllerLabel.BackColor = System.Drawing.Color.Teal;
         this.HubControllerLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.HubControllerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.HubControllerLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.HubControllerLabel.Location = new System.Drawing.Point(8, 264);
         this.HubControllerLabel.Name = "HubControllerLabel";
         this.HubControllerLabel.Size = new System.Drawing.Size(301, 23);
         this.HubControllerLabel.TabIndex = 225;
         this.HubControllerLabel.Text = "HUB CONTROLLER";
         this.HubControllerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.HubControllerLabel.Click += new System.EventHandler(this.HubControllerLabel_Click);
         // 
         // JoystickLabel
         // 
         this.JoystickLabel.BackColor = System.Drawing.Color.Teal;
         this.JoystickLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.JoystickLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.JoystickLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.JoystickLabel.Location = new System.Drawing.Point(8, 8);
         this.JoystickLabel.Name = "JoystickLabel";
         this.JoystickLabel.Size = new System.Drawing.Size(301, 23);
         this.JoystickLabel.TabIndex = 224;
         this.JoystickLabel.Text = "JOYSTICK";
         this.JoystickLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.JoystickLabel.Click += new System.EventHandler(this.JoystickLabel_Click);
         // 
         // RobotBusStatusTextBox
         // 
         this.RobotBusStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.RobotBusStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.RobotBusStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.RobotBusStatusTextBox.Location = new System.Drawing.Point(317, 38);
         this.RobotBusStatusTextBox.Name = "RobotBusStatusTextBox";
         this.RobotBusStatusTextBox.ReadOnly = true;
         this.RobotBusStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.RobotBusStatusTextBox.TabIndex = 219;
         this.RobotBusStatusTextBox.Text = "not connected";
         this.RobotBusStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // LeftTrackControllerStatusTextBox
         // 
         this.LeftTrackControllerStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.LeftTrackControllerStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.LeftTrackControllerStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.LeftTrackControllerStatusTextBox.Location = new System.Drawing.Point(317, 70);
         this.LeftTrackControllerStatusTextBox.Name = "LeftTrackControllerStatusTextBox";
         this.LeftTrackControllerStatusTextBox.ReadOnly = true;
         this.LeftTrackControllerStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.LeftTrackControllerStatusTextBox.TabIndex = 221;
         this.LeftTrackControllerStatusTextBox.Text = "not connected";
         this.LeftTrackControllerStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // LeftTrackControllerLabel
         // 
         this.LeftTrackControllerLabel.BackColor = System.Drawing.Color.Teal;
         this.LeftTrackControllerLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.LeftTrackControllerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LeftTrackControllerLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.LeftTrackControllerLabel.Location = new System.Drawing.Point(8, 72);
         this.LeftTrackControllerLabel.Name = "LeftTrackControllerLabel";
         this.LeftTrackControllerLabel.Size = new System.Drawing.Size(301, 23);
         this.LeftTrackControllerLabel.TabIndex = 222;
         this.LeftTrackControllerLabel.Text = "LEFT TRACK CONTROLLER";
         this.LeftTrackControllerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.LeftTrackControllerLabel.Click += new System.EventHandler(this.LeftTrackControllerLabel_Click);
         // 
         // RobotBusLabel
         // 
         this.RobotBusLabel.BackColor = System.Drawing.Color.Teal;
         this.RobotBusLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.RobotBusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.RobotBusLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.RobotBusLabel.Location = new System.Drawing.Point(8, 40);
         this.RobotBusLabel.Name = "RobotBusLabel";
         this.RobotBusLabel.Size = new System.Drawing.Size(301, 23);
         this.RobotBusLabel.TabIndex = 220;
         this.RobotBusLabel.Text = "ROBOT BUS";
         this.RobotBusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // JoystickStatusTextBox
         // 
         this.JoystickStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.JoystickStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.JoystickStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.JoystickStatusTextBox.Location = new System.Drawing.Point(317, 6);
         this.JoystickStatusTextBox.Name = "JoystickStatusTextBox";
         this.JoystickStatusTextBox.ReadOnly = true;
         this.JoystickStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.JoystickStatusTextBox.TabIndex = 223;
         this.JoystickStatusTextBox.Text = "not connected";
         this.JoystickStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // ComponentStatusLabel
         // 
         this.ComponentStatusLabel.BackColor = System.Drawing.Color.Teal;
         this.ComponentStatusLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.ComponentStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ComponentStatusLabel.ForeColor = System.Drawing.Color.White;
         this.ComponentStatusLabel.Location = new System.Drawing.Point(16, 16);
         this.ComponentStatusLabel.Name = "ComponentStatusLabel";
         this.ComponentStatusLabel.Size = new System.Drawing.Size(1434, 23);
         this.ComponentStatusLabel.TabIndex = 201;
         this.ComponentStatusLabel.Text = "COMPONENT STATUS";
         this.ComponentStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.ComponentStatusLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ComponentStatusLabel_MouseDown);
         this.ComponentStatusLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ComponentStatusLabel_MouseMove);
         this.ComponentStatusLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ComponentStatusLabel_MouseUp);
         // 
         // SettingsPanel
         // 
         this.SettingsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
         this.SettingsPanel.Controls.Add(this.SaveDefaultsButton);
         this.SettingsPanel.Controls.Add(this.TriggerDefaultsButton);
         this.SettingsPanel.Controls.Add(this.label15);
         this.SettingsPanel.EdgeWeight = 3;
         this.SettingsPanel.Location = new System.Drawing.Point(1490, 16);
         this.SettingsPanel.Name = "SettingsPanel";
         this.SettingsPanel.Size = new System.Drawing.Size(275, 130);
         this.SettingsPanel.TabIndex = 5;
         // 
         // SaveDefaultsButton
         // 
         this.SaveDefaultsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SaveDefaultsButton.DisabledBackColor = System.Drawing.SystemColors.ControlDarkDark;
         this.SaveDefaultsButton.DisabledForeColor = System.Drawing.Color.Gray;
         this.SaveDefaultsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.SaveDefaultsButton.ForeColor = System.Drawing.Color.Black;
         this.SaveDefaultsButton.HoldArrorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.SaveDefaultsButton.HoldTimeoutEnable = true;
         this.SaveDefaultsButton.HoldTimeoutInterval = 100;
         this.SaveDefaultsButton.Location = new System.Drawing.Point(142, 47);
         this.SaveDefaultsButton.Name = "SaveDefaultsButton";
         this.SaveDefaultsButton.Size = new System.Drawing.Size(107, 67);
         this.SaveDefaultsButton.TabIndex = 203;
         this.SaveDefaultsButton.Text = "SAVE AS DEFAULTS";
         this.SaveDefaultsButton.UseVisualStyleBackColor = false;
         this.SaveDefaultsButton.HoldTimeout += new Weco.Ui.Controls.HoldTimeoutHandler(this.SaveDefaultsButton_HoldTimeout);
         // 
         // TriggerDefaultsButton
         // 
         this.TriggerDefaultsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.TriggerDefaultsButton.DisabledBackColor = System.Drawing.SystemColors.ControlDarkDark;
         this.TriggerDefaultsButton.DisabledForeColor = System.Drawing.Color.Gray;
         this.TriggerDefaultsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.TriggerDefaultsButton.ForeColor = System.Drawing.Color.Black;
         this.TriggerDefaultsButton.HoldArrorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.TriggerDefaultsButton.HoldTimeoutEnable = true;
         this.TriggerDefaultsButton.HoldTimeoutInterval = 100;
         this.TriggerDefaultsButton.Location = new System.Drawing.Point(27, 47);
         this.TriggerDefaultsButton.Name = "TriggerDefaultsButton";
         this.TriggerDefaultsButton.Size = new System.Drawing.Size(107, 67);
         this.TriggerDefaultsButton.TabIndex = 202;
         this.TriggerDefaultsButton.Text = "TRIGGER DEFAULTS";
         this.TriggerDefaultsButton.UseVisualStyleBackColor = false;
         this.TriggerDefaultsButton.HoldTimeout += new Weco.Ui.Controls.HoldTimeoutHandler(this.TriggerDefaultsButton_HoldTimeout);
         // 
         // label15
         // 
         this.label15.BackColor = System.Drawing.Color.Teal;
         this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label15.ForeColor = System.Drawing.Color.White;
         this.label15.Location = new System.Drawing.Point(16, 16);
         this.label15.Name = "label15";
         this.label15.Size = new System.Drawing.Size(243, 23);
         this.label15.TabIndex = 201;
         this.label15.Text = "SETTINGS";
         this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // BackButton
         // 
         this.BackButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BackButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BackButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.BackButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.BackButton.Location = new System.Drawing.Point(1574, 644);
         this.BackButton.Name = "BackButton";
         this.BackButton.Size = new System.Drawing.Size(107, 67);
         this.BackButton.TabIndex = 4;
         this.BackButton.Text = "BACK";
         this.BackButton.UseVisualStyleBackColor = false;
         this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
         // 
         // UpdateTimer
         // 
         this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
         // 
         // SystemStatusForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1781, 743);
         this.Controls.Add(this.MainPanel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "SystemStatusForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "SystemStatusForm";
         this.Shown += new System.EventHandler(this.SystemStatusForm_Shown);
         this.MainPanel.ResumeLayout(false);
         this.SelectPanel.ResumeLayout(false);
         this.LoggingPanel.ResumeLayout(false);
         this.DeviceStatusPanel.ResumeLayout(false);
         this.DeviceStatusBPanel.ResumeLayout(false);
         this.DeviceStatusBPanel.PerformLayout();
         this.DeviceStatusAPanel.ResumeLayout(false);
         this.DeviceStatusAPanel.PerformLayout();
         this.SettingsPanel.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private Controls.BorderedPanel MainPanel;
      private Controls.BaseButton BackButton;
      private Controls.BorderedPanel SettingsPanel;
      private System.Windows.Forms.Label label15;
      private Controls.HoldButton SaveDefaultsButton;
      private Controls.HoldButton TriggerDefaultsButton;
      private Controls.BorderedPanel DeviceStatusPanel;
      private System.Windows.Forms.Label ComponentStatusLabel;
      private Controls.BorderedPanel DeviceStatusBPanel;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label LaunchCardControllerLabel;
      private System.Windows.Forms.TextBox TargetBusStatusTextBox;
      private System.Windows.Forms.TextBox LaunchCardControllerStatusTextBox;
      private Controls.BorderedPanel DeviceStatusAPanel;
      private System.Windows.Forms.Label JoystickLabel;
      private System.Windows.Forms.TextBox RobotBusStatusTextBox;
      private System.Windows.Forms.TextBox LeftTrackControllerStatusTextBox;
      private System.Windows.Forms.Label LeftTrackControllerLabel;
      private System.Windows.Forms.Label RobotBusLabel;
      private System.Windows.Forms.TextBox JoystickStatusTextBox;
      private System.Windows.Forms.Timer UpdateTimer;
      private Controls.BorderedPanel LoggingPanel;
      private Controls.ValueButton LoggingAddressValueButton;
      private System.Windows.Forms.Label label14;
      private Controls.BaseButton TargetBusHeartbeatButton;
      private Controls.BaseButton MainBusHeartbeatButton;
      private Controls.ValueButton LoggingPortValueButton;
      private Controls.BorderedPanel SelectPanel;
      private Controls.HoldButton SystemResetButton;
      private Controls.BaseButton ShowToggleButton;
      private System.Windows.Forms.TextBox HubControllerStatusTextBox;
      private System.Windows.Forms.Label HubControllerLabel;
      private System.Windows.Forms.TextBox LeftTrackLightStatusTextBox;
      private System.Windows.Forms.Label LeftTrackLightLabel;
      private System.Windows.Forms.TextBox LaunchCardCameraLightsStatusTextBox;
      private System.Windows.Forms.Label LaunchCardCameraLightsLabel;
      private System.Windows.Forms.TextBox RightTrackMotorStatusTextBox;
      private System.Windows.Forms.Label RightTrackMotorLabel;
      private System.Windows.Forms.TextBox RightTrackLightStatusTextBox;
      private System.Windows.Forms.Label RightTrackLightLabel;
      private System.Windows.Forms.TextBox RightTrackControllerStatusTextBox;
      private System.Windows.Forms.Label RightTrackControllerLabel;
      private System.Windows.Forms.TextBox LeftTrackMotorStatusTextBox;
      private System.Windows.Forms.Label LeftTrackMotorLabel;
      private System.Windows.Forms.TextBox FeederRightMotorStatusTextBox;
      private System.Windows.Forms.Label FeederRightMotorLabel;
      private System.Windows.Forms.TextBox FeederLeftMotorStatusTextBox;
      private System.Windows.Forms.Label FeederLeftMotorLabel;
      private System.Windows.Forms.TextBox LaunchCardAnalogIoStatusTextBox;
      private System.Windows.Forms.Label LaunchCardAnalogIoLabel;
      private System.Windows.Forms.TextBox HubTiltMotorStatusTextBox;
      private System.Windows.Forms.Label HubTiltMotorLabel;
      private System.Windows.Forms.TextBox HubPanMotorStatusTextBox;
      private System.Windows.Forms.Label HubPanMotorLabel;
      private System.Windows.Forms.TextBox HubCameraLightsStatusTextBox;
      private System.Windows.Forms.Label HubCameraLightsLabel;
      private System.Windows.Forms.TextBox ReelDigitalIoStatusTextBox;
      private System.Windows.Forms.Label ReelDigitalIoLabel;
      private System.Windows.Forms.TextBox OsdRs232StatusTextBox;
      private System.Windows.Forms.Label OsdRs232Label;
      private System.Windows.Forms.TextBox ReelEncoderStatusTextBox;
      private System.Windows.Forms.Label ReelEncoderLabel;
      private System.Windows.Forms.TextBox ReelMotorStatusTextBox;
      private System.Windows.Forms.Label ReelMotorLabel;
      private System.Windows.Forms.TextBox BulletMotorStatusTextBox;
      private System.Windows.Forms.Label BulletMotorLabel;
   }
}