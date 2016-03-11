/**************************************************************************
MODULE:    MAIN
CONTAINS:  Example application using MicroCANopen
           Philips LPC2000 derivatives with CAN interface.
           Compiled and Tested with Keil Tools www.keil.com
COPYRIGHT: Embedded Systems Academy, Inc. 2002-2015.
           All rights reserved. www.microcanopen.com
DISCLAIM:  Read and understand our disclaimer before using this code!
           www.esacademy.com/disclaim.htm
           This software was written in accordance to the guidelines at
           www.esacademy.com/software/softwarestyleguide.pdf
LICENSE:   THIS IS THE COMMERCIAL VERSION OF MICROCANOPEN PLUS
           ONLY USERS WHO PURCHASED A LICENSE MAY USE THIS SOFTWARE
           See file license_commercial_plus.txt or
           www.microcanopen.com/license_commercial_plus.txt
VERSION:   6.20, ESA 11-MAY-15
           $LastChangedDate: 2015-05-09 19:41:45 -0300 (Sat, 09 May 2015) $
           $LastChangedRevision: 3390 $
***************************************************************************/ 

#if 0

#include "mco.h"
#include "mcop.h"
#include "mcohw.h"


/**************************************************************************
GLOBAL VARIABLES
**************************************************************************/

// external declaration for the process image array
extern UNSIGNED8 MEM_PROC gProcImg[];


/**************************************************************************
DOES:    The main function
RETURNS: nothing
**************************************************************************/
int main(
  void
  )
{
UNSIGNED32 pot;      // Analog value / counter
UNSIGNED32 LED;      // LED output value
UNSIGNED8 buf[4];    // buffer to process image

const unsigned long led_mask[] = { 1<<28, 1<<29, 1UL<<31, 1<<2, 1<<3, 1<<4, 1<<5, 1<<6 };

  //SystemInit();

#if 0
  LPC_GPIO1->FIODIR = 0xB0000000; // P1 LED Outputs
  LPC_GPIO2->FIODIR = 0x0000007C; // P2 LED Outputs
  // Wait for all HW to catch up
  for (LED = 0; LED < 8; LED++)
  {
    static volatile UNSIGNED32 delayvar;

    LPC_GPIO1->FIOPIN = 0;
    LPC_GPIO2->FIOPIN = 0;
    if (LED < 3) LPC_GPIO1->FIOPIN = led_mask[LED];
    else         LPC_GPIO2->FIOPIN = led_mask[LED];

    for (delayvar = 0; delayvar < 300000; delayvar++)
    {
    }
  }
  // Switch LEDS off
  LPC_GPIO1->FIOPIN = 0;
  LPC_GPIO2->FIOPIN = 0;
#endif
  // Reset/Initialize CANopen communication
  MCOUSER_ResetCommunication();

  // foreground loop
  while(1)
  {
    // Operate on CANopen protocol stack
    MCO_ProcessStack();
#if 0
    // Test: echo some data
    gProcImg[P600003_DIGINPUT8_3] = gProcImg[P620003_DIGOUTPUT8_3];
    gProcImg[P600004_DIGINPUT8_4] = gProcImg[P620004_DIGOUTPUT8_4];

    // Update process data
    // Output to LEDs
    MCO_ReadProcessData(buf,4,P641101_ANALOGOUTPUT16_1+1);
    for (LED = 0; LED < 8; LED++) {
      if (buf[0] & (1<<LED))
      {
        if (LED < 3)
          LPC_GPIO1->FIOPIN |= led_mask[LED];
        else
          LPC_GPIO2->FIOPIN |= led_mask[LED];
      }
      else
      {
        if (LED < 3)
          LPC_GPIO1->FIOPIN &= ~led_mask[LED];
        else
          LPC_GPIO2->FIOPIN &= ~led_mask[LED];
      }
    }

    // first analog input is a counter
    pot++;

    buf[0] = pot & 0x000000FF; // lo byte
    buf[1] = (pot >> 8) & 0x000000FF; // hi byte
    // Write analog data to process image
    MCO_WriteProcessData(P640101_ANALOGINPUT16_1,2,buf);
#endif
    // Check for CAN Err, auto-recover
    if (MCOHW_GetStatus() & HW_BOFF)
    {
      MCOUSER_FatalError(0xF6F6);
    }

  } // end of while(1)
} // end of main

#endif
