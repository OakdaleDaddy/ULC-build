/**************************************************************************
PROJECT:   Communication interface for Remote Access, here UART
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

#ifndef _MCOHW_COM_H
#define _MCOHW_COM_H

#ifdef __cplusplus
extern "C" {
#endif

/***************************************************************************
 DEFINITIONS AND TYPES
***************************************************************************/

#define COM_COMMON_ERR       0x40          // Common error of receiver
#define COM_FULL_RX          0x10          // Full receive buffer
#define COM_FULL_TX          0x20          // Full transmit buffer


/**************************************************************************
DOES:    Initialization of COM interface
RETURNS: -
**************************************************************************/
void COM_Init (
  void
  );


/**************************************************************************
DOES:    Waits for all transmits to be completed
RETURNS: Nothing
**************************************************************************/
void COM_Flush (
  void
  );
	
	
/**************************************************************************
DOES:    Reads next received byte from the receive buffer, if not empty
RETURNS: TRUE, if byte was read, FALSE otherwise
**************************************************************************/
UNSIGNED8 COM_GetByte (
  UNSIGNED8 *byt   // Pointer to return the received character
  );


/**************************************************************************
DOES:    Writes a byte to the transmit buffer.
         If buffer is full, data is NOT copied to buffer, buffer remains
         unchanged.
RETURNS: TRUE, if byte was sent, FALSE otherwise (buffer full)
**************************************************************************/
UNSIGNED8 COM_SendByte (
  UNSIGNED8 byt   // Character to send
  );


/**************************************************************************
DOES:    Reads the error status of the communication port and clears status
RETURNS: Error status
**************************************************************************/
UNSIGNED8 COM_GetErrorStatus (
  void
  );


/**************************************************************************
DOES:    Checks if the transmit buffer is empty
RETURNS: TRUE if transmit buffer is empty
**************************************************************************/
UNSIGNED8 COM_TxEmpty (
  void
  );


#ifdef __cplusplus
}
#endif

#endif // ifndef _MCOHW_COM_H
/***************************************************************************
END OF FILE
***************************************************************************/
