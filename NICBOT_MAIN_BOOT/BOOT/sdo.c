/**************************************************************************
PROJECT:     CANopen Bootloader for Atmel AT90CAN128 AVR Microcontroller
MODULE:      SDO.C
CONTAINS:    Implementation of the Service Data Object access to the OD
ENVIRONMENT: Compiled and tested with IAR EW for AVR on
             Atmel STK500/501 with AT90CAN128
             Compiler Version 5.11b
COPYRIGHT:   This version Embedded Systems Academy, Inc.
             Developed by Embedded Systems Academy, Inc.
VERSION:     $LastChangedDate: 2016-02-26 20:53:15 +0100 (Fri, 26 Feb 2016) $
             $LastChangedRevision: 3576 $
***************************************************************************/

#include "types.h"
#include "userdefs.h"
#include "main.h"
#include "canhw.h"
#include "flash.h"
#include "sdo.h"


/**************************************************************************
 LOCAL DEFINITIONS
***************************************************************************/

// SDO first byte values and masks for commands and confirmations
#define SDO_FBT_TOGGLEBIT          0x10U     // Toggle bit in SDO command byte
#define SDO_FBT_ABORT              0x80U     // Abort command
#define SDO_FBT_CCS_MASK           0xE0U     // Expedited command: Command specifier
#define SDO_FBT_EXP_LEN_MASK       0x0CU     // Expedited command: Data length
#define SDO_FBT_EXP_BITS_MASK      0x03U     // Expedited request protocol bits
#define SDO_FBT_SEG_LEN_MASK       0x0EU     // Segmented command: Data length
#define SDO_FBT_SEG_S_MASK         0x01U     // Segmented command: Length-indicated bit
#define SDO_FBT_EXP_READ_CMD       0x40U     // Expedited read command
#define SDO_FBT_EXP_WRITE_CMD      0x23U     // Expedited write command
#define SDO_FBT_SEG_NEW_READ_CMD   0x40U     // New segmented read command
#define SDO_FBT_SEG_NEW_WRITE_CMD  0x20U     // New segmented write command
#define SDO_FBT_SEG_READ_CMD       0x60U     // Segmented read command
#define SDO_FBT_SEG_WRITE_CMD      0x00U     // Segmented write command
#define SDO_FBT_EXP_READ_CONF      0x43U     // Expedited read response
#define SDO_FBT_EXP_WRITE_CONF     0x60U     // Expedited write response
#define SDO_FBT_SEG_READ_CONF      0x00U     // Segmented read response
#define SDO_FBT_SEG_WRITE_CONF     0x20U     // Segmented write response
#define SDO_FBT_SEG_NEW_READ_CONF  0x40U     // New segmented read response


typedef enum {
  SEG_STAT_NONE,                // no segmented transfer
  SEG_STAT_WRITE,               // sdo segmentation in progress (download/write)
  SEG_STAT_READ                 // sdo segmentation in progress (upload/read)
} SDO_TransType;


// Type of supported Object Dictionary entries: Expedited/segmented transfer
typedef enum {
  OD_NOENTRY,                   // Object Dictionary entry doesn't exist
  OD_EXP_RO,                    // Object Dictionary entry is <= 4 bytes and read-only
  OD_EXP_WO,                    // Object Dictionary entry is <= 4 bytes and write-only
  OD_EXP_RW,                    // Object Dictionary entry is <= 4 bytes and read-write
  OD_SEG_RO,                    // Object Dictionary entry is > 4 bytes and read-only
  OD_SEG_WO,                    // Object Dictionary entry is > 4 bytes and write-only
  OD_SEG_RW                     // Object Dictionary entry is > 4 bytes and read-write
} SDO_ODEntryType;


// Type of supported SDO requests
typedef enum {
  SDO_UNKNOWN,                  // SDO unknown/unsupported access
  SDO_INIT_DOWN,                // SDO initiate download (segmented/expedited)
  SDO_INIT_UP,                  // SDO initiate upload (segmented/expedited)
  SDO_SEG_DOWN,                 // SDO download segment
  SDO_SEG_UP                    // SDO upload segment
} SDO_RequestType;


// This structure holds all SDO segmented transmission specific variables
typedef struct
{
  SDO_TransType   trans_type;    // type of transfer (expedited, segmented, read/write)
  UNSIGNED8       seg_toggle;    // toggle bit for segmented transfer
  UNSIGNED16      seg_index;     // od entry index and subindex for ongoing segmented transfer
  UNSIGNED8       seg_subindex;
} SDO_TransStatusType;

static SDO_TransStatusType mSDO_TransStatus;


/**************************************************************************
 EXTERNAL REFERENCES
***************************************************************************/


/**************************************************************************
 PUBLIC DEFINITIONS
***************************************************************************/


/**************************************************************************
 LOCAL FUNCTION PROTOTYPES
***************************************************************************/

static SDO_ODEntryType SDO_OD_Get_Entry (
  UNSIGNED16  index,     // Index and
  UNSIGNED8   subindex,  // Sub-index of the entry
  UNSIGNED8   *length    // Returns length of entry. 0 if unknown (domain).
  );

static BOOLEAN SDO_OD_Read_Entry (
  UNSIGNED16  index,     // Index and
  UNSIGNED8   subindex   // Sub-index of the entry
  );

static SDO_ReturnType SDO_OD_Write_Entry (
  UNSIGNED16  index,     // Index and
  UNSIGNED8   subindex,  // Sub-index of the entry
  UNSIGNED8   length     // Number of bytes to write
  );

static BOOLEAN SDO_OD_Read_Segment (
  UNSIGNED8   *length,   // Number of bytes read into buffer
  BOOLEAN     open       // Open read channel
  );

static BOOLEAN SDO_OD_Write_Segment (
  UNSIGNED8   length,         // Number of bytes from buffer to write
  BOOLEAN     open,           // Open write channel
  UNSIGNED32  *sdo_abort_code // If error, returns a specific abort code
  );

static void SDO_Write_DWord (
  UNSIGNED32 dat
  );

static void SDO_Send_DWord (
  UNSIGNED32 dat
  );

static void SDO_Send_SDO_Abort (
  UNSIGNED32 error_code   // 4 byte SDO abort error code
  );

static void SDO_Read_Confirm (
  UNSIGNED8 len  // Length of SDO data
  );

static void SDO_Write_Confirm (
  void
  );

static SDO_ReturnType SDO_Handle_Exp_Request (
  UNSIGNED16      index,       // Requested OD index
  UNSIGNED8       subindex,    // Requested OD subindex
  UNSIGNED8       length,      // Number of bytes to write or read
  SDO_ODEntryType access_type  // Type of entry (ro/wo/rw)
  );

static SDO_ReturnType SDO_Handle_Seg_Read (
  void
  );

static SDO_ReturnType SDO_Handle_Seg_Write (
  void
  );

static SDO_ReturnType SDO_Handle_Seg_Request (
  UNSIGNED16      index,       // Requested OD index
  UNSIGNED8       subindex,    // Requested OD subindex
  SDO_ODEntryType access_type  // Type of entry (ro/wo/rw)
  );


/**************************************************************************
 LOCAL FUNCTIONS
***************************************************************************/

/**************************************************************************
DOES:    Gets information about an Object Dictionary entry: Which type of
         entry, what access modes are supported and how long it is.
RETURNS: Access type of Object Dictionary entry
GLOBALS: -
**************************************************************************/
static SDO_ODEntryType SDO_OD_Get_Entry (
  UNSIGNED16  index,     // Index and
  UNSIGNED8   subindex,  // Sub-index of the entry
  UNSIGNED8   *length    // Returns length of entry. 0 if unknown (domain).
  )
{
  SDO_ODEntryType return_val;

  return_val = OD_NOENTRY;  // Not found by default

  switch (index)
  {
    case 0x1000:                 // Device Type [1000h,0]
      if (subindex == 0)
      {
        return_val = OD_EXP_RO;  // read-only
        *length    = 4;          // UNSIGNED32
      }
      else
      { // Nothing to do here
        ;
      }
      break;

    case 0x1001:                 // Error Register [1001h,0]
      if (subindex == 0)
      {
        return_val = OD_EXP_RO;  // read-only
        *length    = 1;          // UNSIGNED8
      }
      else
      { // Nothing to do here
        ;
      }
      break;

    case 0x1003:                 // Pre-defined Error Field [1003h,0-NUM_ERRORS_MAX]
      if (subindex == 0)
      {
        return_val = OD_EXP_RW;  // read-write
        *length    = 1;          // UNSIGNED8 (Number of Subentries)
      }
      else if (subindex <= NUM_ERRORS_MAX)  // All errors in the error list are readable (even unused ones)
      {
        return_val = OD_EXP_RO;  // read-only
        *length    = 4;          // UNSIGNED32
      }
      else
      { // Nothing to do here
        ;
      }
      break;

    case 0x1018:                 // Identity Object [1018h,0-4]
      if (subindex == 0)
      {
        return_val = OD_EXP_RO;  // read-only
        *length    = 1;          // UNSIGNED8 (Number of Subentries)
      }
      else if (subindex < 5)
      {
        return_val = OD_EXP_RO;  // read-only
        *length    = 4;          // UNSIGNED32
      }
      else
      { // Nothing to do here
        ;
      }
      break;

    case 0x1F50:                 // Download Program Data [1F50h,0-1]
      if (subindex == 0)
      {
        return_val = OD_EXP_RO;  // read-only
        *length    = 1;          // UNSIGNED8 (Number of Subentries)
      }
      else if (subindex == 1)
      {
        return_val = OD_SEG_RW;  // read-write
        *length    = 0;          // unknown (DOMAIN)
      }
      else
      { // Nothing to do here
        ;
      }
      break;

    case 0x1F51:                 // Program Control [1F51h,0-1]
      if (subindex == 0)
      {
        return_val = OD_EXP_RO;  // read-only
        *length    = 1;          // UNSIGNED8 (Number of Subentries)
      }
      else if (subindex == 1)
      {
        return_val = OD_EXP_RW;  // read-write
        *length    = 1;          // UNSIGNED8
      }
      else
      { // Nothing to do here
        ;
      }
      break;

    case 0x2FFF:                 // Special Functions [2FFFh,0-1]
      if (subindex == 0)
      {
        return_val = OD_EXP_RO;  // read-only
        *length    = 1;          // UNSIGNED8 (Number of Subentries)
      }
      else if (subindex == 1)
      {
        return_val = OD_EXP_RW;  // read-write
        *length    = 4;          // UNSIGNED32
      }
      else
      { // Nothing to do here
        ;
      }
      break;

    default:
      break;
  }

  return (return_val);
}



/**************************************************************************
DOES:    Reads expedited OD entries and writes data into response buffer
RETURNS: TRUE if entry could be read, FALSE otherwise.
GLOBALS: gTxCAN - CAN message with SDO response
**************************************************************************/
static BOOLEAN SDO_OD_Read_Entry (
  UNSIGNED16  index,     // Index and
  UNSIGNED8   subindex   // Sub-index of the entry
  )
{
  BOOLEAN return_val;

  return_val = FALSE;  // Not found by default

  switch (index)
  {
    case 0x1000:                 // Device Type [1000h,0]
      if (subindex == 0)
      {
        SDO_Write_DWord(BL_OBJ_1000_0);
        return_val = TRUE;       // Entry found
      }
      else
      { // Nothing to do here
        ;
      }
      break;

    case 0x1001:                 // Error Register [1001h,0]
      if (subindex == 0)
      {
        SDO_Write_DWord((UNSIGNED32)gNodeStatus.error_register);
        return_val = TRUE;       // Entry found
      }
      else
      { // Nothing to do here
        ;
      }
      break;

    case 0x1003:                    // Pre-defined error field [1003h,0-NUM_ERRORS_MAX]
      if (subindex == 0)
      {
        SDO_Write_DWord((UNSIGNED32)gNodeStatus.errorfield_errors); // Number of Subentries
        return_val = TRUE;       // Entry found
      }
      else if (subindex <= gNodeStatus.errorfield_errors)
      { // Used error: Return the error code
        UNSIGNED32 error_field;

        // Calculate the complete error field from error code (bits 0..15) and
        // additional error inf (bits 16..31).
        error_field   = (UNSIGNED32)gNodeStatus.errorfield_addl_info[subindex-1];
        error_field <<= 16;
        error_field  |= (UNSIGNED32)gNodeStatus.errorfield_errorlist[subindex-1];

        SDO_Write_DWord(error_field); // Error Field
        return_val = TRUE;            // Entry found
      }
      else if (subindex <= NUM_ERRORS_MAX)
      { // Unused error: Return 0
        SDO_Write_DWord(0x00000000UL); // Error Field
        return_val = TRUE;             // Entry found
      }
      else
      { // Entry not found - do nothing
        ;
      }
      break;

    case 0x1018:                    // Identity Object [1018h,0-4]
      switch (subindex)
      {
        case 0:
          SDO_Write_DWord((UNSIGNED32)4); // '4': Number of Subentries
          return_val = TRUE;       // Entry found
          break;
        case 1:
          SDO_Write_DWord(BL_OBJ_1018_1); // Device Type
          return_val = TRUE;       // Entry found
          break;
        case 2:
          SDO_Write_DWord(BL_OBJ_1018_2); // Vendor ID
          return_val = TRUE;       // Entry found
          break;
        case 3:
          SDO_Write_DWord(BL_OBJ_1018_3); // Product Code
          return_val = TRUE;       // Entry found
          break;
        case 4:
          SDO_Write_DWord(gNodeStatus.serial); // Serial Number
          return_val = TRUE;       // Entry found
          break;
        default:
          break;
      }
      break;

    case 0x1F50:                 // Download Program Data [1F50h,0-1]
      if (subindex == 0)
      {
        SDO_Write_DWord((UNSIGNED32)1); // '1': Number of Subentries
        return_val = TRUE;       // Entry found
      }
      else
      { // Nothing to do here
        ;
      }
      break;

    case 0x1F51:                 // Program Control [1F51h,0-1]
      if (subindex == 0)
      {
        SDO_Write_DWord((UNSIGNED32)1); // '1': Number of Subentries
        return_val = TRUE;       // Entry found
      }
      else if (subindex == 1)
      {
        SDO_Write_DWord((UNSIGNED32)0); // '0': Program stopped
        return_val = TRUE;       // Entry found
      }
      else
      { // Nothing to do here
        ;
      }
      break;

    case 0x2FFF:                 // Special Functions [2FFFh,0-1]
      if (subindex == 0)
      {
        SDO_Write_DWord((UNSIGNED32)1); // '1': Number of Subentries
        return_val = TRUE;       // Entry found
      }
      else
      { // On read, return the protection status of the bootloader
        if (FLASH_Read_Fusebits())
        {
          SDO_Write_DWord(0x746F7270UL);  // return "prot"
        }
        else
        {
          SDO_Write_DWord(0x656E6F6EUL);  // return "none"
        }
        return_val = TRUE;       // Entry found
      }
      break;

    default:
      break;
  }

  return (return_val);
}



/**************************************************************************
DOES:    Writes expedited OD entries from SDO receive buffer
RETURNS: SDO_OD_NOTFOUND - Requested OD entry not found
         SDO_ACCESS_OK - Requested OD entry found and data written
         SDO_ACCESS_FAILED - Request failed, e.g. data length wrong
         SDO_WRITE_WRONGDATA - Write attempt with data out-of-range
GLOBALS: gRxCAN - CAN message with SDO request
**************************************************************************/
static SDO_ReturnType SDO_OD_Write_Entry (
  UNSIGNED16  index,     // Index and
  UNSIGNED8   subindex,  // Sub-index of the entry
  UNSIGNED8   length     // Number of bytes to write
  )
{
  SDO_ReturnType return_val;

  return_val = SDO_OD_NOTFOUND;  // Not found by default

  switch (index)
  {
    case 0x1003:              // Reset Pre-defined Error Field [1003h,0]
      if (subindex == 0)
      {
        if (length == 1)
        {
          if (gRxCAN.BUF[4] == 0)  // '0' written => Reset error list and error status
          {
            gNodeStatus.errorfield_errors = 0;
            gNodeStatus.error_register    = ERRREG_NOERROR;
            return_val = SDO_ACCESS_OK;       // Entry found
          }
          else
          {
            return_val = SDO_WRITE_WRONGDATA;  // Found but wrong data
          }
        }
        else
        {
          return_val = SDO_ACCESS_FAILED;    // Found but wrong length
        }
      }
      else
      { // Not found
        ;
      }
      break;

    case 0x1F51:                 // Program Control [1F51h,0-1]
      if (subindex == 1)
      {
        if (length == 1)
        {
          if (gRxCAN.BUF[4] == 0)  // '0' written => Nothing
          {
            return_val = SDO_ACCESS_OK;       // Entry found
          }
          else if (gRxCAN.BUF[4] == 1)  // '1' written => Run application
          {
            gNodeStatus.run_mode = EXECMODE_RUN;
            return_val = SDO_ACCESS_OK;       // Entry found
          }
          else
          {
            return_val = SDO_WRITE_WRONGDATA;  // Found but wrong data
          }
        }
        else
        {
          return_val = SDO_ACCESS_FAILED;    // Found but wrong length
        }
      }
      else
      {  // Do nothing
        ;
      }
      break;

    case 0x2FFF:                 // Special Functions [2FFFh,0-1]
      if (subindex == 1)
      { // Only subentriy 1 can be written
        if (length == 4)
        {
          if ( (gRxCAN.BUF[4] == 'p') &&
               (gRxCAN.BUF[5] == 'r') &&
               (gRxCAN.BUF[6] == 'o') &&
               (gRxCAN.BUF[7] == 't') )
          { // "prot" written => Engage bootloader flash protection
            FLASH_Lock_Fusebits();
            return_val = SDO_ACCESS_OK;       // Entry found and written
          }
          else
          {
            return_val = SDO_WRITE_WRONGDATA;  // Found but wrong data
          }
        }
        else
        {
          return_val = SDO_ACCESS_FAILED;   // Found but wrong length
        }
      }
      else
      { // Not found
        ;
      }
      break;

    default:
      break;
  }

  return (return_val);
}



/**************************************************************************
DOES:    Reads one segment of a segmented OD entry into SDO transmit buffer
RETURNS: TRUE, if last segment was read, FALSE otherwise
GLOBALS: gTxCAN - CAN message with SDO response
         mSDO_TransStatus - Global configuration information
**************************************************************************/
static BOOLEAN SDO_OD_Read_Segment (
  UNSIGNED8   *length,   // Number of bytes read into buffer
  BOOLEAN     open       // Open read channel
  )
{
  static UNSIGNED32 address    = 0x00000000UL;
  BOOLEAN           return_val = FALSE;

  // mSDO_TransStatus contains the index and subindex of the segmented
  // transfer. Different processing for different entries could therefore
  // be handled here. However, if there is only one domain read entry
  // for the node, no check for different entries needs to be done here.

  if (open)
  {
    address = ADR_READ_START;
    *length = 0;
  }
  else
  { // Read next segment
    UNSIGNED8 i;

    for (i=1; (i < 8) && (address <= ADR_READ_END); i++, address++)
    {
      gTxCAN.BUF[i] = FLASH_Read_Byte(address);

      if (address == ADR_READ_END)
      { // Indicate last segment
        return_val = TRUE;
      }
      else
      { // do nothing
        ;
      }
    }

    // Return number of bytes actually read
    *length = i-1;
  }

  return (return_val);
}



/**************************************************************************
DOES:    Writes one segment from SDO receive buffer to a segmented OD entry
RETURNS: TRUE, if error during write, FALSE otherwise
         If error, sets sdo_abort_code
GLOBALS: gRxCAN - CAN message with SDO request
         mSDO_TransStatus - Global configuration information
**************************************************************************/
static BOOLEAN SDO_OD_Write_Segment (
  UNSIGNED8   length,         // Number of bytes from buffer to write
  BOOLEAN     open,           // Open write channel
  UNSIGNED32  *sdo_abort_code // If error, returns a specific abort code
  )
{
  BOOLEAN return_val = TRUE;

  // mSDO_TransStatus contains the index and subindex of the segmented
  // transfer. Different processing for different entries could therefore
  // be handled here. However, if there is only one domain write entry
  // for the node, no check for different entries needs to be done here.

  if (open)
  { // Nothing needs to be done for opening the write channel
    return_val = FALSE;
  }
  else
  {
    return_val = !FLASH_Parse_Program_Hex_Data(length, sdo_abort_code);
  }

  return (return_val);
}



/**************************************************************************
DOES:    Writes one data dword into SDO response buffer
GLOBALS: gTxCAN
**************************************************************************/
static void SDO_Write_DWord (
  UNSIGNED32 dat
  )
{
  UNSIGNED8 i;

  for (i=4; i<8; i++)
  {
    gTxCAN.BUF[i] = (UNSIGNED8)(dat & 0x000000FFUL);
    dat         >>= 8;    // Rotate the next byte into bit position 0-7
  }

  return;
}



/**************************************************************************
DOES:    Common send routine for SDO_Handler
         Send SDO response with one data dword.
GLOBALS: Assumes that gTxCAN.ID, LEN and BUF[0-3] are already set
**************************************************************************/
static void SDO_Send_DWord (
  UNSIGNED32 dat
  )
{
  SDO_Write_DWord(dat);

  // Transmit SDO Response message
  CANHW_Push_Message();

  return;
}



/**************************************************************************
DOES:    Common exit routine for SDO_Handler
         Generates an SDO abort message
GLOBALS: Assumes that gTxCAN.ID, LEN and BUF[1-3] are already set
**************************************************************************/
static void SDO_Send_SDO_Abort (
  UNSIGNED32 error_code   // 4 byte SDO abort error code
  )
{
  // Load SDO abort code into transmit buffer
  gTxCAN.BUF[0] = SDO_FBT_ABORT;

  SDO_Send_DWord(error_code);

  return;
}


/**************************************************************************
DOES:    Common send routine for SDO_Handler
         Send SDO read response (current transmit buffer) with variable
         data (1-4 bytes).
GLOBALS: Assumes that gTxCAN.ID, LEN and BUF[0-7] are already set
**************************************************************************/
static void SDO_Read_Confirm (
  UNSIGNED8 len  // Length of SDO data
  )
{
  // Load SDO Response into transmit buffer
  gTxCAN.BUF[0] = SDO_FBT_EXP_READ_CONF | ((UNSIGNED8)(4-len) << 2); // Expedited, len of data

  // Transmit SDO Response message
  CANHW_Push_Message();

  return;
}



/**************************************************************************
DOES: Common exit routine for SDO_Handler.
      Send SDO response with write confirmation.
      Assumes that gTxCAN.ID, LEN and BUF[1-3] are already set
**************************************************************************/
static void SDO_Write_Confirm (
  void
  )
{
  // Load SDO Response into transmit buffer
  gTxCAN.BUF[0] = SDO_FBT_EXP_WRITE_CONF; // Write response code

  // Clear unused bytes and send
  SDO_Send_DWord(0x00000000UL);

  return;
}



/**************************************************************************
DOES:    Verifies and processes expedited SDO Requests to OD entries
GLOBALS: gRxCAN - CAN message with SDO request
         gTxCAN - CAN message with SDO response
RETURNS: SDO_OD_NOTFOUND -   Requested OD entry not found
         SDO_ACCESS_OK -     Requested OD entry found and response sent
         SDO_ACCESS_FAILED - Request failed, SDO Abort sent
                             (e.g. READ-ONLY or WRITE-ONLY entry)
**************************************************************************/
static SDO_ReturnType SDO_Handle_Exp_Request (
  UNSIGNED16      index,       // Requested OD index
  UNSIGNED8       subindex,    // Requested OD subindex
  UNSIGNED8       length,      // Number of bytes to write or read
  SDO_ODEntryType access_type  // Type of entry (ro/wo/rw)
  )
{
  UNSIGNED8      len_req    = 0x00;            // Write: Length of data according to the request
  UNSIGNED8      cmd        = 0x00U;           // SDO command
  SDO_ReturnType return_val = SDO_OD_NOTFOUND; // Return value for this function

  // Get SDO expedited command:
  // Upper 3-bits are the actual command, lower two bits for expedited transfer
  cmd = gRxCAN.BUF[0] & (SDO_FBT_CCS_MASK | SDO_FBT_EXP_BITS_MASK);

  if (cmd == SDO_FBT_EXP_WRITE_CMD)
  { // Expedited write command with length indicated - only those writes are supported
    // Calculate number of valid bytes for writes: Bits 2 and 3 are number of bytes without data
    len_req = 4 - ((gRxCAN.BUF[0] & SDO_FBT_EXP_LEN_MASK) >> 2);

    if ((access_type == OD_EXP_WO) || (access_type == OD_EXP_RW))
    { // Write allowed
      if (len_req != length) // Check len
      { // Length of request doesn't match with definition
        SDO_Send_SDO_Abort(SDO_ABORT_TYPEMISMATCH);
        return_val = SDO_ACCESS_FAILED;
      }
      else
      {
        // Length ok: Write data
        return_val = SDO_OD_Write_Entry(index,subindex,length);

        if (return_val == SDO_ACCESS_OK)
        { // Successful write
          SDO_Write_Confirm();
        }
        else if (return_val == SDO_ACCESS_FAILED)
        { // Length of request doesn't match with definition
          SDO_Send_SDO_Abort(SDO_ABORT_TYPEMISMATCH);
        }
        else if (return_val == SDO_WRITE_WRONGDATA)
        { // Data was out-of-range
          SDO_Send_SDO_Abort(SDO_ABORT_TRANSFER);
        }
        else
        { // Object not found - nothing to do here
          ;
        }
      }
    }
    else
    { // Write not allowed
      SDO_Send_SDO_Abort(SDO_ABORT_READONLY);
      return_val = SDO_ACCESS_FAILED;
    }
  }
  else if (cmd == SDO_FBT_EXP_READ_CMD)
  { // Expedited read command?
    if ((access_type == OD_EXP_RO) || (access_type == OD_EXP_RW))
    { // Read allowed
      if (SDO_OD_Read_Entry(index,subindex))
      {
        SDO_Read_Confirm(length);
        return_val = SDO_ACCESS_OK;
      }
      else
      { // Object not found
        return_val = SDO_OD_NOTFOUND;
      }
    }
    else
    { // Read not allowed
      SDO_Send_SDO_Abort(SDO_ABORT_WRITEONLY);
      return_val = SDO_ACCESS_FAILED;
    }
  }
  else
  { // Command not allowed
    SDO_Send_SDO_Abort(SDO_ABORT_UNSUPPORTED);
    return_val = SDO_ACCESS_FAILED;
  }

  return (return_val);
}



/**************************************************************************
DOES:    Process segmented SDO Upload (Read) Requests to generic OD entries
RETURNS: SDO_ACCESS_OK -     Segment read and response sent
         SDO_ACCESS_FAILED - Request failed, SDO Abort sent
                             (e.g. protocol error)
GLOBALS: gRxCAN - CAN message with SDO request
         gTxCAN - CAN message with SDO response
         mSDO_TransStatus - Global configuration information
**************************************************************************/
static SDO_ReturnType SDO_Handle_Seg_Read (
  void
  )
{
  BOOLEAN        last       = FALSE;
  UNSIGNED8      len        = 0x00U;
  SDO_ReturnType return_val = SDO_OD_NOTFOUND; // Return value for this function

  // Check if command specifier is right
  if ((gRxCAN.BUF[0] & SDO_FBT_CCS_MASK) != SDO_FBT_SEG_READ_CMD)
  { // If wrong command specifier, abort here
    SDO_Send_SDO_Abort(SDO_ABORT_TRANSFER);
    return_val = SDO_ACCESS_FAILED;
  }
  else
  { // Check toggle bit
    if ((gRxCAN.BUF[0] & SDO_FBT_TOGGLEBIT) != (mSDO_TransStatus.seg_toggle & SDO_FBT_TOGGLEBIT))
    { // Report toggle error
      SDO_Send_SDO_Abort(SDO_ABORT_TOGGLE);
      return_val = SDO_ACCESS_FAILED;
    }
    else
    { // Read next segment into transmit buffer
      last = SDO_OD_Read_Segment(&len, FALSE);

      // Calculate the number of bytes in the segment that do NOT contain data
      len  = 7-len;

      // Now calculate contents of first byte
      gTxCAN.BUF[0] = SDO_FBT_SEG_READ_CONF | mSDO_TransStatus.seg_toggle | (UNSIGNED8)(len << 1);

      if (last)
      { // End of all segmented data reached
        SETBIT(gTxCAN.BUF[0],0);                      // Set "last segment" bit
        mSDO_TransStatus.trans_type  = SEG_STAT_NONE; // transfer completed
      }
      else
      {
        mSDO_TransStatus.seg_toggle ^= SDO_FBT_TOGGLEBIT; // Toggle the toggle bit
      }

      // Transmit SDO Response message
      CANHW_Push_Message();
      return_val = SDO_ACCESS_OK;    // Success
    }
  }

  return (return_val);
}



/**************************************************************************
DOES:    Process segmented SDO Download (Write) requests to generic OD entries
         mSDO_TransStatus -  Global configuration information
RETURNS: SDO_ACCESS_OK -     Segment written and response sent
         SDO_ACCESS_FAILED - Request failed, SDO Abort sent
                             (e.g. protocol error)
GLOBALS: gRxCAN - CAN message with SDO request
         gTxCAN - CAN message with SDO response
         mSDO_TransStatus - Global configuration information
**************************************************************************/
static SDO_ReturnType SDO_Handle_Seg_Write (
  void
  )
{
  BOOLEAN        error          = FALSE;
  UNSIGNED8      len            = 0x00U;
  UNSIGNED32     sdo_abort_code = 0x00000000UL;
  SDO_ReturnType return_val     = SDO_OD_NOTFOUND; // Return value for this function

  // Check if command specifier is right
  if ((gRxCAN.BUF[0] & SDO_FBT_CCS_MASK) != SDO_FBT_SEG_WRITE_CMD)
  { // If wrong command specifier, abort here
    SDO_Send_SDO_Abort(SDO_ABORT_TRANSFER);
    return_val = SDO_ACCESS_FAILED;
  }
  else if ((gRxCAN.BUF[0] & SDO_FBT_TOGGLEBIT) != mSDO_TransStatus.seg_toggle)
  { // Check toggle bit
    // Report toggle error
    SDO_Send_SDO_Abort(SDO_ABORT_TOGGLE);
    return_val = SDO_ACCESS_FAILED;
  }
  else
  {
    // Calculate number of bytes to write
    len   = 7 - ((gRxCAN.BUF[0] & SDO_FBT_SEG_LEN_MASK) >> 1);

    // Write next segment from receive buffer
    error = SDO_OD_Write_Segment(len, FALSE, &sdo_abort_code);

    if (error)
    { // Write error
      SDO_Send_SDO_Abort(sdo_abort_code);
      return_val = SDO_ACCESS_FAILED;
      mSDO_TransStatus.trans_type  = SEG_STAT_NONE; // transfer completed
    }
    else
    {
      if (BITSET(gRxCAN.BUF[0],0))
      { // if last segment, transfer is complete
        mSDO_TransStatus.trans_type  = SEG_STAT_NONE; // transfer completed
      }
      else
      { // do nothing
        ;
      }

      // Now calculate contents of first byte
      gTxCAN.BUF[0] = SDO_FBT_SEG_WRITE_CONF | mSDO_TransStatus.seg_toggle;

      mSDO_TransStatus.seg_toggle ^= SDO_FBT_TOGGLEBIT; // Toggle the toggle bit

      // Transmit SDO Response message
      CANHW_Push_Message();
      return_val = SDO_ACCESS_OK;    // Success
    }
  }

  return (return_val);
}



/**************************************************************************
DOES:    Verifies and processes segmented SDO Requests to OD entries
RETURNS: SDO_ACCESS_OK     - Requested OD entry found and response sent
         SDO_ACCESS_FAILED - Request failed, SDO Abort sent
                             (e.g. READ-ONLY or WRITE-ONLY entry)
GLOBALS: gRxCAN - CAN message with SDO request
         gTxCAN - CAN message with SDO response
         mSDO_TransStatus - Global configuration information
**************************************************************************/
static SDO_ReturnType SDO_Handle_Seg_Request (
  UNSIGNED16      index,       // Requested OD index
  UNSIGNED8       subindex,    // Requested OD subindex
  SDO_ODEntryType access_type  // Type of entry (ro/wo/rw)
  )
{
  UNSIGNED32     sdo_abort_code = 0x00000000UL;
  BOOLEAN        result         = FALSE;
  UNSIGNED8      len            = 0x00U;
  SDO_ReturnType return_val     = SDO_OD_NOTFOUND; // Return value for this function

  // Check if this is a new download (write, with size indicated) or upload (read) request
  if ( (gRxCAN.BUF[0] == (SDO_FBT_SEG_NEW_WRITE_CMD | SDO_FBT_SEG_S_MASK)) ||
       (gRxCAN.BUF[0] == SDO_FBT_SEG_NEW_READ_CMD)                         )
  {
    if (gRxCAN.BUF[0] == (SDO_FBT_SEG_NEW_WRITE_CMD | SDO_FBT_SEG_S_MASK))
    { // Segmented download (write, with size indicated)
      if ( (access_type == OD_SEG_WO) || (access_type == OD_SEG_RW) )
      {
        mSDO_TransStatus.trans_type = SEG_STAT_WRITE;

        // Prepare OD entry for writing: len is dummy variable
        len    = 0;
        result = SDO_OD_Write_Segment(len, TRUE, &sdo_abort_code);

        if (!result)
        { // Opening writing channel successful
          return_val = SDO_ACCESS_OK;  // Init transfer ok

          SDO_Write_Confirm();
        }
        else
        { // Could not open writing channel
          SDO_Send_SDO_Abort(sdo_abort_code);
          return_val = SDO_ACCESS_FAILED;
        }
      }
      else
      {
        SDO_Send_SDO_Abort(SDO_ABORT_READONLY);
        return_val = SDO_ACCESS_FAILED;
      }
    }
    else
    { // Segmented upload (read)
      if ( (access_type == OD_SEG_RO) || (access_type == OD_SEG_RW) )
      {
        mSDO_TransStatus.trans_type = SEG_STAT_READ;

        // Prepare OD entry for reading: len is dummy variable
        len    = 0;
        result = SDO_OD_Read_Segment(&len, TRUE);

        if (!result)
        { // Opening reading channel successful
          return_val = SDO_ACCESS_OK;  // Init transfer ok

          // Load SDO Response into transmit buffer
          gTxCAN.BUF[0] = SDO_FBT_SEG_NEW_READ_CONF; // Write response code
          // Clear unused bytes and send
          SDO_Send_DWord(0x00000000UL);
        }
        else
        { // Could not open reading channel
          SDO_Send_SDO_Abort(SDO_ABORT_ACCINCOMP);
          return_val = SDO_ACCESS_FAILED;
        }
      }
      else
      {
        SDO_Send_SDO_Abort(SDO_ABORT_WRITEONLY);
        return_val = SDO_ACCESS_FAILED;
      }
    }
  }
  else
  {
    SDO_Send_SDO_Abort(SDO_ABORT_UNSUPPORTED);
    return_val = SDO_ACCESS_FAILED;
  }

  if (return_val == SDO_ACCESS_OK)
  {
    // Save state for segmented transfer
    mSDO_TransStatus.seg_index    = index;
    mSDO_TransStatus.seg_subindex = subindex;
    mSDO_TransStatus.seg_toggle   = 0; // Initialize toggle bit
  }

  return (return_val);
}




/**************************************************************************
 PUBLIC FUNCTIONS
***************************************************************************/

/**************************************************************************
DOES:    Handle an incoimg SDO request.
RETURNS: SDO_ACCESS_OK: Access was made, SDO Response sent
         SDO_ACCESS_FAILED: Access denied, SDO Abort sent
         *ret_index and *ret_subindex, if known
GLOBALS: uses gRxCAN to retrieve SDO request
         uses gTxCAN to transmit SDO response
**************************************************************************/
SDO_ReturnType SDO_Handle_SDO_Request (
  UNSIGNED16  *ret_index,    // Index of the OD entry for request, if known
  UNSIGNED8   *ret_subindex  // Subndex of the OD entry for request, if known
  )
{
  UNSIGNED8       byte1      = 0x00U;           // First byte of SDO request
  UNSIGNED16      index      = 0x0000U;         // Index of SDO request
  UNSIGNED8       subindex   = 0x00U;           // Subindex of SDO request
  UNSIGNED8       length     = 0x00U;           // Length of a OD entry
  SDO_ODEntryType entry      = OD_NOENTRY;      // Type of the OD entry
  SDO_RequestType request    = SDO_UNKNOWN;     // Type of SDO Request
  SDO_ReturnType  return_val = SDO_OD_NOTFOUND; // Return value for this function

  // Default (unknown) index and subindex is all 0's
  *ret_index    = 0x0000U;
  *ret_subindex = 0x00U;

  // Init SDO Response/Abort message
  gTxCAN.ID     = 0x580U+gNodeStatus.node_id;
  gTxCAN.LEN    = 8;

  // Get first byte of SDO packet
  byte1         = gRxCAN.BUF[0];

  // Check for abort code
  if (byte1 == SDO_FBT_ABORT)
  { // Abort code received

    // Reset state machine for segmented transfers
    mSDO_TransStatus.trans_type = SEG_STAT_NONE;

    // Do nothing further and simply ignore the abort received
  }
  else
  {
    // Get type of SDO request
    switch (byte1 & SDO_FBT_CCS_MASK)
    {
      case SDO_FBT_SEG_READ_CMD:
        request = SDO_SEG_UP;
        break;
      case SDO_FBT_SEG_WRITE_CMD:
        request = SDO_SEG_DOWN;
        break;
      case SDO_FBT_SEG_NEW_READ_CMD:
        request = SDO_INIT_UP;
        break;
      case SDO_FBT_SEG_NEW_WRITE_CMD:
        request = SDO_INIT_DOWN;
        break;
      default:
        request = SDO_UNKNOWN;
        break;
    }

    // Check on command and length to see if it is valid
    if ( (request    == SDO_UNKNOWN) ||
         (gRxCAN.LEN != 8)           )
    { // Command not valid: Abort
      // Copy multiplexor into response message
      gTxCAN.BUF[1] = gRxCAN.BUF[1]; // index lo
      gTxCAN.BUF[2] = gRxCAN.BUF[2]; // index hi
      gTxCAN.BUF[3] = gRxCAN.BUF[3]; // subindex

      SDO_Send_SDO_Abort(SDO_ABORT_UNKNOWN_COMMAND);
      return_val = SDO_ACCESS_FAILED;
    }
    else if ( (request == SDO_INIT_DOWN) ||
              (request == SDO_INIT_UP)   )
    { // If this is an SDO init (expedited/segmented) command, process it
      // Get requested index and subindex (multiplexor)
      index         = ((UNSIGNED16)gRxCAN.BUF[2] << 8) | (UNSIGNED16)gRxCAN.BUF[1];
      subindex      = gRxCAN.BUF[3];

      // Copy multiplexor into response message
      gTxCAN.BUF[1] = gRxCAN.BUF[1]; // index lo
      gTxCAN.BUF[2] = gRxCAN.BUF[2]; // index hi
      gTxCAN.BUF[3] = gRxCAN.BUF[3]; // subindex

      // Index and subindex are known now
      *ret_index    = index;
      *ret_subindex = subindex;

      // Check if that entry exists, and what type it is
      entry = SDO_OD_Get_Entry(index,subindex,&length);

      switch (entry)
      {
        case OD_NOENTRY:
          if (subindex == 0)
          {
            SDO_Send_SDO_Abort(SDO_ABORT_NOT_EXISTS);
          }
          else
          {
            SDO_Send_SDO_Abort(SDO_ABORT_UNKNOWNSUB);
          }
          return_val = SDO_ACCESS_FAILED;
          break;

        case OD_EXP_RO:
        case OD_EXP_WO:
        case OD_EXP_RW:
          // Deal with access to expedited entries
          return_val = SDO_Handle_Exp_Request(index,subindex,length,entry);
          break;

        case OD_SEG_RO:
        case OD_SEG_WO:
        case OD_SEG_RW:
          // Deal with access to segmented entries
            return_val = SDO_Handle_Seg_Request(index,subindex,entry);
            break;

          default:
            break;
      }
    }
    else if ( (request == SDO_SEG_DOWN) ||
              (request == SDO_SEG_UP)   )
    { // All other SDO requests are assumed to be segmented packets
      // If this is an SDO segmented packet, process it

      unsigned char i;
      
      // Index and subindex are those of the ongoing transfer
      *ret_index    = mSDO_TransStatus.seg_index;
      *ret_subindex = mSDO_TransStatus.seg_subindex;

      // Clear reserved bytes in SDO response
      for (i=1; i<8; i++)
      {
        gTxCAN.BUF[i] = 0x00;
      }

      // Check if SDO Segmented Download (Write) Transfer in progress.
      // If so, process segment.
      if (mSDO_TransStatus.trans_type == SEG_STAT_WRITE)
      {
        return_val = SDO_Handle_Seg_Write();
      }
      else
      {
        // Check if SDO Segmented Upload (Read) Transfer in progress
        // If so, process segment.
        if (mSDO_TransStatus.trans_type == SEG_STAT_READ)
        {
          return_val = SDO_Handle_Seg_Read();
        }
        else
        {
          // Set multiplexor in response message
          gTxCAN.BUF[1] = (mSDO_TransStatus.seg_index & 0xFF); // index lo
          gTxCAN.BUF[2] = ((mSDO_TransStatus.seg_index >> 8) & 0xFF); // index hi
          gTxCAN.BUF[3] = (mSDO_TransStatus.seg_subindex & 0xFF); // subindex

          SDO_Send_SDO_Abort(SDO_ABORT_TRANSFER);
          return_val = SDO_ACCESS_FAILED;
        }
      }
    }
    else
    { // Never be here
      ;
    }

    // If request still not processed, generate appropriate abort code
    if (return_val == SDO_OD_NOTFOUND)
    {
      // If we reach here, the SDO command is unknown
      SDO_Send_SDO_Abort(SDO_ABORT_UNKNOWN_COMMAND);
      return_val = SDO_ACCESS_FAILED;
    }
    else
    { // Nothing to do here
      ;
    }
  }

  // All aborts also cancel any ongoing segmented transfer
  if (return_val == SDO_ACCESS_FAILED)
  {
    // Reset state machine for segmented transfers
    mSDO_TransStatus.trans_type = SEG_STAT_NONE;
  }
  else
  { // Nothing to do here
    ;
  }

  return (return_val);
}


/**************************************************************************
DOES:    Initializes SDO protocol and state machine
RETURNS: -
**************************************************************************/
void SDO_Init (
  void
  )
{
  mSDO_TransStatus.trans_type   = SEG_STAT_NONE;
  mSDO_TransStatus.seg_index    = 0;
  mSDO_TransStatus.seg_subindex = 0;
  mSDO_TransStatus.seg_toggle   = 0;

  return;
}



/***************************************************************************
END OF FILE
***************************************************************************/
