using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NICBOT.BusSim
{
   public class NicbotBodyDrillContext
   {
      public delegate void LaserHandler(bool on);

      public NicbotBodyDrillStates state;
      public byte control;
      public int axis;
      public Int16 manualSetPoint;
      public Int16 processedSetPoint;
      public UInt16 retractMask;

      public LaserHandler OnLaser;

      public NicbotBodyDrillContext()
      {
         this.state = NicbotBodyDrillStates.idle;
         this.control = 0;
         this.axis = 0;
         this.manualSetPoint = 0;
         this.processedSetPoint = 0;
         this.retractMask = 0;

         this.OnLaser = null;
      }
   }
}
