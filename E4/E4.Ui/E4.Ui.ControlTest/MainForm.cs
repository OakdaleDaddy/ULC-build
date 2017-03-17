using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace E4.Ui.ControlTest
{
   public partial class MainForm : Form
   {
      public MainForm()
      {
         InitializeComponent();
      }

      private void SetButton_Click(object sender, EventArgs e)
      {
         int value = 0;

         if (int.TryParse(this.ValueTextBox.Text, out value) != false)
         {
            this.TestPositionIndicator.Position = value;
         }
      }
   }
}
