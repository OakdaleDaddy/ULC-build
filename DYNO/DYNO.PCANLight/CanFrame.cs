namespace DYNO.PCANLight
{
   using System;

   public class CanFrame
   {
      public int cobId;
      public byte[] data;

      public CanFrame()
      {
         cobId = 0;
         data = null;
      }

      public CanFrame(int cobId, byte[] data)
      {
         this.cobId = cobId;
         this.data = data;
      }

      public CanFrame(int cobId, byte[] data, int length)
      {
         this.cobId = cobId;
         this.data = new byte[length];

         for (int i = 0; i < length; i++)
         {
            byte ch = 0;

            if ((null != data) && (i < data.Length))
            {
               ch = data[i];
            }

            this.data[i] = ch;
         }
      }
   }
}
