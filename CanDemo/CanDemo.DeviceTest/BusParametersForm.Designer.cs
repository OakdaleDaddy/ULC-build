
namespace CanDemo.DeviceTest
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
         this.IpGatewayParameterPanel.SuspendLayout();
         this.RateParameterPanel.SuspendLayout();
         this.EntryPanel.SuspendLayout();
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
         this.EntryPanel.Controls.Add(this.IpGatewayParameterPanel);
         this.EntryPanel.Controls.Add(this.RateParameterPanel);
         this.EntryPanel.Location = new System.Drawing.Point(0, 12);
         this.EntryPanel.Name = "EntryPanel";
         this.EntryPanel.Size = new System.Drawing.Size(245, 78);
         this.EntryPanel.TabIndex = 62;
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
   }
}