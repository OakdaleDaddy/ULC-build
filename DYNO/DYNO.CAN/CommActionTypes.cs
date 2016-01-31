
namespace DYNO.CAN
{
   public enum CommActionTypes
   {
      // applicable to all device
      NetworkRequest,
      SDODownload,
      SDOUpload,
      PDO1Emit,
      PDO2Emit,
      PDO3Emit,
      PDO4Emit,
      
      // specific to Elmo Whistle Motor
      BinaryInterpreterSet,
      BinaryInterpreterQuery,
      BinaryInterpreterExecute,
   }
}