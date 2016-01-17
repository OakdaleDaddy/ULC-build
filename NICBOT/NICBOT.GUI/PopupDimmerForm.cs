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
   public partial class PopupDimmerForm : Form
   {
      public PopupDimmerForm()
      {
         this.InitializeComponent();
         this.Opacity = 0.83;
      }
   }
}
