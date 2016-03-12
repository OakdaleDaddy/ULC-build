/** ****************************************************************************	
	@file mcohw_LEDs.h

	@brief Atmel AVR hardware drivers for MicroCANopen plus

	@details
	This file contains all of the hardware drivers required to support 
	MicroCANopen plus on the Atmel AVR family of microcontrollers
	
	@author John Jayne

	@version 1.00

	@date 02/26/16

	@bug
	
	@todo
	
	@copyright Copyright 2016 ULC Robotics
	
*******************************************************************************/
#ifndef _LED_H
#define _LED_H

#include <avr/io.h>	        /* Microcontroller hardware header file */
#include "mcohw_cfg.h"      /* AVR configuration header file */

#if USE_LEDS

#ifdef __SIMULATION__

#define LED_RUN_ON SimDriver_UpdateLEDState(SIMDRV_RUNLED, 1) 
#define LED_RUN_OFF SimDriver_UpdateLEDState(SIMDRV_RUNLED, 0)
#define LED_ERR_ON SimDriver_UpdateLEDState(SIMDRV_ERRLED, 1) 
#define LED_ERR_OFF SimDriver_UpdateLEDState(SIMDRV_ERRLED, 0) 

#else

#ifdef _AT90

 #define LED_RUN_ON PORTC |= (1 << PINC0)
 #define LED_RUN_OFF PORTC &= ~(1 << PINC0)
 #define LED_ERR_ON PORTC |= (1 << PINC1)
 #define LED_ERR_OFF PORTC &= ~(1 << PINC1)    

#else

  #define LED_RUN_ON 
  #define LED_RUN_OFF 
  #define LED_ERR_ON 
  #define LED_ERR_OFF 

#endif // _AT90

#endif // __SIMULATION__

#endif // USE_LEDS

#endif
