namespace NICBOT.BusSim
{
   partial class ElmoMotorDeviceControl
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
         this.NodeIdTextBox = new System.Windows.Forms.TextBox();
         this.label1 = new System.Windows.Forms.Label();
         this.DescriptionTextBox = new System.Windows.Forms.TextBox();
         this.DeviceStateLabel = new System.Windows.Forms.Label();
         this.EnabledCheckBox = new System.Windows.Forms.CheckBox();
         this.BusIdTextBox = new System.Windows.Forms.TextBox();
         this.DIn5LevelCheckBox = new System.Windows.Forms.CheckBox();
         this.DIn4LevelCheckBox = new System.Windows.Forms.CheckBox();
         this.DIn3LevelCheckBox = new System.Windows.Forms.CheckBox();
         this.label10 = new System.Windows.Forms.Label();
         this.DIn2LevelCheckBox = new System.Windows.Forms.CheckBox();
         this.label9 = new System.Windows.Forms.Label();
         this.DIn1LevelCheckBox = new System.Windows.Forms.CheckBox();
         this.label8 = new System.Windows.Forms.Label();
         this.DIn0LevelCheckBox = new System.Windows.Forms.CheckBox();
         this.label6 = new System.Windows.Forms.Label();
         this.label12 = new System.Windows.Forms.Label();
         this.label11 = new System.Windows.Forms.Label();
         this.ConsumerHeartbeatTimeTextBox = new System.Windows.Forms.TextBox();
         this.ProducerHeartbeatTimeTextBox = new System.Windows.Forms.TextBox();
         this.label14 = new System.Windows.Forms.Label();
         this.label13 = new System.Windows.Forms.Label();
         this.DisplayModeTextBox = new System.Windows.Forms.TextBox();
         this.ModeTextBox = new System.Windows.Forms.TextBox();
         this.label5 = new System.Windows.Forms.Label();
         this.StatusTextBox = new System.Windows.Forms.TextBox();
         this.label4 = new System.Windows.Forms.Label();
         this.ControlWordTextBox = new System.Windows.Forms.TextBox();
         this.label3 = new System.Windows.Forms.Label();
         this.ErrorStatusTextBox = new System.Windows.Forms.TextBox();
         this.label16 = new System.Windows.Forms.Label();
         this.ErrorCodeTextBox = new System.Windows.Forms.TextBox();
         this.label15 = new System.Windows.Forms.Label();
         this.MaximumTorqueTextBox = new System.Windows.Forms.TextBox();
         this.label19 = new System.Windows.Forms.Label();
         this.MaximumCurrentTextBox = new System.Windows.Forms.TextBox();
         this.label20 = new System.Windows.Forms.Label();
         this.TorqueSlopeTextBox = new System.Windows.Forms.TextBox();
         this.label18 = new System.Windows.Forms.Label();
         this.MotorRatedTorqueTextBox = new System.Windows.Forms.TextBox();
         this.label17 = new System.Windows.Forms.Label();
         this.MotorRatedCurrentTextBox = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.TorqueActualTextBox = new System.Windows.Forms.TextBox();
         this.label7 = new System.Windows.Forms.Label();
         this.TorqueTargetTextBox = new System.Windows.Forms.TextBox();
         this.label21 = new System.Windows.Forms.Label();
         this.VelocityDecelerationTextBox = new System.Windows.Forms.TextBox();
         this.label22 = new System.Windows.Forms.Label();
         this.VelocityAccelerationTextBox = new System.Windows.Forms.TextBox();
         this.label23 = new System.Windows.Forms.Label();
         this.VelocityActualTextBox = new System.Windows.Forms.TextBox();
         this.label24 = new System.Windows.Forms.Label();
         this.VelocityTargetTextBox = new System.Windows.Forms.TextBox();
         this.label25 = new System.Windows.Forms.Label();
         this.ConsumerHeartbeatModeTextBox = new System.Windows.Forms.TextBox();
         this.UserModeLabel = new System.Windows.Forms.Label();
         this.label27 = new System.Windows.Forms.Label();
         this.MotorStateLabel = new System.Windows.Forms.Label();
         this.label28 = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // NodeIdTextBox
         // 
         this.NodeIdTextBox.Location = new System.Drawing.Point(217, 4);
         this.NodeIdTextBox.MaxLength = 3;
         this.NodeIdTextBox.Name = "NodeIdTextBox";
         this.NodeIdTextBox.Size = new System.Drawing.Size(25, 20);
         this.NodeIdTextBox.TabIndex = 0;
         this.NodeIdTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(180, 7);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(36, 13);
         this.label1.TabIndex = 1;
         this.label1.Text = "Node:";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // DescriptionTextBox
         // 
         this.DescriptionTextBox.Location = new System.Drawing.Point(24, 4);
         this.DescriptionTextBox.MaxLength = 65535;
         this.DescriptionTextBox.Name = "DescriptionTextBox";
         this.DescriptionTextBox.Size = new System.Drawing.Size(150, 20);
         this.DescriptionTextBox.TabIndex = 7;
         this.DescriptionTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // DeviceStateLabel
         // 
         this.DeviceStateLabel.BackColor = System.Drawing.SystemColors.Control;
         this.DeviceStateLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.DeviceStateLabel.Location = new System.Drawing.Point(264, 4);
         this.DeviceStateLabel.Name = "DeviceStateLabel";
         this.DeviceStateLabel.Size = new System.Drawing.Size(66, 20);
         this.DeviceStateLabel.TabIndex = 8;
         this.DeviceStateLabel.Text = "OFF";
         this.DeviceStateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // EnabledCheckBox
         // 
         this.EnabledCheckBox.AutoSize = true;
         this.EnabledCheckBox.Location = new System.Drawing.Point(5, 7);
         this.EnabledCheckBox.Name = "EnabledCheckBox";
         this.EnabledCheckBox.Size = new System.Drawing.Size(15, 14);
         this.EnabledCheckBox.TabIndex = 11;
         this.EnabledCheckBox.UseVisualStyleBackColor = true;
         // 
         // BusIdTextBox
         // 
         this.BusIdTextBox.Location = new System.Drawing.Point(245, 4);
         this.BusIdTextBox.MaxLength = 3;
         this.BusIdTextBox.Name = "BusIdTextBox";
         this.BusIdTextBox.ReadOnly = true;
         this.BusIdTextBox.Size = new System.Drawing.Size(15, 20);
         this.BusIdTextBox.TabIndex = 14;
         this.BusIdTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // DIn5LevelCheckBox
         // 
         this.DIn5LevelCheckBox.AutoSize = true;
         this.DIn5LevelCheckBox.Location = new System.Drawing.Point(832, 148);
         this.DIn5LevelCheckBox.Name = "DIn5LevelCheckBox";
         this.DIn5LevelCheckBox.Size = new System.Drawing.Size(15, 14);
         this.DIn5LevelCheckBox.TabIndex = 52;
         this.DIn5LevelCheckBox.UseVisualStyleBackColor = true;
         this.DIn5LevelCheckBox.CheckedChanged += new System.EventHandler(this.DIn5LevelCheckBox_CheckedChanged);
         // 
         // DIn4LevelCheckBox
         // 
         this.DIn4LevelCheckBox.AutoSize = true;
         this.DIn4LevelCheckBox.Location = new System.Drawing.Point(782, 148);
         this.DIn4LevelCheckBox.Name = "DIn4LevelCheckBox";
         this.DIn4LevelCheckBox.Size = new System.Drawing.Size(15, 14);
         this.DIn4LevelCheckBox.TabIndex = 51;
         this.DIn4LevelCheckBox.UseVisualStyleBackColor = true;
         this.DIn4LevelCheckBox.CheckedChanged += new System.EventHandler(this.DIn4LevelCheckBox_CheckedChanged);
         // 
         // DIn3LevelCheckBox
         // 
         this.DIn3LevelCheckBox.AutoSize = true;
         this.DIn3LevelCheckBox.Location = new System.Drawing.Point(732, 148);
         this.DIn3LevelCheckBox.Name = "DIn3LevelCheckBox";
         this.DIn3LevelCheckBox.Size = new System.Drawing.Size(15, 14);
         this.DIn3LevelCheckBox.TabIndex = 50;
         this.DIn3LevelCheckBox.UseVisualStyleBackColor = true;
         this.DIn3LevelCheckBox.CheckedChanged += new System.EventHandler(this.DIn3LevelCheckBox_CheckedChanged);
         // 
         // label10
         // 
         this.label10.AutoSize = true;
         this.label10.Location = new System.Drawing.Point(701, 148);
         this.label10.Name = "label10";
         this.label10.Size = new System.Drawing.Size(30, 13);
         this.label10.TabIndex = 49;
         this.label10.Text = "DIn3";
         this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // DIn2LevelCheckBox
         // 
         this.DIn2LevelCheckBox.AutoSize = true;
         this.DIn2LevelCheckBox.Location = new System.Drawing.Point(682, 148);
         this.DIn2LevelCheckBox.Name = "DIn2LevelCheckBox";
         this.DIn2LevelCheckBox.Size = new System.Drawing.Size(15, 14);
         this.DIn2LevelCheckBox.TabIndex = 48;
         this.DIn2LevelCheckBox.UseVisualStyleBackColor = true;
         this.DIn2LevelCheckBox.CheckedChanged += new System.EventHandler(this.DIn2LevelCheckBox_CheckedChanged);
         // 
         // label9
         // 
         this.label9.AutoSize = true;
         this.label9.Location = new System.Drawing.Point(651, 148);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(30, 13);
         this.label9.TabIndex = 47;
         this.label9.Text = "DIn2";
         this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // DIn1LevelCheckBox
         // 
         this.DIn1LevelCheckBox.AutoSize = true;
         this.DIn1LevelCheckBox.Location = new System.Drawing.Point(632, 148);
         this.DIn1LevelCheckBox.Name = "DIn1LevelCheckBox";
         this.DIn1LevelCheckBox.Size = new System.Drawing.Size(15, 14);
         this.DIn1LevelCheckBox.TabIndex = 46;
         this.DIn1LevelCheckBox.UseVisualStyleBackColor = true;
         this.DIn1LevelCheckBox.CheckedChanged += new System.EventHandler(this.DIn1LevelCheckBox_CheckedChanged);
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.Location = new System.Drawing.Point(601, 148);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(30, 13);
         this.label8.TabIndex = 45;
         this.label8.Text = "DIn1";
         this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // DIn0LevelCheckBox
         // 
         this.DIn0LevelCheckBox.AutoSize = true;
         this.DIn0LevelCheckBox.Location = new System.Drawing.Point(582, 148);
         this.DIn0LevelCheckBox.Name = "DIn0LevelCheckBox";
         this.DIn0LevelCheckBox.Size = new System.Drawing.Size(15, 14);
         this.DIn0LevelCheckBox.TabIndex = 44;
         this.DIn0LevelCheckBox.UseVisualStyleBackColor = true;
         this.DIn0LevelCheckBox.CheckedChanged += new System.EventHandler(this.DIn0LevelCheckBox_CheckedChanged);
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(551, 148);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(30, 13);
         this.label6.TabIndex = 43;
         this.label6.Text = "DIn0";
         this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // label12
         // 
         this.label12.AutoSize = true;
         this.label12.Location = new System.Drawing.Point(801, 148);
         this.label12.Name = "label12";
         this.label12.Size = new System.Drawing.Size(30, 13);
         this.label12.TabIndex = 54;
         this.label12.Text = "DIn5";
         this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // label11
         // 
         this.label11.AutoSize = true;
         this.label11.Location = new System.Drawing.Point(751, 148);
         this.label11.Name = "label11";
         this.label11.Size = new System.Drawing.Size(30, 13);
         this.label11.TabIndex = 53;
         this.label11.Text = "DIn4";
         this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // ConsumerHeartbeatTimeTextBox
         // 
         this.ConsumerHeartbeatTimeTextBox.Location = new System.Drawing.Point(682, 4);
         this.ConsumerHeartbeatTimeTextBox.MaxLength = 3;
         this.ConsumerHeartbeatTimeTextBox.Name = "ConsumerHeartbeatTimeTextBox";
         this.ConsumerHeartbeatTimeTextBox.ReadOnly = true;
         this.ConsumerHeartbeatTimeTextBox.Size = new System.Drawing.Size(83, 20);
         this.ConsumerHeartbeatTimeTextBox.TabIndex = 180;
         this.ConsumerHeartbeatTimeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // ProducerHeartbeatTimeTextBox
         // 
         this.ProducerHeartbeatTimeTextBox.Location = new System.Drawing.Point(682, 27);
         this.ProducerHeartbeatTimeTextBox.MaxLength = 3;
         this.ProducerHeartbeatTimeTextBox.Name = "ProducerHeartbeatTimeTextBox";
         this.ProducerHeartbeatTimeTextBox.ReadOnly = true;
         this.ProducerHeartbeatTimeTextBox.Size = new System.Drawing.Size(83, 20);
         this.ProducerHeartbeatTimeTextBox.TabIndex = 179;
         this.ProducerHeartbeatTimeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label14
         // 
         this.label14.AutoSize = true;
         this.label14.Location = new System.Drawing.Point(542, 7);
         this.label14.Name = "label14";
         this.label14.Size = new System.Drawing.Size(137, 13);
         this.label14.TabIndex = 178;
         this.label14.Text = "CONSUMER HEARTBEAT";
         this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // label13
         // 
         this.label13.AutoSize = true;
         this.label13.Location = new System.Drawing.Point(544, 30);
         this.label13.Name = "label13";
         this.label13.Size = new System.Drawing.Size(136, 13);
         this.label13.TabIndex = 177;
         this.label13.Text = "PRODUCER HEARTBEAT";
         this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // DisplayModeTextBox
         // 
         this.DisplayModeTextBox.Location = new System.Drawing.Point(382, 122);
         this.DisplayModeTextBox.MaxLength = 3;
         this.DisplayModeTextBox.Name = "DisplayModeTextBox";
         this.DisplayModeTextBox.ReadOnly = true;
         this.DisplayModeTextBox.Size = new System.Drawing.Size(25, 20);
         this.DisplayModeTextBox.TabIndex = 176;
         this.DisplayModeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // ModeTextBox
         // 
         this.ModeTextBox.Location = new System.Drawing.Point(351, 122);
         this.ModeTextBox.MaxLength = 3;
         this.ModeTextBox.Name = "ModeTextBox";
         this.ModeTextBox.ReadOnly = true;
         this.ModeTextBox.Size = new System.Drawing.Size(25, 20);
         this.ModeTextBox.TabIndex = 174;
         this.ModeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(311, 125);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(39, 13);
         this.label5.TabIndex = 175;
         this.label5.Text = "MODE";
         this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // StatusTextBox
         // 
         this.StatusTextBox.Location = new System.Drawing.Point(268, 122);
         this.StatusTextBox.MaxLength = 3;
         this.StatusTextBox.Name = "StatusTextBox";
         this.StatusTextBox.ReadOnly = true;
         this.StatusTextBox.Size = new System.Drawing.Size(37, 20);
         this.StatusTextBox.TabIndex = 172;
         this.StatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(216, 125);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(50, 13);
         this.label4.TabIndex = 173;
         this.label4.Text = "STATUS";
         this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // ControlWordTextBox
         // 
         this.ControlWordTextBox.Location = new System.Drawing.Point(173, 122);
         this.ControlWordTextBox.MaxLength = 3;
         this.ControlWordTextBox.Name = "ControlWordTextBox";
         this.ControlWordTextBox.ReadOnly = true;
         this.ControlWordTextBox.Size = new System.Drawing.Size(37, 20);
         this.ControlWordTextBox.TabIndex = 170;
         this.ControlWordTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(112, 125);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(59, 13);
         this.label3.TabIndex = 171;
         this.label3.Text = "CONTROL";
         this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // ErrorStatusTextBox
         // 
         this.ErrorStatusTextBox.Location = new System.Drawing.Point(453, 4);
         this.ErrorStatusTextBox.MaxLength = 3;
         this.ErrorStatusTextBox.Name = "ErrorStatusTextBox";
         this.ErrorStatusTextBox.ReadOnly = true;
         this.ErrorStatusTextBox.Size = new System.Drawing.Size(83, 20);
         this.ErrorStatusTextBox.TabIndex = 184;
         this.ErrorStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label16
         // 
         this.label16.AutoSize = true;
         this.label16.Location = new System.Drawing.Point(405, 7);
         this.label16.Name = "label16";
         this.label16.Size = new System.Drawing.Size(46, 13);
         this.label16.TabIndex = 183;
         this.label16.Text = "ERROR";
         this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // ErrorCodeTextBox
         // 
         this.ErrorCodeTextBox.Location = new System.Drawing.Point(264, 27);
         this.ErrorCodeTextBox.MaxLength = 3;
         this.ErrorCodeTextBox.Name = "ErrorCodeTextBox";
         this.ErrorCodeTextBox.ReadOnly = true;
         this.ErrorCodeTextBox.Size = new System.Drawing.Size(127, 20);
         this.ErrorCodeTextBox.TabIndex = 182;
         this.ErrorCodeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label15
         // 
         this.label15.AutoSize = true;
         this.label15.Location = new System.Drawing.Point(182, 30);
         this.label15.Name = "label15";
         this.label15.Size = new System.Drawing.Size(79, 13);
         this.label15.TabIndex = 181;
         this.label15.Text = "ERROR CODE";
         this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // MaximumTorqueTextBox
         // 
         this.MaximumTorqueTextBox.Location = new System.Drawing.Point(748, 99);
         this.MaximumTorqueTextBox.MaxLength = 3;
         this.MaximumTorqueTextBox.Name = "MaximumTorqueTextBox";
         this.MaximumTorqueTextBox.ReadOnly = true;
         this.MaximumTorqueTextBox.Size = new System.Drawing.Size(83, 20);
         this.MaximumTorqueTextBox.TabIndex = 197;
         this.MaximumTorqueTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label19
         // 
         this.label19.AutoSize = true;
         this.label19.Location = new System.Drawing.Point(667, 102);
         this.label19.Name = "label19";
         this.label19.Size = new System.Drawing.Size(79, 13);
         this.label19.TabIndex = 198;
         this.label19.Text = "MAX TORQUE";
         this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // MaximumCurrentTextBox
         // 
         this.MaximumCurrentTextBox.Location = new System.Drawing.Point(748, 122);
         this.MaximumCurrentTextBox.MaxLength = 3;
         this.MaximumCurrentTextBox.Name = "MaximumCurrentTextBox";
         this.MaximumCurrentTextBox.ReadOnly = true;
         this.MaximumCurrentTextBox.Size = new System.Drawing.Size(83, 20);
         this.MaximumCurrentTextBox.TabIndex = 195;
         this.MaximumCurrentTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label20
         // 
         this.label20.AutoSize = true;
         this.label20.Location = new System.Drawing.Point(660, 125);
         this.label20.Name = "label20";
         this.label20.Size = new System.Drawing.Size(86, 13);
         this.label20.TabIndex = 196;
         this.label20.Text = "MAX CURRENT";
         this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // TorqueSlopeTextBox
         // 
         this.TorqueSlopeTextBox.Location = new System.Drawing.Point(559, 76);
         this.TorqueSlopeTextBox.MaxLength = 3;
         this.TorqueSlopeTextBox.Name = "TorqueSlopeTextBox";
         this.TorqueSlopeTextBox.ReadOnly = true;
         this.TorqueSlopeTextBox.Size = new System.Drawing.Size(83, 20);
         this.TorqueSlopeTextBox.TabIndex = 194;
         this.TorqueSlopeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label18
         // 
         this.label18.AutoSize = true;
         this.label18.Location = new System.Drawing.Point(466, 79);
         this.label18.Name = "label18";
         this.label18.Size = new System.Drawing.Size(91, 13);
         this.label18.TabIndex = 193;
         this.label18.Text = "TORQUE SLOPE";
         this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // MotorRatedTorqueTextBox
         // 
         this.MotorRatedTorqueTextBox.Location = new System.Drawing.Point(559, 99);
         this.MotorRatedTorqueTextBox.MaxLength = 3;
         this.MotorRatedTorqueTextBox.Name = "MotorRatedTorqueTextBox";
         this.MotorRatedTorqueTextBox.ReadOnly = true;
         this.MotorRatedTorqueTextBox.Size = new System.Drawing.Size(83, 20);
         this.MotorRatedTorqueTextBox.TabIndex = 191;
         this.MotorRatedTorqueTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label17
         // 
         this.label17.AutoSize = true;
         this.label17.Location = new System.Drawing.Point(464, 102);
         this.label17.Name = "label17";
         this.label17.Size = new System.Drawing.Size(93, 13);
         this.label17.TabIndex = 192;
         this.label17.Text = "RATED TORQUE";
         this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // MotorRatedCurrentTextBox
         // 
         this.MotorRatedCurrentTextBox.Location = new System.Drawing.Point(559, 122);
         this.MotorRatedCurrentTextBox.MaxLength = 3;
         this.MotorRatedCurrentTextBox.Name = "MotorRatedCurrentTextBox";
         this.MotorRatedCurrentTextBox.ReadOnly = true;
         this.MotorRatedCurrentTextBox.Size = new System.Drawing.Size(83, 20);
         this.MotorRatedCurrentTextBox.TabIndex = 189;
         this.MotorRatedCurrentTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(457, 125);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(100, 13);
         this.label2.TabIndex = 190;
         this.label2.Text = "RATED CURRENT";
         this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // TorqueActualTextBox
         // 
         this.TorqueActualTextBox.Location = new System.Drawing.Point(748, 53);
         this.TorqueActualTextBox.MaxLength = 3;
         this.TorqueActualTextBox.Name = "TorqueActualTextBox";
         this.TorqueActualTextBox.ReadOnly = true;
         this.TorqueActualTextBox.Size = new System.Drawing.Size(83, 20);
         this.TorqueActualTextBox.TabIndex = 188;
         this.TorqueActualTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(648, 56);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(98, 13);
         this.label7.TabIndex = 187;
         this.label7.Text = "TORQUE ACTUAL";
         this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // TorqueTargetTextBox
         // 
         this.TorqueTargetTextBox.Location = new System.Drawing.Point(559, 53);
         this.TorqueTargetTextBox.MaxLength = 3;
         this.TorqueTargetTextBox.Name = "TorqueTargetTextBox";
         this.TorqueTargetTextBox.ReadOnly = true;
         this.TorqueTargetTextBox.Size = new System.Drawing.Size(83, 20);
         this.TorqueTargetTextBox.TabIndex = 185;
         this.TorqueTargetTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label21
         // 
         this.label21.AutoSize = true;
         this.label21.Location = new System.Drawing.Point(457, 56);
         this.label21.Name = "label21";
         this.label21.Size = new System.Drawing.Size(100, 13);
         this.label21.TabIndex = 186;
         this.label21.Text = "TORQUE TARGET";
         this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // VelocityDecelerationTextBox
         // 
         this.VelocityDecelerationTextBox.Location = new System.Drawing.Point(173, 99);
         this.VelocityDecelerationTextBox.MaxLength = 3;
         this.VelocityDecelerationTextBox.Name = "VelocityDecelerationTextBox";
         this.VelocityDecelerationTextBox.ReadOnly = true;
         this.VelocityDecelerationTextBox.Size = new System.Drawing.Size(83, 20);
         this.VelocityDecelerationTextBox.TabIndex = 206;
         this.VelocityDecelerationTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label22
         // 
         this.label22.AutoSize = true;
         this.label22.Location = new System.Drawing.Point(26, 102);
         this.label22.Name = "label22";
         this.label22.Size = new System.Drawing.Size(145, 13);
         this.label22.TabIndex = 205;
         this.label22.Text = "VELOCITY DECELERATION";
         this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // VelocityAccelerationTextBox
         // 
         this.VelocityAccelerationTextBox.Location = new System.Drawing.Point(173, 76);
         this.VelocityAccelerationTextBox.MaxLength = 3;
         this.VelocityAccelerationTextBox.Name = "VelocityAccelerationTextBox";
         this.VelocityAccelerationTextBox.ReadOnly = true;
         this.VelocityAccelerationTextBox.Size = new System.Drawing.Size(83, 20);
         this.VelocityAccelerationTextBox.TabIndex = 203;
         this.VelocityAccelerationTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label23
         // 
         this.label23.AutoSize = true;
         this.label23.Location = new System.Drawing.Point(27, 79);
         this.label23.Name = "label23";
         this.label23.Size = new System.Drawing.Size(144, 13);
         this.label23.TabIndex = 204;
         this.label23.Text = "VELOCITY ACCELERATION";
         this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // VelocityActualTextBox
         // 
         this.VelocityActualTextBox.Location = new System.Drawing.Point(368, 53);
         this.VelocityActualTextBox.MaxLength = 3;
         this.VelocityActualTextBox.Name = "VelocityActualTextBox";
         this.VelocityActualTextBox.ReadOnly = true;
         this.VelocityActualTextBox.Size = new System.Drawing.Size(83, 20);
         this.VelocityActualTextBox.TabIndex = 202;
         this.VelocityActualTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label24
         // 
         this.label24.AutoSize = true;
         this.label24.Location = new System.Drawing.Point(262, 56);
         this.label24.Name = "label24";
         this.label24.Size = new System.Drawing.Size(104, 13);
         this.label24.TabIndex = 201;
         this.label24.Text = "VELOCITY ACTUAL";
         this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // VelocityTargetTextBox
         // 
         this.VelocityTargetTextBox.Location = new System.Drawing.Point(173, 53);
         this.VelocityTargetTextBox.MaxLength = 3;
         this.VelocityTargetTextBox.Name = "VelocityTargetTextBox";
         this.VelocityTargetTextBox.ReadOnly = true;
         this.VelocityTargetTextBox.Size = new System.Drawing.Size(83, 20);
         this.VelocityTargetTextBox.TabIndex = 199;
         this.VelocityTargetTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label25
         // 
         this.label25.AutoSize = true;
         this.label25.Location = new System.Drawing.Point(65, 56);
         this.label25.Name = "label25";
         this.label25.Size = new System.Drawing.Size(106, 13);
         this.label25.TabIndex = 200;
         this.label25.Text = "VELOCITY TARGET";
         this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // ConsumerHeartbeatModeTextBox
         // 
         this.ConsumerHeartbeatModeTextBox.Location = new System.Drawing.Point(771, 5);
         this.ConsumerHeartbeatModeTextBox.MaxLength = 3;
         this.ConsumerHeartbeatModeTextBox.Name = "ConsumerHeartbeatModeTextBox";
         this.ConsumerHeartbeatModeTextBox.ReadOnly = true;
         this.ConsumerHeartbeatModeTextBox.Size = new System.Drawing.Size(25, 20);
         this.ConsumerHeartbeatModeTextBox.TabIndex = 207;
         this.ConsumerHeartbeatModeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // UserModeLabel
         // 
         this.UserModeLabel.BackColor = System.Drawing.SystemColors.Control;
         this.UserModeLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.UserModeLabel.Location = new System.Drawing.Point(843, 4);
         this.UserModeLabel.Name = "UserModeLabel";
         this.UserModeLabel.Size = new System.Drawing.Size(21, 20);
         this.UserModeLabel.TabIndex = 208;
         this.UserModeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label27
         // 
         this.label27.AutoSize = true;
         this.label27.Location = new System.Drawing.Point(817, 8);
         this.label27.Name = "label27";
         this.label27.Size = new System.Drawing.Size(24, 13);
         this.label27.TabIndex = 209;
         this.label27.Text = "UM";
         this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // MotorStateLabel
         // 
         this.MotorStateLabel.BackColor = System.Drawing.SystemColors.Control;
         this.MotorStateLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.MotorStateLabel.Location = new System.Drawing.Point(843, 27);
         this.MotorStateLabel.Name = "MotorStateLabel";
         this.MotorStateLabel.Size = new System.Drawing.Size(21, 20);
         this.MotorStateLabel.TabIndex = 210;
         this.MotorStateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label28
         // 
         this.label28.AutoSize = true;
         this.label28.Location = new System.Drawing.Point(794, 30);
         this.label28.Name = "label28";
         this.label28.Size = new System.Drawing.Size(47, 13);
         this.label28.TabIndex = 211;
         this.label28.Text = "MOTOR";
         this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // ElmoMotorDeviceControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.Controls.Add(this.label28);
         this.Controls.Add(this.MotorStateLabel);
         this.Controls.Add(this.label27);
         this.Controls.Add(this.UserModeLabel);
         this.Controls.Add(this.ConsumerHeartbeatModeTextBox);
         this.Controls.Add(this.VelocityDecelerationTextBox);
         this.Controls.Add(this.label22);
         this.Controls.Add(this.VelocityAccelerationTextBox);
         this.Controls.Add(this.label23);
         this.Controls.Add(this.VelocityActualTextBox);
         this.Controls.Add(this.label24);
         this.Controls.Add(this.VelocityTargetTextBox);
         this.Controls.Add(this.label25);
         this.Controls.Add(this.MaximumTorqueTextBox);
         this.Controls.Add(this.label19);
         this.Controls.Add(this.MaximumCurrentTextBox);
         this.Controls.Add(this.label20);
         this.Controls.Add(this.TorqueSlopeTextBox);
         this.Controls.Add(this.label18);
         this.Controls.Add(this.MotorRatedTorqueTextBox);
         this.Controls.Add(this.label17);
         this.Controls.Add(this.MotorRatedCurrentTextBox);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.TorqueActualTextBox);
         this.Controls.Add(this.label7);
         this.Controls.Add(this.TorqueTargetTextBox);
         this.Controls.Add(this.label21);
         this.Controls.Add(this.ErrorStatusTextBox);
         this.Controls.Add(this.label16);
         this.Controls.Add(this.ErrorCodeTextBox);
         this.Controls.Add(this.label15);
         this.Controls.Add(this.ConsumerHeartbeatTimeTextBox);
         this.Controls.Add(this.ProducerHeartbeatTimeTextBox);
         this.Controls.Add(this.label14);
         this.Controls.Add(this.label13);
         this.Controls.Add(this.DisplayModeTextBox);
         this.Controls.Add(this.ModeTextBox);
         this.Controls.Add(this.label5);
         this.Controls.Add(this.StatusTextBox);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.ControlWordTextBox);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.label12);
         this.Controls.Add(this.label11);
         this.Controls.Add(this.DIn5LevelCheckBox);
         this.Controls.Add(this.DIn4LevelCheckBox);
         this.Controls.Add(this.DIn3LevelCheckBox);
         this.Controls.Add(this.label10);
         this.Controls.Add(this.DIn2LevelCheckBox);
         this.Controls.Add(this.label9);
         this.Controls.Add(this.DIn1LevelCheckBox);
         this.Controls.Add(this.label8);
         this.Controls.Add(this.DIn0LevelCheckBox);
         this.Controls.Add(this.label6);
         this.Controls.Add(this.BusIdTextBox);
         this.Controls.Add(this.EnabledCheckBox);
         this.Controls.Add(this.DeviceStateLabel);
         this.Controls.Add(this.DescriptionTextBox);
         this.Controls.Add(this.NodeIdTextBox);
         this.Controls.Add(this.label1);
         this.Name = "ElmoMotorDeviceControl";
         this.Size = new System.Drawing.Size(884, 169);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.TextBox NodeIdTextBox;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox DescriptionTextBox;
      private System.Windows.Forms.Label DeviceStateLabel;
      private System.Windows.Forms.CheckBox EnabledCheckBox;
      private System.Windows.Forms.TextBox BusIdTextBox;
      private System.Windows.Forms.CheckBox DIn5LevelCheckBox;
      private System.Windows.Forms.CheckBox DIn4LevelCheckBox;
      private System.Windows.Forms.CheckBox DIn3LevelCheckBox;
      private System.Windows.Forms.Label label10;
      private System.Windows.Forms.CheckBox DIn2LevelCheckBox;
      private System.Windows.Forms.Label label9;
      private System.Windows.Forms.CheckBox DIn1LevelCheckBox;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.CheckBox DIn0LevelCheckBox;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.Label label12;
      private System.Windows.Forms.Label label11;
      private System.Windows.Forms.TextBox ConsumerHeartbeatTimeTextBox;
      private System.Windows.Forms.TextBox ProducerHeartbeatTimeTextBox;
      private System.Windows.Forms.Label label14;
      private System.Windows.Forms.Label label13;
      private System.Windows.Forms.TextBox DisplayModeTextBox;
      private System.Windows.Forms.TextBox ModeTextBox;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.TextBox StatusTextBox;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.TextBox ControlWordTextBox;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.TextBox ErrorStatusTextBox;
      private System.Windows.Forms.Label label16;
      private System.Windows.Forms.TextBox ErrorCodeTextBox;
      private System.Windows.Forms.Label label15;
      private System.Windows.Forms.TextBox MaximumTorqueTextBox;
      private System.Windows.Forms.Label label19;
      private System.Windows.Forms.TextBox MaximumCurrentTextBox;
      private System.Windows.Forms.Label label20;
      private System.Windows.Forms.TextBox TorqueSlopeTextBox;
      private System.Windows.Forms.Label label18;
      private System.Windows.Forms.TextBox MotorRatedTorqueTextBox;
      private System.Windows.Forms.Label label17;
      private System.Windows.Forms.TextBox MotorRatedCurrentTextBox;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TextBox TorqueActualTextBox;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.TextBox TorqueTargetTextBox;
      private System.Windows.Forms.Label label21;
      private System.Windows.Forms.TextBox VelocityDecelerationTextBox;
      private System.Windows.Forms.Label label22;
      private System.Windows.Forms.TextBox VelocityAccelerationTextBox;
      private System.Windows.Forms.Label label23;
      private System.Windows.Forms.TextBox VelocityActualTextBox;
      private System.Windows.Forms.Label label24;
      private System.Windows.Forms.TextBox VelocityTargetTextBox;
      private System.Windows.Forms.Label label25;
      private System.Windows.Forms.TextBox ConsumerHeartbeatModeTextBox;
      private System.Windows.Forms.Label UserModeLabel;
      private System.Windows.Forms.Label label27;
      private System.Windows.Forms.Label MotorStateLabel;
      private System.Windows.Forms.Label label28;
   }
}
