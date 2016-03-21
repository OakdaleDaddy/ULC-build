//_____  I N C L U D E S _______________________________________________________

#include <string.h>

#include "config.h"
#include "can_isp_protocol.h"
#include "isp_lib.h"
#include "reduced_can_lib.h"
#include "can_drv.h"
#include "flash_boot_lib.h"
#include "flash_api_lib.h"
#include "eeprom_lib.h"
#include "ServoControl.h"
#include "CommonFunctions.h"
#include "LED_Control.h"
#include "can_access.h"
#include "can_callbacks.h"

#define EEPROM_CONFIGURATION_ADDRESS (256) /*!< address within EEPROM of configuration value */
#define EEPROM_CHECK_VALUE (0xA5) /*!< unique value to indicate valid values */

#define REPAIR_MODE (0) /*!< value indicating repair mode */
#define INSPECT_MODE (1) /*!< value indicating inspect mode */

#define ACCELEROMETER_SAMPLE_PERIOD (50) /*!< number of milliseconds between samples */
//#define DRILL_SAMPLE_PERIOD (100) // mS
//#define SENSOR_SAMPLE_PERIOD (100) // mS
//#define DEVICE_STATUS_SAMPLE_PERIOD (50) // mS

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

typedef struct
{
   DRILL_STATES state;
   U8 axis;
   U8 control;
   U8 status;
   U16 retractMask;
   S16 manualSetPoint;
   S16 processedSetPoint;
}DRILL_CONTEXT;


static U8 readEEPROM(U16 address, U8 * dest, U8 length);
static U8 writeEEPROM(U16 address, U8 * source, U8 length);
static U16 calculateProcessStatus(void);

#if 0
static u8_t setServoAcceleration(u32_t value);
static u8_t setServoVelocity(u32_t value);
static u8_t setServoPosition(u32_t value);
static u8_t setFrontLaserControl(u8_t status);
static u8_t setRearLaserControl(u8_t status);
#endif

static U32 checkSolenoidControl(U16 solenoidControl);
static U32 checkProcessControl(U16 control);

static void processConfiguration(void);
static void processSolenoidControl(void);
static void processProcessControl(void);

static void updateDeviceStatus(void);
static void updateActualDrillPosition(void);
static void updateRepairControl(void);
static void updateDrillContext(DRILL_CONTEXT * drillContext);
static void updateAccelerometer(void);


static U8 deviceMode; /*!< mode of device, 0=repair, 1=inspect */
static U8 initialSolenoidSet; /*!< indicator of initial solenoid, 0 on boot, 1 after assignment */
static U8 running; /*!< indicator of running mode of device process, 0 when not active, 1 when started */
static U16 deviceProcessControl; /*!< control used to determine device processes */

static U16 accelerometerSampleTimeLimit; /*!< time limit for accelerometer sampling */
//static U16 drillPositionSampleTimeLimit;
//static U16 sensorPositionSampleTimeLimit;

static S16 frontDrillSpeedSetPoint; // RPM
static DRILL_CONTEXT frontDrillContext; /*!< control structure for front drill */
static S16 processedFrontDrillSpeedSetPoint;

static S16 rearDrillSpeedSetPoint; // RPM
static DRILL_CONTEXT rearDrillContext; /*!< control structure for rear drill */
static S16 processedRearDrillSpeedSetPoint;

static SENSOR_HOME_STATES sensorHomingState; /*!< state of sensor homing process */
static U8 sensorHomingActivate; /*!< indicator of sensor homing process activity */

static char * softwareVersion = "v1.02 abc def";

/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */

// read EEPROM memory and stores to destination, return 1 on load of pointer, return 0 on no load
static U8 readEEPROM(U16 address, U8 * dest, U8 length)
{
	U8 i;
	
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
static U8 writeEEPROM(U16 address, U8 * source, U8 length)
{
	U8 i;	

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

static U16 calculateProcessStatus(void)
{
   U16 processStatus = 0;
   U16 inputStatus = IO_Read(0x06);

   // evaluate mode for calculation
   if ( (REPAIR_MODE == deviceMode) )
   {
      // calculate process status
      processStatus = (inputStatus << 8);
      //processStatus |= frontDrillContext.status;
      //processStatus |= (rearDrillContext.status << 3);
   }
   else if ( (INSPECT_MODE == deviceMode) )
   {
      // calculate process status
      processStatus = (inputStatus << 8);
   }

   return(processStatus);
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
#if 0
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
#endif
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
static U32 checkSolenoidControl(U16 solenoidControl)
{
   U32 result = 0;
   // todo, evaluate control and determine if valid
   return(result);
}

static U32 checkProcessControl(U16 control)
{
   U32 result = 0;
   // todo, evaluate control and determine if valid
   return(result);
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
static void processConfiguration(void)
{
   U32 configuration;

   if ( (readEEPROM(EEPROM_CONFIGURATION_ADDRESS, (U8 *)&configuration, sizeof(configuration)) != 0) )
   {
      U8 check = ((configuration >> 24) & 0xFF);

      if ( (EEPROM_CHECK_VALUE == check) )
      {
         U16 bps;

         U8 mode = (configuration >> 16) & 0xFF;;
         U8 nodeId = (configuration >> 8) & 0xFF;
         U8 baudCode = (configuration >> 0) & 0xFF;

         // 0=10K, 1=20K, 2=50K, 3=100K, 4=125K, 5=250K, 6=500K, 7=1M
         if (0 == baudCode)
         {
            bps = 10;
         }
         else if (1 == baudCode)
         {
            bps = 20;
         }
         else if (3 == baudCode)
         {
            bps = 100;
         }
         else if (4 == baudCode)
         {
            bps = 125;
         }
         else if (5 == baudCode)
         {
            bps = 250;
         }
         else if (6 == baudCode)
         {
            bps = 500;
         }
         else if (7 == baudCode)
         {
            bps = 1000;
         }
         else
         {
            bps = 50;
         }

         CAN_WriteProcessImageValue(0x2100, 0, baudCode, sizeof(baudCode));
         CAN_WriteProcessImageValue(0x2101, 0, nodeId, sizeof(nodeId));
         CAN_WriteProcessImageValue(0x2102, 0, mode, sizeof(mode));
         
         CAN_SetNodeId(nodeId);
         CAN_SetBaudrate(bps);

         deviceMode = mode;
      }
   }
}

static void processSolenoidControl(void)
{
   U16 control;
   U8 controlH;
   U8 controlL;

   CAN_ReadProcessImage(0x2304, 0, (U8 *)&control, sizeof(control));
   controlH = (U8)((control >> 8) & 0xFF);
   controlL = (U8)(control & 0xFF);
   
   IO_Write(1, controlL); // group 1
   IO_Write(0, controlH); // group 2

   // generate emergency if assignment fails
}

static void processProcessControl(void)
{
   U16 control;

   CAN_ReadProcessImage(0x2500, 0, (U8 *)&control, sizeof(control));

   deviceProcessControl = control;
   CAN_SendDebug(0xCC, deviceProcessControl);
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
static void updateDeviceStatus(void)
{
   U16 currentProcessStatus;
   U16 calculatedProcessStatus;

   // read current process status
   CAN_ReadProcessImage(0x2501, 0, (U8 *)&currentProcessStatus, sizeof(currentProcessStatus));
    
   // calculation process status
   calculatedProcessStatus = calculateProcessStatus();
   
   // check for difference
   if (calculatedProcessStatus != currentProcessStatus)
   {
      // update process image 
      CAN_WriteProcessImageValue(0x2501, 0, calculatedProcessStatus, sizeof(calculatedProcessStatus));
   }
}

static void updateActualDrillPosition(void)
{
#if 0
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
#endif
}

static void updateRepairControl(void)
{
   if ( (processedFrontDrillSpeedSetPoint != frontDrillSpeedSetPoint) )
   {
      U32 speedCache;
      U8 speedRequest;
      
      // active range 64..127
      speedCache = frontDrillSpeedSetPoint;
      speedCache *= 100;
      speedCache /= 8730;
      speedCache += 64;
      speedRequest = (U8)(speedCache & 0xFF);
      CAN_SendDebug(0x5A, speedRequest);
      
      drillspeed(1, 0x30, speedRequest);
      
      CAN_WriteProcessImageValue(0x2411, 0, frontDrillSpeedSetPoint, sizeof(S16));
      
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
         CAN_SendDebug(0x0001A, 0);
         setServoAcceleration(drillServoAcceleration);
         setServoVelocity(drillServoTravelVelocity);
      }

      CAN_SendDebug(0x0001B, positionRequest);
      setServoPosition(positionRequest);
      servo_move_abs(0);
      
      processedFrontDrillIndexSetPoint = frontDrillIndexSetPoint;
   }
#endif

   if ( (processedRearDrillSpeedSetPoint != rearDrillSpeedSetPoint) )
   {
      U32 speedCache;
      U8 speedRequest;
      
      // active range 64..127
      speedCache = rearDrillSpeedSetPoint;
      speedCache *= 100;
      speedCache /= 8730;
      speedCache += 64;
      speedRequest = (U8)(speedCache & 0xFF);
      CAN_SendDebug(0x5B, speedRequest);

      drillspeed(0, 0x30, speedRequest);
      
      CAN_WriteProcessImageValue(0x2413, 0, rearDrillSpeedSetPoint, sizeof(S16));

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
         CAN_SendDebug(0x0002A, 0);
         setServoAcceleration(drillServoAcceleration);
         setServoVelocity(drillServoTravelVelocity);
      }

      CAN_SendDebug(0x0002B, positionRequest);
      setServoPosition(positionRequest);
      servo_move_abs(1);
      
      processedRearDrillIndexSetPoint = rearDrillIndexSetPoint;
   }
#endif

#if 0
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
#endif   
}

static void updateDrillContext(DRILL_CONTEXT * drillContext)
{
#if 0
   if ( (DRILL_IDLE_S == drillContext->state) )
   {
      if ( ((drillContext->control & 0x04) != 0) )
      {
         drillContext->control &= ~(0x04);
         
         setServoAcceleration(drillServoAcceleration);
         setServoVelocity(drillServoHomingVelocity);
         setServoPosition(20000000);
         servo_move_rel(drillContext->axis);
         
         CAN_SendDebug(0xBC, 1);
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
         CAN_SendDebug(0xCC, positionRequest);
         setServoPosition(positionRequest);
         servo_move_abs(drillContext->axis);
         
         drillContext->processedSetPoint = drillContext->manualSetPoint;
         
         CAN_SendDebug(0xBC, 10);
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
         
         CAN_SendDebug(0xBC, 12);
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
               CAN_SendDebug(0xBC, 11);
               drillContext->status |= 0x02;
               drillContext->state = DRILL_IDLE_S;
            }
            else if ( (drillContext->manualSetPoint != drillContext->processedSetPoint) )
            {
               s32_t positionRequest;
               
               positionRequest = -drillContext->manualSetPoint;
               positionRequest *= drillServoPulsesPerUnit;
               positionRequest /= 100L;
               
               CAN_SendDebug(0xCD, positionRequest);
               
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
               CAN_SendDebug(0xBC, 13);
               drillContext->status |= 0x02;
               drillContext->state = DRILL_IDLE_S;
            }
         }
         else if ( (DRILL_RH_RETRACT_TO_LIMIT_WAIT_S == drillContext->state) )
         {
            if ( ((deviceStatus & drillContext->retractMask) != 0) )
            {
               servo_stop_decel(drillContext->axis);
               
               CAN_SendDebug(0xBC, 2);
               drillContext->state = DRILL_RH_STOP_FROM_RETRACT_WAIT_S;
            }
         }
         else if ( (DRILL_RH_STOP_FROM_RETRACT_WAIT_S == drillContext->state) )
         {
            if ( (servoStatus & 0x04) )
            {
               setServoPosition(-4000000);
               servo_move_rel(drillContext->axis);
               
               CAN_SendDebug(0xBC, 3);
               drillContext->state = DRILL_RH_EXTEND_TO_NOT_LIMIT_WAIT_S;
            }
         }
         else if ( (DRILL_RH_EXTEND_TO_NOT_LIMIT_WAIT_S == drillContext->state) )
         {
            if ( ((deviceStatus & drillContext->retractMask) == 0) )
            {
               servo_stop_decel(drillContext->axis);
               
               CAN_SendDebug(0xBC, 4);
               drillContext->state = DRILL_RH_STOP_FROM_EXTEND_WAIT_S;
            }
         }
         else if ( (DRILL_RH_STOP_FROM_EXTEND_WAIT_S == drillContext->state) )
         {
            if ( (servoStatus & 0x04) )
            {
               setServoPosition(-drillServoHomingBackoffCount);
               servo_move_rel(drillContext->axis);
               
               CAN_SendDebug(0xBC, 5);
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
               
               CAN_SendDebug(0xBC, 6);
               drillContext->status |= 0x02;
               drillContext->state = DRILL_IDLE_S;
            }
         }
      }
   }
#endif
}

static void updateAccelerometer(void)
{
   if ( (CAN_IsTimeExpired(accelerometerSampleTimeLimit) != 0) )
   {
      U16 tempAccelerometerX;
      U16 tempAccelerometerY;
      U16 tempAccelerometerZ;
      
      accelerometerSampleTimeLimit = CAN_GetTime() + ACCELEROMETER_SAMPLE_PERIOD;
      
      tempAccelerometerX = accelerometer_get_x();
      tempAccelerometerX &= 0xFE00; // b8 shows activity when sitting still
      CAN_WriteProcessImageValue(0x2441, 0, tempAccelerometerX, sizeof(tempAccelerometerX));
      
      tempAccelerometerY = accelerometer_get_y();
      tempAccelerometerY &= 0xFE00; // b8 shows activity when sitting still
      CAN_WriteProcessImageValue(0x2442, 0, tempAccelerometerY, sizeof(tempAccelerometerY));

      tempAccelerometerZ = accelerometer_get_z();
      tempAccelerometerZ &= 0xFE00; // b8 shows activity when sitting still
      CAN_WriteProcessImageValue(0x2443, 0, tempAccelerometerZ, sizeof(tempAccelerometerZ));
   }
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* See can_callbacks.h for function description */
U32 CAN_GetSerial(void)
{
	return(0);
}

/* See can_callbacks.h for function description */
void CAN_ReloadFlash(void)
{
   processConfiguration();
}

/* See can_callbacks.h for function description */
void CAN_ResetApplication(void)
{
   WDTCR = (1<<WDCE)|(1<<WDE);  // Shortest period to timeout, Watchdog enabled
   for ( ;; ) ;
}

/* See can_callbacks.h for function description */
void CAN_NMTChange(U8 state)
{
   if (127 == state)
   {
      // pre-operational

      U8 i;

      // initialize cameras
      CAN_WriteProcessImageValue(0x2301, 1, 1, sizeof(U8));
      CAN_WriteProcessImageValue(0x2301, 2, 2, sizeof(U8));
      Camera_Select(1, 2);

      // initialize camera lights
      for (i=0; i<12; i++)
      {
         LED_Intensity(i, 0);
      }

      // check to initialize solenoids
      if (0 == initialSolenoidSet)
      {
         // initialize solenoids
         initialSolenoidSet = 1;
         processSolenoidControl();
      }

      // initialize process control
      CAN_WriteProcessImageValue(0x2500, 0, 0, sizeof(U16));

      // initialize process status
      CAN_WriteProcessImageValue(0x2501, 0, calculateProcessStatus(), sizeof(U16));

      // evaluate mode
      if ( (REPAIR_MODE == deviceMode) )
      {
         servo_reset(0);
         servo_init(0);
   	
         servo_reset(1);
         servo_init(1);
   	
         DrillInit();
         drillspeed(0, 0x30, 0x45);
         drillspeed(1, 0x30, 0x45);
      

         // initialize front drill
      
         frontDrillSpeedSetPoint = 0;

         memset(&frontDrillContext, 0, sizeof(frontDrillContext));
         //frontDrillContext.state = DRILL_IDLE_S;
         frontDrillContext.axis = 1;
         //frontDrillContext.control = 0;
         frontDrillContext.retractMask = 0x0400;
         //frontDrillContext.manualSetPoint = 0;
         //frontDrillContext.processedSetPoint = 0;

      	processedFrontDrillSpeedSetPoint = 0;
      
         CAN_WriteProcessImageValue(0x2311, 0, frontDrillSpeedSetPoint, sizeof(frontDrillSpeedSetPoint));
         CAN_WriteProcessImageValue(0x2312, 0, frontDrillContext.manualSetPoint, sizeof(frontDrillContext.manualSetPoint));
         CAN_WriteProcessImageValue(0x2411, 0, 0, sizeof(S16));
         CAN_WriteProcessImageValue(0x2412, 0, 0, sizeof(S16));


         // initialize rear drill

         rearDrillSpeedSetPoint = 0;

         memset(&rearDrillContext, 0, sizeof(rearDrillContext));
         //rearDrillContext.state = DRILL_IDLE_S;
         //rearDrillContext.axis = 0;
         //rearDrillContext.control = 0;
         rearDrillContext.retractMask = 0x0100;
         //rearDrillContext.manualSetPoint = 0;
         //rearDrillContext.processedSetPoint = 0;

      	processedRearDrillSpeedSetPoint = 0;

         CAN_WriteProcessImageValue(0x2313, 0, rearDrillSpeedSetPoint, sizeof(rearDrillSpeedSetPoint));
         CAN_WriteProcessImageValue(0x2314, 0, rearDrillContext.manualSetPoint, sizeof(rearDrillContext.manualSetPoint));
         CAN_WriteProcessImageValue(0x2413, 0, 0, sizeof(S16));
         CAN_WriteProcessImageValue(0x2414, 0, 0, sizeof(S16));
      }
      else if ( (INSPECT_MODE == deviceMode) )
      {
         servo_reset(0);
         servo_init(0);
      
         sensorHomingState = SENSOR_HOMING_IDLE_S;
         sensorHomingActivate = 0;
      }
   
      
      // initialize accelerometer
      CAN_WriteProcessImageValue(0x2441, 0, 0, sizeof(U16));
      CAN_WriteProcessImageValue(0x2442, 0, 0, sizeof(U16));
      CAN_WriteProcessImageValue(0x2443, 0, 0, sizeof(U16));
      accelerometerSampleTimeLimit = CAN_GetTime();


      // set stopped
      running = 0;
   }
   else if (5 == state)
   {
      // set running
      running = 1;
   }
   else if (4 == state)
   {
      // set stopped
      running = 0;
   }
}

/* See can_callbacks.h for function description */
U32 CAN_ODRead(U16 index, U8 subIndex)
{
   U32 result = CAN_ABORT_UNSUPPORTED;

   // evaluate
   if ( (index >= 0x2311) && (index <= 0x2314) )
   {
      if (REPAIR_MODE == deviceMode)
      {
         result = 0;
      }
   }
   else if ( (index >= 0x2411) && (index <= 0x2414) )
   {
      if (REPAIR_MODE == deviceMode)
      {
         result = 0;
      }
   }
   else
   {
      // all other values are good
      result = 0;
   }
   
   return(result);
}

/* See can_callbacks.h for function description */
U8 CAN_AppSDOReadInit(U8 server, U16 index, U8 subIndex, U32 * totalSize, U32 * size, U8 ** pData)
{   
   U8 result;

   // Unused Parameters
   (void)server;
   (void)subIndex;

   // initialize
   result = 0; 

   // evaluate
   if ( (0x100A == index) )   
   {
      *totalSize = strlen(softwareVersion);
      *pData = (U8 *)softwareVersion;      
      result = 1;
   }

   *size = *totalSize;

   return(result);
}

/* See can_callbacks.h for function description */
void CAN_AppSDOReadComplete(U8 server, U16 index, U8 subIndex, U32 * size)
{
   *size = 0;
}

/* See can_callbacks.h for function description */
U32 CAN_ODWrite(U16 index, U8 subIndex, U8 * data, U8 length)
{
   U32 result = CAN_ABORT_GENERAL;

   // evaluate
   if (0x2100 == index)
   {
      if ( (1 == length) )
      {
         result = 0;
      }
   }
   else if (0x2101 == index)
   {
      U8 value = data[0];

      if ( (1 == length) &&
           (value >= 1) &&
           (value <= 127) )
      {
         result = 0;
      }
   }
   else if (0x2102 == index)
   {
      U8 value = data[0];

      if ( (1 == length) &&
           (value >= 0) &&
           (value <= 1) )
      {
         result = 0;
      }
   }
   else if (0x2105 == index)
   {
      if ( (4 == length) )
      {
         U32 command = 0;
         memcpy(&command, data, length);

         if ( (0x65766173 == command) )
         {
            result = 0;
         }
      }
   }
   else if ((0x2301 == index) &&
            ((1 == subIndex) || (2 == subIndex)))
   {
      if (1 == length)
      {
         U8 value = *data;

         if (value <= 12)
         {
            result = 0;
         }
         else 
         {
            result = CAN_ABORT_VALUE_HIGH;
         }
      }
   }
   else if ((0x2303 == index) &&
            (subIndex >= 1) &&
            (subIndex <= 12))
   {
      if (1 == length)
      {
         result = 0;
      }
   }
   else if (0x2304 == index)
   {
      if (2 == length)
      {
         U16 solenoidControl;
         memcpy(&solenoidControl, data, length);
         result = checkSolenoidControl(solenoidControl);
      }
   }
   else if ((index >= 0x2311) && (index <= 0x2314))
   {
      if ((2 == length) && (REPAIR_MODE == deviceMode))
      {
         result = 0;
      }
   }
   else if ((index >= 0x2411) && (index <= 0x2414))
   {
      // read only
   }
   else if (0x2500 == index)
   {
      if (2 == length)
      {
         U16 processControl;
         memcpy(&processControl, data, length);
         result = checkProcessControl(processControl);
      }
   }
   else if ( (0x2501 == index) )
   {
      // read only
   }
   else
   {
      // all other values are good
      result = 0;
   }

   return(result);
}

/* See can_callbacks.h for function description */
void CAN_ODData(U16 index, U8 subIndex, U8 * data, U8 length)
{
   // evaluate
   if (0x2105 == index)
   {
      U8 baudrate;
      U8 nodeId;
      U8 mode;
      U32 configuration;

      CAN_ReadProcessImage(0x2100, 0, &baudrate, sizeof(baudrate));
      CAN_ReadProcessImage(0x2101, 0, &nodeId, sizeof(nodeId));
      CAN_ReadProcessImage(0x2102, 0, &mode, sizeof(mode));

      configuration = EEPROM_CHECK_VALUE;
      configuration <<= 8;
      configuration |= mode;
      configuration <<= 8;
      configuration |= nodeId;
      configuration <<= 8;
      configuration |= baudrate;

      writeEEPROM(EEPROM_CONFIGURATION_ADDRESS, (U8 *)&configuration, sizeof(configuration));
   }
   else if ((0x2301 == index) && 
            ((1 == subIndex) || (2 == subIndex)))
   {
      U8 selectA;
      U8 selectB;

      CAN_ReadProcessImage(0x2301, 1, &selectA, sizeof(selectA));
      CAN_ReadProcessImage(0x2301, 2, &selectB, sizeof(selectB));
      Camera_Select(selectA, selectB);
   }
   else if ((0x2303 == index) &&
            (subIndex >= 1) &&
            (subIndex <= 12))
   {
      U8 intensity;

      CAN_ReadProcessImage(0x2303, subIndex, &intensity, sizeof(intensity));
      LED_Intensity(subIndex - 1, intensity);	
   }
   else if (0x2304 == index)
   {
      processSolenoidControl();
   }
   else if (0x2311 == index)
   {
      CAN_ReadProcessImage(0x2311, 0, &frontDrillSpeedSetPoint, sizeof(frontDrillSpeedSetPoint));
   }
   else if (0x2312 == index)
   {
      CAN_ReadProcessImage(0x2312, 0, &frontDrillContext.manualSetPoint, sizeof(frontDrillContext.manualSetPoint));
   }
   else if (0x2313 == index)
   {
      CAN_ReadProcessImage(0x2313, 0, &rearDrillSpeedSetPoint, sizeof(rearDrillSpeedSetPoint));
   }
   else if (0x2314 == index)
   {
      CAN_ReadProcessImage(0x2314, 0, &rearDrillContext.manualSetPoint, sizeof(rearDrillContext.manualSetPoint));
   }
   else if (0x2500 == index)
   {
      processProcessControl();
   }
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
__attribute__((optimize("O0")))
int main(void)
{   
   DDRD |= 0b00000000;
   PORTD |= 0b00000011;
   DDRC = 0xF0;
   PORTC = 0x00;
   
   DDRA = 0xff;
   PORTA = 0x00;

   DDRG = 0b00011111;
   PORTG = 0b00000011;
   IO_Write(0,0x00);
   IO_Write(1,0x00);
   DDRD |=0b00000011;
   PORTD &= 0b11111100;
   DDRB |= 0b00100001;
   DDRE |= 0b00111110;
   PORTE |= 0b00000100;
   Init_I2C(); // todo move to setPreOperationalState
   DrillInit(); // todo move to setPreOperationalState
   servo_hard_reset();

   //! --- First of all, disabling the Global Interrupt
   Disable_interrupt();

   //- Pull-up on TxCAN & RxCAN one by one to use bit-addressing
   CAN_PORT_DIR &= ~(1<<CAN_INPUT_PIN );
   CAN_PORT_DIR &= ~(1<<CAN_OUTPUT_PIN);
   CAN_PORT_OUT |=  (1<<CAN_INPUT_PIN );
   CAN_PORT_OUT |=  (1<<CAN_OUTPUT_PIN);

   // initialize accelerometer
   accelerometer_init();
   
   // Initialize CAN Communication
   CAN_ResetCommunication();

   // enable interrupts
   Enable_interrupt();
   
   // process
   for (;;)
   {
      // Update CAN Stack
      CAN_ProcessStack();

      // Update Process
      if (0 != running)
      {
         updateDeviceStatus();

      	if (REPAIR_MODE == deviceMode)
      	{
         	updateActualDrillPosition();
         	updateRepairControl();
         	updateDrillContext(&frontDrillContext);
         	updateDrillContext(&rearDrillContext);
      	}
	      else if (INSPECT_MODE == deviceMode)
	      {
	      }

      	updateAccelerometer();
      }
      else
      {
         updateDeviceStatus();

      	if (REPAIR_MODE == deviceMode)
      	{
         	updateActualDrillPosition();
      	}
      	else if (INSPECT_MODE == deviceMode)
      	{
      	}
      
      	updateAccelerometer();
      }
   }
   
   // End of "main"
   return(0);
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
