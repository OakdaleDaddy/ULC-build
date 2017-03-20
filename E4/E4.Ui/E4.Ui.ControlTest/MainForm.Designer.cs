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
         this.TestScannerIndicator = new E4.Ui.Controls.ScannerIndicator();
         this.TestPositionIndicator = new E4.Ui.Controls.PositionIndicator();
         this.ZeroCheckBox = new System.Windows.Forms.CheckBox();
         this.XTrackBar = new System.Windows.Forms.TrackBar();
         this.YTrackBar = new System.Windows.Forms.TrackBar();
         this.FaultCheckBox = new System.Windows.Forms.CheckBox();
         ((System.ComponentModel.ISupportInitialize)(this.XTrackBar)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.YTrackBar)).BeginInit();
         this.SuspendLayout();
         // 
         // SetButton
         // 
         this.SetButton.Location = new System.Drawing.Point(717, 276);
         this.SetButton.Name = "SetButton";
         this.SetButton.Size = new System.Drawing.Size(75, 23);
         this.SetButton.TabIndex = 2;
         this.SetButton.Text = "Set";
         this.SetButton.UseVisualStyleBackColor = true;
         this.SetButton.Click += new System.EventHandler(this.SetButton_Click);
         // 
         // ValueTextBox
         // 
         this.ValueTextBox.Location = new System.Drawing.Point(702, 320);
         this.ValueTextBox.Name = "ValueTextBox";
         this.ValueTextBox.Size = new System.Drawing.Size(100, 20);
         this.ValueTextBox.TabIndex = 3;
         this.ValueTextBox.Text = "52";
         // 
         // TestScannerIndicator
         // 
         this.TestScannerIndicator.BackColor = System.Drawing.SystemColors.ControlDark;
         this.TestScannerIndicator.ControlEdgeColor = System.Drawing.Color.Black;
         this.TestScannerIndicator.ControlEdgeWeight = 2;
         this.TestScannerIndicator.CoordinateBits = 4;
         this.TestScannerIndicator.CoordinateValue = ((uint)(33u));
         this.TestScannerIndicator.GridColor = System.Drawing.Color.DarkGray;
         this.TestScannerIndicator.GridWeight = 1;
         this.TestScannerIndicator.IndicatorEdgeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.TestScannerIndicator.IndicatorEdgeWeight = 2;
         this.TestScannerIndicator.Location = new System.Drawing.Point(23, 20);
         this.TestScannerIndicator.MissColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
         this.TestScannerIndicator.MissWeight = 5;
         this.TestScannerIndicator.Name = "TestScannerIndicator";
         this.TestScannerIndicator.Size = new System.Drawing.Size(182, 182);
         this.TestScannerIndicator.TabIndex = 5;
         this.TestScannerIndicator.TickColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
         // 
         // TestPositionIndicator
         // 
         this.TestPositionIndicator.BackColor = System.Drawing.SystemColors.ControlDark;
         this.TestPositionIndicator.EdgeColor = System.Drawing.Color.Black;
         this.TestPositionIndicator.EdgeWeight = 3;
         this.TestPositionIndicator.Location = new System.Drawing.Point(702, 12);
         this.TestPositionIndicator.MaximumPosition = 100;
         this.TestPositionIndicator.MinimumPosition = 0;
         this.TestPositionIndicator.Name = "TestPositionIndicator";
         this.TestPositionIndicator.Position = 50;
         this.TestPositionIndicator.Size = new System.Drawing.Size(119, 248);
         this.TestPositionIndicator.TabIndex = 1;
         this.TestPositionIndicator.TickColor = System.Drawing.Color.Maroon;
         this.TestPositionIndicator.TickMotion = E4.Ui.Controls.PositionIndicator.TickMotions.horizontal;
         this.TestPositionIndicator.TickWeight = 3;
         // 
         // ZeroCheckBox
         // 
         this.ZeroCheckBox.AutoSize = true;
         this.ZeroCheckBox.Location = new System.Drawing.Point(223, 220);
         this.ZeroCheckBox.Name = "ZeroCheckBox";
         this.ZeroCheckBox.Size = new System.Drawing.Size(48, 17);
         this.ZeroCheckBox.TabIndex = 6;
         this.ZeroCheckBox.Text = "Zero";
         this.ZeroCheckBox.UseVisualStyleBackColor = true;
         this.ZeroCheckBox.CheckedChanged += new System.EventHandler(this.ZeroCheckBox_CheckedChanged);
         // 
         // XTrackBar
         // 
         this.XTrackBar.Location = new System.Drawing.Point(23, 209);
         this.XTrackBar.Maximum = 15;
         this.XTrackBar.Minimum = 1;
         this.XTrackBar.Name = "XTrackBar";
         this.XTrackBar.Size = new System.Drawing.Size(183, 42);
         this.XTrackBar.TabIndex = 7;
         this.XTrackBar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
         this.XTrackBar.Value = 1;
         this.XTrackBar.Scroll += new System.EventHandler(this.XTrackBar_Scroll);
         // 
         // YTrackBar
         // 
         this.YTrackBar.Location = new System.Drawing.Point(212, 20);
         this.YTrackBar.Maximum = 15;
         this.YTrackBar.Minimum = 1;
         this.YTrackBar.Name = "YTrackBar";
         this.YTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
         this.YTrackBar.Size = new System.Drawing.Size(42, 183);
         this.YTrackBar.TabIndex = 8;
         this.YTrackBar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
         this.YTrackBar.Value = 1;
         this.YTrackBar.Scroll += new System.EventHandler(this.YTrackBar_Scroll);
         // 
         // FaultCheckBox
         // 
         this.FaultCheckBox.AutoSize = true;
         this.FaultCheckBox.Location = new System.Drawing.Point(223, 243);
         this.FaultCheckBox.Name = "FaultCheckBox";
         this.FaultCheckBox.Size = new System.Drawing.Size(49, 17);
         this.FaultCheckBox.TabIndex = 9;
         this.FaultCheckBox.Text = "Fault";
         this.FaultCheckBox.UseVisualStyleBackColor = true;
         this.FaultCheckBox.CheckedChanged += new System.EventHandler(this.FaultCheckBox_CheckedChanged);
         // 
         // MainForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(865, 554);
         this.Controls.Add(this.FaultCheckBox);
         this.Controls.Add(this.YTrackBar);
         this.Controls.Add(this.XTrackBar);
         this.Controls.Add(this.ZeroCheckBox);
         this.Controls.Add(this.TestScannerIndicator);
         this.Controls.Add(this.ValueTextBox);
         this.Controls.Add(this.SetButton);
         this.Controls.Add(this.TestPositionIndicator);
         this.Name = "MainForm";
         this.Text = "E4 Controls Test";
         ((System.ComponentModel.ISupportInitialize)(this.XTrackBar)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.YTrackBar)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private Controls.PositionIndicator TestPositionIndicator;
      private System.Windows.Forms.Button SetButton;
      private System.Windows.Forms.TextBox ValueTextBox;
      private Controls.ScannerIndicator TestScannerIndicator;
      private System.Windows.Forms.CheckBox ZeroCheckBox;
      private System.Windows.Forms.TrackBar XTrackBar;
      private System.Windows.Forms.TrackBar YTrackBar;
      private System.Windows.Forms.CheckBox FaultCheckBox;
   }
}

