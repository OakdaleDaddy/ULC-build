/**************************************************************************
PROJECT:     CANopen Bootloader for Atmel AT90CAN128 AVR Microcontroller
MODULE:      TYPES.H
CONTAINS:    Include file for global type and constant definitions
ENVIRONMENT: Compiled and tested with IAR EW for AVR on
             Atmel STK500/501 with AT90CAN128
             Compiler Version 5.11b
COPYRIGHT:   This version Embedded Systems Academy, Inc.
             Developed by Embedded Systems Academy, Inc.
VERSION:     $LastChangedDate: 2013-05-28 02:11:48 +0200 (Tue, 28 May 2013) $
             $LastChangedRevision: 2624 $
***************************************************************************/

typedef signed char    INTEGER8;
typedef signed int     INTEGER16;
typedef signed long    INTEGER32;
typedef unsigned char  UNSIGNED8;
typedef unsigned int   UNSIGNED16;
typedef unsigned long  UNSIGNED32;
typedef unsigned char  BOOLEAN;

#ifndef FALSE
#define FALSE   (1==0)
#endif
#ifndef TRUE
#define TRUE    (!FALSE)
#endif

#ifndef CONST
#define CONST const
#endif

/* Macros */

#define BIT(x)             (1 << (x))
#define SETBIT(addr,x)     ((addr) |= BIT(x))
#define CLEARBIT(addr,x)   ((addr) &= ~BIT(x))
#define BITSET(addr,x)     (((addr) & BIT(x)) == BIT(x))
#define BITCLEARED(addr,x) (((addr) & BIT(x)) == 0)


/*******************************************************************************
END OF FILE
*******************************************************************************/






