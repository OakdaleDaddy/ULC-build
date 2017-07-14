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

   public partial class MessageForm : Form
   {
      #region Fields

      private bool mouseDown;
      private Point mouseDownPoint;

      #endregion

      #region Properties

      public string Title { set; get; }
      public string Message { set; get; }
      public MessageBoxButtons Buttons { set; get; }

      #endregion

      #region User Events

      private void FirstButton_Click(object sender, EventArgs e)
      {
         if (this.Buttons == MessageBoxButtons.AbortRetryIgnore)
         {
            this.DialogResult = System.Windows.Forms.DialogResult.Abort;
         }
         else if (this.Buttons == MessageBoxButtons.OK)
         {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
         }
         else if (this.Buttons == MessageBoxButtons.OKCancel)
         {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
         }
         else if (this.Buttons == MessageBoxButtons.RetryCancel)
         {
            this.DialogResult = System.Windows.Forms.DialogResult.Retry;
         }
         else if (this.Buttons == MessageBoxButtons.YesNo)
         {
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
         }
         else if (this.Buttons == MessageBoxButtons.YesNoCancel)
         {
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
         }
      }

      private void SecondButton_Click(object sender, EventArgs e)
      {
         if (this.Buttons == MessageBoxButtons.AbortRetryIgnore)
         {
            this.DialogResult = System.Windows.Forms.DialogResult.Retry;
         }
         else if (this.Buttons == MessageBoxButtons.OKCancel)
         {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         }
         else if (this.Buttons == MessageBoxButtons.RetryCancel)
         {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         }
         else if (this.Buttons == MessageBoxButtons.YesNo)
         {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
         }
         else if (this.Buttons == MessageBoxButtons.YesNoCancel)
         {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
         }
      }

      private void ThirdButton_Click(object sender, EventArgs e)
      {
         if (this.Buttons == MessageBoxButtons.AbortRetryIgnore)
         {
            this.DialogResult = System.Windows.Forms.DialogResult.Ignore;
         }
         else if (this.Buttons == MessageBoxButtons.YesNoCancel)
         {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         }
      }

      #endregion

      #region Form Events

      private void MessageForm_Shown(object sender, EventArgs e)
      {
         this.TitleLabel.Text = this.Title;
         this.MessageLabel.Text = this.Message;

         if (this.Buttons == MessageBoxButtons.OK)
         {
            this.FirstButton.Text = "OK";

            int firstLeft = (this.Width - this.FirstButton.Width) / 2;
            this.FirstButton.Left = firstLeft;

            this.FirstButton.Visible = true;
            this.SecondButton.Visible = false;
            this.ThirdButton.Visible = false;
         }
         else if ((this.Buttons == MessageBoxButtons.OKCancel) ||
                  (this.Buttons == MessageBoxButtons.RetryCancel) ||
                  (this.Buttons == MessageBoxButtons.YesNo))
         {
            if (this.Buttons == MessageBoxButtons.OKCancel)
            {
               this.FirstButton.Text = "OK";
               this.SecondButton.Text = "CANCEL";
            }
            else if (this.Buttons == MessageBoxButtons.RetryCancel)
            {
               this.FirstButton.Text = "RETRY";
               this.SecondButton.Text = "CANCEL";
            }
            else
            {
               this.FirstButton.Text = "YES";
               this.SecondButton.Text = "NO";
            }

            int firstLeft = (this.Width - this.FirstButton.Width - 8 - this.SecondButton.Width) / 2;
            int secondLeft = firstLeft + this.FirstButton.Width + 8;

            this.FirstButton.Left = firstLeft;
            this.SecondButton.Left = secondLeft;

            this.FirstButton.Visible = true;
            this.SecondButton.Visible = true;
            this.ThirdButton.Visible = false;
         }
         else if ((this.Buttons == MessageBoxButtons.AbortRetryIgnore) ||
                  (this.Buttons == MessageBoxButtons.YesNoCancel))
         {
            if (this.Buttons == MessageBoxButtons.AbortRetryIgnore)
            {
               this.FirstButton.Text = "ABORT";
               this.SecondButton.Text = "RETRY";
               this.ThirdButton.Text = "IGNORE";
            }
            else
            {
               this.FirstButton.Text = "YES";
               this.SecondButton.Text = "NO";
               this.ThirdButton.Text = "CANCEL";
            }

            int firstLeft = (this.Width - this.FirstButton.Width - 8 - this.SecondButton.Width - 8 - this.ThirdButton.Width) / 2;
            int secondLeft = firstLeft + this.FirstButton.Width + 8;
            int thirdLeft = secondLeft + this.SecondButton.Width + 8;

            this.FirstButton.Left = firstLeft;
            this.SecondButton.Left = secondLeft;
            this.ThirdButton.Left = thirdLeft;

            this.FirstButton.Visible = true;
            this.SecondButton.Visible = true;
            this.ThirdButton.Visible = true;
         }
      }

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

      public MessageForm()
      {
         this.InitializeComponent();
      }

      #endregion

   }
}
