namespace E4.Ui
{
   partial class LaserRobotMovementSetupForm
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
         this.MainPanel = new E4.Ui.Controls.BorderedPanel();
         this.WheelDrivePanel = new E4.Ui.Controls.TransparentPanel();
         this.RearDirectionToggleButton = new E4.Ui.Controls.ValueToggleButton();
         this.FrontDirectionToggleButton = new E4.Ui.Controls.ValueToggleButton();
         this.RearStateCycleButton = new E4.Ui.Controls.ValueCycleButton();
         this.transparentLabel1 = new E4.Ui.Controls.TransparentLabel();
         this.FrontStateCycleButton = new E4.Ui.Controls.ValueCycleButton();
         this.MaxSpeedValueButton = new E4.Ui.Controls.ValueButton();
         this.LowSpeedScaleValueButton = new E4.Ui.Controls.ValueButton();
         this.RearPanel = new E4.Ui.Controls.BorderedPanel();
         this.label6 = new System.Windows.Forms.Label();
         this.FrontPanel = new E4.Ui.Controls.BorderedPanel();
         this.label1 = new System.Windows.Forms.Label();
         this.TitleLabel = new System.Windows.Forms.Label();
         this.BackButton = new E4.Ui.Controls.E4Button();
         this.MainPanel.SuspendLayout();
         this.WheelDrivePanel.SuspendLayout();
         this.RearPanel.SuspendLayout();
         this.FrontPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // MainPanel
         // 
         this.MainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(80)))), ((int)(((byte)(96)))));
         this.MainPanel.Controls.Add(this.WheelDrivePanel);
         this.MainPanel.Controls.Add(this.MaxSpeedValueButton);
         this.MainPanel.Controls.Add(this.LowSpeedScaleValueButton);
         this.MainPanel.Controls.Add(this.RearPanel);
         this.MainPanel.Controls.Add(this.FrontPanel);
         this.MainPanel.Controls.Add(this.TitleLabel);
         this.MainPanel.Controls.Add(this.BackButton);
         this.MainPanel.EdgeWeight = 3;
         this.MainPanel.Location = new System.Drawing.Point(0, 0);
         this.MainPanel.Name = "MainPanel";
         this.MainPanel.Size = new System.Drawing.Size(451, 355);
         this.MainPanel.TabIndex = 0;
         // 
         // WheelDrivePanel
         // 
         this.WheelDrivePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(76)))), ((int)(((byte)(52)))));
         this.WheelDrivePanel.Controls.Add(this.RearDirectionToggleButton);
         this.WheelDrivePanel.Controls.Add(this.FrontDirectionToggleButton);
         this.WheelDrivePanel.Controls.Add(this.RearStateCycleButton);
         this.WheelDrivePanel.Controls.Add(this.transparentLabel1);
         this.WheelDrivePanel.Controls.Add(this.FrontStateCycleButton);
         this.WheelDrivePanel.EdgeWeight = 2;
         this.WheelDrivePanel.Location = new System.Drawing.Point(27, 129);
         this.WheelDrivePanel.Name = "WheelDrivePanel";
         this.WheelDrivePanel.Opacity = 50;
         this.WheelDrivePanel.Size = new System.Drawing.Size(274, 210);
         this.WheelDrivePanel.TabIndex = 210;
         // 
         // RearDirectionToggleButton
         // 
         this.RearDirectionToggleButton.AutomaticToggle = true;
         this.RearDirectionToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RearDirectionToggleButton.DisabledBackColor = System.Drawing.Color.Black;
         this.RearDirectionToggleButton.DisabledForeColor = System.Drawing.Color.Gray;
         this.RearDirectionToggleButton.DisabledOptionBackColor = System.Drawing.Color.Black;
         this.RearDirectionToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
         this.RearDirectionToggleButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.RearDirectionToggleButton.HoldEnable = false;
         this.RearDirectionToggleButton.HoldTimeoutInterval = 0;
         this.RearDirectionToggleButton.Location = new System.Drawing.Point(156, 135);
         this.RearDirectionToggleButton.Name = "RearDirectionToggleButton";
         this.RearDirectionToggleButton.OptionASelected = true;
         this.RearDirectionToggleButton.OptionAText = "NORMAL";
         this.RearDirectionToggleButton.OptionBSelected = false;
         this.RearDirectionToggleButton.OptionBText = "INVERSE";
         this.RearDirectionToggleButton.OptionCenterWidth = 0;
         this.RearDirectionToggleButton.OptionEdgeHeight = 8;
         this.RearDirectionToggleButton.OptionHeight = 22;
         this.RearDirectionToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.RearDirectionToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
         this.RearDirectionToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.RearDirectionToggleButton.OptionSelectedBackColor = System.Drawing.Color.Lime;
         this.RearDirectionToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold);
         this.RearDirectionToggleButton.OptionSelectedForeColor = System.Drawing.Color.Black;
         this.RearDirectionToggleButton.OptionWidth = 50;
         this.RearDirectionToggleButton.Size = new System.Drawing.Size(107, 67);
         this.RearDirectionToggleButton.TabIndex = 159;
         this.RearDirectionToggleButton.Text = "DIRECTION";
         this.RearDirectionToggleButton.UseVisualStyleBackColor = false;
         this.RearDirectionToggleButton.Click += new System.EventHandler(this.RearDirectionToggleButton_Click);
         // 
         // FrontDirectionToggleButton
         // 
         this.FrontDirectionToggleButton.AutomaticToggle = true;
         this.FrontDirectionToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.FrontDirectionToggleButton.DisabledBackColor = System.Drawing.Color.Black;
         this.FrontDirectionToggleButton.DisabledForeColor = System.Drawing.Color.Gray;
         this.FrontDirectionToggleButton.DisabledOptionBackColor = System.Drawing.Color.Black;
         this.FrontDirectionToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
         this.FrontDirectionToggleButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.FrontDirectionToggleButton.HoldEnable = false;
         this.FrontDirectionToggleButton.HoldTimeoutInterval = 0;
         this.FrontDirectionToggleButton.Location = new System.Drawing.Point(11, 135);
         this.FrontDirectionToggleButton.Name = "FrontDirectionToggleButton";
         this.FrontDirectionToggleButton.OptionASelected = true;
         this.FrontDirectionToggleButton.OptionAText = "NORMAL";
         this.FrontDirectionToggleButton.OptionBSelected = false;
         this.FrontDirectionToggleButton.OptionBText = "INVERSE";
         this.FrontDirectionToggleButton.OptionCenterWidth = 0;
         this.FrontDirectionToggleButton.OptionEdgeHeight = 8;
         this.FrontDirectionToggleButton.OptionHeight = 22;
         this.FrontDirectionToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.FrontDirectionToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
         this.FrontDirectionToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.FrontDirectionToggleButton.OptionSelectedBackColor = System.Drawing.Color.Lime;
         this.FrontDirectionToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold);
         this.FrontDirectionToggleButton.OptionSelectedForeColor = System.Drawing.Color.Black;
         this.FrontDirectionToggleButton.OptionWidth = 50;
         this.FrontDirectionToggleButton.Size = new System.Drawing.Size(107, 67);
         this.FrontDirectionToggleButton.TabIndex = 158;
         this.FrontDirectionToggleButton.Text = "DIRECTION";
         this.FrontDirectionToggleButton.UseVisualStyleBackColor = false;
         this.FrontDirectionToggleButton.Click += new System.EventHandler(this.FrontDirectionToggleButton_Click);
         // 
         // RearStateCycleButton
         // 
         this.RearStateCycleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RearStateCycleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.RearStateCycleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.RearStateCycleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.RearStateCycleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
         this.RearStateCycleButton.HoldEnable = true;
         this.RearStateCycleButton.HoldTimeoutInterval = 100;
         this.RearStateCycleButton.Location = new System.Drawing.Point(156, 34);
         this.RearStateCycleButton.Name = "RearStateCycleButton";
         this.RearStateCycleButton.OptionAText = "ENABLE";
         this.RearStateCycleButton.OptionBText = "DISABLE";
         this.RearStateCycleButton.OptionCText = "LOCK";
         this.RearStateCycleButton.OptionEdgeSpace = 8;
         this.RearStateCycleButton.OptionHeight = 18;
         this.RearStateCycleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.RearStateCycleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
         this.RearStateCycleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.RearStateCycleButton.OptionOptionSpace = 2;
         this.RearStateCycleButton.OptionSelectedBackColor = System.Drawing.Color.Lime;
         this.RearStateCycleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.RearStateCycleButton.OptionSelectedForeColor = System.Drawing.Color.Black;
         this.RearStateCycleButton.OptionSelectingBackColor = System.Drawing.Color.Yellow;
         this.RearStateCycleButton.OptionSelectingForeColor = System.Drawing.Color.Black;
         this.RearStateCycleButton.OptionWidth = 80;
         this.RearStateCycleButton.SelectedOption = 3;
         this.RearStateCycleButton.SelectionTimeoutInterval = 3000;
         this.RearStateCycleButton.Size = new System.Drawing.Size(107, 93);
         this.RearStateCycleButton.TabIndex = 157;
         this.RearStateCycleButton.Text = "STATE";
         this.RearStateCycleButton.TimedSelection = true;
         this.RearStateCycleButton.UseVisualStyleBackColor = false;
         this.RearStateCycleButton.HoldTimeout += new E4.Ui.Controls.ValueCycleButton.HoldTimeoutHandler(this.RearStateCycleButton_HoldTimeout);
         this.RearStateCycleButton.SelectionTimeout += new E4.Ui.Controls.ValueCycleButton.SelectionTimeoutHandler(this.RearStateCycleButton_SelectionTimeout);
         this.RearStateCycleButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RearStateCycleButton_MouseClick);
         // 
         // transparentLabel1
         // 
         this.transparentLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
         this.transparentLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.transparentLabel1.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
         this.transparentLabel1.Location = new System.Drawing.Point(2, 2);
         this.transparentLabel1.Name = "transparentLabel1";
         this.transparentLabel1.Opacity = 55;
         this.transparentLabel1.Size = new System.Drawing.Size(270, 24);
         this.transparentLabel1.TabIndex = 156;
         this.transparentLabel1.Text = "WHEEL DRIVE";
         this.transparentLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // FrontStateCycleButton
         // 
         this.FrontStateCycleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.FrontStateCycleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.FrontStateCycleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.FrontStateCycleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.FrontStateCycleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
         this.FrontStateCycleButton.HoldEnable = true;
         this.FrontStateCycleButton.HoldTimeoutInterval = 100;
         this.FrontStateCycleButton.Location = new System.Drawing.Point(11, 34);
         this.FrontStateCycleButton.Name = "FrontStateCycleButton";
         this.FrontStateCycleButton.OptionAText = "ENABLE";
         this.FrontStateCycleButton.OptionBText = "DISABLE";
         this.FrontStateCycleButton.OptionCText = "LOCK";
         this.FrontStateCycleButton.OptionEdgeSpace = 8;
         this.FrontStateCycleButton.OptionHeight = 18;
         this.FrontStateCycleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.FrontStateCycleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
         this.FrontStateCycleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.FrontStateCycleButton.OptionOptionSpace = 2;
         this.FrontStateCycleButton.OptionSelectedBackColor = System.Drawing.Color.Lime;
         this.FrontStateCycleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.FrontStateCycleButton.OptionSelectedForeColor = System.Drawing.Color.Black;
         this.FrontStateCycleButton.OptionSelectingBackColor = System.Drawing.Color.Yellow;
         this.FrontStateCycleButton.OptionSelectingForeColor = System.Drawing.Color.Black;
         this.FrontStateCycleButton.OptionWidth = 80;
         this.FrontStateCycleButton.SelectedOption = 3;
         this.FrontStateCycleButton.SelectionTimeoutInterval = 3000;
         this.FrontStateCycleButton.Size = new System.Drawing.Size(107, 93);
         this.FrontStateCycleButton.TabIndex = 155;
         this.FrontStateCycleButton.Text = "STATE";
         this.FrontStateCycleButton.TimedSelection = true;
         this.FrontStateCycleButton.UseVisualStyleBackColor = false;
         this.FrontStateCycleButton.HoldTimeout += new E4.Ui.Controls.ValueCycleButton.HoldTimeoutHandler(this.FrontStateCycleButton_HoldTimeout);
         this.FrontStateCycleButton.SelectionTimeout += new E4.Ui.Controls.ValueCycleButton.SelectionTimeoutHandler(this.FrontStateCycleButton_SelectionTimeout);
         this.FrontStateCycleButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FrontStateCycleButton_MouseClick);
         // 
         // MaxSpeedValueButton
         // 
         this.MaxSpeedValueButton.ArrowWidth = 0;
         this.MaxSpeedValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.MaxSpeedValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.MaxSpeedValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.MaxSpeedValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.MaxSpeedValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.MaxSpeedValueButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.MaxSpeedValueButton.HoldTimeoutInterval = 0;
         this.MaxSpeedValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.MaxSpeedValueButton.LeftArrowVisible = false;
         this.MaxSpeedValueButton.Location = new System.Drawing.Point(317, 68);
         this.MaxSpeedValueButton.Name = "MaxSpeedValueButton";
         this.MaxSpeedValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.MaxSpeedValueButton.RightArrowVisible = false;
         this.MaxSpeedValueButton.Size = new System.Drawing.Size(107, 90);
         this.MaxSpeedValueButton.TabIndex = 209;
         this.MaxSpeedValueButton.Text = "MAX      SPEED";
         this.MaxSpeedValueButton.UseVisualStyleBackColor = false;
         this.MaxSpeedValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.MaxSpeedValueButton.ValueEdgeHeight = 8;
         this.MaxSpeedValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.MaxSpeedValueButton.ValueForeColor = System.Drawing.Color.White;
         this.MaxSpeedValueButton.ValueHeight = 22;
         this.MaxSpeedValueButton.ValueText = "### RPM";
         this.MaxSpeedValueButton.ValueWidth = 80;
         this.MaxSpeedValueButton.Click += new System.EventHandler(this.MaxSpeedValueButton_Click);
         // 
         // LowSpeedScaleValueButton
         // 
         this.LowSpeedScaleValueButton.ArrowWidth = 0;
         this.LowSpeedScaleValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.LowSpeedScaleValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.LowSpeedScaleValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.LowSpeedScaleValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.LowSpeedScaleValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LowSpeedScaleValueButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.LowSpeedScaleValueButton.HoldTimeoutInterval = 0;
         this.LowSpeedScaleValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.LowSpeedScaleValueButton.LeftArrowVisible = false;
         this.LowSpeedScaleValueButton.Location = new System.Drawing.Point(317, 166);
         this.LowSpeedScaleValueButton.Name = "LowSpeedScaleValueButton";
         this.LowSpeedScaleValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.LowSpeedScaleValueButton.RightArrowVisible = false;
         this.LowSpeedScaleValueButton.Size = new System.Drawing.Size(107, 90);
         this.LowSpeedScaleValueButton.TabIndex = 208;
         this.LowSpeedScaleValueButton.Text = "LOW SPEED SCALE";
         this.LowSpeedScaleValueButton.UseVisualStyleBackColor = false;
         this.LowSpeedScaleValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.LowSpeedScaleValueButton.ValueEdgeHeight = 8;
         this.LowSpeedScaleValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.LowSpeedScaleValueButton.ValueForeColor = System.Drawing.Color.White;
         this.LowSpeedScaleValueButton.ValueHeight = 22;
         this.LowSpeedScaleValueButton.ValueText = "## %";
         this.LowSpeedScaleValueButton.ValueWidth = 80;
         this.LowSpeedScaleValueButton.Click += new System.EventHandler(this.LowSpeedScaleValueButton_Click);
         // 
         // RearPanel
         // 
         this.RearPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.RearPanel.Controls.Add(this.label6);
         this.RearPanel.EdgeWeight = 2;
         this.RearPanel.Location = new System.Drawing.Point(172, 68);
         this.RearPanel.Name = "RearPanel";
         this.RearPanel.Size = new System.Drawing.Size(129, 271);
         this.RearPanel.TabIndex = 207;
         // 
         // label6
         // 
         this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
         this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label6.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
         this.label6.Location = new System.Drawing.Point(2, 2);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(124, 51);
         this.label6.TabIndex = 154;
         this.label6.Text = "REAR";
         this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // FrontPanel
         // 
         this.FrontPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.FrontPanel.Controls.Add(this.label1);
         this.FrontPanel.EdgeWeight = 2;
         this.FrontPanel.Location = new System.Drawing.Point(27, 68);
         this.FrontPanel.Name = "FrontPanel";
         this.FrontPanel.Size = new System.Drawing.Size(129, 271);
         this.FrontPanel.TabIndex = 206;
         // 
         // label1
         // 
         this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
         this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label1.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
         this.label1.Location = new System.Drawing.Point(2, 2);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(124, 51);
         this.label1.TabIndex = 155;
         this.label1.Text = "FRONT";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // TitleLabel
         // 
         this.TitleLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.TitleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TitleLabel.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
         this.TitleLabel.Location = new System.Drawing.Point(16, 16);
         this.TitleLabel.Name = "TitleLabel";
         this.TitleLabel.Size = new System.Drawing.Size(419, 36);
         this.TitleLabel.TabIndex = 134;
         this.TitleLabel.Text = "LASER WHEEL MOTOR SETUP";
         this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.TitleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitleLabel_MouseDown);
         this.TitleLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TitleLabel_MouseMove);
         this.TitleLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TitleLabel_MouseUp);
         // 
         // BackButton
         // 
         this.BackButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BackButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BackButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.BackButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.BackButton.Location = new System.Drawing.Point(317, 272);
         this.BackButton.Name = "BackButton";
         this.BackButton.Size = new System.Drawing.Size(107, 67);
         this.BackButton.TabIndex = 5;
         this.BackButton.Text = "BACK";
         this.BackButton.UseVisualStyleBackColor = false;
         this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
         // 
         // LaserRobotMovementSetupForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(451, 355);
         this.Controls.Add(this.MainPanel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "LaserRobotMovementSetupForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "LaserRobotMovementSetupForm";
         this.Shown += new System.EventHandler(this.LaserRobotMovementSetupForm_Shown);
         this.MainPanel.ResumeLayout(false);
         this.WheelDrivePanel.ResumeLayout(false);
         this.RearPanel.ResumeLayout(false);
         this.FrontPanel.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private Controls.BorderedPanel MainPanel;
      private Controls.E4Button BackButton;
      private System.Windows.Forms.Label TitleLabel;
      private Controls.ValueButton MaxSpeedValueButton;
      private Controls.ValueButton LowSpeedScaleValueButton;
      private Controls.BorderedPanel RearPanel;
      private System.Windows.Forms.Label label6;
      private Controls.BorderedPanel FrontPanel;
      private System.Windows.Forms.Label label1;
      private Controls.TransparentPanel WheelDrivePanel;
      private Controls.ValueToggleButton RearDirectionToggleButton;
      private Controls.ValueToggleButton FrontDirectionToggleButton;
      private Controls.ValueCycleButton RearStateCycleButton;
      private Controls.TransparentLabel transparentLabel1;
      private Controls.ValueCycleButton FrontStateCycleButton;
   }
}