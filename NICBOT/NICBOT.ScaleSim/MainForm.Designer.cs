namespace NICBOT.ScaleSim
{
   partial class MainForm
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
         this.ActivityButton = new System.Windows.Forms.Button();
         this.StreamModeRadioButton = new System.Windows.Forms.RadioButton();
         this.CommandModeRadioButton = new System.Windows.Forms.RadioButton();
         this.BaudRateComboBox = new System.Windows.Forms.ComboBox();
         this.label1 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.PortTextBox = new System.Windows.Forms.TextBox();
         this.panel1 = new System.Windows.Forms.Panel();
         this.PoundRadioButton = new System.Windows.Forms.RadioButton();
         this.OunceRadioButton = new System.Windows.Forms.RadioButton();
         this.KilogramRadioButton = new System.Windows.Forms.RadioButton();
         this.panel2 = new System.Windows.Forms.Panel();
         this.ScaleTrackBar = new System.Windows.Forms.TrackBar();
         this.SetReadingButton = new System.Windows.Forms.Button();
         this.ScaleSetPointTextBox = new System.Windows.Forms.TextBox();
         this.OutOfRangeCheckBox = new System.Windows.Forms.CheckBox();
         this.ActualReadingTextBox = new System.Windows.Forms.TextBox();
         this.CloseButton = new System.Windows.Forms.Button();
         this.MaximumScaleLabel = new System.Windows.Forms.Label();
         this.MinimumScaleLabel = new System.Windows.Forms.Label();
         this.MainStatusStrip = new System.Windows.Forms.StatusStrip();
         this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
         this.panel1.SuspendLayout();
         this.panel2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ScaleTrackBar)).BeginInit();
         this.MainStatusStrip.SuspendLayout();
         this.SuspendLayout();
         // 
         // ActivityButton
         // 
         this.ActivityButton.Location = new System.Drawing.Point(35, 220);
         this.ActivityButton.Name = "ActivityButton";
         this.ActivityButton.Size = new System.Drawing.Size(88, 23);
         this.ActivityButton.TabIndex = 4;
         this.ActivityButton.Text = "Activity";
         this.ActivityButton.UseVisualStyleBackColor = true;
         this.ActivityButton.Click += new System.EventHandler(this.ActivityButton_Click);
         // 
         // StreamModeRadioButton
         // 
         this.StreamModeRadioButton.AutoSize = true;
         this.StreamModeRadioButton.Location = new System.Drawing.Point(3, 3);
         this.StreamModeRadioButton.Name = "StreamModeRadioButton";
         this.StreamModeRadioButton.Size = new System.Drawing.Size(88, 17);
         this.StreamModeRadioButton.TabIndex = 5;
         this.StreamModeRadioButton.TabStop = true;
         this.StreamModeRadioButton.Text = "Stream Mode";
         this.StreamModeRadioButton.UseVisualStyleBackColor = true;
         this.StreamModeRadioButton.CheckedChanged += new System.EventHandler(this.StreamModeRadioButton_CheckedChanged);
         // 
         // CommandModeRadioButton
         // 
         this.CommandModeRadioButton.AutoSize = true;
         this.CommandModeRadioButton.Location = new System.Drawing.Point(3, 26);
         this.CommandModeRadioButton.Name = "CommandModeRadioButton";
         this.CommandModeRadioButton.Size = new System.Drawing.Size(102, 17);
         this.CommandModeRadioButton.TabIndex = 6;
         this.CommandModeRadioButton.TabStop = true;
         this.CommandModeRadioButton.Text = "Command Mode";
         this.CommandModeRadioButton.UseVisualStyleBackColor = true;
         this.CommandModeRadioButton.CheckedChanged += new System.EventHandler(this.CommandModeRadioButton_CheckedChanged);
         // 
         // BaudRateComboBox
         // 
         this.BaudRateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.BaudRateComboBox.FormattingEnabled = true;
         this.BaudRateComboBox.Location = new System.Drawing.Point(68, 193);
         this.BaudRateComboBox.Name = "BaudRateComboBox";
         this.BaudRateComboBox.Size = new System.Drawing.Size(55, 21);
         this.BaudRateComboBox.TabIndex = 7;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(16, 196);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(50, 13);
         this.label1.TabIndex = 8;
         this.label1.Text = "Baudrate";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(39, 170);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(26, 13);
         this.label2.TabIndex = 9;
         this.label2.Text = "Port";
         this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // PortTextBox
         // 
         this.PortTextBox.Location = new System.Drawing.Point(68, 167);
         this.PortTextBox.Name = "PortTextBox";
         this.PortTextBox.Size = new System.Drawing.Size(55, 20);
         this.PortTextBox.TabIndex = 10;
         // 
         // panel1
         // 
         this.panel1.Controls.Add(this.StreamModeRadioButton);
         this.panel1.Controls.Add(this.CommandModeRadioButton);
         this.panel1.Location = new System.Drawing.Point(12, 12);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(111, 54);
         this.panel1.TabIndex = 11;
         // 
         // PoundRadioButton
         // 
         this.PoundRadioButton.AutoSize = true;
         this.PoundRadioButton.Location = new System.Drawing.Point(3, 3);
         this.PoundRadioButton.Name = "PoundRadioButton";
         this.PoundRadioButton.Size = new System.Drawing.Size(33, 17);
         this.PoundRadioButton.TabIndex = 12;
         this.PoundRadioButton.TabStop = true;
         this.PoundRadioButton.Text = "lb";
         this.PoundRadioButton.UseVisualStyleBackColor = true;
         this.PoundRadioButton.CheckedChanged += new System.EventHandler(this.PoundRadioButton_CheckedChanged);
         // 
         // OunceRadioButton
         // 
         this.OunceRadioButton.AutoSize = true;
         this.OunceRadioButton.Location = new System.Drawing.Point(3, 26);
         this.OunceRadioButton.Name = "OunceRadioButton";
         this.OunceRadioButton.Size = new System.Drawing.Size(36, 17);
         this.OunceRadioButton.TabIndex = 13;
         this.OunceRadioButton.TabStop = true;
         this.OunceRadioButton.Text = "oz";
         this.OunceRadioButton.UseVisualStyleBackColor = true;
         this.OunceRadioButton.CheckedChanged += new System.EventHandler(this.OunceRadioButton_CheckedChanged);
         // 
         // KilogramRadioButton
         // 
         this.KilogramRadioButton.AutoSize = true;
         this.KilogramRadioButton.Location = new System.Drawing.Point(3, 49);
         this.KilogramRadioButton.Name = "KilogramRadioButton";
         this.KilogramRadioButton.Size = new System.Drawing.Size(37, 17);
         this.KilogramRadioButton.TabIndex = 14;
         this.KilogramRadioButton.TabStop = true;
         this.KilogramRadioButton.Text = "kg";
         this.KilogramRadioButton.UseVisualStyleBackColor = true;
         this.KilogramRadioButton.CheckedChanged += new System.EventHandler(this.KilogramRadioButton_CheckedChanged);
         // 
         // panel2
         // 
         this.panel2.Controls.Add(this.OunceRadioButton);
         this.panel2.Controls.Add(this.KilogramRadioButton);
         this.panel2.Controls.Add(this.PoundRadioButton);
         this.panel2.Location = new System.Drawing.Point(12, 72);
         this.panel2.Name = "panel2";
         this.panel2.Size = new System.Drawing.Size(47, 72);
         this.panel2.TabIndex = 15;
         // 
         // ScaleTrackBar
         // 
         this.ScaleTrackBar.Location = new System.Drawing.Point(166, 12);
         this.ScaleTrackBar.Name = "ScaleTrackBar";
         this.ScaleTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
         this.ScaleTrackBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.ScaleTrackBar.Size = new System.Drawing.Size(45, 278);
         this.ScaleTrackBar.TabIndex = 16;
         this.ScaleTrackBar.ValueChanged += new System.EventHandler(this.ScaleTrackBar_ValueChanged);
         // 
         // SetReadingButton
         // 
         this.SetReadingButton.Location = new System.Drawing.Point(217, 72);
         this.SetReadingButton.Name = "SetReadingButton";
         this.SetReadingButton.Size = new System.Drawing.Size(55, 23);
         this.SetReadingButton.TabIndex = 17;
         this.SetReadingButton.Text = "Set";
         this.SetReadingButton.UseVisualStyleBackColor = true;
         this.SetReadingButton.Click += new System.EventHandler(this.SetReadingButton_Click);
         // 
         // ScaleSetPointTextBox
         // 
         this.ScaleSetPointTextBox.Location = new System.Drawing.Point(217, 46);
         this.ScaleSetPointTextBox.Name = "ScaleSetPointTextBox";
         this.ScaleSetPointTextBox.Size = new System.Drawing.Size(55, 20);
         this.ScaleSetPointTextBox.TabIndex = 18;
         this.ScaleSetPointTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // OutOfRangeCheckBox
         // 
         this.OutOfRangeCheckBox.AutoSize = true;
         this.OutOfRangeCheckBox.Location = new System.Drawing.Point(70, 266);
         this.OutOfRangeCheckBox.Name = "OutOfRangeCheckBox";
         this.OutOfRangeCheckBox.Size = new System.Drawing.Size(90, 17);
         this.OutOfRangeCheckBox.TabIndex = 19;
         this.OutOfRangeCheckBox.Text = "Out of Range";
         this.OutOfRangeCheckBox.UseVisualStyleBackColor = true;
         this.OutOfRangeCheckBox.CheckedChanged += new System.EventHandler(this.OutOfRangeCheckBox_CheckedChanged);
         // 
         // ActualReadingTextBox
         // 
         this.ActualReadingTextBox.Location = new System.Drawing.Point(217, 12);
         this.ActualReadingTextBox.Name = "ActualReadingTextBox";
         this.ActualReadingTextBox.ReadOnly = true;
         this.ActualReadingTextBox.Size = new System.Drawing.Size(55, 20);
         this.ActualReadingTextBox.TabIndex = 20;
         this.ActualReadingTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // CloseButton
         // 
         this.CloseButton.Location = new System.Drawing.Point(217, 260);
         this.CloseButton.Name = "CloseButton";
         this.CloseButton.Size = new System.Drawing.Size(55, 23);
         this.CloseButton.TabIndex = 21;
         this.CloseButton.Text = "Close";
         this.CloseButton.UseVisualStyleBackColor = true;
         this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
         // 
         // MaximumScaleLabel
         // 
         this.MaximumScaleLabel.AutoSize = true;
         this.MaximumScaleLabel.Location = new System.Drawing.Point(163, 4);
         this.MaximumScaleLabel.Name = "MaximumScaleLabel";
         this.MaximumScaleLabel.Size = new System.Drawing.Size(34, 13);
         this.MaximumScaleLabel.TabIndex = 22;
         this.MaximumScaleLabel.Text = "60.00";
         this.MaximumScaleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // MinimumScaleLabel
         // 
         this.MinimumScaleLabel.AutoSize = true;
         this.MinimumScaleLabel.Location = new System.Drawing.Point(163, 286);
         this.MinimumScaleLabel.Name = "MinimumScaleLabel";
         this.MinimumScaleLabel.Size = new System.Drawing.Size(34, 13);
         this.MinimumScaleLabel.TabIndex = 23;
         this.MinimumScaleLabel.Text = "60.00";
         this.MinimumScaleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // MainStatusStrip
         // 
         this.MainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
         this.MainStatusStrip.Location = new System.Drawing.Point(0, 308);
         this.MainStatusStrip.Name = "MainStatusStrip";
         this.MainStatusStrip.Size = new System.Drawing.Size(309, 24);
         this.MainStatusStrip.TabIndex = 24;
         this.MainStatusStrip.Text = "statusStrip1";
         // 
         // StatusLabel
         // 
         this.StatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
         this.StatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
         this.StatusLabel.Name = "StatusLabel";
         this.StatusLabel.Size = new System.Drawing.Size(263, 19);
         this.StatusLabel.Spring = true;
         this.StatusLabel.Text = "toolStripStatusLabel1";
         this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // MainForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(309, 332);
         this.Controls.Add(this.MainStatusStrip);
         this.Controls.Add(this.MinimumScaleLabel);
         this.Controls.Add(this.MaximumScaleLabel);
         this.Controls.Add(this.CloseButton);
         this.Controls.Add(this.ActualReadingTextBox);
         this.Controls.Add(this.OutOfRangeCheckBox);
         this.Controls.Add(this.ScaleSetPointTextBox);
         this.Controls.Add(this.SetReadingButton);
         this.Controls.Add(this.ScaleTrackBar);
         this.Controls.Add(this.panel2);
         this.Controls.Add(this.panel1);
         this.Controls.Add(this.PortTextBox);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.BaudRateComboBox);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.ActivityButton);
         this.MaximumSize = new System.Drawing.Size(325, 370);
         this.MinimumSize = new System.Drawing.Size(325, 370);
         this.Name = "MainForm";
         this.Text = "NICBOT FG Scale Simulation";
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
         this.Shown += new System.EventHandler(this.MainForm_Shown);
         this.panel1.ResumeLayout(false);
         this.panel1.PerformLayout();
         this.panel2.ResumeLayout(false);
         this.panel2.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ScaleTrackBar)).EndInit();
         this.MainStatusStrip.ResumeLayout(false);
         this.MainStatusStrip.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Button ActivityButton;
      private System.Windows.Forms.RadioButton StreamModeRadioButton;
      private System.Windows.Forms.RadioButton CommandModeRadioButton;
      private System.Windows.Forms.ComboBox BaudRateComboBox;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TextBox PortTextBox;
      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.RadioButton PoundRadioButton;
      private System.Windows.Forms.RadioButton OunceRadioButton;
      private System.Windows.Forms.RadioButton KilogramRadioButton;
      private System.Windows.Forms.Panel panel2;
      private System.Windows.Forms.TrackBar ScaleTrackBar;
      private System.Windows.Forms.Button SetReadingButton;
      private System.Windows.Forms.TextBox ScaleSetPointTextBox;
      private System.Windows.Forms.CheckBox OutOfRangeCheckBox;
      private System.Windows.Forms.TextBox ActualReadingTextBox;
      private System.Windows.Forms.Button CloseButton;
      private System.Windows.Forms.Label MaximumScaleLabel;
      private System.Windows.Forms.Label MinimumScaleLabel;
      private System.Windows.Forms.StatusStrip MainStatusStrip;
      private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
   }
}

