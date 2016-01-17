namespace NICBOT.BusSim
{
   partial class NicbotBodyServoUserControl
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
         this.label1 = new System.Windows.Forms.Label();
         this.TargetTextBox = new System.Windows.Forms.TextBox();
         this.ActualTextBox = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.ErrorCheckBox = new System.Windows.Forms.CheckBox();
         this.label3 = new System.Windows.Forms.Label();
         this.AccelerationTextBox = new System.Windows.Forms.TextBox();
         this.label4 = new System.Windows.Forms.Label();
         this.VelocityTextBox = new System.Windows.Forms.TextBox();
         this.NameLabel = new System.Windows.Forms.Label();
         this.PositionTextBox = new System.Windows.Forms.TextBox();
         this.label5 = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(422, 6);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(14, 13);
         this.label1.TabIndex = 0;
         this.label1.Text = "T";
         // 
         // TargetTextBox
         // 
         this.TargetTextBox.Location = new System.Drawing.Point(438, 3);
         this.TargetTextBox.Name = "TargetTextBox";
         this.TargetTextBox.Size = new System.Drawing.Size(71, 20);
         this.TargetTextBox.TabIndex = 1;
         this.TargetTextBox.Text = "4294967295";
         // 
         // ActualTextBox
         // 
         this.ActualTextBox.Location = new System.Drawing.Point(531, 3);
         this.ActualTextBox.Name = "ActualTextBox";
         this.ActualTextBox.Size = new System.Drawing.Size(71, 20);
         this.ActualTextBox.TabIndex = 3;
         this.ActualTextBox.Text = "4294967295";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(515, 6);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(14, 13);
         this.label2.TabIndex = 2;
         this.label2.Text = "A";
         // 
         // ErrorCheckBox
         // 
         this.ErrorCheckBox.AutoSize = true;
         this.ErrorCheckBox.Location = new System.Drawing.Point(608, 5);
         this.ErrorCheckBox.Name = "ErrorCheckBox";
         this.ErrorCheckBox.Size = new System.Drawing.Size(47, 17);
         this.ErrorCheckBox.TabIndex = 4;
         this.ErrorCheckBox.Text = "error";
         this.ErrorCheckBox.UseVisualStyleBackColor = true;
         this.ErrorCheckBox.CheckedChanged += new System.EventHandler(this.ErrorCheckBox_CheckedChanged);
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(100, 6);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(28, 13);
         this.label3.TabIndex = 5;
         this.label3.Text = "ACC";
         // 
         // AccelerationTextBox
         // 
         this.AccelerationTextBox.Location = new System.Drawing.Point(131, 3);
         this.AccelerationTextBox.Name = "AccelerationTextBox";
         this.AccelerationTextBox.Size = new System.Drawing.Size(71, 20);
         this.AccelerationTextBox.TabIndex = 6;
         this.AccelerationTextBox.Text = "4294967295";
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(208, 6);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(27, 13);
         this.label4.TabIndex = 7;
         this.label4.Text = "VEL";
         // 
         // VelocityTextBox
         // 
         this.VelocityTextBox.Location = new System.Drawing.Point(237, 3);
         this.VelocityTextBox.Name = "VelocityTextBox";
         this.VelocityTextBox.Size = new System.Drawing.Size(71, 20);
         this.VelocityTextBox.TabIndex = 8;
         this.VelocityTextBox.Text = "4294967295";
         // 
         // NameLabel
         // 
         this.NameLabel.Location = new System.Drawing.Point(3, 6);
         this.NameLabel.Name = "NameLabel";
         this.NameLabel.Size = new System.Drawing.Size(91, 13);
         this.NameLabel.TabIndex = 9;
         this.NameLabel.Text = "NAME";
         this.NameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // PositionTextBox
         // 
         this.PositionTextBox.Location = new System.Drawing.Point(345, 3);
         this.PositionTextBox.Name = "PositionTextBox";
         this.PositionTextBox.Size = new System.Drawing.Size(71, 20);
         this.PositionTextBox.TabIndex = 11;
         this.PositionTextBox.Text = "4294967295";
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(314, 6);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(29, 13);
         this.label5.TabIndex = 10;
         this.label5.Text = "POS";
         // 
         // NicbotBodyServoUserControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.Controls.Add(this.PositionTextBox);
         this.Controls.Add(this.label5);
         this.Controls.Add(this.NameLabel);
         this.Controls.Add(this.VelocityTextBox);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.AccelerationTextBox);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.ErrorCheckBox);
         this.Controls.Add(this.ActualTextBox);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.TargetTextBox);
         this.Controls.Add(this.label1);
         this.Name = "NicbotBodyServoUserControl";
         this.Size = new System.Drawing.Size(668, 26);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox TargetTextBox;
      private System.Windows.Forms.TextBox ActualTextBox;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.CheckBox ErrorCheckBox;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.TextBox AccelerationTextBox;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.TextBox VelocityTextBox;
      private System.Windows.Forms.Label NameLabel;
      private System.Windows.Forms.TextBox PositionTextBox;
      private System.Windows.Forms.Label label5;
   }
}
