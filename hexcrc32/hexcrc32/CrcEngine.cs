namespace hexcrc32
{
   using System;

   public class CrcEngine
   {
      #region Helper Functions

      private UInt32 Reflect(UInt32 v, int b)
      {
         int i;
         UInt32 t = v;

         for (i = 0; i < b; i++)
         {
            if ((t & 1L) != 0)
            {
               v |= (UInt32)(1L << ((b - 1) - i));
            }
            else
            {
               v &= (UInt32)(~(1L << ((b - 1) - i)));
            }

            t >>= 1;
         }

         return v;
      }

      private UInt32 WidthMask()
      {
         return (UInt32)((((1L << (this.Width - 1)) - 1L) << 1) | 1L);
      }

      #endregion

      #region Properties

      public int Width { set; get; }
      public UInt32 Polynomial { set; get; }
      public UInt32 Initial { set; get; }
      public bool ReflectIn { set; get; }
      public bool ReflectOut { set; get; }
      public UInt32 Xor { set; get; }
      public UInt32 Crc { set; get; }

      #endregion

      #region Constructor 

      public CrcEngine()
      {
      }

      #endregion

      #region Access Functions

      public void Initialize()
      {
         this.Crc = this.Initial;
      }

      public void Next(int ch)
      {
         int i;
         UInt32 uch = (UInt32)ch;
         UInt32 topbit = (UInt32)1L << (this.Width-1);

         if (false != this.ReflectIn)
         {
            uch = this.Reflect(uch, 8);
         }

         this.Crc ^= (uch << (this.Width - 8));
         
         for (i = 0; i < 8; i++)
         {
            if ((this.Crc & topbit) != 0)
            {
               this.Crc = (this.Crc << 1) ^ this.Polynomial;
            }
            else
            {
               this.Crc <<= 1;
            }

            this.Crc &= this.WidthMask();
         }
      }

      public UInt32 Final()
      {
         UInt32 result = 0;

         if (false != this.ReflectOut)
         {
            result = this.Xor ^ this.Reflect(this.Crc, this.Width);
         }
         else
         {
            result = this.Xor ^ this.Crc;
         }

         return (result);
      }

      #endregion
   }
}