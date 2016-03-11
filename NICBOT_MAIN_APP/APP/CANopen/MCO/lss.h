/**************************************************************************
MODULE:    LSS_SLV
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

#ifndef _LSS_H
#define _LSS_H

#ifdef __cplusplus
extern "C" {
#endif

#include "mco.h"

// LSS CAN bps values
#define LSS_BPS_10   8
#define LSS_BPS_20   7
#define LSS_BPS_50   6
#define LSS_BPS_125  4
#define LSS_BPS_250  3
#define LSS_BPS_500  2
#define LSS_BPS_800  1
#define LSS_BPS_1000 0

// Only use the rest of this file if LSS support is enabled
#if USE_LSS_SLAVE

// Doing LSS in stopped state
#define NMTSTATE_LSS 0xF0

// LSS Command Specifiers
#define LSS_SWMOD_GLOB      4
#define LSS_SWMOD_VID      64
#define LSS_SWMOD_PID      65
#define LSS_SWMOD_REV      66
#define LSS_SWMOD_SER      67
#define LSS_SWMOD_RESP     68
#define LSS_CONF_NID       17
#define LSS_CONF_BIT       19
#define LSS_ACT_BIT        21
#define LSS_STOR_CONF      23
#define LSS_INQ_VID        90
#define LSS_INQ_PID        91
#define LSS_INQ_REV        92
#define LSS_INQ_SER        93
#define LSS_INQ_NID        94
#define LSS_REQID_VID      70
#define LSS_REQID_PID      71
#define LSS_REQID_REV_LO   72
#define LSS_REQID_REV_HI   73
#define LSS_REQID_SER_LO   74
#define LSS_REQID_SER_HI   75
#define LSS_REQID_NCONF    76
#define LSS_ID_SLAVE       79
#define LSS_ID_NCONF_SLAVE 80

// Smart LSS Command Specifiers
#define LSS_REQID_VID_LO  100
#define LSS_REQID_VID_HI  101
#define LSS_REQID_PID_LO  102
#define LSS_REQID_PID_HI  103

// Offsets into NVOL memory
#define NVOL_LSSENA 0
#define NVOL_LSSNID 1
#define NVOL_LSSBPS 2
#define NVOL_LSSCHK 3

/*************************************************************************
Function Prototypes
*************************************************************************/

/****************************************************************
DOES:    Process all LSS messages.
RETURNS:
*****************************************************************/
void LSS_HandleMsg (
  UNSIGNED8 Len,
  UNSIGNED8 *pDat
  );

/****************************************************************
DOES:    Check and update LSS status
RETURNS: FALSE: LSS is finished for this node
         TRUE:  Otherwise (LSS is still in process)
*****************************************************************/
UNSIGNED8 LSS_DoLSS (
  void
  );

/****************************************************************
DOES:    Initialize LSS mechanism (variables etc.)
GLOBALS: Sets lss_active status flag to TRUE
         Sets gCCOConfig.nmt_state to NMT_STATE_LSS
RETURNS: -
*****************************************************************/
void LSS_Init(
  UNSIGNED8 node_id // current node id or zero if none
  );


/****************************************************************
DOES:    LSS Load Configuration Command
RETURNS: Retrieves the current configuration stored in memory.
         Node ID is set to zero, if no configuration found
*****************************************************************/
void LSS_LoadConfiguration (
  UNSIGNED16 *Baudrate,  // returns CAN baudrate in kbit (1000,800,500,250,125,50,25 or 10)
  UNSIGNED8 *Node_ID    // returns CANopen node ID (0-127)
  );


#endif  // if (USE_LSS != USE_LSS_NONE)

#ifdef __cplusplus
}
#endif

#endif  // if _LSS_H
/*******************************************************************************
END OF FILE
*******************************************************************************/
