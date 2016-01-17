namespace NICBOT.GUI
{
   partial class MovementSetupForm
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
         this.BackButton = new NICBOT.GUI.NicBotButton();
         this.CornerAxialModePanel = new NICBOT.GUI.TransparentPanel();
         this.transparentLabel3 = new NICBOT.GUI.TransparentLabel();
         this.BottomRearCornerAxialToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.BottomFrontCornerAxialToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.TopRearCornerAxialToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.TopFrontCornerAxialToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.CircumferentialModePanel = new NICBOT.GUI.TransparentPanel();
         this.transparentLabel1 = new NICBOT.GUI.TransparentLabel();
         this.BottomRearCircumferentialToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.BottomFrontCircumferentialToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.TopRearCircumferentialToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.TopFrontCircumferentialToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.LaunchAxialModePanel = new NICBOT.GUI.TransparentPanel();
         this.transparentLabel4 = new NICBOT.GUI.TransparentLabel();
         this.BottomRearLaunchAxialToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.BottomFrontLaunchAxialToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.TopRearLaunchAxialToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.TopFrontLaunchAxialToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.LowSpeedScaleValueButton = new NICBOT.GUI.ValueButton();
         this.CurrentPer1KValueButton = new NICBOT.GUI.ValueButton();
         this.MaxSpeedValueButton = new NICBOT.GUI.ValueButton();
         this.MaxCurrentValueButton = new NICBOT.GUI.ValueButton();
         this.LockCurrentValueButton = new NICBOT.GUI.ValueButton();
         this.TitleLabel = new System.Windows.Forms.Label();
         this.NormalAxialModePanel = new NICBOT.GUI.TransparentPanel();
         this.transparentLabel2 = new NICBOT.GUI.TransparentLabel();
         this.BottomRearNormalAxialModeToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.BottomFrontNormalAxialModeToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.TopRearNormalAxialModeToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.TopFrontNormalAxialModeToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.borderedPanel4 = new NICBOT.GUI.BorderedPanel();
         this.BottomRearStateCycleButton = new NICBOT.GUI.ValueCycleButton();
         this.label1 = new System.Windows.Forms.Label();
         this.BottomRearDirectionToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.borderedPanel3 = new NICBOT.GUI.BorderedPanel();
         this.BottomFrontStateCycleButton = new NICBOT.GUI.ValueCycleButton();
         this.label5 = new System.Windows.Forms.Label();
         this.BottomFrontDirectionToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.borderedPanel1 = new NICBOT.GUI.BorderedPanel();
         this.TopRearStateCycleButton = new NICBOT.GUI.ValueCycleButton();
         this.label6 = new System.Windows.Forms.Label();
         this.TopRearDirectionToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.borderedPanel2 = new NICBOT.GUI.BorderedPanel();
         this.TopFrontStateCycleButton = new NICBOT.GUI.ValueCycleButton();
         this.label7 = new System.Windows.Forms.Label();
         this.TopFrontDirectionToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.MainPanel.SuspendLayout();
         this.CornerAxialModePanel.SuspendLayout();
         this.CircumferentialModePanel.SuspendLayout();
         this.LaunchAxialModePanel.SuspendLayout();
         this.NormalAxialModePanel.SuspendLayout();
         this.borderedPanel4.SuspendLayout();
         this.borderedPanel3.SuspendLayout();
         this.borderedPanel1.SuspendLayout();
         this.borderedPanel2.SuspendLayout();
         this.SuspendLayout();
         // 
         // MainPanel
         // 
         this.MainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(80)))), ((int)(((byte)(96)))));
         this.MainPanel.Controls.Add(this.BackButton);
         this.MainPanel.Controls.Add(this.CornerAxialModePanel);
         this.MainPanel.Controls.Add(this.CircumferentialModePanel);
         this.MainPanel.Controls.Add(this.LaunchAxialModePanel);
         this.MainPanel.Controls.Add(this.LowSpeedScaleValueButton);
         this.MainPanel.Controls.Add(this.CurrentPer1KValueButton);
         this.MainPanel.Controls.Add(this.MaxSpeedValueButton);
         this.MainPanel.Controls.Add(this.MaxCurrentValueButton);
         this.MainPanel.Controls.Add(this.LockCurrentValueButton);
         this.MainPanel.Controls.Add(this.TitleLabel);
         this.MainPanel.Controls.Add(this.NormalAxialModePanel);
         this.MainPanel.Controls.Add(this.borderedPanel4);
         this.MainPanel.Controls.Add(this.borderedPanel3);
         this.MainPanel.Controls.Add(this.borderedPanel1);
         this.MainPanel.Controls.Add(this.borderedPanel2);
         this.MainPanel.EdgeWeight = 3;
         this.MainPanel.Location = new System.Drawing.Point(0, 0);
         this.MainPanel.Name = "MainPanel";
         this.MainPanel.Size = new System.Drawing.Size(741, 901);
         this.MainPanel.TabIndex = 0;
         // 
         // BackButton
         // 
         this.BackButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BackButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BackButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.BackButton.Location = new System.Drawing.Point(607, 796);
         this.BackButton.Name = "BackButton";
         this.BackButton.Size = new System.Drawing.Size(107, 67);
         this.BackButton.TabIndex = 167;
         this.BackButton.Text = "BACK";
         this.BackButton.UseVisualStyleBackColor = false;
         this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
         // 
         // CornerAxialModePanel
         // 
         this.CornerAxialModePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(76)))), ((int)(((byte)(52)))));
         this.CornerAxialModePanel.Controls.Add(this.transparentLabel3);
         this.CornerAxialModePanel.Controls.Add(this.BottomRearCornerAxialToggleButton);
         this.CornerAxialModePanel.Controls.Add(this.BottomFrontCornerAxialToggleButton);
         this.CornerAxialModePanel.Controls.Add(this.TopRearCornerAxialToggleButton);
         this.CornerAxialModePanel.Controls.Add(this.TopFrontCornerAxialToggleButton);
         this.CornerAxialModePanel.EdgeWeight = 2;
         this.CornerAxialModePanel.Location = new System.Drawing.Point(27, 607);
         this.CornerAxialModePanel.Name = "CornerAxialModePanel";
         this.CornerAxialModePanel.Opacity = 50;
         this.CornerAxialModePanel.Size = new System.Drawing.Size(564, 134);
         this.CornerAxialModePanel.TabIndex = 3;
         // 
         // transparentLabel3
         // 
         this.transparentLabel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
         this.transparentLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.transparentLabel3.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
         this.transparentLabel3.Location = new System.Drawing.Point(2, 2);
         this.transparentLabel3.Name = "transparentLabel3";
         this.transparentLabel3.Opacity = 55;
         this.transparentLabel3.Size = new System.Drawing.Size(559, 24);
         this.transparentLabel3.TabIndex = 166;
         this.transparentLabel3.Text = "CORNER AXIAL MODE";
         this.transparentLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // BottomRearCornerAxialToggleButton
         // 
         this.BottomRearCornerAxialToggleButton.AutomaticToggle = true;
         this.BottomRearCornerAxialToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BottomRearCornerAxialToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BottomRearCornerAxialToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BottomRearCornerAxialToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.BottomRearCornerAxialToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomRearCornerAxialToggleButton.ForeColor = System.Drawing.Color.Black;
         this.BottomRearCornerAxialToggleButton.HoldEnable = false;
         this.BottomRearCornerAxialToggleButton.HoldTimeoutInterval = 0;
         this.BottomRearCornerAxialToggleButton.Location = new System.Drawing.Point(446, 34);
         this.BottomRearCornerAxialToggleButton.Name = "BottomRearCornerAxialToggleButton";
         this.BottomRearCornerAxialToggleButton.OptionASelected = false;
         this.BottomRearCornerAxialToggleButton.OptionAText = "SPEED";
         this.BottomRearCornerAxialToggleButton.OptionBSelected = true;
         this.BottomRearCornerAxialToggleButton.OptionBText = "CURRENT";
         this.BottomRearCornerAxialToggleButton.OptionCenterWidth = 0;
         this.BottomRearCornerAxialToggleButton.OptionEdgeHeight = 8;
         this.BottomRearCornerAxialToggleButton.OptionHeight = 22;
         this.BottomRearCornerAxialToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.BottomRearCornerAxialToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomRearCornerAxialToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.BottomRearCornerAxialToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.BottomRearCornerAxialToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomRearCornerAxialToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.BottomRearCornerAxialToggleButton.OptionWidth = 50;
         this.BottomRearCornerAxialToggleButton.Size = new System.Drawing.Size(107, 90);
         this.BottomRearCornerAxialToggleButton.TabIndex = 154;
         this.BottomRearCornerAxialToggleButton.Text = "FORWARD CONTROL";
         this.BottomRearCornerAxialToggleButton.UseVisualStyleBackColor = false;
         this.BottomRearCornerAxialToggleButton.Click += new System.EventHandler(this.BottomRearCornerAxialToggleButton_Click);
         // 
         // BottomFrontCornerAxialToggleButton
         // 
         this.BottomFrontCornerAxialToggleButton.AutomaticToggle = true;
         this.BottomFrontCornerAxialToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BottomFrontCornerAxialToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BottomFrontCornerAxialToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BottomFrontCornerAxialToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.BottomFrontCornerAxialToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomFrontCornerAxialToggleButton.ForeColor = System.Drawing.Color.Black;
         this.BottomFrontCornerAxialToggleButton.HoldEnable = false;
         this.BottomFrontCornerAxialToggleButton.HoldTimeoutInterval = 0;
         this.BottomFrontCornerAxialToggleButton.Location = new System.Drawing.Point(301, 34);
         this.BottomFrontCornerAxialToggleButton.Name = "BottomFrontCornerAxialToggleButton";
         this.BottomFrontCornerAxialToggleButton.OptionASelected = false;
         this.BottomFrontCornerAxialToggleButton.OptionAText = "SPEED";
         this.BottomFrontCornerAxialToggleButton.OptionBSelected = true;
         this.BottomFrontCornerAxialToggleButton.OptionBText = "CURRENT";
         this.BottomFrontCornerAxialToggleButton.OptionCenterWidth = 0;
         this.BottomFrontCornerAxialToggleButton.OptionEdgeHeight = 8;
         this.BottomFrontCornerAxialToggleButton.OptionHeight = 22;
         this.BottomFrontCornerAxialToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.BottomFrontCornerAxialToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomFrontCornerAxialToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.BottomFrontCornerAxialToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.BottomFrontCornerAxialToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomFrontCornerAxialToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.BottomFrontCornerAxialToggleButton.OptionWidth = 50;
         this.BottomFrontCornerAxialToggleButton.Size = new System.Drawing.Size(107, 90);
         this.BottomFrontCornerAxialToggleButton.TabIndex = 153;
         this.BottomFrontCornerAxialToggleButton.Text = "FORWARD CONTROL";
         this.BottomFrontCornerAxialToggleButton.UseVisualStyleBackColor = false;
         this.BottomFrontCornerAxialToggleButton.Click += new System.EventHandler(this.BottomFrontCornerAxialToggleButton_Click);
         // 
         // TopRearCornerAxialToggleButton
         // 
         this.TopRearCornerAxialToggleButton.AutomaticToggle = true;
         this.TopRearCornerAxialToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.TopRearCornerAxialToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.TopRearCornerAxialToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.TopRearCornerAxialToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.TopRearCornerAxialToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopRearCornerAxialToggleButton.ForeColor = System.Drawing.Color.Black;
         this.TopRearCornerAxialToggleButton.HoldEnable = false;
         this.TopRearCornerAxialToggleButton.HoldTimeoutInterval = 0;
         this.TopRearCornerAxialToggleButton.Location = new System.Drawing.Point(156, 34);
         this.TopRearCornerAxialToggleButton.Name = "TopRearCornerAxialToggleButton";
         this.TopRearCornerAxialToggleButton.OptionASelected = false;
         this.TopRearCornerAxialToggleButton.OptionAText = "SPEED";
         this.TopRearCornerAxialToggleButton.OptionBSelected = true;
         this.TopRearCornerAxialToggleButton.OptionBText = "CURRENT";
         this.TopRearCornerAxialToggleButton.OptionCenterWidth = 0;
         this.TopRearCornerAxialToggleButton.OptionEdgeHeight = 8;
         this.TopRearCornerAxialToggleButton.OptionHeight = 22;
         this.TopRearCornerAxialToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.TopRearCornerAxialToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopRearCornerAxialToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.TopRearCornerAxialToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.TopRearCornerAxialToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopRearCornerAxialToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.TopRearCornerAxialToggleButton.OptionWidth = 50;
         this.TopRearCornerAxialToggleButton.Size = new System.Drawing.Size(107, 90);
         this.TopRearCornerAxialToggleButton.TabIndex = 152;
         this.TopRearCornerAxialToggleButton.Text = "FORWARD CONTROL";
         this.TopRearCornerAxialToggleButton.UseVisualStyleBackColor = false;
         this.TopRearCornerAxialToggleButton.Click += new System.EventHandler(this.TopRearCornerAxialToggleButton_Click);
         // 
         // TopFrontCornerAxialToggleButton
         // 
         this.TopFrontCornerAxialToggleButton.AutomaticToggle = true;
         this.TopFrontCornerAxialToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.TopFrontCornerAxialToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.TopFrontCornerAxialToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.TopFrontCornerAxialToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.TopFrontCornerAxialToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopFrontCornerAxialToggleButton.ForeColor = System.Drawing.Color.Black;
         this.TopFrontCornerAxialToggleButton.HoldEnable = false;
         this.TopFrontCornerAxialToggleButton.HoldTimeoutInterval = 0;
         this.TopFrontCornerAxialToggleButton.Location = new System.Drawing.Point(11, 34);
         this.TopFrontCornerAxialToggleButton.Name = "TopFrontCornerAxialToggleButton";
         this.TopFrontCornerAxialToggleButton.OptionASelected = false;
         this.TopFrontCornerAxialToggleButton.OptionAText = "SPEED";
         this.TopFrontCornerAxialToggleButton.OptionBSelected = true;
         this.TopFrontCornerAxialToggleButton.OptionBText = "CURRENT";
         this.TopFrontCornerAxialToggleButton.OptionCenterWidth = 0;
         this.TopFrontCornerAxialToggleButton.OptionEdgeHeight = 8;
         this.TopFrontCornerAxialToggleButton.OptionHeight = 22;
         this.TopFrontCornerAxialToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.TopFrontCornerAxialToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopFrontCornerAxialToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.TopFrontCornerAxialToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.TopFrontCornerAxialToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopFrontCornerAxialToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.TopFrontCornerAxialToggleButton.OptionWidth = 50;
         this.TopFrontCornerAxialToggleButton.Size = new System.Drawing.Size(107, 90);
         this.TopFrontCornerAxialToggleButton.TabIndex = 148;
         this.TopFrontCornerAxialToggleButton.Text = "FORWARD CONTROL";
         this.TopFrontCornerAxialToggleButton.UseVisualStyleBackColor = false;
         this.TopFrontCornerAxialToggleButton.Click += new System.EventHandler(this.TopFrontCornerAxialToggleButton_Click);
         // 
         // CircumferentialModePanel
         // 
         this.CircumferentialModePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(76)))), ((int)(((byte)(52)))));
         this.CircumferentialModePanel.Controls.Add(this.transparentLabel1);
         this.CircumferentialModePanel.Controls.Add(this.BottomRearCircumferentialToggleButton);
         this.CircumferentialModePanel.Controls.Add(this.BottomFrontCircumferentialToggleButton);
         this.CircumferentialModePanel.Controls.Add(this.TopRearCircumferentialToggleButton);
         this.CircumferentialModePanel.Controls.Add(this.TopFrontCircumferentialToggleButton);
         this.CircumferentialModePanel.EdgeWeight = 2;
         this.CircumferentialModePanel.Location = new System.Drawing.Point(27, 467);
         this.CircumferentialModePanel.Name = "CircumferentialModePanel";
         this.CircumferentialModePanel.Opacity = 50;
         this.CircumferentialModePanel.Size = new System.Drawing.Size(564, 134);
         this.CircumferentialModePanel.TabIndex = 2;
         // 
         // transparentLabel1
         // 
         this.transparentLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
         this.transparentLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.transparentLabel1.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
         this.transparentLabel1.Location = new System.Drawing.Point(2, 2);
         this.transparentLabel1.Name = "transparentLabel1";
         this.transparentLabel1.Opacity = 55;
         this.transparentLabel1.Size = new System.Drawing.Size(559, 24);
         this.transparentLabel1.TabIndex = 165;
         this.transparentLabel1.Text = "CIRCUMFERENTIAL MODE";
         this.transparentLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // BottomRearCircumferentialToggleButton
         // 
         this.BottomRearCircumferentialToggleButton.AutomaticToggle = true;
         this.BottomRearCircumferentialToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BottomRearCircumferentialToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BottomRearCircumferentialToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BottomRearCircumferentialToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.BottomRearCircumferentialToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomRearCircumferentialToggleButton.ForeColor = System.Drawing.Color.Black;
         this.BottomRearCircumferentialToggleButton.HoldEnable = false;
         this.BottomRearCircumferentialToggleButton.HoldTimeoutInterval = 0;
         this.BottomRearCircumferentialToggleButton.Location = new System.Drawing.Point(446, 34);
         this.BottomRearCircumferentialToggleButton.Name = "BottomRearCircumferentialToggleButton";
         this.BottomRearCircumferentialToggleButton.OptionASelected = false;
         this.BottomRearCircumferentialToggleButton.OptionAText = "SPEED";
         this.BottomRearCircumferentialToggleButton.OptionBSelected = true;
         this.BottomRearCircumferentialToggleButton.OptionBText = "CURRENT";
         this.BottomRearCircumferentialToggleButton.OptionCenterWidth = 0;
         this.BottomRearCircumferentialToggleButton.OptionEdgeHeight = 8;
         this.BottomRearCircumferentialToggleButton.OptionHeight = 22;
         this.BottomRearCircumferentialToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.BottomRearCircumferentialToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomRearCircumferentialToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.BottomRearCircumferentialToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.BottomRearCircumferentialToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomRearCircumferentialToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.BottomRearCircumferentialToggleButton.OptionWidth = 50;
         this.BottomRearCircumferentialToggleButton.Size = new System.Drawing.Size(107, 90);
         this.BottomRearCircumferentialToggleButton.TabIndex = 154;
         this.BottomRearCircumferentialToggleButton.Text = "FORWARD CONTROL";
         this.BottomRearCircumferentialToggleButton.UseVisualStyleBackColor = false;
         this.BottomRearCircumferentialToggleButton.Click += new System.EventHandler(this.BottomRearCircumferentialToggleButton_Click);
         // 
         // BottomFrontCircumferentialToggleButton
         // 
         this.BottomFrontCircumferentialToggleButton.AutomaticToggle = true;
         this.BottomFrontCircumferentialToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BottomFrontCircumferentialToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BottomFrontCircumferentialToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BottomFrontCircumferentialToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.BottomFrontCircumferentialToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomFrontCircumferentialToggleButton.ForeColor = System.Drawing.Color.Black;
         this.BottomFrontCircumferentialToggleButton.HoldEnable = false;
         this.BottomFrontCircumferentialToggleButton.HoldTimeoutInterval = 0;
         this.BottomFrontCircumferentialToggleButton.Location = new System.Drawing.Point(301, 34);
         this.BottomFrontCircumferentialToggleButton.Name = "BottomFrontCircumferentialToggleButton";
         this.BottomFrontCircumferentialToggleButton.OptionASelected = false;
         this.BottomFrontCircumferentialToggleButton.OptionAText = "SPEED";
         this.BottomFrontCircumferentialToggleButton.OptionBSelected = true;
         this.BottomFrontCircumferentialToggleButton.OptionBText = "CURRENT";
         this.BottomFrontCircumferentialToggleButton.OptionCenterWidth = 0;
         this.BottomFrontCircumferentialToggleButton.OptionEdgeHeight = 8;
         this.BottomFrontCircumferentialToggleButton.OptionHeight = 22;
         this.BottomFrontCircumferentialToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.BottomFrontCircumferentialToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomFrontCircumferentialToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.BottomFrontCircumferentialToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.BottomFrontCircumferentialToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomFrontCircumferentialToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.BottomFrontCircumferentialToggleButton.OptionWidth = 50;
         this.BottomFrontCircumferentialToggleButton.Size = new System.Drawing.Size(107, 90);
         this.BottomFrontCircumferentialToggleButton.TabIndex = 153;
         this.BottomFrontCircumferentialToggleButton.Text = "FORWARD CONTROL";
         this.BottomFrontCircumferentialToggleButton.UseVisualStyleBackColor = false;
         this.BottomFrontCircumferentialToggleButton.Click += new System.EventHandler(this.BottomFrontCircumferentialToggleButton_Click);
         // 
         // TopRearCircumferentialToggleButton
         // 
         this.TopRearCircumferentialToggleButton.AutomaticToggle = true;
         this.TopRearCircumferentialToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.TopRearCircumferentialToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.TopRearCircumferentialToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.TopRearCircumferentialToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.TopRearCircumferentialToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopRearCircumferentialToggleButton.ForeColor = System.Drawing.Color.Black;
         this.TopRearCircumferentialToggleButton.HoldEnable = false;
         this.TopRearCircumferentialToggleButton.HoldTimeoutInterval = 0;
         this.TopRearCircumferentialToggleButton.Location = new System.Drawing.Point(156, 34);
         this.TopRearCircumferentialToggleButton.Name = "TopRearCircumferentialToggleButton";
         this.TopRearCircumferentialToggleButton.OptionASelected = false;
         this.TopRearCircumferentialToggleButton.OptionAText = "SPEED";
         this.TopRearCircumferentialToggleButton.OptionBSelected = true;
         this.TopRearCircumferentialToggleButton.OptionBText = "CURRENT";
         this.TopRearCircumferentialToggleButton.OptionCenterWidth = 0;
         this.TopRearCircumferentialToggleButton.OptionEdgeHeight = 8;
         this.TopRearCircumferentialToggleButton.OptionHeight = 22;
         this.TopRearCircumferentialToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.TopRearCircumferentialToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopRearCircumferentialToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.TopRearCircumferentialToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.TopRearCircumferentialToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopRearCircumferentialToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.TopRearCircumferentialToggleButton.OptionWidth = 50;
         this.TopRearCircumferentialToggleButton.Size = new System.Drawing.Size(107, 90);
         this.TopRearCircumferentialToggleButton.TabIndex = 152;
         this.TopRearCircumferentialToggleButton.Text = "FORWARD CONTROL";
         this.TopRearCircumferentialToggleButton.UseVisualStyleBackColor = false;
         this.TopRearCircumferentialToggleButton.Click += new System.EventHandler(this.TopRearCircumferentialToggleButton_Click);
         // 
         // TopFrontCircumferentialToggleButton
         // 
         this.TopFrontCircumferentialToggleButton.AutomaticToggle = true;
         this.TopFrontCircumferentialToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.TopFrontCircumferentialToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.TopFrontCircumferentialToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.TopFrontCircumferentialToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.TopFrontCircumferentialToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopFrontCircumferentialToggleButton.ForeColor = System.Drawing.Color.Black;
         this.TopFrontCircumferentialToggleButton.HoldEnable = false;
         this.TopFrontCircumferentialToggleButton.HoldTimeoutInterval = 0;
         this.TopFrontCircumferentialToggleButton.Location = new System.Drawing.Point(11, 34);
         this.TopFrontCircumferentialToggleButton.Name = "TopFrontCircumferentialToggleButton";
         this.TopFrontCircumferentialToggleButton.OptionASelected = false;
         this.TopFrontCircumferentialToggleButton.OptionAText = "SPEED";
         this.TopFrontCircumferentialToggleButton.OptionBSelected = true;
         this.TopFrontCircumferentialToggleButton.OptionBText = "CURRENT";
         this.TopFrontCircumferentialToggleButton.OptionCenterWidth = 0;
         this.TopFrontCircumferentialToggleButton.OptionEdgeHeight = 8;
         this.TopFrontCircumferentialToggleButton.OptionHeight = 22;
         this.TopFrontCircumferentialToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.TopFrontCircumferentialToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopFrontCircumferentialToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.TopFrontCircumferentialToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.TopFrontCircumferentialToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopFrontCircumferentialToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.TopFrontCircumferentialToggleButton.OptionWidth = 50;
         this.TopFrontCircumferentialToggleButton.Size = new System.Drawing.Size(107, 90);
         this.TopFrontCircumferentialToggleButton.TabIndex = 148;
         this.TopFrontCircumferentialToggleButton.Text = "FORWARD CONTROL";
         this.TopFrontCircumferentialToggleButton.UseVisualStyleBackColor = false;
         this.TopFrontCircumferentialToggleButton.Click += new System.EventHandler(this.TopFrontCircumferentialToggleButton_Click);
         // 
         // LaunchAxialModePanel
         // 
         this.LaunchAxialModePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(76)))), ((int)(((byte)(52)))));
         this.LaunchAxialModePanel.Controls.Add(this.transparentLabel4);
         this.LaunchAxialModePanel.Controls.Add(this.BottomRearLaunchAxialToggleButton);
         this.LaunchAxialModePanel.Controls.Add(this.BottomFrontLaunchAxialToggleButton);
         this.LaunchAxialModePanel.Controls.Add(this.TopRearLaunchAxialToggleButton);
         this.LaunchAxialModePanel.Controls.Add(this.TopFrontLaunchAxialToggleButton);
         this.LaunchAxialModePanel.EdgeWeight = 2;
         this.LaunchAxialModePanel.Location = new System.Drawing.Point(27, 751);
         this.LaunchAxialModePanel.Name = "LaunchAxialModePanel";
         this.LaunchAxialModePanel.Opacity = 50;
         this.LaunchAxialModePanel.Size = new System.Drawing.Size(564, 134);
         this.LaunchAxialModePanel.TabIndex = 4;
         // 
         // transparentLabel4
         // 
         this.transparentLabel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
         this.transparentLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.transparentLabel4.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
         this.transparentLabel4.Location = new System.Drawing.Point(2, 2);
         this.transparentLabel4.Name = "transparentLabel4";
         this.transparentLabel4.Opacity = 55;
         this.transparentLabel4.Size = new System.Drawing.Size(559, 24);
         this.transparentLabel4.TabIndex = 167;
         this.transparentLabel4.Text = "LAUNCH AXIAL MODE";
         this.transparentLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // BottomRearLaunchAxialToggleButton
         // 
         this.BottomRearLaunchAxialToggleButton.AutomaticToggle = true;
         this.BottomRearLaunchAxialToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BottomRearLaunchAxialToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BottomRearLaunchAxialToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BottomRearLaunchAxialToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.BottomRearLaunchAxialToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomRearLaunchAxialToggleButton.ForeColor = System.Drawing.Color.Black;
         this.BottomRearLaunchAxialToggleButton.HoldEnable = false;
         this.BottomRearLaunchAxialToggleButton.HoldTimeoutInterval = 0;
         this.BottomRearLaunchAxialToggleButton.Location = new System.Drawing.Point(446, 34);
         this.BottomRearLaunchAxialToggleButton.Name = "BottomRearLaunchAxialToggleButton";
         this.BottomRearLaunchAxialToggleButton.OptionASelected = false;
         this.BottomRearLaunchAxialToggleButton.OptionAText = "SPEED";
         this.BottomRearLaunchAxialToggleButton.OptionBSelected = true;
         this.BottomRearLaunchAxialToggleButton.OptionBText = "CURRENT";
         this.BottomRearLaunchAxialToggleButton.OptionCenterWidth = 0;
         this.BottomRearLaunchAxialToggleButton.OptionEdgeHeight = 8;
         this.BottomRearLaunchAxialToggleButton.OptionHeight = 22;
         this.BottomRearLaunchAxialToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.BottomRearLaunchAxialToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomRearLaunchAxialToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.BottomRearLaunchAxialToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.BottomRearLaunchAxialToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomRearLaunchAxialToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.BottomRearLaunchAxialToggleButton.OptionWidth = 50;
         this.BottomRearLaunchAxialToggleButton.Size = new System.Drawing.Size(107, 90);
         this.BottomRearLaunchAxialToggleButton.TabIndex = 154;
         this.BottomRearLaunchAxialToggleButton.Text = "FORWARD CONTROL";
         this.BottomRearLaunchAxialToggleButton.UseVisualStyleBackColor = false;
         this.BottomRearLaunchAxialToggleButton.Click += new System.EventHandler(this.BottomRearLaunchAxialToggleButton_Click);
         // 
         // BottomFrontLaunchAxialToggleButton
         // 
         this.BottomFrontLaunchAxialToggleButton.AutomaticToggle = true;
         this.BottomFrontLaunchAxialToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BottomFrontLaunchAxialToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BottomFrontLaunchAxialToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BottomFrontLaunchAxialToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.BottomFrontLaunchAxialToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomFrontLaunchAxialToggleButton.ForeColor = System.Drawing.Color.Black;
         this.BottomFrontLaunchAxialToggleButton.HoldEnable = false;
         this.BottomFrontLaunchAxialToggleButton.HoldTimeoutInterval = 0;
         this.BottomFrontLaunchAxialToggleButton.Location = new System.Drawing.Point(301, 34);
         this.BottomFrontLaunchAxialToggleButton.Name = "BottomFrontLaunchAxialToggleButton";
         this.BottomFrontLaunchAxialToggleButton.OptionASelected = false;
         this.BottomFrontLaunchAxialToggleButton.OptionAText = "SPEED";
         this.BottomFrontLaunchAxialToggleButton.OptionBSelected = true;
         this.BottomFrontLaunchAxialToggleButton.OptionBText = "CURRENT";
         this.BottomFrontLaunchAxialToggleButton.OptionCenterWidth = 0;
         this.BottomFrontLaunchAxialToggleButton.OptionEdgeHeight = 8;
         this.BottomFrontLaunchAxialToggleButton.OptionHeight = 22;
         this.BottomFrontLaunchAxialToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.BottomFrontLaunchAxialToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomFrontLaunchAxialToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.BottomFrontLaunchAxialToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.BottomFrontLaunchAxialToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomFrontLaunchAxialToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.BottomFrontLaunchAxialToggleButton.OptionWidth = 50;
         this.BottomFrontLaunchAxialToggleButton.Size = new System.Drawing.Size(107, 90);
         this.BottomFrontLaunchAxialToggleButton.TabIndex = 153;
         this.BottomFrontLaunchAxialToggleButton.Text = "FORWARD CONTROL";
         this.BottomFrontLaunchAxialToggleButton.UseVisualStyleBackColor = false;
         this.BottomFrontLaunchAxialToggleButton.Click += new System.EventHandler(this.BottomFrontLaunchAxialToggleButton_Click);
         // 
         // TopRearLaunchAxialToggleButton
         // 
         this.TopRearLaunchAxialToggleButton.AutomaticToggle = true;
         this.TopRearLaunchAxialToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.TopRearLaunchAxialToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.TopRearLaunchAxialToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.TopRearLaunchAxialToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.TopRearLaunchAxialToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopRearLaunchAxialToggleButton.ForeColor = System.Drawing.Color.Black;
         this.TopRearLaunchAxialToggleButton.HoldEnable = false;
         this.TopRearLaunchAxialToggleButton.HoldTimeoutInterval = 0;
         this.TopRearLaunchAxialToggleButton.Location = new System.Drawing.Point(156, 34);
         this.TopRearLaunchAxialToggleButton.Name = "TopRearLaunchAxialToggleButton";
         this.TopRearLaunchAxialToggleButton.OptionASelected = false;
         this.TopRearLaunchAxialToggleButton.OptionAText = "SPEED";
         this.TopRearLaunchAxialToggleButton.OptionBSelected = true;
         this.TopRearLaunchAxialToggleButton.OptionBText = "CURRENT";
         this.TopRearLaunchAxialToggleButton.OptionCenterWidth = 0;
         this.TopRearLaunchAxialToggleButton.OptionEdgeHeight = 8;
         this.TopRearLaunchAxialToggleButton.OptionHeight = 22;
         this.TopRearLaunchAxialToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.TopRearLaunchAxialToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopRearLaunchAxialToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.TopRearLaunchAxialToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.TopRearLaunchAxialToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopRearLaunchAxialToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.TopRearLaunchAxialToggleButton.OptionWidth = 50;
         this.TopRearLaunchAxialToggleButton.Size = new System.Drawing.Size(107, 90);
         this.TopRearLaunchAxialToggleButton.TabIndex = 152;
         this.TopRearLaunchAxialToggleButton.Text = "FORWARD CONTROL";
         this.TopRearLaunchAxialToggleButton.UseVisualStyleBackColor = false;
         this.TopRearLaunchAxialToggleButton.Click += new System.EventHandler(this.TopRearLaunchAxialToggleButton_Click);
         // 
         // TopFrontLaunchAxialToggleButton
         // 
         this.TopFrontLaunchAxialToggleButton.AutomaticToggle = true;
         this.TopFrontLaunchAxialToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.TopFrontLaunchAxialToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.TopFrontLaunchAxialToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.TopFrontLaunchAxialToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.TopFrontLaunchAxialToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopFrontLaunchAxialToggleButton.ForeColor = System.Drawing.Color.Black;
         this.TopFrontLaunchAxialToggleButton.HoldEnable = false;
         this.TopFrontLaunchAxialToggleButton.HoldTimeoutInterval = 0;
         this.TopFrontLaunchAxialToggleButton.Location = new System.Drawing.Point(11, 34);
         this.TopFrontLaunchAxialToggleButton.Name = "TopFrontLaunchAxialToggleButton";
         this.TopFrontLaunchAxialToggleButton.OptionASelected = false;
         this.TopFrontLaunchAxialToggleButton.OptionAText = "SPEED";
         this.TopFrontLaunchAxialToggleButton.OptionBSelected = true;
         this.TopFrontLaunchAxialToggleButton.OptionBText = "CURRENT";
         this.TopFrontLaunchAxialToggleButton.OptionCenterWidth = 0;
         this.TopFrontLaunchAxialToggleButton.OptionEdgeHeight = 8;
         this.TopFrontLaunchAxialToggleButton.OptionHeight = 22;
         this.TopFrontLaunchAxialToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.TopFrontLaunchAxialToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopFrontLaunchAxialToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.TopFrontLaunchAxialToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.TopFrontLaunchAxialToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopFrontLaunchAxialToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.TopFrontLaunchAxialToggleButton.OptionWidth = 50;
         this.TopFrontLaunchAxialToggleButton.Size = new System.Drawing.Size(107, 90);
         this.TopFrontLaunchAxialToggleButton.TabIndex = 148;
         this.TopFrontLaunchAxialToggleButton.Text = "FORWARD CONTROL";
         this.TopFrontLaunchAxialToggleButton.UseVisualStyleBackColor = false;
         this.TopFrontLaunchAxialToggleButton.Click += new System.EventHandler(this.TopFrontLaunchAxialToggleButton_Click);
         // 
         // LowSpeedScaleValueButton
         // 
         this.LowSpeedScaleValueButton.ArrowWidth = 0;
         this.LowSpeedScaleValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.LowSpeedScaleValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.LowSpeedScaleValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.LowSpeedScaleValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.LowSpeedScaleValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
         this.LowSpeedScaleValueButton.HoldTimeoutInterval = 0;
         this.LowSpeedScaleValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.LowSpeedScaleValueButton.LeftArrowVisible = false;
         this.LowSpeedScaleValueButton.Location = new System.Drawing.Point(607, 521);
         this.LowSpeedScaleValueButton.Name = "LowSpeedScaleValueButton";
         this.LowSpeedScaleValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.LowSpeedScaleValueButton.RightArrowVisible = false;
         this.LowSpeedScaleValueButton.Size = new System.Drawing.Size(107, 90);
         this.LowSpeedScaleValueButton.TabIndex = 150;
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
         // CurrentPer1KValueButton
         // 
         this.CurrentPer1KValueButton.ArrowWidth = 0;
         this.CurrentPer1KValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.CurrentPer1KValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.CurrentPer1KValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.CurrentPer1KValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.CurrentPer1KValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
         this.CurrentPer1KValueButton.HoldTimeoutInterval = 0;
         this.CurrentPer1KValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.CurrentPer1KValueButton.LeftArrowVisible = false;
         this.CurrentPer1KValueButton.Location = new System.Drawing.Point(607, 423);
         this.CurrentPer1KValueButton.Name = "CurrentPer1KValueButton";
         this.CurrentPer1KValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.CurrentPer1KValueButton.RightArrowVisible = false;
         this.CurrentPer1KValueButton.Size = new System.Drawing.Size(107, 90);
         this.CurrentPer1KValueButton.TabIndex = 149;
         this.CurrentPer1KValueButton.Text = "CURRENT PER 1K RPM";
         this.CurrentPer1KValueButton.UseVisualStyleBackColor = false;
         this.CurrentPer1KValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.CurrentPer1KValueButton.ValueEdgeHeight = 8;
         this.CurrentPer1KValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.CurrentPer1KValueButton.ValueForeColor = System.Drawing.Color.White;
         this.CurrentPer1KValueButton.ValueHeight = 22;
         this.CurrentPer1KValueButton.ValueText = "#.# A";
         this.CurrentPer1KValueButton.ValueWidth = 80;
         this.CurrentPer1KValueButton.Click += new System.EventHandler(this.CurrentPer1KValueButton_Click);
         // 
         // MaxSpeedValueButton
         // 
         this.MaxSpeedValueButton.ArrowWidth = 0;
         this.MaxSpeedValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.MaxSpeedValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.MaxSpeedValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.MaxSpeedValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.MaxSpeedValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
         this.MaxSpeedValueButton.HoldTimeoutInterval = 0;
         this.MaxSpeedValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.MaxSpeedValueButton.LeftArrowVisible = false;
         this.MaxSpeedValueButton.Location = new System.Drawing.Point(607, 325);
         this.MaxSpeedValueButton.Name = "MaxSpeedValueButton";
         this.MaxSpeedValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.MaxSpeedValueButton.RightArrowVisible = false;
         this.MaxSpeedValueButton.Size = new System.Drawing.Size(107, 90);
         this.MaxSpeedValueButton.TabIndex = 148;
         this.MaxSpeedValueButton.Text = "MAX     SPEED";
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
         // MaxCurrentValueButton
         // 
         this.MaxCurrentValueButton.ArrowWidth = 0;
         this.MaxCurrentValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.MaxCurrentValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.MaxCurrentValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.MaxCurrentValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.MaxCurrentValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
         this.MaxCurrentValueButton.HoldTimeoutInterval = 0;
         this.MaxCurrentValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.MaxCurrentValueButton.LeftArrowVisible = false;
         this.MaxCurrentValueButton.Location = new System.Drawing.Point(607, 227);
         this.MaxCurrentValueButton.Name = "MaxCurrentValueButton";
         this.MaxCurrentValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.MaxCurrentValueButton.RightArrowVisible = false;
         this.MaxCurrentValueButton.Size = new System.Drawing.Size(107, 90);
         this.MaxCurrentValueButton.TabIndex = 147;
         this.MaxCurrentValueButton.Text = "MAX CURRENT";
         this.MaxCurrentValueButton.UseVisualStyleBackColor = false;
         this.MaxCurrentValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.MaxCurrentValueButton.ValueEdgeHeight = 8;
         this.MaxCurrentValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.MaxCurrentValueButton.ValueForeColor = System.Drawing.Color.White;
         this.MaxCurrentValueButton.ValueHeight = 22;
         this.MaxCurrentValueButton.ValueText = "#.# A";
         this.MaxCurrentValueButton.ValueWidth = 80;
         this.MaxCurrentValueButton.Click += new System.EventHandler(this.MaxCurrentValueButton_Click);
         // 
         // LockCurrentValueButton
         // 
         this.LockCurrentValueButton.ArrowWidth = 0;
         this.LockCurrentValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.LockCurrentValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.LockCurrentValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.LockCurrentValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.LockCurrentValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
         this.LockCurrentValueButton.HoldTimeoutInterval = 0;
         this.LockCurrentValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.LockCurrentValueButton.LeftArrowVisible = false;
         this.LockCurrentValueButton.Location = new System.Drawing.Point(607, 129);
         this.LockCurrentValueButton.Name = "LockCurrentValueButton";
         this.LockCurrentValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.LockCurrentValueButton.RightArrowVisible = false;
         this.LockCurrentValueButton.Size = new System.Drawing.Size(107, 90);
         this.LockCurrentValueButton.TabIndex = 146;
         this.LockCurrentValueButton.Text = "LOCK CURRENT";
         this.LockCurrentValueButton.UseVisualStyleBackColor = false;
         this.LockCurrentValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.LockCurrentValueButton.ValueEdgeHeight = 8;
         this.LockCurrentValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.LockCurrentValueButton.ValueForeColor = System.Drawing.Color.White;
         this.LockCurrentValueButton.ValueHeight = 22;
         this.LockCurrentValueButton.ValueText = "#.# A";
         this.LockCurrentValueButton.ValueWidth = 80;
         this.LockCurrentValueButton.Click += new System.EventHandler(this.LockCurrentValueButton_Click);
         // 
         // TitleLabel
         // 
         this.TitleLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.TitleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TitleLabel.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
         this.TitleLabel.Location = new System.Drawing.Point(16, 16);
         this.TitleLabel.Name = "TitleLabel";
         this.TitleLabel.Size = new System.Drawing.Size(709, 36);
         this.TitleLabel.TabIndex = 132;
         this.TitleLabel.Text = "MOTOR SETUP";
         this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // NormalAxialModePanel
         // 
         this.NormalAxialModePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(76)))), ((int)(((byte)(52)))));
         this.NormalAxialModePanel.Controls.Add(this.transparentLabel2);
         this.NormalAxialModePanel.Controls.Add(this.BottomRearNormalAxialModeToggleButton);
         this.NormalAxialModePanel.Controls.Add(this.BottomFrontNormalAxialModeToggleButton);
         this.NormalAxialModePanel.Controls.Add(this.TopRearNormalAxialModeToggleButton);
         this.NormalAxialModePanel.Controls.Add(this.TopFrontNormalAxialModeToggleButton);
         this.NormalAxialModePanel.EdgeWeight = 2;
         this.NormalAxialModePanel.Location = new System.Drawing.Point(27, 325);
         this.NormalAxialModePanel.Name = "NormalAxialModePanel";
         this.NormalAxialModePanel.Opacity = 50;
         this.NormalAxialModePanel.Size = new System.Drawing.Size(564, 134);
         this.NormalAxialModePanel.TabIndex = 1;
         // 
         // transparentLabel2
         // 
         this.transparentLabel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
         this.transparentLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.transparentLabel2.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
         this.transparentLabel2.Location = new System.Drawing.Point(2, 2);
         this.transparentLabel2.Name = "transparentLabel2";
         this.transparentLabel2.Opacity = 55;
         this.transparentLabel2.Size = new System.Drawing.Size(559, 24);
         this.transparentLabel2.TabIndex = 166;
         this.transparentLabel2.Text = "NORMAL AXIAL MODE";
         this.transparentLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // BottomRearNormalAxialModeToggleButton
         // 
         this.BottomRearNormalAxialModeToggleButton.AutomaticToggle = true;
         this.BottomRearNormalAxialModeToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BottomRearNormalAxialModeToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BottomRearNormalAxialModeToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BottomRearNormalAxialModeToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.BottomRearNormalAxialModeToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomRearNormalAxialModeToggleButton.ForeColor = System.Drawing.Color.Black;
         this.BottomRearNormalAxialModeToggleButton.HoldEnable = false;
         this.BottomRearNormalAxialModeToggleButton.HoldTimeoutInterval = 0;
         this.BottomRearNormalAxialModeToggleButton.Location = new System.Drawing.Point(446, 34);
         this.BottomRearNormalAxialModeToggleButton.Name = "BottomRearNormalAxialModeToggleButton";
         this.BottomRearNormalAxialModeToggleButton.OptionASelected = false;
         this.BottomRearNormalAxialModeToggleButton.OptionAText = "SPEED";
         this.BottomRearNormalAxialModeToggleButton.OptionBSelected = true;
         this.BottomRearNormalAxialModeToggleButton.OptionBText = "CURRENT";
         this.BottomRearNormalAxialModeToggleButton.OptionCenterWidth = 0;
         this.BottomRearNormalAxialModeToggleButton.OptionEdgeHeight = 8;
         this.BottomRearNormalAxialModeToggleButton.OptionHeight = 22;
         this.BottomRearNormalAxialModeToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.BottomRearNormalAxialModeToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomRearNormalAxialModeToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.BottomRearNormalAxialModeToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.BottomRearNormalAxialModeToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomRearNormalAxialModeToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.BottomRearNormalAxialModeToggleButton.OptionWidth = 50;
         this.BottomRearNormalAxialModeToggleButton.Size = new System.Drawing.Size(107, 90);
         this.BottomRearNormalAxialModeToggleButton.TabIndex = 151;
         this.BottomRearNormalAxialModeToggleButton.Text = "FORWARD CONTROL";
         this.BottomRearNormalAxialModeToggleButton.UseVisualStyleBackColor = false;
         this.BottomRearNormalAxialModeToggleButton.Click += new System.EventHandler(this.BottomRearNormalAxialModeToggleButton_Click);
         // 
         // BottomFrontNormalAxialModeToggleButton
         // 
         this.BottomFrontNormalAxialModeToggleButton.AutomaticToggle = true;
         this.BottomFrontNormalAxialModeToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BottomFrontNormalAxialModeToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BottomFrontNormalAxialModeToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BottomFrontNormalAxialModeToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.BottomFrontNormalAxialModeToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomFrontNormalAxialModeToggleButton.ForeColor = System.Drawing.Color.Black;
         this.BottomFrontNormalAxialModeToggleButton.HoldEnable = false;
         this.BottomFrontNormalAxialModeToggleButton.HoldTimeoutInterval = 0;
         this.BottomFrontNormalAxialModeToggleButton.Location = new System.Drawing.Point(301, 34);
         this.BottomFrontNormalAxialModeToggleButton.Name = "BottomFrontNormalAxialModeToggleButton";
         this.BottomFrontNormalAxialModeToggleButton.OptionASelected = false;
         this.BottomFrontNormalAxialModeToggleButton.OptionAText = "SPEED";
         this.BottomFrontNormalAxialModeToggleButton.OptionBSelected = true;
         this.BottomFrontNormalAxialModeToggleButton.OptionBText = "CURRENT";
         this.BottomFrontNormalAxialModeToggleButton.OptionCenterWidth = 0;
         this.BottomFrontNormalAxialModeToggleButton.OptionEdgeHeight = 8;
         this.BottomFrontNormalAxialModeToggleButton.OptionHeight = 22;
         this.BottomFrontNormalAxialModeToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.BottomFrontNormalAxialModeToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomFrontNormalAxialModeToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.BottomFrontNormalAxialModeToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.BottomFrontNormalAxialModeToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomFrontNormalAxialModeToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.BottomFrontNormalAxialModeToggleButton.OptionWidth = 50;
         this.BottomFrontNormalAxialModeToggleButton.Size = new System.Drawing.Size(107, 90);
         this.BottomFrontNormalAxialModeToggleButton.TabIndex = 150;
         this.BottomFrontNormalAxialModeToggleButton.Text = "FORWARD CONTROL";
         this.BottomFrontNormalAxialModeToggleButton.UseVisualStyleBackColor = false;
         this.BottomFrontNormalAxialModeToggleButton.Click += new System.EventHandler(this.BottomFrontNormalAxialModeToggleButton_Click);
         // 
         // TopRearNormalAxialModeToggleButton
         // 
         this.TopRearNormalAxialModeToggleButton.AutomaticToggle = true;
         this.TopRearNormalAxialModeToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.TopRearNormalAxialModeToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.TopRearNormalAxialModeToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.TopRearNormalAxialModeToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.TopRearNormalAxialModeToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopRearNormalAxialModeToggleButton.ForeColor = System.Drawing.Color.Black;
         this.TopRearNormalAxialModeToggleButton.HoldEnable = false;
         this.TopRearNormalAxialModeToggleButton.HoldTimeoutInterval = 0;
         this.TopRearNormalAxialModeToggleButton.Location = new System.Drawing.Point(156, 34);
         this.TopRearNormalAxialModeToggleButton.Name = "TopRearNormalAxialModeToggleButton";
         this.TopRearNormalAxialModeToggleButton.OptionASelected = false;
         this.TopRearNormalAxialModeToggleButton.OptionAText = "SPEED";
         this.TopRearNormalAxialModeToggleButton.OptionBSelected = true;
         this.TopRearNormalAxialModeToggleButton.OptionBText = "CURRENT";
         this.TopRearNormalAxialModeToggleButton.OptionCenterWidth = 0;
         this.TopRearNormalAxialModeToggleButton.OptionEdgeHeight = 8;
         this.TopRearNormalAxialModeToggleButton.OptionHeight = 22;
         this.TopRearNormalAxialModeToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.TopRearNormalAxialModeToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopRearNormalAxialModeToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.TopRearNormalAxialModeToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.TopRearNormalAxialModeToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopRearNormalAxialModeToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.TopRearNormalAxialModeToggleButton.OptionWidth = 50;
         this.TopRearNormalAxialModeToggleButton.Size = new System.Drawing.Size(107, 90);
         this.TopRearNormalAxialModeToggleButton.TabIndex = 149;
         this.TopRearNormalAxialModeToggleButton.Text = "FORWARD CONTROL";
         this.TopRearNormalAxialModeToggleButton.UseVisualStyleBackColor = false;
         this.TopRearNormalAxialModeToggleButton.Click += new System.EventHandler(this.TopRearNormalAxialModeToggleButton_Click);
         // 
         // TopFrontNormalAxialModeToggleButton
         // 
         this.TopFrontNormalAxialModeToggleButton.AutomaticToggle = true;
         this.TopFrontNormalAxialModeToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.TopFrontNormalAxialModeToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.TopFrontNormalAxialModeToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.TopFrontNormalAxialModeToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.TopFrontNormalAxialModeToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopFrontNormalAxialModeToggleButton.ForeColor = System.Drawing.Color.Black;
         this.TopFrontNormalAxialModeToggleButton.HoldEnable = false;
         this.TopFrontNormalAxialModeToggleButton.HoldTimeoutInterval = 0;
         this.TopFrontNormalAxialModeToggleButton.Location = new System.Drawing.Point(11, 34);
         this.TopFrontNormalAxialModeToggleButton.Name = "TopFrontNormalAxialModeToggleButton";
         this.TopFrontNormalAxialModeToggleButton.OptionASelected = false;
         this.TopFrontNormalAxialModeToggleButton.OptionAText = "SPEED";
         this.TopFrontNormalAxialModeToggleButton.OptionBSelected = true;
         this.TopFrontNormalAxialModeToggleButton.OptionBText = "CURRENT";
         this.TopFrontNormalAxialModeToggleButton.OptionCenterWidth = 0;
         this.TopFrontNormalAxialModeToggleButton.OptionEdgeHeight = 8;
         this.TopFrontNormalAxialModeToggleButton.OptionHeight = 22;
         this.TopFrontNormalAxialModeToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.TopFrontNormalAxialModeToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopFrontNormalAxialModeToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.TopFrontNormalAxialModeToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.TopFrontNormalAxialModeToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopFrontNormalAxialModeToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.TopFrontNormalAxialModeToggleButton.OptionWidth = 50;
         this.TopFrontNormalAxialModeToggleButton.Size = new System.Drawing.Size(107, 90);
         this.TopFrontNormalAxialModeToggleButton.TabIndex = 148;
         this.TopFrontNormalAxialModeToggleButton.Text = "FORWARD CONTROL";
         this.TopFrontNormalAxialModeToggleButton.UseVisualStyleBackColor = false;
         this.TopFrontNormalAxialModeToggleButton.Click += new System.EventHandler(this.TopFrontNormalAxialModeToggleButton_Click);
         // 
         // borderedPanel4
         // 
         this.borderedPanel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.borderedPanel4.Controls.Add(this.BottomRearStateCycleButton);
         this.borderedPanel4.Controls.Add(this.label1);
         this.borderedPanel4.Controls.Add(this.BottomRearDirectionToggleButton);
         this.borderedPanel4.EdgeWeight = 2;
         this.borderedPanel4.Location = new System.Drawing.Point(462, 68);
         this.borderedPanel4.Name = "borderedPanel4";
         this.borderedPanel4.Size = new System.Drawing.Size(129, 817);
         this.borderedPanel4.TabIndex = 161;
         // 
         // BottomRearStateCycleButton
         // 
         this.BottomRearStateCycleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BottomRearStateCycleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BottomRearStateCycleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BottomRearStateCycleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.BottomRearStateCycleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomRearStateCycleButton.HoldEnable = false;
         this.BottomRearStateCycleButton.HoldTimeoutInterval = 0;
         this.BottomRearStateCycleButton.Location = new System.Drawing.Point(11, 61);
         this.BottomRearStateCycleButton.Name = "BottomRearStateCycleButton";
         this.BottomRearStateCycleButton.OptionAText = "ENABLE";
         this.BottomRearStateCycleButton.OptionBText = "DISABLE";
         this.BottomRearStateCycleButton.OptionCText = "LOCK";
         this.BottomRearStateCycleButton.OptionEdgeSpace = 8;
         this.BottomRearStateCycleButton.OptionHeight = 18;
         this.BottomRearStateCycleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.BottomRearStateCycleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
         this.BottomRearStateCycleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.BottomRearStateCycleButton.OptionOptionSpace = 2;
         this.BottomRearStateCycleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.BottomRearStateCycleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.BottomRearStateCycleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.BottomRearStateCycleButton.OptionWidth = 80;
         this.BottomRearStateCycleButton.SelectedOption = 3;
         this.BottomRearStateCycleButton.Size = new System.Drawing.Size(107, 113);
         this.BottomRearStateCycleButton.TabIndex = 150;
         this.BottomRearStateCycleButton.Text = "STATE";
         this.BottomRearStateCycleButton.UseVisualStyleBackColor = false;
         this.BottomRearStateCycleButton.Click += new System.EventHandler(this.BottomRearStateCycleButton_Click);
         // 
         // label1
         // 
         this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
         this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label1.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
         this.label1.Location = new System.Drawing.Point(2, 2);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(124, 51);
         this.label1.TabIndex = 153;
         this.label1.Text = "REAR LOWER";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // BottomRearDirectionToggleButton
         // 
         this.BottomRearDirectionToggleButton.AutomaticToggle = true;
         this.BottomRearDirectionToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BottomRearDirectionToggleButton.DisabledBackColor = System.Drawing.Color.Black;
         this.BottomRearDirectionToggleButton.DisabledForeColor = System.Drawing.Color.Gray;
         this.BottomRearDirectionToggleButton.DisabledOptionBackColor = System.Drawing.Color.Black;
         this.BottomRearDirectionToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomRearDirectionToggleButton.HoldEnable = false;
         this.BottomRearDirectionToggleButton.HoldTimeoutInterval = 0;
         this.BottomRearDirectionToggleButton.Location = new System.Drawing.Point(11, 182);
         this.BottomRearDirectionToggleButton.Name = "BottomRearDirectionToggleButton";
         this.BottomRearDirectionToggleButton.OptionASelected = true;
         this.BottomRearDirectionToggleButton.OptionAText = "NORMAL";
         this.BottomRearDirectionToggleButton.OptionBSelected = false;
         this.BottomRearDirectionToggleButton.OptionBText = "INVERSE";
         this.BottomRearDirectionToggleButton.OptionCenterWidth = 0;
         this.BottomRearDirectionToggleButton.OptionEdgeHeight = 8;
         this.BottomRearDirectionToggleButton.OptionHeight = 22;
         this.BottomRearDirectionToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.BottomRearDirectionToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomRearDirectionToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.BottomRearDirectionToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.BottomRearDirectionToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomRearDirectionToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.BottomRearDirectionToggleButton.OptionWidth = 50;
         this.BottomRearDirectionToggleButton.Size = new System.Drawing.Size(107, 67);
         this.BottomRearDirectionToggleButton.TabIndex = 152;
         this.BottomRearDirectionToggleButton.Text = "DIRECTION";
         this.BottomRearDirectionToggleButton.UseVisualStyleBackColor = false;
         this.BottomRearDirectionToggleButton.Click += new System.EventHandler(this.BottomRearDirectionToggleButton_Click);
         // 
         // borderedPanel3
         // 
         this.borderedPanel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.borderedPanel3.Controls.Add(this.BottomFrontStateCycleButton);
         this.borderedPanel3.Controls.Add(this.label5);
         this.borderedPanel3.Controls.Add(this.BottomFrontDirectionToggleButton);
         this.borderedPanel3.EdgeWeight = 2;
         this.borderedPanel3.Location = new System.Drawing.Point(317, 68);
         this.borderedPanel3.Name = "borderedPanel3";
         this.borderedPanel3.Size = new System.Drawing.Size(129, 817);
         this.borderedPanel3.TabIndex = 162;
         // 
         // BottomFrontStateCycleButton
         // 
         this.BottomFrontStateCycleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BottomFrontStateCycleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BottomFrontStateCycleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BottomFrontStateCycleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.BottomFrontStateCycleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomFrontStateCycleButton.HoldEnable = false;
         this.BottomFrontStateCycleButton.HoldTimeoutInterval = 0;
         this.BottomFrontStateCycleButton.Location = new System.Drawing.Point(11, 61);
         this.BottomFrontStateCycleButton.Name = "BottomFrontStateCycleButton";
         this.BottomFrontStateCycleButton.OptionAText = "ENABLE";
         this.BottomFrontStateCycleButton.OptionBText = "DISABLE";
         this.BottomFrontStateCycleButton.OptionCText = "LOCK";
         this.BottomFrontStateCycleButton.OptionEdgeSpace = 8;
         this.BottomFrontStateCycleButton.OptionHeight = 18;
         this.BottomFrontStateCycleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.BottomFrontStateCycleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
         this.BottomFrontStateCycleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.BottomFrontStateCycleButton.OptionOptionSpace = 2;
         this.BottomFrontStateCycleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.BottomFrontStateCycleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.BottomFrontStateCycleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.BottomFrontStateCycleButton.OptionWidth = 80;
         this.BottomFrontStateCycleButton.SelectedOption = 3;
         this.BottomFrontStateCycleButton.Size = new System.Drawing.Size(107, 113);
         this.BottomFrontStateCycleButton.TabIndex = 150;
         this.BottomFrontStateCycleButton.Text = "STATE";
         this.BottomFrontStateCycleButton.UseVisualStyleBackColor = false;
         this.BottomFrontStateCycleButton.Click += new System.EventHandler(this.BottomFrontStateCycleButton_Click);
         // 
         // label5
         // 
         this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
         this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label5.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
         this.label5.Location = new System.Drawing.Point(2, 2);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(124, 51);
         this.label5.TabIndex = 153;
         this.label5.Text = "FRONT LOWER";
         this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // BottomFrontDirectionToggleButton
         // 
         this.BottomFrontDirectionToggleButton.AutomaticToggle = true;
         this.BottomFrontDirectionToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BottomFrontDirectionToggleButton.DisabledBackColor = System.Drawing.Color.Black;
         this.BottomFrontDirectionToggleButton.DisabledForeColor = System.Drawing.Color.Gray;
         this.BottomFrontDirectionToggleButton.DisabledOptionBackColor = System.Drawing.Color.Black;
         this.BottomFrontDirectionToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomFrontDirectionToggleButton.HoldEnable = false;
         this.BottomFrontDirectionToggleButton.HoldTimeoutInterval = 0;
         this.BottomFrontDirectionToggleButton.Location = new System.Drawing.Point(11, 182);
         this.BottomFrontDirectionToggleButton.Name = "BottomFrontDirectionToggleButton";
         this.BottomFrontDirectionToggleButton.OptionASelected = true;
         this.BottomFrontDirectionToggleButton.OptionAText = "NORMAL";
         this.BottomFrontDirectionToggleButton.OptionBSelected = false;
         this.BottomFrontDirectionToggleButton.OptionBText = "INVERSE";
         this.BottomFrontDirectionToggleButton.OptionCenterWidth = 0;
         this.BottomFrontDirectionToggleButton.OptionEdgeHeight = 8;
         this.BottomFrontDirectionToggleButton.OptionHeight = 22;
         this.BottomFrontDirectionToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.BottomFrontDirectionToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomFrontDirectionToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.BottomFrontDirectionToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.BottomFrontDirectionToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomFrontDirectionToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.BottomFrontDirectionToggleButton.OptionWidth = 50;
         this.BottomFrontDirectionToggleButton.Size = new System.Drawing.Size(107, 67);
         this.BottomFrontDirectionToggleButton.TabIndex = 152;
         this.BottomFrontDirectionToggleButton.Text = "DIRECTION";
         this.BottomFrontDirectionToggleButton.UseVisualStyleBackColor = false;
         this.BottomFrontDirectionToggleButton.Click += new System.EventHandler(this.BottomFrontDirectionToggleButton_Click);
         // 
         // borderedPanel1
         // 
         this.borderedPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.borderedPanel1.Controls.Add(this.TopRearStateCycleButton);
         this.borderedPanel1.Controls.Add(this.label6);
         this.borderedPanel1.Controls.Add(this.TopRearDirectionToggleButton);
         this.borderedPanel1.EdgeWeight = 2;
         this.borderedPanel1.Location = new System.Drawing.Point(172, 68);
         this.borderedPanel1.Name = "borderedPanel1";
         this.borderedPanel1.Size = new System.Drawing.Size(129, 817);
         this.borderedPanel1.TabIndex = 163;
         // 
         // TopRearStateCycleButton
         // 
         this.TopRearStateCycleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.TopRearStateCycleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.TopRearStateCycleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.TopRearStateCycleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.TopRearStateCycleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopRearStateCycleButton.HoldEnable = false;
         this.TopRearStateCycleButton.HoldTimeoutInterval = 0;
         this.TopRearStateCycleButton.Location = new System.Drawing.Point(11, 61);
         this.TopRearStateCycleButton.Name = "TopRearStateCycleButton";
         this.TopRearStateCycleButton.OptionAText = "ENABLE";
         this.TopRearStateCycleButton.OptionBText = "DISABLE";
         this.TopRearStateCycleButton.OptionCText = "LOCK";
         this.TopRearStateCycleButton.OptionEdgeSpace = 8;
         this.TopRearStateCycleButton.OptionHeight = 18;
         this.TopRearStateCycleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.TopRearStateCycleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
         this.TopRearStateCycleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.TopRearStateCycleButton.OptionOptionSpace = 2;
         this.TopRearStateCycleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.TopRearStateCycleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.TopRearStateCycleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.TopRearStateCycleButton.OptionWidth = 80;
         this.TopRearStateCycleButton.SelectedOption = 3;
         this.TopRearStateCycleButton.Size = new System.Drawing.Size(107, 113);
         this.TopRearStateCycleButton.TabIndex = 150;
         this.TopRearStateCycleButton.Text = "STATE";
         this.TopRearStateCycleButton.UseVisualStyleBackColor = false;
         this.TopRearStateCycleButton.Click += new System.EventHandler(this.TopRearStateCycleButton_Click);
         // 
         // label6
         // 
         this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
         this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label6.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
         this.label6.Location = new System.Drawing.Point(2, 2);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(124, 51);
         this.label6.TabIndex = 153;
         this.label6.Text = "REAR UPPER";
         this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // TopRearDirectionToggleButton
         // 
         this.TopRearDirectionToggleButton.AutomaticToggle = true;
         this.TopRearDirectionToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.TopRearDirectionToggleButton.DisabledBackColor = System.Drawing.Color.Black;
         this.TopRearDirectionToggleButton.DisabledForeColor = System.Drawing.Color.Gray;
         this.TopRearDirectionToggleButton.DisabledOptionBackColor = System.Drawing.Color.Black;
         this.TopRearDirectionToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopRearDirectionToggleButton.HoldEnable = false;
         this.TopRearDirectionToggleButton.HoldTimeoutInterval = 0;
         this.TopRearDirectionToggleButton.Location = new System.Drawing.Point(11, 182);
         this.TopRearDirectionToggleButton.Name = "TopRearDirectionToggleButton";
         this.TopRearDirectionToggleButton.OptionASelected = true;
         this.TopRearDirectionToggleButton.OptionAText = "NORMAL";
         this.TopRearDirectionToggleButton.OptionBSelected = false;
         this.TopRearDirectionToggleButton.OptionBText = "INVERSE";
         this.TopRearDirectionToggleButton.OptionCenterWidth = 0;
         this.TopRearDirectionToggleButton.OptionEdgeHeight = 8;
         this.TopRearDirectionToggleButton.OptionHeight = 22;
         this.TopRearDirectionToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.TopRearDirectionToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopRearDirectionToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.TopRearDirectionToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.TopRearDirectionToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopRearDirectionToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.TopRearDirectionToggleButton.OptionWidth = 50;
         this.TopRearDirectionToggleButton.Size = new System.Drawing.Size(107, 67);
         this.TopRearDirectionToggleButton.TabIndex = 152;
         this.TopRearDirectionToggleButton.Text = "DIRECTION";
         this.TopRearDirectionToggleButton.UseVisualStyleBackColor = false;
         this.TopRearDirectionToggleButton.Click += new System.EventHandler(this.TopRearDirectionToggleButton_Click);
         // 
         // borderedPanel2
         // 
         this.borderedPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.borderedPanel2.Controls.Add(this.TopFrontStateCycleButton);
         this.borderedPanel2.Controls.Add(this.label7);
         this.borderedPanel2.Controls.Add(this.TopFrontDirectionToggleButton);
         this.borderedPanel2.EdgeWeight = 2;
         this.borderedPanel2.Location = new System.Drawing.Point(27, 68);
         this.borderedPanel2.Name = "borderedPanel2";
         this.borderedPanel2.Size = new System.Drawing.Size(129, 817);
         this.borderedPanel2.TabIndex = 164;
         // 
         // TopFrontStateCycleButton
         // 
         this.TopFrontStateCycleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.TopFrontStateCycleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.TopFrontStateCycleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.TopFrontStateCycleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.TopFrontStateCycleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopFrontStateCycleButton.HoldEnable = false;
         this.TopFrontStateCycleButton.HoldTimeoutInterval = 0;
         this.TopFrontStateCycleButton.Location = new System.Drawing.Point(11, 61);
         this.TopFrontStateCycleButton.Name = "TopFrontStateCycleButton";
         this.TopFrontStateCycleButton.OptionAText = "ENABLE";
         this.TopFrontStateCycleButton.OptionBText = "DISABLE";
         this.TopFrontStateCycleButton.OptionCText = "LOCK";
         this.TopFrontStateCycleButton.OptionEdgeSpace = 8;
         this.TopFrontStateCycleButton.OptionHeight = 18;
         this.TopFrontStateCycleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.TopFrontStateCycleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
         this.TopFrontStateCycleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.TopFrontStateCycleButton.OptionOptionSpace = 2;
         this.TopFrontStateCycleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.TopFrontStateCycleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.TopFrontStateCycleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.TopFrontStateCycleButton.OptionWidth = 80;
         this.TopFrontStateCycleButton.SelectedOption = 3;
         this.TopFrontStateCycleButton.Size = new System.Drawing.Size(107, 113);
         this.TopFrontStateCycleButton.TabIndex = 150;
         this.TopFrontStateCycleButton.Text = "STATE";
         this.TopFrontStateCycleButton.UseVisualStyleBackColor = false;
         this.TopFrontStateCycleButton.Click += new System.EventHandler(this.TopFrontStateCycleButton_Click);
         // 
         // label7
         // 
         this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
         this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label7.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
         this.label7.Location = new System.Drawing.Point(2, 2);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(124, 51);
         this.label7.TabIndex = 153;
         this.label7.Text = "FRONT UPPER";
         this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // TopFrontDirectionToggleButton
         // 
         this.TopFrontDirectionToggleButton.AutomaticToggle = true;
         this.TopFrontDirectionToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.TopFrontDirectionToggleButton.DisabledBackColor = System.Drawing.Color.Black;
         this.TopFrontDirectionToggleButton.DisabledForeColor = System.Drawing.Color.Gray;
         this.TopFrontDirectionToggleButton.DisabledOptionBackColor = System.Drawing.Color.Black;
         this.TopFrontDirectionToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopFrontDirectionToggleButton.HoldEnable = false;
         this.TopFrontDirectionToggleButton.HoldTimeoutInterval = 0;
         this.TopFrontDirectionToggleButton.Location = new System.Drawing.Point(11, 182);
         this.TopFrontDirectionToggleButton.Name = "TopFrontDirectionToggleButton";
         this.TopFrontDirectionToggleButton.OptionASelected = true;
         this.TopFrontDirectionToggleButton.OptionAText = "NORMAL";
         this.TopFrontDirectionToggleButton.OptionBSelected = false;
         this.TopFrontDirectionToggleButton.OptionBText = "INVERSE";
         this.TopFrontDirectionToggleButton.OptionCenterWidth = 0;
         this.TopFrontDirectionToggleButton.OptionEdgeHeight = 8;
         this.TopFrontDirectionToggleButton.OptionHeight = 22;
         this.TopFrontDirectionToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.TopFrontDirectionToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopFrontDirectionToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.TopFrontDirectionToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.TopFrontDirectionToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopFrontDirectionToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.TopFrontDirectionToggleButton.OptionWidth = 50;
         this.TopFrontDirectionToggleButton.Size = new System.Drawing.Size(107, 67);
         this.TopFrontDirectionToggleButton.TabIndex = 152;
         this.TopFrontDirectionToggleButton.Text = "DIRECTION";
         this.TopFrontDirectionToggleButton.UseVisualStyleBackColor = false;
         this.TopFrontDirectionToggleButton.Click += new System.EventHandler(this.TopFrontDirectionToggleButton_Click);
         // 
         // MovementSetupForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Control;
         this.ClientSize = new System.Drawing.Size(741, 901);
         this.Controls.Add(this.MainPanel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "MovementSetupForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "MotorSetupForm";
         this.Shown += new System.EventHandler(this.MotorSetupForm_Shown);
         this.MainPanel.ResumeLayout(false);
         this.CornerAxialModePanel.ResumeLayout(false);
         this.CircumferentialModePanel.ResumeLayout(false);
         this.LaunchAxialModePanel.ResumeLayout(false);
         this.NormalAxialModePanel.ResumeLayout(false);
         this.borderedPanel4.ResumeLayout(false);
         this.borderedPanel3.ResumeLayout(false);
         this.borderedPanel1.ResumeLayout(false);
         this.borderedPanel2.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private BorderedPanel MainPanel;
      private System.Windows.Forms.Label TitleLabel;
      private TransparentPanel CircumferentialModePanel;
      private ValueToggleButton TopFrontCircumferentialToggleButton;
      private ValueButton LowSpeedScaleValueButton;
      private ValueButton CurrentPer1KValueButton;
      private ValueButton MaxSpeedValueButton;
      private ValueButton MaxCurrentValueButton;
      private ValueButton LockCurrentValueButton;
      private TransparentPanel LaunchAxialModePanel;
      private ValueToggleButton TopFrontLaunchAxialToggleButton;
      private TransparentPanel CornerAxialModePanel;
      private ValueToggleButton TopFrontCornerAxialToggleButton;
      private TransparentPanel NormalAxialModePanel;
      private ValueToggleButton TopFrontNormalAxialModeToggleButton;
      private ValueToggleButton BottomRearNormalAxialModeToggleButton;
      private ValueToggleButton BottomFrontNormalAxialModeToggleButton;
      private ValueToggleButton TopRearNormalAxialModeToggleButton;
      private ValueCycleButton BottomRearStateCycleButton;
      private ValueToggleButton BottomRearDirectionToggleButton;
      private BorderedPanel borderedPanel4;
      private System.Windows.Forms.Label label1;
      private BorderedPanel borderedPanel3;
      private ValueCycleButton BottomFrontStateCycleButton;
      private System.Windows.Forms.Label label5;
      private ValueToggleButton BottomFrontDirectionToggleButton;
      private ValueToggleButton BottomRearCornerAxialToggleButton;
      private ValueToggleButton BottomFrontCornerAxialToggleButton;
      private ValueToggleButton TopRearCornerAxialToggleButton;
      private ValueToggleButton BottomRearCircumferentialToggleButton;
      private ValueToggleButton BottomFrontCircumferentialToggleButton;
      private ValueToggleButton TopRearCircumferentialToggleButton;
      private ValueToggleButton BottomRearLaunchAxialToggleButton;
      private ValueToggleButton BottomFrontLaunchAxialToggleButton;
      private ValueToggleButton TopRearLaunchAxialToggleButton;
      private BorderedPanel borderedPanel1;
      private ValueCycleButton TopRearStateCycleButton;
      private System.Windows.Forms.Label label6;
      private ValueToggleButton TopRearDirectionToggleButton;
      private BorderedPanel borderedPanel2;
      private ValueCycleButton TopFrontStateCycleButton;
      private System.Windows.Forms.Label label7;
      private ValueToggleButton TopFrontDirectionToggleButton;
      private TransparentLabel transparentLabel1;
      private TransparentLabel transparentLabel3;
      private TransparentLabel transparentLabel4;
      private TransparentLabel transparentLabel2;
      private NicBotButton BackButton;
   }
}