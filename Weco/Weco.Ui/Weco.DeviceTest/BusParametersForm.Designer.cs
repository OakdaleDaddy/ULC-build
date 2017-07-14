
namespace Weco.DeviceTest
{
   partial class BusParametersForm
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
         this.BaudComboBox = new System.Windows.Forms.ComboBox();
         this.label1 = new System.Windows.Forms.Label();
         this.TransmitAddressTextBox = new System.Windows.Forms.TextBox();
         this.TransmitPortTextBox = new System.Windows.Forms.TextBox();
         this.label3 = new System.Windows.Forms.Label();
         this.ReceiveAddressTextBox = new System.Windows.Forms.TextBox();
         this.ReceivePortTextBox = new System.Windows.Forms.TextBox();
         this.OkButton = new System.Windows.Forms.Button();
         this.CancelAButton = new System.Windows.Forms.Button();
         this.IpGatewayParameterPanel = new System.Windows.Forms.Panel();
         this.RateParameterPanel = new System.Windows.Forms.Panel();
         this.label2 = new System.Windows.Forms.Label();
         this.EntryPanel = new System.Windows.Forms.Panel();
         this.UsbParameterPanel = new System.Windows.Forms.Panel();
         this.label4 = new System.Windows.Forms.Label();
         this.UsbBaudComboBox = new System.Windows.Forms.ComboBox();
         this.label5 = new System.Windows.Forms.Label();
         this.UsbNodeTextBox = new System.Windows.Forms.TextBox();
         this.UsbUseNodeCheckBox = new System.Windows.Forms.CheckBox();
         this.IpGatewayParameterPanel.SuspendLayout();
         this.RateParameterPanel.SuspendLayout();
         this.EntryPanel.SuspendLayout();
         this.UsbParameterPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // BaudComboBox
         // 
         this.BaudComboBox.FormattingEnabled = true;
         this.BaudComboBox.Location = new System.Drawing.Point(35, 3);
         this.BaudComboBox.Name = "BaudComboBox";
         this.BaudComboBox.Size = new System.Drawing.Size(64, 21);
         this.BaudComboBox.TabIndex = 27;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(3, 6);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(47, 13);
         this.label1.TabIndex = 53;
         this.label1.Text = "Transmit";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // TransmitAddressTextBox
         // 
         this.TransmitAddressTextBox.Location = new System.Drawing.Point(52, 3);
         this.TransmitAddressTextBox.Name = "TransmitAddressTextBox";
         this.TransmitAddressTextBox.Size = new System.Drawing.Size(87, 20);
         this.TransmitAddressTextBox.TabIndex = 52;
         // 
         // TransmitPortTextBox
         // 
         this.TransmitPortTextBox.Location = new System.Drawing.Point(145, 3);
         this.TransmitPortTextBox.Name = "TransmitPortTextBox";
         this.TransmitPortTextBox.Size = new System.Drawing.Size(55, 20);
         this.TransmitPortTextBox.TabIndex = 51;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(3, 32);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(47, 13);
         this.label3.TabIndex = 57;
         this.label3.Text = "Receive";
         this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // ReceiveAddressTextBox
         // 
         this.ReceiveAddressTextBox.Location = new System.Drawing.Point(52, 29);
         this.ReceiveAddressTextBox.Name = "ReceiveAddressTextBox";
         this.ReceiveAddressTextBox.Size = new System.Drawing.Size(87, 20);
         this.ReceiveAddressTextBox.TabIndex = 56;
         // 
         // ReceivePortTextBox
         // 
         this.ReceivePortTextBox.Location = new System.Drawing.Point(145, 29);
         this.ReceivePortTextBox.Name = "ReceivePortTextBox";
         this.ReceivePortTextBox.Size = new System.Drawing.Size(55, 20);
         this.ReceivePortTextBox.TabIndex = 55;
         // 
         // OkButton
         // 
         this.OkButton.Location = new System.Drawing.Point(64, 96);
         this.OkButton.Name = "OkButton";
         this.OkButton.Size = new System.Drawing.Size(55, 23);
         this.OkButton.TabIndex = 58;
         this.OkButton.Text = "OK";
         this.OkButton.UseVisualStyleBackColor = true;
         this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
         // 
         // CancelAButton
         // 
         this.CancelAButton.Location = new System.Drawing.Point(125, 96);
         this.CancelAButton.Name = "CancelAButton";
         this.CancelAButton.Size = new System.Drawing.Size(55, 23);
         this.CancelAButton.TabIndex = 59;
         this.CancelAButton.Text = "Cancel";
         this.CancelAButton.UseVisualStyleBackColor = true;
         this.CancelAButton.Click += new System.EventHandler(this.CancelAButton_Click);
         // 
         // IpGatewayParameterPanel
         // 
         this.IpGatewayParameterPanel.Controls.Add(this.TransmitAddressTextBox);
         this.IpGatewayParameterPanel.Controls.Add(this.TransmitPortTextBox);
         this.IpGatewayParameterPanel.Controls.Add(this.label3);
         this.IpGatewayParameterPanel.Controls.Add(this.label1);
         this.IpGatewayParameterPanel.Controls.Add(this.ReceiveAddressTextBox);
         this.IpGatewayParameterPanel.Controls.Add(this.ReceivePortTextBox);
         this.IpGatewayParameterPanel.Location = new System.Drawing.Point(0, 0);
         this.IpGatewayParameterPanel.Name = "IpGatewayParameterPanel";
         this.IpGatewayParameterPanel.Size = new System.Drawing.Size(206, 56);
         this.IpGatewayParameterPanel.TabIndex = 60;
         // 
         // RateParameterPanel
         // 
         this.RateParameterPanel.Controls.Add(this.label2);
         this.RateParameterPanel.Controls.Add(this.BaudComboBox);
         this.RateParameterPanel.Location = new System.Drawing.Point(212, 0);
         this.RateParameterPanel.Name = "RateParameterPanel";
         this.RateParameterPanel.Size = new System.Drawing.Size(111, 56);
         this.RateParameterPanel.TabIndex = 61;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(3, 6);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(30, 13);
         this.label2.TabIndex = 54;
         this.label2.Text = "Rate";
         this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // EntryPanel
         // 
         this.EntryPanel.AutoScroll = true;
         this.EntryPanel.Controls.Add(this.UsbParameterPanel);
         this.EntryPanel.Controls.Add(this.IpGatewayParameterPanel);
         this.EntryPanel.Controls.Add(this.RateParameterPanel);
         this.EntryPanel.Location = new System.Drawing.Point(0, 12);
         this.EntryPanel.Name = "EntryPanel";
         this.EntryPanel.Size = new System.Drawing.Size(508, 78);
         this.EntryPanel.TabIndex = 62;
         // 
         // UsbParameterPanel
         // 
         this.UsbParameterPanel.Controls.Add(this.UsbUseNodeCheckBox);
         this.UsbParameterPanel.Controls.Add(this.UsbNodeTextBox);
         this.UsbParameterPanel.Controls.Add(this.label5);
         this.UsbParameterPanel.Controls.Add(this.label4);
         this.UsbParameterPanel.Controls.Add(this.UsbBaudComboBox);
         this.UsbParameterPanel.Location = new System.Drawing.Point(329, 0);
         this.UsbParameterPanel.Name = "UsbParameterPanel";
         this.UsbParameterPanel.Size = new System.Drawing.Size(121, 75);
         this.UsbParameterPanel.TabIndex = 62;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(6, 6);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(30, 13);
         this.label4.TabIndex = 54;
         this.label4.Text = "Rate";
         this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // UsbBaudComboBox
         // 
         this.UsbBaudComboBox.FormattingEnabled = true;
         this.UsbBaudComboBox.Location = new System.Drawing.Point(38, 3);
         this.UsbBaudComboBox.Name = "UsbBaudComboBox";
         this.UsbBaudComboBox.Size = new System.Drawing.Size(64, 21);
         this.UsbBaudComboBox.TabIndex = 27;
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(3, 32);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(33, 13);
         this.label5.TabIndex = 55;
         this.label5.Text = "Node";
         this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // UsbNodeTextBox
         // 
         this.UsbNodeTextBox.Location = new System.Drawing.Point(38, 29);
         this.UsbNodeTextBox.Name = "UsbNodeTextBox";
         this.UsbNodeTextBox.Size = new System.Drawing.Size(40, 20);
         this.UsbNodeTextBox.TabIndex = 56;
         // 
         // UsbUseNodeCheckBox
         // 
         this.UsbUseNodeCheckBox.AutoSize = true;
         this.UsbUseNodeCheckBox.Location = new System.Drawing.Point(84, 31);
         this.UsbUseNodeCheckBox.Name = "UsbUseNodeCheckBox";
         this.UsbUseNodeCheckBox.Size = new System.Drawing.Size(15, 14);
         this.UsbUseNodeCheckBox.TabIndex = 57;
         this.UsbUseNodeCheckBox.UseVisualStyleBackColor = true;
         // 
         // BusParametersForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(234, 129);
         this.Controls.Add(this.EntryPanel);
         this.Controls.Add(this.CancelAButton);
         this.Controls.Add(this.OkButton);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.Name = "BusParametersForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "BusConfigurationForm";
         this.IpGatewayParameterPanel.ResumeLayout(false);
         this.IpGatewayParameterPanel.PerformLayout();
         this.RateParameterPanel.ResumeLayout(false);
         this.RateParameterPanel.PerformLayout();
         this.EntryPanel.ResumeLayout(false);
         this.UsbParameterPanel.ResumeLayout(false);
         this.UsbParameterPanel.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.ComboBox BaudComboBox;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox TransmitAddressTextBox;
      private System.Windows.Forms.TextBox TransmitPortTextBox;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.TextBox ReceiveAddressTextBox;
      private System.Windows.Forms.TextBox ReceivePortTextBox;
      private System.Windows.Forms.Button OkButton;
      private System.Windows.Forms.Button CancelAButton;
      private System.Windows.Forms.Panel IpGatewayParameterPanel;
      private System.Windows.Forms.Panel RateParameterPanel;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Panel EntryPanel;
      private System.Windows.Forms.Panel UsbParameterPanel;
      private System.Windows.Forms.CheckBox UsbUseNodeCheckBox;
      private System.Windows.Forms.TextBox UsbNodeTextBox;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.ComboBox UsbBaudComboBox;
   }
}