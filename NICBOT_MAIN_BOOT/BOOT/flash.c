/**************************************************************************
PROJECT:     CANopen Bootloader for Atmel AT90CAN128 AVR Microcontroller
MODULE:      FLASH.C
CONTAINS:    Flash and EEPROM handling functions.
ENVIRONMENT: Compiled and tested with IAR EW for AVR on
             Atmel STK500/501 with AT90CAN128
             Compiler Version 5.11b
COPYRIGHT:   This version Embedded Systems Academy, Inc.
             Developed by Embedded Systems Academy, Inc.
VERSION:     $LastChangedDate: 2016-02-26 20:53:15 +0100 (Fri, 26 Feb 2016) $
             $LastChangedRevision: 3576 $
***************************************************************************/

#include <avr/pgmspace.h>
#include "config.h"
#include "flash_boot_lib.h"
#include "types.h"
#include "mcu.h"
#include "userdefs.h"
#include "main.h"
#include "canhw.h"
#include "sdo.h"
#include "cop.h"
#include "flash.h"


/**************************************************************************
 LOCAL DEFINITIONS
***************************************************************************/

//#define SHOW_CRC // define to output CRC on CAN bus at start

#define FLASH_NUM_64K_BANKS    2         // Number of 64K Flash pages in the device.
#define FLASH_SIZE_ERASE_PAGE  0x0080U   // Number of words in one flash page
#define FLASH_PAGE_ADR_SHIFT   8         // Number of the first relevant bit for a
                                         // page start address (byte address)

// Dimension the data buffer big enough to hold a maximum-sized hex-record.
// This would be a hex-record with a data length field of 0xFF=255.
#define PROGRAMDATA_BUFFERSIZE 255


// Parsing states
typedef enum {
  PARSESTATE_START,
  PARSESTATE_LENGTH,
  PARSESTATE_ADDRH,
  PARSESTATE_ADDRL,
  PARSESTATE_TYPE,
  PARSESTATE_DATA,
  PARSESTATE_CHECKSUM
} ParseStateType;

typedef struct {
  ParseStateType parse_state;
  UNSIGNED8      record_data;
  UNSIGNED8      checksum;
  UNSIGNED8      record_type;
  UNSIGNED32     hex_usba;      // Hex type-2-record Upper Segment Base Address
  BOOLEAN        file_close;    // Flag to signal end of programming - flush the open page
} ParseStateMachineType;

static ParseStateMachineType mParseStateMachine;


// This structure holds all programming data (related to the load format which is
// a binary Intel hex format)
typedef struct {
  BOOLEAN       valid;       // TRUE, if the program data values contain valid data
  UNSIGNED8     bank;        // Flash memory bank (adr >64kB) of the programming data
  UNSIGNED16    addr_offs;   // Address bits 15..0 of the programming data
  UNSIGNED8     data_size;   // Anticipated number of data bytes in the record
  UNSIGNED8     data_cnt;    // Number of bytes received so far
  UNSIGNED8     data[PROGRAMDATA_BUFFERSIZE];
} ProgramDataType;

static ProgramDataType mProgramData;


// This structure holds flash programming information (related to the programming
// algorithm and the chip hardware)
typedef struct {
  UNSIGNED16 pagebuf[FLASH_SIZE_ERASE_PAGE];  // Flash software page buffer
  UNSIGNED8  last_erased_bank;   // Memorize the last opened (=erased) flash bank
  UNSIGNED8  last_erased_page;   // Memorize the last opened (=erased) flash page
  BOOLEAN    opened;             // The flash is "opened" after reset, before the first data has been programmed
  UNSIGNED8  enabled;            // Must be ==1 to enable SPM code
} FlashStateType;

static FlashStateType mFlashState;



/**************************************************************************
 EXTERNAL REFERENCES
***************************************************************************/


/**************************************************************************
 PUBLIC DEFINITIONS
***************************************************************************/

// extract value from post build step
// define CRC here, refer by extern within main to avoid optimization exclusion
// step 1: define FFFF value and observe output 
// step 2: insert value
__attribute__ ((section (".BOOTCRC"))) const UNSIGNED16 boot_crc = 0x23E7;

/**************************************************************************
 LOCAL FUNCTION PROTOTYPES
***************************************************************************/

static UNSIGNED8 FLASH_Calculate_CRC8 (
  UNSIGNED8 data
  );

static UNSIGNED8 FLASH_EEPROM_ReadByte (
  UNSIGNED16 adr
  );

static void FLASH_Protected_SPM (
  UNSIGNED8 val_spmcsr,
  UNSIGNED8 flag
  );

static UNSIGNED16 FLASH_Read_Word (
  UNSIGNED32 word_addr
  );

static BOOLEAN FLASH_Program (
  UNSIGNED8 new_bank,
  UNSIGNED8 new_page,
  BOOLEAN   flash_close,
  UNSIGNED8 offs_dat,
  UNSIGNED8 len,
  UNSIGNED8 offs_buf
  );

static BOOLEAN FLASH_Program_Data_Received (
  UNSIGNED32 *sdo_abort_code
  );

static void FLASH_Init_Program_Data (
  void
  );



/**************************************************************************
 LOCAL FUNCTIONS
***************************************************************************/

/**************************************************************************
DOES:    Calculates the CRC8 checksum for a byte stream.
         Using a caculated algorithm takes less constant space than a
         table-based one.
RETURNS: Current CRC8 checksum
**************************************************************************/
static UNSIGNED8 FLASH_Calculate_CRC8 (
  UNSIGNED8 data  // New data byte for CRC8 calculation
  )
{
  static UNSIGNED8 crc = 0;
  UNSIGNED8 i        = 0x00U;
  UNSIGNED8 feedback = 0x00U;
  UNSIGNED8 temp     = 0x00U;

  // Calculate CRC-8
  for (i = 0; i < 8; i++)
  {
    feedback  = (crc & 0x01U);
    crc     >>= 1;
    temp      = feedback ^ (data & 0x01U);
    if ( temp != 0x00U )
    {
      crc ^= 0x9C;  // Preferred polynomial
    }

    data >>= 1;
  }

  return (crc);
}



/**************************************************************************
DOES:    Read a byte from EEPROM array
PARAMETERS:
    adr:  Address in EEPROM area to read
RETURNS: 8-bit value
**************************************************************************/
static UNSIGNED8 FLASH_EEPROM_ReadByte (
  UNSIGNED16 adr
  )
{
  EEAR = adr;
  SETBIT(EECR,EERE);
  return(EEDR);
}



/**************************************************************************
DOES:    Placing (protected) SPM code at a fixed location for future
         bootloader upgrades.
RETURNS:
**************************************************************************/
__attribute__ ((section (".SPMCODE")))
// No optimization for this function so that we can ensure linear code.
__attribute__((optimize("O0")))
static void FLASH_Protected_SPM (
  UNSIGNED8 val_spmcsr,   // Value for SPMCSR
  UNSIGNED8 flag          // Flag must have bit 1 set to prevent accidential execution
  )
{
  if (mFlashState.enabled == 0x01U)
  { // Only execute if global enable flag is set
    SPMCSR = val_spmcsr;

    // Skip SPM instruction if bit 1 in R17 (flag) is cleared.
    // After setting SPMCR we have only 4 clock cycles
    // time to execute the SPM instruction, which is
    // not enough for the equivalent C code to skip
    // the SPM instruction if bit 1 in 'flag' is not
    // set:
    //
    //   if ((flag & 0x02) == 0x02)
    //   { ...
    //
    // Logically, the following assembly instruction
    asm( "SBRC R17,1" );
    // fulfils the same purpose, but is fast enough.

    asm( "SPM" );

    for ( ; BITSET(SPMCSR,SPMEN); )
    { // just wait until SPMEN bit has cleared
      ;
    }
  }

  return;
}



/**************************************************************************
DOES:    Read a word from flash memory
PARAMETERS:
         adr:  Address in flash area to read. This is a byte address into
               the word-organized flash memory (bit 0 has to be set to 0).
RETURNS: 16-bit read value
**************************************************************************/
static UNSIGNED16 FLASH_Read_Word (
  UNSIGNED32 word_addr   // Address of the word in flash
  )
{
  UNSIGNED16 read_word;

  read_word = pgm_read_word_far(word_addr);

  // Set RAMPZ register (back) to access the same bank of program memory
  // where this program is located
  RAMPZ = BL_BANK;

  return(read_word);
}



/**************************************************************************
DOES:    Main buffered flash programming and erase function.

         For a freshly opened flash module (after reset or after the flash
         programming has been properly closed), erase the complete
         application flash area.

         Checks if a new page needs to be opened. If so, the old page is
         "closed", which means the hardware page buffer is programmed
         into the flash memory with the data in the software page buffer,
         the programming is verified (return if error), and a new page is
         "opened", which means the software page buffer is initialized
         with 0xFFFFs.

         If flash programming is closed, the current page is always closed.

         When nothing special has to be done (i.e. not called for closing),
         only the software page buffer is filled with a new fragment of
         programming data.

RETURNS: TRUE if programming was successful, FALSE otherwise

GLOBALS: mFlashState for flash programming state information and software
         page buffer.
         mProgramData for programming data from the load file.
**************************************************************************/
static BOOLEAN FLASH_Program (
  UNSIGNED8 new_bank,    // Bank and
  UNSIGNED8 new_page,    // page number of the programming data
  BOOLEAN   flash_close, // If TRUE, forces the closing (programming) of the last page but suppresses the erase of a new page
  UNSIGNED8 offs_dat,    // Array index into mProgramData.data for start of programming data
  UNSIGNED8 len,         // Length of the programming data
  UNSIGNED8 offs_buf     // Byte offset into buffer mFlashState.pagebuf[] for programming data
  )
{
  BOOLEAN    return_val = TRUE;
  UNSIGNED16 addr       = 0x0000U;
  UNSIGNED16 prg_dat    = 0x0000U;
  UNSIGNED16 i          = 0U;

  // If we are dealing with a new page (a new bank automatically means a new page, too), close
  // the old page by writing the software page buffer to the hardware page buffer, and triggering
  // a page program cycle with the flash module. Then the programming of the page is verified
  // and if there is an error in programming, we return with an error status.
  // If there was no error, the new page needs to be opened, which means erasing the hardware
  // flash page and filling the software page buffer with the pattern for unprogrammed flash
  // (0xFFFF).
  if ( (mFlashState.last_erased_bank != new_bank) ||
       (mFlashState.last_erased_page != new_page) ||
       (mFlashState.opened == TRUE)               ||
       (flash_close == TRUE)                        )
  {
    if (mFlashState.opened)
    { // If the flash was freshly opened we don't have any previous page data to program,
      // but we have to erase the whole application area! For the address range we
      // assume that the application checksum area encompasses the complete application
      // area.
      UNSIGNED32 abs_addr = 0x00000000UL;

      for (abs_addr = AP_CHECKSUM_START; abs_addr < (AP_CHECKSUM_END+1); abs_addr += 2*FLASH_SIZE_ERASE_PAGE)
      {
        // Serve the watchdog in between, otherwise reset will occur
        COP_Serve();

		RAMPZ = (UNSIGNED8)((abs_addr >> 16) & 0x01U);
		flash_page_erase((UNSIGNED16)(abs_addr & 0x0000FFFFUL));
      }

      // From this moment on, the flash is no longer freshly opened.
      mFlashState.opened = FALSE;
    }
    else
    { // Otherwise program the previous page

      // Calculate start address offset of the page. Each page is 128 words, or
      // 256 bytes big, so shifting left by eight gets us the absolute address
      // offset within the bank.
      addr = (UNSIGNED16)mFlashState.last_erased_page << FLASH_PAGE_ADR_SHIFT;

	  RAMPZ = mFlashState.last_erased_bank;
	  flash_wr_block((U8*)mFlashState.pagebuf, addr, FLASH_SIZE_ERASE_PAGE*2);

      // Verify programming. For bank 1 the 64k offset has to be added to the (absolute) address.
      for (i = 0; (i < FLASH_SIZE_ERASE_PAGE) && return_val; i++)
      {
		volatile unsigned short x;
		  
		RAMPZ = mFlashState.last_erased_bank;
		x = flash_rd_word(addr+2*i);
		
		if (x != mFlashState.pagebuf[i])
        {
          return_val = FALSE;
        }
      }
    }

    // Fill the software page buffer with 0xFF (to prepare for programming the next page)
    for (addr=0; addr < (FLASH_SIZE_ERASE_PAGE); addr++)
    {
      mFlashState.pagebuf[addr] = 0xFFFFU;
    }

    // Memorize "last erased page"
    mFlashState.last_erased_bank = new_bank;
    mFlashState.last_erased_page = new_page;
  }

  if (flash_close || (return_val == FALSE))
  { // If this is to close the flash, there is no programming data available, so skip this step.
    // Also skip it after a programming error.
    ;
  }
  else
  { // Copy programming data into software page buffer

    // offs_buf is a byte offset, but the software programming buffer is
    // a word buffer. Since the bytes to insert into the software page
    // buffer can be misaligned, we need to copy byte-by-byte while taking
    // care of the word granularity of the target buffer and the endianness.
    offs_buf >>= 1;     // Calculate index into word buffer mFlashState.pagebuf[]

    for (i=0; i < len; i++)
    {
      UNSIGNED8 word_offs;

      word_offs = offs_buf + (i>>1);

      // Get current word from page buffer
      prg_dat = mFlashState.pagebuf[word_offs];

      if (BITCLEARED(i,0))
      { // Even i means byte is in lower byte of word
        prg_dat &= 0xFF00U;
        prg_dat |= (UNSIGNED16)mProgramData.data[offs_dat+i];
      }
      else
      { // Odd i means byte is in upper byte of word
        prg_dat &= 0x00FFU;
        prg_dat |= ((UNSIGNED16)mProgramData.data[offs_dat+i] << 8);
      }

      // Insert word back into page buffer
      mFlashState.pagebuf[word_offs] = prg_dat;
    }
  }

  return (return_val);
}



/**************************************************************************
DOES:    Checks a binary record and programs into Flash it if no errors.
PARAMETERS (GLOBALS):
         Record is stored in mProgramData.data
         Record length is stored in mProgramData.data_size
         Record start address is stored in mProgramData.bank and
         mProgramData.addr_offs.
RETURNS: FALSE for programming error, TRUE for ok
         If error, sdo_abort_code is set.
**************************************************************************/
static BOOLEAN FLASH_Program_Data_Received (
  UNSIGNED32 *sdo_abort_code
  )
{
  BOOLEAN    return_val  = TRUE;
  BOOLEAN    prog_result = TRUE;
  UNSIGNED8  data_offs   = 0U;      // Offset into gNodeStatus.program_data[]. Program data always starts at gNodeStatus.program_data[0].
  UNSIGNED8  page        = 0U;
  UNSIGNED16 page_addr   = 0x0000U;
  UNSIGNED16 end_addr    = 0x0000U;
  BOOLEAN    twopages    = FALSE;
  UNSIGNED8  len         = 0U;

  // End of File: Close programming, write last page
  if ( mParseStateMachine.file_close )
  {
    // Serve the watchdog in between, otherwise reset might occur
    COP_Serve();

    FLASH_Program (0, 0, TRUE, 0, 0, 0);
    FLASH_Init_Program_Data();   // Prepare for new download
  }
  else
  { // Data record to program

    // Check address and bank for validity
    if ( BITSET(mProgramData.addr_offs,0) ||    // Odd addresses in the records are not allowed!
         (mProgramData.bank >= FLASH_NUM_64K_BANKS) )  // Bank beyond flash size?
    { // Error: Don't go any further
      *sdo_abort_code = SDO_ABORT_ACCINCOMP;
      return_val      = FALSE;
    }
    else
    {
      // Calculate the end address offset
      end_addr = mProgramData.addr_offs + mProgramData.data_size - 1;

      // Check if address lies within the bootloader area. If yes => Error
      if ( (mProgramData.bank == (UNSIGNED8)(BL_START_ADR >> 16))  &&
           ((end_addr+1) > (UNSIGNED16)(BL_START_ADR & 0x0000FFFFUL))  )
      {
        *sdo_abort_code = SDO_ABORT_LOCAL;
        return_val      = FALSE;
      }
      else
      { // Now we can go ahead with programming

        // Calculate page number from program data start address
        page      = (mProgramData.addr_offs >> FLASH_PAGE_ADR_SHIFT);

        // Calculate start address for page
        page_addr = ((UNSIGNED16)page << FLASH_PAGE_ADR_SHIFT);

        // If a record crosses a 256-byte page boundary we have to check for two
        // pages to be erased and programmed.

        // Check if start and end address offsets lie in different pages.
        // If yes, 'twopages' becomes TRUE.
        twopages = (((UNSIGNED8)(mProgramData.addr_offs >> FLASH_PAGE_ADR_SHIFT) != (UNSIGNED8)(end_addr >> FLASH_PAGE_ADR_SHIFT)));

        // If the record crosses a page boundary, the length has to be shortened
        // to stay within the first page. Otherwise, the whole record can be
        // programmed in one go.
        if ( twopages )
        {
          len = ((UNSIGNED16)(page+1) << FLASH_PAGE_ADR_SHIFT) - mProgramData.addr_offs;
        }
        else
        {
          len = mProgramData.data_size;
        }

        // Program the record if it fits entirely into a single page, or if not,
        // program the first part. Note that the data index into gNodeStatus.program_data[]
        // (4th parameter) is always 0 because the programming data starts in the beginning.
        prog_result = FLASH_Program (mProgramData.bank, page, FALSE, 0, len, (UNSIGNED8)(mProgramData.addr_offs-page_addr));

        if (!prog_result)
        { // Programming error: Abort
          *sdo_abort_code = SDO_ABORT_LOCAL;
          return_val      = FALSE;
        }
        else if ( twopages )
        { // No error: Go ahead
          // If record crosses page boundary we need to write/set-up second page, too!

          page++;              // Next part needs to go into the next page

          // The second part of the data within gNodeStatus.program_data[] starts behind the
          // first part which is already programmed.
          data_offs  += len;

          // The remainder to program is the total length minus the data length already
          // programmed.
          len        = mProgramData.data_size - len;

          // If there is an UNSIGNED16 overflow when calculating the end address, this means
          // that the load hex record crosses a bank boundary (bank violation)!
          // This should never happen, but some tools (IAR) apparently do it.
          // In order to deal with this, we need to write the remainder of the record into the
          // next bank.
          if (end_addr < mProgramData.addr_offs)
          {
            mProgramData.bank++;
          }

          // Program the second part of the record. Note that it always starts at a new
          // physical page, so the offset into the flash page buffer (6th parameter) is
          // always 0!
          prog_result = FLASH_Program (mProgramData.bank, page, FALSE, data_offs, len, 0);

          if (!prog_result)
          { // Programming error: Abort
            *sdo_abort_code = SDO_ABORT_LOCAL;
            return_val = FALSE;
          }
          else
          { // do nothing
            ;
          }
        }
        else
        { // do nothing
          ;
        }
      }
    }
  }

  return (return_val);
}



/**************************************************************************
DOES:    Initialize programming state machine
RETURNS:
**************************************************************************/
static void FLASH_Init_Program_Data (
  void
  )
{
  mParseStateMachine.parse_state   = PARSESTATE_START;
  mParseStateMachine.checksum      = 0x00U;
  mParseStateMachine.record_type   = 0;
  mParseStateMachine.hex_usba      = 0x00000000UL;
  mParseStateMachine.file_close    = FALSE;

  mProgramData.valid               = FALSE;
  mProgramData.bank                = 0x00U;
  mProgramData.addr_offs           = 0x00U;
  mProgramData.data_cnt            = 0;
  mProgramData.data_size           = 0;

  // Initialize variables for flash programming scheme
  mFlashState.last_erased_bank = 0;
  mFlashState.last_erased_page = 0;

  // The flash is opened only once after reset, before the flash is programmed,
  // or after a complete hex file has been programmed.
  mFlashState.opened = TRUE;

  // This flag is initialized to 0 and only gets set to 1 to enable protected SPM code
  mFlashState.enabled = 0x00;

  return;
}




/**************************************************************************
 PUBLIC FUNCTIONS
***************************************************************************/

/**************************************************************************
DOES:    Lock the fuse bits so that the bootloader is locked forever
PARAMETERS: -
RETURNS: -
**************************************************************************/
void FLASH_Lock_Fusebits (
  void
  )
{
  asm( "ldi r30,1");    //
  asm( "ldi r31,0");    //  Z Pointer  = 0x0001 not used but for future compatibility

  // RAMPZ always points to high bank in this application already

  asm( "ldi r17,239");
  asm( "mov r0,r17");   // R0 = 0xEF => BLB11 = 0 => Set fuse to protect bootloader

  // Program protection fuse byte via protected SPM
  mFlashState.enabled = 0x01U;
  FLASH_Protected_SPM(BIT(SPMEN) | BIT(BLBSET), 0x02U);
  mFlashState.enabled = 0x00U;

  return;
}



/**************************************************************************
DOES:    Read the fuse bits and return locking status
PARAMETERS: -
RETURNS: TRUE, if bootloader is locked
**************************************************************************/
// No optimization for this function so that we can ensure linear code.
__attribute__((optimize("O0")))
BOOLEAN FLASH_Read_Fusebits (
  void
  )
{
  UNSIGNED8 locks = 0x00;
  BOOLEAN   return_val = FALSE;

  asm( "LDI  R30,  1");
  asm( "LDI  R31,  0");    //  Z Pointer = 0x0001 to read lock bits
  asm( "LDI  R16,  9");
  asm( "OUT  0x37, R16");    // Set BLBSET and SPMEN to read lock bits
  asm( "ELPM R17,  Z" );     // Load result into R17 (locks)

  if ((locks & 0x10) == 0x00) // If BLB11 is 0, lock is engaged
  {
    return_val = TRUE;
  }
  else
  {
    return_val = FALSE;
  }

  return (return_val);
}



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
UNSIGNED8 FLASH_Read_Byte (
  UNSIGNED32 addr
  )
{
  UNSIGNED16 read_word;
  UNSIGNED8  return_val;

  // Read word - ignore bit 0
  read_word = FLASH_Read_Word(addr & 0xFFFFFFFEUL);

  if (BITSET((UNSIGNED8)addr,0))
  { // Odd address - upper byte
    return_val = (UNSIGNED8)(read_word >> 8);
  }
  else
  { // Even address - lower byte
    return_val = (UNSIGNED8)(read_word & 0x00FFU);
  }

  return(return_val);
}



/**************************************************************************
DOES:    Initialize the Flash and EEPROM hardware and programming
         variables/flags.
RETURNS:
**************************************************************************/
void FLASH_Init (
  void
  )
{
  // Clock prescaler Reset (factor 1) and clear SREG.I
  CLKPR = 0x80;
  CLKPR = 0x00;
  SREG  = 0x00; // disable all interrupts

  // Set RAMPZ register to access the same bank of program memory
  // where this program is located
  RAMPZ = BL_BANK;

  // Initialize state variables for hex record parser and
  // programming status and data.
  FLASH_Init_Program_Data();

  return;
}



/**************************************************************************
DOES:    Verifies the EEPROM checksum and if the checksum is OK, reads the
         node parameters: Node ID, baudrate indicator and serial number.
PARAMETERS: -
RETURNS: FALSE for checksum error, TRUE for checksum OK and parameter read.
         *checksums bits 0..7 contain the EEPROM checksum and bits 8..15
         the calcuated checksum.
**************************************************************************/
BOOLEAN FLASH_EEPROM_Read_Params (
  UNSIGNED16 *checksums
  )
{
  BOOLEAN    return_val = FALSE;
  UNSIGNED16 adr        = 0x0000U;
  INTEGER8   i          = 0;
  UNSIGNED8  check      = 0x00U;
  UNSIGNED8  eeprom_copy[BL_EEP_CHECK+1];

  // Read EEPROM into local buffer first
  for (adr=EEPROM_START,i=0; adr < (EEPROM_START+sizeof(eeprom_copy)); adr++, i++)
  {
    eeprom_copy[i] = FLASH_EEPROM_ReadByte(adr);
  }

  // Calculate checksum
  for (i=0; i < BL_EEP_CHECK; i++)
  {
    check = FLASH_Calculate_CRC8(eeprom_copy[i]);
  }

  // Return the calculated and EEPROM checksum
  *checksums = ( ((UNSIGNED16)check << 8) | (UNSIGNED16)eeprom_copy[BL_EEP_CHECK] );

  // Now compare with stored checksum
  if (check == eeprom_copy[BL_EEP_CHECK])
  { // Checksum ok

    return_val = TRUE;

    if ((eeprom_copy[BL_EEP_NID] > 0) && (eeprom_copy[BL_EEP_NID] < 128))
    {
      gNodeStatus.node_id = eeprom_copy[BL_EEP_NID];
    }
    else
    { // Out-of-range? Use fallback Node ID
      gNodeStatus.node_id = NodeID_INIT;
    }

    if (eeprom_copy[BL_EEP_BAUD] == 125)
    {
      gNodeStatus.baudrate = TRUE;  // Alt. baudrate (125 kbps)
    }
    else
    {
      gNodeStatus.baudrate = FALSE; // Regular baudrate (50 kbps)
    }

    // Convert little endian serial number from EEPROM to UNSIGNED32
    gNodeStatus.serial = 0;
    for (i = (BL_EEP_OBJ_1018_4+3); i >= BL_EEP_OBJ_1018_4; i--)
    {
      gNodeStatus.serial <<= 8;
      gNodeStatus.serial  |= eeprom_copy[i];
    }
  }
  else
  { // Checksum not correct: Use defaults
    gNodeStatus.node_id  = NodeID_INIT;
    gNodeStatus.baudrate = FALSE; // Regular baudrate (50 kbps)
    gNodeStatus.serial   = 0xFFFFFFFFUL;
  }

  return (return_val);
}



/**************************************************************************
DOES:    Calculate checksum over application flash area with TCP/IP method:
         All words are added, then all carries are added, then calculate
         1s complement.
RETURNS: TRUE if checksum matches and a valid application can be executed.
         *checksum contains the calculated checksum
**************************************************************************/
BOOLEAN FLASH_Checksum_OK (
  UNSIGNED32 start_adr,       // Start address for checksumming
  UNSIGNED32 end_adr,         // End address for checksumming
  UNSIGNED32 checksum_adr,    // Checksum address
  UNSIGNED16 *checksum        // Returns the calculated checksum
  )
{
  BOOLEAN    return_val = FALSE;
  UNSIGNED32 adr        = 0x00000000UL;
  UNSIGNED32 sum        = 0x00000000UL;
  UNSIGNED16 stsum      = 0x0000U;
  UNSIGNED16 val16      = 0x0000U;

  *checksum = 0x0000U;

  // Checksum the code area
  for ( adr = start_adr; adr < end_adr; adr += 2 )
  {
    // Serve the watchdog in between, otherwise reset will occur
    if ((UNSIGNED8)adr == 0x80)
    {
      COP_Serve();
    }

    // Read flash memory word-wise
	val16 = pgm_read_word_far(adr);

    // Detect and save stored checksum value
    if ( adr == checksum_adr )
    {
      stsum = val16;   // Save the stored checksum
      val16 = 0xFFFFU; // For the algorithm the checksum is "empty" flash
    }

    sum += val16;
  }
  
  // Set RAMPZ register to access the same bank of program memory
  // where this program is located (direct flash access may have
  // altered this register).
  RAMPZ = BL_BANK;

  // Add all carries
  sum += sum>>16;
  // 1s complement of the result
  sum  = ~sum;

  *checksum = (UNSIGNED16)(sum & 0x0000FFFFUL);

#ifdef SHOW_CRC
  CANHW_Send_Debug(0x00CC);
  CANHW_Send_Debug(stsum);
  CANHW_Send_Debug(sum);
#endif /* SHOW_CRC */  

  // Compare calculated with stored checksum and return result
  return_val = ( *checksum == stsum );

  return (return_val);
}



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
BOOLEAN FLASH_Parse_Program_Hex_Data (
  UNSIGNED8  load_data_length,  // Length of fragment of load data
  UNSIGNED32 *sdo_abort_code
  )
{
  BOOLEAN    return_val  = TRUE;
  UNSIGNED8  i           = 0U;

  // cannot do anything with a fragment length of zero
  if ( load_data_length == 0 )
  {
    *sdo_abort_code = SDO_ABORT_TRANSFER;
    return_val      = FALSE;
  }
  else
  {
    // For each received byte in the stream perform one loop of the state machine,
    // as long as no error occurs.
    for ( i = 1; (i < load_data_length+1) && (return_val == TRUE); i++ )
    {
      switch ( mParseStateMachine.parse_state )
      {

        // The first byte is the start of record
        case PARSESTATE_START:

          // Initialize program data pointer and other variables for new record processing
          mProgramData.data_cnt          = 0;
          mProgramData.valid             = FALSE;
          mProgramData.bank              = 0;
          mParseStateMachine.record_type = 0;

          // The first byte is the length of the record
          mProgramData.data_size         = gRxCAN.BUF[i];
          mParseStateMachine.checksum    = gRxCAN.BUF[i];

          // Next byte is the address high byte
          mParseStateMachine.parse_state = PARSESTATE_ADDRH;
          break;

        // Get the address high byte
        case PARSESTATE_ADDRH:

          mProgramData.addr_offs         = gRxCAN.BUF[i];
          mParseStateMachine.checksum   += gRxCAN.BUF[i];

          // Next byte is the address low byte
          mParseStateMachine.parse_state = PARSESTATE_ADDRL;
          break;

        // Get the address low byte
        case PARSESTATE_ADDRL:

          mProgramData.addr_offs       <<= 8;
          mProgramData.addr_offs        |= gRxCAN.BUF[i];
          mParseStateMachine.checksum   += gRxCAN.BUF[i];

          // Next byte is the record type
          mParseStateMachine.parse_state = PARSESTATE_TYPE;
          break;

        // Get the record type
        case PARSESTATE_TYPE:

          mParseStateMachine.record_type = gRxCAN.BUF[i];
          mParseStateMachine.checksum   += gRxCAN.BUF[i];

          switch (mParseStateMachine.record_type)
          {
            case 0x00:   // Normal data record
            case 0x01:   // End-of-file record
            case 0x02:   // Extended segment address record
              break;

            default:     // Record type not supported
              *sdo_abort_code = SDO_ABORT_TRANSFER;
              return_val      = FALSE;
              break;
          }

          // If the record contains data, we need to get it, otherwise we proceed
          // straight to the checksum.
          if (mProgramData.data_size > 0)
          {
            mParseStateMachine.parse_state  = PARSESTATE_DATA;
          }
          else
          {
            mParseStateMachine.parse_state  = PARSESTATE_CHECKSUM;
          }
          break;

        // Get programming data
        case PARSESTATE_DATA:
          mProgramData.data[mProgramData.data_cnt]  = gRxCAN.BUF[i];  // Save data in buffer
          mParseStateMachine.checksum              += gRxCAN.BUF[i];
          mProgramData.data_cnt++;

          // If the last data byte for the record has been received, get the checksum,
          // otherwise get the next data byte.
          if ( mProgramData.data_cnt == mProgramData.data_size )
          {
            mParseStateMachine.parse_state  = PARSESTATE_CHECKSUM;
          }
          else
          {
            mParseStateMachine.parse_state  = PARSESTATE_DATA;
          }
          break;

        // End of record: Get checksum and compare with the calculated one. Program data records.
        case PARSESTATE_CHECKSUM:

          // In any case, this record is finished and we can proceed to the next
          mParseStateMachine.parse_state   = PARSESTATE_START;

          if (return_val != FALSE)
          { // Only proceed if another error didn't already invalidate record

            // For checksumming, take two's complement, disregard all overflows
            mParseStateMachine.checksum = ~mParseStateMachine.checksum + 1;

            // Checksum check. If no match => Error
            if ( gRxCAN.BUF[i] != mParseStateMachine.checksum )
            {
              *sdo_abort_code = SDO_ABORT_TRANSFER;
              return_val      = FALSE;
            }
            else if (mParseStateMachine.record_type == 2)
            { // If extended segment address record, get the segment address from the data field
              mParseStateMachine.hex_usba  = ((UNSIGNED32)mProgramData.data[0] << 12) + ((UNSIGNED32)mProgramData.data[1] << 4);
            }
            else
            {
              if (mParseStateMachine.record_type == 1)
              { // If end-of-file record, we are done
                mParseStateMachine.file_close = TRUE;
              }
              else
              { // Otherwise, calculate the physical address
                UNSIGNED32 val32;

                val32 = mParseStateMachine.hex_usba + mProgramData.addr_offs;  // Full absolute address
                mProgramData.addr_offs   = (UNSIGNED16)(val32 & 0x0000FFFFUL);
                mProgramData.bank        = (UNSIGNED8)((val32 >> 16) & 0x00000001UL); // Only Banks 0 and 1 supported
                mProgramData.valid       = TRUE;
              }
              // Now program the new data or close programming
              return_val = FLASH_Program_Data_Received(sdo_abort_code);
            }
          }
          break;

        default:  // never be here
          break;
      }
    }
  }

  return (return_val);
}

/***************************************************************************
END OF FILE
***************************************************************************/
