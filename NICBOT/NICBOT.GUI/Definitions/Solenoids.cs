
namespace NICBOT.GUI
{
   /// <summary>
   /// List of nicbot solenoids.
   /// </summary>
   /// <remarks>Must match bit order expected by device.</remarks>
   public enum Solenoids : int
   {
      frontDrillCover,
      frontNozzleExtend,

      rearDrillCover,
      rearNozzleExtend,

      frontArmExtend,
      frontArmRetract,

      rearArmExtend,
      rearArmRetract,

      lowerArmExtend,
      lowerArmRetract,

      sensorExtend,
      sensorRetract,

      sensorArmStow,
      sensorArmDeploy,      
   }
}
