namespace NICBOT.GUI
{
   partial class RepairBodySetupForm
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
         this.FormBorderedPanel = new NICBOT.GUI.BorderedPanel();
         this.BackButton = new NICBOT.GUI.NicBotButton();
         this.TitleLabel = new System.Windows.Forms.Label();
         this.BodyBorderedPanel = new NICBOT.GUI.BorderedPanel();
         this.BodyRearReleaseButton = new NICBOT.GUI.NicBotButton();
         this.BodyFrontReleaseButton = new NICBOT.GUI.NicBotButton();
         this.BodyOpenButton = new NICBOT.GUI.NicBotButton();
         this.BodyDrillButton = new NICBOT.GUI.NicBotButton();
         this.BodyClosedButton = new NICBOT.GUI.HoldButton();
         this.BodyOffButton = new NICBOT.GUI.HoldButton();
         this.SolenoidBorderedPanel = new NICBOT.GUI.BorderedPanel();
         this.FrontArmExtendButton = new NICBOT.GUI.ValueToggleButton();
         this.FrontArmRetractButton = new NICBOT.GUI.ValueToggleButton();
         this.LowerArmsRetractButton = new NICBOT.GUI.ValueToggleButton();
         this.RearArmExtendButton = new NICBOT.GUI.ValueToggleButton();
         this.LowerArmsExtendButton = new NICBOT.GUI.ValueToggleButton();
         this.RearArmRetractButton = new NICBOT.GUI.ValueToggleButton();
         this.FrontNozzleButton = new NICBOT.GUI.ValueToggleButton();
         this.RearNozzleButton = new NICBOT.GUI.ValueToggleButton();
         this.FrontDrillCoverButton = new NICBOT.GUI.ValueToggleButton();
         this.RearDrillCoverButton = new NICBOT.GUI.ValueToggleButton();
         this.FormBorderedPanel.SuspendLayout();
         this.BodyBorderedPanel.SuspendLayout();
         this.SolenoidBorderedPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // FormBorderedPanel
         // 
         this.FormBorderedPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.FormBorderedPanel.Controls.Add(this.BackButton);
         this.FormBorderedPanel.Controls.Add(this.TitleLabel);
         this.FormBorderedPanel.Controls.Add(this.BodyBorderedPanel);
         this.FormBorderedPanel.Controls.Add(this.SolenoidBorderedPanel);
         this.FormBorderedPanel.EdgeWeight = 3;
         this.FormBorderedPanel.Location = new System.Drawing.Point(0, 0);
         this.FormBorderedPanel.Name = "FormBorderedPanel";
         this.FormBorderedPanel.Size = new System.Drawing.Size(768, 484);
         this.FormBorderedPanel.TabIndex = 142;
         // 
         // BackButton
         // 
         this.BackButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BackButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BackButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.BackButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.BackButton.Location = new System.Drawing.Point(559, 387);
         this.BackButton.Name = "BackButton";
         this.BackButton.Size = new System.Drawing.Size(107, 67);
         this.BackButton.TabIndex = 165;
         this.BackButton.Text = "BACK";
         this.BackButton.UseVisualStyleBackColor = false;
         this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
         // 
         // TitleLabel
         // 
         this.TitleLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.TitleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TitleLabel.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
         this.TitleLabel.Location = new System.Drawing.Point(16, 16);
         this.TitleLabel.Name = "TitleLabel";
         this.TitleLabel.Size = new System.Drawing.Size(736, 36);
         this.TitleLabel.TabIndex = 143;
         this.TitleLabel.Text = "CUSTOM CONFIGURATION";
         this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // BodyBorderedPanel
         // 
         this.BodyBorderedPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
         this.BodyBorderedPanel.Controls.Add(this.BodyRearReleaseButton);
         this.BodyBorderedPanel.Controls.Add(this.BodyFrontReleaseButton);
         this.BodyBorderedPanel.Controls.Add(this.BodyOpenButton);
         this.BodyBorderedPanel.Controls.Add(this.BodyDrillButton);
         this.BodyBorderedPanel.Controls.Add(this.BodyClosedButton);
         this.BodyBorderedPanel.Controls.Add(this.BodyOffButton);
         this.BodyBorderedPanel.EdgeWeight = 2;
         this.BodyBorderedPanel.Location = new System.Drawing.Point(27, 68);
         this.BodyBorderedPanel.Name = "BodyBorderedPanel";
         this.BodyBorderedPanel.Size = new System.Drawing.Size(238, 302);
         this.BodyBorderedPanel.TabIndex = 142;
         // 
         // BodyRearReleaseButton
         // 
         this.BodyRearReleaseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BodyRearReleaseButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BodyRearReleaseButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BodyRearReleaseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.BodyRearReleaseButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.BodyRearReleaseButton.Location = new System.Drawing.Point(8, 204);
         this.BodyRearReleaseButton.Name = "BodyRearReleaseButton";
         this.BodyRearReleaseButton.Size = new System.Drawing.Size(107, 90);
         this.BodyRearReleaseButton.TabIndex = 168;
         this.BodyRearReleaseButton.Text = "REAR RELEASE";
         this.BodyRearReleaseButton.UseVisualStyleBackColor = false;
         this.BodyRearReleaseButton.Click += new System.EventHandler(this.BodyRearReleaseButton_Click);
         // 
         // BodyFrontReleaseButton
         // 
         this.BodyFrontReleaseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BodyFrontReleaseButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BodyFrontReleaseButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BodyFrontReleaseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.BodyFrontReleaseButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.BodyFrontReleaseButton.Location = new System.Drawing.Point(123, 204);
         this.BodyFrontReleaseButton.Name = "BodyFrontReleaseButton";
         this.BodyFrontReleaseButton.Size = new System.Drawing.Size(107, 90);
         this.BodyFrontReleaseButton.TabIndex = 167;
         this.BodyFrontReleaseButton.Text = "FRONT RELEASE";
         this.BodyFrontReleaseButton.UseVisualStyleBackColor = false;
         this.BodyFrontReleaseButton.Click += new System.EventHandler(this.BodyFrontReleaseButton_Click);
         // 
         // BodyOpenButton
         // 
         this.BodyOpenButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BodyOpenButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BodyOpenButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BodyOpenButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.BodyOpenButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.BodyOpenButton.Location = new System.Drawing.Point(123, 106);
         this.BodyOpenButton.Name = "BodyOpenButton";
         this.BodyOpenButton.Size = new System.Drawing.Size(107, 90);
         this.BodyOpenButton.TabIndex = 166;
         this.BodyOpenButton.Text = "OPEN (MOVE)";
         this.BodyOpenButton.UseVisualStyleBackColor = false;
         this.BodyOpenButton.Click += new System.EventHandler(this.BodyOpenButton_Click);
         // 
         // BodyDrillButton
         // 
         this.BodyDrillButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BodyDrillButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BodyDrillButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BodyDrillButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.BodyDrillButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.BodyDrillButton.Location = new System.Drawing.Point(123, 8);
         this.BodyDrillButton.Name = "BodyDrillButton";
         this.BodyDrillButton.Size = new System.Drawing.Size(107, 90);
         this.BodyDrillButton.TabIndex = 163;
         this.BodyDrillButton.Text = "DRILL";
         this.BodyDrillButton.UseVisualStyleBackColor = false;
         this.BodyDrillButton.Click += new System.EventHandler(this.BodyDrillButton_Click);
         // 
         // BodyClosedButton
         // 
         this.BodyClosedButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BodyClosedButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BodyClosedButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BodyClosedButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.BodyClosedButton.ForeColor = System.Drawing.Color.Black;
         this.BodyClosedButton.HoldArrorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.BodyClosedButton.HoldTimeoutEnable = true;
         this.BodyClosedButton.HoldTimeoutInterval = 100;
         this.BodyClosedButton.Location = new System.Drawing.Point(8, 106);
         this.BodyClosedButton.Name = "BodyClosedButton";
         this.BodyClosedButton.Size = new System.Drawing.Size(107, 90);
         this.BodyClosedButton.TabIndex = 144;
         this.BodyClosedButton.Text = "CLOSED (LAUNCH)";
         this.BodyClosedButton.UseVisualStyleBackColor = false;
         this.BodyClosedButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.BodyClosedButton_HoldTimeout);
         // 
         // BodyOffButton
         // 
         this.BodyOffButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BodyOffButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BodyOffButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BodyOffButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.BodyOffButton.ForeColor = System.Drawing.Color.Black;
         this.BodyOffButton.HoldArrorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.BodyOffButton.HoldTimeoutEnable = true;
         this.BodyOffButton.HoldTimeoutInterval = 100;
         this.BodyOffButton.Location = new System.Drawing.Point(8, 8);
         this.BodyOffButton.Name = "BodyOffButton";
         this.BodyOffButton.Size = new System.Drawing.Size(107, 90);
         this.BodyOffButton.TabIndex = 143;
         this.BodyOffButton.Text = "ALL           OFF";
         this.BodyOffButton.UseVisualStyleBackColor = false;
         this.BodyOffButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.BodyOffButton_HoldTimeout);
         // 
         // SolenoidBorderedPanel
         // 
         this.SolenoidBorderedPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
         this.SolenoidBorderedPanel.Controls.Add(this.FrontArmExtendButton);
         this.SolenoidBorderedPanel.Controls.Add(this.FrontArmRetractButton);
         this.SolenoidBorderedPanel.Controls.Add(this.LowerArmsRetractButton);
         this.SolenoidBorderedPanel.Controls.Add(this.RearArmExtendButton);
         this.SolenoidBorderedPanel.Controls.Add(this.LowerArmsExtendButton);
         this.SolenoidBorderedPanel.Controls.Add(this.RearArmRetractButton);
         this.SolenoidBorderedPanel.Controls.Add(this.FrontNozzleButton);
         this.SolenoidBorderedPanel.Controls.Add(this.RearNozzleButton);
         this.SolenoidBorderedPanel.Controls.Add(this.FrontDrillCoverButton);
         this.SolenoidBorderedPanel.Controls.Add(this.RearDrillCoverButton);
         this.SolenoidBorderedPanel.EdgeWeight = 2;
         this.SolenoidBorderedPanel.Location = new System.Drawing.Point(273, 68);
         this.SolenoidBorderedPanel.Name = "SolenoidBorderedPanel";
         this.SolenoidBorderedPanel.Size = new System.Drawing.Size(468, 302);
         this.SolenoidBorderedPanel.TabIndex = 141;
         // 
         // FrontArmExtendButton
         // 
         this.FrontArmExtendButton.AutomaticToggle = true;
         this.FrontArmExtendButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.FrontArmExtendButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.FrontArmExtendButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.FrontArmExtendButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.FrontArmExtendButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.FrontArmExtendButton.ForeColor = System.Drawing.Color.Black;
         this.FrontArmExtendButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.FrontArmExtendButton.HoldEnable = false;
         this.FrontArmExtendButton.HoldTimeoutInterval = 0;
         this.FrontArmExtendButton.Location = new System.Drawing.Point(8, 106);
         this.FrontArmExtendButton.Name = "FrontArmExtendButton";
         this.FrontArmExtendButton.OptionASelected = false;
         this.FrontArmExtendButton.OptionAText = "EXTEND";
         this.FrontArmExtendButton.OptionBSelected = true;
         this.FrontArmExtendButton.OptionBText = "OFF";
         this.FrontArmExtendButton.OptionCenterWidth = 0;
         this.FrontArmExtendButton.OptionEdgeHeight = 8;
         this.FrontArmExtendButton.OptionHeight = 22;
         this.FrontArmExtendButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.FrontArmExtendButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.FrontArmExtendButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.FrontArmExtendButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.FrontArmExtendButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.FrontArmExtendButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.FrontArmExtendButton.OptionWidth = 50;
         this.FrontArmExtendButton.Size = new System.Drawing.Size(107, 90);
         this.FrontArmExtendButton.TabIndex = 144;
         this.FrontArmExtendButton.Text = " FRONT  ARM";
         this.FrontArmExtendButton.UseVisualStyleBackColor = false;
         this.FrontArmExtendButton.Click += new System.EventHandler(this.FrontArmExtendButton_Click);
         // 
         // FrontArmRetractButton
         // 
         this.FrontArmRetractButton.AutomaticToggle = true;
         this.FrontArmRetractButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.FrontArmRetractButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.FrontArmRetractButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.FrontArmRetractButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.FrontArmRetractButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.FrontArmRetractButton.ForeColor = System.Drawing.Color.Black;
         this.FrontArmRetractButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.FrontArmRetractButton.HoldEnable = false;
         this.FrontArmRetractButton.HoldTimeoutInterval = 0;
         this.FrontArmRetractButton.Location = new System.Drawing.Point(123, 106);
         this.FrontArmRetractButton.Name = "FrontArmRetractButton";
         this.FrontArmRetractButton.OptionASelected = true;
         this.FrontArmRetractButton.OptionAText = "RETRACT";
         this.FrontArmRetractButton.OptionBSelected = false;
         this.FrontArmRetractButton.OptionBText = "OFF";
         this.FrontArmRetractButton.OptionCenterWidth = 0;
         this.FrontArmRetractButton.OptionEdgeHeight = 8;
         this.FrontArmRetractButton.OptionHeight = 22;
         this.FrontArmRetractButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.FrontArmRetractButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.FrontArmRetractButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.FrontArmRetractButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.FrontArmRetractButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.FrontArmRetractButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.FrontArmRetractButton.OptionWidth = 50;
         this.FrontArmRetractButton.Size = new System.Drawing.Size(107, 90);
         this.FrontArmRetractButton.TabIndex = 145;
         this.FrontArmRetractButton.Text = " FRONT  ARM";
         this.FrontArmRetractButton.UseVisualStyleBackColor = false;
         this.FrontArmRetractButton.Click += new System.EventHandler(this.FrontArmRetractButton_Click);
         // 
         // LowerArmsRetractButton
         // 
         this.LowerArmsRetractButton.AutomaticToggle = true;
         this.LowerArmsRetractButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.LowerArmsRetractButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.LowerArmsRetractButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.LowerArmsRetractButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.LowerArmsRetractButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LowerArmsRetractButton.ForeColor = System.Drawing.Color.Black;
         this.LowerArmsRetractButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.LowerArmsRetractButton.HoldEnable = false;
         this.LowerArmsRetractButton.HoldTimeoutInterval = 0;
         this.LowerArmsRetractButton.Location = new System.Drawing.Point(123, 204);
         this.LowerArmsRetractButton.Name = "LowerArmsRetractButton";
         this.LowerArmsRetractButton.OptionASelected = true;
         this.LowerArmsRetractButton.OptionAText = "RETRACT";
         this.LowerArmsRetractButton.OptionBSelected = false;
         this.LowerArmsRetractButton.OptionBText = "OFF";
         this.LowerArmsRetractButton.OptionCenterWidth = 0;
         this.LowerArmsRetractButton.OptionEdgeHeight = 8;
         this.LowerArmsRetractButton.OptionHeight = 22;
         this.LowerArmsRetractButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.LowerArmsRetractButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LowerArmsRetractButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.LowerArmsRetractButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.LowerArmsRetractButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LowerArmsRetractButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.LowerArmsRetractButton.OptionWidth = 50;
         this.LowerArmsRetractButton.Size = new System.Drawing.Size(107, 90);
         this.LowerArmsRetractButton.TabIndex = 146;
         this.LowerArmsRetractButton.Text = "LOWER ARMS";
         this.LowerArmsRetractButton.UseVisualStyleBackColor = false;
         this.LowerArmsRetractButton.Click += new System.EventHandler(this.LowerArmsRetractButton_Click);
         // 
         // RearArmExtendButton
         // 
         this.RearArmExtendButton.AutomaticToggle = true;
         this.RearArmExtendButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RearArmExtendButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.RearArmExtendButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.RearArmExtendButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.RearArmExtendButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.RearArmExtendButton.ForeColor = System.Drawing.Color.Black;
         this.RearArmExtendButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.RearArmExtendButton.HoldEnable = false;
         this.RearArmExtendButton.HoldTimeoutInterval = 0;
         this.RearArmExtendButton.Location = new System.Drawing.Point(238, 106);
         this.RearArmExtendButton.Name = "RearArmExtendButton";
         this.RearArmExtendButton.OptionASelected = false;
         this.RearArmExtendButton.OptionAText = "EXTEND";
         this.RearArmExtendButton.OptionBSelected = true;
         this.RearArmExtendButton.OptionBText = "OFF";
         this.RearArmExtendButton.OptionCenterWidth = 0;
         this.RearArmExtendButton.OptionEdgeHeight = 8;
         this.RearArmExtendButton.OptionHeight = 22;
         this.RearArmExtendButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.RearArmExtendButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.RearArmExtendButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.RearArmExtendButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.RearArmExtendButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.RearArmExtendButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.RearArmExtendButton.OptionWidth = 50;
         this.RearArmExtendButton.Size = new System.Drawing.Size(107, 90);
         this.RearArmExtendButton.TabIndex = 146;
         this.RearArmExtendButton.Text = "REAR    ARM ";
         this.RearArmExtendButton.UseVisualStyleBackColor = false;
         this.RearArmExtendButton.Click += new System.EventHandler(this.RearArmExtendButton_Click);
         // 
         // LowerArmsExtendButton
         // 
         this.LowerArmsExtendButton.AutomaticToggle = true;
         this.LowerArmsExtendButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.LowerArmsExtendButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.LowerArmsExtendButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.LowerArmsExtendButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.LowerArmsExtendButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LowerArmsExtendButton.ForeColor = System.Drawing.Color.Black;
         this.LowerArmsExtendButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.LowerArmsExtendButton.HoldEnable = false;
         this.LowerArmsExtendButton.HoldTimeoutInterval = 0;
         this.LowerArmsExtendButton.Location = new System.Drawing.Point(8, 204);
         this.LowerArmsExtendButton.Name = "LowerArmsExtendButton";
         this.LowerArmsExtendButton.OptionASelected = false;
         this.LowerArmsExtendButton.OptionAText = "EXTEND";
         this.LowerArmsExtendButton.OptionBSelected = true;
         this.LowerArmsExtendButton.OptionBText = "OFF";
         this.LowerArmsExtendButton.OptionCenterWidth = 0;
         this.LowerArmsExtendButton.OptionEdgeHeight = 8;
         this.LowerArmsExtendButton.OptionHeight = 22;
         this.LowerArmsExtendButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.LowerArmsExtendButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LowerArmsExtendButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.LowerArmsExtendButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.LowerArmsExtendButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LowerArmsExtendButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.LowerArmsExtendButton.OptionWidth = 50;
         this.LowerArmsExtendButton.Size = new System.Drawing.Size(107, 90);
         this.LowerArmsExtendButton.TabIndex = 145;
         this.LowerArmsExtendButton.Text = "LOWER ARMS";
         this.LowerArmsExtendButton.UseVisualStyleBackColor = false;
         this.LowerArmsExtendButton.Click += new System.EventHandler(this.LowerArmsExtendButton_Click);
         // 
         // RearArmRetractButton
         // 
         this.RearArmRetractButton.AutomaticToggle = true;
         this.RearArmRetractButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RearArmRetractButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.RearArmRetractButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.RearArmRetractButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.RearArmRetractButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.RearArmRetractButton.ForeColor = System.Drawing.Color.Black;
         this.RearArmRetractButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.RearArmRetractButton.HoldEnable = false;
         this.RearArmRetractButton.HoldTimeoutInterval = 0;
         this.RearArmRetractButton.Location = new System.Drawing.Point(353, 106);
         this.RearArmRetractButton.Name = "RearArmRetractButton";
         this.RearArmRetractButton.OptionASelected = true;
         this.RearArmRetractButton.OptionAText = "RETRACT";
         this.RearArmRetractButton.OptionBSelected = false;
         this.RearArmRetractButton.OptionBText = "OFF";
         this.RearArmRetractButton.OptionCenterWidth = 0;
         this.RearArmRetractButton.OptionEdgeHeight = 8;
         this.RearArmRetractButton.OptionHeight = 22;
         this.RearArmRetractButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.RearArmRetractButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.RearArmRetractButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.RearArmRetractButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.RearArmRetractButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.RearArmRetractButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.RearArmRetractButton.OptionWidth = 50;
         this.RearArmRetractButton.Size = new System.Drawing.Size(107, 90);
         this.RearArmRetractButton.TabIndex = 147;
         this.RearArmRetractButton.Text = "REAR    ARM ";
         this.RearArmRetractButton.UseVisualStyleBackColor = false;
         this.RearArmRetractButton.Click += new System.EventHandler(this.RearArmRetractButton_Click);
         // 
         // FrontNozzleButton
         // 
         this.FrontNozzleButton.AutomaticToggle = true;
         this.FrontNozzleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.FrontNozzleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.FrontNozzleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.FrontNozzleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.FrontNozzleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.FrontNozzleButton.ForeColor = System.Drawing.Color.Black;
         this.FrontNozzleButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.FrontNozzleButton.HoldEnable = false;
         this.FrontNozzleButton.HoldTimeoutInterval = 0;
         this.FrontNozzleButton.Location = new System.Drawing.Point(123, 8);
         this.FrontNozzleButton.Name = "FrontNozzleButton";
         this.FrontNozzleButton.OptionASelected = false;
         this.FrontNozzleButton.OptionAText = "EXTEND";
         this.FrontNozzleButton.OptionBSelected = true;
         this.FrontNozzleButton.OptionBText = "RETRACT";
         this.FrontNozzleButton.OptionCenterWidth = 0;
         this.FrontNozzleButton.OptionEdgeHeight = 8;
         this.FrontNozzleButton.OptionHeight = 22;
         this.FrontNozzleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.FrontNozzleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.FrontNozzleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.FrontNozzleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.FrontNozzleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.FrontNozzleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.FrontNozzleButton.OptionWidth = 50;
         this.FrontNozzleButton.Size = new System.Drawing.Size(107, 90);
         this.FrontNozzleButton.TabIndex = 147;
         this.FrontNozzleButton.Text = "FRONT NOZZLE";
         this.FrontNozzleButton.UseVisualStyleBackColor = false;
         this.FrontNozzleButton.Click += new System.EventHandler(this.FrontNozzleButton_Click);
         // 
         // RearNozzleButton
         // 
         this.RearNozzleButton.AutomaticToggle = true;
         this.RearNozzleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RearNozzleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.RearNozzleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.RearNozzleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.RearNozzleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.RearNozzleButton.ForeColor = System.Drawing.Color.Black;
         this.RearNozzleButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.RearNozzleButton.HoldEnable = false;
         this.RearNozzleButton.HoldTimeoutInterval = 0;
         this.RearNozzleButton.Location = new System.Drawing.Point(353, 8);
         this.RearNozzleButton.Name = "RearNozzleButton";
         this.RearNozzleButton.OptionASelected = false;
         this.RearNozzleButton.OptionAText = "EXTEND";
         this.RearNozzleButton.OptionBSelected = true;
         this.RearNozzleButton.OptionBText = "RETRACT";
         this.RearNozzleButton.OptionCenterWidth = 0;
         this.RearNozzleButton.OptionEdgeHeight = 8;
         this.RearNozzleButton.OptionHeight = 22;
         this.RearNozzleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.RearNozzleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.RearNozzleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.RearNozzleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.RearNozzleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.RearNozzleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.RearNozzleButton.OptionWidth = 50;
         this.RearNozzleButton.Size = new System.Drawing.Size(107, 90);
         this.RearNozzleButton.TabIndex = 146;
         this.RearNozzleButton.Text = "REAR NOZZLE";
         this.RearNozzleButton.UseVisualStyleBackColor = false;
         this.RearNozzleButton.Click += new System.EventHandler(this.RearNozzleButton_Click);
         // 
         // FrontDrillCoverButton
         // 
         this.FrontDrillCoverButton.AutomaticToggle = true;
         this.FrontDrillCoverButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.FrontDrillCoverButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.FrontDrillCoverButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.FrontDrillCoverButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.FrontDrillCoverButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.FrontDrillCoverButton.ForeColor = System.Drawing.Color.Black;
         this.FrontDrillCoverButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.FrontDrillCoverButton.HoldEnable = false;
         this.FrontDrillCoverButton.HoldTimeoutInterval = 0;
         this.FrontDrillCoverButton.Location = new System.Drawing.Point(8, 8);
         this.FrontDrillCoverButton.Name = "FrontDrillCoverButton";
         this.FrontDrillCoverButton.OptionASelected = false;
         this.FrontDrillCoverButton.OptionAText = "OPEN";
         this.FrontDrillCoverButton.OptionBSelected = true;
         this.FrontDrillCoverButton.OptionBText = "CLOSE";
         this.FrontDrillCoverButton.OptionCenterWidth = 0;
         this.FrontDrillCoverButton.OptionEdgeHeight = 8;
         this.FrontDrillCoverButton.OptionHeight = 22;
         this.FrontDrillCoverButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.FrontDrillCoverButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.FrontDrillCoverButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.FrontDrillCoverButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.FrontDrillCoverButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.FrontDrillCoverButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.FrontDrillCoverButton.OptionWidth = 50;
         this.FrontDrillCoverButton.Size = new System.Drawing.Size(107, 90);
         this.FrontDrillCoverButton.TabIndex = 146;
         this.FrontDrillCoverButton.Text = "FRONT     DRILL     COVER";
         this.FrontDrillCoverButton.UseVisualStyleBackColor = false;
         this.FrontDrillCoverButton.Click += new System.EventHandler(this.FrontDrillCoverButton_Click);
         // 
         // RearDrillCoverButton
         // 
         this.RearDrillCoverButton.AutomaticToggle = true;
         this.RearDrillCoverButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RearDrillCoverButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.RearDrillCoverButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.RearDrillCoverButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.RearDrillCoverButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.RearDrillCoverButton.ForeColor = System.Drawing.Color.Black;
         this.RearDrillCoverButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.RearDrillCoverButton.HoldEnable = false;
         this.RearDrillCoverButton.HoldTimeoutInterval = 0;
         this.RearDrillCoverButton.Location = new System.Drawing.Point(238, 8);
         this.RearDrillCoverButton.Name = "RearDrillCoverButton";
         this.RearDrillCoverButton.OptionASelected = false;
         this.RearDrillCoverButton.OptionAText = "OPEN";
         this.RearDrillCoverButton.OptionBSelected = true;
         this.RearDrillCoverButton.OptionBText = "CLOSE";
         this.RearDrillCoverButton.OptionCenterWidth = 0;
         this.RearDrillCoverButton.OptionEdgeHeight = 8;
         this.RearDrillCoverButton.OptionHeight = 22;
         this.RearDrillCoverButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.RearDrillCoverButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.RearDrillCoverButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.RearDrillCoverButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.RearDrillCoverButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.RearDrillCoverButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.RearDrillCoverButton.OptionWidth = 50;
         this.RearDrillCoverButton.Size = new System.Drawing.Size(107, 90);
         this.RearDrillCoverButton.TabIndex = 143;
         this.RearDrillCoverButton.Text = "REAR     DRILL     COVER";
         this.RearDrillCoverButton.UseVisualStyleBackColor = false;
         this.RearDrillCoverButton.Click += new System.EventHandler(this.RearDrillCoverButton_Click);
         // 
         // RepairBodySetupForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.Black;
         this.ClientSize = new System.Drawing.Size(768, 484);
         this.Controls.Add(this.FormBorderedPanel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "RepairBodySetupForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "BodySetup";
         this.Shown += new System.EventHandler(this.BodySetupForm_Shown);
         this.FormBorderedPanel.ResumeLayout(false);
         this.BodyBorderedPanel.ResumeLayout(false);
         this.SolenoidBorderedPanel.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private BorderedPanel FormBorderedPanel;
      private BorderedPanel SolenoidBorderedPanel;
      private BorderedPanel BodyBorderedPanel;
      private HoldButton BodyOffButton;
      private HoldButton BodyClosedButton;
      private ValueToggleButton FrontArmRetractButton;
      private ValueToggleButton FrontArmExtendButton;
      private ValueToggleButton RearDrillCoverButton;
      private ValueToggleButton FrontDrillCoverButton;
      private ValueToggleButton FrontNozzleButton;
      private ValueToggleButton RearNozzleButton;
      private ValueToggleButton RearArmExtendButton;
      private ValueToggleButton RearArmRetractButton;
      private ValueToggleButton LowerArmsRetractButton;
      private ValueToggleButton LowerArmsExtendButton;
      private System.Windows.Forms.Label TitleLabel;
      private NicBotButton BodyDrillButton;
      private NicBotButton BackButton;
      private NicBotButton BodyOpenButton;
      private NicBotButton BodyFrontReleaseButton;
      private NicBotButton BodyRearReleaseButton;
   }
}