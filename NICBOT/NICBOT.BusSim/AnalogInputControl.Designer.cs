namespace NICBOT.BusSim
{
   partial class AnalogInputControl
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
         this.SetButton = new System.Windows.Forms.Button();
         this.ReportTextBox = new System.Windows.Forms.TextBox();
         this.FollowCheckBox = new System.Windows.Forms.CheckBox();
         this.ValueTrackBar = new System.Windows.Forms.TrackBar();
         this.ValueEntryTextBox = new System.Windows.Forms.TextBox();
         this.DescriptorLabel = new System.Windows.Forms.Label();
         ((System.ComponentModel.ISupportInitialize)(this.ValueTrackBar)).BeginInit();
         this.SuspendLayout();
         // 
         // SetButton
         // 
         this.SetButton.Location = new System.Drawing.Point(108, 2);
         this.SetButton.Name = "SetButton";
         this.SetButton.Size = new System.Drawing.Size(32, 23);
         this.SetButton.TabIndex = 115;
         this.SetButton.Text = "set";
         this.SetButton.UseVisualStyleBackColor = true;
         this.SetButton.Click += new System.EventHandler(this.SetButton_Click);
         // 
         // ReportTextBox
         // 
         this.ReportTextBox.Enabled = false;
         this.ReportTextBox.Location = new System.Drawing.Point(146, 4);
         this.ReportTextBox.MaxLength = 3;
         this.ReportTextBox.Name = "ReportTextBox";
         this.ReportTextBox.Size = new System.Drawing.Size(33, 20);
         this.ReportTextBox.TabIndex = 114;
         this.ReportTextBox.Text = "4095";
         this.ReportTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // FollowCheckBox
         // 
         this.FollowCheckBox.AutoSize = true;
         this.FollowCheckBox.Location = new System.Drawing.Point(15, 8);
         this.FollowCheckBox.Name = "FollowCheckBox";
         this.FollowCheckBox.Size = new System.Drawing.Size(15, 14);
         this.FollowCheckBox.TabIndex = 113;
         this.FollowCheckBox.UseVisualStyleBackColor = true;
         this.FollowCheckBox.CheckedChanged += new System.EventHandler(this.FollowCheckBox_CheckedChanged);
         // 
         // ValueTrackBar
         // 
         this.ValueTrackBar.AutoSize = false;
         this.ValueTrackBar.Location = new System.Drawing.Point(3, 30);
         this.ValueTrackBar.Maximum = 4095;
         this.ValueTrackBar.Name = "ValueTrackBar";
         this.ValueTrackBar.Size = new System.Drawing.Size(176, 20);
         this.ValueTrackBar.TabIndex = 112;
         this.ValueTrackBar.Scroll += new System.EventHandler(this.ValueTrackBar_Scroll);
         // 
         // ValueEntryTextBox
         // 
         this.ValueEntryTextBox.Location = new System.Drawing.Point(69, 4);
         this.ValueEntryTextBox.MaxLength = 4;
         this.ValueEntryTextBox.Name = "ValueEntryTextBox";
         this.ValueEntryTextBox.Size = new System.Drawing.Size(33, 20);
         this.ValueEntryTextBox.TabIndex = 111;
         this.ValueEntryTextBox.Text = "4095";
         this.ValueEntryTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // DescriptorLabel
         // 
         this.DescriptorLabel.Location = new System.Drawing.Point(34, 7);
         this.DescriptorLabel.Name = "DescriptorLabel";
         this.DescriptorLabel.Size = new System.Drawing.Size(29, 13);
         this.DescriptorLabel.TabIndex = 110;
         this.DescriptorLabel.Text = "AIn0";
         this.DescriptorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // AnalogInputControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.SetButton);
         this.Controls.Add(this.ReportTextBox);
         this.Controls.Add(this.FollowCheckBox);
         this.Controls.Add(this.ValueTrackBar);
         this.Controls.Add(this.ValueEntryTextBox);
         this.Controls.Add(this.DescriptorLabel);
         this.MaximumSize = new System.Drawing.Size(190, 60);
         this.MinimumSize = new System.Drawing.Size(190, 60);
         this.Name = "AnalogInputControl";
         this.Size = new System.Drawing.Size(190, 60);
         ((System.ComponentModel.ISupportInitialize)(this.ValueTrackBar)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Button SetButton;
      private System.Windows.Forms.TextBox ReportTextBox;
      private System.Windows.Forms.CheckBox FollowCheckBox;
      private System.Windows.Forms.TrackBar ValueTrackBar;
      private System.Windows.Forms.TextBox ValueEntryTextBox;
      private System.Windows.Forms.Label DescriptorLabel;
   }
}
