namespace E4.Ui
{
   partial class IpAddressEntryForm
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
         this.ClearButton = new System.Windows.Forms.Button();
         this.DecimalButton = new System.Windows.Forms.Button();
         this.SignButton = new System.Windows.Forms.Button();
         this.DefaultButton = new E4.Ui.Controls.HoldButton();
         this.DefaultValueLabel = new System.Windows.Forms.Label();
         this.PresentValueLabel = new System.Windows.Forms.Label();
         this.EnteredValueLabel = new System.Windows.Forms.Label();
         this.TitleLabel = new System.Windows.Forms.Label();
         this.CanceledButton = new System.Windows.Forms.Button();
         this.PresentLabel = new System.Windows.Forms.Label();
         this.AcceptedButton = new System.Windows.Forms.Button();
         this.DesiredLabel = new System.Windows.Forms.Label();
         this.ZeroButton = new System.Windows.Forms.Button();
         this.SevenButton = new System.Windows.Forms.Button();
         this.DeleteButton = new System.Windows.Forms.Button();
         this.EightButton = new System.Windows.Forms.Button();
         this.ThreeButton = new System.Windows.Forms.Button();
         this.NineButton = new System.Windows.Forms.Button();
         this.TwoButton = new System.Windows.Forms.Button();
         this.FourButton = new System.Windows.Forms.Button();
         this.OneButton = new System.Windows.Forms.Button();
         this.FiveButton = new System.Windows.Forms.Button();
         this.SixButton = new System.Windows.Forms.Button();
         this.MainPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // MainPanel
         // 
         this.MainPanel.BackColor = System.Drawing.Color.DimGray;
         this.MainPanel.Controls.Add(this.ClearButton);
         this.MainPanel.Controls.Add(this.DecimalButton);
         this.MainPanel.Controls.Add(this.SignButton);
         this.MainPanel.Controls.Add(this.DefaultButton);
         this.MainPanel.Controls.Add(this.DefaultValueLabel);
         this.MainPanel.Controls.Add(this.PresentValueLabel);
         this.MainPanel.Controls.Add(this.EnteredValueLabel);
         this.MainPanel.Controls.Add(this.TitleLabel);
         this.MainPanel.Controls.Add(this.CanceledButton);
         this.MainPanel.Controls.Add(this.PresentLabel);
         this.MainPanel.Controls.Add(this.AcceptedButton);
         this.MainPanel.Controls.Add(this.DesiredLabel);
         this.MainPanel.Controls.Add(this.ZeroButton);
         this.MainPanel.Controls.Add(this.SevenButton);
         this.MainPanel.Controls.Add(this.DeleteButton);
         this.MainPanel.Controls.Add(this.EightButton);
         this.MainPanel.Controls.Add(this.ThreeButton);
         this.MainPanel.Controls.Add(this.NineButton);
         this.MainPanel.Controls.Add(this.TwoButton);
         this.MainPanel.Controls.Add(this.FourButton);
         this.MainPanel.Controls.Add(this.OneButton);
         this.MainPanel.Controls.Add(this.FiveButton);
         this.MainPanel.Controls.Add(this.SixButton);
         this.MainPanel.Dock = System.Windows.Forms.DockStyle.Left;
         this.MainPanel.EdgeWeight = 3;
         this.MainPanel.Location = new System.Drawing.Point(0, 0);
         this.MainPanel.Name = "MainPanel";
         this.MainPanel.Size = new System.Drawing.Size(359, 759);
         this.MainPanel.TabIndex = 149;
         // 
         // ClearButton
         // 
         this.ClearButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ClearButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ClearButton.ForeColor = System.Drawing.Color.Black;
         this.ClearButton.Location = new System.Drawing.Point(16, 570);
         this.ClearButton.Name = "ClearButton";
         this.ClearButton.Size = new System.Drawing.Size(107, 90);
         this.ClearButton.TabIndex = 154;
         this.ClearButton.Text = "CLR";
         this.ClearButton.UseVisualStyleBackColor = false;
         this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
         // 
         // DecimalButton
         // 
         this.DecimalButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DecimalButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DecimalButton.ForeColor = System.Drawing.Color.Black;
         this.DecimalButton.Location = new System.Drawing.Point(236, 477);
         this.DecimalButton.Name = "DecimalButton";
         this.DecimalButton.Size = new System.Drawing.Size(107, 90);
         this.DecimalButton.TabIndex = 153;
         this.DecimalButton.Text = ".";
         this.DecimalButton.UseVisualStyleBackColor = false;
         this.DecimalButton.Click += new System.EventHandler(this.DecimalButton_Click);
         // 
         // SignButton
         // 
         this.SignButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SignButton.Enabled = false;
         this.SignButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SignButton.ForeColor = System.Drawing.Color.Black;
         this.SignButton.Location = new System.Drawing.Point(236, 570);
         this.SignButton.Name = "SignButton";
         this.SignButton.Size = new System.Drawing.Size(107, 90);
         this.SignButton.TabIndex = 152;
         this.SignButton.Text = "+/-";
         this.SignButton.UseVisualStyleBackColor = false;
         // 
         // DefaultButton
         // 
         this.DefaultButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DefaultButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.DefaultButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.DefaultButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.DefaultButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.DefaultButton.HoldTimeoutEnable = true;
         this.DefaultButton.HoldTimeoutInterval = 100;
         this.DefaultButton.Location = new System.Drawing.Point(16, 107);
         this.DefaultButton.Name = "DefaultButton";
         this.DefaultButton.Size = new System.Drawing.Size(163, 36);
         this.DefaultButton.TabIndex = 151;
         this.DefaultButton.Text = "DEFAULT";
         this.DefaultButton.UseVisualStyleBackColor = false;
         this.DefaultButton.HoldTimeout += new E4.Ui.Controls.HoldTimeoutHandler(this.DefaultButton_HoldTimeout);
         // 
         // DefaultValueLabel
         // 
         this.DefaultValueLabel.BackColor = System.Drawing.Color.DimGray;
         this.DefaultValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DefaultValueLabel.ForeColor = System.Drawing.Color.DarkGray;
         this.DefaultValueLabel.Location = new System.Drawing.Point(180, 107);
         this.DefaultValueLabel.Name = "DefaultValueLabel";
         this.DefaultValueLabel.Size = new System.Drawing.Size(163, 36);
         this.DefaultValueLabel.TabIndex = 150;
         this.DefaultValueLabel.Text = "255.255.255.255";
         this.DefaultValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // PresentValueLabel
         // 
         this.PresentValueLabel.BackColor = System.Drawing.Color.Black;
         this.PresentValueLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.PresentValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.PresentValueLabel.ForeColor = System.Drawing.Color.White;
         this.PresentValueLabel.Location = new System.Drawing.Point(180, 68);
         this.PresentValueLabel.Name = "PresentValueLabel";
         this.PresentValueLabel.Size = new System.Drawing.Size(163, 36);
         this.PresentValueLabel.TabIndex = 148;
         this.PresentValueLabel.Text = "255.255.255.255";
         this.PresentValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // EnteredValueLabel
         // 
         this.EnteredValueLabel.BackColor = System.Drawing.Color.Black;
         this.EnteredValueLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.EnteredValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.EnteredValueLabel.ForeColor = System.Drawing.Color.White;
         this.EnteredValueLabel.Location = new System.Drawing.Point(180, 146);
         this.EnteredValueLabel.Name = "EnteredValueLabel";
         this.EnteredValueLabel.Size = new System.Drawing.Size(163, 36);
         this.EnteredValueLabel.TabIndex = 147;
         this.EnteredValueLabel.Text = "255.255.255.255";
         this.EnteredValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // TitleLabel
         // 
         this.TitleLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.TitleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TitleLabel.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
         this.TitleLabel.Location = new System.Drawing.Point(16, 16);
         this.TitleLabel.Name = "TitleLabel";
         this.TitleLabel.Size = new System.Drawing.Size(327, 36);
         this.TitleLabel.TabIndex = 131;
         this.TitleLabel.Text = "TITLE";
         this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.TitleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitleLabel_MouseDown);
         this.TitleLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TitleLabel_MouseMove);
         this.TitleLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TitleLabel_MouseUp);
         // 
         // CanceledButton
         // 
         this.CanceledButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.CanceledButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.CanceledButton.ForeColor = System.Drawing.Color.Black;
         this.CanceledButton.Location = new System.Drawing.Point(24, 676);
         this.CanceledButton.Name = "CanceledButton";
         this.CanceledButton.Size = new System.Drawing.Size(107, 67);
         this.CanceledButton.TabIndex = 146;
         this.CanceledButton.Text = "CANCEL";
         this.CanceledButton.UseVisualStyleBackColor = false;
         this.CanceledButton.Click += new System.EventHandler(this.CanceledButton_Click);
         // 
         // PresentLabel
         // 
         this.PresentLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.PresentLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.PresentLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.PresentLabel.ForeColor = System.Drawing.Color.Teal;
         this.PresentLabel.Location = new System.Drawing.Point(16, 68);
         this.PresentLabel.Name = "PresentLabel";
         this.PresentLabel.Size = new System.Drawing.Size(163, 36);
         this.PresentLabel.TabIndex = 132;
         this.PresentLabel.Text = "PRESENT";
         this.PresentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // AcceptedButton
         // 
         this.AcceptedButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.AcceptedButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.AcceptedButton.ForeColor = System.Drawing.Color.Black;
         this.AcceptedButton.Location = new System.Drawing.Point(228, 676);
         this.AcceptedButton.Name = "AcceptedButton";
         this.AcceptedButton.Size = new System.Drawing.Size(107, 67);
         this.AcceptedButton.TabIndex = 145;
         this.AcceptedButton.Text = "ACCEPT";
         this.AcceptedButton.UseVisualStyleBackColor = false;
         this.AcceptedButton.Click += new System.EventHandler(this.AcceptedButton_Click);
         // 
         // DesiredLabel
         // 
         this.DesiredLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.DesiredLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.DesiredLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DesiredLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
         this.DesiredLabel.Location = new System.Drawing.Point(16, 146);
         this.DesiredLabel.Name = "DesiredLabel";
         this.DesiredLabel.Size = new System.Drawing.Size(163, 36);
         this.DesiredLabel.TabIndex = 133;
         this.DesiredLabel.Text = "DESIRED";
         this.DesiredLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // ZeroButton
         // 
         this.ZeroButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ZeroButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ZeroButton.ForeColor = System.Drawing.Color.Black;
         this.ZeroButton.Location = new System.Drawing.Point(16, 477);
         this.ZeroButton.Name = "ZeroButton";
         this.ZeroButton.Size = new System.Drawing.Size(217, 90);
         this.ZeroButton.TabIndex = 144;
         this.ZeroButton.Text = "0";
         this.ZeroButton.UseVisualStyleBackColor = false;
         this.ZeroButton.Click += new System.EventHandler(this.ZeroButton_Click);
         // 
         // SevenButton
         // 
         this.SevenButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SevenButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SevenButton.ForeColor = System.Drawing.Color.Black;
         this.SevenButton.Location = new System.Drawing.Point(16, 198);
         this.SevenButton.Name = "SevenButton";
         this.SevenButton.Size = new System.Drawing.Size(107, 90);
         this.SevenButton.TabIndex = 134;
         this.SevenButton.Text = "7";
         this.SevenButton.UseVisualStyleBackColor = false;
         this.SevenButton.Click += new System.EventHandler(this.SevenButton_Click);
         // 
         // DeleteButton
         // 
         this.DeleteButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DeleteButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DeleteButton.ForeColor = System.Drawing.Color.Black;
         this.DeleteButton.Location = new System.Drawing.Point(126, 570);
         this.DeleteButton.Name = "DeleteButton";
         this.DeleteButton.Size = new System.Drawing.Size(107, 90);
         this.DeleteButton.TabIndex = 143;
         this.DeleteButton.Text = "DEL";
         this.DeleteButton.UseVisualStyleBackColor = false;
         this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
         // 
         // EightButton
         // 
         this.EightButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.EightButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.EightButton.ForeColor = System.Drawing.Color.Black;
         this.EightButton.Location = new System.Drawing.Point(126, 198);
         this.EightButton.Name = "EightButton";
         this.EightButton.Size = new System.Drawing.Size(107, 90);
         this.EightButton.TabIndex = 135;
         this.EightButton.Text = "8";
         this.EightButton.UseVisualStyleBackColor = false;
         this.EightButton.Click += new System.EventHandler(this.EightButton_Click);
         // 
         // ThreeButton
         // 
         this.ThreeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ThreeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ThreeButton.ForeColor = System.Drawing.Color.Black;
         this.ThreeButton.Location = new System.Drawing.Point(236, 384);
         this.ThreeButton.Name = "ThreeButton";
         this.ThreeButton.Size = new System.Drawing.Size(107, 90);
         this.ThreeButton.TabIndex = 142;
         this.ThreeButton.Text = "3";
         this.ThreeButton.UseVisualStyleBackColor = false;
         this.ThreeButton.Click += new System.EventHandler(this.ThreeButton_Click);
         // 
         // NineButton
         // 
         this.NineButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.NineButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.NineButton.ForeColor = System.Drawing.Color.Black;
         this.NineButton.Location = new System.Drawing.Point(236, 198);
         this.NineButton.Name = "NineButton";
         this.NineButton.Size = new System.Drawing.Size(107, 90);
         this.NineButton.TabIndex = 136;
         this.NineButton.Text = "9";
         this.NineButton.UseVisualStyleBackColor = false;
         this.NineButton.Click += new System.EventHandler(this.NineButton_Click);
         // 
         // TwoButton
         // 
         this.TwoButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.TwoButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TwoButton.ForeColor = System.Drawing.Color.Black;
         this.TwoButton.Location = new System.Drawing.Point(126, 384);
         this.TwoButton.Name = "TwoButton";
         this.TwoButton.Size = new System.Drawing.Size(107, 90);
         this.TwoButton.TabIndex = 141;
         this.TwoButton.Text = "2";
         this.TwoButton.UseVisualStyleBackColor = false;
         this.TwoButton.Click += new System.EventHandler(this.TwoButton_Click);
         // 
         // FourButton
         // 
         this.FourButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.FourButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.FourButton.ForeColor = System.Drawing.Color.Black;
         this.FourButton.Location = new System.Drawing.Point(16, 291);
         this.FourButton.Name = "FourButton";
         this.FourButton.Size = new System.Drawing.Size(107, 90);
         this.FourButton.TabIndex = 137;
         this.FourButton.Text = "4";
         this.FourButton.UseVisualStyleBackColor = false;
         this.FourButton.Click += new System.EventHandler(this.FourButton_Click);
         // 
         // OneButton
         // 
         this.OneButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.OneButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.OneButton.ForeColor = System.Drawing.Color.Black;
         this.OneButton.Location = new System.Drawing.Point(16, 384);
         this.OneButton.Name = "OneButton";
         this.OneButton.Size = new System.Drawing.Size(107, 90);
         this.OneButton.TabIndex = 140;
         this.OneButton.Text = "1";
         this.OneButton.UseVisualStyleBackColor = false;
         this.OneButton.Click += new System.EventHandler(this.OneButton_Click);
         // 
         // FiveButton
         // 
         this.FiveButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.FiveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.FiveButton.ForeColor = System.Drawing.Color.Black;
         this.FiveButton.Location = new System.Drawing.Point(126, 291);
         this.FiveButton.Name = "FiveButton";
         this.FiveButton.Size = new System.Drawing.Size(107, 90);
         this.FiveButton.TabIndex = 138;
         this.FiveButton.Text = "5";
         this.FiveButton.UseVisualStyleBackColor = false;
         this.FiveButton.Click += new System.EventHandler(this.FiveButton_Click);
         // 
         // SixButton
         // 
         this.SixButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SixButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SixButton.ForeColor = System.Drawing.Color.Black;
         this.SixButton.Location = new System.Drawing.Point(236, 291);
         this.SixButton.Name = "SixButton";
         this.SixButton.Size = new System.Drawing.Size(107, 90);
         this.SixButton.TabIndex = 139;
         this.SixButton.Text = "6";
         this.SixButton.UseVisualStyleBackColor = false;
         this.SixButton.Click += new System.EventHandler(this.SixButton_Click);
         // 
         // IpAddressEntryForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(359, 759);
         this.Controls.Add(this.MainPanel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "IpAddressEntryForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "IpAddressEntryForm";
         this.Shown += new System.EventHandler(this.IpAddressEntryForm_Shown);
         this.MainPanel.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private Controls.BorderedPanel MainPanel;
      private System.Windows.Forms.Button DecimalButton;
      private System.Windows.Forms.Button SignButton;
      private Controls.HoldButton DefaultButton;
      private System.Windows.Forms.Label DefaultValueLabel;
      private System.Windows.Forms.Label PresentValueLabel;
      private System.Windows.Forms.Label EnteredValueLabel;
      private System.Windows.Forms.Label TitleLabel;
      private System.Windows.Forms.Button CanceledButton;
      private System.Windows.Forms.Label PresentLabel;
      private System.Windows.Forms.Button AcceptedButton;
      private System.Windows.Forms.Label DesiredLabel;
      private System.Windows.Forms.Button ZeroButton;
      private System.Windows.Forms.Button SevenButton;
      private System.Windows.Forms.Button DeleteButton;
      private System.Windows.Forms.Button EightButton;
      private System.Windows.Forms.Button ThreeButton;
      private System.Windows.Forms.Button NineButton;
      private System.Windows.Forms.Button TwoButton;
      private System.Windows.Forms.Button FourButton;
      private System.Windows.Forms.Button OneButton;
      private System.Windows.Forms.Button FiveButton;
      private System.Windows.Forms.Button SixButton;
      private System.Windows.Forms.Button ClearButton;
   }
}