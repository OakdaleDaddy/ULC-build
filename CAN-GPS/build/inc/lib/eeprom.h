
#ifndef _EEPROM_H_
#define _EEPROM_H_

//
//	eeprom.h
//
//	-------------------------------------------------------------------
//! @addtogroup EEPROM
//! <h3> API Functions for eeprom access </h3>
//!
//! The functions can be used to access eeprom memory to store non-volatile
//! The functions are not interrupt protected and will target to the main()
//! level. If the user want to write from interrupts he has to build own wrapper
//! functions for interrupt safety. All functions are made in a polling style.
//! So the speed of execution is limited to the physical speed of the
//! communication media. For external EEPROMS this will be typically I2C@400KHz,
//! for internal it is 375kHz. Write data is cached internally until a
//! cache-miss is detected. Cache will be handled internally too. Make a call to
//! EEPROM_FlushCache() to save critical data before it is lost by power removal.
//!
//! <h3> Targets: </h3>
//! - PCAN-GPS
//!
//! <h3> Resources: </h3>
//!
//! <table border="0" width="30%">
//! <tr>
//!   <th bgcolor="#d4e5ff">Target</th>
//!   <th bgcolor="#dfffd4">eeprom size</th>
//!   <th bgcolor="#dfffd4">cache size</th>
//! </tr>
//! <tr>
//!   <th bgcolor="#d4e5ff">PCAN-Router</th>
//!   <th bgcolor="#dfffd4">32 KByte</th>
//!   <th bgcolor="#dfffd4">64 byte</th>
//! </tr>
//! <tr>
//!   <th bgcolor="#d4e5ff">PCAN-Router Pro</th>
//!   <th bgcolor="#dfffd4">128 KByte</th>
//!   <th bgcolor="#dfffd4">128 byte</th>
//! </tr>
//! <tr>
//!   <th bgcolor="#d4e5ff">PCAN-RS-232</th>
//!   <th bgcolor="#dfffd4">32 KByte</th>
//!   <th bgcolor="#dfffd4">64 byte</th>
//! </tr>
//! </table>
//!
//	-------------------------------------------------------------------
//
//	Copyright (C) 1999-2014  PEAK-System Technik GmbH, Darmstadt
//	more Info at http://www.peak-system.com 
//
//! @{


////////////////////////////////////////////////////////////
//! @name errors
//! A function returns one of these errors:
//! @{
#define	EEPROM_ERR_OK			0		//!< OK, no error
#define	EEPROM_ERR_ILLDEVICE	1		//!< device not responding
#define	EEPROM_ERR_ILLPARAMVAL	2		//!< invalid parameter value
#define	EEPROM_ERR_UNKNOWN		3		//!< unknown error
/*! @}*/

////////////////////////////////////////////////////////////
//! @name port-handles
//! Use one of these port handles to access a serial port
//! @{
#define	EEPROM_INT	0 //!< Internal EEPROM
#define	EEPROM_EXT	1 //!< External EEPROM if available. If not undefine!
#undef	EEPROM_EXT	  //!< No external EEPROM present
#ifdef EEPROM_EXT
#define	EEPROM_CNT	2 //!< number of EEPROMs used
#else
#define	EEPROM_CNT	1 //!< number of EEPROMs used
#endif
/*! @}*/

////////////////////////////////////////////////////////////
//! @name types
//! Basetypes for eeprom functions.
//! @{
#define	EEPROMStatus_t		u32_t			//!< status type for API functions
#define	EEPROMHandle_t		u8_t			//!< EEPROM handle type
/*! @}*/


#ifdef __cplusplus
extern "C" {
#endif

////////////////////////////////////////////////////////////
//! @name functions
//! This section will describe the API functions. The functions will target
//! to the main() level.
//! @{



//! @brief
//! Read data from the eeprom.
//!
//! @param		hEeprom		EEPROM handle
//! @param		addr		eeprom address to read from
//! @param		buffer		user buffer to write data to
//! @param		length		number of bytes to read
//!
//! @return		one error of EEPROM_ERR_...
EEPROMStatus_t EEPROM_Read (EEPROMHandle_t hEeprom, u32_t addr, void *buffer, u32_t  length);



//! @brief
//! Write data to the eeprom.
//!
//! @param		hEeprom		EEPROM handle
//! @param		addr			eeprom address to write data to
//! @param		buffer		user buffer to read data from
//! @param		length		number of bytes to write
//!
//! @return		one error of EEPROM_ERR_...
EEPROMStatus_t	EEPROM_Write (EEPROMHandle_t hEeprom, u32_t addr, void *buffer, u32_t length);



//! @brief
//! Flush the cache to the eeprom.
//!
//! @param		hEeprom		EEPROM handle
//!
//! @return		one error of EEPROM_ERR_...
EEPROMStatus_t	EEPROM_FlushCache (EEPROMHandle_t hEeprom);


/*! @}*/

#ifdef __cplusplus
}
#endif

/*! @}*/

#endif

