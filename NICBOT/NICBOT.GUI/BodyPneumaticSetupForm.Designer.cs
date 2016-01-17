namespace NICBOT.GUI
{
   partial class BodyPneumaticSetupForm
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
         this.BackButton = new System.Windows.Forms.Button();
         this.crossSectionView1 = new NICBOT.Controls.CrossSectionView();
         this.SuspendLayout();
         // 
         // BackButton
         // 
         this.BackButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.BackButton.ForeColor = System.Drawing.Color.Black;
         this.BackButton.Location = new System.Drawing.Point(419, 278);
         this.BackButton.Name = "BackButton";
         this.BackButton.Size = new System.Drawing.Size(107, 67);
         this.BackButton.TabIndex = 133;
         this.BackButton.Text = "BACK";
         this.BackButton.UseVisualStyleBackColor = false;
         this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
         // 
         // crossSectionView1
         // 
         this.crossSectionView1.Axial = false;
         this.crossSectionView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.crossSectionView1.BorderColor = System.Drawing.Color.Black;
         this.crossSectionView1.GaugeBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
         this.crossSectionView1.GaugeEdgekColor = System.Drawing.Color.Black;
         this.crossSectionView1.GaugeEdgeWeight = 1;
         this.crossSectionView1.GaugeInnerNumberSpace = 3;
         this.crossSectionView1.GaugeOuterNumberSpace = 3;
         this.crossSectionView1.Location = new System.Drawing.Point(12, 29);
         this.crossSectionView1.Name = "crossSectionView1";
         this.crossSectionView1.PitchBackColor = System.Drawing.Color.Black;
         this.crossSectionView1.PitchFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.crossSectionView1.PitchForeColor = System.Drawing.Color.White;
         this.crossSectionView1.PitchHeight = 30;
         this.crossSectionView1.PitchVisible = true;
         this.crossSectionView1.PitchWidth = 55;
         this.crossSectionView1.RobotArmColor = System.Drawing.Color.Gray;
         this.crossSectionView1.RobotBodyColor = System.Drawing.Color.DarkGray;
         this.crossSectionView1.RobotPitch = -90;
         this.crossSectionView1.RobotRoll = 60;
         this.crossSectionView1.RobotTopWheelIndicatorColor = System.Drawing.Color.Turquoise;
         this.crossSectionView1.RobotWheelColor = System.Drawing.Color.Teal;
         this.crossSectionView1.Size = new System.Drawing.Size(232, 232);
         this.crossSectionView1.TabIndex = 134;
         // 
         // BodyPneumaticSetupForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(581, 379);
         this.Controls.Add(this.crossSectionView1);
         this.Controls.Add(this.BackButton);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "BodyPneumaticSetupForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "BodyPneumaticSetupForm";
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button BackButton;
      private Controls.CrossSectionView crossSectionView1;
   }
}