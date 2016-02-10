//
//	crc.h
//
//	-------------------------------------------------------------------
//! @addtogroup CRC
//! <h3> API Functions for CRC calculation </h3>
//!
//!
//! <h3> Targets: </h3>
//! - PCAN-GPS
//!
//	-------------------------------------------------------------------
//
//	Copyright (C) 1999-2014  PEAK-System Technik GmbH, Darmstadt
//	more Info at http://www.peak-system.com
//
//! @{

#ifndef CRC_H_
#define CRC_H_

////////////////////////////////////////////////////////////////////////////////
//! @name errors
//! A function returns one of these errors:
//! @{
#define	CRC_ERR_OK				0		//!< OK, no error
#define	CRC_ERR_FAIL			-1		//!< error
#define	CRC_ERR_ILLPARAMVAL		-2		//!< invalid parameter
/*! @}*/

////////////////////////////////////////////////////////////////////////////////
//! @name PRESETS
//! Configuration presets for CRC calculations
//! @{
#ifdef GCC
#define	CRC16_CCITT_CONFIG	{	.seed=0xFFFF, .ply=t_crc16_ccitt, 	\
								.data_rvs=0, .crc_rvs=0, 			\
								.data_1cmp=0, .crc_1cmp=0}

#define	CRC16_CONFIG		{	.seed=0x0000, .ply=t_crc16, 		\
								.data_rvs=1, .crc_rvs=1, 			\
								.data_1cmp=0, .crc_1cmp=0}

#define	CRC32_CONFIG		{	.seed=0xFFFFFFFF, .ply=t_crc32, 	\
								.data_rvs=1, .crc_rvs=1, 			\
								.data_1cmp=0, .crc_1cmp=1}

#else
#define	CRC16_CCITT_CONFIG	{0xFFFF,t_crc16_ccitt,0, 0, 0, 0}
#define	CRC16_CONFIG		{0x0000,t_crc16,1,1,0,0}
#define	CRC32_CONFIG		{0xFFFFFFFF,t_crc32,1,1,0,1}
#endif
/*! @}*/

////////////////////////////////////////////////////////////////////////////////
//! @name types
//! Basetypes for crc functions.
//! @{
#define	CRCStatus_t		s32_t			//!< status type for API functions
/*! @}*/

typedef enum _CRC_DATA_LEN{
	t_crc_8_bit = 0,
	t_crc_16_bit = 1,
	t_crc_32_bit = 2
} E_CRC_DATA_LEN;

typedef enum _CRC_Polynomials{
	t_crc16_ccitt = 0,	//<! use CRC-16 CCITT polynomial x¹⁶+x¹²+x⁵+1 (e.g. HDLC, LAPD, PPP)
	t_crc16 = 1,		//<! use CRC-16 polynomial x¹⁶+x¹⁵+x²+1
	t_crc32 = 2			//<! use CRC-32 polynomial x³²+x²⁶+x²³+x²²+x¹⁶+x¹²+x¹¹+x¹⁰+x⁸+x⁷+x⁵+x⁴+x²+x+1 (e.g. optional in HDLC, PPP)
} E_CRC_Polynomials;

////////////////////////////////////////////////////////////////////////////////
//! @name structures
//! structures for crc functions
//! @{

//! @brief
//! structure for crc initialization
typedef struct {
	u32_t seed;
	E_CRC_Polynomials ply;
	u8_t data_rvs;
	u8_t crc_rvs;
	u8_t data_1cmp;
	u8_t crc_1cmp;
} CRCInit_t;

/*! @}*/

////////////////////////////////////////////////////////////////////////////////
//! @name functions
//! This section will describe the API functions. The functions will target
//! to the main() level.
//! @{

//! @brief
//! Initialize a CRC calculation
//!
//! @param		cfg		structure with setup values for calculation
//!
CRCStatus_t CRC_Init(CRCInit_t *p_cfg);


//! @brief	calculates the CRC sum for a given block of data
//!
//! @param	p_data	pointer to the first data element
//! @param	cnt		number of elements to use for calculation
//! @param	type	type of data (8-, 16- or 32-Bit)
//! @param	p_res	pointer to result buffer (always 32 Bit)
//!
CRCStatus_t CRC_CalcCRC(void *p_data, u32_t cnt, E_CRC_DATA_LEN type, u32_t *p_res);


//! @brief
//! check CRC sum
//!
//! @param	p_data	pointer to the first data element
//! @param	cnt		number of elements to use for calculation
//! @param	type	type of data (8-, 16- or 32-Bit)
//! @param	p_cfg	configuration to use
//! @param	crc		crc to check
//!
//! @return 0 if crc is *not* valid
//! @return 1 if crc is valid
//!
u32_t CRC_Valid(void *p_data, u32_t cnt, u8_t type, CRCInit_t *p_cfg, u32_t crc);


/*! @}*/


#endif /* CRC_H_ */
