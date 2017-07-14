﻿namespace Weco.Ui
{
   partial class E4MainDeviceInformationForm
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
         this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
         this.MainPanel = new Controls.BorderedPanel();
         this.ClearWarningButton = new Controls.BaseButton();
         this.DeviceStatusTextBox = new System.Windows.Forms.TextBox();
         this.RestartButton = new Controls.BaseButton();
         this.DeviceTypeValuePanel = new Controls.TextPanel();
         this.label1 = new System.Windows.Forms.Label();
         this.TraceSelectPanel = new Controls.BorderedPanel();
         this.HbButton = new Controls.BaseButton();
         this.Tpdo4Button = new Controls.BaseButton();
         this.Rpdo4Button = new Controls.BaseButton();
         this.Tpdo3Button = new Controls.BaseButton();
         this.Rpdo3Button = new Controls.BaseButton();
         this.Tpdo2Button = new Controls.BaseButton();
         this.Rpdo2Button = new Controls.BaseButton();
         this.Tpdo1Button = new Controls.BaseButton();
         this.Rpdo1Button = new Controls.BaseButton();
         this.label5 = new System.Windows.Forms.Label();
         this.SdoButton = new Controls.BaseButton();
         this.NodeIdValuePanel = new Controls.TextPanel();
         this.label4 = new System.Windows.Forms.Label();
         this.LoggingNameValuePanel = new Controls.TextPanel();
         this.label3 = new System.Windows.Forms.Label();
         this.DeviceVersionValuePanel = new Controls.TextPanel();
         this.label2 = new System.Windows.Forms.Label();
         this.DeviceNameValuePanel = new Controls.TextPanel();
         this.label28 = new System.Windows.Forms.Label();
         this.TitleLabel = new System.Windows.Forms.Label();
         this.BackButton = new Controls.BaseButton();
         this.Tpdo8Button = new Controls.BaseButton();
         this.Tpdo7Button = new Controls.BaseButton();
         this.Tpdo6Button = new Controls.BaseButton();
         this.Tpdo5Button = new Controls.BaseButton();
         this.MainPanel.SuspendLayout();
         this.TraceSelectPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // UpdateTimer
         // 
         this.UpdateTimer.Interval = 50;
         this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
         // 
         // MainPanel
         // 
         this.MainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
         this.MainPanel.Controls.Add(this.ClearWarningButton);
         this.MainPanel.Controls.Add(this.DeviceStatusTextBox);
         this.MainPanel.Controls.Add(this.RestartButton);
         this.MainPanel.Controls.Add(this.DeviceTypeValuePanel);
         this.MainPanel.Controls.Add(this.label1);
         this.MainPanel.Controls.Add(this.TraceSelectPanel);
         this.MainPanel.Controls.Add(this.NodeIdValuePanel);
         this.MainPanel.Controls.Add(this.label4);
         this.MainPanel.Controls.Add(this.LoggingNameValuePanel);
         this.MainPanel.Controls.Add(this.label3);
         this.MainPanel.Controls.Add(this.DeviceVersionValuePanel);
         this.MainPanel.Controls.Add(this.label2);
         this.MainPanel.Controls.Add(this.DeviceNameValuePanel);
         this.MainPanel.Controls.Add(this.label28);
         this.MainPanel.Controls.Add(this.TitleLabel);
         this.MainPanel.Controls.Add(this.BackButton);
         this.MainPanel.EdgeWeight = 3;
         this.MainPanel.Location = new System.Drawing.Point(0, 0);
         this.MainPanel.Name = "MainPanel";
         this.MainPanel.Size = new System.Drawing.Size(642, 506);
         this.MainPanel.TabIndex = 0;
         // 
         // ClearWarningButton
         // 
         this.ClearWarningButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ClearWarningButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ClearWarningButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ClearWarningButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.ClearWarningButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.ClearWarningButton.Location = new System.Drawing.Point(166, 421);
         this.ClearWarningButton.Name = "ClearWarningButton";
         this.ClearWarningButton.Size = new System.Drawing.Size(107, 67);
         this.ClearWarningButton.TabIndex = 184;
         this.ClearWarningButton.Text = "CLEAR WARNING";
         this.ClearWarningButton.UseVisualStyleBackColor = false;
         this.ClearWarningButton.Visible = false;
         this.ClearWarningButton.Click += new System.EventHandler(this.ClearWarningButton_Click);
         // 
         // DeviceStatusTextBox
         // 
         this.DeviceStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.DeviceStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.DeviceStatusTextBox.ForeColor = System.Drawing.Color.Black;
         this.DeviceStatusTextBox.Location = new System.Drawing.Point(129, 199);
         this.DeviceStatusTextBox.Name = "DeviceStatusTextBox";
         this.DeviceStatusTextBox.ReadOnly = true;
         this.DeviceStatusTextBox.Size = new System.Drawing.Size(380, 26);
         this.DeviceStatusTextBox.TabIndex = 183;
         this.DeviceStatusTextBox.Text = "not connected";
         this.DeviceStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // RestartButton
         // 
         this.RestartButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RestartButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.RestartButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.RestartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.RestartButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.RestartButton.Location = new System.Drawing.Point(43, 421);
         this.RestartButton.Name = "RestartButton";
         this.RestartButton.Size = new System.Drawing.Size(107, 67);
         this.RestartButton.TabIndex = 182;
         this.RestartButton.Text = "RESTART";
         this.RestartButton.UseVisualStyleBackColor = false;
         this.RestartButton.Click += new System.EventHandler(this.RestartButton_Click);
         // 
         // DeviceTypeValuePanel
         // 
         this.DeviceTypeValuePanel.BackColor = System.Drawing.Color.Black;
         this.DeviceTypeValuePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.DeviceTypeValuePanel.Enabled = false;
         this.DeviceTypeValuePanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DeviceTypeValuePanel.ForeColor = System.Drawing.Color.White;
         this.DeviceTypeValuePanel.HoldTimeoutEnable = false;
         this.DeviceTypeValuePanel.HoldTimeoutInterval = 100;
         this.DeviceTypeValuePanel.Location = new System.Drawing.Point(27, 147);
         this.DeviceTypeValuePanel.Name = "DeviceTypeValuePanel";
         this.DeviceTypeValuePanel.Size = new System.Drawing.Size(190, 42);
         this.DeviceTypeValuePanel.TabIndex = 181;
         this.DeviceTypeValuePanel.ValueText = "feeder tf-motor";
         this.DeviceTypeValuePanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label1
         // 
         this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label1.Location = new System.Drawing.Point(27, 124);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(190, 20);
         this.label1.TabIndex = 180;
         this.label1.Text = "DEVICE TYPE";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // TraceSelectPanel
         // 
         this.TraceSelectPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(80)))), ((int)(((byte)(96)))));
         this.TraceSelectPanel.Controls.Add(this.Tpdo8Button);
         this.TraceSelectPanel.Controls.Add(this.Tpdo7Button);
         this.TraceSelectPanel.Controls.Add(this.Tpdo6Button);
         this.TraceSelectPanel.Controls.Add(this.Tpdo5Button);
         this.TraceSelectPanel.Controls.Add(this.HbButton);
         this.TraceSelectPanel.Controls.Add(this.Tpdo4Button);
         this.TraceSelectPanel.Controls.Add(this.Rpdo4Button);
         this.TraceSelectPanel.Controls.Add(this.Tpdo3Button);
         this.TraceSelectPanel.Controls.Add(this.Rpdo3Button);
         this.TraceSelectPanel.Controls.Add(this.Tpdo2Button);
         this.TraceSelectPanel.Controls.Add(this.Rpdo2Button);
         this.TraceSelectPanel.Controls.Add(this.Tpdo1Button);
         this.TraceSelectPanel.Controls.Add(this.Rpdo1Button);
         this.TraceSelectPanel.Controls.Add(this.label5);
         this.TraceSelectPanel.Controls.Add(this.SdoButton);
         this.TraceSelectPanel.EdgeWeight = 2;
         this.TraceSelectPanel.Location = new System.Drawing.Point(28, 235);
         this.TraceSelectPanel.Name = "TraceSelectPanel";
         this.TraceSelectPanel.Size = new System.Drawing.Size(587, 180);
         this.TraceSelectPanel.TabIndex = 179;
         // 
         // HbButton
         // 
         this.HbButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.HbButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.HbButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.HbButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.HbButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.HbButton.Location = new System.Drawing.Point(10, 82);
         this.HbButton.Name = "HbButton";
         this.HbButton.Size = new System.Drawing.Size(107, 40);
         this.HbButton.TabIndex = 187;
         this.HbButton.Text = "HB";
         this.HbButton.UseVisualStyleBackColor = false;
         this.HbButton.Click += new System.EventHandler(this.HbButton_Click);
         // 
         // Tpdo4Button
         // 
         this.Tpdo4Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.Tpdo4Button.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.Tpdo4Button.DisabledForeColor = System.Drawing.Color.Silver;
         this.Tpdo4Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.Tpdo4Button.HoldArrorColor = System.Drawing.Color.Gray;
         this.Tpdo4Button.Location = new System.Drawing.Point(470, 82);
         this.Tpdo4Button.Name = "Tpdo4Button";
         this.Tpdo4Button.Size = new System.Drawing.Size(107, 40);
         this.Tpdo4Button.TabIndex = 186;
         this.Tpdo4Button.Text = "TPDO4";
         this.Tpdo4Button.UseVisualStyleBackColor = false;
         this.Tpdo4Button.Click += new System.EventHandler(this.Tpdo4Button_Click);
         // 
         // Rpdo4Button
         // 
         this.Rpdo4Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.Rpdo4Button.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.Rpdo4Button.DisabledForeColor = System.Drawing.Color.Silver;
         this.Rpdo4Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.Rpdo4Button.HoldArrorColor = System.Drawing.Color.Gray;
         this.Rpdo4Button.Location = new System.Drawing.Point(470, 34);
         this.Rpdo4Button.Name = "Rpdo4Button";
         this.Rpdo4Button.Size = new System.Drawing.Size(107, 40);
         this.Rpdo4Button.TabIndex = 185;
         this.Rpdo4Button.Text = "RPDO4";
         this.Rpdo4Button.UseVisualStyleBackColor = false;
         this.Rpdo4Button.Click += new System.EventHandler(this.Rpdo4Button_Click);
         // 
         // Tpdo3Button
         // 
         this.Tpdo3Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.Tpdo3Button.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.Tpdo3Button.DisabledForeColor = System.Drawing.Color.Silver;
         this.Tpdo3Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.Tpdo3Button.HoldArrorColor = System.Drawing.Color.Gray;
         this.Tpdo3Button.Location = new System.Drawing.Point(355, 82);
         this.Tpdo3Button.Name = "Tpdo3Button";
         this.Tpdo3Button.Size = new System.Drawing.Size(107, 40);
         this.Tpdo3Button.TabIndex = 184;
         this.Tpdo3Button.Text = "TPDO3";
         this.Tpdo3Button.UseVisualStyleBackColor = false;
         this.Tpdo3Button.Click += new System.EventHandler(this.Tpdo3Button_Click);
         // 
         // Rpdo3Button
         // 
         this.Rpdo3Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.Rpdo3Button.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.Rpdo3Button.DisabledForeColor = System.Drawing.Color.Silver;
         this.Rpdo3Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.Rpdo3Button.HoldArrorColor = System.Drawing.Color.Gray;
         this.Rpdo3Button.Location = new System.Drawing.Point(355, 34);
         this.Rpdo3Button.Name = "Rpdo3Button";
         this.Rpdo3Button.Size = new System.Drawing.Size(107, 40);
         this.Rpdo3Button.TabIndex = 183;
         this.Rpdo3Button.Text = "RPDO3";
         this.Rpdo3Button.UseVisualStyleBackColor = false;
         this.Rpdo3Button.Click += new System.EventHandler(this.Rpdo3Button_Click);
         // 
         // Tpdo2Button
         // 
         this.Tpdo2Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.Tpdo2Button.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.Tpdo2Button.DisabledForeColor = System.Drawing.Color.Silver;
         this.Tpdo2Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.Tpdo2Button.HoldArrorColor = System.Drawing.Color.Gray;
         this.Tpdo2Button.Location = new System.Drawing.Point(240, 82);
         this.Tpdo2Button.Name = "Tpdo2Button";
         this.Tpdo2Button.Size = new System.Drawing.Size(107, 40);
         this.Tpdo2Button.TabIndex = 182;
         this.Tpdo2Button.Text = "TPDO2";
         this.Tpdo2Button.UseVisualStyleBackColor = false;
         this.Tpdo2Button.Click += new System.EventHandler(this.Tpdo2Button_Click);
         // 
         // Rpdo2Button
         // 
         this.Rpdo2Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.Rpdo2Button.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.Rpdo2Button.DisabledForeColor = System.Drawing.Color.Silver;
         this.Rpdo2Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.Rpdo2Button.HoldArrorColor = System.Drawing.Color.Gray;
         this.Rpdo2Button.Location = new System.Drawing.Point(240, 34);
         this.Rpdo2Button.Name = "Rpdo2Button";
         this.Rpdo2Button.Size = new System.Drawing.Size(107, 40);
         this.Rpdo2Button.TabIndex = 181;
         this.Rpdo2Button.Text = "RPDO2";
         this.Rpdo2Button.UseVisualStyleBackColor = false;
         this.Rpdo2Button.Click += new System.EventHandler(this.Rpdo2Button_Click);
         // 
         // Tpdo1Button
         // 
         this.Tpdo1Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.Tpdo1Button.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.Tpdo1Button.DisabledForeColor = System.Drawing.Color.Silver;
         this.Tpdo1Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.Tpdo1Button.HoldArrorColor = System.Drawing.Color.Gray;
         this.Tpdo1Button.Location = new System.Drawing.Point(125, 82);
         this.Tpdo1Button.Name = "Tpdo1Button";
         this.Tpdo1Button.Size = new System.Drawing.Size(107, 40);
         this.Tpdo1Button.TabIndex = 180;
         this.Tpdo1Button.Text = "TPDO1";
         this.Tpdo1Button.UseVisualStyleBackColor = false;
         this.Tpdo1Button.Click += new System.EventHandler(this.Tpdo1Button_Click);
         // 
         // Rpdo1Button
         // 
         this.Rpdo1Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.Rpdo1Button.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.Rpdo1Button.DisabledForeColor = System.Drawing.Color.Silver;
         this.Rpdo1Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.Rpdo1Button.HoldArrorColor = System.Drawing.Color.Gray;
         this.Rpdo1Button.Location = new System.Drawing.Point(125, 34);
         this.Rpdo1Button.Name = "Rpdo1Button";
         this.Rpdo1Button.Size = new System.Drawing.Size(107, 40);
         this.Rpdo1Button.TabIndex = 179;
         this.Rpdo1Button.Text = "RPDO1";
         this.Rpdo1Button.UseVisualStyleBackColor = false;
         this.Rpdo1Button.Click += new System.EventHandler(this.Rpdo1Button_Click);
         // 
         // label5
         // 
         this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
         this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label5.ForeColor = System.Drawing.Color.Gainsboro;
         this.label5.Location = new System.Drawing.Point(2, 2);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(583, 24);
         this.label5.TabIndex = 154;
         this.label5.Text = "LOG SELECTS";
         this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // SdoButton
         // 
         this.SdoButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SdoButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.SdoButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.SdoButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.SdoButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.SdoButton.Location = new System.Drawing.Point(10, 34);
         this.SdoButton.Name = "SdoButton";
         this.SdoButton.Size = new System.Drawing.Size(107, 40);
         this.SdoButton.TabIndex = 178;
         this.SdoButton.Text = "SDO";
         this.SdoButton.UseVisualStyleBackColor = false;
         this.SdoButton.Click += new System.EventHandler(this.SdoButton_Click);
         // 
         // NodeIdValuePanel
         // 
         this.NodeIdValuePanel.BackColor = System.Drawing.Color.Black;
         this.NodeIdValuePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.NodeIdValuePanel.Enabled = false;
         this.NodeIdValuePanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.NodeIdValuePanel.ForeColor = System.Drawing.Color.White;
         this.NodeIdValuePanel.HoldTimeoutEnable = false;
         this.NodeIdValuePanel.HoldTimeoutInterval = 100;
         this.NodeIdValuePanel.Location = new System.Drawing.Point(423, 147);
         this.NodeIdValuePanel.Name = "NodeIdValuePanel";
         this.NodeIdValuePanel.Size = new System.Drawing.Size(190, 42);
         this.NodeIdValuePanel.TabIndex = 177;
         this.NodeIdValuePanel.ValueText = "127";
         this.NodeIdValuePanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label4
         // 
         this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label4.Location = new System.Drawing.Point(423, 124);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(190, 20);
         this.label4.TabIndex = 176;
         this.label4.Text = "NODE ID";
         this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // LoggingNameValuePanel
         // 
         this.LoggingNameValuePanel.BackColor = System.Drawing.Color.Black;
         this.LoggingNameValuePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.LoggingNameValuePanel.Enabled = false;
         this.LoggingNameValuePanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LoggingNameValuePanel.ForeColor = System.Drawing.Color.White;
         this.LoggingNameValuePanel.HoldTimeoutEnable = false;
         this.LoggingNameValuePanel.HoldTimeoutInterval = 100;
         this.LoggingNameValuePanel.Location = new System.Drawing.Point(225, 147);
         this.LoggingNameValuePanel.Name = "LoggingNameValuePanel";
         this.LoggingNameValuePanel.Size = new System.Drawing.Size(190, 42);
         this.LoggingNameValuePanel.TabIndex = 175;
         this.LoggingNameValuePanel.ValueText = "feeder tf-motor";
         this.LoggingNameValuePanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label3
         // 
         this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label3.Location = new System.Drawing.Point(225, 124);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(190, 20);
         this.label3.TabIndex = 174;
         this.label3.Text = "LOGGING NAME";
         this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // DeviceVersionValuePanel
         // 
         this.DeviceVersionValuePanel.BackColor = System.Drawing.Color.Black;
         this.DeviceVersionValuePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.DeviceVersionValuePanel.Enabled = false;
         this.DeviceVersionValuePanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DeviceVersionValuePanel.ForeColor = System.Drawing.Color.White;
         this.DeviceVersionValuePanel.HoldTimeoutEnable = false;
         this.DeviceVersionValuePanel.HoldTimeoutInterval = 100;
         this.DeviceVersionValuePanel.Location = new System.Drawing.Point(325, 79);
         this.DeviceVersionValuePanel.Name = "DeviceVersionValuePanel";
         this.DeviceVersionValuePanel.Size = new System.Drawing.Size(290, 42);
         this.DeviceVersionValuePanel.TabIndex = 173;
         this.DeviceVersionValuePanel.ValueText = "v1.0";
         this.DeviceVersionValuePanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label2
         // 
         this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label2.Location = new System.Drawing.Point(325, 56);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(290, 20);
         this.label2.TabIndex = 172;
         this.label2.Text = "DEVICE VERSION";
         this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // DeviceNameValuePanel
         // 
         this.DeviceNameValuePanel.BackColor = System.Drawing.Color.Black;
         this.DeviceNameValuePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.DeviceNameValuePanel.Enabled = false;
         this.DeviceNameValuePanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DeviceNameValuePanel.ForeColor = System.Drawing.Color.White;
         this.DeviceNameValuePanel.HoldTimeoutEnable = false;
         this.DeviceNameValuePanel.HoldTimeoutInterval = 100;
         this.DeviceNameValuePanel.Location = new System.Drawing.Point(27, 79);
         this.DeviceNameValuePanel.Name = "DeviceNameValuePanel";
         this.DeviceNameValuePanel.Size = new System.Drawing.Size(290, 42);
         this.DeviceNameValuePanel.TabIndex = 171;
         this.DeviceNameValuePanel.ValueText = "Whistle";
         this.DeviceNameValuePanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label28
         // 
         this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label28.Location = new System.Drawing.Point(27, 56);
         this.label28.Name = "label28";
         this.label28.Size = new System.Drawing.Size(290, 20);
         this.label28.TabIndex = 170;
         this.label28.Text = "DEVICE NAME";
         this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // TitleLabel
         // 
         this.TitleLabel.BackColor = System.Drawing.Color.Teal;
         this.TitleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TitleLabel.ForeColor = System.Drawing.Color.Gainsboro;
         this.TitleLabel.Location = new System.Drawing.Point(16, 16);
         this.TitleLabel.Name = "TitleLabel";
         this.TitleLabel.Size = new System.Drawing.Size(610, 36);
         this.TitleLabel.TabIndex = 169;
         this.TitleLabel.Text = "TITLE";
         this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.TitleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitleLabel_MouseDown);
         this.TitleLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TitleLabel_MouseMove);
         this.TitleLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TitleLabel_MouseUp);
         // 
         // BackButton
         // 
         this.BackButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BackButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BackButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.BackButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.BackButton.Location = new System.Drawing.Point(493, 421);
         this.BackButton.Name = "BackButton";
         this.BackButton.Size = new System.Drawing.Size(107, 67);
         this.BackButton.TabIndex = 167;
         this.BackButton.Text = "BACK";
         this.BackButton.UseVisualStyleBackColor = false;
         this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
         // 
         // Tpdo8Button
         // 
         this.Tpdo8Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.Tpdo8Button.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.Tpdo8Button.DisabledForeColor = System.Drawing.Color.Silver;
         this.Tpdo8Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.Tpdo8Button.HoldArrorColor = System.Drawing.Color.Gray;
         this.Tpdo8Button.Location = new System.Drawing.Point(470, 130);
         this.Tpdo8Button.Name = "Tpdo8Button";
         this.Tpdo8Button.Size = new System.Drawing.Size(107, 40);
         this.Tpdo8Button.TabIndex = 191;
         this.Tpdo8Button.Text = "TPDO8";
         this.Tpdo8Button.UseVisualStyleBackColor = false;
         this.Tpdo8Button.Click += new System.EventHandler(this.Tpdo8Button_Click);
         // 
         // Tpdo7Button
         // 
         this.Tpdo7Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.Tpdo7Button.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.Tpdo7Button.DisabledForeColor = System.Drawing.Color.Silver;
         this.Tpdo7Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.Tpdo7Button.HoldArrorColor = System.Drawing.Color.Gray;
         this.Tpdo7Button.Location = new System.Drawing.Point(355, 130);
         this.Tpdo7Button.Name = "Tpdo7Button";
         this.Tpdo7Button.Size = new System.Drawing.Size(107, 40);
         this.Tpdo7Button.TabIndex = 190;
         this.Tpdo7Button.Text = "TPDO7";
         this.Tpdo7Button.UseVisualStyleBackColor = false;
         this.Tpdo7Button.Click += new System.EventHandler(this.Tpdo7Button_Click);
         // 
         // Tpdo6Button
         // 
         this.Tpdo6Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.Tpdo6Button.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.Tpdo6Button.DisabledForeColor = System.Drawing.Color.Silver;
         this.Tpdo6Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.Tpdo6Button.HoldArrorColor = System.Drawing.Color.Gray;
         this.Tpdo6Button.Location = new System.Drawing.Point(240, 130);
         this.Tpdo6Button.Name = "Tpdo6Button";
         this.Tpdo6Button.Size = new System.Drawing.Size(107, 40);
         this.Tpdo6Button.TabIndex = 189;
         this.Tpdo6Button.Text = "TPDO6";
         this.Tpdo6Button.UseVisualStyleBackColor = false;
         this.Tpdo6Button.Click += new System.EventHandler(this.Tpdo6Button_Click);
         // 
         // Tpdo5Button
         // 
         this.Tpdo5Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.Tpdo5Button.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.Tpdo5Button.DisabledForeColor = System.Drawing.Color.Silver;
         this.Tpdo5Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.Tpdo5Button.HoldArrorColor = System.Drawing.Color.Gray;
         this.Tpdo5Button.Location = new System.Drawing.Point(125, 130);
         this.Tpdo5Button.Name = "Tpdo5Button";
         this.Tpdo5Button.Size = new System.Drawing.Size(107, 40);
         this.Tpdo5Button.TabIndex = 188;
         this.Tpdo5Button.Text = "TPDO5";
         this.Tpdo5Button.UseVisualStyleBackColor = false;
         this.Tpdo5Button.Click += new System.EventHandler(this.Tpdo5Button_Click);
         // 
         // E4MainDeviceInformationForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.ClientSize = new System.Drawing.Size(642, 506);
         this.Controls.Add(this.MainPanel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "E4MainDeviceInformationForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "CANDeviceInformationForm";
         this.Shown += new System.EventHandler(this.CANDeviceInformationForm_Shown);
         this.MainPanel.ResumeLayout(false);
         this.MainPanel.PerformLayout();
         this.TraceSelectPanel.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private Controls.BorderedPanel MainPanel;
      private Controls.BaseButton BackButton;
      private System.Windows.Forms.Label TitleLabel;
      private Controls.TextPanel LoggingNameValuePanel;
      private System.Windows.Forms.Label label3;
      private Controls.TextPanel DeviceVersionValuePanel;
      private System.Windows.Forms.Label label2;
      private Controls.TextPanel DeviceNameValuePanel;
      private System.Windows.Forms.Label label28;
      private Controls.BaseButton SdoButton;
      private Controls.TextPanel NodeIdValuePanel;
      private System.Windows.Forms.Label label4;
      private Controls.BorderedPanel TraceSelectPanel;
      private System.Windows.Forms.Label label5;
      private Controls.BaseButton Tpdo3Button;
      private Controls.BaseButton Rpdo3Button;
      private Controls.BaseButton Tpdo2Button;
      private Controls.BaseButton Rpdo2Button;
      private Controls.BaseButton Tpdo1Button;
      private Controls.BaseButton Rpdo1Button;
      private Controls.BaseButton Tpdo4Button;
      private Controls.BaseButton Rpdo4Button;
      private Controls.BaseButton HbButton;
      private Controls.TextPanel DeviceTypeValuePanel;
      private System.Windows.Forms.Label label1;
      private Controls.BaseButton RestartButton;
      private System.Windows.Forms.TextBox DeviceStatusTextBox;
      private System.Windows.Forms.Timer UpdateTimer;
      private Controls.BaseButton ClearWarningButton;
      private Controls.BaseButton Tpdo8Button;
      private Controls.BaseButton Tpdo7Button;
      private Controls.BaseButton Tpdo6Button;
      private Controls.BaseButton Tpdo5Button;
   }
}