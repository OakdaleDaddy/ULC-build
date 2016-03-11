/**************************************************************************
PROJECT:     CANopenIA
MODULE:      CRC.C
CONTAINS:    CRC-16 checksum function for communication and other purposes.
             The polynomial used for the creation of the CRC table was
             x^16 + x^15 + x^2 + 1 (0xA001 for reflected CRC).
VERSION:     $LastChangedDate: 2010-01-19 23:39:58 +0100 (Di, 19 Jan 2010) $
             $LastChangedRevision: 1556 $
***************************************************************************/

#ifndef _CRC_H
#define _CRC_H

#ifdef __cplusplus
extern "C" {
#endif

/**************************************************************************
DOES:    Initialization of CRC module. Prepares for new calculation.
RETURNS: Initial CRC value
**************************************************************************/
extern UNSIGNED16 CRC_Init (
  void
  );


/**************************************************************************
DOES:    Adds one byte to the CRC checksum.
RETURNS: -
**************************************************************************/
extern void CRC_Add (
  UNSIGNED8 data,
  UNSIGNED16 *pCRC
  );


#ifdef __cplusplus
}
#endif

#endif // _CRC_H
/**************************************************************************
END OF FILE
**************************************************************************/
