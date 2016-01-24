namespace NICBOT.GUI
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
         this.TitleLabel = new System.Windows.Forms.Label();
         this.ControlPanel = new System.Windows.Forms.Panel();
         this.SystemResetButton = new NICBOT.GUI.HoldButton();
         this.ExitButton = new NICBOT.GUI.HoldButton();
         this.WriteOsdButton = new System.Windows.Forms.Button();
         this.SystemStatusButton = new System.Windows.Forms.Button();
         this.panel12 = new System.Windows.Forms.Panel();
         this.SystemStatusTextBox = new System.Windows.Forms.TextBox();
         this.label17 = new System.Windows.Forms.Label();
         this.panel23 = new System.Windows.Forms.Panel();
         this.RobotCamera12Button = new NICBOT.GUI.CameraSelectButton();
         this.RobotCamera11Button = new NICBOT.GUI.CameraSelectButton();
         this.RobotCamera10Button = new NICBOT.GUI.CameraSelectButton();
         this.RobotCamera9Button = new NICBOT.GUI.CameraSelectButton();
         this.RobotCamera8Button = new NICBOT.GUI.CameraSelectButton();
         this.RobotCamera7Button = new NICBOT.GUI.CameraSelectButton();
         this.RobotCamera6Button = new NICBOT.GUI.CameraSelectButton();
         this.RobotCamera5Button = new NICBOT.GUI.CameraSelectButton();
         this.RobotCamera4Button = new NICBOT.GUI.CameraSelectButton();
         this.RobotCamera3Button = new NICBOT.GUI.CameraSelectButton();
         this.RobotCamera2Button = new NICBOT.GUI.CameraSelectButton();
         this.RobotCamera1Button = new NICBOT.GUI.CameraSelectButton();
         this.panel25 = new System.Windows.Forms.Panel();
         this.lineControl2 = new NICBOT.GUI.LineControl();
         this.lineControl1 = new NICBOT.GUI.LineControl();
         this.borderedPanel9 = new NICBOT.GUI.BorderedPanel();
         this.BodyFrontReleaseButton = new NICBOT.GUI.HoldButton();
         this.borderedPanel8 = new NICBOT.GUI.BorderedPanel();
         this.BodyRearReleaseButton = new NICBOT.GUI.HoldButton();
         this.BodyDrillButton = new NICBOT.GUI.HoldButton();
         this.BodyClosedButton = new NICBOT.GUI.HoldButton();
         this.CustomSetupButton = new NICBOT.GUI.HoldButton();
         this.label15 = new System.Windows.Forms.Label();
         this.borderedPanel7 = new NICBOT.GUI.BorderedPanel();
         this.BodyOpenButton = new NICBOT.GUI.HoldButton();
         this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
         this.panel24 = new System.Windows.Forms.Panel();
         this.LaunchCameraSelectButton = new NICBOT.GUI.CameraSelectButton();
         this.RobotCameraBSelectButton = new NICBOT.GUI.CameraSelectButton();
         this.LightSelectButton = new NICBOT.GUI.CameraSelectButton();
         this.RobotCameraASelectButton = new NICBOT.GUI.CameraSelectButton();
         this.panel27 = new System.Windows.Forms.Panel();
         this.LaunchCamera4Button = new NICBOT.GUI.CameraSelectButton();
         this.LaunchCamera3Button = new NICBOT.GUI.CameraSelectButton();
         this.LaunchCamera2Button = new NICBOT.GUI.CameraSelectButton();
         this.LaunchCamera1Button = new NICBOT.GUI.CameraSelectButton();
         this.DrillMainPanel = new System.Windows.Forms.Panel();
         this.borderedPanel10 = new NICBOT.GUI.BorderedPanel();
         this.DrillAutoStopButton = new NICBOT.GUI.NicBotButton();
         this.DrillAutoStartButton = new NICBOT.GUI.HoldButton();
         this.DrillAutoPauseResumeButton = new NICBOT.GUI.NicBotButton();
         this.DrillManulDisplayButton = new NICBOT.GUI.NicBotButton();
         this.DrillLaserLightButton = new NICBOT.GUI.ValueToggleButton();
         this.DrillSealModeButton = new NICBOT.GUI.HoldButton();
         this.DrillSelectionLabel = new System.Windows.Forms.Label();
         this.DrillExtendedActualValuePanel = new NICBOT.GUI.TextPanel();
         this.DrillExtendedSetPointValuePanel = new NICBOT.GUI.TextPanel();
         this.DrillRotationActualSpeedValuePanel = new NICBOT.GUI.TextPanel();
         this.label28 = new System.Windows.Forms.Label();
         this.label27 = new System.Windows.Forms.Label();
         this.label26 = new System.Windows.Forms.Label();
         this.DrillModeLabel = new System.Windows.Forms.Label();
         this.label24 = new System.Windows.Forms.Label();
         this.DrillPipePositionLabel = new System.Windows.Forms.Label();
         this.DrillSetupButton = new NICBOT.GUI.HoldButton();
         this.DrillRotaionSetPointSpeedValuePanel = new NICBOT.GUI.TextPanel();
         this.RetractionLimitLightTextBox = new System.Windows.Forms.TextBox();
         this.OriginSetLightTextBox = new System.Windows.Forms.TextBox();
         this.DrillErrorLightTextBox = new System.Windows.Forms.TextBox();
         this.label48 = new System.Windows.Forms.Label();
         this.label47 = new System.Windows.Forms.Label();
         this.label46 = new System.Windows.Forms.Label();
         this.rotatableLabel1 = new NICBOT.GUI.RotatableLabel();
         this.rotatableLabel2 = new NICBOT.GUI.RotatableLabel();
         this.rotatableLabel5 = new NICBOT.GUI.RotatableLabel();
         this.rotatableLabel6 = new NICBOT.GUI.RotatableLabel();
         this.label45 = new System.Windows.Forms.Label();
         this.MovementMainPanel = new System.Windows.Forms.Panel();
         this.MovementLockButton = new NICBOT.GUI.HoldButton();
         this.MovementOffButton = new NICBOT.GUI.HoldButton();
         this.MovementManaulDisplayButton = new NICBOT.GUI.NicBotButton();
         this.MovementCornerModeToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.MovementLaunchModeToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.MovementMoveButton = new NICBOT.GUI.ValueButton();
         this.MotorStatusDirectionalValuePanel = new NICBOT.GUI.DirectionalValuePanel();
         this.MotorTitleLabel = new System.Windows.Forms.Label();
         this.MovementSetupButton = new NICBOT.GUI.HoldButton();
         this.MovementSpeedToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.MovementAxialToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.SealantMainPanel = new System.Windows.Forms.Panel();
         this.borderedPanel11 = new NICBOT.GUI.BorderedPanel();
         this.SealantAutoStopButton = new NICBOT.GUI.NicBotButton();
         this.SealantAutoStartButton = new NICBOT.GUI.HoldButton();
         this.SealantAutoPauseResumeButton = new NICBOT.GUI.NicBotButton();
         this.SealantManulDisplayButton = new NICBOT.GUI.NicBotButton();
         this.SealantLaserLightButton = new NICBOT.GUI.ValueToggleButton();
         this.SealantReserviorTextPanel = new NICBOT.GUI.TextPanel();
         this.label29 = new System.Windows.Forms.Label();
         this.SealantNozzlePositionTextPanel = new NICBOT.GUI.TextPanel();
         this.SealantFlowRateTextPanel = new NICBOT.GUI.TextPanel();
         this.label30 = new System.Windows.Forms.Label();
         this.label25 = new System.Windows.Forms.Label();
         this.SealantActualSpeedValuePanel = new NICBOT.GUI.TextPanel();
         this.label23 = new System.Windows.Forms.Label();
         this.SealantSpeedSetPointValuePanel = new NICBOT.GUI.TextPanel();
         this.SealantActualPressureValuePanel = new NICBOT.GUI.TextPanel();
         this.SealantPressureSetPointValuePanel = new NICBOT.GUI.TextPanel();
         this.SealantActualVolumeValuePanel = new NICBOT.GUI.TextPanel();
         this.label18 = new System.Windows.Forms.Label();
         this.label19 = new System.Windows.Forms.Label();
         this.SealantVolumeSetPointValuePanel = new NICBOT.GUI.TextPanel();
         this.label10 = new System.Windows.Forms.Label();
         this.SealantModeLabel = new System.Windows.Forms.Label();
         this.label12 = new System.Windows.Forms.Label();
         this.SealantPipePositionLabel = new System.Windows.Forms.Label();
         this.SealDrillModeButton = new NICBOT.GUI.HoldButton();
         this.SealantSetupButton = new NICBOT.GUI.HoldButton();
         this.NozzleSelectionLabel = new System.Windows.Forms.Label();
         this.rotatableLabel3 = new NICBOT.GUI.RotatableLabel();
         this.rotatableLabel4 = new NICBOT.GUI.RotatableLabel();
         this.rotatableLabel7 = new NICBOT.GUI.RotatableLabel();
         this.rotatableLabel8 = new NICBOT.GUI.RotatableLabel();
         this.rotatableLabel9 = new NICBOT.GUI.RotatableLabel();
         this.rotatableLabel10 = new NICBOT.GUI.RotatableLabel();
         this.FeederMainPanel = new System.Windows.Forms.Panel();
         this.FeederManulDisplayButton = new NICBOT.GUI.NicBotButton();
         this.FeederLockButton = new NICBOT.GUI.HoldButton();
         this.FeederOffButton = new NICBOT.GUI.HoldButton();
         this.FeederActualValuePanel = new NICBOT.GUI.DirectionalValuePanel();
         this.FeederMoveButton = new NICBOT.GUI.ValueButton();
         this.FeederTitleLabel = new System.Windows.Forms.Label();
         this.FeederSetupButton = new NICBOT.GUI.HoldButton();
         this.FeederSpeedToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.FeederManualPanel = new System.Windows.Forms.Panel();
         this.FeederClampSetupButton = new NICBOT.GUI.HoldButton();
         this.FeederHideManulButton = new NICBOT.GUI.NicBotButton();
         this.FeederManualSetupButton = new NICBOT.GUI.HoldButton();
         this.FeederManualReverseButton = new NICBOT.GUI.HoldButton();
         this.FeederManualForwardButton = new NICBOT.GUI.HoldButton();
         this.FeederSpeedValueButton = new NICBOT.GUI.ValueButton();
         this.DrillManualPanel = new System.Windows.Forms.Panel();
         this.DrillRetractToLimitButton = new NICBOT.GUI.NicBotButton();
         this.DrillStopButton = new NICBOT.GUI.NicBotButton();
         this.DrillMoveToOriginButton = new NICBOT.GUI.NicBotButton();
         this.borderedPanel1 = new NICBOT.GUI.BorderedPanel();
         this.DrillManualToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.DrillDirectionToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.DrillSpeedDecreaseButton = new NICBOT.GUI.UpDownButton();
         this.label43 = new System.Windows.Forms.Label();
         this.DrillSpeedIncreaseButton = new NICBOT.GUI.UpDownButton();
         this.DrillSetOriginButton = new NICBOT.GUI.HoldButton();
         this.DrillFindOriginButton = new NICBOT.GUI.HoldButton();
         this.DrillIndexUpButton = new NICBOT.GUI.UpDownButton();
         this.DrillIndexDownButton = new NICBOT.GUI.UpDownButton();
         this.panel1 = new System.Windows.Forms.Panel();
         this.RearSealantReserviorPanel = new NICBOT.GUI.BorderedPanel();
         this.lineControl6 = new NICBOT.GUI.LineControl();
         this.label11 = new System.Windows.Forms.Label();
         this.RearSealantReserviorWeightTextPanel = new NICBOT.GUI.TextPanel();
         this.label21 = new System.Windows.Forms.Label();
         this.label33 = new System.Windows.Forms.Label();
         this.RearSealantReserviorVolumeTextPanel = new NICBOT.GUI.TextPanel();
         this.FrontSealantReserviorPanel = new NICBOT.GUI.BorderedPanel();
         this.lineControl5 = new NICBOT.GUI.LineControl();
         this.label32 = new System.Windows.Forms.Label();
         this.FrontSealantReserviorWeightTextPanel = new NICBOT.GUI.TextPanel();
         this.label14 = new System.Windows.Forms.Label();
         this.label16 = new System.Windows.Forms.Label();
         this.FrontSealantReserviorVolumeTextPanel = new NICBOT.GUI.TextPanel();
         this.borderedPanel2 = new NICBOT.GUI.BorderedPanel();
         this.label2 = new System.Windows.Forms.Label();
         this.lineControl3 = new NICBOT.GUI.LineControl();
         this.NitrogenPressure1TextPanel = new NICBOT.GUI.TextPanel();
         this.NitrogenPressure2TextPanel = new NICBOT.GUI.TextPanel();
         this.label20 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.ReelMainPanel = new System.Windows.Forms.Panel();
         this.ReelShowManualButton = new NICBOT.GUI.NicBotButton();
         this.ReelSetupButton = new NICBOT.GUI.HoldButton();
         this.ReelResetTotalButton = new NICBOT.GUI.HoldButton();
         this.ReelResetTripButton = new NICBOT.GUI.HoldButton();
         this.ReelCalibrateToButton = new NICBOT.GUI.ValueButton();
         this.ReelLockButton = new NICBOT.GUI.HoldButton();
         this.ReelReverseButton = new NICBOT.GUI.HoldButton();
         this.ReelOffButton = new NICBOT.GUI.HoldButton();
         this.ReelTripTextPanel = new NICBOT.GUI.TextPanel();
         this.label5 = new System.Windows.Forms.Label();
         this.ReelTotalTextPanel = new NICBOT.GUI.TextPanel();
         this.label4 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.ReelActualValuePanel = new NICBOT.GUI.DirectionalValuePanel();
         this.ReelManualPanel = new System.Windows.Forms.Panel();
         this.ReelManualHideButton = new NICBOT.GUI.NicBotButton();
         this.ReelManualSetupButton = new NICBOT.GUI.HoldButton();
         this.ReelManualResetTotalButton = new NICBOT.GUI.HoldButton();
         this.ReelManualResetTripButton = new NICBOT.GUI.HoldButton();
         this.ReelManualCalibrateToButton = new NICBOT.GUI.ValueButton();
         this.ReelManualCurrentTextPanel = new NICBOT.GUI.TextPanel();
         this.label6 = new System.Windows.Forms.Label();
         this.ReelManualCurrentUpButton = new NICBOT.GUI.UpDownButton();
         this.ReelManualCurrentDownButton = new NICBOT.GUI.UpDownButton();
         this.ReelManualTorqueDirectionToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.ReelManualOnOffToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.GuidePanel = new System.Windows.Forms.Panel();
         this.label7 = new System.Windows.Forms.Label();
         this.GuideSetupButton = new NICBOT.GUI.HoldButton();
         this.GuideExtendRightButton = new NICBOT.GUI.CameraSelectButton();
         this.GuideRetractRightButton = new NICBOT.GUI.CameraSelectButton();
         this.GuideExtendLeftButton = new NICBOT.GUI.CameraSelectButton();
         this.GuideRetractLeftButton = new NICBOT.GUI.CameraSelectButton();
         this.SealantManualPanel = new System.Windows.Forms.Panel();
         this.SealantRelievePressureButton = new NICBOT.GUI.ValueButton();
         this.SealantDirectionToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.SealantPressureDecreaseButton = new NICBOT.GUI.UpDownButton();
         this.SealantPressureIncreaseButton = new NICBOT.GUI.UpDownButton();
         this.SealantNozzleToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.SealantManualModeToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.SealantManualPumpToggleButton = new NICBOT.GUI.ValueToggleButton();
         this.SealantSpeedIncreaseButton = new NICBOT.GUI.UpDownButton();
         this.label8 = new System.Windows.Forms.Label();
         this.label9 = new System.Windows.Forms.Label();
         this.SealantSpeedDecreaseButton = new NICBOT.GUI.UpDownButton();
         this.MovementManulPanel = new System.Windows.Forms.Panel();
         this.MotorManualJogForwardButton = new NICBOT.GUI.NicBotButton();
         this.MotorManualJogReverseButton = new NICBOT.GUI.NicBotButton();
         this.MotorManualMoveForwardButton = new NICBOT.GUI.HoldButton();
         this.MotorManualMoveReverseButton = new NICBOT.GUI.HoldButton();
         this.MotorManualMoveSpeedValueButton = new NICBOT.GUI.ValueButton();
         this.MotorManualJogDistanceValueButton = new NICBOT.GUI.ValueButton();
         this.InspectionPanel = new System.Windows.Forms.Panel();
         this.borderedPanel6 = new NICBOT.GUI.BorderedPanel();
         this.SensorPipePositionTextPanel = new NICBOT.GUI.TextPanel();
         this.label13 = new System.Windows.Forms.Label();
         this.SensorGpsTimeTextPanel = new NICBOT.GUI.TextPanel();
         this.label42 = new System.Windows.Forms.Label();
         this.label41 = new System.Windows.Forms.Label();
         this.SensorGpsDateTextPanel = new NICBOT.GUI.TextPanel();
         this.SensorDisplacementTextPanel = new NICBOT.GUI.TextPanel();
         this.label40 = new System.Windows.Forms.Label();
         this.SensorDirectionTextPanel = new NICBOT.GUI.TextPanel();
         this.label39 = new System.Windows.Forms.Label();
         this.SensorLongitudeTextPanel = new NICBOT.GUI.TextPanel();
         this.SensorLatitudeTextPanel = new NICBOT.GUI.TextPanel();
         this.label35 = new System.Windows.Forms.Label();
         this.label37 = new System.Windows.Forms.Label();
         this.rotatableLabel13 = new NICBOT.GUI.RotatableLabel();
         this.label38 = new System.Windows.Forms.Label();
         this.borderedPanel5 = new NICBOT.GUI.BorderedPanel();
         this.SensorStressReadingTextPanel = new NICBOT.GUI.TextPanel();
         this.SensorThicknessAcquireButton = new NICBOT.GUI.HoldButton();
         this.SensorStressAcquireButton = new NICBOT.GUI.HoldButton();
         this.SensorThicknessReadingTextPanel = new NICBOT.GUI.TextPanel();
         this.VersionLabel = new System.Windows.Forms.Label();
         this.RobotCrossSectionView = new NICBOT.Controls.CrossSectionView();
         this.BotSideView = new NICBOT.GUI.NicBotSideView();
         this.ControlPanel.SuspendLayout();
         this.panel12.SuspendLayout();
         this.panel23.SuspendLayout();
         this.panel25.SuspendLayout();
         this.borderedPanel9.SuspendLayout();
         this.borderedPanel8.SuspendLayout();
         this.borderedPanel7.SuspendLayout();
         this.panel24.SuspendLayout();
         this.panel27.SuspendLayout();
         this.DrillMainPanel.SuspendLayout();
         this.borderedPanel10.SuspendLayout();
         this.MovementMainPanel.SuspendLayout();
         this.SealantMainPanel.SuspendLayout();
         this.borderedPanel11.SuspendLayout();
         this.FeederMainPanel.SuspendLayout();
         this.FeederManualPanel.SuspendLayout();
         this.DrillManualPanel.SuspendLayout();
         this.borderedPanel1.SuspendLayout();
         this.panel1.SuspendLayout();
         this.RearSealantReserviorPanel.SuspendLayout();
         this.FrontSealantReserviorPanel.SuspendLayout();
         this.borderedPanel2.SuspendLayout();
         this.ReelMainPanel.SuspendLayout();
         this.ReelManualPanel.SuspendLayout();
         this.GuidePanel.SuspendLayout();
         this.SealantManualPanel.SuspendLayout();
         this.MovementManulPanel.SuspendLayout();
         this.InspectionPanel.SuspendLayout();
         this.borderedPanel6.SuspendLayout();
         this.borderedPanel5.SuspendLayout();
         this.SuspendLayout();
         // 
         // TitleLabel
         // 
         this.TitleLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.TitleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.TitleLabel.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
         this.TitleLabel.Location = new System.Drawing.Point(2, 2);
         this.TitleLabel.Margin = new System.Windows.Forms.Padding(0);
         this.TitleLabel.Name = "TitleLabel";
         this.TitleLabel.Size = new System.Drawing.Size(1916, 40);
         this.TitleLabel.TabIndex = 3;
         this.TitleLabel.Text = "    CIRRIS XR";
         this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // ControlPanel
         // 
         this.ControlPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.ControlPanel.Controls.Add(this.SystemResetButton);
         this.ControlPanel.Controls.Add(this.ExitButton);
         this.ControlPanel.Controls.Add(this.WriteOsdButton);
         this.ControlPanel.Controls.Add(this.SystemStatusButton);
         this.ControlPanel.Location = new System.Drawing.Point(1671, 854);
         this.ControlPanel.Name = "ControlPanel";
         this.ControlPanel.Size = new System.Drawing.Size(246, 222);
         this.ControlPanel.TabIndex = 6;
         // 
         // SystemResetButton
         // 
         this.SystemResetButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SystemResetButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.SystemResetButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.SystemResetButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.SystemResetButton.ForeColor = System.Drawing.Color.Black;
         this.SystemResetButton.HoldTimeoutEnable = true;
         this.SystemResetButton.HoldTimeoutInterval = 100;
         this.SystemResetButton.Location = new System.Drawing.Point(11, 118);
         this.SystemResetButton.Name = "SystemResetButton";
         this.SystemResetButton.Size = new System.Drawing.Size(107, 90);
         this.SystemResetButton.TabIndex = 128;
         this.SystemResetButton.Text = "System Reset";
         this.SystemResetButton.UseVisualStyleBackColor = false;
         this.SystemResetButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.SystemResetButton_HoldTimeout);
         // 
         // ExitButton
         // 
         this.ExitButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ExitButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ExitButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ExitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.ExitButton.ForeColor = System.Drawing.Color.Black;
         this.ExitButton.HoldTimeoutEnable = true;
         this.ExitButton.HoldTimeoutInterval = 100;
         this.ExitButton.Location = new System.Drawing.Point(129, 118);
         this.ExitButton.Name = "ExitButton";
         this.ExitButton.Size = new System.Drawing.Size(107, 90);
         this.ExitButton.TabIndex = 127;
         this.ExitButton.Text = "EXIT";
         this.ExitButton.UseVisualStyleBackColor = false;
         this.ExitButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.ExitButton_HoldTimeout);
         // 
         // WriteOsdButton
         // 
         this.WriteOsdButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.WriteOsdButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.WriteOsdButton.ForeColor = System.Drawing.Color.Black;
         this.WriteOsdButton.Location = new System.Drawing.Point(11, 14);
         this.WriteOsdButton.Name = "WriteOsdButton";
         this.WriteOsdButton.Size = new System.Drawing.Size(107, 90);
         this.WriteOsdButton.TabIndex = 9;
         this.WriteOsdButton.Text = "WRITE OSD";
         this.WriteOsdButton.UseVisualStyleBackColor = false;
         this.WriteOsdButton.Click += new System.EventHandler(this.WriteOsdButton_Click);
         // 
         // SystemStatusButton
         // 
         this.SystemStatusButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SystemStatusButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.SystemStatusButton.ForeColor = System.Drawing.Color.Black;
         this.SystemStatusButton.Location = new System.Drawing.Point(128, 15);
         this.SystemStatusButton.Name = "SystemStatusButton";
         this.SystemStatusButton.Size = new System.Drawing.Size(107, 90);
         this.SystemStatusButton.TabIndex = 8;
         this.SystemStatusButton.Text = "CHECK SYSTEM STATUS";
         this.SystemStatusButton.UseVisualStyleBackColor = false;
         this.SystemStatusButton.Click += new System.EventHandler(this.SystemStatusButton_Click);
         // 
         // panel12
         // 
         this.panel12.BackColor = System.Drawing.Color.Purple;
         this.panel12.Controls.Add(this.SystemStatusTextBox);
         this.panel12.Controls.Add(this.label17);
         this.panel12.Location = new System.Drawing.Point(2, 46);
         this.panel12.Name = "panel12";
         this.panel12.Size = new System.Drawing.Size(661, 36);
         this.panel12.TabIndex = 18;
         // 
         // SystemStatusTextBox
         // 
         this.SystemStatusTextBox.BackColor = System.Drawing.Color.Red;
         this.SystemStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SystemStatusTextBox.ForeColor = System.Drawing.SystemColors.InfoText;
         this.SystemStatusTextBox.Location = new System.Drawing.Point(85, 5);
         this.SystemStatusTextBox.Name = "SystemStatusTextBox";
         this.SystemStatusTextBox.ReadOnly = true;
         this.SystemStatusTextBox.Size = new System.Drawing.Size(558, 26);
         this.SystemStatusTextBox.TabIndex = 3;
         this.SystemStatusTextBox.Text = "not connected";
         this.SystemStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label17
         // 
         this.label17.AutoSize = true;
         this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.label17.Location = new System.Drawing.Point(10, 10);
         this.label17.Name = "label17";
         this.label17.Size = new System.Drawing.Size(69, 16);
         this.label17.TabIndex = 1;
         this.label17.Text = "STATUS";
         // 
         // panel23
         // 
         this.panel23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.panel23.Controls.Add(this.RobotCamera12Button);
         this.panel23.Controls.Add(this.RobotCamera11Button);
         this.panel23.Controls.Add(this.RobotCamera10Button);
         this.panel23.Controls.Add(this.RobotCamera9Button);
         this.panel23.Controls.Add(this.RobotCamera8Button);
         this.panel23.Controls.Add(this.RobotCamera7Button);
         this.panel23.Controls.Add(this.RobotCamera6Button);
         this.panel23.Controls.Add(this.RobotCamera5Button);
         this.panel23.Controls.Add(this.RobotCamera4Button);
         this.panel23.Controls.Add(this.RobotCamera3Button);
         this.panel23.Controls.Add(this.RobotCamera2Button);
         this.panel23.Controls.Add(this.RobotCamera1Button);
         this.panel23.Location = new System.Drawing.Point(1565, 47);
         this.panel23.Name = "panel23";
         this.panel23.Size = new System.Drawing.Size(353, 360);
         this.panel23.TabIndex = 26;
         // 
         // RobotCamera12Button
         // 
         this.RobotCamera12Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RobotCamera12Button.Camera = NICBOT.GUI.CameraLocations.robotFrontUpperBack;
         this.RobotCamera12Button.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RobotCamera12Button.CenterForeColor = System.Drawing.Color.White;
         this.RobotCamera12Button.CenterLevel = 75;
         this.RobotCamera12Button.CenterVisible = true;
         this.RobotCamera12Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.RobotCamera12Button.HoldRepeat = false;
         this.RobotCamera12Button.HoldRepeatInterval = 0;
         this.RobotCamera12Button.HoldTimeoutEnable = true;
         this.RobotCamera12Button.HoldTimeoutInterval = 100;
         this.RobotCamera12Button.IndicatorBetweenSpace = 4;
         this.RobotCamera12Button.IndicatorEdgeSpace = 4;
         this.RobotCamera12Button.LeftColor = System.Drawing.Color.Maroon;
         this.RobotCamera12Button.LeftVisible = true;
         this.RobotCamera12Button.Location = new System.Drawing.Point(236, 272);
         this.RobotCamera12Button.Name = "RobotCamera12Button";
         this.RobotCamera12Button.RightColor = System.Drawing.Color.DarkBlue;
         this.RobotCamera12Button.RightVisible = true;
         this.RobotCamera12Button.Size = new System.Drawing.Size(107, 80);
         this.RobotCamera12Button.TabIndex = 52;
         this.RobotCamera12Button.Text = "SENSOR BAY";
         this.RobotCamera12Button.UseVisualStyleBackColor = false;
         this.RobotCamera12Button.HoldTimeout += new NICBOT.GUI.CameraSelectButton.HoldTimeoutHandler(this.CameraButton_HoldTimeout);
         this.RobotCamera12Button.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RobotCameraButton_MouseClick);
         // 
         // RobotCamera11Button
         // 
         this.RobotCamera11Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RobotCamera11Button.Camera = NICBOT.GUI.CameraLocations.robotFrfDrill;
         this.RobotCamera11Button.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RobotCamera11Button.CenterForeColor = System.Drawing.Color.White;
         this.RobotCamera11Button.CenterLevel = 75;
         this.RobotCamera11Button.CenterVisible = true;
         this.RobotCamera11Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.RobotCamera11Button.HoldRepeat = false;
         this.RobotCamera11Button.HoldRepeatInterval = 0;
         this.RobotCamera11Button.HoldTimeoutEnable = true;
         this.RobotCamera11Button.HoldTimeoutInterval = 100;
         this.RobotCamera11Button.IndicatorBetweenSpace = 4;
         this.RobotCamera11Button.IndicatorEdgeSpace = 4;
         this.RobotCamera11Button.LeftColor = System.Drawing.Color.Maroon;
         this.RobotCamera11Button.LeftVisible = true;
         this.RobotCamera11Button.Location = new System.Drawing.Point(123, 272);
         this.RobotCamera11Button.Name = "RobotCamera11Button";
         this.RobotCamera11Button.RightColor = System.Drawing.Color.DarkBlue;
         this.RobotCamera11Button.RightVisible = true;
         this.RobotCamera11Button.Size = new System.Drawing.Size(107, 80);
         this.RobotCamera11Button.TabIndex = 51;
         this.RobotCamera11Button.Text = "CAM11";
         this.RobotCamera11Button.UseVisualStyleBackColor = false;
         this.RobotCamera11Button.HoldTimeout += new NICBOT.GUI.CameraSelectButton.HoldTimeoutHandler(this.CameraButton_HoldTimeout);
         this.RobotCamera11Button.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RobotCameraButton_MouseClick);
         // 
         // RobotCamera10Button
         // 
         this.RobotCamera10Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RobotCamera10Button.Camera = NICBOT.GUI.CameraLocations.robotLowerForward;
         this.RobotCamera10Button.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RobotCamera10Button.CenterForeColor = System.Drawing.Color.White;
         this.RobotCamera10Button.CenterLevel = 75;
         this.RobotCamera10Button.CenterVisible = true;
         this.RobotCamera10Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.RobotCamera10Button.HoldRepeat = false;
         this.RobotCamera10Button.HoldRepeatInterval = 0;
         this.RobotCamera10Button.HoldTimeoutEnable = true;
         this.RobotCamera10Button.HoldTimeoutInterval = 100;
         this.RobotCamera10Button.IndicatorBetweenSpace = 4;
         this.RobotCamera10Button.IndicatorEdgeSpace = 4;
         this.RobotCamera10Button.LeftColor = System.Drawing.Color.Maroon;
         this.RobotCamera10Button.LeftVisible = true;
         this.RobotCamera10Button.Location = new System.Drawing.Point(236, 184);
         this.RobotCamera10Button.Name = "RobotCamera10Button";
         this.RobotCamera10Button.RightColor = System.Drawing.Color.DarkBlue;
         this.RobotCamera10Button.RightVisible = true;
         this.RobotCamera10Button.Size = new System.Drawing.Size(107, 80);
         this.RobotCamera10Button.TabIndex = 50;
         this.RobotCamera10Button.Text = "LOWER FORWARD";
         this.RobotCamera10Button.UseVisualStyleBackColor = false;
         this.RobotCamera10Button.HoldTimeout += new NICBOT.GUI.CameraSelectButton.HoldTimeoutHandler(this.CameraButton_HoldTimeout);
         this.RobotCamera10Button.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RobotCameraButton_MouseClick);
         // 
         // RobotCamera9Button
         // 
         this.RobotCamera9Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RobotCamera9Button.Camera = NICBOT.GUI.CameraLocations.robotFrontUpperBack;
         this.RobotCamera9Button.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RobotCamera9Button.CenterForeColor = System.Drawing.Color.White;
         this.RobotCamera9Button.CenterLevel = 75;
         this.RobotCamera9Button.CenterVisible = true;
         this.RobotCamera9Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.RobotCamera9Button.HoldRepeat = false;
         this.RobotCamera9Button.HoldRepeatInterval = 0;
         this.RobotCamera9Button.HoldTimeoutEnable = true;
         this.RobotCamera9Button.HoldTimeoutInterval = 100;
         this.RobotCamera9Button.IndicatorBetweenSpace = 4;
         this.RobotCamera9Button.IndicatorEdgeSpace = 4;
         this.RobotCamera9Button.LeftColor = System.Drawing.Color.Maroon;
         this.RobotCamera9Button.LeftVisible = true;
         this.RobotCamera9Button.Location = new System.Drawing.Point(8, 272);
         this.RobotCamera9Button.Name = "RobotCamera9Button";
         this.RobotCamera9Button.RightColor = System.Drawing.Color.DarkBlue;
         this.RobotCamera9Button.RightVisible = true;
         this.RobotCamera9Button.Size = new System.Drawing.Size(107, 80);
         this.RobotCamera9Button.TabIndex = 49;
         this.RobotCamera9Button.Text = "SENSOR ARM";
         this.RobotCamera9Button.UseVisualStyleBackColor = false;
         this.RobotCamera9Button.HoldTimeout += new NICBOT.GUI.CameraSelectButton.HoldTimeoutHandler(this.CameraButton_HoldTimeout);
         this.RobotCamera9Button.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RobotCameraButton_MouseClick);
         // 
         // RobotCamera8Button
         // 
         this.RobotCamera8Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RobotCamera8Button.Camera = NICBOT.GUI.CameraLocations.robotFrontUpperForward;
         this.RobotCamera8Button.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RobotCamera8Button.CenterForeColor = System.Drawing.Color.White;
         this.RobotCamera8Button.CenterLevel = 75;
         this.RobotCamera8Button.CenterVisible = true;
         this.RobotCamera8Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.RobotCamera8Button.HoldRepeat = false;
         this.RobotCamera8Button.HoldRepeatInterval = 0;
         this.RobotCamera8Button.HoldTimeoutEnable = true;
         this.RobotCamera8Button.HoldTimeoutInterval = 100;
         this.RobotCamera8Button.IndicatorBetweenSpace = 4;
         this.RobotCamera8Button.IndicatorEdgeSpace = 4;
         this.RobotCamera8Button.LeftColor = System.Drawing.Color.Maroon;
         this.RobotCamera8Button.LeftVisible = true;
         this.RobotCamera8Button.Location = new System.Drawing.Point(236, 8);
         this.RobotCamera8Button.Name = "RobotCamera8Button";
         this.RobotCamera8Button.RightColor = System.Drawing.Color.DarkBlue;
         this.RobotCamera8Button.RightVisible = true;
         this.RobotCamera8Button.Size = new System.Drawing.Size(107, 80);
         this.RobotCamera8Button.TabIndex = 48;
         this.RobotCamera8Button.Text = "FRONT UPPER FORWARD";
         this.RobotCamera8Button.UseVisualStyleBackColor = false;
         this.RobotCamera8Button.HoldTimeout += new NICBOT.GUI.CameraSelectButton.HoldTimeoutHandler(this.CameraButton_HoldTimeout);
         this.RobotCamera8Button.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RobotCameraButton_MouseClick);
         // 
         // RobotCamera7Button
         // 
         this.RobotCamera7Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RobotCamera7Button.Camera = NICBOT.GUI.CameraLocations.robotRearUpperBack;
         this.RobotCamera7Button.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RobotCamera7Button.CenterForeColor = System.Drawing.Color.White;
         this.RobotCamera7Button.CenterLevel = 75;
         this.RobotCamera7Button.CenterVisible = true;
         this.RobotCamera7Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.RobotCamera7Button.HoldRepeat = false;
         this.RobotCamera7Button.HoldRepeatInterval = 0;
         this.RobotCamera7Button.HoldTimeoutEnable = true;
         this.RobotCamera7Button.HoldTimeoutInterval = 100;
         this.RobotCamera7Button.IndicatorBetweenSpace = 4;
         this.RobotCamera7Button.IndicatorEdgeSpace = 4;
         this.RobotCamera7Button.LeftColor = System.Drawing.Color.Maroon;
         this.RobotCamera7Button.LeftVisible = true;
         this.RobotCamera7Button.Location = new System.Drawing.Point(8, 96);
         this.RobotCamera7Button.Name = "RobotCamera7Button";
         this.RobotCamera7Button.RightColor = System.Drawing.Color.DarkBlue;
         this.RobotCamera7Button.RightVisible = true;
         this.RobotCamera7Button.Size = new System.Drawing.Size(107, 80);
         this.RobotCamera7Button.TabIndex = 47;
         this.RobotCamera7Button.Text = "REAR UPPER BACK";
         this.RobotCamera7Button.UseVisualStyleBackColor = false;
         this.RobotCamera7Button.HoldTimeout += new NICBOT.GUI.CameraSelectButton.HoldTimeoutHandler(this.CameraButton_HoldTimeout);
         this.RobotCamera7Button.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RobotCameraButton_MouseClick);
         // 
         // RobotCamera6Button
         // 
         this.RobotCamera6Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RobotCamera6Button.Camera = NICBOT.GUI.CameraLocations.robotFffDrill;
         this.RobotCamera6Button.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RobotCamera6Button.CenterForeColor = System.Drawing.Color.White;
         this.RobotCamera6Button.CenterLevel = 75;
         this.RobotCamera6Button.CenterVisible = true;
         this.RobotCamera6Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.RobotCamera6Button.HoldRepeat = false;
         this.RobotCamera6Button.HoldRepeatInterval = 0;
         this.RobotCamera6Button.HoldTimeoutEnable = true;
         this.RobotCamera6Button.HoldTimeoutInterval = 100;
         this.RobotCamera6Button.IndicatorBetweenSpace = 4;
         this.RobotCamera6Button.IndicatorEdgeSpace = 4;
         this.RobotCamera6Button.LeftColor = System.Drawing.Color.Maroon;
         this.RobotCamera6Button.LeftVisible = true;
         this.RobotCamera6Button.Location = new System.Drawing.Point(123, 184);
         this.RobotCamera6Button.Name = "RobotCamera6Button";
         this.RobotCamera6Button.RightColor = System.Drawing.Color.DarkBlue;
         this.RobotCamera6Button.RightVisible = true;
         this.RobotCamera6Button.Size = new System.Drawing.Size(107, 80);
         this.RobotCamera6Button.TabIndex = 46;
         this.RobotCamera6Button.Text = "CAM6";
         this.RobotCamera6Button.UseVisualStyleBackColor = false;
         this.RobotCamera6Button.HoldTimeout += new NICBOT.GUI.CameraSelectButton.HoldTimeoutHandler(this.CameraButton_HoldTimeout);
         this.RobotCamera6Button.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RobotCameraButton_MouseClick);
         // 
         // RobotCamera5Button
         // 
         this.RobotCamera5Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RobotCamera5Button.Camera = NICBOT.GUI.CameraLocations.robotRearUpperDown;
         this.RobotCamera5Button.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RobotCamera5Button.CenterForeColor = System.Drawing.Color.White;
         this.RobotCamera5Button.CenterLevel = 75;
         this.RobotCamera5Button.CenterVisible = true;
         this.RobotCamera5Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.RobotCamera5Button.HoldRepeat = false;
         this.RobotCamera5Button.HoldRepeatInterval = 0;
         this.RobotCamera5Button.HoldTimeoutEnable = true;
         this.RobotCamera5Button.HoldTimeoutInterval = 100;
         this.RobotCamera5Button.IndicatorBetweenSpace = 4;
         this.RobotCamera5Button.IndicatorEdgeSpace = 4;
         this.RobotCamera5Button.LeftColor = System.Drawing.Color.Maroon;
         this.RobotCamera5Button.LeftVisible = true;
         this.RobotCamera5Button.Location = new System.Drawing.Point(123, 96);
         this.RobotCamera5Button.Name = "RobotCamera5Button";
         this.RobotCamera5Button.RightColor = System.Drawing.Color.DarkBlue;
         this.RobotCamera5Button.RightVisible = true;
         this.RobotCamera5Button.Size = new System.Drawing.Size(107, 80);
         this.RobotCamera5Button.TabIndex = 45;
         this.RobotCamera5Button.Text = "REAR UPPER DOWN";
         this.RobotCamera5Button.UseVisualStyleBackColor = false;
         this.RobotCamera5Button.HoldTimeout += new NICBOT.GUI.CameraSelectButton.HoldTimeoutHandler(this.CameraButton_HoldTimeout);
         this.RobotCamera5Button.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RobotCameraButton_MouseClick);
         // 
         // RobotCamera4Button
         // 
         this.RobotCamera4Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RobotCamera4Button.Camera = NICBOT.GUI.CameraLocations.robotRearUpperForward;
         this.RobotCamera4Button.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RobotCamera4Button.CenterForeColor = System.Drawing.Color.White;
         this.RobotCamera4Button.CenterLevel = 75;
         this.RobotCamera4Button.CenterVisible = true;
         this.RobotCamera4Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.RobotCamera4Button.HoldRepeat = false;
         this.RobotCamera4Button.HoldRepeatInterval = 0;
         this.RobotCamera4Button.HoldTimeoutEnable = true;
         this.RobotCamera4Button.HoldTimeoutInterval = 100;
         this.RobotCamera4Button.IndicatorBetweenSpace = 4;
         this.RobotCamera4Button.IndicatorEdgeSpace = 4;
         this.RobotCamera4Button.LeftColor = System.Drawing.Color.Maroon;
         this.RobotCamera4Button.LeftVisible = true;
         this.RobotCamera4Button.Location = new System.Drawing.Point(236, 96);
         this.RobotCamera4Button.Name = "RobotCamera4Button";
         this.RobotCamera4Button.RightColor = System.Drawing.Color.DarkBlue;
         this.RobotCamera4Button.RightVisible = true;
         this.RobotCamera4Button.Size = new System.Drawing.Size(107, 80);
         this.RobotCamera4Button.TabIndex = 44;
         this.RobotCamera4Button.Text = "REAR UPPER FORWARD";
         this.RobotCamera4Button.UseVisualStyleBackColor = false;
         this.RobotCamera4Button.HoldTimeout += new NICBOT.GUI.CameraSelectButton.HoldTimeoutHandler(this.CameraButton_HoldTimeout);
         this.RobotCamera4Button.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RobotCameraButton_MouseClick);
         // 
         // RobotCamera3Button
         // 
         this.RobotCamera3Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RobotCamera3Button.Camera = NICBOT.GUI.CameraLocations.robotFrontUpperDown;
         this.RobotCamera3Button.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RobotCamera3Button.CenterForeColor = System.Drawing.Color.White;
         this.RobotCamera3Button.CenterLevel = 75;
         this.RobotCamera3Button.CenterVisible = true;
         this.RobotCamera3Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.RobotCamera3Button.HoldRepeat = false;
         this.RobotCamera3Button.HoldRepeatInterval = 0;
         this.RobotCamera3Button.HoldTimeoutEnable = true;
         this.RobotCamera3Button.HoldTimeoutInterval = 100;
         this.RobotCamera3Button.IndicatorBetweenSpace = 4;
         this.RobotCamera3Button.IndicatorEdgeSpace = 4;
         this.RobotCamera3Button.LeftColor = System.Drawing.Color.Maroon;
         this.RobotCamera3Button.LeftVisible = true;
         this.RobotCamera3Button.Location = new System.Drawing.Point(123, 8);
         this.RobotCamera3Button.Name = "RobotCamera3Button";
         this.RobotCamera3Button.RightColor = System.Drawing.Color.DarkBlue;
         this.RobotCamera3Button.RightVisible = true;
         this.RobotCamera3Button.Size = new System.Drawing.Size(107, 80);
         this.RobotCamera3Button.TabIndex = 43;
         this.RobotCamera3Button.Text = "FRONT UPPER DOWN";
         this.RobotCamera3Button.UseVisualStyleBackColor = false;
         this.RobotCamera3Button.HoldTimeout += new NICBOT.GUI.CameraSelectButton.HoldTimeoutHandler(this.CameraButton_HoldTimeout);
         this.RobotCamera3Button.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RobotCameraButton_MouseClick);
         // 
         // RobotCamera2Button
         // 
         this.RobotCamera2Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RobotCamera2Button.Camera = NICBOT.GUI.CameraLocations.robotLowerBack;
         this.RobotCamera2Button.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RobotCamera2Button.CenterForeColor = System.Drawing.Color.White;
         this.RobotCamera2Button.CenterLevel = 75;
         this.RobotCamera2Button.CenterVisible = true;
         this.RobotCamera2Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.RobotCamera2Button.HoldRepeat = false;
         this.RobotCamera2Button.HoldRepeatInterval = 0;
         this.RobotCamera2Button.HoldTimeoutEnable = true;
         this.RobotCamera2Button.HoldTimeoutInterval = 100;
         this.RobotCamera2Button.IndicatorBetweenSpace = 4;
         this.RobotCamera2Button.IndicatorEdgeSpace = 4;
         this.RobotCamera2Button.LeftColor = System.Drawing.Color.Maroon;
         this.RobotCamera2Button.LeftVisible = true;
         this.RobotCamera2Button.Location = new System.Drawing.Point(8, 184);
         this.RobotCamera2Button.Name = "RobotCamera2Button";
         this.RobotCamera2Button.RightColor = System.Drawing.Color.DarkBlue;
         this.RobotCamera2Button.RightVisible = true;
         this.RobotCamera2Button.Size = new System.Drawing.Size(107, 80);
         this.RobotCamera2Button.TabIndex = 42;
         this.RobotCamera2Button.Text = "LOWER BACK";
         this.RobotCamera2Button.UseVisualStyleBackColor = false;
         this.RobotCamera2Button.HoldTimeout += new NICBOT.GUI.CameraSelectButton.HoldTimeoutHandler(this.CameraButton_HoldTimeout);
         this.RobotCamera2Button.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RobotCameraButton_MouseClick);
         // 
         // RobotCamera1Button
         // 
         this.RobotCamera1Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RobotCamera1Button.Camera = NICBOT.GUI.CameraLocations.robotFrontUpperBack;
         this.RobotCamera1Button.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RobotCamera1Button.CenterForeColor = System.Drawing.Color.White;
         this.RobotCamera1Button.CenterLevel = 75;
         this.RobotCamera1Button.CenterVisible = true;
         this.RobotCamera1Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.RobotCamera1Button.HoldRepeat = false;
         this.RobotCamera1Button.HoldRepeatInterval = 0;
         this.RobotCamera1Button.HoldTimeoutEnable = true;
         this.RobotCamera1Button.HoldTimeoutInterval = 100;
         this.RobotCamera1Button.IndicatorBetweenSpace = 4;
         this.RobotCamera1Button.IndicatorEdgeSpace = 4;
         this.RobotCamera1Button.LeftColor = System.Drawing.Color.Maroon;
         this.RobotCamera1Button.LeftVisible = true;
         this.RobotCamera1Button.Location = new System.Drawing.Point(8, 8);
         this.RobotCamera1Button.Name = "RobotCamera1Button";
         this.RobotCamera1Button.RightColor = System.Drawing.Color.DarkBlue;
         this.RobotCamera1Button.RightVisible = true;
         this.RobotCamera1Button.Size = new System.Drawing.Size(107, 80);
         this.RobotCamera1Button.TabIndex = 30;
         this.RobotCamera1Button.Text = "FRONT UPPER BACK";
         this.RobotCamera1Button.UseVisualStyleBackColor = false;
         this.RobotCamera1Button.HoldTimeout += new NICBOT.GUI.CameraSelectButton.HoldTimeoutHandler(this.CameraButton_HoldTimeout);
         this.RobotCamera1Button.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RobotCameraButton_MouseClick);
         // 
         // panel25
         // 
         this.panel25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.panel25.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
         this.panel25.Controls.Add(this.lineControl2);
         this.panel25.Controls.Add(this.lineControl1);
         this.panel25.Controls.Add(this.borderedPanel9);
         this.panel25.Controls.Add(this.borderedPanel8);
         this.panel25.Controls.Add(this.BodyDrillButton);
         this.panel25.Controls.Add(this.BodyClosedButton);
         this.panel25.Controls.Add(this.CustomSetupButton);
         this.panel25.Controls.Add(this.label15);
         this.panel25.Controls.Add(this.borderedPanel7);
         this.panel25.ForeColor = System.Drawing.Color.White;
         this.panel25.Location = new System.Drawing.Point(274, 322);
         this.panel25.Name = "panel25";
         this.panel25.Size = new System.Drawing.Size(386, 251);
         this.panel25.TabIndex = 27;
         // 
         // lineControl2
         // 
         this.lineControl2.BackColor = System.Drawing.Color.Transparent;
         this.lineControl2.EdgeColor = System.Drawing.Color.Black;
         this.lineControl2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
         this.lineControl2.LineType = NICBOT.GUI.LineControl.LineDrawType.DiagonalDown;
         this.lineControl2.LineWeight = 10;
         this.lineControl2.Location = new System.Drawing.Point(248, 129);
         this.lineControl2.Name = "lineControl2";
         this.lineControl2.Opacity = 100;
         this.lineControl2.ShowBackground = false;
         this.lineControl2.ShowEdge = true;
         this.lineControl2.Size = new System.Drawing.Size(14, 14);
         this.lineControl2.TabIndex = 31;
         this.lineControl2.Text = "lineControl2";
         // 
         // lineControl1
         // 
         this.lineControl1.BackColor = System.Drawing.Color.Transparent;
         this.lineControl1.EdgeColor = System.Drawing.Color.Black;
         this.lineControl1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
         this.lineControl1.LineType = NICBOT.GUI.LineControl.LineDrawType.DiagonalUp;
         this.lineControl1.LineWeight = 10;
         this.lineControl1.Location = new System.Drawing.Point(124, 129);
         this.lineControl1.Name = "lineControl1";
         this.lineControl1.Opacity = 100;
         this.lineControl1.ShowBackground = false;
         this.lineControl1.ShowEdge = true;
         this.lineControl1.Size = new System.Drawing.Size(14, 14);
         this.lineControl1.TabIndex = 30;
         this.lineControl1.Text = "lineControl1";
         // 
         // borderedPanel9
         // 
         this.borderedPanel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
         this.borderedPanel9.Controls.Add(this.BodyFrontReleaseButton);
         this.borderedPanel9.EdgeWeight = 1;
         this.borderedPanel9.Location = new System.Drawing.Point(254, 136);
         this.borderedPanel9.Name = "borderedPanel9";
         this.borderedPanel9.Size = new System.Drawing.Size(124, 107);
         this.borderedPanel9.TabIndex = 144;
         // 
         // BodyFrontReleaseButton
         // 
         this.BodyFrontReleaseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BodyFrontReleaseButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BodyFrontReleaseButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BodyFrontReleaseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.BodyFrontReleaseButton.ForeColor = System.Drawing.Color.Black;
         this.BodyFrontReleaseButton.HoldTimeoutEnable = true;
         this.BodyFrontReleaseButton.HoldTimeoutInterval = 100;
         this.BodyFrontReleaseButton.Location = new System.Drawing.Point(8, 8);
         this.BodyFrontReleaseButton.Name = "BodyFrontReleaseButton";
         this.BodyFrontReleaseButton.Size = new System.Drawing.Size(107, 90);
         this.BodyFrontReleaseButton.TabIndex = 143;
         this.BodyFrontReleaseButton.Text = "FRONT RELEASE";
         this.BodyFrontReleaseButton.UseVisualStyleBackColor = false;
         this.BodyFrontReleaseButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.BodyFrontReleaseButton_HoldTimeout);
         // 
         // borderedPanel8
         // 
         this.borderedPanel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
         this.borderedPanel8.Controls.Add(this.BodyRearReleaseButton);
         this.borderedPanel8.EdgeWeight = 1;
         this.borderedPanel8.Location = new System.Drawing.Point(8, 136);
         this.borderedPanel8.Name = "borderedPanel8";
         this.borderedPanel8.Size = new System.Drawing.Size(124, 107);
         this.borderedPanel8.TabIndex = 143;
         // 
         // BodyRearReleaseButton
         // 
         this.BodyRearReleaseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BodyRearReleaseButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BodyRearReleaseButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BodyRearReleaseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.BodyRearReleaseButton.ForeColor = System.Drawing.Color.Black;
         this.BodyRearReleaseButton.HoldTimeoutEnable = true;
         this.BodyRearReleaseButton.HoldTimeoutInterval = 100;
         this.BodyRearReleaseButton.Location = new System.Drawing.Point(8, 8);
         this.BodyRearReleaseButton.Name = "BodyRearReleaseButton";
         this.BodyRearReleaseButton.Size = new System.Drawing.Size(107, 90);
         this.BodyRearReleaseButton.TabIndex = 142;
         this.BodyRearReleaseButton.Text = "REAR RELEASE";
         this.BodyRearReleaseButton.UseVisualStyleBackColor = false;
         this.BodyRearReleaseButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.BodyRearReleaseButton_HoldTimeout);
         // 
         // BodyDrillButton
         // 
         this.BodyDrillButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BodyDrillButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BodyDrillButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BodyDrillButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.BodyDrillButton.ForeColor = System.Drawing.Color.Black;
         this.BodyDrillButton.HoldTimeoutEnable = true;
         this.BodyDrillButton.HoldTimeoutInterval = 100;
         this.BodyDrillButton.Location = new System.Drawing.Point(262, 38);
         this.BodyDrillButton.Name = "BodyDrillButton";
         this.BodyDrillButton.Size = new System.Drawing.Size(107, 90);
         this.BodyDrillButton.TabIndex = 141;
         this.BodyDrillButton.Text = "DRILL";
         this.BodyDrillButton.UseVisualStyleBackColor = false;
         this.BodyDrillButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.BodyDrillButton_HoldTimeout);
         // 
         // BodyClosedButton
         // 
         this.BodyClosedButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BodyClosedButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BodyClosedButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BodyClosedButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.BodyClosedButton.ForeColor = System.Drawing.Color.Black;
         this.BodyClosedButton.HoldTimeoutEnable = true;
         this.BodyClosedButton.HoldTimeoutInterval = 100;
         this.BodyClosedButton.Location = new System.Drawing.Point(16, 38);
         this.BodyClosedButton.Name = "BodyClosedButton";
         this.BodyClosedButton.Size = new System.Drawing.Size(107, 90);
         this.BodyClosedButton.TabIndex = 140;
         this.BodyClosedButton.Text = "CLOSED (LAUNCH)";
         this.BodyClosedButton.UseVisualStyleBackColor = false;
         this.BodyClosedButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.BodyClosedButton_HoldTimeout);
         // 
         // CustomSetupButton
         // 
         this.CustomSetupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.CustomSetupButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.CustomSetupButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.CustomSetupButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.CustomSetupButton.ForeColor = System.Drawing.Color.Black;
         this.CustomSetupButton.HoldTimeoutEnable = true;
         this.CustomSetupButton.HoldTimeoutInterval = 100;
         this.CustomSetupButton.Location = new System.Drawing.Point(139, 145);
         this.CustomSetupButton.Name = "CustomSetupButton";
         this.CustomSetupButton.Size = new System.Drawing.Size(107, 90);
         this.CustomSetupButton.TabIndex = 139;
         this.CustomSetupButton.Text = "CUSTOM CONFIG";
         this.CustomSetupButton.UseVisualStyleBackColor = false;
         this.CustomSetupButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.CustomSetupButton_HoldTimeout);
         // 
         // label15
         // 
         this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.label15.ForeColor = System.Drawing.Color.Black;
         this.label15.Location = new System.Drawing.Point(63, 4);
         this.label15.Name = "label15";
         this.label15.Size = new System.Drawing.Size(260, 23);
         this.label15.TabIndex = 138;
         this.label15.Text = "CONFIGURATION";
         this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // borderedPanel7
         // 
         this.borderedPanel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
         this.borderedPanel7.Controls.Add(this.BodyOpenButton);
         this.borderedPanel7.EdgeWeight = 1;
         this.borderedPanel7.Location = new System.Drawing.Point(131, 30);
         this.borderedPanel7.Name = "borderedPanel7";
         this.borderedPanel7.Size = new System.Drawing.Size(124, 107);
         this.borderedPanel7.TabIndex = 142;
         // 
         // BodyOpenButton
         // 
         this.BodyOpenButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.BodyOpenButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.BodyOpenButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.BodyOpenButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.BodyOpenButton.ForeColor = System.Drawing.Color.Black;
         this.BodyOpenButton.HoldTimeoutEnable = true;
         this.BodyOpenButton.HoldTimeoutInterval = 100;
         this.BodyOpenButton.Location = new System.Drawing.Point(8, 8);
         this.BodyOpenButton.Name = "BodyOpenButton";
         this.BodyOpenButton.Size = new System.Drawing.Size(107, 90);
         this.BodyOpenButton.TabIndex = 141;
         this.BodyOpenButton.Text = "OPEN (MOVE)";
         this.BodyOpenButton.UseVisualStyleBackColor = false;
         this.BodyOpenButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.BodyOpenButton_HoldTimeout);
         // 
         // UpdateTimer
         // 
         this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
         // 
         // panel24
         // 
         this.panel24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.panel24.Controls.Add(this.LaunchCameraSelectButton);
         this.panel24.Controls.Add(this.RobotCameraBSelectButton);
         this.panel24.Controls.Add(this.LightSelectButton);
         this.panel24.Controls.Add(this.RobotCameraASelectButton);
         this.panel24.Location = new System.Drawing.Point(1438, 411);
         this.panel24.Name = "panel24";
         this.panel24.Size = new System.Drawing.Size(480, 83);
         this.panel24.TabIndex = 32;
         // 
         // LaunchCameraSelectButton
         // 
         this.LaunchCameraSelectButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.LaunchCameraSelectButton.Camera = NICBOT.GUI.CameraLocations.robotFrontUpperBack;
         this.LaunchCameraSelectButton.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.LaunchCameraSelectButton.CenterForeColor = System.Drawing.Color.White;
         this.LaunchCameraSelectButton.CenterLevel = 75;
         this.LaunchCameraSelectButton.CenterVisible = false;
         this.LaunchCameraSelectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.LaunchCameraSelectButton.HoldRepeat = false;
         this.LaunchCameraSelectButton.HoldRepeatInterval = 0;
         this.LaunchCameraSelectButton.HoldTimeoutEnable = true;
         this.LaunchCameraSelectButton.HoldTimeoutInterval = 100;
         this.LaunchCameraSelectButton.IndicatorBetweenSpace = 4;
         this.LaunchCameraSelectButton.IndicatorEdgeSpace = 4;
         this.LaunchCameraSelectButton.LeftColor = System.Drawing.Color.Green;
         this.LaunchCameraSelectButton.LeftVisible = true;
         this.LaunchCameraSelectButton.Location = new System.Drawing.Point(8, 8);
         this.LaunchCameraSelectButton.Name = "LaunchCameraSelectButton";
         this.LaunchCameraSelectButton.RightColor = System.Drawing.Color.DarkBlue;
         this.LaunchCameraSelectButton.RightVisible = false;
         this.LaunchCameraSelectButton.Size = new System.Drawing.Size(107, 67);
         this.LaunchCameraSelectButton.TabIndex = 54;
         this.LaunchCameraSelectButton.Text = "LAUNCH CAMERA";
         this.LaunchCameraSelectButton.UseVisualStyleBackColor = false;
         this.LaunchCameraSelectButton.Click += new System.EventHandler(this.LaunchCameraSelectButton_Click);
         // 
         // RobotCameraBSelectButton
         // 
         this.RobotCameraBSelectButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RobotCameraBSelectButton.Camera = NICBOT.GUI.CameraLocations.robotFrontUpperBack;
         this.RobotCameraBSelectButton.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RobotCameraBSelectButton.CenterForeColor = System.Drawing.Color.White;
         this.RobotCameraBSelectButton.CenterLevel = 75;
         this.RobotCameraBSelectButton.CenterVisible = false;
         this.RobotCameraBSelectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.RobotCameraBSelectButton.HoldRepeat = false;
         this.RobotCameraBSelectButton.HoldRepeatInterval = 0;
         this.RobotCameraBSelectButton.HoldTimeoutEnable = true;
         this.RobotCameraBSelectButton.HoldTimeoutInterval = 100;
         this.RobotCameraBSelectButton.IndicatorBetweenSpace = 4;
         this.RobotCameraBSelectButton.IndicatorEdgeSpace = 4;
         this.RobotCameraBSelectButton.LeftColor = System.Drawing.Color.Maroon;
         this.RobotCameraBSelectButton.LeftVisible = false;
         this.RobotCameraBSelectButton.Location = new System.Drawing.Point(365, 8);
         this.RobotCameraBSelectButton.Name = "RobotCameraBSelectButton";
         this.RobotCameraBSelectButton.RightColor = System.Drawing.Color.DarkBlue;
         this.RobotCameraBSelectButton.RightVisible = true;
         this.RobotCameraBSelectButton.Size = new System.Drawing.Size(107, 67);
         this.RobotCameraBSelectButton.TabIndex = 53;
         this.RobotCameraBSelectButton.Text = "ROBOT CAMERA B";
         this.RobotCameraBSelectButton.UseVisualStyleBackColor = false;
         this.RobotCameraBSelectButton.Click += new System.EventHandler(this.RobotCameraBSelectButton_Click);
         // 
         // LightSelectButton
         // 
         this.LightSelectButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.LightSelectButton.Camera = NICBOT.GUI.CameraLocations.robotFrontUpperBack;
         this.LightSelectButton.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.LightSelectButton.CenterForeColor = System.Drawing.Color.White;
         this.LightSelectButton.CenterLevel = 100;
         this.LightSelectButton.CenterVisible = true;
         this.LightSelectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.LightSelectButton.HoldRepeat = false;
         this.LightSelectButton.HoldRepeatInterval = 0;
         this.LightSelectButton.HoldTimeoutEnable = true;
         this.LightSelectButton.HoldTimeoutInterval = 100;
         this.LightSelectButton.IndicatorBetweenSpace = 4;
         this.LightSelectButton.IndicatorEdgeSpace = 4;
         this.LightSelectButton.LeftColor = System.Drawing.Color.Maroon;
         this.LightSelectButton.LeftVisible = false;
         this.LightSelectButton.Location = new System.Drawing.Point(127, 8);
         this.LightSelectButton.Name = "LightSelectButton";
         this.LightSelectButton.RightColor = System.Drawing.Color.DarkBlue;
         this.LightSelectButton.RightVisible = false;
         this.LightSelectButton.Size = new System.Drawing.Size(107, 67);
         this.LightSelectButton.TabIndex = 52;
         this.LightSelectButton.Text = "LIGHTS";
         this.LightSelectButton.UseVisualStyleBackColor = false;
         this.LightSelectButton.Click += new System.EventHandler(this.LightSelectButton_Click);
         // 
         // RobotCameraASelectButton
         // 
         this.RobotCameraASelectButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RobotCameraASelectButton.Camera = NICBOT.GUI.CameraLocations.robotFrontUpperBack;
         this.RobotCameraASelectButton.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.RobotCameraASelectButton.CenterForeColor = System.Drawing.Color.White;
         this.RobotCameraASelectButton.CenterLevel = 75;
         this.RobotCameraASelectButton.CenterVisible = false;
         this.RobotCameraASelectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.RobotCameraASelectButton.HoldRepeat = false;
         this.RobotCameraASelectButton.HoldRepeatInterval = 0;
         this.RobotCameraASelectButton.HoldTimeoutEnable = true;
         this.RobotCameraASelectButton.HoldTimeoutInterval = 100;
         this.RobotCameraASelectButton.IndicatorBetweenSpace = 4;
         this.RobotCameraASelectButton.IndicatorEdgeSpace = 4;
         this.RobotCameraASelectButton.LeftColor = System.Drawing.Color.Maroon;
         this.RobotCameraASelectButton.LeftVisible = true;
         this.RobotCameraASelectButton.Location = new System.Drawing.Point(246, 8);
         this.RobotCameraASelectButton.Name = "RobotCameraASelectButton";
         this.RobotCameraASelectButton.RightColor = System.Drawing.Color.DarkBlue;
         this.RobotCameraASelectButton.RightVisible = false;
         this.RobotCameraASelectButton.Size = new System.Drawing.Size(107, 67);
         this.RobotCameraASelectButton.TabIndex = 51;
         this.RobotCameraASelectButton.Text = "ROBOT CAMERA A";
         this.RobotCameraASelectButton.UseVisualStyleBackColor = false;
         this.RobotCameraASelectButton.Click += new System.EventHandler(this.RobotCameraASelectButton_Click);
         // 
         // panel27
         // 
         this.panel27.BackColor = System.Drawing.Color.Olive;
         this.panel27.Controls.Add(this.LaunchCamera4Button);
         this.panel27.Controls.Add(this.LaunchCamera3Button);
         this.panel27.Controls.Add(this.LaunchCamera2Button);
         this.panel27.Controls.Add(this.LaunchCamera1Button);
         this.panel27.Location = new System.Drawing.Point(1438, 46);
         this.panel27.Name = "panel27";
         this.panel27.Size = new System.Drawing.Size(123, 360);
         this.panel27.TabIndex = 33;
         // 
         // LaunchCamera4Button
         // 
         this.LaunchCamera4Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.LaunchCamera4Button.Camera = NICBOT.GUI.CameraLocations.launchMain;
         this.LaunchCamera4Button.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.LaunchCamera4Button.CenterForeColor = System.Drawing.Color.White;
         this.LaunchCamera4Button.CenterLevel = 75;
         this.LaunchCamera4Button.CenterVisible = true;
         this.LaunchCamera4Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.LaunchCamera4Button.HoldRepeat = false;
         this.LaunchCamera4Button.HoldRepeatInterval = 0;
         this.LaunchCamera4Button.HoldTimeoutEnable = true;
         this.LaunchCamera4Button.HoldTimeoutInterval = 100;
         this.LaunchCamera4Button.IndicatorBetweenSpace = 4;
         this.LaunchCamera4Button.IndicatorEdgeSpace = 4;
         this.LaunchCamera4Button.LeftColor = System.Drawing.Color.Green;
         this.LaunchCamera4Button.LeftVisible = true;
         this.LaunchCamera4Button.Location = new System.Drawing.Point(8, 272);
         this.LaunchCamera4Button.Name = "LaunchCamera4Button";
         this.LaunchCamera4Button.RightColor = System.Drawing.Color.DarkBlue;
         this.LaunchCamera4Button.RightVisible = true;
         this.LaunchCamera4Button.Size = new System.Drawing.Size(107, 80);
         this.LaunchCamera4Button.TabIndex = 34;
         this.LaunchCamera4Button.Text = "MAIN";
         this.LaunchCamera4Button.UseVisualStyleBackColor = false;
         this.LaunchCamera4Button.HoldTimeout += new NICBOT.GUI.CameraSelectButton.HoldTimeoutHandler(this.CameraButton_HoldTimeout);
         this.LaunchCamera4Button.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LaunchCameraButton_MouseClick);
         // 
         // LaunchCamera3Button
         // 
         this.LaunchCamera3Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.LaunchCamera3Button.Camera = NICBOT.GUI.CameraLocations.launchFeeder;
         this.LaunchCamera3Button.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.LaunchCamera3Button.CenterForeColor = System.Drawing.Color.White;
         this.LaunchCamera3Button.CenterLevel = 75;
         this.LaunchCamera3Button.CenterVisible = true;
         this.LaunchCamera3Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.LaunchCamera3Button.HoldRepeat = false;
         this.LaunchCamera3Button.HoldRepeatInterval = 0;
         this.LaunchCamera3Button.HoldTimeoutEnable = true;
         this.LaunchCamera3Button.HoldTimeoutInterval = 100;
         this.LaunchCamera3Button.IndicatorBetweenSpace = 4;
         this.LaunchCamera3Button.IndicatorEdgeSpace = 4;
         this.LaunchCamera3Button.LeftColor = System.Drawing.Color.Green;
         this.LaunchCamera3Button.LeftVisible = true;
         this.LaunchCamera3Button.Location = new System.Drawing.Point(8, 184);
         this.LaunchCamera3Button.Name = "LaunchCamera3Button";
         this.LaunchCamera3Button.RightColor = System.Drawing.Color.DarkBlue;
         this.LaunchCamera3Button.RightVisible = true;
         this.LaunchCamera3Button.Size = new System.Drawing.Size(107, 80);
         this.LaunchCamera3Button.TabIndex = 33;
         this.LaunchCamera3Button.Text = "FEEDER";
         this.LaunchCamera3Button.UseVisualStyleBackColor = false;
         this.LaunchCamera3Button.HoldTimeout += new NICBOT.GUI.CameraSelectButton.HoldTimeoutHandler(this.CameraButton_HoldTimeout);
         this.LaunchCamera3Button.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LaunchCameraButton_MouseClick);
         // 
         // LaunchCamera2Button
         // 
         this.LaunchCamera2Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.LaunchCamera2Button.Camera = NICBOT.GUI.CameraLocations.launchRightGuide;
         this.LaunchCamera2Button.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.LaunchCamera2Button.CenterForeColor = System.Drawing.Color.White;
         this.LaunchCamera2Button.CenterLevel = 75;
         this.LaunchCamera2Button.CenterVisible = true;
         this.LaunchCamera2Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.LaunchCamera2Button.HoldRepeat = false;
         this.LaunchCamera2Button.HoldRepeatInterval = 0;
         this.LaunchCamera2Button.HoldTimeoutEnable = true;
         this.LaunchCamera2Button.HoldTimeoutInterval = 100;
         this.LaunchCamera2Button.IndicatorBetweenSpace = 4;
         this.LaunchCamera2Button.IndicatorEdgeSpace = 4;
         this.LaunchCamera2Button.LeftColor = System.Drawing.Color.Green;
         this.LaunchCamera2Button.LeftVisible = true;
         this.LaunchCamera2Button.Location = new System.Drawing.Point(8, 96);
         this.LaunchCamera2Button.Name = "LaunchCamera2Button";
         this.LaunchCamera2Button.RightColor = System.Drawing.Color.DarkBlue;
         this.LaunchCamera2Button.RightVisible = true;
         this.LaunchCamera2Button.Size = new System.Drawing.Size(107, 80);
         this.LaunchCamera2Button.TabIndex = 32;
         this.LaunchCamera2Button.Text = "RIGHT GUIDE";
         this.LaunchCamera2Button.UseVisualStyleBackColor = false;
         this.LaunchCamera2Button.HoldTimeout += new NICBOT.GUI.CameraSelectButton.HoldTimeoutHandler(this.CameraButton_HoldTimeout);
         this.LaunchCamera2Button.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LaunchCameraButton_MouseClick);
         // 
         // LaunchCamera1Button
         // 
         this.LaunchCamera1Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.LaunchCamera1Button.Camera = NICBOT.GUI.CameraLocations.launchLeftGuide;
         this.LaunchCamera1Button.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.LaunchCamera1Button.CenterForeColor = System.Drawing.Color.White;
         this.LaunchCamera1Button.CenterLevel = 75;
         this.LaunchCamera1Button.CenterVisible = true;
         this.LaunchCamera1Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.LaunchCamera1Button.HoldRepeat = false;
         this.LaunchCamera1Button.HoldRepeatInterval = 0;
         this.LaunchCamera1Button.HoldTimeoutEnable = true;
         this.LaunchCamera1Button.HoldTimeoutInterval = 100;
         this.LaunchCamera1Button.IndicatorBetweenSpace = 4;
         this.LaunchCamera1Button.IndicatorEdgeSpace = 4;
         this.LaunchCamera1Button.LeftColor = System.Drawing.Color.Green;
         this.LaunchCamera1Button.LeftVisible = true;
         this.LaunchCamera1Button.Location = new System.Drawing.Point(8, 8);
         this.LaunchCamera1Button.Name = "LaunchCamera1Button";
         this.LaunchCamera1Button.RightColor = System.Drawing.Color.DarkBlue;
         this.LaunchCamera1Button.RightVisible = true;
         this.LaunchCamera1Button.Size = new System.Drawing.Size(107, 80);
         this.LaunchCamera1Button.TabIndex = 31;
         this.LaunchCamera1Button.Text = "LEFT GUIDE";
         this.LaunchCamera1Button.UseVisualStyleBackColor = false;
         this.LaunchCamera1Button.HoldTimeout += new NICBOT.GUI.CameraSelectButton.HoldTimeoutHandler(this.CameraButton_HoldTimeout);
         this.LaunchCamera1Button.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LaunchCameraButton_MouseClick);
         // 
         // DrillMainPanel
         // 
         this.DrillMainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.DrillMainPanel.Controls.Add(this.borderedPanel10);
         this.DrillMainPanel.Controls.Add(this.DrillManulDisplayButton);
         this.DrillMainPanel.Controls.Add(this.DrillLaserLightButton);
         this.DrillMainPanel.Controls.Add(this.DrillSealModeButton);
         this.DrillMainPanel.Controls.Add(this.DrillSelectionLabel);
         this.DrillMainPanel.Controls.Add(this.DrillExtendedActualValuePanel);
         this.DrillMainPanel.Controls.Add(this.DrillExtendedSetPointValuePanel);
         this.DrillMainPanel.Controls.Add(this.DrillRotationActualSpeedValuePanel);
         this.DrillMainPanel.Controls.Add(this.label28);
         this.DrillMainPanel.Controls.Add(this.label27);
         this.DrillMainPanel.Controls.Add(this.label26);
         this.DrillMainPanel.Controls.Add(this.DrillModeLabel);
         this.DrillMainPanel.Controls.Add(this.label24);
         this.DrillMainPanel.Controls.Add(this.DrillPipePositionLabel);
         this.DrillMainPanel.Controls.Add(this.DrillSetupButton);
         this.DrillMainPanel.Controls.Add(this.DrillRotaionSetPointSpeedValuePanel);
         this.DrillMainPanel.Controls.Add(this.RetractionLimitLightTextBox);
         this.DrillMainPanel.Controls.Add(this.OriginSetLightTextBox);
         this.DrillMainPanel.Controls.Add(this.DrillErrorLightTextBox);
         this.DrillMainPanel.Controls.Add(this.label48);
         this.DrillMainPanel.Controls.Add(this.label47);
         this.DrillMainPanel.Controls.Add(this.label46);
         this.DrillMainPanel.Controls.Add(this.rotatableLabel1);
         this.DrillMainPanel.Controls.Add(this.rotatableLabel2);
         this.DrillMainPanel.Controls.Add(this.rotatableLabel5);
         this.DrillMainPanel.Controls.Add(this.rotatableLabel6);
         this.DrillMainPanel.Location = new System.Drawing.Point(1063, 46);
         this.DrillMainPanel.Name = "DrillMainPanel";
         this.DrillMainPanel.Size = new System.Drawing.Size(369, 431);
         this.DrillMainPanel.TabIndex = 35;
         // 
         // borderedPanel10
         // 
         this.borderedPanel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
         this.borderedPanel10.Controls.Add(this.DrillAutoStopButton);
         this.borderedPanel10.Controls.Add(this.DrillAutoStartButton);
         this.borderedPanel10.Controls.Add(this.DrillAutoPauseResumeButton);
         this.borderedPanel10.EdgeWeight = 1;
         this.borderedPanel10.Location = new System.Drawing.Point(8, 315);
         this.borderedPanel10.Name = "borderedPanel10";
         this.borderedPanel10.Size = new System.Drawing.Size(353, 108);
         this.borderedPanel10.TabIndex = 166;
         // 
         // DrillAutoStopButton
         // 
         this.DrillAutoStopButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillAutoStopButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.DrillAutoStopButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.DrillAutoStopButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.DrillAutoStopButton.Location = new System.Drawing.Point(238, 8);
         this.DrillAutoStopButton.Name = "DrillAutoStopButton";
         this.DrillAutoStopButton.Size = new System.Drawing.Size(107, 90);
         this.DrillAutoStopButton.TabIndex = 164;
         this.DrillAutoStopButton.Text = "STOP   AUTO  DRILL";
         this.DrillAutoStopButton.UseVisualStyleBackColor = false;
         this.DrillAutoStopButton.Click += new System.EventHandler(this.DrillAutoStopButton_Click);
         // 
         // DrillAutoStartButton
         // 
         this.DrillAutoStartButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillAutoStartButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.DrillAutoStartButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.DrillAutoStartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.DrillAutoStartButton.HoldTimeoutEnable = true;
         this.DrillAutoStartButton.HoldTimeoutInterval = 100;
         this.DrillAutoStartButton.Location = new System.Drawing.Point(8, 8);
         this.DrillAutoStartButton.Name = "DrillAutoStartButton";
         this.DrillAutoStartButton.Size = new System.Drawing.Size(107, 90);
         this.DrillAutoStartButton.TabIndex = 63;
         this.DrillAutoStartButton.Text = "START  AUTO  DRILL";
         this.DrillAutoStartButton.UseVisualStyleBackColor = false;
         this.DrillAutoStartButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.DrillAutoStartButton_HoldTimeout);
         // 
         // DrillAutoPauseResumeButton
         // 
         this.DrillAutoPauseResumeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillAutoPauseResumeButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.DrillAutoPauseResumeButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.DrillAutoPauseResumeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.DrillAutoPauseResumeButton.Location = new System.Drawing.Point(123, 8);
         this.DrillAutoPauseResumeButton.Name = "DrillAutoPauseResumeButton";
         this.DrillAutoPauseResumeButton.Size = new System.Drawing.Size(107, 90);
         this.DrillAutoPauseResumeButton.TabIndex = 163;
         this.DrillAutoPauseResumeButton.Text = "PAUSE";
         this.DrillAutoPauseResumeButton.UseVisualStyleBackColor = false;
         this.DrillAutoPauseResumeButton.Click += new System.EventHandler(this.DrillAutoPauseResumeButton_Click);
         // 
         // DrillManulDisplayButton
         // 
         this.DrillManulDisplayButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillManulDisplayButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.DrillManulDisplayButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.DrillManulDisplayButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.DrillManulDisplayButton.Location = new System.Drawing.Point(246, 215);
         this.DrillManulDisplayButton.Name = "DrillManulDisplayButton";
         this.DrillManulDisplayButton.Size = new System.Drawing.Size(107, 90);
         this.DrillManulDisplayButton.TabIndex = 163;
         this.DrillManulDisplayButton.Text = "SHOW MANUAL";
         this.DrillManulDisplayButton.UseVisualStyleBackColor = false;
         this.DrillManulDisplayButton.Click += new System.EventHandler(this.DrillManulDisplayButton_Click);
         // 
         // DrillLaserLightButton
         // 
         this.DrillLaserLightButton.AutomaticToggle = true;
         this.DrillLaserLightButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillLaserLightButton.DisabledBackColor = System.Drawing.Color.Black;
         this.DrillLaserLightButton.DisabledForeColor = System.Drawing.Color.Gray;
         this.DrillLaserLightButton.DisabledOptionBackColor = System.Drawing.Color.Black;
         this.DrillLaserLightButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DrillLaserLightButton.HoldEnable = false;
         this.DrillLaserLightButton.HoldTimeoutInterval = 100;
         this.DrillLaserLightButton.Location = new System.Drawing.Point(16, 215);
         this.DrillLaserLightButton.Name = "DrillLaserLightButton";
         this.DrillLaserLightButton.OptionASelected = true;
         this.DrillLaserLightButton.OptionAText = "ON";
         this.DrillLaserLightButton.OptionBSelected = false;
         this.DrillLaserLightButton.OptionBText = "OFF";
         this.DrillLaserLightButton.OptionCenterWidth = 2;
         this.DrillLaserLightButton.OptionEdgeHeight = 8;
         this.DrillLaserLightButton.OptionHeight = 22;
         this.DrillLaserLightButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.DrillLaserLightButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
         this.DrillLaserLightButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.DrillLaserLightButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.DrillLaserLightButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DrillLaserLightButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.DrillLaserLightButton.OptionWidth = 45;
         this.DrillLaserLightButton.Size = new System.Drawing.Size(107, 90);
         this.DrillLaserLightButton.TabIndex = 145;
         this.DrillLaserLightButton.Text = "LASER SIGHT";
         this.DrillLaserLightButton.UseVisualStyleBackColor = false;
         this.DrillLaserLightButton.Click += new System.EventHandler(this.DrillLaserLightButton_Click);
         // 
         // DrillSealModeButton
         // 
         this.DrillSealModeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillSealModeButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.DrillSealModeButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.DrillSealModeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DrillSealModeButton.HoldTimeoutEnable = true;
         this.DrillSealModeButton.HoldTimeoutInterval = 100;
         this.DrillSealModeButton.Location = new System.Drawing.Point(246, 27);
         this.DrillSealModeButton.Name = "DrillSealModeButton";
         this.DrillSealModeButton.Size = new System.Drawing.Size(107, 67);
         this.DrillSealModeButton.TabIndex = 142;
         this.DrillSealModeButton.Text = "SWITCH TO SEAL MODE";
         this.DrillSealModeButton.UseVisualStyleBackColor = false;
         this.DrillSealModeButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.DrillSealModeButton_HoldTimeout);
         // 
         // DrillSelectionLabel
         // 
         this.DrillSelectionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.DrillSelectionLabel.Location = new System.Drawing.Point(57, 4);
         this.DrillSelectionLabel.Name = "DrillSelectionLabel";
         this.DrillSelectionLabel.Size = new System.Drawing.Size(260, 23);
         this.DrillSelectionLabel.TabIndex = 141;
         this.DrillSelectionLabel.Text = "FRONT DRILL SELECTED";
         this.DrillSelectionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // DrillExtendedActualValuePanel
         // 
         this.DrillExtendedActualValuePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.DrillExtendedActualValuePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.DrillExtendedActualValuePanel.Enabled = false;
         this.DrillExtendedActualValuePanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DrillExtendedActualValuePanel.ForeColor = System.Drawing.Color.Silver;
         this.DrillExtendedActualValuePanel.HoldTimeoutEnable = false;
         this.DrillExtendedActualValuePanel.HoldTimeoutInterval = 100;
         this.DrillExtendedActualValuePanel.Location = new System.Drawing.Point(20, 116);
         this.DrillExtendedActualValuePanel.Name = "DrillExtendedActualValuePanel";
         this.DrillExtendedActualValuePanel.Size = new System.Drawing.Size(99, 42);
         this.DrillExtendedActualValuePanel.TabIndex = 139;
         this.DrillExtendedActualValuePanel.ValueText = "##### mm";
         this.DrillExtendedActualValuePanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // DrillExtendedSetPointValuePanel
         // 
         this.DrillExtendedSetPointValuePanel.BackColor = System.Drawing.Color.Black;
         this.DrillExtendedSetPointValuePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.DrillExtendedSetPointValuePanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DrillExtendedSetPointValuePanel.ForeColor = System.Drawing.Color.Silver;
         this.DrillExtendedSetPointValuePanel.HoldTimeoutEnable = true;
         this.DrillExtendedSetPointValuePanel.HoldTimeoutInterval = 100;
         this.DrillExtendedSetPointValuePanel.Location = new System.Drawing.Point(20, 165);
         this.DrillExtendedSetPointValuePanel.Name = "DrillExtendedSetPointValuePanel";
         this.DrillExtendedSetPointValuePanel.Size = new System.Drawing.Size(99, 42);
         this.DrillExtendedSetPointValuePanel.TabIndex = 36;
         this.DrillExtendedSetPointValuePanel.ValueText = "##### mm";
         this.DrillExtendedSetPointValuePanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.DrillExtendedSetPointValuePanel.HoldTimeout += new NICBOT.GUI.TextPanel.HoldTimeoutHandler2(this.DrillExtendedValuePanel_HoldTimeout);
         // 
         // DrillRotationActualSpeedValuePanel
         // 
         this.DrillRotationActualSpeedValuePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.DrillRotationActualSpeedValuePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.DrillRotationActualSpeedValuePanel.Enabled = false;
         this.DrillRotationActualSpeedValuePanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DrillRotationActualSpeedValuePanel.ForeColor = System.Drawing.Color.Silver;
         this.DrillRotationActualSpeedValuePanel.HoldTimeoutEnable = false;
         this.DrillRotationActualSpeedValuePanel.HoldTimeoutInterval = 100;
         this.DrillRotationActualSpeedValuePanel.Location = new System.Drawing.Point(135, 116);
         this.DrillRotationActualSpeedValuePanel.Name = "DrillRotationActualSpeedValuePanel";
         this.DrillRotationActualSpeedValuePanel.Size = new System.Drawing.Size(99, 42);
         this.DrillRotationActualSpeedValuePanel.TabIndex = 140;
         this.DrillRotationActualSpeedValuePanel.ValueText = "##### mm";
         this.DrillRotationActualSpeedValuePanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label28
         // 
         this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label28.Location = new System.Drawing.Point(9, 92);
         this.label28.Name = "label28";
         this.label28.Size = new System.Drawing.Size(120, 20);
         this.label28.TabIndex = 138;
         this.label28.Text = "INDEX POSITION";
         this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label27
         // 
         this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label27.Location = new System.Drawing.Point(131, 92);
         this.label27.Name = "label27";
         this.label27.Size = new System.Drawing.Size(107, 20);
         this.label27.TabIndex = 137;
         this.label27.Text = "DRILL SPEED";
         this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label26
         // 
         this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label26.Location = new System.Drawing.Point(122, 30);
         this.label26.Name = "label26";
         this.label26.Size = new System.Drawing.Size(125, 20);
         this.label26.TabIndex = 136;
         this.label26.Text = "AUTO DRILL MODE";
         this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // DrillModeLabel
         // 
         this.DrillModeLabel.BackColor = System.Drawing.SystemColors.ControlDarkDark;
         this.DrillModeLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.DrillModeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DrillModeLabel.ForeColor = System.Drawing.SystemColors.Window;
         this.DrillModeLabel.Location = new System.Drawing.Point(131, 51);
         this.DrillModeLabel.Name = "DrillModeLabel";
         this.DrillModeLabel.Size = new System.Drawing.Size(107, 36);
         this.DrillModeLabel.TabIndex = 135;
         this.DrillModeLabel.Text = "PECK";
         this.DrillModeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label24
         // 
         this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label24.Location = new System.Drawing.Point(15, 30);
         this.label24.Name = "label24";
         this.label24.Size = new System.Drawing.Size(109, 20);
         this.label24.TabIndex = 134;
         this.label24.Text = "PIPE POSITION";
         this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // DrillPipePositionLabel
         // 
         this.DrillPipePositionLabel.BackColor = System.Drawing.SystemColors.ControlDarkDark;
         this.DrillPipePositionLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.DrillPipePositionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.DrillPipePositionLabel.ForeColor = System.Drawing.SystemColors.Window;
         this.DrillPipePositionLabel.Location = new System.Drawing.Point(16, 51);
         this.DrillPipePositionLabel.Name = "DrillPipePositionLabel";
         this.DrillPipePositionLabel.Size = new System.Drawing.Size(107, 36);
         this.DrillPipePositionLabel.TabIndex = 133;
         this.DrillPipePositionLabel.Text = "0";
         this.DrillPipePositionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // DrillSetupButton
         // 
         this.DrillSetupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillSetupButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.DrillSetupButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.DrillSetupButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.DrillSetupButton.HoldTimeoutEnable = true;
         this.DrillSetupButton.HoldTimeoutInterval = 100;
         this.DrillSetupButton.Location = new System.Drawing.Point(131, 215);
         this.DrillSetupButton.Name = "DrillSetupButton";
         this.DrillSetupButton.Size = new System.Drawing.Size(107, 90);
         this.DrillSetupButton.TabIndex = 36;
         this.DrillSetupButton.Text = "DRILL SETUP";
         this.DrillSetupButton.UseVisualStyleBackColor = false;
         this.DrillSetupButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.DrillSetupButton_HoldTimeout);
         // 
         // DrillRotaionSetPointSpeedValuePanel
         // 
         this.DrillRotaionSetPointSpeedValuePanel.BackColor = System.Drawing.Color.Black;
         this.DrillRotaionSetPointSpeedValuePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.DrillRotaionSetPointSpeedValuePanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DrillRotaionSetPointSpeedValuePanel.ForeColor = System.Drawing.Color.White;
         this.DrillRotaionSetPointSpeedValuePanel.HoldTimeoutEnable = true;
         this.DrillRotaionSetPointSpeedValuePanel.HoldTimeoutInterval = 100;
         this.DrillRotaionSetPointSpeedValuePanel.Location = new System.Drawing.Point(135, 165);
         this.DrillRotaionSetPointSpeedValuePanel.Name = "DrillRotaionSetPointSpeedValuePanel";
         this.DrillRotaionSetPointSpeedValuePanel.Size = new System.Drawing.Size(99, 42);
         this.DrillRotaionSetPointSpeedValuePanel.TabIndex = 119;
         this.DrillRotaionSetPointSpeedValuePanel.ValueText = "#### RPM";
         this.DrillRotaionSetPointSpeedValuePanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.DrillRotaionSetPointSpeedValuePanel.HoldTimeout += new NICBOT.GUI.TextPanel.HoldTimeoutHandler2(this.DrillRotaionSpeedValuePanel_HoldTimeout);
         // 
         // RetractionLimitLightTextBox
         // 
         this.RetractionLimitLightTextBox.BackColor = System.Drawing.Color.Black;
         this.RetractionLimitLightTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.RetractionLimitLightTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.RetractionLimitLightTextBox.ForeColor = System.Drawing.SystemColors.Window;
         this.RetractionLimitLightTextBox.Location = new System.Drawing.Point(246, 149);
         this.RetractionLimitLightTextBox.Name = "RetractionLimitLightTextBox";
         this.RetractionLimitLightTextBox.ReadOnly = true;
         this.RetractionLimitLightTextBox.Size = new System.Drawing.Size(35, 17);
         this.RetractionLimitLightTextBox.TabIndex = 86;
         this.RetractionLimitLightTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // OriginSetLightTextBox
         // 
         this.OriginSetLightTextBox.BackColor = System.Drawing.Color.Black;
         this.OriginSetLightTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.OriginSetLightTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.OriginSetLightTextBox.ForeColor = System.Drawing.SystemColors.Window;
         this.OriginSetLightTextBox.Location = new System.Drawing.Point(246, 113);
         this.OriginSetLightTextBox.Name = "OriginSetLightTextBox";
         this.OriginSetLightTextBox.ReadOnly = true;
         this.OriginSetLightTextBox.Size = new System.Drawing.Size(35, 17);
         this.OriginSetLightTextBox.TabIndex = 85;
         this.OriginSetLightTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // DrillErrorLightTextBox
         // 
         this.DrillErrorLightTextBox.BackColor = System.Drawing.Color.Black;
         this.DrillErrorLightTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.DrillErrorLightTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DrillErrorLightTextBox.ForeColor = System.Drawing.SystemColors.Window;
         this.DrillErrorLightTextBox.Location = new System.Drawing.Point(246, 184);
         this.DrillErrorLightTextBox.Name = "DrillErrorLightTextBox";
         this.DrillErrorLightTextBox.ReadOnly = true;
         this.DrillErrorLightTextBox.Size = new System.Drawing.Size(35, 17);
         this.DrillErrorLightTextBox.TabIndex = 83;
         this.DrillErrorLightTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label48
         // 
         this.label48.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label48.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.label48.Location = new System.Drawing.Point(285, 178);
         this.label48.Name = "label48";
         this.label48.Size = new System.Drawing.Size(72, 29);
         this.label48.TabIndex = 101;
         this.label48.Text = "ERROR";
         this.label48.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // label47
         // 
         this.label47.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label47.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.label47.Location = new System.Drawing.Point(286, 143);
         this.label47.Name = "label47";
         this.label47.Size = new System.Drawing.Size(81, 29);
         this.label47.TabIndex = 99;
         this.label47.Text = "RETRACTION LIMIT";
         this.label47.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // label46
         // 
         this.label46.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label46.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.label46.Location = new System.Drawing.Point(287, 108);
         this.label46.Name = "label46";
         this.label46.Size = new System.Drawing.Size(66, 29);
         this.label46.TabIndex = 97;
         this.label46.Text = "ORIGIN REQUIRED";
         this.label46.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // rotatableLabel1
         // 
         this.rotatableLabel1.Angle = 90;
         this.rotatableLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.rotatableLabel1.Location = new System.Drawing.Point(-10, 105);
         this.rotatableLabel1.Name = "rotatableLabel1";
         this.rotatableLabel1.Size = new System.Drawing.Size(47, 60);
         this.rotatableLabel1.TabIndex = 146;
         this.rotatableLabel1.Text = "ACTUAL";
         this.rotatableLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // rotatableLabel2
         // 
         this.rotatableLabel2.Angle = 90;
         this.rotatableLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.rotatableLabel2.Location = new System.Drawing.Point(-10, 156);
         this.rotatableLabel2.Name = "rotatableLabel2";
         this.rotatableLabel2.Size = new System.Drawing.Size(47, 60);
         this.rotatableLabel2.TabIndex = 147;
         this.rotatableLabel2.Text = "SET";
         this.rotatableLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // rotatableLabel5
         // 
         this.rotatableLabel5.Angle = 90;
         this.rotatableLabel5.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.rotatableLabel5.Location = new System.Drawing.Point(105, 105);
         this.rotatableLabel5.Name = "rotatableLabel5";
         this.rotatableLabel5.Size = new System.Drawing.Size(47, 60);
         this.rotatableLabel5.TabIndex = 164;
         this.rotatableLabel5.Text = "ACTUAL";
         this.rotatableLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // rotatableLabel6
         // 
         this.rotatableLabel6.Angle = 90;
         this.rotatableLabel6.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.rotatableLabel6.Location = new System.Drawing.Point(105, 156);
         this.rotatableLabel6.Name = "rotatableLabel6";
         this.rotatableLabel6.Size = new System.Drawing.Size(47, 60);
         this.rotatableLabel6.TabIndex = 165;
         this.rotatableLabel6.Text = "SET";
         this.rotatableLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label45
         // 
         this.label45.AutoSize = true;
         this.label45.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label45.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.label45.Location = new System.Drawing.Point(4, 281);
         this.label45.Name = "label45";
         this.label45.Size = new System.Drawing.Size(114, 16);
         this.label45.TabIndex = 72;
         this.label45.Text = "INDEX POSITION";
         // 
         // MovementMainPanel
         // 
         this.MovementMainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(80)))), ((int)(((byte)(96)))));
         this.MovementMainPanel.Controls.Add(this.MovementLockButton);
         this.MovementMainPanel.Controls.Add(this.MovementOffButton);
         this.MovementMainPanel.Controls.Add(this.MovementManaulDisplayButton);
         this.MovementMainPanel.Controls.Add(this.MovementCornerModeToggleButton);
         this.MovementMainPanel.Controls.Add(this.MovementLaunchModeToggleButton);
         this.MovementMainPanel.Controls.Add(this.MovementMoveButton);
         this.MovementMainPanel.Controls.Add(this.MotorStatusDirectionalValuePanel);
         this.MovementMainPanel.Controls.Add(this.MotorTitleLabel);
         this.MovementMainPanel.Controls.Add(this.MovementSetupButton);
         this.MovementMainPanel.Controls.Add(this.MovementSpeedToggleButton);
         this.MovementMainPanel.Controls.Add(this.MovementAxialToggleButton);
         this.MovementMainPanel.Location = new System.Drawing.Point(671, 46);
         this.MovementMainPanel.Name = "MovementMainPanel";
         this.MovementMainPanel.Size = new System.Drawing.Size(386, 383);
         this.MovementMainPanel.TabIndex = 36;
         // 
         // MovementLockButton
         // 
         this.MovementLockButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.MovementLockButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.MovementLockButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.MovementLockButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.MovementLockButton.ForeColor = System.Drawing.Color.Black;
         this.MovementLockButton.HoldTimeoutEnable = true;
         this.MovementLockButton.HoldTimeoutInterval = 100;
         this.MovementLockButton.Location = new System.Drawing.Point(262, 89);
         this.MovementLockButton.Name = "MovementLockButton";
         this.MovementLockButton.Size = new System.Drawing.Size(107, 90);
         this.MovementLockButton.TabIndex = 164;
         this.MovementLockButton.Text = "LOCK";
         this.MovementLockButton.UseVisualStyleBackColor = false;
         this.MovementLockButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.MovementLockButton_HoldTimeout);
         // 
         // MovementOffButton
         // 
         this.MovementOffButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.MovementOffButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.MovementOffButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.MovementOffButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.MovementOffButton.ForeColor = System.Drawing.Color.Black;
         this.MovementOffButton.HoldTimeoutEnable = true;
         this.MovementOffButton.HoldTimeoutInterval = 100;
         this.MovementOffButton.Location = new System.Drawing.Point(16, 89);
         this.MovementOffButton.Name = "MovementOffButton";
         this.MovementOffButton.Size = new System.Drawing.Size(107, 90);
         this.MovementOffButton.TabIndex = 163;
         this.MovementOffButton.Text = "OFF   (FREE)";
         this.MovementOffButton.UseVisualStyleBackColor = false;
         this.MovementOffButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.MovementOffButton_HoldTimeout);
         // 
         // MovementManaulDisplayButton
         // 
         this.MovementManaulDisplayButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.MovementManaulDisplayButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.MovementManaulDisplayButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.MovementManaulDisplayButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.MovementManaulDisplayButton.Location = new System.Drawing.Point(262, 285);
         this.MovementManaulDisplayButton.Name = "MovementManaulDisplayButton";
         this.MovementManaulDisplayButton.Size = new System.Drawing.Size(107, 90);
         this.MovementManaulDisplayButton.TabIndex = 162;
         this.MovementManaulDisplayButton.Text = "SHOW MANUAL";
         this.MovementManaulDisplayButton.UseVisualStyleBackColor = false;
         this.MovementManaulDisplayButton.Click += new System.EventHandler(this.MovementManaulDisplayButton_Click);
         // 
         // MovementCornerModeToggleButton
         // 
         this.MovementCornerModeToggleButton.AutomaticToggle = true;
         this.MovementCornerModeToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.MovementCornerModeToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.MovementCornerModeToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.MovementCornerModeToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.MovementCornerModeToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.MovementCornerModeToggleButton.HoldEnable = true;
         this.MovementCornerModeToggleButton.HoldTimeoutInterval = 100;
         this.MovementCornerModeToggleButton.Location = new System.Drawing.Point(262, 187);
         this.MovementCornerModeToggleButton.Name = "MovementCornerModeToggleButton";
         this.MovementCornerModeToggleButton.OptionASelected = true;
         this.MovementCornerModeToggleButton.OptionAText = "ON";
         this.MovementCornerModeToggleButton.OptionBSelected = false;
         this.MovementCornerModeToggleButton.OptionBText = "OFF";
         this.MovementCornerModeToggleButton.OptionCenterWidth = 2;
         this.MovementCornerModeToggleButton.OptionEdgeHeight = 8;
         this.MovementCornerModeToggleButton.OptionHeight = 22;
         this.MovementCornerModeToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.MovementCornerModeToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
         this.MovementCornerModeToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.MovementCornerModeToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.MovementCornerModeToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.MovementCornerModeToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.MovementCornerModeToggleButton.OptionWidth = 45;
         this.MovementCornerModeToggleButton.Size = new System.Drawing.Size(107, 90);
         this.MovementCornerModeToggleButton.TabIndex = 161;
         this.MovementCornerModeToggleButton.Text = "CORNER MODE";
         this.MovementCornerModeToggleButton.UseVisualStyleBackColor = false;
         this.MovementCornerModeToggleButton.HoldTimeout += new NICBOT.GUI.ValueToggleButton.HoldTimeoutHandler(this.MovementCornerModeToggleButton_HoldTimeout);
         // 
         // MovementLaunchModeToggleButton
         // 
         this.MovementLaunchModeToggleButton.AutomaticToggle = true;
         this.MovementLaunchModeToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.MovementLaunchModeToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.MovementLaunchModeToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.MovementLaunchModeToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.MovementLaunchModeToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.MovementLaunchModeToggleButton.HoldEnable = true;
         this.MovementLaunchModeToggleButton.HoldTimeoutInterval = 100;
         this.MovementLaunchModeToggleButton.Location = new System.Drawing.Point(139, 187);
         this.MovementLaunchModeToggleButton.Name = "MovementLaunchModeToggleButton";
         this.MovementLaunchModeToggleButton.OptionASelected = true;
         this.MovementLaunchModeToggleButton.OptionAText = "ON";
         this.MovementLaunchModeToggleButton.OptionBSelected = false;
         this.MovementLaunchModeToggleButton.OptionBText = "OFF";
         this.MovementLaunchModeToggleButton.OptionCenterWidth = 2;
         this.MovementLaunchModeToggleButton.OptionEdgeHeight = 8;
         this.MovementLaunchModeToggleButton.OptionHeight = 22;
         this.MovementLaunchModeToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.MovementLaunchModeToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
         this.MovementLaunchModeToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.MovementLaunchModeToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.MovementLaunchModeToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.MovementLaunchModeToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.MovementLaunchModeToggleButton.OptionWidth = 45;
         this.MovementLaunchModeToggleButton.Size = new System.Drawing.Size(107, 90);
         this.MovementLaunchModeToggleButton.TabIndex = 160;
         this.MovementLaunchModeToggleButton.Text = "LAUNCH MODE";
         this.MovementLaunchModeToggleButton.UseVisualStyleBackColor = false;
         this.MovementLaunchModeToggleButton.HoldTimeout += new NICBOT.GUI.ValueToggleButton.HoldTimeoutHandler(this.MovementLaunchModeToggleButton_HoldTimeout);
         // 
         // MovementMoveButton
         // 
         this.MovementMoveButton.ArrowWidth = 12;
         this.MovementMoveButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.MovementMoveButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.MovementMoveButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.MovementMoveButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.MovementMoveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.MovementMoveButton.HoldTimeoutInterval = 100;
         this.MovementMoveButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.MovementMoveButton.LeftArrowVisible = true;
         this.MovementMoveButton.Location = new System.Drawing.Point(139, 89);
         this.MovementMoveButton.Name = "MovementMoveButton";
         this.MovementMoveButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.MovementMoveButton.RightArrowVisible = true;
         this.MovementMoveButton.Size = new System.Drawing.Size(107, 90);
         this.MovementMoveButton.TabIndex = 140;
         this.MovementMoveButton.Text = "MOVE";
         this.MovementMoveButton.UseVisualStyleBackColor = false;
         this.MovementMoveButton.ValueBackColor = System.Drawing.Color.Black;
         this.MovementMoveButton.ValueEdgeHeight = 8;
         this.MovementMoveButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.MovementMoveButton.ValueForeColor = System.Drawing.Color.White;
         this.MovementMoveButton.ValueHeight = 22;
         this.MovementMoveButton.ValueText = "19.04 m/MIN";
         this.MovementMoveButton.ValueWidth = 80;
         this.MovementMoveButton.HoldTimeout += new NICBOT.GUI.ValueButton.HoldTimeoutHandler(this.MovementMoveButton_HoldTimeout);
         // 
         // MotorStatusDirectionalValuePanel
         // 
         this.MotorStatusDirectionalValuePanel.ActiveBackColor = System.Drawing.Color.Black;
         this.MotorStatusDirectionalValuePanel.ActiveFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.MotorStatusDirectionalValuePanel.ActiveForeColor = System.Drawing.Color.White;
         this.MotorStatusDirectionalValuePanel.ArrowWidth = 60;
         this.MotorStatusDirectionalValuePanel.Direction = NICBOT.GUI.DirectionalValuePanel.Directions.Idle;
         this.MotorStatusDirectionalValuePanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.MotorStatusDirectionalValuePanel.ForeColor = System.Drawing.Color.Black;
         this.MotorStatusDirectionalValuePanel.IdleBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(80)))), ((int)(((byte)(96)))));
         this.MotorStatusDirectionalValuePanel.IdleFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
         this.MotorStatusDirectionalValuePanel.IdleForeColor = System.Drawing.Color.White;
         this.MotorStatusDirectionalValuePanel.LeftArrowText = "REV";
         this.MotorStatusDirectionalValuePanel.Location = new System.Drawing.Point(84, 30);
         this.MotorStatusDirectionalValuePanel.Name = "MotorStatusDirectionalValuePanel";
         this.MotorStatusDirectionalValuePanel.RightArrowText = "FWD";
         this.MotorStatusDirectionalValuePanel.Size = new System.Drawing.Size(219, 42);
         this.MotorStatusDirectionalValuePanel.TabIndex = 140;
         this.MotorStatusDirectionalValuePanel.Text = "directionalValuePanel1";
         this.MotorStatusDirectionalValuePanel.ValueBackColor = System.Drawing.Color.Black;
         this.MotorStatusDirectionalValuePanel.ValueForeColor = System.Drawing.Color.White;
         this.MotorStatusDirectionalValuePanel.ValueText = "19.04 m/MIN";
         this.MotorStatusDirectionalValuePanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // MotorTitleLabel
         // 
         this.MotorTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.MotorTitleLabel.Location = new System.Drawing.Point(4, 4);
         this.MotorTitleLabel.Name = "MotorTitleLabel";
         this.MotorTitleLabel.Size = new System.Drawing.Size(379, 23);
         this.MotorTitleLabel.TabIndex = 139;
         this.MotorTitleLabel.Text = "WHEEL MOTORS - CIRCUMFERENTIAL MOTION";
         this.MotorTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // MovementSetupButton
         // 
         this.MovementSetupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.MovementSetupButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.MovementSetupButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.MovementSetupButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.MovementSetupButton.ForeColor = System.Drawing.Color.Black;
         this.MovementSetupButton.HoldTimeoutEnable = true;
         this.MovementSetupButton.HoldTimeoutInterval = 100;
         this.MovementSetupButton.Location = new System.Drawing.Point(16, 285);
         this.MovementSetupButton.Name = "MovementSetupButton";
         this.MovementSetupButton.Size = new System.Drawing.Size(107, 90);
         this.MovementSetupButton.TabIndex = 125;
         this.MovementSetupButton.Text = "MOTOR SETUP";
         this.MovementSetupButton.UseVisualStyleBackColor = false;
         this.MovementSetupButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.MovementSetupButton_HoldTimeout);
         // 
         // MovementSpeedToggleButton
         // 
         this.MovementSpeedToggleButton.AutomaticToggle = true;
         this.MovementSpeedToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.MovementSpeedToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.MovementSpeedToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.MovementSpeedToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.MovementSpeedToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.MovementSpeedToggleButton.HoldEnable = false;
         this.MovementSpeedToggleButton.HoldTimeoutInterval = 0;
         this.MovementSpeedToggleButton.Location = new System.Drawing.Point(139, 285);
         this.MovementSpeedToggleButton.Name = "MovementSpeedToggleButton";
         this.MovementSpeedToggleButton.OptionASelected = true;
         this.MovementSpeedToggleButton.OptionAText = "FAST";
         this.MovementSpeedToggleButton.OptionBSelected = false;
         this.MovementSpeedToggleButton.OptionBText = "SLOW";
         this.MovementSpeedToggleButton.OptionCenterWidth = 2;
         this.MovementSpeedToggleButton.OptionEdgeHeight = 8;
         this.MovementSpeedToggleButton.OptionHeight = 22;
         this.MovementSpeedToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.MovementSpeedToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
         this.MovementSpeedToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.MovementSpeedToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.MovementSpeedToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.MovementSpeedToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.MovementSpeedToggleButton.OptionWidth = 45;
         this.MovementSpeedToggleButton.Size = new System.Drawing.Size(107, 90);
         this.MovementSpeedToggleButton.TabIndex = 123;
         this.MovementSpeedToggleButton.Text = "SPEED";
         this.MovementSpeedToggleButton.UseVisualStyleBackColor = false;
         this.MovementSpeedToggleButton.Click += new System.EventHandler(this.MovementSpeedToggleButton_Click);
         // 
         // MovementAxialToggleButton
         // 
         this.MovementAxialToggleButton.AutomaticToggle = false;
         this.MovementAxialToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.MovementAxialToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.MovementAxialToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.MovementAxialToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.MovementAxialToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.MovementAxialToggleButton.HoldEnable = true;
         this.MovementAxialToggleButton.HoldTimeoutInterval = 100;
         this.MovementAxialToggleButton.Location = new System.Drawing.Point(16, 187);
         this.MovementAxialToggleButton.Name = "MovementAxialToggleButton";
         this.MovementAxialToggleButton.OptionASelected = true;
         this.MovementAxialToggleButton.OptionAText = "AXIAL";
         this.MovementAxialToggleButton.OptionBSelected = false;
         this.MovementAxialToggleButton.OptionBText = "CIRC";
         this.MovementAxialToggleButton.OptionCenterWidth = 2;
         this.MovementAxialToggleButton.OptionEdgeHeight = 8;
         this.MovementAxialToggleButton.OptionHeight = 22;
         this.MovementAxialToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.MovementAxialToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
         this.MovementAxialToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.MovementAxialToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.MovementAxialToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.MovementAxialToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.MovementAxialToggleButton.OptionWidth = 45;
         this.MovementAxialToggleButton.Size = new System.Drawing.Size(107, 90);
         this.MovementAxialToggleButton.TabIndex = 119;
         this.MovementAxialToggleButton.Text = "MOTION";
         this.MovementAxialToggleButton.UseVisualStyleBackColor = false;
         this.MovementAxialToggleButton.HoldTimeout += new NICBOT.GUI.ValueToggleButton.HoldTimeoutHandler(this.MovementAxialToggleButton_HoldTimeout);
         // 
         // SealantMainPanel
         // 
         this.SealantMainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.SealantMainPanel.Controls.Add(this.borderedPanel11);
         this.SealantMainPanel.Controls.Add(this.SealantManulDisplayButton);
         this.SealantMainPanel.Controls.Add(this.SealantLaserLightButton);
         this.SealantMainPanel.Controls.Add(this.SealantReserviorTextPanel);
         this.SealantMainPanel.Controls.Add(this.label29);
         this.SealantMainPanel.Controls.Add(this.SealantNozzlePositionTextPanel);
         this.SealantMainPanel.Controls.Add(this.SealantFlowRateTextPanel);
         this.SealantMainPanel.Controls.Add(this.label30);
         this.SealantMainPanel.Controls.Add(this.label25);
         this.SealantMainPanel.Controls.Add(this.SealantActualSpeedValuePanel);
         this.SealantMainPanel.Controls.Add(this.label23);
         this.SealantMainPanel.Controls.Add(this.SealantSpeedSetPointValuePanel);
         this.SealantMainPanel.Controls.Add(this.SealantActualPressureValuePanel);
         this.SealantMainPanel.Controls.Add(this.SealantPressureSetPointValuePanel);
         this.SealantMainPanel.Controls.Add(this.SealantActualVolumeValuePanel);
         this.SealantMainPanel.Controls.Add(this.label18);
         this.SealantMainPanel.Controls.Add(this.label19);
         this.SealantMainPanel.Controls.Add(this.SealantVolumeSetPointValuePanel);
         this.SealantMainPanel.Controls.Add(this.label10);
         this.SealantMainPanel.Controls.Add(this.SealantModeLabel);
         this.SealantMainPanel.Controls.Add(this.label12);
         this.SealantMainPanel.Controls.Add(this.SealantPipePositionLabel);
         this.SealantMainPanel.Controls.Add(this.SealDrillModeButton);
         this.SealantMainPanel.Controls.Add(this.SealantSetupButton);
         this.SealantMainPanel.Controls.Add(this.NozzleSelectionLabel);
         this.SealantMainPanel.Controls.Add(this.rotatableLabel3);
         this.SealantMainPanel.Controls.Add(this.rotatableLabel4);
         this.SealantMainPanel.Controls.Add(this.rotatableLabel7);
         this.SealantMainPanel.Controls.Add(this.rotatableLabel8);
         this.SealantMainPanel.Controls.Add(this.rotatableLabel9);
         this.SealantMainPanel.Controls.Add(this.rotatableLabel10);
         this.SealantMainPanel.Location = new System.Drawing.Point(2349, 46);
         this.SealantMainPanel.Name = "SealantMainPanel";
         this.SealantMainPanel.Size = new System.Drawing.Size(369, 528);
         this.SealantMainPanel.TabIndex = 38;
         // 
         // borderedPanel11
         // 
         this.borderedPanel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
         this.borderedPanel11.Controls.Add(this.SealantAutoStopButton);
         this.borderedPanel11.Controls.Add(this.SealantAutoStartButton);
         this.borderedPanel11.Controls.Add(this.SealantAutoPauseResumeButton);
         this.borderedPanel11.EdgeWeight = 1;
         this.borderedPanel11.Location = new System.Drawing.Point(8, 414);
         this.borderedPanel11.Name = "borderedPanel11";
         this.borderedPanel11.Size = new System.Drawing.Size(353, 108);
         this.borderedPanel11.TabIndex = 172;
         // 
         // SealantAutoStopButton
         // 
         this.SealantAutoStopButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SealantAutoStopButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.SealantAutoStopButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.SealantAutoStopButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.SealantAutoStopButton.Location = new System.Drawing.Point(238, 8);
         this.SealantAutoStopButton.Name = "SealantAutoStopButton";
         this.SealantAutoStopButton.Size = new System.Drawing.Size(107, 90);
         this.SealantAutoStopButton.TabIndex = 169;
         this.SealantAutoStopButton.Text = "STOP     AUTO     FILL";
         this.SealantAutoStopButton.UseVisualStyleBackColor = false;
         this.SealantAutoStopButton.Click += new System.EventHandler(this.SealantAutoStopButton_Click);
         // 
         // SealantAutoStartButton
         // 
         this.SealantAutoStartButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SealantAutoStartButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.SealantAutoStartButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.SealantAutoStartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.SealantAutoStartButton.HoldTimeoutEnable = true;
         this.SealantAutoStartButton.HoldTimeoutInterval = 100;
         this.SealantAutoStartButton.Location = new System.Drawing.Point(8, 8);
         this.SealantAutoStartButton.Name = "SealantAutoStartButton";
         this.SealantAutoStartButton.Size = new System.Drawing.Size(107, 90);
         this.SealantAutoStartButton.TabIndex = 63;
         this.SealantAutoStartButton.Text = "START     AUTO     FILL";
         this.SealantAutoStartButton.UseVisualStyleBackColor = false;
         this.SealantAutoStartButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.SealantAutoStartButton_HoldTimeout);
         // 
         // SealantAutoPauseResumeButton
         // 
         this.SealantAutoPauseResumeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SealantAutoPauseResumeButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.SealantAutoPauseResumeButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.SealantAutoPauseResumeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.SealantAutoPauseResumeButton.Location = new System.Drawing.Point(123, 8);
         this.SealantAutoPauseResumeButton.Name = "SealantAutoPauseResumeButton";
         this.SealantAutoPauseResumeButton.Size = new System.Drawing.Size(107, 90);
         this.SealantAutoPauseResumeButton.TabIndex = 168;
         this.SealantAutoPauseResumeButton.Text = "PAUSE";
         this.SealantAutoPauseResumeButton.UseVisualStyleBackColor = false;
         this.SealantAutoPauseResumeButton.Click += new System.EventHandler(this.SealantAutoPauseResumeButton_Click);
         // 
         // SealantManulDisplayButton
         // 
         this.SealantManulDisplayButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SealantManulDisplayButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.SealantManulDisplayButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.SealantManulDisplayButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.SealantManulDisplayButton.Location = new System.Drawing.Point(246, 314);
         this.SealantManulDisplayButton.Name = "SealantManulDisplayButton";
         this.SealantManulDisplayButton.Size = new System.Drawing.Size(107, 90);
         this.SealantManulDisplayButton.TabIndex = 167;
         this.SealantManulDisplayButton.Text = "SHOW MANUAL";
         this.SealantManulDisplayButton.UseVisualStyleBackColor = false;
         this.SealantManulDisplayButton.Click += new System.EventHandler(this.SealantManulDisplayButton_Click);
         // 
         // SealantLaserLightButton
         // 
         this.SealantLaserLightButton.AutomaticToggle = true;
         this.SealantLaserLightButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SealantLaserLightButton.DisabledBackColor = System.Drawing.Color.Black;
         this.SealantLaserLightButton.DisabledForeColor = System.Drawing.Color.Gray;
         this.SealantLaserLightButton.DisabledOptionBackColor = System.Drawing.Color.Black;
         this.SealantLaserLightButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SealantLaserLightButton.HoldEnable = false;
         this.SealantLaserLightButton.HoldTimeoutInterval = 100;
         this.SealantLaserLightButton.Location = new System.Drawing.Point(16, 314);
         this.SealantLaserLightButton.Name = "SealantLaserLightButton";
         this.SealantLaserLightButton.OptionASelected = true;
         this.SealantLaserLightButton.OptionAText = "ON";
         this.SealantLaserLightButton.OptionBSelected = false;
         this.SealantLaserLightButton.OptionBText = "OFF";
         this.SealantLaserLightButton.OptionCenterWidth = 2;
         this.SealantLaserLightButton.OptionEdgeHeight = 8;
         this.SealantLaserLightButton.OptionHeight = 22;
         this.SealantLaserLightButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.SealantLaserLightButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
         this.SealantLaserLightButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.SealantLaserLightButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.SealantLaserLightButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SealantLaserLightButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.SealantLaserLightButton.OptionWidth = 45;
         this.SealantLaserLightButton.Size = new System.Drawing.Size(107, 90);
         this.SealantLaserLightButton.TabIndex = 164;
         this.SealantLaserLightButton.Text = "LASER SIGHT";
         this.SealantLaserLightButton.UseVisualStyleBackColor = false;
         this.SealantLaserLightButton.Click += new System.EventHandler(this.SealantLaserLightButton_Click);
         // 
         // SealantReserviorTextPanel
         // 
         this.SealantReserviorTextPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.SealantReserviorTextPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.SealantReserviorTextPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SealantReserviorTextPanel.ForeColor = System.Drawing.Color.Silver;
         this.SealantReserviorTextPanel.HoldTimeoutEnable = false;
         this.SealantReserviorTextPanel.HoldTimeoutInterval = 100;
         this.SealantReserviorTextPanel.Location = new System.Drawing.Point(16, 130);
         this.SealantReserviorTextPanel.Name = "SealantReserviorTextPanel";
         this.SealantReserviorTextPanel.Size = new System.Drawing.Size(99, 42);
         this.SealantReserviorTextPanel.TabIndex = 163;
         this.SealantReserviorTextPanel.ValueText = "##### mL";
         this.SealantReserviorTextPanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label29
         // 
         this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label29.Location = new System.Drawing.Point(5, 106);
         this.label29.Name = "label29";
         this.label29.Size = new System.Drawing.Size(120, 20);
         this.label29.TabIndex = 162;
         this.label29.Text = "RESERVIOR";
         this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // SealantNozzlePositionTextPanel
         // 
         this.SealantNozzlePositionTextPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.SealantNozzlePositionTextPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.SealantNozzlePositionTextPanel.Enabled = false;
         this.SealantNozzlePositionTextPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SealantNozzlePositionTextPanel.ForeColor = System.Drawing.Color.Silver;
         this.SealantNozzlePositionTextPanel.HoldTimeoutEnable = false;
         this.SealantNozzlePositionTextPanel.HoldTimeoutInterval = 100;
         this.SealantNozzlePositionTextPanel.Location = new System.Drawing.Point(250, 130);
         this.SealantNozzlePositionTextPanel.Name = "SealantNozzlePositionTextPanel";
         this.SealantNozzlePositionTextPanel.Size = new System.Drawing.Size(99, 42);
         this.SealantNozzlePositionTextPanel.TabIndex = 161;
         this.SealantNozzlePositionTextPanel.ValueText = "RETRACTED";
         this.SealantNozzlePositionTextPanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // SealantFlowRateTextPanel
         // 
         this.SealantFlowRateTextPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.SealantFlowRateTextPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.SealantFlowRateTextPanel.Enabled = false;
         this.SealantFlowRateTextPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SealantFlowRateTextPanel.ForeColor = System.Drawing.Color.Silver;
         this.SealantFlowRateTextPanel.HoldTimeoutEnable = false;
         this.SealantFlowRateTextPanel.HoldTimeoutInterval = 100;
         this.SealantFlowRateTextPanel.Location = new System.Drawing.Point(135, 130);
         this.SealantFlowRateTextPanel.Name = "SealantFlowRateTextPanel";
         this.SealantFlowRateTextPanel.Size = new System.Drawing.Size(99, 42);
         this.SealantFlowRateTextPanel.TabIndex = 160;
         this.SealantFlowRateTextPanel.ValueText = "### mL/M";
         this.SealantFlowRateTextPanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label30
         // 
         this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label30.Location = new System.Drawing.Point(131, 106);
         this.label30.Name = "label30";
         this.label30.Size = new System.Drawing.Size(107, 20);
         this.label30.TabIndex = 159;
         this.label30.Text = "FLOW RATE";
         this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label25
         // 
         this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label25.Location = new System.Drawing.Point(238, 106);
         this.label25.Name = "label25";
         this.label25.Size = new System.Drawing.Size(122, 20);
         this.label25.TabIndex = 158;
         this.label25.Text = "NOZZLE";
         this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // SealantActualSpeedValuePanel
         // 
         this.SealantActualSpeedValuePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.SealantActualSpeedValuePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.SealantActualSpeedValuePanel.Enabled = false;
         this.SealantActualSpeedValuePanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SealantActualSpeedValuePanel.ForeColor = System.Drawing.Color.Silver;
         this.SealantActualSpeedValuePanel.HoldTimeoutEnable = false;
         this.SealantActualSpeedValuePanel.HoldTimeoutInterval = 100;
         this.SealantActualSpeedValuePanel.Location = new System.Drawing.Point(250, 215);
         this.SealantActualSpeedValuePanel.Name = "SealantActualSpeedValuePanel";
         this.SealantActualSpeedValuePanel.Size = new System.Drawing.Size(99, 42);
         this.SealantActualSpeedValuePanel.TabIndex = 156;
         this.SealantActualSpeedValuePanel.ValueText = "### RPM";
         this.SealantActualSpeedValuePanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label23
         // 
         this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label23.Location = new System.Drawing.Point(241, 191);
         this.label23.Name = "label23";
         this.label23.Size = new System.Drawing.Size(107, 20);
         this.label23.TabIndex = 155;
         this.label23.Text = "SPEED";
         this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // SealantSpeedSetPointValuePanel
         // 
         this.SealantSpeedSetPointValuePanel.BackColor = System.Drawing.Color.Black;
         this.SealantSpeedSetPointValuePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.SealantSpeedSetPointValuePanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SealantSpeedSetPointValuePanel.ForeColor = System.Drawing.Color.White;
         this.SealantSpeedSetPointValuePanel.HoldTimeoutEnable = true;
         this.SealantSpeedSetPointValuePanel.HoldTimeoutInterval = 100;
         this.SealantSpeedSetPointValuePanel.Location = new System.Drawing.Point(250, 264);
         this.SealantSpeedSetPointValuePanel.Name = "SealantSpeedSetPointValuePanel";
         this.SealantSpeedSetPointValuePanel.Size = new System.Drawing.Size(99, 42);
         this.SealantSpeedSetPointValuePanel.TabIndex = 154;
         this.SealantSpeedSetPointValuePanel.ValueText = "### RPM";
         this.SealantSpeedSetPointValuePanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.SealantSpeedSetPointValuePanel.HoldTimeout += new NICBOT.GUI.TextPanel.HoldTimeoutHandler2(this.SealantSpeedSetPointValuePanel_HoldTimeout);
         // 
         // SealantActualPressureValuePanel
         // 
         this.SealantActualPressureValuePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.SealantActualPressureValuePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.SealantActualPressureValuePanel.Enabled = false;
         this.SealantActualPressureValuePanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SealantActualPressureValuePanel.ForeColor = System.Drawing.Color.Silver;
         this.SealantActualPressureValuePanel.HoldTimeoutEnable = false;
         this.SealantActualPressureValuePanel.HoldTimeoutInterval = 100;
         this.SealantActualPressureValuePanel.Location = new System.Drawing.Point(20, 215);
         this.SealantActualPressureValuePanel.Name = "SealantActualPressureValuePanel";
         this.SealantActualPressureValuePanel.Size = new System.Drawing.Size(99, 42);
         this.SealantActualPressureValuePanel.TabIndex = 152;
         this.SealantActualPressureValuePanel.ValueText = "### PSI";
         this.SealantActualPressureValuePanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // SealantPressureSetPointValuePanel
         // 
         this.SealantPressureSetPointValuePanel.BackColor = System.Drawing.Color.Black;
         this.SealantPressureSetPointValuePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.SealantPressureSetPointValuePanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SealantPressureSetPointValuePanel.ForeColor = System.Drawing.Color.White;
         this.SealantPressureSetPointValuePanel.HoldTimeoutEnable = true;
         this.SealantPressureSetPointValuePanel.HoldTimeoutInterval = 100;
         this.SealantPressureSetPointValuePanel.Location = new System.Drawing.Point(20, 264);
         this.SealantPressureSetPointValuePanel.Name = "SealantPressureSetPointValuePanel";
         this.SealantPressureSetPointValuePanel.Size = new System.Drawing.Size(99, 42);
         this.SealantPressureSetPointValuePanel.TabIndex = 148;
         this.SealantPressureSetPointValuePanel.ValueText = "### PSI";
         this.SealantPressureSetPointValuePanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.SealantPressureSetPointValuePanel.HoldTimeout += new NICBOT.GUI.TextPanel.HoldTimeoutHandler2(this.SealantPressureSetPointValuePanel_HoldTimeout);
         // 
         // SealantActualVolumeValuePanel
         // 
         this.SealantActualVolumeValuePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.SealantActualVolumeValuePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.SealantActualVolumeValuePanel.Enabled = false;
         this.SealantActualVolumeValuePanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SealantActualVolumeValuePanel.ForeColor = System.Drawing.Color.Silver;
         this.SealantActualVolumeValuePanel.HoldTimeoutEnable = false;
         this.SealantActualVolumeValuePanel.HoldTimeoutInterval = 100;
         this.SealantActualVolumeValuePanel.Location = new System.Drawing.Point(135, 215);
         this.SealantActualVolumeValuePanel.Name = "SealantActualVolumeValuePanel";
         this.SealantActualVolumeValuePanel.Size = new System.Drawing.Size(99, 42);
         this.SealantActualVolumeValuePanel.TabIndex = 153;
         this.SealantActualVolumeValuePanel.ValueText = "#### mL";
         this.SealantActualVolumeValuePanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label18
         // 
         this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label18.Location = new System.Drawing.Point(5, 191);
         this.label18.Name = "label18";
         this.label18.Size = new System.Drawing.Size(120, 20);
         this.label18.TabIndex = 151;
         this.label18.Text = "PRESSURE";
         this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label19
         // 
         this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label19.Location = new System.Drawing.Point(131, 191);
         this.label19.Name = "label19";
         this.label19.Size = new System.Drawing.Size(107, 20);
         this.label19.TabIndex = 150;
         this.label19.Text = "VOLUME";
         this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // SealantVolumeSetPointValuePanel
         // 
         this.SealantVolumeSetPointValuePanel.BackColor = System.Drawing.Color.Black;
         this.SealantVolumeSetPointValuePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.SealantVolumeSetPointValuePanel.Enabled = false;
         this.SealantVolumeSetPointValuePanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SealantVolumeSetPointValuePanel.ForeColor = System.Drawing.Color.White;
         this.SealantVolumeSetPointValuePanel.HoldTimeoutEnable = false;
         this.SealantVolumeSetPointValuePanel.HoldTimeoutInterval = 100;
         this.SealantVolumeSetPointValuePanel.Location = new System.Drawing.Point(135, 264);
         this.SealantVolumeSetPointValuePanel.Name = "SealantVolumeSetPointValuePanel";
         this.SealantVolumeSetPointValuePanel.Size = new System.Drawing.Size(99, 42);
         this.SealantVolumeSetPointValuePanel.TabIndex = 149;
         this.SealantVolumeSetPointValuePanel.ValueText = "#### mL";
         this.SealantVolumeSetPointValuePanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label10
         // 
         this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label10.Location = new System.Drawing.Point(123, 30);
         this.label10.Name = "label10";
         this.label10.Size = new System.Drawing.Size(122, 20);
         this.label10.TabIndex = 147;
         this.label10.Text = "AUTO FILL MODE";
         this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // SealantModeLabel
         // 
         this.SealantModeLabel.BackColor = System.Drawing.SystemColors.WindowFrame;
         this.SealantModeLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.SealantModeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SealantModeLabel.ForeColor = System.Drawing.SystemColors.Window;
         this.SealantModeLabel.Location = new System.Drawing.Point(131, 51);
         this.SealantModeLabel.Name = "SealantModeLabel";
         this.SealantModeLabel.Size = new System.Drawing.Size(107, 36);
         this.SealantModeLabel.TabIndex = 146;
         this.SealantModeLabel.Text = "VOLUME";
         this.SealantModeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label12
         // 
         this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label12.Location = new System.Drawing.Point(15, 30);
         this.label12.Name = "label12";
         this.label12.Size = new System.Drawing.Size(109, 20);
         this.label12.TabIndex = 145;
         this.label12.Text = "PIPE POSITION";
         this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // SealantPipePositionLabel
         // 
         this.SealantPipePositionLabel.BackColor = System.Drawing.SystemColors.WindowFrame;
         this.SealantPipePositionLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.SealantPipePositionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.SealantPipePositionLabel.ForeColor = System.Drawing.SystemColors.Window;
         this.SealantPipePositionLabel.Location = new System.Drawing.Point(16, 51);
         this.SealantPipePositionLabel.Name = "SealantPipePositionLabel";
         this.SealantPipePositionLabel.Size = new System.Drawing.Size(107, 36);
         this.SealantPipePositionLabel.TabIndex = 144;
         this.SealantPipePositionLabel.Text = "0";
         this.SealantPipePositionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // SealDrillModeButton
         // 
         this.SealDrillModeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SealDrillModeButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.SealDrillModeButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.SealDrillModeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SealDrillModeButton.HoldTimeoutEnable = true;
         this.SealDrillModeButton.HoldTimeoutInterval = 100;
         this.SealDrillModeButton.Location = new System.Drawing.Point(246, 27);
         this.SealDrillModeButton.Name = "SealDrillModeButton";
         this.SealDrillModeButton.Size = new System.Drawing.Size(107, 67);
         this.SealDrillModeButton.TabIndex = 143;
         this.SealDrillModeButton.Text = "SWITCH TO DRILL MODE";
         this.SealDrillModeButton.UseVisualStyleBackColor = false;
         this.SealDrillModeButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.SealDrillModeButton_HoldTimeout);
         // 
         // SealantSetupButton
         // 
         this.SealantSetupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SealantSetupButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.SealantSetupButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.SealantSetupButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.SealantSetupButton.HoldTimeoutEnable = true;
         this.SealantSetupButton.HoldTimeoutInterval = 100;
         this.SealantSetupButton.Location = new System.Drawing.Point(131, 314);
         this.SealantSetupButton.Name = "SealantSetupButton";
         this.SealantSetupButton.Size = new System.Drawing.Size(107, 90);
         this.SealantSetupButton.TabIndex = 124;
         this.SealantSetupButton.Text = "PUMP SETUP";
         this.SealantSetupButton.UseVisualStyleBackColor = false;
         this.SealantSetupButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.SealantSetupButton_HoldTimeout);
         // 
         // NozzleSelectionLabel
         // 
         this.NozzleSelectionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.NozzleSelectionLabel.Location = new System.Drawing.Point(57, 4);
         this.NozzleSelectionLabel.Name = "NozzleSelectionLabel";
         this.NozzleSelectionLabel.Size = new System.Drawing.Size(260, 23);
         this.NozzleSelectionLabel.TabIndex = 88;
         this.NozzleSelectionLabel.Text = "SEALANT PUMP STATUS";
         this.NozzleSelectionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // rotatableLabel3
         // 
         this.rotatableLabel3.Angle = 90;
         this.rotatableLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.rotatableLabel3.Location = new System.Drawing.Point(-10, 205);
         this.rotatableLabel3.Name = "rotatableLabel3";
         this.rotatableLabel3.Size = new System.Drawing.Size(47, 60);
         this.rotatableLabel3.TabIndex = 165;
         this.rotatableLabel3.Text = "ACTUAL";
         this.rotatableLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // rotatableLabel4
         // 
         this.rotatableLabel4.Angle = 90;
         this.rotatableLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.rotatableLabel4.Location = new System.Drawing.Point(-10, 256);
         this.rotatableLabel4.Name = "rotatableLabel4";
         this.rotatableLabel4.Size = new System.Drawing.Size(47, 60);
         this.rotatableLabel4.TabIndex = 166;
         this.rotatableLabel4.Text = "SET";
         this.rotatableLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // rotatableLabel7
         // 
         this.rotatableLabel7.Angle = 90;
         this.rotatableLabel7.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.rotatableLabel7.Location = new System.Drawing.Point(220, 205);
         this.rotatableLabel7.Name = "rotatableLabel7";
         this.rotatableLabel7.Size = new System.Drawing.Size(47, 60);
         this.rotatableLabel7.TabIndex = 168;
         this.rotatableLabel7.Text = "ACTUAL";
         this.rotatableLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // rotatableLabel8
         // 
         this.rotatableLabel8.Angle = 90;
         this.rotatableLabel8.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.rotatableLabel8.Location = new System.Drawing.Point(220, 256);
         this.rotatableLabel8.Name = "rotatableLabel8";
         this.rotatableLabel8.Size = new System.Drawing.Size(47, 60);
         this.rotatableLabel8.TabIndex = 169;
         this.rotatableLabel8.Text = "SET";
         this.rotatableLabel8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // rotatableLabel9
         // 
         this.rotatableLabel9.Angle = 90;
         this.rotatableLabel9.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.rotatableLabel9.Location = new System.Drawing.Point(105, 205);
         this.rotatableLabel9.Name = "rotatableLabel9";
         this.rotatableLabel9.Size = new System.Drawing.Size(47, 60);
         this.rotatableLabel9.TabIndex = 170;
         this.rotatableLabel9.Text = "ACTUAL";
         this.rotatableLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // rotatableLabel10
         // 
         this.rotatableLabel10.Angle = 90;
         this.rotatableLabel10.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.rotatableLabel10.Location = new System.Drawing.Point(105, 256);
         this.rotatableLabel10.Name = "rotatableLabel10";
         this.rotatableLabel10.Size = new System.Drawing.Size(47, 60);
         this.rotatableLabel10.TabIndex = 171;
         this.rotatableLabel10.Text = "SET";
         this.rotatableLabel10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // FeederMainPanel
         // 
         this.FeederMainPanel.BackColor = System.Drawing.Color.Olive;
         this.FeederMainPanel.Controls.Add(this.FeederManulDisplayButton);
         this.FeederMainPanel.Controls.Add(this.FeederLockButton);
         this.FeederMainPanel.Controls.Add(this.FeederOffButton);
         this.FeederMainPanel.Controls.Add(this.FeederActualValuePanel);
         this.FeederMainPanel.Controls.Add(this.FeederMoveButton);
         this.FeederMainPanel.Controls.Add(this.FeederTitleLabel);
         this.FeederMainPanel.Controls.Add(this.FeederSetupButton);
         this.FeederMainPanel.Controls.Add(this.FeederSpeedToggleButton);
         this.FeederMainPanel.Location = new System.Drawing.Point(671, 435);
         this.FeederMainPanel.Name = "FeederMainPanel";
         this.FeederMainPanel.Size = new System.Drawing.Size(386, 277);
         this.FeederMainPanel.TabIndex = 39;
         // 
         // FeederManulDisplayButton
         // 
         this.FeederManulDisplayButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.FeederManulDisplayButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.FeederManulDisplayButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.FeederManulDisplayButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.FeederManulDisplayButton.Location = new System.Drawing.Point(262, 179);
         this.FeederManulDisplayButton.Name = "FeederManulDisplayButton";
         this.FeederManulDisplayButton.Size = new System.Drawing.Size(107, 90);
         this.FeederManulDisplayButton.TabIndex = 163;
         this.FeederManulDisplayButton.Text = "SHOW MANUAL";
         this.FeederManulDisplayButton.UseVisualStyleBackColor = false;
         this.FeederManulDisplayButton.Click += new System.EventHandler(this.FeederManulDisplayButton_Click);
         // 
         // FeederLockButton
         // 
         this.FeederLockButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.FeederLockButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.FeederLockButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.FeederLockButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.FeederLockButton.HoldTimeoutEnable = true;
         this.FeederLockButton.HoldTimeoutInterval = 100;
         this.FeederLockButton.Location = new System.Drawing.Point(262, 81);
         this.FeederLockButton.Name = "FeederLockButton";
         this.FeederLockButton.Size = new System.Drawing.Size(107, 90);
         this.FeederLockButton.TabIndex = 152;
         this.FeederLockButton.Text = "LOCK";
         this.FeederLockButton.UseVisualStyleBackColor = false;
         this.FeederLockButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.FeederLockButton_HoldTimeout);
         // 
         // FeederOffButton
         // 
         this.FeederOffButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.FeederOffButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.FeederOffButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.FeederOffButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.FeederOffButton.HoldTimeoutEnable = true;
         this.FeederOffButton.HoldTimeoutInterval = 100;
         this.FeederOffButton.Location = new System.Drawing.Point(16, 81);
         this.FeederOffButton.Name = "FeederOffButton";
         this.FeederOffButton.Size = new System.Drawing.Size(107, 90);
         this.FeederOffButton.TabIndex = 151;
         this.FeederOffButton.Text = "OFF   (FREE)";
         this.FeederOffButton.UseVisualStyleBackColor = false;
         this.FeederOffButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.FeederOffButton_HoldTimeout);
         // 
         // FeederActualValuePanel
         // 
         this.FeederActualValuePanel.ActiveBackColor = System.Drawing.Color.Black;
         this.FeederActualValuePanel.ActiveFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.FeederActualValuePanel.ActiveForeColor = System.Drawing.Color.White;
         this.FeederActualValuePanel.ArrowWidth = 60;
         this.FeederActualValuePanel.Direction = NICBOT.GUI.DirectionalValuePanel.Directions.Idle;
         this.FeederActualValuePanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.FeederActualValuePanel.ForeColor = System.Drawing.Color.Black;
         this.FeederActualValuePanel.IdleBackColor = System.Drawing.Color.Olive;
         this.FeederActualValuePanel.IdleFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
         this.FeederActualValuePanel.IdleForeColor = System.Drawing.Color.White;
         this.FeederActualValuePanel.LeftArrowText = "REV";
         this.FeederActualValuePanel.Location = new System.Drawing.Point(83, 30);
         this.FeederActualValuePanel.Name = "FeederActualValuePanel";
         this.FeederActualValuePanel.RightArrowText = "FWD";
         this.FeederActualValuePanel.Size = new System.Drawing.Size(219, 42);
         this.FeederActualValuePanel.TabIndex = 143;
         this.FeederActualValuePanel.Text = "directionalValuePanel4";
         this.FeederActualValuePanel.ValueBackColor = System.Drawing.Color.Black;
         this.FeederActualValuePanel.ValueForeColor = System.Drawing.Color.White;
         this.FeederActualValuePanel.ValueText = "### m/s";
         this.FeederActualValuePanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // FeederMoveButton
         // 
         this.FeederMoveButton.ArrowWidth = 12;
         this.FeederMoveButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.FeederMoveButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.FeederMoveButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.FeederMoveButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.FeederMoveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.FeederMoveButton.HoldTimeoutInterval = 100;
         this.FeederMoveButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.FeederMoveButton.LeftArrowVisible = true;
         this.FeederMoveButton.Location = new System.Drawing.Point(139, 81);
         this.FeederMoveButton.Name = "FeederMoveButton";
         this.FeederMoveButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.FeederMoveButton.RightArrowVisible = true;
         this.FeederMoveButton.Size = new System.Drawing.Size(107, 90);
         this.FeederMoveButton.TabIndex = 141;
         this.FeederMoveButton.Text = "MOVE";
         this.FeederMoveButton.UseVisualStyleBackColor = false;
         this.FeederMoveButton.ValueBackColor = System.Drawing.Color.Black;
         this.FeederMoveButton.ValueEdgeHeight = 8;
         this.FeederMoveButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.FeederMoveButton.ValueForeColor = System.Drawing.Color.White;
         this.FeederMoveButton.ValueHeight = 22;
         this.FeederMoveButton.ValueText = "### M/S";
         this.FeederMoveButton.ValueWidth = 80;
         this.FeederMoveButton.HoldTimeout += new NICBOT.GUI.ValueButton.HoldTimeoutHandler(this.FeederMoveButton_HoldTimeout);
         // 
         // FeederTitleLabel
         // 
         this.FeederTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.FeederTitleLabel.Location = new System.Drawing.Point(39, 4);
         this.FeederTitleLabel.Name = "FeederTitleLabel";
         this.FeederTitleLabel.Size = new System.Drawing.Size(306, 23);
         this.FeederTitleLabel.TabIndex = 137;
         this.FeederTitleLabel.Text = "TETHER FEEDER - NO MOTION";
         this.FeederTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // FeederSetupButton
         // 
         this.FeederSetupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.FeederSetupButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.FeederSetupButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.FeederSetupButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.FeederSetupButton.ForeColor = System.Drawing.Color.Black;
         this.FeederSetupButton.HoldTimeoutEnable = true;
         this.FeederSetupButton.HoldTimeoutInterval = 100;
         this.FeederSetupButton.Location = new System.Drawing.Point(16, 179);
         this.FeederSetupButton.Name = "FeederSetupButton";
         this.FeederSetupButton.Size = new System.Drawing.Size(107, 90);
         this.FeederSetupButton.TabIndex = 126;
         this.FeederSetupButton.Text = "FEEDER SETUP";
         this.FeederSetupButton.UseVisualStyleBackColor = false;
         this.FeederSetupButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.FeederSetupButton_HoldTimeout);
         // 
         // FeederSpeedToggleButton
         // 
         this.FeederSpeedToggleButton.AutomaticToggle = true;
         this.FeederSpeedToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.FeederSpeedToggleButton.DisabledBackColor = System.Drawing.Color.Black;
         this.FeederSpeedToggleButton.DisabledForeColor = System.Drawing.Color.Gray;
         this.FeederSpeedToggleButton.DisabledOptionBackColor = System.Drawing.Color.Black;
         this.FeederSpeedToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.FeederSpeedToggleButton.HoldEnable = false;
         this.FeederSpeedToggleButton.HoldTimeoutInterval = 0;
         this.FeederSpeedToggleButton.Location = new System.Drawing.Point(139, 179);
         this.FeederSpeedToggleButton.Name = "FeederSpeedToggleButton";
         this.FeederSpeedToggleButton.OptionASelected = true;
         this.FeederSpeedToggleButton.OptionAText = "FAST";
         this.FeederSpeedToggleButton.OptionBSelected = false;
         this.FeederSpeedToggleButton.OptionBText = "SLOW";
         this.FeederSpeedToggleButton.OptionCenterWidth = 2;
         this.FeederSpeedToggleButton.OptionEdgeHeight = 8;
         this.FeederSpeedToggleButton.OptionHeight = 22;
         this.FeederSpeedToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.FeederSpeedToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
         this.FeederSpeedToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.FeederSpeedToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.FeederSpeedToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.FeederSpeedToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.FeederSpeedToggleButton.OptionWidth = 45;
         this.FeederSpeedToggleButton.Size = new System.Drawing.Size(107, 90);
         this.FeederSpeedToggleButton.TabIndex = 124;
         this.FeederSpeedToggleButton.Text = "SPEED";
         this.FeederSpeedToggleButton.UseVisualStyleBackColor = false;
         this.FeederSpeedToggleButton.Click += new System.EventHandler(this.FeederSpeedToggleButton_Click);
         // 
         // FeederManualPanel
         // 
         this.FeederManualPanel.BackColor = System.Drawing.Color.Olive;
         this.FeederManualPanel.Controls.Add(this.FeederClampSetupButton);
         this.FeederManualPanel.Controls.Add(this.FeederHideManulButton);
         this.FeederManualPanel.Controls.Add(this.FeederManualSetupButton);
         this.FeederManualPanel.Controls.Add(this.FeederManualReverseButton);
         this.FeederManualPanel.Controls.Add(this.FeederManualForwardButton);
         this.FeederManualPanel.Controls.Add(this.FeederSpeedValueButton);
         this.FeederManualPanel.Location = new System.Drawing.Point(1938, 857);
         this.FeederManualPanel.Name = "FeederManualPanel";
         this.FeederManualPanel.Size = new System.Drawing.Size(386, 188);
         this.FeederManualPanel.TabIndex = 125;
         // 
         // FeederClampSetupButton
         // 
         this.FeederClampSetupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.FeederClampSetupButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.FeederClampSetupButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.FeederClampSetupButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.FeederClampSetupButton.ForeColor = System.Drawing.Color.Black;
         this.FeederClampSetupButton.HoldTimeoutEnable = true;
         this.FeederClampSetupButton.HoldTimeoutInterval = 100;
         this.FeederClampSetupButton.Location = new System.Drawing.Point(139, 98);
         this.FeederClampSetupButton.Name = "FeederClampSetupButton";
         this.FeederClampSetupButton.Size = new System.Drawing.Size(107, 90);
         this.FeederClampSetupButton.TabIndex = 165;
         this.FeederClampSetupButton.Text = "CLAMP SETUP";
         this.FeederClampSetupButton.UseVisualStyleBackColor = false;
         this.FeederClampSetupButton.Visible = false;
         this.FeederClampSetupButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.FeederClampSetupButton_HoldTimeout);
         // 
         // FeederHideManulButton
         // 
         this.FeederHideManulButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.FeederHideManulButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.FeederHideManulButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.FeederHideManulButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.FeederHideManulButton.Location = new System.Drawing.Point(262, 98);
         this.FeederHideManulButton.Name = "FeederHideManulButton";
         this.FeederHideManulButton.Size = new System.Drawing.Size(107, 90);
         this.FeederHideManulButton.TabIndex = 164;
         this.FeederHideManulButton.Text = "HIDE MANUAL";
         this.FeederHideManulButton.UseVisualStyleBackColor = false;
         this.FeederHideManulButton.Click += new System.EventHandler(this.FeederManulDisplayButton_Click);
         // 
         // FeederManualSetupButton
         // 
         this.FeederManualSetupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.FeederManualSetupButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.FeederManualSetupButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.FeederManualSetupButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.FeederManualSetupButton.ForeColor = System.Drawing.Color.Black;
         this.FeederManualSetupButton.HoldTimeoutEnable = true;
         this.FeederManualSetupButton.HoldTimeoutInterval = 100;
         this.FeederManualSetupButton.Location = new System.Drawing.Point(16, 98);
         this.FeederManualSetupButton.Name = "FeederManualSetupButton";
         this.FeederManualSetupButton.Size = new System.Drawing.Size(107, 90);
         this.FeederManualSetupButton.TabIndex = 157;
         this.FeederManualSetupButton.Text = "FEEDER SETUP";
         this.FeederManualSetupButton.UseVisualStyleBackColor = false;
         this.FeederManualSetupButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.FeederSetupButton_HoldTimeout);
         // 
         // FeederManualReverseButton
         // 
         this.FeederManualReverseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.FeederManualReverseButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.FeederManualReverseButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.FeederManualReverseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.FeederManualReverseButton.HoldTimeoutEnable = true;
         this.FeederManualReverseButton.HoldTimeoutInterval = 100;
         this.FeederManualReverseButton.Location = new System.Drawing.Point(16, 0);
         this.FeederManualReverseButton.Name = "FeederManualReverseButton";
         this.FeederManualReverseButton.Size = new System.Drawing.Size(107, 90);
         this.FeederManualReverseButton.TabIndex = 156;
         this.FeederManualReverseButton.Text = "MANUAL FEED REV/CCW";
         this.FeederManualReverseButton.UseVisualStyleBackColor = false;
         this.FeederManualReverseButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FeederManualReverseButton_MouseDown);
         this.FeederManualReverseButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FeederManualReverseButton_MouseUp);
         // 
         // FeederManualForwardButton
         // 
         this.FeederManualForwardButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.FeederManualForwardButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.FeederManualForwardButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.FeederManualForwardButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.FeederManualForwardButton.HoldTimeoutEnable = true;
         this.FeederManualForwardButton.HoldTimeoutInterval = 100;
         this.FeederManualForwardButton.Location = new System.Drawing.Point(262, 0);
         this.FeederManualForwardButton.Name = "FeederManualForwardButton";
         this.FeederManualForwardButton.Size = new System.Drawing.Size(107, 90);
         this.FeederManualForwardButton.TabIndex = 155;
         this.FeederManualForwardButton.Text = "MANUAL FEED FWD/CW";
         this.FeederManualForwardButton.UseVisualStyleBackColor = false;
         this.FeederManualForwardButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FeederManualForwardButton_MouseDown);
         this.FeederManualForwardButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FeederManualForwardButton_MouseUp);
         // 
         // FeederSpeedValueButton
         // 
         this.FeederSpeedValueButton.ArrowWidth = 0;
         this.FeederSpeedValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.FeederSpeedValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.FeederSpeedValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.FeederSpeedValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.FeederSpeedValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.FeederSpeedValueButton.HoldTimeoutInterval = 100;
         this.FeederSpeedValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.FeederSpeedValueButton.LeftArrowVisible = false;
         this.FeederSpeedValueButton.Location = new System.Drawing.Point(139, 0);
         this.FeederSpeedValueButton.Name = "FeederSpeedValueButton";
         this.FeederSpeedValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.FeederSpeedValueButton.RightArrowVisible = false;
         this.FeederSpeedValueButton.Size = new System.Drawing.Size(107, 90);
         this.FeederSpeedValueButton.TabIndex = 154;
         this.FeederSpeedValueButton.Text = "FEEDER SPEED";
         this.FeederSpeedValueButton.UseVisualStyleBackColor = false;
         this.FeederSpeedValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.FeederSpeedValueButton.ValueEdgeHeight = 8;
         this.FeederSpeedValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.FeederSpeedValueButton.ValueForeColor = System.Drawing.Color.White;
         this.FeederSpeedValueButton.ValueHeight = 22;
         this.FeederSpeedValueButton.ValueText = "### mm/S";
         this.FeederSpeedValueButton.ValueWidth = 80;
         this.FeederSpeedValueButton.HoldTimeout += new NICBOT.GUI.ValueButton.HoldTimeoutHandler(this.FeederSpeedValueButton_HoldTimeout);
         // 
         // DrillManualPanel
         // 
         this.DrillManualPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.DrillManualPanel.Controls.Add(this.DrillRetractToLimitButton);
         this.DrillManualPanel.Controls.Add(this.DrillStopButton);
         this.DrillManualPanel.Controls.Add(this.DrillMoveToOriginButton);
         this.DrillManualPanel.Controls.Add(this.borderedPanel1);
         this.DrillManualPanel.Controls.Add(this.DrillSetOriginButton);
         this.DrillManualPanel.Controls.Add(this.DrillFindOriginButton);
         this.DrillManualPanel.Controls.Add(this.DrillIndexUpButton);
         this.DrillManualPanel.Controls.Add(this.DrillIndexDownButton);
         this.DrillManualPanel.Controls.Add(this.label45);
         this.DrillManualPanel.Location = new System.Drawing.Point(1063, 481);
         this.DrillManualPanel.Name = "DrillManualPanel";
         this.DrillManualPanel.Size = new System.Drawing.Size(369, 383);
         this.DrillManualPanel.TabIndex = 40;
         // 
         // DrillRetractToLimitButton
         // 
         this.DrillRetractToLimitButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillRetractToLimitButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.DrillRetractToLimitButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.DrillRetractToLimitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.DrillRetractToLimitButton.Location = new System.Drawing.Point(8, 114);
         this.DrillRetractToLimitButton.Name = "DrillRetractToLimitButton";
         this.DrillRetractToLimitButton.Size = new System.Drawing.Size(107, 90);
         this.DrillRetractToLimitButton.TabIndex = 166;
         this.DrillRetractToLimitButton.Text = "RETRACT TO        LIMIT";
         this.DrillRetractToLimitButton.UseVisualStyleBackColor = false;
         this.DrillRetractToLimitButton.Click += new System.EventHandler(this.DrillRetractToLimitButton_Click);
         // 
         // DrillStopButton
         // 
         this.DrillStopButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillStopButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.DrillStopButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.DrillStopButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
         this.DrillStopButton.Location = new System.Drawing.Point(123, 255);
         this.DrillStopButton.Name = "DrillStopButton";
         this.DrillStopButton.Size = new System.Drawing.Size(107, 67);
         this.DrillStopButton.TabIndex = 165;
         this.DrillStopButton.Text = "STOP";
         this.DrillStopButton.UseVisualStyleBackColor = false;
         this.DrillStopButton.Click += new System.EventHandler(this.DrillStopButton_Click);
         // 
         // DrillMoveToOriginButton
         // 
         this.DrillMoveToOriginButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillMoveToOriginButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.DrillMoveToOriginButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.DrillMoveToOriginButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.DrillMoveToOriginButton.Location = new System.Drawing.Point(123, 114);
         this.DrillMoveToOriginButton.Name = "DrillMoveToOriginButton";
         this.DrillMoveToOriginButton.Size = new System.Drawing.Size(107, 90);
         this.DrillMoveToOriginButton.TabIndex = 164;
         this.DrillMoveToOriginButton.Text = "MOVE             TO            ORIGIN";
         this.DrillMoveToOriginButton.UseVisualStyleBackColor = false;
         this.DrillMoveToOriginButton.Click += new System.EventHandler(this.DrillMoveToOriginButton_Click);
         // 
         // borderedPanel1
         // 
         this.borderedPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
         this.borderedPanel1.Controls.Add(this.DrillManualToggleButton);
         this.borderedPanel1.Controls.Add(this.DrillDirectionToggleButton);
         this.borderedPanel1.Controls.Add(this.DrillSpeedDecreaseButton);
         this.borderedPanel1.Controls.Add(this.label43);
         this.borderedPanel1.Controls.Add(this.DrillSpeedIncreaseButton);
         this.borderedPanel1.EdgeWeight = 1;
         this.borderedPanel1.Location = new System.Drawing.Point(238, 8);
         this.borderedPanel1.Name = "borderedPanel1";
         this.borderedPanel1.Size = new System.Drawing.Size(123, 367);
         this.borderedPanel1.TabIndex = 125;
         // 
         // DrillManualToggleButton
         // 
         this.DrillManualToggleButton.AutomaticToggle = true;
         this.DrillManualToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillManualToggleButton.DisabledBackColor = System.Drawing.Color.Black;
         this.DrillManualToggleButton.DisabledForeColor = System.Drawing.Color.Gray;
         this.DrillManualToggleButton.DisabledOptionBackColor = System.Drawing.Color.Black;
         this.DrillManualToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DrillManualToggleButton.HoldEnable = true;
         this.DrillManualToggleButton.HoldTimeoutInterval = 100;
         this.DrillManualToggleButton.Location = new System.Drawing.Point(8, 8);
         this.DrillManualToggleButton.Name = "DrillManualToggleButton";
         this.DrillManualToggleButton.OptionASelected = true;
         this.DrillManualToggleButton.OptionAText = "ON";
         this.DrillManualToggleButton.OptionBSelected = false;
         this.DrillManualToggleButton.OptionBText = "OFF";
         this.DrillManualToggleButton.OptionCenterWidth = 2;
         this.DrillManualToggleButton.OptionEdgeHeight = 8;
         this.DrillManualToggleButton.OptionHeight = 22;
         this.DrillManualToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.DrillManualToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
         this.DrillManualToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.DrillManualToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.DrillManualToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DrillManualToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.DrillManualToggleButton.OptionWidth = 45;
         this.DrillManualToggleButton.Size = new System.Drawing.Size(107, 90);
         this.DrillManualToggleButton.TabIndex = 121;
         this.DrillManualToggleButton.Text = "MANUAL DRILL";
         this.DrillManualToggleButton.UseVisualStyleBackColor = false;
         this.DrillManualToggleButton.HoldTimeout += new NICBOT.GUI.ValueToggleButton.HoldTimeoutHandler(this.DrillManualToggleButton_HoldTimeout);
         // 
         // DrillDirectionToggleButton
         // 
         this.DrillDirectionToggleButton.AutomaticToggle = true;
         this.DrillDirectionToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillDirectionToggleButton.DisabledBackColor = System.Drawing.Color.Black;
         this.DrillDirectionToggleButton.DisabledForeColor = System.Drawing.Color.Gray;
         this.DrillDirectionToggleButton.DisabledOptionBackColor = System.Drawing.Color.Black;
         this.DrillDirectionToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DrillDirectionToggleButton.HoldEnable = false;
         this.DrillDirectionToggleButton.HoldTimeoutInterval = 0;
         this.DrillDirectionToggleButton.Location = new System.Drawing.Point(8, 106);
         this.DrillDirectionToggleButton.Name = "DrillDirectionToggleButton";
         this.DrillDirectionToggleButton.OptionASelected = true;
         this.DrillDirectionToggleButton.OptionAText = "FWD";
         this.DrillDirectionToggleButton.OptionBSelected = false;
         this.DrillDirectionToggleButton.OptionBText = "REV";
         this.DrillDirectionToggleButton.OptionCenterWidth = 2;
         this.DrillDirectionToggleButton.OptionEdgeHeight = 8;
         this.DrillDirectionToggleButton.OptionHeight = 22;
         this.DrillDirectionToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.DrillDirectionToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
         this.DrillDirectionToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.DrillDirectionToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.DrillDirectionToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.DrillDirectionToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.DrillDirectionToggleButton.OptionWidth = 45;
         this.DrillDirectionToggleButton.Size = new System.Drawing.Size(107, 90);
         this.DrillDirectionToggleButton.TabIndex = 124;
         this.DrillDirectionToggleButton.Text = "DRILL DIRECTION";
         this.DrillDirectionToggleButton.UseVisualStyleBackColor = false;
         this.DrillDirectionToggleButton.Visible = false;
         this.DrillDirectionToggleButton.Click += new System.EventHandler(this.DrillDirectionToggleButton_Click);
         // 
         // DrillSpeedDecreaseButton
         // 
         this.DrillSpeedDecreaseButton.ArrowColor = System.Drawing.Color.Black;
         this.DrillSpeedDecreaseButton.ArrowHighlightColor = System.Drawing.Color.DarkGray;
         this.DrillSpeedDecreaseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillSpeedDecreaseButton.DisabledArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.DrillSpeedDecreaseButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.DrillSpeedDecreaseButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.DrillSpeedDecreaseButton.EdgeSpace = 8;
         this.DrillSpeedDecreaseButton.ForeColor = System.Drawing.SystemColors.HighlightText;
         this.DrillSpeedDecreaseButton.HighLightOffset = 7;
         this.DrillSpeedDecreaseButton.HighlightVisible = true;
         this.DrillSpeedDecreaseButton.HighLightWeight = 2;
         this.DrillSpeedDecreaseButton.HoldRepeat = true;
         this.DrillSpeedDecreaseButton.HoldRepeatInterval = 100;
         this.DrillSpeedDecreaseButton.HoldTimeoutInterval = 100;
         this.DrillSpeedDecreaseButton.Location = new System.Drawing.Point(8, 292);
         this.DrillSpeedDecreaseButton.Name = "DrillSpeedDecreaseButton";
         this.DrillSpeedDecreaseButton.Size = new System.Drawing.Size(107, 67);
         this.DrillSpeedDecreaseButton.TabIndex = 76;
         this.DrillSpeedDecreaseButton.Text = "DECREASE";
         this.DrillSpeedDecreaseButton.TextOffset = 0;
         this.DrillSpeedDecreaseButton.TextVisible = false;
         this.DrillSpeedDecreaseButton.UpDown = false;
         this.DrillSpeedDecreaseButton.UseVisualStyleBackColor = false;
         this.DrillSpeedDecreaseButton.HoldTimeout += new NICBOT.GUI.UpDownButton.HoldTimeoutHandler(this.DrillSpeedDecreaseButton_HoldTimeout);
         this.DrillSpeedDecreaseButton.Click += new System.EventHandler(this.DrillSpeedDecreaseButton_Click);
         // 
         // label43
         // 
         this.label43.AutoSize = true;
         this.label43.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label43.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.label43.Location = new System.Drawing.Point(14, 273);
         this.label43.Name = "label43";
         this.label43.Size = new System.Drawing.Size(94, 16);
         this.label43.TabIndex = 74;
         this.label43.Text = "DRILL SPEED";
         // 
         // DrillSpeedIncreaseButton
         // 
         this.DrillSpeedIncreaseButton.ArrowColor = System.Drawing.Color.Black;
         this.DrillSpeedIncreaseButton.ArrowHighlightColor = System.Drawing.Color.DarkGray;
         this.DrillSpeedIncreaseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillSpeedIncreaseButton.DisabledArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.DrillSpeedIncreaseButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.DrillSpeedIncreaseButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.DrillSpeedIncreaseButton.EdgeSpace = 8;
         this.DrillSpeedIncreaseButton.ForeColor = System.Drawing.SystemColors.HighlightText;
         this.DrillSpeedIncreaseButton.HighLightOffset = 7;
         this.DrillSpeedIncreaseButton.HighlightVisible = true;
         this.DrillSpeedIncreaseButton.HighLightWeight = 2;
         this.DrillSpeedIncreaseButton.HoldRepeat = true;
         this.DrillSpeedIncreaseButton.HoldRepeatInterval = 100;
         this.DrillSpeedIncreaseButton.HoldTimeoutInterval = 100;
         this.DrillSpeedIncreaseButton.Location = new System.Drawing.Point(8, 204);
         this.DrillSpeedIncreaseButton.Name = "DrillSpeedIncreaseButton";
         this.DrillSpeedIncreaseButton.Size = new System.Drawing.Size(107, 67);
         this.DrillSpeedIncreaseButton.TabIndex = 75;
         this.DrillSpeedIncreaseButton.Text = "INCREASE";
         this.DrillSpeedIncreaseButton.TextOffset = 0;
         this.DrillSpeedIncreaseButton.TextVisible = false;
         this.DrillSpeedIncreaseButton.UpDown = true;
         this.DrillSpeedIncreaseButton.UseVisualStyleBackColor = false;
         this.DrillSpeedIncreaseButton.HoldTimeout += new NICBOT.GUI.UpDownButton.HoldTimeoutHandler(this.DrillSpeedIncreaseButton_HoldTimeout);
         this.DrillSpeedIncreaseButton.Click += new System.EventHandler(this.DrillSpeedIncreaseButton_Click);
         // 
         // DrillSetOriginButton
         // 
         this.DrillSetOriginButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillSetOriginButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.DrillSetOriginButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.DrillSetOriginButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.DrillSetOriginButton.HoldTimeoutEnable = true;
         this.DrillSetOriginButton.HoldTimeoutInterval = 100;
         this.DrillSetOriginButton.Location = new System.Drawing.Point(123, 16);
         this.DrillSetOriginButton.Name = "DrillSetOriginButton";
         this.DrillSetOriginButton.Size = new System.Drawing.Size(107, 90);
         this.DrillSetOriginButton.TabIndex = 123;
         this.DrillSetOriginButton.Text = "SET  ORIGIN";
         this.DrillSetOriginButton.UseVisualStyleBackColor = false;
         this.DrillSetOriginButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.DrillSetOriginButton_HoldTimeout);
         // 
         // DrillFindOriginButton
         // 
         this.DrillFindOriginButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillFindOriginButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.DrillFindOriginButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.DrillFindOriginButton.Enabled = false;
         this.DrillFindOriginButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.DrillFindOriginButton.HoldTimeoutEnable = true;
         this.DrillFindOriginButton.HoldTimeoutInterval = 100;
         this.DrillFindOriginButton.Location = new System.Drawing.Point(8, 16);
         this.DrillFindOriginButton.Name = "DrillFindOriginButton";
         this.DrillFindOriginButton.Size = new System.Drawing.Size(107, 90);
         this.DrillFindOriginButton.TabIndex = 122;
         this.DrillFindOriginButton.Text = "FIND ORIGIN";
         this.DrillFindOriginButton.UseVisualStyleBackColor = false;
         this.DrillFindOriginButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.DrillFindOriginButton_HoldTimeout);
         // 
         // DrillIndexUpButton
         // 
         this.DrillIndexUpButton.ArrowColor = System.Drawing.Color.Black;
         this.DrillIndexUpButton.ArrowHighlightColor = System.Drawing.Color.DarkGray;
         this.DrillIndexUpButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillIndexUpButton.DisabledArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.DrillIndexUpButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.DrillIndexUpButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.DrillIndexUpButton.EdgeSpace = 8;
         this.DrillIndexUpButton.ForeColor = System.Drawing.SystemColors.HighlightText;
         this.DrillIndexUpButton.HighLightOffset = 5;
         this.DrillIndexUpButton.HighlightVisible = false;
         this.DrillIndexUpButton.HighLightWeight = 1;
         this.DrillIndexUpButton.HoldRepeat = true;
         this.DrillIndexUpButton.HoldRepeatInterval = 100;
         this.DrillIndexUpButton.HoldTimeoutInterval = 100;
         this.DrillIndexUpButton.Location = new System.Drawing.Point(8, 212);
         this.DrillIndexUpButton.Name = "DrillIndexUpButton";
         this.DrillIndexUpButton.Size = new System.Drawing.Size(107, 67);
         this.DrillIndexUpButton.TabIndex = 69;
         this.DrillIndexUpButton.Text = "RETRACT";
         this.DrillIndexUpButton.TextOffset = 5;
         this.DrillIndexUpButton.TextVisible = true;
         this.DrillIndexUpButton.UpDown = true;
         this.DrillIndexUpButton.UseVisualStyleBackColor = false;
         this.DrillIndexUpButton.HoldTimeout += new NICBOT.GUI.UpDownButton.HoldTimeoutHandler(this.DrillIndexUpButton_HoldTimeout);
         this.DrillIndexUpButton.Click += new System.EventHandler(this.DrillIndexUpButton_Click);
         // 
         // DrillIndexDownButton
         // 
         this.DrillIndexDownButton.ArrowColor = System.Drawing.Color.Black;
         this.DrillIndexDownButton.ArrowHighlightColor = System.Drawing.Color.DarkGray;
         this.DrillIndexDownButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.DrillIndexDownButton.DisabledArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.DrillIndexDownButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.DrillIndexDownButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.DrillIndexDownButton.EdgeSpace = 8;
         this.DrillIndexDownButton.ForeColor = System.Drawing.SystemColors.HighlightText;
         this.DrillIndexDownButton.HighLightOffset = 5;
         this.DrillIndexDownButton.HighlightVisible = false;
         this.DrillIndexDownButton.HighLightWeight = 1;
         this.DrillIndexDownButton.HoldRepeat = true;
         this.DrillIndexDownButton.HoldRepeatInterval = 100;
         this.DrillIndexDownButton.HoldTimeoutInterval = 100;
         this.DrillIndexDownButton.Location = new System.Drawing.Point(8, 300);
         this.DrillIndexDownButton.Name = "DrillIndexDownButton";
         this.DrillIndexDownButton.Size = new System.Drawing.Size(107, 67);
         this.DrillIndexDownButton.TabIndex = 70;
         this.DrillIndexDownButton.Text = "EXTEND";
         this.DrillIndexDownButton.TextOffset = 5;
         this.DrillIndexDownButton.TextVisible = true;
         this.DrillIndexDownButton.UpDown = false;
         this.DrillIndexDownButton.UseVisualStyleBackColor = false;
         this.DrillIndexDownButton.HoldTimeout += new NICBOT.GUI.UpDownButton.HoldTimeoutHandler(this.DrillIndexDownButton_HoldTimeout);
         this.DrillIndexDownButton.Click += new System.EventHandler(this.DrillIndexDownButton_Click);
         // 
         // panel1
         // 
         this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.panel1.Controls.Add(this.RearSealantReserviorPanel);
         this.panel1.Controls.Add(this.FrontSealantReserviorPanel);
         this.panel1.Controls.Add(this.borderedPanel2);
         this.panel1.Location = new System.Drawing.Point(2, 322);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(262, 318);
         this.panel1.TabIndex = 41;
         // 
         // RearSealantReserviorPanel
         // 
         this.RearSealantReserviorPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.RearSealantReserviorPanel.Controls.Add(this.lineControl6);
         this.RearSealantReserviorPanel.Controls.Add(this.label11);
         this.RearSealantReserviorPanel.Controls.Add(this.RearSealantReserviorWeightTextPanel);
         this.RearSealantReserviorPanel.Controls.Add(this.label21);
         this.RearSealantReserviorPanel.Controls.Add(this.label33);
         this.RearSealantReserviorPanel.Controls.Add(this.RearSealantReserviorVolumeTextPanel);
         this.RearSealantReserviorPanel.EdgeWeight = 2;
         this.RearSealantReserviorPanel.Location = new System.Drawing.Point(135, 163);
         this.RearSealantReserviorPanel.Name = "RearSealantReserviorPanel";
         this.RearSealantReserviorPanel.Size = new System.Drawing.Size(119, 147);
         this.RearSealantReserviorPanel.TabIndex = 181;
         // 
         // lineControl6
         // 
         this.lineControl6.BackColor = System.Drawing.Color.Transparent;
         this.lineControl6.EdgeColor = System.Drawing.Color.Black;
         this.lineControl6.LineType = NICBOT.GUI.LineControl.LineDrawType.Top;
         this.lineControl6.LineWeight = 1;
         this.lineControl6.Location = new System.Drawing.Point(2, 22);
         this.lineControl6.Name = "lineControl6";
         this.lineControl6.Opacity = 100;
         this.lineControl6.ShowBackground = false;
         this.lineControl6.ShowEdge = false;
         this.lineControl6.Size = new System.Drawing.Size(115, 8);
         this.lineControl6.TabIndex = 171;
         this.lineControl6.Text = "lineControl6";
         // 
         // label11
         // 
         this.label11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(209)))), ((int)(((byte)(234)))));
         this.label11.Location = new System.Drawing.Point(2, 2);
         this.label11.Name = "label11";
         this.label11.Size = new System.Drawing.Size(115, 20);
         this.label11.TabIndex = 169;
         this.label11.Text = "REAR";
         this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // RearSealantReserviorWeightTextPanel
         // 
         this.RearSealantReserviorWeightTextPanel.BackColor = System.Drawing.Color.Black;
         this.RearSealantReserviorWeightTextPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.RearSealantReserviorWeightTextPanel.Enabled = false;
         this.RearSealantReserviorWeightTextPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.RearSealantReserviorWeightTextPanel.ForeColor = System.Drawing.Color.White;
         this.RearSealantReserviorWeightTextPanel.HoldTimeoutEnable = false;
         this.RearSealantReserviorWeightTextPanel.HoldTimeoutInterval = 100;
         this.RearSealantReserviorWeightTextPanel.Location = new System.Drawing.Point(10, 38);
         this.RearSealantReserviorWeightTextPanel.Name = "RearSealantReserviorWeightTextPanel";
         this.RearSealantReserviorWeightTextPanel.Size = new System.Drawing.Size(99, 42);
         this.RearSealantReserviorWeightTextPanel.TabIndex = 165;
         this.RearSealantReserviorWeightTextPanel.ValueText = "##### mL";
         this.RearSealantReserviorWeightTextPanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label21
         // 
         this.label21.AutoSize = true;
         this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label21.Location = new System.Drawing.Point(16, 23);
         this.label21.Name = "label21";
         this.label21.Size = new System.Drawing.Size(86, 15);
         this.label21.TabIndex = 164;
         this.label21.Text = "RESERVIOR";
         this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label33
         // 
         this.label33.AutoSize = true;
         this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label33.Location = new System.Drawing.Point(27, 80);
         this.label33.Name = "label33";
         this.label33.Size = new System.Drawing.Size(64, 15);
         this.label33.TabIndex = 166;
         this.label33.Text = "VOLUME";
         this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // RearSealantReserviorVolumeTextPanel
         // 
         this.RearSealantReserviorVolumeTextPanel.BackColor = System.Drawing.Color.Black;
         this.RearSealantReserviorVolumeTextPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.RearSealantReserviorVolumeTextPanel.Enabled = false;
         this.RearSealantReserviorVolumeTextPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.RearSealantReserviorVolumeTextPanel.ForeColor = System.Drawing.Color.White;
         this.RearSealantReserviorVolumeTextPanel.HoldTimeoutEnable = false;
         this.RearSealantReserviorVolumeTextPanel.HoldTimeoutInterval = 100;
         this.RearSealantReserviorVolumeTextPanel.Location = new System.Drawing.Point(10, 95);
         this.RearSealantReserviorVolumeTextPanel.Name = "RearSealantReserviorVolumeTextPanel";
         this.RearSealantReserviorVolumeTextPanel.Size = new System.Drawing.Size(99, 42);
         this.RearSealantReserviorVolumeTextPanel.TabIndex = 167;
         this.RearSealantReserviorVolumeTextPanel.ValueText = "#### mL";
         this.RearSealantReserviorVolumeTextPanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // FrontSealantReserviorPanel
         // 
         this.FrontSealantReserviorPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.FrontSealantReserviorPanel.Controls.Add(this.lineControl5);
         this.FrontSealantReserviorPanel.Controls.Add(this.label32);
         this.FrontSealantReserviorPanel.Controls.Add(this.FrontSealantReserviorWeightTextPanel);
         this.FrontSealantReserviorPanel.Controls.Add(this.label14);
         this.FrontSealantReserviorPanel.Controls.Add(this.label16);
         this.FrontSealantReserviorPanel.Controls.Add(this.FrontSealantReserviorVolumeTextPanel);
         this.FrontSealantReserviorPanel.EdgeWeight = 2;
         this.FrontSealantReserviorPanel.Location = new System.Drawing.Point(8, 163);
         this.FrontSealantReserviorPanel.Name = "FrontSealantReserviorPanel";
         this.FrontSealantReserviorPanel.Size = new System.Drawing.Size(119, 147);
         this.FrontSealantReserviorPanel.TabIndex = 180;
         // 
         // lineControl5
         // 
         this.lineControl5.BackColor = System.Drawing.Color.Transparent;
         this.lineControl5.EdgeColor = System.Drawing.Color.Black;
         this.lineControl5.LineType = NICBOT.GUI.LineControl.LineDrawType.Top;
         this.lineControl5.LineWeight = 1;
         this.lineControl5.Location = new System.Drawing.Point(2, 22);
         this.lineControl5.Name = "lineControl5";
         this.lineControl5.Opacity = 100;
         this.lineControl5.ShowBackground = false;
         this.lineControl5.ShowEdge = false;
         this.lineControl5.Size = new System.Drawing.Size(115, 8);
         this.lineControl5.TabIndex = 171;
         this.lineControl5.Text = "lineControl5";
         // 
         // label32
         // 
         this.label32.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label32.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(209)))), ((int)(((byte)(234)))));
         this.label32.Location = new System.Drawing.Point(2, 2);
         this.label32.Name = "label32";
         this.label32.Size = new System.Drawing.Size(115, 20);
         this.label32.TabIndex = 169;
         this.label32.Text = "FRONT";
         this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // FrontSealantReserviorWeightTextPanel
         // 
         this.FrontSealantReserviorWeightTextPanel.BackColor = System.Drawing.Color.Black;
         this.FrontSealantReserviorWeightTextPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.FrontSealantReserviorWeightTextPanel.Enabled = false;
         this.FrontSealantReserviorWeightTextPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.FrontSealantReserviorWeightTextPanel.ForeColor = System.Drawing.Color.White;
         this.FrontSealantReserviorWeightTextPanel.HoldTimeoutEnable = false;
         this.FrontSealantReserviorWeightTextPanel.HoldTimeoutInterval = 100;
         this.FrontSealantReserviorWeightTextPanel.Location = new System.Drawing.Point(10, 38);
         this.FrontSealantReserviorWeightTextPanel.Name = "FrontSealantReserviorWeightTextPanel";
         this.FrontSealantReserviorWeightTextPanel.Size = new System.Drawing.Size(99, 42);
         this.FrontSealantReserviorWeightTextPanel.TabIndex = 165;
         this.FrontSealantReserviorWeightTextPanel.ValueText = "##### mL";
         this.FrontSealantReserviorWeightTextPanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label14
         // 
         this.label14.AutoSize = true;
         this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label14.Location = new System.Drawing.Point(16, 23);
         this.label14.Name = "label14";
         this.label14.Size = new System.Drawing.Size(86, 15);
         this.label14.TabIndex = 164;
         this.label14.Text = "RESERVIOR";
         this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label16
         // 
         this.label16.AutoSize = true;
         this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label16.Location = new System.Drawing.Point(27, 80);
         this.label16.Name = "label16";
         this.label16.Size = new System.Drawing.Size(64, 15);
         this.label16.TabIndex = 166;
         this.label16.Text = "VOLUME";
         this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // FrontSealantReserviorVolumeTextPanel
         // 
         this.FrontSealantReserviorVolumeTextPanel.BackColor = System.Drawing.Color.Black;
         this.FrontSealantReserviorVolumeTextPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.FrontSealantReserviorVolumeTextPanel.Enabled = false;
         this.FrontSealantReserviorVolumeTextPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.FrontSealantReserviorVolumeTextPanel.ForeColor = System.Drawing.Color.White;
         this.FrontSealantReserviorVolumeTextPanel.HoldTimeoutEnable = false;
         this.FrontSealantReserviorVolumeTextPanel.HoldTimeoutInterval = 100;
         this.FrontSealantReserviorVolumeTextPanel.Location = new System.Drawing.Point(10, 95);
         this.FrontSealantReserviorVolumeTextPanel.Name = "FrontSealantReserviorVolumeTextPanel";
         this.FrontSealantReserviorVolumeTextPanel.Size = new System.Drawing.Size(99, 42);
         this.FrontSealantReserviorVolumeTextPanel.TabIndex = 167;
         this.FrontSealantReserviorVolumeTextPanel.ValueText = "#### mL";
         this.FrontSealantReserviorVolumeTextPanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // borderedPanel2
         // 
         this.borderedPanel2.BackColor = System.Drawing.Color.Olive;
         this.borderedPanel2.Controls.Add(this.label2);
         this.borderedPanel2.Controls.Add(this.lineControl3);
         this.borderedPanel2.Controls.Add(this.NitrogenPressure1TextPanel);
         this.borderedPanel2.Controls.Add(this.NitrogenPressure2TextPanel);
         this.borderedPanel2.Controls.Add(this.label20);
         this.borderedPanel2.Controls.Add(this.label1);
         this.borderedPanel2.EdgeWeight = 2;
         this.borderedPanel2.Location = new System.Drawing.Point(8, 8);
         this.borderedPanel2.Name = "borderedPanel2";
         this.borderedPanel2.Size = new System.Drawing.Size(119, 147);
         this.borderedPanel2.TabIndex = 177;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label2.Location = new System.Drawing.Point(12, 23);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(94, 15);
         this.label2.TabIndex = 171;
         this.label2.Text = "PRESSURE 1";
         this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // lineControl3
         // 
         this.lineControl3.BackColor = System.Drawing.Color.Transparent;
         this.lineControl3.EdgeColor = System.Drawing.Color.Black;
         this.lineControl3.LineType = NICBOT.GUI.LineControl.LineDrawType.Top;
         this.lineControl3.LineWeight = 1;
         this.lineControl3.Location = new System.Drawing.Point(2, 22);
         this.lineControl3.Name = "lineControl3";
         this.lineControl3.Opacity = 100;
         this.lineControl3.ShowBackground = false;
         this.lineControl3.ShowEdge = false;
         this.lineControl3.Size = new System.Drawing.Size(115, 8);
         this.lineControl3.TabIndex = 170;
         this.lineControl3.Text = "lineControl3";
         // 
         // NitrogenPressure1TextPanel
         // 
         this.NitrogenPressure1TextPanel.BackColor = System.Drawing.Color.Black;
         this.NitrogenPressure1TextPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.NitrogenPressure1TextPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.NitrogenPressure1TextPanel.ForeColor = System.Drawing.Color.White;
         this.NitrogenPressure1TextPanel.HoldTimeoutEnable = false;
         this.NitrogenPressure1TextPanel.HoldTimeoutInterval = 100;
         this.NitrogenPressure1TextPanel.Location = new System.Drawing.Point(10, 38);
         this.NitrogenPressure1TextPanel.Name = "NitrogenPressure1TextPanel";
         this.NitrogenPressure1TextPanel.Size = new System.Drawing.Size(99, 42);
         this.NitrogenPressure1TextPanel.TabIndex = 169;
         this.NitrogenPressure1TextPanel.ValueText = "### PSI";
         this.NitrogenPressure1TextPanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // NitrogenPressure2TextPanel
         // 
         this.NitrogenPressure2TextPanel.BackColor = System.Drawing.Color.Black;
         this.NitrogenPressure2TextPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.NitrogenPressure2TextPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.NitrogenPressure2TextPanel.ForeColor = System.Drawing.Color.White;
         this.NitrogenPressure2TextPanel.HoldTimeoutEnable = false;
         this.NitrogenPressure2TextPanel.HoldTimeoutInterval = 100;
         this.NitrogenPressure2TextPanel.Location = new System.Drawing.Point(10, 95);
         this.NitrogenPressure2TextPanel.Name = "NitrogenPressure2TextPanel";
         this.NitrogenPressure2TextPanel.Size = new System.Drawing.Size(99, 42);
         this.NitrogenPressure2TextPanel.TabIndex = 142;
         this.NitrogenPressure2TextPanel.ValueText = "### PSI";
         this.NitrogenPressure2TextPanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label20
         // 
         this.label20.BackColor = System.Drawing.Color.PaleGoldenrod;
         this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label20.Location = new System.Drawing.Point(2, 2);
         this.label20.Name = "label20";
         this.label20.Size = new System.Drawing.Size(115, 20);
         this.label20.TabIndex = 168;
         this.label20.Text = "NITROGEN";
         this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label1.Location = new System.Drawing.Point(12, 80);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(94, 15);
         this.label1.TabIndex = 141;
         this.label1.Text = "PRESSURE 2";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // ReelMainPanel
         // 
         this.ReelMainPanel.BackColor = System.Drawing.Color.Olive;
         this.ReelMainPanel.Controls.Add(this.ReelShowManualButton);
         this.ReelMainPanel.Controls.Add(this.ReelSetupButton);
         this.ReelMainPanel.Controls.Add(this.ReelResetTotalButton);
         this.ReelMainPanel.Controls.Add(this.ReelResetTripButton);
         this.ReelMainPanel.Controls.Add(this.ReelCalibrateToButton);
         this.ReelMainPanel.Controls.Add(this.ReelLockButton);
         this.ReelMainPanel.Controls.Add(this.ReelReverseButton);
         this.ReelMainPanel.Controls.Add(this.ReelOffButton);
         this.ReelMainPanel.Controls.Add(this.ReelTripTextPanel);
         this.ReelMainPanel.Controls.Add(this.label5);
         this.ReelMainPanel.Controls.Add(this.ReelTotalTextPanel);
         this.ReelMainPanel.Controls.Add(this.label4);
         this.ReelMainPanel.Controls.Add(this.label3);
         this.ReelMainPanel.Controls.Add(this.ReelActualValuePanel);
         this.ReelMainPanel.Location = new System.Drawing.Point(274, 575);
         this.ReelMainPanel.Name = "ReelMainPanel";
         this.ReelMainPanel.Size = new System.Drawing.Size(386, 374);
         this.ReelMainPanel.TabIndex = 42;
         // 
         // ReelShowManualButton
         // 
         this.ReelShowManualButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ReelShowManualButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ReelShowManualButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ReelShowManualButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.ReelShowManualButton.Location = new System.Drawing.Point(262, 276);
         this.ReelShowManualButton.Name = "ReelShowManualButton";
         this.ReelShowManualButton.Size = new System.Drawing.Size(107, 90);
         this.ReelShowManualButton.TabIndex = 163;
         this.ReelShowManualButton.Text = "SHOW MANUAL";
         this.ReelShowManualButton.UseVisualStyleBackColor = false;
         this.ReelShowManualButton.Click += new System.EventHandler(this.ReelShowManualButton_Click);
         // 
         // ReelSetupButton
         // 
         this.ReelSetupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ReelSetupButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ReelSetupButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ReelSetupButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.ReelSetupButton.ForeColor = System.Drawing.Color.Black;
         this.ReelSetupButton.HoldTimeoutEnable = true;
         this.ReelSetupButton.HoldTimeoutInterval = 100;
         this.ReelSetupButton.Location = new System.Drawing.Point(16, 276);
         this.ReelSetupButton.Name = "ReelSetupButton";
         this.ReelSetupButton.Size = new System.Drawing.Size(107, 90);
         this.ReelSetupButton.TabIndex = 156;
         this.ReelSetupButton.Text = "REEL SETUP";
         this.ReelSetupButton.UseVisualStyleBackColor = false;
         this.ReelSetupButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.ReelSetupButton_HoldTimeout);
         // 
         // ReelResetTotalButton
         // 
         this.ReelResetTotalButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ReelResetTotalButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ReelResetTotalButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ReelResetTotalButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.ReelResetTotalButton.HoldTimeoutEnable = true;
         this.ReelResetTotalButton.HoldTimeoutInterval = 100;
         this.ReelResetTotalButton.Location = new System.Drawing.Point(16, 178);
         this.ReelResetTotalButton.Name = "ReelResetTotalButton";
         this.ReelResetTotalButton.Size = new System.Drawing.Size(107, 90);
         this.ReelResetTotalButton.TabIndex = 155;
         this.ReelResetTotalButton.Text = "RESET TOTAL";
         this.ReelResetTotalButton.UseVisualStyleBackColor = false;
         this.ReelResetTotalButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.ReelResetTotalButton_HoldTimeout);
         // 
         // ReelResetTripButton
         // 
         this.ReelResetTripButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ReelResetTripButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ReelResetTripButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ReelResetTripButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.ReelResetTripButton.HoldTimeoutEnable = true;
         this.ReelResetTripButton.HoldTimeoutInterval = 100;
         this.ReelResetTripButton.Location = new System.Drawing.Point(262, 178);
         this.ReelResetTripButton.Name = "ReelResetTripButton";
         this.ReelResetTripButton.Size = new System.Drawing.Size(107, 90);
         this.ReelResetTripButton.TabIndex = 154;
         this.ReelResetTripButton.Text = "RESET   TRIP";
         this.ReelResetTripButton.UseVisualStyleBackColor = false;
         this.ReelResetTripButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.ReelResetTripButton_HoldTimeout);
         // 
         // ReelCalibrateToButton
         // 
         this.ReelCalibrateToButton.ArrowWidth = 0;
         this.ReelCalibrateToButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ReelCalibrateToButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ReelCalibrateToButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ReelCalibrateToButton.DisabledValueBackColor = System.Drawing.Color.Silver;
         this.ReelCalibrateToButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ReelCalibrateToButton.HoldTimeoutInterval = 100;
         this.ReelCalibrateToButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.ReelCalibrateToButton.LeftArrowVisible = false;
         this.ReelCalibrateToButton.Location = new System.Drawing.Point(139, 178);
         this.ReelCalibrateToButton.Name = "ReelCalibrateToButton";
         this.ReelCalibrateToButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.ReelCalibrateToButton.RightArrowVisible = false;
         this.ReelCalibrateToButton.Size = new System.Drawing.Size(107, 90);
         this.ReelCalibrateToButton.TabIndex = 153;
         this.ReelCalibrateToButton.Text = "CALIBRATE TO";
         this.ReelCalibrateToButton.UseVisualStyleBackColor = false;
         this.ReelCalibrateToButton.ValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.ReelCalibrateToButton.ValueEdgeHeight = 8;
         this.ReelCalibrateToButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.ReelCalibrateToButton.ValueForeColor = System.Drawing.Color.Silver;
         this.ReelCalibrateToButton.ValueHeight = 22;
         this.ReelCalibrateToButton.ValueText = "#### mm";
         this.ReelCalibrateToButton.ValueWidth = 80;
         this.ReelCalibrateToButton.HoldTimeout += new NICBOT.GUI.ValueButton.HoldTimeoutHandler(this.ReelCalibrateToButton_HoldTimeout);
         // 
         // ReelLockButton
         // 
         this.ReelLockButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ReelLockButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ReelLockButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ReelLockButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.ReelLockButton.HoldTimeoutEnable = true;
         this.ReelLockButton.HoldTimeoutInterval = 100;
         this.ReelLockButton.Location = new System.Drawing.Point(262, 80);
         this.ReelLockButton.Name = "ReelLockButton";
         this.ReelLockButton.Size = new System.Drawing.Size(107, 90);
         this.ReelLockButton.TabIndex = 152;
         this.ReelLockButton.Text = "LOCK (BRAKE)";
         this.ReelLockButton.UseVisualStyleBackColor = false;
         this.ReelLockButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.ReelLockButton_HoldTimeout);
         // 
         // ReelReverseButton
         // 
         this.ReelReverseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ReelReverseButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ReelReverseButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ReelReverseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ReelReverseButton.HoldTimeoutEnable = true;
         this.ReelReverseButton.HoldTimeoutInterval = 100;
         this.ReelReverseButton.Location = new System.Drawing.Point(139, 80);
         this.ReelReverseButton.Name = "ReelReverseButton";
         this.ReelReverseButton.Size = new System.Drawing.Size(107, 90);
         this.ReelReverseButton.TabIndex = 151;
         this.ReelReverseButton.Text = "REVERSE (ROLLBACK)";
         this.ReelReverseButton.UseVisualStyleBackColor = false;
         this.ReelReverseButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.ReelReverseButton_HoldTimeout);
         // 
         // ReelOffButton
         // 
         this.ReelOffButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ReelOffButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ReelOffButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ReelOffButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.ReelOffButton.HoldTimeoutEnable = true;
         this.ReelOffButton.HoldTimeoutInterval = 100;
         this.ReelOffButton.Location = new System.Drawing.Point(16, 80);
         this.ReelOffButton.Name = "ReelOffButton";
         this.ReelOffButton.Size = new System.Drawing.Size(107, 90);
         this.ReelOffButton.TabIndex = 150;
         this.ReelOffButton.Text = "OFF   (FREE)";
         this.ReelOffButton.UseVisualStyleBackColor = false;
         this.ReelOffButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.ReelOffButton_HoldTimeout);
         // 
         // ReelTripTextPanel
         // 
         this.ReelTripTextPanel.BackColor = System.Drawing.Color.Black;
         this.ReelTripTextPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.ReelTripTextPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ReelTripTextPanel.ForeColor = System.Drawing.Color.White;
         this.ReelTripTextPanel.HoldTimeoutEnable = true;
         this.ReelTripTextPanel.HoldTimeoutInterval = 100;
         this.ReelTripTextPanel.Location = new System.Drawing.Point(312, 30);
         this.ReelTripTextPanel.Name = "ReelTripTextPanel";
         this.ReelTripTextPanel.Size = new System.Drawing.Size(69, 42);
         this.ReelTripTextPanel.TabIndex = 149;
         this.ReelTripTextPanel.ValueText = "### m";
         this.ReelTripTextPanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.ReelTripTextPanel.HoldTimeout += new NICBOT.GUI.TextPanel.HoldTimeoutHandler2(this.ReelTripTextPanel_HoldTimeout);
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label5.Location = new System.Drawing.Point(327, 14);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(38, 15);
         this.label5.TabIndex = 148;
         this.label5.Text = "TRIP";
         this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // ReelTotalTextPanel
         // 
         this.ReelTotalTextPanel.BackColor = System.Drawing.Color.Black;
         this.ReelTotalTextPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.ReelTotalTextPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ReelTotalTextPanel.ForeColor = System.Drawing.Color.White;
         this.ReelTotalTextPanel.HoldTimeoutEnable = true;
         this.ReelTotalTextPanel.HoldTimeoutInterval = 100;
         this.ReelTotalTextPanel.Location = new System.Drawing.Point(8, 30);
         this.ReelTotalTextPanel.Name = "ReelTotalTextPanel";
         this.ReelTotalTextPanel.Size = new System.Drawing.Size(69, 42);
         this.ReelTotalTextPanel.TabIndex = 146;
         this.ReelTotalTextPanel.ValueText = "### m";
         this.ReelTotalTextPanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.ReelTotalTextPanel.HoldTimeout += new NICBOT.GUI.TextPanel.HoldTimeoutHandler2(this.ReelTotalTextPanel_HoldTimeout);
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label4.Location = new System.Drawing.Point(18, 14);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(49, 15);
         this.label4.TabIndex = 147;
         this.label4.Text = "TOTAL";
         this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label3
         // 
         this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.label3.Location = new System.Drawing.Point(64, 5);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(260, 23);
         this.label3.TabIndex = 145;
         this.label3.Text = "TETHER REEL";
         this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // ReelActualValuePanel
         // 
         this.ReelActualValuePanel.ActiveBackColor = System.Drawing.Color.Black;
         this.ReelActualValuePanel.ActiveFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.ReelActualValuePanel.ActiveForeColor = System.Drawing.Color.White;
         this.ReelActualValuePanel.ArrowWidth = 60;
         this.ReelActualValuePanel.Direction = NICBOT.GUI.DirectionalValuePanel.Directions.Idle;
         this.ReelActualValuePanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
         this.ReelActualValuePanel.ForeColor = System.Drawing.Color.Black;
         this.ReelActualValuePanel.IdleBackColor = System.Drawing.Color.Olive;
         this.ReelActualValuePanel.IdleFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
         this.ReelActualValuePanel.IdleForeColor = System.Drawing.Color.White;
         this.ReelActualValuePanel.LeftArrowText = "REV";
         this.ReelActualValuePanel.Location = new System.Drawing.Point(82, 30);
         this.ReelActualValuePanel.Name = "ReelActualValuePanel";
         this.ReelActualValuePanel.RightArrowText = "FWD";
         this.ReelActualValuePanel.Size = new System.Drawing.Size(219, 42);
         this.ReelActualValuePanel.TabIndex = 144;
         this.ReelActualValuePanel.Text = "directionalValuePanel1";
         this.ReelActualValuePanel.ValueBackColor = System.Drawing.Color.Black;
         this.ReelActualValuePanel.ValueForeColor = System.Drawing.Color.White;
         this.ReelActualValuePanel.ValueText = "#.# A";
         this.ReelActualValuePanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // ReelManualPanel
         // 
         this.ReelManualPanel.BackColor = System.Drawing.Color.Olive;
         this.ReelManualPanel.Controls.Add(this.ReelManualHideButton);
         this.ReelManualPanel.Controls.Add(this.ReelManualSetupButton);
         this.ReelManualPanel.Controls.Add(this.ReelManualResetTotalButton);
         this.ReelManualPanel.Controls.Add(this.ReelManualResetTripButton);
         this.ReelManualPanel.Controls.Add(this.ReelManualCalibrateToButton);
         this.ReelManualPanel.Controls.Add(this.ReelManualCurrentTextPanel);
         this.ReelManualPanel.Controls.Add(this.label6);
         this.ReelManualPanel.Controls.Add(this.ReelManualCurrentUpButton);
         this.ReelManualPanel.Controls.Add(this.ReelManualCurrentDownButton);
         this.ReelManualPanel.Controls.Add(this.ReelManualTorqueDirectionToggleButton);
         this.ReelManualPanel.Controls.Add(this.ReelManualOnOffToggleButton);
         this.ReelManualPanel.Location = new System.Drawing.Point(1938, 162);
         this.ReelManualPanel.Name = "ReelManualPanel";
         this.ReelManualPanel.Size = new System.Drawing.Size(386, 392);
         this.ReelManualPanel.TabIndex = 123;
         // 
         // ReelManualHideButton
         // 
         this.ReelManualHideButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ReelManualHideButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ReelManualHideButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ReelManualHideButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.ReelManualHideButton.Location = new System.Drawing.Point(262, 196);
         this.ReelManualHideButton.Name = "ReelManualHideButton";
         this.ReelManualHideButton.Size = new System.Drawing.Size(107, 90);
         this.ReelManualHideButton.TabIndex = 163;
         this.ReelManualHideButton.Text = "HIDE MANUAL";
         this.ReelManualHideButton.UseVisualStyleBackColor = false;
         this.ReelManualHideButton.Click += new System.EventHandler(this.ReelManualHideButton_Click);
         // 
         // ReelManualSetupButton
         // 
         this.ReelManualSetupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ReelManualSetupButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ReelManualSetupButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ReelManualSetupButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.ReelManualSetupButton.ForeColor = System.Drawing.Color.Black;
         this.ReelManualSetupButton.HoldTimeoutEnable = true;
         this.ReelManualSetupButton.HoldTimeoutInterval = 100;
         this.ReelManualSetupButton.Location = new System.Drawing.Point(16, 196);
         this.ReelManualSetupButton.Name = "ReelManualSetupButton";
         this.ReelManualSetupButton.Size = new System.Drawing.Size(107, 90);
         this.ReelManualSetupButton.TabIndex = 159;
         this.ReelManualSetupButton.Text = "REEL SETUP";
         this.ReelManualSetupButton.UseVisualStyleBackColor = false;
         this.ReelManualSetupButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.ReelManualSetupButton_HoldTimeout);
         // 
         // ReelManualResetTotalButton
         // 
         this.ReelManualResetTotalButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ReelManualResetTotalButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ReelManualResetTotalButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ReelManualResetTotalButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.ReelManualResetTotalButton.HoldTimeoutEnable = true;
         this.ReelManualResetTotalButton.HoldTimeoutInterval = 100;
         this.ReelManualResetTotalButton.Location = new System.Drawing.Point(16, 294);
         this.ReelManualResetTotalButton.Name = "ReelManualResetTotalButton";
         this.ReelManualResetTotalButton.Size = new System.Drawing.Size(107, 90);
         this.ReelManualResetTotalButton.TabIndex = 158;
         this.ReelManualResetTotalButton.Text = "RESET TOTAL";
         this.ReelManualResetTotalButton.UseVisualStyleBackColor = false;
         this.ReelManualResetTotalButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.ReelManualResetTotalButton_HoldTimeout);
         // 
         // ReelManualResetTripButton
         // 
         this.ReelManualResetTripButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ReelManualResetTripButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ReelManualResetTripButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ReelManualResetTripButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.ReelManualResetTripButton.HoldTimeoutEnable = true;
         this.ReelManualResetTripButton.HoldTimeoutInterval = 100;
         this.ReelManualResetTripButton.Location = new System.Drawing.Point(262, 294);
         this.ReelManualResetTripButton.Name = "ReelManualResetTripButton";
         this.ReelManualResetTripButton.Size = new System.Drawing.Size(107, 90);
         this.ReelManualResetTripButton.TabIndex = 157;
         this.ReelManualResetTripButton.Text = "RESET  TRIP";
         this.ReelManualResetTripButton.UseVisualStyleBackColor = false;
         this.ReelManualResetTripButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.ReelManualResetTripButton_HoldTimeout);
         // 
         // ReelManualCalibrateToButton
         // 
         this.ReelManualCalibrateToButton.ArrowWidth = 0;
         this.ReelManualCalibrateToButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ReelManualCalibrateToButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ReelManualCalibrateToButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ReelManualCalibrateToButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.ReelManualCalibrateToButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ReelManualCalibrateToButton.HoldTimeoutInterval = 100;
         this.ReelManualCalibrateToButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.ReelManualCalibrateToButton.LeftArrowVisible = false;
         this.ReelManualCalibrateToButton.Location = new System.Drawing.Point(139, 294);
         this.ReelManualCalibrateToButton.Name = "ReelManualCalibrateToButton";
         this.ReelManualCalibrateToButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.ReelManualCalibrateToButton.RightArrowVisible = false;
         this.ReelManualCalibrateToButton.Size = new System.Drawing.Size(107, 90);
         this.ReelManualCalibrateToButton.TabIndex = 156;
         this.ReelManualCalibrateToButton.Text = "CALIBRATE TO";
         this.ReelManualCalibrateToButton.UseVisualStyleBackColor = false;
         this.ReelManualCalibrateToButton.ValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.ReelManualCalibrateToButton.ValueEdgeHeight = 8;
         this.ReelManualCalibrateToButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.ReelManualCalibrateToButton.ValueForeColor = System.Drawing.Color.Silver;
         this.ReelManualCalibrateToButton.ValueHeight = 22;
         this.ReelManualCalibrateToButton.ValueText = "#### mm";
         this.ReelManualCalibrateToButton.ValueWidth = 80;
         this.ReelManualCalibrateToButton.HoldTimeout += new NICBOT.GUI.ValueButton.HoldTimeoutHandler(this.ReelManualCalibrateToButton_HoldTimeout);
         // 
         // ReelManualCurrentTextPanel
         // 
         this.ReelManualCurrentTextPanel.BackColor = System.Drawing.Color.Black;
         this.ReelManualCurrentTextPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.ReelManualCurrentTextPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ReelManualCurrentTextPanel.ForeColor = System.Drawing.Color.White;
         this.ReelManualCurrentTextPanel.HoldTimeoutEnable = true;
         this.ReelManualCurrentTextPanel.HoldTimeoutInterval = 100;
         this.ReelManualCurrentTextPanel.Location = new System.Drawing.Point(143, 56);
         this.ReelManualCurrentTextPanel.Name = "ReelManualCurrentTextPanel";
         this.ReelManualCurrentTextPanel.Size = new System.Drawing.Size(99, 42);
         this.ReelManualCurrentTextPanel.TabIndex = 129;
         this.ReelManualCurrentTextPanel.ValueText = "#.# A";
         this.ReelManualCurrentTextPanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.ReelManualCurrentTextPanel.HoldTimeout += new NICBOT.GUI.TextPanel.HoldTimeoutHandler2(this.ReelManualCurrentTextPanel_HoldTimeout);
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.label6.Location = new System.Drawing.Point(263, 69);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(105, 16);
         this.label6.TabIndex = 128;
         this.label6.Text = "SET CURRENT";
         // 
         // ReelManualCurrentUpButton
         // 
         this.ReelManualCurrentUpButton.ArrowColor = System.Drawing.Color.Black;
         this.ReelManualCurrentUpButton.ArrowHighlightColor = System.Drawing.Color.DarkGray;
         this.ReelManualCurrentUpButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ReelManualCurrentUpButton.DisabledArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.ReelManualCurrentUpButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ReelManualCurrentUpButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ReelManualCurrentUpButton.EdgeSpace = 8;
         this.ReelManualCurrentUpButton.ForeColor = System.Drawing.SystemColors.HighlightText;
         this.ReelManualCurrentUpButton.HighLightOffset = 7;
         this.ReelManualCurrentUpButton.HighlightVisible = true;
         this.ReelManualCurrentUpButton.HighLightWeight = 2;
         this.ReelManualCurrentUpButton.HoldRepeat = true;
         this.ReelManualCurrentUpButton.HoldRepeatInterval = 100;
         this.ReelManualCurrentUpButton.HoldTimeoutInterval = 500;
         this.ReelManualCurrentUpButton.Location = new System.Drawing.Point(262, 0);
         this.ReelManualCurrentUpButton.Name = "ReelManualCurrentUpButton";
         this.ReelManualCurrentUpButton.Size = new System.Drawing.Size(107, 67);
         this.ReelManualCurrentUpButton.TabIndex = 126;
         this.ReelManualCurrentUpButton.Text = "INCREASE";
         this.ReelManualCurrentUpButton.TextOffset = 0;
         this.ReelManualCurrentUpButton.TextVisible = false;
         this.ReelManualCurrentUpButton.UpDown = true;
         this.ReelManualCurrentUpButton.UseVisualStyleBackColor = false;
         this.ReelManualCurrentUpButton.HoldTimeout += new NICBOT.GUI.UpDownButton.HoldTimeoutHandler(this.ReelManualCurrentUpButton_HoldTimeout);
         this.ReelManualCurrentUpButton.Click += new System.EventHandler(this.ReelManualCurrentUpButton_Click);
         // 
         // ReelManualCurrentDownButton
         // 
         this.ReelManualCurrentDownButton.ArrowColor = System.Drawing.Color.Black;
         this.ReelManualCurrentDownButton.ArrowHighlightColor = System.Drawing.Color.DarkGray;
         this.ReelManualCurrentDownButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ReelManualCurrentDownButton.DisabledArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.ReelManualCurrentDownButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ReelManualCurrentDownButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ReelManualCurrentDownButton.EdgeSpace = 8;
         this.ReelManualCurrentDownButton.ForeColor = System.Drawing.SystemColors.HighlightText;
         this.ReelManualCurrentDownButton.HighLightOffset = 7;
         this.ReelManualCurrentDownButton.HighlightVisible = true;
         this.ReelManualCurrentDownButton.HighLightWeight = 2;
         this.ReelManualCurrentDownButton.HoldRepeat = true;
         this.ReelManualCurrentDownButton.HoldRepeatInterval = 100;
         this.ReelManualCurrentDownButton.HoldTimeoutInterval = 500;
         this.ReelManualCurrentDownButton.Location = new System.Drawing.Point(262, 88);
         this.ReelManualCurrentDownButton.Name = "ReelManualCurrentDownButton";
         this.ReelManualCurrentDownButton.Size = new System.Drawing.Size(107, 67);
         this.ReelManualCurrentDownButton.TabIndex = 127;
         this.ReelManualCurrentDownButton.Text = "DECREASE";
         this.ReelManualCurrentDownButton.TextOffset = 0;
         this.ReelManualCurrentDownButton.TextVisible = false;
         this.ReelManualCurrentDownButton.UpDown = false;
         this.ReelManualCurrentDownButton.UseVisualStyleBackColor = false;
         this.ReelManualCurrentDownButton.HoldTimeout += new NICBOT.GUI.UpDownButton.HoldTimeoutHandler(this.ReelManualCurrentDownButton_HoldTimeout);
         this.ReelManualCurrentDownButton.Click += new System.EventHandler(this.ReelManualCurrentDownButton_Click);
         // 
         // ReelManualTorqueDirectionToggleButton
         // 
         this.ReelManualTorqueDirectionToggleButton.AutomaticToggle = true;
         this.ReelManualTorqueDirectionToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ReelManualTorqueDirectionToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.ReelManualTorqueDirectionToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.ReelManualTorqueDirectionToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.ReelManualTorqueDirectionToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ReelManualTorqueDirectionToggleButton.HoldEnable = false;
         this.ReelManualTorqueDirectionToggleButton.HoldTimeoutInterval = 0;
         this.ReelManualTorqueDirectionToggleButton.Location = new System.Drawing.Point(16, 98);
         this.ReelManualTorqueDirectionToggleButton.Name = "ReelManualTorqueDirectionToggleButton";
         this.ReelManualTorqueDirectionToggleButton.OptionASelected = true;
         this.ReelManualTorqueDirectionToggleButton.OptionAText = "FWD";
         this.ReelManualTorqueDirectionToggleButton.OptionBSelected = false;
         this.ReelManualTorqueDirectionToggleButton.OptionBText = "REV";
         this.ReelManualTorqueDirectionToggleButton.OptionCenterWidth = 2;
         this.ReelManualTorqueDirectionToggleButton.OptionEdgeHeight = 8;
         this.ReelManualTorqueDirectionToggleButton.OptionHeight = 22;
         this.ReelManualTorqueDirectionToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.ReelManualTorqueDirectionToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
         this.ReelManualTorqueDirectionToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.ReelManualTorqueDirectionToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.ReelManualTorqueDirectionToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ReelManualTorqueDirectionToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.ReelManualTorqueDirectionToggleButton.OptionWidth = 45;
         this.ReelManualTorqueDirectionToggleButton.Size = new System.Drawing.Size(107, 90);
         this.ReelManualTorqueDirectionToggleButton.TabIndex = 125;
         this.ReelManualTorqueDirectionToggleButton.Text = "TORQUE DIRECTION";
         this.ReelManualTorqueDirectionToggleButton.UseVisualStyleBackColor = false;
         this.ReelManualTorqueDirectionToggleButton.Click += new System.EventHandler(this.ReelManualTorqueDirectionToggleButton_Click);
         // 
         // ReelManualOnOffToggleButton
         // 
         this.ReelManualOnOffToggleButton.AutomaticToggle = true;
         this.ReelManualOnOffToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.ReelManualOnOffToggleButton.DisabledBackColor = System.Drawing.Color.Black;
         this.ReelManualOnOffToggleButton.DisabledForeColor = System.Drawing.Color.Gray;
         this.ReelManualOnOffToggleButton.DisabledOptionBackColor = System.Drawing.Color.Black;
         this.ReelManualOnOffToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ReelManualOnOffToggleButton.HoldEnable = true;
         this.ReelManualOnOffToggleButton.HoldTimeoutInterval = 100;
         this.ReelManualOnOffToggleButton.Location = new System.Drawing.Point(16, 0);
         this.ReelManualOnOffToggleButton.Name = "ReelManualOnOffToggleButton";
         this.ReelManualOnOffToggleButton.OptionASelected = false;
         this.ReelManualOnOffToggleButton.OptionAText = "ON";
         this.ReelManualOnOffToggleButton.OptionBSelected = true;
         this.ReelManualOnOffToggleButton.OptionBText = "OFF";
         this.ReelManualOnOffToggleButton.OptionCenterWidth = 2;
         this.ReelManualOnOffToggleButton.OptionEdgeHeight = 8;
         this.ReelManualOnOffToggleButton.OptionHeight = 22;
         this.ReelManualOnOffToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.ReelManualOnOffToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
         this.ReelManualOnOffToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.ReelManualOnOffToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.ReelManualOnOffToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ReelManualOnOffToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.ReelManualOnOffToggleButton.OptionWidth = 45;
         this.ReelManualOnOffToggleButton.Size = new System.Drawing.Size(107, 90);
         this.ReelManualOnOffToggleButton.TabIndex = 122;
         this.ReelManualOnOffToggleButton.Text = "MANUAL REEL";
         this.ReelManualOnOffToggleButton.UseVisualStyleBackColor = false;
         this.ReelManualOnOffToggleButton.HoldTimeout += new NICBOT.GUI.ValueToggleButton.HoldTimeoutHandler(this.ReelManualOnOffToggleButton_HoldTimeout);
         this.ReelManualOnOffToggleButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ReelManualOnOffToggleButton_MouseClick);
         // 
         // GuidePanel
         // 
         this.GuidePanel.BackColor = System.Drawing.Color.Olive;
         this.GuidePanel.Controls.Add(this.label7);
         this.GuidePanel.Controls.Add(this.GuideSetupButton);
         this.GuidePanel.Controls.Add(this.GuideExtendRightButton);
         this.GuidePanel.Controls.Add(this.GuideRetractRightButton);
         this.GuidePanel.Controls.Add(this.GuideExtendLeftButton);
         this.GuidePanel.Controls.Add(this.GuideRetractLeftButton);
         this.GuidePanel.Location = new System.Drawing.Point(2, 645);
         this.GuidePanel.Name = "GuidePanel";
         this.GuidePanel.Size = new System.Drawing.Size(262, 324);
         this.GuidePanel.TabIndex = 124;
         // 
         // label7
         // 
         this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.label7.Location = new System.Drawing.Point(45, 4);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(174, 23);
         this.label7.TabIndex = 138;
         this.label7.Text = "TETHER GUIDES";
         this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // GuideSetupButton
         // 
         this.GuideSetupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.GuideSetupButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.GuideSetupButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.GuideSetupButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.GuideSetupButton.ForeColor = System.Drawing.Color.Black;
         this.GuideSetupButton.HoldTimeoutEnable = true;
         this.GuideSetupButton.HoldTimeoutInterval = 100;
         this.GuideSetupButton.Location = new System.Drawing.Point(16, 226);
         this.GuideSetupButton.Name = "GuideSetupButton";
         this.GuideSetupButton.Size = new System.Drawing.Size(107, 90);
         this.GuideSetupButton.TabIndex = 127;
         this.GuideSetupButton.Text = "GUIDE SETUP";
         this.GuideSetupButton.UseVisualStyleBackColor = false;
         this.GuideSetupButton.HoldTimeout += new NICBOT.GUI.HoldTimeoutHandler(this.GuideSetupButton_HoldTimeout);
         // 
         // GuideExtendRightButton
         // 
         this.GuideExtendRightButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.GuideExtendRightButton.Camera = NICBOT.GUI.CameraLocations.robotFrontUpperBack;
         this.GuideExtendRightButton.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.GuideExtendRightButton.CenterForeColor = System.Drawing.Color.White;
         this.GuideExtendRightButton.CenterLevel = 75;
         this.GuideExtendRightButton.CenterVisible = false;
         this.GuideExtendRightButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.GuideExtendRightButton.HoldRepeat = false;
         this.GuideExtendRightButton.HoldRepeatInterval = 0;
         this.GuideExtendRightButton.HoldTimeoutEnable = true;
         this.GuideExtendRightButton.HoldTimeoutInterval = 100;
         this.GuideExtendRightButton.IndicatorBetweenSpace = 4;
         this.GuideExtendRightButton.IndicatorEdgeSpace = 4;
         this.GuideExtendRightButton.LeftColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
         this.GuideExtendRightButton.LeftVisible = false;
         this.GuideExtendRightButton.Location = new System.Drawing.Point(139, 128);
         this.GuideExtendRightButton.Name = "GuideExtendRightButton";
         this.GuideExtendRightButton.RightColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
         this.GuideExtendRightButton.RightVisible = true;
         this.GuideExtendRightButton.Size = new System.Drawing.Size(107, 90);
         this.GuideExtendRightButton.TabIndex = 36;
         this.GuideExtendRightButton.Text = "EXTEND RIGHT";
         this.GuideExtendRightButton.UseVisualStyleBackColor = false;
         this.GuideExtendRightButton.HoldTimeout += new NICBOT.GUI.CameraSelectButton.HoldTimeoutHandler(this.GuideExtendRightButton_HoldTimeout);
         this.GuideExtendRightButton.Click += new System.EventHandler(this.GuideExtendRightButton_Click);
         // 
         // GuideRetractRightButton
         // 
         this.GuideRetractRightButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.GuideRetractRightButton.Camera = NICBOT.GUI.CameraLocations.robotFrontUpperBack;
         this.GuideRetractRightButton.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.GuideRetractRightButton.CenterForeColor = System.Drawing.Color.White;
         this.GuideRetractRightButton.CenterLevel = 75;
         this.GuideRetractRightButton.CenterVisible = false;
         this.GuideRetractRightButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.GuideRetractRightButton.HoldRepeat = false;
         this.GuideRetractRightButton.HoldRepeatInterval = 0;
         this.GuideRetractRightButton.HoldTimeoutEnable = true;
         this.GuideRetractRightButton.HoldTimeoutInterval = 100;
         this.GuideRetractRightButton.IndicatorBetweenSpace = 4;
         this.GuideRetractRightButton.IndicatorEdgeSpace = 4;
         this.GuideRetractRightButton.LeftColor = System.Drawing.Color.Yellow;
         this.GuideRetractRightButton.LeftVisible = false;
         this.GuideRetractRightButton.Location = new System.Drawing.Point(139, 30);
         this.GuideRetractRightButton.Name = "GuideRetractRightButton";
         this.GuideRetractRightButton.RightColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
         this.GuideRetractRightButton.RightVisible = true;
         this.GuideRetractRightButton.Size = new System.Drawing.Size(107, 90);
         this.GuideRetractRightButton.TabIndex = 35;
         this.GuideRetractRightButton.Text = "RETRACT RIGHT";
         this.GuideRetractRightButton.UseVisualStyleBackColor = false;
         this.GuideRetractRightButton.HoldTimeout += new NICBOT.GUI.CameraSelectButton.HoldTimeoutHandler(this.GuideRetractRightButton_HoldTimeout);
         this.GuideRetractRightButton.Click += new System.EventHandler(this.GuideRetractRightButton_Click);
         // 
         // GuideExtendLeftButton
         // 
         this.GuideExtendLeftButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.GuideExtendLeftButton.Camera = NICBOT.GUI.CameraLocations.robotFrontUpperBack;
         this.GuideExtendLeftButton.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.GuideExtendLeftButton.CenterForeColor = System.Drawing.Color.White;
         this.GuideExtendLeftButton.CenterLevel = 75;
         this.GuideExtendLeftButton.CenterVisible = false;
         this.GuideExtendLeftButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.GuideExtendLeftButton.HoldRepeat = false;
         this.GuideExtendLeftButton.HoldRepeatInterval = 0;
         this.GuideExtendLeftButton.HoldTimeoutEnable = true;
         this.GuideExtendLeftButton.HoldTimeoutInterval = 100;
         this.GuideExtendLeftButton.IndicatorBetweenSpace = 4;
         this.GuideExtendLeftButton.IndicatorEdgeSpace = 4;
         this.GuideExtendLeftButton.LeftColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
         this.GuideExtendLeftButton.LeftVisible = true;
         this.GuideExtendLeftButton.Location = new System.Drawing.Point(16, 128);
         this.GuideExtendLeftButton.Name = "GuideExtendLeftButton";
         this.GuideExtendLeftButton.RightColor = System.Drawing.Color.DarkBlue;
         this.GuideExtendLeftButton.RightVisible = false;
         this.GuideExtendLeftButton.Size = new System.Drawing.Size(107, 90);
         this.GuideExtendLeftButton.TabIndex = 34;
         this.GuideExtendLeftButton.Text = "EXTEND LEFT";
         this.GuideExtendLeftButton.UseVisualStyleBackColor = false;
         this.GuideExtendLeftButton.HoldTimeout += new NICBOT.GUI.CameraSelectButton.HoldTimeoutHandler(this.GuideExtendLeftButton_HoldTimeout);
         this.GuideExtendLeftButton.Click += new System.EventHandler(this.GuideExtendLeftButton_Click);
         // 
         // GuideRetractLeftButton
         // 
         this.GuideRetractLeftButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.GuideRetractLeftButton.Camera = NICBOT.GUI.CameraLocations.robotFrontUpperBack;
         this.GuideRetractLeftButton.CenterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.GuideRetractLeftButton.CenterForeColor = System.Drawing.Color.White;
         this.GuideRetractLeftButton.CenterLevel = 75;
         this.GuideRetractLeftButton.CenterVisible = false;
         this.GuideRetractLeftButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.GuideRetractLeftButton.HoldRepeat = false;
         this.GuideRetractLeftButton.HoldRepeatInterval = 0;
         this.GuideRetractLeftButton.HoldTimeoutEnable = true;
         this.GuideRetractLeftButton.HoldTimeoutInterval = 100;
         this.GuideRetractLeftButton.IndicatorBetweenSpace = 4;
         this.GuideRetractLeftButton.IndicatorEdgeSpace = 4;
         this.GuideRetractLeftButton.LeftColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(255)))), ((int)(((byte)(50)))));
         this.GuideRetractLeftButton.LeftVisible = true;
         this.GuideRetractLeftButton.Location = new System.Drawing.Point(16, 30);
         this.GuideRetractLeftButton.Name = "GuideRetractLeftButton";
         this.GuideRetractLeftButton.RightColor = System.Drawing.Color.DarkBlue;
         this.GuideRetractLeftButton.RightVisible = false;
         this.GuideRetractLeftButton.Size = new System.Drawing.Size(107, 90);
         this.GuideRetractLeftButton.TabIndex = 33;
         this.GuideRetractLeftButton.Text = "RETRACT LEFT";
         this.GuideRetractLeftButton.UseVisualStyleBackColor = false;
         this.GuideRetractLeftButton.HoldTimeout += new NICBOT.GUI.CameraSelectButton.HoldTimeoutHandler(this.GuideRetractLeftButton_HoldTimeout);
         this.GuideRetractLeftButton.Click += new System.EventHandler(this.GuideRetractLeftButton_Click);
         // 
         // SealantManualPanel
         // 
         this.SealantManualPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.SealantManualPanel.Controls.Add(this.SealantRelievePressureButton);
         this.SealantManualPanel.Controls.Add(this.SealantDirectionToggleButton);
         this.SealantManualPanel.Controls.Add(this.SealantPressureDecreaseButton);
         this.SealantManualPanel.Controls.Add(this.SealantPressureIncreaseButton);
         this.SealantManualPanel.Controls.Add(this.SealantNozzleToggleButton);
         this.SealantManualPanel.Controls.Add(this.SealantManualModeToggleButton);
         this.SealantManualPanel.Controls.Add(this.SealantManualPumpToggleButton);
         this.SealantManualPanel.Controls.Add(this.SealantSpeedIncreaseButton);
         this.SealantManualPanel.Controls.Add(this.label8);
         this.SealantManualPanel.Controls.Add(this.label9);
         this.SealantManualPanel.Controls.Add(this.SealantSpeedDecreaseButton);
         this.SealantManualPanel.Location = new System.Drawing.Point(2349, 578);
         this.SealantManualPanel.Name = "SealantManualPanel";
         this.SealantManualPanel.Size = new System.Drawing.Size(369, 269);
         this.SealantManualPanel.TabIndex = 126;
         // 
         // SealantRelievePressureButton
         // 
         this.SealantRelievePressureButton.ArrowWidth = 0;
         this.SealantRelievePressureButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SealantRelievePressureButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.SealantRelievePressureButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.SealantRelievePressureButton.DisabledValueBackColor = System.Drawing.Color.Silver;
         this.SealantRelievePressureButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.SealantRelievePressureButton.HoldTimeoutInterval = 100;
         this.SealantRelievePressureButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.SealantRelievePressureButton.LeftArrowVisible = false;
         this.SealantRelievePressureButton.Location = new System.Drawing.Point(131, 8);
         this.SealantRelievePressureButton.Name = "SealantRelievePressureButton";
         this.SealantRelievePressureButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.SealantRelievePressureButton.RightArrowVisible = false;
         this.SealantRelievePressureButton.Size = new System.Drawing.Size(107, 90);
         this.SealantRelievePressureButton.TabIndex = 154;
         this.SealantRelievePressureButton.Text = "RELIEVE PRESSURE";
         this.SealantRelievePressureButton.UseVisualStyleBackColor = false;
         this.SealantRelievePressureButton.ValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.SealantRelievePressureButton.ValueEdgeHeight = 8;
         this.SealantRelievePressureButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.SealantRelievePressureButton.ValueForeColor = System.Drawing.Color.Silver;
         this.SealantRelievePressureButton.ValueHeight = 22;
         this.SealantRelievePressureButton.ValueText = "#### mm";
         this.SealantRelievePressureButton.ValueWidth = 80;
         this.SealantRelievePressureButton.HoldTimeout += new NICBOT.GUI.ValueButton.HoldTimeoutHandler(this.SealantRelievePressureButton_HoldTimeout);
         // 
         // SealantDirectionToggleButton
         // 
         this.SealantDirectionToggleButton.AutomaticToggle = true;
         this.SealantDirectionToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SealantDirectionToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.SealantDirectionToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.SealantDirectionToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.SealantDirectionToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.SealantDirectionToggleButton.HoldEnable = false;
         this.SealantDirectionToggleButton.HoldTimeoutInterval = 0;
         this.SealantDirectionToggleButton.Location = new System.Drawing.Point(131, 188);
         this.SealantDirectionToggleButton.Name = "SealantDirectionToggleButton";
         this.SealantDirectionToggleButton.OptionASelected = true;
         this.SealantDirectionToggleButton.OptionAText = "FWD";
         this.SealantDirectionToggleButton.OptionBSelected = false;
         this.SealantDirectionToggleButton.OptionBText = "REV";
         this.SealantDirectionToggleButton.OptionCenterWidth = 2;
         this.SealantDirectionToggleButton.OptionEdgeHeight = 8;
         this.SealantDirectionToggleButton.OptionHeight = 22;
         this.SealantDirectionToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.SealantDirectionToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
         this.SealantDirectionToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.SealantDirectionToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.SealantDirectionToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SealantDirectionToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.SealantDirectionToggleButton.OptionWidth = 45;
         this.SealantDirectionToggleButton.Size = new System.Drawing.Size(107, 73);
         this.SealantDirectionToggleButton.TabIndex = 150;
         this.SealantDirectionToggleButton.Text = "SPEED DIRECTION";
         this.SealantDirectionToggleButton.UseVisualStyleBackColor = false;
         this.SealantDirectionToggleButton.Click += new System.EventHandler(this.SealantDirectionToggleButton_Click);
         // 
         // SealantPressureDecreaseButton
         // 
         this.SealantPressureDecreaseButton.ArrowColor = System.Drawing.Color.Black;
         this.SealantPressureDecreaseButton.ArrowHighlightColor = System.Drawing.Color.DarkGray;
         this.SealantPressureDecreaseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SealantPressureDecreaseButton.DisabledArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.SealantPressureDecreaseButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.SealantPressureDecreaseButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.SealantPressureDecreaseButton.EdgeSpace = 8;
         this.SealantPressureDecreaseButton.ForeColor = System.Drawing.SystemColors.HighlightText;
         this.SealantPressureDecreaseButton.HighLightOffset = 7;
         this.SealantPressureDecreaseButton.HighlightVisible = true;
         this.SealantPressureDecreaseButton.HighLightWeight = 2;
         this.SealantPressureDecreaseButton.HoldRepeat = true;
         this.SealantPressureDecreaseButton.HoldRepeatInterval = 100;
         this.SealantPressureDecreaseButton.HoldTimeoutInterval = 100;
         this.SealantPressureDecreaseButton.Location = new System.Drawing.Point(16, 194);
         this.SealantPressureDecreaseButton.Name = "SealantPressureDecreaseButton";
         this.SealantPressureDecreaseButton.Size = new System.Drawing.Size(107, 67);
         this.SealantPressureDecreaseButton.TabIndex = 149;
         this.SealantPressureDecreaseButton.Text = "DECREASE";
         this.SealantPressureDecreaseButton.TextOffset = 0;
         this.SealantPressureDecreaseButton.TextVisible = false;
         this.SealantPressureDecreaseButton.UpDown = false;
         this.SealantPressureDecreaseButton.UseVisualStyleBackColor = false;
         this.SealantPressureDecreaseButton.HoldTimeout += new NICBOT.GUI.UpDownButton.HoldTimeoutHandler(this.SealantPressureDecreaseButton_HoldTimeout);
         this.SealantPressureDecreaseButton.Click += new System.EventHandler(this.SealantPressureDecreaseButton_Click);
         // 
         // SealantPressureIncreaseButton
         // 
         this.SealantPressureIncreaseButton.ArrowColor = System.Drawing.Color.Black;
         this.SealantPressureIncreaseButton.ArrowHighlightColor = System.Drawing.Color.DarkGray;
         this.SealantPressureIncreaseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SealantPressureIncreaseButton.DisabledArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.SealantPressureIncreaseButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.SealantPressureIncreaseButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.SealantPressureIncreaseButton.EdgeSpace = 8;
         this.SealantPressureIncreaseButton.ForeColor = System.Drawing.SystemColors.HighlightText;
         this.SealantPressureIncreaseButton.HighLightOffset = 7;
         this.SealantPressureIncreaseButton.HighlightVisible = true;
         this.SealantPressureIncreaseButton.HighLightWeight = 2;
         this.SealantPressureIncreaseButton.HoldRepeat = true;
         this.SealantPressureIncreaseButton.HoldRepeatInterval = 100;
         this.SealantPressureIncreaseButton.HoldTimeoutInterval = 100;
         this.SealantPressureIncreaseButton.Location = new System.Drawing.Point(16, 106);
         this.SealantPressureIncreaseButton.Name = "SealantPressureIncreaseButton";
         this.SealantPressureIncreaseButton.Size = new System.Drawing.Size(107, 67);
         this.SealantPressureIncreaseButton.TabIndex = 148;
         this.SealantPressureIncreaseButton.Text = "INCREASE";
         this.SealantPressureIncreaseButton.TextOffset = 0;
         this.SealantPressureIncreaseButton.TextVisible = false;
         this.SealantPressureIncreaseButton.UpDown = true;
         this.SealantPressureIncreaseButton.UseVisualStyleBackColor = false;
         this.SealantPressureIncreaseButton.HoldTimeout += new NICBOT.GUI.UpDownButton.HoldTimeoutHandler(this.SealantPressureIncreaseButton_HoldTimeout);
         this.SealantPressureIncreaseButton.Click += new System.EventHandler(this.SealantPressureIncreaseButton_Click);
         // 
         // SealantNozzleToggleButton
         // 
         this.SealantNozzleToggleButton.AutomaticToggle = true;
         this.SealantNozzleToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SealantNozzleToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.SealantNozzleToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.SealantNozzleToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.SealantNozzleToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.SealantNozzleToggleButton.ForeColor = System.Drawing.Color.Black;
         this.SealantNozzleToggleButton.HoldEnable = true;
         this.SealantNozzleToggleButton.HoldTimeoutInterval = 100;
         this.SealantNozzleToggleButton.Location = new System.Drawing.Point(16, 8);
         this.SealantNozzleToggleButton.Name = "SealantNozzleToggleButton";
         this.SealantNozzleToggleButton.OptionASelected = false;
         this.SealantNozzleToggleButton.OptionAText = "EXTEND";
         this.SealantNozzleToggleButton.OptionBSelected = true;
         this.SealantNozzleToggleButton.OptionBText = "RETRACT";
         this.SealantNozzleToggleButton.OptionCenterWidth = 0;
         this.SealantNozzleToggleButton.OptionEdgeHeight = 8;
         this.SealantNozzleToggleButton.OptionHeight = 22;
         this.SealantNozzleToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.SealantNozzleToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SealantNozzleToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDarkDark;
         this.SealantNozzleToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.SealantNozzleToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SealantNozzleToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.SealantNozzleToggleButton.OptionWidth = 50;
         this.SealantNozzleToggleButton.Size = new System.Drawing.Size(107, 90);
         this.SealantNozzleToggleButton.TabIndex = 147;
         this.SealantNozzleToggleButton.Text = "NOZZLE";
         this.SealantNozzleToggleButton.UseVisualStyleBackColor = false;
         this.SealantNozzleToggleButton.HoldTimeout += new NICBOT.GUI.ValueToggleButton.HoldTimeoutHandler(this.SealantNozzleToggleButton_HoldTimeout);
         // 
         // SealantManualModeToggleButton
         // 
         this.SealantManualModeToggleButton.AutomaticToggle = true;
         this.SealantManualModeToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SealantManualModeToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.SealantManualModeToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.SealantManualModeToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.SealantManualModeToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.SealantManualModeToggleButton.HoldEnable = false;
         this.SealantManualModeToggleButton.HoldTimeoutInterval = 0;
         this.SealantManualModeToggleButton.Location = new System.Drawing.Point(131, 106);
         this.SealantManualModeToggleButton.Name = "SealantManualModeToggleButton";
         this.SealantManualModeToggleButton.OptionASelected = true;
         this.SealantManualModeToggleButton.OptionAText = "PSI";
         this.SealantManualModeToggleButton.OptionBSelected = false;
         this.SealantManualModeToggleButton.OptionBText = "SPEED";
         this.SealantManualModeToggleButton.OptionCenterWidth = 0;
         this.SealantManualModeToggleButton.OptionEdgeHeight = 8;
         this.SealantManualModeToggleButton.OptionHeight = 22;
         this.SealantManualModeToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.SealantManualModeToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SealantManualModeToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.SealantManualModeToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.SealantManualModeToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SealantManualModeToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.SealantManualModeToggleButton.OptionWidth = 50;
         this.SealantManualModeToggleButton.Size = new System.Drawing.Size(107, 73);
         this.SealantManualModeToggleButton.TabIndex = 124;
         this.SealantManualModeToggleButton.Text = "MANUAL MODE";
         this.SealantManualModeToggleButton.UseVisualStyleBackColor = false;
         this.SealantManualModeToggleButton.Click += new System.EventHandler(this.SealantManualModeToggleButton_Click);
         // 
         // SealantManualPumpToggleButton
         // 
         this.SealantManualPumpToggleButton.AutomaticToggle = true;
         this.SealantManualPumpToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SealantManualPumpToggleButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.SealantManualPumpToggleButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.SealantManualPumpToggleButton.DisabledOptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.SealantManualPumpToggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.SealantManualPumpToggleButton.HoldEnable = true;
         this.SealantManualPumpToggleButton.HoldTimeoutInterval = 100;
         this.SealantManualPumpToggleButton.Location = new System.Drawing.Point(246, 8);
         this.SealantManualPumpToggleButton.Name = "SealantManualPumpToggleButton";
         this.SealantManualPumpToggleButton.OptionASelected = true;
         this.SealantManualPumpToggleButton.OptionAText = "ON";
         this.SealantManualPumpToggleButton.OptionBSelected = false;
         this.SealantManualPumpToggleButton.OptionBText = "OFF";
         this.SealantManualPumpToggleButton.OptionCenterWidth = 2;
         this.SealantManualPumpToggleButton.OptionEdgeHeight = 8;
         this.SealantManualPumpToggleButton.OptionHeight = 22;
         this.SealantManualPumpToggleButton.OptionNonSelectedBackColor = System.Drawing.Color.Black;
         this.SealantManualPumpToggleButton.OptionNonSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
         this.SealantManualPumpToggleButton.OptionNonSelectedForeColor = System.Drawing.SystemColors.ControlDark;
         this.SealantManualPumpToggleButton.OptionSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
         this.SealantManualPumpToggleButton.OptionSelectedFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SealantManualPumpToggleButton.OptionSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.SealantManualPumpToggleButton.OptionWidth = 45;
         this.SealantManualPumpToggleButton.Size = new System.Drawing.Size(107, 90);
         this.SealantManualPumpToggleButton.TabIndex = 121;
         this.SealantManualPumpToggleButton.Text = "MANUAL PUMP";
         this.SealantManualPumpToggleButton.UseVisualStyleBackColor = false;
         this.SealantManualPumpToggleButton.HoldTimeout += new NICBOT.GUI.ValueToggleButton.HoldTimeoutHandler(this.SealantManualPumpToggleButton_HoldTimeout);
         // 
         // SealantSpeedIncreaseButton
         // 
         this.SealantSpeedIncreaseButton.ArrowColor = System.Drawing.Color.Black;
         this.SealantSpeedIncreaseButton.ArrowHighlightColor = System.Drawing.Color.DarkGray;
         this.SealantSpeedIncreaseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SealantSpeedIncreaseButton.DisabledArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.SealantSpeedIncreaseButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.SealantSpeedIncreaseButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.SealantSpeedIncreaseButton.EdgeSpace = 8;
         this.SealantSpeedIncreaseButton.ForeColor = System.Drawing.SystemColors.HighlightText;
         this.SealantSpeedIncreaseButton.HighLightOffset = 7;
         this.SealantSpeedIncreaseButton.HighlightVisible = true;
         this.SealantSpeedIncreaseButton.HighLightWeight = 2;
         this.SealantSpeedIncreaseButton.HoldRepeat = true;
         this.SealantSpeedIncreaseButton.HoldRepeatInterval = 100;
         this.SealantSpeedIncreaseButton.HoldTimeoutInterval = 100;
         this.SealantSpeedIncreaseButton.Location = new System.Drawing.Point(246, 106);
         this.SealantSpeedIncreaseButton.Name = "SealantSpeedIncreaseButton";
         this.SealantSpeedIncreaseButton.Size = new System.Drawing.Size(107, 67);
         this.SealantSpeedIncreaseButton.TabIndex = 75;
         this.SealantSpeedIncreaseButton.Text = "INCREASE";
         this.SealantSpeedIncreaseButton.TextOffset = 0;
         this.SealantSpeedIncreaseButton.TextVisible = false;
         this.SealantSpeedIncreaseButton.UpDown = true;
         this.SealantSpeedIncreaseButton.UseVisualStyleBackColor = false;
         this.SealantSpeedIncreaseButton.HoldTimeout += new NICBOT.GUI.UpDownButton.HoldTimeoutHandler(this.SealantSpeedIncreaseButton_HoldTimeout);
         this.SealantSpeedIncreaseButton.Click += new System.EventHandler(this.SealantSpeedIncreaseButton_Click);
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.label8.Location = new System.Drawing.Point(12, 175);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(113, 16);
         this.label8.TabIndex = 72;
         this.label8.Text = "SET PRESSURE";
         // 
         // label9
         // 
         this.label9.AutoSize = true;
         this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.label9.Location = new System.Drawing.Point(257, 175);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(84, 16);
         this.label9.TabIndex = 74;
         this.label9.Text = "SET SPEED";
         // 
         // SealantSpeedDecreaseButton
         // 
         this.SealantSpeedDecreaseButton.ArrowColor = System.Drawing.Color.Black;
         this.SealantSpeedDecreaseButton.ArrowHighlightColor = System.Drawing.Color.DarkGray;
         this.SealantSpeedDecreaseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SealantSpeedDecreaseButton.DisabledArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.SealantSpeedDecreaseButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.SealantSpeedDecreaseButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.SealantSpeedDecreaseButton.EdgeSpace = 8;
         this.SealantSpeedDecreaseButton.ForeColor = System.Drawing.SystemColors.HighlightText;
         this.SealantSpeedDecreaseButton.HighLightOffset = 7;
         this.SealantSpeedDecreaseButton.HighlightVisible = true;
         this.SealantSpeedDecreaseButton.HighLightWeight = 2;
         this.SealantSpeedDecreaseButton.HoldRepeat = true;
         this.SealantSpeedDecreaseButton.HoldRepeatInterval = 100;
         this.SealantSpeedDecreaseButton.HoldTimeoutInterval = 100;
         this.SealantSpeedDecreaseButton.Location = new System.Drawing.Point(246, 194);
         this.SealantSpeedDecreaseButton.Name = "SealantSpeedDecreaseButton";
         this.SealantSpeedDecreaseButton.Size = new System.Drawing.Size(107, 67);
         this.SealantSpeedDecreaseButton.TabIndex = 76;
         this.SealantSpeedDecreaseButton.Text = "DECREASE";
         this.SealantSpeedDecreaseButton.TextOffset = 0;
         this.SealantSpeedDecreaseButton.TextVisible = false;
         this.SealantSpeedDecreaseButton.UpDown = false;
         this.SealantSpeedDecreaseButton.UseVisualStyleBackColor = false;
         this.SealantSpeedDecreaseButton.HoldTimeout += new NICBOT.GUI.UpDownButton.HoldTimeoutHandler(this.SealantSpeedDecreaseButton_HoldTimeout);
         this.SealantSpeedDecreaseButton.Click += new System.EventHandler(this.SealantSpeedDecreaseButton_Click);
         // 
         // MovementManulPanel
         // 
         this.MovementManulPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(80)))), ((int)(((byte)(96)))));
         this.MovementManulPanel.Controls.Add(this.MotorManualJogForwardButton);
         this.MovementManulPanel.Controls.Add(this.MotorManualJogReverseButton);
         this.MovementManulPanel.Controls.Add(this.MotorManualMoveForwardButton);
         this.MovementManulPanel.Controls.Add(this.MotorManualMoveReverseButton);
         this.MovementManulPanel.Controls.Add(this.MotorManualMoveSpeedValueButton);
         this.MovementManulPanel.Controls.Add(this.MotorManualJogDistanceValueButton);
         this.MovementManulPanel.Location = new System.Drawing.Point(1938, 632);
         this.MovementManulPanel.Name = "MovementManulPanel";
         this.MovementManulPanel.Size = new System.Drawing.Size(386, 188);
         this.MovementManulPanel.TabIndex = 127;
         this.MovementManulPanel.Visible = false;
         // 
         // MotorManualJogForwardButton
         // 
         this.MotorManualJogForwardButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.MotorManualJogForwardButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.MotorManualJogForwardButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.MotorManualJogForwardButton.Enabled = false;
         this.MotorManualJogForwardButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.MotorManualJogForwardButton.Location = new System.Drawing.Point(262, 0);
         this.MotorManualJogForwardButton.Name = "MotorManualJogForwardButton";
         this.MotorManualJogForwardButton.Size = new System.Drawing.Size(107, 90);
         this.MotorManualJogForwardButton.TabIndex = 165;
         this.MotorManualJogForwardButton.Text = "JOG          CW";
         this.MotorManualJogForwardButton.UseVisualStyleBackColor = false;
         this.MotorManualJogForwardButton.Click += new System.EventHandler(this.MotorManualJogForwardButton_Click);
         // 
         // MotorManualJogReverseButton
         // 
         this.MotorManualJogReverseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.MotorManualJogReverseButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.MotorManualJogReverseButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.MotorManualJogReverseButton.Enabled = false;
         this.MotorManualJogReverseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.MotorManualJogReverseButton.Location = new System.Drawing.Point(16, 0);
         this.MotorManualJogReverseButton.Name = "MotorManualJogReverseButton";
         this.MotorManualJogReverseButton.Size = new System.Drawing.Size(107, 90);
         this.MotorManualJogReverseButton.TabIndex = 164;
         this.MotorManualJogReverseButton.Text = "JOG          CCW";
         this.MotorManualJogReverseButton.UseVisualStyleBackColor = false;
         this.MotorManualJogReverseButton.Click += new System.EventHandler(this.MotorManualJogReverseButton_Click);
         // 
         // MotorManualMoveForwardButton
         // 
         this.MotorManualMoveForwardButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.MotorManualMoveForwardButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.MotorManualMoveForwardButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.MotorManualMoveForwardButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.MotorManualMoveForwardButton.HoldTimeoutEnable = true;
         this.MotorManualMoveForwardButton.HoldTimeoutInterval = 100;
         this.MotorManualMoveForwardButton.Location = new System.Drawing.Point(262, 98);
         this.MotorManualMoveForwardButton.Name = "MotorManualMoveForwardButton";
         this.MotorManualMoveForwardButton.Size = new System.Drawing.Size(107, 90);
         this.MotorManualMoveForwardButton.TabIndex = 127;
         this.MotorManualMoveForwardButton.Text = "MANUAL MOVE        CW";
         this.MotorManualMoveForwardButton.UseVisualStyleBackColor = false;
         this.MotorManualMoveForwardButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MotorManualMoveForwardButton_MouseDown);
         this.MotorManualMoveForwardButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MotorManualMoveForwardButton_MouseUp);
         // 
         // MotorManualMoveReverseButton
         // 
         this.MotorManualMoveReverseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.MotorManualMoveReverseButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.MotorManualMoveReverseButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.MotorManualMoveReverseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.MotorManualMoveReverseButton.HoldTimeoutEnable = true;
         this.MotorManualMoveReverseButton.HoldTimeoutInterval = 100;
         this.MotorManualMoveReverseButton.Location = new System.Drawing.Point(16, 98);
         this.MotorManualMoveReverseButton.Name = "MotorManualMoveReverseButton";
         this.MotorManualMoveReverseButton.Size = new System.Drawing.Size(107, 90);
         this.MotorManualMoveReverseButton.TabIndex = 126;
         this.MotorManualMoveReverseButton.Text = "MANUAL MOVE     CCW";
         this.MotorManualMoveReverseButton.UseVisualStyleBackColor = false;
         this.MotorManualMoveReverseButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MotorManualMoveReverseButton_MouseDown);
         this.MotorManualMoveReverseButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MotorManualMoveReverseButton_MouseUp);
         // 
         // MotorManualMoveSpeedValueButton
         // 
         this.MotorManualMoveSpeedValueButton.ArrowWidth = 0;
         this.MotorManualMoveSpeedValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.MotorManualMoveSpeedValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.MotorManualMoveSpeedValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.MotorManualMoveSpeedValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.MotorManualMoveSpeedValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.MotorManualMoveSpeedValueButton.HoldTimeoutInterval = 100;
         this.MotorManualMoveSpeedValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.MotorManualMoveSpeedValueButton.LeftArrowVisible = false;
         this.MotorManualMoveSpeedValueButton.Location = new System.Drawing.Point(139, 98);
         this.MotorManualMoveSpeedValueButton.Name = "MotorManualMoveSpeedValueButton";
         this.MotorManualMoveSpeedValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.MotorManualMoveSpeedValueButton.RightArrowVisible = false;
         this.MotorManualMoveSpeedValueButton.Size = new System.Drawing.Size(107, 90);
         this.MotorManualMoveSpeedValueButton.TabIndex = 123;
         this.MotorManualMoveSpeedValueButton.Text = "MOVE SPEED";
         this.MotorManualMoveSpeedValueButton.UseVisualStyleBackColor = false;
         this.MotorManualMoveSpeedValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.MotorManualMoveSpeedValueButton.ValueEdgeHeight = 8;
         this.MotorManualMoveSpeedValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.MotorManualMoveSpeedValueButton.ValueForeColor = System.Drawing.Color.White;
         this.MotorManualMoveSpeedValueButton.ValueHeight = 22;
         this.MotorManualMoveSpeedValueButton.ValueText = "### mm/S";
         this.MotorManualMoveSpeedValueButton.ValueWidth = 80;
         this.MotorManualMoveSpeedValueButton.HoldTimeout += new NICBOT.GUI.ValueButton.HoldTimeoutHandler(this.MotorManualMoveSpeedValueButton_HoldTimeout);
         // 
         // MotorManualJogDistanceValueButton
         // 
         this.MotorManualJogDistanceValueButton.ArrowWidth = 0;
         this.MotorManualJogDistanceValueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.MotorManualJogDistanceValueButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.MotorManualJogDistanceValueButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.MotorManualJogDistanceValueButton.DisabledValueBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.MotorManualJogDistanceValueButton.Enabled = false;
         this.MotorManualJogDistanceValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.MotorManualJogDistanceValueButton.HoldTimeoutInterval = 100;
         this.MotorManualJogDistanceValueButton.LeftArrowBackColor = System.Drawing.Color.Black;
         this.MotorManualJogDistanceValueButton.LeftArrowVisible = false;
         this.MotorManualJogDistanceValueButton.Location = new System.Drawing.Point(139, 0);
         this.MotorManualJogDistanceValueButton.Name = "MotorManualJogDistanceValueButton";
         this.MotorManualJogDistanceValueButton.RightArrowBackColor = System.Drawing.Color.Black;
         this.MotorManualJogDistanceValueButton.RightArrowVisible = false;
         this.MotorManualJogDistanceValueButton.Size = new System.Drawing.Size(107, 90);
         this.MotorManualJogDistanceValueButton.TabIndex = 122;
         this.MotorManualJogDistanceValueButton.Text = "JOG DISTANCE";
         this.MotorManualJogDistanceValueButton.UseVisualStyleBackColor = false;
         this.MotorManualJogDistanceValueButton.ValueBackColor = System.Drawing.Color.Black;
         this.MotorManualJogDistanceValueButton.ValueEdgeHeight = 8;
         this.MotorManualJogDistanceValueButton.ValueFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.MotorManualJogDistanceValueButton.ValueForeColor = System.Drawing.Color.White;
         this.MotorManualJogDistanceValueButton.ValueHeight = 22;
         this.MotorManualJogDistanceValueButton.ValueText = "#### mm";
         this.MotorManualJogDistanceValueButton.ValueWidth = 80;
         this.MotorManualJogDistanceValueButton.HoldTimeout += new NICBOT.GUI.ValueButton.HoldTimeoutHandler(this.MotorManualJogDistanceValueButton_HoldTimeout);
         // 
         // InspectionPanel
         // 
         this.InspectionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.InspectionPanel.Controls.Add(this.borderedPanel6);
         this.InspectionPanel.Controls.Add(this.label38);
         this.InspectionPanel.Controls.Add(this.borderedPanel5);
         this.InspectionPanel.Location = new System.Drawing.Point(2724, 36);
         this.InspectionPanel.Name = "InspectionPanel";
         this.InspectionPanel.Size = new System.Drawing.Size(369, 427);
         this.InspectionPanel.TabIndex = 128;
         this.InspectionPanel.Visible = false;
         // 
         // borderedPanel6
         // 
         this.borderedPanel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
         this.borderedPanel6.Controls.Add(this.SensorPipePositionTextPanel);
         this.borderedPanel6.Controls.Add(this.label13);
         this.borderedPanel6.Controls.Add(this.SensorGpsTimeTextPanel);
         this.borderedPanel6.Controls.Add(this.label42);
         this.borderedPanel6.Controls.Add(this.label41);
         this.borderedPanel6.Controls.Add(this.SensorGpsDateTextPanel);
         this.borderedPanel6.Controls.Add(this.SensorDisplacementTextPanel);
         this.borderedPanel6.Controls.Add(this.label40);
         this.borderedPanel6.Controls.Add(this.SensorDirectionTextPanel);
         this.borderedPanel6.Controls.Add(this.label39);
         this.borderedPanel6.Controls.Add(this.SensorLongitudeTextPanel);
         this.borderedPanel6.Controls.Add(this.SensorLatitudeTextPanel);
         this.borderedPanel6.Controls.Add(this.label35);
         this.borderedPanel6.Controls.Add(this.label37);
         this.borderedPanel6.Controls.Add(this.rotatableLabel13);
         this.borderedPanel6.EdgeWeight = 1;
         this.borderedPanel6.Location = new System.Drawing.Point(8, 27);
         this.borderedPanel6.Name = "borderedPanel6";
         this.borderedPanel6.Size = new System.Drawing.Size(353, 226);
         this.borderedPanel6.TabIndex = 181;
         // 
         // SensorPipePositionTextPanel
         // 
         this.SensorPipePositionTextPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.SensorPipePositionTextPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.SensorPipePositionTextPanel.Enabled = false;
         this.SensorPipePositionTextPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SensorPipePositionTextPanel.ForeColor = System.Drawing.Color.Silver;
         this.SensorPipePositionTextPanel.HoldTimeoutEnable = false;
         this.SensorPipePositionTextPanel.HoldTimeoutInterval = 100;
         this.SensorPipePositionTextPanel.Location = new System.Drawing.Point(50, 168);
         this.SensorPipePositionTextPanel.Name = "SensorPipePositionTextPanel";
         this.SensorPipePositionTextPanel.Size = new System.Drawing.Size(99, 42);
         this.SensorPipePositionTextPanel.TabIndex = 190;
         this.SensorPipePositionTextPanel.ValueText = "#:##";
         this.SensorPipePositionTextPanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label13
         // 
         this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label13.Location = new System.Drawing.Point(42, 148);
         this.label13.Name = "label13";
         this.label13.Size = new System.Drawing.Size(109, 20);
         this.label13.TabIndex = 189;
         this.label13.Text = "PIPE POSITION";
         this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // SensorGpsTimeTextPanel
         // 
         this.SensorGpsTimeTextPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.SensorGpsTimeTextPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.SensorGpsTimeTextPanel.Enabled = false;
         this.SensorGpsTimeTextPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SensorGpsTimeTextPanel.ForeColor = System.Drawing.Color.Silver;
         this.SensorGpsTimeTextPanel.HoldTimeoutEnable = false;
         this.SensorGpsTimeTextPanel.HoldTimeoutInterval = 100;
         this.SensorGpsTimeTextPanel.Location = new System.Drawing.Point(238, 101);
         this.SensorGpsTimeTextPanel.Name = "SensorGpsTimeTextPanel";
         this.SensorGpsTimeTextPanel.Size = new System.Drawing.Size(99, 42);
         this.SensorGpsTimeTextPanel.TabIndex = 186;
         this.SensorGpsTimeTextPanel.ValueText = "##:##:##";
         this.SensorGpsTimeTextPanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label42
         // 
         this.label42.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label42.Location = new System.Drawing.Point(238, 78);
         this.label42.Name = "label42";
         this.label42.Size = new System.Drawing.Size(99, 20);
         this.label42.TabIndex = 185;
         this.label42.Text = "TIME";
         this.label42.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label41
         // 
         this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label41.Location = new System.Drawing.Point(238, 8);
         this.label41.Name = "label41";
         this.label41.Size = new System.Drawing.Size(99, 20);
         this.label41.TabIndex = 184;
         this.label41.Text = "DATE";
         this.label41.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // SensorGpsDateTextPanel
         // 
         this.SensorGpsDateTextPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.SensorGpsDateTextPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.SensorGpsDateTextPanel.Enabled = false;
         this.SensorGpsDateTextPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SensorGpsDateTextPanel.ForeColor = System.Drawing.Color.Silver;
         this.SensorGpsDateTextPanel.HoldTimeoutEnable = false;
         this.SensorGpsDateTextPanel.HoldTimeoutInterval = 100;
         this.SensorGpsDateTextPanel.Location = new System.Drawing.Point(238, 31);
         this.SensorGpsDateTextPanel.Name = "SensorGpsDateTextPanel";
         this.SensorGpsDateTextPanel.Size = new System.Drawing.Size(99, 42);
         this.SensorGpsDateTextPanel.TabIndex = 183;
         this.SensorGpsDateTextPanel.ValueText = "08-26-2015";
         this.SensorGpsDateTextPanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // SensorDisplacementTextPanel
         // 
         this.SensorDisplacementTextPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.SensorDisplacementTextPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.SensorDisplacementTextPanel.Enabled = false;
         this.SensorDisplacementTextPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SensorDisplacementTextPanel.ForeColor = System.Drawing.Color.Silver;
         this.SensorDisplacementTextPanel.HoldTimeoutEnable = false;
         this.SensorDisplacementTextPanel.HoldTimeoutInterval = 100;
         this.SensorDisplacementTextPanel.Location = new System.Drawing.Point(125, 101);
         this.SensorDisplacementTextPanel.Name = "SensorDisplacementTextPanel";
         this.SensorDisplacementTextPanel.Size = new System.Drawing.Size(99, 42);
         this.SensorDisplacementTextPanel.TabIndex = 181;
         this.SensorDisplacementTextPanel.ValueText = "##### cm";
         this.SensorDisplacementTextPanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label40
         // 
         this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label40.Location = new System.Drawing.Point(119, 78);
         this.label40.Name = "label40";
         this.label40.Size = new System.Drawing.Size(111, 20);
         this.label40.TabIndex = 182;
         this.label40.Text = "DISPLACEMENT";
         this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // SensorDirectionTextPanel
         // 
         this.SensorDirectionTextPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.SensorDirectionTextPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.SensorDirectionTextPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SensorDirectionTextPanel.ForeColor = System.Drawing.Color.Silver;
         this.SensorDirectionTextPanel.HoldTimeoutEnable = true;
         this.SensorDirectionTextPanel.HoldTimeoutInterval = 100;
         this.SensorDirectionTextPanel.Location = new System.Drawing.Point(125, 31);
         this.SensorDirectionTextPanel.Name = "SensorDirectionTextPanel";
         this.SensorDirectionTextPanel.Size = new System.Drawing.Size(99, 42);
         this.SensorDirectionTextPanel.TabIndex = 179;
         this.SensorDirectionTextPanel.ValueText = "NORTH";
         this.SensorDirectionTextPanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.SensorDirectionTextPanel.HoldTimeout += new NICBOT.GUI.TextPanel.HoldTimeoutHandler2(this.SensorDirectionTextPanel_HoldTimeout);
         // 
         // label39
         // 
         this.label39.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label39.Location = new System.Drawing.Point(125, 8);
         this.label39.Name = "label39";
         this.label39.Size = new System.Drawing.Size(99, 20);
         this.label39.TabIndex = 180;
         this.label39.Text = "DIRECTION";
         this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // SensorLongitudeTextPanel
         // 
         this.SensorLongitudeTextPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.SensorLongitudeTextPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.SensorLongitudeTextPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SensorLongitudeTextPanel.ForeColor = System.Drawing.Color.Silver;
         this.SensorLongitudeTextPanel.HoldTimeoutEnable = true;
         this.SensorLongitudeTextPanel.HoldTimeoutInterval = 100;
         this.SensorLongitudeTextPanel.Location = new System.Drawing.Point(12, 101);
         this.SensorLongitudeTextPanel.Name = "SensorLongitudeTextPanel";
         this.SensorLongitudeTextPanel.Size = new System.Drawing.Size(99, 42);
         this.SensorLongitudeTextPanel.TabIndex = 176;
         this.SensorLongitudeTextPanel.ValueText = "### °";
         this.SensorLongitudeTextPanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.SensorLongitudeTextPanel.HoldTimeout += new NICBOT.GUI.TextPanel.HoldTimeoutHandler2(this.SensorLongitudeTextPanel_HoldTimeout);
         // 
         // SensorLatitudeTextPanel
         // 
         this.SensorLatitudeTextPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.SensorLatitudeTextPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.SensorLatitudeTextPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SensorLatitudeTextPanel.ForeColor = System.Drawing.Color.Silver;
         this.SensorLatitudeTextPanel.HoldTimeoutEnable = true;
         this.SensorLatitudeTextPanel.HoldTimeoutInterval = 100;
         this.SensorLatitudeTextPanel.Location = new System.Drawing.Point(12, 31);
         this.SensorLatitudeTextPanel.Name = "SensorLatitudeTextPanel";
         this.SensorLatitudeTextPanel.Size = new System.Drawing.Size(99, 42);
         this.SensorLatitudeTextPanel.TabIndex = 175;
         this.SensorLatitudeTextPanel.ValueText = "### °";
         this.SensorLatitudeTextPanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.SensorLatitudeTextPanel.HoldTimeout += new NICBOT.GUI.TextPanel.HoldTimeoutHandler2(this.SensorLatitudeTextPanel_HoldTimeout);
         // 
         // label35
         // 
         this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label35.Location = new System.Drawing.Point(12, 78);
         this.label35.Name = "label35";
         this.label35.Size = new System.Drawing.Size(99, 20);
         this.label35.TabIndex = 177;
         this.label35.Text = "LONGITUDE";
         this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label37
         // 
         this.label37.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label37.Location = new System.Drawing.Point(12, 8);
         this.label37.Name = "label37";
         this.label37.Size = new System.Drawing.Size(99, 20);
         this.label37.TabIndex = 178;
         this.label37.Text = "LATITUDE";
         this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // rotatableLabel13
         // 
         this.rotatableLabel13.Angle = 90;
         this.rotatableLabel13.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.rotatableLabel13.Location = new System.Drawing.Point(19, 158);
         this.rotatableLabel13.Name = "rotatableLabel13";
         this.rotatableLabel13.Size = new System.Drawing.Size(47, 60);
         this.rotatableLabel13.TabIndex = 191;
         this.rotatableLabel13.Tag = "a";
         this.rotatableLabel13.Text = "ACTUAL";
         this.rotatableLabel13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label38
         // 
         this.label38.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.label38.Location = new System.Drawing.Point(49, 4);
         this.label38.Name = "label38";
         this.label38.Size = new System.Drawing.Size(260, 23);
         this.label38.TabIndex = 180;
         this.label38.Text = "INSPECTION SENSOR";
         this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // borderedPanel5
         // 
         this.borderedPanel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
         this.borderedPanel5.Controls.Add(this.SensorStressReadingTextPanel);
         this.borderedPanel5.Controls.Add(this.SensorThicknessAcquireButton);
         this.borderedPanel5.Controls.Add(this.SensorStressAcquireButton);
         this.borderedPanel5.Controls.Add(this.SensorThicknessReadingTextPanel);
         this.borderedPanel5.EdgeWeight = 1;
         this.borderedPanel5.Location = new System.Drawing.Point(8, 261);
         this.borderedPanel5.Name = "borderedPanel5";
         this.borderedPanel5.Size = new System.Drawing.Size(353, 158);
         this.borderedPanel5.TabIndex = 179;
         // 
         // SensorStressReadingTextPanel
         // 
         this.SensorStressReadingTextPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.SensorStressReadingTextPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.SensorStressReadingTextPanel.Enabled = false;
         this.SensorStressReadingTextPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SensorStressReadingTextPanel.ForeColor = System.Drawing.Color.Silver;
         this.SensorStressReadingTextPanel.HoldTimeoutEnable = false;
         this.SensorStressReadingTextPanel.HoldTimeoutInterval = 100;
         this.SensorStressReadingTextPanel.Location = new System.Drawing.Point(199, 95);
         this.SensorStressReadingTextPanel.Name = "SensorStressReadingTextPanel";
         this.SensorStressReadingTextPanel.Size = new System.Drawing.Size(99, 42);
         this.SensorStressReadingTextPanel.TabIndex = 181;
         this.SensorStressReadingTextPanel.ValueText = "### psi";
         this.SensorStressReadingTextPanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // SensorThicknessAcquireButton
         // 
         this.SensorThicknessAcquireButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SensorThicknessAcquireButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.SensorThicknessAcquireButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.SensorThicknessAcquireButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.SensorThicknessAcquireButton.HoldTimeoutEnable = false;
         this.SensorThicknessAcquireButton.HoldTimeoutInterval = 100;
         this.SensorThicknessAcquireButton.Location = new System.Drawing.Point(46, 8);
         this.SensorThicknessAcquireButton.Name = "SensorThicknessAcquireButton";
         this.SensorThicknessAcquireButton.Size = new System.Drawing.Size(107, 67);
         this.SensorThicknessAcquireButton.TabIndex = 148;
         this.SensorThicknessAcquireButton.Text = "ACQUIRE THICKNESS";
         this.SensorThicknessAcquireButton.UseVisualStyleBackColor = false;
         this.SensorThicknessAcquireButton.Click += new System.EventHandler(this.SensorThicknessAcquireButton_Click);
         // 
         // SensorStressAcquireButton
         // 
         this.SensorStressAcquireButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
         this.SensorStressAcquireButton.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
         this.SensorStressAcquireButton.DisabledForeColor = System.Drawing.Color.Silver;
         this.SensorStressAcquireButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.SensorStressAcquireButton.HoldTimeoutEnable = false;
         this.SensorStressAcquireButton.HoldTimeoutInterval = 100;
         this.SensorStressAcquireButton.Location = new System.Drawing.Point(46, 83);
         this.SensorStressAcquireButton.Name = "SensorStressAcquireButton";
         this.SensorStressAcquireButton.Size = new System.Drawing.Size(107, 67);
         this.SensorStressAcquireButton.TabIndex = 149;
         this.SensorStressAcquireButton.Text = "ACQUIRE STRESS";
         this.SensorStressAcquireButton.UseVisualStyleBackColor = false;
         this.SensorStressAcquireButton.Click += new System.EventHandler(this.SensorStressAcquireButton_Click);
         // 
         // SensorThicknessReadingTextPanel
         // 
         this.SensorThicknessReadingTextPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
         this.SensorThicknessReadingTextPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.SensorThicknessReadingTextPanel.Enabled = false;
         this.SensorThicknessReadingTextPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SensorThicknessReadingTextPanel.ForeColor = System.Drawing.Color.Silver;
         this.SensorThicknessReadingTextPanel.HoldTimeoutEnable = false;
         this.SensorThicknessReadingTextPanel.HoldTimeoutInterval = 100;
         this.SensorThicknessReadingTextPanel.Location = new System.Drawing.Point(199, 20);
         this.SensorThicknessReadingTextPanel.Name = "SensorThicknessReadingTextPanel";
         this.SensorThicknessReadingTextPanel.Size = new System.Drawing.Size(99, 42);
         this.SensorThicknessReadingTextPanel.TabIndex = 174;
         this.SensorThicknessReadingTextPanel.ValueText = "### mm";
         this.SensorThicknessReadingTextPanel.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // VersionLabel
         // 
         this.VersionLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.VersionLabel.ForeColor = System.Drawing.SystemColors.AppWorkspace;
         this.VersionLabel.Location = new System.Drawing.Point(1794, 11);
         this.VersionLabel.Name = "VersionLabel";
         this.VersionLabel.Size = new System.Drawing.Size(100, 23);
         this.VersionLabel.TabIndex = 130;
         this.VersionLabel.Text = "2015.11.9.1";
         this.VersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // RobotCrossSectionView
         // 
         this.RobotCrossSectionView.Axial = true;
         this.RobotCrossSectionView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.RobotCrossSectionView.BorderColor = System.Drawing.Color.Black;
         this.RobotCrossSectionView.GaugeBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
         this.RobotCrossSectionView.GaugeEdgekColor = System.Drawing.Color.Black;
         this.RobotCrossSectionView.GaugeEdgeWeight = 1;
         this.RobotCrossSectionView.GaugeInnerNumberSpace = 3;
         this.RobotCrossSectionView.GaugeOuterNumberSpace = 3;
         this.RobotCrossSectionView.ImeMode = System.Windows.Forms.ImeMode.On;
         this.RobotCrossSectionView.Location = new System.Drawing.Point(2, 85);
         this.RobotCrossSectionView.Name = "RobotCrossSectionView";
         this.RobotCrossSectionView.PitchBackColor = System.Drawing.Color.Black;
         this.RobotCrossSectionView.PitchFont = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.RobotCrossSectionView.PitchForeColor = System.Drawing.Color.White;
         this.RobotCrossSectionView.PitchHeight = 30;
         this.RobotCrossSectionView.PitchVisible = true;
         this.RobotCrossSectionView.PitchWidth = 55;
         this.RobotCrossSectionView.RobotArmColor = System.Drawing.Color.Gray;
         this.RobotCrossSectionView.RobotBodyColor = System.Drawing.Color.Gray;
         this.RobotCrossSectionView.RobotPitch = 0;
         this.RobotCrossSectionView.RobotRoll = 0;
         this.RobotCrossSectionView.RobotTopWheelIndicatorColor = System.Drawing.Color.Turquoise;
         this.RobotCrossSectionView.RobotWheelColor = System.Drawing.Color.Teal;
         this.RobotCrossSectionView.Size = new System.Drawing.Size(232, 232);
         this.RobotCrossSectionView.TabIndex = 129;
         // 
         // BotSideView
         // 
         this.BotSideView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.BotSideView.ClosedImage = ((System.Drawing.Bitmap)(resources.GetObject("BotSideView.ClosedImage")));
         this.BotSideView.ClosedImageScale = 1D;
         this.BotSideView.ClosedImageTransparentColor = System.Drawing.Color.White;
         this.BotSideView.ClosedText = "CLOSED";
         this.BotSideView.ClosedTextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
         this.BotSideView.Cursor = System.Windows.Forms.Cursors.Default;
         this.BotSideView.DescriptionColor = System.Drawing.Color.Black;
         this.BotSideView.DescriptionFont = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold);
         this.BotSideView.DrillImage = ((System.Drawing.Bitmap)(resources.GetObject("BotSideView.DrillImage")));
         this.BotSideView.DrillImageScale = 1.057D;
         this.BotSideView.DrillImageTransparentColor = System.Drawing.Color.White;
         this.BotSideView.DrillText = "DRILL";
         this.BotSideView.DrillTextAlignment = System.Drawing.ContentAlignment.TopCenter;
         this.BotSideView.FitHeightPosition = NICBOT.GUI.BodyPositions.opened;
         this.BotSideView.FrontLooseImage = ((System.Drawing.Bitmap)(resources.GetObject("BotSideView.FrontLooseImage")));
         this.BotSideView.FrontLooseImageScale = 1D;
         this.BotSideView.FrontLooseImageTransparentColor = System.Drawing.Color.White;
         this.BotSideView.FrontLooseText = "FRONT RELEASED";
         this.BotSideView.FrontLooseTextAlignment = System.Drawing.ContentAlignment.TopCenter;
         this.BotSideView.HorizontalEdge = 25;
         this.BotSideView.InsideColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
         this.BotSideView.Location = new System.Drawing.Point(238, 86);
         this.BotSideView.ManualImage = ((System.Drawing.Bitmap)(resources.GetObject("BotSideView.ManualImage")));
         this.BotSideView.ManualImageScale = 1D;
         this.BotSideView.ManualImageTransparentColor = System.Drawing.Color.White;
         this.BotSideView.ManualText = "CUSTOM";
         this.BotSideView.ManualTextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
         this.BotSideView.Name = "BotSideView";
         this.BotSideView.OffImage = ((System.Drawing.Bitmap)(resources.GetObject("BotSideView.OffImage")));
         this.BotSideView.OffImageScale = 1D;
         this.BotSideView.OffImageTransparentColor = System.Drawing.Color.White;
         this.BotSideView.OffText = "OFF";
         this.BotSideView.OffTextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
         this.BotSideView.OpenedImage = ((System.Drawing.Bitmap)(resources.GetObject("BotSideView.OpenedImage")));
         this.BotSideView.OpenedImageScale = 1D;
         this.BotSideView.OpenedImageTransparentColor = System.Drawing.Color.White;
         this.BotSideView.OpenedText = "OPENED";
         this.BotSideView.OpenedTextAlignment = System.Drawing.ContentAlignment.TopCenter;
         this.BotSideView.Position = NICBOT.GUI.BodyPositions.manual;
         this.BotSideView.RearLooseImage = ((System.Drawing.Bitmap)(resources.GetObject("BotSideView.RearLooseImage")));
         this.BotSideView.RearLooseImageScale = 1D;
         this.BotSideView.RearLooseImageTransparentColor = System.Drawing.Color.White;
         this.BotSideView.RearLooseText = "REAR RELEASED";
         this.BotSideView.RearLooseTextAlignment = System.Drawing.ContentAlignment.TopCenter;
         this.BotSideView.Size = new System.Drawing.Size(425, 232);
         this.BotSideView.TabIndex = 37;
         this.BotSideView.VerticalEdge = 6;
         // 
         // MainForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.AutoScroll = true;
         this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         this.ClientSize = new System.Drawing.Size(1920, 1092);
         this.Controls.Add(this.VersionLabel);
         this.Controls.Add(this.RobotCrossSectionView);
         this.Controls.Add(this.InspectionPanel);
         this.Controls.Add(this.MovementManulPanel);
         this.Controls.Add(this.SealantManualPanel);
         this.Controls.Add(this.FeederManualPanel);
         this.Controls.Add(this.ReelManualPanel);
         this.Controls.Add(this.GuidePanel);
         this.Controls.Add(this.ReelMainPanel);
         this.Controls.Add(this.panel1);
         this.Controls.Add(this.DrillManualPanel);
         this.Controls.Add(this.FeederMainPanel);
         this.Controls.Add(this.SealantMainPanel);
         this.Controls.Add(this.BotSideView);
         this.Controls.Add(this.MovementMainPanel);
         this.Controls.Add(this.DrillMainPanel);
         this.Controls.Add(this.panel27);
         this.Controls.Add(this.panel24);
         this.Controls.Add(this.ControlPanel);
         this.Controls.Add(this.panel25);
         this.Controls.Add(this.panel23);
         this.Controls.Add(this.panel12);
         this.Controls.Add(this.TitleLabel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "MainForm";
         this.Text = "NICBOT Control";
         this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
         this.Shown += new System.EventHandler(this.MainForm_Shown);
         this.ControlPanel.ResumeLayout(false);
         this.panel12.ResumeLayout(false);
         this.panel12.PerformLayout();
         this.panel23.ResumeLayout(false);
         this.panel25.ResumeLayout(false);
         this.borderedPanel9.ResumeLayout(false);
         this.borderedPanel8.ResumeLayout(false);
         this.borderedPanel7.ResumeLayout(false);
         this.panel24.ResumeLayout(false);
         this.panel27.ResumeLayout(false);
         this.DrillMainPanel.ResumeLayout(false);
         this.DrillMainPanel.PerformLayout();
         this.borderedPanel10.ResumeLayout(false);
         this.MovementMainPanel.ResumeLayout(false);
         this.SealantMainPanel.ResumeLayout(false);
         this.borderedPanel11.ResumeLayout(false);
         this.FeederMainPanel.ResumeLayout(false);
         this.FeederManualPanel.ResumeLayout(false);
         this.DrillManualPanel.ResumeLayout(false);
         this.DrillManualPanel.PerformLayout();
         this.borderedPanel1.ResumeLayout(false);
         this.borderedPanel1.PerformLayout();
         this.panel1.ResumeLayout(false);
         this.RearSealantReserviorPanel.ResumeLayout(false);
         this.RearSealantReserviorPanel.PerformLayout();
         this.FrontSealantReserviorPanel.ResumeLayout(false);
         this.FrontSealantReserviorPanel.PerformLayout();
         this.borderedPanel2.ResumeLayout(false);
         this.borderedPanel2.PerformLayout();
         this.ReelMainPanel.ResumeLayout(false);
         this.ReelMainPanel.PerformLayout();
         this.ReelManualPanel.ResumeLayout(false);
         this.ReelManualPanel.PerformLayout();
         this.GuidePanel.ResumeLayout(false);
         this.SealantManualPanel.ResumeLayout(false);
         this.SealantManualPanel.PerformLayout();
         this.MovementManulPanel.ResumeLayout(false);
         this.InspectionPanel.ResumeLayout(false);
         this.borderedPanel6.ResumeLayout(false);
         this.borderedPanel5.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Label TitleLabel;
      private System.Windows.Forms.Panel ControlPanel;
      private System.Windows.Forms.Button WriteOsdButton;
      private System.Windows.Forms.Button SystemStatusButton;
      private System.Windows.Forms.Panel panel12;
      private System.Windows.Forms.TextBox SystemStatusTextBox;
      private System.Windows.Forms.Label label17;
      private System.Windows.Forms.Panel panel23;
      private System.Windows.Forms.Panel panel25;
      private System.Windows.Forms.Timer UpdateTimer;
      private CameraSelectButton RobotCamera1Button;
      private CameraSelectButton RobotCamera2Button;
      private CameraSelectButton RobotCamera12Button;
      private CameraSelectButton RobotCamera11Button;
      private CameraSelectButton RobotCamera10Button;
      private CameraSelectButton RobotCamera9Button;
      private CameraSelectButton RobotCamera8Button;
      private CameraSelectButton RobotCamera7Button;
      private CameraSelectButton RobotCamera6Button;
      private CameraSelectButton RobotCamera5Button;
      private CameraSelectButton RobotCamera4Button;
      private CameraSelectButton RobotCamera3Button;
      private System.Windows.Forms.Panel panel24;
      private System.Windows.Forms.Panel panel27;
      private CameraSelectButton LaunchCamera4Button;
      private CameraSelectButton LaunchCamera3Button;
      private CameraSelectButton LaunchCamera2Button;
      private CameraSelectButton LaunchCamera1Button;
      private System.Windows.Forms.Panel DrillMainPanel;
      private System.Windows.Forms.TextBox RetractionLimitLightTextBox;
      private System.Windows.Forms.TextBox OriginSetLightTextBox;
      private System.Windows.Forms.TextBox DrillErrorLightTextBox;
      private System.Windows.Forms.Label label48;
      private System.Windows.Forms.Label label47;
      private System.Windows.Forms.Label label46;
      private UpDownButton DrillSpeedDecreaseButton;
      private UpDownButton DrillSpeedIncreaseButton;
      private System.Windows.Forms.Label label43;
      private System.Windows.Forms.Label label45;
      private UpDownButton DrillIndexDownButton;
      private UpDownButton DrillIndexUpButton;
      private TextPanel DrillExtendedSetPointValuePanel;
      private TextPanel DrillRotaionSetPointSpeedValuePanel;
      private CameraSelectButton RobotCameraBSelectButton;
      private CameraSelectButton LightSelectButton;
      private CameraSelectButton RobotCameraASelectButton;
      private LineControl lineControl1;
      private LineControl lineControl2;
      private System.Windows.Forms.Panel MovementMainPanel;
      private ValueToggleButton MovementAxialToggleButton;
      private HoldButton MovementSetupButton;
      private ValueToggleButton MovementSpeedToggleButton;
      private ValueButton MotorManualJogDistanceValueButton;
      private NicBotSideView BotSideView;
      private System.Windows.Forms.Panel SealantMainPanel;
      private System.Windows.Forms.Label NozzleSelectionLabel;
      private System.Windows.Forms.Panel FeederMainPanel;
      private System.Windows.Forms.Label FeederTitleLabel;
      private HoldButton FeederSetupButton;
      private ValueToggleButton FeederSpeedToggleButton;
      private System.Windows.Forms.Label MotorTitleLabel;
      private ValueButton MovementMoveButton;
      private ValueButton FeederMoveButton;
      private DirectionalValuePanel MotorStatusDirectionalValuePanel;
      private DirectionalValuePanel FeederActualValuePanel;
      private System.Windows.Forms.Label label15;
      private System.Windows.Forms.Panel DrillManualPanel;
      private ValueToggleButton DrillManualToggleButton;
      private ValueToggleButton DrillDirectionToggleButton;
      private HoldButton DrillSetOriginButton;
      private HoldButton DrillFindOriginButton;
      private HoldButton DrillSetupButton;
      private System.Windows.Forms.Label DrillSelectionLabel;
      private TextPanel DrillExtendedActualValuePanel;
      private TextPanel DrillRotationActualSpeedValuePanel;
      private System.Windows.Forms.Label label28;
      private System.Windows.Forms.Label label27;
      private System.Windows.Forms.Label label26;
      private System.Windows.Forms.Label DrillModeLabel;
      private System.Windows.Forms.Label label24;
      private System.Windows.Forms.Label DrillPipePositionLabel;
      private HoldButton CustomSetupButton;
      private HoldButton DrillAutoStartButton;
      private HoldButton BodyClosedButton;
      private HoldButton BodyOpenButton;
      private HoldButton BodyDrillButton;
      private HoldButton BodyRearReleaseButton;
      private HoldButton BodyFrontReleaseButton;
      private System.Windows.Forms.Panel panel1;
      private TextPanel NitrogenPressure2TextPanel;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Panel ReelMainPanel;
      private HoldButton ReelSetupButton;
      private HoldButton ReelResetTotalButton;
      private HoldButton ReelResetTripButton;
      private ValueButton ReelCalibrateToButton;
      private HoldButton ReelLockButton;
      private HoldButton ReelReverseButton;
      private HoldButton ReelOffButton;
      private TextPanel ReelTripTextPanel;
      private System.Windows.Forms.Label label5;
      private TextPanel ReelTotalTextPanel;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label label3;
      private DirectionalValuePanel ReelActualValuePanel;
      private ValueToggleButton ReelManualOnOffToggleButton;
      private System.Windows.Forms.Panel ReelManualPanel;
      private TextPanel ReelManualCurrentTextPanel;
      private System.Windows.Forms.Label label6;
      private UpDownButton ReelManualCurrentUpButton;
      private UpDownButton ReelManualCurrentDownButton;
      private ValueToggleButton ReelManualTorqueDirectionToggleButton;
      private System.Windows.Forms.Panel GuidePanel;
      private System.Windows.Forms.Label label7;
      private HoldButton GuideSetupButton;
      private CameraSelectButton GuideExtendRightButton;
      private CameraSelectButton GuideRetractRightButton;
      private CameraSelectButton GuideExtendLeftButton;
      private CameraSelectButton GuideRetractLeftButton;
      private HoldButton FeederOffButton;
      private HoldButton FeederLockButton;
      private System.Windows.Forms.Panel FeederManualPanel;
      private HoldButton FeederManualReverseButton;
      private HoldButton FeederManualForwardButton;
      private ValueButton FeederSpeedValueButton;
      private HoldButton ReelManualSetupButton;
      private HoldButton ReelManualResetTotalButton;
      private HoldButton ReelManualResetTripButton;
      private ValueButton ReelManualCalibrateToButton;
      private HoldButton DrillSealModeButton;
      private HoldButton ExitButton;
      private HoldButton SystemResetButton;
      private System.Windows.Forms.Panel SealantManualPanel;
      private ValueToggleButton SealantManualModeToggleButton;
      private ValueToggleButton SealantManualPumpToggleButton;
      private UpDownButton SealantSpeedIncreaseButton;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.Label label9;
      private UpDownButton SealantSpeedDecreaseButton;
      private TextPanel SealantReserviorTextPanel;
      private System.Windows.Forms.Label label29;
      private TextPanel SealantNozzlePositionTextPanel;
      private TextPanel SealantFlowRateTextPanel;
      private System.Windows.Forms.Label label30;
      private System.Windows.Forms.Label label25;
      private TextPanel SealantActualSpeedValuePanel;
      private System.Windows.Forms.Label label23;
      private TextPanel SealantSpeedSetPointValuePanel;
      private TextPanel SealantActualPressureValuePanel;
      private TextPanel SealantPressureSetPointValuePanel;
      private TextPanel SealantActualVolumeValuePanel;
      private System.Windows.Forms.Label label18;
      private System.Windows.Forms.Label label19;
      private TextPanel SealantVolumeSetPointValuePanel;
      private System.Windows.Forms.Label label10;
      private System.Windows.Forms.Label SealantModeLabel;
      private System.Windows.Forms.Label label12;
      private System.Windows.Forms.Label SealantPipePositionLabel;
      private HoldButton SealDrillModeButton;
      private HoldButton SealantSetupButton;
      private HoldButton SealantAutoStartButton;
      private ValueToggleButton SealantDirectionToggleButton;
      private UpDownButton SealantPressureDecreaseButton;
      private UpDownButton SealantPressureIncreaseButton;
      private ValueToggleButton SealantNozzleToggleButton;
      private BorderedPanel borderedPanel1;
      private ValueToggleButton DrillLaserLightButton;
      private ValueToggleButton SealantLaserLightButton;
      private RotatableLabel rotatableLabel1;
      private RotatableLabel rotatableLabel2;
      private RotatableLabel rotatableLabel3;
      private RotatableLabel rotatableLabel4;
      private ValueToggleButton MovementCornerModeToggleButton;
      private ValueToggleButton MovementLaunchModeToggleButton;
      private System.Windows.Forms.Panel MovementManulPanel;
      private ValueButton MotorManualMoveSpeedValueButton;
      private HoldButton MotorManualMoveReverseButton;
      private HoldButton MotorManualMoveForwardButton;
      private NicBotButton MovementManaulDisplayButton;
      private NicBotButton ReelShowManualButton;
      private NicBotButton ReelManualHideButton;
      private NicBotButton MotorManualJogReverseButton;
      private NicBotButton MotorManualJogForwardButton;
      private HoldButton MovementOffButton;
      private HoldButton MovementLockButton;
      private NicBotButton FeederManulDisplayButton;
      private NicBotButton DrillAutoPauseResumeButton;
      private NicBotButton DrillAutoStopButton;
      private NicBotButton DrillManulDisplayButton;
      private NicBotButton DrillMoveToOriginButton;
      private NicBotButton SealantManulDisplayButton;
      private NicBotButton SealantAutoStopButton;
      private NicBotButton SealantAutoPauseResumeButton;
      private RotatableLabel rotatableLabel5;
      private RotatableLabel rotatableLabel6;
      private RotatableLabel rotatableLabel7;
      private RotatableLabel rotatableLabel8;
      private RotatableLabel rotatableLabel9;
      private RotatableLabel rotatableLabel10;
      private NicBotButton DrillRetractToLimitButton;
      private NicBotButton DrillStopButton;
      private ValueButton SealantRelievePressureButton;
      private HoldButton FeederClampSetupButton;
      private NicBotButton FeederHideManulButton;
      private HoldButton FeederManualSetupButton;
      private TextPanel NitrogenPressure1TextPanel;
      private System.Windows.Forms.Label label20;
      private TextPanel FrontSealantReserviorVolumeTextPanel;
      private System.Windows.Forms.Label label16;
      private TextPanel FrontSealantReserviorWeightTextPanel;
      private System.Windows.Forms.Label label14;
      private BorderedPanel borderedPanel2;
      private BorderedPanel RearSealantReserviorPanel;
      private System.Windows.Forms.Label label11;
      private TextPanel RearSealantReserviorWeightTextPanel;
      private System.Windows.Forms.Label label21;
      private System.Windows.Forms.Label label33;
      private TextPanel RearSealantReserviorVolumeTextPanel;
      private BorderedPanel FrontSealantReserviorPanel;
      private System.Windows.Forms.Label label32;
      private LineControl lineControl3;
      private LineControl lineControl6;
      private LineControl lineControl5;
      private System.Windows.Forms.Panel InspectionPanel;
      private BorderedPanel borderedPanel6;
      private TextPanel SensorLongitudeTextPanel;
      private TextPanel SensorLatitudeTextPanel;
      private System.Windows.Forms.Label label35;
      private System.Windows.Forms.Label label37;
      private System.Windows.Forms.Label label38;
      private BorderedPanel borderedPanel5;
      private TextPanel SensorStressReadingTextPanel;
      private HoldButton SensorThicknessAcquireButton;
      private HoldButton SensorStressAcquireButton;
      private TextPanel SensorThicknessReadingTextPanel;
      private TextPanel SensorDisplacementTextPanel;
      private System.Windows.Forms.Label label40;
      private TextPanel SensorDirectionTextPanel;
      private System.Windows.Forms.Label label39;
      private BorderedPanel borderedPanel7;
      private BorderedPanel borderedPanel9;
      private BorderedPanel borderedPanel8;
      private BorderedPanel borderedPanel10;
      private BorderedPanel borderedPanel11;
      private TextPanel SensorGpsTimeTextPanel;
      private System.Windows.Forms.Label label42;
      private System.Windows.Forms.Label label41;
      private TextPanel SensorGpsDateTextPanel;
      private CameraSelectButton LaunchCameraSelectButton;
      private System.Windows.Forms.Label label2;
      private TextPanel SensorPipePositionTextPanel;
      private System.Windows.Forms.Label label13;
      private RotatableLabel rotatableLabel13;
      private Controls.CrossSectionView RobotCrossSectionView;
      private System.Windows.Forms.Label VersionLabel;
   }
}

