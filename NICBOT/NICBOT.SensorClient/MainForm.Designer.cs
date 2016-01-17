namespace NICBOT.SensorClient
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
         this.TimeLabel = new System.Windows.Forms.ToolStripStatusLabel();
         this.CmdTextBox = new System.Windows.Forms.TextBox();
         this.label6 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.ClientPortTextBox = new System.Windows.Forms.TextBox();
         this.ClientAddressTextBox = new System.Windows.Forms.TextBox();
         this.ClientActivityButton = new System.Windows.Forms.Button();
         this.ClientSendButton = new System.Windows.Forms.Button();
         this.RspTextBox = new System.Windows.Forms.TextBox();
         this.label3 = new System.Windows.Forms.Label();
         this.TickTimer = new System.Windows.Forms.Timer(this.components);
         this.groupBox2 = new System.Windows.Forms.GroupBox();
         this.LocationServerIpTextBox = new System.Windows.Forms.TextBox();
         this.LocationServerRspTextBox = new System.Windows.Forms.TextBox();
         this.label4 = new System.Windows.Forms.Label();
         this.LocationServerCmdTextBox = new System.Windows.Forms.TextBox();
         this.label7 = new System.Windows.Forms.Label();
         this.LocationServerActivityButton = new System.Windows.Forms.Button();
         this.label9 = new System.Windows.Forms.Label();
         this.LocationServerPortTextBox = new System.Windows.Forms.TextBox();
         this.SensorClientGroupBox = new System.Windows.Forms.GroupBox();
         this.label1 = new System.Windows.Forms.Label();
         this.label5 = new System.Windows.Forms.Label();
         this.MainStatusStrip.SuspendLayout();
         this.groupBox2.SuspendLayout();
         this.SensorClientGroupBox.SuspendLayout();
         this.SuspendLayout();
         // 
         // MainStatusStrip
         // 
         this.MainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel,
            this.TimeLabel});
         this.MainStatusStrip.Location = new System.Drawing.Point(0, 270);
         this.MainStatusStrip.Name = "MainStatusStrip";
         this.MainStatusStrip.Size = new System.Drawing.Size(419, 22);
         this.MainStatusStrip.TabIndex = 0;
         this.MainStatusStrip.Text = "statusStrip1";
         // 
         // StatusLabel
         // 
         this.StatusLabel.Name = "StatusLabel";
         this.StatusLabel.Size = new System.Drawing.Size(304, 17);
         this.StatusLabel.Spring = true;
         this.StatusLabel.Text = "Status";
         this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // TimeLabel
         // 
         this.TimeLabel.AutoSize = false;
         this.TimeLabel.Name = "TimeLabel";
         this.TimeLabel.Size = new System.Drawing.Size(100, 17);
         this.TimeLabel.Text = "00:00:00.0";
         // 
         // CmdTextBox
         // 
         this.CmdTextBox.Location = new System.Drawing.Point(53, 50);
         this.CmdTextBox.Name = "CmdTextBox";
         this.CmdTextBox.Size = new System.Drawing.Size(328, 20);
         this.CmdTextBox.TabIndex = 40;
         this.CmdTextBox.Text = "CMD1,+10.1,+10.1,2015-08-25,N,123,1,180.00,12:00:01";
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(25, 53);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(28, 13);
         this.label6.TabIndex = 39;
         this.label6.Text = "Cmd";
         this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(146, 26);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(26, 13);
         this.label2.TabIndex = 41;
         this.label2.Text = "Port";
         this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // ClientPortTextBox
         // 
         this.ClientPortTextBox.Location = new System.Drawing.Point(175, 23);
         this.ClientPortTextBox.Name = "ClientPortTextBox";
         this.ClientPortTextBox.Size = new System.Drawing.Size(55, 20);
         this.ClientPortTextBox.TabIndex = 42;
         // 
         // ClientAddressTextBox
         // 
         this.ClientAddressTextBox.Location = new System.Drawing.Point(53, 23);
         this.ClientAddressTextBox.Name = "ClientAddressTextBox";
         this.ClientAddressTextBox.Size = new System.Drawing.Size(87, 20);
         this.ClientAddressTextBox.TabIndex = 44;
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
         // RspTextBox
         // 
         this.RspTextBox.Location = new System.Drawing.Point(53, 105);
         this.RspTextBox.Name = "RspTextBox";
         this.RspTextBox.Size = new System.Drawing.Size(328, 20);
         this.RspTextBox.TabIndex = 48;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(25, 108);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(26, 13);
         this.label3.TabIndex = 47;
         this.label3.Text = "Rsp";
         this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // TickTimer
         // 
         this.TickTimer.Tick += new System.EventHandler(this.TickTimer_Tick);
         // 
         // groupBox2
         // 
         this.groupBox2.Controls.Add(this.label5);
         this.groupBox2.Controls.Add(this.LocationServerIpTextBox);
         this.groupBox2.Controls.Add(this.LocationServerRspTextBox);
         this.groupBox2.Controls.Add(this.label4);
         this.groupBox2.Controls.Add(this.LocationServerCmdTextBox);
         this.groupBox2.Controls.Add(this.label7);
         this.groupBox2.Controls.Add(this.LocationServerActivityButton);
         this.groupBox2.Controls.Add(this.label9);
         this.groupBox2.Controls.Add(this.LocationServerPortTextBox);
         this.groupBox2.Location = new System.Drawing.Point(12, 152);
         this.groupBox2.Name = "groupBox2";
         this.groupBox2.Size = new System.Drawing.Size(396, 107);
         this.groupBox2.TabIndex = 49;
         this.groupBox2.TabStop = false;
         this.groupBox2.Text = "Location Server";
         // 
         // LocationServerIpTextBox
         // 
         this.LocationServerIpTextBox.Location = new System.Drawing.Point(54, 22);
         this.LocationServerIpTextBox.Name = "LocationServerIpTextBox";
         this.LocationServerIpTextBox.Size = new System.Drawing.Size(87, 20);
         this.LocationServerIpTextBox.TabIndex = 52;
         // 
         // LocationServerRspTextBox
         // 
         this.LocationServerRspTextBox.Location = new System.Drawing.Point(54, 75);
         this.LocationServerRspTextBox.Name = "LocationServerRspTextBox";
         this.LocationServerRspTextBox.Size = new System.Drawing.Size(328, 20);
         this.LocationServerRspTextBox.TabIndex = 50;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(26, 78);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(26, 13);
         this.label4.TabIndex = 49;
         this.label4.Text = "Rsp";
         this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // LocationServerCmdTextBox
         // 
         this.LocationServerCmdTextBox.Location = new System.Drawing.Point(54, 49);
         this.LocationServerCmdTextBox.Name = "LocationServerCmdTextBox";
         this.LocationServerCmdTextBox.ReadOnly = true;
         this.LocationServerCmdTextBox.Size = new System.Drawing.Size(328, 20);
         this.LocationServerCmdTextBox.TabIndex = 39;
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(26, 52);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(28, 13);
         this.label7.TabIndex = 37;
         this.label7.Text = "Cmd";
         this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // LocationServerActivityButton
         // 
         this.LocationServerActivityButton.Location = new System.Drawing.Point(293, 20);
         this.LocationServerActivityButton.Name = "LocationServerActivityButton";
         this.LocationServerActivityButton.Size = new System.Drawing.Size(88, 23);
         this.LocationServerActivityButton.TabIndex = 26;
         this.LocationServerActivityButton.Text = "Activity";
         this.LocationServerActivityButton.UseVisualStyleBackColor = true;
         this.LocationServerActivityButton.Click += new System.EventHandler(this.ServerActivityButton_Click);
         // 
         // label9
         // 
         this.label9.AutoSize = true;
         this.label9.Location = new System.Drawing.Point(147, 25);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(26, 13);
         this.label9.TabIndex = 27;
         this.label9.Text = "Port";
         this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // LocationServerPortTextBox
         // 
         this.LocationServerPortTextBox.Location = new System.Drawing.Point(176, 22);
         this.LocationServerPortTextBox.Name = "LocationServerPortTextBox";
         this.LocationServerPortTextBox.Size = new System.Drawing.Size(55, 20);
         this.LocationServerPortTextBox.TabIndex = 28;
         // 
         // SensorClientGroupBox
         // 
         this.SensorClientGroupBox.Controls.Add(this.label1);
         this.SensorClientGroupBox.Controls.Add(this.RspTextBox);
         this.SensorClientGroupBox.Controls.Add(this.label6);
         this.SensorClientGroupBox.Controls.Add(this.ClientActivityButton);
         this.SensorClientGroupBox.Controls.Add(this.ClientAddressTextBox);
         this.SensorClientGroupBox.Controls.Add(this.label2);
         this.SensorClientGroupBox.Controls.Add(this.ClientSendButton);
         this.SensorClientGroupBox.Controls.Add(this.ClientPortTextBox);
         this.SensorClientGroupBox.Controls.Add(this.CmdTextBox);
         this.SensorClientGroupBox.Controls.Add(this.label3);
         this.SensorClientGroupBox.Location = new System.Drawing.Point(12, 12);
         this.SensorClientGroupBox.Name = "SensorClientGroupBox";
         this.SensorClientGroupBox.Size = new System.Drawing.Size(396, 134);
         this.SensorClientGroupBox.TabIndex = 50;
         this.SensorClientGroupBox.TabStop = false;
         this.SensorClientGroupBox.Text = "Sensor Client";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(6, 26);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(45, 13);
         this.label1.TabIndex = 49;
         this.label1.Text = "Address";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(7, 25);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(45, 13);
         this.label5.TabIndex = 53;
         this.label5.Text = "Address";
         this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // MainForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(419, 292);
         this.Controls.Add(this.SensorClientGroupBox);
         this.Controls.Add(this.groupBox2);
         this.Controls.Add(this.MainStatusStrip);
         this.MinimumSize = new System.Drawing.Size(435, 330);
         this.Name = "MainForm";
         this.Text = "NICBOT Sensor Client";
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
         this.Shown += new System.EventHandler(this.MainForm_Shown);
         this.MainStatusStrip.ResumeLayout(false);
         this.MainStatusStrip.PerformLayout();
         this.groupBox2.ResumeLayout(false);
         this.groupBox2.PerformLayout();
         this.SensorClientGroupBox.ResumeLayout(false);
         this.SensorClientGroupBox.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.StatusStrip MainStatusStrip;
      private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
      private System.Windows.Forms.TextBox CmdTextBox;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TextBox ClientPortTextBox;
      private System.Windows.Forms.TextBox ClientAddressTextBox;
      private System.Windows.Forms.Button ClientActivityButton;
      private System.Windows.Forms.Button ClientSendButton;
      private System.Windows.Forms.TextBox RspTextBox;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Timer TickTimer;
      private System.Windows.Forms.ToolStripStatusLabel TimeLabel;
      private System.Windows.Forms.GroupBox groupBox2;
      private System.Windows.Forms.TextBox LocationServerIpTextBox;
      private System.Windows.Forms.TextBox LocationServerRspTextBox;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.TextBox LocationServerCmdTextBox;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.Button LocationServerActivityButton;
      private System.Windows.Forms.Label label9;
      private System.Windows.Forms.TextBox LocationServerPortTextBox;
      private System.Windows.Forms.GroupBox SensorClientGroupBox;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Label label1;
   }
}

