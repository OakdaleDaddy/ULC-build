/**************************************************************************
MODULE:    MCOP
CONTAINS:  MicroCANopen Plus implementation
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

// this structure holds the CAN message for SDO responses or aborts
extern CAN_MSG gTxSDO;


/**************************************************************************
GLOBAL/MODULE VARIABLES
***************************************************************************/ 

#if ! MGR_MONITOR_ALL_NODES
#if (NR_OF_HB_CONSUMER > 0)
HBCONS_CONFIG MEM_FAR gHBCons[NR_OF_HB_CONSUMER];
#endif // (NR_OF_HB_CONSUMER > 0)
#endif // MGR_MONITOR_ALL_NODES

#if USE_EMCY
EMCY_CONFIG MEM_FAR gEF; // Emergency configuration
#endif


/**************************************************************************
PUBLIC FUNCTIONS
***************************************************************************/ 

/**************************************************************************
DOES:    This function reads data from the process image and copies it
         to an OUTPUT location
RETURNS: Number of bytes that were copied
**************************************************************************/
UNSIGNED8 MCO_ReadProcessData (
  UNSIGNED8 MEM_PROC *pDest, // Destination pointer
  UNSIGNED8 length, // Number of bytes to copy
  UNSIGNED16 offset // Offset of source data in process image
  )
{
  PI_READ(PIACC_APP,offset,pDest,length);
  return length;
}


/**************************************************************************
DOES:    This function writes data from an INPUT location to the process 
         image
RETURNS: Number of bytes that were copied
**************************************************************************/
UNSIGNED8 MCO_WriteProcessData (
  UNSIGNED16 offset, // Offset of destination data in process image
  UNSIGNED8 length,  // Number of bytes to copy
  UNSIGNED8 MEM_PROC *pSrc // Source pointer
  )
{
  PI_WRITE(PIACC_APP,offset,pSrc,length);
  return length;
}


#if USE_EMCY
#if ERROR_FIELD_SIZE > 0
/**************************************************************************
DOES:    This function clears all entries of the error history [1003h]
RETURNS: Nothing
**************************************************************************/
void MCOP_ErrField_Flush (void)
{ // Reset pointer and counter for error field
  gEF.InPtr = 0;
  gEF.NrOfRec = 0;
}


/**************************************************************************
DOES:    This function adds an entry to the error history [1003h]
RETURNS: Nothing
**************************************************************************/
void MCOP_ErrField_Add (
  UNSIGNED32 err_value // the 32bit error code used in the last EMCY
  )
{ // add entry to field
  gEF.Field[gEF.InPtr] = err_value;
  // increment pointer and counter
  gEF.InPtr++;
  if (gEF.InPtr >= ERROR_FIELD_SIZE)
  { // roll over on end of buffer
    gEF.InPtr = 0;
  }
  gEF.NrOfRec++;
  if (gEF.NrOfRec >= ERROR_FIELD_SIZE)
  { // maximum is ERROR_FIELD_SIZE
    gEF.NrOfRec = ERROR_FIELD_SIZE;
  }
}


/**************************************************************************
DOES:    This function retrieves an entry from the error history [1003h]
         based on the subindex of [1003h]
RETURNS: 32bit error code stored at a subindex
**************************************************************************/
UNSIGNED32 MCOP_ErrField_Get (
  UNSIGNED8 subindex // Subindex number of [1003h]
  )
{
UNSIGNED32 ret_value = 0xFFFFFFFF;
INTEGER16 offset;

  if (subindex == 0)
  { // return number of entries
    ret_value = gEF.NrOfRec;
  }
  else
  { // return error value history (1 returns newest)
    if (subindex <= gEF.NrOfRec)
    { // only continue if subindex is in legal range
      offset = gEF.InPtr - subindex;
      if (offset < 0)
      {
        offset += ERROR_FIELD_SIZE;
      }
      ret_value = gEF.Field[offset];
    }
  }
  return ret_value;
}
#endif // ERROR_FIELD_SIZE > 0


/**************************************************************************
DOES:    Transmits an Emergency Message
RETURNS: TRUE - If msg was considered for transmit
         FALSE - If message was not sent due to duplicate
**************************************************************************/
UNSIGNED8 MCOP_PushEMCY
  (
  UNSIGNED16 emcy_code, // 16 bit error code
  UNSIGNED8  em_1, // 5 byte manufacturer specific error code
  UNSIGNED8  em_2,
  UNSIGNED8  em_3,
  UNSIGNED8  em_4,
  UNSIGNED8  em_5
  )
{
UNSIGNED8 ret_val;

  // Do not send same error code twice, unless its zero
  if ( (gEF.emcy_msg.BUF[0] == (UNSIGNED8) emcy_code) &&
       (gEF.emcy_msg.BUF[1] == (UNSIGNED8) (emcy_code >> 8)) &&
       (emcy_code != 0) && (emcy_code != 0x8130)
     )
  { // same error as last
    ret_val = FALSE;
  }
  else 
  {
    gEF.emcy_msg.ID = 0x80 + MY_NODE_ID;
    gEF.emcy_msg.LEN = 8;
    gEF.emcy_msg.BUF[0] = (UNSIGNED8) emcy_code;
    gEF.emcy_msg.BUF[1] = (UNSIGNED8) (emcy_code >> 8);
    gEF.emcy_msg.BUF[2] = gMCOConfig.error_register;
    gEF.emcy_msg.BUF[3] = em_1;
    gEF.emcy_msg.BUF[4] = em_2;
    gEF.emcy_msg.BUF[5] = em_3;
    gEF.emcy_msg.BUF[6] = em_4;
    gEF.emcy_msg.BUF[7] = em_5;

    ret_val = TRUE;

#if ERROR_FIELD_SIZE > 0
    if (emcy_code != 0)
    {
      // add error to error history
      MCOP_ErrField_Add((UNSIGNED32)emcy_code +
                        ((UNSIGNED32)em_1 << 16) +
                        ((UNSIGNED32)em_2 << 24)
                       );
    }
#endif

  }
  return ret_val;
}
#endif // USE_EMCY


#if (NR_OF_RPDOS > 0) || (NR_OF_TPDOS > 0)
/**************************************************************************
DOES: Common exit routine for SDO_Handler. 
      Send SDO response with write confirmation.
      Assumes that gTxSDO.ID, LEN and BUF[1-3] are already set
**************************************************************************/
void MCOP_WriteConfirm (
  void
  )
{
UNSIGNED8 i;

  // Load SDO Response into transmit buffer
  gTxSDO.BUF[0] = 0x60; // Write response code
  // Clear unused bytes
  for (i = 4; i < 8; i++)
  {
    gTxSDO.BUF[i] = 0;
  }
    
  // Transmit SDO Response message
  if (!MCOHW_PushMessage(&gTxSDO))
  {
    MCOUSER_FatalError(ERROFL_SDO);
  }
}


/**************************************************************************
DOES:    Handles incoming SDO Request for accesses to PDO Communication
         Parameters
RETURNS: 0: Wrong access, SDO Abort sent
         1: Access was made, SDO Response sent
GLOBALS: Various global variables with configuration information
**************************************************************************/
UNSIGNED8 SDO_HandlePDOComParam (
  UNSIGNED8  PDOType,  // 0 for TPDO, 1 for RPDO
  UNSIGNED16 index,    // OD index
   UNSIGNED8 *pData    // pointer to SDO Request message
  )
{
UNSIGNED16 lp;
UNSIGNED16 PDONr;    // PDONr - 1
UNSIGNED8 cmd;       // SDO Request command byte
UNSIGNED8 len_req;   // length of SDO write access
COBID_TYPE rdat = 0; // current response data
UNSIGNED8 reply[4];  // SDO reply value
#if USE_EVENT_TIME || USE_INHIBIT_TIME
UNSIGNED16 temp16;
#endif

  cmd = pData[0];
  PDONr = (index & 0x1FF) + 1;
  len_req = 4-((cmd >> 2) & 0x03);

  // calculate real PDONr offset
  if (PDOType == 0)
  { // TPDO, find the PDONr in array
#if (NR_OF_TPDOS > 0)
    lp = 0;
    while (gTPDOConfig[lp].PDONr != PDONr)
    {
      lp++;
      if (lp >= gMCOConfig.nrTPDOs)
      { // not found!
        MCO_SendSDOAbort(SDO_ABORT_NOT_EXISTS);
        return 0;
      }
    }
    // PDO found, set PDONr to index
    PDONr = lp;
#endif
  }
  else
  { // RPDO, find the PDONr in array
#if (NR_OF_RPDOS > 0)
    lp = 0;
    while (gRPDOConfig[lp].PDONr != PDONr)
    {
      lp++;
      if (lp >= gMCOConfig.nrRPDOs)
      { // not found!
        MCO_SendSDOAbort(SDO_ABORT_NOT_EXISTS);
        return 0;
      }
    }
    // PDO found, set PDONr to index
    PDONr = lp;
#endif
  }

  if (pData[3] == 0) // subindex
  { // Nr Of Entries: Read-only, "2" for RPDO, "5" for TPDO
    if (cmd == 0x40)
    { // Read
      if (PDOType == 0)
      { // TPDO
        reply[0] = 5;
      }
      else
      { // RPDO
        reply[0] = 2;
      }
      MCO_ReplyWith(reply,1);
      return 1;
    }
    else
    { // Write
      MCO_SendSDOAbort(SDO_ABORT_READONLY);
      return 0;
    }
  }

  if (PDOType == 0)
  { // TPDO
#if NR_OF_TPDOS > 0
    rdat = gTPDOConfig[PDONr].CAN.ID;
#endif
  }
  else
  { // RPDO
#if NR_OF_RPDOS > 0
    rdat = gRPDOConfig[PDONr].CANID;
#endif
  }

  if (pData[3] == 1) // subindex
  { // COB ID
    if (cmd == 0x40)
    { // Read
      // Load SDO Response into transmit buffer
      if ((rdat & COBID_DISABLED) == 0)
      {
        gTxSDO.BUF[7] = 0x00;
      }
      else
      {
        gTxSDO.BUF[7] = 0x80; // PDO Enable/Disable bit;
      }
      if (PDOType == 0)
      { // TPDO
        gTxSDO.BUF[7] |= 0x40; // No RTR
      }
      gTxSDO.BUF[6] = 0;
      gTxSDO.BUF[5] = (UNSIGNED8) ((rdat) >> 8) & 0x07; // Bits 8-10 of CAN ID
      gTxSDO.BUF[4] = (UNSIGNED8) rdat; // Bits 0-7 of CAN ID
      gTxSDO.BUF[0] = 0x43; // Expedited, 4 bytes
      if (!MCOHW_PushMessage(&gTxSDO))
      {
        MCOUSER_FatalError(ERROFL_SDO);
      }
      return 1;
    }
    else
    { // Write
#if USE_STATIC_PDO == 1
      MCO_SendSDOAbort(SDO_ABORT_READONLY);
      return 0;
#else
      if (len_req != 4)
      { // length of access is not 4 bytes
        MCO_SendSDOAbort(SDO_ABORT_TYPEMISMATCH);
        return 0;
      }
      // for conformance test also check illegal value zero
      if (IS_CANID_RESTRICTED((((UNSIGNED16)pData[5]) << 8) + pData[4]))
      { // zero not allowed
        MCO_SendSDOAbort(SDO_ABORT_VALUE_RANGE);
        return 0;
      }
      if (PDOType == 0)
      { // TPDO
#if NR_OF_TPDOS > 0
        /* in latest cct do not check RTR
        if ((pData[7] & 0xC0) == 0)
        { // RTR not supported
          MCO_SendSDOAbort(SDO_ABORT_VALUE_RANGE);
          return 0;
        }
        */
        if (((gTPDOConfig[PDONr].CAN.ID & COBID_DISABLED) != 0) ||
            ((gTPDOConfig[PDONr].CAN.ID & 0x07FF) == ((((UNSIGNED16)(pData[5])) << 8)+ pData[4])) ||
            ((pData[7] & 0x80) != 0)
           )
        { // Only allowed if PDO is disabled, the same, or this access disables it
          // set new CAN ID
          gTPDOConfig[PDONr].CAN.ID = pData[4] | (((UNSIGNED16)pData[5]) << 8);
          if ((pData[7] & 0x80) != 0)
          {
            gTPDOConfig[PDONr].CAN.ID |= COBID_DISABLED;
          }
          // Reset all possible TPDO trigger
#if USE_EVENT_TIME
          // This assignment split into two lines to work around certain limited C compilers
          temp16 = MCOHW_GetTime();
          gTPDOConfig[PDONr].event_timestamp = temp16;
#endif
#if USE_INHIBIT_TIME
          gTPDOConfig[PDONr].inhibit_status = INHITIM_EXPIRED;
#endif
#if USE_SYNC
          gTPDOConfig[PDONr].SYNCcnt = gTPDOConfig[PDONr].TType;
#endif
          // write completed
          MCOP_WriteConfirm();
          return 1;
        }
#endif // NR_OF_TPDOS
      }
      else
      { // RPDO 
#if NR_OF_RPDOS > 0
        if (((gRPDOConfig[PDONr].CANID & COBID_DISABLED) != 0) ||
            ((gRPDOConfig[PDONr].CANID & 0x07FF) == ((((UNSIGNED16)(pData[5])) << 8)+ pData[4])) ||
            ((pData[7] & 0x80) != 0)
           )
        { // Only allowed if PDO is disabled, or this access disables it
          // remove CAN ID filter
          MCOHW_ClearCANFilter(gRPDOConfig[PDONr].CANID);
          // Signal that RPDO filters are NOT set
          gMCOConfig.error_code &= ~0x80; 
          // set new CAN ID
          gRPDOConfig[PDONr].CANID = pData[4] | (((UNSIGNED16)pData[5]) << 8);
          if ((pData[7] & 0x80) != 0)
          {
            gRPDOConfig[PDONr].CANID |= COBID_DISABLED;
          }
          else
          {
            // set new filter
            MCOHW_SetCANFilter(gRPDOConfig[PDONr].CANID);
          }
          // write completed
          MCOP_WriteConfirm();
          return 1;
        }
#endif // NR_OF_RPDOS
      }
      MCO_SendSDOAbort(SDO_ABORT_UNSUPPORTED);
      return 0;
#endif // USE_STATIC_PDO
    } // write
  } // subindex 1
  
  // Now subindex is > 1, only allow writes if PDO is disabled
  if (cmd == 0x23)
  { // It is a write command
#if USE_STATIC_PDO == 1
     MCO_SendSDOAbort(SDO_ABORT_READONLY);
     return 0;
#else
    if (!(rdat & COBID_DISABLED))
    { // PDO is not disabled
      MCO_SendSDOAbort(SDO_ABORT_UNSUPPORTED);
      return 0;
    }
#endif // USE_STATIC_PDO 
  }
  
  // Now handle remaining subindexes
  if (pData[3] == 2) // subindex
  { // Transmission Type
    if (cmd == 0x40)
    { // Read
      if (PDOType == 0)
      { // TPDO
#if NR_OF_TPDOS > 0
        if (gTPDOConfig[PDONr].TType == 241)
        {
          reply[0] = 0;
        }
        else
        {
         reply[0] = gTPDOConfig[PDONr].TType;
        }
#endif
      }
      else
      { // RPDO
#if NR_OF_RPDOS > 0
        reply[0] = gRPDOConfig[PDONr].TType;
#endif
      }
      MCO_ReplyWith(reply,1);
      return 1;
    }

    // Write
#if USE_STATIC_PDO == 1
     MCO_SendSDOAbort(SDO_ABORT_READONLY);
     return 0;
#else

    if (len_req != 1)
    {
      MCO_SendSDOAbort(SDO_ABORT_TYPEMISMATCH);
      return 0;
    }
 #if ! USE_SYNC
    if (pData[4] <= 252)
    { // SYNC not supported
      MCO_SendSDOAbort(SDO_ABORT_VALUE_RANGE);
      return 0;
    }
 #endif // USE_SYNC

    // RTR is not supported
    if (pData[4] == 253)
    { // RTR not supported
      MCO_SendSDOAbort(SDO_ABORT_VALUE_RANGE);
      return 0;
    }

    // This code version does not support the combination of SYNC with RTR
    if (pData[4] == 252)
    { // SYNC-RTR combination not supported
      MCO_SendSDOAbort(SDO_ABORT_VALUE_RANGE);
      return 0;
    }

    if (PDOType == 0)
    { // TPDO
 #if NR_OF_TPDOS > 0
      gTPDOConfig[PDONr].TType = pData[4];
  #if USE_SYNC
      gTPDOConfig[PDONr].SYNCcnt = gTPDOConfig[PDONr].TType;
  #endif // USE_SYNC
 #endif // NR_OF_TPDOS
    }
    else
    { // RPDO
 #if NR_OF_RPDOS > 0
      gRPDOConfig[PDONr].TType = pData[4];
 #endif
    }
    MCOP_WriteConfirm();
    return 1;
#endif // USE_STATIC_PDO
  }

  if (PDOType != 0)
  { // RPDO
    // No subindex > 2 supported for RPDO
    MCO_SendSDOAbort(SDO_ABORT_UNKNOWNSUB);
    return 0;
  }
  
#if NR_OF_TPDOS > 0
  if (pData[3] == 3) // subindex
  { // Inhibit Time 
    if (cmd == 0x40)
    { // Read
#if USE_INHIBIT_TIME
      rdat = gTPDOConfig[PDONr].inhibit_time * 10;
#else
      rdat = 0;
#endif
      reply[0] = (UNSIGNED8)rdat;
      reply[1] = (UNSIGNED8)(rdat >> 8);
      MCO_ReplyWith(reply,2);
      return 1;
    }
    // Write

#if USE_STATIC_PDO == 1
     MCO_SendSDOAbort(SDO_ABORT_READONLY);
     return 0;
#else

    if (len_req != 2)
    {
      MCO_SendSDOAbort(SDO_ABORT_TYPEMISMATCH);
      return 0;
    }
#if USE_INHIBIT_TIME
    // Set new inhibit time
    // This assignment split into two lines to work around certain limited C compilers
    temp16 = (((((UNSIGNED16) pData[5]) << 8) + pData[4]) + 9)/10;
    gTPDOConfig[PDONr].inhibit_time = temp16;
    
    // Reset inhibit status
    gTPDOConfig[PDONr].inhibit_status = INHITIM_EXPIRED;
    MCOP_WriteConfirm();
    return 1;
#else
    MCO_SendSDOAbort(SDO_ABORT_TYPEMISMATCH);
    return 0;
#endif

#endif // USE_STATIC_PDO

  }

#if USE_EVENT_TIME
  if (pData[3] == 5) // subindex
  { // Event Time
    if (cmd == 0x40)
    { // Read
      rdat = gTPDOConfig[PDONr].event_time;
      reply[0] = (UNSIGNED8) rdat;
      reply[1] = (UNSIGNED8) (rdat >> 8);
      MCO_ReplyWith(reply,2);
      return 1;
    }
    // Write
#if USE_STATIC_PDO == 1
     MCO_SendSDOAbort(SDO_ABORT_READONLY);
     return 0;
#else

    if (len_req != 2)
    {
      MCO_SendSDOAbort(SDO_ABORT_TYPEMISMATCH);
      return 0;
    }
    // This assignment split into two lines to work around certain limited C compilers
    temp16 = (((UNSIGNED16) pData[5]) << 8) + pData[4];
    gTPDOConfig[PDONr].event_time = temp16;
    if (gTPDOConfig[PDONr].event_time > 0x7FFF)
    {
      gTPDOConfig[PDONr].event_time = 0x7FFF;
    }
    // This assignment split into two lines to work around certain limited C compilers
    temp16 = MCOHW_GetTime();
    gTPDOConfig[PDONr].event_timestamp = temp16;
    MCOP_WriteConfirm();
    return 1;
#endif // USE_STATIC_PDO
  }

#endif
#endif // NR_OF_TPDOS > 0
      
  MCO_SendSDOAbort(SDO_ABORT_UNKNOWNSUB);
  return 0;
}
#endif // (NR_OF_RPDOS > 0) || (NR_OF_TPDOS > 0)


#if USE_SYNC
/**************************************************************************
DOES:    Processes reception of the SYNC message
RETURNS: 0: No messages processed
         Bit 0 set: SYNC TPDOs transmitted
         Bit 1 set: SYNC RPDOs received
**************************************************************************/
UNSIGNED8 MCOP_HandleSYNC (
  void
  )
{
UNSIGNED16 PDONr;
UNSIGNED8 retstat;
#if USECB_ODDATARECEIVED
UNSIGNED16 map; // offset into SDOResponseTable, RPDO mapping 
UNSIGNED16 off; // offset into Process Image to RPDO data
MEM_CONST UNSIGNED8 *pSDO; // Pointer into SDOResponseTable
UNSIGNED8 cnt;
#endif // USECB_ODDATARECEIVED
  
#if USECB_SYNCRECEIVE
  MCOUSER_SYNCReceived();
#endif
#if (NR_OF_RPDOS > 0) || (NR_OF_TPDOS > 0)
  if (MY_NMT_STATE != 5)
  { // node is not in operational state
    return 0;
  }
  retstat = 0;

#if (NR_OF_TPDOS > 0)
  for (PDONr = 0; PDONr < gMCOConfig.nrTPDOs; PDONr++)
  {
    if ((gTPDOConfig[PDONr].CAN.ID & COBID_DISABLED) == 0)
    { // this TPDO is used, 241 marks special case, first call since switch to operational of type 0
      if ((gTPDOConfig[PDONr].TType == 0)|| (gTPDOConfig[PDONr].TType == 241))
      { // Combination COS and SYNC
        // has application data changed?
        if ((PDO_TXCOMP(PDONr,&(gTPDOConfig[PDONr].CAN.BUF[0])) != 0) || (gTPDOConfig[PDONr].TType == 241))
        { // ensure type is back to zero, 241 only used on first call
          gTPDOConfig[PDONr].TType = 0;
#if USECB_TPDORDY
          if (MCOUSER_TPDOReady(gTPDOConfig[PDONr].PDONr,2))
          {
#endif
            // Copy application data
            PDO_TXCOPY(PDONr,&(gTPDOConfig[PDONr].CAN.BUF[0]));
            // transmit now
            MCO_TransmitPDO(PDONr);
            retstat |= 1;
#if USECB_TPDORDY
          }
#endif
        }
      }
      if ((gTPDOConfig[PDONr].TType >= 1) &&
          (gTPDOConfig[PDONr].TType <= 240)
         )
      { // This PDO is synced
        gTPDOConfig[PDONr].SYNCcnt--;
        if (gTPDOConfig[PDONr].SYNCcnt == 0)
        { // SYNC counter reached zero, reset counter and transmit PDO
#if USECB_TPDORDY
          if (MCOUSER_TPDOReady(gTPDOConfig[PDONr].PDONr,1))
          {
#endif
            // Copy application data
            PDO_TXCOPY(PDONr,&(gTPDOConfig[PDONr].CAN.BUF[0]));
            // transmit now
            MCO_TransmitPDO(PDONr);
            retstat |= 1;
#if USECB_TPDORDY
          }
#endif
        }
      }
    }
  }
#endif // NR_OF_TPDOS

#if (NR_OF_RPDOS > 0)
  for (PDONr = 0; PDONr < gMCOConfig.nrRPDOs; PDONr++)
  {
    if ((gRPDOConfig[PDONr].CANID & COBID_DISABLED) == 0)
    { // this RPDO is used
      if (gRPDOConfig[PDONr].TType <= 240)
      { // This RPDO is synced
#if USECB_ODDATARECEIVED
          // Process RPDO mapping
 #if USE_DYNAMIC_PDO_MAPPING
 #error USECB_ODDATARECEIVED currently not available with USE_DYNAMIC_PDO_MAPPING
 #else
          map = gRPDOConfig[PDONr].map; // offset to mapping entries
          off = gRPDOConfig[PDONr].offset; // offset to data in process image
          pSDO = OD_SDOResponseTablePtr(0);
          cnt = 1;
          while((pSDO[map+3] == cnt) && (cnt <= 8))
          { // while Subindex is not zero
            RTOS_LOCK_PI(PIACC_APP,PISECT_PDO);
            MCOUSER_ODData((((UNSIGNED16)(pSDO[map+7]))<<8)+pSDO[map+6],pSDO[map+5],&(gRPDOConfig[PDONr].BUF[4]),pSDO[map+4]>>3);
            RTOS_UNLOCK_PI(PIACC_APP,PISECT_PDO);
            off += (pSDO[map+4]>>3); // next mapped OD entry
            map += 8; // next mapping entry
            cnt++;
          }
#endif // USE_DYNAMIC_PDO_MAPPING
#endif // USECB_ODDATARECEIVED

          // copy data from RPDO to process image
          PDO_RXCOPY(PDONr,&(gRPDOConfig[PDONr].BUF[0]));

#if USECB_RPDORECEIVE
          MCOUSER_RPDOReceived(gRPDOConfig[PDONr].PDONr,gRPDOConfig[PDONr].offset,gRPDOConfig[PDONr].len);
#endif // USECB_RPDORECEIVE
        retstat |= 2;
      }
    }
  }
#endif // NR_OF_RPDOS

  return retstat;
#else   // (NR_OF_RPDOS > 0) || (NR_OF_TPDOS > 0)
  return 0;
#endif  // (NR_OF_RPDOS > 0) || (NR_OF_TPDOS > 0)
}
#endif // USE_SYNC


#if USE_NODE_GUARDING
/**************************************************************************
DOES:    Checks if message received is guarding request
RETURNS: 0: message received is not guarding request
         1: guarding request received, response sent
**************************************************************************/
UNSIGNED8 MCOP_HandleGuarding (
  UNSIGNED16 can_id
  )
{
  if (can_id == (UNSIGNED16) 0x0700 + MY_NODE_ID)
  { // ID matches, so probably a request
    // transmit response / heartbeat message
    // Merge toggle bit into response
    MY_NMT_STATE += gMCOConfig.NGtoggle;
    if (!MCOHW_PushMessage(&gMCOConfig.heartbeat_msg))
    {
      MCOUSER_FatalError(ERROFL_HBT);
    }
    // Remove toggle bit again
    MY_NMT_STATE &= 0x7F;
    if (gMCOConfig.NGtoggle == 0)
    {
      gMCOConfig.NGtoggle = 0x80;
    }
    else
    {
      gMCOConfig.NGtoggle = 0;
    }
    return 1;
  }
  return 0;
}
#endif


#if ! MGR_MONITOR_ALL_NODES
#if (NR_OF_HB_CONSUMER > 0)
/**************************************************************************
DOES:    Checks if a node ID is already used, needed for conformance test
RETURNS: TRUE, if node_id is already used
**************************************************************************/
UNSIGNED8 MCOP_IsHBMonitored (
  UNSIGNED8 channel,
  UNSIGNED8 node_id
  )
{
UNSIGNED8 loop;
UNSIGNED8 retval = FALSE;

  if (!(gHBCons[--channel].can_id == 0x700 + (UNSIGNED16) node_id))
  { // the current channel already uses this node ID, so OK to modify

    loop = NR_OF_HB_CONSUMER;  
    while (loop > 0)
    {
      loop--;
      if ((UNSIGNED16) node_id + 0x700 == gHBCons[loop].can_id)
      {
        retval = TRUE;
        break;
      }
    }
  }
  return retval;
}


/**************************************************************************
DOES:    Initializes Heartbeat Consumer
GLOBALS: Inits gHBCons[consumer_channel-1]
**************************************************************************/
void MCOP_InitHBConsumer (
  UNSIGNED8 consumer_channel, // HB Consumer channel
  UNSIGNED8 node_id, // Node ID to monitor, 0 to disable monitor
  UNSIGNED16 hb_time // Timeout ot use (in ms)
  )
{
#if CHECK_PARAMETERS
  // check ranges
  if (((node_id > 127)) ||
      ((consumer_channel == 0) || (consumer_channel > NR_OF_HB_CONSUMER))
     )
  {
    MCOUSER_FatalError(0x9901);
  }
#endif

  consumer_channel--; // adapt to range 0 to NR_OF_HB_CONSUMER-1

  if ((node_id == 0) || (hb_time == 0))
  { // disable
    gHBCons[consumer_channel].status = HBCONS_OFF;
    gHBCons[consumer_channel].time = 0;
    gHBCons[consumer_channel].can_id = 0;
  }
  else
  { // enable
    if (hb_time >= 0x8000)
    { // maximum time supported by MCOP
      hb_time = 0x7FFF;
    }
    gHBCons[consumer_channel].time = hb_time;
    gHBCons[consumer_channel].can_id = 0x700 + node_id;
    gHBCons[consumer_channel].status = HBCONS_INIT;

    if (!MCOHW_SetCANFilter(gHBCons[consumer_channel].can_id))
    {
      MCOUSER_FatalError(0x9902);
    }
  }

}

/**************************************************************************
DOES:    Checks if a message received contains a heartbeat to be consumed
GLOBALS: Updates gHBCons[consumer_channel-1]
RETURNS: one, if message received was a heartbeat monitored, else zero
**************************************************************************/
UNSIGNED8 MCOP_ConsumeHB (
  CAN_MSG *pRxCAN // CAN message received
  )
{
UNSIGNED8 loop;
UNSIGNED8 retval = FALSE;

  for (loop = 0; loop < NR_OF_HB_CONSUMER; loop++)
  {
    if ( (gHBCons[loop].status != HBCONS_OFF) && // consumer is not disabled
         (pRxCAN->ID == gHBCons[loop].can_id) // CAN ID matches
       )
    { // Match found
      /* conformance requires that boot up msg is also considered
      if (pRxCAN->BUF[0] != 0)
      { // This is not the bootup message
      */
      gHBCons[loop].status = HBCONS_ACTIVE; // activate consumption
      // calculate expiration timestamp
      gHBCons[loop].timestamp = MCOHW_GetTime() + gHBCons[loop].time;
#if USE_LEDS
        gMCOConfig.LEDErr = LED_OFF; // clear previous error indication
#endif
      /* } */
      retval = TRUE;
      break;
    }
  }
  return retval;
}


/**************************************************************************
DOES:    Checks if a heartbeat consumer timeout occured
RETURNS:
          HBCONS_OFF,    // Disabled
          HBCONS_INIT,   // Initialized, waiting for first 2 heartbeats
          HBCONS_ACTIVE, // Consumer active and OK
          HBCONS_LOST    // Heartbeat lost
**************************************************************************/
HBCONS_STATE MCOP_ProcessHBCheck (
  UNSIGNED8 consumer_channel // Range 1 to NR_OF_HB_CONSUMER
  )
{

#if CHECK_PARAMETERS
  // check ranges
  if ((consumer_channel == 0) || (consumer_channel > NR_OF_HB_CONSUMER))
  {
    MCOUSER_FatalError(0x9903);
  }
#endif

  consumer_channel--; // adapt to range 0 to NR_OF_HB_CONSUMER-1

  if (gHBCons[consumer_channel].status == HBCONS_ACTIVE)
  { // Heartbeat consumer is active
    if (MCOHW_IsTimeExpired(gHBCons[consumer_channel].timestamp))
    { // active and expired
#if USE_EMCY
      MCOP_PushEMCY(0x8130,(UNSIGNED8)gHBCons[consumer_channel].can_id,0,0,0,0);
#endif // USE_EMCY
#if USE_LEDS
      gMCOConfig.LEDErr = LED_FLASH2;
#endif
      MCOUSER_HeartbeatLost((UNSIGNED8)gHBCons[consumer_channel].can_id);
      // set new state
      MY_NMT_STATE = NMTSTATE_PREOP;
#if USECB_NMTCHANGE
      // Call back to user / application
      MCOUSER_NMTChange(MY_NMT_STATE);
#endif
      // restart HB consumer
      gHBCons[consumer_channel].status = HBCONS_INIT;
    }
  }
  return gHBCons[consumer_channel].status;
}
#endif // (NR_OF_HB_CONSUMER > 0)
#endif // MGR_MONITOR_ALL_NODES



#if USE_SLEEP
/**************************************************************************
Description in mco.h
***************************************************************************/ 
UNSIGNED8 MCOP_TransmitWakeupSleep ( 
  UNSIGNED8 statcmd,
  UNSIGNED8 reason
  )
{
CAN_MSG TxMSG;

  if ((MY_NODE_ID > 0) && (MY_NODE_ID <= 16))
  { // Do we have a node ID?
    TxMSG.ID = 0x690+MY_NODE_ID;
    TxMSG.BUF[0] = statcmd;
    TxMSG.BUF[1] = reason;
  }
  else
  { // use generic ID and wakeup only
    TxMSG.ID = 0x690;
    TxMSG.BUF[0] = SLEEP_WAKEUP;
    TxMSG.BUF[1] = SLEEP_REASON_NONE;
  }
  TxMSG.LEN = 8;
  TxMSG.BUF[2] = 0;
  TxMSG.BUF[3] = 0;
  TxMSG.BUF[4] = 0;
  TxMSG.BUF[5] = 0;
  TxMSG.BUF[6] = 0;
  TxMSG.BUF[7] = 0;
  return MCOHW_PushMessage(&TxMSG);
}

#endif // USE_SLEEP

/*----------------------- END OF FILE ----------------------------------*/
