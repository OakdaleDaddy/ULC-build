/**************************************************************************
PROJECT:     CANopen Bootloader for Atmel AT90CAN128 AVR Microcontroller
MODULE:      TIMER.H
CONTAINS:    Include file for timer functions for the AT90CAN128
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

// Number of local timers
#define NUM_TIMERS  1

// Assignment of local timers to functions
#define TIMER_MAIN  0


/***************************************************************************
 EXPORTED GLOBALS
***************************************************************************/


/***************************************************************************
 EXPORTED FUNCTIONS
***************************************************************************/

/**************************************************************************
DOES:    Configure and start the Timer
RETURNS: -
**************************************************************************/
extern void TIMER_Init (
  void
  );

/**************************************************************************
DOES:    Set a timer to a certain timeout, measured in timer ticks. If the
         timer is enabled, it will then be counted down to 0 by
         Timer_Poll(). Timer_Expired() can be used to determine whether
         the time has passed.
RETURNS: -
**************************************************************************/
extern void TIMER_Set (
  UNSIGNED8  timer_nr, // Number of the timer to set
  UNSIGNED16 timeout   // Timeout in number of timer ticks
  );

/**************************************************************************
DOES:    Set a timer to a certain timeout, measured in timer ticks. If the
         timer is enabled, it will then be counted down to 0 by
         Timer_Poll(). 
RETURNS: -
**************************************************************************/
extern BOOLEAN TIMER_Expired (
  UNSIGNED8 timer_nr   // Number of the timer to check
  );

/**************************************************************************
DOES:    Poll the Timer
         This function serves as a replacement for a real-time interrupt
         and must be called often enough. It is less accurate than an
         interrupt version but sufficient for the bootloader.
RETURNS: -
**************************************************************************/
extern void TIMER_Poll (
  void
  );


/***************************************************************************
END OF FILE
***************************************************************************/
