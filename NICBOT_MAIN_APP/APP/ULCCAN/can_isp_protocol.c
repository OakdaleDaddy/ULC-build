#ifndef USE_MICROCAN_OPEN

//_____ I N C L U D E S ________________________________________________________
#include "string.h"
#include "config.h"
#include "can_isp_protocol.h"
#include "avr\interrupt.h"
#include "isp_lib.h"
#include "reduced_can_lib.h"
#include "can_drv.h"
#include "CommonFunctions.h"
#include "ServoControl.h"
#include "LED_Control.h"
#include "can_callbacks.h"

//_____ D E F I N I T I O N S __________________________________________________

#define CAN_COB(type,id) ((type<<7)|id)

#define CAN_TYPE(cob) ((cob>>7) & 0xF)
#define CAN_ID(cob) (cob & 0x7F)

#define MAXIMUM_HEARTBEAT_DIFF (0x0000FFFFL * 1000)

#define EEPROM_CAN_CONFIGURATION_ADDRESS (256)
#define EEPROM_CAN_CHECK_VALUE (0xA5)

#define REPAIR_MODE (0)
#define INSPECT_MODE (1)

#define DEFAULT_DEVICE_BIT_RATE_CODE (2) // 50K
#define DEFAULT_DEVICE_NODE_ID (0x20)
#define DEFAULT_DEVICE_MODE (REPAIR_MODE) // 0 = repair

#define CAN_MAX_TX_MSGS (8)
#define CAN_MAX_RX_MSGS (16)

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

typedef enum
{
    CAN_NMT_T = 0,
    CAN_SYNC_T = 1,
    CAN_EMGY_T = 1,
    CAN_TS_T = 2,
    CAN_TPDO1_T = 3,
    CAN_RPDO1_T,
    CAN_TPDO2_T,
    CAN_RPDO2_T,
    CAN_TPDO3_T,
    CAN_RPDO3_T,
    CAN_TPDO4_T,
    CAN_RPDO4_T,
    CAN_TSDO_T,
    CAN_RSDO_T,
    CAN_NMTE_T = 14,
}CAN_FRAME_TYPE;

typedef unsigned char u8_t;
typedef char s8_t;
typedef unsigned short u16_t;
typedef short s16_t;
typedef unsigned long u32_t;
typedef long s32_t;

typedef struct  
{
   u16_t cobId;
   u8_t dlc;
   u8_t data[8];  
}CAN_MSG;

typedef struct
{
   u8_t mapCount; // number of mappings
   u32_t mappings[8]; // maps

   u8_t txType; // transmit type: 0..F0=n syncs, FE=event, FF=event
   u32_t cobId; // cob of PDO
   u16_t inhibitTime; // time used for inhibit, 100uS units
   u16_t eventTime; // time used for event, mS units

   u8_t syncCount; // number of received syncs
   u16_t inhibitTimeCount; // used for inhibit time tracking
   u16_t processTimeCount; // used for time tracking
   u8_t initialTriggered; // used to trigger initial change event on start
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

static void setCANTx(void);
static void setCANRx(u8_t mobId, u8_t id, u8_t mask);
static void initCANRate(u8_t bitRateCode);
static u8_t rxCANMsg(CAN_MSG * canMsg);
static u8_t txCANMsg(CAN_MSG * canMsg);
static void setCANLed(int setValue);

static u8_t setServoErrorLimit(u8_t axisNum, u16_t value);
static u8_t setServoProportionalControlConstant(u8_t axisNum, u32_t value);
static u8_t setServoIntegralControlConstant(u8_t axisNum, u32_t value);
static u8_t setServoDerivativeControlConstant(u8_t axisNum, u32_t value);

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
static u32_t loadDeviceData(u8_t signalApplication, u16_t index, u8_t subIndex, u8_t * buffer, u32_t * length, u32_t limit);
static u32_t storeDeviceData(u8_t signalApplication, u16_t index, u8_t subIndex, u8_t * source, u32_t offset, u32_t length);

static u8_t sendDebug(u32_t codeA, u32_t codeB);
static u8_t sendHeartbeat(u8_t value);
static u8_t sendEmergency(u8_t * faultCode);
static u8_t sendPdoData(DEVICE_TPDO_MAP * txPdoMap);
static u8_t sendSdoData(void);
static u8_t sendSdoAbort(u16_t index, u8_t subIndex, u32_t code);

static void updateLED(void);
static void updateConsumerHeartbeat(void);
static void updateProducerHeartbeat(void);
static void updateTxPdoMap(DEVICE_TPDO_MAP * txPdoMap);
static void updateRxPdoMap(DEVICE_RPDO_MAP * rxPdoMap);

static void initiateSdoDownload(u8_t * frame);
static void processSdoDownload(u8_t * frame);
static void initiateSdoUpload(u8_t * frame);
static void processSdoUpload(u8_t * frame);
static void abortSdoTransfer(u8_t * frame);

static void processNmtMessage(u8_t * frame);
static void processSyncMessage(void);
static void processRxPdoMessage(u8_t pdoId, u8_t * frame, u8_t length);
static void processSdoMessage(u8_t * frame);
static void processNmteMessage(u8_t messageDeviceId);

static void setLedState(LED_STATE state);
static void setDeviceFault(const u8_t * faultCode);
static void setConsumerHeartbeatTime(u32_t value);
static void setPreOperationalState(void);
static void setOperationalState(void);
static void setStoppedState(void);

static void executePreOperationalState(void);
static void executeOperationalState(void);
static void executeStoppedState(void);


//_____ D E C L A R A T I O N S ________________________________________________

static volatile u32_t canIsrCount;
static volatile u8_t tovf_f;

static u8_t canTxInIndex;
static volatile u8_t canTxOutIndex;
static volatile u8_t canLastTxOutIndex;
static volatile u8_t canTxActive;
static CAN_MSG canTxBuffer[CAN_MAX_TX_MSGS];

static u8_t canRxInIndex;
static u8_t canRxOutIndex;
static CAN_MSG canRxBuffer[CAN_MAX_RX_MSGS];

static DEVICE_STATE deviceState;
static u8_t deviceNodeId;
static u8_t deviceMode;

static LED_STATE ledState;
static u16_t ledFlashCount;
static u16_t ledFlashLimit;
static u16_t ledFlashStep;
static u16_t ledTimeCounter;

static CAN_MSG message;
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

static u32_t deviceType = 0x00003010;
static char * deviceName = "NICBOT Body";
static char * version = "v1.01 ";// __DATE__ " " __TIME__;

static u32_t errorStatus = 0;
static u8_t * deviceFaultCode;

static u32_t sdoConsumerHeartbeat;
static u8_t consumerHeartbeatNode;
static u16_t consumerHeartbeatTime;
static u8_t consumerHeartbeatActive;
static u16_t consumerHeartbeatTimeLimit;

static u16_t producerHeartbeatTime;
static u16_t heartbeatTimeLimit;

static DEVICE_TPDO_MAP txPdoMapping[4];
static DEVICE_RPDO_MAP rxPdoMapping[4];

static u8_t assignedBaudrateCode = DEFAULT_DEVICE_BIT_RATE_CODE;
static u8_t assignedNodeId = DEFAULT_DEVICE_NODE_ID;

static u8_t canOd2100 = DEFAULT_DEVICE_BIT_RATE_CODE;
static u8_t canOd2101 = DEFAULT_DEVICE_NODE_ID;
static u8_t canOd2102 = REPAIR_MODE;

static u8_t canOd230101;
static u8_t canOd230102;
static u8_t canOd2303[12];

static u16_t canOd2304;

static s16_t canOd2311;
static s16_t canOd2312;
static s16_t canOd2313;
static s16_t canOd2314;

static u8_t autoDrillControl;
static u16_t indexerSearchSpeed;
static u16_t indexerTravelSpeed;
static s16_t drillRotationSpeed;
static u16_t indexerCuttingSpeed;
static u16_t indexerCuttingDepth;
static u16_t indexerPeckCuttingIncrement;
static u16_t indexerPeckRetractionDistance;
static u16_t indexerPeckRetractionPosition;

static s16_t canOd2411;
static u16_t canOd2412;
static s16_t canOd2413;
static u16_t canOd2414;

static u32_t drillServoProportionalControlConstant;
static u32_t drillServoIntegralControlConstant;
static u32_t drillServoDerivativeControlConstant;
static u32_t drillServoAcceleration;
static u32_t drillServoHomingVelocity;
static u32_t drillServoHomingBackoffCount;
static u32_t drillServoTravelVelocity;
static u16_t drillServoErrorLimit;
static u32_t drillServoPulsesPerUnit;

static u32_t sensorServoAcceleration;
static u32_t sensorServoHomingVelocity;
static u32_t sensorServoHomingBackoffCount;
static u32_t sensorServoTravelVelocity;
static u16_t sensorServoErrorLimit;
static s32_t sensorServoPulsesPerDegree;

static u16_t canOd2441;
static u16_t canOd2442;
static u16_t canOd2443;

static u16_t deviceControl;
static u8_t deviceControl0CCache;
static u16_t deviceStatus;

static s16_t processedFrontDrillSpeedSetPoint;
//static u16_t processedFrontDrillIndexSetPoint;
static s16_t processedRearDrillSpeedSetPoint;
//static u16_t processedRearDrillIndexSetPoint;
static u32_t processedSensorIndexSetPoint;
static u16_t processedDeviceControl;

static const u8_t consumerHeartbeatLostFaultCode[8] = { 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
static const u8_t rpdoFailureLostFaultCode[8] = { 0x00, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
static const u8_t servoExcessivePositionFaultCode[8] = { 0x00, 0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
ISR(OVRIT_vect)
{
   tovf_f = 1;
   canIsrCount += 0x00010000; 
}
 
ISR(CANIT_vect)
{
   CAN_MSG * nextMsg;
   u16_t irqStatus;
   
   irqStatus = CANSIT;
   
   if ( (irqStatus & 0x0001) )
   {     
      Can_set_mob(MOB_0);
      CANSTMOB = CANSTMOB & ~(0x20);

      nextMsg = &canRxBuffer[canRxInIndex];

      nextMsg->dlc = Can_get_dlc();
      can_get_data(nextMsg->data);
      Can_get_std_id(nextMsg->cobId);

      canRxInIndex++;
      
      if ( (CAN_MAX_RX_MSGS == canRxInIndex) )
      {
         canRxInIndex = 0;
      }
      
      Can_config_rx();  
   }

   if ( (irqStatus & 0x4000) )
   {
      Can_set_mob(MOB_14);
      CANSTMOB = CANSTMOB & ~(0x40);
      setCANTx();
   }
}

static void setCANTx(void)
{
    CAN_MSG * nextMsg;
   u8_t i;

   if ( (canTxOutIndex != canTxInIndex) )
   {
        nextMsg = &canTxBuffer[canTxOutIndex];

       canLastTxOutIndex = canTxOutIndex;
       canTxOutIndex++;
       
       if ( (CAN_MAX_TX_MSGS == canTxOutIndex) )
       {
          canTxOutIndex = 0;
       }

        Can_set_mob(MOB_14);
        Can_mob_abort();
        Can_clear_ide();
        Can_clear_mob();
        Can_set_std_id(nextMsg->cobId);

        for (i=0; i<nextMsg->dlc; i++)
        {
            CANMSG = nextMsg->data[i];
        }
        
        Can_clear_rtr();
        Can_set_dlc(nextMsg->dlc);
        Can_config_tx();
   }
    else
   {
      canTxActive = 0;
   }           
}

__attribute__((optimize("O0")))
static void setCANRx(u8_t mobId, u8_t id, u8_t mask)
{
   Can_set_mob(mobId);
   Can_clear_mob();
   Can_clear_ide();
   Can_set_std_id(id);
   Can_set_std_msk(mask);
   Can_set_rtrmsk();
   Can_clear_rtr();
   Can_set_idemsk();
   Can_config_rx();
}

static void initCANRate(u8_t bitRateCode)
{
   Can_reset();
    
   get_conf_byte(EB);
        
   // {0=10K, 1=20K, 2=50K, 3=100K, 4=125K, 5=250K, 6=500K, 7=1M} todo
   if ( (4 == bitRateCode) )
   {
      // 125K
      CANBT1 = 0x0E;
      CANBT2 = 0x0C;
      CANBT3 = 0x37;
   }
   else 
   {
      // 50K
      CANBT1 = 0x26;
      CANBT2 = 0x04;
      CANBT3 = 0x3E;
   }
        
   can_clear_all_mob();
   Can_enable();

   canTxInIndex = 1;
   canTxOutIndex = 1;
   canLastTxOutIndex = 0;
   canTxActive = 0;

   canRxInIndex = 0;
   canRxOutIndex = 0;

   CANTCON = 0xF9; // set for 8KHz
    
   CANIE2 |= 0x01; // enable interrupt for MOB0
   CANIE1 |= 0x40; // enable interrupt for MOB14
   CANGIE |= 0xB1; // enable all interrupts, enable receive interrupt, enable transmit interrupt, enable timer overflow interrupt
   setCANRx(MOB_0, 0, 0);
}

// reads CAN buffer, return 1 on load of pointer, return 0 on no load
static u8_t rxCANMsg(CAN_MSG * canMsg)
{
   u8_t result = 0;
   
   if ( (canRxOutIndex != canRxInIndex) )
   {
      CAN_MSG * nextMsg;
      u8_t i;
      
      nextMsg = &canRxBuffer[canRxOutIndex];
      
      canMsg->cobId = nextMsg->cobId;
      canMsg->dlc = nextMsg->dlc;
      
      for (i=0; i<canMsg->dlc; i++)
      {
         canMsg->data[i] = nextMsg->data[i];
      }
      
      canRxOutIndex++;
      
      if ( (CAN_MAX_RX_MSGS == canRxOutIndex) )
      {
         canRxOutIndex = 0;
      }

      result = 1;
   }
   
   return(result);
}

static u8_t txCANMsg(CAN_MSG * canMsg)
{
   CAN_MSG * nextMsg;
   u8_t result;
   
   result = 0;
   
   if ( (canTxInIndex != canLastTxOutIndex) )
   {     
      nextMsg = &canTxBuffer[canTxInIndex];
      memcpy(nextMsg, canMsg, sizeof(CAN_MSG));
      
      canTxInIndex++;
      
      if ( (CAN_MAX_TX_MSGS == canTxInIndex) )
      {
         canTxInIndex = 0;
      }

      if ( (0 == canTxActive) )
      {
         canTxActive = 1;
         cli();
         setCANTx();
         sei();
      }
      
      result = 1;
   }

   return(result);
}

// set LED, 1 is on, 0 is off
static void setCANLed(int setValue)
{
   PORTC &= 0xfe;
   PORTC |= setValue;
}

// service watchdog timer
static void serveCOP(void)
{
   asm( "WDR" ); // Watchdog reset
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
static u8_t setServoErrorLimit(u8_t axisNum, u16_t value)
{
   u8_t result;
   
   result = servo_set_error(axisNum, value);
   
   if (0 == result)
   {
      result = 1;
   }
   else
   {
      result = 0;
   }
   
   return(result);
}

static u8_t setServoProportionalControlConstant(u8_t axisNum, u32_t value)
{
   u8_t result;
   u8_t byte0;
   u8_t byte1;
   
   byte0 = (u8_t)(value & 0xFF);
   byte1 = (u8_t)((value >> 8) & 0xFF);
   
   result = set_kp(axisNum, byte1, byte0);
   (void)result;
   
   #if 0
   if (0 == result)
   {
      result = 1;
   }
   else
   {
      result = 0;
   }
   
   return(result);
   #endif
   return(1);
}

static u8_t setServoIntegralControlConstant(u8_t axisNum, u32_t value)
{
   u8_t result;
   u8_t byte0;
   u8_t byte1;
   
   byte0 = (u8_t)(value & 0xFF);
   byte1 = (u8_t)((value >> 8) & 0xFF);
   
   result = set_ki(axisNum, byte1, byte0);
   (void)result;
   
   #if 0
   if (0 == result)
   {
      result = 1;
   }
   else
   {
      result = 0;
   }
   
   return(result);
   #endif
   return(1);
}

static u8_t setServoDerivativeControlConstant(u8_t axisNum, u32_t value)
{
   u8_t result;
   u8_t byte0;
   u8_t byte1;
   
   byte0 = (u8_t)(value & 0xFF);
   byte1 = (u8_t)((value >> 8) & 0xFF);
   
   result = set_kd(axisNum, byte1, byte0);
   (void)result;
   
   #if 0
   if (0 == result)
   {
      result = 1;
   }
   else
   {
      result = 0;
   }
   
   return(result);
   #endif
   return(1);
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


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

// all mapping objects must specify size in bytes here
static u8_t pdoByteSize(u16_t index, u8_t subIndex)
{
    u8_t result = 0;

    if ( (REPAIR_MODE == deviceMode) )
    {
        if ( ((index >= 0x2411) && (index <= 0x2414)) ||
             ((index >= 0x2441) && (index <= 0x2443)) ||
             (0x2501 == index) )
        {
            result = 2;
        }
    }
    else if ( (INSPECT_MODE == deviceMode) )
    {
        if ( (0x2415 == index) ||
             ((index >= 0x2441) && (index <= 0x2443)) ||
             (0x2501 == index) )
        {
            result = 2;
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
static u8_t pdoTxMappable(u16_t index, u8_t subIndex)
{
    u8_t result = 0;

    if ( (REPAIR_MODE == deviceMode) )
    {
        if ( ((index >= 0x2411) && (index <= 0x2414)) ||
             ((index >= 0x2441) && (index <= 0x2443)) ||
             (0x2501 == index) )
        {
            result = 1;
        }
    }
    else if ( (INSPECT_MODE == deviceMode) )
    {
        if ( (0x2415 == index) ||
             ((index >= 0x2441) && (index <= 0x2443)) ||
             (0x2501 == index) )
        {
            result = 1;
        }
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
                    u8_t i;
                    u8_t mapSize = pdoMapByteCount(value);
                    u8_t pdoSize = pdoByteSize(mapIndex, mapSubIndex);

                    if ( (pdoSize == mapSize) )
                    {
                        for (i = 1; i < subIndex; i++)
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
        u8_t i;
        
        for (i=0; i<txPdoMap->mapCount; i++)
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
    CAN_FRAME_TYPE pdoType;

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
    txPdoMap->inhibitTimeCount = 0;
    txPdoMap->processTimeCount = 0;
    txPdoMap->initialTriggered = 0;
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
                    u8_t i;
                    u8_t mapSize = pdoMapByteCount(value);
                    u8_t pdoSize = pdoByteSize(mapIndex, mapSubIndex);

                    if ( (pdoSize == mapSize) )
                    {
                        for (i = 1; i < subIndex; i++)
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
    CAN_FRAME_TYPE pdoType;

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

    return(result);
}

static u32_t loadDeviceData(u8_t signalApplication, u16_t index, u8_t subIndex, u8_t * buffer, u32_t * length, u32_t limit)
{
   u32_t result = 0;
   u32_t size;
   u8_t * source = 0;
   u32_t transferred = 0;

   if ( (0 != signalApplication) )
   {
      result = CAN_ODRead(index, subIndex);
   }

   if ( (0 == result) )
   {
      if (0x1000 == index)
      {
         size = sizeof(deviceType);
         source = (u8_t *)&deviceType;
      }
      else if (0x1001 == index)
      {
         size = sizeof(errorStatus);
         source = (u8_t *)&errorStatus;
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
      else if ((0x1016 == index) && (0 == subIndex))
      {
         buffer[0] = 1;
         transferred = 1;
      }
      else if ((0x1016 == index) && (1 == subIndex))
      {
         size = sizeof(sdoConsumerHeartbeat);
         source = (u8_t *)&sdoConsumerHeartbeat;
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
         u32_t serialNumber = CAN_GetSerial();
         memcpy(buffer, &serialNumber, 4);
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
         u8_t mappingOffset = (index - 0x1A00);
         transferred = 0;
         loadTxPdoMapData(&txPdoMapping[mappingOffset], subIndex, buffer, &transferred);
      }
      else if (0x2100 == index)
      {
         size = sizeof(canOd2100);
         source = (u8_t *)&canOd2100;
      }
      else if (0x2101 == index)
      {
         size = sizeof(canOd2101);
         source = (u8_t *)&canOd2101;
      }
      else if (0x2102 == index)
      {
         size = sizeof(canOd2102);
         source = (u8_t *)&canOd2102;
      }
      else if (0x2105 == index)
      {
         // write only field, do nothing
      }
      else if ((0x2301 == index) && (0 == subIndex))
      {
         buffer[0] = 1;
         transferred = 1;
      }
      else if ((0x2301 == index) && (1 == subIndex))
      {
         size = sizeof(canOd230101);
         source = (u8_t *)&canOd230101;
      }
      else if ((0x2301 == index) && (2 == subIndex))
      {
         size = sizeof(canOd230102);
         source = (u8_t *)&canOd230102;
      }
      else if ((0x2303 == index) && (0 == subIndex))
      {
         buffer[0] = 12;
         transferred = 1;
      }
      else if ((0x2303 == index) && (1 <= subIndex) && (12 >= subIndex))
      {
         buffer[0] = canOd2303[subIndex-1];
         transferred = 1;
      }
      else if (0x2304 == index)
      {
         size = sizeof(canOd2304);
         source = (u8_t *)&canOd2304;
      }
      else if (0x2311 == index)
      {
         size = sizeof(canOd2311);
         source = (u8_t *)&canOd2311;
      }
      else if (0x2312 == index)
      {
         size = sizeof(canOd2312);
         source = (u8_t *)&canOd2312;
      }
      else if (0x2313 == index)
      {
         size = sizeof(canOd2313);
         source = (u8_t *)&canOd2313;
      }
      else if (0x2314 == index)
      {
         size = sizeof(canOd2314);
         source = (u8_t *)&canOd2314;
      }
      else if ((0x2331 == index) && (0 == subIndex))
      {
         if (REPAIR_MODE == deviceMode)
         {
            buffer[0] = 9;
            transferred = 1;
         }        
      }
      else if ((0x2331 == index) && (1 == subIndex))
      {
         if (REPAIR_MODE == deviceMode)
         {
            size = sizeof(autoDrillControl);
            source = (u8_t *)&autoDrillControl;
         }
      }
      else if ((0x2331 == index) && (2 == subIndex))
      {
         if (REPAIR_MODE == deviceMode)
         {
            size = sizeof(indexerSearchSpeed);
            source = (u8_t *)&indexerSearchSpeed;
         }
      }
      else if ((0x2331 == index) && (3 == subIndex))
      {
         if (REPAIR_MODE == deviceMode)
         {
            size = sizeof(indexerTravelSpeed);
            source = (u8_t *)&indexerTravelSpeed;
         }
      }
      else if ((0x2331 == index) && (4 == subIndex))
      {
         if (REPAIR_MODE == deviceMode)
         {
            size = sizeof(drillRotationSpeed);
            source = (u8_t *)&drillRotationSpeed;
         }
      }
      else if ((0x2331 == index) && (5 == subIndex))
      {
         if (REPAIR_MODE == deviceMode)
         {   
            size = sizeof(indexerCuttingSpeed);
            source = (u8_t *)&indexerCuttingSpeed;
         }
      }
      else if ((0x2331 == index) && (6 == subIndex))
      {
         if (REPAIR_MODE == deviceMode)
         {
            size = sizeof(indexerCuttingDepth);
            source = (u8_t *)&indexerCuttingDepth;
         }
      }
      else if ((0x2331 == index) && (7 == subIndex))
      {
         if (REPAIR_MODE == deviceMode)
         {
            size = sizeof(indexerPeckCuttingIncrement);
            source = (u8_t *)&indexerPeckCuttingIncrement;
         }
      }
      else if ((0x2331 == index) && (8 == subIndex))
      {
         if (REPAIR_MODE == deviceMode)
         {
            size = sizeof(indexerPeckRetractionDistance);
            source = (u8_t *)&indexerPeckRetractionDistance;
         }
      }
      else if ((0x2331 == index) && (9 == subIndex))
      {
         if (REPAIR_MODE == deviceMode)
         {
            size = sizeof(indexerPeckRetractionPosition);
            source = (u8_t *)&indexerPeckRetractionPosition;
         }
      }     
      else if (0x233D == index)
      {
         if (REPAIR_MODE == deviceMode)
         {
            size = sizeof(drillServoProportionalControlConstant);
            source = (u8_t *)&drillServoProportionalControlConstant;
         }
      }
      else if (0x233E == index)
      {
         if (REPAIR_MODE == deviceMode)
         {
            size = sizeof(drillServoIntegralControlConstant);
            source = (u8_t *)&drillServoIntegralControlConstant;
         }
      }
      else if (0x233F == index)
      {
         if (REPAIR_MODE == deviceMode)
         {
            size = sizeof(drillServoDerivativeControlConstant);
            source = (u8_t *)&drillServoDerivativeControlConstant;
         }
      }
      else if (0x2340 == index)
      {
         if (REPAIR_MODE == deviceMode)
         {
            size = sizeof(drillServoAcceleration);
            source = (u8_t *)&drillServoAcceleration;
         }
      }
      else if (0x2341 == index)
      {
         if (REPAIR_MODE == deviceMode)
         {
            size = sizeof(drillServoHomingVelocity);
            source = (u8_t *)&drillServoHomingVelocity;
         }
      }
      else if (0x2342 == index)
      {
         if (REPAIR_MODE == deviceMode)
         {
            size = sizeof(drillServoHomingBackoffCount);
            source = (u8_t *)&drillServoHomingBackoffCount;
         }
      }
      else if (0x2343 == index)
      {
         if (REPAIR_MODE == deviceMode)
         {
            size = sizeof(drillServoTravelVelocity);
            source = (u8_t *)&drillServoTravelVelocity;
         }
      }
      else if (0x2344 == index)
      {
         if (REPAIR_MODE == deviceMode)
         {
            size = sizeof(drillServoErrorLimit);
            source = (u8_t *)&drillServoErrorLimit;
         }
      }
      else if (0x2345 == index)
      {
         if (REPAIR_MODE == deviceMode)
         {
            size = sizeof(drillServoPulsesPerUnit);
            source = (u8_t *)&drillServoPulsesPerUnit;
         }
      }
      else if (0x2346 == index)
      {
         if (REPAIR_MODE == deviceMode)
         {
            buffer[0] = servo_get_status(0);
            transferred = 1;
         }
      }
      else if (0x2347 == index)
      {
         if (REPAIR_MODE == deviceMode)
         {
            buffer[0] = servo_get_status(1);
            transferred = 1;
         }
      }
      else if (0x2350 == index)
      {
         if (INSPECT_MODE == deviceMode)
         {
            size = sizeof(sensorServoAcceleration);
            source = (u8_t *)&sensorServoAcceleration;
         }
      }
      else if (0x2351 == index)
      {
         if (INSPECT_MODE == deviceMode)
         {
            size = sizeof(sensorServoHomingVelocity);
            source = (u8_t *)&sensorServoHomingVelocity;
         }
      }
      else if (0x2352 == index)
      {
         if (INSPECT_MODE == deviceMode)
         {
            size = sizeof(sensorServoHomingBackoffCount);
            source = (u8_t *)&sensorServoHomingBackoffCount;
         }
      }
      else if (0x2353 == index)
      {
         if (INSPECT_MODE == deviceMode)
         {
            buffer[0] = servo_get_status(0);
            transferred = 1;
         }
      }
      else if (0x2354 == index)
      {
         if (INSPECT_MODE == deviceMode)
         {
            size = sizeof(sensorServoErrorLimit);
            source = (u8_t *)&sensorServoErrorLimit;
         }
      }
      else if (0x2355 == index)
      {
         if (INSPECT_MODE == deviceMode)
         {
            size = sizeof(sensorServoTravelVelocity);
            source = (u8_t *)&sensorServoTravelVelocity;
         }
      }
      else if (0x2356 == index)
      {
         if (INSPECT_MODE == deviceMode)
         {
            size = sizeof(sensorServoPulsesPerDegree);
            source = (u8_t *)&sensorServoPulsesPerDegree;
         }
      }
      else if (0x2411 == index)
      {
         size = sizeof(canOd2411);
         source = (u8_t *)&canOd2411;
      }
      else if (0x2412 == index)
      {
         size = sizeof(canOd2412);
         source = (u8_t *)&canOd2412;
      }
      else if (0x2413 == index)
      {
         size = sizeof(canOd2413);
         source = (u8_t *)&canOd2413;
      }
      else if (0x2414 == index)
      {
         size = sizeof(canOd2414);
         source = (u8_t *)&canOd2414;
      }
      else if (0x2441 == index)
      {
         size = sizeof(canOd2441);
         source = (u8_t *)&canOd2441;
      }
      else if (0x2442 == index)
      {
         size = sizeof(canOd2442);
         source = (u8_t *)&canOd2442;
      }
      else if (0x2443 == index)
      {
         size = sizeof(canOd2443);
         source = (u8_t *)&canOd2443;
      }
      else if (0x2500 == index)
      {
         size = sizeof(deviceControl);
         source = (u8_t *)&deviceControl;
      }
      else if (0x2501 == index)
      {
         size = sizeof(deviceStatus);
         source = (u8_t *)&deviceStatus;
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
   }

   return( (0 != transferred) ? 0 : 1 );
}

static u32_t storeDeviceData(u8_t signalApplication, u16_t index, u8_t subIndex, u8_t * source, u32_t offset, u32_t length)
{
   u32_t result = 0;
   
   if ( (0 != signalApplication) )
   {
      result = CAN_ODWrite(index, subIndex, &source[offset], length);
   }

   if ( (0 == result) )
   {
      // todo set result here 
      uint8_t checkTxPdo = 0;

      if ((0x1016 == index) && (1 == subIndex))
      {
         if (4 == length)
         {
            u32_t value = (u32_t)getValue(&source[offset], length);
            setConsumerHeartbeatTime(value);
            result = 1;
         }
      }
      else if (0x1017 == index)
      {
         if ( (2 == length) )
         {
            producerHeartbeatTime = (source[offset+1] << 8) | source[offset];
            heartbeatTimeLimit = canGetTime() + producerHeartbeatTime;
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
         u8_t mappingOffset = (index - 0x1A00);
         u32_t data = getValue(&source[offset], length);
         result = storeTxPdoMapData(&txPdoMapping[mappingOffset], subIndex, length, data);
      }
      else if (0x2100 == index)
      {
         canOd2100 = source[offset];
         result = 1;
      }
      else if (0x2101 == index)
      {
         canOd2101 = source[offset];
         result = 1;
      }
      else if (0x2102 == index)
      {
         canOd2102 = source[offset];
         result = 1;
      }
      else if (0x2105 == index)
      {
         result = 1;
      }
      else if ((0x2301 == index) && (1 == subIndex))
      {
         canOd230101 = source[offset];
         result = 1;
      }
      else if ((0x2301 == index) && (2 == subIndex))
      {
         canOd230102 = source[offset];
         result = 1;
      }
      else if ((0x2303 == index) && (1 <= subIndex) && (12 >= subIndex))
      {
         canOd2303[subIndex-1] = source[offset];
         result = 1;
      }
      else if (0x2304 == index)
      {
         canOd2304 = (u16_t)getValue(&source[offset], length);
         result = 1;
      }
      else if (0x2311 == index)
      {
         canOd2311 = (u16_t)getValue(&source[offset], length);
         result = 1;
      }
      else if (0x2312 == index)
      {
         canOd2312 = (u16_t)getValue(&source[offset], length);
         result = 1;
      }
      else if (0x2313 == index)
      {
         canOd2313 = (u16_t)getValue(&source[offset], length);
         result = 1;
      }
      else if (0x2314 == index)
      {
         canOd2314 = (u16_t)getValue(&source[offset], length);
         result = 1;
      }
      else if ((0x2331 == index) && (1 == subIndex))
      {
         if (REPAIR_MODE == deviceMode)
         {
            if (1 == length)
            {
               autoDrillControl = source[offset];
               result = 1;
            }
         }
      }
      else if ((0x2331 == index) && (2 == subIndex))
      {
         if (REPAIR_MODE == deviceMode)
         {
            if (2 == length)
            {
               u16_t value = (u16_t)getValue(&source[offset], length);
               indexerSearchSpeed = value;
               result = 1;
            }
         }
      }
      else if ((0x2331 == index) && (3 == subIndex))
      {
         if (REPAIR_MODE == deviceMode)
         {
            if (2 == length)
            {
               u16_t value = (u16_t)getValue(&source[offset], length);
               indexerTravelSpeed = value;
               result = 1;
            }
         }
      }
      else if ((0x2331 == index) && (4 == subIndex))
      {
         if (REPAIR_MODE == deviceMode)
         {
            if (2 == length)
            {
               s16_t value = (s16_t)getValue(&source[offset], length);
               drillRotationSpeed = value;
               result = 1;
            }
         }
      }
      else if ((0x2331 == index) && (5 == subIndex))
      {
         if (REPAIR_MODE == deviceMode)
         {
            if (2 == length)
            {
               u16_t value = (u16_t)getValue(&source[offset], length);
               indexerCuttingSpeed = value;
               result = 1;
            }
         }
      }
      else if ((0x2331 == index) && (6 == subIndex))
      {
         if (REPAIR_MODE == deviceMode)
         {
            if (2 == length)
            {
               u16_t value = (u16_t)getValue(&source[offset], length);
               indexerCuttingDepth = value;
               result = 1;
            }
         }
      }
      else if ((0x2331 == index) && (7 == subIndex))
      {
         if (REPAIR_MODE == deviceMode)
         {
            if (2 == length)
            {
               u16_t value = (u16_t)getValue(&source[offset], length);
               indexerPeckCuttingIncrement = value;
               result = 1;
            }
         }
      }
      else if ((0x2331 == index) && (8 == subIndex))
      {
         if (REPAIR_MODE == deviceMode)
         {
            if (2 == length)
            {
               u16_t value = (u16_t)getValue(&source[offset], length);
               indexerPeckRetractionDistance = value;
               result = 1;
            }
         }
      }
      else if ((0x2331 == index) && (9 == subIndex))
      {
         if (REPAIR_MODE == deviceMode)
         {
            if (2 == length)
            {
               u16_t value = (u16_t)getValue(&source[offset], length);
               indexerPeckRetractionPosition = value;
               result = 1;
            }
         }
      }  
      else if (0x233D == index)
      {
         if (REPAIR_MODE == deviceMode)
         {
            if (4 == length)
            {
               u32_t value = (u32_t)getValue(&source[offset], length);
                
               if ( (setServoProportionalControlConstant(0, value) != 0) &&
                    (setServoProportionalControlConstant(1, value) != 0) )
               {
                  drillServoProportionalControlConstant = value;
                  result = 1;
               }
            }
         }
      }
      else if (0x233E == index)
      {
         if (REPAIR_MODE == deviceMode)
         {
            if (4 == length)
            {
               u32_t value = (u32_t)getValue(&source[offset], length);
                
               if ( (setServoIntegralControlConstant(0, value) != 0) &&
                    (setServoIntegralControlConstant(1, value) != 0) )
               {
                  drillServoIntegralControlConstant = value;
                  result = 1;
               }           
            }
         }
      }
      else if (0x233F == index)
      {
         if (REPAIR_MODE == deviceMode)
         {
            if (4 == length)
            {
               u32_t value = (u32_t)getValue(&source[offset], length);
                
               if ( (setServoDerivativeControlConstant(0, value) != 0) &&
                    (setServoDerivativeControlConstant(1, value) != 0) )
               {
                  drillServoDerivativeControlConstant = value;
                  result = 1;
               }
            }
         }
      }
      else if (0x2340 == index)
      {
         if (REPAIR_MODE == deviceMode)
         {
            if (4 == length)
            {
               drillServoAcceleration = (u32_t)getValue(&source[offset], length);
               result = 1;
            }
         }
      }
      else if (0x2341 == index)
      {
         if (REPAIR_MODE == deviceMode)
         {
            if (4 == length)
            {
               drillServoHomingVelocity = (u32_t)getValue(&source[offset], length);
               result = 1;
            }
         }
      }
      else if (0x2342 == index)
      {
         if (REPAIR_MODE == deviceMode)
         {
            if (4 == length)
            {
               drillServoHomingBackoffCount = (u32_t)getValue(&source[offset], length);
               result = 1;
            }
         }
      }
      else if (0x2343 == index)
      {
         if (REPAIR_MODE == deviceMode)
         {
            if (4 == length)
            {
               drillServoTravelVelocity = (u32_t)getValue(&source[offset], length);
               result = 1;
            }
         }
      }
      else if (0x2344 == index)
      {
         if (REPAIR_MODE == deviceMode)
         {
            if (2 == length)
            {
               u16_t value = (u32_t)getValue(&source[offset], length);
                
               if ( (setServoErrorLimit(0, value) != 0) &&
                    (setServoErrorLimit(1, value) != 0) )
               {
                  drillServoErrorLimit = value;
                  result = 1;                         
               }
            }
         }
      }
      else if (0x2345 == index)
      {
         if (REPAIR_MODE == deviceMode)
         {
            if (4 == length)
            {
               drillServoPulsesPerUnit = (u32_t)getValue(&source[offset], length);
               result = 1;
            }
         }
      }
      else if (0x2350 == index)
      {
         if (INSPECT_MODE == deviceMode)
         {
            if (4 == length)
            {
               sensorServoAcceleration = (u32_t)getValue(&source[offset], length);
               result = 1;
            }
         }
      }
      else if (0x2351 == index)
      {
         if (INSPECT_MODE == deviceMode)
         {
            if (4 == length)
            {
               sensorServoHomingVelocity = (u32_t)getValue(&source[offset], length);
               result = 1;
            }
         }
      }
      else if (0x2352 == index)
      {
         if (INSPECT_MODE == deviceMode)
         {
            if (4 == length)
            {
               sensorServoHomingBackoffCount = (u32_t)getValue(&source[offset], length);
               result = 1;
            }
         }
      }
      else if (0x2354 == index)
      {
         if (INSPECT_MODE == deviceMode)
         {
            if (2 == length)
            {
               u16_t value = (u16_t)getValue(&source[offset], length);
                
               if ( (setServoErrorLimit(0, value) != 0) )
               {
                  sensorServoErrorLimit = value;
                  result = 1;
               }            
            }
         }
      }
      else if (0x2355 == index)
      {
         if (INSPECT_MODE == deviceMode)
         {
            if (4 == length)
            { 
               sensorServoTravelVelocity = (u32_t)getValue(&source[offset], length);
               result = 1;
            }
         }
      }  
      else if (0x2356 == index)
      {
         if (INSPECT_MODE == deviceMode)
         {
            if (4 == length)
            {
               sensorServoPulsesPerDegree = (u32_t)getValue(&source[offset], length);
               result = 1;
            }
         }
      }  
      else if (0x2411 == index)
      {
         s16_t value = (u16_t)getValue(&source[offset], length);

         if ( (value != canOd2411) )
         {
            canOd2411 = value;
            checkTxPdo = 1;
         }
      }
      else if (0x2412 == index)
      {
         u16_t value = (u16_t)getValue(&source[offset], length);

         if ( (value != canOd2412) )
         {
            canOd2412 = value;
            checkTxPdo = 1;
         }
      }
      else if (0x2413 == index)
      {
         s16_t value = (u16_t)getValue(&source[offset], length);

         if ( (value != canOd2413) )
         {
            canOd2413 = value;
            checkTxPdo = 1;
         }
      }
      else if (0x2414 == index)
      {
         u16_t value = (u16_t)getValue(&source[offset], length);

         if ( (value != canOd2414) )
         {
            canOd2414 = value;
            checkTxPdo = 1;
         }
      }
      else if (0x2441 == index)
      {
         u16_t value = (u16_t)getValue(&source[offset], length);

         if ( (value != canOd2441) )
         {
            canOd2441 = value;
            checkTxPdo = 1;
         }
      }
      else if (0x2442 == index)
      {
         u16_t value = (u16_t)getValue(&source[offset], length);

         if ( (value != canOd2442) )
         {
            canOd2442 = value;
            checkTxPdo = 1;
         }
      }
      else if (0x2443 == index)
      {
         u16_t value = (u16_t)getValue(&source[offset], length);

         if ( (value != canOd2443) )
         {
            canOd2443 = value;
            checkTxPdo = 1;
         }
      }
      else if (0x2500 == index)
      {
         deviceControl = (u16_t)getValue(&source[offset], length);
      }
      else if (0x2501 == index)
      {
         u16_t value = (u16_t)getValue(&source[offset], length);

         if ( (value != deviceStatus) )
         {
            deviceStatus = value;
            checkTxPdo = 1;
         }
      }

      if ( (0 != checkTxPdo) &&
           (DEVICE_OPERATIONAL_S == deviceState) )
      {
         u8_t i;
      
         for (i=0; i<4; i++)
         {
            activateTxPdoMap(&txPdoMapping[i], index, subIndex);
         }
      }
   }

   if ( (0 != signalApplication) && (0 != result) )
   {
      CAN_ODData(index, subIndex, &source[offset], length);
   }

   return(result);
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
static u8_t sendDebug(u32_t codeA, u32_t codeB)
{
   CAN_MSG txMsg;
   u8_t result;
   
   txMsg.cobId = CAN_COB(CAN_NMTE_T, deviceNodeId);
   txMsg.dlc = 8;
   
   txMsg.data[0] = ((codeA >> 0) & 0xFF);
   txMsg.data[1] = ((codeA >> 8) & 0xFF);
   txMsg.data[2] = ((codeA >> 16) & 0xFF);
   txMsg.data[3] = ((codeA >> 24) & 0xFF);
   txMsg.data[4] = ((codeB >> 0) & 0xFF);
   txMsg.data[5] = ((codeB >> 8) & 0xFF);
   txMsg.data[6] = ((codeB >> 16) & 0xFF);
   txMsg.data[7] = ((codeB >> 24) & 0xFF);

   result = txCANMsg(&txMsg); 
   return(result);
}

static u8_t sendHeartbeat(u8_t value)
{
   CAN_MSG txMsg;
   u8_t result;

   txMsg.cobId = CAN_COB(CAN_NMTE_T, deviceNodeId);
   txMsg.dlc = 1;
   txMsg.data[0] = value;
                
   result = txCANMsg(&txMsg);
   return(result);
}

static u8_t sendEmergency(u8_t * faultCode)
{
   CAN_MSG txMsg;
   u8_t result;
   u8_t i;
   
   txMsg.cobId  = CAN_COB(CAN_EMGY_T, deviceNodeId);
   txMsg.dlc = 8;
   
   for (i=0; i<8; i++)
   {
      txMsg.data[i] = faultCode[i];
   }
   
   result = txCANMsg(&txMsg);
   return(result);
}

static u8_t sendPdoData(DEVICE_TPDO_MAP * txPdoMap)
{
   CAN_MSG txMsg;
   u8_t result;
   u8_t i;
    u8_t offset = 0;

    txMsg.cobId = txPdoMap->cobId;

    for (i = 0; i < txPdoMap->mapCount; i++)
    {
        u16_t mapIndex = (u16_t)((txPdoMap->mappings[i] >> 16) & 0xFFFF);
        u8_t mapSubIndex = (u8_t)((txPdoMap->mappings[i] >> 8) & 0xFF);
        u8_t limit = 8 - offset;
        u32_t length = 0;
        loadDeviceData(0, mapIndex, mapSubIndex, &txMsg.data[offset], &length, limit);
        offset += length;
    }

    txMsg.dlc = offset;

   result = txCANMsg(&txMsg);
   return(result);
}

static u8_t sendSdoData(void)
{
   CAN_MSG txMsg;
   u8_t result;

    txMsg.cobId = CAN_COB(CAN_TSDO_T, deviceNodeId);
    txMsg.dlc = 8;

    if ( (0 != transferUpload) )
    {
        if ( (0 == transferStarted) )
        {
            if ( (transferLength <= 4) )
            {
                // send data

                u8_t i;

                txMsg.data[0] = ((2 << 5) | ((4 - transferLength) << 2) | 3);
                txMsg.data[1] = ((transferIndex >> 0) & 0xFF);
                txMsg.data[2] = ((transferIndex >> 8) & 0xFF);
                txMsg.data[3] = transferSubIndex;

                for (i = 0; i < transferLength; i++)
                {
                    if ( (i < transferLength) )
                    {
                        txMsg.data[4 + i] = transferBuffer[i];
                    }
                    else
                    {
                        txMsg.data[4 + i] = 0x55;
                    }
                }

                transferActive = 0;
            }
            else
            {
                // send length

                txMsg.data[0] = ((2 << 5) | 1); ;
                txMsg.data[1] = ((transferIndex >> 0) & 0xFF);
                txMsg.data[2] = ((transferIndex >> 8) & 0xFF);
                txMsg.data[3] = transferSubIndex;
                txMsg.data[4] = ((transferLength >> 0) & 0xFF);
                txMsg.data[5] = ((transferLength >> 8) & 0xFF);
                txMsg.data[6] = ((transferLength >> 16) & 0xFF);
                txMsg.data[7] = ((transferLength >> 24) & 0xFF);

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

            txMsg.data[0] = ((t << 4) | (n << 1) | c);

            for (i = 0; i < 7; i++)
            {
                u8_t ch = 0;

                if ( (i < remaining) )
                {
                    ch = transferBuffer[transferOffset + i];
                }

                txMsg.data[1 + i] = ch;
            }

            transferLastLength = (7 - n);

            if ( (0 == transferLastLength) )
            {
               uint32_t size = 0;
               CAN_AppSDOReadComplete(0, transferIndex, transferSubIndex, &size);

               if ( (0 != size) )
               { 
                  // block upload with application not support 
                  result = 0;
               }
            }
        }
    }
    else
    {
        if ( (0 == transferStarted) )
        {
            if ( (transferLength <= 4) )
            {
                txMsg.data[0] = 0x60;
                txMsg.data[1] = ((transferIndex >> 0) & 0xFF);
                txMsg.data[2] = ((transferIndex >> 8) & 0xFF);
                txMsg.data[3] = transferSubIndex;
                txMsg.data[4] = 0;
                txMsg.data[5] = 0;
                txMsg.data[6] = 0;
                txMsg.data[7] = 0;

                transferActive = 0;
            }
            else
            {
                txMsg.data[0] = 0x60;
                txMsg.data[1] = ((transferIndex >> 0) & 0xFF);
                txMsg.data[2] = ((transferIndex >> 8) & 0xFF);
                txMsg.data[3] = transferSubIndex;
                txMsg.data[4] = 0;
                txMsg.data[5] = 0;
                txMsg.data[6] = 0;
                txMsg.data[7] = 0;

                transferStarted = 1;
            }
        }
        else
        {
            txMsg.data[0] = (0x20 | ((0 != transferToggle) ? 0x10 : 0));
            txMsg.data[1] = ((transferIndex >> 0) & 0xFF);
            txMsg.data[2] = ((transferIndex >> 8) & 0xFF);
            txMsg.data[3] = transferSubIndex;
            txMsg.data[4] = 0;
            txMsg.data[5] = 0;
            txMsg.data[6] = 0;
            txMsg.data[7] = 0;
        }
    }

   result = txCANMsg(&txMsg);
   return(result);
}

static u8_t sendSdoAbort(u16_t index, u8_t subIndex, u32_t code)
{
   CAN_MSG txMsg;
   u8_t result;

    txMsg.cobId = CAN_COB(CAN_TSDO_T, deviceNodeId);
    txMsg.dlc = 8;
    txMsg.data[0] = (4 << 5);
    txMsg.data[1] = ((index >> 0) & 0xFF);
    txMsg.data[2] = ((index >> 8) & 0xFF);
    txMsg.data[3] = subIndex;
    txMsg.data[4] = ((code >> 0) & 0xFF);
    txMsg.data[5] = ((code >> 8) & 0xFF);
    txMsg.data[6] = ((code >> 16) & 0xFF);
    txMsg.data[7] = ((code >> 24) & 0xFF);

   result = txCANMsg(&txMsg);
   return(result);
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
static void updateLED(void)
{
   if ( (canIsTimeExpired(ledTimeCounter) != 0) )
   {
      ledTimeCounter = canGetTime() + 50;
      ledFlashCount += 50;

      if ( (ledFlashCount >= ledFlashLimit) )
      {
         ledFlashStep++;

         if ( (LED_PREOPERATIONAL_OK_S == ledState) )
         {
            if ( (1 == ledFlashStep) )
            {
               ledFlashLimit = 500;
               setCANLed(0);
            }
            else
            {
               ledFlashStep = 0;
               ledFlashLimit = 250;
               ledFlashCount = 0;
               setCANLed(1);
            }
         }
         else if ( (LED_PREOPERATIONAL_ERROR_S == ledState) )
         {
            if ( (1 == ledFlashStep) )
            {
               ledFlashLimit = 1000;
               setCANLed(0);
            }
            else
            {
               ledFlashStep = 0;
               ledFlashLimit = 250;
               ledFlashCount = 0;
               setCANLed(1);
            }
         }  
         else if ( (LED_STOPPED_S == ledState) )
         {
            if ( (1 == ledFlashStep) )
            {
               ledFlashLimit = 200;
               setCANLed(0);
            }
            else if ( (2 == ledFlashStep) )
            {
               ledFlashLimit = 300;
               setCANLed(1);
            }
            else if ( (3 == ledFlashStep) )
            {
               ledFlashLimit = 1000;
               setCANLed(0);
            }
            else
            {
               ledFlashStep = 0;
               ledFlashLimit = 100;
               ledFlashCount = 0;
               setCANLed(1);
            }
         }
      }
   }
}

static void updateConsumerHeartbeat(void)
{
   if ( (0 != consumerHeartbeatActive) &&
        (0 == deviceFaultCode) )   
   {
      if ( (canIsTimeExpired(consumerHeartbeatTimeLimit) != 0) )
      {
         setDeviceFault(consumerHeartbeatLostFaultCode);       
      }  
   }
}

static void updateProducerHeartbeat(void)
{
   if ( (0 != producerHeartbeatTime) )
   {
      if ( (canIsTimeExpired(heartbeatTimeLimit) != 0) )
      {
         heartbeatTimeLimit = canGetTime() + producerHeartbeatTime;
         u8_t stateValue = (DEVICE_PREOPERATIONAL_S == deviceState) ? 0x7F : ( (DEVICE_OPERATIONAL_S == deviceState) ? 5 : 4);
         sendHeartbeat(stateValue);
      }
   }
}

static void updateTxPdoMap(DEVICE_TPDO_MAP * txPdoMap)
{
    if ( (DEVICE_OPERATIONAL_S == deviceState) )
    {
        if ( (254 == txPdoMap->txType) &&
             (0 == txPdoMap->initialTriggered) )
        {
            txPdoMap->initialTriggered = 1;
            txPdoMap->processNeeded = 1;            
        }
        
        if ( (0 != txPdoMap->eventTime) )
        {
            if ( (canIsTimeExpired(txPdoMap->processTimeCount) != 0) )
            {
                txPdoMap->processTimeCount = canGetTime() + txPdoMap->eventTime;

                // 254 is transmit by change event, 255 is transmit by time event
                if ( (255 == txPdoMap->txType) )
                {
                    txPdoMap->processNeeded = 1;
                }
            }
        }

        if ( (0 != txPdoMap->processNeeded) )
        {
            if ( (canIsTimeExpired(txPdoMap->inhibitTimeCount) != 0) )
            {
                sendPdoData(txPdoMap);
                txPdoMap->inhibitTimeCount = canGetTime() + (txPdoMap->inhibitTime / 10);
                txPdoMap->processNeeded = 0;
            }
        }
    }
}

static void updateRxPdoMap(DEVICE_RPDO_MAP * rxPdoMap)
{
    if ( (DEVICE_OPERATIONAL_S == deviceState) )
    {
        if ( (0 != rxPdoMap->processNeeded) )
        {
            u8_t i;
            u8_t * frame = rxPdoMap->frame;
            u8_t length = rxPdoMap->length;
            u8_t offset = 0;

            for (i = 0; i < rxPdoMap->mapCount; i++)
            {
                u8_t validTransfer = 0;
                u16_t mapIndex = (u16_t)((rxPdoMap->mappings[i] >> 16) & 0xFFFF);
                u8_t mapSubIndex = (u8_t)((rxPdoMap->mappings[i] >> 8) & 0xFF);
                u8_t mapLength = pdoMapByteCount(rxPdoMap->mappings[i]);
                
                if ( (length >= mapLength) )
                {
                    validTransfer = storeDeviceData(1, mapIndex, mapSubIndex, frame, offset, mapLength);
                    offset += pdoMapByteCount(rxPdoMap->mappings[i]);
                    length -= mapLength;
                }
                
                if (0 == validTransfer)
                {
                    setDeviceFault(rpdoFailureLostFaultCode);
                }
            }

            rxPdoMap->length = 0;
            rxPdoMap->processNeeded = 0;
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
            u32_t dataLength = 0;
            dataLength = frame[7];
            dataLength <<= 8;
            dataLength |= frame[6];
            dataLength <<= 8;
            dataLength |= frame[5];
            dataLength <<= 8;
            dataLength |= frame[4];

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
            u8_t valid = storeDeviceData(1, index, subIndex, frame, 4, dataLength);

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

            u8_t valid = storeDeviceData(1, index, subIndex, frame, 4, 4);

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
            u8_t valid = storeDeviceData(1, transferIndex, transferSubIndex, transferBuffer, 0, dataLength);

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
      u32_t totalSize = 0;
      u32_t size = 0;
      u8_t * pData = NULL;
      u8_t appResult = CAN_AppSDOReadInit(0, index, subIndex, &totalSize, &size, &pData);

      if ( (0 == appResult) )
      {
         u32_t dataLength = 0;
         u32_t valid = loadDeviceData(1, index, subIndex, transferBuffer, &dataLength, sizeof(transferBuffer));

         if ( (0 == valid) )
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
      else if ( (1 == appResult) )
      {
         if ( (totalSize <= sizeof(transferBuffer)) )
         {
            memcpy(transferBuffer, pData, totalSize);

            transferActive = 1;
            transferUpload = 1;
            transferStarted = 0;
            transferIndex = index;
            transferSubIndex = subIndex;
            transferToggle = 0;
            transferLength = totalSize;
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
         sendSdoAbort(index, subIndex, 0x06010001UL);
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
    u16_t index;
    u8_t subIndex;
    u32_t abortCode;

    index = (frame[2] << 8) | frame[1];
    subIndex = frame[3];
    
    abortCode = 0;
    abortCode = frame[7];
    abortCode <<= 8;
    abortCode |= frame[6];
    abortCode <<= 8;
    abortCode |= frame[5];
    abortCode <<= 8;
    abortCode |= frame[4];

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
      else if ( (0x80 == frame[0]) )
      {
         setPreOperationalState();
      }
      else if ( (0x81 == frame[0]) )
      {
         CAN_ResetApplication();
      }
      else if ( (0x82 == frame[0]) )
      {
         CAN_ReloadFlash();
         setPreOperationalState();
         sendHeartbeat(0);
      }
   }
}

static void processSyncMessage(void)
{
   u8_t i;
   
   for (i=0; i<4 ;i++)
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
               if ( (0 != rxPdoMapping[i].length) )
               {
                  rxPdoMapping[i].processNeeded = 1;
               }

               rxPdoMapping[i].syncCount = 0;
         }
      }
   }
}

static void processRxPdoMessage(u8_t pdoId, u8_t * frame, u8_t length)
{
   u8_t i;
   u8_t pdoIndex = pdoId - 1;
   DEVICE_RPDO_MAP * pdoMap = &rxPdoMapping[pdoIndex];

   for (i=0; i<length ;i++)
   {
      pdoMap->frame[i] = frame[i];
   }

   pdoMap->length = length;

    if ( (0 == pdoMap->txType) )
    {
        // evaluate index/subIndex, begin action, process when ready
    }        
   else if ( (254 == pdoMap->txType) || (255 == pdoMap->txType) )
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

static void processNmteMessage(u8_t messageDeviceId)
{  
   if ( (0 != consumerHeartbeatTime) &&
        (messageDeviceId == consumerHeartbeatNode) )
   {
      consumerHeartbeatActive = 1;
      consumerHeartbeatTimeLimit = canGetTime() + consumerHeartbeatTime;
      sendDebug(0xAA, consumerHeartbeatTimeLimit);
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
   ledTimeCounter = canGetTime() + 50;

   if ( (LED_OPERATIONAL_S == state) )
   {
      setCANLed(1);
   }
}

static void setDeviceFault(const u8_t * faultCode)
{
    if ( (0 == deviceFaultCode) )
    {
        // store
       deviceFaultCode = (u8_t *)faultCode;

       // load fault error code
      errorStatus = (u32_t)getValue(&deviceFaultCode[4], 4);

      // emit emergency code
      sendEmergency(deviceFaultCode);
    }
}

static void setConsumerHeartbeatTime(u32_t value)
{
    sdoConsumerHeartbeat = value;
    consumerHeartbeatNode = (u8_t)((sdoConsumerHeartbeat >> 16) & 0x7F);
    consumerHeartbeatTime = (u16_t)(sdoConsumerHeartbeat & 0xFFFF);
    consumerHeartbeatActive = 0;
}

static void setPreOperationalState(void)
{
   int i;

   deviceState = DEVICE_PREOPERATIONAL_S;
   transferActive = 0;

   deviceNodeId = assignedNodeId;
   deviceMode = canOd2102;

   if ( (0 == deviceNodeId) || (deviceNodeId > 127) )
   {
      deviceNodeId = DEFAULT_DEVICE_NODE_ID;
   }

   if ( (REPAIR_MODE == deviceMode) )
   {
      drillServoProportionalControlConstant = 30;
      drillServoIntegralControlConstant = 15;
      drillServoDerivativeControlConstant = 767;
      drillServoAcceleration = 2560;
      drillServoHomingVelocity = 1310720;
      drillServoHomingBackoffCount = 44274;
      drillServoTravelVelocity = 2621440;
      drillServoErrorLimit = 3000;
      drillServoPulsesPerUnit = 885472;
   
      setServoProportionalControlConstant(0, drillServoProportionalControlConstant);
      setServoProportionalControlConstant(1, drillServoProportionalControlConstant);
   
      setServoIntegralControlConstant(0, drillServoIntegralControlConstant);
      setServoIntegralControlConstant(1, drillServoIntegralControlConstant);

      setServoDerivativeControlConstant(0, drillServoDerivativeControlConstant);
      setServoDerivativeControlConstant(1, drillServoDerivativeControlConstant);

      setServoErrorLimit(0, drillServoErrorLimit);
      setServoErrorLimit(1, drillServoErrorLimit);
   }
   else if ( (INSPECT_MODE == deviceMode) )
   {
      sensorServoAcceleration = 2560;
      sensorServoHomingVelocity = 1310720;
      sensorServoHomingBackoffCount = 153641;
      sensorServoTravelVelocity = 2621440;
      sensorServoErrorLimit = 300;
      sensorServoPulsesPerDegree = 23199;

      setServoErrorLimit(0, sensorServoErrorLimit);
   }
   
   DDRC |= 0x3;
   PORTC &= 0xFC;
   setCANLed(0);
   
   initCANRate(assignedBaudrateCode);

   errorStatus = 0;
   deviceFaultCode = 0;
    
   setConsumerHeartbeatTime(0);
   producerHeartbeatTime = 0;

   for (i=0; i<4; i++)
   {
      resetTxPdoMap(&txPdoMapping[i], i);
      resetRxPdoMap(&rxPdoMapping[i], i);
   }

   canOd2311 = 0;
   canOd2312 = 0;
   canOd2313 = 0;
   canOd2314 = 0;

   autoDrillControl = 0;
   indexerSearchSpeed = 0;
   indexerTravelSpeed = 0;
   drillRotationSpeed = 0;
   indexerCuttingSpeed = 0;
   indexerCuttingDepth = 0;
   indexerPeckCuttingIncrement = 0;
   indexerPeckRetractionDistance = 0;
   indexerPeckRetractionPosition = 0;

   canOd2411 = 0;
   canOd2412 = 0;
   canOd2413 = 0;
   canOd2414 = 0;

   canOd2441 = 0;
   canOd2442 = 0;
   canOd2443 = 0;
   
   deviceControl = 0;   
   deviceControl0CCache = 0;
   
   processedFrontDrillSpeedSetPoint = 0;
   //processedFrontDrillIndexSetPoint = 0;
   processedRearDrillSpeedSetPoint = 0;
   //processedRearDrillIndexSetPoint = 0;
   processedSensorIndexSetPoint = 0;
   processedDeviceControl = 0;

   setLedState(LED_PREOPERATIONAL_OK_S);
   CAN_NMTChange(127);
}

static void setOperationalState(void)
{
   int i;

   deviceState = DEVICE_OPERATIONAL_S;

   for (i=0; i<4; i++)
   {
      u16_t now = canGetTime();
      txPdoMapping[i].inhibitTimeCount = now;
      txPdoMapping[i].processTimeCount = now + txPdoMapping[i].eventTime;
   }

   setLedState(LED_OPERATIONAL_S);
   CAN_NMTChange(5);
}

static void setStoppedState(void)
{
   deviceState = DEVICE_STOPPED_S;
   setLedState(LED_STOPPED_S);
   CAN_NMTChange(4);
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
   updateProducerHeartbeat();

   receiveResult = rxCANMsg(&message);

   if ( (0 != receiveResult) )
   {
      messageType = CAN_TYPE(message.cobId);
      messageDeviceId = CAN_ID(message.cobId);

      if ( (0 == messageDeviceId) || (deviceNodeId == messageDeviceId) )
      {
         if ( (CAN_NMT_T ==  messageType) )
         {
            processNmtMessage(message.data);
         }
         else if ( (CAN_RSDO_T ==  messageType) )
         {
            processSdoMessage(message.data);
         }
      }
   }
}

static void executeOperationalState(void)
{
    int i;

   updateLED();
   updateConsumerHeartbeat();
   updateProducerHeartbeat();

   for (i=0; i<4; i++)
   {
      updateTxPdoMap(&txPdoMapping[i]);
      updateRxPdoMap(&rxPdoMapping[i]);
   }

   receiveResult = rxCANMsg(&message);

   if ( (0 != receiveResult) )
   {
      messageType = CAN_TYPE(message.cobId);
      messageDeviceId = CAN_ID(message.cobId);

      if ( (0 == messageDeviceId) || (deviceNodeId == messageDeviceId) )
      {
         if ( (CAN_NMT_T ==  messageType) )
         {
            processNmtMessage(message.data);
         }
         else if ( (CAN_SYNC_T == messageType) )
         {
            processSyncMessage();
         }
         else if ( (CAN_RPDO1_T ==  messageType) )
         {
            processRxPdoMessage(1, message.data, message.dlc);
         }
         else if ( (CAN_RPDO2_T ==  messageType) )
         {
            processRxPdoMessage(2, message.data, message.dlc);
         }
         else if ( (CAN_RPDO3_T ==  messageType) )
         {
            processRxPdoMessage(3, message.data, message.dlc);
         }
         else if ( (CAN_RPDO4_T ==  messageType) )
         {
            processRxPdoMessage(4, message.data, message.dlc);
         }
         else if ( (CAN_RSDO_T ==  messageType) )
         {
            processSdoMessage(message.data);
         }
      }

       if ( (CAN_NMTE_T == messageType) )
       {
         processNmteMessage(messageDeviceId);
       }
   }
}

static void executeStoppedState(void)
{
   updateLED();
   updateProducerHeartbeat();

   receiveResult = rxCANMsg(&message);

   if ( (0 != receiveResult) )
   {
      messageType = CAN_TYPE(message.cobId);
      messageDeviceId = CAN_ID(message.cobId);

      if ( (0 == messageDeviceId) || (deviceNodeId == messageDeviceId) )
      {
         if ( (CAN_NMT_T ==  messageType) )
         {
            processNmtMessage(message.data);
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
void canRead(U16 index, U8 subIndex, U8 * destination, U8 length)
{
   uint32_t actualLength = 0;
   loadDeviceData(0, index, subIndex, destination, &actualLength, length);
}

void canWrite(U16 index, U8 subIndex, U8 * source, U8 length)
{
   storeDeviceData(0, index, subIndex, source, 0, length);
}

void canDebug(U32 a, U32 b)
{
   sendDebug(a, b);
}

/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */

U16 canGetTime(void)
{
   uint32_t msec_timer;
   
   do
   {
      tovf_f = 0;
      *((uint8_t *)(&canIsrCount)) = CANTIML;
      *((uint8_t *)(&canIsrCount)+1) = CANTIMH;
      msec_timer = ((canIsrCount + 4) >> 3); /* round and divide by 8 */
   }
   while (tovf_f); /* get time again if timer overflow interrupt occurred */
   
   return (msec_timer&0xFFFF);
}

U8 canIsTimeExpired(U16 timeCount)
{
   uint8_t result;
   uint16_t timeNow;
   
   result = 0;
   timeNow = canGetTime();
   
   if ( (timeNow >= timeCount) )
   {
      if ( ((timeNow - timeCount) < 0x8000) )
      {
         result = 1; /* timestamp has expired */
      }
   }
   else
   {
      if ( ((timeCount - timeNow) >= 0x8000) ) 
      {
         result = 1; /* timestamp has expired */
      }
   }
   
   return (result);
}

/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */

void canSetBaudrate(U16 canBps)
{
   uint8_t code;

   // 0=10K, 1=20K, 2=50K, 3=100K, 4=125K, 5=250K, 6=500K, 7=1M
   if (10 == canBps)
   {
      code = 0;
   }
   else if (100 == canBps)
   {
      code = 1;
   }
   else if (125 == canBps)
   {
      code = 3;
   }
   else if (250 == canBps)
   {
      code = 4;
   }
   else if (250 == canBps)
   {
      code = 5;
   }
   else if (500 == canBps)
   {
      code = 6;
   }
   else if (1000 == canBps)
   {
      code = 7;
   }
   else
   {
      code = 2;
   }

   assignedBaudrateCode = code;
}

void canSetNodeId(U8 nodeId)
{
   assignedNodeId = nodeId;
}

void canInit(void)
{
   isp_select_memory = MEM_DEFAULT;
   isp_select_page   = PAGE_DEFAULT;
   isp_start_address = ADD_DEFAULT;
   isp_number_of_bytes = N_DEFAULT;
   isp_prog_on_going = FALSE;

   // Init_I2C(); // todo: move to setPreOp 
   //  DrillInit(); // todo: move to setPreOp 
    
   // set pre-Operational State
   CAN_ReloadFlash();
   setPreOperationalState();
   sendHeartbeat(0);
}

uint8_t canUpdate(void)
{
   // evaluate state
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
   
   serveCOP();

   return(0);
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
#endif /* USE_MICROCAN_OPEN */
