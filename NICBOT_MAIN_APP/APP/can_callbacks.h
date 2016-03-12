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
* [1018h,0] � Serial Number. It can be used by the application to retrieve the 
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

/**
* @brief Function to evaluate a SDO upload request.
*
* The function can be used to implement custom Object Dictionary read entries
* of any length.  Data is transferred in segmented mode or block mode if
* activated.
*
* @params server : SDO server number
* @params index : index of OD entry
* @params subIndex : subindex of OD entry
* @params totalSize : total size of data, only set if greater than *size
* @params size : size of data buffer
* @params pData : pointer to data buffer
*
* @return 0: The specified OD entry is not handled by this function.
* @return 1: The specified OD entry is handled by this function. A valid pointer and data size are returned.
* @return 5: An SDO abort SDO_ABORT_WRITEONLY is generated.
*/
U8 CAN_AppSDOReadInit(U8 server, U16 index, U8 subIndex, U32 * totalSize, U32 * size, U8 ** pData);

/**
* @brief Function to transfer additional data into upload data buffer.
*
* The function is called upon completion of a SDO upload transfer, or 
* whenever the source buffer has been completely transferred and needs to be 
* refilled to deliver more data.
*
* The value *size returns how many more bytes are going to be transferred to 
* the SDO Client reading from the entry, or 0 if the read transfer has finished. 
*
* Before returning from this function with a size value greater than zero, the
* read buffer given in the CAN_AppSDOReadInit function needs to be updated 
* to point to the next block of data.
*
* @params server : SDO server number
* @params index : index of OD entry
* @params subIndex : subindex of OD entry
* @params size : size of data buffer
*/
void CAN_AppSDOReadComplete(U8 server, U16 index, U8 subIndex, U32 * size);

#endif /* CAN_CALLBACKS_H */ 
