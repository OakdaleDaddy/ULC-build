
namespace Weco.Ui
{
   using System;

   public class CameraSelectParameters
   {
      public string Location { set; get; }
      public int LightIntensity { set; get; }
      public int LightChannelMask { set; get; }

      public CameraSelectParameters()
      {
         this.Location = "";
         this.LightIntensity = 0;
         this.LightChannelMask = 0;
      }

      public CameraSelectParameters(string location, int lightIntensity, int lightChannelMask)
      {
         this.Location = location;
         this.LightIntensity = lightIntensity;
         this.LightChannelMask = lightChannelMask;
      }
   }
}

