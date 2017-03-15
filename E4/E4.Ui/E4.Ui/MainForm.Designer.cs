namespace E4.Ui
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
         this.TitleLabel = new System.Windows.Forms.Label();
         this.MainMotorPanel = new System.Windows.Forms.Panel();
         this.CameraControlPanel = new System.Windows.Forms.Panel();
         this.panel1 = new System.Windows.Forms.Panel();
         this.panel2 = new System.Windows.Forms.Panel();
         this.VersionLabel = new System.Windows.Forms.Label();
         this.StopAllPanel = new System.Windows.Forms.Panel();
         this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
         this.StatusPanel = new System.Windows.Forms.Panel();
         this.TargetStatusTextBox = new System.Windows.Forms.TextBox();
         this.MainStatusTextBox = new System.Windows.Forms.TextBox();
         this.label17 = new System.Windows.Forms.Label();
         this.HeartbeatsDisabledLabel = new System.Windows.Forms.Label();
         this.MainWheelTitleLabel = new System.Windows.Forms.Label();
         this.LaserRangePanel = new System.Windows.Forms.Panel();
         this.textPanel1 = new E4.Ui.Controls.TextPanel();
         this.upDownButton4 = new E4.Ui.Controls.UpDownButton();
         this.textPanel2 = new E4.Ui.Controls.TextPanel();
         this.rotatableLabel1 = new E4.Ui.Controls.RotatableLabel();
         this.leftRightButton2 = new E4.Ui.Controls.LeftRightButton();
         this.rotatableLabel2 = new E4.Ui.Controls.RotatableLabel();
         this.upDownButton5 = new E4.Ui.Controls.UpDownButton();
         this.leftRightButton3 = new E4.Ui.Controls.LeftRightButton();
         this.StopAllButton = new E4.Ui.Controls.E4Button();
         this.ControlPanel = new E4.Ui.Controls.BorderedPanel();
         this.WriteOsdButton = new E4.Ui.Controls.E4Button();
         this.SystemStatusButton = new E4.Ui.Controls.E4Button();
         this.SystemResetButton = new E4.Ui.Controls.HoldButton();
         this.ExitButton = new E4.Ui.Controls.HoldButton();
         this.cameraSelectButton7 = new E4.Ui.Controls.CameraSelectButton();
         this.cameraSelectButton8 = new E4.Ui.Controls.CameraSelectButton();
         this.cameraSelectButton9 = new E4.Ui.Controls.CameraSelectButton();
         this.cameraSelectButton6 = new E4.Ui.Controls.CameraSelectButton();
         this.cameraSelectButton5 = new E4.Ui.Controls.CameraSelectButton();
         this.cameraSelectButton1 = new E4.Ui.Controls.CameraSelectButton();
         this.cameraSelectButton4 = new E4.Ui.Controls.CameraSelectButton();
         this.cameraSelectButton3 = new E4.Ui.Controls.CameraSelectButton();
         this.cameraSelectButton2 = new E4.Ui.Controls.CameraSelectButton();
         this.MainWheelOffButton = new E4.Ui.Controls.HoldButton();
         this.MainWheelMoveButton = new E4.Ui.Controls.ValueButton();
         this.TxDirectionPanel = new E4.Ui.Controls.DirectionalValuePanel();
         this.MainWheelSpeedToggleButton = new E4.Ui.Controls.ValueToggleButton();
         this.MainWheelMotorSetupButton = new E4.Ui.Controls.HoldButton();
         this.MainWheelManualDisplayButton = new E4.Ui.Controls.HoldButton();
         this.MainMotorPanel.SuspendLayout();
         this.CameraControlPanel.SuspendLayout();
         this.panel1.SuspendLayout();
         this.panel2.SuspendLayout();
         this.StopAllPanel.SuspendLayout();
         this.StatusPanel.SuspendLayout();
         this.LaserRangePanel.SuspendLayout();
         this.ControlPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // TitleLabel
         // 
         this.TitleLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.TitleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TitleLabel.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
         this.TitleLabel.Location = new System.Drawing.Point(2, 4);
         this.TitleLabel.Margin = new System.Windows.Forms.Padding(0);
         this.TitleLabel.Name = "TitleLabel";
         this.TitleLabel.Size = new System.Drawing.Size(984, 40);
         this.TitleLabel.TabIndex = 4;
         this.TitleLabel.Text = "    Element 4";
         this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // MainMotorPanel
         // 
         this.MainMotorPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(80)))), ((int)(((byte)(96)))));
         this.MainMotorPanel.Controls.Add(this.MainWheelManualDisplayButton);
         this.MainMotorPanel.Controls.Add(this.MainWheelMotorSetupButton);
         this.MainMotorPanel.Controls.Add(this.MainWheelSpeedToggleButton);
         this.MainMotorPanel.Controls.Add(this.MainWheelTitleLabel);
         this.MainMotorPanel.Controls.Add(this.MainWheelOffButton);
         this.MainMotorPanel.Controls.Add(this.MainWheelMoveButton);
         this.MainMotorPanel.Controls.Add(this.TxDirectionPanel);
         this.MainMotorPanel.Location = new System.Drawing.Point(183, 99);
         this.MainMotorPanel.Name = "MainMotorPanel";
         this.MainMotorPanel.Size = new System.Drawing.Size(328, 371);
         this.MainMotorPanel.TabIndex = 6;
         // 
         // CameraControlPanel
         // 
         this.CameraControlPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.CameraControlPanel.Controls.Add(this.cameraSelectButton4);
         this.CameraControlPanel.Controls.Add(this.cameraSelectButton3);
         this.CameraControlPanel.Controls.Add(this.cameraSelectButton2);
         this.CameraControlPanel.Location = new System.Drawing.Point(1567, 186);
         this.CameraControlPanel.Name = "CameraControlPanel";
         this.CameraControlPanel.Size = new System.Drawing.Size(353, 83);
         this.CameraControlPanel.TabIndex = 7;
         // 
         // panel1
         // 
         this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.panel1.Controls.Add(this.cameraSelectButton6);
         this.panel1.Controls.Add(this.cameraSelectButton5);
         this.panel1.Controls.Add(this.cameraSelectButton1);
         this.panel1.Location = new System.Drawing.Point(1567, 12);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(353, 83);
         this.panel1.TabIndex = 8;
         // 
         // panel2
         // 
         this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(128)))));
         this.panel2.Controls.Add(this.cameraSelectButton7);
         this.panel2.Controls.Add(this.cameraSelectButton8);
         this.panel2.Controls.Add(this.cameraSelectButton9);
         this.panel2.Location = new System.Drawing.Point(1567, 99);
         this.panel2.Name = "panel2";
         this.panel2.Size = new System.Drawing.Size(353, 83);
         this.panel2.TabIndex = 9;
         // 
         // VersionLabel
         // 
         this.VersionLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.VersionLabel.ForeColor = System.Drawing.SystemColors.AppWorkspace;
         this.VersionLabel.Location = new System.Drawing.Point(885, 20);
         this.VersionLabel.Name = "VersionLabel";
         this.VersionLabel.Size = new System.Drawing.Size(100, 23);
         this.VersionLabel.TabIndex = 131;
         this.VersionLabel.Text = "2017.1.27.1";
         this.VersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // StopAllPanel
         // 
         this.StopAllPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.StopAllPanel.Controls.Add(this.StopAllButton);
         this.StopAllPanel.Location = new System.Drawing.Point(1688, 855);
         this.StopAllPanel.Name = "StopAllPanel";
         this.StopAllPanel.Size = new System.Drawing.Size(230, 222);
         this.StopAllPanel.TabIndex = 135;
         // 
         // UpdateTimer
         // 
         this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
         // 
         // StatusPanel
         // 
         this.StatusPanel.BackColor = System.Drawing.Color.Purple;
         this.StatusPanel.Controls.Add(this.TargetStatusTextBox);
         this.StatusPanel.Controls.Add(this.MainStatusTextBox);
         this.StatusPanel.Controls.Add(this.label17);
         this.StatusPanel.Location = new System.Drawing.Point(2, 46);
         this.StatusPanel.Name = "StatusPanel";
         this.StatusPanel.Size = new System.Drawing.Size(652, 36);
         this.StatusPanel.TabIndex = 136;
         // 
         // TargetStatusTextBox
         // 
         this.TargetStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.TargetStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TargetStatusTextBox.ForeColor = System.Drawing.SystemColors.InfoText;
         this.TargetStatusTextBox.Location = new System.Drawing.Point(363, 5);
         this.TargetStatusTextBox.Name = "TargetStatusTextBox";
         this.TargetStatusTextBox.ReadOnly = true;
         this.TargetStatusTextBox.Size = new System.Drawing.Size(280, 26);
         this.TargetStatusTextBox.TabIndex = 4;
         this.TargetStatusTextBox.Text = "communication offline";
         this.TargetStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.TargetStatusTextBox.Visible = false;
         // 
         // MainStatusTextBox
         // 
         this.MainStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.MainStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.MainStatusTextBox.ForeColor = System.Drawing.SystemColors.InfoText;
         this.MainStatusTextBox.Location = new System.Drawing.Point(75, 5);
         this.MainStatusTextBox.Name = "MainStatusTextBox";
         this.MainStatusTextBox.ReadOnly = true;
         this.MainStatusTextBox.Size = new System.Drawing.Size(280, 26);
         this.MainStatusTextBox.TabIndex = 3;
         this.MainStatusTextBox.Text = "not connected";
         this.MainStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label17
         // 
         this.label17.AutoSize = true;
         this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.label17.Location = new System.Drawing.Point(5, 10);
         this.label17.Name = "label17";
         this.label17.Size = new System.Drawing.Size(69, 16);
         this.label17.TabIndex = 1;
         this.label17.Text = "STATUS";
         // 
         // HeartbeatsDisabledLabel
         // 
         this.HeartbeatsDisabledLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.HeartbeatsDisabledLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.HeartbeatsDisabledLabel.ForeColor = System.Drawing.Color.Red;
         this.HeartbeatsDisabledLabel.Location = new System.Drawing.Point(246, 16);
         this.HeartbeatsDisabledLabel.Name = "HeartbeatsDisabledLabel";
         this.HeartbeatsDisabledLabel.Size = new System.Drawing.Size(524, 23);
         this.HeartbeatsDisabledLabel.TabIndex = 190;
         this.HeartbeatsDisabledLabel.Text = "!!! HEARTBEATS DISABLED !!!";
         this.HeartbeatsDisabledLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.HeartbeatsDisabledLabel.Visible = false;
         // 
         // MainWheelTitleLabel
         // 
         this.MainWheelTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.MainWheelTitleLabel.Location = new System.Drawing.Point(3, 8);
         this.MainWheelTitleLabel.Name = "MainWheelTitleLabel";
         this.MainWheelTitleLabel.Size = new System.Drawing.Size(301, 23);
         this.MainWheelTitleLabel.TabIndex = 140;
         this.MainWheelTitleLabel.Text = "MAIN WHEELS";
         this.MainWheelTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // LaserRangePanel
         // 
         this.LaserRangePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(80)))), ((int)(((byte)(96)))));
         this.LaserRangePanel.Controls.Add(this.textPanel1);
         this.LaserRangePanel.Controls.Add(this.upDownButton4);
         this.LaserRangePanel.Controls.Add(this.textPanel2);
         this.LaserRangePanel.Controls.Add(this.rotatableLabel1);
         this.LaserRangePanel.Controls.Add(this.leftRightButton2);
         this.LaserRangePanel.Controls.Add(this.rotatableLabel2);
         this.LaserRangePanel.Controls.Add(this.upDownButton5);
         this.LaserRangePanel.Controls.Add(this.leftRightButton3);
         this.LaserRangePanel.Location = new System.Drawing.Point(714, 118);
         this.LaserRangePanel.Name = "LaserRangePanel";
         this.LaserRangePanel.Size = new System.Drawing.Size(447, 423);
         this.LaserRangePanel.TabIndex = 191;
         // 
         // textPanel1
         // 
         this.textPanel1.BackColor = System.Drawing.Color.Black;
         this.textPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.textPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.textPanel1.ForeColor = System.Drawing.Color.White;
         this.textPanel1.HoldTimeoutEnable = false;
         this.textPanel1.HoldTimeoutInterval = 0;
         this.textPanel1.Location = new System.Drawing.Point(257, 46);
         this.textPanel1.Name = "textPanel1";
         this.textPanel1.Size = new System.Drawing.Size(112, 44);
         this.textPanel1.TabIndex = 18;
         this.textPanel1.ValueText = "#### ticks";
         this.textPanel1.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // upDownButton4
         // 
         this.upDownButton4.ArrowColor = System.Drawing.Color.Black;
         this.upDownButton4.ArrowHighlightColor = System.Drawing.Color.DarkGray;
         this.upDownButton4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.upDownButton4.DisabledArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.upDownButton4.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.upDownButton4.DisabledForeColor = System.Drawing.Color.Silver;
         this.upDownButton4.EdgeSpace = 8;
         this.upDownButton4.ForeColor = System.Drawing.SystemColors.HighlightText;
         this.upDownButton4.HighLightOffset = 7;
         this.upDownButton4.HighlightVisible = true;
         this.upDownButton4.HighLightWeight = 2;
         this.upDownButton4.HoldArrorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.upDownButton4.HoldRepeat = true;
         this.upDownButton4.HoldRepeatInterval = 100;
         this.upDownButton4.HoldTimeoutInterval = 100;
         this.upDownButton4.Location = new System.Drawing.Point(279, 157);
         this.upDownButton4.Name = "upDownButton4";
         this.upDownButton4.Size = new System.Drawing.Size(69, 69);
         this.upDownButton4.TabIndex = 15;
         this.upDownButton4.Text = "upDownButton4";
         this.upDownButton4.TextOffset = 0;
         this.upDownButton4.TextVisible = false;
         this.upDownButton4.UpDown = true;
         this.upDownButton4.UseVisualStyleBackColor = false;
         // 
         // textPanel2
         // 
         this.textPanel2.BackColor = System.Drawing.Color.Black;
         this.textPanel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.textPanel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.textPanel2.ForeColor = System.Drawing.Color.White;
         this.textPanel2.HoldTimeoutEnable = false;
         this.textPanel2.HoldTimeoutInterval = 0;
         this.textPanel2.Location = new System.Drawing.Point(257, 96);
         this.textPanel2.Name = "textPanel2";
         this.textPanel2.Size = new System.Drawing.Size(112, 44);
         this.textPanel2.TabIndex = 21;
         this.textPanel2.ValueText = "#### ticks";
         this.textPanel2.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // rotatableLabel1
         // 
         this.rotatableLabel1.Angle = 90;
         this.rotatableLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold);
         this.rotatableLabel1.Location = new System.Drawing.Point(236, 42);
         this.rotatableLabel1.Name = "rotatableLabel1";
         this.rotatableLabel1.Size = new System.Drawing.Size(62, 52);
         this.rotatableLabel1.TabIndex = 19;
         this.rotatableLabel1.Text = "PITCH";
         this.rotatableLabel1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
         // 
         // leftRightButton2
         // 
         this.leftRightButton2.ArrowColor = System.Drawing.Color.Black;
         this.leftRightButton2.ArrowHighlightColor = System.Drawing.Color.DarkGray;
         this.leftRightButton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.leftRightButton2.DisabledArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.leftRightButton2.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.leftRightButton2.DisabledForeColor = System.Drawing.Color.Silver;
         this.leftRightButton2.EdgeSpace = 8;
         this.leftRightButton2.HighLightOffset = 7;
         this.leftRightButton2.HighlightVisible = true;
         this.leftRightButton2.HighLightWeight = 2;
         this.leftRightButton2.HoldArrorColor = System.Drawing.Color.Gray;
         this.leftRightButton2.HoldRepeat = false;
         this.leftRightButton2.HoldRepeatInterval = 0;
         this.leftRightButton2.HoldTimeoutInterval = 0;
         this.leftRightButton2.LeftRight = true;
         this.leftRightButton2.Location = new System.Drawing.Point(208, 228);
         this.leftRightButton2.Name = "leftRightButton2";
         this.leftRightButton2.Size = new System.Drawing.Size(69, 69);
         this.leftRightButton2.TabIndex = 14;
         this.leftRightButton2.Text = "leftRightButton2";
         this.leftRightButton2.TextOffset = 0;
         this.leftRightButton2.TextVisible = false;
         this.leftRightButton2.UseVisualStyleBackColor = false;
         // 
         // rotatableLabel2
         // 
         this.rotatableLabel2.Angle = 90;
         this.rotatableLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold);
         this.rotatableLabel2.Location = new System.Drawing.Point(236, 92);
         this.rotatableLabel2.Name = "rotatableLabel2";
         this.rotatableLabel2.Size = new System.Drawing.Size(62, 52);
         this.rotatableLabel2.TabIndex = 20;
         this.rotatableLabel2.Text = "YAW";
         this.rotatableLabel2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
         // 
         // upDownButton5
         // 
         this.upDownButton5.ArrowColor = System.Drawing.Color.Black;
         this.upDownButton5.ArrowHighlightColor = System.Drawing.Color.DarkGray;
         this.upDownButton5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.upDownButton5.DisabledArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.upDownButton5.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.upDownButton5.DisabledForeColor = System.Drawing.Color.Silver;
         this.upDownButton5.EdgeSpace = 8;
         this.upDownButton5.ForeColor = System.Drawing.SystemColors.HighlightText;
         this.upDownButton5.HighLightOffset = 7;
         this.upDownButton5.HighlightVisible = true;
         this.upDownButton5.HighLightWeight = 2;
         this.upDownButton5.HoldArrorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.upDownButton5.HoldRepeat = true;
         this.upDownButton5.HoldRepeatInterval = 100;
         this.upDownButton5.HoldTimeoutInterval = 100;
         this.upDownButton5.Location = new System.Drawing.Point(279, 299);
         this.upDownButton5.Name = "upDownButton5";
         this.upDownButton5.Size = new System.Drawing.Size(69, 69);
         this.upDownButton5.TabIndex = 16;
         this.upDownButton5.Text = "upDownButton5";
         this.upDownButton5.TextOffset = 0;
         this.upDownButton5.TextVisible = false;
         this.upDownButton5.UpDown = false;
         this.upDownButton5.UseVisualStyleBackColor = false;
         // 
         // leftRightButton3
         // 
         this.leftRightButton3.ArrowColor = System.Drawing.Color.Black;
         this.leftRightButton3.ArrowHighlightColor = System.Drawing.Color.DarkGray;
         this.leftRightButton3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.leftRightButton3.DisabledArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.leftRightButton3.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.leftRightButton3.DisabledForeColor = System.Drawing.Color.Silver;
         this.leftRightButton3.EdgeSpace = 8;
         this.leftRightButton3.HighLightOffset = 7;
         this.leftRightButton3.HighlightVisible = true;
         this.leftRightButton3.HighLightWeight = 2;
         this.leftRightButton3.HoldArrorColor = System.Drawing.Color.Gray;
         this.leftRightButton3.HoldRepeat = false;
         this.leftRightButton3.HoldRepeatInterval = 0;
         this.leftRightButton3.HoldTimeoutInterval = 0;
         this.leftRightButton3.LeftRight = false;
         this.leftRightButton3.Location = new System.Drawing.Point(350, 228);
         this.leftRightButton3.Name = "leftRightButton3";
         this.leftRightButton3.Size = new System.Drawing.Size(69, 69);
         this.leftRightButton3.TabIndex = 17;
         this.leftRightButton3.Text = "leftRightButton3";
         this.leftRightButton3.TextOffset = 0;
         this.leftRightButton3.TextVisible = false;
         this.leftRightButton3.UseVisualStyleBackColor = false;
         // 
         // StopAllButton
         // 
         this.StopAllButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.StopAllButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.StopAllButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.StopAllButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.StopAllButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.StopAllButton.Location = new System.Drawing.Point(61, 66);
         this.StopAllButton.Name = "StopAllButton";
         this.StopAllButton.Size = new System.Drawing.Size(107, 90);
         this.StopAllButton.TabIndex = 3;
         this.StopAllButton.Text = "STOP        ALL";
         this.StopAllButton.UseVisualStyleBackColor = false;
         this.StopAllButton.Click += new System.EventHandler(this.StopAllButton_Click);
         // 
         // ControlPanel
         // 
         this.ControlPanel.Controls.Add(this.WriteOsdButton);
         this.ControlPanel.Controls.Add(this.SystemStatusButton);
         this.ControlPanel.Controls.Add(this.SystemResetButton);
         this.ControlPanel.Controls.Add(this.ExitButton);
         this.ControlPanel.EdgeWeight = 1;
         this.ControlPanel.Location = new System.Drawing.Point(1438, 855);
         this.ControlPanel.Name = "ControlPanel";
         this.ControlPanel.Size = new System.Drawing.Size(246, 222);
         this.ControlPanel.TabIndex = 134;
         // 
         // WriteOsdButton
         // 
         this.WriteOsdButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.WriteOsdButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.WriteOsdButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.WriteOsdButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.WriteOsdButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.WriteOsdButton.Location = new System.Drawing.Point(11, 14);
         this.WriteOsdButton.Name = "WriteOsdButton";
         this.WriteOsdButton.Size = new System.Drawing.Size(107, 90);
         this.WriteOsdButton.TabIndex = 134;
         this.WriteOsdButton.Text = "WRITE   OSD";
         this.WriteOsdButton.UseVisualStyleBackColor = false;
         this.WriteOsdButton.Click += new System.EventHandler(this.WriteOsdButton_Click);
         // 
         // SystemStatusButton
         // 
         this.SystemStatusButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SystemStatusButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.SystemStatusButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.SystemStatusButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.SystemStatusButton.HoldArrorColor = System.Drawing.Color.Gray;
         this.SystemStatusButton.Location = new System.Drawing.Point(128, 15);
         this.SystemStatusButton.Name = "SystemStatusButton";
         this.SystemStatusButton.Size = new System.Drawing.Size(107, 90);
         this.SystemStatusButton.TabIndex = 8;
         this.SystemStatusButton.Text = "CHECK SYSTEM STATUS";
         this.SystemStatusButton.UseVisualStyleBackColor = false;
         this.SystemStatusButton.Click += new System.EventHandler(this.SystemStatusButton_Click);
         // 
         // SystemResetButton
         // 
         this.SystemResetButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SystemResetButton.DisabledBackColor = System.Drawing.SystemColors.ControlDarkDark;
         this.SystemResetButton.DisabledForeColor = System.Drawing.Color.Gray;
         this.SystemResetButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.SystemResetButton.ForeColor = System.Drawing.Color.Black;
         this.SystemResetButton.HoldArrorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.SystemResetButton.HoldTimeoutEnable = true;
         this.SystemResetButton.HoldTimeoutInterval = 100;
         this.SystemResetButton.Location = new System.Drawing.Point(11, 118);
         this.SystemResetButton.Name = "SystemResetButton";
         this.SystemResetButton.Size = new System.Drawing.Size(107, 90);
         this.SystemResetButton.TabIndex = 133;
         this.SystemResetButton.Text = "SYSTEM RESET";
         this.SystemResetButton.UseVisualStyleBackColor = false;
         this.SystemResetButton.HoldTimeout += new E4.Ui.Controls.HoldTimeoutHandler(this.SystemResetButton_HoldTimeout);
         // 
         // ExitButton
         // 
         this.ExitButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ExitButton.DisabledBackColor = System.Drawing.SystemColors.ControlDarkDark;
         this.ExitButton.DisabledForeColor = System.Drawing.Color.Gray;
         this.ExitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.ExitButton.ForeColor = System.Drawing.Color.Black;
         this.ExitButton.HoldArrorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.ExitButton.HoldTimeoutEnable = true;
         this.ExitButton.HoldTimeoutInterval = 100;
         this.ExitButton.Location = new System.Drawing.Point(129, 118);
         this.ExitButton.Name = "ExitButton";
         this.ExitButton.Size = new System.Drawing.Size(107, 90);
         this.ExitButton.TabIndex = 132;
         this.ExitButton.Text = "EXIT";
         this.ExitButton.UseVisualStyleBackColor = false;
         this.ExitButton.HoldTimeout += new E4.Ui.Controls.HoldTimeoutHandler(this.ExitButton_HoldTimeout);
         // 
         // cameraSelectButton7
         // 
         this.cameraSelectButton7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.cameraSelectButton7.Camera = E4.Ui.Controls.CameraLocations.txForward;
         this.cameraSelectButton7.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.cameraSelectButton7.CenterEnabled = true;
         this.cameraSelectButton7.CenterForeColor = System.Drawing.Color.Yellow;
         this.cameraSelectButton7.CenterLevel = 50;
         this.cameraSelectButton7.CenterVisible = true;
         this.cameraSelectButton7.DisabledBackColor = System.Drawing.SystemColors.ControlDarkDark;
         this.cameraSelectButton7.DisabledForeColor = System.Drawing.Color.Gray;
         this.cameraSelectButton7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.cameraSelectButton7.ForeColor = System.Drawing.Color.Black;
         this.cameraSelectButton7.HoldArrorColor = System.Drawing.Color.Gray;
         this.cameraSelectButton7.HoldRepeat = false;
         this.cameraSelectButton7.HoldRepeatInterval = 0;
         this.cameraSelectButton7.HoldTimeoutEnable = true;
         this.cameraSelectButton7.HoldTimeoutInterval = 100;
         this.cameraSelectButton7.IndicatorBetweenSpace = 4;
         this.cameraSelectButton7.IndicatorEdgeSpace = 4;
         this.cameraSelectButton7.LeftColor = System.Drawing.Color.Maroon;
         this.cameraSelectButton7.LeftVisible = true;
         this.cameraSelectButton7.Location = new System.Drawing.Point(238, 8);
         this.cameraSelectButton7.Name = "cameraSelectButton7";
         this.cameraSelectButton7.RightColor = System.Drawing.Color.DarkBlue;
         this.cameraSelectButton7.RightVisible = true;
         this.cameraSelectButton7.Size = new System.Drawing.Size(107, 67);
         this.cameraSelectButton7.TabIndex = 10;
         this.cameraSelectButton7.Text = "TOP";
         this.cameraSelectButton7.UseVisualStyleBackColor = false;
         // 
         // cameraSelectButton8
         // 
         this.cameraSelectButton8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.cameraSelectButton8.Camera = E4.Ui.Controls.CameraLocations.txForward;
         this.cameraSelectButton8.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.cameraSelectButton8.CenterEnabled = true;
         this.cameraSelectButton8.CenterForeColor = System.Drawing.Color.Yellow;
         this.cameraSelectButton8.CenterLevel = 50;
         this.cameraSelectButton8.CenterVisible = true;
         this.cameraSelectButton8.DisabledBackColor = System.Drawing.SystemColors.ControlDarkDark;
         this.cameraSelectButton8.DisabledForeColor = System.Drawing.Color.Gray;
         this.cameraSelectButton8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.cameraSelectButton8.ForeColor = System.Drawing.Color.Black;
         this.cameraSelectButton8.HoldArrorColor = System.Drawing.Color.Gray;
         this.cameraSelectButton8.HoldRepeat = false;
         this.cameraSelectButton8.HoldRepeatInterval = 0;
         this.cameraSelectButton8.HoldTimeoutEnable = true;
         this.cameraSelectButton8.HoldTimeoutInterval = 100;
         this.cameraSelectButton8.IndicatorBetweenSpace = 4;
         this.cameraSelectButton8.IndicatorEdgeSpace = 4;
         this.cameraSelectButton8.LeftColor = System.Drawing.Color.Maroon;
         this.cameraSelectButton8.LeftVisible = true;
         this.cameraSelectButton8.Location = new System.Drawing.Point(123, 8);
         this.cameraSelectButton8.Name = "cameraSelectButton8";
         this.cameraSelectButton8.RightColor = System.Drawing.Color.DarkBlue;
         this.cameraSelectButton8.RightVisible = true;
         this.cameraSelectButton8.Size = new System.Drawing.Size(107, 67);
         this.cameraSelectButton8.TabIndex = 9;
         this.cameraSelectButton8.Text = "REAR";
         this.cameraSelectButton8.UseVisualStyleBackColor = false;
         // 
         // cameraSelectButton9
         // 
         this.cameraSelectButton9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.cameraSelectButton9.Camera = E4.Ui.Controls.CameraLocations.txForward;
         this.cameraSelectButton9.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.cameraSelectButton9.CenterEnabled = true;
         this.cameraSelectButton9.CenterForeColor = System.Drawing.Color.Yellow;
         this.cameraSelectButton9.CenterLevel = 50;
         this.cameraSelectButton9.CenterVisible = true;
         this.cameraSelectButton9.DisabledBackColor = System.Drawing.SystemColors.ControlDarkDark;
         this.cameraSelectButton9.DisabledForeColor = System.Drawing.Color.Gray;
         this.cameraSelectButton9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.cameraSelectButton9.ForeColor = System.Drawing.Color.Black;
         this.cameraSelectButton9.HoldArrorColor = System.Drawing.Color.Gray;
         this.cameraSelectButton9.HoldRepeat = false;
         this.cameraSelectButton9.HoldRepeatInterval = 0;
         this.cameraSelectButton9.HoldTimeoutEnable = true;
         this.cameraSelectButton9.HoldTimeoutInterval = 100;
         this.cameraSelectButton9.IndicatorBetweenSpace = 4;
         this.cameraSelectButton9.IndicatorEdgeSpace = 4;
         this.cameraSelectButton9.LeftColor = System.Drawing.Color.Maroon;
         this.cameraSelectButton9.LeftVisible = true;
         this.cameraSelectButton9.Location = new System.Drawing.Point(8, 8);
         this.cameraSelectButton9.Name = "cameraSelectButton9";
         this.cameraSelectButton9.RightColor = System.Drawing.Color.DarkBlue;
         this.cameraSelectButton9.RightVisible = true;
         this.cameraSelectButton9.Size = new System.Drawing.Size(107, 67);
         this.cameraSelectButton9.TabIndex = 8;
         this.cameraSelectButton9.Text = "FRONT";
         this.cameraSelectButton9.UseVisualStyleBackColor = false;
         // 
         // cameraSelectButton6
         // 
         this.cameraSelectButton6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.cameraSelectButton6.Camera = E4.Ui.Controls.CameraLocations.txForward;
         this.cameraSelectButton6.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.cameraSelectButton6.CenterEnabled = true;
         this.cameraSelectButton6.CenterForeColor = System.Drawing.Color.Yellow;
         this.cameraSelectButton6.CenterLevel = 50;
         this.cameraSelectButton6.CenterVisible = true;
         this.cameraSelectButton6.DisabledBackColor = System.Drawing.SystemColors.ControlDarkDark;
         this.cameraSelectButton6.DisabledForeColor = System.Drawing.Color.Gray;
         this.cameraSelectButton6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.cameraSelectButton6.ForeColor = System.Drawing.Color.Black;
         this.cameraSelectButton6.HoldArrorColor = System.Drawing.Color.Gray;
         this.cameraSelectButton6.HoldRepeat = false;
         this.cameraSelectButton6.HoldRepeatInterval = 0;
         this.cameraSelectButton6.HoldTimeoutEnable = true;
         this.cameraSelectButton6.HoldTimeoutInterval = 100;
         this.cameraSelectButton6.IndicatorBetweenSpace = 4;
         this.cameraSelectButton6.IndicatorEdgeSpace = 4;
         this.cameraSelectButton6.LeftColor = System.Drawing.Color.Maroon;
         this.cameraSelectButton6.LeftVisible = true;
         this.cameraSelectButton6.Location = new System.Drawing.Point(238, 8);
         this.cameraSelectButton6.Name = "cameraSelectButton6";
         this.cameraSelectButton6.RightColor = System.Drawing.Color.DarkBlue;
         this.cameraSelectButton6.RightVisible = true;
         this.cameraSelectButton6.Size = new System.Drawing.Size(107, 67);
         this.cameraSelectButton6.TabIndex = 10;
         this.cameraSelectButton6.Text = "TOP";
         this.cameraSelectButton6.UseVisualStyleBackColor = false;
         // 
         // cameraSelectButton5
         // 
         this.cameraSelectButton5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.cameraSelectButton5.Camera = E4.Ui.Controls.CameraLocations.txForward;
         this.cameraSelectButton5.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.cameraSelectButton5.CenterEnabled = true;
         this.cameraSelectButton5.CenterForeColor = System.Drawing.Color.Yellow;
         this.cameraSelectButton5.CenterLevel = 50;
         this.cameraSelectButton5.CenterVisible = true;
         this.cameraSelectButton5.DisabledBackColor = System.Drawing.SystemColors.ControlDarkDark;
         this.cameraSelectButton5.DisabledForeColor = System.Drawing.Color.Gray;
         this.cameraSelectButton5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.cameraSelectButton5.ForeColor = System.Drawing.Color.Black;
         this.cameraSelectButton5.HoldArrorColor = System.Drawing.Color.Gray;
         this.cameraSelectButton5.HoldRepeat = false;
         this.cameraSelectButton5.HoldRepeatInterval = 0;
         this.cameraSelectButton5.HoldTimeoutEnable = true;
         this.cameraSelectButton5.HoldTimeoutInterval = 100;
         this.cameraSelectButton5.IndicatorBetweenSpace = 4;
         this.cameraSelectButton5.IndicatorEdgeSpace = 4;
         this.cameraSelectButton5.LeftColor = System.Drawing.Color.Maroon;
         this.cameraSelectButton5.LeftVisible = true;
         this.cameraSelectButton5.Location = new System.Drawing.Point(123, 8);
         this.cameraSelectButton5.Name = "cameraSelectButton5";
         this.cameraSelectButton5.RightColor = System.Drawing.Color.DarkBlue;
         this.cameraSelectButton5.RightVisible = true;
         this.cameraSelectButton5.Size = new System.Drawing.Size(107, 67);
         this.cameraSelectButton5.TabIndex = 9;
         this.cameraSelectButton5.Text = "REAR";
         this.cameraSelectButton5.UseVisualStyleBackColor = false;
         // 
         // cameraSelectButton1
         // 
         this.cameraSelectButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.cameraSelectButton1.Camera = E4.Ui.Controls.CameraLocations.txForward;
         this.cameraSelectButton1.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.cameraSelectButton1.CenterEnabled = true;
         this.cameraSelectButton1.CenterForeColor = System.Drawing.Color.Yellow;
         this.cameraSelectButton1.CenterLevel = 50;
         this.cameraSelectButton1.CenterVisible = true;
         this.cameraSelectButton1.DisabledBackColor = System.Drawing.SystemColors.ControlDarkDark;
         this.cameraSelectButton1.DisabledForeColor = System.Drawing.Color.Gray;
         this.cameraSelectButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.cameraSelectButton1.ForeColor = System.Drawing.Color.Black;
         this.cameraSelectButton1.HoldArrorColor = System.Drawing.Color.Gray;
         this.cameraSelectButton1.HoldRepeat = false;
         this.cameraSelectButton1.HoldRepeatInterval = 0;
         this.cameraSelectButton1.HoldTimeoutEnable = true;
         this.cameraSelectButton1.HoldTimeoutInterval = 100;
         this.cameraSelectButton1.IndicatorBetweenSpace = 4;
         this.cameraSelectButton1.IndicatorEdgeSpace = 4;
         this.cameraSelectButton1.LeftColor = System.Drawing.Color.Maroon;
         this.cameraSelectButton1.LeftVisible = true;
         this.cameraSelectButton1.Location = new System.Drawing.Point(8, 8);
         this.cameraSelectButton1.Name = "cameraSelectButton1";
         this.cameraSelectButton1.RightColor = System.Drawing.Color.DarkBlue;
         this.cameraSelectButton1.RightVisible = true;
         this.cameraSelectButton1.Size = new System.Drawing.Size(107, 67);
         this.cameraSelectButton1.TabIndex = 8;
         this.cameraSelectButton1.Text = "FRONT";
         this.cameraSelectButton1.UseVisualStyleBackColor = false;
         // 
         // cameraSelectButton4
         // 
         this.cameraSelectButton4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.cameraSelectButton4.Camera = E4.Ui.Controls.CameraLocations.txForward;
         this.cameraSelectButton4.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.cameraSelectButton4.CenterEnabled = true;
         this.cameraSelectButton4.CenterForeColor = System.Drawing.Color.Yellow;
         this.cameraSelectButton4.CenterLevel = 50;
         this.cameraSelectButton4.CenterVisible = false;
         this.cameraSelectButton4.DisabledBackColor = System.Drawing.SystemColors.ControlDarkDark;
         this.cameraSelectButton4.DisabledForeColor = System.Drawing.Color.Gray;
         this.cameraSelectButton4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.cameraSelectButton4.ForeColor = System.Drawing.Color.Black;
         this.cameraSelectButton4.HoldArrorColor = System.Drawing.Color.Gray;
         this.cameraSelectButton4.HoldRepeat = false;
         this.cameraSelectButton4.HoldRepeatInterval = 0;
         this.cameraSelectButton4.HoldTimeoutEnable = true;
         this.cameraSelectButton4.HoldTimeoutInterval = 100;
         this.cameraSelectButton4.IndicatorBetweenSpace = 4;
         this.cameraSelectButton4.IndicatorEdgeSpace = 4;
         this.cameraSelectButton4.LeftColor = System.Drawing.Color.Maroon;
         this.cameraSelectButton4.LeftVisible = false;
         this.cameraSelectButton4.Location = new System.Drawing.Point(238, 8);
         this.cameraSelectButton4.Name = "cameraSelectButton4";
         this.cameraSelectButton4.RightColor = System.Drawing.Color.DarkBlue;
         this.cameraSelectButton4.RightVisible = true;
         this.cameraSelectButton4.Size = new System.Drawing.Size(107, 67);
         this.cameraSelectButton4.TabIndex = 11;
         this.cameraSelectButton4.Text = "AUXILIARY MONITOR";
         this.cameraSelectButton4.UseVisualStyleBackColor = false;
         // 
         // cameraSelectButton3
         // 
         this.cameraSelectButton3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.cameraSelectButton3.Camera = E4.Ui.Controls.CameraLocations.txForward;
         this.cameraSelectButton3.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.cameraSelectButton3.CenterEnabled = true;
         this.cameraSelectButton3.CenterForeColor = System.Drawing.Color.Yellow;
         this.cameraSelectButton3.CenterLevel = 50;
         this.cameraSelectButton3.CenterVisible = false;
         this.cameraSelectButton3.DisabledBackColor = System.Drawing.SystemColors.ControlDarkDark;
         this.cameraSelectButton3.DisabledForeColor = System.Drawing.Color.Gray;
         this.cameraSelectButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.cameraSelectButton3.ForeColor = System.Drawing.Color.Black;
         this.cameraSelectButton3.HoldArrorColor = System.Drawing.Color.Gray;
         this.cameraSelectButton3.HoldRepeat = false;
         this.cameraSelectButton3.HoldRepeatInterval = 0;
         this.cameraSelectButton3.HoldTimeoutEnable = true;
         this.cameraSelectButton3.HoldTimeoutInterval = 100;
         this.cameraSelectButton3.IndicatorBetweenSpace = 4;
         this.cameraSelectButton3.IndicatorEdgeSpace = 4;
         this.cameraSelectButton3.LeftColor = System.Drawing.Color.Maroon;
         this.cameraSelectButton3.LeftVisible = true;
         this.cameraSelectButton3.Location = new System.Drawing.Point(123, 8);
         this.cameraSelectButton3.Name = "cameraSelectButton3";
         this.cameraSelectButton3.RightColor = System.Drawing.Color.DarkBlue;
         this.cameraSelectButton3.RightVisible = false;
         this.cameraSelectButton3.Size = new System.Drawing.Size(107, 67);
         this.cameraSelectButton3.TabIndex = 10;
         this.cameraSelectButton3.Text = "MAIN MONITOR";
         this.cameraSelectButton3.UseVisualStyleBackColor = false;
         // 
         // cameraSelectButton2
         // 
         this.cameraSelectButton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.cameraSelectButton2.Camera = E4.Ui.Controls.CameraLocations.txForward;
         this.cameraSelectButton2.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.cameraSelectButton2.CenterEnabled = true;
         this.cameraSelectButton2.CenterForeColor = System.Drawing.Color.Yellow;
         this.cameraSelectButton2.CenterLevel = 100;
         this.cameraSelectButton2.CenterVisible = true;
         this.cameraSelectButton2.DisabledBackColor = System.Drawing.SystemColors.ControlDarkDark;
         this.cameraSelectButton2.DisabledForeColor = System.Drawing.Color.Gray;
         this.cameraSelectButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.cameraSelectButton2.ForeColor = System.Drawing.Color.Black;
         this.cameraSelectButton2.HoldArrorColor = System.Drawing.Color.Gray;
         this.cameraSelectButton2.HoldRepeat = false;
         this.cameraSelectButton2.HoldRepeatInterval = 0;
         this.cameraSelectButton2.HoldTimeoutEnable = true;
         this.cameraSelectButton2.HoldTimeoutInterval = 100;
         this.cameraSelectButton2.IndicatorBetweenSpace = 4;
         this.cameraSelectButton2.IndicatorEdgeSpace = 4;
         this.cameraSelectButton2.LeftColor = System.Drawing.Color.Maroon;
         this.cameraSelectButton2.LeftVisible = false;
         this.cameraSelectButton2.Location = new System.Drawing.Point(8, 8);
         this.cameraSelectButton2.Name = "cameraSelectButton2";
         this.cameraSelectButton2.RightColor = System.Drawing.Color.DarkBlue;
         this.cameraSelectButton2.RightVisible = false;
         this.cameraSelectButton2.Size = new System.Drawing.Size(107, 67);
         this.cameraSelectButton2.TabIndex = 9;
         this.cameraSelectButton2.Text = "LIGHTS";
         this.cameraSelectButton2.UseVisualStyleBackColor = false;
         // 
         // MainWheelOffButton
         // 
         this.MainWheelOffButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.MainWheelOffButton.DisabledBackColor = System.Drawing.SystemColors.ControlDarkDark;
         this.MainWheelOffButton.DisabledForeColor = System.Drawing.Color.Gray;
         this.MainWheelOffButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.MainWheelOffButton.ForeColor = System.Drawing.Color.Black;
         this.MainWheelOffButton.HoldArrorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.MainWheelOffButton.HoldTimeoutEnable = true;
         this.MainWheelOffButton.HoldTimeoutInterval = 100;
         this.MainWheelOffButton.Location = new System.Drawing.Point(38, 95);
         this.MainWheelOffButton.Name = "MainWheelOffButton";
         this.MainWheelOffButton.Size = new System.Drawing.Size(107, 80);
         this.MainWheelOffButton.TabIndex = 7;
         this.MainWheelOffButton.Text = "OFF   (FREE)";
         this.MainWheelOffButton.UseVisualStyleBackColor = false;
         // 
         // MainWheelMoveButton
         // 
         this.MainWheelMoveButton.ArrowWidth = 12;
         this.MainWheelMoveButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.MainWheelMoveButton.DisabledBackColor = System.Drawing.SystemColors.ControlDarkDark;
         this.MainWheelMoveButton.DisabledForeColor = System.Drawing.Color.Gray;
         this.MainWheelMoveButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.MainWheelMoveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.MainWheelMoveButton.ForeColor = System.Drawing.Color.Black;
         this.MainWheelMoveButton.HoldArrorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.MainWheelMoveButton.HoldTimeoutInterval = 0;
         this.MainWheelMoveButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.MainWheelMoveButton.LeftArrowVisible = true;
         this.MainWheelMoveButton.Location = new System.Drawing.Point(172, 95);
         this.MainWheelMoveButton.Name = "MainWheelMoveButton";
         this.MainWheelMoveButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.MainWheelMoveButton.RightArrowVisible = true;
         this.MainWheelMoveButton.Size = new System.Drawing.Size(107, 80);
         this.MainWheelMoveButton.TabIndex = 6;
         this.MainWheelMoveButton.Text = "MOVE";
         this.MainWheelMoveButton.UseVisualStyleBackColor = false;
         this.MainWheelMoveButton.ValueBackColor = System.Drawing.Color.Black;
         this.MainWheelMoveButton.ValueEdgeHeight = 5;
         this.MainWheelMoveButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.MainWheelMoveButton.ValueForeColor = System.Drawing.Color.White;
         this.MainWheelMoveButton.ValueHeight = 22;
         this.MainWheelMoveButton.ValueText = "19.04 m/MIN";
         this.MainWheelMoveButton.ValueWidth = 80;
         // 
         // TxDirectionPanel
         // 
         this.TxDirectionPanel.ActiveBackColor = System.Drawing.Color.Black;
         this.TxDirectionPanel.ActiveFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.TxDirectionPanel.ActiveForeColor = System.Drawing.Color.White;
         this.TxDirectionPanel.ArrowWidth = 60;
         this.TxDirectionPanel.Direction = E4.Ui.Controls.DirectionalValuePanel.Directions.Idle;
         this.TxDirectionPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.TxDirectionPanel.ForeColor = System.Drawing.Color.Black;
         this.TxDirectionPanel.IdleBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(80)))), ((int)(((byte)(96)))));
         this.TxDirectionPanel.IdleFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
         this.TxDirectionPanel.IdleForeColor = System.Drawing.Color.White;
         this.TxDirectionPanel.LeftArrowText = "REV";
         this.TxDirectionPanel.Location = new System.Drawing.Point(51, 41);
         this.TxDirectionPanel.Name = "TxDirectionPanel";
         this.TxDirectionPanel.RightArrowText = "FWD";
         this.TxDirectionPanel.Size = new System.Drawing.Size(219, 42);
         this.TxDirectionPanel.TabIndex = 5;
         this.TxDirectionPanel.Text = "directionalValuePanel1";
         this.TxDirectionPanel.ValueBackColor = System.Drawing.Color.Black;
         this.TxDirectionPanel.ValueForeColor = System.Drawing.Color.White;
         this.TxDirectionPanel.ValueText = "19.04 m/MIN";
         this.TxDirectionPanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // MainWheelSpeedToggleButton
         // 
         this.MainWheelSpeedToggleButton.AutomaticToggle = true;
         this.MainWheelSpeedToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.MainWheelSpeedToggleButton.DisabledBackColor = System.Drawing.SystemColors.ControlDarkDark;
         this.MainWheelSpeedToggleButton.DisabledForeColor = System.Drawing.Color.Gray;
         this.MainWheelSpeedToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.MainWheelSpeedToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.MainWheelSpeedToggleButton.HoldArrorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.MainWheelSpeedToggleButton.HoldEnable = false;
         this.MainWheelSpeedToggleButton.HoldTimeoutInterval = 0;
         this.MainWheelSpeedToggleButton.Location = new System.Drawing.Point(108, 181);
         this.MainWheelSpeedToggleButton.Name = "MainWheelSpeedToggleButton";
         this.MainWheelSpeedToggleButton.OptionASelected = true;
         this.MainWheelSpeedToggleButton.OptionAText = "FAST";
         this.MainWheelSpeedToggleButton.OptionBSelected = false;
         this.MainWheelSpeedToggleButton.OptionBText = "SLOW";
         this.MainWheelSpeedToggleButton.OptionCenterWidth = 2;
         this.MainWheelSpeedToggleButton.OptionEdgeHeight = 8;
         this.MainWheelSpeedToggleButton.OptionHeight = 22;
         this.MainWheelSpeedToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.MainWheelSpeedToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
         this.MainWheelSpeedToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.MainWheelSpeedToggleButton.OptionSelectedBackColor = System.Drawing.Color.Lime;
         this.MainWheelSpeedToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.MainWheelSpeedToggleButton.OptionSelectedForeColor = System.Drawing.Color.Black;
         this.MainWheelSpeedToggleButton.OptionWidth = 45;
         this.MainWheelSpeedToggleButton.Size = new System.Drawing.Size(107, 80);
         this.MainWheelSpeedToggleButton.TabIndex = 141;
         this.MainWheelSpeedToggleButton.Text = "SPEED";
         this.MainWheelSpeedToggleButton.UseVisualStyleBackColor = false;
         // 
         // MainWheelMotorSetupButton
         // 
         this.MainWheelMotorSetupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.MainWheelMotorSetupButton.DisabledBackColor = System.Drawing.SystemColors.ControlDarkDark;
         this.MainWheelMotorSetupButton.DisabledForeColor = System.Drawing.Color.Gray;
         this.MainWheelMotorSetupButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.MainWheelMotorSetupButton.ForeColor = System.Drawing.Color.Black;
         this.MainWheelMotorSetupButton.HoldArrorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.MainWheelMotorSetupButton.HoldTimeoutEnable = false;
         this.MainWheelMotorSetupButton.HoldTimeoutInterval = 100;
         this.MainWheelMotorSetupButton.Location = new System.Drawing.Point(38, 267);
         this.MainWheelMotorSetupButton.Name = "MainWheelMotorSetupButton";
         this.MainWheelMotorSetupButton.Size = new System.Drawing.Size(107, 80);
         this.MainWheelMotorSetupButton.TabIndex = 142;
         this.MainWheelMotorSetupButton.Text = "MOTOR SETUP";
         this.MainWheelMotorSetupButton.UseVisualStyleBackColor = false;
         // 
         // MainWheelManualDisplayButton
         // 
         this.MainWheelManualDisplayButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.MainWheelManualDisplayButton.DisabledBackColor = System.Drawing.SystemColors.ControlDarkDark;
         this.MainWheelManualDisplayButton.DisabledForeColor = System.Drawing.Color.Gray;
         this.MainWheelManualDisplayButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.MainWheelManualDisplayButton.ForeColor = System.Drawing.Color.Black;
         this.MainWheelManualDisplayButton.HoldArrorColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.MainWheelManualDisplayButton.HoldTimeoutEnable = false;
         this.MainWheelManualDisplayButton.HoldTimeoutInterval = 100;
         this.MainWheelManualDisplayButton.Location = new System.Drawing.Point(172, 267);
         this.MainWheelManualDisplayButton.Name = "MainWheelManualDisplayButton";
         this.MainWheelManualDisplayButton.Size = new System.Drawing.Size(107, 80);
         this.MainWheelManualDisplayButton.TabIndex = 143;
         this.MainWheelManualDisplayButton.Text = "JOYSTICK DRIVE";
         this.MainWheelManualDisplayButton.UseVisualStyleBackColor = false;
         // 
         // MainForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.ClientSize = new System.Drawing.Size(1920, 1092);
         this.Controls.Add(this.LaserRangePanel);
         this.Controls.Add(this.HeartbeatsDisabledLabel);
         this.Controls.Add(this.StatusPanel);
         this.Controls.Add(this.StopAllPanel);
         this.Controls.Add(this.ControlPanel);
         this.Controls.Add(this.VersionLabel);
         this.Controls.Add(this.panel2);
         this.Controls.Add(this.panel1);
         this.Controls.Add(this.CameraControlPanel);
         this.Controls.Add(this.MainMotorPanel);
         this.Controls.Add(this.TitleLabel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "MainForm";
         this.Text = "Element 4";
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
         this.Shown += new System.EventHandler(this.MainForm_Shown);
         this.MainMotorPanel.ResumeLayout(false);
         this.CameraControlPanel.ResumeLayout(false);
         this.panel1.ResumeLayout(false);
         this.panel2.ResumeLayout(false);
         this.StopAllPanel.ResumeLayout(false);
         this.StatusPanel.ResumeLayout(false);
         this.StatusPanel.PerformLayout();
         this.LaserRangePanel.ResumeLayout(false);
         this.ControlPanel.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Label TitleLabel;
      private System.Windows.Forms.Panel MainMotorPanel;
      private Controls.DirectionalValuePanel TxDirectionPanel;
      private Controls.ValueButton MainWheelMoveButton;
      private Controls.HoldButton MainWheelOffButton;
      private Controls.CameraSelectButton cameraSelectButton1;
      private System.Windows.Forms.Panel CameraControlPanel;
      private Controls.CameraSelectButton cameraSelectButton4;
      private Controls.CameraSelectButton cameraSelectButton3;
      private Controls.CameraSelectButton cameraSelectButton2;
      private System.Windows.Forms.Panel panel1;
      private Controls.CameraSelectButton cameraSelectButton6;
      private Controls.CameraSelectButton cameraSelectButton5;
      private System.Windows.Forms.Panel panel2;
      private Controls.CameraSelectButton cameraSelectButton7;
      private Controls.CameraSelectButton cameraSelectButton8;
      private Controls.CameraSelectButton cameraSelectButton9;
      private System.Windows.Forms.Label VersionLabel;
      private Controls.HoldButton ExitButton;
      private Controls.HoldButton SystemResetButton;
      private Controls.BorderedPanel ControlPanel;
      private Controls.E4Button SystemStatusButton;
      private System.Windows.Forms.Panel StopAllPanel;
      private Controls.E4Button StopAllButton;
      private Controls.LeftRightButton leftRightButton2;
      private Controls.UpDownButton upDownButton5;
      private Controls.UpDownButton upDownButton4;
      private Controls.LeftRightButton leftRightButton3;
      private Controls.TextPanel textPanel1;
      private Controls.TextPanel textPanel2;
      private Controls.RotatableLabel rotatableLabel2;
      private Controls.RotatableLabel rotatableLabel1;
      private System.Windows.Forms.Timer UpdateTimer;
      private System.Windows.Forms.Panel StatusPanel;
      private System.Windows.Forms.TextBox TargetStatusTextBox;
      private System.Windows.Forms.TextBox MainStatusTextBox;
      private System.Windows.Forms.Label label17;
      private System.Windows.Forms.Label HeartbeatsDisabledLabel;
      private Controls.E4Button WriteOsdButton;
      private System.Windows.Forms.Label MainWheelTitleLabel;
      private System.Windows.Forms.Panel LaserRangePanel;
      private Controls.ValueToggleButton MainWheelSpeedToggleButton;
      private Controls.HoldButton MainWheelMotorSetupButton;
      private Controls.HoldButton MainWheelManualDisplayButton;
   }
}

