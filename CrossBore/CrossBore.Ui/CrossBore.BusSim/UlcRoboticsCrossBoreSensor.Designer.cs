namespace CrossBore.BusSim
{
   partial class UlcRoboticsCrossBoreSensor
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UlcRoboticsCrossBoreSensor));
         this.MainTabControl = new System.Windows.Forms.TabControl();
         this.SensorTabPage = new System.Windows.Forms.TabPage();
         this.SetNumberofSensorReadingsButton = new System.Windows.Forms.Button();
         this.NumberOfSensorReadingsTextBox = new System.Windows.Forms.TextBox();
         this.label4 = new System.Windows.Forms.Label();
         this.SetPipeBoundaryButton = new System.Windows.Forms.Button();
         this.PipeSizeTextBox = new System.Windows.Forms.TextBox();
         this.label3 = new System.Windows.Forms.Label();
         this.NumberOfPointsTextBox = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.SensorBoreDataControl = new UlcRobotics.Ui.Controls.BoreDataControl();
         this.PdoTabPage = new System.Windows.Forms.TabPage();
         this.SubSystemStatusLabel = new System.Windows.Forms.Label();
         this.ErrorRegisterLabel = new System.Windows.Forms.Label();
         this.ProducerHeartbeatTimeLabel = new System.Windows.Forms.Label();
         this.ConsumerHeartbeatTimeLabel = new System.Windows.Forms.Label();
         this.ResetButton = new System.Windows.Forms.Button();
         this.label15 = new System.Windows.Forms.Label();
         this.EmergencyResetDataTextBox = new System.Windows.Forms.TextBox();
         this.EmergencyResetCheckBox = new System.Windows.Forms.CheckBox();
         this.BusIdTextBox = new System.Windows.Forms.TextBox();
         this.DeviceStateLabel = new System.Windows.Forms.Label();
         this.EnabledCheckBox = new System.Windows.Forms.CheckBox();
         this.DescriptionTextBox = new System.Windows.Forms.TextBox();
         this.NodeIdTextBox = new System.Windows.Forms.TextBox();
         this.label1 = new System.Windows.Forms.Label();
         this.ShowBoundaryLimitCheckBox = new System.Windows.Forms.CheckBox();
         this.ShowBoundaryCheckBox = new System.Windows.Forms.CheckBox();
         this.ShowSensorMarkCheckBox = new System.Windows.Forms.CheckBox();
         this.ShowSensorReadingLinesCheckBox = new System.Windows.Forms.CheckBox();
         this.ShowSensorBoundaryCheckBox = new System.Windows.Forms.CheckBox();
         this.MainTabControl.SuspendLayout();
         this.SensorTabPage.SuspendLayout();
         this.PdoTabPage.SuspendLayout();
         this.SuspendLayout();
         // 
         // MainTabControl
         // 
         this.MainTabControl.Controls.Add(this.SensorTabPage);
         this.MainTabControl.Controls.Add(this.PdoTabPage);
         this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.MainTabControl.Location = new System.Drawing.Point(0, 30);
         this.MainTabControl.Name = "MainTabControl";
         this.MainTabControl.SelectedIndex = 0;
         this.MainTabControl.Size = new System.Drawing.Size(930, 507);
         this.MainTabControl.TabIndex = 277;
         // 
         // SensorTabPage
         // 
         this.SensorTabPage.BackColor = System.Drawing.Color.Gainsboro;
         this.SensorTabPage.Controls.Add(this.ShowSensorBoundaryCheckBox);
         this.SensorTabPage.Controls.Add(this.ShowSensorReadingLinesCheckBox);
         this.SensorTabPage.Controls.Add(this.ShowSensorMarkCheckBox);
         this.SensorTabPage.Controls.Add(this.ShowBoundaryCheckBox);
         this.SensorTabPage.Controls.Add(this.ShowBoundaryLimitCheckBox);
         this.SensorTabPage.Controls.Add(this.SetNumberofSensorReadingsButton);
         this.SensorTabPage.Controls.Add(this.NumberOfSensorReadingsTextBox);
         this.SensorTabPage.Controls.Add(this.label4);
         this.SensorTabPage.Controls.Add(this.SetPipeBoundaryButton);
         this.SensorTabPage.Controls.Add(this.PipeSizeTextBox);
         this.SensorTabPage.Controls.Add(this.label3);
         this.SensorTabPage.Controls.Add(this.NumberOfPointsTextBox);
         this.SensorTabPage.Controls.Add(this.label2);
         this.SensorTabPage.Controls.Add(this.SensorBoreDataControl);
         this.SensorTabPage.Location = new System.Drawing.Point(4, 22);
         this.SensorTabPage.Name = "SensorTabPage";
         this.SensorTabPage.Padding = new System.Windows.Forms.Padding(3);
         this.SensorTabPage.Size = new System.Drawing.Size(922, 481);
         this.SensorTabPage.TabIndex = 0;
         this.SensorTabPage.Text = "Sensor";
         // 
         // SetNumberofSensorReadingsButton
         // 
         this.SetNumberofSensorReadingsButton.Location = new System.Drawing.Point(682, 124);
         this.SetNumberofSensorReadingsButton.Name = "SetNumberofSensorReadingsButton";
         this.SetNumberofSensorReadingsButton.Size = new System.Drawing.Size(47, 23);
         this.SetNumberofSensorReadingsButton.TabIndex = 278;
         this.SetNumberofSensorReadingsButton.Text = "Set";
         this.SetNumberofSensorReadingsButton.UseVisualStyleBackColor = true;
         this.SetNumberofSensorReadingsButton.Click += new System.EventHandler(this.SetNumberofSensorReadingsButton_Click);
         // 
         // NumberOfSensorReadingsTextBox
         // 
         this.NumberOfSensorReadingsTextBox.Location = new System.Drawing.Point(576, 126);
         this.NumberOfSensorReadingsTextBox.Name = "NumberOfSensorReadingsTextBox";
         this.NumberOfSensorReadingsTextBox.Size = new System.Drawing.Size(100, 20);
         this.NumberOfSensorReadingsTextBox.TabIndex = 277;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(486, 129);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(88, 13);
         this.label4.TabIndex = 276;
         this.label4.Text = "Number of Points";
         // 
         // SetPipeBoundaryButton
         // 
         this.SetPipeBoundaryButton.Location = new System.Drawing.Point(682, 43);
         this.SetPipeBoundaryButton.Name = "SetPipeBoundaryButton";
         this.SetPipeBoundaryButton.Size = new System.Drawing.Size(47, 23);
         this.SetPipeBoundaryButton.TabIndex = 275;
         this.SetPipeBoundaryButton.Text = "Set";
         this.SetPipeBoundaryButton.UseVisualStyleBackColor = true;
         this.SetPipeBoundaryButton.Click += new System.EventHandler(this.SetPipeBoundaryButton_Click);
         // 
         // PipeSizeTextBox
         // 
         this.PipeSizeTextBox.Location = new System.Drawing.Point(576, 45);
         this.PipeSizeTextBox.Name = "PipeSizeTextBox";
         this.PipeSizeTextBox.Size = new System.Drawing.Size(100, 20);
         this.PipeSizeTextBox.TabIndex = 4;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(547, 48);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(27, 13);
         this.label3.TabIndex = 3;
         this.label3.Text = "Size";
         // 
         // NumberOfPointsTextBox
         // 
         this.NumberOfPointsTextBox.Location = new System.Drawing.Point(576, 19);
         this.NumberOfPointsTextBox.Name = "NumberOfPointsTextBox";
         this.NumberOfPointsTextBox.Size = new System.Drawing.Size(100, 20);
         this.NumberOfPointsTextBox.TabIndex = 2;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(486, 22);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(88, 13);
         this.label2.TabIndex = 1;
         this.label2.Text = "Number of Points";
         // 
         // SensorBoreDataControl
         // 
         this.SensorBoreDataControl.BackColor = System.Drawing.SystemColors.Control;
         this.SensorBoreDataControl.BoundaryReadings = null;
         this.SensorBoreDataControl.CrossLocation = ((System.Drawing.PointF)(resources.GetObject("SensorBoreDataControl.CrossLocation")));
         this.SensorBoreDataControl.CrossSize = new System.Drawing.Size(16, 16);
         this.SensorBoreDataControl.Location = new System.Drawing.Point(6, 6);
         this.SensorBoreDataControl.Name = "SensorBoreDataControl";
         this.SensorBoreDataControl.SensorReadingCount = 4;
         this.SensorBoreDataControl.ShowBoundary = true;
         this.SensorBoreDataControl.ShowBoundaryLimit = true;
         this.SensorBoreDataControl.ShowSensorBoundary = true;
         this.SensorBoreDataControl.ShowSensorMark = true;
         this.SensorBoreDataControl.ShowSensorReadingLines = true;
         this.SensorBoreDataControl.Size = new System.Drawing.Size(469, 469);
         this.SensorBoreDataControl.TabIndex = 0;
         this.SensorBoreDataControl.Text = "Sensor";
         // 
         // PdoTabPage
         // 
         this.PdoTabPage.BackColor = System.Drawing.Color.Gainsboro;
         this.PdoTabPage.Controls.Add(this.SubSystemStatusLabel);
         this.PdoTabPage.Controls.Add(this.ErrorRegisterLabel);
         this.PdoTabPage.Controls.Add(this.ProducerHeartbeatTimeLabel);
         this.PdoTabPage.Controls.Add(this.ConsumerHeartbeatTimeLabel);
         this.PdoTabPage.Location = new System.Drawing.Point(4, 22);
         this.PdoTabPage.Name = "PdoTabPage";
         this.PdoTabPage.Padding = new System.Windows.Forms.Padding(3);
         this.PdoTabPage.Size = new System.Drawing.Size(922, 481);
         this.PdoTabPage.TabIndex = 1;
         this.PdoTabPage.Text = "PDO";
         // 
         // SubSystemStatusLabel
         // 
         this.SubSystemStatusLabel.AutoSize = true;
         this.SubSystemStatusLabel.Location = new System.Drawing.Point(6, 70);
         this.SubSystemStatusLabel.Name = "SubSystemStatusLabel";
         this.SubSystemStatusLabel.Size = new System.Drawing.Size(134, 13);
         this.SubSystemStatusLabel.TabIndex = 289;
         this.SubSystemStatusLabel.Text = "0x5000 Sub System Status";
         this.SubSystemStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // ErrorRegisterLabel
         // 
         this.ErrorRegisterLabel.AutoSize = true;
         this.ErrorRegisterLabel.Location = new System.Drawing.Point(6, 3);
         this.ErrorRegisterLabel.Name = "ErrorRegisterLabel";
         this.ErrorRegisterLabel.Size = new System.Drawing.Size(109, 13);
         this.ErrorRegisterLabel.TabIndex = 172;
         this.ErrorRegisterLabel.Text = "0x1001 Error Register";
         this.ErrorRegisterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // ProducerHeartbeatTimeLabel
         // 
         this.ProducerHeartbeatTimeLabel.AutoSize = true;
         this.ProducerHeartbeatTimeLabel.Location = new System.Drawing.Point(6, 47);
         this.ProducerHeartbeatTimeLabel.Name = "ProducerHeartbeatTimeLabel";
         this.ProducerHeartbeatTimeLabel.Size = new System.Drawing.Size(164, 13);
         this.ProducerHeartbeatTimeLabel.TabIndex = 171;
         this.ProducerHeartbeatTimeLabel.Text = "0x1017 Producer Heartbeat Time";
         this.ProducerHeartbeatTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // ConsumerHeartbeatTimeLabel
         // 
         this.ConsumerHeartbeatTimeLabel.AutoSize = true;
         this.ConsumerHeartbeatTimeLabel.Location = new System.Drawing.Point(6, 27);
         this.ConsumerHeartbeatTimeLabel.Name = "ConsumerHeartbeatTimeLabel";
         this.ConsumerHeartbeatTimeLabel.Size = new System.Drawing.Size(168, 13);
         this.ConsumerHeartbeatTimeLabel.TabIndex = 170;
         this.ConsumerHeartbeatTimeLabel.Text = "0x1016 Consumer Heartbeat Time";
         this.ConsumerHeartbeatTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // ResetButton
         // 
         this.ResetButton.Location = new System.Drawing.Point(580, 3);
         this.ResetButton.Name = "ResetButton";
         this.ResetButton.Size = new System.Drawing.Size(47, 23);
         this.ResetButton.TabIndex = 274;
         this.ResetButton.Text = "Reset";
         this.ResetButton.UseVisualStyleBackColor = true;
         // 
         // label15
         // 
         this.label15.AutoSize = true;
         this.label15.Location = new System.Drawing.Point(336, 7);
         this.label15.Name = "label15";
         this.label15.Size = new System.Drawing.Size(79, 13);
         this.label15.TabIndex = 275;
         this.label15.Text = "Additional Data";
         // 
         // EmergencyResetDataTextBox
         // 
         this.EmergencyResetDataTextBox.Location = new System.Drawing.Point(417, 4);
         this.EmergencyResetDataTextBox.MaxLength = 5;
         this.EmergencyResetDataTextBox.Name = "EmergencyResetDataTextBox";
         this.EmergencyResetDataTextBox.Size = new System.Drawing.Size(26, 20);
         this.EmergencyResetDataTextBox.TabIndex = 276;
         this.EmergencyResetDataTextBox.Text = "0";
         this.EmergencyResetDataTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // EmergencyResetCheckBox
         // 
         this.EmergencyResetCheckBox.AutoSize = true;
         this.EmergencyResetCheckBox.Location = new System.Drawing.Point(449, 6);
         this.EmergencyResetCheckBox.Name = "EmergencyResetCheckBox";
         this.EmergencyResetCheckBox.Size = new System.Drawing.Size(125, 17);
         this.EmergencyResetCheckBox.TabIndex = 273;
         this.EmergencyResetCheckBox.Text = "Emergency on Reset";
         this.EmergencyResetCheckBox.UseVisualStyleBackColor = true;
         // 
         // BusIdTextBox
         // 
         this.BusIdTextBox.Location = new System.Drawing.Point(245, 4);
         this.BusIdTextBox.MaxLength = 3;
         this.BusIdTextBox.Name = "BusIdTextBox";
         this.BusIdTextBox.ReadOnly = true;
         this.BusIdTextBox.Size = new System.Drawing.Size(15, 20);
         this.BusIdTextBox.TabIndex = 272;
         this.BusIdTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // DeviceStateLabel
         // 
         this.DeviceStateLabel.BackColor = System.Drawing.SystemColors.Control;
         this.DeviceStateLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.DeviceStateLabel.Location = new System.Drawing.Point(264, 4);
         this.DeviceStateLabel.Name = "DeviceStateLabel";
         this.DeviceStateLabel.Size = new System.Drawing.Size(66, 20);
         this.DeviceStateLabel.TabIndex = 271;
         this.DeviceStateLabel.Text = "OFF";
         this.DeviceStateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // EnabledCheckBox
         // 
         this.EnabledCheckBox.AutoSize = true;
         this.EnabledCheckBox.Location = new System.Drawing.Point(5, 7);
         this.EnabledCheckBox.Name = "EnabledCheckBox";
         this.EnabledCheckBox.Size = new System.Drawing.Size(15, 14);
         this.EnabledCheckBox.TabIndex = 270;
         this.EnabledCheckBox.UseVisualStyleBackColor = true;
         // 
         // DescriptionTextBox
         // 
         this.DescriptionTextBox.Location = new System.Drawing.Point(24, 4);
         this.DescriptionTextBox.MaxLength = 65535;
         this.DescriptionTextBox.Name = "DescriptionTextBox";
         this.DescriptionTextBox.Size = new System.Drawing.Size(150, 20);
         this.DescriptionTextBox.TabIndex = 269;
         this.DescriptionTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // NodeIdTextBox
         // 
         this.NodeIdTextBox.Location = new System.Drawing.Point(217, 4);
         this.NodeIdTextBox.MaxLength = 3;
         this.NodeIdTextBox.Name = "NodeIdTextBox";
         this.NodeIdTextBox.Size = new System.Drawing.Size(25, 20);
         this.NodeIdTextBox.TabIndex = 267;
         this.NodeIdTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(180, 7);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(36, 13);
         this.label1.TabIndex = 268;
         this.label1.Text = "Node:";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // ShowBoundaryLimitCheckBox
         // 
         this.ShowBoundaryLimitCheckBox.AutoSize = true;
         this.ShowBoundaryLimitCheckBox.Location = new System.Drawing.Point(549, 224);
         this.ShowBoundaryLimitCheckBox.Name = "ShowBoundaryLimitCheckBox";
         this.ShowBoundaryLimitCheckBox.Size = new System.Drawing.Size(95, 17);
         this.ShowBoundaryLimitCheckBox.TabIndex = 279;
         this.ShowBoundaryLimitCheckBox.Text = "Boundary Limit";
         this.ShowBoundaryLimitCheckBox.UseVisualStyleBackColor = true;
         this.ShowBoundaryLimitCheckBox.CheckedChanged += new System.EventHandler(this.ShowBoundaryLimitCheckBox_CheckedChanged);
         // 
         // ShowBoundaryCheckBox
         // 
         this.ShowBoundaryCheckBox.AutoSize = true;
         this.ShowBoundaryCheckBox.Location = new System.Drawing.Point(549, 247);
         this.ShowBoundaryCheckBox.Name = "ShowBoundaryCheckBox";
         this.ShowBoundaryCheckBox.Size = new System.Drawing.Size(71, 17);
         this.ShowBoundaryCheckBox.TabIndex = 280;
         this.ShowBoundaryCheckBox.Text = "Boundary";
         this.ShowBoundaryCheckBox.UseVisualStyleBackColor = true;
         this.ShowBoundaryCheckBox.CheckedChanged += new System.EventHandler(this.ShowBoundaryCheckBox_CheckedChanged);
         // 
         // ShowSensorMarkCheckBox
         // 
         this.ShowSensorMarkCheckBox.AutoSize = true;
         this.ShowSensorMarkCheckBox.Location = new System.Drawing.Point(549, 270);
         this.ShowSensorMarkCheckBox.Name = "ShowSensorMarkCheckBox";
         this.ShowSensorMarkCheckBox.Size = new System.Drawing.Size(86, 17);
         this.ShowSensorMarkCheckBox.TabIndex = 281;
         this.ShowSensorMarkCheckBox.Text = "Sensor Mark";
         this.ShowSensorMarkCheckBox.UseVisualStyleBackColor = true;
         this.ShowSensorMarkCheckBox.CheckedChanged += new System.EventHandler(this.ShowSensorMarkCheckBox_CheckedChanged);
         // 
         // ShowSensorReadingLinesCheckBox
         // 
         this.ShowSensorReadingLinesCheckBox.AutoSize = true;
         this.ShowSensorReadingLinesCheckBox.Location = new System.Drawing.Point(549, 293);
         this.ShowSensorReadingLinesCheckBox.Name = "ShowSensorReadingLinesCheckBox";
         this.ShowSensorReadingLinesCheckBox.Size = new System.Drawing.Size(130, 17);
         this.ShowSensorReadingLinesCheckBox.TabIndex = 282;
         this.ShowSensorReadingLinesCheckBox.Text = "Sensor Reading Lines";
         this.ShowSensorReadingLinesCheckBox.UseVisualStyleBackColor = true;
         this.ShowSensorReadingLinesCheckBox.CheckedChanged += new System.EventHandler(this.ShowSensorReadingLinesCheckBox_CheckedChanged);
         // 
         // ShowSensorBoundaryCheckBox
         // 
         this.ShowSensorBoundaryCheckBox.AutoSize = true;
         this.ShowSensorBoundaryCheckBox.Location = new System.Drawing.Point(550, 316);
         this.ShowSensorBoundaryCheckBox.Name = "ShowSensorBoundaryCheckBox";
         this.ShowSensorBoundaryCheckBox.Size = new System.Drawing.Size(107, 17);
         this.ShowSensorBoundaryCheckBox.TabIndex = 283;
         this.ShowSensorBoundaryCheckBox.Text = "Sensor Boundary";
         this.ShowSensorBoundaryCheckBox.UseVisualStyleBackColor = true;
         this.ShowSensorBoundaryCheckBox.CheckedChanged += new System.EventHandler(this.ShowSensorBoundaryCheckBox_CheckedChanged);
         // 
         // UlcRoboticsCrossBoreSensor
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.Gainsboro;
         this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.Controls.Add(this.MainTabControl);
         this.Controls.Add(this.ResetButton);
         this.Controls.Add(this.label15);
         this.Controls.Add(this.EmergencyResetDataTextBox);
         this.Controls.Add(this.EmergencyResetCheckBox);
         this.Controls.Add(this.BusIdTextBox);
         this.Controls.Add(this.DeviceStateLabel);
         this.Controls.Add(this.EnabledCheckBox);
         this.Controls.Add(this.DescriptionTextBox);
         this.Controls.Add(this.NodeIdTextBox);
         this.Controls.Add(this.label1);
         this.Name = "UlcRoboticsCrossBoreSensor";
         this.Size = new System.Drawing.Size(930, 537);
         this.MainTabControl.ResumeLayout(false);
         this.SensorTabPage.ResumeLayout(false);
         this.SensorTabPage.PerformLayout();
         this.PdoTabPage.ResumeLayout(false);
         this.PdoTabPage.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Button ResetButton;
      private System.Windows.Forms.Label label15;
      private System.Windows.Forms.TextBox EmergencyResetDataTextBox;
      private System.Windows.Forms.CheckBox EmergencyResetCheckBox;
      private System.Windows.Forms.TextBox BusIdTextBox;
      private System.Windows.Forms.Label DeviceStateLabel;
      private System.Windows.Forms.CheckBox EnabledCheckBox;
      private System.Windows.Forms.TextBox DescriptionTextBox;
      private System.Windows.Forms.TextBox NodeIdTextBox;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TabControl MainTabControl;
      private System.Windows.Forms.TabPage SensorTabPage;
      private System.Windows.Forms.TabPage PdoTabPage;
      private UlcRobotics.Ui.Controls.BoreDataControl SensorBoreDataControl;
      private System.Windows.Forms.Label ProducerHeartbeatTimeLabel;
      private System.Windows.Forms.Label ConsumerHeartbeatTimeLabel;
      private System.Windows.Forms.Label ErrorRegisterLabel;
      private System.Windows.Forms.Label SubSystemStatusLabel;
      private System.Windows.Forms.Button SetPipeBoundaryButton;
      private System.Windows.Forms.TextBox PipeSizeTextBox;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.TextBox NumberOfPointsTextBox;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Button SetNumberofSensorReadingsButton;
      private System.Windows.Forms.TextBox NumberOfSensorReadingsTextBox;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.CheckBox ShowSensorBoundaryCheckBox;
      private System.Windows.Forms.CheckBox ShowSensorReadingLinesCheckBox;
      private System.Windows.Forms.CheckBox ShowSensorMarkCheckBox;
      private System.Windows.Forms.CheckBox ShowBoundaryCheckBox;
      private System.Windows.Forms.CheckBox ShowBoundaryLimitCheckBox;

   }
}
