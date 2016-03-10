/**************************************************************************
PROJECT:     CANopen Bootloader for Atmel AT90CAN128 AVR Microcontroller
MODULE:      COP.H
CONTAINS:    Include file for COP watchdog functions for the AT90CAN128
             microcontroller
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


/***************************************************************************
 EXPORTED GLOBALS
***************************************************************************/


/***************************************************************************
 EXPORTED FUNCTIONS
***************************************************************************/

/**************************************************************************
DOES:    Serve the watchdog
RETURNS: -
**************************************************************************/
extern void COP_Serve (
  void
  );

/**************************************************************************
DOES:    Force a COP reset upon command
RETURNS: -
**************************************************************************/
extern void COP_Force_Reset (
  void
  );

/**************************************************************************
END OF FILE
***************************************************************************/
