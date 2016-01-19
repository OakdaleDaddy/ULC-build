namespace NICBOT.OSD
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
         this.ActivityButton = new System.Windows.Forms.Button();
         this.MainStatusStrip = new System.Windows.Forms.StatusStrip();
         this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
         this.TimeStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
         this.PortTextBox = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.UpButton = new System.Windows.Forms.Button();
         this.DownButton = new System.Windows.Forms.Button();
         this.LeftButton = new System.Windows.Forms.Button();
         this.RightButton = new System.Windows.Forms.Button();
         this.BaudTextBox = new System.Windows.Forms.TextBox();
         this.HorizontalOffsetTextBox = new System.Windows.Forms.TextBox();
         this.label3 = new System.Windows.Forms.Label();
         this.VerticalOffsetTextBox = new System.Windows.Forms.TextBox();
         this.label4 = new System.Windows.Forms.Label();
         this.SetButton = new System.Windows.Forms.Button();
         this.MainStatusStrip.SuspendLayout();
         this.SuspendLayout();
         // 
         // ActivityButton
         // 
         this.ActivityButton.Location = new System.Drawing.Point(155, 27);
         this.ActivityButton.Name = "ActivityButton";
         this.ActivityButton.Size = new System.Drawing.Size(59, 23);
         this.ActivityButton.TabIndex = 2;
         this.ActivityButton.Text = "Activity";
         this.ActivityButton.UseVisualStyleBackColor = true;
         this.ActivityButton.Click += new System.EventHandler(this.ActivityButton_Click);
         // 
         // MainStatusStrip
         // 
         this.MainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel,
            this.TimeStatusLabel});
         this.MainStatusStrip.Location = new System.Drawing.Point(0, 251);
         this.MainStatusStrip.Name = "MainStatusStrip";
         this.MainStatusStrip.Size = new System.Drawing.Size(238, 22);
         this.MainStatusStrip.TabIndex = 13;
         this.MainStatusStrip.Text = "statusStrip1";
         // 
         // StatusLabel
         // 
         this.StatusLabel.AutoSize = false;
         this.StatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
         this.StatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
         this.StatusLabel.Name = "StatusLabel";
         this.StatusLabel.Size = new System.Drawing.Size(223, 17);
         this.StatusLabel.Spring = true;
         this.StatusLabel.Text = "Status";
         this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // TimeStatusLabel
         // 
         this.TimeStatusLabel.AutoSize = false;
         this.TimeStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
         this.TimeStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
         this.TimeStatusLabel.Name = "TimeStatusLabel";
         this.TimeStatusLabel.Size = new System.Drawing.Size(109, 17);
         this.TimeStatusLabel.Text = "00:00:00.000";
         // 
         // PortTextBox
         // 
         this.PortTextBox.Location = new System.Drawing.Point(94, 41);
         this.PortTextBox.Name = "PortTextBox";
         this.PortTextBox.Size = new System.Drawing.Size(55, 20);
         this.PortTextBox.TabIndex = 17;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(65, 44);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(26, 13);
         this.label2.TabIndex = 16;
         this.label2.Text = "Port";
         this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(42, 18);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(50, 13);
         this.label1.TabIndex = 15;
         this.label1.Text = "Baudrate";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // UpButton
         // 
         this.UpButton.Location = new System.Drawing.Point(90, 81);
         this.UpButton.Name = "UpButton";
         this.UpButton.Size = new System.Drawing.Size(59, 23);
         this.UpButton.TabIndex = 18;
         this.UpButton.Text = "Up";
         this.UpButton.UseVisualStyleBackColor = true;
         this.UpButton.Click += new System.EventHandler(this.UpButton_Click);
         // 
         // DownButton
         // 
         this.DownButton.Location = new System.Drawing.Point(90, 130);
         this.DownButton.Name = "DownButton";
         this.DownButton.Size = new System.Drawing.Size(59, 23);
         this.DownButton.TabIndex = 19;
         this.DownButton.Text = "Down";
         this.DownButton.UseVisualStyleBackColor = true;
         this.DownButton.Click += new System.EventHandler(this.DownButton_Click);
         // 
         // LeftButton
         // 
         this.LeftButton.Location = new System.Drawing.Point(25, 105);
         this.LeftButton.Name = "LeftButton";
         this.LeftButton.Size = new System.Drawing.Size(59, 23);
         this.LeftButton.TabIndex = 20;
         this.LeftButton.Text = "Left";
         this.LeftButton.UseVisualStyleBackColor = true;
         this.LeftButton.Click += new System.EventHandler(this.LeftButton_Click);
         // 
         // RightButton
         // 
         this.RightButton.Location = new System.Drawing.Point(155, 105);
         this.RightButton.Name = "RightButton";
         this.RightButton.Size = new System.Drawing.Size(59, 23);
         this.RightButton.TabIndex = 21;
         this.RightButton.Text = "Right";
         this.RightButton.UseVisualStyleBackColor = true;
         this.RightButton.Click += new System.EventHandler(this.RightButton_Click);
         // 
         // BaudTextBox
         // 
         this.BaudTextBox.Location = new System.Drawing.Point(94, 15);
         this.BaudTextBox.Name = "BaudTextBox";
         this.BaudTextBox.Size = new System.Drawing.Size(55, 20);
         this.BaudTextBox.TabIndex = 22;
         // 
         // HorizontalOffsetTextBox
         // 
         this.HorizontalOffsetTextBox.Location = new System.Drawing.Point(121, 176);
         this.HorizontalOffsetTextBox.Name = "HorizontalOffsetTextBox";
         this.HorizontalOffsetTextBox.Size = new System.Drawing.Size(38, 20);
         this.HorizontalOffsetTextBox.TabIndex = 24;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(34, 179);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(85, 13);
         this.label3.TabIndex = 23;
         this.label3.Text = "Horizontal Offset";
         this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // VerticalOffsetTextBox
         // 
         this.VerticalOffsetTextBox.Location = new System.Drawing.Point(121, 202);
         this.VerticalOffsetTextBox.Name = "VerticalOffsetTextBox";
         this.VerticalOffsetTextBox.Size = new System.Drawing.Size(38, 20);
         this.VerticalOffsetTextBox.TabIndex = 26;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(46, 205);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(73, 13);
         this.label4.TabIndex = 25;
         this.label4.Text = "Vertical Offset";
         this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // SetButton
         // 
         this.SetButton.Location = new System.Drawing.Point(165, 188);
         this.SetButton.Name = "SetButton";
         this.SetButton.Size = new System.Drawing.Size(39, 23);
         this.SetButton.TabIndex = 27;
         this.SetButton.Text = "Set";
         this.SetButton.UseVisualStyleBackColor = true;
         this.SetButton.Click += new System.EventHandler(this.SetButton_Click);
         // 
         // MainForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(238, 273);
         this.Controls.Add(this.SetButton);
         this.Controls.Add(this.VerticalOffsetTextBox);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.HorizontalOffsetTextBox);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.BaudTextBox);
         this.Controls.Add(this.RightButton);
         this.Controls.Add(this.LeftButton);
         this.Controls.Add(this.DownButton);
         this.Controls.Add(this.UpButton);
         this.Controls.Add(this.PortTextBox);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.MainStatusStrip);
         this.Controls.Add(this.ActivityButton);
         this.MaximumSize = new System.Drawing.Size(246, 300);
         this.MinimumSize = new System.Drawing.Size(246, 300);
         this.Name = "MainForm";
         this.Text = "OSD Control";
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
         this.Shown += new System.EventHandler(this.MainForm_Shown);
         this.MainStatusStrip.ResumeLayout(false);
         this.MainStatusStrip.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Button ActivityButton;
      private System.Windows.Forms.StatusStrip MainStatusStrip;
      private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
      private System.Windows.Forms.ToolStripStatusLabel TimeStatusLabel;
      private System.Windows.Forms.TextBox PortTextBox;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Button UpButton;
      private System.Windows.Forms.Button DownButton;
      private System.Windows.Forms.Button LeftButton;
      private System.Windows.Forms.Button RightButton;
      private System.Windows.Forms.TextBox BaudTextBox;
      private System.Windows.Forms.TextBox HorizontalOffsetTextBox;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.TextBox VerticalOffsetTextBox;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Button SetButton;
   }
}

