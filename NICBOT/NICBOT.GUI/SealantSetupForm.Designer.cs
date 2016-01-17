namespace NICBOT.GUI
{
   partial class SealantSetupForm
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
         this.FlowConstantValueButton = new NICBOT.GUI.ValueButton();
         this.SealantWeightValueButton = new NICBOT.GUI.ValueButton();
         this.MaximumSpeedValueButton = new NICBOT.GUI.ValueButton();
         this.ReverseSpeedValueButton = new NICBOT.GUI.ValueButton();
         this.ForwardSpeedValueButton = new NICBOT.GUI.ValueButton();
         this.RelievedPressureValueButton = new NICBOT.GUI.ValueButton();
         this.MaximumPressureValueButton = new NICBOT.GUI.ValueButton();
         this.AutoFillPressureValueButton = new NICBOT.GUI.ValueButton();
         this.MaximumVolumeValueButton = new NICBOT.GUI.ValueButton();
         this.AutoFillVolumeValueButton = new NICBOT.GUI.ValueButton();
         this.PressureReliefToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.NozzleInsertionToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.AutoFillModeToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.NozzleSelectToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.TitleLabel = new System.Windows.Forms.Label();
         this.MainPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // MainPanel
         // 
         this.MainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.MainPanel.Controls.Add(this.BackButton);
         this.MainPanel.Controls.Add(this.FlowConstantValueButton);
         this.MainPanel.Controls.Add(this.SealantWeightValueButton);
         this.MainPanel.Controls.Add(this.MaximumSpeedValueButton);
         this.MainPanel.Controls.Add(this.ReverseSpeedValueButton);
         this.MainPanel.Controls.Add(this.ForwardSpeedValueButton);
         this.MainPanel.Controls.Add(this.RelievedPressureValueButton);
         this.MainPanel.Controls.Add(this.MaximumPressureValueButton);
         this.MainPanel.Controls.Add(this.AutoFillPressureValueButton);
         this.MainPanel.Controls.Add(this.MaximumVolumeValueButton);
         this.MainPanel.Controls.Add(this.AutoFillVolumeValueButton);
         this.MainPanel.Controls.Add(this.PressureReliefToggleButton);
         this.MainPanel.Controls.Add(this.NozzleInsertionToggleButton);
         this.MainPanel.Controls.Add(this.AutoFillModeToggleButton);
         this.MainPanel.Controls.Add(this.NozzleSelectToggleButton);
         this.MainPanel.Controls.Add(this.TitleLabel);
         this.MainPanel.EdgeWeight = 3;
         this.MainPanel.Location = new System.Drawing.Point(0, 0);
         this.MainPanel.Name = "MainPanel";
         this.MainPanel.Size = new System.Drawing.Size(506, 468);
         this.MainPanel.TabIndex = 0;
         // 
         // BackButton
         // 
         this.BackButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BackButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BackButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.BackButton.Location = new System.Drawing.Point(369, 385);
         this.BackButton.Name = "BackButton";
         this.BackButton.Size = new System.Drawing.Size(107, 67);
         this.BackButton.TabIndex = 165;
         this.BackButton.Text = "BACK";
         this.BackButton.UseVisualStyleBackColor = false;
         this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
         // 
         // FlowConstantValueButton
         // 
         this.FlowConstantValueButton.ArrowWidth = 0;
         this.FlowConstantValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.FlowConstantValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.FlowConstantValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.FlowConstantValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.FlowConstantValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
         this.FlowConstantValueButton.HoldTimeoutInterval = 0;
         this.FlowConstantValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.FlowConstantValueButton.LeftArrowVisible = false;
         this.FlowConstantValueButton.Location = new System.Drawing.Point(142, 362);
         this.FlowConstantValueButton.Name = "FlowConstantValueButton";
         this.FlowConstantValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.FlowConstantValueButton.RightArrowVisible = false;
         this.FlowConstantValueButton.Size = new System.Drawing.Size(107, 90);
         this.FlowConstantValueButton.TabIndex = 161;
         this.FlowConstantValueButton.Text = "FLOW CONSTANT";
         this.FlowConstantValueButton.UseVisualStyleBackColor = false;
         this.FlowConstantValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.FlowConstantValueButton.ValueEdgeHeight = 8;
         this.FlowConstantValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.FlowConstantValueButton.ValueForeColor = System.Drawing.Color.White;
         this.FlowConstantValueButton.ValueHeight = 22;
         this.FlowConstantValueButton.ValueText = "### REV/L";
         this.FlowConstantValueButton.ValueWidth = 92;
         this.FlowConstantValueButton.Click += new System.EventHandler(this.FlowConstantValueButton_Click);
         // 
         // SealantWeightValueButton
         // 
         this.SealantWeightValueButton.ArrowWidth = 0;
         this.SealantWeightValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SealantWeightValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.SealantWeightValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.SealantWeightValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.SealantWeightValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
         this.SealantWeightValueButton.HoldTimeoutInterval = 0;
         this.SealantWeightValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.SealantWeightValueButton.LeftArrowVisible = false;
         this.SealantWeightValueButton.Location = new System.Drawing.Point(27, 362);
         this.SealantWeightValueButton.Name = "SealantWeightValueButton";
         this.SealantWeightValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.SealantWeightValueButton.RightArrowVisible = false;
         this.SealantWeightValueButton.Size = new System.Drawing.Size(107, 90);
         this.SealantWeightValueButton.TabIndex = 160;
         this.SealantWeightValueButton.Text = "SEALANT WEIGHT";
         this.SealantWeightValueButton.UseVisualStyleBackColor = false;
         this.SealantWeightValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.SealantWeightValueButton.ValueEdgeHeight = 8;
         this.SealantWeightValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.SealantWeightValueButton.ValueForeColor = System.Drawing.Color.White;
         this.SealantWeightValueButton.ValueHeight = 22;
         this.SealantWeightValueButton.ValueText = "### g/mL";
         this.SealantWeightValueButton.ValueWidth = 92;
         this.SealantWeightValueButton.Click += new System.EventHandler(this.SealantWeightValueButton_Click);
         // 
         // MaximumSpeedValueButton
         // 
         this.MaximumSpeedValueButton.ArrowWidth = 0;
         this.MaximumSpeedValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.MaximumSpeedValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.MaximumSpeedValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.MaximumSpeedValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.MaximumSpeedValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
         this.MaximumSpeedValueButton.HoldTimeoutInterval = 0;
         this.MaximumSpeedValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.MaximumSpeedValueButton.LeftArrowVisible = false;
         this.MaximumSpeedValueButton.Location = new System.Drawing.Point(372, 264);
         this.MaximumSpeedValueButton.Name = "MaximumSpeedValueButton";
         this.MaximumSpeedValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.MaximumSpeedValueButton.RightArrowVisible = false;
         this.MaximumSpeedValueButton.Size = new System.Drawing.Size(107, 90);
         this.MaximumSpeedValueButton.TabIndex = 159;
         this.MaximumSpeedValueButton.Text = "MAX      SPEED";
         this.MaximumSpeedValueButton.UseVisualStyleBackColor = false;
         this.MaximumSpeedValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.MaximumSpeedValueButton.ValueEdgeHeight = 8;
         this.MaximumSpeedValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.MaximumSpeedValueButton.ValueForeColor = System.Drawing.Color.White;
         this.MaximumSpeedValueButton.ValueHeight = 22;
         this.MaximumSpeedValueButton.ValueText = "### RPM";
         this.MaximumSpeedValueButton.ValueWidth = 92;
         this.MaximumSpeedValueButton.Click += new System.EventHandler(this.MaximumSpeedValueButton_Click);
         // 
         // ReverseSpeedValueButton
         // 
         this.ReverseSpeedValueButton.ArrowWidth = 0;
         this.ReverseSpeedValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ReverseSpeedValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ReverseSpeedValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ReverseSpeedValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.ReverseSpeedValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
         this.ReverseSpeedValueButton.HoldTimeoutInterval = 0;
         this.ReverseSpeedValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.ReverseSpeedValueButton.LeftArrowVisible = false;
         this.ReverseSpeedValueButton.Location = new System.Drawing.Point(257, 264);
         this.ReverseSpeedValueButton.Name = "ReverseSpeedValueButton";
         this.ReverseSpeedValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.ReverseSpeedValueButton.RightArrowVisible = false;
         this.ReverseSpeedValueButton.Size = new System.Drawing.Size(107, 90);
         this.ReverseSpeedValueButton.TabIndex = 158;
         this.ReverseSpeedValueButton.Text = "REVERSE SPEED";
         this.ReverseSpeedValueButton.UseVisualStyleBackColor = false;
         this.ReverseSpeedValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.ReverseSpeedValueButton.ValueEdgeHeight = 8;
         this.ReverseSpeedValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.ReverseSpeedValueButton.ValueForeColor = System.Drawing.Color.White;
         this.ReverseSpeedValueButton.ValueHeight = 22;
         this.ReverseSpeedValueButton.ValueText = "### RPM";
         this.ReverseSpeedValueButton.ValueWidth = 92;
         this.ReverseSpeedValueButton.Click += new System.EventHandler(this.ReverseSpeedValueButton_Click);
         // 
         // ForwardSpeedValueButton
         // 
         this.ForwardSpeedValueButton.ArrowWidth = 0;
         this.ForwardSpeedValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ForwardSpeedValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ForwardSpeedValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ForwardSpeedValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.ForwardSpeedValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
         this.ForwardSpeedValueButton.HoldTimeoutInterval = 0;
         this.ForwardSpeedValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.ForwardSpeedValueButton.LeftArrowVisible = false;
         this.ForwardSpeedValueButton.Location = new System.Drawing.Point(142, 264);
         this.ForwardSpeedValueButton.Name = "ForwardSpeedValueButton";
         this.ForwardSpeedValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.ForwardSpeedValueButton.RightArrowVisible = false;
         this.ForwardSpeedValueButton.Size = new System.Drawing.Size(107, 90);
         this.ForwardSpeedValueButton.TabIndex = 157;
         this.ForwardSpeedValueButton.Text = "FORWARD SPEED";
         this.ForwardSpeedValueButton.UseVisualStyleBackColor = false;
         this.ForwardSpeedValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.ForwardSpeedValueButton.ValueEdgeHeight = 8;
         this.ForwardSpeedValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.ForwardSpeedValueButton.ValueForeColor = System.Drawing.Color.White;
         this.ForwardSpeedValueButton.ValueHeight = 22;
         this.ForwardSpeedValueButton.ValueText = "### RPM";
         this.ForwardSpeedValueButton.ValueWidth = 92;
         this.ForwardSpeedValueButton.Click += new System.EventHandler(this.ForwardSpeedValueButton_Click);
         // 
         // RelievedPressureValueButton
         // 
         this.RelievedPressureValueButton.ArrowWidth = 0;
         this.RelievedPressureValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RelievedPressureValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.RelievedPressureValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.RelievedPressureValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.RelievedPressureValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
         this.RelievedPressureValueButton.HoldTimeoutInterval = 0;
         this.RelievedPressureValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.RelievedPressureValueButton.LeftArrowVisible = false;
         this.RelievedPressureValueButton.Location = new System.Drawing.Point(27, 264);
         this.RelievedPressureValueButton.Name = "RelievedPressureValueButton";
         this.RelievedPressureValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.RelievedPressureValueButton.RightArrowVisible = false;
         this.RelievedPressureValueButton.Size = new System.Drawing.Size(107, 90);
         this.RelievedPressureValueButton.TabIndex = 156;
         this.RelievedPressureValueButton.Text = "RELIEVED PRESSURE";
         this.RelievedPressureValueButton.UseVisualStyleBackColor = false;
         this.RelievedPressureValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.RelievedPressureValueButton.ValueEdgeHeight = 8;
         this.RelievedPressureValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.RelievedPressureValueButton.ValueForeColor = System.Drawing.Color.White;
         this.RelievedPressureValueButton.ValueHeight = 22;
         this.RelievedPressureValueButton.ValueText = "### PSI";
         this.RelievedPressureValueButton.ValueWidth = 92;
         this.RelievedPressureValueButton.Click += new System.EventHandler(this.RelievedPressureValueButton_Click);
         // 
         // MaximumPressureValueButton
         // 
         this.MaximumPressureValueButton.ArrowWidth = 0;
         this.MaximumPressureValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.MaximumPressureValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.MaximumPressureValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.MaximumPressureValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.MaximumPressureValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
         this.MaximumPressureValueButton.HoldTimeoutInterval = 0;
         this.MaximumPressureValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.MaximumPressureValueButton.LeftArrowVisible = false;
         this.MaximumPressureValueButton.Location = new System.Drawing.Point(372, 166);
         this.MaximumPressureValueButton.Name = "MaximumPressureValueButton";
         this.MaximumPressureValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.MaximumPressureValueButton.RightArrowVisible = false;
         this.MaximumPressureValueButton.Size = new System.Drawing.Size(107, 90);
         this.MaximumPressureValueButton.TabIndex = 155;
         this.MaximumPressureValueButton.Text = "MAX PRESSURE";
         this.MaximumPressureValueButton.UseVisualStyleBackColor = false;
         this.MaximumPressureValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.MaximumPressureValueButton.ValueEdgeHeight = 8;
         this.MaximumPressureValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.MaximumPressureValueButton.ValueForeColor = System.Drawing.Color.White;
         this.MaximumPressureValueButton.ValueHeight = 22;
         this.MaximumPressureValueButton.ValueText = "### PSI";
         this.MaximumPressureValueButton.ValueWidth = 92;
         this.MaximumPressureValueButton.Click += new System.EventHandler(this.MaximumPressureValueButton_Click);
         // 
         // AutoFillPressureValueButton
         // 
         this.AutoFillPressureValueButton.ArrowWidth = 0;
         this.AutoFillPressureValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.AutoFillPressureValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.AutoFillPressureValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.AutoFillPressureValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.AutoFillPressureValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
         this.AutoFillPressureValueButton.HoldTimeoutInterval = 0;
         this.AutoFillPressureValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.AutoFillPressureValueButton.LeftArrowVisible = false;
         this.AutoFillPressureValueButton.Location = new System.Drawing.Point(257, 166);
         this.AutoFillPressureValueButton.Name = "AutoFillPressureValueButton";
         this.AutoFillPressureValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.AutoFillPressureValueButton.RightArrowVisible = false;
         this.AutoFillPressureValueButton.Size = new System.Drawing.Size(107, 90);
         this.AutoFillPressureValueButton.TabIndex = 154;
         this.AutoFillPressureValueButton.Text = "AUTO FILL PRESSURE";
         this.AutoFillPressureValueButton.UseVisualStyleBackColor = false;
         this.AutoFillPressureValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.AutoFillPressureValueButton.ValueEdgeHeight = 8;
         this.AutoFillPressureValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.AutoFillPressureValueButton.ValueForeColor = System.Drawing.Color.White;
         this.AutoFillPressureValueButton.ValueHeight = 22;
         this.AutoFillPressureValueButton.ValueText = "### PSI";
         this.AutoFillPressureValueButton.ValueWidth = 92;
         this.AutoFillPressureValueButton.Click += new System.EventHandler(this.AutoFillPressureValueButton_Click);
         // 
         // MaximumVolumeValueButton
         // 
         this.MaximumVolumeValueButton.ArrowWidth = 0;
         this.MaximumVolumeValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.MaximumVolumeValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.MaximumVolumeValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.MaximumVolumeValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.MaximumVolumeValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
         this.MaximumVolumeValueButton.HoldTimeoutInterval = 0;
         this.MaximumVolumeValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.MaximumVolumeValueButton.LeftArrowVisible = false;
         this.MaximumVolumeValueButton.Location = new System.Drawing.Point(142, 166);
         this.MaximumVolumeValueButton.Name = "MaximumVolumeValueButton";
         this.MaximumVolumeValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.MaximumVolumeValueButton.RightArrowVisible = false;
         this.MaximumVolumeValueButton.Size = new System.Drawing.Size(107, 90);
         this.MaximumVolumeValueButton.TabIndex = 153;
         this.MaximumVolumeValueButton.Text = "MAX   VOLUME";
         this.MaximumVolumeValueButton.UseVisualStyleBackColor = false;
         this.MaximumVolumeValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.MaximumVolumeValueButton.ValueEdgeHeight = 8;
         this.MaximumVolumeValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.MaximumVolumeValueButton.ValueForeColor = System.Drawing.Color.White;
         this.MaximumVolumeValueButton.ValueHeight = 22;
         this.MaximumVolumeValueButton.ValueText = "### mL";
         this.MaximumVolumeValueButton.ValueWidth = 92;
         this.MaximumVolumeValueButton.Click += new System.EventHandler(this.MaximumVolumeValueButton_Click);
         // 
         // AutoFillVolumeValueButton
         // 
         this.AutoFillVolumeValueButton.ArrowWidth = 0;
         this.AutoFillVolumeValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.AutoFillVolumeValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.AutoFillVolumeValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.AutoFillVolumeValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.AutoFillVolumeValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
         this.AutoFillVolumeValueButton.HoldTimeoutInterval = 0;
         this.AutoFillVolumeValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.AutoFillVolumeValueButton.LeftArrowVisible = false;
         this.AutoFillVolumeValueButton.Location = new System.Drawing.Point(27, 166);
         this.AutoFillVolumeValueButton.Name = "AutoFillVolumeValueButton";
         this.AutoFillVolumeValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.AutoFillVolumeValueButton.RightArrowVisible = false;
         this.AutoFillVolumeValueButton.Size = new System.Drawing.Size(107, 90);
         this.AutoFillVolumeValueButton.TabIndex = 152;
         this.AutoFillVolumeValueButton.Text = "AUTO FILL VOLUME";
         this.AutoFillVolumeValueButton.UseVisualStyleBackColor = false;
         this.AutoFillVolumeValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.AutoFillVolumeValueButton.ValueEdgeHeight = 8;
         this.AutoFillVolumeValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.AutoFillVolumeValueButton.ValueForeColor = System.Drawing.Color.White;
         this.AutoFillVolumeValueButton.ValueHeight = 22;
         this.AutoFillVolumeValueButton.ValueText = "#### mL";
         this.AutoFillVolumeValueButton.ValueWidth = 92;
         this.AutoFillVolumeValueButton.Click += new System.EventHandler(this.AutoFillVolumeValueButton_Click);
         // 
         // PressureReliefToggleButton
         // 
         this.PressureReliefToggleButton.AutomaticToggle = true;
         this.PressureReliefToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.PressureReliefToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.PressureReliefToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.PressureReliefToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.PressureReliefToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
         this.PressureReliefToggleButton.HoldEnable = false;
         this.PressureReliefToggleButton.HoldTimeoutInterval = 0;
         this.PressureReliefToggleButton.Location = new System.Drawing.Point(372, 68);
         this.PressureReliefToggleButton.Name = "PressureReliefToggleButton";
         this.PressureReliefToggleButton.OptionASelected = false;
         this.PressureReliefToggleButton.OptionAText = "AUTO";
         this.PressureReliefToggleButton.OptionBSelected = true;
         this.PressureReliefToggleButton.OptionBText = "MANUAL";
         this.PressureReliefToggleButton.OptionCenterWidth = 2;
         this.PressureReliefToggleButton.OptionEdgeHeight = 8;
         this.PressureReliefToggleButton.OptionHeight = 22;
         this.PressureReliefToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.PressureReliefToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.PressureReliefToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.PressureReliefToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.PressureReliefToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.PressureReliefToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.PressureReliefToggleButton.OptionWidth = 50;
         this.PressureReliefToggleButton.Size = new System.Drawing.Size(107, 90);
         this.PressureReliefToggleButton.TabIndex = 151;
         this.PressureReliefToggleButton.Text = "PRESSURE RELIEF";
         this.PressureReliefToggleButton.UseVisualStyleBackColor = false;
         this.PressureReliefToggleButton.Click += new System.EventHandler(this.PressureReliefToggleButton_Click);
         // 
         // NozzleInsertionToggleButton
         // 
         this.NozzleInsertionToggleButton.AutomaticToggle = true;
         this.NozzleInsertionToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.NozzleInsertionToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.NozzleInsertionToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.NozzleInsertionToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.NozzleInsertionToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.NozzleInsertionToggleButton.HoldEnable = false;
         this.NozzleInsertionToggleButton.HoldTimeoutInterval = 0;
         this.NozzleInsertionToggleButton.Location = new System.Drawing.Point(257, 68);
         this.NozzleInsertionToggleButton.Name = "NozzleInsertionToggleButton";
         this.NozzleInsertionToggleButton.OptionASelected = false;
         this.NozzleInsertionToggleButton.OptionAText = "AUTO";
         this.NozzleInsertionToggleButton.OptionBSelected = true;
         this.NozzleInsertionToggleButton.OptionBText = "MANUAL";
         this.NozzleInsertionToggleButton.OptionCenterWidth = 2;
         this.NozzleInsertionToggleButton.OptionEdgeHeight = 8;
         this.NozzleInsertionToggleButton.OptionHeight = 22;
         this.NozzleInsertionToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.NozzleInsertionToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.NozzleInsertionToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.NozzleInsertionToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.NozzleInsertionToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.NozzleInsertionToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.NozzleInsertionToggleButton.OptionWidth = 50;
         this.NozzleInsertionToggleButton.Size = new System.Drawing.Size(107, 90);
         this.NozzleInsertionToggleButton.TabIndex = 150;
         this.NozzleInsertionToggleButton.Text = "NOZZLE INSERTION";
         this.NozzleInsertionToggleButton.UseVisualStyleBackColor = false;
         this.NozzleInsertionToggleButton.Click += new System.EventHandler(this.NozzleInsertionToggleButton_Click);
         // 
         // AutoFillModeToggleButton
         // 
         this.AutoFillModeToggleButton.AutomaticToggle = true;
         this.AutoFillModeToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.AutoFillModeToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.AutoFillModeToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.AutoFillModeToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.AutoFillModeToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
         this.AutoFillModeToggleButton.ForeColor = System.Drawing.Color.Black;
         this.AutoFillModeToggleButton.HoldEnable = false;
         this.AutoFillModeToggleButton.HoldTimeoutInterval = 0;
         this.AutoFillModeToggleButton.Location = new System.Drawing.Point(142, 68);
         this.AutoFillModeToggleButton.Name = "AutoFillModeToggleButton";
         this.AutoFillModeToggleButton.OptionASelected = true;
         this.AutoFillModeToggleButton.OptionAText = "PRESSURE";
         this.AutoFillModeToggleButton.OptionBSelected = false;
         this.AutoFillModeToggleButton.OptionBText = "VOLUME";
         this.AutoFillModeToggleButton.OptionCenterWidth = 0;
         this.AutoFillModeToggleButton.OptionEdgeHeight = 8;
         this.AutoFillModeToggleButton.OptionHeight = 22;
         this.AutoFillModeToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.AutoFillModeToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.AutoFillModeToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.AutoFillModeToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.AutoFillModeToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.AutoFillModeToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.AutoFillModeToggleButton.OptionWidth = 52;
         this.AutoFillModeToggleButton.Size = new System.Drawing.Size(107, 90);
         this.AutoFillModeToggleButton.TabIndex = 148;
         this.AutoFillModeToggleButton.Text = "AUTO FILL MODE";
         this.AutoFillModeToggleButton.UseVisualStyleBackColor = false;
         this.AutoFillModeToggleButton.Click += new System.EventHandler(this.AutoFillModeToggleButton_Click);
         // 
         // NozzleSelectToggleButton
         // 
         this.NozzleSelectToggleButton.AutomaticToggle = true;
         this.NozzleSelectToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.NozzleSelectToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.NozzleSelectToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.NozzleSelectToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.NozzleSelectToggleButton.Enabled = false;
         this.NozzleSelectToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
         this.NozzleSelectToggleButton.HoldEnable = false;
         this.NozzleSelectToggleButton.HoldTimeoutInterval = 0;
         this.NozzleSelectToggleButton.Location = new System.Drawing.Point(27, 68);
         this.NozzleSelectToggleButton.Name = "NozzleSelectToggleButton";
         this.NozzleSelectToggleButton.OptionASelected = true;
         this.NozzleSelectToggleButton.OptionAText = "FRONT";
         this.NozzleSelectToggleButton.OptionBSelected = false;
         this.NozzleSelectToggleButton.OptionBText = "REAR";
         this.NozzleSelectToggleButton.OptionCenterWidth = 2;
         this.NozzleSelectToggleButton.OptionEdgeHeight = 8;
         this.NozzleSelectToggleButton.OptionHeight = 22;
         this.NozzleSelectToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.NozzleSelectToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.NozzleSelectToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.NozzleSelectToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.NozzleSelectToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.NozzleSelectToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.NozzleSelectToggleButton.OptionWidth = 45;
         this.NozzleSelectToggleButton.Size = new System.Drawing.Size(107, 90);
         this.NozzleSelectToggleButton.TabIndex = 135;
         this.NozzleSelectToggleButton.Text = "NOZZLE SELECT";
         this.NozzleSelectToggleButton.UseVisualStyleBackColor = false;
         this.NozzleSelectToggleButton.Click += new System.EventHandler(this.NozzleSelectToggleButton_Click);
         // 
         // TitleLabel
         // 
         this.TitleLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.TitleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TitleLabel.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
         this.TitleLabel.Location = new System.Drawing.Point(16, 16);
         this.TitleLabel.Name = "TitleLabel";
         this.TitleLabel.Size = new System.Drawing.Size(474, 36);
         this.TitleLabel.TabIndex = 134;
         this.TitleLabel.Text = "SEALANT PUMP SETUP";
         this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // SealantSetupForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(506, 468);
         this.Controls.Add(this.MainPanel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "SealantSetupForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "SealantSetupForm";
         this.Shown += new System.EventHandler(this.SealantSetupForm_Shown);
         this.MainPanel.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private BorderedPanel MainPanel;
      private System.Windows.Forms.Label TitleLabel;
      private ValueToggleButton NozzleSelectToggleButton;
      private ValueToggleButton PressureReliefToggleButton;
      private ValueToggleButton NozzleInsertionToggleButton;
      private ValueToggleButton AutoFillModeToggleButton;
      private ValueButton FlowConstantValueButton;
      private ValueButton SealantWeightValueButton;
      private ValueButton MaximumSpeedValueButton;
      private ValueButton ReverseSpeedValueButton;
      private ValueButton ForwardSpeedValueButton;
      private ValueButton RelievedPressureValueButton;
      private ValueButton MaximumPressureValueButton;
      private ValueButton AutoFillPressureValueButton;
      private ValueButton MaximumVolumeValueButton;
      private ValueButton AutoFillVolumeValueButton;
      private NicBotButton BackButton;
   }
}