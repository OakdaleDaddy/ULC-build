namespace Weco.Ui
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
         this.ThirdButton = new System.Windows.Forms.Button();
         this.SecondButton = new System.Windows.Forms.Button();
         this.FirstButton = new System.Windows.Forms.Button();
         this.MessageLabel = new System.Windows.Forms.Label();
         this.TitleLabel = new System.Windows.Forms.Label();
         this.MainPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // MainPanel
         // 
         this.MainPanel.BackColor = System.Drawing.Color.DimGray;
         this.MainPanel.Controls.Add(this.ThirdButton);
         this.MainPanel.Controls.Add(this.SecondButton);
         this.MainPanel.Controls.Add(this.FirstButton);
         this.MainPanel.Controls.Add(this.MessageLabel);
         this.MainPanel.Controls.Add(this.TitleLabel);
         this.MainPanel.EdgeWeight = 3;
         this.MainPanel.Location = new System.Drawing.Point(0, 0);
         this.MainPanel.Name = "MainPanel";
         this.MainPanel.Size = new System.Drawing.Size(391, 255);
         this.MainPanel.TabIndex = 0;
         // 
         // ThirdButton
         // 
         this.ThirdButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ThirdButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ThirdButton.ForeColor = System.Drawing.Color.Black;
         this.ThirdButton.Location = new System.Drawing.Point(257, 172);
         this.ThirdButton.Name = "ThirdButton";
         this.ThirdButton.Size = new System.Drawing.Size(107, 67);
         this.ThirdButton.TabIndex = 148;
         this.ThirdButton.Text = "Third";
         this.ThirdButton.UseVisualStyleBackColor = false;
         this.ThirdButton.Click += new System.EventHandler(this.ThirdButton_Click);
         // 
         // SecondButton
         // 
         this.SecondButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SecondButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SecondButton.ForeColor = System.Drawing.Color.Black;
         this.SecondButton.Location = new System.Drawing.Point(142, 172);
         this.SecondButton.Name = "SecondButton";
         this.SecondButton.Size = new System.Drawing.Size(107, 67);
         this.SecondButton.TabIndex = 147;
         this.SecondButton.Text = "Second";
         this.SecondButton.UseVisualStyleBackColor = false;
         this.SecondButton.Click += new System.EventHandler(this.SecondButton_Click);
         // 
         // FirstButton
         // 
         this.FirstButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.FirstButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.FirstButton.ForeColor = System.Drawing.Color.Black;
         this.FirstButton.Location = new System.Drawing.Point(27, 172);
         this.FirstButton.Name = "FirstButton";
         this.FirstButton.Size = new System.Drawing.Size(107, 67);
         this.FirstButton.TabIndex = 146;
         this.FirstButton.Text = "First";
         this.FirstButton.UseVisualStyleBackColor = false;
         this.FirstButton.Click += new System.EventHandler(this.FirstButton_Click);
         // 
         // MessageLabel
         // 
         this.MessageLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.MessageLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.MessageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.MessageLabel.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
         this.MessageLabel.Location = new System.Drawing.Point(16, 68);
         this.MessageLabel.Name = "MessageLabel";
         this.MessageLabel.Size = new System.Drawing.Size(359, 88);
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
         this.TitleLabel.Size = new System.Drawing.Size(359, 36);
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
         this.ClientSize = new System.Drawing.Size(391, 255);
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
      private System.Windows.Forms.Button FirstButton;
      private System.Windows.Forms.Button SecondButton;
      private System.Windows.Forms.Button ThirdButton;
   }
}