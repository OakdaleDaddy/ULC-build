
namespace CanDemo.PCANLight
{
   using System;

   public class RingBuffer<T>
   {
      private int lastIndex;
      private int inIndex;
      private int outIndex;

      private T[] data;

      private DateTime getTimeLimit;

      public RingBuffer()
      {
         this.data = new T[1024 * 16];
         this.Reset();
      }

      public RingBuffer(int capacity)
      {
         this.data = new T[capacity];
         this.Reset();
      }

      public void Reset()
      {
         this.lastIndex = 0;
         this.inIndex = 1;
         this.outIndex = 1;
      }

      public bool Store(T value)
      {
         bool result = false;

         if (this.lastIndex != this.inIndex)
         {
            this.getTimeLimit = DateTime.Now;

            this.data[this.inIndex] = value;
            this.inIndex = ((this.inIndex + 1) < this.data.Length) ? (this.inIndex + 1) : 0;
            result = true;
         }

         return (result);
      }

      public T Get()
      {
         T result = default(T);

         if (this.inIndex != this.outIndex)
         {
            result = this.data[this.outIndex];
            this.data[this.outIndex] = default(T);

            this.lastIndex = this.outIndex;
            this.outIndex = ((this.outIndex + 1) < this.data.Length) ? (this.outIndex + 1) : 0;
         }

         return (result);
      }

      public bool Contains()
      {
         bool result = (this.inIndex != this.outIndex) ? true : false;
         return (result);
      }
   }
}