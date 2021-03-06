/**************************************************************************
MODULE:    USER_CBDATA
CONTAINS:  Default functions for user call-backs accessing process data
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
#include "can_callbacks.h"


#ifdef MCOUSER_MINMAX
// SET THIS DEFINE MANUALLY TO ENABLE A CUSTOM MIN/MAX CHECK
// Also make sure to set USECB_SDO_WR_PI
// User example: Min Max control of SDO acces
UNSIGNED8 MEM_CONST gProcMin[PROCIMG_SIZE] = PIMGMINS;
UNSIGNED8 MEM_CONST gProcMax[PROCIMG_SIZE] = PIMGMAXS;
#endif


#if USECB_RPDORECEIVE
/**************************************************************************
DOES:    This function is called after an RPDO has been received and stored
         into the Process Image.
RETURNS: nothing
**************************************************************************/
void MCOUSER_RPDOReceived (
  UNSIGNED16 RPDONr, // RPDO Number
  UNSIGNED16 offset, // Offset to RPDO data in Process Image
  UNSIGNED8  len     // Length of RPDO
  )
{
}
#endif // USECB_RPDORECEIVE


#if USECB_ODDATARECEIVED
/**************************************************************************
DOES:    This function is called after data was received and stored
         (works for both SDO and PDO).
RETURNS: nothing
**************************************************************************/
void MCOUSER_ODData (
  UNSIGNED16 idx,
  UNSIGNED8 subidx,
  UNSIGNED8 MEM_PROC *pDat,
  UNSIGNED8 len
  )
{
}
#endif // USECB_ODDATARECEIVED


#if USECB_TPDORDY
/**************************************************************************
DOES:    This function is called before a TPDO is sent. For triggering
         modes that are outside of the application's doing (Event Timer,
         SYNC), it is called before the sent data is retrieved from the
         Process Image. This allows the application to update the TPDO
         data if necessary.
NOTE:    This function is also called before a change-of-state or
         application-triggered TPDO is sent, but updating the Process Image
         will not have any effect on the TPDO data in this case.
RETURNS: TRUE to allow the PDO to be sent, FALSE to stop PDO transmission
**************************************************************************/
UNSIGNED8 MCOUSER_TPDOReady (
  UNSIGNED16 TPDONr,      // TPDO Number
  UNSIGNED8  TPDOTrigger  // Trigger for this TPDO's transmission:
                          // 0: Event Timer
                          // 1: SYNC
                          // 2: SYNC+COS
                          // 3: COS or application trigger
  )
{
  // always transmit if event timer or SYNC is being used
  if (TPDOTrigger < 2) return TRUE;

  // customize for application-specific TPDO send conditions
  return TRUE;
}
#endif // USECB_TPDORDY


#if USECB_SYNCRECEIVE
/**************************************************************************
DOES:    This function is called with every SYNC message received.
         It allows the application to now apply all sync-triggered TPDO
         data to be applied to the application
RETURNS: nothing
**************************************************************************/
void MCOUSER_SYNCReceived (
  void
  )
{
}
#endif // USECB_SYNCRECEIVE


#if USECB_SDO_RD_PI
/**************************************************************************
DOES:    This function is called before an SDO read request is executed
         reading from the process image. The application can use this 
         function to either update the data or to deny access 
         (by returning an SDO Abort code).
RETURNS: 0, if access is granted, data can be copied and returned or
         CANopen SDO Abort Code - in which case the SDO transfer is aborted
**************************************************************************/
UNSIGNED32 MCOUSER_SDORdPI (
  UNSIGNED16 index,       // Index of Object Dictionary entry
  UNSIGNED8 subindex,     // Subindex of Object Dictionary entry
  UNSIGNED16 offset,      // Offset to data in process image
  UNSIGNED8 len           // Length of data 
  )
{
  UNSIGNED32 result = 0;

  result = CAN_ODRead(index, subindex);

  return result;
}
#endif // USECB_SDO_RD_PI


#if USECB_SDO_RD_AFTER
/**************************************************************************
DOES:    This function is called after an SDO read request was executed.
         The application can use this to clear the data or mark it as read.
RETURNS: Nothing
**************************************************************************/

void MCOUSER_SDORdAft (
  UNSIGNED16 index,       // Index of Object Dictionary entry
  UNSIGNED8 subindex,     // Subindex of Object Dictionary entry
  UNSIGNED16 offset,      // Offset to data in process image
  UNSIGNED8 len           // Length of data 
  )
{
}
#endif // USECB_SDO_RD_AFTER


#if USECB_SDO_WR_PI
/**************************************************************************
DOES:    This function is called before an SDO write request is executed
         writing to the process image. The application can use this 
         function to check the data (e.g. range check) BEFORE it gets
         written to the process image.
RETURNS: 0, if access is granted, data can be copied to process image or
         CANopen SDO Abort Code - in which case the SDO transfer is aborted
**************************************************************************/
UNSIGNED32 MCOUSER_SDOWrPI (
  UNSIGNED16 index,       // Index of Object Dictionary entry
  UNSIGNED8 subindex,     // Subindex of Object Dictionary entry
  UNSIGNED16 offset,      // Offset to data in process image
  UNSIGNED8 *pDat,        // Pointer to data received
  UNSIGNED8 len           // Length of data 
  )
{
   UNSIGNED32 result = 0;

   result = CAN_ODWrite(index, subindex, pDat, len);
#ifdef MCOUSER_MINMAX
UNSIGNED16 dat;
UNSIGNED16 comp;

  if ((index == 0x2030) && (subindex != 0x00))
  { // MinMax test entry for UNSIGNED16
    // Get current data
    dat = pDat[1];
    dat <<= 8;
    dat += pDat[0];
    // Get comparison minimum data
    comp = gProcMin[offset+1];
    comp <<= 8;
    comp += gProcMin[offset];
    if (dat < comp)
    {
      return SDO_ABORT_VALUE_LOW;
    }
    // Get comparison maximum data
    comp = gProcMax[offset+1];
    comp <<= 8;
    comp += gProcMax[offset];
    if (dat > comp)
    {
      return SDO_ABORT_VALUE_HIGH;
    }
  }
#endif // MCOUSER_MINMAX
  return result;
}
#endif // USECB_SDO_WR_PI


#if USECB_SDO_WR_AFTER
/**************************************************************************
DOES:    This function is called after an SDO write request was executed.
         Data is now in the process image and can be processed.
RETURNS: Nothing
**************************************************************************/

void MCOUSER_SDOWrAft (
  UNSIGNED16 index,       // Index of Object Dictionary entry
  UNSIGNED8 subindex,     // Subindex of Object Dictionary entry
  UNSIGNED16 offset,      // Offset to data in process image
  UNSIGNED8 len           // Length of data 
  )
{
   CAN_ODData(index,subindex,&(gProcImg[offset]),len);
}
#endif // USECB_SDO_WR_AFTER


#if USECB_APPSDO_READ
/*******************************************************************************
DOES:    Call Back function to allow implementation of custom, application
         specific OD Read entries
         Here: Alternating between 2 different strings
RETURNS: 0x00 - OD entry not handled by this function
         0x01 - OD entry handled by this function
         0x05 - Abort with SDO_ABORT_WRITEONLY
         0x06 - Abort with SDO_ABORT_NOT_EXISTS
*******************************************************************************/
UNSIGNED8 MCOUSER_AppSDOReadInit (
  UNSIGNED8 sdoserver,
  UNSIGNED16 idx, // Index of OD entry
  UNSIGNED8 subidx, // Subindex of OD entry
  UNSIGNED32 MEM_FAR *totalsize, // RETURN: total size of data, only set if >*size
  UNSIGNED32 MEM_FAR *size, // RETURN: size of data buffer
  UNSIGNED8 * MEM_FAR *pDat // RETURN: pointer to data buffer
  )
{
   return( CAN_AppSDOReadInit(sdoserver, idx, subidx, totalsize, size, pDat) );
}


/*******************************************************************************
DOES:    Call Back function to allow implementation of custom, application
         specific OD Read entries, called at end of transfer with the option
         to add more data.
RETURNS: Nothing
*******************************************************************************/
void MCOUSER_AppSDOReadComplete (
  UNSIGNED8 sdoserver, // The SDO server number on which the request came in
  UNSIGNED16 idx, // Index of OD entry
  UNSIGNED8 subidx, // Subindex of OD entry
  UNSIGNED32 MEM_FAR *size // RETURN: size of next block of data, 0 for no further data
  )
{
  CAN_AppSDOReadComplete(sdoserver, idx, subidx, size);
  return;
}
#endif // USECB_APPSDO_READ


#if USECB_APPSDO_WRITE
/*******************************************************************************
DOES:    Call Back function to allow implementation of custom, application
         specific OD Read entries
         Here: Simply receive data
RETURNS: 0x00 - OD entry not handled by this function
         0x01 - OD entry handled by this function
         0x05 - Abort with SDO_ABORT_WRITEONLY
         0x06 - Abort with SDO_ABORT_NOT_EXISTS
*******************************************************************************/
UNSIGNED8 MCOUSER_AppSDOWriteInit (
  UNSIGNED8 sdoserver,
  UNSIGNED16 idx, // Index of OD entry
  UNSIGNED8 subidx, // Subindex of OD entry
  UNSIGNED32 MEM_FAR *totalsize, // RETURN: total maximum size of data, only set if >*size
  UNSIGNED32 MEM_FAR *size, // Data size, if known. RETURN: max size of data buffer
  UNSIGNED8 * MEM_FAR *pDat // RETURN: pointer to data buffer
  )
{
  return 0;
}


/*******************************************************************************
DOES:    Call Back function to allow implementation of custom, application
         specific OD Write entries, call at end of transfer of a block. For
         multiple blocks per transfer, the same buffer is used for all blocks.
RETURNS: Nothing
*******************************************************************************/
void MCOUSER_AppSDOWriteComplete (
  UNSIGNED8 sdoserver, // The SDO server number on which the request came in
  UNSIGNED16 idx, // Index of OD entry
  UNSIGNED8 subidx, // Subindex of OD entry
  UNSIGNED32 size, // Number of bytes written (of last block)
  UNSIGNED32 more // Number of bytes still to come (of total transfer)
  )
{
}
#endif // USECB_APPSDO_WRITE


/**************************************************************************
END-OF-FILE 
***************************************************************************/ 

