
namespace Weco.Ui
{
   using System;

   using Ui.Controls;

   public class CameraMaps
   {
      public string Name { set; get; }
      public CameraMap[] Maps { set; get; }

      public CameraMaps()
      {
         this.Name = "";
         this.Maps = null;
      }

      public CameraMaps(string name, CameraMap[] maps)
      {
         this.Name = name;
         this.Maps = maps;
      }

      public int GetIndex(Controls.SystemLocations camera)
      {
         int result = 0;

         for (int i = 0; i < this.Maps.Length; i++)
         {
            if (this.Maps[i].SystemLocation == camera)
            {
               result = this.Maps[i].Index;
            }
         }

         return (result);
      }

   }
}