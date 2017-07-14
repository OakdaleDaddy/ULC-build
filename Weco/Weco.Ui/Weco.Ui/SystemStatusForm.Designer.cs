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
         this.MainPanel = new Controls.BorderedPanel();
         this.LocationPanel = new Controls.BorderedPanel();
         this.SensorLongitudeTextPanel = new Controls.TextPanel();
         this.SensorLatitudeTextPanel = new Controls.TextPanel();
         this.label35 = new System.Windows.Forms.Label();
         this.label37 = new System.Windows.Forms.Label();
         this.label23 = new System.Windows.Forms.Label();
         this.SelectPanel = new Controls.BorderedPanel();
         this.SystemResetButton = new Controls.HoldButton();
         this.ShowToggleButton = new Controls.BaseButton();
         this.LoggingPanel = new Controls.BorderedPanel();
         this.TargetBusHeartbeatButton = new Controls.BaseButton();
         this.MainBusHeartbeatButton = new Controls.BaseButton();
         this.LoggingPortValueButton = new Controls.ValueButton();
         this.LoggingAddressValueButton = new Controls.ValueButton();
         this.label14 = new System.Windows.Forms.Label();
         this.DeviceStatusPanel = new Controls.BorderedPanel();
         this.DeviceStatusBPanel = new Controls.BorderedPanel();
         this.UsbRelayStatusTextBox = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.TargetBoardLabel = new System.Windows.Forms.Label();
         this.TargetBusStatusTextBox = new System.Windows.Forms.TextBox();
         this.TargetBoardStatusTextBox = new System.Windows.Forms.TextBox();
         this.DeviceStatusAPanel = new Controls.BorderedPanel();
         this.LaserBoardCameraLedStatusTextBox = new System.Windows.Forms.TextBox();
         this.LaserBoardCameraLedLabel = new System.Windows.Forms.Label();
         this.GpsStatusTextBox = new System.Windows.Forms.TextBox();
         this.GpsLabel = new System.Windows.Forms.Label();
         this.label26 = new System.Windows.Forms.Label();
         this.LaserBusStatusTextBox = new System.Windows.Forms.TextBox();
         this.LaserBoardStatusTextBox = new System.Windows.Forms.TextBox();
         this.LaserBoardLabel = new System.Windows.Forms.Label();
         this.LaserBusLabel = new System.Windows.Forms.Label();
         this.JoystickStatusTextBox = new System.Windows.Forms.TextBox();
         this.ComponentStatusLabel = new System.Windows.Forms.Label();
         this.SettingsPanel = new Controls.BorderedPanel();
         this.SaveDefaultsButton = new Controls.HoldButton();
         this.TriggerDefaultsButton = new Controls.HoldButton();
         this.label15 = new System.Windows.Forms.Label();
         this.BackButton = new Controls.BaseButton();
         this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
         this.TargetBoardCameraLedStatusTextBox = new System.Windows.Forms.TextBox();
         this.TargetBoardCameraLedLabel = new System.Windows.Forms.Label();
         this.LaserBoardFrontWheelStatusTextBox = new System.Windows.Forms.TextBox();
         this.LaserBoardFrontWheelLabel = new System.Windows.Forms.Label();
         this.LaserBoardRearWheelStatusTextBox = new System.Windows.Forms.TextBox();
         this.LaserBoardRearWheelLabel = new System.Windows.Forms.Label();
         this.LaserBoardLeftStepperStatusTextBox = new System.Windows.Forms.TextBox();
         this.LaserBoardLeftStepperLabel = new System.Windows.Forms.Label();
         this.LaserBoardRightStepperStatusTextBox = new System.Windows.Forms.TextBox();
         this.LaserBoardRightStepperLabel = new System.Windows.Forms.Label();
         this.TargetBoardCameraStepperStatusTextBox = new System.Windows.Forms.TextBox();
         this.TargetBoardCameraStepperLabel = new System.Windows.Forms.Label();
         this.TargetBoardRearWheelStatusTextBox = new System.Windows.Forms.TextBox();
         this.TargetBoardRearWheelLabel = new System.Windows.Forms.Label();
         this.TargetBoardFrontWheelStatusTextBox = new System.Windows.Forms.TextBox();
         this.TargetBoardFrontWheelLabel = new System.Windows.Forms.Label();
         this.MainPanel.SuspendLayout();
         this.LocationPanel.SuspendLayout();
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
         this.MainPanel.Controls.Add(this.LocationPanel);
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
         // LocationPanel
         // 
         this.LocationPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
         this.LocationPanel.Controls.Add(this.SensorLongitudeTextPanel);
         this.LocationPanel.Controls.Add(this.SensorLatitudeTextPanel);
         this.LocationPanel.Controls.Add(this.label35);
         this.LocationPanel.Controls.Add(this.label37);
         this.LocationPanel.Controls.Add(this.label23);
         this.LocationPanel.EdgeWeight = 3;
         this.LocationPanel.Location = new System.Drawing.Point(1490, 367);
         this.LocationPanel.Name = "LocationPanel";
         this.LocationPanel.Size = new System.Drawing.Size(275, 130);
         this.LocationPanel.TabIndex = 9;
         // 
         // SensorLongitudeTextPanel
         // 
         this.SensorLongitudeTextPanel.BackColor = System.Drawing.Color.Black;
         this.SensorLongitudeTextPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.SensorLongitudeTextPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.SensorLongitudeTextPanel.ForeColor = System.Drawing.Color.White;
         this.SensorLongitudeTextPanel.HoldTimeoutEnable = true;
         this.SensorLongitudeTextPanel.HoldTimeoutInterval = 100;
         this.SensorLongitudeTextPanel.Location = new System.Drawing.Point(142, 69);
         this.SensorLongitudeTextPanel.Name = "SensorLongitudeTextPanel";
         this.SensorLongitudeTextPanel.Size = new System.Drawing.Size(99, 42);
         this.SensorLongitudeTextPanel.TabIndex = 208;
         this.SensorLongitudeTextPanel.ValueText = "### °";
         this.SensorLongitudeTextPanel.HoldTimeout += new Controls.TextPanel.HoldTimeoutHandler2(this.SensorLongitudeTextPanel_HoldTimeout);
         // 
         // SensorLatitudeTextPanel
         // 
         this.SensorLatitudeTextPanel.BackColor = System.Drawing.Color.Black;
         this.SensorLatitudeTextPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.SensorLatitudeTextPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.SensorLatitudeTextPanel.ForeColor = System.Drawing.Color.White;
         this.SensorLatitudeTextPanel.HoldTimeoutEnable = true;
         this.SensorLatitudeTextPanel.HoldTimeoutInterval = 100;
         this.SensorLatitudeTextPanel.Location = new System.Drawing.Point(27, 69);
         this.SensorLatitudeTextPanel.Name = "SensorLatitudeTextPanel";
         this.SensorLatitudeTextPanel.Size = new System.Drawing.Size(99, 42);
         this.SensorLatitudeTextPanel.TabIndex = 207;
         this.SensorLatitudeTextPanel.ValueText = "### °";
         this.SensorLatitudeTextPanel.HoldTimeout += new Controls.TextPanel.HoldTimeoutHandler2(this.SensorLatitudeTextPanel_HoldTimeout);
         // 
         // label35
         // 
         this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label35.Location = new System.Drawing.Point(142, 46);
         this.label35.Name = "label35";
         this.label35.Size = new System.Drawing.Size(99, 20);
         this.label35.TabIndex = 205;
         this.label35.Text = "LONGITUDE";
         this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label37
         // 
         this.label37.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label37.Location = new System.Drawing.Point(27, 46);
         this.label37.Name = "label37";
         this.label37.Size = new System.Drawing.Size(99, 20);
         this.label37.TabIndex = 206;
         this.label37.Text = "LATITUDE";
         this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label23
         // 
         this.label23.BackColor = System.Drawing.Color.Teal;
         this.label23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label23.ForeColor = System.Drawing.Color.White;
         this.label23.Location = new System.Drawing.Point(16, 16);
         this.label23.Name = "label23";
         this.label23.Size = new System.Drawing.Size(243, 23);
         this.label23.TabIndex = 204;
         this.label23.Text = "LOCATION";
         this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
         this.SystemResetButton.HoldTimeout += new Controls.HoldTimeoutHandler(this.SystemResetButton_HoldTimeout);
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
         this.DeviceStatusAPanel.Controls.Add(this.LaserBoardRightStepperStatusTextBox);
         this.DeviceStatusAPanel.Controls.Add(this.LaserBoardRightStepperLabel);
         this.DeviceStatusAPanel.Controls.Add(this.LaserBoardLeftStepperStatusTextBox);
         this.DeviceStatusAPanel.Controls.Add(this.LaserBoardLeftStepperLabel);
         this.DeviceStatusAPanel.Controls.Add(this.LaserBoardRearWheelStatusTextBox);
         this.DeviceStatusAPanel.Controls.Add(this.LaserBoardRearWheelLabel);
         this.DeviceStatusAPanel.Controls.Add(this.LaserBoardFrontWheelStatusTextBox);
         this.DeviceStatusAPanel.Controls.Add(this.LaserBoardFrontWheelLabel);
         this.DeviceStatusAPanel.Controls.Add(this.LaserBoardCameraLedStatusTextBox);
         this.DeviceStatusAPanel.Controls.Add(this.LaserBoardCameraLedLabel);
         this.DeviceStatusAPanel.Controls.Add(this.GpsStatusTextBox);
         this.DeviceStatusAPanel.Controls.Add(this.GpsLabel);
         this.DeviceStatusAPanel.Controls.Add(this.label26);
         this.DeviceStatusAPanel.Controls.Add(this.LaserBusStatusTextBox);
         this.DeviceStatusAPanel.Controls.Add(this.LaserBoardStatusTextBox);
         this.DeviceStatusAPanel.Controls.Add(this.LaserBoardLabel);
         this.DeviceStatusAPanel.Controls.Add(this.LaserBusLabel);
         this.DeviceStatusAPanel.Controls.Add(this.JoystickStatusTextBox);
         this.DeviceStatusAPanel.EdgeWeight = 1;
         this.DeviceStatusAPanel.Location = new System.Drawing.Point(24, 47);
         this.DeviceStatusAPanel.Name = "DeviceStatusAPanel";
         this.DeviceStatusAPanel.Size = new System.Drawing.Size(705, 648);
         this.DeviceStatusAPanel.TabIndex = 202;
         // 
         // LaserBoardCameraLedStatusTextBox
         // 
         this.LaserBoardCameraLedStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.LaserBoardCameraLedStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.LaserBoardCameraLedStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.LaserBoardCameraLedStatusTextBox.Location = new System.Drawing.Point(317, 102);
         this.LaserBoardCameraLedStatusTextBox.Name = "LaserBoardCameraLedStatusTextBox";
         this.LaserBoardCameraLedStatusTextBox.ReadOnly = true;
         this.LaserBoardCameraLedStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.LaserBoardCameraLedStatusTextBox.TabIndex = 228;
         this.LaserBoardCameraLedStatusTextBox.Text = "not connected";
         this.LaserBoardCameraLedStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // LaserBoardCameraLedLabel
         // 
         this.LaserBoardCameraLedLabel.BackColor = System.Drawing.Color.Teal;
         this.LaserBoardCameraLedLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.LaserBoardCameraLedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LaserBoardCameraLedLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.LaserBoardCameraLedLabel.Location = new System.Drawing.Point(8, 104);
         this.LaserBoardCameraLedLabel.Name = "LaserBoardCameraLedLabel";
         this.LaserBoardCameraLedLabel.Size = new System.Drawing.Size(301, 23);
         this.LaserBoardCameraLedLabel.TabIndex = 227;
         this.LaserBoardCameraLedLabel.Text = "LASER BOARD CAMERA/LED";
         this.LaserBoardCameraLedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.LaserBoardCameraLedLabel.Click += new System.EventHandler(this.LaserBoardCameraLedLabel_Click);
         // 
         // GpsStatusTextBox
         // 
         this.GpsStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.GpsStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.GpsStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.GpsStatusTextBox.Location = new System.Drawing.Point(317, 262);
         this.GpsStatusTextBox.Name = "GpsStatusTextBox";
         this.GpsStatusTextBox.ReadOnly = true;
         this.GpsStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.GpsStatusTextBox.TabIndex = 226;
         this.GpsStatusTextBox.Text = "not connected";
         this.GpsStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // GpsLabel
         // 
         this.GpsLabel.BackColor = System.Drawing.Color.Teal;
         this.GpsLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.GpsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.GpsLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.GpsLabel.Location = new System.Drawing.Point(8, 264);
         this.GpsLabel.Name = "GpsLabel";
         this.GpsLabel.Size = new System.Drawing.Size(301, 23);
         this.GpsLabel.TabIndex = 225;
         this.GpsLabel.Text = "GPS";
         this.GpsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.GpsLabel.Click += new System.EventHandler(this.GpsLabel_Click);
         // 
         // label26
         // 
         this.label26.BackColor = System.Drawing.Color.Teal;
         this.label26.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label26.ForeColor = System.Drawing.Color.Gainsboro;
         this.label26.Location = new System.Drawing.Point(8, 8);
         this.label26.Name = "label26";
         this.label26.Size = new System.Drawing.Size(301, 23);
         this.label26.TabIndex = 224;
         this.label26.Text = "JOYSTICK";
         this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // LaserBusStatusTextBox
         // 
         this.LaserBusStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.LaserBusStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.LaserBusStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.LaserBusStatusTextBox.Location = new System.Drawing.Point(317, 38);
         this.LaserBusStatusTextBox.Name = "LaserBusStatusTextBox";
         this.LaserBusStatusTextBox.ReadOnly = true;
         this.LaserBusStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.LaserBusStatusTextBox.TabIndex = 219;
         this.LaserBusStatusTextBox.Text = "not connected";
         this.LaserBusStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // LaserBoardStatusTextBox
         // 
         this.LaserBoardStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.LaserBoardStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.LaserBoardStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.LaserBoardStatusTextBox.Location = new System.Drawing.Point(317, 70);
         this.LaserBoardStatusTextBox.Name = "LaserBoardStatusTextBox";
         this.LaserBoardStatusTextBox.ReadOnly = true;
         this.LaserBoardStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.LaserBoardStatusTextBox.TabIndex = 221;
         this.LaserBoardStatusTextBox.Text = "not connected";
         this.LaserBoardStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // LaserBoardLabel
         // 
         this.LaserBoardLabel.BackColor = System.Drawing.Color.Teal;
         this.LaserBoardLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.LaserBoardLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LaserBoardLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.LaserBoardLabel.Location = new System.Drawing.Point(8, 72);
         this.LaserBoardLabel.Name = "LaserBoardLabel";
         this.LaserBoardLabel.Size = new System.Drawing.Size(301, 23);
         this.LaserBoardLabel.TabIndex = 222;
         this.LaserBoardLabel.Text = "LASER BOARD";
         this.LaserBoardLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.LaserBoardLabel.Click += new System.EventHandler(this.LaserBoardLabel_Click);
         // 
         // LaserBusLabel
         // 
         this.LaserBusLabel.BackColor = System.Drawing.Color.Teal;
         this.LaserBusLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.LaserBusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LaserBusLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.LaserBusLabel.Location = new System.Drawing.Point(8, 40);
         this.LaserBusLabel.Name = "LaserBusLabel";
         this.LaserBusLabel.Size = new System.Drawing.Size(301, 23);
         this.LaserBusLabel.TabIndex = 220;
         this.LaserBusLabel.Text = "LASER BUS";
         this.LaserBusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
         this.SaveDefaultsButton.HoldTimeout += new Controls.HoldTimeoutHandler(this.SaveDefaultsButton_HoldTimeout);
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
         this.TriggerDefaultsButton.HoldTimeout += new Controls.HoldTimeoutHandler(this.TriggerDefaultsButton_HoldTimeout);
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
         // LaserBoardFrontWheelStatusTextBox
         // 
         this.LaserBoardFrontWheelStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.LaserBoardFrontWheelStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.LaserBoardFrontWheelStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.LaserBoardFrontWheelStatusTextBox.Location = new System.Drawing.Point(317, 134);
         this.LaserBoardFrontWheelStatusTextBox.Name = "LaserBoardFrontWheelStatusTextBox";
         this.LaserBoardFrontWheelStatusTextBox.ReadOnly = true;
         this.LaserBoardFrontWheelStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.LaserBoardFrontWheelStatusTextBox.TabIndex = 230;
         this.LaserBoardFrontWheelStatusTextBox.Text = "not connected";
         this.LaserBoardFrontWheelStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // LaserBoardFrontWheelLabel
         // 
         this.LaserBoardFrontWheelLabel.BackColor = System.Drawing.Color.Teal;
         this.LaserBoardFrontWheelLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.LaserBoardFrontWheelLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LaserBoardFrontWheelLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.LaserBoardFrontWheelLabel.Location = new System.Drawing.Point(8, 136);
         this.LaserBoardFrontWheelLabel.Name = "LaserBoardFrontWheelLabel";
         this.LaserBoardFrontWheelLabel.Size = new System.Drawing.Size(301, 23);
         this.LaserBoardFrontWheelLabel.TabIndex = 229;
         this.LaserBoardFrontWheelLabel.Text = "LASER BOARD FRONT WHEEL";
         this.LaserBoardFrontWheelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.LaserBoardFrontWheelLabel.Click += new System.EventHandler(this.LaserBoardFrontWheelLabel_Click);
         // 
         // LaserBoardRearWheelStatusTextBox
         // 
         this.LaserBoardRearWheelStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.LaserBoardRearWheelStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.LaserBoardRearWheelStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.LaserBoardRearWheelStatusTextBox.Location = new System.Drawing.Point(317, 166);
         this.LaserBoardRearWheelStatusTextBox.Name = "LaserBoardRearWheelStatusTextBox";
         this.LaserBoardRearWheelStatusTextBox.ReadOnly = true;
         this.LaserBoardRearWheelStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.LaserBoardRearWheelStatusTextBox.TabIndex = 232;
         this.LaserBoardRearWheelStatusTextBox.Text = "not connected";
         this.LaserBoardRearWheelStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // LaserBoardRearWheelLabel
         // 
         this.LaserBoardRearWheelLabel.BackColor = System.Drawing.Color.Teal;
         this.LaserBoardRearWheelLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.LaserBoardRearWheelLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LaserBoardRearWheelLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.LaserBoardRearWheelLabel.Location = new System.Drawing.Point(8, 168);
         this.LaserBoardRearWheelLabel.Name = "LaserBoardRearWheelLabel";
         this.LaserBoardRearWheelLabel.Size = new System.Drawing.Size(301, 23);
         this.LaserBoardRearWheelLabel.TabIndex = 231;
         this.LaserBoardRearWheelLabel.Text = "LASER BOARD REAR WHEEL";
         this.LaserBoardRearWheelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.LaserBoardRearWheelLabel.Click += new System.EventHandler(this.LaserBoardRearWheelLabel_Click);
         // 
         // LaserBoardLeftStepperStatusTextBox
         // 
         this.LaserBoardLeftStepperStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.LaserBoardLeftStepperStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.LaserBoardLeftStepperStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.LaserBoardLeftStepperStatusTextBox.Location = new System.Drawing.Point(317, 198);
         this.LaserBoardLeftStepperStatusTextBox.Name = "LaserBoardLeftStepperStatusTextBox";
         this.LaserBoardLeftStepperStatusTextBox.ReadOnly = true;
         this.LaserBoardLeftStepperStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.LaserBoardLeftStepperStatusTextBox.TabIndex = 234;
         this.LaserBoardLeftStepperStatusTextBox.Text = "not connected";
         this.LaserBoardLeftStepperStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // LaserBoardLeftStepperLabel
         // 
         this.LaserBoardLeftStepperLabel.BackColor = System.Drawing.Color.Teal;
         this.LaserBoardLeftStepperLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.LaserBoardLeftStepperLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LaserBoardLeftStepperLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.LaserBoardLeftStepperLabel.Location = new System.Drawing.Point(8, 200);
         this.LaserBoardLeftStepperLabel.Name = "LaserBoardLeftStepperLabel";
         this.LaserBoardLeftStepperLabel.Size = new System.Drawing.Size(301, 23);
         this.LaserBoardLeftStepperLabel.TabIndex = 233;
         this.LaserBoardLeftStepperLabel.Text = "LASER BOARD LEFT STEPPER";
         this.LaserBoardLeftStepperLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.LaserBoardLeftStepperLabel.Click += new System.EventHandler(this.LaserBoardLeftStepperLabel_Click);
         // 
         // LaserBoardRightStepperStatusTextBox
         // 
         this.LaserBoardRightStepperStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.LaserBoardRightStepperStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.LaserBoardRightStepperStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.LaserBoardRightStepperStatusTextBox.Location = new System.Drawing.Point(317, 230);
         this.LaserBoardRightStepperStatusTextBox.Name = "LaserBoardRightStepperStatusTextBox";
         this.LaserBoardRightStepperStatusTextBox.ReadOnly = true;
         this.LaserBoardRightStepperStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.LaserBoardRightStepperStatusTextBox.TabIndex = 236;
         this.LaserBoardRightStepperStatusTextBox.Text = "not connected";
         this.LaserBoardRightStepperStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // LaserBoardRightStepperLabel
         // 
         this.LaserBoardRightStepperLabel.BackColor = System.Drawing.Color.Teal;
         this.LaserBoardRightStepperLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.LaserBoardRightStepperLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LaserBoardRightStepperLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.LaserBoardRightStepperLabel.Location = new System.Drawing.Point(8, 232);
         this.LaserBoardRightStepperLabel.Name = "LaserBoardRightStepperLabel";
         this.LaserBoardRightStepperLabel.Size = new System.Drawing.Size(301, 23);
         this.LaserBoardRightStepperLabel.TabIndex = 235;
         this.LaserBoardRightStepperLabel.Text = "LASER BOARD RIGHT STEPPER";
         this.LaserBoardRightStepperLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.LaserBoardRightStepperLabel.Click += new System.EventHandler(this.LaserBoardRightStepperLabel_Click);
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
         this.LocationPanel.ResumeLayout(false);
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
      private System.Windows.Forms.Label label26;
      private System.Windows.Forms.TextBox LaserBusStatusTextBox;
      private System.Windows.Forms.TextBox LaserBoardStatusTextBox;
      private System.Windows.Forms.Label LaserBoardLabel;
      private System.Windows.Forms.Label LaserBusLabel;
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
      private System.Windows.Forms.TextBox GpsStatusTextBox;
      private System.Windows.Forms.Label GpsLabel;
      private Controls.BorderedPanel LocationPanel;
      private Controls.TextPanel SensorLongitudeTextPanel;
      private Controls.TextPanel SensorLatitudeTextPanel;
      private System.Windows.Forms.Label label35;
      private System.Windows.Forms.Label label37;
      private System.Windows.Forms.Label label23;
      private System.Windows.Forms.TextBox LaserBoardCameraLedStatusTextBox;
      private System.Windows.Forms.Label LaserBoardCameraLedLabel;
      private System.Windows.Forms.TextBox TargetBoardCameraLedStatusTextBox;
      private System.Windows.Forms.Label TargetBoardCameraLedLabel;
      private System.Windows.Forms.TextBox LaserBoardRightStepperStatusTextBox;
      private System.Windows.Forms.Label LaserBoardRightStepperLabel;
      private System.Windows.Forms.TextBox LaserBoardLeftStepperStatusTextBox;
      private System.Windows.Forms.Label LaserBoardLeftStepperLabel;
      private System.Windows.Forms.TextBox LaserBoardRearWheelStatusTextBox;
      private System.Windows.Forms.Label LaserBoardRearWheelLabel;
      private System.Windows.Forms.TextBox LaserBoardFrontWheelStatusTextBox;
      private System.Windows.Forms.Label LaserBoardFrontWheelLabel;
      private System.Windows.Forms.TextBox TargetBoardCameraStepperStatusTextBox;
      private System.Windows.Forms.Label TargetBoardCameraStepperLabel;
      private System.Windows.Forms.TextBox TargetBoardRearWheelStatusTextBox;
      private System.Windows.Forms.Label TargetBoardRearWheelLabel;
      private System.Windows.Forms.TextBox TargetBoardFrontWheelStatusTextBox;
      private System.Windows.Forms.Label TargetBoardFrontWheelLabel;
   }
}