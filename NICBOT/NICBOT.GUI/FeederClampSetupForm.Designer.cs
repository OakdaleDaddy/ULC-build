namespace NICBOT.GUI
{
   partial class FeederClampSetupForm
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
         this.BackButton = new NICBOT.GUI.NicBotButton();
         this.TitleLabel = new System.Windows.Forms.Label();
         this.ClampHoldButton = new NICBOT.GUI.HoldButton();
         this.ClampReleaseButton = new NICBOT.GUI.HoldButton();
         this.MainPanel = new NICBOT.GUI.BorderedPanel();
         this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
         this.MainPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // BackButton
         // 
         this.BackButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BackButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BackButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.BackButton.Location = new System.Drawing.Point(86, 174);
         this.BackButton.Name = "BackButton";
         this.BackButton.Size = new System.Drawing.Size(107, 67);
         this.BackButton.TabIndex = 175;
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
         this.TitleLabel.Size = new System.Drawing.Size(252, 36);
         this.TitleLabel.TabIndex = 176;
         this.TitleLabel.Text = "FEEDER CLAMP SETUP";
         this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // ClampHoldButton
         // 
         this.ClampHoldButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ClampHoldButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ClampHoldButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ClampHoldButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.ClampHoldButton.HoldTimeoutEnable = true;
         this.ClampHoldButton.HoldTimeoutInterval = 100;
         this.ClampHoldButton.Location = new System.Drawing.Point(27, 68);
         this.ClampHoldButton.Name = "ClampHoldButton";
         this.ClampHoldButton.Size = new System.Drawing.Size(107, 90);
         this.ClampHoldButton.TabIndex = 177;
         this.ClampHoldButton.Text = "CLAMP HOLD";
         this.ClampHoldButton.UseVisualStyleBackColor = false;
         this.ClampHoldButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.ClampHoldButton_HoldTimeout);
         // 
         // ClampReleaseButton
         // 
         this.ClampReleaseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ClampReleaseButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ClampReleaseButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ClampReleaseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.ClampReleaseButton.HoldTimeoutEnable = true;
         this.ClampReleaseButton.HoldTimeoutInterval = 100;
         this.ClampReleaseButton.Location = new System.Drawing.Point(150, 68);
         this.ClampReleaseButton.Name = "ClampReleaseButton";
         this.ClampReleaseButton.Size = new System.Drawing.Size(107, 90);
         this.ClampReleaseButton.TabIndex = 178;
         this.ClampReleaseButton.Text = "CLAMP RELEASE";
         this.ClampReleaseButton.UseVisualStyleBackColor = false;
         this.ClampReleaseButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.ClampReleaseButton_HoldTimeout);
         // 
         // MainPanel
         // 
         this.MainPanel.BackColor = System.Drawing.Color.Olive;
         this.MainPanel.Controls.Add(this.ClampHoldButton);
         this.MainPanel.Controls.Add(this.ClampReleaseButton);
         this.MainPanel.Controls.Add(this.BackButton);
         this.MainPanel.Controls.Add(this.TitleLabel);
         this.MainPanel.EdgeWeight = 3;
         this.MainPanel.Location = new System.Drawing.Point(0, 0);
         this.MainPanel.Name = "MainPanel";
         this.MainPanel.Size = new System.Drawing.Size(284, 257);
         this.MainPanel.TabIndex = 179;
         // 
         // UpdateTimer
         // 
         this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
         // 
         // FeederClampSetupForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(284, 257);
         this.Controls.Add(this.MainPanel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "FeederClampSetupForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "FeederGrabberSetupForm";
         this.Load += new System.EventHandler(this.FeederClampSetupForm_Load);
         this.MainPanel.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private NicBotButton BackButton;
      private System.Windows.Forms.Label TitleLabel;
      private HoldButton ClampHoldButton;
      private HoldButton ClampReleaseButton;
      private BorderedPanel MainPanel;
      private System.Windows.Forms.Timer UpdateTimer;
   }
}