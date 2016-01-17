namespace NICBOT.BusSim
{
   partial class UlcRoboticsGps
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
         this.SetLatitudeButton = new System.Windows.Forms.Button();
         this.LatitudeReportTextBox = new System.Windows.Forms.TextBox();
         this.LatitudeEntryTextBox = new System.Windows.Forms.TextBox();
         this.DescriptorLabel = new System.Windows.Forms.Label();
         this.SetLongitudeButton = new System.Windows.Forms.Button();
         this.LongitudeReportTextBox = new System.Windows.Forms.TextBox();
         this.LongitudeEntryTextBox = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.LatitudeTrackBar = new System.Windows.Forms.TrackBar();
         this.LongitudeTrackBar = new System.Windows.Forms.TrackBar();
         this.SetStatusButton = new System.Windows.Forms.Button();
         this.StatusTextBox = new System.Windows.Forms.TextBox();
         this.label3 = new System.Windows.Forms.Label();
         this.UtcValidCheckBox = new System.Windows.Forms.CheckBox();
         this.SetSatelliteButton = new System.Windows.Forms.Button();
         this.SatelliteTextBox = new System.Windows.Forms.TextBox();
         this.label4 = new System.Windows.Forms.Label();
         this.SetMethodButton = new System.Windows.Forms.Button();
         this.MethodTextBox = new System.Windows.Forms.TextBox();
         this.label5 = new System.Windows.Forms.Label();
         ((System.ComponentModel.ISupportInitialize)(this.LatitudeTrackBar)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.LongitudeTrackBar)).BeginInit();
         this.SuspendLayout();
         // 
         // BusIdTextBox
         // 
         this.BusIdTextBox.Location = new System.Drawing.Point(245, 4);
         this.BusIdTextBox.MaxLength = 3;
         this.BusIdTextBox.Name = "BusIdTextBox";
         this.BusIdTextBox.ReadOnly = true;
         this.BusIdTextBox.Size = new System.Drawing.Size(15, 20);
         this.BusIdTextBox.TabIndex = 130;
         this.BusIdTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // DeviceStateLabel
         // 
         this.DeviceStateLabel.BackColor = System.Drawing.SystemColors.Control;
         this.DeviceStateLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.DeviceStateLabel.Location = new System.Drawing.Point(264, 4);
         this.DeviceStateLabel.Name = "DeviceStateLabel";
         this.DeviceStateLabel.Size = new System.Drawing.Size(66, 20);
         this.DeviceStateLabel.TabIndex = 129;
         this.DeviceStateLabel.Text = "OFF";
         this.DeviceStateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // EnabledCheckBox
         // 
         this.EnabledCheckBox.AutoSize = true;
         this.EnabledCheckBox.Location = new System.Drawing.Point(5, 7);
         this.EnabledCheckBox.Name = "EnabledCheckBox";
         this.EnabledCheckBox.Size = new System.Drawing.Size(15, 14);
         this.EnabledCheckBox.TabIndex = 128;
         this.EnabledCheckBox.UseVisualStyleBackColor = true;
         // 
         // DescriptionTextBox
         // 
         this.DescriptionTextBox.Location = new System.Drawing.Point(24, 4);
         this.DescriptionTextBox.MaxLength = 65535;
         this.DescriptionTextBox.Name = "DescriptionTextBox";
         this.DescriptionTextBox.Size = new System.Drawing.Size(150, 20);
         this.DescriptionTextBox.TabIndex = 127;
         this.DescriptionTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // NodeIdTextBox
         // 
         this.NodeIdTextBox.Location = new System.Drawing.Point(217, 4);
         this.NodeIdTextBox.MaxLength = 3;
         this.NodeIdTextBox.Name = "NodeIdTextBox";
         this.NodeIdTextBox.ReadOnly = true;
         this.NodeIdTextBox.Size = new System.Drawing.Size(25, 20);
         this.NodeIdTextBox.TabIndex = 125;
         this.NodeIdTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(180, 7);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(36, 13);
         this.label1.TabIndex = 126;
         this.label1.Text = "Node:";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // SetLatitudeButton
         // 
         this.SetLatitudeButton.Location = new System.Drawing.Point(135, 28);
         this.SetLatitudeButton.Name = "SetLatitudeButton";
         this.SetLatitudeButton.Size = new System.Drawing.Size(32, 23);
         this.SetLatitudeButton.TabIndex = 134;
         this.SetLatitudeButton.Text = "set";
         this.SetLatitudeButton.UseVisualStyleBackColor = true;
         this.SetLatitudeButton.Click += new System.EventHandler(this.SetLatitudeButton_Click);
         // 
         // LatitudeReportTextBox
         // 
         this.LatitudeReportTextBox.Enabled = false;
         this.LatitudeReportTextBox.Location = new System.Drawing.Point(173, 30);
         this.LatitudeReportTextBox.MaxLength = 0;
         this.LatitudeReportTextBox.Name = "LatitudeReportTextBox";
         this.LatitudeReportTextBox.ReadOnly = true;
         this.LatitudeReportTextBox.Size = new System.Drawing.Size(66, 20);
         this.LatitudeReportTextBox.TabIndex = 133;
         this.LatitudeReportTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // LatitudeEntryTextBox
         // 
         this.LatitudeEntryTextBox.Location = new System.Drawing.Point(63, 30);
         this.LatitudeEntryTextBox.MaxLength = 0;
         this.LatitudeEntryTextBox.Name = "LatitudeEntryTextBox";
         this.LatitudeEntryTextBox.Size = new System.Drawing.Size(66, 20);
         this.LatitudeEntryTextBox.TabIndex = 132;
         this.LatitudeEntryTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // DescriptorLabel
         // 
         this.DescriptorLabel.AutoSize = true;
         this.DescriptorLabel.Location = new System.Drawing.Point(15, 33);
         this.DescriptorLabel.Name = "DescriptorLabel";
         this.DescriptorLabel.Size = new System.Drawing.Size(45, 13);
         this.DescriptorLabel.TabIndex = 131;
         this.DescriptorLabel.Text = "Latitude";
         this.DescriptorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // SetLongitudeButton
         // 
         this.SetLongitudeButton.Location = new System.Drawing.Point(135, 81);
         this.SetLongitudeButton.Name = "SetLongitudeButton";
         this.SetLongitudeButton.Size = new System.Drawing.Size(32, 23);
         this.SetLongitudeButton.TabIndex = 138;
         this.SetLongitudeButton.Text = "set";
         this.SetLongitudeButton.UseVisualStyleBackColor = true;
         this.SetLongitudeButton.Click += new System.EventHandler(this.SetLongitudeButton_Click);
         // 
         // LongitudeReportTextBox
         // 
         this.LongitudeReportTextBox.Enabled = false;
         this.LongitudeReportTextBox.Location = new System.Drawing.Point(173, 83);
         this.LongitudeReportTextBox.MaxLength = 0;
         this.LongitudeReportTextBox.Name = "LongitudeReportTextBox";
         this.LongitudeReportTextBox.ReadOnly = true;
         this.LongitudeReportTextBox.Size = new System.Drawing.Size(66, 20);
         this.LongitudeReportTextBox.TabIndex = 137;
         this.LongitudeReportTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // LongitudeEntryTextBox
         // 
         this.LongitudeEntryTextBox.Location = new System.Drawing.Point(63, 83);
         this.LongitudeEntryTextBox.MaxLength = 0;
         this.LongitudeEntryTextBox.Name = "LongitudeEntryTextBox";
         this.LongitudeEntryTextBox.Size = new System.Drawing.Size(66, 20);
         this.LongitudeEntryTextBox.TabIndex = 136;
         this.LongitudeEntryTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(7, 86);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(54, 13);
         this.label2.TabIndex = 135;
         this.label2.Text = "Longitude";
         this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // LatitudeTrackBar
         // 
         this.LatitudeTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.LatitudeTrackBar.AutoSize = false;
         this.LatitudeTrackBar.Location = new System.Drawing.Point(6, 57);
         this.LatitudeTrackBar.Maximum = 90000000;
         this.LatitudeTrackBar.Minimum = -90000000;
         this.LatitudeTrackBar.Name = "LatitudeTrackBar";
         this.LatitudeTrackBar.Size = new System.Drawing.Size(604, 20);
         this.LatitudeTrackBar.TabIndex = 139;
         this.LatitudeTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
         this.LatitudeTrackBar.Scroll += new System.EventHandler(this.LatitudeTrackBar_Scroll);
         // 
         // LongitudeTrackBar
         // 
         this.LongitudeTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.LongitudeTrackBar.AutoSize = false;
         this.LongitudeTrackBar.Location = new System.Drawing.Point(6, 110);
         this.LongitudeTrackBar.Maximum = 180000000;
         this.LongitudeTrackBar.Minimum = -180000000;
         this.LongitudeTrackBar.Name = "LongitudeTrackBar";
         this.LongitudeTrackBar.Size = new System.Drawing.Size(604, 20);
         this.LongitudeTrackBar.TabIndex = 140;
         this.LongitudeTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
         this.LongitudeTrackBar.Scroll += new System.EventHandler(this.LongitudeTrackBar_Scroll);
         // 
         // SetStatusButton
         // 
         this.SetStatusButton.Location = new System.Drawing.Point(416, 2);
         this.SetStatusButton.Name = "SetStatusButton";
         this.SetStatusButton.Size = new System.Drawing.Size(32, 23);
         this.SetStatusButton.TabIndex = 143;
         this.SetStatusButton.Text = "set";
         this.SetStatusButton.UseVisualStyleBackColor = true;
         this.SetStatusButton.Click += new System.EventHandler(this.SetStatusButton_Click);
         // 
         // StatusTextBox
         // 
         this.StatusTextBox.Location = new System.Drawing.Point(384, 3);
         this.StatusTextBox.MaxLength = 2;
         this.StatusTextBox.Name = "StatusTextBox";
         this.StatusTextBox.Size = new System.Drawing.Size(26, 20);
         this.StatusTextBox.TabIndex = 142;
         this.StatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(333, 31);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(49, 13);
         this.label3.TabIndex = 141;
         this.label3.Text = "Satellites";
         this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // UtcValidCheckBox
         // 
         this.UtcValidCheckBox.AutoSize = true;
         this.UtcValidCheckBox.Location = new System.Drawing.Point(477, 32);
         this.UtcValidCheckBox.Name = "UtcValidCheckBox";
         this.UtcValidCheckBox.Size = new System.Drawing.Size(74, 17);
         this.UtcValidCheckBox.TabIndex = 144;
         this.UtcValidCheckBox.Text = "UTC Valid";
         this.UtcValidCheckBox.UseVisualStyleBackColor = true;
         // 
         // SetSatelliteButton
         // 
         this.SetSatelliteButton.Location = new System.Drawing.Point(416, 26);
         this.SetSatelliteButton.Name = "SetSatelliteButton";
         this.SetSatelliteButton.Size = new System.Drawing.Size(32, 23);
         this.SetSatelliteButton.TabIndex = 147;
         this.SetSatelliteButton.Text = "set";
         this.SetSatelliteButton.UseVisualStyleBackColor = true;
         this.SetSatelliteButton.Click += new System.EventHandler(this.SetSatelliteButton_Click);
         // 
         // SatelliteTextBox
         // 
         this.SatelliteTextBox.Location = new System.Drawing.Point(384, 27);
         this.SatelliteTextBox.MaxLength = 2;
         this.SatelliteTextBox.Name = "SatelliteTextBox";
         this.SatelliteTextBox.Size = new System.Drawing.Size(26, 20);
         this.SatelliteTextBox.TabIndex = 146;
         this.SatelliteTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(345, 7);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(37, 13);
         this.label4.TabIndex = 145;
         this.label4.Text = "Status";
         this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // SetMethodButton
         // 
         this.SetMethodButton.Location = new System.Drawing.Point(531, 3);
         this.SetMethodButton.Name = "SetMethodButton";
         this.SetMethodButton.Size = new System.Drawing.Size(32, 23);
         this.SetMethodButton.TabIndex = 150;
         this.SetMethodButton.Text = "set";
         this.SetMethodButton.UseVisualStyleBackColor = true;
         this.SetMethodButton.Click += new System.EventHandler(this.SetMethodButton_Click);
         // 
         // MethodTextBox
         // 
         this.MethodTextBox.Location = new System.Drawing.Point(499, 4);
         this.MethodTextBox.MaxLength = 2;
         this.MethodTextBox.Name = "MethodTextBox";
         this.MethodTextBox.Size = new System.Drawing.Size(26, 20);
         this.MethodTextBox.TabIndex = 149;
         this.MethodTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(454, 7);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(43, 13);
         this.label5.TabIndex = 148;
         this.label5.Text = "Method";
         this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // UlcRoboticsGps
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.Controls.Add(this.MethodTextBox);
         this.Controls.Add(this.SatelliteTextBox);
         this.Controls.Add(this.StatusTextBox);
         this.Controls.Add(this.SetMethodButton);
         this.Controls.Add(this.label5);
         this.Controls.Add(this.SetSatelliteButton);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.UtcValidCheckBox);
         this.Controls.Add(this.SetStatusButton);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.LongitudeTrackBar);
         this.Controls.Add(this.LatitudeTrackBar);
         this.Controls.Add(this.SetLongitudeButton);
         this.Controls.Add(this.LongitudeReportTextBox);
         this.Controls.Add(this.LongitudeEntryTextBox);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.SetLatitudeButton);
         this.Controls.Add(this.LatitudeReportTextBox);
         this.Controls.Add(this.LatitudeEntryTextBox);
         this.Controls.Add(this.DescriptorLabel);
         this.Controls.Add(this.BusIdTextBox);
         this.Controls.Add(this.DeviceStateLabel);
         this.Controls.Add(this.EnabledCheckBox);
         this.Controls.Add(this.DescriptionTextBox);
         this.Controls.Add(this.NodeIdTextBox);
         this.Controls.Add(this.label1);
         this.Name = "UlcRoboticsGps";
         this.Size = new System.Drawing.Size(623, 139);
         ((System.ComponentModel.ISupportInitialize)(this.LatitudeTrackBar)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.LongitudeTrackBar)).EndInit();
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
      private System.Windows.Forms.Button SetLatitudeButton;
      private System.Windows.Forms.TextBox LatitudeReportTextBox;
      private System.Windows.Forms.TextBox LatitudeEntryTextBox;
      private System.Windows.Forms.Label DescriptorLabel;
      private System.Windows.Forms.Button SetLongitudeButton;
      private System.Windows.Forms.TextBox LongitudeReportTextBox;
      private System.Windows.Forms.TextBox LongitudeEntryTextBox;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TrackBar LatitudeTrackBar;
      private System.Windows.Forms.TrackBar LongitudeTrackBar;
      private System.Windows.Forms.Button SetStatusButton;
      private System.Windows.Forms.TextBox StatusTextBox;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.CheckBox UtcValidCheckBox;
      private System.Windows.Forms.Button SetSatelliteButton;
      private System.Windows.Forms.TextBox SatelliteTextBox;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Button SetMethodButton;
      private System.Windows.Forms.TextBox MethodTextBox;
      private System.Windows.Forms.Label label5;
   }
}
