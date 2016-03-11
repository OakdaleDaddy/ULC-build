/**************************************************************************
MODULE:    Remote Access Handler
CONTAINS:  Implementation of serial communication with a host interface
           as specified by Serial CANopen Slave Control Interface
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


#ifndef _REMOTE_ACCESS_H
#define _REMOTE_ACCESS_H

#ifdef __cplusplus
extern "C" {
#endif


/**************************************************************************
DOES:    Initializes the serial interface to the host, 
         also flushes all transmit/receive buffers
RETURNS: nothing
**************************************************************************/
void MCORA_Init (
  void
  );


/**************************************************************************
DOES:    Gets the current mode value that was passed from the host
         in the initialization command
RETURNS: mode value
**************************************************************************/
UNSIGNED8 MCORA_GetCurrentMode(
  void
  );


/**************************************************************************
DOES:    Main Processing for Host Interface interaction,
         must be called continously
RETURNS: nothing
**************************************************************************/
void MCORA_ProcessHost(
  void
  );


/**************************************************************************
DOES:    Gets the last stack configuration received via the host interface
RETURNS: nothing
**************************************************************************/
void MCORA_GetStackConfiguration(
  UNSIGNED8 *pmode,         // location to store mode
  UNSIGNED8 *pnodeid,       // location to store node id
  UNSIGNED16 *pcanbps,      // location to store can baudrate in kbps
  UNSIGNED32 *pserialnumber // location to store serial number
  );


/**************************************************************************
DOES:    Sends a write local response to the host
RETURNS: TRUE for success, FALSE for error
**************************************************************************/
UNSIGNED8 MCORA_SendWriteLocalResponse(
  UNSIGNED16 index,    // od index
  UNSIGNED8 subindex,  // od subindex
  UNSIGNED16 err       // error code or zero for no error
  );


/**************************************************************************
DOES:    Sends a write remote response to the host
RETURNS: TRUE for success, FALSE for error
**************************************************************************/
UNSIGNED8 MCORA_SendWriteRemoteResponse(
  UNSIGNED8 sdo,       // sdo channel number
  UNSIGNED16 index,    // od index
  UNSIGNED8 subindex,  // od subindex
  UNSIGNED16 err       // error code or zero for no error
  );


/**************************************************************************
DOES:    Sends a read local response to the host
RETURNS: TRUE for success, FALSE for error
**************************************************************************/
UNSIGNED8 MCORA_SendReadLocalResponse(
  UNSIGNED16 index,     // od index
  UNSIGNED8 subindex,   // od subindex
  UNSIGNED16 err,       // error code or zero for no error
  UNSIGNED16 length,    // length of data from od entry
  UNSIGNED8 *pdata      // data from od entry
  );


/**************************************************************************
DOES:    Sends a read remote response to the host
RETURNS: TRUE for success, FALSE for error
**************************************************************************/
UNSIGNED8 MCORA_SendReadRemoteResponse(
  UNSIGNED8 sdo,        // sdo channel number
  UNSIGNED16 index,     // od index
  UNSIGNED8 subindex,   // od subindex
  UNSIGNED16 err,       // error code or zero for no error
  UNSIGNED16 length,    // length of data from od entry
  UNSIGNED8 *pdata      // data from od entry
  );


/**************************************************************************
DOES:    Sends a heartbeat conumber init response to the host
RETURNS: TRUE for success, FALSE for error
**************************************************************************/
UNSIGNED8 MCORA_SendHBConsumerInitResponse(
  UNSIGNED8 nodeid,      // id of node to monitor
  UNSIGNED16 timeout,    // timeout in milliseconds
  UNSIGNED16 err         // error code or zero for no error
  );


/**************************************************************************
DOES:    Sends an initialization complete message to the host
RETURNS: TRUE for success, FALSE for error
**************************************************************************/
UNSIGNED8 MCORA_SendInitComplete(
  UNSIGNED8 nodeid,          // node id being used
  UNSIGNED16 bps,            // can baud rate being used in kbps
  UNSIGNED32 serialnumber    // serial number being used
  );


/**************************************************************************
DOES:    Sends a new process data local write notification to the host
RETURNS: TRUE for success, FALSE for error
**************************************************************************/
UNSIGNED8 MCORA_SendLocalProcessDataWrite(
  UNSIGNED16 index,         // od index
  UNSIGNED8 subindex,       // od subindex
  UNSIGNED16 length,        // length of data written to od entry
  UNSIGNED8 *pdata          // data written to od entry
  );


/**************************************************************************
DOES:    Sends a node status changed indication to the host
RETURNS: TRUE for success, FALSE for error
**************************************************************************/
UNSIGNED8 MCORA_SendNodeStatusChanged(
  UNSIGNED8 nodeid,         // id of node which was lost
  UNSIGNED8 status,         // staus of node
  UNSIGNED8 wait            // if true then blocks until indication has been sent
  );


/**************************************************************************
DOES:    Call Back to Remote Access module, when SDO transfer completed
RETURNS: Nothing
**************************************************************************/
void MCORACB_SDOComplete (
  UNSIGNED8 channel, // SDO channel number in range of 1 to NR_OF_SDO_CLIENTS
  UNSIGNED32 abort_code // status, error, abort code
  );


#ifdef __cplusplus
}
#endif

#endif // _REMOTE_ACCESS_H
/**************************************************************************
END OF FILE
**************************************************************************/
