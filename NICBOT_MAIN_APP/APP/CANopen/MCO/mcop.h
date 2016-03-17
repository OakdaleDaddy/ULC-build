/**************************************************************************
MODULE:    MCOP
CONTAINS:  MicroCANopen Plus implementation
COPYRIGHT: Embedded Systems Academy, Inc. 2002-2015.
           All rights reserved. www.microcanopen.com
DISCLAIM:  Read and understand our disclaimer before using this code!
           www.esacademy.com/disclaim.htm
           This software was written in accordance to the guidelines at
           www.esacademy.com/software/softwarestyleguide.pdf
LICENSE:   THIS IS THE COMMERCIAL VERSION OF MICROCANOPEN PLUS
           ONLY USERS WHO PURCHASED A LICENSE MAY USE THIS SOFTWARE
           See file license_commercial_plus.txt or
           www.microcanopen.com/license_commercial_plus.txt
VERSION:   6.20, ESA 11-MAY-15
           $LastChangedDate: 2015-05-09 19:41:45 -0300 (Sat, 09 May 2015) $
           $LastChangedRevision: 3390 $
***************************************************************************/ 

#ifndef _MCOP_H
#define _MCOP_H

#ifdef __cplusplus
extern "C" {
#endif

#include "mco.h"


/**************************************************************************
Defines for SLEEP modes
**************************************************************************/
#if USE_SLEEP
#define SLEEP_MASTER_QUERY_OBJECTION 0x01
#define SLEEP_SLAVE_SLEEP_OBJECTION 0x81
#define SLEEP_MASTER_SET_SLEEP 0x02
#if USE_LSS_SLAVE
 #define SLEEP_WAKEUP 0x81
#else
 #define SLEEP_WAKEUP 0x82
#endif
#define SLEEP_SLAVE_REQUEST 0x03
#define SLEEP_REASON_NONE 0xFE
#endif // USE_SLEEP


#if USE_EMCY
// This structure holds all emergency configuration
typedef struct
{
  CAN_MSG   emcy_msg;
  UNSIGNED16 emcy_inhibit;       // Emergency inhibit time in ms
  UNSIGNED16 emcy_timestamp;     // Timestamp of next allowed emergency
#if ERROR_FIELD_SIZE > 0
  UNSIGNED32 Field[ERROR_FIELD_SIZE];
  UNSIGNED8 InPtr;
  UNSIGNED8 NrOfRec;
#endif
} EMCY_CONFIG; // emergency handling data record
#endif


#if (NR_OF_HB_CONSUMER > 0)
// This structure holds all node specific configuration
typedef struct
{
  UNSIGNED16 time;        // Heartbeat Consumer time in ms
  UNSIGNED16 timestamp;   // Timestamp of last heartbeat consumed
  UNSIGNED16 can_id;      // CAN ID to monitor 0x700 + node ID
  HBCONS_STATE status;    // satte of this consumer: off, init, active, lost
} HBCONS_CONFIG;
#endif // (NR_OF_HB_CONSUMER > 0)


/**************************************************************************
DOES:    This function reads data from the process image and copies it
         to an OUTPUT location
RETURNS: Number of bytes that were copied
**************************************************************************/
UNSIGNED8 MCO_ReadProcessData (
  UNSIGNED8 MEM_PROC *pDest, // Destination pointer
  UNSIGNED8 length, // Number of bytes to copy
  UNSIGNED16 offset // Offset of source data in process image
  );


/**************************************************************************
DOES:    This function writes data from an INPUT location to the process 
         image
RETURNS: Number of bytes that were copied
**************************************************************************/
UNSIGNED8 MCO_WriteProcessData (
  UNSIGNED16 offset, // Offset of destination data in process image
  UNSIGNED8 length,  // Number of bytes to copy
  UNSIGNED8 MEM_PROC *pSrc // Source pointer
  );

/**************************************************************************
DOES:    Transmits an Debug Message
RETURNS: 1 if message was transmitted, 0 if transmit queue is full
**************************************************************************/
UNSIGNED8 MCOP_PushDebug
  (
  UNSIGNED32 a, // 32 bit data
  UNSIGNED32 b  // 32 bit data
  );

#if USE_EMCY
/**************************************************************************
DOES:    Transmits an Emergency Message
RETURNS: 1 if message was transmitted, 0 if transmit queue is full
**************************************************************************/
UNSIGNED8 MCOP_PushEMCY
  (
  UNSIGNED16 emcy_code, // 16 bit error code
  UNSIGNED8  em_1, // 5 byte manufacturer specific error code
  UNSIGNED8  em_2,
  UNSIGNED8  em_3,
  UNSIGNED8  em_4,
  UNSIGNED8  em_5
  );


#if ERROR_FIELD_SIZE > 0
#endif
/**************************************************************************
DOES:    This function clears all entries of the error history [1003h]
RETURNS: Nothing
**************************************************************************/
void MCOP_ErrField_Flush (void);


/**************************************************************************
DOES:    This function retrieves an entry from the error history [1003h]
         based on the subindex of [1003h]
RETURNS: 32bit error code stored at a subindex
**************************************************************************/
UNSIGNED32 MCOP_ErrField_Get (
  UNSIGNED8 subindex // Subindex number of [1003h]
  );
#endif


#if (MGR_MONITOR_ALL_NODES == 0)
/* Device version, for SELECTED heartbeats, Manager uses comgr.h*/
#if (NR_OF_HB_CONSUMER > 0)
/**************************************************************************
DOES:    Initializes Heartbeat Consumer
GLOBALS: Inits gHBCons[consumer_channel-1]
**************************************************************************/
void MCOP_InitHBConsumer (
  UNSIGNED8 consumer_channel, // HB Consumer channel
  UNSIGNED8 node_id, // Node ID to monitor
  UNSIGNED16 hb_time // Timeout ot use (in ms)
  );


/**************************************************************************
DOES:    Checks if a message received contains a heartbeat to be consumed
GLOBALS: Updates gHBCons[consumer_channel-1]
RETURNS: one, if message received was a heartbeat monitored, else zero
**************************************************************************/
UNSIGNED8 MCOP_ConsumeHB (
  CAN_MSG MEM_FAR *pRxCAN // CAN message received
  );


/**************************************************************************
DOES:    Checks if a heartbeat consumer timeout occured
RETURNS: 
          HBCONS_OFF,    // Disabled
          HBCONS_INIT,   // Initialized, waiting for first 2 heartbeats
          HBCONS_ACTIVE, // Consumer active and OK
          HBCONS_LOST    // Heartbeat lost
**************************************************************************/
HBCONS_STATE MCOP_ProcessHBCheck (
  UNSIGNED8 consumer_channel
  );


/**************************************************************************
DOES:    Checks if a node ID is already used, needed for conformance test
RETURNS: TRUE, if node_id is already used
**************************************************************************/
UNSIGNED8 MCOP_IsHBMonitored (
  UNSIGNED8 channel,
  UNSIGNED8 node_id
  );
#endif // (NR_OF_HB_CONSUMER > 0)
#endif // ! MGR_MONITOR_ALL_NODES


#if USECB_NMTCHANGE
/**************************************************************************
DOES:    This function is called before a change to the NMT Slave state 
         machine is applied. Values passed are one of the following:
         NMTSTATE_BOOT, NMTSTATE_STOP, NMTSTATE_OP or NMTSTATE_PREOP
RETURNS: nothing
**************************************************************************/
void MCOUSER_NMTChange (
  UNSIGNED8 NMTState
  );
#endif // USECB_NMTCHANGE


#if USECB_SDOREQ
/**************************************************************************
DOES:    This call-back function is called by the SDO Handler for unknown
         SDO Requests. This allows the application to add own, custom
         Object Dictionary entries. When using this call-back function,
         the function is responsible for generating the right response or
         abort. The function MCO_ReplyWith can be used to generate a
         response, the function MCO_SendSDOAbort to generate an abort.
RETURNS: 0 - An SDO Abort was generated by the call-back function
         1 - An SDO Response was generated by the call-back function
         2 - The call-back function did not do anything with Request
**************************************************************************/
UNSIGNED8 MCOUSER_SDORequest (
  UNSIGNED8 SDO_Data[8]
  );
#endif // USECB_SDOREQ


/**************************************************************************
INTERNAL FUNCTIONS FROM MCO.C AND MCOP.C 
**************************************************************************/

/**************************************************************************
DOES:    Generates an SDO Abort Response
RETURNS: nothing
**************************************************************************/
void MCO_SendSDOAbort (
  UNSIGNED32 ErrorCode  // 4 byte SDO abort error code
  );


/**************************************************************************
DOES:    Common exit routine for SDO_Handler. 
         Send SDO response with variable length (1-4 bytes).
         Assumes that gTxCAN.ID, LEN and BUF[1-3] are already set.
         Requires data to be in little endian format.
RETURNS: 1 if response transmitted 
**************************************************************************/
UNSIGNED8 MCO_ReplyWith (
  UNSIGNED8 *pDat,  // pointer to sdo data
  UNSIGNED8 len     // number of bytes of data in SDO
  );


/**************************************************************************
DOES:    Handles incoming SDO Request for accesses to PDO Communication
         Parameters
RETURNS: 0: No action, SDO request is not for PDO Communication
         1: Access was made, SDO Response sent
         2: Wrong access, SDO Abort sent
GLOBALS: Various global variables with configuration information
**************************************************************************/
UNSIGNED8 SDO_HandlePDOComParam (
  UNSIGNED8  PDOType,  // 0 for TPDO, 1 for RPDO
  UNSIGNED16 index,    // OD index
  UNSIGNED8 *pData    // pointer to SDO Request message
  );


#if USE_DYNAMIC_PDO_MAPPING
/**************************************************************************
DOES:    Handles incoming SDO Request for accesses to PDO Mapping
         Parameters
RETURNS: 0: No action, SDO request is not for PDO Mapping
         1: Access was made, SDO Response sent
         2: Wrong access, SDO Abort sent
GLOBALS: Various global variables with configuration information
**************************************************************************/
UNSIGNED8 SDO_HandlePDOMapParam (
  UNSIGNED8  PDOType,  // 0 for TPDO, 1 for RPDO
  UNSIGNED16 index,    // OD index
  UNSIGNED8  *pData    // pointer to SDO Request message
  );
#endif


#if USE_SYNC
/**************************************************************************
DOES:    Processes reception of the SYNC message
RETURNS: 0: No messages processed
         Bit 0 set: SYNC TPDOs transmitted
         Bit 1 set: SYNC RPDOs received
**************************************************************************/
UNSIGNED8 MCOP_HandleSYNC (
  void
  );
#endif


/**************************************************************************
DOES:    Called when a TPDO needs to be transmitted
RETURNS: nothing
**************************************************************************/
void MCO_TransmitPDO (
  UNSIGNED16 PDONr  // TPDO number to transmit
  );


#if USE_INHIBIT_TIME
/**************************************************************************
DOES:    Called by application when a TPDO should be transmitted
RETURNS: nothing
**************************************************************************/
void MCO_TriggerTPDO (
  UNSIGNED16 TPDONr  // TPDO number to transmit, as gTPDOConfig[] index
  );
#endif


/**************************************************************************
DOES:    Checks if message received is guarding request
RETURNS: 0: message received is not guarding request
         1: guarding request received, response sent
**************************************************************************/
UNSIGNED8 MCOP_HandleGuarding (
  UNSIGNED16 can_id
  );


/**************************************************************************
DOES:    Function allowing application access to NMT changes
**************************************************************************/
void MCO_HandleNMTRequest (
  UNSIGNED8 NMTReq
  );


#if USE_STORE_PARAMETERS
/**************************************************************************
DOES:    Determines offset values for the the different regions
         in non-volatile memory.
         1. LSS + XOD binary eds checksum
         2. Save Parameters 1XXX
         3. Save Parameters 6XXX
         4. Save Parameters 2XXX
         5. First free space
RETURNS: Info on start offsets is copied into the string (5 values)
**************************************************************************/
void MCOSP_GetNVOLUsage (
  UNSIGNED16 pLoc[5] // info is written into this string
  );


/**************************************************************************
DOES:    Checks the non-volatile memory for saved parameters and retrives
         them all
RETURNS: Nothing.
**************************************************************************/
void MCOSP_GetStoredParameters (
  void  
  );


/**************************************************************************
DOES:    Implements the Store Parameters and Restore Parameters
         functionaility.
RETURNS: TRUE, if data was restored
         FALSE. if no valid data was found
**************************************************************************/
UNSIGNED8 MCOSP_StoreParameters (
  UNSIGNED16 idx, // set to 0x1010 for store, to 0x1011 for restore
  UNSIGNED8 sub // subindex
  );


/**************************************************************************
DOES:    Initializes access to non-volatile memory.
**************************************************************************/
void NVOL_Init (
  void
  );

/**************************************************************************
DOES:    Reads a data byte from non-volatile memory.
         Must be provided by application driver.
NOTE:    The address is relative, an offset to NVOL_STORE_START
RETURNS: The data read from memory
**************************************************************************/
UNSIGNED8 NVOL_ReadByte (
  UNSIGNED16 address // location of byte in NVOL memory
  );


/**************************************************************************
DOES:    Writes a data byte to non-volatile memory
         Must be provided by application driver.
NOTE:    The address is relative, an offset to NVOL_STORE_START
RETURNS: nothing
**************************************************************************/
void NVOL_WriteByte (
  UNSIGNED16 address, // location of byte in NVOL memory
  UNSIGNED8 data
  );


/**************************************************************************
DOES:    Is called when a consecutive block of write cycles is complete. 
         The driver may buffer the data from calls to NVOL_WriteByte with
         consecutive destination addresses in RAM and then write the entire 
         buffer to non-volatile memory upon a call to this function.
**************************************************************************/
void NVOL_WriteComplete (
  void
  );
#endif // USE_STORE_PARAMETERS


#if USECB_ODSERIAL
/**************************************************************************
DOES:    Requests the device's serial number from the application.
         Must be passed in little endian format.
RETURNS: nothing
**************************************************************************/
UNSIGNED32 MCOUSER_GetSerial (
  void
  );
#endif // USECB_ODSERIAL

#if USE_SLEEP
/**************************************************************************
DOES:    DRIVER LEVEL: Sets the processor into sleep or power down mode.
         Called when a sleep request was received and confirmed.
         Wakeup MUST be through a reset.
**************************************************************************/
void MCOHW_Sleep (
  void
  );

/*******************************************************************************
DOES:    APPLICATION LEVEL: Call back function for sleep/wakeup messages 
         received as specified in CiA447 V2.
RETURNS: nothing
*******************************************************************************/
void MCOUSER_Sleep (
  UNSIGNED8 node,     // node ID of the node who sent the message
  UNSIGNED8 command,  // command byte of the message
  UNSIGNED8 reason    // reason byte of the message
  );

/*******************************************************************************
DOES:    Function to transmit wakeup/sleep messages as defined by CiA 447 V2
         SLEEP_MASTER_QUERY_OBJECTION 0x01
         SLEEP_SLAVE_SLEEP_OBJECTION 0x81
         SLEEP_MASTER_SET_SLEEP 0x02
         SLEEP_WAKEUP 0x82
         SLEEP_SLAVE_REQUEST 0x03
         SLEEP_REASON_NONE 0xFE
RETURNS: TRUE, if message was succesfully queued
*******************************************************************************/
UNSIGNED8 MCOP_TransmitWakeupSleep ( 
  UNSIGNED8 statcmd, 
  UNSIGNED8 reason 
  );
#endif // USE_SLEEP


#ifdef __cplusplus
}
#endif

#endif // _MCOP_H
/**************************************************************************
END OF FILE
**************************************************************************/
