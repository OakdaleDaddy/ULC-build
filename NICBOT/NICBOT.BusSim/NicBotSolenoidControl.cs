using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NICBOT.BusSim
{
   public partial class NicBotSolenoidControl : UserControl
   {
      #region Fields

      private bool running;
      private bool solenoidOn;

      #endregion

      #region Properties

      public string Title
      {
         set { this.TitleLabel.Text = value; }
         get { return (this.TitleLabel.Text); }
      }

      public bool Running
      {
         set
         {
            if (value != this.running)
            {
               this.running = value;
               this.SolenoidOn = this.solenoidOn;
            }
         }

         get
         {
            return (this.running);
         }
      }

      public bool SolenoidOn
      {
         set
         {
            this.solenoidOn = value;

            if ((false != this.running) &&
                (false != this.solenoidOn))
            {
               this.OnOffLabel.Text = "on";
               this.OnOffLabel.BackColor = Color.LimeGreen;
            }
            else
            {
               this.OnOffLabel.Text = "off";
               this.OnOffLabel.BackColor = Color.DarkSlateGray;
            }
         }

         get
         {
            return (this.solenoidOn);
         }
      }

      #endregion

      #region Constructor

      public NicBotSolenoidControl()
      {
         InitializeComponent();
      }

      #endregion
   }
}
