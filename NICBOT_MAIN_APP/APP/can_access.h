/*
*/
/**
* @file
*
* @brief CAN access declarations.
*
* Functions defined by the CAN stack and referenced by the application 
* process.
*/

#ifndef CAN_ACCESS_H
#define CAN_ACCESS_H

#include "compiler.h"

/**
* @brief Function to handle application reset request.
*
* This function is called to completely re-initialize the CANopen communication.
* This includes re-initialization of the CAN interface. This function is called 
* upon initialization but also when the CANopen node received the NMT Master
* command to soft-reset itself.
*/
void CAN_ResetCommunication(void);

/**
* @brief Function to update the CAN stack.
*
* This function must be called periodically to keep the CANopen stack operating.
* With each call it is checked if the CAN receive queue contains a message that
* needs to be processed. Depending on configuration it is also checked if timers
* expired or process data changed. This is typically called from the main 
* while(1) loop. For best operation this should be called at least once per 
* millisecond. If called less often multiple calls should be executed (see 
* return value below).
*
* @return 1 : something was processed
* @return 0 : nothing to do
*/
U8 CAN_ProcessStack(void);

#endif /* CAN_ACCESS_H */ 
