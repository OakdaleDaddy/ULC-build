namespace NICBOT.GUI
{
   partial class DrillSetupForm
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
         this.IndexPanel = new System.Windows.Forms.Panel();
         this.IndexLabel = new System.Windows.Forms.Label();
         this.DrillCuttingDepthValueButton = new NICBOT.GUI.ValueButton();
         this.DrillSearchSpeedValueButton = new NICBOT.GUI.ValueButton();
         this.DrillCuttingSpeedValueButton = new NICBOT.GUI.ValueButton();
         this.DrillTravelSpeedValueButton = new NICBOT.GUI.ValueButton();
         this.TitleLabel = new System.Windows.Forms.Label();
         this.AutoOriginToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.DrillSpeedValueButton = new NICBOT.GUI.ValueButton();
         this.PeckModePanel = new System.Windows.Forms.Panel();
         this.PeckModeLabel = new System.Windows.Forms.Label();
         this.DrillRetractModeToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.DrillCuttingIncrementValueButton = new NICBOT.GUI.ValueButton();
         this.DrillRetractDistanceValueButton = new NICBOT.GUI.ValueButton();
         this.DrillRetractPositionValueButton = new NICBOT.GUI.ValueButton();
         this.DrillModeToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.BackButton = new System.Windows.Forms.Button();
         this.DrillSelectToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.ServoPanel = new System.Windows.Forms.Panel();
         this.ServoLabel = new System.Windows.Forms.Label();
         this.ServoAccelerationValueButton = new NICBOT.GUI.ValueButton();
         this.ServoProportionalControlConstantValueButton = new NICBOT.GUI.ValueButton();
         this.ServoDerivativeControlConstantValueButton = new NICBOT.GUI.ValueButton();
         this.ServoErrorLimitValueButton = new NICBOT.GUI.ValueButton();
         this.ServoIntegralControlConstantValueButton = new NICBOT.GUI.ValueButton();
         this.MainPanel.SuspendLayout();
         this.IndexPanel.SuspendLayout();
         this.PeckModePanel.SuspendLayout();
         this.ServoPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // MainPanel
         // 
         this.MainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.MainPanel.Controls.Add(this.ServoPanel);
         this.MainPanel.Controls.Add(this.IndexPanel);
         this.MainPanel.Controls.Add(this.TitleLabel);
         this.MainPanel.Controls.Add(this.AutoOriginToggleButton);
         this.MainPanel.Controls.Add(this.DrillSpeedValueButton);
         this.MainPanel.Controls.Add(this.PeckModePanel);
         this.MainPanel.Controls.Add(this.DrillModeToggleButton);
         this.MainPanel.Controls.Add(this.BackButton);
         this.MainPanel.Controls.Add(this.DrillSelectToggleButton);
         this.MainPanel.EdgeWeight = 3;
         this.MainPanel.Location = new System.Drawing.Point(0, 0);
         this.MainPanel.Name = "MainPanel";
         this.MainPanel.Size = new System.Drawing.Size(506, 776);
         this.MainPanel.TabIndex = 0;
         // 
         // IndexPanel
         // 
         this.IndexPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
         this.IndexPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.IndexPanel.Controls.Add(this.IndexLabel);
         this.IndexPanel.Controls.Add(this.DrillCuttingDepthValueButton);
         this.IndexPanel.Controls.Add(this.DrillSearchSpeedValueButton);
         this.IndexPanel.Controls.Add(this.DrillCuttingSpeedValueButton);
         this.IndexPanel.Controls.Add(this.DrillTravelSpeedValueButton);
         this.IndexPanel.Location = new System.Drawing.Point(19, 166);
         this.IndexPanel.Name = "IndexPanel";
         this.IndexPanel.Size = new System.Drawing.Size(468, 135);
         this.IndexPanel.TabIndex = 134;
         // 
         // IndexLabel
         // 
         this.IndexLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.IndexLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.IndexLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.IndexLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(209)))), ((int)(((byte)(234)))));
         this.IndexLabel.Location = new System.Drawing.Point(-1, -1);
         this.IndexLabel.Name = "IndexLabel";
         this.IndexLabel.Size = new System.Drawing.Size(468, 29);
         this.IndexLabel.TabIndex = 132;
         this.IndexLabel.Text = "INDEXER SETUP";
         this.IndexLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // DrillCuttingDepthValueButton
         // 
         this.DrillCuttingDepthValueButton.ArrowWidth = 0;
         this.DrillCuttingDepthValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillCuttingDepthValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.DrillCuttingDepthValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.DrillCuttingDepthValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.DrillCuttingDepthValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DrillCuttingDepthValueButton.HoldTimeoutInterval = 0;
         this.DrillCuttingDepthValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.DrillCuttingDepthValueButton.LeftArrowVisible = false;
         this.DrillCuttingDepthValueButton.Location = new System.Drawing.Point(8, 37);
         this.DrillCuttingDepthValueButton.Name = "DrillCuttingDepthValueButton";
         this.DrillCuttingDepthValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.DrillCuttingDepthValueButton.RightArrowVisible = false;
         this.DrillCuttingDepthValueButton.Size = new System.Drawing.Size(107, 90);
         this.DrillCuttingDepthValueButton.TabIndex = 123;
         this.DrillCuttingDepthValueButton.Text = "CUTTING DEPTH";
         this.DrillCuttingDepthValueButton.UseVisualStyleBackColor = false;
         this.DrillCuttingDepthValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.DrillCuttingDepthValueButton.ValueEdgeHeight = 8;
         this.DrillCuttingDepthValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.DrillCuttingDepthValueButton.ValueForeColor = System.Drawing.Color.White;
         this.DrillCuttingDepthValueButton.ValueHeight = 22;
         this.DrillCuttingDepthValueButton.ValueText = "##### mm";
         this.DrillCuttingDepthValueButton.ValueWidth = 92;
         this.DrillCuttingDepthValueButton.Click += new System.EventHandler(this.DrillCuttingDepthValueButton_Click);
         // 
         // DrillSearchSpeedValueButton
         // 
         this.DrillSearchSpeedValueButton.ArrowWidth = 0;
         this.DrillSearchSpeedValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillSearchSpeedValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.DrillSearchSpeedValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.DrillSearchSpeedValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.DrillSearchSpeedValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DrillSearchSpeedValueButton.HoldTimeoutInterval = 0;
         this.DrillSearchSpeedValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.DrillSearchSpeedValueButton.LeftArrowVisible = false;
         this.DrillSearchSpeedValueButton.Location = new System.Drawing.Point(353, 37);
         this.DrillSearchSpeedValueButton.Name = "DrillSearchSpeedValueButton";
         this.DrillSearchSpeedValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.DrillSearchSpeedValueButton.RightArrowVisible = false;
         this.DrillSearchSpeedValueButton.Size = new System.Drawing.Size(107, 90);
         this.DrillSearchSpeedValueButton.TabIndex = 120;
         this.DrillSearchSpeedValueButton.Text = "SEARCH SPEED";
         this.DrillSearchSpeedValueButton.UseVisualStyleBackColor = false;
         this.DrillSearchSpeedValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.DrillSearchSpeedValueButton.ValueEdgeHeight = 8;
         this.DrillSearchSpeedValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.DrillSearchSpeedValueButton.ValueForeColor = System.Drawing.Color.White;
         this.DrillSearchSpeedValueButton.ValueHeight = 22;
         this.DrillSearchSpeedValueButton.ValueText = "## mm/s";
         this.DrillSearchSpeedValueButton.ValueWidth = 92;
         this.DrillSearchSpeedValueButton.Click += new System.EventHandler(this.DrillSearchSpeedValueButton_Click);
         // 
         // DrillCuttingSpeedValueButton
         // 
         this.DrillCuttingSpeedValueButton.ArrowWidth = 0;
         this.DrillCuttingSpeedValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillCuttingSpeedValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.DrillCuttingSpeedValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.DrillCuttingSpeedValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.DrillCuttingSpeedValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DrillCuttingSpeedValueButton.HoldTimeoutInterval = 0;
         this.DrillCuttingSpeedValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.DrillCuttingSpeedValueButton.LeftArrowVisible = false;
         this.DrillCuttingSpeedValueButton.Location = new System.Drawing.Point(123, 37);
         this.DrillCuttingSpeedValueButton.Name = "DrillCuttingSpeedValueButton";
         this.DrillCuttingSpeedValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.DrillCuttingSpeedValueButton.RightArrowVisible = false;
         this.DrillCuttingSpeedValueButton.Size = new System.Drawing.Size(107, 90);
         this.DrillCuttingSpeedValueButton.TabIndex = 121;
         this.DrillCuttingSpeedValueButton.Text = "CUTTING    SPEED";
         this.DrillCuttingSpeedValueButton.UseVisualStyleBackColor = false;
         this.DrillCuttingSpeedValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.DrillCuttingSpeedValueButton.ValueEdgeHeight = 8;
         this.DrillCuttingSpeedValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.DrillCuttingSpeedValueButton.ValueForeColor = System.Drawing.Color.White;
         this.DrillCuttingSpeedValueButton.ValueHeight = 22;
         this.DrillCuttingSpeedValueButton.ValueText = "## mm/s";
         this.DrillCuttingSpeedValueButton.ValueWidth = 92;
         this.DrillCuttingSpeedValueButton.Click += new System.EventHandler(this.DrillCuttingSpeedValueButton_Click);
         // 
         // DrillTravelSpeedValueButton
         // 
         this.DrillTravelSpeedValueButton.ArrowWidth = 0;
         this.DrillTravelSpeedValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillTravelSpeedValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.DrillTravelSpeedValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.DrillTravelSpeedValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.DrillTravelSpeedValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DrillTravelSpeedValueButton.HoldTimeoutInterval = 0;
         this.DrillTravelSpeedValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.DrillTravelSpeedValueButton.LeftArrowVisible = false;
         this.DrillTravelSpeedValueButton.Location = new System.Drawing.Point(238, 37);
         this.DrillTravelSpeedValueButton.Name = "DrillTravelSpeedValueButton";
         this.DrillTravelSpeedValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.DrillTravelSpeedValueButton.RightArrowVisible = false;
         this.DrillTravelSpeedValueButton.Size = new System.Drawing.Size(107, 90);
         this.DrillTravelSpeedValueButton.TabIndex = 119;
         this.DrillTravelSpeedValueButton.Text = "TRAVEL SPEED";
         this.DrillTravelSpeedValueButton.UseVisualStyleBackColor = false;
         this.DrillTravelSpeedValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.DrillTravelSpeedValueButton.ValueEdgeHeight = 8;
         this.DrillTravelSpeedValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.DrillTravelSpeedValueButton.ValueForeColor = System.Drawing.Color.White;
         this.DrillTravelSpeedValueButton.ValueHeight = 22;
         this.DrillTravelSpeedValueButton.ValueText = "## mm/s";
         this.DrillTravelSpeedValueButton.ValueWidth = 92;
         this.DrillTravelSpeedValueButton.Click += new System.EventHandler(this.DrillTravelSpeedValueButton_Click);
         // 
         // TitleLabel
         // 
         this.TitleLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.TitleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(209)))), ((int)(((byte)(234)))));
         this.TitleLabel.Location = new System.Drawing.Point(16, 16);
         this.TitleLabel.Name = "TitleLabel";
         this.TitleLabel.Size = new System.Drawing.Size(474, 36);
         this.TitleLabel.TabIndex = 133;
         this.TitleLabel.Text = "DRILL SETUP";
         this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // AutoOriginToggleButton
         // 
         this.AutoOriginToggleButton.AutomaticToggle = true;
         this.AutoOriginToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.AutoOriginToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.AutoOriginToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.AutoOriginToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.AutoOriginToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.AutoOriginToggleButton.HoldEnable = false;
         this.AutoOriginToggleButton.HoldTimeoutInterval = 0;
         this.AutoOriginToggleButton.Location = new System.Drawing.Point(257, 68);
         this.AutoOriginToggleButton.Name = "AutoOriginToggleButton";
         this.AutoOriginToggleButton.OptionASelected = true;
         this.AutoOriginToggleButton.OptionAText = "ON";
         this.AutoOriginToggleButton.OptionBSelected = false;
         this.AutoOriginToggleButton.OptionBText = "OFF";
         this.AutoOriginToggleButton.OptionCenterWidth = 2;
         this.AutoOriginToggleButton.OptionEdgeHeight = 8;
         this.AutoOriginToggleButton.OptionHeight = 22;
         this.AutoOriginToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.AutoOriginToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
         this.AutoOriginToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.AutoOriginToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.AutoOriginToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.AutoOriginToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.AutoOriginToggleButton.OptionWidth = 45;
         this.AutoOriginToggleButton.Size = new System.Drawing.Size(107, 90);
         this.AutoOriginToggleButton.TabIndex = 116;
         this.AutoOriginToggleButton.Text = "AUTO ORIGIN";
         this.AutoOriginToggleButton.UseVisualStyleBackColor = false;
         this.AutoOriginToggleButton.Click += new System.EventHandler(this.AutoOriginToggleButton_Click);
         // 
         // DrillSpeedValueButton
         // 
         this.DrillSpeedValueButton.ArrowWidth = 0;
         this.DrillSpeedValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillSpeedValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.DrillSpeedValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.DrillSpeedValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.DrillSpeedValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DrillSpeedValueButton.HoldTimeoutInterval = 0;
         this.DrillSpeedValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.DrillSpeedValueButton.LeftArrowVisible = false;
         this.DrillSpeedValueButton.Location = new System.Drawing.Point(372, 68);
         this.DrillSpeedValueButton.Name = "DrillSpeedValueButton";
         this.DrillSpeedValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.DrillSpeedValueButton.RightArrowVisible = false;
         this.DrillSpeedValueButton.Size = new System.Drawing.Size(107, 90);
         this.DrillSpeedValueButton.TabIndex = 122;
         this.DrillSpeedValueButton.Text = "DRILL SPEED";
         this.DrillSpeedValueButton.UseVisualStyleBackColor = false;
         this.DrillSpeedValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.DrillSpeedValueButton.ValueEdgeHeight = 8;
         this.DrillSpeedValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.DrillSpeedValueButton.ValueForeColor = System.Drawing.Color.White;
         this.DrillSpeedValueButton.ValueHeight = 22;
         this.DrillSpeedValueButton.ValueText = "#### RPM";
         this.DrillSpeedValueButton.ValueWidth = 92;
         this.DrillSpeedValueButton.Click += new System.EventHandler(this.DrillSpeedValueButton_Click);
         // 
         // PeckModePanel
         // 
         this.PeckModePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
         this.PeckModePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.PeckModePanel.Controls.Add(this.PeckModeLabel);
         this.PeckModePanel.Controls.Add(this.DrillRetractModeToggleButton);
         this.PeckModePanel.Controls.Add(this.DrillCuttingIncrementValueButton);
         this.PeckModePanel.Controls.Add(this.DrillRetractDistanceValueButton);
         this.PeckModePanel.Controls.Add(this.DrillRetractPositionValueButton);
         this.PeckModePanel.Location = new System.Drawing.Point(19, 309);
         this.PeckModePanel.Name = "PeckModePanel";
         this.PeckModePanel.Size = new System.Drawing.Size(468, 135);
         this.PeckModePanel.TabIndex = 128;
         // 
         // PeckModeLabel
         // 
         this.PeckModeLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.PeckModeLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.PeckModeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.PeckModeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(209)))), ((int)(((byte)(234)))));
         this.PeckModeLabel.Location = new System.Drawing.Point(-1, -1);
         this.PeckModeLabel.Name = "PeckModeLabel";
         this.PeckModeLabel.Size = new System.Drawing.Size(468, 29);
         this.PeckModeLabel.TabIndex = 131;
         this.PeckModeLabel.Text = "PECK MODE SETUP";
         this.PeckModeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // DrillRetractModeToggleButton
         // 
         this.DrillRetractModeToggleButton.AutomaticToggle = true;
         this.DrillRetractModeToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillRetractModeToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.DrillRetractModeToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.DrillRetractModeToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.DrillRetractModeToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DrillRetractModeToggleButton.HoldEnable = false;
         this.DrillRetractModeToggleButton.HoldTimeoutInterval = 0;
         this.DrillRetractModeToggleButton.Location = new System.Drawing.Point(123, 37);
         this.DrillRetractModeToggleButton.Name = "DrillRetractModeToggleButton";
         this.DrillRetractModeToggleButton.OptionASelected = true;
         this.DrillRetractModeToggleButton.OptionAText = "DIST";
         this.DrillRetractModeToggleButton.OptionBSelected = false;
         this.DrillRetractModeToggleButton.OptionBText = "POS";
         this.DrillRetractModeToggleButton.OptionCenterWidth = 2;
         this.DrillRetractModeToggleButton.OptionEdgeHeight = 8;
         this.DrillRetractModeToggleButton.OptionHeight = 22;
         this.DrillRetractModeToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.DrillRetractModeToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
         this.DrillRetractModeToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.DrillRetractModeToggleButton.OptionSelectedBackColor = System.Drawing.Color.Black;
         this.DrillRetractModeToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DrillRetractModeToggleButton.OptionSelectedForeColor = System.Drawing.Color.White;
         this.DrillRetractModeToggleButton.OptionWidth = 45;
         this.DrillRetractModeToggleButton.Size = new System.Drawing.Size(107, 90);
         this.DrillRetractModeToggleButton.TabIndex = 115;
         this.DrillRetractModeToggleButton.Text = "RETRACT MODE";
         this.DrillRetractModeToggleButton.UseVisualStyleBackColor = false;
         this.DrillRetractModeToggleButton.Click += new System.EventHandler(this.DrillRetractModeToggleButton_Click);
         // 
         // DrillCuttingIncrementValueButton
         // 
         this.DrillCuttingIncrementValueButton.ArrowWidth = 0;
         this.DrillCuttingIncrementValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillCuttingIncrementValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.DrillCuttingIncrementValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.DrillCuttingIncrementValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.DrillCuttingIncrementValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DrillCuttingIncrementValueButton.HoldTimeoutInterval = 0;
         this.DrillCuttingIncrementValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.DrillCuttingIncrementValueButton.LeftArrowVisible = false;
         this.DrillCuttingIncrementValueButton.Location = new System.Drawing.Point(8, 37);
         this.DrillCuttingIncrementValueButton.Name = "DrillCuttingIncrementValueButton";
         this.DrillCuttingIncrementValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.DrillCuttingIncrementValueButton.RightArrowVisible = false;
         this.DrillCuttingIncrementValueButton.Size = new System.Drawing.Size(107, 90);
         this.DrillCuttingIncrementValueButton.TabIndex = 124;
         this.DrillCuttingIncrementValueButton.Text = "CUTTING INCREMENT";
         this.DrillCuttingIncrementValueButton.UseVisualStyleBackColor = false;
         this.DrillCuttingIncrementValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.DrillCuttingIncrementValueButton.ValueEdgeHeight = 8;
         this.DrillCuttingIncrementValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.DrillCuttingIncrementValueButton.ValueForeColor = System.Drawing.Color.White;
         this.DrillCuttingIncrementValueButton.ValueHeight = 22;
         this.DrillCuttingIncrementValueButton.ValueText = "##### mm";
         this.DrillCuttingIncrementValueButton.ValueWidth = 92;
         this.DrillCuttingIncrementValueButton.Click += new System.EventHandler(this.DrillCuttingIncrementValueButton_Click);
         // 
         // DrillRetractDistanceValueButton
         // 
         this.DrillRetractDistanceValueButton.ArrowWidth = 0;
         this.DrillRetractDistanceValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillRetractDistanceValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.DrillRetractDistanceValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.DrillRetractDistanceValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.DrillRetractDistanceValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DrillRetractDistanceValueButton.HoldTimeoutInterval = 0;
         this.DrillRetractDistanceValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.DrillRetractDistanceValueButton.LeftArrowVisible = false;
         this.DrillRetractDistanceValueButton.Location = new System.Drawing.Point(238, 37);
         this.DrillRetractDistanceValueButton.Name = "DrillRetractDistanceValueButton";
         this.DrillRetractDistanceValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.DrillRetractDistanceValueButton.RightArrowVisible = false;
         this.DrillRetractDistanceValueButton.Size = new System.Drawing.Size(107, 90);
         this.DrillRetractDistanceValueButton.TabIndex = 126;
         this.DrillRetractDistanceValueButton.Text = "RETRACT DISTANCE";
         this.DrillRetractDistanceValueButton.UseVisualStyleBackColor = false;
         this.DrillRetractDistanceValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.DrillRetractDistanceValueButton.ValueEdgeHeight = 8;
         this.DrillRetractDistanceValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.DrillRetractDistanceValueButton.ValueForeColor = System.Drawing.Color.White;
         this.DrillRetractDistanceValueButton.ValueHeight = 22;
         this.DrillRetractDistanceValueButton.ValueText = "##### mm";
         this.DrillRetractDistanceValueButton.ValueWidth = 92;
         this.DrillRetractDistanceValueButton.Click += new System.EventHandler(this.DrillRetractDistanceValueButton_Click);
         // 
         // DrillRetractPositionValueButton
         // 
         this.DrillRetractPositionValueButton.ArrowWidth = 0;
         this.DrillRetractPositionValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillRetractPositionValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.DrillRetractPositionValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.DrillRetractPositionValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.DrillRetractPositionValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DrillRetractPositionValueButton.HoldTimeoutInterval = 0;
         this.DrillRetractPositionValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.DrillRetractPositionValueButton.LeftArrowVisible = false;
         this.DrillRetractPositionValueButton.Location = new System.Drawing.Point(353, 37);
         this.DrillRetractPositionValueButton.Name = "DrillRetractPositionValueButton";
         this.DrillRetractPositionValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.DrillRetractPositionValueButton.RightArrowVisible = false;
         this.DrillRetractPositionValueButton.Size = new System.Drawing.Size(107, 90);
         this.DrillRetractPositionValueButton.TabIndex = 125;
         this.DrillRetractPositionValueButton.Text = "RETRACT POSITION";
         this.DrillRetractPositionValueButton.UseVisualStyleBackColor = false;
         this.DrillRetractPositionValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.DrillRetractPositionValueButton.ValueEdgeHeight = 8;
         this.DrillRetractPositionValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.DrillRetractPositionValueButton.ValueForeColor = System.Drawing.Color.White;
         this.DrillRetractPositionValueButton.ValueHeight = 22;
         this.DrillRetractPositionValueButton.ValueText = "##### mm";
         this.DrillRetractPositionValueButton.ValueWidth = 92;
         this.DrillRetractPositionValueButton.Click += new System.EventHandler(this.DrillRetractPositionValueButton_Click);
         // 
         // DrillModeToggleButton
         // 
         this.DrillModeToggleButton.AutomaticToggle = true;
         this.DrillModeToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillModeToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.DrillModeToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.DrillModeToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.DrillModeToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DrillModeToggleButton.HoldEnable = false;
         this.DrillModeToggleButton.HoldTimeoutInterval = 0;
         this.DrillModeToggleButton.Location = new System.Drawing.Point(142, 68);
         this.DrillModeToggleButton.Name = "DrillModeToggleButton";
         this.DrillModeToggleButton.OptionASelected = true;
         this.DrillModeToggleButton.OptionAText = "CONT";
         this.DrillModeToggleButton.OptionBSelected = false;
         this.DrillModeToggleButton.OptionBText = "PECK";
         this.DrillModeToggleButton.OptionCenterWidth = 2;
         this.DrillModeToggleButton.OptionEdgeHeight = 8;
         this.DrillModeToggleButton.OptionHeight = 22;
         this.DrillModeToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.DrillModeToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
         this.DrillModeToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.DrillModeToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.DrillModeToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DrillModeToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.DrillModeToggleButton.OptionWidth = 45;
         this.DrillModeToggleButton.Size = new System.Drawing.Size(107, 90);
         this.DrillModeToggleButton.TabIndex = 114;
         this.DrillModeToggleButton.Text = "DRILL   MODE";
         this.DrillModeToggleButton.UseVisualStyleBackColor = false;
         this.DrillModeToggleButton.Click += new System.EventHandler(this.DrillModeToggleButton_Click);
         // 
         // BackButton
         // 
         this.BackButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BackButton.ForeColor = System.Drawing.Color.Black;
         this.BackButton.Location = new System.Drawing.Point(372, 693);
         this.BackButton.Name = "BackButton";
         this.BackButton.Size = new System.Drawing.Size(107, 67);
         this.BackButton.TabIndex = 118;
         this.BackButton.Text = "BACK";
         this.BackButton.UseVisualStyleBackColor = false;
         this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
         // 
         // DrillSelectToggleButton
         // 
         this.DrillSelectToggleButton.AutomaticToggle = true;
         this.DrillSelectToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillSelectToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.DrillSelectToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.DrillSelectToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.DrillSelectToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DrillSelectToggleButton.HoldEnable = false;
         this.DrillSelectToggleButton.HoldTimeoutInterval = 0;
         this.DrillSelectToggleButton.Location = new System.Drawing.Point(27, 68);
         this.DrillSelectToggleButton.Name = "DrillSelectToggleButton";
         this.DrillSelectToggleButton.OptionASelected = true;
         this.DrillSelectToggleButton.OptionAText = "FRONT";
         this.DrillSelectToggleButton.OptionBSelected = false;
         this.DrillSelectToggleButton.OptionBText = "REAR";
         this.DrillSelectToggleButton.OptionCenterWidth = 2;
         this.DrillSelectToggleButton.OptionEdgeHeight = 8;
         this.DrillSelectToggleButton.OptionHeight = 22;
         this.DrillSelectToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.DrillSelectToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DrillSelectToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.DrillSelectToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.DrillSelectToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DrillSelectToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.DrillSelectToggleButton.OptionWidth = 45;
         this.DrillSelectToggleButton.Size = new System.Drawing.Size(107, 90);
         this.DrillSelectToggleButton.TabIndex = 117;
         this.DrillSelectToggleButton.Text = "DRILL SELECT";
         this.DrillSelectToggleButton.UseVisualStyleBackColor = false;
         this.DrillSelectToggleButton.Click += new System.EventHandler(this.DrillSelectToggleButton_Click);
         // 
         // ServoPanel
         // 
         this.ServoPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
         this.ServoPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.ServoPanel.Controls.Add(this.ServoIntegralControlConstantValueButton);
         this.ServoPanel.Controls.Add(this.ServoErrorLimitValueButton);
         this.ServoPanel.Controls.Add(this.ServoLabel);
         this.ServoPanel.Controls.Add(this.ServoAccelerationValueButton);
         this.ServoPanel.Controls.Add(this.ServoProportionalControlConstantValueButton);
         this.ServoPanel.Controls.Add(this.ServoDerivativeControlConstantValueButton);
         this.ServoPanel.Location = new System.Drawing.Point(19, 452);
         this.ServoPanel.Name = "ServoPanel";
         this.ServoPanel.Size = new System.Drawing.Size(468, 233);
         this.ServoPanel.TabIndex = 135;
         // 
         // ServoLabel
         // 
         this.ServoLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.ServoLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.ServoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ServoLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(209)))), ((int)(((byte)(234)))));
         this.ServoLabel.Location = new System.Drawing.Point(-1, -1);
         this.ServoLabel.Name = "ServoLabel";
         this.ServoLabel.Size = new System.Drawing.Size(468, 29);
         this.ServoLabel.TabIndex = 131;
         this.ServoLabel.Text = "SERVO SETUP";
         this.ServoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // ServoAccelerationValueButton
         // 
         this.ServoAccelerationValueButton.ArrowWidth = 0;
         this.ServoAccelerationValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ServoAccelerationValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ServoAccelerationValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ServoAccelerationValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.ServoAccelerationValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ServoAccelerationValueButton.HoldTimeoutInterval = 0;
         this.ServoAccelerationValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.ServoAccelerationValueButton.LeftArrowVisible = false;
         this.ServoAccelerationValueButton.Location = new System.Drawing.Point(123, 135);
         this.ServoAccelerationValueButton.Name = "ServoAccelerationValueButton";
         this.ServoAccelerationValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.ServoAccelerationValueButton.RightArrowVisible = false;
         this.ServoAccelerationValueButton.Size = new System.Drawing.Size(107, 90);
         this.ServoAccelerationValueButton.TabIndex = 124;
         this.ServoAccelerationValueButton.Text = "ACCEL";
         this.ServoAccelerationValueButton.UseVisualStyleBackColor = false;
         this.ServoAccelerationValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.ServoAccelerationValueButton.ValueEdgeHeight = 8;
         this.ServoAccelerationValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.ServoAccelerationValueButton.ValueForeColor = System.Drawing.Color.White;
         this.ServoAccelerationValueButton.ValueHeight = 22;
         this.ServoAccelerationValueButton.ValueText = "##### mm";
         this.ServoAccelerationValueButton.ValueWidth = 92;
         this.ServoAccelerationValueButton.Click += new System.EventHandler(this.ServoAccelerationValueButton_Click);
         // 
         // ServoProportionalControlConstantValueButton
         // 
         this.ServoProportionalControlConstantValueButton.ArrowWidth = 0;
         this.ServoProportionalControlConstantValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ServoProportionalControlConstantValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ServoProportionalControlConstantValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ServoProportionalControlConstantValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.ServoProportionalControlConstantValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ServoProportionalControlConstantValueButton.HoldTimeoutInterval = 0;
         this.ServoProportionalControlConstantValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.ServoProportionalControlConstantValueButton.LeftArrowVisible = false;
         this.ServoProportionalControlConstantValueButton.Location = new System.Drawing.Point(123, 37);
         this.ServoProportionalControlConstantValueButton.Name = "ServoProportionalControlConstantValueButton";
         this.ServoProportionalControlConstantValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.ServoProportionalControlConstantValueButton.RightArrowVisible = false;
         this.ServoProportionalControlConstantValueButton.Size = new System.Drawing.Size(107, 90);
         this.ServoProportionalControlConstantValueButton.TabIndex = 126;
         this.ServoProportionalControlConstantValueButton.Text = "KP";
         this.ServoProportionalControlConstantValueButton.UseVisualStyleBackColor = false;
         this.ServoProportionalControlConstantValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.ServoProportionalControlConstantValueButton.ValueEdgeHeight = 8;
         this.ServoProportionalControlConstantValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.ServoProportionalControlConstantValueButton.ValueForeColor = System.Drawing.Color.White;
         this.ServoProportionalControlConstantValueButton.ValueHeight = 22;
         this.ServoProportionalControlConstantValueButton.ValueText = "##### mm";
         this.ServoProportionalControlConstantValueButton.ValueWidth = 92;
         this.ServoProportionalControlConstantValueButton.Click += new System.EventHandler(this.ServoProportionalControlConstantValueButton_Click);
         // 
         // ServoDerivativeControlConstantValueButton
         // 
         this.ServoDerivativeControlConstantValueButton.ArrowWidth = 0;
         this.ServoDerivativeControlConstantValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ServoDerivativeControlConstantValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ServoDerivativeControlConstantValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ServoDerivativeControlConstantValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.ServoDerivativeControlConstantValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ServoDerivativeControlConstantValueButton.HoldTimeoutInterval = 0;
         this.ServoDerivativeControlConstantValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.ServoDerivativeControlConstantValueButton.LeftArrowVisible = false;
         this.ServoDerivativeControlConstantValueButton.Location = new System.Drawing.Point(353, 37);
         this.ServoDerivativeControlConstantValueButton.Name = "ServoDerivativeControlConstantValueButton";
         this.ServoDerivativeControlConstantValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.ServoDerivativeControlConstantValueButton.RightArrowVisible = false;
         this.ServoDerivativeControlConstantValueButton.Size = new System.Drawing.Size(107, 90);
         this.ServoDerivativeControlConstantValueButton.TabIndex = 125;
         this.ServoDerivativeControlConstantValueButton.Text = "KD";
         this.ServoDerivativeControlConstantValueButton.UseVisualStyleBackColor = false;
         this.ServoDerivativeControlConstantValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.ServoDerivativeControlConstantValueButton.ValueEdgeHeight = 8;
         this.ServoDerivativeControlConstantValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.ServoDerivativeControlConstantValueButton.ValueForeColor = System.Drawing.Color.White;
         this.ServoDerivativeControlConstantValueButton.ValueHeight = 22;
         this.ServoDerivativeControlConstantValueButton.ValueText = "##### mm";
         this.ServoDerivativeControlConstantValueButton.ValueWidth = 92;
         this.ServoDerivativeControlConstantValueButton.Click += new System.EventHandler(this.ServoDerivativeControlConstantValueButton_Click);
         // 
         // ServoErrorLimitValueButton
         // 
         this.ServoErrorLimitValueButton.ArrowWidth = 0;
         this.ServoErrorLimitValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ServoErrorLimitValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ServoErrorLimitValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ServoErrorLimitValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.ServoErrorLimitValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ServoErrorLimitValueButton.HoldTimeoutInterval = 0;
         this.ServoErrorLimitValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.ServoErrorLimitValueButton.LeftArrowVisible = false;
         this.ServoErrorLimitValueButton.Location = new System.Drawing.Point(8, 135);
         this.ServoErrorLimitValueButton.Name = "ServoErrorLimitValueButton";
         this.ServoErrorLimitValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.ServoErrorLimitValueButton.RightArrowVisible = false;
         this.ServoErrorLimitValueButton.Size = new System.Drawing.Size(107, 90);
         this.ServoErrorLimitValueButton.TabIndex = 132;
         this.ServoErrorLimitValueButton.Text = "ERROR LIMIT";
         this.ServoErrorLimitValueButton.UseVisualStyleBackColor = false;
         this.ServoErrorLimitValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.ServoErrorLimitValueButton.ValueEdgeHeight = 8;
         this.ServoErrorLimitValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.ServoErrorLimitValueButton.ValueForeColor = System.Drawing.Color.White;
         this.ServoErrorLimitValueButton.ValueHeight = 22;
         this.ServoErrorLimitValueButton.ValueText = "##### mm";
         this.ServoErrorLimitValueButton.ValueWidth = 92;
         this.ServoErrorLimitValueButton.Click += new System.EventHandler(this.ServoErrorLimitValueButton_Click);
         // 
         // ServoIntegralControlConstantValueButton
         // 
         this.ServoIntegralControlConstantValueButton.ArrowWidth = 0;
         this.ServoIntegralControlConstantValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ServoIntegralControlConstantValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ServoIntegralControlConstantValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ServoIntegralControlConstantValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.ServoIntegralControlConstantValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ServoIntegralControlConstantValueButton.HoldTimeoutInterval = 0;
         this.ServoIntegralControlConstantValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.ServoIntegralControlConstantValueButton.LeftArrowVisible = false;
         this.ServoIntegralControlConstantValueButton.Location = new System.Drawing.Point(238, 37);
         this.ServoIntegralControlConstantValueButton.Name = "ServoIntegralControlConstantValueButton";
         this.ServoIntegralControlConstantValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.ServoIntegralControlConstantValueButton.RightArrowVisible = false;
         this.ServoIntegralControlConstantValueButton.Size = new System.Drawing.Size(107, 90);
         this.ServoIntegralControlConstantValueButton.TabIndex = 133;
         this.ServoIntegralControlConstantValueButton.Text = "KI";
         this.ServoIntegralControlConstantValueButton.UseVisualStyleBackColor = false;
         this.ServoIntegralControlConstantValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.ServoIntegralControlConstantValueButton.ValueEdgeHeight = 8;
         this.ServoIntegralControlConstantValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.ServoIntegralControlConstantValueButton.ValueForeColor = System.Drawing.Color.White;
         this.ServoIntegralControlConstantValueButton.ValueHeight = 22;
         this.ServoIntegralControlConstantValueButton.ValueText = "##### mm";
         this.ServoIntegralControlConstantValueButton.ValueWidth = 92;
         this.ServoIntegralControlConstantValueButton.Click += new System.EventHandler(this.ServoIntegralControlConstantValueButton_Click);
         // 
         // DrillSetupForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Control;
         this.ClientSize = new System.Drawing.Size(506, 776);
         this.Controls.Add(this.MainPanel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "DrillSetupForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "DrillAutoSetupForm";
         this.Shown += new System.EventHandler(this.DrillAutoSetupForm_Shown);
         this.MainPanel.ResumeLayout(false);
         this.IndexPanel.ResumeLayout(false);
         this.PeckModePanel.ResumeLayout(false);
         this.ServoPanel.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private BorderedPanel MainPanel;
      private ValueToggleButton DrillModeToggleButton;
      private ValueToggleButton DrillRetractModeToggleButton;
      private ValueToggleButton DrillSelectToggleButton;
      private ValueToggleButton AutoOriginToggleButton;
      private System.Windows.Forms.Button BackButton;
      private ValueButton DrillTravelSpeedValueButton;
      private ValueButton DrillRetractDistanceValueButton;
      private ValueButton DrillRetractPositionValueButton;
      private ValueButton DrillCuttingIncrementValueButton;
      private ValueButton DrillCuttingDepthValueButton;
      private ValueButton DrillSpeedValueButton;
      private ValueButton DrillCuttingSpeedValueButton;
      private ValueButton DrillSearchSpeedValueButton;
      private System.Windows.Forms.Panel PeckModePanel;
      private System.Windows.Forms.Label PeckModeLabel;
      private System.Windows.Forms.Label TitleLabel;
      private System.Windows.Forms.Panel IndexPanel;
      private System.Windows.Forms.Label IndexLabel;
      private System.Windows.Forms.Panel ServoPanel;
      private ValueButton ServoIntegralControlConstantValueButton;
      private ValueButton ServoErrorLimitValueButton;
      private System.Windows.Forms.Label ServoLabel;
      private ValueButton ServoAccelerationValueButton;
      private ValueButton ServoProportionalControlConstantValueButton;
      private ValueButton ServoDerivativeControlConstantValueButton;
   }
}