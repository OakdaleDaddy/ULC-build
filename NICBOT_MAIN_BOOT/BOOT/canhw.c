/**************************************************************************
PROJECT:     CANopen Bootloader for Atmel AT90CAN128 AVR Microcontroller
MODULE:      CANHW.C
CONTAINS:    CAN Interface Layer
ENVIRONMENT: Compiled and tested with IAR EW for AVR on
             Atmel STK500/501 with AT90CAN128
             Compiler Version 5.11b
COPYRIGHT:   This version Embedded Systems Academy, Inc.
             Developed by Embedded Systems Academy, Inc.
VERSION:     $LastChangedDate: 2016-02-26 20:53:15 +0100 (Fri, 26 Feb 2016) $
             $LastChangedRevision: 3576 $
***************************************************************************/

#include "types.h"
#include "mcu.h"
#include "userdefs.h"
#include "main.h"
#include "sdo.h"
#include "canhw.h"

/**************************************************************************
 LOCAL DEFINITIONS
***************************************************************************/

// Define the use of hardware message buffers in the CAN controller
#define CANHW_RECEIVE_MB  0  // Start message buffer number for SDO and NMT mailboxes (2 needed)
#define CANHW_TRANSMIT_MB 2  // Transmit message buffer


/**************************************************************************
 EXTERNAL REFERENCES
***************************************************************************/


/**************************************************************************
 PUBLIC DEFINITIONS
***************************************************************************/

CAN_MsgType gRxCAN;   // Receive CAN buffer
CAN_MsgType gTxCAN;   // Transmit CAN buffer


/**************************************************************************
 LOCAL FUNCTION PROTOTYPES
***************************************************************************/

static BOOLEAN CANHW_Pull_Message (
  void
  );


/**************************************************************************
 LOCAL FUNCTIONS
***************************************************************************/

/**************************************************************************
DOES:    Checks receive message objects in the CAN controller. If message
         was pending, reads the message into CAN receive buffer.
RETURNS: FALSE, if there was no message to receive
         TRUE, if a message was received
**************************************************************************/
static BOOLEAN CANHW_Pull_Message (
  void
  )
{
  BOOLEAN    msg_found = FALSE;
  UNSIGNED8  msg_buf   = CANHW_RECEIVE_MB;

  // All message buffers lower than the transmit message buffer are for reception
  while ((msg_buf < CANHW_TRANSMIT_MB) && (msg_found == FALSE))
  {
    CANPAGE = msg_buf << MOBNB;  // Let registers point to hardware message buffer

    // If there is message pending, receive
    if (BITSET(CANSTMOB,RXOK))
    {
      UNSIGNED16 identifier;
      UNSIGNED8  i;

      // Copy ID and length of message to application receive message buffer
      identifier   = (UNSIGNED16)CANIDT1;         // Get message id bits 3..10
      identifier <<= 3;
      identifier  |= ((UNSIGNED16)CANIDT2 >> 5);  // Or with message id bits 0..2
      gRxCAN.ID    = identifier;
      gRxCAN.LEN   = CANCDMOB & DLC_msk;  // Get message length (DLC)

      // Copy data bytes
      for ( i = 0; i < gRxCAN.LEN; i++ )
      {
        gRxCAN.BUF[i] = CANMSG;
      }

      // Release receive buffer -> Clear RXOK flag
      CLEARBIT(CANSTMOB,RXOK);

      // Enable message buffer for reception with an expected data length
      // of 8 bytes for the first (for SDO message) and 2 bytes for the
      // second buffer.
      if (msg_buf == CANHW_RECEIVE_MB)
      {
        CANCDMOB = (2 << CONMOB) | (8 << DLC);
      }
      else
      {
        CANCDMOB = (2 << CONMOB) | (2 << DLC);
      }

      msg_found = TRUE;
    }
    else
    {
      msg_buf++;  // Try next message buffer
    }
  }

  return (msg_found);
}




/**************************************************************************
 PUBLIC FUNCTIONS
***************************************************************************/


/**************************************************************************
DOES:    Transmit the message in the transmit buffer. After transmission,
         the function waits until the message was transmitted.
RETURNS: -
**************************************************************************/
void CANHW_Push_Message (
  void
  )
{
  UNSIGNED8  i = 0;

  CANPAGE = CANHW_TRANSMIT_MB << MOBNB;  // Using the hardware transmit message buffer

  // Disable Object
  CANCDMOB &= ~CONMOB_msk;
  // Clear TXOK Bit
  CLEARBIT(CANSTMOB,TXOK);

  // Set the Identifier
  CANIDT1 = (UNSIGNED8)(gTxCAN.ID >> 3);
  CANIDT2 = (UNSIGNED8)(gTxCAN.ID << 5);

  // Copy data bytes
  for ( i = 0; i < gTxCAN.LEN; i++ )
  {
    CANMSG = gTxCAN.BUF[i];
  }

  // Set length and
  // CAN controller transmission request
  CANCDMOB = 0x40 + gTxCAN.LEN;

  // wait until not busy
  while (BITCLEARED(CANSTMOB,6))
  { // do nothing
    ;
  }

  return;
}


/**************************************************************************
DOES:    Sends the CANopen bootup message
RETURNS:
**************************************************************************/
void CANHW_Send_Boot (
  void
  )
{
  // transmit boot up message
  gTxCAN.ID     = 0x700 + gNodeStatus.node_id;
  gTxCAN.LEN    = 1;
  gTxCAN.BUF[0] = 0x00;
  CANHW_Push_Message();

  return;
}

/**************************************************************************
DOES:    Sends the CANopen emergency message
RETURNS:
**************************************************************************/
void CANHW_Send_Emergency (
   UNSIGNED16 code,
   UNSIGNED8 em1,
   UNSIGNED8 em2,
   UNSIGNED8 em3,
   UNSIGNED8 em4,
   UNSIGNED8 em5
   )
{
   // transmit boot up message
   gTxCAN.ID     = 0x080 + gNodeStatus.node_id;
   gTxCAN.LEN    = 8;
   gTxCAN.BUF[0] = (code&0xFF);
   gTxCAN.BUF[1] = (code>>8);
   gTxCAN.BUF[2] = 1; // set general error, todo support error register
   gTxCAN.BUF[3] = em1;
   gTxCAN.BUF[4] = em2;
   gTxCAN.BUF[5] = em3;
   gTxCAN.BUF[6] = em4;
   gTxCAN.BUF[7] = em5;

   CANHW_Push_Message();

   return;
}

/**************************************************************************
DOES:    Sends the CANopen debug message
RETURNS:
**************************************************************************/
void CANHW_Send_Debug(
   UNSIGNED16 info
   )
{
	// transmit boot up message
	gTxCAN.ID     = 0x700 + gNodeStatus.node_id;
	gTxCAN.LEN    = 2;
	gTxCAN.BUF[0] = (info&0xFF);
	gTxCAN.BUF[1] = (info>>8);
	CANHW_Push_Message();

	return;
}



/**************************************************************************
DOES:    Configures the CAN Controller
         Sets the bus timing
         Sets up aceeptance filtering to receive SDO requests
         (ID 0x600 + Node ID) and NMT messages (ID 0x000)
PARAMETERS: -
RETURNS: -
**************************************************************************/
void CANHW_Init (
  void
  )
{
  UNSIGNED8 i = 0;

  // Reset CAN Controller
  SETBIT(CANGCON,SWRES);

  // Initialize and disable all msg_bufes
  {
    UNSIGNED8 num_channel;

    for (num_channel = 0; num_channel < 15; num_channel++)
    {
      CANPAGE  = (num_channel << MOBNB);
      CANSTMOB = 0;
      CANCDMOB = 0;
      CANIDT4  = 0;
      CANIDT3  = 0;
      CANIDT2  = 0;
      CANIDT1  = 0;
      CANIDM4  = 0;
      CANIDM3  = 0;
      CANIDM2  = 0;
      CANIDM1  = 0;
      for (i = 0; i < 8; i++)
      {
        CANMSG = 0;
      }
    }
  }

  // Load CAN Timer Prescaler: The timer is unused, but register has
  // to be written for a complete CAN initialization.
  CANTCON = 0xFF;

  // Select main or alternative baudrate depending on setting
  if (gNodeStatus.baudrate)
  {
    // Set BRP. Unused bits in CANBT1 are set to 0.
    CANBT1 = CAN_BRP_INIT1 << BRP;

    // Set SJW and PRS. Unused bits in CANBT2 are set to 0.
    CANBT2 = (CAN_SJW_INIT1 << SJW) | (CAN_PRJ_INIT1 << PRS);

    // Set PHS1 and PHS2. Unused bits in CANBT3 are set to 0.
    CANBT3 = (CAN_PHS2_INIT1 << PHS2) | (CAN_PHS1_INIT1 << PHS1);
  }
  else
  {
    // Set BRP. Unused bits in CANBT1 are set to 0.
    CANBT1 = CAN_BRP_INIT0 << BRP;

    // Set SJW and PRS. Unused bits in CANBT2 are set to 0.
    CANBT2 = (CAN_SJW_INIT0 << SJW) | (CAN_PRJ_INIT0 << PRS);

    // Set PHS1 and PHS2. Unused bits in CANBT3 are set to 0.
    CANBT3 = (CAN_PHS2_INIT0 << PHS2) | (CAN_PHS1_INIT0 << PHS1);
  }

  // Disable all interrupts - we use polling only
  CANGIE = 0x00;
  CANIE2 = 0x00;
  CANIE1 = 0x00;

  // Enable CAN Controller
  SETBIT(CANGCON,ENA);

  // Initialize first buffer for receive of SDO request messages
  // with the identifier 0x600 + CANopen Node ID
  CANPAGE  = CANHW_RECEIVE_MB << MOBNB;
  CANIDT1  = (UNSIGNED8)((UNSIGNED16)(0x600 + gNodeStatus.node_id) >> 3);
  CANIDT2  = (UNSIGNED8)((UNSIGNED16)(0x600 + gNodeStatus.node_id) << 5);
  // Only accept this ID
  CANIDM1  = 0xFF;
  CANIDM2  = 0xFF;
  // Enable message object for reception with an expected data length
  // of 8 bytes.
  CANCDMOB = (2 << CONMOB) | (8 << DLC);

  // Initialize second buffer for receive of NMT messages
  // with the identifier 0x000
  CANPAGE  = (CANHW_RECEIVE_MB+1) << MOBNB;
  CANIDT1  = (UNSIGNED8)(0x000 >> 3);
  CANIDT2  = (UNSIGNED8)(0x000 << 5);
  // Only accept this ID
  CANIDM1  = 0xFF;
  CANIDM2  = 0xFF;
  // Enable message object for reception with an expected data length
  // of 2 bytes.
  CANCDMOB = (2 << CONMOB) | (2 << DLC);

  // Wait for CAN controller to become enabled
  while (BITCLEARED(CANGSTA,ENFG))
  {
    ;
  }
  // configuration done

  // clear buffers
  for ( i = 0; i < 8; i++ )
  {
    gTxCAN.BUF[i] = 0x00;
    gRxCAN.BUF[i] = 0x00;
  }

  return;
}



/**************************************************************************
DOES:    Polls for new received CAN messages. If message has been received,
         process it.
RETURNS: TRUE, if a message was successfully processed, FALSE if not
**************************************************************************/
BOOLEAN CANHW_Process_Messages (
  void
  )
{
  BOOLEAN   return_val = FALSE;
  UNSIGNED8 result     = 0;

  // if msg object received ok then...
  if ( CANHW_Pull_Message() )
  {
    // NMT messages we process right here
    if (gRxCAN.ID == 0x000)               // NMT command (COB-ID=0)?
    {
      if ( (gRxCAN.LEN == 2)                          &&  // And two bytes long?
           ( (gRxCAN.BUF[0] == 129)   ||                  // And is NMT Reset Node...
             (gRxCAN.BUF[0] == 130) )                 &&  // ...or NMT Reset Communication?
           ( (gRxCAN.BUF[1] == 0)     ||                  // And to all nodes...
             (gRxCAN.BUF[1] == gNodeStatus.node_id) )     // ...or to our Node ID?
         )
      {
        // Run application from main()
        gNodeStatus.run_mode = EXECMODE_RUN;
        return_val = TRUE;
      }
      else
      { // Ignore all other NMT commands
        ;
      }
    }
    else
    { // The only other supported messages are SDO requests, so process those
      UNSIGNED16 index    = 0x0000U;
      UNSIGNED8  subindex = 0x00U;

      // A return value of 2 indicates an error and is reported to the error
      // list. The only other result is 1 (success) which we report back.
      result = SDO_Handle_SDO_Request(&index, &subindex);

      if (result == SDO_ACCESS_FAILED)
      { // Additional info is the index of the access
        MAIN_Signal_Error(ERRCODE_PROTOCOL, index);
      }
      else
      {
        return_val = TRUE;
      }
    }
  }
  else
  { // No message => Do nothing
    ;
  }

  return (return_val);
}


/***************************************************************************
END OF FILE
***************************************************************************/
