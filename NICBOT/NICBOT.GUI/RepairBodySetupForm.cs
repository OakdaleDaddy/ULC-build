using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NICBOT.GUI
{
   public partial class RepairBodySetupForm : Form
   {
      #region Fields

      private bool wheelLocked;
      private bool wheelChanging;
      private bool topFrontWheelNotReadyToLock;
      private bool topRearWheelNotReadyToLock;
      private bool bottomFrontWheelNotReadyToLock;
      private bool bottomRearWheelNotReadyToLock;

      #endregion

      #region Helper Functions

      private void UpdateWheelStatus(TextBox statusBox, bool readyToLock, ref bool notReadyToLock, ref int count)
      {
         if (false != this.wheelChanging)
         {
            if (false == readyToLock)
            {
               notReadyToLock = true;
            }

            if (false != readyToLock)
            {
               if (false == notReadyToLock)
               {
                  statusBox.BackColor = Color.Red;
               }
               else
               {
                  statusBox.BackColor = Color.Green;
                  count++;
               }
            }
            else
            {
               statusBox.BackColor = Color.Black;
            }
         }
         else
         {
            statusBox.BackColor = (false != readyToLock) ? Color.LimeGreen : Color.Black;
         }
      }

      private void UpdateDisplay()
      {
         BodyPositions bodyPosition = NicBotComm.Instance.GetBodyPosition();

         switch (bodyPosition)
         {
            case BodyPositions.off:
            {
               this.BodyOffButton.ForeColor = Color.FromArgb(240, 240, 240);
               this.BodyClosedButton.ForeColor = Color.Black;
               this.BodyOpenButton.ForeColor = Color.Black;
               this.BodyDrillButton.ForeColor = Color.Black;
               this.BodyRearReleaseButton.ForeColor = Color.Black;
               this.BodyFrontReleaseButton.ForeColor = Color.Black;
               break;
            }
            case BodyPositions.closed:
            {
               this.BodyOffButton.ForeColor = Color.Black;
               this.BodyClosedButton.ForeColor = Color.FromArgb(240, 240, 240);
               this.BodyOpenButton.ForeColor = Color.Black;
               this.BodyDrillButton.ForeColor = Color.Black;
               this.BodyRearReleaseButton.ForeColor = Color.Black;
               this.BodyFrontReleaseButton.ForeColor = Color.Black;
               break;
            }
            case BodyPositions.opened:
            {
               this.BodyOffButton.ForeColor = Color.Black;
               this.BodyOpenButton.ForeColor = Color.FromArgb(240, 240, 240);
               this.BodyClosedButton.ForeColor = Color.Black;
               this.BodyDrillButton.ForeColor = Color.Black;
               this.BodyRearReleaseButton.ForeColor = Color.Black;
               this.BodyFrontReleaseButton.ForeColor = Color.Black;
               break;
            }
            case BodyPositions.frontLoose:
            {
               this.BodyOffButton.ForeColor = Color.Black;
               this.BodyFrontReleaseButton.ForeColor = Color.FromArgb(240, 240, 240);
               this.BodyClosedButton.ForeColor = Color.Black;
               this.BodyOpenButton.ForeColor = Color.Black;
               this.BodyDrillButton.ForeColor = Color.Black;
               this.BodyRearReleaseButton.ForeColor = Color.Black;
               break;
            }
            case BodyPositions.rearLoose:
            {
               this.BodyOffButton.ForeColor = Color.Black;
               this.BodyRearReleaseButton.ForeColor = Color.FromArgb(240, 240, 240);
               this.BodyClosedButton.ForeColor = Color.Black;
               this.BodyOpenButton.ForeColor = Color.Black;
               this.BodyDrillButton.ForeColor = Color.Black;
               this.BodyFrontReleaseButton.ForeColor = Color.Black;
               break;
            }
            case BodyPositions.drill:
            {
               this.BodyOffButton.ForeColor = Color.Black;
               this.BodyDrillButton.ForeColor = Color.FromArgb(240, 240, 240);
               this.BodyClosedButton.ForeColor = Color.Black;
               this.BodyOpenButton.ForeColor = Color.Black;
               this.BodyRearReleaseButton.ForeColor = Color.Black;
               this.BodyFrontReleaseButton.ForeColor = Color.Black;
               break;
            }
            default:
            case BodyPositions.manual:
            {
               this.BodyOffButton.ForeColor = Color.Black;
               this.BodyClosedButton.ForeColor = Color.Black;
               this.BodyOpenButton.ForeColor = Color.Black;
               this.BodyDrillButton.ForeColor = Color.Black;
               this.BodyRearReleaseButton.ForeColor = Color.Black;
               this.BodyFrontReleaseButton.ForeColor = Color.Black;
               break;
            }
         }

         this.RearDrillCoverButton.OptionASelected = NicBotComm.Instance.GetSolenoidActive(Solenoids.rearDrillCover);
         this.FrontDrillCoverButton.OptionASelected = NicBotComm.Instance.GetSolenoidActive(Solenoids.frontDrillCover);
         this.RearNozzleButton.OptionASelected = NicBotComm.Instance.GetSolenoidActive(Solenoids.rearNozzleExtend);
         this.FrontNozzleButton.OptionASelected = NicBotComm.Instance.GetSolenoidActive(Solenoids.frontNozzleExtend);
         this.FrontArmExtendButton.OptionASelected = NicBotComm.Instance.GetSolenoidActive(Solenoids.frontArmExtend);
         this.FrontArmRetractButton.OptionASelected = NicBotComm.Instance.GetSolenoidActive(Solenoids.frontArmRetract);
         this.RearArmExtendButton.OptionASelected = NicBotComm.Instance.GetSolenoidActive(Solenoids.rearArmExtend);
         this.RearArmRetractButton.OptionASelected = NicBotComm.Instance.GetSolenoidActive(Solenoids.rearArmRetract);
         this.LowerArmsExtendButton.OptionASelected = NicBotComm.Instance.GetSolenoidActive(Solenoids.lowerArmExtend);
         this.LowerArmsRetractButton.OptionASelected = NicBotComm.Instance.GetSolenoidActive(Solenoids.lowerArmRetract);
         this.WheelsCircumferenceButton.OptionASelected = NicBotComm.Instance.GetSolenoidActive(Solenoids.wheelCircumferential);
         this.WheelsAxialButton.OptionASelected = NicBotComm.Instance.GetSolenoidActive(Solenoids.wheelAxial);
         this.WheelLockButton.OptionASelected = NicBotComm.Instance.GetSolenoidActive(Solenoids.wheelLock);
      }

      #endregion
      
      #region User Events

      #region Body Positions

      private void BodyOffButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         NicBotComm.Instance.SetBodyPosition(BodyPositions.off);
         this.UpdateDisplay();
      }

      private void BodyDrillButton_Click(object sender, EventArgs e)
      {
         NicBotComm.Instance.SetBodyPosition(BodyPositions.drill);
         this.UpdateDisplay();
      }

      private void BodyClosedButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         NicBotComm.Instance.SetBodyPosition(BodyPositions.closed);
         this.UpdateDisplay();
      }

      private void BodyOpenButton_Click(object sender, EventArgs e)
      {
         NicBotComm.Instance.SetBodyPosition(BodyPositions.opened);
         this.UpdateDisplay();
      }

      private void BodyFrontReleaseButton_Click(object sender, EventArgs e)
      {
         NicBotComm.Instance.SetBodyPosition(BodyPositions.frontLoose);
         this.UpdateDisplay();
      }

      private void BodyRearReleaseButton_Click(object sender, EventArgs e)
      {
         NicBotComm.Instance.SetBodyPosition(BodyPositions.rearLoose);
         this.UpdateDisplay();
      }

      #endregion

      #region Solinoid Selections

      private void FrontDrillCoverButton_Click(object sender, EventArgs e)
      {
         bool request = !this.FrontDrillCoverButton.OptionASelected;
         NicBotComm.Instance.SetSolenoid(Solenoids.frontDrillCover, request);
         this.UpdateDisplay();
      }

      private void FrontNozzleButton_Click(object sender, EventArgs e)
      {
         bool request = !this.FrontNozzleButton.OptionASelected;
         NicBotComm.Instance.SetSolenoid(Solenoids.frontNozzleExtend, request);
         this.UpdateDisplay();
      }

      private void RearDrillCoverButton_Click(object sender, EventArgs e)
      {
         bool request = !this.RearDrillCoverButton.OptionASelected;
         NicBotComm.Instance.SetSolenoid(Solenoids.rearDrillCover, request);
         this.UpdateDisplay();
      }

      private void RearNozzleButton_Click(object sender, EventArgs e)
      {
         bool request = !this.RearNozzleButton.OptionASelected;
         NicBotComm.Instance.SetSolenoid(Solenoids.rearNozzleExtend, request);
         this.UpdateDisplay();
      }

      private void FrontArmExtendButton_Click(object sender, EventArgs e)
      {
         bool request = !this.FrontArmExtendButton.OptionASelected;
         NicBotComm.Instance.SetSolenoid(Solenoids.frontArmExtend, request);
         this.UpdateDisplay();
      }

      private void FrontArmRetractButton_Click(object sender, EventArgs e)
      {
         bool request = !this.FrontArmRetractButton.OptionASelected;
         NicBotComm.Instance.SetSolenoid(Solenoids.frontArmRetract, request);
         this.UpdateDisplay();
      }

      private void RearArmExtendButton_Click(object sender, EventArgs e)
      {
         bool request = !this.RearArmExtendButton.OptionASelected;
         NicBotComm.Instance.SetSolenoid(Solenoids.rearArmExtend, request);
         this.UpdateDisplay();
      }

      private void RearArmRetractButton_Click(object sender, EventArgs e)
      {
         bool request = !this.RearArmRetractButton.OptionASelected;
         NicBotComm.Instance.SetSolenoid(Solenoids.rearArmRetract, request);
         this.UpdateDisplay();
      }

      private void LowerArmsExtendButton_Click(object sender, EventArgs e)
      {
         bool request = !this.LowerArmsExtendButton.OptionASelected;
         NicBotComm.Instance.SetSolenoid(Solenoids.lowerArmExtend, request);
         this.UpdateDisplay();
      }

      private void LowerArmsRetractButton_Click(object sender, EventArgs e)
      {
         bool request = !this.LowerArmsRetractButton.OptionASelected;
         NicBotComm.Instance.SetSolenoid(Solenoids.lowerArmRetract, request);
         this.UpdateDisplay();
      }

      private void WheelsCircumferenceButton_Click(object sender, EventArgs e)
      {
         bool request = !this.WheelsCircumferenceButton.OptionASelected;

         if ((false != request) && (false == this.wheelLocked))
         {
            this.wheelChanging = true;

            this.topFrontWheelNotReadyToLock = !NicBotComm.Instance.GetTopFrontReadyToLock();
            this.topRearWheelNotReadyToLock = !NicBotComm.Instance.GetTopRearReadyToLock();
            this.bottomFrontWheelNotReadyToLock = !NicBotComm.Instance.GetBottomFrontReadyToLock();
            this.bottomRearWheelNotReadyToLock = !NicBotComm.Instance.GetBottomRearReadyToLock();
         }

         NicBotComm.Instance.SetSolenoid(Solenoids.wheelCircumferential, request);
         this.UpdateDisplay();
      }

      private void WheelsAxialButton_Click(object sender, EventArgs e)
      {
         bool request = !this.WheelsAxialButton.OptionASelected;

         if ((false != request) && (false == this.wheelLocked))
         {
            this.wheelChanging = true;

            this.topFrontWheelNotReadyToLock = !NicBotComm.Instance.GetTopFrontReadyToLock();
            this.topRearWheelNotReadyToLock = !NicBotComm.Instance.GetTopRearReadyToLock();
            this.bottomFrontWheelNotReadyToLock = !NicBotComm.Instance.GetBottomFrontReadyToLock();
            this.bottomRearWheelNotReadyToLock = !NicBotComm.Instance.GetBottomRearReadyToLock();
         }

         NicBotComm.Instance.SetSolenoid(Solenoids.wheelAxial, request);
         this.UpdateDisplay();
      }

      private void WheelLockButton_Click(object sender, EventArgs e)
      {
         bool request = !this.WheelLockButton.OptionASelected;
         this.wheelLocked = request;
         NicBotComm.Instance.SetSolenoid(Solenoids.wheelLock, request);
         this.UpdateDisplay();
      }

      #endregion

      private void BackButton_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      #endregion

      #region Form Events

      private void BodySetupForm_Shown(object sender, EventArgs e)
      {
         //this.FrontNozzleButton.Enabled = NicBotComm.Instance.DrillAtRetractionLimit(ToolLocations.front);
         //this.RearNozzleButton.Enabled = NicBotComm.Instance.DrillAtRetractionLimit(ToolLocations.rear);

         this.wheelLocked = NicBotComm.Instance.GetSolenoidActive(Solenoids.wheelLock);
         this.wheelChanging = false;
         // todo interlock on wheel controls and changing by user

         this.UpdateDisplay();
         this.UpdateTimer.Enabled = true;
      }

      private void UpdateTimer_Tick(object sender, EventArgs e)
      {
         int count = 0;

         this.UpdateWheelStatus(this.TopFrontWheelIndicatorTextBox, NicBotComm.Instance.GetTopFrontReadyToLock(), ref this.topFrontWheelNotReadyToLock, ref count);
         this.UpdateWheelStatus(this.TopRearWheelIndicatorTextBox, NicBotComm.Instance.GetTopRearReadyToLock(), ref this.topRearWheelNotReadyToLock, ref count);
         this.UpdateWheelStatus(this.BottomFrontWheelIndicatorTextBox, NicBotComm.Instance.GetBottomFrontReadyToLock(), ref this.bottomFrontWheelNotReadyToLock, ref count);
         this.UpdateWheelStatus(this.BottomRearWheelIndicatorTextBox, NicBotComm.Instance.GetBottomRearReadyToLock(), ref this.bottomRearWheelNotReadyToLock, ref count);

         if ((false != this.wheelChanging) && (4 == count))
         {
            this.wheelChanging = false;
         }
      }

      #endregion

      #region Constructor

      public RepairBodySetupForm()
      {         
         this.InitializeComponent();
      }

      #endregion

   }
}
