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

#ifdef __SIMULATION__
// header files to create dll
#include <windows.h>
#include "mcohwpcsim.h"
#endif

#include "mcop_xod_inc.h"

#if (NR_OF_SDO_CLIENTS > 0)
#include "mcop_mgr_inc.h"
#endif

#if USE_REMOTE_ACCESS

// the maximum size of an SDO transfer supported
// is the maximum serial packet size minus the longest header
#define MAX_SDO_TRANSFER_SIZE (MAX_PACKET_DATA - 7)

// macros to get and store multi-byte values in little-endian format
#define STORE_U16(value, loc) (loc)[0] = (value) & 0xFF; (loc)[1] = ((value) >> 8) & 0xFF;
#define STORE_U32(value, loc) (loc)[0] = (value) & 0xFF; (loc)[1] = ((value) >> 8) & 0xFF; (loc)[2] = ((value) >> 16) & 0xFF; (loc)[3] = ((value) >> 24) & 0xFF;
#define GET_U16(loc) ((UNSIGNED16)((loc)[1]) << 8) | ((loc)[0]);
#define GET_U32(loc) ((UNSIGNED32)((loc)[3]) << 24) | ((UNSIGNED32)((loc)[2]) << 16) | ((UNSIGNED32)((loc)[1]) << 8) | ((loc)[0]);

// error flags
#define MCORA_ERROR_NONE             0
#define MCORA_ERROR_NOTFOUND         (1 << 0)
#define MCORA_ERROR_INCORRECTLENGTH  (1 << 1)
#define MCORA_ERROR_INVALIDCOMMAND   (1 << 2)
#define MCORA_ERROR_BUSY             (1 << 3)
#define MCORA_ERROR_NORESOURCES      (1 << 4)
#define MCORA_ERROR_TXFULL           (1 << 5)
#define MCORA_ERROR_ABORTED          (1 << 6)
#define MCORA_ERROR_BUFFERSIZE       (1 << 7)
#define MCORA_ERROR_TOGGLE           (1 << 8)
#define MCORA_ERROR_TIMEOUT          (1 << 9)
#define MCORA_ERROR_UNKNOWN          (1 << 10)
#define MCORA_ERROR_NOTSUPPORTED     (1 << 11)
#define MCORA_ERROR_NODENOTAVAILABLE (1 << 12)

// SDO client transfer directions
#define MCORA_SDOUNKNOWN 0
#define MCORA_SDOREAD    (1 << 0)
#define MCORA_SDOWRITE   (1 << 1)

// initialization mode flags
#define MCORA_MODE_LSSSLAVE  (1 << 0)
#define MCORA_MODE_AUTOSTART (1 << 1)
#define MCORA_MODE_DS447     (1 << 2)


/**************************************************************************
MODULE DECLARATIONS
**************************************************************************/

// stack configuration parameters that can be set via the host interface
static UNSIGNED32 mSerialNumber;
static UNSIGNED8  mMode;
static UNSIGNED8  mNodeID;
static UNSIGNED16 mCANBPS;
static UNSIGNED32 mBufLen;
static UNSIGNED32 mBufDat;

// function pointer type to a command handler
typedef void (*COMMAND_HANDLER)(UNSIGNED8 length, UNSIGNED8 *pData);

// type that describes a command
typedef struct _Command
{
  UNSIGNED8 indicator;
  COMMAND_HANDLER handler;
} COMMAND;

// command handler prototypes
static void MCORA_Command_Initialize(UNSIGNED8 length, UNSIGNED8 *pData);
static void MCORA_Command_ReadLocal(UNSIGNED8 length, UNSIGNED8 *pData);
static void MCORA_Command_WriteLocal(UNSIGNED8 length, UNSIGNED8 *pData);
static void MCORA_Command_WriteRemote(UNSIGNED8 length, UNSIGNED8 *pData);
static void MCORA_Command_ReadRemote(UNSIGNED8 length, UNSIGNED8 *pData);
static void MCORA_Command_SetHeartbeatConsumer(UNSIGNED8 length, UNSIGNED8 *pData);

// list of commands supported
static COMMAND mCommands[] = {{'I', MCORA_Command_Initialize},
                              {'R', MCORA_Command_ReadLocal},
                              {'W', MCORA_Command_WriteLocal},
                              {'S', MCORA_Command_WriteRemote},
                              {'U', MCORA_Command_ReadRemote},
                              {'H', MCORA_Command_SetHeartbeatConsumer},
                             };

// macro to return the number of commands supported
#define NUM_COMMANDS() (sizeof(mCommands) / sizeof(COMMAND))

#if (NR_OF_SDO_CLIENTS > 0)
// a type to hold application-specific information on SDO client transfers
// we do this so we don't modify the CANopen stack
typedef struct _MCORASDOClient
{
  UNSIGNED8 busy;                           // TRUE if client is in use
  UNSIGNED16 index;                         // object dictionary index
  UNSIGNED8 subindex;                       // object dictionary subindex
  UNSIGNED8 direction;                      // transfer direction (MCORA_SDOREAD, MCORA_SDOWRITE)
  UNSIGNED8 buffer[MAX_SDO_TRANSFER_SIZE];  // SDO client buffer
  SDOCLIENT *pclient;                       // stack client description
} MCORASDOCLIENT;

// additional SDO client transfer information
static MCORASDOCLIENT mMCORASDOClientList[NR_OF_SDO_CLIENTS];
#endif


/**************************************************************************
DOES:    This function is called after data was received and stored
         (works for both SDO and PDO).
RETURNS: nothing
**************************************************************************/
void MCOUSER_ODData (
  UNSIGNED16 idx,
  UNSIGNED8 subidx,
  UNSIGNED8 *pDat,
  UNSIGNED8 len
  )
{
  // make sure we don't send more data to the host than allowed
  if (len > MAX_PACKET_DATA)
  {
    len = (UNSIGNED8)MAX_PACKET_DATA;
  }

  // send data to host
  MCORA_SendLocalProcessDataWrite(idx, subidx, len, pDat);
}


/**************************************************************************
DOES:    Checks if for a certain index and subindex a data entry exists
         in the Object Dictionary, does NOT check 1xxxh entries and
         constants
RETURNS: TRUE, if entry was found, then pDat contains pointer to data
         and pLen the length of the data
**************************************************************************/
UNSIGNED8 MCOP_FindODDataEntry (
  UNSIGNED16 idx,   // Index of Object Dictionary entry to find
  UNSIGNED8 sub,    // Subindex of Object Dictionary entry to find
  UNSIGNED32 *pLen, // When found, contains length of entry
  UNSIGNED8 **pDat  // When found, contains pointer to data of entry
  )
{
  UNSIGNED16 found;
  // pointer to an entry in gODProcTable
  OD_PROCESS_DATA_ENTRY MEM_CONST *pOD;
  // pointer to const OD entry
  UNSIGNED8 MEM_CONST *pDatConst;
  // return value required for search of generic table, here unused
  UNSIGNED8 acc;

  // Data available in process image?
  found = MCO_SearchODProcTable(idx,sub);
  // entry found?
  if (found != 0xFFFF)
  {
    // initialize pointer
    pOD = OD_ProcTablePtr(0);
    pOD += (UNSIGNED32)(found);
    *pDat = &(gProcImg[pOD->offset]);
    *pLen = (UNSIGNED32) (pOD->len & 0x0F);
    return TRUE;
  }

  // Data available in generic data table?
  found = XSDO_SearchODGenTable(idx,sub,&acc,pLen,pDat);
  // entry found?
  if (found != 0xFF)
  {
    return TRUE;
  }

  // Data available in SDO response table?
  found = MCO_SearchODProcTable(idx,sub);
  // entry found?
  if (found != 0xFFFF)
  {
    pDatConst = OD_SDOResponseTablePtr(0);
    pDatConst += (found * 8); // point to SDOREPLY entry
    // use buffer to return value
    mBufLen = 4 - ((*pDatConst >> 2) & 0x03);
    pDatConst += 4; // point to data
    mBufDat = *pDatConst;
    // return the buffers
    *pDat = (UNSIGNED8 *) &mBufDat;
    *pLen = mBufLen;
    return TRUE;
  }

  // not found
  return FALSE;
}


/**************************************************************************
DOES:    Initializes the serial interface to the host, 
         also flushes all buffers
RETURNS: nothing
**************************************************************************/
void MCORA_Init(
  void
  )
{
  UNSIGNED32 client;

  // initialize serial protocol module
  SerialProtocol_Init();

  // no SDO clients in use
  for (client = 0; client < NR_OF_SDO_CLIENTS; client++)
  {
    mMCORASDOClientList[client].busy = FALSE;
  }
}


/**************************************************************************
DOES:    Sends a init response to the host
RETURNS: TRUE for success, FALSE for error
**************************************************************************/
UNSIGNED8 MCORA_SendInitResponse(
  UNSIGNED16 err           // error code or zero for no error
  )
{
  int length;
  UNSIGNED8 data[3];

  data[0] = 'I';
  data[1] = err & 0xFF;
  data[2] = (err >> 8) & 0xFF;
  length = 3;

  return SerialProtocol_PushMessage(length, data, 0, 0);
}


/**************************************************************************
DOES:    Sends a write local response to the host
RETURNS: TRUE for success, FALSE for error
**************************************************************************/
UNSIGNED8 MCORA_SendWriteLocalResponse(
  UNSIGNED16 index,    // od index
  UNSIGNED8 subindex,  // od subindex
  UNSIGNED16 err       // error code or zero for no error
  )
{
  int length;
  UNSIGNED8 data[6];

  data[0] = 'W';
  STORE_U16(index, data + 1);
  data[3] = subindex;
  data[4] = err & 0xFF;
  data[5] = (err >> 8) & 0xFF;
  length = 6;

  return SerialProtocol_PushMessage(length, data, 0, 0);
}

/**************************************************************************
DOES:    Sends a write remote response to the host
RETURNS: TRUE for success, FALSE for error
**************************************************************************/
UNSIGNED8 MCORA_SendWriteRemoteResponse(
  UNSIGNED8 sdo,       // sdo channel number
  UNSIGNED16 index,    // od index
  UNSIGNED8 subindex,  // od subindex
  UNSIGNED16 err       // error code or zero for no error
  )
{
  int length;
  UNSIGNED8 data[7];

  data[0] = 'S';
  data[1] = sdo;
  STORE_U16(index, data + 2);
  data[4] = subindex;
  data[5] = err & 0xFF;
  data[6] = (err >> 8) & 0xFF;
  length = 7;

  return SerialProtocol_PushMessage(length, data, 0, 0);
}


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
  )
{
  int headerlength;
  UNSIGNED8 headerdata[6];

  headerdata[0] = 'R';
  STORE_U16(index, headerdata + 1);
  headerdata[3] = subindex;
  headerdata[4] = err & 0xFF;
  headerdata[5] = (err >> 8) & 0xFF;
  headerlength = 6;

  // limit to size of packet
  if (length > (MAX_PACKET_DATA - headerlength)) length = MAX_PACKET_DATA - headerlength;

  return SerialProtocol_PushMessage(headerlength, headerdata, length, pdata);
}


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
  )
{
  int headerlength;
  UNSIGNED8 headerdata[7];

  headerdata[0] = 'U';
  headerdata[1] = sdo;
  STORE_U16(index, headerdata + 2);
  headerdata[4] = subindex;
  headerdata[5] = err & 0xFF;
  headerdata[6] = (err >> 8) & 0xFF;
  headerlength = 7;

  // limit to size of packet
  if (length > (MAX_PACKET_DATA - headerlength)) length = MAX_PACKET_DATA - headerlength;

  return SerialProtocol_PushMessage(headerlength, headerdata, length, pdata);
}


/**************************************************************************
DOES:    Sends a heartbeat conumber init response to the host
RETURNS: TRUE for success, FALSE for error
**************************************************************************/
UNSIGNED8 MCORA_SendHBConsumerInitResponse(
  UNSIGNED8 nodeid,      // id of node to monitor
  UNSIGNED16 timeout,    // timeout in milliseconds
  UNSIGNED16 err         // error code or zero for no error
  )
{
  int length;
  UNSIGNED8 data[6];

  data[0] = 'H';
  data[1] = nodeid;
  data[2] = timeout & 0xFF;
  data[3] = (timeout >> 8) & 0xFF;
  data[4] = err & 0xFF;
  data[5] = (err >> 8) & 0xFF;
  length = 6;

  return SerialProtocol_PushMessage(length, data, 0, 0);
}


/**************************************************************************
DOES:    Sends an initialization complete message to the host
RETURNS: TRUE for success, FALSE for error
**************************************************************************/
UNSIGNED8 MCORA_SendInitComplete(
  UNSIGNED8 nodeid,          // node id being used
  UNSIGNED16 bps,            // can baud rate being used in kbps
  UNSIGNED32 serialnumber    // serial number being used
  )
{
  int length;
  UNSIGNED8 data[9];

  data[0] = 'C';
  data[1] = mMode;
  data[2] = nodeid;
  STORE_U16(bps, data + 3);  
  STORE_U32(serialnumber, data + 5);
  length = 9;

  return SerialProtocol_PushMessage(length, data, 0, 0);
}


/**************************************************************************
DOES:    Sends a new process data local write notification to the host
RETURNS: TRUE for success, FALSE for error
**************************************************************************/
UNSIGNED8 MCORA_SendLocalProcessDataWrite(
  UNSIGNED16 index,         // od index
  UNSIGNED8 subindex,       // od subindex
  UNSIGNED16 length,        // length of data written to od entry
  UNSIGNED8 *pdata          // data written to od entry
  )
{
  int headerlength;
  UNSIGNED8 headerdata[4];

  headerdata[0] = 'D';
  STORE_U16(index, headerdata + 1);
  headerdata[3] = subindex;
  headerlength = 4;

  // limit to size of packet
  if (length > (MAX_PACKET_DATA - headerlength)) length = MAX_PACKET_DATA - headerlength;

  return SerialProtocol_PushMessage(headerlength, headerdata, length, pdata);
}



/**************************************************************************
DOES:    Sends a node status change notification to the host
RETURNS: TRUE for success, FALSE for error
**************************************************************************/
UNSIGNED8 MCORA_SendNodeStatusChanged(
  UNSIGNED8 nodeid,
  UNSIGNED8 state,
  UNSIGNED8 wait
  )
{
  int length;
  UNSIGNED8 data[3];
  UNSIGNED8 result;

  // ignore if node id is invalid
  if ((nodeid < 0x01) || (nodeid > 0x7F)) return TRUE;
  
  if (nodeid == MY_NODE_ID)
  { // add extra info if this is own node ID
    nodeid |= 0x80;
  }
  
  data[0] = 'N';
  data[1] = nodeid;
  data[2] = state;
  length = 3;

  // send message
  result = SerialProtocol_PushMessage(length, data, 0, 0);

  // wait for it to go out
  if (wait)
  {
    SerialProtocol_CompleteTransmits();
  }

  return result;
}


/**************************************************************************
DOES:    Main Processing for Host Interface interaction
RETURNS: nothing
**************************************************************************/
void MCORA_ProcessHost (
  void
  )
{
  UNSIGNED8 length;
  UNSIGNED8 data[MAX_PACKET_DATA];
  int cmd;

  // execute serial protocol state machines
  SerialProtocol_Process();
  // check if any errors occurred
  if (SerialProtocol_CheckError())
  {
    // handle error if needed
    return;
  }

  // handle received commands
  if ((length = SerialProtocol_PullMessage(data)) != 0)
  { // search for command in list of supported commands
    for (cmd = 0; cmd < NUM_COMMANDS(); cmd++)
    { // found so execute handler
      if (data[0] == mCommands[cmd].indicator)
      {
        mCommands[cmd].handler(length - 1, data + 1);
        break;
      }
    }
  }
}


/**************************************************************************
DOES:    Command handler for 'I' initialize command. Executes command
RETURNS: nothing
**************************************************************************/
static void MCORA_Command_Initialize(
  UNSIGNED8 length,      // length of command data
  UNSIGNED8 *pData       // command data, excluding command indicator
  )
{
  UNSIGNED16 errorcode = 0;
  UNSIGNED16 delay;
  UNSIGNED8 mode;
  UNSIGNED8 nodeid;
  UNSIGNED16 canbps;
  UNSIGNED32 serialnumber;

  // get stack configuration
  mode         = pData[0];
  nodeid       = pData[1];
  canbps       = GET_U16(pData + 2);
  serialnumber = GET_U32(pData + 4);

  // ignore if the length is invalid
  if (length != 8)
  {
    errorcode |= MCORA_ERROR_INVALIDCOMMAND;
    MCORA_SendInitResponse(errorcode);
    return;
  }

  // check parameters
  if ((nodeid < 0x01) ||
      (nodeid > 0x7F) ||
      ((canbps != 0) && (canbps != 10) && (canbps != 20) && (canbps != 50) &&
      (canbps != 125) && (canbps != 250) && (canbps != 500) && (canbps != 800) && (canbps != 1000))
     )
  {
    errorcode |= MCORA_ERROR_INVALIDCOMMAND;
    MCORA_SendInitResponse(errorcode);
    return;
  }

#if AUTOSTART
  // if autostart not enabled then generate not supported error
  if ((mode & MCORA_MODE_AUTOSTART) != MCORA_MODE_AUTOSTART)
  {
    errorcode |= MCORA_ERROR_NOTSUPPORTED;
    MCORA_SendInitResponse(errorcode);
    return;
  }
#else
  // if autostart not disabled then generate not supported error
  if ((mode & MCORA_MODE_AUTOSTART) == MCORA_MODE_AUTOSTART)
  {
    errorcode |= MCORA_ERROR_NOTSUPPORTED;
    MCORA_SendInitResponse(errorcode);
    return;
  }
#endif

#if USE_LSS_SLAVE
  // if lss slave not enabled then generate not supported error
  if ((mode & MCORA_MODE_LSSSLAVE) != MCORA_MODE_LSSSLAVE)
  { // Warning, application says disable, but we are enabled
    mode |= MCORA_MODE_LSSSLAVE;
  }
#else
  // if lss slave not disabled then generate not supported error
  if ((mode & MCORA_MODE_LSSSLAVE) == MCORA_MODE_LSSSLAVE)
  {
    errorcode |= MCORA_ERROR_NOTSUPPORTED;
    MCORA_SendInitResponse(errorcode);
    return;
  }
#endif

#if USE_CiA447
  // if CiA447 not enabled then generate not supported error
  if ((mode & MCORA_MODE_CiA447) != MCORA_MODE_CiA447)
  {
    errorcode |= MCORA_ERROR_NOTSUPPORTED;
    MCORA_SendInitResponse(errorcode);
    return;
  }
#else
  // if CiA447 not disabled then generate not supported error
  if ((mode & MCORA_MODE_CiA447) == MCORA_MODE_CiA447)
  {
    errorcode |= MCORA_ERROR_NOTSUPPORTED;
    MCORA_SendInitResponse(errorcode);
    return;
  }
#endif

  // acknowledge the init command
  MCORA_SendInitResponse(errorcode);

  // delay to make sure the acknowledge is transmitted
  delay = MCOHW_GetTime() + 50;
  while(!(MCOHW_IsTimeExpired(delay)))
  {
    SerialProtocol_Process();
  }

  if ((mode & MCORA_MODE_RESETAPP) == MCORA_MODE_RESETAPP)
  { // HW reset requested
    // execute HW Reset
    MCOUSER_ResetApplication();
  }

  // activate new settings
  mMode         = mode;
  mNodeID       = nodeid;
  mCANBPS       = canbps;
  mSerialNumber = serialnumber;

  if ((mode & MCORA_MODE_NORESET) != MCORA_MODE_NORESET)
  { // reset - this will cause a reinit followed by an init complete message being
    MCOUSER_ResetCommunication();
  }
}

/**************************************************************************
DOES:    Gets the current mode value that was passed from the host
         in the initialization command
RETURNS: mode value
**************************************************************************/
unsigned char MCORA_GetCurrentMode
  (
  void
  )
{
  return mMode;
}


/**************************************************************************
DOES:    Command handler for 'R' read local command. Executes command
RETURNS: nothing
**************************************************************************/
static void MCORA_Command_ReadLocal(
  UNSIGNED8 length,             // length of command data
  UNSIGNED8 *pData              // command data, excluding command indicator
  )
{
  UNSIGNED16 index;
  UNSIGNED8 subindex;
  UNSIGNED32 entrylen = 0;
  UNSIGNED8 *pentrydata;
  UNSIGNED16 errorcode = 0;

  index    = GET_U16(pData);
  subindex = pData[2];

  if (length != 3)
  {
    errorcode |= MCORA_ERROR_INVALIDCOMMAND;
    MCORA_SendReadLocalResponse(index, subindex, errorcode, 0, 0);
    return;
  }

  // Check if this is a system entry
  if ((index == 0x1017) && (subindex == 0))
  {
    entrylen = 2;
    pentrydata = pData; // overwrite command with response
    pentrydata[0] = (UNSIGNED8) gMCOConfig.heartbeat_time;
    pentrydata[1] = (UNSIGNED8) (gMCOConfig.heartbeat_time >> 8);
  }

  #if USE_SYNC
  // access to [1005,00] - SYNC COB-ID
  if ((index == 0x1005) && (subindex == 0x00))
  {
    entrylen = 4;
    pentrydata = pData; // overwrite command with response
    pentrydata[0] = (UNSIGNED8) gMCOConfig.SYNCid & 0x00FF;
    pentrydata[1] = (UNSIGNED8) (gMCOConfig.SYNCid>>8) & 0x00FF;
    pentrydata[2] = (UNSIGNED8) 0;
    pentrydata[3] = (UNSIGNED8) 0;
  }
  #endif

  #if ERROR_FIELD_SIZE > 0
  // access to [1003,xx] - Error Field (History)
  if (index == 0x1003)
  {
    mBufDat = MCOP_ErrField_Get(subindex);
    if (mBufDat != 0xFFFFFFFF)
    {
      if (subindex == 0)
      {
        entrylen = 1;
        pentrydata = pData; // overwrite command with response
        pentrydata[0] = (UNSIGNED8) mBufDat;
      }
      else
      {
        entrylen = 4;
        pentrydata = pData; // overwrite command with response
        pentrydata[0] = (UNSIGNED8) mBufDat;
        pentrydata[1] = (UNSIGNED8)(mBufDat >> 8);
        pentrydata[2] = (UNSIGNED8)(mBufDat >> 16);
        pentrydata[3] = (UNSIGNED8)(mBufDat >> 24);
      }
    }
  }
  #endif

#if ! MGR_MONITOR_ALL_NODES
#if (NR_OF_HB_CONSUMER > 0)
#if DYNAMIC_HB_CONSUMER
  // dynamic read/write accesses
  // access to [1016,xx] - heartbeat consumer time
  if ((index == 0x1016) && (subindex > 0) && (subindex <= NR_OF_HB_CONSUMER))
  {
    subindex--; // now can directly be used as array pointer
    entrylen = 4;
    pentrydata = pData; // overwrite command with response
    pentrydata[0] = (UNSIGNED8) gHBCons[subindex].time;
    pentrydata[1] = (UNSIGNED8) (gHBCons[subindex].time >> 8);
    pentrydata[2] = (UNSIGNED8) gHBCons[subindex].can_id;
    pentrydata[3] = (UNSIGNED8) 0;
  }
#endif
#endif
#endif

  if (entrylen == 0)
  { // not in list above, check the OD tables
    if (MCOP_FindODDataEntry(index, subindex, &entrylen, &pentrydata) == FALSE)
    {
      errorcode |= MCORA_ERROR_NOTFOUND;
      MCORA_SendReadLocalResponse(index, subindex, errorcode, 0, 0);
      return;
    }
  }
  // send response to host
  MCORA_SendReadLocalResponse(index, subindex, errorcode, entrylen, pentrydata);
}


/**************************************************************************
DOES:    Command handler for 'W' write local command. Executes command
RETURNS: nothing
**************************************************************************/
static void MCORA_Command_WriteLocal(
  UNSIGNED8 length,                // length of command data
  UNSIGNED8 *pData                 // command data, excluding command indicator
  )
{
  UNSIGNED16 index;
  UNSIGNED8 subindex;
  UNSIGNED32 newentrylen, entrylen;
  UNSIGNED32 b;
  UNSIGNED8 *pnewentrydata, *pentrydata;
  UNSIGNED16 errorcode = 0;

  index         = GET_U16(pData);
  subindex      = pData[2];
  pnewentrydata = pData + 3;
  newentrylen   = length - 3;

  // if no data to write
  if (length < 4)
  {
    errorcode |= MCORA_ERROR_INVALIDCOMMAND;
    MCORA_SendWriteLocalResponse(index, subindex, errorcode);
    return;
  }
  #if ERROR_FIELD_SIZE > 0
  // access to [1003,xx] - Error Field (History)
  else if ((index == 0x1003) && (subindex == 0) && (pData[3] == 0))
  { // erase error history
    MCOP_ErrField_Flush();
  }
  #endif
  // Check if this is a system entry
  else if ((index == 0x1017) && (subindex == 0))
  {
    gMCOConfig.heartbeat_time = pData[4];
    gMCOConfig.heartbeat_time = (gMCOConfig.heartbeat_time << 8) + pData[3];
  }
  else
  {
    // find entry in od
    if (MCOP_FindODDataEntry(index, subindex, &entrylen, &pentrydata) == FALSE)
    {
      errorcode |= MCORA_ERROR_NOTFOUND;
      MCORA_SendWriteLocalResponse(index, subindex, errorcode);
      return;
    }
    // if length mismatch
    if (newentrylen != entrylen)
    {
      errorcode |= MCORA_ERROR_INCORRECTLENGTH;
      MCORA_SendWriteLocalResponse(index, subindex, errorcode);
      return;
    }
    // copy data
    for (b = 0; b < entrylen; b++) pentrydata[b] = pnewentrydata[b];
  }

  // send response
  MCORA_SendWriteLocalResponse(index, subindex, errorcode);
}


/**************************************************************************
DOES:    Command handler for 'S' write remote command. Executes command
RETURNS: nothing
**************************************************************************/
static void MCORA_Command_WriteRemote(
  UNSIGNED8 length,                    // length of command data
  UNSIGNED8 *pData                     // command data, excluding command indicator
  )
{
  UNSIGNED8 nodeid;
  UNSIGNED16 index;
  UNSIGNED8 subindex;
  UNSIGNED32 entrylen;
  UNSIGNED8 *pentrydata;
  UNSIGNED16 errorcode = 0;
  UNSIGNED32 b;
  SDOCLIENT *pSCL_ID;
  NODELIST *nodeinfo;
  UNSIGNED8 channel;

  nodeid     = pData[0];
  index      = GET_U16(pData + 1);
  subindex   = pData[3];
  pentrydata = pData + 4;
  entrylen   = length - 4;

  // no data to write
  if (length < 5)
  {
    errorcode |= MCORA_ERROR_INVALIDCOMMAND;
    MCORA_SendWriteRemoteResponse(nodeid, index, subindex, errorcode);
    return;
  }

  // check parameters
  if ((nodeid < 0x01) || (nodeid > 0x7F))
  {
    errorcode |= MCORA_ERROR_INVALIDCOMMAND;
    MCORA_SendWriteRemoteResponse(nodeid, index, subindex, errorcode);
    return;
  }

  // get the channel to use
#if USE_CiA447
  // for DS447 the channel is the same as the nodeid
  channel = nodeid;
#else
  // take first channel
  channel = 1;
#endif

  // if the channel is already in use then don't use it
  if (mMCORASDOClientList[channel - 1].busy)
  {
    errorcode |= MCORA_ERROR_BUSY;
    MCORA_SendWriteRemoteResponse(nodeid, index, subindex, errorcode);
    return;
  }

  nodeinfo = MGR_GetNodeInfoPtr(nodeid);
  if (!nodeinfo)
  {
    errorcode |= MCORA_ERROR_INVALIDCOMMAND;
    MCORA_SendWriteRemoteResponse(nodeid, index, subindex, errorcode);
    return;
  }

  // check if node is accessible, if not return error
  if (nodeinfo->id_scanstat < SCAN_DONE)
  { // currently the scanning process for this node is active
    errorcode |= MCORA_ERROR_NODENOTAVAILABLE;
    MCORA_SendWriteRemoteResponse(nodeid, index, subindex, errorcode);
    return;
  }

  // limit length of data
  if (entrylen > MAX_SDO_TRANSFER_SIZE) entrylen = MAX_SDO_TRANSFER_SIZE; 

  // copy data into SDO client buffer
  for (b = 0; b < entrylen; b++) mMCORASDOClientList[channel - 1].buffer[b] = pentrydata[b];

  // create an SDO client for the operation
  pSCL_ID = SDOCLNT_Init(channel,
            CAN_ID_SDOREQUEST(MY_NODE_ID, nodeid),
            CAN_ID_SDORESPONSE(MY_NODE_ID, nodeid),
            mMCORASDOClientList[channel - 1].buffer,
            entrylen);

  // if client not available
  if (!pSCL_ID)
  {
    errorcode |= MCORA_ERROR_NORESOURCES;
    MCORA_SendWriteRemoteResponse(nodeid, index, subindex, errorcode);
    return;    
  }

  // store additional transfer information
  mMCORASDOClientList[channel - 1].direction = MCORA_SDOWRITE;
  mMCORASDOClientList[channel - 1].index     = index;
  mMCORASDOClientList[channel - 1].subindex  = subindex;
  mMCORASDOClientList[channel - 1].pclient   = pSCL_ID;

  // start operation
  if (!SDOCLNT_Write(pSCL_ID, index, subindex))
  {
    // transmit queue full
    errorcode |= MCORA_ERROR_TXFULL;
    MCORA_SendWriteRemoteResponse(nodeid, index, subindex, errorcode);
    return;    
  }

  // channel is in use
  mMCORASDOClientList[channel - 1].busy = TRUE;
}


/**************************************************************************
DOES:    Command handler for 'U' read remote command. Executes command
RETURNS: nothing
**************************************************************************/
static void MCORA_Command_ReadRemote(
  UNSIGNED8 length,            // length of command data
  UNSIGNED8 *pData             // command data, excluding command indicator
  )
{
  UNSIGNED8 nodeid;
  UNSIGNED16 index;
  UNSIGNED8 subindex;
  UNSIGNED16 errorcode = 0;
  SDOCLIENT *pSCL_ID;
  NODELIST *nodeinfo;
  UNSIGNED8 channel;

  nodeid   = pData[0];
  index    = GET_U16(pData + 1);
  subindex = pData[3];

  // command too short
  if (length < 4)
  {
    errorcode |= MCORA_ERROR_INVALIDCOMMAND;
    MCORA_SendReadRemoteResponse(nodeid, index, subindex, errorcode, 0, 0);
    return;
  }

  // check parameters
  if ((nodeid < 0x01) || (nodeid > 0x7F))
  {
    errorcode |= MCORA_ERROR_INVALIDCOMMAND;
    MCORA_SendReadRemoteResponse(nodeid, index, subindex, errorcode, 0, 0);
    return;
  }

  // get channel to use
#if USE_CiA447
  // for DS447 the channel is the same as the node id
  channel = nodeid;
#else
  // take first available channel
  channel = 1;
#endif

  // if the channel is already in use then don't use it
  if (mMCORASDOClientList[channel - 1].busy)
  {
    errorcode |= MCORA_ERROR_BUSY;
    MCORA_SendReadRemoteResponse(nodeid, index, subindex, errorcode, 0, 0);
    return;
  }

  nodeinfo = MGR_GetNodeInfoPtr(nodeid);
  if (!nodeinfo)
  {
    errorcode |= MCORA_ERROR_INVALIDCOMMAND;
    MCORA_SendReadRemoteResponse(nodeid, index, subindex, errorcode, 0, 0);
    return;
  }

  // check if node is accessible, if not return error
  if (nodeinfo->id_scanstat < SCAN_DONE)
  { // currently the scanning process for this node is active
    errorcode |= MCORA_ERROR_NODENOTAVAILABLE;
    MCORA_SendReadRemoteResponse(nodeid, index, subindex, errorcode, 0, 0);
    return;
  }

  // create an SDO client for the operation
  pSCL_ID = SDOCLNT_Init(channel,
            CAN_ID_SDOREQUEST(MY_NODE_ID, nodeid),
            CAN_ID_SDORESPONSE(MY_NODE_ID, nodeid),
            mMCORASDOClientList[channel - 1].buffer,
            MAX_SDO_TRANSFER_SIZE);
  
  // if client not available
  if (!pSCL_ID)
  {
    errorcode |= MCORA_ERROR_NORESOURCES;
    MCORA_SendReadRemoteResponse(nodeid, index, subindex, errorcode, 0, 0);
    return;    
  }

  // store additional transfer information
  mMCORASDOClientList[channel - 1].direction = MCORA_SDOREAD;
  mMCORASDOClientList[channel - 1].index     = index;
  mMCORASDOClientList[channel - 1].subindex  = subindex;
  mMCORASDOClientList[channel - 1].pclient   = pSCL_ID;

  // start operation
  if (!SDOCLNT_Read(pSCL_ID, index, subindex))
  {
    // transmit queue full
    errorcode |= MCORA_ERROR_TXFULL;
    MCORA_SendReadRemoteResponse(nodeid, index, subindex, errorcode, 0, 0);
    return;    
  }

  // channel is in use
  mMCORASDOClientList[channel - 1].busy = TRUE;
}


/**************************************************************************
DOES:    Command handler for 'H' set heartbeat consumer command.
         Executes command
RETURNS: nothing
**************************************************************************/
static void MCORA_Command_SetHeartbeatConsumer(
  UNSIGNED8 length,            // length of command data
  UNSIGNED8 *pData             // command data, excluding command indicator
  )
{
  UNSIGNED8 nodeid;
  UNSIGNED16 timeout;
  UNSIGNED16 errorcode = 0;

  nodeid    = pData[0];
  timeout   = GET_U16(pData + 1);

  // command too short
  if (length < 2)
  {
    errorcode |= MCORA_ERROR_INVALIDCOMMAND;
    MCORA_SendHBConsumerInitResponse(nodeid, timeout, errorcode);
    return;
  }

  // check parameters
  if ((nodeid < 0x01) || (nodeid > 0x7F))
  {
    errorcode |= MCORA_ERROR_INVALIDCOMMAND;
    MCORA_SendHBConsumerInitResponse(nodeid, timeout, errorcode);
    return;
  }

  // configure heartbeat consumer
  MGR_InitHBConsumer(nodeid, timeout);

  // send response
  MCORA_SendHBConsumerInitResponse(nodeid, timeout, errorcode);
}


/**************************************************************************
DOES:    Gets the last stack configuration received via the host interface
RETURNS: nothing
**************************************************************************/
void MCORA_GetStackConfiguration(
  UNSIGNED8 *pmode,            // location to store mode
  UNSIGNED8 *pnodeid,          // location to store node id
  UNSIGNED16 *pcanbps,         // location to store can baudrate in kbps
  UNSIGNED32 *pserialnumber    // location to store serial number
  )
{
  *pmode = mMode;
  *pnodeid = mNodeID;
  *pcanbps = mCANBPS;
  *pserialnumber = mSerialNumber;
}


/**************************************************************************
DOES:    Called when an SDO client transfer is completed
RETURNS: nothing
**************************************************************************/
void MCORACB_SDOComplete (
  UNSIGNED8 channel,        // SDO channel number in range of 1 to NR_OF_SDO_CLIENTS
  UNSIGNED32 abort_code     // status, error, abort code
  )
{
  UNSIGNED16 error_code = 0;

  // if channel wasn't in use then transfer must have been
  // initiated by the stack, so ignore
  if (mMCORASDOClientList[channel - 1].busy != TRUE)
  {
    return;
  }

  // channel is no longer in use
  mMCORASDOClientList[channel - 1].busy = FALSE;

  switch (abort_code)
  {
    case SDOERR_OK:
      break;
    case SDOERR_ABORT:
      error_code |= MCORA_ERROR_ABORTED;
      // could read mMCORASDOClientList[channel - 1].pclient->last_abort here?
      break;
    case SDOERR_BUFSIZE:
      error_code |= MCORA_ERROR_BUFFERSIZE;
      break;
    case SDOERR_TOGGLE:
      error_code |= MCORA_ERROR_TOGGLE;
      break;
    case SDOERR_TIMEOUT:
      error_code |= MCORA_ERROR_TIMEOUT;
      break;
    default:
    case SDOERR_UNKNOWN:
      error_code |= MCORA_ERROR_UNKNOWN;
      break;
  }

  // if a read transfer has just completed
  if (mMCORASDOClientList[channel - 1].direction == MCORA_SDOREAD)
  {
    MCORA_SendReadRemoteResponse(channel,
                                 mMCORASDOClientList[channel - 1].index,
                                 mMCORASDOClientList[channel - 1].subindex,
                                 error_code,
                                 mMCORASDOClientList[channel - 1].pclient->curlen,
                                 mMCORASDOClientList[channel - 1].buffer);
  }
  // if a write transfer has just completed
  else
  {
    MCORA_SendWriteRemoteResponse(channel,
                                  mMCORASDOClientList[channel - 1].index,
                                  mMCORASDOClientList[channel - 1].subindex,
                                  error_code);
  }
}

#endif // USE_REMOTE_ACCESS

/*******************************************************************************
END OF FILE
*******************************************************************************/

