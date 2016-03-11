/*
*/
/**
* @file
*
* @brief CAN callback declarations.
*
* Functions referenced by the CAN stack and defined within the application 
* process.
*/

#ifndef CAN_CALLBACKS_H
#define CAN_CALLBACKS_H

#include "compiler.h"

/**
* @brief Function to access the device serial number.
*
* The function is called before read accesses to the Object Dictionary entry
* [1018h,0] – Serial Number. It can be used by the application to retrieve the 
* serial number, for example from non-volatile memory.
*
* @return serial number
*/
U32 CAN_GetSerial(void);

/**
* @brief Function to handle application reset request.
*
* The function is called when the CANopen node received the command from the
* NMT Master to hard-reset itself. Both the CANopen communication as well as
* the application is expected to fully reset. This is typically implemented 
* using a watchdog reset.
*/
void CAN_ResetApplication(void);

/**
* @brief Function to signal device state.
* 
* The function is called whenever the CAN protocol stack changes the device 
* state, typically happens after receiving the NMT message.
*
* @params state : new state of the device (0=initializing, 127=pre-operational, 5=operational, 4=stopped)
*/
void CAN_NMTChange(U8 state);

#endif /* CAN_CALLBACKS_H */ 
