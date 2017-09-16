
namespace CrossBore.BusSim
{
   partial class BldcMotor
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

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.MainPanel = new System.Windows.Forms.Panel();
         this.MainTabControl = new System.Windows.Forms.TabControl();
         this.InterfaceTabPage = new System.Windows.Forms.TabPage();
         this.SetMotorCurrentButton = new System.Windows.Forms.Button();
         this.SetInputsButton = new System.Windows.Forms.Button();
         this.SetMotorTemperatureButton = new System.Windows.Forms.Button();
         this.InputsTextBox = new System.Windows.Forms.TextBox();
         this.MotorTemperatureTextBox = new System.Windows.Forms.TextBox();
         this.FormMotorTemperatureLabel = new System.Windows.Forms.Label();
         this.MotorCurrentTextBox = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.AutoHomEnabledCheckBox = new System.Windows.Forms.CheckBox();
         this.HomeSwitchButton = new System.Windows.Forms.Button();
         this.EmergencyTabPage = new System.Windows.Forms.TabPage();
         this.ExcessTemperatureErrorButton = new System.Windows.Forms.Button();
         this.DcLinkUnderVoltageErrorButton = new System.Windows.Forms.Button();
         this.PeakCurrentErrorButton = new System.Windows.Forms.Button();
         this.AverageCurrentErrorButton = new System.Windows.Forms.Button();
         this.ProcessImageTabPage = new System.Windows.Forms.TabPage();
         this.ValuePanel = new System.Windows.Forms.Panel();
         this.FocusTakerLabel = new System.Windows.Forms.Label();
         this.MainPanel.SuspendLayout();
         this.MainTabControl.SuspendLayout();
         this.InterfaceTabPage.SuspendLayout();
         this.EmergencyTabPage.SuspendLayout();
         this.ProcessImageTabPage.SuspendLayout();
         this.SuspendLayout();
         // 
         // MainPanel
         // 
         this.MainPanel.AutoScroll = true;
         this.MainPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.MainPanel.Controls.Add(this.MainTabControl);
         this.MainPanel.Controls.Add(this.FocusTakerLabel);
         this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.MainPanel.Location = new System.Drawing.Point(0, 0);
         this.MainPanel.Name = "MainPanel";
         this.MainPanel.Size = new System.Drawing.Size(575, 134);
         this.MainPanel.TabIndex = 0;
         this.MainPanel.Scroll += new System.Windows.Forms.ScrollEventHandler(this.MainPanel_Scroll);
         // 
         // MainTabControl
         // 
         this.MainTabControl.Controls.Add(this.InterfaceTabPage);
         this.MainTabControl.Controls.Add(this.EmergencyTabPage);
         this.MainTabControl.Controls.Add(this.ProcessImageTabPage);
         this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.MainTabControl.Location = new System.Drawing.Point(0, 0);
         this.MainTabControl.Name = "MainTabControl";
         this.MainTabControl.SelectedIndex = 0;
         this.MainTabControl.Size = new System.Drawing.Size(571, 130);
         this.MainTabControl.TabIndex = 210;
         // 
         // InterfaceTabPage
         // 
         this.InterfaceTabPage.BackColor = System.Drawing.Color.Gainsboro;
         this.InterfaceTabPage.Controls.Add(this.SetMotorCurrentButton);
         this.InterfaceTabPage.Controls.Add(this.SetInputsButton);
         this.InterfaceTabPage.Controls.Add(this.SetMotorTemperatureButton);
         this.InterfaceTabPage.Controls.Add(this.InputsTextBox);
         this.InterfaceTabPage.Controls.Add(this.MotorTemperatureTextBox);
         this.InterfaceTabPage.Controls.Add(this.FormMotorTemperatureLabel);
         this.InterfaceTabPage.Controls.Add(this.MotorCurrentTextBox);
         this.InterfaceTabPage.Controls.Add(this.label2);
         this.InterfaceTabPage.Controls.Add(this.label1);
         this.InterfaceTabPage.Controls.Add(this.AutoHomEnabledCheckBox);
         this.InterfaceTabPage.Controls.Add(this.HomeSwitchButton);
         this.InterfaceTabPage.Location = new System.Drawing.Point(4, 22);
         this.InterfaceTabPage.Name = "InterfaceTabPage";
         this.InterfaceTabPage.Padding = new System.Windows.Forms.Padding(3);
         this.InterfaceTabPage.Size = new System.Drawing.Size(563, 104);
         this.InterfaceTabPage.TabIndex = 0;
         this.InterfaceTabPage.Text = "Interface";
         // 
         // SetMotorCurrentButton
         // 
         this.SetMotorCurrentButton.Location = new System.Drawing.Point(143, 30);
         this.SetMotorCurrentButton.Name = "SetMotorCurrentButton";
         this.SetMotorCurrentButton.Size = new System.Drawing.Size(35, 23);
         this.SetMotorCurrentButton.TabIndex = 206;
         this.SetMotorCurrentButton.Text = "Set";
         this.SetMotorCurrentButton.UseVisualStyleBackColor = true;
         this.SetMotorCurrentButton.Click += new System.EventHandler(this.SetMotorCurrentButton_Click);
         // 
         // SetInputsButton
         // 
         this.SetInputsButton.Location = new System.Drawing.Point(143, 56);
         this.SetInputsButton.Name = "SetInputsButton";
         this.SetInputsButton.Size = new System.Drawing.Size(35, 23);
         this.SetInputsButton.TabIndex = 209;
         this.SetInputsButton.Text = "Set";
         this.SetInputsButton.UseVisualStyleBackColor = true;
         this.SetInputsButton.Click += new System.EventHandler(this.SetInputsButton_Click);
         // 
         // SetMotorTemperatureButton
         // 
         this.SetMotorTemperatureButton.Location = new System.Drawing.Point(143, 4);
         this.SetMotorTemperatureButton.Name = "SetMotorTemperatureButton";
         this.SetMotorTemperatureButton.Size = new System.Drawing.Size(35, 23);
         this.SetMotorTemperatureButton.TabIndex = 199;
         this.SetMotorTemperatureButton.Text = "Set";
         this.SetMotorTemperatureButton.UseVisualStyleBackColor = true;
         this.SetMotorTemperatureButton.Click += new System.EventHandler(this.SetMotorTemperatureButton_Click);
         // 
         // InputsTextBox
         // 
         this.InputsTextBox.Location = new System.Drawing.Point(103, 58);
         this.InputsTextBox.MaxLength = 0;
         this.InputsTextBox.Name = "InputsTextBox";
         this.InputsTextBox.Size = new System.Drawing.Size(34, 20);
         this.InputsTextBox.TabIndex = 208;
         this.InputsTextBox.Text = "0";
         this.InputsTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // MotorTemperatureTextBox
         // 
         this.MotorTemperatureTextBox.Location = new System.Drawing.Point(103, 6);
         this.MotorTemperatureTextBox.MaxLength = 0;
         this.MotorTemperatureTextBox.Name = "MotorTemperatureTextBox";
         this.MotorTemperatureTextBox.Size = new System.Drawing.Size(34, 20);
         this.MotorTemperatureTextBox.TabIndex = 198;
         this.MotorTemperatureTextBox.Text = "23";
         this.MotorTemperatureTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // FormMotorTemperatureLabel
         // 
         this.FormMotorTemperatureLabel.AutoSize = true;
         this.FormMotorTemperatureLabel.Location = new System.Drawing.Point(4, 9);
         this.FormMotorTemperatureLabel.Name = "FormMotorTemperatureLabel";
         this.FormMotorTemperatureLabel.Size = new System.Drawing.Size(97, 13);
         this.FormMotorTemperatureLabel.TabIndex = 1;
         this.FormMotorTemperatureLabel.Text = "Motor Temperature";
         // 
         // MotorCurrentTextBox
         // 
         this.MotorCurrentTextBox.Location = new System.Drawing.Point(103, 32);
         this.MotorCurrentTextBox.MaxLength = 0;
         this.MotorCurrentTextBox.Name = "MotorCurrentTextBox";
         this.MotorCurrentTextBox.Size = new System.Drawing.Size(34, 20);
         this.MotorCurrentTextBox.TabIndex = 204;
         this.MotorCurrentTextBox.Text = "23";
         this.MotorCurrentTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(65, 61);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(36, 13);
         this.label2.TabIndex = 207;
         this.label2.Text = "Inputs";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(30, 35);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(71, 13);
         this.label1.TabIndex = 205;
         this.label1.Text = "Motor Current";
         // 
         // AutoHomEnabledCheckBox
         // 
         this.AutoHomEnabledCheckBox.AutoSize = true;
         this.AutoHomEnabledCheckBox.Location = new System.Drawing.Point(285, 9);
         this.AutoHomEnabledCheckBox.Name = "AutoHomEnabledCheckBox";
         this.AutoHomEnabledCheckBox.Size = new System.Drawing.Size(15, 14);
         this.AutoHomEnabledCheckBox.TabIndex = 202;
         this.AutoHomEnabledCheckBox.UseVisualStyleBackColor = true;
         // 
         // HomeSwitchButton
         // 
         this.HomeSwitchButton.Location = new System.Drawing.Point(194, 4);
         this.HomeSwitchButton.Name = "HomeSwitchButton";
         this.HomeSwitchButton.Size = new System.Drawing.Size(85, 23);
         this.HomeSwitchButton.TabIndex = 201;
         this.HomeSwitchButton.Text = "Home Switch";
         this.HomeSwitchButton.UseVisualStyleBackColor = true;
         this.HomeSwitchButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HomeSwitchButton_MouseDown);
         this.HomeSwitchButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.HomeSwitchButton_MouseUp);
         // 
         // EmergencyTabPage
         // 
         this.EmergencyTabPage.BackColor = System.Drawing.Color.Gainsboro;
         this.EmergencyTabPage.Controls.Add(this.ExcessTemperatureErrorButton);
         this.EmergencyTabPage.Controls.Add(this.DcLinkUnderVoltageErrorButton);
         this.EmergencyTabPage.Controls.Add(this.PeakCurrentErrorButton);
         this.EmergencyTabPage.Controls.Add(this.AverageCurrentErrorButton);
         this.EmergencyTabPage.Location = new System.Drawing.Point(4, 22);
         this.EmergencyTabPage.Name = "EmergencyTabPage";
         this.EmergencyTabPage.Size = new System.Drawing.Size(563, 104);
         this.EmergencyTabPage.TabIndex = 2;
         this.EmergencyTabPage.Text = "Emergency";
         // 
         // ExcessTemperatureErrorButton
         // 
         this.ExcessTemperatureErrorButton.Location = new System.Drawing.Point(142, 35);
         this.ExcessTemperatureErrorButton.Name = "ExcessTemperatureErrorButton";
         this.ExcessTemperatureErrorButton.Size = new System.Drawing.Size(130, 23);
         this.ExcessTemperatureErrorButton.TabIndex = 288;
         this.ExcessTemperatureErrorButton.Text = "Excess Temperature";
         this.ExcessTemperatureErrorButton.UseVisualStyleBackColor = true;
         this.ExcessTemperatureErrorButton.Click += new System.EventHandler(this.ExcessTemperatureErrorButton_Click);
         // 
         // DcLinkUnderVoltageErrorButton
         // 
         this.DcLinkUnderVoltageErrorButton.Location = new System.Drawing.Point(142, 6);
         this.DcLinkUnderVoltageErrorButton.Name = "DcLinkUnderVoltageErrorButton";
         this.DcLinkUnderVoltageErrorButton.Size = new System.Drawing.Size(130, 23);
         this.DcLinkUnderVoltageErrorButton.TabIndex = 287;
         this.DcLinkUnderVoltageErrorButton.Text = "DC Link Under Voltage";
         this.DcLinkUnderVoltageErrorButton.UseVisualStyleBackColor = true;
         this.DcLinkUnderVoltageErrorButton.Click += new System.EventHandler(this.DcLinkUnderVoltageErrorButton_Click);
         // 
         // PeakCurrentErrorButton
         // 
         this.PeakCurrentErrorButton.Location = new System.Drawing.Point(6, 35);
         this.PeakCurrentErrorButton.Name = "PeakCurrentErrorButton";
         this.PeakCurrentErrorButton.Size = new System.Drawing.Size(130, 23);
         this.PeakCurrentErrorButton.TabIndex = 286;
         this.PeakCurrentErrorButton.Text = "Peak Current Error";
         this.PeakCurrentErrorButton.UseVisualStyleBackColor = true;
         this.PeakCurrentErrorButton.Click += new System.EventHandler(this.PeakCurrentErrorButton_Click);
         // 
         // AverageCurrentErrorButton
         // 
         this.AverageCurrentErrorButton.Location = new System.Drawing.Point(6, 6);
         this.AverageCurrentErrorButton.Name = "AverageCurrentErrorButton";
         this.AverageCurrentErrorButton.Size = new System.Drawing.Size(130, 23);
         this.AverageCurrentErrorButton.TabIndex = 285;
         this.AverageCurrentErrorButton.Text = "Average Current Error";
         this.AverageCurrentErrorButton.UseVisualStyleBackColor = true;
         this.AverageCurrentErrorButton.Click += new System.EventHandler(this.AverageCurrentErrorButton_Click);
         // 
         // ProcessImageTabPage
         // 
         this.ProcessImageTabPage.Controls.Add(this.ValuePanel);
         this.ProcessImageTabPage.Location = new System.Drawing.Point(4, 22);
         this.ProcessImageTabPage.Name = "ProcessImageTabPage";
         this.ProcessImageTabPage.Padding = new System.Windows.Forms.Padding(3);
         this.ProcessImageTabPage.Size = new System.Drawing.Size(563, 104);
         this.ProcessImageTabPage.TabIndex = 1;
         this.ProcessImageTabPage.Text = "Process Image";
         this.ProcessImageTabPage.UseVisualStyleBackColor = true;
         // 
         // ValuePanel
         // 
         this.ValuePanel.AutoScroll = true;
         this.ValuePanel.BackColor = System.Drawing.Color.Gainsboro;
         this.ValuePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.ValuePanel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.ValuePanel.Location = new System.Drawing.Point(3, 3);
         this.ValuePanel.Name = "ValuePanel";
         this.ValuePanel.Size = new System.Drawing.Size(557, 98);
         this.ValuePanel.TabIndex = 0;
         this.ValuePanel.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ValuePanel_Scroll);
         // 
         // FocusTakerLabel
         // 
         this.FocusTakerLabel.AutoSize = true;
         this.FocusTakerLabel.Location = new System.Drawing.Point(303, 0);
         this.FocusTakerLabel.Name = "FocusTakerLabel";
         this.FocusTakerLabel.Size = new System.Drawing.Size(0, 13);
         this.FocusTakerLabel.TabIndex = 200;
         // 
         // BldcMotor
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.MainPanel);
         this.Name = "BldcMotor";
         this.Size = new System.Drawing.Size(575, 134);
         this.MainPanel.ResumeLayout(false);
         this.MainPanel.PerformLayout();
         this.MainTabControl.ResumeLayout(false);
         this.InterfaceTabPage.ResumeLayout(false);
         this.InterfaceTabPage.PerformLayout();
         this.EmergencyTabPage.ResumeLayout(false);
         this.ProcessImageTabPage.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Panel MainPanel;
      private System.Windows.Forms.Panel ValuePanel;
      private System.Windows.Forms.Label FormMotorTemperatureLabel;
      private System.Windows.Forms.Button SetMotorTemperatureButton;
      private System.Windows.Forms.TextBox MotorTemperatureTextBox;
      private System.Windows.Forms.Label FocusTakerLabel;
      private System.Windows.Forms.Button HomeSwitchButton;
      private System.Windows.Forms.CheckBox AutoHomEnabledCheckBox;
      private System.Windows.Forms.Button SetMotorCurrentButton;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox MotorCurrentTextBox;
      private System.Windows.Forms.Button SetInputsButton;
      private System.Windows.Forms.TextBox InputsTextBox;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TabControl MainTabControl;
      private System.Windows.Forms.TabPage InterfaceTabPage;
      private System.Windows.Forms.TabPage ProcessImageTabPage;
      private System.Windows.Forms.TabPage EmergencyTabPage;
      private System.Windows.Forms.Button ExcessTemperatureErrorButton;
      private System.Windows.Forms.Button DcLinkUnderVoltageErrorButton;
      private System.Windows.Forms.Button PeakCurrentErrorButton;
      private System.Windows.Forms.Button AverageCurrentErrorButton;
   }
}
