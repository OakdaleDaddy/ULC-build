namespace NICBOT.GUI
{
   partial class CANDeviceInformationForm
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
         this.MainPanel = new NICBOT.GUI.BorderedPanel();
         this.RestartButton = new NICBOT.GUI.NicBotButton();
         this.DeviceTypeValuePanel = new NICBOT.GUI.TextPanel();
         this.label1 = new System.Windows.Forms.Label();
         this.TraceSelectPanel = new NICBOT.GUI.BorderedPanel();
         this.HbButton = new NICBOT.GUI.NicBotButton();
         this.Tpdo4Button = new NICBOT.GUI.NicBotButton();
         this.Rpdo4Button = new NICBOT.GUI.NicBotButton();
         this.Tpdo3Button = new NICBOT.GUI.NicBotButton();
         this.Rpdo3Button = new NICBOT.GUI.NicBotButton();
         this.Tpdo2Button = new NICBOT.GUI.NicBotButton();
         this.Rpdo2Button = new NICBOT.GUI.NicBotButton();
         this.Tpdo1Button = new NICBOT.GUI.NicBotButton();
         this.Rpdo1Button = new NICBOT.GUI.NicBotButton();
         this.label5 = new System.Windows.Forms.Label();
         this.SdoButton = new NICBOT.GUI.NicBotButton();
         this.NodeIdValuePanel = new NICBOT.GUI.TextPanel();
         this.label4 = new System.Windows.Forms.Label();
         this.LoggingNameValuePanel = new NICBOT.GUI.TextPanel();
         this.label3 = new System.Windows.Forms.Label();
         this.DeviceVersionValuePanel = new NICBOT.GUI.TextPanel();
         this.label2 = new System.Windows.Forms.Label();
         this.DeviceNameValuePanel = new NICBOT.GUI.TextPanel();
         this.label28 = new System.Windows.Forms.Label();
         this.TitleLabel = new System.Windows.Forms.Label();
         this.BackButton = new NICBOT.GUI.NicBotButton();
         this.MainPanel.SuspendLayout();
         this.TraceSelectPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // MainPanel
         // 
         this.MainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
         this.MainPanel.Controls.Add(this.RestartButton);
         this.MainPanel.Controls.Add(this.DeviceTypeValuePanel);
         this.MainPanel.Controls.Add(this.label1);
         this.MainPanel.Controls.Add(this.TraceSelectPanel);
         this.MainPanel.Controls.Add(this.NodeIdValuePanel);
         this.MainPanel.Controls.Add(this.label4);
         this.MainPanel.Controls.Add(this.LoggingNameValuePanel);
         this.MainPanel.Controls.Add(this.label3);
         this.MainPanel.Controls.Add(this.DeviceVersionValuePanel);
         this.MainPanel.Controls.Add(this.label2);
         this.MainPanel.Controls.Add(this.DeviceNameValuePanel);
         this.MainPanel.Controls.Add(this.label28);
         this.MainPanel.Controls.Add(this.TitleLabel);
         this.MainPanel.Controls.Add(this.BackButton);
         this.MainPanel.EdgeWeight = 3;
         this.MainPanel.Location = new System.Drawing.Point(8, 8);
         this.MainPanel.Name = "MainPanel";
         this.MainPanel.Size = new System.Drawing.Size(642, 420);
         this.MainPanel.TabIndex = 0;
         // 
         // RestartButton
         // 
         this.RestartButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RestartButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.RestartButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.RestartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.RestartButton.Location = new System.Drawing.Point(43, 335);
         this.RestartButton.Name = "RestartButton";
         this.RestartButton.Size = new System.Drawing.Size(107, 67);
         this.RestartButton.TabIndex = 182;
         this.RestartButton.Text = "RESTART";
         this.RestartButton.UseVisualStyleBackColor = false;
         this.RestartButton.Visible = false;
         this.RestartButton.Click += new System.EventHandler(this.RestartButton_Click);
         // 
         // DeviceTypeValuePanel
         // 
         this.DeviceTypeValuePanel.BackColor = System.Drawing.Color.Black;
         this.DeviceTypeValuePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.DeviceTypeValuePanel.Enabled = false;
         this.DeviceTypeValuePanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DeviceTypeValuePanel.ForeColor = System.Drawing.Color.White;
         this.DeviceTypeValuePanel.HoldTimeoutEnable = false;
         this.DeviceTypeValuePanel.HoldTimeoutInterval = 100;
         this.DeviceTypeValuePanel.Location = new System.Drawing.Point(27, 147);
         this.DeviceTypeValuePanel.Name = "DeviceTypeValuePanel";
         this.DeviceTypeValuePanel.Size = new System.Drawing.Size(190, 42);
         this.DeviceTypeValuePanel.TabIndex = 181;
         this.DeviceTypeValuePanel.ValueText = "feeder tf-motor";
         this.DeviceTypeValuePanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label1
         // 
         this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label1.Location = new System.Drawing.Point(27, 124);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(190, 20);
         this.label1.TabIndex = 180;
         this.label1.Text = "DEVICE TYPE";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // TraceSelectPanel
         // 
         this.TraceSelectPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(80)))), ((int)(((byte)(96)))));
         this.TraceSelectPanel.Controls.Add(this.HbButton);
         this.TraceSelectPanel.Controls.Add(this.Tpdo4Button);
         this.TraceSelectPanel.Controls.Add(this.Rpdo4Button);
         this.TraceSelectPanel.Controls.Add(this.Tpdo3Button);
         this.TraceSelectPanel.Controls.Add(this.Rpdo3Button);
         this.TraceSelectPanel.Controls.Add(this.Tpdo2Button);
         this.TraceSelectPanel.Controls.Add(this.Rpdo2Button);
         this.TraceSelectPanel.Controls.Add(this.Tpdo1Button);
         this.TraceSelectPanel.Controls.Add(this.Rpdo1Button);
         this.TraceSelectPanel.Controls.Add(this.label5);
         this.TraceSelectPanel.Controls.Add(this.SdoButton);
         this.TraceSelectPanel.EdgeWeight = 2;
         this.TraceSelectPanel.Location = new System.Drawing.Point(28, 197);
         this.TraceSelectPanel.Name = "TraceSelectPanel";
         this.TraceSelectPanel.Size = new System.Drawing.Size(587, 132);
         this.TraceSelectPanel.TabIndex = 179;
         // 
         // HbButton
         // 
         this.HbButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.HbButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.HbButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.HbButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.HbButton.Location = new System.Drawing.Point(10, 82);
         this.HbButton.Name = "HbButton";
         this.HbButton.Size = new System.Drawing.Size(107, 40);
         this.HbButton.TabIndex = 187;
         this.HbButton.Text = "HB";
         this.HbButton.UseVisualStyleBackColor = false;
         this.HbButton.Click += new System.EventHandler(this.HbButton_Click);
         // 
         // Tpdo4Button
         // 
         this.Tpdo4Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.Tpdo4Button.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.Tpdo4Button.DisabledForeColor = System.Drawing.Color.Silver;
         this.Tpdo4Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.Tpdo4Button.Location = new System.Drawing.Point(470, 82);
         this.Tpdo4Button.Name = "Tpdo4Button";
         this.Tpdo4Button.Size = new System.Drawing.Size(107, 40);
         this.Tpdo4Button.TabIndex = 186;
         this.Tpdo4Button.Text = "TPDO4";
         this.Tpdo4Button.UseVisualStyleBackColor = false;
         this.Tpdo4Button.Click += new System.EventHandler(this.Tpdo4Button_Click);
         // 
         // Rpdo4Button
         // 
         this.Rpdo4Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.Rpdo4Button.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.Rpdo4Button.DisabledForeColor = System.Drawing.Color.Silver;
         this.Rpdo4Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.Rpdo4Button.Location = new System.Drawing.Point(470, 34);
         this.Rpdo4Button.Name = "Rpdo4Button";
         this.Rpdo4Button.Size = new System.Drawing.Size(107, 40);
         this.Rpdo4Button.TabIndex = 185;
         this.Rpdo4Button.Text = "RPDO4";
         this.Rpdo4Button.UseVisualStyleBackColor = false;
         this.Rpdo4Button.Click += new System.EventHandler(this.Rpdo4Button_Click);
         // 
         // Tpdo3Button
         // 
         this.Tpdo3Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.Tpdo3Button.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.Tpdo3Button.DisabledForeColor = System.Drawing.Color.Silver;
         this.Tpdo3Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.Tpdo3Button.Location = new System.Drawing.Point(355, 82);
         this.Tpdo3Button.Name = "Tpdo3Button";
         this.Tpdo3Button.Size = new System.Drawing.Size(107, 40);
         this.Tpdo3Button.TabIndex = 184;
         this.Tpdo3Button.Text = "TPDO3";
         this.Tpdo3Button.UseVisualStyleBackColor = false;
         this.Tpdo3Button.Click += new System.EventHandler(this.Tpdo3Button_Click);
         // 
         // Rpdo3Button
         // 
         this.Rpdo3Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.Rpdo3Button.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.Rpdo3Button.DisabledForeColor = System.Drawing.Color.Silver;
         this.Rpdo3Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.Rpdo3Button.Location = new System.Drawing.Point(355, 34);
         this.Rpdo3Button.Name = "Rpdo3Button";
         this.Rpdo3Button.Size = new System.Drawing.Size(107, 40);
         this.Rpdo3Button.TabIndex = 183;
         this.Rpdo3Button.Text = "RPDO3";
         this.Rpdo3Button.UseVisualStyleBackColor = false;
         this.Rpdo3Button.Click += new System.EventHandler(this.Rpdo3Button_Click);
         // 
         // Tpdo2Button
         // 
         this.Tpdo2Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.Tpdo2Button.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.Tpdo2Button.DisabledForeColor = System.Drawing.Color.Silver;
         this.Tpdo2Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.Tpdo2Button.Location = new System.Drawing.Point(240, 82);
         this.Tpdo2Button.Name = "Tpdo2Button";
         this.Tpdo2Button.Size = new System.Drawing.Size(107, 40);
         this.Tpdo2Button.TabIndex = 182;
         this.Tpdo2Button.Text = "TPDO2";
         this.Tpdo2Button.UseVisualStyleBackColor = false;
         this.Tpdo2Button.Click += new System.EventHandler(this.Tpdo2Button_Click);
         // 
         // Rpdo2Button
         // 
         this.Rpdo2Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.Rpdo2Button.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.Rpdo2Button.DisabledForeColor = System.Drawing.Color.Silver;
         this.Rpdo2Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.Rpdo2Button.Location = new System.Drawing.Point(240, 34);
         this.Rpdo2Button.Name = "Rpdo2Button";
         this.Rpdo2Button.Size = new System.Drawing.Size(107, 40);
         this.Rpdo2Button.TabIndex = 181;
         this.Rpdo2Button.Text = "RPDO2";
         this.Rpdo2Button.UseVisualStyleBackColor = false;
         this.Rpdo2Button.Click += new System.EventHandler(this.Rpdo2Button_Click);
         // 
         // Tpdo1Button
         // 
         this.Tpdo1Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.Tpdo1Button.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.Tpdo1Button.DisabledForeColor = System.Drawing.Color.Silver;
         this.Tpdo1Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.Tpdo1Button.Location = new System.Drawing.Point(125, 82);
         this.Tpdo1Button.Name = "Tpdo1Button";
         this.Tpdo1Button.Size = new System.Drawing.Size(107, 40);
         this.Tpdo1Button.TabIndex = 180;
         this.Tpdo1Button.Text = "TPDO1";
         this.Tpdo1Button.UseVisualStyleBackColor = false;
         this.Tpdo1Button.Click += new System.EventHandler(this.Tpdo1Button_Click);
         // 
         // Rpdo1Button
         // 
         this.Rpdo1Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.Rpdo1Button.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.Rpdo1Button.DisabledForeColor = System.Drawing.Color.Silver;
         this.Rpdo1Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.Rpdo1Button.Location = new System.Drawing.Point(125, 34);
         this.Rpdo1Button.Name = "Rpdo1Button";
         this.Rpdo1Button.Size = new System.Drawing.Size(107, 40);
         this.Rpdo1Button.TabIndex = 179;
         this.Rpdo1Button.Text = "RPDO1";
         this.Rpdo1Button.UseVisualStyleBackColor = false;
         this.Rpdo1Button.Click += new System.EventHandler(this.Rpdo1Button_Click);
         // 
         // label5
         // 
         this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
         this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label5.ForeColor = System.Drawing.Color.Gainsboro;
         this.label5.Location = new System.Drawing.Point(2, 2);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(583, 24);
         this.label5.TabIndex = 154;
         this.label5.Text = "LOG SELECTS";
         this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // SdoButton
         // 
         this.SdoButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SdoButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.SdoButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.SdoButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.SdoButton.Location = new System.Drawing.Point(10, 34);
         this.SdoButton.Name = "SdoButton";
         this.SdoButton.Size = new System.Drawing.Size(107, 40);
         this.SdoButton.TabIndex = 178;
         this.SdoButton.Text = "SDO";
         this.SdoButton.UseVisualStyleBackColor = false;
         this.SdoButton.Click += new System.EventHandler(this.SdoButton_Click);
         // 
         // NodeIdValuePanel
         // 
         this.NodeIdValuePanel.BackColor = System.Drawing.Color.Black;
         this.NodeIdValuePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.NodeIdValuePanel.Enabled = false;
         this.NodeIdValuePanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.NodeIdValuePanel.ForeColor = System.Drawing.Color.White;
         this.NodeIdValuePanel.HoldTimeoutEnable = false;
         this.NodeIdValuePanel.HoldTimeoutInterval = 100;
         this.NodeIdValuePanel.Location = new System.Drawing.Point(423, 147);
         this.NodeIdValuePanel.Name = "NodeIdValuePanel";
         this.NodeIdValuePanel.Size = new System.Drawing.Size(190, 42);
         this.NodeIdValuePanel.TabIndex = 177;
         this.NodeIdValuePanel.ValueText = "127";
         this.NodeIdValuePanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label4
         // 
         this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label4.Location = new System.Drawing.Point(423, 124);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(190, 20);
         this.label4.TabIndex = 176;
         this.label4.Text = "NODE ID";
         this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // LoggingNameValuePanel
         // 
         this.LoggingNameValuePanel.BackColor = System.Drawing.Color.Black;
         this.LoggingNameValuePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.LoggingNameValuePanel.Enabled = false;
         this.LoggingNameValuePanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LoggingNameValuePanel.ForeColor = System.Drawing.Color.White;
         this.LoggingNameValuePanel.HoldTimeoutEnable = false;
         this.LoggingNameValuePanel.HoldTimeoutInterval = 100;
         this.LoggingNameValuePanel.Location = new System.Drawing.Point(225, 147);
         this.LoggingNameValuePanel.Name = "LoggingNameValuePanel";
         this.LoggingNameValuePanel.Size = new System.Drawing.Size(190, 42);
         this.LoggingNameValuePanel.TabIndex = 175;
         this.LoggingNameValuePanel.ValueText = "feeder tf-motor";
         this.LoggingNameValuePanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label3
         // 
         this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label3.Location = new System.Drawing.Point(225, 124);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(190, 20);
         this.label3.TabIndex = 174;
         this.label3.Text = "LOGGING NAME";
         this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // DeviceVersionValuePanel
         // 
         this.DeviceVersionValuePanel.BackColor = System.Drawing.Color.Black;
         this.DeviceVersionValuePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.DeviceVersionValuePanel.Enabled = false;
         this.DeviceVersionValuePanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DeviceVersionValuePanel.ForeColor = System.Drawing.Color.White;
         this.DeviceVersionValuePanel.HoldTimeoutEnable = false;
         this.DeviceVersionValuePanel.HoldTimeoutInterval = 100;
         this.DeviceVersionValuePanel.Location = new System.Drawing.Point(325, 79);
         this.DeviceVersionValuePanel.Name = "DeviceVersionValuePanel";
         this.DeviceVersionValuePanel.Size = new System.Drawing.Size(290, 42);
         this.DeviceVersionValuePanel.TabIndex = 173;
         this.DeviceVersionValuePanel.ValueText = "v1.0";
         this.DeviceVersionValuePanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label2
         // 
         this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label2.Location = new System.Drawing.Point(325, 56);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(290, 20);
         this.label2.TabIndex = 172;
         this.label2.Text = "DEVICE VERSION";
         this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // DeviceNameValuePanel
         // 
         this.DeviceNameValuePanel.BackColor = System.Drawing.Color.Black;
         this.DeviceNameValuePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.DeviceNameValuePanel.Enabled = false;
         this.DeviceNameValuePanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DeviceNameValuePanel.ForeColor = System.Drawing.Color.White;
         this.DeviceNameValuePanel.HoldTimeoutEnable = false;
         this.DeviceNameValuePanel.HoldTimeoutInterval = 100;
         this.DeviceNameValuePanel.Location = new System.Drawing.Point(27, 79);
         this.DeviceNameValuePanel.Name = "DeviceNameValuePanel";
         this.DeviceNameValuePanel.Size = new System.Drawing.Size(290, 42);
         this.DeviceNameValuePanel.TabIndex = 171;
         this.DeviceNameValuePanel.ValueText = "Whistle";
         this.DeviceNameValuePanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label28
         // 
         this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label28.Location = new System.Drawing.Point(27, 56);
         this.label28.Name = "label28";
         this.label28.Size = new System.Drawing.Size(290, 20);
         this.label28.TabIndex = 170;
         this.label28.Text = "DEVICE NAME";
         this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // TitleLabel
         // 
         this.TitleLabel.BackColor = System.Drawing.Color.Teal;
         this.TitleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TitleLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.TitleLabel.Location = new System.Drawing.Point(16, 16);
         this.TitleLabel.Name = "TitleLabel";
         this.TitleLabel.Size = new System.Drawing.Size(610, 36);
         this.TitleLabel.TabIndex = 169;
         this.TitleLabel.Text = "TITLE";
         this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // BackButton
         // 
         this.BackButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BackButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BackButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.BackButton.Location = new System.Drawing.Point(493, 335);
         this.BackButton.Name = "BackButton";
         this.BackButton.Size = new System.Drawing.Size(107, 67);
         this.BackButton.TabIndex = 167;
         this.BackButton.Text = "BACK";
         this.BackButton.UseVisualStyleBackColor = false;
         this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
         // 
         // CANDeviceInformationForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.ClientSize = new System.Drawing.Size(658, 436);
         this.Controls.Add(this.MainPanel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "CANDeviceInformationForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "CANDeviceInformationForm";
         this.Shown += new System.EventHandler(this.CANDeviceInformationForm_Shown);
         this.MainPanel.ResumeLayout(false);
         this.TraceSelectPanel.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private BorderedPanel MainPanel;
      private NicBotButton BackButton;
      private System.Windows.Forms.Label TitleLabel;
      private TextPanel LoggingNameValuePanel;
      private System.Windows.Forms.Label label3;
      private TextPanel DeviceVersionValuePanel;
      private System.Windows.Forms.Label label2;
      private TextPanel DeviceNameValuePanel;
      private System.Windows.Forms.Label label28;
      private NicBotButton SdoButton;
      private TextPanel NodeIdValuePanel;
      private System.Windows.Forms.Label label4;
      private BorderedPanel TraceSelectPanel;
      private System.Windows.Forms.Label label5;
      private NicBotButton Tpdo3Button;
      private NicBotButton Rpdo3Button;
      private NicBotButton Tpdo2Button;
      private NicBotButton Rpdo2Button;
      private NicBotButton Tpdo1Button;
      private NicBotButton Rpdo1Button;
      private NicBotButton Tpdo4Button;
      private NicBotButton Rpdo4Button;
      private NicBotButton HbButton;
      private TextPanel DeviceTypeValuePanel;
      private System.Windows.Forms.Label label1;
      private NicBotButton RestartButton;
   }
}