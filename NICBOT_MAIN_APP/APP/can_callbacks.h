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

#define CAN_ABORT_UNSUPPORTED (0x06010000UL) /*!< abort code for a value that is not supported */
#define CAN_ABORT_VALUE_HIGH (0x06090031UL) /*!< abort code for a value that is too high */
#define CAN_ABORT_VALUE_LOW (0x06090032UL) /*!< abort code for a value that is low high */
#define CAN_ABORT_GENERAL (0x08000000UL) /*!< abort code for a value that is too high */

/**
* @brief Function to access the device serial number.
*
* @return serial number
*/
U32 CAN_GetSerial(void);

/**
* @brief Function to handle application reset request.
*
* The function is called when the flash needs to be read and communications 
* parameters assigned.
*/
void CAN_ReloadFlash(void);

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
* @brief Function to signal a read of the object dictionary.
*
* The function signals the application that a read request was received
* and is about to be processed.  The call happens BEFORE data is retrieved
* from the process image.  The application can use this call to load new
* values into the process image or reject the request.
*
* @params index : index of OD entry
* @params subIndex : subindex of OD entry
*
* @return 0 : read acceptable.
* @return non-0 : read not acceptable, abort the transfer, see CAN_ABORT codes
*/
U32 CAN_ODRead(U16 index, U8 subIndex);

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
* @return 0 : entry is not handled by this function.
* @return 1 : entry is handled by this function. A valid pointer and data size are returned.
* @return 5 : write-only, abort the transfer
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

/**
* @brief Function to signal a write of the object dictionary.
*
* The function signals the application that a write request was received 
* and is about to be processed.  The call happens BEFORE data is copied to 
* the process image.  The application can use this call to verify the 
* incoming data.
*
* @params index : index of object dictionary entry
* @params subIndex : subindex of OD entry
* @params data : pointer to data buffer
* @params length : length of data
*
* @return 0 : incoming data is good, commit the data
* @return non-0 : incoming data is not good, abort the transfer, see CAN_ABORT codes
*/
U32 CAN_ODWrite(U16 index, U8 subIndex, U8 * data, U8 length);

/**
* @brief Function to signal object dictionary change.
*
* The function signals the receipt of process data stored into the process 
* image, no matter if it came in by PDO or SDO transfer. 
*
* @params index : index of object dictionary entry
* @params subIndex : subindex of object dictionary entry
* @params data : pointer to data buffer
* @params size : size of data buffer
*/
void CAN_ODData(U16 index, U8 subIndex, U8 * data, U8 length);

#endif /* CAN_CALLBACKS_H */ 
