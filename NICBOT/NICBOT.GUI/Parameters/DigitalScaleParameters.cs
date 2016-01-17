namespace NICBOT.GUI
{
   using System;

   public class DigitalScaleParameters
   {
      public string Location;

      public int Port;
      public int BaudRate;

      public DigitalScaleParameters()
      {
         this.Location = "";

         this.Port = 0;
         this.BaudRate = 0;
      }

      public DigitalScaleParameters(string location, int port, int baudRate)
      {
         this.Location = location;

         this.Port = port;
         this.BaudRate = baudRate;
      }
   }
}
