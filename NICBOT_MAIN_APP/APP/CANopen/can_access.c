/*
*/
/**
* @file
*
* @brief CAN access definitions.
*
* Functions defined to access CANopen function.
*/

#ifdef USE_MICROCAN_OPEN

#include "mco.h"
#include "mcop.h"
#include "mcohw.h"

#include "can_access.h"

/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* See can_access.h for function description */
/**
* Locates entry, and copies into destination memory.
*/
void CAN_ReadProcessImage(U16 index, U8 subIndex, void * dest, U8 length)
{
   UNSIGNED16 found;
   OD_PROCESS_DATA_ENTRY MEM_CONST *pOD;
   UNSIGNED16 offset;
   UNSIGNED8 objectLength;
   
   found = MCO_SearchODProcTable(index, subIndex);
   
   if (found != 0xFFFF)
   {
      pOD = OD_ProcTablePtr(found);
      offset = pOD->off_hi;
      offset <<= 8;
      offset +=  pOD->off_lo; 

      objectLength = pOD->len & 0x0F;

      if (objectLength > length)
      {
         objectLength = length;
      }       

      PI_READ(PIACC_APP, offset, dest, objectLength);
   }
}

/* See can_access.h for function description */
/**
* Locates entry, and copies into process image.
*/
void CAN_WriteProcessImageData(U16 index, U8 subIndex, void * source, U8 length)
{
   UNSIGNED16 found;
   OD_PROCESS_DATA_ENTRY MEM_CONST *pOD;
   UNSIGNED16 offset;
   UNSIGNED8 objectLength;
   
   found = MCO_SearchODProcTable(index, subIndex);
   
   if (found != 0xFFFF)
   {
      pOD = OD_ProcTablePtr(found);
      offset = pOD->off_hi;
      offset <<= 8;
      offset +=  pOD->off_lo;

      objectLength = pOD->len & 0x0F;

      if (objectLength > length)
      {
         objectLength = length;
      }

      PI_WRITE(PIACC_APP, offset, source, objectLength);
   }
}

/* See can_access.h for function description */
/**
* Locates entry, and copies into process image.
*/
void CAN_WriteProcessImageValue(U16 index, U8 subIndex, U32 value, U8 length)
{
   CAN_WriteProcessImageData(index, subIndex, (U8 *)&value, length);
}

/* See can_access.h for function description */
/**
* Calls into stack to send debug frame.
*/
void CAN_SendDebug(U32 a, U32 b)
{
   MCOP_PushDebug(a, b);
}


/* See can_access.h for function description */
/**
* Calls into stack to assign baud rate for the next reset.
*/
void CAN_SetBaudrate(U16 canBps)
{
   MCOUSER_SetBaudrate(canBps);
}

/* See can_access.h for function description */
/**
* Calls into stack to assign node ID for the next reset.
*/
void CAN_SetNodeId(U8 nodeId)
{
   MCOUSER_SetNodeId(nodeId);
}

/* See can_access.h for function description */
/**
* Calls into stack to reset communication.
*/
void CAN_ResetCommunication(void)
{
   MCOUSER_ResetCommunication();
}

/* See can_access.h for function description */
/**
* Calls to update stack.
*/
U8 CAN_ProcessStack(void)
{
   U8 result;

   // Update CAN Stack
   result = MCO_ProcessStack();

   // Check for CAN Err, auto-recover     
   if (MCOHW_GetStatus() & HW_BOFF)      
   {
       
      MCOUSER_FatalError(0xF6F6);
     
   }

   return(result);
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
#endif /* USE_MICROCAN_OPEN */
