namespace NICBOT.SensorSim
{
   partial class MainForm
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
         this.MainStatusStrip = new System.Windows.Forms.StatusStrip();
         this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
         this.ThicknessPortTextBox = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.ThicknessActivityButton = new System.Windows.Forms.Button();
         this.label1 = new System.Windows.Forms.Label();
         this.ThicknessReadingTextBox = new System.Windows.Forms.TextBox();
         this.label3 = new System.Windows.Forms.Label();
         this.ThicknessDelayTextBox = new System.Windows.Forms.TextBox();
         this.label4 = new System.Windows.Forms.Label();
         this.ThicknessStatusLabel = new System.Windows.Forms.Label();
         this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
         this.label6 = new System.Windows.Forms.Label();
         this.ThicknessGroupBox = new System.Windows.Forms.GroupBox();
         this.ThicknessAddressTextBox = new System.Windows.Forms.TextBox();
         this.ThicknessCmdTextBox = new System.Windows.Forms.TextBox();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.StressAddressTextBox = new System.Windows.Forms.TextBox();
         this.StressCmdTextBox = new System.Windows.Forms.TextBox();
         this.label5 = new System.Windows.Forms.Label();
         this.StressActivityButton = new System.Windows.Forms.Button();
         this.label8 = new System.Windows.Forms.Label();
         this.StressStatusLabel = new System.Windows.Forms.Label();
         this.StressPortTextBox = new System.Windows.Forms.TextBox();
         this.StressDelayTextBox = new System.Windows.Forms.TextBox();
         this.label10 = new System.Windows.Forms.Label();
         this.label11 = new System.Windows.Forms.Label();
         this.label12 = new System.Windows.Forms.Label();
         this.StressReadingTextBox = new System.Windows.Forms.TextBox();
         this.LocationClientGroupBox = new System.Windows.Forms.GroupBox();
         this.label9 = new System.Windows.Forms.Label();
         this.RspTextBox = new System.Windows.Forms.TextBox();
         this.label7 = new System.Windows.Forms.Label();
         this.ClientActivityButton = new System.Windows.Forms.Button();
         this.ClientAddressTextBox = new System.Windows.Forms.TextBox();
         this.label13 = new System.Windows.Forms.Label();
         this.ClientSendButton = new System.Windows.Forms.Button();
         this.ClientPortTextBox = new System.Windows.Forms.TextBox();
         this.CmdTextBox = new System.Windows.Forms.TextBox();
         this.label14 = new System.Windows.Forms.Label();
         this.MainStatusStrip.SuspendLayout();
         this.ThicknessGroupBox.SuspendLayout();
         this.groupBox1.SuspendLayout();
         this.LocationClientGroupBox.SuspendLayout();
         this.SuspendLayout();
         // 
         // MainStatusStrip
         // 
         this.MainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
         this.MainStatusStrip.Location = new System.Drawing.Point(0, 386);
         this.MainStatusStrip.Name = "MainStatusStrip";
         this.MainStatusStrip.Size = new System.Drawing.Size(421, 24);
         this.MainStatusStrip.TabIndex = 25;
         this.MainStatusStrip.Text = "statusStrip1";
         // 
         // StatusLabel
         // 
         this.StatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
         this.StatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
         this.StatusLabel.Name = "StatusLabel";
         this.StatusLabel.Size = new System.Drawing.Size(406, 19);
         this.StatusLabel.Spring = true;
         this.StatusLabel.Text = "toolStripStatusLabel1";
         this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // ThicknessPortTextBox
         // 
         this.ThicknessPortTextBox.Location = new System.Drawing.Point(54, 48);
         this.ThicknessPortTextBox.Name = "ThicknessPortTextBox";
         this.ThicknessPortTextBox.Size = new System.Drawing.Size(55, 20);
         this.ThicknessPortTextBox.TabIndex = 28;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(25, 51);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(26, 13);
         this.label2.TabIndex = 27;
         this.label2.Text = "Port";
         this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // ThicknessActivityButton
         // 
         this.ThicknessActivityButton.Location = new System.Drawing.Point(167, 46);
         this.ThicknessActivityButton.Name = "ThicknessActivityButton";
         this.ThicknessActivityButton.Size = new System.Drawing.Size(88, 23);
         this.ThicknessActivityButton.TabIndex = 26;
         this.ThicknessActivityButton.Text = "Activity";
         this.ThicknessActivityButton.UseVisualStyleBackColor = true;
         this.ThicknessActivityButton.Click += new System.EventHandler(this.ThicknessActivityButton_Click);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(6, 25);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(45, 13);
         this.label1.TabIndex = 30;
         this.label1.Text = "Address";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // ThicknessReadingTextBox
         // 
         this.ThicknessReadingTextBox.Location = new System.Drawing.Point(326, 48);
         this.ThicknessReadingTextBox.Name = "ThicknessReadingTextBox";
         this.ThicknessReadingTextBox.Size = new System.Drawing.Size(55, 20);
         this.ThicknessReadingTextBox.TabIndex = 32;
         this.ThicknessReadingTextBox.TextChanged += new System.EventHandler(this.ThicknessReadingTextBox_TextChanged);
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(277, 51);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(47, 13);
         this.label3.TabIndex = 31;
         this.label3.Text = "Reading";
         this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // ThicknessDelayTextBox
         // 
         this.ThicknessDelayTextBox.Location = new System.Drawing.Point(326, 22);
         this.ThicknessDelayTextBox.Name = "ThicknessDelayTextBox";
         this.ThicknessDelayTextBox.Size = new System.Drawing.Size(55, 20);
         this.ThicknessDelayTextBox.TabIndex = 34;
         this.ThicknessDelayTextBox.TextChanged += new System.EventHandler(this.ThicknessDelayTextBox_TextChanged);
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(289, 25);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(34, 13);
         this.label4.TabIndex = 33;
         this.label4.Text = "Delay";
         this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // ThicknessStatusLabel
         // 
         this.ThicknessStatusLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.ThicknessStatusLabel.Location = new System.Drawing.Point(166, 21);
         this.ThicknessStatusLabel.Name = "ThicknessStatusLabel";
         this.ThicknessStatusLabel.Size = new System.Drawing.Size(89, 22);
         this.ThicknessStatusLabel.TabIndex = 35;
         this.ThicknessStatusLabel.Text = "Delay";
         this.ThicknessStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // UpdateTimer
         // 
         this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(25, 76);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(28, 13);
         this.label6.TabIndex = 37;
         this.label6.Text = "Cmd";
         this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // ThicknessGroupBox
         // 
         this.ThicknessGroupBox.Controls.Add(this.ThicknessAddressTextBox);
         this.ThicknessGroupBox.Controls.Add(this.ThicknessCmdTextBox);
         this.ThicknessGroupBox.Controls.Add(this.label6);
         this.ThicknessGroupBox.Controls.Add(this.ThicknessActivityButton);
         this.ThicknessGroupBox.Controls.Add(this.label2);
         this.ThicknessGroupBox.Controls.Add(this.ThicknessStatusLabel);
         this.ThicknessGroupBox.Controls.Add(this.ThicknessPortTextBox);
         this.ThicknessGroupBox.Controls.Add(this.ThicknessDelayTextBox);
         this.ThicknessGroupBox.Controls.Add(this.label1);
         this.ThicknessGroupBox.Controls.Add(this.label4);
         this.ThicknessGroupBox.Controls.Add(this.label3);
         this.ThicknessGroupBox.Controls.Add(this.ThicknessReadingTextBox);
         this.ThicknessGroupBox.Location = new System.Drawing.Point(12, 12);
         this.ThicknessGroupBox.Name = "ThicknessGroupBox";
         this.ThicknessGroupBox.Size = new System.Drawing.Size(396, 109);
         this.ThicknessGroupBox.TabIndex = 38;
         this.ThicknessGroupBox.TabStop = false;
         this.ThicknessGroupBox.Text = "Thickness Sensor";
         // 
         // ThicknessAddressTextBox
         // 
         this.ThicknessAddressTextBox.Location = new System.Drawing.Point(53, 22);
         this.ThicknessAddressTextBox.Name = "ThicknessAddressTextBox";
         this.ThicknessAddressTextBox.Size = new System.Drawing.Size(87, 20);
         this.ThicknessAddressTextBox.TabIndex = 45;
         // 
         // ThicknessCmdTextBox
         // 
         this.ThicknessCmdTextBox.Location = new System.Drawing.Point(53, 73);
         this.ThicknessCmdTextBox.Name = "ThicknessCmdTextBox";
         this.ThicknessCmdTextBox.ReadOnly = true;
         this.ThicknessCmdTextBox.Size = new System.Drawing.Size(328, 20);
         this.ThicknessCmdTextBox.TabIndex = 38;
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.StressAddressTextBox);
         this.groupBox1.Controls.Add(this.StressCmdTextBox);
         this.groupBox1.Controls.Add(this.label5);
         this.groupBox1.Controls.Add(this.StressActivityButton);
         this.groupBox1.Controls.Add(this.label8);
         this.groupBox1.Controls.Add(this.StressStatusLabel);
         this.groupBox1.Controls.Add(this.StressPortTextBox);
         this.groupBox1.Controls.Add(this.StressDelayTextBox);
         this.groupBox1.Controls.Add(this.label10);
         this.groupBox1.Controls.Add(this.label11);
         this.groupBox1.Controls.Add(this.label12);
         this.groupBox1.Controls.Add(this.StressReadingTextBox);
         this.groupBox1.Location = new System.Drawing.Point(12, 127);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(396, 109);
         this.groupBox1.TabIndex = 39;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Stress Sensor";
         // 
         // StressAddressTextBox
         // 
         this.StressAddressTextBox.Location = new System.Drawing.Point(53, 22);
         this.StressAddressTextBox.Name = "StressAddressTextBox";
         this.StressAddressTextBox.Size = new System.Drawing.Size(87, 20);
         this.StressAddressTextBox.TabIndex = 45;
         // 
         // StressCmdTextBox
         // 
         this.StressCmdTextBox.Location = new System.Drawing.Point(53, 73);
         this.StressCmdTextBox.Name = "StressCmdTextBox";
         this.StressCmdTextBox.ReadOnly = true;
         this.StressCmdTextBox.Size = new System.Drawing.Size(328, 20);
         this.StressCmdTextBox.TabIndex = 39;
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(25, 76);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(28, 13);
         this.label5.TabIndex = 37;
         this.label5.Text = "Cmd";
         this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // StressActivityButton
         // 
         this.StressActivityButton.Location = new System.Drawing.Point(167, 46);
         this.StressActivityButton.Name = "StressActivityButton";
         this.StressActivityButton.Size = new System.Drawing.Size(88, 23);
         this.StressActivityButton.TabIndex = 26;
         this.StressActivityButton.Text = "Activity";
         this.StressActivityButton.UseVisualStyleBackColor = true;
         this.StressActivityButton.Click += new System.EventHandler(this.StressActivityButton_Click);
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.Location = new System.Drawing.Point(25, 51);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(26, 13);
         this.label8.TabIndex = 27;
         this.label8.Text = "Port";
         this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // StressStatusLabel
         // 
         this.StressStatusLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.StressStatusLabel.Location = new System.Drawing.Point(166, 21);
         this.StressStatusLabel.Name = "StressStatusLabel";
         this.StressStatusLabel.Size = new System.Drawing.Size(89, 22);
         this.StressStatusLabel.TabIndex = 35;
         this.StressStatusLabel.Text = "Delay";
         this.StressStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // StressPortTextBox
         // 
         this.StressPortTextBox.Location = new System.Drawing.Point(54, 48);
         this.StressPortTextBox.Name = "StressPortTextBox";
         this.StressPortTextBox.Size = new System.Drawing.Size(55, 20);
         this.StressPortTextBox.TabIndex = 28;
         // 
         // StressDelayTextBox
         // 
         this.StressDelayTextBox.Location = new System.Drawing.Point(326, 22);
         this.StressDelayTextBox.Name = "StressDelayTextBox";
         this.StressDelayTextBox.Size = new System.Drawing.Size(55, 20);
         this.StressDelayTextBox.TabIndex = 34;
         this.StressDelayTextBox.TextChanged += new System.EventHandler(this.StressDelayTextBox_TextChanged);
         // 
         // label10
         // 
         this.label10.AutoSize = true;
         this.label10.Location = new System.Drawing.Point(6, 25);
         this.label10.Name = "label10";
         this.label10.Size = new System.Drawing.Size(45, 13);
         this.label10.TabIndex = 30;
         this.label10.Text = "Address";
         this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // label11
         // 
         this.label11.AutoSize = true;
         this.label11.Location = new System.Drawing.Point(289, 25);
         this.label11.Name = "label11";
         this.label11.Size = new System.Drawing.Size(34, 13);
         this.label11.TabIndex = 33;
         this.label11.Text = "Delay";
         this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // label12
         // 
         this.label12.AutoSize = true;
         this.label12.Location = new System.Drawing.Point(277, 51);
         this.label12.Name = "label12";
         this.label12.Size = new System.Drawing.Size(47, 13);
         this.label12.TabIndex = 31;
         this.label12.Text = "Reading";
         this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // StressReadingTextBox
         // 
         this.StressReadingTextBox.Location = new System.Drawing.Point(326, 48);
         this.StressReadingTextBox.Name = "StressReadingTextBox";
         this.StressReadingTextBox.Size = new System.Drawing.Size(55, 20);
         this.StressReadingTextBox.TabIndex = 32;
         this.StressReadingTextBox.TextChanged += new System.EventHandler(this.StressReadingTextBox_TextChanged);
         // 
         // LocationClientGroupBox
         // 
         this.LocationClientGroupBox.Controls.Add(this.label9);
         this.LocationClientGroupBox.Controls.Add(this.RspTextBox);
         this.LocationClientGroupBox.Controls.Add(this.label7);
         this.LocationClientGroupBox.Controls.Add(this.ClientActivityButton);
         this.LocationClientGroupBox.Controls.Add(this.ClientAddressTextBox);
         this.LocationClientGroupBox.Controls.Add(this.label13);
         this.LocationClientGroupBox.Controls.Add(this.ClientSendButton);
         this.LocationClientGroupBox.Controls.Add(this.ClientPortTextBox);
         this.LocationClientGroupBox.Controls.Add(this.CmdTextBox);
         this.LocationClientGroupBox.Controls.Add(this.label14);
         this.LocationClientGroupBox.Location = new System.Drawing.Point(12, 242);
         this.LocationClientGroupBox.Name = "LocationClientGroupBox";
         this.LocationClientGroupBox.Size = new System.Drawing.Size(396, 134);
         this.LocationClientGroupBox.TabIndex = 51;
         this.LocationClientGroupBox.TabStop = false;
         this.LocationClientGroupBox.Text = "Location Client";
         // 
         // label9
         // 
         this.label9.AutoSize = true;
         this.label9.Location = new System.Drawing.Point(6, 26);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(45, 13);
         this.label9.TabIndex = 49;
         this.label9.Text = "Address";
         this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // RspTextBox
         // 
         this.RspTextBox.Location = new System.Drawing.Point(53, 105);
         this.RspTextBox.Name = "RspTextBox";
         this.RspTextBox.Size = new System.Drawing.Size(328, 20);
         this.RspTextBox.TabIndex = 48;
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(25, 53);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(28, 13);
         this.label7.TabIndex = 39;
         this.label7.Text = "Cmd";
         this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // ClientActivityButton
         // 
         this.ClientActivityButton.Location = new System.Drawing.Point(293, 21);
         this.ClientActivityButton.Name = "ClientActivityButton";
         this.ClientActivityButton.Size = new System.Drawing.Size(88, 23);
         this.ClientActivityButton.TabIndex = 45;
         this.ClientActivityButton.Text = "Activity";
         this.ClientActivityButton.UseVisualStyleBackColor = true;
         this.ClientActivityButton.Click += new System.EventHandler(this.ClientActivityButton_Click);
         // 
         // ClientAddressTextBox
         // 
         this.ClientAddressTextBox.Location = new System.Drawing.Point(53, 23);
         this.ClientAddressTextBox.Name = "ClientAddressTextBox";
         this.ClientAddressTextBox.Size = new System.Drawing.Size(87, 20);
         this.ClientAddressTextBox.TabIndex = 44;
         // 
         // label13
         // 
         this.label13.AutoSize = true;
         this.label13.Location = new System.Drawing.Point(146, 26);
         this.label13.Name = "label13";
         this.label13.Size = new System.Drawing.Size(26, 13);
         this.label13.TabIndex = 41;
         this.label13.Text = "Port";
         this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // ClientSendButton
         // 
         this.ClientSendButton.Location = new System.Drawing.Point(306, 76);
         this.ClientSendButton.Name = "ClientSendButton";
         this.ClientSendButton.Size = new System.Drawing.Size(75, 23);
         this.ClientSendButton.TabIndex = 46;
         this.ClientSendButton.Text = "Send";
         this.ClientSendButton.UseVisualStyleBackColor = true;
         this.ClientSendButton.Click += new System.EventHandler(this.ClientSendButton_Click);
         // 
         // ClientPortTextBox
         // 
         this.ClientPortTextBox.Location = new System.Drawing.Point(175, 23);
         this.ClientPortTextBox.Name = "ClientPortTextBox";
         this.ClientPortTextBox.Size = new System.Drawing.Size(55, 20);
         this.ClientPortTextBox.TabIndex = 42;
         // 
         // CmdTextBox
         // 
         this.CmdTextBox.Location = new System.Drawing.Point(53, 50);
         this.CmdTextBox.Name = "CmdTextBox";
         this.CmdTextBox.Size = new System.Drawing.Size(328, 20);
         this.CmdTextBox.TabIndex = 40;
         this.CmdTextBox.Text = "CMD1,+10.1,+10.1,2015-08-25,N,123,1,180.00,12:00:01";
         // 
         // label14
         // 
         this.label14.AutoSize = true;
         this.label14.Location = new System.Drawing.Point(25, 108);
         this.label14.Name = "label14";
         this.label14.Size = new System.Drawing.Size(26, 13);
         this.label14.TabIndex = 47;
         this.label14.Text = "Rsp";
         this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // MainForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(421, 410);
         this.Controls.Add(this.LocationClientGroupBox);
         this.Controls.Add(this.groupBox1);
         this.Controls.Add(this.ThicknessGroupBox);
         this.Controls.Add(this.MainStatusStrip);
         this.MinimumSize = new System.Drawing.Size(437, 448);
         this.Name = "MainForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "NICBOT Sensor Simulation";
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
         this.Shown += new System.EventHandler(this.MainForm_Shown);
         this.MainStatusStrip.ResumeLayout(false);
         this.MainStatusStrip.PerformLayout();
         this.ThicknessGroupBox.ResumeLayout(false);
         this.ThicknessGroupBox.PerformLayout();
         this.groupBox1.ResumeLayout(false);
         this.groupBox1.PerformLayout();
         this.LocationClientGroupBox.ResumeLayout(false);
         this.LocationClientGroupBox.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.StatusStrip MainStatusStrip;
      private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
      private System.Windows.Forms.TextBox ThicknessPortTextBox;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Button ThicknessActivityButton;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox ThicknessReadingTextBox;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.TextBox ThicknessDelayTextBox;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label ThicknessStatusLabel;
      private System.Windows.Forms.Timer UpdateTimer;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.GroupBox ThicknessGroupBox;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Button StressActivityButton;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.Label StressStatusLabel;
      private System.Windows.Forms.TextBox StressPortTextBox;
      private System.Windows.Forms.TextBox StressDelayTextBox;
      private System.Windows.Forms.Label label10;
      private System.Windows.Forms.Label label11;
      private System.Windows.Forms.Label label12;
      private System.Windows.Forms.TextBox StressReadingTextBox;
      private System.Windows.Forms.TextBox ThicknessCmdTextBox;
      private System.Windows.Forms.TextBox StressCmdTextBox;
      private System.Windows.Forms.GroupBox LocationClientGroupBox;
      private System.Windows.Forms.TextBox RspTextBox;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.Button ClientActivityButton;
      private System.Windows.Forms.TextBox ClientAddressTextBox;
      private System.Windows.Forms.Label label13;
      private System.Windows.Forms.Button ClientSendButton;
      private System.Windows.Forms.TextBox ClientPortTextBox;
      private System.Windows.Forms.TextBox CmdTextBox;
      private System.Windows.Forms.Label label14;
      private System.Windows.Forms.TextBox ThicknessAddressTextBox;
      private System.Windows.Forms.TextBox StressAddressTextBox;
      private System.Windows.Forms.Label label9;
   }
}

