/**************************************************************************
MODULE:    MCO
CONTAINS:  MicroCANopen implementation
COPYRIGHT: Embedded Systems Academy, Inc. 2002-2015.
           All rights reserved. www.microcanopen.com
DISCLAIM:  Read and understand our disclaimer before using this code!
           www.esacademy.com/disclaim.htm
           This software was written in accordance to the guidelines at
           www.esacademy.com/software/softwarestyleguide.pdf
LICENSE:   THIS IS THE COMMERCIAL PLUS VERSION OF MICROCANOPEN
           ONLY USERS WHO PURCHASED A LICENSE MAY USE THIS SOFTWARE
           See file license_commercial_plus.txt
VERSION:   6.20, ESA 11-MAY-15
           $LastChangedDate: 2015-05-09 19:41:45 -0300 (Sat, 09 May 2015) $
           $LastChangedRevision: 3390 $
***************************************************************************/ 

#include "mcop_inc.h"

#if USE_XOD_ACCESS
#include "xod.h"
#endif

#if USE_CiA447
#include "comgr.h"
#endif


/**************************************************************************
GLOBAL VARIABLES
***************************************************************************/ 

// this structure holds all node specific configuration
MCO_CONFIG MEM_FAR gMCOConfig;

#if (INDEX_FOR_DIAGNOSTICS != 0)
// MCO diagnostics record
MCO_DIAGNOSTICS MEM_FAR gMCODiag;
#endif

#if USE_EMCY
extern EMCY_CONFIG MEM_FAR gEF;
 #ifndef EMCY_INHIBIT_TIME
  // Set default emergency inhibit time
  #define EMCY_INHIBIT_TIME 0
 #endif
#endif

#if ! MGR_MONITOR_ALL_NODES
 #if (NR_OF_HB_CONSUMER > 0)
// from MCOP, HB consumer values
extern HBCONS_CONFIG MEM_FAR gHBCons[NR_OF_HB_CONSUMER];
static UNSIGNED8 MEM_FAR mHBchn = 0; // current channel checked
 #endif // (NR_OF_HB_CONSUMER > 0)
#endif

#if NR_OF_TPDOS > 0
// this structure holds all the TPDO configuration data for the TPDOs
TPDO_CONFIG MEM_FAR gTPDOConfig[NR_OF_TPDOS];
#endif

// this is the next TPDO to be checked in MCO_ProcessStack
UNSIGNED16 MEM_FAR gTPDONr = NR_OF_TPDOS;

#if NR_OF_RPDOS > 0
// this structure holds all the RPDO configuration data for the RPDOs
RPDO_CONFIG MEM_FAR gRPDOConfig[NR_OF_RPDOS];
#endif

// this structure holds the current receive message
static CAN_MSG gRxCAN;

// this structure holds the CAN message for SDO responses or aborts
CAN_MSG gTxSDO;


/**************************************************************************
LOCAL VARIABLES
***************************************************************************/ 


/**************************************************************************
LOCAL FUNCTIONS
***************************************************************************/

#if USE_LEDS
/**************************************************************************
DOES:    This function switches the CANopen Err and Run LEDs
         as specified by DR-303-3
         It must be called every 200ms
GLOBALS: Uses global macros LED_xxx_ON and LED_xxx_OFF.
         Uses module variables mLEDtoggle and mLEDcnt for the blinking.
**************************************************************************/
static void MCO_SwitchLEDs  (
  void
  )
{
  // For blinking or flickering
  gMCOConfig.LEDtoggle = ~gMCOConfig.LEDtoggle;

  switch(gMCOConfig.LEDRun) // Run LED
  {
    case LED_OFF:
      LED_RUN_OFF;
      break;
    case LED_ON:
      LED_RUN_ON;
      break;
    case LED_BLINK:
      if (gMCOConfig.LEDtoggle == 0)
      {
        LED_RUN_ON;
      }
      else
      {
        LED_RUN_OFF;
      }
      break;
    case LED_FLASH1:
      gMCOConfig.LEDcntR++;
      if (gMCOConfig.LEDcntR >= 6)
      {
        gMCOConfig.LEDcntR = 0;
        LED_RUN_ON;
      }
      else
      {
        LED_RUN_OFF;
      }
      break;
    case LED_FLASH2:
      gMCOConfig.LEDcntR++;
      if (gMCOConfig.LEDcntR >= 8)
      {
        gMCOConfig.LEDcntR = 0;
      }
      if ((gMCOConfig.LEDcntR == 0) || (gMCOConfig.LEDcntR == 2))
      {
        LED_RUN_ON;
      }
      else
      {
        LED_RUN_OFF;
      }
      break;
    case LED_FLASH3:
      gMCOConfig.LEDcntR++;
      if (gMCOConfig.LEDcntR >= 10)
      {
        gMCOConfig.LEDcntR = 0;
      }
      if ((gMCOConfig.LEDcntR == 0) || (gMCOConfig.LEDcntR == 2) || (gMCOConfig.LEDcntR == 4))
      {
        LED_RUN_ON;
      }
      else
      {
        LED_RUN_OFF;
      }
      break;
    case LED_FLASH4:
      gMCOConfig.LEDcntR++;
      if (gMCOConfig.LEDcntR >= 12)
      {
        gMCOConfig.LEDcntR = 0;
      }
      if ((gMCOConfig.LEDcntR == 0) || (gMCOConfig.LEDcntR == 2) || 
          (gMCOConfig.LEDcntR == 4) || (gMCOConfig.LEDcntR == 6)
         )
      {
        LED_RUN_ON;
      }
      else
      {
        LED_RUN_OFF;
      }
      break;
    default:
      break;
  }

  switch(gMCOConfig.LEDErr) // Error LED
  {
    case LED_OFF:
      LED_ERR_OFF;
      break;
    case LED_ON:
      LED_ERR_ON;
      break;
    case LED_BLINK: // flicker when called every 50ms
      if (gMCOConfig.LEDtoggle == 0)
      {
        LED_ERR_ON;
      }
      else
      {
        LED_ERR_OFF;
      }
      break;
    case LED_FLASH1:
      gMCOConfig.LEDcntE++;
      if (gMCOConfig.LEDcntE >= 6)
      {
        gMCOConfig.LEDcntE = 0;
        LED_ERR_ON;
      }
      else
      {
        LED_ERR_OFF;
      }
      break;
    case LED_FLASH2:
      gMCOConfig.LEDcntE++;
      if (gMCOConfig.LEDcntE >= 8)
      {
        gMCOConfig.LEDcntE = 0;
      }
      if ((gMCOConfig.LEDcntE == 0) || (gMCOConfig.LEDcntE == 2))
      {
        LED_ERR_ON;
      }
      else
      {
        LED_ERR_OFF;
      }
      break;
    case LED_FLASH3:
      gMCOConfig.LEDcntE++;
      if (gMCOConfig.LEDcntE >= 10)
      {
        gMCOConfig.LEDcntE = 0;
      }
      if ((gMCOConfig.LEDcntE == 0) || (gMCOConfig.LEDcntE == 2) || (gMCOConfig.LEDcntE == 4))
      {
        LED_ERR_ON;
      }
      else
      {
        LED_ERR_OFF;
      }
      break;
    case LED_FLASH4:
      gMCOConfig.LEDcntE++;
      if (gMCOConfig.LEDcntE >= 12)
      {
        gMCOConfig.LEDcntE = 0;
      }
      if ((gMCOConfig.LEDcntE == 0) || (gMCOConfig.LEDcntE == 2) || 
          (gMCOConfig.LEDcntE == 4) || (gMCOConfig.LEDcntE == 6)
         )
      {
        LED_ERR_ON;
      }
      else
      {
        LED_ERR_OFF;
      }
      break;
    default:
      break;
  }
}
#endif // USE_LEDS


/**************************************************************************
DOES:    Handle an incoimg SDO request.
RETURNS: returns 1 if SDO access success, returns 0 if SDO abort generated
**************************************************************************/
static UNSIGNED8 MCO_HandleSDORequest (
   UNSIGNED8 *pData  // pointer to 8 data bytes with SDO data
  )
{
  // command byte of SDO request
  UNSIGNED8 cmd;
  // index of SDO request
  UNSIGNED16 index;
  // subindex of SDO request
  UNSIGNED8 subindex;
  // search result of Search_OD
  UNSIGNED16 found;
  // buffer for read of error data
  UNSIGNED8 buf[4];
  UNSIGNED32 buf32;
  UNSIGNED8 len;
  UNSIGNED16 offset;
  // pointer to an entry in gODProcTable
  OD_PROCESS_DATA_ENTRY MEM_CONST *pOD;
#if USECB_ODSERIAL
  // buffer for serial number
  UNSIGNED32 serial;
#endif 
#if USE_EXTENDED_SDO
  UNSIGNED8 sdoserv;
#endif 
#if USECB_SDO_RD_PI || USECB_SDO_WR_PI
  UNSIGNED32 sdoreturn;
#endif

#if USECB_SDOREQ
    switch (MCOUSER_SDORequest(pData))
    {
    case 0:  // MCOUSER_SDORequest replied with an ABORT
      return 0;
    case 1:  // MCOUSER_SDORequest sent a response
      return 1;
    default: // MCOUSER_SDORequest did not do anything
      break;
    }
#endif // USECB_SDOREQ

#if USE_EXTENDED_SDO
  sdoserv = SDOSERVER(gTxSDO.ID);
  switch(XSDO_HandleExtended(( UNSIGNED8 *)pData,&(gTxSDO),sdoserv))
  {
    case 1:
    // SDO response was sent
    return 1;
  case 2:
    // SDO abort was sent
    return 0;
  default:
    break;
  }
#endif // USE_EXTENDED_SDO

  // init variables
  // upper 3 bits are the command
  cmd = *pData & 0xE0;
  // get high byte of index
  index = pData[2];
  // add low byte of index
  index = (index << 8) + pData[1];
  // subindex
  subindex = pData[3];

  // Copy Multiplexor into response
  // index low
  gTxSDO.BUF[1] = pData[1];
  // index high
  gTxSDO.BUF[2] = pData[2];
  // subindex
  gTxSDO.BUF[3] = pData[3];

#if USE_BLOCKED_SDO
  // Translate block read into regular read
  if ((cmd == 0xA0) && (pData[5] != 0))
  {
    cmd = 0x40;
  }
#endif

  // is it a read or write command?
  if ((cmd == 0x40) || (cmd == 0x20)) 
  {
    // Response for conformance test, data type of 1000h entry
    // this version for cctt V3.01
    if ((index == 0x1000) && (subindex == 0xFF))
    {
      MCO_SendSDOAbort(SDO_ABORT_UNKNOWNSUB);
      return 0;
    }
#if USECB_ODSERIAL
    else if ((index == 0x1018) && (subindex == 4))
    {
      // read command?
      if (cmd == 0x40)
      {
        // Get serial number from call-back function
        serial = MCOUSER_GetSerial();
        return MCO_ReplyWith((UNSIGNED8 *)&(serial),4);
      }
      MCO_SendSDOAbort(SDO_ABORT_READONLY);
      return 0;
    }
#endif // USECB_ODSERIAL

#if NR_OF_SDOSERVER > 1
    if ((index >= 0x1200) && (index <= 0x1200+NR_OF_SDOSERVER))
    {
      return XSDO_HandleSDOServerParam(index,pData);
    }
#endif
    
#if (NR_OF_RPDOS > 0) || (NR_OF_TPDOS > 0)
  #if (NR_OF_RPDOS > 0)
    if ((index >= 0x1400) && (index <= 0x15FF))
    { // RPDO
      return SDO_HandlePDOComParam(1,index,pData);
    }
    #if USE_DYNAMIC_PDO_MAPPING
    if ((index >= 0x1600) && (index <= 0x17FF))
    { // RPDO
      return SDO_HandlePDOMapParam(1,index,pData);
    }
    #endif
  #endif
  #if (NR_OF_TPDOS > 0)
    if ((index >= 0x1800) && (index <= 0x19FF))
    { // TPDO
      return SDO_HandlePDOComParam(0,index,pData);
    }
    #if USE_DYNAMIC_PDO_MAPPING
    if ((index >= 0x1A00) && (index <= 0x1BFF))
    { // TPDO
      return SDO_HandlePDOMapParam(0,index,pData);
    }
    #endif
  #endif
#endif // (NR_OF_RPDOS > 0) || (NR_OF_TPDOS > 0)

    // access to [1017,00] - heartbeat time
    if ((index == 0x1017) && (subindex == 0x00))
    {
      // read command
      if (cmd == 0x40)
      {
        // expedited, 2 bytes of data
        gTxSDO.BUF[0] = 0x4B;
        gTxSDO.BUF[4] = (UNSIGNED8) gMCOConfig.heartbeat_time;
        gTxSDO.BUF[5] = (UNSIGNED8) (gMCOConfig.heartbeat_time >> 8);
        if (!MCOHW_PushMessage(&gTxSDO))
        {
          MCOUSER_FatalError(ERROFL_SDO);
        }
        return 1;
      }
      // expedited write command with 2 bytes of data
      if (*pData == 0x2B)
      {
        if (pData[5] > 0x7F)
        { // maximum supported is 0x7FFF
          MCO_SendSDOAbort(SDO_ABORT_VALUE_HIGH);
          return 0;
        }
#if USE_CiA447
        // Support Profile specific write support, 
        // write is only allowed, if PROFILE_GetSDOFromNode() is <= 1
        if (PROFILE_GetSDOFromNode() > 1) 
        {
          MCO_SendSDOAbort(SDO_ABORT_NOTRANSFERCTRL);
          return 0;
        }
#endif
        gMCOConfig.heartbeat_time = pData[5];
        gMCOConfig.heartbeat_time = (gMCOConfig.heartbeat_time << 8) + pData[4];
        // reset heartbeat time for immediate transmission or current time plus new heartbeat time?
        // Current 3.01 conformance test 9.4 (state 04) requires this to be current time plus new heartbeat time
        // gMCOConfig.heartbeat_timestamp = MCOHW_GetTime();
        gMCOConfig.heartbeat_timestamp = MCOHW_GetTime() + gMCOConfig.heartbeat_time;
        // write response
        gTxSDO.BUF[0] = 0x60;
        // Needed to pass conformance test: clear unused bytes
        gTxSDO.BUF[4] = 0;
        gTxSDO.BUF[5] = 0;
        gTxSDO.BUF[6] = 0;
        gTxSDO.BUF[7] = 0;
        if (!MCOHW_PushMessage(&gTxSDO))
        {
          MCOUSER_FatalError(ERROFL_SDO);
        }
        return 1;
      }
      if (*pData == 0x21)
      {
        // Needed to pass conformance test: Abort code different
        // for segmented write command with 2 bytes of data
        MCO_SendSDOAbort(SDO_ABORT_UNKNOWN_COMMAND);
      }
      else if (*pData == 0x23)
      {
        // expedited write command with 4 bytes of data
        // Needed to pass conformance test: Abort code different for this case
        MCO_SendSDOAbort(SDO_ABORT_TYPEMISMATCH);
      }
      else
      {
        // MCO_SendSDOAbort(SDO_ABORT_UNSUPPORTED);
        // Conformance test does not accept above error code
        MCO_SendSDOAbort(SDO_ABORT_GENERAL);
      }
      return 0;
    }

    // access to [1018h] with extd conform compatibility
    else if ( (index == 0x1018) && (subindex > 0) && (cmd == 0x40) && (pData[4] == 0x41) && (pData[5] == 0x53) && (pData[6] == 0x45) && (pData[7] == 0xFF) )
    {
      buf[0] = 0x41; buf[1] = 0x53; buf[2] = 0x45; buf[3] = 0xFF;
      if (subindex == 0x02)
      {
        buf[0] = 0x4D; buf[1] = 0x43; buf[2] = 0x4F; buf[3] = 0x50;
      }
      else if (subindex == 0x03)
      {
      buf[3] = 0x06; buf[2] = 0x15;
#ifdef REV
      buf[1] = (UNSIGNED8) REV; buf[0] = (UNSIGNED8) (REV >> 8);
#else
      buf[1] = 0; buf[0] = 0;
#endif
      }
      return MCO_ReplyWith(buf,4);
    }

#ifdef REBOOT_FLAG_ADR
    // Supporting the ESAcademy CANopen Bootloader
    if ((index == 0x1F51) && (subindex == 0x01))
    {
      // read command
      if (cmd == 0x40)
      {
        // expedited, 1 byte of data
        gTxSDO.BUF[0] = 0x4F;
        gTxSDO.BUF[4] = 1;   // return "application running"
        gTxSDO.BUF[5] = (UNSIGNED8) 0;
        gTxSDO.BUF[6] = (UNSIGNED8) 0;
        gTxSDO.BUF[7] = (UNSIGNED8) 0;
        if (!MCOHW_PushMessage(&gTxSDO))
        {
          MCOUSER_FatalError(ERROFL_SDO);
        }
        return 1;
      }

      // expedited write command with 1 byte of data
      if (*pData == 0x2F)
      {
        // Write of 0 (stop application)?
        if (pData[4] == 0x00)
        { // Set signal to bootloader
          REBOOT_FLAG = REBOOT_BOOTLOAD_VAL;
          // write response
          gTxSDO.BUF[0] = 0x60;
          // Needed to pass conformance test: clear unused bytes
          gTxSDO.BUF[4] = 0;
          gTxSDO.BUF[5] = 0;
          gTxSDO.BUF[6] = 0;
          gTxSDO.BUF[7] = 0;
          if (!MCOHW_PushMessage(&gTxSDO))
          {
            MCOUSER_FatalError(ERROFL_SDO);
          }
          return 1;
        }
      }
      MCO_SendSDOAbort(SDO_ABORT_UNSUPPORTED);
      return 0;
    }
#endif // REBOOT_FLAG_ADR


#if USE_SYNC
    // dynamic access to [1005]
    // access to [1005,00] - SYNC COB-ID
    if ((index == 0x1005) && (subindex == 0x00))
    {
      // read command
      if (cmd == 0x40)
      {
        // expedited, 4 bytes of data
        gTxSDO.BUF[0] = 0x43;
        gTxSDO.BUF[4] = (UNSIGNED8) gMCOConfig.SYNCid & 0x00FF;
        gTxSDO.BUF[5] = (UNSIGNED8) (gMCOConfig.SYNCid>>8) & 0x00FF;
        gTxSDO.BUF[6] = (UNSIGNED8) 0;
        gTxSDO.BUF[7] = (UNSIGNED8) 0;
        if (!MCOHW_PushMessage(&gTxSDO))
        {
          MCOUSER_FatalError(ERROFL_SDO);
        }
        return 1;
      }
      // write access
      if (*pData == (1<<5) + 3)
      { // expedited write command with 4 bytes of data
        // Retrieve new COB-ID
        // conformance test: verify range
        if (((pData[6] > 0) || (pData[7] > 0) || IS_CANID_RESTRICTED((((UNSIGNED16)pData[5]) << 8) + pData[4])))
        { // illegal value
          MCO_SendSDOAbort(SDO_ABORT_VALUE_HIGH);
          return 0;
        }
        gMCOConfig.SYNCid = pData[5];        
        gMCOConfig.SYNCid <<= 8;
        gMCOConfig.SYNCid += pData[4];        
        // Set new CAN receive filter
        MCOHW_SetCANFilter(gMCOConfig.SYNCid);
        // write response
        gTxSDO.BUF[0] = 0x60;
        // Needed to pass conformance test: clear unused bytes
        gTxSDO.BUF[4] = 0;
        gTxSDO.BUF[5] = 0;
        gTxSDO.BUF[6] = 0;
        gTxSDO.BUF[7] = 0;
        if (!MCOHW_PushMessage(&gTxSDO))
        {
          MCOUSER_FatalError(ERROFL_SDO);
        }
        return 1;
      }
      // if we reach here, wrong write length
      // MCO_SendSDOAbort(SDO_ABORT_UNSUPPORTED);
      // error above not accepted by conformance test
      MCO_SendSDOAbort(SDO_ABORT_GENERAL);
      return 0;
    }
#endif // USE_SYNC


#if USE_EMCY
    // dynamic access to [1014]
    // access to [1014,00] - EMCY COB-ID
    if ((index == 0x1014) && (subindex == 0x00))
    {
      // read command
      if (cmd == 0x40)
      {
        // expedited, 4 bytes of data
        gTxSDO.BUF[0] = 0x43;
        gTxSDO.BUF[4] = (UNSIGNED8) 0x80 + MY_NODE_ID;
        gTxSDO.BUF[5] = (UNSIGNED8) 0;
        gTxSDO.BUF[6] = (UNSIGNED8) 0;
        gTxSDO.BUF[7] = (UNSIGNED8) 0;
        if (!MCOHW_PushMessage(&gTxSDO))
        {
          MCOUSER_FatalError(ERROFL_SDO);
        }
        return 1;
      }
      // fail on write access
      MCO_SendSDOAbort(SDO_ABORT_UNSUPPORTED);
      return 0;
    }
    // access to [1015,00] - EMCY Inhibit Time
    if ((index == 0x1015) && (subindex == 0x00))
    {
      // read command
      if (cmd == 0x40)
      {
        // expedited, 2 bytes of data
        buf32 = gEF.emcy_inhibit * 10; // internally stored in ms, reported in 100us
        gTxSDO.BUF[0] = 0x4B;
        gTxSDO.BUF[4] = (UNSIGNED8) buf32;
        gTxSDO.BUF[5] = (UNSIGNED8) (buf32 >> 8);
        if (!MCOHW_PushMessage(&gTxSDO))
        {
          MCOUSER_FatalError(ERROFL_SDO);
        }
        return 1;
      }
      // expedited write command with 2 bytes of data
      if (*pData == 0x2B)
      {
        buf32 = pData[5];
        buf32 = (buf32 << 8) + pData[4];
        gEF.emcy_inhibit = (buf32 + 9) / 10; // passed in 100us, saved in ms
        // reset timestamp for immediate transmission
        gEF.emcy_timestamp = MCOHW_GetTime();
        // write response
        gTxSDO.BUF[0] = 0x60;
        // Needed to pass conformance test: clear unused bytes
        gTxSDO.BUF[4] = 0;
        gTxSDO.BUF[5] = 0;
        gTxSDO.BUF[6] = 0;
        gTxSDO.BUF[7] = 0;
        if (!MCOHW_PushMessage(&gTxSDO))
        {
          MCOUSER_FatalError(ERROFL_SDO);
        }
        return 1;
      }
      // expedited write command with 4 bytes of data
      // Needed to pass conformance test: Abort code different for this case
      else if (*pData == 0x23)
      {
        MCO_SendSDOAbort(SDO_ABORT_TYPEMISMATCH);
      }
      MCO_SendSDOAbort(SDO_ABORT_UNSUPPORTED);
      return 0;
    }
#if ERROR_FIELD_SIZE > 0
    // access to [1003,xx] - Error Field (History)
    if (index == 0x1003)
    {
      // read command
      if (cmd == 0x40)
      {
        if (subindex > ERROR_FIELD_SIZE)
        {
          MCO_SendSDOAbort(SDO_ABORT_UNKNOWNSUB);
          return 0;
        }
        buf32 = MCOP_ErrField_Get(subindex);
        if (buf32 != 0xFFFFFFFF)
        {
          if (subindex == 0)
          { // expedited, 1 byte of data
            gTxSDO.BUF[0] = 0x4F;
            gTxSDO.BUF[4] = (UNSIGNED8)buf32;
            gTxSDO.BUF[5] = 0;
            gTxSDO.BUF[6] = 0;
            gTxSDO.BUF[7] = 0;
          }
          else
          { // expedited, 4 bytes of data
            gTxSDO.BUF[0] = 0x43;
            gTxSDO.BUF[4] = (UNSIGNED8) buf32;
            gTxSDO.BUF[5] = (UNSIGNED8)(buf32 >> 8);
            gTxSDO.BUF[6] = (UNSIGNED8)(buf32 >> 16);
            gTxSDO.BUF[7] = (UNSIGNED8)(buf32 >> 24);
          }
          if (!MCOHW_PushMessage(&gTxSDO))
          {
            MCOUSER_FatalError(ERROFL_SDO);
          }
          return 1;
        }
        else
        {
          MCO_SendSDOAbort(SDO_ABORT_UNSUPPORTED);
          return 0;
        }
      }
      else if (pData[0] == 0x2F)
      { // write command
        if (subindex == 0)
        { // flush error history
          if (pData[4] != 0)
          { // only write of 0 allowed
            MCO_SendSDOAbort(SDO_ABORT_VALUE_HIGH);
            return 0;
          }
          MCOP_ErrField_Flush();
          // write response
          gTxSDO.BUF[0] = 0x60;
          // Needed to pass conformance test: clear unused bytes
          gTxSDO.BUF[4] = 0;
          gTxSDO.BUF[5] = 0;
          gTxSDO.BUF[6] = 0;
          gTxSDO.BUF[7] = 0;
          if (!MCOHW_PushMessage(&gTxSDO))
          {
            MCOUSER_FatalError(ERROFL_SDO);
          }
          return 1;
        }
        MCO_SendSDOAbort(SDO_ABORT_UNSUPPORTED);
        return 0;
      }
      // if we reach here, fail
      // for conformance test produce generic error
      MCO_SendSDOAbort(SDO_ABORT_GENERAL);
      // MCO_SendSDOAbort(SDO_ABORT_UNSUPPORTED);
      return 0;
    }
#endif // ERROR_FIELD_SIZE > 0
#endif // USE_EMCY

#if (NR_OF_HB_CONSUMER > 0)
#if DYNAMIC_HB_CONSUMER
    // dynamic read/write accesses
    // access to [1016,xx] - heartbeat consumer time
    if (index == 0x1016)
    {
      if (cmd == 0x40)
      { // read command
        if (subindex == 0)
        { // expedited response, 1 byte data
          gTxSDO.BUF[0] = 0x4F;
          gTxSDO.BUF[4] = NR_OF_HB_CONSUMER;
          gTxSDO.BUF[5] = 0;
          gTxSDO.BUF[6] = 0;
          gTxSDO.BUF[7] = 0;
          if (!MCOHW_PushMessage(&gTxSDO))
          {
            MCOUSER_FatalError(ERROFL_SDO);
          }
          return 1;
        }
        else if (subindex > NR_OF_HB_CONSUMER)
        {
          MCO_SendSDOAbort(SDO_ABORT_UNKNOWNSUB);
          return 0;
        }
        else
        { // regular access
          subindex--; // now can directly be used as array pointer
          // expedited, 4 bytes of data
          gTxSDO.BUF[0] = 0x43;
#if ! MGR_MONITOR_ALL_NODES
          gTxSDO.BUF[4] = (UNSIGNED8) gHBCons[subindex].time;
          gTxSDO.BUF[5] = (UNSIGNED8) (gHBCons[subindex].time >> 8);
          gTxSDO.BUF[6] = (UNSIGNED8) gHBCons[subindex].can_id;
          gTxSDO.BUF[7] = (UNSIGNED8) 0;
#else
          if (MY_NODE_ID != subindex)
          {
            gTxSDO.BUF[4] = (UNSIGNED8) gNodeList[subindex].hb_time;
            gTxSDO.BUF[5] = (UNSIGNED8) (gNodeList[subindex].hb_time >> 8);
            gTxSDO.BUF[6] = (UNSIGNED8) subindex;
          }
          else
          {
            gTxSDO.BUF[4] = 0;
            gTxSDO.BUF[5] = 0;
            gTxSDO.BUF[6] = 0;
          }
          gTxSDO.BUF[7] = 0;
#endif // MGR_MONITOR_ALL_NODES
          if (!MCOHW_PushMessage(&gTxSDO))
          {
            MCOUSER_FatalError(ERROFL_SDO);
          }
          return 1;
        }
      }
      // expedited write command with 4 bytes of data
#if ! MGR_MONITOR_ALL_NODES
      if ( (*pData == 0x23) && 
           (pData[6] != MY_NODE_ID) && // not our own node ID
           (pData[6] < 128) && // not an illegal node ID
           // removed to work with PCOMPDS V1.23
           // re-added to work with Conformance Test
           (!MCOP_IsHBMonitored(subindex,pData[6]))
         )
      { // only if requested node ID is unequal to own node id and not yet used
        MCOP_InitHBConsumer(subindex,pData[6],((UNSIGNED16)(pData[5])<< 8) + pData[4]);
        // write response
        gTxSDO.BUF[0] = 0x60;
        // Needed to pass conformance test: clear unused bytes
        gTxSDO.BUF[4] = 0;
        gTxSDO.BUF[5] = 0;
        gTxSDO.BUF[6] = 0;
        gTxSDO.BUF[7] = 0;
        if (!MCOHW_PushMessage(&gTxSDO))
        {
          MCOUSER_FatalError(ERROFL_SDO);
        }
        return 1;
      }
#else
       if (*pData == 0x23) 
       { // write access
         MCO_SendSDOAbort(SDO_ABORT_READONLY);
       }
#endif // MGR_MONITOR_ALL_NODES
      // if we reach here probably conformance test tries something...
      if (subindex == 0)
      {
        MCO_SendSDOAbort(SDO_ABORT_READONLY);
      }
      else
      {
        // MCO_SendSDOAbort(SDO_ABORT_PARAMETER);
        // Abort code required by Conformance Test SDO 09
        MCO_SendSDOAbort(SDO_ABORT_TYPEMISMATCH);
      }
      return 0;
    }
#endif // DYNAMIC_HB_CONSUMER
#endif // (NR_OF_HB_CONSUMER > 0)

#if USE_STORE_PARAMETERS
    // access to [1010,xx] or [1011,xx] - Store Parameters, Restore
    if ( ((index == 0x1010) || (index == 0x1011)) && (subindex <= NROF_STORE_PARAMETERS) )
    {
      // read command
      if (cmd == 0x40)
      {
        if (subindex == 0)
        {
          // expedited, 1 byte of data
          gTxSDO.BUF[0] = 0x4F;
          gTxSDO.BUF[4] = NROF_STORE_PARAMETERS;
        }
        else
        { // access supported
          // expedited, 4 bytes of data
          gTxSDO.BUF[0] = 0x43;
          gTxSDO.BUF[4] = 1;
        }
        gTxSDO.BUF[5] = 0;
        gTxSDO.BUF[6] = 0;
        gTxSDO.BUF[7] = 0;
        if (!MCOHW_PushMessage(&gTxSDO))
        {
          MCOUSER_FatalError(ERROFL_SDO);
        }
        return 1;
      }
      // expedited write command with 4 bytes of data
      if (*pData == 0x23)
      {
        if(
// ===== BEGIN COMPATIBILITY SECTION ==================
// This section allows to use both "save" resp. "evas"
// as well as "load" resp. "daol". For strict CiA301
// behaviour, disable this section. There are different
// interpretations and implementations in common
// CANopen tools, so allowing both options is the safe
// choice.
           ( (index == 0x1010) &&
             (pData[4] == 'e') && (pData[5] == 'v') &&
             (pData[6] == 'a') && (pData[7] == 's')
           )
           ||
           ( (index == 0x1011) &&
             (pData[4] == 'd') && (pData[5] == 'a') &&
             (pData[6] == 'o') && (pData[7] == 'l')
           )
           ||
// ===== END COMPATIBILITY SECTION ====================
           ( (index == 0x1010) &&
             (pData[4] == 's') && (pData[5] == 'a') &&
             (pData[6] == 'v') && (pData[7] == 'e')
           )
           ||
           ( (index == 0x1011) &&
             (pData[4] == 'l') && (pData[5] == 'o') &&
             (pData[6] == 'a') && (pData[7] == 'd')
           )
          )
        { // write of "save" to 0x1010 or "load" to 0x1011
          if (MCOSP_StoreParameters(index,subindex))
          { // Parameters stored OK.
            // write response
            gTxSDO.BUF[0] = 0x60;
            // Needed to pass conformance test: clear unused bytes
            gTxSDO.BUF[4] = 0;
            gTxSDO.BUF[5] = 0;
            gTxSDO.BUF[6] = 0;
            gTxSDO.BUF[7] = 0;
            if (!MCOHW_PushMessage(&gTxSDO))
            {
              MCOUSER_FatalError(ERROFL_SDO);
            }
            return 1;
          }
          else
          {
            MCO_SendSDOAbort(SDO_ABORT_TRANSFER);
            return 0;
          }
        }
        else
        { // wrong code
          MCO_SendSDOAbort(SDO_ABORT_TRANSFER);
          return 0;
        }
      }
      else
      { // wrong size or access
        MCO_SendSDOAbort(SDO_ABORT_TYPEMISMATCH);
        return 0;
      }
    }
#endif // USE_STORE_PARAMETERS

#if (INDEX_FOR_DIAGNOSTICS != 0)
    if (index == INDEX_FOR_DIAGNOSTICS)
    {
      // read command
      if (cmd == 0x40)
      { // default response bytes
        gTxSDO.BUF[4] = 0;
        gTxSDO.BUF[5] = 0;
        gTxSDO.BUF[6] = 0;
        gTxSDO.BUF[7] = 0;
        if (subindex == 0)
        { // return 22
          gTxSDO.BUF[0] = 0x4F;
          gTxSDO.BUF[4] = 22;
        }
        else if (subindex <= 7)
        { // expedited, 4 bytes
          gTxSDO.BUF[0] = 0x43;
          switch(subindex)
          {
            case 1: // Identify
              gTxSDO.BUF[4] = 0x00;
              gTxSDO.BUF[5] = 0x45;
              gTxSDO.BUF[6] = 0x53;
              gTxSDO.BUF[7] = 0x41;
              break;
            case 2: // Version
             #ifdef REV
              gTxSDO.BUF[4] = (UNSIGNED8) REV;
              gTxSDO.BUF[5] = (UNSIGNED8) (REV >> 8);
             #else
              gTxSDO.BUF[4] = 0;
              gTxSDO.BUF[5] = 0;
             #endif
              gTxSDO.BUF[6] = 0x15;
              gTxSDO.BUF[7] = 6;
              break;
            case 3: // Functionality
              #if USE_EVENT_TIME == 1
                gTxSDO.BUF[4] |= 0x10;
              #endif
              #if USE_INHIBIT_TIME == 1
                gTxSDO.BUF[4] |= 0x20;
              #endif
              #if USE_SYNC == 1
                gTxSDO.BUF[4] |= 0x40;
              #endif
              #if USE_EXTENDED_SDO == 1
                gTxSDO.BUF[5] |= 0x01;
              #endif
              #if USE_BLOCKED_SDO == 1
                gTxSDO.BUF[5] |= 0x02;
              #endif
              break;
            case 4: // Status
              break;
#if (TXFIFOSIZE > 0)
            case 5: // TxFIFO
              break;
#endif
#if (RXFIFOSIZE > 0)
            case 6: // RxFIFO
              break;
#endif
#if (MGRFIFOSIZE > 0)
            case 7: // MgrRxFIFO
              break;
#endif
          }
        }
        else if (subindex <= 22)
        { // expedited, 2 bytes
          gTxSDO.BUF[0] = 0x4B;
          switch(subindex)
          {
            case 8: // ProcTickPerSecCur
              gTxSDO.BUF[4] = (UNSIGNED8) gMCODiag.ProcTickPerSecCur;
              gTxSDO.BUF[5] = (UNSIGNED8) (gMCODiag.ProcTickPerSecCur >> 8);
              break;
            case 9: // ProcTickPerSecMin
              gTxSDO.BUF[4] = (UNSIGNED8) gMCODiag.ProcTickPerSecMin;
              gTxSDO.BUF[5] = (UNSIGNED8) (gMCODiag.ProcTickPerSecMin >> 8);
              break;
            case 10: // ProcTickPerSecMax
              gTxSDO.BUF[4] = (UNSIGNED8) gMCODiag.ProcTickPerSecMax;
              gTxSDO.BUF[5] = (UNSIGNED8) (gMCODiag.ProcTickPerSecMax >> 8);
              break;
            case 11: // ProcTickBurstMax
              gTxSDO.BUF[4] = (UNSIGNED8) gMCODiag.ProcTickBurstMax;
              gTxSDO.BUF[5] = (UNSIGNED8) (gMCODiag.ProcTickBurstMax >> 8);
              break;
            case 12: // ProcRxPerSecCur
              gTxSDO.BUF[4] = (UNSIGNED8) gMCODiag.ProcRxPerSecCur;
              gTxSDO.BUF[5] = (UNSIGNED8) (gMCODiag.ProcRxPerSecCur >> 8);
              break;
            case 13: // ProcRxPerSecMin
              gTxSDO.BUF[4] = (UNSIGNED8) gMCODiag.ProcRxPerSecMin;
              gTxSDO.BUF[5] = (UNSIGNED8) (gMCODiag.ProcRxPerSecMin >> 8);
              break;
            case 14: // ProcRxPerSecMax
              gTxSDO.BUF[4] = (UNSIGNED8) gMCODiag.ProcRxPerSecMax;
              gTxSDO.BUF[5] = (UNSIGNED8) (gMCODiag.ProcRxPerSecMax >> 8);
              break;
#if MGR_MONITOR_ALL_NODES
            case 16: // ProcMgrTickPerSecCur
              gTxSDO.BUF[4] = (UNSIGNED8) gMCODiag.ProcMgrTickPerSecCur;
              gTxSDO.BUF[5] = (UNSIGNED8) (gMCODiag.ProcMgrTickPerSecCur >> 8);
              break;
            case 17: // ProcMgrTickPerSecMin
              gTxSDO.BUF[4] = (UNSIGNED8) gMCODiag.ProcMgrTickPerSecMin;
              gTxSDO.BUF[5] = (UNSIGNED8) (gMCODiag.ProcMgrTickPerSecMin >> 8);
              break;
            case 18: // ProcMgrTickPerSecMax
              gTxSDO.BUF[4] = (UNSIGNED8) gMCODiag.ProcMgrTickPerSecMax;
              gTxSDO.BUF[5] = (UNSIGNED8) (gMCODiag.ProcMgrTickPerSecMax >> 8);
              break;
            case 19: // ProcMgrTickBurstMax
              gTxSDO.BUF[4] = (UNSIGNED8) gMCODiag.ProcMgrTickBurstMax;
              gTxSDO.BUF[5] = (UNSIGNED8) (gMCODiag.ProcMgrTickBurstMax >> 8);
              break;
            case 20: // ProcMgrRxPerSecCur
              gTxSDO.BUF[4] = (UNSIGNED8) gMCODiag.ProcMgrRxPerSecCur;
              gTxSDO.BUF[5] = (UNSIGNED8) (gMCODiag.ProcMgrRxPerSecCur >> 8);
              break;
            case 21: // ProcMgrRxPerSecMin
              gTxSDO.BUF[4] = (UNSIGNED8) gMCODiag.ProcMgrRxPerSecMin;
              gTxSDO.BUF[5] = (UNSIGNED8) (gMCODiag.ProcMgrRxPerSecMin >> 8);
              break;
            case 22: // ProcMgrRxPerSecMax
              gTxSDO.BUF[4] = (UNSIGNED8) gMCODiag.ProcMgrRxPerSecMax;
              gTxSDO.BUF[5] = (UNSIGNED8) (gMCODiag.ProcMgrRxPerSecMax >> 8);
              break;
#endif
            default: // not used, return zero
              MCO_SendSDOAbort(SDO_ABORT_UNKNOWNSUB);
              return 0;
          }
        }
        else
        { // unknown subindex
          MCO_SendSDOAbort(SDO_ABORT_UNKNOWNSUB);
          return 0;
        }
        // gTxSDO all set with current data
        if (!MCOHW_PushMessage(&gTxSDO))
        {
          MCOUSER_FatalError(ERROFL_SDO);
        }
        return 1;
      }
      else
      { // write cmd or unsupported command
        MCO_SendSDOAbort(SDO_ABORT_GENERAL);
        return 0;
      }
    }
#endif // INDEX_FOR_DIAGNOSTICS

    // deal with access to process image area
    found = MCO_SearchODProcTable(index,subindex);
    // entry found?
    if (found != 0xFFFF)
    {
      pOD = OD_ProcTablePtr(found);
      offset = pOD->off_hi;
      offset <<= 8;
      offset +=  pOD->off_lo;
      // read command?
      if (cmd == 0x40)
      {
        // read allowed?
        if ((pOD->len & ODRD) != 0) // Check if RD bit is set
        {
#if USECB_SDO_RD_PI
          // Application call back, SDO read access to process image
          sdoreturn = MCOUSER_SDORdPI(index,subindex,offset,pOD->len & 0x0F);
          if (sdoreturn != 0)
          { // access not granted
            MCO_SendSDOAbort(sdoreturn);
            return 0;
          }
#endif // USECB_SDO_RD_PI

          PI_READ(PIACC_SDO,offset,( UNSIGNED8 *)&buf32,pOD->len & 0x0F);

#if USECB_SDO_RD_AFTER
          MCOUSER_SDORdAft(index,subindex,offset,pOD->len & 0x0F);
#endif // USECB_SDO_RD_AFTER

          // ensure little endian format for MCO_ReplyWith
          buf[0] = (UNSIGNED8) buf32;
          buf[1] = (UNSIGNED8) (buf32 >> 8);
          buf[2] = (UNSIGNED8) (buf32 >> 16);
          buf[3] = (UNSIGNED8) (buf32 >> 24);
          
          return MCO_ReplyWith(buf,(pOD->len & 0x0F));
        }
        // read not allowed
        else
        {
          MCO_SendSDOAbort(SDO_ABORT_UNSUPPORTED);
          return 0;
        }
      }
      // write command?
      else
      {
        // is WR bit set? - then write allowed
        if ((pOD->len & ODWR) != 0)
        {
          // for writes: Bits 2 and 3 of *pData are number of bytes without data
          len = 4 - ((*pData & 0x0C) >> 2); 
          // is length ok?
          if (len != (pOD->len & 0x0F))
          {
            MCO_SendSDOAbort(SDO_ABORT_TYPEMISMATCH);
            return 0;
          }

#if USECB_SDO_WR_PI
          // Application call back, SDO write access to process image
          sdoreturn = MCOUSER_SDOWrPI(index,subindex,offset,&(gRxCAN.BUF[4]),len);
          if (sdoreturn != 0)
          { // access not granted
            MCO_SendSDOAbort(sdoreturn);
            return 0;
          }
#endif // USECB_SDO_WR_PI

          // Write Data
          PI_WRITE(PIACC_SDO, offset, &(gRxCAN.BUF[4]), len);

#if USE_CiA447
          MCOUSER_NodeSpecificSDOWrite(PROFILE_GetSDOFromNode(),index,subindex,offset,len);
#endif

#if USECB_SDO_WR_AFTER
          MCOUSER_SDOWrAft(index,subindex,offset,len);
#endif // USECB_SDO_WR_AFTER

#if USECB_ODDATARECEIVED
          RTOS_LOCK_PI(PIACC_APP,PISECT_ALL);
          MCOUSER_ODData(index,subindex,&(gProcImg[offset]),len);
          RTOS_UNLOCK_PI(PIACC_APP,PISECT_ALL);
#endif // USECB_ODDATARECEIVED

          // write response
          gTxSDO.BUF[0] = 0x60;
          // Needed to pass conformance test: clear unused bytes 
          gTxSDO.BUF[4] = 0; 
          gTxSDO.BUF[5] = 0; 
          gTxSDO.BUF[6] = 0; 
          gTxSDO.BUF[7] = 0;           
          if (!MCOHW_PushMessage(&gTxSDO))
          {
            MCOUSER_FatalError(ERROFL_SDO);
          }
          return 1;
        }
        // write not allowed
        else
        {
          MCO_SendSDOAbort(SDO_ABORT_UNSUPPORTED);
          return 0;
        }
      }
    }

    // search table with constants
    found = MCO_SearchOD(index,subindex);
    // entry found?
    if (found < 0xFFFF)
    {
      // read command?
      if (cmd == 0x40)
      {
        MEM_CPY_FAR(&(gTxSDO.BUF[0]),OD_SDOResponseTablePtr(found<<3),8);
        if (!MCOHW_PushMessage(&gTxSDO))
        {
          MCOUSER_FatalError(ERROFL_SDO);
        }
        return 1;
      }
      // write command
      MCO_SendSDOAbort(SDO_ABORT_READONLY);
      return 0;
    }
    // Error and status byte
    if ((index == 0x1001) && (subindex == 0x00))
    {
      // read command
      if (cmd == 0x40)
      {
        // expedited, 1 byte of data
        gTxSDO.BUF[0] = 0x4F;
        gTxSDO.BUF[4] = gMCOConfig.error_register;
        gTxSDO.BUF[5] = 0;
        gTxSDO.BUF[6] = 0;
        gTxSDO.BUF[7] = 0;
        if (!MCOHW_PushMessage(&gTxSDO))
        {
          MCOUSER_FatalError(ERROFL_SDO);
        }
        return 1;
      }
      // write command
      MCO_SendSDOAbort(SDO_ABORT_READONLY);
      return 0;
    }

    // Requested OD entry not found
    // can not use more specific error code here, as we don't know if index or subindex failed
    MCO_SendSDOAbort(SDO_ABORT_GENERAL);
    return 0;
  }
  // ignore abort received - all other produce an error
  if (cmd != 0x80)
  {
    MCO_SendSDOAbort(SDO_ABORT_UNKNOWN_COMMAND);
    return 0;
  }
#if USE_EXTENDED_SDO
  else
  { // Inform extended handling about the abort
    XSDO_Abort(sdoserv);
  }
#endif // USE_EXTENDED_SDO
  return 1;
}


#if NR_OF_RPDOS > 0
/**************************************************************************
DOES:    Called when going into the operational mode.
         Inits all RPDO Filters
RETURNS: nothing
**************************************************************************/
static void MCO_PrepareRPDOs (
    void
  )
{
UNSIGNED8 i;

  i = 0;
  // prepare all RPDO filters for reception
  while (i < gMCOConfig.nrRPDOs)
  {
    if ((gMCOConfig.error_code & 0x80) == 0)
    { // RPDO filters not yet set
      if ((gRPDOConfig[i].CANID & COBID_DISABLED) == 0)
      {
        if (!MCOHW_SetCANFilter(gRPDOConfig[i].CANID))
        {
          MCOUSER_FatalError(ERRFT_RXFLTP);
        }
      }
    }
    i++;
  }
  gMCOConfig.error_code |= 0x80; // Signal that RPDO filters are now set
}
#endif // NR_OF_RPDOS > 0


#if NR_OF_TPDOS > 0
/**************************************************************************
DOES:    Called when going into the operational mode.
         Prepares all TPDOs for operational.
RETURNS: nothing
**************************************************************************/
static void MCO_PrepareTPDOs (
    void
  )
{
UNSIGNED8 i;

  i = 0;
  // prepare all TPDOs for transmission
  while (i < gMCOConfig.nrTPDOs)
  {
    // Copy current process data
    PDO_TXCOPY(i,( UNSIGNED8 *)&(gTPDOConfig[i].CAN.BUF[0]));
#if USE_EVENT_TIME
    // Reset event timer for immediate transmission
    gTPDOConfig[i].event_timestamp = MCOHW_GetTime() - 2;
#endif
#if USE_INHIBIT_TIME
    gTPDOConfig[i].inhibit_status = INHITIM_RUNNING_TRIGGERED; // Mark as ready for transmission
    // Reset inhibit timer for immediate transmission
    gTPDOConfig[i].inhibit_timestamp = MCOHW_GetTime() - 2;
#endif
#if USE_SYNC
    gTPDOConfig[i].SYNCcnt = gTPDOConfig[i].TType;
    if (gTPDOConfig[i].TType == 0)
    { // set to 241 for very first call
      gTPDOConfig[i].TType = 241;
    }
#endif
    i++;
  }
  // ensure that MCO_ProcessStack starts with TPDO1
  gTPDONr = NR_OF_TPDOS;
}
#endif // NR_OF_TPDOS > 0




#if NR_OF_RPDOS > 0
/**************************************************************************
DOES:    Handles Receive PDOs
RETURNS: FALSE, if RPDO not processed
         TRUE, if RPDO processed
**************************************************************************/
static UNSIGNED8 MCO_HandleRPDO (
  CAN_MSG *pRPDO
  )
{
UNSIGNED8 retval = FALSE;
UNSIGNED8 i; // loop variable
#if USECB_ODDATARECEIVED
UNSIGNED16 map; // offset into SDOResponseTable, RPDO mapping 
UNSIGNED16 off; // offset into Process Image to RPDO data
UNSIGNED8 MEM_CONST *pSDO; // pointer into SDO response table
UNSIGNED8 cnt;
#endif // USECB_ODDATARECEIVED


  if (MY_NMT_STATE == NMTSTATE_OP)
  { // node is operational

#if USE_PROFILE_RPDO
    pRPDO->ID = PROFILE_ExtHandleRPDO(pRPDO->ID);
#endif

    i = 0;
    // loop through RPDOs
    while (i < gMCOConfig.nrRPDOs)
    {
      // is this one of our RPDOs and is not disabled
      if ((pRPDO->ID == gRPDOConfig[i].CANID) &&
        ((gRPDOConfig[i].CANID & COBID_DISABLED) == 0)
         )
      {
#if USE_EMCY
// Only supported with MicroCANopen Plus
        // if (gRxCAN.LEN != gRPDOConfig[i].len)
        // For backwards compatibility, allow PDOs that are too long
        // Only produce emergency for PDOs that are too short
        if (gRxCAN.LEN < gRPDOConfig[i].len)
        { // Length of CAN message does not match PDO len
          gMCOConfig.error_register |= 1; // set generic error bit
          // send EMCY message PDO not processed
          if (!MCOP_PushEMCY(0x8210,(UNSIGNED8)gRPDOConfig[i].PDONr,(UNSIGNED8)(gRPDOConfig[i].PDONr >> 8),gRPDOConfig[i].len,gRxCAN.LEN,0))
          {
            MCOUSER_FatalError(ERROFL_EMCY);
          }
        }
#endif // USE_EMCY
        else if (gRPDOConfig[i].TType >= 254)
        { // This PDO is not synced
#if USECB_ODDATARECEIVED
          // Process RPDO mapping
 #if USE_DYNAMIC_PDO_MAPPING
 #error USECB_ODDATARECEIVED currently not available with USE_DYNAMIC_PDO_MAPPING
 #else
          // copy data from RPDO to process image
          PDO_RXCOPY(i,&(pRPDO->BUF[0]));

          map = gRPDOConfig[i].map; // offset to mapping entries
          off = 0; // start with offset zero
          pSDO = OD_SDOResponseTablePtr(0);
          cnt = 1;
          while((pSDO[map+3] == cnt) && (cnt <= 8))
          { // while Subindex is not zero
            RTOS_LOCK_PI(PIACC_APP,PISECT_PDO);
            MCOUSER_ODData((((UNSIGNED16)(pSDO[map+7]))<<8)+pSDO[map+6],pSDO[map+5],&(pRPDO->BUF[0+off]),pSDO[map+4]>>3);
            RTOS_UNLOCK_PI(PIACC_APP,PISECT_PDO);
            off += (pSDO[map+4]>>3); // next mapped OD entry
            map += 8; // next mapping entry
            cnt++;
          }
#endif // USE_DYNAMIC_PDO_MAPPING

#else // USECB_ODDATARECEIVED
          // copy data from RPDO to process image
          PDO_RXCOPY(i,&(pRPDO->BUF[0]));

#endif // USECB_ODDATARECEIVED

#if USECB_RPDORECEIVE
          MCOUSER_RPDOReceived(gRPDOConfig[i].PDONr,gRPDOConfig[i].offset,gRPDOConfig[i].len);
#endif // USECB_RPDORECEIVE

        }
#if USE_SYNC
        else if (gRPDOConfig[i].TType <= 240)
        { // This PDO is synced
          // copy data from CAN message to RPDO buffer
          MEM_CPY_FAR(&(gRPDOConfig[i].BUF[0]),&(pRPDO->BUF[0]),gRPDOConfig[i].len);
        }
#endif // USE_SYNC
        // exit the loop
        retval = TRUE;
        break;
      }  
      i++;
    } // for all RPDOs
  }
  return retval; // not a RPDO for us
}
#endif // NR_OF_RPDOS > 0


#if NR_OF_TPDOS > 0
/**************************************************************************
DOES:    Handles Transmit PDOs, checks only one TPDO with each call
RETURNS: FALSE, if no TPDO was sent
         TRUE, if TPDO was sent
**************************************************************************/
static UNSIGNED8 MCO_HandleTPDO (
  UNSIGNED16 TPDONr // Number of TPDO to check, as gTPDOConfig[] array index
  )
{
UNSIGNED8 retval = FALSE;

  // is the TPDO 'TPDONr' in use?
  if ((gTPDOConfig[TPDONr].CAN.ID != 0) && 
      ((gTPDOConfig[TPDONr].CAN.ID & COBID_DISABLED) == 0)
#if USE_SYNC
      && (gTPDOConfig[TPDONr].TType >= 254) // Not a synced PDO
#endif
     )
  {
#if USE_EVENT_TIME
    // does TPDO use event timer and event timer is expired? if so we need to transmit now
    if ((gTPDOConfig[TPDONr].event_time != 0) && 
        (MCOHW_IsTimeExpired(gTPDOConfig[TPDONr].event_timestamp)) )
    {
#if USECB_TPDORDY
      if (MCOUSER_TPDOReady(gTPDOConfig[TPDONr].PDONr,0))
      {
#endif
        // get data from process image and transmit
        PDO_TXCOPY(TPDONr,( UNSIGNED8 *)&(gTPDOConfig[TPDONr].CAN.BUF[0]));
        MCO_TransmitPDO(TPDONr);
        retval = TRUE;
#if USECB_TPDORDY
      }
#endif
    }
#endif // USE_EVENT_TIME

#if USE_INHIBIT_TIME
    // is the inihibit timer currently running?
    if (gTPDOConfig[TPDONr].inhibit_status > INHITIM_EXPIRED)
    {
      // has the inhibit time expired?
      if (MCOHW_IsTimeExpired(gTPDOConfig[TPDONr].inhibit_timestamp))
      {
        // is there a new transmit message already waiting?
        if (gTPDOConfig[TPDONr].inhibit_status == INHITIM_RUNNING_TRIGGERED)
        { 
#if USECB_TPDORDY
          if (MCOUSER_TPDOReady(gTPDOConfig[TPDONr].PDONr,3))
          {
#endif
            // transmit now
            MCO_TransmitPDO(TPDONr);
            retval = TRUE;
#if USECB_TPDORDY
          }
#endif
        }
        // no new message waiting, but timer expired
        else 
        {
          gTPDOConfig[TPDONr].inhibit_status = INHITIM_EXPIRED;
        }
      }
    }
    // is inhibit status INHITIM_RUNNING_TRIGGERED?
    else if ( (gTPDOConfig[TPDONr].inhibit_status < INHITIM_RUNNING_TRIGGERED) && 
              (gTPDOConfig[TPDONr].inhibit_time != 0) &&
              ((gTPDOConfig[TPDONr].inhibit_time < gTPDOConfig[TPDONr].event_time) || (gTPDOConfig[TPDONr].event_time == 0))
            )
    {
      // has application data changed?
      if (PDO_TXCOMP(TPDONr,&(gTPDOConfig[TPDONr].CAN.BUF[0])) != 0)
      {
        // Copy application data
        PDO_TXCOPY(TPDONr,&(gTPDOConfig[TPDONr].CAN.BUF[0]));
        // has inhibit time expired?
        if (gTPDOConfig[TPDONr].inhibit_status == INHITIM_EXPIRED)
        {
#if USECB_TPDORDY
          if (MCOUSER_TPDOReady(gTPDOConfig[TPDONr].PDONr,3))
          {
#endif
            // transmit now
            MCO_TransmitPDO(TPDONr);
            retval = TRUE;
#if USECB_TPDORDY
          }
#endif
        }
        else
        {
          // wait for inhibit time to expire 
          gTPDOConfig[TPDONr].inhibit_status = INHITIM_RUNNING_TRIGGERED;
        }
      }
    }
#endif // USE_INHIBIT_TIME
  } // PDO active (CAN_ID != 0)  
  return retval;
}
#endif // NR_OF_TPDOS > 0


/**************************************************************************
PUBLIC FUNCTIONS
***************************************************************************/ 

/**************************************************************************
DOES:    Initializes the MicroCANopen stack
         It must be called from within MCOUSER_ResetApplication
RETURNS: TRUE, if init OK, else FALSE (also when unconfigured and in LSS)
**************************************************************************/
UNSIGNED8 MCO_Init (
  UNSIGNED16 Baudrate,  // CAN baudrate in kbit (1000,800,500,250,125,50,25 or 10)
  UNSIGNED8 Node_ID,    // CANopen node ID (1-126)
  UNSIGNED16 Heartbeat  // Heartbeat time in ms (0 for none)
  )
{
  UNSIGNED32 i;

  if (Baudrate == 0)
  {
    Baudrate = 125; // default baud rate
  }

  // Init the global variables
  // MY_NODE_ID is gMCOConfig.Node_ID
  if (Node_ID != 0)
  {
    MY_NODE_ID = Node_ID;
  }
  // else use previously assigned ID (for example LSS)
  gMCOConfig.error_code = 0;
  gMCOConfig.Baudrate = Baudrate;
  gMCOConfig.heartbeat_time = Heartbeat;
  gMCOConfig.error_register = 0;
  gMCOConfig.last_rxtime = MCOHW_GetTime();
#if USE_LEDS
  // Initialize Control Lines
  DDRC |= 0x3;
  PORTC &= 0xFC;

  // Initialize LED blink control 200ms timer
  gMCOConfig.LED_timestamp = MCOHW_GetTime() + 200; 
  gMCOConfig.LEDtoggle = 0;
  gMCOConfig.LEDcntR = 0;
  gMCOConfig.LEDcntE = 0;
  gMCOConfig.LEDRun = LED_OFF;
  gMCOConfig.LEDErr = LED_ON;
#endif // USE_LEDS
#if USE_SYNC
  gMCOConfig.SYNCid = 0x80;
#endif // USE_SYNC
#if NR_OF_TPDOS > 0
  gMCOConfig.nrTPDOs = 0;
#endif
#if NR_OF_RPDOS > 0
  gMCOConfig.nrRPDOs = 0;
#endif

  // init the CAN interface
  if (!MCOHW_Init(Baudrate))
  {
    MCOUSER_FatalError(ERRFT_INIT);
  }

#if USE_LSS_SLAVE
  // Set receive filter for LSS master message 
  if (!MCOHW_SetCANFilter(LSS_MASTER_ID))
  {
    MCOUSER_FatalError(ERRFT_RXFLTN);
  }
  if (MY_NODE_ID == 0)
  {
    return FALSE; // stay in LSS mode
  }
#endif

  // continue with vartiable initialization
  gMCOConfig.heartbeat_msg.ID = 0x700+MY_NODE_ID;
  gMCOConfig.heartbeat_msg.LEN = 1;
  // current NMT state of this node = bootup
  MY_NMT_STATE = 0;

  // Init SDO Response/Abort message
  gTxSDO.ID = 0x0580+MY_NODE_ID;
  gTxSDO.LEN = 8;

#if ! MGR_MONITOR_ALL_NODES
#if (NR_OF_HB_CONSUMER > 0)
  // init heartbeat consumption
  for (i = 0; i < NR_OF_HB_CONSUMER; i++)
  {
    gHBCons[i].status = HBCONS_OFF; // disable consumption
  }
#endif // (NR_OF_HB_CONSUMER > 0)
#endif // MGR_MONITOR_ALL_NODES
   
#if NR_OF_TPDOS > 0
  i = 0;
  // init TPDOs
  while (i < NR_OF_TPDOS)
  {
    gTPDOConfig[i].CAN.ID = COBID_DISABLED; // Disable by default
    gTPDOConfig[i].PDONr = 0;
    i++;
  }
#endif

#if NR_OF_RPDOS > 0
  i = 0;
  // init RPDOs
  while (i < NR_OF_RPDOS)
  {
    gRPDOConfig[i].CANID = COBID_DISABLED; // Disable by default
    gRPDOConfig[i].PDONr = 0;
    i++;
  }
#endif

#if USECB_TIMEOFDAY
  // filter for CAN ID 0x100
  if (!MCOHW_SetCANFilter(0x100))
  {
    MCOUSER_FatalError(ERRFT_RXFLTN);
  }
#endif

  // filter for nmt master message
  if (!MCOHW_SetCANFilter(0))
  {
    MCOUSER_FatalError(ERRFT_RXFLTN);
  }
#if USE_SYNC
  // for SYNC message
  if (!MCOHW_SetCANFilter(gMCOConfig.SYNCid))
  {
    MCOUSER_FatalError(ERRFT_RXFLTN);
  }
#endif

#if USE_EMCY
  // Init emergency inhibit time
  gEF.emcy_inhibit = (EMCY_INHIBIT_TIME + 9) / 10;
#if ERROR_FIELD_SIZE > 0
  MCOP_ErrField_Flush();
#endif
#endif

#if USE_SDOMESH
  // for receiving meshed SDO requests
  for (i=1;i<=16;i++)
  { // for all 16 node IDs
    if (i != MY_NODE_ID)
    { // only proceed if node id is not own node id
      if (!MCOHW_SetCANFilter(CAN_ID_SDOREQUEST(i, MY_NODE_ID)))
      {
        MCOUSER_FatalError(ERRFT_RXFLTS);
      }
    }
  }
#else // USE_SDOMESH
#if USE_CiA447
  // for receiving CiA447 SDO requests
  for (i=1;i<=16;i++)
  { // for all 16 node IDs
    if (i != MY_NODE_ID)
    { // only proceed if node id is not own node id
      if (!MCOHW_SetCANFilter(CAN_ID_SDOREQUEST(i, MY_NODE_ID)))
      {
        MCOUSER_FatalError(ERRFT_RXFLTS);
      }
    }
  }
#endif // USE_CiA447
  // for standard CANopen SDO requests
  if (!MCOHW_SetCANFilter(0x600+MY_NODE_ID))
  {
    MCOUSER_FatalError(ERRFT_RXFLTS);
  }
#endif // USE_SDOMESH

#if USE_NODE_GUARDING
  // for Node Guarding requests
  if (!MCOHW_SetCANFilter(COBID_RTR | 0x700 | MY_NODE_ID))
  {
    MCOUSER_FatalError(ERRFT_RXFLTN);
  }
  gMCOConfig.NGtoggle = 0;
#endif

#if USE_SLEEP
  for (i = 0; i <= 16; i++)
  { // for wakeup/sleep requests
    if (!MCOHW_SetCANFilter(0x690+i))
    {
      MCOUSER_FatalError(ERRFT_RXFLTS);
    }
  }
#endif

#if (INDEX_FOR_DIAGNOSTICS != 0)
  // Init diagnostic data
  gMCODiag.Status = 0;
#if (TXFIFOSIZE > 0)
  gMCODiag.TxFIFOStatus = 0;
#endif
#if (RXFIFOSIZE > 0)
  gMCODiag.RxFIFOStatus = 0;
#endif
#if (MGRFIFOSIZE > 0)
  gMCODiag.RxMgrFIFOStatus = 0;
#endif
  gMCODiag.ProcTickPerSecMin = 0xFFFF;
  gMCODiag.ProcTickPerSecMax = 0;
  gMCODiag.ProcTickBurstMax = 0;
  gMCODiag.ProcRxPerSecMin = 0xFFFF;
  gMCODiag.ProcRxPerSecMax = 0;
#if MGR_MONITOR_ALL_NODES
  gMCODiag.ProcMgrTickPerSecMin = 0xFFFF;
  gMCODiag.ProcMgrTickPerSecMax = 0;
  gMCODiag.ProcMgrTickBurstMax = 0;
  gMCODiag.ProcMgrRxPerSecMin = 0xFFFF;
  gMCODiag.ProcMgrRxPerSecMax = 0;
#endif
  gMCODiag.TickCnt = 0;
  gMCODiag.RxCnt = 0;
  gMCODiag.BurstCnt = 0;
#if MGR_MONITOR_ALL_NODES
  gMCODiag.MgrTickCnt = 0;
  gMCODiag.MgrRxCnt = 0;
  gMCODiag.MgrBurstCnt = 0;
#endif
#endif // Diag record

  // signal to MCO_ProcessStack: we just initialized
  gTPDONr = 0xFFFF;

  return TRUE;
}  


/**************************************************************************
DOES:    Common exit routine for SDO_Handler.
         Send SDO response with variable length (1-4 bytes).
         Assumes that gTxCAN.ID, LEN and BUF[1-3] are already set.
         Requires data to be in little endian format.
RETURNS: 1 if response transmitted
**************************************************************************/
UNSIGNED8 MCO_ReplyWith (
  UNSIGNED8 *pDat,  // pointer to sdo data
  UNSIGNED8 len     // number of bytes of data in SDO
  )
{
  UNSIGNED8 k; // for loop counter

  // expedited, len of data
  gTxSDO.BUF[0] = 0x43 | ((4-len) << 2);
  // copy data
  for (k = 4; k < 8; k++)
  {
    if (k < len+4)
    {
      gTxSDO.BUF[k] = *pDat;
      pDat++;
    }
    else
    { // required by conformance test, fill unused with zero
      gTxSDO.BUF[k] = 0;
    }
  }

  // transmit message
  if (!MCOHW_PushMessage(&gTxSDO))
  {
    // failed to transmit
    MCOUSER_FatalError(ERROFL_SDO);
  }

  // transmitted ok
  return 1;
}


/**************************************************************************
DOES:    Generates an SDO Abort Response
RETURNS: nothing
**************************************************************************/
void MCO_SendSDOAbort (
  UNSIGNED32 ErrorCode  // 4 byte SDO abort error code
  )
{
UNSIGNED8 i;

#if USE_EXTENDED_SDO
  // Inform extended SDO handling about the abort
  // XSDO_Abort();
  // commented as SDO server info is not available here
#endif // USE_EXTENDED_SDO

  // construct message data
  gTxSDO.BUF[0] = 0x80;
  for (i=0;i<4;i++)
  {
    gTxSDO.BUF[4+i] = (UNSIGNED8)ErrorCode;
    ErrorCode >>= 8;
  }

  // transmit message
  if (!MCOHW_PushMessage(&gTxSDO))
  {
    // failed to transmit
    MCOUSER_FatalError(ERROFL_SDO);
  }
}


#if NR_OF_TPDOS > 0
#if USE_INHIBIT_TIME > 0
/**************************************************************************
DOES:    Called by application when a TPDO should be transmitted.
         Can be called after a write to the process image to avoid lengthy
         auto-detection of a COS (Change Of State)
RETURNS: nothing
**************************************************************************/
void MCO_TriggerTPDO (
  UNSIGNED16 TPDONr  // TPDO number to transmit (range 1 to 512)
  )
{
UNSIGNED16 found_rec = 0;

  // first guess: PDONr matches offset
  if ( (TPDONr < NR_OF_TPDOS) &&
       (gTPDOConfig[TPDONr-1].PDONr == TPDONr)
     )
  { // found the TPDO
    found_rec = TPDONr-1;
  }
  else
  {
    while ((found_rec < gMCOConfig.nrTPDOs) && (gTPDOConfig[found_rec].PDONr != TPDONr))
    {
      found_rec++;
    }
  }
  // found_rec is gMCOConfig.nrTPDOs if the TPDO was not found
  if (found_rec < gMCOConfig.nrTPDOs)
  { // Update TPDO data and mark for transmission
    // Copy current process data
    PDO_TXCOPY(found_rec,( UNSIGNED8 *)&(gTPDOConfig[found_rec].CAN.BUF[0]));
    // Mark for ASAP transmission
    if (gTPDOConfig[found_rec].inhibit_status == INHITIM_EXPIRED)
    { // inhibit timer is currently NOT running
      gTPDOConfig[found_rec].inhibit_timestamp = MCOHW_GetTime() - 1;
    }
    gTPDOConfig[found_rec].inhibit_status = INHITIM_RUNNING_TRIGGERED;
    // now it is marked for transmission, and timer is corrected
  }
}
#endif // USE_INHIBIT_TIME > 0


/**************************************************************************
DOES:    Called when a TPDO needs to be transmitted
RETURNS: nothing
**************************************************************************/
void MCO_TransmitPDO (
  UNSIGNED16 PDONr  // TPDO number to transmit, as gTPDOConfig[] index
  )
{
#if USE_INHIBIT_TIME
  // new inhibit timer started
  gTPDOConfig[PDONr].inhibit_status = INHITIM_RUNNING_NO_TRIGGER;
  gTPDOConfig[PDONr].inhibit_timestamp = MCOHW_GetTime() + gTPDOConfig[PDONr].inhibit_time;
#endif
#if USE_EVENT_TIME
  gTPDOConfig[PDONr].event_timestamp = MCOHW_GetTime() + gTPDOConfig[PDONr].event_time;
#endif
#if USE_SYNC
  gTPDOConfig[PDONr].SYNCcnt = gTPDOConfig[PDONr].TType;
#endif

  if (!MCOHW_PushMessage(&gTPDOConfig[PDONr].CAN))
  {
    MCOUSER_FatalError(ERROFL_PDO);
  }
}
#endif // NR_OF_TPDOS > 0


/**************************************************************************
DOES:    Handles the NMT Master Message, switching NMT Slave state
RETURNS: nothing
**************************************************************************/
void MCO_HandleNMTRequest (
  UNSIGNED8 NMTReq
  )
{
#ifdef __SIMULATION__
  SimDriver_printf("CANopen Event: NMT message received: 0x%2.2x\n",NMTReq);
#endif

  switch (NMTReq)
  {
    // start node
    case NMTMSG_OP:
      if (MY_NMT_STATE != NMTSTATE_OP)
      { // only if not already operational
        // set new state
        MY_NMT_STATE = NMTSTATE_OP;
#if USE_LEDS
        gMCOConfig.LEDRun = LED_ON;
#endif
#if USECB_NMTCHANGE
        // Call back to user / application
        MCOUSER_NMTChange(MY_NMT_STATE);
#endif
#if NR_OF_TPDOS > 0
        MCO_PrepareTPDOs();
#endif
#if NR_OF_RPDOS > 0
        MCO_PrepareRPDOs();
#endif
      }
      break;

    // stop node
    case NMTMSG_STOP:
      // set new state
      MY_NMT_STATE = NMTSTATE_STOP;
#if USE_LEDS
      gMCOConfig.LEDRun = LED_FLASH1;
#endif
#if USECB_NMTCHANGE
      // Call back to user / application
      MCOUSER_NMTChange(MY_NMT_STATE);
#endif
      break;

    // enter pre-operational
    case NMTMSG_PREOP:
      // set new state
      MY_NMT_STATE = NMTSTATE_PREOP;
#if USE_LEDS
      gMCOConfig.LEDRun = LED_BLINK;
#endif
      // Call back to user / application
#if USECB_NMTCHANGE
      // Call back to user / application
      MCOUSER_NMTChange(MY_NMT_STATE);
#endif
      break;

    // application reset
    case NMTMSG_RESETAPP:
      MCOUSER_ResetApplication();
      break;

    // node reset communication
    case NMTMSG_RESETCOM:
      MCOUSER_ResetCommunication();
      break;

    // unknown command
    default:
#if USE_EMCY
      // Only supported by MicroCANopen Plus
      if (!MCOP_PushEMCY(0x8200,NMTReq,0,0,0,0))
      {
        MCOUSER_FatalError(ERROFL_EMCY);
      }
#endif // USE_EMCY
      break;
  }
}


#if NR_OF_RPDOS > 0
/**************************************************************************
DOES:    This function initializes a receive PDO. Once initialized, the 
         MicroCANopen stack automatically updates the data at offset.
NOTE:    For data consistency, the application should not read the data
         while function MCO_ProcessStack() executes.
RETURNS: nothing
**************************************************************************/
void MCO_InitRPDO (
  UNSIGNED16 PDO_NR,      // RPDO number (1-512)
  UNSIGNED32 CAN_ID,      // CAN identifier to be used (set to 0 to use default)
  UNSIGNED8 len,          // Number of data bytes in RPDO
  UNSIGNED16 offset       // Offset to data location in process image
  )
{
#if USECB_ODDATARECEIVED
UNSIGNED8 MEM_CONST *pSDO; // pointer into SDO response table
#endif
UNSIGNED16 lp;

#if CHECK_PARAMETERS
  // check PDO range and check node id range 1 - 127
  // 29-bit COB-IDs are not supported
  if (((PDO_NR < 1)     || (PDO_NR > 512))     || 
      ((MY_NODE_ID < 1) || (MY_NODE_ID > 127)) ||
      (len > 8)        
     )
  {
    MCOUSER_FatalError(ERRFT_IPP);
  }
  // is size of process image exceeded?
  if (offset >= PROCIMG_SIZE)   
  { 
    MCOUSER_FatalError(ERRFT_PIR);
  }
#endif

  // check, if this PDO was initialized before
  lp = 0;
  while (lp < gMCOConfig.nrRPDOs)
  {
    if (gRPDOConfig[lp].PDONr == PDO_NR)
    { // found
      break;
    }
    lp++;
  }

  // is this a new entry?
  if (lp >= gMCOConfig.nrRPDOs)
  { // not yet in list, new entry
    gMCOConfig.nrRPDOs++;
    if (gMCOConfig.nrRPDOs > NR_OF_RPDOS) 
    { // error, all PDOs used
      gMCOConfig.nrRPDOs--;
      MCOUSER_FatalError(ERRFT_RPDOR);
      return;
    }
  }

  // if we reach here, lp is record to use
  gRPDOConfig[lp].PDONr = PDO_NR;

  // initialize PDO
  gRPDOConfig[lp].len = len;
  gRPDOConfig[lp].offset = offset;
  if (IS_CANID_RESTRICTED(CAN_ID))
  { // ID not usable for PDO
    if (((CAN_ID & 0x07FF) == 0) && (PDO_NR <= 4))
    { // if ID is zero and PDO <= 4 then use default CAN ID
      gRPDOConfig[lp].CANID = 0x200 + (0x100 * ((UNSIGNED16)(PDO_NR-1))) + MY_NODE_ID;
    }
    else
    { // disable PDO
      gRPDOConfig[lp].CANID = COBID_DISABLED;
    }
  }
  else
  { // use CAN ID passed
    gRPDOConfig[lp].CANID = (COBID_TYPE) (CAN_ID & 0x1FFFFFFFUL);
  }

  if (CAN_ID & 0x80000000UL)
  { // PDO is disabled
    gRPDOConfig[lp].CANID |= COBID_DISABLED;
  }

#if USECB_ODDATARECEIVED
  gRPDOConfig[lp].map = 0; // offset in SDOResponseTable
  pSDO = OD_SDOResponseTablePtr(gRPDOConfig[lp].map);
  // find first entry with RPDO mapping 0x16xx where xx is PDO_NR-1
  while  (!( (pSDO[0] == 0x4F     ) &&
             (pSDO[1] == PDO_NR-1 ) &&
             (pSDO[2] == (UNSIGNED8) ((0x1600 + (PDO_NR-1)) >> 8))
           )
         )
  {
    gRPDOConfig[lp].map += 8; // next record
    pSDO += 8;
    if (*pSDO == 0xFF)
    { // End of table, no mapping found
      MCOUSER_FatalError(ERRFT_RPMAP);
    }
  }
  // go to next entry at subindex one
  gRPDOConfig[lp].map += 8; // next record
#endif // USECB_ODDATARECEIVED

  gRPDOConfig[lp].TType = 255;
  gMCOConfig.error_code &= 0x7F; // Signal that RPDO filter are not yet set

#if USE_DYNAMIC_PDO_MAPPING
  XPDO_ResetPDOMapEntry(1,PDO_NR);
#endif
}


/**************************************************************************
DOES:    This function changes the COBID of a RPDO
RETURNS: nothing
**************************************************************************/
void MCO_ChangeRPDOID (
  UNSIGNED16 PDO_NR,      // RPDO number (1-512)
  UNSIGNED32 CAN_ID       // CAN identifier to be used 
  )
{
UNSIGNED16 lp;

#if CHECK_PARAMETERS
  // check PDO range and check node id range 1 - 127
  // 29-bit COB-IDs are not supported
  if (((PDO_NR < 1)     || (PDO_NR > 512))     || 
      ((MY_NODE_ID < 1) || (MY_NODE_ID > 127)) ||
      (CAN_ID & 0x20000000UL) || (CAN_ID == 0)
     )
  {
    MCOUSER_FatalError(ERRFT_IPP);
  }
#endif

  // find matching PDOnr
  lp = 0;
  while ((lp < NR_OF_RPDOS) && (gRPDOConfig[lp].PDONr != PDO_NR))
  {
    lp++;
  }

  if ((lp < NR_OF_RPDOS) && (gRPDOConfig[lp].PDONr == PDO_NR))
  {
    // delete old CAN filter
    if ((gRPDOConfig[lp].CANID & COBID_DISABLED) == 0)
    { // PDO is enabled
      MCOHW_ClearCANFilter(gRPDOConfig[lp].CANID);
    }
    // set new CAN ID
    gRPDOConfig[lp].CANID = (COBID_TYPE) (CAN_ID & 0x1FFFFFFFUL);
    // set new filter and enabled/disabled status
    if (CAN_ID & 0x80000000UL)
    { // PDO is disabled
      gRPDOConfig[lp].CANID |= COBID_DISABLED;
    }
    else
    {
      MCOHW_SetCANFilter(gRPDOConfig[lp].CANID);
    }
    #ifdef __SIMULATION__
    SimDriver_printf("RPRO_Nr %d init to CAN ID 0x%8.8X\n",PDO_NR,gRPDOConfig[lp].CANID);
    #endif
  }

}
#endif // NR_OF_RPDOS > 0


#if NR_OF_TPDOS > 0
/**************************************************************************
DOES:    This function initializes a transmit PDO. Once initialized, the 
         MicroCANopen stack automatically handles transmitting the PDO.
         The application can directly change the data at any time.
NOTE:    For data consistency, the application should not write to the data
         while function MCO_ProcessStack executes.
RETURNS: nothing
**************************************************************************/
void MCO_InitTPDO
  (
  UNSIGNED16 PDO_NR,       // TPDO number (1-512)
  UNSIGNED32 CAN_ID,       // CAN identifier to be used (set to 0 to use default)
  UNSIGNED16 event_time,   // Transmitted every event_tim ms 
  UNSIGNED16 inhibit_time, // Inhibit time in ms for change-of-state transmit
                           // (set to 0 if ONLY event_tim should be used)
  UNSIGNED8 len,           // Number of data bytes in TPDO
  UNSIGNED16 offset        // Offset to data location in process image
  )
{
UNSIGNED16 lp;
UNSIGNED16 found;

#if CHECK_PARAMETERS
  // check PDO range, node id, len range 0-8 and event time or inhibit time set
  // 29-bit COB-IDs are not supported
  if (((PDO_NR < 1)     || (PDO_NR > 512))     ||
      ((MY_NODE_ID < 1) || (MY_NODE_ID > 127)) ||
      (len > 8)        
     )
  {
    MCOUSER_FatalError(ERRFT_IPP);
  }
  // is size of process image exceeded?
  if (offset >= PROCIMG_SIZE)   
  { 
    MCOUSER_FatalError(ERRFT_PIR);
  }
  // is PDO number 
#endif

  // check, if this PDO was initialized before
  lp = 0;
  while (lp < gMCOConfig.nrTPDOs)
  {
    if (gTPDOConfig[lp].PDONr == PDO_NR)
    { // found
      break;
    }
    lp++;
  }

  // is this a new entry?
  if (lp >= gMCOConfig.nrTPDOs)
  { // not yet in list, new entry
    gMCOConfig.nrTPDOs++;
    if (gMCOConfig.nrTPDOs > NR_OF_TPDOS) 
    { // error, all PDOs used
      gMCOConfig.nrTPDOs--;
      MCOUSER_FatalError(ERRFT_TPDOR);
      return;
    }
  }

  // if we reach here, gMCOConfig.nrTPDOs points to next record
  gTPDOConfig[lp].PDONr = PDO_NR;

  // initialize PDO
  gTPDOConfig[lp].CAN.LEN = len;
  gTPDOConfig[lp].offset = offset;

  if (IS_CANID_RESTRICTED(CAN_ID))
  { // ID not usable for PDO
    if (((CAN_ID & 0x07FF) == 0) && (PDO_NR <= 4))
    { // if ID is zero, use default
      gTPDOConfig[lp].CAN.ID = 0x180 + (0x100 * (UNSIGNED16)(PDO_NR-1)) + MY_NODE_ID;
    }
    else
    { // else disable PDO
      gTPDOConfig[lp].CAN.ID = COBID_DISABLED;
    }
  }
  else
  { // use COB ID
    gTPDOConfig[lp].CAN.ID = (COBID_TYPE) (CAN_ID & 0x1FFFFFFFUL);
  }

  if (CAN_ID & 0x40000000UL)
  { // PDO doesn't support RTR
    gTPDOConfig[lp].CAN.ID |= COBID_RTR;
  }

  if (CAN_ID & 0x80000000UL)
  { // PDO is disabled
    gTPDOConfig[lp].CAN.ID |= COBID_DISABLED;
  }

#if USE_EVENT_TIME
  gTPDOConfig[lp].event_time = event_time;
#endif
#if USE_INHIBIT_TIME
  gTPDOConfig[lp].inhibit_time = inhibit_time;
#endif
  
  // handle transmission type, take from OD, if found
  found = MCO_SearchOD(0x1800+PDO_NR-1,2);
  if (found < 0xFFFF)
  {
    gTPDOConfig[lp].TType = *OD_SDOResponseTablePtr((found<<3)+4);
  }
  else
  { // set default
    gTPDOConfig[lp].TType = 255;
  }

#if USE_DYNAMIC_PDO_MAPPING
  XPDO_ResetPDOMapEntry(0,PDO_NR);
#endif
}


/**************************************************************************
DOES:    This function initializes a transmit PDO. Once initialized, the 
         MicroCANopen stack automatically handles transmitting the PDO.
         The application can directly change the data at any time.
NOTE:    For data consistency, the application should not write to the data
         while function MCO_ProcessStack executes.
         This is an extended version of MCO_InitTPDO() that includes the
         transmission type. MCO_InitTPDO() is still available for backward-
         compatibility.
RETURNS: nothing
**************************************************************************/
void MCO_InitTPDOFull
  (
  UNSIGNED16 PDO_NR,       // TPDO number (1-512)
  UNSIGNED32 CAN_ID,       // CAN identifier to be used (set to 0 to use default)
  UNSIGNED16 event_time,   // Transmitted every event_tim ms 
  UNSIGNED16 inhibit_time, // Inhibit time in ms for change-of-state transmit
                           // (set to 0 if ONLY event_tim should be used)
  UNSIGNED8 trans_type,    // Transmission type of the TPDO
  UNSIGNED8 len,           // Number of data bytes in TPDO
  UNSIGNED16 offset        // Offset to data location in process image
  )
{
UNSIGNED16 lp;

  MCO_InitTPDO(PDO_NR,CAN_ID,event_time,inhibit_time,len,offset);
  
  // verify that this PDO is initialized
  lp = 0;
  while (lp < gMCOConfig.nrTPDOs)
  {
    if (gTPDOConfig[lp].PDONr == PDO_NR)
    { // found
      break;
    }
    lp++;
  }

  // not in list, error
  if (lp >= gMCOConfig.nrTPDOs)
  {
    MCOUSER_FatalError(ERRFT_TPDOR);
    return;
  }

  gTPDOConfig[lp].TType = trans_type;
}


/**************************************************************************
DOES:    This function changes the COBID of a TPDO
RETURNS: nothing
**************************************************************************/
void MCO_ChangeTPDOID (
  UNSIGNED16 PDO_NR,      // RPDO number (1-512)
  UNSIGNED32 CAN_ID       // CAN identifier to be used 
  )
{
UNSIGNED16 lp;

#if CHECK_PARAMETERS
  // check PDO range and check node id range 1 - 127
  // 29-bit COB-IDs are not supported
  if (((PDO_NR < 1)     || (PDO_NR > 512))     || 
      ((MY_NODE_ID < 1) || (MY_NODE_ID > 127)) ||
      (CAN_ID & 0x20000000UL) || (CAN_ID == 0)
     )
  {
    MCOUSER_FatalError(ERRFT_IPP);
  }
#endif

  // find matching PDOnr
  lp = 0;
  while ((lp < NR_OF_TPDOS) && (gTPDOConfig[lp].PDONr != PDO_NR))
  {
    lp++;
  }

  if ((lp < NR_OF_TPDOS) && (gTPDOConfig[lp].PDONr == PDO_NR))
  {
    // set new CAN ID
    gTPDOConfig[lp].CAN.ID = (COBID_TYPE) CAN_ID;
    // set new filter and enabled/disabled status
    if (CAN_ID & 0x80000000UL)
    { // PDO is disabled
      gTPDOConfig[lp].CAN.ID |= COBID_DISABLED;
    }
#ifdef __SIMULATION__
    SimDriver_printf("TPDO_Nr %d set to CAN ID 0x%8.8X\n",PDO_NR,CAN_ID);
#endif
  }

}
#endif // NR_OF_TPDOS > 0


/**************************************************************************
DOES:    This function processes the next CAN message from the CAN receive
         queue. When using an RTOS, this can be turned into a task
         triggered by a CAN receive event.
RETURNS: FALSE, if no message was processed, 
         TRUE, if a CAN message received was processed
**************************************************************************/
UNSIGNED8 MCO_ProcessStackRx (
  void
  )
{
UNSIGNED8 retval = FALSE;
#if USECB_TIMEOFDAY
UNSIGNED32 millis;
UNSIGNED16 days;
#endif

  // work on next incoming messages
  // if message received
  if (MCOHW_PullMessage(&gRxCAN))
  {

#if (INDEX_FOR_DIAGNOSTICS != 0)
    gMCODiag.RxCnt++;
#endif

#if USECB_TIMEOFDAY
    if ((gRxCAN.ID == 0x100) &&
      ((MY_NMT_STATE == NMTSTATE_PREOP) || (MY_NMT_STATE == NMTSTATE_OP))
     )
    { // time stamp received, needs to be processed
      millis = gRxCAN.BUF[0];
      millis <<= 8;
      millis += gRxCAN.BUF[1];
      millis <<= 8;
      millis += gRxCAN.BUF[2];
      millis <<= 8;
      millis += gRxCAN.BUF[3];
      days = gRxCAN.BUF[4];
      days <<= 8;
      days += gRxCAN.BUF[5];
      MCOUSER_TimeOfDay(millis,days);
      retval = TRUE;
    }
    else
#endif

    // is it an NMT master message?
    if (gRxCAN.ID == NMT_MASTER_ID)
    {
      // nmt message is for this node or all nodes
      if ((gRxCAN.BUF[1] == MY_NODE_ID) || (gRxCAN.BUF[1] == 0))
      {
        MCO_HandleNMTRequest(gRxCAN.BUF[0]);
        retval = TRUE;
      } // NMT message addressed to this node
    } // NMT master message received
    
#if USE_LSS_SLAVE
    else if ( (gRxCAN.ID == LSS_MASTER_ID) &&    
              ((MY_NMT_STATE == NMTSTATE_LSS) || (MY_NMT_STATE == NMTSTATE_STOP))
            )
    { // in LSS slave mode only process this message, ignore the rest
      LSS_HandleMsg(gRxCAN.LEN,&(gRxCAN.BUF[0]));
      retval = TRUE;
    }
#endif

#if USE_SYNC
    // Handle SYNC
    else if (gRxCAN.ID == gMCOConfig.SYNCid)
    { // SYNC received
      if (MCOP_HandleSYNC() != 0)
      { // SYNC PDOs processed
        retval = TRUE;
      }
    }
#endif // USE_SYNC

#if NR_OF_RPDOS > 0
    // Handle RPDO
    else if (MCO_HandleRPDO(&gRxCAN) == 1)
    { // RPDO processed
      retval = TRUE;
    }
#endif // NR_OF_RPDOS > 0

    // SDO Request, handle if node is not stopped...
    else if ((IS_CAN_ID_SDOREQUEST(gRxCAN.ID)) && (MY_NMT_STATE != NMTSTATE_STOP))
    { // one of the SDO messages for us
      // set response ID
      gTxSDO.ID = CAN_ID_SDORESPONSE_FROM_RXID(gRxCAN.ID);
      // handle SDO request - return value not used in this version
#if USE_CiA447
      PROFILE_SetSDOFromNode(0);
      if (gRxCAN.ID != 0x600 + MY_NODE_ID)
      {
        PROFILE_SetSDOFromNode((UNSIGNED8) ((gRxCAN.ID & 0x0030) >> 4) + (((gRxCAN.ID & 0x0700) - 0x0200) >> 6) + 1);
      }
#endif
      MCO_HandleSDORequest(&gRxCAN.BUF[0]);
      retval = TRUE;
    }

#if USE_NODE_GUARDING
    else if (MCOP_HandleGuarding(gRxCAN.ID) == 1)
    {
      retval = TRUE;
    }
#endif

#if ! MGR_MONITOR_ALL_NODES
#if (NR_OF_HB_CONSUMER > 0)
    // Check if message received was a Heartbeat monitored
    else if (MCOP_ConsumeHB(&gRxCAN))
    {
      retval = TRUE;
    }
#endif// (NR_OF_HB_CONSUMER > 0)
#endif // MGR_MONITOR_ALL_NODES

#if USE_SLEEP
    else if ((gRxCAN.ID >= 0x690) && (gRxCAN.ID <= 0x690+16))
    { // sleep/wakeup requst
      MCOUSER_Sleep(gRxCAN.ID-0x690,gRxCAN.BUF[0],gRxCAN.BUF[1]);
      retval = TRUE;
    }
#endif // USE SLEEP

  } // Message received
  
  return retval; // no message in receive queue
}


/**************************************************************************
DOES:    This function executes all sub functions required to keep the 
         CANopen stack operating. It should be called frequently. When used
         in an RTOS it should be called repeatedly every RTOS time tick
         until it returns zero.
RETURNS: FALSE, if there was nothing to process 
         TRUE, if functions were
**************************************************************************/
UNSIGNED8 MCO_ProcessStackTick (
  void
  )
{
UNSIGNED8 retval = NOT_SET;

  // check if this is right after boot-up
  // was set by MCO_Init
  if (gTPDONr >= 0xFFFE)
  { // first call or wait for bootup to be transmited
    if (gTPDONr > 0xFFFE)
    {
      // init heartbeat time
      gMCOConfig.heartbeat_timestamp = MCOHW_GetTime() + 1000;
      // send boot-up message  
      if (!MCOHW_PushMessage(&gMCOConfig.heartbeat_msg))
      {
        MCOUSER_FatalError(ERROFL_HBT);
      }
      gTPDONr--;
#if USECB_NMTCHANGE
      MCOUSER_NMTChange(NMTSTATE_BOOT);
#endif
      retval = TRUE;
    }
    else
    { // Now wait for bootup to go out
      if (MCOHW_GetStatus() & HW_CERR)
      { // a CAN error occured, re-try bootup after 1s
        if (MCOHW_IsTimeExpired(gMCOConfig.heartbeat_timestamp))
        { // start again
          gTPDONr = 0xFFFF;
        }
      }
      if ((MCOHW_GetStatus() & HW_TXBSY) == 0)
      { // Bootup went out, now continue
        gMCOConfig.heartbeat_timestamp = MCOHW_GetTime() + gMCOConfig.heartbeat_time;
#if AUTOSTART
        // going into operational state
        MY_NMT_STATE = NMTSTATE_OP;
#if USE_LEDS
        gMCOConfig.LEDRun = LED_ON;
        gMCOConfig.LEDErr = LED_OFF;
#endif
#if NR_OF_TPDOS > 0
        MCO_PrepareTPDOs();
#endif
#if NR_OF_RPDOS > 0
        MCO_PrepareRPDOs();
#endif
#if USECB_NMTCHANGE
        MCOUSER_NMTChange(NMTSTATE_OP);
#endif
#else // AUTOSTART
        // going into pre-operational state
        MY_NMT_STATE = NMTSTATE_PREOP;
#if USE_LEDS
        gMCOConfig.LEDRun = LED_BLINK;
        gMCOConfig.LEDErr = LED_OFF;
#endif
#if USECB_NMTCHANGE
        MCOUSER_NMTChange(NMTSTATE_PREOP);
#endif
#endif// AUTOSTART

#if USE_EMCY
        // Only supported by MicroCANopen Plus
        // set time stamp to expired
        gEF.emcy_timestamp = MCOHW_GetTime() - 1;
        // send EMCY clear message  
        if (!MCOP_PushEMCY(0,0,0,0,0,0))
        {
          MCOUSER_FatalError(ERROFL_EMCY);
        }
#endif // USE_EMCY

        // return TPDONr value to default
        gTPDONr = NR_OF_TPDOS;

        retval = TRUE;
      }
    }
  }

#if USE_LSS_SLAVE
  else if ((MY_NMT_STATE != NMTSTATE_PREOP) && (MY_NMT_STATE != NMTSTATE_OP))
  { // work on LSS
    LSS_DoLSS();
  }
#endif

#if USE_LEDS
  else if ((MCOHW_IsTimeExpired(gMCOConfig.LED_timestamp)))
  { // LED time did expire
    gMCOConfig.LED_timestamp = MCOHW_GetTime() + 200; 
    MCO_SwitchLEDs();
  }
#endif // USE_LEDS

#if USE_EMCY
  // Check if an EMCY waits for transmission
  if ((retval == NOT_SET) && (MCOHW_IsTimeExpired(gEF.emcy_timestamp)))
  { // Timer overrun protection, ensure that it remains expired
    gEF.emcy_timestamp = MCOHW_GetTime() - 1;
    if (gEF.emcy_msg.ID != 0)
    { // An emergency is due for transmission
      MCOHW_PushMessage(&(gEF.emcy_msg));
      gEF.emcy_msg.ID = 0; // Mark as transmitted
      // now reset the inhibit time
      gEF.emcy_timestamp = MCOHW_GetTime() + gEF.emcy_inhibit;
      retval = TRUE;
    }
  }
#endif

#if NR_OF_TPDOS > 0
  // is the node operational?
  if ((retval == NOT_SET) && (MY_NMT_STATE == NMTSTATE_OP))
  {
    // check next TPDO for transmission
    gTPDONr++;
    if (gTPDONr >= gMCOConfig.nrTPDOs)
    {
      gTPDONr = 0;
    }
    if (MCO_HandleTPDO(gTPDONr) == 1)
    { // TPDO was generated
      retval = TRUE;
    }
  } // if node is operational
#endif // NR_OF_TPDOS > 0
  
#if USE_BLOCKED_SDO
  // Check if we are in the middle of a block read transfer
  if ((retval == NOT_SET) && (XSDO_BlkRdProgress()))
  { // yes, message generated
    retval = TRUE;
  }
#endif //USE_BLOCKED_SDO

  // do we produce a heartbeat?
  if ((retval == NOT_SET) && (gMCOConfig.heartbeat_time != 0) && 
      ((MY_NMT_STATE == NMTSTATE_PREOP) || (MY_NMT_STATE == NMTSTATE_OP) || (MY_NMT_STATE == NMTSTATE_STOP))
     )
  {
    // has heartbeat time passed?
    if (MCOHW_IsTimeExpired(gMCOConfig.heartbeat_timestamp))
    {
      // transmit heartbeat message
      if (!MCOHW_PushMessage(&gMCOConfig.heartbeat_msg))
      {
        MCOUSER_FatalError(ERROFL_HBT);
      }
      // get new heartbeat time for next transmission
      gMCOConfig.heartbeat_timestamp = MCOHW_GetTime() + gMCOConfig.heartbeat_time;
      retval = TRUE;
    }
  }

#if ! MGR_MONITOR_ALL_NODES
#if (NR_OF_HB_CONSUMER > 0)
  // Check Heartbeat monitors
  mHBchn++;
  if (mHBchn > NR_OF_HB_CONSUMER)
  {
    mHBchn = 1;
  }
  if(MCOP_ProcessHBCheck(mHBchn) == HBCONS_LOST)
  { // timeout occured
    retval = TRUE;
  }
#endif
#endif

#if (INDEX_FOR_DIAGNOSTICS != 0)
  // diagnostics: how often does this get executed per second
  if (gMCODiag.BurstCnt == 0)
  { // first call, we start diagnostics now
    RTOS_LOCK; // receive counter might be incremented in some interrupt
    gMCODiag.RxCnt = 0;
    RTOS_UNLOCK;
    gMCODiag.TickCnt = 0;
    gMCODiag.BurstCnt = 1;
    gMCODiag.NextSecond = MCOHW_GetTime() + 1000;
  }
  else
  {
    gMCODiag.TickCnt++;
    if(MCOHW_IsTimeExpired(gMCODiag.NextSecond))
    { // once per second
      // work on current tick counter
      gMCODiag.ProcTickPerSecCur = gMCODiag.TickCnt;
      // check min, max
      if (gMCODiag.ProcTickPerSecCur < gMCODiag.ProcTickPerSecMin)
      {
        gMCODiag.ProcTickPerSecMin = gMCODiag.ProcTickPerSecCur;
      }
      if (gMCODiag.ProcTickPerSecCur > gMCODiag.ProcTickPerSecMax)
      {
        gMCODiag.ProcTickPerSecMax = gMCODiag.ProcTickPerSecCur;
      }
      // reset counter
      gMCODiag.TickCnt = 0;

      // work on current receive counter
      RTOS_LOCK; // receive counter might be incremented in some interrupt
      gMCODiag.ProcRxPerSecCur = gMCODiag.RxCnt;
      RTOS_UNLOCK;
      // check min (!= 0), max
      if ((gMCODiag.ProcRxPerSecCur < gMCODiag.ProcRxPerSecMin) && (gMCODiag.ProcRxPerSecCur != 0))
      {
        gMCODiag.ProcRxPerSecMin = gMCODiag.ProcRxPerSecCur;
      }
      if (gMCODiag.ProcRxPerSecCur > gMCODiag.ProcRxPerSecMax)
      {
        gMCODiag.ProcRxPerSecMax = gMCODiag.ProcRxPerSecCur;
      }
      // reset counter
      RTOS_LOCK; // receive counter might be incremented in some interrupt
      gMCODiag.RxCnt = 0;
      RTOS_UNLOCK;
    }

    // diagnostics: what is the longest burst of back to back calls executing something
    if (retval == TRUE)
    { // there was something to do
      gMCODiag.BurstCnt++;
    }
    else
    { // there was nothing to do
      if (gMCODiag.BurstCnt > gMCODiag.ProcTickBurstMax)
      {
        gMCODiag.ProcTickBurstMax = gMCODiag.BurstCnt;
      }
      gMCODiag.BurstCnt = 1;
    }
  }
#endif

  if (retval == NOT_SET)
  {
    retval = FALSE;
  }
  return retval;
}


/**************************************************************************
DOES:    This function implements the main MicroCANopen protocol stack. 
         It must be called frequently to ensure proper operation of the
         communication stack. 
         When using an RTOS this function should not be called, instead
         MCO_ProcessStackRx() and MCO_ProcessStackTick() should be used.
         Typically it is called from the while(1) loop in main.
RETURNS: 0 if nothing was done, 1 if a CAN message was sent or received
**************************************************************************/
UNSIGNED8 MCO_ProcessStack (
  void
  )
{
  if (MCO_ProcessStackRx() > 0)
  {
    return 1;
  }
  return MCO_ProcessStackTick();
}


/**************************************************************************
DOES:    Search the SDO Response table for a specifc index and subindex.
RETURNS: 0xFFFF if not found, otherwise the number of the record found
         (staring at zero)
NOTE:    Used from xpdo.c
**************************************************************************/
UNSIGNED16 MCO_SearchOD (
  UNSIGNED16 index,  // Index of OD entry searched
  UNSIGNED8 subindex // Subindex of OD entry searched 
  )
{
  UNSIGNED16 i;
  UNSIGNED8 i_hi, hi;
  UNSIGNED8 i_lo, lo;
  UNSIGNED8 MEM_CONST *p;
  UNSIGNED8 MEM_CONST *r;
  UNSIGNED16 retval = 0xFFFF;

  i = 0;
  i_hi = (UNSIGNED8) (index >> 8);
  i_lo = (UNSIGNED8) index;
  r = OD_SDOResponseTablePtr(0);
  while (i < 0xFFFF)
  {
    p = r;
    // set r to next record in table
    r += 8;
    // skip command byte
    p++;
    lo = *p;
    p++;
    hi = *p;
    // if index in table is 0xFFFF, then this is the end of the table
    if ((lo == 0xFF) && (hi == 0xFF))
    {
      break;
    }
    else if (lo == i_lo)
    { 
      if (hi == i_hi)
      { 
        p++;
        // entry found?
        if (*p == subindex)
        {
          retval = i;
          break;
        }
      }
    }
    i++;
  }
  // not found
  return retval;
}


/**************************************************************************
DOES:    Search the gODProcTable from user_xxxx.c for a specifc index 
         and subindex.
RETURNS: 0xFFFF if not found, otherwise the number of the record found
         (staring at zero)
NOTE:    Used from xpdo.c
**************************************************************************/
UNSIGNED16 MCO_SearchODProcTable (
  UNSIGNED16 index,   // Index of OD entry searched
  UNSIGNED8 subindex  // Subindex of OD entry searched 
  )
{
  UNSIGNED16 j = 0;
  UNSIGNED16 compare;
  // pointer to od records
  OD_PROCESS_DATA_ENTRY MEM_CONST *pOD;
  UNSIGNED16 retval = 0xFFFF;

  // initialize pointer
  pOD = OD_ProcTablePtr(0);
  // loop until maximum table size
  while (j < 0xFFFF)
  {
    compare = pOD->idx_hi;
    compare <<= 8;
    compare += pOD->idx_lo;
    // end of table reached? 
    if (compare == 0xFFFF)
    {
      break;
    }
    // index found?
    else if (compare == index)
    {
      // subindex found?
      if (pOD->subidx == subindex)
      {
        retval = j;
        break;
      }
    }
    // increment by SIZEOF(OD_PROCESS_DATA_ENTRY)
    pOD++;
    j++;
  }
  // not found
  return retval;
}

/*----------------------- END OF FILE ----------------------------------*/
