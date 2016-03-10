/**************************************************************************
PROJECT:     CANopen Bootloader for Atmel AT90CAN128 AVR Microcontroller
MODULE:      FLASH.H
CONTAINS:    Include file for flash and EEPROM handling functions.
ENVIRONMENT: Compiled and tested with IAR EW for AVR on
             Atmel STK500/501 with AT90CAN128
             Compiler Version 5.11b
COPYRIGHT:   This version Embedded Systems Academy, Inc.
             Developed by Embedded Systems Academy, Inc.
VERSION:     $LastChangedDate: 2013-05-28 02:11:48 +0200 (Tue, 28 May 2013) $
             $LastChangedRevision: 2624 $
***************************************************************************/

/***************************************************************************
 DEFINITIONS AND TYPES
***************************************************************************/


/***************************************************************************
 EXPORTED GLOBALS
***************************************************************************/


/***************************************************************************
 EXPORTED FUNCTIONS
***************************************************************************/

/**************************************************************************
DOES:    Lock the fuse bits so that the bootloader is locked forever
PARAMETERS: -
RETURNS: -
**************************************************************************/
extern void FLASH_Lock_Fusebits (
  void
  );

/**************************************************************************
DOES:    Read the fuse bits and return locking status
PARAMETERS: -
RETURNS: TRUE, if bootloader is locked
**************************************************************************/
extern BOOLEAN FLASH_Read_Fusebits (
  void
  );

/**************************************************************************
DOES:    Read a byte from flash memory
PARAMETERS:
         addr:  Address in flash area to read. This is a byte address into
                the word-organized 128kB flash memory. The even/odd bit 0
                determines, which byte from a word is read. The address
                bit 16 determines the bank (0/1) from which the memory is
                read.
RETURNS: 8-bit read value
**************************************************************************/
extern UNSIGNED8 FLASH_Read_Byte (
  UNSIGNED32 addr
  );

/**************************************************************************
DOES:    Initialize the Flash and EEPROM hardware and programming
         variables/flags.
RETURNS:
**************************************************************************/
extern void FLASH_Init (
  void
  );

/**************************************************************************
DOES:    Verifies the EEPROM checksum and if the checksum is OK, reads the
         node parameters: Node ID, baudrate indicator and serial number.
PARAMETERS: -
RETURNS: FALSE for checksum error, TRUE for checksum OK and parameter read.
         *checksums bits 0..7 contain the EEPROM checksum and bits 8..15
         the calcuated checksum.
**************************************************************************/
extern BOOLEAN FLASH_EEPROM_Read_Params (
  UNSIGNED16 *checksums
  );

/**************************************************************************
DOES:    Calculate checksum over application flash area with TCP/IP method:
         All words are added, then all carries are added, then calculate
         1s complement.
RETURNS: TRUE if checksum matches and a valid application can be executed.
         *checksum contains the calculated checksum
**************************************************************************/
extern BOOLEAN FLASH_Checksum_OK (
  UNSIGNED32 start_adr,       // Start address for checksumming
  UNSIGNED32 end_adr,         // End address for checksumming
  UNSIGNED32 checksum_adr,    // Checksum address
  UNSIGNED16 *checksum        // Returns the calculated checksum
  );

/**************************************************************************
DOES:    Parses the load data stream in binary Intel hex format, detects
         the hex records, checks the checksum for each record and extracts
         the load data and load data address from each record. If a record
         is complete, it is programmed into flash memory.

         The input to this function is the payload of an SDO packet,
         between 1 and 7 bytes long. The SDO domain transfer doesn't "know"
         the structure of the file that is transferred. We receive a data
         stream and have to parse the data with a state machine that
         keeps it's state between function calls.

         The format of the hex record is:

         || BYTE 1 | BYTE 2 | BYTE 3 | BYTE 4 | BYTE 5 | ...  | BYTE n ||
         || LENGTH | ADDRH  | ADDRL  | RTYPE  | DATA   | ...  | CHECK  ||

         Higher addresses than 0xFFFF are set with a type-2 record that
         switches the segment for all following records.

PARAMETERS:
         load_data_length:   Length of fragment of load data

RETURNS: FALSE for error, TRUE for ok
         If error, sdo_abort_code is set.

GLOBALS: mParseStateMachine: State machine data
         mProgramData:       Programming data
         gRxCAN - CAN message with SDO data segment
**************************************************************************/
extern BOOLEAN FLASH_Parse_Program_Hex_Data (
  UNSIGNED8  load_data_length,  // Length of fragment of load data
  UNSIGNED32 *sdo_abort_code
  );

/***************************************************************************
END OF FILE
***************************************************************************/
