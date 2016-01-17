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
   public partial class FeederClampSetupForm : Form
   {
      #region Helper Functions

      private void UpdateFeederDisplay()
      {
         bool feederClampRequest = NicBotComm.Instance.GetFeederClampSetPoint();
         bool feederClampActual = NicBotComm.Instance.GetFeederClamp();

         if (feederClampRequest == feederClampActual)
         {
            string feederClampStatus = "";

            this.ClampHoldButton.Enabled = false;
            this.ClampReleaseButton.Enabled = false;

            if (false == feederClampRequest)
            {
               feederClampStatus = "CLAMP GRABBING";
            }
            else
            {
               feederClampStatus = "CLAMP RELEASING";
            }

            this.TitleLabel.Text = feederClampStatus;
         }
         else
         {
            this.TitleLabel.Text = "FEEDER CLAMP SETUP";

            this.ClampHoldButton.Enabled = true;
            this.ClampReleaseButton.Enabled = true;

            if (false != feederClampActual)
            {
               this.ClampHoldButton.Text = "CLAMP HELD";
               this.ClampReleaseButton.Text = "RELEASE";

               this.ClampHoldButton.ForeColor = Color.FromArgb(240, 240, 240);
               this.ClampReleaseButton.ForeColor = Color.Black;
            }
            else
            {
               this.ClampHoldButton.Text = "GRAB";
               this.ClampReleaseButton.Text = "CLAMP RELEASED";

               this.ClampReleaseButton.ForeColor = Color.FromArgb(240, 240, 240);
               this.ClampHoldButton.ForeColor = Color.Black;
            }
         }
      }

      #endregion

      #region User Event Functions

      private void ClampHoldButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         NicBotComm.Instance.SetFeederClamp(false);
      }

      private void ClampReleaseButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         NicBotComm.Instance.SetFeederClamp(true);
      }

      private void BackButton_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      #endregion

      #region Form Event Functions

      private void UpdateTimer_Tick(object sender, EventArgs e)
      {
         this.UpdateFeederDisplay();
      }

      private void FeederClampSetupForm_Load(object sender, EventArgs e)
      {
         this.UpdateFeederDisplay();
         this.UpdateTimer.Enabled = true;
      }

      #endregion

      #region Constructor

      public FeederClampSetupForm()
      {
         InitializeComponent();
      }

      #endregion
   }
}
