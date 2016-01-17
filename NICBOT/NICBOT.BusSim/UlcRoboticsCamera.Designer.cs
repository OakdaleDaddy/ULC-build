namespace NICBOT.BusSim
{
   partial class UlcRoboticsCamera
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

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.BusIdTextBox = new System.Windows.Forms.TextBox();
         this.DeviceStateLabel = new System.Windows.Forms.Label();
         this.EnabledCheckBox = new System.Windows.Forms.CheckBox();
         this.DescriptionTextBox = new System.Windows.Forms.TextBox();
         this.NodeIdTextBox = new System.Windows.Forms.TextBox();
         this.label1 = new System.Windows.Forms.Label();
         this.CameraStateLabel = new System.Windows.Forms.Label();
         this.LightIntensityLabel = new System.Windows.Forms.Label();
         this.LightLabel = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // BusIdTextBox
         // 
         this.BusIdTextBox.Location = new System.Drawing.Point(245, 4);
         this.BusIdTextBox.MaxLength = 3;
         this.BusIdTextBox.Name = "BusIdTextBox";
         this.BusIdTextBox.ReadOnly = true;
         this.BusIdTextBox.Size = new System.Drawing.Size(15, 20);
         this.BusIdTextBox.TabIndex = 124;
         this.BusIdTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // DeviceStateLabel
         // 
         this.DeviceStateLabel.BackColor = System.Drawing.SystemColors.Control;
         this.DeviceStateLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.DeviceStateLabel.Location = new System.Drawing.Point(264, 4);
         this.DeviceStateLabel.Name = "DeviceStateLabel";
         this.DeviceStateLabel.Size = new System.Drawing.Size(66, 20);
         this.DeviceStateLabel.TabIndex = 123;
         this.DeviceStateLabel.Text = "OFF";
         this.DeviceStateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // EnabledCheckBox
         // 
         this.EnabledCheckBox.AutoSize = true;
         this.EnabledCheckBox.Location = new System.Drawing.Point(5, 7);
         this.EnabledCheckBox.Name = "EnabledCheckBox";
         this.EnabledCheckBox.Size = new System.Drawing.Size(15, 14);
         this.EnabledCheckBox.TabIndex = 122;
         this.EnabledCheckBox.UseVisualStyleBackColor = true;
         // 
         // DescriptionTextBox
         // 
         this.DescriptionTextBox.Location = new System.Drawing.Point(24, 4);
         this.DescriptionTextBox.MaxLength = 65535;
         this.DescriptionTextBox.Name = "DescriptionTextBox";
         this.DescriptionTextBox.ReadOnly = true;
         this.DescriptionTextBox.Size = new System.Drawing.Size(150, 20);
         this.DescriptionTextBox.TabIndex = 121;
         this.DescriptionTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // NodeIdTextBox
         // 
         this.NodeIdTextBox.Location = new System.Drawing.Point(217, 4);
         this.NodeIdTextBox.MaxLength = 3;
         this.NodeIdTextBox.Name = "NodeIdTextBox";
         this.NodeIdTextBox.ReadOnly = true;
         this.NodeIdTextBox.Size = new System.Drawing.Size(25, 20);
         this.NodeIdTextBox.TabIndex = 119;
         this.NodeIdTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(180, 7);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(36, 13);
         this.label1.TabIndex = 120;
         this.label1.Text = "Node:";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // CameraStateLabel
         // 
         this.CameraStateLabel.BackColor = System.Drawing.SystemColors.Control;
         this.CameraStateLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.CameraStateLabel.Location = new System.Drawing.Point(336, 4);
         this.CameraStateLabel.Name = "CameraStateLabel";
         this.CameraStateLabel.Size = new System.Drawing.Size(46, 20);
         this.CameraStateLabel.TabIndex = 125;
         this.CameraStateLabel.Text = "OFF";
         this.CameraStateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // LightIntensityLabel
         // 
         this.LightIntensityLabel.BackColor = System.Drawing.SystemColors.Control;
         this.LightIntensityLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.LightIntensityLabel.Location = new System.Drawing.Point(388, 4);
         this.LightIntensityLabel.Name = "LightIntensityLabel";
         this.LightIntensityLabel.Size = new System.Drawing.Size(46, 20);
         this.LightIntensityLabel.TabIndex = 126;
         this.LightIntensityLabel.Text = "0";
         this.LightIntensityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // LightLabel
         // 
         this.LightLabel.BackColor = System.Drawing.Color.Black;
         this.LightLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.LightLabel.Location = new System.Drawing.Point(440, 4);
         this.LightLabel.Name = "LightLabel";
         this.LightLabel.Size = new System.Drawing.Size(46, 20);
         this.LightLabel.TabIndex = 127;
         this.LightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // UlcRoboticsCamera
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.Controls.Add(this.LightLabel);
         this.Controls.Add(this.LightIntensityLabel);
         this.Controls.Add(this.CameraStateLabel);
         this.Controls.Add(this.BusIdTextBox);
         this.Controls.Add(this.DeviceStateLabel);
         this.Controls.Add(this.EnabledCheckBox);
         this.Controls.Add(this.DescriptionTextBox);
         this.Controls.Add(this.NodeIdTextBox);
         this.Controls.Add(this.label1);
         this.Name = "UlcRoboticsCamera";
         this.Size = new System.Drawing.Size(542, 30);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.TextBox BusIdTextBox;
      private System.Windows.Forms.Label DeviceStateLabel;
      private System.Windows.Forms.CheckBox EnabledCheckBox;
      private System.Windows.Forms.TextBox DescriptionTextBox;
      private System.Windows.Forms.TextBox NodeIdTextBox;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label CameraStateLabel;
      private System.Windows.Forms.Label LightIntensityLabel;
      private System.Windows.Forms.Label LightLabel;
   }
}
