namespace NICBOT.GUI
{
   partial class ReelSetupForm
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
         this.CalibrationDistanceValueButton = new NICBOT.GUI.ValueButton();
         this.LockCurrentValueButton = new NICBOT.GUI.ValueButton();
         this.ReverseCurrentValueButton = new NICBOT.GUI.ValueButton();
         this.TitleLabel = new System.Windows.Forms.Label();
         this.MainPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // MainPanel
         // 
         this.MainPanel.BackColor = System.Drawing.Color.Olive;
         this.MainPanel.Controls.Add(this.BackButton);
         this.MainPanel.Controls.Add(this.CalibrationDistanceValueButton);
         this.MainPanel.Controls.Add(this.LockCurrentValueButton);
         this.MainPanel.Controls.Add(this.ReverseCurrentValueButton);
         this.MainPanel.Controls.Add(this.TitleLabel);
         this.MainPanel.EdgeWeight = 3;
         this.MainPanel.Location = new System.Drawing.Point(0, 0);
         this.MainPanel.Name = "MainPanel";
         this.MainPanel.Size = new System.Drawing.Size(385, 257);
         this.MainPanel.TabIndex = 1;
         // 
         // BackButton
         // 
         this.BackButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BackButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BackButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.BackButton.Location = new System.Drawing.Point(139, 173);
         this.BackButton.Name = "BackButton";
         this.BackButton.Size = new System.Drawing.Size(107, 67);
         this.BackButton.TabIndex = 166;
         this.BackButton.Text = "BACK";
         this.BackButton.UseVisualStyleBackColor = false;
         this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
         // 
         // CalibrationDistanceValueButton
         // 
         this.CalibrationDistanceValueButton.ArrowWidth = 0;
         this.CalibrationDistanceValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.CalibrationDistanceValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.CalibrationDistanceValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.CalibrationDistanceValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.CalibrationDistanceValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.CalibrationDistanceValueButton.HoldTimeoutInterval = 0;
         this.CalibrationDistanceValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.CalibrationDistanceValueButton.LeftArrowVisible = false;
         this.CalibrationDistanceValueButton.Location = new System.Drawing.Point(254, 68);
         this.CalibrationDistanceValueButton.Name = "CalibrationDistanceValueButton";
         this.CalibrationDistanceValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.CalibrationDistanceValueButton.RightArrowVisible = false;
         this.CalibrationDistanceValueButton.Size = new System.Drawing.Size(107, 90);
         this.CalibrationDistanceValueButton.TabIndex = 136;
         this.CalibrationDistanceValueButton.Text = "CALIBRATION DISTANCE";
         this.CalibrationDistanceValueButton.UseVisualStyleBackColor = false;
         this.CalibrationDistanceValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.CalibrationDistanceValueButton.ValueEdgeHeight = 8;
         this.CalibrationDistanceValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.CalibrationDistanceValueButton.ValueForeColor = System.Drawing.Color.White;
         this.CalibrationDistanceValueButton.ValueHeight = 22;
         this.CalibrationDistanceValueButton.ValueText = "#### m";
         this.CalibrationDistanceValueButton.ValueWidth = 80;
         this.CalibrationDistanceValueButton.Click += new System.EventHandler(this.CalibrationDistanceValueButton_Click);
         // 
         // LockCurrentValueButton
         // 
         this.LockCurrentValueButton.ArrowWidth = 0;
         this.LockCurrentValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.LockCurrentValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.LockCurrentValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.LockCurrentValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.LockCurrentValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LockCurrentValueButton.HoldTimeoutInterval = 0;
         this.LockCurrentValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.LockCurrentValueButton.LeftArrowVisible = false;
         this.LockCurrentValueButton.Location = new System.Drawing.Point(139, 68);
         this.LockCurrentValueButton.Name = "LockCurrentValueButton";
         this.LockCurrentValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.LockCurrentValueButton.RightArrowVisible = false;
         this.LockCurrentValueButton.Size = new System.Drawing.Size(107, 90);
         this.LockCurrentValueButton.TabIndex = 135;
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
         // ReverseCurrentValueButton
         // 
         this.ReverseCurrentValueButton.ArrowWidth = 0;
         this.ReverseCurrentValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ReverseCurrentValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ReverseCurrentValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ReverseCurrentValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.ReverseCurrentValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ReverseCurrentValueButton.HoldTimeoutInterval = 0;
         this.ReverseCurrentValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.ReverseCurrentValueButton.LeftArrowVisible = false;
         this.ReverseCurrentValueButton.Location = new System.Drawing.Point(24, 68);
         this.ReverseCurrentValueButton.Name = "ReverseCurrentValueButton";
         this.ReverseCurrentValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.ReverseCurrentValueButton.RightArrowVisible = false;
         this.ReverseCurrentValueButton.Size = new System.Drawing.Size(107, 90);
         this.ReverseCurrentValueButton.TabIndex = 134;
         this.ReverseCurrentValueButton.Text = "REVERSE CURRENT";
         this.ReverseCurrentValueButton.UseVisualStyleBackColor = false;
         this.ReverseCurrentValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.ReverseCurrentValueButton.ValueEdgeHeight = 8;
         this.ReverseCurrentValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.ReverseCurrentValueButton.ValueForeColor = System.Drawing.Color.White;
         this.ReverseCurrentValueButton.ValueHeight = 22;
         this.ReverseCurrentValueButton.ValueText = "#.# A";
         this.ReverseCurrentValueButton.ValueWidth = 80;
         this.ReverseCurrentValueButton.Click += new System.EventHandler(this.ReverseCurrentValueButton_Click);
         // 
         // TitleLabel
         // 
         this.TitleLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.TitleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TitleLabel.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
         this.TitleLabel.Location = new System.Drawing.Point(16, 16);
         this.TitleLabel.Name = "TitleLabel";
         this.TitleLabel.Size = new System.Drawing.Size(353, 36);
         this.TitleLabel.TabIndex = 132;
         this.TitleLabel.Text = "TETHER REEL SETUP";
         this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // ReelSetupForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(385, 257);
         this.Controls.Add(this.MainPanel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "ReelSetupForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "ReelSetupForm";
         this.Shown += new System.EventHandler(this.TetherSetupForm_Shown);
         this.MainPanel.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private BorderedPanel MainPanel;
      private System.Windows.Forms.Label TitleLabel;
      private ValueButton LockCurrentValueButton;
      private ValueButton ReverseCurrentValueButton;
      private ValueButton CalibrationDistanceValueButton;
      private NicBotButton BackButton;
   }
}