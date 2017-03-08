
namespace E4.BusSim
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
         this.DevicePanel = new System.Windows.Forms.Panel();
         this.AddDeviceButton = new System.Windows.Forms.Button();
         this.RemoveButton = new System.Windows.Forms.Button();
         this.ActivityButton = new System.Windows.Forms.Button();
         this.MainStatusStrip = new System.Windows.Forms.StatusStrip();
         this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
         this.TimeStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
         this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
         this.DeviceComboBox = new System.Windows.Forms.ComboBox();
         this.MoveUpButton = new System.Windows.Forms.Button();
         this.MoveDownButton = new System.Windows.Forms.Button();
         this.WriteButton = new System.Windows.Forms.Button();
         this.ReadButton = new System.Windows.Forms.Button();
         this.BusAInterfaceComboBox = new System.Windows.Forms.ComboBox();
         this.BusComboBox = new System.Windows.Forms.ComboBox();
         this.BusBInterfaceComboBox = new System.Windows.Forms.ComboBox();
         this.ClearButton = new System.Windows.Forms.Button();
         this.MainStatusStrip.SuspendLayout();
         this.SuspendLayout();
         // 
         // DevicePanel
         // 
         this.DevicePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.DevicePanel.AutoScroll = true;
         this.DevicePanel.BackColor = System.Drawing.SystemColors.ControlDark;
         this.DevicePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.DevicePanel.Location = new System.Drawing.Point(5, 5);
         this.DevicePanel.Name = "DevicePanel";
         this.DevicePanel.Size = new System.Drawing.Size(544, 203);
         this.DevicePanel.TabIndex = 0;
         this.DevicePanel.Scroll += new System.Windows.Forms.ScrollEventHandler(this.DevicePanel_Scroll);
         // 
         // AddDeviceButton
         // 
         this.AddDeviceButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.AddDeviceButton.Location = new System.Drawing.Point(263, 221);
         this.AddDeviceButton.Name = "AddDeviceButton";
         this.AddDeviceButton.Size = new System.Drawing.Size(56, 23);
         this.AddDeviceButton.TabIndex = 1;
         this.AddDeviceButton.Text = "Add";
         this.AddDeviceButton.UseVisualStyleBackColor = true;
         this.AddDeviceButton.Click += new System.EventHandler(this.AddDeviceButton_Click);
         // 
         // RemoveButton
         // 
         this.RemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.RemoveButton.Location = new System.Drawing.Point(263, 250);
         this.RemoveButton.Name = "RemoveButton";
         this.RemoveButton.Size = new System.Drawing.Size(56, 23);
         this.RemoveButton.TabIndex = 2;
         this.RemoveButton.Text = "Remove";
         this.RemoveButton.UseVisualStyleBackColor = true;
         this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
         // 
         // ActivityButton
         // 
         this.ActivityButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.ActivityButton.Location = new System.Drawing.Point(29, 250);
         this.ActivityButton.Name = "ActivityButton";
         this.ActivityButton.Size = new System.Drawing.Size(88, 23);
         this.ActivityButton.TabIndex = 3;
         this.ActivityButton.Text = "Activity";
         this.ActivityButton.UseVisualStyleBackColor = true;
         this.ActivityButton.Click += new System.EventHandler(this.ActivityButton_Click);
         // 
         // MainStatusStrip
         // 
         this.MainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel,
            this.TimeStatusLabel});
         this.MainStatusStrip.Location = new System.Drawing.Point(0, 276);
         this.MainStatusStrip.Name = "MainStatusStrip";
         this.MainStatusStrip.Size = new System.Drawing.Size(552, 22);
         this.MainStatusStrip.TabIndex = 4;
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
         this.StatusLabel.Size = new System.Drawing.Size(406, 17);
         this.StatusLabel.Spring = true;
         this.StatusLabel.Text = "Status";
         this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // TimeStatusLabel
         // 
         this.TimeStatusLabel.AutoSize = false;
         this.TimeStatusLabel.Name = "TimeStatusLabel";
         this.TimeStatusLabel.Size = new System.Drawing.Size(100, 17);
         this.TimeStatusLabel.Text = "00:00:00.000";
         // 
         // UpdateTimer
         // 
         this.UpdateTimer.Interval = 1;
         this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
         // 
         // DeviceComboBox
         // 
         this.DeviceComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.DeviceComboBox.FormattingEnabled = true;
         this.DeviceComboBox.Location = new System.Drawing.Point(325, 223);
         this.DeviceComboBox.Name = "DeviceComboBox";
         this.DeviceComboBox.Size = new System.Drawing.Size(168, 21);
         this.DeviceComboBox.TabIndex = 5;
         // 
         // MoveUpButton
         // 
         this.MoveUpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.MoveUpButton.Location = new System.Drawing.Point(207, 221);
         this.MoveUpButton.Name = "MoveUpButton";
         this.MoveUpButton.Size = new System.Drawing.Size(50, 23);
         this.MoveUpButton.TabIndex = 6;
         this.MoveUpButton.Text = "Up";
         this.MoveUpButton.UseVisualStyleBackColor = true;
         this.MoveUpButton.Click += new System.EventHandler(this.MoveUpButton_Click);
         // 
         // MoveDownButton
         // 
         this.MoveDownButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.MoveDownButton.Location = new System.Drawing.Point(207, 250);
         this.MoveDownButton.Name = "MoveDownButton";
         this.MoveDownButton.Size = new System.Drawing.Size(50, 23);
         this.MoveDownButton.TabIndex = 7;
         this.MoveDownButton.Text = "Down";
         this.MoveDownButton.UseVisualStyleBackColor = true;
         this.MoveDownButton.Click += new System.EventHandler(this.MoveDownButton_Click);
         // 
         // WriteButton
         // 
         this.WriteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.WriteButton.Location = new System.Drawing.Point(151, 250);
         this.WriteButton.Name = "WriteButton";
         this.WriteButton.Size = new System.Drawing.Size(50, 23);
         this.WriteButton.TabIndex = 8;
         this.WriteButton.Text = "Write";
         this.WriteButton.UseVisualStyleBackColor = true;
         this.WriteButton.Click += new System.EventHandler(this.WriteButton_Click);
         // 
         // ReadButton
         // 
         this.ReadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.ReadButton.Location = new System.Drawing.Point(151, 221);
         this.ReadButton.Name = "ReadButton";
         this.ReadButton.Size = new System.Drawing.Size(50, 23);
         this.ReadButton.TabIndex = 9;
         this.ReadButton.Text = "Read";
         this.ReadButton.UseVisualStyleBackColor = true;
         this.ReadButton.Click += new System.EventHandler(this.ReadButton_Click);
         // 
         // BusAInterfaceComboBox
         // 
         this.BusAInterfaceComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.BusAInterfaceComboBox.FormattingEnabled = true;
         this.BusAInterfaceComboBox.Location = new System.Drawing.Point(12, 223);
         this.BusAInterfaceComboBox.Name = "BusAInterfaceComboBox";
         this.BusAInterfaceComboBox.Size = new System.Drawing.Size(57, 21);
         this.BusAInterfaceComboBox.TabIndex = 10;
         // 
         // BusComboBox
         // 
         this.BusComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.BusComboBox.FormattingEnabled = true;
         this.BusComboBox.Location = new System.Drawing.Point(499, 223);
         this.BusComboBox.Name = "BusComboBox";
         this.BusComboBox.Size = new System.Drawing.Size(33, 21);
         this.BusComboBox.TabIndex = 11;
         // 
         // BusBInterfaceComboBox
         // 
         this.BusBInterfaceComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.BusBInterfaceComboBox.FormattingEnabled = true;
         this.BusBInterfaceComboBox.Location = new System.Drawing.Point(75, 223);
         this.BusBInterfaceComboBox.Name = "BusBInterfaceComboBox";
         this.BusBInterfaceComboBox.Size = new System.Drawing.Size(57, 21);
         this.BusBInterfaceComboBox.TabIndex = 12;
         // 
         // ClearButton
         // 
         this.ClearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.ClearButton.Location = new System.Drawing.Point(325, 250);
         this.ClearButton.Name = "ClearButton";
         this.ClearButton.Size = new System.Drawing.Size(56, 23);
         this.ClearButton.TabIndex = 13;
         this.ClearButton.Text = "Clear";
         this.ClearButton.UseVisualStyleBackColor = true;
         this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
         // 
         // MainForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(552, 298);
         this.Controls.Add(this.ClearButton);
         this.Controls.Add(this.BusBInterfaceComboBox);
         this.Controls.Add(this.BusComboBox);
         this.Controls.Add(this.BusAInterfaceComboBox);
         this.Controls.Add(this.ReadButton);
         this.Controls.Add(this.WriteButton);
         this.Controls.Add(this.MoveDownButton);
         this.Controls.Add(this.MoveUpButton);
         this.Controls.Add(this.DeviceComboBox);
         this.Controls.Add(this.MainStatusStrip);
         this.Controls.Add(this.ActivityButton);
         this.Controls.Add(this.RemoveButton);
         this.Controls.Add(this.AddDeviceButton);
         this.Controls.Add(this.DevicePanel);
         this.MinimumSize = new System.Drawing.Size(560, 240);
         this.Name = "MainForm";
         this.Text = "E4 BUS Simulator";
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
         this.Shown += new System.EventHandler(this.MainForm_Shown);
         this.Resize += new System.EventHandler(this.MainForm_Resize);
         this.MainStatusStrip.ResumeLayout(false);
         this.MainStatusStrip.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Panel DevicePanel;
      private System.Windows.Forms.Button AddDeviceButton;
      private System.Windows.Forms.Button RemoveButton;
      private System.Windows.Forms.Button ActivityButton;
      private System.Windows.Forms.StatusStrip MainStatusStrip;
      private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
      private System.Windows.Forms.ToolStripStatusLabel TimeStatusLabel;
      private System.Windows.Forms.Timer UpdateTimer;
      private System.Windows.Forms.ComboBox DeviceComboBox;
      private System.Windows.Forms.Button MoveUpButton;
      private System.Windows.Forms.Button MoveDownButton;
      private System.Windows.Forms.Button WriteButton;
      private System.Windows.Forms.Button ReadButton;
      private System.Windows.Forms.ComboBox BusAInterfaceComboBox;
      private System.Windows.Forms.ComboBox BusComboBox;
      private System.Windows.Forms.ComboBox BusBInterfaceComboBox;
      private System.Windows.Forms.Button ClearButton;
   }
}

