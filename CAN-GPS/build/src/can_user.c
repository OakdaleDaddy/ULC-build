
//------------------------------------------------------------------------------
//
//	Module       : can_user.c
//
//  Project      : PCAN-GPS
//
//  Version/Date : 1.2 , 10/2014
//
//  Copyright (c): PEAK-SYSTEM TECHNIK GMBH, DARMSTADT
//
//------------------------------------------------------------------------------
/*******************************************************************************
 include files
 ******************************************************************************/
//
// System header files
//
#include "math.h"

//
// Library header files
//
#include <system_LPC407x_8x_177x_8x.h>
#include <lpc407x_8x_177x_8x.h>

//
// Source code header files
//
#include "typedefs.h"
#include "hardware.h"
#include "timer.h"
#include "ssp.h"
#include "MEMS_BMC050.h"
#include "MEMS_L3GD20.h"
#include "MAX-7W.h"
#include "can.h"
#include "can_user.h"
#include "eeprom.h"
#include "crc.h"
#include "crc_data.h"
#include "rtc.h"


/*******************************************************************************
 global definitions
 ******************************************************************************/
extern S_CONFIG_DATA_t cfg_data;

/*******************************************************************************
 local definitions
 ******************************************************************************/

#define SYM_BMC_ACCELERATION	0x600
#define SYM_BMC_MAGNETIC_FIELD	0x601

#define SYM_L3GD_ROTATION_01	0x610
#define SYM_L3GD_ROTATION_02	0x611

#define SYM_GPS_STATUS			0x620
#define SYM_GPS_COURSE_SPEED	0x621
#define SYM_GPS_POS_LONGITUDE	0x622
#define SYM_GPS_POS_LATITUDE	0x623
#define SYM_GPS_POS_ALTITUDE	0x624
#define SYM_GPS_DELUSIONS_01	0x625
#define SYM_GPS_DELUSIONS_02	0x626
#define SYM_GPS_DATE_TIME		0x627

#define SYM_IO					0x630

#define SYM_RTC_TIME			0x640


#define SYM_OUT_IO				0x650
#define SYM_OUT_POWEROFF		0x651
#define SYM_OUT_GYRO			0x652
#define SYM_OUT_ACC_SCALE		0x653
#define SYM_OUT_SAVE_CFG		0x654
#define SYM_OUT_RTC_SET_TIME		0x655
#define SYM_OUT_RTC_ADOPT_GPS_TIME	0x656
#define SYM_OUT_ACC_FAST_CALIBRATION	0x657

#define INCOMING_CAN_ID_MIN	0x650
#define INCOMING_CAN_ID_MAX	0x657


#define STORE_ACC_COMPENSATION_PERMANENT 1

/*******************************************************************************
 local function prototypes
 ******************************************************************************/
#ifndef EEPROM_CFG_ADDR
#define EEPROM_CFG_ADDR 0x00
#endif


// Queues for CAN interface
CANMsg_t TxQueueCAN1[CAN_TX_QUEUE_SIZE];
CANMsg_t RxQueueCAN1[CAN_RX_QUEUE_SIZE];

/*******************************************************************************
 global functions
 ******************************************************************************/


//------------------------------------------------------------------------------
//! CANStatus_t CAN_UserWrite(CANMsg_t *pBuff)
//------------------------------------------------------------------------------
//! @brief	Send a message on CAN bus
//------------------------------------------------------------------------------
CANStatus_t CAN_UserWrite(CANMsg_t *pBuff){
	CANStatus_t ret;
	CANMsg_t *pMsg;

	ret = CAN_ERR_OK;

	pMsg = CAN_TxQueueGetNext(CAN_HW_BUS2);

	if (pMsg != NULL) {
		pMsg->Id = pBuff->Id;
		pMsg->Len = pBuff->Len;
		pMsg->Type = pBuff->Type;

		pMsg->Data.Data32[0] = pBuff->Data.Data32[0];
		pMsg->Data.Data32[1] = pBuff->Data.Data32[1];

		// Send Msg
		ret = CAN_TxQueueWriteNext(CAN_HW_BUS2);
	} else {
		// Tx Queue FULL
		ret = CAN_ERR_FAIL;
	}

	return ret;
}

//------------------------------------------------------------------------------
//! u32_t CAN_UserRead(CANMsg_t *pBuff)
//------------------------------------------------------------------------------
//! @brief	Read message from CAN bus
//------------------------------------------------------------------------------
//! @return	 	1 is a message was read
//! 	 		0 otherwise
//------------------------------------------------------------------------------
u32_t CAN_UserRead(CANMsg_t *pBuff) {
	u32_t ret;
	CANMsg_t *pMsg;

	ret = 0;

	pMsg = CAN_RxQueueGetNext(CAN_HW_BUS2);

	if (pMsg != NULL) {
		pBuff->Id = pMsg->Id;
		pBuff->Len = pMsg->Len;
		pBuff->Type = pMsg->Type;

		pBuff->Data.Data32[0] = pMsg->Data.Data32[0];
		pBuff->Data.Data32[1] = pMsg->Data.Data32[1];

		CAN_RxQueueReadNext(CAN_HW_BUS2);
		ret = 1;
	}

	return ret;
}

//------------------------------------------------------------------------------
//! void CAN_UserInit(u32_t bitRateCode)
//------------------------------------------------------------------------------
//! @brief	Initializes CAN bus
//------------------------------------------------------------------------------
u32_t CAN_UserInit(u32_t bitRateCode) 
{
   u32_t result = 1;
   u32_t bitTiming = CAN_BAUD_500K;

   if ( (0 == bitRateCode) )
   {
      bitTiming = CAN_BAUD_10K;
   }
   else if ( (1 == bitRateCode) )
   {
      bitTiming = CAN_BAUD_20K;
   }
   else if ( (3 == bitRateCode) )
   {
      bitTiming = CAN_BAUD_50K;
   }
   else if ( (4 == bitRateCode) )
   {
      bitTiming = CAN_BAUD_125K;
   }
   else if ( (5 == bitRateCode) )
   {
      bitTiming = CAN_BAUD_250K;
   }
   else if ( (6 == bitRateCode) )
   {
      bitTiming = CAN_BAUD_500K;
   }
   else if ( (7 == bitRateCode) )
   {
      bitTiming = CAN_BAUD_1M;
   }

   result &= CAN_SetBusMode(CAN_HW_BUS2, BUS_OFF);              // CAN Bus On

	//! init CAN interface
   result &= CAN_ReferenceTxQueue(CAN_HW_BUS2, &TxQueueCAN1[0], CAN_TX_QUEUE_SIZE); // Reference above Arrays as Queues
   result &= CAN_ReferenceRxQueue(CAN_HW_BUS2, &RxQueueCAN1[0], CAN_RX_QUEUE_SIZE);

   result &= CAN_SetTimestampHandler(CAN_HW_BUS2, NULL);

   result &= CAN_SetErrorLimit(CAN_HW_BUS2, STD_TX_ERRORLIMIT);

   result &= CAN_SetTxErrorCallback(CAN_HW_BUS2, NULL);  // Set ErrorLimit & Callbacks
   result &= CAN_SetRxCallback(CAN_HW_BUS2, NULL);

   result &= CAN_SetChannelInfo(CAN_HW_BUS2, NULL);               // Textinfo is NULL

	// Setup Filters
   result &= CAN_InitFilters();                             // Clear Filter LUT
   result &= CAN_SetFilterMode(AF_ON_BYPASS_ON);         // No Filters ( Bypassed)

	//! init CAN 
   result &= CAN_InitChannel(CAN_HW_BUS2, bitTiming);

	//! bring tranceivers into normal mode
   result &= CAN_SetTransceiverMode(CAN_HW_BUS2, CAN_TRANSCEIVER_MODE_NORMAL);

	NVIC_SetPriority(CAN_IRQn, 0);
	NVIC_EnableIRQ(CAN_IRQn);

	//! Busses on
   result &= CAN_SetBusMode(CAN_HW_BUS2, BUS_ON);               // CAN Bus On

   return (result);
}


//------------------------------------------------------------------------------
//! void CAN_UserSendBMCData(void)
//------------------------------------------------------------------------------
//! @brief	Sends last read data of the BMC050 sensor
//------------------------------------------------------------------------------
void CAN_UserSendBMCData(void){
	CANMsg_t TxMsg;
	u8_t tmp8u=0;

	TxMsg.Id  = SYM_BMC_ACCELERATION;
	TxMsg.Len = 8;
	TxMsg.Type = CAN_MSG_STANDARD;
	TxMsg.Data.Data16[0] = BMC050_Readings.Acceleration_X;
	TxMsg.Data.Data16[1] = BMC050_Readings.Acceleration_Y;
	TxMsg.Data.Data16[2] = BMC050_Readings.Acceleration_Z;
	TxMsg.Data.Data8[6] = BMC050_Readings.Temperature;
	MEMS_BMC050_GetVertialAxis(&tmp8u);
	TxMsg.Data.Data8[7] = (tmp8u&0x3) | (BMC050_Readings.orientation&0x7)<<2;
	CAN_UserWrite(&TxMsg);

	TxMsg.Id  = SYM_BMC_MAGNETIC_FIELD;
	TxMsg.Len = 6;
	TxMsg.Type = CAN_MSG_STANDARD;
	TxMsg.Data.Data16[0] = BMC050_Readings.MagField_X;
	TxMsg.Data.Data16[1] = BMC050_Readings.MagField_Y;
	TxMsg.Data.Data16[2] = BMC050_Readings.MagField_Z;
	CAN_UserWrite(&TxMsg);

	// allow task for reading new values
	BMC050_Readings.data_valid = FALSE;

	return;
}

//------------------------------------------------------------------------------
//! void CAN_UserSendL3GDData(void)
//------------------------------------------------------------------------------
//! @brief	Sends last read data of the L3GD20 sensor
//------------------------------------------------------------------------------
void CAN_UserSendL3GDData(void){
	CANMsg_t TxMsg;

	TxMsg.Id = SYM_L3GD_ROTATION_01;
	TxMsg.Len = 8;
	TxMsg.Type = CAN_MSG_STANDARD;
	TxMsg.Data.DataFlt[0] = L3GD20_Readings.Gyro_X;
	TxMsg.Data.DataFlt[1] = L3GD20_Readings.Gyro_Y;
	CAN_UserWrite(&TxMsg);

	TxMsg.Id = SYM_L3GD_ROTATION_02;
	TxMsg.Len = 4;
	TxMsg.Type = CAN_MSG_STANDARD;
	TxMsg.Data.DataFlt[0] = L3GD20_Readings.Gyro_Z;
	CAN_UserWrite(&TxMsg);

	return;
}

//------------------------------------------------------------------------------
//! void CAN_UserSendGPSData(void)
//------------------------------------------------------------------------------
//! @brief	Sends latest GPS data
//------------------------------------------------------------------------------
void CAN_UserSendGPSData(void){
	CANMsg_t TxMsg;
	double frc_tmp, int_tmp;

	TxMsg.Id = SYM_GPS_STATUS;
	TxMsg.Len = 3;
	TxMsg.Type = CAN_MSG_STANDARD;
	TxMsg.Data.Data8[0] = MAX7W_Readings.Gps_AntennaStatus; // (0=INIT, 1=DONTKNOW, 2=OK, 3=SHORT, 4=OPEN)
	TxMsg.Data.Data8[1] = MAX7W_Readings.Nav_NumSatellites;
	TxMsg.Data.Data8[2] = MAX7W_Readings.Nav_Method;      	// None / 2D / 3D
	CAN_UserWrite(&TxMsg);

	TxMsg.Id = SYM_GPS_COURSE_SPEED;
	TxMsg.Len = 8;
	TxMsg.Type = CAN_MSG_STANDARD;
	TxMsg.Data.DataFlt[0] = MAX7W_Readings.Nav_CourseOverGround;
	TxMsg.Data.DataFlt[1] = MAX7W_Readings.Nav_SpeedOverGroundKmh;
	CAN_UserWrite(&TxMsg);

	TxMsg.Id = SYM_GPS_POS_LONGITUDE;
	TxMsg.Len = 7;
	TxMsg.Type = CAN_MSG_STANDARD;
	frc_tmp = modf((MAX7W_Readings.Pos_Longitude/100), &int_tmp);
	TxMsg.Data.DataFlt[0]= (float) frc_tmp*100;
	TxMsg.Data.Data16[2] = (u16_t)int_tmp;
	TxMsg.Data.Data8[6] = MAX7W_Readings.Pos_LongitudeIndEW;
	CAN_UserWrite(&TxMsg);

	TxMsg.Id = SYM_GPS_POS_LATITUDE;
	TxMsg.Len = 7;
	TxMsg.Type = CAN_MSG_STANDARD;
	frc_tmp = modf((MAX7W_Readings.Pos_Latitude/100), &int_tmp);
	TxMsg.Data.DataFlt[0]= (float) frc_tmp*100;
	TxMsg.Data.Data16[2] = (u16_t)int_tmp;
	TxMsg.Data.Data8[6] = MAX7W_Readings.Pos_LatitudeIndNS;
	CAN_UserWrite(&TxMsg);

	TxMsg.Id = SYM_GPS_POS_ALTITUDE;
	TxMsg.Len = 4;
	TxMsg.Type = CAN_MSG_STANDARD;
	TxMsg.Data.DataFlt[0] = MAX7W_Readings.Pos_AltitudeOverSea;
	CAN_UserWrite(&TxMsg);

	TxMsg.Id = SYM_GPS_DELUSIONS_01;
	TxMsg.Len = 8;
	TxMsg.Type = CAN_MSG_STANDARD;
	TxMsg.Data.DataFlt[0] = MAX7W_Readings.PDOP;
	TxMsg.Data.DataFlt[1] = MAX7W_Readings.HDOP;
	CAN_UserWrite(&TxMsg);

	TxMsg.Id = SYM_GPS_DELUSIONS_02;
	TxMsg.Len = 4;
	TxMsg.Type = CAN_MSG_STANDARD;
	TxMsg.Data.DataFlt[0] = MAX7W_Readings.VDOP;
	CAN_UserWrite(&TxMsg);

	TxMsg.Id = SYM_GPS_DATE_TIME;
	TxMsg.Len = 6;
	TxMsg.Type = CAN_MSG_STANDARD;
	TxMsg.Data.Data8[0] = MAX7W_Readings.Date_Year;
	TxMsg.Data.Data8[1] = MAX7W_Readings.Date_Month;
	TxMsg.Data.Data8[2] = MAX7W_Readings.Date_DayOfMonth;
	TxMsg.Data.Data8[3] = MAX7W_Readings.Time_Hrs;
	TxMsg.Data.Data8[4] = MAX7W_Readings.Time_Min;
	TxMsg.Data.Data8[5] = MAX7W_Readings.Time_Sec;
	CAN_UserWrite(&TxMsg);

	return;
}


//------------------------------------------------------------------------------
//! void CAN_UserSendDioData(void)
//------------------------------------------------------------------------------
//! @brief	Sends current IO values
//------------------------------------------------------------------------------
void CAN_UserSendDioData(void){
	CANMsg_t TxMsg;
	u32_t tmp32=0;
	u8_t tmp8=0;

	TxMsg.Id = SYM_IO;
	TxMsg.Len = 1;
	TxMsg.Type = CAN_MSG_STANDARD;
	TxMsg.Data.Data8[0] = tmp8;

	HW_GetDIN(&tmp32);
	TxMsg.Data.Data8[0] = (u8_t) (tmp32&0x3);

	HW_GetDOUT(&tmp32);
	TxMsg.Data.Data8[0] |= (u8_t) (tmp32&0x1)<<2;

	HW_SDCardPresent(&tmp8);
	TxMsg.Data.Data8[0] |= (tmp8&0x1)<<3;

	tmp8 = HW_GPS_GetPowerState();
	TxMsg.Data.Data8[0] |= (tmp8&0x1)<<4;

	HW_GetModuleID(&tmp8);
	TxMsg.Data.Data8[0] |= (tmp8&0x7)<<5;

	CAN_UserWrite(&TxMsg);

	return;
}


//------------------------------------------------------------------------------
//! void CAN_UserSendRTCTime(void)
//------------------------------------------------------------------------------
//! @brief	Sends current RTC time
//------------------------------------------------------------------------------
void CAN_UserSendRTCTime(void){
	RTC rtc;
	CANMsg_t TxMsg;

 	rtc_gettime(&rtc);

	TxMsg.Id = SYM_RTC_TIME;
	TxMsg.Len = 8;
	TxMsg.Type = CAN_MSG_STANDARD;
	TxMsg.Data.Data8[0] = rtc.sec;
	TxMsg.Data.Data8[1] = rtc.min;
	TxMsg.Data.Data8[2] = rtc.hour;
	TxMsg.Data.Data8[3] = rtc.wday;
	TxMsg.Data.Data8[4] = rtc.mday;
	TxMsg.Data.Data8[5] = rtc.month;
	TxMsg.Data.Data16[3] = rtc.year;

	CAN_UserWrite(&TxMsg);

	return;
}



//------------------------------------------------------------------------------
//! void CAN_UserProcessMsg(void)
//------------------------------------------------------------------------------
//! @brief	Processes incoming CAN messages
//------------------------------------------------------------------------------
void CAN_UserProcessMsg(void){
	RTC rtc;
	CANMsg_t msg;
	u8_t res=0;
	static u8_t toggle_led=0;
	CRCInit_t cfg= CRC32_CONFIG;
	u32_t cnt=0;


	res = CAN_UserRead(&msg);
	if(!res)
		return;

#if 0
	if(toggle_led){
		HW_SetLED (HW_LED_STATUS_2, HW_LED_GREEN);
		toggle_led = 0;
	}else{
		HW_SetLED (HW_LED_STATUS_2, HW_LED_ORANGE);
		toggle_led = 1;
	}
#endif

	if(msg.Id<INCOMING_CAN_ID_MIN || msg.Id>INCOMING_CAN_ID_MAX)
		return;

	switch(msg.Id){
	case SYM_OUT_IO:
		HW_SetDOUTn(HW_DOUT_1, (msg.Data.Data8[0] & 0x01));

		if (msg.Data.Data8[0] & 0x02)
			HW_GPS_PowerOn();
		else
			HW_GPS_PowerOff();
		break;

	case SYM_OUT_POWEROFF:
		if(msg.Data.Data8[0]&0x01)
			HW_SwitchOFF();
		break;

	case SYM_OUT_GYRO:
		MEMS_L3GD20_SetRange(msg.Data.Data8[0]&0x03);
		break;

	case SYM_OUT_ACC_SCALE:
		MEMS_BMC050_SetAccRange(msg.Data.Data8[0]&0x7);
		break;

	case SYM_OUT_SAVE_CFG:
		// only write configuration if LSB in lowest byte is set
		if(!(msg.Data.Data8[0]&0x1))
			break;

		MEMS_BMC050_GetAccCalTargets(	&cfg_data.Acc.cmp_target_x,
										&cfg_data.Acc.cmp_target_y,
										&cfg_data.Acc.cmp_target_z);
		MEMS_BMC050_GetAccRange(&cfg_data.Acc.range);
#if STORE_ACC_COMPENSATION_PERMANENT
		cfg_data.Acc.flags |= ACC_USE_EEPROM_RAW_COMPENSATION_VALUES;
		cfg_data.Acc.flags |= ACC_USE_EEPROM_FILT_COMPENSATION_VALUES;

		MEMS_BMC050_GetAccCalFiltValues(&cfg_data.Acc.cmp_filt_x,
										&cfg_data.Acc.cmp_filt_y,
										&cfg_data.Acc.cmp_filt_z);
		MEMS_BMC050_GetAccCalRawValues(	&cfg_data.Acc.cmp_raw_x,
										&cfg_data.Acc.cmp_raw_y,
										&cfg_data.Acc.cmp_raw_z);
#else
		cfg_data.Acc.flags = 0;
#endif

		MEMS_L3GD20_GetRange(&cfg_data.Gyro.range);

		res = CRC_Init(&cfg);
		if(res != CRC_ERR_OK)
			break;

		cnt = sizeof(cfg_data)-sizeofmember(S_CONFIG_DATA_t, crc32);
		res = CRC_CalcCRC((void*) &cfg_data, cnt, t_crc_8_bit, &cfg_data.crc32);
		if(res != CRC_ERR_OK)
					break;

		if(EEPROM_Write(EEPROM_INT, EEPROM_CFG_ADDR, &cfg_data,
				sizeof(cfg_data))!= EEPROM_ERR_OK){
			// write failed
		}
		if(EEPROM_FlushCache(EEPROM_INT)!= EEPROM_ERR_OK){
			// flush failed
		}
		break;

	case SYM_OUT_RTC_SET_TIME:
		rtc.sec = msg.Data.Data8[0] ;
		rtc.min	= msg.Data.Data8[1] ;
		rtc.hour = msg.Data.Data8[2] ;
		rtc.wday = msg.Data.Data8[3] ;
		rtc.mday = msg.Data.Data8[4] ;
		rtc.month = msg.Data.Data8[5] ;
		rtc.year = msg.Data.Data16[3] ;
		rtc_settime(&rtc);
		break;

	case SYM_OUT_RTC_ADOPT_GPS_TIME:
		if(!(msg.Data.Data8[0]&0x1))
			break;

		// only copy values if they are valid
		if((MAX7W_Readings.Validity & GPS_TIME_VALID)!=GPS_TIME_VALID)
			break;
		if((MAX7W_Readings.Validity & GPS_DATE_VALID)!=GPS_DATE_VALID)
			break;

		// get old values to keep day of week that was set before
		rtc_gettime(&rtc);

		rtc.sec = MAX7W_Readings.Time_Sec;
		rtc.min	= MAX7W_Readings.Time_Min;
		rtc.hour = MAX7W_Readings.Time_Hrs;
		rtc.mday = MAX7W_Readings.Date_DayOfMonth;
		rtc.month = MAX7W_Readings.Date_Month;
		rtc.year = MAX7W_Readings.Date_Year;
		rtc_settime(&rtc);
		break;

	case SYM_OUT_ACC_FAST_CALIBRATION:
		MEMS_BMC050_SetAccCalTargets(&msg.Data.Data8[0], &msg.Data.Data8[1], &msg.Data.Data8[2]);
		if(msg.Data.Data8[3]&0x1)
			MEMS_BMC050_StartFastAccCompensation();
		break;

	default:
		break;
	}

	return;
}


//! Update the explorer panel with currently set config values
void CAN_UserResetPanelValues(void){
	CANMsg_t TxMsg;
	u32_t tmp_u32;
	RTC rtc;
	
	HW_GetDOUT(&tmp_u32);

	TxMsg.Id = SYM_OUT_IO;
	TxMsg.Len = 1;
	TxMsg.Type = CAN_MSG_STANDARD;
	TxMsg.Data.Data8[0] =  (HW_GPS_GetPowerState()<<1) | ((u8_t)tmp_u32) ;
	CAN_UserWrite(&TxMsg);

	TxMsg.Id = SYM_OUT_GYRO;
	TxMsg.Len = 1;
	MEMS_L3GD20_GetRange(&TxMsg.Data.Data8[0]);
	CAN_UserWrite(&TxMsg);
	
	TxMsg.Id = SYM_OUT_ACC_SCALE;
	TxMsg.Len = 1;
	MEMS_BMC050_GetAccRange(&TxMsg.Data.Data8[0]);
	CAN_UserWrite(&TxMsg);

	TxMsg.Id = SYM_OUT_ACC_FAST_CALIBRATION;
	TxMsg.Len = 4;
	MEMS_BMC050_GetAccCalTargets(&TxMsg.Data.Data8[0],
			&TxMsg.Data.Data8[1],
			&TxMsg.Data.Data8[2]);
	TxMsg.Data.Data8[3] = 0;
	CAN_UserWrite(&TxMsg);

	rtc_gettime(&rtc);
	TxMsg.Id = SYM_OUT_RTC_SET_TIME;
	TxMsg.Len = 8;
	TxMsg.Data.Data8[0] = rtc.sec;
	TxMsg.Data.Data8[1] = rtc.min;
	TxMsg.Data.Data8[2] = rtc.hour;
	TxMsg.Data.Data8[3] = rtc.wday;
	TxMsg.Data.Data8[4] = rtc.mday;
	TxMsg.Data.Data8[5] = rtc.month;
	TxMsg.Data.Data16[3] = rtc.year;
	CAN_UserWrite(&TxMsg);
	
	return;
}

/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */

#if 0

static u8_t canNodeId;

void CAN_SetNodeId(u8_t nodeId)
{
   canNodeId = nodeId;
}

void CAN_SendHeartbeat(u8_t value)
{
   CANMsg_t TxMsg;
 
   TxMsg.Id = CAN_COB(CAN_ERROR_T, canNodeId);
   TxMsg.Len = 1;
   TxMsg.Type = CAN_MSG_STANDARD;
   TxMsg.Data.Data8[0] = value;

   CAN_UserWrite(&TxMsg);
}
#endif
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
