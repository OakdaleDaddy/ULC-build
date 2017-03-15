namespace E4.Ui
{
   partial class MessageForm
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
         this.MainPanel = new Controls.BorderedPanel();
         this.OkButton = new System.Windows.Forms.Button();
         this.MessageLabel = new System.Windows.Forms.Label();
         this.TitleLabel = new System.Windows.Forms.Label();
         this.MainPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // MainPanel
         // 
         this.MainPanel.BackColor = System.Drawing.Color.DimGray;
         this.MainPanel.Controls.Add(this.OkButton);
         this.MainPanel.Controls.Add(this.MessageLabel);
         this.MainPanel.Controls.Add(this.TitleLabel);
         this.MainPanel.EdgeWeight = 3;
         this.MainPanel.Location = new System.Drawing.Point(0, 0);
         this.MainPanel.Name = "MainPanel";
         this.MainPanel.Size = new System.Drawing.Size(359, 255);
         this.MainPanel.TabIndex = 0;
         // 
         // OkButton
         // 
         this.OkButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.OkButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.OkButton.ForeColor = System.Drawing.Color.Black;
         this.OkButton.Location = new System.Drawing.Point(126, 172);
         this.OkButton.Name = "OkButton";
         this.OkButton.Size = new System.Drawing.Size(107, 67);
         this.OkButton.TabIndex = 146;
         this.OkButton.Text = "OK";
         this.OkButton.UseVisualStyleBackColor = false;
         this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
         // 
         // MessageLabel
         // 
         this.MessageLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.MessageLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.MessageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.MessageLabel.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
         this.MessageLabel.Location = new System.Drawing.Point(16, 68);
         this.MessageLabel.Name = "MessageLabel";
         this.MessageLabel.Size = new System.Drawing.Size(327, 88);
         this.MessageLabel.TabIndex = 133;
         this.MessageLabel.Text = "TEXT";
         this.MessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
         this.TitleLabel.TabIndex = 132;
         this.TitleLabel.Text = "TITLE";
         this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.TitleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitleLabel_MouseDown);
         this.TitleLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TitleLabel_MouseMove);
         this.TitleLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TitleLabel_MouseUp);
         // 
         // MessageForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.Black;
         this.ClientSize = new System.Drawing.Size(359, 255);
         this.Controls.Add(this.MainPanel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "MessageForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "MessageForm";
         this.Shown += new System.EventHandler(this.MessageForm_Shown);
         this.MainPanel.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private Controls.BorderedPanel MainPanel;
      private System.Windows.Forms.Label TitleLabel;
      private System.Windows.Forms.Label MessageLabel;
      private System.Windows.Forms.Button OkButton;
   }
}