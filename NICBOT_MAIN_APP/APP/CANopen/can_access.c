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
