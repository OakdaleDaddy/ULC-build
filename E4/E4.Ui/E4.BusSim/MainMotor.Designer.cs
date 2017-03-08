namespace E4.BusSim
{
   partial class MainMotor
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
         this.HomeSwitchButton = new System.Windows.Forms.Button();
         this.FocusTakerLabel = new System.Windows.Forms.Label();
         this.SetMotorTemperatureButton = new System.Windows.Forms.Button();
         this.MotorTemperatureTextBox = new System.Windows.Forms.TextBox();
         this.FormMotorTemperatureLabel = new System.Windows.Forms.Label();
         this.ValuePanel = new System.Windows.Forms.Panel();
         this.MainPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // MainPanel
         // 
         this.MainPanel.AutoScroll = true;
         this.MainPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.MainPanel.Controls.Add(this.HomeSwitchButton);
         this.MainPanel.Controls.Add(this.FocusTakerLabel);
         this.MainPanel.Controls.Add(this.SetMotorTemperatureButton);
         this.MainPanel.Controls.Add(this.MotorTemperatureTextBox);
         this.MainPanel.Controls.Add(this.FormMotorTemperatureLabel);
         this.MainPanel.Controls.Add(this.ValuePanel);
         this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.MainPanel.Location = new System.Drawing.Point(0, 0);
         this.MainPanel.Name = "MainPanel";
         this.MainPanel.Size = new System.Drawing.Size(575, 91);
         this.MainPanel.TabIndex = 0;
         this.MainPanel.Scroll += new System.Windows.Forms.ScrollEventHandler(this.MainPanel_Scroll);
         // 
         // HomeSwitchButton
         // 
         this.HomeSwitchButton.Location = new System.Drawing.Point(198, 3);
         this.HomeSwitchButton.Name = "HomeSwitchButton";
         this.HomeSwitchButton.Size = new System.Drawing.Size(85, 23);
         this.HomeSwitchButton.TabIndex = 201;
         this.HomeSwitchButton.Text = "Home Switch";
         this.HomeSwitchButton.UseVisualStyleBackColor = true;
         this.HomeSwitchButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HomeSwitchButton_MouseDown);
         this.HomeSwitchButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.HomeSwitchButton_MouseUp);
         // 
         // FocusTakerLabel
         // 
         this.FocusTakerLabel.AutoSize = true;
         this.FocusTakerLabel.Location = new System.Drawing.Point(303, 0);
         this.FocusTakerLabel.Name = "FocusTakerLabel";
         this.FocusTakerLabel.Size = new System.Drawing.Size(0, 13);
         this.FocusTakerLabel.TabIndex = 200;
         // 
         // SetMotorTemperatureButton
         // 
         this.SetMotorTemperatureButton.Location = new System.Drawing.Point(136, 3);
         this.SetMotorTemperatureButton.Name = "SetMotorTemperatureButton";
         this.SetMotorTemperatureButton.Size = new System.Drawing.Size(35, 23);
         this.SetMotorTemperatureButton.TabIndex = 199;
         this.SetMotorTemperatureButton.Text = "Set";
         this.SetMotorTemperatureButton.UseVisualStyleBackColor = true;
         this.SetMotorTemperatureButton.Click += new System.EventHandler(this.SetMotorTemperatureButton_Click);
         // 
         // MotorTemperatureTextBox
         // 
         this.MotorTemperatureTextBox.Location = new System.Drawing.Point(105, 5);
         this.MotorTemperatureTextBox.MaxLength = 0;
         this.MotorTemperatureTextBox.Name = "MotorTemperatureTextBox";
         this.MotorTemperatureTextBox.Size = new System.Drawing.Size(25, 20);
         this.MotorTemperatureTextBox.TabIndex = 198;
         this.MotorTemperatureTextBox.Text = "23";
         this.MotorTemperatureTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // FormMotorTemperatureLabel
         // 
         this.FormMotorTemperatureLabel.AutoSize = true;
         this.FormMotorTemperatureLabel.Location = new System.Drawing.Point(6, 8);
         this.FormMotorTemperatureLabel.Name = "FormMotorTemperatureLabel";
         this.FormMotorTemperatureLabel.Size = new System.Drawing.Size(97, 13);
         this.FormMotorTemperatureLabel.TabIndex = 1;
         this.FormMotorTemperatureLabel.Text = "Motor Temperature";
         // 
         // ValuePanel
         // 
         this.ValuePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.ValuePanel.AutoScroll = true;
         this.ValuePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.ValuePanel.Location = new System.Drawing.Point(0, 32);
         this.ValuePanel.Name = "ValuePanel";
         this.ValuePanel.Size = new System.Drawing.Size(570, 52);
         this.ValuePanel.TabIndex = 0;
         this.ValuePanel.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ValuePanel_Scroll);
         // 
         // MainMotor
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.MainPanel);
         this.Name = "MainMotor";
         this.Size = new System.Drawing.Size(575, 91);
         this.MainPanel.ResumeLayout(false);
         this.MainPanel.PerformLayout();
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
   }
}
