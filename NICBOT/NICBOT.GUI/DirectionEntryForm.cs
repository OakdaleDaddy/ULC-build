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
   public partial class DirectionEntryForm : Form
   {
      #region Properties

      public Directions Direction { set; get; }

      #endregion

      #region User Events

      private void NorthButton_Click(object sender, EventArgs e)
      {
         this.Direction = Directions.north;
         this.DialogResult = System.Windows.Forms.DialogResult.OK;
      }

      private void EastButton_Click(object sender, EventArgs e)
      {
         this.Direction = Directions.east;
         this.DialogResult = System.Windows.Forms.DialogResult.OK;
      }

      private void SouthButton_Click(object sender, EventArgs e)
      {
         this.Direction = Directions.south;
         this.DialogResult = System.Windows.Forms.DialogResult.OK;
      }

      private void WestButton_Click(object sender, EventArgs e)
      {
         this.Direction = Directions.west;
         this.DialogResult = System.Windows.Forms.DialogResult.OK;
      }

      #endregion

      #region Form Events

      private void DirectionalForm_Shown(object sender, EventArgs e)
      {
         if (Directions.north == this.Direction)
         {
            this.NorthButton.ForeColor = Color.White;
            this.EastButton.ForeColor = Color.Black;
            this.SouthButton.ForeColor = Color.Black;
            this.WestButton.ForeColor = Color.Black;
         }
         else if (Directions.east == this.Direction)
         {
            this.EastButton.ForeColor = Color.White;
            this.NorthButton.ForeColor = Color.Black;
            this.SouthButton.ForeColor = Color.Black;
            this.WestButton.ForeColor = Color.Black;
         }
         else if (Directions.south == this.Direction)
         {
            this.SouthButton.ForeColor = Color.White;
            this.NorthButton.ForeColor = Color.Black;
            this.EastButton.ForeColor = Color.Black;
            this.WestButton.ForeColor = Color.Black;
         }
         else if (Directions.west == this.Direction)
         {
            this.WestButton.ForeColor = Color.White;
            this.NorthButton.ForeColor = Color.Black;
            this.EastButton.ForeColor = Color.Black;
            this.SouthButton.ForeColor = Color.Black;
         }
      }

      #endregion

      #region Constructor

      public DirectionEntryForm()
      {
         this.InitializeComponent();
      }

      #endregion

   }
}
