
#include <string.h>

#include "lpc21xx.h"

#include "datatypes.h"
#include "can.h"
#include "can_user.h"
#include "hardware.h"
#include "crc_data.h"
#include "ser_user.h"
#include "systime.h"
#include "eeprom.h"

// identifier is needed by PCANFlash.exe -> do not delete
const b8_t Ident[] __attribute__ ((used)) = { "PCAN-RS-232"};


// info data for PCANFlash.exe
const crc_array_t  C2F_Array __attribute__((section(".C2F_Info"), used)) = {

	.Str = CRC_IDENT_STRING,
	.Version = 0x21,
	.Day = 5,
	.Month = 5,
	.Year = 6,
	.Mode = 1,

	// crc infos are patched during link time by flash.ld
	// crc value is patched by PCANFlash.exe
};

/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */

#define EEPROM_CHECK_VALUE (0xA2)
#define SERIAL_TX_BUFFER_SIZE (128)
#define SERIAL_RX_BUFFER_SIZE (128)

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

typedef struct
{
   u8_t mapCount; // number of mappings
   u32_t mappings[8]; // maps

   u8_t txType; // transmit type: 0..F0=n syncs, FE=event, FF=event
   u32_t cobId; // cob of PDO

   u8_t syncCount; // number of received syncs
   u8_t processNeeded; // used for event activity and SYNC trigger

   u8_t frame[8]; // holds data until trigger
   u8_t length; // length of frame data

}DEVICE_RPDO_MAP;

static u32_t getValue(u8_t * source, u8_t length);
static u8_t pdoMapByteCount(u32_t mapping);
static u8_t pdoByteSize(u16_t index, u8_t subIndex);

static u8_t pdoTxMappable(u16_t index, u8_t subIndex);
static void loadTxPdoMapParameter(DEVICE_TPDO_MAP * txPdoMap, u8_t subIndex, u8_t * buffer, u32_t * dataCount);
static void loadTxPdoMapData(DEVICE_TPDO_MAP * txPdoMap, u8_t subIndex, u8_t * buffer, u32_t * dataCount);
static u8_t storeTxPdoMapParameter(DEVICE_TPDO_MAP * txPdoMap, u8_t subIndex, u8_t byteCount, u32_t value);
static u8_t storeTxPdoMapData(DEVICE_TPDO_MAP * txPdoMap, u8_t subIndex, u8_t byteCount, u32_t value);
static void activateTxPdoMap(DEVICE_TPDO_MAP * txPdoMap, u16_t index, u8_t subIndex);
static void resetTxPdoMap(DEVICE_TPDO_MAP * txPdoMap, u8_t index);

static u8_t pdoRxMappable(u16_t index, u8_t subIndex);
static void loadRxPdoMapParameter(DEVICE_RPDO_MAP * rxPdoMap, u8_t subIndex, u8_t * buffer, u32_t * dataCount);
static void loadRxPdoMapData(DEVICE_RPDO_MAP * rxPdoMap, u8_t subIndex, u8_t * buffer, u32_t * dataCount);
static u8_t storeRxPdoMapParameter(DEVICE_RPDO_MAP * rxPdoMap, u8_t subIndex, u8_t byteCount, u32_t value);
static u8_t storeRxPdoMapData(DEVICE_RPDO_MAP * rxPdoMap, u8_t subIndex, u8_t byteCount, u32_t value);
static void resetRxPdoMap(DEVICE_RPDO_MAP * rxPdoMap, u8_t index);

static u8_t evaluateDeviceDataSize(u16_t index, u8_t subIndex, u32_t length);
static u8_t loadDeviceData(u16_t index, u8_t subIndex, u8_t * buffer, u32_t * length, u32_t limit);
static u8_t storeDeviceData(u16_t index, u8_t subIndex, u8_t * source, u32_t offset, u32_t length);

static u32_t sendDebug(u32_t codeA, u32_t codeB);
static u32_t sendHeartbeat(u8_t value);
static u32_t sendPdoData(DEVICE_TPDO_MAP * txPdoMap);
static u32_t sendSdoData(void);
static u32_t sendSdoAbort(u16_t index, u8_t subIndex, u32_t code);

static void updateLED(void);
static void updateHeartbeat(void);
static void updateTxPdoMap(DEVICE_TPDO_MAP * txPdoMap);
static void updateRxPdoMap(DEVICE_RPDO_MAP * rxPdoMap);
static void updateSerial(void);

static void initiateSdoDownload(u8_t * frame);
static void processSdoDownload(u8_t * frame);
static void initiateSdoUpload(u8_t * frame);
static void processSdoUpload(u8_t * frame);
static void abortSdoTransfer(u8_t * frame);

static void processNmtMessage(u8_t * frame);
static void processSyncMessage(void);
static void processRxPdoMessage(u8_t pdoId, u8_t * frame, u8_t length);
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
static u32_t ledTimeCounter;

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
static u8_t transferBuffer[128];

static DEVICE_TPDO_MAP txPdoMapping[4];
static DEVICE_RPDO_MAP rxPdoMapping[4];

static u32_t deviceType = 0;
static u32_t errorStatus = 0;
static char * deviceName = "PCAN-RS232 by ULC Robotics";
static char * version = "v1.00 " __DATE__ " " __TIME__;  

static u8_t objectBitRateCode;
static u8_t objectNodeId;
static u8_t objectNodeOffset;

static u16_t producerHeartbeatTime;
static u32_t heartbeatTimeCounter;

static u32_t serialBaud;
static u8_t serialDataBits;
static u8_t serialStopBits;
static u8_t serialParity;

static u8_t serialTxInIndex;
static u8_t serialTxOutIndex;
static u8_t serialTxLastIndex;
static u8_t serialTxBuffer[SERIAL_TX_BUFFER_SIZE];

static u8_t serialRxInIndex;
static u8_t serialRxOutIndex;
static u8_t serialRxLastIndex;
static u8_t serialRxBuffer[SERIAL_RX_BUFFER_SIZE];

static u32_t serialRxTimeCounter; 

/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
static u32_t getValue(u8_t * source, u8_t length)
{
   u32_t result = 0;
   u8_t i;
   u32_t shifter = 0;

   for (i=0; i<length; i++)
   {
      result |= (u32_t)source[i] << shifter;
      shifter += 8;
   }

   return(result);
}

static u8_t pdoMapByteCount(u32_t mapping)
{
   u8_t mapByteCount = (u8_t)((mapping & 0xFF) / 8);
   return (mapByteCount);
}

static u8_t pdoByteSize(u16_t index, u8_t subIndex)
{
   u8_t result = 0;

   if ( (0x2300 == index) ||
        (0x2400 == index) )
   {
      result = 8;
   }
   else if ( (0x2500 == index) ||
             (0x2600 == index) )
   {
      result = 1;
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

   if ( (0x2400 == index) ||
        (0x2600 == index) )
   {
      result = 1;
   }

   return(result);
}

static void loadTxPdoMapParameter(DEVICE_TPDO_MAP * txPdoMap, u8_t subIndex, u8_t * buffer, u32_t * dataCount)
{
   if (0 == subIndex)
   {
      buffer[0] = 5;
      *dataCount = 1;
   }
   else if (1 == subIndex)
   {
      u32_t cobId = txPdoMap->cobId;
      buffer[0] = ((cobId >> 0) & 0xFF);
      buffer[1] = ((cobId >> 8) & 0xFF);
      buffer[2] = ((cobId >> 16) & 0xFF);
      buffer[3] = ((cobId >> 24) & 0xFF);
      *dataCount = 4;
   }
   else if (2 == subIndex)
   {
      buffer[0] = txPdoMap->txType;
      *dataCount = 1;
   }
   else if (3 == subIndex)
   {
      buffer[0] = ((txPdoMap->inhibitTime >> 0) & 0xFF);
      buffer[1] = ((txPdoMap->inhibitTime >> 8) & 0xFF);
      *dataCount = 2;
   }
   else if (5 == subIndex)
   {
      buffer[0] = ((txPdoMap->eventTime >> 0) & 0xFF);
      buffer[1] = ((txPdoMap->eventTime >> 8) & 0xFF);
      *dataCount = 2;
   }
}

static void loadTxPdoMapData(DEVICE_TPDO_MAP * txPdoMap, u8_t subIndex, u8_t * buffer, u32_t * dataCount)
{
   if (0 == subIndex)
   {
      buffer[0] = txPdoMap->mapCount;
      *dataCount = 1;
   }
   else if ((subIndex >= 1) && (subIndex <= 8))
   {
      u32_t mapping = txPdoMap->mappings[subIndex - 1];

      buffer[0] = ((mapping >> 0) & 0xFF);
      buffer[1] = ((mapping >> 8) & 0xFF);
      buffer[2] = ((mapping >> 16) & 0xFF);
      buffer[3] = ((mapping >> 24) & 0xFF);

      *dataCount = 4;
   }
}

static u8_t storeTxPdoMapParameter(DEVICE_TPDO_MAP * txPdoMap, u8_t subIndex, u8_t byteCount, u32_t value)
{
   u8_t result = 0;

   if ( (DEVICE_PREOPERATIONAL_S == deviceState) )
   {
      if ( (1 == subIndex) )
      {
         if ( (4 == byteCount) )
         {
            txPdoMap->cobId = value;
            result = 1;
         }
      }
      else if ( (2 == subIndex) )
      {
         if ( (1 == byteCount) )
         {
            txPdoMap->txType = (u8_t)value;
            result = 1;
         }
      }
      else if ( (3 == subIndex) )
      {
         if ( (2 == byteCount) )
         {
            txPdoMap->inhibitTime = (u16_t)value;
            result = 1;
         }
      }
      else if ( (5 == subIndex) )
      {
         if ( (2 == byteCount) )
         {
            txPdoMap->eventTime = (u16_t)value;
            result = 1;
         }
      }
   }

   return (result);
}

static u8_t storeTxPdoMapData(DEVICE_TPDO_MAP * txPdoMap, u8_t subIndex, u8_t byteCount, u32_t value)
{
   u8_t result = 0;

   if ( (DEVICE_PREOPERATIONAL_S == deviceState) )
   {
      if ( (0 == subIndex) )
      {
         if ( (1 == byteCount) )
         {
            txPdoMap->mapCount = (u8_t)value;
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
                     mapSize += pdoMapByteCount(txPdoMap->mappings[i - 1]);
                  }

                  if (mapSize <= 8)
                  {
                     txPdoMap->mappings[subIndex - 1] = value;
                     result = 1;
                  }
               }
            }
         }
      }
   }

   return (result);
}

static void activateTxPdoMap(DEVICE_TPDO_MAP * txPdoMap, u16_t index, u8_t subIndex)
{
   if ( (DEVICE_OPERATIONAL_S == deviceState) )
   {
      for (int i=0; i<txPdoMap->mapCount; i++)
      {
         u32_t mapping = txPdoMap->mappings[i];
         u16_t mapIndex = (u16_t)((mapping >> 16) & 0xFFFF);
         u8_t mapSubIndex = (u8_t)((mapping >> 8) & 0xFF);

         if ( (index == mapIndex) && 
              (subIndex == mapSubIndex) )
         {
            txPdoMap->processNeeded = 1;
            break;
         }
      }
   }
}

static void resetTxPdoMap(DEVICE_TPDO_MAP * txPdoMap, u8_t index)
{
   u8_t i;
   CAN_FrameType pdoType;

   txPdoMap->mapCount = 0;

   for (i = 0; i < 8; i++)
   {
      txPdoMap->mappings[i] = 0;
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

   txPdoMap->txType = 0;
   txPdoMap->cobId = (u32_t)(0x40000000 | ((u32_t)pdoType) << 7) | ((u32_t)deviceNodeId & 0x7F);

   txPdoMap->inhibitTime = 0;
   txPdoMap->eventTime = 0;

   txPdoMap->syncCount = 0;
   txPdoMap->inhibitTimeCount = SYSTIME_NOW;
   txPdoMap->processTimeCount = SYSTIME_NOW;
   txPdoMap->processNeeded = 0;
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
static u8_t pdoRxMappable(u16_t index, u8_t subIndex)
{
   u8_t result = 0;

   if ( (0x2300 == index) ||
        (0x2500 == index) )
   {
      result = 1;
   }

   return(result);
}

static void loadRxPdoMapParameter(DEVICE_RPDO_MAP * rxPdoMap, u8_t subIndex, u8_t * buffer, u32_t * dataCount)
{
   if (0 == subIndex)
   {
      buffer[0] = 2;
      *dataCount = 1;
   }
   else if (1 == subIndex)
   {
      u32_t cobId = rxPdoMap->cobId;
      buffer[0] = ((cobId >> 0) & 0xFF);
      buffer[1] = ((cobId >> 8) & 0xFF);
      buffer[2] = ((cobId >> 16) & 0xFF);
      buffer[3] = ((cobId >> 24) & 0xFF);
      *dataCount = 4;
   }
   else if (2 == subIndex)
   {
      buffer[0] = rxPdoMap->txType;
      *dataCount = 1;
   }
}

static void loadRxPdoMapData(DEVICE_RPDO_MAP * rxPdoMap, u8_t subIndex, u8_t * buffer, u32_t * dataCount)
{
   if (0 == subIndex)
   {
      buffer[0] = rxPdoMap->mapCount;
      *dataCount = 1;
   }
   else if ((subIndex >= 1) && (subIndex <= 8))
   {
      u32_t mapping = rxPdoMap->mappings[subIndex - 1];

      buffer[0] = ((mapping >> 0) & 0xFF);
      buffer[1] = ((mapping >> 8) & 0xFF);
      buffer[2] = ((mapping >> 16) & 0xFF);
      buffer[3] = ((mapping >> 24) & 0xFF);

      *dataCount = 4;
   }
}

static u8_t storeRxPdoMapParameter(DEVICE_RPDO_MAP * rxPdoMap, u8_t subIndex, u8_t byteCount, u32_t value)
{
   u8_t result = 0;

   if ( (DEVICE_PREOPERATIONAL_S == deviceState) )
   {
      if ( (1 == subIndex) )
      {
         if ( (4 == byteCount) )
         {
            rxPdoMap->cobId = value;
            result = 1;
         }
      }
      else if ( (2 == subIndex) )
      {
         if ( (1 == byteCount) )
         {
            rxPdoMap->txType = (u8_t)value;
            result = 1;
         }
      }
   }

   return (result);
}

static u8_t storeRxPdoMapData(DEVICE_RPDO_MAP * rxPdoMap, u8_t subIndex, u8_t byteCount, u32_t value)
{
   u8_t result = 0;

   if ( (DEVICE_PREOPERATIONAL_S == deviceState) )
   {
      if ( (0 == subIndex) )
      {
         if ( (1 == byteCount) )
         {
            rxPdoMap->mapCount = (u8_t)value;
            result = 1;
         }
      }
      else if ( (subIndex >= 1) && (subIndex <= 8) )
      {
         if ( (4 == byteCount) )
         {
            u16_t mapIndex = (u16_t)((value >> 16) & 0xFFFF);
            u8_t mapSubIndex = (u8_t)((value >> 8) & 0xFF);
            u8_t mappableObject = pdoRxMappable(mapIndex, mapSubIndex);

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
                     mapSize += pdoMapByteCount(rxPdoMap->mappings[i - 1]);
                  }

                  if (mapSize <= 8)
                  {
                     rxPdoMap->mappings[subIndex - 1] = value;
                     result = 1;
                  }
               }
            }
         }
      }
   }

   return (result);
}

static void resetRxPdoMap(DEVICE_RPDO_MAP * rxPdoMap, u8_t index)
{
   u8_t i;
   CAN_FrameType pdoType;

   rxPdoMap->mapCount = 0;

   for (i = 0; i < 8; i++)
   {
      rxPdoMap->mappings[i] = 0;
   }

   if ( (0 == index) )
   {
      pdoType = CAN_RPDO1_T;
   }
   else if ( (1 == index) )
   {
      pdoType = CAN_RPDO2_T;
   }
   else if ( (2 == index) )
   {
      pdoType = CAN_RPDO3_T;
   }
   else if ( (3 == index) )
   {
      pdoType = CAN_RPDO4_T;
   }

   rxPdoMap->txType = 0;
   rxPdoMap->cobId = (u32_t)(0x40000000 | ((u32_t)pdoType) << 7) | ((u32_t)deviceNodeId & 0x7F);

   rxPdoMap->syncCount = 0;
   rxPdoMap->processNeeded = 0;
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
static u8_t evaluateDeviceDataSize(u16_t index, u8_t subIndex, u32_t length)
{
   u8_t result = 0;

   // used for writable fields that are larger than 4 bytes
   if (0x2300 == index)
   {
      u8_t space = 0;
                 
      for (u8_t i = serialTxInIndex; i != serialTxLastIndex; )
      {
         space++;
         i = ((i+1) < SERIAL_TX_BUFFER_SIZE) ? (i+1) : 0;           
      }

      if ( (length <= space) )
      {
         result = 1;
      }      
   }

   return(result);
}

static u8_t loadDeviceData(u16_t index, u8_t subIndex, u8_t * buffer, u32_t * length, u32_t limit)
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
      buffer[0] = 1;
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
   else if ((0x1400 <= index) && (0x1403 >= index))
   {
      u8_t mappingOffset = (index - 0x1400);
      transferred = 0;
      loadRxPdoMapParameter(&rxPdoMapping[mappingOffset], subIndex, buffer, &transferred);
   }
   else if ((0x1600 <= index) && (0x1603 >= index))
   {
      u8_t mappingOffset = (index - 0x1600);
      transferred = 0;
      loadRxPdoMapData(&rxPdoMapping[mappingOffset], subIndex, buffer, &transferred);
   }
   else if ((0x1800 <= index) && (0x1803 >= index))
   {
      u8_t mappingOffset = (index - 0x1800);
      transferred = 0;
      loadTxPdoMapParameter(&txPdoMapping[mappingOffset], subIndex, buffer, &transferred);
   }
   else if ((0x1A00 <= index) && (0x1A03 >= index))
   {
      u8_t mappingOffset = (index - 0x1800);
      transferred = 0;
      loadTxPdoMapData(&txPdoMapping[mappingOffset], subIndex, buffer, &transferred);
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
   else if (0x2102 == index)
   {
      size = sizeof(objectNodeOffset);
      source = (u8_t *)&objectNodeOffset;
   }
   else if (0x2105 == index)
   {
      buffer[0] = 0x73;
      buffer[1] = 0x61;
      buffer[2] = 0x76;
      buffer[3] = 0x65;
      transferred = 4;
   }
   else if ((0x2200 == index) && (0 == subIndex))
   {
      buffer[0] = 4;
      transferred = 1;
   }
   else if ((0x2200 == index) && (1 == subIndex))
   {
      size = sizeof(serialBaud);
      source = (u8_t *)&serialBaud;
   }
   else if ((0x2200 == index) && (2 == subIndex))
   {
      size = sizeof(serialDataBits);
      source = (u8_t *)&serialDataBits;
   }
   else if ((0x2200 == index) && (3 == subIndex))
   {
      size = sizeof(serialStopBits);
      source = (u8_t *)&serialStopBits;
   }
   else if ((0x2200 == index) && (4 == subIndex))
   {
      size = sizeof(serialParity);
      source = (u8_t *)&serialParity;
   }
   else if (0x2400 == index)
   {
      u8_t count = 0;

      while ( (serialRxOutIndex != serialRxInIndex) )
      {
         if ( (count >= limit) )
         {
            break;
         }

         buffer[count++] = serialRxBuffer[serialRxOutIndex];
         serialRxLastIndex = serialRxOutIndex;      
         serialRxOutIndex = ((serialRxOutIndex+1) < SERIAL_RX_BUFFER_SIZE) ? (serialRxOutIndex+1) : 0;
      }

      transferred = count;
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
   else if ((0x1400 <= index) && (0x1403 >= index))
   {
      u8_t mappingOffset = (index - 0x1400);
      u32_t parameter = getValue(&source[offset], length);
      result = storeRxPdoMapParameter(&rxPdoMapping[mappingOffset], subIndex, length, parameter);
   }
   else if ((0x1600 <= index) && (0x1603 >= index))
   {
      u8_t mappingOffset = (index - 0x1600);
      u32_t data = getValue(&source[offset], length);
      result = storeRxPdoMapData(&rxPdoMapping[mappingOffset], subIndex, length, data);
   }
   else if ((0x1800 <= index) && (0x1803 >= index))
   {
      u8_t mappingOffset = (index - 0x1800);
      u32_t parameter = getValue(&source[offset], length);
      result = storeTxPdoMapParameter(&txPdoMapping[mappingOffset], subIndex, length, parameter);
   }
   else if ((0x1A00 <= index) && (0x1A03 >= index))
   {
      u8_t mappingOffset = (index - 0x1800);
      u32_t data = getValue(&source[offset], length);
      result = storeTxPdoMapData(&txPdoMapping[mappingOffset], subIndex, length, data);
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
         u8_t value = source[offset];

         if ( (value >= 0) && (value <= 127) )
         {
            objectNodeId = value;
            result = 1;
         }
      }
   }            
   else if (0x2102 == index)
   {
      if ( (1 == length) )
      {
         u8_t value = source[offset];

         if ( (value >= 1) && (value <= 112) )
         {
            objectNodeOffset = value;
            result = 1;
         }
      }
   }            
   else if (0x2105 == index)
   {
      if ( (4 == length) )
      {
         u32_t command  = (source[offset+3] << 24) | (source[offset+2] << 16) | (source[offset+1] << 8) | source[offset+0];

         if ( (0x65766173 == command) )
         {
            u32_t busConfiguration = (EEPROM_CHECK_VALUE << 24) | (objectNodeOffset << 16) | (objectNodeId << 8) | objectBitRateCode;

            if ( (EEPROM_Write(0, &busConfiguration, sizeof(busConfiguration)) == EEPROM_ERR_OK) &&
                 (EEPROM_FlushCache() == EEPROM_ERR_OK) ) 
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
   else if ((0x2200 == index) && (1 == subIndex))
   {
      if ( (DEVICE_PREOPERATIONAL_S == deviceState) && (4 == length) )
      {        
         serialBaud = getValue(&source[offset], length);
         result = 1;
      }
   }
   else if ((0x2200 == index) && (2 == subIndex))
   {
      if ( (DEVICE_PREOPERATIONAL_S == deviceState) && (1 == length) )
      {        
         u8_t value = source[offset];

         if ( (value >= 5) && (value <= 8) )
         {
            serialDataBits = value;
            result = 1;
         }
      }
   }
   else if ((0x2200 == index) && (3 == subIndex))
   {
      if ( (DEVICE_PREOPERATIONAL_S == deviceState) && (1 == length) )
      {        
         u8_t value = source[offset];

         if ( (value >= 1) && (value <= 2) )
         {
            serialStopBits = value;
            result = 1;
         }
      }
   }
   else if ((0x2200 == index) && (4 == subIndex))
   {
      if ( (DEVICE_PREOPERATIONAL_S == deviceState) && (1 == length) )
      {        
         u8_t value = source[offset];

         if ( (value >= 0) && (value <= 2) )
         {
            serialParity = value;
            result = 1;
         }
      }
   }
   else if (0x2300 == index)
   {
      result = 1;
      
      for (u8_t i=0; i<length; i++)
      {
         if ( (serialTxInIndex != serialTxLastIndex) )
         {
            serialTxBuffer[serialTxInIndex] = source[offset+i];
            serialTxInIndex = ((serialTxInIndex+1) < SERIAL_TX_BUFFER_SIZE) ? (serialTxInIndex+1) : 0;           
         }
         else
         {
            result = 0;
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
   TxMsg.Data32[0] = codeA;
   TxMsg.Data32[1] = codeB;

   return( CAN_UserWrite(CAN_BUS1, &TxMsg) );
}

static u32_t sendHeartbeat(u8_t value)
{
   CANMsg_t TxMsg;
 
   TxMsg.Id = CAN_COB(CAN_ERROR_T, deviceNodeId);
   TxMsg.Len = 1;
   TxMsg.Type = CAN_MSG_STANDARD;
   TxMsg.Data8[0] = value;

   return( CAN_UserWrite(CAN_BUS1, &TxMsg) );
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
      u8_t limit = 8 - offset;
      u32_t length = 0;
      loadDeviceData(mapIndex, mapSubIndex, &TxMsg.Data8[offset], &length, limit);
      offset += length;
   }

   TxMsg.Len = offset;

   return( CAN_UserWrite(CAN_BUS1, &TxMsg) );
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

            TxMsg.Data8[0] = ((2 << 5) | ((4 - transferLength) << 2) | 3);
            TxMsg.Data8[1] = ((transferIndex >> 0) & 0xFF);
            TxMsg.Data8[2] = ((transferIndex >> 8) & 0xFF);
            TxMsg.Data8[3] = transferSubIndex;

            for (i = 0; i < transferLength; i++)
            {
               if ( (i < transferLength) )
               {
                  TxMsg.Data8[4 + i] = transferBuffer[i];
               }
               else
               {
                  TxMsg.Data8[4 + i] = 0x55;
               }
            }

            transferActive = 0;
         }
         else
         {
            // send length

            TxMsg.Data8[0] = ((2 << 5) | 1); ;
            TxMsg.Data8[1] = ((transferIndex >> 0) & 0xFF);
            TxMsg.Data8[2] = ((transferIndex >> 8) & 0xFF);
            TxMsg.Data8[3] = transferSubIndex;
            TxMsg.Data8[4] = ((transferLength >> 0) & 0xFF);
            TxMsg.Data8[5] = ((transferLength >> 8) & 0xFF);
            TxMsg.Data8[6] = ((transferLength >> 16) & 0xFF);
            TxMsg.Data8[7] = ((transferLength >> 24) & 0xFF);

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

         TxMsg.Data8[0] = ((t << 4) | (n << 1) | c);

         for (i = 0; i < 7; i++)
         {
            u8_t ch = 0;

            if ( (i < remaining) )
            {
               ch = transferBuffer[transferOffset + i];
            }

            TxMsg.Data8[1 + i] = ch;
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
            TxMsg.Data8[0] = 0x60;
            TxMsg.Data8[1] = ((transferIndex >> 0) & 0xFF);
            TxMsg.Data8[2] = ((transferIndex >> 8) & 0xFF);
            TxMsg.Data8[3] = transferSubIndex;
            TxMsg.Data8[4] = 0;
            TxMsg.Data8[5] = 0;
            TxMsg.Data8[6] = 0;
            TxMsg.Data8[7] = 0;

            transferActive = 0;
         }
         else
         {
            TxMsg.Data8[0] = 0x60;
            TxMsg.Data8[1] = ((transferIndex >> 0) & 0xFF);
            TxMsg.Data8[2] = ((transferIndex >> 8) & 0xFF);
            TxMsg.Data8[3] = transferSubIndex;
            TxMsg.Data8[4] = 0;
            TxMsg.Data8[5] = 0;
            TxMsg.Data8[6] = 0;
            TxMsg.Data8[7] = 0;

            transferStarted = 1;
         }
      }
      else
      {
         TxMsg.Data8[0] = (0x20 | ((0 != transferToggle) ? 0x10 : 0));
         TxMsg.Data8[1] = ((transferIndex >> 0) & 0xFF);
         TxMsg.Data8[2] = ((transferIndex >> 8) & 0xFF);
         TxMsg.Data8[3] = transferSubIndex;
         TxMsg.Data8[4] = 0;
         TxMsg.Data8[5] = 0;
         TxMsg.Data8[6] = 0;
         TxMsg.Data8[7] = 0;
      }
   }

   return( CAN_UserWrite(CAN_BUS1, &TxMsg) );
}

static u32_t sendSdoAbort(u16_t index, u8_t subIndex, u32_t code)
{
   CANMsg_t TxMsg;
 
   TxMsg.Id = CAN_COB(CAN_TSDO_T, deviceNodeId);
   TxMsg.Len = 8;
   TxMsg.Type = CAN_MSG_STANDARD;
   TxMsg.Data8[0] = (4 << 5);
   TxMsg.Data8[1] = ((index >> 0) & 0xFF);
   TxMsg.Data8[2] = ((index >> 8) & 0xFF);
   TxMsg.Data8[3] = subIndex;
   TxMsg.Data8[4] = ((code >> 0) & 0xFF);
   TxMsg.Data8[5] = ((code >> 8) & 0xFF);
   TxMsg.Data8[6] = ((code >> 16) & 0xFF);
   TxMsg.Data8[7] = ((code >> 24) & 0xFF);

   return( CAN_UserWrite(CAN_BUS1, &TxMsg) );
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
static void updateLED(void)
{
   if (SYSTIME_DIFF (ledTimeCounter, SYSTIME_NOW) >= (u32_t)(1000))
   {
      ledTimeCounter = SYSTIME_NOW ;
      ledFlashCount++;

      if ( (ledFlashCount > ledFlashLimit) )
      {
         ledFlashStep++;

         if ( (LED_PREOPERATIONAL_OK_S == ledState) )
         {
            if ( (1 == ledFlashStep) )
            {
               ledFlashLimit = 500;               
               HW_SetLED(HW_LED_CAN1, HW_LED_OFF);
            }
            else 
            {
               ledFlashStep = 0;
               ledFlashLimit = 250;               
               ledFlashCount = 0;
               HW_SetLED(HW_LED_CAN1, HW_LED_GREEN);
            }
         }
         else if ( (LED_PREOPERATIONAL_ERROR_S == ledState) )
         {
            if ( (1 == ledFlashStep) )
            {
               ledFlashLimit = 1000;               
               HW_SetLED(HW_LED_CAN1, HW_LED_OFF);
            }
            else 
            {
               ledFlashStep = 0;
               ledFlashLimit = 250;               
               ledFlashCount = 0;
               HW_SetLED(HW_LED_CAN1, HW_LED_RED);
            }
         }
         else if ( (LED_STOPPED_S == ledState) )
         {
            if ( (1 == ledFlashStep) )
            {
               ledFlashLimit = 200;               
               HW_SetLED(HW_LED_CAN1, HW_LED_OFF);
            }
            else if ( (2 == ledFlashStep) )
            {
               ledFlashLimit = 300;               
               HW_SetLED(HW_LED_CAN1, HW_LED_GREEN);
            }
            else if ( (3 == ledFlashStep) )
            {
               ledFlashLimit = 1000;               
               HW_SetLED(HW_LED_CAN1, HW_LED_OFF);
            }
            else 
            {
               ledFlashStep = 0;
               ledFlashLimit = 100;               
               ledFlashCount = 0;
               HW_SetLED(HW_LED_CAN1, HW_LED_GREEN);
            }
         }
      }
   }
}

static void updateHeartbeat(void)
{
   if ( (0 != producerHeartbeatTime) )
   {
      if (SYSTIME_DIFF (heartbeatTimeCounter, SYSTIME_NOW) >= (u32_t)(1000 * producerHeartbeatTime))
      {
         heartbeatTimeCounter = SYSTIME_NOW ;
         u8_t stateValue = (DEVICE_PREOPERATIONAL_S == deviceState) ? 0x7F : ( (DEVICE_OPERATIONAL_S == deviceState) ? 5 : 4);
         sendHeartbeat(stateValue);
      }
   }
}

static void updateTxPdoMap(DEVICE_TPDO_MAP * txPdoMap)
{
   if ( (DEVICE_OPERATIONAL_S == deviceState) )
   {
      if ( (0 != txPdoMap->eventTime) &&
           (SYSTIME_DIFF (txPdoMap->processTimeCount, SYSTIME_NOW) >= (u32_t)(1000 * txPdoMap->eventTime)) )
      {
         txPdoMap->processTimeCount = SYSTIME_NOW ;

         if ((254 == txPdoMap->txType) || (255 == txPdoMap->txType))
         {
            txPdoMap->processNeeded = 1;
         }
      }

      if ( (0 != txPdoMap->processNeeded) && 
           (SYSTIME_DIFF (txPdoMap->inhibitTimeCount, SYSTIME_NOW) >= (u32_t)(100 * txPdoMap->inhibitTime)) )
      {
         sendPdoData(txPdoMap);
         txPdoMap->inhibitTimeCount = SYSTIME_NOW;
         txPdoMap->processNeeded = 0;
      }
   } 
}

static void updateRxPdoMap(DEVICE_RPDO_MAP * rxPdoMap)
{
   if ( (0 != rxPdoMap->processNeeded) )
   {
      u8_t * frame = rxPdoMap->frame;
      u8_t length = rxPdoMap->length;
      u8_t offset = 0;

      for (int i = 0; i < rxPdoMap->mapCount; i++)
      {
         u16_t mapIndex = (u16_t)((rxPdoMap->mappings[i] >> 16) & 0xFFFF);
         u8_t mapSubIndex = (u8_t)((rxPdoMap->mappings[i] >> 8) & 0xFF);
         storeDeviceData(mapIndex, mapSubIndex, frame, offset, length);                     
         offset += pdoMapByteCount(rxPdoMap->mappings[i]);
      }

      rxPdoMap->processNeeded = 0;
   }
}

static void updateSerial(void)
{
   u8_t rxCount;
   u8_t rxOutIndex;
   u8_t activateTpdo;

   do
   {
      u8_t ch;

      rxCount = serialRead(&ch);

      if ( (0 != rxCount) )
      {
         serialRxBuffer[serialRxInIndex] = ch;
         serialRxInIndex = ((serialRxInIndex+1) < SERIAL_RX_BUFFER_SIZE) ? (serialRxInIndex+1) : 0;
         serialRxTimeCounter = SYSTIME_NOW;
      }
   }
   while ( (0 != rxCount) );

   rxCount = 0;
   rxOutIndex = serialRxOutIndex;
   activateTpdo = 0;

   while ( (rxOutIndex != serialRxInIndex) )
   {
      rxCount++;
      rxOutIndex = ((rxOutIndex+1) < SERIAL_RX_BUFFER_SIZE) ? (rxOutIndex+1) : 0;
   }

   if ( (0 != rxCount) &&
        (SYSTIME_DIFF (serialRxTimeCounter, SYSTIME_NOW) >= 20000) )
   {
      activateTpdo = 1;
   }
   else if ( (rxCount >= 8) )
   {
      activateTpdo = 1;
   }
   
   if ( (0 != activateTpdo) )
   {
      for (int i=0; i<4; i++)
      {
         activateTxPdoMap(&txPdoMapping[i], 0x2400, 0);
      }
   }

   while ( (serialTxOutIndex != serialTxInIndex) )
   {
      serialWrite(&serialTxBuffer[serialTxOutIndex], 1);
      serialTxLastIndex = serialTxOutIndex;      
      serialTxOutIndex = ((serialTxOutIndex+1) < SERIAL_TX_BUFFER_SIZE) ? (serialTxOutIndex+1) : 0;
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
      u8_t valid = loadDeviceData(index, subIndex, transferBuffer, &dataLength, sizeof(transferBuffer));

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
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
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
   for (int i=0; i<4 ;i++)
   {
      if ( (txPdoMapping[i].txType >= 1) && (txPdoMapping[i].txType <= 240) )
      {
         txPdoMapping[i].syncCount++;

         if ( (txPdoMapping[i].syncCount >= txPdoMapping[i].txType) )
         {
            txPdoMapping[i].syncCount = 0;
            txPdoMapping[i].processNeeded = 1;
         }
      }

      if ( (rxPdoMapping[i].txType >= 1) && (rxPdoMapping[i].txType <= 240) )
      {
         rxPdoMapping[i].syncCount++;

         if ( (rxPdoMapping[i].syncCount >= rxPdoMapping[i].txType) )
         {
            rxPdoMapping[i].syncCount = 0;
            rxPdoMapping[i].processNeeded = 1;
         }
      }
   }
}

static void processRxPdoMessage(u8_t pdoId, u8_t * frame, u8_t length)
{
   u8_t pdoIndex = pdoId - 1;
   DEVICE_RPDO_MAP * pdoMap = &rxPdoMapping[pdoIndex];

   for (int i=0; i<length ;i++)
   {
      pdoMap->frame[i] = frame[i];
   }

   pdoMap->length = length;

   if ( (254 == pdoMap->txType) || (255 == pdoMap->txType) )
   {
      pdoMap->processNeeded = 1;
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
   ledTimeCounter = SYSTIME_NOW;

   if ( (LED_OPERATIONAL_S == state) )
   {
      HW_SetLED(HW_LED_CAN1, HW_LED_GREEN);
   }
}

static void setPreOperationalState(void)
{
   u32_t busConfiguration;

   HW_Init();
   SYSTIME_Init();

   deviceState = DEVICE_PREOPERATIONAL_S;

   objectBitRateCode = 6;
   objectNodeId = 31;
   objectNodeOffset = 1;

   if ( (EEPROM_Read(0, (u8_t*)&busConfiguration, sizeof(busConfiguration)) == EEPROM_ERR_OK) )
   {
      u8_t check = ((busConfiguration >> 24) & 0xFF);

      if ( (EEPROM_CHECK_VALUE == check) )
      {
         objectBitRateCode = (busConfiguration >> 0) & 0xFF;
         objectNodeId = (busConfiguration >> 8) & 0xFF;
         objectNodeOffset = (busConfiguration >> 16) & 0xFF;
      }
   }

   deviceNodeId = objectNodeId;

   if ( (0 == deviceNodeId) )
   {
      u8_t moduleId;

      HW_GetModuleID(&moduleId);
      deviceNodeId = objectNodeOffset + (moduleId >> 4);
   }

   CAN_UserInit(objectBitRateCode);


   producerHeartbeatTime = 0;

   serialBaud = 9600;
   serialDataBits = 7;
   serialStopBits = 1;
   serialParity = 2;

   serialTxInIndex = 1;
   serialTxOutIndex = 1;
   serialTxLastIndex = 0;

   serialRxInIndex = 1;
   serialRxOutIndex = 1;
   serialRxLastIndex = 0;

   transferActive = 0;

   for (int i=0; i<4; i++)
   {
      resetTxPdoMap(&txPdoMapping[i], i);
      resetRxPdoMap(&rxPdoMapping[i], i);
   }

   txPdoMapping[0].mapCount = 1;
   txPdoMapping[0].mappings[0] = 0x24000040;
   txPdoMapping[0].txType = 0xFE;
   txPdoMapping[0].inhibitTime = 200; // 20.0mS

   rxPdoMapping[0].mapCount = 1;
   rxPdoMapping[0].mappings[0] = 0x23000040;
   rxPdoMapping[0].txType = 0xFE;

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
   u32_t result;

   result = serialInit(serialBaud, serialDataBits, serialStopBits, serialParity);

   if ( (0 == result) )
   {
      deviceState = DEVICE_OPERATIONAL_S;
      setLedState(LED_OPERATIONAL_S);
   }
   else
   {
      setStoppedState();
   }
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
   updateLED();
   updateHeartbeat();

   receiveResult = CAN_UserRead(CAN_BUS1, &message);

   if ( (0 != receiveResult) )
   {
      messageType = CAN_TYPE(message.Id);
      messageDeviceId = CAN_ID(message.Id);

      if ( (0 == messageDeviceId) || (deviceNodeId == messageDeviceId) )
      {
         if ( (CAN_NMT_T ==  messageType) )
         {
            processNmtMessage(message.Data8);
         }
         else if ( (CAN_RSDO_T ==  messageType) )
         {
            processSdoMessage(message.Data8);
         }
      }      
   }
}

static void executeOperationalState(void)
{  
   updateLED();
   updateHeartbeat();
   updateSerial();

   for (int i=0; i<4; i++)
   {
      updateTxPdoMap(&txPdoMapping[i]);
      updateRxPdoMap(&rxPdoMapping[i]);
   }

   receiveResult = CAN_UserRead(CAN_BUS1, &message);

   if ( (0 != receiveResult) )
   {
      messageType = CAN_TYPE(message.Id);
      messageDeviceId = CAN_ID(message.Id);

      if ( (0 == messageDeviceId) || (deviceNodeId == messageDeviceId) )
      {
         if ( (CAN_NMT_T ==  messageType) )
         {
            processNmtMessage(message.Data8);
         }
         else if ( (CAN_SYNC_T == messageType) )
         {
            processSyncMessage();
         }
         else if ( (CAN_RPDO1_T ==  messageType) )
         {
            processRxPdoMessage(1, message.Data8, message.Len);
         }
         else if ( (CAN_RPDO2_T ==  messageType) )
         {
            processRxPdoMessage(2, message.Data8, message.Len);
         }
         else if ( (CAN_RPDO3_T ==  messageType) )
         {
            processRxPdoMessage(3, message.Data8, message.Len);
         }
         else if ( (CAN_RPDO4_T ==  messageType) )
         {
            processRxPdoMessage(4, message.Data8, message.Len);
         }
         else if ( (CAN_RSDO_T ==  messageType) )
         {
            processSdoMessage(message.Data8);
         }
      }      
   }
}

static void executeStoppedState(void)
{
   updateLED();
   updateHeartbeat();

   receiveResult = CAN_UserRead(CAN_BUS1, &message);

   if ( (0 != receiveResult) )
   {
      messageType = CAN_TYPE(message.Id);
      messageDeviceId = CAN_ID(message.Id);

      if ( (0 == messageDeviceId) || (deviceNodeId == messageDeviceId) )
      {
         if ( (CAN_NMT_T ==  messageType) )
         {
            processNmtMessage(message.Data8);
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
int  main ( void)
{
   /* Set Pre-Operational State */
   setPreOperationalState();

   /* Process */
   while(1)    
   {      
      /* Evaluate State */
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
