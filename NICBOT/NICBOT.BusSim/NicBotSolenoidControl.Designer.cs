namespace NICBOT.BusSim
{
   partial class NicBotSolenoidControl
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
         this.TitleLabel = new System.Windows.Forms.Label();
         this.OnOffLabel = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // TitleLabel
         // 
         this.TitleLabel.Location = new System.Drawing.Point(-3, 3);
         this.TitleLabel.Name = "TitleLabel";
         this.TitleLabel.Size = new System.Drawing.Size(141, 23);
         this.TitleLabel.TabIndex = 6;
         this.TitleLabel.Text = "WHEEL AXIAL";
         this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // OnOffLabel
         // 
         this.OnOffLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.OnOffLabel.Location = new System.Drawing.Point(134, 3);
         this.OnOffLabel.Name = "OnOffLabel";
         this.OnOffLabel.Size = new System.Drawing.Size(44, 23);
         this.OnOffLabel.TabIndex = 5;
         this.OnOffLabel.Text = "on";
         this.OnOffLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // NicBotSolenoidControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.Controls.Add(this.OnOffLabel);
         this.Controls.Add(this.TitleLabel);
         this.MaximumSize = new System.Drawing.Size(181, 31);
         this.MinimumSize = new System.Drawing.Size(181, 31);
         this.Name = "NicBotSolenoidControl";
         this.Size = new System.Drawing.Size(179, 29);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Label TitleLabel;
      private System.Windows.Forms.Label OnOffLabel;
   }
}
