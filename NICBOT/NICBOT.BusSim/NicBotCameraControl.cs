
namespace NICBOT.BusSim
{
   using System;
   using System.Collections.Generic;
   using System.ComponentModel;
   using System.Drawing;
   using System.Data;
   using System.Linq;
   using System.Text;
   using System.Windows.Forms;

   public partial class NicBotCameraControl : UserControl
   {
      #region Fields

      private bool running;
      private bool cameraOn;
      private byte lightLevel;

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
               this.CameraOn = this.cameraOn;
               this.LightLevel = this.lightLevel;
            }
         }

         get
         {
            return (this.running);
         }
      }

      public bool CameraOn
      {
         set
         {
            this.cameraOn = value;

            if ((false != this.running) &&
                (false != this.cameraOn))
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
            return (this.cameraOn);
         }
      }

      public byte LightLevel
      {
         set
         {
            this.lightLevel = value;

            if (false != this.running) 
            {
               this.LightLevelLabel.Text = string.Format("{0}", value.ToString());
               this.LightLevelLabel.BackColor = Color.FromArgb(value, value, value);
            }
            else
            {
               this.LightLevelLabel.Text = string.Format("{0}", 0);
               this.LightLevelLabel.BackColor = Color.FromArgb(0, 0, 0);
            }
         }

         get
         {
            return (this.lightLevel);
         }
      }

      #endregion

      #region Constructor

      public NicBotCameraControl()
      {
         this.InitializeComponent();
      }

      #endregion
   }
}
