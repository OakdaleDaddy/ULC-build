
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

   public class OsdParameters
   {
      public int HorizontalOffset;
      public int VerticalOffset;

      public string Line1;
      public string Line2;
      public string Line3;
      public string Line4;
      public string Line5;
      public string Line6;
      public string Line7;
      public string Line8;

      public bool ShowDate;
      public bool ShowDistance;
      public bool ShowTime;
      public bool ShowDescription;
      public bool ShowCameraId;

      public OsdParameters()
      {
         this.HorizontalOffset = 0;
         this.VerticalOffset = 0;

         this.Line1 = "";
         this.Line2 = "";
         this.Line3 = "";
         this.Line4 = "";
         this.Line5 = "";
         this.Line6 = "";
         this.Line7 = "";
         this.Line8 = "";

         this.ShowDate = false;
         this.ShowDistance = false;
         this.ShowTime = false;
         this.ShowDescription = true;
         this.ShowCameraId = false;
      }
   }
}