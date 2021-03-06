﻿namespace DYNO.GUI
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
         this.ActivityButton = new System.Windows.Forms.Button();
         this.MainStatusStrip = new System.Windows.Forms.StatusStrip();
         this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
         this.TimeStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
         this.CloseButton = new System.Windows.Forms.Button();
         this.WheelSpeedTextBox = new System.Windows.Forms.TextBox();
         this.label123 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.label4 = new System.Windows.Forms.Label();
         this.label5 = new System.Windows.Forms.Label();
         this.label6 = new System.Windows.Forms.Label();
         this.WheelStartLoadTextBox = new System.Windows.Forms.TextBox();
         this.WheelStopLoadTextBox = new System.Windows.Forms.TextBox();
         this.CurrentLimitTextBox = new System.Windows.Forms.TextBox();
         this.RunTimeTextBox = new System.Windows.Forms.TextBox();
         this.label7 = new System.Windows.Forms.Label();
         this.label8 = new System.Windows.Forms.Label();
         this.ThermalLimitTextBox = new System.Windows.Forms.TextBox();
         this.SlippageLimitTextBox = new System.Windows.Forms.TextBox();
         this.label9 = new System.Windows.Forms.Label();
         this.label10 = new System.Windows.Forms.Label();
         this.label11 = new System.Windows.Forms.Label();
         this.label12 = new System.Windows.Forms.Label();
         this.label13 = new System.Windows.Forms.Label();
         this.MainTabControl = new System.Windows.Forms.TabControl();
         this.TestTabPage = new System.Windows.Forms.TabPage();
         this.SaveParametersButton = new System.Windows.Forms.Button();
         this.LoadParametersButton = new System.Windows.Forms.Button();
         this.SetupTabPage = new System.Windows.Forms.TabPage();
         this.ProducerHeartbeatTimeTextBox = new System.Windows.Forms.TextBox();
         this.label106 = new System.Windows.Forms.Label();
         this.ConsumerHeartbeatTimeTextBox = new System.Windows.Forms.TextBox();
         this.ConsumerHeartbeatNodeIdTextBox = new System.Windows.Forms.TextBox();
         this.label105 = new System.Windows.Forms.Label();
         this.AnalogIoVoltsToLoadPoundsTextBox = new System.Windows.Forms.TextBox();
         this.AnalogIoVoltsToSupplyAmpsSlopeTextBox = new System.Windows.Forms.TextBox();
         this.BodyRpmToSpeedTextBox = new System.Windows.Forms.TextBox();
         this.UutRpmToSpeedTextBox = new System.Windows.Forms.TextBox();
         this.label26 = new System.Windows.Forms.Label();
         this.label25 = new System.Windows.Forms.Label();
         this.label24 = new System.Windows.Forms.Label();
         this.label23 = new System.Windows.Forms.Label();
         this.DigitalIoIdTextBox = new System.Windows.Forms.TextBox();
         this.AnalogIoIdTextBox = new System.Windows.Forms.TextBox();
         this.EncoderIdTextBox = new System.Windows.Forms.TextBox();
         this.label18 = new System.Windows.Forms.Label();
         this.label17 = new System.Windows.Forms.Label();
         this.label16 = new System.Windows.Forms.Label();
         this.UutIdTextBox = new System.Windows.Forms.TextBox();
         this.label15 = new System.Windows.Forms.Label();
         this.label14 = new System.Windows.Forms.Label();
         this.BusInterfaceComboBox = new System.Windows.Forms.ComboBox();
         this.BaudComboBox = new System.Windows.Forms.ComboBox();
         this.TraceActivityTabPage = new System.Windows.Forms.TabPage();
         this.label21 = new System.Windows.Forms.Label();
         this.label22 = new System.Windows.Forms.Label();
         this.DeviceTraceComboBox = new System.Windows.Forms.ComboBox();
         this.LogTraceComboBox = new System.Windows.Forms.ComboBox();
         this.CanTraceLabel = new System.Windows.Forms.Label();
         this.label19 = new System.Windows.Forms.Label();
         this.TestTraceComboBox = new System.Windows.Forms.ComboBox();
         this.label20 = new System.Windows.Forms.Label();
         this.TraceMaskTextBox = new System.Windows.Forms.TextBox();
         this.CanTraceComboBox = new System.Windows.Forms.ComboBox();
         this.ClearActivityLogButton = new System.Windows.Forms.Button();
         this.MainControlSplitter = new System.Windows.Forms.Splitter();
         this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
         this.ActivityControlPanel = new System.Windows.Forms.Panel();
         this.AutoScrollCheckBox = new System.Windows.Forms.CheckBox();
         this.MainActivityPanel = new System.Windows.Forms.Panel();
         this.ActivityRichTextBox = new System.Windows.Forms.RichTextBox();
         this.ActivityTitleRichTextBox = new System.Windows.Forms.RichTextBox();
         this.AnalogIoVoltsToSupplyAmpsOffsetTextBox = new System.Windows.Forms.TextBox();
         this.MainStatusStrip.SuspendLayout();
         this.MainTabControl.SuspendLayout();
         this.TestTabPage.SuspendLayout();
         this.SetupTabPage.SuspendLayout();
         this.TraceActivityTabPage.SuspendLayout();
         this.ActivityControlPanel.SuspendLayout();
         this.MainActivityPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // ActivityButton
         // 
         this.ActivityButton.Location = new System.Drawing.Point(68, 1);
         this.ActivityButton.Name = "ActivityButton";
         this.ActivityButton.Size = new System.Drawing.Size(59, 23);
         this.ActivityButton.TabIndex = 12;
         this.ActivityButton.Text = "Activity";
         this.ActivityButton.UseVisualStyleBackColor = true;
         this.ActivityButton.Click += new System.EventHandler(this.ActivityButton_Click);
         // 
         // MainStatusStrip
         // 
         this.MainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel,
            this.TimeStatusLabel});
         this.MainStatusStrip.Location = new System.Drawing.Point(0, 326);
         this.MainStatusStrip.Name = "MainStatusStrip";
         this.MainStatusStrip.Size = new System.Drawing.Size(663, 22);
         this.MainStatusStrip.TabIndex = 15;
         this.MainStatusStrip.Text = "statusStrip1";
         // 
         // StatusLabel
         // 
         this.StatusLabel.AutoSize = false;
         this.StatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
         this.StatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
         this.StatusLabel.Name = "StatusLabel";
         this.StatusLabel.Size = new System.Drawing.Size(539, 17);
         this.StatusLabel.Spring = true;
         this.StatusLabel.Text = "Status";
         this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // TimeStatusLabel
         // 
         this.TimeStatusLabel.AutoSize = false;
         this.TimeStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
         this.TimeStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
         this.TimeStatusLabel.Name = "TimeStatusLabel";
         this.TimeStatusLabel.Size = new System.Drawing.Size(109, 17);
         this.TimeStatusLabel.Text = "00:00:00.000";
         // 
         // CloseButton
         // 
         this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.CloseButton.Location = new System.Drawing.Point(588, 92);
         this.CloseButton.Name = "CloseButton";
         this.CloseButton.Size = new System.Drawing.Size(59, 23);
         this.CloseButton.TabIndex = 16;
         this.CloseButton.Text = "Close";
         this.CloseButton.UseVisualStyleBackColor = true;
         this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
         // 
         // WheelSpeedTextBox
         // 
         this.WheelSpeedTextBox.Location = new System.Drawing.Point(102, 35);
         this.WheelSpeedTextBox.Name = "WheelSpeedTextBox";
         this.WheelSpeedTextBox.Size = new System.Drawing.Size(62, 20);
         this.WheelSpeedTextBox.TabIndex = 168;
         this.WheelSpeedTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // label123
         // 
         this.label123.AutoSize = true;
         this.label123.Location = new System.Drawing.Point(28, 38);
         this.label123.Name = "label123";
         this.label123.Size = new System.Drawing.Size(72, 13);
         this.label123.TabIndex = 167;
         this.label123.Text = "Wheel Speed";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(10, 64);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(90, 13);
         this.label1.TabIndex = 169;
         this.label1.Text = "Wheel Start Load";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(10, 90);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(90, 13);
         this.label2.TabIndex = 170;
         this.label2.Text = "Wheel Stop Load";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(227, 12);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(65, 13);
         this.label3.TabIndex = 171;
         this.label3.Text = "Current Limit";
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(223, 38);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(69, 13);
         this.label4.TabIndex = 172;
         this.label4.Text = "Thermal Limit";
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(47, 12);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(53, 13);
         this.label5.TabIndex = 173;
         this.label5.Text = "Run Time";
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(220, 64);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(72, 13);
         this.label6.TabIndex = 174;
         this.label6.Text = "Slippage Limit";
         // 
         // WheelStartLoadTextBox
         // 
         this.WheelStartLoadTextBox.Location = new System.Drawing.Point(102, 61);
         this.WheelStartLoadTextBox.Name = "WheelStartLoadTextBox";
         this.WheelStartLoadTextBox.Size = new System.Drawing.Size(62, 20);
         this.WheelStartLoadTextBox.TabIndex = 175;
         this.WheelStartLoadTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // WheelStopLoadTextBox
         // 
         this.WheelStopLoadTextBox.Location = new System.Drawing.Point(102, 87);
         this.WheelStopLoadTextBox.Name = "WheelStopLoadTextBox";
         this.WheelStopLoadTextBox.Size = new System.Drawing.Size(62, 20);
         this.WheelStopLoadTextBox.TabIndex = 176;
         this.WheelStopLoadTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // CurrentLimitTextBox
         // 
         this.CurrentLimitTextBox.Location = new System.Drawing.Point(294, 9);
         this.CurrentLimitTextBox.Name = "CurrentLimitTextBox";
         this.CurrentLimitTextBox.Size = new System.Drawing.Size(62, 20);
         this.CurrentLimitTextBox.TabIndex = 177;
         this.CurrentLimitTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // RunTimeTextBox
         // 
         this.RunTimeTextBox.Location = new System.Drawing.Point(102, 9);
         this.RunTimeTextBox.Name = "RunTimeTextBox";
         this.RunTimeTextBox.Size = new System.Drawing.Size(62, 20);
         this.RunTimeTextBox.TabIndex = 178;
         this.RunTimeTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(166, 12);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(53, 13);
         this.label7.TabIndex = 179;
         this.label7.Text = "(hr/min/s)";
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.Location = new System.Drawing.Point(358, 64);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(21, 13);
         this.label8.TabIndex = 180;
         this.label8.Text = "(%)";
         this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // ThermalLimitTextBox
         // 
         this.ThermalLimitTextBox.Location = new System.Drawing.Point(294, 35);
         this.ThermalLimitTextBox.Name = "ThermalLimitTextBox";
         this.ThermalLimitTextBox.Size = new System.Drawing.Size(62, 20);
         this.ThermalLimitTextBox.TabIndex = 181;
         this.ThermalLimitTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // SlippageLimitTextBox
         // 
         this.SlippageLimitTextBox.Location = new System.Drawing.Point(294, 61);
         this.SlippageLimitTextBox.Name = "SlippageLimitTextBox";
         this.SlippageLimitTextBox.Size = new System.Drawing.Size(62, 20);
         this.SlippageLimitTextBox.TabIndex = 182;
         this.SlippageLimitTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // label9
         // 
         this.label9.AutoSize = true;
         this.label9.Location = new System.Drawing.Point(358, 38);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(24, 13);
         this.label9.TabIndex = 183;
         this.label9.Text = "(°C)";
         this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // label10
         // 
         this.label10.AutoSize = true;
         this.label10.Location = new System.Drawing.Point(358, 12);
         this.label10.Name = "label10";
         this.label10.Size = new System.Drawing.Size(20, 13);
         this.label10.TabIndex = 184;
         this.label10.Text = "(A)";
         this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // label11
         // 
         this.label11.AutoSize = true;
         this.label11.Location = new System.Drawing.Point(166, 90);
         this.label11.Name = "label11";
         this.label11.Size = new System.Drawing.Size(24, 13);
         this.label11.TabIndex = 185;
         this.label11.Text = "(lbf)";
         this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // label12
         // 
         this.label12.AutoSize = true;
         this.label12.Location = new System.Drawing.Point(166, 64);
         this.label12.Name = "label12";
         this.label12.Size = new System.Drawing.Size(24, 13);
         this.label12.TabIndex = 186;
         this.label12.Text = "(lbf)";
         this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // label13
         // 
         this.label13.AutoSize = true;
         this.label13.Location = new System.Drawing.Point(166, 38);
         this.label13.Name = "label13";
         this.label13.Size = new System.Drawing.Size(31, 13);
         this.label13.TabIndex = 187;
         this.label13.Text = "(in/s)";
         this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // MainTabControl
         // 
         this.MainTabControl.Controls.Add(this.TestTabPage);
         this.MainTabControl.Controls.Add(this.SetupTabPage);
         this.MainTabControl.Controls.Add(this.TraceActivityTabPage);
         this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.MainTabControl.Location = new System.Drawing.Point(0, 179);
         this.MainTabControl.MinimumSize = new System.Drawing.Size(612, 147);
         this.MainTabControl.Name = "MainTabControl";
         this.MainTabControl.SelectedIndex = 0;
         this.MainTabControl.Size = new System.Drawing.Size(663, 147);
         this.MainTabControl.TabIndex = 188;
         // 
         // TestTabPage
         // 
         this.TestTabPage.Controls.Add(this.SaveParametersButton);
         this.TestTabPage.Controls.Add(this.LoadParametersButton);
         this.TestTabPage.Controls.Add(this.CloseButton);
         this.TestTabPage.Controls.Add(this.SlippageLimitTextBox);
         this.TestTabPage.Controls.Add(this.ThermalLimitTextBox);
         this.TestTabPage.Controls.Add(this.RunTimeTextBox);
         this.TestTabPage.Controls.Add(this.CurrentLimitTextBox);
         this.TestTabPage.Controls.Add(this.label123);
         this.TestTabPage.Controls.Add(this.WheelStopLoadTextBox);
         this.TestTabPage.Controls.Add(this.label1);
         this.TestTabPage.Controls.Add(this.WheelStartLoadTextBox);
         this.TestTabPage.Controls.Add(this.label2);
         this.TestTabPage.Controls.Add(this.WheelSpeedTextBox);
         this.TestTabPage.Controls.Add(this.label3);
         this.TestTabPage.Controls.Add(this.label13);
         this.TestTabPage.Controls.Add(this.label4);
         this.TestTabPage.Controls.Add(this.label12);
         this.TestTabPage.Controls.Add(this.label5);
         this.TestTabPage.Controls.Add(this.label11);
         this.TestTabPage.Controls.Add(this.label6);
         this.TestTabPage.Controls.Add(this.label10);
         this.TestTabPage.Controls.Add(this.label7);
         this.TestTabPage.Controls.Add(this.label9);
         this.TestTabPage.Controls.Add(this.label8);
         this.TestTabPage.Location = new System.Drawing.Point(4, 22);
         this.TestTabPage.Name = "TestTabPage";
         this.TestTabPage.Padding = new System.Windows.Forms.Padding(3);
         this.TestTabPage.Size = new System.Drawing.Size(655, 121);
         this.TestTabPage.TabIndex = 0;
         this.TestTabPage.Text = "Test";
         this.TestTabPage.UseVisualStyleBackColor = true;
         // 
         // SaveParametersButton
         // 
         this.SaveParametersButton.Location = new System.Drawing.Point(308, 92);
         this.SaveParametersButton.Name = "SaveParametersButton";
         this.SaveParametersButton.Size = new System.Drawing.Size(59, 23);
         this.SaveParametersButton.TabIndex = 189;
         this.SaveParametersButton.Text = "Save";
         this.SaveParametersButton.UseVisualStyleBackColor = true;
         this.SaveParametersButton.Click += new System.EventHandler(this.SaveParametersButton_Click);
         // 
         // LoadParametersButton
         // 
         this.LoadParametersButton.Location = new System.Drawing.Point(243, 92);
         this.LoadParametersButton.Name = "LoadParametersButton";
         this.LoadParametersButton.Size = new System.Drawing.Size(59, 23);
         this.LoadParametersButton.TabIndex = 188;
         this.LoadParametersButton.Text = "Load";
         this.LoadParametersButton.UseVisualStyleBackColor = true;
         this.LoadParametersButton.Click += new System.EventHandler(this.LoadParametersButton_Click);
         // 
         // SetupTabPage
         // 
         this.SetupTabPage.Controls.Add(this.AnalogIoVoltsToSupplyAmpsOffsetTextBox);
         this.SetupTabPage.Controls.Add(this.ProducerHeartbeatTimeTextBox);
         this.SetupTabPage.Controls.Add(this.label106);
         this.SetupTabPage.Controls.Add(this.ConsumerHeartbeatTimeTextBox);
         this.SetupTabPage.Controls.Add(this.ConsumerHeartbeatNodeIdTextBox);
         this.SetupTabPage.Controls.Add(this.label105);
         this.SetupTabPage.Controls.Add(this.AnalogIoVoltsToLoadPoundsTextBox);
         this.SetupTabPage.Controls.Add(this.AnalogIoVoltsToSupplyAmpsSlopeTextBox);
         this.SetupTabPage.Controls.Add(this.BodyRpmToSpeedTextBox);
         this.SetupTabPage.Controls.Add(this.UutRpmToSpeedTextBox);
         this.SetupTabPage.Controls.Add(this.label26);
         this.SetupTabPage.Controls.Add(this.label25);
         this.SetupTabPage.Controls.Add(this.label24);
         this.SetupTabPage.Controls.Add(this.label23);
         this.SetupTabPage.Controls.Add(this.DigitalIoIdTextBox);
         this.SetupTabPage.Controls.Add(this.AnalogIoIdTextBox);
         this.SetupTabPage.Controls.Add(this.EncoderIdTextBox);
         this.SetupTabPage.Controls.Add(this.label18);
         this.SetupTabPage.Controls.Add(this.label17);
         this.SetupTabPage.Controls.Add(this.label16);
         this.SetupTabPage.Controls.Add(this.UutIdTextBox);
         this.SetupTabPage.Controls.Add(this.label15);
         this.SetupTabPage.Controls.Add(this.label14);
         this.SetupTabPage.Controls.Add(this.BusInterfaceComboBox);
         this.SetupTabPage.Controls.Add(this.BaudComboBox);
         this.SetupTabPage.Location = new System.Drawing.Point(4, 22);
         this.SetupTabPage.Name = "SetupTabPage";
         this.SetupTabPage.Padding = new System.Windows.Forms.Padding(3);
         this.SetupTabPage.Size = new System.Drawing.Size(655, 121);
         this.SetupTabPage.TabIndex = 1;
         this.SetupTabPage.Text = "Setup";
         this.SetupTabPage.UseVisualStyleBackColor = true;
         // 
         // ProducerHeartbeatTimeTextBox
         // 
         this.ProducerHeartbeatTimeTextBox.Location = new System.Drawing.Point(84, 84);
         this.ProducerHeartbeatTimeTextBox.Name = "ProducerHeartbeatTimeTextBox";
         this.ProducerHeartbeatTimeTextBox.Size = new System.Drawing.Size(39, 20);
         this.ProducerHeartbeatTimeTextBox.TabIndex = 199;
         this.ProducerHeartbeatTimeTextBox.Text = "1000";
         this.ProducerHeartbeatTimeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.ProducerHeartbeatTimeTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // label106
         // 
         this.label106.AutoSize = true;
         this.label106.Location = new System.Drawing.Point(14, 87);
         this.label106.Name = "label106";
         this.label106.Size = new System.Drawing.Size(68, 13);
         this.label106.TabIndex = 198;
         this.label106.Text = "Producer HB";
         // 
         // ConsumerHeartbeatTimeTextBox
         // 
         this.ConsumerHeartbeatTimeTextBox.Location = new System.Drawing.Point(113, 60);
         this.ConsumerHeartbeatTimeTextBox.Name = "ConsumerHeartbeatTimeTextBox";
         this.ConsumerHeartbeatTimeTextBox.Size = new System.Drawing.Size(39, 20);
         this.ConsumerHeartbeatTimeTextBox.TabIndex = 197;
         this.ConsumerHeartbeatTimeTextBox.Text = "3000";
         this.ConsumerHeartbeatTimeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.ConsumerHeartbeatTimeTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // ConsumerHeartbeatNodeIdTextBox
         // 
         this.ConsumerHeartbeatNodeIdTextBox.Location = new System.Drawing.Point(84, 60);
         this.ConsumerHeartbeatNodeIdTextBox.Name = "ConsumerHeartbeatNodeIdTextBox";
         this.ConsumerHeartbeatNodeIdTextBox.Size = new System.Drawing.Size(23, 20);
         this.ConsumerHeartbeatNodeIdTextBox.TabIndex = 196;
         this.ConsumerHeartbeatNodeIdTextBox.Text = "80";
         this.ConsumerHeartbeatNodeIdTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.ConsumerHeartbeatNodeIdTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // label105
         // 
         this.label105.AutoSize = true;
         this.label105.Location = new System.Drawing.Point(10, 63);
         this.label105.Name = "label105";
         this.label105.Size = new System.Drawing.Size(72, 13);
         this.label105.TabIndex = 195;
         this.label105.Text = "Consumer HB";
         // 
         // AnalogIoVoltsToLoadPoundsTextBox
         // 
         this.AnalogIoVoltsToLoadPoundsTextBox.Location = new System.Drawing.Point(502, 58);
         this.AnalogIoVoltsToLoadPoundsTextBox.Name = "AnalogIoVoltsToLoadPoundsTextBox";
         this.AnalogIoVoltsToLoadPoundsTextBox.Size = new System.Drawing.Size(62, 20);
         this.AnalogIoVoltsToLoadPoundsTextBox.TabIndex = 194;
         this.AnalogIoVoltsToLoadPoundsTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // AnalogIoVoltsToSupplyAmpsSlopeTextBox
         // 
         this.AnalogIoVoltsToSupplyAmpsSlopeTextBox.Location = new System.Drawing.Point(502, 84);
         this.AnalogIoVoltsToSupplyAmpsSlopeTextBox.Name = "AnalogIoVoltsToSupplyAmpsSlopeTextBox";
         this.AnalogIoVoltsToSupplyAmpsSlopeTextBox.Size = new System.Drawing.Size(62, 20);
         this.AnalogIoVoltsToSupplyAmpsSlopeTextBox.TabIndex = 193;
         this.AnalogIoVoltsToSupplyAmpsSlopeTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // BodyRpmToSpeedTextBox
         // 
         this.BodyRpmToSpeedTextBox.Location = new System.Drawing.Point(502, 32);
         this.BodyRpmToSpeedTextBox.Name = "BodyRpmToSpeedTextBox";
         this.BodyRpmToSpeedTextBox.Size = new System.Drawing.Size(62, 20);
         this.BodyRpmToSpeedTextBox.TabIndex = 192;
         this.BodyRpmToSpeedTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // UutRpmToSpeedTextBox
         // 
         this.UutRpmToSpeedTextBox.Location = new System.Drawing.Point(502, 6);
         this.UutRpmToSpeedTextBox.Name = "UutRpmToSpeedTextBox";
         this.UutRpmToSpeedTextBox.Size = new System.Drawing.Size(62, 20);
         this.UutRpmToSpeedTextBox.TabIndex = 188;
         this.UutRpmToSpeedTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // label26
         // 
         this.label26.AutoSize = true;
         this.label26.Location = new System.Drawing.Point(346, 87);
         this.label26.Name = "label26";
         this.label26.Size = new System.Drawing.Size(154, 13);
         this.label26.TabIndex = 191;
         this.label26.Text = "AnalogIO Volts per Supply Amp";
         // 
         // label25
         // 
         this.label25.AutoSize = true;
         this.label25.Location = new System.Drawing.Point(385, 35);
         this.label25.Name = "label25";
         this.label25.Size = new System.Drawing.Size(115, 13);
         this.label25.TabIndex = 190;
         this.label25.Text = "Body RPM to inch/sec";
         // 
         // label24
         // 
         this.label24.AutoSize = true;
         this.label24.Location = new System.Drawing.Point(386, 9);
         this.label24.Name = "label24";
         this.label24.Size = new System.Drawing.Size(114, 13);
         this.label24.TabIndex = 189;
         this.label24.Text = "UUT RPM to inch/sec";
         // 
         // label23
         // 
         this.label23.AutoSize = true;
         this.label23.Location = new System.Drawing.Point(370, 61);
         this.label23.Name = "label23";
         this.label23.Size = new System.Drawing.Size(130, 13);
         this.label23.TabIndex = 187;
         this.label23.Text = "AnalogIO Volts Per Pound";
         // 
         // DigitalIoIdTextBox
         // 
         this.DigitalIoIdTextBox.Location = new System.Drawing.Point(253, 84);
         this.DigitalIoIdTextBox.Name = "DigitalIoIdTextBox";
         this.DigitalIoIdTextBox.Size = new System.Drawing.Size(62, 20);
         this.DigitalIoIdTextBox.TabIndex = 186;
         this.DigitalIoIdTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // AnalogIoIdTextBox
         // 
         this.AnalogIoIdTextBox.Location = new System.Drawing.Point(253, 58);
         this.AnalogIoIdTextBox.Name = "AnalogIoIdTextBox";
         this.AnalogIoIdTextBox.Size = new System.Drawing.Size(62, 20);
         this.AnalogIoIdTextBox.TabIndex = 185;
         this.AnalogIoIdTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // EncoderIdTextBox
         // 
         this.EncoderIdTextBox.Location = new System.Drawing.Point(253, 32);
         this.EncoderIdTextBox.Name = "EncoderIdTextBox";
         this.EncoderIdTextBox.Size = new System.Drawing.Size(62, 20);
         this.EncoderIdTextBox.TabIndex = 184;
         this.EncoderIdTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // label18
         // 
         this.label18.AutoSize = true;
         this.label18.Location = new System.Drawing.Point(190, 87);
         this.label18.Name = "label18";
         this.label18.Size = new System.Drawing.Size(61, 13);
         this.label18.TabIndex = 183;
         this.label18.Text = "DigitalIO ID";
         // 
         // label17
         // 
         this.label17.AutoSize = true;
         this.label17.Location = new System.Drawing.Point(190, 35);
         this.label17.Name = "label17";
         this.label17.Size = new System.Drawing.Size(61, 13);
         this.label17.TabIndex = 182;
         this.label17.Text = "Encoder ID";
         // 
         // label16
         // 
         this.label16.AutoSize = true;
         this.label16.Location = new System.Drawing.Point(186, 61);
         this.label16.Name = "label16";
         this.label16.Size = new System.Drawing.Size(65, 13);
         this.label16.TabIndex = 181;
         this.label16.Text = "AnalogIO ID";
         // 
         // UutIdTextBox
         // 
         this.UutIdTextBox.Location = new System.Drawing.Point(253, 6);
         this.UutIdTextBox.Name = "UutIdTextBox";
         this.UutIdTextBox.Size = new System.Drawing.Size(62, 20);
         this.UutIdTextBox.TabIndex = 180;
         this.UutIdTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // label15
         // 
         this.label15.AutoSize = true;
         this.label15.Location = new System.Drawing.Point(207, 9);
         this.label15.Name = "label15";
         this.label15.Size = new System.Drawing.Size(44, 13);
         this.label15.TabIndex = 179;
         this.label15.Text = "UUT ID";
         // 
         // label14
         // 
         this.label14.AutoSize = true;
         this.label14.Location = new System.Drawing.Point(12, 9);
         this.label14.Name = "label14";
         this.label14.Size = new System.Drawing.Size(29, 13);
         this.label14.TabIndex = 174;
         this.label14.Text = "BUS";
         // 
         // BusInterfaceComboBox
         // 
         this.BusInterfaceComboBox.FormattingEnabled = true;
         this.BusInterfaceComboBox.Location = new System.Drawing.Point(113, 6);
         this.BusInterfaceComboBox.Name = "BusInterfaceComboBox";
         this.BusInterfaceComboBox.Size = new System.Drawing.Size(57, 21);
         this.BusInterfaceComboBox.TabIndex = 16;
         this.BusInterfaceComboBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // BaudComboBox
         // 
         this.BaudComboBox.FormattingEnabled = true;
         this.BaudComboBox.Location = new System.Drawing.Point(43, 6);
         this.BaudComboBox.Name = "BaudComboBox";
         this.BaudComboBox.Size = new System.Drawing.Size(64, 21);
         this.BaudComboBox.TabIndex = 15;
         this.BaudComboBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // TraceActivityTabPage
         // 
         this.TraceActivityTabPage.Controls.Add(this.label21);
         this.TraceActivityTabPage.Controls.Add(this.label22);
         this.TraceActivityTabPage.Controls.Add(this.DeviceTraceComboBox);
         this.TraceActivityTabPage.Controls.Add(this.LogTraceComboBox);
         this.TraceActivityTabPage.Controls.Add(this.CanTraceLabel);
         this.TraceActivityTabPage.Controls.Add(this.label19);
         this.TraceActivityTabPage.Controls.Add(this.TestTraceComboBox);
         this.TraceActivityTabPage.Controls.Add(this.label20);
         this.TraceActivityTabPage.Controls.Add(this.TraceMaskTextBox);
         this.TraceActivityTabPage.Controls.Add(this.CanTraceComboBox);
         this.TraceActivityTabPage.Location = new System.Drawing.Point(4, 22);
         this.TraceActivityTabPage.Name = "TraceActivityTabPage";
         this.TraceActivityTabPage.Padding = new System.Windows.Forms.Padding(3);
         this.TraceActivityTabPage.Size = new System.Drawing.Size(604, 121);
         this.TraceActivityTabPage.TabIndex = 2;
         this.TraceActivityTabPage.Text = "Activity";
         this.TraceActivityTabPage.UseVisualStyleBackColor = true;
         // 
         // label21
         // 
         this.label21.AutoSize = true;
         this.label21.Location = new System.Drawing.Point(12, 46);
         this.label21.Name = "label21";
         this.label21.Size = new System.Drawing.Size(29, 13);
         this.label21.TabIndex = 101;
         this.label21.Text = "LOG";
         // 
         // label22
         // 
         this.label22.AutoSize = true;
         this.label22.Location = new System.Drawing.Point(121, 46);
         this.label22.Name = "label22";
         this.label22.Size = new System.Drawing.Size(46, 13);
         this.label22.TabIndex = 100;
         this.label22.Text = "DEVICE";
         this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // DeviceTraceComboBox
         // 
         this.DeviceTraceComboBox.FormattingEnabled = true;
         this.DeviceTraceComboBox.Items.AddRange(new object[] {
            "error",
            "high",
            "medium",
            "low"});
         this.DeviceTraceComboBox.Location = new System.Drawing.Point(169, 43);
         this.DeviceTraceComboBox.Name = "DeviceTraceComboBox";
         this.DeviceTraceComboBox.Size = new System.Drawing.Size(75, 21);
         this.DeviceTraceComboBox.TabIndex = 99;
         this.DeviceTraceComboBox.SelectedIndexChanged += new System.EventHandler(this.DeviceTraceComboBox_SelectedIndexChanged);
         // 
         // LogTraceComboBox
         // 
         this.LogTraceComboBox.FormattingEnabled = true;
         this.LogTraceComboBox.Items.AddRange(new object[] {
            "error",
            "high",
            "medium",
            "low"});
         this.LogTraceComboBox.Location = new System.Drawing.Point(43, 43);
         this.LogTraceComboBox.Name = "LogTraceComboBox";
         this.LogTraceComboBox.Size = new System.Drawing.Size(75, 21);
         this.LogTraceComboBox.TabIndex = 98;
         this.LogTraceComboBox.SelectedIndexChanged += new System.EventHandler(this.LogTraceComboBox_SelectedIndexChanged);
         // 
         // CanTraceLabel
         // 
         this.CanTraceLabel.AutoSize = true;
         this.CanTraceLabel.Location = new System.Drawing.Point(12, 19);
         this.CanTraceLabel.Name = "CanTraceLabel";
         this.CanTraceLabel.Size = new System.Drawing.Size(29, 13);
         this.CanTraceLabel.TabIndex = 97;
         this.CanTraceLabel.Text = "CAN";
         // 
         // label19
         // 
         this.label19.AutoSize = true;
         this.label19.Location = new System.Drawing.Point(132, 19);
         this.label19.Name = "label19";
         this.label19.Size = new System.Drawing.Size(35, 13);
         this.label19.TabIndex = 96;
         this.label19.Text = "TEST";
         this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // TestTraceComboBox
         // 
         this.TestTraceComboBox.FormattingEnabled = true;
         this.TestTraceComboBox.Items.AddRange(new object[] {
            "error",
            "high",
            "medium",
            "low"});
         this.TestTraceComboBox.Location = new System.Drawing.Point(169, 16);
         this.TestTraceComboBox.Name = "TestTraceComboBox";
         this.TestTraceComboBox.Size = new System.Drawing.Size(75, 21);
         this.TestTraceComboBox.TabIndex = 95;
         this.TestTraceComboBox.SelectedIndexChanged += new System.EventHandler(this.TestTraceComboBox_SelectedIndexChanged);
         // 
         // label20
         // 
         this.label20.AutoSize = true;
         this.label20.Location = new System.Drawing.Point(266, 19);
         this.label20.Name = "label20";
         this.label20.Size = new System.Drawing.Size(33, 13);
         this.label20.TabIndex = 93;
         this.label20.Text = "Mask";
         // 
         // TraceMaskTextBox
         // 
         this.TraceMaskTextBox.Location = new System.Drawing.Point(301, 16);
         this.TraceMaskTextBox.Name = "TraceMaskTextBox";
         this.TraceMaskTextBox.Size = new System.Drawing.Size(58, 20);
         this.TraceMaskTextBox.TabIndex = 94;
         this.TraceMaskTextBox.TextChanged += new System.EventHandler(this.TraceMaskTextBox_TextChanged);
         // 
         // CanTraceComboBox
         // 
         this.CanTraceComboBox.FormattingEnabled = true;
         this.CanTraceComboBox.Items.AddRange(new object[] {
            "error",
            "high",
            "medium",
            "low"});
         this.CanTraceComboBox.Location = new System.Drawing.Point(43, 16);
         this.CanTraceComboBox.Name = "CanTraceComboBox";
         this.CanTraceComboBox.Size = new System.Drawing.Size(75, 21);
         this.CanTraceComboBox.TabIndex = 92;
         this.CanTraceComboBox.SelectedIndexChanged += new System.EventHandler(this.CanTraceComboBox_SelectedIndexChanged);
         // 
         // ClearActivityLogButton
         // 
         this.ClearActivityLogButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.ClearActivityLogButton.Location = new System.Drawing.Point(588, 1);
         this.ClearActivityLogButton.Name = "ClearActivityLogButton";
         this.ClearActivityLogButton.Size = new System.Drawing.Size(59, 23);
         this.ClearActivityLogButton.TabIndex = 190;
         this.ClearActivityLogButton.Text = "Clear";
         this.ClearActivityLogButton.UseVisualStyleBackColor = true;
         this.ClearActivityLogButton.Click += new System.EventHandler(this.ClearActivityLogButton_Click);
         // 
         // MainControlSplitter
         // 
         this.MainControlSplitter.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.MainControlSplitter.Location = new System.Drawing.Point(0, 176);
         this.MainControlSplitter.Name = "MainControlSplitter";
         this.MainControlSplitter.Size = new System.Drawing.Size(663, 3);
         this.MainControlSplitter.TabIndex = 189;
         this.MainControlSplitter.TabStop = false;
         // 
         // UpdateTimer
         // 
         this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
         // 
         // ActivityControlPanel
         // 
         this.ActivityControlPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.ActivityControlPanel.Controls.Add(this.AutoScrollCheckBox);
         this.ActivityControlPanel.Controls.Add(this.ClearActivityLogButton);
         this.ActivityControlPanel.Controls.Add(this.ActivityButton);
         this.ActivityControlPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.ActivityControlPanel.Location = new System.Drawing.Point(0, 148);
         this.ActivityControlPanel.Name = "ActivityControlPanel";
         this.ActivityControlPanel.Size = new System.Drawing.Size(663, 28);
         this.ActivityControlPanel.TabIndex = 1;
         // 
         // AutoScrollCheckBox
         // 
         this.AutoScrollCheckBox.AutoSize = true;
         this.AutoScrollCheckBox.Location = new System.Drawing.Point(10, 4);
         this.AutoScrollCheckBox.Name = "AutoScrollCheckBox";
         this.AutoScrollCheckBox.Size = new System.Drawing.Size(52, 17);
         this.AutoScrollCheckBox.TabIndex = 191;
         this.AutoScrollCheckBox.Text = "Scroll";
         this.AutoScrollCheckBox.UseVisualStyleBackColor = true;
         // 
         // MainActivityPanel
         // 
         this.MainActivityPanel.Controls.Add(this.ActivityRichTextBox);
         this.MainActivityPanel.Controls.Add(this.ActivityTitleRichTextBox);
         this.MainActivityPanel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.MainActivityPanel.Location = new System.Drawing.Point(0, 0);
         this.MainActivityPanel.Name = "MainActivityPanel";
         this.MainActivityPanel.Size = new System.Drawing.Size(663, 148);
         this.MainActivityPanel.TabIndex = 191;
         // 
         // ActivityRichTextBox
         // 
         this.ActivityRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.ActivityRichTextBox.Font = new System.Drawing.Font("Consolas", 9.75F);
         this.ActivityRichTextBox.Location = new System.Drawing.Point(0, 20);
         this.ActivityRichTextBox.Name = "ActivityRichTextBox";
         this.ActivityRichTextBox.Size = new System.Drawing.Size(663, 128);
         this.ActivityRichTextBox.TabIndex = 2;
         this.ActivityRichTextBox.Text = "";
         // 
         // ActivityTitleRichTextBox
         // 
         this.ActivityTitleRichTextBox.Dock = System.Windows.Forms.DockStyle.Top;
         this.ActivityTitleRichTextBox.Font = new System.Drawing.Font("Consolas", 9.75F);
         this.ActivityTitleRichTextBox.Location = new System.Drawing.Point(0, 0);
         this.ActivityTitleRichTextBox.Name = "ActivityTitleRichTextBox";
         this.ActivityTitleRichTextBox.Size = new System.Drawing.Size(663, 20);
         this.ActivityTitleRichTextBox.TabIndex = 1;
         this.ActivityTitleRichTextBox.Text = "";
         // 
         // AnalogIoVoltsToSupplyAmpsOffsetTextBox
         // 
         this.AnalogIoVoltsToSupplyAmpsOffsetTextBox.Location = new System.Drawing.Point(570, 84);
         this.AnalogIoVoltsToSupplyAmpsOffsetTextBox.Name = "AnalogIoVoltsToSupplyAmpsOffsetTextBox";
         this.AnalogIoVoltsToSupplyAmpsOffsetTextBox.Size = new System.Drawing.Size(62, 20);
         this.AnalogIoVoltsToSupplyAmpsOffsetTextBox.TabIndex = 200;
         this.AnalogIoVoltsToSupplyAmpsOffsetTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // MainForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(663, 348);
         this.Controls.Add(this.MainActivityPanel);
         this.Controls.Add(this.ActivityControlPanel);
         this.Controls.Add(this.MainControlSplitter);
         this.Controls.Add(this.MainTabControl);
         this.Controls.Add(this.MainStatusStrip);
         this.MinimumSize = new System.Drawing.Size(679, 386);
         this.Name = "MainForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "dynoGUI";
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
         this.Shown += new System.EventHandler(this.MainForm_Shown);
         this.MainStatusStrip.ResumeLayout(false);
         this.MainStatusStrip.PerformLayout();
         this.MainTabControl.ResumeLayout(false);
         this.TestTabPage.ResumeLayout(false);
         this.TestTabPage.PerformLayout();
         this.SetupTabPage.ResumeLayout(false);
         this.SetupTabPage.PerformLayout();
         this.TraceActivityTabPage.ResumeLayout(false);
         this.TraceActivityTabPage.PerformLayout();
         this.ActivityControlPanel.ResumeLayout(false);
         this.ActivityControlPanel.PerformLayout();
         this.MainActivityPanel.ResumeLayout(false);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Button ActivityButton;
      private System.Windows.Forms.StatusStrip MainStatusStrip;
      private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
      private System.Windows.Forms.ToolStripStatusLabel TimeStatusLabel;
      private System.Windows.Forms.Button CloseButton;
      private System.Windows.Forms.TextBox WheelSpeedTextBox;
      private System.Windows.Forms.Label label123;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.TextBox WheelStartLoadTextBox;
      private System.Windows.Forms.TextBox WheelStopLoadTextBox;
      private System.Windows.Forms.TextBox CurrentLimitTextBox;
      private System.Windows.Forms.TextBox RunTimeTextBox;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.TextBox ThermalLimitTextBox;
      private System.Windows.Forms.TextBox SlippageLimitTextBox;
      private System.Windows.Forms.Label label9;
      private System.Windows.Forms.Label label10;
      private System.Windows.Forms.Label label11;
      private System.Windows.Forms.Label label12;
      private System.Windows.Forms.Label label13;
      private System.Windows.Forms.TabControl MainTabControl;
      private System.Windows.Forms.TabPage TestTabPage;
      private System.Windows.Forms.Splitter MainControlSplitter;
      private System.Windows.Forms.Button SaveParametersButton;
      private System.Windows.Forms.Button LoadParametersButton;
      private System.Windows.Forms.Timer UpdateTimer;
      private System.Windows.Forms.TabPage SetupTabPage;
      private System.Windows.Forms.Label label18;
      private System.Windows.Forms.Label label17;
      private System.Windows.Forms.Label label16;
      private System.Windows.Forms.TextBox UutIdTextBox;
      private System.Windows.Forms.Label label15;
      private System.Windows.Forms.Label label14;
      private System.Windows.Forms.ComboBox BusInterfaceComboBox;
      private System.Windows.Forms.ComboBox BaudComboBox;
      private System.Windows.Forms.TextBox DigitalIoIdTextBox;
      private System.Windows.Forms.TextBox AnalogIoIdTextBox;
      private System.Windows.Forms.TextBox EncoderIdTextBox;
      private System.Windows.Forms.TabPage TraceActivityTabPage;
      private System.Windows.Forms.Label CanTraceLabel;
      private System.Windows.Forms.Label label19;
      private System.Windows.Forms.ComboBox TestTraceComboBox;
      private System.Windows.Forms.Label label20;
      private System.Windows.Forms.TextBox TraceMaskTextBox;
      private System.Windows.Forms.ComboBox CanTraceComboBox;
      private System.Windows.Forms.Label label21;
      private System.Windows.Forms.Label label22;
      private System.Windows.Forms.ComboBox DeviceTraceComboBox;
      private System.Windows.Forms.ComboBox LogTraceComboBox;
      private System.Windows.Forms.Button ClearActivityLogButton;
      private System.Windows.Forms.TextBox UutRpmToSpeedTextBox;
      private System.Windows.Forms.Label label23;
      private System.Windows.Forms.Label label25;
      private System.Windows.Forms.Label label24;
      private System.Windows.Forms.Label label26;
      private System.Windows.Forms.TextBox AnalogIoVoltsToLoadPoundsTextBox;
      private System.Windows.Forms.TextBox AnalogIoVoltsToSupplyAmpsSlopeTextBox;
      private System.Windows.Forms.TextBox BodyRpmToSpeedTextBox;
      private System.Windows.Forms.Panel ActivityControlPanel;
      private System.Windows.Forms.Panel MainActivityPanel;
      private System.Windows.Forms.CheckBox AutoScrollCheckBox;
      private System.Windows.Forms.RichTextBox ActivityRichTextBox;
      private System.Windows.Forms.RichTextBox ActivityTitleRichTextBox;
      private System.Windows.Forms.TextBox ProducerHeartbeatTimeTextBox;
      private System.Windows.Forms.Label label106;
      private System.Windows.Forms.TextBox ConsumerHeartbeatTimeTextBox;
      private System.Windows.Forms.TextBox ConsumerHeartbeatNodeIdTextBox;
      private System.Windows.Forms.Label label105;
      private System.Windows.Forms.TextBox AnalogIoVoltsToSupplyAmpsOffsetTextBox;
   }
}

