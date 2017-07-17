
namespace Weco.Ui
{
   using System;

   public class LightSelectParameters
   {
      public string Location { set; get; }
      public int LightIntensity { set; get; }
      public int LightChannelMask { set; get; }

      public LightSelectParameters()
      {
         this.Location = "";
         this.LightIntensity = 0;
         this.LightChannelMask = 0;
      }

      public LightSelectParameters(string location, int lightIntensity, int lightChannelMask)
      {
         this.Location = location;
         this.LightIntensity = lightIntensity;
         this.LightChannelMask = lightChannelMask;
      }
   }
}

