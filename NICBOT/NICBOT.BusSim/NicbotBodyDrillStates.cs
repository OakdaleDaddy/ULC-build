using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NICBOT.BusSim
{
   public enum NicbotBodyDrillStates
   {
      idle,
      manual,
      homeRetractToLimit,
      homeStopFromRetract,
      homeExtendToNotLimit,
      homeStopFromExtend,
      homeBackoff,
      stop,
      faulted,
   }
}
