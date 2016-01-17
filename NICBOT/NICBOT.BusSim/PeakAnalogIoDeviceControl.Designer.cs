namespace NICBOT.BusSim
{
   partial class PeakAnalogIoDeviceControl
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
         this.AOut3TimeoutLevelTextBox = new System.Windows.Forms.TextBox();
         this.AOut3PowerUpLevelTextBox = new System.Windows.Forms.TextBox();
         this.AOut3TextBox = new System.Windows.Forms.TextBox();
         this.label12 = new System.Windows.Forms.Label();
         this.label13 = new System.Windows.Forms.Label();
         this.label14 = new System.Windows.Forms.Label();
         this.AOut2TimeoutLevelTextBox = new System.Windows.Forms.TextBox();
         this.AOut2PowerUpLevelTextBox = new System.Windows.Forms.TextBox();
         this.AOut2TextBox = new System.Windows.Forms.TextBox();
         this.label9 = new System.Windows.Forms.Label();
         this.label10 = new System.Windows.Forms.Label();
         this.label11 = new System.Windows.Forms.Label();
         this.AOut1TimeoutLevelTextBox = new System.Windows.Forms.TextBox();
         this.AOut1PowerUpLevelTextBox = new System.Windows.Forms.TextBox();
         this.AOut1TextBox = new System.Windows.Forms.TextBox();
         this.label3 = new System.Windows.Forms.Label();
         this.label6 = new System.Windows.Forms.Label();
         this.label8 = new System.Windows.Forms.Label();
         this.AOut0TimeoutLevelTextBox = new System.Windows.Forms.TextBox();
         this.AOut0PowerUpLevelTextBox = new System.Windows.Forms.TextBox();
         this.AOut0TextBox = new System.Windows.Forms.TextBox();
         this.label5 = new System.Windows.Forms.Label();
         this.label4 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.TimeoutTextBox = new System.Windows.Forms.TextBox();
         this.RateTextBox = new System.Windows.Forms.TextBox();
         this.DeviceStateLabel = new System.Windows.Forms.Label();
         this.label17 = new System.Windows.Forms.Label();
         this.label7 = new System.Windows.Forms.Label();
         this.EnabledCheckBox = new System.Windows.Forms.CheckBox();
         this.DescriptionTextBox = new System.Windows.Forms.TextBox();
         this.NodeIdTextBox = new System.Windows.Forms.TextBox();
         this.label1 = new System.Windows.Forms.Label();
         this.AIn1AnalogInputControl = new NICBOT.BusSim.AnalogInputControl();
         this.AIn0AnalogInputControl = new NICBOT.BusSim.AnalogInputControl();
         this.AIn2AnalogInputControl = new NICBOT.BusSim.AnalogInputControl();
         this.AIn4AnalogInputControl = new NICBOT.BusSim.AnalogInputControl();
         this.AIn3AnalogInputControl = new NICBOT.BusSim.AnalogInputControl();
         this.AIn5AnalogInputControl = new NICBOT.BusSim.AnalogInputControl();
         this.AIn6AnalogInputControl = new NICBOT.BusSim.AnalogInputControl();
         this.AIn7AnalogInputControl = new NICBOT.BusSim.AnalogInputControl();
         this.BusIdTextBox = new System.Windows.Forms.TextBox();
         this.SuspendLayout();
         // 
         // AOut3TimeoutLevelTextBox
         // 
         this.AOut3TimeoutLevelTextBox.Location = new System.Drawing.Point(748, 54);
         this.AOut3TimeoutLevelTextBox.MaxLength = 4;
         this.AOut3TimeoutLevelTextBox.Name = "AOut3TimeoutLevelTextBox";
         this.AOut3TimeoutLevelTextBox.Size = new System.Drawing.Size(33, 20);
         this.AOut3TimeoutLevelTextBox.TabIndex = 103;
         this.AOut3TimeoutLevelTextBox.Text = "0";
         this.AOut3TimeoutLevelTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // AOut3PowerUpLevelTextBox
         // 
         this.AOut3PowerUpLevelTextBox.Location = new System.Drawing.Point(748, 30);
         this.AOut3PowerUpLevelTextBox.MaxLength = 4;
         this.AOut3PowerUpLevelTextBox.Name = "AOut3PowerUpLevelTextBox";
         this.AOut3PowerUpLevelTextBox.Size = new System.Drawing.Size(33, 20);
         this.AOut3PowerUpLevelTextBox.TabIndex = 102;
         this.AOut3PowerUpLevelTextBox.Text = "0";
         this.AOut3PowerUpLevelTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // AOut3TextBox
         // 
         this.AOut3TextBox.Enabled = false;
         this.AOut3TextBox.Location = new System.Drawing.Point(668, 30);
         this.AOut3TextBox.MaxLength = 3;
         this.AOut3TextBox.Name = "AOut3TextBox";
         this.AOut3TextBox.Size = new System.Drawing.Size(33, 20);
         this.AOut3TextBox.TabIndex = 101;
         this.AOut3TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.AOut3TextBox.TextChanged += new System.EventHandler(this.AOut3TextBox_TextChanged);
         // 
         // label12
         // 
         this.label12.AutoSize = true;
         this.label12.Location = new System.Drawing.Point(705, 57);
         this.label12.Name = "label12";
         this.label12.Size = new System.Drawing.Size(41, 13);
         this.label12.TabIndex = 100;
         this.label12.Text = "to-level";
         this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // label13
         // 
         this.label13.AutoSize = true;
         this.label13.Location = new System.Drawing.Point(702, 33);
         this.label13.Name = "label13";
         this.label13.Size = new System.Drawing.Size(44, 13);
         this.label13.TabIndex = 99;
         this.label13.Text = "pu-level";
         this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // label14
         // 
         this.label14.AutoSize = true;
         this.label14.Location = new System.Drawing.Point(627, 33);
         this.label14.Name = "label14";
         this.label14.Size = new System.Drawing.Size(37, 13);
         this.label14.TabIndex = 98;
         this.label14.Text = "AOut3";
         this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // AOut2TimeoutLevelTextBox
         // 
         this.AOut2TimeoutLevelTextBox.Location = new System.Drawing.Point(552, 54);
         this.AOut2TimeoutLevelTextBox.MaxLength = 4;
         this.AOut2TimeoutLevelTextBox.Name = "AOut2TimeoutLevelTextBox";
         this.AOut2TimeoutLevelTextBox.Size = new System.Drawing.Size(33, 20);
         this.AOut2TimeoutLevelTextBox.TabIndex = 97;
         this.AOut2TimeoutLevelTextBox.Text = "0";
         this.AOut2TimeoutLevelTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // AOut2PowerUpLevelTextBox
         // 
         this.AOut2PowerUpLevelTextBox.Location = new System.Drawing.Point(552, 30);
         this.AOut2PowerUpLevelTextBox.MaxLength = 4;
         this.AOut2PowerUpLevelTextBox.Name = "AOut2PowerUpLevelTextBox";
         this.AOut2PowerUpLevelTextBox.Size = new System.Drawing.Size(33, 20);
         this.AOut2PowerUpLevelTextBox.TabIndex = 96;
         this.AOut2PowerUpLevelTextBox.Text = "0";
         this.AOut2PowerUpLevelTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // AOut2TextBox
         // 
         this.AOut2TextBox.Enabled = false;
         this.AOut2TextBox.Location = new System.Drawing.Point(472, 30);
         this.AOut2TextBox.MaxLength = 3;
         this.AOut2TextBox.Name = "AOut2TextBox";
         this.AOut2TextBox.Size = new System.Drawing.Size(33, 20);
         this.AOut2TextBox.TabIndex = 95;
         this.AOut2TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.AOut2TextBox.TextChanged += new System.EventHandler(this.AOut2TextBox_TextChanged);
         // 
         // label9
         // 
         this.label9.AutoSize = true;
         this.label9.Location = new System.Drawing.Point(509, 57);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(41, 13);
         this.label9.TabIndex = 94;
         this.label9.Text = "to-level";
         this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // label10
         // 
         this.label10.AutoSize = true;
         this.label10.Location = new System.Drawing.Point(506, 33);
         this.label10.Name = "label10";
         this.label10.Size = new System.Drawing.Size(44, 13);
         this.label10.TabIndex = 93;
         this.label10.Text = "pu-level";
         this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // label11
         // 
         this.label11.AutoSize = true;
         this.label11.Location = new System.Drawing.Point(431, 33);
         this.label11.Name = "label11";
         this.label11.Size = new System.Drawing.Size(37, 13);
         this.label11.TabIndex = 92;
         this.label11.Text = "AOut2";
         this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // AOut1TimeoutLevelTextBox
         // 
         this.AOut1TimeoutLevelTextBox.Location = new System.Drawing.Point(356, 54);
         this.AOut1TimeoutLevelTextBox.MaxLength = 4;
         this.AOut1TimeoutLevelTextBox.Name = "AOut1TimeoutLevelTextBox";
         this.AOut1TimeoutLevelTextBox.Size = new System.Drawing.Size(33, 20);
         this.AOut1TimeoutLevelTextBox.TabIndex = 91;
         this.AOut1TimeoutLevelTextBox.Text = "0";
         this.AOut1TimeoutLevelTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // AOut1PowerUpLevelTextBox
         // 
         this.AOut1PowerUpLevelTextBox.Location = new System.Drawing.Point(356, 30);
         this.AOut1PowerUpLevelTextBox.MaxLength = 4;
         this.AOut1PowerUpLevelTextBox.Name = "AOut1PowerUpLevelTextBox";
         this.AOut1PowerUpLevelTextBox.Size = new System.Drawing.Size(33, 20);
         this.AOut1PowerUpLevelTextBox.TabIndex = 90;
         this.AOut1PowerUpLevelTextBox.Text = "0";
         this.AOut1PowerUpLevelTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // AOut1TextBox
         // 
         this.AOut1TextBox.Enabled = false;
         this.AOut1TextBox.Location = new System.Drawing.Point(276, 30);
         this.AOut1TextBox.MaxLength = 3;
         this.AOut1TextBox.Name = "AOut1TextBox";
         this.AOut1TextBox.Size = new System.Drawing.Size(33, 20);
         this.AOut1TextBox.TabIndex = 89;
         this.AOut1TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.AOut1TextBox.TextChanged += new System.EventHandler(this.AOut1TextBox_TextChanged);
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(313, 57);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(41, 13);
         this.label3.TabIndex = 88;
         this.label3.Text = "to-level";
         this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(310, 33);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(44, 13);
         this.label6.TabIndex = 87;
         this.label6.Text = "pu-level";
         this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.Location = new System.Drawing.Point(235, 33);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(37, 13);
         this.label8.TabIndex = 86;
         this.label8.Text = "AOut1";
         this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // AOut0TimeoutLevelTextBox
         // 
         this.AOut0TimeoutLevelTextBox.Location = new System.Drawing.Point(160, 54);
         this.AOut0TimeoutLevelTextBox.MaxLength = 4;
         this.AOut0TimeoutLevelTextBox.Name = "AOut0TimeoutLevelTextBox";
         this.AOut0TimeoutLevelTextBox.Size = new System.Drawing.Size(33, 20);
         this.AOut0TimeoutLevelTextBox.TabIndex = 85;
         this.AOut0TimeoutLevelTextBox.Text = "0";
         this.AOut0TimeoutLevelTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // AOut0PowerUpLevelTextBox
         // 
         this.AOut0PowerUpLevelTextBox.Location = new System.Drawing.Point(160, 30);
         this.AOut0PowerUpLevelTextBox.MaxLength = 4;
         this.AOut0PowerUpLevelTextBox.Name = "AOut0PowerUpLevelTextBox";
         this.AOut0PowerUpLevelTextBox.Size = new System.Drawing.Size(33, 20);
         this.AOut0PowerUpLevelTextBox.TabIndex = 84;
         this.AOut0PowerUpLevelTextBox.Text = "0";
         this.AOut0PowerUpLevelTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // AOut0TextBox
         // 
         this.AOut0TextBox.Enabled = false;
         this.AOut0TextBox.Location = new System.Drawing.Point(80, 30);
         this.AOut0TextBox.MaxLength = 3;
         this.AOut0TextBox.Name = "AOut0TextBox";
         this.AOut0TextBox.Size = new System.Drawing.Size(33, 20);
         this.AOut0TextBox.TabIndex = 83;
         this.AOut0TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.AOut0TextBox.TextChanged += new System.EventHandler(this.AOut0TextBox_TextChanged);
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(117, 57);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(41, 13);
         this.label5.TabIndex = 82;
         this.label5.Text = "to-level";
         this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(114, 33);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(44, 13);
         this.label4.TabIndex = 81;
         this.label4.Text = "pu-level";
         this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(39, 33);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(37, 13);
         this.label2.TabIndex = 80;
         this.label2.Text = "AOut0";
         this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // TimeoutTextBox
         // 
         this.TimeoutTextBox.Location = new System.Drawing.Point(380, 4);
         this.TimeoutTextBox.MaxLength = 5;
         this.TimeoutTextBox.Name = "TimeoutTextBox";
         this.TimeoutTextBox.Size = new System.Drawing.Size(44, 20);
         this.TimeoutTextBox.TabIndex = 77;
         this.TimeoutTextBox.Text = "1000";
         this.TimeoutTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // RateTextBox
         // 
         this.RateTextBox.Location = new System.Drawing.Point(455, 4);
         this.RateTextBox.MaxLength = 5;
         this.RateTextBox.Name = "RateTextBox";
         this.RateTextBox.Size = new System.Drawing.Size(44, 20);
         this.RateTextBox.TabIndex = 76;
         this.RateTextBox.Text = "100";
         this.RateTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // DeviceStateLabel
         // 
         this.DeviceStateLabel.BackColor = System.Drawing.SystemColors.Control;
         this.DeviceStateLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.DeviceStateLabel.Location = new System.Drawing.Point(264, 4);
         this.DeviceStateLabel.Name = "DeviceStateLabel";
         this.DeviceStateLabel.Size = new System.Drawing.Size(66, 20);
         this.DeviceStateLabel.TabIndex = 79;
         this.DeviceStateLabel.Text = "OFF";
         this.DeviceStateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label17
         // 
         this.label17.AutoSize = true;
         this.label17.Location = new System.Drawing.Point(336, 7);
         this.label17.Name = "label17";
         this.label17.Size = new System.Drawing.Size(41, 13);
         this.label17.TabIndex = 78;
         this.label17.Text = "timeout";
         this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(428, 7);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(25, 13);
         this.label7.TabIndex = 75;
         this.label7.Text = "rate";
         this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // EnabledCheckBox
         // 
         this.EnabledCheckBox.AutoSize = true;
         this.EnabledCheckBox.Location = new System.Drawing.Point(5, 7);
         this.EnabledCheckBox.Name = "EnabledCheckBox";
         this.EnabledCheckBox.Size = new System.Drawing.Size(15, 14);
         this.EnabledCheckBox.TabIndex = 74;
         this.EnabledCheckBox.UseVisualStyleBackColor = true;
         // 
         // DescriptionTextBox
         // 
         this.DescriptionTextBox.Location = new System.Drawing.Point(24, 4);
         this.DescriptionTextBox.MaxLength = 65535;
         this.DescriptionTextBox.Name = "DescriptionTextBox";
         this.DescriptionTextBox.Size = new System.Drawing.Size(150, 20);
         this.DescriptionTextBox.TabIndex = 73;
         this.DescriptionTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // NodeIdTextBox
         // 
         this.NodeIdTextBox.Location = new System.Drawing.Point(217, 4);
         this.NodeIdTextBox.MaxLength = 3;
         this.NodeIdTextBox.Name = "NodeIdTextBox";
         this.NodeIdTextBox.Size = new System.Drawing.Size(25, 20);
         this.NodeIdTextBox.TabIndex = 71;
         this.NodeIdTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(180, 7);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(36, 13);
         this.label1.TabIndex = 72;
         this.label1.Text = "Node:";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // AIn1AnalogInputControl
         // 
         this.AIn1AnalogInputControl.Descriptor = "AIn1";
         this.AIn1AnalogInputControl.Follows = true;
         this.AIn1AnalogInputControl.Location = new System.Drawing.Point(3, 146);
         this.AIn1AnalogInputControl.MaximumSize = new System.Drawing.Size(190, 60);
         this.AIn1AnalogInputControl.MinimumSize = new System.Drawing.Size(190, 60);
         this.AIn1AnalogInputControl.Name = "AIn1AnalogInputControl";
         this.AIn1AnalogInputControl.Size = new System.Drawing.Size(190, 60);
         this.AIn1AnalogInputControl.TabIndex = 111;
         this.AIn1AnalogInputControl.Value = 240;
         this.AIn1AnalogInputControl.ValueText = "240";
         // 
         // AIn0AnalogInputControl
         // 
         this.AIn0AnalogInputControl.Descriptor = "AIn0";
         this.AIn0AnalogInputControl.Follows = false;
         this.AIn0AnalogInputControl.Location = new System.Drawing.Point(3, 80);
         this.AIn0AnalogInputControl.MaximumSize = new System.Drawing.Size(190, 60);
         this.AIn0AnalogInputControl.MinimumSize = new System.Drawing.Size(190, 60);
         this.AIn0AnalogInputControl.Name = "AIn0AnalogInputControl";
         this.AIn0AnalogInputControl.Size = new System.Drawing.Size(190, 60);
         this.AIn0AnalogInputControl.TabIndex = 110;
         this.AIn0AnalogInputControl.Value = 0;
         this.AIn0AnalogInputControl.ValueText = "0";
         // 
         // AIn2AnalogInputControl
         // 
         this.AIn2AnalogInputControl.Descriptor = "AIn2";
         this.AIn2AnalogInputControl.Follows = false;
         this.AIn2AnalogInputControl.Location = new System.Drawing.Point(199, 80);
         this.AIn2AnalogInputControl.MaximumSize = new System.Drawing.Size(190, 60);
         this.AIn2AnalogInputControl.MinimumSize = new System.Drawing.Size(190, 60);
         this.AIn2AnalogInputControl.Name = "AIn2AnalogInputControl";
         this.AIn2AnalogInputControl.Size = new System.Drawing.Size(190, 60);
         this.AIn2AnalogInputControl.TabIndex = 112;
         this.AIn2AnalogInputControl.Value = 0;
         this.AIn2AnalogInputControl.ValueText = "0";
         // 
         // AIn4AnalogInputControl
         // 
         this.AIn4AnalogInputControl.Descriptor = "AIn4";
         this.AIn4AnalogInputControl.Follows = false;
         this.AIn4AnalogInputControl.Location = new System.Drawing.Point(395, 80);
         this.AIn4AnalogInputControl.MaximumSize = new System.Drawing.Size(190, 60);
         this.AIn4AnalogInputControl.MinimumSize = new System.Drawing.Size(190, 60);
         this.AIn4AnalogInputControl.Name = "AIn4AnalogInputControl";
         this.AIn4AnalogInputControl.Size = new System.Drawing.Size(190, 60);
         this.AIn4AnalogInputControl.TabIndex = 113;
         this.AIn4AnalogInputControl.Value = 0;
         this.AIn4AnalogInputControl.ValueText = "0";
         // 
         // AIn3AnalogInputControl
         // 
         this.AIn3AnalogInputControl.Descriptor = "AIn3";
         this.AIn3AnalogInputControl.Follows = false;
         this.AIn3AnalogInputControl.Location = new System.Drawing.Point(199, 146);
         this.AIn3AnalogInputControl.MaximumSize = new System.Drawing.Size(190, 60);
         this.AIn3AnalogInputControl.MinimumSize = new System.Drawing.Size(190, 60);
         this.AIn3AnalogInputControl.Name = "AIn3AnalogInputControl";
         this.AIn3AnalogInputControl.Size = new System.Drawing.Size(190, 60);
         this.AIn3AnalogInputControl.TabIndex = 114;
         this.AIn3AnalogInputControl.Value = 0;
         this.AIn3AnalogInputControl.ValueText = "0";
         // 
         // AIn5AnalogInputControl
         // 
         this.AIn5AnalogInputControl.Descriptor = "AIn5";
         this.AIn5AnalogInputControl.Follows = false;
         this.AIn5AnalogInputControl.Location = new System.Drawing.Point(395, 146);
         this.AIn5AnalogInputControl.MaximumSize = new System.Drawing.Size(190, 60);
         this.AIn5AnalogInputControl.MinimumSize = new System.Drawing.Size(190, 60);
         this.AIn5AnalogInputControl.Name = "AIn5AnalogInputControl";
         this.AIn5AnalogInputControl.Size = new System.Drawing.Size(190, 60);
         this.AIn5AnalogInputControl.TabIndex = 115;
         this.AIn5AnalogInputControl.Value = 0;
         this.AIn5AnalogInputControl.ValueText = "0";
         // 
         // AIn6AnalogInputControl
         // 
         this.AIn6AnalogInputControl.Descriptor = "AIn6";
         this.AIn6AnalogInputControl.Follows = false;
         this.AIn6AnalogInputControl.Location = new System.Drawing.Point(591, 80);
         this.AIn6AnalogInputControl.MaximumSize = new System.Drawing.Size(190, 60);
         this.AIn6AnalogInputControl.MinimumSize = new System.Drawing.Size(190, 60);
         this.AIn6AnalogInputControl.Name = "AIn6AnalogInputControl";
         this.AIn6AnalogInputControl.Size = new System.Drawing.Size(190, 60);
         this.AIn6AnalogInputControl.TabIndex = 116;
         this.AIn6AnalogInputControl.Value = 0;
         this.AIn6AnalogInputControl.ValueText = "0";
         // 
         // AIn7AnalogInputControl
         // 
         this.AIn7AnalogInputControl.Descriptor = "AIn7";
         this.AIn7AnalogInputControl.Follows = false;
         this.AIn7AnalogInputControl.Location = new System.Drawing.Point(591, 146);
         this.AIn7AnalogInputControl.MaximumSize = new System.Drawing.Size(190, 60);
         this.AIn7AnalogInputControl.MinimumSize = new System.Drawing.Size(190, 60);
         this.AIn7AnalogInputControl.Name = "AIn7AnalogInputControl";
         this.AIn7AnalogInputControl.Size = new System.Drawing.Size(190, 60);
         this.AIn7AnalogInputControl.TabIndex = 117;
         this.AIn7AnalogInputControl.Value = 0;
         this.AIn7AnalogInputControl.ValueText = "0";
         // 
         // BusIdTextBox
         // 
         this.BusIdTextBox.Location = new System.Drawing.Point(245, 4);
         this.BusIdTextBox.MaxLength = 3;
         this.BusIdTextBox.Name = "BusIdTextBox";
         this.BusIdTextBox.ReadOnly = true;
         this.BusIdTextBox.Size = new System.Drawing.Size(15, 20);
         this.BusIdTextBox.TabIndex = 118;
         this.BusIdTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // PeakAnalogIoDeviceControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.Controls.Add(this.BusIdTextBox);
         this.Controls.Add(this.AIn7AnalogInputControl);
         this.Controls.Add(this.AIn6AnalogInputControl);
         this.Controls.Add(this.AIn5AnalogInputControl);
         this.Controls.Add(this.AIn3AnalogInputControl);
         this.Controls.Add(this.AIn4AnalogInputControl);
         this.Controls.Add(this.AIn2AnalogInputControl);
         this.Controls.Add(this.AIn1AnalogInputControl);
         this.Controls.Add(this.AIn0AnalogInputControl);
         this.Controls.Add(this.AOut3TimeoutLevelTextBox);
         this.Controls.Add(this.AOut3PowerUpLevelTextBox);
         this.Controls.Add(this.AOut3TextBox);
         this.Controls.Add(this.label12);
         this.Controls.Add(this.label13);
         this.Controls.Add(this.label14);
         this.Controls.Add(this.AOut2TimeoutLevelTextBox);
         this.Controls.Add(this.AOut2PowerUpLevelTextBox);
         this.Controls.Add(this.AOut2TextBox);
         this.Controls.Add(this.label9);
         this.Controls.Add(this.label10);
         this.Controls.Add(this.label11);
         this.Controls.Add(this.AOut1TimeoutLevelTextBox);
         this.Controls.Add(this.AOut1PowerUpLevelTextBox);
         this.Controls.Add(this.AOut1TextBox);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.label6);
         this.Controls.Add(this.label8);
         this.Controls.Add(this.AOut0TimeoutLevelTextBox);
         this.Controls.Add(this.AOut0PowerUpLevelTextBox);
         this.Controls.Add(this.AOut0TextBox);
         this.Controls.Add(this.label5);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.TimeoutTextBox);
         this.Controls.Add(this.RateTextBox);
         this.Controls.Add(this.DeviceStateLabel);
         this.Controls.Add(this.label17);
         this.Controls.Add(this.label7);
         this.Controls.Add(this.EnabledCheckBox);
         this.Controls.Add(this.DescriptionTextBox);
         this.Controls.Add(this.NodeIdTextBox);
         this.Controls.Add(this.label1);
         this.Name = "PeakAnalogIoDeviceControl";
         this.Size = new System.Drawing.Size(784, 208);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.TextBox TimeoutTextBox;
      private System.Windows.Forms.TextBox RateTextBox;
      private System.Windows.Forms.Label DeviceStateLabel;
      private System.Windows.Forms.Label label17;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.CheckBox EnabledCheckBox;
      private System.Windows.Forms.TextBox DescriptionTextBox;
      private System.Windows.Forms.TextBox NodeIdTextBox;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox AOut0TimeoutLevelTextBox;
      private System.Windows.Forms.TextBox AOut0PowerUpLevelTextBox;
      private System.Windows.Forms.TextBox AOut0TextBox;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TextBox AOut1TimeoutLevelTextBox;
      private System.Windows.Forms.TextBox AOut1PowerUpLevelTextBox;
      private System.Windows.Forms.TextBox AOut1TextBox;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.TextBox AOut2TimeoutLevelTextBox;
      private System.Windows.Forms.TextBox AOut2PowerUpLevelTextBox;
      private System.Windows.Forms.TextBox AOut2TextBox;
      private System.Windows.Forms.Label label9;
      private System.Windows.Forms.Label label10;
      private System.Windows.Forms.Label label11;
      private System.Windows.Forms.TextBox AOut3TimeoutLevelTextBox;
      private System.Windows.Forms.TextBox AOut3PowerUpLevelTextBox;
      private System.Windows.Forms.TextBox AOut3TextBox;
      private System.Windows.Forms.Label label12;
      private System.Windows.Forms.Label label13;
      private System.Windows.Forms.Label label14;
      private AnalogInputControl AIn0AnalogInputControl;
      private AnalogInputControl AIn1AnalogInputControl;
      private AnalogInputControl AIn2AnalogInputControl;
      private AnalogInputControl AIn4AnalogInputControl;
      private AnalogInputControl AIn3AnalogInputControl;
      private AnalogInputControl AIn5AnalogInputControl;
      private AnalogInputControl AIn6AnalogInputControl;
      private AnalogInputControl AIn7AnalogInputControl;
      private System.Windows.Forms.TextBox BusIdTextBox;
   }
}
