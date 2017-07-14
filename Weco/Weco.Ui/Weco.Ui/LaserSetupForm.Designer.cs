namespace Weco.Ui
{
   partial class LaserSetupForm
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
         this.MainPanel = new Controls.BorderedPanel();
         this.LaserSampeCountValueButton = new Controls.ValueButton();
         this.ManualFrequencyValueButton = new Controls.ValueButton();
         this.TitleLabel = new System.Windows.Forms.Label();
         this.BackButton = new Controls.BaseButton();
         this.LaserMeasureFrequencyButton = new Controls.ValueToggleButton();
         this.MainPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // MainPanel
         // 
         this.MainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(80)))), ((int)(((byte)(96)))));
         this.MainPanel.Controls.Add(this.LaserMeasureFrequencyButton);
         this.MainPanel.Controls.Add(this.LaserSampeCountValueButton);
         this.MainPanel.Controls.Add(this.ManualFrequencyValueButton);
         this.MainPanel.Controls.Add(this.TitleLabel);
         this.MainPanel.Controls.Add(this.BackButton);
         this.MainPanel.EdgeWeight = 3;
         this.MainPanel.Location = new System.Drawing.Point(0, 0);
         this.MainPanel.Name = "MainPanel";
         this.MainPanel.Size = new System.Drawing.Size(391, 257);
         this.MainPanel.TabIndex = 0;
         // 
         // LaserSampeCountValueButton
         // 
         this.LaserSampeCountValueButton.ArrowWidth = 0;
         this.LaserSampeCountValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.LaserSampeCountValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.LaserSampeCountValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.LaserSampeCountValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.LaserSampeCountValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.LaserSampeCountValueButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.LaserSampeCountValueButton.HoldTimeoutInterval = 0;
         this.LaserSampeCountValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.LaserSampeCountValueButton.LeftArrowVisible = false;
         this.LaserSampeCountValueButton.Location = new System.Drawing.Point(257, 68);
         this.LaserSampeCountValueButton.Name = "LaserSampeCountValueButton";
         this.LaserSampeCountValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.LaserSampeCountValueButton.RightArrowVisible = false;
         this.LaserSampeCountValueButton.Size = new System.Drawing.Size(107, 90);
         this.LaserSampeCountValueButton.TabIndex = 203;
         this.LaserSampeCountValueButton.Text = "SAMPLE COUNT";
         this.LaserSampeCountValueButton.UseVisualStyleBackColor = false;
         this.LaserSampeCountValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.LaserSampeCountValueButton.ValueEdgeHeight = 8;
         this.LaserSampeCountValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
         this.LaserSampeCountValueButton.ValueForeColor = System.Drawing.Color.White;
         this.LaserSampeCountValueButton.ValueHeight = 22;
         this.LaserSampeCountValueButton.ValueText = "10";
         this.LaserSampeCountValueButton.ValueWidth = 80;
         this.LaserSampeCountValueButton.Click += new System.EventHandler(this.LaserSampeCountValueButton_Click);
         // 
         // ManualFrequencyValueButton
         // 
         this.ManualFrequencyValueButton.ArrowWidth = 0;
         this.ManualFrequencyValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ManualFrequencyValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ManualFrequencyValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ManualFrequencyValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.ManualFrequencyValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.ManualFrequencyValueButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.ManualFrequencyValueButton.HoldTimeoutInterval = 0;
         this.ManualFrequencyValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.ManualFrequencyValueButton.LeftArrowVisible = false;
         this.ManualFrequencyValueButton.Location = new System.Drawing.Point(142, 68);
         this.ManualFrequencyValueButton.Name = "ManualFrequencyValueButton";
         this.ManualFrequencyValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.ManualFrequencyValueButton.RightArrowVisible = false;
         this.ManualFrequencyValueButton.Size = new System.Drawing.Size(107, 90);
         this.ManualFrequencyValueButton.TabIndex = 202;
         this.ManualFrequencyValueButton.Text = "MANUAL FREQ";
         this.ManualFrequencyValueButton.UseVisualStyleBackColor = false;
         this.ManualFrequencyValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.ManualFrequencyValueButton.ValueEdgeHeight = 8;
         this.ManualFrequencyValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
         this.ManualFrequencyValueButton.ValueForeColor = System.Drawing.Color.White;
         this.ManualFrequencyValueButton.ValueHeight = 22;
         this.ManualFrequencyValueButton.ValueText = "100.0 Hz";
         this.ManualFrequencyValueButton.ValueWidth = 80;
         this.ManualFrequencyValueButton.Click += new System.EventHandler(this.ManualFrequencyValueButton_Click);
         // 
         // TitleLabel
         // 
         this.TitleLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.TitleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TitleLabel.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
         this.TitleLabel.Location = new System.Drawing.Point(16, 16);
         this.TitleLabel.Name = "TitleLabel";
         this.TitleLabel.Size = new System.Drawing.Size(359, 36);
         this.TitleLabel.TabIndex = 133;
         this.TitleLabel.Text = "LASER SETUP";
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
         this.BackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.BackButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.BackButton.Location = new System.Drawing.Point(142, 174);
         this.BackButton.Name = "BackButton";
         this.BackButton.Size = new System.Drawing.Size(107, 67);
         this.BackButton.TabIndex = 5;
         this.BackButton.Text = "BACK";
         this.BackButton.UseVisualStyleBackColor = false;
         this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
         // 
         // LaserMeasureFrequencyButton
         // 
         this.LaserMeasureFrequencyButton.AutomaticToggle = true;
         this.LaserMeasureFrequencyButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.LaserMeasureFrequencyButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.LaserMeasureFrequencyButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.LaserMeasureFrequencyButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.LaserMeasureFrequencyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.LaserMeasureFrequencyButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.LaserMeasureFrequencyButton.HoldEnable = false;
         this.LaserMeasureFrequencyButton.HoldTimeoutInterval = 0;
         this.LaserMeasureFrequencyButton.Location = new System.Drawing.Point(27, 68);
         this.LaserMeasureFrequencyButton.Name = "LaserMeasureFrequencyButton";
         this.LaserMeasureFrequencyButton.OptionASelected = true;
         this.LaserMeasureFrequencyButton.OptionAText = "AUTO";
         this.LaserMeasureFrequencyButton.OptionBSelected = false;
         this.LaserMeasureFrequencyButton.OptionBText = "MANUAL";
         this.LaserMeasureFrequencyButton.OptionCenterWidth = 2;
         this.LaserMeasureFrequencyButton.OptionEdgeHeight = 8;
         this.LaserMeasureFrequencyButton.OptionHeight = 22;
         this.LaserMeasureFrequencyButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.LaserMeasureFrequencyButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LaserMeasureFrequencyButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.LaserMeasureFrequencyButton.OptionSelectedBackColor = System.Drawing.Color.Lime;
         this.LaserMeasureFrequencyButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
         this.LaserMeasureFrequencyButton.OptionSelectedForeColor = System.Drawing.Color.Black;
         this.LaserMeasureFrequencyButton.OptionWidth = 50;
         this.LaserMeasureFrequencyButton.Size = new System.Drawing.Size(107, 90);
         this.LaserMeasureFrequencyButton.TabIndex = 205;
         this.LaserMeasureFrequencyButton.Text = "MEASURE FREQ";
         this.LaserMeasureFrequencyButton.UseVisualStyleBackColor = false;
         this.LaserMeasureFrequencyButton.Click += new System.EventHandler(this.LaserMeasureFrequencyButton_Click);
         // 
         // LaserSetupForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(391, 257);
         this.Controls.Add(this.MainPanel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "LaserSetupForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "LaserSetupForm";
         this.Shown += new System.EventHandler(this.LaserSetupForm_Shown);
         this.MainPanel.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private Controls.BorderedPanel MainPanel;
      private Controls.BaseButton BackButton;
      private System.Windows.Forms.Label TitleLabel;
      private Controls.ValueButton LaserSampeCountValueButton;
      private Controls.ValueButton ManualFrequencyValueButton;
      private Controls.ValueToggleButton LaserMeasureFrequencyButton;
   }
}