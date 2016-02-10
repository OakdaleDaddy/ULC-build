/*******************************************************************************
 *
 * Project  :	PCAN-GPS - Timer Example
 * Module   :
 * Filename :	main.c
 * System   :
 * Compiler :
 * Switches :
 * Rights   : 	(c) PEAK-System Technik GmbH
 *            	www.peak-system.com
 *
 *******************************************************************************
 * Implementation description
 *
 *
 *
 *******************************************************************************
 * History: (newer entries first!)
 *------------------------------------------------------------------------------
 * Date / Name      Vers.   changes made
 *------------------------------------------------------------------------------
 * 2014 October StM	1.0.2	Changed wrong mask when sending module ID
 * 2014 October StM	1.0.1	Changed format for GPS position in CAN frame
 * 2014 July StM	1.0.0	Initial Version
 ******************************************************************************/

/*******************************************************************************
 include files
 ******************************************************************************/
//
// System header files
//
#include <system_LPC407x_8x_177x_8x.h>
#include <lpc407x_8x_177x_8x.h>
#include <string.h>
#include <math.h>

//
// Library header files
//
#include "typedefs.h"
#include "hardware.h"
#include "can.h"
#include "ssp.h"
#include "eeprom.h"
#include "serial.h"
#include "crc.h"

//
// Source code header files
//
#include "timer.h"
#include "MEMS_BMC050.h"
#include "MEMS_L3GD20.h"
#include "MAX-7W.h"
#include "can_user.h"
#include "crc_data.h"
#include "rtc.h"


/*******************************************************************************
 global definitions
 ******************************************************************************/
//! identifier is needed by PCANFlash.exe -> do not delete
const b8_t Ident[] __attribute__ ((section(".ident"), used)) = { "PCAN-GPS"};

//! info data for PCANFlash.exe
const crc_array_t  C2F_Array __attribute__((section(".C2F_Info"), used)) = {
	.Str = CRC_IDENT_STRING,
	.Version = 0x21,
	.Day = 5,
	.Month = 5,
	.Year = 6,

	// crc infos are patched during link time by flash.ld
	// crc value is patched by PCANFlash.exe
};

#define EEPROM_CFG_ADDR 0x00
S_CONFIG_DATA_t cfg_data;

#define EEPROM_BUS_ADDR (EEPROM_CFG_ADDR + sizeof(u32_t))

/*******************************************************************************
 local definitions
 ******************************************************************************/
u8_t Initialized = 0;
u8_t persistant_config_used=0;



/*******************************************************************************
 local function prototypes
 ******************************************************************************/
//static s32_t init_system(void);
static s32_t get_default_config(void);
static s32_t read_persistent_config(void);
void Timer_1000usec(void);

static void processTickIsr(void);
static void executeDevice(void);

/*******************************************************************************
 global functions
 ******************************************************************************/


//------------------------------------------------------------------------------
//! void Timer_1000usec(void)
//------------------------------------------------------------------------------
//! @brief	this function is called every 1[ms] by timer0 match-interrupt ISR
//------------------------------------------------------------------------------
void Timer_1000usec(void){
  //   static u32_t  LedBlink_counter=0;
  //   static u8_t toggle_led = 0;

	if (!Initialized)
		return;

   processTickIsr();

#if 0
	if (LedBlink_counter < 1000){
		LedBlink_counter++;
	}else{
		LedBlink_counter = 0;

		toggle_led ^= 1;		// invert flag and set new value
		if (toggle_led){
			HW_SetLED (HW_LED_STATUS_1, HW_LED_GREEN);
		} else {
			HW_SetLED(HW_LED_STATUS_1, HW_LED_OFF);
		}
	}
#endif
}

//------------------------------------------------------------------------------
//! int main(void)
//------------------------------------------------------------------------------
//! @brief	initialization and main loop
//------------------------------------------------------------------------------
int main(void)
{
   executeDevice();

#if 0
   Initialized = 0;
   init_system();

   {
      u8_t nodeId;

      HW_GetModuleID(&nodeId);
      nodeId = 120 + (nodeId&0x7);

      CAN_SetNodeId(nodeId);
      CAN_SendHeartbeat(0);
   }

	while(1) 	
	{
		static u32_t  		BMC050task_counter=0;
		static u32_t  		L3GD20task_counter=0;
		static u32_t  		MAX7Wtask_counter=0;
		static u32_t  		DIO_counter=0;
		static u32_t  		TIME_counter=0;

#if 0
		// read new compass/accelerometer/temperature data
		if (SYSTIME_DIFF(BMC050task_counter, SYSTIME_NOW) >= 25000)
		{
			// update time interval
			BMC050task_counter = SYSTIME_NOW;
			// as long as there is not a complete set of readings
			if (BMC050_Readings.data_valid == FALSE)
			{
				MEMS_BMC050_task();
			
				if (BMC050_Readings.data_valid == TRUE){
					CAN_UserSendBMCData();
				}
			}
		}
		//-------------------------
#endif
		
#if 0
		// read new gyro/temperature data
		if (SYSTIME_DIFF (L3GD20task_counter, SYSTIME_NOW) >= 50000)
		{
			// update time interval
			L3GD20task_counter = SYSTIME_NOW;
			// as long as there is not a complete set of readings
			MEMS_L3GD20_task();
			
			if (L3GD20_Readings.data_valid == TRUE)
				CAN_UserSendL3GDData();
		}
		//-------------------------
#endif

#if 0
		// evaluate new gps data as fast as possible
		UBLOX_MAX7W_task();
		if (SYSTIME_DIFF (MAX7Wtask_counter, SYSTIME_NOW) >= 100000)
		{
			// update time interval
			MAX7Wtask_counter = SYSTIME_NOW;
			CAN_UserSendGPSData();
		}
		//-------------------------
#endif

#if 0
		if (SYSTIME_DIFF (DIO_counter, SYSTIME_NOW) >= 25000)
		{
			// update time interval
			DIO_counter = SYSTIME_NOW;
			CAN_UserSendDioData();
		}
		//-------------------------
#endif

#if 0
		if (SYSTIME_DIFF (TIME_counter, SYSTIME_NOW) >= 500000)
		{
			// update time interval
			TIME_counter = SYSTIME_NOW;
			CAN_UserSendRTCTime();
		}
		//-------------------------
#endif

		CAN_UserProcessMsg();
	} // end while (1)
#endif
}



   #if 0
//------------------------------------------------------------------------------
//! s32_t init_system(void)
//------------------------------------------------------------------------------
//! @brief initializes system
//------------------------------------------------------------------------------
s32_t init_system(void){
	s32_t res = 0;

	// Initialize Basic Parts
	HW_Init();

	// Initialize Systemtimer
	Init_Timer0();

	rtc_initialize();

	// read configuration values from EEPROM if present
	res = read_persistent_config();
	if(res!=RET_OK)
		get_default_config();

	// Initialize CAN Controller
   res = CAN_UserInit(6);

	// Init GPS and UART2
	UBLOX_MAX7W_init();

	// Initialize gyroscope
	MEMS_L3GD20_init();

	// Initialize magnetometer and accelerometer
	MEMS_BMC050_init_Accelerometer();
	MEMS_BMC050_init_Magnetometer();

	// set values read from EEPROM *after* initialization of sensors with
	// default values
	MEMS_BMC050_SetAccCalTargets(	&cfg_data.Acc.cmp_target_x,
									&cfg_data.Acc.cmp_target_y,
									&cfg_data.Acc.cmp_target_z);
	MEMS_BMC050_SetAccRange(cfg_data.Acc.range);

	MEMS_L3GD20_SetRange(cfg_data.Gyro.range);

#if 0
	// Transmit initial configuration values once
	CAN_UserResetPanelValues();
#endif

	Initialized = 1;
   return (res);
}

#endif

//------------------------------------------------------------------------------
//! static s32_t get_default_config(void)
//------------------------------------------------------------------------------
//! @brief	sets system configuration to default values
//------------------------------------------------------------------------------
static s32_t get_default_config(void){

	cfg_data.Acc.cmp_target_x = 0;
	cfg_data.Acc.cmp_target_y = 0;
	cfg_data.Acc.cmp_target_z = 0;	
	cfg_data.Acc.cmp_filt_x = 0;
	cfg_data.Acc.cmp_filt_y = 0;
	cfg_data.Acc.cmp_filt_z = 0;
	cfg_data.Acc.cmp_raw_x = 0;
	cfg_data.Acc.cmp_raw_y = 0;
	cfg_data.Acc.cmp_raw_z = 0;
	cfg_data.Acc.flags = 0;
	
	cfg_data.Acc.range = 1;

	cfg_data.Gyro.range = 0;
	
	return RET_OK;
}



//------------------------------------------------------------------------------
//! static s32_t read_persistent_config(void)
//------------------------------------------------------------------------------
//! @brief reads configuration from EEPROM
//------------------------------------------------------------------------------
//! @return	RET_ERR if EEPROM read fails or if CRC checksum was not valid
//!			RET_OK	elsewise
//------------------------------------------------------------------------------
static s32_t read_persistent_config(void){
	CRCInit_t cfg= CRC32_CONFIG;
	u32_t res=0;
	u32_t cnt=0;
	
	//! read static configuration data from eeprom
	if(EEPROM_Read(EEPROM_INT, EEPROM_CFG_ADDR,
			(u8_t*)&cfg_data, sizeof(cfg_data))!=EEPROM_ERR_OK)
		return RET_ERR;

	cnt = sizeof(cfg_data)-sizeofmember(S_CONFIG_DATA_t, crc32);
	res = CRC_Valid(&cfg_data, cnt, t_crc_8_bit, &cfg, cfg_data.crc32);
	if(res!=1)
		return RET_ERR;

	persistant_config_used = 1;

	return RET_OK;
}

/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */

// todo: different state for LED

typedef enum
{
   LED_PREOPERATIONAL_OK_S,
   LED_PREOPERATIONAL_ERROR_S,
   LED_OPERATIONAL_S,
   LED_STOPPED_S,
}LED_STATE;

typedef enum
{
   DEVICE_PREOPERATIONAL_S,
   DEVICE_OPERATIONAL_S,
   DEVICE_STOPPED_S,
}DEVICE_STATE;

typedef struct
{
   u8_t mapCount; // number of mappings
   u32_t mappings[8]; // maps

   u8_t txType; // transmit type: 0..F0=n syncs, FE=event, FF=event
   u32_t cobId; // cob of PDO
   u16_t inhibitTime; // time used for inhibit, 100uS units
   u16_t eventTime; // time used for event, mS units

   u8_t syncCount; // number of received syncs
   u32_t inhibitTimeCount; // used for inhibit time tracking
   u32_t processTimeCount; // used for time tracking
   u8_t processNeeded; // used for event activity and SYNC trigger

}DEVICE_TPDO_MAP;


static u8_t pdoMapByteCount(u32_t mapping);
static u8_t pdoByteSize(u16_t index, u8_t subIndex);

static u8_t pdoTxMappable(u16_t index, u8_t subIndex);
static void loadTxPdoMapParameter(DEVICE_TPDO_MAP * tpdoMap, u8_t subIndex, u8_t * buffer, u32_t * dataCount);
static void loadTxPdoMapData(DEVICE_TPDO_MAP * tpdoMap, u8_t subIndex, u8_t * buffer, u32_t * dataCount);
static u8_t storeTxPdoMapParameter(DEVICE_TPDO_MAP * tpdoMap, u8_t subIndex, u8_t byteCount, u32_t value);
static u8_t storeTxPdoMapData(DEVICE_TPDO_MAP * tpdoMap, u8_t subIndex, u8_t byteCount, u32_t value);
static void resetTxPdoMap(DEVICE_TPDO_MAP * tpdoMap, u8_t index);

//static u8_t pdoRxMappable(u16_t index, u8_t subIndex);

static u8_t evaluateDeviceDataSize(u16_t index, u8_t subIndex, u32_t length);
static u8_t loadDeviceData(u16_t index, u8_t subIndex, u8_t * buffer, u32_t * length);
static u8_t storeDeviceData(u16_t index, u8_t subIndex, u8_t * source, u32_t offset, u32_t length);

static u32_t sendDebug(u32_t codeA, u32_t codeB);
static u32_t sendHeartbeat(u8_t value);
static u32_t sendPdoData(DEVICE_TPDO_MAP * txPdoMap);
static u32_t sendSdoData(void);
static u32_t sendSdoAbort(u16_t index, u8_t subIndex, u32_t code);

static void updateHeartbeat(void);
static void updateTxPdoMap(DEVICE_TPDO_MAP * tpdoMap);

static void initiateSdoDownload(u8_t * frame);
static void processSdoDownload(u8_t * frame);
static void initiateSdoUpload(u8_t * frame);
static void processSdoUpload(u8_t * frame);
static void abortSdoTransfer(u8_t * frame);
static void processNmtMessage(u8_t * frame);
static void processSyncMessage(void);
static void processSdoMessage(u8_t * frame);

static void setLedState(LED_STATE state);
static void setPreOperationalState(void);
static void setOperationalState(void);
static void setStoppedState(void);

static void executePreOperationalState(void);
static void executeOperationalState(void);
static void executeStoppedState(void);


static DEVICE_STATE deviceState;

static u8_t deviceNodeId;

static LED_STATE ledState;
static u16_t ledFlashCount;
static u16_t ledFlashLimit;
static u16_t ledFlashStep;

static CANMsg_t message;
static u8_t receiveResult;
static u8_t messageType;
static u8_t messageDeviceId;

static u8_t transferActive;
static u8_t transferUpload;
static u8_t transferStarted;
static u16_t transferIndex;
static u8_t transferSubIndex;
static u32_t transferLength;
static u32_t transferOffset;
static u8_t transferToggle;
static u8_t transferLastLength;
static u8_t transferBuffer[64];

static DEVICE_TPDO_MAP tpdoMapping[4];

static u32_t deviceType = 0x12345678;
static u32_t errorStatus = 0;
static char * deviceName = "PCAN-GPS by ULC Robotics";
static char * version = "v1.00 " __DATE__ " " __TIME__;  
static u8_t objectNodeId;
static u8_t objectBitRateCode;
static u16_t producerHeartbeatTime;

static u32_t heartbeatTimeCounter;

/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
static void processTickIsr(void)
{
   ledFlashCount++;

   if ( (ledFlashCount > ledFlashLimit) )
   {
      ledFlashStep++;

      if ( (LED_PREOPERATIONAL_OK_S == ledState) )
      {
         if ( (1 == ledFlashStep) )
         {
            ledFlashLimit = 500;               
            HW_SetLED(HW_LED_STATUS_1, HW_LED_OFF);
         }
         else 
         {
            ledFlashStep = 0;
            ledFlashLimit = 250;               
            ledFlashCount = 0;
            HW_SetLED(HW_LED_STATUS_1, HW_LED_GREEN);
         }
      }
      else if ( (LED_PREOPERATIONAL_ERROR_S == ledState) )
      {
         if ( (1 == ledFlashStep) )
         {
            ledFlashLimit = 1000;               
            HW_SetLED(HW_LED_STATUS_1, HW_LED_OFF);
         }
         else 
         {
            ledFlashStep = 0;
            ledFlashLimit = 250;               
            ledFlashCount = 0;
            HW_SetLED(HW_LED_STATUS_1, HW_LED_RED);
         }
      }
      else if ( (LED_STOPPED_S == ledState) )
      {
         if ( (1 == ledFlashStep) )
         {
            ledFlashLimit = 200;               
            HW_SetLED(HW_LED_STATUS_1, HW_LED_OFF);
         }
         else if ( (2 == ledFlashStep) )
         {
            ledFlashLimit = 300;               
            HW_SetLED(HW_LED_STATUS_1, HW_LED_GREEN);
         }
         else if ( (3 == ledFlashStep) )
         {
            ledFlashLimit = 1000;               
            HW_SetLED(HW_LED_STATUS_1, HW_LED_OFF);
         }
         else 
         {
            ledFlashStep = 0;
            ledFlashLimit = 100;               
            ledFlashCount = 0;
            HW_SetLED(HW_LED_STATUS_1, HW_LED_GREEN);
         }
      }
   }
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
static u8_t pdoMapByteCount(u32_t mapping)
{
   u8_t mapByteCount = (u8_t)((mapping & 0xFF) / 8);
   return (mapByteCount);
}

static u8_t pdoByteSize(u16_t index, u8_t subIndex)
{
   u8_t result = 0;

   if ( (0x2201 == index) )
   {
      if ( (subIndex >= 1) && (subIndex <= 3) )
      {
         result = 1;
      }
      else if ( (4 == subIndex) ||
                (5 == subIndex) )
      {
         result = 4;
      }
   }
   else if ( (0x2202 == index) ||
             (0x2203 == index) )
   {
      result = 7;
   }
   else if ( (0x2205== index) )
   {
      result = 6;
   }

   return(result);
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
static u8_t pdoTxMappable(u16_t index, u8_t subIndex)
{
   u8_t result = 0;

   if ( (0x2201 == index) )
   {
      if ( (subIndex >= 1) && (subIndex <= 5) )
      {
         result = 1;
      }
   }
   else if ( (0x2202 == index) ||
             (0x2203 == index) ||
             (0x2205 == index) )
   {
      result = 1;
   }

   return(result);
}

static void loadTxPdoMapParameter(DEVICE_TPDO_MAP * tpdoMap, u8_t subIndex, u8_t * buffer, u32_t * dataCount)
{
   if (0 == subIndex)
   {
      buffer[0] = 5;
      *dataCount = 1;
   }
   else if (1 == subIndex)
   {
      u32_t cobId = tpdoMap->cobId;
      buffer[0] = ((cobId >> 0) & 0xFF);
      buffer[1] = ((cobId >> 8) & 0xFF);
      buffer[2] = ((cobId >> 16) & 0xFF);
      buffer[3] = ((cobId >> 24) & 0xFF);
      *dataCount = 4;
   }
   else if (2 == subIndex)
   {
      buffer[0] = tpdoMap->txType;
      *dataCount = 1;
   }
   else if (3 == subIndex)
   {
      buffer[0] = ((tpdoMap->inhibitTime >> 0) & 0xFF);
      buffer[1] = ((tpdoMap->inhibitTime >> 8) & 0xFF);
      *dataCount = 2;
   }
   else if (5 == subIndex)
   {
      buffer[0] = ((tpdoMap->eventTime >> 0) & 0xFF);
      buffer[1] = ((tpdoMap->eventTime >> 8) & 0xFF);
      *dataCount = 2;
   }
}

static void loadTxPdoMapData(DEVICE_TPDO_MAP * tpdoMap, u8_t subIndex, u8_t * buffer, u32_t * dataCount)
{
   if (0 == subIndex)
   {
      buffer[0] = tpdoMap->mapCount;
      *dataCount = 1;
   }
   else if ((subIndex >= 1) && (subIndex <= 8))
   {
      u32_t mapping = tpdoMap->mappings[subIndex - 1];

      buffer[0] = ((mapping >> 0) & 0xFF);
      buffer[1] = ((mapping >> 8) & 0xFF);
      buffer[2] = ((mapping >> 16) & 0xFF);
      buffer[3] = ((mapping >> 24) & 0xFF);

      *dataCount = 4;
   }
}

static u8_t storeTxPdoMapParameter(DEVICE_TPDO_MAP * tpdoMap, u8_t subIndex, u8_t byteCount, u32_t value)
{
   u8_t result = 0;

   if ( (DEVICE_PREOPERATIONAL_S == deviceState) )
   {
      if ( (1 == subIndex) )
      {
         if ( (4 == byteCount) )
         {
            tpdoMap->cobId = value;
            result = 1;
         }
      }
      else if ( (2 == subIndex) )
      {
         if ( (1 == byteCount) )
         {
            tpdoMap->txType = (u8_t)value;
            result = 1;
         }
      }
      else if ( (3 == subIndex) )
      {
         if ( (2 == byteCount) )
         {
            tpdoMap->inhibitTime = (u16_t)value;
            result = 1;
         }
      }
      else if ( (5 == subIndex) )
      {
         if ( (2 == byteCount) )
         {
            tpdoMap->eventTime = (u16_t)value;
            result = 1;
         }
      }
   }

   return (result);
}

static u8_t storeTxPdoMapData(DEVICE_TPDO_MAP * tpdoMap, u8_t subIndex, u8_t byteCount, u32_t value)
{
   u8_t result = 0;

   if ( (DEVICE_PREOPERATIONAL_S == deviceState) )
   {
      if ( (0 == subIndex) )
      {
         if ( (1 == byteCount) )
         {
            tpdoMap->mapCount = (u8_t)value;
            result = 1;
         }
      }
      else if ( (subIndex >= 1) && (subIndex <= 8) )
      {
         if ( (4 == byteCount) )
         {
            u16_t mapIndex = (u16_t)((value >> 16) & 0xFFFF);
            u8_t mapSubIndex = (u8_t)((value >> 8) & 0xFF);
            u8_t mappableObject = pdoTxMappable(mapIndex, mapSubIndex);

            if ( (0 != mappableObject) &&
                 (subIndex >= 1) &&
                 (subIndex <= 8))
            {
               u8_t mapSize = pdoMapByteCount(value);
               u8_t pdoSize = pdoByteSize(mapIndex, mapSubIndex);

               if ( (pdoSize == mapSize) )
               {
                  for (int i = 1; i < subIndex; i++)
                  {
                     mapSize += pdoMapByteCount(tpdoMap->mappings[i - 1]);
                  }

                  if (mapSize <= 8)
                  {
                     tpdoMap->mappings[subIndex - 1] = value;
                     result = 1;
                  }
               }
            }
         }
      }
   }

   return (result);
}

static void resetTxPdoMap(DEVICE_TPDO_MAP * tpdoMap, u8_t index)
{
   u8_t i;
   CAN_FrameType pdoType;

   tpdoMap->mapCount = 0;

   for (i = 0; i < 8; i++)
   {
      tpdoMap->mappings[i] = 0;
   }

   if ( (0 == index) )
   {
      pdoType = CAN_TPDO1_T;
   }
   else if ( (1 == index) )
   {
      pdoType = CAN_TPDO2_T;
   }
   else if ( (2 == index) )
   {
      pdoType = CAN_TPDO3_T;
   }
   else if ( (3 == index) )
   {
      pdoType = CAN_TPDO4_T;
   }

   tpdoMap->cobId = (u32_t)(0x40000000 | ((u32_t)pdoType) << 7) | ((u32_t)deviceNodeId & 0x7F);

   tpdoMap->inhibitTime = 0;
   tpdoMap->eventTime = 0;

   tpdoMap->syncCount = 0;
   tpdoMap->inhibitTimeCount = SYSTIME_NOW;
   tpdoMap->processTimeCount = SYSTIME_NOW;
   tpdoMap->processNeeded = 0;
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
#if 0
static u8_t pdoRxMappable(u16_t index, u8_t subIndex)
{
   u8_t result = 0;

   return(result);
}
#endif
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
static u8_t evaluateDeviceDataSize(u16_t index, u8_t subIndex, u32_t length)
{
   // used for writable fields that are larger than 4 bytes
   return(0);
}

static u8_t loadDeviceData(u16_t index, u8_t subIndex, u8_t * buffer, u32_t * length)
{
   u32_t size;
   u8_t * source = 0;
   u32_t transferred = 0;

   if (0x1000 == index)
   {
      size = sizeof(deviceType);
      source = (u8_t *)&deviceType;
   }
   else if (0x1001 == index)
   {
      size = sizeof(errorStatus);
      source = (u8_t *)&deviceType;
   }
   else if (0x1008 == index)
   {
      size = strlen(deviceName);
      source = (u8_t *)deviceName;
   }
   else if (0x100A == index)
   {
      size = strlen(version);
      source = (u8_t *)version;
   }
   else if (0x1017 == index)
   {
      size = sizeof(producerHeartbeatTime);
      source = (u8_t *)&producerHeartbeatTime;
   }            
   else if ((0x1018 == index) && (0 == subIndex))
   {
      transferBuffer[0] = 1;
      transferred = 1;
   }
   else if ((0x1018 == index) && (1 == subIndex))
   {
      buffer[0] = 0;
      buffer[1] = 0;
      buffer[2] = 0;
      buffer[3] = 0;
      transferred = 4;
   }
   else if ((0x1800 <= index) && (0x1803 >= index))
   {
      u8_t mappingOffset = (index - 0x1800);
      transferred = 0;
      loadTxPdoMapParameter(&tpdoMapping[mappingOffset], subIndex, buffer, &transferred);
   }
   else if ((0x1A00 <= index) && (0x1A03 >= index))
   {
      u8_t mappingOffset = (index - 0x1800);
      transferred = 0;
      loadTxPdoMapData(&tpdoMapping[mappingOffset], subIndex, buffer, &transferred);
   }
   else if (0x2100 == index)
   {
      size = sizeof(objectBitRateCode);
      source = (u8_t *)&objectBitRateCode;
   }
   else if (0x2101 == index)
   {
      size = sizeof(objectNodeId);
      source = (u8_t *)&objectNodeId;
   }
   else if (0x2105 == index)
   {
      buffer[0] = 0x73;
      buffer[1] = 0x61;
      buffer[2] = 0x76;
      buffer[3] = 0x65;
      transferred = 4;
   }
   else if ( (0x2201 == index) && (0 == subIndex) )
   {
      buffer[0] = 5;
      transferred = 1;
   }
   else if ( (0x2201 == index) && (1 == subIndex) )
   {
      size = sizeof(MAX7W_Readings.Gps_AntennaStatus);
      source = (u8_t *)&MAX7W_Readings.Gps_AntennaStatus;
   }
   else if ( (0x2201 == index) && (2 == subIndex) )
   {
      size = sizeof(MAX7W_Readings.Nav_NumSatellites);
      source = (u8_t *)&MAX7W_Readings.Nav_NumSatellites;
   }
   else if ( (0x2201 == index) && (3 == subIndex) )
   {
      size = sizeof(MAX7W_Readings.Nav_Method);
      source = (u8_t *)&MAX7W_Readings.Nav_Method;
   }
   else if ( (0x2201 == index) && (4 == subIndex) )
   {
      size = sizeof(MAX7W_Readings.Nav_CourseOverGround);
      source = (u8_t *)&MAX7W_Readings.Nav_CourseOverGround;
   }
   else if ( (0x2201 == index) && (5 == subIndex) )
   {
      size = sizeof(MAX7W_Readings.Nav_SpeedOverGroundKmh);
      source = (u8_t *)&MAX7W_Readings.Nav_SpeedOverGroundKmh;
   }
   else if ( (0x2202 == index) )
   {
      double tempA, tempB;
      u16_t degrees;
      float minutes;
      tempA = modf((MAX7W_Readings.Pos_Longitude/100), &tempB);
      degrees = (u16_t)tempB;
      minutes = (float)tempA * 100;

      buffer[0] = ((u8_t *)&minutes)[0];
      buffer[1] = ((u8_t *)&minutes)[1];
      buffer[2] = ((u8_t *)&minutes)[2];
      buffer[3] = ((u8_t *)&minutes)[3];
      buffer[4] = ((u8_t *)&degrees)[0];
      buffer[5] = ((u8_t *)&degrees)[1];
      buffer[6] = MAX7W_Readings.Pos_LongitudeIndEW;
      {
         CANMsg_t TxMsg;
 
         TxMsg.Id = CAN_COB(CAN_EMGY_T, deviceNodeId);
         TxMsg.Len = 8;
         TxMsg.Type = CAN_MSG_STANDARD;
         TxMsg.Data.DataDbl = MAX7W_Readings.Pos_Longitude;
         CAN_UserWrite(&TxMsg);
      }

#if 0
      
      buffer[0] = 0x00;
      buffer[1] = 0x22;
      buffer[2] = 0x08;
      buffer[3] = 0x40;
      buffer[4] = 0x49;
      buffer[5] = 0x00;
      buffer[6] = 0x57;

#endif
      
      transferred = 7;
   }
   else if ( (0x2203 == index) )
   {
      double tempA, tempB;
      u16_t degrees;
      float minutes;
      tempA = modf((MAX7W_Readings.Pos_Latitude/100), &tempB);
      degrees = (u16_t)tempB;
      minutes = (float)tempA * 100;

      buffer[0] = ((u8_t *)&minutes)[0];
      buffer[1] = ((u8_t *)&minutes)[1];
      buffer[2] = ((u8_t *)&minutes)[2];
      buffer[3] = ((u8_t *)&minutes)[3];
      buffer[4] = ((u8_t *)&degrees)[0];
      buffer[5] = ((u8_t *)&degrees)[1];
      buffer[6] = MAX7W_Readings.Pos_LatitudeIndNS;

      {
         CANMsg_t TxMsg;
 
         TxMsg.Id = CAN_COB(CAN_EMGY_T, deviceNodeId);
         TxMsg.Len = 8;
         TxMsg.Type = CAN_MSG_STANDARD;
         TxMsg.Data.DataDbl = MAX7W_Readings.Pos_Latitude;
         CAN_UserWrite(&TxMsg);
      }


#if 0
      buffer[0] = 0x14;
      buffer[1] = 0xDB;
      buffer[2] = 0x4E;
      buffer[3] = 0x42;
      buffer[4] = 0x28;
      buffer[5] = 0x00;
      buffer[6] = 0x4E;
#endif      
      transferred = 7;
   }
   else if ( (0x2205 == index) )
   {
      buffer[0] = MAX7W_Readings.Time_Hrs;
      buffer[1] = MAX7W_Readings.Time_Min;
      buffer[2] = MAX7W_Readings.Time_Sec;
      buffer[3] = MAX7W_Readings.Date_Year;
      buffer[4] = MAX7W_Readings.Date_Month;
      buffer[5] = MAX7W_Readings.Date_DayOfMonth;
      transferred = 6;
   }

   if ( (size < sizeof(transferBuffer)) )
   {
      if ( (0 != source) )
      {
         u32_t i;

         for (i=0; i<size; i++)
         {
            buffer[i] = source[i];
         }

         for (; i<4; i++)
         {
            transferBuffer[i] = 0xCC;
         }

         transferred = size;
      }
   }

   if ( (0 != transferred) &&
        (0 != length) )
   {
      *length = transferred;
   }

   return( (0 != transferred) ? 1 : 0 );
}

static u8_t storeDeviceData(u16_t index, u8_t subIndex, u8_t * source, u32_t offset, u32_t length)
{
   u8_t i;
   u8_t result = 0;

   if (0x1017 == index)
   {
      if ( (2 == length) )
      {
         producerHeartbeatTime = (source[offset+1] << 8) | source[offset];
         heartbeatTimeCounter = SYSTIME_NOW;
         result = 1;
      }
   }            
   else if ((0x1800 <= index) && (0x1803 >= index))
   {
      u8_t mappingOffset = (index - 0x1800);
      u32_t parameter = 0;
      u32_t shifter = 0;

      for (i=0; i<length; i++)
      {
         parameter |= (u32_t)source[offset+i] << shifter;
         shifter += 8;
      }

      result = storeTxPdoMapParameter(&tpdoMapping[mappingOffset], subIndex, length, parameter);
   }
   else if ((0x1A00 <= index) && (0x1A03 >= index))
   {
      u8_t mappingOffset = (index - 0x1800);
      u32_t data = 0;
      u32_t shifter = 0;

      for (i=0; i<length; i++)
      {
         data |= (u32_t)source[offset+i] << shifter;
         shifter += 8;
      }

      result = storeTxPdoMapData(&tpdoMapping[mappingOffset], subIndex, length, data);
   }
   else if (0x2100 == index)
   {
      if ( (1 == length) )
      {        
         objectBitRateCode = source[offset];
         result = 1;
      }
   }            
   else if (0x2101 == index)
   {
      if ( (1 == length) )
      {
         objectNodeId = source[offset];
         result = 1;
      }
   }            
   else if (0x2105 == index)
   {
      if ( (4 == length) )
      {
         u32_t command  = (source[offset+3] << 24) | (source[offset+2] << 16) | (source[offset+1] << 8) | source[offset+0];

         if ( (0x65766173 == command) )
         {
            u32_t busConfiguration = (0x5A25 << 16) | (objectBitRateCode << 8) | objectNodeId;

            if ( (EEPROM_Write(EEPROM_INT, EEPROM_BUS_ADDR, &busConfiguration, sizeof(busConfiguration)) == EEPROM_ERR_OK) &&
                 (EEPROM_FlushCache(EEPROM_INT) == EEPROM_ERR_OK) ) 
            {
               result = 1;
            }
            else
            {
               result = 0;
            }
         }
      }
   }            

   return(result);
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
static u32_t sendDebug(u32_t codeA, u32_t codeB)
{
   CANMsg_t TxMsg;
 
   TxMsg.Id = CAN_COB(CAN_EMGY_T, deviceNodeId);
   TxMsg.Len = 8;
   TxMsg.Type = CAN_MSG_STANDARD;
   TxMsg.Data.Data8[0] = ((codeA >> 0) & 0xFF);
   TxMsg.Data.Data8[1] = ((codeA >> 8) & 0xFF);
   TxMsg.Data.Data8[2] = ((codeA >> 16) & 0xFF);
   TxMsg.Data.Data8[3] = ((codeA >> 24) & 0xFF);
   TxMsg.Data.Data8[4] = ((codeB >> 0) & 0xFF);
   TxMsg.Data.Data8[5] = ((codeB >> 8) & 0xFF);
   TxMsg.Data.Data8[6] = ((codeB >> 16) & 0xFF);
   TxMsg.Data.Data8[7] = ((codeB >> 24) & 0xFF);

   return( CAN_UserWrite(&TxMsg) );
}

static u32_t sendHeartbeat(u8_t value)
{
   CANMsg_t TxMsg;
 
   TxMsg.Id = CAN_COB(CAN_ERROR_T, deviceNodeId);
   TxMsg.Len = 1;
   TxMsg.Type = CAN_MSG_STANDARD;
   TxMsg.Data.Data8[0] = value;

   return( CAN_UserWrite(&TxMsg) );
}

static u32_t sendPdoData(DEVICE_TPDO_MAP * txPdoMap)
{
   CANMsg_t TxMsg;
   u8_t i;
   u8_t offset = 0;

   TxMsg.Id = txPdoMap->cobId;
   TxMsg.Type = CAN_MSG_STANDARD;

   for (i = 0; i < txPdoMap->mapCount; i++)
   {
      u16_t mapIndex = (u16_t)((txPdoMap->mappings[i] >> 16) & 0xFFFF);
      u8_t mapSubIndex = (u8_t)((txPdoMap->mappings[i] >> 8) & 0xFF);
      loadDeviceData(mapIndex, mapSubIndex, &TxMsg.Data.Data8[offset], 0);                     
      offset += pdoMapByteCount(txPdoMap->mappings[i]);;
   }

   TxMsg.Len = offset;

   return( CAN_UserWrite(&TxMsg) );
}

static u32_t sendSdoData(void)
{
   CANMsg_t TxMsg;
 
   TxMsg.Id = CAN_COB(CAN_TSDO_T, deviceNodeId);
   TxMsg.Len = 8;
   TxMsg.Type = CAN_MSG_STANDARD;

   if ( (0 != transferUpload) )
   {
      if ( (0 == transferStarted) )
      {
         if ( (transferLength <= 4) )
         {
            // send data

            u8_t i;

            TxMsg.Data.Data8[0] = ((2 << 5) | ((4 - transferLength) << 2) | 3);
            TxMsg.Data.Data8[1] = ((transferIndex >> 0) & 0xFF);
            TxMsg.Data.Data8[2] = ((transferIndex >> 8) & 0xFF);
            TxMsg.Data.Data8[3] = transferSubIndex;

            for (i = 0; i < transferLength; i++)
            {
               if ( (i < transferLength) )
               {
                  TxMsg.Data.Data8[4 + i] = transferBuffer[i];
               }
               else
               {
                  TxMsg.Data.Data8[4 + i] = 0x55;
               }
            }

            transferActive = 0;
         }
         else
         {
            // send length

            TxMsg.Data.Data8[0] = ((2 << 5) | 1); ;
            TxMsg.Data.Data8[1] = ((transferIndex >> 0) & 0xFF);
            TxMsg.Data.Data8[2] = ((transferIndex >> 8) & 0xFF);
            TxMsg.Data.Data8[3] = transferSubIndex;
            TxMsg.Data.Data8[4] = ((transferLength >> 0) & 0xFF);
            TxMsg.Data.Data8[5] = ((transferLength >> 8) & 0xFF);
            TxMsg.Data.Data8[6] = ((transferLength >> 16) & 0xFF);
            TxMsg.Data.Data8[7] = ((transferLength >> 24) & 0xFF);

            transferStarted = 1;
         }
      }
      else
      {
         // send segment

         u32_t remaining = transferLength - transferOffset;
         u8_t n = ((remaining < 7) ? (7 - remaining) : 0);
         u8_t t = (0 != transferToggle) ? 1 : 0;
         u8_t c = (remaining <= 7) ? 1 : 0;
         u8_t i;

         TxMsg.Data.Data8[0] = ((t << 4) | (n << 1) | c);

         for (i = 0; i < 7; i++)
         {
            u8_t ch = 0;

            if ( (i < remaining) )
            {
               ch = transferBuffer[transferOffset + i];
            }

            TxMsg.Data.Data8[1 + i] = ch;
         }

         transferLastLength = (7 - n);
      }
   }
   else
   {
      if ( (0 == transferStarted) )
      {
         if ( (transferLength <= 4) )
         {
            TxMsg.Data.Data8[0] = 0x60;
            TxMsg.Data.Data8[1] = ((transferIndex >> 0) & 0xFF);
            TxMsg.Data.Data8[2] = ((transferIndex >> 8) & 0xFF);
            TxMsg.Data.Data8[3] = transferSubIndex;
            TxMsg.Data.Data8[4] = 0;
            TxMsg.Data.Data8[5] = 0;
            TxMsg.Data.Data8[6] = 0;
            TxMsg.Data.Data8[7] = 0;

            transferActive = 0;
         }
         else
         {
            TxMsg.Data.Data8[0] = 0x60;
            TxMsg.Data.Data8[1] = ((transferIndex >> 0) & 0xFF);
            TxMsg.Data.Data8[2] = ((transferIndex >> 8) & 0xFF);
            TxMsg.Data.Data8[3] = transferSubIndex;
            TxMsg.Data.Data8[4] = 0;
            TxMsg.Data.Data8[5] = 0;
            TxMsg.Data.Data8[6] = 0;
            TxMsg.Data.Data8[7] = 0;

            transferStarted = 1;
         }
      }
      else
      {
         TxMsg.Data.Data8[0] = (0x20 | ((0 != transferToggle) ? 0x10 : 0));
         TxMsg.Data.Data8[1] = ((transferIndex >> 0) & 0xFF);
         TxMsg.Data.Data8[2] = ((transferIndex >> 8) & 0xFF);
         TxMsg.Data.Data8[3] = transferSubIndex;
         TxMsg.Data.Data8[4] = 0;
         TxMsg.Data.Data8[5] = 0;
         TxMsg.Data.Data8[6] = 0;
         TxMsg.Data.Data8[7] = 0;
      }
   }

   return( CAN_UserWrite(&TxMsg) );
}

static u32_t sendSdoAbort(u16_t index, u8_t subIndex, u32_t code)
{
   CANMsg_t TxMsg;
 
   TxMsg.Id = CAN_COB(CAN_TSDO_T, deviceNodeId);
   TxMsg.Len = 8;
   TxMsg.Type = CAN_MSG_STANDARD;
   TxMsg.Data.Data8[0] = (4 << 5);
   TxMsg.Data.Data8[1] = ((index >> 0) & 0xFF);
   TxMsg.Data.Data8[2] = ((index >> 8) & 0xFF);
   TxMsg.Data.Data8[3] = subIndex;
   TxMsg.Data.Data8[4] = ((code >> 0) & 0xFF);
   TxMsg.Data.Data8[5] = ((code >> 8) & 0xFF);
   TxMsg.Data.Data8[6] = ((code >> 16) & 0xFF);
   TxMsg.Data.Data8[7] = ((code >> 24) & 0xFF);

   return( CAN_UserWrite(&TxMsg) );
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
static void updateHeartbeat(void)
{
   if ( (0 != producerHeartbeatTime) )
   {
      if (SYSTIME_DIFF (heartbeatTimeCounter, SYSTIME_NOW) >= (u32_t)(1000 * producerHeartbeatTime))
      {
         u8_t stateValue = (DEVICE_PREOPERATIONAL_S == deviceState) ? 0x7F : ( (DEVICE_OPERATIONAL_S == deviceState) ? 5 : 4);
         heartbeatTimeCounter = SYSTIME_NOW ;
         sendHeartbeat(stateValue);
      }
   }
}

static void updateTxPdoMap(DEVICE_TPDO_MAP * tpdoMap)
{
   if ( (DEVICE_OPERATIONAL_S == deviceState) )
   {
      if ( (0 != tpdoMap->eventTime) &&
           (SYSTIME_DIFF (tpdoMap->processTimeCount, SYSTIME_NOW) >= (u32_t)(1000 * tpdoMap->eventTime)) )
      {
         tpdoMap->processTimeCount = SYSTIME_NOW ;

         if ((254 == tpdoMap->txType) || (255 == tpdoMap->txType))
         {
            tpdoMap->processNeeded = 1;
         }
      }

      if ( (0 != tpdoMap->processNeeded) && 
           (SYSTIME_DIFF (tpdoMap->inhibitTimeCount, SYSTIME_NOW) >= (u32_t)(100 * tpdoMap->inhibitTime)) )
      {
         sendPdoData(tpdoMap);
         tpdoMap->inhibitTimeCount = SYSTIME_NOW;
         tpdoMap->processNeeded = 0;
      }
   } 
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
static void initiateSdoDownload(u8_t * frame)
{
   if ( (0 != transferActive) )
   {
      u32_t dataLength = transferOffset + transferLastLength;

      if ( (dataLength == transferLength) )
      {
         transferActive = 0;
      }
   }

   if ( (0 == transferActive) )
   {
      u8_t n = ((frame[0] >> 2) & 0x3);
      u8_t e = ((frame[0] >> 1) & 0x1);
      u8_t s = ((frame[0] >> 0) & 0x1);
      u16_t index = (frame[2] << 8) | frame[1];
      u8_t subIndex = frame[3];

      if ((0 == e) && (0 == s))
      {
         // reserved
         sendSdoAbort(index, subIndex, 0x08000000);
      }
      else if ((0 == e) && (1 == s))
      {
         // data = number of bytes to transfer

         u32_t dataLength = (frame[7] << 24) | (frame[6] << 16) | (frame[5] << 8) | frame[4];
         u8_t valid = evaluateDeviceDataSize(index, subIndex, dataLength);
         
         if ( (0 != valid) )
         {
            transferActive = 1;
            transferUpload = 0;
            transferStarted = 0;
            transferIndex = index;
            transferSubIndex = subIndex;
            transferToggle = 0;
            transferLength = dataLength;
            transferOffset = 0;
            transferLastLength = 0;

            sendSdoData();
         }
         else
         {
            sendSdoAbort(index, subIndex, 0x08000000);
         }
      }
      else if ((1 == e) && (1 == s))
      {
         // data = (4 - n) bytes within frame

         u32_t dataLength = (u32_t)(4 - n);
         u8_t valid = storeDeviceData(index, subIndex, frame, 4, dataLength);

         if ( (0 != valid) )
         {
            transferActive = 1;
            transferUpload = 0;
            transferStarted = 0;
            transferIndex = index;
            transferSubIndex = subIndex;
            transferToggle = 0;
            transferLength = dataLength;
            transferOffset = 0;
            transferLastLength = 0;

            sendSdoData();
         }
         else
         {
            sendSdoAbort(index, subIndex, 0x08000000);
         }
      }
      else if ((1 == e) && (0 == s))
      {
         // data = unspecified number of bytes to transfer

         u8_t valid = storeDeviceData(index, subIndex, frame, 4, 4);

         if ( (0 != valid) )
         {
            transferActive = 1;
            transferUpload = 0;
            transferStarted = 0;
            transferIndex = index;
            transferSubIndex = subIndex;
            transferToggle = 0;
            transferLength = 4;
            transferOffset = 0;
            transferLastLength = 0;

            sendSdoData();
         }
         else
         {
            sendSdoAbort(index, subIndex, 0x08000000);
         }
      }
   }
   else
   {
      u16_t index = (frame[2] << 8) | frame[1];
      u8_t subIndex = frame[3];

      sendSdoAbort(index, subIndex, 0x08000001);
      transferActive = 0;
   }
}

static void processSdoDownload(u8_t * frame)
{
   u8_t t = ((frame[0] >> 4) & 0x1);
   u8_t n = ((frame[0] >> 1) & 0x7);
   u8_t c = ((frame[0] >> 0) & 0x1);

   if ( (0 != transferActive) &&
        (0 != transferStarted) &&
        (0 == transferUpload) )
   {
      u8_t dataCount = (7 - n);
      u8_t i;
      u32_t dataLength;

      if ( (t != transferToggle) )
      {
         transferOffset += transferLastLength;
         transferToggle = t;
      }

      for (i = 0; i < dataCount; i++)
      {
         transferBuffer[transferOffset + i] = frame[i + 1];
      }

      transferLastLength = dataCount;
      dataLength = transferOffset + transferLastLength;

      if ( (0 != c) &&
           (dataLength == transferLength) )
      {
         u8_t valid = storeDeviceData(transferIndex, transferSubIndex, transferBuffer, 0, dataLength);

         if ( (0 != valid) )
         {
            sendSdoData();
         }
         else
         {
            u16_t index = (frame[2] << 8) | frame[1];
            u8_t subIndex = frame[3];

            sendSdoAbort(index, subIndex, 0x08000000);
            transferActive = 0;
         }
      }
      else
      {
         sendSdoData();
      }
   }
   else
   {
      u16_t index = (frame[2] << 8) | frame[1];
      u8_t subIndex = frame[3];

      sendSdoAbort(index, subIndex, 0x08000000);
      transferActive = 0;
   }
}

static void initiateSdoUpload(u8_t * frame)
{
   u16_t index = (frame[2] << 8) | frame[1];
   u8_t subIndex = frame[3];

   if ( (0 != transferActive) )
   {
      u32_t dataLength = transferOffset + transferLastLength;

      if ( (dataLength == transferLength) )
      {
         transferActive = 0;
      }
   }

   if ( (0 == transferActive) )
   {
      u32_t dataLength = 0;
      u8_t valid = loadDeviceData(index, subIndex, transferBuffer, &dataLength);

      if ( (0 != valid) )
      {
         transferActive = 1;
         transferUpload = 1;
         transferStarted = 0;
         transferIndex = index;
         transferSubIndex = subIndex;
         transferToggle = 0;
         transferLength = dataLength;
         transferOffset = 0;
         transferLastLength = 0;

         sendSdoData();
      }
      else
      {
         sendSdoAbort(index, subIndex, 0x08000000);
      }
   }
   else
   {
      sendSdoAbort(index, subIndex, 0x08000001);
      transferActive = 0;
   }
}

static void processSdoUpload(u8_t * frame)
{
   u8_t toggled = ((frame[0] >> 4) & 0x1);
   u16_t index = (frame[2] << 8) | frame[1];
   u8_t subIndex = frame[3];

   if ( (0 != transferActive) &&
        (0 != transferStarted) &&
        (0 != transferUpload) && 
        (index == transferIndex) &&
        (subIndex == transferSubIndex) )
   {
      if (toggled != transferToggle)
      {
         transferOffset += transferLastLength;
         transferToggle = toggled;
      }

      sendSdoData();
   }
   else
   {
      sendSdoAbort(index, subIndex, 0x08000000);
      transferActive = 0;
   }
}

static void abortSdoTransfer(u8_t * frame)
{
   u16_t index = (frame[2] << 8) | frame[1];
   u8_t subIndex = frame[3];
   u32_t abortCode = (frame[7] << 24) | (frame[6] << 16) | (frame[5] << 8) | frame[4];

   if ( (0 != transferActive) &&
        (index == transferIndex) &&
        (subIndex == transferSubIndex) )
   {
      sendSdoAbort(index, subIndex, abortCode);
      transferActive = 0;
   }
}

static void processNmtMessage(u8_t * frame)
{
   if ( (0 == frame[1]) ||
        (deviceNodeId == frame[1]) )
   {
      if ( (0x01 == frame[0]) )
      {
         setOperationalState();                             
      }
      else if ( (0x02 == frame[0]) )
      {
         setStoppedState();
      }
      else if ( (0x80 == frame[0]) ||
                (0x81 == frame[0]) ||
                (0x82 == frame[0]) )
      {
         setPreOperationalState();
      }
   }
}

static void processSyncMessage(void)
{
   u8_t i;

   for (i=0; i<4 ;i++)
   {
      if ( (tpdoMapping[i].txType >= 1) && (tpdoMapping[i].txType <= 240) )
      {
         tpdoMapping[i].syncCount++;

         if ( (tpdoMapping[i].syncCount >= tpdoMapping[i].txType) )
         {
            tpdoMapping[i].syncCount = 0;
            tpdoMapping[i].processNeeded = 1;
         }
      }
   }
}

static void processSdoMessage(u8_t * frame)
{
   u8_t css = (int)((frame[0] >> 5) & 0x7);

   if ( (0 == css) )
   {
      processSdoDownload(frame);
   }
   else if ( (1 == css) )
   {
      initiateSdoDownload(frame);
   }
   else if ( (2 == css) )
   {
      initiateSdoUpload(frame);
   }
   else if ( (3 == css) )
   {
      processSdoUpload(frame);
   }
   else if ( (4 == css) )
   {
      abortSdoTransfer(frame);
   }
   else
   {
      u16_t index = index = (frame[1] << 8) | frame[0];
      u8_t subIndex = subIndex = frame[2];      

      sendSdoAbort(index, subIndex, 0x08000000);
   }
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
static void setLedState(LED_STATE state)
{
   ledState = state;
   ledFlashStep = 0;
   ledFlashLimit = 0;               
   ledFlashCount = 0;   

   if ( (LED_OPERATIONAL_S == state) )
   {
      HW_SetLED(HW_LED_STATUS_1, HW_LED_GREEN);
   }
}

static void setPreOperationalState(void)
{
   u8_t i;
   s32_t res = 0;
   u32_t busConfiguration;
   u8_t deviceBitRateCode;

   deviceState = DEVICE_PREOPERATIONAL_S;

   Initialized = 0;
      
   //init_system();
#if 1
   // Initialize Basic Parts
   HW_Init();

   // Initialize Systemtimer
   Init_Timer0();

   rtc_initialize();

   // read configuration values from EEPROM if present
   res = read_persistent_config();
   if(res!=RET_OK)
      get_default_config();

   // Init GPS and UART2
   UBLOX_MAX7W_init();

   // Initialize gyroscope
   MEMS_L3GD20_init();

   // Initialize magnetometer and accelerometer
   MEMS_BMC050_init_Accelerometer();
   MEMS_BMC050_init_Magnetometer();

   // set values read from EEPROM *after* initialization of sensors with
   // default values
   MEMS_BMC050_SetAccCalTargets( &cfg_data.Acc.cmp_target_x, &cfg_data.Acc.cmp_target_y, &cfg_data.Acc.cmp_target_z);
   MEMS_BMC050_SetAccRange(cfg_data.Acc.range);

   MEMS_L3GD20_SetRange(cfg_data.Gyro.range);

   Initialized = 1;

#endif

   HW_SetLED(HW_LED_STATUS_1, HW_LED_OFF);
   HW_SetLED(HW_LED_STATUS_2, HW_LED_OFF);
   
   HW_GetModuleID(&deviceNodeId);
   deviceNodeId = 63 + (~deviceNodeId&0x7);

   deviceBitRateCode = 6;

   objectNodeId = deviceNodeId;
   objectBitRateCode = deviceBitRateCode;

   if ( (EEPROM_Read(EEPROM_INT, EEPROM_BUS_ADDR, (u8_t*)&busConfiguration, sizeof(busConfiguration)) == EEPROM_ERR_OK) )
   {
      u16_t check = ((busConfiguration >> 16) & 0xFFFF);

      if ( (0x5A25 == check) )
      {
         objectNodeId = (busConfiguration >> 0) & 0xFF;
         objectBitRateCode = (busConfiguration >> 8) & 0xFF;

         deviceNodeId = objectNodeId;
         deviceBitRateCode = objectBitRateCode;
      }
   }

   // Initialize CAN Controller
   res = CAN_UserInit(deviceBitRateCode);

   //sendDebug(objectNodeId, objectBitRateCode);

   transferActive = 0;
   producerHeartbeatTime = 0;

   for (i=0; i<4; i++)
   {
      resetTxPdoMap(&tpdoMapping[i], i);
   }

   if ( (sendHeartbeat(0) != 0) )
   {
      setLedState(LED_PREOPERATIONAL_OK_S);
   }
   else
   {
      setLedState(LED_PREOPERATIONAL_ERROR_S);
   }
}

static void setOperationalState(void)
{
   deviceState = DEVICE_OPERATIONAL_S;
   setLedState(LED_OPERATIONAL_S);
}

static void setStoppedState(void)
{
   deviceState = DEVICE_STOPPED_S;
   setLedState(LED_STOPPED_S);
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
static void executePreOperationalState(void)
{
   UBLOX_MAX7W_task();
   updateHeartbeat();

   receiveResult = CAN_UserRead(&message);

   if ( (0 != receiveResult) )
   {
      messageType = CAN_TYPE(message.Id);
      messageDeviceId = CAN_ID(message.Id);

      if ( (0 == messageDeviceId) || (deviceNodeId == messageDeviceId) )
      {
         if ( (CAN_NMT_T ==  messageType) )
         {
            processNmtMessage(message.Data.Data8);
         }
         else if ( (CAN_RSDO_T ==  messageType) )
         {
            processSdoMessage(message.Data.Data8);
         }
      }      
   }
}

static void executeOperationalState(void)
{  
   u8_t i;
    
   UBLOX_MAX7W_task();
   updateHeartbeat();

   for (i=0; i<4; i++)
   {
      updateTxPdoMap(&tpdoMapping[i]);
   }

   receiveResult = CAN_UserRead(&message);

   if ( (0 != receiveResult) )
   {
      messageType = CAN_TYPE(message.Id);
      messageDeviceId = CAN_ID(message.Id);

      if ( (0 == messageDeviceId) || (deviceNodeId == messageDeviceId) )
      {
         if ( (CAN_NMT_T ==  messageType) )
         {
            processNmtMessage(message.Data.Data8);
         }
         else if ( (CAN_SYNC_T == messageType) )
         {
            processSyncMessage();
         }
         else if ( (CAN_RSDO_T ==  messageType) )
         {
            processSdoMessage(message.Data.Data8);
         }
      }      
   }
}

static void executeStoppedState(void)
{
   UBLOX_MAX7W_task();
   updateHeartbeat();

   receiveResult = CAN_UserRead(&message);

   if ( (0 != receiveResult) )
   {
      messageType = CAN_TYPE(message.Id);
      messageDeviceId = CAN_ID(message.Id);

      if ( (0 == messageDeviceId) || (deviceNodeId == messageDeviceId) )
      {
         if ( (CAN_NMT_T ==  messageType) )
         {
            processNmtMessage(message.Data.Data8);
         }
      }      
   }
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */



/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
static void executeDevice(void)
{
   setPreOperationalState();

   while(1)    
   {      
      // todo state table
      switch (deviceState)
      {
         case DEVICE_PREOPERATIONAL_S:
         {
            executePreOperationalState();
            break;
         }
         case DEVICE_OPERATIONAL_S:
         {
            executeOperationalState();
            break;
         }
         case DEVICE_STOPPED_S:
         {
            executeStoppedState();
            break;
         }
      }
   }

   sendDebug(0,0);
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


