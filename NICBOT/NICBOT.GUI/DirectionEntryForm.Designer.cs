namespace NICBOT.GUI
{
   partial class DirectionEntryForm
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
         this.borderedPanel1 = new NICBOT.GUI.BorderedPanel();
         this.NorthButton = new System.Windows.Forms.Button();
         this.EastButton = new System.Windows.Forms.Button();
         this.WestButton = new System.Windows.Forms.Button();
         this.SouthButton = new System.Windows.Forms.Button();
         this.borderedPanel1.SuspendLayout();
         this.SuspendLayout();
         // 
         // borderedPanel1
         // 
         this.borderedPanel1.BackColor = System.Drawing.Color.DimGray;
         this.borderedPanel1.Controls.Add(this.SouthButton);
         this.borderedPanel1.Controls.Add(this.WestButton);
         this.borderedPanel1.Controls.Add(this.EastButton);
         this.borderedPanel1.Controls.Add(this.NorthButton);
         this.borderedPanel1.EdgeWeight = 3;
         this.borderedPanel1.Location = new System.Drawing.Point(0, 0);
         this.borderedPanel1.Name = "borderedPanel1";
         this.borderedPanel1.Size = new System.Drawing.Size(385, 334);
         this.borderedPanel1.TabIndex = 0;
         // 
         // NorthButton
         // 
         this.NorthButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.NorthButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.NorthButton.ForeColor = System.Drawing.Color.Black;
         this.NorthButton.Location = new System.Drawing.Point(139, 16);
         this.NorthButton.Name = "NorthButton";
         this.NorthButton.Size = new System.Drawing.Size(107, 90);
         this.NorthButton.TabIndex = 136;
         this.NorthButton.Text = "NORTH";
         this.NorthButton.UseVisualStyleBackColor = false;
         this.NorthButton.Click += new System.EventHandler(this.NorthButton_Click);
         // 
         // EastButton
         // 
         this.EastButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.EastButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.EastButton.ForeColor = System.Drawing.Color.Black;
         this.EastButton.Location = new System.Drawing.Point(262, 122);
         this.EastButton.Name = "EastButton";
         this.EastButton.Size = new System.Drawing.Size(107, 90);
         this.EastButton.TabIndex = 137;
         this.EastButton.Text = "EAST";
         this.EastButton.UseVisualStyleBackColor = false;
         this.EastButton.Click += new System.EventHandler(this.EastButton_Click);
         // 
         // WestButton
         // 
         this.WestButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.WestButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.WestButton.ForeColor = System.Drawing.Color.Black;
         this.WestButton.Location = new System.Drawing.Point(16, 122);
         this.WestButton.Name = "WestButton";
         this.WestButton.Size = new System.Drawing.Size(107, 90);
         this.WestButton.TabIndex = 138;
         this.WestButton.Text = "WEST";
         this.WestButton.UseVisualStyleBackColor = false;
         this.WestButton.Click += new System.EventHandler(this.WestButton_Click);
         // 
         // SouthButton
         // 
         this.SouthButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SouthButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SouthButton.ForeColor = System.Drawing.Color.Black;
         this.SouthButton.Location = new System.Drawing.Point(139, 228);
         this.SouthButton.Name = "SouthButton";
         this.SouthButton.Size = new System.Drawing.Size(107, 90);
         this.SouthButton.TabIndex = 139;
         this.SouthButton.Text = "SOUTH";
         this.SouthButton.UseVisualStyleBackColor = false;
         this.SouthButton.Click += new System.EventHandler(this.SouthButton_Click);
         // 
         // DirectionalForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(385, 334);
         this.Controls.Add(this.borderedPanel1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "DirectionalForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "DirectionalForm";
         this.Shown += new System.EventHandler(this.DirectionalForm_Shown);
         this.borderedPanel1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private BorderedPanel borderedPanel1;
      private System.Windows.Forms.Button NorthButton;
      private System.Windows.Forms.Button SouthButton;
      private System.Windows.Forms.Button WestButton;
      private System.Windows.Forms.Button EastButton;
   }
}