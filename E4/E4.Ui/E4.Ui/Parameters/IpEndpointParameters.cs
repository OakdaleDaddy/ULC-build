namespace E4.Ui
{
   using System;

   public class IpEndpointParameters
   {
      public string Name;
      public string Address;
      public int Port;

      public IpEndpointParameters()
      {
         this.Name = "";
         this.Address = "";
         this.Port = 0;
      }

      public IpEndpointParameters(string name, string address, int port)
      {
         this.Name = name;
         this.Address = address;
         this.Port = port;
      }
   }
}
