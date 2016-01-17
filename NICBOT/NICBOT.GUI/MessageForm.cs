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
   public partial class MessageForm : Form
   {

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

      #endregion

      #region Constructor

      public MessageForm()
      {
         this.InitializeComponent();
      }

      #endregion
      
   }
}
