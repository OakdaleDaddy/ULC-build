namespace Weco.TraceListener
{
   partial class MainForm
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
         this.LogRichTextBox = new System.Windows.Forms.RichTextBox();
         this.CloseButton = new System.Windows.Forms.Button();
         this.ActivityButton = new System.Windows.Forms.Button();
         this.PortTextBox = new System.Windows.Forms.TextBox();
         this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
         this.ClearButton = new System.Windows.Forms.Button();
         this.ScrollToCursorCheckBox = new System.Windows.Forms.CheckBox();
         this.SuspendLayout();
         // 
         // LogRichTextBox
         // 
         this.LogRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.LogRichTextBox.Location = new System.Drawing.Point(2, 2);
         this.LogRichTextBox.Name = "LogRichTextBox";
         this.LogRichTextBox.Size = new System.Drawing.Size(579, 228);
         this.LogRichTextBox.TabIndex = 0;
         this.LogRichTextBox.Text = "";
         this.LogRichTextBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LogRichTextBox_MouseDown);
         // 
         // CloseButton
         // 
         this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.CloseButton.Location = new System.Drawing.Point(484, 234);
         this.CloseButton.Name = "CloseButton";
         this.CloseButton.Size = new System.Drawing.Size(75, 23);
         this.CloseButton.TabIndex = 1;
         this.CloseButton.Text = "Close";
         this.CloseButton.UseVisualStyleBackColor = true;
         this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
         // 
         // ActivityButton
         // 
         this.ActivityButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.ActivityButton.Location = new System.Drawing.Point(118, 234);
         this.ActivityButton.Name = "ActivityButton";
         this.ActivityButton.Size = new System.Drawing.Size(75, 23);
         this.ActivityButton.TabIndex = 2;
         this.ActivityButton.Text = "Activity";
         this.ActivityButton.UseVisualStyleBackColor = true;
         this.ActivityButton.Click += new System.EventHandler(this.ActivityButton_Click);
         // 
         // PortTextBox
         // 
         this.PortTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.PortTextBox.Location = new System.Drawing.Point(12, 236);
         this.PortTextBox.Name = "PortTextBox";
         this.PortTextBox.Size = new System.Drawing.Size(100, 20);
         this.PortTextBox.TabIndex = 3;
         this.PortTextBox.Text = "10000";
         // 
         // UpdateTimer
         // 
         this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
         // 
         // ClearButton
         // 
         this.ClearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.ClearButton.Location = new System.Drawing.Point(286, 233);
         this.ClearButton.Name = "ClearButton";
         this.ClearButton.Size = new System.Drawing.Size(75, 23);
         this.ClearButton.TabIndex = 4;
         this.ClearButton.Text = "Clear";
         this.ClearButton.UseVisualStyleBackColor = true;
         this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
         // 
         // ScrollToCursorCheckBox
         // 
         this.ScrollToCursorCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.ScrollToCursorCheckBox.AutoSize = true;
         this.ScrollToCursorCheckBox.Location = new System.Drawing.Point(367, 237);
         this.ScrollToCursorCheckBox.Name = "ScrollToCursorCheckBox";
         this.ScrollToCursorCheckBox.Size = new System.Drawing.Size(77, 17);
         this.ScrollToCursorCheckBox.TabIndex = 5;
         this.ScrollToCursorCheckBox.Text = "Auto Scroll";
         this.ScrollToCursorCheckBox.UseVisualStyleBackColor = true;
         // 
         // MainForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(592, 273);
         this.Controls.Add(this.ScrollToCursorCheckBox);
         this.Controls.Add(this.ClearButton);
         this.Controls.Add(this.PortTextBox);
         this.Controls.Add(this.ActivityButton);
         this.Controls.Add(this.CloseButton);
         this.Controls.Add(this.LogRichTextBox);
         this.MinimumSize = new System.Drawing.Size(600, 300);
         this.Name = "MainForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "Weco Trace Listener";
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
         this.Shown += new System.EventHandler(this.MainForm_Shown);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.RichTextBox LogRichTextBox;
      private System.Windows.Forms.Button CloseButton;
      private System.Windows.Forms.Button ActivityButton;
      private System.Windows.Forms.TextBox PortTextBox;
      private System.Windows.Forms.Timer UpdateTimer;
      private System.Windows.Forms.Button ClearButton;
      private System.Windows.Forms.CheckBox ScrollToCursorCheckBox;
   }
}

