/**************************************************************************
PROJECT:   Packet Based Serial Protocol Handler
COPYRIGHT: Embedded Systems Academy, Inc. 2002-2016
           All rights reserved. www.microcanopen.com
DISCLAIM:  Read and understand our disclaimer before using this code!
           www.esacademy.com/disclaim.htm
           This software was written in accordance to the guidelines at
           www.esacademy.com/software/softwarestyleguide.pdf
LICENSE:   THIS IS THE COMMERCIAL VERSION OF MICROCANOPEN PLUS
           ONLY USERS WHO PURCHASED A LICENSE MAY USE THIS SOFTWARE
           See file license_commercial_plus.txt or
           www.microcanopen.com/license_commercial_plus.txt
VERSION:   6.21, ESA 16-JUN-16
           $LastChangedDate: 2016-02-16 22:14:13 +0100 (Tue, 16 Feb 2016) $
           $LastChangedRevision: 3568 $
***************************************************************************/


#include "mcop_xod_inc.h"

#if (USE_REMOTE_ACCESS == 1)

/**************************************************************************
 LOCAL DEFINITIONS
***************************************************************************/

// Timeout for the reception of the next byte in packet
#define SERIALPROTOCOL_NEXTBYTETIMEOUT    200   // 200 ms

// Packet type for send/receive
typedef struct {
  UNSIGNED8   dlc;                    // data length
  UNSIGNED8   data[MAX_PACKET_DATA];  // data buffer
} SerialProtocol_PacketType;

// packet buffer
typedef struct {
  SerialProtocol_PacketType *buffer;
  UNSIGNED16 size;
  SerialProtocol_PacketType *nextread, *nextwrite;
} SerialProtocol_PacketBuffer;

#define BUFFER_INCREAD(name)  if (++name.nextread >= (name.buffer + name.size)) name.nextread = name.buffer; 
#define BUFFER_INCWRITE(name) if (++name.nextwrite >= (name.buffer + name.size)) name.nextwrite = name.buffer; 
#define BUFFER_EMPTY(name)    (name.nextwrite == name.nextread)

// Parsing states for packet protocol reception
typedef enum {
  PARSESTATE_START,    // Start-of-header
  PARSESTATE_LENGTH,   // Length of data
  PARSESTATE_DATA,     // Data bytes
  PARSESTATE_CHECKL,   // Checksum low
  PARSESTATE_CHECKH    // Checksum high
} SerialProtocol_ParseStateMachineType;

// Current parsing state
typedef struct {
  SerialProtocol_ParseStateMachineType parse_state;
  UNSIGNED8     dlc;       // Number of data bytes in packet
  UNSIGNED8     num_bytes; // Number of received data bytes
  UNSIGNED16    checksum;  // Calculated checksum so far
  UNSIGNED16    crc;       // Received checksum
  UNSIGNED8     wait_next; // Wait for next byte
  UNSIGNED8     ignore;    // True if command is ignored
  UNSIGNED8     err;       // True if packet has to be NAK'd
} SerialProtocol_ParseStateType;

static SerialProtocol_ParseStateType mSerialProtocol_ParseState;


// States for packet protocol transmission
typedef enum {
  SENDSTATE_IDLE,     // Nothing to send
  SENDSTATE_START,    // Start-of-header
  SENDSTATE_LENGTH,   // Length of data
  SENDSTATE_DATA,     // Data bytes
  SENDSTATE_CHECKL,   // Checksum low
  SENDSTATE_CHECKH,   // Checksum high
  SENDSTATE_WAITACK   // Wait for acknowledge
} SerialProtocol_SendStateMachineType;

// Current sending state
typedef struct {
  SerialProtocol_SendStateMachineType send_state;
  UNSIGNED8     num_bytes; // Number of sent data bytes
  UNSIGNED16    checksum;  // Calculated checksum so far
} SerialProtocol_SendStateType;

static SerialProtocol_SendStateType mSerialProtocol_SendState;

// current error state
static UNSIGNED16 mSerialProtocol_errorstate = 0;
// receive byte timeout timestamp
static UNSIGNED16 mByteTimeout;


/**************************************************************************
 EXTERNAL REFERENCES
***************************************************************************/


/**************************************************************************
 PUBLIC DEFINITIONS
***************************************************************************/

// create transmit and receive buffers
static SerialProtocol_PacketType _rxbuffer[RX_BUFFER_SIZE];
static volatile SerialProtocol_PacketBuffer rxbuffer = {_rxbuffer, RX_BUFFER_SIZE, _rxbuffer, _rxbuffer};
static SerialProtocol_PacketType _txbuffer[TX_BUFFER_SIZE];
static volatile SerialProtocol_PacketBuffer txbuffer = {_txbuffer, TX_BUFFER_SIZE, _txbuffer, _txbuffer};

// Error counters for receive and transmit
static UNSIGNED16 SerialProtocol_ReceiveErrorCounter;
static UNSIGNED16 SerialProtocol_TransmitErrorCounter;


/**************************************************************************
 LOCAL FUNCTION PROTOTYPES
***************************************************************************/

static void SerialProtocol_ResetReceiveHandler(void);
static void SerialProtocol_ResetTransmitHandler(void);
static UNSIGNED8 SerialProtocol_ProcessReceive(void);
static UNSIGNED8 SerialProtocol_ProcessTransmit(void);


/**************************************************************************
 LOCAL FUNCTIONS
***************************************************************************/

/**************************************************************************
DOES:    Prepare protocol receive handler for new command
RETURNS: -
**************************************************************************/
static void SerialProtocol_ResetReceiveHandler (
  void
  )
{
  mSerialProtocol_ParseState.parse_state = PARSESTATE_START;  // no command yet
  mSerialProtocol_ParseState.dlc         = 0;                 // no bytes yet
  mSerialProtocol_ParseState.num_bytes   = 0;                 // no bytes yet
  mSerialProtocol_ParseState.checksum    = 0;                 // start check with 0
  mSerialProtocol_ParseState.wait_next   = FALSE;             // don't wait for next byte
  mSerialProtocol_ParseState.ignore      = FALSE;             // don't ignore
  mSerialProtocol_ParseState.err         = FALSE;             // no error yet

  return;
}


/**************************************************************************
DOES:    Prepare protocol transmit handler for new command
RETURNS: -
**************************************************************************/
static void SerialProtocol_ResetTransmitHandler (
  void
  )
{
  mSerialProtocol_SendState.send_state  = SENDSTATE_IDLE;   // no command yet
  mSerialProtocol_SendState.num_bytes   = 0;                // no bytes yet
  mSerialProtocol_SendState.checksum    = 0;                // start check with o

  return;
}


/**************************************************************************
DOES:    Processes received bytes. Handles synchronization packets
         autonomously.
RETURNS: TRUE, if a complete, correct command was received and
         'SerialProtocol_In' contains the command.
GLOBALS: SerialProtocol_In, SerialProtocol_ReceiveErrorCounter
**************************************************************************/
static UNSIGNED8 SerialProtocol_ProcessReceive (
  void
  )
{
  UNSIGNED8 return_val = FALSE;
  UNSIGNED8 byte;
  static UNSIGNED16 mycrc;

  // Only process receive packets if not currently sending
  if (mSerialProtocol_SendState.send_state == SENDSTATE_IDLE)
  { 
    // Get the next byte from the receive buffer, if available, and process.
    if (COM_GetByte(&byte))
    {
      // Arm the timer for receiving the next byte
      mByteTimeout = MCOHW_GetTime() + SERIALPROTOCOL_NEXTBYTETIMEOUT;

      switch ( mSerialProtocol_ParseState.parse_state )
      {
        // The first byte is the start-of-header
        case PARSESTATE_START:

          // Test if start-of-header was received
          if (byte == PROT_SOH)
          {
            // Initialize CRC calculator
            mycrc = CRC_Init();

            // Packet reception started - now monitor subsequent bytes
            mSerialProtocol_ParseState.wait_next   = TRUE;

            // Next byte is the length
            mSerialProtocol_ParseState.parse_state = PARSESTATE_LENGTH;
          }
          else
          { // otherwise ignore byte and stay in this state
            ;
          }
          break;


        // Get the length
        case PARSESTATE_LENGTH:

          // Add to CRC
          CRC_Add(byte, &mycrc);

          mSerialProtocol_ParseState.dlc = byte;  // Memorize number of data bytes

          if (byte > MAX_PACKET_DATA)
          { // If this packet is too long: Error
            mSerialProtocol_ParseState.err = TRUE;
          }
          if (byte == 0)
          { // No data: Next state is the low checksum bytes
            mSerialProtocol_ParseState.parse_state = PARSESTATE_CHECKL;
          }
          else
          { // Next state is the field of data bytes
            mSerialProtocol_ParseState.parse_state = PARSESTATE_DATA;
          }
          break;


        // Get data
        case PARSESTATE_DATA:

          // Add to CRC
          CRC_Add(byte, &mycrc);

          // Only receive data if no error, packet is for us, and not too long
          if ( !mSerialProtocol_ParseState.err       &&
               !mSerialProtocol_ParseState.ignore      &&
               (mSerialProtocol_ParseState.num_bytes < MAX_PACKET_DATA) )
          { // If possible, save data in buffer
            rxbuffer.nextwrite->data[mSerialProtocol_ParseState.num_bytes] = byte;
          }

          // Increment counter for received data bytes
          mSerialProtocol_ParseState.num_bytes++;

          if (mSerialProtocol_ParseState.num_bytes == mSerialProtocol_ParseState.dlc)
          { // All data bytes received: Now checksum
            mSerialProtocol_ParseState.parse_state  = PARSESTATE_CHECKL;
          }
          else
          { // Otherwise stay here
            mSerialProtocol_ParseState.parse_state  = PARSESTATE_DATA;
          }
          break;

        // Get checksum low byte
        case PARSESTATE_CHECKL:

          // Memorize low byte
          mSerialProtocol_ParseState.crc = (UNSIGNED16)byte;

          // Now get high byte of checksum
          mSerialProtocol_ParseState.parse_state  = PARSESTATE_CHECKH;
          break;


        // End-of-packet: Get checksum and compare with the calculated one
        case PARSESTATE_CHECKH:

          // If this packet is not for us, don't do anything (just keep on parsing)
          if (!mSerialProtocol_ParseState.ignore)
          {
            // Add high byte of received checksum
            mSerialProtocol_ParseState.crc |= (UNSIGNED16)byte << 8;

            if (mSerialProtocol_ParseState.crc != mycrc)
            {
              mSerialProtocol_ParseState.err = TRUE;
            }

            // if packet received correct then store in receive buffer
            if (!mSerialProtocol_ParseState.err)
            {
              rxbuffer.nextwrite->dlc = mSerialProtocol_ParseState.dlc;
              BUFFER_INCWRITE(rxbuffer);
              // if overflow then lose oldest packet
              if (BUFFER_EMPTY(rxbuffer))
              {
                BUFFER_INCREAD(rxbuffer);
              }
              return_val = TRUE;
            }
          }

          // Now we are ready for the next packet
          mSerialProtocol_ParseState.wait_next = FALSE;
          SerialProtocol_ResetReceiveHandler();
          break;

        default:  // never be here, reset
          mSerialProtocol_ParseState.wait_next = FALSE;
          SerialProtocol_ResetReceiveHandler();
          break;
      }
    }
    else
    {
      // If no byte has been received, monitor timeout if we are in-packet
      if ( mSerialProtocol_ParseState.wait_next &&
           MCOHW_IsTimeExpired(mByteTimeout)
         )
      {
        // In-packet bytes have to arrive in time. After timeout resort to
        // the ground state to be ready for the next packet reception.
        mSerialProtocol_ParseState.wait_next = FALSE;
        SerialProtocol_ResetReceiveHandler();
        SerialProtocol_ReceiveErrorCounter++;
      }
    }
  }

  return (return_val);
}


/**************************************************************************
DOES:    Processes packet transmission
RETURNS: TRUE, if an error occured during transmission
GLOBALS: SerialProtocol_Out, SerialProtocol_TransmitErrorCounter
**************************************************************************/
static UNSIGNED8 SerialProtocol_ProcessTransmit (
  void
  )
{
  UNSIGNED8 return_val = FALSE;
  static UNSIGNED16 crc;

  // Depending on the state, send the next byte from the transmit buffer.
  switch ( mSerialProtocol_SendState.send_state )
  {
    case SENDSTATE_IDLE:

      // If there is something to send, start
      if (!BUFFER_EMPTY(txbuffer))
      {
        mSerialProtocol_SendState.send_state = SENDSTATE_START;
      }
      break;

    // The first byte is the start-of-header
    case SENDSTATE_START:

      if (COM_SendByte(PROT_SOH))
      {
        // Initialize CRC calculator
        crc = CRC_Init();

        // Next byte is the length
        mSerialProtocol_SendState.send_state = SENDSTATE_LENGTH;
      }
      break;


    // Send the length
    case SENDSTATE_LENGTH:

      if (COM_SendByte(txbuffer.nextread->dlc))
      {
        // Add to CRC
        CRC_Add(txbuffer.nextread->dlc, &crc);

        if (txbuffer.nextread->dlc == 0)
        { // No data: Next state is the low checksum bytes
          mSerialProtocol_SendState.send_state = SENDSTATE_CHECKL;
        }
        else
        { // Next state is the field of data bytes
          mSerialProtocol_SendState.send_state = SENDSTATE_DATA;
        }
      }
      break;


    // Send data
    case SENDSTATE_DATA:

      if (COM_SendByte(txbuffer.nextread->data[mSerialProtocol_SendState.num_bytes]))
      {
        // Add to CRC
        CRC_Add(txbuffer.nextread->data[mSerialProtocol_SendState.num_bytes], &crc);

        mSerialProtocol_SendState.num_bytes++;

        if (mSerialProtocol_SendState.num_bytes == txbuffer.nextread->dlc)
        { // All data bytes sent: Now checksum
          mSerialProtocol_SendState.send_state  = SENDSTATE_CHECKL;
        }
        else
        { // Otherwise stay here
          mSerialProtocol_SendState.send_state  = SENDSTATE_DATA;
        }
      }
      break;

    // Send checksum low byte
    case SENDSTATE_CHECKL:

      mSerialProtocol_SendState.checksum = crc;

      // Send low byte
      if(COM_SendByte((UNSIGNED8)(mSerialProtocol_SendState.checksum & 0x00FFU)))
      {
        // Now send high byte of checksum
        mSerialProtocol_SendState.send_state  = SENDSTATE_CHECKH;
      }
      break;


    // End-of-packet: Send high byte of checksum
    case SENDSTATE_CHECKH:

      // Send high byte
      if(COM_SendByte((UNSIGNED8)(mSerialProtocol_SendState.checksum >> 8)))
      {
        // finished sending packet
        // Now we are ready for the next packet
        SerialProtocol_ResetTransmitHandler();
        BUFFER_INCREAD(txbuffer);
      }
      break;

    default:  // never be here, if we get here by error, reset state machine
      SerialProtocol_ResetTransmitHandler();
      break;
  }

  return (return_val);
}


/**************************************************************************
 PUBLIC FUNCTIONS
***************************************************************************/

/**************************************************************************
DOES:    Initialization of protocol handler
RETURNS: -
GLOBALS: SerialProtocol_In, SerialProtocol_Out,
         SerialProtocol_ReceiveErrorCounter, SerialProtocol_TransmitErrorCounter
**************************************************************************/
void SerialProtocol_Init (
  void
  )
{
  // Initialize UART used for communication
  COM_Init();

  SerialProtocol_ResetReceiveHandler();
  SerialProtocol_ReceiveErrorCounter  = 0;
  rxbuffer.nextread = rxbuffer.nextwrite = rxbuffer.buffer;

  SerialProtocol_ResetTransmitHandler();
  SerialProtocol_TransmitErrorCounter = 0;
  txbuffer.nextread = txbuffer.nextwrite = txbuffer.buffer;
}


/**************************************************************************
DOES:    Checks if any communication errors occured and returns the
         error status. Only TRUE/FALSE value for the global UART error
         status. This could be made more specific if needed at the host.
         Clears error flags and counters.
RETURNS: Current error flags (SERIALPROTOCOL_ERROR_xxx)
GLOBALS: SerialProtocol_ReceiveErrorCounter,
         SerialProtocol_TransmitErrorCounter
**************************************************************************/
UNSIGNED16 SerialProtocol_CheckError (
  void
  )
{
  if ((SerialProtocol_ReceiveErrorCounter  > 0)  ||
      (SerialProtocol_TransmitErrorCounter > 0)  ||
      (COM_GetErrorStatus() != 0)
     )
  {
    mSerialProtocol_errorstate |= SERIALPROTOCOL_ERROR_UART;
  }
  else
  {
    mSerialProtocol_errorstate &= ~SERIALPROTOCOL_ERROR_UART;
  }

  // clear error counters
  SerialProtocol_ReceiveErrorCounter  = 0;
  SerialProtocol_TransmitErrorCounter = 0;
    
  return mSerialProtocol_errorstate;
}

/**************************************************************************
DOES:    Queues a message for transmission. Gathers data from two memory
         areas and stores sequentially in buffer.
RETURNS: TRUE if message was queued, FALSE if there was an error
**************************************************************************/
UNSIGNED8 SerialProtocol_PushMessage (
  UNSIGNED8 length1, // length of data chunk 1
  UNSIGNED8 *pTx1,   // pointer to data chunk 1
  UNSIGNED8 length2, // length of data chunk 2
  UNSIGNED8 *pTx2    // pointer to data chunk 2
  )
{
  int b;

  // store data in transmit buffer - it will be transmitted by the
  // state machine when all currently buffered messages have been
  // transmitted
  txbuffer.nextwrite->dlc = length1 + length2;
  for (b = 0; b < length1; b++)
  {
    txbuffer.nextwrite->data[b] = *(pTx1 + b);
  }
  for (b = 0; b < length2; b++)
  {
    txbuffer.nextwrite->data[b + length1] = *(pTx2 + b);
  }
  BUFFER_INCWRITE(txbuffer);
  // if overflow lose oldest packet
  if (BUFFER_EMPTY(txbuffer))
  {
    BUFFER_INCREAD(txbuffer);
  }

  return TRUE;
}

volatile UNSIGNED16 num = 0;

/**************************************************************************
DOES:    Gets the next message from the receive buffer
RETURNS: Number of data bytes copied or zero if no messages
**************************************************************************/
UNSIGNED8 SerialProtocol_PullMessage (
  UNSIGNED8 *pRx    // pointer to location to store data
  )
{
  int b;
  UNSIGNED8 length = 0;

  if (!BUFFER_EMPTY(rxbuffer))
  {
    length = rxbuffer.nextread->dlc;
    for (b = 0; b < length; b++)
    {
      *(pRx + b) = rxbuffer.nextread->data[b];
    }
    BUFFER_INCREAD(rxbuffer);

    if (pRx[0] == 'W')
    {
      num = ((UNSIGNED16)pRx[5] << 8) | pRx[4];
    }
  }

  return length;
}


/**************************************************************************
DOES:    Executes state machines - must be called periodically
RETURNS: Nothing
**************************************************************************/
void SerialProtocol_Process (
  void
  )
{
  SerialProtocol_ProcessReceive();
  SerialProtocol_ProcessTransmit();
}


/**************************************************************************
DOES:    Transmits all packets in transmit buffer
RETURNS: Nothing
**************************************************************************/
void SerialProtocol_CompleteTransmits (
  void
  )
{
  while (!BUFFER_EMPTY(txbuffer))
  {
    SerialProtocol_ProcessTransmit();
  }

  COM_Flush();
}

#endif // USE_REMOTE_ACCESS

/***************************************************************************
END OF FILE
***************************************************************************/
