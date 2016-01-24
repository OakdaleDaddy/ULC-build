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
         this.FeederOffButton = new NICBOT.GUI.HoldButton();
         this.button1 = new System.Windows.Forms.Button();
         this.button2 = new System.Windows.Forms.Button();
         this.button3 = new System.Windows.Forms.Button();
         this.button4 = new System.Windows.Forms.Button();
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
         // FeederOffButton
         // 
         this.FeederOffButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.FeederOffButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.FeederOffButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.FeederOffButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.FeederOffButton.HoldTimeoutEnable = true;
         this.FeederOffButton.HoldTimeoutInterval = 100;
         this.FeederOffButton.Location = new System.Drawing.Point(347, 110);
         this.FeederOffButton.Name = "FeederOffButton";
         this.FeederOffButton.Size = new System.Drawing.Size(107, 90);
         this.FeederOffButton.TabIndex = 152;
         this.FeederOffButton.Text = "OFF   (FREE)";
         this.FeederOffButton.UseVisualStyleBackColor = false;
         // 
         // button1
         // 
         this.button1.BackColor = System.Drawing.Color.LightGray;
         this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.button1.ForeColor = System.Drawing.Color.Black;
         this.button1.Location = new System.Drawing.Point(308, 206);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(72, 67);
         this.button1.TabIndex = 153;
         this.button1.Text = "BACK";
         this.button1.UseVisualStyleBackColor = false;
         // 
         // button2
         // 
         this.button2.BackColor = System.Drawing.Color.DarkGray;
         this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.button2.ForeColor = System.Drawing.Color.Black;
         this.button2.Location = new System.Drawing.Point(386, 206);
         this.button2.Name = "button2";
         this.button2.Size = new System.Drawing.Size(72, 67);
         this.button2.TabIndex = 154;
         this.button2.Text = "BACK";
         this.button2.UseVisualStyleBackColor = false;
         // 
         // button3
         // 
         this.button3.BackColor = System.Drawing.Color.Gray;
         this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.button3.ForeColor = System.Drawing.Color.Black;
         this.button3.Location = new System.Drawing.Point(464, 205);
         this.button3.Name = "button3";
         this.button3.Size = new System.Drawing.Size(72, 67);
         this.button3.TabIndex = 155;
         this.button3.Text = "BACK";
         this.button3.UseVisualStyleBackColor = false;
         // 
         // button4
         // 
         this.button4.BackColor = System.Drawing.Color.Silver;
         this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.button4.ForeColor = System.Drawing.Color.Black;
         this.button4.Location = new System.Drawing.Point(464, 133);
         this.button4.Name = "button4";
         this.button4.Size = new System.Drawing.Size(72, 67);
         this.button4.TabIndex = 156;
         this.button4.Text = "BACK";
         this.button4.UseVisualStyleBackColor = false;
         // 
         // BodyPneumaticSetupForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.Olive;
         this.ClientSize = new System.Drawing.Size(581, 379);
         this.Controls.Add(this.button4);
         this.Controls.Add(this.button3);
         this.Controls.Add(this.button2);
         this.Controls.Add(this.button1);
         this.Controls.Add(this.FeederOffButton);
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
      private HoldButton FeederOffButton;
      private System.Windows.Forms.Button button1;
      private System.Windows.Forms.Button button2;
      private System.Windows.Forms.Button button3;
      private System.Windows.Forms.Button button4;
   }
}