namespace E4.Ui
{
   partial class LightIntensitySelectForm
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
         this.BackButton = new E4.Ui.Controls.E4Button();
         this.MainPanel = new E4.Ui.Controls.BorderedPanel();
         this.progressBarValueDisplay1 = new E4.Ui.Controls.ProgressBarValueDisplay();
         this.LocationLabel = new System.Windows.Forms.Label();
         this.TitleLabel = new System.Windows.Forms.Label();
         this.LaserLeftButton = new E4.Ui.Controls.LeftRightButton();
         this.LaserRightButton = new E4.Ui.Controls.LeftRightButton();
         this.MainPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // BackButton
         // 
         this.BackButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BackButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BackButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.BackButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.BackButton.Location = new System.Drawing.Point(71, 222);
         this.BackButton.Name = "BackButton";
         this.BackButton.Size = new System.Drawing.Size(107, 67);
         this.BackButton.TabIndex = 6;
         this.BackButton.Text = "BACK";
         this.BackButton.UseVisualStyleBackColor = false;
         // 
         // MainPanel
         // 
         this.MainPanel.BackColor = System.Drawing.Color.Teal;
         this.MainPanel.Controls.Add(this.progressBarValueDisplay1);
         this.MainPanel.Controls.Add(this.LocationLabel);
         this.MainPanel.Controls.Add(this.TitleLabel);
         this.MainPanel.Controls.Add(this.LaserLeftButton);
         this.MainPanel.Controls.Add(this.LaserRightButton);
         this.MainPanel.Controls.Add(this.BackButton);
         this.MainPanel.EdgeWeight = 3;
         this.MainPanel.Location = new System.Drawing.Point(0, 0);
         this.MainPanel.Name = "MainPanel";
         this.MainPanel.Size = new System.Drawing.Size(250, 305);
         this.MainPanel.TabIndex = 7;
         // 
         // progressBarValueDisplay1
         // 
         this.progressBarValueDisplay1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.progressBarValueDisplay1.BarColor = System.Drawing.Color.Yellow;
         this.progressBarValueDisplay1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.progressBarValueDisplay1.Location = new System.Drawing.Point(16, 86);
         this.progressBarValueDisplay1.Maximum = 100;
         this.progressBarValueDisplay1.Minimum = 0;
         this.progressBarValueDisplay1.Name = "progressBarValueDisplay1";
         this.progressBarValueDisplay1.Size = new System.Drawing.Size(218, 35);
         this.progressBarValueDisplay1.TabIndex = 136;
         this.progressBarValueDisplay1.Text = "progressBarValueDisplay1";
         this.progressBarValueDisplay1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.progressBarValueDisplay1.TextColor = System.Drawing.Color.Black;
         this.progressBarValueDisplay1.Value = 50;
         // 
         // LocationLabel
         // 
         this.LocationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LocationLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.LocationLabel.Location = new System.Drawing.Point(16, 52);
         this.LocationLabel.Name = "LocationLabel";
         this.LocationLabel.Size = new System.Drawing.Size(218, 35);
         this.LocationLabel.TabIndex = 135;
         this.LocationLabel.Text = "CAM1";
         this.LocationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // TitleLabel
         // 
         this.TitleLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.TitleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TitleLabel.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
         this.TitleLabel.Location = new System.Drawing.Point(16, 16);
         this.TitleLabel.Name = "TitleLabel";
         this.TitleLabel.Size = new System.Drawing.Size(218, 36);
         this.TitleLabel.TabIndex = 134;
         this.TitleLabel.Text = "LIGHT ADJUST";
         this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // LaserLeftButton
         // 
         this.LaserLeftButton.ArrowColor = System.Drawing.Color.Black;
         this.LaserLeftButton.ArrowHighlightColor = System.Drawing.Color.DarkGray;
         this.LaserLeftButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.LaserLeftButton.DisabledArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.LaserLeftButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.LaserLeftButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.LaserLeftButton.EdgeSpace = 8;
         this.LaserLeftButton.HighLightOffset = 7;
         this.LaserLeftButton.HighlightVisible = true;
         this.LaserLeftButton.HighLightWeight = 2;
         this.LaserLeftButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.LaserLeftButton.HoldRepeat = false;
         this.LaserLeftButton.HoldRepeatInterval = 0;
         this.LaserLeftButton.HoldTimeoutInterval = 0;
         this.LaserLeftButton.LeftRight = true;
         this.LaserLeftButton.Location = new System.Drawing.Point(48, 137);
         this.LaserLeftButton.Name = "LaserLeftButton";
         this.LaserLeftButton.Size = new System.Drawing.Size(69, 69);
         this.LaserLeftButton.TabIndex = 18;
         this.LaserLeftButton.Text = "leftRightButton2";
         this.LaserLeftButton.TextOffset = 0;
         this.LaserLeftButton.TextVisible = false;
         this.LaserLeftButton.UseVisualStyleBackColor = false;
         // 
         // LaserRightButton
         // 
         this.LaserRightButton.ArrowColor = System.Drawing.Color.Black;
         this.LaserRightButton.ArrowHighlightColor = System.Drawing.Color.DarkGray;
         this.LaserRightButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.LaserRightButton.DisabledArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.LaserRightButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.LaserRightButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.LaserRightButton.EdgeSpace = 8;
         this.LaserRightButton.HighLightOffset = 7;
         this.LaserRightButton.HighlightVisible = true;
         this.LaserRightButton.HighLightWeight = 2;
         this.LaserRightButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.LaserRightButton.HoldRepeat = false;
         this.LaserRightButton.HoldRepeatInterval = 0;
         this.LaserRightButton.HoldTimeoutInterval = 0;
         this.LaserRightButton.LeftRight = false;
         this.LaserRightButton.Location = new System.Drawing.Point(133, 137);
         this.LaserRightButton.Name = "LaserRightButton";
         this.LaserRightButton.Size = new System.Drawing.Size(69, 69);
         this.LaserRightButton.TabIndex = 19;
         this.LaserRightButton.Text = "leftRightButton3";
         this.LaserRightButton.TextOffset = 0;
         this.LaserRightButton.TextVisible = false;
         this.LaserRightButton.UseVisualStyleBackColor = false;
         // 
         // LightIntensitySelectForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(250, 305);
         this.Controls.Add(this.MainPanel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "LightIntensitySelectForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "LightIntensitySelectForm";
         this.MainPanel.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private Controls.E4Button BackButton;
      private Controls.BorderedPanel MainPanel;
      private Controls.LeftRightButton LaserLeftButton;
      private Controls.LeftRightButton LaserRightButton;
      private System.Windows.Forms.Label LocationLabel;
      private System.Windows.Forms.Label TitleLabel;
      private Controls.ProgressBarValueDisplay progressBarValueDisplay1;
   }
}