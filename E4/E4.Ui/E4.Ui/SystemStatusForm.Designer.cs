namespace E4.Ui
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
         this.MainPanel = new E4.Ui.Controls.BorderedPanel();
         this.SelectPanel = new E4.Ui.Controls.BorderedPanel();
         this.SystemResetButton = new E4.Ui.Controls.HoldButton();
         this.ShowToggleButton = new E4.Ui.Controls.E4Button();
         this.LoggingPanel = new E4.Ui.Controls.BorderedPanel();
         this.TargetBusHeartbeatButton = new E4.Ui.Controls.E4Button();
         this.MainBusHeartbeatButton = new E4.Ui.Controls.E4Button();
         this.LoggingPortValueButton = new E4.Ui.Controls.ValueButton();
         this.LoggingAddressValueButton = new E4.Ui.Controls.ValueButton();
         this.label14 = new System.Windows.Forms.Label();
         this.DeviceStatusPanel = new E4.Ui.Controls.BorderedPanel();
         this.DeviceStatusBPanel = new E4.Ui.Controls.BorderedPanel();
         this.label1 = new System.Windows.Forms.Label();
         this.TargetBoardLabel = new System.Windows.Forms.Label();
         this.TargetBusStatusTextBox = new System.Windows.Forms.TextBox();
         this.TargetBoardStatusTextBox = new System.Windows.Forms.TextBox();
         this.DeviceStatusAPanel = new E4.Ui.Controls.BorderedPanel();
         this.label26 = new System.Windows.Forms.Label();
         this.MainBusStatusTextBox = new System.Windows.Forms.TextBox();
         this.LaserBoardStatusTextBox = new System.Windows.Forms.TextBox();
         this.LaserBoardLabel = new System.Windows.Forms.Label();
         this.MainBusLabel = new System.Windows.Forms.Label();
         this.JoystickStatusTextBox = new System.Windows.Forms.TextBox();
         this.ComponentStatusLabel = new System.Windows.Forms.Label();
         this.SettingsPanel = new E4.Ui.Controls.BorderedPanel();
         this.SaveDefaultsButton = new E4.Ui.Controls.HoldButton();
         this.TriggerDefaultsButton = new E4.Ui.Controls.HoldButton();
         this.label15 = new System.Windows.Forms.Label();
         this.BackButton = new E4.Ui.Controls.E4Button();
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
         this.SystemResetButton.HoldTimeout += new E4.Ui.Controls.HoldTimeoutHandler(this.SystemResetButton_HoldTimeout);
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
         this.DeviceStatusAPanel.Controls.Add(this.label26);
         this.DeviceStatusAPanel.Controls.Add(this.MainBusStatusTextBox);
         this.DeviceStatusAPanel.Controls.Add(this.LaserBoardStatusTextBox);
         this.DeviceStatusAPanel.Controls.Add(this.LaserBoardLabel);
         this.DeviceStatusAPanel.Controls.Add(this.MainBusLabel);
         this.DeviceStatusAPanel.Controls.Add(this.JoystickStatusTextBox);
         this.DeviceStatusAPanel.EdgeWeight = 1;
         this.DeviceStatusAPanel.Location = new System.Drawing.Point(24, 47);
         this.DeviceStatusAPanel.Name = "DeviceStatusAPanel";
         this.DeviceStatusAPanel.Size = new System.Drawing.Size(705, 648);
         this.DeviceStatusAPanel.TabIndex = 202;
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
         // MainBusStatusTextBox
         // 
         this.MainBusStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.MainBusStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.MainBusStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.MainBusStatusTextBox.Location = new System.Drawing.Point(317, 38);
         this.MainBusStatusTextBox.Name = "MainBusStatusTextBox";
         this.MainBusStatusTextBox.ReadOnly = true;
         this.MainBusStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.MainBusStatusTextBox.TabIndex = 219;
         this.MainBusStatusTextBox.Text = "not connected";
         this.MainBusStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
         // MainBusLabel
         // 
         this.MainBusLabel.BackColor = System.Drawing.Color.Teal;
         this.MainBusLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.MainBusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.MainBusLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.MainBusLabel.Location = new System.Drawing.Point(8, 40);
         this.MainBusLabel.Name = "MainBusLabel";
         this.MainBusLabel.Size = new System.Drawing.Size(301, 23);
         this.MainBusLabel.TabIndex = 220;
         this.MainBusLabel.Text = "MAIN BUS";
         this.MainBusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
         this.SaveDefaultsButton.HoldTimeout += new E4.Ui.Controls.HoldTimeoutHandler(this.SaveDefaultsButton_HoldTimeout);
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
         this.TriggerDefaultsButton.HoldTimeout += new E4.Ui.Controls.HoldTimeoutHandler(this.TriggerDefaultsButton_HoldTimeout);
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
      private Controls.E4Button BackButton;
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
      private System.Windows.Forms.TextBox MainBusStatusTextBox;
      private System.Windows.Forms.TextBox LaserBoardStatusTextBox;
      private System.Windows.Forms.Label LaserBoardLabel;
      private System.Windows.Forms.Label MainBusLabel;
      private System.Windows.Forms.TextBox JoystickStatusTextBox;
      private System.Windows.Forms.Timer UpdateTimer;
      private Controls.BorderedPanel LoggingPanel;
      private Controls.ValueButton LoggingAddressValueButton;
      private System.Windows.Forms.Label label14;
      private Controls.E4Button TargetBusHeartbeatButton;
      private Controls.E4Button MainBusHeartbeatButton;
      private Controls.ValueButton LoggingPortValueButton;
      private Controls.BorderedPanel SelectPanel;
      private Controls.HoldButton SystemResetButton;
      private Controls.E4Button ShowToggleButton;
   }
}