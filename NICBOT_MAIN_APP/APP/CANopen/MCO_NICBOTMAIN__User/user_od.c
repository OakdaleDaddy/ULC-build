/**************************************************************************
MODULE:    USER_OD - Here: STANDARD USAGE OF GENERATED OD TABLES
CONTAINS:  MicroCANopen Object Dictionary and Process Image implementation
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

#include "entriesandreplies.h"


/**************************************************************************
GLOBAL VARIABLES
***************************************************************************/ 

// This structure holds all node specific configuration
UNSIGNED8 MEM_PROC gProcImg[PROCIMG_SIZE] = PIMGDEFAULTS;

// Table with SDO Responses for read requests to OD
UNSIGNED8 MEM_CONST gSDOResponseTable[] = {
// Each Row has 8 Bytes:
// Command Specifier for SDO Response (1 byte)
//   bits 2+3 contain: '4' � {number of data bytes}
// Object Dictionary Index (2 bytes, low first)
// Object Dictionary Subindex (1 byte)
// Data (4 bytes, lowest bytes first)

  // Include file generated by CANopen Architect
  SDOREPLY_ENTRIES

  // End-of-table marker
  0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF
};


// Table with Object Dictionary entries to process Data
OD_PROCESS_DATA_ENTRY MEM_CONST gODProcTable[] = 
{

  // Include automatically generated files from CANopenArchitect EDS
  ODENTRY_ENTRIES

  // End-of-table marker
  ODENTRY(0xFFFF,0xFF,0xFF,0xFFFF)
};


#if USE_EXTENDED_SDO
// Table with generic entries to memory
OD_GENERIC_DATA_ENTRY MEM_CONST gODGenericTable[] = 
{
  ODGENTRY_ENTRIES

  // End-of-table marker
  ODGENTRYP(0xFFFF,0xFF,0xFF,0xFFFF,0xFFFF)
};
#endif // USE_EXTENDED_SDO


/**************************************************************************
END-OF-FILE 
***************************************************************************/ 

