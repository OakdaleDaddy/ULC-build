/**************************************************************************
PROJECT:     CANopen Bootloader for Atmel AT90CAN128 AVR Microcontroller
MODULE:      HW.H
CONTAINS:    Include file for hardware-specific low-level initializations
             that the bootloader has to do on startup.
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

// Solenoid valve ports
// ====================



/***************************************************************************
 EXPORTED GLOBALS
***************************************************************************/


/***************************************************************************
 EXPORTED FUNCTIONS
***************************************************************************/

/**************************************************************************
DOES:    Hardware-specific initialization
RETURNS: -
***************************************************************************/
extern void HW_Init (
  void
  );

/**************************************************************************
END OF FILE
***************************************************************************/
