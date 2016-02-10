#ifndef _HARDWARE_H_
#define _HARDWARE_H_

//---------------------------------------------------------------------
//
//	Module       : HARDWARE.H
//
//  Project      : PCAN-GPS
//
//  Version/Date : 1.0 , 09/2013  
//
//  Copyright (c): PEAK-SYSTEM TECHNIK GMBH, DARMSTADT
//
//---------------------------------------------------------------------

//#define	WAIT_10NOP			{u32_t  iwait;  for ( iwait = 0; iwait < 22; iwait++){}}

// GLOBALS
#define	CPU_QUARTZ_HZ	 12000000UL 		/* 12 [MHz] */
#define PCLK			120000000UL			/* PCLK frequency [Hz] */

extern void ResetISR(void);

////////////////////////////////////////////////////////////
//! @name errors
//! A function returns one of these errors:
//! @{
#define	HW_ERR_OK			0		//!< OK, no error
#define	HW_ERR_ILLPARAMVAL	1		//!< invalid parameter value
#define	HW_ERR_RESOURCE		2		//!< hardware resource is not available
/*! @}*/


////////////////////////////////////////////////////////////
//! @name LED-handles
//! Handles to the LEDs. Not all LEDs are available on all targets.
//! @{
#define	HW_LED_STATUS_1	1		//!< status LED 1
#define	HW_LED_STATUS_2	2		//!< status LED 2
/*! @}*/

////////////////////////////////////////////////////////////
//! @name Din-handles
//! Handles to the digital input pins
//! @{
#define	HW_DIN_1		0		//!< Din 1
#define	HW_DIN_2		1		//!< Din 2
/*! @}*/

////////////////////////////////////////////////////////////
//! @name Din-handles
//! Handles to the digital output pins
//! @{
#define	HW_DOUT_1		0		//!< Dout 1
/*! @}*/

////////////////////////////////////////////////////////////
//! @name LED-colors
//! Colors used for the LEDs
//! @{
#define	HW_LED_OFF			0		//!< LED switch OFF
#define	HW_LED_RED			1		//!< LED switch red
#define	HW_LED_GREEN		2		//!< LED switch green
#define	HW_LED_ORANGE		3		//!< LED switch orange
/*! @}*/


////////////////////////////////////////////////////////////
//! @name types
//! Basetypes for hardware functions
//! @{
#define	HWStatus_t		u32_t			//!< status code type for API functions
#define	LEDHandle_t		u8_t			//!< handle type
#define	DinHandle_t		u8_t			//!< handle type
#define	DoutHandle_t	u8_t			//!< handle type
#define	LEDColor_t		u8_t			//!< color type
/*! @}*/


////////////////////////////////////////////////////////////
//! @name functions
//! This section will describe the API functions. The functions will target
//! to the main() level.
//! @{

//! @brief
//! Initialize hardware (port pins, cpu internals etc.). Call this on
//! main() entry.
//!
//! @return		one error of HW_ERR_...
HWStatus_t		HW_Init (void);

//! @brief
//! Retrieve the module ID from the solder pins or coding switch.
//!
//! @param		*buffer		Buffer for the ID
//!
//! @return		one error of HW_ERR_...
HWStatus_t		HW_GetModuleID (u8_t  *buffer);


//! @brief
//! Check is SD card is present.
//!
//! @param		*buffer		Buffer for the status
//!
//! @return		one error of HW_ERR_...
HWStatus_t  HW_SDCardPresent(u8_t * buffer);


//! @brief
//! Read digital inputs. Each bit will represent a digital pin.
//! Not available on PCAN-Router Pro
//!
//! @param		hDin			handle of the Din, see Din-handles
//! @param		*buffer		Buffer for DIN-value
//!
//! @return		one error of HW_ERR_...
HWStatus_t  HW_GetDINn (DinHandle_t hDin, u32_t *buffer);

//! @brief
//! Read digital inputs. Each bit will represent a digital pin.
//! Not available on PCAN-Router Pro
//!
//! @param		*buffer		Buffer for DIN-value
//!
//! @return		one error of HW_ERR_...
HWStatus_t  HW_GetDIN (u32_t  *buffer);

//! @brief
//! Set single digital output.
//! 
//! @param		hDout		handle of the Dout, see Dout-handles
//! @param		value		value to set
//!
//! @return		one error of HW_ERR_...
HWStatus_t  HW_SetDOUTn (DoutHandle_t hDout, u8_t  value);

//! @brief
//! Set digital outputs. Each bit will represent a digital pin.
//! 
//! @param		*buffer		Buffer for DOUT-value
//!
//! @return		one error of HW_ERR_...
HWStatus_t  HW_SetDOUT (u32_t  *buffer);

//! @brief
//! Read digital outputs. Each bit will represent a digital pin.
//!
//! @param		*buffer		Buffer for DOUT-value
//!
//! @return		one error of HW_ERR_...
HWStatus_t  HW_GetDOUT (u32_t  *buffer);

//! @brief
//! Switch a LED ON or OFF.
//!
//! @param		hLED		handle of the LED, see LED-handles
//! @param		color		see HW_LED_ON ...
//!
//! @return		one error of HW_ERR_...
HWStatus_t		HW_SetLED (	LEDHandle_t  hLED, LEDColor_t  color);

//! @brief
//! Jumps to the Bootloader. This function will never return. Using '0' for
//! the baudrate will instruct the Bootloader to use the default baudrate.
//!
//! @param		baudrate		baudrate for the bootloader, see can_user.h
//!
//! @return		this function will never return
void		HW_JumpToBootloader ( u32_t  baudrate);

//! @brief
//! Switch OFF the hardware. This function will never return.
void		HW_SwitchOFF ( void);


//! @brief	activates power supply for GPS module
void HW_GPS_PowerOn(void);

//! @brief	switches off power supply for GPS module
void HW_GPS_PowerOff(void);

//! @brief	reads current state of GPS power sypply
//!
//! @return 1 if power is switched on
u8_t HW_GPS_GetPowerState(void);

//! @brief	activates chip select of acceleration part of BMC50/BMC150 sensor
void HW_BMC_CsAccelerationOn(void);

//! @brief	deactivates chip select of acceleration part of BMC50/BMC150 sensor
void HW_BMC_CsAccelerationOff(void);

//! @brief	activates chip select of magnetometer part of BMC50/BMC150 sensor
void HW_BMC_CsMagnetometerOn(void);

//! @brief	deactivates chip select of magnetometer part of BMC50/BMC150
void HW_BMC_CsMagnetometerOff(void);

//! @brief	activates chip select of gyroscope
void HW_L3GD20_CsOn(void);

//! @brief	deactivates chip select of gyroscope
void HW_L3GD20_CsOff(void);




/*! @}*/

#endif
