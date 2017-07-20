namespace Weco.TouchTest
{
   partial class MainForm : TouchForm
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
         this.CloseButton = new System.Windows.Forms.Button();
         this.TestButton = new System.Windows.Forms.Button();
         this.BaseButton = new Weco.Ui.Controls.BaseButton();
         this.TouchIndicator = new Weco.Ui.Controls.RoundIndicator();
         this.SuspendLayout();
         // 
         // CloseButton
         // 
         this.CloseButton.Location = new System.Drawing.Point(175, 178);
         this.CloseButton.Name = "CloseButton";
         this.CloseButton.Size = new System.Drawing.Size(75, 23);
         this.CloseButton.TabIndex = 0;
         this.CloseButton.Text = "Close";
         this.CloseButton.UseVisualStyleBackColor = true;
         this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
         // 
         // TestButton
         // 
         this.TestButton.Location = new System.Drawing.Point(42, 24);
         this.TestButton.Name = "TestButton";
         this.TestButton.Size = new System.Drawing.Size(75, 23);
         this.TestButton.TabIndex = 3;
         this.TestButton.Text = "Test";
         this.TestButton.UseVisualStyleBackColor = true;
         this.TestButton.Click += new System.EventHandler(this.TestButton_Click);
         this.TestButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TestButton_MouseClick);
         this.TestButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TestButton_MouseDown);
         this.TestButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TestButton_MouseUp);
         // 
         // BaseButton
         // 
         this.BaseButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BaseButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BaseButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.BaseButton.Location = new System.Drawing.Point(26, 69);
         this.BaseButton.Name = "BaseButton";
         this.BaseButton.Size = new System.Drawing.Size(107, 80);
         this.BaseButton.TabIndex = 2;
         this.BaseButton.Text = "Base";
         this.BaseButton.UseVisualStyleBackColor = true;
         this.BaseButton.Click += new System.EventHandler(this.BaseButton_Click);
         this.BaseButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.BaseButton_MouseClick);
         this.BaseButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BaseButton_MouseDown);
         this.BaseButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BaseButton_MouseUp);
         // 
         // TouchIndicator
         // 
         this.TouchIndicator.IndicatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(15)))));
         this.TouchIndicator.IndicatorLineWeight = 1F;
         this.TouchIndicator.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
         this.TouchIndicator.InnerLineWeight = 1F;
         this.TouchIndicator.InnerSpacing = 3F;
         this.TouchIndicator.Location = new System.Drawing.Point(182, 42);
         this.TouchIndicator.Name = "TouchIndicator";
         this.TouchIndicator.OuterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
         this.TouchIndicator.OuterLineWeight = 1F;
         this.TouchIndicator.OuterSpacing = 6F;
         this.TouchIndicator.Size = new System.Drawing.Size(68, 76);
         this.TouchIndicator.TabIndex = 1;
         this.TouchIndicator.Text = "roundIndicator1";
         // 
         // MainForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(284, 224);
         this.Controls.Add(this.TestButton);
         this.Controls.Add(this.BaseButton);
         this.Controls.Add(this.TouchIndicator);
         this.Controls.Add(this.CloseButton);
         this.Name = "MainForm";
         this.Text = "Touch Test";
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button CloseButton;
      private Ui.Controls.RoundIndicator TouchIndicator;
      private Ui.Controls.BaseButton BaseButton;
      private System.Windows.Forms.Button TestButton;
   }
}

