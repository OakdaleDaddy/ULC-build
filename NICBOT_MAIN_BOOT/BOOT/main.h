/**************************************************************************
PROJECT:     CANopen Bootloader for Atmel AT90CAN128 AVR Microcontroller
MODULE:      MAIN.H
CONTAINS:    Main include file
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

// Type definition to identify a mode of execution for the bootloader
typedef enum {
  EXECMODE_POWERUP,  // Powerup reset (first entry)
  EXECMODE_RESET,    // Watchdog reset (entry from application)
  EXECMODE_JUMP,     // Jump from application
  EXECMODE_TIMER,    // Wait for timeout before running the application
  EXECMODE_NORMAL,   // Normal execution: Wait for commands and execute
  EXECMODE_RUN       // Bootloader finished - run application
} RunModeType;

// Type definition for the error status (error register)
typedef enum {
  ERRREG_NOERROR  = 0x00,     // Initial state (no error)
  ERRREG_GENERROR = 0x01      // General error
} ErrorRegType;

// Type definition for the error list (emergency codes)
typedef enum {
  ERRCODE_NOERROR  = 0x0000U,  // No Error/Reset
  ERRCODE_GENERIC  = 0x1000U,  // Generic Error
  ERRCODE_INTERNAL = 0x6100U,  // Internal software error (bootloader checksum)
  ERRCODE_USER     = 0x6200U,  // User software error (application checksum)
  ERRCODE_DATA     = 0x6300U,  // Data set error (EEPROM checksum)
  ERRCODE_PROTOCOL = 0x8200U   // Protocol error (SDO communication)
} ErrorCodeType;

// This structure holds all bootloader global configuration and
// processing data.
typedef struct
{
  UNSIGNED8     node_id;                 // Current Node ID (1-127)
  BOOLEAN       baudrate;                // Current baudrate (main/alternative)
  UNSIGNED32    serial;                  // Serial number
  RunModeType   run_mode;                // Current execution mode
  ErrorRegType  error_register;          // Error register for OD entry [1001,00]
  UNSIGNED8     errorfield_errors;       // Number of recent errors (0..NUM_ERRORS_MAX)
  ErrorCodeType errorfield_errorlist[NUM_ERRORS_MAX]; // Errors for Pre-defined Error Field OD entry [1003,1-NUM_ERRORS_MAX]
  UNSIGNED16    errorfield_addl_info[NUM_ERRORS_MAX]; // Attitional Error Info for Pre-defined Error Field OD entry [1003,0-NUM_ERRORS_MAX]
} NodeStatusType;


/***************************************************************************
 EXPORTED GLOBALS
***************************************************************************/

// Reference to global status structure
extern NodeStatusType gNodeStatus;


/***************************************************************************
 EXPORTED FUNCTIONS
***************************************************************************/

/**************************************************************************
DOES:    Add an error to the error list and set error status
RETURNS: -
**************************************************************************/
extern void MAIN_Signal_Error ( 
  ErrorCodeType error_code,  // Error code according to DS-301
  UNSIGNED16    addl_info    // Additional error info, depends on error
  );


/***************************************************************************
END OF FILE
***************************************************************************/
