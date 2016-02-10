
/***************************************************************************//**
* @file		MAX-7W.h
* @brief	Functions for MAX 7W GPS Module
* @version	1.0.0
* @date		18. July 2014
* @author	PEAK-SYSTEM TECHNIK
*
* Copyright (c): PEAK-SYSTEM TECHNIK GMBH, DARMSTADT
* *****************************************************************************/
#ifndef UBLOX_MAX7W_H_
#define UBLOX_MAX7W_H_


/*****************************************************************************
                                 include files
 ****************************************************************************/

/*****************************************************************************
                             global data definitions
 ****************************************************************************/

#define GPS_TIME_VALID (1ul<<0)
#define GPS_DATE_VALID (1ul<<1)

typedef struct
{
		 u8_t Gps_AntennaStatus; 		// (0=INIT, 1=DONTKNOW, 2=OK, 3=SHORT, 4=OPEN)
		 u8_t Nav_NumSatellites;		
		 u8_t Nav_Method;      			// None / 2D / 3D
		 u8_t dummy;
		float Nav_SpeedOverGroundKmh;
		float Nav_CourseOverGround;
		float Pos_Longitude;				// left or right of Greenwich (ger: "Meridian")
		 u8_t Pos_LongitudeIndEW;
		float Pos_Latitude;					// above or below the equator (ger. "Deklination")
		 u8_t Pos_LatitudeIndNS;
		float Pos_AltitudeOverSea;	// +/- 23m accuracy !!
		float PDOP;	//
		float HDOP;	//
		float VDOP;	//
		 u8_t Time_Hrs;
		 u8_t Time_Min;
		 u8_t Time_Sec;
		 u8_t Date_Year;
		 u8_t Date_Month;
		 u8_t Date_DayOfMonth;
		 u32_t Validity;
} UBLOX_MAX7W_Result_Type;

extern UBLOX_MAX7W_Result_Type MAX7W_Readings;

/*****************************************************************************
                           global function prototypes
 ****************************************************************************/

void Init_UART2(void);

s32_t UBLOX_MAX7W_init(void);

void UBLOX_MAX7W_task(void);

#endif /* UBLOX_MAX7W_H_ */
