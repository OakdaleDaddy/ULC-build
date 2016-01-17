namespace NICBOT.GUI
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
         this.LocationLabel = new System.Windows.Forms.Label();
         this.BackButton = new System.Windows.Forms.Button();
         this.TitleLabel = new System.Windows.Forms.Label();
         this.IntensityProgressBar = new ProgressBarValueDisplay();
         this.DownButton = new UpDownButton();
         this.UpButton = new UpDownButton();
         this.HoldTimer = new System.Windows.Forms.Timer(this.components);
         this.SuspendLayout();
         // 
         // LocationLabel
         // 
         this.LocationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LocationLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.LocationLabel.Location = new System.Drawing.Point(18, 48);
         this.LocationLabel.Name = "LocationLabel";
         this.LocationLabel.Size = new System.Drawing.Size(215, 35);
         this.LocationLabel.TabIndex = 28;
         this.LocationLabel.Text = "CAM1";
         this.LocationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // BackButton
         // 
         this.BackButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BackButton.ForeColor = System.Drawing.Color.Black;
         this.BackButton.Location = new System.Drawing.Point(72, 283);
         this.BackButton.Name = "BackButton";
         this.BackButton.Size = new System.Drawing.Size(107, 67);
         this.BackButton.TabIndex = 32;
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
         this.TitleLabel.Location = new System.Drawing.Point(16, 11);
         this.TitleLabel.Name = "TitleLabel";
         this.TitleLabel.Size = new System.Drawing.Size(218, 35);
         this.TitleLabel.TabIndex = 37;
         this.TitleLabel.Text = "LIGHT ADJUST";
         this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // IntensityProgressBar
         // 
         this.IntensityProgressBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.IntensityProgressBar.BarColor = System.Drawing.Color.White;
         this.IntensityProgressBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.IntensityProgressBar.Location = new System.Drawing.Point(16, 86);
         this.IntensityProgressBar.Margin = new System.Windows.Forms.Padding(5);
         this.IntensityProgressBar.Maximum = 100;
         this.IntensityProgressBar.Minimum = 0;
         this.IntensityProgressBar.Name = "IntensityProgressBar";
         this.IntensityProgressBar.Size = new System.Drawing.Size(218, 35);
         this.IntensityProgressBar.TabIndex = 38;
         this.IntensityProgressBar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.IntensityProgressBar.TextColor = System.Drawing.Color.Black;
         this.IntensityProgressBar.Value = 10;
         this.IntensityProgressBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.IntensityProgressBar_MouseDown);
         this.IntensityProgressBar.MouseLeave += new System.EventHandler(this.IntensityProgressBar_MouseLeave);
         this.IntensityProgressBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.IntensityProgressBar_MouseUp);
         // 
         // DownButton
         // 
         this.DownButton.ArrowColor = System.Drawing.Color.Black;
         this.DownButton.ArrowHighlightColor = System.Drawing.Color.DarkGray;
         this.DownButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DownButton.EdgeSpace = 8;
         this.DownButton.HighLightOffset = 7;
         this.DownButton.HighlightVisible = true;
         this.DownButton.HighLightWeight = 2;
         this.DownButton.HoldRepeat = true;
         this.DownButton.HoldRepeatInterval = 100;
         this.DownButton.HoldTimeoutInterval = 500;
         this.DownButton.Location = new System.Drawing.Point(72, 204);
         this.DownButton.Name = "DownButton";
         this.DownButton.Size = new System.Drawing.Size(107, 67);
         this.DownButton.TabIndex = 36;
         this.DownButton.Text = "upDownButton2";
         this.DownButton.TextOffset = 0;
         this.DownButton.TextVisible = false;
         this.DownButton.UpDown = false;
         this.DownButton.UseVisualStyleBackColor = false;
         this.DownButton.HoldTimeout += new UpDownButton.HoldTimeoutHandler(this.DownButton_HoldTimeout);
         this.DownButton.Click += new System.EventHandler(this.DownButton_Click);
         // 
         // UpButton
         // 
         this.UpButton.ArrowColor = System.Drawing.Color.Black;
         this.UpButton.ArrowHighlightColor = System.Drawing.Color.DarkGray;
         this.UpButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.UpButton.EdgeSpace = 8;
         this.UpButton.HighLightOffset = 7;
         this.UpButton.HighlightVisible = true;
         this.UpButton.HighLightWeight = 2;
         this.UpButton.HoldRepeat = true;
         this.UpButton.HoldRepeatInterval = 100;
         this.UpButton.HoldTimeoutInterval = 500;
         this.UpButton.Location = new System.Drawing.Point(72, 129);
         this.UpButton.Name = "UpButton";
         this.UpButton.Size = new System.Drawing.Size(107, 67);
         this.UpButton.TabIndex = 34;
         this.UpButton.Text = "upDownButton1";
         this.UpButton.TextOffset = 0;
         this.UpButton.TextVisible = false;
         this.UpButton.UpDown = true;
         this.UpButton.UseVisualStyleBackColor = false;
         this.UpButton.HoldTimeout += new UpDownButton.HoldTimeoutHandler(this.UpButton_HoldTimeout);
         this.UpButton.Click += new System.EventHandler(this.UpButton_Click);
         // 
         // HoldTimer
         // 
         this.HoldTimer.Tick += new System.EventHandler(this.HoldTimer_Tick);
         // 
         // LightIntensitySelectForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.ClientSize = new System.Drawing.Size(250, 366);
         this.Controls.Add(this.IntensityProgressBar);
         this.Controls.Add(this.TitleLabel);
         this.Controls.Add(this.DownButton);
         this.Controls.Add(this.UpButton);
         this.Controls.Add(this.BackButton);
         this.Controls.Add(this.LocationLabel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "LightIntensitySelectForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "LightIntensitySelectForm";
         this.Shown += new System.EventHandler(this.LightIntensitySelectForm_Shown);
         this.Paint += new System.Windows.Forms.PaintEventHandler(this.LightIntensitySelectForm_Paint);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Label LocationLabel;
      private System.Windows.Forms.Button BackButton;
      private UpDownButton UpButton;
      private UpDownButton DownButton;
      private System.Windows.Forms.Label TitleLabel;
      private ProgressBarValueDisplay IntensityProgressBar;
      private System.Windows.Forms.Timer HoldTimer;
   }
}