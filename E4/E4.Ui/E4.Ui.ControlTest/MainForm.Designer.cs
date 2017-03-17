namespace E4.Ui.ControlTest
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
         this.SetButton = new System.Windows.Forms.Button();
         this.ValueTextBox = new System.Windows.Forms.TextBox();
         this.TestPositionIndicator = new E4.Ui.Controls.PositionIndicator();
         this.SuspendLayout();
         // 
         // SetButton
         // 
         this.SetButton.Location = new System.Drawing.Point(295, 26);
         this.SetButton.Name = "SetButton";
         this.SetButton.Size = new System.Drawing.Size(75, 23);
         this.SetButton.TabIndex = 2;
         this.SetButton.Text = "Set";
         this.SetButton.UseVisualStyleBackColor = true;
         this.SetButton.Click += new System.EventHandler(this.SetButton_Click);
         // 
         // ValueTextBox
         // 
         this.ValueTextBox.Location = new System.Drawing.Point(280, 70);
         this.ValueTextBox.Name = "ValueTextBox";
         this.ValueTextBox.Size = new System.Drawing.Size(100, 20);
         this.ValueTextBox.TabIndex = 3;
         // 
         // TestPositionIndicator
         // 
         this.TestPositionIndicator.BackColor = System.Drawing.SystemColors.ControlDark;
         this.TestPositionIndicator.EdgeColor = System.Drawing.Color.Black;
         this.TestPositionIndicator.EdgeWeight = 3;
         this.TestPositionIndicator.Location = new System.Drawing.Point(54, 26);
         this.TestPositionIndicator.MaximumPosition = 100;
         this.TestPositionIndicator.MinimumPosition = 0;
         this.TestPositionIndicator.Name = "TestPositionIndicator";
         this.TestPositionIndicator.Position = 50;
         this.TestPositionIndicator.Size = new System.Drawing.Size(119, 248);
         this.TestPositionIndicator.TabIndex = 1;
         this.TestPositionIndicator.TickColor = System.Drawing.Color.Red;
         this.TestPositionIndicator.TickMotion = E4.Ui.Controls.PositionIndicator.TickMotions.horizontal;
         this.TestPositionIndicator.TickWeight = 3;
         // 
         // MainForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(865, 554);
         this.Controls.Add(this.ValueTextBox);
         this.Controls.Add(this.SetButton);
         this.Controls.Add(this.TestPositionIndicator);
         this.Name = "MainForm";
         this.Text = "E4 Controls Test";
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private Controls.PositionIndicator TestPositionIndicator;
      private System.Windows.Forms.Button SetButton;
      private System.Windows.Forms.TextBox ValueTextBox;
   }
}

