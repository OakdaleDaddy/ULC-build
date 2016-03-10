/**************************************************************************
PROJECT:     CANopen Bootloader for Atmel AT90CAN128 AVR Microcontroller
MODULE:      USERDEFS.H
CONTAINS:    Customization and settings file for CANopen Bootloader
ENVIRONMENT: Compiled and tested with IAR EW for AVR on
             Atmel STK500/501 with AT90CAN128
             Compiler Version 5.11b
COPYRIGHT:   This version Embedded Systems Academy, Inc.
             Developed by Embedded Systems Academy, Inc.
VERSION:     $LastChangedDate: 2013-05-28 02:11:48 +0200 (Tue, 28 May 2013) $
             $LastChangedRevision: 2624 $
***************************************************************************/


// GENERAL PARAMETERS
// ==================

// Default Node ID, used if "BL_EEP_NID" is invalid (see below)
#define NodeID_INIT         0x20U

// Address of the reset vector (application start address)
#define RESVEC_ADR          0x00000000UL

// Main waiting timeout in seconds, if bootloader was started from application
#define WAIT_TIMEOUT        5

// Maximum number of errors to record in object 1003h (Pre-defined error field)
#define NUM_ERRORS_MAX      10


// CODE SPACE PARAMETERS
// =====================

// Addresses range for application checksum calculation. These are byte addresses
// into the (word organized) program memory.
#define AP_CHECKSUM_START     0x00000000UL
#define AP_CHECKSUM_END       0x0001DFFFUL

// Address location (byte address) of the 16-bit application checksum
#define AP_CHECKSUM_ADR       0x0001DFFEUL

// Startaddress (byte address) for the bootloader code
#define BL_START_ADR          0x0001E000UL

// Hardware (64k) bank of the bootloader code
#define BL_BANK               ((UNSIGNED8)(BL_START_ADR >> 16))

// Addresses range for bootloader checksum calculation. These are byte addresses
// into the (word organized) program memory.
#define BL_CHECKSUM_START     BL_START_ADR
#define BL_CHECKSUM_END       0x0001FFFFUL

// Address location (byte address) of the 16-bit bootloader checksum
#define BL_CHECKSUM_ADR       0x0001FFFEUL

// Addresses range for reading code flash via object [1F51h,1]. These
// are byte addresses into the (word organized) program memory.
#define ADR_READ_START        0x00000000UL
#define ADR_READ_END          0x0001FFFFUL


// EEPROM PARAMETERS
// =================

// Startaddress of Bootloader parameters in EEPROM space
#define EEPROM_START        0x0F01U

// Address offsets into the EEPROM area of the various parameters

// Node ID, UNSIGNED8, =0 or >127: NodeID_INIT, otherwise NodeID value
#define BL_EEP_NID         0

// Baudrate option, UNSIGNED8: If =125 alternate baudrate, otherwise default
#define BL_EEP_BAUD        1

// Serial Number (OD entry [1018h,4]), UNSIGNED32
#define BL_EEP_OBJ_1018_4  2

// CRC8 Checksum, UNSIGNED8
#define BL_EEP_CHECK       6



// CONSTANTS
// =========

// Device Type (OD entry [1000h,0]), UNSIGNED32
#define BL_OBJ_1000_0    0x00000000UL

// Vendor ID (OD entry [1018h,1]), UNSIGNED32
#define BL_OBJ_1018_1    0x00000000UL

// Product Code (OD entry [1018h,2]), UNSIGNED32
#define BL_OBJ_1018_2    0x00000000UL

// Revision Number (OD entry [1018h,3]), UNSIGNED32
#define BL_OBJ_1018_3    0x00000000UL



// CAN SETTINGS
// ============

// CAN Baudrate Register Values

// Values defined for 8 Mhz
//  BRP SJW PRJ PHS2 PHS1
//  19   0   2    1    1   50k bit/s
//   7   0   2    1    1   125k bit/s
//   3   0   2    1    1   250k bit/s
//   1   0   2    1    1   500k bit/s
//   0   0   2    1    1   1000k bit/s
// Values defined for 12 Mhz
//  BRP SJW PRJ PHS2 PHS1
//  29   0   2    1    1   50k bit/s
//  11   0   2    1    1   125k bit/s
//   5   0   2    1    1   250k bit/s
//   3   0   2    1    1   500k bit/s
// Values defined for 16 Mhz
//  BRP SJW PRJ PHS2 PHS1
//  39   0   2    1    1   50k bit/s
//  15   0   2    1    1   125k bit/s
//   7   0   2    1    1   250k bit/s
//   3   0   2    1    1   500k bit/s
//   1   0   2    1    1   1000k bit/s

// Default baudrate parameters
// Set to 50kbps
#define CAN_BRP_INIT0  39
#define CAN_SJW_INIT0  0
#define CAN_PRJ_INIT0  2
#define CAN_PHS2_INIT0 1
#define CAN_PHS1_INIT0 1

// Alternate baudrate parameters
// Set to 125kbps
#define CAN_BRP_INIT1  15
#define CAN_SJW_INIT1  0
#define CAN_PRJ_INIT1  2
#define CAN_PHS2_INIT1 1
#define CAN_PHS1_INIT1 1



// TIMER SETTINGS
// ==============

// Timer clock = system clock/1024
// Set OC0A on compare match
// Clear Timer on Compare Mode

// Timings for 16 MHz CPU clock speed:
//   1 ms cycle with OCR0 = 16
//   2 ms cycle with OCR0 = 31
//   5 ms cycle with OCR0 = 78
#define OCR0_INIT  78

// Timer tick in milliseconds
#define TIMERTICK_MS   5

/*******************************************************************************
END OF FILE
*******************************************************************************/
