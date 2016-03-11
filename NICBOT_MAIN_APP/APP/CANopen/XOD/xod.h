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
           $LastChangedDate: 2014-06-27 10:04:06 +0100 (Fri, 27 Jun 2014) $
           $LastChangedRevision: 1786 $
***************************************************************************/ 

#ifndef _XOD_H
#define _XOD_H

#include "mco.h"

typedef struct {
  UNSIGNED8 *p_SDOResponseBase;
  OD_PROCESS_DATA_ENTRY *p_ProcBase;
#if USE_EXTENDED_SDO
  OD_GENERIC_DATA_ENTRY *p_GenericBase;
#endif
  UNSIGNED8 *p_PIDefaults;
  UNSIGNED32 pimg_size;
  UNSIGNED16 chksum;
  UNSIGNED16 nr_rpdo;
  UNSIGNED16 nr_tpdo;
} OD_CONFIG;

extern OD_CONFIG gOD;

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
  );


/**************************************************************************
DOES:    Find the Process Image offset to an OD entry.
         When USE_XOD_ACCESS is set, use this to find an offset before
         using the PI_READ or PI_WRITE macros.
RETURNS: 0xFFFF if not found, else the offset
**************************************************************************/
UNSIGNED16 OD_GetPIEntryOffset(
  UNSIGNED16 index,
  UNSIGNED8 subindex
  );


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
  );

/**************************************************************************
DOES:     SWITCHES OVER TO A DIFFERENT OBJECT DICTIONARY
NOTE:     At pointer location, a binary EDS file as generated by 
          CANopen Architect is expected.
RETURNS:  TRUE if switch was completed, else FALSE
***************************************************************************/
UNSIGNED8 OD_SwitchObjectDictionary(
  UNSIGNED8 *pCfg // pointer to Object Dictionary tables
  );


/**************************************************************************
DOES:     ACCESS TO THE OBJECT DICTIONARY DATA
          HERE: SDO response table (8 bytes per entry)
RETURNS:  Pointer to requested entry
***************************************************************************/
UNSIGNED8 *OD_SDOResponseTablePtr_fct(UNSIGNED16 offset);


/**************************************************************************
DOES:     ACCESS TO THE OBJECT DICTIONARY DATA
          HERE: OD data in process image
RETURNS:  Pointer to requested entry
***************************************************************************/
OD_PROCESS_DATA_ENTRY *OD_ProcTablePtr_fct(UNSIGNED16 record);


#if USE_EXTENDED_SDO
/**************************************************************************
DOES:     ACCESS TO THE OBJECT DICTIONARY DATA
          HERE: Generic entries
RETURNS:  Pointer to requested entry
***************************************************************************/
OD_GENERIC_DATA_ENTRY *OD_GenericTablePtr_fct(UNSIGNED8 record);
#endif


#endif // (USE_XOD_ACCESS == 1)

#endif
/*----------------------- END OF FILE ----------------------------------*/
