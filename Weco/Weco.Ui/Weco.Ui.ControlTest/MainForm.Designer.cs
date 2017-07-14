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
         this.ZeroCheckBox = new System.Windows.Forms.CheckBox();
         this.XTrackBar = new System.Windows.Forms.TrackBar();
         this.YTrackBar = new System.Windows.Forms.TrackBar();
         this.FaultCheckBox = new System.Windows.Forms.CheckBox();
         this.RollTrackBar = new System.Windows.Forms.TrackBar();
         this.PositionRollTrackBar = new System.Windows.Forms.TrackBar();
         this.roundIndicator1 = new E4.Ui.Controls.RoundIndicator();
         this.clockwiseButton4 = new E4.Ui.Controls.ClockwiseButton();
         this.clockwiseButton3 = new E4.Ui.Controls.ClockwiseButton();
         this.clockwiseButton2 = new E4.Ui.Controls.ClockwiseButton();
         this.clockwiseButton1 = new E4.Ui.Controls.ClockwiseButton();
         this.TestRollDisplay = new E4.Ui.Controls.RollDisplay();
         this.TestScannerIndicator = new E4.Ui.Controls.ScannerIndicator();
         this.TestPositionIndicator = new E4.Ui.Controls.PositionIndicator();
         ((System.ComponentModel.ISupportInitialize)(this.XTrackBar)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.YTrackBar)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.RollTrackBar)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.PositionRollTrackBar)).BeginInit();
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
         // RollTrackBar
         // 
         this.RollTrackBar.Location = new System.Drawing.Point(331, 195);
         this.RollTrackBar.Maximum = 359;
         this.RollTrackBar.Name = "RollTrackBar";
         this.RollTrackBar.Size = new System.Drawing.Size(183, 42);
         this.RollTrackBar.TabIndex = 11;
         this.RollTrackBar.TickFrequency = 5;
         this.RollTrackBar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
         this.RollTrackBar.Value = 1;
         this.RollTrackBar.Scroll += new System.EventHandler(this.RollTrackBar_Scroll);
         // 
         // PositionRollTrackBar
         // 
         this.PositionRollTrackBar.Location = new System.Drawing.Point(331, 230);
         this.PositionRollTrackBar.Maximum = 359;
         this.PositionRollTrackBar.Name = "PositionRollTrackBar";
         this.PositionRollTrackBar.Size = new System.Drawing.Size(183, 42);
         this.PositionRollTrackBar.TabIndex = 17;
         this.PositionRollTrackBar.TickFrequency = 5;
         this.PositionRollTrackBar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
         this.PositionRollTrackBar.Value = 1;
         this.PositionRollTrackBar.Scroll += new System.EventHandler(this.PositionRollTrackBar_Scroll);
         // 
         // roundIndicator1
         // 
         this.roundIndicator1.BackColor = System.Drawing.SystemColors.Control;
         this.roundIndicator1.IndicatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(15)))));
         this.roundIndicator1.IndicatorLineWeight = 1F;
         this.roundIndicator1.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
         this.roundIndicator1.InnerLineWeight = 1F;
         this.roundIndicator1.InnerSpacing = 5F;
         this.roundIndicator1.Location = new System.Drawing.Point(163, 320);
         this.roundIndicator1.Name = "roundIndicator1";
         this.roundIndicator1.OuterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
         this.roundIndicator1.OuterLineWeight = 2F;
         this.roundIndicator1.OuterSpacing = 9F;
         this.roundIndicator1.Size = new System.Drawing.Size(100, 100);
         this.roundIndicator1.TabIndex = 16;
         // 
         // clockwiseButton4
         // 
         this.clockwiseButton4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.clockwiseButton4.Direction = E4.Ui.Controls.ClockwiseButton.Directions.counterClockwise;
         this.clockwiseButton4.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.clockwiseButton4.DisabledFillColor = System.Drawing.Color.Gray;
         this.clockwiseButton4.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
         this.clockwiseButton4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.clockwiseButton4.HoldArrorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.clockwiseButton4.HoldRepeat = false;
         this.clockwiseButton4.HoldRepeatInterval = 0;
         this.clockwiseButton4.HoldTimeoutInterval = 2000;
         this.clockwiseButton4.LineWeight = 3F;
         this.clockwiseButton4.Location = new System.Drawing.Point(382, 390);
         this.clockwiseButton4.Name = "clockwiseButton4";
         this.clockwiseButton4.Size = new System.Drawing.Size(69, 69);
         this.clockwiseButton4.TabIndex = 15;
         this.clockwiseButton4.Text = "clockwiseButton4";
         this.clockwiseButton4.UseVisualStyleBackColor = false;
         // 
         // clockwiseButton3
         // 
         this.clockwiseButton3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.clockwiseButton3.Direction = E4.Ui.Controls.ClockwiseButton.Directions.counterClockwise;
         this.clockwiseButton3.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.clockwiseButton3.DisabledFillColor = System.Drawing.Color.Gray;
         this.clockwiseButton3.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
         this.clockwiseButton3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.clockwiseButton3.HoldArrorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.clockwiseButton3.HoldRepeat = false;
         this.clockwiseButton3.HoldRepeatInterval = 0;
         this.clockwiseButton3.HoldTimeoutInterval = 2000;
         this.clockwiseButton3.LineWeight = 3F;
         this.clockwiseButton3.Location = new System.Drawing.Point(344, 289);
         this.clockwiseButton3.Name = "clockwiseButton3";
         this.clockwiseButton3.Size = new System.Drawing.Size(107, 80);
         this.clockwiseButton3.TabIndex = 14;
         this.clockwiseButton3.Text = "clockwiseButton3";
         this.clockwiseButton3.UseVisualStyleBackColor = false;
         // 
         // clockwiseButton2
         // 
         this.clockwiseButton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.clockwiseButton2.Direction = E4.Ui.Controls.ClockwiseButton.Directions.clockwise;
         this.clockwiseButton2.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.clockwiseButton2.DisabledFillColor = System.Drawing.Color.Gray;
         this.clockwiseButton2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
         this.clockwiseButton2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.clockwiseButton2.HoldArrorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.clockwiseButton2.HoldRepeat = false;
         this.clockwiseButton2.HoldRepeatInterval = 0;
         this.clockwiseButton2.HoldTimeoutInterval = 2000;
         this.clockwiseButton2.LineWeight = 3F;
         this.clockwiseButton2.Location = new System.Drawing.Point(457, 390);
         this.clockwiseButton2.Name = "clockwiseButton2";
         this.clockwiseButton2.Size = new System.Drawing.Size(69, 69);
         this.clockwiseButton2.TabIndex = 13;
         this.clockwiseButton2.Text = "clockwiseButton2";
         this.clockwiseButton2.UseVisualStyleBackColor = false;
         // 
         // clockwiseButton1
         // 
         this.clockwiseButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.clockwiseButton1.Direction = E4.Ui.Controls.ClockwiseButton.Directions.clockwise;
         this.clockwiseButton1.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.clockwiseButton1.DisabledFillColor = System.Drawing.Color.Gray;
         this.clockwiseButton1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
         this.clockwiseButton1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.clockwiseButton1.HoldArrorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.clockwiseButton1.HoldRepeat = false;
         this.clockwiseButton1.HoldRepeatInterval = 0;
         this.clockwiseButton1.HoldTimeoutInterval = 2000;
         this.clockwiseButton1.LineWeight = 3F;
         this.clockwiseButton1.Location = new System.Drawing.Point(457, 289);
         this.clockwiseButton1.Name = "clockwiseButton1";
         this.clockwiseButton1.Size = new System.Drawing.Size(107, 80);
         this.clockwiseButton1.TabIndex = 12;
         this.clockwiseButton1.Text = "clockwiseButton1";
         this.clockwiseButton1.UseVisualStyleBackColor = false;
         // 
         // TestRollDisplay
         // 
         this.TestRollDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.TestRollDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TestRollDisplay.GaugeBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
         this.TestRollDisplay.GaugeEdgeColor = System.Drawing.Color.Black;
         this.TestRollDisplay.GaugeEdgeWeight = 1F;
         this.TestRollDisplay.GaugeIndicator = E4.Ui.Controls.RollDisplay.GaugeIndicators.rectangle;
         this.TestRollDisplay.GaugeInnerNumberSpace = 3;
         this.TestRollDisplay.GaugeOuterNumberSpace = 3;
         this.TestRollDisplay.IndicatorColor = System.Drawing.Color.Firebrick;
         this.TestRollDisplay.IndicatorWeight = 2F;
         this.TestRollDisplay.LeftPushIndicatorAngle = 60F;
         this.TestRollDisplay.LeftPushIndicatorColor = System.Drawing.Color.LimeGreen;
         this.TestRollDisplay.Location = new System.Drawing.Point(331, 20);
         this.TestRollDisplay.Name = "TestRollDisplay";
         this.TestRollDisplay.PinWeight = 7F;
         this.TestRollDisplay.Pitch = 180F;
         this.TestRollDisplay.PitchBackColor = System.Drawing.Color.Black;
         this.TestRollDisplay.PitchFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TestRollDisplay.PitchForeColor = System.Drawing.Color.White;
         this.TestRollDisplay.PitchHeight = 20;
         this.TestRollDisplay.PitchVisible = true;
         this.TestRollDisplay.PitchWidth = 35;
         this.TestRollDisplay.PositionCcwLimit = -45F;
         this.TestRollDisplay.PositionCwLimit = 45F;
         this.TestRollDisplay.PositionRoll = 90F;
         this.TestRollDisplay.PushIndicatorEdgeColor = System.Drawing.Color.Black;
         this.TestRollDisplay.PushIndicatorEdgeWeight = 1F;
         this.TestRollDisplay.RightPushIndicatorAngle = 60F;
         this.TestRollDisplay.RightPushIndicatorColor = System.Drawing.Color.LimeGreen;
         this.TestRollDisplay.Roll = 0F;
         this.TestRollDisplay.RollBackColor = System.Drawing.Color.Black;
         this.TestRollDisplay.RollFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
         this.TestRollDisplay.RollForeColor = System.Drawing.Color.White;
         this.TestRollDisplay.RollHeight = 20;
         this.TestRollDisplay.RollVisible = true;
         this.TestRollDisplay.RollWidth = 35;
         this.TestRollDisplay.ShowPosition = true;
         this.TestRollDisplay.ShowPushIndicators = false;
         this.TestRollDisplay.Size = new System.Drawing.Size(150, 150);
         this.TestRollDisplay.TabIndex = 10;
         this.TestRollDisplay.Yaw = 51F;
         this.TestRollDisplay.YawBackColor = System.Drawing.Color.Black;
         this.TestRollDisplay.YawFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
         this.TestRollDisplay.YawForeColor = System.Drawing.Color.White;
         this.TestRollDisplay.YawHeight = 20;
         this.TestRollDisplay.YawVisible = true;
         this.TestRollDisplay.YawWidth = 35;
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
         // MainForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Control;
         this.ClientSize = new System.Drawing.Size(865, 554);
         this.Controls.Add(this.PositionRollTrackBar);
         this.Controls.Add(this.roundIndicator1);
         this.Controls.Add(this.clockwiseButton4);
         this.Controls.Add(this.clockwiseButton3);
         this.Controls.Add(this.clockwiseButton2);
         this.Controls.Add(this.clockwiseButton1);
         this.Controls.Add(this.RollTrackBar);
         this.Controls.Add(this.TestRollDisplay);
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
         ((System.ComponentModel.ISupportInitialize)(this.RollTrackBar)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.PositionRollTrackBar)).EndInit();
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
      private Controls.RollDisplay TestRollDisplay;
      private System.Windows.Forms.TrackBar RollTrackBar;
      private Controls.ClockwiseButton clockwiseButton1;
      private Controls.ClockwiseButton clockwiseButton2;
      private Controls.ClockwiseButton clockwiseButton3;
      private Controls.ClockwiseButton clockwiseButton4;
      private Controls.RoundIndicator roundIndicator1;
      private System.Windows.Forms.TrackBar PositionRollTrackBar;
   }
}

