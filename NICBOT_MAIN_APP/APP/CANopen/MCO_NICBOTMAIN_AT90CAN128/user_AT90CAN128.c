/**************************************************************************
MODULE:    USER - User System Functions
CONTAINS:  MicroCANopen User System Functions
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

#include "mcop_inc.h"

#include "stackinit.h"
#include "can_callbacks.h"


/**************************************************************************
GLOBAL FUNCTIONS
***************************************************************************/ 

/**************************************************************************
DOES:    Call-back function for occurance of a fatal error. 
         Stops operation and displays blnking error pattern on LED
**************************************************************************/
void MCOUSER_FatalError (UNSIGNED16 ErrCode)
{
UNSIGNED16 timeout;

  // Remember last error for stack
  gMCOConfig.last_fatal = ErrCode;

#if USE_LEDS
  LED_ERR_ON;
  gMCOConfig.LEDErr = LED_ON;
#endif

#if USE_EMCY
  MCOP_PushEMCY(0x6100,ErrCode >> 8,ErrCode,0,0,0);
#endif // USE_EMCY

  // Wait 10ms
  timeout = MCOHW_GetTime() + 10;
  while (!MCOHW_IsTimeExpired(timeout))
  { // Wait until timeout
  }

  if (ErrCode >= ERR_FATAL)
  { // Fatal error, should abort/reset
    MCOUSER_ResetApplication();
  }

  // Warning only, simply timeout, then continue
}


/**************************************************************************
DOES:    Call-back function for reset application.
         Starts the watchdog and waits until watchdog causes a reset.
**************************************************************************/
void MCOUSER_ResetApplication (void)
{
   CAN_ResetApplication();
}


/**************************************************************************
DOES:    Call-back function for reset communication.
         Re-initializes the process image and the entire MicroCANopen
         communication.
**************************************************************************/
void MCOUSER_ResetCommunication (void)
{
UNSIGNED16 can_bps;
UNSIGNED8 node_id;

#if USE_LSS_SLAVE

  LSS_LoadConfiguration(&can_bps,&node_id);
  if (node_id == 0) // unconfigured? assume the Node ID from this configuration
  {
    node_id = NODEID;
    can_bps = CAN_BITRATE;
  }
  LSS_Init(node_id);

#else

  node_id = NODEID;
  can_bps = CAN_BITRATE;

#endif

  if (MCO_Init(can_bps,node_id,DEFAULT_HEARTBEAT)) 
  {
    //Initialization of PDOs comes from EDS
    INITPDOS_CALLS

#if USE_STORE_PARAMETERS
    MCOSP_GetStoredParameters();
#endif

  }

}


#if USECB_NMTCHANGE
/**************************************************************************
DOES:    Called when the NMT state of the stack changes
RETURNS: nothing
**************************************************************************/
void MCOUSER_NMTChange
  (
  UNSIGNED8 nmtstate    // new nmt state of stack
  )
{
   CAN_NMTChange(nmtstate);
}
#endif


#if USECB_ODSERIAL
/**************************************************************************
DOES:    This function is called upon read requests to Object Dictionary
         entry [1018h,4] - serial number
RETURNS: The 32bit serial number
**************************************************************************/
UNSIGNED32 MCOUSER_GetSerial (
  void
  )
{
  // replace with code to read serial number, for example from 
  // non-volatile memory
  return CAN_GetSerial();
}
#endif // USECB_ODSERIAL


/**************************************************************************
DOES:    Call-back function, heartbeat lost, timeout occured.
         Gets called when a heartbeat timeout occured for a node.
RETURNS: Nothing
**************************************************************************/
void MCOUSER_HeartbeatLost (
  UNSIGNED8 node_id
  )
{
  // Add code to react on the loss of heartbeat,
  // if node is essential, switch to pre-operational mode
  MCO_HandleNMTRequest(NMTMSG_PREOP);
}


#if USECB_TIMEOFDAY
/**************************************************************************
DOES:    This function is called if the message with the time object has
         been received. This example implementtion calculates the current
         time in hours, minutes, seconds.
RETURNS: nothing
**************************************************************************/
// Global variables holding clock info
UNSIGNED32 hours;
UNSIGNED32 minutes;
UNSIGNED32 seconds;

void MCOUSER_TimeOfDay (
  UNSIGNED32 millis, // Milliseconds since midnight
  UNSIGNED16 days  // Number of days since January 1st, 1984
  )
{
  if (millis < (1000UL * 60 * 60 * 24))
  { // less Milliseconds as one day has
    // calculate hours, minutes & seconds since midnight
    hours = millis / (1000UL * 60 * 60);
    minutes = (millis - (hours * 1000UL * 60 * 60)) / (1000UL * 60);
    seconds = (millis - (hours * 1000UL * 60 * 60) - (minutes * 1000UL * 60)) / 1000;
  }
}
#endif // USECB_TIMEOFDAY


/**************************************************************************
END-OF-FILE 
***************************************************************************/ 

