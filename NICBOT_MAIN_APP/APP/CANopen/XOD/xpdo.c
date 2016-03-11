/**************************************************************************
MODULE:    XPDO
CONTAINS:  MicroCANopen Plus implementation of dynmaic PDO mapping
COPYRIGHT: Embedded Systems Academy, Inc. 2002-2016
           All rights reserved. www.microcanopen.com
DISCLAIM:  Read and understand our disclaimer before using this code!
           www.esacademy.com/disclaim.htm
           This software was written in accordance to the guidelines at
           www.esacademy.com/software/softwarestyleguide.pdf
LICENSE:   THIS IS THE COMMERCIAL PLUS VERSION OF MICROCANOPEN
           ONLY USERS WHO PURCHASED A LICENSE MAY USE THIS SOFTWARE
           See file license_commercial_plus.txt
VERSION:   6.21, ESA 16-JUN-16
           $LastChangedDate: 2016-02-16 22:14:13 +0100 (Tue, 16 Feb 2016) $
           $LastChangedRevision: 3568 $
***************************************************************************/ 

#include "mcop_xod_inc.h"


#if (USE_DYNAMIC_PDO_MAPPING == 1)

#define ERRFT_XPDO_FINDPDO 8501 // Fatal Error, PDO not found

// this structure holds the CAN message for SDO responses or aborts
extern CAN_MSG MEM_FAR gTxSDO;

// Internal MCO from mco.c, mcop.c
void MCOP_WriteConfirm (void);


/**************************************************************************
GLOBAL VARIABLES
**************************************************************************/

#if NR_OF_TPDOS > 0
// TPDO Mapping
UNSIGNED16 MEM_FAR gTPDOmap[NR_OF_TPDOS*9];

// Direct pointer access for TPDO mapping
UNSIGNED16 MEM_FAR gTPDODataPtr[NR_OF_TPDOS*8];
#endif

#if NR_OF_RPDOS > 0
// RPDO Mapping
UNSIGNED16 MEM_FAR gRPDOmap[NR_OF_RPDOS*9];

// Direct pointer access for RPDO mapping
UNSIGNED16 MEM_FAR gRPDODataPtr[NR_OF_RPDOS*8];
#endif


/**************************************************************************
MODULE VARIABLES
**************************************************************************/

// Pointer into gODProcTable[]
static OD_PROCESS_DATA_ENTRY MEM_CONST *mpODProc;

// State variables needed for tracking conformance test conditions
static UNSIGNED8 MEM_FAR mCnt = 0;
static UNSIGNED8 MEM_FAR mLen = 0;
static UNSIGNED32 MEM_FAR mAbort = 0;


/**************************************************************************
 LOCAL FUNCTIONS
***************************************************************************/

/**************************************************************************
DOES:    Gets a byte from the current record in gODProcTable
GLOBALS: mpODProc - Uses the current record 
RETURNS: The requested byte
**************************************************************************/
static UNSIGNED8 XSDO_ReadODProc (
  UNSIGNED8 offset
  )
{
UNSIGNED8 *pB;

  pB = (UNSIGNED8 *) mpODProc;
  pB += offset;
  return *pB;
}


/**************************************************************************
DOES:     For a given PDO Number, finds the matching record index
RETURNS:  Record number, if found, otherwise 0xFFFF
***************************************************************************/
static UNSIGNED16 XPDO_FindPDORecord (
  UNSIGNED8 TxRx, // Set to 0 for TPDO, 1 for RPDO
  UNSIGNED16 PDONr // Number of PDO, 1 to 512
  )
{
UNSIGNED16 found_rec;

#if CHECK_PARAMETERS
  // check ranges
  if ( (TxRx > 1) || (PDONr == 0) || (PDONr > 512) )
  {
    MCOUSER_FatalError(ERRFT_XPDO_FINDPDO);
  }
#endif

  found_rec = 0;

  if (TxRx == 0)
  { // TPDO
#if NR_OF_TPDOS > 0
    // Make quick guess, check for linear PDO numbering
    if ( (PDONr < NR_OF_TPDOS) && 
         (gTPDOConfig[PDONr-1].PDONr == PDONr) 
       )
    { // found the TPDO
      return PDONr-1;
    }
    while (gTPDOConfig[found_rec].PDONr != PDONr)
    {
      found_rec++;
      if (found_rec >= gMCOConfig.nrTPDOs)
      { // not found!
        return 0xFFFF;
      }
    }
    return found_rec; // record found
#else
    // No TPDOS available
    return 0xFFFF;
#endif
  }
  else
  { // RPDO
#if NR_OF_RPDOS > 0
    // Make quick guess, check for linear PDO numbering
    if ( (PDONr < NR_OF_RPDOS) && 
         (gRPDOConfig[PDONr-1].PDONr == PDONr) 
       )
    { // found the TPDO
      return PDONr-1;
    }
    while (gRPDOConfig[found_rec].PDONr != PDONr)
    {
      found_rec++;
      if (found_rec >= gMCOConfig.nrRPDOs)
      { // not found!
        return 0xFFFF;
      }
    }
    return found_rec; // record found
#else
    // No RPDOS available
    return 0xFFFF;
#endif
  }
}


/**************************************************************************
 PUBLIC FUNCTIONS
***************************************************************************/

#if NR_OF_RPDOS > 0
/**************************************************************************
DOES:    Receive RPDO Data and copy to destination, depending on mapping
NOTE:    Must be used instead of the standard PDO_RXCOPY macro if the
         dynamic PDO option is enabled.
         Might require extra locking in RTOS environment.
***************************************************************************/ 
UNSIGNED8 PDO_RXCOPY_fct ( 
  UNSIGNED16 RPDONr, // PDO number, as index from 0 to NR_OF_xPDOS-1
  UNSIGNED8 *pSrc // Pointer to data received
  )
{
UNSIGNED8 b; // Count number of bytes copied
UNSIGNED16 MEM_FAR *pPDODataPtrEntry; // Pointer to a record in PDODataPtr array

  pPDODataPtrEntry = &(gRPDODataPtr[RPDONr*8]);
  b = 0;
  RTOS_LOCK_PI(PIACC_PDO,PISECT_PDO);
  while ((b < gRPDOConfig[RPDONr].len) && (*pPDODataPtrEntry != 0xFFFF)) // End of PDO reached
  {
    gProcImg[*pPDODataPtrEntry] = *pSrc;
    pSrc++;
    pPDODataPtrEntry++;
    b++;
  }
  RTOS_UNLOCK_PI(PIACC_PDO,PISECT_PDO);
  return b;
}
#endif


#if NR_OF_TPDOS > 0
/**************************************************************************
DOES:    Get TPDO Data and copy to CAN message, depending on mapping
NOTE:    Must be used instead of the standard PDO_TXCOPY macro if the
         dynamic PDO option is enabled.
         Might require extra locking in RTOS environment.
***************************************************************************/ 
UNSIGNED8 PDO_TXCOPY_fct ( 
  UNSIGNED16 TPDONr, //  PDO number, as index from 0 to NR_OF_xPDOS-1
  UNSIGNED8 *pDest // Pointer to transmission buffer
  )
{
UNSIGNED8 b; // Count number of bytes copied
UNSIGNED16 MEM_FAR *pPDODataPtrEntry; // Pointer to a record in PDODataPtr array

  pPDODataPtrEntry = &(gTPDODataPtr[TPDONr*8]);
  b = 0;
  RTOS_LOCK_PI(PIACC_PDO,PISECT_PDO);
  while ((b < gTPDOConfig[TPDONr].CAN.LEN) && (*pPDODataPtrEntry != 0xFFFF)) // End of PDO reached
  {
    *pDest = gProcImg[*pPDODataPtrEntry];
    pDest++;
    pPDODataPtrEntry++;
    b++;
  }
  RTOS_UNLOCK_PI(PIACC_PDO,PISECT_PDO);
  return b;
}


/**************************************************************************
DOES:    Get TPDO Data and compare, depending on mapping
         used for change-of-state detection
NOTE:    Must be used instead of the standard PDO_TXCOMP macro if the
         dynamic PDO option is enabled.
         Might require extra locking in RTOS environment.
***************************************************************************/ 
UNSIGNED8 PDO_TXCOMP_fct ( 
  UNSIGNED16 TPDONr, //  PDO number, as index from 0 to NR_OF_xPDOS-1
  UNSIGNED8 *pDest // Pointer to transmission buffer
  )
{
UNSIGNED8 b; // Count number of bytes copied
UNSIGNED16 MEM_FAR *pPDODataPtrEntry; // Pointer to a record in PDODataPtr array

  pPDODataPtrEntry = &(gTPDODataPtr[TPDONr*8]);
  b = 0;
  RTOS_LOCK_PI(PIACC_PDO,PISECT_PDO);
  while ((b < gTPDOConfig[TPDONr].CAN.LEN) && (*pPDODataPtrEntry != 0xFFFF)) // End of PDO reached
  {
    if (*pDest != gProcImg[*pPDODataPtrEntry])
    {
      return b+1;
    }
    pDest++;
    pPDODataPtrEntry++;
    b++;
  }
  RTOS_UNLOCK_PI(PIACC_PDO,PISECT_PDO);
  return 0;
}
#endif


/**************************************************************************
DOES: INITIALIZE A SINGLE RPDO OR TPDO MAPPING
Ensures that all PDO communication and mapping parameters are usable and
initializes the global variables used by RPDOs and TPDOs
***************************************************************************/
void XPDO_UpdatePDOMapping (
  UNSIGNED8 TxRx, // Set to 0 for TPDO, 1 for RPDO
  UNSIGNED16 PDONr // PDO number (1 to 512), or set highest bit if (0 to NR_OF_xPDOs)
  )
{
INTEGER8    k; // for loop counter
UNSIGNED8   b; // for while loop
// Pointer to a record in xPDODataPtr array
UNSIGNED16 MEM_FAR *pPDODataPtrEntry = 0;
// Pointer to a record in xPDOMap array
UNSIGNED16 MEM_FAR *pPDOMapEntry = 0;
UNSIGNED16  ODoff; // offset of an OD entry in the process image
UNSIGNED8   ODlen; // length of an OD entry
UNSIGNED16  map_num = 0; // number of entries mapped
UNSIGNED8   map_cnt; // count map entries in loop

  if(PDONr >= 0x8000)
  { // 
    PDONr &= 0x7FFF;
  }
  else
  {
    // Find PDO record
    PDONr = XPDO_FindPDORecord(TxRx,PDONr&0x7FFF);
    if (PDONr == 0xFFFF)
    {
      return;
    }
  }

#if NR_OF_RPDOS > 0
  if (TxRx == 1)
  { // Init for RPDO handling
    pPDODataPtrEntry = &(gRPDODataPtr[PDONr*8]);
    pPDOMapEntry = &(gRPDOmap[PDONr*9]);
    map_num = *pPDOMapEntry;
    pPDOMapEntry++;
  }
#endif

#if NR_OF_TPDOS > 0
  if (TxRx == 0)
  { // Init for TPDO handling
    pPDODataPtrEntry = &(gTPDODataPtr[PDONr*8]);
    pPDOMapEntry = &(gTPDOmap[PDONr*9]);
    map_num = *pPDOMapEntry;
    pPDOMapEntry++;
  }
#endif

  map_cnt = 0;
  b = 0;
  // Initilize Mapping in pPDOPataPtr
  while (b <= 7) // Loop until maximum number of bytes in PDO
  {
    if (map_cnt >= map_num) // Last entry reached
    {
      *pPDODataPtrEntry = 0xFFFF; // Set to last entry indication
#if NR_OF_RPDOS > 0
      if (TxRx == 1)
      { // RPDO, store new length
        gRPDOConfig[PDONr].len = b;
      }
#endif
#if NR_OF_TPDOS > 0
      if (TxRx == 0)
      { // TPDO, store new length for CAN transmit
        gTPDOConfig[PDONr].CAN.LEN = b;
      }
#endif
      break;
    }
    else
    {
      mpODProc = OD_ProcTablePtr(0);
      mpODProc += *pPDOMapEntry;
      ODlen = XSDO_ReadODProc(3) & 0x07; // length entry
      ODoff = XSDO_ReadODProc(5); // PIMG offset hi
      ODoff <<= 8;
      ODoff += XSDO_ReadODProc(4); // PIMG offset lo

#if (CHECK_PARAMETERS == 1)
      if ((ODlen > 4) || (ODoff > PIMGEND))
      {
        MCOUSER_FatalError(0x6000+(((UNSIGNED16)ODlen)<<8)+ODoff);
      }
#endif
        
      for (k = 0; k < (INTEGER8)ODlen; k++)    // For length of process data
      {
        // pPDODataPtrEntry is an offset into the process image
        // ODoff is the offset of the data in the Process Image
        *pPDODataPtrEntry = ODoff;
        *pPDODataPtrEntry += k;
        pPDODataPtrEntry++;
        b++;
      }
      map_cnt++; // increment number of entries mapped so far
    }
    pPDOMapEntry++;
  }
}


/**************************************************************************
DOES:     Makes a single PDO mapping entry
          Set EntryNr to zero and Length to number of entries mapped to set
          the number of mapping entries for this PDO
RETURNS:  TRUE, if PDO found and set, 
          FALSE, if PDO not implemented
GLOBALS:  Sets gRPDOmap and gTPDOmap to values defined in gSDOResponseTable
***************************************************************************/
UNSIGNED8 XPDO_SetPDOMapEntry (
  UNSIGNED8 TxRx, // Set to 0 for TPDO, 1 for RPDO
  UNSIGNED16 PDONr, // Number of PDO, 1 to 512
  UNSIGNED8 EntryNr, // Mapping entry (0 to 8)
  UNSIGNED16 Index, // Index of OD entry to be mapped
  UNSIGNED8 SubIdx, // Subindex of OD entry to be mapped
  UNSIGNED8 Length // Length of OD entry to be mapped (in bytes)
  )
{
UNSIGNED32 pdo_idx;   // record number in mXPDOmap[]
UNSIGNED32 found_od;  // record number in gODProcTable[]
OD_PROCESS_DATA_ENTRY MEM_CONST *pOD; // pointer into OD array

  // Find PDO record
  pdo_idx = XPDO_FindPDORecord(TxRx,PDONr);
  if (pdo_idx == 0xFFFF)
  {
    return FALSE;
  }
  // adjust to offset in array
  pdo_idx *= 9;

  // Find mapping entry in OD_SDOResponseTable(]
  if (TxRx == 0)
  { // TPDO
#if (NR_OF_TPDOS > 0)
    if (EntryNr > 0)
    {
      // Find mapped value in ODProcTable
      found_od = MCO_SearchODProcTable(Index,SubIdx);
      pOD = OD_ProcTablePtr(found_od);
      if ( (found_od == 0xFFFF) ||
           (Length != (pOD->len&0x07))
         )
      { // not found or length error
        gTPDOmap[pdo_idx+EntryNr] = 0xFFFF;
        return FALSE;
      }
      // Make mXPDOmap entry
      gTPDOmap[pdo_idx+EntryNr] = found_od;
    }
    else
    { // NrOfEntries is 0
      gTPDOmap[pdo_idx] = Length;
    }
#endif // (NR_OF_TPDOS > 0)
  }
  else
  {
#if (NR_OF_RPDOS > 0)
    if (EntryNr > 0)
    {
      // Find mapped value in ODProcTable
      found_od = MCO_SearchODProcTable(Index,SubIdx);
      pOD = OD_ProcTablePtr(found_od);
      if ( (found_od == 0xFFFF) ||
           (Length != (pOD->len&0x07))
         )
      { // not found or length error
        gRPDOmap[pdo_idx+EntryNr] = 0xFFFF;
        return FALSE;
      }
      // Make mXPDOmap entry
      gRPDOmap[pdo_idx+EntryNr] = found_od;
    }
    else
    { // NrOfEntries is 0
      gRPDOmap[pdo_idx] = Length;
    }
#endif // (NR_OF_RPDOS > 0)
  }

  return TRUE;
}


/**************************************************************************
DOES:     RESETS SINGLE PDO MAPPING ENTRY TO DEFAULT (HARDCODED)
RETURNS:  TRUE, if PDO found and reset, 
          FALSE, if PDO not implemented
***************************************************************************/
UNSIGNED8 XPDO_ResetPDOMapEntry (
  UNSIGNED8 TxRx, // Set to 0 for TPDO, 1 for RPDO
  UNSIGNED16 PDONr // Number of PDO, 1 to 512
  )
{

UNSIGNED32 pdo_idx;   // record number in mXPDOmap[]
UNSIGNED32 found_sdo; // record number in OD_SDOResponseTable[]
UNSIGNED8 *pSDO; // pointer into SDO response table
UNSIGNED16 index;
UNSIGNED8 map_nr;
UNSIGNED8 map_cnt;

  // Find PDO record
  pdo_idx = XPDO_FindPDORecord(TxRx,PDONr);
  if (pdo_idx == 0xFFFF)
  {
    return FALSE;
  }
  // adjust to offset in array
  pdo_idx *= 9;

  // Find mapping entry in OD_SDOResponseTable(]
  if (TxRx == 0)
  { // TPDO
#if (NR_OF_TPDOS > 0)
    // find length (number of mapping entries)
    found_sdo = MCO_SearchOD(0x1A00+PDONr-1,0);
    gTPDOmap[pdo_idx] = 0;
    for(map_cnt=1;map_cnt<=8;map_cnt++)
    {
      gTPDOmap[pdo_idx+map_cnt] = 0xFFFF;
    }
#endif // (NR_OF_TPDOS > 0)
  }
  else
  {
#if (NR_OF_RPDOS > 0)
    // find length (number of mapping entries)
    found_sdo = MCO_SearchOD(0x1600+PDONr-1,0);
    gRPDOmap[pdo_idx] = 0;
    for(map_cnt=1;map_cnt<=8;map_cnt++)
    {
      gRPDOmap[pdo_idx+map_cnt] = 0xFFFF;
    }
#endif // (NR_OF_RPDOS > 0)
  }

  map_nr = 0;
  map_cnt = 0;

  if (found_sdo == 0xFFFF)
  {
    return FALSE;
  }
  // adjust to offset in array
  found_sdo *= 8;
  pSDO = (UNSIGNED8 *) OD_SDOResponseTablePtr(0);
  map_nr = pSDO[found_sdo+4];
  if (map_nr > 8)
  {
    map_nr = 8;
  }

  // For all mapping entries
  while (map_cnt < map_nr)
  {
    // go to next record in gSDOResponseTable
    found_sdo += 8;
    map_cnt++;

    // get index, subindex and len info
    index = pSDO[found_sdo+7];
    index <<= 8;
    index += pSDO[found_sdo+6];

    // Make new mapping entry
    if (!(XPDO_SetPDOMapEntry(TxRx,PDONr,map_cnt,index,pSDO[found_sdo+5],(pSDO[found_sdo+4]>>3)&0x07)))
    { // Mapping entry could not be made
      map_nr = 0; // Set number of map entries to zero
    }
  }

  // Now set number of entries
  XPDO_SetPDOMapEntry(TxRx,PDONr,0,0,0,map_nr);

  XPDO_UpdatePDOMapping(TxRx,PDONr);

  return TRUE;
}


/**************************************************************************
DOES:    Handles incoming SDO Request for accesses to PDO Mapping
         Parameters
RETURNS: 0: Wrong access, SDO Abort sent
         1: Access was made, SDO Response sent
GLOBALS: Various global variables with configuration information
**************************************************************************/
UNSIGNED8 SDO_HandlePDOMapParam (
  UNSIGNED8  PDOType,  // 0 for TPDO, 1 for RPDO
  UNSIGNED16 index,    // OD index
  UNSIGNED8  *pData    // pointer to SDO Request message
  )
{

UNSIGNED16 pdonr; // PDONr - 1
UNSIGNED8 num; // Number of PDO map entries
UNSIGNED16 MEM_FAR *pMap; // current mapping entry, first is number of
UNSIGNED8 cmd; // command byte of SDO request
UNSIGNED16 table_offset; // offset in mapping table
UNSIGNED16 found_rec; // Entry in OD table found 
COBID_TYPE CID; // CAN ID

  pdonr = (index & 0x1FF)+1;
  found_rec = XPDO_FindPDORecord(PDOType,pdonr);
  if (found_rec == 0xFFFF)
  { // Not found
    MCO_SendSDOAbort(SDO_ABORT_GENERAL);
    return 0;
  }

  pdonr = found_rec; // record found

  if (PDOType == 0)
  { // TPDO
#if NR_OF_TPDOS > 0
    CID = gTPDOConfig[pdonr].CAN.ID;
    pMap = &(gTPDOmap[pdonr * 9]);
#else
    // No TPDOS available
    MCO_SendSDOAbort(SDO_ABORT_GENERAL);
    return 0;
#endif
  }
  else
  { // RPDO
#if NR_OF_RPDOS > 0
    CID = gRPDOConfig[pdonr].CANID;
    pMap = &(gRPDOmap[pdonr * 9]);
#else
    // No RPDOS available
    MCO_SendSDOAbort(SDO_ABORT_GENERAL);
    return 0;
#endif
  }

  cmd = *pData & 0xE0;
  num = (UNSIGNED8) *pMap;

  if (pData[3] > 8) // subindex
  { //  max of 8 map entries supported
    MCO_SendSDOAbort(SDO_ABORT_UNKNOWNSUB);
    return 0;
  }

  if (pData[3] == 0) // subindex
  { // Nr of entries

    // Conformance test tracker
    mCnt = 1;
    mLen = 0;

    if (cmd == 0x40)
    { // Read
      MCO_ReplyWith(&(num),1);
      return 1;
    }

    // Here it is a write
    if (pData[4] > 8)
    {
      // For conformance test: PDO Mapping (test 9)
      // this needs to report 06020000h, 06040041h or 06040042
      MCO_SendSDOAbort(SDO_ABORT_MAPLENGTH);
      // For conformance test: SDO Mapping (test 10)
      // this needs to report 06090030h, 06090031h or 06090032
      return 0;
    }
    // only allow write if PDO is disabled
    if (!(CID & COBID_DISABLED))
    { // PDO is not disabled
      MCO_SendSDOAbort(SDO_ABORT_UNSUPPORTED);
      return 0;
    }
    // get pointer to map data
    *pMap = pData[4];
    if (pData[4] != 0) // New value to be written
    { 
      if (mAbort > 0)
      { // detected abort reason
        MCO_SendSDOAbort(mAbort);
        mAbort = 0; // reset reason
        return 0;
      }
      // Test if all mapping entries are legal
      while (pData[4] > 0)
      {
        if (pMap[pData[4]] == 0xFFFF)
        { // Illegal value in mapping
          *pMap = 0; // set nr of mapped entries back to zero
          // MCO_SendSDOAbort(SDO_ABORT_UNSUPPORTED);
          // Changed for conformance test PDO25 to report:
          MCO_SendSDOAbort(SDO_ABORT_MAPLENGTH);
          return 0;
        }
        pData[4]--;
      }
      // Now re-initialize the PDO mapping
      XPDO_UpdatePDOMapping(PDOType,pdonr+0x8000);
    }
    MCOP_WriteConfirm();
    return 1;
  }
  
  if (cmd == 0x40)
  { // Read
    table_offset = *(pMap +  pData[3]);
    if (table_offset != 0xFFFF)
    { // Valid map entry
      mpODProc = OD_ProcTablePtr(0);
      mpODProc += table_offset;
      gTxSDO.BUF[7] = XSDO_ReadODProc(1); // index hi
      gTxSDO.BUF[6] = XSDO_ReadODProc(0); // index lo
      gTxSDO.BUF[5] = XSDO_ReadODProc(2); // subindex
      gTxSDO.BUF[4] = (XSDO_ReadODProc(3) & 0x07) << 3; // len (in bits)
    }
    else
    { // not a valid map entry, return dummy mapping
      gTxSDO.BUF[7] = 0; // index hi
      gTxSDO.BUF[6] = 0; // index lo
      gTxSDO.BUF[5] = 0; // subindex
      gTxSDO.BUF[4] = 0; // len (in bits)
    }
    gTxSDO.BUF[0] = 0x43; // Expedited, 4 bytes
    MCOHW_PushMessage(&gTxSDO);
    return 1;
  }

  // Write PDO mapping parameter
  if (num != 0)
  { // Nr of Entries must be zero to allow writing
    MCO_SendSDOAbort(SDO_ABORT_UNSUPPORTED);
    return 0;
  }
    
  // Search OD entry that is about to be mapped
  found_rec = MCO_SearchODProcTable(pData[6]+(((UNSIGNED16)pData[7])<<8),pData[5]);
  if (found_rec == 0xFFFF)
  { // Not found in list, so cannot be mapped
    MCO_SendSDOAbort(SDO_ABORT_NOTMAPPED);
    return 0;
  }
  // Now set module pointer to record found
  mpODProc = OD_ProcTablePtr(0);
  mpODProc += found_rec;

  num = XSDO_ReadODProc(3); // Get len and RD/WR/MAP info

  if (((num & 0x07) << 3) == pData[4]) // And of right length
  { // length is correct
    if ( ( (PDOType == 0) && (num & ODRD) && (num & RMAP)) || // TPDO and Read Mappable
         ( (PDOType == 1) && (num & ODWR) && (num & WMAP)) // RPDO and Write Mappable
       )
    { // entrry is of correct type
      // This is for the conformance test, when executed in sequence
      // count number of bits mapped so far
      if (pData[3] == mCnt)
      {
        mCnt++;
        mLen += pData[4]; // count all bits mapped so far
        if (mLen > 64)
        { /* in previous conformance test abort was required here,
             now set a state to produce abort later, when activating the mapping
            // MCO_SendSDOAbort(SDO_ABORT_MAPLENGTH);
            // return 2;
          */
          mAbort = SDO_ABORT_MAPLENGTH;
        }
      }
      else
      { // reset conformance test variables
        mCnt = 0;
        mLen = 0;
      }

      // Make new entry into map entry       
      pMap[pData[3]] = found_rec;
      MCOP_WriteConfirm();
      return 1;
    }
  }
  MCO_SendSDOAbort(SDO_ABORT_NOTMAPPED);
  return 0;
}

#endif // USE_DYNAMIC_PDO_MAPPING
