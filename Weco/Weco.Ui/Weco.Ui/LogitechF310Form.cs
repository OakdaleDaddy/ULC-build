
namespace Weco.Ui
{
   using System;
   using System.Collections.Generic;
   using System.ComponentModel;
   using System.Data;
   using System.Drawing;
   using System.Linq;
   using System.Text;
   using System.Threading.Tasks;
   using System.Windows.Forms;

   using Weco.Utilities;

   public partial class LogitechF310Form : Form
   {
      #region Fields

      private bool flasher;

      #endregion

      #region Helper Functions

      private void UpdateView()
      {           
         if (false != Joystick.Instance.Valid)
         {
            this.Button1Indicator.IndicatorColor = (false != Joystick.Instance.Button1Pressed) ? Color.Cyan : Color.DarkBlue;
            this.Button2Indicator.IndicatorColor = (false != Joystick.Instance.Button2Pressed) ? Color.Lime : Color.DarkGreen;
            this.Button3Indicator.IndicatorColor = (false != Joystick.Instance.Button3Pressed) ? Color.Red : Color.DarkRed;
            this.Button4Indicator.IndicatorColor = (false != Joystick.Instance.Button4Pressed) ? Color.Orange : Color.FromArgb(149, 74, 0);
            this.Button5Indicator.IndicatorColor = (false != Joystick.Instance.Button5Pressed) ? Color.Silver : Color.FromArgb(64, 64, 64);
            this.Button6Indicator.IndicatorColor = (false != Joystick.Instance.Button6Pressed) ? Color.Silver : Color.FromArgb(64, 64, 64);
            this.Button7Indicator.IndicatorColor = (false != Joystick.Instance.Button7Pressed) ? Color.Silver : Color.FromArgb(64, 64, 64);
            this.Button8Indicator.IndicatorColor = (false != Joystick.Instance.Button8Pressed) ? Color.Silver : Color.FromArgb(64, 64, 64);
            this.Button9Indicator.IndicatorColor = (false != Joystick.Instance.Button9Pressed) ? Color.Silver : Color.FromArgb(64, 64, 64);
            this.Button10Indicator.IndicatorColor = (false != Joystick.Instance.Button10Pressed) ? Color.Silver : Color.FromArgb(64, 64, 64);
            this.Button11Indicator.IndicatorColor = (false != Joystick.Instance.Button11Pressed) ? Color.Silver : Color.FromArgb(64, 64, 64);
            this.Button12Indicator.IndicatorColor = (false != Joystick.Instance.Button12Pressed) ? Color.Silver : Color.FromArgb(64, 64, 64);

            if (false != Joystick.Instance.PovPressed)
            {
               this.PovPanel.ValueText = string.Format("{0}", Joystick.Instance.PovValue);
            }
            else
            {
               this.PovPanel.ValueText = "";
            }
            this.PovPanel.BackColor = Color.Black;

            this.Axis1XPanel.ValueText = string.Format("{0}", Joystick.Instance.XAxis); ;
            this.Axis1XPanel.BackColor = Color.Black;
            this.Axis1YPanel.ValueText = string.Format("{0}", Joystick.Instance.YAxis); ;
            this.Axis1YPanel.BackColor = Color.Black;
            
            this.Axis2XPanel.ValueText = string.Format("{0}", Joystick.Instance.Throttle); ;
            this.Axis2XPanel.BackColor = Color.Black;
            this.Axis2YPanel.ValueText = string.Format("{0}", Joystick.Instance.ZAxis); ;
            this.Axis2YPanel.BackColor = Color.Black;
         }
         else
         {
            Color errorColor = (false != this.flasher) ? Color.Red : Color.FromArgb(127, 0, 0);

            this.Button1Indicator.IndicatorColor = errorColor;
            this.Button2Indicator.IndicatorColor = errorColor;
            this.Button3Indicator.IndicatorColor = errorColor;
            this.Button4Indicator.IndicatorColor = errorColor;
            this.Button5Indicator.IndicatorColor = errorColor;
            this.Button6Indicator.IndicatorColor = errorColor;
            this.Button7Indicator.IndicatorColor = errorColor;
            this.Button8Indicator.IndicatorColor = errorColor;
            this.Button9Indicator.IndicatorColor = errorColor;
            this.Button10Indicator.IndicatorColor = errorColor;
            this.Button11Indicator.IndicatorColor = errorColor;
            this.Button12Indicator.IndicatorColor = errorColor;

            this.PovPanel.ValueText = "";
            this.PovPanel.BackColor = errorColor;

            this.Axis1XPanel.ValueText = "";
            this.Axis1XPanel.BackColor = errorColor;
            this.Axis1YPanel.ValueText = "";
            this.Axis1YPanel.BackColor = errorColor;

            this.Axis2XPanel.ValueText = "";
            this.Axis2XPanel.BackColor = errorColor;
            this.Axis2YPanel.ValueText = "";
            this.Axis2YPanel.BackColor = errorColor;
         }
      }

      #endregion

      #region User Events

      private void BackButton_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      #endregion

      #region Form Events

      private void LogitechF310Form_Shown(object sender, EventArgs e)
      {
         this.UpdateView();
         this.UpdateTimer.Enabled = true;
      }

      private void UpdateTimer_Tick(object sender, EventArgs e)
      {
         this.flasher = !this.flasher;
         this.UpdateView();
      }

      #endregion

      #region Constructor

      public LogitechF310Form()
      {
         this.InitializeComponent();
      }

      #endregion

   }
}
