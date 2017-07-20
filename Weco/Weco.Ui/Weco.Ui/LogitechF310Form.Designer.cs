namespace Weco.Ui
{
   partial class LogitechF310Form
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
         this.BackButton = new Weco.Ui.Controls.BaseButton();
         this.MainPanel = new Weco.Ui.Controls.BorderedPanel();
         this.Button4Indicator = new Weco.Ui.Controls.RoundIndicator();
         this.Button2Indicator = new Weco.Ui.Controls.RoundIndicator();
         this.Button3Indicator = new Weco.Ui.Controls.RoundIndicator();
         this.Button1Indicator = new Weco.Ui.Controls.RoundIndicator();
         this.Axis1XPanel = new Weco.Ui.Controls.TextPanel();
         this.Axis1YPanel = new Weco.Ui.Controls.TextPanel();
         this.Axis2YPanel = new Weco.Ui.Controls.TextPanel();
         this.Axis2XPanel = new Weco.Ui.Controls.TextPanel();
         this.PovPanel = new Weco.Ui.Controls.TextPanel();
         this.Button9Indicator = new Weco.Ui.Controls.RoundIndicator();
         this.Button10Indicator = new Weco.Ui.Controls.RoundIndicator();
         this.Button5Indicator = new Weco.Ui.Controls.RoundIndicator();
         this.Button7Indicator = new Weco.Ui.Controls.RoundIndicator();
         this.Button6Indicator = new Weco.Ui.Controls.RoundIndicator();
         this.Button8Indicator = new Weco.Ui.Controls.RoundIndicator();
         this.Button11Indicator = new Weco.Ui.Controls.RoundIndicator();
         this.Button12Indicator = new Weco.Ui.Controls.RoundIndicator();
         this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
         this.MainPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // BackButton
         // 
         this.BackButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BackButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BackButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.BackButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.BackButton.Location = new System.Drawing.Point(400, 313);
         this.BackButton.Name = "BackButton";
         this.BackButton.Size = new System.Drawing.Size(107, 67);
         this.BackButton.TabIndex = 7;
         this.BackButton.Text = "BACK";
         this.BackButton.UseVisualStyleBackColor = false;
         this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
         // 
         // MainPanel
         // 
         this.MainPanel.BackColor = System.Drawing.Color.Teal;
         this.MainPanel.Controls.Add(this.Button12Indicator);
         this.MainPanel.Controls.Add(this.Button11Indicator);
         this.MainPanel.Controls.Add(this.Button8Indicator);
         this.MainPanel.Controls.Add(this.Button6Indicator);
         this.MainPanel.Controls.Add(this.Button7Indicator);
         this.MainPanel.Controls.Add(this.Button5Indicator);
         this.MainPanel.Controls.Add(this.Button10Indicator);
         this.MainPanel.Controls.Add(this.Button9Indicator);
         this.MainPanel.Controls.Add(this.PovPanel);
         this.MainPanel.Controls.Add(this.Axis2YPanel);
         this.MainPanel.Controls.Add(this.Axis2XPanel);
         this.MainPanel.Controls.Add(this.Axis1YPanel);
         this.MainPanel.Controls.Add(this.Axis1XPanel);
         this.MainPanel.Controls.Add(this.Button1Indicator);
         this.MainPanel.Controls.Add(this.Button3Indicator);
         this.MainPanel.Controls.Add(this.Button2Indicator);
         this.MainPanel.Controls.Add(this.Button4Indicator);
         this.MainPanel.Controls.Add(this.BackButton);
         this.MainPanel.EdgeWeight = 3;
         this.MainPanel.Location = new System.Drawing.Point(0, 0);
         this.MainPanel.Name = "MainPanel";
         this.MainPanel.Size = new System.Drawing.Size(523, 396);
         this.MainPanel.TabIndex = 8;
         // 
         // Button4Indicator
         // 
         this.Button4Indicator.IndicatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
         this.Button4Indicator.IndicatorLineWeight = 0F;
         this.Button4Indicator.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
         this.Button4Indicator.InnerLineWeight = 0F;
         this.Button4Indicator.InnerSpacing = 0F;
         this.Button4Indicator.Location = new System.Drawing.Point(421, 126);
         this.Button4Indicator.Name = "Button4Indicator";
         this.Button4Indicator.OuterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
         this.Button4Indicator.OuterLineWeight = 1F;
         this.Button4Indicator.OuterSpacing = 1F;
         this.Button4Indicator.Size = new System.Drawing.Size(40, 40);
         this.Button4Indicator.TabIndex = 8;
         this.Button4Indicator.Text = "roundIndicator1";
         // 
         // Button2Indicator
         // 
         this.Button2Indicator.IndicatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
         this.Button2Indicator.IndicatorLineWeight = 0F;
         this.Button2Indicator.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
         this.Button2Indicator.InnerLineWeight = 0F;
         this.Button2Indicator.InnerSpacing = 0F;
         this.Button2Indicator.Location = new System.Drawing.Point(421, 215);
         this.Button2Indicator.Name = "Button2Indicator";
         this.Button2Indicator.OuterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
         this.Button2Indicator.OuterLineWeight = 1F;
         this.Button2Indicator.OuterSpacing = 1F;
         this.Button2Indicator.Size = new System.Drawing.Size(40, 40);
         this.Button2Indicator.TabIndex = 9;
         this.Button2Indicator.Text = "roundIndicator2";
         // 
         // Button3Indicator
         // 
         this.Button3Indicator.IndicatorColor = System.Drawing.Color.DarkRed;
         this.Button3Indicator.IndicatorLineWeight = 0F;
         this.Button3Indicator.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
         this.Button3Indicator.InnerLineWeight = 0F;
         this.Button3Indicator.InnerSpacing = 0F;
         this.Button3Indicator.Location = new System.Drawing.Point(467, 169);
         this.Button3Indicator.Name = "Button3Indicator";
         this.Button3Indicator.OuterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
         this.Button3Indicator.OuterLineWeight = 1F;
         this.Button3Indicator.OuterSpacing = 1F;
         this.Button3Indicator.Size = new System.Drawing.Size(40, 40);
         this.Button3Indicator.TabIndex = 10;
         this.Button3Indicator.Text = "roundIndicator3";
         // 
         // Button1Indicator
         // 
         this.Button1Indicator.IndicatorColor = System.Drawing.Color.LightBlue;
         this.Button1Indicator.IndicatorLineWeight = 0F;
         this.Button1Indicator.InnerColor = System.Drawing.Color.Blue;
         this.Button1Indicator.InnerLineWeight = 0F;
         this.Button1Indicator.InnerSpacing = 0F;
         this.Button1Indicator.Location = new System.Drawing.Point(375, 169);
         this.Button1Indicator.Name = "Button1Indicator";
         this.Button1Indicator.OuterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
         this.Button1Indicator.OuterLineWeight = 1F;
         this.Button1Indicator.OuterSpacing = 1F;
         this.Button1Indicator.Size = new System.Drawing.Size(40, 40);
         this.Button1Indicator.TabIndex = 11;
         this.Button1Indicator.Text = "roundIndicator4";
         // 
         // Axis1XPanel
         // 
         this.Axis1XPanel.BackColor = System.Drawing.Color.Black;
         this.Axis1XPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.Axis1XPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.Axis1XPanel.ForeColor = System.Drawing.Color.White;
         this.Axis1XPanel.HoldTimeoutEnable = false;
         this.Axis1XPanel.HoldTimeoutInterval = 0;
         this.Axis1XPanel.Location = new System.Drawing.Point(129, 244);
         this.Axis1XPanel.Name = "Axis1XPanel";
         this.Axis1XPanel.Size = new System.Drawing.Size(99, 42);
         this.Axis1XPanel.TabIndex = 221;
         this.Axis1XPanel.ValueText = "#.## A";
         this.Axis1XPanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // Axis1YPanel
         // 
         this.Axis1YPanel.BackColor = System.Drawing.Color.Black;
         this.Axis1YPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.Axis1YPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.Axis1YPanel.ForeColor = System.Drawing.Color.White;
         this.Axis1YPanel.HoldTimeoutEnable = false;
         this.Axis1YPanel.HoldTimeoutInterval = 0;
         this.Axis1YPanel.Location = new System.Drawing.Point(129, 292);
         this.Axis1YPanel.Name = "Axis1YPanel";
         this.Axis1YPanel.Size = new System.Drawing.Size(99, 42);
         this.Axis1YPanel.TabIndex = 222;
         this.Axis1YPanel.ValueText = "#.## A";
         this.Axis1YPanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // Axis2YPanel
         // 
         this.Axis2YPanel.BackColor = System.Drawing.Color.Black;
         this.Axis2YPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.Axis2YPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.Axis2YPanel.ForeColor = System.Drawing.Color.White;
         this.Axis2YPanel.HoldTimeoutEnable = false;
         this.Axis2YPanel.HoldTimeoutInterval = 0;
         this.Axis2YPanel.Location = new System.Drawing.Point(246, 292);
         this.Axis2YPanel.Name = "Axis2YPanel";
         this.Axis2YPanel.Size = new System.Drawing.Size(99, 42);
         this.Axis2YPanel.TabIndex = 224;
         this.Axis2YPanel.ValueText = "#.## A";
         this.Axis2YPanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // Axis2XPanel
         // 
         this.Axis2XPanel.BackColor = System.Drawing.Color.Black;
         this.Axis2XPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.Axis2XPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.Axis2XPanel.ForeColor = System.Drawing.Color.White;
         this.Axis2XPanel.HoldTimeoutEnable = false;
         this.Axis2XPanel.HoldTimeoutInterval = 0;
         this.Axis2XPanel.Location = new System.Drawing.Point(246, 244);
         this.Axis2XPanel.Name = "Axis2XPanel";
         this.Axis2XPanel.Size = new System.Drawing.Size(99, 42);
         this.Axis2XPanel.TabIndex = 223;
         this.Axis2XPanel.ValueText = "#.## A";
         this.Axis2XPanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // PovPanel
         // 
         this.PovPanel.BackColor = System.Drawing.Color.Black;
         this.PovPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.PovPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.PovPanel.ForeColor = System.Drawing.Color.White;
         this.PovPanel.HoldTimeoutEnable = false;
         this.PovPanel.HoldTimeoutInterval = 0;
         this.PovPanel.Location = new System.Drawing.Point(16, 168);
         this.PovPanel.Name = "PovPanel";
         this.PovPanel.Size = new System.Drawing.Size(99, 42);
         this.PovPanel.TabIndex = 225;
         this.PovPanel.ValueText = "#.## A";
         this.PovPanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // Button9Indicator
         // 
         this.Button9Indicator.IndicatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.Button9Indicator.IndicatorLineWeight = 0F;
         this.Button9Indicator.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
         this.Button9Indicator.InnerLineWeight = 0F;
         this.Button9Indicator.InnerSpacing = 0F;
         this.Button9Indicator.Location = new System.Drawing.Point(176, 136);
         this.Button9Indicator.Name = "Button9Indicator";
         this.Button9Indicator.OuterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
         this.Button9Indicator.OuterLineWeight = 1F;
         this.Button9Indicator.OuterSpacing = 1F;
         this.Button9Indicator.Size = new System.Drawing.Size(29, 21);
         this.Button9Indicator.TabIndex = 226;
         this.Button9Indicator.Text = "roundIndicator1";
         // 
         // Button10Indicator
         // 
         this.Button10Indicator.IndicatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.Button10Indicator.IndicatorLineWeight = 0F;
         this.Button10Indicator.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
         this.Button10Indicator.InnerLineWeight = 0F;
         this.Button10Indicator.InnerSpacing = 0F;
         this.Button10Indicator.Location = new System.Drawing.Point(263, 136);
         this.Button10Indicator.Name = "Button10Indicator";
         this.Button10Indicator.OuterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
         this.Button10Indicator.OuterLineWeight = 1F;
         this.Button10Indicator.OuterSpacing = 1F;
         this.Button10Indicator.Size = new System.Drawing.Size(29, 21);
         this.Button10Indicator.TabIndex = 227;
         this.Button10Indicator.Text = "roundIndicator2";
         // 
         // Button5Indicator
         // 
         this.Button5Indicator.IndicatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.Button5Indicator.IndicatorLineWeight = 0F;
         this.Button5Indicator.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
         this.Button5Indicator.InnerLineWeight = 0F;
         this.Button5Indicator.InnerSpacing = 0F;
         this.Button5Indicator.Location = new System.Drawing.Point(16, 63);
         this.Button5Indicator.Name = "Button5Indicator";
         this.Button5Indicator.OuterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
         this.Button5Indicator.OuterLineWeight = 1F;
         this.Button5Indicator.OuterSpacing = 1F;
         this.Button5Indicator.Size = new System.Drawing.Size(40, 40);
         this.Button5Indicator.TabIndex = 228;
         this.Button5Indicator.Text = "roundIndicator1";
         // 
         // Button7Indicator
         // 
         this.Button7Indicator.IndicatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.Button7Indicator.IndicatorLineWeight = 0F;
         this.Button7Indicator.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
         this.Button7Indicator.InnerLineWeight = 0F;
         this.Button7Indicator.InnerSpacing = 0F;
         this.Button7Indicator.Location = new System.Drawing.Point(16, 17);
         this.Button7Indicator.Name = "Button7Indicator";
         this.Button7Indicator.OuterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
         this.Button7Indicator.OuterLineWeight = 1F;
         this.Button7Indicator.OuterSpacing = 1F;
         this.Button7Indicator.Size = new System.Drawing.Size(40, 40);
         this.Button7Indicator.TabIndex = 229;
         this.Button7Indicator.Text = "roundIndicator1";
         // 
         // Button6Indicator
         // 
         this.Button6Indicator.IndicatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.Button6Indicator.IndicatorLineWeight = 0F;
         this.Button6Indicator.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
         this.Button6Indicator.InnerLineWeight = 0F;
         this.Button6Indicator.InnerSpacing = 0F;
         this.Button6Indicator.Location = new System.Drawing.Point(421, 63);
         this.Button6Indicator.Name = "Button6Indicator";
         this.Button6Indicator.OuterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
         this.Button6Indicator.OuterLineWeight = 1F;
         this.Button6Indicator.OuterSpacing = 1F;
         this.Button6Indicator.Size = new System.Drawing.Size(40, 40);
         this.Button6Indicator.TabIndex = 230;
         this.Button6Indicator.Text = "roundIndicator1";
         // 
         // Button8Indicator
         // 
         this.Button8Indicator.IndicatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.Button8Indicator.IndicatorLineWeight = 0F;
         this.Button8Indicator.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
         this.Button8Indicator.InnerLineWeight = 0F;
         this.Button8Indicator.InnerSpacing = 0F;
         this.Button8Indicator.Location = new System.Drawing.Point(421, 17);
         this.Button8Indicator.Name = "Button8Indicator";
         this.Button8Indicator.OuterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
         this.Button8Indicator.OuterLineWeight = 1F;
         this.Button8Indicator.OuterSpacing = 1F;
         this.Button8Indicator.Size = new System.Drawing.Size(40, 40);
         this.Button8Indicator.TabIndex = 231;
         this.Button8Indicator.Text = "roundIndicator1";
         // 
         // Button11Indicator
         // 
         this.Button11Indicator.IndicatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.Button11Indicator.IndicatorLineWeight = 0F;
         this.Button11Indicator.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
         this.Button11Indicator.InnerLineWeight = 0F;
         this.Button11Indicator.InnerSpacing = 0F;
         this.Button11Indicator.Location = new System.Drawing.Point(158, 340);
         this.Button11Indicator.Name = "Button11Indicator";
         this.Button11Indicator.OuterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
         this.Button11Indicator.OuterLineWeight = 1F;
         this.Button11Indicator.OuterSpacing = 1F;
         this.Button11Indicator.Size = new System.Drawing.Size(40, 40);
         this.Button11Indicator.TabIndex = 232;
         this.Button11Indicator.Text = "roundIndicator1";
         // 
         // Button12Indicator
         // 
         this.Button12Indicator.IndicatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.Button12Indicator.IndicatorLineWeight = 0F;
         this.Button12Indicator.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
         this.Button12Indicator.InnerLineWeight = 0F;
         this.Button12Indicator.InnerSpacing = 0F;
         this.Button12Indicator.Location = new System.Drawing.Point(275, 340);
         this.Button12Indicator.Name = "Button12Indicator";
         this.Button12Indicator.OuterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
         this.Button12Indicator.OuterLineWeight = 1F;
         this.Button12Indicator.OuterSpacing = 1F;
         this.Button12Indicator.Size = new System.Drawing.Size(40, 40);
         this.Button12Indicator.TabIndex = 233;
         this.Button12Indicator.Text = "roundIndicator1";
         // 
         // UpdateTimer
         // 
         this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
         // 
         // LogitechF310Form
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(523, 396);
         this.Controls.Add(this.MainPanel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "LogitechF310Form";
         this.Text = "LogitechF310Form";
         this.Shown += new System.EventHandler(this.LogitechF310Form_Shown);
         this.MainPanel.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private Controls.BaseButton BackButton;
      private Controls.BorderedPanel MainPanel;
      private Controls.RoundIndicator Button1Indicator;
      private Controls.RoundIndicator Button3Indicator;
      private Controls.RoundIndicator Button2Indicator;
      private Controls.RoundIndicator Button4Indicator;
      private Controls.TextPanel Axis1YPanel;
      private Controls.TextPanel Axis1XPanel;
      private Controls.TextPanel Axis2YPanel;
      private Controls.TextPanel Axis2XPanel;
      private Controls.RoundIndicator Button8Indicator;
      private Controls.RoundIndicator Button6Indicator;
      private Controls.RoundIndicator Button7Indicator;
      private Controls.RoundIndicator Button5Indicator;
      private Controls.RoundIndicator Button10Indicator;
      private Controls.RoundIndicator Button9Indicator;
      private Controls.TextPanel PovPanel;
      private Controls.RoundIndicator Button12Indicator;
      private Controls.RoundIndicator Button11Indicator;
      private System.Windows.Forms.Timer UpdateTimer;
   }
}