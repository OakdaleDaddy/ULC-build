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
  return 0;
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
  return 0;
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
}
#endif // USECB_SDO_WR_AFTER



#if USECB_APPSDO_READ || USECB_APPSDO_WRITE
#include <string.h>
char MEM_CONST od_2222_23_rd_buf1[] = "012345678901234567890123456789";
char MEM_CONST od_2222_23_rd_buf2[] = "Test of custom entry 2222h,23h 0123456789";
UNSIGNED8 od_2222_23_wr_buf[64];

#define RW_BUFSIZE      20              // maximum size for single r/w buffer
#define FSSIMU_PACKETS  10              // maximum number of packets for the multi-buffer access
#define FSSIMU_MAXSIZE  (FSSIMU_PACKETS*RW_BUFSIZE) // maximum size for the multi-buffer access

// od_2222_24_rw_buf is the read/write buffer for entry [2222h,24h]. That's the 
// buffer the stack "sees".
// od_2222_24_fssimu_buf simulates "some data source/sink in the background,"
// such as a file system. The stack never accesses this buffer directly. Instead,
// the call-back copies data back and forth in maximum chunks of RW_BUFSIZE.
// These MEM_CPY calls simulate file read/write.
UNSIGNED8 od_2222_24_rw_buf[RW_BUFSIZE];
UNSIGNED8 od_2222_24_fssimu_buf[FSSIMU_PACKETS][RW_BUFSIZE];

UNSIGNED16 bufcnt;
volatile UNSIGNED32 lenw = 0;
volatile UNSIGNED32 lenwc = 0;
UNSIGNED8 zero = 0;
#endif // USECB_APPSDO_READ || USECB_APPSDO_WRITE

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
  static UNSIGNED16 lenr;
  
  if ((idx == 0x2222) && (subidx == 0x23))
  { // handle this access, read alternating strings in single-buffer transfer
    if (lenr != sizeof(od_2222_23_rd_buf1)-1)
    {
      lenr = sizeof(od_2222_23_rd_buf1)-1;
      *size = lenr;
      *pDat = (UNSIGNED8 *)&od_2222_23_rd_buf1[0];
    }
    else
    {
      lenr = sizeof(od_2222_23_rd_buf2)-1;
      *size = sizeof(od_2222_23_rd_buf2)-1;
      *pDat = (UNSIGNED8 *)&od_2222_23_rd_buf2[0];
    }
  }
  else if ((idx == 0x2222) && (subidx == 0x24))
  { // handle this access, multi-buffer transfer
    *pDat = (UNSIGNED8 *)&od_2222_24_rw_buf[0];
    *totalsize = lenw;
    // either transmit full r/w buffer length, or partial buffer if data length is smaller
    *size = (lenw > sizeof(od_2222_24_rw_buf)) ? sizeof(od_2222_24_rw_buf) : lenw;
    // keep track of how many bytes have been transmitted
    lenwc = *totalsize - *size;
    // Simulate file system read by copying from simulation buffer to single r/w buffer
    bufcnt = 0;
    MEM_CPY(&od_2222_24_rw_buf[0], &od_2222_24_fssimu_buf[bufcnt][0], sizeof(od_2222_24_rw_buf));
    bufcnt++;
  }
  else
  {
    return 0;
  }

  return 1;
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
  if ((idx == 0x2222) && (subidx == 0x23))
  { // handle this access, single-buffer transfer finished
    *size = 0;
  }
  else if ((idx == 0x2222) && (subidx == 0x24))
  { // handle this access, multi-buffer transfer
    // either transmit full r/w buffer length, or partial buffer if it's the last one
    *size = (lenwc > sizeof(od_2222_24_rw_buf)) ? sizeof(od_2222_24_rw_buf) : lenwc;
    // keep track of how many bytes have been transmitted
    lenwc -= *size;
    // Simulate file system read by copying from simulation buffer to single r/w buffer
    MEM_CPY(&od_2222_24_rw_buf[0], &od_2222_24_fssimu_buf[bufcnt][0], sizeof(od_2222_24_rw_buf));
    bufcnt++;
  }
  
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
  if ((idx == 0x2222) && (subidx == 0x23))
  { // handle this access, single-buffer transfer
    *size = sizeof(od_2222_23_wr_buf);
    *pDat = (UNSIGNED8 *)&od_2222_23_wr_buf[0];
    return 1;
  }
  else if ((idx == 0x2222) && (subidx == 0x24))
  { // handle this access, multi-buffer transfer
    *totalsize = sizeof(od_2222_24_fssimu_buf);
    *size = sizeof(od_2222_24_rw_buf);
    *pDat = (UNSIGNED8 *)&od_2222_24_rw_buf[0];
    lenwc = 0;
    bufcnt = 0;
    return 1;
  }
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
  if ((idx == 0x2222) && (subidx == 0x23))
  { // handle this access, all should be done because of single-buffer transfer
    // Here enter code to retrieve data from buffer
    // Data length: size, more == 0
    if (more != 0)
    { // this should never happen
      for (;;); // wait here for break      
    } 
  }
  else if ((idx == 0x2222) && (subidx == 0x24))
  { // handle this access, multi-buffer transfer
    // simulate file system write by storing data from the single r/w buffer into the simulation buffer array
    MEM_CPY(&(od_2222_24_fssimu_buf[bufcnt][0]), &(od_2222_24_rw_buf[0]), sizeof(od_2222_24_rw_buf));
    bufcnt++;
    // keep track of how many bytes have been transferred
    lenwc += size;
    if (more == 0)
    { // this is the last transfer, all received 
      lenw = lenwc; // save new length of the entry, for read access
    } 
  }
}
#endif // USECB_APPSDO_WRITE


/**************************************************************************
END-OF-FILE 
***************************************************************************/ 

