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
   public partial class RobotBusControlForm : Form
   {
      #region Helper Functions

      private void ShowBusStatus()
      {
         string status = RobotCommBus.Instance.GetStatus(RobotCommBus.BusComponentId.Bus);

         if (null != status)
         {
            this.BusStatusTextBox.Text = status;

            if ("off" == status)
            {
               this.BusStatusTextBox.BackColor = Color.Gray;
            }
            else
            {
               this.BusStatusTextBox.BackColor = Color.Red;
            }
         }
         else
         {
            this.BusStatusTextBox.Text = "ready";
            this.BusStatusTextBox.BackColor = Color.LimeGreen;
         }
      }

      private void UpdateActivityButton()
      {
         if (false != RobotCommBus.Instance.Running)
         {
            if (false == ParameterAccessor.Instance.EnableRobotBus)
            {
               this.ActivityButton.Text = "STOP";
               this.ActivityButton.Enabled = true;
            }
            else
            {
               this.ActivityButton.Text = "STOP";
               this.ActivityButton.Enabled = false;
            }
         }
         else
         {
            if (false != ParameterAccessor.Instance.EnableRobotBus)
            {
               this.ActivityButton.Text = "START";
               this.ActivityButton.Enabled = true;
            }
            else
            {
               this.ActivityButton.Text = "START";
               this.ActivityButton.Enabled = false;
            }
         }
      }      

      #endregion

      #region User Events

      private void StateToggleButton_Click(object sender, EventArgs e)
      {
         bool selection = !this.StateToggleButton.OptionASelected;
         this.StateToggleButton.OptionASelected = selection;
         ParameterAccessor.Instance.EnableRobotBus = selection;

         this.UpdateActivityButton();
      }

      private void ActivityButton_HoldTimeout(object sender, HoldTimeoutEventArgs e)
      {
         if (false != RobotCommBus.Instance.Running)
         {
            RobotCommBus.Instance.Stop();
         }
         else
         {
            RobotCommBus.Instance.Start();
         }

         this.UpdateActivityButton();
      }

      private void BackButton_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      #endregion

      #region Form Events

      private void RobotBusControlForm_Shown(object sender, EventArgs e)
      {
         this.StateToggleButton.OptionASelected = ParameterAccessor.Instance.EnableRobotBus;
         this.UpdateActivityButton();

         this.ShowBusStatus();
         this.UpdateTimer.Enabled = true;
      }

      private void UpdateTimer_Tick(object sender, EventArgs e)
      {
         this.ShowBusStatus();
      }

      #endregion

      #region Constructor

      public RobotBusControlForm()
      {
         InitializeComponent();
      }

      #endregion

   }
}
