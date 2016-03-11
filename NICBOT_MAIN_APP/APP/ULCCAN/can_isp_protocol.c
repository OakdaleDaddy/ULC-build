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

//_____ D E F I N I T I O N S __________________________________________________

#define CAN_COB(type,id) ((type<<7)|id)

#define CAN_TYPE(cob) ((cob>>7) & 0xF)
#define CAN_ID(cob) (cob & 0x7F)

#define	SYSTIME_NOW		getCANTimer()
#define	SYSTIME_MAX		0xFFFFFFFF
#define	SYSTIME_DIFF( _First, _Second)		(( _First <= _Second ? ( _Second - _First) : (( SYSTIME_MAX - _First) + _Second)))

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

#define ACCELEROMETER_SAMPLE_PERIOD (50) // mS
#define DRILL_SAMPLE_PERIOD (100) // mS
#define SENSOR_SAMPLE_PERIOD (100) // mS
#define DEVICE_STATUS_SAMPLE_PERIOD (50) // mS

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

typedef enum
{
    DRILL_IDLE_S,
    DRILL_MANUAL_MOVE_S,
    DRILL_RH_RETRACT_TO_LIMIT_WAIT_S,
    DRILL_RH_STOP_FROM_RETRACT_WAIT_S,
    DRILL_RH_EXTEND_TO_NOT_LIMIT_WAIT_S,
    DRILL_RH_STOP_FROM_EXTEND_WAIT_S,
    DRILL_RH_BACKOFF_S,
    DRILL_STOP_S,
    DRILL_FAULTED_S,
}DRILL_STATES;

typedef enum
{
	SENSOR_HOMING_IDLE_S,
	SENSOR_HOMING_CCW_WAIT_S,
	SENSOR_HOMING_STOP_FROM_CCW_S,
	SENSOR_HOMING_NOT_CCW_WAIT_S,
	SENSOR_HOMING_STOP_FROM_NOT_CCW_S,
	SENSOR_HOMING_STOP_FROM_BACKOFF_S,
}SENSOR_HOME_STATES;

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
    u32_t inhibitTimeCount; // used for inhibit time tracking
    u32_t processTimeCount; // used for time tracking
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

typedef struct  
{
    DRILL_STATES state;
    u8_t axis;
    u8_t control;
	u8_t status;
    u16_t retractMask;    
    s16_t manualSetPoint;
    s16_t processedSetPoint;	
}DRILL_CONTEXT;

static void setCANTx(void);
static void setCANRx(u8_t mobId, u8_t id, u8_t mask);
static void initCANRate(u8_t bitRateCode);
static u8_t rxCANMsg(CAN_MSG * canMsg);
static u8_t txCANMsg(CAN_MSG * canMsg);
static u8_t readEEPROM(u16_t address, u8_t * dest, u8_t length);
static u8_t writeEEPROM(u16_t address, u8_t * source, u8_t length);
static u32_t getCANTimer(void);
static void setCANLed(int setValue);

static u8_t setServoAcceleration(u32_t value);
static u8_t setServoVelocity(u32_t value);
static u8_t setServoPosition(u32_t value);
static u8_t setServoErrorLimit(u8_t axisNum, u16_t value);
static u8_t setServoProportionalControlConstant(u8_t axisNum, u32_t value);
static u8_t setServoIntegralControlConstant(u8_t axisNum, u32_t value);
static u8_t setServoDerivativeControlConstant(u8_t axisNum, u32_t value);
static void setCameraLightIntensity(u8_t cameraLocation, u8_t intensity);
static u8_t setFrontLaserControl(u8_t status);
static u8_t setRearLaserControl(u8_t status);
static u8_t setSolenoidControl(u16_t control);
static u8_t setDeviceControl(u16_t control);
static u16_t getDeviceStatus(void);

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
static void updateDeviceStatus(void);
static void updateActualDrillPosition(void);
static void updateActualSensorPosition(void);
static void updateRepairControl(void);
static void updateDrillContext(DRILL_CONTEXT * drillContext);
static void updateInspectControl(void);
static void updateSensorHoming(void);
static void updateAccelerometer(void);

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
static void setPreOperationalState(u8_t initialSet);
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
static u32_t ledTimeCounter;

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
static char * version = "v1.00 " __DATE__ " " __TIME__;

static u32_t errorStatus = 0;
static u8_t * deviceFaultCode;

static u32_t now;
static u32_t difference;
static u32_t limit;

static u32_t sdoConsumerHeartbeat;
static u8_t consumerHeartbeatNode;
static u16_t consumerHeartbeatTime;
static u8_t consumerHeartbeatActive;
static u32_t consumerHeartbeatTimeLimit;

static u16_t producerHeartbeatTime;
static u32_t heartbeatTimeLimit;

static DEVICE_TPDO_MAP txPdoMapping[4];
static DEVICE_RPDO_MAP rxPdoMapping[4];

static u8_t objectBitRateCode;
static u8_t objectNodeId;
static u8_t objectDeviceMode;

static u8_t cameraSelectA;
static u8_t cameraSelectB;
static u8_t cameraLightIntensity[12];

static u16_t solenoidControl;
static u8_t lastWheelPositionRequest;

static s16_t frontDrillSpeedSetPoint; // RPM
//static u16_t frontDrillIndexSetPoint; // units of 1/10 mm
static s16_t rearDrillSpeedSetPoint; // RPM
//static u16_t rearDrillIndexSetPoint; // units of 1/10 mm
static u16_t sensorIndexSetPoint;

static u16_t frontDrillIndexLimit = 635; // 1/10 mm with 2.5 inch stroke
static u16_t rearDrillIndexLimit = 635; // 1/10 mm with 2.5 inch stroke

static u8_t autoDrillControl;
static u16_t indexerSearchSpeed;
static u16_t indexerTravelSpeed;
static s16_t drillRotationSpeed;
static u16_t indexerCuttingSpeed;
static u16_t indexerCuttingDepth;
static u16_t indexerPeckCuttingIncrement;
static u16_t indexerPeckRetractionDistance;
static u16_t indexerPeckRetractionPosition;

static s16_t actualFrontDrillSpeed;
static u16_t actualFrontDrillIndex;
static s16_t actualRearDrillSpeed;
static u16_t actualRearDrillIndex;
static u16_t actualSensorIndex;

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

static u16_t accelerometerX;
static u16_t accelerometerY;
static u16_t accelerometerZ;

static u16_t deviceControl;
static u8_t deviceControl0CCache;
static u16_t deviceStatus;

static u32_t accelerometerSampleTimeLimit;
static u32_t drillPositionSampleTimeLimit;
static u32_t sensorPositionSampleTimeLimit;
static u32_t deviceStatusSampleTimeLimit;

static s16_t processedFrontDrillSpeedSetPoint;
//static u16_t processedFrontDrillIndexSetPoint;
static s16_t processedRearDrillSpeedSetPoint;
//static u16_t processedRearDrillIndexSetPoint;
static u32_t processedSensorIndexSetPoint;
static u16_t processedDeviceControl;

static DRILL_CONTEXT frontDrillContext;
static DRILL_CONTEXT rearDrillContext;

static SENSOR_HOME_STATES sensorHomingState;
static u8_t sensorHomingActivate;

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

    CANTCON = 0x01;
    
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

// read EEPROM memory and stores to destination, return 1 on load of pointer, return 0 on no load
static u8_t readEEPROM(u16_t address, u8_t * dest, u8_t length)
{
	u8_t i;
	
	/* Wait for completion of previous write */
	while(EECR & (1<<EEWE));

	for (i=0; i<length; i++)
	{
		/* Set up address register */
		EEAR = (address + i);

		/* Start eeprom read by writing EERE */
		EECR |= (1<<EERE);
		
		dest[i] = EEDR;
	}

	return(1);
}

// write EEPROM memory from source, return 1 on success, return 0 on failure
static u8_t writeEEPROM(u16_t address, u8_t * source, u8_t length)
{
	u8_t i;	

	for (i=0; i<length; i++)
	{
		/* Wait for completion of previous write */
		while(EECR & (1<<EEWE));
		
		/* Set up address register */
		EEAR = (address + i);

		/* Store Data */
		EEDR = source[i];

		/* Write logical one to EEMWE */
		EECR |= (1<<EEMWE);

		/* Start eeprom write by setting EEWE */
		EECR |= (1<<EEWE);
	}

	return(1);
}

// return free running counter value in uS
static u32_t getCANTimer(void)
{
    u32_t result;

    do
    {
	    tovf_f = 0;
	    *((unsigned char *)(&canIsrCount)) = CANTIML;
	    *((unsigned char *)(&canIsrCount)+1) = CANTIMH;
	    result = canIsrCount;
    } while (tovf_f != 0);
    
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
static u8_t setServoAcceleration(u32_t value)
{
	u8_t result;
	u8_t byte0;
	u8_t byte1;
	u8_t byte2;
	u8_t byte3;
	
	byte0 = (u8_t)(value & 0xFF);
	byte1 = (u8_t)((value >> 8) & 0xFF);
	byte2 = (u8_t)((value >> 16) & 0xFF);
	byte3 = (u8_t)((value >> 24) & 0xFF);
		
	result = servo_load_accel(byte3, byte2, byte1, byte0);
	
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

static u8_t setServoVelocity(u32_t value)
{
	u8_t result;
	u8_t byte0;
	u8_t byte1;
	u8_t byte2;
	u8_t byte3;
	
	byte0 = (u8_t)(value & 0xFF);
	byte1 = (u8_t)((value >> 8) & 0xFF);
	byte2 = (u8_t)((value >> 16) & 0xFF);
	byte3 = (u8_t)((value >> 24) & 0xFF);
	
	result = servo_load_velocity(byte3, byte2, byte1, byte0);
	
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

static u8_t setServoPosition(u32_t value)
{
	u8_t result;
	u8_t byte0;
	u8_t byte1;
	u8_t byte2;
	u8_t byte3;
				
	byte0 = (u8_t)(value & 0xFF);
	byte1 = (u8_t)((value >> 8) & 0xFF);
	byte2 = (u8_t)((value >> 16) & 0xFF);
	byte3 = (u8_t)((value >> 24) & 0xFF);
				
	result = servo_load_position(byte3, byte2, byte1, byte0);
	
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

static void setCameraLightIntensity(u8_t cameraLocation, u8_t intensity)
{
    u8_t index = cameraLocation-1;
    cameraLightIntensity[index] = intensity;
	LED_Intensity(cameraLocation, intensity);	
}

// laser status 1=on, 0=off
static u8_t setFrontLaserControl(u8_t status)
{
	if ( (0 != status) )
	{
    	// turn the laser ON
    	deviceControl0CCache |= 0x06;
    	PORTG |= 0b00010000;
	}
	else
	{
    	// turn the laser OFF
    	if (deviceControl0CCache & 0x1)
    	{
        	deviceControl0CCache &= 0xFB; // if front laser is ON
    	}
    	else
    	{
        	deviceControl0CCache &= 0xF9; // front laser is OFF
    	}
    	
    	PORTG &= 0b11101111;
	}

	IO_Write(0x0C, deviceControl0CCache);
	
	return(1);
}

// laser status 1=on, 0=off
static u8_t setRearLaserControl(u8_t status)
{
	if ( (0 != status) )
	{
    	// turn the laser ON
    	deviceControl0CCache |= 0x03;
    	PORTG |= 0b00001000;
	}
	else
	{
    	// turn the laser OFF
    	if (deviceControl0CCache & 0x4)
    	{
        	deviceControl0CCache &= 0xFE; // if rear laser is ON
    	}
    	else
    	{
        	deviceControl0CCache &= 0xFC; // rear laser is OFF
    	}
    	
    	PORTG &= 0b11110111;
	}

	IO_Write(0x0C, deviceControl0CCache);

	return(1);
}

// return 0 for error, 1 for good; sets solenoid position based on control
static u8_t setSolenoidControl(u16_t control)
{
    u8_t result = 1;

    // todo, evaluate control and determine if valid, set hardware based on bits
	
    if ( (0 != result) )
    {
		u8_t controlH;
		u8_t controlL;
		
		controlH = (u8_t)((control >> 8) & 0xFF);
		controlL = (u8_t)(control & 0xFF);
		
		IO_Write(1, controlL); // group 1
		IO_Write(0, controlH); // group 2
		
        solenoidControl = control;	
		
	    if ( (REPAIR_MODE == deviceMode) )
		{
			controlL = (control & 0x30) << 2; // isolate wheel axial and circ bits
	    }
		else if ( (INSPECT_MODE == deviceMode) )
	    {
			controlL &= 0xC0; // isolate wheel axial and circ bits
		}
		
		if ( (0 != controlL) )
		{
			lastWheelPositionRequest = controlL;
		}
    }

    return(result);
}

// return 0 for error, 1 for good; 
static u8_t setDeviceControl(u16_t control)
{
    u8_t result = 1;

    if ( (REPAIR_MODE == deviceMode) )
    {
	}
    else if ( (INSPECT_MODE == deviceMode) )
    {
	}
	
    if ( (0 != result) )
    {
        deviceControl = control;
    }

    return(result);
}

static u16_t getDeviceStatus(void)
{
    u16_t result = 0;
    u16_t inputStatus = IO_Read(0x06);
    
    if ( (REPAIR_MODE == deviceMode) )
    {        		
		result = (inputStatus << 8);
		result |= frontDrillContext.status;
		result |= (rearDrillContext.status << 3);		
		result |= lastWheelPositionRequest;
    }
    else if ( (INSPECT_MODE == deviceMode) )
    {
		result = (inputStatus << 8);		
		result |= lastWheelPositionRequest;
    }

    return (result);
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
    txPdoMap->inhibitTimeCount = SYSTIME_NOW;
    txPdoMap->processTimeCount = SYSTIME_NOW;
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
        size = sizeof(objectDeviceMode);
        source = (u8_t *)&objectDeviceMode;
    }
    else if (0x2105 == index)
    {
        buffer[0] = 0x73;
        buffer[1] = 0x61;
        buffer[2] = 0x76;
        buffer[3] = 0x65;
        transferred = 4;
    }
    else if ((0x2301 == index) && (0 == subIndex))
    {
        buffer[0] = 1;
        transferred = 1;
    }
    else if ((0x2301 == index) && (1 == subIndex))
    {
        size = sizeof(cameraSelectA);
        source = (u8_t *)&cameraSelectA;
    }
    else if ((0x2301 == index) && (2 == subIndex))
    {
        size = sizeof(cameraSelectB);
        source = (u8_t *)&cameraSelectB;
    }
    else if ((0x2303 == index) && (0 == subIndex))
    {
        buffer[0] = 12;
        transferred = 1;
    }
    else if ((0x2303 == index) && (1 <= subIndex) && (12 >= subIndex))
    {
        buffer[0] = cameraLightIntensity[subIndex-1];
        transferred = 1;
    }
    else if (0x2304 == index)
    {
        size = sizeof(solenoidControl);
        source = (u8_t *)&solenoidControl;
    }
    else if (0x2311 == index)
    {
        if (REPAIR_MODE == deviceMode)
        {
            size = sizeof(frontDrillSpeedSetPoint);
            source = (u8_t *)&frontDrillSpeedSetPoint;
        }
    }
    else if (0x2312 == index)
    {
        if (REPAIR_MODE == deviceMode)
        {            
            size = sizeof(frontDrillContext.manualSetPoint);
            source = (u8_t *)&frontDrillContext.manualSetPoint;
        }
    }
    else if (0x2313 == index)
    {
        if (REPAIR_MODE == deviceMode)
        {
            size = sizeof(rearDrillSpeedSetPoint);
            source = (u8_t *)&rearDrillSpeedSetPoint;
        }
    }
    else if (0x2314 == index)
    {
        if (REPAIR_MODE == deviceMode)
        {            
            size = sizeof(rearDrillContext.manualSetPoint);
            source = (u8_t *)&rearDrillContext.manualSetPoint;
        }
    }
    else if (0x2315 == index)
    {
        if (INSPECT_MODE == deviceMode)
        {
            size = sizeof(sensorIndexSetPoint);
            source = (u8_t *)&sensorIndexSetPoint;
        }
    }
    else if (0x2322 == index)
    {
        if (REPAIR_MODE == deviceMode)
        {
            size = sizeof(frontDrillIndexLimit);
            source = (u8_t *)&frontDrillIndexLimit;
        }
    }
    else if (0x2324 == index)
    {
        if (REPAIR_MODE == deviceMode)
        {
            size = sizeof(rearDrillIndexLimit);
            source = (u8_t *)&rearDrillIndexLimit;
        }
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
        if (REPAIR_MODE == deviceMode)
        {
            size = sizeof(actualFrontDrillSpeed);
            source = (u8_t *)&actualFrontDrillSpeed;
        }        
    }
    else if (0x2412 == index)
    {
        if (REPAIR_MODE == deviceMode)
        {
            size = sizeof(actualFrontDrillIndex);
            source = (u8_t *)&actualFrontDrillIndex;
        }        
    }
    else if (0x2413 == index)
    {
        if (REPAIR_MODE == deviceMode)
        {
            size = sizeof(actualRearDrillSpeed);
            source = (u8_t *)&actualRearDrillSpeed;
        }        
    }
    else if (0x2414 == index)
    {
        if (REPAIR_MODE == deviceMode)
        {
            size = sizeof(actualRearDrillIndex);
            source = (u8_t *)&actualRearDrillIndex;
        }        
    }
    else if (0x2415 == index)
    {
        if (INSPECT_MODE == deviceMode)
        {
            size = sizeof(actualSensorIndex);
            source = (u8_t *)&actualSensorIndex;
        }        
    }
    else if (0x2441 == index)
    {
        size = sizeof(accelerometerX);
        source = (u8_t *)&accelerometerX;
    }
    else if (0x2442 == index)
    {
        size = sizeof(accelerometerY);
        source = (u8_t *)&accelerometerY;
    }
    else if (0x2443 == index)
    {
        size = sizeof(accelerometerZ);
        source = (u8_t *)&accelerometerZ;
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

    return( (0 != transferred) ? 1 : 0 );
}

static u8_t storeDeviceData(u16_t index, u8_t subIndex, u8_t * source, u32_t offset, u32_t length)
{
    u8_t result = 0;

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
        	heartbeatTimeLimit = SYSTIME_NOW;
        	result = 1;
    	}
	}
    if ((0x1400 <= index) && (0x1403 >= index))
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
            objectNodeId = value;
            result = 1;
        }
    }
    else if (0x2102 == index)
    {
        if ( (1 == length) )
        {
            u8_t value = source[offset];

            if ( (value >= 0) && (value <= 1) )
            {
                objectDeviceMode = value;
                result = 1;
            }
        }
    }
    else if (0x2105 == index)
    {
        if ( (4 == length) )
        {
            u32_t command  = getValue(&source[offset], length);

            if ( (0x65766173 == command) )
            {
                u32_t busConfiguration = EEPROM_CAN_CHECK_VALUE;
                busConfiguration <<= 8;
                busConfiguration |= objectDeviceMode;
                busConfiguration <<= 8;
                busConfiguration |= objectNodeId;
                busConfiguration <<= 8;
                busConfiguration |= objectBitRateCode;

                if ( (writeEEPROM(EEPROM_CAN_CONFIGURATION_ADDRESS, (u8_t *)&busConfiguration, sizeof(busConfiguration)) != 0) )
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
    else if ((0x2301 == index) && (1 == subIndex))
    {
        if (1 == length)
        {
            u8_t value = source[offset];

            if (value <= 12)
            {
                cameraSelectA = value;
				Camera_Select(cameraSelectA, cameraSelectB);
	            result = 1;
            }
        }
    }
    else if ((0x2301 == index) && (2 == subIndex))
    {
        if (1 == length)
        {
            u8_t value = source[offset];

            if (value <= 12)
            {
                cameraSelectB = value;
				Camera_Select(cameraSelectA, cameraSelectB);
                result = 1;
            }
        }
    }
    else if ((0x2303 == index) && (1 <= subIndex) && (12 >= subIndex))
    {
        if (1 == length)
        {
            setCameraLightIntensity(subIndex, source[offset]);
            result = 1;
        }
    }
    else if (0x2304 == index)
    {
        if (2 == length)
        {
            u16_t value = (u16_t)getValue(&source[offset], length);
            result = setSolenoidControl(value);
        }
    }
    else if (0x2311 == index)
    {
        if (REPAIR_MODE == deviceMode)
        {
            if (2 == length)
            {
                frontDrillSpeedSetPoint = (u16_t)getValue(&source[offset], length);
                result = 1;
            }
         }
    }
    else if (0x2312 == index)
    {
        if (REPAIR_MODE == deviceMode)
        {
            if (2 == length)
            {
                u16_t value = (u16_t)getValue(&source[offset], length);

                if (value <= frontDrillIndexLimit)
                {
                    frontDrillContext.manualSetPoint = value;
                    result = 1;
                }
            }
        }
    }
    else if (0x2313 == index)
    {
        if (REPAIR_MODE == deviceMode)
        {
            if (2 == length)
            {
                rearDrillSpeedSetPoint = (u16_t)getValue(&source[offset], length);
                result = 1;
            }
        }
    }
    else if (0x2314 == index)
    {
        if (REPAIR_MODE == deviceMode)
        {
            if (2 == length)
            {
                u16_t value = (u16_t)getValue(&source[offset], length);

                if (value <= rearDrillIndexLimit)
                {
                    rearDrillContext.manualSetPoint = value;
                    result = 1;
                }
            }
        }
    }
    else if (0x2315 == index)
    {
        if (INSPECT_MODE == deviceMode)
        {
            if (2 == length)
            {				
                sensorIndexSetPoint = (u16_t)getValue(&source[offset], length);
                result = 1;
            }
        }
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
    else if (0x2354== index)
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
    else if (0x2500 == index)
    {
        if (2 == length)
        {
			u16_t value = (u16_t)getValue(&source[offset], length);
            result = setDeviceControl(value);
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
        loadDeviceData(mapIndex, mapSubIndex, &txMsg.data[offset], &length, limit);
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
    u32_t now = SYSTIME_NOW;
    u32_t diff = SYSTIME_DIFF (ledTimeCounter, now);

	if ( (diff >= 50000) )
	{
		ledTimeCounter = now ;
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
        now = SYSTIME_NOW;
        difference = SYSTIME_DIFF (consumerHeartbeatTimeLimit, now);
        limit = (u32_t)consumerHeartbeatTime * 1000;

	    if ( (difference < MAXIMUM_HEARTBEAT_DIFF) && (difference > limit) )
        {
		    setDeviceFault(consumerHeartbeatLostFaultCode);			
			sendDebug(difference, now);
        }
	}
}

static void updateProducerHeartbeat(void)
{
    if ( (0 != producerHeartbeatTime) )
    {
        u32_t now = SYSTIME_NOW;
        u32_t difference = SYSTIME_DIFF (heartbeatTimeLimit, now);
        u32_t limit = (u32_t)producerHeartbeatTime * 1000;

        if ( (difference >= limit) )
        {
            heartbeatTimeLimit = now ;
            u8_t stateValue = (DEVICE_PREOPERATIONAL_S == deviceState) ? 0x7F : ( (DEVICE_OPERATIONAL_S == deviceState) ? 5 : 4);
            sendHeartbeat(stateValue);
        }
    }
}

static void updateTxPdoMap(DEVICE_TPDO_MAP * txPdoMap)
{
    if ( (DEVICE_OPERATIONAL_S == deviceState) )
    {
        u32_t now = SYSTIME_NOW;
        
        if ( (254 == txPdoMap->txType) &&
             (0 == txPdoMap->initialTriggered) )
        {
            txPdoMap->initialTriggered = 1;
            txPdoMap->processNeeded = 1;            
        }
        
        if ( (0 != txPdoMap->eventTime) )
        {
            u32_t difference = SYSTIME_DIFF (txPdoMap->processTimeCount, now);
            u32_t limit = (u32_t)txPdoMap->eventTime * 1000;

            if (difference >= limit)
            {
                txPdoMap->processTimeCount = now;

                // 254 is transmit by change event, 255 is transmit by time event
                if ( (255 == txPdoMap->txType) )
                {
                    txPdoMap->processNeeded = 1;
                }
            }
        }

        if ( (0 != txPdoMap->processNeeded) )
        {
            u32_t difference = SYSTIME_DIFF (txPdoMap->inhibitTimeCount, now);
            u32_t limit = (u32_t)txPdoMap->inhibitTime * 100;
            
            if (difference >= limit)
            {
                sendPdoData(txPdoMap);
                txPdoMap->inhibitTimeCount = now;
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
                    validTransfer = storeDeviceData(mapIndex, mapSubIndex, frame, offset, mapLength);
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

static void updateDeviceStatus(void)
{
	u16_t tempDeviceStatus;
	
	deviceStatusSampleTimeLimit = SYSTIME_NOW;
	tempDeviceStatus = getDeviceStatus();
	
	if (tempDeviceStatus != deviceStatus)
	{
		u8_t i;

		if (DEVICE_OPERATIONAL_S == deviceState)
		{
			for (i=0; i<4; i++)
			{
				activateTxPdoMap(&txPdoMapping[i], 0x2501, 0);
			}
		}

		deviceStatus = tempDeviceStatus;
	}
}

static void updateActualDrillPosition(void)
{
	difference = SYSTIME_DIFF (drillPositionSampleTimeLimit, now);
	limit = (u32_t)DRILL_SAMPLE_PERIOD * 1000;

	if ( (difference >= limit) )
	{
		u8_t * dataPointer;
		s32_t motorCount;
		
		drillPositionSampleTimeLimit = SYSTIME_NOW;
		
		dataPointer = servo_get_position(1);
		motorCount = dataPointer[0];
		motorCount <<= 8;
		motorCount |= dataPointer[1];
		motorCount <<= 8;
		motorCount |= dataPointer[2];
		motorCount <<= 8;
		motorCount |= dataPointer[3];
		
		motorCount *= -100L;
		motorCount /= drillServoPulsesPerUnit;
		motorCount &= 0xFFFF;
		
		if (motorCount != actualFrontDrillIndex)
		{
			actualFrontDrillIndex = motorCount;
			
			if (DEVICE_OPERATIONAL_S == deviceState)
			{
				u8_t i;
				
				for (i=0; i<4; i++)
				{
					activateTxPdoMap(&txPdoMapping[i], 0x2412, 0);
				}
			}
		}

		dataPointer = servo_get_position(0);
		motorCount = dataPointer[0];
		motorCount <<= 8;
		motorCount |= dataPointer[1];
		motorCount <<= 8;
		motorCount |= dataPointer[2];
		motorCount <<= 8;
		motorCount |= dataPointer[3];
		
		motorCount *= -100L;
		motorCount /= drillServoPulsesPerUnit;
		motorCount &= 0xFFFF;
		
		if (motorCount != actualRearDrillIndex)
		{
			actualRearDrillIndex = motorCount;
			
			if (DEVICE_OPERATIONAL_S == deviceState)
			{
				u8_t i;
				
				for (i=0; i<4; i++)
				{
					activateTxPdoMap(&txPdoMapping[i], 0x2414, 0);
				}
			}
		}

	}
}

static void updateActualSensorPosition(void)
{
    difference = SYSTIME_DIFF (sensorPositionSampleTimeLimit, now);
    limit = (u32_t)SENSOR_SAMPLE_PERIOD * 1000;

    if ( (difference >= limit) )
    {
	    u8_t * dataPointer;
	    s32_t sensorCount;
	        
	    sensorPositionSampleTimeLimit = SYSTIME_NOW;
	        
	    dataPointer = servo_get_position(0);
	    sensorCount = dataPointer[0];
	    sensorCount <<= 8;
	    sensorCount |= dataPointer[1];
	    sensorCount <<= 8;
	    sensorCount |= dataPointer[2];
	    sensorCount <<= 8;
	    sensorCount |= dataPointer[3];
	        
	    sensorCount *= 100L;
	    sensorCount /= sensorServoPulsesPerDegree;
	        
	    if (sensorCount != actualSensorIndex)
	    {
		    actualSensorIndex = sensorCount;
		        
		    if (DEVICE_OPERATIONAL_S == deviceState)
		    {
			    u8_t i;
			        
			    for (i=0; i<4; i++)
			    {
				    activateTxPdoMap(&txPdoMapping[i], 0x2415, 0);
			    }
		    }
	    }
    }
}

static void updateRepairControl(void)
{
	if ( (processedFrontDrillSpeedSetPoint != frontDrillSpeedSetPoint) )
	{
        u32_t speedCache;
		u8_t speedRequest;
		
		// active range 64..127
        speedCache = frontDrillSpeedSetPoint;
        speedCache *= 100;
        speedCache /= 8730;
        speedCache += 64;
        speedRequest = (u8_t)(speedCache & 0xFF);
        sendDebug(0x5A, speedRequest);
        
		drillspeed(1, 0x30, speedRequest);
		
        actualFrontDrillSpeed = frontDrillSpeedSetPoint;

		if (DEVICE_OPERATIONAL_S == deviceState)
		{
    		u8_t i;
    			
    		for (i=0; i<4; i++)
    		{
        		activateTxPdoMap(&txPdoMapping[i], 0x2411, 0);
    		}
		}        
    
        processedFrontDrillSpeedSetPoint = frontDrillSpeedSetPoint;
	}
	
#if 0    
	if ( (processedFrontDrillIndexSetPoint != frontDrillIndexSetPoint) )
	{
		u32_t positionRequest;
		u8_t servoStatus;
		
		positionRequest = frontDrillIndexSetPoint;
		positionRequest *= drillServoPulsesPerUnit;
		positionRequest /= 100L;
		
		servoStatus = servo_get_status(0);

		if ( (servoStatus & 0x04) )
		{
			sendDebug(0x0001A, 0);
			setServoAcceleration(drillServoAcceleration);
			setServoVelocity(drillServoTravelVelocity);
		}

		sendDebug(0x0001B, positionRequest);
		setServoPosition(positionRequest);
		servo_move_abs(0);
		
		processedFrontDrillIndexSetPoint = frontDrillIndexSetPoint;
	}
#endif    

	if ( (processedRearDrillSpeedSetPoint != rearDrillSpeedSetPoint) )
	{
        u32_t speedCache;
        u8_t speedRequest;
        
        // active range 64..127
        speedCache = rearDrillSpeedSetPoint;
        speedCache *= 100;
        speedCache /= 8730;
        speedCache += 64;
        speedRequest = (u8_t)(speedCache & 0xFF);
        sendDebug(0x5B, speedRequest);

		drillspeed(0, 0x30, speedRequest);
		
        actualRearDrillSpeed = rearDrillSpeedSetPoint;
        
		if (DEVICE_OPERATIONAL_S == deviceState)
		{
    		u8_t i;
    			
    		for (i=0; i<4; i++)
    		{
        		activateTxPdoMap(&txPdoMapping[i], 0x2413, 0);
    		}
		}        

		processedRearDrillSpeedSetPoint = rearDrillSpeedSetPoint;
	}

#if 0
	if ( (processedRearDrillIndexSetPoint != rearDrillIndexSetPoint) )
	{
		u32_t positionRequest;
		u8_t servoStatus;
		
		positionRequest = rearDrillIndexSetPoint;
		positionRequest *= drillServoPulsesPerUnit;
		positionRequest /= 100L;
		
		servoStatus = servo_get_status(1);

		if ( (servoStatus & 0x04) )
		{
			sendDebug(0x0002A, 0);
			setServoAcceleration(drillServoAcceleration);
			setServoVelocity(drillServoTravelVelocity);
		}

		sendDebug(0x0002B, positionRequest);
		setServoPosition(positionRequest);
		servo_move_abs(1);
		
		processedRearDrillIndexSetPoint = rearDrillIndexSetPoint;
	}
#endif

	if (processedDeviceControl != deviceControl)
	{
        processedDeviceControl = deviceControl;
        frontDrillContext.control = (u8_t)(deviceControl & 0x00FF);
        rearDrillContext.control = (u8_t)((deviceControl & 0xFF00) >> 8);

		if ( ((deviceControl & 0x0001) != 0) )
		{
			setFrontLaserControl(1);
		}
		else
		{
			setFrontLaserControl(0);
		}

		if ( ((deviceControl & 0x0100) != 0) )
		{
			setRearLaserControl(1);
		}
		else
		{
			setRearLaserControl(0);
		}
	}
}

static void updateDrillContext(DRILL_CONTEXT * drillContext)
{
	if ( (DRILL_IDLE_S == drillContext->state) )
	{
    	if ( ((drillContext->control & 0x04) != 0) )
    	{
            drillContext->control &= ~(0x04);
            
        	setServoAcceleration(drillServoAcceleration);
        	setServoVelocity(drillServoHomingVelocity);
        	setServoPosition(20000000);
        	servo_move_rel(drillContext->axis);
        	
        	sendDebug(0xBC, 1);
			drillContext->status &= ~(0x02);
        	drillContext->state = DRILL_RH_RETRACT_TO_LIMIT_WAIT_S;
    	}
        else if ( (drillContext->manualSetPoint != drillContext->processedSetPoint) )
        {
    		s32_t positionRequest;
		
		    positionRequest = -drillContext->manualSetPoint;
    		positionRequest *= drillServoPulsesPerUnit;
	    	positionRequest /= 100L;
		
    		setServoAcceleration(drillServoAcceleration);
    		setServoVelocity(drillServoTravelVelocity);
		    sendDebug(0xCC, positionRequest);
		    setServoPosition(positionRequest);
		    servo_move_abs(drillContext->axis);
		
    		drillContext->processedSetPoint = drillContext->manualSetPoint;
            
        	sendDebug(0xBC, 10);
			drillContext->status &= ~(0x02);
        	drillContext->state = DRILL_MANUAL_MOVE_S;            
        }            
	}    
	else if ( (DRILL_FAULTED_S == drillContext->state) )
	{
        if ( ((drillContext->control & 0x02) != 0) )
        {
	        drillContext->control &= ~(0x02);
			drillContext->status &= ~(0x01);
			// do the thing to clear the error bit
			drillContext->state = DRILL_IDLE_S;
        }
	}
	else
	{
    	u8_t servoStatus;
    	
    	servoStatus = servo_get_status(drillContext->axis);
    	
    	if ( (servoStatus & 0x20) )
    	{	
			drillContext->status |= 0x01;
			drillContext->state = DRILL_FAULTED_S;
    	}
        else if ( ((drillContext->control & 0x02) != 0) )
        {
            servo_stop_decel(drillContext->axis);
            
            sendDebug(0xBC, 12);
            drillContext->control &= ~(0x02);
            drillContext->state = DRILL_STOP_S;
        }
    	else
    	{
            if ( (DRILL_MANUAL_MOVE_S == drillContext->state) )
            {
                // todo check limits to stop
            	if ( (servoStatus & 0x04) )
            	{
                	sendDebug(0xBC, 11);
					drillContext->status |= 0x02;
                	drillContext->state = DRILL_IDLE_S;
            	}
                else if ( (drillContext->manualSetPoint != drillContext->processedSetPoint) )
                {
                    s32_t positionRequest;
            
                    positionRequest = -drillContext->manualSetPoint;
                    positionRequest *= drillServoPulsesPerUnit;
                    positionRequest /= 100L;
            
                    sendDebug(0xCD, positionRequest);
					
					setServoVelocity(drillServoTravelVelocity);
					servo_set_velocity_while_moving(drillContext->axis);
										
                    setServoPosition(positionRequest);
                    servo_move_abs_while_moving(drillContext->axis);
            
                    drillContext->processedSetPoint = drillContext->manualSetPoint;
                }                
            } 
            else if ( (DRILL_STOP_S == drillContext->state) )
            {
            	if ( (servoStatus & 0x04) )
            	{
                	sendDebug(0xBC, 13);
					drillContext->status |= 0x02;
                	drillContext->state = DRILL_IDLE_S;
            	}            
            }                
        	else if ( (DRILL_RH_RETRACT_TO_LIMIT_WAIT_S == drillContext->state) )
        	{
            	if ( ((deviceStatus & drillContext->retractMask) != 0) )
            	{
                	servo_stop_decel(drillContext->axis);
                	
                	sendDebug(0xBC, 2);
                	drillContext->state = DRILL_RH_STOP_FROM_RETRACT_WAIT_S;
            	}
        	}
        	else if ( (DRILL_RH_STOP_FROM_RETRACT_WAIT_S == drillContext->state) )
        	{
            	if ( (servoStatus & 0x04) )
            	{
                	setServoPosition(-4000000);
                	servo_move_rel(drillContext->axis);
                	
                	sendDebug(0xBC, 3);
                	drillContext->state = DRILL_RH_EXTEND_TO_NOT_LIMIT_WAIT_S;
            	}
        	}
        	else if ( (DRILL_RH_EXTEND_TO_NOT_LIMIT_WAIT_S == drillContext->state) )
        	{
            	if ( ((deviceStatus & drillContext->retractMask) == 0) )
            	{
                	servo_stop_decel(drillContext->axis);
                	
                	sendDebug(0xBC, 4);
                	drillContext->state = DRILL_RH_STOP_FROM_EXTEND_WAIT_S;
            	}
        	}
        	else if ( (DRILL_RH_STOP_FROM_EXTEND_WAIT_S == drillContext->state) )
        	{
            	if ( (servoStatus & 0x04) )
            	{
                	setServoPosition(-drillServoHomingBackoffCount);
                	servo_move_rel(drillContext->axis);
                	
                	sendDebug(0xBC, 5);
                	drillContext->state = DRILL_RH_BACKOFF_S;
            	}
        	}
        	else if ( (DRILL_RH_BACKOFF_S == drillContext->state) )
        	{
            	if ( (servoStatus & 0x04) )
            	{
                	servo_set_origin(drillContext->axis);
                    drillContext->processedSetPoint = 0;
					drillContext->manualSetPoint = 0;
                	
                	sendDebug(0xBC, 6);
					drillContext->status |= 0x02;					
                	drillContext->state = DRILL_IDLE_S;
            	}
        	}
    	}
	}
}

static void updateInspectControl(void)
{
	if ( (processedSensorIndexSetPoint != sensorIndexSetPoint) && (SENSOR_HOMING_IDLE_S == sensorHomingState) )
	{
		u32_t positionRequest;
		u8_t servoStatus;
		
		positionRequest = sensorIndexSetPoint;
		positionRequest *= sensorServoPulsesPerDegree;
		positionRequest /= 100L;
		
		servoStatus = servo_get_status(0);

		if ( (servoStatus & 0x04) )
		{
			sendDebug(0x0000A, 0);
			setServoAcceleration(sensorServoAcceleration);
			setServoVelocity(sensorServoTravelVelocity);
		}

		sendDebug(0x0000B, positionRequest);
		setServoPosition(positionRequest);
		servo_move_abs(0);
		
		processedSensorIndexSetPoint = sensorIndexSetPoint;
	}
	
	if (processedDeviceControl != deviceControl)
	{
		u16_t controlToEvalute = deviceControl & ~(processedDeviceControl);
		processedDeviceControl = deviceControl;
		
		if ( ((controlToEvalute & 0x0001) != 0) && (SENSOR_HOMING_IDLE_S == sensorHomingState) )
		{
			sensorHomingActivate = 1;
		}

		if ( ((controlToEvalute & 0x0002) != 0) )
		{
			sendDebug(0x0000C, 0);
			servo_motor_off(0);
		}
	}
}

static void updateSensorHoming(void)
{
	if ( (SENSOR_HOMING_IDLE_S == sensorHomingState) )
	{
		if ( (0 != sensorHomingActivate) )
		{
			sensorHomingActivate = 0;			
			
			setServoAcceleration(sensorServoAcceleration);
			setServoVelocity(sensorServoHomingVelocity);
			setServoPosition(-20000000);
			servo_move_rel(0);	
			
			sendDebug(0xAB, 1);
			sensorHomingState = SENSOR_HOMING_CCW_WAIT_S;
		}
	}
	else
	{
		u8_t servoStatus;
		
		servoStatus = servo_get_status(0);
		
		if ( (servoStatus & 0x20) )
		{
			sensorHomingState = SENSOR_HOMING_IDLE_S;
			setDeviceFault(servoExcessivePositionFaultCode);
		}
		else
		{
			if ( (SENSOR_HOMING_CCW_WAIT_S == sensorHomingState) )
			{
				if ( ((deviceStatus & 0x0400) == 0) )
				{
					servo_stop_decel(0);
				
					sendDebug(0xAB, 2);				
					sensorHomingState = SENSOR_HOMING_STOP_FROM_CCW_S;
				}
			}
			else if ( (SENSOR_HOMING_STOP_FROM_CCW_S == sensorHomingState) )
			{
				if ( (servoStatus & 0x04) )
				{
					setServoPosition(4000000);
					servo_move_rel(0);
				
					sendDebug(0xAB, 3);
					sensorHomingState = SENSOR_HOMING_NOT_CCW_WAIT_S;				
				}
			}
			else if ( (SENSOR_HOMING_NOT_CCW_WAIT_S == sensorHomingState) )
			{
				if ( ((deviceStatus & 0x0400) != 0) )
				{
					servo_stop_decel(0);
				
					sendDebug(0xAB, 4);
					sensorHomingState = SENSOR_HOMING_STOP_FROM_NOT_CCW_S;
				}
			}
			else if ( (SENSOR_HOMING_STOP_FROM_NOT_CCW_S == sensorHomingState) )
			{
				if ( (servoStatus & 0x04) )
				{
					setServoPosition(sensorServoHomingBackoffCount);
					servo_move_rel(0);
				
					sendDebug(0xAB, 5);
					sensorHomingState = SENSOR_HOMING_STOP_FROM_BACKOFF_S;
				}
			}
			else if ( (SENSOR_HOMING_STOP_FROM_BACKOFF_S == sensorHomingState) )
			{
				if ( (servoStatus & 0x04) )
				{		
					servo_set_origin(0);
					processedSensorIndexSetPoint = 0;
					sensorIndexSetPoint = 0;
					
					sendDebug(0xAB, 7);
					sensorHomingState = SENSOR_HOMING_IDLE_S;
				    sensorPositionSampleTimeLimit = SYSTIME_NOW;
				}
			}		
		}		
	}
}

static void updateAccelerometer(void)
{
    now = SYSTIME_NOW;
    difference = SYSTIME_DIFF (accelerometerSampleTimeLimit, now);
    limit = (u32_t)ACCELEROMETER_SAMPLE_PERIOD * 1000;
    
    if ( (difference > limit) )
    {
	    u16_t tempAccelerometerX;
	    u16_t tempAccelerometerY;
	    u16_t tempAccelerometerZ;
	    
	    accelerometerSampleTimeLimit = SYSTIME_NOW;
	    
	    tempAccelerometerX = accelerometer_get_x();
		tempAccelerometerX &= 0xFE00; // b8 shows activity when sitting still
		
	    tempAccelerometerY = accelerometer_get_y();
		tempAccelerometerY &= 0xFE00; // b8 shows activity when sitting still

	    tempAccelerometerZ = accelerometer_get_z();
		tempAccelerometerZ &= 0xFE00; // b8 shows activity when sitting still

	    if (tempAccelerometerX != accelerometerX)
	    {
		    u8_t i;

		    if (DEVICE_OPERATIONAL_S == deviceState)
		    {
			    for (i=0; i<4; i++)
			    {
				    activateTxPdoMap(&txPdoMapping[i], 0x2441, 0);
			    }
		    }

		    accelerometerX = tempAccelerometerX;
	    }
	    
	    if (tempAccelerometerY != accelerometerY)
	    {
		    u8_t i;

		    if (DEVICE_OPERATIONAL_S == deviceState)
		    {
			    for (i=0; i<4; i++)
			    {
				    activateTxPdoMap(&txPdoMapping[i], 0x2442, 0);
			    }
		    }

		    accelerometerY = tempAccelerometerY;
	    }
	    
	    if (tempAccelerometerZ != accelerometerZ)
	    {
		    u8_t i;

		    if (DEVICE_OPERATIONAL_S == deviceState)
		    {
			    for (i=0; i<4; i++)
			    {
				    activateTxPdoMap(&txPdoMapping[i], 0x2443, 0);
			    }
		    }

		    accelerometerZ = tempAccelerometerZ;
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
			CAN_ResetApplication();
		}
		else if ( (0x81 == frame[0]) ||
		          (0x82 == frame[0]) )
		{
		    setPreOperationalState(0);
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
         (messageDeviceId == consumerHeartbeatNode))
	{
		consumerHeartbeatActive = 1;
		consumerHeartbeatTimeLimit = SYSTIME_NOW;
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
	ledTimeCounter = SYSTIME_NOW;

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

static void setPreOperationalState(u8_t initialSet)
{
    u32_t busConfiguration;
    int i;

	deviceState = DEVICE_PREOPERATIONAL_S;
	transferActive = 0;

	objectBitRateCode = DEFAULT_DEVICE_BIT_RATE_CODE;
	objectNodeId = DEFAULT_DEVICE_NODE_ID;
	objectDeviceMode = DEFAULT_DEVICE_MODE;

	if ( (readEEPROM(EEPROM_CAN_CONFIGURATION_ADDRESS, (u8_t*)&busConfiguration, sizeof(busConfiguration)) != 0) )
	{
		u8_t check = ((busConfiguration >> 24) & 0xFF);

		if ( (EEPROM_CAN_CHECK_VALUE == check) )
		{
			objectDeviceMode = (busConfiguration >> 16) & 0xFF;
			objectNodeId = (busConfiguration >> 8) & 0xFF;
			objectBitRateCode = (busConfiguration >> 0) & 0xFF;
		}
	}
	
	deviceNodeId = objectNodeId;
    deviceMode = objectDeviceMode;

	if ( (0 == deviceNodeId) || (deviceNodeId > 127) )
	{
		deviceNodeId = DEFAULT_DEVICE_NODE_ID;
	}
	
	if ( (REPAIR_MODE == deviceMode) )
	{
		servo_reset(0);
		servo_init(0);
		
		servo_reset(1);
		servo_init(1);
		
		DrillInit();
        drillspeed(0, 0x30, 0x45);
        drillspeed(1, 0x30, 0x45);        
        
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
        
        memset(&frontDrillContext, 0, sizeof(frontDrillContext));
        //frontDrillContext.state = DRILL_IDLE_S;
        frontDrillContext.axis = 1;
        //frontDrillContext.control = 0;
        frontDrillContext.retractMask = 0x0400;
        //frontDrillContext.manualSetPoint = 0;
        //frontDrillContext.processedSetPoint = 0;
        
        memset(&rearDrillContext, 0, sizeof(rearDrillContext));
        //rearDrillContext.state = DRILL_IDLE_S;
        //rearDrillContext.axis = 0;
        //rearDrillContext.control = 0;
        rearDrillContext.retractMask = 0x0100;
        //rearDrillContext.manualSetPoint = 0;
        //rearDrillContext.processedSetPoint = 0;
	}	
	else if ( (INSPECT_MODE == deviceMode) )
	{
		servo_reset(0);
		servo_init(0);
		   
		sensorServoAcceleration = 2560;
		sensorServoHomingVelocity = 1310720;
	    sensorServoHomingBackoffCount = 153641;
		sensorServoTravelVelocity = 2621440;
		sensorServoErrorLimit = 300;
		sensorServoPulsesPerDegree = 23199;

		setServoErrorLimit(0, sensorServoErrorLimit);
    
		sensorHomingState = SENSOR_HOMING_IDLE_S;
		sensorHomingActivate = 0;
	}
	
	accelerometer_init();
	setCANLed(0);
	
	initCANRate(objectBitRateCode);

	errorStatus = 0;
	deviceFaultCode = 0;
    
	setConsumerHeartbeatTime(0);
	producerHeartbeatTime = 0;

	for (i=0; i<4; i++)
	{
		resetTxPdoMap(&txPdoMapping[i], i);
		resetRxPdoMap(&rxPdoMapping[i], i);
	}

    cameraSelectA = 1;
    cameraSelectB = 2;
	Camera_Select(cameraSelectA, cameraSelectB);

    for (i=1; i<12; i++)
    {
        setCameraLightIntensity(i, 0);
    }

	// check to set solenoids
	if ( (0 != initialSet) )
	{
		// set solenoids
		solenoidControl = 0;
		setSolenoidControl(0);
	}

    frontDrillSpeedSetPoint = 0;
    //frontDrillIndexSetPoint = 0;
    rearDrillSpeedSetPoint = 0;
    //rearDrillIndexSetPoint = 0;
    sensorIndexSetPoint = 0;

    autoDrillControl = 0;
    indexerSearchSpeed = 0;
    indexerTravelSpeed = 0;
    drillRotationSpeed = 0;
    indexerCuttingSpeed = 0;
    indexerCuttingDepth = 0;
    indexerPeckCuttingIncrement = 0;
    indexerPeckRetractionDistance = 0;
    indexerPeckRetractionPosition = 0;

    actualFrontDrillSpeed = 0;
    actualFrontDrillIndex = 0;
    actualRearDrillSpeed = 0;
    actualRearDrillIndex = 0;
    actualSensorIndex = 0;
    
    accelerometerSampleTimeLimit = SYSTIME_NOW;
	drillPositionSampleTimeLimit = SYSTIME_NOW;
	sensorPositionSampleTimeLimit = SYSTIME_NOW;
    deviceStatusSampleTimeLimit = SYSTIME_NOW;

    accelerometerX = 0;
    accelerometerY = 0;
    accelerometerZ = 0;
	
    deviceControl = 0;	
	deviceControl0CCache = 0;
    deviceStatus = getDeviceStatus();
	
	processedFrontDrillSpeedSetPoint = 0;
	//processedFrontDrillIndexSetPoint = 0;
	processedRearDrillSpeedSetPoint = 0;
	//processedRearDrillIndexSetPoint = 0;
	processedSensorIndexSetPoint = 0;
	processedDeviceControl = 0;

	if ( (sendHeartbeat(0) != 0) )
	{
		setLedState(LED_PREOPERATIONAL_OK_S);
	}
	else
	{
		sendDebug(1,1); // prevent compiler warning
		setLedState(LED_PREOPERATIONAL_ERROR_S);
	}

	CAN_NMTChange(127);
}

static void setOperationalState(void)
{
	deviceState = DEVICE_OPERATIONAL_S;
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
	updateDeviceStatus();
	
	if (REPAIR_MODE == deviceMode)
	{
		updateActualDrillPosition();		
	}
	else if (INSPECT_MODE == deviceMode)
	{
		updateActualSensorPosition();
	}

	updateAccelerometer();

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
	updateDeviceStatus(); // todo separate device status for repair and inspect
	
	if (REPAIR_MODE == deviceMode)
	{
		updateActualDrillPosition();
		updateRepairControl();
        updateDrillContext(&frontDrillContext);
        updateDrillContext(&rearDrillContext);
	}
	else if (INSPECT_MODE == deviceMode)
	{
		updateActualSensorPosition();
		updateInspectControl();
		updateSensorHoming();	
	}
	
	updateAccelerometer();

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
	updateDeviceStatus();
	
	if (REPAIR_MODE == deviceMode)
	{
		updateActualDrillPosition();
	}
	else if (INSPECT_MODE == deviceMode)
	{
		updateActualSensorPosition();
	}
	
	updateAccelerometer();

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
void can_isp_protocol_init(void)
{
    isp_select_memory = MEM_DEFAULT;
    isp_select_page   = PAGE_DEFAULT;
    isp_start_address = ADD_DEFAULT;
    isp_number_of_bytes = N_DEFAULT;
    isp_prog_on_going = FALSE;

    //	Init_I2C(); // todo: move to setPreOp 
    //  DrillInit(); // todo: move to setPreOp 
    
    // enable interrupts
    sei();
    
    // set pre-Operational State
    setPreOperationalState(1);
}

void can_isp_protocol_task(void)
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
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
#endif /* USE_MICROCAN_OPEN */
