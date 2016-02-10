

#include "datatypes.h"
#include "serial.h"
#include "ser_user.h"
#include "hardware_user.h"

// size for TX Fifo
#define  SER_TX_FIFO_SIZE     128

// size for RX Fifo
#define  SER_RX_FIFO_SIZE     60


// TX Fifo (soft-fifo read by TX complete interrupt)
static u8_t  TxFifo[SER_TX_FIFO_SIZE];

// RX Fifo (soft-fifo write by RX complete interrupt)
static u8_t  RxFifo[SER_RX_FIFO_SIZE];

/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
u32_t serialInit(u32_t baud, u8_t dataBits, u8_t stopBits, u8_t parityCode)
{
   u32_t result;
   u8_t parity;
   SERInit_t setup;

   result = 0;   
   
   if ( (1 == parityCode) )
   {
      parity = SER_PARITY_ODD;
   }
   else if ( (2 == parityCode) )
   {
      parity = SER_PARITY_EVEN;
   }
   else 
   {
      parity = SER_PARITY_NONE;
   }
	
   setup.prescaler = ( HW_CPU_CLOCK_HZ + 8 * baud) / ( 16 * baud);
   setup.databits = dataBits;
   setup.stopbits = stopBits;
   setup.parity = parity;
	
	setup.pTxFifo = &TxFifo;
	setup.pRxFifo = &RxFifo;
	
	setup.TxFifoSize = SER_TX_FIFO_SIZE;
	setup.RxFifoSize = SER_RX_FIFO_SIZE;
	
	setup.ISRnum = 5;		// VIC channels 0 to 4 used for CAN
	
   result = SER_Initialize(SER_PORT1, &setup);   

   return(result);
}

u32_t serialWrite(u8_t * data, u8_t length)
{
   u32_t result;

   result = SER_Write(SER_PORT1, data, length);

   return(result);   
}

u32_t serialRead(u8_t * data)
{
   u32_t result;
   u8_t bytesRead;

   result = SER_Read(SER_PORT1, data, 1, &bytesRead);

   if ( (0 == result) )
   {
      result = bytesRead;      
   }
   else
   {
      result = 0;
   }

   return(result);
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */

