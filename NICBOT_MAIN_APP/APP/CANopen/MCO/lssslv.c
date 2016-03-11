/**************************************************************************
MODULE:    LSSSLV
CONTAINS:  MicroCANopen Plus - Support for Layer Setting Services
COPYRIGHT: Embedded Systems Academy, Inc. 2002-2015.
           All rights reserved. www.microcanopen.com
DISCLAIM:  Read and understand our disclaimer before using this code!
           www.esacademy.com/disclaim.htm
           This software was written in accordance to the guidelines at
           www.esacademy.com/software/softwarestyleguide.pdf
LICENSE:   THIS IS THE COMMERCIAL VERSION OF MICROCANOPEN
           ONLY USERS WHO PURCHASED A LICENSE MAY USE THIS SOFTWARE
VERSION:   6.20, ESA 11-MAY-15
           $LastChangedDate: 2015-05-09 19:41:45 -0300 (Sat, 09 May 2015) $
           $LastChangedRevision: 3390 $
***************************************************************************/

#include "mcop_inc.h"
#include <stackinit.h>
#include <string.h>

// External function for NVOL storage of LSS info
extern void MCOSP_GetNVOLUsage (UNSIGNED16 pLoc[5]);

#if ((USE_LSS_SLAVE == 1) && (USE_MICROLSS == 0))
// Only use this file if project is configured for LSS Slave


/**************************************************************************
EXTERNAL GLOBAL VARIABLES
**************************************************************************/

// This structure holds all node specific configuration
extern MCO_CONFIG MEM_FAR gMCOConfig;

// Process Image
extern UNSIGNED8 MEM_PROC gProcImg[];


/**************************************************************************
PRIVATE VARIABLES
**************************************************************************/

static struct {
  UNSIGNED32 myserial;         // Local serial number
  UNSIGNED8  active;           // TRUE if node is in LSS mode
  UNSIGNED8  operation_mode;   // Node is in operation mode. If not=>configuration
  UNSIGNED8  confbt_mode;      // Node is in "Configure bit timing" mode
  UNSIGNED8  new_node_id;      // New configured node ID
  UNSIGNED8  old_node_id;      // Original (pre-configured) node ID
  UNSIGNED8  new_node_bps;     // New configured baudrate
  UNSIGNED8  node_id_set;      // Flag to indicate if node ID is configured
  UNSIGNED8  match_vid;        // Match of Vendor ID from "Switch Mode Selective" command
  UNSIGNED8  match_pid;        // Match of Product Code from "Switch Mode Selective" command
  UNSIGNED8  match_rev;        // Match of Revision Number from "Switch Mode Selective" command
  UNSIGNED8  idr_match_vid;    // Match of Vendor ID from "Identify Remote Slave" command
  UNSIGNED8  idr_match_pid;    // Match of Product Code from "Identify Remote Slave" command
  UNSIGNED32 idr_rev_lo;       // Low boundary of revision from "Identify Remote Slave" command
  UNSIGNED8  idr_match_rev_lo; // Match of Revision Number low boundary from "Identify Remote Slave" command
  UNSIGNED32 idr_rev_hi;       // High boundary of revision from "Identify Remote Slave" command
  UNSIGNED8  idr_match_rev_hi; // Match of Revision Number high boundary from "Identify Remote Slave" command
  UNSIGNED32 idr_ser_lo;       // Low boundary of serial from "Identify Remote Slave" command
  UNSIGNED8  idr_match_ser_lo; // Match of Serial Number low boundary from "Identify Remote Slave" command
  UNSIGNED16 actbt_sw_delay;   // Time in ms after which LSS switches the baudrate, and after which it is ready again
  UNSIGNED16 actbt_delay;      // Timestamp to switch or be ready to receive/transmit again
  UNSIGNED8  actbt_waitswitch; // If true, we wait until we can switch the baudrate
  UNSIGNED8  actbt_waitready;  // If true, we have swiched the baudrate and wait until we can send/receive again
} MEM_FAR mLSS;


// This structure holds the current transmit message
static CAN_MSG MEM_BUF mTxCAN;


/*******************************************************************************
PRIVATE FUNCTIONS
*******************************************************************************/


/****************************************************************
DOES:    Extracts dword in CANopen byte order from memory location
RETURNS: UNSIGNED32
*****************************************************************/
static UNSIGNED32 LSS_GetDword (
  UNSIGNED8 *pDat
  )
{
  UNSIGNED32 lvalue;
  UNSIGNED8  i;

  lvalue  = 0x00000000L;
  pDat   += 3;   // Set pointer to MSB
  for (i=0; i<4; i++)
  {
    lvalue <<= 8;
    lvalue  |= *pDat--;
  }

  return lvalue;
}


/****************************************************************
DOES:    Inserts dword in CANopen byte order into memory location
RETURNS: UNSIGNED32
*****************************************************************/
static void LSS_PutDword (
  UNSIGNED32 lvalue,
  UNSIGNED8 *pDat
  )
{
  UNSIGNED8  i;

  for (i=0; i<4; i++)
  {
    *pDat++  = (UNSIGNED8)lvalue & 0xFF;
    lvalue >>= 8;
  }

  return;
}


/****************************************************************
DOES:    Initializes CAN buffer for LSS response, sets byte 0
RETURNS:
*****************************************************************/
static void LSS_InitResponse (
  UNSIGNED8 data0
  )
{
  UNSIGNED8 i;

  mTxCAN.ID      = LSS_SLAVE_ID;
  mTxCAN.LEN     = 8;
  mTxCAN.BUF[0]  = data0;
  for (i = 1; i < 8; i++)
  {
    mTxCAN.BUF[i] = 0;
  }

  return;
}


/****************************************************************
DOES:    Reset the control flags for the Switch Mode Selective commands
RETURNS:
*****************************************************************/
static void LSS_ResetSwitchMode(void)
{
  mLSS.match_vid      = FALSE;
  mLSS.match_pid      = FALSE;
  mLSS.match_rev      = FALSE;
}


/****************************************************************
DOES:    Reset the control flags for the Identify Remote Slave commands
RETURNS:
*****************************************************************/
static void LSS_ResetInquireRemoteSlave(void)
{
  mLSS.idr_match_vid     = FALSE;
  mLSS.idr_match_pid     = FALSE;
  mLSS.idr_rev_lo        = 0xFFFFFFFFUL;
  mLSS.idr_match_rev_lo  = FALSE;
  mLSS.idr_rev_hi        = 0xFFFFFFFFUL;
  mLSS.idr_match_rev_hi  = FALSE;
  mLSS.idr_ser_lo        = 0xFFFFFFFFUL;
  mLSS.idr_match_ser_lo  = FALSE;
}


/****************************************************************
DOES:    LSS Switch Mode Global Command
GLOBALS: Sets mLSS.active status flag to FALSE if end-of-LSS
RETURNS:
*****************************************************************/
static UNSIGNED8 LSS_SwitchModeGlobal (
  UNSIGNED8 *pDat
  )
{
  LSS_ResetInquireRemoteSlave();

  if (*(pDat+1) == 1)
  {
    mLSS.operation_mode = FALSE;

    if (MY_NMT_STATE != NMTSTATE_LSS)
    {
      MY_NMT_STATE = NMTSTATE_LSS;
      mLSS.active = TRUE;
    }
  }
  else
  {
    LSS_ResetSwitchMode();

    mLSS.operation_mode = TRUE;

    // If a node is configured, a switch back into operation mode
    // means the node leaves LSS and initializes into CANopen NMT
    if (mLSS.node_id_set)
    {
      // Set module-internal LSS status to leave mLSS. The next call
      // of LSS_Do_LSS() will then re-initialize the node with LSS
      // parameters.
      mLSS.active = FALSE;

      return TRUE;
    }
  }

  return FALSE;
}


/****************************************************************
DOES:    LSS Switch Mode Selective Commands
RETURNS: -
*****************************************************************/
static void LSS_SwitchModeSelective (
  UNSIGNED8 *pDat
  )
{
  UNSIGNED32 lvalue;  // dword to compare
  UNSIGNED8  command; // Command Specifier Byte

  // These commands are only accepted in operation mode but
  // outside of "configure bit timing"!
  if ((!mLSS.operation_mode) || (mLSS.confbt_mode))
  {
    LSS_ResetSwitchMode();
    return;
  }

  command = *pDat++;

  // extract 32-bit value from CAN message bytes 1-4
  lvalue = LSS_GetDword(pDat);

  switch (command)
  {
    case LSS_SWMOD_VID:
      if (lvalue == OD_VENDOR_ID)
      {
        mLSS.match_vid = TRUE;
      }
      else
      {
        LSS_ResetSwitchMode();
      }
      break;
    case LSS_SWMOD_PID:
      if ( mLSS.match_vid  &&
          (lvalue == OD_PRODUCT_CODE) )
      {
        mLSS.match_pid = TRUE;
      }
      else
      {
        LSS_ResetSwitchMode();
      }
      break;
    case LSS_SWMOD_REV:
      if ( mLSS.match_vid && mLSS.match_pid &&
          (lvalue == OD_REVISION) )
      {
        mLSS.match_rev = TRUE;
      }
      else
      {
        LSS_ResetSwitchMode();
      }
      break;
    case LSS_SWMOD_SER:
      if ( mLSS.match_vid && mLSS.match_pid && mLSS.match_rev &&
          (lvalue == mLSS.myserial) )
      {
        // Send confirmation
        LSS_InitResponse(LSS_SWMOD_RESP);
        // Sending message
        if (!MCOHW_PushMessage(&mTxCAN))
        {
          MCOUSER_FatalError(0x0602);
        }

        // This node is in configuration mode now!
        mLSS.operation_mode = FALSE;

        if (MY_NMT_STATE != NMTSTATE_LSS)
        {
          MY_NMT_STATE = NMTSTATE_LSS;
          mLSS.active = TRUE;
        }
      }
      else
      {
        LSS_ResetSwitchMode();
      }
  }
}


/****************************************************************
DOES:    LSS Configure Node ID Command
RETURNS: -
*****************************************************************/
static void LSS_ConfigureNodeID (
  UNSIGNED8 *pDat
  )
{
UNSIGNED8 node_id;
#if USE_STORE_PARAMETERS
UNSIGNED16 nvol_offsets[5];
#endif

  // This command is only accepted in configuration mode but
  // outside of "configure bit timing"!
  if ((mLSS.operation_mode) || (mLSS.confbt_mode))
  {
    return;
  }

  // Prepare answer
  LSS_InitResponse(LSS_CONF_NID);

  node_id = *(pDat+1);  // Byte 1 in message is node id

  if ((node_id < 1) || (node_id > 127))
  {
    mTxCAN.BUF[1] = 1;  // Node ID out of range

#if USE_STORE_PARAMETERS
    // Get offsets
    MCOSP_GetNVOLUsage(nvol_offsets);
    // Erase all current settings
    NVOL_WriteByte(nvol_offsets[0]+NVOL_LSSNID,0xFF);
    NVOL_WriteByte(nvol_offsets[0]+NVOL_LSSBPS,0xFF);
    NVOL_WriteByte(nvol_offsets[0]+NVOL_LSSENA,0xFF);
    NVOL_WriteByte(nvol_offsets[0]+NVOL_LSSCHK,0xFF);
    // No further consecutive writes after this
    NVOL_WriteComplete();
#endif // USE_STORE_PARAMETERS

  }
  else
  {
    mTxCAN.BUF[1] = 0;  // Node ID accepted

    mLSS.new_node_id = node_id;
    mLSS.node_id_set = TRUE;

    MY_NODE_ID = node_id;

   // Memorize this as old node ID for inquiry command
   mLSS.old_node_id    = node_id;
  }

  // Sending message
  if (!MCOHW_PushMessage(&mTxCAN))
  {
    MCOUSER_FatalError(0x0602);
  }
}


/****************************************************************
DOES:    LSS Configure Bit Timing Command
RETURNS: -
*****************************************************************/
static void LSS_ConfigureBitTiming (
  UNSIGNED8 *pDat
  )
{
  // This command is only accepted in configuration mode
  if (mLSS.operation_mode)
  {
    return;
  }

  LSS_InitResponse(LSS_CONF_BIT);

  if (CAN_BITRATE_SUPPORTED & (1U << pDat[2]))
  { // => bit timing supported
    mLSS.new_node_bps = pDat[2];  // Set new baudrate
    mLSS.confbt_mode  = TRUE;     // Configure bit timing mode is active
  }
  else
  { // => bit timing not supported
    mTxCAN.BUF[1] = 1;
    mLSS.confbt_mode  = FALSE;    // Configure bit timing mode is not active
  }

  mLSS.actbt_waitswitch = FALSE;
  mLSS.actbt_waitready  = FALSE;

  // Sending message
  if (!MCOHW_PushMessage(&mTxCAN))
  {
    MCOUSER_FatalError(0x0602);
  }

  return;
}


/****************************************************************
DOES:    LSS Activate Bit Timing Command
RETURNS: -
*****************************************************************/
static void LSS_ActivateBitTiming (
  UNSIGNED8 *pDat
  )
{
  // This command is only accepted in configure bit timing mode
  if (!mLSS.confbt_mode)
  {
    return;
  }

  pDat++;   // Point to switch delay LSB
  mLSS.actbt_sw_delay  = *pDat++;
  mLSS.actbt_sw_delay |= (*pDat << 8);

  mLSS.actbt_waitswitch = TRUE;
  mLSS.actbt_waitready  = FALSE;

// Calculate the timestamp to switch
  mLSS.actbt_delay = MCOHW_GetTime() + mLSS.actbt_sw_delay;

  return;
}


/****************************************************************
DOES:    LSS Store Configuration Command
RETURNS: -
*****************************************************************/
static void LSS_StoreConfiguration(void)
{
UNSIGNED8 lss_chk;
UNSIGNED16 nvol_offsets[5];

  // This command is only accepted in configuration mode
  if (mLSS.operation_mode)
  {
    return;
  }

  LSS_InitResponse(LSS_STOR_CONF);

#if USE_STORE_PARAMETERS

  // Get offsets
  MCOSP_GetNVOLUsage(nvol_offsets);

  NVOL_WriteByte(nvol_offsets[0]+NVOL_LSSNID,mLSS.new_node_id);
  NVOL_WriteByte(nvol_offsets[0]+NVOL_LSSBPS,mLSS.new_node_bps);

  // After LSS configuration is saved, disable LSS on startup
  NVOL_WriteByte(nvol_offsets[0]+NVOL_LSSENA,0x00);

  // Save checksum
  lss_chk = NVOL_ReadByte(nvol_offsets[0]+NVOL_LSSNID);
  lss_chk += NVOL_ReadByte(nvol_offsets[0]+NVOL_LSSBPS);
  lss_chk += NVOL_ReadByte(nvol_offsets[0]+NVOL_LSSENA);
  NVOL_WriteByte(nvol_offsets[0]+NVOL_LSSCHK,lss_chk);
  // No further consecutive writes after this
  NVOL_WriteComplete();

  if ( (NVOL_ReadByte(nvol_offsets[0]+NVOL_LSSNID) != mLSS.new_node_id)  ||
       (NVOL_ReadByte(nvol_offsets[0]+NVOL_LSSBPS) != mLSS.new_node_bps) ||
       (NVOL_ReadByte(nvol_offsets[0]+NVOL_LSSCHK) != lss_chk)
     )
  {
    // Storage media access error
    mTxCAN.BUF[1] = 0x02;
  }

#else // USE_STORE_PARAMETERS
  // If NVOL configuration storage not supported, respond
  // with "not supported"
  mTxCAN.BUF[1] = 0x01;

#endif // USE_STORE_PARAMETERS

  // Sending response
  if (!MCOHW_PushMessage(&mTxCAN))
  {
    MCOUSER_FatalError(0x0602);
  }

  return;
}


/****************************************************************
DOES:    LSS Inquire Identity Commands
RETURNS: -
*****************************************************************/
static void LSS_InquireIdentity (
  UNSIGNED8 *pDat
  )
{
  UNSIGNED32 lvalue;  // dword for response

  // These commands are only accepted in configuration mode but
  // outside of "configure bit timing"!
  if ((mLSS.operation_mode) || (mLSS.confbt_mode))
  {
    return;
  }

  switch (*pDat)
  {
    case LSS_INQ_VID:
      lvalue = OD_VENDOR_ID;
      break;
    case LSS_INQ_PID:
      lvalue = OD_PRODUCT_CODE;
      break;
    case LSS_INQ_REV:
      lvalue = OD_REVISION;
      break;
    case LSS_INQ_SER:
      lvalue = mLSS.myserial;
      break;
    default:
      lvalue = 0L;
  }

  LSS_InitResponse(*pDat);

  // store 32-bit value in CAN message bytes 1-4
  LSS_PutDword(lvalue,&mTxCAN.BUF[1]);

  // Sending message
  if (!MCOHW_PushMessage(&mTxCAN))
  {
    MCOUSER_FatalError(0x0602);
  }
}


/****************************************************************
DOES:    LSS Inquire Node ID Command
RETURNS: -
*****************************************************************/
static void LSS_InquireNodeID (void)
{
  // This command is only accepted in configuration mode but
  // outside of "configure bit timing"!
  if ((mLSS.operation_mode) || (mLSS.confbt_mode))
  {
    return;
  }

  LSS_InitResponse(LSS_INQ_NID);

  // Respond with the original (not LSS-configured) Node ID
  mTxCAN.BUF[1] =   mLSS.old_node_id;

  // Sending message
  if (!MCOHW_PushMessage(&mTxCAN))
  {
    MCOUSER_FatalError(0x0602);
  }

  return;
}


/****************************************************************
DOES:    LSS Identify Remote Slaves Commands
RETURNS: -
*****************************************************************/
static void LSS_IdentifyRemoteSlaves (
  UNSIGNED8 *pDat
  )
{
  UNSIGNED32 lvalue;  // dword to compare

  // These commands are accepted in either operation and configuration mode
  // but outside of "configure bit timing"!
  if (mLSS.confbt_mode)
  {
    return;
  }

  // extract 32-bit value from CAN message bytes 1-4
  lvalue = LSS_GetDword(pDat+1);

  switch (*pDat)
  {
    case LSS_REQID_VID:
      if (lvalue == OD_VENDOR_ID)
      {
        mLSS.idr_match_vid = TRUE;
      }
      else
      {
        LSS_ResetInquireRemoteSlave();
      }
      break;
    case LSS_REQID_PID:
      if ( mLSS.idr_match_vid  &&
          (lvalue == OD_PRODUCT_CODE) )
      {
        mLSS.idr_match_pid = TRUE;
      }
      else
      {
        LSS_ResetInquireRemoteSlave();
      }
      break;
    case LSS_REQID_REV_LO:
      if ( mLSS.idr_match_vid && mLSS.idr_match_pid &&
          (lvalue <= OD_REVISION) )
      {
        mLSS.idr_match_rev_lo = TRUE;
      }
      else
      {
        LSS_ResetInquireRemoteSlave();
      }
      break;
    case LSS_REQID_REV_HI:
      if ( mLSS.idr_match_vid     && mLSS.idr_match_pid &&
           mLSS.idr_match_rev_lo  &&
          ((lvalue > OD_REVISION) || (lvalue == OD_REVISION)) )
      {
        mLSS.idr_match_rev_hi = TRUE;
      }
      else
      {
        LSS_ResetInquireRemoteSlave();
      }
      break;
    case LSS_REQID_SER_LO:
      if ( mLSS.idr_match_vid     && mLSS.idr_match_pid    &&
           mLSS.idr_match_rev_lo  && mLSS.idr_match_rev_hi &&
          (lvalue <= mLSS.myserial) )
      {
        mLSS.idr_match_ser_lo = TRUE;
      }
      else
      {
        LSS_ResetInquireRemoteSlave();
      }
      break;
    case LSS_REQID_SER_HI:
      if ( mLSS.idr_match_vid     && mLSS.idr_match_pid    &&
           mLSS.idr_match_rev_lo  && mLSS.idr_match_rev_hi &&
           mLSS.idr_match_ser_lo  &&
          (lvalue >= mLSS.myserial) )
      {
        // Send confirmation
        LSS_InitResponse(LSS_ID_SLAVE);
        // Sending message
        if (!MCOHW_PushMessage(&mTxCAN))
        {
          MCOUSER_FatalError(0x0602);
        }
      }
      else
      {
        LSS_ResetInquireRemoteSlave();
      }
    default: ;
  }

  return;
}


/****************************************************************
DOES:    LSS Identify Non-configured Remote Slaves
RETURNS: -
*****************************************************************/
static void LSS_IdentifyNonconfigRemoteSlaves(void)
{
  // This command is accepted in either operation and configuration mode
  // but outside of "configure bit timing"!
  if (mLSS.confbt_mode)
  {
    return;
  }

  // Send a response if we are still unconfigured
  if (!mLSS.node_id_set)
  {
    // Send confirmation
    LSS_InitResponse(LSS_ID_NCONF_SLAVE);
    // Sending message
    if (!MCOHW_PushMessage(&mTxCAN))
    {
      MCOUSER_FatalError(0x0602);
    }
  }

  return;
}


/*******************************************************************************
GLOBAL FUNCTIONS
*******************************************************************************/

/****************************************************************
DOES:    LSS Store Configuration Command
RETURNS: -
*****************************************************************/
void LSS_LoadConfiguration (
  UNSIGNED16 *Baudrate,  // returns CAN baudrate in kbit
  UNSIGNED8 *Node_ID    // returns CANopen node ID (0-127)
  )
{
#if USE_STORE_PARAMETERS
UNSIGNED16 nvol_offsets[5];
UNSIGNED8 nodeid;
UNSIGNED8 nodebps;
UNSIGNED8 lssena;
#endif

  if (MY_NODE_ID != 0)
  { // only do this if node id is unknown
    *Baudrate = gMCOConfig.Baudrate;
    *Node_ID = MY_NODE_ID;
    return;
  }

#if USE_STORE_PARAMETERS
  // Get offsets
  MCOSP_GetNVOLUsage(nvol_offsets);

  // Get values
  nodeid = NVOL_ReadByte(nvol_offsets[0]+NVOL_LSSNID);
  nodebps = NVOL_ReadByte(nvol_offsets[0]+NVOL_LSSBPS);
  lssena = NVOL_ReadByte(nvol_offsets[0]+NVOL_LSSENA);

  if (NVOL_ReadByte(nvol_offsets[0]+NVOL_LSSCHK) == nodeid+nodebps+lssena)
  { // valid LSS configuration found
    switch(nodebps)
    {
    case LSS_BPS_1000:
      *Baudrate = 1000;
      break;
    case LSS_BPS_800:
      *Baudrate = 800;
      break;
    case LSS_BPS_500:
      *Baudrate = 500;
      break;
    case LSS_BPS_250:
      *Baudrate = 250;
      break;
    case LSS_BPS_125:
    default:
      *Baudrate = 125;
      break;
    case LSS_BPS_50:
      *Baudrate = 50;
      break;
    case LSS_BPS_20:
      *Baudrate = 20;
      break;
    case LSS_BPS_10:
      *Baudrate = 10;
      break;
    }
    *Node_ID = nodeid;
    return;
  }
  else
  {
    *Baudrate = 125;
    *Node_ID = 0;
  }
#else // USE_STORE_PARAMETERS
  *Baudrate = 0;
  *Node_ID = 0;
#endif // USE_STORE_PARAMETERS
}


/****************************************************************
DOES:    Process all LSS messages.
RETURNS:
*****************************************************************/
void LSS_HandleMsg (
  UNSIGNED8 Len,
  UNSIGNED8 *pDat
  )
{
  // After "Activate Bit Timing Parameter" command, don't execute
  // any commands for the time of 2*mLSS.actbt_sw_delay (ms) => LSS_Do_LSS()
  if ( (mLSS.actbt_waitswitch) ||
       (mLSS.actbt_waitready) )
  {
    return;
  }

  // Allow LSS commands only in pure-LSS and stopped mode
  if ( (MY_NMT_STATE != NMTSTATE_LSS) &&
       (MY_NMT_STATE != NMTSTATE_STOP) )
    return;

  if (Len == 8)   // must be 8 bytes long!
  {
    switch (*pDat)
    {
      case LSS_SWMOD_GLOB:
        LSS_SwitchModeGlobal(pDat);
        break;

      case LSS_SWMOD_VID:
      case LSS_SWMOD_PID:
      case LSS_SWMOD_REV:
      case LSS_SWMOD_SER:
        LSS_SwitchModeSelective(pDat);
        break;

      case LSS_CONF_NID:
        if (MY_NMT_STATE == NMTSTATE_LSS)
          LSS_ConfigureNodeID(pDat);
        break;

      case LSS_CONF_BIT:
        if (MY_NMT_STATE == NMTSTATE_LSS)
          LSS_ConfigureBitTiming(pDat);
        break;
      case LSS_ACT_BIT:
        if (MY_NMT_STATE == NMTSTATE_LSS)
          LSS_ActivateBitTiming(pDat);
        break;
      case LSS_STOR_CONF:
        if (MY_NMT_STATE == NMTSTATE_LSS)
          LSS_StoreConfiguration();
        break;

      case LSS_INQ_VID:
      case LSS_INQ_PID:
      case LSS_INQ_REV:
      case LSS_INQ_SER:
        if (MY_NMT_STATE == NMTSTATE_LSS)
          LSS_InquireIdentity(pDat);
        break;

      case LSS_INQ_NID:
        if (MY_NMT_STATE == NMTSTATE_LSS)
          LSS_InquireNodeID();
        break;

      case LSS_REQID_VID:
      case LSS_REQID_PID:
      case LSS_REQID_REV_LO:
      case LSS_REQID_REV_HI:
      case LSS_REQID_SER_LO:
      case LSS_REQID_SER_HI:
        if (MY_NMT_STATE == NMTSTATE_LSS)
          LSS_IdentifyRemoteSlaves(pDat);
        break;

      case LSS_REQID_NCONF:
        if (MY_NMT_STATE == NMTSTATE_LSS)
          LSS_IdentifyNonconfigRemoteSlaves();
        break;

      default:
        break;
    }
  }
}


/****************************************************************
DOES:    Check and update LSS status
RETURNS: FALSE: LSS is finished for this node
         TRUE:  Otherwise (LSS is still in process)
*****************************************************************/
UNSIGNED8 LSS_DoLSS(void)
{

#if USE_LEDS
  if (mLSS.active)
  {
    // If LEDs are used, toggle all 50ms
    if (MCOHW_IsTimeExpired(gMCOConfig.LED_timestamp))
    {
      if (gMCOConfig.LED_timestamp & 1)
	    {
        LED_RUN_ON;
        LED_ERR_OFF;
        gMCOConfig.LED_timestamp = (MCOHW_GetTime() + 50) & 0xFFFE;
	    }
	    else
	    {
        LED_RUN_OFF;
        LED_ERR_ON;
        gMCOConfig.LED_timestamp = (MCOHW_GetTime() + 50) | 0x0001;
	    }
    }
  }
  else
#else
  if (!mLSS.active)
#endif //USE_LEDS
  {

    // Has LSS just been finished?
    if (MY_NMT_STATE == NMTSTATE_LSS)
    {

#if USE_LEDS
      LED_RUN_OFF;
      LED_ERR_OFF;
#endif // USE_LEDS

      // Re-init
      MCOUSER_ResetCommunication();

    }

    return FALSE;
  }

  // After "Activate Bit Timing" command, wait until we can actually
  // switch the baudrate
  if (mLSS.actbt_waitswitch)
  {
    if (MCOHW_IsTimeExpired(mLSS.actbt_delay))
    {
      // Calculate the timestamp to be ready to receive/transmit again
      mLSS.actbt_delay = MCOHW_GetTime() + mLSS.actbt_sw_delay;

      // Set baudrate for CANopen stack
      switch(mLSS.new_node_bps)
      {
      case LSS_BPS_1000:
        gMCOConfig.Baudrate = 1000;
        break;
      case LSS_BPS_800:
        gMCOConfig.Baudrate = 800;
        break;
      case LSS_BPS_500:
        gMCOConfig.Baudrate = 500;
        break;
      case LSS_BPS_250:
        gMCOConfig.Baudrate = 250;
        break;
       case LSS_BPS_125:
       default:
        gMCOConfig.Baudrate = 125;
        break;
      case LSS_BPS_50:
        gMCOConfig.Baudrate = 50;
        break;
      case LSS_BPS_20:
        gMCOConfig.Baudrate = 20;
        break;
      case LSS_BPS_10:
        gMCOConfig.Baudrate = 10;
        break;
      }

      // Reset CAN controller, set new baudrate
      MCOHW_Init(gMCOConfig.Baudrate);

      // Set receive filter for LSS master message
      if (!MCOHW_SetCANFilter(LSS_MASTER_ID))
      {
        MCOUSER_FatalError(0x0601);
      }

       // We are no longer in "configure bit timing" mode
      mLSS.confbt_mode      = FALSE;

      mLSS.actbt_waitswitch = FALSE;
      mLSS.actbt_waitready  = TRUE;
    }
  }

  // After the baudrate has changed, wait until we are ready to receive/
  // transmit again
  if (mLSS.actbt_waitready)
  {
    if (MCOHW_IsTimeExpired(mLSS.actbt_delay))
    {
      mLSS.actbt_waitready = FALSE;
    }
  }

  return TRUE;
}


/****************************************************************
DOES:    Initialize LSS mechanism (variables etc.)
GLOBALS: Sets mLSS.active status flag to TRUE
         Sets gMCOConfig.nmt_state to NMTSTATE_LSS
RETURNS: -
*****************************************************************/
void LSS_Init(
  UNSIGNED8 node_id
  )
{

  memset(&mLSS, 0x00, sizeof(mLSS));

#if USECB_ODSERIAL
  mLSS.myserial = MCOUSER_GetSerial();
#else
  mLSS.myserial = OD_SERIAL;
#endif

  LSS_ResetSwitchMode();
  LSS_ResetInquireRemoteSlave();

  // The Node ID is not yet set
  mLSS.node_id_set    = FALSE;

  // After reset, the node is in operation mode
  mLSS.operation_mode = TRUE;

  // After reset, the node is not in "configure bit timing" mode
  mLSS.confbt_mode    = FALSE;

  // Pre-set LSS baudrate and node ID to default values
  mLSS.new_node_bps   = 4; // 125kbps
  mLSS.new_node_id    = node_id;

  // Memorize the old node ID for inquiry command
  mLSS.old_node_id    = node_id;

  if (node_id != 0)
  { // Stop here if node ID is known
    return;
  }

  // Set receive filter for LSS master message
  if (!MCOHW_SetCANFilter(LSS_MASTER_ID))
  {
    MCOUSER_FatalError(0x0601);
  }

  // We only get here if node must go in LSS state
  MY_NMT_STATE = NMTSTATE_LSS;
  mLSS.active = TRUE;

#if USE_LEDS
  LED_RUN_OFF;
  LED_ERR_OFF;
  gMCOConfig.LED_timestamp = MCOHW_GetTime() + 50;
#endif // USE_LEDS

}

#endif

/*******************************************************************************
END OF FILE
*******************************************************************************/

