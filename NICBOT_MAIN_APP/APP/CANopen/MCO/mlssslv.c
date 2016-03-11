/**************************************************************************
MODULE:    MLSS
CONTAINS:  MicroCANopen - Support for MicroLSS - Layer Setting Services
COPYRIGHT: Embedded Systems Academy, Inc. 2002-2015.
           All rights reserved. www.microcanopen.com
           This software was written in accordance to the guidelines at
           www.esacademy.com/software/softwarestyleguide.pdf
DISCLAIM:  Read and understand our disclaimer before using this code!
           www.esacademy.com/disclaim.htm
LICENSE:   THIS IS THE COMMERCIAL PLUS VERSION OF MICROCANOPEN
           ONLY USERS WHO PURCHASED A LICENSE MAY USE THIS SOFTWARE
           See file license_commercial_plus.txt
VERSION:   6.20, ESA 11-MAY-15
           $LastChangedDate: 2015-05-09 19:41:45 -0300 (Sat, 09 May 2015) $
           $LastChangedRevision: 3390 $
DEFINES:   USE_MICROLSS and USE_LSS_SLAVE must both be defined.
           If MLSS_ONLY	is defined, "pure" fastscan is used, else backward
           compatible LSS commands for switch mode selective and global are
           implemented, too (as needed by CiA447)
***************************************************************************/ 

#include "mcop_inc.h"


#if USE_MICROLSS
// Only use this file if project is configured for LSS Slave

#if ! USE_LSS_SLAVE
#error USE_LSS_SLAVE must be defined to use MicroLSS/LSS FastScan
#endif

#if USE_STORE_PARAMETERS
// usage areas of the nvol memory
static UNSIGNED16 nvol_offsets[5];

// External function for NVOL storage of LSS info
extern void MCOSP_GetNVOLUsage (UNSIGNED16 pLoc[5]);
#endif


/**************************************************************************
EXTERNAL GLOBAL VARIABLES
**************************************************************************/ 


/**************************************************************************
PRIVATE VARIABLES
**************************************************************************/ 

// The 128bit LSS_ID with vendor, product, rev and serial
static UNSIGNED32 MEM_FAR mLSS_ID[4]; // contains the 1018h Object, the LSSID

static struct {
  UNSIGNED8  active;           // TRUE if node is in LSS mode
  UNSIGNED8  operation_mode;   // Node is in operation mode. If not=>configuration
  UNSIGNED8  new_node_id;      // New configured node ID
  UNSIGNED8  old_node_id;      // Original (pre-configured) node ID
  UNSIGNED8  node_id_set;      // Flag to indicate if node ID is configured
  UNSIGNED8  new_node_bps;     // New configured baudrate
  UNSIGNED8  mLSS_state;       // In MicroLSS mode, shows which 32bit part we
                               // are currently working on
#if ! MLSS_ONLY
  UNSIGNED8  match_vid;        // Match of Vendor ID from "Switch Mode Selective" command
  UNSIGNED8  match_pid;        // Match of Product Code from "Switch Mode Selective" command
  UNSIGNED8  match_rev;        // Match of Revision Number from "Switch Mode Selective" command
#endif
} MEM_FAR mLSS;


// This structure holds the current transmit/response message
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
DOES:    Initializes CAN buffer for LSS response with feedback
RETURNS: 
*****************************************************************/
static void LSS_FeedBackResponse (
  UNSIGNED8 BitChecked, // bit currently checked
  UNSIGNED8 LSSNext // ID step checked
  )
{
UNSIGNED32 feedback = (LSS_SLAVE_ID << 18);

  if ((BitChecked == 0x80) || (BitChecked == 0))
  { // respond with hi word
    feedback += (((mLSS_ID[LSSNext]) & 0xFFFF0000UL) >> 16);
  }
  else
  { // respond with low word, set toggle
    feedback += ((mLSS_ID[LSSNext]) & 0x0000FFFFUL) + 0x00010000UL;
  }
#if USE_29BIT_LSSFEEDBACK == 1
#ifdef __SIMULATION__
  SimDriver_printf(" [LSSfeedback:%08xh]\n",feedback & 0x0000FFFFUL);
#endif 
  MCOHW_Push29Message(feedback);
#endif
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
DOES:    LSS Configure Node ID Command
RETURNS: -
*****************************************************************/
static void LSS_ConfigureNodeID (
  UNSIGNED8 *pDat
  )
{
UNSIGNED8 node_id;

  // This command is only accepted in configuration mode but
  // outside of "configure bit timing"!
  if (mLSS.operation_mode) 
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
    // Erase all current settings
    NVOL_WriteByte(NVOL_LSSNID,0xFF);
    NVOL_WriteByte(NVOL_LSSBPS,0xFF);
    NVOL_WriteByte(NVOL_LSSENA,0xFF);
    NVOL_WriteByte(NVOL_LSSCHK,0xFF);
    NVOL_WriteComplete();
#endif

  }
  else
  {

    mTxCAN.BUF[1] = 0;  // Node ID accepted
      
    mLSS.new_node_id = node_id;
    mLSS.node_id_set = TRUE;

    MY_NODE_ID = node_id;

    // Memorize this as old node ID for inquiry command
    mLSS.old_node_id = node_id;
  }

#ifdef __SIMULATION__
  SimDriver_printf("\nNode ID assigned by LSS: %d",MY_NODE_ID);
#endif 

  // Sending message
  if (!MCOHW_PushMessage(&mTxCAN))
  {
    MCOUSER_FatalError(0x0602);
  }
}
  

/****************************************************************
DOES:    LSS Store Configuration Command
RETURNS: -
*****************************************************************/
static void LSS_StoreConfiguration(void)
{
#if USE_STORE_PARAMETERS
UNSIGNED8 lss_chk;
#endif

  // This command is only accepted in configuration mode
  if (mLSS.operation_mode)
  {
    return;
  }

  LSS_InitResponse(LSS_STOR_CONF);

#if USE_STORE_PARAMETERS

  NVOL_WriteByte(NVOL_LSSNID,mLSS.new_node_id);
 
  // After LSS configuration is saved, disable LSS on startup
  NVOL_WriteByte(NVOL_LSSENA,NVOL_LSSENA_VAL);

  // Save checksum
  lss_chk = mLSS.new_node_id;
  lss_chk += NVOL_LSSENA_VAL;
  NVOL_WriteByte(NVOL_LSSCHK,lss_chk);

  NVOL_WriteComplete();

  if ( (NVOL_ReadByte(NVOL_LSSNID) != mLSS.new_node_id)  ||
       (NVOL_ReadByte(NVOL_LSSENA) != NVOL_LSSENA_VAL) ||
       (NVOL_ReadByte(NVOL_LSSCHK) != lss_chk)
     )
  {
    // Storage media access error
    mTxCAN.BUF[1] = 0x02;
  }

#endif // USE_STORE_PARAMETERS

  // Sending response
  if (!MCOHW_PushMessage(&mTxCAN))
  {
    MCOUSER_FatalError(0x0602);
  }

  return;
}
  

/****************************************************************
DOES:    LSS Load Configuration Command
RETURNS: -
*****************************************************************/
static void LSS_LoadConfiguration (
  UNSIGNED16 *Baudrate,  // returns CAN baudrate, here default
  UNSIGNED8 *Node_ID    // returns CANopen node ID (0-127)
  )
{
#if USE_STORE_PARAMETERS
UNSIGNED8 nodeid;
UNSIGNED8 lssena;
UNSIGNED8 lsschk;
#endif

  // This implementation is for Node ID only, not baudrate
  *Baudrate = gMCOConfig.Baudrate;

  if (MY_NODE_ID != 0)
  { // only do this if node id is unknown
    *Node_ID = MY_NODE_ID;
    return;
  }

#if USE_STORE_PARAMETERS
  // Initialize access to non-volatile memory
  NVOL_Init();

  // Get offsets
  MCOSP_GetNVOLUsage(nvol_offsets);

  // Get values
  nodeid = NVOL_ReadByte(nvol_offsets[0]+NVOL_LSSNID);
  lssena = NVOL_ReadByte(nvol_offsets[0]+NVOL_LSSENA);
  lsschk = NVOL_ReadByte(nvol_offsets[0]+NVOL_LSSCHK);

  if ((nodeid > 0) && (nodeid < 128) && ( lsschk == (nodeid+lssena)))
  { // valid LSS configuration found
    *Node_ID = nodeid;
  }
  else
  {
    *Node_ID = 0;
  }
#else
  *Node_ID = 0;
#endif
}


/****************************************************************
DOES:    LSS Identify Remote Slaves Commands
RETURNS: -
*****************************************************************/
static void LSS_IdentifyRemoteSlaves (
  UNSIGNED8 *pDat
  )
{
// Values received in MicroLSS Master Message
UNSIGNED32 IDNumber;    // current LSS_ID Subindex IDnumber (32bit)
UNSIGNED8  BitChecked;  // current bit requested 0..31 (0x80 for init/restart)
UNSIGNED8  LSSSub;      // current LSS_ID subindex 0..3 (vendor,product,rev,serial)
UNSIGNED8  LSSNext;     // next state for the MicroLSS states

UNSIGNED32 mask;        // compare mask
UNSIGNED8 found;        // return value

  // Initialize variables
  mask = 0;
  found = 0;
 
  // extract 32-bit value from CAN message bytes 1-4
  IDNumber = LSS_GetDword(pDat+1);
  BitChecked = pDat[5]; // 0..31
  LSSSub = pDat[6]; // 0..3 
  LSSNext = pDat[7];

  switch (*pDat)
  {
    case LSS_MICROLSS:
      found = 0;
      if (BitChecked & 0x80)
      { // MicroLSS initialization
        found = 1;
        mLSS.mLSS_state = 0;
        LSS_FeedBackResponse(BitChecked,LSSNext);
      }
      else if (LSSSub == mLSS.mLSS_state)
      { // we are still "on go" for the next 32bit value
        mask = 0xFFFFFFFF << BitChecked;
        if ((mLSS_ID[LSSSub] & mask) == (IDNumber & mask))
        { // match
          if ((BitChecked == 0x10) || (BitChecked == 0))
          {
            if (LSSNext < 4)
            { 
              LSS_FeedBackResponse(BitChecked,LSSNext);
            }
          }
          found = 1;  
          // Set new state as commanded by MicroLSS master
          mLSS.mLSS_state = LSSNext; 
          if (BitChecked == 0)
          { // 32bit scan completed with success
            if (LSSSub == 3)
            { // All done and matched, scan completed, NODE IDENTIFIED
              // Switch node to configuration mode now!
              mLSS.operation_mode = FALSE;
              mLSS.active = TRUE;
              MY_NMT_STATE = NMTSTATE_LSS;
            }
          }
        }
      }
      
      if (found == 1)
      { // Send a response as long as found
        // Send confirmation
        LSS_InitResponse(LSS_ID_SLAVE);
        // Sending message
        if (!MCOHW_PushMessage(&mTxCAN))
        {
          MCOUSER_FatalError(0x0602);
        }
      }
    default:
      break;
  }

  return;
}


#if ! MLSS_ONLY
// Functions for backward compatibility with older LSS version
/****************************************************************
DOES:    Reset the control flags for the Switch Mode Selective commands
RETURNS: 
*****************************************************************/
static void LSS_ResetSwitchMode(void)
{
  mLSS.match_vid      = FALSE;
  mLSS.match_pid      = FALSE;
  mLSS.match_rev      = FALSE;
  mLSS.operation_mode = TRUE; // go back to operation, even in case we were in config mode
}


/****************************************************************
DOES:    LSS Identify Non-configured Remote Slaves
RETURNS: -
*****************************************************************/
static void LSS_IdentifyNonconfigRemoteSlaves(void)
{
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
    // Force operation mode, to deal with faulty LSS master implementation or when
    // the switch global message is not received for whatever reason.
    // Not part of CiA 305.
    LSS_ResetSwitchMode();
  }

  return;
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

  // These commands are only accepted in operation mode
  if (!mLSS.operation_mode)
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
      if (lvalue == mLSS_ID[0])
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
          (lvalue == mLSS_ID[1]) )
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
          (lvalue == mLSS_ID[2]) )
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
          (lvalue == mLSS_ID[3]) )
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
#endif


/*******************************************************************************
GLOBAL FUNCTIONS
*******************************************************************************/

/****************************************************************
DOES:    Process all LSS messages. 
RETURNS: 
*****************************************************************/
void LSS_HandleMsg (
  UNSIGNED8 Len,
  UNSIGNED8 *pDat
  )
{
  // Allow LSS commands only in pure-LSS and stopped mode  
  if ( (MY_NMT_STATE != NMTSTATE_LSS) &&
       (MY_NMT_STATE != NMTSTATE_STOP) )
  {
    return;
  }

  if (Len == 8)   // must be 8 bytes long!
  {
    switch (*pDat)
    {
      case LSS_SWMOD_GLOB:
        LSS_SwitchModeGlobal(pDat);
        break;

      case LSS_CONF_NID:
        if (MY_NMT_STATE == NMTSTATE_LSS)
          LSS_ConfigureNodeID(pDat);
        break;

      case LSS_STOR_CONF:
        if (MY_NMT_STATE == NMTSTATE_LSS)
          LSS_StoreConfiguration();
        break;

      case LSS_MICROLSS:
        if (MY_NMT_STATE == NMTSTATE_LSS)
          LSS_IdentifyRemoteSlaves(pDat);
        break;

#if ! MLSS_ONLY
      case LSS_REQID_NCONF:
        if (MY_NMT_STATE == NMTSTATE_LSS)
          LSS_IdentifyNonconfigRemoteSlaves();
        break;

      case LSS_SWMOD_VID:
      case LSS_SWMOD_PID:
      case LSS_SWMOD_REV:
      case LSS_SWMOD_SER:
        LSS_SwitchModeSelective(pDat);
        break;
#endif

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
UNSIGNED8 ret_val;

  ret_val = FALSE;
  if (mLSS.active)
  {
    ret_val = TRUE;
#if USE_LEDS
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
#endif
  }
  else
  {
    // Has LSS just been finished?
    if (MY_NMT_STATE == NMTSTATE_LSS)
    {
      // Re-init
      // apply new node ID NOW
      MY_NODE_ID = mLSS.new_node_id;
      MCOUSER_ResetCommunication();
      ret_val = TRUE;
    }
  }
  return ret_val;
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

#ifdef P101801_VENDOR_ID
  PI_READ(PIACC_APP,P101801_VENDOR_ID,&(mLSS_ID[0]),4);
#else
  mLSS_ID[0] = OD_VENDOR_ID;
#endif

#ifdef P101802_PRODUCT_CODE
  PI_READ(PIACC_APP,P101802_PRODUCT_CODE,&(mLSS_ID[1]),4);
#else
  mLSS_ID[1] = OD_PRODUCT_CODE;
#endif

#ifdef P101803_REVISION_NUMBER
  PI_READ(PIACC_APP,P101803_REVISION_NUMBER,&(mLSS_ID[2]),4);
#else
  mLSS_ID[2] = OD_REVISION;
#endif
  
#if USECB_ODSERIAL
  mLSS_ID[3] = MCOUSER_GetSerial();
#else
 #ifdef P101804_SERIAL_NUMBER
  PI_READ(PIACC_APP,P101804_SERIAL_NUMBER,&(mLSS_ID[3]),4);
 #else
  mLSS_ID[3] = OD_SERIAL;
 #endif
#endif

  // Init MicroLSS state machine
  mLSS.mLSS_state = 0;

  // The Node ID is not yet set
  mLSS.node_id_set    = FALSE;

  // After reset, the node is in operation mode
  mLSS.operation_mode = TRUE;

  // Pre-set LSS baudrate and node ID to default values
  mLSS.new_node_bps   = 4; // 125kbps
  mLSS.new_node_id    = node_id;

  // Memorize the old node ID for inquiry command
  mLSS.old_node_id    = node_id;

  if (node_id == 0)
  { // No node ID, remain in LSS mode
    MY_NMT_STATE = NMTSTATE_LSS;
    mLSS.active = TRUE;
  }

}

#endif 

/*******************************************************************************
END OF FILE
*******************************************************************************/

