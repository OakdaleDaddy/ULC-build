
#ifndef TYPEDEFS_H
#define TYPEDEFS_H
/***************************************************************************//**
 * @file	typedefs.h
 * @brief	Project related type definitions
 * @version	1.0.0
 * @date		18. July 2014
 * @author	PEAK-SYSTEM TECHNIK
 *
 * Copyright (c): PEAK-SYSTEM TECHNIK GMBH, DARMSTADT
 *****************************************************************************
 *
 *	History: (newer entries first!)
 *
 *	dd-mm-yy/Sgn.			Vers.	changes made
 *
 *	13-Nov-2013/StM		0.0		start of development
 *
 ****************************************************************************/

/*****************************************************************************
                                include files
 ****************************************************************************/
 
/*****************************************************************************
                        type definitions export section
 ****************************************************************************/
//----------------------------------------------------------------------------
// Global type definitions in the PCAN-GPS project
//----------------------------------------------------------------------------
#define sizeofmember(type, member) sizeof(((type *)0)->member)

typedef unsigned char u8_t;
typedef	char b8_t;			// blank
typedef unsigned short u16_t;
typedef unsigned long u32_t;
typedef unsigned long long u64_t;
typedef signed char s8_t;
typedef signed short s16_t;
typedef signed long s32_t;

#ifndef NULL
#define NULL ((void*)0)
#endif

#ifndef FALSE
#define FALSE (0)
#endif

#ifndef TRUE
#define TRUE (1)
#endif
#define	_BV(bit) (1<<(bit))

/***** wrappings for ff *****/
	/* This type MUST be 8 bit */
	typedef u8_t BYTE;

	/* These types MUST be 16 bit */
	typedef s16_t	SHORT;
	typedef u16_t	WORD;
	typedef u16_t	WCHAR;

	/* These types MUST be 16 bit or 32 bit */
	typedef int				INT;
	typedef unsigned int	UINT;

	/* These types MUST be 32 bit */
	typedef s32_t	LONG;
	typedef u32_t	DWORD;
/****************************/


// unions to map the four data types
typedef union {
	u8_t  b[8];
	u16_t w[4];
	u32_t l[2];
	u64_t ll;
} U_LONGLONG;

typedef union {
	u8_t b[4];
	u16_t w[2];
	u32_t l;
} U_LONGWORD;


//<! common return values
#define RET_ERR -1
#define RET_OK 	0

#define ACC_USE_EEPROM_RAW_COMPENSATION_VALUES (1<<0)
#define ACC_USE_EEPROM_FILT_COMPENSATION_VALUES (1<<1)

typedef struct CONFIG_DATA_t{
	struct{
		u8_t cmp_target_x;	//! compenstaion target for accelerometer x-axis
		u8_t cmp_target_y;	//! compenstaion target for accelerometer y-axis
		u8_t cmp_target_z;	//! compenstaion target for accelerometer z-axis
		u8_t cmp_raw_x;		//! compenstaion value for raw accelerometer x-axis
		u8_t cmp_raw_y;		//! compenstaion value for raw accelerometer y-axis
		u8_t cmp_raw_z;		//! compenstaion value for raw accelerometer z-axis
		u8_t cmp_filt_x;	//! compenstaion value for filtered accelerometer x-axis
		u8_t cmp_filt_y;	//! compenstaion value for filtered accelerometer y-axis
		u8_t cmp_filt_z;	//! compenstaion value for filtered accelerometer z-axis
		u8_t range;			//! range selection {1,2,4,8 -> +- 2,4,8,16 g}
		u8_t flags;			//!
	}Acc;

	struct{
		u8_t range;			//! 0=250[dps]; 1=500[dps]; 2=2000[dps]; 3=2000[dps]
	}Gyro;
	
	struct{
		s16_t cmp_x;		//! compenstaion value for x-axis magnitude
		s16_t cmp_y;		//! compenstaion value for y-axis magnitude
		s16_t cmp_z;		//! compenstaion value for z-axis magnitude
		float cmp_fac_x;	//! compenstaion factor for x-axis magnitude
		float cmp_fac_y;	//! compenstaion factor for y-axis magnitude
		float cmp_fac_z;	//! compenstaion factor for z-axis magnitude
	}Mag;

	u32_t crc32;	//! must be last element!
} S_CONFIG_DATA_t;


#endif /* TYPEDEFS_H_ */
