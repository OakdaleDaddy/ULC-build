//_____  I N C L U D E S _______________________________________________________
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
// #include <util/delay.h> // causes integer overflow, 
#include <util/delay_basic.h>
#include "can_access.h"
#include "can_callbacks.h"

static void waitStart(unsigned short milliSeconds);

//------------------------------------------------------------------------------
static void waitStart(unsigned short milliSeconds)
{
    unsigned short i, j;

    for (i=0; i<milliSeconds; i += 1000)
    {
        PORTC ^= 0x01;

        for (j=0; j<4000; j++)
        {
            _delay_loop_2(1000);
        }
    }        
}

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

//------------------------------------------------------------------------------
int main(void)
{   
    DDRD |=0b00000000;
	PORTD |= 0b00000011;
	DDRC = 0xF3;
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
    
    // allow time for boot loading of other devices on the bus
    //waitStart(3000);

    // Initialize CAN Communication
    CAN_ResetCommunication();

    // enable interrupts
    sei();
    
    // process
    for (;;)
    {
	   // Update CAN Stack
	   CAN_ProcessStack();
    }

#if 0
    // initialize protocol
    can_isp_protocol_init();
	

    // process
    for (;;)
    {
        can_isp_protocol_task();
    }    
#endif    
	
    // End of "main"
    return(0);
}
