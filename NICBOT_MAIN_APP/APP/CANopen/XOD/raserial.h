/**************************************************************************
PROJECT:   Packet Based Serial Protocol Handler
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


#ifndef _SERIALPROTOCOL_H
#define _SERIALPROTOCOL_H

#ifdef __cplusplus
extern "C" {
#endif

/***************************************************************************
 DEFINITIONS AND TYPES
***************************************************************************/

// maximum number of bytes of data in a packet
#define MAX_PACKET_DATA 32

// error flags
#define SERIALPROTOCOL_ERROR_UART 0x01UL

/***************************************************************************
 EXPORTED GLOBALS
***************************************************************************/


/***************************************************************************
 EXPORTED FUNCTIONS
***************************************************************************/

/**************************************************************************
DOES:    Initialization of protocol handler
RETURNS: -
**************************************************************************/
extern void SerialProtocol_Init (
  void
  );

/**************************************************************************
DOES:    Checks if any communication errors occured and returns the
         error status. Only TRUE/FALSE value for the global UART error
         status. This could be made more specific if needed at the host.
		 Clears error flags and counters.
RETURNS: Current error flags (SERIALPROTOCOL_ERROR_xxx)
GLOBALS: SerialProtocol_ReceiveErrorCounter,
         SerialProtocol_TransmitErrorCounter
**************************************************************************/
extern UNSIGNED16 SerialProtocol_CheckError (
  void
  );


/**************************************************************************
DOES:    Transmits all packets in transmit buffer
RETURNS: Nothing
**************************************************************************/
void SerialProtocol_CompleteTransmits (
  void
  );


/**************************************************************************
DOES:    Executes state machines - must be called periodically
RETURNS: Nothing
**************************************************************************/
extern void SerialProtocol_Process (
  void
  );


/**************************************************************************
DOES:    Queues a message for transmission
RETURNS: TRUE if message was queued, FALSE if there was an error
**************************************************************************/
extern UNSIGNED8 SerialProtocol_PushMessage (
  UNSIGNED8 length1,                                       // length of data chunk 1
  UNSIGNED8 *pTx1,                                         // pointer to data chunk 1
  UNSIGNED8 length2,                                       // length of data chunk 2
  UNSIGNED8 *pTx2                                          // pointer to data chunk 2
  );


/**************************************************************************
DOES:    Gets the next message from the receive buffer
RETURNS: Number of data bytes copied or zero if no messages
**************************************************************************/
extern UNSIGNED8 SerialProtocol_PullMessage (
  UNSIGNED8 *pRx                                           // pointer to location to store data
  );

#ifdef __cplusplus
}
#endif

#endif // _SERIALPROTOCOL_H
/**************************************************************************
END OF FILE
**************************************************************************/
