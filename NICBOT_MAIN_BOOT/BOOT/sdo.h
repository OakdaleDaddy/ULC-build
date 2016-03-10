/**************************************************************************
PROJECT:     CANopen Bootloader for Atmel AT90CAN128 AVR Microcontroller
MODULE:      SDO.H
CONTAINS:    Include file for implementation of the Service Data Object
             access to the OD
ENVIRONMENT: Compiled and tested with IAR EW for AVR on
             Atmel STK500/501 with AT90CAN128
             Compiler Version 5.11b
COPYRIGHT:   This version Embedded Systems Academy, Inc.
             Developed by Embedded Systems Academy, Inc.
VERSION:     $LastChangedDate: 2013-05-28 02:11:48 +0200 (Tue, 28 May 2013) $
             $LastChangedRevision: 2624 $
***************************************************************************/

/***************************************************************************
 DEFINITIONS AND TYPES
***************************************************************************/

/**************************************************************************
SDO Abort Codes
**************************************************************************/ 
#define SDO_ABORT_TOGGLE          0x05030000UL  // Toggle bit not alternated
#define SDO_ABORT_SDOTIMEOUT      0x05040000UL  // SDO protocol timed out
#define SDO_ABORT_UNKNOWN_COMMAND 0x05040001UL  // Client/server command specifier not valid or unknown
#define SDO_ABORT_UNSUPPORTED     0x06010000UL  // Unsupported access to an object
#define SDO_ABORT_WRITEONLY       0x06010001UL  // Attempt to read a write only object
#define SDO_ABORT_READONLY        0x06010002UL  // Attempt to write a read only object
#define SDO_ABORT_NOT_EXISTS      0x06020000UL  // Object does not exist in the object dictionary
#define SDO_ABORT_PARAINCOMP      0x06040043UL  // General parameter incompatibility reason
#define SDO_ABORT_ACCINCOMP       0x06040047UL  // General internal incompatibility in the device
#define SDO_ABORT_TYPEMISMATCH    0x06070010UL  // Data type does not match, length of service parameter does not match
#define SDO_ABORT_UNKNOWNSUB      0x06090011UL  // Sub-index does not exist
#define SDO_ABORT_VALUE_RANGE     0x06090030UL  // Value range of parameter exceeded (only for write access)
#define SDO_ABORT_TRANSFER        0x08000020UL  // Data cannot be transferred or stored to the application
#define SDO_ABORT_LOCAL           0x08000021UL  // Data cannot be transferred or stored to the application because of local control
#define SDO_ABORT_DEVSTAT         0x08000022UL  // Data cannot be transferred or stored to the application because of the present device state


// Return status type for SDO functions
typedef enum {
  SDO_OD_NOTFOUND,              // Object Dictionary entry doesn't exist
  SDO_ACCESS_OK,                // Object Dictionary entry found and request processed OK
  SDO_ACCESS_FAILED,            // Object Dictionary access failed, SDO Abort sent
  SDO_WRITE_WRONGDATA           // Write access with data out of accepted range
} SDO_ReturnType;


/***************************************************************************
 EXPORTED GLOBALS
***************************************************************************/


/***************************************************************************
 EXPORTED FUNCTIONS
***************************************************************************/

/**************************************************************************
DOES:    Handle an incoimg SDO request.
RETURNS: SDO_ACCESS_OK: Access was made, SDO Response sent
         SDO_ACCESS_FAILED: Access denied, SDO Abort sent
         *ret_index and *ret_subindex, if known
GLOBALS: uses gRxCAN to retrieve SDO request
         uses gTxCAN to transmit SDO response
**************************************************************************/
extern SDO_ReturnType SDO_Handle_SDO_Request (
  UNSIGNED16  *ret_index,    // Index of the OD entry for request, if known
  UNSIGNED8   *ret_subindex  // Subndex of the OD entry for request, if known
  );

/**************************************************************************
DOES:    Initializes SDO protocol and state machine
RETURNS: -
**************************************************************************/
extern void SDO_Init (
  void
  );

/***************************************************************************
END OF FILE
***************************************************************************/
