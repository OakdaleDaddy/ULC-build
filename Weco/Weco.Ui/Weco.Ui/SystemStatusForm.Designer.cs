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
         this.TargetBoardCameraStepperStatusTextBox = new System.Windows.Forms.TextBox();
         this.TargetBoardCameraStepperLabel = new System.Windows.Forms.Label();
         this.TargetBoardRearWheelStatusTextBox = new System.Windows.Forms.TextBox();
         this.TargetBoardRearWheelLabel = new System.Windows.Forms.Label();
         this.TargetBoardFrontWheelStatusTextBox = new System.Windows.Forms.TextBox();
         this.TargetBoardFrontWheelLabel = new System.Windows.Forms.Label();
         this.TargetBoardCameraLedStatusTextBox = new System.Windows.Forms.TextBox();
         this.TargetBoardCameraLedLabel = new System.Windows.Forms.Label();
         this.UsbRelayStatusTextBox = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.TargetBoardLabel = new System.Windows.Forms.Label();
         this.TargetBusStatusTextBox = new System.Windows.Forms.TextBox();
         this.TargetBoardStatusTextBox = new System.Windows.Forms.TextBox();
         this.DeviceStatusAPanel = new Weco.Ui.Controls.BorderedPanel();
         this.HubTiltMotorStatusTextBox = new System.Windows.Forms.TextBox();
         this.HubTiltMotorLabel = new System.Windows.Forms.Label();
         this.textBox3 = new System.Windows.Forms.TextBox();
         this.HubPanMotorLabel = new System.Windows.Forms.Label();
         this.HubCameraLightsStatusTextBox = new System.Windows.Forms.TextBox();
         this.HubCameraLightsLabel = new System.Windows.Forms.Label();
         this.RightTrackMotorStatusTextBox = new System.Windows.Forms.TextBox();
         this.RightTrackMotorLabel = new System.Windows.Forms.Label();
         this.RightTrackLightsStatusTextBox = new System.Windows.Forms.TextBox();
         this.RightTrackLightsLabel = new System.Windows.Forms.Label();
         this.RightTrackcontrollerStatusTextBox = new System.Windows.Forms.TextBox();
         this.RightTrackControllerLabel = new System.Windows.Forms.Label();
         this.LeftTrackMotorStatusTextBox = new System.Windows.Forms.TextBox();
         this.LeftTrackMotorLabel = new System.Windows.Forms.Label();
         this.LeftTrackLightsStatusTextBox = new System.Windows.Forms.TextBox();
         this.LeftTrackLightsLabel = new System.Windows.Forms.Label();
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
         this.DeviceStatusBPanel.Controls.Add(this.TargetBoardCameraStepperStatusTextBox);
         this.DeviceStatusBPanel.Controls.Add(this.TargetBoardCameraStepperLabel);
         this.DeviceStatusBPanel.Controls.Add(this.TargetBoardRearWheelStatusTextBox);
         this.DeviceStatusBPanel.Controls.Add(this.TargetBoardRearWheelLabel);
         this.DeviceStatusBPanel.Controls.Add(this.TargetBoardFrontWheelStatusTextBox);
         this.DeviceStatusBPanel.Controls.Add(this.TargetBoardFrontWheelLabel);
         this.DeviceStatusBPanel.Controls.Add(this.TargetBoardCameraLedStatusTextBox);
         this.DeviceStatusBPanel.Controls.Add(this.TargetBoardCameraLedLabel);
         this.DeviceStatusBPanel.Controls.Add(this.UsbRelayStatusTextBox);
         this.DeviceStatusBPanel.Controls.Add(this.label2);
         this.DeviceStatusBPanel.Controls.Add(this.label1);
         this.DeviceStatusBPanel.Controls.Add(this.TargetBoardLabel);
         this.DeviceStatusBPanel.Controls.Add(this.TargetBusStatusTextBox);
         this.DeviceStatusBPanel.Controls.Add(this.TargetBoardStatusTextBox);
         this.DeviceStatusBPanel.EdgeWeight = 1;
         this.DeviceStatusBPanel.Location = new System.Drawing.Point(737, 47);
         this.DeviceStatusBPanel.Name = "DeviceStatusBPanel";
         this.DeviceStatusBPanel.Size = new System.Drawing.Size(705, 648);
         this.DeviceStatusBPanel.TabIndex = 203;
         // 
         // TargetBoardCameraStepperStatusTextBox
         // 
         this.TargetBoardCameraStepperStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.TargetBoardCameraStepperStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.TargetBoardCameraStepperStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.TargetBoardCameraStepperStatusTextBox.Location = new System.Drawing.Point(317, 198);
         this.TargetBoardCameraStepperStatusTextBox.Name = "TargetBoardCameraStepperStatusTextBox";
         this.TargetBoardCameraStepperStatusTextBox.ReadOnly = true;
         this.TargetBoardCameraStepperStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.TargetBoardCameraStepperStatusTextBox.TabIndex = 240;
         this.TargetBoardCameraStepperStatusTextBox.Text = "not connected";
         this.TargetBoardCameraStepperStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // TargetBoardCameraStepperLabel
         // 
         this.TargetBoardCameraStepperLabel.BackColor = System.Drawing.Color.Teal;
         this.TargetBoardCameraStepperLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.TargetBoardCameraStepperLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TargetBoardCameraStepperLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.TargetBoardCameraStepperLabel.Location = new System.Drawing.Point(8, 200);
         this.TargetBoardCameraStepperLabel.Name = "TargetBoardCameraStepperLabel";
         this.TargetBoardCameraStepperLabel.Size = new System.Drawing.Size(301, 23);
         this.TargetBoardCameraStepperLabel.TabIndex = 239;
         this.TargetBoardCameraStepperLabel.Text = "TARGET BOARD CAMERA STEPPER";
         this.TargetBoardCameraStepperLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.TargetBoardCameraStepperLabel.Click += new System.EventHandler(this.TargetBoardCameraStepperLabel_Click);
         // 
         // TargetBoardRearWheelStatusTextBox
         // 
         this.TargetBoardRearWheelStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.TargetBoardRearWheelStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.TargetBoardRearWheelStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.TargetBoardRearWheelStatusTextBox.Location = new System.Drawing.Point(317, 166);
         this.TargetBoardRearWheelStatusTextBox.Name = "TargetBoardRearWheelStatusTextBox";
         this.TargetBoardRearWheelStatusTextBox.ReadOnly = true;
         this.TargetBoardRearWheelStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.TargetBoardRearWheelStatusTextBox.TabIndex = 238;
         this.TargetBoardRearWheelStatusTextBox.Text = "not connected";
         this.TargetBoardRearWheelStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // TargetBoardRearWheelLabel
         // 
         this.TargetBoardRearWheelLabel.BackColor = System.Drawing.Color.Teal;
         this.TargetBoardRearWheelLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.TargetBoardRearWheelLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TargetBoardRearWheelLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.TargetBoardRearWheelLabel.Location = new System.Drawing.Point(8, 168);
         this.TargetBoardRearWheelLabel.Name = "TargetBoardRearWheelLabel";
         this.TargetBoardRearWheelLabel.Size = new System.Drawing.Size(301, 23);
         this.TargetBoardRearWheelLabel.TabIndex = 237;
         this.TargetBoardRearWheelLabel.Text = "TARGET BOARD REAR WHEEL";
         this.TargetBoardRearWheelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.TargetBoardRearWheelLabel.Click += new System.EventHandler(this.TargetBoardRearWheelLabel_Click);
         // 
         // TargetBoardFrontWheelStatusTextBox
         // 
         this.TargetBoardFrontWheelStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.TargetBoardFrontWheelStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.TargetBoardFrontWheelStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.TargetBoardFrontWheelStatusTextBox.Location = new System.Drawing.Point(317, 134);
         this.TargetBoardFrontWheelStatusTextBox.Name = "TargetBoardFrontWheelStatusTextBox";
         this.TargetBoardFrontWheelStatusTextBox.ReadOnly = true;
         this.TargetBoardFrontWheelStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.TargetBoardFrontWheelStatusTextBox.TabIndex = 236;
         this.TargetBoardFrontWheelStatusTextBox.Text = "not connected";
         this.TargetBoardFrontWheelStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // TargetBoardFrontWheelLabel
         // 
         this.TargetBoardFrontWheelLabel.BackColor = System.Drawing.Color.Teal;
         this.TargetBoardFrontWheelLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.TargetBoardFrontWheelLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TargetBoardFrontWheelLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.TargetBoardFrontWheelLabel.Location = new System.Drawing.Point(8, 136);
         this.TargetBoardFrontWheelLabel.Name = "TargetBoardFrontWheelLabel";
         this.TargetBoardFrontWheelLabel.Size = new System.Drawing.Size(301, 23);
         this.TargetBoardFrontWheelLabel.TabIndex = 235;
         this.TargetBoardFrontWheelLabel.Text = "TARGET BOARD FRONT WHEEL";
         this.TargetBoardFrontWheelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.TargetBoardFrontWheelLabel.Click += new System.EventHandler(this.TargetBoardFrontWheelLabel_Click);
         // 
         // TargetBoardCameraLedStatusTextBox
         // 
         this.TargetBoardCameraLedStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.TargetBoardCameraLedStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.TargetBoardCameraLedStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.TargetBoardCameraLedStatusTextBox.Location = new System.Drawing.Point(317, 102);
         this.TargetBoardCameraLedStatusTextBox.Name = "TargetBoardCameraLedStatusTextBox";
         this.TargetBoardCameraLedStatusTextBox.ReadOnly = true;
         this.TargetBoardCameraLedStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.TargetBoardCameraLedStatusTextBox.TabIndex = 230;
         this.TargetBoardCameraLedStatusTextBox.Text = "not connected";
         this.TargetBoardCameraLedStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // TargetBoardCameraLedLabel
         // 
         this.TargetBoardCameraLedLabel.AccessibleDescription = "111";
         this.TargetBoardCameraLedLabel.BackColor = System.Drawing.Color.Teal;
         this.TargetBoardCameraLedLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.TargetBoardCameraLedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TargetBoardCameraLedLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.TargetBoardCameraLedLabel.Location = new System.Drawing.Point(8, 104);
         this.TargetBoardCameraLedLabel.Name = "TargetBoardCameraLedLabel";
         this.TargetBoardCameraLedLabel.Size = new System.Drawing.Size(301, 23);
         this.TargetBoardCameraLedLabel.TabIndex = 229;
         this.TargetBoardCameraLedLabel.Text = "TARGET BOARD CAMERA/LED";
         this.TargetBoardCameraLedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.TargetBoardCameraLedLabel.Click += new System.EventHandler(this.TargetBoardCameraLedLabel_Click);
         // 
         // UsbRelayStatusTextBox
         // 
         this.UsbRelayStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.UsbRelayStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.UsbRelayStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.UsbRelayStatusTextBox.Location = new System.Drawing.Point(317, 6);
         this.UsbRelayStatusTextBox.Name = "UsbRelayStatusTextBox";
         this.UsbRelayStatusTextBox.ReadOnly = true;
         this.UsbRelayStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.UsbRelayStatusTextBox.TabIndex = 226;
         this.UsbRelayStatusTextBox.Text = "not connected";
         this.UsbRelayStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label2
         // 
         this.label2.BackColor = System.Drawing.Color.Teal;
         this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label2.ForeColor = System.Drawing.Color.Gainsboro;
         this.label2.Location = new System.Drawing.Point(8, 8);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(301, 23);
         this.label2.TabIndex = 225;
         this.label2.Text = "USB RELAY";
         this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
         // TargetBoardLabel
         // 
         this.TargetBoardLabel.BackColor = System.Drawing.Color.Teal;
         this.TargetBoardLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.TargetBoardLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TargetBoardLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.TargetBoardLabel.Location = new System.Drawing.Point(8, 72);
         this.TargetBoardLabel.Name = "TargetBoardLabel";
         this.TargetBoardLabel.Size = new System.Drawing.Size(301, 23);
         this.TargetBoardLabel.TabIndex = 183;
         this.TargetBoardLabel.Text = "TARGET BOARD";
         this.TargetBoardLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.TargetBoardLabel.Click += new System.EventHandler(this.TargetBoardLabel_Click);
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
         // TargetBoardStatusTextBox
         // 
         this.TargetBoardStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.TargetBoardStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.TargetBoardStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.TargetBoardStatusTextBox.Location = new System.Drawing.Point(317, 70);
         this.TargetBoardStatusTextBox.Name = "TargetBoardStatusTextBox";
         this.TargetBoardStatusTextBox.ReadOnly = true;
         this.TargetBoardStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.TargetBoardStatusTextBox.TabIndex = 180;
         this.TargetBoardStatusTextBox.Text = "not connected";
         this.TargetBoardStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // DeviceStatusAPanel
         // 
         this.DeviceStatusAPanel.Controls.Add(this.HubTiltMotorStatusTextBox);
         this.DeviceStatusAPanel.Controls.Add(this.HubTiltMotorLabel);
         this.DeviceStatusAPanel.Controls.Add(this.textBox3);
         this.DeviceStatusAPanel.Controls.Add(this.HubPanMotorLabel);
         this.DeviceStatusAPanel.Controls.Add(this.HubCameraLightsStatusTextBox);
         this.DeviceStatusAPanel.Controls.Add(this.HubCameraLightsLabel);
         this.DeviceStatusAPanel.Controls.Add(this.RightTrackMotorStatusTextBox);
         this.DeviceStatusAPanel.Controls.Add(this.RightTrackMotorLabel);
         this.DeviceStatusAPanel.Controls.Add(this.RightTrackLightsStatusTextBox);
         this.DeviceStatusAPanel.Controls.Add(this.RightTrackLightsLabel);
         this.DeviceStatusAPanel.Controls.Add(this.RightTrackcontrollerStatusTextBox);
         this.DeviceStatusAPanel.Controls.Add(this.RightTrackControllerLabel);
         this.DeviceStatusAPanel.Controls.Add(this.LeftTrackMotorStatusTextBox);
         this.DeviceStatusAPanel.Controls.Add(this.LeftTrackMotorLabel);
         this.DeviceStatusAPanel.Controls.Add(this.LeftTrackLightsStatusTextBox);
         this.DeviceStatusAPanel.Controls.Add(this.LeftTrackLightsLabel);
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
         // textBox3
         // 
         this.textBox3.BackColor = System.Drawing.Color.Red;
         this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.textBox3.ForeColor = System.Drawing.Color.Black;
         this.textBox3.Location = new System.Drawing.Point(317, 326);
         this.textBox3.Name = "textBox3";
         this.textBox3.ReadOnly = true;
         this.textBox3.Size = new System.Drawing.Size(380, 26);
         this.textBox3.TabIndex = 240;
         this.textBox3.Text = "not connected";
         this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
         // RightTrackLightsStatusTextBox
         // 
         this.RightTrackLightsStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.RightTrackLightsStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.RightTrackLightsStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.RightTrackLightsStatusTextBox.Location = new System.Drawing.Point(317, 198);
         this.RightTrackLightsStatusTextBox.Name = "RightTrackLightsStatusTextBox";
         this.RightTrackLightsStatusTextBox.ReadOnly = true;
         this.RightTrackLightsStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.RightTrackLightsStatusTextBox.TabIndex = 234;
         this.RightTrackLightsStatusTextBox.Text = "not connected";
         this.RightTrackLightsStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // RightTrackLightsLabel
         // 
         this.RightTrackLightsLabel.BackColor = System.Drawing.Color.Teal;
         this.RightTrackLightsLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.RightTrackLightsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.RightTrackLightsLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.RightTrackLightsLabel.Location = new System.Drawing.Point(8, 200);
         this.RightTrackLightsLabel.Name = "RightTrackLightsLabel";
         this.RightTrackLightsLabel.Size = new System.Drawing.Size(301, 23);
         this.RightTrackLightsLabel.TabIndex = 233;
         this.RightTrackLightsLabel.Text = "RIGHT TRACK LIGHTS";
         this.RightTrackLightsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.RightTrackLightsLabel.Click += new System.EventHandler(this.RightTrackLightsLabel_Click);
         // 
         // RightTrackcontrollerStatusTextBox
         // 
         this.RightTrackcontrollerStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.RightTrackcontrollerStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.RightTrackcontrollerStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.RightTrackcontrollerStatusTextBox.Location = new System.Drawing.Point(317, 166);
         this.RightTrackcontrollerStatusTextBox.Name = "RightTrackcontrollerStatusTextBox";
         this.RightTrackcontrollerStatusTextBox.ReadOnly = true;
         this.RightTrackcontrollerStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.RightTrackcontrollerStatusTextBox.TabIndex = 232;
         this.RightTrackcontrollerStatusTextBox.Text = "not connected";
         this.RightTrackcontrollerStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
         // LeftTrackLightsStatusTextBox
         // 
         this.LeftTrackLightsStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.LeftTrackLightsStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.LeftTrackLightsStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.LeftTrackLightsStatusTextBox.Location = new System.Drawing.Point(317, 102);
         this.LeftTrackLightsStatusTextBox.Name = "LeftTrackLightsStatusTextBox";
         this.LeftTrackLightsStatusTextBox.ReadOnly = true;
         this.LeftTrackLightsStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.LeftTrackLightsStatusTextBox.TabIndex = 228;
         this.LeftTrackLightsStatusTextBox.Text = "not connected";
         this.LeftTrackLightsStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // LeftTrackLightsLabel
         // 
         this.LeftTrackLightsLabel.BackColor = System.Drawing.Color.Teal;
         this.LeftTrackLightsLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.LeftTrackLightsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LeftTrackLightsLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.LeftTrackLightsLabel.Location = new System.Drawing.Point(8, 104);
         this.LeftTrackLightsLabel.Name = "LeftTrackLightsLabel";
         this.LeftTrackLightsLabel.Size = new System.Drawing.Size(301, 23);
         this.LeftTrackLightsLabel.TabIndex = 227;
         this.LeftTrackLightsLabel.Text = "LEFT TRACK LIGHTS";
         this.LeftTrackLightsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.LeftTrackLightsLabel.Click += new System.EventHandler(this.LeftTrackLightsLabel_Click);
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
      private System.Windows.Forms.Label TargetBoardLabel;
      private System.Windows.Forms.TextBox TargetBusStatusTextBox;
      private System.Windows.Forms.TextBox TargetBoardStatusTextBox;
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
      private System.Windows.Forms.TextBox UsbRelayStatusTextBox;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TextBox HubControllerStatusTextBox;
      private System.Windows.Forms.Label HubControllerLabel;
      private System.Windows.Forms.TextBox LeftTrackLightsStatusTextBox;
      private System.Windows.Forms.Label LeftTrackLightsLabel;
      private System.Windows.Forms.TextBox TargetBoardCameraLedStatusTextBox;
      private System.Windows.Forms.Label TargetBoardCameraLedLabel;
      private System.Windows.Forms.TextBox RightTrackMotorStatusTextBox;
      private System.Windows.Forms.Label RightTrackMotorLabel;
      private System.Windows.Forms.TextBox RightTrackLightsStatusTextBox;
      private System.Windows.Forms.Label RightTrackLightsLabel;
      private System.Windows.Forms.TextBox RightTrackcontrollerStatusTextBox;
      private System.Windows.Forms.Label RightTrackControllerLabel;
      private System.Windows.Forms.TextBox LeftTrackMotorStatusTextBox;
      private System.Windows.Forms.Label LeftTrackMotorLabel;
      private System.Windows.Forms.TextBox TargetBoardCameraStepperStatusTextBox;
      private System.Windows.Forms.Label TargetBoardCameraStepperLabel;
      private System.Windows.Forms.TextBox TargetBoardRearWheelStatusTextBox;
      private System.Windows.Forms.Label TargetBoardRearWheelLabel;
      private System.Windows.Forms.TextBox TargetBoardFrontWheelStatusTextBox;
      private System.Windows.Forms.Label TargetBoardFrontWheelLabel;
      private System.Windows.Forms.TextBox HubTiltMotorStatusTextBox;
      private System.Windows.Forms.Label HubTiltMotorLabel;
      private System.Windows.Forms.TextBox textBox3;
      private System.Windows.Forms.Label HubPanMotorLabel;
      private System.Windows.Forms.TextBox HubCameraLightsStatusTextBox;
      private System.Windows.Forms.Label HubCameraLightsLabel;
   }
}