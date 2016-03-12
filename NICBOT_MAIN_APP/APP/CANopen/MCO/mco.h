/**************************************************************************
MODULE:    MCO
CONTAINS:  Main MicroCANopen implementation
COPYRIGHT: Embedded Systems Academy, Inc. 2002-2015.
           All rights reserved. www.microcanopen.com
DISCLAIM:  Read and understand our disclaimer before using this code!
           www.esacademy.com/disclaim.htm
           This software was written in accordance to the guidelines at
           www.esacademy.com/software/softwarestyleguide.pdf
LICENSE:   This file may be freely distributed.
VERSION:   6.20, ESA 11-MAY-15
           $LastChangedDate: 2015-05-09 19:41:45 -0300 (Sat, 09 May 2015) $
           $LastChangedRevision: 3390 $
***************************************************************************/ 


#ifndef _MCO_H
#define _MCO_H

#ifdef __cplusplus
extern "C" {
#endif


/**************************************************************************
Version Information, here V6.11
**************************************************************************/
#define _MCOPVERSION_ 6
#define _MCOSUBPVERSION_ 11

  
  // The following configuration files need to be supplied by the application
#include "nodecfg.h"
#include "procimg.h"


/**************************************************************************
ENSURE THAT IN REGARDS TO OPTIMIZATION THE DEFAULT CONFIGURATION IS USED, 
IF NOT SELECTED OTHERWISE in nodecfg.h
**************************************************************************/

#if (ENFORCE_DEFAULT_CONFIGURATION == 1)

// 1 or more TPDOs use the event timer
#define USE_EVENT_TIME 1
// 1 or more TPDOs are change-of-state and use the inhibit timer
#define USE_INHIBIT_TIME 1
// 1 or more PDOs use the SYNC signal
#define USE_SYNC 1
// 1 to allow PDO mapping to be configurable
#ifndef USE_DYNAMIC_PDO_MAPPING
#define USE_DYNAMIC_PDO_MAPPING 1
#endif
// Do not use static PDO comunication parameters
#define USE_STATIC_PDO 0
// Heartbeat consumer is dynamic / configurable
#define DYNAMIC_HB_CONSUMER 1
// Use MicroCANopen Plus functionality
#define USE_MCOP 1
// Produce emergencies when required
#define USE_EMCY 1
// Size of error field history [1003h]
#define ERROR_FIELD_SIZE 4
// Support segmented SDOs
#define USE_EXTENDED_SDO 1
// Do not use Node Guarding
#define USE_NODE_GUARDING 0
// If enabled, the call-back function MCOUSER_SDORequest() is called every
// time the CANopen stack receives an SDO Request that it cannot handle
#define USECB_SDOREQ 0
// If enabled, use extended call backs MCOUSER_XSDOInitWrite()
// and MCOUSER_XSDOWriteSegment() to enable custom segmented SDO handling
// NOTE: This requires a complete custom SDO handler!
#define USE_XSDOCB_WRITE 0
// Custom SDO channel handling
#define USE_CiA447 0
#define USE_SDOMESH 0

#endif

// Manager functionality, if not defined, disable
#ifndef MGR_MONITOR_ALL_NODES
  #define MGR_MONITOR_ALL_NODES 0
#endif


/**************************************************************************
CANopen NMT (Network Management) Master Msg and Slave States
**************************************************************************/
#define NMTMSG_OP 1
#define NMTMSG_STOP 2
#define NMTMSG_PREOP 128
#define NMTMSG_RESETAPP 129
#define NMTMSG_RESETCOM 130

#define NMTSTATE_BOOT 0
#define NMTSTATE_STOP 4
#define NMTSTATE_OP 5
#define NMTSTATE_PREOP 127


/**************************************************************************
Error codes used when calling MCOUSER_FatalError
**************************************************************************/
#define ERR_WARN     0x4000 // Warning only, continue execution
#define ERROFL_EMCY  0x4810 // Transmit buffer overflow, TPDO message lost
#define ERROFL_PDO   0x4820 // Transmit buffer overflow, TPDO message lost
#define ERROFL_SDO   0x4830 // Transmit buffer overflow, SDO message lost
#define ERROFL_HBT   0x4840 // Transmit buffer overflow, Heartbeat message lost
#define ERR_FATAL    0x8000 // Fatal error, should abort/reset
#define ERRFT_INIT   0x8010 // MCO Init failed
#define ERRFT_RXFLTN 0x8021 // Out of CAN receive filters, NMT
#define ERRFT_RXFLTP 0x8022 // Out of CAN receive filters, PDO
#define ERRFT_RXFLTS 0x8023 // Out of CAN receive filters, SDO
#define ERRFT_IPP    0x8031 // Init PDO Parameters out of range
#define ERRFT_PIR    0x8032 // Process Image access out of range
#define ERRFT_TPDOR  0x8041 // Out of TPDOs
#define ERRFT_RPDOR  0x8042 // Out of RPDOs
#define ERRFT_RPMAP  0x8043 // Found no RPDO Mapping


/**************************************************************************
DEFINES FOR ACCESS TYPE TO OD ENRIES
Readable, Writable, Read-Mapable, Write-Mapable, Call-Back
**************************************************************************/
#define ODRD 0x10
#define ODWR 0x20
#define RMAP 0x40
#define WMAP 0x80
#define CALB 0x08


/**************************************************************************
MACROS FOR PLATFORM-ENDIANNESS-INDEPENDENT LITTLE-ENDIAN MEMORY ACCESS
**************************************************************************/
#define GEN_RD32(ptr)  ( ((UNSIGNED32)(*((UNSIGNED8 *)ptr+3))        << 24) | \
                         ((UNSIGNED32)(*((UNSIGNED8 *)ptr+2) & 0xFF) << 16) | \
                         ((UNSIGNED32)(*((UNSIGNED8 *)ptr+1) & 0xFF) <<  8) | \
                          (UNSIGNED32)(*((UNSIGNED8 *)ptr  ) & 0xFF)        )

#define GEN_RD24(ptr)  ( ((UNSIGNED32)(*((UNSIGNED8 *)ptr+2) & 0xFF) << 16) | \
                         ((UNSIGNED32)(*((UNSIGNED8 *)ptr+1) & 0xFF) <<  8) | \
                          (UNSIGNED32)(*((UNSIGNED8 *)ptr  ) & 0xFF)        )

#define GEN_RD16(ptr)  ( ((UNSIGNED16)(*((UNSIGNED8 *)ptr+1))        << 8) |  \
                          (UNSIGNED16)(*((UNSIGNED8 *)ptr  ) & 0xFF)       )

#define GEN_RD8(ptr)   (*((UNSIGNED8 *)ptr) & 0xFF)

#define GEN_WR32(ptr,value) \
  do {                             \
    *((UNSIGNED8 *)ptr+3) = (UNSIGNED8)(((UNSIGNED32)(value) >> 24) & 0xFF); \
    *((UNSIGNED8 *)ptr+2) = (UNSIGNED8)(((UNSIGNED32)(value) >> 16) & 0xFF); \
    *((UNSIGNED8 *)ptr+1) = (UNSIGNED8)(((UNSIGNED32)(value) >>  8) & 0xFF); \
    *((UNSIGNED8 *)ptr  ) = (UNSIGNED8)( (UNSIGNED32)(value)        & 0xFF); \
  } while (0)
    
#define GEN_WR24(ptr,value) \
  do {                             \
    *((UNSIGNED8 *)ptr+2) = (UNSIGNED8)(((UNSIGNED32)(value) >> 16) & 0xFF); \
    *((UNSIGNED8 *)ptr+1) = (UNSIGNED8)(((UNSIGNED32)(value) >>  8) & 0xFF); \
    *((UNSIGNED8 *)ptr  ) = (UNSIGNED8)( (UNSIGNED32)(value)        & 0xFF); \
  } while (0)
    
#define GEN_WR16(ptr,value) \
  do {                             \
    *((UNSIGNED8 *)ptr+1) = (UNSIGNED8)(((UNSIGNED16)(value) >>  8) & 0xFF); \
    *((UNSIGNED8 *)ptr  ) = (UNSIGNED8)( (UNSIGNED16)(value)        & 0xFF); \
  } while (0)

#define GEN_WR8(ptr,value)  \
  do {                             \
    *((UNSIGNED8 *)ptr  ) = (UNSIGNED8)( value        & 0xFF); \
  } while (0)


/**************************************************************************
MACROS FOR OBJECT DICTIONARY ENTRIES
**************************************************************************/
#define GETBYTE(val,pos) (((val) >> (pos)) & 0xFF)
#define GETBYTES16(val) GETBYTE(val, 0), GETBYTE(val, 8)
#define GETBYTES32(val) GETBYTE(val, 0), GETBYTE(val, 8), GETBYTE(val,16), GETBYTE(val,24)

#define SDOREPLY(index,sub,len,val)  0x43 | ((4-len)<<2), GETBYTES16(index), sub, GETBYTES32(val)
#define SDOREPLY4(index,sub,len,d1,d2,d3,d4)  0x43 | ((4-len)<<2), GETBYTES16(index), sub, d1, d2, d3, d4

#define ODENTRY(index,sub,len,offset) { GETBYTES16(index), sub, len, GETBYTES16(offset) }

#if USE_EXTENDED_SDO
 #if USE_GENOD_PTR
  #define ODGENTRYC(index,sub,acc,len,ptr) { GETBYTES16(index), sub, acc, GETBYTES16(len), (UNSIGNED8 *) ptr }
  #define ODGENTRYP(index,sub,acc,len,off) { GETBYTES16(index), sub, acc, GETBYTES16(len), &(gProcImg[off]) }
 #else
  #define ODGENTRYP(index,sub,acc,len,off) { GETBYTES16(index), sub, acc, GETBYTES16(len), GETBYTES16(off) }
 #endif
#endif // USE_EXTENDED_SDO


/**************************************************************************
DO NOT USE THESE PIxxACC MACROS!
USE PI_READ AND PI_WRITE INSTEAD!
NEW VERSIONS SUPPORT RTOS, DATA CONSITENCY, SIMULATION
MACROS FOR PROCESS IMAGE ACCESS 8bit, 16bit and 32bit access
**************************************************************************/
/*
#define PI8ACC(offset) (gProcImg[offset])
#define PI16ACC(offset) (*((UNSIGNED16 *) &(gProcImg[offset])))
#define PI32ACC(offset) (*((UNSIGNED32 *) &(gProcImg[offset])))
*/


/**************************************************************************
MACROS FOR ACCESS TO INTERNAL VARIABLES SOMETIMES NEEDED BY APPLICATION
**************************************************************************/
#define MY_NODE_ID (gMCOConfig.Node_ID)
#define MY_NMT_STATE (gMCOConfig.heartbeat_msg.BUF[0])


/**************************************************************************
MACROS FOR CAN-ID DEFINITION
**************************************************************************/
// CAN-ID used for NMT Master message
#define NMT_MASTER_ID 0

// CAN-ID for LSS reception (slave) or transmission (master)
#define LSS_MASTER_ID 2021ul

// CAN-ID for LSS transmission (slave) or reception (master)
#define LSS_SLAVE_ID 2020ul


/**************************************************************************
MACROS FOR CAN-ID USAGE
**************************************************************************/
#define IS_CANID_LSS_RESPONSE(canid) (canid == LSS_SLAVE_ID)

#define IS_CANID_RESTRICTED(canid) ( (canid < 0x0080) \
    || ((canid >  0x0100) && (canid <= 0x0180)) \
    || ((canid >  0x0580) && (canid <  0x0600)) \
    || ((canid >  0x0600) && (canid <  0x0680)) \
    || ((canid >= 0x06E0) && (canid <  0x0700)) \
    || ((canid >  0x0700) && (canid <  0x0800)) )

#if (USE_CiA447 == 1)

// SDO Transfers: Server = j, Client = i
// Request from client node i to server node j
// (0x240 + ((i-1) & 0xC) << 6) + (((i-1) & 0x03) << 4) + j-1) 
// Response from server node j to client node i
// (0x1C0 + ((i-1) & 0xC) << 6) + (((i-1) & 0x03) << 4) + j-1) 
#define IS_CAN_ID_EMERGENCY(canid) ((canid >= 0x081) && (canid <= 0x090))
#define IS_CAN_ID_HEARTBEAT(canid) ((canid >= 0x701) && (canid <= 0x710))
#define IS_CAN_ID_ISO_TP(canid)   ( \
       (canid==0x251) || (canid==0x262) || (canid==0x273) \
    || (canid==0x344) || (canid==0x355) || (canid==0x366) \
    || (canid==0x377) || (canid==0x448) || (canid==0x459) \
    || (canid==0x46A) || (canid==0x47B) || (canid==0x54C) \
    || (canid==0x55D) || (canid==0x56D) || (canid==0x56E) \
    || (canid==0x57F) \
  )

#ifdef P61C200_TESTER_COMMAND_2
UNSIGNED8 IS_CAN_ID_SDOREQUEST (UNSIGNED16 canid);
#else
#define IS_CAN_ID_SDOREQUEST(canid) ( \
       (canid == 0x600+MY_NODE_ID) \
    || (    (canid > 0x240) \
         && (canid < 0x580) \
         && ((canid & 0x00CF) == (0x040 + MY_NODE_ID - 1)) \
       ) \
  )
#endif

#define IS_CAN_ID_SDORESPONSE(canid) ( \
       (canid > 0x1C0ul) \
    && (canid < 0x4FFul) \
    && ((canid & 0x07F0ul) \
         == 0x1C0ul + (((MY_NODE_ID - 1) & 0x0Cul) << 6) + (((MY_NODE_ID - 1) & 0x03ul) << 4) \
       ) \
  )

#define CAN_ID_SDORESPONSE_FROM_RXID(canid) ( canid - 0x80 )
#define CAN_ID_SDOREQUEST(client_nodeid, server_nodeid) ( \
    0x240 +  (((client_nodeid-1) & 0x0C) << 6) \
          +  (((client_nodeid-1) & 0x03) << 4) \
          +  server_nodeid-1 \
  )
#define CAN_ID_SDORESPONSE(client_nodeid, server_nodeid) ( \
    0x1C0 +  (((client_nodeid-1) & 0x0C) << 6) \
          +  (((client_nodeid-1) & 0x03) << 4) \
          + server_nodeid-1 \
  )
#define SDOSERVER(tx_canid) ( \
    (tx_canid == 0x580 + MY_NODE_ID) \
      ? (MY_NODE_ID-1) \
      : ((((tx_canid-0x100) >> 6) & 0x0C) + ((tx_canid >> 4) & 0x03)) \
  )

#define CAN_ID_PDO_CiA447(node) (0x40000180ul + (((UNSIGNED16)((node-1)&7)<<7)) + (((UNSIGNED16)((node-1)&8))<<2))

#else

#if (USE_SDOMESH == 1)

#define IS_CAN_ID_EMERGENCY(canid) ((canid >= 0x081) && (canid <= 0x0FF))
#define IS_CAN_ID_HEARTBEAT(canid) ((canid >= 0x701) && (canid <= 0x77F))

#define IS_CAN_ID_SDOREQUEST(canid) ( \
       ((canid & 0x700) == 0x600) \
    && ((canid & 0x00F) == (MY_NODE_ID - 1)) \
  )
#define IS_CAN_ID_SDORESPONSE(canid) ( \
       (   (MY_NODE_ID < 9) \
        && ((canid & 0x700) == 0x500) \
        && ((canid & 0x0F0) == ((MY_NODE_ID + 7) << 4)) \
       ) \
    ||   \
       (   (MY_NODE_ID > 8) \
        && ((canid & 0x700) == 0x100) \
        && ((canid & 0x0F0) == ((MY_NODE_ID - 9) << 4)) \
       ) \
  ) 
#define CAN_ID_SDORESPONSE_FROM_RXID(canid) ( \
    (((canid & 0x0F0) >> 4) < 8) \
      ? ( 0x500 +  ((((canid & 0x0F0) >> 4) + 8) << 4) \
                +  (canid & 0xF) \
        ) \
      : ( 0x100 +  ((((canid & 0x0F0) >> 4) - 8) << 4) \
                +  (canid & 0xF) \
        ) \
  )
#define CAN_ID_SDOREQUEST(client_nodeid, server_nodeid) ( \
    0x600 +  (((client_nodeid-1) & 0xF) << 4) \
          +  ((server_nodeid-1) & 0xF) \
  )
#define CAN_ID_SDORESPONSE(client_nodeid, server_nodeid) ( \
    (client_nodeid < 9) \
      ? 0x500 +  ((((client_nodeid-1) & 0xF) + 8) << 4) \
              +  ((server_nodeid-1) & 0xF) \
      : 0x100 +  ((((client_nodeid-1) & 0xF) - 8) << 4) \
              +  ((server_nodeid-1) & 0xF) \
  )
#define SDOSERVER(tx_canid) ( \
    ((gTxSDO.ID & 0x700) == 0x500) \
      ? (((gTxSDO.ID & 0x0F0) >> 4) - 8) \
      : (((gTxSDO.ID & 0x0F0) >> 4) + 8) \
  )

#else

#define IS_CAN_ID_EMERGENCY(canid) ((canid >= 0x081) && (canid <= 0x0FF))
#define IS_CAN_ID_HEARTBEAT(canid) ((canid >= 0x701) && (canid <= 0x77F))
#define IS_CAN_ID_SDOREQUEST(canid) (canid == 0x600+MY_NODE_ID)
#define IS_CAN_ID_SDORESPONSE(canid) (canid >= 0x581) && (canid <= 0x5FF)
#define CAN_ID_SDORESPONSE_FROM_RXID(canid) (canid - 0x80)
#define CAN_ID_SDOREQUEST(client_nodeid, server_nodeid)  ( 0x600 + server_nodeid )
#define CAN_ID_SDORESPONSE(client_nodeid, server_nodeid) ( 0x580 + server_nodeid )
#define SDOSERVER(tx_canid) 0

#endif

#endif


/**************************************************************************
SDO ABORT MESSAGES
**************************************************************************/
#define SDO_ABORT_TOGGLE          0x05030000UL
#define SDO_ABORT_UNKNOWN_COMMAND 0x05040001UL
#define SDO_ABORT_INVALID_SEQ     0x05040003UL
#define SDO_ABORT_CRC             0x05040004UL
#define SDO_ABORT_UNSUPPORTED     0x06010000UL
#define SDO_ABORT_WRITEONLY       0x06010001UL
#define SDO_ABORT_READONLY        0x06010002UL
#define SDO_ABORT_NOT_EXISTS      0x06020000UL
#define SDO_ABORT_PARAMETER       0x06040043UL
#define SDO_ABORT_TYPEMISMATCH    0x06070010UL
#define SDO_ABORT_DATATOBIG       0x06070012UL
#define SDO_ABORT_UNKNOWNSUB      0x06090011UL
#define SDO_ABORT_VALUE_RANGE     0x06090030UL
#define SDO_ABORT_VALUE_HIGH      0x06090031UL
#define SDO_ABORT_VALUE_LOW       0x06090032UL
#define SDO_ABORT_GENERAL         0x08000000UL
#define SDO_ABORT_TRANSFER        0x08000020UL
#define SDO_ABORT_NOTRANSFERCTRL  0x08000021UL
#define SDO_ABORT_NOTMAPPED       0x06040041UL
#define SDO_ABORT_MAPLENGTH       0x06040042UL


/**************************************************************************
Status bits for function MCOHW_GetStatus
**************************************************************************/
#define HW_INIT  0x01
#define HW_CERR  0x02
#define HW_ERPA  0x04
#define HW_RXOR  0x08
#define HW_TXOR  0x10
#define HW_TXBSY 0x40
#define HW_BOFF  0x80


/**************************************************************************
Defines for LED control
**************************************************************************/
#if USE_LEDS
// LED Flash Patterns
// Flickering directly implemented in lss.c
#define LED_OFF 0
#define LED_ON 0xFF
#define LED_BLINK 0x7F
// Single, double, triple and quadruple flashes
#define LED_FLASH1 1
#define LED_FLASH2 2
#define LED_FLASH3 3
#define LED_FLASH4 4
#endif // USE_LEDS


/**************************************************************************
States for Heartbeat Consumer handling 
**************************************************************************/
typedef enum {
  HBCONS_OFF,    // Disabled
  HBCONS_INIT,   // Initialized, waiting for second heartbeat
  HBCONS_ACTIVE, // Consumer active and OK
  HBCONS_LOST    // Heartbeat lost
} HBCONS_STATE;


/**************************************************************************
GLOBAL TYPE DEFINITIONS
**************************************************************************/

// Data structure for a single CAN message 
typedef struct
{ // order optimized for allignment
  UNSIGNED8 BUF[8];              // Data buffer 
  COBID_TYPE ID;                 // Message Identifier 
  UNSIGNED8 LEN;                 // Data length (0-8) 
} CAN_MSG;

// This structure holds all node specific configuration
typedef struct
{
  CAN_MSG heartbeat_msg;         // Heartbeat message contents
  UNSIGNED16 Baudrate;           // Current Baud rate in kbit
  UNSIGNED16 heartbeat_time;     // Heartbeat time in ms
  UNSIGNED16 heartbeat_timestamp;// Timestamp of last heartbeat
  UNSIGNED16 last_fatal;         // Last Fatal Error code
  UNSIGNED16 last_rxtime;        // Timestamp of last CAN receive
#if USE_SYNC
  UNSIGNED16 SYNCid;             // CAN ID used for SYNC
#endif
#if NR_OF_TPDOS > 255
  UNSIGNED16 nrTPDOs;
#else
 #if NR_OF_TPDOS > 0
  UNSIGNED8 nrTPDOs;
 #endif
#endif
#if NR_OF_RPDOS > 255
  UNSIGNED16 nrRPDOs;
#else
 #if NR_OF_RPDOS > 0
  UNSIGNED8 nrRPDOs;
 #endif
#endif
#if USE_LEDS
  UNSIGNED16 LED_timestamp;      // LED control timestamp
  UNSIGNED8 LEDtoggle;           // Toggler for blinking or flickering pattern
  UNSIGNED8 LEDRun;              // Current pattern on run led
  UNSIGNED8 LEDcntR;             // Current flash counter on run led
  UNSIGNED8 LEDErr;              // Current pattern on error led
  UNSIGNED8 LEDcntE;             // Current flash counter on error led
#endif // USE_LEDS
  UNSIGNED8 Node_ID;             // Current Node ID (1-126)
  UNSIGNED8 error_code;          // Bits: 0=RxQueue 1=TxQueue 3=CAN 7=first_op
  UNSIGNED8 error_register;      // Error regiter for OD entry [1001,00]
#if USE_NODE_GUARDING
  UNSIGNED8 NGtoggle;            // Toggle value for node guarding
#endif
  UNSIGNED8 HWStatus;            // CAN HW status
} MCO_CONFIG;

// Enumeration type for inhibit status
typedef enum
{
  INHITIM_EXPIRED,             // Inhibit timer not started or expired
  INHITIM_RUNNING_NO_TRIGGER,  // Inhibit timer started, not yet triggered for transmission
  INHITIM_RUNNING_TRIGGERED    // Transmit msg waiting for expiration of inhibit
} INHITIM_TYPE;

// This structure holds all the TPDO configuration data for one TPDO
typedef struct 
{
  CAN_MSG CAN;                 // Current/last CAN message to be transmitted
  UNSIGNED16 offset;           // Offest to application data in process image
  UNSIGNED16 PDONr;            // PDO number (1-512)
#if USE_EVENT_TIME
  UNSIGNED16 event_time;       // Event timer in ms (0 for COS only operation)
  UNSIGNED16 event_timestamp;  // If event timer is used, this is the 
                               // timestamp for the next transmission
#endif
#if USE_INHIBIT_TIME 
  UNSIGNED16 inhibit_time;     // Inhibit timer in ms (0 if COS not used)
  UNSIGNED16 inhibit_timestamp;// If inhibit timer is used, this is the 
                               // timestamp for the next transmission
  INHITIM_TYPE inhibit_status; // Status of inhibit timer
#endif
#if USE_SYNC
  UNSIGNED8 SYNCcnt;           // SYNC counter for counting SYNC signals
#endif
  UNSIGNED8 TType;             // Transmission Type
} TPDO_CONFIG;

// This structure holds all the RPDO configuration data for one RPDO
typedef struct 
{
#if USE_SYNC
  UNSIGNED8 BUF[8];  // SYNC data buffer 
#endif
  COBID_TYPE CANID;  // Message Identifier 
  UNSIGNED16 offset; // Pointer to destination of data 
  UNSIGNED16 PDONr;  // PDO number (1-512)
  UNSIGNED8 len;     // Data length (0-8) 
  UNSIGNED8 TType;   // Transmission Type
#if USECB_ODDATARECEIVED
  UNSIGNED16 map;    // offset to RPDO mapping in SDOResponseTable
#endif // USECB_ODDATARECEIVED
} RPDO_CONFIG;


/**************************************************************************
GLOBAL TYPES: Object Dictionary tables
**************************************************************************/
// This structure holds all data for one process data entry in the OD
typedef struct 
{
  UNSIGNED8 idx_lo;              // Index of OD entry
  UNSIGNED8 idx_hi;
  UNSIGNED8 subidx;              // Subindex of OD entry 
  UNSIGNED8 len;                 // Data length in bytes (1-4), plus bits ODRD, ODWR, RMAP/WMAP
  UNSIGNED8 off_lo;              // Offset to process data in process image
  UNSIGNED8 off_hi;
} OD_PROCESS_DATA_ENTRY;

#if USE_EXTENDED_SDO
// This structure holds all data for generic entries into the OD
typedef struct 
{
  UNSIGNED8 idx_lo;              // Index of OD entry
  UNSIGNED8 idx_hi;
  UNSIGNED8 subidx;              // Subindex of OD entry 
  UNSIGNED8 access;              // Bits ODRD, ODWR
  UNSIGNED8 len_lo;              // Length of data
  UNSIGNED8 len_hi;
#if USE_GENOD_PTR
  UNSIGNED8 *pDat;
#else
  UNSIGNED8 off_lo;              // Offset to process data in process image
  UNSIGNED8 off_hi;
#endif
} OD_GENERIC_DATA_ENTRY;
#endif // USE_EXTENDED_SDO


#if (INDEX_FOR_DIAGNOSTICS != 0)
/**************************************************************************
GLOBAL TYPES: Diagnostic Data Record
**************************************************************************/
typedef struct 
{ // Subindexes provided at Index INDEX_FOR_DIAGNOSTICS
                                    //  0: 22
                                    //  1: Identification (hard coded)
                                    //  2: Version (hard coded)
                                    //  3: Functionailty (hard coded)
  UNSIGNED32 Status;                //  4
#if (TXFIFOSIZE > 0)
  UNSIGNED32 TxFIFOStatus;          //  5: from lo to hi byte: FIFO size, max use, ovrflow count, 81 on ovrflow
#endif
#if (RXFIFOSIZE > 0)
  UNSIGNED32 RxFIFOStatus;          //  6: from lo to hi byte: FIFO size, max use, ovrflow count, 81 on ovrflow
#endif
#if (MGRFIFOSIZE > 0)
  UNSIGNED32 RxMgrFIFOStatus;       //  7: from lo to hi byte: FIFO size, max use, ovrflow count, 81 on ovrflow
#endif
  UNSIGNED16 ProcTickPerSecCur;     //  8
  UNSIGNED16 ProcTickPerSecMin;     //  9
  UNSIGNED16 ProcTickPerSecMax;     // 10
  UNSIGNED16 ProcTickBurstMax;      // 11
  UNSIGNED16 ProcRxPerSecCur;       // 12
  UNSIGNED16 ProcRxPerSecMin;       // 13
  UNSIGNED16 ProcRxPerSecMax;       // 14
                                    // 15: Reserved
#if MGR_MONITOR_ALL_NODES
  UNSIGNED16 ProcMgrTickPerSecCur;  // 16
  UNSIGNED16 ProcMgrTickPerSecMin;  // 17
  UNSIGNED16 ProcMgrTickPerSecMax;  // 18
  UNSIGNED16 ProcMgrTickBurstMax;   // 19
  UNSIGNED16 ProcMgrRxPerSecCur;    // 20
  UNSIGNED16 ProcMgrRxPerSecMin;    // 21
  UNSIGNED16 ProcMgrRxPerSecMax;    // 22
#endif
  // working variables
  UNSIGNED16 TickCnt;               // Counting calls to MCO_ProcessStackTick()
  UNSIGNED16 RxCnt;                 // Counting calls to MCO_ProcessStackRx()
  UNSIGNED16 BurstCnt;              // Counting retrun values of TRUE for MCO_ProcessStackTick()
  UNSIGNED16 NextSecond;            // Analyse diagnostics once per second
#if MGR_MONITOR_ALL_NODES
  UNSIGNED16 MgrTickCnt;
  UNSIGNED16 MgrRxCnt;
  UNSIGNED16 MgrBurstCnt;
  UNSIGNED16 MgrNextSecond;
#endif
} MCO_DIAGNOSTICS;
#endif

  
/**************************************************************************
GLOBAL VARIABLES
**************************************************************************/
// this structure holds all node specific configuration
extern MCO_CONFIG MEM_FAR gMCOConfig;

#if (INDEX_FOR_DIAGNOSTICS != 0)
// MCO diagnostics record
extern MCO_DIAGNOSTICS MEM_FAR gMCODiag;
#endif

// Global timer/conter variable, incremented every millisecond
extern UNSIGNED16 volatile gTimCnt;

// The process image
extern UNSIGNED8 MEM_PROC gProcImg[];

// table with SDO Responses for read requests to OD - defined in user_xxx.c
extern UNSIGNED8 MEM_CONST gSDOResponseTable[];

// table with Object Dictionary entries to process Data - defined in user_xxx.c
extern OD_PROCESS_DATA_ENTRY MEM_CONST gODProcTable[];

#if USE_EXTENDED_SDO
// table with generic OD entries
extern OD_GENERIC_DATA_ENTRY MEM_CONST gODGenericTable[];
#endif

#if NR_OF_TPDOS > 0
// this structure holds all the TPDO configuration data
extern TPDO_CONFIG MEM_FAR gTPDOConfig[NR_OF_TPDOS];
#endif

#if NR_OF_RPDOS > 0
// this structure holds all the RPDO configuration data for up to 4 RPDOs
extern RPDO_CONFIG MEM_FAR gRPDOConfig[NR_OF_RPDOS];
#endif


/**************************************************************************
GLOBAL FUNCTIONS
**************************************************************************/

/**************************************************************************
DOES:    Initializes the MicroCANopen stack
         It must be called from within MCOUSER_ResetApplication
RETURNS: TRUE, if init OK, else FALSE (also when unconfigured and in LSS)
**************************************************************************/
UNSIGNED8 MCO_Init (
  UNSIGNED16 Baudrate,  // CAN baudrate in kbit(1000,800,500,250,125,50,25 or 10)
  UNSIGNED8 Node_ID,    // CANopen node ID (1-126)
  UNSIGNED16 Heartbeat  // Heartbeat time in ms (0 for none)
  );


/**************************************************************************
DOES:    This function initializes a transmit PDO. Once initialized, the 
         MicroCANopen stack automatically handles transmitting the PDO.
         The application can directly change the data at any time.
NOTE:    For data consistency, the application should not write to the data
         while function MCO_ProcessStack executes.
RETURNS: nothing
**************************************************************************/
void MCO_InitTPDO (
  UNSIGNED16 PDO_NR,     // TPDO number (1-512)
  UNSIGNED32 CAN_ID,     // CAN identifier to be used (set to 0 to use default)
  UNSIGNED16 event_tim,  // Transmitted every event_tim ms 
                         // (set to 0 if ONLY inhibit_tim should be used)
  UNSIGNED16 inhibit_tim,// Inhibit time in ms for change-of-state transmit
                         // (set to 0 if ONLY event_tim should be used)
  UNSIGNED8 len,         // Number of data bytes in TPDO
  UNSIGNED16 offset      // Offset to data location in process image
  );


/**************************************************************************
DOES:    This function initializes a transmit PDO. Once initialized, the 
         MicroCANopen stack automatically handles transmitting the PDO.
         The application can directly change the data at any time.
NOTE:    For data consistency, the application should not write to the data
         while function MCO_ProcessStack executes.
         This is an extended version of MCO_InitTPDO() that includes the
         transmission type. MCO_InitTPDO() is still available for backward-
         compatibility.
RETURNS: nothing
**************************************************************************/
void MCO_InitTPDOFull
  (
  UNSIGNED16 PDO_NR,       // TPDO number (1-512)
  UNSIGNED32 CAN_ID,       // CAN identifier to be used (set to 0 to use default)
  UNSIGNED16 event_time,   // Transmitted every event_tim ms 
  UNSIGNED16 inhibit_time, // Inhibit time in ms for change-of-state transmit
                           // (set to 0 if ONLY event_tim should be used)
  UNSIGNED8 trans_type,    // Transmission type of the TPDO
  UNSIGNED8 len,           // Number of data bytes in TPDO
  UNSIGNED16 offset        // Offset to data location in process image
  );


/**************************************************************************
DOES:    This function changes the COBID of a TPDO
RETURNS: nothing
**************************************************************************/
void MCO_ChangeTPDOID (
  UNSIGNED16 PDO_NR,      // RPDO number (1-512)
  UNSIGNED32 CAN_ID       // CAN identifier to be used 
  );


/**************************************************************************
DOES:    This function initializes a receive PDO. Once initialized, the 
         MicroCANopen stack automatically updates the data at offset.
NOTE:    For data consistency, the application should not read the data
         while function MCO_ProcessStack executes.
RETURNS: nothing
**************************************************************************/
void MCO_InitRPDO (
  UNSIGNED16 PDO_NR, // RPDO number (1-512)
  UNSIGNED32 CAN_ID, // CAN identifier to be used (set to 0 to use default)
  UNSIGNED8 len,     // Number of data bytes in RPDO
  UNSIGNED16 offset  // Offset to data location in process image
  );


/**************************************************************************
DOES:    This function changes the COBID of a RPDO
RETURNS: nothing
**************************************************************************/
void MCO_ChangeRPDOID (
  UNSIGNED16 PDO_NR,      // RPDO number (1-512)
  UNSIGNED32 CAN_ID       // CAN identifier to be used 
  );


/**************************************************************************
DOES:    This function implements the main MicroCANopen protocol stack. 
         It must be called frequently to ensure proper operation of the
         communication stack. 
         Typically it is called from the while(1) loop in main.
RETURNS: 0 if nothing was done, 1 if a CAN message was sent or received
**************************************************************************/
UNSIGNED8 MCO_ProcessStack (
  void
  );


/**************************************************************************
         ALTERNATE PROCESS FUNCTION TO BE USED WITH RTOS INTEGRATION
DOES:    This function processes the next CAN message from the CAN receive
         queue. When using an RTOS, this can be turned into a task
         triggered by a CAN receive event.
RETURNS: FALSE, if no message was processed, 
         TRUE, if a CAN message received was processed
**************************************************************************/
UNSIGNED8 MCO_ProcessStackRx (
  void
  );


/**************************************************************************
         ALTERNATE PROCESS FUNCTION TO BE USED WITH RTOS INTEGRATION
DOES:    This function executes all sub functions required to keep the 
         CANopen stack operating. It should be called frequently. When used
         in an RTOS it should be called repeatedly every RTOS time tick
         until it returns zero.
RETURNS: FALSE, if there was nothing to process 
         TRUE, if stack functions were executed
**************************************************************************/
UNSIGNED8 MCO_ProcessStackTick (
  void
  );


/**************************************************************************
USER CALL-BACK FUNCTIONS
These must be implemented by the application.
**************************************************************************/

/**************************************************************************
DOES:    Call-back function for reset application.
         Starts the watchdog and waits until watchdog causes a reset.
RETURNS: nothing
**************************************************************************/
void MCOUSER_ResetApplication (
  void
  );


/**************************************************************************
DOES:    This function both resets and initializes both the CAN interface
         and the CANopen protocol stack. It is called from within the
         CANopen protocol stack, if a NMT master message was received that
         demanded "Reset Communication".
         This function should call MCO_Init and MCO_InitTPDO/MCO_InitRPDO.
RETURNS: nothing
**************************************************************************/
void MCOUSER_ResetCommunication (
  void
  );


/**************************************************************************
DOES:    This function is called if a fatal error occurred. 
         Error codes of mcohwxxx.c are in the range of 0x8000 to 0x87FF.
         Error codes of mco.c are in the range of 0x8800 to 0x8FFF. 
         All other error codes may be used by the application.
RETURNS: nothing
**************************************************************************/
void MCOUSER_FatalError (
  UNSIGNED16 ErrCode // To debug, search source code for the ErrCode encountered
  );


/**************************************************************************
OPTIONAL USER CALL-BACK FUNCTIONS
These must be implemented by the application, if enabled by nodecfg.h
Default implementation in user_callback.c
**************************************************************************/

/**************************************************************************
DOES:    Call-back function, heartbeat lost, timeout occured.
         Gets called when a heartbeat timeout occured for a node.
RETURNS: Nothing
**************************************************************************/
void MCOUSER_HeartbeatLost (
  UNSIGNED8 node_id
  );


#if USECB_TIMEOFDAY
/**************************************************************************
DOES:    This function is called if the message with the time object has
         been received.
RETURNS: nothing
**************************************************************************/
void MCOUSER_TimeOfDay (
  UNSIGNED32 millis, // Milliseconds since midnight
  UNSIGNED16 days  // Number of days since January 1st, 1984
  );
#endif // USECB_TIMEOFDAY


#if USECB_RPDORECEIVE
/**************************************************************************
DOES:    This function is called after an RPDO has been received and stored
         into the Process Image.
RETURNS: nothing
**************************************************************************/
void MCOUSER_RPDOReceived (
  UNSIGNED16 RPDONr, // RPDO Number
  UNSIGNED16 offset, // Offset to RPDO data in Process Image
  UNSIGNED8  len     // Length of RPDO
  );
#endif // USECB_RPDORECEIVE


#if USECB_ODDATARECEIVED
/**************************************************************************
DOES:    This function is called after data was received and stored
         (works for both SDO and PDO).
RETURNS: nothing
**************************************************************************/
void MCOUSER_ODData (
  UNSIGNED16 idx,
  UNSIGNED8 subidx,
  UNSIGNED8 MEM_PROC *pDat,
  UNSIGNED8 len
  );
#endif // USECB_ODDATARECEIVED


#if USECB_TPDORDY
/**************************************************************************
DOES:    This function is called before a TPDO is sent. For triggering
         modes that are outside of the application's doing (Event Timer,
         SYNC), it is called before the send data is retrieved from the
         Process Image. This allows the application to update the TPDO
         data if necessary.
NOTE:    This function is also called before a change-of-state or
         application-triggered TPDO is sent, but updating the Process Image
         will not have any effect on the TPDO data in this case.
RETURNS: TRUE to allow the PDO to be sent, FALSE to stop PDO transmission
**************************************************************************/
UNSIGNED8 MCOUSER_TPDOReady (
  UNSIGNED16 TPDONr,      // TPDO Number
  UNSIGNED8  TPDOTrigger  // Trigger for this TPDO's transmission:
                          // 0: Event Timer
                          // 1: SYNC
                          // 2: SYNC+COS
                          // 3: COS or application trigger
  );
#endif // USECB_TPDORDY


#if USECB_SYNCRECEIVE
/**************************************************************************
DOES:    This function is called with every SYNC message received.
         It allows the application to now apply all sync-triggered TPDO
         data to be applied to the application
RETURNS: nothing
**************************************************************************/
void MCOUSER_SYNCReceived (
  void
  );
#endif // USECB_SYNCRECEIVE


#if USECB_SDO_RD_PI
/**************************************************************************
DOES:    This function is called before an SDO read request is executed
         reading from the process image. The application can use this 
         function to either update the data or to deny access 
         (by returning an SDO Abort code).
RETURNS: 0, if access is granted, data can be copied and returned or
         CANopen SDO Abort Code - in which case the SDO transfer is aborted
**************************************************************************/
UNSIGNED32 MCOUSER_SDORdPI (
  UNSIGNED16 index,       // Index of Object Dictionary entry
  UNSIGNED8 subindex,     // Subindex of Object Dictionary entry
  UNSIGNED16 offset,      // Offset to data in process image
  UNSIGNED8 len           // Length of data 
  );
#endif // USECB_SDO_RD_PI


#if USECB_SDO_RD_AFTER
/**************************************************************************
DOES:    This function is called after an SDO read request was executed.
         The application can use this to clear the data or mark it as read.
RETURNS: Nothing
**************************************************************************/

void MCOUSER_SDORdAft (
  UNSIGNED16 index,       // Index of Object Dictionary entry
  UNSIGNED8 subindex,     // Subindex of Object Dictionary entry
  UNSIGNED16 offset,      // Offset to data in process image
  UNSIGNED8 len           // Length of data 
  );
#endif // USECB_SDO_RD_AFTER


#if USECB_SDO_WR_PI
/**************************************************************************
DOES:    This function is called before an SDO write request is executed
         writing to the process image. The application can use this 
         function to check the data (e.g. range check) BEFORE it gets
         written to the process image.
RETURNS: 0, if access is granted, data can be copied to process image or
         CANopen SDO Abort Code - in which case the SDO transfer is aborted
**************************************************************************/
UNSIGNED32 MCOUSER_SDOWrPI (
  UNSIGNED16 index,       // Index of Object Dictionary entry
  UNSIGNED8 subindex,     // Subindex of Object Dictionary entry
  UNSIGNED16 offset,      // Offset to data in process image
  UNSIGNED8 *pDat,        // Pointer to data received
  UNSIGNED8 len           // Length of data 
  );
#endif // USECB_SDO_WR_PI


#if USECB_SDO_WR_AFTER
/**************************************************************************
DOES:    This function is called after an SDO write request was executed.
         Data is now in the process image and can be processed.
RETURNS: Nothing
**************************************************************************/

void MCOUSER_SDOWrAft (
  UNSIGNED16 index,       // Index of Object Dictionary entry
  UNSIGNED8 subindex,     // Subindex of Object Dictionary entry
  UNSIGNED16 offset,      // Offset to data in process image
  UNSIGNED8 len           // Length of data 
  );
#endif // USECB_SDO_WR_AFTER


#if USECB_APPSDO_READ
/*******************************************************************************
DOES:    Call Back function to allow implementation of custom, application
         specific OD Read entries
RETURNS: 0x00 - OD entry not handled by this function
         0x01 - OD entry handled by this function
         0x05 - Abort with SDO_ABORT_WRITEONLY
*******************************************************************************/
UNSIGNED8 MCOUSER_AppSDOReadInit (
  UNSIGNED8 sdoserver,
  UNSIGNED16 idx, // Index of OD entry
  UNSIGNED8 subidx, // Subindex of OD entry
  UNSIGNED32 MEM_FAR *totalsize, // RETURN: total size of data, only set if >*size
  UNSIGNED32 MEM_FAR *size, // RETURN: size of data buffer
  UNSIGNED8 * MEM_FAR *pDat // RETURN: pointer to data buffer
  );


/*******************************************************************************
DOES:    Call Back function to allow implementation of custom, application
         specific OD Read entries, called at end of transfer with the option
         to add more data.
RETURNS: Nothing
*******************************************************************************/
void MCOUSER_AppSDOReadComplete (
  UNSIGNED8 sdoserver, // The SDO server number on which the request came in
  UNSIGNED16 idx, // Index of OD entry
  UNSIGNED8 subidx, // Subindex of OD entry
  UNSIGNED32 MEM_FAR *size // RETURN: size of next block of data, 0 for no further data
  );
#endif // USECB_APPSDO_READ


#if USECB_APPSDO_WRITE
/*******************************************************************************
DOES:    Call Back function to allow implementation of custom, application
         specific OD Write entries
RETURNS: 0x00 - OD entry not handled by this function
         0x01 - OD entry handled by this function
         0x04 - Abort with SDO_ABORT_READONLY
*******************************************************************************/
UNSIGNED8 MCOUSER_AppSDOWriteInit (
  UNSIGNED8 sdoserver,
  UNSIGNED16 idx, // Index of OD entry
  UNSIGNED8 subidx, // Subindex of OD entry
  UNSIGNED32 MEM_FAR *totalsize, // RETURN: total maximum size of data, only set if >*size
  UNSIGNED32 MEM_FAR *size, // Data size, if known. RETURN: max size of data buffer
  UNSIGNED8 * MEM_FAR *pDat // RETURN: pointer to data buffer
  );

/*******************************************************************************
DOES:    Call Back function to allow implementation of custom, application
         specific OD Write entries, call at end of transfer of a block. For
         multiple blocks per transfer, the same buffer is used for all blocks.
RETURNS: Nothing
*******************************************************************************/
void MCOUSER_AppSDOWriteComplete (
  UNSIGNED8 sdoserver, // The SDO server number on which the request came in
  UNSIGNED16 idx, // Index of OD entry
  UNSIGNED8 subidx, // Subindex of OD entry
  UNSIGNED32 size, // Number of bytes written (of last block)
  UNSIGNED32 more // number of bytes still to come (of total transfer)
  );
#endif // USECB_APPSDO_WRITE


#if USE_XSDOCB_WRITE
/**************************************************************************
DOES:    This function is called before a segmented SDO write access is 
         made. The application can use this to implement custom, segmented
		     SDO write transfers.
RETURNS: TRUE, if this access is supported by the application
**************************************************************************/
UNSIGNED8 MCOUSER_XSDOInitWrite (
  UNSIGNED16 index, 
  UNSIGNED8 subindex,
  UNSIGNED32 size	  // size in bytes
  );


/*******************************************************************************
DOES:    After an access was approved by the application by the 
         MCOUSER_XSDOInitWrite function, this function is called with each
         new segment received.
RETURNS: TRUE if no error occured and the data was processed
         FALSE if a major error occured and the transfer needs to be aborted
*******************************************************************************/
UNSIGNED8 MCOUSER_XSDOWriteSegment (
  UNSIGNED8 last, // Is set to 1 if this is the last segment
  UNSIGNED8 len, // length of segment (0-7)
  UNSIGNED8 *pDat // pointer to 'len' data bytes
  );
#endif // USE_XSDOCB_WRITE


#if USE_DYNAMIC_PDO_MAPPING
/**************************************************************************
DOES:     RESETS SINGLE PDO MAPPING ENTRY TO DEFAULT
RETURNS:  TRUE, if PDO found and reset, 
          FALSE, if PDO not implemented
GLOBALS:  Sets gRPDOmap and gTPDOmap to values defined in gSDOResponseTable
***************************************************************************/
UNSIGNED8 XPDO_ResetPDOMapEntry (
  UNSIGNED8 TxRx, // Set to 0 for TPDO, 1 for RPDO
  UNSIGNED16 PDONr // Number of PDO, 1 to 512
  );
#endif


/*******************************************************************************
DOES:    Internal functions of MCOP that get used in multiple modules
         Seartch tables with OD definitions for a specific index/subindex
RETURNS: 0xFFFF if not found, else offset in tabel
*******************************************************************************/
UNSIGNED16 MCO_SearchOD (
  UNSIGNED16 index,  // Index of OD entry searched
  UNSIGNED8 subindex // Subindex of OD entry searched
  );
  
UNSIGNED16 MCO_SearchODProcTable (
  UNSIGNED16 index,   // Index of OD entry searched
  UNSIGNED8 subindex  // Subindex of OD entry searched
  );
  
UNSIGNED8 XSDO_SearchODGenTable (  
  UNSIGNED16 index,     // Index of OD entry searched
  UNSIGNED8  subindex,  // Subindex of OD entry searched 
  UNSIGNED8  *access,
  UNSIGNED32 *len,
  UNSIGNED8  **pDat
  );


#if USE_CiA447
/**************************************************************************
DOES:    Advanced call-back function for expedited SDO write accesses made.
         Version that includes node ID number of node making the request
RETURNS: nothing
**************************************************************************/
void MCOUSER_NodeSpecificSDOWrite (
  UNSIGNED8 node, // node id of node that made the request (if known, else zero)
  UNSIGNED16 idx, 
  UNSIGNED8 sub, 
  UNSIGNED16 offset, 
  UNSIGNED8 len
  );


/**************************************************************************
DOES:    Working on CiA447 background tasks and timeouts
RETURNS: nothing
**************************************************************************/
void CiA447_Process (void);

#endif


/**************************************************************************
Plausability check for settings
**************************************************************************/

#if ! USE_EVENT_TIME
  #if ! USE_INHIBIT_TIME
#error At least one, USE_EVENT_TIME or USE_INHIBIT_TIME must be defined!
  #endif
#endif

#if (NR_OF_RPDOS == 0)
  #if (NR_OF_TPDOS == 0)
//#error At least one PDO must be defined!
  #endif
#endif

#if USE_STORE_PARAMETERS
 #if ! USE_EVENT_TIME
  #error When using Save Parameters, USE_EVENT_TIME must be used, too
 #endif
 #if ! USE_INHIBIT_TIME
  #error When using Save Parameters, USE_INHIBIT_TIME must be used, too
 #endif
#endif

#if ((NR_OF_TPDOS > 512) || (NR_OF_RPDOS > 512))
#error Illegal number of PDOs
#endif

#if ERROR_FIELD_SIZE > 253
#error Illegal size of error field
#endif

#if USE_XOD_ACCESS == 1
 #if USE_DYNAMIC_PDO_MAPPING == 1
 #error USE_XOD_ACCESS can not be combined with USE_DYNAMIC_PDO_MAPPING
 #endif
 #if USE_GENOD_PTR == 1
 #error USE_XOD_ACCESS can not be combined with USE_GENOD_PTR
 #endif
 #if CAN_ID_SIZE != 32
 #error USE_XOD_ACCESS requires CAN_ID_SIZE to be 32
 #endif
#endif

#ifdef __cplusplus
}
#endif

#endif // _MCO_H
/**************************************************************************
END OF FILE
**************************************************************************/
