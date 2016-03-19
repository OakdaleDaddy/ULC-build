/**************************************************************************
MODULE:    NODECFG.H
CONTAINS:  MicroCANopen node configuration
HERE:      Slave Device, Full Functionality Configuration
COPYRIGHT: Embedded Systems Academy, Inc. 2002-2015.
           All rights reserved. www.microcanopen.com
           This software was written in accordance to the guidelines at
           www.esacademy.com/software/softwarestyleguide.pdf
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

#ifndef _NODECFG_H
#define _NODECFG_H


/* CANopen Data Types */
#include "mco_types.h"

// Hardware configuration
#include "mcohw_cfg.h"


/**************************************************************************
DEFINES: DEFAULT CONFIGURATION
NOTE:    ESAcademy only tests this code with ENFORCE_DEFAULT_CONFIGURATION
         set to one. Disabling allows setting several optimization features
**************************************************************************/
#define ENFORCE_DEFAULT_CONFIGURATION 1


/**************************************************************************
DEFINES: DEFAULT NODE ID
**************************************************************************/

#ifdef __SIMULATION__
 #define NODEID simconfiguration.nodeid
#else
// Define the default Node ID that is used to initialize the CANopen
// stack if no user-specific setting is implemented in
// MCOUSER_ResetCommunication(). If the Node ID from auto-generated code
// via CANopen Architect EDS is to be used, set it to NODEID_DCF, if node
// ID can change (e.g. by LSS) set to MY_NODE_ID
 #define NODEID NODEID_DCF
#endif


/**************************************************************************
DEFINES: ENABLING/DISABLING CODE FUNCTIONALITY
**************************************************************************/

// If enabled, node starts up automatically (does not wait for NMT master)
#define AUTOSTART 0

// If enabled, parameters passed to functions are checked for consistency. 
// On failures, the user function MCOUSER_FatalError is called.
#define CHECK_PARAMETERS 1

// If enabled, CANopen indicator lights are implemented
#define USE_LEDS 1


/**************************************************************************
DEFINES: ENABLING SYSTEM NEAR CALL-BACK FUNCTIONS, 
         DEFAULT IMPLEMENTATION IN user_[hwplatform].c
**************************************************************************/

// If enabled, the call-back function MCOUSER_NMTChange() is called
// everytime the CANopen stack changes its NMT Slave State
#define USECB_NMTCHANGE 1

// If enabled, the call-back function MCOUSER_GetSerial() is called every
// time the CANopen stack receives an SDO Request for the serial number
#define USECB_ODSERIAL 1

// If enabled, call-back MCOUSER_TimeOfDay() is called when time object 
// is received
#define USECB_TIMEOFDAY 1


/**************************************************************************
DEFINES: ENABLING CANopen DATA CALL-BACK FUNCTIONS
         DEFAULT IMPLEMENTATION IN user_cbdata.c
**************************************************************************/

// If enabled, the call-back function MCOUSER_ODData() is called every 
// time data is received 
// NOTE: PI Access, RPDO or SDO write
#define USECB_ODDATARECEIVED 0

// If enabled, the call-back function MCOUSER_RPDOReceived() is called 
// every time the CANopen stack receives an RPDO
// NOTE: PI Access, RPDO 
#define USECB_RPDORECEIVE 0

// If enabled, the call-back function MCOUSER_TPDOReady() is called every
// time right before the CANopen stack sends a TPDO
// NOTE: PI Access, TPDO 
#define USECB_TPDORDY 0

// If enabled, the call-back function MCOUSER_SYNCReceived() is called 
// every time the CANopen stack receives the SYNC message
// NOTE: No data, RPDO data needs to be applied, TPDO uses USECB_TPDORDY
#define USECB_SYNCRECEIVE 0

// If enabled, the call back functions MCOUSER_SDORdPI(), 
// MCOUSER_SDOWrPI(), MCOUSER_SDORdAft() and MCOUSER_SDOWrAft() are called 
// before/after every expedited SDO access to the process image
// NOTE: SDO access to PI
#define USECB_SDO_RD_PI    1
#define USECB_SDO_RD_AFTER 0
#define USECB_SDO_WR_PI    1
#define USECB_SDO_WR_AFTER 1

// If enabled, the call back functions MCOUSER_AppSDOReadInit() with
// MCOUSER_AppSDOReadComplete or MCOUSER_AppSDOWriteInit() with
// MCOUSER_AppSDOWriteComplete() are called before/after every segmented 
// SDO request. Allows implementation of application specific, custom 
// segmented SDOs (e.g dynamic length entries)
// NOTE: segmented SDO access, also outside PI
#define USECB_APPSDO_READ  1
#define USECB_APPSDO_WRITE 0

// Support of dynamic PDO mapping.
#define USE_DYNAMIC_PDO_MAPPING 1

/**************************************************************************
DEFINES: ADDITIONAL ENABLING/DISABLING CODE FUNCTIONALITY OF PLUS PACKAGE
**************************************************************************/

// If enabled, Emergency Messages are used
#define USE_EMCY 1

// If EMCYs are used, size of error field history [1003h]
#define ERROR_FIELD_SIZE 4

// If enabled, extended SDO handling (segmented transfer) is used
#define USE_EXTENDED_SDO 1

// If enabled, blocked SDO transfers are allowed
#define USE_BLOCKED_SDO 0
// Max size of a block supported (4 to 127)
#define BLK_MAX_SIZE 8
#define BLK_B2B_TIMEOUT 4

// If enabled, the function Store Parameters [1010h] is supported
// The driver must provide funtcions NVOL_ReadByte and NVOL_WriteByte
#define USE_STORE_PARAMETERS 1
// Nr of Subindexes used at [1010h]
#define NROF_STORE_PARAMETERS 4
// Specify offset and size of non-volatile memory,
#define NVOL_STORE_START 16
#define NVOL_STORE_SIZE (0x200 - NVOL_STORE_START)

// If enabled, Layer Setting Services (Slave) are implemented
#ifndef USE_LSS_SLAVE
#define USE_LSS_SLAVE 0
#endif

// If enabled, LSS Fast Scan is implemented
#ifndef USE_MICROLSS
#define USE_MICROLSS 0
#endif

// If enabled, supports the sleep mode 
#define USE_SLEEP 0

// Number of SDO servers supported
#define NR_OF_SDOSERVER 1

// Support of generic data pointer for OD contents.
// Set to zero, if all data is in process image.
#define USE_GENOD_PTR 0

#endif // _NODECFG_H

/*----------------------- END OF FILE ----------------------------------*/
