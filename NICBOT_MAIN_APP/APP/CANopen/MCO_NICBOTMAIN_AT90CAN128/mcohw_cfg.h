/** ****************************************************************************	
	@file mcohw_cfg.h

	@brief Atmel AVR configuration for MicroCANopen plus XOD

	@details
	This file contains the hardware and software configuration required to  
	support MicroCANopen plus XOD on the Atmel AVR family of microcontrollers
    in this specific application
	
	@author John Jayne

	@version 1.00

	@date 02/26/16

	@bug
	
	@todo
	
	@copyright Copyright 2016 ULC Robotics
	
*******************************************************************************/ 

#ifndef _MCOHW_CFG_H
#define _MCOHW_CFG_H

// CANopen Data Types
#include "mco_types.h"


/**************************************************************************
MACROS: Memory copy and compare to use
**************************************************************************/
#include "string.h"

#define MEM_CPY_FAR(d,s,l) memcpy((void *)d,(void *)s,l)
#define MEM_CPY memcpy
#define MEM_CMP memcmp


/**************************************************************************
MACROS: RTOS SLEEP FUNCTION
**************************************************************************/

// If used in RTOS, allow other tasks to run in blocking function calls
// #define RTOS_SLEEP Sleep(3);
#define RTOS_SLEEP
#define RTOS_LOCK_PI(access,area)
#define RTOS_UNLOCK_PI(access,area)
#define RESOURCE_LOCK(resource) __disable_irq()
#define RESOURCE_UNLOCK(resource) __enable_irq()

/**************************************************************************
DEFINES: HARDWARE PLATFORM
Modify these for your application
**************************************************************************/

#if defined (__AVR_AT90CAN32__)
#define MAX_MOB 14
#define _AT90 1
#elif defined (__AVR_AT90CAN64__)
#define MAX_MOB 14
#define _AT90 1
#elif defined (__AVR_AT90CAN128__)
#define MAX_MOB 14
#define _AT90 1
#else
#define MAX_MOB 5
#define _ATMEGA 1
#endif


/**************************************************************************
DEFINES: BASIC CAN COMMUNICATION
Modify these for your application
**************************************************************************/

// Default CAN bit rate
#define CAN_BITRATE 50

// Use CAN SW Filtering
#define USE_CANSWFILTER 0


/**************************************************************************
DEFINES: CAN HARDWARE DRIVER DEFINITIONS
**************************************************************************/

// if defined a SW FIFO for CAN receive and transmit is used
#define USE_CANFIFO 1

// Tx FIFO depth (must be 0, 4, 8, 16 or 32)
#define TXFIFOSIZE 8

// Rx FIFO depth (must be 0, 4, 8, 16 or 32)
#define RXFIFOSIZE 8

// Manager Rx FIFO depth (must be 0, 4, 8, 16 or 32)
#define MGRFIFOSIZE 0


/**************************************************************************
DEFINES: BOOTLOADER SUPPORT
**************************************************************************/
// If enabled, supports the ESAcademy CANopen boot-loader, copy values
// from boot-loader configuration
#define REBOOT_FLAG_ADR     0x40003FFCUL
#define REBOOT_FLAG         *(volatile UNSIGNED32 *)REBOOT_FLAG_ADR
#define REBOOT_BOOTLOAD_VAL 0x21654387


#endif // _MCOHW_CFG_H

/*----------------------- END OF FILE ----------------------------------*/
