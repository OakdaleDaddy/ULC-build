
namespace Weco.Ui
{
   using System;

   public class LaserCameraMappings
   {
      public int Front { set; get; }
      public int Rear { set; get; }

      public LaserCameraMappings()
      {
         this.Front = 0;
         this.Rear = 0;
      }

      public LaserCameraMappings(int front, int rear)
      {
         this.Front = front;
         this.Rear = rear;
      }
   }
}