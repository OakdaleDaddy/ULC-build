namespace NICBOT.BusSim
{
   partial class NicBotCameraControl
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
         this.LightLevelLabel = new System.Windows.Forms.Label();
         this.OnOffLabel = new System.Windows.Forms.Label();
         this.TitleLabel = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // LightLevelLabel
         // 
         this.LightLevelLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.LightLevelLabel.Location = new System.Drawing.Point(134, 3);
         this.LightLevelLabel.Name = "LightLevelLabel";
         this.LightLevelLabel.Size = new System.Drawing.Size(44, 23);
         this.LightLevelLabel.TabIndex = 2;
         this.LightLevelLabel.Text = "255";
         this.LightLevelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // OnOffLabel
         // 
         this.OnOffLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.OnOffLabel.Location = new System.Drawing.Point(84, 3);
         this.OnOffLabel.Name = "OnOffLabel";
         this.OnOffLabel.Size = new System.Drawing.Size(44, 23);
         this.OnOffLabel.TabIndex = 3;
         this.OnOffLabel.Text = "on";
         this.OnOffLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // TitleLabel
         // 
         this.TitleLabel.Location = new System.Drawing.Point(3, 3);
         this.TitleLabel.Name = "TitleLabel";
         this.TitleLabel.Size = new System.Drawing.Size(75, 23);
         this.TitleLabel.TabIndex = 4;
         this.TitleLabel.Text = "CAM1";
         this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // NicBotCameraControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.Controls.Add(this.TitleLabel);
         this.Controls.Add(this.OnOffLabel);
         this.Controls.Add(this.LightLevelLabel);
         this.MaximumSize = new System.Drawing.Size(181, 31);
         this.MinimumSize = new System.Drawing.Size(181, 31);
         this.Name = "NicBotCameraControl";
         this.Size = new System.Drawing.Size(181, 31);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Label LightLevelLabel;
      private System.Windows.Forms.Label OnOffLabel;
      private System.Windows.Forms.Label TitleLabel;
   }
}
