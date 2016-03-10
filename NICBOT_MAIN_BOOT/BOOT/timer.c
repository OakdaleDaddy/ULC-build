/**************************************************************************
PROJECT:     CANopen Bootloader for Atmel AT90CAN128 AVR Microcontroller
MODULE:      TIMER.C
CONTAINS:    Timer functions for the AT90CAN128 microcontroller
ENVIRONMENT: Compiled and tested with IAR EW for AVR on
             Atmel STK500/501 with AT90CAN128
             Compiler Version 5.11b
COPYRIGHT:   This version Embedded Systems Academy, Inc.
             Developed by Embedded Systems Academy, Inc.
VERSION:     $LastChangedDate: 2016-02-26 20:53:15 +0100 (Fri, 26 Feb 2016) $
             $LastChangedRevision: 3576 $
***************************************************************************/

#include "types.h"
#include "mcu.h"
#include "userdefs.h"
#include "timer.h"


/**************************************************************************
 LOCAL DEFINITIONS
***************************************************************************/

// Local timer variables that are automatically
// counted down. Used for individual timeouts.
static UNSIGNED16 volatile timer[NUM_TIMERS];


/**************************************************************************
 EXTERNAL REFERENCES
***************************************************************************/


/**************************************************************************
 PUBLIC DEFINITIONS
***************************************************************************/


/**************************************************************************
 LOCAL FUNCTION PROTOTYPES
***************************************************************************/


/**************************************************************************
 LOCAL FUNCTIONS
***************************************************************************/


/**************************************************************************
 PUBLIC FUNCTIONS
***************************************************************************/

/**************************************************************************
DOES:    Configure and start the Timer
RETURNS: -
**************************************************************************/
void TIMER_Init (
  void
  )
{
  UNSIGNED8 timer_nr = 0;

  TIMSK0 = 0x00; // No interrupts - only polling

  // Timer clock = system clock/1024
  // Set OC0A on compare match
  // Clear Timer on Compare Mode
  TCCR0A = BIT(CS02) | BIT(CS00) | BIT(COM0A1) | BIT(COM0A0) | BIT(WGM01);

  TIFR0  = BIT(OCF0A) | BIT(TOV0); // Clear OCF0A/TOV0

  // Timings for 16 MHz CPU clock speed:
  //   1 ms cycle with OCR0 = 16
  //   2 ms cycle with OCR0 = 31
  //   5 ms cycle with OCR0 = 78
  OCR0A = OCR0_INIT;

  TCNT0 = 0x00; // Reset counter

  // Initialize timeout variables
  for (timer_nr = 0; timer_nr < NUM_TIMERS; timer_nr++)
  {
    timer[timer_nr] = 0;
  }

  return;
}


/**************************************************************************
DOES:    Set a timer to a certain timeout, measured in timer ticks. If the
         timer is enabled, it will then be counted down to 0 by
         Timer_Poll(). Timer_Expired() can be used to determine whether
         the time has passed.
RETURNS: -
**************************************************************************/
void TIMER_Set (
  UNSIGNED8  timer_nr, // Number of the timer to set
  UNSIGNED16 timeout   // Timeout in number of timer ticks
  )
{
  timer[timer_nr] = timeout;

  return;
}


/**************************************************************************
DOES:    Set a timer to a certain timeout, measured in timer ticks. If the
         timer is enabled, it will then be counted down to 0 by
         Timer_Poll().
RETURNS: -
**************************************************************************/
BOOLEAN TIMER_Expired (
  UNSIGNED8 timer_nr   // Number of the timer to check
  )
{
  BOOLEAN return_val;

  return_val = (timer[timer_nr] == 0);

  return (return_val);
}


/**************************************************************************
DOES:    Poll the Timer
         This function serves as a replacement for a real-time interrupt
         and must be called often enough. It is less accurate than an
         interrupt version but sufficient for the bootloader.
RETURNS: -
**************************************************************************/
void TIMER_Poll (
  void
  )
{
  UNSIGNED8 timer_nr = 0;

  if ( BITSET(TIFR0,OCF0A) ) // Compare event?
  {
    TIFR0 = BIT(OCF0A) | BIT(TOV0); // Clear OCF0A/TOV0

    // Timer tick overflow: Count down all timeout
    // variables until they reach 0.
    for (timer_nr = 0; timer_nr < NUM_TIMERS; timer_nr++)
    {
      if (timer[timer_nr] > 0)
      {
        timer[timer_nr]--;
      }
    }
  }

  return;
}

/***************************************************************************
END OF FILE
***************************************************************************/
