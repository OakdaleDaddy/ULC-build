using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace E4.Ui
{
   public partial class LaserRobotMovementSetupForm : Form
   {
      #region Fields

      private bool mouseDown;
      private Point mouseDownPoint;

      #endregion

      #region User Events

      private void BackButton_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      #endregion

      #region Form Events

      private void TitleLabel_MouseDown(object sender, MouseEventArgs e)
      {
         this.mouseDownPoint = e.Location;
         this.mouseDown = true;
      }

      private void TitleLabel_MouseUp(object sender, MouseEventArgs e)
      {
         this.mouseDown = false;
      }

      private void TitleLabel_MouseMove(object sender, MouseEventArgs e)
      {
         if (false != this.mouseDown)
         {
            this.Top += (e.Y - mouseDownPoint.Y);
            this.Left += (e.X - mouseDownPoint.X);
         }
      }

      #endregion

      #region Constructor

      public LaserRobotMovementSetupForm()
      {
         this.InitializeComponent();
      }

      #endregion

   }
}
