namespace E4.Ui
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
         this.MainPanel = new E4.Ui.Controls.BorderedPanel();
         this.BackButton = new E4.Ui.Controls.E4Button();
         this.TitleLabel = new System.Windows.Forms.Label();
         this.LaserSampleTimeValueButton = new E4.Ui.Controls.ValueButton();
         this.LaserSampeCountValueButton = new E4.Ui.Controls.ValueButton();
         this.MainPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // MainPanel
         // 
         this.MainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(80)))), ((int)(((byte)(96)))));
         this.MainPanel.Controls.Add(this.LaserSampeCountValueButton);
         this.MainPanel.Controls.Add(this.LaserSampleTimeValueButton);
         this.MainPanel.Controls.Add(this.TitleLabel);
         this.MainPanel.Controls.Add(this.BackButton);
         this.MainPanel.EdgeWeight = 3;
         this.MainPanel.Location = new System.Drawing.Point(0, 0);
         this.MainPanel.Name = "MainPanel";
         this.MainPanel.Size = new System.Drawing.Size(385, 257);
         this.MainPanel.TabIndex = 0;
         // 
         // BackButton
         // 
         this.BackButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BackButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BackButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.BackButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.BackButton.Location = new System.Drawing.Point(136, 174);
         this.BackButton.Name = "BackButton";
         this.BackButton.Size = new System.Drawing.Size(107, 67);
         this.BackButton.TabIndex = 5;
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
         this.TitleLabel.Size = new System.Drawing.Size(353, 36);
         this.TitleLabel.TabIndex = 133;
         this.TitleLabel.Text = "LASER SETUP";
         this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.TitleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitleLabel_MouseDown);
         this.TitleLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TitleLabel_MouseMove);
         this.TitleLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TitleLabel_MouseUp);
         // 
         // LaserSampleTimeValueButton
         // 
         this.LaserSampleTimeValueButton.ArrowWidth = 0;
         this.LaserSampleTimeValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.LaserSampleTimeValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.LaserSampleTimeValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.LaserSampleTimeValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.LaserSampleTimeValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.LaserSampleTimeValueButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.LaserSampleTimeValueButton.HoldTimeoutInterval = 0;
         this.LaserSampleTimeValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.LaserSampleTimeValueButton.LeftArrowVisible = false;
         this.LaserSampleTimeValueButton.Location = new System.Drawing.Point(55, 68);
         this.LaserSampleTimeValueButton.Name = "LaserSampleTimeValueButton";
         this.LaserSampleTimeValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.LaserSampleTimeValueButton.RightArrowVisible = false;
         this.LaserSampleTimeValueButton.Size = new System.Drawing.Size(107, 90);
         this.LaserSampleTimeValueButton.TabIndex = 202;
         this.LaserSampleTimeValueButton.Text = "SAMPLE TIME";
         this.LaserSampleTimeValueButton.UseVisualStyleBackColor = false;
         this.LaserSampleTimeValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.LaserSampleTimeValueButton.ValueEdgeHeight = 8;
         this.LaserSampleTimeValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
         this.LaserSampleTimeValueButton.ValueForeColor = System.Drawing.Color.White;
         this.LaserSampleTimeValueButton.ValueHeight = 22;
         this.LaserSampleTimeValueButton.ValueText = "1.23 s";
         this.LaserSampleTimeValueButton.ValueWidth = 80;
         this.LaserSampleTimeValueButton.Click += new System.EventHandler(this.LaserSampleTimeValueButton_Click);
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
         this.LaserSampeCountValueButton.Location = new System.Drawing.Point(217, 68);
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
         // LaserSetupForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(385, 257);
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
      private Controls.E4Button BackButton;
      private System.Windows.Forms.Label TitleLabel;
      private Controls.ValueButton LaserSampeCountValueButton;
      private Controls.ValueButton LaserSampleTimeValueButton;
   }
}