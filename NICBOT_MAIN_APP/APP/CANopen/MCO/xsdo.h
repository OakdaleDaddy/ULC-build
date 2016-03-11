/**************************************************************************
MODULE:    XSDO
CONTAINS:  MicroCANopen Plus, Extended SDO implementation
COPYRIGHT: Embedded Systems Academy, Inc. 2002-2015.
           All rights reserved. www.microcanopen.com
DISCLAIM:  Read and understand our disclaimer before using this code!
           www.esacademy.com/disclaim.htm
           This software was written in accordance to the guidelines at
           www.esacademy.com/software/softwarestyleguide.pdf
LICENSE:   THIS IS THE COMMERCIAL PLUS VERSION OF MICROCANOPEN
           ONLY USERS WHO PURCHASED A LICENSE MAY USE THIS SOFTWARE
VERSION:   6.20, ESA 11-MAY-15
           $LastChangedDate: 2015-05-09 19:41:45 -0300 (Sat, 09 May 2015) $
           $LastChangedRevision: 3390 $
***************************************************************************/ 

#ifndef _XSDO_H
#define _XSDO_H

#ifdef __cplusplus
extern "C" {
#endif

#include "mco.h"

#if USE_EXTENDED_SDO

/**************************************************************************
PUBLIC FUNCTIONS
***************************************************************************/
void XSDO_Abort (
  UNSIGNED8 SDOServer // Number of SDO Server (1 to NR_OF_SDOSERVER)
  );


/**************************************************************************
DOES:    Process SDO Segmented Requests to generic OD entries
RETURNS: 0x00 Nothing was done
         0x01 OK, handled, response generated
         0x02 Abort, SDO Abort was generated
**************************************************************************/
UNSIGNED8 XSDO_HandleExtended (
  UNSIGNED8 *pReqBUF, // Pointer to 8 data bytes with SDO data from request
  CAN_MSG *pResCAN, // Pointer to SDO response
  UNSIGNED8 SDOServer // Number of SDO Server (1 to NR_OF_SDOSERVER)
  );


/**************************************************************************
DOES:    Called from ProcessStackTick
         Checks if we are in middle of Block Read transfer
RETURNS: FALSE, nothing done
         TRUE, transfer in progress, message generated
**************************************************************************/
UNSIGNED8 XSDO_BlkRdProgress (
  void
  );


/**************************************************************************
 DOES:   Internal Funtion: Handles incoming SDO Request for accesses to 
         SDO Server Parameters
RETURNS: 0: Wrong access, SDO Abort sent
         1: Access was made, SDO Response sent
GLOBALS: Various global variables with configuration information
**************************************************************************/
UNSIGNED8 XSDO_HandleSDOServerParam (
   UNSIGNED16 index,    // OD index
   UNSIGNED8 *pData    // pointer to SDO Request message
  );

#endif // USE_EXTENDED_SDO

#ifdef __cplusplus
}
#endif

#endif // _XSDO_H
/**************************************************************************
END OF FILE
**************************************************************************/
