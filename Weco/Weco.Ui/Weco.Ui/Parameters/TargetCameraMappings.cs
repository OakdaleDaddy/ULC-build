namespace Weco.Ui
{
   using System;

   public class TargetCameraMappings
   {
      public int Front { set; get; }
      public int Rear { set; get; }
      public int Top { set; get; }

      public TargetCameraMappings()
      {
         this.Front = 0;
         this.Rear = 0;
         this.Top = 0;
      }

      public TargetCameraMappings(int front, int rear, int top)
      {
         this.Front = front;
         this.Rear = rear;
         this.Top = top;
      }
   }
}

