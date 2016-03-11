/** ****************************************************************************	
	@file mco_types.h

	@brief Atmel AVR data types for MicroCANopen plus XOD

	@details
	This file contains all of the data types required to support 
	MicroCANopen plus XOD on the Atmel AVR family of microcontrollers
	
	@author John Jayne

	@version 1.00

	@date 02/26/16

	@bug
	
	@todo
	
	@copyright Copyright 2016 ULC Robotics
	
*******************************************************************************/
#ifndef _MCO_TYPES_H
#define _MCO_TYPES_H

#include "stdint.h"

/**************************************************************************
DEFINES: MEMORY TYPE OPTIMIZATION
**************************************************************************/
// CONST Object Dictionary Data
#define MEM_CONST const

// Process data
#define MEM_PROC

// buffers
#define MEM_BUF

// non-process data
#define MEM_FAR


/**************************************************************************
DEFINES: TRUE AND FALSE
**************************************************************************/
#ifndef TRUE
#define TRUE  (1==1)
#endif
#ifndef FALSE
#define FALSE (!TRUE)
#endif
#ifndef NOT_SET
#define NOT_SET 2
#endif


/**************************************************************************
TYPEDEF: CANOPEN DATA TYPES
**************************************************************************/
typedef uint8_t UNSIGNED8;
typedef uint16_t UNSIGNED16;
typedef uint32_t UNSIGNED32;
typedef int8_t INTEGER8;
typedef int16_t INTEGER16;
typedef int32_t INTEGER32;


/**************************************************************************
TYPEDEF: CAN IDENTIFIER TYPE
         Plain CANopen does not use 29-bit IDs, use 16 here for memory
         optimization.
**************************************************************************/
#define CAN_ID_SIZE 32

#if CAN_ID_SIZE == 16
typedef UNSIGNED16 COBID_TYPE;
#define COBID_DISABLED 0x8000U
#define COBID_RTR      0x4000U
#define COBID_EXT      0x2000U
#define COBID_MASK     0xE000U
#elif CAN_ID_SIZE == 32
typedef UNSIGNED32 COBID_TYPE;
#define COBID_DISABLED 0x80000000UL
#define COBID_RTR      0x40000000UL
#define COBID_EXT      0x20000000UL
#define COBID_MASK     0xE0000000UL
#else
#error Only CAN_ID_SIZE 16 or 32 is possible
#endif

#endif  // _MCO_TYPES_H
