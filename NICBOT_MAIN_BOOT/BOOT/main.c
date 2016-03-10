/**************************************************************************
PROJECT:     CANopen Bootloader for Atmel AT90CAN128 AVR Microcontroller
MODULE:      MAIN.C
CONTAINS:    main() function to be called after every reset, initialization,
             error handling and main execution loop.
ENVIRONMENT: Compiled and tested with IAR EW for AVR on
             Atmel STK500/501 with AT90CAN128
             Compiler Version 5.11b
COPYRIGHT:   This version Embedded Systems Academy, Inc.
             Developed by Embedded Systems Academy, Inc.
VERSION:     $LastChangedDate: 2016-02-26 20:53:15 +0100 (Fri, 26 Feb 2016) $
             $LastChangedRevision: 3576 $
***************************************************************************/

#include <avr/io.h>
#include "config.h"
#include "flash_boot_lib.h"
#include "types.h"
#include "mcu.h"
#include "userdefs.h"
#include "timer.h"
#include "cop.h"
#include "hw.h"
#include "canhw.h"
#include "flash.h"
#include "sdo.h"
#include "main.h"


/**************************************************************************
 LOCAL DEFINITIONS
***************************************************************************/

#define Disable_interrupt()     { asm ("cli"::) ; }

/**************************************************************************
 EXTERNAL REFERENCES
***************************************************************************/

extern const U16 boot_crc;	

/**************************************************************************
 PUBLIC DEFINITIONS
***************************************************************************/

// Global status information for the node
NodeStatusType gNodeStatus;


/**************************************************************************
 LOCAL FUNCTION PROTOTYPES
***************************************************************************/

static RunModeType MAIN_Get_Execution_Mode (
  void
  );

static void MAIN_Init (
  void
  );


/**************************************************************************
 LOCAL FUNCTIONS
***************************************************************************/

/**************************************************************************
DOES:    Determine the execution mode of the bootloader, depending on the
         cause-of-reset status.
RETURNS: Execution mode
**************************************************************************/
static RunModeType MAIN_Get_Execution_Mode (
  void
  )
{
  RunModeType return_val = EXECMODE_POWERUP;

  if (BITSET(MCUSR,WDRF))
  { // Watchdog reset occured?
    return_val = EXECMODE_RESET;
  }
  else if (BITSET(MCUSR,PORF))
  { // Power-up reset occured?
    return_val = EXECMODE_POWERUP;
  }
  else if (BITSET(MCUSR,JTRF))
  { // JTAG reset (during debugging) is handled just like power-up reset
    return_val = EXECMODE_POWERUP;
  }
  else
  { // If not power-up reset and not watchdog reset it must be a direct jump
    return_val = EXECMODE_JUMP;
  }

  // Reset MCU Status Register
  MCUSR = 0x00;

  return (return_val);
}



/**************************************************************************
DOES:    Initialization of main application variables
RETURNS: -
**************************************************************************/
static void MAIN_Init (
  void
  )
{
  UNSIGNED16  checksums;

  checksums = 0x0000U;

  gNodeStatus.run_mode = MAIN_Get_Execution_Mode ();

  // Initialize error register and error list
  gNodeStatus.error_register          = ERRREG_NOERROR;
  gNodeStatus.errorfield_errors       = 0;

  // Read EEPROM parameters (Node ID, baudrate, serial number) or set default
  if (!FLASH_EEPROM_Read_Params(&checksums))
  { // If EEPROM checksum not correct, signal error. Extra info are the checksums.
    MAIN_Signal_Error(ERRCODE_DATA, checksums);
  }

  return;
}



/**************************************************************************
 PUBLIC FUNCTIONS
***************************************************************************/

/**************************************************************************
DOES:    Add an error to the error list and set error status
RETURNS: -
**************************************************************************/
void MAIN_Signal_Error (
  ErrorCodeType error_code,  // Error code according to DS-301
  UNSIGNED16    addl_info    // Additional error info, depends on error
  )
{
  UNSIGNED8 error_highest = 0U;
  UNSIGNED8 i             = 0U;

  // Is list full?
  if (gNodeStatus.errorfield_errors >= NUM_ERRORS_MAX)
  { // If list is full, the number of items to shift is one less the total
    error_highest = NUM_ERRORS_MAX-1;
  }
  else
  { // If list is not yet full, we have to shift all items
    error_highest = gNodeStatus.errorfield_errors;

    // One error more in the list
    gNodeStatus.errorfield_errors++;
  }

  // Shift the list down to make room for new error (may remove the oldest error)
  for (i = error_highest; i > 0 ; i--)
  {
    gNodeStatus.errorfield_errorlist[i] = gNodeStatus.errorfield_errorlist[i-1];
    gNodeStatus.errorfield_addl_info[i] = gNodeStatus.errorfield_addl_info[i-1];
  }

  // Add the new error on top
  gNodeStatus.errorfield_errorlist[0] = error_code;
  gNodeStatus.errorfield_addl_info[0] = addl_info;

  // Set error register
  gNodeStatus.error_register = ERRREG_GENERROR;

  return;
}



/**************************************************************************
DOES:    Main function to be executed after reset. Checks the reset cause
         and flash checksum
RETURNS: -
**************************************************************************/
int main (
  void
  )
{
  UNSIGNED16 checksum;

  // Disable all interrupts (just in case we jumped here)
  Disable_interrupt();

  // Initialize low-level hardware
  HW_Init();

  // Initialize Flash/EEPROM module and routines
  FLASH_Init();

  // Read node configuration and initialize status/error variables
  MAIN_Init();

  // Initialize Timer
  TIMER_Init();

  // Initialize CAN contoller
  CANHW_Init();

  // Initialize SDO handler
  SDO_Init();

  // If the bootloader is executed from power-up reset, initiate
  // waiting period of 60 seconds before attempting to start the
  // application.
  // Otherwise, we assume that the bootloader was called from the
  // application. In this case, send the bootup message (and don't
  // wait the 60 seconds).
  if ( gNodeStatus.run_mode == EXECMODE_POWERUP )
  {
    // Set the timer for the main waiting period in number of ticks
    TIMER_Set(TIMER_MAIN, WAIT_TIMEOUT*(1000/TIMERTICK_MS));
    gNodeStatus.run_mode = EXECMODE_TIMER;
  }
  else
  {
    CANHW_Send_Boot();
    gNodeStatus.run_mode = EXECMODE_NORMAL;
  }

  // Calculat bootloader checksum
  FLASH_Checksum_OK(BL_CHECKSUM_START, BL_CHECKSUM_END, BL_CHECKSUM_ADR, &checksum);

  // Check the bootloader (provides reference for embedded CRC)
  if (boot_crc == checksum)
  { // no error - do nothing
    ;
  }
  else
  { // Signal error. Extra info is the calculated checksum.
    MAIN_Signal_Error(ERRCODE_INTERNAL, checksum);
  }

  // Check the main application checksum and set error status if wrong
  if ( FLASH_Checksum_OK(AP_CHECKSUM_START, AP_CHECKSUM_END,
                         AP_CHECKSUM_ADR,   &checksum)     )
  { // no error - do nothing
    ;
  }
  else
  { // Signal error. Extra info is the calculated checksum.
    MAIN_Signal_Error(ERRCODE_USER, checksum);
    // Don't auto-start application after timeout
    gNodeStatus.run_mode = EXECMODE_NORMAL;
  }

  // Endless loop to execute the bootloader. The only exit point is
  // a jump to the application if the condition for execution is
  // fulfilled.
  for ( ;; )
  {
    BOOLEAN message_processed;

    // Update timers
    TIMER_Poll();

    // Serve the watchdog
    COP_Serve();

    // Check for a waiting CAN message and process them (SDO, NMT).
    // This can change the execution mode!
    message_processed = CANHW_Process_Messages();

    // If requested, check the application and jump to it, if successful
    if ( gNodeStatus.run_mode == EXECMODE_RUN )
    {
      // Jump to main application, if the checksum is ok
      if ( FLASH_Checksum_OK(AP_CHECKSUM_START, AP_CHECKSUM_END,
                             AP_CHECKSUM_ADR,   &checksum)     )
      {
        // Set the location of the interrupt vectors to 0
        MCUCR = BIT(IVCE);
        MCUCR = 0x00;
        // Call the absolute address RESVEC_ADR
        ((void(*)(void))RESVEC_ADR)();
      }
      else
      { // Checksum not valid: Don't start application and report error, stay here
        MAIN_Signal_Error(ERRCODE_USER, checksum); // Signal error. Extra info is the calculated checksum.
        gNodeStatus.run_mode = EXECMODE_NORMAL;
      }
    }
    else if ( gNodeStatus.run_mode == EXECMODE_TIMER )
    { // check for other conditions when running in timed mode

      if ( message_processed )
      { // After a successfully received message leave the timed mode
        gNodeStatus.run_mode = EXECMODE_NORMAL;
      }
      else if ( TIMER_Expired(TIMER_MAIN) )
      { // Check for initial 60 second waiting period, if expired prepare
        // for running the application.
        gNodeStatus.run_mode = EXECMODE_RUN;
      }
      else
      { // do nothing
        ;
      }
    }
    else
    { // do nothing
      ;
    }
  } // for ever loop

  // don't return from here
  return(0);
}


/***************************************************************************
END OF FILE
***************************************************************************/