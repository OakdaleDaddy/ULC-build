namespace Weco.Ui
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
         this.components = new System.ComponentModel.Container();
         this.BackButton = new Controls.BaseButton();
         this.MainPanel = new Controls.BorderedPanel();
         this.IntensityProgressBar = new Controls.ProgressBarValueDisplay();
         this.LocationLabel = new System.Windows.Forms.Label();
         this.TitleLabel = new System.Windows.Forms.Label();
         this.DecreaseButton = new Controls.LeftRightButton();
         this.IncreaseButton = new Controls.LeftRightButton();
         this.HoldTimer = new System.Windows.Forms.Timer(this.components);
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
         this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
         // 
         // MainPanel
         // 
         this.MainPanel.BackColor = System.Drawing.Color.Teal;
         this.MainPanel.Controls.Add(this.IntensityProgressBar);
         this.MainPanel.Controls.Add(this.LocationLabel);
         this.MainPanel.Controls.Add(this.TitleLabel);
         this.MainPanel.Controls.Add(this.DecreaseButton);
         this.MainPanel.Controls.Add(this.IncreaseButton);
         this.MainPanel.Controls.Add(this.BackButton);
         this.MainPanel.EdgeWeight = 3;
         this.MainPanel.Location = new System.Drawing.Point(0, 0);
         this.MainPanel.Name = "MainPanel";
         this.MainPanel.Size = new System.Drawing.Size(250, 305);
         this.MainPanel.TabIndex = 7;
         // 
         // IntensityProgressBar
         // 
         this.IntensityProgressBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.IntensityProgressBar.BarColor = System.Drawing.Color.Yellow;
         this.IntensityProgressBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.IntensityProgressBar.Location = new System.Drawing.Point(16, 86);
         this.IntensityProgressBar.Maximum = 100;
         this.IntensityProgressBar.Minimum = 0;
         this.IntensityProgressBar.Name = "IntensityProgressBar";
         this.IntensityProgressBar.Size = new System.Drawing.Size(218, 35);
         this.IntensityProgressBar.TabIndex = 136;
         this.IntensityProgressBar.Text = "progressBarValueDisplay1";
         this.IntensityProgressBar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.IntensityProgressBar.TextColor = System.Drawing.Color.Black;
         this.IntensityProgressBar.Value = 50;
         this.IntensityProgressBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.IntensityProgressBar_MouseDown);
         this.IntensityProgressBar.MouseLeave += new System.EventHandler(this.IntensityProgressBar_MouseLeave);
         this.IntensityProgressBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.IntensityProgressBar_MouseUp);
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
         this.TitleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitleLabel_MouseDown);
         this.TitleLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TitleLabel_MouseMove);
         this.TitleLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TitleLabel_MouseUp);
         // 
         // DecreaseButton
         // 
         this.DecreaseButton.ArrowColor = System.Drawing.Color.Black;
         this.DecreaseButton.ArrowHighlightColor = System.Drawing.Color.DarkGray;
         this.DecreaseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DecreaseButton.DisabledArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.DecreaseButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.DecreaseButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.DecreaseButton.EdgeSpace = 8;
         this.DecreaseButton.HighLightOffset = 7;
         this.DecreaseButton.HighlightVisible = true;
         this.DecreaseButton.HighLightWeight = 2;
         this.DecreaseButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.DecreaseButton.HoldRepeat = true;
         this.DecreaseButton.HoldRepeatInterval = 100;
         this.DecreaseButton.HoldTimeoutInterval = 500;
         this.DecreaseButton.LeftRight = true;
         this.DecreaseButton.Location = new System.Drawing.Point(48, 137);
         this.DecreaseButton.Name = "DecreaseButton";
         this.DecreaseButton.Size = new System.Drawing.Size(69, 69);
         this.DecreaseButton.TabIndex = 18;
         this.DecreaseButton.Text = "leftRightButton2";
         this.DecreaseButton.TextOffset = 0;
         this.DecreaseButton.TextVisible = false;
         this.DecreaseButton.UseVisualStyleBackColor = false;
         this.DecreaseButton.HoldTimeout += new Controls.LeftRightButton.HoldTimeoutHandler(this.DecreaseButton_HoldTimeout);
         this.DecreaseButton.Click += new System.EventHandler(this.DecreaseButton_Click);
         // 
         // IncreaseButton
         // 
         this.IncreaseButton.ArrowColor = System.Drawing.Color.Black;
         this.IncreaseButton.ArrowHighlightColor = System.Drawing.Color.DarkGray;
         this.IncreaseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.IncreaseButton.DisabledArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.IncreaseButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.IncreaseButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.IncreaseButton.EdgeSpace = 8;
         this.IncreaseButton.HighLightOffset = 7;
         this.IncreaseButton.HighlightVisible = true;
         this.IncreaseButton.HighLightWeight = 2;
         this.IncreaseButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.IncreaseButton.HoldRepeat = true;
         this.IncreaseButton.HoldRepeatInterval = 100;
         this.IncreaseButton.HoldTimeoutInterval = 500;
         this.IncreaseButton.LeftRight = false;
         this.IncreaseButton.Location = new System.Drawing.Point(133, 137);
         this.IncreaseButton.Name = "IncreaseButton";
         this.IncreaseButton.Size = new System.Drawing.Size(69, 69);
         this.IncreaseButton.TabIndex = 19;
         this.IncreaseButton.Text = "leftRightButton3";
         this.IncreaseButton.TextOffset = 0;
         this.IncreaseButton.TextVisible = false;
         this.IncreaseButton.UseVisualStyleBackColor = false;
         this.IncreaseButton.HoldTimeout += new Controls.LeftRightButton.HoldTimeoutHandler(this.IncreaseButton_HoldTimeout);
         this.IncreaseButton.Click += new System.EventHandler(this.IncreaseButton_Click);
         // 
         // HoldTimer
         // 
         this.HoldTimer.Tick += new System.EventHandler(this.HoldTimer_Tick);
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
         this.Shown += new System.EventHandler(this.LightIntensitySelectForm_Shown);
         this.MainPanel.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private Controls.BaseButton BackButton;
      private Controls.BorderedPanel MainPanel;
      private Controls.LeftRightButton DecreaseButton;
      private Controls.LeftRightButton IncreaseButton;
      private System.Windows.Forms.Label LocationLabel;
      private System.Windows.Forms.Label TitleLabel;
      private Controls.ProgressBarValueDisplay IntensityProgressBar;
      private System.Windows.Forms.Timer HoldTimer;
   }
}