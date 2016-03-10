﻿
namespace NICBOT.CAN
{
   public enum CommActionTypes
   {
      // applicable to all devices
      NetworkRequest,
      SDODownload,
      SDOUpload,
      PDO1Emit,
      PDO2Emit,
      PDO3Emit,
      PDO4Emit,
      
      // specific to Elmo Whitle Motor
      BinaryInterpreterSet,
      BinaryInterpreterQuery,
      BinaryInterpreterExecute,
   }
}