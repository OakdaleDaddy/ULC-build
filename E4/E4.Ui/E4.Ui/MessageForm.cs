namespace E4.Ui
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

      #endregion

      #region User Events

      private void OkButton_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      #endregion

      #region Form Events

      private void MessageForm_Shown(object sender, EventArgs e)
      {
         this.TitleLabel.Text = this.Title;
         this.MessageLabel.Text = this.Message;
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
