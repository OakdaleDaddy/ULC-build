namespace NICBOT.GUI
{
   partial class RobotBusControlForm
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
         this.MainPanel = new NICBOT.GUI.BorderedPanel();
         this.ActivityButton = new NICBOT.GUI.HoldButton();
         this.BusStatusTextBox = new System.Windows.Forms.TextBox();
         this.StateToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.TitleLabel = new System.Windows.Forms.Label();
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
         this.BackButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.BackButton.Location = new System.Drawing.Point(300, 193);
         this.BackButton.Name = "BackButton";
         this.BackButton.Size = new System.Drawing.Size(107, 67);
         this.BackButton.TabIndex = 167;
         this.BackButton.Text = "BACK";
         this.BackButton.UseVisualStyleBackColor = false;
         this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
         // 
         // MainPanel
         // 
         this.MainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
         this.MainPanel.Controls.Add(this.ActivityButton);
         this.MainPanel.Controls.Add(this.BusStatusTextBox);
         this.MainPanel.Controls.Add(this.StateToggleButton);
         this.MainPanel.Controls.Add(this.TitleLabel);
         this.MainPanel.Controls.Add(this.BackButton);
         this.MainPanel.EdgeWeight = 3;
         this.MainPanel.Location = new System.Drawing.Point(0, 0);
         this.MainPanel.Name = "MainPanel";
         this.MainPanel.Size = new System.Drawing.Size(434, 276);
         this.MainPanel.TabIndex = 168;
         // 
         // ActivityButton
         // 
         this.ActivityButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ActivityButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ActivityButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ActivityButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.ActivityButton.ForeColor = System.Drawing.Color.Black;
         this.ActivityButton.HoldArrorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.ActivityButton.HoldTimeoutEnable = true;
         this.ActivityButton.HoldTimeoutInterval = 100;
         this.ActivityButton.Location = new System.Drawing.Point(163, 110);
         this.ActivityButton.Name = "ActivityButton";
         this.ActivityButton.Size = new System.Drawing.Size(107, 67);
         this.ActivityButton.TabIndex = 185;
         this.ActivityButton.Text = "ACTIVITY";
         this.ActivityButton.UseVisualStyleBackColor = false;
         this.ActivityButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.ActivityButton_HoldTimeout);
         // 
         // BusStatusTextBox
         // 
         this.BusStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.BusStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.BusStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.BusStatusTextBox.Location = new System.Drawing.Point(27, 68);
         this.BusStatusTextBox.Name = "BusStatusTextBox";
         this.BusStatusTextBox.ReadOnly = true;
         this.BusStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.BusStatusTextBox.TabIndex = 184;
         this.BusStatusTextBox.Text = "not connected";
         this.BusStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // StateToggleButton
         // 
         this.StateToggleButton.AutomaticToggle = true;
         this.StateToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.StateToggleButton.DisabledBackColor = System.Drawing.Color.Black;
         this.StateToggleButton.DisabledForeColor = System.Drawing.Color.Gray;
         this.StateToggleButton.DisabledOptionBackColor = System.Drawing.Color.Black;
         this.StateToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.StateToggleButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.StateToggleButton.HoldEnable = false;
         this.StateToggleButton.HoldTimeoutInterval = 0;
         this.StateToggleButton.Location = new System.Drawing.Point(27, 110);
         this.StateToggleButton.Name = "StateToggleButton";
         this.StateToggleButton.OptionASelected = true;
         this.StateToggleButton.OptionAText = "ENABLE";
         this.StateToggleButton.OptionBSelected = false;
         this.StateToggleButton.OptionBText = "DISABLE";
         this.StateToggleButton.OptionCenterWidth = 0;
         this.StateToggleButton.OptionEdgeHeight = 8;
         this.StateToggleButton.OptionHeight = 22;
         this.StateToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.StateToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.StateToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.StateToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.StateToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.StateToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.StateToggleButton.OptionWidth = 50;
         this.StateToggleButton.Size = new System.Drawing.Size(107, 67);
         this.StateToggleButton.TabIndex = 172;
         this.StateToggleButton.Text = "STATE";
         this.StateToggleButton.UseVisualStyleBackColor = false;
         this.StateToggleButton.Click += new System.EventHandler(this.StateToggleButton_Click);
         // 
         // TitleLabel
         // 
         this.TitleLabel.BackColor = System.Drawing.Color.Teal;
         this.TitleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TitleLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.TitleLabel.Location = new System.Drawing.Point(16, 16);
         this.TitleLabel.Name = "TitleLabel";
         this.TitleLabel.Size = new System.Drawing.Size(402, 36);
         this.TitleLabel.TabIndex = 170;
         this.TitleLabel.Text = "ROBOT BUS";
         this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // UpdateTimer
         // 
         this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
         // 
         // RobotBusControlForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(434, 276);
         this.Controls.Add(this.MainPanel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "RobotBusControlForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "RobotBusControlForm";
         this.Shown += new System.EventHandler(this.RobotBusControlForm_Shown);
         this.MainPanel.ResumeLayout(false);
         this.MainPanel.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private NicBotButton BackButton;
      private BorderedPanel MainPanel;
      private System.Windows.Forms.Label TitleLabel;
      private ValueToggleButton StateToggleButton;
      private System.Windows.Forms.TextBox BusStatusTextBox;
      private HoldButton ActivityButton;
      private System.Windows.Forms.Timer UpdateTimer;
   }
}