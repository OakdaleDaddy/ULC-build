/**************************************************************************
PROJECT:     CANopen Bootloader for Atmel AT90CAN128 AVR Microcontroller
MODULE:      CANHW.H
CONTAINS:    Include file for CAN Interface Layer
ENVIRONMENT: Compiled and tested with IAR EW for AVR on
             Atmel STK500/501 with AT90CAN128
             Compiler Version 5.11b
COPYRIGHT:   This version Embedded Systems Academy, Inc.
             Developed by Embedded Systems Academy, Inc.
VERSION:     $LastChangedDate: 2013-05-28 02:11:48 +0200 (Tue, 28 May 2013) $
             $LastChangedRevision: 2624 $
***************************************************************************/

/***************************************************************************
 DEFINITIONS AND TYPES
***************************************************************************/

// Data structure for a CAN message
typedef struct
{
    UNSIGNED16 ID;      // Identifier
    UNSIGNED8  LEN;     // Data length (0-8)
    UNSIGNED8  BUF[8];  // Data buffer
} CAN_MsgType;


/***************************************************************************
 EXPORTED GLOBALS
***************************************************************************/

extern CAN_MsgType gRxCAN;   // Receive CAN buffer
extern CAN_MsgType gTxCAN;   // Transmit CAN buffer


/***************************************************************************
 EXPORTED FUNCTIONS
***************************************************************************/

/**************************************************************************
DOES:    Transmit the message in the transmit buffer. After transmission,
         the function waits until the message was transmitted.
RETURNS: -
**************************************************************************/
extern void CANHW_Push_Message (
  void
  );

/**************************************************************************
DOES:    Sends the CANopen bootup message
RETURNS:
**************************************************************************/
extern void CANHW_Send_Boot (
  void
  );
  
/**************************************************************************
DOES:    Sends the CANopen emergency message
RETURNS:
**************************************************************************/
extern void CANHW_Send_Emergency (
   UNSIGNED16 code,
   UNSIGNED8 em1,
   UNSIGNED8 em2,
   UNSIGNED8 em3,
   UNSIGNED8 em4,
   UNSIGNED8 em5
   );

/**************************************************************************
DOES:    Sends the CANopen debug message
RETURNS:
**************************************************************************/
extern void CANHW_Send_Debug (
  UNSIGNED16 info
  );

/**************************************************************************
DOES:    Configures the CAN Controller
         Sets the bus timing
         Sets up aceeptance filtering to receive SDO requests
         (ID 0x600 + Node ID) and NMT messages (ID 0x000)
PARAMETERS: -
RETURNS: -
**************************************************************************/
extern void CANHW_Init (
  void
  );

/**************************************************************************
DOES:    Polls for new received CAN messages. If message has been received,
         process it.
RETURNS: TRUE, if a message was successfully processed, FALSE if not
**************************************************************************/
extern BOOLEAN CANHW_Process_Messages (
  void
  );


/***************************************************************************
END OF FILE
***************************************************************************/
