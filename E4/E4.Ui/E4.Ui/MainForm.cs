namespace E4.Ui
{
   using System;
   using System.Collections.Generic;
   using System.ComponentModel;
   using System.Data;
   using System.Diagnostics;
   using System.Drawing;
   using System.Linq;
   using System.Text;
   using System.Threading.Tasks;
   using System.Windows.Forms;

   using E4.Utilities;

   public partial class MainForm : Form
   {
      #region Definitions

      private delegate void ProcessHandler();

      private enum JoystickApplications
      {
         none,
         laserRobot,
         targetRobot,
         laserAim,
      }

      #endregion

      #region Fields

      private FileTraceListener fileTraceListener;
      private UdpTraceListener traceListener;

      private bool processStopNeeded;
      private bool processExitNeeded;
      private bool processStopped;

      private bool indicatorFlasher;
      private bool messageFlasher;
      private int messageFlashCount;

      private JoystickApplications joystickApplication;

      private bool laserMeasurementRequested;

      private bool laserMeasurementRecorded;

      private int laserXButtonChange;
      private int laserXRequestedChange;
      private int laserYButtonChange;
      private int laserYRequestedChange;
      
      private int targetButtonChange;
      private int targetRequestedChange;

      private PopupDimmerForm dimmerForm;

      #endregion

      #region Properties

      private ProcessHandler Process { set; get; }

      #endregion

      #region Helper Functions

      #region General Functions

      private string GetValueText(double operationalValue, ValueParameter parameter)
      {
         string doubleFormat = "N" + parameter.Precision.ToString();
         string result = operationalValue.ToString(doubleFormat) + " " + parameter.Unit;
         return (result);
      }

      private string GetValueText(int value, string unit = "")
      {
         string result = value.ToString();

         if ("" != unit)
         {
            result += (" " + unit);
         }

         return (result);
      }

      private string GetValueText(double value, string doubleFormat, string unit)
      {
         string result = value.ToString(doubleFormat) + " " + unit;
         return (result);
      }

      private int GetAbsoluteLeft(Control control)
      {
         int result = 0;

         while (null != control)
         {
            result += control.Left;
            control = control.Parent;
         }

         return (result);
      }

      private int GetAbsoluteTop(Control control)
      {
         int result = 0;

         while (null != control)
         {
            result += control.Top;
            control = control.Parent;
         }

         return (result);
      }

      private void SetDialogLocation(Control control, Form form)
      {
         int offsetX = (form.Width - control.Width) / 2;
         int offsetY = (form.Height - control.Height) / 2;
         int formLeft = this.GetAbsoluteLeft(control) - offsetX;
         int formTop = this.GetAbsoluteTop(control) - offsetY;
         int formLeftMaximum = (this.Left + this.Width) - form.Width - 1;
         int formTopMaximum = (this.Top + this.Height) - form.Height - 1;

         if (formLeft < this.Left)
         {
            formLeft = this.Left;
         }
         else if (formLeft > formLeftMaximum)
         {
            formLeft = formLeftMaximum;
         }

         if (formTop < this.Top)
         {
            formTop = this.Top;
         }
         else if (formTop > formTopMaximum)
         {
            formTop = formTopMaximum;
         }

         form.Left = formLeft;
         form.Top = formTop;
      }

      private void DimBackground()
      {
         this.dimmerForm.Top = 0;
         this.dimmerForm.Left = 0;
         this.dimmerForm.Height = this.Height;
         this.dimmerForm.Width = this.Width;
         this.dimmerForm.Show();
      }

      private void LightBackground()
      {
         this.dimmerForm.Hide();
      }

      private void RestartSystem()
      {
         this.processStopNeeded = true;
      }

      private void SetTraceListenerDestination()
      {
         this.traceListener.SetDestination(ParameterAccessor.Instance.Trace.Address, ParameterAccessor.Instance.Trace.Port);
         Tracer.WriteHigh(TraceGroup.UI, null, "endpoint set");
      }

      #endregion

      #region Joystick 

      private void UpdateJoystickApplicationButtons()
      {
         if (JoystickApplications.none == this.joystickApplication)
         {
            this.LaserRobotMovementJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.LaserRobotMovementJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);

            this.LaserJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.LaserJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);

            this.TargetRobotMovementJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.TargetRobotMovementJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);
         }
         else if (JoystickApplications.laserRobot == this.joystickApplication)
         {
            this.LaserRobotMovementJoystickEnableButton.Text = "RELEASE JOYSTICK";
            this.LaserRobotMovementJoystickEnableButton.BackColor = Color.Lime;

            this.LaserJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.LaserJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);

            this.TargetRobotMovementJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.TargetRobotMovementJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);
         }
         else if (JoystickApplications.targetRobot == this.joystickApplication)
         {
            this.LaserRobotMovementJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.LaserRobotMovementJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);
            
            this.LaserJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.LaserJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);

            this.TargetRobotMovementJoystickEnableButton.Text = "RELEASE JOYSTICK";
            this.TargetRobotMovementJoystickEnableButton.BackColor = Color.Lime;
         }
         else if (JoystickApplications.laserAim == this.joystickApplication)
         {
            this.LaserRobotMovementJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.LaserRobotMovementJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);

            this.LaserJoystickEnableButton.Text = "RELEASE JOYSTICK";
            this.LaserJoystickEnableButton.BackColor = Color.Lime;

            this.TargetRobotMovementJoystickEnableButton.Text = "JOYSTICK DRIVE";
            this.TargetRobotMovementJoystickEnableButton.BackColor = Color.FromArgb(171, 171, 171);
         }
      }

      #endregion

      #endregion

      #region Process Functions

      private void ProcessStart()
      {
         // zero-initialize fields

         this.processStopped = false;

         this.indicatorFlasher = false;
         this.messageFlasher = false;
         this.messageFlashCount = 0;

         this.joystickApplication = JoystickApplications.none;

         this.laserMeasurementRequested = false;

         this.laserMeasurementRecorded = false;

         this.laserXButtonChange = 0;
         this.laserXRequestedChange = 0;
         this.laserYButtonChange = 0;
         this.laserYRequestedChange = 0;

         this.targetButtonChange = 0;
         this.targetRequestedChange = 0;


         // set next state

         this.UpdateTimer.Interval = 1;
         this.Process = this.ProcessShow;
      }
      
      private void ProcessShow()
      {
         // set version

         string versionString = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
         this.VersionLabel.Text = versionString;


         // clear display

         this.LaserPitchTickPanel.ValueText = this.GetValueText(DeviceCommunication.Instance.GetLaserStepperYActualPosition());
         this.LaserYawTickPanel.ValueText = this.GetValueText(DeviceCommunication.Instance.GetLaserStepperXActualPosition());

         this.SensorPitchPanel.ValueText = "";
         this.SensorPitchTickPanel.ValueText = this.GetValueText(DeviceCommunication.Instance.GetTargetStepperActualPosition());

         this.LaserTitleLabel.Text = "LASER MEASURE";
         this.LaserMeasurementValuePanel.ValueText = "";
         this.LaserMeasureButton.Text = "MEASURE";

         this.SensorIndicator.MissColor = Color.FromKnownColor(KnownColor.ControlDark);
         this.SensorIndicator.BackColor = Color.FromKnownColor(KnownColor.ControlDark);
         this.SensorIndicator.CoordinateValue = 0;

         this.StopAllPanel.BackColor = Color.FromArgb(64, 64, 64);
         this.StopAllPanel.Refresh();
         this.StopAllButton.Refresh();


         // set next state

         this.UpdateTimer.Interval = 1;

         this.MainStatusTextBox.Width = (this.TargetStatusTextBox.Left + this.TargetStatusTextBox.Width - this.MainStatusTextBox.Left);
         this.TargetStatusTextBox.Visible = false;

         this.Process = this.ProcessStarting;
      }

      private void ProcessStarting()
      {
         // read parameters

         ParameterAccessor.Instance.Read(Application.ExecutablePath);
         this.traceListener.SetDestination(ParameterAccessor.Instance.Trace.Address, ParameterAccessor.Instance.Trace.Port);
         Tracer.WriteHigh(TraceGroup.UI, null, "starting");


         // process parameters

         if ((0 == ParameterAccessor.Instance.LaserBus.ConsumerHeartbeatRate) ||
             (0 == ParameterAccessor.Instance.LaserBus.ProducerHeartbeatRate) ||
             (0 == ParameterAccessor.Instance.TargetBus.ConsumerHeartbeatRate) ||
             (0 == ParameterAccessor.Instance.TargetBus.ProducerHeartbeatRate))
         {
            this.HeartbeatsDisabledLabel.Visible = true;
         }


         this.LaserRangeJoystickXRequestIndicator.Position = 0;
         this.LaserRangeJoystickXRequestIndicator.MaximumPosition = 32767 - ParameterAccessor.Instance.JoystickDeadband;
         this.LaserRangeJoystickXRequestIndicator.MinimumPosition = -32767 + ParameterAccessor.Instance.JoystickDeadband;

         this.LaserRangeJoystickYRequestIndicator.Position = 0;
         this.LaserRangeJoystickYRequestIndicator.MaximumPosition = 32767 - ParameterAccessor.Instance.JoystickDeadband;
         this.LaserRangeJoystickYRequestIndicator.MinimumPosition = -32767 + ParameterAccessor.Instance.JoystickDeadband;

         this.LaserScannerJoystickYRequestIndicator.Position = 0;
         this.LaserScannerJoystickYRequestIndicator.MaximumPosition = 32767 - ParameterAccessor.Instance.JoystickDeadband;
         this.LaserScannerJoystickYRequestIndicator.MinimumPosition = -32767 + ParameterAccessor.Instance.JoystickDeadband;

         DeviceCommunication.Instance.Start();


         // set next state

         this.MainStatusTextBox.Text = "starting";
         this.MainStatusTextBox.BackColor = Color.Yellow;

         this.UpdateTimer.Interval = 100;
         this.Process = this.ProcessWaitComm;
      }

      private void ProcessWaitComm()
      {
         if (false != this.processExitNeeded)
         {
            this.Process = this.ProcessStopping;
         }
         else if (false != DeviceCommunication.Instance.Ready)
         {
            // initialize based on hardware

            Tracer.WriteHigh(TraceGroup.UI, null, "started");
            this.Process = this.ProcessExecution;

            this.MainStatusTextBox.Width = (this.TargetStatusTextBox.Left - 8 - this.MainStatusTextBox.Left);
            this.TargetStatusTextBox.Visible = true;
         }
      }

      private void ProcessExecution()
      {
         #region General 

         this.indicatorFlasher = !this.indicatorFlasher;

         this.messageFlashCount++;

         if (messageFlashCount >= 3)
         {
            this.messageFlashCount = 0;
            this.messageFlasher = !this.messageFlasher;
         }

         DeviceCommunication.Instance.Service();

         #endregion

         #region Status

         string mainStatus = null;
         bool mainWarning = false;

         if (null != Joystick.Instance.FaultReason)
         {
            mainStatus = "joystick missing";
         }
         else
         {
            mainStatus = DeviceCommunication.Instance.GetMainFaultStatus();

            if (null == mainStatus)
            {
               mainStatus = DeviceCommunication.Instance.GetMainWarningStatus();
               mainWarning = true;
            }
         }

         if (null == mainStatus)
         {
            this.MainStatusTextBox.Text = "ready";
            this.MainStatusTextBox.BackColor = Color.LimeGreen;
         }
         else
         {
            this.MainStatusTextBox.Text = mainStatus;
            this.MainStatusTextBox.BackColor = (false == mainWarning) ? Color.Red : Color.Yellow;
         }

         #endregion

         #region Target Status

         string targetStatus = null;
         bool targetWarning = false;

         targetStatus = DeviceCommunication.Instance.GetTargetFaultStatus();

         if (null == targetStatus)
         {
            targetStatus = DeviceCommunication.Instance.GetTargetWarningStatus();
            targetWarning = true;
         }

         if (null == targetStatus)
         {
            this.TargetStatusTextBox.Text = "ready";
            this.TargetStatusTextBox.BackColor = Color.LimeGreen;
         }
         else
         {
            this.TargetStatusTextBox.Text = targetStatus;
            this.TargetStatusTextBox.BackColor = (false == targetWarning) ? Color.Red : Color.Yellow;
         }

         #endregion

         #region Joystick and Movement

         int joystickXChange = 0;
         int joystickYChange = 0;
         int joystickThrottleChange = 0;

         Joystick.Instance.Update();

         if (false != Joystick.Instance.Valid)
         {
            #region Joystick Idle

            //bool joystickXIdle = false;
            //bool joystickYIdle = false;
            //bool joystickThrottleIdle = false;



            int joystickXAxis = (int)(((Joystick.Instance.XAxis) - 32767) * -1);
            int joystickYAxis = (int)(((Joystick.Instance.YAxis) - 32767) * -1);
            int joystickThrottle = (int)(((Joystick.Instance.Throttle) - 32767) * -1);
            //int joystickThrottle = (int)(65535 - Joystick.Instance.Throttle);

            int joystickDeadband = ParameterAccessor.Instance.JoystickDeadband;

            if (joystickXAxis > joystickDeadband)
            {
               joystickXAxis -= joystickDeadband;
            }
            else if (joystickXAxis < -joystickDeadband)
            {
               joystickXAxis += joystickDeadband;
            }
            else
            {
               joystickXAxis = 0;
            }

            if (joystickYAxis > joystickDeadband)
            {
               joystickYAxis -= joystickDeadband;
            }
            else if (joystickYAxis < -joystickDeadband)
            {
               joystickYAxis += joystickDeadband;
            }
            else
            {
               joystickYAxis = 0;
            }

            if (joystickThrottle > joystickDeadband)
            {
               joystickThrottle -= joystickDeadband;
            }
            else if (joystickThrottle < -joystickDeadband)
            {
               joystickThrottle += joystickDeadband;
            }
            else
            {
               joystickThrottle = 0;
            }

            this.LaserRangeJoystickXRequestIndicator.Position = joystickXAxis;
            this.LaserRangeJoystickYRequestIndicator.Position = joystickYAxis;
            this.LaserScannerJoystickYRequestIndicator.Position = joystickThrottle;

            if (JoystickApplications.none != this.joystickApplication)
            {
               joystickXChange = joystickXAxis;
               joystickYChange = joystickYAxis;
               joystickThrottleChange = joystickThrottle;
            }

            if (this.joystickApplication != JoystickApplications.laserRobot)
            {
               if ((0 != joystickXAxis) ||
                   (0 != joystickYAxis))
               {
                  this.LaserRobotMovementJoystickEnableButton.Enabled = false;
               }
               else
               {
                  this.LaserRobotMovementJoystickEnableButton.Enabled = true;
               }
            }

            if (this.joystickApplication != JoystickApplications.targetRobot)
            {
               if ((0 != joystickXAxis) ||
                   (0 != joystickYAxis))
               {
                  this.TargetRobotMovementJoystickEnableButton.Enabled = false;
               }
               else
               {
                  this.TargetRobotMovementJoystickEnableButton.Enabled = true;
               }
            }

            if (this.joystickApplication != JoystickApplications.laserAim)
            {
               if ((0 != joystickXAxis) ||
                   (0 != joystickYAxis) ||
                   (0 != joystickThrottle))
               {            
                  this.LaserJoystickEnableButton.Enabled = false;
               }
               else
               {
                  this.LaserJoystickEnableButton.Enabled = true;
               }
            }

            #endregion
         }

         #endregion

         #region Laser Measurement

         string laserMeasureFault = LaserCommunicationBus.Instance.GetFaultStatus(LaserCommunicationBus.BusComponentId.LaserBoard);
         string targetMeasureFault = TargetCommunicationBus.Instance.GetFaultStatus(TargetCommunicationBus.BusComponentId.TargetBoard);

         this.LaserPitchTickPanel.ValueText = this.GetValueText(DeviceCommunication.Instance.GetLaserStepperYActualPosition());
         this.LaserYawTickPanel.ValueText = this.GetValueText(DeviceCommunication.Instance.GetLaserStepperXActualPosition());

         double targetPitchValue = DeviceCommunication.Instance.GetTargetPitch();
         this.SensorPitchPanel.ValueText = this.GetValueText(targetPitchValue, "N1", "°");
         this.SensorPitchTickPanel.ValueText = this.GetValueText(DeviceCommunication.Instance.GetTargetStepperActualPosition());

         if ((null == laserMeasureFault) && (null == targetMeasureFault))
         {
            this.LaserMeasureButton.BackColor = Color.FromArgb(171, 171, 171);
            
            bool laserAimActive = DeviceCommunication.Instance.GetLaserAim();
            this.LaserAimButton.BackColor = (false != laserAimActive) ? Color.LimeGreen : Color.FromArgb(171, 171, 171);

            bool laserReadingReady = DeviceCommunication.Instance.GetLaserMeasurementReady();
            bool laserMeasureActive = DeviceCommunication.Instance.GetLaserMeasurementActivity();

            if (false != this.laserMeasurementRequested)
            {
               if (false != laserReadingReady)
               {
                  double laserMeasurement = DeviceCommunication.Instance.GetAverageLaserMeasurement();
                  this.LaserMeasurementValuePanel.BackColor = Color.Black;
                  this.LaserMeasurementValuePanel.ValueText = this.GetValueText(laserMeasurement, ParameterAccessor.Instance.LaserMeasurementConstant);
                  this.LaserTitleLabel.Text = "LASER MEASURE - COMPLETE";

                  this.LaserMeasureButton.Text = "MEASURE";
               }
               else
               {
                  this.LaserMeasurementValuePanel.BackColor = (false != this.messageFlasher) ? Color.DodgerBlue : Color.SteelBlue;

                  if (false != laserMeasureActive)
                  {
                     int laserMeasureRemainingCount = DeviceCommunication.Instance.GetLaserSampleRemainingCount();
                     string laserMeasureStatusText = string.Format("LASER MEASURE - READING ({0})", laserMeasureRemainingCount);
                     this.LaserTitleLabel.Text = laserMeasureStatusText;
                  }
                  else
                  {
                     string laserMeasureStatusText = string.Format("LASER MEASURE - STARTING");
                     this.LaserMeasurementValuePanel.ValueText = "";
                     this.laserMeasurementRecorded = false;
                  }

                  this.LaserMeasureButton.Text = "CANCEL";
               }
            }
            else
            {
               this.LaserTitleLabel.Text = "LASER MEASURE";
               this.LaserMeasureButton.Text = "MEASURE";
               this.LaserMeasurementValuePanel.BackColor = Color.Black;
            }
         }
         else
         {
            this.LaserMeasureButton.BackColor = Color.Red;
            this.LaserTitleLabel.Text = "LASER MEASURE - FAULTED";
         }


         this.RecordLaserMeasurementButton.Enabled = ((DeviceCommunication.Instance.GetLaserMeasurementReady() != false) && (false == this.laserMeasurementRecorded)) ? true : false;

         string laserScannerFault = TargetCommunicationBus.Instance.GetFaultStatus(TargetCommunicationBus.BusComponentId.TargetBoard);

         if (null == laserScannerFault)
         {
            this.SensorIndicator.MissColor = Color.FromArgb(140, 0, 0);
            this.SensorIndicator.BackColor = Color.FromKnownColor(KnownColor.ControlDark);
            this.SensorIndicator.CoordinateValue = DeviceCommunication.Instance.GetLaserScannerCoordinates();
         }
         else
         {
            this.SensorIndicator.MissColor = Color.Red;
            this.SensorIndicator.BackColor = Color.Red;
            this.SensorIndicator.CoordinateValue = 0;
         }

         #endregion

         #region Laser Aiming

         int laserStepperXPosition = DeviceCommunication.Instance.GetLaserStepperXActualPosition();
         int laserStepperYPosition = DeviceCommunication.Instance.GetLaserStepperYActualPosition();
         int targetStepperPosition = DeviceCommunication.Instance.GetTargetStepperActualPosition();

         int laserXChange = this.laserXButtonChange;
         int laserYChange = this.laserYButtonChange;
         int targetChange = this.targetButtonChange;

         if (JoystickApplications.laserAim == this.joystickApplication)
         {
            laserXChange = joystickXChange;
            laserYChange = joystickYChange;
            targetChange = joystickThrottleChange;
         }

         if (laserXChange > 0)
         {
            laserStepperXPosition = ParameterAccessor.Instance.LaserXStepper.MaximumPosition;
         }
         else if (laserXChange < 0)
         {
            laserStepperXPosition = ParameterAccessor.Instance.LaserXStepper.MinimumPosition;
         }

         if (laserYChange > 0)
         {
            laserStepperYPosition = ParameterAccessor.Instance.LaserYStepper.MaximumPosition;
         }
         else if (laserYChange < 0)
         {
            laserStepperYPosition = ParameterAccessor.Instance.LaserYStepper.MinimumPosition;
         }

         if (targetChange > 0)
         {
            targetStepperPosition = ParameterAccessor.Instance.TargetStepper.MaximumPosition;
         }
         else if (targetChange < 0)
         {
            targetStepperPosition = ParameterAccessor.Instance.TargetStepper.MinimumPosition;
         }

         if (laserXChange != this.laserXRequestedChange)
         {
            if (0 != laserXChange)
            {
               DeviceCommunication.Instance.SetLaserStepperXPosition(laserStepperXPosition);
            }
            else
            {
               DeviceCommunication.Instance.StopLaserStepperX();
            }

            this.laserXRequestedChange = laserXChange;
         }

         if (laserYChange != this.laserYRequestedChange)
         {
            if (0 != laserYChange)
            {
               DeviceCommunication.Instance.SetLaserStepperYPosition(laserStepperYPosition);
            }
            else
            {
               DeviceCommunication.Instance.StopLaserStepperY();
            }

            this.laserYRequestedChange = laserYChange;
         }

         if (targetChange != this.targetRequestedChange)
         {
            if (0 != targetChange)
            {
               DeviceCommunication.Instance.SetTargetStepperPosition(targetStepperPosition);
            }
            else
            {
               DeviceCommunication.Instance.StopTargetStepper();
            }

            this.targetRequestedChange = targetChange;
         }

         #endregion

#if false
         // update readings

         #region Joystick and Movement

         Joystick.Instance.Update();

         double movementRequestValue = 0;
         ValueParameter movementParameter = null;
         NicBotComm.Instance.GetMovementRequestValues(ref movementParameter, ref movementRequestValue);

         if (false != Joystick.Instance.Valid)
         {
            if (false == this.joystickIsValid)
            {
               this.FeederSpeedValueButton.HoldTimeoutInterval = 0;
               this.joystickIsValid = true;
            }

         #region Y Axis Movement

            int yAxis = (int)(((Joystick.Instance.YAxis) - 32768) * -1);
            bool movementReverse = false;
            bool movementForward = false;
            int joystickDeadBand = ParameterAccessor.Instance.JoystickDeadband;
            int joystickRange;

            int yAxisPrevious = yAxis;

            if (yAxis > joystickDeadBand)
            {
               this.WheelSpeedToggleButton.Enabled = false;
               yAxis -= joystickDeadBand;
               joystickRange = (32768 - joystickDeadBand);
            }
            else if (yAxis < -joystickDeadBand)
            {
               this.WheelSpeedToggleButton.Enabled = false;
               yAxis += joystickDeadBand;
               joystickRange = (32767 - joystickDeadBand);
            }
            else
            {
               this.WheelSpeedToggleButton.Enabled = true;
               yAxis = 0;
               joystickRange = 1;
            }

            if (yAxis < 0)
            {
               movementReverse = true;
               movementForward = false;
            }
            else if (yAxis > 0)
            {
               movementReverse = false;
               movementForward = true;
            }

            // Tracer.WriteHigh(TraceGroup.GUI, "", "joystick {0} {1}", yAxisPrevious, yAxis); // todo add joystick group
            bool fastSelected = ((false != this.movementFastSelected) && (false == this.WheelCornerModeToggleButton.OptionASelected)) ? true : false;
            this.WheelSpeedToggleButton.OptionASelected = fastSelected;
            double movementScale = (false != fastSelected) ? 1.0 : ParameterAccessor.Instance.MovementMotorLowSpeedScale.OperationalValue / 100;
            double movementRequest = movementScale * yAxis / joystickRange;

            bool movementTriggered = ((false != Joystick.Instance.Button1Pressed) && (false == this.MovementManulPanel.Visible));
            NicBotComm.Instance.SetMovementRequest(movementRequest, movementTriggered);

            bool movementActivated = NicBotComm.Instance.GetMovementActivated();

            if (false != this.WheelMoveButton.Enabled)
            {
               double movementDisplayValue = Math.Abs(movementRequestValue);
               this.WheelMoveButton.LeftArrowVisible = movementReverse;
               this.WheelMoveButton.RightArrowVisible = movementForward;
               this.WheelMoveButton.ValueForeColor = (false != movementActivated) ? Color.White : Color.FromKnownColor(KnownColor.ControlDarkDark);
               this.WheelMoveButton.ValueText = this.GetValueText(movementDisplayValue, movementParameter);
            }
            else
            {
               this.WheelMoveButton.LeftArrowVisible = false;
               this.WheelMoveButton.RightArrowVisible = false;
               this.WheelMoveButton.ValueForeColor = Color.FromKnownColor(KnownColor.ControlDarkDark);
               this.WheelMoveButton.ValueText = this.GetValueText(0, movementParameter);
            }
         #endregion

         #region Joystick Idle

            int joystickYAxis = (int)(((Joystick.Instance.YAxis) - 32768) * -1);
            int joystickIdleBand = ParameterAccessor.Instance.JoystickIdleBand;
            bool joystickIdle = false;

            if ((joystickYAxis < joystickIdleBand) && (joystickYAxis > -joystickIdleBand))
            {
               joystickIdle = true;
            }

         #endregion

         #region Steering using Button 2 Trigger and Z Axis

            if (false != Joystick.Instance.Button2Pressed)
            {
               if (false == this.joystickButton2Activated)
               {
                  this.joystickButton2Activated = true;
                  this.joystickButton2ActivationTimeLimit = DateTime.Now.AddMilliseconds(500);
               }
            }
            else
            {
               if (false != this.joystickButton2Activated)
               {
                  this.joystickButton2Activated = false;

                  if (DateTime.Now < this.joystickButton2ActivationTimeLimit)
                  {
                     NicBotComm.Instance.SetMovementStraightOffset(0);
                  }
               }
            }

            if ((false != this.joystickButton2Activated) &&
                (DateTime.Now > this.joystickButton2ActivationTimeLimit))
            {
               double turnTicksPerDegree = ParameterAccessor.Instance.PositionerTurnOffset / 90;
               double zAxis = (int)(((Joystick.Instance.ZAxis) - 32768) * -1);
               int joystickZRange = 1;

               if (zAxis > joystickDeadBand)
               {
                  zAxis -= joystickDeadBand;
                  joystickZRange = (32768 - joystickDeadBand);
               }
               else if (zAxis < -joystickDeadBand)
               {
                  zAxis += joystickDeadBand;
                  joystickZRange = (32767 - joystickDeadBand);
               }
               else
               {
                  zAxis = 0;
                  joystickZRange = 1;
               }

               double straightOffset = (zAxis / joystickZRange) * turnTicksPerDegree * ParameterAccessor.Instance.PositionerStraightSwingDegrees;
               NicBotComm.Instance.SetMovementStraightOffset(straightOffset);
            }

         #endregion

         #region Acquire Stress (BARK) using Button 3 Trigger

            if (RobotApplications.inspect == ParameterAccessor.Instance.RobotApplication)
            {
               if (false != Joystick.Instance.Button3Pressed)
               {
                  if (false == this.joystickButton3Activated)
                  {
                     this.SensorAcquireStressButton_Click(this, EventArgs.Empty);
                     this.joystickButton3Activated = true;
                  }
               }
               else
               {
                  if (false != this.joystickButton3Activated)
                  {
                     this.joystickButton3Activated = false;
                  }
               }
            }

         #endregion

         #region Acquire Thickness (EMAT) using Button 4 Trigger

            if (RobotApplications.inspect == ParameterAccessor.Instance.RobotApplication)
            {
               if (false != Joystick.Instance.Button4Pressed)
               {
                  if (false == this.joystickButton4Activated)
                  {
                     this.SensorAcquireThicknessButton_Click(this, EventArgs.Empty);
                     this.joystickButton4Activated = true;
                  }
               }
               else
               {
                  if (false != this.joystickButton4Activated)
                  {
                     this.joystickButton4Activated = false;
                  }
               }
            }

         #endregion

         #region Straight and Turn Select Using Button 6 Trigger

            if ((false != joystickIdle) &&
                (false != Joystick.Instance.Button1Pressed) &&
                (false != Joystick.Instance.Button6Pressed))
            {
               if (false == this.joystickButton6Activated)
               {
                  NicBotComm.Instance.TriggerMacro(SystemMacros.toggleWheelPosition);
                  this.joystickButton6Activated = true;
                  this.joystickButton9PreviousMode = AcrobaticWheelModes.unknown;
               }
            }
            else
            {
               if ((false == Joystick.Instance.Button6Pressed) &&
                   (false != this.joystickButton6Activated))
               {
                  this.joystickButton6Activated = false;
               }
            }

         #endregion

         #region Sensor Block Toggle using Buttons 7 and 8

            if (RobotApplications.inspect == ParameterAccessor.Instance.RobotApplication)
            {
               if ((false != Joystick.Instance.Button7Pressed) &&
                   (false != Joystick.Instance.Button8Pressed))
               {
                  if (false == this.joystickSensorBlockActivated)
                  {
                     this.SensorBlockButton_Click(this, EventArgs.Empty);
                     this.joystickSensorBlockActivated = true;
                  }
               }
               else
               {
                  if (false != this.joystickSensorBlockActivated)
                  {
                     this.joystickSensorBlockActivated = false;
                  }
               }
            }

         #endregion

         #region Lower Pivot Selection using Button 9

            if (false != joystickIdle)
            {
               if (false != Joystick.Instance.Button9Pressed)
               {
                  if (false == this.joystickButton9Activated)
                  {
                     this.joystickButton9Activated = true;

                     if ((false != this.WheelLowerPivotModeButton.Enabled) &&
                         (AcrobaticWheelModes.lowerPivot != this.acrobaticWheelMode))
                     {
                        this.robotWheelConfiguration.StoreConfiguration();
                        this.joystickButton9PreviousMode = this.acrobaticWheelMode;
                        this.WheelLowerPivotModeButton_Click(this, EventArgs.Empty);
                     }
                  }
               }
               else
               {
                  if (false != this.joystickButton9Activated)
                  {
                     this.joystickButton9Activated = false;

                     if (AcrobaticWheelModes.unknown != this.joystickButton9PreviousMode)
                     {
                        this.robotWheelConfiguration.RestoreConfiguration();
                        this.HighlightSelectedWheelMode();
                     }

                     this.joystickButton9PreviousMode = AcrobaticWheelModes.unknown;
                  }
               }
            }

         #endregion

         #region Sensor Flap Toggle using Button 10

            if (RobotApplications.inspect == ParameterAccessor.Instance.RobotApplication)
            {
               if (false != Joystick.Instance.Button10Pressed)
               {
                  if (false == this.joystickButton10Activated)
                  {
                     this.SensorFlapsButton_Click(this, EventArgs.Empty);
                     this.joystickButton10Activated = true;
                  }
               }
               else
               {
                  if (false != this.joystickButton10Activated)
                  {
                     this.joystickButton10Activated = false;
                  }
               }
            }

         #endregion

         #region Feeder In using Button 11

            if (false != Joystick.Instance.Button11Pressed)
            {
               if (false == this.joystickButton11Pressed)
               {
                  this.joystickButton11Pressed = true;
                  this.joystickButton11ActivationTimeLimit = DateTime.Now.AddMilliseconds(500);
               }
               else if (false == this.joystickButton11Activated)
               {
                  if (DateTime.Now > this.joystickButton11ActivationTimeLimit)
                  {
                     this.joystickButton11Activated = true;

                     if (false != this.FeederManualPanel.Visible)
                     {
                        this.FeederManualInButton_MouseDown(this, null);
                     }
                  }
               }
            }
            else
            {
               if (false != this.joystickButton11Pressed)
               {
                  this.joystickButton11Pressed = false;
                  this.joystickButton11Activated = false;

                  if (false != this.FeederManualPanel.Visible)
                  {
                     this.FeederManualInButton_MouseUp(this, null);
                  }
               }
            }

         #endregion

         #region Feeder Out using Button 12

            if (false != Joystick.Instance.Button12Pressed)
            {
               if (false == this.joystickButton12Pressed)
               {
                  this.joystickButton12Pressed = true;
                  this.joystickButton12ActivationTimeLimit = DateTime.Now.AddMilliseconds(500);
               }
               else if (false == this.joystickButton12Activated)
               {
                  if (DateTime.Now > this.joystickButton12ActivationTimeLimit)
                  {
                     this.joystickButton12Activated = true;

                     if (false != this.FeederManualPanel.Visible)
                     {
                        this.FeederManualOutButton_MouseDown(this, null);
                     }
                  }
               }
            }
            else
            {
               if (false != this.joystickButton12Pressed)
               {
                  this.joystickButton12Pressed = false;
                  this.joystickButton12Activated = false;

                  if (false != this.FeederManualPanel.Visible)
                  {
                     this.FeederManualOutButton_MouseUp(this, null);
                  }
               }
            }

         #endregion

         #region Trottle Axis Feeder

            int joystickThrottle = (int)(65535 - Joystick.Instance.Throttle);

            double feederRequest = 0;
            bool feederAutomaticTracking = this.GetAutomaticFeederTracking();


            if (false != feederAutomaticTracking)
            {
               feederRequest = movementRequestValue * ParameterAccessor.Instance.FeederTrackingCalibration.OperationalValue / 100;
            }
            else
            {
               double feederMaxSpeed = ParameterAccessor.Instance.FeederMaxSpeed.OperationalValue;
               feederRequest = feederMaxSpeed * joystickThrottle / 65535;

               if (feederRequest < ParameterAccessor.Instance.FeederMaxSpeed.StepValue)
               {
                  feederRequest = 0;
               }

               if (false != movementReverse)
               {
                  feederRequest *= -1;
               }
            }


            bool directionSelected = (false != movementReverse) || (false != movementForward);
            bool feederMoveEnabled = false;

            if ((NicBotComm.Instance.GetFeederMode() == FeederModes.move) && (false == this.FeederManualPanel.Visible) && (false != movementActivated))
            {
               feederMoveEnabled = true;
            }

            if (false != this.FeederMoveButton.Enabled)
            {
               double feederDisplayValue = Math.Abs(feederRequest);
               this.FeederMoveButton.LeftArrowVisible = movementReverse;
               this.FeederMoveButton.RightArrowVisible = movementForward;
               this.FeederMoveButton.ValueForeColor = (false != feederMoveEnabled) ? Color.White : Color.FromKnownColor(KnownColor.ControlDarkDark);
               this.FeederMoveButton.ValueText = this.GetValueText(feederDisplayValue, ParameterAccessor.Instance.FeederMaxSpeed);
            }
            else
            {
               this.FeederMoveButton.LeftArrowVisible = false;
               this.FeederMoveButton.RightArrowVisible = false;
               this.FeederMoveButton.ValueForeColor = Color.FromKnownColor(KnownColor.ControlDarkDark);
               this.FeederMoveButton.ValueText = this.GetValueText(0, ParameterAccessor.Instance.FeederMaxSpeed);
            }

            if (false != feederMoveEnabled)
            {
               if (feederRequest != this.feederPreviousRequest)
               {
                  this.feederPreviousRequest = feederRequest;
                  NicBotComm.Instance.SetFeederVelocity(feederRequest);
               }
            }
            else
            {
               if (0 != this.feederPreviousRequest)
               {
                  this.feederPreviousRequest = 0;
                  NicBotComm.Instance.SetFeederVelocity(0);
               }
            }

            if (false != this.FeederManualPanel.Visible)
            {
               double feederMaxSpeed = ParameterAccessor.Instance.FeederMaxSpeed.OperationalValue;
               double feederManualRequest = feederMaxSpeed * joystickThrottle / 65535;
               bool feederManualMoveActive = false;

               if (feederManualRequest < ParameterAccessor.Instance.FeederMaxSpeed.StepValue)
               {
                  feederManualRequest = 0;
               }

               this.FeederSpeedValueButton.ValueText = this.GetValueText(feederManualRequest, ParameterAccessor.Instance.FeederManualSpeed);

               if (this.feederManualSelection == FeederSelection.goingIn)
               {
                  feederManualRequest *= -1;
                  feederManualMoveActive = true;
               }
               else if (this.feederManualSelection == FeederSelection.goingOut)
               {
                  feederManualMoveActive = true;
               }
               else
               {
                  feederManualRequest = 0;
               }

               if (this.feederPreviousManualRequest != feederManualRequest)
               {
                  this.feederPreviousManualRequest = feederManualRequest;

                  if (false != feederManualMoveActive)
                  {
                     NicBotComm.Instance.SetFeederVelocity(feederManualRequest);
                     Tracer.WriteHigh(TraceGroup.GUI, null, "feeder throttle request {0}", feederManualRequest);
                  }
               }
            }

         #endregion

         #region Camera Select Using POV

            if (false != Joystick.Instance.PovPressed)
            {
               if (false == this.joystickPovActivated)
               {
                  this.joystickPovActivated = true;
                  int povValue = Joystick.Instance.PovValue;

                  if (90 == povValue)
                  {
                     this.AssignNextCamera(true);
                  }
                  else if (270 == povValue)
                  {
                     this.AssignNextCamera(false);
                  }
               }
            }
            else
            {
               this.joystickPovActivated = false;
            }

         #endregion

         }
         else
         {
            if (false != this.joystickIsValid)
            {
               this.joystickIsValid = false;

               this.FeederSpeedValueButton.HoldTimeoutInterval = 100;
               this.FeederSpeedValueButton.ValueText = this.GetValueText(ParameterAccessor.Instance.FeederManualSpeed);
               //this.FeederSpeedValueButton.Invalidate();
            }
         }

         double movementValue = NicBotComm.Instance.GetMovementValue();
         double movementStatusDisplayValue = Math.Abs(movementValue);

         if (movementStatusDisplayValue < movementParameter.StepValue)
         {
            movementValue = 0;
            movementStatusDisplayValue = 0;
         }

         if (movementValue > 0)
         {
            this.MotorStatusDirectionalValuePanel.Direction = DirectionalValuePanel.Directions.Forward;
         }
         else if (movementValue < 0)
         {
            this.MotorStatusDirectionalValuePanel.Direction = DirectionalValuePanel.Directions.Reverse;
         }
         else
         {
            this.MotorStatusDirectionalValuePanel.Direction = DirectionalValuePanel.Directions.Idle;
         }

         this.MotorStatusDirectionalValuePanel.ValueText = this.GetValueText(movementStatusDisplayValue, movementParameter);


         double maximumMovementCurrent = 0;
         double movementCurrent = 0;
         double maximumMovementTemperature = 0;
         double movementTemperature = 0;
         Color wheelIndicatorForeColor = Color.White;
         Color wheelIndicatorBackColor = Color.Black;

         MovementWheelModes requestedMovementWheelMode = NicBotComm.Instance.GetRequestedMovementWheelMode();
         ValueParameter tickToDistance = ParameterAccessor.Instance.MovementMotorTurnTickToDistance;
         double movementMotorDistance;

         if (MovementWheelModes.straight == requestedMovementWheelMode)
         {
            tickToDistance = ParameterAccessor.Instance.MovementMotorStraightTickToDistance;
         }

         double positionerTurnOffset = ParameterAccessor.Instance.PositionerTurnOffset;
         double positionerPosition = 0;


         this.GetWheelIndicatorColor(WheelLocations.frontUpper, ref wheelIndicatorForeColor, ref wheelIndicatorBackColor);

         movementCurrent = NicBotComm.Instance.GetWheelCurrent(WheelLocations.frontUpper);
         maximumFeederCurrent = (movementCurrent > maximumMovementCurrent) ? movementCurrent : maximumMovementCurrent;
         this.FrontUpperMovementMotorCurrentTextPanel.ValueText = this.GetValueText(movementCurrent, "N2", "A");
         this.FrontUpperMovementMotorCurrentTextPanel.ForeColor = wheelIndicatorForeColor;
         this.FrontUpperMovementMotorCurrentTextPanel.BackColor = wheelIndicatorBackColor;

         movementTemperature = NicBotComm.Instance.GetWheelTemperature(WheelLocations.frontUpper);
         maximumMovementTemperature = (movementTemperature > maximumMovementTemperature) ? movementTemperature : maximumMovementTemperature;
         this.FrontUpperMovementMotorTemperatureTextPanel.ValueText = this.GetValueText(movementTemperature, "N1", "°C");
         this.FrontUpperMovementMotorTemperatureTextPanel.ForeColor = wheelIndicatorForeColor;
         this.FrontUpperMovementMotorTemperatureTextPanel.BackColor = wheelIndicatorBackColor;

         movementMotorDistance = NicBotComm.Instance.GetWheelPosition(WheelLocations.frontUpper) * tickToDistance.OperationalValue;
         this.FrontUpperMovementMotorPositionTextPanel.ValueText = this.GetValueText(movementMotorDistance, tickToDistance);
         this.FrontUpperMovementMotorPositionTextPanel.ForeColor = wheelIndicatorForeColor;
         this.FrontUpperMovementMotorPositionTextPanel.BackColor = wheelIndicatorBackColor;

         positionerPosition = NicBotComm.Instance.GetWheelRotation(WheelLocations.frontUpper) * 90 / positionerTurnOffset;
         this.FrontUpperMovementMotorRotationTextPanel.ValueText = this.GetValueText(positionerPosition, "N1", "°");
         this.FrontUpperMovementMotorRotationTextPanel.ForeColor = wheelIndicatorForeColor;
         this.FrontUpperMovementMotorRotationTextPanel.BackColor = wheelIndicatorBackColor;


         this.GetWheelIndicatorColor(WheelLocations.frontLower, ref wheelIndicatorForeColor, ref wheelIndicatorBackColor);

         movementCurrent = NicBotComm.Instance.GetWheelCurrent(WheelLocations.frontLower);
         maximumMovementCurrent = (movementCurrent > maximumMovementCurrent) ? movementCurrent : maximumMovementCurrent;
         this.FrontLowerMovementMotorCurrentTextPanel.ValueText = this.GetValueText(movementCurrent, "N2", "A");
         this.FrontLowerMovementMotorCurrentTextPanel.ForeColor = wheelIndicatorForeColor;
         this.FrontLowerMovementMotorCurrentTextPanel.BackColor = wheelIndicatorBackColor;

         movementTemperature = NicBotComm.Instance.GetWheelTemperature(WheelLocations.frontLower);
         maximumMovementTemperature = (movementTemperature > maximumMovementTemperature) ? movementTemperature : maximumMovementTemperature;
         this.FrontLowerMovementMotorTemperatureTextPanel.ValueText = this.GetValueText(movementTemperature, "N1", "°C");
         this.FrontLowerMovementMotorTemperatureTextPanel.ForeColor = wheelIndicatorForeColor;
         this.FrontLowerMovementMotorTemperatureTextPanel.BackColor = wheelIndicatorBackColor;

         movementMotorDistance = NicBotComm.Instance.GetWheelPosition(WheelLocations.frontLower) * tickToDistance.OperationalValue;
         this.FrontLowerMovementMotorPositionTextPanel.ValueText = this.GetValueText(movementMotorDistance, tickToDistance);
         this.FrontLowerMovementMotorPositionTextPanel.ForeColor = wheelIndicatorForeColor;
         this.FrontLowerMovementMotorPositionTextPanel.BackColor = wheelIndicatorBackColor;

         positionerPosition = NicBotComm.Instance.GetWheelRotation(WheelLocations.frontLower) * 90 / positionerTurnOffset;
         this.FrontLowerMovementMotorRotationTextPanel.ValueText = this.GetValueText(positionerPosition, "N1", "°");
         this.FrontLowerMovementMotorRotationTextPanel.ForeColor = wheelIndicatorForeColor;
         this.FrontLowerMovementMotorRotationTextPanel.BackColor = wheelIndicatorBackColor;


         this.GetWheelIndicatorColor(WheelLocations.rearUpper, ref wheelIndicatorForeColor, ref wheelIndicatorBackColor);

         movementCurrent = NicBotComm.Instance.GetWheelCurrent(WheelLocations.rearUpper);
         maximumMovementCurrent = (movementCurrent > maximumMovementCurrent) ? movementCurrent : maximumMovementCurrent;
         this.RearUpperMovementMotorCurrentTextPanel.ValueText = this.GetValueText(movementCurrent, "N2", "A");
         this.RearUpperMovementMotorCurrentTextPanel.ForeColor = wheelIndicatorForeColor;
         this.RearUpperMovementMotorCurrentTextPanel.BackColor = wheelIndicatorBackColor;

         movementTemperature = NicBotComm.Instance.GetWheelTemperature(WheelLocations.rearUpper);
         maximumMovementTemperature = (movementTemperature > maximumMovementTemperature) ? movementTemperature : maximumMovementTemperature;
         this.RearUpperMovementMotorTemperatureTextPanel.ValueText = this.GetValueText(movementTemperature, "N1", "°C");
         this.RearUpperMovementMotorTemperatureTextPanel.ForeColor = wheelIndicatorForeColor;
         this.RearUpperMovementMotorTemperatureTextPanel.BackColor = wheelIndicatorBackColor;

         movementMotorDistance = NicBotComm.Instance.GetWheelPosition(WheelLocations.rearUpper) * tickToDistance.OperationalValue;
         this.RearUpperMovementMotorPositionTextPanel.ValueText = this.GetValueText(movementMotorDistance, tickToDistance);
         this.RearUpperMovementMotorPositionTextPanel.ForeColor = wheelIndicatorForeColor;
         this.RearUpperMovementMotorPositionTextPanel.BackColor = wheelIndicatorBackColor;

         positionerPosition = NicBotComm.Instance.GetWheelRotation(WheelLocations.rearUpper) * 90 / positionerTurnOffset;
         this.RearUpperMovementMotorRotationTextPanel.ValueText = this.GetValueText(positionerPosition, "N1", "°");
         this.RearUpperMovementMotorRotationTextPanel.ForeColor = wheelIndicatorForeColor;
         this.RearUpperMovementMotorRotationTextPanel.BackColor = wheelIndicatorBackColor;


         this.GetWheelIndicatorColor(WheelLocations.rearLower, ref wheelIndicatorForeColor, ref wheelIndicatorBackColor);

         movementCurrent = NicBotComm.Instance.GetWheelCurrent(WheelLocations.rearLower);
         maximumMovementCurrent = (movementCurrent > maximumMovementCurrent) ? movementCurrent : maximumMovementCurrent;
         this.RearLowerMovementMotorCurrentTextPanel.ValueText = this.GetValueText(movementCurrent, "N2", "A");
         this.RearLowerMovementMotorCurrentTextPanel.ForeColor = wheelIndicatorForeColor;
         this.RearLowerMovementMotorCurrentTextPanel.BackColor = wheelIndicatorBackColor;

         movementTemperature = NicBotComm.Instance.GetWheelTemperature(WheelLocations.rearLower);
         maximumMovementTemperature = (movementTemperature > maximumMovementTemperature) ? movementTemperature : maximumMovementTemperature;
         this.RearLowerMovementMotorTemperatureTextPanel.ValueText = this.GetValueText(movementTemperature, "N1", "°C");
         this.RearLowerMovementMotorTemperatureTextPanel.ForeColor = wheelIndicatorForeColor;
         this.RearLowerMovementMotorTemperatureTextPanel.BackColor = wheelIndicatorBackColor;

         movementMotorDistance = NicBotComm.Instance.GetWheelPosition(WheelLocations.rearLower) * tickToDistance.OperationalValue;
         this.RearLowerMovementMotorPositionTextPanel.ValueText = this.GetValueText(movementMotorDistance, tickToDistance);
         this.RearLowerMovementMotorPositionTextPanel.ForeColor = wheelIndicatorForeColor;
         this.RearLowerMovementMotorPositionTextPanel.BackColor = wheelIndicatorBackColor;

         positionerPosition = NicBotComm.Instance.GetWheelRotation(WheelLocations.rearLower) * 90 / positionerTurnOffset;
         this.RearLowerMovementMotorRotationTextPanel.ValueText = this.GetValueText(positionerPosition, "N1", "°");
         this.RearLowerMovementMotorRotationTextPanel.ForeColor = wheelIndicatorForeColor;
         this.RearLowerMovementMotorRotationTextPanel.BackColor = wheelIndicatorBackColor;


         this.SetCautionPanel(maximumMovementCurrent, ParameterAccessor.Instance.MovementCurrentCaution, this.MovementCurrentIndciatorTextBox);
         this.SetCautionPanel(maximumMovementTemperature, ParameterAccessor.Instance.MovementTemperatureCaution, this.MovementTemperatureIndicatorTextBox);

         #endregion

         #region Index Position

         #endregion

         #region Robot Status

         if (false != this.robotMainSolenoidControlFault)
         {
            this.robotMainSolenoidControlFault = false;

            SolenoidControlFault solenoidControlFault = new SolenoidControlFault();
            this.SetDialogLocation(this, solenoidControlFault);
            this.DimBackground();
            solenoidControlFault.ShowDialog();
            this.LightBackground();
         }

         this.UpdateBodyControls();
         this.UpdatePipePositionDisplay();
         this.UpdateMovementControls();

         if (false != this.messageFlasher)
         {
            bool sensorBlockOut = NicBotComm.Instance.GetSolenoidActive(Solenoids.sensorBlockOut);
            bool sensorBlockIn = NicBotComm.Instance.GetSolenoidActive(Solenoids.sensorBlockIn);
            bool sensorBlockRetracted = ((false != sensorBlockIn) && (false == sensorBlockOut)) ? true : false;
            bool movementTriggered = ((false != Joystick.Instance.Button1Pressed) && (false == this.MovementManulPanel.Visible));

            if ((RobotApplications.inspect == ParameterAccessor.Instance.RobotApplication) &&
                (false == sensorBlockRetracted) &&
                (false != movementTriggered))
            {
               this.RobotPositionWarningLabel.ForeColor = Color.Yellow;
               this.RobotPositionWarningLabel.Text = "!!! RETRACT SENSOR BLOCK !!!";
               this.RobotPositionWarningLabel.Visible = true;
            }
            else
            {
               MovementForwardModes movementForwardMode = NicBotComm.Instance.GetMovementForwardMode();
               bool lowerArmsExtended = NicBotComm.Instance.GetSolenoidActive(Solenoids.lowerArmExtend);
               bool lowerArmsRetracted = NicBotComm.Instance.GetSolenoidActive(Solenoids.lowerArmRetract);
               bool torsoRaised = ((false != lowerArmsExtended) && (false == lowerArmsRetracted)) ? true : false;

               if ((MovementForwardModes.cornerStraight == movementForwardMode) &&
                   (false == torsoRaised))
               {
                  this.RobotPositionWarningLabel.ForeColor = Color.Yellow;
                  this.RobotPositionWarningLabel.Text = "!!! RAISE TORSO !!!";
                  this.RobotPositionWarningLabel.Visible = true;
               }
               else
               {
                  bool sensorActive = NicBotComm.Instance.GetSensorIndicatorStatus(SensorIndicatorLocations.lowerLeft);

                  if (false == sensorActive)
                  {
                     this.RobotPositionWarningLabel.ForeColor = Color.Yellow;
                     this.RobotPositionWarningLabel.Text = "!!! SENSOR NOT CLEAR !!!";
                     this.RobotPositionWarningLabel.Visible = true;
                  }
               }
            }
         }
         else
         {
            this.RobotPositionWarningLabel.Visible = false;
         }

         #endregion

         #region Drill Status

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            double drillIndexSetPoint = NicBotComm.Instance.GetDrillIndexSetPoint(this.toolLocation) * -1;
            double actualDrillIndexPosition = NicBotComm.Instance.GetDrillIndexPosition(this.toolLocation) * -1;
            bool atRetractionLimit = (0 == actualDrillIndexPosition) ? true : false;
            double drillOriginOffset = NicBotComm.Instance.GetDrillOriginOffset(this.toolLocation);

            if (double.IsNaN(drillOriginOffset) == false)
            {
               drillIndexSetPoint += drillOriginOffset;
               actualDrillIndexPosition += drillOriginOffset;
               this.OriginSetLightTextBox.BackColor = Color.Black;
               this.DrillMoveToOriginButton.Enabled = true;
            }
            else
            {
               this.OriginSetLightTextBox.BackColor = Color.Red;
               this.DrillMoveToOriginButton.Enabled = false;
            }

            this.DrillExtendedActualValuePanel.ValueText = this.GetValueText(actualDrillIndexPosition, ParameterAccessor.Instance.FrontDrill.ExtendedDistance);
            this.DrillExtendedSetPointValuePanel.ValueText = this.GetValueText(drillIndexSetPoint, ParameterAccessor.Instance.FrontDrill.ExtendedDistance);
            this.DrillRotationActualSpeedValuePanel.ValueText = this.GetValueText(NicBotComm.Instance.GetDrillRotationSpeed(this.toolLocation), ParameterAccessor.Instance.FrontDrill.RotationSpeed);

            if (false != atRetractionLimit)
            {
               this.RetractionLimitLightTextBox.BackColor = Color.Yellow;
               this.DrillSealModeButton.Enabled = true;
            }
            else
            {
               this.RetractionLimitLightTextBox.BackColor = Color.Black;
               this.DrillSealModeButton.Enabled = false;
            }

            this.DrillErrorLightTextBox.BackColor = (NicBotComm.Instance.GetDrillError(this.toolLocation) != false) ? Color.Red : Color.Black;

            this.UpdateDrillControls();
         }

         #endregion

         #region Sealant and Nozzle Status

         if (RobotApplications.repair == ParameterAccessor.Instance.RobotApplication)
         {
            bool nozzleExtended = NicBotComm.Instance.GetNozzleExtended(this.toolLocation);
            bool pumpSetPointActive = NicBotComm.Instance.GetPumpActivity(this.toolLocation);
            PumpAutoStates pumpAutoState = NicBotComm.Instance.GetPumpAutoState(this.toolLocation);

            bool nozzleError = (false != pumpSetPointActive) && (PumpAutoStates.running != pumpAutoState) && (false == nozzleExtended);

            this.SealantNozzlePositionTextPanel.BackColor = (false == nozzleError) ? Color.FromArgb(51, 51, 51) : Color.Red;
            this.SealantNozzlePositionTextPanel.ValueText = (false != nozzleExtended) ? "EXTENDED" : "RETRACTED";
            this.SealantNozzleToggleButton.OptionASelected = nozzleExtended;

            PumpParameters pumpParameters = (ToolLocations.front == this.toolLocation) ? ParameterAccessor.Instance.FrontPump : ParameterAccessor.Instance.RearPump;

            double sealantReserviorReading = NicBotComm.Instance.GetReserviorWeightReading(this.toolLocation);

            if (double.IsNaN(sealantReserviorReading) == false)
            {
               sealantReserviorReading *= 1000;
               this.SealantReserviorTextPanel.ValueText = this.GetValueText(sealantReserviorReading, "N0", "g");
            }
            else
            {
               this.SealantReserviorTextPanel.ValueText = "---";
            }

            double pumpVolumePerMinute = NicBotComm.Instance.GetPumpVolumePerSecond(this.toolLocation) * 60;
            this.SealantFlowRateTextPanel.ValueText = this.GetValueText((int)pumpVolumePerMinute, "mL/M");

            double pumpPressureReading = NicBotComm.Instance.GetPumpPressureReading(this.toolLocation);
            double pumpVolumeMeasure = NicBotComm.Instance.GetPumpVolumeMeasure(this.toolLocation);
            double pumpSpeedReading = NicBotComm.Instance.GetPumpSpeedReading(this.toolLocation);

            this.SealantActualPressureValuePanel.ValueText = this.GetValueText(pumpPressureReading, pumpParameters.MaximumPressure);
            this.SealantActualVolumeValuePanel.ValueText = this.GetValueText(pumpVolumeMeasure, pumpParameters.MaximumVolume);
            this.SealantActualSpeedValuePanel.ValueText = this.GetValueText(pumpSpeedReading, pumpParameters.MaximumSpeed);

            this.SealantActualVolumeValuePanel.ValueText = this.GetValueText(pumpVolumeMeasure, pumpParameters.MaximumVolume);

            PumpModes pumpMode = NicBotComm.Instance.GetPumpMode(this.toolLocation);
            double pumpSetPoint = NicBotComm.Instance.GetPumpSetPoint(this.toolLocation);

            if (PumpModes.pressure == pumpMode)
            {
               this.SealantPressureSetPointValuePanel.ForeColor = (false != pumpSetPointActive) ? Color.White : Color.Gray;
               this.SealantPressureSetPointValuePanel.ValueText = this.GetValueText(pumpSetPoint, pumpParameters.MaximumPressure);

               this.SealantVolumeSetPointValuePanel.ForeColor = Color.Black;
               this.SealantSpeedSetPointValuePanel.ForeColor = Color.Black;
            }
            else if (PumpModes.volume == pumpMode)
            {
               this.SealantVolumeSetPointValuePanel.ForeColor = (false != pumpSetPointActive) ? Color.White : Color.Gray;
               this.SealantVolumeSetPointValuePanel.ValueText = this.GetValueText(pumpSetPoint, pumpParameters.MaximumVolume);

               this.SealantPressureSetPointValuePanel.ForeColor = Color.Black;
               this.SealantSpeedSetPointValuePanel.ForeColor = Color.Black;
            }
            else if (PumpModes.speed == pumpMode)
            {
               PumpDirections pumpDirection = NicBotComm.Instance.GetPumpDirection(this.toolLocation);
               double pumpDisplaySetPoint = (PumpDirections.forward == pumpDirection) ? pumpSetPoint : -pumpSetPoint;

               this.SealantSpeedSetPointValuePanel.ForeColor = (false != pumpSetPointActive) ? Color.White : Color.Gray;
               this.SealantSpeedSetPointValuePanel.ValueText = this.GetValueText(pumpDisplaySetPoint, pumpParameters.MaximumSpeed);

               this.SealantPressureSetPointValuePanel.ForeColor = Color.Black;
               this.SealantVolumeSetPointValuePanel.ForeColor = Color.Black;
            }

            this.UpdateSealantControls();
         }

         #endregion

         #region Extention Status

         if (RobotApplications.inspect == ParameterAccessor.Instance.RobotApplication)
         {
         }

         #endregion

         #region Sensor Status

         if (RobotApplications.inspect == ParameterAccessor.Instance.RobotApplication)
         {
            double latitude = NicBotComm.Instance.GetGpsLatitude();
            if (double.IsNaN(latitude) == false)
            {
               ParameterAccessor.Instance.Latitude = latitude;
               this.SensorLatitudeTextPanel.ValueText = latitude.ToString("N4");
               this.SensorLatitudeTextPanel.ForeColor = Color.Silver;
               this.SensorLatitudeTextPanel.BackColor = Color.FromArgb(51, 51, 51);
            }
            else
            {
               if (double.IsNaN(ParameterAccessor.Instance.Latitude) == false)
               {
                  this.SensorLatitudeTextPanel.ForeColor = Color.Black;
                  this.SensorLatitudeTextPanel.BackColor = Color.Yellow;
               }
               else
               {
                  this.SensorLatitudeTextPanel.ForeColor = Color.Black;
                  this.SensorLatitudeTextPanel.BackColor = Color.Red;
               }
            }

            double longitude = NicBotComm.Instance.GetGpsLongitude();
            if (double.IsNaN(latitude) == false)
            {
               ParameterAccessor.Instance.Longitude = longitude;
               this.SensorLongitudeTextPanel.ValueText = longitude.ToString("N4");
               this.SensorLongitudeTextPanel.ForeColor = Color.Silver;
               this.SensorLongitudeTextPanel.BackColor = Color.FromArgb(51, 51, 51);
            }
            else
            {
               if (double.IsNaN(ParameterAccessor.Instance.Longitude) == false)
               {
                  this.SensorLongitudeTextPanel.ForeColor = Color.Black;
                  this.SensorLongitudeTextPanel.BackColor = Color.Yellow;
               }
               else
               {
                  this.SensorLongitudeTextPanel.ForeColor = Color.Black;
                  this.SensorLongitudeTextPanel.BackColor = Color.Red;
               }
            }

            DateTime dateTime = NicBotComm.Instance.GetGpsTime();
            if (dateTime.Year < 2000)
            {
               dateTime = DateTime.Now;
            }
            else
            {
               dateTime = dateTime.ToLocalTime();
            }

            this.SensorGpsDateTextPanel.ValueText = string.Format("{0:D2}-{1:D2}-{2:D4}", dateTime.Month, dateTime.Day, dateTime.Year);
            this.SensorGpsTimeTextPanel.ValueText = string.Format("{0:D2}:{1:D2}:{2:D2}", dateTime.Hour, dateTime.Minute, dateTime.Second);

            this.SensorPipeDisplacementTextPanel.ValueText = this.GetValueText(tetherTotalDistance, ParameterAccessor.Instance.TetherDistanceScale);

            this.SetSensorButtonState(this.SensorScrapperButton, Solenoids.sensorScraperExtend, Solenoids.sensorScraperRetract, "SCRAPER EXTENDED", "SCRAPER RETRACTED", "SCRAPER NOT SET");
            this.SetSensorButtonState(this.SensorFlapsButton, Solenoids.sensorFlapsExtend, Solenoids.sensorFlapsRetract, "FLAPS EXTENDED", "FLAPS RETRACTED", "FLAPS      NOT SET");
            this.SetSensorBlockButtonState();
            this.SetSensorButtonState(this.SensorScraperAirButton, Solenoids.sensorScraperAirBlast, Solenoids.none, "SCRAPER AIR BLAST ON", "SCRAPER AIR BLAST OFF", "");
            this.SetSensorButtonState(this.SensorFlapsAirButton, Solenoids.sensorFlapsAirBlast, Solenoids.none, "FLAPS        AIR BLAST ON", "FLAPS        AIR BLAST OFF", "");

            if (NicBotComm.Instance.GetRobotMainFaulted() == false)
            {
               this.SensorUpperLeftIndicatorTextBox.BackColor = (NicBotComm.Instance.GetSensorIndicatorStatus(SensorIndicatorLocations.upperLeft) != false) ? Color.Lime : Color.Black;
               this.SensorUpperRightIndicatorTextBox.BackColor = (NicBotComm.Instance.GetSensorIndicatorStatus(SensorIndicatorLocations.upperRight) != false) ? Color.Lime : Color.Black;
               this.SensorLowerLeftIndicatorTextBox.BackColor = (NicBotComm.Instance.GetSensorIndicatorStatus(SensorIndicatorLocations.lowerLeft) != false) ? Color.Lime : Color.Black;
               this.SensorLowerRightndicatorTextBox.BackColor = (NicBotComm.Instance.GetSensorIndicatorStatus(SensorIndicatorLocations.lowerRight) != false) ? Color.Lime : Color.Black;
            }
            else
            {
               this.SensorUpperLeftIndicatorTextBox.BackColor = Color.Red;
               this.SensorUpperRightIndicatorTextBox.BackColor = Color.Red;
               this.SensorLowerLeftIndicatorTextBox.BackColor = Color.Red;
               this.SensorLowerRightndicatorTextBox.BackColor = Color.Red;
            }


            bool sensorLocationValid = true;

            if ((double.IsNaN(ParameterAccessor.Instance.Latitude) != false) ||
                (double.IsNaN(ParameterAccessor.Instance.Longitude) != false) ||
                (this.sensorDirection == Directions.unknown))
            {
               sensorLocationValid = false;
            }


            double sensorTemperatureReading = NicBotComm.Instance.GetSensorTemperature();

            if (double.IsNaN(sensorTemperatureReading) == false)
            {
               this.SensorSensorTemperatureTextPanel.BackColor = Color.Black;
               this.SensorSensorTemperatureTextPanel.ValueText = this.GetValueText(sensorTemperatureReading, ParameterAccessor.Instance.SensorTemperatureConversionUnit);
            }
            else
            {
               this.SensorSensorTemperatureTextPanel.BackColor = Color.Red;
               this.SensorSensorTemperatureTextPanel.ValueText = "---";
            }


            if (false != ParameterAccessor.Instance.SensorEmatEnable)
            {
               double ematCoilTemperatureReading = NicBotComm.Instance.GetSensorEmatCoilTemperature();

               if (double.IsNaN(ematCoilTemperatureReading) == false)
               {
                  this.SensorEmatCoilTemperatureTextPanel.ValueText = this.GetValueText(ematCoilTemperatureReading, ParameterAccessor.Instance.SensorEmatCoilTemperationConversionUnit);
                  this.SetCautionPanel(ematCoilTemperatureReading, ParameterAccessor.Instance.SensorEmatCoilTemperatureCaution, this.SensorEmatCoilTemperatureTextPanel);
               }
               else
               {
                  this.SensorEmatCoilTemperatureTextPanel.BackColor = Color.Red;
                  this.SensorEmatCoilTemperatureTextPanel.ValueText = "---";
               }
            }


            double sensorDisplacementReading = NicBotComm.Instance.GetSensorDisplacement();

            if (double.IsNaN(sensorDisplacementReading) == false)
            {
               this.SensorSensorDisplacementTextPanel.BackColor = Color.Black;
               this.SensorSensorDisplacementTextPanel.ValueText = this.GetValueText(sensorDisplacementReading, ParameterAccessor.Instance.SensorDisplacementConversionUnit);
            }
            else
            {
               this.SensorSensorDisplacementTextPanel.BackColor = Color.Red;
               this.SensorSensorDisplacementTextPanel.ValueText = "---";
            }

            if (false == this.SensorDirectionTextPanel.Enabled)
            {
               if ((false == this.sensorThicknessReadingPending) && (false == this.sensorStressReadingPending))
               {
                  this.SensorDirectionTextPanel.Enabled = true;
               }
            }
         }

         #endregion
#endif

         if (false != this.processStopNeeded)
         {
            this.processStopNeeded = false;
            this.Process = this.ProcessStopping;
         }
      }

      private void ProcessStopping()
      {
         this.TargetStatusTextBox.Visible = false;
         this.MainStatusTextBox.Width = (this.TargetStatusTextBox.Left + this.TargetStatusTextBox.Width - this.MainStatusTextBox.Left);
         this.MainStatusTextBox.Text = "stopping";
         this.MainStatusTextBox.BackColor = Color.Yellow;
         Tracer.WriteHigh(TraceGroup.UI, null, "stopping");

         DeviceCommunication.Instance.Stop();

         this.Process = this.ProcessWaitCommStop;
      }

      private void ProcessWaitCommStop()
      {
         if (false == DeviceCommunication.Instance.Running)
         {
            ParameterAccessor.Instance.Write(Application.ExecutablePath);

            Tracer.WriteHigh(TraceGroup.UI, null, "stopped");

            this.Process = this.ProcessCommStopped;
         }
      }

      private void ProcessCommStopped()
      {
         this.processStopped = true;

         if (false != this.processExitNeeded)
         {
            this.Process = this.ProcessExit;
         }
         else
         {
            this.Process = this.ProcessStart;
         }
      }

      private void ProcessExit()
      {
         this.UpdateTimer.Enabled = false;
         this.Close();
      }

      private void TickProcess()
      {
         this.Process();
      }

      #endregion

      #region Laser Robot Movement Events

      private void LaserRobotMotorSetupButton_Click(object sender, EventArgs e)
      {
         Button button = (Button)sender;

         LaserRobotMovementSetupForm laserRobotMovementSetupForm = new LaserRobotMovementSetupForm();

         this.SetDialogLocation(button, laserRobotMovementSetupForm);
         this.DimBackground();
         laserRobotMovementSetupForm.ShowDialog();
         this.LightBackground();
      }

      private void LaserRobotMovementJoystickEnableButton_Click(object sender, EventArgs e)
      {
         if (JoystickApplications.laserRobot != this.joystickApplication)
         {
            this.joystickApplication = JoystickApplications.laserRobot;
         }
         else
         {
            this.joystickApplication = JoystickApplications.none;
         }

         this.UpdateJoystickApplicationButtons();
      }

      #endregion

      #region Target Robot Movement Events

      private void TargetRobotMotorSetupButton_Click(object sender, EventArgs e)
      {
         Button button = (Button)sender;
         
         TargetRobotMovementSetupForm targetRobotMovementSetupForm = new TargetRobotMovementSetupForm();

         this.SetDialogLocation(button, targetRobotMovementSetupForm);
         this.DimBackground();
         targetRobotMovementSetupForm.ShowDialog();
         this.LightBackground();
      }

      private void TargetRobotMovementJoystickEnableButton_Click(object sender, EventArgs e)
      {
         if (JoystickApplications.targetRobot != this.joystickApplication)
         {
            this.joystickApplication = JoystickApplications.targetRobot;
         }
         else
         {
            this.joystickApplication = JoystickApplications.none;
         }

         this.UpdateJoystickApplicationButtons();
      }

      #endregion

      #region Laser Actions Events

      private void SensorUpButton_MouseDown(object sender, MouseEventArgs e)
      {
         this.targetButtonChange = 1;
      }

      private void SensorUpButton_MouseUp(object sender, MouseEventArgs e)
      {
         this.targetButtonChange = 0;
      }

      private void SensorDownButton_MouseDown(object sender, MouseEventArgs e)
      {
         this.targetButtonChange = -1;
      }

      private void SensorDownButton_MouseUp(object sender, MouseEventArgs e)
      {
         this.targetButtonChange = 0;      
      }

      private void SensorCenterButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         DeviceCommunication.Instance.SetTargetCenter();
      }

      private void LaserUpButton_MouseDown(object sender, MouseEventArgs e)
      {
         this.laserYButtonChange = 1;
      }

      private void LaserUpButton_MouseUp(object sender, MouseEventArgs e)
      {
         this.laserYButtonChange = 0;
      }

      private void LaserDownButton_MouseDown(object sender, MouseEventArgs e)
      {
         this.laserYButtonChange = -1;
      }

      private void LaserDownButton_MouseUp(object sender, MouseEventArgs e)
      {
         this.laserYButtonChange = 0;
      }

      private void LaserLeftButton_MouseDown(object sender, MouseEventArgs e)
      {
         this.laserXButtonChange = 1;
      }

      private void LaserLeftButton_MouseUp(object sender, MouseEventArgs e)
      {
         this.laserXButtonChange = 0;
      }

      private void LaserRightButton_MouseDown(object sender, MouseEventArgs e)
      {
         this.laserXButtonChange = -1;
      }

      private void LaserRightButton_MouseUp(object sender, MouseEventArgs e)
      {
         this.laserXButtonChange = 0;
      }

      private void TargetCenterButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         DeviceCommunication.Instance.SetLaserCenter();
      }

      private void RecordLaserMeasurementButton_Click(object sender, EventArgs e)
      {
         this.laserMeasurementRecorded = true;
      }

      private void LaserSetupButton_Click(object sender, EventArgs e)
      {
         Button button = (Button)sender;

         LaserSetupForm laserSetupForm = new LaserSetupForm();

         this.SetDialogLocation(button, laserSetupForm);
         this.DimBackground();
         laserSetupForm.ShowDialog();
         this.LightBackground();
      }

      private void LaserAimButton_Click(object sender, EventArgs e)
      {
         bool request = !DeviceCommunication.Instance.GetLaserAim();
         DeviceCommunication.Instance.SetLaserAim(request);
      }

      private void LaserMeasureButton_Click(object sender, EventArgs e)
      {
         bool laserMeasureActivity = DeviceCommunication.Instance.GetLaserMeasurementActivity();

         if (false == laserMeasureActivity)
         {
            DeviceCommunication.Instance.StartLaserMeasurement();
            this.laserMeasurementRequested = true;
         }
         else
         {
            DeviceCommunication.Instance.CancelLaserMeasurement();
            this.laserMeasurementRequested = false;
         }
      }

      private void LaserJoystickEnableButton_Click(object sender, EventArgs e)
      {
         if (JoystickApplications.laserAim != this.joystickApplication)
         {
            this.joystickApplication = JoystickApplications.laserAim;
         }
         else
         {
            this.joystickApplication = JoystickApplications.none;
         }

         this.UpdateJoystickApplicationButtons();
      }

      #endregion

      #region System User Actions

      private void StopAllButton_Click(object sender, EventArgs e)
      {
         this.StopAllPanel.BackColor = Color.Red;
         DeviceCommunication.Instance.StopAll();
      }

      private void WriteOsdButton_Click(object sender, EventArgs e)
      {
         Button button = (Button)sender;

         OsdForm osdForm = new OsdForm();

         this.SetDialogLocation(button, osdForm);
         this.DimBackground();
         osdForm.ShowDialog();
         this.LightBackground();
      }

      private void SystemStatusButton_Click(object sender, EventArgs e)
      {
         Button button = (Button)sender;

         SystemStatusForm systemStatusForm = new SystemStatusForm();
         systemStatusForm.SystemResetHandler = new SystemStatusForm.SystemResetDelegate(this.RestartSystem);
         systemStatusForm.TraceListenerDestination = new SystemStatusForm.TraceListenerDestinationDelegate(this.SetTraceListenerDestination);

         this.SetDialogLocation(button, systemStatusForm);
         this.DimBackground();
         systemStatusForm.ShowDialog();
         this.LightBackground();
      }


      private void SystemResetButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         this.RestartSystem();
      }

      private void ExitButton_HoldTimeout(object sender, Controls.HoldTimeoutEventArgs e)
      {
         this.processExitNeeded = true;
         this.processStopNeeded = true;
      }

      #endregion

      #region Form Events Process

      private void MainForm_Shown(object sender, EventArgs e)
      {
         this.Process = this.ProcessStart;
         this.UpdateTimer.Interval = 1;
         this.UpdateTimer.Enabled = true;
      }

      private void UpdateTimer_Tick(object sender, EventArgs e)
      {
         this.TickProcess();
      }

      private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
      {
         if (false == this.processStopped)
         {
            this.processExitNeeded = true;
            this.processStopNeeded = true;
            e.Cancel = true;
         }
      }

      private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
      {
      }

      #endregion

      #region Constructor

      public MainForm()
      {
         this.InitializeComponent();

         this.fileTraceListener = new FileTraceListener();
         this.fileTraceListener.LogFilePath = @"c:\logs\e4";
         this.fileTraceListener.MaximumLines = 10000;
         this.fileTraceListener.Prefix = "E4_";
         Trace.Listeners.Add(this.fileTraceListener);

         this.traceListener = new UdpTraceListener("127.0.0.1", 10000);
         Trace.Listeners.Add(this.traceListener);

         Tracer.MaskString = "FFFFFFFF";
      
         this.dimmerForm = new PopupDimmerForm();
         this.dimmerForm.Opacity = 0.65;
         this.dimmerForm.ShowInTaskbar = false;
      }

      #endregion

   }
}
