/**************************************************************************
MODULE:    EXTENDED_OD - Here: Support of Custom Object Dictionary Access
CONTAINS:  MicroCANopen Custom Object Dictionary Access
COPYRIGHT: Embedded Systems Academy, Inc. 2002-2016
           All rights reserved. www.microcanopen.com
DISCLAIM:  Read and understand our disclaimer before using this code!
           www.esacademy.com/disclaim.htm
           This software was written in accordance to the guidelines at
           www.esacademy.com/software/softwarestyleguide.pdf
LICENSE:   THIS IS THE COMMERCIAL VERSION OF MICROCANOPEN PLUS
           ONLY USERS WHO PURCHASED A LICENSE MAY USE THIS SOFTWARE
           See file license_commercial_plus.txt or
           www.microcanopen.com/license_commercial_plus.txt
VERSION:   6.21, ESA 16-JUN-16
           $LastChangedDate: 2016-02-16 22:14:13 +0100 (Tue, 16 Feb 2016) $
           $LastChangedRevision: 3568 $
***************************************************************************/ 

#include "mcop_xod_inc.h"
#include "entriesandreplies.h"

#ifdef __SIMULATION__
#include <windows.h>
#include "mcohwpcsim.h"
#include "simnodehandler.h"
#endif

#if CAN_ID_SIZE != 32
#error XOD only supports 32-bit CAN IDs as all IDs from binary EDS/DCF are 32-bit
#endif

// table with SDO Responses for read requests to OD - defined in user_xxx.c
extern UNSIGNED8 MEM_CONST gSDOResponseTable[];

// table with Object Dictionary entries to process Data - defined in user_xxx.c
extern OD_PROCESS_DATA_ENTRY MEM_CONST gODProcTable[];

#if USE_EXTENDED_SDO
// table with generic OD entries
extern OD_GENERIC_DATA_ENTRY MEM_CONST gODGenericTable[];
#endif


#if (USE_XOD_ACCESS == 1)
/**************************************************************************
MODULE VARIABLES, CUSTOM OD ACCESS MANAGEMENT
***************************************************************************/ 

OD_CONFIG gOD = {0,0
#if USE_EXTENDED_SDO
         ,0
#endif
         ,0,0,0,0,0};
#endif


/**************************************************************************
LOCAL FUNCTIONS
***************************************************************************/ 

/**************************************************************************
DOES:     Get a 16/32bit value from binary eds file, regardless of 
          alignment, always stored in little endian format
RETURNS:  16/32 bit value stored at location
***************************************************************************/
static UNSIGNED16 Get16 (UNSIGNED8 *ptr)
{
UNSIGNED16 ret_val;
  
  ptr++;
  ret_val = *ptr--;
  ret_val <<= 8;
  ret_val += *ptr;
  return ret_val;
}

#if (USE_XOD_ACCESS == 1)
static UNSIGNED32 Get32 (UNSIGNED8 *ptr)
{
UNSIGNED32 ret_val;
  
  ptr += 2;
  ret_val = Get16(ptr);
  ret_val <<= 16;
  ptr -= 2;
  ret_val += Get16(ptr);
  return ret_val;
}
#endif 

/**************************************************************************
GLOBAL FUNCTIONS
***************************************************************************/ 

/**************************************************************************
DOES:    Checks if for a certain index and subindex a data entry exists
         in the Object Dictionary, does NOT check 1xxxh entries
RETURNS: TRUE, if entry was found, then pDat contains pointer to data
         and pLen the length of the data
**************************************************************************/
UNSIGNED8 OD_FindODDataEntry (
  UNSIGNED8 mode,   // Bit 0 set: Search Process Image (up to 4 byte data)
                    // Bit 1 set: Search Generic Data (any size, any location)
                    // Bit 2 set: Search Constant table (up to 4 byte data)
  UNSIGNED16 idx,   // Index of Object Dictionary entry to find
  UNSIGNED8 sub,    // Subindex of Object Dictionary entry to find
  UNSIGNED32 *pLen, // When found, contains length of entry
  UNSIGNED8 **pDat  // When found, contains pointer to data of entry
  )
{
  UNSIGNED16 found;
  // pointer to an entry in gODProcTable
  OD_PROCESS_DATA_ENTRY MEM_CONST *pOD;
  // pointer to const OD entry
  UNSIGNED8 MEM_CONST *pDatConst;
  // return value required for search of generic table, here unused
  UNSIGNED8 acc;
  // return value: pointer to const OD entry
  static UNSIGNED8 mBufDat;
  // return value: length of entry
  static UNSIGNED32 mBufLen;

  if (mode & 0x01)
  { // Data available in process image?
    found = MCO_SearchODProcTable(idx,sub);
    // entry found?
    if (found != 0xFFFF)
    {
      // initialize pointer
      pOD = OD_ProcTablePtr(0);
      pOD += (UNSIGNED32)(found);
      *pDat = &(gProcImg[Get16((UNSIGNED8 *)&(pOD->off_lo))]);
      *pLen = (UNSIGNED32) (pOD->len & 0x0F);
      return TRUE;
    }
  }

  if (mode & 0x02)
  { // Data available in generic data table?
    found = XSDO_SearchODGenTable(idx,sub,&acc,pLen,pDat);
    // entry found?
    if (found != 0xFF)
    {
      return TRUE;
    }
  }

  if (mode & 0x04)
  { // Data available in SDO response table?
    found = MCO_SearchOD(idx,sub);
    // entry found?
    if (found != 0xFFFF)
    {
      pDatConst = OD_SDOResponseTablePtr(0);
      pDatConst += (found * 8); // point to SDOREPLY entry
      // use buffer to return value
      mBufLen = 4 - ((*pDatConst >> 2) & 0x03);
      pDatConst += 4; // point to data
      mBufDat = *pDatConst;
      // return the buffers
      *pDat = (UNSIGNED8 *) &mBufDat;
      *pLen = mBufLen;
      return TRUE;
    }
  }

  // not found
  return FALSE;
}


/**************************************************************************
DOES:    Find the Process Image offset to an OD entry.
         When USE_XOD_ACCESS is set, use this to find an offset before
         using the PI_READ or PI_WRITE macros.
RETURNS: 0xFFFF if not found, else the offset
**************************************************************************/
UNSIGNED16 OD_GetPIEntryOffset(
  UNSIGNED16 index,
  UNSIGNED8 subindex
  )
{
UNSIGNED32 offset = 0xFFFFFFFFul;
static UNSIGNED32 len;
static UNSIGNED8 *pdat;

  if (OD_FindODDataEntry(3,index,subindex,&len,&pdat))
  { // entry found, now check if result is in process image
    offset = (intptr_t) pdat; // pointer into process image
    offset -= (intptr_t) (&(gProcImg[0])); // deduct start of proces simage to calculate offset
    if (offset >= PROCIMG_SIZE)
    { // not in allowed range
      offset = 0xFFFFFFFFul;
    }
  }
  return (UNSIGNED16) offset;
}


#if (USE_XOD_ACCESS == 1)
/**************************************************************************
DOES:     Retrieves CAN bps and node ID values from configuration file
NOTE:     At pointer location, a binary EDS file as generated by 
          CANopen Architect is expected.
RETURNS:  TRUE if parameters could be retrieved, else FALSE
***************************************************************************/
UNSIGNED8 OD_GetLayerSettings(
  UNSIGNED8 *pCfg, // pointer to Object Dictionary tables
  UNSIGNED8 *pnode_id,
  UNSIGNED16 *pcan_bps
  )
{
UNSIGNED8 ret_val = FALSE;
  
  // invalid pointer means we can't load configuration
  if (pCfg == NULL) return FALSE;

  if ( (Get16(&(pCfg[0])) == 2) && // Version match
       (pCfg[4] == 'P') && (pCfg[5] == 'O') && (pCfg[6] == 'C') && (pCfg[7] == 'M') && // ID match
       (gOD.nr_rpdo <= NR_OF_RPDOS) && (gOD.nr_tpdo <= NR_OF_TPDOS) && (gOD.pimg_size <= PROCIMG_SIZE) // enough resources?
     )
  { // Version match and file ID match and memory big enough
    *pcan_bps = Get16(&(pCfg[12]));
    *pnode_id = pCfg[14];
    ret_val = TRUE;
  }
  return ret_val;
}


/**************************************************************************
DOES:     SWITCHES OVER TO A DIFFERENT OBJECT DICTIONARY
NOTE:     At pointer location, a binary EDS file as generated by 
          CANopen Architect is expected.
RETURNS:  TRUE if switch was completed, else FALSE
***************************************************************************/
UNSIGNED8 OD_SwitchObjectDictionary(
  UNSIGNED8 *pCfg // pointer to Object Dictionary tables
  )
{
UNSIGNED8 ret_val = FALSE;
static UNSIGNED8 *pOD;
static UNSIGNED8 lp;

  // invalid pointer means we can't load configuration
  if (pCfg == NULL) return FALSE;

  gOD.nr_rpdo = Get16(&(pCfg[16]));
  gOD.nr_tpdo = Get16(&(pCfg[18]));
  gOD.pimg_size = Get32(&pCfg[20]);
#ifdef __SIMULATION__
  SimDriver_printf("Loading Object Dictionary: %d bytes ProcessImage, %d RPDOs, %d TPDOs\n", gOD.pimg_size, gOD.nr_rpdo, gOD.nr_tpdo);
#endif

  if ( (Get16(&(pCfg[0])) == 2) && // Version match
       (pCfg[4] == 'P') && (pCfg[5] == 'O') && (pCfg[6] == 'C') && (pCfg[7] == 'M') && // ID match
       (gOD.nr_rpdo <= NR_OF_RPDOS) && (gOD.nr_tpdo <= NR_OF_TPDOS) && (gOD.pimg_size <= PROCIMG_SIZE) // enough resources?
     )
  { // Version match and file ID match and memory big enough
    
    // get beginning of SDO response table
    pOD = pCfg; 
    pOD += Get32(&(pCfg[128])); // add offset from first table entry
    gOD.p_SDOResponseBase = pOD;
    
    // get beginning of OD entry table
    pOD = pCfg; 
    pOD += Get32(&(pCfg[128+4])); // add offset from second table entry
    gOD.p_ProcBase = (OD_PROCESS_DATA_ENTRY *) pOD;

    // get beginning of generic entry table
    pOD = pCfg; 
    pOD += Get32(&(pCfg[128+8])); // add offset from third table entry
#if USE_EXTENDED_SDO
    gOD.p_GenericBase = (OD_GENERIC_DATA_ENTRY *) pOD;
#endif
    
    // get beginning of process image defaults
    pOD = pCfg; 
    pOD += Get32(&(pCfg[128+12])); // add offset from 4th table entry
    gOD.p_PIDefaults = pOD;
    // copy them over
    MEM_CPY(&(gProcImg),pOD,gOD.pimg_size);

#if NR_OF_RPDOS > 0
    pOD = pCfg; 
    pOD += Get32(&(pCfg[128+(6*4)])); // add offset from 7th table entry
    // configure all RPDOs
    lp = 0;
    while (lp < NR_OF_RPDOS)
    {
      if (lp < gOD.nr_rpdo)
      {
        gRPDOConfig[lp].PDONr = *pOD++;
        gRPDOConfig[lp].TType = *pOD++;
        gRPDOConfig[lp].len = *pOD++;
        pOD++;
        gRPDOConfig[lp].CANID = Get32(pOD);
        pOD += 4;
        gRPDOConfig[lp].offset = Get32(pOD);
        pOD += 4;


#ifdef __SIMULATION__
        if (!(gRPDOConfig[lp].CANID & COBID_DISABLED))
        SimDriver_printf("  RPDO%d, TType=%d, Len=%d, ID=%8.8lXh, Offset=%8.8lXh\n", gRPDOConfig[lp].PDONr, gRPDOConfig[lp].TType, gRPDOConfig[lp].len, gRPDOConfig[lp].CANID, gRPDOConfig[lp].offset);
#endif
      }
      else
      { // unused, disable RPDO
        gRPDOConfig[lp].PDONr = 0;
        gRPDOConfig[lp].CANID = COBID_DISABLED;
      }
      lp++;
    }
    gMCOConfig.nrRPDOs = (UNSIGNED8)gOD.nr_rpdo;
#endif
    
#if NR_OF_TPDOS > 0
    // configure all TPDOs
    pOD = pCfg; 
    pOD += Get32(&(pCfg[128+(7*4)])); // add offset from 8th table entry
    lp = 0;
    while (lp < NR_OF_TPDOS)
    {
      if (lp < gOD.nr_tpdo)
      {
        gTPDOConfig[lp].PDONr = *pOD++;
        gTPDOConfig[lp].TType = *pOD++;
        gTPDOConfig[lp].CAN.LEN = *pOD++;
        pOD++;
        gTPDOConfig[lp].CAN.ID = Get32(pOD);
        pOD += 4;
        gTPDOConfig[lp].offset = Get32(pOD);
        pOD += 4;
        gTPDOConfig[lp].event_time = Get16(pOD);
        pOD += 2;
        gTPDOConfig[lp].inhibit_time = Get16(pOD);
        pOD += 2;
#ifdef __SIMULATION__
        if (!(gTPDOConfig[lp].CAN.ID & COBID_DISABLED))
          SimDriver_printf("  TPDO%d, TType=%d, Len=%d, ID=%8.8lXh, Event=%d, Inhibit=%d, Offset=%8.8lXh\n", gTPDOConfig[lp].PDONr, gTPDOConfig[lp].TType, gTPDOConfig[lp].CAN.LEN, gTPDOConfig[lp].CAN.ID, gTPDOConfig[lp].event_time, gTPDOConfig[lp].inhibit_time,gTPDOConfig[lp].offset);
#endif
      }
      else
      {
        gTPDOConfig[lp].PDONr = 0;
        gTPDOConfig[lp].CAN.ID = COBID_DISABLED;
      }
      lp++;
    }
    gMCOConfig.nrTPDOs = (UNSIGNED8)gOD.nr_tpdo;
#endif

    ret_val = TRUE;

#ifdef __SIMULATION__
    // tell simulation system of od switch
    SimDriver_ODSwitched();
#endif
  }
  return ret_val;
}


/**************************************************************************
DOES:     ACCESS TO THE OBJECT DICTIONARY DATA
          HERE: SDO response table (8 bytes per entry)
RETURNS:  Pointer to requested entry
***************************************************************************/
UNSIGNED8 *OD_SDOResponseTablePtr_fct(UNSIGNED16 offset)
{
UNSIGNED8 *p_ret;

  if (gOD.p_SDOResponseBase != 0)
  {
    p_ret = (UNSIGNED8 *) (&(gOD.p_SDOResponseBase[offset]));
  }
  else
  {
    p_ret = (UNSIGNED8 *) (&(gSDOResponseTable[offset]));
  }
  return p_ret;
}


/**************************************************************************
DOES:     ACCESS TO THE OBJECT DICTIONARY DATA
          HERE: OD data in process image
RETURNS:  Pointer to requested entry
***************************************************************************/
OD_PROCESS_DATA_ENTRY *OD_ProcTablePtr_fct(UNSIGNED16 record)
{
OD_PROCESS_DATA_ENTRY *p_ret;

  if (gOD.p_ProcBase != 0)
  {
    p_ret = (OD_PROCESS_DATA_ENTRY *) (&(gOD.p_ProcBase[record]));
  }
  else
  {
    p_ret = (OD_PROCESS_DATA_ENTRY *) (&(gODProcTable[record]));
  }
  return p_ret;
}


#if USE_EXTENDED_SDO
/**************************************************************************
DOES:     ACCESS TO THE OBJECT DICTIONARY DATA
          HERE: Generic entries
RETURNS:  Pointer to requested entry
***************************************************************************/
OD_GENERIC_DATA_ENTRY *OD_GenericTablePtr_fct(UNSIGNED8 record)
{
OD_GENERIC_DATA_ENTRY *p_ret;

  if (gOD.p_GenericBase != 0)
  {
    p_ret = (OD_GENERIC_DATA_ENTRY *) (&(gOD.p_GenericBase[record]));
  }
  else
  {
    p_ret = (OD_GENERIC_DATA_ENTRY *) (&(gODGenericTable[record]));
  }
  return p_ret;
}
#endif


#endif // (USE_XOD_ACCESS == 1)

/*----------------------- END OF FILE ----------------------------------*/
