namespace NICBOT.GUI
{
   partial class FeederSetupForm
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
         this.TrackingCalibrationValueButton = new NICBOT.GUI.ValueButton();
         this.SpeedTrackingToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.LowSpeedScaleValueButton = new NICBOT.GUI.ValueButton();
         this.MaxSpeedValueButton = new NICBOT.GUI.ValueButton();
         this.LockCurrentValueButton = new NICBOT.GUI.ValueButton();
         this.BottomRearPanel = new NICBOT.GUI.BorderedPanel();
         this.BottomRearStateCycleButton = new NICBOT.GUI.ValueCycleButton();
         this.BottomRearLabel = new System.Windows.Forms.Label();
         this.BottomRearDirectionToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.BottomFrontPanel = new NICBOT.GUI.BorderedPanel();
         this.BottomFrontStateCycleButton = new NICBOT.GUI.ValueCycleButton();
         this.BottomFrontLabel = new System.Windows.Forms.Label();
         this.BottomFrontDirectionToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.TopRearPanel = new NICBOT.GUI.BorderedPanel();
         this.TopRearStateCycleButton = new NICBOT.GUI.ValueCycleButton();
         this.TopRearLabel = new System.Windows.Forms.Label();
         this.TopRearDirectionToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.TopFrontPanel = new NICBOT.GUI.BorderedPanel();
         this.TopFrontStateCycleButton = new NICBOT.GUI.ValueCycleButton();
         this.TopFrontLabel = new System.Windows.Forms.Label();
         this.TopFrontDirectionToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.TitleLabel = new System.Windows.Forms.Label();
         this.CurrentPer1KValueButton = new NICBOT.GUI.ValueButton();
         this.MainPanel.SuspendLayout();
         this.BottomRearPanel.SuspendLayout();
         this.BottomFrontPanel.SuspendLayout();
         this.TopRearPanel.SuspendLayout();
         this.TopFrontPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // MainPanel
         // 
         this.MainPanel.BackColor = System.Drawing.Color.Olive;
         this.MainPanel.Controls.Add(this.CurrentPer1KValueButton);
         this.MainPanel.Controls.Add(this.BackButton);
         this.MainPanel.Controls.Add(this.TrackingCalibrationValueButton);
         this.MainPanel.Controls.Add(this.SpeedTrackingToggleButton);
         this.MainPanel.Controls.Add(this.LowSpeedScaleValueButton);
         this.MainPanel.Controls.Add(this.MaxSpeedValueButton);
         this.MainPanel.Controls.Add(this.LockCurrentValueButton);
         this.MainPanel.Controls.Add(this.BottomRearPanel);
         this.MainPanel.Controls.Add(this.BottomFrontPanel);
         this.MainPanel.Controls.Add(this.TopRearPanel);
         this.MainPanel.Controls.Add(this.TopFrontPanel);
         this.MainPanel.Controls.Add(this.TitleLabel);
         this.MainPanel.EdgeWeight = 3;
         this.MainPanel.Location = new System.Drawing.Point(0, 0);
         this.MainPanel.Name = "MainPanel";
         this.MainPanel.Size = new System.Drawing.Size(621, 554);
         this.MainPanel.TabIndex = 1;
         // 
         // BackButton
         // 
         this.BackButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BackButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BackButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.BackButton.Location = new System.Drawing.Point(465, 404);
         this.BackButton.Name = "BackButton";
         this.BackButton.Size = new System.Drawing.Size(107, 67);
         this.BackButton.TabIndex = 174;
         this.BackButton.Text = "BACK";
         this.BackButton.UseVisualStyleBackColor = false;
         this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
         // 
         // TrackingCalibrationValueButton
         // 
         this.TrackingCalibrationValueButton.ArrowWidth = 0;
         this.TrackingCalibrationValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.TrackingCalibrationValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.TrackingCalibrationValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.TrackingCalibrationValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.TrackingCalibrationValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
         this.TrackingCalibrationValueButton.HoldTimeoutInterval = 0;
         this.TrackingCalibrationValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.TrackingCalibrationValueButton.LeftArrowVisible = false;
         this.TrackingCalibrationValueButton.Location = new System.Drawing.Point(184, 342);
         this.TrackingCalibrationValueButton.Name = "TrackingCalibrationValueButton";
         this.TrackingCalibrationValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.TrackingCalibrationValueButton.RightArrowVisible = false;
         this.TrackingCalibrationValueButton.Size = new System.Drawing.Size(107, 90);
         this.TrackingCalibrationValueButton.TabIndex = 173;
         this.TrackingCalibrationValueButton.Text = "TRACKING CALIBRATION";
         this.TrackingCalibrationValueButton.UseVisualStyleBackColor = false;
         this.TrackingCalibrationValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.TrackingCalibrationValueButton.ValueEdgeHeight = 8;
         this.TrackingCalibrationValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.TrackingCalibrationValueButton.ValueForeColor = System.Drawing.Color.White;
         this.TrackingCalibrationValueButton.ValueHeight = 22;
         this.TrackingCalibrationValueButton.ValueText = "+/- ## %";
         this.TrackingCalibrationValueButton.ValueWidth = 80;
         this.TrackingCalibrationValueButton.Click += new System.EventHandler(this.TrackingCalibrationValueButton_Click);
         // 
         // SpeedTrackingToggleButton
         // 
         this.SpeedTrackingToggleButton.AutomaticToggle = true;
         this.SpeedTrackingToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SpeedTrackingToggleButton.DisabledBackColor = System.Drawing.Color.Black;
         this.SpeedTrackingToggleButton.DisabledForeColor = System.Drawing.Color.Gray;
         this.SpeedTrackingToggleButton.DisabledOptionBackColor = System.Drawing.Color.Black;
         this.SpeedTrackingToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
         this.SpeedTrackingToggleButton.HoldEnable = false;
         this.SpeedTrackingToggleButton.HoldTimeoutInterval = 0;
         this.SpeedTrackingToggleButton.Location = new System.Drawing.Point(61, 342);
         this.SpeedTrackingToggleButton.Name = "SpeedTrackingToggleButton";
         this.SpeedTrackingToggleButton.OptionASelected = false;
         this.SpeedTrackingToggleButton.OptionAText = "AUTO";
         this.SpeedTrackingToggleButton.OptionBSelected = true;
         this.SpeedTrackingToggleButton.OptionBText = "MANUAL";
         this.SpeedTrackingToggleButton.OptionCenterWidth = 0;
         this.SpeedTrackingToggleButton.OptionEdgeHeight = 8;
         this.SpeedTrackingToggleButton.OptionHeight = 22;
         this.SpeedTrackingToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.SpeedTrackingToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SpeedTrackingToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.SpeedTrackingToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.SpeedTrackingToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SpeedTrackingToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.SpeedTrackingToggleButton.OptionWidth = 50;
         this.SpeedTrackingToggleButton.Size = new System.Drawing.Size(107, 90);
         this.SpeedTrackingToggleButton.TabIndex = 172;
         this.SpeedTrackingToggleButton.Text = "SPEED TRACKING";
         this.SpeedTrackingToggleButton.UseVisualStyleBackColor = false;
         this.SpeedTrackingToggleButton.Click += new System.EventHandler(this.SpeedTrackingToggleButton_Click);
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
         this.LowSpeedScaleValueButton.Location = new System.Drawing.Point(307, 342);
         this.LowSpeedScaleValueButton.Name = "LowSpeedScaleValueButton";
         this.LowSpeedScaleValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.LowSpeedScaleValueButton.RightArrowVisible = false;
         this.LowSpeedScaleValueButton.Size = new System.Drawing.Size(107, 90);
         this.LowSpeedScaleValueButton.TabIndex = 171;
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
         this.MaxSpeedValueButton.Location = new System.Drawing.Point(307, 448);
         this.MaxSpeedValueButton.Name = "MaxSpeedValueButton";
         this.MaxSpeedValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.MaxSpeedValueButton.RightArrowVisible = false;
         this.MaxSpeedValueButton.Size = new System.Drawing.Size(107, 90);
         this.MaxSpeedValueButton.TabIndex = 170;
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
         this.LockCurrentValueButton.Location = new System.Drawing.Point(184, 448);
         this.LockCurrentValueButton.Name = "LockCurrentValueButton";
         this.LockCurrentValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.LockCurrentValueButton.RightArrowVisible = false;
         this.LockCurrentValueButton.Size = new System.Drawing.Size(107, 90);
         this.LockCurrentValueButton.TabIndex = 169;
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
         // BottomRearPanel
         // 
         this.BottomRearPanel.BackColor = System.Drawing.Color.DarkOliveGreen;
         this.BottomRearPanel.Controls.Add(this.BottomRearStateCycleButton);
         this.BottomRearPanel.Controls.Add(this.BottomRearLabel);
         this.BottomRearPanel.Controls.Add(this.BottomRearDirectionToggleButton);
         this.BottomRearPanel.EdgeWeight = 2;
         this.BottomRearPanel.Location = new System.Drawing.Point(465, 68);
         this.BottomRearPanel.Name = "BottomRearPanel";
         this.BottomRearPanel.Size = new System.Drawing.Size(129, 260);
         this.BottomRearPanel.TabIndex = 165;
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
         // BottomRearLabel
         // 
         this.BottomRearLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
         this.BottomRearLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomRearLabel.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
         this.BottomRearLabel.Location = new System.Drawing.Point(2, 2);
         this.BottomRearLabel.Name = "BottomRearLabel";
         this.BottomRearLabel.Size = new System.Drawing.Size(124, 51);
         this.BottomRearLabel.TabIndex = 153;
         this.BottomRearLabel.Text = "BOTTOM REAR";
         this.BottomRearLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
         // BottomFrontPanel
         // 
         this.BottomFrontPanel.BackColor = System.Drawing.Color.DarkOliveGreen;
         this.BottomFrontPanel.Controls.Add(this.BottomFrontStateCycleButton);
         this.BottomFrontPanel.Controls.Add(this.BottomFrontLabel);
         this.BottomFrontPanel.Controls.Add(this.BottomFrontDirectionToggleButton);
         this.BottomFrontPanel.EdgeWeight = 2;
         this.BottomFrontPanel.Location = new System.Drawing.Point(319, 68);
         this.BottomFrontPanel.Name = "BottomFrontPanel";
         this.BottomFrontPanel.Size = new System.Drawing.Size(129, 260);
         this.BottomFrontPanel.TabIndex = 166;
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
         // BottomFrontLabel
         // 
         this.BottomFrontLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
         this.BottomFrontLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BottomFrontLabel.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
         this.BottomFrontLabel.Location = new System.Drawing.Point(2, 2);
         this.BottomFrontLabel.Name = "BottomFrontLabel";
         this.BottomFrontLabel.Size = new System.Drawing.Size(124, 51);
         this.BottomFrontLabel.TabIndex = 153;
         this.BottomFrontLabel.Text = "BOTTOM FRONT";
         this.BottomFrontLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
         // TopRearPanel
         // 
         this.TopRearPanel.BackColor = System.Drawing.Color.DarkOliveGreen;
         this.TopRearPanel.Controls.Add(this.TopRearStateCycleButton);
         this.TopRearPanel.Controls.Add(this.TopRearLabel);
         this.TopRearPanel.Controls.Add(this.TopRearDirectionToggleButton);
         this.TopRearPanel.EdgeWeight = 2;
         this.TopRearPanel.Location = new System.Drawing.Point(173, 68);
         this.TopRearPanel.Name = "TopRearPanel";
         this.TopRearPanel.Size = new System.Drawing.Size(129, 260);
         this.TopRearPanel.TabIndex = 167;
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
         // TopRearLabel
         // 
         this.TopRearLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
         this.TopRearLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopRearLabel.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
         this.TopRearLabel.Location = new System.Drawing.Point(2, 2);
         this.TopRearLabel.Name = "TopRearLabel";
         this.TopRearLabel.Size = new System.Drawing.Size(124, 51);
         this.TopRearLabel.TabIndex = 153;
         this.TopRearLabel.Text = "  TOP   REAR";
         this.TopRearLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
         // TopFrontPanel
         // 
         this.TopFrontPanel.BackColor = System.Drawing.Color.DarkOliveGreen;
         this.TopFrontPanel.Controls.Add(this.TopFrontStateCycleButton);
         this.TopFrontPanel.Controls.Add(this.TopFrontLabel);
         this.TopFrontPanel.Controls.Add(this.TopFrontDirectionToggleButton);
         this.TopFrontPanel.EdgeWeight = 2;
         this.TopFrontPanel.Location = new System.Drawing.Point(27, 68);
         this.TopFrontPanel.Name = "TopFrontPanel";
         this.TopFrontPanel.Size = new System.Drawing.Size(129, 260);
         this.TopFrontPanel.TabIndex = 168;
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
         // TopFrontLabel
         // 
         this.TopFrontLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
         this.TopFrontLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TopFrontLabel.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
         this.TopFrontLabel.Location = new System.Drawing.Point(2, 2);
         this.TopFrontLabel.Name = "TopFrontLabel";
         this.TopFrontLabel.Size = new System.Drawing.Size(124, 51);
         this.TopFrontLabel.TabIndex = 153;
         this.TopFrontLabel.Text = "  TOP   FRONT";
         this.TopFrontLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
         // TitleLabel
         // 
         this.TitleLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.TitleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TitleLabel.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
         this.TitleLabel.Location = new System.Drawing.Point(16, 16);
         this.TitleLabel.Name = "TitleLabel";
         this.TitleLabel.Size = new System.Drawing.Size(589, 36);
         this.TitleLabel.TabIndex = 132;
         this.TitleLabel.Text = "TETHER FEEDER SETUP";
         this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
         this.CurrentPer1KValueButton.Location = new System.Drawing.Point(61, 448);
         this.CurrentPer1KValueButton.Name = "CurrentPer1KValueButton";
         this.CurrentPer1KValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.CurrentPer1KValueButton.RightArrowVisible = false;
         this.CurrentPer1KValueButton.Size = new System.Drawing.Size(107, 90);
         this.CurrentPer1KValueButton.TabIndex = 175;
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
         // FeederSetupForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Control;
         this.ClientSize = new System.Drawing.Size(621, 554);
         this.Controls.Add(this.MainPanel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "FeederSetupForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "FeederSetupForm";
         this.Shown += new System.EventHandler(this.FeederSetupForm_Shown);
         this.MainPanel.ResumeLayout(false);
         this.BottomRearPanel.ResumeLayout(false);
         this.BottomFrontPanel.ResumeLayout(false);
         this.TopRearPanel.ResumeLayout(false);
         this.TopFrontPanel.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private BorderedPanel MainPanel;
      private System.Windows.Forms.Label TitleLabel;
      private BorderedPanel BottomRearPanel;
      private ValueCycleButton BottomRearStateCycleButton;
      private System.Windows.Forms.Label BottomRearLabel;
      private ValueToggleButton BottomRearDirectionToggleButton;
      private BorderedPanel BottomFrontPanel;
      private ValueCycleButton BottomFrontStateCycleButton;
      private System.Windows.Forms.Label BottomFrontLabel;
      private ValueToggleButton BottomFrontDirectionToggleButton;
      private BorderedPanel TopRearPanel;
      private ValueCycleButton TopRearStateCycleButton;
      private System.Windows.Forms.Label TopRearLabel;
      private ValueToggleButton TopRearDirectionToggleButton;
      private BorderedPanel TopFrontPanel;
      private ValueCycleButton TopFrontStateCycleButton;
      private System.Windows.Forms.Label TopFrontLabel;
      private ValueToggleButton TopFrontDirectionToggleButton;
      private ValueButton LowSpeedScaleValueButton;
      private ValueButton MaxSpeedValueButton;
      private ValueButton LockCurrentValueButton;
      private ValueToggleButton SpeedTrackingToggleButton;
      private ValueButton TrackingCalibrationValueButton;
      private NicBotButton BackButton;
      private ValueButton CurrentPer1KValueButton;
   }
}