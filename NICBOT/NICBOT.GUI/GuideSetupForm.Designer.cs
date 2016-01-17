namespace NICBOT.GUI
{
   partial class GuideSetupForm
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
         this.ButtonActionToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.RetractionSpeedValueButton = new NICBOT.GUI.ValueButton();
         this.ExtensionSpeedValueButton = new NICBOT.GUI.ValueButton();
         this.TitleLabel = new System.Windows.Forms.Label();
         this.MainPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // MainPanel
         // 
         this.MainPanel.BackColor = System.Drawing.Color.Olive;
         this.MainPanel.Controls.Add(this.BackButton);
         this.MainPanel.Controls.Add(this.ButtonActionToggleButton);
         this.MainPanel.Controls.Add(this.RetractionSpeedValueButton);
         this.MainPanel.Controls.Add(this.ExtensionSpeedValueButton);
         this.MainPanel.Controls.Add(this.TitleLabel);
         this.MainPanel.EdgeWeight = 3;
         this.MainPanel.Location = new System.Drawing.Point(0, 0);
         this.MainPanel.Name = "MainPanel";
         this.MainPanel.Size = new System.Drawing.Size(385, 257);
         this.MainPanel.TabIndex = 2;
         // 
         // BackButton
         // 
         this.BackButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BackButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BackButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.BackButton.Location = new System.Drawing.Point(139, 174);
         this.BackButton.Name = "BackButton";
         this.BackButton.Size = new System.Drawing.Size(107, 67);
         this.BackButton.TabIndex = 164;
         this.BackButton.Text = "BACK";
         this.BackButton.UseVisualStyleBackColor = false;
         this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
         // 
         // ButtonActionToggleButton
         // 
         this.ButtonActionToggleButton.AutomaticToggle = true;
         this.ButtonActionToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ButtonActionToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ButtonActionToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ButtonActionToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.ButtonActionToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
         this.ButtonActionToggleButton.HoldEnable = true;
         this.ButtonActionToggleButton.HoldTimeoutInterval = 100;
         this.ButtonActionToggleButton.Location = new System.Drawing.Point(254, 68);
         this.ButtonActionToggleButton.Name = "ButtonActionToggleButton";
         this.ButtonActionToggleButton.OptionASelected = true;
         this.ButtonActionToggleButton.OptionAText = "MOM";
         this.ButtonActionToggleButton.OptionBSelected = false;
         this.ButtonActionToggleButton.OptionBText = "ALT";
         this.ButtonActionToggleButton.OptionCenterWidth = 2;
         this.ButtonActionToggleButton.OptionEdgeHeight = 8;
         this.ButtonActionToggleButton.OptionHeight = 22;
         this.ButtonActionToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.ButtonActionToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
         this.ButtonActionToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.ButtonActionToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.ButtonActionToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ButtonActionToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.ButtonActionToggleButton.OptionWidth = 45;
         this.ButtonActionToggleButton.Size = new System.Drawing.Size(107, 90);
         this.ButtonActionToggleButton.TabIndex = 136;
         this.ButtonActionToggleButton.Text = "BUTTON ACTION";
         this.ButtonActionToggleButton.UseVisualStyleBackColor = false;
         this.ButtonActionToggleButton.Click += new System.EventHandler(this.ButtonActionToggleButton_Click);
         // 
         // RetractionSpeedValueButton
         // 
         this.RetractionSpeedValueButton.ArrowWidth = 0;
         this.RetractionSpeedValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RetractionSpeedValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.RetractionSpeedValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.RetractionSpeedValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.RetractionSpeedValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.RetractionSpeedValueButton.HoldTimeoutInterval = 0;
         this.RetractionSpeedValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.RetractionSpeedValueButton.LeftArrowVisible = false;
         this.RetractionSpeedValueButton.Location = new System.Drawing.Point(139, 68);
         this.RetractionSpeedValueButton.Name = "RetractionSpeedValueButton";
         this.RetractionSpeedValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.RetractionSpeedValueButton.RightArrowVisible = false;
         this.RetractionSpeedValueButton.Size = new System.Drawing.Size(107, 90);
         this.RetractionSpeedValueButton.TabIndex = 135;
         this.RetractionSpeedValueButton.Text = "RETRACTION SPEED";
         this.RetractionSpeedValueButton.UseVisualStyleBackColor = false;
         this.RetractionSpeedValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.RetractionSpeedValueButton.ValueEdgeHeight = 8;
         this.RetractionSpeedValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.RetractionSpeedValueButton.ValueForeColor = System.Drawing.Color.White;
         this.RetractionSpeedValueButton.ValueHeight = 22;
         this.RetractionSpeedValueButton.ValueText = "### RPM";
         this.RetractionSpeedValueButton.ValueWidth = 80;
         this.RetractionSpeedValueButton.Click += new System.EventHandler(this.RetractionSpeedValueButton_Click);
         // 
         // ExtensionSpeedValueButton
         // 
         this.ExtensionSpeedValueButton.ArrowWidth = 0;
         this.ExtensionSpeedValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ExtensionSpeedValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ExtensionSpeedValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ExtensionSpeedValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.ExtensionSpeedValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ExtensionSpeedValueButton.HoldTimeoutInterval = 0;
         this.ExtensionSpeedValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.ExtensionSpeedValueButton.LeftArrowVisible = false;
         this.ExtensionSpeedValueButton.Location = new System.Drawing.Point(24, 68);
         this.ExtensionSpeedValueButton.Name = "ExtensionSpeedValueButton";
         this.ExtensionSpeedValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.ExtensionSpeedValueButton.RightArrowVisible = false;
         this.ExtensionSpeedValueButton.Size = new System.Drawing.Size(107, 90);
         this.ExtensionSpeedValueButton.TabIndex = 134;
         this.ExtensionSpeedValueButton.Text = "EXTENSION SPEED";
         this.ExtensionSpeedValueButton.UseVisualStyleBackColor = false;
         this.ExtensionSpeedValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.ExtensionSpeedValueButton.ValueEdgeHeight = 8;
         this.ExtensionSpeedValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.ExtensionSpeedValueButton.ValueForeColor = System.Drawing.Color.White;
         this.ExtensionSpeedValueButton.ValueHeight = 22;
         this.ExtensionSpeedValueButton.ValueText = "### RPM";
         this.ExtensionSpeedValueButton.ValueWidth = 80;
         this.ExtensionSpeedValueButton.Click += new System.EventHandler(this.ExtensionSpeedValueButton_Click);
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
         this.TitleLabel.Text = "TETHER GUIDE SETUP";
         this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // GuideSetupForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(385, 257);
         this.Controls.Add(this.MainPanel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "GuideSetupForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "TetherGuildSetupForm";
         this.Shown += new System.EventHandler(this.GuildSetupForm_Shown);
         this.MainPanel.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private BorderedPanel MainPanel;
      private ValueButton RetractionSpeedValueButton;
      private ValueButton ExtensionSpeedValueButton;
      private System.Windows.Forms.Label TitleLabel;
      private ValueToggleButton ButtonActionToggleButton;
      private NicBotButton BackButton;
   }
}