namespace DYNO.GUI
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
         this.ControlTabPage = new System.Windows.Forms.TabPage();
         this.SaveParametersButton = new System.Windows.Forms.Button();
         this.LoadParametersButton = new System.Windows.Forms.Button();
         this.CanBusTabPage = new System.Windows.Forms.TabPage();
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
         this.MainControlSplitter = new System.Windows.Forms.Splitter();
         this.MainActivityPanel = new System.Windows.Forms.Panel();
         this.ActivityRichTextBox = new System.Windows.Forms.RichTextBox();
         this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
         this.TraceActivityTabPage = new System.Windows.Forms.TabPage();
         this.CanTraceLabel = new System.Windows.Forms.Label();
         this.label19 = new System.Windows.Forms.Label();
         this.TestTraceComboBox = new System.Windows.Forms.ComboBox();
         this.label20 = new System.Windows.Forms.Label();
         this.TraceMaskTextBox = new System.Windows.Forms.TextBox();
         this.CanTraceComboBox = new System.Windows.Forms.ComboBox();
         this.label21 = new System.Windows.Forms.Label();
         this.label22 = new System.Windows.Forms.Label();
         this.DeviceTraceComboBox = new System.Windows.Forms.ComboBox();
         this.LogTraceComboBox = new System.Windows.Forms.ComboBox();
         this.ClearActivityLogButton = new System.Windows.Forms.Button();
         this.MainStatusStrip.SuspendLayout();
         this.MainTabControl.SuspendLayout();
         this.ControlTabPage.SuspendLayout();
         this.CanBusTabPage.SuspendLayout();
         this.MainActivityPanel.SuspendLayout();
         this.TraceActivityTabPage.SuspendLayout();
         this.SuspendLayout();
         // 
         // ActivityButton
         // 
         this.ActivityButton.Location = new System.Drawing.Point(42, 34);
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
         this.MainStatusStrip.Size = new System.Drawing.Size(612, 22);
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
         this.StatusLabel.Size = new System.Drawing.Size(597, 17);
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
         this.CloseButton.Location = new System.Drawing.Point(537, 92);
         this.CloseButton.Name = "CloseButton";
         this.CloseButton.Size = new System.Drawing.Size(59, 23);
         this.CloseButton.TabIndex = 16;
         this.CloseButton.Text = "Close";
         this.CloseButton.UseVisualStyleBackColor = true;
         this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
         // 
         // WheelSpeedTextBox
         // 
         this.WheelSpeedTextBox.Location = new System.Drawing.Point(221, 36);
         this.WheelSpeedTextBox.Name = "WheelSpeedTextBox";
         this.WheelSpeedTextBox.Size = new System.Drawing.Size(62, 20);
         this.WheelSpeedTextBox.TabIndex = 168;
         this.WheelSpeedTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // label123
         // 
         this.label123.AutoSize = true;
         this.label123.Location = new System.Drawing.Point(147, 39);
         this.label123.Name = "label123";
         this.label123.Size = new System.Drawing.Size(72, 13);
         this.label123.TabIndex = 167;
         this.label123.Text = "Wheel Speed";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(129, 65);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(90, 13);
         this.label1.TabIndex = 169;
         this.label1.Text = "Wheel Start Load";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(129, 91);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(90, 13);
         this.label2.TabIndex = 170;
         this.label2.Text = "Wheel Stop Load";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(340, 39);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(65, 13);
         this.label3.TabIndex = 171;
         this.label3.Text = "Current Limit";
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(336, 65);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(69, 13);
         this.label4.TabIndex = 172;
         this.label4.Text = "Thermal Limit";
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(166, 13);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(53, 13);
         this.label5.TabIndex = 173;
         this.label5.Text = "Run Time";
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(333, 91);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(72, 13);
         this.label6.TabIndex = 174;
         this.label6.Text = "Slippage Limit";
         // 
         // WheelStartLoadTextBox
         // 
         this.WheelStartLoadTextBox.Location = new System.Drawing.Point(221, 62);
         this.WheelStartLoadTextBox.Name = "WheelStartLoadTextBox";
         this.WheelStartLoadTextBox.Size = new System.Drawing.Size(62, 20);
         this.WheelStartLoadTextBox.TabIndex = 175;
         this.WheelStartLoadTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // WheelStopLoadTextBox
         // 
         this.WheelStopLoadTextBox.Location = new System.Drawing.Point(221, 88);
         this.WheelStopLoadTextBox.Name = "WheelStopLoadTextBox";
         this.WheelStopLoadTextBox.Size = new System.Drawing.Size(62, 20);
         this.WheelStopLoadTextBox.TabIndex = 176;
         this.WheelStopLoadTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // CurrentLimitTextBox
         // 
         this.CurrentLimitTextBox.Location = new System.Drawing.Point(407, 36);
         this.CurrentLimitTextBox.Name = "CurrentLimitTextBox";
         this.CurrentLimitTextBox.Size = new System.Drawing.Size(62, 20);
         this.CurrentLimitTextBox.TabIndex = 177;
         this.CurrentLimitTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // RunTimeTextBox
         // 
         this.RunTimeTextBox.Location = new System.Drawing.Point(221, 10);
         this.RunTimeTextBox.Name = "RunTimeTextBox";
         this.RunTimeTextBox.Size = new System.Drawing.Size(62, 20);
         this.RunTimeTextBox.TabIndex = 178;
         this.RunTimeTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(285, 13);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(53, 13);
         this.label7.TabIndex = 179;
         this.label7.Text = "(hr/min/s)";
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.Location = new System.Drawing.Point(471, 91);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(21, 13);
         this.label8.TabIndex = 180;
         this.label8.Text = "(%)";
         this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // ThermalLimitTextBox
         // 
         this.ThermalLimitTextBox.Location = new System.Drawing.Point(407, 62);
         this.ThermalLimitTextBox.Name = "ThermalLimitTextBox";
         this.ThermalLimitTextBox.Size = new System.Drawing.Size(62, 20);
         this.ThermalLimitTextBox.TabIndex = 181;
         this.ThermalLimitTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // SlippageLimitTextBox
         // 
         this.SlippageLimitTextBox.Location = new System.Drawing.Point(407, 88);
         this.SlippageLimitTextBox.Name = "SlippageLimitTextBox";
         this.SlippageLimitTextBox.Size = new System.Drawing.Size(62, 20);
         this.SlippageLimitTextBox.TabIndex = 182;
         this.SlippageLimitTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // label9
         // 
         this.label9.AutoSize = true;
         this.label9.Location = new System.Drawing.Point(472, 65);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(24, 13);
         this.label9.TabIndex = 183;
         this.label9.Text = "(°C)";
         this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // label10
         // 
         this.label10.AutoSize = true;
         this.label10.Location = new System.Drawing.Point(471, 39);
         this.label10.Name = "label10";
         this.label10.Size = new System.Drawing.Size(20, 13);
         this.label10.TabIndex = 184;
         this.label10.Text = "(A)";
         this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // label11
         // 
         this.label11.AutoSize = true;
         this.label11.Location = new System.Drawing.Point(285, 91);
         this.label11.Name = "label11";
         this.label11.Size = new System.Drawing.Size(35, 13);
         this.label11.TabIndex = 185;
         this.label11.Text = "(volts)";
         this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // label12
         // 
         this.label12.AutoSize = true;
         this.label12.Location = new System.Drawing.Point(285, 65);
         this.label12.Name = "label12";
         this.label12.Size = new System.Drawing.Size(35, 13);
         this.label12.TabIndex = 186;
         this.label12.Text = "(volts)";
         this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // label13
         // 
         this.label13.AutoSize = true;
         this.label13.Location = new System.Drawing.Point(285, 39);
         this.label13.Name = "label13";
         this.label13.Size = new System.Drawing.Size(37, 13);
         this.label13.TabIndex = 187;
         this.label13.Text = "(RPM)";
         this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // MainTabControl
         // 
         this.MainTabControl.Controls.Add(this.ControlTabPage);
         this.MainTabControl.Controls.Add(this.CanBusTabPage);
         this.MainTabControl.Controls.Add(this.TraceActivityTabPage);
         this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.MainTabControl.Location = new System.Drawing.Point(0, 179);
         this.MainTabControl.Name = "MainTabControl";
         this.MainTabControl.SelectedIndex = 0;
         this.MainTabControl.Size = new System.Drawing.Size(612, 147);
         this.MainTabControl.TabIndex = 188;
         // 
         // ControlTabPage
         // 
         this.ControlTabPage.Controls.Add(this.ClearActivityLogButton);
         this.ControlTabPage.Controls.Add(this.SaveParametersButton);
         this.ControlTabPage.Controls.Add(this.LoadParametersButton);
         this.ControlTabPage.Controls.Add(this.CloseButton);
         this.ControlTabPage.Controls.Add(this.SlippageLimitTextBox);
         this.ControlTabPage.Controls.Add(this.ThermalLimitTextBox);
         this.ControlTabPage.Controls.Add(this.ActivityButton);
         this.ControlTabPage.Controls.Add(this.RunTimeTextBox);
         this.ControlTabPage.Controls.Add(this.CurrentLimitTextBox);
         this.ControlTabPage.Controls.Add(this.label123);
         this.ControlTabPage.Controls.Add(this.WheelStopLoadTextBox);
         this.ControlTabPage.Controls.Add(this.label1);
         this.ControlTabPage.Controls.Add(this.WheelStartLoadTextBox);
         this.ControlTabPage.Controls.Add(this.label2);
         this.ControlTabPage.Controls.Add(this.WheelSpeedTextBox);
         this.ControlTabPage.Controls.Add(this.label3);
         this.ControlTabPage.Controls.Add(this.label13);
         this.ControlTabPage.Controls.Add(this.label4);
         this.ControlTabPage.Controls.Add(this.label12);
         this.ControlTabPage.Controls.Add(this.label5);
         this.ControlTabPage.Controls.Add(this.label11);
         this.ControlTabPage.Controls.Add(this.label6);
         this.ControlTabPage.Controls.Add(this.label10);
         this.ControlTabPage.Controls.Add(this.label7);
         this.ControlTabPage.Controls.Add(this.label9);
         this.ControlTabPage.Controls.Add(this.label8);
         this.ControlTabPage.Location = new System.Drawing.Point(4, 22);
         this.ControlTabPage.Name = "ControlTabPage";
         this.ControlTabPage.Padding = new System.Windows.Forms.Padding(3);
         this.ControlTabPage.Size = new System.Drawing.Size(604, 121);
         this.ControlTabPage.TabIndex = 0;
         this.ControlTabPage.Text = "Control";
         this.ControlTabPage.UseVisualStyleBackColor = true;
         // 
         // SaveParametersButton
         // 
         this.SaveParametersButton.Location = new System.Drawing.Point(437, 8);
         this.SaveParametersButton.Name = "SaveParametersButton";
         this.SaveParametersButton.Size = new System.Drawing.Size(59, 23);
         this.SaveParametersButton.TabIndex = 189;
         this.SaveParametersButton.Text = "Save";
         this.SaveParametersButton.UseVisualStyleBackColor = true;
         this.SaveParametersButton.Click += new System.EventHandler(this.SaveParametersButton_Click);
         // 
         // LoadParametersButton
         // 
         this.LoadParametersButton.Location = new System.Drawing.Point(372, 8);
         this.LoadParametersButton.Name = "LoadParametersButton";
         this.LoadParametersButton.Size = new System.Drawing.Size(59, 23);
         this.LoadParametersButton.TabIndex = 188;
         this.LoadParametersButton.Text = "Load";
         this.LoadParametersButton.UseVisualStyleBackColor = true;
         this.LoadParametersButton.Click += new System.EventHandler(this.LoadParametersButton_Click);
         // 
         // CanBusTabPage
         // 
         this.CanBusTabPage.Controls.Add(this.DigitalIoIdTextBox);
         this.CanBusTabPage.Controls.Add(this.AnalogIoIdTextBox);
         this.CanBusTabPage.Controls.Add(this.EncoderIdTextBox);
         this.CanBusTabPage.Controls.Add(this.label18);
         this.CanBusTabPage.Controls.Add(this.label17);
         this.CanBusTabPage.Controls.Add(this.label16);
         this.CanBusTabPage.Controls.Add(this.UutIdTextBox);
         this.CanBusTabPage.Controls.Add(this.label15);
         this.CanBusTabPage.Controls.Add(this.label14);
         this.CanBusTabPage.Controls.Add(this.BusInterfaceComboBox);
         this.CanBusTabPage.Controls.Add(this.BaudComboBox);
         this.CanBusTabPage.Location = new System.Drawing.Point(4, 22);
         this.CanBusTabPage.Name = "CanBusTabPage";
         this.CanBusTabPage.Padding = new System.Windows.Forms.Padding(3);
         this.CanBusTabPage.Size = new System.Drawing.Size(604, 121);
         this.CanBusTabPage.TabIndex = 1;
         this.CanBusTabPage.Text = "CAN BUS";
         this.CanBusTabPage.UseVisualStyleBackColor = true;
         // 
         // DigitalIoIdTextBox
         // 
         this.DigitalIoIdTextBox.Location = new System.Drawing.Point(375, 59);
         this.DigitalIoIdTextBox.Name = "DigitalIoIdTextBox";
         this.DigitalIoIdTextBox.Size = new System.Drawing.Size(62, 20);
         this.DigitalIoIdTextBox.TabIndex = 186;
         this.DigitalIoIdTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // AnalogIoIdTextBox
         // 
         this.AnalogIoIdTextBox.Location = new System.Drawing.Point(375, 33);
         this.AnalogIoIdTextBox.Name = "AnalogIoIdTextBox";
         this.AnalogIoIdTextBox.Size = new System.Drawing.Size(62, 20);
         this.AnalogIoIdTextBox.TabIndex = 185;
         this.AnalogIoIdTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // EncoderIdTextBox
         // 
         this.EncoderIdTextBox.Location = new System.Drawing.Point(375, 7);
         this.EncoderIdTextBox.Name = "EncoderIdTextBox";
         this.EncoderIdTextBox.Size = new System.Drawing.Size(62, 20);
         this.EncoderIdTextBox.TabIndex = 184;
         this.EncoderIdTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // label18
         // 
         this.label18.AutoSize = true;
         this.label18.Location = new System.Drawing.Point(312, 62);
         this.label18.Name = "label18";
         this.label18.Size = new System.Drawing.Size(61, 13);
         this.label18.TabIndex = 183;
         this.label18.Text = "DigitalIO ID";
         // 
         // label17
         // 
         this.label17.AutoSize = true;
         this.label17.Location = new System.Drawing.Point(312, 10);
         this.label17.Name = "label17";
         this.label17.Size = new System.Drawing.Size(61, 13);
         this.label17.TabIndex = 182;
         this.label17.Text = "Encoder ID";
         // 
         // label16
         // 
         this.label16.AutoSize = true;
         this.label16.Location = new System.Drawing.Point(308, 36);
         this.label16.Name = "label16";
         this.label16.Size = new System.Drawing.Size(65, 13);
         this.label16.TabIndex = 181;
         this.label16.Text = "AnalogIO ID";
         // 
         // UutIdTextBox
         // 
         this.UutIdTextBox.Location = new System.Drawing.Point(234, 7);
         this.UutIdTextBox.Name = "UutIdTextBox";
         this.UutIdTextBox.Size = new System.Drawing.Size(62, 20);
         this.UutIdTextBox.TabIndex = 180;
         this.UutIdTextBox.Enter += new System.EventHandler(this.ParsedEntryControl_Enter);
         // 
         // label15
         // 
         this.label15.AutoSize = true;
         this.label15.Location = new System.Drawing.Point(188, 10);
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
         // MainControlSplitter
         // 
         this.MainControlSplitter.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.MainControlSplitter.Location = new System.Drawing.Point(0, 176);
         this.MainControlSplitter.Name = "MainControlSplitter";
         this.MainControlSplitter.Size = new System.Drawing.Size(612, 3);
         this.MainControlSplitter.TabIndex = 189;
         this.MainControlSplitter.TabStop = false;
         // 
         // MainActivityPanel
         // 
         this.MainActivityPanel.Controls.Add(this.ActivityRichTextBox);
         this.MainActivityPanel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.MainActivityPanel.Location = new System.Drawing.Point(0, 0);
         this.MainActivityPanel.Name = "MainActivityPanel";
         this.MainActivityPanel.Size = new System.Drawing.Size(612, 176);
         this.MainActivityPanel.TabIndex = 190;
         // 
         // ActivityRichTextBox
         // 
         this.ActivityRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.ActivityRichTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ActivityRichTextBox.Location = new System.Drawing.Point(0, 0);
         this.ActivityRichTextBox.Name = "ActivityRichTextBox";
         this.ActivityRichTextBox.Size = new System.Drawing.Size(612, 176);
         this.ActivityRichTextBox.TabIndex = 0;
         this.ActivityRichTextBox.Text = "";
         this.ActivityRichTextBox.WordWrap = false;
         // 
         // UpdateTimer
         // 
         this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
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
         // ClearActivityLogButton
         // 
         this.ClearActivityLogButton.Location = new System.Drawing.Point(537, 8);
         this.ClearActivityLogButton.Name = "ClearActivityLogButton";
         this.ClearActivityLogButton.Size = new System.Drawing.Size(59, 23);
         this.ClearActivityLogButton.TabIndex = 190;
         this.ClearActivityLogButton.Text = "Clear";
         this.ClearActivityLogButton.UseVisualStyleBackColor = true;
         this.ClearActivityLogButton.Click += new System.EventHandler(this.ClearActivityLogButton_Click);
         // 
         // MainForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(612, 348);
         this.Controls.Add(this.MainActivityPanel);
         this.Controls.Add(this.MainControlSplitter);
         this.Controls.Add(this.MainTabControl);
         this.Controls.Add(this.MainStatusStrip);
         this.MinimumSize = new System.Drawing.Size(620, 375);
         this.Name = "MainForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "dynoGUI";
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
         this.Shown += new System.EventHandler(this.MainForm_Shown);
         this.MainStatusStrip.ResumeLayout(false);
         this.MainStatusStrip.PerformLayout();
         this.MainTabControl.ResumeLayout(false);
         this.ControlTabPage.ResumeLayout(false);
         this.ControlTabPage.PerformLayout();
         this.CanBusTabPage.ResumeLayout(false);
         this.CanBusTabPage.PerformLayout();
         this.MainActivityPanel.ResumeLayout(false);
         this.TraceActivityTabPage.ResumeLayout(false);
         this.TraceActivityTabPage.PerformLayout();
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
      private System.Windows.Forms.TabPage ControlTabPage;
      private System.Windows.Forms.Splitter MainControlSplitter;
      private System.Windows.Forms.Panel MainActivityPanel;
      private System.Windows.Forms.RichTextBox ActivityRichTextBox;
      private System.Windows.Forms.Button SaveParametersButton;
      private System.Windows.Forms.Button LoadParametersButton;
      private System.Windows.Forms.Timer UpdateTimer;
      private System.Windows.Forms.TabPage CanBusTabPage;
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
   }
}

