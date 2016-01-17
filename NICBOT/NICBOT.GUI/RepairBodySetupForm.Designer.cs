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
         this.components = new System.ComponentModel.Container();
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
         this.BottomRearWheelIndicatorTextBox = new System.Windows.Forms.TextBox();
         this.label6 = new System.Windows.Forms.Label();
         this.BottomFrontWheelIndicatorTextBox = new System.Windows.Forms.TextBox();
         this.label5 = new System.Windows.Forms.Label();
         this.TopRearWheelIndicatorTextBox = new System.Windows.Forms.TextBox();
         this.label4 = new System.Windows.Forms.Label();
         this.TopFrontWheelIndicatorTextBox = new System.Windows.Forms.TextBox();
         this.label3 = new System.Windows.Forms.Label();
         this.WheelLockButton = new NICBOT.GUI.ValueToggleButton();
         this.WheelsAxialButton = new NICBOT.GUI.ValueToggleButton();
         this.FrontArmExtendButton = new NICBOT.GUI.ValueToggleButton();
         this.WheelsCircumferenceButton = new NICBOT.GUI.ValueToggleButton();
         this.FrontArmRetractButton = new NICBOT.GUI.ValueToggleButton();
         this.LowerArmsRetractButton = new NICBOT.GUI.ValueToggleButton();
         this.RearArmExtendButton = new NICBOT.GUI.ValueToggleButton();
         this.LowerArmsExtendButton = new NICBOT.GUI.ValueToggleButton();
         this.RearArmRetractButton = new NICBOT.GUI.ValueToggleButton();
         this.FrontNozzleButton = new NICBOT.GUI.ValueToggleButton();
         this.RearNozzleButton = new NICBOT.GUI.ValueToggleButton();
         this.FrontDrillCoverButton = new NICBOT.GUI.ValueToggleButton();
         this.RearDrillCoverButton = new NICBOT.GUI.ValueToggleButton();
         this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
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
         this.FormBorderedPanel.Size = new System.Drawing.Size(752, 484);
         this.FormBorderedPanel.TabIndex = 142;
         // 
         // BackButton
         // 
         this.BackButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BackButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BackButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.BackButton.Location = new System.Drawing.Point(84, 387);
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
         this.TitleLabel.Size = new System.Drawing.Size(720, 36);
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
         this.BodyBorderedPanel.Location = new System.Drawing.Point(19, 68);
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
         this.SolenoidBorderedPanel.Controls.Add(this.BottomRearWheelIndicatorTextBox);
         this.SolenoidBorderedPanel.Controls.Add(this.label6);
         this.SolenoidBorderedPanel.Controls.Add(this.BottomFrontWheelIndicatorTextBox);
         this.SolenoidBorderedPanel.Controls.Add(this.label5);
         this.SolenoidBorderedPanel.Controls.Add(this.TopRearWheelIndicatorTextBox);
         this.SolenoidBorderedPanel.Controls.Add(this.label4);
         this.SolenoidBorderedPanel.Controls.Add(this.TopFrontWheelIndicatorTextBox);
         this.SolenoidBorderedPanel.Controls.Add(this.label3);
         this.SolenoidBorderedPanel.Controls.Add(this.WheelLockButton);
         this.SolenoidBorderedPanel.Controls.Add(this.WheelsAxialButton);
         this.SolenoidBorderedPanel.Controls.Add(this.FrontArmExtendButton);
         this.SolenoidBorderedPanel.Controls.Add(this.WheelsCircumferenceButton);
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
         this.SolenoidBorderedPanel.Location = new System.Drawing.Point(265, 68);
         this.SolenoidBorderedPanel.Name = "SolenoidBorderedPanel";
         this.SolenoidBorderedPanel.Size = new System.Drawing.Size(468, 400);
         this.SolenoidBorderedPanel.TabIndex = 141;
         // 
         // BottomRearWheelIndicatorTextBox
         // 
         this.BottomRearWheelIndicatorTextBox.BackColor = System.Drawing.Color.Black;
         this.BottomRearWheelIndicatorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.BottomRearWheelIndicatorTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomRearWheelIndicatorTextBox.ForeColor = System.Drawing.SystemColors.Window;
         this.BottomRearWheelIndicatorTextBox.Location = new System.Drawing.Point(144, 346);
         this.BottomRearWheelIndicatorTextBox.Name = "BottomRearWheelIndicatorTextBox";
         this.BottomRearWheelIndicatorTextBox.ReadOnly = true;
         this.BottomRearWheelIndicatorTextBox.Size = new System.Drawing.Size(35, 17);
         this.BottomRearWheelIndicatorTextBox.TabIndex = 189;
         this.BottomRearWheelIndicatorTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.label6.Location = new System.Drawing.Point(180, 348);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(80, 13);
         this.label6.TabIndex = 190;
         this.label6.Text = "LOWER REAR";
         this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // BottomFrontWheelIndicatorTextBox
         // 
         this.BottomFrontWheelIndicatorTextBox.BackColor = System.Drawing.Color.Black;
         this.BottomFrontWheelIndicatorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.BottomFrontWheelIndicatorTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomFrontWheelIndicatorTextBox.ForeColor = System.Drawing.SystemColors.Window;
         this.BottomFrontWheelIndicatorTextBox.Location = new System.Drawing.Point(283, 346);
         this.BottomFrontWheelIndicatorTextBox.Name = "BottomFrontWheelIndicatorTextBox";
         this.BottomFrontWheelIndicatorTextBox.ReadOnly = true;
         this.BottomFrontWheelIndicatorTextBox.Size = new System.Drawing.Size(35, 17);
         this.BottomFrontWheelIndicatorTextBox.TabIndex = 187;
         this.BottomFrontWheelIndicatorTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.label5.Location = new System.Drawing.Point(319, 348);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(87, 13);
         this.label5.TabIndex = 188;
         this.label5.Text = "LOWER FRONT";
         this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // TopRearWheelIndicatorTextBox
         // 
         this.TopRearWheelIndicatorTextBox.BackColor = System.Drawing.Color.Black;
         this.TopRearWheelIndicatorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.TopRearWheelIndicatorTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopRearWheelIndicatorTextBox.ForeColor = System.Drawing.SystemColors.Window;
         this.TopRearWheelIndicatorTextBox.Location = new System.Drawing.Point(144, 323);
         this.TopRearWheelIndicatorTextBox.Name = "TopRearWheelIndicatorTextBox";
         this.TopRearWheelIndicatorTextBox.ReadOnly = true;
         this.TopRearWheelIndicatorTextBox.Size = new System.Drawing.Size(35, 17);
         this.TopRearWheelIndicatorTextBox.TabIndex = 185;
         this.TopRearWheelIndicatorTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.label4.Location = new System.Drawing.Point(180, 325);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(77, 13);
         this.label4.TabIndex = 186;
         this.label4.Text = "UPPER REAR";
         this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // TopFrontWheelIndicatorTextBox
         // 
         this.TopFrontWheelIndicatorTextBox.BackColor = System.Drawing.Color.Black;
         this.TopFrontWheelIndicatorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.TopFrontWheelIndicatorTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopFrontWheelIndicatorTextBox.ForeColor = System.Drawing.SystemColors.Window;
         this.TopFrontWheelIndicatorTextBox.Location = new System.Drawing.Point(283, 323);
         this.TopFrontWheelIndicatorTextBox.Name = "TopFrontWheelIndicatorTextBox";
         this.TopFrontWheelIndicatorTextBox.ReadOnly = true;
         this.TopFrontWheelIndicatorTextBox.Size = new System.Drawing.Size(35, 17);
         this.TopFrontWheelIndicatorTextBox.TabIndex = 183;
         this.TopFrontWheelIndicatorTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.label3.Location = new System.Drawing.Point(319, 325);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(84, 13);
         this.label3.TabIndex = 184;
         this.label3.Text = "UPPER FRONT";
         this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // WheelLockButton
         // 
         this.WheelLockButton.AutomaticToggle = true;
         this.WheelLockButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.WheelLockButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.WheelLockButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.WheelLockButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.WheelLockButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.WheelLockButton.ForeColor = System.Drawing.Color.Black;
         this.WheelLockButton.HoldEnable = false;
         this.WheelLockButton.HoldTimeoutInterval = 0;
         this.WheelLockButton.Location = new System.Drawing.Point(8, 302);
         this.WheelLockButton.Name = "WheelLockButton";
         this.WheelLockButton.OptionASelected = false;
         this.WheelLockButton.OptionAText = "LOCK";
         this.WheelLockButton.OptionBSelected = true;
         this.WheelLockButton.OptionBText = "OFF";
         this.WheelLockButton.OptionCenterWidth = 0;
         this.WheelLockButton.OptionEdgeHeight = 8;
         this.WheelLockButton.OptionHeight = 22;
         this.WheelLockButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.WheelLockButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.WheelLockButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.WheelLockButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.WheelLockButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.WheelLockButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.WheelLockButton.OptionWidth = 50;
         this.WheelLockButton.Size = new System.Drawing.Size(107, 90);
         this.WheelLockButton.TabIndex = 182;
         this.WheelLockButton.Text = "WHEEL LOCK";
         this.WheelLockButton.UseVisualStyleBackColor = false;
         this.WheelLockButton.Click += new System.EventHandler(this.WheelLockButton_Click);
         // 
         // WheelsAxialButton
         // 
         this.WheelsAxialButton.AutomaticToggle = true;
         this.WheelsAxialButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.WheelsAxialButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.WheelsAxialButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.WheelsAxialButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.WheelsAxialButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.WheelsAxialButton.ForeColor = System.Drawing.Color.Black;
         this.WheelsAxialButton.HoldEnable = false;
         this.WheelsAxialButton.HoldTimeoutInterval = 0;
         this.WheelsAxialButton.Location = new System.Drawing.Point(353, 204);
         this.WheelsAxialButton.Name = "WheelsAxialButton";
         this.WheelsAxialButton.OptionASelected = false;
         this.WheelsAxialButton.OptionAText = "AXIAL";
         this.WheelsAxialButton.OptionBSelected = true;
         this.WheelsAxialButton.OptionBText = "OFF";
         this.WheelsAxialButton.OptionCenterWidth = 0;
         this.WheelsAxialButton.OptionEdgeHeight = 8;
         this.WheelsAxialButton.OptionHeight = 22;
         this.WheelsAxialButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.WheelsAxialButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.WheelsAxialButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.WheelsAxialButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.WheelsAxialButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.WheelsAxialButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.WheelsAxialButton.OptionWidth = 50;
         this.WheelsAxialButton.Size = new System.Drawing.Size(107, 90);
         this.WheelsAxialButton.TabIndex = 148;
         this.WheelsAxialButton.Text = "WHEELS";
         this.WheelsAxialButton.UseVisualStyleBackColor = false;
         this.WheelsAxialButton.Click += new System.EventHandler(this.WheelsAxialButton_Click);
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
         // WheelsCircumferenceButton
         // 
         this.WheelsCircumferenceButton.AutomaticToggle = true;
         this.WheelsCircumferenceButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.WheelsCircumferenceButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.WheelsCircumferenceButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.WheelsCircumferenceButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.WheelsCircumferenceButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.WheelsCircumferenceButton.ForeColor = System.Drawing.Color.Black;
         this.WheelsCircumferenceButton.HoldEnable = false;
         this.WheelsCircumferenceButton.HoldTimeoutInterval = 0;
         this.WheelsCircumferenceButton.Location = new System.Drawing.Point(237, 204);
         this.WheelsCircumferenceButton.Name = "WheelsCircumferenceButton";
         this.WheelsCircumferenceButton.OptionASelected = false;
         this.WheelsCircumferenceButton.OptionAText = "CIRC";
         this.WheelsCircumferenceButton.OptionBSelected = true;
         this.WheelsCircumferenceButton.OptionBText = "OFF";
         this.WheelsCircumferenceButton.OptionCenterWidth = 0;
         this.WheelsCircumferenceButton.OptionEdgeHeight = 8;
         this.WheelsCircumferenceButton.OptionHeight = 22;
         this.WheelsCircumferenceButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.WheelsCircumferenceButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.WheelsCircumferenceButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.WheelsCircumferenceButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.WheelsCircumferenceButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.WheelsCircumferenceButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.WheelsCircumferenceButton.OptionWidth = 50;
         this.WheelsCircumferenceButton.Size = new System.Drawing.Size(107, 90);
         this.WheelsCircumferenceButton.TabIndex = 147;
         this.WheelsCircumferenceButton.Text = "WHEELS";
         this.WheelsCircumferenceButton.UseVisualStyleBackColor = false;
         this.WheelsCircumferenceButton.Click += new System.EventHandler(this.WheelsCircumferenceButton_Click);
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
         this.RearArmExtendButton.HoldEnable = false;
         this.RearArmExtendButton.HoldTimeoutInterval = 0;
         this.RearArmExtendButton.Location = new System.Drawing.Point(237, 106);
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
         this.RearDrillCoverButton.HoldEnable = false;
         this.RearDrillCoverButton.HoldTimeoutInterval = 0;
         this.RearDrillCoverButton.Location = new System.Drawing.Point(237, 8);
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
         // UpdateTimer
         // 
         this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
         // 
         // RepairBodySetupForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.Black;
         this.ClientSize = new System.Drawing.Size(752, 484);
         this.Controls.Add(this.FormBorderedPanel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "RepairBodySetupForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "BodySetup";
         this.Shown += new System.EventHandler(this.BodySetupForm_Shown);
         this.FormBorderedPanel.ResumeLayout(false);
         this.BodyBorderedPanel.ResumeLayout(false);
         this.SolenoidBorderedPanel.ResumeLayout(false);
         this.SolenoidBorderedPanel.PerformLayout();
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
      private ValueToggleButton WheelsAxialButton;
      private ValueToggleButton WheelsCircumferenceButton;
      private ValueToggleButton LowerArmsRetractButton;
      private ValueToggleButton LowerArmsExtendButton;
      private System.Windows.Forms.Label TitleLabel;
      private NicBotButton BodyDrillButton;
      private NicBotButton BackButton;
      private NicBotButton BodyOpenButton;
      private NicBotButton BodyFrontReleaseButton;
      private NicBotButton BodyRearReleaseButton;
      private System.Windows.Forms.TextBox BottomRearWheelIndicatorTextBox;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.TextBox BottomFrontWheelIndicatorTextBox;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.TextBox TopRearWheelIndicatorTextBox;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.TextBox TopFrontWheelIndicatorTextBox;
      private System.Windows.Forms.Label label3;
      private ValueToggleButton WheelLockButton;
      private System.Windows.Forms.Timer UpdateTimer;
   }
}