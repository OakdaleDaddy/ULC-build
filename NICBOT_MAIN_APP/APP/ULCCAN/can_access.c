/*
*/
/**
* @file
*
* @brief CAN access definitions.
*
* Functions defined to access ULC CAN function.
*/

#ifndef USE_MICROCAN_OPEN

#include "config.h"
#include "can_isp_protocol.h"
#include "can_access.h"

/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */

/* See can_access.h for function description */
/**
* Calls into stack to read from process image.
*/
void CAN_ReadProcessImage(U16 index, U8 subIndex, void * destination, U8 length)
{
   canRead(index, subIndex, destination, length);
}

/* See can_access.h for function description */
/**
* Calls into stack to write to process image.
*/
void CAN_WriteProcessImageData(U16 index, U8 subIndex, void * source, U8 length)
{
   canWrite(index, subIndex, source, length);
}

/* See can_access.h for function description */
/**
* Calls into stack to write to process image.
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
   canDebug(a,b);
}

/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */

/* See can_access.h for function description */
/**
* Calls into stack to set baud rate for next reset.
*/
void CAN_SetBaudrate(U16 canBps)
{
   canSetBaudrate(canBps);
}

/* See can_access.h for function description */
/**
* Calls into stack to set node ID for next reset.
*/
void CAN_SetNodeId(U8 nodeId)
{
   canSetNodeId(nodeId);
}

/* See can_access.h for function description */
/**
* Calls into stack to reset communication.
*/
void CAN_ResetCommunication(void)
{
   canInit();
}

/* See can_access.h for function description */
/**
* Calls to update stack.
*/
U8 CAN_ProcessStack(void)
{
   U8 result;

   // Update CAN Stack
   result = canUpdate();

   return(result);
}

/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
#endif /* not USE_MICROCAN_OPEN */
