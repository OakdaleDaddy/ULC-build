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
***************************************************************************/ 

#ifndef _MLSS_H
#define _MLSS_H

#ifdef __cplusplus
extern "C" {
#endif

#include "mco.h"


// LSS CAN bps values
#define LSS_BPS_125  4
#define LSS_BPS_250  3
#define LSS_BPS_500  2
#define LSS_BPS_800  1
#define LSS_BPS_1000 0

// Doing LSS in stopped state
#define NMTSTATE_LSS 0xF0

// LSS Command Specifiers
#define LSS_SWMOD_GLOB      4
#define LSS_SWMOD_RESP     68
#define LSS_CONF_NID       17
#define LSS_STOR_CONF      23
#define LSS_ID_SLAVE       79
#define LSS_ID_NCONF_SLAVE 80
#define LSS_MICROLSS       81
// Older commands for backward compatibility
#define LSS_SWMOD_VID      64
#define LSS_SWMOD_PID      65
#define LSS_SWMOD_REV      66
#define LSS_SWMOD_SER      67

// Baqckward compatibility
#define LSS_REQID_NCONF    76

// Offsets into NVOL memory
#define NVOL_LSSENA 0
#define NVOL_LSSNID 1
#define NVOL_LSSBPS 2
#define NVOL_LSSCHK 3

// LSS Enabled value
#define NVOL_LSSENA_VAL 0x5A

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


#if USE_29BIT_LSSFEEDBACK == 1
/****************************************************************
DOES:    Sends an LSS response feedback msg, this transmits
         a 29bit CAN ID message with DLC = 0
RETURNS: -
*****************************************************************/
void MCOHW_Push29Message (
  UNSIGNED32 canid // CAN ID to use
  );
#endif


#if USE_STORE_PARAMETERS
/****************************************************************
DOES:    LSS Load Configuration Command
RETURNS: Retrieves the current configuration stored in memory.
         Node ID is set to zero, if no configuration found
*****************************************************************/
void LSS_LoadConfiguration (
  UNSIGNED16 *Baudrate,  // returns CAN baudrate in kbit (1000,800,500,250,125,50,25 or 10)
  UNSIGNED8 *Node_ID    // returns CANopen node ID (0-127)
  );
#endif

#ifdef __cplusplus
}
#endif

#endif  // if _MLSS_H
/*******************************************************************************
END OF FILE
*******************************************************************************/
