namespace Weco.Ui
{
   partial class DeviceComponentInformationForm
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
         this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
         this.MainPanel = new Controls.BorderedPanel();
         this.ErrorIndexLabel = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.ClearFaultButton = new Controls.BaseButton();
         this.ClearErrorButton = new Controls.BaseButton();
         this.ErrorTextBox = new System.Windows.Forms.TextBox();
         this.ErrorUpButton = new Controls.UpDownButton();
         this.ErrorDownButton = new Controls.UpDownButton();
         this.FaultTextBox = new System.Windows.Forms.TextBox();
         this.TitleLabel = new System.Windows.Forms.Label();
         this.BackButton = new Controls.BaseButton();
         this.MainPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // UpdateTimer
         // 
         this.UpdateTimer.Interval = 50;
         this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
         // 
         // MainPanel
         // 
         this.MainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
         this.MainPanel.Controls.Add(this.ErrorIndexLabel);
         this.MainPanel.Controls.Add(this.label2);
         this.MainPanel.Controls.Add(this.label1);
         this.MainPanel.Controls.Add(this.ClearFaultButton);
         this.MainPanel.Controls.Add(this.ClearErrorButton);
         this.MainPanel.Controls.Add(this.ErrorTextBox);
         this.MainPanel.Controls.Add(this.ErrorUpButton);
         this.MainPanel.Controls.Add(this.ErrorDownButton);
         this.MainPanel.Controls.Add(this.FaultTextBox);
         this.MainPanel.Controls.Add(this.TitleLabel);
         this.MainPanel.Controls.Add(this.BackButton);
         this.MainPanel.EdgeWeight = 3;
         this.MainPanel.Location = new System.Drawing.Point(0, 0);
         this.MainPanel.Name = "MainPanel";
         this.MainPanel.Size = new System.Drawing.Size(564, 312);
         this.MainPanel.TabIndex = 0;
         // 
         // ErrorIndexLabel
         // 
         this.ErrorIndexLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ErrorIndexLabel.ForeColor = System.Drawing.Color.Yellow;
         this.ErrorIndexLabel.Location = new System.Drawing.Point(478, 105);
         this.ErrorIndexLabel.Name = "ErrorIndexLabel";
         this.ErrorIndexLabel.Size = new System.Drawing.Size(58, 20);
         this.ErrorIndexLabel.TabIndex = 191;
         this.ErrorIndexLabel.Text = "(1/2)";
         this.ErrorIndexLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // label2
         // 
         this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label2.ForeColor = System.Drawing.Color.Gainsboro;
         this.label2.Location = new System.Drawing.Point(13, 71);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(73, 20);
         this.label2.TabIndex = 190;
         this.label2.Text = "FAULT";
         this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // label1
         // 
         this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label1.ForeColor = System.Drawing.Color.Gainsboro;
         this.label1.Location = new System.Drawing.Point(13, 105);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(73, 20);
         this.label1.TabIndex = 189;
         this.label1.Text = "ERRORS";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // ClearFaultButton
         // 
         this.ClearFaultButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ClearFaultButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ClearFaultButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ClearFaultButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.ClearFaultButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.ClearFaultButton.Location = new System.Drawing.Point(86, 144);
         this.ClearFaultButton.Name = "ClearFaultButton";
         this.ClearFaultButton.Size = new System.Drawing.Size(107, 67);
         this.ClearFaultButton.TabIndex = 188;
         this.ClearFaultButton.Text = "CLEAR FAULT";
         this.ClearFaultButton.UseVisualStyleBackColor = false;
         this.ClearFaultButton.Click += new System.EventHandler(this.ClearFaultButton_Click);
         // 
         // ClearErrorButton
         // 
         this.ClearErrorButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ClearErrorButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ClearErrorButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ClearErrorButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.ClearErrorButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.ClearErrorButton.Location = new System.Drawing.Point(209, 144);
         this.ClearErrorButton.Name = "ClearErrorButton";
         this.ClearErrorButton.Size = new System.Drawing.Size(107, 67);
         this.ClearErrorButton.TabIndex = 187;
         this.ClearErrorButton.Text = "CLEAR ERROR";
         this.ClearErrorButton.UseVisualStyleBackColor = false;
         this.ClearErrorButton.Click += new System.EventHandler(this.ClearErrorButton_Click);
         // 
         // ErrorTextBox
         // 
         this.ErrorTextBox.BackColor = System.Drawing.Color.Yellow;
         this.ErrorTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.ErrorTextBox.ForeColor = System.Drawing.Color.Black;
         this.ErrorTextBox.Location = new System.Drawing.Point(92, 102);
         this.ErrorTextBox.Name = "ErrorTextBox";
         this.ErrorTextBox.ReadOnly = true;
         this.ErrorTextBox.Size = new System.Drawing.Size(380, 26);
         this.ErrorTextBox.TabIndex = 186;
         this.ErrorTextBox.Text = "not connected";
         this.ErrorTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // ErrorUpButton
         // 
         this.ErrorUpButton.ArrowColor = System.Drawing.Color.Black;
         this.ErrorUpButton.ArrowHighlightColor = System.Drawing.Color.DarkGray;
         this.ErrorUpButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ErrorUpButton.DisabledArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.ErrorUpButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ErrorUpButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ErrorUpButton.EdgeSpace = 8;
         this.ErrorUpButton.ForeColor = System.Drawing.SystemColors.HighlightText;
         this.ErrorUpButton.HighLightOffset = 7;
         this.ErrorUpButton.HighlightVisible = true;
         this.ErrorUpButton.HighLightWeight = 2;
         this.ErrorUpButton.HoldArrorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.ErrorUpButton.HoldRepeat = true;
         this.ErrorUpButton.HoldRepeatInterval = 100;
         this.ErrorUpButton.HoldTimeoutInterval = 100;
         this.ErrorUpButton.Location = new System.Drawing.Point(332, 144);
         this.ErrorUpButton.Name = "ErrorUpButton";
         this.ErrorUpButton.Size = new System.Drawing.Size(69, 69);
         this.ErrorUpButton.TabIndex = 184;
         this.ErrorUpButton.Text = "upDownButton4";
         this.ErrorUpButton.TextOffset = 0;
         this.ErrorUpButton.TextVisible = false;
         this.ErrorUpButton.UpDown = true;
         this.ErrorUpButton.UseVisualStyleBackColor = false;
         this.ErrorUpButton.Click += new System.EventHandler(this.ErrorUpButton_Click);
         // 
         // ErrorDownButton
         // 
         this.ErrorDownButton.ArrowColor = System.Drawing.Color.Black;
         this.ErrorDownButton.ArrowHighlightColor = System.Drawing.Color.DarkGray;
         this.ErrorDownButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ErrorDownButton.DisabledArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.ErrorDownButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ErrorDownButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ErrorDownButton.EdgeSpace = 8;
         this.ErrorDownButton.ForeColor = System.Drawing.SystemColors.HighlightText;
         this.ErrorDownButton.HighLightOffset = 7;
         this.ErrorDownButton.HighlightVisible = true;
         this.ErrorDownButton.HighLightWeight = 2;
         this.ErrorDownButton.HoldArrorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.ErrorDownButton.HoldRepeat = true;
         this.ErrorDownButton.HoldRepeatInterval = 100;
         this.ErrorDownButton.HoldTimeoutInterval = 100;
         this.ErrorDownButton.Location = new System.Drawing.Point(409, 144);
         this.ErrorDownButton.Name = "ErrorDownButton";
         this.ErrorDownButton.Size = new System.Drawing.Size(69, 69);
         this.ErrorDownButton.TabIndex = 185;
         this.ErrorDownButton.Text = "upDownButton5";
         this.ErrorDownButton.TextOffset = 0;
         this.ErrorDownButton.TextVisible = false;
         this.ErrorDownButton.UpDown = false;
         this.ErrorDownButton.UseVisualStyleBackColor = false;
         this.ErrorDownButton.Click += new System.EventHandler(this.ErrorDownButton_Click);
         // 
         // FaultTextBox
         // 
         this.FaultTextBox.BackColor = System.Drawing.Color.Red;
         this.FaultTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.FaultTextBox.ForeColor = System.Drawing.Color.Black;
         this.FaultTextBox.Location = new System.Drawing.Point(92, 68);
         this.FaultTextBox.Name = "FaultTextBox";
         this.FaultTextBox.ReadOnly = true;
         this.FaultTextBox.Size = new System.Drawing.Size(380, 26);
         this.FaultTextBox.TabIndex = 183;
         this.FaultTextBox.Text = "not connected";
         this.FaultTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // TitleLabel
         // 
         this.TitleLabel.BackColor = System.Drawing.Color.Teal;
         this.TitleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TitleLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.TitleLabel.Location = new System.Drawing.Point(16, 16);
         this.TitleLabel.Name = "TitleLabel";
         this.TitleLabel.Size = new System.Drawing.Size(532, 36);
         this.TitleLabel.TabIndex = 169;
         this.TitleLabel.Text = "TITLE";
         this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.TitleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitleLabel_MouseDown);
         this.TitleLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TitleLabel_MouseMove);
         this.TitleLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TitleLabel_MouseUp);
         // 
         // BackButton
         // 
         this.BackButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BackButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BackButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.BackButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.BackButton.Location = new System.Drawing.Point(430, 229);
         this.BackButton.Name = "BackButton";
         this.BackButton.Size = new System.Drawing.Size(107, 67);
         this.BackButton.TabIndex = 167;
         this.BackButton.Text = "BACK";
         this.BackButton.UseVisualStyleBackColor = false;
         this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
         // 
         // DeviceComponentInformationForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.ClientSize = new System.Drawing.Size(564, 312);
         this.Controls.Add(this.MainPanel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "DeviceComponentInformationForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "CANDeviceInformationForm";
         this.Shown += new System.EventHandler(this.CANDeviceInformationForm_Shown);
         this.MainPanel.ResumeLayout(false);
         this.MainPanel.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private Weco.Ui.Controls.BorderedPanel MainPanel;
      private Weco.Ui.Controls.BaseButton BackButton;
      private System.Windows.Forms.Label TitleLabel;
      private System.Windows.Forms.TextBox FaultTextBox;
      private System.Windows.Forms.Timer UpdateTimer;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label1;
      private Controls.BaseButton ClearFaultButton;
      private Controls.BaseButton ClearErrorButton;
      private System.Windows.Forms.TextBox ErrorTextBox;
      private Controls.UpDownButton ErrorUpButton;
      private Controls.UpDownButton ErrorDownButton;
      private System.Windows.Forms.Label ErrorIndexLabel;
   }
}