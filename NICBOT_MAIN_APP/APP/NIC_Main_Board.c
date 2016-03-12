//_____  I N C L U D E S _______________________________________________________

#include <string.h>

#include "config.h"
#include "can_isp_protocol.h"
#include "isp_lib.h"
#include "reduced_can_lib.h"
#include "can_drv.h"
#include "flash_boot_lib.h"
#include "flash_api_lib.h"
#include "eeprom_lib.h"
#include "ServoControl.h"
#include "CommonFunctions.h"
#include "LED_Control.h"
#include "can_access.h"
#include "can_callbacks.h"

static char * softwareVersion = "v1.02 abc def";

//------------------------------------------------------------------------------
/* See can_callbacks.h for function description */
U32 CAN_GetSerial(void)
{
	return(0);
}

/* See can_callbacks.h for function description */
void CAN_ResetApplication(void)
{
   WDTCR = (1<<WDCE)|(1<<WDE);  // Shortest period to timeout, Watchdog enabled
   for ( ;; ) ;
}

/* See can_callbacks.h for function description */
void CAN_NMTChange(U8 state)
{
}

/* See can_callbacks.h for function description */
U8 CAN_AppSDOReadInit(U8 server, U16 index, U8 subIndex, U32 * totalSize, U32 * size, U8 ** pData)
{   
   U8 result;

   // Unused Parameters
   (void)server;
   (void)subIndex;

   // initialize
   result = 0; 

   // evaluate
   if ( (0x100A == index) )   
   {
      *size = *totalSize = strlen(softwareVersion);
      *pData = (U8 *)softwareVersion;      
      result = 1;
   }

   return(result);
}

/* See can_callbacks.h for function description */
void CAN_AppSDOReadComplete(U8 server, U16 index, U8 subIndex, U32 * size)
{
   *size = 0;
}


//------------------------------------------------------------------------------
int main(void)
{   
   DDRD |=0b00000000;
	PORTD |= 0b00000011;
	DDRC = 0xF0;
	PORTC = 0x00;
		
	DDRA = 0xff;
	PORTA = 0x00;

	DDRG = 0b00011111;
	PORTG = 0b00000011;
	IO_Write(0,0x00); 
	IO_Write(1,0x00);
	DDRD |=0b00000011;
	PORTD &= 0b11111100;
	DDRB |= 0b00100001;
	DDRE |= 0b00111110;
	PORTE |= 0b00000100;
	Init_I2C(); // todo move to setPreOperationalState
   DrillInit(); // todo move to setPreOperationalState
	servo_hard_reset();

    //! --- First of all, disabling the Global Interrupt
    Disable_interrupt();

    //- Pull-up on TxCAN & RxCAN one by one to use bit-addressing
    CAN_PORT_DIR &= ~(1<<CAN_INPUT_PIN );
    CAN_PORT_DIR &= ~(1<<CAN_OUTPUT_PIN);
    CAN_PORT_OUT |=  (1<<CAN_INPUT_PIN );
    CAN_PORT_OUT |=  (1<<CAN_OUTPUT_PIN);
    
    // Initialize CAN Communication
    CAN_ResetCommunication();

    // enable interrupts
    Enable_interrupt();
    
    // process
    for (;;)
    {
	   // Update CAN Stack
	   CAN_ProcessStack();
    }
	
    // End of "main"
    return(0);
}
