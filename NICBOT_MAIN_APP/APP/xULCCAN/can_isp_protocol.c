
#ifndef USE_MICROCAN_OPEN

//_____ I N C L U D E S ________________________________________________________
#include "string.h"
#include "config.h"
#include "wheel_motor_control.h"
#include "can_isp_protocol.h"
#include "isp_lib.h"
#include "can_drv.h"
#include "can_callbacks.h"

//_____ D E F I N I T I O N S __________________________________________________
#define EEPROM_CAN_CONFIGURATION_ADDRESS (256)
#define EEPROM_CAN_CHECK_VALUE (0x2A)

#define CAN_COB(type,id) ((type<<7)|id)
#define CAN_TYPE(cob) ((cob>>7) & 0xF)
#define CAN_ID(cob) (cob & 0x7F)

#define	SYSTIME_NOW		getCANTimer()
#define	SYSTIME_MAX		0xFFFFFFFF
#define	SYSTIME_DIFF( _First, _Second)		(( _First <= _Second ? ( _Second - _First) : (( SYSTIME_MAX - _First) + _Second)))

#define MAXIMUM_HEARTBEAT_DIFF (0x0000FFFFL * 1000)

#define MOTOR_TEMPERATURE_SAMPLE_BUFFER_SIZE (10)
#define MOTOR_TEMPERATURE_SAMPLE_PERIOD (20)
#define MOTOR_UPDATE_PERIOD (100)

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
	NOT_READY_TO_SWITCH_ON,
	SWITCH_ON_DISABLED,
	READY_TO_SWITCH_ON,
	SWITCHED_ON,
	OPERATIONAL_ENABLE,
	QUICK_STOP_ACTIVE,
	FAULT_REACTION_ACTIVE,
	NOT_READY_TO_SWITCH_ON_FAULTED,
}MOTOR_STATE;

typedef unsigned char u8_t;
typedef char s8_t;
typedef unsigned short u16_t;
typedef short s16_t;
typedef unsigned long u32_t;
typedef long s32_t;

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

static void initCANRate(u8_t bitRateCode);
static u32_t readHardwareInput(void);
static u8_t readEEPROM(u16_t address, u8_t * dest, u8_t length);
static u8_t writeEEPROM(u16_t address, u8_t * source, u8_t length);
static u32_t getCANTimer(void);
static void setCANLed(int setValue);
static u8_t readCAN(st_cmd_t *canMsg);

static u32_t getValue(u8_t * source, u8_t length);
static void setDeviceFault(const u8_t * faultCode);
static void setConsumerHeartbeatTime(u32_t value);
static u8_t pdoMapByteCount(u32_t mapping);
static u8_t pdoByteSize(u16_t index, u8_t subIndex);
static u32_t setMotorControlWord(u16_t value);
static u16_t getMotorStatusWord(void);
static void setMotorFreeWheel(void);

static u8_t pdoTxMappable(u16_t index, u8_t subIndex);
static void loadTxPdoMapParameter(DEVICE_TPDO_MAP * txPdoMap, u8_t subIndex, u8_t * buffer, u32_t * dataCount);
static void loadTxPdoMapData(DEVICE_TPDO_MAP * txPdoMap, u8_t subIndex, u8_t * buffer, u32_t * dataCount);
static u8_t storeTxPdoMapParameter(DEVICE_TPDO_MAP * txPdoMap, u8_t subIndex, u8_t byteCount, u32_t value);
static u8_t storeTxPdoMapData(DEVICE_TPDO_MAP * txPdoMap, u8_t subIndex, u8_t byteCount, u32_t value);
static void activateTxPdoMap(DEVICE_TPDO_MAP * txPdoMap, u16_t index, u8_t subIndex);
static void checkTxPdoMap(u16_t index, u8_t subIndex);
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
static u32_t sendEmergency(u8_t * faultCode);
static u32_t sendPdoData(DEVICE_TPDO_MAP * txPdoMap);
static u32_t sendSdoData(void);
static u32_t sendSdoAbort(u16_t index, u8_t subIndex, u32_t code);

static void updateLED(void);
static void updateConsumerHeartbeat(void);
static void updateProducerHeartbeat(void);
static void updateTxPdoMap(DEVICE_TPDO_MAP * txPdoMap);
static void updateRxPdoMap(DEVICE_RPDO_MAP * rxPdoMap);
static void updateMotorState(u8_t operational);

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
static void setPreOperationalState(void);
static void setOperationalState(void);
static void setStoppedState(void);

static void executePreOperationalState(void);
static void executeOperationalState(void);
static void executeStoppedState(void);

static void initCANInterface(void);
static void updateCANInterface(void);

//_____ D E C L A R A T I O N S ________________________________________________

static DEVICE_STATE deviceState;
static u8_t deviceNodeId;

static volatile u32_t canIsrCount;
static volatile u8_t tovf_f;
#if 0
static volatile u8_t  canIsrOccurred;
static volatile u16_t canIsrCount;
#endif

static LED_STATE ledState;
static u16_t ledFlashCount;
static u16_t ledFlashLimit;
static u16_t ledFlashStep;
static u32_t ledTimeCounter;

static st_cmd_t can_isp_rx_msg;
static u8_t can_isp_rx_buffer[8];
static st_cmd_t can_isp_tx_msg;
static u8_t can_isp_tx_buffer[8];

static st_cmd_t message;
static u8_t messageBuffer[8];
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

static u32_t deviceType = 0x00004020;
static char * deviceName = "NICBOT Wheel";
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
static u8_t objectNodeOffset;

static u8_t motorTemperatureSampleBuffer[MOTOR_TEMPERATURE_SAMPLE_BUFFER_SIZE];
static u8_t motorTemperatureSampleIndex;
static u8_t motorTemperatureSampleCount;
static u8_t motorTemperature;
static u32_t motorTemperatureTimeLimit;

static MOTOR_STATE motorState;
static u16_t motorControlWord;
static u16_t motorStatusWord;
static u8_t motorMode;
static u8_t motorDisplayMode;
static u32_t motorTimeLimit;

static u32_t motorRatedCurrent;
static u16_t motorMaximumCurrent;
static u32_t motorRatedTorque;
static u16_t motorMaximumTorque;

static u32_t motorMaximumVelocity;
static u32_t motorVelocityAcceleration;
static u32_t motorVelocityDeceleration;
static s32_t motorVelocityTarget;
static s32_t motorActualVelocity;

static u32_t motorTorqueSlope;
static s16_t motorTorqueTarget;
static s16_t motorActualTorque;

static u8_t  motorFreeRequested;
static s32_t motorRequestedVelocity;
static s16_t motorRequestedCurrent;

static const u8_t consumerHeartbeatLostFaultCode[8] = { 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
static const u8_t rpdoFailureLostFaultCode[8] = { 0x00, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
static const u8_t motorFaultCode[8] = { 0x00, 0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */



/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
ISR(CAN_TOVF_vect)
{
	tovf_f = 1;
	canIsrCount += 0x00010000;
#if 0
	canIsrCount++;
	canIsrOccurred = 1;
#endif	
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

    CANTCON = 0b00000001;
    
    CANGIT &= ~(0x20);
    CANGIE |= 0x01;
}

// read inputs into processor towards device node ID
static u32_t readHardwareInput(void)
{
	// todo
	return(0);
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
#if 0	
	u16_t timerCount;
	u16_t upperCount;
	u32_t result;
	
	do
	{
    	canIsrOccurred = 0;
    	timerCount = CANTIM;
	    timerCount = CANTIM; // read twice as upper byte of 16-bit timer shown to be stuck at FF		
		upperCount = canIsrCount;
		upperCount = canIsrCount; // read twice as upper byte of 16-bit timer shown to be stuck at FF		
	} while (0 != canIsrOccurred);
	
	result = canIsrCount;
	result <<= 16;
	result |= timerCount;
	
	return(result);
#endif
}

// set LED, 1 is on, 0 is off
static void setCANLed(int setValue)
{
	if ( (0 != setValue) )
	{
		PORTB |= 0x10;
	}
	else 
	{
		PORTB &= ~(0x10);
	}
}

// reads CAN buffer, return 1 on load of pointer, return 0 on no load
static u8_t readCAN(st_cmd_t * canMsg)
{
	static u8_t received = 0;
	u8_t result = 0;
	u8_t u8_temp;
	u8_t i;
	
	// check to command receive
	if ( (0 == received) )
	{
		// flag activity
		received = 1;
		
		// prepare structure
		can_isp_rx_msg.dlc = 8;
		can_isp_rx_msg.id.std = 0;
		
		// issue receive command
		while (can_cmd(&can_isp_rx_msg) != CAN_CMD_ACCEPTED);
	}
	
	u8_temp = can_get_status(&can_isp_rx_msg);
	if (u8_temp != CAN_STATUS_NOT_COMPLETED)
	{
		received = 0;

		if (u8_temp == CAN_STATUS_ERROR)
		{
			return(0);  // Re-evaluation of bit-timing asked, out of the function
		}
		
		canMsg->id.std = can_isp_rx_msg.id.std;
		canMsg->dlc = can_isp_rx_msg.dlc;
		
		for (i=0; i<8; i++)
		{
			canMsg->pt_data[i] = can_isp_rx_msg.pt_data[i];
		}
		
		result = 1;
	}
	
	return(result);
}

void can_isp_send_frame(void)
{
	//- Tx Command
	while(can_cmd(&can_isp_tx_msg) != CAN_CMD_ACCEPTED);
	
	//- wait for Tx completed
	while(can_get_status(&can_isp_tx_msg) == CAN_STATUS_NOT_COMPLETED);
	
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

static void setDeviceFault(const u8_t * faultCode)
{
	deviceFaultCode = (u8_t *)faultCode;
	motorState = FAULT_REACTION_ACTIVE;
}

static void setConsumerHeartbeatTime(u32_t value)
{
    sdoConsumerHeartbeat = value;
    consumerHeartbeatNode = (u8_t)((sdoConsumerHeartbeat >> 16) & 0x7F);
    consumerHeartbeatTime = (u16_t)(sdoConsumerHeartbeat & 0xFFFF);
    consumerHeartbeatActive = 0;
}

static u8_t pdoMapByteCount(u32_t mapping)
{
    u8_t mapByteCount = (u8_t)((mapping & 0xFF) / 8);
    return (mapByteCount);
}

static u8_t pdoByteSize(u16_t index, u8_t subIndex)
{
    u8_t result = 0;

	if ( (0x2301 == index) )
	{
		result = 1;
	}
    else if ( (0x6040 == index) ||
              (0x6041 == index) ||
              (0x6071 == index) ||
              (0x6077 == index))
    {
        result = 2;
    }
    else if ( (0x606c == index) ||
              (0x60FF == index))
    {
        result = 4;
    }

    return(result);
}

// returns 1 on ok, 0 on error; todo invert logic
static u32_t setMotorControlWord(u16_t value)
{
	u32_t result = 0;

	if (SWITCH_ON_DISABLED == motorState)
	{
		if (0x0000 == (u16_t)(value & 0x0082))
		{
			// no transition
			result = 1;
		}
		else if (0x0006 == (u16_t)(value & 0x0087))
		{
			// transition 2
			motorState = READY_TO_SWITCH_ON;					
			result = 1;
			
			motorRequestedVelocity = 0;
			motorRequestedCurrent = 0;
		}
	}
	else if (READY_TO_SWITCH_ON == motorState)
	{
		if (0x0000 == (u16_t)(value & 0x0082))
		{
			// transition 7
			motorState = SWITCH_ON_DISABLED;
			result = 1;
		}
		else if (0x0007 == (u16_t)(value & 0x008F))
		{
			// transition 3
			motorState = SWITCHED_ON;
			result = 1;
		}
	}
	else if (SWITCHED_ON == motorState)
	{
		if (0x0006 == (u16_t)(value & 0x0087))
		{
			// transition 6
			motorState = READY_TO_SWITCH_ON;
			result = 1;
		}
		else if (0x000F == (u16_t)(value & 0x008F))
		{
			// transition 4
			motorState = OPERATIONAL_ENABLE;
			result = 1;
		}
	}
	else if (OPERATIONAL_ENABLE == motorState)
	{
		if (0x0007 == (u16_t)(value & 0x008F))
		{
			// transition 5
			motorState = SWITCHED_ON;
			result = 1;
		}
		else if (0x0006 == (u16_t)(value & 0x0087))
		{
			// transition 8
			motorState = READY_TO_SWITCH_ON;
			result = 1;
		}
		else if (0x0000 == (u16_t)(value & 0x0082))
		{
			// transition 9
			motorState = SWITCH_ON_DISABLED;
			result = 1;
		}
		else if (0x0002 == (u16_t)(value & 0x0086))
		{
			// transition 11
			motorState = QUICK_STOP_ACTIVE;
			result = 1;
		}
	}
	else if (QUICK_STOP_ACTIVE == motorState)
	{
		if (0x0000 == (u16_t)(value & 0x0082))
		{
			// transition 12
			motorState = SWITCH_ON_DISABLED;
			result = 1;
		}
		else if (0x000F == (u16_t)(value & 0x008F))
		{
			// transition 16
			motorState = OPERATIONAL_ENABLE;
			result = 1;
		}
	}
	else if (NOT_READY_TO_SWITCH_ON_FAULTED == motorState)
	{
		if (0x0080 == (u16_t)(value & 0x008F))
		{
			// transition 15
			motorState = SWITCH_ON_DISABLED;
			result = 1;
		}
	}
	
	if (0 != result)
	{
		motorControlWord = value;
	}

	return (result);
}

static u16_t getMotorStatusWord(void)
{
	u16_t result = 0;

	if ((READY_TO_SWITCH_ON == motorState) ||
	    (SWITCHED_ON == motorState) ||
	    (OPERATIONAL_ENABLE == motorState))
	{
		result |= 0x0001;
	}

	if ((SWITCHED_ON == motorState) ||
      	(OPERATIONAL_ENABLE == motorState))
 	{
		result |= 0x0002;
	}

	if (OPERATIONAL_ENABLE == motorState)
	{
		result |= 0x0004;
	}

	if ((FAULT_REACTION_ACTIVE == motorState) ||
	    (NOT_READY_TO_SWITCH_ON_FAULTED == motorState))
	{
		result |= 0x0008;
	}

	if ((READY_TO_SWITCH_ON == motorState) ||
	    (SWITCHED_ON == motorState) ||
	    (OPERATIONAL_ENABLE == motorState))
	{
		result |= 0x0010;
	}

	if ((SWITCH_ON_DISABLED != motorState) &&
     	(QUICK_STOP_ACTIVE != motorState))
	{
		result |= 0x0020;
	}

	if (SWITCH_ON_DISABLED == motorState)
	{
		result |= 0x0040;
	}
	
	result |= 0x200;
	
	if ( (DEVICE_OPERATIONAL_S == deviceState) &&
	     (OPERATIONAL_ENABLE == motorState) )
	{
		if ( (3 == motorDisplayMode) )
		{
			if ( (motorVelocityTarget == motorActualVelocity) )
			{
				result |= 0x400;
			}
		}
		else if ( (4 == motorDisplayMode) )
		{
			if ( (motorTorqueTarget == motorActualTorque) )
			{
				result |= 0x400;
			}	
		}
	}

	return (result);
}

static void setMotorFreeWheel(void)
{
	if ( (0 == motorFreeRequested) )
	{
		motorRequestedVelocity = 0;
		motorRequestedCurrent = 0;
			
		motorActualVelocity = 0;
		checkTxPdoMap(0x606C, 0);

		motorActualTorque = 0;
		checkTxPdoMap(0x6077, 0);
			
		spin(0, 0, 0, 0, 0);
		motorFreeRequested = 1;
	}
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

    if ( (0x2301 == index) ||
	     (0x6041 == index) ||
         (0x606c == index) ||
         (0x6077 == index) )
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

static void checkTxPdoMap(u16_t index, u8_t subIndex)
{
	u8_t i;

	for (i=0; i<4; i++)
	{
		activateTxPdoMap(&txPdoMapping[i], index, subIndex);
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

	if ((0x6040 == index) ||
        (0x6071 == index) ||
        (0x60FF == index))
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
	else if (0x2301 == index)
	{
		size = sizeof(motorTemperature);
		source = (u8_t *)&motorTemperature;
	}   
    else if (0x6040 == index)
    {
		size = sizeof(motorControlWord);
		source = (u8_t *)&motorControlWord;
    }
    else if (0x6041 == index)
    {
		size = sizeof(motorStatusWord);
		source = (u8_t *)&motorStatusWord;
    }
    else if (0x6060 == index)
    {
		size = sizeof(motorMode);
		source = (u8_t *)&motorMode;
    }
    else if (0x6061 == index)
    {
		size = sizeof(motorDisplayMode);
		source = (u8_t *)&motorDisplayMode;
    }	
    else if (0x606C == index)
    {
		size = sizeof(motorActualVelocity);
		source = (u8_t *)&motorActualVelocity;
    }
    else if (0x6071 == index)
    {
		size = sizeof(motorTorqueTarget);
		source = (u8_t *)&motorTorqueTarget;
    }
    else if (0x6072 == index)
    {
		size = sizeof(motorMaximumTorque);
		source = (u8_t *)&motorMaximumTorque;
    }
    else if (0x6073 == index)
    {
		size = sizeof(motorMaximumCurrent);
		source = (u8_t *)&motorMaximumCurrent;
    }
    else if (0x6075 == index)
    {
		size = sizeof(motorRatedCurrent);
		source = (u8_t *)&motorRatedCurrent;
    }
    else if (0x6076 == index)
    {
		size = sizeof(motorRatedTorque);
		source = (u8_t *)&motorRatedTorque;
    }
    else if (0x6077 == index)
    {
		size = sizeof(motorActualTorque);
		source = (u8_t *)&motorActualTorque;
    }
    else if (0x607F == index)
    {
	    size = sizeof(motorMaximumVelocity);
	    source = (u8_t *)&motorMaximumVelocity;
    }	
    else if (0x6083 == index)
    {
		size = sizeof(motorVelocityAcceleration);
		source = (u8_t *)&motorVelocityAcceleration;
    }
    else if (0x6084 == index)
    {
		size = sizeof(motorVelocityDeceleration);
		source = (u8_t *)&motorVelocityDeceleration;
    }	
    else if (0x6087 == index)
    {
		size = sizeof(motorTorqueSlope);
		source = (u8_t *)&motorTorqueSlope;
    }
    else if (0x60FF == index)
    {
		size = sizeof(motorVelocityTarget);
		source = (u8_t *)&motorVelocityTarget;
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

			if ( (value >= 1) && (value <= 117) )
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
			u32_t command  = getValue(&source[offset], length);

			if ( (0x65766173 == command) )
			{
				u32_t busConfiguration = EEPROM_CAN_CHECK_VALUE;
				busConfiguration <<= 8;
				busConfiguration |= objectNodeOffset;
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
	else if (0x6040 == index)
    {
		if (2 == length)
	    {
			u16_t value = (u16_t)((source[offset+1] << 8) | source[offset]);
		    result = setMotorControlWord(value);
         }
	}
	else if (0x6060 == index)
	{
		if (1 == length)
		{
			motorMode = source[offset];
			result = 1;
		}
	}		
    else if (0x6071 == index)
    {
	    if (2 == length)
	    {
			s16_t value = (u16_t)((source[offset+1] << 8) | source[offset]);
			motorTorqueTarget = value;
			result = 1;
	    }
    }
    else if (0x6072 == index)
    {
	    if (2 == length)
	    {
			u16_t value = (u16_t)((source[offset+1] << 8) | source[offset]);
			motorMaximumTorque = value;
			result = 1;
	    }
    }
    else if (0x6073 == index)
    {
	    if (2 == length)
	    {
			u16_t value = (u16_t)((source[offset+1] << 8) | source[offset]);
			motorMaximumCurrent = value;
			result = 1;
	    }
    }
    else if (0x6075 == index)
    {
	    if (4 == length)
	    {
			u32_t value = (u32_t)getValue(&source[offset], length);
			motorRatedCurrent = value;
			result = 1;
	    }
    }
    else if (0x6076 == index)
    {
	    if (4 == length)
	    {
			u32_t value = (u32_t)getValue(&source[offset], length);
			motorRatedTorque = value;
			result = 1;
	    }
    }
    else if (0x607F == index)
    {
	    if (4 == length)
	    {
		    u32_t value = (u32_t)getValue(&source[offset], length);
		    motorMaximumVelocity = value;
		    result = 1;
	    }
    }
    else if (0x6083 == index)
    {
	    if (4 == length)
	    {
			u32_t value = (u32_t)getValue(&source[offset], length);
			motorVelocityAcceleration = value;
			result = 1;
	    }
    }
    else if (0x6084 == index)
    {
	    if (4 == length)
	    {
			u32_t value = (u32_t)getValue(&source[offset], length);
			motorVelocityDeceleration = value;
			result = 1;
	    }
    }
    else if (0x6087 == index)
    {
	    if (4 == length)
	    {
			u32_t value = (u32_t)getValue(&source[offset], length);
			motorTorqueSlope = value;
			result = 1;
	    }
    }
    else if (0x60FF == index)
    {
	    if (4 == length)
	    {
			s32_t value = (s32_t)getValue(&source[offset], length);
			
			if ( (value >= 0) && (value <= motorMaximumVelocity) )
			{                
				result = 1;
			}
			else if ( (value < 0) && (value >= -motorMaximumVelocity) )
			{
				result = 1;
			}
            
            if ( (0 != result) )
            {
				motorVelocityTarget = value;
                motorTimeLimit = SYSTIME_NOW + ((u32_t)MOTOR_UPDATE_PERIOD * 1000); // triggers immediate evaluation of target value  
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
	can_isp_tx_msg.id.std = CAN_COB(CAN_NMTE_T, deviceNodeId);
	can_isp_tx_msg.dlc = 8;
	
	can_isp_tx_buffer[0] = ((codeA >> 0) & 0xFF);
	can_isp_tx_buffer[1] = ((codeA >> 8) & 0xFF);
	can_isp_tx_buffer[2] = ((codeA >> 16) & 0xFF);
	can_isp_tx_buffer[3] = ((codeA >> 24) & 0xFF);
	can_isp_tx_buffer[4] = ((codeB >> 0) & 0xFF);
	can_isp_tx_buffer[5] = ((codeB >> 8) & 0xFF);
	can_isp_tx_buffer[6] = ((codeB >> 16) & 0xFF);
	can_isp_tx_buffer[7] = ((codeB >> 24) & 0xFF);

    can_isp_send_frame();

	return( 1 );
}

static u32_t sendHeartbeat(u8_t value)
{
	can_isp_tx_msg.id.std = CAN_COB(CAN_NMTE_T, deviceNodeId);
    can_isp_tx_msg.dlc = 1;
    can_isp_tx_buffer[0] = value;
	       
    can_isp_send_frame();
	
	return( 1 );
}

static u32_t sendEmergency(u8_t * faultCode)
{
	u8_t i;
	
	can_isp_tx_msg.id.std = CAN_COB(CAN_EMGY_T, deviceNodeId);
	can_isp_tx_msg.dlc = 8;
	
	for (i=0; i<8; i++)
	{
		can_isp_tx_buffer[i] = faultCode[i];
	}
	
	can_isp_send_frame();
	
	return( 1 );
}

static u32_t sendPdoData(DEVICE_TPDO_MAP * txPdoMap)
{
	u8_t i;
	u8_t offset = 0;

	can_isp_tx_msg.id.std = txPdoMap->cobId;

	for (i = 0; i < txPdoMap->mapCount; i++)
	{
		u16_t mapIndex = (u16_t)((txPdoMap->mappings[i] >> 16) & 0xFFFF);
		u8_t mapSubIndex = (u8_t)((txPdoMap->mappings[i] >> 8) & 0xFF);
		u8_t limit = 8 - offset;
		u32_t length = 0;
		loadDeviceData(mapIndex, mapSubIndex, &can_isp_tx_buffer[offset], &length, limit);
		offset += length;
	}

	can_isp_tx_msg.dlc = offset;

    can_isp_send_frame();
    
    return( 1 );
}

static u32_t sendSdoData(void)
{
	can_isp_tx_msg.id.std = CAN_COB(CAN_TSDO_T, deviceNodeId);
	can_isp_tx_msg.dlc = 8;

	if ( (0 != transferUpload) )
	{
		if ( (0 == transferStarted) )
		{
			if ( (transferLength <= 4) )
			{
				// send data

				u8_t i;

				can_isp_tx_buffer[0] = ((2 << 5) | ((4 - transferLength) << 2) | 3);
				can_isp_tx_buffer[1] = ((transferIndex >> 0) & 0xFF);
				can_isp_tx_buffer[2] = ((transferIndex >> 8) & 0xFF);
				can_isp_tx_buffer[3] = transferSubIndex;

				for (i = 0; i < transferLength; i++)
				{
					if ( (i < transferLength) )
					{
						can_isp_tx_buffer[4 + i] = transferBuffer[i];
					}
					else
					{
						can_isp_tx_buffer[4 + i] = 0x55;
					}
				}

				transferActive = 0;
			}
			else
			{
				// send length

				can_isp_tx_buffer[0] = ((2 << 5) | 1); ;
				can_isp_tx_buffer[1] = ((transferIndex >> 0) & 0xFF);
				can_isp_tx_buffer[2] = ((transferIndex >> 8) & 0xFF);
				can_isp_tx_buffer[3] = transferSubIndex;
				can_isp_tx_buffer[4] = ((transferLength >> 0) & 0xFF);
				can_isp_tx_buffer[5] = ((transferLength >> 8) & 0xFF);
				can_isp_tx_buffer[6] = ((transferLength >> 16) & 0xFF);
				can_isp_tx_buffer[7] = ((transferLength >> 24) & 0xFF);

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

			can_isp_tx_buffer[0] = ((t << 4) | (n << 1) | c);

			for (i = 0; i < 7; i++)
			{
				u8_t ch = 0;

				if ( (i < remaining) )
				{
					ch = transferBuffer[transferOffset + i];
				}

				can_isp_tx_buffer[1 + i] = ch;
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
				can_isp_tx_buffer[0] = 0x60;
				can_isp_tx_buffer[1] = ((transferIndex >> 0) & 0xFF);
				can_isp_tx_buffer[2] = ((transferIndex >> 8) & 0xFF);
				can_isp_tx_buffer[3] = transferSubIndex;
				can_isp_tx_buffer[4] = 0;
				can_isp_tx_buffer[5] = 0;
				can_isp_tx_buffer[6] = 0;
				can_isp_tx_buffer[7] = 0;

				transferActive = 0;
			}
			else
			{
				can_isp_tx_buffer[0] = 0x60;
				can_isp_tx_buffer[1] = ((transferIndex >> 0) & 0xFF);
				can_isp_tx_buffer[2] = ((transferIndex >> 8) & 0xFF);
				can_isp_tx_buffer[3] = transferSubIndex;
				can_isp_tx_buffer[4] = 0;
				can_isp_tx_buffer[5] = 0;
				can_isp_tx_buffer[6] = 0;
				can_isp_tx_buffer[7] = 0;

				transferStarted = 1;
			}
		}
		else
		{
			can_isp_tx_buffer[0] = (0x20 | ((0 != transferToggle) ? 0x10 : 0));
			can_isp_tx_buffer[1] = ((transferIndex >> 0) & 0xFF);
			can_isp_tx_buffer[2] = ((transferIndex >> 8) & 0xFF);
			can_isp_tx_buffer[3] = transferSubIndex;
			can_isp_tx_buffer[4] = 0;
			can_isp_tx_buffer[5] = 0;
			can_isp_tx_buffer[6] = 0;
			can_isp_tx_buffer[7] = 0;
		}
	}

    can_isp_send_frame();
    
    return( 1 );
}

static u32_t sendSdoAbort(u16_t index, u8_t subIndex, u32_t code)
{
	can_isp_tx_msg.id.std = CAN_COB(CAN_TSDO_T, deviceNodeId);
	can_isp_tx_msg.dlc = 8;
	can_isp_tx_buffer[0] = (4 << 5);
	can_isp_tx_buffer[1] = ((index >> 0) & 0xFF);
	can_isp_tx_buffer[2] = ((index >> 8) & 0xFF);
	can_isp_tx_buffer[3] = subIndex;
	can_isp_tx_buffer[4] = ((code >> 0) & 0xFF);
	can_isp_tx_buffer[5] = ((code >> 8) & 0xFF);
	can_isp_tx_buffer[6] = ((code >> 16) & 0xFF);
	can_isp_tx_buffer[7] = ((code >> 24) & 0xFF);

    can_isp_send_frame();
    
    return( 1 );
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
#if 0
static void updateTick(void)
{	
	u8_t nowL = CANTIML;
	u8_t nowH = CANTIMH;
	u16_t now = (nowH << 8) | nowL;
	u16_t diff;
	
	//static int x = 0;
	
	//u32_t a;
	
	if ( (now > tickTimeCounter) )
	{		
		//a = 0;
		diff = now - tickTimeCounter;
	}
	else
	{
		//a = 1;
		diff = (0xFFFF - tickTimeCounter) + now;
	}
	
	if (diff >= 1000)
	{
		tickCount++;
		tickTimeCounter = now;
		
#if 0
		x++;

		if (x > 100)
		{
			x = 0;
			sendDebug(0, diff);
		}
#endif		
		
	}	
}
#endif

static void updateLED(void)
{
    u32_t now = SYSTIME_NOW;
    u32_t diff = SYSTIME_DIFF (ledTimeCounter, now);

    if ( (diff >= 50000) )
    {
		ledTimeCounter = now ;
		ledFlashCount += 50;

		if ( (ledFlashCount > ledFlashLimit) )
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

static void updateMotorState(u8_t operational)
{
	u32_t now = SYSTIME_NOW;
	u32_t difference;
	u32_t limit;
	u16_t tempMotorStatus;
	
	if (NOT_READY_TO_SWITCH_ON == motorState)
	{
		// transition 1
		motorState = SWITCH_ON_DISABLED;
	}
	else if (FAULT_REACTION_ACTIVE == motorState)
	{
		if (0 != deviceFaultCode)
		{
			// load fault error code
			errorStatus = (u32_t)getValue(&deviceFaultCode[4], 4);

			// emit emergency code
			sendEmergency(deviceFaultCode);
		}

		// transition 14
		motorState = NOT_READY_TO_SWITCH_ON_FAULTED;
	}
	
	if (motorDisplayMode != motorMode)
	{
		motorDisplayMode = motorMode;
	}

	if ( (OPERATIONAL_ENABLE == motorState) &&
	     (0 != operational) )
	{
		u8_t motorFaultValue = MotorStatus();
		difference = SYSTIME_DIFF (motorTimeLimit, now);
		limit = (u32_t)MOTOR_UPDATE_PERIOD * 1000;
		
		if ( (0 != motorFaultValue) )
		{
			setDeviceFault(motorFaultCode);
		}
		else if (difference >= limit)
		{
			motorTimeLimit = now;
				
			if ( (3 == motorDisplayMode) )
			{
				u8_t assign;
					
				if ( (motorRequestedVelocity < motorVelocityTarget) )
				{
					// accelerate
					u32_t adjustValue = motorVelocityAcceleration * MOTOR_UPDATE_PERIOD / 1000;
					motorRequestedVelocity += adjustValue;
						
					if ( (motorRequestedVelocity > motorVelocityTarget) )
					{
						motorRequestedVelocity = motorVelocityTarget;
					}
						
					assign = 1;
				}
				else if ( (motorRequestedVelocity > motorVelocityTarget) )
				{
					// decelerate
					u32_t adjustValue = motorVelocityDeceleration * MOTOR_UPDATE_PERIOD / 1000;
					motorRequestedVelocity -= adjustValue;
						
					if ( (motorRequestedVelocity < motorVelocityTarget) )
					{
						motorRequestedVelocity = motorVelocityTarget;
					}

					assign = 1;
				}
				else
				{
					assign = 0;
				}

				if ( (0 != assign) )
				{
					u8_t direction;
					u32_t magnitude;
					u8_t magnitudeH;
					u8_t magnitudeL;

					if ( (motorRequestedVelocity > 0) )
					{
						direction = 0;
						magnitude = motorRequestedVelocity;
					}
					else
					{
						direction = 1;
						magnitude = -motorRequestedVelocity;
					}
					
					magnitude = (magnitude / 50) + 100; // convert 40000 to 800, range is 100 to 900 counts for 0 to 40000 RPM
					magnitudeH = (u8_t)(magnitude >> 8);
					magnitudeL = (u8_t)(magnitude & 0xFF);
						
					//sendDebug(1,magnitude);
					spin(0, magnitudeH, magnitudeL, direction, 0x11);
					motorFreeRequested = 0;
					
					motorActualVelocity = motorRequestedVelocity;
					checkTxPdoMap(0x606C, 0);				
				}
			}
			else if ( (4 == motorDisplayMode) )
			{
				u8_t assign;
				u8_t tempMotorCurrent = MotorCurrent();

				if ( (tempMotorCurrent != motorActualTorque) )
				{
					motorActualTorque = tempMotorCurrent;
					checkTxPdoMap(0x6077, 0);
				}
					
				if ( (motorRequestedCurrent < motorTorqueTarget) )
				{
					// increase
					u32_t adjustValue = motorTorqueSlope * MOTOR_UPDATE_PERIOD / 1000;
					motorRequestedCurrent += adjustValue;
						
					if ( (motorRequestedCurrent > motorTorqueTarget) )
					{
						motorRequestedCurrent = motorTorqueTarget;
					}
					
					assign = 1;
				}
				else if ( (motorRequestedCurrent > motorTorqueTarget) )
				{
					// decrease
					u32_t adjustValue = motorTorqueSlope * MOTOR_UPDATE_PERIOD / 1000;
					motorRequestedCurrent -= adjustValue;
						
					if ( (motorRequestedCurrent < motorTorqueTarget) )
					{
						motorRequestedCurrent = motorTorqueTarget;
					}

					assign = 1;
				}
				else
				{
					assign = 0;
				}

				if ( (0 != assign) )
				{
					u8_t direction;
					u8_t magnitude;

					if ( (motorRequestedCurrent > 0) )
					{
						direction = 0;
						magnitude = (u8_t)(motorRequestedCurrent & 0xFF);
					}
					else
					{
						direction = 1;
						magnitude = (u8_t)((motorRequestedCurrent * -1) & 0xFF);
					}
						
					spin(0, 0, magnitude, direction, 1);
					motorFreeRequested = 0;					

					motorActualTorque = motorRequestedCurrent;
					checkTxPdoMap(0x6077, 0);
				}
			}
			else
			{
				setMotorFreeWheel();
			}
		}
	}
	else
	{
		setMotorFreeWheel();
	}
	
	difference = SYSTIME_DIFF (motorTemperatureTimeLimit, now);
	limit = (u32_t)MOTOR_TEMPERATURE_SAMPLE_PERIOD * 1000;
		
	if (difference >= limit)
	{
		u8_t i;
		u8_t tempMotorTemperature;
		u32_t motorTemperatureTotal = 0;
	
		motorTemperatureSampleBuffer[motorTemperatureSampleIndex++] = MotorTemp();		
		motorTemperatureTimeLimit = SYSTIME_NOW;
		
		if ( (MOTOR_TEMPERATURE_SAMPLE_BUFFER_SIZE == motorTemperatureSampleIndex) )
		{
			motorTemperatureSampleIndex = 0;	
		}

		if ( (motorTemperatureSampleCount < MOTOR_TEMPERATURE_SAMPLE_BUFFER_SIZE) )
		{
			motorTemperatureSampleCount++;
		}
		
		for (i=0; i<motorTemperatureSampleCount; i++)
		{
			motorTemperatureTotal += motorTemperatureSampleBuffer[i];			
		}
		
		tempMotorTemperature = (u8_t)(motorTemperatureTotal / motorTemperatureSampleCount);
	
		if ( (tempMotorTemperature != motorTemperature) )
		{
			motorTemperature = tempMotorTemperature;
			checkTxPdoMap(0x2301, 0);
		}
	}

	tempMotorStatus = getMotorStatusWord();
	
	if (tempMotorStatus != motorStatusWord)
	{
		motorStatusWord = tempMotorStatus;
		checkTxPdoMap(0x6041, 0);
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
	u16_t index = (frame[2] << 8) | frame[1];
	u8_t subIndex = frame[3];
	//u32_t abortCode = (frame[7] << 24) | (frame[6] << 16) | (frame[5] << 8) | frame[4];
	u32_t abortCode = 0;
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
			setPreOperationalState();
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

static void setPreOperationalState(void)
{
	u32_t busConfiguration;
	u8_t i;
	
	deviceState = DEVICE_PREOPERATIONAL_S;
	transferActive = 0;

	objectBitRateCode = 2;
	objectNodeId = 0xFF;
	objectNodeOffset = 0x30;

	if ( (readEEPROM(EEPROM_CAN_CONFIGURATION_ADDRESS, (u8_t*)&busConfiguration, sizeof(busConfiguration)) != 0) )
	{
		u8_t check = ((busConfiguration >> 24) & 0xFF);

		if ( (EEPROM_CAN_CHECK_VALUE == check) )
		{
			objectNodeOffset = (busConfiguration >> 16) & 0xFF;
			objectNodeId = (busConfiguration >> 8) & 0xFF;
			objectBitRateCode = (busConfiguration >> 0) & 0xFF;
		}
	}
	
	deviceNodeId = objectNodeId;

	if ( (0 == deviceNodeId) || (deviceNodeId > 127) )
	{
		u32_t digitInput = readHardwareInput();
		deviceNodeId = objectNodeOffset + (digitInput & 0x3);
	}
	
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
	
	motorTemperature = MotorTemp();
	motorTemperatureTimeLimit = SYSTIME_NOW;
	motorTemperatureSampleIndex = 0;
	motorTemperatureSampleCount = 0;

	motorState = NOT_READY_TO_SWITCH_ON;
	motorControlWord = 0;
	motorStatusWord = getMotorStatusWord();
	motorMode = 0xFF;
	motorDisplayMode = 0xFF;
	motorTimeLimit = SYSTIME_NOW;
	
	motorRatedCurrent = 1000;
	motorMaximumCurrent = 5000;
	motorRatedTorque = 1000;
	motorMaximumTorque = 5000;

	motorMaximumVelocity = 40000;
	motorVelocityAcceleration = 40000;
	motorVelocityDeceleration = 40000;
	motorVelocityTarget = 0;
	motorActualVelocity = 0;

	motorTorqueSlope = 20;
	motorTorqueTarget = 0;
	motorActualTorque = 0;
	
	motorFreeRequested = 0;
	motorRequestedVelocity = 0;
	motorRequestedCurrent = 0;

	if ( (sendHeartbeat(0) != 0) )
	{
		setLedState(LED_PREOPERATIONAL_OK_S);
	}
	else
	{
		sendDebug(0,0); // prevent warning
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
	motorFreeRequested = 0;
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
	updateMotorState(0);

	receiveResult = readCAN(&message);

	if ( (0 != receiveResult) )
	{
		messageType = CAN_TYPE(message.id.std);
		messageDeviceId = CAN_ID(message.id.std);

		if ( (0 == messageDeviceId) || (deviceNodeId == messageDeviceId) )
		{
			if ( (CAN_NMT_T ==  messageType) )
			{
				processNmtMessage(message.pt_data);
			}
			else if ( (CAN_RSDO_T ==  messageType) )
			{
				processSdoMessage(message.pt_data);
			}
		}
	}
}

static void executeOperationalState(void)
{
	u8_t i;
	
	updateLED();
	updateConsumerHeartbeat();
	updateProducerHeartbeat();
	updateMotorState(1);
	
	for (i=0; i<4; i++)
	{
		updateTxPdoMap(&txPdoMapping[i]);
		updateRxPdoMap(&rxPdoMapping[i]);
	}

	receiveResult = readCAN(&message);

	if ( (0 != receiveResult) )
	{
		messageType = CAN_TYPE(message.id.std);
		messageDeviceId = CAN_ID(message.id.std);

		if ( (0 == messageDeviceId) || (deviceNodeId == messageDeviceId) )
		{
			if ( (CAN_NMT_T ==  messageType) )
			{
				processNmtMessage(message.pt_data);
			}
			else if ( (CAN_SYNC_T == messageType) )
			{
				processSyncMessage();
			}
			else if ( (CAN_RPDO1_T ==  messageType) )
			{
				processRxPdoMessage(1, message.pt_data, message.dlc);
			}
			else if ( (CAN_RPDO2_T ==  messageType) )
			{
				processRxPdoMessage(2, message.pt_data, message.dlc);
			}
			else if ( (CAN_RPDO3_T ==  messageType) )
			{
				processRxPdoMessage(3, message.pt_data, message.dlc);
			}
			else if ( (CAN_RPDO4_T ==  messageType) )
			{
				processRxPdoMessage(4, message.pt_data, message.dlc);
			}
			else if ( (CAN_RSDO_T ==  messageType) )
			{
				processSdoMessage(message.pt_data);
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
	updateMotorState(0);

	receiveResult = readCAN(&message);

	if ( (0 != receiveResult) )
	{
		messageType = CAN_TYPE(message.id.std);
		messageDeviceId = CAN_ID(message.id.std);

		if ( (0 == messageDeviceId) || (deviceNodeId == messageDeviceId) )
		{
			if ( (CAN_NMT_T ==  messageType) )
			{
				processNmtMessage(message.pt_data);
			}
		}
	}
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
static void initCANInterface(void)
{
    /* Associate Receive Buffer with Structure */
	message.pt_data = messageBuffer;
	
	/* Set Pre-Operational State */
	setPreOperationalState();
}

static void updateCANInterface(void)
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
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
void can_isp_protocol_init(void)
{
	isp_select_memory = MEM_DEFAULT;
	isp_select_page   = PAGE_DEFAULT;
	isp_start_address = ADD_DEFAULT;
	isp_number_of_bytes = N_DEFAULT;
	isp_prog_on_going = FALSE;
	
    setCANLed(0);

    //- Init Rx data
    can_isp_rx_msg.pt_data = &can_isp_rx_buffer[0];
    loc_buf_index = 0;

    //- Prepare Rx Command
    can_isp_rx_msg.msk.std = 0;//MAX_BASE_ISP_IAP_ID;
    can_isp_rx_msg.cmd = CMD_RX_DATA_MASKED;

    //- Init Tx data
    can_isp_tx_msg.pt_data = &can_isp_tx_buffer[0];

    //- Prepare Tx Command
    can_isp_tx_msg.cmd = CMD_TX_DATA;
    
    // enable interrupts
    sei();
    
    // initialize protocol
    initCANInterface();
}

void can_isp_protocol_task(void)
{
	updateCANInterface();
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */

#endif /* USE_MICROCAN_OPEN */


