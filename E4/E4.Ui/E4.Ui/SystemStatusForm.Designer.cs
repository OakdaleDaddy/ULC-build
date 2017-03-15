namespace E4.Ui
{
   partial class SystemStatusForm
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
         this.SettingsPanel = new E4.Ui.Controls.BorderedPanel();
         this.label15 = new System.Windows.Forms.Label();
         this.TriggerDefaultsButton = new E4.Ui.Controls.HoldButton();
         this.SaveDefaultsButton = new E4.Ui.Controls.HoldButton();
         this.DeviceStatusPanel = new E4.Ui.Controls.BorderedPanel();
         this.ComponentStatusLabel = new System.Windows.Forms.Label();
         this.MainPanel.SuspendLayout();
         this.SettingsPanel.SuspendLayout();
         this.DeviceStatusPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // MainPanel
         // 
         this.MainPanel.BackColor = System.Drawing.Color.SeaGreen;
         this.MainPanel.Controls.Add(this.DeviceStatusPanel);
         this.MainPanel.Controls.Add(this.SettingsPanel);
         this.MainPanel.Controls.Add(this.BackButton);
         this.MainPanel.EdgeWeight = 3;
         this.MainPanel.Location = new System.Drawing.Point(0, 0);
         this.MainPanel.Name = "MainPanel";
         this.MainPanel.Size = new System.Drawing.Size(839, 580);
         this.MainPanel.TabIndex = 0;
         // 
         // BackButton
         // 
         this.BackButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BackButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BackButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.BackButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.BackButton.Location = new System.Drawing.Point(681, 466);
         this.BackButton.Name = "BackButton";
         this.BackButton.Size = new System.Drawing.Size(107, 67);
         this.BackButton.TabIndex = 4;
         this.BackButton.Text = "BACK";
         this.BackButton.UseVisualStyleBackColor = false;
         this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
         // 
         // SettingsPanel
         // 
         this.SettingsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
         this.SettingsPanel.Controls.Add(this.SaveDefaultsButton);
         this.SettingsPanel.Controls.Add(this.TriggerDefaultsButton);
         this.SettingsPanel.Controls.Add(this.label15);
         this.SettingsPanel.EdgeWeight = 3;
         this.SettingsPanel.Location = new System.Drawing.Point(548, 12);
         this.SettingsPanel.Name = "SettingsPanel";
         this.SettingsPanel.Size = new System.Drawing.Size(275, 130);
         this.SettingsPanel.TabIndex = 5;
         // 
         // label15
         // 
         this.label15.BackColor = System.Drawing.Color.Teal;
         this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label15.ForeColor = System.Drawing.Color.White;
         this.label15.Location = new System.Drawing.Point(16, 16);
         this.label15.Name = "label15";
         this.label15.Size = new System.Drawing.Size(243, 23);
         this.label15.TabIndex = 201;
         this.label15.Text = "SETTINGS";
         this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // TriggerDefaultsButton
         // 
         this.TriggerDefaultsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.TriggerDefaultsButton.DisabledBackColor = System.Drawing.SystemColors.ControlDarkDark;
         this.TriggerDefaultsButton.DisabledForeColor = System.Drawing.Color.Gray;
         this.TriggerDefaultsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.TriggerDefaultsButton.ForeColor = System.Drawing.Color.Black;
         this.TriggerDefaultsButton.HoldArrorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.TriggerDefaultsButton.HoldTimeoutEnable = true;
         this.TriggerDefaultsButton.HoldTimeoutInterval = 100;
         this.TriggerDefaultsButton.Location = new System.Drawing.Point(27, 47);
         this.TriggerDefaultsButton.Name = "TriggerDefaultsButton";
         this.TriggerDefaultsButton.Size = new System.Drawing.Size(107, 67);
         this.TriggerDefaultsButton.TabIndex = 202;
         this.TriggerDefaultsButton.Text = "TRIGGER DEFAULTS";
         this.TriggerDefaultsButton.UseVisualStyleBackColor = false;
         this.TriggerDefaultsButton.HoldTimeout += new E4.Ui.Controls.HoldTimeoutHandler(this.TriggerDefaultsButton_HoldTimeout);
         // 
         // SaveDefaultsButton
         // 
         this.SaveDefaultsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SaveDefaultsButton.DisabledBackColor = System.Drawing.SystemColors.ControlDarkDark;
         this.SaveDefaultsButton.DisabledForeColor = System.Drawing.Color.Gray;
         this.SaveDefaultsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.SaveDefaultsButton.ForeColor = System.Drawing.Color.Black;
         this.SaveDefaultsButton.HoldArrorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.SaveDefaultsButton.HoldTimeoutEnable = true;
         this.SaveDefaultsButton.HoldTimeoutInterval = 100;
         this.SaveDefaultsButton.Location = new System.Drawing.Point(142, 47);
         this.SaveDefaultsButton.Name = "SaveDefaultsButton";
         this.SaveDefaultsButton.Size = new System.Drawing.Size(107, 67);
         this.SaveDefaultsButton.TabIndex = 203;
         this.SaveDefaultsButton.Text = "SAVE AS DEFAULTS";
         this.SaveDefaultsButton.UseVisualStyleBackColor = false;
         this.SaveDefaultsButton.HoldTimeout += new E4.Ui.Controls.HoldTimeoutHandler(this.SaveDefaultsButton_HoldTimeout);
         // 
         // DeviceStatusPanel
         // 
         this.DeviceStatusPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
         this.DeviceStatusPanel.Controls.Add(this.ComponentStatusLabel);
         this.DeviceStatusPanel.EdgeWeight = 3;
         this.DeviceStatusPanel.Location = new System.Drawing.Point(12, 12);
         this.DeviceStatusPanel.Name = "DeviceStatusPanel";
         this.DeviceStatusPanel.Size = new System.Drawing.Size(530, 537);
         this.DeviceStatusPanel.TabIndex = 6;
         // 
         // ComponentStatusLabel
         // 
         this.ComponentStatusLabel.BackColor = System.Drawing.Color.Teal;
         this.ComponentStatusLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.ComponentStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ComponentStatusLabel.ForeColor = System.Drawing.Color.White;
         this.ComponentStatusLabel.Location = new System.Drawing.Point(24, 14);
         this.ComponentStatusLabel.Name = "ComponentStatusLabel";
         this.ComponentStatusLabel.Size = new System.Drawing.Size(461, 23);
         this.ComponentStatusLabel.TabIndex = 201;
         this.ComponentStatusLabel.Text = "COMPONENT STATUS";
         this.ComponentStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.ComponentStatusLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ComponentStatusLabel_MouseDown);
         this.ComponentStatusLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ComponentStatusLabel_MouseMove);
         this.ComponentStatusLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ComponentStatusLabel_MouseUp);
         // 
         // SystemStatusForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(851, 592);
         this.Controls.Add(this.MainPanel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "SystemStatusForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "SystemStatusForm";
         this.MainPanel.ResumeLayout(false);
         this.SettingsPanel.ResumeLayout(false);
         this.DeviceStatusPanel.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private Controls.BorderedPanel MainPanel;
      private Controls.E4Button BackButton;
      private Controls.BorderedPanel SettingsPanel;
      private System.Windows.Forms.Label label15;
      private Controls.HoldButton SaveDefaultsButton;
      private Controls.HoldButton TriggerDefaultsButton;
      private Controls.BorderedPanel DeviceStatusPanel;
      private System.Windows.Forms.Label ComponentStatusLabel;
   }
}