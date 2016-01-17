namespace NICBOT.GUI
{
   using System;

   public enum MovementWheelModes
   {
      /// <summary>
      /// both solenoids are off
      /// </summary>
      neither,

      axial,
      circumferential,

      /// <summary>
      /// both solenoids are on
      /// </summary>
      both,
   }
}
