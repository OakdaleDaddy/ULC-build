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
* @brief Function to read from process image.
*
* @param index : index of object definition
* @param subIndex : sub index of object definition
* @param destination : pointer to memory to contain value
* @param length : maximum amount to transfer
*/
void CAN_ReadProcessImage(U16 index, U8 subIndex, void * destination, U8 length);

/**
* @brief Function to write to process image.
*
* @param index : index of object definition
* @param subIndex : sub index of object definition
* @param source : pointer to memory to define value
* @param length : maximum amount to transfer
*/
void CAN_WriteProcessImageData(U16 index, U8 subIndex, void * source, U8 length);

/**
* @brief Function to write to process image.
*
* @param index : index of object definition
* @param subIndex : sub index of object definition
* @param value : value to write
* @param length : maximum amount to transfer
*/
void CAN_WriteProcessImageValue(U16 index, U8 subIndex, U32 value, U8 length);

/**
* @brief Function to send debug information.
*
* @param a : debug value a
* @param b : debug value b
*/
void CAN_SendDebug(U32 a, U32 b);

/**
* @brief Function to get interface time count.
*
* @return millisecond tick count
*/
U16 CAN_GetTime(void);

/**
* @brief Function to check timeout.
*
* @warning maximum measurable time is 0x8000 (about 32 seconds).
*
* @return 0 : time not reached
* @return 1 : time has expired
*/
U8 CAN_IsTimeExpired(U16 timeCount);

/**
* @brief Function to assign CAN baud rate to use on reset.
*
* @param canBps : kilo baud rate of interface
*/
void CAN_SetBaudrate(U16 canBps);

/**
* @brief Function to assign node ID to use on reset.
*
* @param nodeId : node of device
*/
void CAN_SetNodeId(U8 nodeId);

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
