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

   public class OsdParameters
   {
      public int HozizontalOffset;
      public int VerticalOffset;
      
      public string Line1;
      public string Line2;
      public string Line3;
      public string Line4;
      public string Line5;

      public bool ShowDate;
      public bool ShowTime;
      public bool ShowCameraId;
      public bool ShowDescription;      
      public bool ShowPipeDisplacement;
      public bool ShowPipePosition;

      public OsdParameters()
      {
         this.HozizontalOffset = 0;
         this.VerticalOffset = 0;

         this.Line1 = "";
         this.Line2 = "";
         this.Line3 = "";
         this.Line4 = "";
         this.Line5 = "";

         this.ShowDate = false;
         this.ShowTime = false;
         this.ShowCameraId = false;
         this.ShowDescription = true;
         this.ShowPipeDisplacement = false;
         this.ShowPipePosition = false;
      }
   }
}