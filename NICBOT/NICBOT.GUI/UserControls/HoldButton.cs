using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NICBOT.GUI
{
   public class HoldButton : NicBotButton
   {
      private Timer holdTimer;
      private bool holdTimeout;

      public bool HoldTimeoutEnable { set; get; }
      public int HoldTimeoutInterval { set; get; }

      public event HoldTimeoutHandler HoldTimeout;

      public new event EventHandler Click;

      public HoldButton()
         : base()
      {
         this.Enter += HoldButton_Enter;

         this.MouseDown += HoldButton_MouseDown;
         this.MouseUp += HoldButton_MouseUp;

         base.Click += HoldButton_Click;

         this.holdTimer = new Timer();
         this.holdTimer.Tick += HoldButton_HoldTimeout;
      }

      void HoldButton_Click(object sender, EventArgs e)
      {
         if (false == this.holdTimeout)
         {
            if (null != this.Click)
            {
               this.Click(this, e);
            }
         }
      }

      void HoldButton_Enter(object sender, EventArgs e)
      {
         this.holdTimeout = false;
      }

      void HoldButton_HoldTimeout(object sender, EventArgs e)
      {
         this.holdTimeout = true;
         this.holdTimer.Stop();

         HoldTimeoutEventArgs holdEventArg = new HoldTimeoutEventArgs();
         this.HoldTimeout(this, holdEventArg);
         this.holdTimeout = !holdEventArg.Handled;

         if (false != holdEventArg.Handled)
         {
            this.Release();
         }
      }

      void HoldButton_MouseUp(object sender, MouseEventArgs e)
      {
         this.holdTimeout = false;
         this.holdTimer.Stop();
      }

      void HoldButton_MouseDown(object sender, MouseEventArgs e)
      {
         if ((false != this.HoldTimeoutEnable) && (0 != this.HoldTimeoutInterval) && (null != this.HoldTimeout))
         {
            this.holdTimer.Interval = this.HoldTimeoutInterval;
            this.holdTimer.Start();
         }
      }


   }
}
