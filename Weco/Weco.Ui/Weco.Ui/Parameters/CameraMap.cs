
namespace Weco.Ui
{
   using System;

   using Ui.Controls;

   public class CameraMap
   {
      public int Index { set; get; }
      public SystemLocations SystemLocation { set; get; }

      public CameraMap()
      {
         this.Index = 0;
         this.SystemLocation = SystemLocations.none;
      }

      public CameraMap(int index, SystemLocations systemLocation)
      {
         this.Index = index;
         this.SystemLocation = systemLocation;
      }
   }
}