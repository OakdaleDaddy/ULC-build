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
* Calls into stack to reset communication.
*/
void CAN_ResetCommunication(void)
{
   can_isp_protocol_init();
}

/* See can_access.h for function description */
/**
* Calls to update stack.
*/
U8 CAN_ProcessStack(void)
{
   U8 result;

   // Update CAN Stack
   result = can_isp_protocol_task();

   return(result);
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
#endif /* not USE_MICROCAN_OPEN */
